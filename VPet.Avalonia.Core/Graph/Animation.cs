using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

using LinePutScript;

namespace VPet.Avalonia.Core.Graph;

/// <summary>
/// 单帧动画
/// </summary>
public class Animation
{
    public static FoodAnimatGrid FoodGrid { get; private set; } = new();

    private readonly FoodAnimation parent;

    public Thickness MarginWI;
    public double Rotate = 0;
    public double Opacity = 1;
    public bool IsVisiable = true;
    public double Width;
    /// <summary>
    /// 帧时间
    /// </summary>
    public int Time;
    /// <summary>
    /// 创建单帧动画
    /// </summary>
    /// <param name="parent">FoodAnimation</param>
    /// <param name="time">持续时间</param>
    /// <param name="wx"></param>
    public Animation(FoodAnimation parent, int time, Thickness wx, double width, double rotate = 0, bool isVisiable = true, double opacity = 1)
    {
        this.parent = parent;
        Time = time;
        MarginWI = wx;
        Rotate = rotate;
        IsVisiable = isVisiable;
        Width = width;
        Opacity = opacity;
    }
    /// <summary>
    /// 创建单帧动画
    /// </summary>
    public Animation(FoodAnimation parent, ISub sub)
    {
        this.parent = parent;
        var strs = sub.GetInfos();
        Time = int.Parse(strs[0]);//0: Time
        if (strs.Length == 1)
            IsVisiable = false;
        else
        {
            //1,2: Margin X,Y
            Width = double.Parse(strs[3]);//3:Width
            MarginWI = new Thickness(double.Parse(strs[1]), double.Parse(strs[2]), 0, 0);
            if (strs.Length > 4)
            {
                Rotate = double.Parse(strs[4]);//Rotate
                if (strs.Length > 5)
                    Opacity = double.Parse(strs[5]);//Opacity
            }
        }
    }
    /// <summary>
    /// 运行该图层
    /// </summary>
    public void Run(Control This, TaskControl Control)
    {
        //先显示该图层
        Dispatcher.UIThread.Invoke(() =>
        {
            if (IsVisiable)
            {
                This.IsVisible = true;
                This.Margin = MarginWI;
                This.RenderTransform = new RotateTransform(Rotate);
                This.Opacity = Opacity;
                This.Width = Width;
                This.Height = Width;
            }
            else
            {
                This.IsVisible = false;
            }

        });
        //然后等待帧时间毫秒
        Thread.Sleep(Time);
        //判断是否要下一步
        switch (Control.Type)
        {
            case TaskControl.ControlType.Stop:
                Control.EndAction?.Invoke();
                return;
            case TaskControl.ControlType.Status_Stoped:
                return;
            case TaskControl.ControlType.Status_Quo:
            case TaskControl.ControlType.Continue:
                if (++parent.Nowid >= parent.Animations.Count)
                    if (parent.IsLoop)
                    {
                        parent.Nowid = 0;
                        //让循环动画重新开始立线程,不stackoverflow
                        Task.Run(() => parent.Animations[0].Run(This, Control));
                        return;
                    }
                    else if (Control.Type == TaskControl.ControlType.Continue)
                    {
                        Control.Type = TaskControl.ControlType.Status_Quo;
                        parent.Nowid = 0;
                    }
                    else
                    {
                        //parent.endwilldo = () => parent.Dispatcher.Invoke(Hidden);
                        //parent.Dispatcher.Invoke(Hidden);
                        Control.Type = TaskControl.ControlType.Status_Stoped;
                        //等待其他两个动画完成
                        Control.EndAction?.Invoke(); //运行结束动画时事件
                                                     ////延时隐藏
                                                     //Task.Run(() =>
                                                     //{
                                                     //    Thread.Sleep(25);
                                                     //    parent.Dispatcher.Invoke(Hidden);
                                                     //});
                        return;
                    }
                //要下一步,现在就隐藏图层
                //隐藏该图层
                //parent.Dispatcher.Invoke(Hidden);
                parent.Animations[parent.Nowid].Run(This, Control);
                return;
        }
    }
}