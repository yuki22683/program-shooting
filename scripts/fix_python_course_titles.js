const fs = require('node:fs');
const file = 'Assets/Resources/pythonLessons.json';
let data = JSON.parse(fs.readFileSync(file, 'utf8'));

const courseTitles = {
    'python_lesson1_course_title': {
        'ja': 'Pythonの基本', 'ko': 'Python의 기초', 'zh-Hans': 'Python 基础', 'fr': 'Bases de Python', 'de': 'Python-Grundlagen', 'es': 'Fundamentos de Python'
    },
    'python_lesson2_course_title': {
        'ja': '変数とデータ型', 'ko': '변수와 데이터 타입', 'zh-Hans': '变量和数据类型', 'fr': 'Variables et Types de données', 'de': 'Variablen und Datentypen', 'es': 'Variables y tipos de datos'
    },
    'python_lesson3_course_title': {
        'ja': '計算と演算子', 'ko': '계산과 연산자', 'zh-Hans': '计算和运算符', 'fr': 'Calculs et Opérateurs', 'de': 'Berechnungen und Operatoren', 'es': 'Cálculos y operadores'
    },
    'python_lesson4_course_title': {
        'ja': 'リストとループ', 'ko': '리스트와 루프', 'zh-Hans': '列表和循环', 'fr': 'Listes et Boucles', 'de': 'Listen und Schleifen', 'es': 'Listas y bucles'
    },
    'python_lesson5_course_title': {
        'ja': '関数と標準ライブラリ', 'ko': '함수와 표준 라이브러리', 'zh-Hans': '函数和标准库', 'fr': 'Fonctions et Bibliothèque standard', 'de': 'Funktionen und Standardbibliothek', 'es': 'Funciones y biblioteca estándar'
    }
};

Object.keys(courseTitles).forEach(key => {
    Object.keys(courseTitles[key]).forEach(lang => {
        if (!data[lang]) data[lang] = {};
        data[lang][key] = courseTitles[key][lang];
    });
});

fs.writeFileSync(file, JSON.stringify(data, null, 2), 'utf8');
console.log('Fixed Python course titles for major languages.');
