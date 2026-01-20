using UnityEngine;

public class SmoothResize : MonoBehaviour
{
	[SerializeField] Transform m_targetTransform; // サイズを変更する対象のTransform

	private Vector3 m_originalScale;    // 元のサイズを保存
	private float m_time;               // 時間カウンター
	public float m_scaleMultiplier = 0.2f; // スケールの変動幅（±20%）
	public float m_cycleDuration = 1f;  // 1周期の時間（秒）

	void Start()
	{
		// 初期スケールを保存
		m_originalScale = transform.localScale;
	}

	void Update()
	{
		// 時間を進める
		m_time += Time.deltaTime;

		// sin波を計算（2πで1周期）
		float sineWave = Mathf.Sin((m_time * 2 * Mathf.PI) / m_cycleDuration);

		// 元のスケールに±20%の変動を適用
		float scaleFactor = 1f + (sineWave * m_scaleMultiplier);
		m_targetTransform.localScale = m_originalScale * scaleFactor;
	}
}