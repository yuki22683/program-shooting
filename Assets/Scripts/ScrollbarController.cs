using UnityEngine;
using UnityEngine.UI;


public class ScrollbarController : MonoBehaviour
{
	[SerializeField] float scrollSpeed;
	private Scrollbar scrollbar;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		scrollbar = GetComponent<Scrollbar>();
	}

	// Update is called once per frame
	void Update()
	{
		if (OVRInput.Get(OVRInput.RawButton.RThumbstickUp)
			|| OVRInput.Get(OVRInput.RawButton.LThumbstickUp))
		{
			if (scrollbar.value < (1.0f - scrollSpeed))
			{
				scrollbar.value += scrollSpeed;
			}
			else
			{
				scrollbar.value = 1.0f;
			}
		}
		else if (OVRInput.Get(OVRInput.RawButton.RThumbstickDown)
			|| OVRInput.Get(OVRInput.RawButton.LThumbstickDown))
		{
			if (scrollbar.value > scrollSpeed)
			{
				scrollbar.value -= scrollSpeed;
			}
			else
			{
				scrollbar.value = 0.0f;
			}
		}
	}
}
