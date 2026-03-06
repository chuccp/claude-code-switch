using ConsoleApp1.Models;

namespace ConsoleApp1.Services;

/// <summary>
/// 语言服务 - 管理应用语言设置
/// </summary>
public static class LanguageService
{
    private static AppLanguage _currentLanguage = AppLanguage.English;
    private static readonly Dictionary<string, Dictionary<string, string>> _resources = new();

    /// <summary>
    /// 当前语言
    /// </summary>
    public static AppLanguage CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage != value)
            {
                _currentLanguage = value;
                OnLanguageChanged?.Invoke(value);
            }
        }
    }

    /// <summary>
    /// 语言变更事件
    /// </summary>
    public static event Action<AppLanguage>? OnLanguageChanged;

    /// <summary>
    /// 初始化语言服务
    /// </summary>
    public static void Initialize()
    {
        LoadResources();
    }

    /// <summary>
    /// 加载语言资源
    /// </summary>
    private static void LoadResources()
    {
        // 英文资源
        _resources["en"] = new Dictionary<string, string>
        {
            ["AppTitle"] = "Config Switch",
            ["Add"] = "+ Add",
            ["Refresh"] = "~ Refresh",
            ["Current"] = "Current",
            ["System"] = "System",
            ["ReadOnly"] = "Read Only",
            ["Model"] = "Model:",
            ["URL"] = "URL:",
            ["SelectedProfile"] = "Selected Profile: {0}",
            ["ProfilesCount"] = "{0} profiles",
            ["DebugMode"] = "Debug Mode",
            ["Theme"] = "Theme",
            ["Language"] = "Language",
            ["None"] = "None",
            ["ConfirmApply"] = "Are you sure you want to apply this configuration?",
            ["ConfirmApplyTitle"] = "Confirm Apply",
            ["Name"] = "Name",
            ["Token"] = "Token",
            ["Copied"] = "Copied: {0}",
            ["Copy"] = "Copy",
            ["Edit"] = "Edit",
            ["Delete"] = "Delete",
            ["Apply"] = "Apply",
            ["EditDialogTitle"] = "Edit Configuration",
            ["EditDialogSave"] = "Save",
            ["EditDialogCancel"] = "Cancel",
            ["EditDialogNameLabel"] = "Name:",
            ["EditDialogNameWatermark"] = "Enter configuration name",
            ["EditDialogTokenLabel"] = "Token:",
            ["EditDialogUrlLabel"] = "Base URL:",
            ["EditDialogModelLabel"] = "Model:",
            ["EditDialogTimeoutLabel"] = "Timeout (ms):",
            ["EditDialogDisableTrafficLabel"] = "Disable Non-essential Traffic:",
            ["FormEditLabel"] = "Form Editor",
            ["ConfigNameLabel"] = "Configuration Name",
            ["ApiConfigLabel"] = "API Configuration",
            ["JsonConfigLabel"] = "JSON Configuration",
            ["JsonConfigWatermark"] = "Enter JSON configuration here, changes will sync to the form...",
            ["JsonError"] = "JSON Error",
            ["Terminal"] = "Terminal"
        };

        // 简体中文资源
        _resources["zh-CN"] = new Dictionary<string, string>
        {
            ["AppTitle"] = "配置切换器",
            ["Add"] = "+ 新增",
            ["Refresh"] = "~ 刷新",
            ["Current"] = "当前",
            ["System"] = "系统",
            ["ReadOnly"] = "不可修改",
            ["Model"] = "Model:",
            ["URL"] = "URL:",
            ["SelectedProfile"] = "当前配置：{0}",
            ["ProfilesCount"] = "{0} 个配置",
            ["DebugMode"] = "调试模式",
            ["Theme"] = "主题",
            ["Language"] = "语言",
            ["None"] = "无",
            ["ConfirmApply"] = "确定要应用此配置吗？",
            ["ConfirmApplyTitle"] = "确认应用配置",
            ["Name"] = "名称",
            ["Token"] = "Token",
            ["Copied"] = "已复制：{0}",
            ["Copy"] = "复制",
            ["Edit"] = "编辑",
            ["Delete"] = "删除",
            ["Apply"] = "应用",
            ["EditDialogTitle"] = "编辑配置",
            ["EditDialogSave"] = "保存",
            ["EditDialogCancel"] = "取消",
            ["EditDialogNameLabel"] = "名称：",
            ["EditDialogNameWatermark"] = "请输入配置名称",
            ["EditDialogTokenLabel"] = "Token:",
            ["EditDialogUrlLabel"] = "基础 URL:",
            ["EditDialogModelLabel"] = "模型:",
            ["EditDialogTimeoutLabel"] = "超时 (ms):",
            ["EditDialogDisableTrafficLabel"] = "禁用非必要流量:",
            ["FormEditLabel"] = "表单编辑",
            ["ConfigNameLabel"] = "配置名称",
            ["ApiConfigLabel"] = "API 配置",
            ["JsonConfigLabel"] = "JSON 配置",
            ["JsonConfigWatermark"] = "在此输入 JSON 配置，修改将自动同步到左侧表单...",
            ["JsonError"] = "JSON 错误",
            ["Terminal"] = "终端"
        };

        // 繁体中文资源
        _resources["zh-TW"] = new Dictionary<string, string>
        {
            ["AppTitle"] = "設定切換器",
            ["Add"] = "+ 新增",
            ["Refresh"] = "~ 重新整理",
            ["Current"] = "目前",
            ["System"] = "系統",
            ["ReadOnly"] = "不可修改",
            ["Model"] = "Model:",
            ["URL"] = "URL:",
            ["SelectedProfile"] = "目前設定：{0}",
            ["ProfilesCount"] = "{0} 個設定",
            ["DebugMode"] = "偵錯模式",
            ["Theme"] = "主題",
            ["Language"] = "語言",
            ["None"] = "無",
            ["ConfirmApply"] = "確定要套用到此設定嗎？",
            ["ConfirmApplyTitle"] = "確認套用設定",
            ["Name"] = "名稱",
            ["Token"] = "Token",
            ["Copied"] = "已複製：{0}",
            ["Copy"] = "複製",
            ["Edit"] = "編輯",
            ["Delete"] = "刪除",
            ["Apply"] = "套用",
            ["EditDialogTitle"] = "編輯設定",
            ["EditDialogSave"] = "儲存",
            ["EditDialogCancel"] = "取消",
            ["EditDialogNameLabel"] = "名稱：",
            ["EditDialogNameWatermark"] = "請輸入設定名稱",
            ["EditDialogTokenLabel"] = "Token:",
            ["EditDialogUrlLabel"] = "基礎 URL:",
            ["EditDialogModelLabel"] = "模型:",
            ["EditDialogTimeoutLabel"] = "逾時 (ms):",
            ["EditDialogDisableTrafficLabel"] = "停用非必要流量:",
            ["FormEditLabel"] = "表單編輯",
            ["ConfigNameLabel"] = "設定名稱",
            ["ApiConfigLabel"] = "API 設定",
            ["JsonConfigLabel"] = "JSON 設定",
            ["JsonConfigWatermark"] = "在此輸入 JSON 設定，修改將自動同步到左側表單...",
            ["JsonError"] = "JSON 錯誤",
            ["Terminal"] = "終端"
        };
    }

    /// <summary>
    /// 获取本地化文本
    /// </summary>
    public static string GetText(string key, params object[] args)
    {
        var langCode = GetLanguageCode(CurrentLanguage);
        
        if (_resources.TryGetValue(langCode, out var langResources) && 
            langResources.TryGetValue(key, out var value))
        {
            return args.Length > 0 ? string.Format(value, args) : value;
        }
        
        // 回退到英文
        if (_resources.TryGetValue("en", out var enResources) && 
            enResources.TryGetValue(key, out var enValue))
        {
            return args.Length > 0 ? string.Format(enValue, args) : enValue;
        }
        
        return key;
    }

    /// <summary>
    /// 获取语言代码
    /// </summary>
    private static string GetLanguageCode(AppLanguage language)
    {
        return LanguageConfig.GetLanguageCode(language);
    }

    /// <summary>
    /// 获取所有可用语言
    /// </summary>
    public static List<AppLanguage> GetAvailableLanguages()
    {
        return new List<AppLanguage>
        {
            AppLanguage.English,
            AppLanguage.SimplifiedChinese,
            AppLanguage.TraditionalChinese
        };
    }

    /// <summary>
    /// 获取语言显示名称列表
    /// </summary>
    public static List<string> GetLanguageDisplayNames()
    {
        return GetAvailableLanguages()
            .Select(lang => LanguageConfig.GetDisplayName(lang))
            .ToList();
    }
}
