public class PuzzleLock
{
    public int CurrentPosition { get; set; }

    public int TurnLeft(int aCount)
    {
        var aCurrentPosition = CurrentPosition;
        aCurrentPosition -= aCount;
        aCurrentPosition = (aCurrentPosition + 100) % 100;

        CurrentPosition = aCurrentPosition;
        return CurrentPosition;
    }

    public int TurnRight(int aCount)
    {
        var aCurrentPosition = CurrentPosition;
        aCurrentPosition += aCount;
        aCurrentPosition %= 100;

        CurrentPosition = aCurrentPosition;
        return CurrentPosition;
    }

    public int Turn(string aCode)
    {
        var aDirection = aCode[0];
        var aTicksString = aCode.Replace("L", "").Replace("R", "");

        if (!int.TryParse(aTicksString, out var aTicks))
            throw new ArgumentException("Wrong direction");

        if (aDirection == 'L')
        {
            TurnLeft(aTicks);
        }
        else if (aDirection == 'R')
        {
            TurnRight(aTicks);
        }
        else
        {
            throw new ArgumentException("Wrong direction");
        }

        return CurrentPosition;
    }
}