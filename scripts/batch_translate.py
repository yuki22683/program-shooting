#!/usr/bin/env python3
"""
Batch translate Japanese lesson content to English using Gemini CLI.
Processes one language at a time, saves progress.
"""

import json
import os
import subprocess
import sys
import re
import time

def has_japanese(text):
    """Check if text contains Japanese characters."""
    if not text:
        return False
    return bool(re.search(r'[\u3040-\u309f\u30a0-\u30ff\u4e00-\u9fff]', text))


def call_gemini(prompt):
    """Call Gemini CLI."""
    try:
        result = subprocess.run(
            ['gemini.cmd', '-p', prompt],
            capture_output=True,
            text=True,
            encoding='utf-8',
            timeout=120
        )
        # Remove "Loaded cached credentials." line
        output = result.stdout.strip()
        lines = output.split('\n')
        if lines and 'cached credentials' in lines[0].lower():
            output = '\n'.join(lines[1:]).strip()
        return output
    except Exception as e:
        print(f"  Error: {e}")
        return None


def translate_text(text, key_type="content"):
    """Translate a single text."""
    if not text or not has_japanese(text):
        return text

    if key_type == "title":
        prompt = f"""Translate this Japanese title to English. Return ONLY the translation:
{text}"""
    else:
        prompt = f"""Translate this Japanese programming tutorial text to English.
Keep all code blocks (``` ```) and inline code (` `) exactly as they are.
Keep markdown formatting (# headers, lists, etc.).
Return ONLY the translation, no explanations:

{text}"""

    result = call_gemini(prompt)

    if result and not has_japanese(result):
        return result
    return text


def process_language(lang):
    """Process a single language file."""
    filepath = f'Assets/Resources/{lang}Lessons.json'

    if not os.path.exists(filepath):
        print(f"File not found: {filepath}")
        return 0

    with open(filepath, 'r', encoding='utf-8') as f:
        data = json.load(f)

    en_data = data.get('en', {})

    # Find keys with Japanese content
    keys_to_translate = []
    for key, value in en_data.items():
        if has_japanese(value) and not key.endswith('_image'):
            keys_to_translate.append(key)

    total = len(keys_to_translate)
    print(f"\n{lang}: {total} keys to translate")

    if total == 0:
        return 0

    translated = 0
    for i, key in enumerate(keys_to_translate):
        print(f"  [{i+1}/{total}] {key[:50]}...", end=" ", flush=True)

        key_type = "title" if '_title' in key else "content"
        result = translate_text(en_data[key], key_type)

        if result != en_data[key]:
            en_data[key] = result
            translated += 1
            print("OK")
        else:
            print("SKIP")

        # Save progress every 10 keys
        if (i + 1) % 10 == 0:
            data['en'] = en_data
            with open(filepath, 'w', encoding='utf-8') as f:
                json.dump(data, f, ensure_ascii=False, indent=4)
            print(f"  [Saved progress: {translated}/{i+1}]")

        time.sleep(0.3)  # Rate limiting

    # Final save
    data['en'] = en_data
    with open(filepath, 'w', encoding='utf-8') as f:
        json.dump(data, f, ensure_ascii=False, indent=4)

    print(f"  Completed: {translated}/{total}")
    return translated


def main():
    if len(sys.argv) > 1:
        langs = sys.argv[1:]
    else:
        # All languages
        files = [os.path.basename(f).replace('Lessons.json', '')
                 for f in sorted(os.listdir('Assets/Resources'))
                 if f.endswith('Lessons.json')]
        langs = files

    total = 0
    for lang in langs:
        count = process_language(lang)
        total += count

    print(f"\n=== Total: {total} keys translated ===")


if __name__ == '__main__':
    main()
