using UnityEngine;
using System.Collections;

/// <summary>
/// Positions the attached object in front of the VR headset when enabled.
/// Useful for UI panels that should appear in front of the user.
/// </summary>
public class PositionInFrontOfHeadset : MonoBehaviour
{
    [Header("Position Settings")]
    [Tooltip("Distance in meters from the headset")]
    [SerializeField] private float distanceFromHeadset = 1.0f;

    [Tooltip("Height offset from headset (0 = same height as headset)")]
    [SerializeField] private float heightOffset = 0f;

    [Header("Orientation")]
    [Tooltip("If true, the panel will face the headset")]
    [SerializeField] private bool faceHeadset = true;

    [Tooltip("If true, only use horizontal rotation (ignore pitch)")]
    [SerializeField] private bool horizontalOnly = true;

    [Header("Timing")]
    [Tooltip("Delay before positioning (helps ensure camera is ready)")]
    [SerializeField] private float positionDelay = 1.0f;

    [Tooltip("Maximum attempts to find camera")]
    [SerializeField] private int maxRetries = 30;

    private Coroutine positionCoroutine;
    private bool skipNextReposition = false;

    private void OnEnable()
    {
        Debug.Log($"[PositionInFrontOfHeadset] OnEnable called on {gameObject.name}");

        // Check if repositioning should be skipped
        if (skipNextReposition)
        {
            Debug.Log($"[PositionInFrontOfHeadset] Skipping reposition for {gameObject.name}");
            skipNextReposition = false;
            return;
        }

        // Use coroutine for more robust camera detection
        if (positionCoroutine != null)
        {
            StopCoroutine(positionCoroutine);
        }
        positionCoroutine = StartCoroutine(PositionWithRetry());
    }

    /// <summary>
    /// Call this before SetActive(true) to skip the automatic repositioning once
    /// </summary>
    public void SkipNextReposition()
    {
        skipNextReposition = true;
        Debug.Log($"[PositionInFrontOfHeadset] Will skip next reposition for {gameObject.name}");
    }

    private void OnDisable()
    {
        if (positionCoroutine != null)
        {
            StopCoroutine(positionCoroutine);
            positionCoroutine = null;
        }
    }

    private IEnumerator PositionWithRetry()
    {
        // Initial delay to let VR system initialize
        yield return new WaitForSeconds(positionDelay);

        int attempts = 0;
        Vector3 lastCameraPos = Vector3.zero;
        int stableFrames = 0;
        const int requiredStableFrames = 3; // Wait for camera to stabilize

        while (attempts < maxRetries)
        {
            Transform headset = GetHeadsetTransform();
            if (headset != null)
            {
                // Wait for camera position to stabilize (not changing rapidly)
                Vector3 currentPos = headset.position;

                // Check if position has stabilized (within 1cm of last position)
                if (Vector3.Distance(currentPos, lastCameraPos) < 0.01f)
                {
                    stableFrames++;
                    if (stableFrames >= requiredStableFrames)
                    {
                        PositionPanelInternal(headset);
                        yield break;
                    }
                }
                else
                {
                    stableFrames = 0;
                    lastCameraPos = currentPos;
                }
            }

            attempts++;
            if (attempts % 5 == 0)
            {
                Debug.Log($"[PositionInFrontOfHeadset] Waiting for camera to stabilize, attempt {attempts}/{maxRetries}, pos: {lastCameraPos}");
            }
            yield return new WaitForSeconds(0.1f);
        }

        // Last resort: position anyway if camera exists
        Transform finalHeadset = GetHeadsetTransform();
        if (finalHeadset != null)
        {
            Debug.LogWarning($"[PositionInFrontOfHeadset] Camera didn't stabilize, positioning anyway at {finalHeadset.position}");
            PositionPanelInternal(finalHeadset);
        }
        else
        {
            Debug.LogWarning($"[PositionInFrontOfHeadset] Failed to find camera after {maxRetries} attempts");
        }
    }

    private Transform GetHeadsetTransform()
    {
        // Try Camera.main first
        if (Camera.main != null)
        {
            return Camera.main.transform;
        }

        // Try to find OVRCameraRig's CenterEyeAnchor
        var ovrRig = FindObjectOfType<OVRCameraRig>();
        if (ovrRig != null && ovrRig.centerEyeAnchor != null)
        {
            return ovrRig.centerEyeAnchor;
        }

        // Try to find any camera tagged MainCamera
        var mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCam != null)
        {
            return mainCam.transform;
        }

        return null;
    }

    /// <summary>
    /// Positions the panel in front of the headset
    /// </summary>
    public void PositionPanel()
    {
        Transform headset = GetHeadsetTransform();
        if (headset == null)
        {
            Debug.LogWarning("[PositionInFrontOfHeadset] Camera not found, starting retry coroutine");
            if (positionCoroutine != null)
            {
                StopCoroutine(positionCoroutine);
            }
            positionCoroutine = StartCoroutine(PositionWithRetry());
            return;
        }

        PositionPanelInternal(headset);
    }

    private void PositionPanelInternal(Transform headset)
    {
        // Calculate forward direction
        Vector3 forward = headset.forward;
        if (horizontalOnly)
        {
            // Project forward direction onto horizontal plane
            forward.y = 0;
            forward.Normalize();

            // Fallback if looking straight up/down
            if (forward.sqrMagnitude < 0.001f)
            {
                forward = Vector3.forward;
            }
        }

        // Calculate position: headset position + forward * distance + height offset
        Vector3 targetPosition = headset.position + forward * distanceFromHeadset;
        targetPosition.y = headset.position.y + heightOffset;

        transform.position = targetPosition;

        // Set rotation to face the headset
        if (faceHeadset)
        {
            Vector3 lookDirection = headset.position - transform.position;
            if (horizontalOnly)
            {
                lookDirection.y = 0;
            }

            if (lookDirection.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(-lookDirection);
            }
        }

        Debug.Log($"[PositionInFrontOfHeadset] Positioned {gameObject.name} at {targetPosition}, {distanceFromHeadset}m in front of headset (camera: {headset.name})");
    }

    /// <summary>
    /// Manually trigger repositioning
    /// </summary>
    public void Reposition()
    {
        PositionPanel();
    }

    /// <summary>
    /// Configure distance from headset
    /// </summary>
    public void SetDistance(float distance)
    {
        distanceFromHeadset = distance;
    }

    /// <summary>
    /// Configure height offset
    /// </summary>
    public void SetHeightOffset(float offset)
    {
        heightOffset = offset;
    }

    /// <summary>
    /// Configure all settings at once
    /// </summary>
    public void Configure(float distance, float height = 0f, bool face = true, bool horizontalOnlyRotation = true)
    {
        distanceFromHeadset = distance;
        heightOffset = height;
        faceHeadset = face;
        horizontalOnly = horizontalOnlyRotation;
    }
}
