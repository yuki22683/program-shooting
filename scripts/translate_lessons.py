#!/usr/bin/env python3
"""
Translate Japanese lesson content to English.
"""

import json
import glob
import os
import re

# 翻訳辞書
TRANSLATIONS = {
    # 共通フレーズ
    "やってみよう！": "Let's Try It!",
    "やってみましょう！": "Let's Try It!",
    "目標": "Goal",
    "完成形": "Complete Code",
    "チェックポイント": "Checklist",
    "準備ができたら「レッスン開始」を選択してください。": 'When you are ready, select "Start Lesson".',
    "（準備ができたら「レッスン開始」を選択してください。）": '(When you are ready, select "Start Lesson".)',

    # 見出しパターン
    "とは？": "?",
    "って何？": "?",
    "の使い方": " Usage",
    "の書き方": " Syntax",
    "について": "",

    # プログラミング用語
    "変数": "variable",
    "関数": "function",
    "配列": "array",
    "リスト": "list",
    "辞書": "dictionary",
    "文字列": "string",
    "数値": "number",
    "整数": "integer",
    "小数": "decimal",
    "真偽値": "boolean",
    "条件分岐": "conditional",
    "繰り返し": "loop",
    "ループ": "loop",
    "クラス": "class",
    "オブジェクト": "object",
    "メソッド": "method",
    "プロパティ": "property",
    "引数": "argument",
    "戻り値": "return value",
    "コメント": "comment",
    "出力": "output",
    "入力": "input",
    "表示": "display",
    "画面": "screen",
    "実行": "execute",
    "プログラム": "program",
    "コード": "code",
    "構文": "syntax",
    "演算子": "operator",
    "型": "type",
    "宣言": "declaration",
    "定義": "definition",
    "代入": "assignment",
    "比較": "comparison",
    "論理": "logical",
    "算術": "arithmetic",
    "インデックス": "index",
    "要素": "element",
    "キー": "key",
    "値": "value",
    "ペア": "pair",
    "タプル": "tuple",
    "セット": "set",
    "集合": "set",
    "イテレータ": "iterator",
    "ジェネレータ": "generator",
    "例外": "exception",
    "エラー": "error",
    "デバッグ": "debug",
    "テスト": "test",
    "モジュール": "module",
    "パッケージ": "package",
    "ライブラリ": "library",
    "インポート": "import",
    "エクスポート": "export",
    "名前空間": "namespace",
    "スコープ": "scope",
    "グローバル": "global",
    "ローカル": "local",
    "静的": "static",
    "動的": "dynamic",
    "継承": "inheritance",
    "多態性": "polymorphism",
    "カプセル化": "encapsulation",
    "抽象": "abstract",
    "インターフェース": "interface",
    "実装": "implementation",
    "コンストラクタ": "constructor",
    "デストラクタ": "destructor",
    "ポインタ": "pointer",
    "参照": "reference",
    "アドレス": "address",
    "メモリ": "memory",
    "ヒープ": "heap",
    "スタック": "stack",
    "再帰": "recursion",
    "アルゴリズム": "algorithm",
    "データ構造": "data structure",
    "ソート": "sort",
    "検索": "search",
    "フィルタ": "filter",
    "マップ": "map",
    "リデュース": "reduce",
    "コールバック": "callback",
    "クロージャ": "closure",
    "ラムダ": "lambda",
    "無名関数": "anonymous function",
    "高階関数": "higher-order function",
    "純粋関数": "pure function",
    "副作用": "side effect",
    "イミュータブル": "immutable",
    "ミュータブル": "mutable",
    "非同期": "asynchronous",
    "同期": "synchronous",
    "並行": "concurrent",
    "並列": "parallel",
    "スレッド": "thread",
    "プロセス": "process",
    "ファイル": "file",
    "読み込み": "read",
    "書き込み": "write",
    "開く": "open",
    "閉じる": "close",

    # 説明的なフレーズ
    "たとえば": "For example",
    "例えば": "For example",
    "つまり": "In other words",
    "なぜなら": "Because",
    "したがって": "Therefore",
    "しかし": "However",
    "また": "Also",
    "さらに": "Furthermore",
    "最初に": "First",
    "次に": "Next",
    "最後に": "Finally",
    "注意": "Note",
    "ポイント": "Point",
    "ヒント": "Hint",
    "大事": "Important",
    "重要": "Important",
    "基本": "Basic",
    "応用": "Advanced",
    "練習": "Practice",
    "解説": "Explanation",
    "説明": "Explanation",
    "特徴": "Feature",
    "利点": "Advantage",
    "欠点": "Disadvantage",
    "使い方": "Usage",
    "書き方": "Syntax",
    "例": "Example",
    "結果": "Result",
    "出力結果": "Output",

    # 動詞
    "する": "do",
    "使う": "use",
    "使います": "use",
    "書く": "write",
    "書きます": "write",
    "読む": "read",
    "読みます": "read",
    "作る": "create",
    "作ります": "create",
    "追加する": "add",
    "追加します": "add",
    "削除する": "delete",
    "削除します": "delete",
    "変更する": "change",
    "変更します": "change",
    "確認する": "check",
    "確認します": "check",
    "実行する": "execute",
    "実行します": "execute",
    "表示する": "display",
    "表示します": "display",
    "出力する": "output",
    "出力します": "output",
    "入力する": "input",
    "入力します": "input",
    "定義する": "define",
    "定義します": "define",
    "宣言する": "declare",
    "宣言します": "declare",
    "呼び出す": "call",
    "呼び出します": "call",
    "返す": "return",
    "返します": "return",
    "繰り返す": "repeat",
    "繰り返します": "repeat",
    "比較する": "compare",
    "比較します": "compare",
    "計算する": "calculate",
    "計算します": "calculate",
}

# タイトル翻訳辞書（特定のタイトルパターン）
TITLE_TRANSLATIONS = {
    "画面に文字を出してみましょう": "Display Text on Screen",
    "計算をしてみましょう": "Let's Do Some Math",
    "変数を使ってみましょう": "Using Variables",
    "条件分岐を使ってみましょう": "Using Conditionals",
    "繰り返しを使ってみましょう": "Using Loops",
    "関数を作ってみましょう": "Creating Functions",
    "配列を使ってみましょう": "Using Arrays",
    "リストを使ってみましょう": "Using Lists",
    "辞書を使ってみましょう": "Using Dictionaries",
    "クラスを作ってみましょう": "Creating Classes",
    "ファイルを扱ってみましょう": "Working with Files",
    "エラー処理を学びましょう": "Learning Error Handling",
}


def protect_code_blocks(text):
    """Protect code blocks from translation."""
    code_blocks = []

    def save_block(match):
        code_blocks.append(match.group(0))
        return f"<<<CODEBLOCK{len(code_blocks)-1}>>>"

    # Protect fenced code blocks
    result = re.sub(r'```[\s\S]*?```', save_block, text)
    # Protect inline code
    result = re.sub(r'`[^`]+`', save_block, result)

    return result, code_blocks


def restore_code_blocks(text, code_blocks):
    """Restore protected code blocks."""
    for i, block in enumerate(code_blocks):
        text = text.replace(f"<<<CODEBLOCK{i}>>>", block)
    return text


def translate_text(text):
    """Translate Japanese text to English."""
    if not text:
        return text

    # Protect code blocks
    result, code_blocks = protect_code_blocks(text)

    # Apply translations (longer phrases first to avoid partial matches)
    sorted_translations = sorted(TRANSLATIONS.items(), key=lambda x: -len(x[0]))
    for ja, en in sorted_translations:
        result = result.replace(ja, en)

    # Restore code blocks
    result = restore_code_blocks(result, code_blocks)

    return result


def translate_title(text):
    """Translate title text."""
    if not text:
        return text

    # Check for exact matches first
    if text in TITLE_TRANSLATIONS:
        return TITLE_TRANSLATIONS[text]

    # Apply general translations
    return translate_text(text)


def process_file(filepath):
    """Process a single lessons JSON file."""
    with open(filepath, 'r', encoding='utf-8') as f:
        data = json.load(f)

    lang = os.path.basename(filepath).replace('Lessons.json', '')

    en_data = data.get('en', {})
    ja_data = data.get('ja', {})

    translated = 0
    for key, ja_value in ja_data.items():
        if key not in en_data or en_data[key] == ja_value:
            # Translate based on key type
            if '_title' in key:
                en_data[key] = translate_title(ja_value)
            else:
                en_data[key] = translate_text(ja_value)
            translated += 1

    data['en'] = en_data

    with open(filepath, 'w', encoding='utf-8') as f:
        json.dump(data, f, ensure_ascii=False, indent=4)

    return translated


def main():
    files = glob.glob('Assets/Resources/*Lessons.json')
    total = 0

    for f in files:
        count = process_file(f)
        lang = os.path.basename(f).replace('Lessons.json', '')
        print(f"{lang}: {count} keys translated")
        total += count

    print(f"\nTotal: {total} keys translated")


if __name__ == '__main__':
    main()
