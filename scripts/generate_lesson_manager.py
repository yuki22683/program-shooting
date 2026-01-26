#!/usr/bin/env python3
"""
Generate LessonManager.cs code from senkou-code source data
"""
import json
import re
from pathlib import Path

def parse_ts_file(filepath):
    """Parse TypeScript file and extract JSON data"""
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()

    json_start = content.find('{')
    if json_start == -1:
        return None
    json_str = content[json_start:].rstrip()

    if json_str.endswith(';'):
        json_str = json_str[:-1]

    try:
        return json.loads(json_str)
    except json.JSONDecodeError as e:
        print(f"  Warning: Failed to parse {filepath}: {e}")
        return None

def get_source_files(source_base, language):
    """Get all source files for a language"""
    files = []
    first_file = source_base / f'{language}.ts'
    if first_file.exists():
        files.append(first_file)
    for i in range(2, 20):
        numbered_file = source_base / f'{language}{i}.ts'
        if numbered_file.exists():
            files.append(numbered_file)
    return files

def get_comment_prefix(language):
    """Get comment prefix for a language"""
    if language in ['python', 'ruby', 'perl', 'bash', 'elixir']:
        return '#'
    elif language in ['assembly']:
        return ';'
    elif language in ['sql', 'lua', 'haskell']:
        return '--'
    else:
        return '//'

def escape_csharp_string(s):
    """Escape a string for C# string literal"""
    if s is None:
        return ''
    s = s.replace('\\', '\\\\')
    s = s.replace('"', '\\"')
    s = s.replace('\n', '\\n')
    s = s.replace('\r', '')
    s = s.replace('\t', '\\t')
    return s

def extract_correct_lines(exercise, language):
    """Extract correct lines from exercise data"""
    correct_lines = exercise.get('correctLines', [])
    if not correct_lines:
        # Fallback to correctCode
        correct_code = exercise.get('correctCode', '')
        if correct_code:
            correct_lines = correct_code.split('\n')

    result = []
    for line in correct_lines:
        if isinstance(line, list):
            # Multiple valid answers - use first one
            line = line[0] if line else ''
        if isinstance(line, dict):
            # Skip dict entries
            continue
        if line is None:
            line = ''
        result.append(escape_csharp_string(str(line)))
    return result

def extract_comments(correct_lines, language):
    """Extract comment info from correct lines"""
    comment_prefix = get_comment_prefix(language)
    comments = []

    for i, line in enumerate(correct_lines):
        stripped = line.strip() if isinstance(line, str) else ''
        if stripped.startswith(comment_prefix):
            comments.append({
                'lineIndex': i,
                'prefix': comment_prefix,
                'commentNum': len(comments) + 1
            })
    return comments

def generate_method_for_language(language, source_files, source_base):
    """Generate Initialize{Language}Lessons method"""

    # Map language to method name
    method_names = {
        'python': 'Python',
        'javascript': 'JavaScript',
        'typescript': 'TypeScript',
        'java': 'Java',
        'c': 'C',
        'cpp': 'Cpp',
        'csharp': 'CSharp',
        'go': 'Go',
        'rust': 'Rust',
        'ruby': 'Ruby',
        'php': 'PHP',
        'swift': 'Swift',
        'kotlin': 'Kotlin',
        'bash': 'Bash',
        'sql': 'SQL',
        'lua': 'Lua',
        'perl': 'Perl',
        'haskell': 'Haskell',
        'elixir': 'Elixir',
        'assembly': 'Assembly'
    }

    method_name = method_names.get(language, language.capitalize())

    lines = []
    lines.append(f'    private void Initialize{method_name}Lessons()')
    lines.append('    {')
    lines.append('        lessons.Clear();')
    lines.append('')

    for lesson_idx, source_file in enumerate(source_files):
        lesson_num = lesson_idx + 1

        source_data = parse_ts_file(source_file)
        if source_data is None:
            continue

        exercises = source_data.get('exercises', [])
        lesson_title = source_data.get('lessonTitle', f'{language} Lesson {lesson_num}')

        lines.append(f'        // ==================== LESSON {lesson_num}: {lesson_title} ====================')
        lines.append(f'        var lesson{lesson_num} = new Lesson {{ titleKey = "{language}_lesson{lesson_num}_title" }};')
        lines.append('')

        for ex_idx, exercise in enumerate(exercises):
            ex_num = ex_idx + 1
            title = exercise.get('title', f'Exercise {ex_num}')
            slides = exercise.get('tutorialSlides', [])
            slide_count = len(slides) + 1  # +1 for "Let's try it" slide

            correct_lines = extract_correct_lines(exercise, language)
            comments = extract_comments(correct_lines, language)

            # Generate exercise
            lines.append(f'        // Ex{ex_num}: {title}')
            lines.append(f'        lesson{lesson_num}.exercises.Add(new Exercise')
            lines.append('        {')
            lines.append(f'            titleKey = "{language}_lesson{lesson_num}_ex{ex_num}_title",')
            lines.append(f'            slideKeyPrefix = "{language}_lesson{lesson_num}_ex{ex_num}",')
            lines.append(f'            slideCount = {slide_count},')

            # correctLines
            if correct_lines:
                correct_lines_str = ', '.join([f'"{line}"' for line in correct_lines])
                lines.append(f'            correctLines = new List<string> {{ {correct_lines_str} }},')
            else:
                lines.append('            correctLines = new List<string>(),')

            # comments
            if comments:
                lines.append('            comments = new List<LocalizedComment>')
                lines.append('            {')
                for c in comments:
                    lines.append(f'                new LocalizedComment {{ lineIndex = {c["lineIndex"]}, commentPrefix = "{c["prefix"]}", localizationKey = "{language}_lesson{lesson_num}_ex{ex_num}_comment{c["commentNum"]}" }},')
                lines.append('            },')
            else:
                lines.append('            comments = new List<LocalizedComment>(),')

            # expectedOutput (empty for now)
            lines.append('            expectedOutput = new List<string>()')
            lines.append('        });')
            lines.append('')

        lines.append(f'        lessons.Add(lesson{lesson_num});')
        lines.append('')

    lines.append('        currentLessonIndex = 0;')
    lines.append('        currentExerciseIndex = 0;')
    lines.append('    }')

    return '\n'.join(lines)


def main():
    source_base = Path('C:/Work/git/senkou-code/data/lessons')
    output_dir = Path('C:/Work/MetaXR/ProgramShooting/scripts/generated')
    output_dir.mkdir(exist_ok=True)

    # Languages to generate (excluding existing ones that are already in LessonManager.cs)
    new_languages = [
        'bash', 'csharp', 'elixir', 'go', 'haskell', 'kotlin',
        'lua', 'perl', 'php', 'ruby', 'rust', 'sql', 'swift'
    ]

    for language in new_languages:
        source_files = get_source_files(source_base, language)
        if not source_files:
            print(f"No source files for {language}")
            continue

        print(f"Generating {language}...")
        code = generate_method_for_language(language, source_files, source_base)

        output_file = output_dir / f'{language}_lessons.cs'
        with open(output_file, 'w', encoding='utf-8') as f:
            f.write(code)

        print(f"  Written to {output_file}")

    print("\nDone! Check scripts/generated/ for the generated code.")

if __name__ == '__main__':
    main()
