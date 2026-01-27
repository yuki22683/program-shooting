import json
import os
import subprocess
import re

def call_gemini_fix(text):
    """日本語を含むテキストを完璧な英語に修正する"""
    prompt = f"Fix the following text to be perfect English for a programming tutorial. If it contains Japanese, translate it to natural English. Return ONLY the fixed English text.\n\nText: {text}"
    
    try:
        process = subprocess.Popen(
            'gemini --model gemini-2.0-flash-exp',
            stdin=subprocess.PIPE,
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
            text=True,
            encoding='utf-8',
            shell=True
        )
        stdout, stderr = process.communicate(input=prompt, timeout=60)
        return stdout.strip()
    except Exception as e:
        print(f"Error: {e}")
        return text

def process_files():
    dir_path = 'Assets/Resources'
    files = [f for f in os.listdir(dir_path) if f.endswith('Lessons.json')]
    
    for filename in files:
        file_path = os.path.join(dir_path, filename)
        with open(file_path, 'r', encoding='utf-8') as f:
            data = json.load(f)
        
        if 'en' not in data:
            continue
            
        modified = False
        print(f"Checking {filename}...")
        
        for key in data['en']:
            val = data['en'][key]
            # 日本語が含まれているかチェック
            if re.search(r'[\u3040-\u309F\u30A0-\u30FF\u4E00-\u9FFF]', val):
                print(f"  Fixing key: {key}")
                fixed_val = call_gemini_fix(val)
                # 万が一指示文が残っていたら除去
                fixed_val = fixed_val.replace('(Select "Start Lesson" when you are ready.)', '').strip()
                fixed_val = fixed_val + '\n\n(Select "Start Lesson" when you are ready.)' if '_slide' in key and ('ex' in key or 'lesson' in key) else fixed_val
                
                data['en'][key] = fixed_val
                modified = True
        
        if modified:
            with open(file_path, 'w', encoding='utf-8') as f:
                json.dump(data, f, indent=2, ensure_ascii=False)
            print(f"Saved {filename}")

if __name__ == "__main__":
    process_files()
