using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using ConsoleApp1.Controls;
using ConsoleApp1.Models;
using ConsoleApp1.Services;
using ReactiveUI;

namespace ConsoleApp1.ViewModels;

/// <summary>
/// 主窗口 ViewModel
/// </summary>
public class MainViewModel : ReactiveObject
{
    private readonly AppConfig _appConfig;
    private string _selectedProfileName = "无";
    private string _currentTheme = "Dark";
    private string _currentLanguage = "en";

    public ObservableCollection<ConfigProfile> Profiles { get; } = new();
    public List<string> Themes { get; } = new() { "Dark", "Light" };
    public List<string> Languages { get; } = new() { "English", "简体中文", "繁體中文" };

    public ReactiveCommand<Unit, Unit> AddCommand { get; }
    public ReactiveCommand<Unit, Unit> RefreshCommand { get; }
    public ReactiveCommand<ConfigProfile, Unit> ApplyCommand { get; }
    public ReactiveCommand<ConfigProfile, Unit> EditCommand { get; }
    public ReactiveCommand<ConfigProfile, Unit> CopyCommand { get; }
    public ReactiveCommand<ConfigProfile, Unit> DeleteCommand { get; }
    public ReactiveCommand<string, Unit> ChangeLanguageCommand { get; }
    public ReactiveCommand<Unit, Unit> OpenClaudeTerminalCommand { get; }

    public event Action<ConfigProfile>? RequestEdit;
    public event Action? RequestAdd;
    public event Action<string>? RequestLanguageChange;

    public string CurrentTheme
    {
        get => _currentTheme;
        set
        {
            Console.WriteLine($"CurrentTheme setter called: {_currentTheme} -> {value}");
            if (_currentTheme != value)
            {
                this.RaiseAndSetIfChanged(ref _currentTheme, value);
                App.SetTheme(value);
                _appConfig.Theme = value;
                ConfigManager.SaveConfig(_appConfig);
            }
        }
    }

    public string CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage != value)
            {
                this.RaiseAndSetIfChanged(ref _currentLanguage, value);
                var languageCode = GetLanguageCodeFromDisplayName(value);
                _appConfig.Language = languageCode;
                LanguageService.CurrentLanguage = LanguageConfig.Parse(languageCode);
                ConfigManager.SaveConfig(_appConfig);
                RequestLanguageChange?.Invoke(languageCode);
            }
        }
    }

    private string GetLanguageCodeFromDisplayName(string displayName)
    {
        return displayName switch
        {
            "简体中文" => "zh-CN",
            "繁體中文" => "zh-TW",
            _ => "en"
        };
    }

    public MainViewModel()
    {
        _appConfig = ConfigManager.LoadConfig();
        _currentTheme = _appConfig.Theme;
        _currentLanguage = GetLanguageDisplayName(_appConfig.Language);

        // 初始化语言服务
        LanguageService.Initialize();
        LanguageService.CurrentLanguage = LanguageConfig.Parse(_appConfig.Language);

        // 使用 Scheduler 的方式确保在主线程上执行
        AddCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await Dispatcher.UIThread.InvokeAsync(() => RequestAdd?.Invoke());
        });

        RefreshCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await Dispatcher.UIThread.InvokeAsync(ReloadProfiles);
        });

        ApplyCommand = ReactiveCommand.CreateFromTask<ConfigProfile>(async (profile) =>
        {
            await Dispatcher.UIThread.InvokeAsync(() => ApplyProfile(profile));
        });

        EditCommand = ReactiveCommand.CreateFromTask<ConfigProfile>(async (profile) =>
        {
            await Dispatcher.UIThread.InvokeAsync(() => RequestEdit?.Invoke(profile));
        });

        CopyCommand = ReactiveCommand.CreateFromTask<ConfigProfile>(async (profile) =>
        {
            await Dispatcher.UIThread.InvokeAsync(() => CopyProfile(profile));
        });

        DeleteCommand = ReactiveCommand.CreateFromTask<ConfigProfile>(async (profile) =>
        {
            await Dispatcher.UIThread.InvokeAsync(() => DeleteProfile(profile));
        });

        ChangeLanguageCommand = ReactiveCommand.Create<string>(language =>
        {
            CurrentLanguage = language;
        });

        OpenClaudeTerminalCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await OpenClaudeTerminal();
        });

        LoadProfiles();

        // 应用保存的主题
        App.SetTheme(_currentTheme);
    }

    private string GetLanguageDisplayName(string languageCode)
    {
        return languageCode.ToLower() switch
        {
            "zh-cn" => "简体中文",
            "zh-tw" => "繁體中文",
            _ => "English"
        };
    }

    public bool DebugMode => _appConfig.DebugMode;

    public string SelectedProfileName
    {
        get => _selectedProfileName;
        set => this.RaiseAndSetIfChanged(ref _selectedProfileName, value);
    }

    public void LoadProfiles()
    {
        Profiles.Clear();

        // 加载当前系统配置
        var currentConfig = StorageService.LoadCurrentConfig(_appConfig.DebugMode);
        if (currentConfig != null)
            Profiles.Add(currentConfig);

        // 加载用户配置（会自动比较并标记匹配的配置为"当前"）
        var userProfiles = StorageService.LoadProfiles(_appConfig.DebugMode);
        foreach (var profile in userProfiles)
            Profiles.Add(profile);

        // 如果没有配置，创建默认配置
        if (Profiles.Count <= 1)
        {
            var defaultProfiles = new[]
            {
                new ConfigProfile { Name = "默认配置", IsSelected = true },
                new ConfigProfile { Name = "开发配置" },
                new ConfigProfile { Name = "生产配置" }
            };

            foreach (var p in defaultProfiles)
                Profiles.Add(p);

            StorageService.SaveProfiles(Profiles.ToList());
        }

        UpdateSelectedName();
    }

    public void ReloadProfiles()
    {
        LoadProfiles();
        UpdateSelectedName();
    }

    private async void ApplyProfile(ConfigProfile profile)
    {
        if (profile.IsReadonly) return;

        // 显示确认对话框
        var confirmMessage = $"{LanguageService.GetText("ConfirmApply")}\n\n" +
            $"{LanguageService.GetText("Name")}: {profile.Name}\n" +
            $"{LanguageService.GetText("Model")} {profile.Env.AnthropicModel}\n" +
            $"{LanguageService.GetText("URL")} {profile.Env.AnthropicBaseUrl}\n" +
            $"{LanguageService.GetText("Token")}: {MaskToken(profile.Env.AnthropicAuthToken)}";

        var mainWindow = GetMainWindow();
        if (mainWindow != null)
        {
            var result = await MessageBox.Show(mainWindow, confirmMessage, LanguageService.GetText("ConfirmApplyTitle"),
                MessageBoxButton.YesNo, MessageBoxIcon.Question);

            if (result != MessageBoxResult.Yes)
                return;
        }

        // 保存配置
        StorageService.SaveProfiles(Profiles.ToList());

        // 应用配置到系统
        StorageService.ApplyConfig(profile, _appConfig.DebugMode);

        // 重新加载配置，系统配置会排在第一位
        ReloadProfiles();
    }

    private string MaskToken(string token)
    {
        if (string.IsNullOrEmpty(token)) return "(空)";
        if (token.Length <= 8) return "***";
        return token.Substring(0, 4) + "..." + token.Substring(token.Length - 4);
    }

    private Window? GetMainWindow()
    {
        return Application.Current?.ApplicationLifetime is
            Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;
    }

    private void CopyProfile(ConfigProfile profile)
    {
        var copied = new ConfigProfile
        {
            Name = $"复制: {profile.Name}",
            Env = new EnvConfig
            {
                AnthropicAuthToken = profile.Env.AnthropicAuthToken,
                AnthropicBaseUrl = profile.Env.AnthropicBaseUrl,
                AnthropicModel = profile.Env.AnthropicModel,
                ApiTimeoutMs = profile.Env.ApiTimeoutMs,
                ClaudeCodeDisableNonessentialTraffic = profile.Env.ClaudeCodeDisableNonessentialTraffic
            }
        };

        Profiles.Add(copied);
        StorageService.SaveProfiles(Profiles.ToList());
    }

    private void DeleteProfile(ConfigProfile profile)
    {
        if (profile.IsReadonly || Profiles.Count <= 2) return;

        Profiles.Remove(profile);
        StorageService.SaveProfiles(Profiles.ToList());
    }

    public void AddProfile(ConfigProfile profile)
    {
        Profiles.Add(profile);
        StorageService.SaveProfiles(Profiles.ToList());
    }

    public void UpdateProfile(ConfigProfile profile)
    {
        var existing = Profiles.FirstOrDefault(p => p.Id == profile.Id);
        if (existing != null)
        {
            existing.Name = profile.Name;
            existing.Env = profile.Env;
        }
        StorageService.SaveProfiles(Profiles.ToList());
    }

    public void SaveWindowConfig(double width, double height, double? x, double? y)
    {
        _appConfig.Window.Width = (uint)width;
        _appConfig.Window.Height = (uint)height;
        _appConfig.Window.X = x.HasValue ? (int)x.Value : null;
        _appConfig.Window.Y = y.HasValue ? (int)y.Value : null;
        ConfigManager.SaveConfig(_appConfig);
    }

    private void UpdateSelectedName()
    {
        SelectedProfileName = Profiles.FirstOrDefault(p => p.IsSelected)?.Name ?? "无";
    }

    private async Task OpenClaudeTerminal()
    {
        try
        {
            var isWindows = OperatingSystem.IsWindows();
            var isMacOs = OperatingSystem.IsMacOS();
            var isLinux = OperatingSystem.IsLinux();

            ProcessStartInfo processStartInfo;

            if (isWindows)
            {
                processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/k claude",
                    UseShellExecute = true,
                    WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                };
            }
            else if (isMacOs)
            {
                processStartInfo = new ProcessStartInfo
                {
                    FileName = "osascript",
                    Arguments = "-e 'tell application \"Terminal\" to do script \"claude\"'",
                    UseShellExecute = true,
                    WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                };
            }
            else if (isLinux)
            {
                // 尝试常见的 Linux 终端
                var terminalEmulators = new[]
                {
                    ("gnome-terminal", "-- claude"),
                    ("konsole", "-e claude"),
                    ("xfce4-terminal", "-e claude"),
                    ("xterm", "-e claude"),
                    ("alacritty", "-e claude"),
                    ("kitty", "-e claude")
                };

                foreach (var (terminal, args) in terminalEmulators)
                {
                    if (IsCommandAvailable(terminal))
                    {
                        processStartInfo = new ProcessStartInfo
                        {
                            FileName = terminal,
                            Arguments = args,
                            UseShellExecute = true,
                            WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                        };
                        Process.Start(processStartInfo);
                        return;
                    }
                }

                // 如果都没有，尝试使用 xdg-open 或默认终端
                processStartInfo = new ProcessStartInfo
                {
                    FileName = "xdg-terminal",
                    Arguments = "claude",
                    UseShellExecute = true,
                    WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                };
            }
            else
            {
                throw new NotSupportedException("不支持的操作系统");
            }

            Process.Start(processStartInfo);
        }
        catch (Exception ex)
        {
            var mainWindow = GetMainWindow();
            if (mainWindow != null)
            {
                await MessageBox.Show(mainWindow, $"无法打开终端：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxIcon.Error);
            }
        }
    }

    private static bool IsCommandAvailable(string command)
    {
        try
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "which",
                Arguments = command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            using var process = Process.Start(processStartInfo);
            process?.WaitForExit(1000);
            return process?.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }
}