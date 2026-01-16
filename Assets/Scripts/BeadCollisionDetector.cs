using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Haptics;
using static BeadValue;

public class BeadCollisionDetector : MonoBehaviour
{
	[Header("OVRHand")]
	[SerializeField] private Transform rightIndexFingerTip; // 右手の人指し指の先端（Transformで指定）
	[SerializeField] private Transform leftIndexFingerTip;  // 左手の人指し指の先端（Transformで指定）
	[SerializeField] private Transform rightThumbTip;       // 右手の親指の先端（Transformで指定）
	[SerializeField] private Transform leftThumbTip;        // 左手の親指の先端（Transformで指定）

	[Header("OVRControllerDrivenHands")]
	[SerializeField] private Transform rightCDHIndexFingerTip; // 右手の人指し指の先端（Transformで指定）
	[SerializeField] private Transform leftCDHIndexFingerTip;  // 左手の人指し指の先端（Transformで指定）
	[SerializeField] private Transform rightCDHThumbTip;       // 右手の親指の先端（Transformで指定）
	[SerializeField] private Transform leftCDHThumbTip;        // 左手の親指の先端（Transformで指定）

	[SerializeField] private GameManager gameManager;

	[SerializeField] public BeadType beadType;
	[SerializeField] private Collider upperCollider;
	[SerializeField] private Collider lowerCollider;
	[SerializeField] private BeadMusk beadMusk;

	private AudioSource[] audioSources;

	private BeadColorManager beadColorManager;

	private bool initFlg = false;

	public float DownMaxDistance;
	public float UpMaxDistance;
	public Vector3 m_beadStartLocalPos; // 初期位置

	public enum BeadType
	{
		Upper,
		Lower,
	}

	private enum AudioType
	{
		MoveUpper,
		MoveLower,
	}

	private enum CollisionState
	{
		None,
		Upper,
		Lower,
	}

	private enum Direction
	{
		Up,
		Down,
	}

	public enum BeadState
	{
		None,
		InActive,
		Active
	}

	public const float beadHeight = 0.26f; // ビーズの高さ

	void Awake()
	{
		Init();
	}
	void Start()
	{
		//Init();
	}

	void Update()
	{
		DetectFingerCollision();
	}

	private void Init()
	{
		m_beadStartLocalPos = transform.parent.localPosition; // 初期位置を保存

		if (beadType == BeadType.Upper)
		{
			DownMaxDistance = beadHeight;
			UpMaxDistance = 0f;
		}
		else if (beadType == BeadType.Lower)
		{
			DownMaxDistance = 0f;
			UpMaxDistance = beadHeight;
		}
		else
		{
			Debug.LogError("BeadTypeが正しく設定されていません！");
		}

		audioSources = GetComponents<AudioSource>();
		beadColorManager = GetComponent<BeadColorManager>();

		initFlg = true;
	}

	private void DetectFingerCollision()
	{
		if (gameManager == null) return;
		//if (!gameManager.isActive)
		//{
		//	return;
		//}

		if (rightIndexFingerTip == null || leftIndexFingerTip == null || rightThumbTip == null || leftThumbTip == null)
		{
			return;
		}

		if (beadMusk.isMusk)
		{
			return;
		}

		RodManager rodManager = transform.parent.parent.GetComponent<RodManager>();

		if (rodManager.GetState(beadType) == RodManager.RodState.Disable)
		{
			return;
		}

		Vector3 rightThumbPos = rightThumbTip.position;
		Vector3 leftThumbPos = leftThumbTip.position;
		Vector3 rightIndexPos = rightIndexFingerTip.position;
		Vector3 leftIndexPos = leftIndexFingerTip.position;

		Vector3 rightCDHThumbPos = rightCDHThumbTip.position;
		Vector3 leftCDHThumbPos = leftCDHThumbTip.position;
		Vector3 rightCDHIndexPos = rightCDHIndexFingerTip.position;
		Vector3 leftCDHIndexPos = leftCDHIndexFingerTip.position;

		if (upperCollider.bounds.Contains(rightThumbPos)
		|| upperCollider.bounds.Contains(rightIndexPos)
		|| lowerCollider.bounds.Contains(rightThumbPos)
		|| lowerCollider.bounds.Contains(rightIndexPos)
		|| upperCollider.bounds.Contains(rightCDHThumbPos)
		|| upperCollider.bounds.Contains(rightCDHIndexPos)
		|| lowerCollider.bounds.Contains(rightCDHThumbPos)
		|| lowerCollider.bounds.Contains(rightCDHIndexPos)
		|| upperCollider.bounds.Contains(leftThumbPos)
		|| upperCollider.bounds.Contains(leftIndexPos)
		|| lowerCollider.bounds.Contains(leftThumbPos)
		|| lowerCollider.bounds.Contains(leftIndexPos)
		|| upperCollider.bounds.Contains(leftCDHThumbPos)
		|| upperCollider.bounds.Contains(leftCDHIndexPos)
		|| lowerCollider.bounds.Contains(leftCDHThumbPos)
		|| lowerCollider.bounds.Contains(leftCDHIndexPos))
		{
			MoveBead();
			if (beadType == BeadType.Lower)
			{
				//if (gameManager.gameMode != GameManager.GameMode.TutorialMode)
				//{
				//	rodManager.SetLowerBeadsStateDisable(transform.parent);
				//}
			}
			else
			{
				//if (gameManager.gameMode != GameManager.GameMode.TutorialMode)
				//{
				//	rodManager.SetUpperBeadStateDisable();
				//}
			}
		}
	}

	private void MoveBead()
	{
		float middleLocalPosY = m_beadStartLocalPos.y + (UpMaxDistance - DownMaxDistance) / 2;
		Vector3 newLocalPos = transform.parent.localPosition;

		if (transform.parent.localPosition.y > middleLocalPosY)
		{
			newLocalPos.y = m_beadStartLocalPos.y - DownMaxDistance;
			transform.parent.localPosition = newLocalPos;

			transform.parent.GetComponent<BeadValue>().SetBeadValue(BeadPos.Low, true);

			if (beadType == BeadType.Lower)
			{
				GetComponent<BeadPush>().LowerAdjustBeadsPosition(); // ビーズの位置を調整
			}

			PlayAudio(AudioType.MoveLower);
		}
		else
		{
			newLocalPos.y = m_beadStartLocalPos.y + UpMaxDistance;
			transform.parent.localPosition = newLocalPos;

			transform.parent.GetComponent<BeadValue>().SetBeadValue(BeadPos.High, true);

			if (beadType == BeadType.Lower)
			{
				GetComponent<BeadPush>().UpperAdjustBeadsPosition(); // ビーズの位置を調整
			}

			PlayAudio(AudioType.MoveUpper);
		}
		transform.parent.parent.GetComponent<RodValue>().CalcRodValue();
	}

	public void SetBeadPos(BeadPos beadPos)
	{
		Vector3 newLocalPos = transform.parent.localPosition;

		if (beadPos == BeadPos.High)
		{
			newLocalPos.y = m_beadStartLocalPos.y + UpMaxDistance;
		}
		else
		{
			newLocalPos.y = m_beadStartLocalPos.y - DownMaxDistance;
		}

		transform.parent.localPosition = newLocalPos;
	}

	private void PlayAudio(AudioType type)
	{
		audioSources[(int)type].Stop();
		audioSources[(int)type].Play();
	}
}