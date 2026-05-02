namespace ClrHook.Terraria.Modules.UI;

public abstract class Window
{
    public abstract string Title { get; set; }
    public bool IsOpen { get; set; } = true;

    public virtual void Draw()
    {
        // Override this method to implement custom drawing logic for the window
    }
}