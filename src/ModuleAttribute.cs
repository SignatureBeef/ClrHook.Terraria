using System;
using System.Linq;

namespace ClrHook.Terraria;

[AttributeUsage(AttributeTargets.Method)]
public class ModuleAttribute : Attribute
{
    public static void Invoke()
    {
        var asm = typeof(ModuleAttribute).Assembly;
        var methods = asm.GetTypes()
            .SelectMany(t => t.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
            .Where(m => m.GetCustomAttributes(typeof(ModuleAttribute), false).Length > 0)
            .ToArray();

        foreach (var method in methods)
        {
            try
            {
                method.Invoke(null, null);
            }
            catch (Exception ex)
            {
                Globals.Error($"Failed to invoke module method {method}: {ex}");
            }
        }
    }
}

