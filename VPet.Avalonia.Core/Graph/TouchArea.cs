using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;

namespace VPet.Avalonia.Core.Graph;

/// <summary>
/// 触摸范围事件
/// </summary>
public class TouchArea
{
    /// <summary>
    /// 位置
    /// </summary>
    public Point Locate;
    /// <summary>
    /// 大小
    /// </summary>
    public Size Size;
    /// <summary>
    /// 如果是触发的内容
    /// </summary>
    public Func<bool> DoAction;
    /// <summary>
    /// 否:立即触发/是:长按触发
    /// </summary>
    public bool IsPress;
    /// <summary>
    /// 创建个触摸范围事件
    /// </summary>
    /// <param name="locate">位置</param>
    /// <param name="size">大小</param>
    /// <param name="doAction">如果是触发的内容</param>
    /// <param name="isPress">否:立即触发/是:长按触发</param>
    public TouchArea(Point locate, Size size, Func<bool> doAction, bool isPress = false)
    {
        Locate = locate;
        Size = size;
        DoAction = doAction;
        IsPress = isPress;
    }
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