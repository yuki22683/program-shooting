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
    public ChallengeModeSettings challengeModeSettings = new ChallengeModeSettings();
    public AbacusSettings abacusSettings = new AbacusSettings();
    public ControlSettings controlSettings = new ControlSettings();
    public ColorSettings colorSettings = new ColorSettings();
    public ImmersiveSettings immersiveSettings = new ImmersiveSettings();
    public SoundSettings soundSettings = new SoundSettings();
    public LanguageSettings languageSettings = new LanguageSettings();
}

[System.Serializable]
public class ChallengeModeSettings
{
    public DigitRangeInfo digitRangeInfo = new DigitRangeInfo();

    public ChallengeModeSettings()
    {
        digitRangeInfo.easy.basic.num1.min = 1;
        digitRangeInfo.easy.basic.num1.max = 2;
        digitRangeInfo.easy.basic.num2.min = 1;
        digitRangeInfo.easy.basic.num2.max = 2;
        digitRangeInfo.easy.add.num1.min = 1;
        digitRangeInfo.easy.add.num1.max = 2;
        digitRangeInfo.easy.add.num2.min = 1;
        digitRangeInfo.easy.add.num2.max = 2;
        digitRangeInfo.easy.sub.num1.min = 1;
        digitRangeInfo.easy.sub.num1.max = 2;
        digitRangeInfo.easy.sub.num2.min = 1;
        digitRangeInfo.easy.sub.num2.max = 2;
        digitRangeInfo.easy.multi.num1.min = 2;
        digitRangeInfo.easy.multi.num1.max = 2;
        digitRangeInfo.easy.multi.num2.min = 1;
        digitRangeInfo.easy.multi.num2.max = 1;
        digitRangeInfo.easy.divide.num1.min = 2;
        digitRangeInfo.easy.divide.num1.max = 2;
        digitRangeInfo.easy.divide.num2.min = 1;
        digitRangeInfo.easy.divide.num2.max = 1;
        digitRangeInfo.medium.basic.num1.min = 3;
        digitRangeInfo.medium.basic.num1.max = 4;
        digitRangeInfo.medium.basic.num2.min = 3;
        digitRangeInfo.medium.basic.num2.max = 4;
        digitRangeInfo.medium.add.num1.min = 3;
        digitRangeInfo.medium.add.num1.max = 4;
        digitRangeInfo.medium.add.num2.min = 3;
        digitRangeInfo.medium.add.num2.max = 4;
        digitRangeInfo.medium.sub.num1.min = 3;
        digitRangeInfo.medium.sub.num1.max = 4;
        digitRangeInfo.medium.sub.num2.min = 3;
        digitRangeInfo.medium.sub.num2.max = 4;
        digitRangeInfo.medium.multi.num1.min = 3;
        digitRangeInfo.medium.multi.num1.max = 3;
        digitRangeInfo.medium.multi.num2.min = 2;
        digitRangeInfo.medium.multi.num2.max = 2;
        digitRangeInfo.medium.divide.num1.min = 3;
        digitRangeInfo.medium.divide.num1.max = 3;
        digitRangeInfo.medium.divide.num2.min = 1;
        digitRangeInfo.medium.divide.num2.max = 1;
        digitRangeInfo.hard.basic.num1.min = 5;
        digitRangeInfo.hard.basic.num1.max = 6;
        digitRangeInfo.hard.basic.num2.min = 5;
        digitRangeInfo.hard.basic.num2.max = 6;
        digitRangeInfo.hard.add.num1.min = 5;
        digitRangeInfo.hard.add.num1.max = 6;
        digitRangeInfo.hard.add.num2.min = 5;
        digitRangeInfo.hard.add.num2.max = 6;
        digitRangeInfo.hard.sub.num1.min = 5;
        digitRangeInfo.hard.sub.num1.max = 6;
        digitRangeInfo.hard.sub.num2.min = 5;
        digitRangeInfo.hard.sub.num2.max = 6;
        digitRangeInfo.hard.multi.num1.min = 4;
        digitRangeInfo.hard.multi.num1.max = 4;
        digitRangeInfo.hard.multi.num2.min = 3;
        digitRangeInfo.hard.multi.num2.max = 3;
        digitRangeInfo.hard.divide.num1.min = 4;
        digitRangeInfo.hard.divide.num1.max = 4;
        digitRangeInfo.hard.divide.num2.min = 1;
        digitRangeInfo.hard.divide.num2.max = 1;
    }
}

[System.Serializable]
public class AbacusSettings
{
    public int digitCout = 7;
    public bool isRodValueVisible = true;
}

[System.Serializable]
public class ControlSettings
{
    public float controlDebounceTime = COUNTROL_DEBOUNCE_TIME_MEDIUM;
}

[System.Serializable]
public class ColorSettings
{
    public BeadColors[] beadsColors = new BeadColors[ROD_COUNT];
    public Color[] rodColors = new Color[ROD_COUNT];
    public Color frameColor = new Color(FRAME_DEFAULT_COLOR_R, FRAME_DEFAULT_COLOR_G, FRAME_DEFAULT_COLOR_B);
    public Color touchColor = Color.red;
    public Color debounceColor = Color.grey;
    public Color activeColor = Color.yellow;

    public ColorSettings()
    {
        beadsColors = new BeadColors[ROD_COUNT];

        for (int i = 0; i < ROD_COUNT; i++)
        {
            beadsColors[i] = new BeadColors();
            rodColors[i] = new Color(ROD_DEFAULT_COLOR_R, ROD_DEFAULT_COLOR_G, ROD_DEFAULT_COLOR_B);
        }
    }
}

[System.Serializable]
public class BeadColors
{
    public Color[] colors = new Color[BEADS_COUNT];

    public BeadColors()
    {
        colors = new Color[BEADS_COUNT];
        
        for (int i = 0; i < BEADS_COUNT; i++)
        {
            colors[i] = new Color(BEAD_DEFAULT_COLOR_R, BEAD_DEFAULT_COLOR_G, BEAD_DEFAULT_COLOR_B);
        }
    }
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

public enum OperationType
{
    None = 0,
    Custom_Add = 1,
    Custom_Sub = 2,
    Custom_Add_Sub = 3,
    Custom_Mul = 4,
    Custom_Add_Mul = 5,
    Custom_Sub_Mul = 6,
    Custom_Add_Sub_Mul = 7,
    Custom_Div = 8,
    Custom_Add_Div = 9,
    Custom_Sub_Div = 10,
    Custom_Add_Sub_Div = 11,
    Custom_Mul_Div = 12,
    Custom_Add_Mul_Div = 13,
    Custom_Sub_Mul_Div = 14,
    Custom_ALL = 15,
    Basic_Read = 16,
    Basic_Write = 17,
    Addition_Normal = 18,
    Addition_Chain = 19,
    Addition_Flash = 20,
    Subtraction = 21,
    Multiplication = 22,
    Division = 23,
    Num = 24
}

[System.Serializable]
public struct RecordKey
{
    public OperationType operationType;
    public ChallengeDifficultyType difficulty;
    public int questionCount;

    public RecordKey(OperationType opType, ChallengeDifficultyType diff, int count)
    {
        operationType = opType;
        difficulty = diff;
        questionCount = count;
    }
}

// 単一の記録を表すクラス
[System.Serializable]
public class GameRecord
{
    public string completionDateTime; // 日時
    public string completionTime = "00:00"; // 完了時間（MM:SS形式）

    public GameRecord(string time, string dateTime)
    {
        completionTime = time;
        completionDateTime = dateTime;
    }
}
[System.Serializable]
public class SerializableRecordEntry
{
    public RecordKey key;
    public List<GameRecord> records;
}

[System.Serializable]
public class GameRecords
{
    [SerializeField]
    private List<SerializableRecordEntry> serializedRecords = new List<SerializableRecordEntry>();
    private Dictionary<RecordKey, List<GameRecord>> recordsByCategory = new Dictionary<RecordKey, List<GameRecord>>();

    // 記録を追加
    public void AddRecord(OperationType operationType, ChallengeDifficultyType difficulty, int questionCount, string completionTime)
    {
        var key = new RecordKey(operationType, difficulty, questionCount);
        if (!recordsByCategory.ContainsKey(key))
        {
            recordsByCategory[key] = new List<GameRecord>();
        }
        recordsByCategory[key].Add(new GameRecord(completionTime, DateTime.Now.ToString("yyyy-MM-dd")));
        UpdateSerializedRecords();
    }

    

    // 指定キーのレコードリストを削除
    public bool DeleteRecords(OperationType operationType, ChallengeDifficultyType difficulty, int questionCount)
    {
        var key = new RecordKey(operationType, difficulty, questionCount);
        if (recordsByCategory.ContainsKey(key))
        {
            recordsByCategory.Remove(key);
            UpdateSerializedRecords();
            return true; // 削除成功
        }
        Debug.LogWarning($"No records found for key: {operationType}, {difficulty}, {questionCount}");
        return false; // 削除対象が存在しない
    }
    // 指定順位のレコードを取得
    public GameRecord GetRecordByRank(OperationType operationType, ChallengeDifficultyType difficulty, int questionCount, int rank)
    {
        if (rank <= 0)
        {
            Debug.LogWarning($"Invalid rank: {rank}. Rank must be 1 or greater.");
            return null;
        }

        var records = GetRecords(operationType, difficulty, questionCount);
        if (records.Count == 0)
        {
            Debug.LogWarning($"No records found for key: {operationType}, {difficulty}, {questionCount}");
            return null;
        }

        // completionTimeでソート
        var sortedRecords = records.OrderBy(record =>
        {
            return IsValidTimeFormat(record.completionTime) ? ConvertToSeconds(record.completionTime) : float.MaxValue;
        }).ToList();

        if (rank > sortedRecords.Count)
        {
            Debug.LogWarning($"Rank {rank} exceeds the number of records ({sortedRecords.Count}) for key: {operationType}, {difficulty}, {questionCount}");
            return null;
        }

        return sortedRecords[rank - 1]; // 1-based rankを0-basedインデックスに変換
    }
    // シリアライズ用リストを更新
    private void UpdateSerializedRecords()
    {
        serializedRecords.Clear();
        foreach (var pair in recordsByCategory)
        {
            serializedRecords.Add(new SerializableRecordEntry { key = pair.Key, records = pair.Value });
        }
    }

    // デシリアライズ時に辞書を復元
    public void RestoreFromSerialized()
    {
        recordsByCategory.Clear();
        foreach (var entry in serializedRecords)
        {
            recordsByCategory[entry.key] = entry.records;
        }
    }

    // 特定カテゴリの記録を取得
    public List<GameRecord> GetRecords(OperationType operationType, ChallengeDifficultyType difficulty, int questionCount)
    {
        var key = new RecordKey(operationType, difficulty, questionCount);
        return recordsByCategory.ContainsKey(key) ? recordsByCategory[key] : new List<GameRecord>();
    }

    // ハイスコアを返す
    public string GetHighScore(OperationType operationType, ChallengeDifficultyType difficulty, int questionCount)
    {
        var records = GetRecords(operationType, difficulty, questionCount);
        if (records.Count == 0)
        {
            return null; // 記録がない場合は null を返す
        }

        string highScore = null;
        float minTimeInSeconds = float.MaxValue;

        foreach (var record in records)
        {
            if (IsValidTimeFormat(record.completionTime))
            {
                float timeInSeconds = ConvertToSeconds(record.completionTime);
                if (timeInSeconds < minTimeInSeconds)
                {
                    minTimeInSeconds = timeInSeconds;
                    highScore = record.completionTime;
                }
            }
        }

        return highScore;
    }

    // completionTime の順位を計算
    public int GetRank(OperationType operationType, ChallengeDifficultyType difficulty, int questionCount, string completionTime)
    {
        var records = GetRecords(operationType, difficulty, questionCount);
        if (string.IsNullOrEmpty(completionTime) || !IsValidTimeFormat(completionTime))
        {
            return records.Count + 1; // 無効な時間は最下位
        }

        float newTimeInSeconds = ConvertToSeconds(completionTime);
        int rank = 1;

        foreach (var record in records)
        {
            if (IsValidTimeFormat(record.completionTime))
            {
                float recordTimeInSeconds = ConvertToSeconds(record.completionTime);
                if (recordTimeInSeconds < newTimeInSeconds)
                {
                    rank++;
                }
            }
        }

        return rank;
    }

    // MM:SS 形式を秒に変換
    private float ConvertToSeconds(string time)
    {
        if (!IsValidTimeFormat(time)) return float.MaxValue;
        string[] parts = time.Split(':');
        int minutes = int.Parse(parts[0]);
        int seconds = int.Parse(parts[1]);
        return minutes * 60f + seconds;
    }

    // MM:SS 形式の検証
    private bool IsValidTimeFormat(string time)
    {
        if (string.IsNullOrEmpty(time)) return false;
        string[] parts = time.Split(':');
        if (parts.Length != 2) return false;
        return int.TryParse(parts[0], out int minutes) && int.TryParse(parts[1], out int seconds) &&
               minutes >= 0 && seconds >= 0 && seconds < 60;
    }

    public void ClearRecords()
    {
        recordsByCategory.Clear();
        serializedRecords.Clear();
    }
}

// JSONファイルの管理クラス
public static class DataManager
{
    private static string settingsFilePath;
    private static string recordsFilePath;
    private  static GameRecords gameRecords;

    public static GameSettings gameSettings;

    static DataManager()
    {
        // ファイルパスの設定（プラットフォームに依存しない保存先）
        settingsFilePath = Path.Combine(Application.persistentDataPath, "settings.json");
        recordsFilePath = Path.Combine(Application.persistentDataPath, "records.json");
        gameRecords = new GameRecords(); // 初期化
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

    // 新しい記録を追加して保存
    public static int AddAndSaveRecord(OperationType operationType, ChallengeDifficultyType difficulty, int questionCount, string completionTime)
    {
        // 既存の記録を読み込む
        gameRecords = LoadRecords();

        // 新しい記録の順位を計算
        int rank = gameRecords.GetRank(operationType, difficulty, questionCount, completionTime);

        // 記録をリストに追加
        gameRecords.AddRecord(operationType, difficulty, questionCount, completionTime);

        // 記録を保存
        string json = JsonUtility.ToJson(gameRecords, true);
        try
        {
            File.WriteAllText(recordsFilePath, json);
            Debug.Log($"New record added and saved to: {recordsFilePath}, JSON: {json}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save records: {e.Message}");
        }
        return rank;
    }

    public static bool DeleteAndSaveRecords(OperationType operationType, ChallengeDifficultyType difficulty, int questionCount)
    {
        gameRecords = LoadRecords();
        bool deleted = gameRecords.DeleteRecords(operationType, difficulty, questionCount);

        if (deleted)
        {
            string json = JsonUtility.ToJson(gameRecords, true);
            try
            {
                File.WriteAllText(recordsFilePath, json);
                Debug.Log($"Records deleted and saved to: {recordsFilePath}, JSON: {json}");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save records after deletion: {e.Message}");
                return false;   
            }
        }
        return false; // 削除対象が存在しなかった
    }

    public static string GetHighScore(OperationType operationType, ChallengeDifficultyType difficulty, int questionCount)
    {
        gameRecords = LoadRecords();

        return gameRecords.GetHighScore(operationType, difficulty, questionCount);
    }

    // 指定順位のレコードを取得
    public static GameRecord GetRecordByRank(OperationType operationType, ChallengeDifficultyType difficulty, int questionCount, int rank)
    {
        gameRecords = LoadRecords();
        return gameRecords.GetRecordByRank(operationType, difficulty, questionCount, rank);
    }

    public static OperationType GetCustomOperationType(ChallengeCustomInfo challengeCustomInfo)
    {
        int operationCount = 0;

        if (challengeCustomInfo.add)
        {
            operationCount += 1 << 0;
        }
        if (challengeCustomInfo.sub)
        {
            operationCount += 1 << 1;
        }
        if (challengeCustomInfo.multi)
        {
            operationCount += 1 << 2;
        }
        if (challengeCustomInfo.divide)
        {
            operationCount += 1 << 3;
        }
        return (OperationType)operationCount;
    }

    private static void ParamGameSettingsCheck()
    {
        gameSettings.challengeModeSettings = PraramChallengeModeSettinsCheck(gameSettings.challengeModeSettings);
    }

    private static ChallengeModeSettings PraramChallengeModeSettinsCheck(ChallengeModeSettings challengeModeSettings)
    {
        challengeModeSettings.digitRangeInfo = ParamDigitRangeCheck(challengeModeSettings.digitRangeInfo);

        return challengeModeSettings;
    }

    private static DigitRangeInfo ParamDigitRangeCheck(DigitRangeInfo digitRange)
    {
        digitRange.easy = ParamDifficultyTypeDigitRangeCheck(digitRange.easy);
        digitRange.medium = ParamDifficultyTypeDigitRangeCheck(digitRange.medium);
        digitRange.hard = ParamDifficultyTypeDigitRangeCheck(digitRange.hard);

        return digitRange;
    }

    private static DifficultyTypeDigitRange ParamDifficultyTypeDigitRangeCheck(DifficultyTypeDigitRange difficultyTypeDigitRange)
    {
        difficultyTypeDigitRange.basic = ParamArithmeticDigitRangeCheck(difficultyTypeDigitRange.basic, ArithmeticType.None);
        difficultyTypeDigitRange.add = ParamArithmeticDigitRangeCheck(difficultyTypeDigitRange.add, ArithmeticType.Addition);
        difficultyTypeDigitRange.sub = ParamArithmeticDigitRangeCheck(difficultyTypeDigitRange.sub, ArithmeticType.Subtraction);
        difficultyTypeDigitRange.multi = ParamArithmeticDigitRangeCheck(difficultyTypeDigitRange.multi, ArithmeticType.Multiplication);
        difficultyTypeDigitRange.divide = ParamArithmeticDigitRangeCheck(difficultyTypeDigitRange.divide, ArithmeticType.Division);

        return difficultyTypeDigitRange;
    }

    private static ArithmeticDigitRange ParamArithmeticDigitRangeCheck(ArithmeticDigitRange arithmeticDigitRange, ArithmeticType arithmeticType)
    {
        arithmeticDigitRange.num1 = ParamDigitRangeCheck(arithmeticDigitRange.num1);
        arithmeticDigitRange.num2 = ParamDigitRangeCheck(arithmeticDigitRange.num2);

        if (arithmeticType == ArithmeticType.Division)
        {
            if ((arithmeticDigitRange.num1.min > arithmeticDigitRange.num2.min)
            || (arithmeticDigitRange.num1.max > arithmeticDigitRange.num2.max))
            {
                arithmeticDigitRange.num1.min = DIGIT_RANGE_DIVISION_NUM1_MIN_DEF;
                arithmeticDigitRange.num1.max = DIGIT_RANGE_DIVISION_NUM1_MAX_DEF;
                arithmeticDigitRange.num2.min = DIGIT_RANGE_DIVISION_NUM2_MIN_DEF;
                arithmeticDigitRange.num2.max = DIGIT_RANGE_DIVISION_NUM2_MAX_DEF;
            }
        }
        return arithmeticDigitRange;
    }

    private static DigitRange ParamDigitRangeCheck(DigitRange digitRange)
    {
        if ((digitRange.min < 1)
        || (digitRange.max > ROD_COUNT)
        || (digitRange.min > digitRange.max))
        {
            digitRange.min = DIGIT_RANGE_MIN_DEF;
            digitRange.max = DIGIT_RANGE_MAX_DEF;
        }

        return digitRange;
    }

    private static AbacusSettings PraramAbacusSettingsCheck(AbacusSettings abacusSettings)
    {
        return abacusSettings;
    }

    private static ColorSettings PraramColorSettingsCheck(ColorSettings colorSettings)
    {
        return colorSettings;
    }

    private static ImmersiveSettings PraramImmersiveSettingsCheck(ImmersiveSettings immersiveSettings)
    {
        return immersiveSettings;
    }

    private static SoundSettings PraramSoundSettingsCheck(SoundSettings soundSettings)
    {
        return soundSettings;
    }

    private static LanguageSettings PraramLanguageSettingsCheck(LanguageSettings languageSettings)
    {
        return languageSettings;
    }
    // 全ての記録を読み込み
    public static GameRecords LoadRecords()
    {
        if (File.Exists(recordsFilePath))
        {
            try
                {
                string json = File.ReadAllText(recordsFilePath);
                if (string.IsNullOrWhiteSpace(json) || json == "{}")
                {
                    Debug.LogWarning("Records file is empty or invalid, returning new GameRecords.");
                    return new GameRecords();
                }
                GameRecords records = JsonUtility.FromJson<GameRecords>(json);
                if (records != null)
                {
                    records.RestoreFromSerialized(); // 辞書を復元
                    Debug.Log($"Records loaded from: {recordsFilePath}");
                    return records;
                }
                else
                {
                    Debug.LogWarning("Failed to deserialize records, returning new GameRecords.");
                    return new GameRecords();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load records: {e.Message}");
                return new GameRecords();
            }
        }
        else
        {
            Debug.LogWarning("Records file not found, returning new GameRecords.");
            return new GameRecords();
        }
    }

    // 記録をクリアして保存
    public static void ClearRecords()
    {
        GameRecords emptyRecords = new GameRecords();
        string json = JsonUtility.ToJson(emptyRecords, true);
        try
        {
            File.WriteAllText(recordsFilePath, json);
            Debug.Log($"Records cleared and saved to: {recordsFilePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to clear records: {e.Message}");
        }
        gameRecords = emptyRecords; // 現在のインスタンスを更新
    }
}