using System.Diagnostics;

var aInput = File.ReadAllLines("input.txt").TakeWhile(l => !string.IsNullOrWhiteSpace(l));
//var aInput = File.ReadAllLines("sample.txt").TakeWhile(l => !string.IsNullOrWhiteSpace(l));


var aBoxes = CreateBoxes(aInput);
var aDistances = CalculateDistances(aBoxes);
var aCircuts = CreateCircuts(aBoxes);

Stopwatch aStopwatch = Stopwatch.StartNew();
float aMinDistance = 0;
ulong aProduct = 0;

// Solange es mehr als einen riesengroßen Circut gibt
while (aCircuts.Count > 1)
{
    // Kleinste Distanz herausfinden
    aMinDistance = aDistances.Min(kvp => kvp.Value);
    var aBoxPair = aDistances.First(kvp => kvp.Value == aMinDistance).Key;
    
    // Diese Kombination entfernen
    aDistances.Remove(aBoxPair);
    
    // Falls es die letzten sind, die angefasst werden, bevor die Schleife abbricht
    // Abstand merken
    aProduct = (ulong)aBoxPair.Box1.x * (ulong)aBoxPair.Box2.x;

    // Alle Circuts finden, die eine der beiden Boxen enthalten
    // und diese Circuts zusammenführen
    var aAffectedCircuts = aCircuts.Where(aCircuts => aCircuts.Contains(aBoxPair.Box1) || aCircuts.Contains(aBoxPair.Box2)).ToList();

    // Alle Circuts der betroffenen Boxen aus den Circuts entfernen
    aCircuts.RemoveAll(c => aAffectedCircuts.Contains(c));

    // Einen neuen Circut erstellen, der alle Boxen der betroffenen Circuts enthält
    var aNewCircut = new List<Box>();
    foreach (var aCircut in aAffectedCircuts)
        aNewCircut.AddRange(aCircut);

    // Den neuen Circut zu den Circuts hinzufügen
    aCircuts.Add(aNewCircut);
}

aStopwatch.Stop();
Console.WriteLine($"Milliseconds elapsed: {aStopwatch.ElapsedMilliseconds}");
Console.WriteLine($"Result: {aProduct}");
Console.WriteLine("[ENTER]");
Console.ReadLine();

Dictionary<(Box Box1, Box Box2), float> CalculateDistances(List<Box> aBoxes)
{
    var Result = new Dictionary<(Box Box1, Box Box2), float>();
    for (var aColumnIndex = 0; aColumnIndex < aBoxes.Count; aColumnIndex++)
    {
        for (var aRowIndex = aColumnIndex + 1; aRowIndex < aBoxes.Count; aRowIndex++)
        {
            var aBox1 = aBoxes[aColumnIndex];
            var aBox2 = aBoxes[aRowIndex];
            var aDistance = MathF.Sqrt(
                MathF.Pow(aBox1.x - aBox2.x, 2) +
                MathF.Pow(aBox1.y - aBox2.y, 2) +
                MathF.Pow(aBox1.z - aBox2.z, 2));
            Result.Add((aBox1, aBox2), aDistance);
        }
    }
    return Result;
}

List<Box> CreateBoxes(IEnumerable<string> aInput)
{
    var Result = new List<Box>();
    foreach (var aLine in aInput)
    {
        var aBox = new Box(aLine);
        Result.Add(aBox);
    }
    return Result;
}


List<List<Box>> CreateCircuts(List<Box> aBoxes)
{
    var Result = new List<List<Box>>();
    foreach (var aBox in aBoxes)
    {
        var aCircute = new List<Box> { aBox };
        Result.Add(aCircute);
    }
    return Result;
}


record Box(int x, int y, int z)
{
    public Box(string aString) : this(0, 0, 0)
    {
        var aParts = aString.Split(',');
        x = int.Parse(aParts[0]);
        y = int.Parse(aParts[1]);
        z = int.Parse(aParts[2]);
    }
}

