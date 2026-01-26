
import os
import subprocess
import json
import time

def get_target_files():
    resources_dir = 'Assets/Resources'
    files = ['localizedText.json']
    lessons = [f for f in os.listdir(resources_dir) if f.endswith('Lessons.json')]
    files.extend(sorted(lessons))
    return files

def get_languages():
    # Use the same list as full_translate.py
    return ['en', 'cs', 'de', 'nl', 'da', 'el', 'fi', 'fr', 'it', 'ko', 'no', 'pl', 'pt', 'ro', 'ru', 'es', 'sv', 'tr', 'zh-Hans', 'zh-Hant']

def main():
    files = get_target_files()
    languages = get_languages()
    
    print(f"Starting complete translation process for {len(files)} files and {len(languages)} languages...")
    
    # Priority 1: localizedText.json
    # Priority 2: All other Lessons files
    
    for file_name in files:
        print(f"\n{'='*60}")
        print(f"WORKING ON: {file_name}")
        print(f"{'='*60}")
        
        for lang in languages:
            if lang == 'ja': continue # Skip source
            
            print(f"\n>>> File: {file_name}, Language: {lang}")
            
            # Call full_translate.py for this specific file and language
            # Using a smaller batch size for better reliability
            cmd = ['python', 'scripts/full_translate.py', file_name, '--lang', lang, '--batch', '12']
            
            try:
                # Set a reasonable timeout for each language/file combination
                subprocess.run(cmd, check=False)
            except Exception as e:
                print(f"Error executing translation: {e}")
            
            time.sleep(1) # Short break between languages

    print("\nAll files and languages have been processed at least once.")
    print("Run this script again to fill any remaining FAILED batches.")

if __name__ == "__main__":
    main()
