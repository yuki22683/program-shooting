using UnityEngine;
using System.IO;
using System.Collections.Generic;
using static GameManager;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using Oculus.Platform.Models;
using System;
using System.Linq;

// 設定情報を表すクラス
[System.Serializable]
public class GameSettings
{
    public bool isPrivacyPolicyAccept = false;
    public ImmersiveSettings immersiveSettings = new ImmersiveSettings();
    public SoundSettings soundSettings = new SoundSettings();
    public LanguageSettings languageSettings = new LanguageSettings();
}

[System.Serializable]
public class ImmersiveSettings
{
    public int skyBoxId = 0;
    public bool isImmersive = false;
}

[System.Serializable]
public class SoundSettings
{
    public int bgmId = 0;
    public float bgmVolume = SOUND_VOLUME_MEDIUM; // BGM音量
    public bool isBgmMute = false;
    public float soundVolume = SOUND_VOLUME_MEDIUM; // BGM音量

    public bool isSoundMute = false;
}

[System.Serializable]
public class LanguageSettings
{
    public string languageCode = "";
}

// JSONファイルの管理クラス
public static class DataManager
{
    private static string settingsFilePath;
    private static string recordsFilePath;

    public static GameSettings gameSettings;

    static DataManager()
    {
        // ファイルパスの設定（プラットフォームに依存しない保存先）
        settingsFilePath = Path.Combine(Application.persistentDataPath, "settings.json");
        recordsFilePath = Path.Combine(Application.persistentDataPath, "records.json");
        LoadSettings();
    }

    // 設定情報を保存
    public static void SaveSettings()
    {
        string json = JsonUtility.ToJson(gameSettings, true); // 読みやすい形式でJSONに変換
        try
        {
            File.WriteAllText(settingsFilePath, json);
            Debug.Log("Settings saved to: " + settingsFilePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save settings: " + e.Message);
        }
    }

    // 設定情報を読み込み
    public static void LoadSettings()
    {
        if (File.Exists(settingsFilePath))
        {
            try
            {
                string json = File.ReadAllText(settingsFilePath);
                gameSettings = JsonUtility.FromJson<GameSettings>(json);
                Debug.Log("Settings loaded from: " + settingsFilePath);
                ParamGameSettingsCheck();
                SaveSettings();
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load settings: " + e.Message);
                gameSettings = new GameSettings();
                SaveSettings();
            }
        }
        else
        {
            Debug.LogWarning("Settings file not found, returning default settings.");
            gameSettings = new GameSettings();
            SaveSettings();
        }
    }

    private static void ParamGameSettingsCheck()
    {
        gameSettings.isPrivacyPolicyAccept = ParamPrivacyPolicyAcceptCheck(gameSettings.isPrivacyPolicyAccept);
        gameSettings.immersiveSettings = ParamImmersiveSettingsCheck(gameSettings.immersiveSettings);
        gameSettings.soundSettings = ParamSoundSettingsCheck(gameSettings.soundSettings);
        gameSettings.languageSettings = ParamLanguageSettingsCheck(gameSettings.languageSettings);
    }

    private static bool ParamPrivacyPolicyAcceptCheck(bool isPrivacyPolicyAccept)
    {
        return isPrivacyPolicyAccept;
    }

    private static ImmersiveSettings ParamImmersiveSettingsCheck(ImmersiveSettings immersiveSettings)
    {
        if ((immersiveSettings.skyBoxId < 0)
        || (immersiveSettings.skyBoxId >= SKYBOX_ID_MAX))
        {
            immersiveSettings.skyBoxId = 0;
        }
        return immersiveSettings;
    }

    private static SoundSettings ParamSoundSettingsCheck(SoundSettings soundSettings)
    {
        if ((soundSettings.bgmVolume < 0)
        || (soundSettings.bgmVolume > 1.0))
        {
            soundSettings.bgmVolume = SOUND_VOLUME_MEDIUM;
        }
        if ((soundSettings.soundVolume < 0)
        || (soundSettings.soundVolume > 1.0))
        {
            soundSettings.soundVolume = SOUND_VOLUME_MEDIUM;
        }
        return soundSettings;
    }

    private static LanguageSettings ParamLanguageSettingsCheck(LanguageSettings languageSettings)
    {
        return languageSettings;
    }
}