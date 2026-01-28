using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public class SlideManager : MonoBehaviour
{
    public static SlideManager Instance { get; private set; }

    [Header("UI References (Auto-found if null)")]
    [SerializeField] private TMP_Text exerciseTitleText; // TopBar for exercise title
    [SerializeField] private TMP_Text slideTitleText;    // Title object for slide page title
    [SerializeField] private Image slideImage;           // Image object for slide illustration
    [SerializeField] private TMP_Text contentText;       // Label for slide content (template)
    [SerializeField] private ScrollRect scrollRect;      // Scroll View for resetting scroll position
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button completeButton;  // Only enabled on last slide
    [SerializeField] private Transform contentContainer; // Parent for dynamically created Labels

    [Header("Panel References")]
    [SerializeField] private GameObject sheetPanel; // SheetPanel to move on lesson start
    [SerializeField] private GameObject selectLessonPanel; // SelectLessonPanel to show on back

    [Header("TopBar Buttons")]
    [SerializeField] private Button backButton; // Back button in TopBar

    [Header("Code Block Settings")]
    [SerializeField] private Color codeBackgroundColor = new Color(0.169f, 0.169f, 0.169f, 1f); // #2B2B2B
    [SerializeField] private float codePadding = 20f;
    [SerializeField] private string currentLanguage = "python";

    [Header("Slide Settings")]
    [SerializeField] private string exerciseTitleKey = "";
    [SerializeField] private string slideKeyPrefix = "python_lesson1_ex1";
    [SerializeField] private int totalSlides = 3;

    private int currentSlideIndex = 0;
    private List<GameObject> dynamicContentObjects = new List<GameObject>();

    // Store the last position/rotation of SlidePanel before it was deactivated
    private Vector3 lastSlidePanelPosition;
    private Quaternion lastSlidePanelRotation;

    /// <summary>
    /// Gets the last position of the SlidePanel before it was deactivated
    /// </summary>
    public Vector3 LastSlidePanelPosition => lastSlidePanelPosition;

    /// <summary>
    /// Gets the last rotation of the SlidePanel before it was deactivated
    /// </summary>
    public Quaternion LastSlidePanelRotation => lastSlidePanelRotation;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Auto-find UI references in Awake so they're available before other scripts call SetSlideConfig
        FindUIReferences();
    }

    private void Start()
    {
        // Register button listeners
        if (previousButton != null)
        {
            previousButton.onClick.AddListener(PreviousSlide);
        }
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(NextSlide);
        }
        if (completeButton != null)
        {
            completeButton.onClick.AddListener(OnCompleteButtonClicked);
        }
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }

        // Load first slide
        LoadSlide(0);
    }

    /// <summary>
    /// Called when the back button in TopBar is clicked - returns to lesson selection
    /// </summary>
    private void OnBackButtonClicked()
    {
        Debug.Log("[SlideManager] Back button clicked - returning to lesson selection");

        // NOTE: Do NOT mark lesson as completed here.
        // Lesson completion only happens when the exercise is actually finished
        // (all code lines completed and ConsolePanel is shown)

        // Find SelectLessonPanel if not cached
        if (selectLessonPanel == null)
        {
            selectLessonPanel = FindObjectByName("SelectLessonPanel");
        }

        if (selectLessonPanel == null)
        {
            Debug.LogError("[SlideManager] SelectLessonPanel not found!");
            return;
        }

        // Store current position
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        // Position lesson panel at current slide panel position
        selectLessonPanel.transform.position = currentPosition;
        selectLessonPanel.transform.rotation = currentRotation;

        // Show lesson panel
        selectLessonPanel.SetActive(true);
        Debug.Log($"[SlideManager] Showed SelectLessonPanel at {currentPosition}");

        // Hide this slide panel
        gameObject.SetActive(false);
        Debug.Log("[SlideManager] Hidden SlidePanel");
    }

    /// <summary>
    /// Finds a GameObject by name in the scene (including inactive)
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
    /// Called when the complete/start lesson button is clicked
    /// </summary>
    private void OnCompleteButtonClicked()
    {
        Debug.Log("[SlideManager] Complete button clicked - starting lesson");

        // Save SlidePanel's position before deactivating
        lastSlidePanelPosition = transform.position;
        lastSlidePanelRotation = transform.rotation;
        Debug.Log($"[SlideManager] Saved SlidePanel position: {lastSlidePanelPosition}");

        // Move SheetPanel to SlidePanel's position and deactivate SlidePanel
        if (sheetPanel != null)
        {
            // Copy SlidePanel's position and rotation to SheetPanel
            sheetPanel.transform.position = transform.position;
            sheetPanel.transform.rotation = transform.rotation;
            sheetPanel.SetActive(true);
            Debug.Log($"[SlideManager] Moved SheetPanel to SlidePanel position: {transform.position}");

            // Deactivate SlidePanel (this game object)
            gameObject.SetActive(false);
            Debug.Log("[SlideManager] Deactivated SlidePanel");
        }
        else
        {
            Debug.LogWarning("[SlideManager] SheetPanel reference is not set!");
        }

        // Find LessonManager and start spawning blocks
        LessonManager lessonManager = FindObjectOfType<LessonManager>();
        if (lessonManager != null)
        {
            lessonManager.SpawnBlocksForFirstCodeLine();
        }
        else
        {
            Debug.LogWarning("[SlideManager] LessonManager not found!");
        }
    }

    private void FindUIReferences()
    {
        // Find exercise title text in TopBar
        if (exerciseTitleText == null)
        {
            Transform topBarTitle = transform.Find("PanelInteractable/PanelCanvas/TopBar/Horizontal/Text (TMP) (1)");
            if (topBarTitle != null)
            {
                exerciseTitleText = topBarTitle.GetComponent<TMP_Text>();

                // Disable LocalizationText component to prevent it from overwriting our text
                LocalizationText locText = topBarTitle.GetComponent<LocalizationText>();
                if (locText != null)
                {
                    locText.enabled = false;
                    Debug.Log("[SlideManager] Disabled LocalizationText on TopBar title");
                }

                Debug.Log("[SlideManager] Found exerciseTitleText");
            }
            else
            {
                Debug.LogWarning("[SlideManager] Could not find TopBar title at expected path");
            }
        }

        // Find back button in TopBar
        if (backButton == null)
        {
            Transform backBtnTransform = transform.Find("PanelInteractable/PanelCanvas/TopBar/Horizontal/Vertical/Horizontal/PrimaryButton_IconAndLabel_UnityUIButton (2)");
            if (backBtnTransform != null)
            {
                backButton = backBtnTransform.GetComponent<Button>();
                Debug.Log("[SlideManager] Found backButton");
            }
            else
            {
                Debug.LogWarning("[SlideManager] Could not find back button at expected path");
            }
        }

        string basePath = "PanelInteractable/PanelCanvas/Content/Scroll View/Viewport/Content/Text";

        // Find slide title text (Title object)
        if (slideTitleText == null)
        {
            Transform titleTransform = transform.Find($"{basePath}/Title");
            if (titleTransform != null)
            {
                slideTitleText = titleTransform.GetComponent<TMP_Text>();
                Debug.Log("[SlideManager] Found slideTitleText");
            }
            else
            {
                Debug.LogWarning("[SlideManager] Could not find Title at expected path");
            }
        }

        // Find slide image (Image object)
        if (slideImage == null)
        {
            Transform imageTransform = transform.Find($"{basePath}/Image");
            if (imageTransform != null)
            {
                slideImage = imageTransform.GetComponent<Image>();
                Debug.Log("[SlideManager] Found slideImage");
            }
            else
            {
                Debug.LogWarning("[SlideManager] Could not find Image at expected path");
            }
        }

        // Find content text (Label) - used as template
        if (contentText == null)
        {
            Transform labelTransform = transform.Find($"{basePath}/Label");
            if (labelTransform != null)
            {
                contentText = labelTransform.GetComponent<TMP_Text>();
                Debug.Log("[SlideManager] Found contentText");
            }
            else
            {
                Debug.LogWarning("[SlideManager] Could not find Label at expected path");
            }
        }

        // Find content container (Text parent with VerticalLayoutGroup)
        if (contentContainer == null)
        {
            Transform textContainer = transform.Find(basePath);
            if (textContainer != null)
            {
                contentContainer = textContainer;
                Debug.Log("[SlideManager] Found contentContainer");
            }
            else
            {
                Debug.LogWarning("[SlideManager] Could not find content container at expected path");
            }
        }

        // Find ScrollRect (Scroll View)
        if (scrollRect == null)
        {
            Transform scrollViewTransform = transform.Find("PanelInteractable/PanelCanvas/Content/Scroll View");
            if (scrollViewTransform != null)
            {
                scrollRect = scrollViewTransform.GetComponent<ScrollRect>();
                Debug.Log("[SlideManager] Found scrollRect");
            }
            else
            {
                Debug.LogWarning("[SlideManager] Could not find Scroll View at expected path");
            }
        }

        // Find buttons in BottomBar
        Transform bottomBar = transform.Find("PanelInteractable/PanelCanvas/BottomBar/Horizontal");
        if (bottomBar != null)
        {
            if (previousButton == null)
            {
                Transform prevBtnTransform = bottomBar.Find("PrimaryButton_IconAndLabel_UnityUIButton (2)");
                if (prevBtnTransform != null)
                {
                    previousButton = prevBtnTransform.GetComponent<Button>();
                    Debug.Log("[SlideManager] Found previousButton");
                }
            }

            if (nextButton == null)
            {
                Transform nextBtnTransform = bottomBar.Find("PrimaryButton_IconAndLabel_UnityUIButton (3)");
                if (nextBtnTransform != null)
                {
                    nextButton = nextBtnTransform.GetComponent<Button>();
                    Debug.Log("[SlideManager] Found nextButton");
                }
            }

            if (completeButton == null)
            {
                Transform completeBtnTransform = bottomBar.Find("PrimaryButton_IconAndLabel_UnityUIButton (4)");
                if (completeBtnTransform != null)
                {
                    completeButton = completeBtnTransform.GetComponent<Button>();
                    Debug.Log("[SlideManager] Found completeButton");
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (previousButton != null)
        {
            previousButton.onClick.RemoveListener(PreviousSlide);
        }
        if (nextButton != null)
        {
            nextButton.onClick.RemoveListener(NextSlide);
        }
        if (completeButton != null)
        {
            completeButton.onClick.RemoveListener(OnCompleteButtonClicked);
        }
        if (backButton != null)
        {
            backButton.onClick.RemoveListener(OnBackButtonClicked);
        }
    }

    /// <summary>
    /// Sets only the exercise title in TopBar without loading slides
    /// </summary>
    public void SetExerciseTitle(string exerciseTitle)
    {
        exerciseTitleKey = exerciseTitle;

        if (exerciseTitleText != null && LocalizationManager.Instance != null)
        {
            // Disable LocalizationText component to prevent overwriting
            LocalizationText locText = exerciseTitleText.GetComponent<LocalizationText>();
            if (locText != null)
            {
                locText.enabled = false;
            }

            exerciseTitleText.font = LocalizationManager.Instance.GetLocalizedFont();
            string titleText = LocalizationManager.Instance.GetText(exerciseTitleKey);
            exerciseTitleText.text = titleText;
            Debug.Log($"[SlideManager] Set TopBar title to: '{titleText}' (key: {exerciseTitleKey})");
        }
        else
        {
            Debug.LogWarning($"[SlideManager] Cannot set TopBar title - exerciseTitleText:{exerciseTitleText != null}, LocalizationManager:{LocalizationManager.Instance != null}");
        }
    }

    public void SetSlideConfig(string exerciseTitle, string keyPrefix, int slideCount)
    {
        exerciseTitleKey = exerciseTitle;
        slideKeyPrefix = keyPrefix;
        totalSlides = slideCount;
        currentSlideIndex = 0;

        // Set exercise title in TopBar
        if (exerciseTitleText != null && LocalizationManager.Instance != null)
        {
            // Disable LocalizationText component to prevent overwriting
            LocalizationText locText = exerciseTitleText.GetComponent<LocalizationText>();
            if (locText != null)
            {
                locText.enabled = false;
            }

            exerciseTitleText.font = LocalizationManager.Instance.GetLocalizedFont();
            string titleText = LocalizationManager.Instance.GetText(exerciseTitleKey);
            exerciseTitleText.text = titleText;
            Debug.Log($"[SlideManager] Set TopBar title to: '{titleText}' (key: {exerciseTitleKey})");
        }
        else
        {
            Debug.LogWarning($"[SlideManager] Cannot set TopBar title - exerciseTitleText:{exerciseTitleText != null}, LocalizationManager:{LocalizationManager.Instance != null}");
        }

        LoadSlide(0);
    }

    public void LoadSlide(int index)
    {
        if (index < 0 || index >= totalSlides)
        {
            Debug.LogWarning($"[SlideManager] Invalid slide index: {index}");
            return;
        }

        currentSlideIndex = index;

        if (LocalizationManager.Instance == null)
        {
            Debug.LogWarning("[SlideManager] LocalizationManager not available");
            return;
        }

        // Load slide page title and content (e.g., python_lesson1_ex1_slide1_title)
        string titleKey = $"{slideKeyPrefix}_slide{index + 1}_title";
        string contentKey = $"{slideKeyPrefix}_slide{index + 1}_content";

        if (slideTitleText != null)
        {
            slideTitleText.font = LocalizationManager.Instance.GetLocalizedFont();
            slideTitleText.text = LocalizationManager.Instance.GetText(titleKey);
        }

        // Parse and display content with separate Labels for text and code
        string rawContent = LocalizationManager.Instance.GetText(contentKey);
        DisplaySplitContent(rawContent);

        // Disable slide images completely
        if (slideImage != null)
        {
            slideImage.gameObject.SetActive(false);
        }

        // Force layout rebuild to recalculate sizes
        if (contentContainer != null)
        {
            Canvas.ForceUpdateCanvases();
            RectTransform containerRect = contentContainer as RectTransform;
            if (containerRect != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(containerRect);
            }
            // Also rebuild parent layouts
            Transform parent = contentContainer.parent;
            while (parent != null && parent != transform)
            {
                RectTransform parentRect = parent as RectTransform;
                if (parentRect != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
                }
                parent = parent.parent;
            }
        }

        // Reset scroll position to top
        if (scrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 1f;
        }

        UpdateButtonStates();

        Debug.Log($"[SlideManager] Loaded slide {index + 1}/{totalSlides}");
    }

    public void NextSlide()
    {
        if (currentSlideIndex < totalSlides - 1)
        {
            LoadSlide(currentSlideIndex + 1);
        }
        else
        {
            Debug.Log("[SlideManager] Already at last slide");
        }
    }

    public void PreviousSlide()
    {
        if (currentSlideIndex > 0)
        {
            LoadSlide(currentSlideIndex - 1);
        }
        else
        {
            Debug.Log("[SlideManager] Already at first slide");
        }
    }

    private void UpdateButtonStates()
    {
        bool prevEnabled = currentSlideIndex > 0;
        bool nextEnabled = currentSlideIndex < totalSlides - 1;
        bool completeEnabled = currentSlideIndex >= totalSlides - 1;

        Debug.Log($"[SlideManager] UpdateButtonStates: slide={currentSlideIndex}, total={totalSlides}, prev={prevEnabled}, next={nextEnabled}, complete={completeEnabled}");

        // Disable previous button on first slide
        if (previousButton != null)
        {
            previousButton.interactable = prevEnabled;
            UpdateButtonAnimator(previousButton, prevEnabled);
        }

        // Disable next button on last slide
        if (nextButton != null)
        {
            nextButton.interactable = nextEnabled;
            UpdateButtonAnimator(nextButton, nextEnabled);
        }

        // Enable complete button only on last slide
        if (completeButton != null)
        {
            completeButton.interactable = completeEnabled;
            UpdateButtonAnimator(completeButton, completeEnabled);
        }
    }

    private void UpdateButtonAnimator(Button button, bool enabled)
    {
        // Force button to update its visual state
        Animator animator = button.GetComponent<Animator>();
        if (animator != null)
        {
            // Reset all triggers first
            animator.ResetTrigger("Normal");
            animator.ResetTrigger("Highlighted");
            animator.ResetTrigger("Pressed");
            animator.ResetTrigger("Selected");
            animator.ResetTrigger("Disabled");

            // Set the correct trigger
            animator.SetTrigger(enabled ? "Normal" : "Disabled");
            animator.Update(0f);
        }

        // Also set alpha on child CanvasGroup or create one
        CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = button.gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = enabled ? 1f : 0.5f;
        canvasGroup.blocksRaycasts = enabled;
    }

    public int CurrentSlideIndex => currentSlideIndex;
    public int TotalSlides => totalSlides;

    #region Content Splitting and Code Highlighting

    /// <summary>
    /// Content section type
    /// </summary>
    private enum ContentSectionType
    {
        Text,
        Code
    }

    /// <summary>
    /// Represents a section of content (text or code)
    /// </summary>
    private class ContentSection
    {
        public ContentSectionType Type;
        public string Content;
    }

    /// <summary>
    /// Clears all dynamically created content objects
    /// </summary>
    private void ClearDynamicContent()
    {
        foreach (var obj in dynamicContentObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        dynamicContentObjects.Clear();
    }

    /// <summary>
    /// Parses content and splits it into text and code sections
    /// Supports:
    /// - Markdown code blocks (```language ... ```)
    /// - Lines starting with 2+ spaces (legacy)
    /// - Output lines starting with =>
    /// Also converts inline `code` to styled text
    /// </summary>
    private List<ContentSection> ParseContent(string content)
    {
        List<ContentSection> sections = new List<ContentSection>();
        if (string.IsNullOrEmpty(content)) return sections;

        // DEBUG: Check raw content
        Debug.Log($"[SlideManager] ParseContent RAW length={content.Length}, has backslash-n={(content.Contains("\\n"))}, has triple backtick={(content.Contains("```"))}");

        // Convert escaped newlines to actual newlines
        // JSON stores \\n which becomes \n (literal backslash-n) after JSON parsing
        // We need to convert this to actual newline characters
        content = content.Replace("\\n", "\n");

        // DEBUG: Check after replace
        Debug.Log($"[SlideManager] ParseContent AFTER REPLACE has triple backtick={(content.Contains("```"))}");

        string[] lines = content.Split('\n');
        Debug.Log($"[SlideManager] ParseContent: split into {lines.Length} lines");

        // DEBUG: Log first few lines
        for (int i = 0; i < Mathf.Min(5, lines.Length); i++)
        {
            Debug.Log($"[SlideManager] Line {i}: '{lines[i]}'");
        }
        StringBuilder currentText = new StringBuilder();
        StringBuilder currentCode = new StringBuilder();
        bool inCodeBlock = false;
        bool inMarkdownCodeBlock = false;

        foreach (string line in lines)
        {
            // Check for markdown code block start/end (``` or ```language)
            if (line.TrimStart().StartsWith("```"))
            {
                if (!inMarkdownCodeBlock)
                {
                    // Starting a markdown code block
                    // Save any accumulated text first
                    if (currentText.Length > 0)
                    {
                        sections.Add(new ContentSection
                        {
                            Type = ContentSectionType.Text,
                            Content = ProcessInlineCode(currentText.ToString().TrimEnd('\n'))
                        });
                        currentText.Clear();
                    }
                    inMarkdownCodeBlock = true;
                    inCodeBlock = true;
                    // Don't add the ``` line itself to code
                    continue;
                }
                else
                {
                    // Ending a markdown code block
                    if (currentCode.Length > 0)
                    {
                        sections.Add(new ContentSection
                        {
                            Type = ContentSectionType.Code,
                            Content = currentCode.ToString()
                        });
                        currentCode.Clear();
                    }
                    inMarkdownCodeBlock = false;
                    inCodeBlock = false;
                    // Don't add the closing ``` line
                    continue;
                }
            }

            // If inside markdown code block, add line as code
            if (inMarkdownCodeBlock)
            {
                if (currentCode.Length > 0) currentCode.Append("\n");
                currentCode.Append(line);
                continue;
            }

            // Legacy: Check if line is code (starts with 2+ spaces or is indented code continuation)
            bool isCodeLine = line.StartsWith("  ") || (line.StartsWith("=>") && inCodeBlock);

            // Also treat "=>" output lines as code
            if (line.TrimStart().StartsWith("=>"))
            {
                isCodeLine = true;
            }

            if (isCodeLine)
            {
                // If we were in text mode, save the text section
                if (!inCodeBlock && currentText.Length > 0)
                {
                    sections.Add(new ContentSection
                    {
                        Type = ContentSectionType.Text,
                        Content = ProcessInlineCode(currentText.ToString().TrimEnd('\n'))
                    });
                    currentText.Clear();
                }

                inCodeBlock = true;
                if (currentCode.Length > 0) currentCode.Append("\n");
                // Remove leading 2 spaces from code lines for display
                string codeLine = line.StartsWith("  ") ? line.Substring(2) : line;
                currentCode.Append(codeLine);
            }
            else
            {
                // If we were in code mode, save the code section
                if (inCodeBlock && currentCode.Length > 0)
                {
                    sections.Add(new ContentSection
                    {
                        Type = ContentSectionType.Code,
                        Content = currentCode.ToString()
                    });
                    currentCode.Clear();
                }

                inCodeBlock = false;
                if (currentText.Length > 0) currentText.Append("\n");
                currentText.Append(line);
            }
        }

        // Add any remaining content
        if (currentText.Length > 0)
        {
            sections.Add(new ContentSection
            {
                Type = ContentSectionType.Text,
                Content = ProcessInlineCode(currentText.ToString().TrimEnd('\n'))
            });
        }
        if (currentCode.Length > 0)
        {
            sections.Add(new ContentSection
            {
                Type = ContentSectionType.Code,
                Content = currentCode.ToString()
            });
        }

        return sections;
    }

    /// <summary>
    /// Processes inline code (text between backticks) and converts to styled text
    /// Example: `code here` becomes <color=#DCDCAA><b>code here</b></color>
    /// </summary>
    private string ProcessInlineCode(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        // DEBUG: Check for backticks
        int backtickCount = 0;
        foreach (char c in text) if (c == '`') backtickCount++;
        Debug.Log($"[SlideManager] ProcessInlineCode: text length={text.Length}, backtick count={backtickCount}");

        // Get the color for inline code (use function definition color for visibility)
        string codeColor = ColorToHex(TextBasedScaler.SyntaxColors.FunctionDef);

        // Replace `code` with colored/styled version
        // Use regex to match content between backticks
        var regex = new Regex(@"`([^`]+)`");
        var result = regex.Replace(text, match =>
        {
            string code = match.Groups[1].Value;
            Debug.Log($"[SlideManager] Found inline code: {code}");
            return $"<color={codeColor}><b>{code}</b></color>";
        });

        Debug.Log($"[SlideManager] ProcessInlineCode result length={result.Length}");
        return result;
    }

    /// <summary>
    /// Displays content split into separate Labels for text and code sections
    /// </summary>
    private void DisplaySplitContent(string rawContent)
    {
        // Clear previous dynamic content
        ClearDynamicContent();

        // Hide the template Label
        if (contentText != null)
        {
            contentText.gameObject.SetActive(false);
        }

        if (contentContainer == null || contentText == null)
        {
            Debug.LogWarning("[SlideManager] Content container or template not found");
            return;
        }

        // DEBUG: Log raw content
        Debug.Log($"[SlideManager] Raw content length: {rawContent?.Length ?? 0}");
        Debug.Log($"[SlideManager] Contains backtick: {rawContent?.Contains("`") ?? false}");
        Debug.Log($"[SlideManager] Contains triple backtick: {rawContent?.Contains("```") ?? false}");

        // Parse content into sections
        List<ContentSection> sections = ParseContent(rawContent);

        TMP_FontAsset localizedFont = LocalizationManager.Instance?.GetLocalizedFont();

        // Create a Label for each section
        foreach (var section in sections)
        {
            if (string.IsNullOrWhiteSpace(section.Content)) continue;

            bool isCode = section.Type == ContentSectionType.Code;
            GameObject containerObj;
            TMP_Text tmpText;

            CreateContentLabel(isCode, out containerObj, out tmpText);
            if (containerObj == null || tmpText == null) continue;

            if (localizedFont != null)
            {
                tmpText.font = localizedFont;
            }
            tmpText.richText = true;

            if (isCode)
            {
                // Apply syntax highlighting to code
                tmpText.text = ApplySyntaxHighlighting(section.Content);
            }
            else
            {
                tmpText.text = section.Content;
            }

            tmpText.ForceMeshUpdate();
            dynamicContentObjects.Add(containerObj);
        }

        Debug.Log($"[SlideManager] Created {dynamicContentObjects.Count} content sections");
    }

    /// <summary>
    /// Creates a content Label, optionally with code block styling
    /// </summary>
    private void CreateContentLabel(bool isCodeBlock, out GameObject containerObj, out TMP_Text tmpText)
    {
        containerObj = null;
        tmpText = null;

        if (contentText == null || contentContainer == null) return;

        // Get the container width from parent RectTransform
        RectTransform containerRect = contentContainer as RectTransform;
        float containerWidth = containerRect != null && containerRect.rect.width > 0 ? containerRect.rect.width : 1700f;

        TextMeshProUGUI templateTmp = contentText as TextMeshProUGUI;

        if (isCodeBlock)
        {
            // Create container GameObject for code block (with background)
            GameObject labelObj = new GameObject("CodeBlock");
            labelObj.transform.SetParent(contentContainer, false);

            // Set up RectTransform with explicit size (parent VerticalLayoutGroup has childControlWidth=false)
            RectTransform rectTransform = labelObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(containerWidth, 0); // Width explicit, height auto

            // Add HorizontalLayoutGroup for padding
            HorizontalLayoutGroup hlg = labelObj.AddComponent<HorizontalLayoutGroup>();
            hlg.padding = new RectOffset((int)codePadding, (int)codePadding, (int)codePadding, (int)codePadding);
            hlg.childAlignment = TextAnchor.UpperLeft;
            hlg.childControlWidth = true;
            hlg.childControlHeight = true;
            hlg.childForceExpandWidth = true;
            hlg.childForceExpandHeight = false;

            // Add background Image for code blocks
            Image bgImage = labelObj.AddComponent<Image>();
            bgImage.color = codeBackgroundColor;

            // Add ContentSizeFitter to auto-size height based on content
            ContentSizeFitter fitter = labelObj.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Add LayoutElement with explicit preferred width
            LayoutElement layoutElement = labelObj.AddComponent<LayoutElement>();
            layoutElement.preferredWidth = containerWidth;
            layoutElement.minWidth = containerWidth;

            // Create child for text
            GameObject textChild = new GameObject("Text");
            textChild.transform.SetParent(labelObj.transform, false);

            RectTransform textRect = textChild.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.pivot = new Vector2(0.5f, 0.5f);
            textRect.sizeDelta = Vector2.zero;

            // Add TextMeshProUGUI to child
            TextMeshProUGUI codeTmpText = textChild.AddComponent<TextMeshProUGUI>();
            CopyTextSettings(templateTmp, codeTmpText);
            codeTmpText.color = TextBasedScaler.SyntaxColors.Foreground; // Light gray text for code

            containerObj = labelObj;
            tmpText = codeTmpText;
        }
        else
        {
            // Create regular text Label
            GameObject labelObj = new GameObject("TextBlock");
            labelObj.transform.SetParent(contentContainer, false);

            // Set up RectTransform with explicit size (parent VerticalLayoutGroup has childControlWidth=false)
            RectTransform rectTransform = labelObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(containerWidth, 0); // Width explicit, height auto

            // Add TextMeshProUGUI
            TextMeshProUGUI textTmpText = labelObj.AddComponent<TextMeshProUGUI>();
            CopyTextSettings(templateTmp, textTmpText);

            // Add ContentSizeFitter to auto-size height based on content
            ContentSizeFitter fitter = labelObj.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Add LayoutElement with explicit preferred width
            LayoutElement layoutElement = labelObj.AddComponent<LayoutElement>();
            layoutElement.preferredWidth = containerWidth;
            layoutElement.minWidth = containerWidth;

            containerObj = labelObj;
            tmpText = textTmpText;
        }
    }

    /// <summary>
    /// Copies text settings from source to target TextMeshProUGUI
    /// </summary>
    private void CopyTextSettings(TextMeshProUGUI source, TextMeshProUGUI target)
    {
        if (source == null || target == null) return;

        target.font = source.font;
        target.fontSize = source.fontSize;
        target.fontStyle = source.fontStyle;
        target.alignment = source.alignment;
        target.color = source.color;
        target.enableWordWrapping = source.enableWordWrapping;
        target.overflowMode = source.overflowMode;
        target.margin = source.margin;
        target.richText = true;
    }

    /// <summary>
    /// Applies syntax highlighting to code text
    /// </summary>
    private string ApplySyntaxHighlighting(string code)
    {
        if (string.IsNullOrEmpty(code)) return code;

        var config = GetLanguageConfig(currentLanguage);
        if (config == null) return code;

        StringBuilder result = new StringBuilder();
        string[] lines = code.Split('\n');

        for (int lineIdx = 0; lineIdx < lines.Length; lineIdx++)
        {
            if (lineIdx > 0) result.Append("\n");

            string line = lines[lineIdx];

            // Check for output line (starts with =>)
            if (line.TrimStart().StartsWith("=>"))
            {
                string outputColor = ColorToHex(TextBasedScaler.SyntaxColors.Comment);
                result.Append($"<color={outputColor}>{line}</color>");
                continue;
            }

            // Tokenize and colorize
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
        }

        return result.ToString();
    }

    private string[] TokenizeLine(string line)
    {
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
