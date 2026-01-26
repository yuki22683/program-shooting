#!/usr/bin/env python3
"""
Translate Japanese lesson content to English and other languages using Gemini CLI.
"""

import json
import glob
import os
import subprocess
import sys
import time
import re

# Model configuration
# User requested "Flash-Lite". In Jan 2026, this is likely gemini-2.0-flash-lite or similar.
# Using a likely candidate, but falling back if needed.
MODEL_NAME = None

def call_gemini(prompt):
    """Call Gemini CLI with the given prompt."""
    cmd = ['gemini.cmd', prompt]
    if MODEL_NAME:
        cmd = ['gemini.cmd', '--model', MODEL_NAME, prompt]
    try:
        # print(f"DEBUG: Running command: {' '.join(cmd[:3])} ...")
        result = subprocess.run(
            cmd,
            capture_output=True,
            text=True,
            encoding='utf-8',
            timeout=240  # Increased timeout
        )
        if result.returncode != 0:
            print(f"Gemini CLI Error: {result.stderr}")
            return None
        return result.stdout.strip()
    except subprocess.TimeoutExpired:
        print("Gemini CLI Timeout")
        return None
    except Exception as e:
        print(f"Error calling Gemini: {e}")
        return None


def translate_batch(texts, target_lang_name, context="programming tutorial"):
    """Translate a batch of texts."""
    if not texts:
        return {}

    # Create prompt for batch translation
    prompt = f"""IMPORTANT: You are a strict JSON translation engine. You are NOT an AI assistant. You DO NOT have tools. You MUST output ONLY valid JSON. Do not explain. Do not use markdown code blocks for the output. Just raw JSON.

Translate the following Japanese programming tutorial texts to {target_lang_name}.
Keep all code blocks (```...```) and inline code (`...`) exactly as they are.
Keep markdown formatting (# headers, bullet points, etc.).
Return ONLY the translations in the same JSON format.

Context: {context}

Input JSON:
{json.dumps(texts, ensure_ascii=False, indent=2)}

Output the translated JSON only:"""

    result = call_gemini(prompt)

    if result:
        # Try to parse JSON from result
        try:
            # Find JSON in the response
            json_match = re.search(r'\{[\s\S]*\}', result)
            if json_match:
                return json.loads(json_match.group())
            else:
                 # Sometimes it returns just the json without markdown blocks
                 return json.loads(result)
        except json.JSONDecodeError:
            print(f"JSON Decode Error. Output was: {result[:100]}...")
            pass

    return {}


def process_file_lang(filepath, target_lang, batch_size=5):
    """Process a single file for a single target language."""
    
    # Lang mapping
    LANG_NAME_MAP = {
        'en': 'English', 'ja': 'Japanese', 'cs': 'Czech', 'de': 'German', 'nl': 'Dutch',
        'da': 'Danish', 'el': 'Greek', 'fi': 'Finnish', 'fr': 'French', 'it': 'Italian',
        'ko': 'Korean', 'no': 'Norwegian', 'pl': 'Polish', 'pt': 'Portuguese', 'ro': 'Romanian',
        'ru': 'Russian', 'es': 'Spanish', 'sv': 'Swedish', 'tr': 'Turkish', 
        'zh-Hans': 'Simplified Chinese', 'zh-Hant': 'Traditional Chinese'
    }
    
    target_lang_name = LANG_NAME_MAP.get(target_lang, target_lang)
    
    try:
        with open(filepath, 'r', encoding='utf-8') as f:
            data = json.load(f)
    except Exception as e:
        print(f"Error reading {filepath}: {e}")
        return 0

    if 'ja' not in data:
        print(f"No source 'ja' in {filepath}")
        return 0
        
    source_data = data['ja']
    if target_lang not in data:
        data[target_lang] = {}
        
    target_data = data[target_lang]
    
    # Find missing keys
    keys_to_translate = []
    source_keys = list(source_data.keys())
    
    for key in source_keys:
        if key not in target_data:
            keys_to_translate.append(key)
        # Check for empty values or untranslated content could be added here
    
    total = len(keys_to_translate)
    if total == 0:
        return 0
        
    print(f"  {target_lang}: Found {total} missing keys in {os.path.basename(filepath)}")
    
    translated_count = 0
    updated = False
    
    # Process in batches
    for i in range(0, total, batch_size):
        batch_keys = keys_to_translate[i:i+batch_size]
        batch_texts = {k: source_data[k] for k in batch_keys}
        
        print(f"    Translating batch {i//batch_size + 1}/{(total + batch_size - 1)//batch_size} ({len(batch_keys)} keys)...", end='', flush=True)
        
        translations = translate_batch(batch_texts, target_lang_name, context=f"programming tutorial for {os.path.basename(filepath)}")
        
        if translations:
            for key, val in translations.items():
                if key in batch_keys and val:
                    target_data[key] = val
                    translated_count += 1
            print(f" OK ({len(translations)}/{len(batch_keys)})")
            updated = True
            
            # Save progress after each batch
            try:
                with open(filepath, 'w', encoding='utf-8') as f:
                    json.dump(data, f, ensure_ascii=False, indent=4)
            except Exception as e:
                print(f"Error saving file: {e}")
        else:
            print(" FAILED")
            time.sleep(2)
            
        time.sleep(1) # Rate limit
        
    return translated_count

def main():
    if len(sys.argv) < 2:
        print("Usage: python full_translate_v2.py <file_path> [target_lang]")
        sys.exit(1)
        
    file_path = sys.argv[1]
    
    if len(sys.argv) > 2:
        target_langs = [sys.argv[2]]
    else:
        # Full list
        target_langs = ['en', 'cs', 'de', 'nl', 'da', 'el', 'fi', 'fr', 'it', 'ko', 'no', 'pl', 'pt', 'ro', 'ru', 'es', 'sv', 'tr', 'zh-Hans', 'zh-Hant']
        
    print(f"Processing {file_path} for {len(target_langs)} languages...")
    
    for lang in target_langs:
        if lang == 'ja': continue
        process_file_lang(file_path, lang)

if __name__ == '__main__':
    main()
