using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace VPet.Avalonia.Core.Graph;

public static class GraphHelper
{
    internal static string[][]? graphtypevalue;
    /// <summary>
    /// 动画类型默认前文本
    /// </summary>
    public static string[][] GraphTypeValue
    {
        get
        {
            if (graphtypevalue == null)
            {
                var gtv = new List<string[]>();
                foreach (string v in Enum.GetNames(typeof(GraphType)))
                {
                    gtv.Add(v.ToLower().Split('_'));
                }
                graphtypevalue = [.. gtv];
            }
            return graphtypevalue;
        }
    }

    /// <summary>
    /// 使用RunImage 从0开始运行该动画 若无RunImage 则使用Run
    /// </summary>
    /// <param name="graph">动画接口</param>
    /// <param name="parant">显示位置</param>
    /// <param name="endAction">结束方法</param>
    /// <param name="image">额外图片</param>
    public static void Run(this IGraph graph, Decorator parant, Bitmap image, Action? endAction = null)
    {
        if (graph is IRunImage iri)
        {
            iri.Run(parant, image, endAction);
        }
        else
        {
            graph.Run(parant, endAction);
        }
    }

    /// <summary>
    /// 使用ImageRun 指定图像图像控件准备运行该动画
    /// </summary>
    /// <param name="graph">动画接口</param>
    /// <param name="img">用于显示的Image</param>
    /// <param name="endAction">结束动画</param>
    /// <returns>准备好的线程</returns>
    public static Task? Run(this IGraph graph, Image img, Action? endAction = null)
    {
        if (graph is IImageRun iri)
        {
            return iri.Run(img, endAction);
        }
        else
        {
            return null;
        }
    }
}
