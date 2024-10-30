using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VPet.Avalonia.Core.Graph;

namespace VPet.Avalonia.Core.Display;

public partial class MainModel : ObservableObject
{

    public const int TreeRND = 5;

    /// <summary>
    /// 游戏核心
    /// </summary>
    public GameCore Core;

    [ObservableProperty]
    private bool _isWorkDisplay;

    public MainModel()
    {
        //MessageInit();
    }

    [RelayCommand]
    public void StopWork()
    {


    }

    [RelayCommand]
    public void SwitchState()
    {

    }

    /// <summary>
    /// 刷新时间时会调用该方法,在所有任务处理完之后
    /// </summary>
    public void TimeUIHandle()
    {
        WorkTimerTick();
    }
}
