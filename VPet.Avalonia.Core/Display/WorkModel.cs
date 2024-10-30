using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPet.Avalonia.Core.Graph;
using VPet.Core;

namespace VPet.Avalonia.Core.Display;

public partial class MainModel
{
    public Work NowWork;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime WorkStartTime;
    /// <summary>
    /// 累计获得的钱/经验值
    /// </summary>
    public double WorkGetCount;
    /// <summary>
    /// 显示模式
    /// 0 = 默认
    /// 1 = 剩余时间
    /// 2 = 已获取(金钱/等级)
    /// </summary>
    public int WorkDisplayType = 0;

    /// <summary>
    /// 任务完成时调用该参数
    /// </summary>
    public event Action<FinishWorkInfo> E_FinishWork;

    private void WorkTimerTick()
    {
        //if (!IsWorkDisplay) return;
        //TimeSpan ts = DateTime.Now - WorkStartTime;
        //TimeSpan tleft;
        //if (ts.TotalMinutes > NowWork.Time)
        //{
        //    //学完了,停止
        //    //ts = TimeSpan.FromMinutes(MaxTime);
        //    //tleft = TimeSpan.Zero;
        //    //PBLeft.Value = MaxTime;
        //    FinishWorkInfo fwi = new FinishWorkInfo(NowWork, WorkGetCount, FinishWorkInfo.StopReason.TimeFinish);
        //    if (NowWork.Type == WorkType.Work)
        //    {
        //        Core.Save.Money += WorkGetCount * NowWork.FinishBonus;
        //        Stop(() => SayRnd(LocalizeCore.Translate("{2}完成啦, 累计赚了 {0:f2} 金钱\n共计花费了{1}分钟", fwi.count,
        //            fwi.spendtime, fwi.work.NameTrans), true), StopReason.TimeFinish);
        //    }
        //    else
        //    {
        //        m.Core.Save.Exp += WorkGetCount * NowWork.FinishBonus;
        //        Stop(() => m.SayRnd(LocalizeCore.Translate("{2}完成啦, 累计获得 {0:f2} 经验\n共计花费了{1}分钟", fwi.count,
        //            fwi.spendtime, fwi.work.NameTrans), true), StopReason.TimeFinish);
        //    }
        //    return;
        //}
        //else
        //{
        //    tleft = TimeSpan.FromMinutes(m.NowWork.Time) - ts;
        //    PBLeft.Value = ts.TotalMinutes;
        //}
        //switch (DisplayType)
        //{
        //    default:
        //    case 0:
        //        ShowTimeSpan(ts); break;
        //    case 1:
        //        ShowTimeSpan(tleft); break;
        //    case 2:
        //        tNumber.Text = WorkGetCount.ToString("f0");
        //        if (m.NowWork.Type == Work.WorkType.Work)
        //            tNumberUnit.Text = LocalizeCore.Translate("钱");
        //        else
        //            tNumberUnit.Text = "EXP";
        //        break;
        //    case 3:
        //        break;
        //}
    }

    //public void ShowTimeSpan(TimeSpan ts)
    //{
    //    if (ts.TotalSeconds < 90)
    //    {
    //        tNumber.Text = ts.TotalSeconds.ToString("f1");
    //        tNumberUnit.Text = LocalizeCore.Translate("秒");
    //    }
    //    else if (ts.TotalMinutes < 90)
    //    {
    //        tNumber.Text = ts.TotalMinutes.ToString("f1");
    //        tNumberUnit.Text = LocalizeCore.Translate("分钟");
    //    }
    //    else
    //    {
    //        tNumber.Text = ts.TotalHours.ToString("f1");
    //        tNumberUnit.Text = LocalizeCore.Translate("小时");
    //    }
    //}
    //public void DisplayUI()
    //{
    //    if (DisplayType == 3)
    //    {
    //        btnSwitch.Opacity = 0.5;
    //        DisplayBorder.Visibility = Visibility.Collapsed;
    //    }
    //    else
    //    {
    //        btnSwitch.Opacity = 1;
    //        DisplayBorder.Visibility = Visibility.Visible;
    //        btnStop.Content = LocalizeCore.Translate("停止") + m.NowWork.NameTrans;
    //        switch (DisplayType)
    //        {
    //            default:
    //            case 0:
    //                tNow.Text = LocalizeCore.Translate("当前已{0}", m.NowWork.NameTrans);
    //                break;
    //            case 1:
    //                tNow.Text = LocalizeCore.Translate("剩余{0}时间", m.NowWork.NameTrans);
    //                break;
    //            case 2:
    //                if (m.NowWork.Type == Work.WorkType.Work)
    //                    tNow.Text = LocalizeCore.Translate("累计金钱收益");
    //                else
    //                    tNow.Text = LocalizeCore.Translate("获得经验值");
    //                break;
    //        }
    //    }
    //    M_TimeUIHandle(m);
    //}
    //private void SwitchState_Click(object sender, RoutedEventArgs e)
    //{
    //    DisplayType++;
    //    if (DisplayType >= 4)
    //        DisplayType = 0;
    //    DisplayUI();
    //}
    //public void Start(Work work)
    //{
    //    //if (state == Main.WorkingState.Nomal)
    //    //    return;
    //    Visibility = Visibility.Visible;
    //    m.State = Main.WorkingState.Work;
    //    m.NowWork = work;
    //    StartTime = DateTime.Now;
    //    WorkGetCount = 0;

    //    work.SetStyle(this);
    //    work.Display(m);

    //    PBLeft.Maximum = work.Time;
    //    DisplayUI();
    //}
    ///// <summary>
    ///// 停止工作
    ///// </summary>
    ///// <param name="then"></param>
    //public void Stop(Action @then = null, StopReason reason = StopReason.MenualStop)
    //{
    //    if (m.State == Main.WorkingState.Work && m.NowWork != null)
    //    {
    //        FinishWorkInfo fwi = new FinishWorkInfo(m.NowWork, WorkGetCount, StartTime, reason);
    //        E_FinishWork?.Invoke(fwi);
    //    }
    //    Visibility = Visibility.Collapsed;
    //    m.State = Main.WorkingState.Nomal;
    //    m.Display(m.NowWork.Graph, AnimatType.C_End, then ?? m.DisplayNomal);
    //}
    //private void btnStop_Click(object sender, RoutedEventArgs e)
    //{
    //    Stop(reason: StopReason.MenualStop);
    //}
}
