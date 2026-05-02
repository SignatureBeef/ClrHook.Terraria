// using System;

// namespace ClrHook.Terraria.Modules;

// public class ConsoleModule
// {
//     [Module]
//     public static void Initialize()
//     {
//         var interactiveThread = new System.Threading.Thread(InteractiveTerminal);

//         // Register Ctrl+C and process exit handlers
//         Console.CancelKeyPress += (sender, e) =>
//         {
//             Globals.Log("Ctrl+C pressed, shutting down interactive terminal...");
//             Globals.IsRunning = false;
//             interactiveThread.Join();
//         };
//         AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
//         {
//             Globals.Log("Process is exiting, shutting down interactive terminal...");
//             Globals.IsRunning = false;
//         };

//         interactiveThread.Start();
//     }

//     static void InteractiveTerminal()
//     {
//         string buffer = "";
//         while (Globals.IsRunning)
//         {
//             Console.Write(buffer.Length == 0 ? "> " : "");
//             while (Globals.IsRunning && !Console.KeyAvailable)
//             {
//                 System.Threading.Thread.Sleep(50);
//             }
//             if (!Globals.IsRunning) break;
//             var key = Console.ReadKey(intercept: true);
//             if (key.Key == ConsoleKey.Enter)
//             {
//                 Console.WriteLine();
//                 var input = buffer;
//                 buffer = "";
//                 if (input == "exit")
//                 {
//                     break;
//                 }
//                 else
//                 {
//                     Globals.Log($"You entered: {input}");
//                 }
//             }
//             else if (key.Key == ConsoleKey.Backspace)
//             {
//                 if (buffer.Length > 0)
//                 {
//                     buffer = buffer.Substring(0, buffer.Length - 1);
//                     Console.Write("\b \b");
//                 }
//             }
//             else if (!char.IsControl(key.KeyChar))
//             {
//                 buffer += key.KeyChar;
//                 Console.Write(key.KeyChar);
//             }
//         }

//         Globals.Log("Interactive terminal exiting");
//     }
// }
