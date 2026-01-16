using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections.Generic;

public class DisableGrabOnTrigger : MonoBehaviour
{
	[SerializeField] private GameObject abacusObject;

	private GrabInteractable[] grabInteractables;
	private HandGrabInteractable[] handGrabInteractables;
	private Grabbable[] grabbables;
	//private DistanceGrabInteractable[] distanceGrabbables;
	private List<Collider> collidersInTrigger = new List<Collider>(); // トリガー内のコライダーを追跡
	
	void Start()
	{
		grabInteractables = abacusObject.GetComponentsInChildren<GrabInteractable>();
		handGrabInteractables = abacusObject.GetComponentsInChildren<HandGrabInteractable>();
		grabbables = abacusObject.GetComponentsInChildren<Grabbable>();
		//distanceGrabbables = abacusObject.GetComponentsInChildren<DistanceGrabInteractable>();
	}

	void OnTriggerEnter(Collider other)
	{
		// 自身や無効なコライダーを除外
		if (other == null || other.gameObject == gameObject || other is SphereCollider) return;

		if (!other.CompareTag("Player")) return;

		// コライダーをリストに追加（重複防止）
		if (!collidersInTrigger.Contains(other))
		{
			collidersInTrigger.Add(other);
		}

		// GrabInteractableを無効化
		if (grabInteractables != null)
		{
			foreach (GrabInteractable grabInteractable in grabInteractables)
			{
				grabInteractable.enabled = false;
			}
		}

		// HandGrabInteractableを無効化
		if (handGrabInteractables != null)
		{
			foreach(HandGrabInteractable handGrabInteractable in handGrabInteractables)
			{
				handGrabInteractable.enabled = false;
			}
		}

		// grabbableを無効化
		if (grabbables != null)
		{
			foreach(Grabbable grabbable in grabbables)
			{
				grabbable.enabled = false;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		// 自身や無効なコライダーを除外
		if (other == null || other.gameObject == gameObject || other is SphereCollider) return;

		if (!other.CompareTag("Player")) return;

		// コライダーをリストから削除
		if (collidersInTrigger.Contains(other))
		{
			collidersInTrigger.Remove(other);
		}

		// トリガー内に他のコライダーがない場合のみ有効化
		if (collidersInTrigger.Count == 0)
		{
			// GrabInteractableを有効化
			if (grabInteractables != null)
			{
				foreach (GrabInteractable grabInteractable in grabInteractables)
				{
					grabInteractable.enabled = true;
				}
			}
			// HandGrabInteractableを有効化
			if (handGrabInteractables != null)
			{
				foreach (HandGrabInteractable handGrabInteractable in handGrabInteractables)
				{
					handGrabInteractable.enabled = true;
				}
			}
			// grabbableを有効化
			if (grabbables != null)
			{
				foreach (Grabbable grabbable in grabbables)
				{
					grabbable.enabled = true;
				}
			}
		}
	}
}