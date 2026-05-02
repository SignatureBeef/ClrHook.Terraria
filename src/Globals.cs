using System;

namespace ClrHook.Terraria;

public static class Globals
{
    public static bool IsRunning { get; set; } = true;

    public static readonly bool IsVerbose = Environment.GetEnvironmentVariable("CLRHOOK_VERBOSE") == "1";

    public static void Log(string message)
    {
        if (IsVerbose)
        {
            Console.WriteLine($"[ClrHook.Terraria] {message}");
        }
    }

    public static void Error(string message)
    {
        Console.Error.WriteLine($"[ClrHook.Terraria] ERROR: {message}");
    }
}
