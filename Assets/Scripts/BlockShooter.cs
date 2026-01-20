using Oculus.Interaction;
using UnityEngine;

public class BlockShooter : MonoBehaviour
{
    [SerializeField] private AudioClip destroySound;
    [SerializeField] private RayInteractable rayInteractable;

    private bool isDestroying;

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
        isDestroying = true;

        Debug.Log("[BlockShooter] Block shot - destroying");

        if (destroySound != null)
        {
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
        }

        Destroy(gameObject);
    }
}
