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
    
    [Header("Course Data")]
    [SerializeField] private List<CourseData> courses = new List<CourseData>();
    
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
    }

    private void Start()
    {
        SpawnCourseButtons();
    }

    /// <summary>
    /// Initializes default course data for Python lessons
    /// </summary>
    private void InitializeDefaultCourses()
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
            totalExercises = 10,
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

        Debug.Log("[CourseButtonSpawner] Initialized 5 default courses");
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
        
        // Configure SelectCoursePanelManager component
        SelectCoursePanelManager panelManager = buttonObj.GetComponent<SelectCoursePanelManager>();
        if (panelManager == null)
        {
            panelManager = buttonObj.AddComponent<SelectCoursePanelManager>();
        }
        
        // Use reflection or serialized fields to set the course data
        // For now, we'll use a public method if available
        ConfigurePanelManager(panelManager, courseData);
        
        spawnedButtons.Add(buttonObj);
        Debug.Log($"[CourseButtonSpawner] Spawned course button: {buttonObj.name}");
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
}
