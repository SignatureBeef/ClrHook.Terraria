using Microsoft.Xna.Framework;

namespace ClrHook.Terraria.Modules.UI;

public class UI
{
    public static ImGuiHost? ImGuiHost;

    [Module]
    public static void Initialize()
    {
        EventsModule.OnMainLoadContent += LoadContent;
        EventsModule.OnMainDraw += Draw;
    }

    public static void LoadContent(global::Terraria.Main main)
    {
        Globals.Log("Main.LoadContent called");

        ImGuiHost = new ImGuiHost(main);
    }

    static bool isFirstDraw = true;
    public static void Draw(global::Terraria.Main main, GameTime gameTime)
    {
        if (isFirstDraw)
        {
            Globals.Log("Main.Draw called");
            isFirstDraw = false;
        }

        ImGuiHost?.Draw(gameTime);
    }
}
