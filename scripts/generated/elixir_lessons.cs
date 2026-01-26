    private void InitializeElixirLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: Elixir (エリクサー) に挑戦！ ====================
        var lesson1 = new Lesson { titleKey = "elixir_lesson1_title" };

        // Ex1: 画面に文字を出してみましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson1_ex1_title",
            slideKeyPrefix = "elixir_lesson1_ex1",
            slideCount = 4,
            correctLines = new List<string> { "# Hello, Elixir!を表示", "IO.puts \"Hello, Elixir!\"" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson1_ex1_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: 便利な「はこ」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson1_ex2_title",
            slideKeyPrefix = "elixir_lesson1_ex2",
            slideCount = 3,
            correctLines = new List<string> { "# 10を入力", "x = 10", "# xを入力", "IO.puts x" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson1_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson1_ex2_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: コンピュータで計算しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson1_ex3_title",
            slideKeyPrefix = "elixir_lesson1_ex3",
            slideCount = 3,
            correctLines = new List<string> { "# 5を入力", "a = 5", "# 3を入力", "b = 3", "# +でたし算", "IO.puts a + b" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson1_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson1_ex3_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "elixir_lesson1_ex3_comment3" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: Elixir II - パターンマッチと並行処理 ====================
        var lesson2 = new Lesson { titleKey = "elixir_lesson2_title" };

        // Ex1: パターンマッチングの基本
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson2_ex1_title",
            slideKeyPrefix = "elixir_lesson2_ex1",
            slideCount = 3,
            correctLines = new List<string> { "# yでタプルの2番目を受け取る", "{x, y} = {10, 20}", "# xを入力", "IO.puts x" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson2_ex1_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson2_ex1_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: リストのパターンマッチ
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson2_ex2_title",
            slideKeyPrefix = "elixir_lesson2_ex2",
            slideCount = 3,
            correctLines = new List<string> { "# |でリストを分解", "[head | tail] = [1, 2, 3]", "# headを入力", "IO.puts head" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson2_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson2_ex2_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: パイプ演算子を使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson2_ex3_title",
            slideKeyPrefix = "elixir_lesson2_ex3",
            slideCount = 3,
            correctLines = new List<string> { "# 1..5で範囲を作成", "1..5", "# |>でパイプ", "|> Enum.sum()", "# |>でパイプ", "|> IO.puts()" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson2_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson2_ex3_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "elixir_lesson2_ex3_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: Enum.map で変換
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson2_ex4_title",
            slideKeyPrefix = "elixir_lesson2_ex4",
            slideCount = 3,
            correctLines = new List<string> { "# mapで各要素を変換", "result = Enum.map([1, 2, 3], fn x -> x * x end)", "# resultを入力", "IO.inspect result" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson2_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson2_ex4_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: Enum.filter で絞り込み
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson2_ex5_title",
            slideKeyPrefix = "elixir_lesson2_ex5",
            slideCount = 3,
            correctLines = new List<string> { "# filterで条件に合う要素を抽出", "result = Enum.filter([1, 2, 3, 4, 5], fn x -> x >= 3 end)", "# resultを入力", "IO.inspect result" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson2_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson2_ex5_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: case でパターンマッチ
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson2_ex6_title",
            slideKeyPrefix = "elixir_lesson2_ex6",
            slideCount = 3,
            correctLines = new List<string> { "# 2を入力", "x = 2", "# caseでパターンマッチ", "result = case x do", "  1 -> \"one\"", "  2 -> \"two\"", "  _ -> \"other\"", "end", "# resultを入力", "IO.puts result" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson2_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson2_ex6_comment2" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "#", localizationKey = "elixir_lesson2_ex6_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: 関数を定義しよう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson2_ex7_title",
            slideKeyPrefix = "elixir_lesson2_ex7",
            slideCount = 3,
            correctLines = new List<string> { "# defmoduleでモジュールを定義", "defmodule Calc do", "  # defで関数を定義", "  def triple(x), do: x * 3", "end", "# Calc.triple(4)を入力", "IO.puts Calc.triple(4)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson2_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson2_ex7_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "elixir_lesson2_ex7_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: 再帰で計算しよう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson2_ex8_title",
            slideKeyPrefix = "elixir_lesson2_ex8",
            slideCount = 3,
            correctLines = new List<string> { "# defmoduleでモジュールを定義", "defmodule Math do", "  def factorial(0), do: 1", "  # factorialを再帰的に呼び出す", "  def factorial(n), do: n * factorial(n - 1)", "end", "# Math.factorial(5)を入力", "IO.puts Math.factorial(5)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson2_ex8_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "elixir_lesson2_ex8_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "#", localizationKey = "elixir_lesson2_ex8_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: プロセスを作ろう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson2_ex9_title",
            slideKeyPrefix = "elixir_lesson2_ex9",
            slideCount = 3,
            correctLines = new List<string> { "# spawnでプロセスを作成", "spawn(fn -> IO.puts \"hello\" end)", "# 100を入力", "Process.sleep(100)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson2_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson2_ex9_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: メッセージを送ろう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson2_ex10_title",
            slideKeyPrefix = "elixir_lesson2_ex10",
            slideCount = 3,
            correctLines = new List<string> { "# sendでメッセージを送る", "send(self(), {:msg, 42})", "# receiveでメッセージを受け取る", "receive do", "  {:msg, n} -> IO.puts n", "end" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson2_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson2_ex10_comment2" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: Elixir III - 構造体とプロトコル ====================
        var lesson3 = new Lesson { titleKey = "elixir_lesson3_title" };

        // Ex1: 構造体（Struct）
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson3_ex1_title",
            slideKeyPrefix = "elixir_lesson3_ex1",
            slideCount = 2,
            correctLines = new List<string> { "# defmoduleでモジュールを定義", "defmodule Person do", "  # defstructで構造体を定義", "  defstruct name: \"\", email: \"\"", "# endでモジュールを閉じる", "end", "", "# nameに\"Bob\"、emailに\"bob@example.com\"を指定して構造体を作成", "person = %Person{name: \"Bob\", email: \"bob@example.com\"}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson3_ex1_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson3_ex1_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "elixir_lesson3_ex1_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "#", localizationKey = "elixir_lesson3_ex1_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: 構造体の更新
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson3_ex2_title",
            slideKeyPrefix = "elixir_lesson3_ex2",
            slideCount = 2,
            correctLines = new List<string> { "# defmoduleでモジュールを定義", "defmodule Product do", "  # defstructで構造体を定義", "  defstruct name: \"\", price: 0", "# endでモジュールを閉じる", "end", "", "# nameに\"Book\"、priceに1000を指定して構造体を作成", "product = %Product{name: \"Book\", price: 1000}", "# |で辞書を更新（priceを1200に）", "updated = %{product | price: 1200}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson3_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson3_ex2_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "elixir_lesson3_ex2_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "#", localizationKey = "elixir_lesson3_ex2_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "#", localizationKey = "elixir_lesson3_ex2_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: Enum.reduce
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson3_ex3_title",
            slideKeyPrefix = "elixir_lesson3_ex3",
            slideCount = 2,
            correctLines = new List<string> { "# リストを定義", "numbers = [1, 2, 3, 4, 5]", "# reduceで畳み込み", "sum = Enum.reduce(numbers, 0, fn x, acc -> x + acc end)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson3_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson3_ex3_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: Enum.group_by
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson3_ex4_title",
            slideKeyPrefix = "elixir_lesson3_ex4",
            slideCount = 2,
            correctLines = new List<string> { "# Alice(25), Bob(30), Carol(25) のタプルのリストを定義", "users = [{\"Alice\", 25}, {\"Bob\", 30}, {\"Carol\", 25}]", "# group_byでグループ化", "grouped = Enum.group_by(users, fn {_name, age} -> age end)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson3_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson3_ex4_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: 内包表記（for）
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson3_ex5_title",
            slideKeyPrefix = "elixir_lesson3_ex5",
            slideCount = 2,
            correctLines = new List<string> { "# forで内包表記", "squares = for x <- 1..5, do: x * x" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson3_ex5_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: with式
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson3_ex6_title",
            slideKeyPrefix = "elixir_lesson3_ex6",
            slideCount = 2,
            correctLines = new List<string> { "# defで関数を定義", "def process(map) do", "  # withでパターンマッチを連鎖", "  with {:ok, name} <- Map.fetch(map, :name),", "       {:ok, age} <- Map.fetch(map, :age) do", "    {:ok, \"#{name} is #{age} years old\"}", "  # elseでエラー処理", "  else", "    :error -> {:error, \"Missing field\"}", "  # endでブロックを閉じる", "  end", "# endで関数を閉じる", "end" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson3_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson3_ex6_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "#", localizationKey = "elixir_lesson3_ex6_comment3" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "#", localizationKey = "elixir_lesson3_ex6_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "#", localizationKey = "elixir_lesson3_ex6_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: プロトコルの定義
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson3_ex7_title",
            slideKeyPrefix = "elixir_lesson3_ex7",
            slideCount = 2,
            correctLines = new List<string> { "# defprotocolでプロトコルを定義", "defprotocol Describable do", "  # defで関数を宣言", "  def describe(data)", "# endでプロトコルを閉じる", "end" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson3_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson3_ex7_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "elixir_lesson3_ex7_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: プロトコルの実装
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson3_ex8_title",
            slideKeyPrefix = "elixir_lesson3_ex8",
            slideCount = 2,
            correctLines = new List<string> { "# defimplでプロトコルを実装", "defimpl Describable, for: Map do", "  # defで関数を定義", "  def describe(map) do", "    \"Map with #{map_size(map)} keys\"", "  # endで関数を閉じる", "  end", "# endでブロックを閉じる", "end" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson3_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson3_ex8_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "elixir_lesson3_ex8_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "#", localizationKey = "elixir_lesson3_ex8_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: Agent
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson3_ex9_title",
            slideKeyPrefix = "elixir_lesson3_ex9",
            slideCount = 2,
            correctLines = new List<string> { "# start_linkでAgentを開始", "{:ok, counter} = Agent.start_link(fn -> 0 end)", "# updateでAgentの状態を更新", "Agent.update(counter, fn state -> state + 1 end)", "# getでAgentの状態を取得", "value = Agent.get(counter, fn state -> state end)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson3_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson3_ex9_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "elixir_lesson3_ex9_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: Task
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "elixir_lesson3_ex10_title",
            slideKeyPrefix = "elixir_lesson3_ex10",
            slideCount = 2,
            correctLines = new List<string> { "# asyncで非同期タスクを開始", "task = Task.async(fn -> 1 + 2 end)", "# awaitでタスクの結果を待機", "result = Task.await(task)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "elixir_lesson3_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "elixir_lesson3_ex10_comment2" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson3);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;
    }