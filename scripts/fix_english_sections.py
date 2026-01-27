#!/usr/bin/env python3
"""
Extract and fix Japanese text in English sections of lesson files.
Uses Claude to translate Japanese to English.
"""

import json
import re
import os
from pathlib import Path
from collections import defaultdict

# Character ranges for language detection
JAPANESE_PATTERN = re.compile(r'[\u3040-\u309F\u30A0-\u30FF\u4E00-\u9FAF]')

def has_japanese(text):
    """Check if text contains Japanese characters."""
    if not text or not isinstance(text, str):
        return False
    return bool(JAPANESE_PATTERN.search(text))

def extract_japanese_keys(filepath):
    """Extract keys from English section that contain Japanese."""
    with open(filepath, 'r', encoding='utf-8') as f:
        data = json.load(f)

    en_section = data.get('en', {})
    japanese_keys = {}

    for key, value in en_section.items():
        if isinstance(value, str) and has_japanese(value):
            japanese_keys[key] = value

    return japanese_keys

def apply_translations(filepath, translations):
    """Apply translations to the English section."""
    with open(filepath, 'r', encoding='utf-8') as f:
        data = json.load(f)

    en_section = data.get('en', {})

    for key, new_value in translations.items():
        if key in en_section:
            en_section[key] = new_value

    data['en'] = en_section

    with open(filepath, 'w', encoding='utf-8') as f:
        json.dump(data, f, ensure_ascii=False, indent=2)

    print(f"Updated {len(translations)} keys in {filepath}")

def main():
    resources_dir = Path(__file__).parent.parent / 'Assets' / 'Resources'

    # Process all lesson files
    lesson_files = sorted(resources_dir.glob('*Lessons.json'))

    for filepath in lesson_files:
        print(f"\n{'='*60}")
        print(f"File: {filepath.name}")
        print('='*60)

        japanese_keys = extract_japanese_keys(filepath)

        if not japanese_keys:
            print("No Japanese text found in English section.")
            continue

        print(f"Found {len(japanese_keys)} keys with Japanese text:")

        # Output for translation
        output_file = filepath.parent / f"{filepath.stem}_en_fixes.json"
        with open(output_file, 'w', encoding='utf-8') as f:
            json.dump(japanese_keys, f, ensure_ascii=False, indent=2)

        print(f"Exported to: {output_file}")
        print("\nSample keys:")
        for i, (key, value) in enumerate(list(japanese_keys.items())[:5]):
            print(f"  {key}: {value[:80]}...")

if __name__ == '__main__':
    main()
