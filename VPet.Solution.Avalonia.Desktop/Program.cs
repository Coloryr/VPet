using System;
using System.Diagnostics;
using System.IO;
using Avalonia;
using VPet.Avalonia;

namespace VPet.Solution.Avalonia.Desktop;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        SystemInfo.Init();

        if (args.Length > 0)
        {
            switch (args[0].ToLower())
            {
                case "removestarup":
                    var path =
                        Environment.GetFolderPath(Environment.SpecialFolder.Startup)
                        + @"\VPET_Simulator.lnk";
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    break;
                case "launchsteam":
                    var psi = new ProcessStartInfo
                    {
                        FileName = "cmd",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        Arguments = "/c start steam://rungameid/1920960"
                    };
                    Process.Start(psi);
                    break;
            }
            return;
        }

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

}
