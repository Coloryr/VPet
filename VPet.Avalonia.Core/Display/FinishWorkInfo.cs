using System;
using VPet.Avalonia.Core.Graph;

namespace VPet.Avalonia.Core.Display;

/// <summary>
/// 完成工作信息
/// </summary>
public class FinishWorkInfo
{
    /// <summary>
    /// 当前完成工作
    /// </summary>
    public Work Work;
    /// <summary>
    /// 当前完成工作收入
    /// </summary>
    public double Count;
    /// <summary>
    /// 当前完成工作花费时间
    /// </summary>
    public double Spendtime;
    /// <summary>
    /// 停止工作的原因
    /// </summary>
    public enum StopReason
    {
        /// <summary>
        /// 时间结束完成
        /// </summary>
        TimeFinish,
        /// <summary>
        /// 玩家手动停止
        /// </summary>
        MenualStop,
        /// <summary>
        /// 因为状态等停止
        /// </summary>
        StateFail,
        /// <summary>
        /// 其他原因
        /// </summary>
        Other,
    }
    /// <summary>
    /// 停止原因
    /// </summary>
    public StopReason Reason;
    /// <summary>
    /// 完成工作信息
    /// </summary>
    /// <param name="work">当前工作</param>
    /// <param name="count">当前盈利(自动计算附加)</param>
    public FinishWorkInfo(Work work, double count, StopReason reason)
    {
        Work = work;
        Count = count * (1 + work.FinishBonus);
        Spendtime = work.Time;
        Reason = reason;
    }
    /// <summary>
    /// 完成工作信息
    /// </summary>
    /// <param name="work">当前工作</param>
    /// <param name="count">当前盈利(自动计算附加)</param>
    public FinishWorkInfo(Work work, double count, DateTime starttime, StopReason reason)
    {
        Work = work;
        Count = count * (1 + work.FinishBonus);
        Spendtime = DateTime.Now.Subtract(starttime).TotalMinutes;
        Reason = reason;
    }
}