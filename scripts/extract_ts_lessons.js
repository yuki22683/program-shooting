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

  // Lesson title and description
  if (lessonData.lessonTitle) {
    keys[`${langPrefix}_lesson${lessonNum}_title`] = lessonData.lessonTitle;
  }
  if (lessonData.lessonDescription) {
    keys[`${langPrefix}_lesson${lessonNum}_course_description`] = lessonData.lessonDescription;
  }

  // Exercises
  if (lessonData.exercises) {
    lessonData.exercises.forEach((exercise, exIdx) => {
      const exNum = exIdx + 1;
      const exPrefix = `${langPrefix}_lesson${lessonNum}_ex${exNum}`;

      if (exercise.title) {
        keys[`${exPrefix}_title`] = exercise.title;
      }
      if (exercise.description) {
        keys[`${exPrefix}_description`] = exercise.description;
      }

      // Tutorial slides
      if (exercise.tutorialSlides) {
        exercise.tutorialSlides.forEach((slide, slideIdx) => {
          const slideNum = slideIdx + 1;
          const slidePrefix = `${exPrefix}_slide${slideNum}`;

          if (slide.title) {
            keys[`${slidePrefix}_title`] = slide.title;
          }
          if (slide.content) {
            keys[`${slidePrefix}_content`] = slide.content;
          }
        });
      }

      // Line hints (comments)
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

  // Main file (lesson 1)
  const mainFile = path.join(REFERENCE_DIR, `${lang}.ts`);
  if (fs.existsSync(mainFile)) {
    files.push({ file: mainFile, lessonNum: 1 });
  }

  // Additional lessons (2, 3, 4, 5...)
  for (let i = 2; i <= 10; i++) {
    const extraFile = path.join(REFERENCE_DIR, `${lang}${i}.ts`);
    if (fs.existsSync(extraFile)) {
      files.push({ file: extraFile, lessonNum: i });
    }
  }

  return files;
}

function loadTsData(filePath) {
  try {
    const content = fs.readFileSync(filePath, 'utf-8');

    // Extract the object from export statement
    const match = content.match(/export\s+const\s+\w+Data\s*=\s*(\{[\s\S]*\})\s*;?\s*$/);
    if (!match) {
      console.error(`Could not extract object from ${filePath}`);
      return null;
    }

    // Use eval to parse the JavaScript object (be careful with this in production!)
    const objStr = match[1];
    const data = eval('(' + objStr + ')');
    return data;
  } catch (err) {
    console.error(`Error loading ${filePath}: ${err.message}`);
    return null;
  }
}

function main() {
  const allRefKeys = {};
  const statsByLang = {};

  console.log('Extracting Japanese data from TypeScript files...\n');

  for (const lang of LANGUAGES) {
    const files = getLessonFiles(lang);
    let totalKeys = 0;

    for (const { file, lessonNum } of files) {
      const data = loadTsData(file);
      if (data) {
        const keys = extractKeysFromLesson(lang, data, lessonNum);
        Object.assign(allRefKeys, keys);
        totalKeys += Object.keys(keys).length;
        console.log(`  ${path.basename(file)}: ${Object.keys(keys).length} keys`);
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
    console.log(`  ${lang}: ${count}`);
  }
}

main();
