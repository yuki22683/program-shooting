    private void InitializeHaskellLessons()
    {
        lessons.Clear();

        // ==================== LESSON 2: Haskell II - 関数と型 ====================
        var lesson2 = new Lesson { titleKey = "haskell_lesson2_title" };

        // Ex1: 関数を定義しよう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson2_ex1_title",
            slideKeyPrefix = "haskell_lesson2_ex1",
            slideCount = 3,
            correctLines = new List<string> { "-- *で掛け算", "triple x = x * 3", "-- tripleを入力", "main = print (triple 4)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson2_ex1_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "haskell_lesson2_ex1_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: 型注釈を書こう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson2_ex2_title",
            slideKeyPrefix = "haskell_lesson2_ex2",
            slideCount = 3,
            correctLines = new List<string> { "-- ::で型注釈", "square :: Int -> Int", "-- *で掛け算", "square x = x * x", "-- squareを入力", "main = print (square 5)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson2_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "haskell_lesson2_ex2_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "--", localizationKey = "haskell_lesson2_ex2_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: リストを使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson2_ex3_title",
            slideKeyPrefix = "haskell_lesson2_ex3",
            slideCount = 3,
            correctLines = new List<string> { "-- [1, 2, 3]を入力", "nums = [1, 2, 3]", "-- headで先頭要素を取得", "main = print (head nums)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson2_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "haskell_lesson2_ex3_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: map で変換しよう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson2_ex4_title",
            slideKeyPrefix = "haskell_lesson2_ex4",
            slideCount = 3,
            correctLines = new List<string> { "-- mapで各要素を変換", "main = print (map (*2) [1, 2, 3])" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson2_ex4_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: filter で絞り込もう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson2_ex5_title",
            slideKeyPrefix = "haskell_lesson2_ex5",
            slideCount = 3,
            correctLines = new List<string> { "-- filterで条件に合う要素を抽出", "main = print (filter (>=3) [1, 2, 3, 4, 5])" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson2_ex5_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: パターンマッチを使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson2_ex6_title",
            slideKeyPrefix = "haskell_lesson2_ex6",
            slideCount = 3,
            correctLines = new List<string> { "-- 0を入力", "fib 0 = 0", "-- 1を入力", "fib 1 = 1", "-- 2を引いて再帰", "fib n = fib (n - 1) + fib (n - 2)", "-- fibを入力", "main = print (fib 6)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson2_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "haskell_lesson2_ex6_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "--", localizationKey = "haskell_lesson2_ex6_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "--", localizationKey = "haskell_lesson2_ex6_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: ガードで条件分岐
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson2_ex7_title",
            slideKeyPrefix = "haskell_lesson2_ex7",
            slideCount = 3,
            correctLines = new List<string> { "-- xを入力", "sign x", "  -- >で大なり比較", "  | x > 0     = \"positive\"", "  -- <で小なり比較", "  | x < 0     = \"negative\"", "  -- otherwiseはそれ以外の場合", "  | otherwise = \"zero\"", "-- signを入力", "main = putStrLn (sign 5)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson2_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "haskell_lesson2_ex7_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "--", localizationKey = "haskell_lesson2_ex7_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "--", localizationKey = "haskell_lesson2_ex7_comment4" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "--", localizationKey = "haskell_lesson2_ex7_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: ラムダ式を使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson2_ex8_title",
            slideKeyPrefix = "haskell_lesson2_ex8",
            slideCount = 3,
            correctLines = new List<string> { "-- \\でラムダ式を開始", "main = print (map (\\x -> x * x) [1, 2, 3])" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson2_ex8_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: リスト内包表記
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson2_ex9_title",
            slideKeyPrefix = "haskell_lesson2_ex9",
            slideCount = 3,
            correctLines = new List<string> { "-- <-でリストから要素を取り出す", "main = print [x * x | x <- [1..5]]" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson2_ex9_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: foldr で畳み込み
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson2_ex10_title",
            slideKeyPrefix = "haskell_lesson2_ex10",
            slideCount = 3,
            correctLines = new List<string> { "-- foldrで畳み込み", "main = print (foldr (+) 0 [1, 2, 3, 4, 5])" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson2_ex10_comment1" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: Haskell III - 高階関数とモナド ====================
        var lesson3 = new Lesson { titleKey = "haskell_lesson3_title" };

        // Ex1: map関数
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson3_ex1_title",
            slideKeyPrefix = "haskell_lesson3_ex1",
            slideCount = 2,
            correctLines = new List<string> { "-- Intを入力", "double :: Int -> Int", "-- *で掛け算", "double x = x * 2", "", "-- 各要素に関数を適用してリストを変換する関数", "doubled = map double [1, 2, 3, 4, 5]" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson3_ex1_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "haskell_lesson3_ex1_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "--", localizationKey = "haskell_lesson3_ex1_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: filter関数
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson3_ex2_title",
            slideKeyPrefix = "haskell_lesson3_ex2",
            slideCount = 2,
            correctLines = new List<string> { "-- Boolを入力", "isEven :: Int -> Bool", "-- ==で等価比較", "isEven x = x `mod` 2 == 0", "", "-- 条件を満たす要素だけを抽出する関数", "evens = filter isEven [1, 2, 3, 4, 5, 6]" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson3_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "haskell_lesson3_ex2_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "--", localizationKey = "haskell_lesson3_ex2_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: ラムダ式
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson3_ex3_title",
            slideKeyPrefix = "haskell_lesson3_ex3",
            slideCount = 2,
            correctLines = new List<string> { "-- ラムダ式を開始する記号", "squared = map (\\x -> x * x) [1, 2, 3, 4, 5]" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson3_ex3_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: foldr（右畳み込み）
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson3_ex4_title",
            slideKeyPrefix = "haskell_lesson3_ex4",
            slideCount = 2,
            correctLines = new List<string> { "-- Intを入力", "sumList :: [Int] -> Int", "-- リストを右から畳み込む関数", "sumList xs = foldr (+) 0 xs", "-- sumListを入力", "total = sumList [1, 2, 3, 4, 5]" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson3_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "haskell_lesson3_ex4_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "--", localizationKey = "haskell_lesson3_ex4_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: 関数合成
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson3_ex5_title",
            slideKeyPrefix = "haskell_lesson3_ex5",
            slideCount = 2,
            correctLines = new List<string> { "-- Intを入力", "squareDouble :: Int -> Int", "-- 2つの関数を合成する演算子", "squareDouble = (^2) . (*2)", "-- squareDoubleを入力", "result = squareDouble 3" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson3_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "haskell_lesson3_ex5_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "--", localizationKey = "haskell_lesson3_ex5_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: Maybe型
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson3_ex6_title",
            slideKeyPrefix = "haskell_lesson3_ex6",
            slideCount = 2,
            correctLines = new List<string> { "-- Maybe Intを入力", "safeDiv :: Int -> Int -> Maybe Int", "-- 値がないことを表すMaybeのコンストラクタ", "safeDiv _ 0 = Nothing", "-- Justを入力", "safeDiv x y = Just (x `div` y)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson3_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "haskell_lesson3_ex6_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "--", localizationKey = "haskell_lesson3_ex6_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: Either型
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson3_ex7_title",
            slideKeyPrefix = "haskell_lesson3_ex7",
            slideCount = 2,
            correctLines = new List<string> { "-- Either String Intを入力", "validateAge :: Int -> Either String Int", "-- ageを入力", "validateAge age", "  -- 失敗・エラーを表すEitherのコンストラクタ", "  | age < 0   = Left \"Age cannot be negative\"", "  -- Rightを入力", "  | otherwise = Right age" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson3_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "haskell_lesson3_ex7_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "--", localizationKey = "haskell_lesson3_ex7_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "--", localizationKey = "haskell_lesson3_ex7_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: fmap（ファンクタ）
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson3_ex8_title",
            slideKeyPrefix = "haskell_lesson3_ex8",
            slideCount = 2,
            correctLines = new List<string> { "-- Maybe Intを入力", "doubleJust :: Maybe Int -> Maybe Int", "-- Functor内の値に関数を適用する関数", "doubleJust = fmap (*2)", "-- doubleJustを入力", "result = doubleJust (Just 5)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson3_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "haskell_lesson3_ex8_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "--", localizationKey = "haskell_lesson3_ex8_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: do記法
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson3_ex9_title",
            slideKeyPrefix = "haskell_lesson3_ex9",
            slideCount = 2,
            correctLines = new List<string> { "-- Maybe Intを入力", "addMaybe :: Maybe Int -> Maybe Int -> Maybe Int", "-- モナド操作を順次実行する記法", "addMaybe mx my = do", "  -- mxを入力", "  x <- mx", "  -- myを入力", "  y <- my", "  -- +で足し算", "  return (x + y)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson3_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "haskell_lesson3_ex9_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "--", localizationKey = "haskell_lesson3_ex9_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "--", localizationKey = "haskell_lesson3_ex9_comment4" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "--", localizationKey = "haskell_lesson3_ex9_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: リスト内包表記
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "haskell_lesson3_ex10_title",
            slideKeyPrefix = "haskell_lesson3_ex10",
            slideCount = 2,
            correctLines = new List<string> { "-- [Int]を入力", "squares :: [Int]", "-- リスト内包表記で式と生成器を区切る記号", "squares = [x * x | x <- [1..5]]" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "--", localizationKey = "haskell_lesson3_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "--", localizationKey = "haskell_lesson3_ex10_comment2" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson3);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;
    }