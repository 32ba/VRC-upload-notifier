# VRChat Upload Notifier

VRChat SDKでアバター/ワールドをアップロードした際に、OSネイティブ通知で完了・失敗を知らせるUnity Editor拡張です。

## 特徴

- アップロード成功時・失敗時にデスクトップ通知を表示
- macOS / Windows / Linux対応
- VRChat SDK 3.0+に対応
- Project Settingsから設定をカスタマイズ可能

## インストール

### VCC (VRChat Creator Companion) 経由

1. VCCで「Add Repository」からリポジトリURLを追加：
   ```
   https://32ba.github.io/vpm-repo/index.json
   ```
2. プロジェクトの「Manage Project」から「VRChat Upload Notifier」を追加

### Unity Package Manager (Git URL)

1. Window > Package Manager を開く
2. 「+」ボタン > 「Add package from git URL...」を選択
3. 以下のURLを入力：
   ```
   https://github.com/32ba/VRC-upload-notifier.git
   ```

### 手動インストール

1. [Releases](https://github.com/32ba/VRC-upload-notifier/releases) から `.unitypackage` をダウンロード
2. Unityプロジェクトにインポート

## 使い方

インストール後、VRChat SDKでアバターやワールドをアップロードすると自動的に通知が表示されます。

### 設定

**Edit > Project Settings > VRChat Upload Notifier** から以下の設定が可能です：

| 設定項目 | 説明 |
|---------|------|
| Enable Notifications | 通知のON/OFF |
| Play Sound | 通知音の再生 |
| Notify on Success | アップロード成功時の通知 |
| Notify on Error | アップロード失敗時の通知 |
| Show Content ID | 成功時にContent IDを表示 |

### テスト通知

Project Settingsの「Send Test Notification」ボタンで通知が正しく動作するか確認できます。

### 再初期化

VRChat SDK Control Panelを開く前にUnityを起動した場合、イベント購読が行われないことがあります。その場合は **Tools > VRChat Upload Notifier > Reinitialize Event Subscription** を実行してください。

## 対応プラットフォーム

| プラットフォーム | 通知方式 |
|----------------|---------|
| macOS | Notification Center (osascript) |
| Windows 10/11 | Toast Notification (PowerShell) |
| Linux | notify-send (libnotify) |

## 動作要件

- Unity 2019.4以降
- VRChat SDK 3.0.0以降

## ライセンス

MIT License - 詳細は [LICENSE](LICENSE) を参照してください。
