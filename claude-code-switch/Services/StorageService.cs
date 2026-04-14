using System.Text.Json;
using ConsoleApp1.Models;

namespace ConsoleApp1.Services;

/// <summary>
/// 存储服务 - 负责配置的加载和保存
/// </summary>
public static class StorageService
{
    private static readonly string ConfigPath = GetConfigPath("config.json");

    private static string GetConfigPath(string fileName)
    {
        var appDataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "claude-code-switch");
        if (!Directory.Exists(appDataDir))
            Directory.CreateDirectory(appDataDir);
        return Path.Combine(appDataDir, fileName);
    }
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    /// <summary>
    /// 加载配置档案列表
    /// </summary>
    public static List<ConfigProfile> LoadProfiles(bool debugMode = false)
    {
        var fullPath = Path.GetFullPath(ConfigPath);
        Console.WriteLine($"从配置文件加载：{fullPath}");

        if (!File.Exists(ConfigPath))
        {
            Console.WriteLine($"配置文件不存在：{fullPath}");
            return new List<ConfigProfile>();
        }

        // 获取当前系统配置用于比较
        var currentConfig = LoadCurrentConfig(debugMode);

        try
        {
            var content = File.ReadAllText(ConfigPath);
            var profiles = JsonSerializer.Deserialize<List<ConfigProfile>>(content, JsonOptions);

            if (profiles != null && profiles.Count > 0)
            {
                Console.WriteLine($"成功加载 {profiles.Count} 个配置");

                ConfigProfile? matchingProfile = null;

                // 找到第一个匹配的配置，标记为"当前"
                if (currentConfig != null)
                {
                    foreach (var profile in profiles)
                    {
                        if (IsProfileMatchingCurrent(profile, currentConfig))
                        {
                            profile.IsSelected = true;
                            matchingProfile = profile;
                            Console.WriteLine($"匹配到当前配置：{profile.Name}");
                            break; // 只匹配第一个
                        }
                    }
                }

                if (!profiles.Any(p => p.IsSelected))
                {
                    profiles[0].IsSelected = true;
                    Console.WriteLine("没有找到匹配的配置，默认选中第一个配置");
                }

                // 将匹配的配置移动到第一个位置
                if (matchingProfile != null)
                {
                    profiles.Remove(matchingProfile);
                    profiles.Insert(0, matchingProfile);
                    Console.WriteLine($"已将当前配置移动到第一位：{matchingProfile.Name}");
                }

                return profiles;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"配置文件解析失败：{e.Message}");
        }

        return new List<ConfigProfile>();
    }

    /// <summary>
    /// 判断配置档案是否与当前系统配置匹配
    /// </summary>
    private static bool IsProfileMatchingCurrent(ConfigProfile profile, ConfigProfile? currentConfig)
    {
        if (currentConfig == null) return false;

        return string.Equals(profile.Env.AnthropicAuthToken, currentConfig.Env.AnthropicAuthToken, StringComparison.Ordinal)
            && string.Equals(profile.Env.AnthropicBaseUrl, currentConfig.Env.AnthropicBaseUrl, StringComparison.Ordinal)
            && string.Equals(profile.Env.AnthropicModel, currentConfig.Env.AnthropicModel, StringComparison.Ordinal);
    }

    /// <summary>
    /// 保存配置档案列表
    /// </summary>
    public static void SaveProfiles(List<ConfigProfile> profiles)
    {
        var fullPath = Path.GetFullPath(ConfigPath);
        try
        {
            var profilesToSave = profiles.Where(p => !p.IsReadonly).ToList();
            var content = JsonSerializer.Serialize(profilesToSave, JsonOptions);
            File.WriteAllText(ConfigPath, content);
            Console.WriteLine($"✓ 配置已成功保存到 {fullPath}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"✗ 无法保存配置：{e.Message}");
        }
    }

    /// <summary>
    /// 加载当前系统配置
    /// </summary>
    public static ConfigProfile? LoadCurrentConfig(bool debugMode)
    {
        var settingsPath = ConfigManager.GetSettingsPath(debugMode);
        var fullPath = Path.GetFullPath(settingsPath);
        Console.WriteLine($"尝试读取配置文件：{fullPath}");

        if (!File.Exists(settingsPath))
        {
            Console.WriteLine($"配置文件不存在：{fullPath}");
            return null;
        }

        try
        {
            var content = File.ReadAllText(settingsPath);
            Console.WriteLine($"成功读取到配置文件内容：{fullPath}");

            var settings = JsonSerializer.Deserialize<SettingsJson>(content);
            if (settings?.Env != null)
            {
                var env = new EnvConfig();

                if (settings.Env.TryGetValue("ANTHROPIC_AUTH_TOKEN", out var token))
                    env.AnthropicAuthToken = token;
                if (settings.Env.TryGetValue("ANTHROPIC_BASE_URL", out var baseUrl))
                    env.AnthropicBaseUrl = baseUrl;
                if (settings.Env.TryGetValue("ANTHROPIC_MODEL", out var model))
                    env.AnthropicModel = model;

                var profileName = debugMode ? "当前配置 (调试模式)" : "当前配置 (不可修改)";

                return new ConfigProfile
                {
                    Id = "current-config",
                    Name = profileName,
                    Env = env,
                    IsSelected = false,
                    IsReadonly = true
                };
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"解析配置文件失败：{e.Message}");
        }

        return null;
    }

    /// <summary>
    /// 应用配置到系统
    /// </summary>
    public static void ApplyConfig(ConfigProfile profile, bool debugMode)
    {
        var settingsPath = ConfigManager.GetSettingsPath(debugMode);
        var fullPath = Path.GetFullPath(settingsPath);

        var env = new Dictionary<string, string>
        {
            ["ANTHROPIC_AUTH_TOKEN"] = profile.Env.AnthropicAuthToken,
            ["ANTHROPIC_BASE_URL"] = profile.Env.AnthropicBaseUrl,
            ["ANTHROPIC_MODEL"] = profile.Env.AnthropicModel,
            ["API_TIMEOUT_MS"] = profile.Env.ApiTimeoutMs,
            ["CLAUDE_CODE_DISABLE_NONESSENTIAL_TRAFFIC"] = profile.Env.ClaudeCodeDisableNonessentialTraffic
        };

        var settings = new SettingsJson { Env = env };

        try
        {
            var content = JsonSerializer.Serialize(settings, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(settingsPath, content);
            Console.WriteLine($"✓ 配置已应用到：{fullPath}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"✗ 应用配置失败：{e.Message}");
        }
    }
}