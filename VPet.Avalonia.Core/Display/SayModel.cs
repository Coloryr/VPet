using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VPet.Avalonia.Core.Graph;

namespace VPet.Avalonia.Core.Display;

public partial class MainModel
{
    /// <summary>
    /// 处理说话内容
    /// </summary>
    public event Action<string> OnSay;

    private int timeleft;
    private string graphName;

    private List<char> outputtext;

    public Timer EndTimer = new Timer() { Interval = 200 };
    public Timer ShowTimer = new Timer() { Interval = 50 };
    public Timer CloseTimer = new Timer() { Interval = 20 };

    /// <summary>
    /// 被关闭时事件
    /// </summary>
    public event Action EndAction;

    [ObservableProperty]
    private double _messageOpacity;
    [ObservableProperty]
    private bool _messageDisplay;

    [ObservableProperty]
    private string _messageText;

    public string MessageContent;

    [RelayCommand]
    public async Task MessageCopy(ContextMenu? menu)
    {
        var top = TopLevel.GetTopLevel(menu);
        if (top?.Clipboard is { } clip)
        {
            await clip.SetTextAsync(MessageText);
        }
    }

    [RelayCommand]
    public void MessageClose()
    {
        ForceClose();
    }

    private void ForceClose()
    {
        EndTimer.Stop(); ShowTimer.Stop(); CloseTimer.Close();
        MessageDisplay = false;
        if ((DisplayType.Name == graphName || DisplayType.Type == GraphType.Say) && DisplayType.Animat != AnimatType.C_End)
            DisplayCEndtoNomal(DisplayType.Name);
        EndAction?.Invoke();
    }

    public void MessageStop()
    {
        EndTimer.Stop();
        CloseTimer.Stop();
        MessageOpacity = 0.8;
    }

    public void MessageStart()
    {
        if (!ShowTimer.Enabled)
            EndTimer.Start();
    }

    private void MessageInit()
    {
        EndTimer.Elapsed += EndTimer_Elapsed;
        ShowTimer.Elapsed += ShowTimer_Elapsed;
        CloseTimer.Elapsed += CloseTimer_Elapsed;
    }

    private void CloseTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        if (Dispatcher.UIThread.Invoke(() => MessageOpacity) <= 0.05)
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                MessageOpacity = 1;
                MessageDisplay = false;
            });
            EndAction?.Invoke();
        }
        else
        {
            Dispatcher.UIThread.Invoke(() => MessageOpacity -= 0.02);
        }
    }

    private void EndTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        if (--timeleft <= 0)
        {
            if ((DisplayType.Name == graphName || DisplayType.Type == GraphType.Say) && DisplayType.Animat != AnimatType.C_End)
                DisplayCEndtoNomal(DisplayType.Name);
            EndTimer.Stop();
            CloseTimer.Start();
        }
    }

    private void ShowTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        if (outputtext.Count > 0)
        {
            var str = outputtext[0];
            outputtext.RemoveAt(0);
            Dispatcher.UIThread.Invoke(() => { MessageText += str; });
        }
        else
        {
            if (PlayingVoice)
            {
                if (windowMediaPlayerAvailable)
                {
                    TimeSpan ts = Dispatcher.Invoke(() => VoicePlayer?.Clock?.NaturalDuration.HasTimeSpan == true ? (VoicePlayer.Clock.NaturalDuration.TimeSpan - VoicePlayer.Clock.CurrentTime.Value) : TimeSpan.Zero);
                    if (ts.TotalSeconds > 2)
                    {
                        return;
                    }
                    else
                    {
                        Console.WriteLine(1);
                    }
                }
                else
                {
                    if (soundPlayer.IsLoadCompleted)
                    {
                        PlayingVoice = false;
                        soundPlayer.PlaySync();
                    }
                }
            }
            ShowTimer.Stop();
            EndTimer.Start();
        }
    }

    /// <summary>
    /// 说话,使用随机表情
    /// </summary>
    public void SayRnd(string text, bool force = false, string desc = null)
    {
        Say(text, Core.Graph.FindName(GraphType.Say), force, desc);
    }

    /// <summary>
    /// 说话
    /// </summary>
    /// <param name="text">说话内容</param>
    /// <param name="graphname">图像名</param>
    /// <param name="desc">描述</param>
    /// <param name="force">强制显示图像</param>
    public void Say(string text, string graphname = null, bool force = false, string desc = null)
    {
        Task.Run(() =>
        {
            OnSay?.Invoke(text);
            if (force || !string.IsNullOrWhiteSpace(graphname) && DisplayType.Type == GraphType.Default)//这里不使用idle是因为idle包括学习等
                Display(graphname, AnimatType.A_Start, () =>
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        MsgBar.Show(Core.Save.Name, text, graphname, (string.IsNullOrWhiteSpace(desc) ? null :
                            new TextBlock() { Text = desc, FontSize = 20, ToolTip = desc, HorizontalAlignment = HorizontalAlignment.Right }));
                    });
                    DisplayBLoopingForce(graphname);
                });
            else
            {
                Dispatcher.Invoke(() =>
                {
                    MsgBar.Show(Core.Save.Name, text, graphname, msgcontent: (string.IsNullOrWhiteSpace(desc) ? null :
                        new TextBlock() { Text = desc, FontSize = 20, ToolTip = desc, HorizontalAlignment = HorizontalAlignment.Right }));
                });
            }
        });
    }
    /// <summary>
    /// 说话
    /// </summary>
    /// <param name="text">说话内容</param>
    /// <param name="graphname">图像名</param>
    /// <param name="msgcontent">消息内容</param>
    /// <param name="force">强制显示图像</param>
    public void Say(string text, Control msgcontent, string graphname = null, bool force = false)
    {
        Task.Run(() =>
        {
            OnSay?.Invoke(text);
            if (force || !string.IsNullOrWhiteSpace(graphname) && DisplayType.Type == GraphType.Default)//这里不使用idle是因为idle包括学习等
                Display(graphname, AnimatType.A_Start, () =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        MsgBar.Show(Core.Save.Name, text, graphname, msgcontent);
                    });
                    DisplayBLoopingForce(graphname);
                });
            else
            {
                Dispatcher.Invoke(() =>
                {
                    MsgBar.Show(Core.Save.Name, text, msgcontent: msgcontent);
                });
            }
        });
    }
}
