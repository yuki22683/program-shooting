#!/usr/bin/env python3
"""
Append generated lesson methods to LessonManager.cs
"""
from pathlib import Path

def main():
    generated_dir = Path('C:/Work/MetaXR/ProgramShooting/scripts/generated')
    lesson_manager = Path('C:/Work/MetaXR/ProgramShooting/Assets/Scripts/LessonManager.cs')

    # Languages to add (excluding go and csharp which already exist)
    languages = ['rust', 'ruby', 'php', 'swift', 'kotlin', 'bash', 'sql', 'lua', 'perl', 'haskell', 'elixir']

    # Read current content
    with open(lesson_manager, 'r', encoding='utf-8') as f:
        content = f.read()

    # Find the position before the last closing brace
    # The file ends with:
    #     #endregion
    # }

    # Find the last occurrence of "#endregion" followed by "}"
    insert_pos = content.rfind('#endregion')
    if insert_pos == -1:
        # Fallback: find last closing brace
        insert_pos = content.rfind('}')

    # Collect all method content
    methods_content = []
    for lang in languages:
        gen_file = generated_dir / f'{lang}_lessons.cs'
        if gen_file.exists():
            with open(gen_file, 'r', encoding='utf-8') as f:
                method_code = f.read()
            methods_content.append(f'\n{method_code}\n')
            print(f"Added {lang}")
        else:
            print(f"Warning: {gen_file} not found")

    # Insert before #endregion
    new_content = content[:insert_pos] + '\n'.join(methods_content) + '\n    ' + content[insert_pos:]

    # Write back
    with open(lesson_manager, 'w', encoding='utf-8') as f:
        f.write(new_content)

    print(f"\nDone! Added {len(methods_content)} language methods to LessonManager.cs")

if __name__ == '__main__':
    main()
