using System;
using System.Timers;
using Avalonia;
using VPet.Avalonia.Core.Graph;
using VPet.Avalonia.Core.Handle;

namespace VPet.Avalonia.Core.Display;

/// <summary>
/// 当前正在的状态
/// </summary>
public enum WorkingState
{
    /// <summary>
    /// 默认:啥都没干
    /// </summary>
    Nomal,
    /// <summary>
    /// 正在干活/学习中
    /// </summary>
    Work,
    /// <summary>
    /// 睡觉
    /// </summary>
    Sleep,
    /// <summary>
    /// 旅游中
    /// </summary>
    Travel,
    /// <summary>
    /// 其他状态,给开发者留个空位计算
    /// </summary>
    Empty,
}

public partial class MainModel
{
    /// <summary>
    /// 默认循环次数
    /// </summary>
    public int CountNomal = 0;

    /// <summary>
    /// 定点移动位置向量
    /// </summary>
    public Point MoveTimerPoint = new();
    /// <summary>
    /// 定点移动定时器
    /// </summary>
    public Timer MoveTimer = new();

    public bool MoveTimerSmartMove = false;

    /// <summary>
    /// 当前状态
    /// </summary>
    public WorkingState State = WorkingState.Nomal;
    /// <summary>
    /// 显示默认情况, 默认为默认动画
    /// </summary>
    public Action DisplayNomal;
    /// <summary>
    /// 当前动画类型
    /// </summary>
    public GraphInfo DisplayType = new GraphInfo("");

    private int looptimes;

    /// <summary>
    /// 以标准形式显示当前默认状态
    /// </summary>
    public void DisplayToNomal()
    {
        switch (State)
        {
            default:
            case WorkingState.Nomal:
                DisplayNomal();
                return;
            case WorkingState.Sleep:
                DisplaySleep(true);
                return;
            case WorkingState.Work:
                NowWork.Display(this);
                return;
            case WorkingState.Travel:
                //TODO
                return;
        }
    }

    /// <summary>
    /// 显示睡觉情况
    /// </summary>
    public void DisplaySleep(bool force = false)
    {
        looptimes = 0;
        CountNomal = 0;
        if (force)
        {
            State = WorkingState.Sleep;
            Display(GraphType.Sleep, AnimatType.A_Start, DisplayBLoopingForce);
        }
        else
            Display(GraphType.Sleep, AnimatType.A_Start, (x) => DisplayBLoopingToNomal(x, Core.Graph.GraphConfig.GetDuration(x)));
    }

    /// <summary>
    /// 显示B循环 (强制)
    /// </summary>
    public void DisplayBLoopingForce(string graphname)
    {
        Display(graphname, AnimatType.B_Loop, DisplayBLoopingForce);
    }

    /// <summary>
    /// 显示B循环+C循环+ToNomal
    /// </summary>
    public Action<string> DisplayBLoopingToNomal(int looplength) 
        => (gn) => DisplayBLoopingToNomal(gn, looplength);

    /// <summary>
    /// 显示B循环+C循环+ToNomal
    /// </summary>
    public void DisplayBLoopingToNomal(string graphname, int loopLength)
    {
        if (Function.Rnd.Next(++looptimes) > loopLength)
            DisplayCEndtoNomal(graphname);
        else
            Display(graphname, AnimatType.B_Loop, DisplayBLoopingToNomal(loopLength));
    }

    /// <summary>
    /// 显示结束动画到正常动画 (DisplayToNomal)
    /// </summary>
    public void DisplayCEndtoNomal(string graphname)
    {
        Display(graphname, AnimatType.C_End, DisplayToNomal);
    }

    /// <summary>
    /// 显示动画 (自动查找和匹配)
    /// </summary>
    /// <param name="Type">动画类型</param>
    /// <param name="endAction">动画结束后操作(附带名字)</param>
    /// <param name="animat">动画的动作 Start Loop End</param>
    public void Display(GraphType Type, AnimatType animat, Action<string>? endAction = null)
    {
        var name = Core.Graph.FindName(Type);
        if (name == null)
        {
            return;
        }
        Display(name, animat, endAction);
    }
    /// <summary>
    /// 显示动画 根据名字播放
    /// </summary>
    /// <param name="name">动画名称</param>
    /// <param name="endAction">动画结束后操作(附带名字)</param>
    /// <param name="animat">动画的动作 Start Loop End</param>
    public void Display(string name, AnimatType animat, Action<string>? endAction)
    {
        Display(Core.Graph.FindGraph(name, animat, Core.Save.Mode), () => endAction?.Invoke(name));
    }
    /// <summary>
    /// 显示动画 根据名字和类型查找运行,若无则查找类型
    /// </summary>
    /// <param name="Type">动画类型</param>
    /// <param name="name">动画名称</param>
    /// <param name="endAction">动画结束后操作(附带名字)</param>
    /// <param name="animat">动画的动作 Start Loop End</param>
    public void Display(string name, AnimatType animat, GraphType Type, Action<string>? endAction = null)
    {
        var list = Core.Graph.FindGraphs(name, animat, Core.Save.Mode)?.FindAll(x => x.GraphInfo.Type == Type);
        if (list?.Count > 0)
            Display(list[Function.Rnd.Next(list.Count)], () => endAction?.Invoke(name));
        else
            Display(Type, animat, endAction);
    }
    /// <summary>
    /// 显示动画 根据名字和类型查找运行,若无则查找类型
    /// </summary>
    /// <param name="type">动画类型</param>
    /// <param name="name">动画名称</param>
    /// <param name="endAction">动画结束后操作</param>
    /// <param name="animat">动画的动作 Start Loop End</param>
    public void Display(string name, AnimatType animat, GraphType type, Action? endAction = null)
    {
        var list = Core.Graph.FindGraphs(name, animat, Core.Save.Mode)?.FindAll(x => x.GraphInfo.Type == type);
        if (list?.Count > 0)
            Display(list[Function.Rnd.Next(list.Count)], endAction);
        else
            Display(type, animat, endAction);
    }

    /// <summary>
    /// 显示动画 (自动查找和匹配)
    /// </summary>
    /// <param name="type">动画类型</param>
    /// <param name="endAction">动画结束后操作</param>
    /// <param name="animat">动画的动作 Start Loop End</param>
    public void Display(GraphType type, AnimatType animat, Action? endAction = null)
    {
        var name = Core.Graph.FindName(type);
        if (name == null)
        {
            return;
        }
        
        Display(name, animat, endAction);
    }
    /// <summary>
    /// 显示动画 根据名字播放
    /// </summary>
    /// <param name="name">动画名称</param>
    /// <param name="endAction">动画结束后操作</param>
    /// <param name="animat">动画的动作 Start Loop End</param>
    public void Display(string name, AnimatType animat, Action? endAction = null)
    {
        Display(Core.Graph.FindGraph(name, animat, Core.Save.Mode), endAction);
    }

    public void Display(IGraph? graph, Action? endAction = null)
    {
        if (graph == null)
        {
            return;
        }

        //TODO
    }
}
