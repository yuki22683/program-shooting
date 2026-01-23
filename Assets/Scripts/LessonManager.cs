using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Meta.XR.MRUtilityKit;

[Serializable]
public class LocalizedComment
{
    [Tooltip("Line index where comment will be inserted (0 = first line)")]
    public int lineIndex;
    [Tooltip("Comment prefix for the programming language (e.g., '#', '//', '/*')")]
    public string commentPrefix = "#";
    [Tooltip("Localization key for the comment text")]
    public string localizationKey;
}

[Serializable]
public class Exercise
{
    public string titleKey;
    public string slideKeyPrefix;
    public int slideCount = 1;
    [TextArea(3, 10)]
    public List<string> correctLines = new List<string>();
    public List<LocalizedComment> comments = new List<LocalizedComment>();
    [TextArea(2, 5)]
    public List<string> expectedOutput = new List<string>();
}

[Serializable]
public class Lesson
{
    public string titleKey;
    public List<Exercise> exercises = new List<Exercise>();
}

public class LessonManager : MonoBehaviour
{
    [Header("Lesson Data")]
    [SerializeField] private List<Lesson> lessons = new List<Lesson>();
    [SerializeField] private int currentLessonIndex = 0;
    [SerializeField] private int currentExerciseIndex = 1;
    [SerializeField] private string currentLanguage = "python";

    [Header("UI References")]
    [SerializeField] private Transform sheetPanelContent;
    [SerializeField] private GameObject consolePanel;
    [SerializeField] private Transform consolePanelContent;
    [SerializeField] private TextMeshProUGUI slidePanelLabel;
    [SerializeField] private GameObject slidePanel;
    [SerializeField] private GameObject sheetPanel;
    [SerializeField] private Button consolePanelNextButton;

    [Header("Block Spawning")]
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private Transform blockSpawnParent;
    [SerializeField] private Vector3 spawnAreaCenter = new Vector3(0, 1.5f, 2f);
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(3f, 1f, 2f);
    [SerializeField] private AudioSource blockDestroySound;
    [SerializeField] private AudioSource wrongSelectionSound;

    private List<TextMeshProUGUI> lineTexts = new List<TextMeshProUGUI>();
    private List<GameObject> spawnedBlocks = new List<GameObject>();
    private List<LocalizationText> lineLocalizationTexts = new List<LocalizationText>();
    private string cachedLanguageCode = "";

    // Track display lines and visibility
    private List<string> currentDisplayLines = new List<string>();
    private List<bool> isCommentLine = new List<bool>();
    private int visibleLineCount = 0;

    // Track block selection order
    private int currentBlockIndex = 0;

    // Track collected tokens for display
    private List<string> currentCodeTokens = new List<string>();
    private List<string> collectedTokens = new List<string>();
    private int currentDisplayLineIndex = -1;

    // Track which code line we're currently working on (0 = first, 1 = second, etc.)
    private int currentCodeLineIndex = 0;

    private void Awake()
    {
        Debug.Log("[LessonManager] Awake called");
        FindLineTexts();
    }

    private void Start()
    {
        Debug.Log("[LessonManager] Start called");

        // Initialize with Python lesson 1, exercise 2 (variables) from senkou-code
        InitializeDefaultExercise();
        StartCoroutine(WaitForLocalizationAndDisplay());

        // Register console panel next button listener
        if (consolePanelNextButton != null)
        {
            consolePanelNextButton.onClick.AddListener(GoToNextExercise);
            Debug.Log("[LessonManager] Registered GoToNextExercise on consolePanelNextButton");
        }

        // Setup SelectLanguagePanel to position in front of headset
        Debug.Log("[LessonManager] Calling SetupSelectLanguagePanel");
        SetupSelectLanguagePanel();
    }

    /// <summary>
    /// Sets up SelectLanguagePanel (言語選択パネル) to appear 1m in front of the headset when activated
    /// </summary>
    private void SetupSelectLanguagePanel()
    {
        Debug.Log("[LessonManager] SetupSelectLanguagePanel started");

        // Find SelectLanguagePanel even if inactive - search through all root objects
        GameObject selectLanguagePanel = null;

        // Search in all root objects
        foreach (GameObject rootObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            selectLanguagePanel = FindChildByName(rootObj.transform, "SelectLanguagePanel");
            if (selectLanguagePanel != null) break;
        }

        if (selectLanguagePanel == null)
        {
            Debug.LogWarning("[LessonManager] SelectLanguagePanel not found in scene");
            return;
        }

        Debug.Log($"[LessonManager] Found SelectLanguagePanel: {selectLanguagePanel.name}");

        // Add PositionInFrontOfHeadset component if not already present
        PositionInFrontOfHeadset positioner = selectLanguagePanel.GetComponent<PositionInFrontOfHeadset>();
        if (positioner == null)
        {
            positioner = selectLanguagePanel.AddComponent<PositionInFrontOfHeadset>();
            Debug.Log("[LessonManager] Added PositionInFrontOfHeadset component");
        }

        // Configure: 1m in front of headset, same height, facing headset
        positioner.Configure(1.0f, 0f, true, true);

        // Add SelectLanguagePanelController to handle button click events
        SelectLanguagePanelController controller = selectLanguagePanel.GetComponent<SelectLanguagePanelController>();
        if (controller == null)
        {
            controller = selectLanguagePanel.AddComponent<SelectLanguagePanelController>();
            Debug.Log("[LessonManager] Added SelectLanguagePanelController component to SelectLanguagePanel");
        }

        // Activate the panel
        selectLanguagePanel.SetActive(true);

        Debug.Log("[LessonManager] SelectLanguagePanel activated and configured to appear 1m in front of headset");
    }

    /// <summary>
    /// Recursively finds a child GameObject by name (works with inactive objects)
    /// </summary>
    private GameObject FindChildByName(Transform parent, string name)
    {
        if (parent.name == name)
            return parent.gameObject;

        foreach (Transform child in parent)
        {
            GameObject result = FindChildByName(child, name);
            if (result != null)
                return result;
        }
        return null;
    }

    private void OnDestroy()
    {
        // Unregister button listener
        if (consolePanelNextButton != null)
        {
            consolePanelNextButton.onClick.RemoveListener(GoToNextExercise);
        }
    }

    /// <summary>
    /// Initialize with all exercises from all Python lessons
    /// Based on senkou-code/data/lessons/python.ts, python2.ts, python3.ts, python4.ts, python5.ts
    /// </summary>
    private void InitializeDefaultExercise()
    {
        lessons.Clear();

        // ==================== LESSON 1: Python I - パイソンに挑戦！ ====================
        var lesson1 = new Lesson { titleKey = "python_lesson1_title" };

        // Ex1: 画面に文字を出してみましょう (Hello, World!)
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "python_lesson1_ex1_title",
            slideKeyPrefix = "python_lesson1_ex1",
            slideCount = 4,
            correctLines = new List<string> { "print('Hello, World!')" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson1_ex1_comment1" }
            },
            expectedOutput = new List<string> { "Hello, World!" }
        });

        // Ex2: 便利な「はこ」変数
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "python_lesson1_ex2_title",
            slideKeyPrefix = "python_lesson1_ex2",
            slideCount = 3,
            correctLines = new List<string> { "name = 'Python'", "print(name)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson1_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson1_ex2_comment2" }
            },
            expectedOutput = new List<string> { "Python" }
        });

        // Ex3: コンピュータで計算しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "python_lesson1_ex3_title",
            slideKeyPrefix = "python_lesson1_ex3",
            slideCount = 3,
            correctLines = new List<string> { "x = 10", "y = 5", "print(x + y)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson1_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson1_ex3_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "python_lesson1_ex3_comment3" }
            },
            expectedOutput = new List<string> { "15" }
        });

        // Ex4: 剰余演算子（%）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "python_lesson1_ex4_title",
            slideKeyPrefix = "python_lesson1_ex4",
            slideCount = 3,
            correctLines = new List<string> { "print(10 % 3)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson1_ex4_comment1" }
            },
            expectedOutput = new List<string> { "1" }
        });

        // Ex5: 累算代入演算子（+=、-=）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "python_lesson1_ex5_title",
            slideKeyPrefix = "python_lesson1_ex5",
            slideCount = 3,
            correctLines = new List<string> { "score = 50", "score += 10", "print(score)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson1_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson1_ex5_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "python_lesson1_ex5_comment3" }
            },
            expectedOutput = new List<string> { "60" }
        });

        // Ex6: 文章の中に「はこ」を入れましょう (f-string)
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "python_lesson1_ex6_title",
            slideKeyPrefix = "python_lesson1_ex6",
            slideCount = 3,
            correctLines = new List<string> { "age = 10", "print(f'私は{age}歳です')" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson1_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson1_ex6_comment2" }
            },
            expectedOutput = new List<string> { "私は10歳です" }
        });

        // Ex7: たくさんのデータをまとめましょう「リスト」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "python_lesson1_ex7_title",
            slideKeyPrefix = "python_lesson1_ex7",
            slideCount = 3,
            correctLines = new List<string> { "colors = ['あか', 'あお']", "print(colors[1])" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson1_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson1_ex7_comment2" }
            },
            expectedOutput = new List<string> { "あお" }
        });

        // Ex8: 「もし〜なら」で分ける if文
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "python_lesson1_ex8_title",
            slideKeyPrefix = "python_lesson1_ex8",
            slideCount = 3,
            correctLines = new List<string> { "score = 100", "if score > 80:", "    print('ごうかく！')" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson1_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson1_ex8_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "python_lesson1_ex8_comment3" }
            },
            expectedOutput = new List<string> { "ごうかく！" }
        });

        // Ex9: ちがう場合は？ if-else文
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "python_lesson1_ex9_title",
            slideKeyPrefix = "python_lesson1_ex9",
            slideCount = 3,
            correctLines = new List<string> { "age = 10", "if age >= 20:", "    print('おとな')", "else:", "    print('こども')" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson1_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson1_ex9_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "#", localizationKey = "python_lesson1_ex9_comment3" }
            },
            expectedOutput = new List<string> { "こども" }
        });

        // Ex10: 論理演算子（and, or）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "python_lesson1_ex10_title",
            slideKeyPrefix = "python_lesson1_ex10",
            slideCount = 3,
            correctLines = new List<string> { "score = 85", "if score >= 80 and score <= 100:", "    print('ごうかく！')" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson1_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson1_ex10_comment2" }
            },
            expectedOutput = new List<string> { "ごうかく！" }
        });

        // Ex11: ぐるぐる回す for文
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "python_lesson1_ex11_title",
            slideKeyPrefix = "python_lesson1_ex11",
            slideCount = 3,
            correctLines = new List<string> { "names = ['たろう', 'はなこ']", "for name in names:", "    print(name)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson1_ex11_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson1_ex11_comment2" }
            },
            expectedOutput = new List<string> { "たろう", "はなこ" }
        });

        // Ex12: 名前で探しましょう「辞書」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "python_lesson1_ex12_title",
            slideKeyPrefix = "python_lesson1_ex12",
            slideCount = 3,
            correctLines = new List<string> { "colors = {'みかん': 'オレンジ'}", "print(colors['みかん'])" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson1_ex12_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson1_ex12_comment2" }
            },
            expectedOutput = new List<string> { "オレンジ" }
        });

        // Ex13: 自分だけの関数を作ろう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "python_lesson1_ex13_title",
            slideKeyPrefix = "python_lesson1_ex13",
            slideCount = 3,
            correctLines = new List<string> { "def greet():", "    print('こんにちは')", "greet()" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson1_ex13_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "python_lesson1_ex13_comment2" }
            },
            expectedOutput = new List<string> { "こんにちは" }
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: Python II - ステップアップ！ ====================
        var lesson2 = new Lesson { titleKey = "python_lesson2_title" };

        // Ex1: 引数を使った関数
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "python_lesson2_ex1_title",
            slideKeyPrefix = "python_lesson2_ex1",
            slideCount = 2,
            correctLines = new List<string> { "def greet(name):", "    print(f'Hello, {name}!')", "greet('Python')" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson2_ex1_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "python_lesson2_ex1_comment2" }
            },
            expectedOutput = new List<string> { "Hello, Python!" }
        });

        // Ex2: デフォルト引数
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "python_lesson2_ex2_title",
            slideKeyPrefix = "python_lesson2_ex2",
            slideCount = 2,
            correctLines = new List<string> { "def greet(name='World'):", "    print(f'Hello, {name}!')", "greet()" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson2_ex2_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "python_lesson2_ex2_comment2" }
            },
            expectedOutput = new List<string> { "Hello, World!" }
        });

        // Ex3: 戻り値（return）
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "python_lesson2_ex3_title",
            slideKeyPrefix = "python_lesson2_ex3",
            slideCount = 2,
            correctLines = new List<string> { "def add(a, b):", "    return a + b", "result = add(3, 5)", "print(result)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson2_ex3_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "python_lesson2_ex3_comment2" }
            },
            expectedOutput = new List<string> { "8" }
        });

        // Ex4: 複数の戻り値
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "python_lesson2_ex4_title",
            slideKeyPrefix = "python_lesson2_ex4",
            slideCount = 2,
            correctLines = new List<string> { "def calc(a, b):", "    return a + b, a - b", "sum_val, diff = calc(10, 3)", "print(sum_val)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson2_ex4_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "python_lesson2_ex4_comment2" }
            },
            expectedOutput = new List<string> { "13" }
        });

        // Ex5: 文字列スライス
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "python_lesson2_ex5_title",
            slideKeyPrefix = "python_lesson2_ex5",
            slideCount = 2,
            correctLines = new List<string> { "word = 'Programming'", "print(word[0:4])" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson2_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson2_ex5_comment2" }
            },
            expectedOutput = new List<string> { "Prog" }
        });

        // Ex6: range() で数列を作る
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "python_lesson2_ex6_title",
            slideKeyPrefix = "python_lesson2_ex6",
            slideCount = 2,
            correctLines = new List<string> { "for i in range(1, 6):", "    print(i)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson2_ex6_comment1" }
            },
            expectedOutput = new List<string> { "1", "2", "3", "4", "5" }
        });

        // Ex7: 累算代入演算子（+=、-=）
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "python_lesson2_ex7_title",
            slideKeyPrefix = "python_lesson2_ex7",
            slideCount = 2,
            correctLines = new List<string> { "total = 0", "total += 10", "total += 5", "print(total)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson2_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson2_ex7_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "python_lesson2_ex7_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "#", localizationKey = "python_lesson2_ex7_comment4" }
            },
            expectedOutput = new List<string> { "15" }
        });

        // Ex8: 剰余演算子（%）
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "python_lesson2_ex8_title",
            slideKeyPrefix = "python_lesson2_ex8",
            slideCount = 2,
            correctLines = new List<string> { "remainder = 10 % 3", "print(remainder)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson2_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson2_ex8_comment2" }
            },
            expectedOutput = new List<string> { "1" }
        });

        // Ex9: 論理演算子（and, or, not）
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "python_lesson2_ex9_title",
            slideKeyPrefix = "python_lesson2_ex9",
            slideCount = 2,
            correctLines = new List<string> { "age = 25", "if age >= 20 and age < 30:", "    print('20代です')" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson2_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson2_ex9_comment2" }
            },
            expectedOutput = new List<string> { "20代です" }
        });

        // Ex10: リスト内包表記
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "python_lesson2_ex10_title",
            slideKeyPrefix = "python_lesson2_ex10",
            slideCount = 2,
            correctLines = new List<string> { "doubled = [x * 2 for x in range(1, 6)]", "print(doubled)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson2_ex10_comment1" }
            },
            expectedOutput = new List<string> { "[2, 4, 6, 8, 10]" }
        });

        // Ex11: 条件付きリスト内包表記
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "python_lesson2_ex11_title",
            slideKeyPrefix = "python_lesson2_ex11",
            slideCount = 2,
            correctLines = new List<string> { "multiples = [n for n in range(1, 11) if n % 3 == 0]", "print(multiples)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson2_ex11_comment1" }
            },
            expectedOutput = new List<string> { "[3, 6, 9]" }
        });

        // Ex12: 例外処理（try-except）
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "python_lesson2_ex12_title",
            slideKeyPrefix = "python_lesson2_ex12",
            slideCount = 2,
            correctLines = new List<string> { "try:", "    num = int('abc')", "except:", "    print('Error')" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson2_ex12_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "python_lesson2_ex12_comment2" }
            },
            expectedOutput = new List<string> { "Error" }
        });

        // Ex13: クラスの基本
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "python_lesson2_ex13_title",
            slideKeyPrefix = "python_lesson2_ex13",
            slideCount = 2,
            correctLines = new List<string> { "class Cat:", "    def meow(self):", "        print('Meow!')", "", "cat = Cat()", "cat.meow()" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson2_ex13_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "python_lesson2_ex13_comment2" }
            },
            expectedOutput = new List<string> { "Meow!" }
        });

        // Ex14: コンストラクタ（__init__）
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "python_lesson2_ex14_title",
            slideKeyPrefix = "python_lesson2_ex14",
            slideCount = 2,
            correctLines = new List<string> { "class Robot:", "    def __init__(self, name):", "        self.name = name", "    def say_name(self):", "        print(self.name)", "", "r = Robot('R2D2')", "r.say_name()" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson2_ex14_comment1" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "#", localizationKey = "python_lesson2_ex14_comment2" }
            },
            expectedOutput = new List<string> { "R2D2" }
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: Python III - 上級テクニック ====================
        var lesson3 = new Lesson { titleKey = "python_lesson3_title" };

        // Ex1: ラムダ式（無名関数）
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "python_lesson3_ex1_title",
            slideKeyPrefix = "python_lesson3_ex1",
            slideCount = 2,
            correctLines = new List<string> { "square = lambda x: x ** 2", "print(square(5))" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson3_ex1_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson3_ex1_comment2" }
            },
            expectedOutput = new List<string> { "25" }
        });

        // Ex2: *args（可変長引数）
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "python_lesson3_ex2_title",
            slideKeyPrefix = "python_lesson3_ex2",
            slideCount = 2,
            correctLines = new List<string> { "def add_all(*args):", "    total = 0", "    for n in args:", "        total += n", "    return total", "", "print(add_all(1, 2, 3, 4))" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson3_ex2_comment1" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "#", localizationKey = "python_lesson3_ex2_comment2" }
            },
            expectedOutput = new List<string> { "10" }
        });

        // Ex3: **kwargs（キーワード引数）
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "python_lesson3_ex3_title",
            slideKeyPrefix = "python_lesson3_ex3",
            slideCount = 2,
            correctLines = new List<string> { "def print_info(**kwargs):", "    for k, v in kwargs.items():", "        print(f'{k} = {v}')", "", "print_info(x=10, y=20)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson3_ex3_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "python_lesson3_ex3_comment2" }
            },
            expectedOutput = new List<string> { "x = 10", "y = 20" }
        });

        // Ex4: enumerate で番号付きループ
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "python_lesson3_ex4_title",
            slideKeyPrefix = "python_lesson3_ex4",
            slideCount = 2,
            correctLines = new List<string> { "colors = ['red', 'green', 'blue']", "for i, color in enumerate(colors):", "    print(f'{i}: {color}')" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson3_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson3_ex4_comment2" }
            },
            expectedOutput = new List<string> { "0: red", "1: green", "2: blue" }
        });

        // Ex5: zip で複数リストを同時にループ
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "python_lesson3_ex5_title",
            slideKeyPrefix = "python_lesson3_ex5",
            slideCount = 2,
            correctLines = new List<string> { "keys = ['a', 'b', 'c']", "values = [1, 2, 3]", "for k, v in zip(keys, values):", "    print(f'{k}: {v}')" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson3_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson3_ex5_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "python_lesson3_ex5_comment3" }
            },
            expectedOutput = new List<string> { "a: 1", "b: 2", "c: 3" }
        });

        // Ex6: ジェネレータ（yield）
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "python_lesson3_ex6_title",
            slideKeyPrefix = "python_lesson3_ex6",
            slideCount = 2,
            correctLines = new List<string> { "def even_numbers(n):", "    for i in range(n):", "        yield i * 2", "", "for num in even_numbers(4):", "    print(num)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson3_ex6_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "python_lesson3_ex6_comment2" }
            },
            expectedOutput = new List<string> { "0", "2", "4", "6" }
        });

        // Ex7: ジェネレータ式
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "python_lesson3_ex7_title",
            slideKeyPrefix = "python_lesson3_ex7",
            slideCount = 2,
            correctLines = new List<string> { "nums = range(1, 6)", "total = sum(x * x for x in nums)", "print(total)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson3_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson3_ex7_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "python_lesson3_ex7_comment3" }
            },
            expectedOutput = new List<string> { "55" }
        });

        // Ex8: デコレータの基本
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "python_lesson3_ex8_title",
            slideKeyPrefix = "python_lesson3_ex8",
            slideCount = 2,
            correctLines = new List<string> { "def show_call(func):", "    def wrapper():", "        print('関数を呼び出します')", "        func()", "    return wrapper", "", "@show_call", "def greet():", "    print('Hello!')", "", "greet()" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson3_ex8_comment1" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "#", localizationKey = "python_lesson3_ex8_comment2" }
            },
            expectedOutput = new List<string> { "関数を呼び出します", "Hello!" }
        });

        // Ex9: any と all
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "python_lesson3_ex9_title",
            slideKeyPrefix = "python_lesson3_ex9",
            slideCount = 2,
            correctLines = new List<string> { "nums = [1, 2, 3, 4, 5]", "result = all(x > 0 for x in nums)", "print(result)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson3_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson3_ex9_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "python_lesson3_ex9_comment3" }
            },
            expectedOutput = new List<string> { "True" }
        });

        // Ex10: with文（コンテキストマネージャ）
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "python_lesson3_ex10_title",
            slideKeyPrefix = "python_lesson3_ex10",
            slideCount = 2,
            correctLines = new List<string> { "class MockFile:", "    def __enter__(self):", "        print('opened')", "        return self", "    def __exit__(self, *args):", "        print('closed')", "", "with MockFile() as f:", "    print('using')" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson3_ex10_comment1" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "#", localizationKey = "python_lesson3_ex10_comment2" }
            },
            expectedOutput = new List<string> { "opened", "using", "closed" }
        });

        lessons.Add(lesson3);

        // ==================== LESSON 4: Python IV - オブジェクト指向の極意 ====================
        var lesson4 = new Lesson { titleKey = "python_lesson4_title" };

        // Ex1: クラスの継承
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "python_lesson4_ex1_title",
            slideKeyPrefix = "python_lesson4_ex1",
            slideCount = 1,
            correctLines = new List<string> { "class Animal:", "    def __init__(self, name):", "        self.name = name", "", "class Dog(Animal):", "    def bark(self):", "        print(f'{self.name} says Woof!')", "", "dog = Dog('Pochi')", "dog.bark()" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson4_ex1_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "python_lesson4_ex1_comment2" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "#", localizationKey = "python_lesson4_ex1_comment3" }
            },
            expectedOutput = new List<string> { "Pochi says Woof!" }
        });

        // Ex2: super()で親を呼ぶ
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "python_lesson4_ex2_title",
            slideKeyPrefix = "python_lesson4_ex2",
            slideCount = 1,
            correctLines = new List<string> { "class Person:", "    def __init__(self, name):", "        self.name = name", "", "class Student(Person):", "    def __init__(self, name, grade):", "        super().__init__(name)", "        self.grade = grade", "", "s = Student('Taro', 3)", "print(f'{s.name} is in grade {s.grade}')" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson4_ex2_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "python_lesson4_ex2_comment2" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "#", localizationKey = "python_lesson4_ex2_comment3" }
            },
            expectedOutput = new List<string> { "Taro is in grade 3" }
        });

        // Ex3: @propertyデコレータ
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "python_lesson4_ex3_title",
            slideKeyPrefix = "python_lesson4_ex3",
            slideCount = 1,
            correctLines = new List<string> { "class Rectangle:", "    def __init__(self, width, height):", "        self.width = width", "        self.height = height", "    @property", "    def area(self):", "        return self.width * self.height", "", "r = Rectangle(4, 5)", "print(r.area)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson4_ex3_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "python_lesson4_ex3_comment2" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "#", localizationKey = "python_lesson4_ex3_comment3" }
            },
            expectedOutput = new List<string> { "20" }
        });

        // Ex4: @classmethodデコレータ
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "python_lesson4_ex4_title",
            slideKeyPrefix = "python_lesson4_ex4",
            slideCount = 1,
            correctLines = new List<string> { "class Dog:", "    count = 0", "    def __init__(self, name):", "        self.name = name", "        Dog.count += 1", "    @classmethod", "    def get_count(cls):", "        return cls.count", "", "d1 = Dog('Pochi')", "d2 = Dog('Hachi')", "print(Dog.get_count())" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson4_ex4_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "#", localizationKey = "python_lesson4_ex4_comment2" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "#", localizationKey = "python_lesson4_ex4_comment3" }
            },
            expectedOutput = new List<string> { "2" }
        });

        // Ex5: @staticmethodデコレータ
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "python_lesson4_ex5_title",
            slideKeyPrefix = "python_lesson4_ex5",
            slideCount = 1,
            correctLines = new List<string> { "class Validator:", "    @staticmethod", "    def is_positive(n):", "        return n > 0", "", "print(Validator.is_positive(5))", "print(Validator.is_positive(-3))" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson4_ex5_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "python_lesson4_ex5_comment2" }
            },
            expectedOutput = new List<string> { "True", "False" }
        });

        // Ex6: 抽象基底クラス
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "python_lesson4_ex6_title",
            slideKeyPrefix = "python_lesson4_ex6",
            slideCount = 1,
            correctLines = new List<string> { "from abc import ABC, abstractmethod", "", "class Animal(ABC):", "    @abstractmethod", "    def speak(self):", "        pass", "", "class Cat(Animal):", "    def speak(self):", "        return 'Meow'", "", "cat = Cat()", "print(cat.speak())" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson4_ex6_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "python_lesson4_ex6_comment2" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "#", localizationKey = "python_lesson4_ex6_comment3" }
            },
            expectedOutput = new List<string> { "Meow" }
        });

        // Ex7: __str__メソッド
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "python_lesson4_ex7_title",
            slideKeyPrefix = "python_lesson4_ex7",
            slideCount = 1,
            correctLines = new List<string> { "class Book:", "    def __init__(self, title, author):", "        self.title = title", "        self.author = author", "    def __str__(self):", "        return f'{self.title} by {self.author}'", "", "book = Book('Python Guide', 'Taro')", "print(book)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson4_ex7_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "python_lesson4_ex7_comment2" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "#", localizationKey = "python_lesson4_ex7_comment3" }
            },
            expectedOutput = new List<string> { "Python Guide by Taro" }
        });

        // Ex8: __eq__メソッド
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "python_lesson4_ex8_title",
            slideKeyPrefix = "python_lesson4_ex8",
            slideCount = 1,
            correctLines = new List<string> { "class Vector:", "    def __init__(self, x, y):", "        self.x = x", "        self.y = y", "    def __eq__(self, other):", "        return self.x == other.x and self.y == other.y", "", "v1 = Vector(3, 4)", "v2 = Vector(3, 4)", "print(v1 == v2)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson4_ex8_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "python_lesson4_ex8_comment2" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "#", localizationKey = "python_lesson4_ex8_comment3" }
            },
            expectedOutput = new List<string> { "True" }
        });

        // Ex9: __len__メソッド
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "python_lesson4_ex9_title",
            slideKeyPrefix = "python_lesson4_ex9",
            slideCount = 1,
            correctLines = new List<string> { "class Team:", "    def __init__(self, members):", "        self.members = members", "    def __len__(self):", "        return len(self.members)", "", "team = Team(['Alice', 'Bob', 'Charlie'])", "print(len(team))" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson4_ex9_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "python_lesson4_ex9_comment2" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "#", localizationKey = "python_lesson4_ex9_comment3" }
            },
            expectedOutput = new List<string> { "3" }
        });

        // Ex10: dataclassデコレータ
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "python_lesson4_ex10_title",
            slideKeyPrefix = "python_lesson4_ex10",
            slideCount = 1,
            correctLines = new List<string> { "from dataclasses import dataclass", "", "@dataclass", "class Person:", "    name: str", "    age: int", "", "p = Person('Taro', 25)", "print(p)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson4_ex10_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "python_lesson4_ex10_comment2" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "#", localizationKey = "python_lesson4_ex10_comment3" }
            },
            expectedOutput = new List<string> { "Person(name='Taro', age=25)" }
        });

        lessons.Add(lesson4);

        // ==================== LESSON 5: Python V - ファイルとデータ処理 ====================
        var lesson5 = new Lesson { titleKey = "python_lesson5_title" };

        // Ex1: ファイルを開く（with文）
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "python_lesson5_ex1_title",
            slideKeyPrefix = "python_lesson5_ex1",
            slideCount = 1,
            correctLines = new List<string> { "filename = 'test.txt'", "with open(filename, 'w') as f:", "    f.write('Hello, Python!')", "", "with open(filename, 'r') as f:", "    print(f.read())" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson5_ex1_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "python_lesson5_ex1_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "python_lesson5_ex1_comment3" }
            },
            expectedOutput = new List<string> { "Hello, Python!" }
        });

        // Ex2: ファイルを1行ずつ読む
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "python_lesson5_ex2_title",
            slideKeyPrefix = "python_lesson5_ex2",
            slideCount = 1,
            correctLines = new List<string> { "with open('test.txt', 'w') as f:", "    f.write('line1\\nline2\\nline3')", "", "with open('test.txt', 'r') as f:", "    for line in f:", "        print(line.strip())" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson5_ex2_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "python_lesson5_ex2_comment2" }
            },
            expectedOutput = new List<string> { "line1", "line2", "line3" }
        });

        // Ex3: JSONを読み込む
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "python_lesson5_ex3_title",
            slideKeyPrefix = "python_lesson5_ex3",
            slideCount = 1,
            correctLines = new List<string> { "import json", "", "json_str = '{\"name\": \"Python\", \"version\": 3.12}'", "data = json.loads(json_str)", "print(data['name'])" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson5_ex3_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "python_lesson5_ex3_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "python_lesson5_ex3_comment3" }
            },
            expectedOutput = new List<string> { "Python" }
        });

        // Ex4: JSONに変換する
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "python_lesson5_ex4_title",
            slideKeyPrefix = "python_lesson5_ex4",
            slideCount = 1,
            correctLines = new List<string> { "import json", "", "data = {'language': 'Python', 'level': 'advanced'}", "json_str = json.dumps(data)", "print(json_str)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson5_ex4_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "python_lesson5_ex4_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "python_lesson5_ex4_comment3" }
            },
            expectedOutput = new List<string> { "{\"language\": \"Python\", \"level\": \"advanced\"}" }
        });

        // Ex5: 正規表現（search）
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "python_lesson5_ex5_title",
            slideKeyPrefix = "python_lesson5_ex5",
            slideCount = 1,
            correctLines = new List<string> { "import re", "", "text = 'Call me at 090-1234-5678'", "match = re.search(r'\\d{3}-\\d{4}-\\d{4}', text)", "if match:", "    print(match.group())" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson5_ex5_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "python_lesson5_ex5_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "python_lesson5_ex5_comment3" }
            },
            expectedOutput = new List<string> { "090-1234-5678" }
        });

        // Ex6: 正規表現（findall）
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "python_lesson5_ex6_title",
            slideKeyPrefix = "python_lesson5_ex6",
            slideCount = 1,
            correctLines = new List<string> { "import re", "", "text = 'email1@test.com and email2@test.com'", "emails = re.findall(r'\\w+@\\w+\\.\\w+', text)", "print(emails)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson5_ex6_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "python_lesson5_ex6_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "python_lesson5_ex6_comment3" }
            },
            expectedOutput = new List<string> { "['email1@test.com', 'email2@test.com']" }
        });

        // Ex7: 正規表現（sub）
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "python_lesson5_ex7_title",
            slideKeyPrefix = "python_lesson5_ex7",
            slideCount = 1,
            correctLines = new List<string> { "import re", "", "text = 'Hello   World   Python'", "result = re.sub(r'\\s+', ' ', text)", "print(result)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson5_ex7_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "python_lesson5_ex7_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "python_lesson5_ex7_comment3" }
            },
            expectedOutput = new List<string> { "Hello World Python" }
        });

        // Ex8: collections.Counter
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "python_lesson5_ex8_title",
            slideKeyPrefix = "python_lesson5_ex8",
            slideCount = 1,
            correctLines = new List<string> { "from collections import Counter", "", "text = 'hello world'", "count = Counter(text)", "print(count.most_common(3))" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson5_ex8_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "python_lesson5_ex8_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "python_lesson5_ex8_comment3" }
            },
            expectedOutput = new List<string> { "[('l', 3), ('o', 2), ('h', 1)]" }
        });

        // Ex9: collections.defaultdict
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "python_lesson5_ex9_title",
            slideKeyPrefix = "python_lesson5_ex9",
            slideCount = 1,
            correctLines = new List<string> { "from collections import defaultdict", "", "d = defaultdict(list)", "d['fruits'].append('apple')", "d['fruits'].append('banana')", "print(d['fruits'])" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson5_ex9_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "python_lesson5_ex9_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "#", localizationKey = "python_lesson5_ex9_comment3" }
            },
            expectedOutput = new List<string> { "['apple', 'banana']" }
        });

        // Ex10: itertools.chain
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "python_lesson5_ex10_title",
            slideKeyPrefix = "python_lesson5_ex10",
            slideCount = 1,
            correctLines = new List<string> { "from itertools import chain", "", "list1 = [1, 2, 3]", "list2 = [4, 5, 6]", "result = list(chain(list1, list2))", "print(result)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "python_lesson5_ex10_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "python_lesson5_ex10_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "python_lesson5_ex10_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "#", localizationKey = "python_lesson5_ex10_comment4" }
            },
            expectedOutput = new List<string> { "[1, 2, 3, 4, 5, 6]" }
        });

        lessons.Add(lesson5);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;

        Debug.Log($"[LessonManager] Initialized {lessons.Count} Python lessons with {lessons[0].exercises.Count + lessons[1].exercises.Count + lessons[2].exercises.Count + lessons[3].exercises.Count + lessons[4].exercises.Count} total exercises");
    }

    private IEnumerator WaitForLocalizationAndDisplay()
    {
        while (LocalizationManager.Instance == null || string.IsNullOrEmpty(LocalizationManager.languageCode))
        {
            yield return null;
        }

        cachedLanguageCode = LocalizationManager.languageCode;
        DisplayCurrentExercise();
        // Block spawning is now triggered by SlideManager's complete button
    }

    private void Update()
    {
        if (LocalizationManager.languageCode != cachedLanguageCode)
        {
            cachedLanguageCode = LocalizationManager.languageCode;
            DisplayCurrentExercise();
        }
    }

    private void FindLineTexts()
    {
        lineTexts.Clear();
        lineLocalizationTexts.Clear();

        if (sheetPanelContent == null)
        {
            Debug.LogError("SheetPanelContent is not assigned!");
            return;
        }

        for (int i = 0; i < sheetPanelContent.childCount; i++)
        {
            Transform child = sheetPanelContent.GetChild(i);
            if (child.name.StartsWith("Line"))
            {
                Transform instruction = child.Find("Instruction");
                if (instruction != null)
                {
                    TextMeshProUGUI tmp = instruction.GetComponent<TextMeshProUGUI>();
                    if (tmp != null)
                    {
                        lineTexts.Add(tmp);

                        // Also collect LocalizationText component (may be null)
                        LocalizationText locText = instruction.GetComponent<LocalizationText>();
                        lineLocalizationTexts.Add(locText);
                    }
                }
            }
        }

        Debug.Log($"Found {lineTexts.Count} line text components");
    }

    public void DisplayCurrentExercise()
    {
        if (lessons.Count == 0)
        {
            Debug.LogWarning("No lessons configured!");
            return;
        }

        if (currentLessonIndex < 0 || currentLessonIndex >= lessons.Count)
        {
            Debug.LogError($"Invalid lesson index: {currentLessonIndex}");
            return;
        }

        Lesson currentLesson = lessons[currentLessonIndex];

        if (currentLesson.exercises.Count == 0)
        {
            Debug.LogWarning("Current lesson has no exercises!");
            return;
        }

        if (currentExerciseIndex < 0 || currentExerciseIndex >= currentLesson.exercises.Count)
        {
            Debug.LogError($"Invalid exercise index: {currentExerciseIndex}");
            return;
        }

        Exercise currentExercise = currentLesson.exercises[currentExerciseIndex];
        DisplayExercise(currentExercise);
    }

    private void DisplayExercise(Exercise exercise)
    {
        currentDisplayLines = BuildDisplayLines(exercise);

        // Find the first comment line index to determine initial visible count
        visibleLineCount = 0;
        for (int i = 0; i < isCommentLine.Count; i++)
        {
            if (isCommentLine[i])
            {
                visibleLineCount = i + 1; // Show up to and including the first comment
                break;
            }
        }

        // If no comment found, show nothing
        if (visibleLineCount == 0 && currentDisplayLines.Count > 0)
        {
            visibleLineCount = 0;
        }

        UpdateLineDisplay();
        UpdateSlidePanelContent(exercise);

        Debug.Log($"Displayed exercise: {exercise.titleKey} with {currentDisplayLines.Count} lines, showing {visibleLineCount} initially");
    }

    /// <summary>
    /// Updates the slide panel label with the exercise's tutorial content
    /// </summary>
    private void UpdateSlidePanelContent(Exercise exercise)
    {
        // Use SlideManager to handle paginated slides
        if (SlideManager.Instance != null)
        {
            if (!string.IsNullOrEmpty(exercise.slideKeyPrefix) && exercise.slideCount > 0)
            {
                SlideManager.Instance.SetSlideConfig(exercise.titleKey, exercise.slideKeyPrefix, exercise.slideCount);
                Debug.Log($"[LessonManager] Configured SlideManager with prefix: {exercise.slideKeyPrefix}, count: {exercise.slideCount}");
            }
            else
            {
                // Even without slides, set the title in TopBar
                SlideManager.Instance.SetExerciseTitle(exercise.titleKey);
                Debug.Log($"[LessonManager] Set exercise title only: {exercise.titleKey}");
            }
        }
        else
        {
            Debug.LogWarning("[LessonManager] SlideManager.Instance is null");
        }
    }

    private void UpdateLineDisplay()
    {
        // Get the appropriate font for current language
        TMP_FontAsset localizedFont = null;
        if (LocalizationManager.Instance != null)
        {
            localizedFont = LocalizationManager.Instance.GetLocalizedFont();
        }

        for (int i = 0; i < lineTexts.Count; i++)
        {
            // Disable LocalizationText component to prevent it from overwriting our text
            if (i < lineLocalizationTexts.Count && lineLocalizationTexts[i] != null)
            {
                lineLocalizationTexts[i].enabled = false;
            }

            // Set the localized font
            if (localizedFont != null)
            {
                lineTexts[i].font = localizedFont;
            }

            // Only show lines up to visibleLineCount
            if (i < visibleLineCount && i < currentDisplayLines.Count)
            {
                // Apply syntax highlighting
                lineTexts[i].text = ApplySyntaxHighlighting(currentDisplayLines[i]);
            }
            else
            {
                lineTexts[i].text = "";
            }
        }
    }

    /// <summary>
    /// Reveals the next line in the exercise
    /// </summary>
    public void RevealNextLine()
    {
        if (visibleLineCount < currentDisplayLines.Count)
        {
            visibleLineCount++;
            UpdateLineDisplay();
            Debug.Log($"Revealed line {visibleLineCount} of {currentDisplayLines.Count}");
        }
    }

    /// <summary>
    /// Reveals all remaining lines in the exercise
    /// </summary>
    public void RevealAllLines()
    {
        visibleLineCount = currentDisplayLines.Count;
        UpdateLineDisplay();
        Debug.Log($"Revealed all {currentDisplayLines.Count} lines");
    }

    /// <summary>
    /// Gets whether all lines are currently visible
    /// </summary>
    public bool AreAllLinesVisible => visibleLineCount >= currentDisplayLines.Count;

    /// <summary>
    /// Gets the current visible line count
    /// </summary>
    public int VisibleLineCount => visibleLineCount;

    private List<string> BuildDisplayLines(Exercise exercise)
    {
        List<string> result = new List<string>();
        isCommentLine.Clear();

        Debug.Log($"BuildDisplayLines: correctLines={exercise.correctLines.Count}, comments={exercise.comments.Count}");

        Dictionary<int, LocalizedComment> commentMap = new Dictionary<int, LocalizedComment>();
        foreach (var comment in exercise.comments)
        {
            commentMap[comment.lineIndex] = comment;
            Debug.Log($"Comment at line {comment.lineIndex}: prefix='{comment.commentPrefix}', key='{comment.localizationKey}'");
        }

        int codeIndex = 0;
        int lineIndex = 0;

        while (codeIndex < exercise.correctLines.Count || commentMap.ContainsKey(lineIndex))
        {
            if (commentMap.TryGetValue(lineIndex, out LocalizedComment comment))
            {
                string commentText = GetLocalizedText(comment.localizationKey);
                string fullComment = $"{comment.commentPrefix} {commentText}";
                result.Add(fullComment);
                isCommentLine.Add(true);
                Debug.Log($"Line {lineIndex}: Added comment '{fullComment}'");
            }
            else if (codeIndex < exercise.correctLines.Count)
            {
                result.Add(exercise.correctLines[codeIndex]);
                isCommentLine.Add(false);
                Debug.Log($"Line {lineIndex}: Added code '{exercise.correctLines[codeIndex]}'");
                codeIndex++;
            }

            lineIndex++;

            if (lineIndex > 100) break;
        }

        return result;
    }

    private string GetLocalizedText(string key)
    {
        if (LocalizationManager.Instance != null)
        {
            return LocalizationManager.Instance.GetText(key);
        }
        return key;
    }

    public void NextExercise()
    {
        if (lessons.Count == 0) return;

        Lesson currentLesson = lessons[currentLessonIndex];
        currentExerciseIndex++;

        if (currentExerciseIndex >= currentLesson.exercises.Count)
        {
            currentExerciseIndex = 0;
            NextLesson();
        }
        else
        {
            DisplayCurrentExercise();
        }
    }

    /// <summary>
    /// Called when the "Next Exercise" button on ConsolePanel is clicked.
    /// Hides ConsolePanel and SheetPanel, shows SlidePanel at its last position, and advances to next exercise.
    /// </summary>
    public void GoToNextExercise()
    {
        Debug.Log("[LessonManager] GoToNextExercise called");

        // Hide ConsolePanel
        if (consolePanel != null)
        {
            consolePanel.SetActive(false);
            Debug.Log("[LessonManager] ConsolePanel hidden");
        }

        // Hide SheetPanel
        if (sheetPanel != null)
        {
            sheetPanel.SetActive(false);
            Debug.Log("[LessonManager] SheetPanel hidden");
        }

        // Advance exercise index first to check if next exercise has slides
        if (lessons.Count == 0) return;
        Lesson currentLesson = lessons[currentLessonIndex];
        int nextExerciseIndex = currentExerciseIndex + 1;

        // Check if we're moving to next lesson
        if (nextExerciseIndex >= currentLesson.exercises.Count)
        {
            nextExerciseIndex = 0;
            int nextLessonIndex = (currentLessonIndex + 1) % lessons.Count;
            currentLesson = lessons[nextLessonIndex];
        }

        Exercise nextExercise = currentLesson.exercises[nextExerciseIndex];
        bool hasSlides = !string.IsNullOrEmpty(nextExercise.slideKeyPrefix) && nextExercise.slideCount > 0;

        if (hasSlides)
        {
            // Show SlidePanel at its last saved position
            if (slidePanel != null)
            {
                if (SlideManager.Instance != null)
                {
                    slidePanel.transform.position = SlideManager.Instance.LastSlidePanelPosition;
                    slidePanel.transform.rotation = SlideManager.Instance.LastSlidePanelRotation;
                    Debug.Log($"[LessonManager] SlidePanel shown at last saved position: {SlideManager.Instance.LastSlidePanelPosition}");
                }
                slidePanel.SetActive(true);
            }

            // Advance to next exercise (this calls SetSlideConfig which loads the new slide content)
            NextExercise();
        }
        else
        {
            // No slides for this exercise - show SheetPanel directly and spawn blocks
            Debug.Log("[LessonManager] Next exercise has no slides, showing SheetPanel directly");

            // Show SheetPanel at SlidePanel's last position
            if (sheetPanel != null && SlideManager.Instance != null)
            {
                sheetPanel.transform.position = SlideManager.Instance.LastSlidePanelPosition;
                sheetPanel.transform.rotation = SlideManager.Instance.LastSlidePanelRotation;
                sheetPanel.SetActive(true);
            }

            // Advance to next exercise
            NextExercise();

            // Spawn blocks for the first code line
            SpawnBlocksForFirstCodeLine();
        }
    }

    public void PreviousExercise()
    {
        if (lessons.Count == 0) return;

        currentExerciseIndex--;

        if (currentExerciseIndex < 0)
        {
            PreviousLesson();
            if (currentLessonIndex >= 0 && currentLessonIndex < lessons.Count)
            {
                currentExerciseIndex = lessons[currentLessonIndex].exercises.Count - 1;
            }
        }

        DisplayCurrentExercise();
    }

    public void NextLesson()
    {
        currentLessonIndex++;
        if (currentLessonIndex >= lessons.Count)
        {
            currentLessonIndex = 0;
        }
        currentExerciseIndex = 0;
        DisplayCurrentExercise();
    }

    public void PreviousLesson()
    {
        currentLessonIndex--;
        if (currentLessonIndex < 0)
        {
            currentLessonIndex = lessons.Count - 1;
        }
        currentExerciseIndex = 0;
        DisplayCurrentExercise();
    }

    public void SetLesson(int lessonIndex, int exerciseIndex = 0)
    {
        if (lessonIndex < 0 || lessonIndex >= lessons.Count) return;

        currentLessonIndex = lessonIndex;
        currentExerciseIndex = Mathf.Clamp(exerciseIndex, 0, lessons[lessonIndex].exercises.Count - 1);
        DisplayCurrentExercise();
    }

    public Lesson GetCurrentLesson()
    {
        if (currentLessonIndex >= 0 && currentLessonIndex < lessons.Count)
        {
            return lessons[currentLessonIndex];
        }
        return null;
    }

    public Exercise GetCurrentExercise()
    {
        Lesson lesson = GetCurrentLesson();
        if (lesson != null && currentExerciseIndex >= 0 && currentExerciseIndex < lesson.exercises.Count)
        {
            return lesson.exercises[currentExerciseIndex];
        }
        return null;
    }

    public string GetCurrentLessonTitle()
    {
        var lesson = GetCurrentLesson();
        if (lesson != null)
        {
            return GetLocalizedText(lesson.titleKey);
        }
        return "";
    }

    public string GetCurrentExerciseTitle()
    {
        var exercise = GetCurrentExercise();
        if (exercise != null)
        {
            return GetLocalizedText(exercise.titleKey);
        }
        return "";
    }

    public int CurrentLessonIndex => currentLessonIndex;
    public int CurrentExerciseIndex => currentExerciseIndex;
    public int TotalLessons => lessons.Count;
    public int TotalExercisesInCurrentLesson => GetCurrentLesson()?.exercises.Count ?? 0;

    #region Block Spawning

    /// <summary>
    /// Spawns blocks for the first code line of the current exercise
    /// </summary>
    public void SpawnBlocksForFirstCodeLine()
    {
        // Reset to first code line
        currentCodeLineIndex = 0;
        SpawnBlocksForCurrentCodeLine();
    }

    /// <summary>
    /// Spawns blocks for the current code line (based on currentCodeLineIndex)
    /// </summary>
    private void SpawnBlocksForCurrentCodeLine()
    {
        // Clear existing blocks
        ClearSpawnedBlocks();

        // Clear collected tokens
        collectedTokens.Clear();
        currentCodeTokens.Clear();

        // Find the display line index for the current code line
        currentDisplayLineIndex = GetDisplayLineIndexForCodeLine(currentCodeLineIndex);

        if (blockPrefab == null)
        {
            Debug.LogWarning("Block prefab is not assigned!");
            return;
        }

        // Get the current code line
        string codeLine = GetCodeLine(currentCodeLineIndex);
        if (string.IsNullOrEmpty(codeLine))
        {
            Debug.Log("[LessonManager] No more code lines - exercise complete!");
            return;
        }

        Debug.Log($"[LessonManager] Spawning blocks for code line {currentCodeLineIndex}: {codeLine}");

        // Split into tokens (words), excluding spaces
        List<string> tokens = SplitIntoTokens(codeLine);
        currentCodeTokens = new List<string>(tokens);

        // Reset block selection index
        currentBlockIndex = 0;

        // Clear the code line display initially
        UpdateCodeLineDisplay();

        // Spawn a block for each token
        for (int i = 0; i < tokens.Count; i++)
        {
            SpawnBlock(tokens[i], i, tokens.Count);
        }

        Debug.Log($"[LessonManager] Spawned {tokens.Count} blocks for code line {currentCodeLineIndex}");
    }

    /// <summary>
    /// Gets the display line index for a given code line index
    /// </summary>
    private int GetDisplayLineIndexForCodeLine(int codeLineIndex)
    {
        int codeLineCount = 0;
        for (int i = 0; i < isCommentLine.Count; i++)
        {
            if (!isCommentLine[i])
            {
                if (codeLineCount == codeLineIndex)
                {
                    return i;
                }
                codeLineCount++;
            }
        }
        return -1;
    }

    /// <summary>
    /// Gets the code line at the specified index
    /// </summary>
    private string GetCodeLine(int index)
    {
        Exercise exercise = GetCurrentExercise();
        if (exercise == null || index < 0 || index >= exercise.correctLines.Count)
        {
            return null;
        }

        return exercise.correctLines[index];
    }

    private List<string> SplitIntoTokens(string line)
    {
        List<string> tokens = new List<string>();

        // Regex to match: strings, words, numbers, operators/punctuation
        var regex = new Regex(@"""[^""]*""|'[^']*'|[a-zA-Z_][a-zA-Z0-9_]*|\d+|[^\s]");

        foreach (Match match in regex.Matches(line))
        {
            string token = match.Value;
            // Skip whitespace-only tokens
            if (!string.IsNullOrWhiteSpace(token))
            {
                tokens.Add(token);
            }
        }

        return tokens;
    }

    private void SpawnBlock(string tokenText, int index, int totalCount)
    {
        // Calculate spread position for this block, avoiding overlap with existing blocks
        Vector3 spawnPosition = CalculateNonOverlappingPosition(index, totalCount, tokenText);

        // Instantiate block
        GameObject block = Instantiate(blockPrefab, spawnPosition, Quaternion.identity, blockSpawnParent);
        block.name = $"Block_{index}_{tokenText}";

        // Find and set the TextMeshProUGUI text
        TextMeshProUGUI textComponent = block.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = tokenText;
        }
        else
        {
            Debug.LogWarning($"No TextMeshProUGUI found in block prefab for token: {tokenText}");
        }

        // Trigger scale/color update on TextBasedScaler if present
        TextBasedScaler scaler = block.GetComponent<TextBasedScaler>();
        if (scaler != null)
        {
            scaler.UpdateScales();
            scaler.UpdateColors();
        }

        // Set order index and LessonManager reference on BlockShooter
        BlockShooter shooter = block.GetComponent<BlockShooter>();
        if (shooter != null)
        {
            shooter.OrderIndex = index;
            shooter.TokenText = tokenText;
            shooter.LessonManager = this;
        }

        spawnedBlocks.Add(block);
    }

    /// <summary>
    /// Calculates a spread position for a block so blocks don't overlap
    /// Positions are distributed in a circle around 0.5m in front of the headset
    /// </summary>
    private Vector3 CalculateSpreadPosition(int index, int totalCount)
    {
        Transform headset = Camera.main?.transform;
        if (headset == null)
        {
            return spawnAreaCenter;
        }

        // Base position: 0.5m in front of headset
        Vector3 centerPosition = headset.position + headset.forward * 0.5f;

        // Spread radius based on number of blocks (minimum 0.08m, max 0.25m)
        float spreadRadius = Mathf.Clamp(0.06f + totalCount * 0.02f, 0.08f, 0.25f);

        // Calculate position on a circle with some randomness
        float angleStep = 360f / totalCount;
        float angle = angleStep * index;
        // Add slight random offset to angle to make it look more natural
        angle += UnityEngine.Random.Range(-angleStep * 0.3f, angleStep * 0.3f);
        float angleRad = angle * Mathf.Deg2Rad;

        // Calculate offset in headset's local space (right and up directions)
        Vector3 rightOffset = headset.right * Mathf.Cos(angleRad) * spreadRadius;
        Vector3 upOffset = headset.up * Mathf.Sin(angleRad) * spreadRadius;

        // Add small random depth variation (forward/back)
        float depthVariation = UnityEngine.Random.Range(-0.05f, 0.05f);
        Vector3 forwardOffset = headset.forward * depthVariation;

        Vector3 finalPosition = centerPosition + rightOffset + upOffset + forwardOffset;

        Debug.Log($"[LessonManager] Block {index}/{totalCount} spawned at angle {angle:F1}° radius {spreadRadius:F2}m");
        return finalPosition;
    }

    /// <summary>
    /// Calculates a spawn position that doesn't overlap with existing blocks
    /// </summary>
    private Vector3 CalculateNonOverlappingPosition(int index, int totalCount, string tokenText)
    {
        // Calculate base position
        Vector3 basePosition = CalculateSpreadPosition(index, totalCount);

        // Estimate block size based on token length (same formula as TextBasedScaler)
        float charCount = tokenText.Length;
        float scaledCharCount = charCount * (3f / 4f);
        scaledCharCount = Mathf.Max(scaledCharCount, 4f);
        float blockWidth = scaledCharCount * 0.05f + 0.02f; // cubeScaleX formula
        float blockHeight = 0.08f; // Approximate height

        // Minimum distance to avoid overlap (half of both blocks + small margin)
        float minDistance = blockWidth * 0.6f + 0.02f;

        // Check for overlap with existing blocks and adjust if needed
        int maxAttempts = 10;
        Vector3 adjustedPosition = basePosition;
        Transform headset = Camera.main?.transform;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            bool hasOverlap = false;
            Vector3 totalPushDirection = Vector3.zero;

            foreach (var existingBlock in spawnedBlocks)
            {
                if (existingBlock == null) continue;

                // Get existing block's size
                float existingWidth = 0.15f; // Default estimate
                TextBasedScaler scaler = existingBlock.GetComponent<TextBasedScaler>();
                if (scaler != null)
                {
                    Transform rayInteraction = existingBlock.transform.Find("RayInteraction");
                    if (rayInteraction != null)
                    {
                        existingWidth = rayInteraction.localScale.x + 0.02f;
                    }
                }

                float requiredDistance = (blockWidth + existingWidth) * 0.5f + 0.03f;
                Vector3 toExisting = existingBlock.transform.position - adjustedPosition;
                float distance = toExisting.magnitude;

                if (distance < requiredDistance)
                {
                    hasOverlap = true;
                    // Calculate push direction (away from existing block)
                    Vector3 pushDir = -toExisting.normalized;
                    if (pushDir.sqrMagnitude < 0.001f)
                    {
                        // If positions are nearly identical, push in random direction
                        pushDir = new Vector3(
                            UnityEngine.Random.Range(-1f, 1f),
                            UnityEngine.Random.Range(-1f, 1f),
                            UnityEngine.Random.Range(-0.3f, 0.3f)
                        ).normalized;
                    }
                    float pushStrength = requiredDistance - distance + 0.01f;
                    totalPushDirection += pushDir * pushStrength;
                }
            }

            if (!hasOverlap)
            {
                break; // No overlap, position is good
            }

            // Apply push and try again
            adjustedPosition += totalPushDirection;

            // Keep the block in front of headset (don't let it go behind)
            if (headset != null)
            {
                Vector3 toBlock = adjustedPosition - headset.position;
                float forwardDist = Vector3.Dot(toBlock, headset.forward);
                if (forwardDist < 0.3f)
                {
                    adjustedPosition = headset.position + headset.forward * 0.5f +
                        headset.right * Vector3.Dot(toBlock, headset.right) +
                        headset.up * Vector3.Dot(toBlock, headset.up);
                }
            }
        }

        return adjustedPosition;
    }

    /// <summary>
    /// Clears all spawned blocks
    /// </summary>
    public void ClearSpawnedBlocks()
    {
        foreach (var block in spawnedBlocks)
        {
            if (block != null)
            {
                Destroy(block);
            }
        }
        spawnedBlocks.Clear();
    }

    /// <summary>
    /// Gets the list of spawned blocks
    /// </summary>
    public List<GameObject> SpawnedBlocks => spawnedBlocks;

    /// <summary>
    /// Gets the current expected block index
    /// </summary>
    public int CurrentBlockIndex => currentBlockIndex;

    /// <summary>
    /// Tries to select a block. Returns true if the block is the correct next one in the sequence.
    /// </summary>
    public bool TrySelectBlock(int blockIndex)
    {
        if (blockIndex == currentBlockIndex)
        {
            currentBlockIndex++;
            Debug.Log($"[LessonManager] Block {blockIndex} selected correctly. Next expected: {currentBlockIndex}");

            // Check if all blocks have been selected (use currentCodeTokens.Count since spawnedBlocks gets modified)
            if (currentBlockIndex >= currentCodeTokens.Count)
            {
                Debug.Log("[LessonManager] All blocks selected in correct order!");
                OnAllBlocksSelected();
            }

            return true;
        }
        return false;
    }

    /// <summary>
    /// Called when all blocks have been selected in the correct order
    /// </summary>
    private void OnAllBlocksSelected()
    {
        // Reveal the next line (shows the completed code line)
        RevealNextLine();

        // Move to the next code line
        currentCodeLineIndex++;

        // Check if there are more code lines
        Exercise exercise = GetCurrentExercise();
        if (exercise != null && currentCodeLineIndex < exercise.correctLines.Count)
        {
            // Reveal the comment for the next code line (if any)
            RevealNextLine();

            // Spawn blocks for the next code line
            Debug.Log($"[LessonManager] Moving to code line {currentCodeLineIndex}");
            SpawnBlocksForCurrentCodeLine();
        }
        else
        {
            // All code lines completed
            Debug.Log("[LessonManager] Exercise completed! All code lines done.");
            ClearSpawnedBlocks();

            // Show ConsolePanel with expected output
            ShowConsolePanelWithOutput();
        }
    }

    /// <summary>
    /// Shows the ConsolePanel in front of the headset and displays expected output
    /// </summary>
    private void ShowConsolePanelWithOutput()
    {
        if (consolePanel == null)
        {
            Debug.LogWarning("[LessonManager] ConsolePanel is not assigned!");
            return;
        }

        // Position ConsolePanel 0.5m in front of the headset
        Transform headset = Camera.main?.transform;
        if (headset != null)
        {
            Vector3 panelPosition = headset.position + headset.forward * 0.5f;
            consolePanel.transform.position = panelPosition;

            // Make the panel face the headset
            consolePanel.transform.rotation = Quaternion.LookRotation(consolePanel.transform.position - headset.position);
        }

        // Activate the ConsolePanel
        consolePanel.SetActive(true);

        // Display expected output
        DisplayExpectedOutput();
    }

    /// <summary>
    /// Displays the expected output in the ConsolePanel's Instruction objects
    /// </summary>
    private void DisplayExpectedOutput()
    {
        Exercise exercise = GetCurrentExercise();
        if (exercise == null || consolePanelContent == null)
        {
            return;
        }

        // Find all Instruction objects under ConsolePanelContent
        List<TextMeshProUGUI> outputTexts = new List<TextMeshProUGUI>();
        for (int i = 0; i < consolePanelContent.childCount; i++)
        {
            Transform child = consolePanelContent.GetChild(i);
            Transform instruction = child.Find("Instruction");
            if (instruction != null)
            {
                TextMeshProUGUI tmp = instruction.GetComponent<TextMeshProUGUI>();
                if (tmp != null)
                {
                    outputTexts.Add(tmp);
                }
            }
        }

        // Display expected output lines
        for (int i = 0; i < outputTexts.Count; i++)
        {
            if (i < exercise.expectedOutput.Count)
            {
                outputTexts[i].text = exercise.expectedOutput[i];
                Debug.Log($"[LessonManager] Console output line {i}: {exercise.expectedOutput[i]}");
            }
            else
            {
                outputTexts[i].text = "";
            }
        }

        Debug.Log($"[LessonManager] Displayed {exercise.expectedOutput.Count} output lines in ConsolePanel");
    }

    /// <summary>
    /// Plays the block destroy sound
    /// </summary>
    public void PlayBlockDestroySound()
    {
        if (blockDestroySound != null)
        {
            blockDestroySound.Play();
        }
    }

    /// <summary>
    /// Removes a block from the spawned blocks list
    /// </summary>
    public void RemoveBlockFromList(GameObject block)
    {
        spawnedBlocks.Remove(block);
    }

    /// <summary>
    /// Adds a token to the collected list and updates the display
    /// </summary>
    public void AddCollectedToken(string token)
    {
        collectedTokens.Add(token);
        UpdateCodeLineDisplay();
        Debug.Log($"[LessonManager] Token collected: {token}, total collected: {collectedTokens.Count}/{currentCodeTokens.Count}");
    }

    /// <summary>
    /// Updates the code line display with collected tokens
    /// </summary>
    private void UpdateCodeLineDisplay()
    {
        if (currentDisplayLineIndex < 0 || currentDisplayLineIndex >= lineTexts.Count)
        {
            return;
        }

        // Build the display string with collected tokens
        StringBuilder displayText = new StringBuilder();
        var config = GetLanguageConfig(currentLanguage);

        for (int i = 0; i < collectedTokens.Count; i++)
        {
            string token = collectedTokens[i];

            // Get the color for this token
            string prevToken = i > 0 ? collectedTokens[i - 1] : null;
            string nextToken = i < collectedTokens.Count - 1 ? collectedTokens[i + 1] : null;

            var tokenType = GetTokenStyle(token, config, prevToken, nextToken);
            Color color = GetColorForTokenType(tokenType);
            string hexColor = ColorToHex(color);

            // Add space before token (except for opening brackets after function names, etc.)
            if (i > 0 && !ShouldOmitSpaceBefore(token, collectedTokens[i - 1]))
            {
                displayText.Append(" ");
            }

            displayText.Append($"<color={hexColor}>{token}</color>");
        }

        // Update the text
        lineTexts[currentDisplayLineIndex].text = displayText.ToString();
    }

    /// <summary>
    /// Determines if space should be omitted before a token
    /// </summary>
    private bool ShouldOmitSpaceBefore(string currentToken, string prevToken)
    {
        // No space before closing brackets/parens
        if (currentToken == ")" || currentToken == "]" || currentToken == "}" || currentToken == ",")
            return true;

        // No space after opening brackets/parens
        if (prevToken == "(" || prevToken == "[" || prevToken == "{")
            return true;

        return false;
    }

    /// <summary>
    /// Gets the world position of the first code line's center on the SheetPanel
    /// </summary>
    public Vector3 GetFirstCodeLinePosition()
    {
        // Find the first code line index
        for (int i = 0; i < isCommentLine.Count && i < lineTexts.Count; i++)
        {
            if (!isCommentLine[i])
            {
                // This is the first code line
                return lineTexts[i].transform.position;
            }
        }

        // Fallback to sheetPanelContent position if no code line found
        if (sheetPanelContent != null)
        {
            return sheetPanelContent.position;
        }

        return Vector3.zero;
    }

    /// <summary>
    /// Plays the wrong selection sound and respawns all blocks
    /// </summary>
    public void OnWrongSelection()
    {
        if (wrongSelectionSound != null)
        {
            wrongSelectionSound.Play();
        }

        // Respawn all blocks
        RespawnAllBlocks();
    }

    /// <summary>
    /// Clears all blocks and respawns them at random positions for the current code line
    /// </summary>
    public void RespawnAllBlocks()
    {
        // Clear existing blocks
        ClearSpawnedBlocks();

        // Respawn blocks for the current code line (not the first one)
        SpawnBlocksForCurrentCodeLine();

        // Skip init delay on all respawned blocks
        foreach (var block in spawnedBlocks)
        {
            RandomWalk randomWalk = block.GetComponent<RandomWalk>();
            if (randomWalk != null)
            {
                randomWalk.InitializeImmediately();
            }
        }

        Debug.Log($"[LessonManager] Blocks respawned for code line {currentCodeLineIndex}");
    }

    #endregion

    #region Syntax Highlighting

    private string ApplySyntaxHighlighting(string line)
    {
        if (string.IsNullOrEmpty(line)) return line;

        // Get language config from TextBasedScaler
        var config = GetLanguageConfig(currentLanguage);
        if (config == null) return line;

        // Check if it's a comment line
        if (!string.IsNullOrEmpty(config.CommentPrefix) && line.TrimStart().StartsWith(config.CommentPrefix))
        {
            string commentColor = ColorToHex(TextBasedScaler.SyntaxColors.Comment);
            return $"<color={commentColor}>{line}</color>";
        }

        // Tokenize and colorize
        StringBuilder result = new StringBuilder();
        string[] tokens = TokenizeLine(line);

        for (int i = 0; i < tokens.Length; i++)
        {
            string token = tokens[i];
            string prevToken = i > 0 ? tokens[i - 1].Trim() : null;
            string nextToken = i < tokens.Length - 1 ? tokens[i + 1].Trim() : null;

            // Preserve whitespace
            if (string.IsNullOrWhiteSpace(token))
            {
                result.Append(token);
                continue;
            }

            var tokenType = GetTokenStyle(token, config, prevToken, nextToken);
            Color color = GetColorForTokenType(tokenType);
            string hexColor = ColorToHex(color);

            result.Append($"<color={hexColor}>{token}</color>");
        }

        return result.ToString();
    }

    private string[] TokenizeLine(string line)
    {
        // Split by word boundaries while preserving delimiters and whitespace
        var tokens = new List<string>();
        var regex = new Regex(@"(\s+|""[^""]*""|'[^']*'|[a-zA-Z_][a-zA-Z0-9_]*|\d+|[^\s\w])");

        int lastIndex = 0;
        foreach (Match match in regex.Matches(line))
        {
            if (match.Index > lastIndex)
            {
                tokens.Add(line.Substring(lastIndex, match.Index - lastIndex));
            }
            tokens.Add(match.Value);
            lastIndex = match.Index + match.Length;
        }

        if (lastIndex < line.Length)
        {
            tokens.Add(line.Substring(lastIndex));
        }

        return tokens.ToArray();
    }

    private TextBasedScaler.LanguageConfig GetLanguageConfig(string lang)
    {
        if (TextBasedScaler.LanguageConfigs.TryGetValue(lang, out var config))
        {
            return config;
        }
        // Fallback to python if language not found
        if (TextBasedScaler.LanguageConfigs.TryGetValue("python", out var fallback))
        {
            return fallback;
        }
        return null;
    }

    private static readonly HashSet<string> DefKeywords = new HashSet<string>
    {
        "def", "function", "fn", "func", "fun", "sub",
        "defp", "defmodule", "defmacro",
        "class", "interface", "struct", "enum", "trait", "type",
        "object", "data", "newtype"
    };

    private TextBasedScaler.TokenType GetTokenStyle(string token, TextBasedScaler.LanguageConfig config, string prevToken, string nextToken)
    {
        if (string.IsNullOrEmpty(token)) return TextBasedScaler.TokenType.Foreground;

        // String
        if (token.StartsWith("\"") || token.StartsWith("'") || token.StartsWith("`"))
        {
            return TextBasedScaler.TokenType.String;
        }

        // Comment
        if (!string.IsNullOrEmpty(config.CommentPrefix) && token.StartsWith(config.CommentPrefix))
        {
            return TextBasedScaler.TokenType.Comment;
        }

        // Decorator
        if (token.StartsWith("@")) return TextBasedScaler.TokenType.Decorator;

        // Number
        if (Regex.IsMatch(token, @"^\d+$")) return TextBasedScaler.TokenType.Number;

        // Keywords
        if (config.Keywords.Contains(token)) return TextBasedScaler.TokenType.Keyword;

        // Built-ins
        if (config.Builtins.Contains(token)) return TextBasedScaler.TokenType.BuiltIn;

        // Constant (All uppercase)
        if (Regex.IsMatch(token, @"^[A-Z][A-Z0-9_]+$") && token.Length > 1)
        {
            return TextBasedScaler.TokenType.Constant;
        }

        // Function/Class Definition
        if (!string.IsNullOrEmpty(prevToken) && DefKeywords.Contains(prevToken))
        {
            return TextBasedScaler.TokenType.FunctionDef;
        }

        // Brackets
        if (Regex.IsMatch(token, @"^[()[\]{}]$"))
        {
            return TextBasedScaler.TokenType.Bracket;
        }

        // Variables/Identifiers
        if (Regex.IsMatch(token, @"^[a-zA-Z_][a-zA-Z0-9_]*$"))
        {
            return TextBasedScaler.TokenType.Variable;
        }

        return TextBasedScaler.TokenType.Foreground;
    }

    private Color GetColorForTokenType(TextBasedScaler.TokenType type)
    {
        switch (type)
        {
            case TextBasedScaler.TokenType.Keyword: return TextBasedScaler.SyntaxColors.Keyword;
            case TextBasedScaler.TokenType.String: return TextBasedScaler.SyntaxColors.String;
            case TextBasedScaler.TokenType.Number: return TextBasedScaler.SyntaxColors.Number;
            case TextBasedScaler.TokenType.Comment: return TextBasedScaler.SyntaxColors.Comment;
            case TextBasedScaler.TokenType.DocComment: return TextBasedScaler.SyntaxColors.DocComment;
            case TextBasedScaler.TokenType.Constant: return TextBasedScaler.SyntaxColors.Constant;
            case TextBasedScaler.TokenType.FunctionDef: return TextBasedScaler.SyntaxColors.FunctionDef;
            case TextBasedScaler.TokenType.Decorator: return TextBasedScaler.SyntaxColors.Decorator;
            case TextBasedScaler.TokenType.BuiltIn: return TextBasedScaler.SyntaxColors.BuiltIn;
            case TextBasedScaler.TokenType.Variable: return TextBasedScaler.SyntaxColors.Variable;
            case TextBasedScaler.TokenType.Bracket: return TextBasedScaler.SyntaxColors.Bracket;
            default: return TextBasedScaler.SyntaxColors.Foreground;
        }
    }

    private string ColorToHex(Color color)
    {
        return $"#{ColorUtility.ToHtmlStringRGB(color)}";
    }

    #endregion
}
