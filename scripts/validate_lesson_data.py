#!/usr/bin/env python3
"""
Lesson Data Validation Script

This script validates that all lesson JSON files have complete data:
1. Every exercise has required slides
2. No missing slide titles or content
3. Exercise numbering is sequential
4. All required keys exist

Run this after syncing data from the reference site.
"""

import json
import re
from pathlib import Path
from collections import defaultdict

PROJECT_RESOURCES_PATH = Path(r"C:\Work\MetaXR\ProgramShooting\Assets\Resources")

LANGUAGES = [
    "python", "javascript", "typescript", "java", "c", "cpp", "csharp",
    "go", "rust", "ruby", "php", "swift", "kotlin", "bash", "sql",
    "lua", "perl", "haskell", "elixir", "assembly"
]

# Minimum required keys for each exercise
REQUIRED_EXERCISE_KEYS = [
    "title",
    "description",
    "slide1_title",
    "slide1_content",
]

def load_json_file(file_path):
    """Load a JSON file."""
    if not file_path.exists():
        return None
    with open(file_path, 'r', encoding='utf-8') as f:
        return json.load(f)

def extract_exercise_structure(data, language):
    """Extract exercise structure from lesson data."""
    exercises = defaultdict(lambda: defaultdict(dict))

    # Pattern: {language}_lesson{N}_ex{M}_{field}
    pattern = re.compile(rf'^{language}_lesson(\d+)_ex(\d+)_(.+)$')

    for key in data.keys():
        match = pattern.match(key)
        if match:
            lesson_num = int(match.group(1))
            ex_num = int(match.group(2))
            field = match.group(3)
            exercises[lesson_num][ex_num][field] = True

    return exercises

def validate_language(language):
    """Validate lesson data for a language."""
    errors = []
    warnings = []

    # Load Japanese file (primary content)
    ja_file = PROJECT_RESOURCES_PATH / f"{language}Lessons_ja.json"
    ja_data = load_json_file(ja_file)

    if not ja_data:
        errors.append(f"Missing file: {ja_file.name}")
        return errors, warnings

    # Extract exercise structure
    exercises = extract_exercise_structure(ja_data, language)

    if not exercises:
        errors.append(f"No exercises found in {ja_file.name}")
        return errors, warnings

    # Validate each lesson
    for lesson_num in sorted(exercises.keys()):
        lesson_exercises = exercises[lesson_num]

        # Check for sequential exercise numbering
        ex_numbers = sorted(lesson_exercises.keys())
        expected = list(range(1, max(ex_numbers) + 1))
        missing_ex = set(expected) - set(ex_numbers)
        if missing_ex:
            errors.append(f"{language} lesson{lesson_num}: Missing exercises: {sorted(missing_ex)}")

        # Validate each exercise
        for ex_num in sorted(lesson_exercises.keys()):
            ex_fields = lesson_exercises[ex_num]
            prefix = f"{language}_lesson{lesson_num}_ex{ex_num}"

            # Check required keys
            for req_key in REQUIRED_EXERCISE_KEYS:
                if req_key not in ex_fields:
                    errors.append(f"Missing: {prefix}_{req_key}")

            # Check slide continuity
            slide_nums = []
            for field in ex_fields.keys():
                match = re.match(r'slide(\d+)_', field)
                if match:
                    slide_nums.append(int(match.group(1)))

            if slide_nums:
                slide_nums = sorted(set(slide_nums))
                expected_slides = list(range(1, max(slide_nums) + 1))
                missing_slides = set(expected_slides) - set(slide_nums)
                if missing_slides:
                    warnings.append(f"{prefix}: Missing slides {sorted(missing_slides)}")

                # Check each slide has both title and content
                for slide_num in slide_nums:
                    if f"slide{slide_num}_title" not in ex_fields:
                        errors.append(f"Missing: {prefix}_slide{slide_num}_title")
                    if f"slide{slide_num}_content" not in ex_fields:
                        errors.append(f"Missing: {prefix}_slide{slide_num}_content")
            else:
                errors.append(f"{prefix}: No slides found")

    return errors, warnings

def main():
    print("=" * 70)
    print("Lesson Data Validation Report")
    print("=" * 70)
    print()

    total_errors = 0
    total_warnings = 0
    languages_with_errors = []

    for language in LANGUAGES:
        errors, warnings = validate_language(language)

        if errors or warnings:
            print(f"\n{language.upper()}:")

            if errors:
                print(f"  ERRORS ({len(errors)}):")
                for err in errors[:5]:  # Show first 5
                    print(f"    - {err}")
                if len(errors) > 5:
                    print(f"    ... and {len(errors) - 5} more errors")
                total_errors += len(errors)
                languages_with_errors.append(language)

            if warnings:
                print(f"  WARNINGS ({len(warnings)}):")
                for warn in warnings[:3]:  # Show first 3
                    print(f"    - {warn}")
                if len(warnings) > 3:
                    print(f"    ... and {len(warnings) - 3} more warnings")
                total_warnings += len(warnings)
        else:
            print(f"{language.upper()}: OK")

    # Summary
    print("\n" + "=" * 70)
    print("Summary")
    print("=" * 70)
    print(f"Total Errors: {total_errors}")
    print(f"Total Warnings: {total_warnings}")

    if languages_with_errors:
        print(f"\nLanguages with errors: {', '.join(languages_with_errors)}")
        print("\nValidation FAILED - Please fix the errors before proceeding.")
        return 1
    else:
        print("\nValidation PASSED - All lesson data is complete.")
        return 0

if __name__ == "__main__":
    exit(main())
