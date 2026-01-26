    private void InitializeSQLLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: SQL (エスキューエル) に挑戦！ ====================
        var lesson1 = new Lesson { titleKey = "sql_lesson1_title" };

        // Ex1: 画面に文字を出してみましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson1_ex1_title",
            slideKeyPrefix = "sql_lesson1_ex1",
            slideCount = 4,
            correctLines = new List<string> { "-- SELECTでデータを取得", "SELECT 'Hello, World!';" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson1_ex1_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: データに名前をつけましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson1_ex2_title",
            slideKeyPrefix = "sql_lesson1_ex2",
            slideCount = 4,
            correctLines = new List<string> { "-- 文字にgreetingという名前をつける", "SELECT 'こんにちは' AS greeting;" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson1_ex2_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: 数字を足し算しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson1_ex3_title",
            slideKeyPrefix = "sql_lesson1_ex3",
            slideCount = 4,
            correctLines = new List<string> { "-- 100と50を足し算する", "SELECT 100 + 50 AS total;" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson1_ex3_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: いくつもの値を表示しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson1_ex4_title",
            slideKeyPrefix = "sql_lesson1_ex4",
            slideCount = 4,
            correctLines = new List<string> { "-- 名前と年齢を表示", "SELECT 'Taro' AS name, 10 AS age;" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson1_ex4_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: 条件で絞り込みましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson1_ex5_title",
            slideKeyPrefix = "sql_lesson1_ex5",
            slideCount = 4,
            correctLines = new List<string> { "-- 10より大きいかチェック", "SELECT 1 WHERE 15 > 10;" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson1_ex5_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: 文字をつなげましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson1_ex6_title",
            slideKeyPrefix = "sql_lesson1_ex6",
            slideCount = 4,
            correctLines = new List<string> { "-- 文字をつなげる", "SELECT 'SQL' || 'は楽しい！';" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson1_ex6_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: 大きい順に並べ替えましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson1_ex7_title",
            slideKeyPrefix = "sql_lesson1_ex7",
            slideCount = 4,
            correctLines = new List<string> { "-- 降順で並べる", "SELECT 1 AS num ORDER BY num DESC;" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson1_ex7_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: 条件を組み合わせましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson1_ex8_title",
            slideKeyPrefix = "sql_lesson1_ex8",
            slideCount = 4,
            correctLines = new List<string> { "-- 両方の条件をチェック", "SELECT 1 WHERE 10 > 5 AND 20 > 10;" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson1_ex8_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: CASEで条件分岐しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson1_ex9_title",
            slideKeyPrefix = "sql_lesson1_ex9",
            slideCount = 4,
            correctLines = new List<string> { "-- 条件分岐", "SELECT CASE WHEN 10 > 5 THEN '大きい' ELSE '小さい' END;" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson1_ex9_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: NULLをチェックしましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson1_ex10_title",
            slideKeyPrefix = "sql_lesson1_ex10",
            slideCount = 4,
            correctLines = new List<string> { "-- NULLチェック", "SELECT CASE WHEN NULL IS NULL THEN 1 ELSE 0 END;" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson1_ex10_comment1" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: SQL II - テーブル操作とJOIN ====================
        var lesson2 = new Lesson { titleKey = "sql_lesson2_title" };

        // Ex1: WHERE で絞り込み
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson2_ex1_title",
            slideKeyPrefix = "sql_lesson2_ex1",
            slideCount = 3,
            correctLines = new List<string> { "-- WHEREで条件を指定", "SELECT 'found' WHERE 10 > 5;" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson2_ex1_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: ORDER BY で並べ替え
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson2_ex2_title",
            slideKeyPrefix = "sql_lesson2_ex2",
            slideCount = 4,
            correctLines = new List<string> { "-- SELECT文でデータを作成", "SELECT 1 AS num UNION SELECT 3 UNION SELECT 2", "-- ORDERで並べ替え", "ORDER BY num;" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson2_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "sql_lesson2_ex2_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: LIMIT で件数制限
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson2_ex3_title",
            slideKeyPrefix = "sql_lesson2_ex3",
            slideCount = 3,
            correctLines = new List<string> { "-- SELECT文でデータを作成", "SELECT 1 UNION SELECT 2 UNION SELECT 3", "-- LIMITで件数制限", "LIMIT 2;" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson2_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "sql_lesson2_ex3_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: COUNT で件数を数える
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson2_ex4_title",
            slideKeyPrefix = "sql_lesson2_ex4",
            slideCount = 3,
            correctLines = new List<string> { "-- COUNTで行数を数える", "SELECT COUNT(*) FROM (SELECT 1 UNION SELECT 2 UNION SELECT 3);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson2_ex4_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: SUM で合計を計算
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson2_ex5_title",
            slideKeyPrefix = "sql_lesson2_ex5",
            slideCount = 3,
            correctLines = new List<string> { "-- SUMで合計を計算", "SELECT SUM(n) FROM (SELECT 1 AS n UNION SELECT 2 UNION SELECT 3);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson2_ex5_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: AVG で平均を計算
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson2_ex6_title",
            slideKeyPrefix = "sql_lesson2_ex6",
            slideCount = 3,
            correctLines = new List<string> { "-- AVGで平均を計算", "SELECT AVG(n) FROM (SELECT 10 AS n UNION SELECT 20 UNION SELECT 30);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson2_ex6_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: GROUP BY でグループ化
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson2_ex7_title",
            slideKeyPrefix = "sql_lesson2_ex7",
            slideCount = 3,
            correctLines = new List<string> { "-- SELECT文でデータを取得", "SELECT category, COUNT(*) FROM (", "  -- カテゴリを定義", "  SELECT 'A' AS category UNION ALL", "  -- データを追加", "  SELECT 'A' UNION ALL SELECT 'B'", "-- GROUPでグループ化", ") GROUP BY category;" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson2_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "sql_lesson2_ex7_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "--", localizationKey = "sql_lesson2_ex7_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "--", localizationKey = "sql_lesson2_ex7_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: HAVING でグループを絞り込み
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson2_ex8_title",
            slideKeyPrefix = "sql_lesson2_ex8",
            slideCount = 3,
            correctLines = new List<string> { "-- SELECT文でデータを取得", "SELECT category, COUNT(*) AS cnt FROM (", "  -- カテゴリを定義", "  SELECT 'A' AS category UNION ALL", "  -- データを追加", "  SELECT 'A' UNION ALL SELECT 'B'", "-- HAVINGでグループを絞り込み", ") GROUP BY category HAVING cnt > 1;" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson2_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "sql_lesson2_ex8_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "--", localizationKey = "sql_lesson2_ex8_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "--", localizationKey = "sql_lesson2_ex8_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: CASE で条件分岐
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson2_ex9_title",
            slideKeyPrefix = "sql_lesson2_ex9",
            slideCount = 4,
            correctLines = new List<string> { "-- SELECT CASE と入力して条件分岐", "SELECT CASE", "  -- 条件を指定", "  WHEN 85 >= 80 THEN 'pass'", "  -- それ以外の場合", "  ELSE 'fail'", "-- ENDで終了", "END;" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson2_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "sql_lesson2_ex9_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "--", localizationKey = "sql_lesson2_ex9_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "--", localizationKey = "sql_lesson2_ex9_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: COALESCE でNULL処理
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson2_ex10_title",
            slideKeyPrefix = "sql_lesson2_ex10",
            slideCount = 3,
            correctLines = new List<string> { "-- COALESCEでNULL処理", "SELECT COALESCE(NULL, 'default');" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson2_ex10_comment1" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: SQL III - サブクエリと高度な操作 ====================
        var lesson3 = new Lesson { titleKey = "sql_lesson3_title" };

        // Ex1: サブクエリ（WHERE内）
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson3_ex1_title",
            slideKeyPrefix = "sql_lesson3_ex1",
            slideCount = 3,
            correctLines = new List<string> { "-- SELECTでサブクエリを作成", "SELECT 'found' WHERE 5 > (SELECT 3);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson3_ex1_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: EXISTS で存在チェック
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson3_ex2_title",
            slideKeyPrefix = "sql_lesson3_ex2",
            slideCount = 3,
            correctLines = new List<string> { "-- EXISTSで存在チェック", "SELECT 'has data' WHERE EXISTS (SELECT 1);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson3_ex2_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: UNION で結合
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson3_ex3_title",
            slideKeyPrefix = "sql_lesson3_ex3",
            slideCount = 3,
            correctLines = new List<string> { "-- UNIONで結果を結合", "SELECT 'A' UNION SELECT 'B' UNION SELECT 'C';" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson3_ex3_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: INNER JOIN
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson3_ex4_title",
            slideKeyPrefix = "sql_lesson3_ex4",
            slideCount = 3,
            correctLines = new List<string> { "-- SELECTで列を取得", "SELECT a.x, b.y FROM", "  -- テーブルaを作成", "  (SELECT 1 AS id, 'A' AS x) a", "  -- INNERで内部結合", "  INNER JOIN", "  -- テーブルbを作成", "  (SELECT 1 AS id, 'B' AS y) b", "  -- 結合条件を指定", "  ON a.id = b.id;" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson3_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "sql_lesson3_ex4_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "--", localizationKey = "sql_lesson3_ex4_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "--", localizationKey = "sql_lesson3_ex4_comment4" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "--", localizationKey = "sql_lesson3_ex4_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: LEFT JOIN
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson3_ex5_title",
            slideKeyPrefix = "sql_lesson3_ex5",
            slideCount = 3,
            correctLines = new List<string> { "-- SELECTで列を取得", "SELECT a.x, b.y FROM", "  -- テーブルaを作成", "  (SELECT 1 AS id, 'A' AS x) a", "  -- LEFTで左外部結合", "  LEFT JOIN", "  -- テーブルbを作成", "  (SELECT 2 AS id, 'B' AS y) b", "  -- 結合条件を指定", "  ON a.id = b.id;" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson3_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "sql_lesson3_ex5_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "--", localizationKey = "sql_lesson3_ex5_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "--", localizationKey = "sql_lesson3_ex5_comment4" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "--", localizationKey = "sql_lesson3_ex5_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: MAX と MIN
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson3_ex6_title",
            slideKeyPrefix = "sql_lesson3_ex6",
            slideCount = 3,
            correctLines = new List<string> { "-- MAXで最大値を取得", "SELECT MAX(n) FROM (SELECT 5 AS n UNION SELECT 10 UNION SELECT 3);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson3_ex6_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: DISTINCT で重複除去
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson3_ex7_title",
            slideKeyPrefix = "sql_lesson3_ex7",
            slideCount = 3,
            correctLines = new List<string> { "-- SELECT DISTINCT と入力して重複除去", "SELECT DISTINCT n FROM (", "  -- データを作成", "  SELECT 1 AS n UNION ALL", "  -- 重複データを追加", "  SELECT 1 UNION ALL", "  -- 異なるデータを追加", "  SELECT 2", ");" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson3_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "sql_lesson3_ex7_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "--", localizationKey = "sql_lesson3_ex7_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "--", localizationKey = "sql_lesson3_ex7_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: LIKE でパターン検索
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson3_ex8_title",
            slideKeyPrefix = "sql_lesson3_ex8",
            slideCount = 3,
            correctLines = new List<string> { "-- LIKEでパターン検索", "SELECT 'matched' WHERE 'Hello' LIKE 'H%';" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson3_ex8_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: BETWEEN で範囲指定
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson3_ex9_title",
            slideKeyPrefix = "sql_lesson3_ex9",
            slideCount = 3,
            correctLines = new List<string> { "-- BETWEENで範囲指定", "SELECT 'in range' WHERE 5 BETWEEN 1 AND 10;" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson3_ex9_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: IN で複数値マッチ
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson3_ex10_title",
            slideKeyPrefix = "sql_lesson3_ex10",
            slideCount = 3,
            correctLines = new List<string> { "-- INで複数値マッチ", "SELECT 'found' WHERE 'B' IN ('A', 'B', 'C');" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "sql_lesson3_ex10_comment1" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson3);

        // ==================== LESSON 4: SQL IV - CTEとウィンドウ関数 ====================
        var lesson4 = new Lesson { titleKey = "sql_lesson4_title" };

        // Ex1: CTE（WITH句）
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson4_ex1_title",
            slideKeyPrefix = "sql_lesson4_ex1",
            slideCount = 3,
            correctLines = new List<string> { "WITH nums AS (", "  SELECT 1 AS n UNION SELECT 2 UNION SELECT 3", ")", "SELECT * FROM nums;" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex2: ROW_NUMBER()
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson4_ex2_title",
            slideKeyPrefix = "sql_lesson4_ex2",
            slideCount = 3,
            correctLines = new List<string> { "SELECT", "  val,", "  ROW_NUMBER() OVER (ORDER BY val) AS rn", "FROM (SELECT 'A' AS val UNION SELECT 'B' UNION SELECT 'C');" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex3: RANK()
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson4_ex3_title",
            slideKeyPrefix = "sql_lesson4_ex3",
            slideCount = 3,
            correctLines = new List<string> { "SELECT", "  val,", "  RANK() OVER (ORDER BY val) AS rnk", "FROM (SELECT 1 AS val UNION ALL SELECT 1 UNION ALL SELECT 2);" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex4: SUM() OVER
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson4_ex4_title",
            slideKeyPrefix = "sql_lesson4_ex4",
            slideCount = 3,
            correctLines = new List<string> { "SELECT", "  val,", "  SUM(val) OVER (ORDER BY val) AS running", "FROM (SELECT 1 AS val UNION ALL SELECT 2 UNION ALL SELECT 3);" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex5: CASE WHEN
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson4_ex5_title",
            slideKeyPrefix = "sql_lesson4_ex5",
            slideCount = 3,
            correctLines = new List<string> { "SELECT", "  CASE WHEN 1 > 0 THEN 'yes' ELSE 'no' END AS result;" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex6: COALESCE
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson4_ex6_title",
            slideKeyPrefix = "sql_lesson4_ex6",
            slideCount = 3,
            correctLines = new List<string> { "SELECT COALESCE(NULL, NULL, 'default') AS val;" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex7: INSERT 文
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson4_ex7_title",
            slideKeyPrefix = "sql_lesson4_ex7",
            slideCount = 3,
            correctLines = new List<string> { "CREATE TABLE test(x TEXT);", "INSERT INTO test VALUES ('hello');", "SELECT * FROM test;" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex8: UPDATE 文
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson4_ex8_title",
            slideKeyPrefix = "sql_lesson4_ex8",
            slideCount = 3,
            correctLines = new List<string> { "CREATE TABLE test(x TEXT);", "INSERT INTO test VALUES ('old');", "UPDATE test SET x = 'new';", "SELECT * FROM test;" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex9: DELETE 文
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson4_ex9_title",
            slideKeyPrefix = "sql_lesson4_ex9",
            slideCount = 3,
            correctLines = new List<string> { "CREATE TABLE test(x INT);", "INSERT INTO test VALUES (1), (2), (3);", "DELETE FROM test WHERE x = 2;", "SELECT * FROM test;" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex10: GROUP_CONCAT
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "sql_lesson4_ex10_title",
            slideKeyPrefix = "sql_lesson4_ex10",
            slideCount = 3,
            correctLines = new List<string> { "SELECT GROUP_CONCAT(val, '-') FROM", "  (SELECT 'A' AS val UNION SELECT 'B' UNION SELECT 'C');" },
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson4);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;
    }