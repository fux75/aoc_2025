using System.Diagnostics.CodeAnalysis;

public class Range 
{
    public ulong FirstId { get; set; }
    public ulong LastId { get; set; }

    public bool IsMerged { get; set; } = false;

    public Range? GetOverlap(Range aRange)
    {
        Range? Result = null;

        if (aRange.LastId < FirstId - 1)
            return Result;
        if (aRange.FirstId > LastId + 1)
            return Result;

        return new Range(
            Math.Min(FirstId, aRange.FirstId),
            Math.Max(LastId, aRange.LastId));
    }

    public override string ToString() => $"{FirstId}-{LastId} {IsMerged}";

    public bool Equals(Range aRange)
    {
        return FirstId == aRange.FirstId && LastId == aRange.LastId;
    }

    public Range() : base() { }

    public Range(ulong aFirstId, ulong aLastId) : this()
    {
        FirstId = aFirstId;
        LastId = aLastId;
    }

    public Range(Range aRange) : this(aRange.FirstId, aRange.LastId) { }

}
