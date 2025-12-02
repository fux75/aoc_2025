public class PuzzleLock
{
    public int CurrentPosition { get; set; }

    public int TurnLeft(int aCount)
    {
        var aEffectiveCount = aCount % 100;
        var aPassedZeros = aCount / 100;

        var aCurrentPosition = CurrentPosition;
        aCurrentPosition -= aEffectiveCount;
        if ((aCurrentPosition <= 0) && (CurrentPosition != 0))
            aPassedZeros++;

        aCurrentPosition = (aCurrentPosition + 100) % 100;

        CurrentPosition = aCurrentPosition;
        return aPassedZeros;
    }

    public int TurnRight(int aCount)
    {
        var aEffectiveCount = aCount % 100;
        var aPassedZeros = (aCount / 100);

        var aCurrentPosition = CurrentPosition;
        aCurrentPosition += aEffectiveCount;
        if ((aCurrentPosition >= 100) && (CurrentPosition != 0))
            aPassedZeros++;

        aCurrentPosition %= 100;

        CurrentPosition = aCurrentPosition;
        return aPassedZeros;
    }

    public int Turn(string aCode)
    {
        var aDirection = aCode[0];
        var aTicksString = aCode.Substring(1, (aCode.Length - 1));

        if (!int.TryParse(aTicksString, out var aTicks))
            throw new ArgumentException("Wrong direction");

        if (aDirection == 'L')
        {
            return TurnLeft(aTicks);
        }
        else if (aDirection == 'R')
        {
            return TurnRight(aTicks);
        }
        else
        {
            throw new ArgumentException("Wrong direction");
        }
    }
}

