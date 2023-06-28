namespace StgSharp.Geometries
{
    public delegate float ExpressionHandler
        (
        Counter<uint> tickCounter,
        float beginRange,
        float endRange
        );
}
