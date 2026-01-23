using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the language selection flow.
/// When a language is selected, hides the language panel and shows the course panel.
/// </summary>
public class LanguageSelectionManager : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private GameObject selectLanguagePanel;
    [SerializeField] private GameObject selectCoursePanel;

    [Header("Settings")]
    [SerializeField] private float panelDistance = 1.0f;

    private string selectedLanguage = "";

    private void Awake()
    {
        Debug.Log("[LanguageSelectionManager] Awake called");
    }

    private void Start()
    {
        Debug.Log("[LanguageSelectionManager] Start called");

        // Find panels if not assigned
        FindPanels();

        // NOTE: Button handlers are now set up by SelectLanguagePanelController
        // to avoid duplicate handlers being added to the same buttons
        // SetupLanguageButtonHandlers();
    }

    private void OnEnable()
    {
        Debug.Log("[LanguageSelectionManager] OnEnable called");
    }

    private void FindPanels()
    {
        if (selectLanguagePanel == null)
        {
            selectLanguagePanel = FindInactiveObjectByName("SelectLanguagePanel");
        }

        if (selectCoursePanel == null)
        {
            selectCoursePanel = FindInactiveObjectByName("SelectCoursePanel");
        }

        if (selectLanguagePanel != null)
        {
            Debug.Log("[LanguageSelectionManager] Found SelectLanguagePanel");
        }
        else
        {
            Debug.LogWarning("[LanguageSelectionManager] SelectLanguagePanel not found");
        }

        if (selectCoursePanel != null)
        {
            Debug.Log("[LanguageSelectionManager] Found SelectCoursePanel");
        }
        else
        {
            Debug.LogWarning("[LanguageSelectionManager] SelectCoursePanel not found");
        }
    }

    private GameObject FindInactiveObjectByName(string name)
    {
        foreach (GameObject rootObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            GameObject found = FindChildByName(rootObj.transform, name);
            if (found != null) return found;
        }
        return null;
    }

    private GameObject FindChildByName(Transform parent, string name)
    {
        if (parent.name == name)
            return parent.gameObject;

        foreach (Transform child in parent)
        {
            GameObject result = FindChildByName(child, name);
            if (result != null)
                return result;
        }
        return null;
    }

    // Known programming languages
    private static readonly string[] KnownLanguages = {
        "python", "javascript", "typescript", "java", "csharp", "cpp",
        "go", "rust", "ruby", "php", "swift", "kotlin", "bash", "sql", "lua", "perl",
        "haskell", "elixir", "assembly"
    };

    /// <summary>
    /// Sets up click handlers for all language buttons in SelectLanguagePanel
    /// </summary>
    private void SetupLanguageButtonHandlers()
    {
        if (selectLanguagePanel == null)
        {
            Debug.LogError("[LanguageSelectionManager] selectLanguagePanel is null!");
            return;
        }

        Debug.Log($"[LanguageSelectionManager] Setting up handlers for panel: {selectLanguagePanel.name}");

        // Find all buttons in the language panel
        Button[] buttons = selectLanguagePanel.GetComponentsInChildren<Button>(true);
        Debug.Log($"[LanguageSelectionManager] Found {buttons.Length} buttons in panel");

        int handlerCount = 0;

        foreach (Button button in buttons)
        {
            string btnName = button.name.ToLower();

            // Skip close/back buttons
            if (btnName.Contains("close") || btnName.Contains("back"))
            {
                continue;
            }

            // Get the language from the button's label text or name
            TMP_Text label = button.GetComponentInChildren<TMP_Text>();
            string labelText = label != null ? label.text.ToLower() : "";

            // Check if this is a language button
            string languageId = null;
            foreach (string lang in KnownLanguages)
            {
                if (btnName.Contains(lang) || labelText.Contains(lang))
                {
                    languageId = lang;
                    break;
                }
            }

            if (languageId == null)
            {
                Debug.Log($"[LanguageSelectionManager] Skipping non-language button: {button.name}");
                continue;
            }

            // Add click listener
            string captured = languageId;
            button.onClick.AddListener(() => OnLanguageSelected(captured));
            handlerCount++;
            Debug.Log($"[LanguageSelectionManager] Added click handler for: {button.name} -> {languageId}");
        }

        Debug.Log($"[LanguageSelectionManager] Set up {handlerCount} language button handlers");
    }

    /// <summary>
    /// Gets the language ID from a button's label text
    /// </summary>
    private string GetLanguageFromButton(Button button)
    {
        // Try to find TextMeshProUGUI label
        TMP_Text label = button.GetComponentInChildren<TMP_Text>();
        if (label != null && !string.IsNullOrEmpty(label.text))
        {
            return label.text.ToLower().Replace(" ", "").Replace("#", "sharp").Replace("++", "pp");
        }
        return null;
    }

    /// <summary>
    /// Extracts language ID from button name (e.g., "TextTileButton_python" -> "python")
    /// </summary>
    private string ExtractLanguageId(string buttonName)
    {
        // Try to extract language from button name
        string[] parts = buttonName.Split('_');
        if (parts.Length >= 2)
        {
            return parts[parts.Length - 1].ToLower();
        }
        return buttonName.ToLower();
    }

    /// <summary>
    /// Called when a language button is clicked
    /// </summary>
    public void OnLanguageSelected(string languageId)
    {
        Debug.Log($"[LanguageSelectionManager] Language selected: {languageId}");
        selectedLanguage = languageId;

        // Store the current position of the language panel
        Vector3 panelPosition = Vector3.zero;
        Quaternion panelRotation = Quaternion.identity;

        if (selectLanguagePanel != null)
        {
            panelPosition = selectLanguagePanel.transform.position;
            panelRotation = selectLanguagePanel.transform.rotation;

            // Hide the language panel
            selectLanguagePanel.SetActive(false);
            Debug.Log("[LanguageSelectionManager] Hidden SelectLanguagePanel");
        }

        // Show the course panel at the same position
        if (selectCoursePanel != null)
        {
            // Position course panel at the same location as language panel
            selectCoursePanel.transform.position = panelPosition;
            selectCoursePanel.transform.rotation = panelRotation;

            // Add PositionInFrontOfHeadset if not present (for future repositioning)
            PositionInFrontOfHeadset positioner = selectCoursePanel.GetComponent<PositionInFrontOfHeadset>();
            if (positioner == null)
            {
                positioner = selectCoursePanel.AddComponent<PositionInFrontOfHeadset>();
                positioner.Configure(panelDistance, 0f, true, true);
            }

            // Show the course panel
            selectCoursePanel.SetActive(true);
            Debug.Log($"[LanguageSelectionManager] Shown SelectCoursePanel at position {panelPosition}");

            // Update course panel to show courses for the selected language
            UpdateCoursePanelForLanguage(languageId);
        }
    }

    /// <summary>
    /// Updates the course panel to display courses for the selected language
    /// </summary>
    private void UpdateCoursePanelForLanguage(string languageId)
    {
        // Find the Content container for buttons
        Transform content = FindContentContainer(selectCoursePanel.transform);
        if (content == null)
        {
            content = selectCoursePanel.transform;
        }

        // Find or add CourseButtonSpawner
        CourseButtonSpawner spawner = selectCoursePanel.GetComponentInChildren<CourseButtonSpawner>();
        if (spawner == null)
        {
            spawner = content.gameObject.AddComponent<CourseButtonSpawner>();
            Debug.Log("[LanguageSelectionManager] Added CourseButtonSpawner to content container");
        }

        // Load prefab from Resources and configure spawner
        GameObject prefab = Resources.Load<GameObject>("Prefabs/TextTileButton_Lesson Variant");
        if (prefab != null)
        {
            spawner.SetButtonPrefab(prefab);
            Debug.Log("[LanguageSelectionManager] Loaded button prefab from Resources");
        }
        spawner.SetButtonContainer(content);

        // Set language index and spawn course buttons
        int languageIndex = GetLanguageIndex(languageId);
        spawner.SetLanguageIndex(languageIndex);
        Debug.Log($"[LanguageSelectionManager] Updated course buttons for {languageId} (index {languageIndex})");

        // Also notify LessonManager of the selected language
        LessonManager lessonManager = FindObjectOfType<LessonManager>();
        if (lessonManager != null)
        {
            // LessonManager could have a method to set the current language
            Debug.Log($"[LanguageSelectionManager] Language {languageId} selected, LessonManager notified");
        }
    }

    /// <summary>
    /// Finds the Content container for buttons (Scroll View/Viewport/Content)
    /// </summary>
    private Transform FindContentContainer(Transform parent)
    {
        // Look for common content container patterns
        Transform scrollView = parent.Find("Scroll View");
        if (scrollView != null)
        {
            Transform viewport = scrollView.Find("Viewport");
            if (viewport != null)
            {
                Transform content = viewport.Find("Content");
                if (content != null) return content;
            }
        }

        // Try to find any child named "Content"
        return FindChildTransformByName(parent, "Content");
    }

    private Transform FindChildTransformByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            Transform found = FindChildTransformByName(child, name);
            if (found != null) return found;
        }
        return null;
    }

    /// <summary>
    /// Gets the currently selected language
    /// </summary>
    public string SelectedLanguage => selectedLanguage;

    /// <summary>
    /// Converts language ID string to index
    /// </summary>
    private int GetLanguageIndex(string languageId)
    {
        switch (languageId.ToLower())
        {
            case "python": return 0;
            case "javascript": return 1;
            case "typescript": return 2;
            case "java": return 3;
            case "csharp": return 4;
            case "cpp": return 5;
            case "go": return 6;
            case "rust": return 7;
            case "ruby": return 8;
            case "php": return 9;
            case "swift": return 10;
            case "kotlin": return 11;
            case "bash": return 12;
            case "sql": return 13;
            case "lua": return 14;
            case "perl": return 15;
            case "haskell": return 16;
            case "elixir": return 17;
            case "assembly": return 18;
            default: return 0;
        }
    }

    /// <summary>
    /// Shows the language selection panel and hides the course panel
    /// </summary>
    public void ShowLanguageSelection()
    {
        if (selectCoursePanel != null)
        {
            selectCoursePanel.SetActive(false);
        }

        if (selectLanguagePanel != null)
        {
            // Reposition in front of headset
            PositionInFrontOfHeadset positioner = selectLanguagePanel.GetComponent<PositionInFrontOfHeadset>();
            if (positioner != null)
            {
                positioner.Reposition();
            }

            selectLanguagePanel.SetActive(true);
        }
    }
}
