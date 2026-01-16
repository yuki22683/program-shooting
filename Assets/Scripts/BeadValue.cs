using UnityEngine;
using static BeadCollisionDetector;
using static RodManager;

public class BeadValue : MonoBehaviour
{
    public int value = 0;
	public int id = 0;

    public enum BeadPos
    {
        High,
        Low
    }

	[SerializeField] private BeadCollisionDetector bead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void Init()
	{
		if (bead.beadType == BeadType.Upper)
		{
			SetBeadValue(BeadPos.High, false);
		}
		else
		{
			SetBeadValue(BeadPos.Low, false);
		}
	}

    public void SetBeadPos(BeadPos beadPos)
    {
		SetBeadValue(beadPos, false);
		BeadCollisionDetector beadCollisionDetector = GetComponentInChildren<BeadCollisionDetector>();
        beadCollisionDetector.SetBeadPos(beadPos);
	}

    public void  SetBeadValue(BeadPos beadPos, bool isHand)
    {
		if (beadPos == BeadPos.High)
		{
			if (bead.beadType == BeadType.Upper)
			{
				value = 0;
			}
			else
			{
				value = 1;
			}
		}
		else
		{
			if (beadPos == BeadPos.Low)
			{
				if (bead.beadType == BeadType.Upper)
				{
					value = 5;
				}
				else
				{
					value = 0;
				}
			}
		}
		SetBeadColor(isHand);
	}

	private void SetBeadColor(bool isHand)
	{
		BeadColorManager beadColor = bead.gameObject.GetComponent<BeadColorManager>();
		RodManager rodManager = transform.parent.GetComponent<RodManager>();
		if (isHand)
		{
			beadColor.SetMeshColor(BeadColorManager.BeadColorType.Touch);
		}
		else
		{
			if (value > 0)
			{
				beadColor.SetMeshColor(BeadColorManager.BeadColorType.Active);
			}
			else
			{
				beadColor.SetMeshColor(BeadColorManager.BeadColorType.None);
			}
		}
	}
}
