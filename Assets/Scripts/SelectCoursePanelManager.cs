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
    [SerializeField] private Image iconLevelImage;          // Icon_Level - Difficulty icon

    [Header("Difficulty Colors")]
    [SerializeField] private Color easyColor = new Color(0.0f, 0.8f, 1.0f, 1f);      // 水色 (Cyan)
    [SerializeField] private Color mediumColor = new Color(0.6f, 1.0f, 0.2f, 1f);    // 黄緑 (Yellow-Green)
    [SerializeField] private Color hardColor = new Color(1.0f, 0.5f, 0.0f, 1f);      // オレンジ (Orange)

    [Header("Lesson Data Keys")]
    [SerializeField] private string courseTitleKey = "python_lesson1_course_title";
    [SerializeField] private string courseDescriptionKey = "python_lesson1_course_description";
    [SerializeField] private string difficultyKey = "difficulty_easy";

    [Header("Progress Tracking")]
    [SerializeField] private int totalExercises = 10;
    [SerializeField] private int completedExercises = 0;

    [Header("Navigation")]
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject selectLanguagePanel;

    private void Awake()
    {
        // Auto-find UI references if not set
        FindUIReferences();
    }

    private void Start()
    {
        SetupBackButton();
        UpdateDisplay();
    }

    private void OnEnable()
    {
        UpdateDisplay();
    }

    /// <summary>
    /// Sets up the back button click handler
    /// </summary>
    private void SetupBackButton()
    {
        // Find back button if not set
        if (backButton == null)
        {
            // Path: PanelInteractable/PanelCanvas/TopBar/Horizontal/Horizontal/PrimaryButton_IconAndLabel_UnityUIButton (2)
            Transform buttonTransform = transform.Find("PanelInteractable/PanelCanvas/TopBar/Horizontal/Horizontal/PrimaryButton_IconAndLabel_UnityUIButton (2)");
            if (buttonTransform != null)
            {
                backButton = buttonTransform.GetComponent<Button>();
                Debug.Log("[SelectCoursePanelManager] Found back button");
            }
            else
            {
                Debug.LogWarning("[SelectCoursePanelManager] Back button not found");
            }
        }

        // Find language panel if not set
        if (selectLanguagePanel == null)
        {
            selectLanguagePanel = FindObjectByName("SelectLanguagePanel");
            if (selectLanguagePanel != null)
            {
                Debug.Log("[SelectCoursePanelManager] Found SelectLanguagePanel");
            }
        }

        // Add click handler
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
            Debug.Log("[SelectCoursePanelManager] Added back button click handler");
        }
    }

    /// <summary>
    /// Called when the back button is clicked
    /// </summary>
    private void OnBackButtonClicked()
    {
        Debug.Log("[SelectCoursePanelManager] Back button clicked");

        // Store current position
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        // Find language panel if not cached
        if (selectLanguagePanel == null)
        {
            selectLanguagePanel = FindObjectByName("SelectLanguagePanel");
        }

        if (selectLanguagePanel != null)
        {
            // Position language panel at current course panel position
            selectLanguagePanel.transform.position = currentPosition;
            selectLanguagePanel.transform.rotation = currentRotation;

            // Skip automatic repositioning in PositionInFrontOfHeadset
            PositionInFrontOfHeadset positioner = selectLanguagePanel.GetComponent<PositionInFrontOfHeadset>();
            if (positioner != null)
            {
                positioner.SkipNextReposition();
            }

            // Show language panel
            selectLanguagePanel.SetActive(true);
            Debug.Log($"[SelectCoursePanelManager] Showed SelectLanguagePanel at {currentPosition}");
        }
        else
        {
            Debug.LogError("[SelectCoursePanelManager] SelectLanguagePanel not found!");
        }

        // Hide this course panel
        gameObject.SetActive(false);
        Debug.Log("[SelectCoursePanelManager] Hidden SelectCoursePanel");
    }

    /// <summary>
    /// Finds a GameObject by name in the scene
    /// </summary>
    private GameObject FindObjectByName(string name)
    {
        foreach (GameObject root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            GameObject found = FindInChildren(root.transform, name);
            if (found != null) return found;
        }
        return null;
    }

    private GameObject FindInChildren(Transform parent, string name)
    {
        if (parent.name == name) return parent.gameObject;
        foreach (Transform child in parent)
        {
            GameObject found = FindInChildren(child, name);
            if (found != null) return found;
        }
        return null;
    }

    /// <summary>
    /// Automatically finds UI references in child objects if not already set
    /// </summary>
    private void FindUIReferences()
    {
        // Correct path: Content/Background/Elements/Vertical/Label (2)
        // Find title label (Label (2))
        if (titleLabel == null)
        {
            Transform titleTransform = transform.Find("Content/Background/Elements/Vertical/Label (2)");
            if (titleTransform != null)
            {
                titleLabel = titleTransform.GetComponent<TMP_Text>();
                Debug.Log("[SelectCoursePanelManager] Found titleLabel");
            }
            else
            {
                Debug.LogWarning("[SelectCoursePanelManager] Could not find titleLabel at Content/Background/Elements/Vertical/Label (2)");
            }
        }

        // Find description label (Label (3))
        if (descriptionLabel == null)
        {
            Transform descTransform = transform.Find("Content/Background/Elements/Vertical/Label (3)");
            if (descTransform != null)
            {
                descriptionLabel = descTransform.GetComponent<TMP_Text>();
                Debug.Log("[SelectCoursePanelManager] Found descriptionLabel");
            }
            else
            {
                Debug.LogWarning("[SelectCoursePanelManager] Could not find descriptionLabel at Content/Background/Elements/Vertical/Label (3)");
            }
        }

        // Find level label (Label_Level)
        if (levelLabel == null)
        {
            Transform levelTransform = transform.Find("Content/Background/Elements/Label_Level");
            if (levelTransform != null)
            {
                levelLabel = levelTransform.GetComponent<TMP_Text>();
                Debug.Log("[SelectCoursePanelManager] Found levelLabel");
            }
            else
            {
                Debug.LogWarning("[SelectCoursePanelManager] Could not find levelLabel at Content/Background/Elements/Label_Level");
            }
        }

        // Find progress bar (ProgressBar_In)
        if (progressBarIn == null)
        {
            Transform progressBarTransform = transform.Find("Content/Background/Elements/Progress/ProgressBar_In");
            if (progressBarTransform != null)
            {
                progressBarIn = progressBarTransform.GetComponent<Image>();
                Debug.Log("[SelectCoursePanelManager] Found progressBarIn");
            }
            else
            {
                Debug.LogWarning("[SelectCoursePanelManager] Could not find progressBarIn at Content/Background/Elements/Progress/ProgressBar_In");
            }
        }

        // Find progress label (Progress/Label)
        if (progressLabel == null)
        {
            Transform progressTransform = transform.Find("Content/Background/Elements/Progress/Label");
            if (progressTransform != null)
            {
                progressLabel = progressTransform.GetComponent<TMP_Text>();
                Debug.Log("[SelectCoursePanelManager] Found progressLabel");
            }
            else
            {
                Debug.LogWarning("[SelectCoursePanelManager] Could not find progressLabel at Content/Background/Elements/Progress/Label");
            }
        }

        // Find Icon_Level image
        if (iconLevelImage == null)
        {
            Transform iconTransform = transform.Find("Content/Background/Elements/Icon_Level");
            if (iconTransform != null)
            {
                iconLevelImage = iconTransform.GetComponent<Image>();
                Debug.Log("[SelectCoursePanelManager] Found iconLevelImage");
            }
            else
            {
                Debug.LogWarning("[SelectCoursePanelManager] Could not find iconLevelImage at Content/Background/Elements/Icon_Level");
            }
        }
    }

    /// <summary>
    /// Recursively finds a child by name
    /// </summary>
    private Transform FindDeepChild(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child;
            }
            Transform found = FindDeepChild(child, name);
            if (found != null)
            {
                return found;
            }
        }
        return null;
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

        // Update icon color based on difficulty
        UpdateIconColor();

        // Update progress bar
        UpdateProgressBar();

        // Update progress label
        UpdateProgressLabel();
    }

    /// <summary>
    /// Updates the Icon_Level color based on difficulty level
    /// </summary>
    private void UpdateIconColor()
    {
        if (iconLevelImage == null) return;

        Color targetColor;
        switch (difficultyKey)
        {
            case "difficulty_easy":
                targetColor = easyColor;   // 水色
                break;
            case "difficulty_medium":
                targetColor = mediumColor; // 黄緑
                break;
            case "difficulty_hard":
                targetColor = hardColor;   // オレンジ
                break;
            default:
                targetColor = easyColor;
                break;
        }

        iconLevelImage.color = targetColor;
        Debug.Log($"[SelectCoursePanelManager] Set icon color for {difficultyKey}");
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

    /// <summary>
    /// Sets the localization keys for this course panel
    /// </summary>
    public void SetLocalizationKeys(string titleKey, string descriptionKey, string levelKey)
    {
        courseTitleKey = titleKey;
        courseDescriptionKey = descriptionKey;
        difficultyKey = levelKey;
        UpdateDisplay();
    }

    /// <summary>
    /// Configures this panel with course data
    /// </summary>
    public void Configure(string titleKey, string descriptionKey, string levelKey, int total, int completed)
    {
        courseTitleKey = titleKey;
        courseDescriptionKey = descriptionKey;
        difficultyKey = levelKey;
        totalExercises = total;
        completedExercises = completed;
        UpdateDisplay();
    }
}
