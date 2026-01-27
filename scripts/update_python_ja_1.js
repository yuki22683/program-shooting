const fs = require('node:fs');
const file = 'Assets/Resources/pythonLessons.json';
let data = JSON.parse(fs.readFileSync(file, 'utf8'));

const jaFull = {
  "python_lesson1_course_title": "Pythonの基本",
  "python_lesson1_course_description": "Pythonの基本文法を学び、プログラムの動かし方をマスターしましょう。",
  "python_lesson1_ex1_title": "こんにちは Python",
  "python_lesson1_ex1_comment1": "まずは画面に文字を表示させるところから始めましょう！",
  "python_lesson1_ex1_slide1_title": "Pythonとは？",
  "python_lesson1_ex1_slide1_content": "Python（パイソン）は、初心者にも分かりやすく、世界中で使われているプログラミング言語です。AIの開発やWebサイトの作成など、幅広く使われています。",
  "python_lesson1_ex1_slide2_title": "文字を出力しよう",
  "python_lesson1_ex1_slide2_content": "画面に文字を表示するには「print()」という命令を使います。カッコの中に表示したい文字を入れます。",
  "python_lesson1_ex1_slide3_title": "やってみよう！",
  "python_lesson1_ex1_slide3_content": "実際にコードを書いて、画面に「Hello World」と表示させてみましょう。\n\n（準備ができたら「レッスン開始」を選択してください。）",
  "python_lesson1_ex2_title": "数値と計算",
  "python_lesson1_ex2_slide1_title": "数値の扱い",
  "python_lesson1_ex2_slide1_content": "Pythonでは数字をそのまま書くことで、計算を行うことができます。足し算は「+」、引き算は「-」を使います。",
  "python_lesson1_ex2_slide2_title": "やってみよう！",
  "python_lesson1_ex2_slide2_content": "10 + 5 の計算結果を表示させてみましょう。\n\n（準備ができたら「レッスン開始」を選択してください。）",
  "python_lesson2_course_title": "変数とデータ型",
  "python_lesson2_course_description": "データに名前をつけて保存する「変数」について学びます。",
  "python_lesson2_ex1_title": "変数を使ってみよう",
  "python_lesson2_ex1_slide1_title": "変数とは？",
  "python_lesson2_ex1_slide1_content": "変数を使うと、データに名前をつけて保存できます。例えば x = 10 と書くと、x という名前に 10 を保存できます。",
  "python_lesson2_ex1_slide2_title": "やってみよう！",
  "python_lesson2_ex1_slide2_content": "変数を使った計算をしてみましょう。\n\n（準備ができたら「レッスン開始」を選択してください。）"
};

Object.assign(data.ja, jaFull);
fs.writeFileSync(file, JSON.stringify(data, null, 2), 'utf8');
console.log('Fixed Japanese for Python Lesson 1-2.');
