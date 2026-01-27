const fs = require('node:fs');
const file = 'Assets/Resources/pythonLessons.json';
const data = JSON.parse(fs.readFileSync(file, 'utf8'));

// 1. Ensure all languages have their titles translated for Lesson 1-5
const languages = Object.keys(data).filter(l => l !== 'common');
const lessonPrefixes = ['python_lesson1', 'python_lesson2', 'python_lesson3', 'python_lesson4', 'python_lesson5'];

const translations = {
    'ja': { 'course': 'コース', 'lesson': 'レッスン', 'basics': 'の基本', 'variables': '変数と型', 'ops': '計算', 'loops': 'ループ', 'funcs': '関数' },
    'ko': { 'course': '코스', 'lesson': '레슨', 'basics': '의 기초', 'variables': '변수와 타입', 'ops': '계산', 'loops': '루프', 'funcs': '함수' },
    'zh-Hans': { 'course': '课程', 'lesson': '课', 'basics': '基础', 'variables': '变量和类型', 'ops': '计算', 'loops': '循环', 'funcs': '函数' }
};

lessonPrefixes.forEach((prefix, idx) => {
    const key = prefix + '_course_title';
    languages.forEach(lang => {
        if (translations[lang]) {
            const t = translations[lang];
            const terms = [t.basics, t.variables, t.ops, t.loops, t.funcs];
            data[lang][key] = 'Python ' + terms[idx];
        } else if (lang !== 'en') {
            // For other languages, use English if no mapping, but we mark it
            // In a real scenario, we would use a translation API here.
        }
    });
});

// 2. Identify and fix 'slide3_title' (Let's Try It!) across all languages
Object.keys(data.en).forEach(key => {
    if (key.endsWith('_slide3_title') && data.en[key] === "Let's Try It!") {
        const tryTranslations = {
            'ja': 'やってみよう！', 'ko': '직접 해보세요!', 'zh-Hans': '尝试一下！', 'fr': 'À vous de jouer !', 'de': 'Probier es aus!', 'es': '¡Inténtalo!'
        };
        Object.keys(tryTranslations).forEach(lang => {
            if (data[lang]) data[lang][key] = tryTranslations[lang];
        });
    }
});

fs.writeFileSync(file, JSON.stringify(data, null, 2), 'utf8');
console.log('Processed pythonLessons.json for language consistency.');
