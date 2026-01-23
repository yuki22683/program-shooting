using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles lesson selection panel navigation.
/// Back button returns to course selection panel.
/// </summary>
public class SelectLessonPanelController : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private GameObject selectCoursePanel;
    [SerializeField] private Button backButton;

    private void Awake()
    {
        FindPanelsIfNeeded();
        SetupBackButton();
    }

    private void Start()
    {
        Debug.Log("[SelectLessonPanelController] Start");
    }

    /// <summary>
    /// Finds panel references if not assigned
    /// </summary>
    private void FindPanelsIfNeeded()
    {
        if (selectCoursePanel == null)
        {
            selectCoursePanel = FindObjectByName("SelectCoursePanel");
            if (selectCoursePanel != null)
            {
                Debug.Log("[SelectLessonPanelController] Found SelectCoursePanel");
            }
        }
    }

    /// <summary>
    /// Sets up the back button click handler
    /// </summary>
    private void SetupBackButton()
    {
        if (backButton == null)
        {
            // Path: PanelInteractable/PanelCanvas/TopBar/Horizontal/Horizontal/PrimaryButton_IconAndLabel_UnityUIButton (2)
            Transform buttonTransform = transform.Find("PanelInteractable/PanelCanvas/TopBar/Horizontal/Horizontal/PrimaryButton_IconAndLabel_UnityUIButton (2)");
            if (buttonTransform != null)
            {
                backButton = buttonTransform.GetComponent<Button>();
                Debug.Log("[SelectLessonPanelController] Found back button");
            }
            else
            {
                Debug.LogWarning("[SelectLessonPanelController] Back button not found at expected path");
            }
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
            Debug.Log("[SelectLessonPanelController] Added back button click handler");
        }
    }

    /// <summary>
    /// Called when back button is clicked - returns to course selection
    /// </summary>
    public void OnBackButtonClicked()
    {
        Debug.Log("[SelectLessonPanelController] Back button clicked");

        if (selectCoursePanel == null)
        {
            selectCoursePanel = FindObjectByName("SelectCoursePanel");
        }

        if (selectCoursePanel == null)
        {
            Debug.LogError("[SelectLessonPanelController] SelectCoursePanel not found!");
            return;
        }

        // Store current position
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        // Position course panel at current position
        selectCoursePanel.transform.position = currentPosition;
        selectCoursePanel.transform.rotation = currentRotation;

        // Skip automatic repositioning in PositionInFrontOfHeadset
        PositionInFrontOfHeadset positioner = selectCoursePanel.GetComponent<PositionInFrontOfHeadset>();
        if (positioner != null)
        {
            positioner.SkipNextReposition();
        }

        // Show course panel
        selectCoursePanel.SetActive(true);
        Debug.Log($"[SelectLessonPanelController] Showed SelectCoursePanel at {currentPosition}");

        // Hide this lesson panel
        gameObject.SetActive(false);
        Debug.Log("[SelectLessonPanelController] Hidden SelectLessonPanel");
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
