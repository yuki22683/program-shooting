using UnityEngine;

public class ButtonInputEmptyCheck : MonoBehaviour
{
    [SerializeField] private ToggleBool[] toggleBools;

    public bool isEmpty = true;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		EmptyCheck();
	}

    private void EmptyCheck()
    {
		foreach (var toggleBool in toggleBools)
		{
			if (toggleBool.flg)
			{
				isEmpty = false;
				return;
			}
		}
		isEmpty = true;
	}
}
