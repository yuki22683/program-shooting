using System.Collections;
using Oculus.Interaction;
using UnityEngine;

public class BlockShooter : MonoBehaviour
{
    [SerializeField] private RayInteractable rayInteractable;
    [SerializeField] private float moveSpeed = 0.5f;

    [Header("Ray Visual Color")]
    [SerializeField] private Color hoverRayColor = new Color(1f, 0f, 0f, 0.9f); // Red

    private bool isDestroying;

    // Ray visual caching
    private static ControllerRayVisual[] cachedRayVisuals;
    private static Color[] originalRayHoverColor0;
    private static Color[] originalRayHoverColor1;

    // Cursor visual caching
    private static RayInteractorCursorVisual[] cachedCursorVisuals;
    private static Color[] originalCursorHoverColor;

    private static bool colorsInitialized;

    /// <summary>
    /// The order index of this block in the code sequence
    /// </summary>
    public int OrderIndex { get; set; } = -1;

    /// <summary>
    /// The token text displayed on this block
    /// </summary>
    public string TokenText { get; set; }

    /// <summary>
    /// Reference to the LessonManager for order validation
    /// </summary>
    public LessonManager LessonManager { get; set; }

    private void Start()
    {
        if (rayInteractable != null)
        {
            rayInteractable.WhenPointerEventRaised += OnPointerEvent;
        }
    }

    private void OnDestroy()
    {
        if (rayInteractable != null)
        {
            rayInteractable.WhenPointerEventRaised -= OnPointerEvent;
        }

        // Restore ray color when block is destroyed (in case it was being hovered)
        RestoreRayColor();
    }

    private void OnPointerEvent(PointerEvent evt)
    {
        switch (evt.Type)
        {
            case PointerEventType.Hover:
                Debug.Log("[BlockShooter] Pointer entered");
                SetRayColorRed();
                break;
            case PointerEventType.Unhover:
                Debug.Log("[BlockShooter] Pointer exited");
                RestoreRayColor();
                break;
            case PointerEventType.Select:
                Debug.Log("[BlockShooter] Selected (clicked)");
                OnShoot();
                break;
            case PointerEventType.Unselect:
                Debug.Log("[BlockShooter] Unselected");
                break;
        }
    }

    private void InitializeRayVisuals()
    {
        if (colorsInitialized) return;

        // Cache ray visuals
        cachedRayVisuals = FindObjectsOfType<ControllerRayVisual>();
        if (cachedRayVisuals != null && cachedRayVisuals.Length > 0)
        {
            originalRayHoverColor0 = new Color[cachedRayVisuals.Length];
            originalRayHoverColor1 = new Color[cachedRayVisuals.Length];

            for (int i = 0; i < cachedRayVisuals.Length; i++)
            {
                originalRayHoverColor0[i] = cachedRayVisuals[i].HoverColor0;
                originalRayHoverColor1[i] = cachedRayVisuals[i].HoverColor1;
            }
            Debug.Log($"[BlockShooter] Initialized {cachedRayVisuals.Length} ray visuals");
        }

        // Cache cursor visuals (reticle)
        cachedCursorVisuals = FindObjectsOfType<RayInteractorCursorVisual>();
        if (cachedCursorVisuals != null && cachedCursorVisuals.Length > 0)
        {
            originalCursorHoverColor = new Color[cachedCursorVisuals.Length];

            for (int i = 0; i < cachedCursorVisuals.Length; i++)
            {
                originalCursorHoverColor[i] = cachedCursorVisuals[i].HoverColor;
            }
            Debug.Log($"[BlockShooter] Initialized {cachedCursorVisuals.Length} cursor visuals");
        }

        colorsInitialized = true;
    }

    private void SetRayColorRed()
    {
        InitializeRayVisuals();

        // Set ray color to red
        if (cachedRayVisuals != null)
        {
            foreach (var rayVisual in cachedRayVisuals)
            {
                if (rayVisual != null)
                {
                    rayVisual.HoverColor0 = hoverRayColor;
                    rayVisual.HoverColor1 = new Color(hoverRayColor.r, hoverRayColor.g, hoverRayColor.b, 0f);
                }
            }
        }

        // Set cursor (reticle) color to red
        if (cachedCursorVisuals != null)
        {
            foreach (var cursorVisual in cachedCursorVisuals)
            {
                if (cursorVisual != null)
                {
                    cursorVisual.HoverColor = hoverRayColor;
                }
            }
        }
    }

    private void RestoreRayColor()
    {
        if (!colorsInitialized) return;

        // Restore ray colors
        if (cachedRayVisuals != null && originalRayHoverColor0 != null)
        {
            for (int i = 0; i < cachedRayVisuals.Length; i++)
            {
                if (cachedRayVisuals[i] != null)
                {
                    cachedRayVisuals[i].HoverColor0 = originalRayHoverColor0[i];
                    cachedRayVisuals[i].HoverColor1 = originalRayHoverColor1[i];
                }
            }
        }

        // Restore cursor (reticle) colors
        if (cachedCursorVisuals != null && originalCursorHoverColor != null)
        {
            for (int i = 0; i < cachedCursorVisuals.Length; i++)
            {
                if (cachedCursorVisuals[i] != null)
                {
                    cachedCursorVisuals[i].HoverColor = originalCursorHoverColor[i];
                }
            }
        }
    }

    public void OnShoot()
    {
        if (isDestroying) return;

        // Check if this block is the correct next one in the sequence
        if (LessonManager != null && OrderIndex >= 0)
        {
            if (!LessonManager.TrySelectBlock(OrderIndex))
            {
                Debug.Log($"[BlockShooter] Wrong order! Expected index {LessonManager.CurrentBlockIndex}, got {OrderIndex}");
                LessonManager.OnWrongSelection();
                return;
            }
        }

        isDestroying = true;

        Debug.Log("[BlockShooter] Block shot - moving to target");

        // Play destroy sound through LessonManager and remove from list
        if (LessonManager != null)
        {
            LessonManager.PlayBlockDestroySound();
            LessonManager.RemoveBlockFromList(gameObject);

            // Disable RandomWalk during movement (check self and children)
            RandomWalk randomWalk = GetComponentInChildren<RandomWalk>();
            if (randomWalk != null)
            {
                randomWalk.enabled = false;
                Debug.Log("[BlockShooter] RandomWalk disabled");
            }
            else
            {
                // Also check parent
                randomWalk = GetComponentInParent<RandomWalk>();
                if (randomWalk != null)
                {
                    randomWalk.enabled = false;
                    Debug.Log("[BlockShooter] RandomWalk disabled (from parent)");
                }
            }

            // Disable RandomWalk's wall detection by destroying Rigidbodies
            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>(true))
            {
                Destroy(rb);
            }

            // Start moving to target position
            Vector3 targetPosition = LessonManager.GetFirstCodeLinePosition();
            StartCoroutine(MoveToTargetAndDestroy(targetPosition));
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator MoveToTargetAndDestroy(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(transform.position, targetPosition);
        float maxTime = distance / moveSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < maxTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Add token to the code line display
        if (LessonManager != null && !string.IsNullOrEmpty(TokenText))
        {
            LessonManager.AddCollectedToken(TokenText);
        }

        Debug.Log("[BlockShooter] Time reached - destroying");
        Destroy(gameObject);
    }
}
