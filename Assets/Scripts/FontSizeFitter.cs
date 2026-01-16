using UnityEngine;

public class FontSizeFitter : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private float minFontSize;
    [SerializeField] private float maxFontSize;

    [SerializeField] private int minStringLength;
    [SerializeField] private int maxStringLength;

    private int _stringLength;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateFontSize();
    }

    // Update is called once per frame
    void Update()
    {
        if (_stringLength != textMeshProUGUI.text.Length)
        {
            UpdateFontSize();
        }
    }

    private void UpdateFontSize()
    {
        _stringLength = textMeshProUGUI.text.Length;

        float fontSize = Mathf.Lerp(maxFontSize, minFontSize, (float)(_stringLength - minStringLength) / (maxStringLength - minStringLength));
        textMeshProUGUI.fontSize = fontSize;
    }
}
