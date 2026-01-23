using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Auto-initializes language panel on game start.
/// Uses RuntimeInitializeOnLoadMethod to run without being attached to any GameObject.
/// </summary>
public static class LanguagePanelInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        Debug.Log("[LanguagePanelInitializer] === INITIALIZING ===");

        // Find SelectLanguagePanel (言語選択パネル)
        GameObject languagePanel = FindObjectByName("SelectLanguagePanel");
        if (languagePanel == null)
        {
            Debug.LogError("[LanguagePanelInitializer] SelectLanguagePanel not found!");
            return;
        }
        Debug.Log($"[LanguagePanelInitializer] Found SelectLanguagePanel: {languagePanel.name}");

        // Remove or disable PersistentTransform to prevent it from overriding position
        PersistentTransform persistentTransform = languagePanel.GetComponent<PersistentTransform>();
        if (persistentTransform != null)
        {
            // Delete saved position data from PlayerPrefs
            string positionKey = $"SavedPosition_{languagePanel.name}";
            string rotationKey = $"SavedRotation_{languagePanel.name}";
            if (PlayerPrefs.HasKey(positionKey))
            {
                PlayerPrefs.DeleteKey(positionKey);
                Debug.Log($"[LanguagePanelInitializer] Deleted saved position for {languagePanel.name}");
            }
            if (PlayerPrefs.HasKey(rotationKey))
            {
                PlayerPrefs.DeleteKey(rotationKey);
                Debug.Log($"[LanguagePanelInitializer] Deleted saved rotation for {languagePanel.name}");
            }
            PlayerPrefs.Save();

            // Disable the PersistentTransform component so it doesn't interfere
            Object.Destroy(persistentTransform);
            Debug.Log("[LanguagePanelInitializer] Removed PersistentTransform from SelectLanguagePanel");
        }

        // Find SelectCoursePanel and ensure it's hidden initially
        GameObject coursePanel = FindObjectByName("SelectCoursePanel");
        if (coursePanel != null)
        {
            coursePanel.SetActive(false);
            Debug.Log($"[LanguagePanelInitializer] Found and hid SelectCoursePanel: {coursePanel.name}");
        }

        // Add PositionInFrontOfHeadset component to position panel 1m in front of headset
        PositionInFrontOfHeadset positioner = languagePanel.GetComponent<PositionInFrontOfHeadset>();
        if (positioner == null)
        {
            positioner = languagePanel.AddComponent<PositionInFrontOfHeadset>();
            Debug.Log("[LanguagePanelInitializer] Added PositionInFrontOfHeadset component");
        }
        // Always configure to ensure correct settings
        positioner.Configure(1.0f, 0f, true, true); // 1m distance, no height offset, face headset, horizontal only
        Debug.Log("[LanguagePanelInitializer] Configured PositionInFrontOfHeadset (1.0m)");

        // Add controller if not present
        SelectLanguagePanelController controller = languagePanel.GetComponent<SelectLanguagePanelController>();
        if (controller == null)
        {
            controller = languagePanel.AddComponent<SelectLanguagePanelController>();
            Debug.Log("[LanguagePanelInitializer] Added SelectLanguagePanelController");
        }

        // Activate the panel - this will trigger OnEnable which calls PositionPanel
        Debug.Log($"[LanguagePanelInitializer] Activating SelectLanguagePanel (currently active: {languagePanel.activeSelf})");
        languagePanel.SetActive(true);
        Debug.Log($"[LanguagePanelInitializer] SelectLanguagePanel activated, position: {languagePanel.transform.position}");
    }

    private static GameObject FindObjectByName(string name)
    {
        // Search through all root objects including inactive ones
        foreach (GameObject root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            GameObject found = FindInChildren(root.transform, name);
            if (found != null) return found;
        }
        return null;
    }

    private static GameObject FindInChildren(Transform parent, string name)
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
