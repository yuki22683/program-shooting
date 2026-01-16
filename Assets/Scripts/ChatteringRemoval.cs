using UnityEngine;
using UnityEngine.UI;

public class ChatteringRemoval : MonoBehaviour
{
    private Button button;

    public const float CHATTERING_REMOVAL_TIME = 0.1f;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InactiveButton()
    {
        button.interactable = false;
        Invoke("ActivateButton", CHATTERING_REMOVAL_TIME);
    }

    private void ActivateButton()
    {
        button.interactable = true;
    }
}
