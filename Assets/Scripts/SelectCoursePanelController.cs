using UnityEngine;

/// <summary>
/// Handles course selection panel.
/// Note: Course button onClick handlers are now registered by CourseButtonSpawner when buttons are spawned.
/// This controller only handles panel-level operations like back button.
/// </summary>
public class SelectCoursePanelController : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private GameObject selectLanguagePanel;

    private void Start()
    {
        Debug.Log("[SelectCoursePanelController] Start");

        // Find language panel if not assigned (for back button functionality)
        if (selectLanguagePanel == null)
        {
            selectLanguagePanel = FindObjectByName("SelectLanguagePanel");
            if (selectLanguagePanel != null)
            {
                Debug.Log("[SelectCoursePanelController] Found SelectLanguagePanel");
            }
        }
    }

    /// <summary>
    /// Called when back button is clicked - returns to language selection
    /// </summary>
    public void OnBackButtonClicked()
    {
        Debug.Log("[SelectCoursePanelController] Back button clicked");

        if (selectLanguagePanel == null)
        {
            selectLanguagePanel = FindObjectByName("SelectLanguagePanel");
        }

        if (selectLanguagePanel == null)
        {
            Debug.LogError("[SelectCoursePanelController] SelectLanguagePanel not found!");
            return;
        }

        // Store current position
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        // Position language panel at current position
        selectLanguagePanel.transform.position = currentPosition;
        selectLanguagePanel.transform.rotation = currentRotation;

        // Show language panel
        selectLanguagePanel.SetActive(true);
        Debug.Log($"[SelectCoursePanelController] Showed SelectLanguagePanel at {currentPosition}");

        // Hide this course panel
        gameObject.SetActive(false);
        Debug.Log("[SelectCoursePanelController] Hidden SelectCoursePanel");
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
}
