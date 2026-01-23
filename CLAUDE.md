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
