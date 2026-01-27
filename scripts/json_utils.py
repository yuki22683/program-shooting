#!/usr/bin/env python3
"""
JSON utility functions for translation scripts.

Provides sanitization and validation to prevent JSON corruption.
"""

import json
import re


def sanitize_text_for_json(text):
    """
    Sanitize translated text for safe JSON storage.

    Fixes common issues from translation APIs:
    1. Smart quotes -> regular quotes
    2. Control characters -> escaped sequences
    3. Invalid escape sequences
    """
    if not text or not isinstance(text, str):
        return text

    # Replace smart quotes with regular quotes
    smart_quote_map = {
        '\u201c': '"',  # Left double quotation mark
        '\u201d': '"',  # Right double quotation mark
        '\u2018': "'",  # Left single quotation mark
        '\u2019': "'",  # Right single quotation mark
        '\u00ab': '"',  # Left-pointing double angle quotation mark
        '\u00bb': '"',  # Right-pointing double angle quotation mark
    }
    for smart, regular in smart_quote_map.items():
        text = text.replace(smart, regular)

    # Replace other problematic Unicode characters
    text = text.replace('\u2013', '-')  # En dash
    text = text.replace('\u2014', '--')  # Em dash
    text = text.replace('\u2026', '...')  # Horizontal ellipsis
    text = text.replace('\u00a0', ' ')  # Non-breaking space

    return text


def validate_json_string(text):
    """
    Validate that a string can be safely stored in JSON.

    Returns (is_valid, error_message)
    """
    try:
        # Try to encode as JSON string
        json.dumps({"test": text})
        return True, None
    except (TypeError, ValueError) as e:
        return False, str(e)


def validate_json_file(filepath):
    """
    Validate a JSON file.

    Returns (is_valid, error_message, data)
    """
    try:
        with open(filepath, 'r', encoding='utf-8') as f:
            data = json.load(f)
        return True, None, data
    except json.JSONDecodeError as e:
        return False, f"{e.msg} at line {e.lineno}, col {e.colno}", None
    except Exception as e:
        return False, str(e), None


def safe_json_dump(data, filepath, indent=2):
    """
    Safely write JSON to file with validation.

    Raises ValueError if the data cannot be serialized to valid JSON.
    """
    # First, try to serialize to string to catch any issues
    try:
        json_str = json.dumps(data, ensure_ascii=False, indent=indent)
    except (TypeError, ValueError) as e:
        raise ValueError(f"Failed to serialize data: {e}")

    # Validate by parsing it back
    try:
        json.loads(json_str)
    except json.JSONDecodeError as e:
        raise ValueError(f"Serialized JSON is invalid: {e}")

    # Write to file
    with open(filepath, 'w', encoding='utf-8') as f:
        f.write(json_str)


def sanitize_translation_dict(translations):
    """
    Sanitize all values in a translation dictionary.

    Returns sanitized dict and list of keys that had issues.
    """
    sanitized = {}
    issues = []

    for key, value in translations.items():
        if isinstance(value, str):
            original = value
            sanitized_value = sanitize_text_for_json(value)

            # Validate the sanitized value
            is_valid, error = validate_json_string(sanitized_value)
            if not is_valid:
                issues.append((key, error))
                # Skip problematic values
                continue

            sanitized[key] = sanitized_value

            if sanitized_value != original:
                issues.append((key, "sanitized"))
        else:
            sanitized[key] = value

    return sanitized, issues
