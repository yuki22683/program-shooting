    private void InitializeKotlinLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: Kotlin (コトリン) に挑戦！ ====================
        var lesson1 = new Lesson { titleKey = "kotlin_lesson1_title" };

        // Ex1: 画面にメッセージを出しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson1_ex1_title",
            slideKeyPrefix = "kotlin_lesson1_ex1",
            slideCount = 5,
            correctLines = new List<string> { "// Hello, World!と出力する関数", "println(\"Hello, World!\")" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson1_ex1_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: 便利な「はこ」変数（へんすう）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson1_ex2_title",
            slideKeyPrefix = "kotlin_lesson1_ex2",
            slideCount = 5,
            correctLines = new List<string> { "// x に 10 を入れる", "val x = 10", "// y に 5 を入れる", "val y = 5", "// + でたし算する", "println(x + y)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson1_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson1_ex2_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "kotlin_lesson1_ex2_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: 剰余演算子（%）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson1_ex3_title",
            slideKeyPrefix = "kotlin_lesson1_ex3",
            slideCount = 4,
            correctLines = new List<string> { "// 10 を 3 で割ったあまりを出力する", "println(10 % 3)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson1_ex3_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: 累算代入演算子（+=、-=）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson1_ex4_title",
            slideKeyPrefix = "kotlin_lesson1_ex4",
            slideCount = 5,
            correctLines = new List<string> { "// var で変更可能な変数を作る", "var hp = 100", "// += で 20 を足す", "hp += 20", "// -= で 50 を引く", "hp -= 50", "println(hp)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson1_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson1_ex4_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "kotlin_lesson1_ex4_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: 「もし〜なら」で分けましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson1_ex5_title",
            slideKeyPrefix = "kotlin_lesson1_ex5",
            slideCount = 5,
            correctLines = new List<string> { "// score に 100 を入れる", "val score = 100", "// > で比較する", "if (score > 80) {", "    println(\"Great!\")", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson1_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson1_ex5_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: 論理演算子（&&、||）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson1_ex6_title",
            slideKeyPrefix = "kotlin_lesson1_ex6",
            slideCount = 5,
            correctLines = new List<string> { "val score = 80", "val bonus = 10", "// && で両方の条件をチェック", "if (score >= 70 && bonus > 0) {", "    println(\"Bonus Pass\")", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson1_ex6_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: たくさんのデータをまとめましょう「リスト」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson1_ex7_title",
            slideKeyPrefix = "kotlin_lesson1_ex7",
            slideCount = 5,
            correctLines = new List<string> { "// colors というリストを作る（'あか', 'あお'の順）", "val colors = listOf(\"あか\", \"あお\")", "// 2番目のデータ（1番）を出す", "println(colors[1])" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson1_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson1_ex7_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: 名前で引き出す「辞書」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson1_ex8_title",
            slideKeyPrefix = "kotlin_lesson1_ex8",
            slideCount = 5,
            correctLines = new List<string> { "// 辞書を作る（キーは'みかん'、値は'オレンジ'）", "val colors = mapOf(\"みかん\" to \"オレンジ\")", "// 中身を出す", "println(colors[\"みかん\"])" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson1_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson1_ex8_comment2" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: Kotlin II - Null安全とラムダ ====================
        var lesson2 = new Lesson { titleKey = "kotlin_lesson2_title" };

        // Ex1: Null安全を学ぼう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson2_ex1_title",
            slideKeyPrefix = "kotlin_lesson2_ex1",
            slideCount = 5,
            correctLines = new List<string> { "// ? で nullable 型にする", "var num: Int? = 42", "// null チェックをする", "if (num != null) {", "    // num を出力する", "    println(num)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex1_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex1_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex1_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: セーフコール演算子 ?.
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson2_ex2_title",
            slideKeyPrefix = "kotlin_lesson2_ex2",
            slideCount = 4,
            correctLines = new List<string> { "// nullable 型の変数を宣言する", "val text: String? = \"Hello\"", "// ?. で安全にプロパティにアクセスする", "println(text?.length)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex2_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: エルビス演算子 ?:
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson2_ex3_title",
            slideKeyPrefix = "kotlin_lesson2_ex3",
            slideCount = 4,
            correctLines = new List<string> { "// nullable 型の変数を宣言する", "val value: Int? = null", "// ?: でデフォルト値を指定する", "println(value ?: 0)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex3_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: データクラスを使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson2_ex4_title",
            slideKeyPrefix = "kotlin_lesson2_ex4",
            slideCount = 4,
            correctLines = new List<string> { "// data でデータクラスを定義する", "data class Point(val x: Int, val y: Int)", "// Point のインスタンスを作成する", "val p = Point(3, 4)", "// p を出力する", "println(p)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex4_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex4_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: ラムダ式を使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson2_ex5_title",
            slideKeyPrefix = "kotlin_lesson2_ex5",
            slideCount = 5,
            correctLines = new List<string> { "// -> で引数と処理を区切る", "val double = { n: Int -> n * 2 }", "// double(5) を出力する", "println(double(5))" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex5_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: map で変換しよう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson2_ex6_title",
            slideKeyPrefix = "kotlin_lesson2_ex6",
            slideCount = 5,
            correctLines = new List<string> { "// listOf でリストを作成する", "val nums = listOf(1, 2, 3)", "// map で各要素を変換する", "val squared = nums.map { it * it }", "// squared を出力する", "println(squared)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex6_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex6_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: filter で絞り込もう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson2_ex7_title",
            slideKeyPrefix = "kotlin_lesson2_ex7",
            slideCount = 5,
            correctLines = new List<string> { "// listOf でリストを作成する", "val nums = listOf(1, 2, 3, 4, 5)", "// filter で条件に合う要素を抽出する", "val result = nums.filter { it >= 3 }", "// result を出力する", "println(result)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex7_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex7_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: when 式を使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson2_ex8_title",
            slideKeyPrefix = "kotlin_lesson2_ex8",
            slideCount = 5,
            correctLines = new List<string> { "// x に 2 を代入する", "val x = 2", "// when で分岐する", "val result = when (x) {", "    // 1 の場合", "    1 -> \"one\"", "    // 2 の場合", "    2 -> \"two\"", "    // その他の場合", "    else -> \"other\"", "}", "// result を出力する", "println(result)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex8_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex8_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex8_comment4" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex8_comment5" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex8_comment6" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: 拡張関数を作ろう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson2_ex9_title",
            slideKeyPrefix = "kotlin_lesson2_ex9",
            slideCount = 5,
            correctLines = new List<string> { "// this でレシーバを参照する", "fun Int.double() = this * 2", "// 5.double() を出力する", "println(5.double())" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex9_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: スコープ関数 let
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson2_ex10_title",
            slideKeyPrefix = "kotlin_lesson2_ex10",
            slideCount = 5,
            correctLines = new List<string> { "// nullable 型の変数を宣言する", "val num: Int? = 42", "// let で処理を実行する", "num?.let { println(it * 2) }" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson2_ex10_comment2" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: Kotlin III - コルーチンとDSL ====================
        var lesson3 = new Lesson { titleKey = "kotlin_lesson3_title" };

        // Ex1: suspend 関数
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson3_ex1_title",
            slideKeyPrefix = "kotlin_lesson3_ex1",
            slideCount = 6,
            correctLines = new List<string> { "// coroutines をインポートする", "import kotlinx.coroutines.*", "", "// suspend で一時停止可能な関数にする", "suspend fun getMessage(): String {", "    // 100ミリ秒待機する", "    delay(100)", "    // \"Hello\" を返す", "    return \"Hello\"", "}", "", "// main 関数を定義する", "fun main() = runBlocking {", "    // getMessage() を出力する", "    println(getMessage())", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex1_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex1_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex1_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex1_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex1_comment5" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex1_comment6" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: sequence（シーケンス）
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson3_ex2_title",
            slideKeyPrefix = "kotlin_lesson3_ex2",
            slideCount = 6,
            correctLines = new List<string> { "// main 関数を定義する", "fun main() {", "    // generateSequence で無限シーケンスを生成する", "    val nums = generateSequence(1) { it * 2 }", "        // 4つ取得する", "        .take(4)", "        // リストに変換する", "        .toList()", "    // nums を出力する", "    println(nums)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex2_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex2_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex2_comment4" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex2_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: inline 関数
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson3_ex3_title",
            slideKeyPrefix = "kotlin_lesson3_ex3",
            slideCount = 6,
            correctLines = new List<string> { "// inline で関数をインライン化する", "inline fun repeat(times: Int, action: (Int) -> Unit) {", "    // 0 から times まで繰り返す", "    for (i in 0 until times) action(i)", "}", "", "// main 関数を定義する", "fun main() {", "    // repeat を呼び出す", "    repeat(3) { println(it) }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex3_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex3_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex3_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: reified 型パラメータ
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson3_ex4_title",
            slideKeyPrefix = "kotlin_lesson3_ex4",
            slideCount = 6,
            correctLines = new List<string> { "// reified で実行時に型情報を使う", "inline fun <reified T> checkType(value: Any): Boolean {", "    // value が T 型かチェックする", "    return value is T", "}", "", "// main 関数を定義する", "fun main() {", "    // String 型かチェックする", "    println(checkType<String>(\"test\"))", "    // Int 型かチェックする", "    println(checkType<Int>(\"test\"))", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex4_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex4_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex4_comment4" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex4_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: apply スコープ関数
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson3_ex5_title",
            slideKeyPrefix = "kotlin_lesson3_ex5",
            slideCount = 6,
            correctLines = new List<string> { "// data class を定義する", "data class Config(var host: String = \"\", var port: Int = 0)", "", "// main 関数を定義する", "fun main() {", "    // apply で設定してオブジェクトを返す", "    val cfg = Config().apply {", "        // hostを設定する", "        host = \"localhost\"", "        // portを設定する", "        port = 8080", "    }", "    // cfg を出力する", "    println(cfg)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex5_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex5_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex5_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex5_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex5_comment5" },
                new LocalizedComment { lineIndex = 12, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex5_comment6" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: also スコープ関数
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson3_ex6_title",
            slideKeyPrefix = "kotlin_lesson3_ex6",
            slideCount = 6,
            correctLines = new List<string> { "// main 関数を定義する", "fun main() {", "    // also で副作用を実行してオブジェクトを返す", "    val num = 42.also {", "        // 値を出力する", "        println(\"Value: $it\")", "    }", "    // num を出力する", "    println(num)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex6_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex6_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex6_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: run スコープ関数
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson3_ex7_title",
            slideKeyPrefix = "kotlin_lesson3_ex7",
            slideCount = 6,
            correctLines = new List<string> { "// main 関数を定義する", "fun main() {", "    // run でブロックを実行して結果を返す", "    val result = \"Hello World\".run {", "        // スペースで分割してサイズを取得する", "        split(\" \").size", "    }", "    // result を出力する", "    println(result)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex7_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex7_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex7_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: takeIf と takeUnless
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson3_ex8_title",
            slideKeyPrefix = "kotlin_lesson3_ex8",
            slideCount = 6,
            correctLines = new List<string> { "// main 関数を定義する", "fun main() {", "    // takeIf で条件を満たせば値を返す", "    val num = 10.takeIf { it > 5 }", "    // num を出力する", "    println(num)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex8_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex8_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: groupBy
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson3_ex9_title",
            slideKeyPrefix = "kotlin_lesson3_ex9",
            slideCount = 6,
            correctLines = new List<string> { "// main 関数を定義する", "fun main() {", "    // listOf でリストを作成する", "    val nums = listOf(1, 2, 3, 4, 5)", "    // groupBy でグループ化する", "    val grouped = nums.groupBy { it % 2 }", "    // grouped を出力する", "    println(grouped)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex9_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex9_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex9_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: fold と reduce
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson3_ex10_title",
            slideKeyPrefix = "kotlin_lesson3_ex10",
            slideCount = 6,
            correctLines = new List<string> { "// main 関数を定義する", "fun main() {", "    // listOf でリスト(1, 2, 3, 4)を作成する", "    val nums = listOf(1, 2, 3, 4)", "    // fold で畳み込む", "    val product = nums.fold(1) { acc, n -> acc * n }", "    // product を出力する", "    println(product)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex10_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex10_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "kotlin_lesson3_ex10_comment4" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson3);

        // ==================== LESSON 4: Kotlin IV - Sealed ClassとDelegation ====================
        var lesson4 = new Lesson { titleKey = "kotlin_lesson4_title" };

        // Ex1: Sealed Class
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson4_ex1_title",
            slideKeyPrefix = "kotlin_lesson4_ex1",
            slideCount = 6,
            correctLines = new List<string> { "// sealed で継承を制限する", "sealed class Shape", "// Circle クラスを定義する", "class Circle(val radius: Double) : Shape()", "// Rectangle クラスを定義する", "class Rectangle(val w: Double, val h: Double) : Shape()", "", "// 面積を計算する関数", "fun area(s: Shape): Double = when (s) {", "    // Circle の場合 (radius * radius)", "    is Circle -> 3.14 * s.radius * s.radius", "    // Rectangle の場合 (w * h)", "    is Rectangle -> s.w * s.h", "}", "", "// main 関数を定義する", "fun main() {", "    // area を出力する", "    println(area(Circle(2.0)))", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex1_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex1_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex1_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex1_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex1_comment5" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex1_comment6" },
                new LocalizedComment { lineIndex = 15, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex1_comment7" },
                new LocalizedComment { lineIndex = 17, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex1_comment8" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: object 宣言
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson4_ex2_title",
            slideKeyPrefix = "kotlin_lesson4_ex2",
            slideCount = 6,
            correctLines = new List<string> { "// object でシングルトンを定義する", "object Counter {", "    // count を初期化する", "    private var count = 0", "    // increment 関数を定義する", "    fun increment() { count++ }", "    // get 関数を定義する", "    fun get() = count", "}", "", "// main 関数を定義する", "fun main() {", "    // 1回目の increment を呼び出す", "    Counter.increment()", "    // 2回目の increment を呼び出す", "    Counter.increment()", "    // get を出力する", "    println(Counter.get())", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex2_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex2_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex2_comment4" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex2_comment5" },
                new LocalizedComment { lineIndex = 12, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex2_comment6" },
                new LocalizedComment { lineIndex = 14, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex2_comment7" },
                new LocalizedComment { lineIndex = 16, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex2_comment8" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: 委譲プロパティ by lazy
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson4_ex3_title",
            slideKeyPrefix = "kotlin_lesson4_ex3",
            slideCount = 6,
            correctLines = new List<string> { "// Config クラスを定義する", "class Config {", "    // lazy で遅延初期化する", "    val value: Int by lazy {", "        // \"Init\" を出力する", "        println(\"Init\")", "        // 42 を返す", "        42", "    }", "}", "", "// main 関数を定義する", "fun main() {", "    // Config のインスタンスを作成する", "    val c = Config()", "    // \"Created\" を出力する", "    println(\"Created\")", "    // c.value を出力する", "    println(c.value)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex3_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex3_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex3_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex3_comment5" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex3_comment6" },
                new LocalizedComment { lineIndex = 15, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex3_comment7" },
                new LocalizedComment { lineIndex = 17, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex3_comment8" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: lateinit 修飾子
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson4_ex4_title",
            slideKeyPrefix = "kotlin_lesson4_ex4",
            slideCount = 6,
            correctLines = new List<string> { "// Service クラスを定義する", "class Service {", "    // lateinit で後から初期化を宣言する", "    lateinit var config: String", "    ", "    // setup 関数を定義する", "    fun setup(c: String) { config = c }", "}", "", "// main 関数を定義する", "fun main() {", "    // Service のインスタンスを作成する", "    val s = Service()", "    // setup を呼び出す", "    s.setup(\"OK\")", "    // s.config を出力する", "    println(s.config)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex4_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex4_comment3" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex4_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex4_comment5" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex4_comment6" },
                new LocalizedComment { lineIndex = 15, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex4_comment7" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: inline 関数
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson4_ex5_title",
            slideKeyPrefix = "kotlin_lesson4_ex5",
            slideCount = 6,
            correctLines = new List<string> { "// inline でインライン展開する", "inline fun repeat(times: Int, action: (Int) -> Unit) {", "    // 0 から times まで繰り返す", "    for (i in 0 until times) {", "        // action を呼び出す", "        action(i)", "    }", "}", "", "// main 関数を定義する", "fun main() {", "    // repeat を呼び出す", "    repeat(3) { println(it) }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex5_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex5_comment3" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex5_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex5_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: reified 型パラメータ
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson4_ex6_title",
            slideKeyPrefix = "kotlin_lesson4_ex6",
            slideCount = 6,
            correctLines = new List<string> { "// reified で型情報を保持する", "inline fun <reified T> typeOf(): String {", "    // 型名を返す", "    return T::class.simpleName ?: \"Unknown\"", "}", "", "// main 関数を定義する", "fun main() {", "    // typeOf<String>() を出力する", "    println(typeOf<String >())", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex6_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex6_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex6_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: 拡張関数
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson4_ex7_title",
            slideKeyPrefix = "kotlin_lesson4_ex7",
            slideCount = 6,
            correctLines = new List<string> { "// isEven() で拡張関数を定義する", "fun Int.isEven() = this % 2 == 0", "", "// main 関数を定義する", "fun main() {", "    // 4.isEven() を出力する", "    println(4.isEven())", "    // 7.isEven() を出力する", "    println(7.isEven())", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex7_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex7_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex7_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex7_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: scope 関数 let
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson4_ex8_title",
            slideKeyPrefix = "kotlin_lesson4_ex8",
            slideCount = 6,
            correctLines = new List<string> { "// main 関数を定義する", "fun main() {", "    // let で変換処理を行う", "    val result = \"hello\".let {", "        // 大文字に変換する", "        it.uppercase()", "    }", "    // result を出力する", "    println(result)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex8_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex8_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex8_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: scope 関数 apply
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson4_ex9_title",
            slideKeyPrefix = "kotlin_lesson4_ex9",
            slideCount = 6,
            correctLines = new List<string> { "// data class を定義する", "data class Config(var host: String = \"\", var port: Int = 0)", "", "// main 関数を定義する", "fun main() {", "    // apply でオブジェクトを設定する", "    val config = Config().apply {", "        // hostに\"localhost\"、portに8080を設定", "        host = \"localhost\"", "        // port を設定する", "        port = 8080", "    }", "    // config を出力する", "    println(\"${config.host}:${config.port}\")", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex9_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex9_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex9_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex9_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex9_comment5" },
                new LocalizedComment { lineIndex = 12, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex9_comment6" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: typealias
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "kotlin_lesson4_ex10_title",
            slideKeyPrefix = "kotlin_lesson4_ex10",
            slideCount = 6,
            correctLines = new List<string> { "// typealias で型に別名をつける", "typealias StringList = List<String>", "", "// printAll 関数を定義する", "fun printAll(items: StringList) {", "    // forEach で各要素を出力する", "    items.forEach { println(it) }", "}", "", "// main 関数を定義する", "fun main() {", "    // printAll を呼び出す", "    printAll(listOf(\"A\", \"B\", \"C\"))", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex10_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex10_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex10_comment3" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex10_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "kotlin_lesson4_ex10_comment5" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson4);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;
    }