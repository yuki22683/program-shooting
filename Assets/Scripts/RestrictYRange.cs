using UnityEngine;

public class RestrictYRange : MonoBehaviour
{
	[SerializeField] private float minY; // 初期Y位置からの下限（ローカル座標）
	[SerializeField] private float maxY;  // 初期Y位置からの上限（ローカル座標）

	private Rigidbody rb;
	private Transform parentTransform;
	private float initialLocalY; // 初期のローカルY位置

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		parentTransform = transform.parent;

		// 初期のローカルY位置を記録
		if (parentTransform != null)
		{
			initialLocalY = parentTransform.InverseTransformPoint(rb.position).y;
		}
		else
		{
			// 親がない場合はワールド座標のYを基準に
			initialLocalY = rb.position.y;
			Debug.LogWarning("親オブジェクトがないため、ワールド座標のYを基準にします。");
		}
	}

	void FixedUpdate()
	{
		// 現在のローカルY位置を取得
		float currentLocalY;
		if (parentTransform != null)
		{
			currentLocalY = parentTransform.InverseTransformPoint(rb.position).y;
		}
		else
		{
			currentLocalY = rb.position.y; // 親がない場合はワールドY
		}

		// 初期位置からのYのずれを計算
		float yOffset = currentLocalY - initialLocalY;

		// ずれをクランプ
		float clampedOffset = Mathf.Clamp(yOffset, minY, maxY);

		// クランプ後のローカルY位置を計算
		float targetLocalY = initialLocalY + clampedOffset;

		// ローカル座標をワールド座標に変換してRigidbodyに適用
		if (parentTransform != null)
		{
			Vector3 localPosition = parentTransform.InverseTransformPoint(rb.position);
			localPosition.y = targetLocalY;
			rb.position = parentTransform.TransformPoint(localPosition);
		}
		else
		{
			// 親がない場合はワールド座標で処理
			Vector3 worldPosition = rb.position;
			worldPosition.y = targetLocalY;
			rb.position = worldPosition;
		}
	}
}