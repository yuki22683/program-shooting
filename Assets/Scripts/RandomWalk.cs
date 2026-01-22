using UnityEngine;
using Meta.XR.MRUtilityKit;

public class RandomWalk : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float wallDetectionDistance = 0f;

    [Header("Height Settings")]
    [SerializeField] private float minHeight = -2.0f;
    [SerializeField] private float maxHeight = 2.0f;

    [Header("Room Boundary")]
    [SerializeField] private float roomCheckInterval = 0.2f;
    [SerializeField] private float maxOutsideTime = 3.0f;

    [Header("Distance Limit")]
    [SerializeField] private float maxDistanceFromHeadset = 5.0f;
    [SerializeField] private float randomAngleRange = 45f;

    private Vector3 moveDirection;
    private bool isInitialized;
    private Collider ownCollider;
    private Transform headsetTransform;
    private float roomCheckTimer;
    private bool isReturningToRoom;
    private float outsideRoomTimer;

    private void Start()
    {
        Debug.Log($"[RandomWalk] Start called - minHeight={minHeight}, maxHeight={maxHeight}, moveSpeed={moveSpeed}, wallDetectionDistance={wallDetectionDistance}");
        ownCollider = GetComponent<Collider>();
        headsetTransform = Camera.main?.transform;
        Initialize();
    }

    private void Update()
    {
        if (!isInitialized)
        {
            return;
        }

        // Check distance from headset - turn inward if too far
        if (CheckDistanceFromHeadset())
        {
            TurnTowardsHeadsetWithRandomAngle();
        }

        // Check for wall collision using raycast
        RaycastHit? wallHit = CheckWallAhead();
        if (wallHit.HasValue)
        {
            // Reflect off the wall
            Vector3 normal = wallHit.Value.normal;
            moveDirection = Vector3.Reflect(moveDirection, normal).normalized;
        }

        // Always move
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Clamp height and reflect if hitting height limits
        Vector3 pos = transform.position;
        if (pos.y < minHeight || pos.y > maxHeight)
        {
            pos.y = Mathf.Clamp(pos.y, minHeight, maxHeight);
            transform.position = pos;
            moveDirection.y = -moveDirection.y;
            moveDirection = moveDirection.normalized;
        }

        // Always face the headset
        FaceHeadset();
    }

    private void CheckRoomBoundary()
    {
        // Use raycast-based check instead of MRUK's IsPositionInRoom
        bool isInsideWalls = CheckInsideUsingRaycast();

        if (!isInsideWalls)
        {
            // Block is outside the room
            outsideRoomTimer += roomCheckInterval;

            // Get target position (headset is always inside the room)
            Vector3 targetPos = GetHeadsetPosition();

            // Check if been outside too long - teleport back
            if (outsideRoomTimer >= maxOutsideTime)
            {
                Debug.Log($"[RandomWalk] Block outside too long, teleporting to {targetPos}");
                transform.position = targetPos;
                isReturningToRoom = false;
                outsideRoomTimer = 0f;
                SetRandomDirection();
                return;
            }

            // Always update direction towards target while outside
            isReturningToRoom = true;
            Vector3 directionToTarget = (targetPos - transform.position).normalized;
            moveDirection = directionToTarget;

            Debug.Log($"[RandomWalk] Block outside room at {transform.position}, moving to headset at {targetPos}");
        }
        else
        {
            // Block is in the room
            if (isReturningToRoom)
            {
                // Just returned to room, resume normal movement
                isReturningToRoom = false;
                outsideRoomTimer = 0f;
                SetRandomDirection();
                Debug.Log("[RandomWalk] Block returned to room, resuming random walk");
            }
            else
            {
                // Normal operation, reset outside timer
                outsideRoomTimer = 0f;
            }
        }
    }

    /// <summary>
    /// Check if inside room by casting rays in multiple directions
    /// If we can hit walls in opposite directions, we're likely inside
    /// </summary>
    private bool CheckInsideUsingRaycast()
    {
        Vector3 pos = transform.position;
        float maxDistance = 10f;
        int wallHits = 0;

        // Cast rays in 4 horizontal directions
        Vector3[] directions = new Vector3[]
        {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right
        };

        foreach (var dir in directions)
        {
            if (Physics.Raycast(pos, dir, out RaycastHit hit, maxDistance))
            {
                // Check if we hit a room mesh (SceneMesh/GlobalMesh)
                if (IsRoomMesh(hit.collider))
                {
                    wallHits++;
                }
            }
        }

        // If we hit walls in at least 2 directions, we're probably inside
        return wallHits >= 2;
    }

    /// <summary>
    /// Check if collider is part of room mesh
    /// </summary>
    private bool IsRoomMesh(Collider col)
    {
        if (col == null) return false;

        // Check various ways room mesh might be identified
        string name = col.gameObject.name.ToLower();
        if (name.Contains("mesh") || name.Contains("room") || name.Contains("wall") ||
            name.Contains("floor") || name.Contains("ceiling") || name.Contains("global") ||
            name.Contains("scene") || name.Contains("volume"))
        {
            return true;
        }

        // Check for Room tag (use direct comparison to avoid exception if tag doesn't exist)
        try
        {
            if (col.CompareTag("Room"))
            {
                return true;
            }
        }
        catch (UnityException)
        {
            // Tag doesn't exist, ignore
        }

        // Check if it has MeshCollider (room meshes typically do)
        if (col is MeshCollider)
        {
            return true;
        }

        return false;
    }

    private Vector3 GetHeadsetPosition()
    {
        if (headsetTransform == null)
        {
            headsetTransform = Camera.main?.transform;
        }

        if (headsetTransform != null)
        {
            Vector3 headPos = headsetTransform.position;
            return new Vector3(headPos.x, Mathf.Clamp(headPos.y, minHeight, maxHeight), headPos.z);
        }

        return new Vector3(0, (minHeight + maxHeight) / 2f, 0);
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

    private RaycastHit? CheckWallAhead()
    {
        Vector3 origin = transform.position;

        // Main ray in movement direction
        RaycastHit[] hits = Physics.RaycastAll(origin, moveDirection, wallDetectionDistance);

        // Find the closest hit (any collider except self)
        RaycastHit? closestHit = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            // Ignore self
            if (hit.collider == ownCollider) continue;
            if (hit.collider.transform.IsChildOf(transform)) continue;

            // Ignore other blocks (BlockShooter)
            if (hit.collider.GetComponent<BlockShooter>() != null) continue;
            if (hit.collider.GetComponentInParent<BlockShooter>() != null) continue;

            if (hit.distance < closestDistance)
            {
                closestDistance = hit.distance;
                closestHit = hit;
            }
        }

        return closestHit;
    }

    private void Initialize()
    {
        Debug.Log("[RandomWalk] Initializing...");
        SetRandomDirection();
        isInitialized = true;
        Debug.Log($"[RandomWalk] Initialized! Direction={moveDirection}, Position={transform.position}");
    }

    /// <summary>
    /// Skips the initialization delay and starts moving immediately
    /// </summary>
    public void InitializeImmediately()
    {
        if (!isInitialized)
        {
            Initialize();
        }
    }

    private void SetRandomDirection()
    {
        moveDirection = Random.onUnitSphere;
        // Vertical movement is now unrestricted
        moveDirection = moveDirection.normalized;
    }

    /// <summary>
    /// Check if the block is too far from the headset
    /// </summary>
    /// <returns>True if distance exceeds maxDistanceFromHeadset</returns>
    private bool CheckDistanceFromHeadset()
    {
        if (headsetTransform == null)
        {
            headsetTransform = Camera.main?.transform;
            if (headsetTransform == null) return false;
        }

        float distance = Vector3.Distance(transform.position, headsetTransform.position);
        return distance > maxDistanceFromHeadset;
    }

    /// <summary>
    /// Turn towards the headset with a random angle offset (similar to wall bounce behavior)
    /// </summary>
    private void TurnTowardsHeadsetWithRandomAngle()
    {
        if (headsetTransform == null)
        {
            headsetTransform = Camera.main?.transform;
            if (headsetTransform == null) return;
        }

        // Get direction towards headset
        Vector3 directionToHeadset = (headsetTransform.position - transform.position).normalized;

        // Add random angle offset around the Y axis
        float randomYAngle = Random.Range(-randomAngleRange, randomAngleRange);
        Quaternion randomRotation = Quaternion.Euler(0, randomYAngle, 0);
        Vector3 randomizedDirection = randomRotation * directionToHeadset;

        // Add vertical randomness
        randomizedDirection.y += Random.Range(-0.5f, 0.5f);
        moveDirection = randomizedDirection.normalized;

        Debug.Log($"[RandomWalk] Too far from headset, turning inward. New direction: {moveDirection}");
    }
}
