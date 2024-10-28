using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace VPet.Solution.Models.SaveViewer;

/// <summary>
/// 存档模型
/// </summary>
public partial class SaveModel : ObservableObject
{
    /// <summary>
    /// 名称
    /// </summary>
    [ObservableProperty]
    private string _name;

    /// <summary>
    /// 文件路径
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// 统计数据
    /// </summary>
    public ObservableCollection<StatisticDataModel> Statistics { get; set; } = [];

    /// <summary>
    /// 是损坏的
    /// </summary>
    public bool IsDamaged { get; set; }

    [ObservableProperty]
    private DateTime _dateSaved;

    [ObservableProperty]
    private string _petName;
    [ObservableProperty]
    private int _level;
    [ObservableProperty]
    private double _money = 100;
    [ObservableProperty]
    private double _exp = 0;
    [ObservableProperty]
    private double _feeling = 60;
    [ObservableProperty]
    private double _health = 100;
    [ObservableProperty]
    private double _likability = 0;
    [ObservableProperty]
    private IGameSave.ModeType _mode;
    [ObservableProperty]
    private double _strength = 100;
    [ObservableProperty]
    private double _strengthFood = 100;
    [ObservableProperty]
    private double _strengthDrink = 100;
    [ObservableProperty]
    private bool _hashChecked;
    [ObservableProperty]
    private long _totalTime;


    public SaveModel(string filePath, GameSave_v2 save)
    {
        Name = Path.GetFileNameWithoutExtension(filePath);
        FilePath = filePath;
        DateSaved = File.GetLastWriteTime(filePath);
        LoadSave(save.GameSave);
        if (save.Statistics.Data.TryGetValue("stat_total_time", out var time))
            TotalTime = time.GetInteger64();
        HashChecked = save.HashCheck;
        foreach (var data in save.Statistics.Data)
        {
            Statistics.Add(
                new()
                {
                    Id = data.Key,
                    Name = data.Key.Translate(),
                    Value = data.Value
                }
            );
        }
    }

    private void LoadSave(VPet_Simulator.Core.IGameSave save)
    {
        ReflectionUtils.SetValue(save, this);
    }
}
