using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [Header("UI References")]
    [SerializeField] private Transform sheetPanelContent;

    private List<TextMeshProUGUI> lineTexts = new List<TextMeshProUGUI>();
    private string cachedLanguageCode = "";

    private void Awake()
    {
        FindLineTexts();
    }

    private void Start()
    {
        DisplayCurrentExercise();
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
        List<string> displayLines = BuildDisplayLines(exercise);

        for (int i = 0; i < lineTexts.Count; i++)
        {
            if (i < displayLines.Count)
            {
                lineTexts[i].text = displayLines[i];
            }
            else
            {
                lineTexts[i].text = "";
            }
        }

        Debug.Log($"Displayed exercise: {exercise.titleKey} with {displayLines.Count} lines");
    }

    private List<string> BuildDisplayLines(Exercise exercise)
    {
        List<string> result = new List<string>();

        Dictionary<int, LocalizedComment> commentMap = new Dictionary<int, LocalizedComment>();
        foreach (var comment in exercise.comments)
        {
            commentMap[comment.lineIndex] = comment;
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
            }
            else if (codeIndex < exercise.correctLines.Count)
            {
                result.Add(exercise.correctLines[codeIndex]);
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
}
