using UnityEngine;
using static BeadValue;

public class BeadPush : MonoBehaviour
{
    [SerializeField] private Transform[] upperBeads;
	[SerializeField] private Transform[] lowerBeads;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
	}

    // Update is called once per frame
    void Update()
    {
    }

	public void UpperAdjustBeadsPosition()
	{
		for (int i = 0; i < upperBeads.Length; i++)
		{
			float bead1Y = (i == 0) ? transform.parent.localPosition.y : upperBeads[i - 1].parent.localPosition.y;
			float bead2Y = upperBeads[i].parent.localPosition.y;

			if (bead2Y - bead1Y < BeadCollisionDetector.beadHeight)
			{
				Vector3 newLocalPos = upperBeads[i].parent.localPosition;
				newLocalPos.y = bead1Y + BeadCollisionDetector.beadHeight;

				BeadCollisionDetector beadCollisionDetector = upperBeads[i].GetComponent<BeadCollisionDetector>();

				if (newLocalPos.y > beadCollisionDetector.m_beadStartLocalPos.y + beadCollisionDetector.UpMaxDistance)
				{
					newLocalPos.y = beadCollisionDetector.m_beadStartLocalPos.y + beadCollisionDetector.UpMaxDistance;
				}
				upperBeads[i].parent.localPosition = newLocalPos;

				upperBeads[i].parent.GetComponent<BeadValue>().SetBeadValue(BeadPos.High, false);
			}
		}
	}

	public void LowerAdjustBeadsPosition()
	{
		for (int i = 0; i < lowerBeads.Length; i++)
		{
			float bead1Y = (i == 0) ? transform.parent.localPosition.y : lowerBeads[i - 1].parent.localPosition.y;
			float bead2Y = lowerBeads[i].parent.localPosition.y;

			BeadCollisionDetector beadCollisionDetector = lowerBeads[i].GetComponent<BeadCollisionDetector>();

			if (bead1Y - bead2Y < BeadCollisionDetector.beadHeight)
			{
				Vector3 newLocalPos = lowerBeads[i].parent.localPosition;
				newLocalPos.y = bead1Y - BeadCollisionDetector.beadHeight;

				if (newLocalPos.y < beadCollisionDetector.m_beadStartLocalPos.y - beadCollisionDetector.DownMaxDistance)
				{
					newLocalPos.y = beadCollisionDetector.m_beadStartLocalPos.y - beadCollisionDetector.DownMaxDistance;
				}
				lowerBeads[i].parent.localPosition = newLocalPos;

				lowerBeads[i].parent.GetComponent<BeadValue>().SetBeadValue(BeadPos.Low, false);
			}
		}
	}
}
