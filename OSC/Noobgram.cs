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

namespace Ophura;

internal static class Noobgram
{
    private static readonly Socket Sender = new(SocketType.Dgram, ProtocolType.Udp);
    //private static readonly Socket Receiver = new(SocketType.Dgram, ProtocolType.Udp);

    static Noobgram()
    {
        Sender.Connect(new IPEndPoint(IPAddress.Loopback, 9000));
        //Receiver.Bind(new IPEndPoint(IPAddress.Loopback, 9001));
    }

    private static unsafe void Main()
    {
        ReadOnlySpan<byte> RMessage = "/avatar/parameters/R\0\0\0\0,f\0\0\0\0\0\0"u8;
        ReadOnlySpan<byte> GMessage = "/avatar/parameters/G\0\0\0\0,f\0\0\0\0\0\0"u8;
        ReadOnlySpan<byte> BMessage = "/avatar/parameters/B\0\0\0\0,f\0\0\0\0\0\0"u8;
        ReadOnlySpan<byte> QMessage = "/avatar/parameters/Q\0\0\0\0,i\0\0\0\0\0\0"u8;

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

            Thread.Sleep(200);

            BinaryPrimitives.WriteInt32BigEndian(QDatagram[^4..], Pixel + 1 & PixelCount - 1);

            PrintReadableDatagramWithInt32(QDatagram);

            Sender.Send(QDatagram, SocketFlags.None);

            Thread.Sleep(200);
        }

        SendColor(Datagrams, Colors[0]);
    }

    private static void SendColor(ChannelDatagrams Datagrams, Vector3 Color)
    {
        for (int Channel = 0; Channel < Datagrams.Count; ++Channel)
        {
            Span<byte> Datagram = Datagrams[Channel];

            BinaryPrimitives.WriteSingleBigEndian(Datagram[^4..], Color[Channel]);

            Sender.Send(Datagram, SocketFlags.None);

            PrintReadableDatagramWithSingle(Datagram);
        }
    }

    private static float GammaToLinearSpace(float Value) => Value switch
    {
        <= 0.04045F => Value / 12.92F,
        < 1.0F => MathF.Pow((Value + 0.055F) / 1.055F, 2.4F),
        _ => MathF.Pow(Value, 2.2F)
    };

    [Conditional("DEBUG")]
    private static void PrintReadableDatagramWithSingle(ReadOnlySpan<byte> Datagram)
    {
        string String = Encoding.UTF8.GetString(Datagram[..^4]).Replace('\0', '-');

        float Float = BinaryPrimitives.ReadSingleBigEndian(Datagram[^4..]);

        Console.WriteLine($"{String}{Float}");
    }

    [Conditional("DEBUG")]
    private static void PrintReadableDatagramWithInt32(ReadOnlySpan<byte> Datagram)
    {
        string String = Encoding.UTF8.GetString(Datagram[..^4]).Replace('\0', '-');

        int Int32 = BinaryPrimitives.ReadInt32BigEndian(Datagram[^4..]);

        Console.WriteLine($"{String}{Int32}");
    }
}
