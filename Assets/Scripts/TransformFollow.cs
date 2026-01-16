using UnityEngine;

public class TransformFollow : MonoBehaviour
{
	[SerializeField] private Transform target; // The target transform to follow

	private Vector3 m_TargetPreviousLocalScale;

	private float xRatio = 1f;
	private float yRatio = 1f;
	private float zRatio = 1f;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		xRatio = transform.localScale.x / target.localScale.x;
		yRatio = transform.localScale.y / target.localScale.y;
		zRatio = transform.localScale.z / target.localScale.z;
		m_TargetPreviousLocalScale = target.localScale;
	}

	// Update is called once per frame
	void Update()
	{
		if (transform.position != target.position)
		{
			transform.position = target.position;
		}
		if (transform.rotation != target.rotation)
		{
			transform.rotation = target.rotation;
		}
		if (m_TargetPreviousLocalScale != target.localScale)
		{
			Vector3 localScale = target.localScale;
			localScale.x *= xRatio;
			localScale.y *= yRatio;
			localScale.z *= zRatio;
			transform.localScale = localScale;
			m_TargetPreviousLocalScale = target.localScale;
		}
	}
}
