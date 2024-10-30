using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace VPet.Avalonia.Core.Graph;

/// <summary>
/// 指示该ImageRun支持
/// </summary>
public interface IRunImage : IGraph
{
    /// <summary>
    /// 从0开始运行该动画
    /// </summary>
    /// <param name="parant">显示位置</param>
    /// <param name="endAction">结束方法</param>
    /// <param name="image">额外图片</param>
    void Run(Decorator parant, Bitmap image, Action? endAction = null);
}
