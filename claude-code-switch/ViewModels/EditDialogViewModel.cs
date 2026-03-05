using System;
using System.Reactive;
using System.Text.Json;
using ConsoleApp1.Models;
using ReactiveUI;

namespace ConsoleApp1.ViewModels;

/// <summary>
/// 编辑对话框模式
/// </summary>
public enum DialogMode
{
    Add,
    Edit
}

/// <summary>
/// 编辑标签页
/// </summary>
public enum EditTab
{
    Form,
    Json
}

/// <summary>
/// 编辑对话框 ViewModel
/// </summary>
public class EditDialogViewModel : ReactiveObject
{
    private readonly MainViewModel _mainViewModel;
    private DialogMode _mode;
    private EditTab _currentTab = EditTab.Form;
    private string _jsonText = "";
    private string? _jsonError;
    private ConfigProfile _profile = new();
    private bool _isUpdatingFromJson;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }

    public event Action? CloseRequested;

    public EditDialogViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        SaveCommand = ReactiveCommand.Create(Save);
        CancelCommand = ReactiveCommand.Create(() => CloseRequested?.Invoke());
    }

    public DialogMode Mode
    {
        get => _mode;
        set
        {
            this.RaiseAndSetIfChanged(ref _mode, value);
            this.RaisePropertyChanged(nameof(Title));
            this.RaisePropertyChanged(nameof(ConfirmText));
        }
    }

    public string Title => Mode == DialogMode.Add ? "创建新配置" : "编辑配置";
    public string ConfirmText => Mode == DialogMode.Add ? "确定" : "保存";

    public EditTab CurrentTab
    {
        get => _currentTab;
        set
        {
            if (value != _currentTab)
            {
                if (value == EditTab.Form && string.IsNullOrEmpty(_jsonError))
                {
                    SyncFromJson();
                }
                else if (value == EditTab.Json)
                {
                    UpdateJsonText();
                }
            }
            this.RaiseAndSetIfChanged(ref _currentTab, value);
        }
    }

    public ConfigProfile Profile
    {
        get => _profile;
        set
        {
            _profile = value;
            UpdateJsonText();
            this.RaisePropertyChanged(nameof(Name));
            this.RaisePropertyChanged(nameof(AuthToken));
            this.RaisePropertyChanged(nameof(BaseUrl));
            this.RaisePropertyChanged(nameof(Model));
        }
    }

    public string Name
    {
        get => _profile.Name;
        set
        {
            if (_profile.Name != value)
            {
                _profile.Name = value;
                this.RaisePropertyChanged();
                UpdateJsonText();
            }
        }
    }

    public string AuthToken
    {
        get => _profile.Env.AnthropicAuthToken;
        set
        {
            if (_profile.Env.AnthropicAuthToken != value)
            {
                _profile.Env.AnthropicAuthToken = value;
                this.RaisePropertyChanged();
                UpdateJsonText();
            }
        }
    }

    public string BaseUrl
    {
        get => _profile.Env.AnthropicBaseUrl;
        set
        {
            if (_profile.Env.AnthropicBaseUrl != value)
            {
                _profile.Env.AnthropicBaseUrl = value;
                this.RaisePropertyChanged();
                UpdateJsonText();
            }
        }
    }

    public string Model
    {
        get => _profile.Env.AnthropicModel;
        set
        {
            if (_profile.Env.AnthropicModel != value)
            {
                _profile.Env.AnthropicModel = value;
                this.RaisePropertyChanged();
                UpdateJsonText();
            }
        }
    }

    public string JsonText
    {
        get => _jsonText;
        set
        {
            if (_jsonText != value)
            {
                this.RaiseAndSetIfChanged(ref _jsonText, value);
                ValidateAndSyncFromJson();
            }
        }
    }

    public string? JsonError
    {
        get => _jsonError;
        set => this.RaiseAndSetIfChanged(ref _jsonError, value);
    }

    public bool CanSave => string.IsNullOrEmpty(_jsonError);

    public void OpenAdd()
    {
        Mode = DialogMode.Add;
        Profile = new ConfigProfile();
        CurrentTab = EditTab.Form;
        JsonError = null;
    }

    public void OpenEdit(ConfigProfile profile)
    {
        Mode = DialogMode.Edit;
        Profile = new ConfigProfile
        {
            Id = profile.Id,
            Name = profile.Name,
            Env = new EnvConfig
            {
                AnthropicAuthToken = profile.Env.AnthropicAuthToken,
                AnthropicBaseUrl = profile.Env.AnthropicBaseUrl,
                AnthropicModel = profile.Env.AnthropicModel,
                ApiTimeoutMs = profile.Env.ApiTimeoutMs,
                ClaudeCodeDisableNonessentialTraffic = profile.Env.ClaudeCodeDisableNonessentialTraffic
            }
        };
        CurrentTab = EditTab.Form;
        JsonError = null;
    }

    private void UpdateJsonText()
    {
        if (_isUpdatingFromJson) return;
        
        _jsonText = JsonSerializer.Serialize(_profile, JsonOpts);
        this.RaisePropertyChanged(nameof(JsonText));
        JsonError = null;
        this.RaisePropertyChanged(nameof(CanSave));
    }

    private void ValidateAndSyncFromJson()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_jsonText))
            {
                JsonError = null;
                return;
            }

            var parsed = JsonSerializer.Deserialize<ConfigProfile>(_jsonText, JsonOpts);
            if (parsed != null)
            {
                _isUpdatingFromJson = true;
                _profile.Name = parsed.Name;
                _profile.Env = parsed.Env ?? new EnvConfig();
                _isUpdatingFromJson = false;
                
                JsonError = null;
                this.RaisePropertyChanged(nameof(Name));
                this.RaisePropertyChanged(nameof(AuthToken));
                this.RaisePropertyChanged(nameof(BaseUrl));
                this.RaisePropertyChanged(nameof(Model));
            }
        }
        catch (JsonException ex)
        {
            JsonError = $"JSON 错误：{ex.Message}";
        }
        
        this.RaisePropertyChanged(nameof(CanSave));
    }

    private void SyncFromJson()
    {
        if (string.IsNullOrEmpty(_jsonError) && !string.IsNullOrEmpty(_jsonText))
        {
            try
            {
                var parsed = JsonSerializer.Deserialize<ConfigProfile>(_jsonText, JsonOpts);
                if (parsed != null)
                {
                    _isUpdatingFromJson = true;
                    _profile.Name = parsed.Name;
                    _profile.Env = parsed.Env ?? new EnvConfig();
                    _isUpdatingFromJson = false;
                    
                    this.RaisePropertyChanged(nameof(Name));
                    this.RaisePropertyChanged(nameof(AuthToken));
                    this.RaisePropertyChanged(nameof(BaseUrl));
                    this.RaisePropertyChanged(nameof(Model));
                }
            }
            catch { }
        }
    }

    private void Save()
    {
        if (!CanSave) return;

        ConfigProfile finalProfile;
        if (CurrentTab == EditTab.Json)
        {
            try
            {
                var parsed = JsonSerializer.Deserialize<ConfigProfile>(_jsonText, JsonOpts);
                if (parsed == null) return;
                finalProfile = parsed;
            }
            catch
            {
                return;
            }
        }
        else
        {
            finalProfile = _profile;
        }

        if (Mode == DialogMode.Add)
        {
            finalProfile.Id = Guid.NewGuid().ToString();
            _mainViewModel.AddProfile(finalProfile);
        }
        else
        {
            _mainViewModel.UpdateProfile(finalProfile);
        }

        CloseRequested?.Invoke();
    }
}
