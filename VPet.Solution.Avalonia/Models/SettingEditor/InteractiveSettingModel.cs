﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using VPet_Simulator.Core;

namespace VPet.Solution.Models.SettingEditor;

public class InteractiveSettingModel : ObservableClass<InteractiveSettingModel>
{
    // NOTE: 这玩意其实在存档里 而不是设置里
    //#region PetName
    //private string _petName;

    ///// <summary>
    ///// 宠物名称
    ///// </summary>
    //[ReflectionProperty(nameof(VPet_Simulator.Windows.Setting.PetName))]
    //public string PetName
    //{
    //    get => _petName;
    //    set => SetProperty(ref _petName, value);
    //}
    //#endregion

    #region VoiceVolume
    private double _voiceVolume;

    /// <summary>
    /// 播放声音大小
    /// </summary>
    [ReflectionProperty(nameof(Setting.VoiceVolume))]
    public double VoiceVolume
    {
        get => _voiceVolume;
        set => SetProperty(ref _voiceVolume, value);
    }
    #endregion

    #region EnableFunction
    private bool _enableFunction;

    /// <summary>
    /// 启用计算等数据功能
    /// </summary>
    [ReflectionProperty(nameof(Setting.EnableFunction))]
    public bool EnableFunction
    {
        get => _enableFunction;
        set => SetProperty(ref _enableFunction, value);
    }
    #endregion

    #region CalFunState
    private IGameSave.ModeType _calFunState;

    /// <summary>
    /// 非计算模式下默认模式
    /// </summary>
    [ReflectionProperty(nameof(Setting.CalFunState))]
    public IGameSave.ModeType CalFunState
    {
        get => _calFunState;
        set => SetProperty(ref _calFunState, value);
    }

    public ObservableCollection<IGameSave.ModeType> ModeTypes { get; } =
        new(Enum.GetValues(typeof(IGameSave.ModeType)).Cast<IGameSave.ModeType>());
    #endregion

    #region LastCacheDate
    private DateTime _lastCacheDate;

    /// <summary>
    /// 上次清理缓存日期
    /// </summary>
    [ReflectionProperty(nameof(Setting.LastCacheDate))]
    public DateTime LastCacheDate
    {
        get => _lastCacheDate;
        set => SetProperty(ref _lastCacheDate, value);
    }
    #endregion

    #region SaveTimes
    private int _saveTimes;

    /// <summary>
    /// 储存顺序次数
    /// </summary>
    [ReflectionProperty(nameof(Setting.SaveTimes))]
    public int SaveTimes
    {
        get => _saveTimes;
        set => SetProperty(ref _saveTimes, value);
    }
    #endregion

    #region PressLength
    private int _pressLength;

    /// <summary>
    /// 按多久视为长按 单位毫秒
    /// </summary>
    [ReflectionProperty(nameof(Setting.PressLength))]
    public int PressLength
    {
        get => _pressLength;
        set => SetProperty(ref _pressLength, value);
    }
    #endregion

    #region InteractionCycle
    private int _interactionCycle;

    /// <summary>
    /// 互动周期
    /// </summary>
    [ReflectionProperty(nameof(Setting.InteractionCycle))]
    public int InteractionCycle
    {
        get => _interactionCycle;
        set => SetProperty(ref _interactionCycle, value);
    }
    #endregion

    #region LogicInterval
    private double _logicInterval;

    /// <summary>
    /// 计算间隔 (秒)
    /// </summary>
    [ReflectionProperty(nameof(Setting.LogicInterval))]
    public double LogicInterval
    {
        get => _logicInterval;
        set => SetProperty(ref _logicInterval, value);
    }
    #endregion

    #region AllowMove
    private bool _allowMove;

    /// <summary>
    /// 允许移动事件
    /// </summary>
    [ReflectionProperty(nameof(Setting.AllowMove))]
    public bool AllowMove
    {
        get => _allowMove;
        set => SetProperty(ref _allowMove, value);
    }
    #endregion

    #region SmartMove
    private bool _smartMove;

    /// <summary>
    /// 智能移动
    /// </summary>
    [ReflectionProperty(nameof(Setting.SmartMove))]
    public bool SmartMove
    {
        get => _smartMove;
        set => SetProperty(ref _smartMove, value);
    }
    #endregion

    #region SmartMoveInterval
    private int _smartMoveInterval = 0;

    /// <summary>
    /// 智能移动周期 (秒)
    /// </summary>
    [DefaultValue(1)]
    [ReflectionProperty(nameof(Setting.SmartMoveInterval))]
    [ReflectionPropertyConverter(typeof(SecondToMinuteConverter))]
    public int SmartMoveInterval
    {
        get => _smartMoveInterval;
        set => SetProperty(ref _smartMoveInterval, value);
    }

    public static ObservableCollection<int> SmartMoveIntervals { get; } =
        new() { 1, 2, 5, 10, 20, 30, 40, 50, 60 };
    #endregion

    #region PetGraph
    private string _petGraph;

    /// <summary>
    /// 桌宠选择内容
    /// </summary>
    [ReflectionProperty(nameof(Setting.PetGraph))]
    public string PetGraph
    {
        get => _petGraph;
        set => SetProperty(ref _petGraph, value);
    }
    #endregion

    #region MusicCatch
    private int _musicCatch;

    /// <summary>
    /// 当实时播放音量达到该值时运行音乐动作
    /// </summary>
    [ReflectionProperty(nameof(Setting.MusicCatch))]
    [ReflectionPropertyConverter(typeof(PercentageConverter))]
    public int MusicCatch
    {
        get => _musicCatch;
        set => SetProperty(ref _musicCatch, value);
    }
    #endregion

    #region MusicMax
    private int _musicMax;

    /// <summary>
    /// 当实时播放音量达到该值时运行特殊音乐动作
    /// </summary>
    [ReflectionProperty(nameof(Setting.MusicMax))]
    [ReflectionPropertyConverter(typeof(PercentageConverter))]
    public int MusicMax
    {
        get => _musicMax;
        set => SetProperty(ref _musicMax, value);
    }
    #endregion

    #region AutoBuy
    private bool _autoBuy;

    // TODO 加入 AutoBuy
    /// <summary>
    /// 允许桌宠自动购买食品
    /// </summary>
    [ReflectionProperty(nameof(Setting.AutoBuy))]
    public bool AutoBuy
    {
        get => _autoBuy;
        set => SetProperty(ref _autoBuy, value);
    }
    #endregion

    #region AutoGift
    private bool _autoGift;

    // TODO 加入 AutoGift
    /// <summary>
    /// 允许桌宠自动购买礼物
    /// </summary>
    [ReflectionProperty(nameof(Setting.AutoGift))]
    public bool AutoGift
    {
        get => _autoGift;
        set => SetProperty(ref _autoGift, value);
    }
    #endregion

    #region MoveAreaDefault
    private bool _moveAreaDefault;

    [ReflectionProperty(nameof(Setting.MoveAreaDefault))]
    public bool MoveAreaDefault
    {
        get => _moveAreaDefault;
        set => SetProperty(ref _moveAreaDefault, value);
    }
    #endregion

    #region MoveArea
    private System.Drawing.Rectangle _moveArea;

    [ReflectionProperty(nameof(Setting.MoveArea))]
    public System.Drawing.Rectangle MoveArea
    {
        get => _moveArea;
        set => SetProperty(ref _moveArea, value);
    }
    #endregion
}

public class SecondToMinuteConverter : ReflectionConverterBase<int, int>
{
    public override int Convert(int sourceValue)
    {
        return sourceValue * 60;
    }

    public override int ConvertBack(int targetValue)
    {
        if (targetValue == 30)
            return 1;
        else
            return targetValue / 60;
    }
}

public class PercentageConverter : ReflectionConverterBase<int, double>
{
    public override double Convert(int sourceValue)
    {
        return sourceValue / 100.0;
    }

    public override int ConvertBack(double targetValue)
    {
        return System.Convert.ToInt32(targetValue * 100);
    }
}
