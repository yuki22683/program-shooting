using UnityEngine;

[System.Serializable]
public class LanguageInfo
{
    public string id;
    public string name;
    public string extension;
    public Sprite icon;
    public string description;
    public Color backgroundColor;
}

[CreateAssetMenu(fileName = "LanguageData", menuName = "ProgramShooting/Language Data")]
public class LanguageData : ScriptableObject
{
    public LanguageInfo[] languages = new LanguageInfo[]
    {
        new LanguageInfo { id = "python", name = "Python", extension = ".py", description = "Beginner-friendly scripting language", backgroundColor = new Color(0.878f, 0.949f, 1f) }, // sky-50
        new LanguageInfo { id = "javascript", name = "JavaScript", extension = ".js", description = "The language of the web", backgroundColor = new Color(0.996f, 0.988f, 0.878f) }, // yellow-50
        new LanguageInfo { id = "typescript", name = "TypeScript", extension = ".ts", description = "Type-safe modern JavaScript", backgroundColor = new Color(0.937f, 0.965f, 1f) }, // blue-50
        new LanguageInfo { id = "java", name = "Java", extension = ".java", description = "Enterprise systems language", backgroundColor = new Color(0.996f, 0.949f, 0.949f) }, // red-50
        new LanguageInfo { id = "c", name = "C", extension = ".c", description = "Foundation of system programming", backgroundColor = new Color(0.973f, 0.980f, 0.988f) }, // slate-50
        new LanguageInfo { id = "cpp", name = "C++", extension = ".cpp", description = "High-performance system language", backgroundColor = new Color(0.933f, 0.949f, 1f) }, // indigo-50
        new LanguageInfo { id = "csharp", name = "C#", extension = ".cs", description = ".NET framework language", backgroundColor = new Color(0.961f, 0.953f, 1f) }, // violet-50
        new LanguageInfo { id = "go", name = "Go", extension = ".go", description = "Google's modern language", backgroundColor = new Color(0.925f, 0.996f, 1f) }, // cyan-50
        new LanguageInfo { id = "rust", name = "Rust", extension = ".rs", description = "Safe and fast systems language", backgroundColor = new Color(1f, 0.969f, 0.933f) }, // orange-50
        new LanguageInfo { id = "ruby", name = "Ruby", extension = ".rb", description = "Developer-friendly scripting", backgroundColor = new Color(1f, 0.945f, 0.949f) }, // rose-50
        new LanguageInfo { id = "php", name = "PHP", extension = ".php", description = "Web server-side language", backgroundColor = new Color(0.933f, 0.949f, 1f) }, // indigo-50
        new LanguageInfo { id = "bash", name = "Bash", extension = ".sh", description = "Shell scripting essential", backgroundColor = new Color(0.961f, 0.961f, 0.961f) }, // neutral-100
        new LanguageInfo { id = "haskell", name = "Haskell", extension = ".hs", description = "Pure functional language", backgroundColor = new Color(0.980f, 0.961f, 1f) }, // purple-50
        new LanguageInfo { id = "kotlin", name = "Kotlin", extension = ".kt", description = "Android development standard", backgroundColor = new Color(0.980f, 0.961f, 1f) }, // purple-50
        new LanguageInfo { id = "swift", name = "Swift", extension = ".swift", description = "iOS/macOS development", backgroundColor = new Color(1f, 0.988f, 0.933f) }, // amber-50
        new LanguageInfo { id = "sql", name = "SQL", extension = ".sql", description = "Database query language", backgroundColor = new Color(0.973f, 0.980f, 0.988f) }, // slate-50
        new LanguageInfo { id = "lua", name = "Lua", extension = ".lua", description = "Lightweight game scripting", backgroundColor = new Color(0.937f, 0.965f, 1f) }, // blue-50
        new LanguageInfo { id = "perl", name = "Perl", extension = ".pl", description = "Text processing language", backgroundColor = new Color(0.941f, 0.992f, 0.984f) }, // teal-50
    };

    public LanguageInfo GetLanguageById(string id)
    {
        foreach (var lang in languages)
        {
            if (lang.id == id)
                return lang;
        }
        return null;
    }

    public LanguageInfo GetLanguageByIndex(int index)
    {
        if (index >= 0 && index < languages.Length)
            return languages[index];
        return null;
    }
}
