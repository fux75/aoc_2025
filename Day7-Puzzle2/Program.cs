// daten lesen und "..." Zeilen entfernen
var aInput = File.ReadLines("Input.txt").ToList().Where(aLine => aLine.Any(s => s == 'S' || s == '^')).ToList();


// "Sicherheitsspalte" vorne und hinten anhängen
for (var aRowIndex = 0; aRowIndex < aInput.Count; aRowIndex++)
    aInput[aRowIndex] = $".{aInput[aRowIndex]}.";

// das Array enthält in jeder Zelle, wie oft sie beim Durchqueren eines Tachyons erreicht wird
ulong[,] aGrid = new ulong[aInput.Count, aInput[0].Length];

// Initialisierung mit dem ersten Tachyon
aGrid[0, aInput[0].IndexOf("S")] = 1;
// erste Zeile entfernen, macht die Zählung und Indizierung leichter
aInput.RemoveAt(0);

// Alle zeilen durchgehen
for (var aRowIndex = 0; aRowIndex < aInput.Count; aRowIndex++)
{
    // Ermitteln, an welchen Stellen sich Splitter befinden
    var aSplitterColumns = GetSplitterColumns(aInput[aRowIndex]);

    // Im Array für die aktuelle Zeile alle Spalten durchgehen
    for (var aColumnIndex = 0; aColumnIndex < aInput[aRowIndex].Length; aColumnIndex++)
    {
        // Steht an der Stelle schon ein Wert größer 0 (also wurde diese Zelle schon erreicht)?
        if (aGrid[aRowIndex, aColumnIndex] > 0)
        {
            if (aSplitterColumns.Contains(aColumnIndex))
            {
                // Die aktuelle Zelle ist ein Splitter -> neue Wege nach unten links und unten rechts erzeugen
                // Zellen unten links und rechts um den Wert der aktuellen Zelle erhöhen
                aGrid[aRowIndex + 1, aColumnIndex - 1] = aGrid[aRowIndex + 1, aColumnIndex - 1] + aGrid[aRowIndex, aColumnIndex];
                aGrid[aRowIndex + 1, aColumnIndex + 1] = aGrid[aRowIndex + 1, aColumnIndex + 1] + aGrid[aRowIndex, aColumnIndex];
            }
            else
            {
                // Kein Splitter -> Weg gerade nach unten fortsetzen
                // Zelle darunter um den Wert der aktuellen Zelle erhöhen
                aGrid[aRowIndex + 1, aColumnIndex] = aGrid[aRowIndex + 1, aColumnIndex] + aGrid[aRowIndex, aColumnIndex];
            }
        }
    }
}

// In der letzten Zeile alle Zellen aufsummieren
// Das wären alle möglichen Wege von oben nach unten
// In einem Baum wären das alle Wege von jedem Blatt zur Wurzel
ulong aTotalPaths = 0;
for (var aColumnIndex = 0; aColumnIndex < aGrid.GetLength(1); aColumnIndex++)
{
    aTotalPaths += aGrid[aGrid.GetLength(0) - 1, aColumnIndex];
}

Console.WriteLine($"Total Paths: {aTotalPaths}");
Console.WriteLine("[ENTER]");
Console.ReadLine();



List<int> GetSplitterColumns(string aRow)
{
    List<int> Result = new List<int>();
    for (var aIndex = 0; aIndex < aRow.Length; aIndex++)
    {
        if (aRow[aIndex] == '^')
        {
            Result.Add(aIndex);
        }
    }
    return Result;
}
