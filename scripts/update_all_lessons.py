#!/usr/bin/env python3
"""
Update all lesson JSON files from senkou-code source data
Supports all 20 programming languages
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
    json_str = content[json_start:].rstrip()

    # Remove trailing semicolon and whitespace
    if json_str.endswith(';'):
        json_str = json_str[:-1]

    try:
        return json.loads(json_str)
    except json.JSONDecodeError as e:
        print(f"  Warning: Failed to parse {filepath}: {e}")
        return None

def extract_comments_from_correct_code(correct_code, language):
    """Extract comment lines from correctCode based on language"""
    comments = []
    lines = correct_code.split('\n')
    for line in lines:
        stripped = line.strip()
        # Python-style comments
        if stripped.startswith('#') and language in ['python', 'ruby', 'perl', 'bash']:
            comment_text = stripped[1:].strip()
            comments.append(comment_text)
        # C-style single-line comments
        elif stripped.startswith('//'):
            comment_text = stripped[2:].strip()
            comments.append(comment_text)
        # Assembly-style comments (;)
        elif stripped.startswith(';') and language == 'assembly':
            comment_text = stripped[1:].strip()
            comments.append(comment_text)
        # SQL-style comments (--)
        elif stripped.startswith('--') and language == 'sql':
            comment_text = stripped[2:].strip()
            comments.append(comment_text)
        # Lua-style comments (--)
        elif stripped.startswith('--') and language == 'lua':
            comment_text = stripped[2:].strip()
            comments.append(comment_text)
        # Haskell-style comments (--)
        elif stripped.startswith('--') and language == 'haskell':
            comment_text = stripped[2:].strip()
            comments.append(comment_text)
        # Elixir-style comments (#)
        elif stripped.startswith('#') and language == 'elixir':
            comment_text = stripped[1:].strip()
            comments.append(comment_text)
    return comments

def get_source_files(source_base, language):
    """Get all source files for a language, sorted by lesson number"""
    # Find all files matching the pattern
    files = []

    # First file (e.g., python.ts)
    first_file = source_base / f'{language}.ts'
    if first_file.exists():
        files.append(first_file)

    # Numbered files (e.g., python2.ts, python3.ts, ...)
    for i in range(2, 20):  # Support up to 19 lessons
        numbered_file = source_base / f'{language}{i}.ts'
        if numbered_file.exists():
            files.append(numbered_file)

    return files

def update_language_lessons(language, source_files, target_file):
    """Update lesson JSON for a specific language"""
    print(f"\n=== Updating {language} ===")

    # Load existing target JSON or create new
    if target_file.exists():
        with open(target_file, 'r', encoding='utf-8') as f:
            target_data = json.load(f)
    else:
        target_data = {'en': {}, 'ja': {}}

    ja = target_data.get('ja', {})
    if ja is None:
        ja = {}

    total_exercises = 0

    for lesson_idx, source_file in enumerate(source_files):
        lesson_num = lesson_idx + 1

        if not source_file.exists():
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
            comments = extract_comments_from_correct_code(correct_code, language)

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

        print(f"  Lesson {lesson_num}: {len(exercises)} exercises ({source_file.name})")

    target_data['ja'] = ja

    # Save updated JSON
    with open(target_file, 'w', encoding='utf-8') as f:
        json.dump(target_data, f, ensure_ascii=False, indent=4)

    print(f"  Total: {total_exercises} exercises updated")
    return total_exercises

def main():
    # Base paths
    source_base = Path('C:/Work/git/senkou-code/data/lessons')
    target_base = Path('C:/Work/MetaXR/ProgramShooting/Assets/Resources')

    # All supported languages
    all_languages = [
        'python', 'javascript', 'typescript', 'java', 'c', 'cpp', 'csharp',
        'go', 'rust', 'ruby', 'php', 'swift', 'kotlin', 'bash', 'sql',
        'lua', 'perl', 'haskell', 'elixir', 'assembly'
    ]

    total_all = 0

    for language in all_languages:
        source_files = get_source_files(source_base, language)
        target_file = target_base / f'{language}Lessons.json'

        if source_files:
            count = update_language_lessons(language, source_files, target_file)
            total_all += count

    print(f"\n=== All updates complete ===")
    print(f"Total: {len(all_languages)} languages, {total_all} exercises")

if __name__ == '__main__':
    main()
