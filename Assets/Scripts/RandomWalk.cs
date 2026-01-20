using UnityEngine;

public class RandomWalk : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float arrivalThreshold = 0.1f;

    [Header("Bounds Settings")]
    [SerializeField] private float wanderRadius = 3f;
    [SerializeField] private float minHeight = 0.5f;
    [SerializeField] private float maxHeight = 2.0f;
    [SerializeField] private float wallCheckDistance = 0.3f;

    [Header("Raycast Settings")]
    [SerializeField] private LayerMask wallLayerMask = -1;

    private Vector3 targetPosition;
    private Vector3 startPosition;
    private bool hasTarget;
    private Transform headset;

    private void Start()
    {
        startPosition = transform.position;
        headset = Camera.main?.transform;
        SetNewTargetPosition();
        Debug.Log($"[RandomWalk] Started at {startPosition}");
    }

    private void Update()
    {
        if (!hasTarget)
        {
            SetNewTargetPosition();
            return;
        }

        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance <= arrivalThreshold)
        {
            SetNewTargetPosition();
            return;
        }

        // Check for walls ahead
        if (Physics.Raycast(transform.position, direction, wallCheckDistance, wallLayerMask))
        {
            SetNewTargetPosition();
            return;
        }

        transform.position += direction * moveSpeed * Time.deltaTime;

        FaceHeadset();
    }

    private void FaceHeadset()
    {
        if (headset == null)
        {
            headset = Camera.main?.transform;
            if (headset == null) return;
        }

        Vector3 directionFromHeadset = transform.position - headset.position;

        if (directionFromHeadset.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(directionFromHeadset);
        }
    }

    private void SetNewTargetPosition()
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection.y = 0;
            Vector3 candidate = startPosition + randomDirection;
            candidate.y = Random.Range(minHeight, maxHeight);

            // Check if path is clear
            Vector3 dirToCandidate = (candidate - transform.position).normalized;
            float distToCandidate = Vector3.Distance(transform.position, candidate);

            if (!Physics.Raycast(transform.position, dirToCandidate, distToCandidate, wallLayerMask))
            {
                targetPosition = candidate;
                hasTarget = true;
                Debug.Log($"[RandomWalk] New target: {targetPosition}");
                return;
            }
        }

        // Fallback: just move to a nearby position
        targetPosition = transform.position + Random.insideUnitSphere * 0.5f;
        targetPosition.y = Mathf.Clamp(targetPosition.y, minHeight, maxHeight);
        hasTarget = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = Application.isPlaying ? startPosition : transform.position;
        Gizmos.DrawWireSphere(center, wanderRadius);

        if (hasTarget)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, targetPosition);
            Gizmos.DrawSphere(targetPosition, 0.05f);
        }
    }
}
