using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

var aInput = File.ReadAllText("ProductIds.txt");
var aRegEx = new Regex(@"^(\d+)\1+$");

var aParts = aInput.Split(',');

ulong aSum = 0;
foreach (var aRange in aParts)
{
    var aFirstLast = aRange.Split('-');

    ulong aFirstId = ulong.Parse(aFirstLast[0]);
    ulong aLastId = ulong.Parse(aFirstLast[1]);

    for (var aCheckId = aFirstId; aCheckId <= aLastId; aCheckId++)
    {
        var aCheckString = aCheckId.ToString();
        if (aRegEx.IsMatch(aCheckString))
            aSum += aCheckId;
    }
}

Console.WriteLine($"Sum: {aSum}");
Console.WriteLine("[ENTER]");
Console.ReadLine();
