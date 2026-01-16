using UnityEngine;

public class UpdateAnimationController : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController firstAnimatorController;
	[SerializeField] private RuntimeAnimatorController secondAnimatorController;

    private Animator animator;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		UpdateAnimatorController();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAnimatorController()
	{
		animator = GetComponent<Animator>();
		bool flg = GetComponent<ToggleBool>().flg;

		if (flg)
		{
			animator.runtimeAnimatorController = secondAnimatorController;
		}
		else
		{
			animator.runtimeAnimatorController = firstAnimatorController;
		}
	}
}
