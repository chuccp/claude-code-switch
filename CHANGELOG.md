# 更新日志

## [未发布]

### 新增
- 状态栏右侧添加"⌘ 终端"按钮，可快速打开命令窗口并执行 `claude` 命令
- 支持多平台终端打开方式：
  - **Windows**: 使用 cmd.exe
  - **macOS**: 使用 AppleScript 调用 Terminal 应用
  - **Linux**: 自动检测并支持多种终端模拟器（gnome-terminal、konsole、xfce4-terminal、xterm、alacritty、kitty）

### 修改
- `MainView.axaml`: 状态栏布局改为 Grid，支持左右两侧内容
- `MainViewModel.cs`: 新增 `OpenClaudeTerminalCommand` 命令和跨平台终端打开逻辑
