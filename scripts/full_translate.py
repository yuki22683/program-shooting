
import json
import os
import subprocess
import time
import re
import sys

# Master language list from localizedText.json
TARGET_LANGUAGES = ['en', 'ja', 'cs', 'de', 'nl', 'da', 'el', 'fi', 'fr', 'it', 'ko', 'no', 'pl', 'pt', 'ro', 'ru', 'es', 'sv', 'tr', 'zh-Hans', 'zh-Hant']

# Language name mapping for better translation
LANG_NAME_MAP = {
    'en': 'English', 'ja': 'Japanese', 'cs': 'Czech', 'de': 'German', 'nl': 'Dutch',
    'da': 'Danish', 'el': 'Greek', 'fi': 'Finnish', 'fr': 'French', 'it': 'Italian',
    'ko': 'Korean', 'no': 'Norwegian', 'pl': 'Polish', 'pt': 'Portuguese', 'ro': 'Romanian',
    'ru': 'Russian', 'es': 'Spanish', 'sv': 'Swedish', 'tr': 'Turkish', 
    'zh-Hans': 'Simplified Chinese', 'zh-Hant': 'Traditional Chinese'
}

def call_gemini(prompt):
    try:
        # Use shell=True on Windows to better handle batch files and encodings
        # Use stdin to pass the prompt to avoid command line length limits and encoding issues
        result = subprocess.run(
            ['gemini.cmd'],
            input=prompt,
            capture_output=True,
            text=True,
            encoding='utf-8',
            timeout=180,
            shell=True
        )
        output = result.stdout.strip()
        # Clean up Gemini CLI noise
        lines = output.split('\n')
        # Skip the first few lines if they are about cached credentials or command echo
        filtered_lines = [l for l in lines if 'cached credentials' not in l.lower() and l.strip()]
        return '\n'.join(filtered_lines).strip()
    except Exception as e:
        print(f"  Error calling Gemini: {e}")
        return None

def translate_batch(source_texts, target_lang, context="VR Programming Learning App"):
    if not source_texts:
        return {}
    
    target_lang_name = LANG_NAME_MAP.get(target_lang, target_lang)
    input_json = json.dumps(source_texts, ensure_ascii=False, indent=2)
    
    prompt = f"""Translate the following Japanese programming tutorial/UI texts to {target_lang_name}.
Context: {context}
- Keep code blocks (```...```) and inline code (`...`) exactly as they are.
- Keep markdown formatting and special symbols like \n.
- Return ONLY the translated JSON. No markdown code blocks, no explanations.

Input (Japanese JSON):
{input_json}

Output ({target_lang_name} JSON):"""

    max_retries = 2
    for attempt in range(max_retries + 1):
        output = call_gemini(prompt)
        if not output:
            continue

        try:
            # Extract JSON
            json_match = re.search(r'\{[\s\S]*\}', output)
            if json_match:
                result = json.loads(json_match.group())
                # Verify that some expected keys are present
                if any(k in result for k in source_texts):
                    return result
            else:
                result = json.loads(output)
                return result
        except Exception as e:
            if attempt == max_retries:
                print(f"\n    DEBUG: Gemini output (first 100 chars): {output[:100]}...")
                print(f"    DEBUG: Error: {e}")
    
    return {}

def has_japanese(text):
    if not isinstance(text, str):
        return False
    return bool(re.search(r'[\u3040-\u309f\u30a0-\u30ff\u4e00-\u9fff]', text))

def process_file(file_path, batch_size=10):
    print(f"\nProcessing {file_path}...")
    with open(file_path, 'r', encoding='utf-8') as f:
        data = json.load(f)
    
    source_lang = 'ja'
    if source_lang not in data:
        print(f"  Error: Source language '{source_lang}' not found in {file_path}")
        return

    source_data = data[source_lang]
    all_keys = list(source_data.keys())
    
    updated = False
    
    for lang in TARGET_LANGUAGES:
        if lang == source_lang:
            continue
            
        print(f"  Target language: {lang} ({LANG_NAME_MAP.get(lang)})")
        
        if lang not in data:
            data[lang] = {}
            
        lang_data = data[lang]
        
        # Max attempts to fill all keys for this language
        for attempt_round in range(3):
            # Identification of missing or incomplete keys (those containing Japanese)
            missing_keys = []
            for k in all_keys:
                val = lang_data.get(k)
                if not val or (lang != 'ja' and has_japanese(val)):
                    if source_data.get(k):
                        missing_keys.append(k)
            
            if not missing_keys:
                print(f"    All keys (100%) are now complete and translated.")
                break
                
            print(f"    Attempt {attempt_round+1}: Found {len(missing_keys)} missing/incomplete keys.")
            
            for i in range(0, len(missing_keys), batch_size):
                batch_keys = missing_keys[i:i+batch_size]
            print(f"    All keys already present.")
            continue
            
        print(f"    Found {len(missing_keys)} missing keys.")
        
        for i in range(0, len(missing_keys), batch_size):
            batch_keys = missing_keys[i:i+batch_size]
            batch_to_translate = {k: source_data[k] for k in batch_keys}
            
            print(f"    Translating batch {i//batch_size + 1}/{(len(missing_keys)+batch_size-1)//batch_size}...", end=" ", flush=True)
            
            translations = translate_batch(batch_to_translate, lang)
            
            if translations:
                for k, v in translations.items():
                    if k in batch_keys:
                        lang_data[k] = v
                print("OK")
                updated = True
                
                # Intermediate save
                with open(file_path, 'w', encoding='utf-8') as f:
                    json.dump(data, f, ensure_ascii=False, indent=4)
            else:
                print("FAILED")
                time.sleep(2) # Wait a bit before next attempt
            
            time.sleep(0.5) # Rate limiting

    if updated:
        with open(file_path, 'w', encoding='utf-8') as f:
            json.dump(data, f, ensure_ascii=False, indent=4)
    
    print(f"Finished {file_path}")

if __name__ == "__main__":
    import argparse
    parser = argparse.ArgumentParser()
    parser.add_argument("files", nargs="*", help="Files to process (localizedText.json or lesson names)")
    parser.add_argument("--lang", help="Target specific language code (e.g., fr)")
    parser.add_argument("--batch", type=int, default=15, help="Batch size")
    args = parser.parse_args()

    files_to_process = []
    if args.files:
        for arg in args.files:
            if os.path.exists(arg):
                files_to_process.append(arg)
            elif os.path.exists(f'Assets/Resources/{arg}'):
                files_to_process.append(f'Assets/Resources/{arg}')
            elif os.path.exists(f'Assets/Resources/{arg}Lessons.json'):
                files_to_process.append(f'Assets/Resources/{arg}Lessons.json')
    else:
        files_to_process.append('Assets/Resources/localizedText.json')
        lessons_files = sorted([os.path.join('Assets/Resources', f) for f in os.listdir('Assets/Resources') if f.endswith('Lessons.json')])
        files_to_process.extend(lessons_files)

    if args.lang:
        if args.lang in TARGET_LANGUAGES:
            TARGET_LANGUAGES = [args.lang]
        else:
            print(f"Error: Unsupported language code '{args.lang}'")
            sys.exit(1)

    for file_path in files_to_process:
        process_file(file_path, batch_size=args.batch)
