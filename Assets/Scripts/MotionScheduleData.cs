using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MotionScheduleData", menuName = "Motion/ScheduleData", order = 1)]
public class MotionScheduleData : ScriptableObject
{
	[System.Serializable]
	public struct MotionScheduleEntry
	{
		public float startTime;
		public MotionScheduler.MotionType motionType;
		public float value;
		public Vector3 pivot; // 回転時のピボットなど
	}

	[System.Serializable]
	public struct BombSchedule
	{
		public int bombId; // 爆弾の識別子
		public Vector3 initialPosition; // インスタンティエイト座標
		public List<MotionScheduleEntry> schedules; // 爆弾ごとのスケジュール
	}

	[System.Serializable]
	public struct FrameSchedule
	{
		public BombSchedule[] bombs; // 各爆弾のスケジュール（固定長配列）
	}

	[System.Serializable]
	public struct StageSchedule
	{
		public FrameSchedule[] frames; // 各レベルのスケジュール（固定長配列）
	}

	public StageSchedule[] stages; // 各ステージのスケジュール（固定長配列）

	// 特定のステージ、レベル、爆弾IDのスケジュールを取得するメソッド
	public BombSchedule GetBombSchedule(int stage, int frame, int bombId)
	{
		if (stage >= 0 && stage < stages.Length &&
			frame >= 0 && frame < stages[stage].frames.Length)
		{
			var bomb = System.Array.Find(stages[stage].frames[frame].bombs, b => b.bombId == bombId);
			return bomb.bombId == bombId ? bomb : default; // bombIdが一致しない場合もdefault
		}
		return default; // 無効な場合はデフォルト値を返す
	}
}