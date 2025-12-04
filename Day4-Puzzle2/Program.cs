var aInput = File.ReadAllLines("input.txt").ToList();

var aPaperGrid = new PaperGrid(aInput);
var aCurrentlyRemovedRolls = 0;
var aTotalRemovedRolls = 0;
do
{
    aPaperGrid.MarkAccessibles();
    aCurrentlyRemovedRolls = aPaperGrid.RemoveAccessibles();
    aTotalRemovedRolls += aCurrentlyRemovedRolls;
} while (aCurrentlyRemovedRolls != 0);

Console.WriteLine($"Total removed rolls: {aTotalRemovedRolls}");

Console.WriteLine("[ENTER]");
Console.ReadLine();