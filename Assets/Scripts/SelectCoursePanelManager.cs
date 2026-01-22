using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the SelectCoursePanel UI, displaying lesson information from LessonManager.
/// </summary>
public class SelectCoursePanelManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text titleLabel;           // Label (2) - Lesson title
    [SerializeField] private TMP_Text descriptionLabel;     // Label (3) - Lesson description
    [SerializeField] private TMP_Text levelLabel;           // Label_Level - Difficulty level
    [SerializeField] private Image progressBarIn;           // ProgressBar_In - Progress bar fill
    [SerializeField] private TMP_Text progressLabel;        // Progress/Label - "completed/total" text

    [Header("Lesson Data Keys")]
    [SerializeField] private string courseTitleKey = "python_lesson1_course_title";
    [SerializeField] private string courseDescriptionKey = "python_lesson1_course_description";
    [SerializeField] private string difficultyKey = "difficulty_easy";

    [Header("Progress Tracking")]
    [SerializeField] private int totalExercises = 10;
    [SerializeField] private int completedExercises = 0;

    private void Start()
    {
        UpdateDisplay();
    }

    private void OnEnable()
    {
        UpdateDisplay();
    }

    /// <summary>
    /// Updates all UI elements with current lesson data
    /// </summary>
    public void UpdateDisplay()
    {
        if (LocalizationManager.Instance == null)
        {
            Debug.LogWarning("[SelectCoursePanelManager] LocalizationManager not available");
            return;
        }

        TMP_FontAsset localizedFont = LocalizationManager.Instance.GetLocalizedFont();

        // Update title
        if (titleLabel != null)
        {
            if (localizedFont != null) titleLabel.font = localizedFont;
            titleLabel.text = LocalizationManager.Instance.GetText(courseTitleKey);
            Debug.Log($"[SelectCoursePanelManager] Set title: {titleLabel.text}");
        }

        // Update description
        if (descriptionLabel != null)
        {
            if (localizedFont != null) descriptionLabel.font = localizedFont;
            descriptionLabel.text = LocalizationManager.Instance.GetText(courseDescriptionKey);
            // Enable word wrapping and set appropriate font size for description
            descriptionLabel.textWrappingMode = TextWrappingModes.Normal;
            descriptionLabel.enableAutoSizing = true;
            descriptionLabel.fontSizeMin = 18f;
            descriptionLabel.fontSizeMax = 36f;
            Debug.Log($"[SelectCoursePanelManager] Set description: {descriptionLabel.text}");
        }

        // Update difficulty level
        if (levelLabel != null)
        {
            if (localizedFont != null) levelLabel.font = localizedFont;
            levelLabel.text = LocalizationManager.Instance.GetText(difficultyKey);
            Debug.Log($"[SelectCoursePanelManager] Set level: {levelLabel.text}");
        }

        // Update progress bar
        UpdateProgressBar();

        // Update progress label
        UpdateProgressLabel();
    }

    /// <summary>
    /// Updates the progress bar fill amount based on completed exercises
    /// </summary>
    private void UpdateProgressBar()
    {
        if (progressBarIn == null) return;

        float progress = totalExercises > 0 ? (float)completedExercises / totalExercises : 0f;
        progressBarIn.fillAmount = progress;
        Debug.Log($"[SelectCoursePanelManager] Set progress bar: {progress:P0}");
    }

    /// <summary>
    /// Updates the progress label text (e.g., "0/10")
    /// </summary>
    private void UpdateProgressLabel()
    {
        if (progressLabel == null) return;

        TMP_FontAsset localizedFont = LocalizationManager.Instance?.GetLocalizedFont();
        if (localizedFont != null) progressLabel.font = localizedFont;

        progressLabel.text = $"{completedExercises}/{totalExercises}";
        Debug.Log($"[SelectCoursePanelManager] Set progress label: {progressLabel.text}");
    }

    /// <summary>
    /// Sets the number of completed exercises and updates the display
    /// </summary>
    public void SetCompletedExercises(int completed)
    {
        completedExercises = Mathf.Clamp(completed, 0, totalExercises);
        UpdateProgressBar();
        UpdateProgressLabel();
    }

    /// <summary>
    /// Sets the total number of exercises
    /// </summary>
    public void SetTotalExercises(int total)
    {
        totalExercises = Mathf.Max(1, total);
        UpdateProgressBar();
        UpdateProgressLabel();
    }

    /// <summary>
    /// Gets the total number of exercises from LessonManager if available
    /// </summary>
    public void SyncWithLessonManager()
    {
        LessonManager lessonManager = FindObjectOfType<LessonManager>();
        if (lessonManager != null)
        {
            totalExercises = lessonManager.TotalExercisesInCurrentLesson;
            UpdateProgressBar();
            UpdateProgressLabel();
            Debug.Log($"[SelectCoursePanelManager] Synced with LessonManager: {totalExercises} exercises");
        }
    }
}
