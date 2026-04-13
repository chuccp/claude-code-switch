using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ConsoleApp1.Models;
using ConsoleApp1.Services;
using ConsoleApp1.ViewModels;

namespace ConsoleApp1.Views;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainViewModel();
        DataContext = _viewModel;

        // 初始化已经通过 XAML 完成：
        // - ExtendClientAreaToDecorationsHint = True
        // - ExtendClientAreaChromeHints = NoChrome
        // - SystemDecorations = BorderOnly

        // 初始化语言切换
        InitializeLanguageChange();

        // 初始化语言切换
        InitializeLanguageChange();

        // 处理编辑请求
        _viewModel.RequestEdit += profile =>
        {
            var dialogVm = new EditDialogViewModel(_viewModel);
            dialogVm.OpenEdit(profile);
            OpenDialog(dialogVm);
        };

        _viewModel.RequestAdd += () =>
        {
            var dialogVm = new EditDialogViewModel(_viewModel);
            dialogVm.OpenAdd();
            OpenDialog(dialogVm);
        };
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        UpdateTitle();
    }

    private void InitializeLanguageChange()
    {
        LanguageService.OnLanguageChanged += OnLanguageChanged;
    }

    private void OnLanguageChanged(AppLanguage language)
    {
        UpdateTitle();
    }

    private void UpdateTitle()
    {
        Title = "claude-code-switch";
    }

    private async void OpenDialog(EditDialogViewModel viewModel)
    {
        var dialog = new EditDialogWindow { DataContext = viewModel };
        viewModel.CloseRequested += () => dialog.Close();
        await dialog.ShowDialog(this);
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        // 保存窗口位置
        var pos = Position;
        _viewModel.SaveWindowConfig(Width, Height, pos.X, pos.Y);
        base.OnClosing(e);
    }
}