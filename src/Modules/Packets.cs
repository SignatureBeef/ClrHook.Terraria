using System;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;

namespace ClrHook.Terraria.Modules;

public class PacketWriter(Stream stream) : IDisposable
{
    private readonly BinaryWriter _writer = new(stream);

    public PacketWriter Write(byte value) { _writer.Write(value); return this; }
    public PacketWriter Write(ushort value) { _writer.Write(value); return this; }
    public PacketWriter Write(int value) { _writer.Write(value); return this; }
    public PacketWriter Write(short value) { _writer.Write(value); return this; }
    public PacketWriter Write(float value) { _writer.Write(value); return this; }
    public PacketWriter Write(double value) { _writer.Write(value); return this; }
    public PacketWriter Write(string value) { _writer.Write(value); return this; }
    public PacketWriter Write7BitEncodedInt(int value) { _writer.Write7BitEncodedInt(value); return this; }
    public PacketWriter WriteVector2(Vector2 value) { _writer.Write(value.X); _writer.Write(value.Y); return this; }
    public PacketWriter WritePackedVector2(Vector2 value) { _writer.WritePackedVector2(value); return this; }

    public void Dispose() => _writer.Dispose();
}

public static class PacketBuilder
{
    public enum Versions : int
    {
        v1455 = 318,
        v1456 = 319
    }

    // Helper to build a packet with header (id, length) and contents
    public static byte[] BuildPacket(byte packetId, Action<PacketWriter> writeContents)
    {
        using MemoryStream ms = new();
        using PacketWriter writer = new(ms);
        writer.Write((ushort)0); // Placeholder for length
        writer.Write(packetId);
        long start = ms.Position;
        writeContents(writer);
        long end = ms.Position;
        ushort len = (ushort)(end - start);
        ms.Position = 0;
        writer.Write((ushort)(len + 3 /*header*/));
        ms.Position = end;
        return ms.ToArray();
    }

    public static void BuildAndSend(byte packetId, Action<PacketWriter> writeContents)
    {
        byte[] data = BuildPacket(packetId, writeContents);
        SendPacketToServer(data);
    }

    // public static byte[] ConnectRequest(Versions version = Versions.v1456)
    // {
    //     return BuildPacket(1, writer =>
    //     {
    //         writer.Write("Terraria" + (int)version);
    //     });
    // }

    // public static byte[] GetSection()
    // {
    //     return BuildPacket(1, writer =>
    //     {
    //         writer.Write(0);
    //         writer.Write(0);
    //         writer.Write((byte)0);
    //     });
    // }

    // Add more packet builders as needed, e.g. PlayerInfo, etc.


    // use reflection or direct method calls to send the raw packet data to the server
    // this way we dont host the vanilla code.
    static readonly MethodInfo _sendPacketToServerMethod = typeof(NetMessage)
        .GetMethod("SendPacketToServer", BindingFlags.NonPublic | BindingFlags.Static);

    public static void SendPacketToServer(byte[] data)
    {
        // use reflection or direct method calls to send the raw packet data to the server
        // this way we dont host the vanilla code.
        _sendPacketToServerMethod.Invoke(Netplay.Connection, [data]);
    }
}
