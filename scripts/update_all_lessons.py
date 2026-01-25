#!/usr/bin/env python3
"""
Update all lesson JSON files from senkou-code source data
"""
import json
import re
import os
from pathlib import Path

def parse_ts_file(filepath):
    """Parse TypeScript file and extract JSON data"""
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()

    # Extract the JSON part (after 'export const XXXData = ')
    json_start = content.find('{')
    if json_start == -1:
        return None
    json_str = content[json_start:]

    # Remove trailing semicolon and whitespace
    json_str = json_str.rstrip()
    if json_str.endswith(';'):
        json_str = json_str[:-1]

    try:
        return json.loads(json_str)
    except json.JSONDecodeError as e:
        print(f"  Warning: Failed to parse {filepath}: {e}")
        return None

def extract_comments_from_correct_code(correct_code):
    """Extract comment lines from correctCode based on language"""
    comments = []
    lines = correct_code.split('\n')
    for line in lines:
        stripped = line.strip()
        # Python-style comments
        if stripped.startswith('#'):
            comment_text = stripped[1:].strip()
            comments.append(comment_text)
        # C-style single-line comments
        elif stripped.startswith('//'):
            comment_text = stripped[2:].strip()
            comments.append(comment_text)
        # Assembly-style comments (;)
        elif stripped.startswith(';'):
            comment_text = stripped[1:].strip()
            comments.append(comment_text)
    return comments

def get_comment_prefix(language):
    """Get the comment prefix for a language"""
    if language in ['python']:
        return '#'
    elif language in ['assembly']:
        return ';'
    else:
        return '//'

def update_language_lessons(language, source_files, target_file):
    """Update lesson JSON for a specific language"""
    print(f"\n=== Updating {language} ===")

    # Load existing target JSON
    if not os.path.exists(target_file):
        print(f"  Target file not found: {target_file}")
        return False

    with open(target_file, 'r', encoding='utf-8') as f:
        target_data = json.load(f)

    ja = target_data.get('ja', {})

    total_exercises = 0

    for lesson_idx, source_file in enumerate(source_files):
        lesson_num = lesson_idx + 1

        if not os.path.exists(source_file):
            print(f"  Source file not found: {source_file}")
            continue

        source_data = parse_ts_file(source_file)
        if source_data is None:
            continue

        exercises = source_data.get('exercises', [])

        # Update lesson title
        lesson_title = source_data.get('lessonTitle', '')
        lesson_title_key = f"{language}_lesson{lesson_num}_title"
        ja[lesson_title_key] = lesson_title

        # Update course title and description
        course_title_key = f"{language}_lesson{lesson_num}_course_title"
        course_desc_key = f"{language}_lesson{lesson_num}_course_description"
        ja[course_title_key] = lesson_title
        ja[course_desc_key] = source_data.get('lessonDescription', '')

        for ex_idx, exercise in enumerate(exercises):
            ex_num = ex_idx + 1
            prefix = f"{language}_lesson{lesson_num}_ex{ex_num}"

            # Get slides
            slides = exercise.get('tutorialSlides', [])
            description = exercise.get('description', '')

            # Update slide data
            for slide_idx, slide in enumerate(slides):
                slide_num = slide_idx + 1

                # Title
                title_key = f"{prefix}_slide{slide_num}_title"
                ja[title_key] = slide.get('title', '')

                # Content
                content_key = f"{prefix}_slide{slide_num}_content"
                ja[content_key] = slide.get('content', '')

                # Image (if exists)
                if 'image' in slide:
                    image_key = f"{prefix}_slide{slide_num}_image"
                    ja[image_key] = slide['image']

            # Add "Let's Try It!" slide (final slide)
            final_slide_num = len(slides) + 1
            final_title_key = f"{prefix}_slide{final_slide_num}_title"
            final_content_key = f"{prefix}_slide{final_slide_num}_content"

            ja[final_title_key] = "やってみよう！"
            ja[final_content_key] = f"{description}\n\n（準備ができたら「レッスン開始」を選択してください。）"

            # Extract and update comments from correctCode
            correct_code = exercise.get('correctCode', '')
            comments = extract_comments_from_correct_code(correct_code)

            for comment_idx, comment in enumerate(comments):
                comment_num = comment_idx + 1
                comment_key = f"{prefix}_comment{comment_num}"
                ja[comment_key] = comment

            # Update description
            desc_key = f"{prefix}_description"
            ja[desc_key] = description

            # Update title
            title_key = f"{prefix}_title"
            ja[title_key] = exercise.get('title', '')

            total_exercises += 1

        print(f"  Lesson {lesson_num}: {len(exercises)} exercises ({source_file})")

    target_data['ja'] = ja

    # Save updated JSON
    with open(target_file, 'w', encoding='utf-8') as f:
        json.dump(target_data, f, ensure_ascii=False, indent=4)

    print(f"  Total: {total_exercises} exercises updated")
    return True

def main():
    # Base paths
    source_base = Path('C:/Work/git/senkou-code/data/lessons')
    target_base = Path('C:/Work/MetaXR/ProgramShooting/Assets/Resources')

    # Language configurations: (language_key, [source_files], target_file)
    languages = [
        ('python', [
            source_base / 'python.ts',
            source_base / 'python2.ts',
            source_base / 'python3.ts',
            source_base / 'python4.ts',
            source_base / 'python5.ts',
        ], target_base / 'pythonLessons.json'),

        ('javascript', [
            source_base / 'javascript.ts',
            source_base / 'javascript2.ts',
            source_base / 'javascript3.ts',
            source_base / 'javascript4.ts',
            source_base / 'javascript5.ts',
        ], target_base / 'javascriptLessons.json'),

        ('typescript', [
            source_base / 'typescript.ts',
            source_base / 'typescript2.ts',
            source_base / 'typescript3.ts',
            source_base / 'typescript4.ts',
        ], target_base / 'typescriptLessons.json'),

        ('java', [
            source_base / 'java.ts',
            source_base / 'java2.ts',
            source_base / 'java3.ts',
            source_base / 'java4.ts',
            source_base / 'java5.ts',
        ], target_base / 'javaLessons.json'),

        ('c', [
            source_base / 'c.ts',
            source_base / 'c2.ts',
            source_base / 'c3.ts',
            source_base / 'c4.ts',
        ], target_base / 'cLessons.json'),

        ('cpp', [
            source_base / 'cpp.ts',
            source_base / 'cpp2.ts',
            source_base / 'cpp3.ts',
            source_base / 'cpp4.ts',
        ], target_base / 'cppLessons.json'),

        ('assembly', [
            source_base / 'assembly.ts',
            source_base / 'assembly2.ts',
            source_base / 'assembly3.ts',
        ], target_base / 'assemblyLessons.json'),
    ]

    for language, source_files, target_file in languages:
        # Filter to only existing source files
        existing_sources = [f for f in source_files if f.exists()]
        if existing_sources:
            update_language_lessons(language, existing_sources, target_file)

    print("\n=== All updates complete ===")

if __name__ == '__main__':
    main()
