using System;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace VPet.Avalonia.Core.Graph;

/// <summary>
/// 可以通过图片模块运行该动画
/// </summary>
public interface IImageRun : IGraph
{
    /// <summary>
    /// 指定图像图像控件准备运行该动画
    /// </summary>
    /// <param name="img">用于显示的Image</param>
    /// <param name="endAction">结束动画</param>
    /// <returns>准备好的线程</returns>
    Task Run(Image img, Action? endAction = null);
}
