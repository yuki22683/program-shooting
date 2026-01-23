using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages individual lesson button UI in the SelectLessonPanel.
/// Displays exercise title, description, and difficulty level.
/// </summary>
public class SelectLessonPanelManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text titleLabel;           // Label (2) - Exercise title
    [SerializeField] private TMP_Text descriptionLabel;     // Label (3) - Exercise description
    [SerializeField] private TMP_Text levelLabel;           // Label_Level - Difficulty level
    [SerializeField] private Image iconLevelImage;          // Icon_Level - Difficulty icon
    [SerializeField] private Image iconPassImage;           // Icon_pass - Completed icon
    [SerializeField] private Image iconLockImage;           // Icon_lock - Locked icon

    [Header("Difficulty Colors")]
    [SerializeField] private Color easyColor = new Color(0.0f, 0.8f, 1.0f, 1f);      // Cyan
    [SerializeField] private Color mediumColor = new Color(0.6f, 1.0f, 0.2f, 1f);    // Yellow-Green
    [SerializeField] private Color hardColor = new Color(1.0f, 0.5f, 0.0f, 1f);      // Orange

    [Header("Lesson Data Keys")]
    [SerializeField] private string exerciseTitleKey = "python_lesson1_ex1_title";
    [SerializeField] private string exerciseDescriptionKey = "python_lesson1_ex1_description";
    [SerializeField] private string difficultyKey = "difficulty_easy";

    [Header("Exercise State")]
    [SerializeField] private bool isCompleted = false;
    [SerializeField] private bool isLocked = false;

    /// <summary>
    /// The exercise index within the lesson (0-based)
    /// </summary>
    public int ExerciseIndex { get; set; } = 0;

    /// <summary>
    /// The lesson index (0-based)
    /// </summary>
    public int LessonIndex { get; set; } = 0;

    private void Awake()
    {
        Debug.Log($"[SelectLessonPanelManager] Awake() called on {gameObject.name}");
        FindUIReferences();
    }

    private void Start()
    {
        UpdateDisplay();
    }

    private void OnEnable()
    {
        UpdateDisplay();
    }

    /// <summary>
    /// Automatically finds UI references in child objects if not already set
    /// </summary>
    private void FindUIReferences()
    {
        Debug.Log($"[SelectLessonPanelManager] FindUIReferences() on {gameObject.name}, child count: {transform.childCount}");

        // Find title label (Label (2))
        if (titleLabel == null)
        {
            Transform titleTransform = transform.Find("Content/Background/Elements/Vertical/Label (2)");
            if (titleTransform != null)
            {
                titleLabel = titleTransform.GetComponent<TMP_Text>();
                Debug.Log("[SelectLessonPanelManager] Found titleLabel");
            }
            else
            {
                Debug.LogWarning("[SelectLessonPanelManager] Could not find titleLabel");
            }
        }

        // Find description label (Label (3))
        if (descriptionLabel == null)
        {
            Transform descTransform = transform.Find("Content/Background/Elements/Vertical/Label (3)");
            if (descTransform != null)
            {
                descriptionLabel = descTransform.GetComponent<TMP_Text>();
                Debug.Log("[SelectLessonPanelManager] Found descriptionLabel");
            }
            else
            {
                Debug.LogWarning("[SelectLessonPanelManager] Could not find descriptionLabel");
            }
        }

        // Find level label (Label_Level)
        if (levelLabel == null)
        {
            Transform levelTransform = transform.Find("Content/Background/Elements/Label_Level");
            if (levelTransform != null)
            {
                levelLabel = levelTransform.GetComponent<TMP_Text>();
                Debug.Log("[SelectLessonPanelManager] Found levelLabel");
            }
            else
            {
                Debug.LogWarning("[SelectLessonPanelManager] Could not find levelLabel");
            }
        }

        // Find Icon_Level image
        if (iconLevelImage == null)
        {
            Transform iconTransform = transform.Find("Content/Background/Elements/Icon_Level");
            if (iconTransform != null)
            {
                iconLevelImage = iconTransform.GetComponent<Image>();
                Debug.Log("[SelectLessonPanelManager] Found iconLevelImage");
            }
            else
            {
                Debug.LogWarning("[SelectLessonPanelManager] Could not find iconLevelImage");
            }
        }

        // Find Icon_pass image
        if (iconPassImage == null)
        {
            Transform iconPassTransform = transform.Find("Content/Background/Elements/Icon_pass");
            if (iconPassTransform != null)
            {
                iconPassImage = iconPassTransform.GetComponent<Image>();
                Debug.Log("[SelectLessonPanelManager] Found iconPassImage");
            }
        }

        // Find Icon_lock image
        if (iconLockImage == null)
        {
            Transform iconLockTransform = transform.Find("Content/Background/Elements/Icon_lock");
            if (iconLockTransform != null)
            {
                iconLockImage = iconLockTransform.GetComponent<Image>();
                Debug.Log("[SelectLessonPanelManager] Found iconLockImage");
            }
        }
    }

    /// <summary>
    /// Updates all UI elements with current exercise data
    /// </summary>
    public void UpdateDisplay()
    {
        if (LocalizationManager.Instance == null)
        {
            Debug.LogWarning("[SelectLessonPanelManager] LocalizationManager not available");
            return;
        }

        TMP_FontAsset localizedFont = LocalizationManager.Instance.GetLocalizedFont();

        // Update title
        if (titleLabel != null)
        {
            if (localizedFont != null) titleLabel.font = localizedFont;
            titleLabel.text = LocalizationManager.Instance.GetText(exerciseTitleKey);
            Debug.Log($"[SelectLessonPanelManager] Set title: {titleLabel.text}");
        }

        // Update description
        if (descriptionLabel != null)
        {
            if (localizedFont != null) descriptionLabel.font = localizedFont;
            string descText = LocalizationManager.Instance.GetText(exerciseDescriptionKey);
            // If description key doesn't exist, leave it empty or use a fallback
            if (descText == exerciseDescriptionKey)
            {
                descriptionLabel.text = "";
            }
            else
            {
                descriptionLabel.text = descText;
            }
            descriptionLabel.textWrappingMode = TextWrappingModes.Normal;
            descriptionLabel.enableAutoSizing = true;
            descriptionLabel.fontSizeMin = 14f;
            descriptionLabel.fontSizeMax = 28f;
            Debug.Log($"[SelectLessonPanelManager] Set description: {descriptionLabel.text}");
        }

        // Update exercise number (display as 1-based index)
        if (levelLabel != null)
        {
            if (localizedFont != null) levelLabel.font = localizedFont;
            levelLabel.text = (ExerciseIndex + 1).ToString();
            Debug.Log($"[SelectLessonPanelManager] Set exercise number: {levelLabel.text}");
        }

        // Update icon color based on difficulty
        UpdateIconColor();

        // Update state icons
        UpdateStateIcons();
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
                targetColor = easyColor;
                break;
            case "difficulty_medium":
                targetColor = mediumColor;
                break;
            case "difficulty_hard":
                targetColor = hardColor;
                break;
            default:
                targetColor = easyColor;
                break;
        }

        iconLevelImage.color = targetColor;
        Debug.Log($"[SelectLessonPanelManager] Set icon color for {difficultyKey}");
    }

    /// <summary>
    /// Updates the completed and locked icons based on state
    /// </summary>
    private void UpdateStateIcons()
    {
        if (iconPassImage != null)
        {
            iconPassImage.gameObject.SetActive(isCompleted);
        }

        if (iconLockImage != null)
        {
            iconLockImage.gameObject.SetActive(isLocked);
        }
    }

    /// <summary>
    /// Sets the localization keys for this lesson panel
    /// </summary>
    public void SetLocalizationKeys(string titleKey, string descriptionKey, string levelKey)
    {
        exerciseTitleKey = titleKey;
        exerciseDescriptionKey = descriptionKey;
        difficultyKey = levelKey;
        UpdateDisplay();
    }

    /// <summary>
    /// Configures this panel with exercise data
    /// </summary>
    public void Configure(string titleKey, string descriptionKey, string levelKey, bool completed = false, bool locked = false)
    {
        Debug.Log($"[SelectLessonPanelManager] Configure() on {gameObject.name} with title={titleKey}");

        // Ensure UI references are found (in case Awake wasn't called yet)
        FindUIReferences();

        exerciseTitleKey = titleKey;
        exerciseDescriptionKey = descriptionKey;
        difficultyKey = levelKey;
        isCompleted = completed;
        isLocked = locked;
        UpdateDisplay();
    }

    /// <summary>
    /// Sets the completion state of this exercise
    /// </summary>
    public void SetCompleted(bool completed)
    {
        isCompleted = completed;
        UpdateStateIcons();
    }

    /// <summary>
    /// Sets the locked state of this exercise
    /// </summary>
    public void SetLocked(bool locked)
    {
        isLocked = locked;
        UpdateStateIcons();
    }
}
