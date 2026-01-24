/**
 * JavaScript Lessons Converter
 *
 * This script reads JavaScript lesson data from senkou-code and converts it to:
 * 1. javascript_lessons.json - For LessonManager.cs
 * 2. javascript_localized.json - For localizedText.json
 */

const fs = require('fs');
const path = require('path');

// Source paths
const sourcePath = 'C:\\Work\\git\\senkou-code\\data\\lessons';
const outputPath = 'C:\\Work\\MetaXR\\ProgramShooting\\scripts\\output';

// Read TypeScript files and extract data
function readTsFile(filename) {
    const filepath = path.join(sourcePath, filename);
    const content = fs.readFileSync(filepath, 'utf-8');

    // Extract the data object from the TypeScript file
    // Remove 'export const javascriptDataX = ' and trailing ';'
    const match = content.match(/export const \w+ = ({[\s\S]*});?\s*$/);
    if (!match) {
        throw new Error(`Could not parse ${filename}`);
    }

    // Evaluate the JSON-like object
    // First, handle the bq variable if present
    let jsonStr = match[1];
    // Replace any backtick references
    jsonStr = jsonStr.replace(/\${bq}/g, '`');

    try {
        return eval('(' + jsonStr + ')');
    } catch (e) {
        console.error(`Error parsing ${filename}:`, e.message);
        throw e;
    }
}

// Convert markdown to plain text for slides
function markdownToPlainText(md) {
    if (!md) return '';
    return md
        .replace(/^#+ /gm, '')  // Remove headers
        .replace(/\*\*/g, '')   // Remove bold
        .replace(/```[\s\S]*?```/g, (match) => {
            // Keep code blocks but remove markers
            return match.replace(/```\w*\n?/g, '').trim();
        })
        .replace(/`([^`]+)`/g, '$1')  // Remove inline code markers
        .replace(/\[([^\]]+)\]\([^)]+\)/g, '$1')  // Convert links to text
        .replace(/\n{3,}/g, '\n\n')  // Reduce multiple newlines
        .trim();
}

// Generate Unity exercise data
function generateUnityExercise(exercise, lessonNum, exNum) {
    const prefix = `javascript_lesson${lessonNum}_ex${exNum}`;

    // Count slides
    const slideCount = exercise.tutorialSlides ? exercise.tutorialSlides.length : 2;

    // Generate correct lines (handling arrays for alternatives)
    const correctLines = exercise.correctLines || exercise.correctCode.split('\n');

    // Process comment keys
    const comments = [];
    if (exercise.correctCode) {
        const lines = exercise.correctCode.split('\n');
        let commentIndex = 1;
        for (const line of lines) {
            if (line.trim().startsWith('//')) {
                comments.push(`${prefix}_comment${commentIndex}`);
                commentIndex++;
            }
        }
    }

    return {
        title: `${prefix}_title`,
        comments: comments,
        slideKeyPrefix: prefix,
        slideCount: slideCount,
        correctCode: exercise.correctCode,
        correctLines: correctLines.map(line => {
            if (Array.isArray(line)) {
                return line;
            }
            return line;
        }),
        candidates: exercise.candidates || {}
    };
}

// Generate localized text for an exercise
function generateLocalizedText(exercise, lessonNum, exNum, lang) {
    const prefix = `javascript_lesson${lessonNum}_ex${exNum}`;
    const result = {};

    // Title
    result[`${prefix}_title`] = exercise.title;

    // Description (stored separately, not as a key)
    result[`${prefix}_description`] = exercise.description;

    // Comments from correct code
    if (exercise.correctCode) {
        const lines = exercise.correctCode.split('\n');
        let commentIndex = 1;
        for (const line of lines) {
            if (line.trim().startsWith('//')) {
                const comment = line.trim().replace(/^\/\/\s*/, '');
                result[`${prefix}_comment${commentIndex}`] = comment;
                commentIndex++;
            }
        }
    }

    // Slides
    if (exercise.tutorialSlides) {
        exercise.tutorialSlides.forEach((slide, idx) => {
            const slideNum = idx + 1;
            result[`${prefix}_slide${slideNum}_title`] = slide.title;
            result[`${prefix}_slide${slideNum}_content`] = markdownToPlainText(slide.content);
        });
    }

    return result;
}

// Generate course-level localized text
function generateCourseLocalizedText(lessonData, lessonNum) {
    const prefix = `javascript_lesson${lessonNum}`;
    return {
        [`${prefix}_course_title`]: lessonData.lessonTitle,
        [`${prefix}_course_description`]: lessonData.lessonDescription
    };
}

// Main conversion function
function convertJavaScriptLessons() {
    // Ensure output directory exists
    if (!fs.existsSync(outputPath)) {
        fs.mkdirSync(outputPath, { recursive: true });
    }

    const files = [
        'javascript.ts',
        'javascript2.ts',
        'javascript3.ts',
        'javascript4.ts',
        'javascript5.ts'
    ];

    const allLessons = [];
    const localizedText = {
        en: {},
        ja: {}
    };

    files.forEach((file, fileIndex) => {
        const lessonNum = fileIndex + 1;
        console.log(`Processing ${file} (Lesson ${lessonNum})...`);

        const data = readTsFile(file);

        // Add course-level info
        const courseInfo = generateCourseLocalizedText(data, lessonNum);
        Object.assign(localizedText.ja, courseInfo);

        // English course info (using same Japanese text for now - will need translation)
        const enCourseInfo = {};
        Object.keys(courseInfo).forEach(key => {
            // For English, we'll create placeholder translations
            if (key.includes('_title')) {
                enCourseInfo[key] = data.lessonTitle.replace(/JavaScript/g, 'JavaScript');
            } else {
                enCourseInfo[key] = data.lessonDescription;
            }
        });
        Object.assign(localizedText.en, enCourseInfo);

        // Process exercises
        const lessonExercises = [];

        data.exercises.forEach((exercise, exIndex) => {
            const exNum = exIndex + 1;

            // Generate Unity exercise data
            const unityExercise = generateUnityExercise(exercise, lessonNum, exNum);
            lessonExercises.push(unityExercise);

            // Generate localized text (Japanese - original)
            const jaLocalized = generateLocalizedText(exercise, lessonNum, exNum, 'ja');
            Object.assign(localizedText.ja, jaLocalized);

            // For English, use same text (Japanese) as placeholder
            Object.assign(localizedText.en, jaLocalized);
        });

        allLessons.push({
            lessonId: data.lessonId,
            lessonNum: lessonNum,
            difficulty: data.lessonDifficulty,
            exerciseCount: lessonExercises.length,
            exercises: lessonExercises
        });
    });

    // Write Unity lessons JSON
    const lessonsOutput = {
        language: 'javascript',
        totalCourses: allLessons.length,
        courses: allLessons.map((lesson, idx) => ({
            courseId: lesson.lessonId,
            courseNum: lesson.lessonNum,
            titleKey: `javascript_lesson${lesson.lessonNum}_course_title`,
            descriptionKey: `javascript_lesson${lesson.lessonNum}_course_description`,
            difficulty: lesson.difficulty,
            exercises: lesson.exercises
        }))
    };

    fs.writeFileSync(
        path.join(outputPath, 'javascript_lessons.json'),
        JSON.stringify(lessonsOutput, null, 2),
        'utf-8'
    );
    console.log('Written: javascript_lessons.json');

    // Write localized text JSON
    fs.writeFileSync(
        path.join(outputPath, 'javascript_localized.json'),
        JSON.stringify(localizedText, null, 2),
        'utf-8'
    );
    console.log('Written: javascript_localized.json');

    // Print summary
    console.log('\n=== Summary ===');
    console.log(`Total courses: ${allLessons.length}`);
    allLessons.forEach(lesson => {
        console.log(`  Lesson ${lesson.lessonNum}: ${lesson.exerciseCount} exercises`);
    });
    console.log(`\nLocalized keys generated:`);
    console.log(`  Japanese: ${Object.keys(localizedText.ja).length}`);
    console.log(`  English: ${Object.keys(localizedText.en).length}`);
}

// Run the conversion
convertJavaScriptLessons();
