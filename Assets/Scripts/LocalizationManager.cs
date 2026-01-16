using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

public class LocalizationManager : MonoBehaviour
{
	public static string languageCode = "";
	public static LocalizationManager Instance { get; private set; }

	private Dictionary<string, Dictionary<string, string>> allLanguages;
	private Dictionary<string, Dictionary<string, string>> allLanguagesPrivacyPolicy;
	private Dictionary<string, string> localizedTexts;
	private Dictionary<string, string> localizedTextsPrivacyPolicy;

	[Header("TMP SDF Fonts")]
	[SerializeField] private TMP_FontAsset chineseFont;
	[SerializeField] private TMP_FontAsset koreanFont;
	[SerializeField] private TMP_FontAsset japaneseFont;
	[SerializeField] private TMP_FontAsset defaultFont;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
			LoadLocalizationData();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void LoadLocalizationData()
	{
		TextAsset jsonFile = Resources.Load<TextAsset>("localizedText");
		TextAsset jsonFilePrivacyPolicy = Resources.Load<TextAsset>("localizedPrivacyPolicyText");
		if (jsonFile == null)
		{
			Debug.LogError("localizedText.json not found in Resources folder!");
			return;
		}

		allLanguages = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonFile.text);
		allLanguagesPrivacyPolicy = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonFilePrivacyPolicy.text);

        if (DataManager.gameSettings.languageSettings.languageCode == "")
        {
            languageCode = GetLanguageCode(Application.systemLanguage);
        }
		else
		{
			languageCode = DataManager.gameSettings.languageSettings.languageCode;
		}

		if (!allLanguages.TryGetValue(languageCode, out localizedTexts))
		{
			Debug.LogWarning($"Language '{languageCode}' not found. Falling back to English.");
			localizedTexts = allLanguages.ContainsKey("en") ? allLanguages["en"] : new Dictionary<string, string>();
		}

		if (!allLanguagesPrivacyPolicy.TryGetValue(languageCode, out localizedTextsPrivacyPolicy))
		{
			Debug.LogWarning($"Language '{languageCode}' not found. Falling back to English.");
			localizedTextsPrivacyPolicy = allLanguagesPrivacyPolicy.ContainsKey("en") ? allLanguagesPrivacyPolicy["en"] : new Dictionary<string, string>();
		}
	}

	public string GetText(string key)
	{
		if (localizedTexts != null && localizedTexts.TryGetValue(key, out var value))
		{
			return value;
		}
		else
		{
			Debug.LogWarning($"Missing localization key: {key}");
			return key;
		}
	}

	public TMP_FontAsset GetLocalizedFont()
	{
		switch (Application.systemLanguage)
		{
			case SystemLanguage.ChineseSimplified:
			case SystemLanguage.ChineseTraditional:
				return chineseFont;

			case SystemLanguage.Korean:
				return koreanFont;

			case SystemLanguage.Japanese:
				return japaneseFont;

			default:
				return defaultFont;
		}
	}

	private string GetLanguageCode(SystemLanguage lang)
	{
		switch (lang)
		{
			case SystemLanguage.Czech: return "cs";
			case SystemLanguage.Danish: return "da";
			case SystemLanguage.Dutch: return "nl";
			case SystemLanguage.German: return "de";
			case SystemLanguage.Greek: return "el";
			case SystemLanguage.English: return "en";
			case SystemLanguage.Finnish: return "fi";
			case SystemLanguage.French: return "fr";
			case SystemLanguage.Italian: return "it";
			case SystemLanguage.Japanese: return "ja";
			case SystemLanguage.Korean: return "ko";
			case SystemLanguage.Norwegian: return "no";
			case SystemLanguage.Polish: return "pl";
			case SystemLanguage.Portuguese: return "pt";
			case SystemLanguage.Romanian: return "ro";
			case SystemLanguage.Russian: return "ru";
			case SystemLanguage.Spanish: return "es";
			case SystemLanguage.Swedish: return "sv";
			case SystemLanguage.Turkish: return "tr";
			case SystemLanguage.ChineseSimplified: return "zh-Hans";
			case SystemLanguage.ChineseTraditional: return "zh-Hant";
			default: return "en";
		}
	}

	public string GetPrivacyPolicyText()
	{
		if (localizedTextsPrivacyPolicy != null && localizedTextsPrivacyPolicy.TryGetValue("privacy_policy_body", out var value))
		{
			return value;
		}
		else
		{
			Debug.LogWarning($"Missing localization Privacy Policy");
			return "privacy policy";
		}
	}
}
