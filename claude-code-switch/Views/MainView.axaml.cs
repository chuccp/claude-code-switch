using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using ConsoleApp1.Models;
using ConsoleApp1.ViewModels;

namespace ConsoleApp1.Views;

public partial class MainView : UserControl
{
    private const double MinCardWidth = 280; // 卡片最小宽度
    private const double CardGap = 12; // 卡片间距
    private ContextMenu? _currentMenu; // 跟踪当前打开的菜单

    public MainView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private Window? GetWindow() => this.FindAncestorOfType<Window>();

    private void OnMinimizeClick(object? sender, RoutedEventArgs e)
    {
        GetWindow()?.WindowState = WindowState.Minimized;
    }

    private void OnMaximizeClick(object? sender, RoutedEventArgs e)
    {
        var window = GetWindow();
        if (window == null) return;

        window.WindowState = window.WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }

    private void OnCloseClick(object? sender, RoutedEventArgs e)
    {
        GetWindow()?.Close();
    }

    private void OnTitleBarPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // 如果点击的是 ComboBox 或其子元素，不处理拖动
        var source = e.Source as Visual;
        while (source != null)
        {
            if (source is ComboBox)
                return;
            source = source.GetVisualParent() as Visual;
        }

        var window = GetWindow();
        if (window == null) return;

        // 开始拖动窗口
        window.BeginMoveDrag(e);
    }

    private void OnItemsControlSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        if (sender is not ItemsControl itemsControl) return;

        var wrapPanel = itemsControl.GetVisualDescendants()
            .OfType<WrapPanel>()
            .FirstOrDefault();
        if (wrapPanel == null) return;

        var availableWidth = e.NewSize.Width;

        // 计算可以放多少列
        // 每个槽位最小宽度 = MinCardWidth + gap
        // columns = floor(availableWidth / (MinCardWidth + gap))
        int columns = Math.Max(1, (int)(availableWidth / (MinCardWidth + CardGap)));

        // 计算每个卡片的实际宽度
        // WrapPanel.ItemWidth 定义槽位宽度，Border.Margin="0,0,12,12"
        // 所以：槽位宽度 = cardWidth + gap
        // 可用宽度 = columns * (cardWidth + gap)
        // cardWidth = availableWidth / columns - gap
        double slotWidth = availableWidth / columns;
        double cardWidth = slotWidth - CardGap;

        wrapPanel.ItemWidth = slotWidth;
        wrapPanel.ItemHeight = double.NaN; // 自动高度
    }

    private void OnCardPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
        {
            if (sender is Border border && border.DataContext is ConfigProfile profile)
            {
                var viewModel = (MainViewModel?)DataContext;
                if (viewModel == null) return;

                // 关闭之前打开的菜单
                if (_currentMenu != null)
                {
                    _currentMenu.Close();
                    _currentMenu = null;
                }

                // 创建并打开新菜单
                var menu = new ContextMenu
                {
                    ItemsSource = CreateMenuItems(profile, viewModel)
                };

                // 跟踪当前菜单
                _currentMenu = menu;
                menu.Closed += (s, args) => _currentMenu = null;

                menu.Open(border);
            }
        }
    }

    private IList CreateMenuItems(ConfigProfile profile, MainViewModel viewModel)
    {
        var items = new ArrayList();

        // 应用
        if (!profile.IsReadonly)
        {
            items.Add(new MenuItem
            {
                Header = "应用",
                Command = viewModel.ApplyCommand,
                CommandParameter = profile
            });
        }

        // 编辑
        if (!profile.IsReadonly)
        {
            items.Add(new MenuItem
            {
                Header = "编辑",
                Command = viewModel.EditCommand,
                CommandParameter = profile
            });
        }

        // 复制
        items.Add(new MenuItem
        {
            Header = "复制",
            Command = viewModel.CopyCommand,
            CommandParameter = profile
        });

        // 分隔符
        if (!profile.IsReadonly)
        {
            items.Add(new Separator());

            // 删除
            items.Add(new MenuItem
            {
                Header = "删除",
                Command = viewModel.DeleteCommand,
                CommandParameter = profile
            });
        }

        return items;
    }
}