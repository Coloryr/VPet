﻿using LinePutScript;
using LinePutScript.Converter;

namespace VPet.Core;

/// <summary>
/// 游戏存档
/// </summary>
public class GameSave : IGameSave
{
    /// <summary>
    /// 宠物名字
    /// </summary>
    [Line(name: "name")]
    public string Name { get; set; }
    public string HostName { get; set; }

    /// <summary>
    /// 金钱
    /// </summary>
    [Line(Type = LPSConvert.ConvertType.ToFloat, Name = "money")]
    public double Money { get; set; }
    /// <summary>
    /// 经验值
    /// </summary>
    [Line(type: LPSConvert.ConvertType.ToFloat, name: "exp")] 
    public double Exp { get; set; }
    /// <summary>
    /// 等级
    /// </summary>
    public int Level => Exp < 0 ? 1 : (int)(Math.Sqrt(Exp) / 10) + 1;
    /// <summary>
    /// 升级所需经验值
    /// </summary>
    /// <returns></returns>
    public int LevelUpNeed() => (int)Math.Pow(Level * 10, 2);
    /// <summary>
    /// 体力 0-100
    /// </summary>
    [Line(Type = LPSConvert.ConvertType.ToFloat, IgnoreCase = true)]
    public double Strength { get => _strength; set => _strength = Math.Min(StrengthMax, Math.Max(0, value)); }

    public double StrengthMax => 100;

    protected double _strength;

    /// <summary>
    /// 待补充的体力,随着时间缓慢加给桌宠
    /// </summary>//让游戏更有游戏性
    [Line(Type = LPSConvert.ConvertType.ToFloat, IgnoreCase = true)]
    public double StoreStrength { get; set; }
    /// <summary>
    /// 变化 体力
    /// </summary>
    public double ChangeStrength { get; set; } = 0;
    public void StrengthChange(double value)
    {
        ChangeStrength += value;
        Strength += value;
    }
    /// <summary>
    /// 饱腹度
    /// </summary>
    [Line(Type = LPSConvert.ConvertType.ToFloat)]
    public double StrengthFood
    {
        get => _strengthFood; set
        {
            value = Math.Min(100, value);
            if (value <= 0)
            {
                Health += value;
                _strengthFood = 0;
            }
            else
                _strengthFood = value;
        }
    }

    protected double _strengthFood;

    /// <summary>
    /// 待补充的饱腹度,随着时间缓慢加给桌宠
    /// </summary>//让游戏更有游戏性
    [Line(Type = LPSConvert.ConvertType.ToFloat)]
    public double StoreStrengthFood { get; set; }
    public void StrengthChangeFood(double value)
    {
        ChangeStrengthFood += value;
        StrengthFood += value;
    }
    /// <summary>
    /// 变化 食物
    /// </summary>
    public double ChangeStrengthFood { get; set; } = 0;

    /// <summary>
    /// 口渴度
    /// </summary>
    [Line(Type = LPSConvert.ConvertType.ToFloat, IgnoreCase = true)]
    public double StrengthDrink
    {
        get => _strengthDrink; set
        {
            value = Math.Min(100, value);
            if (value <= 0)
            {
                Health += value;
                _strengthDrink = 0;
            }
            else
                _strengthDrink = value;
        }
    }

    protected double _strengthDrink;

    /// <summary>
    /// 待补充的口渴度,随着时间缓慢加给桌宠
    /// </summary>//让游戏更有游戏性
    [Line(Type = LPSConvert.ConvertType.ToFloat)]
    public double StoreStrengthDrink { get; set; }
    /// <summary>
    /// 变化 口渴度
    /// </summary>
    public double ChangeStrengthDrink { get; set; } = 0;
    public void StrengthChangeDrink(double value)
    {
        ChangeStrengthDrink += value;
        StrengthDrink += value;
    }
    /// <summary>
    /// 心情
    /// </summary>
    [Line(Type = LPSConvert.ConvertType.ToFloat, IgnoreCase = true)]
    public double Feeling
    {
        get => _feeling; set
        {

            value = Math.Min(100, value);
            if (value <= 0)
            {
                Health += value / 2;
                Likability += value / 2;
                _feeling = 0;
            }
            else
                _feeling = value;
        }
    }

    protected double _feeling;

    /// <summary>
    /// 变化 心情
    /// </summary>
    public double ChangeFeeling { get; set; } = 0;
    public void FeelingChange(double value)
    {
        ChangeFeeling += value;
        Feeling += value;
    }
    /// <summary>
    /// 健康(生病)(隐藏)
    /// </summary>
    [Line(Type = LPSConvert.ConvertType.ToFloat)]
    public double Health { get => _health; set => _health = Math.Min(100, Math.Max(0, value)); }

    protected double _health;

    /// <summary>
    /// 好感度(隐藏)(累加值)
    /// </summary>
    [Line(Type = LPSConvert.ConvertType.ToFloat)]
    public double Likability
    {
        get => _likability; set
        {
            var max = LikabilityMax;
            value = Math.Max(0, value);
            if (value > max)
            {
                _likability = max;
                Health += value - max;
            }
            else
                _likability = value;
        }
    }

    protected double _likability;

    /// <summary>
    /// 清除变化
    /// </summary>
    public void CleanChange()
    {
        ChangeStrength /= 2;
        ChangeFeeling /= 2;
        ChangeStrengthDrink /= 2;
        ChangeStrengthFood /= 2;
    }
    /// <summary>
    /// 取回被储存的体力
    /// </summary>
    public void StoreTake()
    {
        const int t = 10;

        var s = StoreStrength / t;
        StoreStrength -= s;
        if (Math.Abs(StoreStrength) < 1)
            StoreStrength = 0;
        else
            StrengthChange(s);

        s = StoreStrengthDrink / t;
        StoreStrengthDrink -= s;
        if (Math.Abs(StoreStrengthDrink) < 1)
            StoreStrengthDrink = 0;
        else
            StrengthChangeDrink(s);

        s = StoreStrengthFood / t;
        StoreStrengthFood -= s;
        if (Math.Abs(StoreStrengthFood) < 1)
            StoreStrengthFood = 0;
        else
            StrengthChangeFood(s);
    }
    /// <summary>
    /// 吃食物
    /// </summary>
    /// <param name="food">食物类</param>
    public void EatFood(IFood food)
    {
        Exp += food.Exp;
        var tmp = food.Strength / 2;
        StrengthChange(tmp);
        StoreStrength += tmp;
        tmp = food.StrengthFood / 2;
        StrengthChangeFood(tmp);
        StoreStrengthFood += tmp;
        tmp = food.StrengthDrink / 2;
        StrengthChangeDrink(tmp);
        StoreStrengthDrink += tmp;
        FeelingChange(food.Feeling);
        Health += food.Health;
        Likability += food.Likability;
    }
    /// <summary>
    /// 宠物当前状态
    /// </summary>
    [Line(name: "mode")]
    public IGameSave.ModeType Mode { get; set; } = IGameSave.ModeType.Nomal;

    public double LikabilityMax => 90 + Level * 10;

    public double FeelingMax => 100;

    public double ExpBonus => 1;

    /// <summary>
    /// 计算宠物当前状态
    /// </summary>
    public IGameSave.ModeType CalMode()
    {
        int realhel = 60 - (Feeling >= 80 ? 12 : 0) - (Likability >= 80 ? 12 : Likability >= 40 ? 6 : 0);
        //先从最次的开始
        if (Health <= realhel)
        {
            //可以确认从状态不佳和生病二选一
            if (Health <= realhel / 2)
            {//生病
                return IGameSave.ModeType.Ill;
            }
            else
            {
                return IGameSave.ModeType.PoorCondition;
            }
        }
        //然后判断是高兴还是普通
        double realfel = .90 - (Likability >= 80 ? .20 : Likability >= 40 ? .10 : 0);
        double felps = Feeling / FeelingMax;
        if (felps >= realfel)
        {
            return IGameSave.ModeType.Happy;
        }
        else if (felps <= realfel / 2)
        {
            return IGameSave.ModeType.PoorCondition;
        }
        return IGameSave.ModeType.Nomal;
    }
    /// <summary>
    /// 新游戏
    /// </summary>
    public GameSave(string name)
    {
        Name = name;
        Money = 100;
        Exp = 0;
        Strength = 100;
        StrengthFood = 100;
        StrengthDrink = 100;
        Feeling = 60;
        Health = 100;
        Likability = 0;
        Mode = CalMode();
    }
    /// <summary>
    /// 读档
    /// </summary>
    public GameSave()
    {
        //Money = line.GetFloat("money");
        //Name = line.Info;
        //Exp = line.GetInt("exp");
        //Strength = line.GetFloat("strength");
        //StrengthDrink = line.GetFloat("strengthdrink");
        //StrengthFood = line.GetFloat("strengthfood");
        //Feeling = line.GetFloat("feeling");
        //Health = line.GetFloat("health");
        //Likability = line.GetFloat("likability");
        //Mode = CalMode();
    }
    /// <summary>
    /// 读档
    /// </summary>
    public static GameSave Load(ILine data) => LPSConvert.DeserializeObject<GameSave>(data)!;
    /// <summary>
    /// 存档
    /// </summary>
    /// <returns>存档行</returns>
    public Line ToLine()
    {
        //Line save = new Line("vpet", Name);
        //save.SetFloat("money", Money);
        //save.SetInt("exp", Exp);
        //save.SetFloat("strength", Strength);
        //save.SetFloat("strengthdrink", StrengthDrink);
        //save.SetFloat("strengthfood", StrengthFood);
        //save.SetFloat("feeling", Feeling);
        //save.SetFloat("health", Health);
        //save.SetFloat("Likability", Likability);
        return LPSConvert.SerializeObject(this, "vpet");
    }
}