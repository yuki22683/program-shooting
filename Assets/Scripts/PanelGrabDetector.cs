using Oculus.Interaction;
using UnityEngine;

public class PanelGrabDetector : MonoBehaviour
{
	[SerializeField] private Transform panel;
	[SerializeField] private GameManager gameManager;
	private Grabbable[] grabbables; // パネルのGrabbableコンポーネント

	private bool isActive = true;
	private float inactiveTimer = 0f;
	private float inactiveInterval = 1f;

	void Start()
	{
		// PanelWithManipulators内のすべてのGrabbableを取得
		grabbables = panel.GetComponentsInChildren<Grabbable>();
		//isActive = gameManager.isActive;
	}

	void Update()
	{
		bool isGrabbed = false;
		foreach (var grabbable in grabbables)
		{
			if (grabbable.SelectingPointsCount > 0)
			{
				isGrabbed = true;
				break;
			}
		}

		if (isGrabbed)
		{
			if (!isActive)
			{
				isActive = true;
				SetAbacusInactive();
			}
		}
		else
		{
			if (isActive)
			{
				isActive = false;
				inactiveTimer = Time.time;
			}
			else
			{
				//if ((Time.time > inactiveTimer + inactiveInterval)
				//	&& !gameManager.isActive)
				//{
				//	SetAbacusActive();
				//}
			}
		}
	}

	private void SetAbacusActive()
	{
		//gameManager.isActive = true;
		//gameManager.SetAbacusActive();
	}

	private void SetAbacusInactive()
	{
		//gameManager.isActive = false;
		//gameManager.SetAbacusInactive();
	}
}