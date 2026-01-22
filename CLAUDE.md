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

## 必須: 修正後の動作確認

**重要**: スクリプトやシーンを修正した後は、必ず以下の手順で動作確認を行うこと：

1. `refresh_unity` でコンパイル
2. `read_console` でコンパイルエラーがないことを確認
3. `manage_editor` で `action: play` を実行してPlayモードに入る
4. `read_console` で関連するログを確認し、修正が正しく反映されていることを検証
5. 問題があれば `manage_editor` で `action: stop` してから修正を行う
6. 動作確認が完了したら `manage_scene` で `action: save` してシーンを保存

**この手順を省略してはならない。修正後の動作確認は義務である。**

## Current Language

日本語で応答してください。
