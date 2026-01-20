using UnityEngine;

public class RandomWalk : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float wallDetectionDistance = 0.5f;

    [Header("Height Settings")]
    [SerializeField] private float minHeight = 0.5f;
    [SerializeField] private float maxHeight = 2.0f;

    [Header("Initialization")]
    [SerializeField] private float initDelay = 3.0f;

    private Vector3 moveDirection;
    private bool isInitialized;
    private float initTimer;
    private Collider ownCollider;
    private Transform headsetTransform;

    private void Start()
    {
        Debug.Log("[RandomWalk] Start called - waiting for room mesh to load");
        initTimer = 0f;
        ownCollider = GetComponent<Collider>();
        headsetTransform = Camera.main?.transform;
    }

    private void Update()
    {
        // Wait for initialization delay
        if (!isInitialized)
        {
            initTimer += Time.deltaTime;
            if (initTimer >= initDelay)
            {
                Initialize();
            }
            return;
        }

        // Check for wall collision using raycast (detect ANY collider, not just "Room" tag)
        bool willHitWall = CheckWallAhead();

        if (willHitWall)
        {
            // Change direction until we find a clear path
            SetRandomDirection();
        }
        else
        {
            // Move only when path is clear
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }

        // Clamp height
        Vector3 pos = transform.position;
        if (pos.y < minHeight || pos.y > maxHeight)
        {
            pos.y = Mathf.Clamp(pos.y, minHeight, maxHeight);
            transform.position = pos;
            SetRandomDirection();
        }

        // Always face the headset
        FaceHeadset();
    }

    private void FaceHeadset()
    {
        if (headsetTransform == null)
        {
            headsetTransform = Camera.main?.transform;
            if (headsetTransform == null) return;
        }

        Vector3 directionToHeadset = transform.position - headsetTransform.position;

        if (directionToHeadset.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(directionToHeadset);
        }
    }

    private bool CheckWallAhead()
    {
        // Cast multiple rays to detect walls more reliably
        Vector3 origin = transform.position;

        // Main ray in movement direction
        RaycastHit[] hits = Physics.RaycastAll(origin, moveDirection, wallDetectionDistance);

        foreach (var hit in hits)
        {
            // Ignore self
            if (hit.collider == ownCollider) continue;
            if (hit.collider.transform.IsChildOf(transform)) continue;

            // Hit something that's not self - it's a wall or obstacle
            Debug.Log($"[RandomWalk] Wall detected: {hit.collider.name} at distance {hit.distance}");
            return true;
        }

        return false;
    }

    private void Initialize()
    {
        Debug.Log("[RandomWalk] Initializing...");
        SetRandomDirection();
        isInitialized = true;
        Debug.Log($"[RandomWalk] Initialized! Direction={moveDirection}, Position={transform.position}");
    }

    private void SetRandomDirection()
    {
        moveDirection = Random.onUnitSphere;
        moveDirection.y *= 0.3f; // Reduce vertical movement
        moveDirection = moveDirection.normalized;
    }
}
