using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Spawns course buttons dynamically at runtime in the SelectCoursePanel.
/// </summary>
public class CourseButtonSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject courseButtonPrefab;

    [Header("Spawn Container")]
    [SerializeField] private Transform buttonContainer;

    [Header("Panel References")]
    [SerializeField] private GameObject selectLessonPanel;
    [SerializeField] private GameObject selectCoursePanel;

    [Header("Course Data")]
    [SerializeField] private List<CourseData> courses = new List<CourseData>();

    [Header("Language Selection")]
    [SerializeField] private int currentLanguageIndex = 0; // 0=Python, 1=JavaScript, etc.
    private string currentLanguage = "python";

    private List<GameObject> spawnedButtons = new List<GameObject>();
    
    [System.Serializable]
    public class CourseData
    {
        public string courseTitleKey = "python_lesson1_course_title";
        public string courseDescriptionKey = "python_lesson1_course_description";
        public string difficultyKey = "difficulty_easy";
        public int totalExercises = 10;
        public int completedExercises = 0;
    }
    
    private void Awake()
    {
        // Initialize default courses if none are configured
        if (courses.Count == 0)
        {
            InitializeDefaultCourses();
        }

        // Load prefab from Resources if not assigned
        LoadPrefabIfNeeded();

        // Find panels if not assigned
        FindPanelsIfNeeded();
    }

    /// <summary>
    /// Finds panel references if not assigned in Inspector
    /// </summary>
    private void FindPanelsIfNeeded()
    {
        if (selectLessonPanel == null)
        {
            selectLessonPanel = FindObjectByName("SelectLessonPanel");
            if (selectLessonPanel != null)
            {
                Debug.Log("[CourseButtonSpawner] Found SelectLessonPanel");
            }
        }

        if (selectCoursePanel == null)
        {
            // Try to find parent SelectCoursePanel
            Transform parent = transform;
            while (parent != null)
            {
                if (parent.name == "SelectCoursePanel")
                {
                    selectCoursePanel = parent.gameObject;
                    Debug.Log("[CourseButtonSpawner] Found SelectCoursePanel (parent)");
                    break;
                }
                parent = parent.parent;
            }

            // If still not found, search in scene
            if (selectCoursePanel == null)
            {
                selectCoursePanel = FindObjectByName("SelectCoursePanel");
                if (selectCoursePanel != null)
                {
                    Debug.Log("[CourseButtonSpawner] Found SelectCoursePanel (scene search)");
                }
            }
        }
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
    /// Recursively finds a child transform by name
    /// </summary>
    private Transform FindDeepChild(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            Transform found = FindDeepChild(child, name);
            if (found != null) return found;
        }
        return null;
    }

    private void Start()
    {
        // Only auto-spawn if we have valid references
        if (courseButtonPrefab != null && buttonContainer != null)
        {
            SpawnCourseButtons();
        }
    }

    private void OnEnable()
    {
        // Re-spawn buttons when panel becomes active to reflect latest progress
        if (courseButtonPrefab != null && buttonContainer != null)
        {
            SpawnCourseButtons();
            Debug.Log("[CourseButtonSpawner] OnEnable - re-spawned buttons to reflect progress");
        }
    }

    /// <summary>
    /// Loads the course button prefab from Resources if not already assigned
    /// </summary>
    private void LoadPrefabIfNeeded()
    {
        // Use own transform as container if not assigned
        if (buttonContainer == null)
        {
            // Try to find Content container
            Transform content = FindContentContainer(transform);
            buttonContainer = content != null ? content : transform;
            Debug.Log($"[CourseButtonSpawner] Using {buttonContainer.name} as button container");
        }

        if (courseButtonPrefab == null)
        {
            // Try to use existing button as template
            Button existingButton = buttonContainer.GetComponentInChildren<Button>();
            if (existingButton != null)
            {
                // Create a copy to use as prefab template
                courseButtonPrefab = existingButton.gameObject;
                Debug.Log("[CourseButtonSpawner] Using existing button as template");
            }
            else
            {
                // Try to load from Resources
                courseButtonPrefab = Resources.Load<GameObject>("Prefabs/TextTileButton_Course Variant");
                if (courseButtonPrefab == null)
                {
                    courseButtonPrefab = Resources.Load<GameObject>("Prefabs/TextTileButton_Lesson Variant");
                }
                if (courseButtonPrefab != null)
                {
                    Debug.Log("[CourseButtonSpawner] Loaded prefab from Resources");
                }
            }
        }
    }

    /// <summary>
    /// Finds the Content container (Scroll View/Viewport/Content)
    /// </summary>
    private Transform FindContentContainer(Transform parent)
    {
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

        // Try direct Content child
        Transform directContent = parent.Find("Content");
        if (directContent != null) return directContent;

        return null;
    }

    /// <summary>
    /// Sets the button container (for dynamic setup)
    /// </summary>
    public void SetButtonContainer(Transform container)
    {
        buttonContainer = container;
    }

    /// <summary>
    /// Sets the course button prefab (for dynamic setup)
    /// </summary>
    public void SetButtonPrefab(GameObject prefab)
    {
        courseButtonPrefab = prefab;
    }

    /// <summary>
    /// Initializes course data based on the current language
    /// </summary>
    private void InitializeDefaultCourses()
    {
        InitializeCoursesForLanguage(currentLanguage);
    }

    /// <summary>
    /// Initializes courses for the specified language
    /// </summary>
    private void InitializeCoursesForLanguage(string language)
    {
        courses.Clear();

        switch (language.ToLower())
        {
            case "python":
                InitializePythonCourses();
                break;
            case "javascript":
                InitializeJavaScriptCourses();
                break;
            case "typescript":
                InitializeTypeScriptCourses();
                break;
            default:
                Debug.LogWarning($"[CourseButtonSpawner] Unknown language: {language}, defaulting to Python");
                InitializePythonCourses();
                break;
        }
    }

    /// <summary>
    /// Initializes Python courses
    /// </summary>
    private void InitializePythonCourses()
    {
        courses.Add(new CourseData
        {
            courseTitleKey = "python_lesson1_course_title",
            courseDescriptionKey = "python_lesson1_course_description",
            difficultyKey = "difficulty_easy",
            totalExercises = 13,
            completedExercises = 0
        });

        courses.Add(new CourseData
        {
            courseTitleKey = "python_lesson2_course_title",
            courseDescriptionKey = "python_lesson2_course_description",
            difficultyKey = "difficulty_easy",
            totalExercises = 12,
            completedExercises = 0
        });

        courses.Add(new CourseData
        {
            courseTitleKey = "python_lesson3_course_title",
            courseDescriptionKey = "python_lesson3_course_description",
            difficultyKey = "difficulty_medium",
            totalExercises = 10,
            completedExercises = 0
        });

        courses.Add(new CourseData
        {
            courseTitleKey = "python_lesson4_course_title",
            courseDescriptionKey = "python_lesson4_course_description",
            difficultyKey = "difficulty_medium",
            totalExercises = 10,
            completedExercises = 0
        });

        courses.Add(new CourseData
        {
            courseTitleKey = "python_lesson5_course_title",
            courseDescriptionKey = "python_lesson5_course_description",
            difficultyKey = "difficulty_hard",
            totalExercises = 10,
            completedExercises = 0
        });

        Debug.Log("[CourseButtonSpawner] Initialized 5 Python courses");
    }

    /// <summary>
    /// Initializes JavaScript courses
    /// </summary>
    private void InitializeJavaScriptCourses()
    {
        courses.Add(new CourseData
        {
            courseTitleKey = "javascript_lesson1_course_title",
            courseDescriptionKey = "javascript_lesson1_course_description",
            difficultyKey = "difficulty_easy",
            totalExercises = 10,
            completedExercises = 0
        });

        courses.Add(new CourseData
        {
            courseTitleKey = "javascript_lesson2_course_title",
            courseDescriptionKey = "javascript_lesson2_course_description",
            difficultyKey = "difficulty_medium",
            totalExercises = 13,
            completedExercises = 0
        });

        courses.Add(new CourseData
        {
            courseTitleKey = "javascript_lesson3_course_title",
            courseDescriptionKey = "javascript_lesson3_course_description",
            difficultyKey = "difficulty_hard",
            totalExercises = 10,
            completedExercises = 0
        });

        courses.Add(new CourseData
        {
            courseTitleKey = "javascript_lesson4_course_title",
            courseDescriptionKey = "javascript_lesson4_course_description",
            difficultyKey = "difficulty_hard",
            totalExercises = 10,
            completedExercises = 0
        });

        courses.Add(new CourseData
        {
            courseTitleKey = "javascript_lesson5_course_title",
            courseDescriptionKey = "javascript_lesson5_course_description",
            difficultyKey = "difficulty_hard",
            totalExercises = 10,
            completedExercises = 0
        });

        Debug.Log("[CourseButtonSpawner] Initialized 5 JavaScript courses");
    }

    /// <summary>
    /// Initializes TypeScript courses
    /// </summary>
    private void InitializeTypeScriptCourses()
    {
        courses.Add(new CourseData
        {
            courseTitleKey = "typescript_lesson1_course_title",
            courseDescriptionKey = "typescript_lesson1_course_description",
            difficultyKey = "difficulty_easy",
            totalExercises = 13,
            completedExercises = 0
        });

        courses.Add(new CourseData
        {
            courseTitleKey = "typescript_lesson2_course_title",
            courseDescriptionKey = "typescript_lesson2_course_description",
            difficultyKey = "difficulty_medium",
            totalExercises = 10,
            completedExercises = 0
        });

        courses.Add(new CourseData
        {
            courseTitleKey = "typescript_lesson3_course_title",
            courseDescriptionKey = "typescript_lesson3_course_description",
            difficultyKey = "difficulty_hard",
            totalExercises = 10,
            completedExercises = 0
        });

        courses.Add(new CourseData
        {
            courseTitleKey = "typescript_lesson4_course_title",
            courseDescriptionKey = "typescript_lesson4_course_description",
            difficultyKey = "difficulty_hard",
            totalExercises = 10,
            completedExercises = 0
        });

        Debug.Log("[CourseButtonSpawner] Initialized 4 TypeScript courses");
    }

    /// <summary>
    /// Spawns all course buttons based on the course data list
    /// </summary>
    public void SpawnCourseButtons()
    {
        if (courseButtonPrefab == null)
        {
            Debug.LogError("[CourseButtonSpawner] Course button prefab is not assigned!");
            return;
        }
        
        if (buttonContainer == null)
        {
            Debug.LogError("[CourseButtonSpawner] Button container is not assigned!");
            return;
        }
        
        // Clear existing buttons
        ClearSpawnedButtons();
        
        // Spawn buttons for each course
        for (int i = 0; i < courses.Count; i++)
        {
            SpawnCourseButton(courses[i], i);
        }
        
        Debug.Log($"[CourseButtonSpawner] Spawned {courses.Count} course buttons");
    }
    
    /// <summary>
    /// Spawns a single course button
    /// </summary>
    private void SpawnCourseButton(CourseData courseData, int index)
    {
        GameObject buttonObj = Instantiate(courseButtonPrefab, buttonContainer);
        buttonObj.name = $"TextTileButton_Course{index + 1}";

        // Get progress from ProgressManager
        int completedLessons = 0;
        bool isCourseComplete = false;
        bool isCourseLocked = false;

        if (ProgressManager.Instance != null)
        {
            completedLessons = ProgressManager.Instance.GetCompletedLessonCount(
                currentLanguage, index, courseData.totalExercises);
            isCourseComplete = ProgressManager.Instance.IsCourseCompleted(currentLanguage, index);
            isCourseLocked = !ProgressManager.Instance.IsCourseUnlocked(currentLanguage, index);
            Debug.Log($"[CourseButtonSpawner] Course {index}: {completedLessons}/{courseData.totalExercises} completed, complete={isCourseComplete}, locked={isCourseLocked}");
        }

        // Update completed exercises from progress
        courseData.completedExercises = completedLessons;

        // Configure SelectCoursePanelManager component
        SelectCoursePanelManager panelManager = buttonObj.GetComponent<SelectCoursePanelManager>();
        if (panelManager == null)
        {
            panelManager = buttonObj.AddComponent<SelectCoursePanelManager>();
        }

        // Configure panel with course data
        ConfigurePanelManager(panelManager, courseData);

        // Show/hide icon_pass based on course completion
        Transform iconPass = FindDeepChild(buttonObj.transform, "Icon_pass");
        if (iconPass != null)
        {
            iconPass.gameObject.SetActive(isCourseComplete);
            Debug.Log($"[CourseButtonSpawner] Set Icon_pass active={isCourseComplete} for course {index}");
        }

        // Show/hide icon_lock based on locked state
        Transform iconLock = FindDeepChild(buttonObj.transform, "Icon_lock");
        if (iconLock != null)
        {
            iconLock.gameObject.SetActive(isCourseLocked);
            Debug.Log($"[CourseButtonSpawner] Set Icon_lock active={isCourseLocked} for course {index}");
        }

        // Disable Animator and ButtonLabelHover if locked
        if (isCourseLocked)
        {
            Animator animator = buttonObj.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = false;
                Debug.Log($"[CourseButtonSpawner] Disabled Animator for locked course {index}");
            }

            ButtonLabelHover buttonLabelHover = buttonObj.GetComponent<ButtonLabelHover>();
            if (buttonLabelHover != null)
            {
                buttonLabelHover.enabled = false;
                Debug.Log($"[CourseButtonSpawner] Disabled ButtonLabelHover for locked course {index}");
            }
        }

        // Register onClick handler via AddListener
        Button button = buttonObj.GetComponent<Button>();
        if (button == null)
        {
            button = buttonObj.GetComponentInChildren<Button>();
        }

        if (button != null)
        {
            // Keep button enabled even if locked (check lock state on click)
            button.interactable = true;

            int capturedIndex = index; // Capture for closure
            button.onClick.AddListener(() => OnCourseButtonClicked(capturedIndex));
            Debug.Log($"[CourseButtonSpawner] Registered onClick for course {capturedIndex} (locked={isCourseLocked})");
        }
        else
        {
            Debug.LogWarning($"[CourseButtonSpawner] No Button component found on {buttonObj.name}");
        }

        spawnedButtons.Add(buttonObj);
        Debug.Log($"[CourseButtonSpawner] Spawned course button: {buttonObj.name}");
    }

    /// <summary>
    /// Called when a course button is clicked
    /// </summary>
    private void OnCourseButtonClicked(int courseIndex)
    {
        Debug.Log($"[CourseButtonSpawner] Course button clicked: index {courseIndex}");

        // Check if course is locked
        if (ProgressManager.Instance != null && !ProgressManager.Instance.IsCourseUnlocked(currentLanguage, courseIndex))
        {
            Debug.Log($"[CourseButtonSpawner] Course {courseIndex} is locked, ignoring click");
            return;
        }

        // Find panels if not cached
        if (selectLessonPanel == null)
        {
            selectLessonPanel = FindObjectByName("SelectLessonPanel");
        }
        if (selectCoursePanel == null)
        {
            selectCoursePanel = FindObjectByName("SelectCoursePanel");
        }

        if (selectLessonPanel == null)
        {
            Debug.LogError("[CourseButtonSpawner] SelectLessonPanel not found!");
            return;
        }

        // Store current position of course panel
        Vector3 currentPosition = Vector3.zero;
        Quaternion currentRotation = Quaternion.identity;

        if (selectCoursePanel != null)
        {
            currentPosition = selectCoursePanel.transform.position;
            currentRotation = selectCoursePanel.transform.rotation;
        }

        // Find LessonButtonSpawner and set the course index
        LessonButtonSpawner lessonSpawner = selectLessonPanel.GetComponent<LessonButtonSpawner>();
        if (lessonSpawner == null)
        {
            lessonSpawner = selectLessonPanel.GetComponentInChildren<LessonButtonSpawner>();
        }

        if (lessonSpawner != null)
        {
            lessonSpawner.SetCourseAndLanguage(courseIndex, currentLanguageIndex);
            Debug.Log($"[CourseButtonSpawner] Set LessonButtonSpawner to course {courseIndex}, language {currentLanguageIndex}");
        }
        else
        {
            Debug.LogWarning("[CourseButtonSpawner] LessonButtonSpawner not found on SelectLessonPanel");
        }

        // Position lesson panel at current course panel position
        selectLessonPanel.transform.position = currentPosition;
        selectLessonPanel.transform.rotation = currentRotation;

        // Show lesson panel
        selectLessonPanel.SetActive(true);
        Debug.Log($"[CourseButtonSpawner] Showed SelectLessonPanel at {currentPosition}");

        // Hide course panel
        if (selectCoursePanel != null)
        {
            selectCoursePanel.SetActive(false);
            Debug.Log("[CourseButtonSpawner] Hidden SelectCoursePanel");
        }
    }
    
    /// <summary>
    /// Configures the SelectCoursePanelManager with course data
    /// </summary>
    private void ConfigurePanelManager(SelectCoursePanelManager panelManager, CourseData courseData)
    {
        // Configure all course data including localization keys
        panelManager.Configure(
            courseData.courseTitleKey,
            courseData.courseDescriptionKey,
            courseData.difficultyKey,
            courseData.totalExercises,
            courseData.completedExercises
        );
    }
    
    /// <summary>
    /// Clears all spawned buttons
    /// </summary>
    public void ClearSpawnedButtons()
    {
        foreach (var button in spawnedButtons)
        {
            if (button != null)
            {
                Destroy(button);
            }
        }
        spawnedButtons.Clear();
    }
    
    /// <summary>
    /// Adds a new course and spawns its button
    /// </summary>
    public void AddCourse(CourseData courseData)
    {
        courses.Add(courseData);
        SpawnCourseButton(courseData, courses.Count - 1);
    }
    
    /// <summary>
    /// Gets the number of courses
    /// </summary>
    public int CourseCount => courses.Count;
    
    /// <summary>
    /// Gets a spawned button by index
    /// </summary>
    public GameObject GetButton(int index)
    {
        if (index >= 0 && index < spawnedButtons.Count)
        {
            return spawnedButtons[index];
        }
        return null;
    }

    /// <summary>
    /// Sets the language index and respawns buttons
    /// </summary>
    public void SetLanguageIndex(int languageIndex)
    {
        currentLanguageIndex = languageIndex;
        currentLanguage = ProgressManager.GetLanguageString(languageIndex);
        // Reinitialize courses for the new language
        InitializeCoursesForLanguage(currentLanguage);
        SpawnCourseButtons();
    }

    /// <summary>
    /// Gets the current language index
    /// </summary>
    public int CurrentLanguageIndex => currentLanguageIndex;
}
