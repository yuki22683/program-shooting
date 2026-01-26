    private void InitializeCSharpLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: C# (シーシャープ) に挑戦！ ====================
        var lesson1 = new Lesson { titleKey = "csharp_lesson1_title" };

        // Ex1: 画面にメッセージを出しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex1_title",
            slideKeyPrefix = "csharp_lesson1_ex1",
            slideCount = 3,
            correctLines = new List<string> { "using System;", "", "class Program {", "    static void Main() {", "        // 画面にメッセージを出す関数", "        Console.WriteLine(\"Hello, C#!\");", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "csharp_lesson1_ex1_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: 便利な「はこ」変数（へんすう）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex2_title",
            slideKeyPrefix = "csharp_lesson1_ex2",
            slideCount = 3,
            correctLines = new List<string> { "// nameというはこに \"CSharp\" を入れる", "string name = \"CSharp\";", "// はこの中身を画面に出す", "Console.WriteLine(name);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson1_ex2_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: コンピュータで計算しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex3_title",
            slideKeyPrefix = "csharp_lesson1_ex3",
            slideCount = 3,
            correctLines = new List<string> { "// xというはこに 10 を入れる", "int x = 10;", "// yというはこに 5 を入れる", "int y = 5;", "// x と y をたした答えを出す", "Console.WriteLine(x + y);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson1_ex3_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "csharp_lesson1_ex3_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: 剰余演算子（%）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex4_title",
            slideKeyPrefix = "csharp_lesson1_ex4",
            slideCount = 3,
            correctLines = new List<string> { "// 10 を 3 で割ったあまりを出力する", "Console.WriteLine(10 % 3);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex4_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: 累算代入演算子（+=、-=）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex5_title",
            slideKeyPrefix = "csharp_lesson1_ex5",
            slideCount = 3,
            correctLines = new List<string> { "// hp に 100 を入れる", "int hp = 100;", "// += で 20 を足す", "hp += 20;", "// -= で 50 を引く", "hp -= 50;", "Console.WriteLine(hp);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson1_ex5_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "csharp_lesson1_ex5_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: 文章の中に変数を入れましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex6_title",
            slideKeyPrefix = "csharp_lesson1_ex6",
            slideCount = 3,
            correctLines = new List<string> { "// ageというはこに 10 を入れる", "int age = 10;", "// 文字列補間を使ってメッセージを出す", "Console.WriteLine($\"私は{age}歳です\");" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson1_ex6_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: たくさんのデータをまとめましょう「配列」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex7_title",
            slideKeyPrefix = "csharp_lesson1_ex7",
            slideCount = 3,
            correctLines = new List<string> { "// colorsという配列を作る", "string[] colors = {\"赤\", \"青\", \"緑\"};", "// 2番目のデータを出す", "Console.WriteLine(colors[1]);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson1_ex7_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: 「もし〜なら」で分ける if文
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex8_title",
            slideKeyPrefix = "csharp_lesson1_ex8",
            slideCount = 3,
            correctLines = new List<string> { "// scoreに100を入れる", "int score = 100;", "// もし80より大きかったら", "if (score > 80)", "{", "    // メッセージを表示する", "    Console.WriteLine(\"合格！\");", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson1_ex8_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson1_ex8_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: ちがう場合は？ if-else文
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex9_title",
            slideKeyPrefix = "csharp_lesson1_ex9",
            slideCount = 3,
            correctLines = new List<string> { "// ageに10を入れる", "int age = 10;", "// 20歳以上かどうかで分ける", "if (age >= 20)", "{", "    // 大人というメッセージを出力", "    Console.WriteLine(\"大人\");", "}", "// else でそれ以外の場合", "else", "{", "    // それ以外の場合", "    Console.WriteLine(\"子供\");", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson1_ex9_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson1_ex9_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "csharp_lesson1_ex9_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "csharp_lesson1_ex9_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: 論理演算子（&&、||）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex10_title",
            slideKeyPrefix = "csharp_lesson1_ex10",
            slideCount = 3,
            correctLines = new List<string> { "// score と bonus を定義", "int score = 80;", "int bonus = 10;", "// && で両方の条件をチェック", "if (score >= 70 && bonus > 0)", "{", "    Console.WriteLine(\"ボーナスあり合格\");", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex10_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "csharp_lesson1_ex10_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex11: ぐるぐる回す foreach
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex11_title",
            slideKeyPrefix = "csharp_lesson1_ex11",
            slideCount = 3,
            correctLines = new List<string> { "// 名前の配列を作る", "string[] names = {\"太郎\", \"花子\"};", "// 順番に取り出すループ", "foreach (string name in names)", "{", "    // 取り出した名前を表示", "    Console.WriteLine(name);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex11_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson1_ex11_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson1_ex11_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex12: 名前で探しましょう「Dictionary」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex12_title",
            slideKeyPrefix = "csharp_lesson1_ex12",
            slideCount = 3,
            correctLines = new List<string> { "// Dictionaryを作る", "var fruits = new Dictionary<string, string>();", "// キーと値を追加", "fruits[\"みかん\"] = \"オレンジ\";", "// キーを指定して値を取り出す", "Console.WriteLine(fruits[\"みかん\"]);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex12_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson1_ex12_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "csharp_lesson1_ex12_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex13: 自分だけの関数を作ろう「メソッド」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson1_ex13_title",
            slideKeyPrefix = "csharp_lesson1_ex13",
            slideCount = 3,
            correctLines = new List<string> { "// Greetというメソッドを定義", "static void Greet()", "{", "    // こんにちは と表示", "    Console.WriteLine(\"こんにちは\");", "}", "// メソッドを呼び出す", "Greet();" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson1_ex13_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "csharp_lesson1_ex13_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "csharp_lesson1_ex13_comment3" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: C# II - オブジェクト指向とLINQ ====================
        var lesson2 = new Lesson { titleKey = "csharp_lesson2_title" };

        // Ex1: クラスの継承
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex1_title",
            slideKeyPrefix = "csharp_lesson2_ex1",
            slideCount = 3,
            correctLines = new List<string> { "// Vehicleクラスを定義", "class Vehicle {", "    // Moveメソッドを定義", "    public void Move() {", "        // 画面に出力", "        Console.WriteLine(\"moving\");", "    }", "}", "// :で継承", "class Car : Vehicle { }", "", "// Carインスタンスを作成", "Car c = new Car();", "// Moveメソッドを呼び出し", "c.Move();" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson2_ex1_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson2_ex1_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "csharp_lesson2_ex1_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "csharp_lesson2_ex1_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "csharp_lesson2_ex1_comment5" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "csharp_lesson2_ex1_comment6" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: メソッドのオーバーライド
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex2_title",
            slideKeyPrefix = "csharp_lesson2_ex2",
            slideCount = 3,
            correctLines = new List<string> { "// Shapeクラスを定義", "class Shape {", "    // virtualで仮想メソッド", "    public virtual void Draw() {", "        // 画面に出力", "        Console.WriteLine(\"shape\");", "    }", "}", "// Shapeを継承", "class Circle : Shape {", "    // overrideで親メソッドを上書き", "    public override void Draw() {", "        // 画面に出力", "        Console.WriteLine(\"circle\");", "    }", "}", "", "// Circleインスタンスを作成", "Circle c = new Circle();", "// Drawメソッドを呼び出し", "c.Draw();" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson2_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson2_ex2_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "csharp_lesson2_ex2_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "csharp_lesson2_ex2_comment4" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "csharp_lesson2_ex2_comment5" },
                new LocalizedComment { lineIndex = 12, commentPrefix = "//", localizationKey = "csharp_lesson2_ex2_comment6" },
                new LocalizedComment { lineIndex = 17, commentPrefix = "//", localizationKey = "csharp_lesson2_ex2_comment7" },
                new LocalizedComment { lineIndex = 19, commentPrefix = "//", localizationKey = "csharp_lesson2_ex2_comment8" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: インターフェースを使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex3_title",
            slideKeyPrefix = "csharp_lesson2_ex3",
            slideCount = 3,
            correctLines = new List<string> { "// interfaceでインターフェースを定義", "interface IRunner {", "    // Runメソッドを宣言", "    void Run();", "}", "// IRunnerを実装", "class Robot : IRunner {", "    // Runメソッドを実装", "    public void Run() {", "        // 画面に出力", "        Console.WriteLine(\"running\");", "    }", "}", "", "// Robotインスタンスを作成", "Robot r = new Robot();", "// Runメソッドを呼び出し", "r.Run();" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson2_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson2_ex3_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson2_ex3_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "csharp_lesson2_ex3_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "csharp_lesson2_ex3_comment5" },
                new LocalizedComment { lineIndex = 14, commentPrefix = "//", localizationKey = "csharp_lesson2_ex3_comment6" },
                new LocalizedComment { lineIndex = 16, commentPrefix = "//", localizationKey = "csharp_lesson2_ex3_comment7" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: プロパティを使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex4_title",
            slideKeyPrefix = "csharp_lesson2_ex4",
            slideCount = 3,
            correctLines = new List<string> { "// Itemクラスを定義", "class Item {", "    // getで値を取得", "    public int Price { get; set; }", "}", "", "// Itemインスタンスを作成", "Item item = new Item();", "// Priceに値を設定", "item.Price = 500;", "// Priceを表示", "Console.WriteLine(item.Price);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson2_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson2_ex4_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "csharp_lesson2_ex4_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "csharp_lesson2_ex4_comment4" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "csharp_lesson2_ex4_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: List<T> を使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex5_title",
            slideKeyPrefix = "csharp_lesson2_ex5",
            slideCount = 3,
            correctLines = new List<string> { "// int型のListを作成", "List<int> nums = new List<int>();", "// Addで要素を追加", "nums.Add(10);", "// 2つ目の要素も追加", "nums.Add(20);", "// インデックス1の要素を表示", "Console.WriteLine(nums[1]);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson2_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson2_ex5_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "csharp_lesson2_ex5_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "csharp_lesson2_ex5_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: LINQ - Where で絞り込み
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex6_title",
            slideKeyPrefix = "csharp_lesson2_ex6",
            slideCount = 3,
            correctLines = new List<string> { "// int型のListを作成", "List<int> nums = new List<int> {1, 5, 10, 15, 20};", "// Whereで条件に合う要素を抽出", "var result = nums.Where(n => n >= 10);", "// foreachでループ", "foreach (var n in result) {", "    // 画面に出力", "    Console.WriteLine(n);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson2_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson2_ex6_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "csharp_lesson2_ex6_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "csharp_lesson2_ex6_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: LINQ - Select で変換
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex7_title",
            slideKeyPrefix = "csharp_lesson2_ex7",
            slideCount = 3,
            correctLines = new List<string> { "// int型のListを作成", "List<int> nums = new List<int> {1, 2, 3};", "// Selectで各要素を変換", "var squared = nums.Select(n => n * n);", "// foreachでループ", "foreach (var n in squared) {", "    // 画面に出力", "    Console.WriteLine(n);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson2_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson2_ex7_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "csharp_lesson2_ex7_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "csharp_lesson2_ex7_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: ラムダ式を使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex8_title",
            slideKeyPrefix = "csharp_lesson2_ex8",
            slideCount = 3,
            correctLines = new List<string> { "// =>でラムダ式を定義", "Func<int, int> triple = x => x * 3;", "// tripleを呼び出して表示", "Console.WriteLine(triple(7));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson2_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson2_ex8_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: 例外処理 try-catch
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex9_title",
            slideKeyPrefix = "csharp_lesson2_ex9",
            slideCount = 3,
            correctLines = new List<string> { "// tryで例外処理を開始", "try {", "    // 例外を投げる", "    throw new Exception(\"\");", "// catchで例外を捕捉", "} catch (Exception e) {", "    // 画面に出力", "    Console.WriteLine(\"caught\");", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson2_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson2_ex9_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "csharp_lesson2_ex9_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "csharp_lesson2_ex9_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: null条件演算子 ?.
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson2_ex10_title",
            slideKeyPrefix = "csharp_lesson2_ex10",
            slideCount = 3,
            correctLines = new List<string> { "// 文字列を定義（\"Hello\"）", "string text = \"Hello\" ;", "// ?.でnull安全にアクセス", "int? length = text?.Length;", "// 画面に出力", "Console.WriteLine(length);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson2_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson2_ex10_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "csharp_lesson2_ex10_comment3" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: C# III - 非同期とLINQ応用 ====================
        var lesson3 = new Lesson { titleKey = "csharp_lesson3_title" };

        // Ex1: async/await の基本
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex1_title",
            slideKeyPrefix = "csharp_lesson3_ex1",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "// usingでTasks名前空間をインポート", "using System.Threading.Tasks;", "", "// Programクラスを定義", "class Program {", "    // async Taskでエントリーポイント", "    static async Task Main() {", "        // awaitで非同期処理を待機", "        var result = await GetMessageAsync();", "        // 結果を表示", "        Console.WriteLine(result);", "    }", "    ", "    // 非同期メソッドを定義", "    static async Task<string> GetMessageAsync() {", "        // 遅延を待機", "        await Task.Delay(100);", "        // 結果を返す", "        return \"Hello Async!\";", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson3_ex1_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson3_ex1_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson3_ex1_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "csharp_lesson3_ex1_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "csharp_lesson3_ex1_comment5" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "csharp_lesson3_ex1_comment6" },
                new LocalizedComment { lineIndex = 15, commentPrefix = "//", localizationKey = "csharp_lesson3_ex1_comment7" },
                new LocalizedComment { lineIndex = 17, commentPrefix = "//", localizationKey = "csharp_lesson3_ex1_comment8" },
                new LocalizedComment { lineIndex = 19, commentPrefix = "//", localizationKey = "csharp_lesson3_ex1_comment9" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: LINQ OrderBy
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex2_title",
            slideKeyPrefix = "csharp_lesson3_ex2",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "// usingでLinq名前空間をインポート", "using System.Linq;", "", "// Programクラスを定義", "class Program {", "    // Mainメソッドを定義", "    static void Main() {", "        // 3 を含む配列を定義", "        var nums = new[] { 3, 1, 4, 1, 5 };", "        // OrderByでソート", "        var sorted = nums.OrderBy(x => x);", "        // Joinで \",\" を区切り文字として結果を表示", "        Console.WriteLine(string.Join(\",\", sorted));", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson3_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson3_ex2_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson3_ex2_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "csharp_lesson3_ex2_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "csharp_lesson3_ex2_comment5" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "csharp_lesson3_ex2_comment6" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "csharp_lesson3_ex2_comment7" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: LINQ First と FirstOrDefault
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex3_title",
            slideKeyPrefix = "csharp_lesson3_ex3",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "// usingでLinq名前空間をインポート", "using System.Linq;", "", "// Programクラスを定義", "class Program {", "    // Mainメソッドを定義", "    static void Main() {", "        // 空の配列を定義", "        var nums = new int[] {};", "        // FirstOrDefaultで空の場合はデフォルト値", "        var result = nums.FirstOrDefault();", "        // 結果を表示", "        Console.WriteLine(result);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson3_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson3_ex3_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson3_ex3_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "csharp_lesson3_ex3_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "csharp_lesson3_ex3_comment5" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "csharp_lesson3_ex3_comment6" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "csharp_lesson3_ex3_comment7" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: LINQ Any と All
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex4_title",
            slideKeyPrefix = "csharp_lesson3_ex4",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "// usingでLinq名前空間をインポート", "using System.Linq;", "", "// Programクラスを定義", "class Program {", "    // Mainメソッドを定義", "    static void Main() {", "        // 1 を含む配列を定義", "        var nums = new[] { 1, 2, 3, 4, 5 };", "        // Allで全要素が条件を満たすかチェック", "        var allPositive = nums.All(x => x > 0);", "        // 結果を表示", "        Console.WriteLine(allPositive);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson3_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson3_ex4_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson3_ex4_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "csharp_lesson3_ex4_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "csharp_lesson3_ex4_comment5" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "csharp_lesson3_ex4_comment6" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "csharp_lesson3_ex4_comment7" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: LINQ Sum と Average
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex5_title",
            slideKeyPrefix = "csharp_lesson3_ex5",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "// usingでLinq名前空間をインポート", "using System.Linq;", "", "// Programクラスを定義", "class Program {", "    // Mainメソッドを定義", "    static void Main() {", "        // 10 を含む配列を定義", "        var nums = new[] { 10, 20, 30 };", "        // Sumで合計を計算", "        var total = nums.Sum();", "        // 結果を表示", "        Console.WriteLine(total);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson3_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson3_ex5_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson3_ex5_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "csharp_lesson3_ex5_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "csharp_lesson3_ex5_comment5" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "csharp_lesson3_ex5_comment6" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "csharp_lesson3_ex5_comment7" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: switch式（パターンマッチ）
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex6_title",
            slideKeyPrefix = "csharp_lesson3_ex6",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "", "// Programクラスを定義", "class Program {", "    // Mainメソッドを定義", "    static void Main() {", "        // 変数を定義", "        int n = 2;", "        // switchで式を分岐", "        var result = n switch {", "            // 1の場合", "            1 => \"one\",", "            // 2の場合", "            2 => \"two\",", "            // その他の場合", "            _ => \"other\"", "        };", "        // 結果を表示", "        Console.WriteLine(result);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson3_ex6_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "csharp_lesson3_ex6_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson3_ex6_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "csharp_lesson3_ex6_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "csharp_lesson3_ex6_comment5" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "csharp_lesson3_ex6_comment6" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "csharp_lesson3_ex6_comment7" },
                new LocalizedComment { lineIndex = 15, commentPrefix = "//", localizationKey = "csharp_lesson3_ex6_comment8" },
                new LocalizedComment { lineIndex = 18, commentPrefix = "//", localizationKey = "csharp_lesson3_ex6_comment9" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: Dictionary
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex7_title",
            slideKeyPrefix = "csharp_lesson3_ex7",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "// usingでCollections.Generic名前空間をインポート", "using System.Collections.Generic;", "", "// Programクラスを定義", "class Program {", "    // Mainメソッドを定義", "    static void Main() {", "        // Dictionaryでキーと値の辞書", "        var dict = new Dictionary<string, int>();", "        // キー\"a\"に値を設定", "        dict[\"a\"] = 1;", "        // キー\"b\"に値を設定", "        dict[\"b\"] = 2;", "        // キー\"a\"の値を表示", "        Console.WriteLine(dict[\"a\"]);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson3_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson3_ex7_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson3_ex7_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "csharp_lesson3_ex7_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "csharp_lesson3_ex7_comment5" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "csharp_lesson3_ex7_comment6" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "csharp_lesson3_ex7_comment7" },
                new LocalizedComment { lineIndex = 15, commentPrefix = "//", localizationKey = "csharp_lesson3_ex7_comment8" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: string interpolation
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex8_title",
            slideKeyPrefix = "csharp_lesson3_ex8",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "", "// Programクラスを定義", "class Program {", "    // Mainメソッドを定義", "    static void Main() {", "        // 変数xを定義", "        var x = 10;", "        // 変数yを定義", "        var y = 20;", "        // $で文字列補間", "        Console.WriteLine($\"Sum: {x + y}\");", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson3_ex8_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "csharp_lesson3_ex8_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson3_ex8_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "csharp_lesson3_ex8_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "csharp_lesson3_ex8_comment5" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "csharp_lesson3_ex8_comment6" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: record 型
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex9_title",
            slideKeyPrefix = "csharp_lesson3_ex9",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "", "// recordで値型を定義", "record Point(int X, int Y);", "", "// Programクラスを定義", "class Program {", "    // Mainメソッドを定義", "    static void Main() {", "        // Pointインスタンスを作成", "        var p = new Point(10, 20);", "        // 結果を表示", "        Console.WriteLine(p);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson3_ex9_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "csharp_lesson3_ex9_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "csharp_lesson3_ex9_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "csharp_lesson3_ex9_comment4" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "csharp_lesson3_ex9_comment5" },
                new LocalizedComment { lineIndex = 12, commentPrefix = "//", localizationKey = "csharp_lesson3_ex9_comment6" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: using で自動解放
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson3_ex10_title",
            slideKeyPrefix = "csharp_lesson3_ex10",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "// usingでIO名前空間をインポート", "using System.IO;", "", "// Programクラスを定義", "class Program {", "    // Mainメソッドを定義", "    static void Main() {", "        // usingで自動解放", "        using var writer = new StringWriter();", "        // 文字列を書き込む", "        writer.WriteLine(\"Hello\");", "        // 結果を表示", "        Console.WriteLine(writer.ToString().Trim());", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson3_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson3_ex10_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson3_ex10_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "csharp_lesson3_ex10_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "csharp_lesson3_ex10_comment5" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "csharp_lesson3_ex10_comment6" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "csharp_lesson3_ex10_comment7" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson3);

        // ==================== LESSON 4: C# IV - ジェネリクスとデリゲート ====================
        var lesson4 = new Lesson { titleKey = "csharp_lesson4_title" };

        // Ex1: ジェネリッククラス
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex1_title",
            slideKeyPrefix = "csharp_lesson4_ex1",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "", "// 型パラメータを定義する記号", "class Container<T> {", "    // プロパティを定義", "    public T Item { get; set; }", "}", "", "// Programクラスを定義", "class Program {", "    // Mainメソッドを定義", "    static void Main() {", "        // \"Hello\" を持つ Container インスタンスを作成", "        var c = new Container<string> { Item = \"Hello\" };", "        // 結果を表示", "        Console.WriteLine(c.Item);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson4_ex1_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "csharp_lesson4_ex1_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson4_ex1_comment3" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "csharp_lesson4_ex1_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "csharp_lesson4_ex1_comment5" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "csharp_lesson4_ex1_comment6" },
                new LocalizedComment { lineIndex = 15, commentPrefix = "//", localizationKey = "csharp_lesson4_ex1_comment7" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: ジェネリック制約 where
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex2_title",
            slideKeyPrefix = "csharp_lesson4_ex2",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "", "// 型パラメータに制約を付けるキーワード", "class Comparer<T> where T : IComparable<T> {", "    // Compareメソッドを定義", "    public int Compare(T a, T b) => a.CompareTo(b);", "}", "", "// Programクラスを定義", "class Program {", "    // Mainメソッドを定義", "    static void Main() {", "        // Comparerインスタンスを作成", "        var c = new Comparer<int>();", "        // 比較結果を表示", "        Console.WriteLine(c.Compare(5, 3));", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson4_ex2_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "csharp_lesson4_ex2_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson4_ex2_comment3" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "csharp_lesson4_ex2_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "csharp_lesson4_ex2_comment5" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "csharp_lesson4_ex2_comment6" },
                new LocalizedComment { lineIndex = 15, commentPrefix = "//", localizationKey = "csharp_lesson4_ex2_comment7" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: デリゲートの基本
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex3_title",
            slideKeyPrefix = "csharp_lesson4_ex3",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "", "// Programクラスを定義", "class Program {", "    // メソッドを参照できる型を定義するキーワード", "    delegate int MathOp(int x);", "    ", "    // Doubleメソッドを定義", "    static int Double(int n) => n * 2;", "    ", "    // Mainメソッドを定義", "    static void Main() {", "        // デリゲートにメソッドを代入", "        MathOp op = Double;", "        // 結果を表示", "        Console.WriteLine(op(5));", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson4_ex3_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "csharp_lesson4_ex3_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson4_ex3_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "csharp_lesson4_ex3_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "csharp_lesson4_ex3_comment5" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "csharp_lesson4_ex3_comment6" },
                new LocalizedComment { lineIndex = 15, commentPrefix = "//", localizationKey = "csharp_lesson4_ex3_comment7" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: Func デリゲート
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex4_title",
            slideKeyPrefix = "csharp_lesson4_ex4",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "", "// Programクラスを定義", "class Program {", "    // Mainメソッドを定義", "    static void Main() {", "        // 戻り値を持つ汎用デリゲート型", "        Func<int, int> triple = x => x * 3;", "        // 結果を表示", "        Console.WriteLine(triple(7));", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson4_ex4_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "csharp_lesson4_ex4_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson4_ex4_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "csharp_lesson4_ex4_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "csharp_lesson4_ex4_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: Action デリゲート
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex5_title",
            slideKeyPrefix = "csharp_lesson4_ex5",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "", "// Programクラスを定義", "class Program {", "    // Mainメソッドを定義", "    static void Main() {", "        // 戻り値を持たない汎用デリゲート型", "        Action<string> greet = name => Console.WriteLine($\"Hello, {name}!\");", "        // 関数を呼び出し", "        greet(\"World\");", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson4_ex5_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "csharp_lesson4_ex5_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson4_ex5_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "csharp_lesson4_ex5_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "csharp_lesson4_ex5_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: LINQ GroupBy
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex6_title",
            slideKeyPrefix = "csharp_lesson4_ex6",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "// usingでLinq名前空間をインポート", "using System.Linq;", "", "// Programクラスを定義", "class Program {", "    // Mainメソッドを定義", "    static void Main() {", "        // 配列を定義", "        var nums = new[] { 1, 2, 3, 4, 5, 6 };", "        // キーでグループ分けするLINQメソッド", "        var groups = nums.GroupBy(n => n % 2 == 0 ? \"even\" : \"odd\");", "        // foreachでループ", "        foreach (var g in groups) {", "            // グループを表示", "            Console.WriteLine($\"{g.Key}: {g.Count()}\");", "        }", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson4_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "csharp_lesson4_ex6_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson4_ex6_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "csharp_lesson4_ex6_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "csharp_lesson4_ex6_comment5" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "csharp_lesson4_ex6_comment6" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "csharp_lesson4_ex6_comment7" },
                new LocalizedComment { lineIndex = 15, commentPrefix = "//", localizationKey = "csharp_lesson4_ex6_comment8" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: null条件演算子 ?.
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex7_title",
            slideKeyPrefix = "csharp_lesson4_ex7",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "", "// Programクラスを定義", "class Program {", "    // Mainメソッドを定義", "    static void Main() {", "        // null許容型の変数を定義", "        string? s = null;", "        // nullでなければアクセスする演算子", "        int? len = s?.Length;", "        // null合体演算子でデフォルト値", "        Console.WriteLine(len ?? 0);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson4_ex7_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "csharp_lesson4_ex7_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson4_ex7_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "csharp_lesson4_ex7_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "csharp_lesson4_ex7_comment5" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "csharp_lesson4_ex7_comment6" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: null合体演算子 ??
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex8_title",
            slideKeyPrefix = "csharp_lesson4_ex8",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "", "// Programクラスを定義", "class Program {", "    // Mainメソッドを定義", "    static void Main() {", "        // null許容型の変数を定義", "        string? value = null;", "        // nullなら右側を返す演算子", "        string result = value ?? \"default\";", "        // 結果を表示", "        Console.WriteLine(result);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson4_ex8_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "csharp_lesson4_ex8_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson4_ex8_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "csharp_lesson4_ex8_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "csharp_lesson4_ex8_comment5" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "csharp_lesson4_ex8_comment6" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: record 型
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex9_title",
            slideKeyPrefix = "csharp_lesson4_ex9",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "", "// イミュータブルなデータ型を定義するキーワード", "record Point(int X, int Y);", "", "// Programクラスを定義", "class Program {", "    // Mainメソッドを定義", "    static void Main() {", "        // Pointインスタンスを作成", "        var p = new Point(3, 4);", "        // 結果を表示", "        Console.WriteLine(p);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson4_ex9_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "csharp_lesson4_ex9_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "csharp_lesson4_ex9_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "csharp_lesson4_ex9_comment4" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "csharp_lesson4_ex9_comment5" },
                new LocalizedComment { lineIndex = 12, commentPrefix = "//", localizationKey = "csharp_lesson4_ex9_comment6" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: init プロパティ
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "csharp_lesson4_ex10_title",
            slideKeyPrefix = "csharp_lesson4_ex10",
            slideCount = 3,
            correctLines = new List<string> { "// usingでSystem名前空間をインポート", "using System;", "", "// Itemクラスを定義", "class Item {", "    // 初期化時のみ値を設定できるアクセサ", "    public string Name { get; init; }", "}", "", "// Programクラスを定義", "class Program {", "    // Mainメソッドを定義", "    static void Main() {", "        // \"Apple\" を持つ Item インスタンスを作成", "        var item = new Item { Name = \"Apple\" };", "        // 結果を表示", "        Console.WriteLine(item.Name);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "csharp_lesson4_ex10_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "csharp_lesson4_ex10_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "csharp_lesson4_ex10_comment3" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "csharp_lesson4_ex10_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "csharp_lesson4_ex10_comment5" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "csharp_lesson4_ex10_comment6" },
                new LocalizedComment { lineIndex = 15, commentPrefix = "//", localizationKey = "csharp_lesson4_ex10_comment7" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson4);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;
    }