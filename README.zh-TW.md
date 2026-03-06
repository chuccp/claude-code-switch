# Claude Code Switch

[简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [English](README.md)

一個基於 Avalonia UI 的跨平台桌面應用程式。

## 下載

從 [GitHub Releases](https://github.com/chuccp/claude-code-switch/releases) 下載最新版本。

> **Mac 和 Linux 使用者注意事項：**
> - **Linux**：需要執行 `chmod +x claude-code-switch` 賦予可執行權限。
> - **macOS**：應用程式已打包為 `.app` 套件。首次啟動時，可能需要在 `系統偏好設定 > 隱私權與安全性` 中點擊「仍要打開」以允許應用程式執行。

## 專案截圖

![應用截圖](images/83ecfdf5-d907-4979-8c6d-def15320c4fc.png)

## 技術棧

- **.NET 10.0**
- **Avalonia UI 11.1.3** - 跨平台 UI 框架
- **Avalonia ReactiveUI** - 響應式 UI 模式
- **Tomlyn** - TOML 設定檔解析

## 主要特性

- 🎨 **Fluent 設計主題** - 現代化的 UI 風格
- 🌙 **深色/淺色主題切換** - 支援主題切換
- 📱 **跨平台支援** - Windows、Linux、macOS
- 🔧 **可配置視窗設定** - 支援自訂視窗大小和位置

## 專案結構

```
claude-code-switch/
├── Controls/          # 自訂控制項
├── Converters/        # 資料轉換器
├── Models/            # 資料模型
├── Services/          # 服務層
├── Theme/             # 主題樣式
├── ViewModels/        # 視圖模型
├── Views/             # 視圖頁面
└── Program.cs         # 應用程式入口
```

## 開發環境要求

- .NET 10.0 SDK
- 支援 Avalonia UI 的 IDE（如 Visual Studio、Rider 或 VS Code）

## 建構與執行

```bash
# 還原相依性
dotnet restore

# 建構專案
dotnet build

# 執行應用
dotnet run --project claude-code-switch/claude-code-switch.csproj
```

## 設定檔

應用程式使用 `appl.toml` 進行設定：

```toml
debug_mode = false
theme = "Dark"
[window]
width = 800
height = 600
x = 1320
y = 594
[dialog]
width = 700
height = 550
```

## 授權

本專案採用 MIT 授權條款。詳見 [LICENSE](LICENSE) 檔案。
