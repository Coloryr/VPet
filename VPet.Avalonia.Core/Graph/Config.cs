using System.Collections.Generic;
using Avalonia;
using LinePutScript;
using LinePutScript.Converter;
using LinePutScript.Dictionary;
using VPet.Core;

namespace VPet.Avalonia.Core.Graph;

/// <summary>
/// 动画设置
/// </summary>
public class Config
{
    /// <summary>
    /// 摸头触发位置
    /// </summary>
    public Point TouchHeadLocate;
    /// <summary>
    /// 提起触发位置
    /// </summary>
    public Point[] TouchRaisedLocate;
    /// <summary>
    /// 摸头触发大小
    /// </summary>
    public Size TouchHeadSize;
    /// <summary>
    /// 摸身体触发位置
    /// </summary>
    public Point TouchBodyLocate;
    /// <summary>
    /// 摸身体触发大小
    /// </summary>
    public Size TouchBodySize;
    /// <summary>
    /// 提起触发大小
    /// </summary>
    public Size[] TouchRaisedSize;

    /// <summary>
    /// 提起定位点
    /// </summary>
    public Point[] RaisePoint;

    /// <summary>
    /// 所有移动
    /// </summary>
    public List<Move> Moves = [];

    /// <summary>
    /// 所有工作/学习
    /// </summary>
    public List<Work> Works = [];

    public Line_D Str;
    /// <summary>
    /// 持续时间
    /// </summary>
    public Line_D Duration;

    /// <summary>
    /// 剩余设置数据
    /// </summary>
    public LPS_D Data;

    /// <summary>
    /// 获取持续时间
    /// </summary>
    /// <param name="name">动画名称</param>
    /// <returns>持续时间</returns>
    public int GetDuration(string name) => Duration.GetInt(name ?? "", 10);
    /// <summary>
    /// 获得 Str 里面储存的文本 (已翻译)
    /// </summary>
    /// <param name="name">定位名称</param>
    /// <returns>储存的文本 (已翻译)</returns>
    public string? StrGetString(string name) => Str.GetString(name)?.Translate();
    /// <summary>
    /// 初始化设置
    /// </summary>
    /// <param name="lps"></param>
    public Config(LpsDocument lps)
    {
        TouchHeadLocate = new Point(lps["touchhead"][(gdbe)"px"], lps["touchhead"][(gdbe)"py"]);
        TouchHeadSize = new Size(lps["touchhead"][(gdbe)"sw"], lps["touchhead"][(gdbe)"sh"]);
        TouchBodyLocate = new Point(lps["touchbody"][(gdbe)"px"], lps["touchbody"][(gdbe)"py"]);
        TouchBodySize = new Size(lps["touchbody"][(gdbe)"sw"], lps["touchbody"][(gdbe)"sh"]);
        TouchRaisedLocate =
        [
            new(lps["touchraised"][(gdbe)"happy_px"], lps["touchraised"][(gdbe)"happy_py"]),
            new(lps["touchraised"][(gdbe)"nomal_px"], lps["touchraised"][(gdbe)"nomal_py"]),
            new(lps["touchraised"][(gdbe)"poorcondition_px"], lps["touchraised"][(gdbe)"poorcondition_py"]),
            new(lps["touchraised"][(gdbe)"ill_px"], lps["touchraised"][(gdbe)"ill_py"])
        ];
        TouchRaisedSize =
        [
            new Size(lps["touchraised"][(gdbe)"happy_sw"], lps["touchraised"][(gdbe)"happy_sh"]),
            new Size(lps["touchraised"][(gdbe)"nomal_sw"], lps["touchraised"][(gdbe)"nomal_sh"]),
            new Size(lps["touchraised"][(gdbe)"poorcondition_sw"], lps["touchraised"][(gdbe)"poorcondition_sh"]),
            new Size(lps["touchraised"][(gdbe)"ill_sw"], lps["touchraised"][(gdbe)"ill_sh"])
        ];
        RaisePoint =
        [
            new(lps["raisepoint"][(gdbe)"happy_x"], lps["raisepoint"][(gdbe)"happy_y"]),
            new(lps["raisepoint"][(gdbe)"nomal_x"], lps["raisepoint"][(gdbe)"nomal_y"]),
            new(lps["raisepoint"][(gdbe)"poorcondition_x"], lps["raisepoint"][(gdbe)"poorcondition_y"]),
            new(lps["raisepoint"][(gdbe)"ill_x"], lps["raisepoint"][(gdbe)"ill_y"])
        ];

        foreach (var line in lps.FindAllLine("work"))
        {
            Works.Add(LPSConvert.DeserializeObject<Work>(line)!);
        }
        foreach (var line in lps.FindAllLine("move"))
        {
            Moves.Add(LPSConvert.DeserializeObject<Move>(line)!);
        }
        Str = new Line_D(lps["str"]);
        Duration = new Line_D(lps["duration"]);
        Data = new LPS_D(lps);
    }

    /// <summary>
    /// 加载更多设置,新的替换后来的,允许空内容
    /// </summary>
    public void Set(LpsDocument lps)
    {
        if (lps.FindLine("touchhead") != null && lps["touchhead"][(gdbe)"py"] != 0)
        {
            TouchHeadLocate = new Point(lps["touchhead"][(gdbe)"px"], lps["touchhead"][(gdbe)"py"]);
            TouchHeadSize = new Size(lps["touchhead"][(gdbe)"sw"], lps["touchhead"][(gdbe)"sh"]);
        }
        if (lps.FindLine("touchbody") != null && lps["touchbody"][(gdbe)"py"] != 0)
        {
            TouchBodyLocate = new Point(lps["touchbody"][(gdbe)"px"], lps["touchbody"][(gdbe)"py"]);
            TouchBodySize = new Size(lps["touchbody"][(gdbe)"sw"], lps["touchbody"][(gdbe)"sh"]);
        }

        if (lps.FindLine("touchraised") != null)
        {
            if (lps["touchraised"][(gdbe)"happy_py"] != 0)
                TouchRaisedLocate =
                [
                    new(lps["touchraised"].GetDouble("happy_px", TouchRaisedLocate[0].X), lps["touchraised"].GetDouble("happy_py", TouchRaisedLocate[0].Y)),
                    new(lps["touchraised"].GetDouble("nomal_px", TouchRaisedLocate[1].X), lps["touchraised"].GetDouble("nomal_py", TouchRaisedLocate[1].Y)),
                    new(lps["touchraised"].GetDouble("poorcondition_px", TouchRaisedLocate[2].X), lps["touchraised"].GetDouble("poorcondition_py", TouchRaisedLocate[2].Y)),
                    new(lps["touchraised"].GetDouble("ill_px", TouchRaisedLocate[3].X), lps["touchraised"].GetDouble("ill_py", TouchRaisedLocate[3].Y))
                ];
            if (lps["touchraised"][(gdbe)"happy_sh"] != 0)
                TouchRaisedSize =
                [
                    new(lps["touchraised"].GetDouble("happy_sw", TouchRaisedSize[0].Width), lps["touchraised"].GetDouble("happy_sh", TouchRaisedSize[0].Height)),
                    new(lps["touchraised"].GetDouble("nomal_sw", TouchRaisedSize[1].Width), lps["touchraised"].GetDouble("nomal_sh", TouchRaisedSize[1].Height)),
                    new(lps["touchraised"].GetDouble("poorcondition_sw", TouchRaisedSize[2].Width), lps["touchraised"].GetDouble("poorcondition_sh", TouchRaisedSize[2].Height)),
                    new(lps["touchraised"].GetDouble("ill_sw", TouchRaisedSize[3].Width), lps["touchraised"].GetDouble("ill_sh", TouchRaisedSize[3].Height))
                ];
        }
        if (lps.FindLine("raisepoint") != null && lps["raisepoint"][(gdbe)"happy_y"] != 0)
        {
            RaisePoint =
            [
                new(lps["raisepoint"].GetDouble("happy_x",RaisePoint[0].X), lps["raisepoint"].GetDouble("happy_y",RaisePoint[0].Y)),
                new(lps["raisepoint"].GetDouble ("nomal_x",RaisePoint[1].X), lps["raisepoint"].GetDouble("nomal_y",RaisePoint[1].Y)),
                new(lps["raisepoint"].GetDouble("poorcondition_x",RaisePoint[2].X), lps["raisepoint"].GetDouble("poorcondition_y",RaisePoint[2].Y)),
                new(lps["raisepoint"].GetDouble("ill_x",RaisePoint[3].X), lps["raisepoint"].GetDouble("ill_y",RaisePoint[3].Y))
            ];
        }

        Str.AddRange(lps["str"]);
        Duration.AddRange(lps["duration"]);

        foreach (var line in lps.FindAllLine("work"))
        {
            Works.Add(LPSConvert.DeserializeObject<Work>(line)!);
        }
        foreach (var line in lps.FindAllLine("move"))
        {
            Moves.Add(LPSConvert.DeserializeObject<Move>(line)!);
        }
        foreach (var line in lps)
        {
            if (!string.IsNullOrEmpty(line.info))
                Data.Add(line);
        }
    }
}
