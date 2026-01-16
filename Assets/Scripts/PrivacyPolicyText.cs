using TMPro;
using UnityEngine;

public class PrivacyPolicyText : MonoBehaviour
{
    [SerializeField] private TMP_Text privacyPolicyText;
    [SerializeField] private RectTransform rectTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (privacyPolicyText == null)
        {
            privacyPolicyText = GetComponent<TMP_Text>();
        }

        if (LocalizationManager.Instance != null)
        {
            privacyPolicyText.font = LocalizationManager.Instance.GetLocalizedFont();
            privacyPolicyText.text = LocalizationManager.Instance.GetPrivacyPolicyText();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
