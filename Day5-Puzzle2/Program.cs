//var aInput = File.ReadAllLines("Input.txt");

string[] aInput = [
    "3-5",
    "10-14",
    "16-20",
    "12-18"];

List<Range> aRanges = [];
foreach (var aRangeString in aInput)
{
    var aParts = aRangeString.Split('-');
    ulong aFirst = ulong.Parse(aParts[0]);
    ulong aLast = ulong.Parse(aParts[1]);
    aRanges.Add(new Range(aFirst, aLast));
}

var aResultingRanges = new List<Range>();
for (var aOuterIndex = 0; aOuterIndex < aRanges.Count; aOuterIndex++)
{
    var aOuterRange = aRanges[aOuterIndex];

    if (aOuterRange.IsMerged)
        continue;

    aOuterRange.IsMerged = true;

    var aCurrentRange = new Range(aOuterRange);
    aCurrentRange.IsMerged = true;

    for (var aInnerIndex = 0; aInnerIndex < aRanges.Count; aInnerIndex++)
    {
        var aInnerRange = aRanges[aInnerIndex];
        if (aInnerRange.Equals(aCurrentRange))
            continue;

        var aRange = aCurrentRange.GetOverlap(aInnerRange);
        if (aRange == null)
            continue;

        aCurrentRange = aRange;
        aInnerRange.IsMerged = true;
    }
    aResultingRanges.Add(aCurrentRange);
}

foreach (var aRange in aResultingRanges)
{
    Console.WriteLine($"{aRange.FirstId} - {aRange.LastId}");
}



Console.WriteLine("[ENTER]");
Console.ReadLine();