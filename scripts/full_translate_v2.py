#!/usr/bin/env python3
"""
Full translation script using deep-translator.
Processes all JSON files and translates missing keys.
"""

import json
import sys
import time
from pathlib import Path
from deep_translator import GoogleTranslator

TARGET_LANGUAGES = [
    "cs", "de", "nl", "da", "el", "fi", "fr", "it",
    "ko", "no", "pl", "pt", "ro", "ru", "es", "sv", "tr",
    "zh-Hans", "zh-Hant", "uk", "vi"
]

# Google Translate language codes
LANG_MAP = {
    "zh-Hans": "zh-CN",
    "zh-Hant": "zh-TW",
    "no": "no",
}

def translate(text, target_lang, source_lang="en"):
    """Translate text using Google Translate."""
    if not text or len(text) < 2:
        return text
    
    target = LANG_MAP.get(target_lang, target_lang)
    source = LANG_MAP.get(source_lang, source_lang)
    
    try:
        # Split long text into chunks (max 5000 chars for Google Translate)
        if len(text) > 4500:
            # Keep original for very long texts with code
            if "```" in text or "print(" in text or "def " in text:
                return text
            chunks = []
            current = ""
            for line in text.split("\n"):
                if len(current) + len(line) > 4000:
                    if current:
                        chunks.append(current)
                    current = line
                else:
                    current += "\n" + line if current else line
            if current:
                chunks.append(current)
            
            translated_chunks = []
            for chunk in chunks:
                try:
                    result = GoogleTranslator(source=source, target=target).translate(chunk)
                    translated_chunks.append(result)
                    time.sleep(0.1)
                except:
                    translated_chunks.append(chunk)
            return "\n".join(translated_chunks)
        
        result = GoogleTranslator(source=source, target=target).translate(text)
        time.sleep(0.05)
        return result
    except Exception as e:
        print(f"  Translation error ({target_lang}): {e}")
        time.sleep(0.5)
        return text

def process_file(filepath, dry_run=False):
    """Process a single JSON file."""
    print(f"\n{'='*60}")
    print(f"Processing: {filepath.name}")
    print(f"{'='*60}")
    
    with open(filepath, 'r', encoding='utf-8') as f:
        data = json.load(f)
    
    # Determine base language
    if "en" in data and isinstance(data["en"], dict):
        base_lang = "en"
    elif "ja" in data and isinstance(data["ja"], dict):
        base_lang = "ja"
    else:
        print("  No valid base language found!")
        return
    
    base_keys = set(data[base_lang].keys())
    print(f"  Base: {base_lang}, Keys: {len(base_keys)}")
    
    modified = False
    
    # Ensure all target languages exist
    for lang in TARGET_LANGUAGES:
        if lang not in data:
            data[lang] = {}
            modified = True
    
    # Also ensure en and ja exist
    if "en" not in data:
        data["en"] = {}
        modified = True
    if "ja" not in data:
        data["ja"] = {}
        modified = True
    
    # Translate missing keys
    for lang in TARGET_LANGUAGES:
        if lang == base_lang:
            continue
        
        lang_keys = set(data[lang].keys()) if lang in data else set()
        missing = base_keys - lang_keys
        
        if not missing:
            continue
        
        print(f"  {lang}: {len(missing)} keys to translate")
        
        if dry_run:
            continue
        
        count = 0
        for key in sorted(missing):
            base_value = data[base_lang][key]
            
            # Skip non-translatable content
            if "_image" in key or key.endswith("_code"):
                translated = base_value
            elif base_value.startswith("/") or base_value.startswith("http"):
                translated = base_value
            else:
                translated = translate(base_value, lang, base_lang)
            
            data[lang][key] = translated
            modified = True
            count += 1
            
            if count % 20 == 0:
                print(f"    {count}/{len(missing)}")
        
        print(f"    Done: {count} keys")
    
    if modified and not dry_run:
        with open(filepath, 'w', encoding='utf-8', newline='\n') as f:
            json.dump(data, f, indent=4, ensure_ascii=False)
        print(f"  Saved!")
    
    return modified

def main():
    resources = Path(__file__).parent.parent / "Assets" / "Resources"
    
    # Files to process
    files = [
        # Small files first
        "luaLessons.json",
        "haskellLessons.json",
        "elixirLessons.json",
        "perlLessons.json",
        "phpLessons.json",
        "rubyLessons.json",
        "rustLessons.json",
        "bashLessons.json",
        "sqlLessons.json",
        "swiftLessons.json",
        "goLessons.json",
        "csharpLessons.json",
        "cLessons.json",
        "cppLessons.json",
        "kotlinLessons.json",
        "javaLessons.json",
        "typescriptLessons.json",
        "javascriptLessons.json",
        "assemblyLessons.json",
        "pythonLessons.json",
        "localizedText.json",
    ]
    
    dry_run = "--dry-run" in sys.argv
    if dry_run:
        print("DRY RUN MODE - No changes will be saved")
    
    for filename in files:
        filepath = resources / filename
        if filepath.exists():
            try:
                process_file(filepath, dry_run)
            except Exception as e:
                print(f"  ERROR: {e}")
                import traceback
                traceback.print_exc()

if __name__ == "__main__":
    main()
