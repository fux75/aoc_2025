using Day9_Puzzle2;
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
var aResult = FindOutlinePoints(aStartingPoint, aTiles, aOutlinePoints, aStrategies);
var aEdges = CreateConnections(aOutlinePoints);

var aHorizontalPolygonEdges = aEdges.Where(e => e.From.Y == e.To.Y).ToList();
var aVerticalPolygonEdges = aEdges.Where(e => e.From.X == e.To.X).ToList();


int aRectanglesChecked = 0;
(Position From, Position To, ulong Area)? aRectangle = FindSolution(aRectangleInfos);


Console.WriteLine();
if (aRectangle is not null)
    Console.WriteLine($"Largest rectangle in polygon from {aRectangle.Value.From.X},{aRectangle.Value.From.Y} to {aRectangle.Value.To.X},{aRectangle.Value.To.Y} with area {aRectangle.Value.Area:0}");
else
    Console.WriteLine("No rectangle found in polygon.");

sw.Stop();
Console.WriteLine($"Ellapsed milliseconds: {sw.ElapsedMilliseconds}");

Console.WriteLine("[ENTER]");
Console.ReadLine();

(Position From, Position To, ulong Area)? FindSolution(List<(Position From, Position To, ulong Area)> aRectangles)
{
    foreach (var aRectangle in aRectangles)
    {
        aRectanglesChecked++;
        if (aRectanglesChecked % 100 == 0)
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

//(Position From, Position To, ulong Area)? FindSolution(List<(Position From, Position To, ulong Area)> aRectangles)
//{
//    // Wenns nichts mehr gibt, dann Fehlschlage -> Ende
//    if (aRectangles.Count == 0)
//        return null;

//    if (aRectangles.Count == 1)
//    {
//        aRectanglesChecked++;
//        if (aRectanglesChecked % 100 == 0)
//        {
//            Console.Clear();
//            Console.WriteLine($"Rectangles checked: {aRectanglesChecked} out of {aRectangleInfos.Count}");
//        }

//        var aSingleRectangle = aRectangles[0];
//        if (IsRectangleInPolygon((aSingleRectangle.From, aSingleRectangle.To), aHorizontalPolygonEdges, aVerticalPolygonEdges))
//        {
//            // aAreaInfo = (aSingleRectangle.From, aSingleRectangle.To, aSingleRectangle.Area);
//            return aSingleRectangle;
//        }
//        else return null;
//    }

//    // Linke Hälfte (mit den größeren Flächen) durchsuchen
//    var aMiddleIndex = aRectangles.Count / 2;
//    var aLeftHalf = aRectangles.Take(aMiddleIndex).ToList();
//    var aResult = FindSolution(aLeftHalf);
//    if (aResult is not null)
//        return aResult;

//    // Rechte Hälfte (Kleinere Flächen) durchsuchen
//    var aRightHalf = aRectangles.Skip(aMiddleIndex).ToList();
//    aResult = FindSolution(aLeftHalf);
//    if (aResult is not null)
//        return aResult;

//    return null;
//}


List<(Position From, Position To)> CreateConnections(List<Position> aOuterPoints)
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
foreach (var aPosition in aOutlinePoints)
{
    Console.WriteLine($"Outline Point: {aPosition.X},{aPosition.Y}");
}
bool FindOutlinePoints(Position aCurrentPoint, List<Position> aTiles, List<Position> aOutline, List<Func<Position, Position, bool>> aComparisions)
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
            FindOutlinePoints(aNeighbour, aTiles, aOutline, aComparisions);
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


    //// Alle 4 Ecken müssen im Polygon liegen
    //foreach (var aVertex in aVertexes)
    //{
    //    if (!IsPointInPolygon(aVertex, aHorizontalPolygonEdges, aVerticalPolygonEdges))
    //        return false;
    //}

    // Die Punkte aller Kanten der Rechtecks (inkl. der 4 Ecken) müssen im Polygon liegen
    // Oberkante und Unterkante
    for (var x = aVertexes[0].X; x <= aVertexes[1].X; x++)
    {
        if (!IsPointInPolygon(new Position(x, aVertexes[0].Y), aHorizontalPolygonEdges, aVerticalPolygonEdges))
            return false;

        if (!IsPointInPolygon(new Position(x, aVertexes[2].Y), aHorizontalPolygonEdges, aVerticalPolygonEdges))
            return false;
    }

    //// Linke und rechte Kante
    for (var y = aVertexes[0].Y; y <= aVertexes[2].Y; y++)
    {
        if (!IsPointInPolygon(new Position(aVertexes[0].X, y), aHorizontalPolygonEdges, aVerticalPolygonEdges))
            return false;

        if (!IsPointInPolygon(new Position(aVertexes[2].X, y), aHorizontalPolygonEdges, aVerticalPolygonEdges))
            return false;
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
        if (aVerticalEdge.From.Y <= aPoint.Y && aPoint.Y <= aVerticalEdge.To.Y)
            aIntersections++;
    }

    // Ist die Anzahl der Schnittpunkte ungerade, ist der Punkt im Polygon 
    return (aIntersections % 2) == 1;
}

bool IsPointOnVerticalEdge(Position aPoint, (Position From, Position To) aEdge)
{
    return (aEdge.From.X == aPoint.X) && (aEdge.From.Y <= aPoint.Y && aPoint.Y <= aEdge.To.Y);
}
bool IsPointOnHorizontalEdge(Position aPoint, (Position From, Position To) aEdge)
{
    return (aEdge.From.Y == aPoint.Y) && (aEdge.From.X <= aPoint.X && aPoint.X <= aEdge.To.X);
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