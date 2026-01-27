#!/usr/bin/env python3
"""
Batch translate JA files to EN using efficient chunking.
"""
import json
import re
import sys
import io
import time
from pathlib import Path

sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')

# Import JSON utilities for sanitization
try:
    from json_utils import sanitize_text_for_json, safe_json_dump, validate_json_file
except ImportError:
    # Fallback if module not found
    def sanitize_text_for_json(text):
        if not text:
            return text
        # Basic smart quote replacement
        text = text.replace('\u201c', '"').replace('\u201d', '"')
        text = text.replace('\u2018', "'").replace('\u2019', "'")
        return text
    safe_json_dump = None
    validate_json_file = None

try:
    from deep_translator import GoogleTranslator
except ImportError:
    import subprocess
    subprocess.check_call([sys.executable, "-m", "pip", "install", "deep-translator", "-q"])
    from deep_translator import GoogleTranslator

LOCAL_DIR = Path("C:/Work/MetaXR/ProgramShooting/Assets/Resources")

LANGUAGES = [
    "python", "javascript", "typescript", "java",
    "c", "cpp", "csharp", "assembly",
    "go", "rust", "ruby", "php",
    "swift", "kotlin", "bash", "sql",
    "lua", "perl", "haskell", "elixir"
]

def protect_code(text):
    blocks = []
    def save_block(m):
        blocks.append(m.group(0))
        return f"CODEBLOCK{len(blocks)-1}ENDBLOCK"
    text = re.sub(r'```[\s\S]*?```', save_block, text)
    inlines = []
    def save_inline(m):
        inlines.append(m.group(0))
        return f"INLINECODE{len(inlines)-1}ENDINLINE"
    text = re.sub(r'`[^`]+`', save_inline, text)
    return text, blocks, inlines

def restore_code(text, blocks, inlines):
    for i, b in enumerate(blocks):
        text = text.replace(f"CODEBLOCK{i}ENDBLOCK", b)
    for i, c in enumerate(inlines):
        text = text.replace(f"INLINECODE{i}ENDINLINE", c)
    return text

def translate_text(translator, text):
    if not text or not text.strip():
        return text
    protected, blocks, inlines = protect_code(text)
    try:
        translated = translator.translate(protected)
        restored = restore_code(translated, blocks, inlines)
        # Sanitize to prevent JSON corruption
        return sanitize_text_for_json(restored)
    except Exception as e:
        return text

def process_language(lang):
    ja_path = LOCAL_DIR / f"{lang}Lessons_ja.json"
    en_path = LOCAL_DIR / f"{lang}Lessons_en.json"
    if not ja_path.exists():
        return 0
    with open(ja_path, 'r', encoding='utf-8') as f:
        ja_data = json.load(f)
    en_data = {}
    if en_path.exists():
        with open(en_path, 'r', encoding='utf-8') as f:
            en_data = json.load(f)
    translator = GoogleTranslator(source='ja', target='en')
    keys_to_translate = [k for k in ja_data if k not in en_data or not en_data[k]]
    if not keys_to_translate:
        print(f"  Already done")
        return 0
    print(f"  {len(keys_to_translate)} keys to translate...")
    count = 0
    for key in keys_to_translate:
        en_data[key] = translate_text(translator, ja_data[key])
        count += 1
        if count % 50 == 0:
            print(f"    {count}/{len(keys_to_translate)}")
        time.sleep(0.02)
    sorted_data = dict(sorted(en_data.items()))

    # Validate before writing
    if safe_json_dump:
        try:
            safe_json_dump(sorted_data, en_path, indent=2)
        except ValueError as e:
            print(f"    ERROR: JSON validation failed: {e}")
            return 0
    else:
        with open(en_path, 'w', encoding='utf-8') as f:
            json.dump(sorted_data, f, ensure_ascii=False, indent=2)

    # Verify the written file
    if validate_json_file:
        is_valid, error, _ = validate_json_file(en_path)
        if not is_valid:
            print(f"    WARNING: Written file has JSON errors: {error}")

    return count

def main():
    print("JA -> EN Translation")
    for lang in LANGUAGES:
        print(f"[{lang}]")
        try:
            count = process_language(lang)
            print(f"  OK: {count}")
        except Exception as e:
            print(f"  ERROR: {e}")
    print("Done")

if __name__ == "__main__":
    main()
