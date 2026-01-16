using UnityEngine;

public class ToggleState : MonoBehaviour
{
    public bool isOn = true;

    [SerializeField] private GameObject[] toggleOnIconObject;
    [SerializeField] private GameObject[] toggleOffIconObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetIcon();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Toggle()
    {
        isOn = !isOn;
        SetIcon();
    }

    public void  SetOnOff(bool flg)
    {
        if (flg != isOn)
        {
            Toggle();
        }
    }

    private void SetIcon()
    {
        foreach (GameObject icon in toggleOnIconObject)
        {
            icon.SetActive(isOn);
        }
        foreach (GameObject icon in toggleOffIconObject)
        {
            icon.SetActive(!isOn);
        }
    }
}
