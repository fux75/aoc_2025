var aInput = System.IO.File.ReadAllLines("Input.txt").ToList();

ulong aGrandTotal = 0;
Section aSection = new Section();

while (aInput[0].Length > 0)
{
    aSection.Parse(aInput);
    aGrandTotal += aSection.CalculateResult();
}

Console.WriteLine($"Grand total: {aGrandTotal}");
Console.WriteLine("[ENTER]");
Console.ReadLine();
