using System;

namespace VPet.Avalonia.Core.Graph;

/// <summary>
/// 动画控制类
/// </summary>
/// <remarks>
/// 为动画控制类提供操作和结束动作
/// </remarks>
/// <param name="endAction"></param>
public class TaskControl(Action? endAction = null)
{
    /// <summary>
    /// 当前动画播放状态
    /// </summary>
    public bool PlayState => Type != ControlType.Status_Stoped && Type != ControlType.Stop;
    /// <summary>
    /// 设置为继续播放
    /// </summary>
    public void SetContinue() { Type = ControlType.Continue; }
    /// <summary>
    /// 停止播放
    /// </summary>
    public void Stop(Action? endAction = null) { EndAction = endAction; Type = ControlType.Stop; }
    /// <summary>
    /// 控制类型
    /// </summary>
    public enum ControlType
    {
        /// <summary>
        /// 维持现状, 不进行任何超控
        /// </summary>
        Status_Quo,
        /// <summary>
        /// 停止当前动画
        /// </summary>
        Stop,
        /// <summary>
        /// 播放完成后继续播放,仅生效一次, 之后将恢复为Status_Quo
        /// </summary>
        Continue,
        /// <summary>
        /// 动画已停止
        /// </summary>
        Status_Stoped,
    }
    /// <summary>
    /// 结束动作
    /// </summary>
    public Action? EndAction = endAction;
    /// <summary>
    /// 控制类型
    /// </summary>
    public ControlType Type = ControlType.Status_Quo;
}
