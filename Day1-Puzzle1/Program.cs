var aLock = new PuzzleLock();
aLock.CurrentPosition = 50;
var aZeros = 0;

var aKeyCodes = File.ReadAllLines("Codes.txt");

foreach (var aKeyCode in aKeyCodes)
{
    if (aLock.Turn(aKeyCode) == 0)
        aZeros++;
}

Console.WriteLine($"The code is: {aZeros}");
Console.WriteLine("[ENTER]");
Console.ReadLine();

