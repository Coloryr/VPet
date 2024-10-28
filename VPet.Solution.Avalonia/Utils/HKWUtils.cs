using System.IO;
using System.Text;
using Avalonia.Media.Imaging;

namespace VPet.Solution.Avalonia.Utils;

/// <summary>
/// 工具
/// </summary>
public static class HKWUtils
{
    /// <summary>
    /// 解码像素宽度
    /// </summary>
    public const int DecodePixelWidth = 250;

    /// <summary>
    /// 解码像素高度
    /// </summary>
    public const int DecodePixelHeight = 250;
    public static char[] Separator { get; } = new char[] { '_' };

    /// <summary>
    /// 载入图片到流
    /// </summary>
    /// <param name="imagePath">图片路径</param>
    /// <returns>图片</returns>
    public static Bitmap LoadImageToStream(string imagePath)
    {
        using var stream = File.OpenRead(imagePath);
        return new Bitmap(stream);
    }

    /// <summary>
    /// 获取布尔值
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="boolValue">目标布尔值</param>
    /// <param name="nullValue">为空时布尔值</param>
    /// <returns></returns>
    public static bool GetBool(object value, bool boolValue, bool nullValue)
    {
        if (value is null)
            return nullValue;
        else if (value is bool b)
            return b == boolValue;
        else if (bool.TryParse(value.ToString(), out b))
            return b == boolValue;
        else
            return false;
    }

    /// <summary>
    /// 打开文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    public static void OpenLink(string filePath)
    {
        System.Diagnostics.Process
            .Start(new System.Diagnostics.ProcessStartInfo(filePath) { UseShellExecute = true })
            ?.Close();
    }

    /// <summary>
    /// 从资源管理器打开文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    public static void OpenFileInExplorer(string filePath)
    {
        System.Diagnostics.Process
            .Start("Explorer", $"/select,{Path.GetFullPath(filePath)}")
            ?.Close();
    }

    /// <summary>
    /// 从文件获取只读流 (用于目标文件被其它进程访问的情况)
    /// </summary>
    /// <param name="path">文件</param>
    /// <param name="encoding">编码</param>
    /// <returns>流读取器</returns>
    public static StreamReader StreamReaderOnReadOnly(string path, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        return new StreamReader(fs, encoding);
    }
}
