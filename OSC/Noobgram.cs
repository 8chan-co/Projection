using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading;

namespace Projection;

internal static class Noobgram
{
    private static readonly Socket Client = new(SocketType.Dgram, ProtocolType.Udp);
    //private static readonly Socket Server = new(SocketType.Dgram, ProtocolType.Udp);

    static Noobgram()
    {
        Client.Connect(new IPEndPoint(IPAddress.Loopback, 9000));
        //Server.Bind(new IPEndPoint(IPAddress.Loopback, 9001));
    }

    private static unsafe void Main()
    {
        ReadOnlySpan<byte> RMessage = "/avatar/parameters/R\0\0\0\0,f\0\0\0\0\0\0"u8;
        ReadOnlySpan<byte> GMessage = "/avatar/parameters/G\0\0\0\0,f\0\0\0\0\0\0"u8;
        ReadOnlySpan<byte> BMessage = "/avatar/parameters/B\0\0\0\0,f\0\0\0\0\0\0"u8;
        ReadOnlySpan<byte> QMessage = "/avatar/parameters/Q\0\0\0\0,T\0\0"u8;

        Span<byte> RDatagram = stackalloc byte[RMessage.Length];
        Span<byte> GDatagram = stackalloc byte[GMessage.Length];
        Span<byte> BDatagram = stackalloc byte[BMessage.Length];
        Span<byte> QDatagram = stackalloc byte[QMessage.Length];

        RMessage.CopyTo(RDatagram);
        GMessage.CopyTo(GDatagram);
        BMessage.CopyTo(BDatagram);
        QMessage.CopyTo(QDatagram);

        Bitmap Texture = new("c:/users/ophur/desktop/4x4.png");

        int PixelCount = Texture.Width * Texture.Height;

        List<Vector3> Colors = new(PixelCount);

        for (int Index = 0; Index < PixelCount; ++Index)
        {
            Color Color = Texture.GetPixel(Index & 3, Index >> 2);

            float R = GammaToLinearSpace(Color.R / (float)byte.MaxValue);
            float G = GammaToLinearSpace(Color.G / (float)byte.MaxValue);
            float B = GammaToLinearSpace(Color.B / (float)byte.MaxValue);

            Colors.Add(Vector3.Create(R, G, B));
        }

        ChannelDatagrams Datagrams = new(RDatagram, GDatagram, BDatagram);

        for (int Pixel = 0; Pixel < Colors.Count; ++Pixel)
        {
            SendColor(Datagrams, Colors[Pixel]);

            Thread.Sleep(500);

            AdvanceQueue(QMessage);

            Thread.Sleep(100);
        }
    }

    private static void SendColor(ChannelDatagrams Channels, Vector3 Color)
    {
        for (int Channel = 0; Channel < Channels.Count; ++Channel)
        {
            Span<byte> Datagram = Channels[Channel];

            BinaryPrimitives.WriteSingleBigEndian(Datagram[^4..], Color[Channel]);

            Client.Send(Datagram, SocketFlags.None);

            PrintReadableSingleDatagram(Datagram);
        }
    }

    private static void AdvanceQueue(ReadOnlySpan<byte> Datagram)
    {
        Client.Send(Datagram, SocketFlags.None);

        PrintReadableBooleanDatagram(Datagram);
    }

    private static float GammaToLinearSpace(float Value) => Value switch
    {
        <= 0.04045F => Value / 12.92F,
        < 1.0F => MathF.Pow((Value + 0.055F) / 1.055F, 2.4F),
        _ => MathF.Pow(Value, 2.2F)
    };

    [Conditional("DEBUG")]
    private static void PrintReadableSingleDatagram(ReadOnlySpan<byte> Datagram)
    {
        string String = Encoding.UTF8.GetString(Datagram[..^4]).Replace('\0', '-');

        float Single = BinaryPrimitives.ReadSingleBigEndian(Datagram[^4..]);

        Console.WriteLine($"{String}{Single}");
    }

    [Conditional("DEBUG")]
    private static void PrintReadableBooleanDatagram(ReadOnlySpan<byte> Datagram)
    {
        string String = Encoding.UTF8.GetString(Datagram).Replace('\0', '-');

        bool Boolean = Datagram[^3] is (byte)'T';

        Console.WriteLine($"{String}{Boolean}");
    }
}
