using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.Layout;

namespace VPet.Avalonia.Core.Display;

public partial class MessageControl : UserControl
{
    public MessageControl()
    {
        InitializeComponent();

        DataContextChanged += MessageControl_DataContextChanged;
    }

    private void MessageControl_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is MainModel model)
        {
            model.PropertyChanged += Model_PropertyChanged;
        }
    }

    private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainModel.MessageDisplay))
        {
            var model = (sender as MainModel)!;
            if (!model.MessageDisplay)
            {
                MessageBoxContent.Children.Clear();
            }
        }
    }

    public void SetPlaceIN()
    {
        Height = 500;
        BorderMain.VerticalAlignment = VerticalAlignment.Bottom;
        Margin = new Thickness(0);
    }
    public void SetPlaceOUT()
    {
        Height = double.NaN;
        BorderMain.VerticalAlignment = VerticalAlignment.Top;
        Margin = new Thickness(0, 500, 0, 0);
    }

    private void ContextMenu_Opened(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainModel model)
        {
            model.MessageStop();
        }
    }

    private void ContextMenu_Closed(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainModel model)
        {
            model.MessageStart();
        }
    }

    private void Border_PointerEntered(object? sender, PointerEventArgs e)
    {
        if (DataContext is MainModel model)
        {
            model.MessageStop();
        }
    }

    private void Border_PointerExited(object? sender, PointerEventArgs e)
    {
        if (DataContext is MainModel model)
        {
            model.MessageStart();
        }
    }

    private void TextBox_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        ScrollViewer1.ScrollToEnd();
    }
}