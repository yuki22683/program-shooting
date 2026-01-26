#!/usr/bin/env python3
"""
Check all JSON localization files for missing translations.
Reports which keys are missing in which language codes.
"""

import json
import os
from pathlib import Path
from collections import defaultdict

# Expected language codes based on pythonLessons.json
EXPECTED_LANGUAGES = [
    "en", "ja", "cs", "de", "nl", "da", "el", "fi", "fr", "it",
    "ko", "no", "pl", "pt", "ro", "ru", "es", "sv", "tr"
]

# Base language to use as reference (English)
BASE_LANG = "en"

def check_json_file(filepath):
    """Check a single JSON file for missing translations."""
    print(f"\n{'='*60}")
    print(f"Checking: {filepath.name}")
    print(f"{'='*60}")

    try:
        with open(filepath, 'r', encoding='utf-8') as f:
            data = json.load(f)
    except json.JSONDecodeError as e:
        print(f"  ERROR: Invalid JSON - {e}")
        return None
    except FileNotFoundError:
        print(f"  ERROR: File not found")
        return None

    # Get all language codes in the file
    file_languages = [k for k in data.keys() if isinstance(data.get(k), dict)]
    print(f"  Languages found: {len(file_languages)}")

    if BASE_LANG not in data:
        print(f"  WARNING: Base language '{BASE_LANG}' not found!")
        if "ja" in data:
            print(f"  Using 'ja' as base instead")
            base_lang = "ja"
        else:
            return None
    else:
        base_lang = BASE_LANG

    base_keys = set(data[base_lang].keys())
    print(f"  Total keys in '{base_lang}': {len(base_keys)}")

    missing_by_lang = {}

    for lang in file_languages:
        if lang == base_lang:
            continue

        lang_keys = set(data[lang].keys())
        missing = base_keys - lang_keys

        if missing:
            missing_by_lang[lang] = sorted(list(missing))
            print(f"  {lang}: Missing {len(missing)} keys")

    # Check for missing languages entirely
    missing_languages = set(EXPECTED_LANGUAGES) - set(file_languages)
    if missing_languages:
        print(f"  Missing language sections: {sorted(missing_languages)}")

    return {
        "file": str(filepath.name),
        "languages": file_languages,
        "base_key_count": len(base_keys),
        "missing_by_lang": missing_by_lang,
        "missing_languages": sorted(list(missing_languages))
    }

def main():
    resources_dir = Path(__file__).parent.parent / "Assets" / "Resources"

    json_files = list(resources_dir.glob("*.json"))
    print(f"Found {len(json_files)} JSON files")

    results = []

    for json_file in sorted(json_files):
        result = check_json_file(json_file)
        if result:
            results.append(result)

    # Summary
    print("\n" + "="*60)
    print("SUMMARY")
    print("="*60)

    total_missing = 0
    files_with_issues = []

    for result in results:
        missing_count = sum(len(v) for v in result["missing_by_lang"].values())
        missing_lang_count = len(result["missing_languages"])

        if missing_count > 0 or missing_lang_count > 0:
            files_with_issues.append(result["file"])
            total_missing += missing_count

    print(f"\nTotal files with issues: {len(files_with_issues)}")
    print(f"Total missing key-language pairs: {total_missing}")

    # Save detailed report
    report_path = Path(__file__).parent / "localization_report.json"
    with open(report_path, 'w', encoding='utf-8') as f:
        json.dump(results, f, indent=2, ensure_ascii=False, default=str)
    print(f"\nDetailed report saved to: {report_path}")

if __name__ == "__main__":
    main()
