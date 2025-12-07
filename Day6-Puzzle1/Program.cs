var aRawInput = File.ReadLines("input.txt");
var aInput = new List<List<string>>();
foreach (var aLine in aRawInput)
{
    var aCurrentContent = aLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    aInput.Add(aCurrentContent.ToList());
}
var aOperators = aInput[^1];


ulong aGradTotal = 0;
for (var aColumnIndex = 0; aColumnIndex < aOperators.Count; aColumnIndex++)
{
    var aOperator = aOperators[aColumnIndex];
    ulong aResult = ulong.Parse(aInput[0][aColumnIndex]);

    for (var aRowIndex = 1; aRowIndex < aInput.Count - 1; aRowIndex++)
    {
        var aValue = ulong.Parse(aInput[aRowIndex][aColumnIndex]);
        switch (aOperator)
        {
            case "+":
                aResult += aValue;
                break;
            case "*":
                aResult *= aValue;
                break;
        }
    }
    aGradTotal += aResult;
}

Console.WriteLine($"Grand total: {aGradTotal}");
Console.WriteLine("[ENTER]");
Console.ReadLine();
