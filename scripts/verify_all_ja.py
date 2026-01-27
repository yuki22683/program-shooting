#!/usr/bin/env python3
"""
Verify all *Lessons_ja.json files against reference site's extracted data.
"""
import json
import re
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

def normalize_text(text):
    """Normalize text for comparison - handle whitespace and markdown differences"""
    if not text:
        return ""
    # Normalize whitespace
    text = re.sub(r'\s+', ' ', text.strip())
    return text

def compare_texts(local_text, ref_text):
    """Compare two texts, returning True if they are essentially the same"""
    local_norm = normalize_text(local_text)
    ref_norm = normalize_text(ref_text)
    return local_norm == ref_norm

def main():
    # Load reference data
    print("=" * 70)
    print("JA ファイル照合開始")
    print("=" * 70)

    with open(REFERENCE_FILE, 'r', encoding='utf-8') as f:
        all_ref_keys = json.load(f)

    print(f"参考サイトキー数: {len(all_ref_keys)}")

    results = {}
    checklist = []

    for lang in LANGUAGES:
        # Get reference keys for this language
        prefix = f"{lang}_"
        ref_lang_keys = {k: v for k, v in all_ref_keys.items() if k.startswith(prefix)}

        # Load local file
        local_filename = f"{lang}Lessons_ja.json"
        local_path = LOCAL_DIR / local_filename

        if not local_path.exists():
            results[local_filename] = {"status": "MISSING", "ref_count": len(ref_lang_keys)}
            checklist.append(f"[X] {local_filename} - ファイルなし (参考:{len(ref_lang_keys)})")
            continue

        with open(local_path, 'r', encoding='utf-8') as f:
            local_data = json.load(f)

        # Compare
        missing = []
        mismatch = []
        extra = []

        for key, ref_value in ref_lang_keys.items():
            if key not in local_data:
                missing.append(key)
            elif not compare_texts(local_data[key], ref_value):
                mismatch.append({
                    "key": key,
                    "local": local_data[key][:80] if local_data[key] else "",
                    "ref": ref_value[:80] if ref_value else ""
                })

        for key in local_data:
            if key not in ref_lang_keys:
                extra.append(key)

        # Determine status
        if missing or mismatch:
            status = "MISMATCH"
            icon = "[X]"
        elif extra:
            status = "EXTRA"
            icon = "[?]"
        else:
            status = "OK"
            icon = "[O]"

        results[local_filename] = {
            "status": status,
            "ref_count": len(ref_lang_keys),
            "local_count": len(local_data),
            "missing": missing,
            "mismatch": mismatch,
            "extra": extra
        }

        msg = f"{icon} {local_filename} - Ref:{len(ref_lang_keys)} Local:{len(local_data)}"
        if missing:
            msg += f" 欠損:{len(missing)}"
        if mismatch:
            msg += f" 不一致:{len(mismatch)}"
        if extra:
            msg += f" 余分:{len(extra)}"
        checklist.append(msg)

    # Print checklist
    print("\n" + "=" * 70)
    print("チェックリスト (全20ファイル)")
    print("=" * 70)
    for line in checklist:
        print(line)

    # Print details for issues
    print("\n" + "=" * 70)
    print("詳細情報 (問題があるファイル)")
    print("=" * 70)

    for filename, result in results.items():
        if result.get("status") in ["MISMATCH", "MISSING"]:
            print(f"\n【{filename}】")

            if result.get("missing"):
                print(f"  欠損キー ({len(result['missing'])}):")
                for key in result["missing"][:5]:
                    print(f"    - {key}")
                if len(result["missing"]) > 5:
                    print(f"    ... 他 {len(result['missing']) - 5} 件")

            if result.get("mismatch"):
                print(f"  値の不一致 ({len(result['mismatch'])}):")
                for item in result["mismatch"][:3]:
                    print(f"    - {item['key']}")
                    local_preview = item['local'][:60].replace('\n', '\\n') if item['local'] else "(empty)"
                    ref_preview = item['ref'][:60].replace('\n', '\\n') if item['ref'] else "(empty)"
                    print(f"      Local: {local_preview}...")
                    print(f"      Ref:   {ref_preview}...")
                if len(result["mismatch"]) > 3:
                    print(f"    ... 他 {len(result['mismatch']) - 3} 件")

    # Summary
    ok_count = sum(1 for r in results.values() if r.get("status") == "OK")
    extra_count = sum(1 for r in results.values() if r.get("status") == "EXTRA")
    mismatch_count = sum(1 for r in results.values() if r.get("status") == "MISMATCH")
    missing_count = sum(1 for r in results.values() if r.get("status") == "MISSING")

    print("\n" + "=" * 70)
    print("サマリー")
    print("=" * 70)
    print(f"[O] 完全一致: {ok_count}/20")
    print(f"[?] ローカルに追加キーあり: {extra_count}/20")
    print(f"[X] 不一致/欠損あり: {mismatch_count}/20")
    print(f"[-] ファイル欠損: {missing_count}/20")

    # Calculate total issues
    total_missing = sum(len(r.get("missing", [])) for r in results.values())
    total_mismatch = sum(len(r.get("mismatch", [])) for r in results.values())
    total_extra = sum(len(r.get("extra", [])) for r in results.values())

    print(f"\n問題キー合計:")
    print(f"  - 欠損キー: {total_missing}")
    print(f"  - 値不一致: {total_mismatch}")
    print(f"  - 余分キー: {total_extra}")

    # Save detailed report
    report_path = LOCAL_DIR.parent.parent / "ja_verification_final.json"
    with open(report_path, 'w', encoding='utf-8') as f:
        json.dump(results, f, ensure_ascii=False, indent=2)
    print(f"\n詳細レポート保存: {report_path}")

if __name__ == "__main__":
    main()
