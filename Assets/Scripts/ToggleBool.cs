using UnityEngine;

public class ToggleBool : MonoBehaviour
{
    public bool flg = false;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Toggle()
	{
		flg = !flg;
	}
}
