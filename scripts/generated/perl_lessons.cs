    private void InitializePerlLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: Perl (パール) に挑戦！ ====================
        var lesson1 = new Lesson { titleKey = "perl_lesson1_title" };

        // Ex1: 画面に文字を出してみましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson1_ex1_title",
            slideKeyPrefix = "perl_lesson1_ex1",
            slideCount = 5,
            correctLines = new List<string> { "# printで出力", "print \"Hello, World!\\n\";" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson1_ex1_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: 変数を使ってみましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson1_ex2_title",
            slideKeyPrefix = "perl_lesson1_ex2",
            slideCount = 4,
            correctLines = new List<string> { "# 変数に文字を入れる", "my $message = \"Perl\";", "print \"$message\\n\";" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson1_ex2_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: 計算をしてみましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson1_ex3_title",
            slideKeyPrefix = "perl_lesson1_ex3",
            slideCount = 4,
            correctLines = new List<string> { "# 足し算する", "my $a = 7;", "my $b = 3;", "# 2つの変数を足し算する", "my $sum = $a + $b;", "print \"$sum\\n\";" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson1_ex3_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "#", localizationKey = "perl_lesson1_ex3_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: 文字をつなげましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson1_ex4_title",
            slideKeyPrefix = "perl_lesson1_ex4",
            slideCount = 4,
            correctLines = new List<string> { "# 文字をつなげる", "my $greeting = \"Hello, \" . \"Perl!\";", "print \"$greeting\\n\";" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson1_ex4_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: 配列を使いましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson1_ex5_title",
            slideKeyPrefix = "perl_lesson1_ex5",
            slideCount = 4,
            correctLines = new List<string> { "# 配列を作る", "my @fruits = (\"りんご\", \"みかん\", \"ぶどう\");", "print \"$fruits[0]\\n\";" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson1_ex5_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: if文で条件分岐しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson1_ex6_title",
            slideKeyPrefix = "perl_lesson1_ex6",
            slideCount = 4,
            correctLines = new List<string> { "# 条件分岐", "my $score = 100;", "# もしスコアが100なら", "if ($score == 100) {", "  print \"満点！\\n\";", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson1_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "perl_lesson1_ex6_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: if-elseを使いましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson1_ex7_title",
            slideKeyPrefix = "perl_lesson1_ex7",
            slideCount = 4,
            correctLines = new List<string> { "# if-else", "my $num = 5;", "if ($num >= 10) {", "  print \"大きい\\n\";", "# そうでなければ", "} else {", "  print \"小さい\\n\";", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson1_ex7_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "perl_lesson1_ex7_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: foreachでループしましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson1_ex8_title",
            slideKeyPrefix = "perl_lesson1_ex8",
            slideCount = 4,
            correctLines = new List<string> { "# 配列をループ", "my @animals = (\"犬\", \"猫\", \"鳥\");", "# 配列の各要素について繰り返す", "foreach my $animal (@animals) {", "  print \"$animal\\n\";", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson1_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "perl_lesson1_ex8_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: ハッシュを使いましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson1_ex9_title",
            slideKeyPrefix = "perl_lesson1_ex9",
            slideCount = 4,
            correctLines = new List<string> { "# ハッシュを作る", "my %fruit = (\"color\" => \"赤\");", "print \"$fruit{color}\\n\";" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson1_ex9_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: サブルーチンを作りましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson1_ex10_title",
            slideKeyPrefix = "perl_lesson1_ex10",
            slideCount = 4,
            correctLines = new List<string> { "# サブルーチンを定義", "sub welcome {", "  print \"Welcome!\\n\";", "}", "", "# 呼び出し", "welcome();" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson1_ex10_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "perl_lesson1_ex10_comment2" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: Perl II - 正規表現とリファレンス ====================
        var lesson2 = new Lesson { titleKey = "perl_lesson2_title" };

        // Ex1: 正規表現でマッチング
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson2_ex1_title",
            slideKeyPrefix = "perl_lesson2_ex1",
            slideCount = 3,
            correctLines = new List<string> { "# 変数に文字列を代入", "my $text = \"hello world\";", "# =~でパターンマッチング", "if ($text =~ /world/) {", "    # printで出力", "    print \"found\\n\";", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson2_ex1_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "perl_lesson2_ex1_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "perl_lesson2_ex1_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: 正規表現で置換
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson2_ex2_title",
            slideKeyPrefix = "perl_lesson2_ex2",
            slideCount = 3,
            correctLines = new List<string> { "# 変数に文字列を代入", "my $text = \"cat\";", "# sで置換", "$text =~ s/cat/dog/;", "# printで出力", "print \"$text\\n\";" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson2_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "perl_lesson2_ex2_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "perl_lesson2_ex2_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: 配列を使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson2_ex3_title",
            slideKeyPrefix = "perl_lesson2_ex3",
            slideCount = 3,
            correctLines = new List<string> { "# @で配列を宣言", "my @fruits = (\"apple\", \"banana\");", "# printで出力", "print $fruits[1] . \"\\n\";" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson2_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "perl_lesson2_ex3_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: ハッシュを使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson2_ex4_title",
            slideKeyPrefix = "perl_lesson2_ex4",
            slideCount = 3,
            correctLines = new List<string> { "# %でハッシュを宣言", "my %scores = (\"math\" => 90);", "# printで出力", "print $scores{\"math\"} . \"\\n\";" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson2_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "perl_lesson2_ex4_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: foreach でループ
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson2_ex5_title",
            slideKeyPrefix = "perl_lesson2_ex5",
            slideCount = 3,
            correctLines = new List<string> { "# 配列を宣言（1, 2, 3）", "my @nums = (1, 2, 3);", "# foreachで各要素を処理", "foreach my $n (@nums) {", "    # printで出力", "    print \"$n\\n\";", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson2_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "perl_lesson2_ex5_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "perl_lesson2_ex5_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: サブルーチンを作ろう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson2_ex6_title",
            slideKeyPrefix = "perl_lesson2_ex6",
            slideCount = 3,
            correctLines = new List<string> { "# subでサブルーチンを定義", "sub double {", "    # shiftで引数を取得", "    my $n = shift;", "    # returnで戻り値を返す", "    return $n * 2;", "}", "# printで出力", "print double(5) . \"\\n\";" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson2_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "perl_lesson2_ex6_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "perl_lesson2_ex6_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "#", localizationKey = "perl_lesson2_ex6_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: リファレンスを学ぼう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson2_ex7_title",
            slideKeyPrefix = "perl_lesson2_ex7",
            slideCount = 3,
            correctLines = new List<string> { "# 配列を宣言（10, 20）", "my @nums = (10, 20);", "# \\でリファレンスを作成", "my $ref = \\@nums;", "# printで出力", "print $ref->[1] . \"\\n\";" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson2_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "perl_lesson2_ex7_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "perl_lesson2_ex7_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: 無名配列リファレンス
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson2_ex8_title",
            slideKeyPrefix = "perl_lesson2_ex8",
            slideCount = 3,
            correctLines = new List<string> { "# [で無名配列を作成", "my $ref = [5, 10, 15];", "# printで出力", "print $ref->[2] . \"\\n\";" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson2_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "perl_lesson2_ex8_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: map で変換
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson2_ex9_title",
            slideKeyPrefix = "perl_lesson2_ex9",
            slideCount = 3,
            correctLines = new List<string> { "# 配列を宣言（1, 2, 3）", "my @nums = (1, 2, 3);", "# mapで各要素を変換", "my @squared = map { $_ * $_ } @nums;", "# printで出力", "print \"@squared\\n\";" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson2_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "perl_lesson2_ex9_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "perl_lesson2_ex9_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: grep でフィルタ
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson2_ex10_title",
            slideKeyPrefix = "perl_lesson2_ex10",
            slideCount = 3,
            correctLines = new List<string> { "# 配列を宣言（1, 2, 3, 4, 5）", "my @nums = (1, 2, 3, 4, 5);", "# grepで条件に合う要素を抽出", "my @result = grep { $_ >= 3 } @nums;", "# printで出力", "print \"@result\\n\";" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "perl_lesson2_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "perl_lesson2_ex10_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "perl_lesson2_ex10_comment3" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: Perl III - ファイル処理と高度な機能 ====================
        var lesson3 = new Lesson { titleKey = "perl_lesson3_title" };

        // Ex1: ファイルを開こう
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson3_ex1_title",
            slideKeyPrefix = "perl_lesson3_ex1",
            slideCount = 3,
            correctLines = new List<string> { "open(my $fh, \">\", \"/tmp/test.txt\") or die;", "print $fh \"hello\\n\";", "close($fh);", "print \"written\\n\";" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex2: ファイルを閉じよう
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson3_ex2_title",
            slideKeyPrefix = "perl_lesson3_ex2",
            slideCount = 3,
            correctLines = new List<string> { "open(my $fh, \">\", \"/tmp/test2.txt\") or die;", "print $fh \"data\\n\";", "close($fh);", "print \"closed\\n\";" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex3: chomp で改行を除去
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson3_ex3_title",
            slideKeyPrefix = "perl_lesson3_ex3",
            slideCount = 3,
            correctLines = new List<string> { "my $text = \"hello\\n\";", "chomp($text);", "print \"[$text]\\n\";" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex4: split で文字列を分割
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson3_ex4_title",
            slideKeyPrefix = "perl_lesson3_ex4",
            slideCount = 3,
            correctLines = new List<string> { "my $data = \"apple:banana:cherry\";", "my @fruits = split(/:/, $data);", "print $fruits[1] . \"\\n\";" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex5: join で配列を結合
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson3_ex5_title",
            slideKeyPrefix = "perl_lesson3_ex5",
            slideCount = 3,
            correctLines = new List<string> { "my @words = (\"Hello\", \"World\");", "my $sentence = join(\" \", @words);", "print \"$sentence\\n\";" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex6: push で配列に追加
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson3_ex6_title",
            slideKeyPrefix = "perl_lesson3_ex6",
            slideCount = 3,
            correctLines = new List<string> { "my @nums = (1, 2);", "push(@nums, 3);", "print \"@nums\\n\";" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex7: pop で配列から取り出す
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson3_ex7_title",
            slideKeyPrefix = "perl_lesson3_ex7",
            slideCount = 3,
            correctLines = new List<string> { "my @nums = (10, 20, 30);", "my $last = pop(@nums);", "print \"$last\\n\";" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex8: sort で配列を並べ替え
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson3_ex8_title",
            slideKeyPrefix = "perl_lesson3_ex8",
            slideCount = 3,
            correctLines = new List<string> { "my @words = (\"banana\", \"apple\", \"cherry\");", "my @sorted = sort @words;", "print \"$sorted[0]\\n\";" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex9: reverse で配列を逆順に
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson3_ex9_title",
            slideKeyPrefix = "perl_lesson3_ex9",
            slideCount = 3,
            correctLines = new List<string> { "my @nums = (1, 2, 3);", "my @rev = reverse @nums;", "print \"@rev\\n\";" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex10: length で文字列の長さ
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "perl_lesson3_ex10_title",
            slideKeyPrefix = "perl_lesson3_ex10",
            slideCount = 3,
            correctLines = new List<string> { "my $text = \"Perl\";", "my $len = length($text);", "print \"$len\\n\";" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson3);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;
    }