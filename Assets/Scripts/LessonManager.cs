using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using Meta.XR.MRUtilityKit;

[Serializable]
public class LocalizedComment
{
    [Tooltip("Line index where comment will be inserted (0 = first line)")]
    public int lineIndex;
    [Tooltip("Comment prefix for the programming language (e.g., '#', '//', '/*')")]
    public string commentPrefix = "#";
    [Tooltip("Localization key for the comment text")]
    public string localizationKey;
}

[Serializable]
public class Exercise
{
    public string titleKey;
    [TextArea(3, 10)]
    public List<string> correctLines = new List<string>();
    public List<LocalizedComment> comments = new List<LocalizedComment>();
}

[Serializable]
public class Lesson
{
    public string titleKey;
    public List<Exercise> exercises = new List<Exercise>();
}

public class LessonManager : MonoBehaviour
{
    [Header("Lesson Data")]
    [SerializeField] private List<Lesson> lessons = new List<Lesson>();
    [SerializeField] private int currentLessonIndex = 0;
    [SerializeField] private int currentExerciseIndex = 0;
    [SerializeField] private string currentLanguage = "python";

    [Header("UI References")]
    [SerializeField] private Transform sheetPanelContent;

    [Header("Block Spawning")]
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private Transform blockSpawnParent;
    [SerializeField] private Vector3 spawnAreaCenter = new Vector3(0, 1.5f, 2f);
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(3f, 1f, 2f);
    [SerializeField] private AudioSource blockDestroySound;
    [SerializeField] private AudioSource wrongSelectionSound;

    private List<TextMeshProUGUI> lineTexts = new List<TextMeshProUGUI>();
    private List<GameObject> spawnedBlocks = new List<GameObject>();
    private List<LocalizationText> lineLocalizationTexts = new List<LocalizationText>();
    private string cachedLanguageCode = "";

    // Track display lines and visibility
    private List<string> currentDisplayLines = new List<string>();
    private List<bool> isCommentLine = new List<bool>();
    private int visibleLineCount = 0;

    // Track block selection order
    private int currentBlockIndex = 0;

    // Track collected tokens for display
    private List<string> currentCodeTokens = new List<string>();
    private List<string> collectedTokens = new List<string>();
    private int firstCodeLineIndex = -1;

    private void Awake()
    {
        FindLineTexts();
    }

    private void Start()
    {
        StartCoroutine(WaitForLocalizationAndDisplay());
    }

    private IEnumerator WaitForLocalizationAndDisplay()
    {
        while (LocalizationManager.Instance == null || string.IsNullOrEmpty(LocalizationManager.languageCode))
        {
            yield return null;
        }

        cachedLanguageCode = LocalizationManager.languageCode;
        DisplayCurrentExercise();

        // Wait 3 seconds after scene start before spawning blocks
        yield return new WaitForSeconds(3f);
        SpawnBlocksForFirstCodeLine();
    }

    private void Update()
    {
        if (LocalizationManager.languageCode != cachedLanguageCode)
        {
            cachedLanguageCode = LocalizationManager.languageCode;
            DisplayCurrentExercise();
        }
    }

    private void FindLineTexts()
    {
        lineTexts.Clear();
        lineLocalizationTexts.Clear();

        if (sheetPanelContent == null)
        {
            Debug.LogError("SheetPanelContent is not assigned!");
            return;
        }

        for (int i = 0; i < sheetPanelContent.childCount; i++)
        {
            Transform child = sheetPanelContent.GetChild(i);
            if (child.name.StartsWith("Line"))
            {
                Transform instruction = child.Find("Instruction");
                if (instruction != null)
                {
                    TextMeshProUGUI tmp = instruction.GetComponent<TextMeshProUGUI>();
                    if (tmp != null)
                    {
                        lineTexts.Add(tmp);

                        // Also collect LocalizationText component (may be null)
                        LocalizationText locText = instruction.GetComponent<LocalizationText>();
                        lineLocalizationTexts.Add(locText);
                    }
                }
            }
        }

        Debug.Log($"Found {lineTexts.Count} line text components");
    }

    public void DisplayCurrentExercise()
    {
        if (lessons.Count == 0)
        {
            Debug.LogWarning("No lessons configured!");
            return;
        }

        if (currentLessonIndex < 0 || currentLessonIndex >= lessons.Count)
        {
            Debug.LogError($"Invalid lesson index: {currentLessonIndex}");
            return;
        }

        Lesson currentLesson = lessons[currentLessonIndex];

        if (currentLesson.exercises.Count == 0)
        {
            Debug.LogWarning("Current lesson has no exercises!");
            return;
        }

        if (currentExerciseIndex < 0 || currentExerciseIndex >= currentLesson.exercises.Count)
        {
            Debug.LogError($"Invalid exercise index: {currentExerciseIndex}");
            return;
        }

        Exercise currentExercise = currentLesson.exercises[currentExerciseIndex];
        DisplayExercise(currentExercise);
    }

    private void DisplayExercise(Exercise exercise)
    {
        currentDisplayLines = BuildDisplayLines(exercise);

        // Find the first comment line index to determine initial visible count
        visibleLineCount = 0;
        for (int i = 0; i < isCommentLine.Count; i++)
        {
            if (isCommentLine[i])
            {
                visibleLineCount = i + 1; // Show up to and including the first comment
                break;
            }
        }

        // If no comment found, show nothing
        if (visibleLineCount == 0 && currentDisplayLines.Count > 0)
        {
            visibleLineCount = 0;
        }

        UpdateLineDisplay();

        Debug.Log($"Displayed exercise: {exercise.titleKey} with {currentDisplayLines.Count} lines, showing {visibleLineCount} initially");
    }

    private void UpdateLineDisplay()
    {
        // Get the appropriate font for current language
        TMP_FontAsset localizedFont = null;
        if (LocalizationManager.Instance != null)
        {
            localizedFont = LocalizationManager.Instance.GetLocalizedFont();
        }

        for (int i = 0; i < lineTexts.Count; i++)
        {
            // Disable LocalizationText component to prevent it from overwriting our text
            if (i < lineLocalizationTexts.Count && lineLocalizationTexts[i] != null)
            {
                lineLocalizationTexts[i].enabled = false;
            }

            // Set the localized font
            if (localizedFont != null)
            {
                lineTexts[i].font = localizedFont;
            }

            // Only show lines up to visibleLineCount
            if (i < visibleLineCount && i < currentDisplayLines.Count)
            {
                // Apply syntax highlighting
                lineTexts[i].text = ApplySyntaxHighlighting(currentDisplayLines[i]);
            }
            else
            {
                lineTexts[i].text = "";
            }
        }
    }

    /// <summary>
    /// Reveals the next line in the exercise
    /// </summary>
    public void RevealNextLine()
    {
        if (visibleLineCount < currentDisplayLines.Count)
        {
            visibleLineCount++;
            UpdateLineDisplay();
            Debug.Log($"Revealed line {visibleLineCount} of {currentDisplayLines.Count}");
        }
    }

    /// <summary>
    /// Reveals all remaining lines in the exercise
    /// </summary>
    public void RevealAllLines()
    {
        visibleLineCount = currentDisplayLines.Count;
        UpdateLineDisplay();
        Debug.Log($"Revealed all {currentDisplayLines.Count} lines");
    }

    /// <summary>
    /// Gets whether all lines are currently visible
    /// </summary>
    public bool AreAllLinesVisible => visibleLineCount >= currentDisplayLines.Count;

    /// <summary>
    /// Gets the current visible line count
    /// </summary>
    public int VisibleLineCount => visibleLineCount;

    private List<string> BuildDisplayLines(Exercise exercise)
    {
        List<string> result = new List<string>();
        isCommentLine.Clear();

        Debug.Log($"BuildDisplayLines: correctLines={exercise.correctLines.Count}, comments={exercise.comments.Count}");

        Dictionary<int, LocalizedComment> commentMap = new Dictionary<int, LocalizedComment>();
        foreach (var comment in exercise.comments)
        {
            commentMap[comment.lineIndex] = comment;
            Debug.Log($"Comment at line {comment.lineIndex}: prefix='{comment.commentPrefix}', key='{comment.localizationKey}'");
        }

        int codeIndex = 0;
        int lineIndex = 0;

        while (codeIndex < exercise.correctLines.Count || commentMap.ContainsKey(lineIndex))
        {
            if (commentMap.TryGetValue(lineIndex, out LocalizedComment comment))
            {
                string commentText = GetLocalizedText(comment.localizationKey);
                string fullComment = $"{comment.commentPrefix} {commentText}";
                result.Add(fullComment);
                isCommentLine.Add(true);
                Debug.Log($"Line {lineIndex}: Added comment '{fullComment}'");
            }
            else if (codeIndex < exercise.correctLines.Count)
            {
                result.Add(exercise.correctLines[codeIndex]);
                isCommentLine.Add(false);
                Debug.Log($"Line {lineIndex}: Added code '{exercise.correctLines[codeIndex]}'");
                codeIndex++;
            }

            lineIndex++;

            if (lineIndex > 100) break;
        }

        return result;
    }

    private string GetLocalizedText(string key)
    {
        if (LocalizationManager.Instance != null)
        {
            return LocalizationManager.Instance.GetText(key);
        }
        return key;
    }

    public void NextExercise()
    {
        if (lessons.Count == 0) return;

        Lesson currentLesson = lessons[currentLessonIndex];
        currentExerciseIndex++;

        if (currentExerciseIndex >= currentLesson.exercises.Count)
        {
            currentExerciseIndex = 0;
            NextLesson();
        }
        else
        {
            DisplayCurrentExercise();
        }
    }

    public void PreviousExercise()
    {
        if (lessons.Count == 0) return;

        currentExerciseIndex--;

        if (currentExerciseIndex < 0)
        {
            PreviousLesson();
            if (currentLessonIndex >= 0 && currentLessonIndex < lessons.Count)
            {
                currentExerciseIndex = lessons[currentLessonIndex].exercises.Count - 1;
            }
        }

        DisplayCurrentExercise();
    }

    public void NextLesson()
    {
        currentLessonIndex++;
        if (currentLessonIndex >= lessons.Count)
        {
            currentLessonIndex = 0;
        }
        currentExerciseIndex = 0;
        DisplayCurrentExercise();
    }

    public void PreviousLesson()
    {
        currentLessonIndex--;
        if (currentLessonIndex < 0)
        {
            currentLessonIndex = lessons.Count - 1;
        }
        currentExerciseIndex = 0;
        DisplayCurrentExercise();
    }

    public void SetLesson(int lessonIndex, int exerciseIndex = 0)
    {
        if (lessonIndex < 0 || lessonIndex >= lessons.Count) return;

        currentLessonIndex = lessonIndex;
        currentExerciseIndex = Mathf.Clamp(exerciseIndex, 0, lessons[lessonIndex].exercises.Count - 1);
        DisplayCurrentExercise();
    }

    public Lesson GetCurrentLesson()
    {
        if (currentLessonIndex >= 0 && currentLessonIndex < lessons.Count)
        {
            return lessons[currentLessonIndex];
        }
        return null;
    }

    public Exercise GetCurrentExercise()
    {
        Lesson lesson = GetCurrentLesson();
        if (lesson != null && currentExerciseIndex >= 0 && currentExerciseIndex < lesson.exercises.Count)
        {
            return lesson.exercises[currentExerciseIndex];
        }
        return null;
    }

    public string GetCurrentLessonTitle()
    {
        var lesson = GetCurrentLesson();
        if (lesson != null)
        {
            return GetLocalizedText(lesson.titleKey);
        }
        return "";
    }

    public string GetCurrentExerciseTitle()
    {
        var exercise = GetCurrentExercise();
        if (exercise != null)
        {
            return GetLocalizedText(exercise.titleKey);
        }
        return "";
    }

    public int CurrentLessonIndex => currentLessonIndex;
    public int CurrentExerciseIndex => currentExerciseIndex;
    public int TotalLessons => lessons.Count;
    public int TotalExercisesInCurrentLesson => GetCurrentLesson()?.exercises.Count ?? 0;

    #region Block Spawning

    /// <summary>
    /// Spawns blocks for the first code line of the current exercise
    /// </summary>
    public void SpawnBlocksForFirstCodeLine()
    {
        // Clear existing blocks
        ClearSpawnedBlocks();

        // Clear collected tokens
        collectedTokens.Clear();
        currentCodeTokens.Clear();

        // Find the first code line index in display
        firstCodeLineIndex = -1;
        for (int i = 0; i < isCommentLine.Count; i++)
        {
            if (!isCommentLine[i])
            {
                firstCodeLineIndex = i;
                break;
            }
        }

        if (blockPrefab == null)
        {
            Debug.LogWarning("Block prefab is not assigned!");
            return;
        }

        // Find the first code line (non-comment line)
        string firstCodeLine = GetFirstCodeLine();
        if (string.IsNullOrEmpty(firstCodeLine))
        {
            Debug.LogWarning("No code line found in current exercise!");
            return;
        }

        Debug.Log($"Spawning blocks for code line: {firstCodeLine}");

        // Split into tokens (words), excluding spaces
        List<string> tokens = SplitIntoTokens(firstCodeLine);
        currentCodeTokens = new List<string>(tokens);

        // Reset block selection index
        currentBlockIndex = 0;

        // Clear the code line display initially
        UpdateCodeLineDisplay();

        // Spawn a block for each token
        for (int i = 0; i < tokens.Count; i++)
        {
            SpawnBlock(tokens[i], i, tokens.Count);
        }

        Debug.Log($"Spawned {tokens.Count} blocks");
    }

    private string GetFirstCodeLine()
    {
        Exercise exercise = GetCurrentExercise();
        if (exercise == null || exercise.correctLines.Count == 0)
        {
            return null;
        }

        // Return the first correct line (code line)
        return exercise.correctLines[0];
    }

    private List<string> SplitIntoTokens(string line)
    {
        List<string> tokens = new List<string>();

        // Regex to match: strings, words, numbers, operators/punctuation
        var regex = new Regex(@"""[^""]*""|'[^']*'|[a-zA-Z_][a-zA-Z0-9_]*|\d+|[^\s]");

        foreach (Match match in regex.Matches(line))
        {
            string token = match.Value;
            // Skip whitespace-only tokens
            if (!string.IsNullOrWhiteSpace(token))
            {
                tokens.Add(token);
            }
        }

        return tokens;
    }

    private void SpawnBlock(string tokenText, int index, int totalCount)
    {
        // Try to spawn inside the room using MRUK
        Vector3 spawnPosition = GetRandomPositionInRoom();

        // Instantiate block
        GameObject block = Instantiate(blockPrefab, spawnPosition, Quaternion.identity, blockSpawnParent);
        block.name = $"Block_{index}_{tokenText}";

        // Find and set the TextMeshProUGUI text
        TextMeshProUGUI textComponent = block.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = tokenText;
        }
        else
        {
            Debug.LogWarning($"No TextMeshProUGUI found in block prefab for token: {tokenText}");
        }

        // Trigger scale/color update on TextBasedScaler if present
        TextBasedScaler scaler = block.GetComponent<TextBasedScaler>();
        if (scaler != null)
        {
            scaler.UpdateScales();
            scaler.UpdateColors();
        }

        // Set order index and LessonManager reference on BlockShooter
        BlockShooter shooter = block.GetComponent<BlockShooter>();
        if (shooter != null)
        {
            shooter.OrderIndex = index;
            shooter.TokenText = tokenText;
            shooter.LessonManager = this;
        }

        spawnedBlocks.Add(block);
    }

    /// <summary>
    /// Gets a spawn position 0.5m in front of the headset
    /// </summary>
    private Vector3 GetRandomPositionInRoom()
    {
        Transform headset = Camera.main?.transform;
        if (headset != null)
        {
            // Spawn 0.5m in front of the headset
            Vector3 spawnPos = headset.position + headset.forward * 0.5f;
            Debug.Log($"[LessonManager] Spawning at headset forward position: {spawnPos}");
            return spawnPos;
        }

        // Fallback if no headset found
        return spawnAreaCenter;
    }

    /// <summary>
    /// Clears all spawned blocks
    /// </summary>
    public void ClearSpawnedBlocks()
    {
        foreach (var block in spawnedBlocks)
        {
            if (block != null)
            {
                Destroy(block);
            }
        }
        spawnedBlocks.Clear();
    }

    /// <summary>
    /// Gets the list of spawned blocks
    /// </summary>
    public List<GameObject> SpawnedBlocks => spawnedBlocks;

    /// <summary>
    /// Gets the current expected block index
    /// </summary>
    public int CurrentBlockIndex => currentBlockIndex;

    /// <summary>
    /// Tries to select a block. Returns true if the block is the correct next one in the sequence.
    /// </summary>
    public bool TrySelectBlock(int blockIndex)
    {
        if (blockIndex == currentBlockIndex)
        {
            currentBlockIndex++;
            Debug.Log($"[LessonManager] Block {blockIndex} selected correctly. Next expected: {currentBlockIndex}");

            // Check if all blocks have been selected
            if (currentBlockIndex >= spawnedBlocks.Count)
            {
                Debug.Log("[LessonManager] All blocks selected in correct order!");
                OnAllBlocksSelected();
            }

            return true;
        }
        return false;
    }

    /// <summary>
    /// Called when all blocks have been selected in the correct order
    /// </summary>
    private void OnAllBlocksSelected()
    {
        // Reveal the next line and spawn blocks for the next code line
        RevealNextLine();

        // TODO: Spawn blocks for next code line if needed
    }

    /// <summary>
    /// Plays the block destroy sound
    /// </summary>
    public void PlayBlockDestroySound()
    {
        if (blockDestroySound != null)
        {
            blockDestroySound.Play();
        }
    }

    /// <summary>
    /// Removes a block from the spawned blocks list
    /// </summary>
    public void RemoveBlockFromList(GameObject block)
    {
        spawnedBlocks.Remove(block);
    }

    /// <summary>
    /// Adds a token to the collected list and updates the display
    /// </summary>
    public void AddCollectedToken(string token)
    {
        collectedTokens.Add(token);
        UpdateCodeLineDisplay();
        Debug.Log($"[LessonManager] Token collected: {token}, total collected: {collectedTokens.Count}/{currentCodeTokens.Count}");
    }

    /// <summary>
    /// Updates the code line display with collected tokens
    /// </summary>
    private void UpdateCodeLineDisplay()
    {
        if (firstCodeLineIndex < 0 || firstCodeLineIndex >= lineTexts.Count)
        {
            return;
        }

        // Build the display string with collected tokens
        StringBuilder displayText = new StringBuilder();
        var config = GetLanguageConfig(currentLanguage);

        for (int i = 0; i < collectedTokens.Count; i++)
        {
            string token = collectedTokens[i];

            // Get the color for this token
            string prevToken = i > 0 ? collectedTokens[i - 1] : null;
            string nextToken = i < collectedTokens.Count - 1 ? collectedTokens[i + 1] : null;

            var tokenType = GetTokenStyle(token, config, prevToken, nextToken);
            Color color = GetColorForTokenType(tokenType);
            string hexColor = ColorToHex(color);

            // Add space before token (except for opening brackets after function names, etc.)
            if (i > 0 && !ShouldOmitSpaceBefore(token, collectedTokens[i - 1]))
            {
                displayText.Append(" ");
            }

            displayText.Append($"<color={hexColor}>{token}</color>");
        }

        // Update the text
        lineTexts[firstCodeLineIndex].text = displayText.ToString();
    }

    /// <summary>
    /// Determines if space should be omitted before a token
    /// </summary>
    private bool ShouldOmitSpaceBefore(string currentToken, string prevToken)
    {
        // No space before closing brackets/parens
        if (currentToken == ")" || currentToken == "]" || currentToken == "}" || currentToken == ",")
            return true;

        // No space after opening brackets/parens
        if (prevToken == "(" || prevToken == "[" || prevToken == "{")
            return true;

        return false;
    }

    /// <summary>
    /// Gets the world position of the first code line's center on the SheetPanel
    /// </summary>
    public Vector3 GetFirstCodeLinePosition()
    {
        // Find the first code line index
        for (int i = 0; i < isCommentLine.Count && i < lineTexts.Count; i++)
        {
            if (!isCommentLine[i])
            {
                // This is the first code line
                return lineTexts[i].transform.position;
            }
        }

        // Fallback to sheetPanelContent position if no code line found
        if (sheetPanelContent != null)
        {
            return sheetPanelContent.position;
        }

        return Vector3.zero;
    }

    /// <summary>
    /// Plays the wrong selection sound and respawns all blocks
    /// </summary>
    public void OnWrongSelection()
    {
        if (wrongSelectionSound != null)
        {
            wrongSelectionSound.Play();
        }

        // Respawn all blocks
        RespawnAllBlocks();
    }

    /// <summary>
    /// Clears all blocks and respawns them at random positions
    /// </summary>
    public void RespawnAllBlocks()
    {
        // Clear existing blocks
        ClearSpawnedBlocks();

        // Respawn blocks for the first code line
        SpawnBlocksForFirstCodeLine();

        // Skip init delay on all respawned blocks
        foreach (var block in spawnedBlocks)
        {
            RandomWalk randomWalk = block.GetComponent<RandomWalk>();
            if (randomWalk != null)
            {
                randomWalk.InitializeImmediately();
            }
        }

        Debug.Log("[LessonManager] All blocks respawned");
    }

    #endregion

    #region Syntax Highlighting

    private string ApplySyntaxHighlighting(string line)
    {
        if (string.IsNullOrEmpty(line)) return line;

        // Get language config from TextBasedScaler
        var config = GetLanguageConfig(currentLanguage);
        if (config == null) return line;

        // Check if it's a comment line
        if (!string.IsNullOrEmpty(config.CommentPrefix) && line.TrimStart().StartsWith(config.CommentPrefix))
        {
            string commentColor = ColorToHex(TextBasedScaler.SyntaxColors.Comment);
            return $"<color={commentColor}>{line}</color>";
        }

        // Tokenize and colorize
        StringBuilder result = new StringBuilder();
        string[] tokens = TokenizeLine(line);

        for (int i = 0; i < tokens.Length; i++)
        {
            string token = tokens[i];
            string prevToken = i > 0 ? tokens[i - 1].Trim() : null;
            string nextToken = i < tokens.Length - 1 ? tokens[i + 1].Trim() : null;

            // Preserve whitespace
            if (string.IsNullOrWhiteSpace(token))
            {
                result.Append(token);
                continue;
            }

            var tokenType = GetTokenStyle(token, config, prevToken, nextToken);
            Color color = GetColorForTokenType(tokenType);
            string hexColor = ColorToHex(color);

            result.Append($"<color={hexColor}>{token}</color>");
        }

        return result.ToString();
    }

    private string[] TokenizeLine(string line)
    {
        // Split by word boundaries while preserving delimiters and whitespace
        var tokens = new List<string>();
        var regex = new Regex(@"(\s+|""[^""]*""|'[^']*'|[a-zA-Z_][a-zA-Z0-9_]*|\d+|[^\s\w])");

        int lastIndex = 0;
        foreach (Match match in regex.Matches(line))
        {
            if (match.Index > lastIndex)
            {
                tokens.Add(line.Substring(lastIndex, match.Index - lastIndex));
            }
            tokens.Add(match.Value);
            lastIndex = match.Index + match.Length;
        }

        if (lastIndex < line.Length)
        {
            tokens.Add(line.Substring(lastIndex));
        }

        return tokens.ToArray();
    }

    private TextBasedScaler.LanguageConfig GetLanguageConfig(string lang)
    {
        if (TextBasedScaler.LanguageConfigs.TryGetValue(lang, out var config))
        {
            return config;
        }
        // Fallback to python if language not found
        if (TextBasedScaler.LanguageConfigs.TryGetValue("python", out var fallback))
        {
            return fallback;
        }
        return null;
    }

    private static readonly HashSet<string> DefKeywords = new HashSet<string>
    {
        "def", "function", "fn", "func", "fun", "sub",
        "defp", "defmodule", "defmacro",
        "class", "interface", "struct", "enum", "trait", "type",
        "object", "data", "newtype"
    };

    private TextBasedScaler.TokenType GetTokenStyle(string token, TextBasedScaler.LanguageConfig config, string prevToken, string nextToken)
    {
        if (string.IsNullOrEmpty(token)) return TextBasedScaler.TokenType.Foreground;

        // String
        if (token.StartsWith("\"") || token.StartsWith("'") || token.StartsWith("`"))
        {
            return TextBasedScaler.TokenType.String;
        }

        // Comment
        if (!string.IsNullOrEmpty(config.CommentPrefix) && token.StartsWith(config.CommentPrefix))
        {
            return TextBasedScaler.TokenType.Comment;
        }

        // Decorator
        if (token.StartsWith("@")) return TextBasedScaler.TokenType.Decorator;

        // Number
        if (Regex.IsMatch(token, @"^\d+$")) return TextBasedScaler.TokenType.Number;

        // Keywords
        if (config.Keywords.Contains(token)) return TextBasedScaler.TokenType.Keyword;

        // Built-ins
        if (config.Builtins.Contains(token)) return TextBasedScaler.TokenType.BuiltIn;

        // Constant (All uppercase)
        if (Regex.IsMatch(token, @"^[A-Z][A-Z0-9_]+$") && token.Length > 1)
        {
            return TextBasedScaler.TokenType.Constant;
        }

        // Function/Class Definition
        if (!string.IsNullOrEmpty(prevToken) && DefKeywords.Contains(prevToken))
        {
            return TextBasedScaler.TokenType.FunctionDef;
        }

        // Brackets
        if (Regex.IsMatch(token, @"^[()[\]{}]$"))
        {
            return TextBasedScaler.TokenType.Bracket;
        }

        // Variables/Identifiers
        if (Regex.IsMatch(token, @"^[a-zA-Z_][a-zA-Z0-9_]*$"))
        {
            return TextBasedScaler.TokenType.Variable;
        }

        return TextBasedScaler.TokenType.Foreground;
    }

    private Color GetColorForTokenType(TextBasedScaler.TokenType type)
    {
        switch (type)
        {
            case TextBasedScaler.TokenType.Keyword: return TextBasedScaler.SyntaxColors.Keyword;
            case TextBasedScaler.TokenType.String: return TextBasedScaler.SyntaxColors.String;
            case TextBasedScaler.TokenType.Number: return TextBasedScaler.SyntaxColors.Number;
            case TextBasedScaler.TokenType.Comment: return TextBasedScaler.SyntaxColors.Comment;
            case TextBasedScaler.TokenType.DocComment: return TextBasedScaler.SyntaxColors.DocComment;
            case TextBasedScaler.TokenType.Constant: return TextBasedScaler.SyntaxColors.Constant;
            case TextBasedScaler.TokenType.FunctionDef: return TextBasedScaler.SyntaxColors.FunctionDef;
            case TextBasedScaler.TokenType.Decorator: return TextBasedScaler.SyntaxColors.Decorator;
            case TextBasedScaler.TokenType.BuiltIn: return TextBasedScaler.SyntaxColors.BuiltIn;
            case TextBasedScaler.TokenType.Variable: return TextBasedScaler.SyntaxColors.Variable;
            case TextBasedScaler.TokenType.Bracket: return TextBasedScaler.SyntaxColors.Bracket;
            default: return TextBasedScaler.SyntaxColors.Foreground;
        }
    }

    private string ColorToHex(Color color)
    {
        return $"#{ColorUtility.ToHtmlStringRGB(color)}";
    }

    #endregion
}
