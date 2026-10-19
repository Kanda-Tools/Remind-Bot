# DISCORD ボイスチャット開始時に通知するBOT


## 機能

- サーバー内のボイスチャンネルで通話が開始されたことを検知
- 指定したテキストチャンネルへ通知メッセージを送信
- 設定は `.env` ファイルまたは環境変数で管理


## 動作環境

- .NET 6.0 以上
- Discord Bot アカウント


### 使用ライブラリ
 
| ライブラリ | 用途 |
| --- | --- |
| [Discord.Net](https://github.com/discord-net/Discord.Net) | Discord Gateway / API クライアント |
| [dotenv.net](https://github.com/bolorundurowb/dotenv.net) | `.env` ファイルの読み込み |


## セットアップ

### 1. Discord Bot の作成

1. [Discord Developer Portal](https://discord.com/developers/applications) で **New Application** を作成
2. **Bot** タブで Bot を追加し、**Token** をコピー
3. **Bot** タブの **Privileged Gateway Intents** を設定
   - 現在の実装は `GatewayIntents.All` を指定しているため、次の 3 つをすべて有効化してください
     - Presence Intent
     - Server Members Intent
     - Message Content Intent
   - ボイスチャンネルの検知のみが目的であれば、`GatewayIntents.Guilds | GatewayIntents.GuildVoiceStates` への絞り込みを推奨します（特権 Intent が不要になります）
4. **OAuth2 > URL Generator** で以下を選択し、生成された URL から Bot をサーバーに招待
   - Scopes: `bot`
   - Bot Permissions: `View Channels` / `Send Messages`


### 2. 通知先チャンネル ID の取得

1. Discord の **ユーザー設定 > 詳細設定** で **開発者モード** を有効化
2. 通知先のテキストチャンネルを右クリックし、**チャンネル ID をコピー**


### 3. 環境変数の設定

プロジェクトのルートに `.env` ファイルを作成します。
 
```env
DISCORD_TOKEN=your-bot-token
DISCORD_NOTIFY_CHANNEL_ID=123456789012345678
```
 
| 変数名 | 必須 | 説明 |
| --- | --- | --- |
| `DISCORD_TOKEN` | ○ | Bot のトークン |
| `DISCORD_NOTIFY_CHANNEL_ID` | ○ | 通知を送信するテキストチャンネルの ID |
 
> **注意**
> `.env` にはトークンが含まれます。必ず `.gitignore` に追加し、リポジトリにコミットしないでください。
>
> ```gitignore
> .env
> ```


### 4. 依存パッケージのインストール
 
```bash
dotnet add package Discord.Net
dotnet add package dotenv.net
```

## 実行方法
 
```bash
dotnet run
```
 
起動すると `.env` を読み込み、Discord に Bot としてログインします。
 

## プロジェクト構成
 
```
ReminderBot/
├── Program.cs   # エントリーポイント。.env を読み込み Bot を起動
├── Bot.cs       # Discord クライアントの初期化とログイン処理
├── .env         # トークン・チャンネル ID（Git 管理外）
└── README.md
```


### 処理の流れ
 
1. `Program.Main` が `DotEnv.Load()` で `.env` を読み込む
2. `Bot` のコンストラクタで環境変数からトークンと通知先チャンネル ID を取得し、`DiscordSocketClient` を生成
3. `Bot.RunAsync` で Discord にログイン


## 実装状況
 
現時点のコードは骨組みの状態です。以下は今後の実装予定です。
 
- [ ] `StartAsync` による Gateway 接続と常駐処理（`Task.Delay(-1)` など）
- [ ] `LogAsync` によるログ出力（`_client.Log`）
- [ ] `OnReadyAsync` による起動完了通知（`_client.Ready`）
- [ ] `UserVoiceStateUpdated` イベントによる通話開始の検知と通知メッセージの送信
- [ ] 環境変数が未設定の場合のエラーハンドリング
- [ ] `GatewayIntents` の最小化


## トラブルシューティング
 
| 症状 | 確認ポイント |
| --- | --- |
| 起動時に `ArgumentNullException` / `FormatException` が出る | `.env` の変数名・値が正しいか、`.env` が実行ディレクトリにあるか |
| `Disallowed intents` エラーで接続できない | Developer Portal で特権 Intent を有効化しているか |
| 通知が届かない | Bot が通知先チャンネルを閲覧・送信できる権限を持っているか |
 
 
## ライセンス
 
任意のライセンスを記載してください（例: MIT License）。