# Claude Code Switch

[![GitHub release](https://img.shields.io/github/v/release/chuccp/claude-code-switch?include_prereleases)](https://github.com/chuccp/claude-code-switch/releases)
[![GitHub downloads](https://img.shields.io/github/downloads/chuccp/claude-code-switch/total)](https://github.com/chuccp/claude-code-switch/releases)
[![GitHub stars](https://img.shields.io/github/stars/chuccp/claude-code-switch)](https://github.com/chuccp/claude-code-switch/stargazers)
[![License](https://img.shields.io/github/license/chuccp/claude-code-switch)](LICENSE)
[![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blue)](https://github.com/chuccp/claude-code-switch/releases)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-13.0-239120)](https://docs.microsoft.com/en-us/dotnet/csharp/)

[简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [English](README.md)

一个基于 Avalonia UI 的跨平台桌面应用程序。

## 下载

从 [GitHub Releases](https://github.com/chuccp/claude-code-switch/releases) 下载最新版本。

> **Mac 和 Linux 用户注意事项：**
> - **Linux**：需要运行 `chmod +x claude-code-switch` 赋予可执行权限。
> - **macOS**：应用已打包为 `.app` 包。首次启动时，可能需要在 `系统偏好设置 > 隐私与安全性` 中点击"仍要打开"以允许应用运行。

## 项目截图

![应用截图](images/83ecfdf5-d907-4979-8c6d-def15320c4fc.png)

## 技术栈

- **.NET 10.0**
- **Avalonia UI 11.1.3** - 跨平台 UI 框架
- **Avalonia ReactiveUI** - 响应式 UI 模式
- **Tomlyn** - TOML 配置文件解析

## 主要特性

- 🎨 **Fluent 设计主题** - 现代化的 UI 风格
- 🌙 **深色/浅色主题切换** - 支持主题切换
- 📱 **跨平台支持** - Windows、Linux、macOS
- 🔧 **可配置窗口设置** - 支持自定义窗口大小和位置

## 项目结构

```
claude-code-switch/
├── Controls/          # 自定义控件
├── Converters/        # 数据转换器
├── Models/            # 数据模型
├── Services/          # 服务层
├── Theme/             # 主题样式
├── ViewModels/        # 视图模型
├── Views/             # 视图页面
└── Program.cs         # 应用程序入口
```

## 开发环境要求

- .NET 10.0 SDK
- 支持 Avalonia UI 的 IDE（如 Visual Studio、Rider 或 VS Code）

## 构建与运行

```bash
# 还原依赖
dotnet restore

# 构建项目
dotnet build

# 运行应用
dotnet run --project claude-code-switch/claude-code-switch.csproj
```

## 配置文件

应用程序使用 `appl.toml` 进行配置：

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

## 许可证

本项目采用 MIT 许可证。详见 [LICENSE](LICENSE) 文件。
