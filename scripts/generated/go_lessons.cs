    private void InitializeGoLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: Go (ゴー) 言語に挑戦！ ====================
        var lesson1 = new Lesson { titleKey = "go_lesson1_title" };

        // Ex1: 画面にメッセージを出しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex1_title",
            slideKeyPrefix = "go_lesson1_ex1",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "func main() {", "    // Hello, Go! と表示する", "    fmt.Println(\"Hello, Go!\")", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "go_lesson1_ex1_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: 便利な「はこ」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex2_title",
            slideKeyPrefix = "go_lesson1_ex2",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "func main() {", "    // x に 10 を代入する", "    x := 10", "    // x を表示する", "    fmt.Println(x)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "go_lesson1_ex2_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "go_lesson1_ex2_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: コンピュータで計算しましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex3_title",
            slideKeyPrefix = "go_lesson1_ex3",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "func main() {", "    // はこに数字を入れる", "    a := 5", "    b := 3", "    // + でたし算する", "    fmt.Println(a + b)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "go_lesson1_ex3_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "go_lesson1_ex3_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: 剰余演算子（%）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex4_title",
            slideKeyPrefix = "go_lesson1_ex4",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "func main() {", "    // 10 を 3 で割ったあまりを出力する", "    fmt.Println(10 % 3)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "go_lesson1_ex4_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: 累算代入演算子（+=、-=）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex5_title",
            slideKeyPrefix = "go_lesson1_ex5",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "func main() {", "    hp := 100", "    // += で 20 を足す", "    hp += 20", "    // -= で 50 を引く", "    hp -= 50", "    fmt.Println(hp)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson1_ex5_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "go_lesson1_ex5_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: 文章の中に「はこ」を入れましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex6_title",
            slideKeyPrefix = "go_lesson1_ex6",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "func main() {", "    // ageに10を入れる", "    age := 10", "    // age を埋め込む", "    fmt.Printf(\"I am %d years old.\\n\", age)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "go_lesson1_ex6_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "go_lesson1_ex6_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: データをならべる「スライス」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex7_title",
            slideKeyPrefix = "go_lesson1_ex7",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "func main() {", "    nums := []int{10, 20}", "    // インデックス 1 で2番目を取得", "    fmt.Println(nums[1])", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson1_ex7_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: 「もし〜なら」で分けましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex8_title",
            slideKeyPrefix = "go_lesson1_ex8",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "func main() {", "    // scoreに100を入れる", "    score := 100", "    // > で比較する", "    if score > 80 {", "        // Great と表示する", "        fmt.Println(\"Great\")", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "go_lesson1_ex8_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "go_lesson1_ex8_comment2" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "go_lesson1_ex8_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: ちがう場合はどうしましょう？
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex9_title",
            slideKeyPrefix = "go_lesson1_ex9",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "func main() {", "    // xに5を入れる", "    x := 5", "    // 10より大きいかを比較する演算子", "    if x > 10 {", "        // 10より大きいときのメッセージ（'Big'）", "        fmt.Println(\"Big\")", "    // else で「そうでなければ」", "    } else {", "        // それ以外のメッセージ（'Small'）", "        fmt.Println(\"Small\")", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "go_lesson1_ex9_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "go_lesson1_ex9_comment2" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "go_lesson1_ex9_comment3" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "go_lesson1_ex9_comment4" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "go_lesson1_ex9_comment5" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: 論理演算子（&&、||）
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex10_title",
            slideKeyPrefix = "go_lesson1_ex10",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "func main() {", "    score := 80", "    bonus := 10", "    // && で両方の条件をチェック", "    if score >= 70 && bonus > 0 {", "        fmt.Println(\"Bonus Pass\")", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "go_lesson1_ex10_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex11: 中身を全部出してみましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex11_title",
            slideKeyPrefix = "go_lesson1_ex11",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "func main() {", "    nums := []int{1, 2, 3}", "    // range で全要素をループ", "    for _, n := range nums {", "        fmt.Println(n)", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson1_ex11_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex12: 名前で探しましょう「じしょ」
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex12_title",
            slideKeyPrefix = "go_lesson1_ex12",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "func main() {", "    // string をキーの型に指定", "    scores := map[string]int{\"Math\": 90}", "    // 'Math' をキーに指定", "    fmt.Println(scores[\"Math\"])", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "go_lesson1_ex12_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "go_lesson1_ex12_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex13: 自分だけの関数を作りましょう
        lesson1.exercises.Add(new Exercise
        {
            titleKey = "go_lesson1_ex13_title",
            slideKeyPrefix = "go_lesson1_ex13",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "func greet() {", "    fmt.Println(\"Hello\")", "}", "func main() {", "    // greet 関数を呼び出す", "    greet()", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "go_lesson1_ex13_comment1" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: Go II - 構造体と並行処理 ====================
        var lesson2 = new Lesson { titleKey = "go_lesson2_title" };

        // Ex1: 複数の値を返す関数
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex1_title",
            slideKeyPrefix = "go_lesson2_ex1",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "func minmax(a, b int) (int, int) {", "    if a < b {", "        // return で複数の値を返す", "        return a, b", "    }", "    // return で2つの値を返す", "    return b, a", "}", "func main() {", "    // 関数から2つの戻り値を受け取る", "    min, max := minmax(5, 3)", "    fmt.Println(min, max)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson2_ex1_comment1" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "go_lesson2_ex1_comment2" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "go_lesson2_ex1_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: エラー処理の基本
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex2_title",
            slideKeyPrefix = "go_lesson2_ex2",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import (", "    \"errors\"", "    \"fmt\"", ")", "func check(n int) (int, error) {", "    if n < 0 {", "        // errors.New でエラーを作成", "        return 0, errors.New(\"negative\")", "    }", "    // nil でエラーなしを表す", "    return n, nil", "}", "func main() {", "    // 関数から2つの戻り値を受け取る", "    val, err := check(5)", "    if err != nil {", "        fmt.Println(err)", "    } else {", "        fmt.Println(val)", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "go_lesson2_ex2_comment1" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "go_lesson2_ex2_comment2" },
                new LocalizedComment { lineIndex = 14, commentPrefix = "//", localizationKey = "go_lesson2_ex2_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: ポインタを使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex3_title",
            slideKeyPrefix = "go_lesson2_ex3",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "func main() {", "    x := 5", "    // & でアドレスを取得", "    p := &x", "    // *p でポインタの値を変更", "    *p = 10", "    fmt.Println(x)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson2_ex3_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "go_lesson2_ex3_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: 構造体を定義しよう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex4_title",
            slideKeyPrefix = "go_lesson2_ex4",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "// struct で構造体を定義", "type Point struct {", "    // フィールド名と型を定義", "    X int", "    Y int", "}", "func main() {", "    // 構造体を初期化", "    p := Point{X: 3, Y: 4}", "    fmt.Println(p.X)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson2_ex4_comment1" },
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson2_ex4_comment2" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "go_lesson2_ex4_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: メソッドを作ろう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex5_title",
            slideKeyPrefix = "go_lesson2_ex5",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "type Rect struct {", "    W int", "    H int", "}", "func (r Rect) Area() int {", "    // * でかけ算", "    return r.W * r.H", "}", "func main() {", "    // 構造体を初期化", "    rect := Rect{W: 3, H: 4}", "    // メソッドを呼び出す", "    fmt.Println(rect.Area())", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "go_lesson2_ex5_comment1" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "go_lesson2_ex5_comment2" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "go_lesson2_ex5_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: インターフェースを使おう
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex6_title",
            slideKeyPrefix = "go_lesson2_ex6",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "// interface でインターフェースを定義", "type Speaker interface {", "    Speak()", "}", "type Dog struct{}", "func (d Dog) Speak() {", "    fmt.Println(\"woof\")", "}", "func main() {", "    // インターフェース型の変数に代入", "    var s Speaker = Dog{}", "    // インターフェースのメソッドを呼び出す", "    s.Speak()", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 2, commentPrefix = "//", localizationKey = "go_lesson2_ex6_comment1" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "go_lesson2_ex6_comment2" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "go_lesson2_ex6_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: defer で後処理
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex7_title",
            slideKeyPrefix = "go_lesson2_ex7",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "func main() {", "    // defer で関数終了時に実行", "    defer fmt.Println(\"end\")", "    // これが先に実行される", "    fmt.Println(\"start\")", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "go_lesson2_ex7_comment1" },
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "go_lesson2_ex7_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: ゴルーチンで並行処理
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex8_title",
            slideKeyPrefix = "go_lesson2_ex8",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import (", "    \"fmt\"", "    \"time\"", ")", "func say(msg string) {", "    fmt.Println(msg)", "}", "func main() {", "    // go でゴルーチンを起動", "    go say(\"hello\")", "    // ゴルーチンの完了を待つ", "    time.Sleep(100 * time.Millisecond)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "go_lesson2_ex8_comment1" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "go_lesson2_ex8_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: チャネルで通信
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex9_title",
            slideKeyPrefix = "go_lesson2_ex9",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "func main() {", "    // chan でチャネルを作成", "    ch := make(chan int)", "    go func() {", "        // チャネルに値を送信", "        ch <- 100", "    }()", "    // チャネルから値を受信", "    val := <-ch", "    fmt.Println(val)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 3, commentPrefix = "//", localizationKey = "go_lesson2_ex9_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "go_lesson2_ex9_comment2" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "go_lesson2_ex9_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: 無名関数とクロージャ
        lesson2.exercises.Add(new Exercise
        {
            titleKey = "go_lesson2_ex10_title",
            slideKeyPrefix = "go_lesson2_ex10",
            slideCount = 3,
            correctLines = new List<string> { "package main", "import \"fmt\"", "func main() {", "    n := 5", "    // func で無名関数を定義", "    double := func() int {", "        // 外側の変数 n を参照", "        return n * 2", "    }", "    fmt.Println(double())", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson2_ex10_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "go_lesson2_ex10_comment2" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: Go III - ジェネリクスとテスト ====================
        var lesson3 = new Lesson { titleKey = "go_lesson3_title" };

        // Ex1: ジェネリクスの基本
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex1_title",
            slideKeyPrefix = "go_lesson3_ex1",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import \"fmt\"", "", "// any で任意の型を受け入れる", "func First[T any](slice []T) T {", "    // スライスの最初の要素を返す", "    return slice[0]", "}", "", "func main() {", "    // 10, 20, 30 でスライスを初期化", "    nums := []int{10, 20, 30}", "    fmt.Println(First(nums))", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson3_ex1_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "go_lesson3_ex1_comment2" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "go_lesson3_ex1_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: 型制約 comparable
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex2_title",
            slideKeyPrefix = "go_lesson3_ex2",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import \"fmt\"", "", "// comparable で比較可能な型に制限", "func IndexOf[T comparable](slice []T, v T) int {", "    // range でスライスをイテレート", "    for i, x := range slice {", "        // == で比較", "        if x == v {", "            return i", "        }", "    }", "    return -1", "}", "", "func main() {", "    // \"a\", \"b\", \"c\" でスライスを初期化", "    names := []string{\"a\", \"b\", \"c\"}", "    fmt.Println(IndexOf(names, \"b\"))", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson3_ex2_comment1" },
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "go_lesson3_ex2_comment2" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "go_lesson3_ex2_comment3" },
                new LocalizedComment { lineIndex = 17, commentPrefix = "//", localizationKey = "go_lesson3_ex2_comment4" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: make でスライス作成
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex3_title",
            slideKeyPrefix = "go_lesson3_ex3",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import \"fmt\"", "", "func main() {", "    // make でスライスを作成", "    nums := make([]int, 3)", "    // インデックス 0 に 10 を代入", "    nums[0] = 10", "    nums[1] = 20", "    nums[2] = 30", "    fmt.Println(nums)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "go_lesson3_ex3_comment1" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "go_lesson3_ex3_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: append でスライス結合
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex4_title",
            slideKeyPrefix = "go_lesson3_ex4",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import \"fmt\"", "", "func main() {", "    // スライスを初期化", "    a := []int{1, 2}", "    b := []int{3, 4}", "    // ... でスライスを展開", "    c := append(a, b...)", "    fmt.Println(c)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "go_lesson3_ex4_comment1" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "go_lesson3_ex4_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: copy でスライスコピー
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex5_title",
            slideKeyPrefix = "go_lesson3_ex5",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import \"fmt\"", "", "func main() {", "    // コピー元のスライス (10, 20, 30)", "    src := []int{10, 20, 30}", "    // make でコピー先を作成", "    dst := make([]int, len(src))", "    // copy でスライスをコピー", "    copy(dst, src)", "    fmt.Println(dst)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "go_lesson3_ex5_comment1" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "go_lesson3_ex5_comment2" },
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "go_lesson3_ex5_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: select で複数チャネル
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex6_title",
            slideKeyPrefix = "go_lesson3_ex6",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import \"fmt\"", "", "func main() {", "    // バッファ付きチャネルを作成", "    ch1 := make(chan int, 1)", "    ch2 := make(chan int, 1)", "    // チャネルに値を送信", "    ch1 <- 10", "    ", "    // select で複数チャネルを待機", "    select {", "    case v := <-ch1:", "        fmt.Println(v)", "    case v := <-ch2:", "        fmt.Println(v)", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "go_lesson3_ex6_comment1" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "go_lesson3_ex6_comment2" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "go_lesson3_ex6_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: range で辞書をループ
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex7_title",
            slideKeyPrefix = "go_lesson3_ex7",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import \"fmt\"", "", "func main() {", "    // マップを初期化 (math: 90, english: 85)", "    scores := map[string]int{\"math\": 90, \"english\": 85}", "    // range で辞書をイテレート", "    for k, v := range scores {", "        fmt.Printf(\"%s: %d\\n\", k, v)", "    }", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 5, commentPrefix = "//", localizationKey = "go_lesson3_ex7_comment1" },
                new LocalizedComment { lineIndex = 7, commentPrefix = "//", localizationKey = "go_lesson3_ex7_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: type で新しい型
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex8_title",
            slideKeyPrefix = "go_lesson3_ex8",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import \"fmt\"", "", "// type で新しい型を定義", "type Age int", "", "func main() {", "    // 新しい型の変数を宣言", "    var age Age = 25", "    fmt.Println(age)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 4, commentPrefix = "//", localizationKey = "go_lesson3_ex8_comment1" },
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "go_lesson3_ex8_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: 埋め込み（Embedding）
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex9_title",
            slideKeyPrefix = "go_lesson3_ex9",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import \"fmt\"", "", "type Base struct {", "    Value int", "}", "", "type Extended struct {", "    // Base を埋め込む", "    Base", "    Extra string", "}", "", "func main() {", "    // 埋め込み構造体を初期化", "    e := Extended{Base: Base{Value: 100}, Extra: \"test\"}", "    fmt.Println(e.Value)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "go_lesson3_ex9_comment1" },
                new LocalizedComment { lineIndex = 15, commentPrefix = "//", localizationKey = "go_lesson3_ex9_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: panic と recover
        lesson3.exercises.Add(new Exercise
        {
            titleKey = "go_lesson3_ex10_title",
            slideKeyPrefix = "go_lesson3_ex10",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import \"fmt\"", "", "func main() {", "    defer func() {", "        // recover でパニックを捕捉", "        if r := recover(); r != nil {", "            fmt.Println(\"caught\")", "        }", "    }()", "    // panic でパニックを発生", "    panic(\"error\")", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 6, commentPrefix = "//", localizationKey = "go_lesson3_ex10_comment1" },
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "go_lesson3_ex10_comment2" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson3);

        // ==================== LESSON 4: Go IV - 並行処理とネットワーク ====================
        var lesson4 = new Lesson { titleKey = "go_lesson4_title" };

        // Ex1: sync.Mutex
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex1_title",
            slideKeyPrefix = "go_lesson4_ex1",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import (", "    \"fmt\"", "    \"sync\"", ")", "", "func main() {", "    var mu sync.Mutex", "    count := 0", "    ", "    // Lock でロックを取得", "    mu.Lock()", "    count++", "    // Unlock でロックを解放", "    mu.Unlock()", "    ", "    fmt.Println(count)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 11, commentPrefix = "//", localizationKey = "go_lesson4_ex1_comment1" },
                new LocalizedComment { lineIndex = 14, commentPrefix = "//", localizationKey = "go_lesson4_ex1_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex2: sync.WaitGroup
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex2_title",
            slideKeyPrefix = "go_lesson4_ex2",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import (", "    \"fmt\"", "    \"sync\"", ")", "", "func main() {", "    var wg sync.WaitGroup", "    // Add でカウンタを増やす", "    wg.Add(1)", "    ", "    go func() {", "        // Done でカウンタを減らす", "        defer wg.Done()", "        fmt.Println(\"goroutine\")", "    }()", "    ", "    // Wait でカウンタが0になるまで待つ", "    wg.Wait()", "    fmt.Println(\"done\")", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 9, commentPrefix = "//", localizationKey = "go_lesson4_ex2_comment1" },
                new LocalizedComment { lineIndex = 13, commentPrefix = "//", localizationKey = "go_lesson4_ex2_comment2" },
                new LocalizedComment { lineIndex = 18, commentPrefix = "//", localizationKey = "go_lesson4_ex2_comment3" },
            },
            expectedOutput = new List<string>()
        });

        // Ex3: context.Background
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex3_title",
            slideKeyPrefix = "go_lesson4_ex3",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import (", "    \"context\"", "    \"fmt\"", ")", "", "func main() {", "    // Background でルートコンテキストを作成", "    ctx := context.Background()", "    // Err でエラーを取得", "    fmt.Println(ctx.Err())", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "go_lesson4_ex3_comment1" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "go_lesson4_ex3_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex4: context.WithCancel
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex4_title",
            slideKeyPrefix = "go_lesson4_ex4",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import (", "    \"context\"", "    \"fmt\"", ")", "", "func main() {", "    // WithCancel でキャンセル可能に", "    ctx, cancel := context.WithCancel(context.Background())", "    // cancel でキャンセル", "    cancel()", "    fmt.Println(ctx.Err())", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "go_lesson4_ex4_comment1" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "go_lesson4_ex4_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex5: time.Duration
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex5_title",
            slideKeyPrefix = "go_lesson4_ex5",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import (", "    \"fmt\"", "    \"time\"", ")", "", "func main() {", "    // Millisecond でミリ秒を表す", "    d := 500 * time.Millisecond", "    fmt.Println(d)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "go_lesson4_ex5_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex6: json.Marshal
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex6_title",
            slideKeyPrefix = "go_lesson4_ex6",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import (", "    \"encoding/json\"", "    \"fmt\"", ")", "", "type Item struct {", "    Name string `json:\"name\"`", "}", "", "func main() {", "    // \"Apple\" で構造体を初期化", "    item := Item{Name: \"Apple\"}", "    // Marshal で JSON に変換", "    data, _ := json.Marshal(item)", "    fmt.Println(string(data))", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 12, commentPrefix = "//", localizationKey = "go_lesson4_ex6_comment1" },
                new LocalizedComment { lineIndex = 14, commentPrefix = "//", localizationKey = "go_lesson4_ex6_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex7: json.Unmarshal
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex7_title",
            slideKeyPrefix = "go_lesson4_ex7",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import (", "    \"encoding/json\"", "    \"fmt\"", ")", "", "type Item struct {", "    Name string `json:\"name\"`", "}", "", "func main() {", "    // \"Banana\" を含むJSON文字列をバイト列に変換", "    data := []byte(`{\"name\":\"Banana\"}`)", "    var item Item", "    // Unmarshal で JSON をパース", "    json.Unmarshal(data, &item)", "    fmt.Println(item.Name)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 12, commentPrefix = "//", localizationKey = "go_lesson4_ex7_comment1" },
                new LocalizedComment { lineIndex = 15, commentPrefix = "//", localizationKey = "go_lesson4_ex7_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex8: strings パッケージ
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex8_title",
            slideKeyPrefix = "go_lesson4_ex8",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import (", "    \"fmt\"", "    \"strings\"", ")", "", "func main() {", "    // 分割する文字列 \"hello,world\"", "    s := \"hello,world\"", "    // Split で文字列を分割", "    parts := strings.Split(s, \",\")", "    fmt.Println(parts[0])", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "go_lesson4_ex8_comment1" },
                new LocalizedComment { lineIndex = 10, commentPrefix = "//", localizationKey = "go_lesson4_ex8_comment2" },
            },
            expectedOutput = new List<string>()
        });

        // Ex9: strconv.Atoi
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex9_title",
            slideKeyPrefix = "go_lesson4_ex9",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import (", "    \"fmt\"", "    \"strconv\"", ")", "", "func main() {", "    // Atoi で文字列を整数に変換", "    num, _ := strconv.Atoi(\"42\")", "    fmt.Println(num * 2)", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "go_lesson4_ex9_comment1" },
            },
            expectedOutput = new List<string>()
        });

        // Ex10: os.Args
        lesson4.exercises.Add(new Exercise
        {
            titleKey = "go_lesson4_ex10_title",
            slideKeyPrefix = "go_lesson4_ex10",
            slideCount = 3,
            correctLines = new List<string> { "package main", "", "import (", "    \"fmt\"", "    \"os\"", ")", "", "func main() {", "    // Args でコマンドライン引数を取得", "    fmt.Println(len(os.Args))", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 8, commentPrefix = "//", localizationKey = "go_lesson4_ex10_comment1" },
            },
            expectedOutput = new List<string>()
        });

        lessons.Add(lesson4);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;
    }