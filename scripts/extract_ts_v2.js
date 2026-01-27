const fs = require('fs');
const path = require('path');

const REFERENCE_DIR = 'C:/Work/git/senkou-code/data/lessons';
const OUTPUT_DIR = 'C:/Work/MetaXR/ProgramShooting';

const LANGUAGES = [
  'python', 'javascript', 'typescript', 'java',
  'c', 'cpp', 'csharp', 'assembly',
  'go', 'rust', 'ruby', 'php',
  'swift', 'kotlin', 'bash', 'sql',
  'lua', 'perl', 'haskell', 'elixir'
];

function extractKeysFromLesson(langPrefix, lessonData, lessonNum) {
  const keys = {};

  if (lessonData.lessonTitle) {
    keys[`${langPrefix}_lesson${lessonNum}_title`] = lessonData.lessonTitle;
  }
  if (lessonData.lessonDescription) {
    keys[`${langPrefix}_lesson${lessonNum}_course_description`] = lessonData.lessonDescription;
  }

  if (lessonData.exercises) {
    lessonData.exercises.forEach((exercise, exIdx) => {
      const exNum = exIdx + 1;
      const exPrefix = `${langPrefix}_lesson${lessonNum}_ex${exNum}`;

      if (exercise.title) keys[`${exPrefix}_title`] = exercise.title;
      if (exercise.description) keys[`${exPrefix}_description`] = exercise.description;

      if (exercise.tutorialSlides) {
        exercise.tutorialSlides.forEach((slide, slideIdx) => {
          const slideNum = slideIdx + 1;
          const slidePrefix = `${exPrefix}_slide${slideNum}`;
          if (slide.title) keys[`${slidePrefix}_title`] = slide.title;
          if (slide.content) keys[`${slidePrefix}_content`] = slide.content;
        });
      }

      if (exercise.lineHints) {
        let hintNum = 1;
        exercise.lineHints.forEach((hint) => {
          if (hint) {
            keys[`${exPrefix}_comment${hintNum}`] = hint;
            hintNum++;
          }
        });
      }
    });
  }

  return keys;
}

function getLessonFiles(lang) {
  const files = [];
  const mainFile = path.join(REFERENCE_DIR, `${lang}.ts`);
  if (fs.existsSync(mainFile)) {
    files.push({ file: mainFile, lessonNum: 1 });
  }
  for (let i = 2; i <= 10; i++) {
    const extraFile = path.join(REFERENCE_DIR, `${lang}${i}.ts`);
    if (fs.existsSync(extraFile)) {
      files.push({ file: extraFile, lessonNum: i });
    }
  }
  return files;
}

function loadTsDataSafe(filePath) {
  try {
    let content = fs.readFileSync(filePath, 'utf-8');

    // Handle bq variable for backticks
    const bq = '`';
    content = content.replace(/\$\{bq\}/g, bq);

    // Check for and handle bq definition
    if (content.includes('const bq = String.fromCharCode(96)')) {
      content = content.replace(/const bq = String\.fromCharCode\(96\);?\s*\n?/, '');
    }

    // Extract the object - handle variable names like pythonData, pythonData2, etc.
    const match = content.match(/export\s+const\s+\w+\s*=\s*(\{[\s\S]*\})\s*;?\s*$/);
    if (!match) {
      console.error(`Could not extract object from ${filePath}`);
      return null;
    }

    let objStr = match[1];

    // Replace template literals ${bq} with actual backticks
    objStr = objStr.replace(/\$\{bq\}/g, '`');

    // Try Function constructor instead of eval (safer in some cases)
    try {
      const fn = new Function('return ' + objStr);
      return fn();
    } catch (e) {
      // If that fails, try with vm module for safer execution
      const vm = require('vm');
      const sandbox = {};
      const code = 'result = ' + objStr;
      try {
        vm.runInNewContext(code, sandbox, { timeout: 5000 });
        return sandbox.result;
      } catch (e2) {
        console.error(`Parse error in ${path.basename(filePath)}: ${e2.message}`);
        return null;
      }
    }
  } catch (err) {
    console.error(`Error loading ${path.basename(filePath)}: ${err.message}`);
    return null;
  }
}

function main() {
  const allRefKeys = {};
  const statsByLang = {};

  console.log('Extracting Japanese data from TypeScript files (v2)...\n');

  for (const lang of LANGUAGES) {
    const files = getLessonFiles(lang);
    let totalKeys = 0;

    for (const { file, lessonNum } of files) {
      const data = loadTsDataSafe(file);
      if (data) {
        const keys = extractKeysFromLesson(lang, data, lessonNum);
        Object.assign(allRefKeys, keys);
        totalKeys += Object.keys(keys).length;
        console.log(`  [OK] ${path.basename(file)}: ${Object.keys(keys).length} keys`);
      } else {
        console.log(`  [NG] ${path.basename(file)}`);
      }
    }

    statsByLang[lang] = totalKeys;
  }

  console.log(`\nTotal reference keys: ${Object.keys(allRefKeys).length}`);

  // Save extracted data
  const outputPath = path.join(OUTPUT_DIR, 'reference_ja_complete.json');
  fs.writeFileSync(outputPath, JSON.stringify(allRefKeys, null, 2), 'utf-8');
  console.log(`\nSaved to: ${outputPath}`);

  // Print summary by language
  console.log('\nKeys per language:');
  for (const [lang, count] of Object.entries(statsByLang)) {
    const status = count > 0 ? '[OK]' : '[NG]';
    console.log(`  ${status} ${lang}: ${count}`);
  }

  // Count successes
  const successCount = Object.values(statsByLang).filter(c => c > 0).length;
  console.log(`\nSuccessfully extracted: ${successCount}/${Object.keys(statsByLang).length} languages`);
}

main();
