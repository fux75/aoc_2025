var aLock = new PuzzleLock();
aLock.CurrentPosition = 50;
var aZeros = 0;

var aKeyCodes = File.ReadAllLines("Codes.txt");

foreach (var aKeyCode in aKeyCodes)
{
    aZeros += aLock.Turn(aKeyCode);
}

Console.WriteLine($"The code is: {aZeros}");
Console.WriteLine("[ENTER]");
Console.ReadLine();


