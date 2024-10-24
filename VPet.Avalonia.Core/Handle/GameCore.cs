using System;
using System.Collections.Generic;
using Avalonia;
using VPet.Avalonia.Core.Graph;

namespace VPet.Avalonia.Core.Handle;

/// <summary>
/// 游戏使用资源
/// </summary>
public class GameCore
{
    /// <summary>
    /// 控制器
    /// </summary>
    public IController Controller;
    /// <summary>
    /// 触摸范围和事件列表
    /// </summary>
    public List<TouchArea> TouchEvent = [];
    /// <summary>
    /// 图形核心
    /// </summary>
    public GraphCore Graph;
    /// <summary>
    /// 游戏数据
    /// </summary>
    public IGameSave Save;
}
/// <remarks>
/// 触摸范围事件
/// 创建个触摸范围事件
/// </remarks>
/// <param name="locate">位置</param>
/// <param name="size">大小</param>
/// <param name="doAction">如果是触发的内容</param>
/// <param name="isPress">否:立即触发/是:长按触发</param>
public class TouchArea(Point locate, Size size, Func<bool> doAction, bool isPress = false)
{
    /// <summary>
    /// 位置
    /// </summary>
    public Point Locate = locate;
    /// <summary>
    /// 大小
    /// </summary>
    public Size Size = size;
    /// <summary>
    /// 如果是触发的内容
    /// </summary>
    public Func<bool> DoAction = doAction;
    /// <summary>
    /// 否:立即触发/是:长按触发
    /// </summary>
    public bool IsPress = isPress;

    /// <summary>
    /// 判断是否成功触发该点击事件
    /// </summary>
    /// <param name="point">位置</param>
    /// <returns>是否成功</returns>
    public bool Touch(Point point)
    {
        double inx = point.X - Locate.X;
        double iny = point.Y - Locate.Y;
        return inx >= 0 && inx <= Size.Width && iny >= 0 && iny <= Size.Height;
    }
}
