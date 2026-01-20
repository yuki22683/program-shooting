using Oculus.Interaction;
using UnityEngine;

public class CollisionDetectBall : MonoBehaviour
{
	[SerializeField] private UniformLinearMotion m_uniformLinearMotion;
	[SerializeField] private PeriodicRotationChange m_periodicRotation;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
	}

	private void DestroyBall()
	{
		Destroy(m_uniformLinearMotion.gameObject);
	}

	private void Stop()
	{
		m_uniformLinearMotion.speed = 0f;
		m_periodicRotation.rotationSpeed = 0f;
		Debug.Log("Stop");
	}

	private void OnCollisionEnter(Collision collision)
	{
		switch (collision.gameObject.tag)
		{
			case "Bomb":
				Debug.Log("Bomb");

				// ”j‰ó‚·‚é
				Stop();
				Invoke("DestroyBall", 0.2f);
				collision.transform.parent.parent.gameObject.GetComponent<Explosion>().Explode();

				break;
			case "Room":
				Debug.Log("Room");

				Stop();
				break;
			default:
				Debug.Log("Unknown");
				break;
		}
	}
}
