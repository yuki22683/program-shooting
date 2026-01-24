using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Spawns lesson (exercise) buttons dynamically at runtime in the SelectLessonPanel.
/// Reads exercise data from LessonManager and creates buttons for each exercise.
/// </summary>
public class LessonButtonSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject lessonButtonPrefab;

    [Header("Spawn Container")]
    [SerializeField] private Transform buttonContainer;

    [Header("Panel References")]
    [SerializeField] private GameObject slidePanel;
    [SerializeField] private GameObject selectLessonPanel;

    [Header("Lesson Selection")]
    [SerializeField] private int currentLessonIndex = 0;
    [SerializeField] private int currentLanguageIndex = 0; // 0=Python, 1=JavaScript, etc.

    private List<GameObject> spawnedButtons = new List<GameObject>();
    private LessonManager lessonManager;
    private string currentLanguage = "python";

    /// <summary>
    /// Exercise data structure for button spawning
    /// </summary>
    [System.Serializable]
    public class ExerciseButtonData
    {
        public string exerciseTitleKey;
        public string exerciseDescriptionKey;
        public string difficultyKey;
        public string slideKeyPrefix;
        public int slideCount = 3;
        public bool isCompleted;
        public bool isLocked;
    }

    private void Awake()
    {
        lessonManager = FindObjectOfType<LessonManager>();
        FindReferences();
        FindPanelsIfNeeded();
    }

    /// <summary>
    /// Finds panel references if not assigned in Inspector
    /// </summary>
    private void FindPanelsIfNeeded()
    {
        if (slidePanel == null)
        {
            slidePanel = FindObjectByName("SlidePanel");
            if (slidePanel != null)
            {
                Debug.Log("[LessonButtonSpawner] Found SlidePanel");
            }
        }

        if (selectLessonPanel == null)
        {
            // Try to find parent SelectLessonPanel
            Transform parent = transform;
            while (parent != null)
            {
                if (parent.name == "SelectLessonPanel")
                {
                    selectLessonPanel = parent.gameObject;
                    Debug.Log("[LessonButtonSpawner] Found SelectLessonPanel (parent)");
                    break;
                }
                parent = parent.parent;
            }

            // If still not found, search in scene
            if (selectLessonPanel == null)
            {
                selectLessonPanel = FindObjectByName("SelectLessonPanel");
                if (selectLessonPanel != null)
                {
                    Debug.Log("[LessonButtonSpawner] Found SelectLessonPanel (scene search)");
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

    private void Start()
    {
        SpawnLessonButtons();
    }

    private void OnEnable()
    {
        // Re-spawn buttons when panel becomes active to reflect latest progress
        if (lessonButtonPrefab != null && buttonContainer != null)
        {
            SpawnLessonButtons();
            Debug.Log("[LessonButtonSpawner] OnEnable - re-spawned buttons to reflect progress");
        }
    }

    /// <summary>
    /// Automatically finds prefab and container references if not set
    /// </summary>
    private void FindReferences()
    {
        // Load prefab from Resources if not set
        if (lessonButtonPrefab == null)
        {
            lessonButtonPrefab = Resources.Load<GameObject>("Prefabs/TextTileButton_Lesson Variant");
            if (lessonButtonPrefab != null)
            {
                Debug.Log("[LessonButtonSpawner] Loaded prefab from Resources");
            }
            else
            {
                Debug.LogWarning("[LessonButtonSpawner] Could not load prefab from Resources/Prefabs/TextTileButton_Lesson Variant");
            }
        }

        // Find button container in SelectLessonPanel hierarchy
        // IMPORTANT: There are TWO "Content" objects in Viewport - one inactive with VerticalLayoutGroup,
        // and one active with GridLayoutGroup. We MUST find the one with GridLayoutGroup that is active.
        if (buttonContainer == null)
        {
            // Search all children for the ACTIVE Content with GridLayoutGroup
            Transform[] allTransforms = GetComponentsInChildren<Transform>(true);
            foreach (var t in allTransforms)
            {
                if (t.name == "Content" && t.GetComponent<GridLayoutGroup>() != null)
                {
                    // Check if this is under the Scroll View path and is active
                    if (t.parent != null && t.parent.name == "Viewport" && t.gameObject.activeInHierarchy)
                    {
                        buttonContainer = t;
                        Debug.Log($"[LessonButtonSpawner] Found ACTIVE button container with GridLayoutGroup at: {GetTransformPath(t)}");
                        break;
                    }
                }
            }

            // Fallback: if not found active, just find any Content with GridLayoutGroup
            if (buttonContainer == null)
            {
                foreach (var t in allTransforms)
                {
                    if (t.name == "Content" && t.GetComponent<GridLayoutGroup>() != null)
                    {
                        buttonContainer = t;
                        Debug.Log($"[LessonButtonSpawner] Found button container with GridLayoutGroup (fallback): {GetTransformPath(t)}");
                        break;
                    }
                }
            }

            if (buttonContainer == null)
            {
                Debug.LogError("[LessonButtonSpawner] Could not find button container with GridLayoutGroup!");
            }
        }
    }

    /// <summary>
    /// Helper to get full transform path for debugging
    /// </summary>
    private string GetTransformPath(Transform t)
    {
        string path = t.name;
        Transform parent = t.parent;
        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }
        return path;
    }

    /// <summary>
    /// Sets the lesson index and respawns buttons
    /// </summary>
    public void SetLessonIndex(int lessonIndex)
    {
        currentLessonIndex = lessonIndex;
        SpawnLessonButtons();
    }

    /// <summary>
    /// Sets the language index
    /// </summary>
    public void SetLanguageIndex(int languageIndex)
    {
        currentLanguageIndex = languageIndex;
        currentLanguage = ProgressManager.GetLanguageString(languageIndex);
    }

    /// <summary>
    /// Sets both course (lesson) index and language index, then respawns buttons
    /// </summary>
    public void SetCourseAndLanguage(int courseIndex, int languageIndex)
    {
        currentLessonIndex = courseIndex;
        currentLanguageIndex = languageIndex;
        currentLanguage = ProgressManager.GetLanguageString(languageIndex);
        SpawnLessonButtons();
    }

    /// <summary>
    /// Spawns all lesson buttons based on the current lesson's exercises
    /// </summary>
    public void SpawnLessonButtons()
    {
        if (lessonButtonPrefab == null)
        {
            Debug.LogError("[LessonButtonSpawner] Lesson button prefab is not assigned!");
            return;
        }

        if (buttonContainer == null)
        {
            Debug.LogError("[LessonButtonSpawner] Button container is not assigned!");
            return;
        }

        // Clear existing buttons
        ClearSpawnedButtons();

        // Get exercise data from LessonManager or use defaults
        List<ExerciseButtonData> exercises = GetExercisesForLesson(currentLessonIndex);

        // Spawn buttons for each exercise
        for (int i = 0; i < exercises.Count; i++)
        {
            SpawnLessonButton(exercises[i], i);
        }

        Debug.Log($"[LessonButtonSpawner] Spawned {exercises.Count} lesson buttons for lesson {currentLessonIndex + 1}");
    }

    /// <summary>
    /// Gets exercise data for the specified lesson based on current language
    /// </summary>
    private List<ExerciseButtonData> GetExercisesForLesson(int lessonIndex)
    {
        // Get exercises based on language
        switch (currentLanguage.ToLower())
        {
            case "python":
                return GetPythonExercisesForLesson(lessonIndex);
            case "javascript":
                return GetJavaScriptExercisesForLesson(lessonIndex);
            case "typescript":
                return GetTypeScriptExercisesForLesson(lessonIndex);
            default:
                Debug.LogWarning($"[LessonButtonSpawner] Unknown language: {currentLanguage}, defaulting to Python");
                return GetPythonExercisesForLesson(lessonIndex);
        }
    }

    /// <summary>
    /// Gets Python exercise data for the specified lesson
    /// </summary>
    private List<ExerciseButtonData> GetPythonExercisesForLesson(int lessonIndex)
    {
        switch (lessonIndex)
        {
            case 0: return GetPythonLesson1Exercises();
            case 1: return GetPythonLesson2Exercises();
            case 2: return GetPythonLesson3Exercises();
            case 3: return GetPythonLesson4Exercises();
            case 4: return GetPythonLesson5Exercises();
            default: return GetPythonLesson1Exercises();
        }
    }

    /// <summary>
    /// Gets JavaScript exercise data for the specified lesson
    /// </summary>
    private List<ExerciseButtonData> GetJavaScriptExercisesForLesson(int lessonIndex)
    {
        switch (lessonIndex)
        {
            case 0: return GetJavaScriptLesson1Exercises();
            case 1: return GetJavaScriptLesson2Exercises();
            case 2: return GetJavaScriptLesson3Exercises();
            case 3: return GetJavaScriptLesson4Exercises();
            case 4: return GetJavaScriptLesson5Exercises();
            default: return GetJavaScriptLesson1Exercises();
        }
    }

    /// <summary>
    /// Gets TypeScript exercise data for the specified lesson
    /// </summary>
    private List<ExerciseButtonData> GetTypeScriptExercisesForLesson(int lessonIndex)
    {
        switch (lessonIndex)
        {
            case 0: return GetTypeScriptLesson1Exercises();
            case 1: return GetTypeScriptLesson2Exercises();
            case 2: return GetTypeScriptLesson3Exercises();
            case 3: return GetTypeScriptLesson4Exercises();
            default: return GetTypeScriptLesson1Exercises();
        }
    }

    /// <summary>
    /// Lesson 1 exercises (13 exercises, all easy)
    /// </summary>
    private List<ExerciseButtonData> GetPythonLesson1Exercises()
    {
        return new List<ExerciseButtonData>
        {
            new ExerciseButtonData { exerciseTitleKey = "python_lesson1_ex1_title", exerciseDescriptionKey = "python_lesson1_ex1_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson1_ex2_title", exerciseDescriptionKey = "python_lesson1_ex2_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson1_ex3_title", exerciseDescriptionKey = "python_lesson1_ex3_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson1_ex4_title", exerciseDescriptionKey = "python_lesson1_ex4_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson1_ex5_title", exerciseDescriptionKey = "python_lesson1_ex5_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson1_ex6_title", exerciseDescriptionKey = "python_lesson1_ex6_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson1_ex7_title", exerciseDescriptionKey = "python_lesson1_ex7_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson1_ex8_title", exerciseDescriptionKey = "python_lesson1_ex8_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson1_ex9_title", exerciseDescriptionKey = "python_lesson1_ex9_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson1_ex10_title", exerciseDescriptionKey = "python_lesson1_ex10_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson1_ex11_title", exerciseDescriptionKey = "python_lesson1_ex11_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson1_ex12_title", exerciseDescriptionKey = "python_lesson1_ex12_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson1_ex13_title", exerciseDescriptionKey = "python_lesson1_ex13_description", difficultyKey = "difficulty_easy" }
        };
    }

    /// <summary>
    /// Lesson 2 exercises (12 exercises, all easy)
    /// </summary>
    private List<ExerciseButtonData> GetPythonLesson2Exercises()
    {
        return new List<ExerciseButtonData>
        {
            new ExerciseButtonData { exerciseTitleKey = "python_lesson2_ex1_title", exerciseDescriptionKey = "python_lesson2_ex1_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson2_ex2_title", exerciseDescriptionKey = "python_lesson2_ex2_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson2_ex3_title", exerciseDescriptionKey = "python_lesson2_ex3_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson2_ex4_title", exerciseDescriptionKey = "python_lesson2_ex4_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson2_ex5_title", exerciseDescriptionKey = "python_lesson2_ex5_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson2_ex6_title", exerciseDescriptionKey = "python_lesson2_ex6_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson2_ex7_title", exerciseDescriptionKey = "python_lesson2_ex7_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson2_ex8_title", exerciseDescriptionKey = "python_lesson2_ex8_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson2_ex9_title", exerciseDescriptionKey = "python_lesson2_ex9_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson2_ex10_title", exerciseDescriptionKey = "python_lesson2_ex10_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson2_ex11_title", exerciseDescriptionKey = "python_lesson2_ex11_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson2_ex12_title", exerciseDescriptionKey = "python_lesson2_ex12_description", difficultyKey = "difficulty_medium" }
        };
    }

    /// <summary>
    /// Lesson 3 exercises (10 exercises, medium difficulty)
    /// </summary>
    private List<ExerciseButtonData> GetPythonLesson3Exercises()
    {
        return new List<ExerciseButtonData>
        {
            new ExerciseButtonData { exerciseTitleKey = "python_lesson3_ex1_title", exerciseDescriptionKey = "python_lesson3_ex1_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson3_ex2_title", exerciseDescriptionKey = "python_lesson3_ex2_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson3_ex3_title", exerciseDescriptionKey = "python_lesson3_ex3_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson3_ex4_title", exerciseDescriptionKey = "python_lesson3_ex4_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson3_ex5_title", exerciseDescriptionKey = "python_lesson3_ex5_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson3_ex6_title", exerciseDescriptionKey = "python_lesson3_ex6_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson3_ex7_title", exerciseDescriptionKey = "python_lesson3_ex7_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson3_ex8_title", exerciseDescriptionKey = "python_lesson3_ex8_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson3_ex9_title", exerciseDescriptionKey = "python_lesson3_ex9_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson3_ex10_title", exerciseDescriptionKey = "python_lesson3_ex10_description", difficultyKey = "difficulty_medium" }
        };
    }

    /// <summary>
    /// Lesson 4 exercises (10 exercises, medium difficulty)
    /// </summary>
    private List<ExerciseButtonData> GetPythonLesson4Exercises()
    {
        return new List<ExerciseButtonData>
        {
            new ExerciseButtonData { exerciseTitleKey = "python_lesson4_ex1_title", exerciseDescriptionKey = "python_lesson4_ex1_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson4_ex2_title", exerciseDescriptionKey = "python_lesson4_ex2_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson4_ex3_title", exerciseDescriptionKey = "python_lesson4_ex3_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson4_ex4_title", exerciseDescriptionKey = "python_lesson4_ex4_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson4_ex5_title", exerciseDescriptionKey = "python_lesson4_ex5_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson4_ex6_title", exerciseDescriptionKey = "python_lesson4_ex6_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson4_ex7_title", exerciseDescriptionKey = "python_lesson4_ex7_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson4_ex8_title", exerciseDescriptionKey = "python_lesson4_ex8_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson4_ex9_title", exerciseDescriptionKey = "python_lesson4_ex9_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson4_ex10_title", exerciseDescriptionKey = "python_lesson4_ex10_description", difficultyKey = "difficulty_hard" }
        };
    }

    /// <summary>
    /// Lesson 5 exercises (10 exercises, hard difficulty)
    /// </summary>
    private List<ExerciseButtonData> GetPythonLesson5Exercises()
    {
        return new List<ExerciseButtonData>
        {
            new ExerciseButtonData { exerciseTitleKey = "python_lesson5_ex1_title", exerciseDescriptionKey = "python_lesson5_ex1_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson5_ex2_title", exerciseDescriptionKey = "python_lesson5_ex2_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson5_ex3_title", exerciseDescriptionKey = "python_lesson5_ex3_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson5_ex4_title", exerciseDescriptionKey = "python_lesson5_ex4_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson5_ex5_title", exerciseDescriptionKey = "python_lesson5_ex5_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson5_ex6_title", exerciseDescriptionKey = "python_lesson5_ex6_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson5_ex7_title", exerciseDescriptionKey = "python_lesson5_ex7_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson5_ex8_title", exerciseDescriptionKey = "python_lesson5_ex8_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson5_ex9_title", exerciseDescriptionKey = "python_lesson5_ex9_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "python_lesson5_ex10_title", exerciseDescriptionKey = "python_lesson5_ex10_description", difficultyKey = "difficulty_hard" }
        };
    }

    #region JavaScript Exercises

    /// <summary>
    /// JavaScript Lesson 1 exercises (10 exercises, easy)
    /// </summary>
    private List<ExerciseButtonData> GetJavaScriptLesson1Exercises()
    {
        return new List<ExerciseButtonData>
        {
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson1_ex1_title", exerciseDescriptionKey = "javascript_lesson1_ex1_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson1_ex2_title", exerciseDescriptionKey = "javascript_lesson1_ex2_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson1_ex3_title", exerciseDescriptionKey = "javascript_lesson1_ex3_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson1_ex4_title", exerciseDescriptionKey = "javascript_lesson1_ex4_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson1_ex5_title", exerciseDescriptionKey = "javascript_lesson1_ex5_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson1_ex6_title", exerciseDescriptionKey = "javascript_lesson1_ex6_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson1_ex7_title", exerciseDescriptionKey = "javascript_lesson1_ex7_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson1_ex8_title", exerciseDescriptionKey = "javascript_lesson1_ex8_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson1_ex9_title", exerciseDescriptionKey = "javascript_lesson1_ex9_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson1_ex10_title", exerciseDescriptionKey = "javascript_lesson1_ex10_description", difficultyKey = "difficulty_easy" }
        };
    }

    /// <summary>
    /// JavaScript Lesson 2 exercises (13 exercises, medium)
    /// </summary>
    private List<ExerciseButtonData> GetJavaScriptLesson2Exercises()
    {
        return new List<ExerciseButtonData>
        {
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson2_ex1_title", exerciseDescriptionKey = "javascript_lesson2_ex1_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson2_ex2_title", exerciseDescriptionKey = "javascript_lesson2_ex2_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson2_ex3_title", exerciseDescriptionKey = "javascript_lesson2_ex3_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson2_ex4_title", exerciseDescriptionKey = "javascript_lesson2_ex4_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson2_ex5_title", exerciseDescriptionKey = "javascript_lesson2_ex5_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson2_ex6_title", exerciseDescriptionKey = "javascript_lesson2_ex6_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson2_ex7_title", exerciseDescriptionKey = "javascript_lesson2_ex7_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson2_ex8_title", exerciseDescriptionKey = "javascript_lesson2_ex8_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson2_ex9_title", exerciseDescriptionKey = "javascript_lesson2_ex9_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson2_ex10_title", exerciseDescriptionKey = "javascript_lesson2_ex10_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson2_ex11_title", exerciseDescriptionKey = "javascript_lesson2_ex11_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson2_ex12_title", exerciseDescriptionKey = "javascript_lesson2_ex12_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson2_ex13_title", exerciseDescriptionKey = "javascript_lesson2_ex13_description", difficultyKey = "difficulty_medium" }
        };
    }

    /// <summary>
    /// JavaScript Lesson 3 exercises (10 exercises, hard)
    /// </summary>
    private List<ExerciseButtonData> GetJavaScriptLesson3Exercises()
    {
        return new List<ExerciseButtonData>
        {
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson3_ex1_title", exerciseDescriptionKey = "javascript_lesson3_ex1_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson3_ex2_title", exerciseDescriptionKey = "javascript_lesson3_ex2_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson3_ex3_title", exerciseDescriptionKey = "javascript_lesson3_ex3_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson3_ex4_title", exerciseDescriptionKey = "javascript_lesson3_ex4_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson3_ex5_title", exerciseDescriptionKey = "javascript_lesson3_ex5_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson3_ex6_title", exerciseDescriptionKey = "javascript_lesson3_ex6_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson3_ex7_title", exerciseDescriptionKey = "javascript_lesson3_ex7_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson3_ex8_title", exerciseDescriptionKey = "javascript_lesson3_ex8_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson3_ex9_title", exerciseDescriptionKey = "javascript_lesson3_ex9_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson3_ex10_title", exerciseDescriptionKey = "javascript_lesson3_ex10_description", difficultyKey = "difficulty_hard" }
        };
    }

    /// <summary>
    /// JavaScript Lesson 4 exercises (10 exercises, hard)
    /// </summary>
    private List<ExerciseButtonData> GetJavaScriptLesson4Exercises()
    {
        return new List<ExerciseButtonData>
        {
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson4_ex1_title", exerciseDescriptionKey = "javascript_lesson4_ex1_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson4_ex2_title", exerciseDescriptionKey = "javascript_lesson4_ex2_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson4_ex3_title", exerciseDescriptionKey = "javascript_lesson4_ex3_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson4_ex4_title", exerciseDescriptionKey = "javascript_lesson4_ex4_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson4_ex5_title", exerciseDescriptionKey = "javascript_lesson4_ex5_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson4_ex6_title", exerciseDescriptionKey = "javascript_lesson4_ex6_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson4_ex7_title", exerciseDescriptionKey = "javascript_lesson4_ex7_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson4_ex8_title", exerciseDescriptionKey = "javascript_lesson4_ex8_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson4_ex9_title", exerciseDescriptionKey = "javascript_lesson4_ex9_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson4_ex10_title", exerciseDescriptionKey = "javascript_lesson4_ex10_description", difficultyKey = "difficulty_hard" }
        };
    }

    /// <summary>
    /// JavaScript Lesson 5 exercises (10 exercises, hard)
    /// </summary>
    private List<ExerciseButtonData> GetJavaScriptLesson5Exercises()
    {
        return new List<ExerciseButtonData>
        {
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson5_ex1_title", exerciseDescriptionKey = "javascript_lesson5_ex1_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson5_ex2_title", exerciseDescriptionKey = "javascript_lesson5_ex2_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson5_ex3_title", exerciseDescriptionKey = "javascript_lesson5_ex3_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson5_ex4_title", exerciseDescriptionKey = "javascript_lesson5_ex4_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson5_ex5_title", exerciseDescriptionKey = "javascript_lesson5_ex5_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson5_ex6_title", exerciseDescriptionKey = "javascript_lesson5_ex6_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson5_ex7_title", exerciseDescriptionKey = "javascript_lesson5_ex7_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson5_ex8_title", exerciseDescriptionKey = "javascript_lesson5_ex8_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson5_ex9_title", exerciseDescriptionKey = "javascript_lesson5_ex9_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "javascript_lesson5_ex10_title", exerciseDescriptionKey = "javascript_lesson5_ex10_description", difficultyKey = "difficulty_hard" }
        };
    }

    #endregion


    #region TypeScript Exercises

    /// <summary>
    /// TypeScript Lesson 1 exercises (13 exercises, easy)
    /// </summary>
    private List<ExerciseButtonData> GetTypeScriptLesson1Exercises()
    {
        return new List<ExerciseButtonData>
        {
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex1_title", exerciseDescriptionKey = "typescript_lesson1_ex1_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex2_title", exerciseDescriptionKey = "typescript_lesson1_ex2_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex3_title", exerciseDescriptionKey = "typescript_lesson1_ex3_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex4_title", exerciseDescriptionKey = "typescript_lesson1_ex4_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex5_title", exerciseDescriptionKey = "typescript_lesson1_ex5_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex6_title", exerciseDescriptionKey = "typescript_lesson1_ex6_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex7_title", exerciseDescriptionKey = "typescript_lesson1_ex7_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex8_title", exerciseDescriptionKey = "typescript_lesson1_ex8_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex9_title", exerciseDescriptionKey = "typescript_lesson1_ex9_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex10_title", exerciseDescriptionKey = "typescript_lesson1_ex10_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex11_title", exerciseDescriptionKey = "typescript_lesson1_ex11_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex12_title", exerciseDescriptionKey = "typescript_lesson1_ex12_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex13_title", exerciseDescriptionKey = "typescript_lesson1_ex13_description", difficultyKey = "difficulty_easy" }
        };
    }

    /// <summary>
    /// TypeScript Lesson 2 exercises (10 exercises, medium)
    /// </summary>
    private List<ExerciseButtonData> GetTypeScriptLesson2Exercises()
    {
        return new List<ExerciseButtonData>
        {
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex1_title", exerciseDescriptionKey = "typescript_lesson2_ex1_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex2_title", exerciseDescriptionKey = "typescript_lesson2_ex2_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex3_title", exerciseDescriptionKey = "typescript_lesson2_ex3_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex4_title", exerciseDescriptionKey = "typescript_lesson2_ex4_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex5_title", exerciseDescriptionKey = "typescript_lesson2_ex5_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex6_title", exerciseDescriptionKey = "typescript_lesson2_ex6_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex7_title", exerciseDescriptionKey = "typescript_lesson2_ex7_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex8_title", exerciseDescriptionKey = "typescript_lesson2_ex8_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex9_title", exerciseDescriptionKey = "typescript_lesson2_ex9_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex10_title", exerciseDescriptionKey = "typescript_lesson2_ex10_description", difficultyKey = "difficulty_medium" }
        };
    }

    /// <summary>
    /// TypeScript Lesson 3 exercises (10 exercises, hard)
    /// </summary>
    private List<ExerciseButtonData> GetTypeScriptLesson3Exercises()
    {
        return new List<ExerciseButtonData>
        {
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex1_title", exerciseDescriptionKey = "typescript_lesson3_ex1_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex2_title", exerciseDescriptionKey = "typescript_lesson3_ex2_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex3_title", exerciseDescriptionKey = "typescript_lesson3_ex3_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex4_title", exerciseDescriptionKey = "typescript_lesson3_ex4_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex5_title", exerciseDescriptionKey = "typescript_lesson3_ex5_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex6_title", exerciseDescriptionKey = "typescript_lesson3_ex6_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex7_title", exerciseDescriptionKey = "typescript_lesson3_ex7_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex8_title", exerciseDescriptionKey = "typescript_lesson3_ex8_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex9_title", exerciseDescriptionKey = "typescript_lesson3_ex9_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex10_title", exerciseDescriptionKey = "typescript_lesson3_ex10_description", difficultyKey = "difficulty_hard" }
        };
    }

    /// <summary>
    /// TypeScript Lesson 4 exercises (10 exercises, hard)
    /// </summary>
    private List<ExerciseButtonData> GetTypeScriptLesson4Exercises()
    {
        return new List<ExerciseButtonData>
        {
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex1_title", exerciseDescriptionKey = "typescript_lesson4_ex1_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex2_title", exerciseDescriptionKey = "typescript_lesson4_ex2_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex3_title", exerciseDescriptionKey = "typescript_lesson4_ex3_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex4_title", exerciseDescriptionKey = "typescript_lesson4_ex4_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex5_title", exerciseDescriptionKey = "typescript_lesson4_ex5_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex6_title", exerciseDescriptionKey = "typescript_lesson4_ex6_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex7_title", exerciseDescriptionKey = "typescript_lesson4_ex7_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex8_title", exerciseDescriptionKey = "typescript_lesson4_ex8_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex9_title", exerciseDescriptionKey = "typescript_lesson4_ex9_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex10_title", exerciseDescriptionKey = "typescript_lesson4_ex10_description", difficultyKey = "difficulty_hard" }
        };
    }

    #endregion

    /// <summary>
    /// Spawns a single lesson button
    /// </summary>
    private void SpawnLessonButton(ExerciseButtonData exerciseData, int index)
    {
        GameObject buttonObj = Instantiate(lessonButtonPrefab, buttonContainer);
        buttonObj.name = $"TextTileButton_Lesson{index + 1}";

        // Check progress for this lesson
        bool isCompleted = false;
        bool isLocked = false;

        if (ProgressManager.Instance != null)
        {
            isCompleted = ProgressManager.Instance.IsLessonCompleted(currentLanguage, currentLessonIndex, index);
            isLocked = !ProgressManager.Instance.IsLessonUnlocked(currentLanguage, currentLessonIndex, index);
        }

        // Configure SelectLessonPanelManager component
        SelectLessonPanelManager panelManager = buttonObj.GetComponent<SelectLessonPanelManager>();
        if (panelManager == null)
        {
            Debug.Log($"[LessonButtonSpawner] Adding SelectLessonPanelManager to {buttonObj.name}");
            panelManager = buttonObj.AddComponent<SelectLessonPanelManager>();
        }
        else
        {
            Debug.Log($"[LessonButtonSpawner] Found existing SelectLessonPanelManager on {buttonObj.name}");
        }

        if (panelManager == null)
        {
            Debug.LogError($"[LessonButtonSpawner] Failed to get/add SelectLessonPanelManager on {buttonObj.name}!");
            spawnedButtons.Add(buttonObj);
            return;
        }

        // Set indices
        panelManager.LessonIndex = currentLessonIndex;
        panelManager.ExerciseIndex = index;

        Debug.Log($"[LessonButtonSpawner] Configuring {buttonObj.name} with title={exerciseData.exerciseTitleKey}, completed={isCompleted}, locked={isLocked}");

        // Configure panel with exercise data and progress state
        panelManager.Configure(
            exerciseData.exerciseTitleKey,
            exerciseData.exerciseDescriptionKey,
            exerciseData.difficultyKey,
            isCompleted,
            isLocked
        );

        // Disable Animator and ButtonLabelHover if locked
        if (isLocked)
        {
            Animator animator = buttonObj.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = false;
                Debug.Log($"[LessonButtonSpawner] Disabled Animator for locked exercise {index}");
            }

            ButtonLabelHover buttonLabelHover = buttonObj.GetComponent<ButtonLabelHover>();
            if (buttonLabelHover != null)
            {
                buttonLabelHover.enabled = false;
                Debug.Log($"[LessonButtonSpawner] Disabled ButtonLabelHover for locked exercise {index}");
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

            int capturedLessonIndex = currentLessonIndex;
            int capturedExerciseIndex = index;

            button.onClick.AddListener(() => OnLessonButtonClicked(capturedLessonIndex, capturedExerciseIndex));
            Debug.Log($"[LessonButtonSpawner] Registered onClick for lesson {capturedLessonIndex}, exercise {capturedExerciseIndex} (locked={isLocked})");
        }
        else
        {
            Debug.LogWarning($"[LessonButtonSpawner] No Button component found on {buttonObj.name}");
        }

        spawnedButtons.Add(buttonObj);
        Debug.Log($"[LessonButtonSpawner] Spawned lesson button: {buttonObj.name}");
    }

    /// <summary>
    /// Called when a lesson button is clicked
    /// </summary>
    private void OnLessonButtonClicked(int lessonIndex, int exerciseIndex)
    {
        Debug.Log($"[LessonButtonSpawner] Lesson button clicked: lesson {lessonIndex}, exercise {exerciseIndex}");

        // Check if lesson is locked
        if (ProgressManager.Instance != null && !ProgressManager.Instance.IsLessonUnlocked(currentLanguage, lessonIndex, exerciseIndex))
        {
            Debug.Log($"[LessonButtonSpawner] Lesson {lessonIndex} exercise {exerciseIndex} is locked, ignoring click");
            return;
        }

        // Find panels if not cached
        if (slidePanel == null)
        {
            slidePanel = FindObjectByName("SlidePanel");
        }
        if (selectLessonPanel == null)
        {
            selectLessonPanel = FindObjectByName("SelectLessonPanel");
        }

        if (slidePanel == null)
        {
            Debug.LogError("[LessonButtonSpawner] SlidePanel not found!");
            return;
        }

        // Store current position of lesson panel
        Vector3 currentPosition = Vector3.zero;
        Quaternion currentRotation = Quaternion.identity;

        if (selectLessonPanel != null)
        {
            currentPosition = selectLessonPanel.transform.position;
            currentRotation = selectLessonPanel.transform.rotation;
        }

        // Set LessonManager to this lesson/exercise and get exercise data
        string titleKey = "";
        string slideKeyPrefix = "";
        int slideCount = 3;

        if (lessonManager != null)
        {
            // Use SetLanguageAndLesson to ensure correct language exercises are loaded
            lessonManager.SetLanguageAndLesson(currentLanguage, lessonIndex, exerciseIndex);
            Debug.Log($"[LessonButtonSpawner] Set LessonManager to language {currentLanguage}, lesson {lessonIndex}, exercise {exerciseIndex}");

            // Get slide data from the current exercise
            Exercise exercise = lessonManager.GetCurrentExercise();
            if (exercise != null)
            {
                titleKey = exercise.titleKey;
                slideKeyPrefix = exercise.slideKeyPrefix;
                slideCount = exercise.slideCount;
                Debug.Log($"[LessonButtonSpawner] Got exercise data: title={titleKey}, prefix={slideKeyPrefix}, count={slideCount}");
            }
        }

        // Position slide panel at current lesson panel position
        slidePanel.transform.position = currentPosition;
        slidePanel.transform.rotation = currentRotation;

        // Show slide panel FIRST (so Awake/Start runs and UI references are found)
        slidePanel.SetActive(true);
        Debug.Log($"[LessonButtonSpawner] Showed SlidePanel at {currentPosition}");

        // Configure SlideManager with slide data AFTER panel is active
        SlideManager slideManager = slidePanel.GetComponent<SlideManager>();
        if (slideManager == null)
        {
            slideManager = SlideManager.Instance;
        }

        if (slideManager != null && !string.IsNullOrEmpty(slideKeyPrefix))
        {
            slideManager.SetSlideConfig(titleKey, slideKeyPrefix, slideCount);
            Debug.Log($"[LessonButtonSpawner] Configured SlideManager: title={titleKey}, prefix={slideKeyPrefix}, count={slideCount}");
        }
        else
        {
            Debug.LogWarning("[LessonButtonSpawner] SlideManager not found or no slide data!");
        }

        // Hide lesson panel
        if (selectLessonPanel != null)
        {
            selectLessonPanel.SetActive(false);
            Debug.Log("[LessonButtonSpawner] Hidden SelectLessonPanel");
        }
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
    /// Gets the number of exercises
    /// </summary>
    public int ExerciseCount => spawnedButtons.Count;

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
}
