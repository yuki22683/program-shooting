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

    /// <summary>
    /// Initialize JavaScript lessons from senkou-code data
    /// </summary>
    private void InitializeJavaScriptLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: JavaScript I - はじめてのJavaScript ====================
        var lesson1 = new Lesson { titleKey = "javascript_lesson1_title" };

        // Ex1: Hello, JavaScript!
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson1_ex1_title",
            slideKeyPrefix = "javascript_lesson1_ex1",
            slideCount = 2,
            correctLines = new List<string> { "console.log('Hello, JavaScript!');" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson1_ex1_comment1" }
            },
            expectedOutput = new List<string> { "Hello, JavaScript!" }
        });

        // Ex2: 変数を使おう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson1_ex2_title",
            slideKeyPrefix = "javascript_lesson1_ex2",
            slideCount = 2,
            correctLines = new List<string> { "const name = 'JavaScript';", "console.log(name);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson1_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson1_ex2_comment2" }
            },
            expectedOutput = new List<string> { "JavaScript" }
        });

        // Ex3: 計算しよう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson1_ex3_title",
            slideKeyPrefix = "javascript_lesson1_ex3",
            slideCount = 2,
            correctLines = new List<string> { "const x = 10;", "const y = 5;", "console.log(x + y);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson1_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson1_ex3_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson1_ex3_comment3" }
            },
            expectedOutput = new List<string> { "15" }
        });

        // Ex4: テンプレートリテラル
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson1_ex4_title",
            slideKeyPrefix = "javascript_lesson1_ex4",
            slideCount = 2,
            correctLines = new List<string> { "const age = 10;", "console.log(`私は${age}歳です`);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson1_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson1_ex4_comment2" }
            },
            expectedOutput = new List<string> { "私は10歳です" }
        });

        // Ex5: 配列
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson1_ex5_title",
            slideKeyPrefix = "javascript_lesson1_ex5",
            slideCount = 2,
            correctLines = new List<string> { "const colors = ['あか', 'あお'];", "console.log(colors[1]);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson1_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson1_ex5_comment2" }
            },
            expectedOutput = new List<string> { "あお" }
        });

        // Ex6: if文
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson1_ex6_title",
            slideKeyPrefix = "javascript_lesson1_ex6",
            slideCount = 2,
            correctLines = new List<string> { "const score = 100;", "if (score > 80) {", "    console.log('ごうかく！');", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson1_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson1_ex6_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson1_ex6_comment3" }
            },
            expectedOutput = new List<string> { "ごうかく！" }
        });

        // Ex7: if-else文
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson1_ex7_title",
            slideKeyPrefix = "javascript_lesson1_ex7",
            slideCount = 2,
            correctLines = new List<string> { "const age = 10;", "if (age >= 20) {", "    console.log('おとな');", "} else {", "    console.log('こども');", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson1_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson1_ex7_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "javascript_lesson1_ex7_comment3" }
            },
            expectedOutput = new List<string> { "こども" }
        });

        // Ex8: for-of文
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson1_ex8_title",
            slideKeyPrefix = "javascript_lesson1_ex8",
            slideCount = 2,
            correctLines = new List<string> { "const names = ['たろう', 'はなこ'];", "for (const name of names) {", "    console.log(name);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson1_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson1_ex8_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson1_ex8_comment3" }
            },
            expectedOutput = new List<string> { "たろう", "はなこ" }
        });

        // Ex9: オブジェクト
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson1_ex9_title",
            slideKeyPrefix = "javascript_lesson1_ex9",
            slideCount = 2,
            correctLines = new List<string> { "const user = { name: 'たろう' };", "console.log(user.name);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson1_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson1_ex9_comment2" }
            },
            expectedOutput = new List<string> { "たろう" }
        });

        // Ex10: 関数
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson1_ex10_title",
            slideKeyPrefix = "javascript_lesson1_ex10",
            slideCount = 2,
            correctLines = new List<string> { "function greet() {", "    console.log('こんにちは');", "}", "greet();" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson1_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson1_ex10_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "javascript_lesson1_ex10_comment3" }
            },
            expectedOutput = new List<string> { "こんにちは" }
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: JavaScript II - モダンな書き方 ====================
        var lesson2 = new Lesson { titleKey = "javascript_lesson2_title" };

        // Ex1: アロー関数
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson2_ex1_title",
            slideKeyPrefix = "javascript_lesson2_ex1",
            slideCount = 2,
            correctLines = new List<string> { "const square = x => x * x;", "", "console.log(square(5));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson2_ex1_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "javascript_lesson2_ex1_comment2" }
            },
            expectedOutput = new List<string> { "25" }
        });

        // Ex2: mapメソッド
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson2_ex2_title",
            slideKeyPrefix = "javascript_lesson2_ex2",
            slideCount = 2,
            correctLines = new List<string> { "const nums = [1, 2, 3];", "const tripled = nums.map(n => n * 3);", "console.log(tripled);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson2_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson2_ex2_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson2_ex2_comment3" }
            },
            expectedOutput = new List<string> { "[3, 6, 9]" }
        });

        // Ex3: 剰余演算子
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson2_ex3_title",
            slideKeyPrefix = "javascript_lesson2_ex3",
            slideCount = 2,
            correctLines = new List<string> { "const remainder = 10 % 3;", "console.log(remainder);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson2_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson2_ex3_comment2" }
            },
            expectedOutput = new List<string> { "1" }
        });

        // Ex4: 複合代入演算子
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson2_ex4_title",
            slideKeyPrefix = "javascript_lesson2_ex4",
            slideCount = 2,
            correctLines = new List<string> { "let total = 0;", "total += 10;", "total += 5;", "console.log(total);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson2_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson2_ex4_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson2_ex4_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "javascript_lesson2_ex4_comment4" }
            },
            expectedOutput = new List<string> { "15" }
        });

        // Ex5: 論理演算子
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson2_ex5_title",
            slideKeyPrefix = "javascript_lesson2_ex5",
            slideCount = 2,
            correctLines = new List<string> { "const age = 25;", "if (age >= 20 && age < 30) {", "    console.log('20代です');", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson2_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson2_ex5_comment2" }
            },
            expectedOutput = new List<string> { "20代です" }
        });

        // Ex6: filterメソッド
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson2_ex6_title",
            slideKeyPrefix = "javascript_lesson2_ex6",
            slideCount = 2,
            correctLines = new List<string> { "const nums = [5, 15, 8, 20];", "const big = nums.filter(n => n > 10);", "console.log(big);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson2_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson2_ex6_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson2_ex6_comment3" }
            },
            expectedOutput = new List<string> { "[15, 20]" }
        });

        // Ex7: reduceメソッド
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson2_ex7_title",
            slideKeyPrefix = "javascript_lesson2_ex7",
            slideCount = 2,
            correctLines = new List<string> { "const nums = [10, 20, 30];", "const total = nums.reduce((acc, n) => acc + n, 0);", "console.log(total);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson2_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson2_ex7_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson2_ex7_comment3" }
            },
            expectedOutput = new List<string> { "60" }
        });

        // Ex8: 分割代入（配列）
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson2_ex8_title",
            slideKeyPrefix = "javascript_lesson2_ex8",
            slideCount = 2,
            correctLines = new List<string> { "const colors = ['red', 'green', 'blue'];", "const [first, second] = colors;", "console.log(first);", "console.log(second);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson2_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson2_ex8_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson2_ex8_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "javascript_lesson2_ex8_comment4" }
            },
            expectedOutput = new List<string> { "red", "green" }
        });

        // Ex9: 分割代入（オブジェクト）
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson2_ex9_title",
            slideKeyPrefix = "javascript_lesson2_ex9",
            slideCount = 2,
            correctLines = new List<string> { "const user = { name: 'Alice', score: 100 };", "const { name, score } = user;", "console.log(name);", "console.log(score);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson2_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson2_ex9_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson2_ex9_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "javascript_lesson2_ex9_comment4" }
            },
            expectedOutput = new List<string> { "Alice", "100" }
        });

        // Ex10: スプレッド演算子
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson2_ex10_title",
            slideKeyPrefix = "javascript_lesson2_ex10",
            slideCount = 2,
            correctLines = new List<string> { "const arr1 = [1, 2];", "const arr2 = [3, 4];", "const merged = [...arr1, ...arr2];", "console.log(merged);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson2_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson2_ex10_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson2_ex10_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "javascript_lesson2_ex10_comment4" }
            },
            expectedOutput = new List<string> { "[1, 2, 3, 4]" }
        });

        // Ex11: 三項演算子
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson2_ex11_title",
            slideKeyPrefix = "javascript_lesson2_ex11",
            slideCount = 2,
            correctLines = new List<string> { "const num = 5;", "const sign = num >= 0 ? 'positive' : 'negative';", "console.log(sign);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson2_ex11_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson2_ex11_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson2_ex11_comment3" }
            },
            expectedOutput = new List<string> { "positive" }
        });

        // Ex12: findメソッド
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson2_ex12_title",
            slideKeyPrefix = "javascript_lesson2_ex12",
            slideCount = 2,
            correctLines = new List<string> { "const numbers = [1, 3, 4, 7, 8];", "const firstEven = numbers.find(n => n % 2 === 0);", "console.log(firstEven);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson2_ex12_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson2_ex12_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson2_ex12_comment3" }
            },
            expectedOutput = new List<string> { "4" }
        });

        // Ex13: プロパティショートハンド
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson2_ex13_title",
            slideKeyPrefix = "javascript_lesson2_ex13",
            slideCount = 2,
            correctLines = new List<string> { "const x = 10;", "const y = 20;", "const point = { x, y };", "console.log(point.x);", "console.log(point.y);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson2_ex13_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson2_ex13_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson2_ex13_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "javascript_lesson2_ex13_comment4" }
            },
            expectedOutput = new List<string> { "10", "20" }
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: JavaScript III - 非同期とクラス ====================
        var lesson3 = new Lesson { titleKey = "javascript_lesson3_title" };

        // Ex1: Promise
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson3_ex1_title",
            slideKeyPrefix = "javascript_lesson3_ex1",
            slideCount = 2,
            correctLines = new List<string> { "const p = new Promise((resolve) => {", "  resolve('Hello Promise!');", "});", "", "p.then(msg => console.log(msg));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson3_ex1_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson3_ex1_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "javascript_lesson3_ex1_comment3" }
            },
            expectedOutput = new List<string> { "Hello Promise!" }
        });

        // Ex2: Promise.resolve
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson3_ex2_title",
            slideKeyPrefix = "javascript_lesson3_ex2",
            slideCount = 2,
            correctLines = new List<string> { "Promise.resolve(42)", "  .then(n => console.log(n * 2));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson3_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson3_ex2_comment2" }
            },
            expectedOutput = new List<string> { "84" }
        });

        // Ex3: async関数
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson3_ex3_title",
            slideKeyPrefix = "javascript_lesson3_ex3",
            slideCount = 2,
            correctLines = new List<string> { "async function getMessage() {", "  return 'Async works!';", "}", "", "getMessage().then(msg => console.log(msg));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson3_ex3_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson3_ex3_comment2" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "javascript_lesson3_ex3_comment3" }
            },
            expectedOutput = new List<string> { "Async works!" }
        });

        // Ex4: await
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson3_ex4_title",
            slideKeyPrefix = "javascript_lesson3_ex4",
            slideCount = 2,
            correctLines = new List<string> { "async function main() {", "  const value = await Promise.resolve(100);", "  console.log(value);", "}", "", "main();" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson3_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson3_ex4_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson3_ex4_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "javascript_lesson3_ex4_comment4" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "javascript_lesson3_ex4_comment5" }
            },
            expectedOutput = new List<string> { "100" }
        });

        // Ex5: Promise.all
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson3_ex5_title",
            slideKeyPrefix = "javascript_lesson3_ex5",
            slideCount = 2,
            correctLines = new List<string> { "const p1 = Promise.resolve(10);", "const p2 = Promise.resolve(20);", "Promise.all([p1, p2]).then(nums => {", "  console.log(nums[0] + nums[1]);", "});" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson3_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson3_ex5_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson3_ex5_comment3" }
            },
            expectedOutput = new List<string> { "30" }
        });

        // Ex6: class
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson3_ex6_title",
            slideKeyPrefix = "javascript_lesson3_ex6",
            slideCount = 2,
            correctLines = new List<string> { "class Dog {", "  constructor(name) {", "    this.name = name;", "  }", "  bark() {", "    console.log(`${this.name}: Woof!`);", "  }", "}", "", "const dog = new Dog('Pochi');", "dog.bark();" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson3_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson3_ex6_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "javascript_lesson3_ex6_comment3" },
                new LocalizedComment { lineIndex = 12, commentPrefix = "//", localizationKey = "javascript_lesson3_ex6_comment4" },
                new LocalizedComment { lineIndex = 14, commentPrefix = "//", localizationKey = "javascript_lesson3_ex6_comment5" },
                new LocalizedComment { lineIndex = 16, commentPrefix = "//", localizationKey = "javascript_lesson3_ex6_comment6" }
            },
            expectedOutput = new List<string> { "Pochi: Woof!" }
        });

        // Ex7: 継承
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson3_ex7_title",
            slideKeyPrefix = "javascript_lesson3_ex7",
            slideCount = 2,
            correctLines = new List<string> { "class Animal {", "  speak() { console.log('...'); }", "}", "", "class Cat extends Animal {", "  speak() { console.log('Meow!'); }", "}", "", "const cat = new Cat();", "cat.speak();" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson3_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson3_ex7_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "javascript_lesson3_ex7_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "javascript_lesson3_ex7_comment4" },
                new LocalizedComment { lineIndex = 12, commentPrefix = "//", localizationKey = "javascript_lesson3_ex7_comment5" },
                new LocalizedComment { lineIndex = 14, commentPrefix = "//", localizationKey = "javascript_lesson3_ex7_comment6" },
                new LocalizedComment { lineIndex = 16, commentPrefix = "//", localizationKey = "javascript_lesson3_ex7_comment7" }
            },
            expectedOutput = new List<string> { "Meow!" }
        });

        // Ex8: staticメソッド
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson3_ex8_title",
            slideKeyPrefix = "javascript_lesson3_ex8",
            slideCount = 2,
            correctLines = new List<string> { "class Calculator {", "  static multiply(a, b) {", "    return a * b;", "  }", "}", "", "console.log(Calculator.multiply(3, 4));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson3_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson3_ex8_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson3_ex8_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "javascript_lesson3_ex8_comment4" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "javascript_lesson3_ex8_comment5" }
            },
            expectedOutput = new List<string> { "12" }
        });

        // Ex9: オプショナルチェイニング
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson3_ex9_title",
            slideKeyPrefix = "javascript_lesson3_ex9",
            slideCount = 2,
            correctLines = new List<string> { "const data = { user: { name: 'Bob' } };", "console.log(data?.user?.name);", "console.log(data?.profile?.age);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson3_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson3_ex9_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson3_ex9_comment3" }
            },
            expectedOutput = new List<string> { "Bob", "undefined" }
        });

        // Ex10: Null合体演算子
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson3_ex10_title",
            slideKeyPrefix = "javascript_lesson3_ex10",
            slideCount = 2,
            correctLines = new List<string> { "const value = undefined;", "const result = value ?? 'default';", "console.log(result);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson3_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson3_ex10_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson3_ex10_comment3" }
            },
            expectedOutput = new List<string> { "default" }
        });

        lessons.Add(lesson3);

        // ==================== LESSON 4: JavaScript IV - 関数型プログラミング ====================
        var lesson4 = new Lesson { titleKey = "javascript_lesson4_title" };

        // Ex1: every
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson4_ex1_title",
            slideKeyPrefix = "javascript_lesson4_ex1",
            slideCount = 1,
            correctLines = new List<string> { "const scores = [80, 90, 75, 85];", "const allPassed = scores.every(score => score >= 60);", "console.log(allPassed);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson4_ex1_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson4_ex1_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson4_ex1_comment3" }
            },
            expectedOutput = new List<string> { "true" }
        });

        // Ex2: some
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson4_ex2_title",
            slideKeyPrefix = "javascript_lesson4_ex2",
            slideCount = 1,
            correctLines = new List<string> { "const ages = [15, 22, 17, 19];", "const hasAdult = ages.some(age => age >= 20);", "console.log(hasAdult);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson4_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson4_ex2_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson4_ex2_comment3" }
            },
            expectedOutput = new List<string> { "true" }
        });

        // Ex3: flatMap
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson4_ex3_title",
            slideKeyPrefix = "javascript_lesson4_ex3",
            slideCount = 1,
            correctLines = new List<string> { "const words = ['hello', 'world'];", "const chars = words.flatMap(w => w.split(''));", "console.log(chars);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson4_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson4_ex3_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson4_ex3_comment3" }
            },
            expectedOutput = new List<string> { "['h', 'e', 'l', 'l', 'o', 'w', 'o', 'r', 'l', 'd']" }
        });

        // Ex4: Object.entries
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson4_ex4_title",
            slideKeyPrefix = "javascript_lesson4_ex4",
            slideCount = 1,
            correctLines = new List<string> { "const user = { name: 'Taro', age: 25 };", "for (const [key, value] of Object.entries(user)) {", "  console.log(`${key}: ${value}`);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson4_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson4_ex4_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson4_ex4_comment3" }
            },
            expectedOutput = new List<string> { "name: Taro", "age: 25" }
        });

        // Ex5: Object.fromEntries
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson4_ex5_title",
            slideKeyPrefix = "javascript_lesson4_ex5",
            slideCount = 1,
            correctLines = new List<string> { "const pairs = [['name', 'Python'], ['version', '3.12']];", "const obj = Object.fromEntries(pairs);", "console.log(obj);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson4_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson4_ex5_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson4_ex5_comment3" }
            },
            expectedOutput = new List<string> { "{ name: 'Python', version: '3.12' }" }
        });

        // Ex6: カリー化
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson4_ex6_title",
            slideKeyPrefix = "javascript_lesson4_ex6",
            slideCount = 1,
            correctLines = new List<string> { "const multiply = a => b => a * b;", "const double = multiply(2);", "console.log(double(5));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson4_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson4_ex6_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson4_ex6_comment3" }
            },
            expectedOutput = new List<string> { "10" }
        });

        // Ex7: 関数合成
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson4_ex7_title",
            slideKeyPrefix = "javascript_lesson4_ex7",
            slideCount = 1,
            correctLines = new List<string> { "const compose = (f, g) => x => f(g(x));", "const square = x => x * x;", "const negate = x => -x;", "const squareThenNegate = compose(negate, square);", "console.log(squareThenNegate(3));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson4_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson4_ex7_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson4_ex7_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "javascript_lesson4_ex7_comment4" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "javascript_lesson4_ex7_comment5" }
            },
            expectedOutput = new List<string> { "-9" }
        });

        // Ex8: クロージャ
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson4_ex8_title",
            slideKeyPrefix = "javascript_lesson4_ex8",
            slideCount = 1,
            correctLines = new List<string> { "function createCounter() {", "  let count = 0;", "  return () => ++count;", "}", "const counter = createCounter();", "console.log(counter());", "console.log(counter());", "console.log(counter());" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson4_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson4_ex8_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson4_ex8_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "javascript_lesson4_ex8_comment4" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "javascript_lesson4_ex8_comment5" }
            },
            expectedOutput = new List<string> { "1", "2", "3" }
        });

        // Ex9: メモ化
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson4_ex9_title",
            slideKeyPrefix = "javascript_lesson4_ex9",
            slideCount = 1,
            correctLines = new List<string> { "function memoize(fn) {", "  const cache = {};", "  return (x) => {", "    if (!(x in cache)) {", "      cache[x] = fn(x);", "    }", "    return cache[x];", "  };", "}", "const square = memoize(x => x * x);", "console.log(square(5));", "console.log(square(5));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson4_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson4_ex9_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "javascript_lesson4_ex9_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "javascript_lesson4_ex9_comment4" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "javascript_lesson4_ex9_comment5" },
                new LocalizedComment { lineIndex = 12, commentPrefix = "//", localizationKey = "javascript_lesson4_ex9_comment6" },
                new LocalizedComment { lineIndex = 16, commentPrefix = "//", localizationKey = "javascript_lesson4_ex9_comment7" }
            },
            expectedOutput = new List<string> { "25", "25" }
        });

        // Ex10: パイプライン
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson4_ex10_title",
            slideKeyPrefix = "javascript_lesson4_ex10",
            slideCount = 1,
            correctLines = new List<string> { "const pipe = (...fns) => x =>", "  fns.reduce((v, f) => f(v), x);", "", "const process = pipe(", "  x => x + 1,", "  x => x * 2,", "  x => x - 3", ");", "console.log(process(5));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson4_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "javascript_lesson4_ex10_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "javascript_lesson4_ex10_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "javascript_lesson4_ex10_comment4" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "javascript_lesson4_ex10_comment5" },
                new LocalizedComment { lineIndex = 12, commentPrefix = "//", localizationKey = "javascript_lesson4_ex10_comment6" },
                new LocalizedComment { lineIndex = 16, commentPrefix = "//", localizationKey = "javascript_lesson4_ex10_comment7" }
            },
            expectedOutput = new List<string> { "9" }
        });

        lessons.Add(lesson4);

        // ==================== LESSON 5: JavaScript V - 正規表現とエラー処理 ====================
        var lesson5 = new Lesson { titleKey = "javascript_lesson5_title" };

        // Ex1: 正規表現リテラル
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson5_ex1_title",
            slideKeyPrefix = "javascript_lesson5_ex1",
            slideCount = 1,
            correctLines = new List<string> { "const pattern = /javascript/i;", "console.log(pattern.test('I love JavaScript'));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson5_ex1_comment1" }
            },
            expectedOutput = new List<string> { "true" }
        });

        // Ex2: test()
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson5_ex2_title",
            slideKeyPrefix = "javascript_lesson5_ex2",
            slideCount = 1,
            correctLines = new List<string> { "const emailPattern = /\\w+@\\w+\\.\\w+/;", "console.log(emailPattern.test('test@example.com'));", "console.log(emailPattern.test('invalid-email'));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson5_ex2_comment1" }
            },
            expectedOutput = new List<string> { "true", "false" }
        });

        // Ex3: match()
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson5_ex3_title",
            slideKeyPrefix = "javascript_lesson5_ex3",
            slideCount = 1,
            correctLines = new List<string> { "const text = 'Contact: 090-1234-5678 or 080-9876-5432';", "const phones = text.match(/\\d{3}-\\d{4}-\\d{4}/g);", "console.log(phones);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson5_ex3_comment1" }
            },
            expectedOutput = new List<string> { "['090-1234-5678', '080-9876-5432']" }
        });

        // Ex4: replace()
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson5_ex4_title",
            slideKeyPrefix = "javascript_lesson5_ex4",
            slideCount = 1,
            correctLines = new List<string> { "const text = 'Hello   World   JavaScript';", "const result = text.replace(/\\s+/g, ' ');", "console.log(result);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson5_ex4_comment1" }
            },
            expectedOutput = new List<string> { "Hello World JavaScript" }
        });

        // Ex5: try-catch
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson5_ex5_title",
            slideKeyPrefix = "javascript_lesson5_ex5",
            slideCount = 1,
            correctLines = new List<string> { "try {", "  const result = JSON.parse('invalid json');", "} catch (e) {", "  console.log('Error:', e.message);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson5_ex5_comment1" }
            },
            expectedOutput = new List<string> { "Error: Unexpected token i in JSON at position 0" }
        });

        // Ex6: finally
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson5_ex6_title",
            slideKeyPrefix = "javascript_lesson5_ex6",
            slideCount = 1,
            correctLines = new List<string> { "function process() {", "  try {", "    console.log('Processing...');", "    throw new Error('Error!');", "  } catch (e) {", "    console.log('Caught:', e.message);", "  } finally {", "    console.log('Cleanup done');", "  }", "}", "process();" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson5_ex6_comment1" }
            },
            expectedOutput = new List<string> { "Processing...", "Caught: Error!", "Cleanup done" }
        });

        // Ex7: カスタムエラー
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson5_ex7_title",
            slideKeyPrefix = "javascript_lesson5_ex7",
            slideCount = 1,
            correctLines = new List<string> { "class ValidationError extends Error {", "  constructor(message) {", "    super(message);", "    this.name = 'ValidationError';", "  }", "}", "", "try {", "  throw new ValidationError('Invalid input');", "} catch (e) {", "  console.log(`${e.name}: ${e.message}`);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson5_ex7_comment1" }
            },
            expectedOutput = new List<string> { "ValidationError: Invalid input" }
        });

        // Ex8: Symbol
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson5_ex8_title",
            slideKeyPrefix = "javascript_lesson5_ex8",
            slideCount = 1,
            correctLines = new List<string> { "const secret = Symbol('secret');", "const user = {", "  name: 'Taro',", "  [secret]: 'password123'", "};", "console.log(user.name);", "console.log(user[secret]);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson5_ex8_comment1" }
            },
            expectedOutput = new List<string> { "Taro", "password123" }
        });

        // Ex9: WeakMap
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson5_ex9_title",
            slideKeyPrefix = "javascript_lesson5_ex9",
            slideCount = 1,
            correctLines = new List<string> { "const privateData = new WeakMap();", "", "class User {", "  constructor(name) {", "    privateData.set(this, { password: 'secret' });", "    this.name = name;", "  }", "  getPassword() {", "    return privateData.get(this).password;", "  }", "}", "", "const user = new User('Taro');", "console.log(user.getPassword());" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson5_ex9_comment1" }
            },
            expectedOutput = new List<string> { "secret" }
        });

        // Ex10: Proxy
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "javascript_lesson5_ex10_title",
            slideKeyPrefix = "javascript_lesson5_ex10",
            slideCount = 1,
            correctLines = new List<string> { "const handler = {", "  get(target, prop) {", "    console.log(`Getting ${prop}`);", "    return target[prop];", "  }", "};", "", "const user = new Proxy({ name: 'Taro' }, handler);", "console.log(user.name);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "javascript_lesson5_ex10_comment1" }
            },
            expectedOutput = new List<string> { "Getting name", "Taro" }
        });

        lessons.Add(lesson5);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;

        Debug.Log($"[LessonManager] Initialized {lessons.Count} JavaScript lessons");
    }

    /// <summary>
    /// Initialize Java lessons from senkou-code data
    /// </summary>
    private void InitializeJavaLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: Java I ====================
        var lesson1 = new Lesson { titleKey = "java_lesson1_title" };

        // Exercise 1: 画面にメッセージを出しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "java_lesson1_ex1_title",
            slideKeyPrefix = "java_lesson1_ex1",
            slideCount = 2,
            correctLines = new List<string> { "public class Main {", "    public static void main(String[] args) {", "        // Hello, Java! と出力する", "        System.out.println(\"Hello, Java!\");", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "java_lesson1_ex1_comment1" }
            },
            expectedOutput = new List<string> { "Hello, Java!" }
        });

        // Exercise 2: 文字を入れる「はこ」String
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "java_lesson1_ex2_title",
            slideKeyPrefix = "java_lesson1_ex2",
            slideCount = 2,
            correctLines = new List<string> { "public class Main {", "    public static void main(String[] args) {", "        // 文字列 Java を代入する", "        String name = \"Java\";", "        // 変数 name を出力する", "        System.out.println(name);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "java_lesson1_ex2_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "java_lesson1_ex2_comment2" }
            },
            expectedOutput = new List<string> { "Java" }
        });

        // Exercise 3: 数字を入れる「はこ」int
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "java_lesson1_ex3_title",
            slideKeyPrefix = "java_lesson1_ex3",
            slideCount = 2,
            correctLines = new List<string> { "public class Main {", "    public static void main(String[] args) {", "        // x に 10 を代入する", "        int x = 10;", "        // y に 5 を代入する", "        int y = 5;", "        // + でたし算した答えを出す", "        System.out.println(x + y);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "java_lesson1_ex3_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "java_lesson1_ex3_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "java_lesson1_ex3_comment3" }
            },
            expectedOutput = new List<string> { "15" }
        });

        // Exercise 4: 剰余演算子（%）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "java_lesson1_ex4_title",
            slideKeyPrefix = "java_lesson1_ex4",
            slideCount = 2,
            correctLines = new List<string> { "public class Main {", "    public static void main(String[] args) {", "        // 10 を 3 で割ったあまりを出力する", "        System.out.println(10 % 3);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "java_lesson1_ex4_comment1" }
            },
            expectedOutput = new List<string> { "1" }
        });

        // Exercise 5: 累算代入演算子（+=、-=）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "java_lesson1_ex5_title",
            slideKeyPrefix = "java_lesson1_ex5",
            slideCount = 2,
            correctLines = new List<string> { "public class Main {", "    public static void main(String[] args) {", "        // hp に 100 を入れる", "        int hp = 100;", "        // += で 20 を足す", "        hp += 20;", "        // -= で 50 を引く", "        hp -= 50;", "        System.out.println(hp);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "java_lesson1_ex5_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "java_lesson1_ex5_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "java_lesson1_ex5_comment3" }
            },
            expectedOutput = new List<string> { "70" }
        });

        // Exercise 6: 文字と「はこ」をくっつけましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "java_lesson1_ex6_title",
            slideKeyPrefix = "java_lesson1_ex6",
            slideCount = 2,
            correctLines = new List<string> { "public class Main {", "    public static void main(String[] args) {", "        // age というはこに 10 を入れる", "        int age = 10;", "        // 変数 age をくっつけて表示する", "        System.out.println(\"私は\" + age + \"歳です\");", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "java_lesson1_ex6_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "java_lesson1_ex6_comment2" }
            },
            expectedOutput = new List<string> { "私は10歳です" }
        });

        // Exercise 7: データをならべる「配列」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "java_lesson1_ex7_title",
            slideKeyPrefix = "java_lesson1_ex7",
            slideCount = 2,
            correctLines = new List<string> { "public class Main {", "    public static void main(String[] args) {", "        // \"あか\", \"あお\" の順で配列を作る", "        String[] colors = {\"あか\", \"あお\"};", "        // 添字 1 で2番目を出す", "        System.out.println(colors[1]);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "java_lesson1_ex7_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "java_lesson1_ex7_comment2" }
            },
            expectedOutput = new List<string> { "あお" }
        });

        // Exercise 8: 「もし〜なら」で分ける if文
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "java_lesson1_ex8_title",
            slideKeyPrefix = "java_lesson1_ex8",
            slideCount = 2,
            correctLines = new List<string> { "public class Main {", "    public static void main(String[] args) {", "        // score に 100 を入れる", "        int score = 100;", "        // > で80より大きいか比較する", "        if (score > 80) {", "            // ごうかく！ と表示する", "            System.out.println(\"ごうかく！\");", "        }", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "java_lesson1_ex8_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "java_lesson1_ex8_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "java_lesson1_ex8_comment3" }
            },
            expectedOutput = new List<string> { "ごうかく！" }
        });

        // Exercise 9: ちがう場合は？ if-else文
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "java_lesson1_ex9_title",
            slideKeyPrefix = "java_lesson1_ex9",
            slideCount = 2,
            correctLines = new List<string> { "public class Main {", "    public static void main(String[] args) {", "        // age に 10 を入れる", "        int age = 10;", "        // 20さい以上かどうかで分ける", "        if (age >= 20) {", "            System.out.println(\"おとな\");", "        // else でそれ以外の場合", "        } else {", "            // こども と表示する", "            System.out.println(\"こども\");", "        }", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "java_lesson1_ex9_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "java_lesson1_ex9_comment2" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "java_lesson1_ex9_comment3" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "java_lesson1_ex9_comment4" }
            },
            expectedOutput = new List<string> { "こども" }
        });

        // Exercise 10: 論理演算子（&&、||）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "java_lesson1_ex10_title",
            slideKeyPrefix = "java_lesson1_ex10",
            slideCount = 2,
            correctLines = new List<string> { "public class Main {", "    public static void main(String[] args) {", "        // score と bonus を定義する", "        int score = 80;", "        int bonus = 10;", "        // && で両方の条件をチェックする", "        if (score >= 70 && bonus > 0) {", "            System.out.println(\"ボーナスあり合格\");", "        }", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "java_lesson1_ex10_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "java_lesson1_ex10_comment2" }
            },
            expectedOutput = new List<string> { "ボーナスあり合格" }
        });

        // Exercise 11: 順番に取り出す「拡張for文」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "java_lesson1_ex11_title",
            slideKeyPrefix = "java_lesson1_ex11",
            slideCount = 2,
            correctLines = new List<string> { "public class Main {", "    public static void main(String[] args) {", "        // 配列を作る", "        String[] names = {\"たろう\", \"はなこ\"};", "        // 変数 name で配列 names を順番に取り出す", "        for (String name : names) {", "            System.out.println(name);", "        }", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "java_lesson1_ex11_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "java_lesson1_ex11_comment2" }
            },
            expectedOutput = new List<string> { "たろう", "はなこ" }
        });

        // Exercise 12: 名前で探しましょう「HashMap」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "java_lesson1_ex12_title",
            slideKeyPrefix = "java_lesson1_ex12",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.HashMap;", "public class Main {", "    public static void main(String[] args) {", "        // new HashMap で作る", "        HashMap<String, String> user = new HashMap<>();", "        // put でデータを追加する", "        user.put(\"name\", \"たろう\");", "        // get でデータを取り出す", "        System.out.println(user.get(\"name\"));", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "java_lesson1_ex12_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "java_lesson1_ex12_comment2" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "java_lesson1_ex12_comment3" }
            },
            expectedOutput = new List<string> { "たろう" }
        });

        // Exercise 13: 自分だけの関数を作ろう「メソッド」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "java_lesson1_ex13_title",
            slideKeyPrefix = "java_lesson1_ex13",
            slideCount = 2,
            correctLines = new List<string> { "public class Main {", "    // greet というメソッドを定義する", "    public static void greet() {", "        System.out.println(\"こんにちは\");", "    }", "    public static void main(String[] args) {", "        // greet メソッドを呼び出す", "        greet();", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "java_lesson1_ex13_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "java_lesson1_ex13_comment2" }
            },
            expectedOutput = new List<string> { "こんにちは" }
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: Java II ====================
        var lesson2 = new Lesson { titleKey = "java_lesson2_title" };

        // Exercise 1: クラスの継承
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "java_lesson2_ex1_title",
            slideKeyPrefix = "java_lesson2_ex1",
            slideCount = 2,
            correctLines = new List<string> { "class Vehicle {", "    void move() {", "        System.out.println(\"Moving\");", "    }", "}", "", "// extends で Vehicle を継承する", "class Car extends Vehicle {", "    void honk() {", "        System.out.println(\"Beep!\");", "    }", "}", "", "class Main {", "    public static void main(String[] args) {", "        Car c = new Car();", "        c.move();", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "java_lesson2_ex1_comment1" }
            },
            expectedOutput = new List<string> { "Moving" }
        });

        // Exercise 2: メソッドのオーバーライド
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "java_lesson2_ex2_title",
            slideKeyPrefix = "java_lesson2_ex2",
            slideCount = 2,
            correctLines = new List<string> { "class Shape {", "    void draw() {", "        System.out.println(\"Shape\");", "    }", "}", "", "class Circle extends Shape {", "    // @Override でメソッドを上書き宣言する", "    @Override", "    void draw() {", "        System.out.println(\"Circle\");", "    }", "}", "", "class Main {", "    public static void main(String[] args) {", "        Circle c = new Circle();", "        c.draw();", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "java_lesson2_ex2_comment1" }
            },
            expectedOutput = new List<string> { "Circle" }
        });

        // Exercise 3: インターフェース
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "java_lesson2_ex3_title",
            slideKeyPrefix = "java_lesson2_ex3",
            slideCount = 2,
            correctLines = new List<string> { "interface Greeting {", "    void sayHello();", "}", "", "// implements でインターフェースを実装する", "class Person implements Greeting {", "    public void sayHello() {", "        System.out.println(\"Hello!\");", "    }", "}", "", "class Main {", "    public static void main(String[] args) {", "        Person p = new Person();", "        p.sayHello();", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "java_lesson2_ex3_comment1" }
            },
            expectedOutput = new List<string> { "Hello!" }
        });

        // Exercise 4: 抽象クラス
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "java_lesson2_ex4_title",
            slideKeyPrefix = "java_lesson2_ex4",
            slideCount = 2,
            correctLines = new List<string> { "// abstract で抽象クラスを定義する", "abstract class Animal {", "    abstract void speak();", "}", "", "class Dog extends Animal {", "    void speak() {", "        System.out.println(\"Woof!\");", "    }", "}", "", "class Main {", "    public static void main(String[] args) {", "        Dog d = new Dog();", "        d.speak();", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "java_lesson2_ex4_comment1" }
            },
            expectedOutput = new List<string> { "Woof!" }
        });

        // Exercise 5: 例外処理（try-catch）
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "java_lesson2_ex5_title",
            slideKeyPrefix = "java_lesson2_ex5",
            slideCount = 2,
            correctLines = new List<string> { "class Main {", "    public static void main(String[] args) {", "        // try で例外が起きる可能性のある処理を囲む", "        try {", "            // parseInt で文字列を整数に変換する", "            int x = Integer.parseInt(\"abc\");", "        } catch (Exception e) {", "            System.out.println(\"Error\");", "        }", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "java_lesson2_ex5_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "java_lesson2_ex5_comment2" }
            },
            expectedOutput = new List<string> { "Error" }
        });

        // Exercise 6: finally ブロック
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "java_lesson2_ex6_title",
            slideKeyPrefix = "java_lesson2_ex6",
            slideCount = 2,
            correctLines = new List<string> { "class Main {", "    public static void main(String[] args) {", "        try {", "            // println で出力する", "            System.out.println(\"Try\");", "        } catch (Exception e) {", "            System.out.println(\"Catch\");", "        // finally で必ず実行する", "        } finally {", "            System.out.println(\"Finally\");", "        }", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "java_lesson2_ex6_comment1" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "java_lesson2_ex6_comment2" }
            },
            expectedOutput = new List<string> { "Try", "Finally" }
        });

        // Exercise 7: ArrayList
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "java_lesson2_ex7_title",
            slideKeyPrefix = "java_lesson2_ex7",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.ArrayList;", "", "class Main {", "    public static void main(String[] args) {", "        ArrayList<String> items = new ArrayList<>();", "        // add で要素を追加する", "        items.add(\"A\");", "        items.add(\"B\");", "        System.out.println(items.get(0));", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "java_lesson2_ex7_comment1" }
            },
            expectedOutput = new List<string> { "A" }
        });

        // Exercise 8: 拡張for文（for-each）
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "java_lesson2_ex8_title",
            slideKeyPrefix = "java_lesson2_ex8",
            slideCount = 2,
            correctLines = new List<string> { "class Main {", "    public static void main(String[] args) {", "        String[] colors = {\"R\", \"G\", \"B\"};", "        // : で配列から順番に取り出す", "        for (String c : colors) {", "            System.out.println(c);", "        }", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "java_lesson2_ex8_comment1" }
            },
            expectedOutput = new List<string> { "R", "G", "B" }
        });

        // Exercise 9: staticメソッド
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "java_lesson2_ex9_title",
            slideKeyPrefix = "java_lesson2_ex9",
            slideCount = 2,
            correctLines = new List<string> { "class Calculator {", "    // static でクラスメソッドを定義する", "    static int multiply(int a, int b) {", "        // return で値を返す", "        return a * b;", "    }", "}", "", "class Main {", "    public static void main(String[] args) {", "        int result = Calculator.multiply(4, 5);", "        System.out.println(result);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "java_lesson2_ex9_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "java_lesson2_ex9_comment2" }
            },
            expectedOutput = new List<string> { "20" }
        });

        // Exercise 10: アクセス修飾子
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "java_lesson2_ex10_title",
            slideKeyPrefix = "java_lesson2_ex10",
            slideCount = 2,
            correctLines = new List<string> { "class Item {", "    // private でフィールドを隠蔽する", "    private int price;", "    ", "    public Item(int p) {", "        price = p;", "    }", "    ", "    public int getPrice() {", "        return price;", "    }", "}", "", "class Main {", "    public static void main(String[] args) {", "        Item item = new Item(100);", "        System.out.println(item.getPrice());", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "java_lesson2_ex10_comment1" }
            },
            expectedOutput = new List<string> { "100" }
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: Java III ====================
        var lesson3 = new Lesson { titleKey = "java_lesson3_title" };

        // Exercise 1: ラムダ式の基本
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "java_lesson3_ex1_title",
            slideKeyPrefix = "java_lesson3_ex1",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.function.Function;", "", "public class Main {", "    public static void main(String[] args) {", "        Function<Integer, Integer> square = x -> x * x;", "        System.out.println(square.apply(5));", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "25" }
        });

        // Exercise 2: Stream の作成
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "java_lesson3_ex2_title",
            slideKeyPrefix = "java_lesson3_ex2",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.*;", "import java.util.stream.*;", "", "public class Main {", "    public static void main(String[] args) {", "        List<Integer> nums = Arrays.asList(1, 2, 3);", "        nums.stream().forEach(System.out::println);", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "1", "2", "3" }
        });

        // Exercise 3: map で変換
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "java_lesson3_ex3_title",
            slideKeyPrefix = "java_lesson3_ex3",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.*;", "", "public class Main {", "    public static void main(String[] args) {", "        List<Integer> nums = Arrays.asList(1, 2, 3);", "        nums.stream()", "            .map(n -> n * 10)", "            .forEach(System.out::println);", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "10", "20", "30" }
        });

        // Exercise 4: filter で絞り込み
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "java_lesson3_ex4_title",
            slideKeyPrefix = "java_lesson3_ex4",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.*;", "", "public class Main {", "    public static void main(String[] args) {", "        List<Integer> nums = Arrays.asList(1, 2, 3, 4, 5);", "        nums.stream()", "            .filter(n -> n > 2)", "            .forEach(System.out::println);", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "3", "4", "5" }
        });

        // Exercise 5: collect でリストに変換
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "java_lesson3_ex5_title",
            slideKeyPrefix = "java_lesson3_ex5",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.*;", "import java.util.stream.*;", "", "public class Main {", "    public static void main(String[] args) {", "        List<Integer> nums = Arrays.asList(1, 2, 3);", "        List<Integer> doubled = nums.stream()", "            .map(n -> n * 2)", "            .collect(Collectors.toList());", "        System.out.println(doubled);", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "[2, 4, 6]" }
        });

        // Exercise 6: reduce で集約
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "java_lesson3_ex6_title",
            slideKeyPrefix = "java_lesson3_ex6",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.*;", "", "public class Main {", "    public static void main(String[] args) {", "        List<Integer> nums = Arrays.asList(1, 2, 3, 4);", "        int product = nums.stream()", "            .reduce(1, (a, b) -> a * b);", "        System.out.println(product);", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "24" }
        });

        // Exercise 7: Optional の基本
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "java_lesson3_ex7_title",
            slideKeyPrefix = "java_lesson3_ex7",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.*;", "", "public class Main {", "    public static void main(String[] args) {", "        Optional<String> opt = Optional.of(\"Hello\");", "        String value = opt.orElse(\"default\");", "        System.out.println(value);", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "Hello" }
        });

        // Exercise 8: メソッド参照
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "java_lesson3_ex8_title",
            slideKeyPrefix = "java_lesson3_ex8",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.*;", "", "public class Main {", "    public static void main(String[] args) {", "        List<String> names = Arrays.asList(\"Alice\", \"Bob\");", "        names.forEach(System.out::println);", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "Alice", "Bob" }
        });

        // Exercise 9: sorted でソート
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "java_lesson3_ex9_title",
            slideKeyPrefix = "java_lesson3_ex9",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.*;", "", "public class Main {", "    public static void main(String[] args) {", "        List<Integer> nums = Arrays.asList(5, 2, 8, 1);", "        nums.stream()", "            .sorted()", "            .forEach(System.out::println);", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "1", "2", "5", "8" }
        });

        // Exercise 10: distinct で重複除去
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "java_lesson3_ex10_title",
            slideKeyPrefix = "java_lesson3_ex10",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.*;", "", "public class Main {", "    public static void main(String[] args) {", "        List<Integer> nums = Arrays.asList(1, 1, 2, 2, 3);", "        nums.stream()", "            .distinct()", "            .forEach(System.out::println);", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "1", "2", "3" }
        });

        lessons.Add(lesson3);

        // ==================== LESSON 4: Java IV ====================
        var lesson4 = new Lesson { titleKey = "java_lesson4_title" };

        // Exercise 1: ジェネリクスの基本
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "java_lesson4_ex1_title",
            slideKeyPrefix = "java_lesson4_ex1",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.*;", "", "class Box<T> {", "    private T value;", "    public void set(T v) { value = v; }", "    public T get() { return value; }", "}", "", "public class Main {", "    public static void main(String[] args) {", "        Box<String> box = new Box<>();", "        box.set(\"Java\");", "        System.out.println(box.get());", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "Java" }
        });

        // Exercise 2: 境界型パラメータ
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "java_lesson4_ex2_title",
            slideKeyPrefix = "java_lesson4_ex2",
            slideCount = 2,
            correctLines = new List<string> { "class Calculator<T extends Number> {", "    private T value;", "    public Calculator(T v) { value = v; }", "    public double getDouble() { return value.doubleValue(); }", "}", "", "public class Main {", "    public static void main(String[] args) {", "        Calculator<Integer> calc = new Calculator<>(42);", "        System.out.println(calc.getDouble());", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "42.0" }
        });

        // Exercise 3: ワイルドカード
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "java_lesson4_ex3_title",
            slideKeyPrefix = "java_lesson4_ex3",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.*;", "", "public class Main {", "    public static void printAll(List<?> list) {", "        for (Object item : list) {", "            System.out.println(item);", "        }", "    }", "    public static void main(String[] args) {", "        List<String> names = Arrays.asList(\"A\", \"B\");", "        printAll(names);", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "A", "B" }
        });

        // Exercise 4: Map の基本操作
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "java_lesson4_ex4_title",
            slideKeyPrefix = "java_lesson4_ex4",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.*;", "", "public class Main {", "    public static void main(String[] args) {", "        Map<String, Integer> scores = new HashMap<>();", "        scores.put(\"Math\", 90);", "        scores.put(\"English\", 85);", "        System.out.println(scores.get(\"Math\"));", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "90" }
        });

        // Exercise 5: Map のイテレーション
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "java_lesson4_ex5_title",
            slideKeyPrefix = "java_lesson4_ex5",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.*;", "", "public class Main {", "    public static void main(String[] args) {", "        Map<String, Integer> map = new HashMap<>();", "        map.put(\"A\", 1);", "        map.put(\"B\", 2);", "        for (Map.Entry<String, Integer> e : map.entrySet()) {", "            System.out.println(e.getKey());", "        }", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "A", "B" }
        });

        // Exercise 6: Comparator
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "java_lesson4_ex6_title",
            slideKeyPrefix = "java_lesson4_ex6",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.*;", "", "public class Main {", "    public static void main(String[] args) {", "        List<String> words = new ArrayList<>(Arrays.asList(\"cat\", \"a\", \"elephant\"));", "        words.sort(Comparator.comparing(String::length));", "        System.out.println(words);", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "[a, cat, elephant]" }
        });

        // Exercise 7: Comparable の実装
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "java_lesson4_ex7_title",
            slideKeyPrefix = "java_lesson4_ex7",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.*;", "", "class Score implements Comparable<Score> {", "    int value;", "    Score(int v) { value = v; }", "    public int compareTo(Score other) {", "        return this.value - other.value;", "    }", "}", "", "public class Main {", "    public static void main(String[] args) {", "        List<Score> scores = Arrays.asList(new Score(80), new Score(60));", "        Collections.sort(scores);", "        System.out.println(scores.get(0).value);", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "60" }
        });

        // Exercise 8: Enum の定義
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "java_lesson4_ex8_title",
            slideKeyPrefix = "java_lesson4_ex8",
            slideCount = 2,
            correctLines = new List<string> { "enum Day {", "    MON, TUE, WED, THU, FRI, SAT, SUN", "}", "", "public class Main {", "    public static void main(String[] args) {", "        Day today = Day.MON;", "        System.out.println(today);", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "MON" }
        });

        // Exercise 9: Queue の使い方
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "java_lesson4_ex9_title",
            slideKeyPrefix = "java_lesson4_ex9",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.*;", "", "public class Main {", "    public static void main(String[] args) {", "        Queue<String> queue = new LinkedList<>();", "        queue.offer(\"First\");", "        queue.offer(\"Second\");", "        System.out.println(queue.poll());", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "First" }
        });

        // Exercise 10: Deque の使い方
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "java_lesson4_ex10_title",
            slideKeyPrefix = "java_lesson4_ex10",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.*;", "", "public class Main {", "    public static void main(String[] args) {", "        Deque<Integer> stack = new ArrayDeque<>();", "        stack.push(10);", "        stack.push(20);", "        System.out.println(stack.pop());", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "20" }
        });

        lessons.Add(lesson4);

        // ==================== LESSON 5: Java V ====================
        var lesson5 = new Lesson { titleKey = "java_lesson5_title" };

        // Exercise 1: try-catch の基本
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "java_lesson5_ex1_title",
            slideKeyPrefix = "java_lesson5_ex1",
            slideCount = 2,
            correctLines = new List<string> { "public class Main {", "    public static void main(String[] args) {", "        try {", "            int result = 10 / 0;", "        } catch (ArithmeticException e) {", "            System.out.println(\"Error\");", "        }", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "Error" }
        });

        // Exercise 2: finally ブロック
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "java_lesson5_ex2_title",
            slideKeyPrefix = "java_lesson5_ex2",
            slideCount = 2,
            correctLines = new List<string> { "public class Main {", "    public static void main(String[] args) {", "        try {", "            System.out.println(\"Try\");", "        } catch (Exception e) {", "            System.out.println(\"Catch\");", "        } finally {", "            System.out.println(\"Finally\");", "        }", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "Try", "Finally" }
        });

        // Exercise 3: throws 宣言
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "java_lesson5_ex3_title",
            slideKeyPrefix = "java_lesson5_ex3",
            slideCount = 2,
            correctLines = new List<string> { "import java.io.*;", "", "public class Main {", "    public static void riskyMethod() throws Exception {", "        throw new Exception(\"Error!\");", "    }", "    public static void main(String[] args) {", "        try {", "            riskyMethod();", "        } catch (Exception e) {", "            System.out.println(\"Caught\");", "        }", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "Caught" }
        });

        // Exercise 4: カスタム例外
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "java_lesson5_ex4_title",
            slideKeyPrefix = "java_lesson5_ex4",
            slideCount = 2,
            correctLines = new List<string> { "class InvalidAgeException extends Exception {", "    public InvalidAgeException(String msg) {", "        super(msg);", "    }", "}", "", "public class Main {", "    public static void main(String[] args) {", "        try {", "            throw new InvalidAgeException(\"Invalid\");", "        } catch (InvalidAgeException e) {", "            System.out.println(e.getMessage());", "        }", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "Invalid" }
        });

        // Exercise 5: try-with-resources
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "java_lesson5_ex5_title",
            slideKeyPrefix = "java_lesson5_ex5",
            slideCount = 2,
            correctLines = new List<string> { "import java.io.*;", "", "public class Main {", "    public static void main(String[] args) {", "        try (StringReader reader = new StringReader(\"Hello\")) {", "            System.out.println((char) reader.read());", "        } catch (IOException e) {", "            e.printStackTrace();", "        }", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "H" }
        });

        // Exercise 6: BufferedReader でファイル読み込み
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "java_lesson5_ex6_title",
            slideKeyPrefix = "java_lesson5_ex6",
            slideCount = 2,
            correctLines = new List<string> { "import java.io.*;", "", "public class Main {", "    public static void main(String[] args) throws IOException {", "        String content = \"Line1\\nLine2\";", "        BufferedReader br = new BufferedReader(new StringReader(content));", "        String line = br.readLine();", "        System.out.println(line);", "        br.close();", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "Line1" }
        });

        // Exercise 7: BufferedWriter でファイル書き込み
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "java_lesson5_ex7_title",
            slideKeyPrefix = "java_lesson5_ex7",
            slideCount = 2,
            correctLines = new List<string> { "import java.io.*;", "", "public class Main {", "    public static void main(String[] args) throws IOException {", "        StringWriter sw = new StringWriter();", "        BufferedWriter bw = new BufferedWriter(sw);", "        bw.write(\"Hello\");", "        bw.flush();", "        System.out.println(sw.toString());", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "Hello" }
        });

        // Exercise 8: Scanner での入力
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "java_lesson5_ex8_title",
            slideKeyPrefix = "java_lesson5_ex8",
            slideCount = 2,
            correctLines = new List<string> { "import java.util.*;", "", "public class Main {", "    public static void main(String[] args) {", "        Scanner sc = new Scanner(\"42 Hello\");", "        int num = sc.nextInt();", "        String word = sc.next();", "        System.out.println(num + \" \" + word);", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "42 Hello" }
        });

        // Exercise 9: Files.readAllLines
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "java_lesson5_ex9_title",
            slideKeyPrefix = "java_lesson5_ex9",
            slideCount = 2,
            correctLines = new List<string> { "import java.nio.file.*;", "import java.util.*;", "", "public class Main {", "    public static void main(String[] args) throws Exception {", "        Path path = Paths.get(\".\");", "        boolean exists = Files.exists(path);", "        System.out.println(exists);", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "true" }
        });

        // Exercise 10: 複数例外のキャッチ
        lesson5.exercises.Add(new Exercise
        {
            titleKey = "java_lesson5_ex10_title",
            slideKeyPrefix = "java_lesson5_ex10",
            slideCount = 2,
            correctLines = new List<string> { "public class Main {", "    public static void main(String[] args) {", "        try {", "            String s = null;", "            s.length();", "        } catch (NullPointerException | ArrayIndexOutOfBoundsException e) {", "            System.out.println(\"Caught\");", "        }", "    }", "}" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string> { "Caught" }
        });

        lessons.Add(lesson5);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;

        Debug.Log($"[LessonManager] Initialized {lessons.Count} Java lessons");
    }

    /// <summary>
    /// Initialize C lessons from cLessons.json data
    /// </summary>
    private void InitializeCLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: C I - Getting Started ====================
        var lesson1 = new Lesson { titleKey = "c_lesson1_title" };

        // Exercise 1: Display text on screen
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "c_lesson1_ex1_title",
            slideKeyPrefix = "c_lesson1_ex1",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    // 画面にメッセージを出す関数", "    printf(\"Hello, C!\\n\");", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "c_lesson1_ex1_comment1" }
            },
            expectedOutput = new List<string> { "Hello, C!" }
        });

        // Exercise 2: Number box 'int'
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "c_lesson1_ex2_title",
            slideKeyPrefix = "c_lesson1_ex2",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    // int（数字）ラベルのはこを作る", "    int count = 10;", "    // 中身を表示する", "    printf(\"%d\\n\", count);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "c_lesson1_ex2_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "c_lesson1_ex2_comment2" }
            },
            expectedOutput = new List<string> { "10" }
        });

        // Exercise 3: Calculate with computer
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "c_lesson1_ex3_title",
            slideKeyPrefix = "c_lesson1_ex3",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    // はこに数字を入れる", "    int x = 10;", "    int y = 20;", "    // たし算した結果を表示する", "    printf(\"%d\\n\", x + y);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "c_lesson1_ex3_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "c_lesson1_ex3_comment2" }
            },
            expectedOutput = new List<string> { "30" }
        });

        // Exercise 4: Modulo operator (%)
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "c_lesson1_ex4_title",
            slideKeyPrefix = "c_lesson1_ex4",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    // 10 を 3 で割ったあまりを出力する", "    printf(\"%d\\n\", 10 % 3);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "c_lesson1_ex4_comment1" }
            },
            expectedOutput = new List<string> { "1" }
        });

        // Exercise 5: Compound assignment (+=, -=)
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "c_lesson1_ex5_title",
            slideKeyPrefix = "c_lesson1_ex5",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    // scoreに50を入れる", "    int score = 50;", "    // 10点プラスする", "    score += 10;", "    // 結果を表示", "    printf(\"%d\\n\", score);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "c_lesson1_ex5_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "c_lesson1_ex5_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "c_lesson1_ex5_comment3" }
            },
            expectedOutput = new List<string> { "60" }
        });

        // Exercise 6: Variables in text
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "c_lesson1_ex6_title",
            slideKeyPrefix = "c_lesson1_ex6",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    // ageに10を入れる", "    int age = 10;", "    // 文章の中に中身を表示する", "    printf(\"I am %d years old\\n\", age);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "c_lesson1_ex6_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "c_lesson1_ex6_comment2" }
            },
            expectedOutput = new List<string> { "I am 10 years old" }
        });

        // Exercise 7: Arrays
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "c_lesson1_ex7_title",
            slideKeyPrefix = "c_lesson1_ex7",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    // 配列を作成", "    int scores[] = {100, 50};", "    // 2番目（番号は1）を出す", "    printf(\"%d\\n\", scores[1]);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "c_lesson1_ex7_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "c_lesson1_ex7_comment2" }
            },
            expectedOutput = new List<string> { "50" }
        });

        // Exercise 8: if statement
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "c_lesson1_ex8_title",
            slideKeyPrefix = "c_lesson1_ex8",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    // scoreに100を入れる", "    int score = 100;", "    // > で「より大きい」を比較", "    if (score > 80) {", "        // メッセージを表示", "        printf(\"Pass!\\n\");", "    }", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "c_lesson1_ex8_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "c_lesson1_ex8_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "c_lesson1_ex8_comment3" }
            },
            expectedOutput = new List<string> { "Pass!" }
        });

        // Exercise 9: if-else statement
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "c_lesson1_ex9_title",
            slideKeyPrefix = "c_lesson1_ex9",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    // xに5を入れる", "    int x = 5;", "    // 10より大きいかを比較", "    if (x > 10) {", "        printf(\"Big\\n\");", "    // else で「それ以外」", "    } else {", "        printf(\"Small\\n\");", "    }", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "c_lesson1_ex9_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "c_lesson1_ex9_comment2" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "c_lesson1_ex9_comment3" }
            },
            expectedOutput = new List<string> { "Small" }
        });

        // Exercise 10: Logical operators (&&, ||)
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "c_lesson1_ex10_title",
            slideKeyPrefix = "c_lesson1_ex10",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    // scoreに85を入れる", "    int score = 85;", "    // 両方の条件を満たすので && を使います", "    if (score >= 80 && score <= 100) {", "        printf(\"Excellent!\\n\");", "    }", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "c_lesson1_ex10_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "c_lesson1_ex10_comment2" }
            },
            expectedOutput = new List<string> { "Excellent!" }
        });

        // Exercise 11: for loop
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "c_lesson1_ex11_title",
            slideKeyPrefix = "c_lesson1_ex11",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    // i++ で1つ増やす", "    for (int i = 0; i < 3; i++) {", "        printf(\"Hey\\n\");", "    }", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "c_lesson1_ex11_comment1" }
            },
            expectedOutput = new List<string> { "Hey", "Hey", "Hey" }
        });

        // Exercise 12: struct
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "c_lesson1_ex12_title",
            slideKeyPrefix = "c_lesson1_ex12",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "struct Book {", "    int price;", "};", "int main() {", "    struct Book b = {500};", "    // x でメンバにアクセス", "    printf(\"%d\\n\", b.price);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "c_lesson1_ex12_comment1" }
            },
            expectedOutput = new List<string> { "500" }
        });

        // Exercise 13: Functions
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "c_lesson1_ex13_title",
            slideKeyPrefix = "c_lesson1_ex13",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "void greet() {", "    printf(\"Hello\\n\");", "}", "int main() {", "    // greet で関数を呼び出す", "    greet();", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "c_lesson1_ex13_comment1" }
            },
            expectedOutput = new List<string> { "Hello" }
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: C II - Pointers & Memory ====================
        var lesson2 = new Lesson { titleKey = "c_lesson2_title" };

        // Exercise 1: What is a pointer?
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "c_lesson2_ex1_title",
            slideKeyPrefix = "c_lesson2_ex1",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    int x = 10;", "    // & でアドレスを取得", "    int *p = &x;", "    printf(\"%d\\n\", *p);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "c_lesson2_ex1_comment1" }
            },
            expectedOutput = new List<string> { "10" }
        });

        // Exercise 2: Changing values via pointer
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "c_lesson2_ex2_title",
            slideKeyPrefix = "c_lesson2_ex2",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    int x = 5;", "    int *p = &x;", "    // * で値にアクセス", "    *p = 100;", "    printf(\"%d\\n\", x);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "c_lesson2_ex2_comment1" }
            },
            expectedOutput = new List<string> { "100" }
        });

        // Exercise 3: Function arguments
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "c_lesson2_ex3_title",
            slideKeyPrefix = "c_lesson2_ex3",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "// 引数名 num を定義", "void greet(int age) {", "    printf(\"%d years old\\n\", age);", "}", "int main() {", "    greet(10);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "c_lesson2_ex3_comment1" }
            },
            expectedOutput = new List<string> { "10 years old" }
        });

        // Exercise 4: Return values
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "c_lesson2_ex4_title",
            slideKeyPrefix = "c_lesson2_ex4",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int add(int a, int b) {", "    // return で結果を返す", "    return a + b;", "}", "int main() {", "    int result = add(3, 5);", "    printf(\"%d\\n\", result);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "c_lesson2_ex4_comment1" }
            },
            expectedOutput = new List<string> { "8" }
        });

        // Exercise 5: Arrays and pointers
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "c_lesson2_ex5_title",
            slideKeyPrefix = "c_lesson2_ex5",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    int arr[] = {10, 20, 30};", "    // 配列名がアドレス", "    int *p = arr;", "    printf(\"%d\\n\", *(p + 1));", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "c_lesson2_ex5_comment1" }
            },
            expectedOutput = new List<string> { "20" }
        });

        // Exercise 6: String basics
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "c_lesson2_ex6_title",
            slideKeyPrefix = "c_lesson2_ex6",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    char name[] = \"Taro\";", "    // %s で文字列を表示", "    printf(\"%s\\n\", name);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "c_lesson2_ex6_comment1" }
            },
            expectedOutput = new List<string> { "Taro" }
        });

        // Exercise 7: Passing pointers to functions
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "c_lesson2_ex7_title",
            slideKeyPrefix = "c_lesson2_ex7",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "void add_ten(int *p) {", "    *p = *p + 10;", "}", "int main() {", "    int x = 5;", "    // & でアドレスを渡す", "    add_ten(&x);", "    printf(\"%d\\n\", x);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "c_lesson2_ex7_comment1" }
            },
            expectedOutput = new List<string> { "15" }
        });

        // Exercise 8: Dynamic memory (malloc)
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "c_lesson2_ex8_title",
            slideKeyPrefix = "c_lesson2_ex8",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "#include <stdlib.h>", "int main() {", "    int *p = malloc(sizeof(int));", "    *p = 100;", "    printf(\"%d\\n\", *p);", "    // free でメモリを解放", "    free(p);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "c_lesson2_ex8_comment1" }
            },
            expectedOutput = new List<string> { "100" }
        });

        // Exercise 9: Struct pointers
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "c_lesson2_ex9_title",
            slideKeyPrefix = "c_lesson2_ex9",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "struct Point {", "    int x;", "    int y;", "};", "int main() {", "    struct Point pt = {3, 4};", "    struct Point *p = &pt;", "    // アロー演算子 ->", "    printf(\"%d\\n\", p->x);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "c_lesson2_ex9_comment1" }
            },
            expectedOutput = new List<string> { "3" }
        });

        // Exercise 10: enum
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "c_lesson2_ex10_title",
            slideKeyPrefix = "c_lesson2_ex10",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "enum Color { RED, GREEN, BLUE };", "int main() {", "    // enum 型の変数を宣言", "    enum Color c = GREEN;", "    printf(\"%d\\n\", c);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "c_lesson2_ex10_comment1" }
            },
            expectedOutput = new List<string> { "1" }
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: C III - Advanced Pointers & Files ====================
        var lesson3 = new Lesson { titleKey = "c_lesson3_title" };

        // Exercise 1: Function pointers
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "c_lesson3_ex1_title",
            slideKeyPrefix = "c_lesson3_ex1",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int add(int a, int b) { return a + b; }", "int main() {", "    // *fp で関数ポインタを宣言", "    int (*fp)(int, int) = add;", "    printf(\"%d\\n\", fp(2, 3));", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "c_lesson3_ex1_comment1" }
            },
            expectedOutput = new List<string> { "5" }
        });

        // Exercise 2: Callback functions
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "c_lesson3_ex2_title",
            slideKeyPrefix = "c_lesson3_ex2",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "void print(int x) { printf(\"%d \", x); }", "void process(int arr[], int n, void (*f)(int)) {", "    for (int i = 0; i < n; i++) {", "        // f で関数ポインタを呼び出す", "        f(arr[i]);", "    }", "}", "int main() {", "    int arr[] = {1, 2, 3};", "    process(arr, 3, print);", "    printf(\"\\n\");", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "c_lesson3_ex2_comment1" }
            },
            expectedOutput = new List<string> { "1 2 3" }
        });

        // Exercise 3: Bitwise AND
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "c_lesson3_ex3_title",
            slideKeyPrefix = "c_lesson3_ex3",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    // & でビットAND", "    int result = 5 & 3;", "    printf(\"%d\\n\", result);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "c_lesson3_ex3_comment1" }
            },
            expectedOutput = new List<string> { "1" }
        });

        // Exercise 4: Bitwise OR
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "c_lesson3_ex4_title",
            slideKeyPrefix = "c_lesson3_ex4",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    // | でビットOR", "    int result = 5 | 3;", "    printf(\"%d\\n\", result);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "c_lesson3_ex4_comment1" }
            },
            expectedOutput = new List<string> { "7" }
        });

        // Exercise 5: Bit shift
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "c_lesson3_ex5_title",
            slideKeyPrefix = "c_lesson3_ex5",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    int x = 1;", "    // << で左シフト", "    printf(\"%d\\n\", x << 3);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "c_lesson3_ex5_comment1" }
            },
            expectedOutput = new List<string> { "8" }
        });

        // Exercise 6: sizeof operator
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "c_lesson3_ex6_title",
            slideKeyPrefix = "c_lesson3_ex6",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    int arr[] = {1, 2, 3, 4, 5};", "    // sizeof でサイズを取得", "    int count = sizeof(arr) / sizeof(arr[0]);", "    printf(\"%d\\n\", count);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "c_lesson3_ex6_comment1" }
            },
            expectedOutput = new List<string> { "5" }
        });

        // Exercise 7: typedef
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "c_lesson3_ex7_title",
            slideKeyPrefix = "c_lesson3_ex7",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "// typedef で別名を定義", "typedef struct {", "    int x;", "    int y;", "} Point;", "int main() {", "    Point p = {10, 20};", "    printf(\"%d\\n\", p.x);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "c_lesson3_ex7_comment1" }
            },
            expectedOutput = new List<string> { "10" }
        });

        // Exercise 8: const pointer
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "c_lesson3_ex8_title",
            slideKeyPrefix = "c_lesson3_ex8",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "void print(const char *str) {", "    printf(\"%s\\n\", str);", "}", "int main() {", "    // const で変更不可", "    print(\"Hello\");", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "c_lesson3_ex8_comment1" }
            },
            expectedOutput = new List<string> { "Hello" }
        });

        // Exercise 9: static variable
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "c_lesson3_ex9_title",
            slideKeyPrefix = "c_lesson3_ex9",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "void count() {", "    // static で値を保持", "    static int n = 0;", "    n++;", "    printf(\"%d\\n\", n);", "}", "int main() {", "    count();", "    count();", "    count();", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "c_lesson3_ex9_comment1" }
            },
            expectedOutput = new List<string> { "1", "2", "3" }
        });

        // Exercise 10: Macro definition
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "c_lesson3_ex10_title",
            slideKeyPrefix = "c_lesson3_ex10",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "// define でマクロを定義", "#define SQUARE(x) ((x) * (x))", "int main() {", "    int result = SQUARE(5);", "    printf(\"%d\\n\", result);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "c_lesson3_ex10_comment1" }
            },
            expectedOutput = new List<string> { "25" }
        });

        lessons.Add(lesson3);

        // ==================== LESSON 4: C IV - File I/O & Advanced Types ====================
        var lesson4 = new Lesson { titleKey = "c_lesson4_title" };

        // Exercise 1: File open (fopen)
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "c_lesson4_ex1_title",
            slideKeyPrefix = "c_lesson4_ex1",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    // fopen でファイルを開く", "    FILE *fp = fopen(\"test.txt\", \"w\");", "    if (fp != NULL) {", "        fprintf(fp, \"Hello\\n\");", "        fclose(fp);", "    }", "    printf(\"Done\\n\");", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "c_lesson4_ex1_comment1" }
            },
            expectedOutput = new List<string> { "Done" }
        });

        // Exercise 2: File close (fclose)
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "c_lesson4_ex2_title",
            slideKeyPrefix = "c_lesson4_ex2",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    FILE *fp = fopen(\"test.txt\", \"w\");", "    fprintf(fp, \"Data\\n\");", "    // fclose でファイルを閉じる", "    fclose(fp);", "    printf(\"Closed\\n\");", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "c_lesson4_ex2_comment1" }
            },
            expectedOutput = new List<string> { "Closed" }
        });

        // Exercise 3: File write (fprintf)
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "c_lesson4_ex3_title",
            slideKeyPrefix = "c_lesson4_ex3",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    FILE *fp = fopen(\"out.txt\", \"w\");", "    // fprintf でファイルに書き込む", "    fprintf(fp, \"Name: %s\\n\", \"Alice\");", "    fclose(fp);", "    printf(\"Written\\n\");", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "c_lesson4_ex3_comment1" }
            },
            expectedOutput = new List<string> { "Written" }
        });

        // Exercise 4: Line read (fgets)
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "c_lesson4_ex4_title",
            slideKeyPrefix = "c_lesson4_ex4",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    char buf[256] = \"Default\";", "    // fgets で1行読み込む", "    printf(\"%s\\n\", buf);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "c_lesson4_ex4_comment1" }
            },
            expectedOutput = new List<string> { "Default" }
        });

        // Exercise 5: File seek (fseek)
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "c_lesson4_ex5_title",
            slideKeyPrefix = "c_lesson4_ex5",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    FILE *fp = fopen(\"test.txt\", \"w\");", "    fprintf(fp, \"ABCDEF\");", "    // fseek でファイル位置を移動", "    fseek(fp, 0, SEEK_SET);", "    fclose(fp);", "    printf(\"Seeked\\n\");", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "c_lesson4_ex5_comment1" }
            },
            expectedOutput = new List<string> { "Seeked" }
        });

        // Exercise 6: File position (ftell)
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "c_lesson4_ex6_title",
            slideKeyPrefix = "c_lesson4_ex6",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    FILE *fp = fopen(\"test.txt\", \"w\");", "    fprintf(fp, \"Hello\");", "    // ftell で現在位置を取得", "    long pos = ftell(fp);", "    fclose(fp);", "    printf(\"%ld\\n\", pos);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "c_lesson4_ex6_comment1" }
            },
            expectedOutput = new List<string> { "5" }
        });

        // Exercise 7: enum type
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "c_lesson4_ex7_title",
            slideKeyPrefix = "c_lesson4_ex7",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "// enum で列挙型を定義", "enum Status {", "    OK = 200,", "    NOT_FOUND = 404,", "    ERROR = 500", "};", "int main() {", "    enum Status s = OK;", "    printf(\"%d\\n\", s);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "c_lesson4_ex7_comment1" }
            },
            expectedOutput = new List<string> { "200" }
        });

        // Exercise 8: union
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "c_lesson4_ex8_title",
            slideKeyPrefix = "c_lesson4_ex8",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "// union で共用体を定義", "union Data {", "    int i;", "    float f;", "};", "int main() {", "    union Data d;", "    d.i = 10;", "    printf(\"%d\\n\", d.i);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "c_lesson4_ex8_comment1" }
            },
            expectedOutput = new List<string> { "10" }
        });

        // Exercise 9: Double pointer
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "c_lesson4_ex9_title",
            slideKeyPrefix = "c_lesson4_ex9",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "int main() {", "    int x = 10;", "    int *p = &x;", "    // ** で二重ポインタ", "    int **pp = &p;", "    printf(\"%d\\n\", **pp);", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "c_lesson4_ex9_comment1" }
            },
            expectedOutput = new List<string> { "10" }
        });

        // Exercise 10: Variable arguments
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "c_lesson4_ex10_title",
            slideKeyPrefix = "c_lesson4_ex10",
            slideCount = 2,
            correctLines = new List<string> { "#include <stdio.h>", "#include <stdarg.h>", "int sum(int count, ...) {", "    va_list args;", "    // va_start で可変長引数を初期化", "    va_start(args, count);", "    int total = 0;", "    for (int i = 0; i < count; i++) {", "        total += va_arg(args, int);", "    }", "    va_end(args);", "    return total;", "}", "int main() {", "    printf(\"%d\\n\", sum(3, 10, 20, 30));", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "c_lesson4_ex10_comment1" }
            },
            expectedOutput = new List<string> { "60" }
        });

        lessons.Add(lesson4);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;

        Debug.Log($"[LessonManager] Initialized {lessons.Count} C lessons");
    }


    /// <summary>
    /// Initialize C++ lessons from cppLessons.json data
    /// </summary>
    private void InitializeCppLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: C++ I - Getting Started ====================
        var lesson1 = new Lesson { titleKey = "cpp_lesson1_title" };

        // Exercise 1: Display a message
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson1_ex1_title",
            slideKeyPrefix = "cpp_lesson1_ex1",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "int main() {", "    // 入出力の機能を使えるように準備します", "    std::cout << \"Hello, C++!\" << std::endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "cpp_lesson1_ex1_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "cpp_lesson1_ex1_comment2" }
            },
            expectedOutput = new List<string> { "Hello, C++!" }
        });

        // Exercise 2: Number box 'int'
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson1_ex2_title",
            slideKeyPrefix = "cpp_lesson1_ex2",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "int main() {", "    // int（数字）ラベルのはこを作る", "    int x = 10;", "    // 中身を表示する", "    std::cout << x << std::endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "cpp_lesson1_ex2_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "cpp_lesson1_ex2_comment2" }
            },
            expectedOutput = new List<string> { "10" }
        });

        // Exercise 3: Calculate with computer
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson1_ex3_title",
            slideKeyPrefix = "cpp_lesson1_ex3",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "int main() {", "    // はこに数字を入れる", "    int a = 5;", "    int b = 3;", "    // たし算した結果を表示する", "    std::cout << a + b << std::endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "cpp_lesson1_ex3_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "cpp_lesson1_ex3_comment2" }
            },
            expectedOutput = new List<string> { "8" }
        });

        // Exercise 4: Modulo operator (%)
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson1_ex4_title",
            slideKeyPrefix = "cpp_lesson1_ex4",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "int main() {", "    // 10 を 3 で割ったあまりを出力する", "    std::cout << 10 % 3 << std::endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "cpp_lesson1_ex4_comment1" }
            },
            expectedOutput = new List<string> { "1" }
        });

        // Exercise 5: Compound assignment (+=, -=)
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson1_ex5_title",
            slideKeyPrefix = "cpp_lesson1_ex5",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "int main() {", "    // scoreに50を入れる", "    int score = 50;", "    // 10点プラスする", "    score += 10;", "    // 結果を表示", "    std::cout << score << std::endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "cpp_lesson1_ex5_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "cpp_lesson1_ex5_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "cpp_lesson1_ex5_comment3" }
            },
            expectedOutput = new List<string> { "60" }
        });

        // Exercise 6: Combine text and variables
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson1_ex6_title",
            slideKeyPrefix = "cpp_lesson1_ex6",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "int main() {", "    // ageに10を入れる", "    int age = 10;", "    // 文字とはこを並べて表示する", "    std::cout << \"I am \" << age << \" years old.\" << std::endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "cpp_lesson1_ex6_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "cpp_lesson1_ex6_comment2" }
            },
            expectedOutput = new List<string> { "I am 10 years old." }
        });

        // Exercise 7: Flexible box 'vector'
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson1_ex7_title",
            slideKeyPrefix = "cpp_lesson1_ex7",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <vector>", "#include <string>", "int main() {", "    // くだものの配列（vector）を作る", "    std::vector<std::string> fruits = {\"りんご\", \"バナナ\"};", "    // 2番目のデータを表示する", "    std::cout << fruits[1] << std::endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "cpp_lesson1_ex7_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "cpp_lesson1_ex7_comment2" }
            },
            expectedOutput = new List<string> { "バナナ" }
        });

        // Exercise 8: if statement
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson1_ex8_title",
            slideKeyPrefix = "cpp_lesson1_ex8",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "int main() {", "    // scoreに100を入れる", "    int score = 100;", "    // もし80より大きければ表示する", "    if (score > 80) {", "        // メッセージを表示", "        std::cout << \"Perfect\" << std::endl;", "    }", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "cpp_lesson1_ex8_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "cpp_lesson1_ex8_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "cpp_lesson1_ex8_comment3" }
            },
            expectedOutput = new List<string> { "Perfect" }
        });

        // Exercise 9: if-else statement
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson1_ex9_title",
            slideKeyPrefix = "cpp_lesson1_ex9",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "int main() {", "    // ageに10を入れる", "    int age = 10;", "    // 20さい以上かどうかで分ける", "    if (age >= 20) {", "        std::cout << \"Adult\" << std::endl;", "    } else {", "        // それ以外の場合", "        std::cout << \"Minor\" << std::endl;", "    }", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "cpp_lesson1_ex9_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "cpp_lesson1_ex9_comment2" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "cpp_lesson1_ex9_comment3" }
            },
            expectedOutput = new List<string> { "Minor" }
        });

        // Exercise 10: Logical operators (&&, ||)
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson1_ex10_title",
            slideKeyPrefix = "cpp_lesson1_ex10",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "int main() {", "    // scoreに85を入れる", "    int score = 85;", "    // 80以上 かつ 100以下 ならメッセージを出す", "    if (score >= 80 && score <= 100) {", "        std::cout << \"Pass\" << std::endl;", "    }", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "cpp_lesson1_ex10_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "cpp_lesson1_ex10_comment2" }
            },
            expectedOutput = new List<string> { "Pass" }
        });

        // Exercise 11: Range-based for loop
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson1_ex11_title",
            slideKeyPrefix = "cpp_lesson1_ex11",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <vector>", "int main() {", "    // 配列を作る", "    std::vector<int> nums = {1, 2, 3};", "    // 全部取り出すループ", "    for (int n : nums) {", "        std::cout << n << std::endl;", "    }", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "cpp_lesson1_ex11_comment1" }
            },
            expectedOutput = new List<string> { "1", "2", "3" }
        });

        // Exercise 12: Dictionary 'map'
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson1_ex12_title",
            slideKeyPrefix = "cpp_lesson1_ex12",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <map>", "#include <string>", "int main() {", "    // 辞書のはこを作る", "    std::map<std::string, int> scores;", "    // 名前（キー）として登録します", "    scores[\"Math\"] = 90;", "    // 同じキー名でデータを取り出します", "    std::cout << scores[\"Math\"] << std::endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "cpp_lesson1_ex12_comment1" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "cpp_lesson1_ex12_comment2" }
            },
            expectedOutput = new List<string> { "90" }
        });

        // Exercise 13: Create your own function
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson1_ex13_title",
            slideKeyPrefix = "cpp_lesson1_ex13",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "void greet() {", "    std::cout << \"Hello\" << std::endl;", "}", "int main() {", "    // 関数を実行する", "    greet();", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "cpp_lesson1_ex13_comment1" }
            },
            expectedOutput = new List<string> { "Hello" }
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: C++ II - Classes & OOP ====================
        var lesson2 = new Lesson { titleKey = "cpp_lesson2_title" };

        // Exercise 1: Create a class
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson2_ex1_title",
            slideKeyPrefix = "cpp_lesson2_ex1",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <string>", "class Cat {", "public:", "    std::string name;", "};", "int main() {", "    Cat c;", "    c.name = \"Tama\";", "    std::cout << c.name << std::endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "cpp_lesson2_ex1_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "cpp_lesson2_ex1_comment2" }
            },
            expectedOutput = new List<string> { "Tama" }
        });

        // Exercise 2: Using constructors
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson2_ex2_title",
            slideKeyPrefix = "cpp_lesson2_ex2",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "class Counter {", "public:", "    int count;", "    // コンストラクタはクラス名と同じ", "    Counter(int c) {", "        // 引数で受け取った値を設定します", "        count = c;", "    }", "};", "int main() {", "    Counter cnt(5);", "    std::cout << cnt.count << std::endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "cpp_lesson2_ex2_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "cpp_lesson2_ex2_comment2" }
            },
            expectedOutput = new List<string> { "5" }
        });

        // Exercise 3: Member functions
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson2_ex3_title",
            slideKeyPrefix = "cpp_lesson2_ex3",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "class Rect {", "public:", "    int w;", "    int h;", "    int area() {", "        // * を使って掛け算します", "        return w * h;", "    }", "};", "int main() {", "    Rect r;", "    r.w = 3;", "    r.h = 4;", "    // r.area() でメンバ関数を呼びます", "    std::cout << r.area() << std::endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "cpp_lesson2_ex3_comment1" },
                new LocalizedComment { lineIndex = 14, commentPrefix = "//", localizationKey = "cpp_lesson2_ex3_comment2" }
            },
            expectedOutput = new List<string> { "12" }
        });

        // Exercise 4: References
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson2_ex4_title",
            slideKeyPrefix = "cpp_lesson2_ex4",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "int main() {", "    int num = 5;", "    // & を使って参照を作ります", "    int& ref = num;", "    // ref を変えると num も変わります", "    ref = 100;", "    std::cout << num << std::endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "cpp_lesson2_ex4_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "cpp_lesson2_ex4_comment2" }
            },
            expectedOutput = new List<string> { "100" }
        });

        // Exercise 5: Inheritance
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson2_ex5_title",
            slideKeyPrefix = "cpp_lesson2_ex5",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "class Vehicle {", "public:", "    void move() {", "        std::cout << \"moving\" << std::endl;", "    }", "};", "// public を使って継承します", "class Car : public Vehicle {", "};", "int main() {", "    Car c;", "    // Car は move() を使えます", "    c.move();", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "cpp_lesson2_ex5_comment1" },
                new LocalizedComment { lineIndex = 12, commentPrefix = "//", localizationKey = "cpp_lesson2_ex5_comment2" }
            },
            expectedOutput = new List<string> { "moving" }
        });

        // Exercise 6: Virtual functions & override
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson2_ex6_title",
            slideKeyPrefix = "cpp_lesson2_ex6",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "class Shape {", "public:", "    // virtual をつけて仮想関数にします", "    virtual void draw() {", "        std::cout << \"shape\" << std::endl;", "    }", "};", "class Circle : public Shape {", "public:", "    // override で上書きします", "    void draw() override {", "        std::cout << \"circle\" << std::endl;", "    }", "};", "int main() {", "    Circle c;", "    c.draw();", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "cpp_lesson2_ex6_comment1" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "cpp_lesson2_ex6_comment2" }
            },
            expectedOutput = new List<string> { "circle" }
        });

        // Exercise 7: Templates
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson2_ex7_title",
            slideKeyPrefix = "cpp_lesson2_ex7",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "// T を型パラメータとして定義します", "template<typename T>", "T bigger(T a, T b) {", "    if (a > b) return a;", "    return b;", "}", "int main() {", "    // int型で呼び出されます", "    std::cout << bigger(3, 7) << std::endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "cpp_lesson2_ex7_comment1" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "cpp_lesson2_ex7_comment2" }
            },
            expectedOutput = new List<string> { "7" }
        });

        // Exercise 8: Lambda expressions
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson2_ex8_title",
            slideKeyPrefix = "cpp_lesson2_ex8",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "int main() {", "    // { でラムダ式の本体を始めます", "    auto square = [](int x) {", "        return x * x;", "    };", "    // square(4) で 16 が出力されます", "    std::cout << square(4) << std::endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "cpp_lesson2_ex8_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "cpp_lesson2_ex8_comment2" }
            },
            expectedOutput = new List<string> { "16" }
        });

        // Exercise 9: Smart pointer unique_ptr
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson2_ex9_title",
            slideKeyPrefix = "cpp_lesson2_ex9",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <memory>", "int main() {", "    // make_unique でスマートポインタを作ります", "    auto ptr = std::make_unique<int>(100);", "    // *ptr で中身にアクセスします", "    std::cout << *ptr << std::endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "cpp_lesson2_ex9_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "cpp_lesson2_ex9_comment2" }
            },
            expectedOutput = new List<string> { "100" }
        });

        // Exercise 10: Exception handling try-catch
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson2_ex10_title",
            slideKeyPrefix = "cpp_lesson2_ex10",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "int main() {", "    try {", "        // throw で例外を投げます", "        throw 404;", "    // catch で例外を捕まえます", "    } catch (int e) {", "        std::cout << e << std::endl;", "    }", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "cpp_lesson2_ex10_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "cpp_lesson2_ex10_comment2" }
            },
            expectedOutput = new List<string> { "404" }
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: C++ III - Modern C++ ====================
        var lesson3 = new Lesson { titleKey = "cpp_lesson3_title" };

        // Exercise 1: Type inference with auto
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson3_ex1_title",
            slideKeyPrefix = "cpp_lesson3_ex1",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "using namespace std;", "", "int main() {", "    // auto で型を推論させます", "    auto x = 100;", "    auto y = 2.5;", "    cout << x * y << endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "cpp_lesson3_ex1_comment1" }
            },
            expectedOutput = new List<string> { "250" }
        });

        // Exercise 2: Range-based for loop
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson3_ex2_title",
            slideKeyPrefix = "cpp_lesson3_ex2",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <vector>", "using namespace std;", "", "int main() {", "    // vector で配列を作成", "    vector<int> nums = {10, 20, 30};", "    // : で範囲for文を使います", "    for (int n : nums) {", "        cout << n << endl;", "    }", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "cpp_lesson3_ex2_comment1" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "cpp_lesson3_ex2_comment2" }
            },
            expectedOutput = new List<string> { "10", "20", "30" }
        });

        // Exercise 3: nullptr
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson3_ex3_title",
            slideKeyPrefix = "cpp_lesson3_ex3",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "using namespace std;", "", "int main() {", "    // nullptr で型安全なヌルを表します", "    int* p = nullptr;", "    // nullptr と比較", "    if (p == nullptr) {", "        cout << \"null\" << endl;", "    }", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "cpp_lesson3_ex3_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "cpp_lesson3_ex3_comment2" }
            },
            expectedOutput = new List<string> { "null" }
        });

        // Exercise 4: constexpr
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson3_ex4_title",
            slideKeyPrefix = "cpp_lesson3_ex4",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "using namespace std;", "", "// constexpr でコンパイル時計算を可能にします", "constexpr int cube(int x) {", "    return x * x * x;", "}", "", "int main() {", "    constexpr int val = cube(3);", "    cout << val << endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "cpp_lesson3_ex4_comment1" }
            },
            expectedOutput = new List<string> { "27" }
        });

        // Exercise 5: Initializer lists
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson3_ex5_title",
            slideKeyPrefix = "cpp_lesson3_ex5",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <vector>", "using namespace std;", "", "int main() {", "    // { で初期化子リスト", "    vector<int> v{1, 2, 3, 4, 5};", "    // : で範囲for文", "    for (int n : v) cout << n << \" \";", "    cout << endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "cpp_lesson3_ex5_comment1" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "cpp_lesson3_ex5_comment2" }
            },
            expectedOutput = new List<string> { "1 2 3 4 5" }
        });

        // Exercise 6: shared_ptr
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson3_ex6_title",
            slideKeyPrefix = "cpp_lesson3_ex6",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <memory>", "using namespace std;", "", "int main() {", "    // make_shared で共有ポインタを作成します", "    auto p = make_shared<int>(100);", "    // *p で中身にアクセス", "    cout << *p << endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "cpp_lesson3_ex6_comment1" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "cpp_lesson3_ex6_comment2" }
            },
            expectedOutput = new List<string> { "100" }
        });

        // Exercise 7: std::move
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson3_ex7_title",
            slideKeyPrefix = "cpp_lesson3_ex7",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <string>", "#include <utility>", "using namespace std;", "", "int main() {", "    // string で文字列を作成", "    string s1 = \"Hello\";", "    // move で所有権を移動", "    string s2 = move(s1);", "    cout << s2 << endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "cpp_lesson3_ex7_comment1" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "cpp_lesson3_ex7_comment2" }
            },
            expectedOutput = new List<string> { "Hello" }
        });

        // Exercise 8: std::optional
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson3_ex8_title",
            slideKeyPrefix = "cpp_lesson3_ex8",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <optional>", "using namespace std;", "", "int main() {", "    // optional で値を保持", "    optional<int> opt = 42;", "    // has_value で値の有無をチェックします", "    if (opt.has_value()) {", "        cout << opt.value() << endl;", "    }", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "cpp_lesson3_ex8_comment1" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "cpp_lesson3_ex8_comment2" }
            },
            expectedOutput = new List<string> { "42" }
        });

        // Exercise 9: std::array
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson3_ex9_title",
            slideKeyPrefix = "cpp_lesson3_ex9",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <array>", "using namespace std;", "", "int main() {", "    // array で固定長配列を使います", "    array<int, 3> arr = {10, 20, 30};", "    // : で範囲for文", "    for (int n : arr) cout << n << \" \";", "    cout << endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "cpp_lesson3_ex9_comment1" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "cpp_lesson3_ex9_comment2" }
            },
            expectedOutput = new List<string> { "10 20 30" }
        });

        // Exercise 10: Lambda captures
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson3_ex10_title",
            slideKeyPrefix = "cpp_lesson3_ex10",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "using namespace std;", "", "int main() {", "    // int で変数を宣言", "    int x = 5;", "    // [x] で x をコピーキャプチャします", "    auto f = [x]() { return x * x; };", "    cout << f() << endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "cpp_lesson3_ex10_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "cpp_lesson3_ex10_comment2" }
            },
            expectedOutput = new List<string> { "25" }
        });

        lessons.Add(lesson3);

        // ==================== LESSON 4: C++ IV - STL & Advanced ====================
        var lesson4 = new Lesson { titleKey = "cpp_lesson4_title" };

        // Exercise 1: unique_ptr
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson4_ex1_title",
            slideKeyPrefix = "cpp_lesson4_ex1",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <memory>", "using namespace std;", "", "int main() {", "    // make_unique で排他的ポインタを作成します", "    auto p = make_unique<int>(99);", "    // *p で中身にアクセス", "    cout << *p << endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "cpp_lesson4_ex1_comment1" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "cpp_lesson4_ex1_comment2" }
            },
            expectedOutput = new List<string> { "99" }
        });

        // Exercise 2: std::variant
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson4_ex2_title",
            slideKeyPrefix = "cpp_lesson4_ex2",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <variant>", "using namespace std;", "", "int main() {", "    // variant で複数型のうち1つを保持", "    variant<int, double> v = 3.14;", "    // get<型> で値を取得します", "    cout << get<double>(v) << endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "cpp_lesson4_ex2_comment1" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "cpp_lesson4_ex2_comment2" }
            },
            expectedOutput = new List<string> { "3.14" }
        });

        // Exercise 3: Structured bindings
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson4_ex3_title",
            slideKeyPrefix = "cpp_lesson4_ex3",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <tuple>", "using namespace std;", "", "int main() {", "    // tuple で複数の値をまとめる", "    tuple<int, double, string> t{1, 2.5, \"hi\"};", "    // auto で構造化束縛を使います", "    auto [a, b, c] = t;", "    cout << a << \" \" << b << \" \" << c << endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "cpp_lesson4_ex3_comment1" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "cpp_lesson4_ex3_comment2" }
            },
            expectedOutput = new List<string> { "1 2.5 hi" }
        });

        // Exercise 4: std::string_view
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson4_ex4_title",
            slideKeyPrefix = "cpp_lesson4_ex4",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <string_view>", "using namespace std;", "", "// string_view で文字列ビューを受け取ります", "void greet(string_view name) {", "    cout << \"Hello, \" << name << endl;", "}", "", "int main() {", "    // greet で関数を呼び出し", "    greet(\"World\");", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "cpp_lesson4_ex4_comment1" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "cpp_lesson4_ex4_comment2" }
            },
            expectedOutput = new List<string> { "Hello, World" }
        });

        // Exercise 5: std::transform
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson4_ex5_title",
            slideKeyPrefix = "cpp_lesson4_ex5",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <vector>", "#include <algorithm>", "using namespace std;", "", "int main() {", "    // vector で配列を作成", "    vector<int> v = {1, 2, 3};", "    // transform で各要素を変換します", "    transform(v.begin(), v.end(), v.begin(), [](int x) { return x * 10; });", "    for (int n : v) cout << n << \" \";", "    cout << endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "cpp_lesson4_ex5_comment1" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "cpp_lesson4_ex5_comment2" }
            },
            expectedOutput = new List<string> { "10 20 30" }
        });

        // Exercise 6: std::accumulate
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson4_ex6_title",
            slideKeyPrefix = "cpp_lesson4_ex6",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <vector>", "#include <numeric>", "using namespace std;", "", "int main() {", "    // vector で配列を作成", "    vector<int> v = {1, 2, 3, 4, 5};", "    // accumulate で要素を集約します", "    int sum = accumulate(v.begin(), v.end(), 0);", "    cout << sum << endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "cpp_lesson4_ex6_comment1" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "cpp_lesson4_ex6_comment2" }
            },
            expectedOutput = new List<string> { "15" }
        });

        // Exercise 7: std::find_if
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson4_ex7_title",
            slideKeyPrefix = "cpp_lesson4_ex7",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <vector>", "#include <algorithm>", "using namespace std;", "", "int main() {", "    // vector で配列を作成", "    vector<int> v = {1, 2, 3, 4, 5};", "    // find_if で条件に合う要素を検索します", "    auto it = find_if(v.begin(), v.end(), [](int x) { return x > 3; });", "    cout << *it << endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "cpp_lesson4_ex7_comment1" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "cpp_lesson4_ex7_comment2" }
            },
            expectedOutput = new List<string> { "4" }
        });

        // Exercise 8: std::sort with lambda
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson4_ex8_title",
            slideKeyPrefix = "cpp_lesson4_ex8",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <vector>", "#include <algorithm>", "using namespace std;", "", "int main() {", "    // vector で配列を作成", "    vector<int> v = {3, 1, 4, 1, 5};", "    // sort でカスタム順序でソートします", "    sort(v.begin(), v.end(), [](int a, int b) { return a > b; });", "    for (int n : v) cout << n << \" \";", "    cout << endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "cpp_lesson4_ex8_comment1" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "cpp_lesson4_ex8_comment2" }
            },
            expectedOutput = new List<string> { "5 4 3 1 1" }
        });

        // Exercise 9: try-catch exception handling
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson4_ex9_title",
            slideKeyPrefix = "cpp_lesson4_ex9",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "#include <stdexcept>", "using namespace std;", "", "int main() {", "    try {", "        // throw で例外を投げます", "        throw runtime_error(\"Oops!\");", "    // catch で例外を捕まえます", "    } catch (const exception& e) {", "        cout << e.what() << endl;", "    }", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "cpp_lesson4_ex9_comment1" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "cpp_lesson4_ex9_comment2" }
            },
            expectedOutput = new List<string> { "Oops!" }
        });

        // Exercise 10: noexcept specifier
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "cpp_lesson4_ex10_title",
            slideKeyPrefix = "cpp_lesson4_ex10",
            slideCount = 2,
            correctLines = new List<string> { "#include <iostream>", "using namespace std;", "", "// noexcept で例外を投げないことを宣言します", "int add(int a, int b) noexcept {", "    // + で足し算", "    return a + b;", "}", "", "int main() {", "    cout << add(10, 20) << endl;", "    return 0;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "cpp_lesson4_ex10_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "cpp_lesson4_ex10_comment2" }
            },
            expectedOutput = new List<string> { "30" }
        });

        lessons.Add(lesson4);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;

        Debug.Log($"[LessonManager] Initialized {lessons.Count} C++ lessons");
    }


    /// <summary>
    /// Initialize TypeScript lessons from senkou-code data
    /// </summary>
    private void InitializeTypeScriptLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: TypeScript I ====================
        var lesson1 = new Lesson { titleKey = "typescript_lesson1_title" };

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex1_title",
            slideKeyPrefix = "typescript_lesson1_ex1",
            slideCount = 3,
            correctLines = new List<string> { "const message: string = 'Hello TS';", "console.log(message);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex1_comment1" }
            },
            expectedOutput = new List<string> { "Hello TS" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex2_title",
            slideKeyPrefix = "typescript_lesson1_ex2",
            slideCount = 2,
            correctLines = new List<string> { "const x: number = 10;", "const y: number = 5;", "console.log(x + y);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex2_comment1" }
            },
            expectedOutput = new List<string> { "15" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex3_title",
            slideKeyPrefix = "typescript_lesson1_ex3",
            slideCount = 2,
            correctLines = new List<string> { "console.log(10 % 3);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex3_comment1" }
            },
            expectedOutput = new List<string> { "1" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex4_title",
            slideKeyPrefix = "typescript_lesson1_ex4",
            slideCount = 2,
            correctLines = new List<string> { "let score: number = 50;", "score += 10;", "console.log(score);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex4_comment1" }
            },
            expectedOutput = new List<string> { "60" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex5_title",
            slideKeyPrefix = "typescript_lesson1_ex5",
            slideCount = 2,
            correctLines = new List<string> { "const age: number = 10;", "console.log(`私は${age}歳です`);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex5_comment1" }
            },
            expectedOutput = new List<string> { "私は10歳です" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex6_title",
            slideKeyPrefix = "typescript_lesson1_ex6",
            slideCount = 2,
            correctLines = new List<string> { "const colors: string[] = ['あか', 'あお'];", "console.log(colors[1]);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex6_comment1" }
            },
            expectedOutput = new List<string> { "あお" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex7_title",
            slideKeyPrefix = "typescript_lesson1_ex7",
            slideCount = 2,
            correctLines = new List<string> { "const isAdult: boolean = true;", "if (isAdult) {", "    console.log('おとなです');", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex7_comment1" }
            },
            expectedOutput = new List<string> { "おとなです" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex8_title",
            slideKeyPrefix = "typescript_lesson1_ex8",
            slideCount = 2,
            correctLines = new List<string> { "const score: number = 75;", "if (score >= 80) {", "    console.log('ごうかく');", "} else {", "    console.log('ざんねん');", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex8_comment1" }
            },
            expectedOutput = new List<string> { "ざんねん" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex9_title",
            slideKeyPrefix = "typescript_lesson1_ex9",
            slideCount = 2,
            correctLines = new List<string> { "const score: number = 85;", "if (score >= 80 && score <= 100) {", "    console.log('ごうかく');", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex9_comment1" }
            },
            expectedOutput = new List<string> { "ごうかく" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex10_title",
            slideKeyPrefix = "typescript_lesson1_ex10",
            slideCount = 3,
            correctLines = new List<string> { "const names: string[] = ['たろう', 'はなこ'];", "for (const name of names) {", "    console.log(name);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex10_comment1" }
            },
            expectedOutput = new List<string> { "たろう", "はなこ" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex11_title",
            slideKeyPrefix = "typescript_lesson1_ex11",
            slideCount = 2,
            correctLines = new List<string> { "type User = { name: string };", "const user: User = { name: 'たろう' };", "console.log(user.name);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex11_comment1" }
            },
            expectedOutput = new List<string> { "たろう" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex12_title",
            slideKeyPrefix = "typescript_lesson1_ex12",
            slideCount = 2,
            correctLines = new List<string> { "function greet(name: string) {", "    console.log(`こんにちは、${name}`);", "}", "greet('TypeScript');" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex12_comment1" }
            },
            expectedOutput = new List<string> { "こんにちは、TypeScript" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex13_title",
            slideKeyPrefix = "typescript_lesson1_ex13",
            slideCount = 2,
            correctLines = new List<string> { "function showDate(): void {", "    console.log('今日の日付');", "}", "showDate();" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex13_comment1" }
            },
            expectedOutput = new List<string> { "今日の日付" }
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: TypeScript II ====================
        var lesson2 = new Lesson { titleKey = "typescript_lesson2_title" };

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex1_title",
            slideKeyPrefix = "typescript_lesson2_ex1",
            slideCount = 2,
            correctLines = new List<string> { "function show(value: string | number): void {", "    console.log(value);", "}", "show('Hello');", "show(42);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex1_comment1" }
            },
            expectedOutput = new List<string> { "Hello", "42" }
        });

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex2_title",
            slideKeyPrefix = "typescript_lesson2_ex2",
            slideCount = 2,
            correctLines = new List<string> { "interface Person {", "    name: string;", "    age: number;", "}", "const p: Person = { name: 'Alice', age: 25 };", "console.log(p.name);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex2_comment1" }
            },
            expectedOutput = new List<string> { "Alice" }
        });

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex3_title",
            slideKeyPrefix = "typescript_lesson2_ex3",
            slideCount = 2,
            correctLines = new List<string> { "interface Profile {", "    name: string;", "    nickname?: string;", "}", "const prof: Profile = { name: 'Bob' };", "console.log(prof.name);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex3_comment1" }
            },
            expectedOutput = new List<string> { "Bob" }
        });

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex4_title",
            slideKeyPrefix = "typescript_lesson2_ex4",
            slideCount = 2,
            correctLines = new List<string> { "type Score = number;", "const math: Score = 85;", "const english: Score = 90;", "console.log(math + english);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex4_comment1" }
            },
            expectedOutput = new List<string> { "175" }
        });

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex5_title",
            slideKeyPrefix = "typescript_lesson2_ex5",
            slideCount = 2,
            correctLines = new List<string> { "interface Item {", "    readonly id: number;", "    name: string;", "}", "const item: Item = { id: 1, name: 'Apple' };", "console.log(item.id);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex5_comment1" }
            },
            expectedOutput = new List<string> { "1" }
        });

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex6_title",
            slideKeyPrefix = "typescript_lesson2_ex6",
            slideCount = 2,
            correctLines = new List<string> { "enum Day {", "    Sun,", "    Mon,", "    Tue", "}", "const today: Day = Day.Mon;", "console.log(today);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex6_comment1" }
            },
            expectedOutput = new List<string> { "1" }
        });

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex7_title",
            slideKeyPrefix = "typescript_lesson2_ex7",
            slideCount = 2,
            correctLines = new List<string> { "function wrap<T>(value: T): T[] {", "    return [value];", "}", "const arr = wrap(5);", "console.log(arr);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex7_comment1" }
            },
            expectedOutput = new List<string> { "[ 5 ]" }
        });

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex8_title",
            slideKeyPrefix = "typescript_lesson2_ex8",
            slideCount = 2,
            correctLines = new List<string> { "interface Container<T> {", "    item: T;", "}", "const box: Container<string> = { item: 'Hello' };", "console.log(box.item);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex8_comment1" }
            },
            expectedOutput = new List<string> { "Hello" }
        });

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex9_title",
            slideKeyPrefix = "typescript_lesson2_ex9",
            slideCount = 2,
            correctLines = new List<string> { "const point = { x: 10, y: 20 };", "const point2: typeof point = { x: 5, y: 15 };", "console.log(point2.x);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex9_comment1" }
            },
            expectedOutput = new List<string> { "5" }
        });

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex10_title",
            slideKeyPrefix = "typescript_lesson2_ex10",
            slideCount = 2,
            correctLines = new List<string> { "interface Base {", "    id: number;", "}", "interface User extends Base {", "    name: string;", "}", "const u: User = { id: 1, name: 'Taro' };", "console.log(u.name);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex10_comment1" }
            },
            expectedOutput = new List<string> { "Taro" }
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: TypeScript III ====================
        var lesson3 = new Lesson { titleKey = "typescript_lesson3_title" };

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex1_title",
            slideKeyPrefix = "typescript_lesson3_ex1",
            slideCount = 2,
            correctLines = new List<string> { "interface Config { host: string; port: number; }", "function update(config: Config, patch: Partial<Config>): Config {", "  return { ...config, ...patch };", "}", "const cfg = { host: 'localhost', port: 3000 };", "console.log(update(cfg, { port: 8080 }).port);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex1_comment1" }
            },
            expectedOutput = new List<string> { "8080" }
        });

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex2_title",
            slideKeyPrefix = "typescript_lesson3_ex2",
            slideCount = 2,
            correctLines = new List<string> { "interface Options { debug?: boolean; verbose?: boolean; }", "function init(opts: Required<Options>) {", "  console.log(opts.debug);", "}", "init({ debug: true, verbose: false });" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex2_comment1" }
            },
            expectedOutput = new List<string> { "true" }
        });

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex3_title",
            slideKeyPrefix = "typescript_lesson3_ex3",
            slideCount = 2,
            correctLines = new List<string> { "interface Product { id: number; name: string; price: number; }", "type ProductName = Pick<Product, 'name'>;", "const item: ProductName = { name: 'Apple' };", "console.log(item.name);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex3_comment1" }
            },
            expectedOutput = new List<string> { "Apple" }
        });

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex4_title",
            slideKeyPrefix = "typescript_lesson3_ex4",
            slideCount = 2,
            correctLines = new List<string> { "interface User { id: number; name: string; secret: string; }", "type SafeUser = Omit<User, 'secret'>;", "const user: SafeUser = { id: 1, name: 'Alice' };", "console.log(user.name);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex4_comment1" }
            },
            expectedOutput = new List<string> { "Alice" }
        });

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex5_title",
            slideKeyPrefix = "typescript_lesson3_ex5",
            slideCount = 2,
            correctLines = new List<string> { "type Fruit = 'apple' | 'banana';", "type Prices = Record<Fruit, number>;", "const prices: Prices = { apple: 100, banana: 80 };", "console.log(prices.apple);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex5_comment1" }
            },
            expectedOutput = new List<string> { "100" }
        });

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex6_title",
            slideKeyPrefix = "typescript_lesson3_ex6",
            slideCount = 2,
            correctLines = new List<string> { "function createPoint() { return { x: 10, y: 20 }; }", "type Point = ReturnType<typeof createPoint>;", "const p: Point = { x: 5, y: 15 };", "console.log(p.x + p.y);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex6_comment1" }
            },
            expectedOutput = new List<string> { "20" }
        });

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex7_title",
            slideKeyPrefix = "typescript_lesson3_ex7",
            slideCount = 2,
            correctLines = new List<string> { "type IsArray<T> = T extends any[] ? true : false;", "type A = IsArray<number[]>;", "type B = IsArray<string>;", "const a: A = true;", "const b: B = false;", "console.log(a, b);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex7_comment1" }
            },
            expectedOutput = new List<string> { "true false" }
        });

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex8_title",
            slideKeyPrefix = "typescript_lesson3_ex8",
            slideCount = 2,
            correctLines = new List<string> { "type Unwrap<T> = T extends Promise<infer U> ? U : T;", "type A = Unwrap<Promise<string>>;", "type B = Unwrap<number>;", "const a: A = 'hello';", "const b: B = 42;", "console.log(a, b);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex8_comment1" }
            },
            expectedOutput = new List<string> { "hello 42" }
        });

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex9_title",
            slideKeyPrefix = "typescript_lesson3_ex9",
            slideCount = 2,
            correctLines = new List<string> { "interface Person { name: string; age: number; }", "function getProperty<K extends keyof Person>(p: Person, key: K) { return p[key]; }", "const person = { name: 'Bob', age: 30 };", "console.log(getProperty(person, 'name'));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex9_comment1" }
            },
            expectedOutput = new List<string> { "Bob" }
        });

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex10_title",
            slideKeyPrefix = "typescript_lesson3_ex10",
            slideCount = 2,
            correctLines = new List<string> { "type Optional<T> = { [K in keyof T]?: T[K]; };", "interface Config { host: string; port: number; }", "const partial: Optional<Config> = { host: 'localhost' };", "console.log(partial.host);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex10_comment1" }
            },
            expectedOutput = new List<string> { "localhost" }
        });

        lessons.Add(lesson3);

        // ==================== LESSON 4: TypeScript IV ====================
        var lesson4 = new Lesson { titleKey = "typescript_lesson4_title" };

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex1_title",
            slideKeyPrefix = "typescript_lesson4_ex1",
            slideCount = 2,
            correctLines = new List<string> { "function isNumber(x: unknown): x is number {", "  return typeof x === 'number';", "}", "const value: unknown = 42;", "if (isNumber(value)) { console.log(value * 2); }" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex1_comment1" }
            },
            expectedOutput = new List<string> { "84" }
        });

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex2_title",
            slideKeyPrefix = "typescript_lesson4_ex2",
            slideCount = 2,
            correctLines = new List<string> { "type Car = { drive: () => void };", "type Boat = { sail: () => void };", "function operate(vehicle: Car | Boat): void {", "  if ('drive' in vehicle) { console.log('Driving'); } else { console.log('Sailing'); }", "}", "operate({ drive: () => {} });" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex2_comment1" }
            },
            expectedOutput = new List<string> { "Driving" }
        });

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex3_title",
            slideKeyPrefix = "typescript_lesson4_ex3",
            slideCount = 2,
            correctLines = new List<string> { "type Success = { status: 'success'; data: string };", "type Failure = { status: 'failure'; error: string };", "type Result = Success | Failure;", "function handle(result: Result): void {", "  switch (result.status) { case 'success': console.log(result.data); break; case 'failure': console.log(result.error); break; }", "}", "handle({ status: 'success', data: 'OK' });" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex3_comment1" }
            },
            expectedOutput = new List<string> { "OK" }
        });

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex4_title",
            slideKeyPrefix = "typescript_lesson4_ex4",
            slideCount = 2,
            correctLines = new List<string> { "type Color = 'red' | 'green' | 'blue';", "function getHex(color: Color): string {", "  switch (color) { case 'red': return '#ff0000'; case 'green': return '#00ff00'; case 'blue': return '#0000ff'; default: const _exhaustive: never = color; return _exhaustive; }", "}", "console.log(getHex('red'));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex4_comment1" }
            },
            expectedOutput = new List<string> { "#ff0000" }
        });

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex5_title",
            slideKeyPrefix = "typescript_lesson4_ex5",
            slideCount = 2,
            correctLines = new List<string> { "type Method = 'get' | 'post';", "type Endpoint = '/users' | '/posts';", "type Route = `${Method} ${Endpoint}`;", "const route: Route = 'get /users';", "console.log(route);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex5_comment1" }
            },
            expectedOutput = new List<string> { "get /users" }
        });

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex6_title",
            slideKeyPrefix = "typescript_lesson4_ex6",
            slideCount = 2,
            correctLines = new List<string> { "type Status = 'pending' | 'success' | 'error' | 'cancelled';", "type ActiveStatus = Exclude<Status, 'cancelled'>;", "const status: ActiveStatus = 'pending';", "console.log(status);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex6_comment1" }
            },
            expectedOutput = new List<string> { "pending" }
        });

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex7_title",
            slideKeyPrefix = "typescript_lesson4_ex7",
            slideCount = 2,
            correctLines = new List<string> { "type Event = 'click' | 'scroll' | 'mouseover' | 'keydown';", "type MouseEvent = Extract<Event, 'click' | 'scroll' | 'mouseover'>;", "const event: MouseEvent = 'click';", "console.log(event);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex7_comment1" }
            },
            expectedOutput = new List<string> { "click" }
        });

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex8_title",
            slideKeyPrefix = "typescript_lesson4_ex8",
            slideCount = 2,
            correctLines = new List<string> { "type MaybeString = string | null | undefined;", "type DefiniteString = NonNullable<MaybeString>;", "const text: DefiniteString = 'Hello';", "console.log(text);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex8_comment1" }
            },
            expectedOutput = new List<string> { "Hello" }
        });

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex9_title",
            slideKeyPrefix = "typescript_lesson4_ex9",
            slideCount = 2,
            correctLines = new List<string> { "function greet(name: string, age: number): void { console.log(`${name} is ${age}`); }", "type GreetParams = Parameters<typeof greet>;", "const args: GreetParams = ['Taro', 25];", "greet(...args);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex9_comment1" }
            },
            expectedOutput = new List<string> { "Taro is 25" }
        });

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex10_title",
            slideKeyPrefix = "typescript_lesson4_ex10",
            slideCount = 2,
            correctLines = new List<string> { "type AsyncResult = Promise<{ data: string }>;", "type Result = Awaited<AsyncResult>;", "const result: Result = { data: 'success' };", "console.log(result.data);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex10_comment1" }
            },
            expectedOutput = new List<string> { "success" }
        });

        lessons.Add(lesson4);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;

        Debug.Log($"[LessonManager] Initialized {lessons.Count} TypeScript lessons");
    }

    /// <summary>
    /// Initialize Assembly lessons
    /// </summary>
    private void InitializeAssemblyLessons()
    {
        // ==================== LESSON 1: Assembly I - Getting Started ====================
        var lesson1 = new Lesson { titleKey = "assembly_lesson1_title" };

        // Exercise 1: Display a message
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson1_ex1_title",
            slideKeyPrefix = "assembly_lesson1_ex1",
            slideCount = 2,
            correctLines = new List<string> { "section .data", "  msg db \"Hello\", 0xA", "", "section .text", "  global _start", "", "_start:", "  mov rax, 1", "  mov rdi, 1", "  mov rsi, msg", "  mov rdx, 6", "  syscall", "", "  mov rax, 60", "  xor rdi, rdi", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = ";", localizationKey = "assembly_lesson1_ex1_comment1" },
                new LocalizedComment { lineIndex = 1, commentPrefix = ";", localizationKey = "assembly_lesson1_ex1_comment2" },
                new LocalizedComment { lineIndex = 3, commentPrefix = ";", localizationKey = "assembly_lesson1_ex1_comment3" },
                new LocalizedComment { lineIndex = 4, commentPrefix = ";", localizationKey = "assembly_lesson1_ex1_comment4" },
                new LocalizedComment { lineIndex = 6, commentPrefix = ";", localizationKey = "assembly_lesson1_ex1_comment5" },
                new LocalizedComment { lineIndex = 7, commentPrefix = ";", localizationKey = "assembly_lesson1_ex1_comment6" },
                new LocalizedComment { lineIndex = 8, commentPrefix = ";", localizationKey = "assembly_lesson1_ex1_comment7" },
                new LocalizedComment { lineIndex = 9, commentPrefix = ";", localizationKey = "assembly_lesson1_ex1_comment8" },
                new LocalizedComment { lineIndex = 10, commentPrefix = ";", localizationKey = "assembly_lesson1_ex1_comment9" },
                new LocalizedComment { lineIndex = 11, commentPrefix = ";", localizationKey = "assembly_lesson1_ex1_comment10" },
                new LocalizedComment { lineIndex = 13, commentPrefix = ";", localizationKey = "assembly_lesson1_ex1_comment11" },
                new LocalizedComment { lineIndex = 14, commentPrefix = ";", localizationKey = "assembly_lesson1_ex1_comment12" },
                new LocalizedComment { lineIndex = 15, commentPrefix = ";", localizationKey = "assembly_lesson1_ex1_comment13" }
            },
            expectedOutput = new List<string> { "Hello" }
        });

        // Exercise 2: Display a different message
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson1_ex2_title",
            slideKeyPrefix = "assembly_lesson1_ex2",
            slideCount = 2,
            correctLines = new List<string> { "section .data", "  msg db \"Hi\", 0xA", "", "section .text", "  global _start", "", "_start:", "  mov rax, 1", "  mov rdi, 1", "  mov rsi, msg", "  mov rdx, 3", "  syscall", "", "  mov rax, 60", "  xor rdi, rdi", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = ";", localizationKey = "assembly_lesson1_ex2_comment1" },
                new LocalizedComment { lineIndex = 10, commentPrefix = ";", localizationKey = "assembly_lesson1_ex2_comment2" }
            },
            expectedOutput = new List<string> { "Hi" }
        });

        // Exercise 3: Return an exit code
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson1_ex3_title",
            slideKeyPrefix = "assembly_lesson1_ex3",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "", "_start:", "  mov rax, 60", "  xor rdi, rdi", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = ";", localizationKey = "assembly_lesson1_ex3_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = ";", localizationKey = "assembly_lesson1_ex3_comment2" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 4: Add numbers
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson1_ex4_title",
            slideKeyPrefix = "assembly_lesson1_ex4",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "", "_start:", "  mov rax, 10", "  add rax, 5", "", "  mov rdi, rax", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = ";", localizationKey = "assembly_lesson1_ex4_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 5: Subtract numbers
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson1_ex5_title",
            slideKeyPrefix = "assembly_lesson1_ex5",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "", "_start:", "  mov rax, 20", "  sub rax, 8", "", "  mov rdi, rax", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = ";", localizationKey = "assembly_lesson1_ex5_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 6: Copy between registers
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson1_ex6_title",
            slideKeyPrefix = "assembly_lesson1_ex6",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "", "_start:", "  mov rax, 25", "  mov rdi, rax", "", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = ";", localizationKey = "assembly_lesson1_ex6_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 7: Compare values
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson1_ex7_title",
            slideKeyPrefix = "assembly_lesson1_ex7",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "", "_start:", "  mov rax, 5", "  cmp rax, 10", "", "  mov rax, 60", "  xor rdi, rdi", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = ";", localizationKey = "assembly_lesson1_ex7_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 8: Use conditional jumps
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson1_ex8_title",
            slideKeyPrefix = "assembly_lesson1_ex8",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "", "_start:", "  mov rax, 5", "  cmp rax, 5", "  je equal", "  mov rdi, 1", "  jmp done", "", "equal:", "  mov rdi, 0", "", "done:", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 6, commentPrefix = ";", localizationKey = "assembly_lesson1_ex8_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 9: Jump to a label
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson1_ex9_title",
            slideKeyPrefix = "assembly_lesson1_ex9",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "", "_start:", "  jmp done", "", "skip:", "  mov rdi, 1", "", "done:", "  xor rdi, rdi", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = ";", localizationKey = "assembly_lesson1_ex9_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 10: Increment a value
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson1_ex10_title",
            slideKeyPrefix = "assembly_lesson1_ex10",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "", "_start:", "  mov rax, 9", "  inc rax", "", "  mov rdi, rax", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = ";", localizationKey = "assembly_lesson1_ex10_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: Assembly II - Registers and Operations ====================
        var lesson2 = new Lesson { titleKey = "assembly_lesson2_title" };

        // Exercise 1: Put values in registers
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson2_ex1_title",
            slideKeyPrefix = "assembly_lesson2_ex1",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "_start:", "  mov rax, 60", "  mov rdi, 0", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = ";", localizationKey = "assembly_lesson2_ex1_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 2: Add with add
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson2_ex2_title",
            slideKeyPrefix = "assembly_lesson2_ex2",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "_start:", "  mov rax, 10", "  add rax, 5", "  mov rdi, rax", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = ";", localizationKey = "assembly_lesson2_ex2_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 3: Subtract with sub
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson2_ex3_title",
            slideKeyPrefix = "assembly_lesson2_ex3",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "_start:", "  mov rax, 20", "  sub rax, 5", "  mov rdi, rax", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = ";", localizationKey = "assembly_lesson2_ex3_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 4: Multiply with mul
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson2_ex4_title",
            slideKeyPrefix = "assembly_lesson2_ex4",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "_start:", "  mov rax, 4", "  mov rbx, 3", "  mul rbx", "  mov rdi, rax", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = ";", localizationKey = "assembly_lesson2_ex4_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 5: Compare with cmp
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson2_ex5_title",
            slideKeyPrefix = "assembly_lesson2_ex5",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "_start:", "  mov rax, 5", "  cmp rax, 5", "  mov rax, 60", "  mov rdi, 0", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = ";", localizationKey = "assembly_lesson2_ex5_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 6: Unconditional jump with jmp
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson2_ex6_title",
            slideKeyPrefix = "assembly_lesson2_ex6",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "_start:", "  jmp done", "done:", "  mov rax, 60", "  xor rdi, rdi", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = ";", localizationKey = "assembly_lesson2_ex6_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 7: Jump if equal with je
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson2_ex7_title",
            slideKeyPrefix = "assembly_lesson2_ex7",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "_start:", "  mov rax, 5", "  cmp rax, 5", "  je done", "done:", "  mov rax, 60", "  xor rdi, rdi", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = ";", localizationKey = "assembly_lesson2_ex7_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 8: Stack operations with push and pop
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson2_ex8_title",
            slideKeyPrefix = "assembly_lesson2_ex8",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "_start:", "  mov rax, 42", "  push rax", "  pop rbx", "  mov rax, 60", "  mov rdi, rbx", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = ";", localizationKey = "assembly_lesson2_ex8_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 9: Zero clear with xor
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson2_ex9_title",
            slideKeyPrefix = "assembly_lesson2_ex9",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "_start:", "  mov rax, 60", "  xor rdi, rdi", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = ";", localizationKey = "assembly_lesson2_ex9_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 10: Function call with call
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson2_ex10_title",
            slideKeyPrefix = "assembly_lesson2_ex10",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "_start:", "  call done", "done:", "  mov rax, 60", "  xor rdi, rdi", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = ";", localizationKey = "assembly_lesson2_ex10_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: Assembly III - Memory and Stack ====================
        var lesson3 = new Lesson { titleKey = "assembly_lesson3_title" };

        // Exercise 1: Decrement a value
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson3_ex1_title",
            slideKeyPrefix = "assembly_lesson3_ex1",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "", "_start:", "  mov rax, 10", "  dec rax", "", "  mov rdi, rax", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = ";", localizationKey = "assembly_lesson3_ex1_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 2: Multiply numbers
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson3_ex2_title",
            slideKeyPrefix = "assembly_lesson3_ex2",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "", "_start:", "  mov rbx, 3", "  imul rax, rbx, 4", "", "  mov rdi, rax", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = ";", localizationKey = "assembly_lesson3_ex2_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 3: Save to stack
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson3_ex3_title",
            slideKeyPrefix = "assembly_lesson3_ex3",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "", "_start:", "  mov rax, 42", "  push rax", "  pop rdi", "", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = ";", localizationKey = "assembly_lesson3_ex3_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 4: Retrieve from stack
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson3_ex4_title",
            slideKeyPrefix = "assembly_lesson3_ex4",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "", "_start:", "  push 99", "  pop rdi", "", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = ";", localizationKey = "assembly_lesson3_ex4_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 5: Logical AND operation
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson3_ex5_title",
            slideKeyPrefix = "assembly_lesson3_ex5",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "", "_start:", "  mov rax, 0xFF", "  and rax, 0x0F", "", "  mov rdi, rax", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = ";", localizationKey = "assembly_lesson3_ex5_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 6: Logical OR operation
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson3_ex6_title",
            slideKeyPrefix = "assembly_lesson3_ex6",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "", "_start:", "  mov rax, 0x10", "  or rax, 0x01", "", "  mov rdi, rax", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = ";", localizationKey = "assembly_lesson3_ex6_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 7: Left shift operation
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson3_ex7_title",
            slideKeyPrefix = "assembly_lesson3_ex7",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "", "_start:", "  mov rax, 5", "  shl rax, 2", "", "  mov rdi, rax", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = ";", localizationKey = "assembly_lesson3_ex7_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 8: Right shift operation
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson3_ex8_title",
            slideKeyPrefix = "assembly_lesson3_ex8",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "", "_start:", "  mov rax, 20", "  shr rax, 1", "", "  mov rdi, rax", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = ";", localizationKey = "assembly_lesson3_ex8_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 9: Test bits with test
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson3_ex9_title",
            slideKeyPrefix = "assembly_lesson3_ex9",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "", "_start:", "  mov rax, 5", "  test rax, 1", "  jnz is_odd", "  mov rdi, 0", "  jmp done", "is_odd:", "  mov rdi, 1", "done:", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = ";", localizationKey = "assembly_lesson3_ex9_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        // Exercise 10: Negate with neg
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "assembly_lesson3_ex10_title",
            slideKeyPrefix = "assembly_lesson3_ex10",
            slideCount = 2,
            correctLines = new List<string> { "section .text", "  global _start", "", "_start:", "  mov rax, 10", "  neg rax", "", "  xor rdi, rdi", "  mov rax, 60", "  syscall" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = ";", localizationKey = "assembly_lesson3_ex10_comment1" }
            },
            expectedOutput = new List<string> { }
        });

        lessons.Add(lesson3);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;

        Debug.Log($"[LessonManager] Initialized {lessons.Count} Assembly lessons");
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

        // Mark current exercise as completed in ProgressManager
        if (ProgressManager.Instance != null)
        {
            ProgressManager.Instance.SetLessonCompleted(currentLanguage, currentLessonIndex, currentExerciseIndex, true);
            Debug.Log($"[LessonManager] Marked exercise {currentLanguage} course {currentLessonIndex} lesson {currentExerciseIndex} as completed");
        }

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

    /// <summary>
    /// Sets the programming language and course/lesson indices, reinitializing lessons for the new language
    /// </summary>
    public void SetLanguageAndLesson(string language, int lessonIndex, int exerciseIndex = 0)
    {
        Debug.Log($"[LessonManager] SetLanguageAndLesson called: language={language}, lesson={lessonIndex}, exercise={exerciseIndex}");

        // Only reinitialize if language changed
        if (currentLanguage != language)
        {
            currentLanguage = language;
            InitializeLessonsForLanguage(language);
        }

        // Set lesson and exercise
        if (lessonIndex >= 0 && lessonIndex < lessons.Count)
        {
            currentLessonIndex = lessonIndex;
            currentExerciseIndex = Mathf.Clamp(exerciseIndex, 0, lessons[lessonIndex].exercises.Count - 1);
        }
        else
        {
            currentLessonIndex = 0;
            currentExerciseIndex = 0;
        }

        DisplayCurrentExercise();
    }

    /// <summary>
    /// Initializes lessons based on the programming language
    /// </summary>
    private void InitializeLessonsForLanguage(string language)
    {
        lessons.Clear();

        switch (language.ToLower())
        {
            case "python":
                InitializePythonLessons();
                break;
            case "javascript":
                InitializeJavaScriptLessons();
                break;
            case "typescript":
                InitializeTypeScriptLessons();
                break;
            case "java":
                InitializeJavaLessons();
                break;
            case "c":
                InitializeCLessons();
                break;
            case "cpp":
                InitializeCppLessons();
                break;
            case "assembly":
                InitializeAssemblyLessons();
                break;
            case "csharp":
                InitializeCSharpLessons();
                break;
            case "go":
                InitializeGoLessons();
                break;
            default:
                Debug.LogWarning($"[LessonManager] Unknown language: {language}, defaulting to Python");
                InitializePythonLessons();
                break;
        }

        Debug.Log($"[LessonManager] Initialized {lessons.Count} lessons for {language}");
    }

    /// <summary>
    /// Initialize Python lessons (extracted from InitializeDefaultExercise)
    /// </summary>
    private void InitializePythonLessons()
    {
        // Call the existing initialization which is for Python
        InitializeDefaultExercise();
    }

    /// <summary>
    /// Initialize C# lessons
    /// </summary>
    private void InitializeCSharpLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: C# (シーシャープ) に挑戦！ ====================
        var lesson1 = new Lesson { titleKey = "csharp_lesson1_title" };

        // Exercise 1: 画面にメッセージを出しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex1_title",
            slideKeyPrefix = "csharp_lesson1_ex1",
            slideCount = 2,
            correctLines = new List<string> { "// Hello, C#! と出力する", "Console.WriteLine(\"Hello, C#!\");" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex1_comment1" }
            },
            expectedOutput = new List<string> { "Hello, C#!" }
        });

        // Exercise 2: 便利な「はこ」変数（へんすう）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex2_title",
            slideKeyPrefix = "csharp_lesson1_ex2",
            slideCount = 2,
            correctLines = new List<string> { "// name というはこに \"CSharp\" を入れる", "string name = \"CSharp\";", "// はこの中身を画面に出す", "Console.WriteLine(name);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson1_ex2_comment2" }
            },
            expectedOutput = new List<string> { "CSharp" }
        });

        // Exercise 3: コンピュータで計算しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex3_title",
            slideKeyPrefix = "csharp_lesson1_ex3",
            slideCount = 2,
            correctLines = new List<string> { "// x というはこに 10 を入れる", "int x = 10;", "// y というはこに 5 を入れる", "int y = 5;", "// x と y をたした答えを出す", "Console.WriteLine(x + y);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson1_ex3_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "csharp_lesson1_ex3_comment3" }
            },
            expectedOutput = new List<string> { "15" }
        });

        // Exercise 4: 剰余演算子（%）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex4_title",
            slideKeyPrefix = "csharp_lesson1_ex4",
            slideCount = 2,
            correctLines = new List<string> { "// 10 を 3 で割ったあまりを出力する", "Console.WriteLine(10 % 3);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex4_comment1" }
            },
            expectedOutput = new List<string> { "1" }
        });

        // Exercise 5: 累算代入演算子（+=、-=）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex5_title",
            slideKeyPrefix = "csharp_lesson1_ex5",
            slideCount = 2,
            correctLines = new List<string> { "// hp に 100 を入れる", "int hp = 100;", "// += で 20 を足す", "hp += 20;", "// -= で 50 を引く", "hp -= 50;", "Console.WriteLine(hp);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson1_ex5_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "csharp_lesson1_ex5_comment3" }
            },
            expectedOutput = new List<string> { "70" }
        });

        // Exercise 6: 文章の中に変数を入れましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex6_title",
            slideKeyPrefix = "csharp_lesson1_ex6",
            slideCount = 2,
            correctLines = new List<string> { "// age というはこに 10 を入れる", "int age = 10;", "// 文字列補間を使ってメッセージを出す", "Console.WriteLine($\"私は{age}歳です\");" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson1_ex6_comment2" }
            },
            expectedOutput = new List<string> { "私は10歳です" }
        });

        // Exercise 7: たくさんのデータをまとめましょう「配列」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex7_title",
            slideKeyPrefix = "csharp_lesson1_ex7",
            slideCount = 2,
            correctLines = new List<string> { "// colors という配列を作る", "string[] colors = {\"赤\", \"青\", \"緑\"};", "// 2番目のデータを出す", "Console.WriteLine(colors[1]);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson1_ex7_comment2" }
            },
            expectedOutput = new List<string> { "青" }
        });

        // Exercise 8: 「もし〜なら」で分ける if文
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex8_title",
            slideKeyPrefix = "csharp_lesson1_ex8",
            slideCount = 2,
            correctLines = new List<string> { "// score に 100 を入れる", "int score = 100;", "// もし 80 より大きかったら", "if (score > 80)", "{", "    // メッセージを表示する", "    Console.WriteLine(\"合格！\");", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson1_ex8_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson1_ex8_comment3" }
            },
            expectedOutput = new List<string> { "合格！" }
        });

        // Exercise 9: ちがう場合は？ if-else文
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex9_title",
            slideKeyPrefix = "csharp_lesson1_ex9",
            slideCount = 2,
            correctLines = new List<string> { "// age に 10 を入れる", "int age = 10;", "// 20歳以上かどうかで分ける", "if (age >= 20)", "{", "    Console.WriteLine(\"大人\");", "}", "// それ以外の場合", "else", "{", "    Console.WriteLine(\"子供\");", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson1_ex9_comment2" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "csharp_lesson1_ex9_comment3" }
            },
            expectedOutput = new List<string> { "子供" }
        });

        // Exercise 10: 論理演算子（&&、||）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex10_title",
            slideKeyPrefix = "csharp_lesson1_ex10",
            slideCount = 2,
            correctLines = new List<string> { "// score と bonus を定義", "int score = 80;", "int bonus = 10;", "// && で両方の条件をチェック", "if (score >= 70 && bonus > 0)", "{", "    Console.WriteLine(\"ボーナスあり合格\");", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex10_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "csharp_lesson1_ex10_comment2" }
            },
            expectedOutput = new List<string> { "ボーナスあり合格" }
        });

        // Exercise 11: ぐるぐる回す foreach
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex11_title",
            slideKeyPrefix = "csharp_lesson1_ex11",
            slideCount = 2,
            correctLines = new List<string> { "// 名前の配列を作る", "string[] names = {\"太郎\", \"花子\"};", "// 順番に取り出すループ", "foreach (string name in names)", "{", "    // 取り出した名前を表示", "    Console.WriteLine(name);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex11_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson1_ex11_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson1_ex11_comment3" }
            },
            expectedOutput = new List<string> { "太郎", "花子" }
        });

        // Exercise 12: 名前で探しましょう「Dictionary」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex12_title",
            slideKeyPrefix = "csharp_lesson1_ex12",
            slideCount = 2,
            correctLines = new List<string> { "// Dictionary を作る", "var fruits = new Dictionary<string, string>();", "// キーと値を追加", "fruits[\"みかん\"] = \"オレンジ\";", "// キーを指定して値を取り出す", "Console.WriteLine(fruits[\"みかん\"]);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex12_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson1_ex12_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "csharp_lesson1_ex12_comment3" }
            },
            expectedOutput = new List<string> { "オレンジ" }
        });

        // Exercise 13: 自分だけの関数を作ろう「メソッド」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex13_title",
            slideKeyPrefix = "csharp_lesson1_ex13",
            slideCount = 2,
            correctLines = new List<string> { "// Greet というメソッドを定義", "static void Greet()", "{", "    Console.WriteLine(\"こんにちは\");", "}", "// メソッドを呼び出す", "Greet();" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex13_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson1_ex13_comment2" }
            },
            expectedOutput = new List<string> { "こんにちは" }
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: C# II - オブジェクト指向とLINQ ====================
        var lesson2 = new Lesson { titleKey = "csharp_lesson2_title" };

        // Exercise 1: クラスの継承
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex1_title",
            slideKeyPrefix = "csharp_lesson2_ex1",
            slideCount = 2,
            correctLines = new List<string> { "// 基底クラス Vehicle を定義", "class Vehicle {", "    public void Move() {", "        Console.WriteLine(\"moving\");", "    }", "}", "// Car は Vehicle を継承", "class Car : Vehicle { }", "", "Car c = new Car();", "c.Move();" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson2_ex1_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "csharp_lesson2_ex1_comment2" }
            },
            expectedOutput = new List<string> { "moving" }
        });

        // Exercise 2: メソッドのオーバーライド
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex2_title",
            slideKeyPrefix = "csharp_lesson2_ex2",
            slideCount = 2,
            correctLines = new List<string> { "// virtual でオーバーライド可能にする", "class Shape {", "    public virtual void Draw() {", "        Console.WriteLine(\"shape\");", "    }", "}", "// override で上書き", "class Circle : Shape {", "    public override void Draw() {", "        Console.WriteLine(\"circle\");", "    }", "}", "", "Circle c = new Circle();", "c.Draw();" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson2_ex2_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "csharp_lesson2_ex2_comment2" }
            },
            expectedOutput = new List<string> { "circle" }
        });

        // Exercise 3: インターフェースを使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex3_title",
            slideKeyPrefix = "csharp_lesson2_ex3",
            slideCount = 2,
            correctLines = new List<string> { "// インターフェースを定義", "interface IRunner {", "    void Run();", "}", "// インターフェースを実装", "class Robot : IRunner {", "    public void Run() {", "        Console.WriteLine(\"running\");", "    }", "}", "", "Robot r = new Robot();", "r.Run();" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson2_ex3_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "csharp_lesson2_ex3_comment2" }
            },
            expectedOutput = new List<string> { "running" }
        });

        // Exercise 4: プロパティを使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex4_title",
            slideKeyPrefix = "csharp_lesson2_ex4",
            slideCount = 2,
            correctLines = new List<string> { "// 自動プロパティを定義", "class Item {", "    public int Price { get; set; }", "}", "", "Item item = new Item();", "// プロパティに値を設定", "item.Price = 500;", "Console.WriteLine(item.Price);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson2_ex4_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "csharp_lesson2_ex4_comment2" }
            },
            expectedOutput = new List<string> { "500" }
        });

        // Exercise 5: List<T> を使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex5_title",
            slideKeyPrefix = "csharp_lesson2_ex5",
            slideCount = 2,
            correctLines = new List<string> { "// List を作成", "List<int> nums = new List<int>();", "// Add で要素を追加", "nums.Add(10);", "nums.Add(20);", "Console.WriteLine(nums[1]);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson2_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson2_ex5_comment2" }
            },
            expectedOutput = new List<string> { "20" }
        });

        // Exercise 6: LINQ - Where で絞り込み
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex6_title",
            slideKeyPrefix = "csharp_lesson2_ex6",
            slideCount = 2,
            correctLines = new List<string> { "List<int> nums = new List<int> {1, 5, 10, 15, 20};", "// Where で条件に合う要素を絞り込む", "var result = nums.Where(n => n >= 10);", "foreach (var n in result) {", "    Console.WriteLine(n);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "csharp_lesson2_ex6_comment1" }
            },
            expectedOutput = new List<string> { "10", "15", "20" }
        });

        // Exercise 7: LINQ - Select で変換
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex7_title",
            slideKeyPrefix = "csharp_lesson2_ex7",
            slideCount = 2,
            correctLines = new List<string> { "List<int> nums = new List<int> {1, 2, 3};", "// Select で各要素を変換", "var squared = nums.Select(n => n * n);", "foreach (var n in squared) {", "    Console.WriteLine(n);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "csharp_lesson2_ex7_comment1" }
            },
            expectedOutput = new List<string> { "1", "4", "9" }
        });

        // Exercise 8: ラムダ式を使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex8_title",
            slideKeyPrefix = "csharp_lesson2_ex8",
            slideCount = 2,
            correctLines = new List<string> { "// Func デリゲートでラムダ式を定義", "Func<int, int> triple = x => x * 3;", "Console.WriteLine(triple(7));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson2_ex8_comment1" }
            },
            expectedOutput = new List<string> { "21" }
        });

        // Exercise 9: 例外処理 try-catch
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex9_title",
            slideKeyPrefix = "csharp_lesson2_ex9",
            slideCount = 2,
            correctLines = new List<string> { "// try で例外が発生する可能性のあるコードを囲む", "try {", "    throw new Exception(\"oops\");", "// catch で例外をキャッチ", "} catch (Exception e) {", "    Console.WriteLine(\"caught\");", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson2_ex9_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "csharp_lesson2_ex9_comment2" }
            },
            expectedOutput = new List<string> { "caught" }
        });

        // Exercise 10: null条件演算子 ?.
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex10_title",
            slideKeyPrefix = "csharp_lesson2_ex10",
            slideCount = 2,
            correctLines = new List<string> { "string text = \"Hello\";", "// ?. で null チェックしながらプロパティにアクセス", "int? length = text?.Length;", "Console.WriteLine(length);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "csharp_lesson2_ex10_comment1" }
            },
            expectedOutput = new List<string> { "5" }
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: C# III - 非同期とLINQ応用 ====================
        var lesson3 = new Lesson { titleKey = "csharp_lesson3_title" };

        // Exercise 1: async/await の基本
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex1_title",
            slideKeyPrefix = "csharp_lesson3_ex1",
            slideCount = 2,
            correctLines = new List<string> { "// async メソッドを定義", "async Task SayHelloAsync() {", "    await Task.Delay(100);", "    Console.WriteLine(\"Hello Async!\");", "}", "// 非同期メソッドを呼び出す", "await SayHelloAsync();" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson3_ex1_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson3_ex1_comment2" }
            },
            expectedOutput = new List<string> { "Hello Async!" }
        });

        // Exercise 2: LINQ OrderBy
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex2_title",
            slideKeyPrefix = "csharp_lesson3_ex2",
            slideCount = 2,
            correctLines = new List<string> { "List<int> nums = new List<int> {3, 1, 4, 1, 5};", "// OrderBy でソート", "var sorted = nums.OrderBy(n => n);", "Console.WriteLine(string.Join(\",\", sorted));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "csharp_lesson3_ex2_comment1" }
            },
            expectedOutput = new List<string> { "1,1,3,4,5" }
        });

        // Exercise 3: LINQ First と FirstOrDefault
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex3_title",
            slideKeyPrefix = "csharp_lesson3_ex3",
            slideCount = 2,
            correctLines = new List<string> { "List<int> nums = new List<int>();", "// FirstOrDefault で要素がない場合はデフォルト値を返す", "int first = nums.FirstOrDefault();", "Console.WriteLine(first);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "csharp_lesson3_ex3_comment1" }
            },
            expectedOutput = new List<string> { "0" }
        });

        // Exercise 4: LINQ Any と All
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex4_title",
            slideKeyPrefix = "csharp_lesson3_ex4",
            slideCount = 2,
            correctLines = new List<string> { "List<int> nums = new List<int> {2, 4, 6, 8};", "// All で全ての要素が条件を満たすかチェック", "bool allEven = nums.All(n => n % 2 == 0);", "Console.WriteLine(allEven);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "csharp_lesson3_ex4_comment1" }
            },
            expectedOutput = new List<string> { "True" }
        });

        // Exercise 5: LINQ Sum と Average
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex5_title",
            slideKeyPrefix = "csharp_lesson3_ex5",
            slideCount = 2,
            correctLines = new List<string> { "List<int> nums = new List<int> {10, 20, 30};", "// Sum で合計を計算", "int total = nums.Sum();", "Console.WriteLine(total);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "csharp_lesson3_ex5_comment1" }
            },
            expectedOutput = new List<string> { "60" }
        });

        // Exercise 6: switch式（パターンマッチ）
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex6_title",
            slideKeyPrefix = "csharp_lesson3_ex6",
            slideCount = 2,
            correctLines = new List<string> { "int num = 2;", "// switch 式でパターンマッチ", "string result = num switch {", "    1 => \"one\",", "    2 => \"two\",", "    _ => \"other\"", "};", "Console.WriteLine(result);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "csharp_lesson3_ex6_comment1" }
            },
            expectedOutput = new List<string> { "two" }
        });

        // Exercise 7: Dictionary の TryGetValue
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex7_title",
            slideKeyPrefix = "csharp_lesson3_ex7",
            slideCount = 2,
            correctLines = new List<string> { "var dict = new Dictionary<string, int>();", "dict[\"apple\"] = 1;", "// TryGetValue で安全に値を取得", "if (dict.TryGetValue(\"apple\", out int value)) {", "    Console.WriteLine(value);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson3_ex7_comment1" }
            },
            expectedOutput = new List<string> { "1" }
        });

        // Exercise 8: string interpolation
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex8_title",
            slideKeyPrefix = "csharp_lesson3_ex8",
            slideCount = 2,
            correctLines = new List<string> { "int a = 10, b = 20;", "// $ で文字列補間を使う", "Console.WriteLine($\"Sum: {a + b}\");" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "csharp_lesson3_ex8_comment1" }
            },
            expectedOutput = new List<string> { "Sum: 30" }
        });

        // Exercise 9: record 型
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex9_title",
            slideKeyPrefix = "csharp_lesson3_ex9",
            slideCount = 2,
            correctLines = new List<string> { "// record でイミュータブルなデータ型を定義", "record Point(int X, int Y);", "", "var p = new Point(10, 20);", "Console.WriteLine(p);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson3_ex9_comment1" }
            },
            expectedOutput = new List<string> { "Point { X = 10, Y = 20 }" }
        });

        // Exercise 10: using で自動解放
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex10_title",
            slideKeyPrefix = "csharp_lesson3_ex10",
            slideCount = 2,
            correctLines = new List<string> { "// using でリソースを自動解放", "using (var reader = new StringReader(\"Hello\")) {", "    Console.WriteLine(reader.ReadLine());", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson3_ex10_comment1" }
            },
            expectedOutput = new List<string> { "Hello" }
        });

        lessons.Add(lesson3);

        // ==================== LESSON 4: C# IV - ジェネリクスとデリゲート ====================
        var lesson4 = new Lesson { titleKey = "csharp_lesson4_title" };

        // Exercise 1: ジェネリッククラス
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex1_title",
            slideKeyPrefix = "csharp_lesson4_ex1",
            slideCount = 2,
            correctLines = new List<string> { "// ジェネリッククラスを定義", "class Box<T> {", "    public T Value { get; set; }", "}", "", "var box = new Box<string>();", "box.Value = \"Hello\";", "Console.WriteLine(box.Value);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson4_ex1_comment1" }
            },
            expectedOutput = new List<string> { "Hello" }
        });

        // Exercise 2: ジェネリック制約 where
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex2_title",
            slideKeyPrefix = "csharp_lesson4_ex2",
            slideCount = 2,
            correctLines = new List<string> { "// where で型制約を指定", "class NumBox<T> where T : struct {", "    public T Value { get; set; }", "}", "", "var box = new NumBox<int>();", "box.Value = 1;", "Console.WriteLine(box.Value);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson4_ex2_comment1" }
            },
            expectedOutput = new List<string> { "1" }
        });

        // Exercise 3: デリゲートの基本
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex3_title",
            slideKeyPrefix = "csharp_lesson4_ex3",
            slideCount = 2,
            correctLines = new List<string> { "// デリゲート型を定義", "delegate int Operation(int x);", "", "Operation op = x => x * 2;", "Console.WriteLine(op(5));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson4_ex3_comment1" }
            },
            expectedOutput = new List<string> { "10" }
        });

        // Exercise 4: Func デリゲート
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex4_title",
            slideKeyPrefix = "csharp_lesson4_ex4",
            slideCount = 2,
            correctLines = new List<string> { "// Func で戻り値のあるデリゲート", "Func<int, int> triple = x => x * 3;", "Console.WriteLine(triple(7));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson4_ex4_comment1" }
            },
            expectedOutput = new List<string> { "21" }
        });

        // Exercise 5: Action デリゲート
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex5_title",
            slideKeyPrefix = "csharp_lesson4_ex5",
            slideCount = 2,
            correctLines = new List<string> { "// Action で戻り値なしのデリゲート", "Action<string> greet = name => Console.WriteLine($\"Hello, {name}!\");", "greet(\"World\");" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson4_ex5_comment1" }
            },
            expectedOutput = new List<string> { "Hello, World!" }
        });

        // Exercise 6: LINQ GroupBy
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex6_title",
            slideKeyPrefix = "csharp_lesson4_ex6",
            slideCount = 2,
            correctLines = new List<string> { "List<int> nums = new List<int> {1, 2, 3, 4, 5, 6};", "// GroupBy でグループ化", "var groups = nums.GroupBy(n => n % 2 == 0 ? \"even\" : \"odd\");", "foreach (var g in groups) {", "    Console.WriteLine($\"{g.Key}: {g.Count()}\");", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "csharp_lesson4_ex6_comment1" }
            },
            expectedOutput = new List<string> { "odd: 3", "even: 3" }
        });

        // Exercise 7: null条件演算子 ?. (配列版)
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex7_title",
            slideKeyPrefix = "csharp_lesson4_ex7",
            slideCount = 2,
            correctLines = new List<string> { "int[]? arr = null;", "// ?. と ?? で null 安全にアクセス", "int len = arr?.Length ?? 0;", "Console.WriteLine(len);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "csharp_lesson4_ex7_comment1" }
            },
            expectedOutput = new List<string> { "0" }
        });

        // Exercise 8: null合体演算子 ??
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex8_title",
            slideKeyPrefix = "csharp_lesson4_ex8",
            slideCount = 2,
            correctLines = new List<string> { "string? text = null;", "// ?? で null の場合のデフォルト値を指定", "string result = text ?? \"default\";", "Console.WriteLine(result);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "csharp_lesson4_ex8_comment1" }
            },
            expectedOutput = new List<string> { "default" }
        });

        // Exercise 9: record 型 with 式
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex9_title",
            slideKeyPrefix = "csharp_lesson4_ex9",
            slideCount = 2,
            correctLines = new List<string> { "record Point(int X, int Y);", "", "var p1 = new Point(1, 2);", "// with で一部を変更した新しいインスタンスを作成", "var p2 = p1 with { X = 3, Y = 4 };", "Console.WriteLine(p2);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "csharp_lesson4_ex9_comment1" }
            },
            expectedOutput = new List<string> { "Point { X = 3, Y = 4 }" }
        });

        // Exercise 10: init プロパティ
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex10_title",
            slideKeyPrefix = "csharp_lesson4_ex10",
            slideCount = 2,
            correctLines = new List<string> { "// init で初期化時のみ設定可能なプロパティ", "class Product {", "    public string Name { get; init; }", "}", "", "var p = new Product { Name = \"Apple\" };", "Console.WriteLine(p.Name);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson4_ex10_comment1" }
            },
            expectedOutput = new List<string> { "Apple" }
        });

        lessons.Add(lesson4);
    }

    /// <summary>
    /// Initialize Go lessons
    /// </summary>
    private void InitializeGoLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: Go I - ゴー言語に挑戦！ ====================
        var lesson1 = new Lesson { titleKey = "go_lesson1_title" };

        // Exercise 1: 画面にメッセージを出しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex1_title",
            slideKeyPrefix = "go_lesson1_ex1",
            slideCount = 2,
            correctLines = new List<string> { "// Hello, Go! と出力する", "fmt.Println(\"Hello, Go!\")" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson1_ex1_comment1" }
            },
            expectedOutput = new List<string> { "Hello, Go!" }
        });

        // Exercise 2: 便利な「はこ」変数
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex2_title",
            slideKeyPrefix = "go_lesson1_ex2",
            slideCount = 2,
            correctLines = new List<string> { "// x というはこに 10 を入れる", "x := 10", "// はこの中身を画面に出す", "fmt.Println(x)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson1_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson1_ex2_comment2" }
            },
            expectedOutput = new List<string> { "10" }
        });

        // Exercise 3: コンピュータで計算しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex3_title",
            slideKeyPrefix = "go_lesson1_ex3",
            slideCount = 2,
            correctLines = new List<string> { "// a というはこに 5 を入れる", "a := 5", "// b というはこに 3 を入れる", "b := 3", "// a と b をたした答えを出す", "fmt.Println(a + b)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson1_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson1_ex3_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson1_ex3_comment3" }
            },
            expectedOutput = new List<string> { "8" }
        });

        // Exercise 4: 剰余演算子（%）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex4_title",
            slideKeyPrefix = "go_lesson1_ex4",
            slideCount = 2,
            correctLines = new List<string> { "// 10 を 3 で割ったあまりを出力する", "fmt.Println(10 % 3)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson1_ex4_comment1" }
            },
            expectedOutput = new List<string> { "1" }
        });

        // Exercise 5: 累算代入演算子（+=、-=）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex5_title",
            slideKeyPrefix = "go_lesson1_ex5",
            slideCount = 2,
            correctLines = new List<string> { "// hp に 100 を入れる", "hp := 100", "// += で 20 を足す", "hp += 20", "// -= で 50 を引く", "hp -= 50", "fmt.Println(hp)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson1_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson1_ex5_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson1_ex5_comment3" }
            },
            expectedOutput = new List<string> { "70" }
        });

        // Exercise 6: 文章の中に変数を入れましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex6_title",
            slideKeyPrefix = "go_lesson1_ex6",
            slideCount = 2,
            correctLines = new List<string> { "// age というはこに 10 を入れる", "age := 10", "// Printf でフォーマット出力", "fmt.Printf(\"I am %d years old.\\n\", age)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson1_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson1_ex6_comment2" }
            },
            expectedOutput = new List<string> { "I am 10 years old." }
        });

        // Exercise 7: データをならべる「スライス」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex7_title",
            slideKeyPrefix = "go_lesson1_ex7",
            slideCount = 2,
            correctLines = new List<string> { "// nums というスライスを作る", "nums := []int{10, 20}", "// 2番目のデータを出す", "fmt.Println(nums[1])" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson1_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson1_ex7_comment2" }
            },
            expectedOutput = new List<string> { "20" }
        });

        // Exercise 8: 「もし〜なら」if文
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex8_title",
            slideKeyPrefix = "go_lesson1_ex8",
            slideCount = 2,
            correctLines = new List<string> { "// score に 100 を入れる", "score := 100", "// もし 80 より大きかったら", "if score > 80 {", "    // メッセージを表示する", "    fmt.Println(\"Great\")", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson1_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson1_ex8_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson1_ex8_comment3" }
            },
            expectedOutput = new List<string> { "Great" }
        });

        // Exercise 9: if-else文
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex9_title",
            slideKeyPrefix = "go_lesson1_ex9",
            slideCount = 2,
            correctLines = new List<string> { "// x に 5 を入れる", "x := 5", "// 10 より大きいかどうかで分ける", "if x > 10 {", "    fmt.Println(\"Big\")", "} else {", "    fmt.Println(\"Small\")", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson1_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson1_ex9_comment2" }
            },
            expectedOutput = new List<string> { "Small" }
        });

        // Exercise 10: 論理演算子
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex10_title",
            slideKeyPrefix = "go_lesson1_ex10",
            slideCount = 2,
            correctLines = new List<string> { "// score と bonus を定義", "score := 80", "bonus := 10", "// && で両方の条件をチェック", "if score >= 70 && bonus > 0 {", "    fmt.Println(\"Bonus Pass\")", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson1_ex10_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "go_lesson1_ex10_comment2" }
            },
            expectedOutput = new List<string> { "Bonus Pass" }
        });

        // Exercise 11: rangeで全部出す
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex11_title",
            slideKeyPrefix = "go_lesson1_ex11",
            slideCount = 2,
            correctLines = new List<string> { "// 数のスライスを作る", "nums := []int{1, 2, 3}", "// range で順番に取り出す", "for _, n := range nums {", "    fmt.Println(n)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson1_ex11_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson1_ex11_comment2" }
            },
            expectedOutput = new List<string> { "1", "2", "3" }
        });

        // Exercise 12: Map辞書
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex12_title",
            slideKeyPrefix = "go_lesson1_ex12",
            slideCount = 2,
            correctLines = new List<string> { "// Map を作る", "scores := map[string]int{\"Math\": 90}", "// キーを指定して値を取り出す", "fmt.Println(scores[\"Math\"])" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson1_ex12_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson1_ex12_comment2" }
            },
            expectedOutput = new List<string> { "90" }
        });

        // Exercise 13: 自分だけの関数
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex13_title",
            slideKeyPrefix = "go_lesson1_ex13",
            slideCount = 2,
            correctLines = new List<string> { "// greet という関数を定義", "func greet() {", "    fmt.Println(\"Hello\")", "}", "// 関数を呼び出す", "greet()" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson1_ex13_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson1_ex13_comment2" }
            },
            expectedOutput = new List<string> { "Hello" }
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: Go II - 構造体と並行処理 ====================
        var lesson2 = new Lesson { titleKey = "go_lesson2_title" };

        // Exercise 1: 複数の値を返す関数
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex1_title",
            slideKeyPrefix = "go_lesson2_ex1",
            slideCount = 2,
            correctLines = new List<string> { "// 2つの値を返す関数を定義", "func minmax(a, b int) (int, int) {", "    if a < b {", "        return a, b", "    }", "    return b, a", "}", "", "min, max := minmax(5, 3)", "fmt.Println(min, max)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson2_ex1_comment1" }
            },
            expectedOutput = new List<string> { "3 5" }
        });

        // Exercise 2: エラー処理
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex2_title",
            slideKeyPrefix = "go_lesson2_ex2",
            slideCount = 2,
            correctLines = new List<string> { "// エラーを返す関数を定義", "func checkPositive(n int) (int, error) {", "    if n < 0 {", "        return 0, errors.New(\"negative\")", "    }", "    return n, nil", "}", "", "val, err := checkPositive(5)", "fmt.Println(val, err)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson2_ex2_comment1" }
            },
            expectedOutput = new List<string> { "5 <nil>" }
        });

        // Exercise 3: ポインタ
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex3_title",
            slideKeyPrefix = "go_lesson2_ex3",
            slideCount = 2,
            correctLines = new List<string> { "// x に 5 を入れる", "x := 5", "// p は x のアドレスを持つ", "p := &x", "// ポインタ経由で値を変更", "*p = 10", "fmt.Println(x)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson2_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson2_ex3_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson2_ex3_comment3" }
            },
            expectedOutput = new List<string> { "10" }
        });

        // Exercise 4: 構造体定義
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex4_title",
            slideKeyPrefix = "go_lesson2_ex4",
            slideCount = 2,
            correctLines = new List<string> { "// Point 構造体を定義", "type Point struct {", "    X int", "    Y int", "}", "", "p := Point{X: 3, Y: 4}", "fmt.Println(p.X)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson2_ex4_comment1" }
            },
            expectedOutput = new List<string> { "3" }
        });

        // Exercise 5: メソッド
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex5_title",
            slideKeyPrefix = "go_lesson2_ex5",
            slideCount = 2,
            correctLines = new List<string> { "// Rect 構造体を定義", "type Rect struct {", "    W, H int", "}", "// メソッドを定義", "func (r Rect) Area() int {", "    return r.W * r.H", "}", "", "rect := Rect{W: 3, H: 4}", "fmt.Println(rect.Area())" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson2_ex5_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson2_ex5_comment2" }
            },
            expectedOutput = new List<string> { "12" }
        });

        // Exercise 6: インターフェース
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex6_title",
            slideKeyPrefix = "go_lesson2_ex6",
            slideCount = 2,
            correctLines = new List<string> { "// Speaker インターフェースを定義", "type Speaker interface {", "    Speak()", "}", "// Dog 構造体を定義", "type Dog struct{}", "// Speak メソッドを実装", "func (d Dog) Speak() {", "    fmt.Println(\"woof\")", "}", "", "var s Speaker = Dog{}", "s.Speak()" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson2_ex6_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson2_ex6_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "go_lesson2_ex6_comment3" }
            },
            expectedOutput = new List<string> { "woof" }
        });

        // Exercise 7: defer
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex7_title",
            slideKeyPrefix = "go_lesson2_ex7",
            slideCount = 2,
            correctLines = new List<string> { "// defer で関数終了時に実行", "defer fmt.Println(\"end\")", "fmt.Println(\"start\")" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson2_ex7_comment1" }
            },
            expectedOutput = new List<string> { "start", "end" }
        });

        // Exercise 8: ゴルーチン
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex8_title",
            slideKeyPrefix = "go_lesson2_ex8",
            slideCount = 2,
            correctLines = new List<string> { "// say 関数を定義", "func say(msg string) {", "    fmt.Println(msg)", "}", "// go で並行実行", "go say(\"hello\")", "// 少し待つ", "time.Sleep(100 * time.Millisecond)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson2_ex8_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson2_ex8_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "go_lesson2_ex8_comment3" }
            },
            expectedOutput = new List<string> { "hello" }
        });

        // Exercise 9: チャネル
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex9_title",
            slideKeyPrefix = "go_lesson2_ex9",
            slideCount = 2,
            correctLines = new List<string> { "// チャネルを作成", "ch := make(chan int)", "// ゴルーチンで値を送信", "go func() {", "    ch <- 100", "}()", "// 値を受信", "val := <-ch", "fmt.Println(val)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson2_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson2_ex9_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "go_lesson2_ex9_comment3" }
            },
            expectedOutput = new List<string> { "100" }
        });

        // Exercise 10: 無名関数
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex10_title",
            slideKeyPrefix = "go_lesson2_ex10",
            slideCount = 2,
            correctLines = new List<string> { "// n に 5 を入れる", "n := 5", "// 無名関数を定義", "double := func() int {", "    return n * 2", "}", "fmt.Println(double())" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson2_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson2_ex10_comment2" }
            },
            expectedOutput = new List<string> { "10" }
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: Go III - ジェネリクスとテスト ====================
        var lesson3 = new Lesson { titleKey = "go_lesson3_title" };

        // Exercise 1: ジェネリクス基本
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex1_title",
            slideKeyPrefix = "go_lesson3_ex1",
            slideCount = 2,
            correctLines = new List<string> { "// ジェネリック関数を定義", "func First[T any](slice []T) T {", "    return slice[0]", "}", "", "nums := []int{10, 20, 30}", "fmt.Println(First(nums))" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson3_ex1_comment1" }
            },
            expectedOutput = new List<string> { "10" }
        });

        // Exercise 2: comparable制約
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex2_title",
            slideKeyPrefix = "go_lesson3_ex2",
            slideCount = 2,
            correctLines = new List<string> { "// comparable 制約付きジェネリック関数", "func IndexOf[T comparable](slice []T, v T) int {", "    for i, x := range slice {", "        if x == v {", "            return i", "        }", "    }", "    return -1", "}", "", "nums := []int{10, 20, 30}", "fmt.Println(IndexOf(nums, 20))" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson3_ex2_comment1" }
            },
            expectedOutput = new List<string> { "1" }
        });

        // Exercise 3: make
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex3_title",
            slideKeyPrefix = "go_lesson3_ex3",
            slideCount = 2,
            correctLines = new List<string> { "// make でスライスを作成", "nums := make([]int, 3)", "nums[0] = 10", "nums[1] = 20", "nums[2] = 30", "fmt.Println(nums)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson3_ex3_comment1" }
            },
            expectedOutput = new List<string> { "[10 20 30]" }
        });

        // Exercise 4: append
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex4_title",
            slideKeyPrefix = "go_lesson3_ex4",
            slideCount = 2,
            correctLines = new List<string> { "// 2つのスライスを作成", "a := []int{1, 2}", "b := []int{3, 4}", "// append で結合", "c := append(a, b...)", "fmt.Println(c)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson3_ex4_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "go_lesson3_ex4_comment2" }
            },
            expectedOutput = new List<string> { "[1 2 3 4]" }
        });

        // Exercise 5: copy
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex5_title",
            slideKeyPrefix = "go_lesson3_ex5",
            slideCount = 2,
            correctLines = new List<string> { "// コピー元スライス", "src := []int{10, 20, 30}", "// コピー先スライスを作成", "dst := make([]int, len(src))", "// copy でコピー", "copy(dst, src)", "fmt.Println(dst)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson3_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson3_ex5_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson3_ex5_comment3" }
            },
            expectedOutput = new List<string> { "[10 20 30]" }
        });

        // Exercise 6: select
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex6_title",
            slideKeyPrefix = "go_lesson3_ex6",
            slideCount = 2,
            correctLines = new List<string> { "// バッファ付きチャネルを作成", "ch1 := make(chan int, 1)", "ch1 <- 10", "// select で待機", "select {", "case v := <-ch1:", "    fmt.Println(v)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson3_ex6_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "go_lesson3_ex6_comment2" }
            },
            expectedOutput = new List<string> { "10" }
        });

        // Exercise 7: range map
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex7_title",
            slideKeyPrefix = "go_lesson3_ex7",
            slideCount = 2,
            correctLines = new List<string> { "// Map を作成", "scores := map[string]int{\"math\": 90, \"english\": 85}", "// range で反復", "for k, v := range scores {", "    fmt.Printf(\"%s: %d\\n\", k, v)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson3_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson3_ex7_comment2" }
            },
            expectedOutput = new List<string> { "math: 90", "english: 85" }
        });

        // Exercise 8: type新型
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex8_title",
            slideKeyPrefix = "go_lesson3_ex8",
            slideCount = 2,
            correctLines = new List<string> { "// type で新しい型を定義", "type Age int", "// 新しい型を使う", "var age Age = 25", "fmt.Println(age)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson3_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson3_ex8_comment2" }
            },
            expectedOutput = new List<string> { "25" }
        });

        // Exercise 9: 埋め込み
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex9_title",
            slideKeyPrefix = "go_lesson3_ex9",
            slideCount = 2,
            correctLines = new List<string> { "// Base 構造体を定義", "type Base struct {", "    Name string", "}", "// Base を埋め込んだ構造体", "type Extended struct {", "    Base", "    Extra string", "}", "", "e := Extended{Base: Base{Name: \"Go\"}, Extra: \"lang\"}", "fmt.Println(e.Name)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson3_ex9_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson3_ex9_comment2" }
            },
            expectedOutput = new List<string> { "Go" }
        });

        // Exercise 10: panic/recover
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex10_title",
            slideKeyPrefix = "go_lesson3_ex10",
            slideCount = 2,
            correctLines = new List<string> { "// defer で recover を設定", "defer func() {", "    if r := recover(); r != nil {", "        fmt.Println(\"caught\")", "    }", "}()", "// panic を発生", "panic(\"error\")" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson3_ex10_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "go_lesson3_ex10_comment2" }
            },
            expectedOutput = new List<string> { "caught" }
        });

        lessons.Add(lesson3);

        // ==================== LESSON 4: Go IV - 並行処理とネットワーク ====================
        var lesson4 = new Lesson { titleKey = "go_lesson4_title" };

        // Exercise 1: sync.Mutex
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex1_title",
            slideKeyPrefix = "go_lesson4_ex1",
            slideCount = 2,
            correctLines = new List<string> { "// Mutex を宣言", "var mu sync.Mutex", "count := 0", "// Lock で排他制御", "mu.Lock()", "count++", "mu.Unlock()", "fmt.Println(count)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson4_ex1_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "go_lesson4_ex1_comment2" }
            },
            expectedOutput = new List<string> { "1" }
        });

        // Exercise 2: sync.WaitGroup
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex2_title",
            slideKeyPrefix = "go_lesson4_ex2",
            slideCount = 2,
            correctLines = new List<string> { "// WaitGroup を宣言", "var wg sync.WaitGroup", "// ゴルーチンを追加", "wg.Add(1)", "go func() {", "    defer wg.Done()", "    fmt.Println(\"done\")", "}()", "wg.Wait()" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson4_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson4_ex2_comment2" }
            },
            expectedOutput = new List<string> { "done" }
        });

        // Exercise 3: context.Background
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex3_title",
            slideKeyPrefix = "go_lesson4_ex3",
            slideCount = 2,
            correctLines = new List<string> { "// 空のコンテキストを作成", "ctx := context.Background()", "// エラーを確認", "fmt.Println(ctx.Err())" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson4_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson4_ex3_comment2" }
            },
            expectedOutput = new List<string> { "<nil>" }
        });

        // Exercise 4: context.WithCancel
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex4_title",
            slideKeyPrefix = "go_lesson4_ex4",
            slideCount = 2,
            correctLines = new List<string> { "// キャンセル可能なコンテキストを作成", "ctx, cancel := context.WithCancel(context.Background())", "// キャンセルを実行", "cancel()", "fmt.Println(ctx.Err())" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson4_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson4_ex4_comment2" }
            },
            expectedOutput = new List<string> { "context canceled" }
        });

        // Exercise 5: time.Duration
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex5_title",
            slideKeyPrefix = "go_lesson4_ex5",
            slideCount = 2,
            correctLines = new List<string> { "// Duration を作成", "d := 500 * time.Millisecond", "fmt.Println(d)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson4_ex5_comment1" }
            },
            expectedOutput = new List<string> { "500ms" }
        });

        // Exercise 6: json.Marshal
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex6_title",
            slideKeyPrefix = "go_lesson4_ex6",
            slideCount = 2,
            correctLines = new List<string> { "// 構造体を定義", "type Item struct {", "    Name string `json:\"name\"`", "}", "", "item := Item{Name: \"Apple\"}", "// JSON に変換", "data, _ := json.Marshal(item)", "fmt.Println(string(data))" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson4_ex6_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "go_lesson4_ex6_comment2" }
            },
            expectedOutput = new List<string> { "{\"name\":\"Apple\"}" }
        });

        // Exercise 7: json.Unmarshal
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex7_title",
            slideKeyPrefix = "go_lesson4_ex7",
            slideCount = 2,
            correctLines = new List<string> { "// 構造体を定義", "type Item struct {", "    Name string `json:\"name\"`", "}", "", "data := []byte(`{\"name\":\"Banana\"}`)", "var item Item", "// JSON をパース", "json.Unmarshal(data, &item)", "fmt.Println(item.Name)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson4_ex7_comment1" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "go_lesson4_ex7_comment2" }
            },
            expectedOutput = new List<string> { "Banana" }
        });

        // Exercise 8: strings.Split
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex8_title",
            slideKeyPrefix = "go_lesson4_ex8",
            slideCount = 2,
            correctLines = new List<string> { "// 文字列を分割", "parts := strings.Split(\"hello,world\", \",\")", "fmt.Println(parts[0])" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson4_ex8_comment1" }
            },
            expectedOutput = new List<string> { "hello" }
        });

        // Exercise 9: strconv.Atoi
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex9_title",
            slideKeyPrefix = "go_lesson4_ex9",
            slideCount = 2,
            correctLines = new List<string> { "// 文字列を数値に変換", "num, _ := strconv.Atoi(\"42\")", "fmt.Println(num * 2)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson4_ex9_comment1" }
            },
            expectedOutput = new List<string> { "84" }
        });

        // Exercise 10: os.Args
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex10_title",
            slideKeyPrefix = "go_lesson4_ex10",
            slideCount = 2,
            correctLines = new List<string> { "// コマンドライン引数の数を出力", "fmt.Println(len(os.Args))" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "go_lesson4_ex10_comment1" }
            },
            expectedOutput = new List<string> { "1" }
        });

        lessons.Add(lesson4);
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
    public string CurrentLanguage => currentLanguage;
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

        // Mark lesson as completed if not already
        if (ProgressManager.Instance != null)
        {
            if (!ProgressManager.Instance.IsLessonCompleted(currentLanguage, currentLessonIndex, currentExerciseIndex))
            {
                ProgressManager.Instance.SetLessonCompleted(currentLanguage, currentLessonIndex, currentExerciseIndex, true);
                Debug.Log($"[LessonManager] Marked exercise {currentLanguage} course {currentLessonIndex} lesson {currentExerciseIndex} as completed");
            }
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
