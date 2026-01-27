const fs = require('node:fs');
const path = require('node:path');

const file = 'Assets/Resources/pythonLessons.json';
let data = JSON.parse(fs.readFileSync(file, 'utf8'));

const translations = {
    'ja': {
        'python_lesson1_course_title': 'Pythonの基本',
        'python_lesson1_course_description': 'Pythonの基本文法を学び、プログラムの動かし方をマスターしましょう。',
        'python_lesson1_ex1_title': 'こんにちは Python',
        'python_lesson1_ex1_comment1': 'まずは画面に文字を表示させるところから始めましょう！',
        'python_lesson1_ex1_slide1_title': 'Pythonとは？',
        'python_lesson1_ex1_slide1_content': 'Python（パイソン）は、初心者にも分かりやすく、世界中で使われているプログラミング言語です。AIの開発やWebサイトの作成など、幅広く使われています。',
        'python_lesson1_ex1_slide2_title': '文字を出力しよう',
        'python_lesson1_ex1_slide2_content': '画面に文字を表示するには「print()」という命令を使います。カッコの中に表示したい文字を入れます。',
        'python_lesson2_course_title': '変数とデータ型',
        'python_lesson3_course_title': '計算と演算子',
        'python_lesson4_course_title': 'リストとループ'
    },
    'ko': {
        'python_lesson1_course_title': 'Python의 기초',
        'python_lesson1_course_description': 'Python의 기본 문법을 배우고 프로그램 작동 방식을 마스터하세요.',
        'python_lesson1_ex1_title': '안녕 Python',
        'python_lesson1_ex1_slide1_title': 'Python이란?',
        'python_lesson1_ex1_slide1_content': 'Python은 배우기 쉽고 전 세계적으로 널리 사용되는 프로그래ミング 언어입니다. AI 개발이나 웹사이트 제작 등 다양한 분야에서 활용됩니다.'
    },
    'zh-Hans': {
        'python_lesson1_course_title': 'Python 基础',
        'python_lesson1_course_description': '学习 Python 的基本语法，掌握运行程序的方法。',
        'python_lesson1_ex1_title': '你好 Python',
        'python_lesson1_ex1_slide1_title': '什么是 Python？',
        'python_lesson1_ex1_slide1_content': 'Python 是一种简单易学、在全球范围内广泛使用的编程语言。它在 AI 开发和网站建设等多个领域都有应用。'
    }
};

Object.keys(translations).forEach(lang => {
    if (!data[lang]) data[lang] = {};
    Object.assign(data[lang], translations[lang]);
});

fs.writeFileSync(file, JSON.stringify(data, null, 2), 'utf8');
console.log('Successfully updated pythonLessons.json with correct translations.');
