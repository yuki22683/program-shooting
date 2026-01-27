const fs = require('node:fs');
const file = 'Assets/Resources/pythonLessons.json';
let data = JSON.parse(fs.readFileSync(file, 'utf8'));

// Unicode-escaped Japanese to avoid environment issues
const jaUpdates = {
    "python_lesson1_course_title": "\u0050\u0079\u0074\u0068\u006f\u006e\u306e\u57fa\u672c",
    "python_lesson1_ex1_title": "\u3053\u3093\u306b\u3061\u306f\u0020\u0050\u0079\u0074\u0068\u006f\u006e",
    "python_lesson1_ex1_comment1": "\u307e\u305a\u306f\u753b\u9762\u306b\u6587\u5b57\u3092\u8868\u793a\u3055\u305b\u308b\u3068\u3053\u308d\u304b\u3089\u59cb\u3081\u307e\u3057\u3087\u3046\uff01",
    "python_lesson1_ex1_slide1_title": "\u0050\u0079\u0074\u0068\u006f\u006e\u3068\u306f\uff1f",
    "python_lesson2_course_title": "\u5909\u6570\u3068\u30c7\u30fc\u30bf\u578b",
    "python_lesson3_course_title": "\u8a08\u7b97\u3068\u6\u6f14\u7b97\u5b50",
    "python_lesson4_course_title": "\u30ea\u30b9\u30c8\u3068\u30eb\u30fc\u30d7",
    "python_lesson5_course_title": "\u95a2\u6570\u3068\u6a19\u6e96\u30e9\u30a4\u30d6\u30e9\u30ea"
};

Object.assign(data.ja, jaUpdates);
fs.writeFileSync(file, JSON.stringify(data, null, 2), 'utf8');
console.log('Fixed Python JA basic keys using Unicode escape.');
