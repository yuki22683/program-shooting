#!/usr/bin/env python3
"""
Translate a single JSON file with progress tracking.
"""

import json
import sys
import time
from pathlib import Path

from googletrans import Translator

TARGET_LANGUAGES = [
    "en", "ja", "cs", "de", "nl", "da", "el", "fi", "fr", "it",
    "ko", "no", "pl", "pt", "ro", "ru", "es", "sv", "tr",
    "zh-Hans", "zh-Hant", "uk", "vi"
]

LANG_CODE_MAP = {
    "zh-Hans": "zh-cn",
    "zh-Hant": "zh-tw",
}

def translate_text(translator, text, dest_lang, src_lang="en"):
    dest = LANG_CODE_MAP.get(dest_lang, dest_lang)
    try:
        result = translator.translate(text, dest=dest, src=src_lang)
        time.sleep(0.05)  # Rate limiting
        return result.text
    except Exception as e:
        print(f"  Error translating to {dest_lang}: {e}")
        time.sleep(1)
        return text

def process_file(filepath):
    print(f"Processing: {filepath}")
    
    with open(filepath, 'r', encoding='utf-8') as f:
        data = json.load(f)
    
    # Determine base language
    base_lang = "en" if "en" in data else "ja"
    base_keys = set(data[base_lang].keys())
    print(f"Base language: {base_lang}, Total keys: {len(base_keys)}")
    
    translator = Translator()
    modified = False
    
    # Ensure all target languages exist
    for lang in TARGET_LANGUAGES:
        if lang not in data:
            data[lang] = {}
            modified = True
            print(f"Added language section: {lang}")
    
    # Translate missing keys
    for lang in TARGET_LANGUAGES:
        if lang == base_lang:
            continue
        
        lang_keys = set(data[lang].keys())
        missing_keys = base_keys - lang_keys
        
        if not missing_keys:
            continue
            
        print(f"\nTranslating {len(missing_keys)} keys to {lang}...")
        
        for i, key in enumerate(sorted(missing_keys)):
            base_value = data[base_lang][key]
            
            # Skip image paths and code content
            if "_image" in key or base_value.startswith("/"):
                translated = base_value
            elif len(base_value) > 3000:
                # For very long text, keep original
                translated = base_value
            else:
                translated = translate_text(translator, base_value, lang, base_lang)
            
            data[lang][key] = translated
            modified = True
            
            if (i + 1) % 25 == 0:
                print(f"  Progress: {i + 1}/{len(missing_keys)}")
        
        print(f"  Completed: {len(missing_keys)} translations for {lang}")
    
    if modified:
        with open(filepath, 'w', encoding='utf-8', newline='\n') as f:
            json.dump(data, f, indent=4, ensure_ascii=False)
        print(f"\nSaved: {filepath}")
    
    return modified

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python translate_single_file.py <filename>")
        sys.exit(1)
    
    filepath = Path(sys.argv[1])
    if not filepath.exists():
        filepath = Path(__file__).parent.parent / "Assets" / "Resources" / sys.argv[1]
    
    if not filepath.exists():
        print(f"File not found: {filepath}")
        sys.exit(1)
    
    process_file(filepath)
