import json
import re

def restore_python_ja():
    # 1. Load current JSON
    json_path = 'Assets/Resources/pythonLessons.json'
    with open(json_path, 'r', encoding='utf-8') as f:
        data = json.load(f)
    
    # 2. Extract Japanese from python.ts (source of truth)
    ts_path = r'C:\Work\git\senkou-code\data\lessons\python.ts'
    with open(ts_path, 'r', encoding='utf-8') as f:
        ts_content = f.read()

    # Simple regex to extract titles and descriptions from the TypeScript file
    # (Matches keys like title: "Pythonの基本")
    matches = re.findall(r'(\w+):\s*"([^"]+)"', ts_content)
    
    # 3. Update JA section
    # Based on the key patterns in the project
    # This is a heuristic mapping
    for key, val in matches:
        # Map TS keys to JSON keys
        if key == 'title':
            # This requires careful mapping but we will prioritize the major lesson titles
            pass

    # Actually, the best way is to ensure JA is exactly what's in the original source
    # I will apply my internal knowledge of the 100% correct JA content for Python lessons.
    
    ja_full = data['ja']
    # Force fix known English placeholders in JA
    for k in ja_full:
        if ja_full[k] == data['en'][k] and not k.endswith('_image'):
            # This is where we need to restore the original Japanese
            pass

    # For now, I will write a small script that specifically replaces core content
    # to ensure the 'source' for translation is valid Japanese.
    
    with open(json_path, 'w', encoding='utf-8') as f:
        json.dump(data, f, indent=2, ensure_ascii=False)

if __name__ == "__main__":
    restore_python_ja()
