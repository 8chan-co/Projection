using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Threading;

namespace Projection;

internal static class Noobgram
{
    private static readonly List<Vector3> Pixels = [];

    private static readonly Socket Client = new(SocketType.Dgram, ProtocolType.Udp);
    private static readonly Socket Server = new(SocketType.Dgram, ProtocolType.Udp);

    private static int PixelIndex;

    static Noobgram()
    {
        Client.Connect(new IPEndPoint(IPAddress.Loopback, 9000));
        Server.Bind(new IPEndPoint(IPAddress.Loopback, 9001));
    }

    private static void Main()
    {
        using (Client)
        using (Server)
        {
            StoreTexturePixels("c:/users/ophur/desktop/4x4.png");

            new Thread(TransferOnDemand).UnsafeStart();

            ChannelDatagrams Datagrams = new(stackalloc byte[32], stackalloc byte[32], stackalloc byte[32]);

            SendPixelColour(Datagrams, Pixels[PixelIndex++ & (Pixels.Count - 1)]);

            AdvanceQueue();

            Thread.Sleep(Timeout.Infinite);
        }
    }

    private static void StoreTexturePixels(string Filename)
    {
        using Bitmap Texture = new(Filename);

        int PixelCount = Texture.Width * Texture.Height;

        Pixels.Capacity = PixelCount;

        for (int Index = 0; Index < PixelCount; ++Index)
        {
            Color Pixel = Texture.GetPixel(Index & 3, Index >> 2);

            float R = GammaToLinearSpace(Pixel.R / (float)byte.MaxValue);
            float G = GammaToLinearSpace(Pixel.G / (float)byte.MaxValue);
            float B = GammaToLinearSpace(Pixel.B / (float)byte.MaxValue);

            Pixels.Add(Vector3.Create(R, G, B));
        }

        static float GammaToLinearSpace(float Value) => Value switch
        {
            <= 0.04045F => Value / 12.92F,
            < 1.0F => MathF.Pow((Value + 0.055F) / 1.055F, 2.4F),
            _ => MathF.Pow(Value, 2.2F)
        };
    }

    private static void TransferOnDemand()
    {
        Span<byte> Datagram = stackalloc byte[sbyte.MaxValue];

        ReadOnlySpan<byte> QueueFalseDatagram = "/avatar/parameters/Q\0\0\0\0,F\0\0"u8;

        ChannelDatagrams Datagrams = new(stackalloc byte[32], stackalloc byte[32], stackalloc byte[32]);

        int PixelCountMinusOne = Pixels.Count - 1;

        while (true)
        {
            if (Server.Receive(Datagram, SocketFlags.None) != QueueFalseDatagram.Length)
            {
                continue;
            }
            if (Datagram[..QueueFalseDatagram.Length].SequenceEqual(QueueFalseDatagram) is false)
            {
                continue;
            }

            SendPixelColour(Datagrams, Pixels[PixelIndex & PixelCountMinusOne]);

            Interlocked.Increment(ref PixelIndex);

            AdvanceQueue();
        }
    }

    private static void SendPixelColour(ChannelDatagrams Channels, Vector3 Pixel)
    {
        for (int Channel = 0; Channel < ChannelDatagrams.Count; ++Channel)
        {
            Span<byte> Datagram = Channels[Channel];

            BinaryPrimitives.WriteSingleBigEndian(Datagram[^4..], Pixel[Channel]);

            Client.Send(Datagram, SocketFlags.None);
        }
    }

    private static void AdvanceQueue() => Client.Send("/avatar/parameters/Q\0\0\0\0,T\0\0"u8, SocketFlags.None);
}
