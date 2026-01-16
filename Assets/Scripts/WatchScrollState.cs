using UnityEngine;
using UnityEngine.UI;

public class WatchScrollState : MonoBehaviour
{
	[SerializeField] private Scrollbar scrollbar;

	//private Toggle toggle;
	private Button button;

	private bool flg = false;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		button = GetComponent<Button>();
		button.interactable = false;

		Invoke("FlgOn", 1f);
	}

	// Update is called once per frame
	void Update()
	{
		if ((scrollbar.value <= 0.01f) && (flg == true))
		{
			button.interactable = true;
		}
	}

	private void FlgOn()
	{
		flg = true;
	}
}
