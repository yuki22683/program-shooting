    private void InitializeRubyLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: Ruby (ルビー) に挑戦！ ====================
        var lesson1 = new Lesson { titleKey = "ruby_lesson1_title" };

        // Ex1: 画面に文字を出してみましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson1_ex1_title",
            slideKeyPrefix = "ruby_lesson1_ex1",
            slideCount = 3,
            correctLines = new List<string> { "# 「Hello, Ruby!」と出力する関数", "puts 'Hello, Ruby!'" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson1_ex1_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: 便利な「はこ」変数（へんすう）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson1_ex2_title",
            slideKeyPrefix = "ruby_lesson1_ex2",
            slideCount = 3,
            correctLines = new List<string> { "# nameというはこに 'Ruby' を入れる", "name = 'Ruby'", "# はこの中身を画面に出す", "puts name" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson1_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson1_ex2_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: コンピュータで計算しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson1_ex3_title",
            slideKeyPrefix = "ruby_lesson1_ex3",
            slideCount = 3,
            correctLines = new List<string> { "# xというはこに 10 を入れる", "x = 10", "# yというはこに 5 を入れる", "y = 5", "# x と y をたした答えを出す", "puts x + y" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson1_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson1_ex3_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "ruby_lesson1_ex3_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: 剰余演算子（%）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson1_ex4_title",
            slideKeyPrefix = "ruby_lesson1_ex4",
            slideCount = 3,
            correctLines = new List<string> { "# 10 を 3 で割ったあまりを出力する", "puts 10 % 3" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson1_ex4_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: 累算代入演算子（+=、-=）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson1_ex5_title",
            slideKeyPrefix = "ruby_lesson1_ex5",
            slideCount = 3,
            correctLines = new List<string> { "# scoreに50を入れる", "score = 50", "# 10点プラスする", "score += 10", "# 結果を表示", "puts score" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson1_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson1_ex5_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "ruby_lesson1_ex5_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: 文章の中に変数を入れましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson1_ex6_title",
            slideKeyPrefix = "ruby_lesson1_ex6",
            slideCount = 3,
            correctLines = new List<string> { "# ageというはこに 10 を入れる", "age = 10", "# 式展開を使ってメッセージを出す", "puts \"私は#{age}歳です\"" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson1_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson1_ex6_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: たくさんのデータをまとめましょう「配列」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson1_ex7_title",
            slideKeyPrefix = "ruby_lesson1_ex7",
            slideCount = 3,
            correctLines = new List<string> { "# colorsという配列を作る", "colors = ['赤', '青', '緑']", "# 2番目のデータ（インデックス1）を出す", "puts colors[1]" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson1_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson1_ex7_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: 「もし〜なら」で分ける if文
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson1_ex8_title",
            slideKeyPrefix = "ruby_lesson1_ex8",
            slideCount = 3,
            correctLines = new List<string> { "# scoreに100を入れる", "score = 100", "# もし80より大きかったら", "if score > 80", "  # メッセージを表示する", "  puts '合格！'", "end" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson1_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson1_ex8_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "ruby_lesson1_ex8_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: ちがう場合は？ if-else文
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson1_ex9_title",
            slideKeyPrefix = "ruby_lesson1_ex9",
            slideCount = 3,
            correctLines = new List<string> { "# ageに10を入れる", "age = 10", "# 20歳以上かどうかで分ける", "if age >= 20", "  # 大人と表示", "  puts '大人'", "# else でそれ以外の場合", "else", "  # それ以外の場合", "  puts '子供'", "end" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson1_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson1_ex9_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "ruby_lesson1_ex9_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "#", localizationKey = "ruby_lesson1_ex9_comment4" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "#", localizationKey = "ruby_lesson1_ex9_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: 論理演算子（and, or）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson1_ex10_title",
            slideKeyPrefix = "ruby_lesson1_ex10",
            slideCount = 3,
            correctLines = new List<string> { "# scoreに85を入れる", "score = 85", "# 80以上 かつ 100以下 ならメッセージを出す", "if score >= 80 and score <= 100", "  # 結果を表示", "  puts '合格！'", "end" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson1_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson1_ex10_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "ruby_lesson1_ex10_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex11: ぐるぐる回す each
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson1_ex11_title",
            slideKeyPrefix = "ruby_lesson1_ex11",
            slideCount = 3,
            correctLines = new List<string> { "# 名前の配列を作る", "names = ['太郎', '花子']", "# 順番に取り出すループ", "names.each do |name|", "  # 取り出した名前を表示", "  puts name", "end" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson1_ex11_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson1_ex11_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "ruby_lesson1_ex11_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex12: 名前で探しましょう「ハッシュ」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson1_ex12_title",
            slideKeyPrefix = "ruby_lesson1_ex12",
            slideCount = 3,
            correctLines = new List<string> { "# ハッシュを作る", "fruits = {'みかん' => 'オレンジ'}", "# キーを指定して値を取り出す", "puts fruits['みかん']" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson1_ex12_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson1_ex12_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex13: 自分だけの関数を作ろう「メソッド」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson1_ex13_title",
            slideKeyPrefix = "ruby_lesson1_ex13",
            slideCount = 3,
            correctLines = new List<string> { "# greetというメソッドを定義", "def greet", "  # こんにちは と表示", "  puts 'こんにちは'", "end", "# メソッドを呼び出す", "greet" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson1_ex13_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson1_ex13_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "#", localizationKey = "ruby_lesson1_ex13_comment3" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: Ruby II - ブロックとオブジェクト指向 ====================
        var lesson2 = new Lesson { titleKey = "ruby_lesson2_title" };

        // Ex1: ブロックを使おう - each
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson2_ex1_title",
            slideKeyPrefix = "ruby_lesson2_ex1",
            slideCount = 3,
            correctLines = new List<string> { "# numsに配列を代入（1, 2, 3）", "nums = [1, 2, 3]", "# eachで各要素を処理", "nums.each do |n|", "  # putsで出力", "  puts n", "# endで終了", "end" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson2_ex1_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson2_ex1_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "ruby_lesson2_ex1_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "#", localizationKey = "ruby_lesson2_ex1_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: map で変換しよう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson2_ex2_title",
            slideKeyPrefix = "ruby_lesson2_ex2",
            slideCount = 3,
            correctLines = new List<string> { "# numsに配列を代入（1, 2, 3）", "nums = [1, 2, 3]", "# mapで各要素を変換", "doubled = nums.map { |n| n * 2 }", "# putsで出力", "puts doubled" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson2_ex2_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson2_ex2_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "ruby_lesson2_ex2_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: select で絞り込もう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson2_ex3_title",
            slideKeyPrefix = "ruby_lesson2_ex3",
            slideCount = 3,
            correctLines = new List<string> { "# numsに配列を代入（1, 2, 3, 4, 5）", "nums = [1, 2, 3, 4, 5]", "# selectで条件に合う要素を抽出", "big = nums.select { |n| n >= 3 }", "# putsで出力", "puts big" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson2_ex3_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson2_ex3_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "ruby_lesson2_ex3_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: シンボルを使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson2_ex4_title",
            slideKeyPrefix = "ruby_lesson2_ex4",
            slideCount = 3,
            correctLines = new List<string> { "# itemにハッシュを代入", "item = { name: 'Apple', price: 100 }", "# :でシンボルを指定してアクセス", "puts item[:price]" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson2_ex4_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson2_ex4_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: クラスを定義しよう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson2_ex5_title",
            slideKeyPrefix = "ruby_lesson2_ex5",
            slideCount = 3,
            correctLines = new List<string> { "# classでクラスを定義", "class Cat", "  # initializeを定義", "  def initialize(name)", "    # @nameに代入", "    @name = name", "  # endで終了", "  end", "  # greetを定義", "  def greet", "    # @nameを出力", "    puts @name", "  # endで終了", "  end", "# endで終了", "end", "# catにインスタンスを代入", "cat = Cat.new('Tama')", "# greetを呼び出し", "cat.greet" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson2_ex5_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson2_ex5_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "ruby_lesson2_ex5_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "#", localizationKey = "ruby_lesson2_ex5_comment4" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "#", localizationKey = "ruby_lesson2_ex5_comment5" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "#", localizationKey = "ruby_lesson2_ex5_comment6" },
                new LocalizedComment { lineIndex = 12, commentPrefix = "#", localizationKey = "ruby_lesson2_ex5_comment7" },
                new LocalizedComment { lineIndex = 14, commentPrefix = "#", localizationKey = "ruby_lesson2_ex5_comment8" },
                new LocalizedComment { lineIndex = 16, commentPrefix = "#", localizationKey = "ruby_lesson2_ex5_comment9" },
                new LocalizedComment { lineIndex = 18, commentPrefix = "#", localizationKey = "ruby_lesson2_ex5_comment10" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: アクセサを使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson2_ex6_title",
            slideKeyPrefix = "ruby_lesson2_ex6",
            slideCount = 3,
            correctLines = new List<string> { "# classでクラスを定義", "class Item", "  # attr_accessorで読み書き可能に", "  attr_accessor :price", "# endで終了", "end", "# itemにインスタンスを代入", "item = Item.new", "# priceに値を代入", "item.price = 200", "# priceを出力", "puts item.price" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson2_ex6_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson2_ex6_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "ruby_lesson2_ex6_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "#", localizationKey = "ruby_lesson2_ex6_comment4" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "#", localizationKey = "ruby_lesson2_ex6_comment5" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "#", localizationKey = "ruby_lesson2_ex6_comment6" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: 継承を学ぼう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson2_ex7_title",
            slideKeyPrefix = "ruby_lesson2_ex7",
            slideCount = 3,
            correctLines = new List<string> { "# classでクラスを定義", "class Vehicle", "  # moveを定義", "  def move", "    # movingを出力", "    puts 'moving'", "  # endで終了", "  end", "# endで終了", "end", "# <で親クラスを継承", "class Car < Vehicle", "# endで終了", "end", "# carにインスタンスを代入", "car = Car.new", "# moveを呼び出し", "car.move" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson2_ex7_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson2_ex7_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "ruby_lesson2_ex7_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "#", localizationKey = "ruby_lesson2_ex7_comment4" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "#", localizationKey = "ruby_lesson2_ex7_comment5" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "#", localizationKey = "ruby_lesson2_ex7_comment6" },
                new LocalizedComment { lineIndex = 12, commentPrefix = "#", localizationKey = "ruby_lesson2_ex7_comment7" },
                new LocalizedComment { lineIndex = 14, commentPrefix = "#", localizationKey = "ruby_lesson2_ex7_comment8" },
                new LocalizedComment { lineIndex = 16, commentPrefix = "#", localizationKey = "ruby_lesson2_ex7_comment9" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: モジュールで機能を追加
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson2_ex8_title",
            slideKeyPrefix = "ruby_lesson2_ex8",
            slideCount = 3,
            correctLines = new List<string> { "# moduleでモジュールを定義", "module Speakable", "  # speakを定義", "  def speak", "    # speakingを出力", "    puts 'speaking'", "  # endで終了", "  end", "# endで終了", "end", "# classでクラスを定義", "class Robot", "  # includeでモジュールを取り込み", "  include Speakable", "# endで終了", "end", "# robotにインスタンスを代入", "robot = Robot.new", "# speakを呼び出し", "robot.speak" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson2_ex8_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson2_ex8_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "ruby_lesson2_ex8_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "#", localizationKey = "ruby_lesson2_ex8_comment4" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "#", localizationKey = "ruby_lesson2_ex8_comment5" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "#", localizationKey = "ruby_lesson2_ex8_comment6" },
                new LocalizedComment { lineIndex = 12, commentPrefix = "#", localizationKey = "ruby_lesson2_ex8_comment7" },
                new LocalizedComment { lineIndex = 14, commentPrefix = "#", localizationKey = "ruby_lesson2_ex8_comment8" },
                new LocalizedComment { lineIndex = 16, commentPrefix = "#", localizationKey = "ruby_lesson2_ex8_comment9" },
                new LocalizedComment { lineIndex = 18, commentPrefix = "#", localizationKey = "ruby_lesson2_ex8_comment10" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: 例外処理 begin-rescue
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson2_ex9_title",
            slideKeyPrefix = "ruby_lesson2_ex9",
            slideCount = 3,
            correctLines = new List<string> { "# beginで開始", "begin", "  # raiseでエラーを発生", "  raise 'oops'", "# rescueで例外を捕捉", "rescue => e", "  # caughtを出力", "  puts 'caught'", "# endで終了", "end" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson2_ex9_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson2_ex9_comment2" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "#", localizationKey = "ruby_lesson2_ex9_comment3" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "#", localizationKey = "ruby_lesson2_ex9_comment4" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "#", localizationKey = "ruby_lesson2_ex9_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: ラムダを使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson2_ex10_title",
            slideKeyPrefix = "ruby_lesson2_ex10",
            slideCount = 3,
            correctLines = new List<string> { "# ->でラムダを定義", "double = ->(n) { n * 2 }", "# callで実行", "puts double.call(5)" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "#", localizationKey = "ruby_lesson2_ex10_comment1" },
                new LocalizedComment { lineIndex = 2, commentPrefix = "#", localizationKey = "ruby_lesson2_ex10_comment2" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: Ruby III - メタプログラミングと関数型スタイル ====================
        var lesson3 = new Lesson { titleKey = "ruby_lesson3_title" };

        // Ex1: yieldとブロック
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson3_ex1_title",
            slideKeyPrefix = "ruby_lesson3_ex1",
            slideCount = 3,
            correctLines = new List<string>(),
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex2: Procオブジェクト
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson3_ex2_title",
            slideKeyPrefix = "ruby_lesson3_ex2",
            slideCount = 3,
            correctLines = new List<string>(),
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex3: ラムダ
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson3_ex3_title",
            slideKeyPrefix = "ruby_lesson3_ex3",
            slideCount = 3,
            correctLines = new List<string>(),
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex4: シンボルとProc変換
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson3_ex4_title",
            slideKeyPrefix = "ruby_lesson3_ex4",
            slideCount = 3,
            correctLines = new List<string>(),
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex5: reduceメソッド
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson3_ex5_title",
            slideKeyPrefix = "ruby_lesson3_ex5",
            slideCount = 3,
            correctLines = new List<string>(),
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex6: selectとreject
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson3_ex6_title",
            slideKeyPrefix = "ruby_lesson3_ex6",
            slideCount = 3,
            correctLines = new List<string>(),
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex7: スプラット演算子
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson3_ex7_title",
            slideKeyPrefix = "ruby_lesson3_ex7",
            slideCount = 3,
            correctLines = new List<string>(),
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex8: method_missing
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson3_ex8_title",
            slideKeyPrefix = "ruby_lesson3_ex8",
            slideCount = 3,
            correctLines = new List<string>(),
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex9: Struct
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson3_ex9_title",
            slideKeyPrefix = "ruby_lesson3_ex9",
            slideCount = 3,
            correctLines = new List<string>(),
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        // Ex10: tapメソッド
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "ruby_lesson3_ex10_title",
            slideKeyPrefix = "ruby_lesson3_ex10",
            slideCount = 3,
            correctLines = new List<string>(),
            comments = new List<LocalizedComment>(),
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson3);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;
    }