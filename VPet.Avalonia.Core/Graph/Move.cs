using System;
using System.Collections.Generic;
using Avalonia;
using LinePutScript.Converter;
using VPet.Avalonia.Core.Display;
using VPet.Avalonia.Core.Handle;
using VPet.Core;

namespace VPet.Avalonia.Core.Graph;

/// <summary>
/// 移动
/// </summary>
public class Move
{
    /// <summary>
    /// 使用动画名称
    /// </summary>
    [Line(ignoreCase: true, converter: typeof(Function.LPSConvertToLower))]
    public string Graph { get; set; }
    /// <summary>
    /// 定位类型
    /// </summary>
    [Flags]
    public enum DirectionType
    {
        None,
        Left,
        Right = 2,
        Top = 4,
        Bottom = 8,
        LeftGreater = 16,
        RightGreater = 32,
        TopGreater = 64,
        BottomGreater = 128,
    }
    /// <summary>
    /// 定位类型: 需要固定到屏幕边缘启用这个
    /// </summary>
    [Line(ignoreCase: true)]
    public DirectionType LocateType { get; set; } = DirectionType.None;
    /// <summary>
    /// 移动间隔
    /// </summary>
    [Line(ignoreCase: true)]
    public int Interval { get; set; } = 125;

    private int _checkType;

    /// <summary>
    /// 检查类型
    /// </summary>
    [Line(ignoreCase: true)]
    public DirectionType CheckType
    {
        get => (DirectionType)_checkType;
        set => _checkType = (int)value;
    }
   
    private int _modeType = 30;

    /// <summary>
    /// 支持的动画模式
    /// </summary>
    [Line(ignoreCase: true, name: "modeType")]
    public ModeType Mode
    {
        get => (ModeType)_modeType;
        set => _modeType = (int)value;
    }

    /// <summary>
    /// 宠物状态模式 (Flag版)
    /// </summary>
    [Flags]
    public enum ModeType
    {
        /// <summary>
        /// 高兴
        /// </summary>
        Happy = 2,
        /// <summary>
        /// 普通
        /// </summary>
        Nomal = 4,
        /// <summary>
        /// 状态不佳
        /// </summary>
        PoorCondition = 8,
        /// <summary>
        /// 生病(躺床)
        /// </summary>
        Ill = 16,
    }

    public static ModeType GetModeType(IGameSave.ModeType type)
    {
        return type switch
        {
            IGameSave.ModeType.Happy => ModeType.Happy,
            IGameSave.ModeType.Nomal => ModeType.Nomal,
            IGameSave.ModeType.PoorCondition => ModeType.PoorCondition,
            IGameSave.ModeType.Ill => ModeType.Ill,
            _ => ModeType.Nomal,
        };
    }
    /// <summary>
    /// 检查距离左边
    /// </summary>
    [Line(ignoreCase: true)] public int CheckLeft { get; set; } = 100;
    /// <summary>
    /// 检查距离右边
    /// </summary>
    [Line(ignoreCase: true)] public int CheckRight { get; set; } = 100;
    /// <summary>
    /// 检查距离上面
    /// </summary>
    [Line(ignoreCase: true)] public int CheckTop { get; set; } = 100;
    /// <summary>
    /// 检查距离下面
    /// </summary>
    [Line(ignoreCase: true)] public int CheckBottom { get; set; } = 100;
    /// <summary>
    /// 移动速度(X轴)
    /// </summary>
    [Line(ignoreCase: true)] public int SpeedX { get; set; }
    /// <summary>
    /// 移动速度(Y轴)
    /// </summary>
    [Line(ignoreCase: true)] public int SpeedY { get; set; }
    /// <summary>
    /// 定位位置
    /// </summary>
    [Line(ignoreCase: true)]
    public int LocateLength { get; set; }
    /// <summary>
    /// 移动距离
    /// </summary>
    [Line(ignoreCase: true)] public int Distance { get; set; } = 5;

    private int _triggerType;

    /// <summary>
    /// 触发检查类型
    /// </summary>
    [Line(ignoreCase: true)]
    public DirectionType TriggerType
    {
        get => (DirectionType)_triggerType;
        set => _triggerType = (int)value;
    }
    /// <summary>
    /// 检查距离左边
    /// </summary>
    [Line(ignoreCase: true)] public int TriggerLeft { get; set; } = 100;
    /// <summary>
    /// 检查距离右边
    /// </summary>
    [Line(ignoreCase: true)] public int TriggerRight { get; set; } = 100;
    /// <summary>
    /// 检查距离上面
    /// </summary>
    [Line(ignoreCase: true)] public int TriggerTop { get; set; } = 100;
    /// <summary>
    /// 检查距离下面
    /// </summary>
    [Line(ignoreCase: true)] public int TriggerBottom { get; set; } = 100;
    /// <summary>
    /// 是否可以触发
    /// </summary>
    public bool Triggered(MainModel m)
    {
        var c = m.Core.Controller;
        if (!Mode.HasFlag(GetModeType(m.Core.Save.Mode))) return false;
        if (TriggerType == DirectionType.None) return true;
        if (TriggerType.HasFlag(DirectionType.Left) && c.GetWindowsDistanceLeft() > TriggerLeft * c.ZoomRatio)
            return false;
        if (TriggerType.HasFlag(DirectionType.Right) && c.GetWindowsDistanceRight() > TriggerRight * c.ZoomRatio)
            return false;
        if (TriggerType.HasFlag(DirectionType.Top) && c.GetWindowsDistanceUp() > TriggerTop * c.ZoomRatio)
            return false;
        if (TriggerType.HasFlag(DirectionType.Bottom) && c.GetWindowsDistanceDown() > TriggerBottom * c.ZoomRatio)
            return false;
        if (TriggerType.HasFlag(DirectionType.LeftGreater) && c.GetWindowsDistanceLeft() < TriggerLeft * c.ZoomRatio)
            return false;
        if (TriggerType.HasFlag(DirectionType.RightGreater) && c.GetWindowsDistanceRight() < TriggerRight * c.ZoomRatio)
            return false;
        if (TriggerType.HasFlag(DirectionType.TopGreater) && c.GetWindowsDistanceUp() < TriggerTop * c.ZoomRatio)
            return false;
        if (TriggerType.HasFlag(DirectionType.BottomGreater) && c.GetWindowsDistanceDown() < TriggerBottom * c.ZoomRatio)
            return false;
        return true;
    }

    /// <summary>
    /// 是否可以继续动
    /// </summary>
    public bool Checked(IController c)
    {
        if (CheckType == DirectionType.None) return true;
        if (CheckType.HasFlag(DirectionType.Left) && c.GetWindowsDistanceLeft() > CheckLeft * c.ZoomRatio)
            return false;
        if (CheckType.HasFlag(DirectionType.Right) && c.GetWindowsDistanceRight() > CheckRight * c.ZoomRatio)
            return false;
        if (CheckType.HasFlag(DirectionType.Top) && c.GetWindowsDistanceUp() > CheckTop * c.ZoomRatio)
            return false;
        if (CheckType.HasFlag(DirectionType.Bottom) && c.GetWindowsDistanceDown() > CheckBottom * c.ZoomRatio)
            return false;
        if (CheckType.HasFlag(DirectionType.LeftGreater) && c.GetWindowsDistanceLeft() < CheckLeft * c.ZoomRatio)
            return false;
        if (CheckType.HasFlag(DirectionType.RightGreater) && c.GetWindowsDistanceRight() < CheckRight * c.ZoomRatio)
            return false;
        if (CheckType.HasFlag(DirectionType.TopGreater) && c.GetWindowsDistanceUp() < CheckTop * c.ZoomRatio)
            return false;
        if (CheckType.HasFlag(DirectionType.BottomGreater) && c.GetWindowsDistanceDown() < CheckBottom * c.ZoomRatio)
            return false;
        return true;
    }

    int walklength = 0;
    /// <summary>
    /// 获取兼容支持下个播放的移动
    /// </summary>
    public Move? GetCompatibilityMove(MainModel main)
    {
        var ms = new List<Move>();
        bool x = SpeedX > 0;
        bool y = SpeedY > 0;
        foreach (Move m in main.Core.Graph.GraphConfig.Moves)
        {
            //if (m == this) continue;
            int bns = 0;
            if (SpeedX != 0 && m.SpeedX != 0)
            {
                if (m.SpeedX > 0 != x)
                    bns--;
                else
                    bns++;
            }
            if (SpeedY != 0 && m.SpeedY != 0)
            {
                if (m.SpeedY > 0 != y)
                    bns--;
                else
                    bns++;
            }
            if (bns >= 0 && m.Triggered(main))
            {
                ms.Add(m);
            }
        }
        if (ms.Count == 0) return null;
        return ms[Function.Rnd.Next(ms.Count)];
    }

    /// <summary>
    /// 显示开始移动 (假设已经检查过了)
    /// </summary>
    public void Display(MainModel m)
    {
        walklength = 0;
        m.CountNomal = 0;
        m.Display(Graph, AnimatType.A_Start, () =>
        {
            if (m.MoveTimerSmartMove)
            {
                switch (LocateType)
                {
                    case DirectionType.Top:
                        m.Core.Controller.MoveWindows(0, -m.Core.Controller.GetWindowsDistanceUp() / m.Core.Controller.ZoomRatio - LocateLength);
                        break;
                    case DirectionType.Bottom:
                        m.Core.Controller.MoveWindows(0, m.Core.Controller.GetWindowsDistanceDown() / m.Core.Controller.ZoomRatio + LocateLength);
                        break;
                    case DirectionType.Left:
                        m.Core.Controller.MoveWindows(-m.Core.Controller.GetWindowsDistanceLeft() / m.Core.Controller.ZoomRatio - LocateLength, 0);
                        break;
                    case DirectionType.Right:
                        m.Core.Controller.MoveWindows(m.Core.Controller.GetWindowsDistanceRight() / m.Core.Controller.ZoomRatio + LocateLength, 0);
                        break;
                }
                m.MoveTimerPoint = new Point(SpeedX, SpeedY);
                m.MoveTimer.Interval = Interval;
                m.MoveTimer.Start();
            }
            Displaying(m);
        });
    }
    /// <summary>
    /// 显示正在移动
    /// </summary>
    /// <param name="m"></param>
    public void Displaying(MainModel m)
    {
        //看看距离是不是不足
        if (!Checked(m.Core.Controller))
        {
            //是,停下恢复默认 or/爬墙
            if (Function.Rnd.Next(MainModel.TreeRND) <= 1)
            {
                var newmove = GetCompatibilityMove(m);
                if (newmove != null)
                {
                    newmove.Display(m);
                    return;
                }
            }
            StopMoving(m);
            return;
        }
        //不是:继续右边走or停下
        if (Function.Rnd.Next(walklength++) < Distance)
        {
            m.Display(Graph, AnimatType.B_Loop, () => Displaying(m));
            return;
        }
        else if (Function.Rnd.Next(MainModel.TreeRND) <= 1)
        {
            //停下来
            var newmove = GetCompatibilityMove(m);
            if (newmove != null)
            {
                newmove.Display(m);
                return;
            }
        }
        StopMoving(m);
    }

    private void StopMoving(MainModel m)
    {
        if (m.Core.Controller.RePostionActive)
            m.Core.Controller.ResetPosition();
        m.Core.Controller.RePostionActive = !m.Core.Controller.CheckPosition();
        m.MoveTimer.Enabled = false;
        m.Display(Graph, AnimatType.C_End, m.DisplayToNomal);
    }
}
