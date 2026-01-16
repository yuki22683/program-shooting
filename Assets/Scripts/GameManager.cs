using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

public class GameManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI[] sheetLineTexts;
	[SerializeField] private TextMeshProUGUI[] answerLineTexts;
	[SerializeField] private GameObject[] interfaceButtons;
	[SerializeField] private GameObject interfaceButtonLineBottom;
	[SerializeField] private GameObject interfaceButtonLineMiddle;
	[SerializeField] private TextMeshProUGUI consoleText;
	[SerializeField] private ToggleBool sheetAnswerToggleBool;
	[SerializeField] private ToggleBool sheetSlideToggleBool;
	[SerializeField] private ToggleBool answerSheetAllToggleBool;
	[SerializeField] private ToggleBool consoleAnswerToggleBool;
	[SerializeField] private GameObject answerPanel;
	[SerializeField] private GameObject slidePanel;
	private string consoleTextTemp;
	private Coroutine coroutineRequestExeCode = null;
	public List<Word> interfaceWords;
	public const float SOUND_VOLUME_SOFT = 0.2f;
	public const float SOUND_VOLUME_MEDIUM = 0.6f;
	public const float SOUND_VOLUME_LOUD = 1.0f;
	public const int SKYBOX_ID_MAX = 10;
	public const int LINE_NUM = 20;
	public const int LESSON_NUM = 10;
	public const int INTERFACE_BUTTON_NUM = 12;
	//public static readonly Color color_COMMENT = new Color(12f / 255f, 250f / 255f, 0f / 255f, 1f);
	public static readonly Color color_COMMENT = new Color(0.8f, 0.8f, 0.8f, 1f);
	public static readonly int[] chapter_NUMS = new int[LESSON_NUM]
	{
		10, 10, 10, 10, 10, 10, 10, 10, 10, 10
	};
	private readonly string serverUrl = "https://ytStudio.pythonanywhere.com/start"; // サーバーのエンドポイント

	public Sheet activeSheet;
	public AudioSource[] audioSources;

	public struct Sheet
	{
		public Line[] lines;
		public int lineId;
		public bool cursorBlinkState;

		public Sheet(int languageNo, int lessonNo, int chapterNo)
		{
			lines = new Line[LINE_NUM];

			for (int i = 0; i < LINE_NUM; i++)
			{
				lines[i] = new Line(languageNo, lessonNo, chapterNo, i);
			}
			lineId = 0;
			cursorBlinkState = true;
		}
	}

	public struct Line
	{
		public List<Word> answerWords;
		public List<Word> inputWords;
		public List<Word> initialWords;
		public bool reqExe;
		public bool reqInput;
		public bool isComment;
		public int wordId;

		public Line(int languageNo, int lessonNo, int chapterNo, int lineNo)
		{
			List<Word> words = WordMapper.GetWords(languageNo, lessonNo, chapterNo, lineNo);

			if ((words == null) || (words.Count == 0))
			{
				reqExe = false;
				reqInput = false;
				isComment = false;
				answerWords = new List<Word>();
				initialWords = new List<Word>();
			}
			else if (words.Any(word => word.body.Contains("#")))
			{
				reqExe = false;
				reqInput = false;
				isComment = true;
				answerWords = new List<Word>();
				initialWords = words;
			}
			else if (words.Any(word => word.wordType == WordType.Reserve))
			{
				// 行の中に予約語(if,def,class等)があれば、入力完了して即時実行しない
				reqExe = false;
				reqInput = true;
				isComment = false;
				answerWords = words;
				initialWords = new List<Word>();
			}
			else
			{
				reqExe = true;
				reqInput = true;
				isComment = false;
				answerWords = words;
				initialWords = new List<Word>();
			}
			inputWords = new List<Word>();
			wordId = 0;
		}
	}

	// リストの要素をスワップするメソッド
	public static void Swap<T>(List<T> list, int index1, int index2)
	{
		// 範囲チェック（必要に応じて）
		if (index1 < 0 || index1 >= list.Count || index2 < 0 || index2 >= list.Count)
		{
			throw new ArgumentOutOfRangeException("インデックスがリストの範囲外です。");
		}

		// 一時変数を使ってスワップ
		T temp = list[index1];
		list[index1] = list[index2];
		list[index2] = temp;
	}

	public static void Shuffle<T>(List<T> list)
	{
		int n = list.Count;
		for (int i = n - 1; i > 0; i--)
		{
			int j = UnityEngine.Random.Range(0, i + 1); // UnityEngine.Randomを使用
			T temp = list[i];
			list[i] = list[j];
			list[j] = temp;
		}
	}

	public static class WordMapper
	{
		private static readonly Dictionary<(int languageNo, int lessonNo, int chapterNo, int lineNo), List<Word>> _wordMap;

		static WordMapper()
		{
			_wordMap = new Dictionary<(int languageNo, int lessonNo, int chapterNo, int lineNo), List<Word>>
			{
				// Language Python
				// Lesson Hello World
				// Line1
				{ (0, 0, 0, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_hello_world_line1_comment")),
					}
				},
				// Line2
				{ (0, 0, 0, 1), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'Hello World'"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Lesson Variables and data
				// Line1
				{ (0, 1, 0, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_variables_data_line1_comment")),
					}
				},
				// Line2
				{ (0, 1, 0, 1), new List<Word>
					{
						new Word(false, WordType.Variable, "name"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'Sarah'"),
					}
				},
				// Line3
				{ (0, 1, 0, 2), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_variables_data_line3_comment")),
					}
				},
				// Line4
				{ (0, 1, 0, 3), new List<Word>
					{
						new Word(false, WordType.Variable, "age"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Num, "30"),
					}
				},
				// Line5
				{ (0, 1, 0, 4), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_variables_data_line5_comment")),
					}
				},
				// Line6
				{ (0, 1, 0, 5), new List<Word>
					{
						new Word(false, WordType.Variable, "height"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Num, "158.2"),
					}
				},
				// Line7
				{ (0, 1, 0, 6), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_variables_data_line7_comment")),
					}
				},
				// Line8
				{ (0, 1, 0, 7), new List<Word>
					{
						new Word(false, WordType.Variable, "is_student"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Bool, "False"),
					}
				},
				// Line9
				{ (0, 1, 0, 8), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_variables_data_line9_comment")),
					}
				},
				// Line10
				{ (0, 1, 0, 9), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "name"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "age"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "height"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "is_student"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Lesson String
				// Chapter1
				// Line1
				{ (0, 2, 0, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_string_chapter1_line1_comment")),
					}
				},
				// Line2
				{ (0, 2, 0, 1), new List<Word>
					{
						new Word(false, WordType.Variable, "first_name"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'Sarah'"),
					}
				},
				// Line3
				{ (0, 2, 0, 2), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_string_chapter1_line3_comment")),
					}
				},
				// Line4
				{ (0, 2, 0, 3), new List<Word>
					{
						new Word(false, WordType.Variable, "last_name"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'Wilson'"),
					}
				},
				// Line5
				{ (0, 2, 0, 4), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_string_chapter1_line5_comment")),
					}
				},
				// Line4
				{ (0, 2, 0, 5), new List<Word>
					{
						new Word(false, WordType.Variable, "full_name"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "first_name"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "+"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "' '"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "+"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "last_name"),
					}
				},
				// Line6
				{ (0, 2, 0, 6), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_string_chapter1_line7_comment")),
					}
				},
				// Line7
				{ (0, 2, 0, 7), new List<Word>
					{
						new Word(false, WordType.Variable, "name_len"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_BuildIn, "len"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "full_name"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line8
				{ (0, 2, 0, 8), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_string_chapter1_line9_comment")),
					}
				},
				// Line9
				{ (0, 2, 0, 9), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.FString, "f"),
						new Word(false, WordType.Symbol, "'"),
						new Word(false, WordType.Text, "Full Name:"),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Variable, "full_name"),
						new Word(false, WordType.Symbol, "}"),
						new Word(false, WordType.NewLine, "\\\\n"),
						new Word(false, WordType.Text, "Length:"),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Variable, "name_len"),
						new Word(false, WordType.Symbol, "}"),
						new Word(false, WordType.Symbol, "'"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Lesson String
				// Chapter2
				// Line1
				{ (0, 2, 1, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_string_chapter2_line1_comment")),
					}
				},
				// Line2
				{ (0, 2, 1, 1), new List<Word>
					{
						new Word(false, WordType.Variable, "name"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'Sarah'"),
					}
				},
				// Line3
				{ (0, 2, 1, 2), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_string_chapter2_line3_comment")),
					}
				},
				// Line4
				{ (0, 2, 1, 3), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "name"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line5
				{ (0, 2, 1, 4), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_string_chapter2_line5_comment")),
					}
				},
				// Line6
				{ (0, 2, 1, 5), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "name"),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Num, "0"),
						new Word(false, WordType.Symbol, "]"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line7
				{ (0, 2, 1, 6), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_string_chapter2_line7_comment")),
					}
				},
				// Line8
				{ (0, 2, 1, 7), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "name"),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Num, "-1"),
						new Word(false, WordType.Symbol, "]"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line9
				{ (0, 2, 1, 8), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_string_chapter2_line9_comment")),
					}
				},
				// Line10
				{ (0, 2, 1, 9), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "name"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Function_Custom, "find"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'ah'"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line11
				{ (0, 2, 1, 10), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_string_chapter2_line11_comment")),
					}
				},
				// Line12
				{ (0, 2, 1, 11), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "name"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Function_Custom, "find"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'x'"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line13
				{ (0, 2, 1, 12), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_string_chapter2_line13_comment")),
					}
				},
				// Line14
				{ (0, 2, 1, 13), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "name"),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Num, "2"),
						new Word(false, WordType.Symbol, ":"),
						new Word(false, WordType.Symbol, "]"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line15
				{ (0, 2, 1, 14), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_string_chapter2_line15_comment")),
					}
				},
				// Line16
				{ (0, 2, 1, 15), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "name"),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Num, "1"),
						new Word(false, WordType.Symbol, ":"),
						new Word(false, WordType.Num, "3"),
						new Word(false, WordType.Symbol, "]"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Lesson Num
				// Chapter1
				// Line1
				{ (0, 3, 0, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_num_chapter1_line1_comment")),
					}
				},
				// Line2
				{ (0, 3, 0, 1), new List<Word>
					{
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Num, "12"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Num, "3"),
					}
				},
				// Line3
				{ (0, 3, 0, 2), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_num_chapter1_line3_comment")),
					}
				},
				// Line4
				{ (0, 3, 0, 3), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.FString, "f"),
						new Word(false, WordType.Symbol, "'"),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Symbol, "}"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "+"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, "}"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "+"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line5
				{ (0, 3, 0, 4), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_num_chapter1_line5_comment")),
					}
				},
				// Line6
				{ (0, 3, 0, 5), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.FString, "f"),
						new Word(false, WordType.Symbol, "'"),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Symbol, "}"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "-"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, "}"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "-"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line7
				{ (0, 3, 0, 6), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_num_chapter1_line7_comment")),
					}
				},
				// Line8
				{ (0, 3, 0, 7), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.FString, "f"),
						new Word(false, WordType.Symbol, "'"),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Symbol, "}"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "×"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, "}"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "*"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line9
				{ (0, 3, 0, 8), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_num_chapter1_line9_comment")),
					}
				},
				// Line10
				{ (0, 3, 0, 9), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.FString, "f"),
						new Word(false, WordType.Symbol, "'"),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Symbol, "}"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "÷"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, "}"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "/"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line11
				{ (0, 3, 0, 10), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_num_chapter1_line11_comment")),
					}
				},
				// Line12
				{ (0, 3, 0, 11), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.FString, "f"),
						new Word(false, WordType.Symbol, "'"),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Symbol, "}"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "÷"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, "}"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "//"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line13
				{ (0, 3, 0, 12), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_num_chapter1_line13_comment")),
					}
				},
				// Line14
				{ (0, 3, 0, 13), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.FString, "f"),
						new Word(false, WordType.Symbol, "'"),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Symbol, "}"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "mod"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, "}"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "%"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line15
				{ (0, 3, 0, 14), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_num_chapter1_line15_comment")),
					}
				},
				// Line16
				{ (0, 3, 0, 15), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.FString, "f"),
						new Word(false, WordType.Symbol, "'"),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Symbol, "}"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "^"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, "}"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "**"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Chapter2
				// Line1
				{ (0, 3, 1, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_num_chapter2_line1_comment")),
					}
				},
				// Line2
				{ (0, 3, 1, 1), new List<Word>
					{
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Num, "15"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Num, "3"),
					}
				},
				// Line3
				{ (0, 3, 1, 2), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_num_chapter2_line3_comment")),
					}
				},
				// Line4
				{ (0, 3, 1, 3), new List<Word>
					{
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "+"),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
					}
				},
				// Line5
				{ (0, 3, 1, 4), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line6
				{ (0, 3, 1, 5), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_num_chapter2_line6_comment")),
					}
				},
				// Line7
				{ (0, 3, 1, 6), new List<Word>
					{
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "-"),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
					}
				},
				// Line8
				{ (0, 3, 1, 7), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line9
				{ (0, 3, 1, 8), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_num_chapter2_line9_comment")),
					}
				},
				// Line8
				{ (0, 3, 1, 9), new List<Word>
					{
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "*"),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
					}
				},
				// Line11
				{ (0, 3, 1, 10), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line12
				{ (0, 3, 1, 11), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_num_chapter2_line12_comment")),
					}
				},
				// Line13
				{ (0, 3, 1, 12), new List<Word>
					{
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "/"),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
					}
				},
				// Line14
				{ (0, 3, 1, 13), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Chapter3
				// Line1
				{ (0, 3, 2, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_num_chapter3_line1_comment")),
					}
				},
				// Line2
				{ (0, 3, 2, 1), new List<Word>
					{
						new Word(true, WordType.Reserve, "import"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Module, "math"),
					}
				},
				// Line3
				{ (0, 3, 2, 2), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_num_chapter3_line3_comment")),
					}
				},
				// Line4
				{ (0, 3, 2, 3), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Function_BuildIn, "abs"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Num, "-5"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line5
				{ (0, 3, 2, 4), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_num_chapter3_line5_comment")),
					}
				},
				// Line6
				{ (0, 3, 2, 5), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Function_BuildIn, "divmod"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Num, "10"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Num, "3"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line7
				{ (0, 3, 2, 6), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_num_chapter3_line7_comment")),
					}
				},
				// Line8
				{ (0, 3, 2, 7), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Module, "math"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Function_Custom, "ceil"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Num, "3.14"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line9
				{ (0, 3, 2, 8), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_num_chapter3_line9_comment")),
					}
				},
				// Line10
				{ (0, 3, 2, 9), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Module, "math"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Function_Custom, "floor"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Num, "3.14"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Lesson List
				// Chapter1
				// Line1
				{ (0, 4, 0, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_list_chapter1_line1_comment")),
					}
				},
				// Line2
				{ (0, 4, 0, 1), new List<Word>
					{
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Text, "'dog'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'cat'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'bird'"),
						new Word(false, WordType.Symbol, "]"),
					}
				},
				// Line3
				{ (0, 4, 0, 2), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_list_chapter1_line3_comment")),
					}
				},
				// Line4
				{ (0, 4, 0, 3), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line5
				{ (0, 4, 0, 4), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_list_chapter1_line5_comment")),
					}
				},
				// Line6
				{ (0, 4, 0, 5), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Num, "0"),
						new Word(false, WordType.Symbol, "]"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line7
				{ (0, 4, 0, 6), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_list_chapter1_line7_comment")),
					}
				},
				// Line8
				{ (0, 4, 0, 7), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Num, "-1"),
						new Word(false, WordType.Symbol, "]"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line9
				{ (0, 4, 0, 8), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_list_chapter1_line9_comment")),
					}
				},
				// Line10
				{ (0, 4, 0, 9), new List<Word>
					{
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Num, "0"),
						new Word(false, WordType.Symbol, "]"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'fish'"),
					}
				},
				// Line11
				{ (0, 4, 0, 10), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_list_chapter1_line11_comment")),
					}
				},
				// Line12
				{ (0, 4, 0, 11), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Function_BuildIn, "len"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line13
				{ (0, 4, 0, 12), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_list_chapter1_line13_comment")),
					}
				},
				// Line14
				{ (0, 4, 0, 13), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Num, "1"),
						new Word(false, WordType.Symbol, ":"),
						new Word(false, WordType.Symbol, "]"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line15
				{ (0, 4, 0, 14), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_list_chapter1_line14_comment")),
					}
				},
				// Line16
				{ (0, 4, 0, 15), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Num, "0"),
						new Word(false, WordType.Symbol, ":"),
						new Word(false, WordType.Num, "2"),
						new Word(false, WordType.Symbol, "]"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Lesson List
				// Chapter2
				// Line1
				{ (0, 4, 1, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_list_chapter2_line1_comment")),
					}
				},
				// Line2
				{ (0, 4, 1, 1), new List<Word>
					{
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Text, "'dog'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'cat'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'bird'"),
						new Word(false, WordType.Symbol, "]"),
					}
				},
				// Line3
				{ (0, 4, 1, 2), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_list_chapter2_line3_comment")),
					}
				},
				// Line4
				{ (0, 4, 1, 3), new List<Word>
					{
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Function_Custom, "append"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'fish'"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ";"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line5
				{ (0, 4, 1, 4), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_list_chapter2_line5_comment")),
					}
				},
				// Line6
				{ (0, 4, 1, 5), new List<Word>
					{
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Function_Custom, "insert"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Num, "1"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'bird'"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ";"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line7
				{ (0, 4, 1, 6), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_list_chapter2_line7_comment")),
					}
				},
				// Line8
				{ (0, 4, 1, 7), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Function_Custom, "index"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'cat'"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line9
				{ (0, 4, 1, 8), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_list_chapter2_line9_comment")),
					}
				},
				// Line10
				{ (0, 4, 1, 9), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Function_Custom, "count"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'bird'"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line11
				{ (0, 4, 1, 10), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_list_chapter2_line11_comment")),
					}
				},
				// Line12
				{ (0, 4, 1, 11), new List<Word>
					{
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Function_Custom, "remove"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'bird'"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ";"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line13
				{ (0, 4, 1, 12), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_list_chapter2_line13_comment")),
					}
				},
				// Line14
				{ (0, 4, 1, 13), new List<Word>
					{
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Function_Custom, "reverse"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ";"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line15
				{ (0, 4, 1, 14), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_list_chapter2_line15_comment")),
					}
				},
				// Line16
				{ (0, 4, 1, 15), new List<Word>
					{
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Function_Custom, "sort"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ";"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line17
				{ (0, 4, 1, 16), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_list_chapter2_line17_comment")),
					}
				},
				// Line18
				{ (0, 4, 1, 17), new List<Word>
					{
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Function_Custom, "clear"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ";"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Lesson Tuple
				// Chapter1
				// Line1
				{ (0, 5, 0, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_tuple_chapter1_line1_comment")),
					}
				},
				// Line2
				{ (0, 5, 0, 1), new List<Word>
					{
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'dog'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'cat'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'bird'"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line3
				{ (0, 5, 0, 2), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_tuple_chapter1_line3_comment")),
					}
				},
				// Line4
				{ (0, 5, 0, 3), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line5
				{ (0, 5, 0, 4), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_tuple_chapter1_line5_comment")),
					}
				},
				// Line6
				{ (0, 5, 0, 5), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Num, "0"),
						new Word(false, WordType.Symbol, "]"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line7
				{ (0, 5, 0, 6), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_tuple_chapter1_line7_comment")),
					}
				},
				// Line8
				{ (0, 5, 0, 7), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Num, "-1"),
						new Word(false, WordType.Symbol, "]"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line9
				{ (0, 5, 0, 8), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_tuple_chapter1_line9_comment")),
					}
				},
				// Line10
				{ (0, 5, 0, 9), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Function_BuildIn, "len"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line11
				{ (0, 5, 0, 10), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_tuple_chapter1_line11_comment")),
					}
				},
				// Line12
				{ (0, 5, 0, 11), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Num, "1"),
						new Word(false, WordType.Symbol, ":"),
						new Word(false, WordType.Symbol, "]"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line13
				{ (0, 5, 0, 12), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_tuple_chapter1_line13_comment")),
					}
				},
				// Line14
				{ (0, 5, 0, 13), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Num, "0"),
						new Word(false, WordType.Symbol, ":"),
						new Word(false, WordType.Num, "2"),
						new Word(false, WordType.Symbol, "]"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Lesson Function
				// Chapter1
				// Line1
				{ (0, 6, 0, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_function_chapter1_line1_comment")),
					}
				},
				// Line2
				{ (0, 6, 0, 1), new List<Word>
					{
						new Word(true, WordType.Reserve, "def"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_Define, "show_true"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line3
				{ (0, 6, 0, 2), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_function_chapter1_line3_comment")),
					}
				},
				// Line4
				{ (0, 6, 0, 3), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Bool, "True"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line5
				{ (0, 6, 0, 4), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_function_chapter1_line5_comment")),
					}
				},
				// Line6
				{ (0, 6, 0, 5), new List<Word>
					{
						new Word(false, WordType.Function_Custom, "show_true"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line7 空行
				// Line8
				{ (0, 6, 0, 7), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_function_chapter1_line8_comment")),
					}
				},
				// Line9
				{ (0, 6, 0, 8), new List<Word>
					{
						new Word(true, WordType.Reserve, "def"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_Define, "show_bool"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "flg"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line10
				{ (0, 6, 0, 9), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_function_chapter1_line10_comment")),
					}
				},
				// Line11
				{ (0, 6, 0, 10), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "flg"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line12
				{ (0, 6, 0, 11), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_function_chapter1_line12_comment")),
					}
				},
				// Line13
				{ (0, 6, 0, 12), new List<Word>
					{
						new Word(false, WordType.Function_Custom, "show_bool"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Bool, "False"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Lesson Return
				// Chapter1
				// Line1
				{ (0, 7, 0, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_return_chapter1_line1_comment")),
					}
				},
				// Line2
				{ (0, 7, 0, 1), new List<Word>
					{
						new Word(true, WordType.Reserve, "def"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_Define, "multiply"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "x"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "y"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line3
				{ (0, 7, 0, 2), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_function_chapter1_line3_comment")),
					}
				},
				// Line4
				{ (0, 7, 0, 3), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(true, WordType.Reserve, "return"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "x"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "*"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "y"),
					}
				},
				// Line5
				{ (0, 7, 0, 4), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_function_chapter1_line5_comment")),
					}
				},
				// Line6
				{ (0, 7, 0, 5), new List<Word>
					{
						new Word(false, WordType.Variable, "result"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_Custom, "multiply"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Num, "4"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Num, "6"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line7
				{ (0, 7, 0, 6), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_function_chapter1_line7_comment")),
					}
				},
				// Line8
				{ (0, 7, 0, 7), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "result"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Lesson if
				// Chapter1
				// Line1
				{ (0, 8, 0, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter1_line1_comment")),
					}
				},
				// Line2
				{ (0, 8, 0, 1), new List<Word>
					{
						new Word(false, WordType.Variable, "is_member"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Bool, "True"),
					}
				},
				// Line3
				{ (0, 8, 0, 2), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter1_line3_comment")),
					}
				},
				// Line4
				{ (0, 8, 0, 3), new List<Word>
					{
						new Word(true, WordType.Reserve, "if"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "is_member"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line5
				{ (0, 8, 0, 4), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter1_line5_comment")),
					}
				},
				// Line6
				{ (0, 8, 0, 5), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'member'"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line7
				{ (0, 8, 0, 6), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter1_line7_comment")),
					}
				},
				// Line8
				{ (0, 8, 0, 7), new List<Word>
					{
						new Word(false, WordType.Reserve, "else"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line9
				{ (0, 8, 0, 8), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter1_line9_comment")),
					}
				},
				// Line10
				{ (0, 8, 0, 9), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'non-member'"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Chapter1
				// Line1
				{ (0, 8, 1, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter2_line1_comment")),
					}
				},
				// Line2
				{ (0, 8, 1, 1), new List<Word>
					{
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Num, "2"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Num, "7"),
					}
				},
				// Line3
				{ (0, 8, 1, 2), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter2_line3_comment")),
					}
				},
				// Line4
				{ (0, 8, 1, 3), new List<Word>
					{
						new Word(true, WordType.Reserve, "if"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Comparison, "=="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, ":"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'a == b'"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line5
				{ (0, 8, 1, 4), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter2_line5_comment")),
					}
				},
				// Line6
				{ (0, 8, 1, 5), new List<Word>
					{
						new Word(true, WordType.Reserve, "if"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Comparison, "!="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, ":"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'a != b'"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line7
				{ (0, 8, 1, 6), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter2_line7_comment")),
					}
				},
				// Line8
				{ (0, 8, 1, 7), new List<Word>
					{
						new Word(true, WordType.Reserve, "if"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Comparison, ">"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, ":"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'a > b'"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line9
				{ (0, 8, 1, 8), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter2_line9_comment")),
					}
				},
				// Line10
				{ (0, 8, 1, 9), new List<Word>
					{
						new Word(true, WordType.Reserve, "if"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Comparison, "<"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, ":"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'a < b'"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line11
				{ (0, 8, 1, 10), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter2_line11_comment")),
					}
				},
				// Line12
				{ (0, 8, 1, 11), new List<Word>
					{
						new Word(true, WordType.Reserve, "if"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Comparison, ">="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, ":"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'a >= b'"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line13
				{ (0, 8, 1, 12), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter2_line13_comment")),
					}
				},
				// Line14
				{ (0, 8, 1, 13), new List<Word>
					{
						new Word(true, WordType.Reserve, "if"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "a"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Comparison, "<="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "b"),
						new Word(false, WordType.Symbol, ":"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'a <= b'"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Lesson if
				// Chapter3
				// Line1
				{ (0, 8, 2, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter3_line1_comment")),
					}
				},
				// Line2
				{ (0, 8, 2, 1), new List<Word>
					{
						new Word(false, WordType.Variable, "is_member"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Bool, "True"),
					}
				},
				// Line3
				{ (0, 8, 2, 2), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter3_line3_comment")),
					}
				},
				// Line4
				{ (0, 8, 2, 3), new List<Word>
					{
						new Word(false, WordType.Variable, "is_student"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Bool, "False"),
					}
				},
				// Line5 空行
				// Line6
				{ (0, 8, 2, 5), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter3_line6_comment")),
					}
				},
				// Line7
				{ (0, 8, 2, 6), new List<Word>
					{
						new Word(true, WordType.Reserve, "if"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.Variable, "is_member"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.Operator_Logic, "and"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "is_student"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line8
				{ (0, 8, 2, 7), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter3_line8_comment")),
					}
				},
				// Line9
				{ (0, 8, 2, 8), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'Member and student.'"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line10 空行
				// Line11
				{ (0, 8, 2, 10), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter3_line10_comment")),
					}
				},
				// Line12
				{ (0, 8, 2, 11), new List<Word>
					{
						new Word(true, WordType.Reserve, "if"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.Variable, "is_member"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.Operator_Logic, "or"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "is_student"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line13
				{ (0, 8, 2, 12), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter3_line13_comment")),
					}
				},
				// Line14
				{ (0, 8, 2, 13), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'Member or student.'"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line15 空行
				// Line16
				{ (0, 8, 2, 15), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter3_line15_comment")),
					}
				},
				// Line17
				{ (0, 8, 2, 16), new List<Word>
					{
						new Word(true, WordType.Reserve, "if"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.Operator_Logic, "not"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "is_student"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line18
				{ (0, 8, 2, 17), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_if_chapter3_line18_comment")),
					}
				},
				// Line19
				{ (0, 8, 2, 18), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'Not student.'"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Lesson Dictionary
				// Chapter1
				// Line1
				{ (0, 9, 0, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_dictionary_chapter1_line1_comment")),
					}
				},
				// Line2
				{ (0, 9, 0, 1), new List<Word>
					{
						new Word(false, WordType.Variable, "fruit"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Text, "'name'"),
						new Word(false, WordType.Symbol, ":"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'orange'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'count'"),
						new Word(false, WordType.Symbol, ":"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Num, "10"),
						new Word(false, WordType.Symbol, "}"),
					}
				},
				// Line3
				{ (0, 9, 0, 2), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_dictionary_chapter1_line3_comment")),
					}
				},
				// Line4
				{ (0, 9, 0, 3), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "fruit"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line5
				{ (0, 9, 0, 4), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_dictionary_chapter1_line5_comment")),
					}
				},
				// Line6
				{ (0, 9, 0, 5), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "fruit"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Function_Custom, "keys"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line7
				{ (0, 9, 0, 6), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_dictionary_chapter1_line7_comment")),
					}
				},
				// Line8
				{ (0, 9, 0, 7), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "fruit"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Function_Custom, "values"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line9
				{ (0, 9, 0, 8), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_dictionary_chapter1_line9_comment")),
					}
				},
				// Line10
				{ (0, 9, 0, 9), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "fruit"),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Text, "'name'"),
						new Word(false, WordType.Symbol, "]"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line11
				{ (0, 9, 0, 10), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_dictionary_chapter1_line11_comment")),
					}
				},
				// Line12
				{ (0, 9, 0, 11), new List<Word>
					{
						new Word(false, WordType.Variable, "fruit"),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Text, "'count'"),
						new Word(false, WordType.Symbol, "]"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Num, "15"),
					}
				},
				// Line13
				{ (0, 9, 0, 12), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_dictionary_chapter1_line13_comment")),
					}
				},
				// Line14
				{ (0, 9, 0, 13), new List<Word>
					{
						new Word(true, WordType.Reserve, "del"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "fruit"),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Text, "'name'"),
						new Word(false, WordType.Symbol, "]"),
					}
				},
				// Line15
				{ (0, 9, 0, 14), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_dictionary_chapter1_line15_comment")),
					}
				},
				// Line16
				{ (0, 9, 0, 15), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "fruit"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Lesson While
				// Chapter1
				// Line1
				{ (0, 10, 0, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_while_chapter1_line1_comment")),
					}
				},
				// Line2
				{ (0, 10, 0, 1), new List<Word>
					{
						new Word(false, WordType.Variable, "i"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Num, "5"),
					}
				},
				// Line3
				{ (0, 10, 0, 2), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_while_chapter1_line3_comment")),
					}
				},
				// Line4
				{ (0, 10, 0, 3), new List<Word>
					{
						new Word(true, WordType.Reserve, "while"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "i"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Comparison, ">="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Num, "0"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line5
				{ (0, 10, 0, 4), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_while_chapter1_line5_comment")),
					}
				},
				// Line6
				{ (0, 10, 0, 5), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "i"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line7
				{ (0, 10, 0, 6), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_while_chapter1_line7_comment")),
					}
				},
				// Line8
				{ (0, 10, 0, 7), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Variable, "i"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "-"),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Num, "1"),

					}
				},
				// Lesson For
				// Chapter1
				// Line1
				{ (0, 11, 0, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_for_chapter1_line1_comment")),
					}
				},
				// Line2
				{ (0, 11, 0, 1), new List<Word>
					{
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Text, "'dog'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'cat'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'bird'"),
						new Word(false, WordType.Symbol, "]"),
					}
				},
				// Line3
				{ (0, 11, 0, 2), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_for_chapter1_line3_comment")),
					}
				},
				// Line4
				{ (0, 11, 0, 3), new List<Word>
					{
						new Word(true, WordType.Reserve, "for"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.Variable, "animal"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.Reserve, "in"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line5
				{ (0, 11, 0, 4), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_for_chapter1_line5_comment")),
					}
				},
				// Line6
				{ (0, 11, 0, 5), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animal"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line7
				{ (0, 11, 0, 6), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_for_chapter1_line7_comment")),
					}
				},
				// Line8
				{ (0, 11, 0, 7), new List<Word>
					{
						new Word(true, WordType.Reserve, "for"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.Variable, "letter"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.Reserve, "in"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'dog'"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line9
				{ (0, 11, 0, 8), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_for_chapter1_line9_comment")),
					}
				},
				// Line10
				{ (0, 11, 0, 9), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "letter"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line11
				{ (0, 11, 0, 10), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_for_chapter1_line11_comment")),
					}
				},
				// Line12
				{ (0, 11, 0, 11), new List<Word>
					{
						new Word(true, WordType.Reserve, "for"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.Variable, "i"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.Reserve, "in"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_BuildIn, "range"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Num, "3"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line13
				{ (0, 11, 0, 12), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_for_chapter1_line13_comment")),
					}
				},
				// Line14
				{ (0, 11, 0, 13), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "i"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line15
				{ (0, 11, 0, 14), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_for_chapter1_line15_comment")),
					}
				},
				// Line16
				{ (0, 11, 0, 15), new List<Word>
					{
						new Word(true, WordType.Reserve, "for"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.Variable, "i"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.Reserve, "in"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_BuildIn, "range"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Num, "4"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Num, "7"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line17
				{ (0, 11, 0, 16), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_for_chapter1_line17_comment")),
					}
				},
				// Line18
				{ (0, 11, 0, 17), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "i"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Lesson For
				// Chapter2
				// Line1
				{ (0, 11, 1, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_for_chapter2_line1_comment")),
					}
				},
				// Line2
				{ (0, 11, 1, 1), new List<Word>
					{
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "["),
						new Word(false, WordType.Text, "'dog'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'cat'"),
						new Word(false, WordType.Symbol, "]"),
					}
				},
				// Line3
				{ (0, 11, 1, 2), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_for_chapter2_line3_comment")),
					}
				},
				// Line4
				{ (0, 11, 1, 3), new List<Word>
					{
						new Word(true, WordType.Reserve, "for"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.Variable, "animal"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.Reserve, "in"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "animals"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line5
				{ (0, 11, 1, 4), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_for_chapter2_line5_comment")),
					}
				},
				// Line6
				{ (0, 11, 1, 5), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "animal"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line7
				{ (0, 11, 1, 6), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_for_chapter2_line7_comment")),
					}
				},
				// Line8
				{ (0, 11, 1, 7), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(true, WordType.Reserve, "for"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.Variable, "letter"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.Reserve, "in"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "animal"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line9
				{ (0, 11, 1, 8), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_for_chapter2_line9_comment")),
					}
				},
				// Line10
				{ (0, 11, 1, 9), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "letter"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Lesson Try
				// Chapter1
				// Line1
				{ (0, 12, 0, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_try_chapter1_line1_comment")),
					}
				},
				// Line2
				{ (0, 12, 0, 1), new List<Word>
					{
						new Word(false, WordType.Reserve, "try"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line3
				{ (0, 12, 0, 2), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_try_chapter1_line3_comment")),
					}
				},
				// Line4
				{ (0, 12, 0, 3), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Variable, "x"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Num, "5"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Arithmetic, "+"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'abc'"),
					}
				},
				// Line5
				{ (0, 12, 0, 4), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_try_chapter1_line5_comment")),
					}
				},
				// Line6
				{ (0, 12, 0, 5), new List<Word>
					{
						new Word(true, WordType.Reserve, "except"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.ExceptionType, "TypeError"),
						new Word(false, WordType.Space, " "),
						new Word(true, WordType.Reserve, "as"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Variable, "e"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line7
				{ (0, 12, 0, 6), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_try_chapter1_line7_comment")),
					}
				},
				// Line8
				{ (0, 12, 0, 7), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "e"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line9
				{ (0, 12, 0, 8), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_try_chapter1_line9_comment")),
					}
				},
				// Line10
				{ (0, 12, 0, 9), new List<Word>
					{
						new Word(false, WordType.Reserve, "except"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line11
				{ (0, 12, 0, 10), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_try_chapter1_line11_comment")),
					}
				},
				// Line12
				{ (0, 12, 0, 11), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'exception'"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Lesson Class
				// Chapter1
				// Line1
				{ (0, 13, 0, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_class_chapter1_line1_comment")),
					}
				},
				// Line2
				{ (0, 13, 0, 1), new List<Word>
					{
						new Word(true, WordType.Reserve, "class"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Class, "Car"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line3
				{ (0, 13, 0, 2), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_class_chapter1_line3_comment")),
					}
				},
				// Line4
				{ (0, 13, 0, 3), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(true, WordType.Reserve, "def"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.InitializeMethod, "__init__"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Self, "self"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, ""),
						new Word(false, WordType.Variable, "brand"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, ""),
						new Word(false, WordType.Variable, "color"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line5
				{ (0, 13, 0, 4), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_class_chapter1_line5_comment")),
					}
				},
				// Line6
				{ (0, 13, 0, 5), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Self, "self"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Variable, "brand"),
						new Word(false, WordType.Space, ""),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, ""),
						new Word(false, WordType.Variable, "brand"),
					}
				},
				// Line7
				{ (0, 13, 0, 6), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_class_chapter1_line7_comment")),
					}
				},
				// Line8
				{ (0, 13, 0, 7), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Self, "self"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Variable, "color"),
						new Word(false, WordType.Space, ""),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, ""),
						new Word(false, WordType.Variable, "color"),
					}
				},
				// Line9
				{ (0, 13, 0, 8), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_class_chapter1_line9_comment")),
					}
				},
				// Line10
				{ (0, 13, 0, 9), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(true, WordType.Reserve, "def"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_Define, "describe"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Self, "self"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line11
				{ (0, 13, 0, 10), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_class_chapter1_line11_comment")),
					}
				},
				// Line12
				{ (0, 13, 0, 11), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(true, WordType.Reserve, "return"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.FString, "f"),
						new Word(false, WordType.Symbol, "'"),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Self, "self"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Variable, "color"),
						new Word(false, WordType.Symbol, "}"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Symbol, "{"),
						new Word(false, WordType.Self, "self"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Variable, "brand"),
						new Word(false, WordType.Symbol, "}"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "car"),
						new Word(false, WordType.Symbol, "'"),
					}
				},
				// Line13
				{ (0, 13, 0, 12), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_class_chapter1_line13_comment")),
					}
				},
				// Line14
				{ (0, 13, 0, 13), new List<Word>
					{
						new Word(false, WordType.Variable, "car1"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Class, "Car"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'Toyota'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'blue'"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line15
				{ (0, 13, 0, 14), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_class_chapter1_line15_comment")),
					}
				},
				// Line16
				{ (0, 13, 0, 15), new List<Word>
					{
						new Word(false, WordType.Variable, "car1"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Variable, "color"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'red'"),
					}
				},
				// Line17
				{ (0, 13, 0, 16), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_class_chapter1_line17_comment")),
					}
				},
				// Line18
				{ (0, 13, 0, 17), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "car1"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Function_Custom, "describe"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Lesson Inheritance
				// Chapter1
				// Line1
				{ (0, 14, 0, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter1_line1_comment")),
					}
				},
				// Line2
				{ (0, 14, 0, 1), new List<Word>
					{
						new Word(true, WordType.Reserve, "class"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Class, "Car"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line3
				{ (0, 14, 0, 2), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter1_line3_comment")),
					}
				},
				// Line4
				{ (0, 14, 0, 3), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(true, WordType.Reserve, "def"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.InitializeMethod, "__init__"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Self, "self"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, ""),
						new Word(false, WordType.Variable, "brand"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line5
				{ (0, 14, 0, 4), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter1_line5_comment")),
					}
				},
				// Line6
				{ (0, 14, 0, 5), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Self, "self"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Variable, "brand"),
						new Word(false, WordType.Space, ""),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, ""),
						new Word(false, WordType.Variable, "brand"),
					}
				},
				// Line7
				{ (0, 14, 0, 6), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter1_line7_comment")),
					}
				},
				// Line8
				{ (0, 14, 0, 7), new List<Word>
					{
						new Word(true, WordType.Reserve, "class"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Class, "ElectricCar"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Class, "Car"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line9
				{ (0, 14, 0, 8), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter1_line9_comment")),
					}
				},
				// Line10
				{ (0, 14, 0, 9), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(true, WordType.Reserve, "def"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.InitializeMethod, "__init__"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Self, "self"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, ""),
						new Word(false, WordType.Variable, "brand"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, ""),
						new Word(false, WordType.Variable, "battery_size"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line11
				{ (0, 14, 0, 10), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter1_line11_comment")),
					}
				},
				// Line12
				{ (0, 14, 0, 11), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(true, WordType.Function_BuildIn, "super"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.InitializeMethod, "__init__"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "brand"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line13
				{ (0, 14, 0, 12), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter1_line13_comment")),
					}
				},
				// Line14
				{ (0, 14, 0, 13), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Self, "self"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Variable, "battery_size"),
						new Word(false, WordType.Space, ""),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, ""),
						new Word(false, WordType.Variable, "battery_size"),
					}
				},
				// Line15
				{ (0, 14, 0, 14), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter1_line15_comment")),
					}
				},
				// Line16
				{ (0, 14, 0, 15), new List<Word>
					{
						new Word(false, WordType.Variable, "ev1"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Class, "ElectricCar"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Text, "'Tesla'"),
						new Word(false, WordType.Symbol, ","),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Num, "85"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line17
				{ (0, 14, 0, 16), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter1_line17_comment")),
					}
				},
				// Line18
				{ (0, 14, 0, 17), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "ev1"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Variable, "brand"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Chapter2
				// Line1
				{ (0, 14, 1, 0), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter2_line1_comment")),
					}
				},
				// Line2
				{ (0, 14, 1, 1), new List<Word>
					{
						new Word(true, WordType.Reserve, "class"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Class, "Car"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line3
				{ (0, 14, 1, 2), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter2_line3_comment")),
					}
				},
				// Line4
				{ (0, 14, 1, 3), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(true, WordType.Reserve, "def"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_Define, "describe"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Self, "self"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line5
				{ (0, 14, 1, 4), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter2_line5_comment")),
					}
				},
				// Line6
				{ (0, 14, 1, 5), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(true, WordType.Reserve, "return"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'gasoline car'"),
					}
				},
				// Line7
				{ (0, 14, 1, 6), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter2_line7_comment")),
					}
				},
				// Line8
				{ (0, 14, 1, 7), new List<Word>
					{
						new Word(true, WordType.Reserve, "class"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Class, "ElectricCar"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Class, "Car"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line9
				{ (0, 14, 1, 8), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter2_line9_comment")),
					}
				},
				// Line10
				{ (0, 14, 1, 9), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(true, WordType.Reserve, "def"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_Define, "describe"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Self, "self"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line11
				{ (0, 14, 1, 10), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter2_line11_comment")),
					}
				},
				// Line12
				{ (0, 14, 1, 11), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(true, WordType.Reserve, "return"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'electric car'"),
					}
				},
				// Line13
				{ (0, 14, 1, 12), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter2_line13_comment")),
					}
				},
				// Line14
				{ (0, 14, 1, 13), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(true, WordType.Reserve, "def"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Function_Define, "charge"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Self, "self"),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ":"),
					}
				},
				// Line15
				{ (0, 14, 1, 14), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter2_line15_comment")),
					}
				},
				// Line16
				{ (0, 14, 1, 15), new List<Word>
					{
						new Word(false, WordType.Tab, "\t"),
						new Word(false, WordType.Tab, "\t"),
						new Word(true, WordType.Reserve, "return"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Text, "'charging'"),
					}
				},
				// Line17
				{ (0, 14, 1, 16), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter1_line17_comment")),
					}
				},
				// Line18
				{ (0, 14, 1, 17), new List<Word>
					{
						new Word(false, WordType.Variable, "ev1"),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Operator_Assignment, "="),
						new Word(false, WordType.Space, " "),
						new Word(false, WordType.Class, "ElectricCar"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Symbol, ")"),
					}
				},
				// Line19
				{ (0, 14, 1, 18), new List<Word>
					{
						new Word(false, WordType.Symbol, "#"),
						new Word(false, WordType.Comment, LocalizationManager.Instance.GetText("python_inheritance_chapter2_line19_comment")),
					}
				},
				// Line20
				{ (0, 14, 1, 19), new List<Word>
					{
						new Word(false, WordType.Function_BuildIn, "print"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Variable, "ev1"),
						new Word(false, WordType.Symbol, "."),
						new Word(false, WordType.Function_Custom, "describe"),
						new Word(false, WordType.Symbol, "("),
						new Word(false, WordType.Symbol, ")"),
						new Word(false, WordType.Symbol, ")"),
					}
				},
			};
		}
		// キーを使ってWord[]を取得するメソッド
		public static List<Word> GetWords(int languageNo, int lessonNo, int chapterNo, int lineNo)
		{
			if (_wordMap.TryGetValue((languageNo, lessonNo, chapterNo, lineNo), out List<Word> words))
			{
				return words;
			}
			return null; // キーが存在しない場合はnullを返す（または例外を投げる選択肢もあり）
		}
	}

	public struct Word
	{
		public bool needSpaceNext;
		public WordType wordType;
		public string body;

		public Word(bool needSpace, WordType wordType, string body)
		{
			this.needSpaceNext = needSpace;
			this.wordType = wordType;
			this.body = body;
		}
	}

	public enum WordState
	{
		Normal = 0,
		Text = 1,
		VariableInText = 2
	}

	public enum WordType
	{
		Num = 0,
		Text = 1,
		Comment = 2,
		Variable = 3,
		Reserve = 4,
		Function_BuildIn = 5,
		Function_Custom = 6,
		Function_Define = 7,
		Class = 8,
		Struct = 9,
		Operator_Arithmetic = 10,
		Operator_Comparison = 11,
		Operator_Logic = 12,
		Operator_Assignment = 13,
		Symbol = 14,
		Space = 15,
		Bool = 16,
		FString = 17,
		NewLine = 18,
		Tab = 19,
		Module = 20,
		ExceptionType = 21,
		Self = 22,
		InitializeMethod = 23,
		Cursor = 24
	}

	public enum ProgramLanguage
	{
		Python = 0,
	}

	public enum AudioType
	{
		Start = 0,
		Correct = 1,
		Complete = 2,
	}

	void Awake()
	{
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		audioSources = GetComponents<AudioSource>();
		SetSheet();
	}

	// Update is called once per frame
	void Update()
	{
	}

	public void OnClickButtonPeriod()
	{
		InsertInputWords(new Word(false, WordType.Symbol, "."));
	}

	public void OnClickButtonLeftParentheses()
	{
		InsertInputWords(new Word(false, WordType.Symbol, "("));
	}

	public void OnClickButtonRightParentheses()
	{
		InsertInputWords(new Word(false, WordType.Symbol, ")"));
	}

	public void OnClickButtonSingleQuote()
	{
		InsertInputWords(new Word(false, WordType.Symbol, "'"));
	}

	public void OnClickButtonSpace()
	{
		InsertInputWords(new Word(false, WordType.Space, " "));
	}

	public void OnClickButtonBackSpace()
	{
		BackSpaceInputWords();
	}

	public void OnClickButtonPlus()
	{
		InsertInputWords(new Word(false, WordType.Operator_Arithmetic, "+"));
	}

	public void OnClickButtonMinus()
	{
		InsertInputWords(new Word(false, WordType.Operator_Arithmetic, "-"));
	}

	public void OnClickButtonMultiplication()
	{
		InsertInputWords(new Word(false, WordType.Operator_Arithmetic, "*"));
	}

	public void OnClickButtonDivision()
	{
		InsertInputWords(new Word(false, WordType.Operator_Arithmetic, "/"));
	}

	public void OnClickButtonRemainder()
	{
		InsertInputWords(new Word(false, WordType.Operator_Arithmetic, "%"));
	}

	public void OnClickButtonNumber(int num)
	{
		InsertInputWords(new Word(false, WordType.Num, num.ToString()));
	}

	public void OnClickButtonLeftArrow()
	{
		MoveCursor(true);
	}

	public void OnClickButtonRightArrow()
	{
		MoveCursor(false);
	}

	public void OnClickButtonCustomButton(int id)
	{
		if (id < interfaceWords.Count)
		{
			InsertInputWords(interfaceWords[id]);
		}
	}

	public void OnClickButtonSheetAnser()
	{
		answerPanel.SetActive(sheetAnswerToggleBool.flg);
		UpdateAnswerText();
	}

	public void OnClickButtonSlide()
	{
		slidePanel.SetActive(sheetSlideToggleBool.flg);
	}

	public void OnClickButtonAnswerAll()
	{
		UpdateAnswerText();
	}

	public void OnClickButtonConsoleAnswer()
	{
		if (consoleAnswerToggleBool.flg)
		{
			consoleTextTemp = consoleText.text;
			SetCode(false);
		}
		else
		{
			consoleText.text = consoleTextTemp;
		}
	}

	private void UpdateAnswerText()
	{
		for (int i = 0; i < LINE_NUM; i++)
		{
			if (i <= activeSheet.lineId)
			{
				answerLineTexts[i].gameObject.SetActive(true);
			}
			else
			{
				if (answerSheetAllToggleBool.flg)
				{
					answerLineTexts[i].gameObject.SetActive(true);
				}
				else
				{
					answerLineTexts[i].gameObject.SetActive(false);
				}
			}
		}
	}

	private void BackSpaceInputWords()
	{
		Word cursor = new Word(false, WordType.Cursor, "|");
		activeSheet.lines[activeSheet.lineId].wordId = activeSheet.lines[activeSheet.lineId].inputWords.IndexOf(cursor);

		if (activeSheet.lines[activeSheet.lineId].wordId > 0)
		{
			activeSheet.lines[activeSheet.lineId].inputWords.RemoveAt(activeSheet.lines[activeSheet.lineId].wordId - 1);
		}
		UpdateInputWordsText();
		CheckInputWords();
	}

	private void MoveCursor(bool isLeft)
	{
		Word cursor = new Word(false, WordType.Cursor, "|");
		activeSheet.lines[activeSheet.lineId].wordId = activeSheet.lines[activeSheet.lineId].inputWords.IndexOf(cursor);

		if (isLeft)
		{
			if (activeSheet.lines[activeSheet.lineId].wordId > 0)
			{
				int index = activeSheet.lines[activeSheet.lineId].wordId;
				Swap(activeSheet.lines[activeSheet.lineId].inputWords, index - 1, index);
			}
		}
		else
		{
			if (activeSheet.lines[activeSheet.lineId].wordId < (activeSheet.lines[activeSheet.lineId].inputWords.Count - 1))
			{
				int index = activeSheet.lines[activeSheet.lineId].wordId;
				Swap(activeSheet.lines[activeSheet.lineId].inputWords, index + 1, index);
			}
		}
		UpdateInputWordsText();
	}

	private void InsertInputWords(Word word)
	{	
		Word cursor = new Word(false, WordType.Cursor, "|");
		activeSheet.lines[activeSheet.lineId].wordId = activeSheet.lines[activeSheet.lineId].inputWords.IndexOf(cursor);
		activeSheet.lines[activeSheet.lineId].inputWords.Insert(activeSheet.lines[activeSheet.lineId].wordId, word);

		UpdateInputWordsText();
		CheckInputWords();
	}

	private void CheckInputWords()
	{
		List<Word> input = ConvertSpaceToTab(activeSheet.lines[activeSheet.lineId].inputWords);
		input = RemoveOptionalSpace(input);

		List<Word> answer = RemoveOptionalSpace(activeSheet.lines[activeSheet.lineId].answerWords);

		if (input.SequenceEqual(answer))
		{
			PlayCorrectAudio();

			if (activeSheet.lines[activeSheet.lineId].reqExe)
			{
				consoleAnswerToggleBool.flg = false;
				consoleAnswerToggleBool.GetComponent<UpdateAnimationController>().UpdateAnimatorController();

				SetCode(true);
			}
			SetNextLine();
		}
	}

	private List<Word> ConvertSpaceToTab(List<Word> words)
	{
		List<Word> removeCursor = new List<Word>(words);
		removeCursor.Remove(new Word(false, WordType.Cursor, "|"));

		List<Word> result = new List<Word>();

		for (int i = 0; i < removeCursor.Count; i++)
		{
			if ((removeCursor[i].wordType == WordType.Space && removeCursor[i].body == " ") && ((i + 3) < (removeCursor.Count - 1)))
			{
				if ((removeCursor[i + 1].wordType == WordType.Space && removeCursor[i + 1].body == " ")
				&& (removeCursor[i + 2].wordType == WordType.Space && removeCursor[i + 2].body == " ")
				&& (removeCursor[i + 3].wordType == WordType.Space && removeCursor[i + 3].body == " "))
				{
					result.Add(new Word(false, WordType.Tab, "\t"));
					i += 3; // Skip the next three spaces
				}
				else
				{
					result.Add(removeCursor[i]);
				}
			}
			else
			{
				result.Add(removeCursor[i]);
			}
		}
		return result;
	}

	private List<Word> ConvertTabToSpace(List<Word> words)
	{
		List<Word> result = new List<Word>();

		foreach (Word word in words)
		{
			if (word.wordType == WordType.Tab)
			{
				result.Add(new Word(false, WordType.Space, " "));
				result.Add(new Word(false, WordType.Space, " "));
				result.Add(new Word(false, WordType.Space, " "));
				result.Add(new Word(false, WordType.Space, " "));
			}
			else
			{
				result.Add(word);
			}
		}
		return result;
	}

	private List<Word> RemoveOptionalSpace(List<Word> words)
	{
		List<Word> result = new List<Word>();
		bool essentialSpace = false;
		foreach (Word word in words)
		{
			if (word.wordType == WordType.Cursor)
			{
				continue;
			}
			else if (word.wordType == WordType.Space)
			{
				if (essentialSpace)
				{
					result.Add(word);
				}
			}
			else
			{
				result.Add(word);
			}
			essentialSpace = word.needSpaceNext;
		}
		return result;
	}

	private void SetCode(bool isInput)
	{
		List<Word> input = new List<Word>();
		string code = "";

		for (int i = 0; i < LINE_NUM; i++)
		{
			if ((isInput ? activeSheet.lines[i].inputWords.Count : activeSheet.lines[i].answerWords.Count) == 0)
			{
				continue;
			}
			foreach (Word word in (isInput ? activeSheet.lines[i].inputWords : activeSheet.lines[i].answerWords))
			{
				if (!(word.wordType == WordType.Cursor))
				{
					code += word.body;
				}
			}
			code += "\n";
		}
		if (coroutineRequestExeCode != null)
		{
			StopCoroutine(coroutineRequestExeCode);
		}
		coroutineRequestExeCode = StartCoroutine(SendCodeRequest(code));
	}

	private void SetNextLine()
	{
		DeleteCursor(activeSheet.lineId);

		if (CheckCompleteChapter())
		{
			// Chapter Complete!
			Invoke("PlayCompleteEffect", 2f);
		}
		else
		{
			SetCursor(activeSheet.lineId, activeSheet.lines[activeSheet.lineId].wordId);
			SetInterface();
			UpdateAnswerText();
		}
	}

	private void PlayStartAudio()
	{
		audioSources[(int)AudioType.Start].Stop();
		audioSources[(int)AudioType.Start].Play();
	}

	private void PlayCorrectAudio()
	{
		audioSources[(int)AudioType.Correct].Stop();
		audioSources[(int)AudioType.Correct].Play();
	}

	private void PlayCompleteAudio()
	{
		audioSources[(int)AudioType.Complete].Stop();
		audioSources[(int)AudioType.Complete].Play();
	}

	private void PlayCompleteEffect()
	{
		PlayCompleteAudio();
	}

	private bool CheckCompleteChapter()
	{
		for (int i = activeSheet.lineId + 1; i < LINE_NUM; i++)
		{
			SetLine(activeSheet.lines[i], sheetLineTexts[i], i);
			if (activeSheet.lines[i].reqInput)
			{
				activeSheet.lineId = i;
				return false;
			}
		}
		return true;
	}

	private void SetSheet()
	{
		activeSheet = new Sheet((int)ProgramLanguage.Python, 14, 1);

		for (int i = 0; i < LINE_NUM; i++)
		{
			SetLine(activeSheet.lines[i], sheetLineTexts[i], i);
			if (activeSheet.lines[i].reqInput)
			{
				activeSheet.lineId = i;
				break;
			}
		}

		for (int i = 0; i < LINE_NUM; i++)
		{
			SetAnswerLine(activeSheet.lines[i], answerLineTexts[i]);

			if (activeSheet.lines[i].answerWords.Any(word => word.body.Contains("try")))
			{
				bool isExceptHandle = false;
				activeSheet.lines[i].reqExe = false;

				for (int j = i + 1; j < LINE_NUM; j++)
				{
					if (isExceptHandle && !(activeSheet.lines[j].answerWords.Any(word => word.body.Contains("#"))))
					{
						break;
					}
					if (activeSheet.lines[j].answerWords.Any(word => word.body.Contains("except")))
					{
						isExceptHandle = true;
					}
					activeSheet.lines[j].reqExe = false;
				}
			}
		}
		UpdateAnswerText();

		SetCursor(activeSheet.lineId, activeSheet.lines[activeSheet.lineId].wordId);
		SetInterface();

		InvokeRepeating("ToggleCursorBlinkState", 0.5f, 0.5f);

		//PlayStartAudio();
	}

	private void ToggleCursorBlinkState()
	{
		activeSheet.cursorBlinkState = !activeSheet.cursorBlinkState;
		UpdateInputWordsText();
	}

	private void SetCursor(int lineNo, int wordNo)
	{
		activeSheet.lines[lineNo].wordId = wordNo;
		activeSheet.lines[lineNo].inputWords.Insert(wordNo, new Word(false, WordType.Cursor, "|"));
		UpdateInputWordsText();
	}

	private void DeleteCursor(int lineNo)
	{
		activeSheet.lines[lineNo].wordId = 0;
		activeSheet.lines[lineNo].inputWords.Remove(new Word(false, WordType.Cursor, "|"));
		UpdateInputWordsText();
	}

	private void SetInterface()
	{
		interfaceWords = new List<Word>();

		foreach (Word word in activeSheet.lines[activeSheet.lineId].answerWords)
		{
			if ((word.body == ".")
			|| (word.body == "(")
			|| (word.body == ")")
			|| (word.body == "'")
			|| (word.body == " ")
			|| (word.body == "+")
			|| (word.body == "-")
			|| (word.body == "*")
			|| (word.body == "/")
			|| (word.body == "%")
			|| (word.body == "\t"))
			{
				continue;
			}

			if (!interfaceWords.Contains(word))
			{
				interfaceWords.Add(word);
			}
		}

		Shuffle(interfaceWords);

		for (int i = 0; i < INTERFACE_BUTTON_NUM; i++)
		{
			if (i < interfaceWords.Count)
			{
				interfaceButtons[i].SetActive(true);
				interfaceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = interfaceWords[i].body;
			}
			else
			{
				interfaceButtons[i].SetActive(false);
				interfaceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
			}
		}
		if (interfaceWords.Count <= 4)
		{
			interfaceButtonLineMiddle.SetActive(false);
			interfaceButtonLineBottom.SetActive(false);
		}
		else if (interfaceWords.Count <= 8)
		{
			interfaceButtonLineMiddle.SetActive(true);
			interfaceButtonLineBottom.SetActive(false);
		}
		else
		{
			interfaceButtonLineMiddle.SetActive(true);
			interfaceButtonLineBottom.SetActive(true);
		}
	}

	private void UpdateInputWordsText()
	{
		sheetLineTexts[activeSheet.lineId].color = Color.white;
		sheetLineTexts[activeSheet.lineId].text = "";
		WordState wordState = WordState.Normal;

		foreach (Word word in activeSheet.lines[activeSheet.lineId].inputWords)
		{
			if (word.wordType == WordType.Cursor)
			{
				sheetLineTexts[activeSheet.lineId].text += activeSheet.cursorBlinkState ? "<alpha=#FF>" : "<alpha=#00>";
				sheetLineTexts[activeSheet.lineId].text += word.body;
				sheetLineTexts[activeSheet.lineId].text += "<alpha=#FF>";
			}
			else if (word.wordType == WordType.Operator_Arithmetic)
			{
				sheetLineTexts[activeSheet.lineId].text += GetTextColor("<color=#FFFFFFFF>", wordState);
				sheetLineTexts[activeSheet.lineId].text += word.body;
				sheetLineTexts[activeSheet.lineId].text += "</color>";
			}
			else if (word.wordType == WordType.Operator_Comparison)
			{
				sheetLineTexts[activeSheet.lineId].text += GetTextColor("<color=#FFFFFFFF>", wordState);
				sheetLineTexts[activeSheet.lineId].text += word.body;
				sheetLineTexts[activeSheet.lineId].text += "</color>";
			}
			else if (word.wordType == WordType.Operator_Logic)
			{
				sheetLineTexts[activeSheet.lineId].text += GetTextColor("<color=#FFA500FF>", wordState);
				sheetLineTexts[activeSheet.lineId].text += word.body;
				sheetLineTexts[activeSheet.lineId].text += "</color>";
			}
			else if (word.wordType == WordType.Operator_Assignment)
			{
				sheetLineTexts[activeSheet.lineId].text += GetTextColor("<color=#FFFFFFFF>", wordState);
				sheetLineTexts[activeSheet.lineId].text += word.body;
				sheetLineTexts[activeSheet.lineId].text += "</color>";
			}
			else if (word.wordType == WordType.Function_BuildIn)
			{
				sheetLineTexts[activeSheet.lineId].text += GetTextColor("<color=#8080FFFF>", wordState);
				sheetLineTexts[activeSheet.lineId].text += word.body;
				sheetLineTexts[activeSheet.lineId].text += "</color>";
			}
			else if (word.wordType == WordType.Text)
			{
				sheetLineTexts[activeSheet.lineId].text += GetTextColor("<color=#00FF00FF>", wordState);
				sheetLineTexts[activeSheet.lineId].text += word.body;
				sheetLineTexts[activeSheet.lineId].text += "</color>";
			}
			else if ((word.wordType == WordType.Symbol) && (word.body == "'"))
			{
				sheetLineTexts[activeSheet.lineId].text += "<color=#00FF00FF>";
				sheetLineTexts[activeSheet.lineId].text += word.body;
				sheetLineTexts[activeSheet.lineId].text += "</color>";

				if (wordState == WordState.Text)
				{
					wordState = WordState.Normal;
				}
				else
				{
					wordState = WordState.Text;
				}
			}
			else if (word.wordType == WordType.FString)
			{
				sheetLineTexts[activeSheet.lineId].text += "<color=#00FF00FF>";
				sheetLineTexts[activeSheet.lineId].text += word.body;
				sheetLineTexts[activeSheet.lineId].text += "</color>";
			}
			else if (word.wordType == WordType.Num)
			{
				sheetLineTexts[activeSheet.lineId].text += GetTextColor("<color=#00FFFFFF>", wordState);
				sheetLineTexts[activeSheet.lineId].text += word.body;
				sheetLineTexts[activeSheet.lineId].text += "</color>";
			}
			else if (word.wordType == WordType.Bool)
			{
				sheetLineTexts[activeSheet.lineId].text += GetTextColor("<color=#FFA500FF>", wordState);
				sheetLineTexts[activeSheet.lineId].text += word.body;
				sheetLineTexts[activeSheet.lineId].text += "</color>";
			}
			else if ((word.wordType == WordType.Symbol)
			&& ((word.body == "{") || (word.body == "}")))
			{

				if (wordState == WordState.Text)
				{
					wordState = WordState.VariableInText;
					sheetLineTexts[activeSheet.lineId].text += "<color=#FFA500FF>";
				}
				else if (wordState == WordState.VariableInText)
				{
					wordState = WordState.Text;
					sheetLineTexts[activeSheet.lineId].text += "<color=#FFA500FF>";
				}
				else
				{
					wordState = WordState.Normal;
					sheetLineTexts[activeSheet.lineId].text += "<color=#FFFFFFFF>";

				}
				sheetLineTexts[activeSheet.lineId].text += word.body;
				sheetLineTexts[activeSheet.lineId].text += "</color>";
			}
			else if (word.wordType == WordType.Reserve)
			{
				sheetLineTexts[activeSheet.lineId].text += GetTextColor("<color=#FFA500FF>", wordState);
				sheetLineTexts[activeSheet.lineId].text += word.body;
				sheetLineTexts[activeSheet.lineId].text += "</color>";
			}
			else if (word.wordType == WordType.Function_Define)
			{
				sheetLineTexts[activeSheet.lineId].text += GetTextColor("<color=#0080FFFF>", wordState);
				sheetLineTexts[activeSheet.lineId].text += word.body;
				sheetLineTexts[activeSheet.lineId].text += "</color>";
			}
			else if (word.wordType == WordType.Self)
			{
				sheetLineTexts[activeSheet.lineId].text += GetTextColor("<color=#C040C0FF>", wordState);
				sheetLineTexts[activeSheet.lineId].text += word.body;
				sheetLineTexts[activeSheet.lineId].text += "</color>";
			}
			else if (word.wordType == WordType.InitializeMethod)
			{
				sheetLineTexts[activeSheet.lineId].text += GetTextColor("<color=#FF0080FF>", wordState);
				sheetLineTexts[activeSheet.lineId].text += word.body;
				sheetLineTexts[activeSheet.lineId].text += "</color>";
			}
			else if (word.wordType == WordType.NewLine)
			{
				sheetLineTexts[activeSheet.lineId].text += "<color=#FFA500FF>";
				sheetLineTexts[activeSheet.lineId].text += word.body;
				sheetLineTexts[activeSheet.lineId].text += "</color>";
			}
			else
			{
				sheetLineTexts[activeSheet.lineId].text += GetTextColor("<color=#FFFFFFFF>", wordState);
				sheetLineTexts[activeSheet.lineId].text += word.body;
				sheetLineTexts[activeSheet.lineId].text += "</color>";
			}
		}
	}

	private string GetTextColor(string inputColor, WordState wordState)
	{
		if (wordState == WordState.Text)
		{
			return "<color=#00FF00FF>";
		}
		else
		{
			return inputColor;
		}
	}

	private void SetLine(Line line, TextMeshProUGUI textMeshPro, int index)
	{
		if (line.isComment)
		{
			textMeshPro.color = color_COMMENT;
			textMeshPro.font = LocalizationManager.Instance.GetLocalizedFont();

			List<Word> initialLine = ConvertTabToSpace(line.initialWords);

			foreach (Word word in initialLine)
			{
				textMeshPro.text += word.body;

				if (word.body == "#")
				{
					textMeshPro.text += " ";
				}
			}
		}
		else
		{
			textMeshPro.color = Color.white;
			
			List<Word> answerLine = ConvertTabToSpace(line.answerWords);

			foreach (Word word in answerLine)
			{
				if (word.wordType == WordType.Space)
				{
					textMeshPro.text += word.body;
					activeSheet.lines[index].inputWords.Add(word);
					activeSheet.lines[index].wordId++;
				}
				else
				{
					break;
				}
			}
		}
	}

	private void SetAnswerLine(Line line, TextMeshProUGUI textMeshPro)
	{
		WordState wordState = WordState.Normal;

		if (line.isComment)
		{
			textMeshPro.color = color_COMMENT;
			textMeshPro.font = LocalizationManager.Instance.GetLocalizedFont();

			List<Word> initialLine = ConvertTabToSpace(line.initialWords);

			foreach (Word word in initialLine)
			{
				textMeshPro.text += word.body;

				if (word.body == "#")
				{
					textMeshPro.text += " ";
				}
			}
		}
		else
		{
			textMeshPro.color = Color.white;

			List<Word> answerLine = ConvertTabToSpace(line.answerWords);

			foreach (Word word in answerLine)
			{
				if (word.wordType == WordType.Operator_Arithmetic)
				{
					textMeshPro.text += GetTextColor("<color=#FFFFFFFF>", wordState);
					textMeshPro.text += word.body;
					textMeshPro.text += "</color>";
				}
				else if (word.wordType == WordType.Operator_Comparison)
				{
					textMeshPro.text += GetTextColor("<color=#FFFFFFFF>", wordState);
					textMeshPro.text += word.body;
					textMeshPro.text += "</color>";
				}
				else if (word.wordType == WordType.Operator_Logic)
				{
					textMeshPro.text += GetTextColor("<color=#FFA500FF>", wordState);
					textMeshPro.text += word.body;
					textMeshPro.text += "</color>";
				}
				else if (word.wordType == WordType.Operator_Assignment)
				{
					textMeshPro.text += GetTextColor("<color=#FFFFFFFF>", wordState);
					textMeshPro.text += word.body;
					textMeshPro.text += "</color>";
				}
				else if (word.wordType == WordType.Function_BuildIn)
				{
					textMeshPro.text += GetTextColor("<color=#8080FFFF>", wordState);
					textMeshPro.text += word.body;
					textMeshPro.text += "</color>";
				}
				else if (word.wordType == WordType.Text)
				{
					textMeshPro.text += GetTextColor("<color=#00FF00FF>", wordState);
					textMeshPro.text += word.body;
					textMeshPro.text += "</color>";
				}
				else if ((word.wordType == WordType.Symbol) && (word.body == "'"))
				{
					textMeshPro.text += "<color=#00FF00FF>";
					textMeshPro.text += word.body;
					textMeshPro.text += "</color>";

					if (wordState == WordState.Text)
					{
						wordState = WordState.Normal;
					}
					else
					{
						wordState = WordState.Text;
					}
				}
				else if (word.wordType == WordType.FString)
				{
					textMeshPro.text += "<color=#00FF00FF>";
					textMeshPro.text += word.body;
					textMeshPro.text += "</color>";
				}
				else if (word.wordType == WordType.Num)
				{
					textMeshPro.text += GetTextColor("<color=#00FFFFFF>", wordState);
					textMeshPro.text += word.body;
					textMeshPro.text += "</color>";
				}
				else if (word.wordType == WordType.Bool)
				{
					textMeshPro.text += GetTextColor("<color=#FFA500FF>", wordState);
					textMeshPro.text += word.body;
					textMeshPro.text += "</color>";
				}
				else if ((word.wordType == WordType.Symbol)
				&& ((word.body == "{") || (word.body == "}")))
				{
					if (wordState == WordState.Text)
					{
						wordState = WordState.VariableInText;
						textMeshPro.text += "<color=#FFA500FF>";
					}
					else if (wordState == WordState.VariableInText)
					{
						wordState = WordState.Text;
						textMeshPro.text += "<color=#FFA500FF>";
					}
					else
					{
						wordState = WordState.Normal;
						textMeshPro.text += "<color=#FFFFFFFF>";

					}
					textMeshPro.text += word.body;
					textMeshPro.text += "</color>";
				}
				else if (word.wordType == WordType.Reserve)
				{
					textMeshPro.text += GetTextColor("<color=#FFA500FF>", wordState);
					textMeshPro.text += word.body;
					textMeshPro.text += "</color>";
				}
				else if (word.wordType == WordType.Function_Define)
				{
					textMeshPro.text += GetTextColor("<color=#0080FFFF>", wordState);
					textMeshPro.text += word.body;
					textMeshPro.text += "</color>";
				}
				else if (word.wordType == WordType.Self)
				{
					textMeshPro.text += GetTextColor("<color=#C040C0FF>", wordState);
					textMeshPro.text += word.body;
					textMeshPro.text += "</color>";
				}
				else if (word.wordType == WordType.InitializeMethod)
				{
					textMeshPro.text += GetTextColor("<color=#FF0080FF>", wordState);
					textMeshPro.text += word.body;
					textMeshPro.text += "</color>";
				}
				else if (word.wordType == WordType.NewLine)
				{
					textMeshPro.text += "<color=#FFA500FF>";
					textMeshPro.text += word.body;
					textMeshPro.text += "</color>";
				}
				else
				{
					textMeshPro.text += GetTextColor("<color=#FFFFFFFF>", wordState);
					textMeshPro.text += word.body;
					textMeshPro.text += "</color>";
				}
			}
		}
	}

	private IEnumerator SendCodeRequest(string code)
	{
		try
		{
			// JSONデータを作成
			var requestData = new { code = code };
			string jsonData = JsonConvert.SerializeObject(requestData);
			byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

			// POSTリクエストを設定
			using (UnityWebRequest request = new UnityWebRequest(serverUrl, "POST"))
			{
				request.uploadHandler = new UploadHandlerRaw(bodyRaw);
				request.downloadHandler = new DownloadHandlerBuffer();
				request.SetRequestHeader("Content-Type", "application/json");

				bool isWaiting = true;
				consoleText.text = ".";
				StartCoroutine(ShowProgressDots(() => isWaiting));

				// リクエスト送信
				yield return request.SendWebRequest();

				isWaiting = false;

				if (request.result != UnityWebRequest.Result.Success)
				{
					Debug.Log($"HTTP Error: {request.error}");
					consoleText.text = $"HTTP Error: {request.error}";
				}
				else
				{
					string responseText = request.downloadHandler.text;

					try
					{
						JObject json = JObject.Parse(responseText);
						bool success = json["success"]?.Value<bool>() ?? false;

						if (success)
						{
							string output = json["output"]?.ToString().Replace("\\n", "\n") ?? "No output";
							Debug.Log(output);
							consoleText.text = output;
						}
						else
						{
							string error = json["error"]?.ToString().Replace("\\n", "\n") ?? "Unknown error";
							Debug.Log(error);
							consoleText.text = error;
						}
					}
					catch (JsonException ex)
					{
						Debug.Log($"JSON parse error: {ex.Message}");
					}
				}
			}
		}
		finally
		{
			// コルーチンが完了（または例外で終了）したときに null を設定
			coroutineRequestExeCode = null;
		}
	}

	private IEnumerator ShowProgressDots(System.Func<bool> keepRunning)
	{
		while (keepRunning())
		{
			consoleText.text += ".";
			yield return new WaitForSeconds(0.5f);
		}
	}
}

// サーバーからの応答を格納するクラス
[System.Serializable]
public class ServerResponse
{
	public bool success;
	public string result;
	public string error;
}