var aInput = File.ReadAllLines("input.txt").ToList();

var aPaperGrid = new PaperGrid(aInput);
var aAccessibleRolls = aPaperGrid.MarkAccessibles();

Console.WriteLine($"Accessible rolls: {aAccessibleRolls}");
Console.WriteLine("[ENTER]");
Console.ReadLine();