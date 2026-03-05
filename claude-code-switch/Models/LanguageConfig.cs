namespace ConsoleApp1.Models;

/// <summary>
/// 支持的语言
/// </summary>
public enum AppLanguage
{
    /// <summary>
    /// 英文
    /// </summary>
    English = 0,
    
    /// <summary>
    /// 简体中文
    /// </summary>
    SimplifiedChinese = 1,
    
    /// <summary>
    /// 繁体中文
    /// </summary>
    TraditionalChinese = 2
}

/// <summary>
/// 语言配置
/// </summary>
public static class LanguageConfig
{
    /// <summary>
    /// 获取语言显示名称
    /// </summary>
    public static string GetDisplayName(AppLanguage language)
    {
        return language switch
        {
            AppLanguage.English => "English",
            AppLanguage.SimplifiedChinese => "简体中文",
            AppLanguage.TraditionalChinese => "繁體中文",
            _ => "English"
        };
    }
    
    /// <summary>
    /// 获取语言代码
    /// </summary>
    public static string GetLanguageCode(AppLanguage language)
    {
        return language switch
        {
            AppLanguage.English => "en",
            AppLanguage.SimplifiedChinese => "zh-CN",
            AppLanguage.TraditionalChinese => "zh-TW",
            _ => "en"
        };
    }
    
    /// <summary>
    /// 从字符串解析语言
    /// </summary>
    public static AppLanguage Parse(string value)
    {
        return value.ToLower() switch
        {
            "zh-cn" or "zh_cn" or "simplifiedchinese" or "简体中文" => AppLanguage.SimplifiedChinese,
            "zh-tw" or "zh_tw" or "traditionalchinese" or "繁體中文" => AppLanguage.TraditionalChinese,
            _ => AppLanguage.English
        };
    }
}
