using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPet.Core;

namespace VPet.Avalonia.Core.Graph;

/// <summary>
/// 游戏使用资源
/// </summary>
public class GameCore
{
    /// <summary>
    /// 控制器
    /// </summary>
    public IController Controller;
    /// <summary>
    /// 触摸范围和事件列表
    /// </summary>
    public List<TouchArea> TouchEvent = new List<TouchArea>();
    /// <summary>
    /// 图形核心
    /// </summary>
    public GraphCore Graph;
    /// <summary>
    /// 游戏数据
    /// </summary>
    public IGameSave Save;
}
