using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ConsoleApp1.Models;
using ConsoleApp1.Services;
using ConsoleApp1.ViewModels;

namespace ConsoleApp1.Views;

public partial class EditDialogWindow : Window
{
    public EditDialogWindow()
    {
        InitializeComponent();
        InitializeLanguageChange();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        UpdateLanguageTexts();
    }

    private void InitializeLanguageChange()
    {
        LanguageService.OnLanguageChanged += OnLanguageChanged;
    }

    private void OnLanguageChanged(AppLanguage language)
    {
        UpdateLanguageTexts();
    }

    private void UpdateLanguageTexts()
    {
        if (this.FindControl<TextBlock>("FormEditLabel") is TextBlock formEditLabel)
            formEditLabel.Text = LanguageService.GetText("FormEditLabel");
        
        if (this.FindControl<TextBlock>("ConfigNameLabel") is TextBlock configNameLabel)
            configNameLabel.Text = LanguageService.GetText("EditDialogNameLabel");
        
        if (this.FindControl<TextBox>("NameTextBox") is TextBox nameTextBox)
            nameTextBox.Watermark = LanguageService.GetText("EditDialogNameWatermark");
        
        if (this.FindControl<TextBlock>("ApiConfigLabel") is TextBlock apiConfigLabel)
            apiConfigLabel.Text = LanguageService.GetText("ApiConfigLabel");
        
        if (this.FindControl<TextBlock>("JsonConfigLabel") is TextBlock jsonConfigLabel)
            jsonConfigLabel.Text = LanguageService.GetText("JsonConfigLabel");
        
        if (this.FindControl<TextBox>("JsonWatermark") is TextBox jsonWatermark)
            jsonWatermark.Watermark = LanguageService.GetText("JsonConfigWatermark");
    }

    private void OnCloseClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void OnTitleBarPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        BeginMoveDrag(e);
    }
}
