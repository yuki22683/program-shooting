using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages lesson and course completion progress.
/// Uses PlayerPrefs for persistence.
/// </summary>
public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance { get; private set; }

    // Key format: "progress_{language}_{course}_{lesson}" = 1 (completed) or 0 (not completed)
    private const string PROGRESS_KEY_FORMAT = "progress_{0}_{1}_{2}";

    // Course completion key format: "course_complete_{language}_{course}"
    private const string COURSE_COMPLETE_KEY_FORMAT = "course_complete_{0}_{1}";

    // Lesson counts per course (0-indexed course)
    private static readonly int[] LESSON_COUNTS = { 13, 12, 10, 10, 10 }; // Python lessons 1-5

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[ProgressManager] Initialized");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Checks if a specific lesson is completed
    /// </summary>
    public bool IsLessonCompleted(string language, int courseIndex, int lessonIndex)
    {
        string key = string.Format(PROGRESS_KEY_FORMAT, language, courseIndex, lessonIndex);
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    /// <summary>
    /// Marks a lesson as completed and updates course completion if needed
    /// </summary>
    public void SetLessonCompleted(string language, int courseIndex, int lessonIndex, bool completed = true)
    {
        string key = string.Format(PROGRESS_KEY_FORMAT, language, courseIndex, lessonIndex);
        PlayerPrefs.SetInt(key, completed ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log($"[ProgressManager] Lesson {language} course {courseIndex} lesson {lessonIndex} = {(completed ? "completed" : "not completed")}");

        // Check if all lessons in the course are now complete
        if (completed)
        {
            CheckAndUpdateCourseCompletion(language, courseIndex);
        }
    }

    /// <summary>
    /// Checks if a lesson is unlocked (can be played)
    /// A lesson is unlocked if it's the first lesson or the previous lesson is completed
    /// </summary>
    public bool IsLessonUnlocked(string language, int courseIndex, int lessonIndex)
    {
        // First lesson is always unlocked
        if (lessonIndex == 0)
        {
            return true;
        }

        // Otherwise, check if previous lesson is completed
        return IsLessonCompleted(language, courseIndex, lessonIndex - 1);
    }

    /// <summary>
    /// Gets the index of the first locked lesson (or -1 if all are unlocked)
    /// </summary>
    public int GetFirstLockedLessonIndex(string language, int courseIndex, int totalLessons)
    {
        for (int i = 0; i < totalLessons; i++)
        {
            if (!IsLessonUnlocked(language, courseIndex, i))
            {
                return i;
            }
        }
        return -1; // All unlocked
    }

    /// <summary>
    /// Checks if all lessons in a course are completed
    /// </summary>
    public bool IsCourseCompleted(string language, int courseIndex)
    {
        string key = string.Format(COURSE_COMPLETE_KEY_FORMAT, language, courseIndex);
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    /// <summary>
    /// Checks if a course is unlocked (can be played)
    /// A course is unlocked if it's the first course or the previous course is completed
    /// </summary>
    public bool IsCourseUnlocked(string language, int courseIndex)
    {
        // First course is always unlocked
        if (courseIndex == 0)
        {
            return true;
        }

        // Otherwise, check if previous course is completed
        return IsCourseCompleted(language, courseIndex - 1);
    }

    /// <summary>
    /// Gets the number of completed lessons in a course
    /// </summary>
    public int GetCompletedLessonCount(string language, int courseIndex, int totalLessons)
    {
        int count = 0;
        for (int i = 0; i < totalLessons; i++)
        {
            if (IsLessonCompleted(language, courseIndex, i))
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// Checks and updates course completion status
    /// </summary>
    private void CheckAndUpdateCourseCompletion(string language, int courseIndex)
    {
        int totalLessons = GetLessonCount(courseIndex);
        int completedCount = GetCompletedLessonCount(language, courseIndex, totalLessons);

        if (completedCount >= totalLessons)
        {
            string key = string.Format(COURSE_COMPLETE_KEY_FORMAT, language, courseIndex);
            PlayerPrefs.SetInt(key, 1);
            PlayerPrefs.Save();
            Debug.Log($"[ProgressManager] Course {language} {courseIndex} completed!");
        }
    }

    /// <summary>
    /// Gets the number of lessons for a course
    /// </summary>
    public int GetLessonCount(int courseIndex)
    {
        if (courseIndex >= 0 && courseIndex < LESSON_COUNTS.Length)
        {
            return LESSON_COUNTS[courseIndex];
        }
        return 10; // Default
    }

    /// <summary>
    /// Resets all progress (for testing)
    /// </summary>
    public void ResetAllProgress()
    {
        // Clear all progress keys
        string[] languages = {
            "python", "javascript", "typescript", "java", "csharp", "cpp",
            "go", "rust", "ruby", "php", "swift", "kotlin", "bash", "sql",
            "lua", "perl", "haskell", "elixir", "assembly"
        };

        for (int lang = 0; lang < languages.Length; lang++)
        {
            for (int course = 0; course < 5; course++)
            {
                // Clear course completion
                string courseKey = string.Format(COURSE_COMPLETE_KEY_FORMAT, languages[lang], course);
                PlayerPrefs.DeleteKey(courseKey);

                // Clear all lesson progress
                for (int lesson = 0; lesson < 20; lesson++)
                {
                    string lessonKey = string.Format(PROGRESS_KEY_FORMAT, languages[lang], course, lesson);
                    PlayerPrefs.DeleteKey(lessonKey);
                }
            }
        }

        PlayerPrefs.Save();
        Debug.Log("[ProgressManager] All progress reset");
    }

    /// <summary>
    /// Gets current language string based on language index
    /// </summary>
    public static string GetLanguageString(int languageIndex)
    {
        switch (languageIndex)
        {
            case 0: return "python";
            case 1: return "javascript";
            case 2: return "typescript";
            case 3: return "java";
            case 4: return "csharp";
            case 5: return "cpp";
            case 6: return "go";
            case 7: return "rust";
            case 8: return "ruby";
            case 9: return "php";
            case 10: return "swift";
            case 11: return "kotlin";
            case 12: return "bash";
            case 13: return "sql";
            case 14: return "lua";
            case 15: return "perl";
            case 16: return "haskell";
            case 17: return "elixir";
            case 18: return "assembly";
            default: return "python";
        }
    }
}
