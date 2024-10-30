using System;
using Avalonia;
using Avalonia.Media;
using LinePutScript.Converter;
using VPet.Avalonia.Core.Display;
using VPet.Avalonia.Core.Handle;
using VPet.Core;

namespace VPet.Avalonia.Core.Graph;

/// <summary>
/// 类型
/// </summary>
public enum WorkType { Work, Study, Play }

/// <summary>
/// 工作/学习
/// </summary>
public class Work : ICloneable
{
    /// <summary>
    /// 工作/学习
    /// </summary>
    [Line(ignoreCase: true)]
    public WorkType Type { get; set; }
    /// <summary>
    /// 工作名称
    /// </summary>
    [Line(ignoreCase: true)]
    public string Name { get; set; }
    /// <summary>
    /// 工作名称 已翻译
    /// </summary>
    public string NameTrans => Name.Translate();
    /// <summary>
    /// 使用动画名称
    /// </summary>
    [Line(ignoreCase: true, converter: typeof(Function.LPSConvertToLower))]
    public string Graph { get; set; }
    /// <summary>
    /// 工作盈利/学习基本倍率
    /// </summary>
    [Line(ignoreCase: true)]
    public double MoneyBase { get; set; }
    /// <summary>
    /// 工作体力(食物)消耗倍率
    /// </summary>
    [Line(ignoreCase: true)]
    public double StrengthFood { get; set; }
    /// <summary>
    /// 工作体力(饮料)消耗倍率
    /// </summary>
    [Line(ignoreCase: true)]
    public double StrengthDrink { get; set; }
    /// <summary>
    /// 心情消耗倍率
    /// </summary>
    [Line(ignoreCase: true)]
    public double Feeling { get; set; }
    /// <summary>
    /// 等级限制
    /// </summary>
    [Line(ignoreCase: true)]
    public int LevelLimit { get; set; }
    /// <summary>
    /// 花费时间(分钟)
    /// </summary>
    [Line(ignoreCase: true)]
    public int Time { get; set; }
    /// <summary>
    /// 完成奖励倍率(0+)
    /// </summary>
    [Line(ignoreCase: true)]
    public double FinishBonus { get; set; }


    [Line(ignoreCase: true)]
    public string BorderBrush = "0290D5";
    [Line(ignoreCase: true)]
    public string Background = "81d4fa";
    [Line(ignoreCase: true)]
    public string ButtonBackground = "0286C6";
    [Line(ignoreCase: true)]
    public string ButtonForeground = "ffffff";
    [Line(ignoreCase: true)]
    public string Foreground = "0286C6";
    [Line(ignoreCase: true)]
    public double Left = 100;
    [Line(ignoreCase: true)]
    public double Top = 160;
    [Line(ignoreCase: true)]
    public double Width = 300;

    public void SetStyle(WorkTimerControl wt)
    {
        wt.Margin = new Thickness(Left, Top, 0, 0);
        wt.Width = Width;
        wt.Height = Width / 300 * 180;
        wt.Resources.Clear();
        wt.Resources.Add("BorderBrush", new SolidColorBrush(Color.Parse("#FF" + BorderBrush)));
        wt.Resources.Add("Background", new SolidColorBrush(Color.Parse("#FF" + Background)));
        wt.Resources.Add("ButtonBackground", new SolidColorBrush(Color.Parse("#AA" + ButtonBackground)));
        wt.Resources.Add("ButtonBackgroundHover", new SolidColorBrush(Color.Parse("#FF" + ButtonBackground)));
        wt.Resources.Add("ButtonForeground", new SolidColorBrush(Color.Parse("#FF" + ButtonForeground)));
        wt.Resources.Add("Foreground", new SolidColorBrush(Color.Parse("#FF" + Foreground)));
    }
    /// <summary>
    /// 显示工作/学习动画
    /// </summary>
    /// <param name="m"></param>
    public void Display(MainModel m)
    {
        m.Display(Graph, AnimatType.A_Start, () => m.DisplayBLoopingForce(Graph));
    }
    /// <summary>
    /// 克隆相同的工作/学习
    /// </summary>
    public object Clone()
    {
        return new Work
        {
            Type = Type,
            Name = Name,
            Graph = Graph,
            MoneyBase = MoneyBase,
            StrengthFood = StrengthFood,
            StrengthDrink = StrengthDrink,
            Feeling = Feeling,
            LevelLimit = LevelLimit,
            Time = Time,
            FinishBonus = FinishBonus,
            BorderBrush = BorderBrush,
            Background = Background,
            ButtonBackground = ButtonBackground,
            ButtonForeground = ButtonForeground,
            Foreground = Foreground,
            Left = Left,
            Top = Top,
            Width = Width
        };
    }
}