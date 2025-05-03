using System;

namespace Ophura;

internal readonly ref struct ChannelDatagrams
{
    private readonly Span<byte> S0;
    private readonly Span<byte> S1;
    private readonly Span<byte> S2;

    internal int Count { get; init; }

    internal ChannelDatagrams(Span<byte> S0, Span<byte> S1, Span<byte> S2)
    {
        this.S0 = S0;
        this.S1 = S1;
        this.S2 = S2;
        Count = 3;
    }

    internal Span<byte> this[int Index] => Index switch
    {
        0 => S0,
        1 => S1,
        2 => S2,
        _ => throw new ArgumentOutOfRangeException(nameof(Index))
    };
}
