#!/usr/bin/env python3
"""
Translate Japanese lesson content to multiple languages using Gemini CLI.
Supports all JSON files in Resources folder: *Lessons.json and localizedText.json
"""

import json
import glob
import os
import subprocess
import sys
import time
import re
import argparse

# Import JSON utilities for sanitization and validation
try:
    from json_utils import sanitize_text_for_json, sanitize_translation_dict, safe_json_dump, validate_json_file
    HAS_JSON_UTILS = True
except ImportError:
    HAS_JSON_UTILS = False
    def sanitize_text_for_json(text):
        if not text:
            return text
        text = text.replace('\u201c', '"').replace('\u201d', '"')
        text = text.replace('\u2018', "'").replace('\u2019', "'")
        return text

# 言語コードと言語名のマッピング
LANGUAGE_NAMES = {
    'en': 'English',
    'ja': 'Japanese',
    'cs': 'Czech',
    'de': 'German',
    'nl': 'Dutch',
    'da': 'Danish',
    'el': 'Greek',
    'fi': 'Finnish',
    'fr': 'French',
    'it': 'Italian',
    'ko': 'Korean',
    'no': 'Norwegian',
    'pl': 'Polish',
    'pt': 'Portuguese',
    'ro': 'Romanian',
    'ru': 'Russian',
    'es': 'Spanish',
    'sv': 'Swedish',
    'tr': 'Turkish',
    'zh-Hans': 'Simplified Chinese',
    'zh-Hant': 'Traditional Chinese',
    'uk': 'Ukrainian',
    'vi': 'Vietnamese',
}

# ソース言語（日本語）以外の全言語
ALL_TARGET_LANGUAGES = [lang for lang in LANGUAGE_NAMES.keys() if lang != 'ja']


def call_gemini(prompt):
    """Call Gemini CLI with the given prompt in non-interactive mode."""
    # 利用したいモデルを指定
    model_name = "gemini-2.5-flash-lite"

    try:
        # Popenを使用して標準入出力を制御
        # Windows環境ではshell=Trueが必要な場合がある
        process = subprocess.Popen(
            f'gemini --model {model_name}',
            stdin=subprocess.PIPE,
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
            text=True,
            encoding='utf-8',
            shell=True
        )

        # inputとしてpromptを流し込み、入力を閉じる(非対話実行)
        # タイムアウトはcommunicateの引数で設定
        stdout, stderr = process.communicate(input=prompt, timeout=300)

        if process.returncode != 0:
            print(f"CLI Error: {stderr}")
            return None

        # ツールが出力する「Loaded cached...」「Ready...」などのログを除去
        clean_output = stdout.strip()

        return clean_output

    except subprocess.TimeoutExpired:
        process.kill()
        print("Error: Gemini CLI timeout")
        return None
    except Exception as e:
        print(f"Error calling Gemini: {e}")
        return None


def translate_batch(texts, target_lang, context="programming tutorial"):
    """Translate a batch of Japanese texts to the target language."""
    if not texts:
        return {}

    target_lang_name = LANGUAGE_NAMES.get(target_lang, target_lang)

    # Create prompt for batch translation
    prompt = f"""Translate the following Japanese programming tutorial texts to {target_lang_name}.
Keep all code blocks (```...```) and inline code (`...`) exactly as they are.
Keep markdown formatting (# headers, bullet points, etc.).
Keep all variable names, function names, and technical terms in their original form.
Return ONLY the translations in the same JSON format.

Context: {context}

Input JSON:
{json.dumps(texts, ensure_ascii=False, indent=2)}

Output the translated JSON only, no explanation:"""

    result = call_gemini(prompt)

    if result:
        # Try to parse JSON from result
        try:
            # Find JSON in the response
            json_match = re.search(r'\{[\s\S]*\}', result)
            if json_match:
                parsed = json.loads(json_match.group())
                # Sanitize all values to prevent JSON corruption
                if HAS_JSON_UTILS:
                    sanitized, issues = sanitize_translation_dict(parsed)
                    if issues:
                        for key, issue in issues:
                            if issue != "sanitized":
                                print(f"    Warning: {key} - {issue}")
                    return sanitized
                else:
                    return {k: sanitize_text_for_json(v) if isinstance(v, str) else v for k, v in parsed.items()}
        except json.JSONDecodeError as e:
            print(f"    Warning: Failed to parse Gemini JSON response: {e}")

    return {}


def translate_single(key, text, target_lang):
    """Translate a single text to the target language."""
    target_lang_name = LANGUAGE_NAMES.get(target_lang, target_lang)

    prompt = f"""Translate this Japanese programming tutorial text to {target_lang_name}.
Keep all code blocks and inline code exactly as they are.
Keep markdown formatting.
Keep all variable names, function names, and technical terms in their original form.
Return ONLY the {target_lang_name} translation, nothing else.

Japanese:
{text}

{target_lang_name}:"""

    result = call_gemini(prompt)
    if result:
        # Sanitize to prevent JSON corruption
        return sanitize_text_for_json(result)
    return text


def has_japanese(text):
    """Check if text contains Japanese characters."""
    return bool(re.search(r'[\u3040-\u309f\u30a0-\u30ff\u4e00-\u9fff]', text))


def get_file_context(filepath):
    """Get translation context based on filename."""
    basename = os.path.basename(filepath)
    if basename == 'localizedText.json':
        return "programming learning app UI and general texts"
    else:
        lang = basename.replace('Lessons.json', '')
        return f"{lang} programming tutorial"


def process_file(filepath, target_lang, batch_size=10, force=False):
    """Process a single JSON file for a specific target language."""
    with open(filepath, 'r', encoding='utf-8') as f:
        data = json.load(f)

    basename = os.path.basename(filepath)
    print(f"\nProcessing {basename} -> {target_lang} ({LANGUAGE_NAMES.get(target_lang, target_lang)})...")

    ja_data = data.get('ja', {})
    target_data = data.get(target_lang, {})

    if not ja_data:
        print(f"  No Japanese data found in {basename}")
        return 0

    if not target_data:
        # ターゲット言語のセクションがない場合は作成
        target_data = {}
        data[target_lang] = target_data

    # Find keys that need translation
    keys_to_translate = []
    for key, ja_value in ja_data.items():
        # Skip image keys
        if key.endswith('_image'):
            continue

        # Skip empty values
        if not ja_value:
            continue

        target_value = target_data.get(key, '')

        if force:
            # 強制モード: 日本語データがあれば翻訳
            keys_to_translate.append(key)
        else:
            # 通常モード: ターゲットが空、または日本語が含まれている場合に翻訳
            if not target_value or has_japanese(target_value):
                keys_to_translate.append(key)

    total = len(keys_to_translate)
    print(f"  Found {total} keys to translate")

    if total == 0:
        return 0

    translated = 0
    context = get_file_context(filepath)

    # Process in batches
    for i in range(0, total, batch_size):
        batch_keys = keys_to_translate[i:i+batch_size]
        batch_texts = {k: ja_data[k] for k in batch_keys}

        print(f"  Translating batch {i//batch_size + 1}/{(total + batch_size - 1)//batch_size}...")

        translations = translate_batch(batch_texts, target_lang, context=context)

        if translations:
            for key, translated_text in translations.items():
                if key in batch_keys and translated_text:
                    target_data[key] = translated_text
                    translated += 1
        else:
            # Fallback to single translation
            for key in batch_keys:
                print(f"    Translating {key}...")
                result = translate_single(key, ja_data[key], target_lang)
                if result:
                    # 翻訳結果が日本語でないことを確認（英語以外のターゲットでは日本語が残っていないかチェック）
                    if target_lang == 'en' and has_japanese(result):
                        print(f"    Warning: Translation still contains Japanese for {key}")
                    else:
                        target_data[key] = result
                        translated += 1
                time.sleep(0.5)  # Rate limiting

        time.sleep(1)  # Rate limiting between batches

    data[target_lang] = target_data

    # Write with validation
    if HAS_JSON_UTILS:
        try:
            safe_json_dump(data, filepath, indent=4)
        except ValueError as e:
            print(f"  ERROR: JSON validation failed: {e}")
            return 0

        # Verify written file
        is_valid, error, _ = validate_json_file(filepath)
        if not is_valid:
            print(f"  WARNING: Written file has JSON errors: {error}")
    else:
        with open(filepath, 'w', encoding='utf-8') as f:
            json.dump(data, f, ensure_ascii=False, indent=4)

    print(f"  Translated {translated}/{total} keys")
    return translated


def get_all_translation_files():
    """Get all translation JSON files in Resources folder."""
    files = []

    # localizedText.json
    localized = 'Assets/Resources/localizedText.json'
    if os.path.exists(localized):
        files.append(localized)

    # All *Lessons.json files
    lesson_files = sorted(glob.glob('Assets/Resources/*Lessons.json'))
    files.extend(lesson_files)

    return files


def main():
    parser = argparse.ArgumentParser(description='Translate lesson content using Gemini CLI')
    parser.add_argument('--lang', '-l',
                        help='Target language code (e.g., en, ko, zh-Hans). Use "all" for all languages.')
    parser.add_argument('--file', '-f',
                        help='Specific file: "localized" for localizedText.json, or lesson prefix (e.g., python)')
    parser.add_argument('--lessons-only', action='store_true',
                        help='Only process *Lessons.json files (exclude localizedText.json)')
    parser.add_argument('--localized-only', action='store_true',
                        help='Only process localizedText.json')
    parser.add_argument('--batch-size', '-b', type=int, default=5,
                        help='Batch size for translation (default: 5)')
    parser.add_argument('--force', action='store_true',
                        help='Force re-translation of all keys')
    parser.add_argument('--list-langs', action='store_true',
                        help='List all supported language codes')
    parser.add_argument('--list-files', action='store_true',
                        help='List all translation files')

    args = parser.parse_args()

    if args.list_langs:
        print("Supported language codes:")
        for code, name in sorted(LANGUAGE_NAMES.items()):
            marker = " (source)" if code == 'ja' else ""
            print(f"  {code:10} - {name}{marker}")
        return

    if args.list_files:
        print("Translation files:")
        for f in get_all_translation_files():
            print(f"  {f}")
        return

    # Determine target languages
    if args.lang:
        if args.lang.lower() == 'all':
            target_langs = ALL_TARGET_LANGUAGES
        else:
            target_langs = [args.lang]
            if args.lang not in LANGUAGE_NAMES:
                print(f"Warning: Unknown language code '{args.lang}'")
    else:
        # デフォルトは英語
        target_langs = ['en']

    # Get files to process
    if args.file:
        if args.file.lower() == 'localized':
            files = ['Assets/Resources/localizedText.json']
        else:
            files = [f'Assets/Resources/{args.file}Lessons.json']
    elif args.localized_only:
        files = ['Assets/Resources/localizedText.json']
    elif args.lessons_only:
        files = sorted(glob.glob('Assets/Resources/*Lessons.json'))
    else:
        # デフォルト: 全ファイル
        files = get_all_translation_files()

    print(f"Files to process: {len(files)}")
    print(f"Target languages: {len(target_langs)}")
    print(f"Batch size: {args.batch_size}")
    print(f"Force mode: {args.force}")

    total = 0
    for target_lang in target_langs:
        if target_lang == 'ja':
            print(f"Skipping 'ja' (source language)")
            continue

        print(f"\n{'='*60}")
        print(f"Target language: {target_lang} ({LANGUAGE_NAMES.get(target_lang, target_lang)})")
        print(f"{'='*60}")

        for f in files:
            if os.path.exists(f):
                count = process_file(f, target_lang, args.batch_size, args.force)
                total += count
            else:
                print(f"File not found: {f}")

    print(f"\n{'='*60}")
    print(f"=== Total: {total} keys translated ===")
    print(f"{'='*60}")


if __name__ == '__main__':
    main()
