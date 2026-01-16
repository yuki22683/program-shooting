using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target; // インスペクターで設定するターゲットのTransform

    private void Update()
    {
        if (target != null)
        {
            // 自オブジェクトの位置をターゲットの位置に追従させる
            transform.position = target.position;
            // 自オブジェクトの回転をターゲットの回転に追従させる
            transform.rotation = target.rotation;
        }
    }
}