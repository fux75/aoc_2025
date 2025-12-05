var aInput = File.ReadAllLines("input.txt");

var aRangeStrings = aInput.TakeWhile(s => !string.IsNullOrWhiteSpace(s));
var aIdStrings = aInput.SkipWhile(s => !string.IsNullOrWhiteSpace(s)).SkipWhile(s => string.IsNullOrWhiteSpace(s));

//string[] aRangeStrings =
//    ["3-5",
//     "10-14",
//     "16-20",
//     "12-18"];

//string[] aIdStrings =
//    ["1",
//     "5",
//     "8",
//     "11",
//     "17",
//     "32"];

List<(ulong FirstId, ulong LastId)> aRanges = [];
foreach(var aRangeString in aRangeStrings)
{
    var aParts = aRangeString.Split('-');
    ulong aFirst = ulong.Parse(aParts[0]);
    ulong aLast = ulong.Parse(aParts[1]);
    aRanges.Add((aFirst, aLast));
}

int aTotalFreshIngredients = 0;
foreach(var aIdString in aIdStrings)
{
    var aId = ulong.Parse(aIdString);
    if (aRanges.Any(s => s.FirstId <= aId && s.LastId >= aId))
    {
        aTotalFreshIngredients++;
    }
}

Console.WriteLine($"Total fresh ingredients: {aTotalFreshIngredients}");
Console.WriteLine("[ENTER]");
Console.ReadLine();