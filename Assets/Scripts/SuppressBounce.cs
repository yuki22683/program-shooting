using UnityEngine;

public class SuppressBounce : MonoBehaviour
{
	private Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	// 衝突が継続中（妨げられている場合）
	void OnCollisionStay(Collision collision)
	{
		// 相手がRigidbodyを持つ場合
		if (collision.gameObject.GetComponent<Rigidbody>() != null)
		{
			// 速度をゼロにリセット
			rb.linearVelocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}
	}

	// 衝突が終了した瞬間（念のため）
	void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.GetComponent<Rigidbody>() != null)
		{
			rb.linearVelocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}
	}
}