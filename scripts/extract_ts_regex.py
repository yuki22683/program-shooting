#!/usr/bin/env python3
"""
Extract Japanese content from TypeScript lesson files using regex-based parsing.
This avoids JavaScript eval issues with special characters.
"""
import json
import re
import sys
import io
from pathlib import Path

sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')

REFERENCE_DIR = Path("C:/Work/git/senkou-code/data/lessons")
OUTPUT_DIR = Path("C:/Work/MetaXR/ProgramShooting")

LANGUAGES = [
    "python", "javascript", "typescript", "java",
    "c", "cpp", "csharp", "assembly",
    "go", "rust", "ruby", "php",
    "swift", "kotlin", "bash", "sql",
    "lua", "perl", "haskell", "elixir"
]

def get_lesson_files(lang):
    """Get all lesson files for a language"""
    files = []
    main_file = REFERENCE_DIR / f"{lang}.ts"
    if main_file.exists():
        files.append((main_file, 1))
    for i in range(2, 10):
        extra_file = REFERENCE_DIR / f"{lang}{i}.ts"
        if extra_file.exists():
            files.append((extra_file, i))
    return files

def extract_string_value(text, key):
    """Extract a string value for a given key from text"""
    # Pattern to match "key": "value" or "key": 'value'
    patterns = [
        rf'"{re.escape(key)}"\s*:\s*"((?:[^"\\]|\\.)*)"',
        rf'"{re.escape(key)}"\s*:\s*\'((?:[^\'\\]|\\.)*)\'',
    ]
    for pattern in patterns:
        match = re.search(pattern, text, re.DOTALL)
        if match:
            value = match.group(1)
            # Unescape common escape sequences
            value = value.replace('\\n', '\n')
            value = value.replace('\\t', '\t')
            value = value.replace('\\"', '"')
            value = value.replace("\\'", "'")
            value = value.replace('\\\\', '\\')
            return value
    return None

def extract_lesson_data(ts_file, lang_prefix, lesson_num):
    """Extract key-value pairs from a TypeScript lesson file"""
    keys = {}

    with open(ts_file, 'r', encoding='utf-8') as f:
        content = f.read()

    # Extract lesson title
    title = extract_string_value(content, "lessonTitle")
    if title:
        keys[f"{lang_prefix}_lesson{lesson_num}_title"] = title

    # Extract lesson description
    desc = extract_string_value(content, "lessonDescription")
    if desc:
        keys[f"{lang_prefix}_lesson{lesson_num}_course_description"] = desc

    # Find all exercises
    # Pattern to find exercise blocks
    exercise_pattern = r'\{"title"\s*:\s*"((?:[^"\\]|\\.)*)"[^}]*?"description"\s*:\s*"((?:[^"\\]|\\.)*)"'

    # Find exercises and their positions
    ex_positions = []
    for match in re.finditer(r'\{"title"\s*:', content):
        ex_positions.append(match.start())

    # Process each exercise
    ex_num = 0
    for i, pos in enumerate(ex_positions):
        # Get the content until next exercise or end
        end_pos = ex_positions[i + 1] if i + 1 < len(ex_positions) else len(content)
        ex_content = content[pos:end_pos]

        # Extract exercise title
        title_match = re.search(r'\{"title"\s*:\s*"((?:[^"\\]|\\.)*)"', ex_content)
        if not title_match:
            continue

        ex_title = title_match.group(1).replace('\\n', '\n')

        # Check if this is actually an exercise (has tutorialSlides or testCases)
        if 'tutorialSlides' not in ex_content and 'testCases' not in ex_content:
            continue

        ex_num += 1
        ex_prefix = f"{lang_prefix}_lesson{lesson_num}_ex{ex_num}"

        keys[f"{ex_prefix}_title"] = ex_title

        # Extract exercise description
        desc_match = re.search(r'"description"\s*:\s*"((?:[^"\\]|\\.)*)"', ex_content)
        if desc_match:
            keys[f"{ex_prefix}_description"] = desc_match.group(1).replace('\\n', '\n')

        # Find tutorial slides
        slide_num = 0
        slide_pattern = r'\{"title"\s*:\s*"((?:[^"\\]|\\.)*)"\s*,\s*"image"\s*:\s*"[^"]*"\s*,\s*"content"\s*:\s*"((?:[^"\\]|\\.)*)"'

        for slide_match in re.finditer(slide_pattern, ex_content, re.DOTALL):
            slide_num += 1
            slide_prefix = f"{ex_prefix}_slide{slide_num}"

            slide_title = slide_match.group(1).replace('\\n', '\n')
            slide_content = slide_match.group(2).replace('\\n', '\n')

            keys[f"{slide_prefix}_title"] = slide_title
            keys[f"{slide_prefix}_content"] = slide_content

        # Extract line hints as comments
        hints_match = re.search(r'"lineHints"\s*:\s*\[(.*?)\]', ex_content, re.DOTALL)
        if hints_match:
            hints_str = hints_match.group(1)
            # Find all string values in hints array
            hint_num = 0
            for hint_match in re.finditer(r'"((?:[^"\\]|\\.)+)"', hints_str):
                hint_value = hint_match.group(1)
                if hint_value and hint_value != 'null':
                    hint_num += 1
                    hint_text = hint_value.replace('\\n', '\n')
                    keys[f"{ex_prefix}_comment{hint_num}"] = hint_text

    return keys

def main():
    all_ref_keys = {}
    stats_by_lang = {}

    print("=" * 70)
    print("参考サイト TypeScript ファイルから日本語データを抽出中 (Regex版)...")
    print("=" * 70)

    for lang in LANGUAGES:
        files = get_lesson_files(lang)
        total_keys = 0

        for ts_file, lesson_num in files:
            try:
                keys = extract_lesson_data(ts_file, lang, lesson_num)
                all_ref_keys.update(keys)
                total_keys += len(keys)
                print(f"  [OK] {ts_file.name}: {len(keys)} keys")
            except Exception as e:
                print(f"  [NG] {ts_file.name}: {e}")

        stats_by_lang[lang] = total_keys

    print(f"\n参考サイト合計キー数: {len(all_ref_keys)}")

    # Save extracted data
    output_path = OUTPUT_DIR / "reference_ja_complete.json"
    with open(output_path, 'w', encoding='utf-8') as f:
        json.dump(all_ref_keys, f, ensure_ascii=False, indent=2)
    print(f"参考データ保存: {output_path}")

    # Print summary by language
    print("\n言語別キー数:")
    for lang, count in stats_by_lang.items():
        status = "[OK]" if count > 0 else "[NG]"
        print(f"  {status} {lang}: {count}")

    success_count = sum(1 for c in stats_by_lang.values() if c > 0)
    print(f"\n抽出成功: {success_count}/{len(stats_by_lang)} 言語")

if __name__ == "__main__":
    main()
