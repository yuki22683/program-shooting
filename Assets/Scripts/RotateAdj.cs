using UnityEngine;

public class RotateAdj : MonoBehaviour
{
	[SerializeField] private Transform _target;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		RotateUpdate();
	}

	private void RotateUpdate()
	{
		float step = 5.0f * Time.deltaTime;

		Quaternion targetRotation = Quaternion.LookRotation(transform.position - _target.transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
	}
}