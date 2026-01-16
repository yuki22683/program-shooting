using UnityEngine;
using UnityEngine.UI;

public class StartButtonEnableChack : MonoBehaviour
{
    [SerializeField] private ButtonInputEmptyCheck[] emptyChecks; // Array of boolean values to check if any are true

	private Button button; // Reference to the button component

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		button = GetComponent<Button>(); // Get the button component attached to this GameObject
		button.interactable = false; // Disable the button initially
	}

    // Update is called once per frame
    void Update()
    {
		CheckEmptyChecks(); // Call the method to check the boolean values
	}

	private void CheckEmptyChecks()
	{
		foreach (var emptyCheck in emptyChecks)
		{
			if (emptyCheck.isEmpty)
			{
				// If any of the boolean values are true, enable the button
				button.interactable = false;
				return;
			}
		}
		button.interactable = true; // Enable the button if all boolean values are false
	}
}
