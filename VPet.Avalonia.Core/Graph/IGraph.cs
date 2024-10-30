using System;
using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace VPet.Avalonia.Core.Graph;

/// <summary>
/// 动画显示接口
/// </summary>
public interface IGraph : IEquatable<object>
{
    /// <summary>
    /// 从0开始运行该动画
    /// </summary>
    /// <param name="EndAction">停止动作</param>
    /// <param name="parant">显示位置</param>
    void Run(Decorator parant, Action? EndAction = null);

    /// <summary>
    /// 是否循环播放
    /// </summary>
    bool IsLoop { get; set; }

    /// <summary>
    /// 是否准备完成
    /// </summary>
    bool IsReady { get; }

    /// <summary>
    /// 是否读取失败
    /// </summary>
    bool IsFail { get; }
    /// <summary>
    /// 失败报错信息
    /// </summary>
    string FailMessage { get; }
    /// <summary>
    /// 该动画信息
    /// </summary>
    GraphInfo GraphInfo { get; }

    /// <summary>
    /// 当前动画播放状态和控制
    /// </summary>
    TaskControl Control { get; }

    /// <summary>
    /// 停止动画
    /// </summary>
    /// <param name="StopEndAction">停止动画时是否不运行结束动画</param>
    void Stop(bool StopEndAction)
    {
        if (Control == null)
            return;
        if (StopEndAction)
            Control.EndAction = null;
        Control.Type = TaskControl.ControlType.Stop;
    }
    /// <summary>
    /// 设置为继续播放
    /// </summary>
    void SetContinue()
    {
        Control.Type = TaskControl.ControlType.Continue;
    }
}
