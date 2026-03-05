using Tomlyn;

namespace ConsoleApp1.Models;

/// <summary>
/// 应用配置结构
/// </summary>
public class AppConfig
{
    /// <summary>
    /// 调试模式：启用时会在工作目录读取 settings.json 用于模拟操作
    /// </summary>
    public bool DebugMode { get; set; }

    /// <summary>
    /// 主题：Dark 或 Light
    /// </summary>
    public string Theme { get; set; } = "Dark";

    /// <summary>
    /// 语言：English, zh-CN, zh-TW
    /// </summary>
    public string Language { get; set; } = "en";

    /// <summary>
    /// 窗口设置
    /// </summary>
    public WindowConfig Window { get; set; } = new();

    /// <summary>
    /// 对话框设置
    /// </summary>
    public DialogConfig Dialog { get; set; } = new();
}

/// <summary>
/// 窗口配置结构
/// </summary>
public class WindowConfig
{
    public uint Width { get; set; } = 800;
    public uint Height { get; set; } = 600;
    public int? X { get; set; }
    public int? Y { get; set; }
}

/// <summary>
/// 对话框配置结构
/// </summary>
public class DialogConfig
{
    public uint Width { get; set; } = 700;
    public uint Height { get; set; } = 550;
    public int? X { get; set; }
    public int? Y { get; set; }
}

/// <summary>
/// 配置文件管理
/// </summary>
public static class ConfigManager
{
    private static readonly string ConfigPath = "appl.toml";

    public static AppConfig LoadConfig()
    {
        var fullPath = Path.GetFullPath(ConfigPath);
        if (!File.Exists(ConfigPath))
        {
            Console.WriteLine($"配置文件不存在：{fullPath}，使用默认配置");
            return new AppConfig();
        }

        try
        {
            var content = File.ReadAllText(ConfigPath);
            var config = Toml.ToModel<AppConfig>(content);
            Console.WriteLine($"✓ 成功加载配置文件：{fullPath}");
            return config;
        }
        catch (Exception e)
        {
            Console.WriteLine($"✗ 配置文件解析失败：{e.Message}");
            return new AppConfig();
        }
    }

    public static void SaveConfig(AppConfig config)
    {
        var fullPath = Path.GetFullPath(ConfigPath);
        try
        {
            var content = Toml.FromModel(config);
            File.WriteAllText(ConfigPath, content);
            Console.WriteLine($"✓ 配置已保存到：{fullPath}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"✗ 保存配置失败：{e.Message}");
        }
    }

    /// <summary>
    /// 获取 settings.json 的路径
    /// </summary>
    public static string GetSettingsPath(bool debugMode)
    {
        if (debugMode)
        {
            var debugPath = Path.Combine(Directory.GetCurrentDirectory(), "settings.json");
            Console.WriteLine($"调试模式启用，使用工作目录配置：{debugPath}");
            return debugPath;
        }

        var homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var normalPath = Path.Combine(homeDir, ".claude", "settings.json");
        Console.WriteLine($"正常模式，使用系统配置：{normalPath}");
        return normalPath;
    }
}