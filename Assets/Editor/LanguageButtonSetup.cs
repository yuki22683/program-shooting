using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class LanguageButtonSetup : EditorWindow
{
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
        var languages = new (string id, string name, Color bgColor)[]
        {
            ("python", "Python", new Color(0.878f, 0.949f, 1f)),       // sky-50
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
            ("kotlin", "Kotlin", new Color(0.980f, 0.961f, 1f)),          // purple-50
            ("swift", "Swift", new Color(1f, 0.988f, 0.933f)),            // amber-50
            ("sql", "SQL", new Color(0.973f, 0.980f, 0.988f)),            // slate-50
            ("lua", "Lua", new Color(0.937f, 0.965f, 1f)),                // blue-50
            ("perl", "Perl", new Color(0.941f, 0.992f, 0.984f)),          // teal-50
        };

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
