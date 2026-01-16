using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
	public TextMeshPro timerText; // TextMeshProUGUIコンポーネントへの参照
	private float elapsedTime = 0f;   // 経過時間（秒）
	private bool isRunning = false; // タイマーが動作中かどうか

	void Start()
	{
		// 必要に応じてTextMeshProUGUIを自動取得
		if (timerText == null)
		{
			timerText = GetComponent<TextMeshPro>();
		}
		timerText.text = ""; // 初期表示を設定
	}

	void Update()
	{
		if (isRunning)
		{
			// 経過時間を更新
			elapsedTime += Time.deltaTime;

			// 分と秒を計算
			int minutes = Mathf.FloorToInt(elapsedTime / 60);
			int seconds = Mathf.FloorToInt(elapsedTime % 60);

			// TextMeshProに時間を表示（00:00形式）
			timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
		}
	}

	// 経過時間をリセット
	public void ResetTimer()
	{
		isRunning = true;
		elapsedTime = 0f;
		timerText.text = "00:00"; // リセット時に表示を更新
	}

	public void ExitTimer()
	{
		isRunning = false;
		elapsedTime = 0f;
		timerText.text = "00:00"; // リセット時に表示を更新
	}

	public void StopTimer()
	{
		isRunning = false;
	}

	public void ReStartTimer()
	{
		isRunning = true;
	}
}