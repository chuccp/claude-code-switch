using System.Text.Json.Serialization;
using ReactiveUI;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models;

/// <summary>
/// 配置档案
/// </summary>
public class ConfigProfile : ReactiveObject
{
    public ConfigProfile()
    {
        // 订阅语言变更事件
        LanguageService.OnLanguageChanged += _ => OnLanguageChanged();
    }

    private void OnLanguageChanged()
    {
        this.RaisePropertyChanged(nameof(CurrentText));
        this.RaisePropertyChanged(nameof(SystemText));
        this.RaisePropertyChanged(nameof(ReadOnlyText));
    }

    public string Id { get; set; } = Guid.NewGuid().ToString();

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private EnvConfig _env = new();
    public EnvConfig Env
    {
        get => _env;
        set => this.RaiseAndSetIfChanged(ref _env, value);
    }

    private bool _isSelected;
    [JsonIgnore]
    public bool IsSelected
    {
        get => _isSelected;
        set => this.RaiseAndSetIfChanged(ref _isSelected, value);
    }

    private bool _isReadonly;
    [JsonIgnore]
    public bool IsReadonly
    {
        get => _isReadonly;
        set => this.RaiseAndSetIfChanged(ref _isReadonly, value);
    }

    /// <summary>
    /// 本地化的"当前"文本
    /// </summary>
    [JsonIgnore]
    public string CurrentText => LanguageService.GetText("Current");

    /// <summary>
    /// 本地化的"系统"文本
    /// </summary>
    [JsonIgnore]
    public string SystemText => LanguageService.GetText("System");

    /// <summary>
    /// 本地化的"不可修改"文本
    /// </summary>
    [JsonIgnore]
    public string ReadOnlyText => LanguageService.GetText("ReadOnly");
}

/// <summary>
/// 环境配置
/// </summary>
public class EnvConfig
{
    [JsonPropertyName("anthropic_auth_token")]
    public string AnthropicAuthToken { get; set; } = string.Empty;

    [JsonPropertyName("anthropic_base_url")]
    public string AnthropicBaseUrl { get; set; } = "https://api.anthropic.com";

    [JsonPropertyName("anthropic_model")]
    public string AnthropicModel { get; set; } = string.Empty;

    [JsonPropertyName("api_timeout_ms")]
    public string ApiTimeoutMs { get; set; } = "3000000";

    [JsonPropertyName("claude_code_disable_nonessential_traffic")]
    public string ClaudeCodeDisableNonessentialTraffic { get; set; } = "1";
}

/// <summary>
/// settings.json 的结构
/// </summary>
public class SettingsJson
{
    [JsonPropertyName("env")]
    public Dictionary<string, string>? Env { get; set; }
}