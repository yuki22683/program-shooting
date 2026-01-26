    private void InitializeBashLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: Bash (バッシュ) に挑戦！ ====================
        var lesson1 = new Lesson { titleKey = "bash_lesson1_title" };

        // Ex1: 画面にメッセージを出しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson1_ex1_title",
            slideKeyPrefix = "bash_lesson1_ex1",
            slideCount = 4,
            correctLines = new List<string> { "# 画面にメッセージを出す関数", "echo \"Hello, World!\"" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson1_ex1_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: 便利な「はこ」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson1_ex2_title",
            slideKeyPrefix = "bash_lesson1_ex2",
            slideCount = 4,
            correctLines = new List<string> { "# 'Bash' と入力する", "name=\"Bash\"", "# はこの名前 'name' と入力する", "echo $name" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson1_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "bash_lesson1_ex2_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: コンピュータで計算しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson1_ex3_title",
            slideKeyPrefix = "bash_lesson1_ex3",
            slideCount = 3,
            correctLines = new List<string> { "# はこに数字を入れる", "a=5", "b=3", "# +でたし算", "echo $((a + b))" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson1_ex3_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "bash_lesson1_ex3_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: 「もし〜なら」で分けましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson1_ex4_title",
            slideKeyPrefix = "bash_lesson1_ex4",
            slideCount = 4,
            correctLines = new List<string> { "# scoreに100を入れる", "score=100", "# -gtで「より大きい」を指定", "if [ $score -gt 80 ]; then", "    # メッセージ（'Great'）", "    echo \"Great\"", "# fiでif文を閉じる", "fi" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson1_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "bash_lesson1_ex4_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "bash_lesson1_ex4_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "#", localizationKey = "bash_lesson1_ex4_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: ちがう場合はどうしましょう？
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson1_ex5_title",
            slideKeyPrefix = "bash_lesson1_ex5",
            slideCount = 3,
            correctLines = new List<string> { "# ageに18を入れる", "age=18", "# 20以上かを比較する演算子", "if [ $age -ge 20 ]; then", "    # 20歳以上のときのメッセージ（'Adult'）", "    echo \"Adult\"", "# elseでそれ以外の場合", "else", "    # それ以外のメッセージ（'Minor'）", "    echo \"Minor\"", "fi" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson1_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "bash_lesson1_ex5_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "bash_lesson1_ex5_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "#", localizationKey = "bash_lesson1_ex5_comment4" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "#", localizationKey = "bash_lesson1_ex5_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: 中身を全部出してみましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson1_ex6_title",
            slideKeyPrefix = "bash_lesson1_ex6",
            slideCount = 3,
            correctLines = new List<string> { "names=(\"Alice\" \"Bob\")", "# inで配列の中身を取り出す", "for name in \"${names[@]}\"; do", "    echo $name", "# doneでループを終了", "done" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "#", localizationKey = "bash_lesson1_ex6_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "bash_lesson1_ex6_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: 自分だけの関数を作りましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson1_ex7_title",
            slideKeyPrefix = "bash_lesson1_ex7",
            slideCount = 3,
            correctLines = new List<string> { "greet() {", "    echo \"Hello\"", "}", "# greetで関数を呼び出す", "greet" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "bash_lesson1_ex7_comment1" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: Bash II - スクリプトと自動化 ====================
        var lesson2 = new Lesson { titleKey = "bash_lesson2_title" };

        // Ex1: 配列を使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson2_ex1_title",
            slideKeyPrefix = "bash_lesson2_ex1",
            slideCount = 3,
            correctLines = new List<string> { "# 配列を定義（10, 20, 30）", "nums=(10 20 30)", "# 3番目の要素にアクセスするインデックス（0から数える）", "echo ${nums[2]}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson2_ex1_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "bash_lesson2_ex1_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: 文字列の長さを取得
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson2_ex2_title",
            slideKeyPrefix = "bash_lesson2_ex2",
            slideCount = 3,
            correctLines = new List<string> { "# 変数に文字列を代入", "word=\"Bash\"", "# 文字列の長さを取得する記号", "echo ${#word}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson2_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "bash_lesson2_ex2_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: 文字列の部分取得
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson2_ex3_title",
            slideKeyPrefix = "bash_lesson2_ex3",
            slideCount = 3,
            correctLines = new List<string> { "# 変数に文字列を代入", "text=\"Hello World\"", "# Worldが始まる位置（0から数える）", "echo ${text:6:5}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson2_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "bash_lesson2_ex3_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: 関数に引数を渡す
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson2_ex4_title",
            slideKeyPrefix = "bash_lesson2_ex4",
            slideCount = 3,
            correctLines = new List<string> { "# 関数を定義", "add() {", "    # 最初の引数を参照する変数", "    echo $(($1 + $2))", "}", "# 関数を呼び出す", "add 3 5" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson2_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "bash_lesson2_ex4_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "bash_lesson2_ex4_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: 戻り値を使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson2_ex5_title",
            slideKeyPrefix = "bash_lesson2_ex5",
            slideCount = 3,
            correctLines = new List<string> { "# 関数を定義", "square() {", "    # 引数を2乗する", "    echo $(($1 * $1))", "}", "# コマンドの出力を取得する構文", "result=$(square 4)", "# 結果を表示", "echo $result" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson2_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "bash_lesson2_ex5_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "bash_lesson2_ex5_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "#", localizationKey = "bash_lesson2_ex5_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: while ループ
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson2_ex6_title",
            slideKeyPrefix = "bash_lesson2_ex6",
            slideCount = 3,
            correctLines = new List<string> { "# 変数を初期化", "i=1", "# 条件が真の間繰り返すキーワード", "while [ $i -le 3 ]; do", "    # 変数を表示", "    echo $i", "    # 変数をインクリメント", "    i=$((i + 1))", "done" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson2_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "bash_lesson2_ex6_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "bash_lesson2_ex6_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "#", localizationKey = "bash_lesson2_ex6_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: case で分岐
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson2_ex7_title",
            slideKeyPrefix = "bash_lesson2_ex7",
            slideCount = 3,
            correctLines = new List<string> { "# 変数に値を代入", "fruit=\"apple\"", "# パターンマッチで分岐するキーワード", "case $fruit in", "    # appleの場合", "    apple) echo \"red\";;", "    # bananaの場合", "    banana) echo \"yellow\";;", "    # その他の場合", "    *) echo \"unknown\";;", "esac" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson2_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "bash_lesson2_ex7_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "bash_lesson2_ex7_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "#", localizationKey = "bash_lesson2_ex7_comment4" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "#", localizationKey = "bash_lesson2_ex7_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: コマンド置換
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson2_ex8_title",
            slideKeyPrefix = "bash_lesson2_ex8",
            slideCount = 3,
            correctLines = new List<string> { "# コマンドの出力を変数に格納する構文", "files=$(echo \"test\")", "# 変数を表示", "echo \"Files: $files\"" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson2_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "bash_lesson2_ex8_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: read で入力を受け取る
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson2_ex9_title",
            slideKeyPrefix = "bash_lesson2_ex9",
            slideCount = 3,
            correctLines = new List<string> { "# 標準入力を変数に読み込むコマンド", "echo \"input:\" && read val && echo \"You entered: $val\"" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson2_ex9_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: ヒアドキュメント
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson2_ex10_title",
            slideKeyPrefix = "bash_lesson2_ex10",
            slideCount = 3,
            correctLines = new List<string> { "# ヒアドキュメントを開始する演算子", "cat <<END", "# 1行目のテキストを入力", "Hello", "# 2行目のテキストを入力", "World", "END" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson2_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "bash_lesson2_ex10_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "bash_lesson2_ex10_comment3" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: Bash III - ファイルとパイプライン ====================
        var lesson3 = new Lesson { titleKey = "bash_lesson3_title" };

        // Ex1: パイプでコマンドをつなぐ
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson3_ex1_title",
            slideKeyPrefix = "bash_lesson3_ex1",
            slideCount = 3,
            correctLines = new List<string> { "# |でパイプを使う", "echo \"hello world\" | wc -w" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson3_ex1_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: ファイルにリダイレクト
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson3_ex2_title",
            slideKeyPrefix = "bash_lesson3_ex2",
            slideCount = 3,
            correctLines = new List<string> { "# >でファイルに出力", "echo \"test\" > /tmp/out.txt && cat /tmp/out.txt" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson3_ex2_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: ファイルから読み込む
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson3_ex3_title",
            slideKeyPrefix = "bash_lesson3_ex3",
            slideCount = 3,
            correctLines = new List<string> { "echo \"line1\" > /tmp/in.txt", "# <でファイルから読み込む", "wc -l < /tmp/in.txt" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "#", localizationKey = "bash_lesson3_ex3_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: エラー出力をリダイレクト
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson3_ex4_title",
            slideKeyPrefix = "bash_lesson3_ex4",
            slideCount = 3,
            correctLines = new List<string> { "# 2>でエラー出力をリダイレクト", "ls /nonexistent 2> /dev/null && echo \"ok\" || echo \"error hidden\"" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson3_ex4_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: テストでファイルを確認
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson3_ex5_title",
            slideKeyPrefix = "bash_lesson3_ex5",
            slideCount = 3,
            correctLines = new List<string> { "# -fでファイルが存在するか確認", "if [ -f /etc/passwd ]; then", "    echo \"file exists\"", "fi" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson3_ex5_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: xargs でコマンドに渡す
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson3_ex6_title",
            slideKeyPrefix = "bash_lesson3_ex6",
            slideCount = 3,
            correctLines = new List<string> { "# xargsで引数に変換", "echo \"hello world\" | xargs echo \"Message:\"" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson3_ex6_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: sed で置換
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson3_ex7_title",
            slideKeyPrefix = "bash_lesson3_ex7",
            slideCount = 3,
            correctLines = new List<string> { "# sedで置換", "echo \"cat\" | sed 's/cat/dog/'" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson3_ex7_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: awk でフィールドを抽出
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson3_ex8_title",
            slideKeyPrefix = "bash_lesson3_ex8",
            slideCount = 3,
            correctLines = new List<string> { "# awkでフィールドを抽出", "echo \"apple 100 yen\" | awk '{print $2}'" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson3_ex8_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: sort で並べ替え
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson3_ex9_title",
            slideKeyPrefix = "bash_lesson3_ex9",
            slideCount = 3,
            correctLines = new List<string> { "# sortで並べ替え", "echo -e \"banana\\napple\\ncherry\" | sort" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson3_ex9_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: uniq で重複を除去
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "bash_lesson3_ex10_title",
            slideKeyPrefix = "bash_lesson3_ex10",
            slideCount = 3,
            correctLines = new List<string> { "# uniqで重複を除去", "echo -e \"a\\nb\\na\\nb\" | sort | uniq" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "bash_lesson3_ex10_comment1" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson3);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;
    }