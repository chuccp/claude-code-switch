using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using ConsoleApp1.Views;

namespace ConsoleApp1;

public class App : Application
{
    public static void SetTheme(string theme)
    {
        Console.WriteLine($"[主题] SetTheme 被调用，参数：{theme}");

        if (Current == null)
        {
            Console.WriteLine("[主题] 错误：Current is null");
            return;
        }

        var themeVariant = theme.ToLower() switch
        {
            "light" => ThemeVariant.Light,
            "dark" => ThemeVariant.Dark,
            _ => ThemeVariant.Dark
        };

        Console.WriteLine($"[主题] 当前主题：{Current.RequestedThemeVariant}");
        Console.WriteLine($"[主题] 目标主题：{themeVariant}");

        // 切换主题
        Current.RequestedThemeVariant = themeVariant;

        Console.WriteLine($"[主题] 切换后主题：{Current.RequestedThemeVariant}");

        // 更新资源字典中的动态资源引用
        UpdateThemeResources(themeVariant);
    }

    private static void UpdateThemeResources(ThemeVariant themeVariant)
    {
        if (Current == null) return;

        var suffix = themeVariant == ThemeVariant.Dark ? ".Dark" : ".Light";
        var resourceNames = new[]
        {
            "AppBackgroundBrush", "TitleBarBackgroundBrush", "CardBackgroundBrush",
            "PanelBackgroundBrush", "SurfaceBackgroundBrush", "BorderBrush",
            "CardBorderBrush", "DividerBrush", "TextPrimaryBrush", "TextSecondaryBrush",
            "TextTertiaryBrush", "TextDisabledBrush", "AccentBrush", "AccentHoverBrush",
            "AccentLightBrush", "ButtonBackgroundBrush", "ButtonHoverBrush",
            "ButtonPressedBrush", "ButtonBorderBrush", "InputBackgroundBrush",
            "InputBorderBrush", "InputFocusedBorderBrush", "SuccessBrush",
            "SuccessBackgroundBrush", "WarningBrush", "WarningBackgroundBrush",
            "ErrorBrush", "ErrorBackgroundBrush", "InfoBrush",
            "ScrollBarThumbBrush", "ScrollBarThumbHoverBrush"
        };

        foreach (var name in resourceNames)
        {
            var keyWithSuffix = name + suffix;
            if (Current.Resources.TryGetValue(keyWithSuffix, out var value))
            {
                Current.Resources[name] = value;
            }
        }

        Console.WriteLine($"[主题] 资源更新完成 ({themeVariant})");
    }

    public override void Initialize()
    {
        Console.WriteLine("[主题] Initialize 开始");
        AvaloniaXamlLoader.Load(this);
        Console.WriteLine("[主题] XAML 加载完成");
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Console.WriteLine("[主题] OnFrameworkInitializationCompleted 开始");

        if (ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();

        Console.WriteLine($"[主题] 最终主题：{RequestedThemeVariant}");
    }
}
