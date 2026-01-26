#!/usr/bin/env python3
"""
Update pythonLessons.json from senkou-code source data
"""
import json
import re
import os

def parse_ts_file(filepath):
    """Parse TypeScript file and extract JSON data"""
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()

    # Extract the JSON part (after 'export const XXXData = ')
    json_start = content.find('{')
    json_str = content[json_start:]

    return json.loads(json_str)

def extract_comments_from_correct_code(correct_code):
    """Extract comment lines from correctCode"""
    comments = []
    lines = correct_code.split('\n')
    for line in lines:
        stripped = line.strip()
        if stripped.startswith('#'):
            # Remove the # and leading space
            comment_text = stripped[1:].strip()
            comments.append(comment_text)
    return comments

def main():
    # Paths
    source_file = 'C:/Work/git/senkou-code/data/lessons/python.ts'
    target_file = 'C:/Work/MetaXR/ProgramShooting/Assets/Resources/pythonLessons.json'

    # Parse source data
    source_data = parse_ts_file(source_file)

    # Load existing target JSON
    with open(target_file, 'r', encoding='utf-8') as f:
        target_data = json.load(f)

    # Process each exercise
    exercises = source_data['exercises']

    for ex_idx, exercise in enumerate(exercises):
        ex_num = ex_idx + 1
        prefix = f"python_lesson1_ex{ex_num}"

        # Get slides
        slides = exercise.get('tutorialSlides', [])
        description = exercise.get('description', '')

        # Update Japanese entries
        ja = target_data.get('ja', {})

        # Update slide data
        for slide_idx, slide in enumerate(slides):
            slide_num = slide_idx + 1

            # Title
            title_key = f"{prefix}_slide{slide_num}_title"
            ja[title_key] = slide['title']

            # Content
            content_key = f"{prefix}_slide{slide_num}_content"
            ja[content_key] = slide['content']

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
        ja[title_key] = exercise['title']

        target_data['ja'] = ja

        print(f"Updated Ex{ex_num}: {exercise['title']} ({len(slides)} slides + 1 try-it slide)")

    # Also update English entries from the same source (for consistency)
    # For now, we keep English as-is since source is Japanese

    # Save updated JSON
    with open(target_file, 'w', encoding='utf-8') as f:
        json.dump(target_data, f, ensure_ascii=False, indent=4)

    print(f"\nUpdated {target_file}")

if __name__ == '__main__':
    main()
