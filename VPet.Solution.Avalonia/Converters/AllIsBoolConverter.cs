using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using Avalonia;
using HKW.WPF.Converters;

namespace VPet.Solution.Avalonia.Converters;

/// <summary>
/// 全部为布尔值转换器
/// </summary>
public class AllIsBoolConverter : MultiValueToBoolConverter<AllIsBoolConverter>
{
    /// <summary>
    ///
    /// </summary>
    public static readonly StyledProperty<bool> BoolOnNullProperty = 
        AvaloniaProperty.Register<AllIsBoolConverter, bool>(nameof(BoolOnNull));

    /// <summary>
    /// 目标为空时的指定值
    /// </summary>
    [DefaultValue(false)]
    public bool BoolOnNull
    {
        get => (bool)GetValue(BoolOnNullProperty);
        set => SetValue(BoolOnNullProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="values"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public override object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        var boolValue = TargetBoolValue;
        var nullValue = BoolOnNull;
        return values.All(o => HKWUtils.GetBool(o, boolValue, nullValue));
    }
}
