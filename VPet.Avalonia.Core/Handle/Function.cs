﻿using System;
using Avalonia;
using Avalonia.Media;
using LinePutScript.Converter;

namespace VPet.Avalonia.Core.Handle;

public static partial class Function
{
    /// <summary>
    /// HEX值转颜色
    /// </summary>
    /// <param name="hex">HEX值</param>
    /// <returns>颜色</returns>
    public static Color HEXToColor(string hex) => Color.Parse(hex);
    /// <summary>
    /// 颜色转HEX值
    /// </summary>
    /// <param name="color">颜色</param>
    /// <returns>HEX值</returns>
    public static string ColorToHEX(Color color) => "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

    public static Random Rnd { get; } = new();
    /// <summary>
    /// 获取资源笔刷
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Brush ResourcesBrush(BrushType name)
    {
        return (Brush)Application.Current!.Resources[name.ToString()]!;
    }
    public enum BrushType
    {
        Primary,
        PrimaryTrans,
        PrimaryTrans4,
        PrimaryTransA,
        PrimaryTransE,
        PrimaryLight,
        PrimaryLighter,
        PrimaryDark,
        PrimaryDarker,
        PrimaryText,

        Secondary,
        SecondaryTrans,
        SecondaryTrans4,
        SecondaryTransA,
        SecondaryTransE,
        SecondaryLight,
        SecondaryLighter,
        SecondaryDark,
        SecondaryDarker,
        SecondaryText,

        DARKPrimary,
        DARKPrimaryTrans,
        DARKPrimaryTrans4,
        DARKPrimaryTransA,
        DARKPrimaryTransE,
        DARKPrimaryLight,
        DARKPrimaryLighter,
        DARKPrimaryDark,
        DARKPrimaryDarker,
        DARKPrimaryText,
    }
    ///// <summary>
    ///// 翻译文本
    ///// </summary>
    ///// <param name="TranFile">翻译文件</param>
    ///// <param name="Name">翻译内容</param>
    ///// <returns>翻译后的文本</returns>
    //public static string Translate(this LPS_D TranFile, string Name) => TranFile.GetString(Name, Name);

    public class LPSConvertToLower : LPSConvert.ConvertFunction
    {
        public override string Convert(dynamic value) => value;

        public override dynamic ConvertBack(string info) => info.ToLower();
    }
}
