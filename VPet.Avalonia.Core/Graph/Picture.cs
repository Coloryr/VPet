using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using LinePutScript;

namespace VPet.Avalonia.Core.Graph;

/// <summary>
/// Picture.xaml 的交互逻辑
/// </summary>
public class Picture : IImageRun
{
    public static void LoadGraph(GraphCore graph, FileSystemInfo path, ILine info)
    {
        if (path is not FileInfo)
        {
            PNGAnimation.LoadGraph(graph, path, info);
            return;
        }
        if (path.Extension != ".png")
            return;
        int length = info.GetInt("length");
        if (length == 0)
        {
            if (!int.TryParse(path.Name.Split('.').Reverse().ToArray()[1].Split('_').Last(), out length))
                length = 1000;
        }
        bool isLoop = info[(gbol)"loop"];
        var pa = new Picture(graph, path.FullName, new GraphInfo(path, info), length, isLoop);
        graph.AddGraph(pa);
    }

    /// <summary>
    /// 图片资源
    /// </summary>
    public string Path;

    public bool IsLoop { get; set; }
    /// <summary>
    /// 播放持续时间 毫秒
    /// </summary>
    public int Length { get; set; }

    //public bool StoreMemory => true;//经过测试,储存到内存好处多多,不储存也要占用很多内存,干脆存了吧

    /// <summary>
    /// 动画信息
    /// </summary>
    public GraphInfo GraphInfo { get; private set; }

    public bool IsReady { get; set; } = false;

    public TaskControl Control { get; set; }

    public bool IsFail => false;

    public string FailMessage => "";

    private readonly GraphCore _graphCore;

    /// <summary>
    /// 新建新静态图像
    /// </summary>
    /// <param name="path">图片路径</param>
    public Picture(GraphCore graphCore, string path, GraphInfo graphinfo, int length = 1000, bool isloop = false)
    {
        GraphInfo = graphinfo;
        IsLoop = isloop;
        Length = length;
        _graphCore = graphCore;
        Path = path;
        if (!_graphCore.CommConfig.ContainsKey("PIC_Setup"))
        {
            _graphCore.CommConfig["PIC_Setup"] = true;
            _graphCore.CommUIElements["Image1.Picture"] = new Image() { Width = 500, Height = 500 };
            _graphCore.CommUIElements["Image2.Picture"] = new Image() { Width = 500, Height = 500 };
            _graphCore.CommUIElements["Image3.Picture"] = new Image() { Width = 500, Height = 500 };
        }
        IsReady = true;
    }

    public void Run(Decorator parant, Action? endAction = null)
    {
        if (Control?.PlayState == true)
        {//如果当前正在运行,重置状态
            Control.SetContinue();
            Control.EndAction = endAction;
            return;
        }
        var NEWControl = new TaskControl(endAction);
        Control = NEWControl;

        Dispatcher.UIThread.Invoke(() =>
        {
            if (parant.Tag != this)
            {
                Image img;
                if (parant.Child == _graphCore.CommUIElements["Image1.Picture"])
                {
                    img = (Image)_graphCore.CommUIElements["Image1.Picture"];
                }
                else if (parant.Child == _graphCore.CommUIElements["Image3.Picture"])
                {
                    img = (Image)_graphCore.CommUIElements["Image3.Picture"];
                }
                else
                {
                    img = (Image)_graphCore.CommUIElements["Image2.Picture"];
                    if (parant.Child != img)
                    {
                        if (img.Parent == null)
                        {
                            parant.Child = img;
                        }
                        else
                        {
                            img = (Image)_graphCore.CommUIElements["Image1.Picture"];
                            if (img.Parent != null)
                                ((Decorator)img.Parent).Child = null;
                            parant.Child = img;
                        }
                    }
                }
                img.Width = 500;
                img.Source = new Bitmap(Path);
                parant.Tag = this;
            }
            Task.Run(() => Run(NEWControl));
        });
    }

    /// <summary>
    /// 通过控制器运行
    /// </summary>
    /// <param name="Control"></param>
    public void Run(TaskControl Control)
    {
        Thread.Sleep(Length);
        //判断是否要下一步
        switch (Control.Type)
        {
            case TaskControl.ControlType.Stop:
                Control.EndAction?.Invoke();
                return;
            case TaskControl.ControlType.Status_Stoped:
                return;
            case TaskControl.ControlType.Continue:
                Control.Type = TaskControl.ControlType.Status_Quo;
                Run(Control);
                return;
            case TaskControl.ControlType.Status_Quo:
                if (IsLoop)
                {
                    Task.Run(() => Run(Control));
                }
                else
                {
                    Control.Type = TaskControl.ControlType.Status_Stoped;
                    Control.EndAction?.Invoke(); //运行结束动画时事件
                }
                return;
        }
    }

    public Task Run(Image img, Action? endAction = null)
    {
        if (Control?.PlayState == true)
        {
            //如果当前正在运行,重置状态
            Control.EndAction = null;
            Control.Type = TaskControl.ControlType.Stop;
        }
        Control = new TaskControl(endAction);
        return Dispatcher.UIThread.Invoke(() =>
        {
            if (img.Tag == this)
            {
                return new Task(() => Run(Control));
            }
            img.Tag = this;
            img.Source = new Bitmap(Path);
            img.Width = 500;
            return new Task(() => Run(Control));
        });
    }
}

