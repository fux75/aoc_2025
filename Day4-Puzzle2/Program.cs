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
var aCurrentlyRemovedRolls = 0;
var aTotalRemovedRolls = 0;
do
{
    var aAccessibleRolls = aPaperGrid.MarkAccessibles();
    //Console.Clear();
    //Console.WriteLine($"Accessible rolls: {aAccessibleRolls}");
    //PrintPaperGrid(aPaperGrid);
    //Console.ReadLine();
    aCurrentlyRemovedRolls = aPaperGrid.RemoveAccessibles();
    aTotalRemovedRolls += aCurrentlyRemovedRolls;
    //Console.Clear();
    //Console.WriteLine($"Removed rolls: {aCurrentlyRemovedRolls}");
    //PrintPaperGrid(aPaperGrid);
    //Console.ReadLine();
} while (aCurrentlyRemovedRolls != 0);

Console.WriteLine($"Total removed rolls: {aTotalRemovedRolls}");

Console.WriteLine("[ENTER]");
Console.ReadLine();

void PrintPaperGrid(PaperGrid aPaperGrid)
{
    for (var aRowIndex = 0; aRowIndex < aPaperGrid.RowCount; aRowIndex++)
    {
        for (var aColumnIndex = 0; aColumnIndex < aPaperGrid.ColumnCount; aColumnIndex++)
        {
            Console.Write(aPaperGrid[aColumnIndex, aRowIndex]);
        }
        Console.WriteLine();
    }
}