var aInput = File.ReadLines("input.txt");

ulong aSum = 0;
foreach(var aBank in aInput)
{
    aSum += GetLargestOutputJoltage(aBank);
}

Console.WriteLine($"Sum is: {aSum}");
Console.WriteLine("[ENTER]");
Console.ReadLine();

ulong GetLargestOutputJoltage(string aBank)
{
    var aIndex = -1;
    var aResultingString = "";
    for (var i = 0; i < 12; i++)
    {
        aIndex = GetLeftMostLargestDigitIndex(aBank, aIndex + 1, aBank.Length - 12 + i);
        aResultingString += aBank[aIndex];
    }
    return ulong.Parse(aResultingString);
}


int GetLeftMostLargestDigitIndex(string aString, int aInclusiveStartIndex, int aInclusiveEndIndex)
{
    if ((string.IsNullOrEmpty(aString)) || (aString.Length < 2))
        throw new ArgumentException("Invalid Bank");

    var aMaximum = -1;
    var aMaxIndex = -1;
    for (var aIndex = aInclusiveEndIndex; aIndex >= aInclusiveStartIndex; aIndex--)
    {
        var aDigit = aString[aIndex];

        if (aDigit >= aMaximum)
        {
            aMaximum = aDigit;
            aMaxIndex = aIndex;
        }
    }

    return aMaxIndex;
}
