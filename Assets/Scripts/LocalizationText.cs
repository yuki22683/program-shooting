using UnityEngine;
using TMPro;

public class LocalizationText : MonoBehaviour
{
	[SerializeField] private TMP_Text localizedText;
	[SerializeField] private string key;

    private string _languageCode = "";

	void Start()
	{
		if (localizedText == null)
		{
			localizedText = GetComponent<TMP_Text>();
		}

		if (LocalizationManager.Instance != null)
		{
			localizedText.font = LocalizationManager.Instance.GetLocalizedFont();
			localizedText.text = LocalizationManager.Instance.GetText(key);
		}
		else
		{
			Debug.LogWarning("LocalizationManager.Instance is null");
		}
	}

    void Update()
    {
        if (LocalizationManager.languageCode != _languageCode)
        {
            _languageCode = LocalizationManager.languageCode;
			localizedText.font = LocalizationManager.Instance.GetLocalizedFont();
			localizedText.text = LocalizationManager.Instance.GetText(key);
        }
    }

    public void SetKey(string _key)
    {
		key = _key;
        localizedText.font = LocalizationManager.Instance.GetLocalizedFont();
		localizedText.text = LocalizationManager.Instance.GetText(_key);
    }
}
