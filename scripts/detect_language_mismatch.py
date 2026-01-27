#!/usr/bin/env python3
"""
Detect language mismatches in lesson JSON files.
Identifies values that don't match their language section.
"""

import json
import re
import os
from pathlib import Path
from collections import defaultdict

# Character ranges for language detection
JAPANESE_PATTERN = re.compile(r'[\u3040-\u309F\u30A0-\u30FF\u4E00-\u9FAF]')
KOREAN_PATTERN = re.compile(r'[\uAC00-\uD7AF\u1100-\u11FF]')
CHINESE_PATTERN = re.compile(r'[\u4E00-\u9FFF]')
CYRILLIC_PATTERN = re.compile(r'[\u0400-\u04FF]')
ARABIC_PATTERN = re.compile(r'[\u0600-\u06FF]')
THAI_PATTERN = re.compile(r'[\u0E00-\u0E7F]')
GREEK_PATTERN = re.compile(r'[\u0370-\u03FF]')
HEBREW_PATTERN = re.compile(r'[\u0590-\u05FF]')
LATIN_EXTENDED_PATTERN = re.compile(r'[\u00C0-\u024F]')  # Accented Latin chars

def detect_language(text):
    """Detect the primary language of a text."""
    if not text or len(text.strip()) == 0:
        return 'empty'

    # Skip if mostly code
    code_indicators = ['std::', '#include', 'int ', 'void ', 'return ', '();', '{}', '[]']
    if any(ind in text for ind in code_indicators):
        # Still check for non-ASCII in non-code parts
        pass

    ja_count = len(JAPANESE_PATTERN.findall(text))
    ko_count = len(KOREAN_PATTERN.findall(text))
    cyrillic_count = len(CYRILLIC_PATTERN.findall(text))
    thai_count = len(THAI_PATTERN.findall(text))
    greek_count = len(GREEK_PATTERN.findall(text))
    latin_ext_count = len(LATIN_EXTENDED_PATTERN.findall(text))

    total_chars = len(text)

    # Determine primary language based on character counts
    if ja_count > 5 or (ja_count > 0 and ja_count / total_chars > 0.1):
        return 'ja'
    if ko_count > 5 or (ko_count > 0 and ko_count / total_chars > 0.1):
        return 'ko'
    if cyrillic_count > 5 or (cyrillic_count > 0 and cyrillic_count / total_chars > 0.1):
        return 'ru'  # Could be other Cyrillic languages
    if thai_count > 3:
        return 'th'
    if greek_count > 3:
        return 'el'

    # If only ASCII, assume English
    return 'en'

def check_language_match(lang_code, detected_lang):
    """Check if detected language matches expected language code."""
    # English section should not contain Japanese
    if lang_code == 'en' and detected_lang == 'ja':
        return False

    # Japanese section should contain Japanese
    if lang_code == 'ja' and detected_lang == 'en':
        return False

    # Korean section should contain Korean
    if lang_code == 'ko' and detected_lang != 'ko' and detected_lang != 'en':
        # Allow English for now since many are untranslated
        pass

    # For other languages, if they contain Japanese, it's wrong
    if lang_code not in ['ja', 'zh-CN', 'zh-TW'] and detected_lang == 'ja':
        return False

    return True

def analyze_file(filepath):
    """Analyze a single lesson file for language mismatches."""
    results = {
        'file': filepath,
        'mismatches': defaultdict(list),
        'untranslated': defaultdict(int),
        'summary': {}
    }

    with open(filepath, 'r', encoding='utf-8') as f:
        data = json.load(f)

    for lang_code, translations in data.items():
        if lang_code == 'common':
            continue
        if not isinstance(translations, dict):
            continue

        total_keys = 0
        english_count = 0
        mismatch_count = 0

        for key, value in translations.items():
            if not isinstance(value, str):
                continue

            total_keys += 1
            detected = detect_language(value)

            # Check for Japanese in English section
            if lang_code == 'en' and detected == 'ja':
                results['mismatches']['en'].append({
                    'key': key,
                    'value': value[:100] + '...' if len(value) > 100 else value,
                    'detected': detected
                })
                mismatch_count += 1

            # Check if non-English sections are actually in English
            elif lang_code not in ['en'] and detected == 'en':
                english_count += 1

        if english_count > 0 and lang_code != 'en':
            results['untranslated'][lang_code] = english_count

        results['summary'][lang_code] = {
            'total': total_keys,
            'untranslated': english_count if lang_code != 'en' else 0,
            'mismatched': mismatch_count
        }

    return results

def main():
    resources_dir = Path(__file__).parent.parent / 'Assets' / 'Resources'
    lesson_files = list(resources_dir.glob('*Lessons.json'))

    print("=" * 80)
    print("Language Mismatch Detection Report")
    print("=" * 80)

    all_results = []

    for filepath in sorted(lesson_files):
        print(f"\nAnalyzing: {filepath.name}")
        print("-" * 40)

        results = analyze_file(filepath)
        all_results.append(results)

        # Print mismatches in English section
        if results['mismatches']['en']:
            print(f"\n  [!] English section contains Japanese ({len(results['mismatches']['en'])} keys):")
            for m in results['mismatches']['en'][:5]:  # Show first 5
                print(f"      - {m['key']}: {m['value'][:60]}...")
            if len(results['mismatches']['en']) > 5:
                print(f"      ... and {len(results['mismatches']['en']) - 5} more")

        # Print untranslated languages
        untranslated = [(k, v) for k, v in results['untranslated'].items() if v > 0]
        if untranslated:
            print(f"\n  [!] Untranslated sections (still in English):")
            for lang, count in sorted(untranslated, key=lambda x: -x[1]):
                total = results['summary'][lang]['total']
                pct = (count / total * 100) if total > 0 else 0
                print(f"      - {lang}: {count}/{total} keys ({pct:.1f}%)")

    # Summary
    print("\n" + "=" * 80)
    print("SUMMARY")
    print("=" * 80)

    total_en_mismatches = sum(len(r['mismatches']['en']) for r in all_results)
    print(f"\nTotal English keys with Japanese text: {total_en_mismatches}")

    print("\nUntranslated sections by language:")
    lang_totals = defaultdict(int)
    for r in all_results:
        for lang, count in r['untranslated'].items():
            lang_totals[lang] += count

    for lang, count in sorted(lang_totals.items(), key=lambda x: -x[1]):
        print(f"  {lang}: {count} keys need translation")

if __name__ == '__main__':
    main()
