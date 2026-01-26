#!/usr/bin/env python3
"""
Translate Japanese lesson content to English using Gemini CLI.
"""

import json
import glob
import os
import subprocess
import sys
import time
import re

def call_gemini(prompt):
    """Call Gemini CLI with the given prompt."""
    try:
        result = subprocess.run(
            ['gemini.cmd', '-p', prompt],
            capture_output=True,
            text=True,
            encoding='utf-8',
            timeout=60
        )
        return result.stdout.strip()
    except subprocess.TimeoutExpired:
        return None
    except Exception as e:
        print(f"Error calling Gemini: {e}")
        return None


def translate_batch(texts, context="programming tutorial"):
    """Translate a batch of Japanese texts to English."""
    if not texts:
        return {}

    # Create prompt for batch translation
    prompt = f"""Translate the following Japanese programming tutorial texts to English.
Keep all code blocks (```...```) and inline code (`...`) exactly as they are.
Keep markdown formatting (# headers, bullet points, etc.).
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
                return json.loads(json_match.group())
        except json.JSONDecodeError:
            pass

    return {}


def translate_single(key, text):
    """Translate a single text."""
    prompt = f"""Translate this Japanese programming tutorial text to English.
Keep all code blocks and inline code exactly as they are.
Keep markdown formatting.
Return ONLY the English translation, nothing else.

Japanese:
{text}

English:"""

    result = call_gemini(prompt)
    return result if result else text


def process_file(filepath, batch_size=10):
    """Process a single lessons JSON file."""
    with open(filepath, 'r', encoding='utf-8') as f:
        data = json.load(f)

    lang = os.path.basename(filepath).replace('Lessons.json', '')
    print(f"\nProcessing {lang}...")

    en_data = data.get('en', {})
    ja_data = data.get('ja', {})

    # Find keys that need translation (Japanese content in English section)
    keys_to_translate = []
    for key, en_value in en_data.items():
        ja_value = ja_data.get(key, '')
        # Check if English value contains Japanese characters
        if en_value and re.search(r'[\u3040-\u309f\u30a0-\u30ff\u4e00-\u9fff]', en_value):
            # Skip image keys
            if not key.endswith('_image'):
                keys_to_translate.append(key)

    total = len(keys_to_translate)
    print(f"  Found {total} keys to translate")

    if total == 0:
        return 0

    translated = 0

    # Process in batches
    for i in range(0, total, batch_size):
        batch_keys = keys_to_translate[i:i+batch_size]
        batch_texts = {k: en_data[k] for k in batch_keys}

        print(f"  Translating batch {i//batch_size + 1}/{(total + batch_size - 1)//batch_size}...")

        translations = translate_batch(batch_texts, context=f"{lang} programming tutorial")

        if translations:
            for key, translated_text in translations.items():
                if key in en_data and translated_text:
                    en_data[key] = translated_text
                    translated += 1
        else:
            # Fallback to single translation
            for key in batch_keys:
                print(f"    Translating {key}...")
                result = translate_single(key, en_data[key])
                if result and not re.search(r'[\u3040-\u309f\u30a0-\u30ff\u4e00-\u9fff]', result):
                    en_data[key] = result
                    translated += 1
                time.sleep(0.5)  # Rate limiting

        time.sleep(1)  # Rate limiting between batches

    data['en'] = en_data

    with open(filepath, 'w', encoding='utf-8') as f:
        json.dump(data, f, ensure_ascii=False, indent=4)

    print(f"  Translated {translated}/{total} keys")
    return translated


def main():
    # Get specific file or all files
    if len(sys.argv) > 1:
        files = [f'Assets/Resources/{sys.argv[1]}Lessons.json']
    else:
        files = sorted(glob.glob('Assets/Resources/*Lessons.json'))

    total = 0
    for f in files:
        if os.path.exists(f):
            count = process_file(f)
            total += count
        else:
            print(f"File not found: {f}")

    print(f"\n=== Total: {total} keys translated ===")


if __name__ == '__main__':
    main()
