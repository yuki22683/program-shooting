#!/usr/bin/env python3
"""
Complete translation script using Google Translate API (googletrans library).
Translates all missing keys from English to all required languages.
"""

import json
import os
import time
from pathlib import Path
from typing import Dict, Any

try:
    from googletrans import Translator
    HAS_GOOGLETRANS = True
except ImportError:
    HAS_GOOGLETRANS = False
    print("WARNING: googletrans not installed. Install with: pip install googletrans==4.0.0-rc1")

# Target languages with their codes
TARGET_LANGUAGES = {
    "en": "English",
    "ja": "Japanese", 
    "cs": "Czech",
    "de": "German",
    "nl": "Dutch",
    "da": "Danish",
    "el": "Greek",
    "fi": "Finnish",
    "fr": "French",
    "it": "Italian",
    "ko": "Korean",
    "no": "Norwegian",
    "pl": "Polish",
    "pt": "Portuguese",
    "ro": "Romanian",
    "ru": "Russian",
    "es": "Spanish",
    "sv": "Swedish",
    "tr": "Turkish",
    "zh-Hans": "zh-cn",  # Simplified Chinese
    "zh-Hant": "zh-tw",  # Traditional Chinese
    "uk": "Ukrainian",
    "vi": "Vietnamese"
}

# Map for googletrans codes
LANG_CODE_MAP = {
    "zh-Hans": "zh-cn",
    "zh-Hant": "zh-tw",
    "no": "no"  # Norwegian
}

def get_translator():
    if HAS_GOOGLETRANS:
        return Translator()
    return None

def translate_text(translator, text: str, dest_lang: str, src_lang: str = "en") -> str:
    """Translate text using Google Translate."""
    if not translator:
        return text  # Return original if no translator
    
    # Map language codes
    dest = LANG_CODE_MAP.get(dest_lang, dest_lang)
    
    try:
        result = translator.translate(text, dest=dest, src=src_lang)
        time.sleep(0.1)  # Rate limiting
        return result.text
    except Exception as e:
        print(f"    Translation error for {dest_lang}: {e}")
        return text  # Return original on error

def load_json(filepath: Path) -> Dict[str, Any]:
    """Load JSON file."""
    try:
        with open(filepath, 'r', encoding='utf-8') as f:
            return json.load(f)
    except Exception as e:
        print(f"Error loading {filepath}: {e}")
        return None

def save_json(filepath: Path, data: Dict[str, Any]):
    """Save JSON file."""
    with open(filepath, 'w', encoding='utf-8', newline='\n') as f:
        json.dump(data, f, indent=4, ensure_ascii=False)

def process_file(filepath: Path, translator):
    """Process a single JSON file to complete translations."""
    print(f"\n{'='*60}")
    print(f"Processing: {filepath.name}")
    print(f"{'='*60}")
    
    data = load_json(filepath)
    if not data:
        return False
    
    # Get all language codes in file
    file_languages = [k for k in data.keys() if isinstance(data.get(k), dict)]
    
    # Determine base language (prefer English, fallback to Japanese)
    if "en" in data:
        base_lang = "en"
    elif "ja" in data:
        base_lang = "ja"
    else:
        print(f"  No base language found, skipping")
        return False
    
    base_keys = set(data[base_lang].keys())
    print(f"  Base language: {base_lang}, Keys: {len(base_keys)}")
    
    modified = False
    
    # First, ensure all target languages exist
    for lang in TARGET_LANGUAGES.keys():
        if lang not in data:
            data[lang] = {}
            print(f"  Added new language section: {lang}")
            modified = True
    
    # Now translate missing keys for each language
    for lang in TARGET_LANGUAGES.keys():
        if lang == base_lang:
            continue
        
        lang_keys = set(data[lang].keys())
        missing_keys = base_keys - lang_keys
        
        if missing_keys:
            print(f"  Translating {len(missing_keys)} keys to {lang}...")
            
            for i, key in enumerate(sorted(missing_keys)):
                base_value = data[base_lang][key]
                
                # Don't translate certain content types
                if "_image" in key or key.endswith("_code"):
                    translated = base_value
                else:
                    translated = translate_text(translator, base_value, lang, base_lang)
                
                data[lang][key] = translated
                modified = True
                
                if (i + 1) % 50 == 0:
                    print(f"    Translated {i + 1}/{len(missing_keys)} keys")
            
            print(f"    Completed {len(missing_keys)} translations for {lang}")
    
    if modified:
        save_json(filepath, data)
        print(f"  Saved changes to {filepath.name}")
    else:
        print(f"  No changes needed for {filepath.name}")
    
    return modified

def main():
    resources_dir = Path(__file__).parent.parent / "Assets" / "Resources"
    
    # Files to process (excluding broken ones)
    json_files = [
        "localizedText.json",
        "pythonLessons.json",
        "javascriptLessons.json",
        "typescriptLessons.json",
        "javaLessons.json",
        "cLessons.json",
        "cppLessons.json",
        "csharpLessons.json",
        "assemblyLessons.json",
        "goLessons.json",
        "rustLessons.json",
        "rubyLessons.json",
        "phpLessons.json",
        "swiftLessons.json",
        "kotlinLessons.json",
        "bashLessons.json",
        "sqlLessons.json",
        "luaLessons.json",
        "perlLessons.json",
        "haskellLessons.json",
        "elixirLessons.json"
    ]
    
    translator = get_translator()
    if not translator:
        print("No translator available. Running in dry-run mode (adding empty sections only).")
    
    for filename in json_files:
        filepath = resources_dir / filename
        if filepath.exists():
            try:
                process_file(filepath, translator)
            except Exception as e:
                print(f"Error processing {filename}: {e}")
    
    print("\n" + "="*60)
    print("Translation complete!")
    print("="*60)

if __name__ == "__main__":
    main()
