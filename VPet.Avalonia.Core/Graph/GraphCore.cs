using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using VPet.Avalonia.Core.Handle;
using VPet.Core;

namespace VPet.Avalonia.Core.Graph;

/// <summary>
/// 图像显示核心
/// </summary>
public class GraphCore
{
    public static string CachePath = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + @"\cache";

    public Config GraphConfig;

    /// <summary>
    /// 桌宠图形渲染的分辨率,越高图形越清晰
    /// </summary>
    public int Resolution { get; set; } = 1000;

    /// <summary>
    /// 图像名字字典: 动画类型->动画名字
    /// </summary>
    public Dictionary<GraphType, HashSet<string>> GraphsName = [];
    /// <summary>
    /// 图像字典 动画名字->状态+动作->动画
    /// </summary>
    public Dictionary<string, Dictionary<AnimatType, List<IGraph>>> GraphsList = [];
    /// <summary>
    /// 通用UI资源
    /// </summary>
    public Dictionary<string, Control> CommUIElements = [];
    /// <summary>
    /// 通用设置属性/方法
    /// </summary>
    public Dictionary<string, object> CommConfig = [];

    public GraphCore(int resolution)
    {
        if (!Directory.Exists(CachePath))
            Directory.CreateDirectory(CachePath);
        CommConfig["Cache"] = new List<string>();
        Resolution = resolution;
    }

    /// <summary>
    /// 添加动画
    /// </summary>
    /// <param name="graph">动画</param>
    public void AddGraph(IGraph graph)
    {
        if (graph.GraphInfo.Type != GraphType.Common)
        {
            if (!GraphsName.TryGetValue(graph.GraphInfo.Type, out var d2))
            {
                d2 = [];
                GraphsName.Add(graph.GraphInfo.Type, d2);
            }
            d2.Add(graph.GraphInfo.Name);
        }
        if (!GraphsList.TryGetValue(graph.GraphInfo.Name, out var d3))
        {
            d3 = [];
            GraphsList.Add(graph.GraphInfo.Name, d3);
        }
        if (!d3.TryGetValue(graph.GraphInfo.Animat, out var l3))
        {
            l3 = [];
            d3.Add(graph.GraphInfo.Animat, l3);
        }
        l3.Add(graph);
    }

    /// <summary>
    /// 获得随机动画名字
    /// </summary>
    /// <param name="type">动画类型</param>
    /// <returns>动画名字,找不到则返回null</returns>
    public string? FindName(GraphType type)
    {
        if (GraphsName.TryGetValue(type, out var gl))
        {
            return gl.ElementAt(Function.Rnd.Next(gl.Count));
        }
        return null;
    }

    /// <summary>
    /// 查找动画
    /// </summary>
    /// <param name="GraphName">动画名字</param>
    /// <param name="mode">状态类型,找不到就找相同动画类型</param>
    /// <param name="animat">动画的动作 Start Loop End</param>
    public IGraph? FindGraph(string GraphName, AnimatType animat, IGameSave.ModeType mode)
    {
        if (GraphName == null)
            return null;
        if (GraphsList.TryGetValue(GraphName, out var d3) && d3.TryGetValue(animat, out var gl))
        {
            var list = gl.FindAll(x => x.GraphInfo.ModeType == mode);
            if (list.Count > 0)
            {
                if (list.Count == 1)
                    return list[0];
                return list[Function.Rnd.Next(list.Count)];
            }
            if (mode == IGameSave.ModeType.Ill)
            {
                return null;
            }
            int i = (int)mode + 1;
            if (i < 3)
            {
                //向下兼容的动画
                list = gl.FindAll(x => x.GraphInfo.ModeType == (IGameSave.ModeType)i);
                if (list.Count > 0)
                    return list[Function.Rnd.Next(list.Count)];
            }
            i = (int)mode - 1;
            if (i >= 1)
            {
                //向上兼容的动画
                list = gl.FindAll(x => x.GraphInfo.ModeType == (IGameSave.ModeType)i);
                if (list.Count > 0)
                    return list[Function.Rnd.Next(list.Count)];
            }
            //如果实在找不到,就走随机数(无生病)
            list = gl.FindAll(x => x.GraphInfo.ModeType != IGameSave.ModeType.Ill);
            if (list.Count > 0)
                return list[Function.Rnd.Next(list.Count)];
        }
        return null;// FindGraph(GraphType.Default, mode);
    }
    /// <summary>
    /// 查找动画列表
    /// </summary>
    /// <param name="mode">状态类型,找不到就找相同动画类型</param>
    /// <param name="animat">动画的动作 Start Loop End</param>
    public List<IGraph>? FindGraphs(string GraphName, AnimatType animat, IGameSave.ModeType mode)
    {
        if (GraphName == null)
            return null;
        if (GraphsList.TryGetValue(GraphName, out var d3) && d3.TryGetValue(animat, out var gl))
        {
            var list = gl.FindAll(x => x.GraphInfo.ModeType == mode);
            if (list.Count > 0)
            {
                return list;
            }
            int i = (int)mode + 1;
            if (i < 3)
            {
                //向下兼容的动画
                list = gl.FindAll(x => x.GraphInfo.ModeType == (IGameSave.ModeType)i);
                if (list.Count > 0)
                    return list;
            }
            i = (int)mode - 1;
            if (i >= 0)
            {
                //向上兼容的动画
                list = gl.FindAll(x => x.GraphInfo.ModeType == (IGameSave.ModeType)i);
                if (list.Count > 0)
                    return list;
            }
            //如果实在找不到,就走随机数
            //if (mode != GameSave.ModeType.Ill)
            //{
            list = gl;
            if (list.Count > 0)
                return list;
            //}                
        }
        return [];// FindGraph(GraphType.Default, mode);
    }
}
