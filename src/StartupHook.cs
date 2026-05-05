using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using ClrHook.Terraria;

#pragma warning disable IDE0052 // Ignore unused private members

/// <see cref="https://github.com/dotnet/runtime/blob/main/docs/design/features/host-startup-hook.md"/>
internal class StartupHook
{
    // prevent GC from collecting the hook
    static Hook? runHook;

    public static void Initialize()
    {
        // Terraria .net4 will typically request mscorlib, which is a facsade
        // This dll is built on netstandard, which is somewhat the same and is 1:1 with mscorlib
        // and as such here we just resolve netstandard => mscorlib.

        AppDomain.CurrentDomain.AssemblyResolve += (sender, resolveArgs) =>
        {
            Globals.Log($"Resolving assembly: {resolveArgs.Name}");
            var name = new AssemblyName(resolveArgs.Name).Name;
            return name == "netstandard" ? typeof(object).Assembly : null;
        };

        // Avoid touching Terraria.Main until Terraria.Program.LaunchGame is called.
        // Static variable dependencies and initialization order is a nightmare in this game.
        // Thus, instead we hook FNA/XNA's Run method

        var from = typeof(Game).GetMethod("Run", BindingFlags.Instance | BindingFlags.Public);
        var to = typeof(StartupHook).GetMethod("Xna_Run", BindingFlags.Static | BindingFlags.Public);

        Globals.Log($"Hooking XNA Game.Run at {from} to {to}");

        runHook = new Hook(from, to);
    }

    public static void Xna_Run(Action<Game> original, Game instance)
    {
        Globals.Log("XNA Game.Run called");

        // Terraria.Main variables are ready now... ~_~
        ModuleAttribute.InvokeStaticInitializers();

        original(instance);

        Globals.IsRunning = false;
    }
}
