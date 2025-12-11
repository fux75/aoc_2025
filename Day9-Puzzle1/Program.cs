var aInput = File.ReadAllLines("input.txt").TakeWhile(l => !string.IsNullOrWhiteSpace(l));
//var aInput = File.ReadAllLines("sample.txt").TakeWhile(l => !string.IsNullOrWhiteSpace(l));

var aTiles = aInput
    .Select(line => new Position(line))
    .OrderBy(p => p.x)
    .ThenBy(p => p.y)
    .ToList();


var aDistances = new Dictionary<(Position aTile1, Position Tile2), long>();

for (var aColumnIndex = 0; aColumnIndex < aTiles.Count; aColumnIndex++)
{
    for (var aRowIndex = 0; aRowIndex < aTiles.Count; aRowIndex++)
    {
        var aPositionA = aTiles[aColumnIndex];
        var aPositionB = aTiles[aRowIndex];
        long aArea = (long)(aPositionA.x - aPositionB.x + 1) * (long)(aPositionA.y - aPositionB.y + 1);
        if (aArea < 0) aArea = -aArea;
        aDistances.Add((aPositionA, aPositionB), aArea);
    }
}

var aMaxArea = aDistances.Values.Max();

Console.WriteLine($"Maximum area: {aMaxArea}");
Console.WriteLine("[ENTER]");
Console.ReadLine();




record Position(int x, int y)
{
    public Position(string aString) : this(0, 0)
    {
        var aParts = aString.Split(",");
        x = int.Parse(aParts[0]);
        y = int.Parse(aParts[1]);
    }
}