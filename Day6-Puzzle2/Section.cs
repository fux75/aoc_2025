using System.Diagnostics;

public class Section : List<string>
{
    public char Operator { get; set; } = '+';

    public ulong CalculateResult()
    {
        ulong Result = 0;
        for (var aIndex = 0; aIndex < this[0].Length; aIndex++)
        {
            var aColumnValues = new string(this.Select(line => line[aIndex]).ToArray());
            if(string.IsNullOrWhiteSpace(aColumnValues))
                continue;

            var aCurrentValue = ulong.Parse(aColumnValues!);
            if (aIndex == 0)
            {
                Result = aCurrentValue;
                continue;
            }

            switch (Operator)
            {
                case '+':
                    Result += aCurrentValue;
                    break;
                case '*':
                    Result *= aCurrentValue;
                    break;
            }
        }
        return Result;
    }

    internal void Parse(List<string> aList)
    {
        Clear();
        var aOperatorLine = aList[^1];
        Operator = aOperatorLine[0];
        var aSectionLength = Math.Min((aOperatorLine.SkipWhile(c => c != ' ').TakeWhile(c => c == ' ')).Count() + 1, aOperatorLine.Length);

        for (var i = 0; i < aList.Count; i++)
        {
            var aLine = aList[i];
            var aContent = aLine.Substring(0, aSectionLength);
            aList[i] = aLine.Substring(aSectionLength);
            if (!aContent.ContainsAny(['+', '*']))
                this.Add(aContent);
        }
    }
}
