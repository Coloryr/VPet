using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using LinePutScript;

namespace VPet.Avalonia.Core.Graph;

/// <summary>
/// 食物动画 支持显示前中后3层夹心动画
/// 不一定只用于食物,只是叫这个名字
/// </summary>
public class FoodAnimation : IRunImage
{
    /// <summary>
    /// 创建食物动画 第二层夹心为运行时提供
    /// </summary>
    /// <param name="graphCore">动画核心</param>
    /// <param name="graphinfo">动画信息</param>
    /// <param name="front_Lay">前层 动画名</param>
    /// <param name="back_Lay">后层 动画名</param>
    /// <param name="animations">中间层运动轨迹</param>
    /// <param name="isLoop">是否循环</param>
    public FoodAnimation(GraphCore graphCore, GraphInfo graphinfo, string front_Lay,
        string back_Lay, ILine animations, bool isLoop = false)
    {
        IsLoop = isLoop;
        GraphInfo = graphinfo;
        GraphCore = graphCore;
        Front_Lay = front_Lay;
        Back_Lay = back_Lay;
        Animations = [];
        int i = 0;
        var sub = animations.Find("a" + i);
        while (sub != null)
        {
            Animations.Add(new Animation(this, sub));
            sub = animations.Find("a" + ++i);
        }
        IsReady = true;
    }

    public static void LoadGraph(GraphCore graph, FileSystemInfo path, ILine info)
    {
        bool isLoop = info[(gbol)"loop"];
        var pa = new FoodAnimation(graph, new GraphInfo(path, info), info[(gstr)"front_lay"]!, info[(gstr)"back_lay"]!, info, isLoop);
        graph.AddGraph(pa);
    }
    /// <summary>
    /// 前层名字
    /// </summary>
    public string Front_Lay;
    /// <summary>
    /// 后层名字
    /// </summary>
    public string Back_Lay;
    /// <summary>
    /// 所有动画帧
    /// </summary>
    public List<Animation> Animations;

    /// <summary>
    /// 是否循环播放
    /// </summary>
    public bool IsLoop { get; set; }

    /// <summary>
    /// 动画信息
    /// </summary>
    public GraphInfo GraphInfo { get; private set; }
    /// <summary>
    /// 是否准备完成
    /// </summary>
    public bool IsReady { get; set; } = false;
    public bool IsFail => false;
    public string FailMessage => "";

    public TaskControl Control { get; set; }

    public int Nowid { get; set; }

    /// <summary>
    /// 图片资源
    /// </summary>
    public string Path;

    private readonly GraphCore GraphCore;

    public void Run(Decorator parant, Action? endAction = null) => Run(parant, null, endAction);

    public void Run(Decorator parant, Bitmap? image, Action? endAction = null)
    {
        if (Control?.PlayState == true)
        {//如果当前正在运行,重置状态
            Control.Stop(() => Run(parant, endAction));
            return;
        }
        Nowid = 0;
        var NEWControl = new TaskControl(endAction);
        Control = NEWControl;
        Dispatcher.UIThread.Invoke(() =>
        {
            parant.Tag = this;
            if (parant.Child != Animation.FoodGrid)
            {
                if (Animation.FoodGrid.Parent is Decorator decorator)
                {
                    decorator.Child = null;
                }
                parant.Child = Animation.FoodGrid;
            }
            var FL = GraphCore.FindGraph(Front_Lay, GraphInfo.Animat, GraphInfo.ModeType);
            var BL = GraphCore.FindGraph(Back_Lay, GraphInfo.Animat, GraphInfo.ModeType);
            var t1 = FL?.Run(Animation.FoodGrid.Front);
            var t2 = BL?.Run(Animation.FoodGrid.Back);
            if (Animation.FoodGrid.Food.Source != image)
            {
                Animation.FoodGrid.Food.Source = image;
            }
            t1?.Start();
            t2?.Start();
            Task.Run(() => Animations[0].Run(Animation.FoodGrid.Food, NEWControl));
        });
    }
}
