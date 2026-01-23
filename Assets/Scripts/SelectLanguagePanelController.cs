using UnityEngine;

/// <summary>
/// Attach this script directly to SelectLanguagePanel.
/// Handles positioning and language button click events.
/// </summary>
public class SelectLanguagePanelController : MonoBehaviour
{
    [Header("Course Panel")]
    [SerializeField] private GameObject selectCoursePanel;

    private void Awake()
    {
        Debug.Log("[SelectLanguagePanelController] Awake");
    }

    private void Start()
    {
        Debug.Log("[SelectLanguagePanelController] Start");
        // Find course panel if not assigned
        if (selectCoursePanel == null)
        {
            selectCoursePanel = FindObjectByName("SelectCoursePanel");
            if (selectCoursePanel != null)
            {
                Debug.Log($"[SelectLanguagePanelController] Found SelectCoursePanel");
            }
        }
    }

    private void OnEnable()
    {
        Debug.Log("[SelectLanguagePanelController] OnEnable");
    }

    #region Language Button Click Handlers (Register in Button.onClick via Inspector)

    public void OnPythonButtonClicked() => OnLanguageButtonClicked("python");
    public void OnJavaScriptButtonClicked() => OnLanguageButtonClicked("javascript");
    public void OnTypeScriptButtonClicked() => OnLanguageButtonClicked("typescript");
    public void OnJavaButtonClicked() => OnLanguageButtonClicked("java");
    public void OnCButtonClicked() => OnLanguageButtonClicked("c");
    public void OnCppButtonClicked() => OnLanguageButtonClicked("cpp");
    public void OnCSharpButtonClicked() => OnLanguageButtonClicked("csharp");
    public void OnGoButtonClicked() => OnLanguageButtonClicked("go");
    public void OnRustButtonClicked() => OnLanguageButtonClicked("rust");
    public void OnRubyButtonClicked() => OnLanguageButtonClicked("ruby");
    public void OnPHPButtonClicked() => OnLanguageButtonClicked("php");
    public void OnBashButtonClicked() => OnLanguageButtonClicked("bash");
    public void OnHaskellButtonClicked() => OnLanguageButtonClicked("haskell");
    public void OnSwiftButtonClicked() => OnLanguageButtonClicked("swift");
    public void OnKotlinButtonClicked() => OnLanguageButtonClicked("kotlin");
    public void OnSQLButtonClicked() => OnLanguageButtonClicked("sql");
    public void OnLuaButtonClicked() => OnLanguageButtonClicked("lua");
    public void OnPerlButtonClicked() => OnLanguageButtonClicked("perl");
    public void OnElixirButtonClicked() => OnLanguageButtonClicked("elixir");
    public void OnAssemblyButtonClicked() => OnLanguageButtonClicked("assembly");

    #endregion

    /// <summary>
    /// Called when any language button is clicked
    /// </summary>
    public void OnLanguageButtonClicked(string languageId)
    {
        Debug.Log($"[SelectLanguagePanelController] Language selected: {languageId}");

        // Find course panel if not yet found
        if (selectCoursePanel == null)
        {
            selectCoursePanel = FindObjectByName("SelectCoursePanel");
        }

        if (selectCoursePanel == null)
        {
            Debug.LogError("[SelectLanguagePanelController] SelectCoursePanel not found! Cannot show course selection.");
            return;
        }

        // Store position before hiding
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        Debug.Log($"[SelectLanguagePanelController] Storing position: {pos}, rotation: {rot.eulerAngles}");

        // Hide this panel (SelectLanguagePanel)
        gameObject.SetActive(false);
        Debug.Log("[SelectLanguagePanelController] Hidden SelectLanguagePanel");

        // Show course panel at same position
        selectCoursePanel.transform.position = pos;
        selectCoursePanel.transform.rotation = rot;
        selectCoursePanel.SetActive(true);
        Debug.Log($"[SelectLanguagePanelController] Showed SelectCoursePanel at position: {pos}");
    }

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
