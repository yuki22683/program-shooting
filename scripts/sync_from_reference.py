#!/usr/bin/env python3
"""
Sync Lesson Data from Reference Site (senkou-code)

This script:
1. Reads TypeScript lesson files from senkou-code repository
2. Extracts Japanese content and generates localization keys
3. Updates the project's lesson JSON files
4. Validates the result

Usage:
    python sync_from_reference.py [--language python] [--dry-run]
"""

import json
import re
import argparse
from pathlib import Path

SENKOU_CODE_PATH = Path(r"C:\Work\git\senkou-code\data\lessons")
PROJECT_RESOURCES_PATH = Path(r"C:\Work\MetaXR\ProgramShooting\Assets\Resources")

LANGUAGES = [
    "python", "javascript", "typescript", "java", "c", "cpp", "csharp",
    "go", "rust", "ruby", "php", "swift", "kotlin", "bash", "sql",
    "lua", "perl", "haskell", "elixir", "assembly"
]

def sanitize_json_string(json_str):
    """Sanitize JSON string to handle special characters from TypeScript files."""
    # Valid JSON escape characters: " \ / b f n r t u
    # Any backslash followed by other characters needs to be escaped

    # Handle actual tab characters inside strings -> \t
    def replace_tabs_in_strings(m):
        return m.group(0).replace('\t', '\\t')
    json_str = re.sub(r'"[^"\\]*(?:\\.[^"\\]*)*"', replace_tabs_in_strings, json_str)

    # Escape backslashes before invalid escape characters
    # Match backslash NOT followed by valid escape chars: " \ / b f n r t u
    # We need to be careful not to double-escape already valid sequences
    def fix_invalid_escapes(m):
        s = m.group(0)
        result = []
        i = 0
        while i < len(s):
            if s[i] == '\\' and i + 1 < len(s):
                next_char = s[i + 1]
                # Valid JSON escapes
                if next_char in '"\\\/bfnrtu':
                    result.append(s[i:i+2])
                    i += 2
                else:
                    # Invalid escape - add extra backslash
                    result.append('\\\\')
                    result.append(next_char)
                    i += 2
            else:
                result.append(s[i])
                i += 1
        return ''.join(result)

    # Apply fix to all string values
    json_str = re.sub(r'"[^"]*(?:\\.[^"]*)*"', fix_invalid_escapes, json_str)

    return json_str


def parse_ts_file(file_path):
    """Parse TypeScript file and extract JSON data."""
    if not file_path.exists():
        return None

    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()

    match = re.search(r'export\s+const\s+\w+\s*=\s*(\{[\s\S]*\})\s*;?\s*$', content)
    if match:
        json_str = match.group(1)
        json_str = re.sub(r',(\s*[}\]])', r'\1', json_str)
        # Sanitize special characters
        json_str = sanitize_json_string(json_str)
        try:
            return json.loads(json_str)
        except json.JSONDecodeError as e:
            print(f"  Warning: JSON parse error in {file_path.name}: {e}")
    return None

def load_all_ts_files_for_language(language):
    """Load all TypeScript lesson files for a language."""
    all_data = []

    files_to_check = [f"{language}.ts"]
    for i in range(2, 10):
        files_to_check.append(f"{language}{i}.ts")

    for filename in files_to_check:
        file_path = SENKOU_CODE_PATH / filename
        if file_path.exists():
            data = parse_ts_file(file_path)
            if data:
                all_data.append(data)

    return all_data

def generate_lesson_keys(ref_data, language):
    """Generate localization keys from reference data."""
    ja_keys = {}
    common_keys = {}

    for lesson in ref_data:
        lesson_id = lesson.get('lessonId', '')
        try:
            lesson_num = int(lesson_id.split('-')[-1])
        except (ValueError, IndexError):
            lesson_num = 1

        prefix = f"{language}_lesson{lesson_num}"

        # Lesson-level keys
        ja_keys[f"{prefix}_title"] = lesson.get('lessonTitle', '')
        ja_keys[f"{prefix}_course_title"] = lesson.get('lessonTitle', '')
        ja_keys[f"{prefix}_course_description"] = lesson.get('lessonDescription', '')

        # Exercise keys
        for ex in lesson.get('exercises', []):
            ex_num = ex.get('orderIndex', 1)
            ex_prefix = f"{prefix}_ex{ex_num}"

            ja_keys[f"{ex_prefix}_title"] = ex.get('title', '')
            ja_keys[f"{ex_prefix}_description"] = ex.get('description', '')

            # Slide keys
            for slide_idx, slide in enumerate(ex.get('tutorialSlides', []), 1):
                slide_prefix = f"{ex_prefix}_slide{slide_idx}"
                ja_keys[f"{slide_prefix}_title"] = slide.get('title', '')
                ja_keys[f"{slide_prefix}_content"] = slide.get('content', '')

                # Image paths go to common
                if slide.get('image'):
                    common_keys[f"{slide_prefix}_image"] = slide.get('image')

            # Add "Let's try" slide at the end
            last_slide_num = len(ex.get('tutorialSlides', [])) + 1
            ja_keys[f"{ex_prefix}_slide{last_slide_num}_title"] = "やってみよう！"
            ja_keys[f"{ex_prefix}_slide{last_slide_num}_content"] = f"{ex.get('description', '')}\n\n（準備ができたら「レッスン開始」を選択してください。）"

            # Line hints as comments
            for hint_idx, hint in enumerate(ex.get('lineHints', []), 1):
                if hint:
                    ja_keys[f"{ex_prefix}_comment{hint_idx}"] = hint

    return ja_keys, common_keys

def sync_language(language, dry_run=False):
    """Sync a single language from reference site."""
    print(f"\n{'='*50}")
    print(f"Syncing: {language.upper()}")
    print(f"{'='*50}")

    # Load reference data
    ref_data = load_all_ts_files_for_language(language)
    if not ref_data:
        print(f"  ERROR: No reference data found for {language}")
        return False

    total_exercises = sum(len(l.get('exercises', [])) for l in ref_data)
    print(f"  Found {len(ref_data)} lessons, {total_exercises} exercises")

    # Generate keys
    ja_keys, common_keys = generate_lesson_keys(ref_data, language)
    print(f"  Generated {len(ja_keys)} JA keys, {len(common_keys)} common keys")

    if dry_run:
        print("  [DRY RUN] Would update files:")
        print(f"    - {language}Lessons_ja.json")
        print(f"    - {language}Lessons_common.json")
        return True

    # Load existing files
    ja_file = PROJECT_RESOURCES_PATH / f"{language}Lessons_ja.json"
    common_file = PROJECT_RESOURCES_PATH / f"{language}Lessons_common.json"

    existing_ja = {}
    existing_common = {}

    if ja_file.exists():
        with open(ja_file, 'r', encoding='utf-8') as f:
            existing_ja = json.load(f)

    if common_file.exists():
        with open(common_file, 'r', encoding='utf-8') as f:
            existing_common = json.load(f)

    # Merge (new data overwrites existing)
    updated_ja = {**existing_ja, **ja_keys}
    updated_common = {**existing_common, **common_keys}

    # Sort keys
    updated_ja = dict(sorted(updated_ja.items()))
    updated_common = dict(sorted(updated_common.items()))

    # Write files
    with open(ja_file, 'w', encoding='utf-8') as f:
        json.dump(updated_ja, f, ensure_ascii=False, indent=2)

    with open(common_file, 'w', encoding='utf-8') as f:
        json.dump(updated_common, f, ensure_ascii=False, indent=2)

    print(f"  Updated {ja_file.name} ({len(updated_ja)} keys)")
    print(f"  Updated {common_file.name} ({len(updated_common)} keys)")

    return True

def main():
    parser = argparse.ArgumentParser(description='Sync lesson data from reference site')
    parser.add_argument('--language', '-l', help='Specific language to sync (default: all)')
    parser.add_argument('--dry-run', '-n', action='store_true', help='Show what would be done')
    args = parser.parse_args()

    print("Lesson Data Sync from senkou-code")
    print(f"Reference path: {SENKOU_CODE_PATH}")
    print(f"Project path: {PROJECT_RESOURCES_PATH}")

    if args.dry_run:
        print("\n*** DRY RUN MODE ***\n")

    languages_to_sync = [args.language] if args.language else LANGUAGES
    success_count = 0

    for language in languages_to_sync:
        if language not in LANGUAGES:
            print(f"Unknown language: {language}")
            continue
        if sync_language(language, args.dry_run):
            success_count += 1

    print(f"\n{'='*50}")
    print(f"Sync complete: {success_count}/{len(languages_to_sync)} languages")
    print(f"{'='*50}")

    # Reminder to validate
    print("\nIMPORTANT: Run validation script to verify data integrity:")
    print("  python scripts/validate_lesson_data.py")

if __name__ == "__main__":
    main()
