    private void InitializePHPLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: PHP (ピーエイチピー) に挑戦！ ====================
        var lesson1 = new Lesson { titleKey = "php_lesson1_title" };

        // Ex1: 画面に文字を出してみましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "php_lesson1_ex1_title",
            slideKeyPrefix = "php_lesson1_ex1",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "  // 画面にメッセージを出す関数", "  echo \"Hello, PHP!\";", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson1_ex1_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: 便利な「はこ」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "php_lesson1_ex2_title",
            slideKeyPrefix = "php_lesson1_ex2",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "  // x というはこに 10 を入れる", "  $x = 10;", "  // 中身を表示する", "  echo $x;", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson1_ex2_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "php_lesson1_ex2_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: コンピュータで計算しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "php_lesson1_ex3_title",
            slideKeyPrefix = "php_lesson1_ex3",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "  // はこに数字を入れる", "  $a = 5;", "  $b = 3;", "  // たし算した結果を表示する", "  echo $a + $b;", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson1_ex3_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "php_lesson1_ex3_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: 剰余演算子（%）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "php_lesson1_ex4_title",
            slideKeyPrefix = "php_lesson1_ex4",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "  // 10 を 3 で割ったあまりを出力する", "  echo 10 % 3;", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson1_ex4_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: 累算代入演算子（+=、-=）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "php_lesson1_ex5_title",
            slideKeyPrefix = "php_lesson1_ex5",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "  // scoreに50を入れる", "  $score = 50;", "  // 10点プラスする", "  $score += 10;", "  // 結果を表示", "  echo $score;", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson1_ex5_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "php_lesson1_ex5_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "php_lesson1_ex5_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: 文章の中に「はこ」を入れましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "php_lesson1_ex6_title",
            slideKeyPrefix = "php_lesson1_ex6",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "  // ageに20を入れる", "  $age = 20;", "  // 文章の中に中身を表示する", "  echo \"I am $age years old.\";", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson1_ex6_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "php_lesson1_ex6_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: データをならべる「配列」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "php_lesson1_ex7_title",
            slideKeyPrefix = "php_lesson1_ex7",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "  // 配列を作る", "  $fruits = ['りんご', 'バナナ'];", "  // 2番目のデータを表示する", "  echo $fruits[1];", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson1_ex7_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "php_lesson1_ex7_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: 「もし〜なら」で分けましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "php_lesson1_ex8_title",
            slideKeyPrefix = "php_lesson1_ex8",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "  // scoreに100を入れる", "  $score = 100;", "  // >で大きいか比較", "  if ($score > 80) {", "    // メッセージ（'Excellent'）", "    echo \"Excellent\";", "  }", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson1_ex8_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "php_lesson1_ex8_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "php_lesson1_ex8_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: ちがう場合はどうしましょう？
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "php_lesson1_ex9_title",
            slideKeyPrefix = "php_lesson1_ex9",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "  // ageに18を入れる", "  $age = 18;", "  // 20以上かを比較する演算子", "  if ($age >= 20) {", "    // 20歳以上のときのメッセージ（'Adult'）", "    echo \"Adult\";", "  // elseで「そうでなければ」", "  } else {", "    // それ以外のメッセージ（'Minor'）", "    echo \"Minor\";", "  }", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson1_ex9_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "php_lesson1_ex9_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "php_lesson1_ex9_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "php_lesson1_ex9_comment4" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "php_lesson1_ex9_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: 論理演算子（&&、||）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "php_lesson1_ex10_title",
            slideKeyPrefix = "php_lesson1_ex10",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "  // scoreに85を入れる", "  $score = 85;", "  // 80以上 かつ 100以下 ならメッセージを出す", "  if ($score >= 80 && $score <= 100) {", "    // 結果を出力", "    echo \"Pass\";", "  }", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson1_ex10_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "php_lesson1_ex10_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "php_lesson1_ex10_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex11: 中身を全部出してみましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "php_lesson1_ex11_title",
            slideKeyPrefix = "php_lesson1_ex11",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "  $nums = [1, 2, 3];", "  // asで各要素を取り出す", "  foreach ($nums as $n) {", "    echo $n;", "  }", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "php_lesson1_ex11_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex12: 名前で探しましょう「連想配列」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "php_lesson1_ex12_title",
            slideKeyPrefix = "php_lesson1_ex12",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "  $user = ['name' => 'Alice'];", "  // nameでキーを指定してアクセス", "  echo $user['name'];", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "php_lesson1_ex12_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex13: 自分だけの関数を作りましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "php_lesson1_ex13_title",
            slideKeyPrefix = "php_lesson1_ex13",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "  function greet() {", "    echo \"Hello\";", "  }", "  // 関数を実行する", "  greet();", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "php_lesson1_ex13_comment1" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: PHP II - クラスとデータベース ====================
        var lesson2 = new Lesson { titleKey = "php_lesson2_title" };

        // Ex1: クラスを定義しよう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "php_lesson2_ex1_title",
            slideKeyPrefix = "php_lesson2_ex1",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "// classでクラスを定義", "class Cat {", "    // publicでアクセス修飾子", "    public $name;", "}", "// newでインスタンスを作成", "$cat = new Cat();", "// ->でプロパティにアクセス", "$cat->name = 'Tama';", "// echoで出力", "echo $cat->name;", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson2_ex1_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "php_lesson2_ex1_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "php_lesson2_ex1_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "php_lesson2_ex1_comment4" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "php_lesson2_ex1_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: コンストラクタを使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "php_lesson2_ex2_title",
            slideKeyPrefix = "php_lesson2_ex2",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "// classでクラスを定義", "class Counter {", "    // publicでアクセス修飾子", "    public $count;", "    // __constructでコンストラクタを定義", "    public function __construct($c) {", "        // $thisで自分自身を参照", "        $this->count = $c;", "    }", "}", "// newでインスタンスを作成", "$cnt = new Counter(5);", "// echoで出力", "echo $cnt->count;", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson2_ex2_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "php_lesson2_ex2_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "php_lesson2_ex2_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "php_lesson2_ex2_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "php_lesson2_ex2_comment5" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "php_lesson2_ex2_comment6" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: 継承を学ぼう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "php_lesson2_ex3_title",
            slideKeyPrefix = "php_lesson2_ex3",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "// classでクラスを定義", "class Vehicle {", "    // functionで関数を定義", "    public function move() {", "        // echoで出力", "        echo 'moving';", "    }", "}", "// extendsで継承", "class Car extends Vehicle { }", "// newでインスタンスを作成", "$car = new Car();", "// ->でメソッドを呼び出し", "$car->move();", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson2_ex3_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "php_lesson2_ex3_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "php_lesson2_ex3_comment3" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "php_lesson2_ex3_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "php_lesson2_ex3_comment5" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "php_lesson2_ex3_comment6" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: インターフェースを使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "php_lesson2_ex4_title",
            slideKeyPrefix = "php_lesson2_ex4",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "// interfaceでインターフェースを定義", "interface Runner {", "    // functionでメソッドを宣言", "    public function run();", "}", "// implementsでインターフェースを実装", "class Robot implements Runner {", "    // functionでメソッドを実装", "    public function run() {", "        // echoで出力", "        echo 'running';", "    }", "}", "// newでインスタンスを作成", "$r = new Robot();", "// ->でメソッドを呼び出し", "$r->run();", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson2_ex4_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "php_lesson2_ex4_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "php_lesson2_ex4_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "php_lesson2_ex4_comment4" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "php_lesson2_ex4_comment5" },
                new LocalizedComment { lineIndex = 14, commentPrefix = "//", localizationKey = "php_lesson2_ex4_comment6" },
                new LocalizedComment { lineIndex = 16, commentPrefix = "//", localizationKey = "php_lesson2_ex4_comment7" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: 配列の array_map
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "php_lesson2_ex5_title",
            slideKeyPrefix = "php_lesson2_ex5",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "// 配列を定義（1, 2, 3）", "$nums = [1, 2, 3];", "// array_mapで各要素を変換", "$squared = array_map(fn($n) => $n * $n, $nums);", "// print_rで配列を出力", "print_r($squared);", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson2_ex5_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "php_lesson2_ex5_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "php_lesson2_ex5_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: 配列の array_filter
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "php_lesson2_ex6_title",
            slideKeyPrefix = "php_lesson2_ex6",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "// 配列を定義", "$nums = [1, 2, 3, 4, 5];", "// array_filterで条件に合う要素を抽出", "$result = array_filter($nums, fn($n) => $n >= 3);", "// print_rで配列を出力", "print_r($result);", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson2_ex6_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "php_lesson2_ex6_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "php_lesson2_ex6_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: アロー関数を使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "php_lesson2_ex7_title",
            slideKeyPrefix = "php_lesson2_ex7",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "// fnでアロー関数を定義", "$triple = fn($n) => $n * 3;", "// echoで出力", "echo $triple(7);", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson2_ex7_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "php_lesson2_ex7_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: 例外処理 try-catch
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "php_lesson2_ex8_title",
            slideKeyPrefix = "php_lesson2_ex8",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "// tryで例外を発生させる可能性があるコードを囲む", "try {", "    // throwで例外を投げる", "    throw new Exception('oops');", "// catchで例外を捕捉", "} catch (Exception $e) {", "    // echoで出力", "    echo 'caught';", "}", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson2_ex8_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "php_lesson2_ex8_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "php_lesson2_ex8_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "php_lesson2_ex8_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: 名前空間を使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "php_lesson2_ex9_title",
            slideKeyPrefix = "php_lesson2_ex9",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "// namespaceで名前空間を宣言", "namespace App;", "// classでクラスを定義", "class Hello {", "    // functionでメソッドを定義", "    public function say() {", "        // echoで出力", "        echo 'hello';", "    }", "}", "// newでインスタンスを作成", "$h = new Hello();", "// ->でメソッドを呼び出し", "$h->say();", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson2_ex9_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "php_lesson2_ex9_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "php_lesson2_ex9_comment3" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "php_lesson2_ex9_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "php_lesson2_ex9_comment5" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "php_lesson2_ex9_comment6" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: Null合体演算子 ??
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "php_lesson2_ex10_title",
            slideKeyPrefix = "php_lesson2_ex10",
            slideCount = 3,
            correctLines = new List<string> { "<?php", "// nullを代入", "$value = null;", "// ??でnullの場合のデフォルト値を指定", "echo $value ?? 'default';", "?>" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "php_lesson2_ex10_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "php_lesson2_ex10_comment2" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: PHP III - モダンPHPとクロージャ ====================
        var lesson3 = new Lesson { titleKey = "php_lesson3_title" };

        // Ex1: クロージャ（無名関数）
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "php_lesson3_ex1_title",
            slideKeyPrefix = "php_lesson3_ex1",
            slideCount = 3,
            correctLines = new List<string> { "// functionで無名関数を定義", "$doubler = function($x) {", "    // *で乗算", "    return $x * 2;", "};" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "php_lesson3_ex1_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "php_lesson3_ex1_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: アロー関数
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "php_lesson3_ex2_title",
            slideKeyPrefix = "php_lesson3_ex2",
            slideCount = 3,
            correctLines = new List<string> { "// fnでアロー関数、=>で式を記述", "$cube = fn($x) => $x ** 3;" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "php_lesson3_ex2_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: Null合体演算子
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "php_lesson3_ex3_title",
            slideKeyPrefix = "php_lesson3_ex3",
            slideCount = 3,
            correctLines = new List<string> { "// nullを代入", "$name = null;", "// ??でNull合体演算子", "$result = $name ?? 'Guest';" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "php_lesson3_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "php_lesson3_ex3_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: スプレッド演算子
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "php_lesson3_ex4_title",
            slideKeyPrefix = "php_lesson3_ex4",
            slideCount = 3,
            correctLines = new List<string> { "// ...で配列を展開", "$merged = [...[1, 2], ...[3, 4]];" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "php_lesson3_ex4_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: match式
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "php_lesson3_ex5_title",
            slideKeyPrefix = "php_lesson3_ex5",
            slideCount = 3,
            correctLines = new List<string> { "// 変数を定義", "$grade = 'A';", "// matchで式によるパターンマッチ", "$message = match($grade) {", "    // =>で値をマッピング", "    'A' => 'Excellent',", "    // =>で値をマッピング", "    'B' => 'Good',", "    // defaultでデフォルトケース", "    default => 'Try harder'", "};" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "php_lesson3_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "php_lesson3_ex5_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "php_lesson3_ex5_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "php_lesson3_ex5_comment4" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "php_lesson3_ex5_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: 名前付き引数
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "php_lesson3_ex6_title",
            slideKeyPrefix = "php_lesson3_ex6",
            slideCount = 3,
            correctLines = new List<string> { "// functionで関数を定義", "function createUser($name, $age) {", "    // returnで連想配列を返す", "    return ['name' => $name, 'age' => $age];", "}", "// age, nameの順で名前付き引数を指定", "$user = createUser(age: 30, name: 'Alice');" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "php_lesson3_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "php_lesson3_ex6_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "php_lesson3_ex6_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: トレイト
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "php_lesson3_ex7_title",
            slideKeyPrefix = "php_lesson3_ex7",
            slideCount = 3,
            correctLines = new List<string> { "// traitでトレイトを定義", "trait HelloTrait {", "    // functionでメソッドを定義", "    public function sayHello() {", "        // returnで値を返す", "        return 'Hello!';", "    }", "}", "// classでクラスを定義", "class Greeter {", "    // useでトレイトを使用", "    use HelloTrait;", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "php_lesson3_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "php_lesson3_ex7_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "php_lesson3_ex7_comment3" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "php_lesson3_ex7_comment4" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "php_lesson3_ex7_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: ジェネレータ
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "php_lesson3_ex8_title",
            slideKeyPrefix = "php_lesson3_ex8",
            slideCount = 3,
            correctLines = new List<string> { "// functionで関数を定義", "function rangeGen($n) {", "    // forでループ", "    for ($i = 1; $i <= $n; $i++) {", "        // yieldで値を一つずつ返す", "        yield $i;", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "php_lesson3_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "php_lesson3_ex8_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "php_lesson3_ex8_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: 配列の分割代入
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "php_lesson3_ex9_title",
            slideKeyPrefix = "php_lesson3_ex9",
            slideCount = 3,
            correctLines = new List<string> { "// name、age、cityに分割代入", "[$name, $age, $city] = ['Alice', 25, 'Tokyo'];" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "php_lesson3_ex9_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: コンストラクタプロパティ昇格
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "php_lesson3_ex10_title",
            slideKeyPrefix = "php_lesson3_ex10",
            slideCount = 3,
            correctLines = new List<string> { "// classでクラスを定義", "class Person {", "    // __constructでコンストラクタを定義", "    public function __construct(", "        // publicでアクセス修飾子", "        public string $name,", "        // publicでアクセス修飾子", "        public int $age", "    ) {}", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "php_lesson3_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "php_lesson3_ex10_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "php_lesson3_ex10_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "php_lesson3_ex10_comment4" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson3);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;
    }