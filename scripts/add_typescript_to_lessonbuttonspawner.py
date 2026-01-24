#!/usr/bin/env python3
"""
Script to add TypeScript support to LessonButtonSpawner.cs
"""

import re

file_path = r"C:\Work\MetaXR\ProgramShooting\Assets\Scripts\LessonButtonSpawner.cs"

# Read the file
with open(file_path, 'r', encoding='utf-8') as f:
    content = f.read()

# 1. Add TypeScript case to switch statement in GetExercisesForLesson
# Find the switch statement and add typescript case after javascript
switch_pattern = r'(case "javascript":\s*\n\s*return GetJavaScriptExercisesForLesson\(lessonIndex\);)'
switch_replacement = r'''\1
            case "typescript":
                return GetTypeScriptExercisesForLesson(lessonIndex);'''

content = re.sub(switch_pattern, switch_replacement, content)

# 2. Add GetTypeScriptExercisesForLesson method after GetJavaScriptExercisesForLesson
typescript_exercises_method = '''
    /// <summary>
    /// Gets TypeScript exercise data for the specified lesson
    /// </summary>
    private List<ExerciseButtonData> GetTypeScriptExercisesForLesson(int lessonIndex)
    {
        switch (lessonIndex)
        {
            case 0: return GetTypeScriptLesson1Exercises();
            case 1: return GetTypeScriptLesson2Exercises();
            case 2: return GetTypeScriptLesson3Exercises();
            case 3: return GetTypeScriptLesson4Exercises();
            default: return GetTypeScriptLesson1Exercises();
        }
    }
'''

# Find end of GetJavaScriptExercisesForLesson and add after it
pattern_js_method = r'(private List<ExerciseButtonData> GetJavaScriptExercisesForLesson\(int lessonIndex\)\s*\{[^}]+\})'
content = re.sub(pattern_js_method, r'\1' + typescript_exercises_method, content)

# 3. Add TypeScript region with all lesson exercises before the closing of the class
typescript_region = '''
    #region TypeScript Exercises

    /// <summary>
    /// TypeScript Lesson 1 exercises (13 exercises, easy)
    /// </summary>
    private List<ExerciseButtonData> GetTypeScriptLesson1Exercises()
    {
        return new List<ExerciseButtonData>
        {
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex1_title", exerciseDescriptionKey = "typescript_lesson1_ex1_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex2_title", exerciseDescriptionKey = "typescript_lesson1_ex2_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex3_title", exerciseDescriptionKey = "typescript_lesson1_ex3_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex4_title", exerciseDescriptionKey = "typescript_lesson1_ex4_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex5_title", exerciseDescriptionKey = "typescript_lesson1_ex5_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex6_title", exerciseDescriptionKey = "typescript_lesson1_ex6_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex7_title", exerciseDescriptionKey = "typescript_lesson1_ex7_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex8_title", exerciseDescriptionKey = "typescript_lesson1_ex8_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex9_title", exerciseDescriptionKey = "typescript_lesson1_ex9_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex10_title", exerciseDescriptionKey = "typescript_lesson1_ex10_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex11_title", exerciseDescriptionKey = "typescript_lesson1_ex11_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex12_title", exerciseDescriptionKey = "typescript_lesson1_ex12_description", difficultyKey = "difficulty_easy" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson1_ex13_title", exerciseDescriptionKey = "typescript_lesson1_ex13_description", difficultyKey = "difficulty_easy" }
        };
    }

    /// <summary>
    /// TypeScript Lesson 2 exercises (10 exercises, medium)
    /// </summary>
    private List<ExerciseButtonData> GetTypeScriptLesson2Exercises()
    {
        return new List<ExerciseButtonData>
        {
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex1_title", exerciseDescriptionKey = "typescript_lesson2_ex1_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex2_title", exerciseDescriptionKey = "typescript_lesson2_ex2_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex3_title", exerciseDescriptionKey = "typescript_lesson2_ex3_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex4_title", exerciseDescriptionKey = "typescript_lesson2_ex4_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex5_title", exerciseDescriptionKey = "typescript_lesson2_ex5_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex6_title", exerciseDescriptionKey = "typescript_lesson2_ex6_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex7_title", exerciseDescriptionKey = "typescript_lesson2_ex7_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex8_title", exerciseDescriptionKey = "typescript_lesson2_ex8_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex9_title", exerciseDescriptionKey = "typescript_lesson2_ex9_description", difficultyKey = "difficulty_medium" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson2_ex10_title", exerciseDescriptionKey = "typescript_lesson2_ex10_description", difficultyKey = "difficulty_medium" }
        };
    }

    /// <summary>
    /// TypeScript Lesson 3 exercises (10 exercises, hard)
    /// </summary>
    private List<ExerciseButtonData> GetTypeScriptLesson3Exercises()
    {
        return new List<ExerciseButtonData>
        {
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex1_title", exerciseDescriptionKey = "typescript_lesson3_ex1_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex2_title", exerciseDescriptionKey = "typescript_lesson3_ex2_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex3_title", exerciseDescriptionKey = "typescript_lesson3_ex3_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex4_title", exerciseDescriptionKey = "typescript_lesson3_ex4_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex5_title", exerciseDescriptionKey = "typescript_lesson3_ex5_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex6_title", exerciseDescriptionKey = "typescript_lesson3_ex6_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex7_title", exerciseDescriptionKey = "typescript_lesson3_ex7_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex8_title", exerciseDescriptionKey = "typescript_lesson3_ex8_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex9_title", exerciseDescriptionKey = "typescript_lesson3_ex9_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson3_ex10_title", exerciseDescriptionKey = "typescript_lesson3_ex10_description", difficultyKey = "difficulty_hard" }
        };
    }

    /// <summary>
    /// TypeScript Lesson 4 exercises (10 exercises, hard)
    /// </summary>
    private List<ExerciseButtonData> GetTypeScriptLesson4Exercises()
    {
        return new List<ExerciseButtonData>
        {
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex1_title", exerciseDescriptionKey = "typescript_lesson4_ex1_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex2_title", exerciseDescriptionKey = "typescript_lesson4_ex2_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex3_title", exerciseDescriptionKey = "typescript_lesson4_ex3_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex4_title", exerciseDescriptionKey = "typescript_lesson4_ex4_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex5_title", exerciseDescriptionKey = "typescript_lesson4_ex5_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex6_title", exerciseDescriptionKey = "typescript_lesson4_ex6_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex7_title", exerciseDescriptionKey = "typescript_lesson4_ex7_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex8_title", exerciseDescriptionKey = "typescript_lesson4_ex8_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex9_title", exerciseDescriptionKey = "typescript_lesson4_ex9_description", difficultyKey = "difficulty_hard" },
            new ExerciseButtonData { exerciseTitleKey = "typescript_lesson4_ex10_title", exerciseDescriptionKey = "typescript_lesson4_ex10_description", difficultyKey = "difficulty_hard" }
        };
    }

    #endregion
'''

# Find the #endregion of JavaScript Exercises and add TypeScript region after it
# Insert before SpawnLessonButton method
insert_pattern = r'(#endregion\s*\n)(\s*/// <summary>\s*\n\s*/// Spawns a single lesson button)'
content = re.sub(insert_pattern, r'\1' + typescript_region + r'\n\2', content)

# Write the file
with open(file_path, 'w', encoding='utf-8') as f:
    f.write(content)

print("Successfully added TypeScript support to LessonButtonSpawner.cs")
