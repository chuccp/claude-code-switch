# claude-code-switch

快速切换 Claude Code 的 API 配置工具 | Quickly switch Claude Code API configurations

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Avalonia](https://img.shields.io/badge/Avalonia-11.1.3-009688?logo=avalonia)](https://avaloniaui.net/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

---

## 📖 简介 | Introduction

**claude-code-switch** 是一个基于 Avalonia UI 的桌面应用程序，用于快速切换和管理 Claude Code 的 API 配置。支持多套配置方案，一键切换不同的 API 端点、认证令牌和模型设置。

**claude-code-switch** is a cross-platform desktop application for quickly switching and managing Claude Code API configurations. Supports multiple configuration profiles with one-click switching.

---

## ✨ 功能特性 | Features

- 🔄 **快速切换** - 在不同 API 配置之间一键切换
- 📝 **多配置管理** - 支持创建、编辑、删除多个配置方案
- 🎨 **深色/浅色主题** - 支持深色和浅色主题切换
- 💾 **自动保存** - 配置变更自动保存
- 🔧 **灵活配置** - 支持自定义 API 端点、认证令牌、模型、超时等参数
- 🌐 **跨平台** - 基于 Avalonia UI，支持 Windows、Linux、macOS

---

## 🚀 快速开始 | Quick Start

### 系统要求 | Requirements

- .NET 9.0 SDK 或更高版本
- Windows 10/11, Linux, 或 macOS

### 构建 | Build

```bash
# 克隆仓库
git clone https://github.com/your-username/claude-code-switch.git
cd claude-code-switch

# 还原依赖
dotnet restore

# 构建项目
dotnet build

# 运行应用
dotnet run --project claude-code-switch/claude-code-switch.csproj
```

### 发布 | Publish

```bash
# 发布为单文件应用 (Windows x64)
dotnet publish claude-code-switch/claude-code-switch.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish
```

---

## 📖 使用说明 | Usage

### 配置说明 | Configuration

应用使用两个配置文件：

| 文件 | 说明 |
|------|------|
| `config.json` | 存储多套 API 配置方案 |
| `appl.toml` | 存储应用设置（主题、窗口位置等） |

### 配置参数 | Configuration Parameters

| 参数 | 说明 | 默认值 |
|------|------|--------|
| `anthropic_auth_token` | API 认证令牌 | - |
| `anthropic_base_url` | API 基础 URL | `https://api.anthropic.com` |
| `anthropic_model` | 使用的模型名称 | - |
| `api_timeout_ms` | API 超时时间（毫秒） | `3000000` |
| `claude_code_disable_nonessential_traffic` | 禁用非必要流量 | `1` |

### 快捷键 | Keyboard Shortcuts

| 快捷键 | 功能 |
|--------|------|
| `Ctrl + S` | 保存当前配置 |
| `Ctrl + D` | 切换深色/浅色主题 |

---

## 🛠️ 技术栈 | Tech Stack

- **[Avalonia UI 11.1](https://avaloniaui.net/)** - 跨平台 UI 框架
- **.NET 9.0** - 运行时环境
- **ReactiveUI** - MVVM 框架
- **Tomlyn** - TOML 配置文件解析
- **System.Text.Json** - JSON 序列化/反序列化

---

## 📁 项目结构 | Project Structure

```
claude-code-switch/
├── claude-code-switch/       # 主项目目录
│   ├── Models/               # 数据模型
│   ├── Views/                # UI 视图
│   ├── ViewModels/           # 视图模型
│   ├── Controls/             # 自定义控件
│   ├── Converters/           # 值转换器
│   ├── Services/             # 服务层
│   ├── Theme/                # 主题资源
│   ├── App.axaml             # 应用资源字典
│   ├── Program.cs            # 程序入口
│   └── claude-code-switch.csproj
├── config.json               # 配置文件
├── appl.toml                 # 应用设置
└── README.md
```

---

## 📝 许可证 | License

本项目采用 MIT 许可证。详见 [LICENSE](LICENSE) 文件。

---

## 🤝 贡献 | Contributing

欢迎提交 Issue 和 Pull Request！

1. Fork 本仓库
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 开启 Pull Request

---

## 📧 联系方式 | Contact

如有问题或建议，请通过 GitHub Issues 联系我们。
