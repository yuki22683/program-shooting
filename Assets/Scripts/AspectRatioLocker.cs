using UnityEngine;

public class AspectRatioLocker : MonoBehaviour
{
	[SerializeField, Tooltip("縦横比を固定する対象のTransform（PanelWithManipulatorsのルート）")]
	private Transform _targetTransform;

	[SerializeField, Tooltip("固定するX/Yの縦横比（幅/高さ、例: 16f/9f）。1で初期比率を使用")]
	private float _customXYAspectRatio = 1f;

	[SerializeField, Tooltip("固定するZ/Yの比率（奥行き/高さ）。1で初期比率を使用")]
	private float _customZYAspectRatio = 1f;

	private Vector3 _initialScale;
	private float _xyAspectRatio; // X/Yの初期比率
	private float _zyAspectRatio; // Z/Yの初期比率

	private void Start()
	{
		if (_targetTransform == null)
		{
			_targetTransform = transform; // デフォルトで自身を対象
		}

		// 初期スケールを保存
		_initialScale = _targetTransform.localScale;

		// 初期比率を計算（ゼロ除算回避）
		_xyAspectRatio = _initialScale.y > Mathf.Epsilon ? _initialScale.x / _initialScale.y : 1f;
		_zyAspectRatio = _initialScale.y > Mathf.Epsilon ? _initialScale.z / _initialScale.y : 1f;

		// カスタム比率が指定されている場合、上書き
		if (_customXYAspectRatio > 0f)
		{
			_xyAspectRatio = _customXYAspectRatio;
		}
		if (_customZYAspectRatio > 0f)
		{
			_zyAspectRatio = _customZYAspectRatio;
		}
	}

	private void LateUpdate()
	{
		// 現在のスケールを取得
		Vector3 currentScale = _targetTransform.localScale;

		// Yスケールを基準にXとZを調整（縦横比を維持）
		float newXScale = currentScale.y * _xyAspectRatio;
		float newZScale = currentScale.y * _zyAspectRatio;

		// 新しいスケールを設定
		Vector3 newScale = new Vector3(newXScale, currentScale.y, newZScale);

		// スケール範囲を制限（オプション）
		newScale.x = Mathf.Clamp(newScale.x, 0.1f, 10f);
		newScale.y = Mathf.Clamp(newScale.y, 0.1f, 10f);
		newScale.z = Mathf.Clamp(newScale.z, 0.1f, 10f);

		// 新しいスケールを適用
		_targetTransform.localScale = newScale;
	}
}