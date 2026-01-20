using UnityEngine;
using System.Collections.Generic;

public class MotionScheduler : MonoBehaviour
{
	// MotionManagerへの参照
	[SerializeField] private MotionManager motionManager;

	// 動作スケジュールのリスト
	public List<MotionSchedule> motionSchedules = new List<MotionSchedule>();

	// 完了フラグ
	public bool isCompleted { get; private set; } = false;

	private float elapsedTime = 0f;     // 経過時間
	public bool isRunning = false;     // 実行中フラグ
	private int currentIndex = 0;       // 現在のスケジュールインデックス
	private bool shouldRepeat = false;  // 繰り返しフラグ

	// 動作スケジュールのデータ構造
	[System.Serializable]
	public class MotionSchedule
	{
		public float triggerTime;       // 動作開始時間（秒）
		public MotionType motionType;   // 動作の種類
		public float speed;             // 速度
		public Vector3 center;          // 回転中心（回転動作の場合のみ使用）

		public MotionSchedule(float time, MotionType type, float speed = 0f, Vector3 center = default)
		{
			triggerTime = time;
			motionType = type;
			this.speed = speed;
			this.center = center;
		}
	}

	// 動作の種類を定義する列挙型
	public enum MotionType
	{
		MoveRight,
		MoveLeft,
		MoveUp,
		MoveDown,
		MoveForward,
		MoveBackward,
		RotateXPositive,
		RotateXNegative,
		RotateYPositive,
		RotateYNegative,
		RotateZPositive,
		RotateZNegative,
		Stop
	}

	void Update()
	{
		if (!isRunning) return;

		elapsedTime += Time.deltaTime;

		// 現在のスケジュールを確認
		if (currentIndex < motionSchedules.Count)
		{
			MotionSchedule currentSchedule = motionSchedules[currentIndex];

			if (elapsedTime >= currentSchedule.triggerTime)
			{
				ExecuteMotion(currentSchedule);
				currentIndex++;
			}
		}

		// すべてのスケジュールが完了した場合の処理
		if (currentIndex >= motionSchedules.Count)
		{
			if (shouldRepeat)
			{
				// 繰り返しの場合、最初からやり直す
				elapsedTime = 0f;
				currentIndex = 0;
			}
			else
			{
				// 繰り返さない場合、完了フラグを立てて停止
				isCompleted = true;
				isRunning = false;
				motionManager.StopMotion();
			}
		}
	}

	// 動作開始の公開メソッド（繰り返しフラグを追加）
	public void StartMotion(List<MotionSchedule> schedules, bool repeat = false)
	{
		if (motionManager == null)
		{
			Debug.LogError("MotionManagerが設定されていません");
			return;
		}

		motionSchedules = schedules;
		elapsedTime = 0f;
		currentIndex = 0;
		isRunning = true;
		isCompleted = false;
		shouldRepeat = repeat;
	}

	// スケジュールに基づいて動作を実行
	private void ExecuteMotion(MotionSchedule schedule)
	{
		switch (schedule.motionType)
		{
			case MotionType.MoveRight:
				motionManager.StartMoveRight(schedule.speed);
				break;
			case MotionType.MoveLeft:
				motionManager.StartMoveLeft(schedule.speed);
				break;
			case MotionType.MoveUp:
				motionManager.StartMoveUp(schedule.speed);
				break;
			case MotionType.MoveDown:
				motionManager.StartMoveDown(schedule.speed);
				break;
			case MotionType.MoveForward:
				motionManager.StartMoveForward(schedule.speed);
				break;
			case MotionType.MoveBackward:
				motionManager.StartMoveBackward(schedule.speed);
				break;
			case MotionType.RotateXPositive:
				motionManager.StartRotateXPositive(schedule.speed, schedule.center);
				break;
			case MotionType.RotateXNegative:
				motionManager.StartRotateXNegative(schedule.speed, schedule.center);
				break;
			case MotionType.RotateYPositive:
				motionManager.StartRotateYPositive(schedule.speed, schedule.center);
				break;
			case MotionType.RotateYNegative:
				motionManager.StartRotateYNegative(schedule.speed, schedule.center);
				break;
			case MotionType.RotateZPositive:
				motionManager.StartRotateZPositive(schedule.speed, schedule.center);
				break;
			case MotionType.RotateZNegative:
				motionManager.StartRotateZNegative(schedule.speed, schedule.center);
				break;
			case MotionType.Stop:
				motionManager.StopMotion(); // ストップ動作
				break;
		}
	}
}