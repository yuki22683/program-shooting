using UnityEngine;

public class PeriodicRotationChange : MonoBehaviour
{
	// 回転する対象のTransform（インスペクターで設定可能）
	public Transform targetTransform;

	// 回転軸（インスペクターで設定可能）
	public Vector3 rotationAxis = Vector3.up; // デフォルトはY軸

	// 回転速度（度/秒、インスペクターで設定可能）
	public float rotationSpeed = 720f; // デフォルトは1秒に2回転 = 720度

	void Update()
	{
		// targetTransformが指定されているか確認
		if (targetTransform != null)
		{
			// 指定されたTransformを回転
			targetTransform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime);
		}
	}

}
