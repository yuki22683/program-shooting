using UnityEngine;

public class UniformLinearMotion : MonoBehaviour
{
	// 速度（メートル毎秒）をインスペクターで設定可能
	[SerializeField]
	public float speed = 1f;

	void Update()
	{
		// Transformのforward方向に速度を掛けて移動
		transform.position += transform.forward * speed * Time.deltaTime;
	}
}
