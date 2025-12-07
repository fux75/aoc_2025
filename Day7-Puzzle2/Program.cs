//var aInput = File.ReadLines("Input.txt").ToList().Where(aLine => aLine.Any(s => s == 'S' || s == '^')).ToList();
var aInput = File.ReadLines("Sample.txt").ToList().Where(aLine => aLine.Any(s => s == 'S' || s == '^')).ToList();


var aBeamColumns = new Dictionary<int, int>();
aBeamColumns.Add(aInput[0].IndexOf('S'), 1);
aInput.RemoveAt(0);
var aBeamColumnList = new List<Dictionary<int, int>>();

for (var aRowIndex = 0; aRowIndex < aInput.Count; aRowIndex++)
{
    var aRow = aInput[aRowIndex];
    var aSplitterColumns = GetSplitterColumns(aRow);
    Console.WriteLine($"Processing row {aRowIndex + 1} / {aInput.Count}, current paths: {aBeamColumns.Values.Sum()} - Splitter columns: {aSplitterColumns.Count}");

    var aColumns = new List<int>(aBeamColumns.Keys);
    //aBeamColumnList.Add(aColumns);

    foreach (var aBeamColumn in aColumns)
    {
        if (aSplitterColumns.Contains(aBeamColumn))
        {
            // Originalbeam entfernen
            aBeamColumns[aBeamColumn] = aBeamColumns[aBeamColumn] - 1;
            if(aBeamColumns[aBeamColumn] == 0)
                aBeamColumns.Remove(aBeamColumn);

            if (!aBeamColumns.ContainsKey(aBeamColumn - 1))
                aBeamColumns.Add(aBeamColumn - 1, 1);
            else
                aBeamColumns[aBeamColumn - 1] = aBeamColumns[aBeamColumn - 1] * 2;

            if (!aBeamColumns.ContainsKey(aBeamColumn + 1))
                aBeamColumns.Add(aBeamColumn + 1, 1);
            else
                aBeamColumns[aBeamColumn + 1] = aBeamColumns[aBeamColumn + 1] * 2;
        }
    }
}

//var aSum = 0;
//foreach (var aBeamColumnListEntry in aBeamColumnList)
//{
//    aSum += aBeamColumnListEntry.Values.Sum();
//}


Console.WriteLine($"Total paths: {aBeamColumns.Values.Sum()}");
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
