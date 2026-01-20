using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI要素に手のレイがホバーしている間、
/// 対応するDistanceHandGrabInteractorを無効化するスクリプト
/// </summary>
public class UIButtonGrabDisabler : MonoBehaviour
{
    [Header("Distance Hand Grab Interactors")]
    [SerializeField] private DistanceHandGrabInteractor leftHandDistanceGrab;
    [SerializeField] private DistanceHandGrabInteractor rightHandDistanceGrab;

    [Header("Hand Ray Interactors")]
    [SerializeField] private RayInteractor leftHandRayInteractor;
    [SerializeField] private RayInteractor rightHandRayInteractor;

    [Header("UI Elements")]
    [Tooltip("判定対象のUI要素（ボタン、ScrollViewなど）のRectTransformリスト")]
    [SerializeField] private List<RectTransform> uiElements = new List<RectTransform>();

    [Header("Settings")]
    [SerializeField] private float maxRayDistance = 10f;

    private bool isLeftHandHovering = false;
    private bool isRightHandHovering = false;

    private void Update()
    {
        // 左手のレイチェック
        bool leftHovering = CheckRayHoverOnElements(leftHandRayInteractor);
        if (leftHovering != isLeftHandHovering)
        {
            isLeftHandHovering = leftHovering;
            if (leftHandDistanceGrab != null)
            {
                leftHandDistanceGrab.enabled = !isLeftHandHovering;
                Debug.Log($"[UIButtonGrabDisabler] Left Hand DistanceHandGrabInteractor {(isLeftHandHovering ? "disabled" : "enabled")}");
            }
        }

        // 右手のレイチェック
        bool rightHovering = CheckRayHoverOnElements(rightHandRayInteractor);
        if (rightHovering != isRightHandHovering)
        {
            isRightHandHovering = rightHovering;
            if (rightHandDistanceGrab != null)
            {
                rightHandDistanceGrab.enabled = !isRightHandHovering;
                Debug.Log($"[UIButtonGrabDisabler] Right Hand DistanceHandGrabInteractor {(isRightHandHovering ? "disabled" : "enabled")}");
            }
        }
    }

    private bool CheckRayHoverOnElements(RayInteractor rayInteractor)
    {
        if (rayInteractor == null) return false;

        Ray ray = rayInteractor.Ray;

        foreach (var rect in uiElements)
        {
            if (rect == null) continue;
            if (!rect.gameObject.activeInHierarchy) continue;

            if (IsRayHittingRect(ray, rect))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsRayHittingRect(Ray ray, RectTransform rectTransform)
    {
        // RectTransformのワールド座標での4隅を取得
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        // 4隅から平面を作成（corners: 0=左下, 1=左上, 2=右上, 3=右下）
        Vector3 bottomLeft = corners[0];
        Vector3 topLeft = corners[1];
        Vector3 topRight = corners[2];

        // 平面の法線を計算
        Vector3 right = topRight - topLeft;
        Vector3 up = topLeft - bottomLeft;
        Vector3 normal = Vector3.Cross(right, up).normalized;

        // 平面との交点を計算
        Plane plane = new Plane(normal, bottomLeft);

        float enter;
        if (!plane.Raycast(ray, out enter))
        {
            // レイが平面の裏側から来ている場合も考慮
            plane = new Plane(-normal, bottomLeft);
            if (!plane.Raycast(ray, out enter))
            {
                return false;
            }
        }

        if (enter > maxRayDistance || enter < 0) return false;

        // 交点を計算
        Vector3 hitPoint = ray.GetPoint(enter);

        // 交点がRectの範囲内かチェック
        return IsPointInRect(hitPoint, corners);
    }

    private bool IsPointInRect(Vector3 point, Vector3[] corners)
    {
        // corners: 0=左下, 1=左上, 2=右上, 3=右下
        Vector3 bottomLeft = corners[0];
        Vector3 topLeft = corners[1];
        Vector3 bottomRight = corners[3];

        Vector3 right = (bottomRight - bottomLeft);
        Vector3 up = (topLeft - bottomLeft);

        float rightLength = right.magnitude;
        float upLength = up.magnitude;

        if (rightLength < 0.0001f || upLength < 0.0001f) return false;

        right /= rightLength;
        up /= upLength;

        Vector3 localPoint = point - bottomLeft;

        float projRight = Vector3.Dot(localPoint, right);
        float projUp = Vector3.Dot(localPoint, up);

        return projRight >= 0 && projRight <= rightLength &&
               projUp >= 0 && projUp <= upLength;
    }
}
