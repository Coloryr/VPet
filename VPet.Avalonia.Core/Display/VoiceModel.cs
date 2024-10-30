using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPet.Avalonia.Core.Display;

public partial class MainModel
{
    /// <summary>
    /// 当前是否正在播放
    /// </summary>
    public bool PlayingVoice = false;

    public bool windowMediaPlayerAvailable = true;
}
