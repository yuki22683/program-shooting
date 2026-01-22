using System.Collections;
using Oculus.Interaction;
using UnityEngine;

public class BlockShooter : MonoBehaviour
{
    [SerializeField] private RayInteractable rayInteractable;
    [SerializeField] private float moveSpeed = 0.5f;

    private bool isDestroying;

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
    }

    private void OnPointerEvent(PointerEvent evt)
    {
        switch (evt.Type)
        {
            case PointerEventType.Hover:
                Debug.Log("[BlockShooter] Pointer entered");
                break;
            case PointerEventType.Unhover:
                Debug.Log("[BlockShooter] Pointer exited");
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
