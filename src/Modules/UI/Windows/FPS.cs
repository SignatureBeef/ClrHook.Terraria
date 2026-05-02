using ImGuiNET;

namespace ClrHook.Terraria.Modules.UI.Windows;

public class FPS : Window
{
    public override string Title { get; set; } = "FPS";

    public override void Draw()
    {
        ImGui.Text(string.Format("Application average {0:F3} ms/frame ({1:F1} FPS)", 1000f / ImGui.GetIO().Framerate, ImGui.GetIO().Framerate));
    }
}
