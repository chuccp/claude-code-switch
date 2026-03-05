using Avalonia.Media;

namespace ClaudeCodeSwitcher;

/// <summary>
/// 应用主题
/// </summary>
public enum AppTheme
{
    Dark,
    Light
}

/// <summary>
/// 主题颜色
/// </summary>
public static class ThemeColors
{
    // === 强调色 ===
    public static Color AccentColor => Color.FromRgb(88, 101, 242);
    public static Color SuccessColor => Color.FromRgb(34, 197, 94);
    public static Color ErrorColor => Color.FromRgb(255, 100, 100);

    // === 深色主题 ===
    public static class Dark
    {
        public static Color PanelFill => Color.FromRgb(18, 18, 24);
        public static Color WindowFill => Color.FromRgb(25, 25, 32);
        public static Color TextMain => Color.FromRgb(245, 245, 250);
        public static Color TextSub => Color.FromRgb(150, 150, 165);
        public static Color TextTitle => Color.FromRgb(136, 145, 255);
        public static Color Separator => Color.FromRgb(50, 50, 60);
        public static Color ButtonPrimary => Color.FromRgb(88, 101, 242);
        public static Color ButtonSecondary => Color.FromRgb(60, 60, 72);
        public static Color CardActive => Color.FromRgb(23, 42, 69);
        public static Color CardReadonly => Color.FromRgb(22, 55, 38);
        public static Color CardNormal => Color.FromRgb(32, 32, 40);
        public static Color MenuBg => Color.FromRgb(35, 35, 42);
        public static Color MenuBorder => Color.FromRgb(55, 55, 65);
    }

    // === 浅色主题 ===
    public static class Light
    {
        public static Color PanelFill => Color.FromRgb(250, 250, 252);
        public static Color WindowFill => Color.FromRgb(255, 255, 255);
        public static Color TextMain => Color.FromRgb(30, 30, 40);
        public static Color TextSub => Color.FromRgb(100, 100, 115);
        public static Color TextTitle => Color.FromRgb(88, 101, 242);
        public static Color Separator => Color.FromRgb(220, 220, 230);
        public static Color ButtonPrimary => Color.FromRgb(88, 101, 242);
        public static Color ButtonSecondary => Color.FromRgb(230, 230, 235);
        public static Color CardActive => Color.FromRgb(230, 240, 255);
        public static Color CardReadonly => Color.FromRgb(232, 245, 238);
        public static Color CardNormal => Color.FromRgb(252, 252, 254);
        public static Color MenuBg => Color.FromRgb(255, 255, 255);
        public static Color MenuBorder => Color.FromRgb(220, 220, 230);
    }
}