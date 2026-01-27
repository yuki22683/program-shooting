#!/usr/bin/env python3
"""
Apply English translations to lesson files.
Reads *_en_translated.json files and updates the corresponding lesson files.
"""

import json
from pathlib import Path

def apply_translations(lesson_file, translation_file):
    """Apply translations from translation file to lesson file."""
    print(f"Applying translations from {translation_file.name} to {lesson_file.name}")

    # Read lesson file
    with open(lesson_file, 'r', encoding='utf-8') as f:
        data = json.load(f)

    # Read translation file
    with open(translation_file, 'r', encoding='utf-8') as f:
        translations = json.load(f)

    # Apply translations to English section
    en_section = data.get('en', {})
    updated_count = 0

    for key, new_value in translations.items():
        if key in en_section:
            old_value = en_section[key]
            if old_value != new_value:
                en_section[key] = new_value
                updated_count += 1

    data['en'] = en_section

    # Write back
    with open(lesson_file, 'w', encoding='utf-8') as f:
        json.dump(data, f, ensure_ascii=False, indent=2)

    print(f"  Updated {updated_count} keys")
    return updated_count

def main():
    resources_dir = Path(__file__).parent.parent / 'Assets' / 'Resources'

    # Find all translation files
    translation_files = list(resources_dir.glob('*_en_translated.json'))

    if not translation_files:
        print("No translation files found (*_en_translated.json)")
        return

    total_updated = 0

    for trans_file in sorted(translation_files):
        # Get corresponding lesson file
        lesson_name = trans_file.stem.replace('_en_translated', '') + '.json'
        lesson_file = resources_dir / lesson_name

        if lesson_file.exists():
            updated = apply_translations(lesson_file, trans_file)
            total_updated += updated
        else:
            print(f"Warning: Lesson file not found: {lesson_name}")

    print(f"\nTotal: Updated {total_updated} keys across {len(translation_files)} files")

if __name__ == '__main__':
    main()
