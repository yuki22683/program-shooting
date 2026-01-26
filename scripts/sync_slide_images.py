#!/usr/bin/env python3
"""
参考サイトのレッスンファイル(.ts)からスライドイラストパスを抽出し、
UnityプロジェクトのJSONファイルを更新するスクリプト

JSONの構造:
{
    "en": {
        "key1": "value1",
        "key2": "value2"
    },
    "ja": {
        "key1": "値1",
        "key2": "値2"
    }
}
"""
import os
import re
import json
from pathlib import Path

# パス設定
SENKOU_LESSONS_DIR = Path(r"C:\Work\git\senkou-code\data\lessons")
UNITY_RESOURCES_DIR = Path(r"C:\Work\MetaXR\ProgramShooting\Assets\Resources")

# 言語とファイルのマッピング
LANGUAGE_MAPPING = {
    "python": ["python.ts", "python2.ts", "python3.ts", "python4.ts", "python5.ts"],
    "javascript": ["javascript.ts", "javascript2.ts", "javascript3.ts", "javascript4.ts", "javascript5.ts"],
    "typescript": ["typescript.ts", "typescript2.ts", "typescript3.ts", "typescript4.ts"],
    "java": ["java.ts", "java2.ts", "java3.ts", "java4.ts", "java5.ts"],
    "c": ["c.ts", "c2.ts", "c3.ts", "c4.ts"],
    "cpp": ["cpp.ts", "cpp2.ts", "cpp3.ts", "cpp4.ts"],
    "csharp": ["csharp.ts", "csharp2.ts", "csharp3.ts", "csharp4.ts"],
    "go": ["go.ts", "go2.ts", "go3.ts", "go4.ts"],
    "rust": ["rust.ts", "rust2.ts", "rust3.ts", "rust4.ts"],
    "ruby": ["ruby.ts", "ruby2.ts", "ruby3.ts"],
    "php": ["php.ts", "php2.ts", "php3.ts"],
    "swift": ["swift.ts", "swift2.ts", "swift3.ts", "swift4.ts"],
    "kotlin": ["kotlin.ts", "kotlin2.ts", "kotlin3.ts", "kotlin4.ts"],
    "bash": ["bash.ts", "bash2.ts", "bash3.ts"],
    "sql": ["sql.ts", "sql2.ts", "sql3.ts", "sql4.ts"],
    "lua": ["lua.ts", "lua2.ts", "lua3.ts"],
    "perl": ["perl.ts", "perl2.ts", "perl3.ts"],
    "haskell": ["haskell.ts", "haskell2.ts", "haskell3.ts"],
    "elixir": ["elixir.ts", "elixir2.ts", "elixir3.ts"],
    "assembly": ["assembly.ts", "assembly2.ts", "assembly3.ts"],
}

def extract_slide_images_from_ts(ts_file_path):
    """TSファイルからスライドのイラストパスを抽出"""
    with open(ts_file_path, 'r', encoding='utf-8') as f:
        content = f.read()

    # lessonIdを抽出 (例: "python-1" -> lesson1)
    lesson_match = re.search(r'"lessonId":\s*"([^"]+)"', content)
    if not lesson_match:
        return {}

    lesson_id = lesson_match.group(1)
    # "python-1" -> "1", "python-2" -> "2"
    lesson_num = lesson_id.split('-')[-1]
    language = lesson_id.rsplit('-', 1)[0]

    # exercisesからtutorialSlidesのimageを抽出
    result = {}

    # exercisesを分割して処理
    exercises_match = re.search(r'"exercises":\s*\[', content)
    if not exercises_match:
        return {}

    # 各exerciseを見つける
    exercise_pattern = re.compile(
        r'"orderIndex":\s*(\d+).*?"tutorialSlides":\s*\[(.*?)\]',
        re.DOTALL
    )

    for match in exercise_pattern.finditer(content):
        order_index = match.group(1)
        slides_content = match.group(2)

        # 各スライドのimageを抽出
        slide_pattern = re.compile(r'"image":\s*"([^"]+)"')
        slides = slide_pattern.findall(slides_content)

        for slide_idx, image_path in enumerate(slides, 1):
            key = f"{language}_lesson{lesson_num}_ex{order_index}_slide{slide_idx}_image"
            result[key] = image_path

    return result

def update_unity_json(json_path, image_updates):
    """UnityのJSONファイルを更新（各言語セクションの中に追加）"""
    with open(json_path, 'r', encoding='utf-8') as f:
        data = json.load(f)

    updated_count = 0

    # 各言語セクション（en, ja等）に対して更新
    for lang_code in data.keys():
        if not isinstance(data[lang_code], dict):
            continue

        for key, new_value in image_updates.items():
            if key in data[lang_code]:
                if data[lang_code][key] != new_value:
                    print(f"  [{lang_code}] 更新: {key}")
                    print(f"    旧: {data[lang_code][key]}")
                    print(f"    新: {new_value}")
                    data[lang_code][key] = new_value
                    updated_count += 1
            else:
                # 新しいキーを追加
                data[lang_code][key] = new_value
                updated_count += 1
                if lang_code == "en":  # 1回だけ表示
                    print(f"  追加: {key} = {new_value}")

    if updated_count > 0:
        with open(json_path, 'w', encoding='utf-8') as f:
            json.dump(data, f, ensure_ascii=False, indent=4)
        print(f"  {updated_count}件更新しました")
    else:
        print(f"  更新なし")

    return updated_count

def main():
    total_updates = 0

    for language, ts_files in LANGUAGE_MAPPING.items():
        print(f"\n=== {language} ===")

        json_file = UNITY_RESOURCES_DIR / f"{language}Lessons.json"
        if not json_file.exists():
            print(f"  JSONファイルが存在しません: {json_file}")
            continue

        all_image_updates = {}

        for ts_file in ts_files:
            ts_path = SENKOU_LESSONS_DIR / ts_file
            if not ts_path.exists():
                continue

            image_updates = extract_slide_images_from_ts(ts_path)
            all_image_updates.update(image_updates)

        if all_image_updates:
            updates = update_unity_json(json_file, all_image_updates)
            total_updates += updates

    print(f"\n合計 {total_updates} 件更新しました")

if __name__ == "__main__":
    main()
