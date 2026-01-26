    private void InitializeSwiftLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: Swift (スウィフト) に挑戦！ ====================
        var lesson1 = new Lesson { titleKey = "swift_lesson1_title" };

        // Ex1: 画面にメッセージを出しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson1_ex1_title",
            slideKeyPrefix = "swift_lesson1_ex1",
            slideCount = 5,
            correctLines = new List<string> { "// Hello, World!と出力する関数", "print(\"Hello, World!\")" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson1_ex1_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: 便利な「はこ」変数（へんすう）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson1_ex2_title",
            slideKeyPrefix = "swift_lesson1_ex2",
            slideCount = 5,
            correctLines = new List<string> { "// x に 10 を入れる", "let x = 10", "// y に 5 を入れる", "let y = 5", "// + でたし算する", "print(x + y)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson1_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson1_ex2_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "swift_lesson1_ex2_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: 剰余演算子（%）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson1_ex3_title",
            slideKeyPrefix = "swift_lesson1_ex3",
            slideCount = 4,
            correctLines = new List<string> { "// 10 を 3 で割ったあまりを出力する", "print(10 % 3)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson1_ex3_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: 累算代入演算子（+=、-=）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson1_ex4_title",
            slideKeyPrefix = "swift_lesson1_ex4",
            slideCount = 5,
            correctLines = new List<string> { "// var で変更可能な変数を作る", "var hp = 100", "// += で 20 を足す", "hp += 20", "// -= で 50 を引く", "hp -= 50", "print(hp)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson1_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson1_ex4_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "swift_lesson1_ex4_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: 「もし〜なら」で分けましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson1_ex5_title",
            slideKeyPrefix = "swift_lesson1_ex5",
            slideCount = 5,
            correctLines = new List<string> { "// score に 100 を入れる", "let score = 100", "// > で比較する", "if score > 80 {", "    print(\"Great!\")", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson1_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson1_ex5_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: 論理演算子（&&、||）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson1_ex6_title",
            slideKeyPrefix = "swift_lesson1_ex6",
            slideCount = 5,
            correctLines = new List<string> { "let score = 80", "let bonus = 10", "// && で両方の条件をチェック", "if score >= 70 && bonus > 0 {", "    print(\"Bonus Pass\")", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson1_ex6_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: たくさんのデータをまとめましょう「配列」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson1_ex7_title",
            slideKeyPrefix = "swift_lesson1_ex7",
            slideCount = 5,
            correctLines = new List<string> { "// colors という配列を作る（'あか', 'あお'の順）", "let colors = [\"あか\", \"あお\"]", "// 2番目のデータ（1番）を出す", "print(colors[1])" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson1_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson1_ex7_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: 名前で引き出す「辞書」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson1_ex8_title",
            slideKeyPrefix = "swift_lesson1_ex8",
            slideCount = 5,
            correctLines = new List<string> { "// 辞書を作る（キーは'みかん'、値は'オレンジ'）", "let colors = [\"みかん\": \"オレンジ\"]", "// 中身を出す", "print(colors[\"みかん\"]!)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson1_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson1_ex8_comment2" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: Swift II - オプショナルとプロトコル ====================
        var lesson2 = new Lesson { titleKey = "swift_lesson2_title" };

        // Ex1: オプショナルを学ぼう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson2_ex1_title",
            slideKeyPrefix = "swift_lesson2_ex1",
            slideCount = 5,
            correctLines = new List<string> { "// ?でオプショナル型", "var num: Int? = 42", "// nilチェック", "if num != nil {", "    // !でアンラップ", "    print(num!)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson2_ex1_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson2_ex1_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "swift_lesson2_ex1_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: オプショナルバインディング
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson2_ex2_title",
            slideKeyPrefix = "swift_lesson2_ex2",
            slideCount = 4,
            correctLines = new List<string> { "// ?でオプショナル型", "var val: Int? = 100", "// letで値を取り出す", "if let n = val {", "    // 値を出力", "    print(n)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson2_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson2_ex2_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "swift_lesson2_ex2_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: 構造体を作ろう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson2_ex3_title",
            slideKeyPrefix = "swift_lesson2_ex3",
            slideCount = 5,
            correctLines = new List<string> { "// structで構造体を定義", "struct Rect {", "    // widthプロパティ", "    var width: Int", "    // heightプロパティ", "    var height: Int", "}", "// インスタンスを作成", "let r = Rect(width: 3, height: 4)", "// widthを出力", "print(r.width)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson2_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson2_ex3_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "swift_lesson2_ex3_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "swift_lesson2_ex3_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "swift_lesson2_ex3_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: クラスを作ろう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson2_ex4_title",
            slideKeyPrefix = "swift_lesson2_ex4",
            slideCount = 5,
            correctLines = new List<string> { "// classでクラスを定義", "class Cat {", "    // nameプロパティ", "    var name: String", "    // initでイニシャライザ", "    init(name: String) {", "        // selfで自身のプロパティにアクセス", "        self.name = name", "    }", "}", "// インスタンスを作成", "let cat = Cat(name: \"Tama\")", "// nameを出力", "print(cat.name)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson2_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson2_ex4_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "swift_lesson2_ex4_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "swift_lesson2_ex4_comment4" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "swift_lesson2_ex4_comment5" },
                new LocalizedComment { lineIndex = 12, commentPrefix = "//", localizationKey = "swift_lesson2_ex4_comment6" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: プロトコルを使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson2_ex5_title",
            slideKeyPrefix = "swift_lesson2_ex5",
            slideCount = 5,
            correctLines = new List<string> { "// protocolでプロトコルを定義", "protocol Speaker {", "    // speakメソッドを宣言", "    func speak()", "}", "// Speakerに準拠", "struct Dog: Speaker {", "    // speakメソッドを実装", "    func speak() {", "        // woofと出力", "        print(\"woof\")", "    }", "}", "// インスタンスを作成", "let d = Dog()", "// speakを呼び出し", "d.speak()" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson2_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson2_ex5_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "swift_lesson2_ex5_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "swift_lesson2_ex5_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "swift_lesson2_ex5_comment5" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "swift_lesson2_ex5_comment6" },
                new LocalizedComment { lineIndex = 15, commentPrefix = "//", localizationKey = "swift_lesson2_ex5_comment7" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: クロージャを使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson2_ex6_title",
            slideKeyPrefix = "swift_lesson2_ex6",
            slideCount = 5,
            correctLines = new List<string> { "// inで引数と処理を分ける", "let double = { (n: Int) in", "    // n * 2を返す", "    return n * 2", "}", "// doubleを呼び出し", "print(double(5))" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson2_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson2_ex6_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "swift_lesson2_ex6_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: map で変換しよう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson2_ex7_title",
            slideKeyPrefix = "swift_lesson2_ex7",
            slideCount = 5,
            correctLines = new List<string> { "// 配列を作成", "let nums = [1, 2, 3]", "// mapで各要素を変換", "let squared = nums.map { $0 * $0 }", "// 結果を出力", "print(squared)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson2_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson2_ex7_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "swift_lesson2_ex7_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: filter で絞り込もう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson2_ex8_title",
            slideKeyPrefix = "swift_lesson2_ex8",
            slideCount = 4,
            correctLines = new List<string> { "// 配列を作成", "let nums = [1, 2, 3, 4, 5]", "// filterで条件に合う要素を抽出", "let result = nums.filter { $0 >= 3 }", "// 結果を出力", "print(result)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson2_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson2_ex8_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "swift_lesson2_ex8_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: guard で早期リターン
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson2_ex9_title",
            slideKeyPrefix = "swift_lesson2_ex9",
            slideCount = 5,
            correctLines = new List<string> { "// 関数を定義", "func check(_ val: Int?) {", "    // guardで早期リターン", "    guard let n = val else {", "        // nilと出力", "        print(\"nil\")", "        // 早期リターン", "        return", "    }", "    // nを出力", "    print(n)", "}", "// 関数を呼び出し", "check(10)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson2_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson2_ex9_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "swift_lesson2_ex9_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "swift_lesson2_ex9_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "swift_lesson2_ex9_comment5" },
                new LocalizedComment { lineIndex = 12, commentPrefix = "//", localizationKey = "swift_lesson2_ex9_comment6" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: 列挙型を使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson2_ex10_title",
            slideKeyPrefix = "swift_lesson2_ex10",
            slideCount = 5,
            correctLines = new List<string> { "// enumで列挙型を定義", "enum Color {", "    // caseでケースを定義", "    case red, green, blue", "}", "// Color.redを代入", "let c = Color.red", "// switchでパターンマッチ", "switch c {", "case .red:", "    // 赤色を出力", "    print(\"red\")", "case .green:", "    // 緑色を出力", "    print(\"green\")", "case .blue:", "    // 青色を出力", "    print(\"blue\")", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson2_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson2_ex10_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "swift_lesson2_ex10_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "swift_lesson2_ex10_comment4" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "swift_lesson2_ex10_comment5" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "swift_lesson2_ex10_comment6" },
                new LocalizedComment { lineIndex = 16, commentPrefix = "//", localizationKey = "swift_lesson2_ex10_comment7" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: Swift III - 並行処理とResult ====================
        var lesson3 = new Lesson { titleKey = "swift_lesson3_title" };

        // Ex1: Result型
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson3_ex1_title",
            slideKeyPrefix = "swift_lesson3_ex1",
            slideCount = 5,
            correctLines = new List<string> { "// エラー型を定義", "enum MyError: Error { case negative }", "", "// 関数を定義", "func check(_ n: Int) -> Result<Int, MyError> {", "    // 負の場合は失敗", "    if n < 0 { return .failure(.negative) }", "    // successで成功を返す", "    return .success(n)", "}", "", "// 関数を呼び出し", "let result = check(10)", "// switchでパターンマッチ", "switch result {", "// 成功の場合", "case .success(let v): print(v)", "// 失敗の場合", "case .failure(_): print(\"error\")", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson3_ex1_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "swift_lesson3_ex1_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "swift_lesson3_ex1_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "swift_lesson3_ex1_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "swift_lesson3_ex1_comment5" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "swift_lesson3_ex1_comment6" },
                new LocalizedComment { lineIndex = 15, commentPrefix = "//", localizationKey = "swift_lesson3_ex1_comment7" },
                new LocalizedComment { lineIndex = 17, commentPrefix = "//", localizationKey = "swift_lesson3_ex1_comment8" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: compactMap
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson3_ex2_title",
            slideKeyPrefix = "swift_lesson3_ex2",
            slideCount = 4,
            correctLines = new List<string> { "// 配列を作成", "let nums = [\"1\", \"a\", \"2\", \"b\", \"3\"]", "// compactMapでnilを除外して変換", "let ints = nums.compactMap { Int($0) }", "// 結果を出力", "print(ints)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson3_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson3_ex2_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "swift_lesson3_ex2_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: reduce
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson3_ex3_title",
            slideKeyPrefix = "swift_lesson3_ex3",
            slideCount = 5,
            correctLines = new List<string> { "// 配列を作成", "let nums = [1, 2, 3, 4, 5]", "// reduceで畳み込み", "let product = nums.reduce(1) { $0 * $1 }", "// 結果を出力", "print(product)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson3_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson3_ex3_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "swift_lesson3_ex3_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: lazy
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson3_ex4_title",
            slideKeyPrefix = "swift_lesson3_ex4",
            slideCount = 4,
            correctLines = new List<string> { "// 配列を作成", "let nums = [1, 2, 3, 4, 5]", "// lazyで遅延評価", "let result = nums.lazy.map { $0 * 10 }.first!", "// 結果を出力", "print(result)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson3_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson3_ex4_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "swift_lesson3_ex4_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: where 句
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson3_ex5_title",
            slideKeyPrefix = "swift_lesson3_ex5",
            slideCount = 4,
            correctLines = new List<string> { "// 配列を作成", "let nums = [1, -2, 3, -4, 5]", "// whereで条件を追加", "for n in nums where n > 0 {", "    // nを出力", "    print(n)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson3_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson3_ex5_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "swift_lesson3_ex5_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: defer
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson3_ex6_title",
            slideKeyPrefix = "swift_lesson3_ex6",
            slideCount = 5,
            correctLines = new List<string> { "// 関数を定義", "func test() {", "    // deferでスコープ終了時に実行", "    defer { print(\"end\") }", "    // startと出力", "    print(\"start\")", "}", "// 関数を呼び出し", "test()" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson3_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson3_ex6_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "swift_lesson3_ex6_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "swift_lesson3_ex6_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: associatedtype
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson3_ex7_title",
            slideKeyPrefix = "swift_lesson3_ex7",
            slideCount = 5,
            correctLines = new List<string> { "// プロトコルを定義", "protocol Stack {", "    // associatedtypeで関連型を定義", "    associatedtype Element", "    // pushメソッドを宣言", "    mutating func push(_ item: Element)", "}", "", "// Stackに準拠", "struct IntStack: Stack {", "    // items配列", "    var items: [Int] = []", "    // pushメソッドを実装", "    mutating func push(_ item: Int) {", "        // 要素を追加", "        items.append(item)", "    }", "}", "", "// インスタンスを作成", "var s = IntStack()", "// 要素を追加", "s.push(10)", "// itemsを出力", "print(s.items)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson3_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson3_ex7_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "swift_lesson3_ex7_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "swift_lesson3_ex7_comment4" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "swift_lesson3_ex7_comment5" },
                new LocalizedComment { lineIndex = 12, commentPrefix = "//", localizationKey = "swift_lesson3_ex7_comment6" },
                new LocalizedComment { lineIndex = 14, commentPrefix = "//", localizationKey = "swift_lesson3_ex7_comment7" },
                new LocalizedComment { lineIndex = 19, commentPrefix = "//", localizationKey = "swift_lesson3_ex7_comment8" },
                new LocalizedComment { lineIndex = 21, commentPrefix = "//", localizationKey = "swift_lesson3_ex7_comment9" },
                new LocalizedComment { lineIndex = 23, commentPrefix = "//", localizationKey = "swift_lesson3_ex7_comment10" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: throws と rethrows
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson3_ex8_title",
            slideKeyPrefix = "swift_lesson3_ex8",
            slideCount = 5,
            correctLines = new List<string> { "// エラー型を定義", "enum MyError: Error { case invalid }", "", "// throwsでエラーを投げる可能性を示す", "func check(_ n: Int) throws -> Int {", "    // 負の場合はエラー", "    if n < 0 { throw MyError.invalid }", "    // 値を返す", "    return n", "}", "", "// do-catchでエラー処理", "do {", "    // tryで呼び出し", "    let v = try check(10)", "    // 値を出力", "    print(v)", "} catch {", "    // エラーを出力", "    print(\"error\")", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson3_ex8_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "swift_lesson3_ex8_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "swift_lesson3_ex8_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "swift_lesson3_ex8_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "swift_lesson3_ex8_comment5" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "swift_lesson3_ex8_comment6" },
                new LocalizedComment { lineIndex = 15, commentPrefix = "//", localizationKey = "swift_lesson3_ex8_comment7" },
                new LocalizedComment { lineIndex = 18, commentPrefix = "//", localizationKey = "swift_lesson3_ex8_comment8" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: Set（集合）
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson3_ex9_title",
            slideKeyPrefix = "swift_lesson3_ex9",
            slideCount = 5,
            correctLines = new List<string> { "// Setで重複なしのコレクション", "var s: Set = [1, 2, 2, 3, 3, 3]", "// 要素数を出力", "print(s.count)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson3_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson3_ex9_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: mutating
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "swift_lesson3_ex10_title",
            slideKeyPrefix = "swift_lesson3_ex10",
            slideCount = 5,
            correctLines = new List<string> { "// 構造体を定義", "struct Counter {", "    // countプロパティ", "    var count = 0", "    // mutatingで構造体を変更可能に", "    mutating func increment() {", "        // countを1増やす", "        count += 1", "    }", "}", "", "// インスタンスを作成", "var c = Counter()", "// 1回目のincrementを呼び出し", "c.increment()", "// 2回目のincrementを呼び出し", "c.increment()", "// countを出力", "print(c.count)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "swift_lesson3_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "swift_lesson3_ex10_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "swift_lesson3_ex10_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "swift_lesson3_ex10_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "swift_lesson3_ex10_comment5" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "swift_lesson3_ex10_comment6" },
                new LocalizedComment { lineIndex = 15, commentPrefix = "//", localizationKey = "swift_lesson3_ex10_comment7" },
                new LocalizedComment { lineIndex = 17, commentPrefix = "//", localizationKey = "swift_lesson3_ex10_comment8" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson3);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;
    }