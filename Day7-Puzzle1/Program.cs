var aInput = File.ReadLines("Input.txt").ToList().Where(aLine => aLine.Any(s => s == 'S' || s == '^')).ToList();
// var aInput = File.ReadLines("Sample.txt").ToList().Where(aLine => aLine.Any(s => s == 'S' || s == '^')).ToList();


var aBeamColumns = new List<int>();
aBeamColumns.Add(aInput[0].IndexOf('S'));
aInput.RemoveAt(0);

ulong aTotalCollissions = 0;
foreach (var aRow in aInput)
{
    var aSplitterColumns = GetSplitterColumns(aRow);

    foreach(var aColumn in aSplitterColumns)
    {
        // wenn splitter in BeamColumns ist, dann
        if (aBeamColumns.Contains(aColumn))
        {
            // Kollissionen hochzählen
            aTotalCollissions++;

            // Originalsplitter entfernen
            aBeamColumns.Remove(aColumn);

            // aufteilen in zwei beams
            // neuen beam nur hinzufügen, wenn er nicht schon existiert
            if (!aBeamColumns.Contains(aColumn-1))
                aBeamColumns.Add(aColumn-1);
            if (!aBeamColumns.Contains(aColumn + 1))
                aBeamColumns.Add(aColumn+1);
        }
    }

}


Console.WriteLine($"Total splits: {aTotalCollissions}");
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
