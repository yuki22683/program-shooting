# Project Instructions

## Unity MCP Integration

**重要**: このプロジェクトではUnity EditorとMCPサーバー経由で連携しています。

Unity関連の操作を行う際は必ず`mcp__unityMCP__`ツールを使用してください：
- シーン編集: `manage_scene`, `manage_gameobject`
- コンポーネント操作: `manage_components`
- スクリプト編集後: `refresh_unity`でコンパイル
- エラー確認: `read_console`

## Project Overview

- Meta Quest VR向けプログラミング学習アプリ
- ブロックを正しい順番で撃つことでコードを完成させる
- 主要スクリプト:
  - `LessonManager.cs` - レッスン/演習管理
  - `BlockShooter.cs` - ブロックのインタラクション
  - `RandomWalk.cs` - ブロックの移動AI
  - `TextBasedScaler.cs` - シンタックスハイライト

## Exercise Data Reference

演習データを設定する際は以下を参照：
- **参考サイト**: `C:\Work\git\senkou-code` - 演習内容の元データ（以降「参考サイト」と呼ぶ）
- **ローカライズ**: `Assets/Resources/localizedText.json` - 全言語対応のためJSONファイルで管理
  - 演習タイトル、コメント、説明文などはすべてローカライズキーで管理
  - 新しい演習を追加する際はlocalizedText.jsonにも対応するキーを追加すること

### 演習追加時の必須チェックリスト

**過去の失敗事例（2025年1月）**: 演習のタイトル・コメントキーは追加したが、スライドキーを追加し忘れた結果、スライド画面でJSONキー名がそのまま表示される不具合が発生。

**原因分析**:
1. `LessonManager.cs`の演習定義に`slideKeyPrefix`と`slideCount`が存在することを見落とした
2. `SlideManager.cs`がキーを動的に構築する仕組み（`{slideKeyPrefix}_slide{N}_title/content`）を理解していなかった
3. ローカライズキーの全体構造を把握せず、部分的な修正のみ行った
4. 修正後にPlayモードで実際のスライド表示を確認しなかった

**再発防止: 演習追加/更新時の必須確認項目**:

演習を追加または更新する際は、以下のすべてのキーが`localizedText.json`に存在することを確認すること：

```
1. 演習タイトル:     {prefix}_title          例: python_lesson1_ex2_title
2. コメント:         {prefix}_comment{N}     例: python_lesson1_ex2_comment1
3. スライドタイトル: {prefix}_slide{N}_title   例: python_lesson1_ex2_slide1_title  ★見落としやすい
4. スライド本文:     {prefix}_slide{N}_content 例: python_lesson1_ex2_slide1_content ★見落としやすい
```

**スライドキーの数は`LessonManager.cs`の`slideCount`に依存する**:
- `slideCount = 2` の場合: slide1_title, slide1_content, slide2_title, slide2_content が必要
- `slideCount = 3` の場合: 上記 + slide3_title, slide3_content が必要

**確認手順**:
1. `LessonManager.cs`のInitializeDefaultExercise()で演習定義を確認
2. 各演習の`slideKeyPrefix`と`slideCount`を把握
3. `localizedText.json`で対応するスライドキーがすべて存在するかGrepで確認
4. 不足があれば全21言語セクションに追加（日本語・英語は翻訳、他は英語フォールバック）
5. Playモードで実際にスライドを表示し、キー名ではなく本文が表示されることを確認

**過去の失敗事例（2026年1月追加）**: cpp、java、c言語の演習をLessonManager.csに追加し、slideCountも設定したが、localizedText.jsonにスライドキーを追加し忘れた。結果、スライド画面でキー名がそのまま表示された。

**スライドキー追加の検証コマンド**:
```bash
# 新しく追加した言語のスライドキーが存在するか確認
Grep: pattern="{lang}_lesson1_ex1_slide1_title" path=localizedText.json
```
このコマンドでヒットしない場合、スライドキーが未追加である。

**参考サイトからスライドデータを取得する際の注意**:
- 参考サイト（senkou-code）のtutorialSlides配列からスライド数を確認
- slideCount = tutorialSlidesの要素数（「やってみよう」ページは通常含まない）
- 各スライドのtitleとcontentをlocalizedText.jsonに追加

## 必須: 修正後の動作確認

**重要**: スクリプトやシーンを修正した後は、必ず以下の手順で動作確認を行うこと：

1. `refresh_unity` でコンパイル
2. `read_console` でコンパイルエラーがないことを確認
3. `manage_editor` で `action: play` を実行してPlayモードに入る
4. `read_console` で関連するログを確認し、修正が正しく反映されていることを検証
5. 問題があれば `manage_editor` で `action: stop` してから修正を行う
6. 動作確認が完了したら `manage_scene` で `action: save` してシーンを保存

**この手順を省略してはならない。修正後の動作確認は義務である。**

## 必須: 類似問題の再チェック

**重要**: 修正を実施した後は、プロジェクト内に同様の問題が他にないか必ず再チェックすること。

**過去の失敗事例**: `slideCount`の値が実際のスライド数と一致していない問題を1箇所だけ修正したが、同じ問題が他の演習にも存在していた（ex12, ex13も同様に`slideCount`が不足していた）。

**再発防止手順**:
1. 問題を修正したら、その問題のパターンを特定する
2. `Grep`ツールで同じパターンがプロジェクト内の他の箇所にないか検索する
3. 関連するすべての箇所を確認し、同様の修正が必要か判断する
4. 必要な修正をすべて適用してから動作確認を行う

**例**: `slideCount = 2`を`slideCount = 3`に修正した場合
- `Grep`で`slideCount = 2`を検索し、他にも修正が必要な箇所がないか確認
- 対応するlocalizedText.jsonのスライドキー数と照合

**一箇所だけ修正して終わりにしてはならない。類似問題の網羅的チェックは義務である。**

## 必須: ミス発生時の対策記録

**重要**: 修正を間違えた場合は、必ず以下を行うこと：

1. **原因分析**: なぜミスが発生したかを特定する
2. **対策をCLAUDE.mdに追記**: 同じミスを繰り返さないための対策をこのファイルに記録する
3. **対策の具体化**: 抽象的な「気をつける」ではなく、具体的な確認手順を記載する

**この手順を省略してはならない。ミスからの学習と記録は義務である。**

## 必須: 修正完了時のチェックリスト

**重要**: バグ修正や機能追加が完了したら、ユーザーに報告する**前に**以下をすべて実行すること：

```
□ 1. コンパイルエラーがないことを確認したか？
□ 2. Playモードで動作確認したか？
□ 3. シーンを保存したか？
□ 4. 【ミスがあった場合】CLAUDE.mdに再発防止策を追記したか？ ← 必須
□ 5. 類似の問題が他にないかGrepで確認したか？
```

**項目4は絶対に省略してはならない。**
- ミスがあった場合、CLAUDE.mdへの追記が完了するまでユーザーへの報告は禁止
- 「次から気をつけます」は対策ではない。具体的な確認手順を記載すること
- 追記後、ユーザーに追記内容を報告すること

**過去の失敗事例（2026年1月）**: 修正完了後、CLAUDE.mdへの再発防止策追記を忘れてユーザーに報告した。ユーザーに指摘されるまで気づかなかった。

**このチェックリストを無視した場合、同じミスを繰り返す。チェックリストの実行は義務である。**

## 必須: コード修正前の事前確認

**過去の失敗事例（2026年1月）**:
- UIコンポーネントのパスを推測で書いて間違えた（`TopBar/Horizontal/Horizontal/`→正しくは`TopBar/Horizontal/Vertical/Horizontal/`）
- 機能を処理しているスクリプトを確認せず、別のスクリプトを修正した（`SelectCoursePanelController`を修正したが、実際は`SelectCoursePanelManager`が処理していた）

**再発防止: 修正前の必須確認項目**:

1. **UIパス確認**: `find_gameobjects`でオブジェクトを検索し、`ReadMcpResourceTool`で正確なパスを確認する。推測でパスを書かない
2. **処理スクリプト確認**: 機能を修正する前に、`Grep`で関連する処理がどのスクリプトで行われているか確認する。同名のメソッドが複数のスクリプトにある場合は特に注意
3. **処理順序確認**: `SetActive(true)`でOnEnableが呼ばれる前にUI参照が必要な処理を行わない

**推測で修正を行ってはならない。事前確認は義務である。**

## 必須: 新しい言語追加時の全ファイル更新

**過去の失敗事例（2026年1月）**: LessonManager.csとlocalizedText.jsonに新しい言語（Java, C, C++, C#, Go）の演習データを追加したが、CourseButtonSpawner.csを更新し忘れた。その結果、言語選択画面でこれらの言語を選択してもコース選択画面でコースが表示されなかった。

**原因分析**:
1. CourseButtonSpawner.csの`InitializeCoursesForLanguage()`メソッドにPython/JavaScript/TypeScriptしか対応していなかった
2. 新しい言語を追加する際に、CourseButtonSpawner.csも更新が必要であることを認識していなかった
3. LessonManager.csとlocalizedText.jsonだけ更新すれば完了と思い込んでいた

**再発防止: 新しい言語追加時の必須更新ファイル一覧**:

新しいプログラミング言語を追加する際は、以下の**すべて**のファイルを更新すること：

| ファイル | 更新内容 |
|---------|---------|
| `LessonManager.cs` | `Initialize{Language}Lessons()`メソッドを追加、switch文にcaseを追加 |
| `localizedText.json` | 演習タイトル、コメント、スライドキーを追加（英語・日本語） |
| `CourseButtonSpawner.cs` | `Initialize{Language}Courses()`メソッドを追加、switch文にcaseを追加 ★見落としやすい |
| `ProgressManager.cs` | `GetLanguageString()`の言語インデックスを確認 |

**CourseButtonSpawner.csの更新内容**:
1. `InitializeCoursesForLanguage()`のswitch文に新しいcaseを追加
2. `Initialize{Language}Courses()`メソッドを追加（コース数、演習数、タイトルキー、説明キーを設定）

**確認手順**:
1. 言語追加後、Playモードで言語選択→コース選択の遷移を確認
2. コース選択画面で正しい数のコースボタンが表示されることを確認
3. コースタイトルがローカライズキー名ではなく実際のテキストで表示されることを確認

**LessonManager.csだけ更新して終わりにしてはならない。関連するすべてのファイルの更新は義務である。**

## 必須: 言語インデックスの一貫性確認

**過去の失敗事例（2026年1月）**: C言語を選択したのにC++のコースが表示された。原因は`SelectLanguagePanelController.cs`でCがC++と同じインデックス5を使用していたこと（コメントに"C uses same index as C++ for now"と書いてあった）。

**原因分析**:
1. 各言語には一意のインデックスが必要だが、CとC++が同じインデックスを共有していた
2. 言語インデックスの定義が複数のファイルに分散しており、整合性が取れていなかった
3. 既存コードの「for now」というコメントを見落とし、仮実装のまま放置されていた

**言語インデックスの正式な定義（ProgressManager.GetLanguageString）**:

| Index | Language | Index | Language |
|-------|----------|-------|----------|
| 0 | python | 10 | swift |
| 1 | javascript | 11 | kotlin |
| 2 | typescript | 12 | bash |
| 3 | java | 13 | sql |
| 4 | csharp | 14 | lua |
| 5 | cpp | 15 | perl |
| 6 | go | 16 | haskell |
| 7 | rust | 17 | elixir |
| 8 | ruby | 18 | assembly |
| 9 | php | 19 | c |

**再発防止: 言語インデックス変更時の必須確認項目**:

言語インデックスを追加・変更する際は、以下の**すべて**のファイルで整合性を確認すること：

1. `ProgressManager.cs` - `GetLanguageString()` ← 正式な定義
2. `SelectLanguagePanelController.cs` - `GetLanguageIndex()`
3. `LanguageSelectionManager.cs` - `GetLanguageIndex()`

**確認コマンド**: `Grep`で`return 19`や`case "c":`を検索し、すべてのファイルで同じ値を返すことを確認

**「for now」「TODO」「FIXME」などのコメントがあるコードは仮実装の可能性がある。本番前に必ず修正すること。**

## UI Panel命名規則

**重要**: パネル名を間違えないこと！

| パネル名 | 用途 |
|---------|------|
| **SelectLanguagePanel** | 言語選択パネル（Python, JavaScript等のプログラミング言語を選択） |
| **SelectCoursePanel** | コース選択パネル（選択した言語のコース一覧を表示） |
| **SelectLessonPanel** | レッスン選択パネル（コース内の演習一覧を表示） |

**過去の失敗事例（2026年1月）**: 「言語選択パネル」をSelectLessonPanelと間違えて修正した結果、全く違うパネルが表示される不具合が発生。

**パネル操作時の確認事項**:
1. パネル名を正確に確認する
2. SelectLanguagePanel = 言語選択、SelectLessonPanel = レッスン選択、名前が似ているので注意
3. 不明な場合はシーン内のオブジェクト構造を確認する

## Current Language

日本語で応答してください。
