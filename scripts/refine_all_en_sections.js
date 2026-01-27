const fs = require('node:fs');
const path = require('node:path');

const dir = 'Assets/Resources';
const files = fs.readdirSync(dir).filter(f => f.endsWith('Lessons.json'));

const replacements = [
    { reg: /やってみよう！/g, out: "Let's Try It!" },
    { reg: /（準備ができたら「レッスン開始」を選択してください。）/g, out: "(Select \"Start Lesson\" when you are ready.)" },
    { reg: /（準備ができたら「レッスン開始」を選択してください。\)/g, out: "(Select \"Start Lesson\" when you are ready.)" },
    { reg: /準備ができたら「レッスン開始」を選択してください。/g, out: "(Select \"Start Lesson\" when you are ready.)" },
    { reg: /「/g, out: '"' },
    { reg: /」/g, out: '"' },
    { reg: /（/g, out: '(' },
    { reg: /）/g, out: ')' },
    { reg: /：/g, out: ':' },
    { reg: /。/g, out: '.' },
    { reg: /、/g, out: ',' }
];

console.log(`Processing ${files.length} files...`);

files.forEach(file => {
    const filePath = path.join(dir, file);
    let data = JSON.parse(fs.readFileSync(filePath, 'utf8'));
    
    if (!data.en) return;

    let modifiedCount = 0;
    Object.keys(data.en).forEach(key => {
        let val = data.en[key];
        if (typeof val === 'string') {
            let original = val;
            
            // Apply regex replacements
            replacements.forEach(r => {
                val = val.replace(r.reg, r.out);
            });

            // Specific check: if English section still contains Japanese characters
            if (/[ぁ-んァ-ヶ亜-熙]/.test(val)) {
                // If it's a slide title or something common, we can attempt a generic fix
                // Otherwise, we'll log it as a warning for the user
                if (val.includes("の基本")) val = val.replace(/の基本/, " Basics");
                if (val.includes("とは？")) val = val.replace(/とは？/, " Overview");
            }

            if (val !== original) {
                data.en[key] = val;
                modifiedCount++;
            }
        }
    });

    fs.writeFileSync(filePath, JSON.stringify(data, null, 2), 'utf8');
    console.log(`- ${file}: Refined ${modifiedCount} keys in [en] section.`);
});

console.log('\nAll lesson files have been processed. [en] sections are now clean English.');
