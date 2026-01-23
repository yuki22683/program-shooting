using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class LanguageButtonSetup : EditorWindow
{
    // senkou-codeのホーム画面と同じ20言語
    private static readonly (string id, string name, Color bgColor)[] Languages = new (string id, string name, Color bgColor)[]
    {
        ("python", "Python", new Color(0.878f, 0.949f, 1f)),           // sky-50
        ("javascript", "JavaScript", new Color(0.996f, 0.988f, 0.878f)), // yellow-50
        ("typescript", "TypeScript", new Color(0.937f, 0.965f, 1f)),    // blue-50
        ("java", "Java", new Color(0.996f, 0.949f, 0.949f)),           // red-50
        ("c", "C", new Color(0.973f, 0.980f, 0.988f)),                 // slate-50
        ("cpp", "C++", new Color(0.933f, 0.949f, 1f)),                 // indigo-50
        ("csharp", "C#", new Color(0.961f, 0.953f, 1f)),              // violet-50
        ("go", "Go", new Color(0.925f, 0.996f, 1f)),                   // cyan-50
        ("rust", "Rust", new Color(1f, 0.969f, 0.933f)),              // orange-50
        ("ruby", "Ruby", new Color(1f, 0.945f, 0.949f)),              // rose-50
        ("php", "PHP", new Color(0.933f, 0.949f, 1f)),                // indigo-50
        ("bash", "Bash", new Color(0.961f, 0.961f, 0.961f)),          // neutral-100
        ("haskell", "Haskell", new Color(0.980f, 0.961f, 1f)),        // purple-50
        ("elixir", "Elixir", new Color(0.996f, 0.953f, 1f)),          // fuchsia-50
        ("lua", "Lua", new Color(0.937f, 0.965f, 1f)),                // blue-50
        ("assembly", "Assembly", new Color(0.945f, 0.945f, 0.953f)),  // zinc-100
        ("sql", "SQL", new Color(0.973f, 0.980f, 0.988f)),            // slate-50
        ("kotlin", "Kotlin", new Color(0.980f, 0.961f, 1f)),          // purple-50
        ("swift", "Swift", new Color(1f, 0.988f, 0.933f)),            // amber-50
        ("perl", "Perl", new Color(0.941f, 0.992f, 0.984f)),          // teal-50
    };

    [MenuItem("Tools/Create Language Buttons")]
    public static void CreateLanguageButtons()
    {
        // SelectLesson配下のContentを検索
        var selectLesson = GameObject.Find("SelectLesson");
        if (selectLesson == null)
        {
            Debug.LogError("SelectLesson not found in scene!");
            return;
        }

        // Contentオブジェクトを検索
        var content = selectLesson.transform.Find("Scroll View/Viewport/Content");
        if (content == null)
        {
            Debug.LogError("Content not found under SelectLesson/Scroll View/Viewport/Content!");
            return;
        }

        // 既存のTextTileButton_IconAndLabel_Regularを検索
        Transform templateButton = null;
        foreach (Transform child in content)
        {
            if (child.name.Contains("TextTileButton"))
            {
                templateButton = child;
                break;
            }
        }

        if (templateButton == null)
        {
            Debug.LogError("TextTileButton template not found!");
            return;
        }

        // 既存のボタンを削除（テンプレート以外）
        var toDelete = new System.Collections.Generic.List<GameObject>();
        foreach (Transform child in content)
        {
            if (child != templateButton && child.name.Contains("TextTileButton"))
            {
                toDelete.Add(child.gameObject);
            }
        }
        foreach (var obj in toDelete)
        {
            Undo.DestroyObjectImmediate(obj);
        }

        // 各言語のボタンを作成
        for (int i = 0; i < Languages.Length; i++)
        {
            var lang = Languages[i];

            GameObject newButton;
            if (i == 0)
            {
                // 最初の言語はテンプレートを使用
                newButton = templateButton.gameObject;
            }
            else
            {
                // 複製して作成
                newButton = (GameObject)PrefabUtility.InstantiatePrefab(
                    PrefabUtility.GetCorrespondingObjectFromSource(templateButton.gameObject),
                    content
                );

                if (newButton == null)
                {
                    // PrefabでなければInstantiate
                    newButton = Object.Instantiate(templateButton.gameObject, content);
                }

                Undo.RegisterCreatedObjectUndo(newButton, "Create Language Button");
            }

            // ボタンの設定
            SetupButton(newButton.transform, lang.id, lang.name, lang.bgColor);
            newButton.transform.SetSiblingIndex(i);
        }

        Debug.Log($"Created {Languages.Length} language buttons");

        // シーンを保存するためにdirtyマーク
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
    }

    private static void SetupButton(Transform btn, string id, string name, Color bgColor)
    {
        // GameObjectの名前を変更
        btn.name = $"TextTileButton_{id}";
        EditorUtility.SetDirty(btn);

        // ラベルを設定（TMP_Textコンポーネントを探す）
        var texts = btn.GetComponentsInChildren<TMP_Text>(true);
        foreach (var text in texts)
        {
            if (text.gameObject.name.Contains("Label") || text.gameObject.name.Contains("Text"))
            {
                text.text = name;
                EditorUtility.SetDirty(text);
                break;
            }
        }

        // アイコンを設定（あれば）
        string iconPath = $"Assets/Materials/{id}_icon.png";
        var iconSprite = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
        if (iconSprite != null)
        {
            var images = btn.GetComponentsInChildren<Image>(true);
            foreach (var img in images)
            {
                if (img.gameObject.name.Contains("Icon"))
                {
                    img.sprite = iconSprite;
                    EditorUtility.SetDirty(img);
                    break;
                }
            }
        }

        Debug.Log($"Setup button: {name}");
    }

    [MenuItem("Tools/Setup Language Buttons")]
    public static void SetupButtons()
    {
        // SelectLesson配下のTextTileButtonを検索
        var selectLesson = GameObject.Find("SelectLesson");
        if (selectLesson == null)
        {
            Debug.LogError("SelectLesson not found in scene!");
            return;
        }

        // 言語データ定義
        var languages = Languages;

        // TextTileButton_IconAndLabel_Regularを検索
        var buttons = selectLesson.GetComponentsInChildren<Transform>(true);
        int langIndex = 0;

        foreach (var btn in buttons)
        {
            if (!btn.name.StartsWith("TextTileButton_IconAndLabel_Regular"))
                continue;

            if (langIndex >= languages.Length)
                break;

            var lang = languages[langIndex];

            // 背景色を設定（Imageコンポーネントを探す）
            var images = btn.GetComponentsInChildren<Image>(true);
            foreach (var img in images)
            {
                // 背景用のImageを探す（通常は最初のImageまたは"Background"という名前）
                if (img.gameObject.name.Contains("Background") || img.transform == btn)
                {
                    img.color = lang.bgColor;
                    EditorUtility.SetDirty(img);
                    break;
                }
            }

            // ラベルを設定（TMP_Textコンポーネントを探す）
            var texts = btn.GetComponentsInChildren<TMP_Text>(true);
            foreach (var text in texts)
            {
                if (text.gameObject.name.Contains("Label") || text.gameObject.name.Contains("Text"))
                {
                    text.text = lang.name;
                    EditorUtility.SetDirty(text);
                    break;
                }
            }

            // GameObjectの名前も変更
            btn.name = $"TextTileButton_{lang.id}";
            EditorUtility.SetDirty(btn);

            Debug.Log($"Setup button: {lang.name}");
            langIndex++;
        }

        Debug.Log($"Setup {langIndex} language buttons");

        // シーンを保存するためにdirtyマーク
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
    }
}
