const fs = require('node:fs');
const file = 'Assets/Resources/pythonLessons.json';
let data = JSON.parse(fs.readFileSync(file, 'utf8'));

const langs = Object.keys(data).filter(l => l !== 'en' && l !== 'common');

// 翻訳マップ（一部抜粋だが、すべてのスライド構造を網羅するように拡張）
const patterns = {
    'ja': { 'try': 'やってみよう！', 'start_msg': '\n\n（準備ができたら「レッスン開始」を選択してください。）' },
    'ko': { 'try': '직접 해보세요!', 'start_msg': '\n\n(준비가 되면 「레슨 시작」을 선택해 주세요.)' },
    'zh-Hans': { 'try': '尝试一下！', 'start_msg': '\n\n（准备好后，请选择“开始课程”。）' },
    'fr': { 'try': 'À vous de jouer !', 'start_msg': '\n\n(Une fois prêt, sélectionnez « Commencer la leçon ».)' },
    'de': { 'try': 'Probier es aus!', 'start_msg': '\n\n(Wenn du bereit bist, wähle „Lektion starten“.)' },
    'es': { 'try': '¡Inténtalo!', 'start_msg': '\n\n(Cuando estés listo, selecciona 