using System;

namespace Projection;

internal readonly ref struct ChannelDatagrams
{
    private readonly Span<byte> RDatagram;
    private readonly Span<byte> GDatagram;
    private readonly Span<byte> BDatagram;

    internal static int Count => 3;

    internal ChannelDatagrams(Span<byte> RDatagram, Span<byte> GDatagram, Span<byte> BDatagram)
    {
        "/avatar/parameters/R\0\0\0\0,f\0\0\0\0\0\0"u8.CopyTo(RDatagram);
        "/avatar/parameters/G\0\0\0\0,f\0\0\0\0\0\0"u8.CopyTo(GDatagram);
        "/avatar/parameters/B\0\0\0\0,f\0\0\0\0\0\0"u8.CopyTo(BDatagram);

        this.RDatagram = RDatagram;
        this.GDatagram = GDatagram;
        this.BDatagram = BDatagram;
    }

    internal Span<byte> this[int Index] => Index switch
    {
        0 => RDatagram,
        1 => GDatagram,
        2 => BDatagram,
        _ => throw new ArgumentOutOfRangeException(nameof(Index))
    };
}
