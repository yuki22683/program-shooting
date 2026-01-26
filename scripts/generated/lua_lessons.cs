    private void InitializeLuaLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: Lua (ルア) に挑戦！ ====================
        var lesson1 = new Lesson { titleKey = "lua_lesson1_title" };

        // Ex1: 画面にメッセージを出してみましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "lua_lesson1_ex1_title",
            slideKeyPrefix = "lua_lesson1_ex1",
            slideCount = 4,
            correctLines = new List<string> { "-- Hello, Lua!を表示", "print(\"Hello, Lua!\")" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "lua_lesson1_ex1_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: 自分専用の「はこ」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "lua_lesson1_ex2_title",
            slideKeyPrefix = "lua_lesson1_ex2",
            slideCount = 3,
            correctLines = new List<string> { "-- 10を入力", "local x = 10", "-- xを入力", "print(x)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "lua_lesson1_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "lua_lesson1_ex2_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: コンピュータで計算しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "lua_lesson1_ex3_title",
            slideKeyPrefix = "lua_lesson1_ex3",
            slideCount = 3,
            correctLines = new List<string> { "-- 5を入力", "local a = 5", "-- 3を入力", "local b = 3", "-- +でたし算", "print(a + b)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "lua_lesson1_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "lua_lesson1_ex3_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "--", localizationKey = "lua_lesson1_ex3_comment3" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson1);

        // ==================== LESSON 3: Lua III - エラー処理とモジュール ====================
        var lesson3 = new Lesson { titleKey = "lua_lesson3_title" };

        // Ex1: pcall でエラーを捕捉
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "lua_lesson3_ex1_title",
            slideKeyPrefix = "lua_lesson3_ex1",
            slideCount = 3,
            correctLines = new List<string> { "-- pcallでエラーを捕捉", "local ok, result = pcall(function()", "    return 10 + 5", "end)", "-- 成功したら出力", "if ok then", "    -- 結果を出力", "    print(result)", "end" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "lua_lesson3_ex1_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "--", localizationKey = "lua_lesson3_ex1_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "--", localizationKey = "lua_lesson3_ex1_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: error でエラーを発生
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "lua_lesson3_ex2_title",
            slideKeyPrefix = "lua_lesson3_ex2",
            slideCount = 3,
            correctLines = new List<string> { "local ok, msg = pcall(function()", "    -- errorでエラーを発生", "    error(\"test error\")", "end)", "print(msg:match(\"test error\") and \"caught\" or \"not caught\")" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "--", localizationKey = "lua_lesson3_ex2_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: assert で検証
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "lua_lesson3_ex3_title",
            slideKeyPrefix = "lua_lesson3_ex3",
            slideCount = 3,
            correctLines = new List<string> { "-- assertで検証", "local value = assert(10 > 5, \"should be true\")", "print(value and \"pass\" or \"fail\")" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "lua_lesson3_ex3_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: 型を確認しよう
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "lua_lesson3_ex4_title",
            slideKeyPrefix = "lua_lesson3_ex4",
            slideCount = 3,
            correctLines = new List<string> { "-- typeで型を確認", "local t = {}", "-- 型を出力", "print(type(t))" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "lua_lesson3_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "lua_lesson3_ex4_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: tostring で文字列に変換
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "lua_lesson3_ex5_title",
            slideKeyPrefix = "lua_lesson3_ex5",
            slideCount = 3,
            correctLines = new List<string> { "-- tostringで文字列に変換", "local n = 100", "-- 文字列に変換して連結", "print(\"Value: \" .. tostring(n))" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "lua_lesson3_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "lua_lesson3_ex5_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: tonumber で数値に変換
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "lua_lesson3_ex6_title",
            slideKeyPrefix = "lua_lesson3_ex6",
            slideCount = 3,
            correctLines = new List<string> { "-- tonumberで数値に変換", "local s = \"50\"", "-- 数値に変換", "local n = tonumber(s)", "print(n + 10)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "lua_lesson3_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "lua_lesson3_ex6_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: 文字列を連結
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "lua_lesson3_ex7_title",
            slideKeyPrefix = "lua_lesson3_ex7",
            slideCount = 3,
            correctLines = new List<string> { "-- ..で文字列を連結", "local a = \"Lua\"", "local b = \"Script\"", "-- 連結して出力", "print(a .. b)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "lua_lesson3_ex7_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "--", localizationKey = "lua_lesson3_ex7_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: 文字列の長さを取得
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "lua_lesson3_ex8_title",
            slideKeyPrefix = "lua_lesson3_ex8",
            slideCount = 3,
            correctLines = new List<string> { "-- #で長さを取得", "local s = \"Lua\"", "-- 長さを出力", "print(#s)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "lua_lesson3_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "lua_lesson3_ex8_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: 条件演算子のパターン
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "lua_lesson3_ex9_title",
            slideKeyPrefix = "lua_lesson3_ex9",
            slideCount = 3,
            correctLines = new List<string> { "-- and/orで条件分岐", "local age = 20", "-- 条件に応じて値を決める", "local status = age >= 18 and \"adult\" or \"child\"", "print(status)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "lua_lesson3_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "lua_lesson3_ex9_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: ループを制御しよう
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "lua_lesson3_ex10_title",
            slideKeyPrefix = "lua_lesson3_ex10",
            slideCount = 3,
            correctLines = new List<string> { "-- breakでループを抜ける", "for i = 1, 10 do", "    -- 3を超えたらループを抜ける", "    if i > 3 then break end", "    print(i)", "end" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "lua_lesson3_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "lua_lesson3_ex10_comment2" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson3);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;
    }