
import json
import os
import re

def check_localization(file_path):
    with open(file_path, 'r', encoding='utf-8') as f:
        data = json.load(f)
    
    languages = list(data.keys())
    master_lang = 'en' if 'en' in languages else languages[0]
    master_keys = set(data[master_lang].keys())
    
    print(f"File: {file_path}")
    print(f"Languages: {languages}")
    print(f"Master keys count ({master_lang}): {len(master_keys)}")
    
    missing_report = {}
    for lang in languages:
        if lang == master_lang:
            continue
        
        current_keys = set(data[lang].keys())
        missing = master_keys - current_keys
        extra = current_keys - master_keys
        
        if missing or extra:
            missing_report[lang] = {
                "missing_count": len(missing),
                "extra_count": len(extra)
            }
            if len(missing) < 10:
                missing_report[lang]["missing_keys"] = list(missing)
                
    if missing_report:
        print("Missing/Extra keys report:")
        print(json.dumps(missing_report, indent=2))
    else:
        print("All keys are consistent across languages.")

if __name__ == "__main__":
    check_localization('Assets/Resources/localizedText.json')
