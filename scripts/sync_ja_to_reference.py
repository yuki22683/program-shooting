#!/usr/bin/env python3
"""
Sync all *Lessons_ja.json files to match reference site data.
"""
import json
import sys
import io
from pathlib import Path

sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')

LOCAL_DIR = Path("C:/Work/MetaXR/ProgramShooting/Assets/Resources")
REFERENCE_FILE = Path("C:/Work/MetaXR/ProgramShooting/reference_ja_complete.json")

LANGUAGES = [
    "python", "javascript", "typescript", "java",
    "c", "cpp", "csharp", "assembly",
    "go", "rust", "ruby", "php",
    "swift", "kotlin", "bash", "sql",
    "lua", "perl", "haskell", "elixir"
]

def main():
    # Load reference data
    print("参考サイトデータ読み込み中...")
    with open(REFERENCE_FILE, 'r', encoding='utf-8') as f:
        all_ref_keys = json.load(f)
    print(f"参考サイトキー数: {len(all_ref_keys)}")

    total_added = 0
    total_updated = 0

    for lang in LANGUAGES:
        prefix = f"{lang}_"
        ref_lang_keys = {k: v for k, v in all_ref_keys.items() if k.startswith(prefix)}

        local_filename = f"{lang}Lessons_ja.json"
        local_path = LOCAL_DIR / local_filename

        # Load local file
        if local_path.exists():
            with open(local_path, 'r', encoding='utf-8') as f:
                local_data = json.load(f)
        else:
            local_data = {}

        added = 0
        updated = 0

        # Sync reference keys to local
        for key, ref_value in ref_lang_keys.items():
            if key not in local_data:
                local_data[key] = ref_value
                added += 1
            elif local_data[key] != ref_value:
                local_data[key] = ref_value
                updated += 1

        # Sort keys for consistency
        sorted_data = dict(sorted(local_data.items()))

        # Write back
        with open(local_path, 'w', encoding='utf-8') as f:
            json.dump(sorted_data, f, ensure_ascii=False, indent=2)

        print(f"[OK] {local_filename}: +{added} keys, ~{updated} updated")
        total_added += added
        total_updated += updated

    print(f"\n完了: {total_added} keys追加, {total_updated} keys更新")

if __name__ == "__main__":
    main()
