var aInput = File.ReadAllLines("Input.txt");

////string[] aInput = [
////    "544317206779688-544317206779688",
////    "544317206779689-552355052009161"
////];

//string[] aInput = [
//    "3-5",
//    "10-14",
//    "16-20",
//    "12-18"];

List<Range> aRanges = [];
foreach (var aRangeString in aInput)
{
    var aParts = aRangeString.Split('-');
    ulong aFirst = ulong.Parse(aParts[0]);
    ulong aLast = ulong.Parse(aParts[1]);
    aRanges.Add(new Range(aFirst, aLast));
}

aRanges = aRanges.OrderBy(r => r.FirstId).ToList();

var aResultingRanges = new List<Range>();
var aCurrentIndex = 0;
while (aCurrentIndex < aRanges.Count - 1)
{
    var aCurrentRange = aRanges[aCurrentIndex];
    var aNextRange = aRanges[aCurrentIndex + 1];
    while (aNextRange.FirstId <= aCurrentRange.LastId + 1) 
    {
        aCurrentRange.LastId = Math.Max(aCurrentRange.LastId, aNextRange.LastId);
        aRanges.RemoveAt(aCurrentIndex + 1);
        if (aCurrentIndex == aRanges.Count - 1)
            break;
        aNextRange = aRanges[aCurrentIndex + 1];
    }
    aCurrentIndex++;
}

ulong aTotalFreshIngredients = 0;
foreach (var aRange in aRanges)
{
    aTotalFreshIngredients += aRange.LastId - aRange.FirstId + 1;
    Console.WriteLine($"{aRange.FirstId}-{aRange.LastId}-{aRange.LastId - aRange.FirstId + 1}");
}

Console.WriteLine($"{aRanges.Count} resulting ranges");
Console.WriteLine($"{aTotalFreshIngredients} total fresh ingredients");

Console.WriteLine("[ENTER]");
Console.ReadLine();