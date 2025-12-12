using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

var aInput = File.ReadAllLines("input.txt").TakeWhile(l => !string.IsNullOrWhiteSpace(l));
//var aInput = File.ReadAllLines("sample.txt").TakeWhile(l => !string.IsNullOrWhiteSpace(l));

var aTiles = aInput
    .Select(line => new Position(line))
    .OrderByDescending(p => p.X)
    .ThenBy(p => p.Y)
    .ToList();


// Alle möglichen Rechtecke berechnen
// Nach Fläche absteigend sortiert
var aRectangleInfos = CalculateAreas(aTiles);

// Suchstrategien definieren: links, oben, rechts, unten
var aStrategies = new List<Func<Position, Position, bool>>()
{
    (aPosition, p) => p.X < aPosition.X && p.Y == aPosition.Y,  // Strategie links
    (aPosition, p) => p.X == aPosition.X && p.Y < aPosition.Y,  // oben
    (aPosition, p) => p.X > aPosition.X && p.Y == aPosition.Y,  // rechts
    (aPosition, p) => p.X == aPosition.X && p.Y > aPosition.Y,  // unten
};

// Polygonpunkte
var aOutlinePoints = new List<Position>();
var aStartingPoint = aTiles[0];

Stopwatch sw = new Stopwatch();
sw.Start();


CreatePolygon(aStartingPoint, aTiles, aOutlinePoints, aStrategies);
var aEdges = CreateEdges(aOutlinePoints);
WriteDebugData(aOutlinePoints, "outline.txt");
WriteEdgeDebugData(aEdges, "edges.txt");


var aHorizontalPolygonEdges = aEdges.Where(e => e.From.Y == e.To.Y).ToList();
var aVerticalPolygonEdges = aEdges.Where(e => e.From.X == e.To.X).ToList();

int aRectanglesChecked = 0;

//var xy = IsPointInPolygon(new Position(40322, 49292), aHorizontalPolygonEdges, aVerticalPolygonEdges);


(Position From, Position To, ulong Area)? aRectangle = FindSolution(aRectangleInfos);

if (aRectangle is not null)
{
    Console.WriteLine($"Found rectangle from {aRectangle?.From} to {aRectangle?.To} with area {aRectangle?.Area}");
}
else
{
    Console.WriteLine("No rectangle found");
}


sw.Stop();
Console.WriteLine($"Ellapsed milliseconds: {sw.ElapsedMilliseconds}");

Console.WriteLine("[ENTER]");
Console.ReadLine();


(Position From, Position To, ulong Area)? FindSolution(List<(Position From, Position To, ulong Area)> aRectangles)
{
    foreach (var aRectangle in aRectangles)
    {
        aRectanglesChecked++;
        if (aRectanglesChecked % 10 == 0)
        {
            Console.Clear();
            Console.WriteLine($"Rectangles checked: {aRectanglesChecked} out of {aRectangleInfos.Count}");
        }
        if (IsRectangleInPolygon((aRectangle.From, aRectangle.To), aHorizontalPolygonEdges, aVerticalPolygonEdges))
        {
            return aRectangle;
        }
    }
    return null;
}

List<(Position From, Position To)> CreateEdges(List<Position> aOuterPoints)
{
    var Result = new List<(Position From, Position To)>();
    for (var aIndex = 0; aIndex < aOuterPoints.Count; aIndex++)
    {
        var aFrom = aOuterPoints[aIndex];
        var aTo = aOuterPoints[(aIndex + 1) % aOuterPoints.Count];
        Result.Add((aFrom, aTo));
    }
    return Result;
}

bool CreatePolygon(Position aCurrentPoint, List<Position> aTiles, List<Position> aOutline, List<Func<Position, Position, bool>> aComparisions)
{
    // Enthält die Lösungsmenge den aktuellen Punkt ist der Algorithmus fertig
    if (aOutline.Contains(aCurrentPoint))
        return true;

    // Aktuellen Punkt zur Lösungsmenge hinzufügen
    aOutline.Add(aCurrentPoint);

    // Nachbarn in der angegebenen Reihenfolge suchen
    foreach (var aComparision in aComparisions)
    {
        var aNeighbour = GetNeighbour(aCurrentPoint, aTiles, aComparision);
        // Wenn Nachbar gefunden, und noch nicht in der Lösungsmenge, dann rekursiv weitersuchen
        if ((aNeighbour is not null) && (!aOutline.Contains(aNeighbour)))
        {
            CreatePolygon(aNeighbour, aTiles, aOutline, aComparisions);
            break;
        }
    }

    // Wenn der letzte Punkt Nachbar des Startpunktes ist, dann fertig
    if ((aOutline[^1].X == aOutline[0].X || aOutline[^1].Y == aOutline[0].Y))
        return true;


    return false;
}

Position? GetNeighbour(Position aPoint, List<Position> aTiles, Func<Position, Position, bool> aComparision)
{
    return aTiles
        .Where(p => aComparision(aPoint, p))
        .OrderBy(p => p.X)
        .FirstOrDefault();
}

List<(Position From, Position To, ulong Area)> CalculateAreas(List<Position> aTiles)
{
    var aResult = new List<(Position From, Position To, ulong Area)>();
    for (var aColumnIndex = 0; aColumnIndex < aTiles.Count; aColumnIndex++)
    {
        for (var aRowIndex = aColumnIndex + 1; aRowIndex < aTiles.Count; aRowIndex++)
        {
            var aPositionA = aTiles[aColumnIndex];
            var aPositionB = aTiles[aRowIndex];
            ulong aArea = (ulong)(Math.Abs(aPositionA.X - aPositionB.X) + 1) * (ulong)(Math.Abs(aPositionA.Y - aPositionB.Y) + 1);
            aResult.Add((aPositionA, aPositionB, aArea));
        }
    }
    return aResult.OrderByDescending(t => t.Area).ToList();
}

bool IsRectangleInPolygon((Position From, Position To) value,
    List<(Position From, Position To)> aHorizontalPolygonEdges,
    List<(Position From, Position To)> aVerticalPolygonEdges)
{
    var aVertexes = new List<Position>() {
        new Position(Math.Min(value.From.X, value.To.X), Math.Min(value.From.Y, value.To.Y)),   // links oben
        new Position(Math.Max(value.From.X, value.To.X), Math.Min(value.From.Y, value.To.Y)),   // rechts oben
        new Position(Math.Max(value.From.X, value.To.X), Math.Max(value.From.Y, value.To.Y)),   // rechts unten
        new Position(Math.Min(value.From.X, value.To.X), Math.Max(value.From.Y, value.To.Y))};  // links unten

    // Die Punkte aller Kanten der Rechtecks (inkl. der 4 Ecken) müssen im Polygon liegen
    // Oberkante und Unterkante
    for (var aScan = 1000; aScan >= 1; aScan /= 2)
    {
        for (var x = aVertexes[0].X; x <= aVertexes[1].X; x += aScan)
        {
            if (!IsPointInPolygon(new Position(x, aVertexes[0].Y), aHorizontalPolygonEdges, aVerticalPolygonEdges))
                return false;

            if (!IsPointInPolygon(new Position(x, aVertexes[2].Y), aHorizontalPolygonEdges, aVerticalPolygonEdges))
                return false;
        }
    }

    // Linke und rechte Kante
    for (var aScan = 1000; aScan >= 1; aScan /= 2)
    {
        for (var y = aVertexes[0].Y; y <= aVertexes[2].Y; y += aScan)
        {
            if (!IsPointInPolygon(new Position(aVertexes[0].X, y), aHorizontalPolygonEdges, aVerticalPolygonEdges))
                return false;

            if (!IsPointInPolygon(new Position(aVertexes[2].X, y), aHorizontalPolygonEdges, aVerticalPolygonEdges))
                return false;
        }
    }

    return true;
}


bool IsPointInPolygon(Position aPoint,
    List<(Position From, Position To)> HorizontalEdges,
    List<(Position From, Position To)> VerticalEdges
    )
{
    // Ist er auf einer Kante?
    foreach (var aHorizontalEdge in HorizontalEdges)
        if (IsPointOnHorizontalEdge(aPoint, aHorizontalEdge))
            return true;

    foreach (var aVerticalEdge in VerticalEdges)
        if (IsPointOnVerticalEdge(aPoint, aVerticalEdge))
            return true;


    // Jordan Test: https://de.wikipedia.org/wiki/Punkt-in-Polygon-Test_nach_Jordan
    // Zählen der Schnittpunkte 
    // Wir gehen vom Punkt nach rechts bis zum größten x-wert in den vertikalen Kanten
    int aIntersections = 0;
    foreach (var aVerticalEdge in VerticalEdges)
    {
        // Nur Kanten rechts vom Punkt betrachten
        if (aPoint.X >= aVerticalEdge.From.X)
            continue;
        // Prüfen ob die Gerade den Punkt schneidet
        if (Math.Min(aVerticalEdge.From.Y, aVerticalEdge.To.Y) <= aPoint.Y && aPoint.Y <= Math.Max(aVerticalEdge.From.Y, aVerticalEdge.To.Y))
            aIntersections++;
    }

    // Ist die Anzahl der Schnittpunkte ungerade, ist der Punkt im Polygon 
    return (aIntersections % 2) == 1;
}

bool IsPointOnVerticalEdge(Position aPoint, (Position From, Position To) aEdge)
{
    return (aEdge.From.X == aPoint.X) && (Math.Min(aEdge.From.Y, aEdge.To.Y) <= aPoint.Y && aPoint.Y <= Math.Max(aEdge.From.Y, aEdge.To.Y));
}
bool IsPointOnHorizontalEdge(Position aPoint, (Position From, Position To) aEdge)
{
    return (aEdge.From.Y == aPoint.Y) && (Math.Min(aEdge.From.X, aEdge.To.X) <= aPoint.X && aPoint.X <= Math.Max(aEdge.From.X, aEdge.To.X));
}


void WriteEdgeDebugData(List<(Position From, Position To)> aEdges, string aFileName)
{
    var aDebug = "";
    foreach (var aEdge in aEdges)
    {
        if (!string.IsNullOrEmpty(aDebug))
            aDebug += ",";

        aDebug += $"({aEdge.From.X},{aEdge.From.Y})";
    }
    aDebug = $"Vieleck({aDebug})";
    File.WriteAllText(aFileName, aDebug);
}


void WriteDebugData(List<Position> aInvalidPoints, string aFileName)
{
    var aDebug = new List<string>();
    aDebug.Add($"{{");

    foreach (var aPoint in aInvalidPoints)
    {
        aDebug.Add($"({aPoint.X},{aPoint.Y}),");
    }

    aDebug[^1] = aDebug[^1].TrimEnd(',');
    aDebug.Add($"}}");

    File.WriteAllLines(aFileName, aDebug);
}




record Position(int X, int Y)
{
    public Position(string aString) : this(0, 0)
    {
        var aParts = aString.Split(",");
        X = int.Parse(aParts[0]);
        Y = int.Parse(aParts[1]);
    }
}