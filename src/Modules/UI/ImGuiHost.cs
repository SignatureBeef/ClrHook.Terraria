using System;
using System.Collections.Generic;
using ImGuiNET;
using Microsoft.Xna.Framework;

namespace ClrHook.Terraria.Modules.UI;

public class ImGuiHost
{
    private readonly ImGuiRenderer _imGuiRenderer;

    private readonly List<Window> _windows = [];

    public ImGuiHost(Game game)
    {
        _imGuiRenderer = new ImGuiRenderer(game);
        _imGuiRenderer.RebuildFontAtlas();

        // scan current assembly for Window subclasses and add them to the host
        foreach (var type in typeof(ImGuiHost).Assembly.GetTypes())
        {
            if (type.IsSubclassOf(typeof(Window)) && !type.IsAbstract)
            {
                var window = (Window)Activator.CreateInstance(type);
                AddWindow(window);
            }
        }
    }

    public void AddWindow(Window window)
    {
        lock (_windows)
            _windows.Add(window);
    }

    public void RemoveWindow(Window window)
    {
        lock (_windows)
            _windows.Remove(window);
    }

    public void Draw(GameTime gameTime)
    {
        // Call BeforeLayout first to set things up
        _imGuiRenderer.BeforeLayout(gameTime);

        // Draw our UI
        ImGuiLayout();

        // Call AfterLayout now to finish up and draw all the things
        _imGuiRenderer.AfterLayout();
    }

    protected virtual void ImGuiLayout()
    {
        lock (_windows)
            foreach (var window in _windows)
            {
                if (window.IsOpen)
                {
                    ImGui.Begin(window.Title);
                    window.Draw();
                    ImGui.End();
                }
            }
    }
}
