using System.Windows;
using Avalonia;

namespace HKW.WPF.Converters;

/// <summary>
/// 值转换器基础
/// </summary>
public abstract class ConverterBase : AvaloniaObject
{
    /// <summary>
    /// 未设置值
    /// </summary>
    public static readonly object UnsetValue = AvaloniaProperty.UnsetValue;
}
