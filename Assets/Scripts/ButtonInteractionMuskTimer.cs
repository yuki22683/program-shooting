using UnityEngine.UI;
using UnityEngine;

public class ButtonInteractionMuskTimer : MonoBehaviour
{
    private Button button;

    private float timer = 0.0f;

    private const float TOGGLE_MUSK_TIMER = 0.1f; // 100ms

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        button = GetComponent<Button>();
	}

    // Update is called once per frame
    void Update()
    {
		CheckButtonInteractionMuskTimer();
	}

    public void SetButtonInteractionMuskTimer()
	{
        timer = Time.time;
	}

    private void CheckButtonInteractionMuskTimer()
    {
        if ((button.interactable == false)
            && (Time.time > timer + TOGGLE_MUSK_TIMER))
        {
            button.interactable = true;
		}
    }
}
