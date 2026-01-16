using UnityEngine;

public class RodManager : MonoBehaviour
{
	[SerializeField] private Transform upperBead;
	[SerializeField] private Transform[] lowerBeads;

	[SerializeField] private GameManager gameManager;

	public RodState upperBeadState = RodState.Enable; // ロッドの状態を管理する変数
	public RodState lowerBeadsState = RodState.Enable; // ロッドの状態を管理する変数

	public enum RodState
    {
        Disable,
		Enable,
	}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpperBeadStateDisable()
	{
		upperBeadState = RodState.Disable;
		//Invoke("SetUpperBeadStateEnable", DataManager.gameSettings.controlSettings.controlDebounceTime);
	}

	public void SetLowerBeadsStateDisable(Transform moveLowerBead)
    {
		lowerBeadsState = RodState.Disable;
		SetLowerBeadsColor(false, moveLowerBead);
		//Invoke("SetLowerBeadStateEnable", DataManager.gameSettings.controlSettings.controlDebounceTime);
	}

	public void SetUpperBeadStateEnable()
    {
		upperBeadState = RodState.Enable;
		//if (gameManager.isActive)
		//{
		//	SetUpperBeadsColor(true);
		//}
	}

	public void SetLowerBeadStateEnable()
	{
		lowerBeadsState = RodState.Enable;
		//if (gameManager.isActive)
		//{
		//	SetLowerBeadsColor(true, null);
		//}
	}

	private void SetLowerBeadsColor(bool flg, Transform moveLowerBead)
    {
		foreach (Transform lowerBead in lowerBeads)
		{
			if ((lowerBead == moveLowerBead) && !flg)
			{
				continue;
			}
			BeadColorManager beadColorManager = lowerBead.GetChild(0).GetComponent<BeadColorManager>();
			int value = lowerBead.GetComponent<BeadValue>().value;

			if (flg)
			{
				if (value > 0)
				{
					beadColorManager.SetMeshColor(BeadColorManager.BeadColorType.Active);
				}
				else
				{
					beadColorManager.SetMeshColor(BeadColorManager.BeadColorType.None);
				}
			}
			else
			{
				beadColorManager.SetMeshColor(BeadColorManager.BeadColorType.Disable);
			}
		}
	}

	private void SetUpperBeadsColor(bool flg)
	{
		BeadColorManager beadColorManager = upperBead.GetChild(0).GetComponent<BeadColorManager>();
		int value = upperBead.GetComponent<BeadValue>().value;

		if (flg)
		{
			if (value > 0)
			{
				beadColorManager.SetMeshColor(BeadColorManager.BeadColorType.Active);
			}
			else
			{
				beadColorManager.SetMeshColor(BeadColorManager.BeadColorType.None);
			}
		}
		else
		{
			beadColorManager.SetMeshColor(BeadColorManager.BeadColorType.Disable);
		}
	}

	public RodState GetState(BeadCollisionDetector.BeadType beadType)
	{
		if (beadType == BeadCollisionDetector.BeadType.Upper)
		{
			return upperBeadState;
		}
		else
		{
			return lowerBeadsState;
		}
	}
}
