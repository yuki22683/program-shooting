#!/usr/bin/env python3
"""
Verify all *Lessons_ja.json files against reference site's extracted_slides_ja.json
"""
import json
import os
import sys
import io
from pathlib import Path

# Fix Windows console encoding
sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')

# Paths
REFERENCE_FILE = Path("C:/Work/git/senkou-code/data/lessons/extracted_slides_ja.json")
LOCAL_DIR = Path("C:/Work/MetaXR/ProgramShooting/Assets/Resources")

# Language prefixes
LANGUAGE_PREFIXES = [
    "python_", "javascript_", "typescript_", "java_",
    "c_", "cpp_", "csharp_", "assembly_",
    "go_", "rust_", "ruby_", "php_",
    "swift_", "kotlin_", "bash_", "sql_",
    "lua_", "perl_", "haskell_", "elixir_"
]

PREFIX_TO_FILE = {
    "python_": "pythonLessons_ja.json",
    "javascript_": "javascriptLessons_ja.json",
    "typescript_": "typescriptLessons_ja.json",
    "java_": "javaLessons_ja.json",
    "c_": "cLessons_ja.json",
    "cpp_": "cppLessons_ja.json",
    "csharp_": "csharpLessons_ja.json",
    "assembly_": "assemblyLessons_ja.json",
    "go_": "goLessons_ja.json",
    "rust_": "rustLessons_ja.json",
    "ruby_": "rubyLessons_ja.json",
    "php_": "phpLessons_ja.json",
    "swift_": "swiftLessons_ja.json",
    "kotlin_": "kotlinLessons_ja.json",
    "bash_": "bashLessons_ja.json",
    "sql_": "sqlLessons_ja.json",
    "lua_": "luaLessons_ja.json",
    "perl_": "perlLessons_ja.json",
    "haskell_": "haskellLessons_ja.json",
    "elixir_": "elixirLessons_ja.json",
}

def get_prefix_for_key(key):
    """Get the prefix for a key"""
    for prefix in LANGUAGE_PREFIXES:
        if key.startswith(prefix):
            return prefix
    return None

def main():
    # Load reference data
    print(f"Loading reference file: {REFERENCE_FILE}")
    with open(REFERENCE_FILE, 'r', encoding='utf-8') as f:
        reference_data = json.load(f)

    print(f"Reference keys: {len(reference_data)}")

    # Group reference keys by prefix
    reference_by_prefix = {}
    for key, value in reference_data.items():
        prefix = get_prefix_for_key(key)
        if prefix:
            if prefix not in reference_by_prefix:
                reference_by_prefix[prefix] = {}
            reference_by_prefix[prefix][key] = value

    # Load and compare each local file
    results = {}
    all_files_checked = []

    for prefix, filename in PREFIX_TO_FILE.items():
        local_path = LOCAL_DIR / filename
        lang_name = prefix.rstrip('_')

        if not local_path.exists():
            results[filename] = {
                "status": "MISSING",
                "message": f"File not found: {local_path}"
            }
            all_files_checked.append(f"❌ {filename} - ファイルが存在しない")
            continue

        # Load local data
        with open(local_path, 'r', encoding='utf-8') as f:
            local_data = json.load(f)

        ref_keys = reference_by_prefix.get(prefix, {})

        # Compare
        missing_keys = []
        extra_keys = []
        mismatched_values = []

        # Check for missing keys and mismatched values
        for key, ref_value in ref_keys.items():
            if key not in local_data:
                missing_keys.append(key)
            elif local_data[key] != ref_value:
                mismatched_values.append({
                    "key": key,
                    "local": local_data[key][:100] + "..." if len(local_data[key]) > 100 else local_data[key],
                    "reference": ref_value[:100] + "..." if len(ref_value) > 100 else ref_value
                })

        # Check for extra keys (in local but not in reference)
        for key in local_data:
            if key not in ref_keys:
                extra_keys.append(key)

        # Determine status
        if missing_keys or mismatched_values:
            status = "MISMATCH"
            status_icon = "❌"
        elif extra_keys:
            status = "EXTRA_KEYS"
            status_icon = "⚠️"
        else:
            status = "OK"
            status_icon = "✅"

        results[filename] = {
            "status": status,
            "ref_count": len(ref_keys),
            "local_count": len(local_data),
            "missing_keys": missing_keys,
            "extra_keys": extra_keys,
            "mismatched_values": mismatched_values
        }

        msg = f"{status_icon} {filename} - Ref:{len(ref_keys)} Local:{len(local_data)}"
        if missing_keys:
            msg += f" Missing:{len(missing_keys)}"
        if mismatched_values:
            msg += f" Mismatch:{len(mismatched_values)}"
        if extra_keys:
            msg += f" Extra:{len(extra_keys)}"
        all_files_checked.append(msg)

    # Print summary
    print("\n" + "=" * 60)
    print("チェックリスト - JA ファイル照合結果")
    print("=" * 60)
    for line in all_files_checked:
        print(line)

    # Print details for mismatched files
    print("\n" + "=" * 60)
    print("詳細情報")
    print("=" * 60)

    for filename, result in results.items():
        if result["status"] in ["MISMATCH", "EXTRA_KEYS"]:
            print(f"\n【{filename}】")

            if result.get("missing_keys"):
                print(f"  欠損キー ({len(result['missing_keys'])}):")
                for key in result["missing_keys"][:10]:
                    print(f"    - {key}")
                if len(result["missing_keys"]) > 10:
                    print(f"    ... 他 {len(result['missing_keys']) - 10} 件")

            if result.get("mismatched_values"):
                print(f"  値の不一致 ({len(result['mismatched_values'])}):")
                for item in result["mismatched_values"][:5]:
                    print(f"    - {item['key']}")
                    print(f"      Local: {item['local']}")
                    print(f"      Ref:   {item['reference']}")
                if len(result["mismatched_values"]) > 5:
                    print(f"    ... 他 {len(result['mismatched_values']) - 5} 件")

            if result.get("extra_keys"):
                print(f"  ローカルのみに存在 ({len(result['extra_keys'])}):")
                for key in result["extra_keys"][:10]:
                    print(f"    - {key}")
                if len(result["extra_keys"]) > 10:
                    print(f"    ... 他 {len(result['extra_keys']) - 10} 件")

    # Count results
    ok_count = sum(1 for r in results.values() if r["status"] == "OK")
    mismatch_count = sum(1 for r in results.values() if r["status"] == "MISMATCH")
    extra_count = sum(1 for r in results.values() if r["status"] == "EXTRA_KEYS")
    missing_count = sum(1 for r in results.values() if r["status"] == "MISSING")

    print("\n" + "=" * 60)
    print("サマリー")
    print("=" * 60)
    print(f"✅ 完全一致: {ok_count}/20")
    print(f"❌ 不一致: {mismatch_count}/20")
    print(f"⚠️ ローカルに追加キーあり: {extra_count}/20")
    print(f"🚫 ファイル欠損: {missing_count}/20")

    # Save detailed report
    report_path = LOCAL_DIR.parent.parent / "ja_verification_report.json"
    with open(report_path, 'w', encoding='utf-8') as f:
        json.dump(results, f, ensure_ascii=False, indent=2)
    print(f"\n詳細レポート保存: {report_path}")

if __name__ == "__main__":
    main()
