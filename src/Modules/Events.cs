using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;

namespace ClrHook.Terraria;

public delegate void Main_DrawDelegate(global::Terraria.Main main, GameTime gameTime);
public delegate void Main_LoadContentDelegate(global::Terraria.Main main);

public static class EventsModule
{
    public static event Main_DrawDelegate? OnMainDraw;
    public static event Main_LoadContentDelegate? OnMainLoadContent;

    // keep hooks alive by storing them in static fields
    static Hook? mainDrawHook;
    static Hook? mainLoadContentHook;

    [Module]
    public static void Initialize()
    {
        var from = typeof(global::Terraria.Main).GetMethod("Draw", BindingFlags.Instance | BindingFlags.NonPublic);
        var to = typeof(EventsModule).GetMethod("Main_Draw", BindingFlags.Static | BindingFlags.Public);
        Globals.Log($"Hooking Terraria.Main.Draw at {from} to {to}");
        mainDrawHook = new Hook(from, to);

        from = typeof(global::Terraria.Main).GetMethod("LoadContent", BindingFlags.Instance | BindingFlags.NonPublic);
        to = typeof(EventsModule).GetMethod("Main_LoadContent", BindingFlags.Static | BindingFlags.Public);
        Globals.Log($"Hooking Terraria.Main.LoadContent at {from} to {to}");
        mainLoadContentHook = new Hook(from, to);
    }

    public static void Main_LoadContent(Action<global::Terraria.Main> original, global::Terraria.Main main)
    {
        Globals.Log("Main.LoadContent called");

        OnMainLoadContent?.Invoke(main);

        original(main);
    }


    static bool isFirstDraw = true;
    public static void Main_Draw(Action<global::Terraria.Main, GameTime> original, global::Terraria.Main main, GameTime gameTime)
    {
        if (isFirstDraw)
        {
            Globals.Log("Main.Draw called");
            isFirstDraw = false;
        }

        original(main, gameTime);

        OnMainDraw?.Invoke(main, gameTime);
    }
}
