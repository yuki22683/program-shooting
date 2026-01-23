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

    [Header("Lesson Selection")]
    [SerializeField] private int currentLessonIndex = 0;

    private List<GameObject> spawnedButtons = new List<GameObject>();
    private LessonManager lessonManager;

    /// <summary>
    /// Exercise data structure for button spawning
    /// </summary>
    [System.Serializable]
    public class ExerciseButtonData
    {
        public string exerciseTitleKey;
        public string exerciseDescriptionKey;
        public string difficultyKey;
        public bool isCompleted;
        public bool isLocked;
    }

    private void Awake()
    {
        lessonManager = FindObjectOfType<LessonManager>();
        FindReferences();
    }

    private void Start()
    {
        SpawnLessonButtons();
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
    /// Gets exercise data for the specified lesson
    /// </summary>
    private List<ExerciseButtonData> GetExercisesForLesson(int lessonIndex)
    {
        List<ExerciseButtonData> exercises = new List<ExerciseButtonData>();

        // Define exercises based on lesson index
        // Each lesson has different number of exercises with different difficulties
        switch (lessonIndex)
        {
            case 0: // Lesson 1 - Python I
                exercises = GetLesson1Exercises();
                break;
            case 1: // Lesson 2 - Python II
                exercises = GetLesson2Exercises();
                break;
            case 2: // Lesson 3 - Python III
                exercises = GetLesson3Exercises();
                break;
            case 3: // Lesson 4 - Python IV
                exercises = GetLesson4Exercises();
                break;
            case 4: // Lesson 5 - Python V
                exercises = GetLesson5Exercises();
                break;
            default:
                exercises = GetLesson1Exercises();
                break;
        }

        return exercises;
    }

    /// <summary>
    /// Lesson 1 exercises (13 exercises, all easy)
    /// </summary>
    private List<ExerciseButtonData> GetLesson1Exercises()
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
    private List<ExerciseButtonData> GetLesson2Exercises()
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
    private List<ExerciseButtonData> GetLesson3Exercises()
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
    private List<ExerciseButtonData> GetLesson4Exercises()
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
    private List<ExerciseButtonData> GetLesson5Exercises()
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

    /// <summary>
    /// Spawns a single lesson button
    /// </summary>
    private void SpawnLessonButton(ExerciseButtonData exerciseData, int index)
    {
        GameObject buttonObj = Instantiate(lessonButtonPrefab, buttonContainer);
        buttonObj.name = $"TextTileButton_Lesson{index + 1}";

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

        Debug.Log($"[LessonButtonSpawner] Configuring {buttonObj.name} with title={exerciseData.exerciseTitleKey}, desc={exerciseData.exerciseDescriptionKey}");

        // Configure panel with exercise data
        panelManager.Configure(
            exerciseData.exerciseTitleKey,
            exerciseData.exerciseDescriptionKey,
            exerciseData.difficultyKey,
            exerciseData.isCompleted,
            exerciseData.isLocked
        );

        spawnedButtons.Add(buttonObj);
        Debug.Log($"[LessonButtonSpawner] Spawned lesson button: {buttonObj.name}");
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
