#!/usr/bin/env python3
"""
Translate all *Lessons_ja.json files to English using Google Translate.
Preserves code blocks and technical formatting.
"""
import json
import re
import sys
import io
import time
from pathlib import Path

sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')

try:
    from googletrans import Translator
except ImportError:
    print("Installing googletrans...")
    import subprocess
    subprocess.check_call([sys.executable, "-m", "pip", "install", "googletrans==4.0.0-rc1", "-q"])
    from googletrans import Translator

LOCAL_DIR = Path("C:/Work/MetaXR/ProgramShooting/Assets/Resources")

LANGUAGES = [
    "python", "javascript", "typescript", "java",
    "c", "cpp", "csharp", "assembly",
    "go", "rust", "ruby", "php",
    "swift", "kotlin", "bash", "sql",
    "lua", "perl", "haskell", "elixir"
]

def extract_code_blocks(text):
    """Extract code blocks and replace with placeholders"""
    code_blocks = []
    pattern = r'```[\s\S]*?```'

    def replacer(match):
        code_blocks.append(match.group(0))
        return f'__CODE_BLOCK_{len(code_blocks)-1}__'

    text = re.sub(pattern, replacer, text)
    return text, code_blocks

def restore_code_blocks(text, code_blocks):
    """Restore code blocks from placeholders"""
    for i, block in enumerate(code_blocks):
        text = text.replace(f'__CODE_BLOCK_{i}__', block)
    return text

def extract_inline_code(text):
    """Extract inline code and replace with placeholders"""
    inline_codes = []
    pattern = r'`[^`]+`'

    def replacer(match):
        inline_codes.append(match.group(0))
        return f'__INLINE_{len(inline_codes)-1}__'

    text = re.sub(pattern, replacer, text)
    return text, inline_codes

def restore_inline_code(text, inline_codes):
    """Restore inline code from placeholders"""
    for i, code in enumerate(inline_codes):
        text = text.replace(f'__INLINE_{i}__', code)
    return text

def translate_text(translator, text, retries=3):
    """Translate text from Japanese to English, preserving code"""
    if not text or not text.strip():
        return text

    # Extract code blocks first
    text, code_blocks = extract_code_blocks(text)
    # Extract inline code
    text, inline_codes = extract_inline_code(text)

    # Translate
    for attempt in range(retries):
        try:
            result = translator.translate(text, src='ja', dest='en')
            translated = result.text
            break
        except Exception as e:
            if attempt < retries - 1:
                time.sleep(1)
            else:
                print(f"    Translation error: {e}")
                translated = text

    # Restore code
    translated = restore_inline_code(translated, inline_codes)
    translated = restore_code_blocks(translated, code_blocks)

    return translated

def process_file(lang, translator):
    """Process a single language file"""
    ja_file = LOCAL_DIR / f"{lang}Lessons_ja.json"
    en_file = LOCAL_DIR / f"{lang}Lessons_en.json"

    if not ja_file.exists():
        print(f"  [SKIP] {ja_file.name} not found")
        return 0

    # Load JA file
    with open(ja_file, 'r', encoding='utf-8') as f:
        ja_data = json.load(f)

    # Load existing EN file if exists
    if en_file.exists():
        with open(en_file, 'r', encoding='utf-8') as f:
            en_data = json.load(f)
    else:
        en_data = {}

    translated_count = 0
    total = len(ja_data)

    for i, (key, ja_value) in enumerate(ja_data.items()):
        # Skip if already translated and matches
        if key in en_data and en_data[key] and not en_data[key].startswith('__'):
            continue

        # Translate
        en_value = translate_text(translator, ja_value)
        en_data[key] = en_value
        translated_count += 1

        # Progress
        if translated_count % 50 == 0:
            print(f"    {translated_count} translated...")

        # Rate limiting
        time.sleep(0.1)

    # Sort and save
    sorted_data = dict(sorted(en_data.items()))
    with open(en_file, 'w', encoding='utf-8') as f:
        json.dump(sorted_data, f, ensure_ascii=False, indent=2)

    return translated_count

def main():
    print("=" * 60)
    print("JA -> EN 翻訳開始")
    print("=" * 60)

    translator = Translator()

    for lang in LANGUAGES:
        print(f"\n[{lang}]")
        try:
            count = process_file(lang, translator)
            print(f"  [OK] {count} keys translated")
        except Exception as e:
            print(f"  [ERROR] {e}")

    print("\n" + "=" * 60)
    print("翻訳完了")
    print("=" * 60)

if __name__ == "__main__":
    main()
