var aInput = File.ReadAllLines("input.txt").ToList();

//List<string> aInput = [
//    "..@@.@@@@.",
//    "@@@.@.@.@@",
//    "@@@@@.@.@@",
//    "@.@@@@..@.",
//    "@@.@@@@.@@",
//    ".@@@@@@@.@",
//    ".@.@.@.@@@",
//    "@.@@@.@@@@",
//    ".@@@@@@@@.",
//    "@.@.@@@.@."
//    ];


var aPaperGrid = new PaperGrid(aInput);

//for(var aRowIndex=0;aRowIndex < aPaperGrid.RowCount; aRowIndex++)
//{
//    for(var aColumnIndex=0;aColumnIndex < aPaperGrid.ColumnCount; aColumnIndex++)
//    {
//        Console.Write(aPaperGrid[aColumnIndex, aRowIndex]);
//    }
//    Console.WriteLine();
//}

//Console.WriteLine();
var aAccessibleCount = aPaperGrid.MarkAccessibles();
Console.WriteLine($"Accessible rolls: {aAccessibleCount}");
//Console.WriteLine();

//for (var aRowIndex = 0; aRowIndex < aPaperGrid.RowCount; aRowIndex++)
//{
//    for (var aColumnIndex = 0; aColumnIndex < aPaperGrid.ColumnCount; aColumnIndex++)
//    {
//        Console.Write(aPaperGrid[aColumnIndex, aRowIndex]);
//    }
//    Console.WriteLine();
//}

Console.WriteLine("[ENTER]");
Console.ReadLine();