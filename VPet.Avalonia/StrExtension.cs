using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Data.Core;
using Avalonia.Markup.Xaml;
using VPet.Core;

namespace VPet.Avalonia;

/// <summary>
/// 绑定转换成字符串
/// </summary>
public class StrExtension : MarkupExtension
{
    /// <summary>
    /// 查找值
    /// </summary>
    public string? Key { get; set; }
    /// <summary>
    /// 查找值
    /// </summary>
    public Binding? KeySource { get; set; }
    /// <summary>
    /// 替换词
    /// </summary>
    public object? Value { get; set; }
    /// <summary>
    /// 替换词
    /// </summary>
    public Binding? ValueSource { get; set; }
    /// <summary>
    /// WPF绑定转换字符串
    /// </summary>
    /// <param name="key">查找值</param>
    public StrExtension(string key)
    {
        Key = key;
    }
    /// <summary>
    /// WPF绑定转换字符串
    /// </summary>
    /// <param name="keySource">查找值</param>
    /// <param name="valueSource">默认值</param>
    public StrExtension(Binding keySource, Binding valueSource)
    {
        KeySource = keySource;
        ValueSource = valueSource;
    }
    /// <summary>
    /// WPF绑定转换字符串
    /// </summary>
    /// <param name="keySource">查找值</param>
    /// <param name="value">默认值</param>
    public StrExtension(Binding keySource, object value)
    {
        KeySource = keySource;
        Value = value;
    }
    /// <summary>
    /// WPF绑定转换字符串
    /// </summary>
    /// <param name="key">查找值</param>
    /// <param name="value">默认值</param>
    public StrExtension(string key, object value)
    {
        Key = key;
        Value = value;
    }
    /// <summary>
    /// WPF绑定转换字符串
    /// </summary>
    /// <param name="key">查找值</param>
    /// <param name="valueSource">默认值</param>
    public StrExtension(string key, Binding valueSource)
    {
        Key = key;
        ValueSource = valueSource;
    }
    ///// <summary>
    ///// 获得Binding的值
    ///// </summary>
    ///// <param name="bindingexpression">BindingExpression</param>
    ///// <returns>Binding的值</returns>
    //public static object? GetBindingValue(object bindingexpression)
    //{
    //    if (bindingexpression is BindingExpression be)
    //    {
    //        object? itemsSourceObject = be.ResolvedSource;
    //        string? itemSourceProperty = be.ResolvedSourcePropertyName;
    //        if (itemsSourceObject == null || itemSourceProperty == null)
    //        {
    //            return null;
    //        }
    //        else
    //        {
    //            return itemsSourceObject.GetType().GetProperty(itemSourceProperty).GetGetMethod().Invoke(itemsSourceObject, null);
    //        }
    //    }
    //    else
    //        return null;
    //}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        MultiBinding Binding = new MultiBinding
        {
            Converter = new LocalConverter(Key, Value)
        };
        Binding.Bindings.Add(new Binding()
        {
            Source = LocalizeCore.BindingNotify,
            Path = "TmpString"
        });
        if (KeySource != null)
        {
            Binding.Bindings.Add(KeySource);
        }
        if (ValueSource != null)
        {
            Binding.Bindings.Add(ValueSource);
        }
        return Binding;
    }

    private class LocalConverter : IMultiValueConverter
    {
        /// <summary>
        /// 查找值
        /// </summary>
        public string? Key { get; set; }
        /// <summary>
        /// 替换词
        /// </summary>
        public object? Value { get; set; }
        /// <summary>
        /// 生成一个WPF绑定转换成字符串, 开发者无需使用这个
        /// </summary>
        public LocalConverter(string? key = null, object? value = null)
        {
            Key = key;
            Value = value;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => [value.ToString() ?? ""];

        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            string? k = null;
            if (Key != null)
            {
                k = Key;
            }
            else if (values.Count == 2)
            {
                k = System.Convert.ToString(values[1]);
            }
            object? v = null;
            if (Value != null)
            {
                v = Value;
            }
            else if (Key != null && values.Count == 2)
            {
                v = values[1];
            }
            else if (values.Count == 3)
            {
                v = values[2];
            }
            if (v == null)
                return (k ?? "").Translate();
            else
                return (k ?? "").Translate(v);
        }
    }
}
