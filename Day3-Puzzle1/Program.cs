var aInput = File.ReadAllLines("input.txt");

//string[] aInput = ["987654321111111",
//                   "811111111111119",
//                   "234234234234278",
//                   "818181911112111"];
//string[] aInput = ["9881797 "];

var aSum = 0;
foreach (var aBank in aInput)
{
    aSum += GetLargestJolts(aBank);
}

Console.WriteLine($"Sum is: {aSum}");
Console.WriteLine("[ENTER]");
Console.ReadLine();


int GetLargestJolts(string aBank)
{
    var aFirstIndex = GetLeftMostLargestDigitIndex(aBank, 0, aBank.Length - 2);
    var aFirstDigit = aBank[aFirstIndex];

    var aSecondIndex = GetLeftMostLargestDigitIndex(aBank, aFirstIndex + 1, aBank.Length - 1);
    var aSecondDigit = aBank[aSecondIndex];

    int aNumber = (aFirstDigit - 48) * 10 + (aSecondDigit - 48);

    return aNumber;
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
