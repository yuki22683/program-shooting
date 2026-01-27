const fs = require('node:fs');
const file = 'Assets/Resources/pythonLessons.json';
let data = JSON.parse(fs.readFileSync(file, 'utf8'));

// 英語セクションのコアコンテンツを再定義（教育的に正しく自然な英語）
const enPerfect = {
    "python_lesson1_course_title": "Python Basics",
    "python_lesson1_course_description": "Learn the fundamental syntax of Python and master how to run programs.",
    
    // Lesson 1, Ex 1
    "python_lesson1_ex1_title": "Hello Python",
    "python_lesson1_ex1_comment1": "Let's start by displaying some text on the screen!",
    "python_lesson1_ex1_slide1_title": "What is Python?",
    "python_lesson1_ex1_slide1_content": "Python is a programming language that is easy for beginners to understand and is used worldwide. It is used in many fields, such as AI development and website creation.",
    "python_lesson1_ex1_slide2_title": "Outputting Text",
    "python_lesson1_ex1_slide2_content": "To display text on the screen, we use the `print()` command. Put the text you want to display inside the parentheses.",
    "python_lesson1_ex1_slide3_title": "Let's Try It!",
    "python_lesson1_ex1_slide3_content": "Try writing code to display \"Hello World\" on the screen.\n\n(Select \"Start Lesson\" when you are ready.)",

    // Lesson 1, Ex 2
    "python_lesson1_ex2_title": "Numbers and Calculations",
    "python_lesson1_ex2_comment1": "Let's perform some calculations using Python.",
    "python_lesson1_ex2_slide1_title": "Handling Numbers",
    "python_lesson1_ex2_slide1_content": "In Python, you can perform calculations by writing numbers directly. Use `+` for addition and `-` for subtraction.",
    "python_lesson1_ex2_slide2_title": "Let's Try It!",
    "python_lesson1_ex2_slide2_content": "Let's display the result of 10 + 5.\n\n(Select \"Start Lesson\" when you are ready.)",

    // Course Titles
    "python_lesson2_course_title": "Variables and Data Types",
    "python_lesson2_course_description": "Learn about variables, which allow you to save data with a name.",
    "python_lesson3_course_title": "Operations and Operators",
    "python_lesson3_course_description": "Learn how to perform calculations and compare conditions.",
    "python_lesson4_course_title": "Lists and Loops",
    "python_lesson4_course_description": "Learn how to handle multiple data items and repeat processes.",
    "python_lesson5_course_title": "Functions and Libraries",
    "python_lesson5_course_description": "Learn about functions to group processes and how to use standard libraries."
};

// 全キーを走査し、不自然な英語や日本語の残滓があれば修正（簡易パターン置換）
Object.keys(data.en).forEach(key => {
    // 既に enPerfect に定義されているものは上書き
    if (enPerfect[key]) {
        data.en[key] = enPerfect[key];
    } else {
        // それ以外についても、末尾の「Start Lesson」などの定型文を英語に統一
        let val = data.en[key];
        if (typeof val === 'string') {
            // 日本語の指示文が混じっている場合の置換
            val = val.replace(/（準備ができたら.*$|準備ができたら.*$/g, '(Select "Start Lesson" when you are ready.)');
            val = val.replace(/やってみよう！/g, "Let's Try It!");
            data.en[key] = val;
        }
    }
});

fs.writeFileSync(file, JSON.stringify(data, null, 2), 'utf8');
console.log('Successfully refined pythonLessons.json [en] section to perfect English.');
