using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static MotionScheduleData;

public class MotionController : MonoBehaviour
{
	[SerializeField] private GameObject m_bombPrefab;
	[SerializeField] private MotionScheduleData scheduleData; // ステージごとのスケジュールデータ
	[SerializeField] private GameObject[] m_bombs;
	[SerializeField] private Explosion[] m_bombsExplosions;
	[SerializeField] private MotionScheduler[] m_bombsMotionSchedulers;

	public bool isComplete = false;

	private enum Axis
	{
		X,
		Y,
		Z
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		CheckComplete();
	}

#if false
	public void SetStageMotion(int stage, int frame, bool[] nonExplodeBomb)
	{
		isComplete = false;

		var frameSchedule = scheduleData.stages[stage].frames[frame];

		// レベル内のすべての爆弾をアクティブ化
		foreach (var bombSchedule in frameSchedule.bombs)
		{
			if (nonExplodeBomb[bombSchedule.bombId] == false)
			{
				continue;
			}

			// スケジュールから初期位置を使って爆弾を生成
			Vector3 initialPosition = new Vector3(0f, 0f, 0f);
			initialPosition.x = WtoL(Axis.X, bombSchedule.initialPosition.x);
			initialPosition.y = WtoL(Axis.Y, bombSchedule.initialPosition.y);
			initialPosition.z = WtoL(Axis.Z, bombSchedule.initialPosition.z);
			m_bombs[bombSchedule.bombId].SetActive(true); // 爆弾をアクティブにする
			m_bombs[bombSchedule.bombId].transform.position = initialPosition; // 爆弾の初期位置を設定

			// MotionScheduleEntryをMotionScheduler.MotionScheduleに変換
			var schedulerList = new System.Collections.Generic.List<MotionScheduler.MotionSchedule>();
			foreach (var entry in bombSchedule.schedules)
			{
				schedulerList.Add(new MotionScheduler.MotionSchedule(entry.startTime, entry.motionType, entry.value, entry.pivot));
			}

			// 爆弾にスケジュールを適用
			m_bombsMotionSchedulers[bombSchedule.bombId].StartMotion(schedulerList, false);
			m_bombsExplosions[bombSchedule.bombId].m_bombId = bombSchedule.bombId; // 爆弾IDを設定
		}
	}
#else
	public void SetStageMotion(int stage, int frame, int bombId)
	{
		isComplete = false;

		var bombSchedule = scheduleData.stages[stage].frames[frame].bombs[bombId];

		// スケジュールから初期位置を使って爆弾を生成
		Vector3 initialPosition = new Vector3(0f, 0f, 0f);
		initialPosition.x = WtoL(Axis.X, bombSchedule.initialPosition.x);
		initialPosition.y = WtoL(Axis.Y, bombSchedule.initialPosition.y);
		initialPosition.z = WtoL(Axis.Z, bombSchedule.initialPosition.z);
		m_bombs[bombSchedule.bombId].SetActive(true); // 爆弾をアクティブにする
		m_bombs[bombSchedule.bombId].transform.position = initialPosition; // 爆弾の初期位置を設定

		// MotionScheduleEntryをMotionScheduler.MotionScheduleに変換
		var schedulerList = new List<MotionScheduler.MotionSchedule>();
		foreach (var entry in bombSchedule.schedules)
		{
			schedulerList.Add(new MotionScheduler.MotionSchedule(entry.startTime, entry.motionType, entry.value, entry.pivot));
		}

		// 爆弾にスケジュールを適用
		m_bombsMotionSchedulers[bombSchedule.bombId].StartMotion(schedulerList, false);
		m_bombsExplosions[bombSchedule.bombId].m_bombId = bombSchedule.bombId; // 爆弾IDを設定
	}
#endif

	public void InactiveBombs()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(false); // 爆弾を非アクティブにする
		}
	}

	private float WtoL(Axis axis, float localPos)
	{
		float worldPos = 0f;

		switch (axis)
		{
			case Axis.X:
				worldPos = transform.parent.position.x + (localPos * transform.parent.localScale.x);
				break;
			case Axis.Y:
				worldPos = transform.parent.position.y + (localPos * transform.parent.localScale.y);
				break;
			case Axis.Z:
				worldPos = transform.parent.position.z + (localPos * transform.parent.localScale.z);
				break;
		}

		return worldPos;
	}

	private void CheckComplete()
	{
		if (isComplete || (transform.childCount == 0))
		{
			return;
		}
		for (int i = 0; i < transform.childCount; i++)
		{
			if (!transform.GetChild(i).GetComponent<MotionScheduler>().isCompleted)
			{
				return;
			}
		}
		isComplete = true;
		Debug.Log("すべての動作が完了しました。");
	}
}
