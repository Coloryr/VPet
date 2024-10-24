using System;
using System.Timers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.Layout;
using Timer = System.Timers.Timer;

namespace VPet.Avalonia.Core.Display;

/// <summary>
/// ToolBar.xaml 的交互逻辑
/// </summary>
public partial class ToolBar : UserControl, IDisposable
{
    Main m;
    public Timer CloseTimer;
    bool onFocus = false;
    Timer closePanelTimer;

    public ToolBar(Main m)
    {
        InitializeComponent();

        PointerEntered += ToolBar_PointerEntered;
        PointerExited += ToolBar_PointerExited;

        BdrPanel.PointerExited += BdrPanel_PointerExited;
        Button1.Click += Button1_Click;

        MenuPanel.PointerEntered += MenuPanel_PointerEntered;
        MenuPanel.PointerExited += MenuPanel_PointerExited;

        Sleep.Click += Sleep_Click;

        this.m = m;
        CloseTimer = new Timer()
        {
            Interval = 4000,
            AutoReset = false,
            Enabled = false
        };
        CloseTimer.Elapsed += Closetimer_Elapsed;
        closePanelTimer = new Timer();
        closePanelTimer.Elapsed += ClosePanelTimer_Tick;
        // m.TimeUIHandle += M_TimeUIHandle;
    }

    private void Sleep_Click(object? sender, RoutedEventArgs e)
    {
        //if (m.State == Main.WorkingState.Sleep)
        //{
        //    if (m.Core.Save.Mode == IGameSave.ModeType.Ill)
        //        return;
        //    m.State = WorkingState.Nomal;
        //    m.Display(GraphType.Sleep, AnimatType.C_End, m.DisplayNomal);
        //}
        //else if (m.State == Main.WorkingState.Nomal)
        //    m.DisplaySleep(true);
        //else
        //{
        //    m.WorkTimer.Stop(() => m.DisplaySleep(true), WorkTimer.FinishWorkInfo.StopReason.MenualStop);
        //}
    }

    private void MenuPanel_PointerExited(object? sender, PointerEventArgs e)
    {
        closePanelTimer.Start();
    }

    private void MenuPanel_PointerEntered(object? sender, PointerEventArgs e)
    {
        BdrPanel.IsVisible = true;
        M_TimeUIHandle(m);
        EventMenuPanelShow?.Invoke();
    }

    private void Button1_Click(object? sender, RoutedEventArgs e)
    {
        // m.Core.Controller.ShowPanel();
    }

    private void BdrPanel_PointerExited(object? sender, PointerEventArgs e)
    {
        closePanelTimer.Start();
    }

    private void ToolBar_PointerExited(object? sender, PointerEventArgs e)
    {
        CloseTimer.Start();
    }

    private void ToolBar_PointerEntered(object? sender, PointerEventArgs e)
    {
        CloseTimer.Enabled = false;
    }

    public void LoadClean()
    {
        MenuWork.Click -= MenuWork_Click;
        MenuWork.IsVisible = true;
        MenuStudy.Click -= MenuStudy_Click;
        MenuStudy.IsVisible = true;
        MenuPlay.Click -= MenuPlay_Click;


        MenuWork.Items.Clear();
        MenuStudy.Items.Clear();
        MenuPlay.Items.Clear();
    }

    public void StartWork(Work w)
    {
        if (m.StartWork(w))
            Visibility = Visibility.Collapsed;
    }

    /// <summary>
    /// 加载默认工作
    /// </summary>
    public void LoadWork()
    {
        LoadClean();

        m.WorkList(out List<Work> ws, out List<Work> ss, out List<Work> ps);

        if (ws.Count == 0)
        {
            MenuWork.Visibility = Visibility.Collapsed;
        }
        else if (ws.Count == 1)
        {
            MenuWork.Click += MenuWork_Click;
            wwork = ws[0];
            MenuWork.Header = ws[0].NameTrans;
        }
        else
        {
            foreach (var w in ws)
            {
                var mi = new MenuItem()
                {
                    Header = w.NameTrans
                };
                mi.Click += (s, e) => StartWork(w);

                MenuWork.Items.Add(mi);
            }
        }
        if (ss.Count == 0)
        {
            MenuStudy.Visibility = Visibility.Collapsed;
        }
        else if (ss.Count == 1)
        {
            MenuStudy.Click += MenuStudy_Click;
            wstudy = ss[0];
            MenuStudy.Header = ss[0].NameTrans;
        }
        else
        {
            foreach (var w in ss)
            {
                var mi = new MenuItem()
                {
                    Header = w.NameTrans
                };
                mi.Click += (s, e) => StartWork(w);
                MenuStudy.Items.Add(mi);
            }
        }
        if (ps.Count == 0)
        {
            MenuPlay.Visibility = Visibility.Collapsed;
        }
        else if (ps.Count == 1)
        {
            MenuPlay.Click += MenuPlay_Click;
            wplay = ps[0];
            MenuPlay.Header = ps[0].NameTrans;
        }
        else
        {
            foreach (var w in ps)
            {
                var mi = new MenuItem()
                {
                    Header = w.NameTrans
                };
                mi.Click += (s, e) => StartWork(w);
                MenuPlay.Items.Add(mi);
            }
        }
    }

    private void MenuStudy_Click(object? sender, RoutedEventArgs e)
    {
        StartWork(wstudy);
    }

    Work wwork;
    Work wstudy;
    Work wplay;


    private void MenuWork_Click(object? sender, RoutedEventArgs e)
    {
        StartWork(wwork);
    }
    private void MenuPlay_Click(object? sender, RoutedEventArgs e)
    {
        StartWork(wplay);
    }

    /// <summary>
    /// 刷新显示UI
    /// </summary>
    public void M_TimeUIHandle(Main m)
    {
        //if (BdrPanel.IsVisible)
        //{
        //    Tlv.Text = "Lv " + m.Core.Save.Level.ToString();
        //    tExp.Text = "x" + m.Core.Save.ExpBonus.ToString("f2");
        //    tMoney.Text = "$ " + m.Core.Save.Money.ToString("N2");
        //    if (m.Core.Controller.EnableFunction)
        //    {
        //        till.Visibility = m.Core.Save.Mode == IGameSave.ModeType.Ill ? Visibility.Visible : Visibility.Collapsed;
        //        tfun.Visibility = Visibility.Collapsed;
        //    }
        //    else
        //    {
        //        till.Visibility = Visibility.Collapsed;
        //        tfun.Visibility = Visibility.Visible;
        //    }
        //    var max = m.Core.Save.LevelUpNeed();
        //    pExp.Value = 0;
        //    pExp.Maximum = max;
        //    if (m.Core.Save.Exp < 0)
        //    {
        //        pExp.Minimum = m.Core.Save.Exp;
        //    }
        //    else
        //    {
        //        pExp.Minimum = 0;
        //    }
        //    pExp.Value = m.Core.Save.Exp;


        //    pStrengthFood.Value = 0;
        //    pStrengthDrink.Value = 0;
        //    pStrength.Value = 0;
        //    pFeeling.Value = 0;

        //    pStrengthFood.Maximum = m.Core.Save.StrengthMax;
        //    pStrengthDrink.Maximum = m.Core.Save.StrengthMax;
        //    pStrength.Maximum = m.Core.Save.StrengthMax;
        //    pFeeling.Maximum = m.Core.Save.FeelingMax;



        //    pStrength.Value = m.Core.Save.Strength;
        //    pFeeling.Value = m.Core.Save.Feeling;

        //    pStrengthFood.Value = m.Core.Save.StrengthFood;
        //    pStrengthDrink.Value = m.Core.Save.StrengthDrink;
        //    pStrengthFoodMax.Value = Math.Min(100, (m.Core.Save.StrengthFood + m.Core.Save.StoreStrengthFood) / m.Core.Save.StrengthMax * 100);
        //    pStrengthDrinkMax.Value = Math.Min(100, (m.Core.Save.StrengthDrink + m.Core.Save.StoreStrengthDrink) / m.Core.Save.StrengthMax * 100);

        //    if (Math.Abs(m.Core.Save.ChangeStrength) > 1)
        //        tStrength.Text = $"{m.Core.Save.ChangeStrength:f1}/t";
        //    else
        //        tStrength.Text = $"{m.Core.Save.ChangeStrength:f2}/t";
        //    if (Math.Abs(m.Core.Save.ChangeFeeling) > 1)
        //        tFeeling.Text = $"{m.Core.Save.ChangeFeeling:f1}/t";
        //    else
        //        tFeeling.Text = $"{m.Core.Save.ChangeFeeling:f2}/t";
        //    if (Math.Abs(m.Core.Save.ChangeStrengthDrink) > 1)
        //        tStrengthDrink.Text = $"{m.Core.Save.ChangeStrengthDrink:f1}/t";
        //    else
        //        tStrengthDrink.Text = $"{m.Core.Save.ChangeStrengthDrink:f2}/t";
        //    if (Math.Abs(m.Core.Save.ChangeStrengthFood) > 1)
        //        tStrengthFood.Text = $"{m.Core.Save.ChangeStrengthFood:f1}/t";
        //    else
        //        tStrengthFood.Text = $"{m.Core.Save.ChangeStrengthFood:f2}/t";

        //}
    }

    private void ClosePanelTimer_Tick(object sender, EventArgs e)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            if (BdrPanel.IsPointerOver
                || MenuPanel.IsPointerOver)
            {
                closePanelTimer.Stop();
                return;
            }
            BdrPanel.IsVisible = false;
        });
    }

    private void Closetimer_Elapsed(object sender, ElapsedEventArgs e)
    {
        if (onFocus)
        {
            onFocus = false;
            CloseTimer.Start();
        }
        else
            Dispatcher.UIThread.Invoke(() => IsVisible = false);
    }
    /// <summary>
    /// ToolBar显示事件
    /// </summary>
    public event Action EventShow;
    public void Show()
    {
        //EventShow?.Invoke();
        //if (m.UIGrid.Children.IndexOf(this) != m.UIGrid.Children.Count - 1)
        //{
        //    Panel.SetZIndex(this, m.UIGrid.Children.Count);
        //}
        //Visibility = Visibility.Visible;
        //if (CloseTimer.Enabled)
        //    onFocus = true;
        //else
        //    CloseTimer.Start();
    }

    /// <summary>
    /// 窗口类型
    /// </summary>
    public enum MenuType
    {
        /// <summary>
        /// 投喂
        /// </summary>
        Feed,
        /// <summary>
        /// 互动
        /// </summary>
        Interact,
        /// <summary>
        /// 自定
        /// </summary>
        DIY,
        /// <summary>
        /// 设置
        /// </summary>
        Setting,
    }
    /// <summary>
    /// 添加按钮
    /// </summary>
    /// <param name="parentMenu">按钮位置</param>
    /// <param name="displayName">显示名称</param>
    /// <param name="clickCallback">功能</param>
    public void AddMenuButton(MenuType parentMenu,
        string displayName,
        Action clickCallback)
    {
        var menuItem = new MenuItem()
        {
            Header = displayName,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        menuItem.Click += delegate
        {
            clickCallback?.Invoke();
        };
        switch (parentMenu)
        {
            case MenuType.Feed:
                MenuFeed.Items.Add(menuItem);
                break;
            case MenuType.Interact:
                MenuInteract.Items.Add(menuItem);
                break;
            case MenuType.DIY:
                MenuDIY.Items.Add(menuItem);
                break;
            case MenuType.Setting:
                MenuSetting.Items.Add(menuItem);
                break;
        }
    }

    private void PgbExperience_GeneratingPercentText(object sender, GeneratingPercentTextRoutedEventArgs e)
    {
        e.Text = $"{e.Value:f2} / {pExp.Maximum:f0}";
    }

    private void PgbStrength_GeneratingPercentText(object sender, GeneratingPercentTextRoutedEventArgs e)
    {
        e.Text = $"{e.Value:f2} / {pStrength.Maximum:f0}";
    }

    private void PgbSpirit_GeneratingPercentText(object sender, GeneratingPercentTextRoutedEventArgs e)
    {
        var progressBar = (ProgressBar)sender;
        progressBar.Foreground = GetForeground(e.Value / pFeeling.Maximum);
        e.Text = $"{e.Value:f2} / {pFeeling.Maximum:f0}";
    }

    private void PgbHunger_GeneratingPercentText(object sender, GeneratingPercentTextRoutedEventArgs e)
    {
        var progressBar = (ProgressBar)sender;
        progressBar.Foreground = GetForeground(e.Value / pStrength.Maximum);
        e.Text = $"{e.Value:f2} / {pStrength.Maximum:f0}";
    }

    private void PgbThirsty_GeneratingPercentText(object sender, GeneratingPercentTextRoutedEventArgs e)
    {
        var progressBar = (ProgressBar)sender;
        progressBar.Foreground = GetForeground(e.Value / pStrength.Maximum);
        e.Text = $"{e.Value:f2} / {pStrength.Maximum:f0}";
        //if (e.Value <= 20)
        //{
        //    tHearth.Visibility = Visibility.Visible;
        //}
    }

    private Brush GetForeground(double value)
    {
        if (value >= .6)
        {
            return FindResource("SuccessProgressBarForeground") as Brush;
        }
        else if (value >= .3)
        {
            return FindResource("WarningProgressBarForeground") as Brush;
        }
        else
        {
            return FindResource("DangerProgressBarForeground") as Brush;
        }
    }
    /// <summary>
    /// MenuPanel显示事件
    /// </summary>
    public event Action EventMenuPanelShow;

    public void Dispose()
    {
        m = null;
        CloseTimer.Dispose();
        closePanelTimer.Dispose();
    }
}