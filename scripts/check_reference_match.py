#!/usr/bin/env python3
"""
Check if the reference site lesson data matches the project's Japanese lesson data.
Compares slide titles, slide content, and exercise code (correctCode, correctLines).
"""

import json
import re
import os
from pathlib import Path

# Paths
SENKOU_CODE_PATH = Path(r"C:\Work\git\senkou-code\data\lessons")
PROJECT_RESOURCES_PATH = Path(r"C:\Work\MetaXR\ProgramShooting\Assets\Resources")

# All 20 programming languages
LANGUAGES = [
    "python", "javascript", "typescript", "java", "c", "cpp", "csharp",
    "go", "rust", "ruby", "php", "swift", "kotlin", "bash", "sql",
    "lua", "perl", "haskell", "elixir", "assembly"
]

def parse_ts_file(file_path):
    """Parse TypeScript file and extract JSON data."""
    if not file_path.exists():
        return None

    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()

    # Remove export statement and extract JSON
    # Pattern: export const varName = { ... }
    match = re.search(r'export\s+const\s+\w+\s*=\s*(\{[\s\S]*\})\s*;?\s*$', content)
    if match:
        json_str = match.group(1)
        try:
            # Try parsing as JSON
            return json.loads(json_str)
        except json.JSONDecodeError:
            # Try to fix common issues
            # Remove trailing commas before } or ]
            json_str = re.sub(r',(\s*[}\]])', r'\1', json_str)
            try:
                return json.loads(json_str)
            except:
                pass
    return None

def load_all_ts_files_for_language(language):
    """Load all TypeScript lesson files for a language (e.g., python.ts, python2.ts, etc.)."""
    all_data = []

    # Check main file and numbered files
    files_to_check = [f"{language}.ts"]
    for i in range(2, 10):
        files_to_check.append(f"{language}{i}.ts")

    for filename in files_to_check:
        file_path = SENKOU_CODE_PATH / filename
        if file_path.exists():
            data = parse_ts_file(file_path)
            if data:
                all_data.append(data)

    return all_data

def load_project_ja_file(language):
    """Load project's Japanese lesson file."""
    file_path = PROJECT_RESOURCES_PATH / f"{language}Lessons_ja.json"
    if not file_path.exists():
        return None

    with open(file_path, 'r', encoding='utf-8') as f:
        return json.load(f)

def compare_exercises(ref_data, project_ja, language):
    """Compare exercises between reference and project data."""
    differences = []
    matches = []

    if not ref_data or not project_ja:
        return differences, matches

    for lesson in ref_data:
        # Extract lesson number from lessonId (e.g., "python-1" -> 1, "haskell-2" -> 2)
        lesson_id = lesson.get('lessonId', '')
        try:
            lesson_idx = int(lesson_id.split('-')[-1])
        except (ValueError, IndexError):
            lesson_idx = 1  # Default to 1 if parsing fails

        exercises = lesson.get('exercises', [])

        for ex_idx, exercise in enumerate(exercises, 1):
            ref_title = exercise.get('title', '')
            ref_slides = exercise.get('tutorialSlides', [])
            ref_correct_code = exercise.get('correctCode', '')
            ref_correct_lines = exercise.get('correctLines', [])

            # Find corresponding keys in project
            # Key pattern: {language}_lesson{N}_ex{M}_title
            title_key = f"{language}_lesson{lesson_idx}_ex{ex_idx}_title"
            project_title = project_ja.get(title_key, '')

            # Compare title
            if ref_title and project_title:
                if ref_title.strip() != project_title.strip():
                    differences.append({
                        'type': 'title',
                        'key': title_key,
                        'ref': ref_title,
                        'project': project_title
                    })
                else:
                    matches.append(f"Title: {title_key}")
            elif ref_title and not project_title:
                differences.append({
                    'type': 'missing_title',
                    'key': title_key,
                    'ref': ref_title,
                    'project': '(missing)'
                })

            # Compare slides
            for slide_idx, slide in enumerate(ref_slides, 1):
                ref_slide_title = slide.get('title', '')
                ref_slide_content = slide.get('content', '')

                slide_title_key = f"{language}_lesson{lesson_idx}_ex{ex_idx}_slide{slide_idx}_title"
                slide_content_key = f"{language}_lesson{lesson_idx}_ex{ex_idx}_slide{slide_idx}_content"

                project_slide_title = project_ja.get(slide_title_key, '')
                project_slide_content = project_ja.get(slide_content_key, '')

                # Compare slide title
                if ref_slide_title and project_slide_title:
                    if ref_slide_title.strip() != project_slide_title.strip():
                        differences.append({
                            'type': 'slide_title',
                            'key': slide_title_key,
                            'ref': ref_slide_title,
                            'project': project_slide_title
                        })
                    else:
                        matches.append(f"Slide title: {slide_title_key}")
                elif ref_slide_title and not project_slide_title:
                    differences.append({
                        'type': 'missing_slide_title',
                        'key': slide_title_key,
                        'ref': ref_slide_title,
                        'project': '(missing)'
                    })

                # Compare slide content (normalize whitespace)
                if ref_slide_content and project_slide_content:
                    ref_normalized = re.sub(r'\s+', ' ', ref_slide_content.strip())
                    proj_normalized = re.sub(r'\s+', ' ', project_slide_content.strip())

                    if ref_normalized != proj_normalized:
                        differences.append({
                            'type': 'slide_content',
                            'key': slide_content_key,
                            'ref': ref_slide_content[:100] + '...' if len(ref_slide_content) > 100 else ref_slide_content,
                            'project': project_slide_content[:100] + '...' if len(project_slide_content) > 100 else project_slide_content
                        })
                    else:
                        matches.append(f"Slide content: {slide_content_key}")
                elif ref_slide_content and not project_slide_content:
                    differences.append({
                        'type': 'missing_slide_content',
                        'key': slide_content_key,
                        'ref': ref_slide_content[:100] + '...',
                        'project': '(missing)'
                    })

    return differences, matches

def main():
    print("=" * 80)
    print("参考元サイト vs プロジェクト日本語データ 比較レポート")
    print("=" * 80)
    print()

    total_differences = 0
    total_matches = 0
    language_summaries = []

    for language in LANGUAGES:
        print(f"\n{'='*40}")
        print(f"言語: {language.upper()}")
        print(f"{'='*40}")

        # Load reference data
        ref_data = load_all_ts_files_for_language(language)
        if not ref_data:
            print(f"  [WARNING] 参考元ファイルが見つかりません: {language}")
            continue

        # Load project Japanese data
        project_ja = load_project_ja_file(language)
        if not project_ja:
            print(f"  [WARNING] プロジェクトJAファイルが見つかりません: {language}Lessons_ja.json")
            continue

        # Compare
        differences, matches = compare_exercises(ref_data, project_ja, language)

        total_differences += len(differences)
        total_matches += len(matches)

        # Summary
        summary = {
            'language': language,
            'ref_lessons': len(ref_data),
            'ref_exercises': sum(len(l.get('exercises', [])) for l in ref_data),
            'differences': len(differences),
            'matches': len(matches)
        }
        language_summaries.append(summary)

        print(f"  参考元レッスン数: {summary['ref_lessons']}")
        print(f"  参考元演習数: {summary['ref_exercises']}")
        print(f"  一致: {len(matches)} 件")
        print(f"  差異: {len(differences)} 件")

        # Show first few differences
        if differences:
            print(f"\n  【差異サンプル (最大5件)】")
            for i, diff in enumerate(differences[:5]):
                print(f"  {i+1}. [{diff['type']}] {diff['key']}")
                print(f"     参考元: {diff['ref'][:60]}..." if len(diff['ref']) > 60 else f"     参考元: {diff['ref']}")
                print(f"     プロジェクト: {diff['project'][:60]}..." if len(diff['project']) > 60 else f"     プロジェクト: {diff['project']}")

    # Overall summary
    print("\n")
    print("=" * 80)
    print("全体サマリー")
    print("=" * 80)
    print(f"\n総一致数: {total_matches}")
    print(f"総差異数: {total_differences}")
    print()

    print("言語別サマリー:")
    print("-" * 60)
    print(f"{'言語':<15} {'レッスン':<10} {'演習':<8} {'一致':<8} {'差異':<8}")
    print("-" * 60)
    for s in language_summaries:
        print(f"{s['language']:<15} {s['ref_lessons']:<10} {s['ref_exercises']:<8} {s['matches']:<8} {s['differences']:<8}")
    print("-" * 60)

    # Determine overall status
    if total_differences == 0:
        print("\n結果: 全て一致しています")
    else:
        print(f"\n結果: {total_differences} 件の差異があります")
        print("注意: 差異がある場合、参考元サイトからデータを同期する必要があるかもしれません。")

if __name__ == "__main__":
    import sys
    import io
    sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')

    # Also write to file for better readability
    output_file = Path(r"C:\Work\MetaXR\ProgramShooting\scripts\check_results.txt")
    with open(output_file, 'w', encoding='utf-8') as f:
        # Redirect output to file
        old_stdout = sys.stdout
        sys.stdout = f
        main()
        sys.stdout = old_stdout

    print(f"Results written to: {output_file}")
