#!/usr/bin/env python3
"""
Extract Japanese content from reference TypeScript files and verify against local JSON files.
"""
import json
import os
import re
import sys
import io
from pathlib import Path

# Fix Windows console encoding
sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')

# Paths
REFERENCE_DIR = Path("C:/Work/git/senkou-code/data/lessons")
LOCAL_DIR = Path("C:/Work/MetaXR/ProgramShooting/Assets/Resources")

# Language to file mapping
LANGUAGES = [
    "python", "javascript", "typescript", "java",
    "c", "cpp", "csharp", "assembly",
    "go", "rust", "ruby", "php",
    "swift", "kotlin", "bash", "sql",
    "lua", "perl", "haskell", "elixir"
]

def extract_ts_object(content):
    """Extract the JavaScript/TypeScript object from export statement"""
    # Find the object content between { and the final }
    match = re.search(r'export const \w+Data = (\{[\s\S]*\});?\s*$', content)
    if match:
        obj_str = match.group(1)
        # Convert to valid JSON
        # Replace single quotes with double quotes (careful with nested quotes)
        # Handle property names without quotes

        # First, fix the trailing issues
        obj_str = obj_str.rstrip().rstrip(';')

        return obj_str
    return None

def parse_ts_to_dict(ts_content):
    """Parse TypeScript object to Python dict"""
    obj_str = extract_ts_object(ts_content)
    if not obj_str:
        return None

    # Convert TS object literal to JSON
    # 1. Add quotes to unquoted keys
    # 2. Handle arrays properly
    # 3. Handle nested objects

    try:
        # Use a more robust approach - evaluate as JavaScript-like syntax
        # First, try simple JSON5-like parsing

        # Replace: key: value with "key": value
        def fix_keys(match):
            key = match.group(1)
            return f'"{key}":'

        # Fix unquoted keys
        result = re.sub(r'(?<=[{,\s])(\w+)(?=\s*:)', fix_keys, obj_str)

        # Replace single quotes with double quotes (but not in already double-quoted strings)
        # This is tricky - let's handle it carefully
        in_double_quote = False
        in_single_quote = False
        chars = list(result)
        i = 0
        while i < len(chars):
            c = chars[i]
            if c == '\\' and i + 1 < len(chars):
                i += 2
                continue
            if c == '"' and not in_single_quote:
                in_double_quote = not in_double_quote
            elif c == "'" and not in_double_quote:
                in_single_quote = not in_single_quote
                chars[i] = '"'
            i += 1

        result = ''.join(chars)

        # Remove trailing commas
        result = re.sub(r',(\s*[}\]])', r'\1', result)

        return json.loads(result)
    except json.JSONDecodeError as e:
        print(f"JSON parse error: {e}")
        return None

def extract_keys_from_lesson(lang_prefix, lesson_data, lesson_num):
    """Extract key-value pairs from a lesson data structure"""
    keys = {}

    # Lesson title and description
    if 'lessonTitle' in lesson_data:
        keys[f'{lang_prefix}_lesson{lesson_num}_title'] = lesson_data['lessonTitle']

    if 'lessonDescription' in lesson_data:
        keys[f'{lang_prefix}_lesson{lesson_num}_course_description'] = lesson_data['lessonDescription']

    # Exercises
    if 'exercises' in lesson_data:
        for ex_idx, exercise in enumerate(lesson_data['exercises'], 1):
            ex_prefix = f'{lang_prefix}_lesson{lesson_num}_ex{ex_idx}'

            if 'title' in exercise:
                keys[f'{ex_prefix}_title'] = exercise['title']

            if 'description' in exercise:
                keys[f'{ex_prefix}_description'] = exercise['description']

            # Tutorial slides
            if 'tutorialSlides' in exercise:
                for slide_idx, slide in enumerate(exercise['tutorialSlides'], 1):
                    slide_prefix = f'{ex_prefix}_slide{slide_idx}'

                    if 'title' in slide:
                        keys[f'{slide_prefix}_title'] = slide['title']

                    if 'content' in slide:
                        keys[f'{slide_prefix}_content'] = slide['content']

            # Comments/hints
            if 'lineHints' in exercise:
                for hint_idx, hint in enumerate(exercise['lineHints'], 1):
                    if hint:
                        keys[f'{ex_prefix}_comment{hint_idx}'] = hint

    return keys

def get_lesson_files(lang):
    """Get all lesson files for a language (e.g., python.ts, python2.ts, python3.ts)"""
    files = []
    # Main file (lesson 1)
    main_file = REFERENCE_DIR / f"{lang}.ts"
    if main_file.exists():
        files.append((main_file, 1))

    # Additional lessons (2, 3, 4, 5...)
    for i in range(2, 10):
        extra_file = REFERENCE_DIR / f"{lang}{i}.ts"
        if extra_file.exists():
            files.append((extra_file, i))

    return files

def main():
    all_reference_keys = {}

    print("=" * 70)
    print("参考サイト TypeScript ファイルから日本語データを抽出中...")
    print("=" * 70)

    # Extract data from all TypeScript files
    for lang in LANGUAGES:
        lang_prefix = lang
        lesson_files = get_lesson_files(lang)

        for ts_file, lesson_num in lesson_files:
            with open(ts_file, 'r', encoding='utf-8') as f:
                content = f.read()

            data = parse_ts_to_dict(content)
            if data:
                keys = extract_keys_from_lesson(lang_prefix, data, lesson_num)
                all_reference_keys.update(keys)
                print(f"  {ts_file.name}: {len(keys)} keys")

    print(f"\n参考サイト合計キー数: {len(all_reference_keys)}")

    # Save extracted reference data
    ref_output = LOCAL_DIR.parent.parent / "reference_ja_extracted.json"
    with open(ref_output, 'w', encoding='utf-8') as f:
        json.dump(all_reference_keys, f, ensure_ascii=False, indent=2)
    print(f"参考データ保存: {ref_output}")

    # Now verify each local file
    print("\n" + "=" * 70)
    print("ローカルファイル照合中...")
    print("=" * 70)

    results = {}
    checklist = []

    for lang in LANGUAGES:
        # Get all reference keys for this language
        prefix = f"{lang}_"
        ref_lang_keys = {k: v for k, v in all_reference_keys.items() if k.startswith(prefix)}

        # Load local file
        local_filename = f"{lang}Lessons_ja.json"
        local_path = LOCAL_DIR / local_filename

        if not local_path.exists():
            results[local_filename] = {"status": "MISSING"}
            checklist.append(f"[X] {local_filename} - ファイルが存在しない")
            continue

        with open(local_path, 'r', encoding='utf-8') as f:
            local_data = json.load(f)

        # Compare
        missing = []
        mismatch = []
        extra = []

        for key, ref_value in ref_lang_keys.items():
            if key not in local_data:
                missing.append(key)
            elif local_data[key].strip() != ref_value.strip():
                # Normalize whitespace for comparison
                local_norm = re.sub(r'\s+', ' ', local_data[key].strip())
                ref_norm = re.sub(r'\s+', ' ', ref_value.strip())
                if local_norm != ref_norm:
                    mismatch.append({
                        "key": key,
                        "local": local_data[key][:80],
                        "ref": ref_value[:80]
                    })

        for key in local_data:
            if key not in ref_lang_keys:
                extra.append(key)

        # Determine status
        if missing or mismatch:
            status = "MISMATCH"
            icon = "[X]"
        elif extra:
            status = "EXTRA"
            icon = "[?]"
        else:
            status = "OK"
            icon = "[O]"

        results[local_filename] = {
            "status": status,
            "ref_count": len(ref_lang_keys),
            "local_count": len(local_data),
            "missing": missing,
            "mismatch": mismatch,
            "extra": extra
        }

        msg = f"{icon} {local_filename} - Ref:{len(ref_lang_keys)} Local:{len(local_data)}"
        if missing:
            msg += f" 欠損:{len(missing)}"
        if mismatch:
            msg += f" 不一致:{len(mismatch)}"
        if extra:
            msg += f" 余分:{len(extra)}"
        checklist.append(msg)

    # Print checklist
    print("\nチェックリスト:")
    print("-" * 60)
    for line in checklist:
        print(line)

    # Print details for issues
    print("\n" + "=" * 70)
    print("詳細情報")
    print("=" * 70)

    for filename, result in results.items():
        if result.get("status") in ["MISMATCH", "EXTRA"]:
            print(f"\n【{filename}】")

            if result.get("missing"):
                print(f"  欠損キー ({len(result['missing'])}):")
                for key in result["missing"][:5]:
                    print(f"    - {key}")
                if len(result["missing"]) > 5:
                    print(f"    ... 他 {len(result['missing']) - 5} 件")

            if result.get("mismatch"):
                print(f"  値の不一致 ({len(result['mismatch'])}):")
                for item in result["mismatch"][:3]:
                    print(f"    - {item['key']}")
                    print(f"      Local: {item['local']}...")
                    print(f"      Ref:   {item['ref']}...")
                if len(result["mismatch"]) > 3:
                    print(f"    ... 他 {len(result['mismatch']) - 3} 件")

            if result.get("extra"):
                print(f"  ローカルのみ ({len(result['extra'])}):")
                for key in result["extra"][:5]:
                    print(f"    - {key}")
                if len(result["extra"]) > 5:
                    print(f"    ... 他 {len(result['extra']) - 5} 件")

    # Summary
    ok_count = sum(1 for r in results.values() if r.get("status") == "OK")
    mismatch_count = sum(1 for r in results.values() if r.get("status") == "MISMATCH")
    extra_count = sum(1 for r in results.values() if r.get("status") == "EXTRA")
    missing_count = sum(1 for r in results.values() if r.get("status") == "MISSING")

    print("\n" + "=" * 70)
    print("サマリー")
    print("=" * 70)
    print(f"[O] 完全一致: {ok_count}/20")
    print(f"[X] 不一致/欠損あり: {mismatch_count}/20")
    print(f"[?] ローカルに余分なキー: {extra_count}/20")
    print(f"[-] ファイル欠損: {missing_count}/20")

    # Save detailed report
    report_path = LOCAL_DIR.parent.parent / "ja_verification_detailed.json"
    with open(report_path, 'w', encoding='utf-8') as f:
        json.dump(results, f, ensure_ascii=False, indent=2)
    print(f"\n詳細レポート保存: {report_path}")

if __name__ == "__main__":
    main()
