using UnityEngine;
using UnityEngine.UI;

public class ButtonFlgExclusiveManager : MonoBehaviour
{
    [SerializeField] private ToggleBool[] toggleBools;

    public const int BTN_GROUP_MAX = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleExclusive(int index)
	{
		if (toggleBools[index].flg == false)
        {
            toggleBools[index].flg = true;
			toggleBools[index].transform.GetComponent<UpdateAnimationController>().UpdateAnimatorController();

			for (int i = 0; i < toggleBools.Length; i++)
            {
                if (i != index)
                {
                    if (toggleBools[i].flg)
                    {
						toggleBools[i].flg = false;
						toggleBools[i].transform.GetComponent<UpdateAnimationController>().UpdateAnimatorController();
					}
                }
            }
        }
	}
}
