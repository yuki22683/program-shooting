#!/usr/bin/env python3
"""
Validate All JSON Files

This script validates all JSON files in the Resources folder.
Run this:
- After translation scripts complete
- Before committing changes
- As part of CI/CD pipeline

Exit code: 0 if all valid, 1 if errors found
"""

import json
import os
import sys
from pathlib import Path

RESOURCES_PATH = Path(r"C:\Work\MetaXR\ProgramShooting\Assets\Resources")

# Characters that indicate potential issues
SMART_QUOTES = ['\u201c', '\u201d', '\u2018', '\u2019']


def check_potential_issues(content, filename):
    """Check for potential issues that might cause problems later."""
    issues = []

    # Check for smart quotes after backslash (will cause JSON error if not caught)
    for i, c in enumerate(content):
        if c == '\\' and i + 1 < len(content):
            if content[i + 1] in SMART_QUOTES:
                issues.append(f"Smart quote after backslash at position {i}")

    # Check for actual newlines inside strings
    in_string = False
    for i, c in enumerate(content):
        if c == '"':
            bs = 0
            j = i - 1
            while j >= 0 and content[j] == '\\':
                bs += 1
                j -= 1
            if bs % 2 == 0:
                in_string = not in_string
        elif c == '\n' and in_string:
            issues.append(f"Actual newline in string at position {i}")
            break  # One is enough to report

    return issues


def validate_file(filepath):
    """
    Validate a single JSON file.

    Returns (is_valid, error_message, potential_issues)
    """
    try:
        with open(filepath, 'r', encoding='utf-8') as f:
            content = f.read()

        # Check for potential issues first
        potential_issues = check_potential_issues(content, filepath.name)

        # Try to parse
        try:
            json.loads(content)
            return True, None, potential_issues
        except json.JSONDecodeError as e:
            return False, f"{e.msg} at line {e.lineno}, col {e.colno}", potential_issues

    except Exception as e:
        return False, str(e), []


def main():
    print("=" * 70)
    print("JSON Validation Report")
    print("=" * 70)
    print()

    errors = []
    warnings = []
    valid_count = 0

    for filename in sorted(os.listdir(RESOURCES_PATH)):
        if not filename.endswith('.json'):
            continue

        filepath = RESOURCES_PATH / filename
        is_valid, error, potential_issues = validate_file(filepath)

        if not is_valid:
            errors.append((filename, error))
        elif potential_issues:
            warnings.append((filename, potential_issues))
            valid_count += 1
        else:
            valid_count += 1

    # Report results
    print(f"Valid files: {valid_count}")
    print(f"Files with errors: {len(errors)}")
    print(f"Files with warnings: {len(warnings)}")
    print()

    if errors:
        print("ERRORS:")
        print("-" * 50)
        for filename, error in errors:
            print(f"  {filename}")
            print(f"    {error}")
        print()

    if warnings:
        print("WARNINGS (potential future issues):")
        print("-" * 50)
        for filename, issues in warnings:
            print(f"  {filename}")
            for issue in issues[:3]:
                print(f"    - {issue}")
            if len(issues) > 3:
                print(f"    ... and {len(issues) - 3} more")
        print()

    # Summary
    print("=" * 70)
    if errors:
        print("VALIDATION FAILED")
        print(f"Fix {len(errors)} file(s) before proceeding.")
        print()
        print("To attempt automatic fixes, run:")
        print("  python scripts/fix_json_errors.py")
        return 1
    elif warnings:
        print("VALIDATION PASSED WITH WARNINGS")
        print("Consider fixing the warnings to prevent future issues.")
        return 0
    else:
        print("VALIDATION PASSED")
        print("All JSON files are valid.")
        return 0


if __name__ == "__main__":
    sys.exit(main())
