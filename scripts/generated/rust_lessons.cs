    private void InitializeRustLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: Rust (ラスト) に挑戦！ ====================
        var lesson1 = new Lesson { titleKey = "rust_lesson1_title" };

        // Ex1: 画面にメッセージを出しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson1_ex1_title",
            slideKeyPrefix = "rust_lesson1_ex1",
            slideCount = 3,
            correctLines = new List<string> { "fn main() {", "    // Hello, Rust! と表示する", "    println!(\"Hello, Rust!\");", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "rust_lesson1_ex1_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: 便利な「はこ」変数（へんすう）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson1_ex2_title",
            slideKeyPrefix = "rust_lesson1_ex2",
            slideCount = 3,
            correctLines = new List<string> { "fn main() {", "    // x に 10 を入れる", "    let x = 10;", "    // y に 5 を入れる", "    let y = 5;", "    // + でたし算する", "    println!(\"{}\", x + y);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "rust_lesson1_ex2_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "rust_lesson1_ex2_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "rust_lesson1_ex2_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: 剰余演算子（%）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson1_ex3_title",
            slideKeyPrefix = "rust_lesson1_ex3",
            slideCount = 2,
            correctLines = new List<string> { "fn main() {", "    // 10 を 3 で割ったあまりを出力する", "    println!(\"{}\", 10 % 3);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "rust_lesson1_ex3_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: 累算代入演算子（+=、-=）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson1_ex4_title",
            slideKeyPrefix = "rust_lesson1_ex4",
            slideCount = 2,
            correctLines = new List<string> { "fn main() {", "    // mut で変更可能にする", "    let mut hp = 100;", "    // += で 20 を足す", "    hp += 20;", "    // -= で 50 を引く", "    hp -= 50;", "    println!(\"{}\", hp);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "rust_lesson1_ex4_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "rust_lesson1_ex4_comment2" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "rust_lesson1_ex4_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: 「もし〜なら」で分けましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson1_ex5_title",
            slideKeyPrefix = "rust_lesson1_ex5",
            slideCount = 2,
            correctLines = new List<string> { "fn main() {", "    // score に 100 を入れる", "    let score = 100;", "    // > で比較する", "    if score > 80 {", "        println!(\"Great!\");", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "rust_lesson1_ex5_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "rust_lesson1_ex5_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: 論理演算子（&&、||）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson1_ex6_title",
            slideKeyPrefix = "rust_lesson1_ex6",
            slideCount = 2,
            correctLines = new List<string> { "fn main() {", "    let score = 80;", "    let bonus = 10;", "    // && で両方の条件をチェック", "    if score >= 70 && bonus > 0 {", "        println!(\"Bonus Pass\");", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "rust_lesson1_ex6_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: たくさんのデータをまとめましょう「ベクタ」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson1_ex7_title",
            slideKeyPrefix = "rust_lesson1_ex7",
            slideCount = 3,
            correctLines = new List<string> { "fn main() {", "    // colors というベクタを作る（'あか', 'あお'の順）", "    let colors = vec![\"あか\", \"あお\"];", "    // 2番目のデータ（1番）を出す", "    println!(\"{}\", colors[1]);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "rust_lesson1_ex7_comment1" },
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "rust_lesson1_ex7_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: 名前で引き出す「辞書」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson1_ex8_title",
            slideKeyPrefix = "rust_lesson1_ex8",
            slideCount = 3,
            correctLines = new List<string> { "use std::collections::HashMap;", "fn main() {", "    // 辞書を作る（キーは'みかん'、値は'オレンジ'）", "    let mut colors = HashMap::new();", "    // キーと値を追加", "    colors.insert(\"みかん\", \"オレンジ\");", "    // 中身を出す", "    println!(\"{}\", colors[\"みかん\"]);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "rust_lesson1_ex8_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "rust_lesson1_ex8_comment2" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "rust_lesson1_ex8_comment3" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: Rust II - 所有権とトレイト ====================
        var lesson2 = new Lesson { titleKey = "rust_lesson2_title" };

        // Ex1: 所有権の基本
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson2_ex1_title",
            slideKeyPrefix = "rust_lesson2_ex1",
            slideCount = 3,
            correctLines = new List<string> { "fn main() {", "    let s1 = String::from(\"hello\");", "    // s1 の所有権を移動", "    let s2 = s1;", "    println!(\"{}\", s2);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "rust_lesson2_ex1_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: 参照と借用
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson2_ex2_title",
            slideKeyPrefix = "rust_lesson2_ex2",
            slideCount = 3,
            correctLines = new List<string> { "fn print_len(s: &String) {", "    println!(\"{}\", s.len());", "}", "fn main() {", "    let text = String::from(\"hello\");", "    // & で参照を渡す", "    print_len(&text);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "rust_lesson2_ex2_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: 可変参照
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson2_ex3_title",
            slideKeyPrefix = "rust_lesson2_ex3",
            slideCount = 3,
            correctLines = new List<string> { "fn add_world(s: &mut String) {", "    s.push_str(\" world\");", "}", "fn main() {", "    // mut で可変変数にする", "    let mut text = String::from(\"hello\");", "    add_world(&mut text);", "    println!(\"{}\", text);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "rust_lesson2_ex3_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: 構造体を定義しよう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson2_ex4_title",
            slideKeyPrefix = "rust_lesson2_ex4",
            slideCount = 3,
            correctLines = new List<string> { "// struct で構造体を定義", "struct Rect {", "    width: i32,", "    height: i32,", "}", "fn main() {", "    let r = Rect { width: 3, height: 4 };", "    println!(\"{}\", r.width);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "rust_lesson2_ex4_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: impl でメソッドを追加
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson2_ex5_title",
            slideKeyPrefix = "rust_lesson2_ex5",
            slideCount = 3,
            correctLines = new List<string> { "struct Square {", "    side: i32,", "}", "// impl でメソッドを実装", "impl Square {", "    fn area(&self) -> i32 {", "        self.side * self.side", "    }", "}", "fn main() {", "    let s = Square { side: 5 };", "    println!(\"{}\", s.area());", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "rust_lesson2_ex5_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: enum で状態を表す
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson2_ex6_title",
            slideKeyPrefix = "rust_lesson2_ex6",
            slideCount = 3,
            correctLines = new List<string> { "// enum で列挙型を定義", "enum Direction {", "    Up,", "    Down,", "}", "fn main() {", "    let d = Direction::Up;", "    match d {", "        Direction::Up => println!(\"up\"),", "        Direction::Down => println!(\"down\"),", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "rust_lesson2_ex6_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: Option<T> で null を安全に
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson2_ex7_title",
            slideKeyPrefix = "rust_lesson2_ex7",
            slideCount = 3,
            correctLines = new List<string> { "fn main() {", "    // Some で値があることを示す", "    let val: Option<i32> = Some(42);", "    match val {", "        Some(n) => println!(\"{}\", n),", "        None => println!(\"none\"),", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "rust_lesson2_ex7_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: Result<T, E> でエラー処理
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson2_ex8_title",
            slideKeyPrefix = "rust_lesson2_ex8",
            slideCount = 3,
            correctLines = new List<string> { "fn half(n: i32) -> Result<i32, String> {", "    if n % 2 != 0 {", "        return Err(\"odd\".to_string());", "    }", "    // Ok で成功を返す", "    Ok(n / 2)", "}", "fn main() {", "    match half(10) {", "        Ok(v) => println!(\"{}\", v),", "        Err(e) => println!(\"{}\", e),", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "rust_lesson2_ex8_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: トレイトを定義しよう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson2_ex9_title",
            slideKeyPrefix = "rust_lesson2_ex9",
            slideCount = 3,
            correctLines = new List<string> { "// trait でトレイトを定義", "trait Speak {", "    fn speak(&self);", "}", "struct Dog;", "impl Speak for Dog {", "    fn speak(&self) {", "        println!(\"woof\");", "    }", "}", "fn main() {", "    let d = Dog;", "    d.speak();", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "rust_lesson2_ex9_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: ジェネリクスを使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson2_ex10_title",
            slideKeyPrefix = "rust_lesson2_ex10",
            slideCount = 3,
            correctLines = new List<string> { "// T を型パラメータとして使う", "fn print_val<T: std::fmt::Display>(val: T) {", "    println!(\"{}\", val);", "}", "fn main() {", "    print_val(42);", "    print_val(\"hello\");", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "rust_lesson2_ex10_comment1" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: Rust III - ライフタイムとイテレータ ====================
        var lesson3 = new Lesson { titleKey = "rust_lesson3_title" };

        // Ex1: ライフタイムの基本
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson3_ex1_title",
            slideKeyPrefix = "rust_lesson3_ex1",
            slideCount = 3,
            correctLines = new List<string> { "// 'a でライフタイムを定義", "fn first<'a>(s: &'a str) -> &'a str {", "    &s[..1]", "}", "", "fn main() {", "    let s = String::from(\"Hello\");", "    println!(\"{}\", first(&s));", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "rust_lesson3_ex1_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: クロージャの基本
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson3_ex2_title",
            slideKeyPrefix = "rust_lesson3_ex2",
            slideCount = 3,
            correctLines = new List<string> { "fn main() {", "    // |x| でクロージャの引数を定義", "    let double = |x| x * 2;", "    println!(\"{}\", double(5));", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "rust_lesson3_ex2_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: イテレータの基本
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson3_ex3_title",
            slideKeyPrefix = "rust_lesson3_ex3",
            slideCount = 3,
            correctLines = new List<string> { "fn main() {", "    let nums = vec![1, 2, 3];", "    // iter でイテレータを取得", "    for n in nums.iter() {", "        println!(\"{}\", n);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "rust_lesson3_ex3_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: map でイテレータ変換
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson3_ex4_title",
            slideKeyPrefix = "rust_lesson3_ex4",
            slideCount = 3,
            correctLines = new List<string> { "fn main() {", "    let nums = vec![1, 2, 3];", "    // map で各要素を変換", "    let squared: Vec<_> = nums.iter().map(|x| x * x).collect();", "    println!(\"{:?}\", squared);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "rust_lesson3_ex4_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: filter で絞り込み
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson3_ex5_title",
            slideKeyPrefix = "rust_lesson3_ex5",
            slideCount = 3,
            correctLines = new List<string> { "fn main() {", "    let nums = vec![1, 2, 3, 4, 5];", "    // filter で条件に合う要素を絞り込む", "    let big: Vec<_> = nums.iter().filter(|x| **x > 2).collect();", "    println!(\"{:?}\", big);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "rust_lesson3_ex5_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: fold で畳み込み
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson3_ex6_title",
            slideKeyPrefix = "rust_lesson3_ex6",
            slideCount = 3,
            correctLines = new List<string> { "fn main() {", "    let nums = vec![1, 2, 3, 4];", "    // fold で畳み込み", "    let product = nums.iter().fold(1, |acc, x| acc * x);", "    println!(\"{}\", product);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "rust_lesson3_ex6_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: ? 演算子でエラー伝播
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson3_ex7_title",
            slideKeyPrefix = "rust_lesson3_ex7",
            slideCount = 3,
            correctLines = new List<string> { "fn divide(a: i32, b: i32) -> Result<i32, &'static str> {", "    if b == 0 {", "        Err(\"division by zero\")", "    } else {", "        Ok(a / b)", "    }", "}", "", "fn calc() -> Result<i32, &'static str> {", "    // ? でエラーを伝播", "    let x = divide(10, 2)?;", "    Ok(x * 2)", "}", "", "fn main() {", "    println!(\"{:?}\", calc());", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "rust_lesson3_ex7_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: unwrap_or でデフォルト値
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson3_ex8_title",
            slideKeyPrefix = "rust_lesson3_ex8",
            slideCount = 3,
            correctLines = new List<string> { "fn main() {", "    let x: Option<i32> = None;", "    // unwrap_or でデフォルト値を設定", "    let value = x.unwrap_or(42);", "    println!(\"{}\", value);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "rust_lesson3_ex8_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: Vec のメソッド push
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson3_ex9_title",
            slideKeyPrefix = "rust_lesson3_ex9",
            slideCount = 3,
            correctLines = new List<string> { "fn main() {", "    let mut nums = Vec::new();", "    // push で要素を追加", "    nums.push(10);", "    nums.push(20);", "    println!(\"{:?}\", nums);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "rust_lesson3_ex9_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: String と &str
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson3_ex10_title",
            slideKeyPrefix = "rust_lesson3_ex10",
            slideCount = 3,
            correctLines = new List<string> { "fn main() {", "    let s: &str = \"Hello\";", "    // to_string で String に変換", "    let owned: String = s.to_string();", "    println!(\"{}\", owned);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "rust_lesson3_ex10_comment1" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson3);

        // ==================== LESSON 4: Rust IV - トレイトとスマートポインタ ====================
        var lesson4 = new Lesson { titleKey = "rust_lesson4_title" };

        // Ex1: トレイトの定義
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson4_ex1_title",
            slideKeyPrefix = "rust_lesson4_ex1",
            slideCount = 3,
            correctLines = new List<string> { "// trait でトレイトを定義", "trait Speak {", "    fn speak(&self) -> String;", "}", "", "struct Dog;", "", "impl Speak for Dog {", "    fn speak(&self) -> String {", "        String::from(\"Woof!\")", "    }", "}", "", "fn main() {", "    let dog = Dog;", "    println!(\"{}\", dog.speak());", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "rust_lesson4_ex1_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: derive 属性
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson4_ex2_title",
            slideKeyPrefix = "rust_lesson4_ex2",
            slideCount = 3,
            correctLines = new List<string> { "// derive でトレイトを自動実装", "#[derive(Debug)]", "struct User {", "    name: String,", "    age: u32,", "}", "", "fn main() {", "    let user = User { name: String::from(\"Alice\"), age: 30 };", "    println!(\"{:?}\", user);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "rust_lesson4_ex2_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: Box<T>
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson4_ex3_title",
            slideKeyPrefix = "rust_lesson4_ex3",
            slideCount = 3,
            correctLines = new List<string> { "fn main() {", "    // Box::new でヒープに格納", "    let x = Box::new(42);", "    println!(\"{}\", *x);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "rust_lesson4_ex3_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: Rc<T>
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson4_ex4_title",
            slideKeyPrefix = "rust_lesson4_ex4",
            slideCount = 3,
            correctLines = new List<string> { "use std::rc::Rc;", "", "fn main() {", "    let a = Rc::new(String::from(\"Hello\"));", "    // clone で参照カウントを増やす", "    let b = Rc::clone(&a);", "    println!(\"{} {}\", a, b);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "rust_lesson4_ex4_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: HashMap
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson4_ex5_title",
            slideKeyPrefix = "rust_lesson4_ex5",
            slideCount = 3,
            correctLines = new List<string> { "use std::collections::HashMap;", "", "fn main() {", "    let mut map = HashMap::new();", "    // insert でキーと値を追加", "    map.insert(\"a\", 1);", "    map.insert(\"b\", 2);", "    println!(\"{:?}\", map.get(\"a\"));", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "rust_lesson4_ex5_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: HashSet
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson4_ex6_title",
            slideKeyPrefix = "rust_lesson4_ex6",
            slideCount = 3,
            correctLines = new List<string> { "// HashSet をインポート", "use std::collections::HashSet;", "", "fn main() {", "    let mut set = HashSet::new();", "    set.insert(1);", "    set.insert(2);", "    set.insert(1);", "    println!(\"{}\", set.len());", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "rust_lesson4_ex6_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: match ガード
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson4_ex7_title",
            slideKeyPrefix = "rust_lesson4_ex7",
            slideCount = 3,
            correctLines = new List<string> { "fn main() {", "    let num = 7;", "    match num {", "        // if でマッチガードを追加", "        n if n % 2 == 0 => println!(\"even\"),", "        _ => println!(\"odd\"),", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "rust_lesson4_ex7_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: impl Trait
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson4_ex8_title",
            slideKeyPrefix = "rust_lesson4_ex8",
            slideCount = 3,
            correctLines = new List<string> { "// impl でトレイトを実装する型を返す", "fn doubles(n: i32) -> impl Iterator<Item = i32> {", "    (0..n).map(|x| x * 2)", "}", "", "fn main() {", "    for x in doubles(3) {", "        println!(\"{}\", x);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "rust_lesson4_ex8_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: collect で変換
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson4_ex9_title",
            slideKeyPrefix = "rust_lesson4_ex9",
            slideCount = 3,
            correctLines = new List<string> { "fn main() {", "    // collect で Vec に変換", "    let nums: Vec<i32> = (1..=5).collect();", "    println!(\"{:?}\", nums);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 1, commentPrefix = "//", localizationKey = "rust_lesson4_ex9_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: enumerate でインデックス付き
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "rust_lesson4_ex10_title",
            slideKeyPrefix = "rust_lesson4_ex10",
            slideCount = 3,
            correctLines = new List<string> { "fn main() {", "    let items = vec![\"a\", \"b\", \"c\"];", "    // 1番目の i にインデックス、2番目の item に要素が入る", "    for (i, item) in items.iter().enumerate() {", "        println!(\"{}: {}\", i, item);", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "rust_lesson4_ex10_comment1" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson4);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;
    }