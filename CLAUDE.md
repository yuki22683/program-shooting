# Localization Rules

- **File Responsibility**:
  - `localizedText.json`: Global UI strings (menu, settings). No language-specific prefixes (e.g., `python_`).
  - `{language}Lessons.json`: Everything related to that programming language.
- **Key Distribution**:
  - Keys with language prefixes must live in their respective lesson files.
  - **Common Assets**: Keys that have the same value across all languages (specifically those ending in `_image`) **MUST** be placed in the `"common"` section of the JSON file.
  - No duplicate keys between `localizedText.json` and lesson files.
- **Verification**:
  - After any update, run the duplicate check and common key check scripts.
  - Ensure JSON syntax is valid.

# Data Sync Workflow (IMPORTANT)

When syncing lesson data from the reference site (`C:\Work\git\senkou-code`):

1. **Sync from reference**:
   ```bash
   python scripts/sync_from_reference.py [--language python]
   ```

2. **Validate data completeness**:
   ```bash
   python scripts/validate_lesson_data.py
   ```
   This script checks:
   - All exercises have required slides (title, content)
   - Sequential exercise numbering (no gaps)
   - Slide continuity (no missing slide numbers)

3. **Compare with reference** (optional):
   ```bash
   python scripts/check_reference_match.py
   ```

**Required Exercise Keys**:
Each exercise MUST have:
- `{lang}_lesson{N}_ex{M}_title`
- `{lang}_lesson{N}_ex{M}_description`
- `{lang}_lesson{N}_ex{M}_slide1_title`
- `{lang}_lesson{N}_ex{M}_slide1_content`
- (Additional slides as needed)

# Translation Workflow (IMPORTANT)

When running translation scripts:

1. **Run translation**:
   ```bash
   python scripts/batch_translate.py
   # or
   python scripts/translate_with_gemini.py --lang en
   ```

2. **ALWAYS validate JSON after translation**:
   ```bash
   python scripts/validate_all_json.py
   ```

3. **If validation fails, try automatic fix**:
   ```bash
   python scripts/fix_json_errors.py
   ```

**Common JSON Corruption Issues**:
- Smart quotes (`"` `"`) from translation APIs → Must be converted to regular quotes
- Actual newlines in strings → Must be escaped as `\n`
- Invalid escape sequences → Must be fixed

**Prevention**:
- Translation scripts now auto-sanitize output (see `json_utils.py`)
- Always run `validate_all_json.py` after any translation work
- Never commit JSON files without validation

# Recent Fixes (2026-01-27)
- Fixed JSON corruption in `pythonLessons_en.json` (smart quote escape sequences)
- Deleted 12 corrupted English lesson files (will need re-translation)
- Added `json_utils.py` with sanitization functions for translation scripts
- Updated `batch_translate.py` and `translate_with_gemini.py` to sanitize output
- Added `validate_all_json.py` for comprehensive JSON validation
- Fixed Haskell lesson1 missing slide data (ex1 slides were completely absent)
- Added `validate_lesson_data.py` script for data completeness checks
- Added `sync_from_reference.py` script for proper data synchronization
- Added `check_reference_match.py` script for comparing with reference site

# Recent Fixes (2026-01-26)
- Normalized all `*Lessons.json` to include 21 languages and a `"common"` section for images.
- Refactored `LocalizationManager.cs` to support the `"common"` section fallback.
- Cleaned up 1100+ duplicate keys from `localizedText.json`.
- Migrated language-prefixed keys from `localizedText.json` to lesson files.