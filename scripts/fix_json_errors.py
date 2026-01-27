#!/usr/bin/env python3
"""
Fix JSON syntax errors in lesson files.

Common issues:
1. Actual newlines inside strings (should be \n)
2. Smart quotes after backslash (should be regular ")
3. Missing commas
4. Invalid escape sequences
5. Strings broken across multiple lines
"""

import os
import json
import re
from pathlib import Path

RESOURCES_PATH = Path(r"C:\Work\MetaXR\ProgramShooting\Assets\Resources")


def fix_broken_strings(content):
    """
    Fix strings that were incorrectly broken across multiple lines.

    This handles cases where a string like:
      "key": "some content
      more content
      end",

    Should be:
      "key": "some content\\nmore content\\nend",
    """
    lines = content.split('\n')
    result_lines = []
    i = 0

    while i < len(lines):
        line = lines[i]

        # Check if this line starts a string that doesn't end properly
        # Pattern: "key": "value... without closing quote and comma
        if re.match(r'^\s*"[^"]+"\s*:\s*"[^"]*$', line):
            # This line starts a string but doesn't close it
            combined = [line.rstrip()]
            j = i + 1

            # Find subsequent lines until we find one that ends with "," or "}
            while j < len(lines):
                next_line = lines[j].rstrip()

                # Check if this line ends the string
                if next_line.endswith('",') or next_line.endswith('"'):
                    combined.append(next_line)
                    break
                elif re.match(r'^\s*"[^"]+"\s*:', next_line):
                    # This is a new key - previous string was truncated
                    # Close the previous string and continue
                    combined[-1] = combined[-1] + '",'
                    j -= 1  # Process this line again
                    break
                else:
                    # This is continuation of the string
                    # Escape the newline
                    combined.append('\\n' + next_line.replace('\\', '\\\\').replace('"', '\\"'))

                j += 1

            # Combine lines
            result_lines.append(''.join(combined))
            i = j + 1
        else:
            result_lines.append(line)
            i += 1

    return '\n'.join(result_lines)


def fix_smart_quotes(content):
    """Replace smart quotes after backslash with regular quotes."""
    smart_quotes = ['\u201c', '\u201d', '\u2018', '\u2019']
    for sq in smart_quotes:
        content = content.replace('\\' + sq, '\\"')
    return content


def fix_inline_newlines(content):
    """Fix actual newline/tab characters inside JSON strings."""
    result = []
    in_string = False
    i = 0

    while i < len(content):
        char = content[i]

        # Track string boundaries
        if char == '"':
            # Check if escaped
            num_backslashes = 0
            j = i - 1
            while j >= 0 and content[j] == '\\':
                num_backslashes += 1
                j -= 1

            if num_backslashes % 2 == 0:
                # Not escaped, toggle string state
                in_string = not in_string
            result.append(char)
        elif in_string:
            if char == '\n':
                result.append('\\n')
            elif char == '\r':
                pass  # Skip CR
            elif char == '\t':
                result.append('\\t')
            elif ord(char) < 32:
                # Other control characters - escape as unicode
                result.append(f'\\u{ord(char):04x}')
            else:
                result.append(char)
        else:
            result.append(char)

        i += 1

    return ''.join(result)


def validate_and_fix_file(filepath):
    """Validate and fix a JSON file."""
    try:
        with open(filepath, 'r', encoding='utf-8') as f:
            content = f.read()

        # Try parsing first
        try:
            json.loads(content)
            return True, "OK"
        except json.JSONDecodeError:
            pass

        # Apply fixes in order
        fixed = content
        fixed = fix_smart_quotes(fixed)
        fixed = fix_inline_newlines(fixed)

        # Try again
        try:
            json.loads(fixed)
            with open(filepath, 'w', encoding='utf-8') as f:
                f.write(fixed)
            return True, "Fixed"
        except json.JSONDecodeError as e:
            return False, f"{e.msg} at line {e.lineno}, col {e.colno}"

    except Exception as e:
        return False, str(e)


def main():
    print("JSON Error Fixer")
    print("=" * 60)

    files_fixed = 0
    files_errored = 0
    errors = []

    for filename in sorted(os.listdir(RESOURCES_PATH)):
        if not filename.endswith('.json'):
            continue

        filepath = RESOURCES_PATH / filename
        success, msg = validate_and_fix_file(filepath)

        if msg == "OK":
            continue
        elif success:
            print(f"[FIXED] {filename}: {msg}")
            files_fixed += 1
        else:
            print(f"[ERROR] {filename}: {msg}")
            files_errored += 1
            errors.append((filename, msg))

    print()
    print("=" * 60)
    print(f"Fixed: {files_fixed}, Errors remaining: {files_errored}")

    return 0 if files_errored == 0 else 1


if __name__ == "__main__":
    exit(main())
