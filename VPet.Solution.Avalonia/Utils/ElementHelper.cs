using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace VPet.Solution.Avalonia.Utils;

/// <summary>
///
/// </summary>
public class ElementHelper : AvaloniaObject
{
    /// <summary>
    /// 清除元素焦点
    /// </summary>
    /// <param name="obj">元素</param>
    public static void ClearFocus(Control obj)
    {
        var root = obj.GetVisualRoot();
        if (root is Window topLevel)
        {
            topLevel.Focus();
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static string GetClearFocusOnKeyDown(Control element)
    {
        return element.GetValue(ClearFocusOnKeyDownProperty);
    }

    /// <summary>
    ///
    /// </summary>
    /// <exception cref="Exception">禁止使用此方法</exception>
    public static void SetClearFocusOnKeyDown(Control element, string value)
    {
        element.SetValue(ClearFocusOnKeyDownProperty, value);
    }

    /// <summary>
    /// 在按下指定按键时清除选中状态
    /// </summary>
    public static AttachedProperty<string> ClearFocusOnKeyDownProperty =
        AvaloniaProperty.RegisterAttached<ElementHelper, string>(
            "ClearFocusOnKeyDown", typeof(ElementHelper),
             coerce: ClearFocusOnKeyDownPropertyChanged);

    private static string ClearFocusOnKeyDownPropertyChanged(
        AvaloniaObject obj, string e)
    {
        if (obj is not Control element)
            return e;
        var keyName = GetClearFocusOnKeyDown(element);
        if (Enum.TryParse<Key>(keyName, false, out _) is false)
            throw new Exception($"Unknown key {keyName}");
        element.KeyDown -= Element_KeyDown;
        element.KeyDown += Element_KeyDown;

        static void Element_KeyDown(object? sender, KeyEventArgs e)
        {
            if (sender is not Control element)
                return;
            var key = (Key)Enum.Parse(typeof(Key), GetClearFocusOnKeyDown(element));
            if (e.Key == key)
            {
                // 清除控件焦点
                ClearFocus(element);
            }
        }

        return e;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static string GetUniformMinWidthGroup(Control element)
    {
        return element.GetValue(UniformWidthGroupProperty);
    }

    /// <summary>
    ///
    /// </summary>
    public static void SetUniformMinWidthGroup(Control element, string value)
    {
        element.SetValue(UniformWidthGroupProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    public static readonly AttachedProperty<string> UniformWidthGroupProperty =
         AvaloniaProperty.RegisterAttached<ElementHelper, string>(
            "UniformMinWidthGroup", typeof(ElementHelper),
             coerce: UniformMinWidthGroupPropertyChanged);

    /// <summary>
    /// (TopParent ,(GroupName, UniformMinWidthGroupInfo))
    /// </summary>
    private readonly static Dictionary<Control,
        Dictionary<string, UniformMinWidthGroupInfo>> _uniformMinWidthGroups = [];

    private static string UniformMinWidthGroupPropertyChanged(
        AvaloniaObject obj, string e)
    {
        if (obj is not Control element)
            return e;
        var groupName = GetUniformMinWidthGroup(element);
        var topParent = element.GetVisualRoot() as Window;
        // 在设计器中会无法获取顶级元素, 会提示错误, 忽略即可
        if (topParent is null)
            return e;
        if (_uniformMinWidthGroups.TryGetValue(topParent, out var groups) is false)
        {
            Dispatcher.UIThread.ShutdownStarted += Dispatcher_ShutdownStarted;
            groups = _uniformMinWidthGroups[topParent] = [];
        }
        if (groups.TryGetValue(groupName, out var group) is false)
            group = groups[groupName] = new();
        group.Elements.Add(element);

        element.SizeChanged -= Element_SizeChanged;
        element.SizeChanged += Element_SizeChanged;

        static void Dispatcher_ShutdownStarted(object? sender, EventArgs e)
        {
            if (sender is not Control element)
                return;
            _uniformMinWidthGroups.Remove(element);
        }

        static void Element_SizeChanged(object? sender, SizeChangedEventArgs e)
        {
            if (sender is not Control element)
                return;
            var groupName = GetUniformMinWidthGroup(element);
            var topParent = (element.GetVisualRoot() as Window)!;
            var groups = _uniformMinWidthGroups[topParent];
            var group = groups[groupName];
            var maxWidthElement = group.Elements.MaxBy(i => i.Bounds.Width);
            if (maxWidthElement is null)
                return;

            if (maxWidthElement.Bounds.Width == element.Bounds.Width)
                maxWidthElement = element;
            if (maxWidthElement.Bounds.Width > group.MaxWidth)
            {
                // 如果当前控件最大宽度的超过历史最大宽度, 表明非最大宽度列表中的控件超过了历史最大宽度
                foreach (var item in group.Elements)
                    item.MinWidth = maxWidthElement.Bounds.Width;
                // 将当前控件最小宽度设为0
                maxWidthElement.MinWidth = 0;
                group.MaxWidthElements.Clear();
                // 设为最大宽度的唯一控件
                group.MaxWidthElements.Add(maxWidthElement);
                group.MaxWidth = maxWidthElement.Bounds.Width;
            }
            else if (group.MaxWidthElements.Count == 1)
            {
                maxWidthElement = group.MaxWidthElements.First();
                // 当最大宽度控件只有一个时, 并且当前控件宽度小于历史最大宽度时, 表明需要降低全体宽度
                if (group.MaxWidth > maxWidthElement.Bounds.Width)
                {
                    // 最小宽度设为0以自适应宽度
                    foreach (var item in group.Elements)
                        item.MinWidth = 0;
                    // 清空最大宽度列表, 让其刷新
                    group.MaxWidthElements.Clear();
                }
            }
            else
            {
                // 将 MaxWidth 设置为 double.MaxValue 时, 可以让首次加载时进入此处
                foreach (var item in group.Elements)
                {
                    // 当控件最小宽度为0(表示其为主导宽度的控件), 并且其宽度等于最大宽度, 加入最大宽度列表
                    if (item.MinWidth == 0 && item.Bounds.Width == maxWidthElement.Bounds.Width)
                    {
                        group.MaxWidthElements.Add(item);
                    }
                    else
                    {
                        // 如果不是, 则从最大宽度列表删除, 并设置最小宽度为当前最大宽度
                        group.MaxWidthElements.Remove(item);
                        item.MinWidth = maxWidthElement.Bounds.Width;
                    }
                }
                group.MaxWidth = maxWidthElement.Bounds.Width;
            }
        }

        return e;
    }
}

/// <summary>
/// 统一最小宽度分组信息
/// </summary>
public class UniformMinWidthGroupInfo
{
    /// <summary>
    /// 最后一个最大宽度
    /// </summary>
    public double MaxWidth { get; set; } = double.MaxValue;

    /// <summary>
    /// 所有控件
    /// </summary>
    public HashSet<Control> Elements { get; } = [];

    /// <summary>
    /// 最大宽度的控件
    /// </summary>
    public HashSet<Control> MaxWidthElements { get; } = [];
}
