using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;

namespace ConsoleApp1.Controls;

/// <summary>
/// 简单的消息框
/// </summary>
public class MessageBox : Window
{
    private MessageBoxResult _result = MessageBoxResult.None;

    public static async Task<MessageBoxResult> Show(Window parent, string message, string title = "提示",
        MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.None)
    {
        var messageBox = new MessageBox
        {
            Title = title,
            Width = 400,
            Height = 200,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
            SystemDecorations = SystemDecorations.BorderOnly,
            SizeToContent = SizeToContent.WidthAndHeight
        };

        var panel = new StackPanel { Spacing = 16, Margin = new Thickness(20) };

        // 图标和消息
        var contentPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 12 };

        if (icon != MessageBoxIcon.None)
        {
            var iconText = icon switch
            {
                MessageBoxIcon.Question => "❓",
                MessageBoxIcon.Warning => "⚠️",
                MessageBoxIcon.Error => "❌",
                MessageBoxIcon.Information => "ℹ️",
                _ => ""
            };
            contentPanel.Children.Add(new TextBlock
            {
                Text = iconText,
                FontSize = 24,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
            });
        }

        var messageTextBlock = new TextBlock
        {
            Text = message,
            TextWrapping = TextWrapping.Wrap,
            MaxWidth = 350,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            Foreground = Application.Current?.FindResource("TextPrimaryBrush") as IBrush ?? Brushes.White
        };
        contentPanel.Children.Add(messageTextBlock);
        panel.Children.Add(contentPanel);

        // 按钮
        var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Spacing = 10 };

        if (buttons == MessageBoxButton.OK || buttons == MessageBoxButton.OKCancel)
        {
            var okButton = new Button
            {
                Content = "确定",
                MinWidth = 80,
                Classes = { "accent" }
            };
            okButton.Click += (s, e) =>
            {
                messageBox._result = MessageBoxResult.OK;
                messageBox.Close();
            };
            buttonPanel.Children.Add(okButton);
        }

        if (buttons == MessageBoxButton.YesNo || buttons == MessageBoxButton.YesNoCancel || buttons == MessageBoxButton.OKCancel)
        {
            if (buttons == MessageBoxButton.YesNo || buttons == MessageBoxButton.YesNoCancel)
            {
                var yesButton = new Button
                {
                    Content = "是",
                    MinWidth = 80,
                    Classes = { "accent" }
                };
                yesButton.Click += (s, e) =>
                {
                    messageBox._result = MessageBoxResult.Yes;
                    messageBox.Close();
                };
                buttonPanel.Children.Add(yesButton);
            }

            var cancelButton = new Button { Content = "否", MinWidth = 80 };
            cancelButton.Click += (s, e) =>
            {
                messageBox._result = MessageBoxResult.No;
                messageBox.Close();
            };
            buttonPanel.Children.Add(cancelButton);
        }
        else if (buttons == MessageBoxButton.OKCancel)
        {
            var cancelButton = new Button { Content = "取消", MinWidth = 80 };
            cancelButton.Click += (s, e) =>
            {
                messageBox._result = MessageBoxResult.Cancel;
                messageBox.Close();
            };
            buttonPanel.Children.Add(cancelButton);
        }

        panel.Children.Add(buttonPanel);
        messageBox.Content = panel;

        await messageBox.ShowDialog(parent);
        return messageBox._result;
    }
}

public enum MessageBoxResult
{
    None,
    Yes,
    No,
    OK,
    Cancel
}

public enum MessageBoxButton
{
    OK,
    OKCancel,
    YesNo,
    YesNoCancel
}

public enum MessageBoxIcon
{
    None,
    Information,
    Question,
    Warning,
    Error
}
