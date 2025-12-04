public class PaperGrid
{
    private char[,] InternalGrid { get; set; } = new Char[0, 0];
    private int InternalColumnCount => InternalGrid.GetUpperBound(0) + 1;
    private int InternalRowCount => InternalGrid.GetUpperBound(1) + 1;

    public char this[int aColumn, int aRow]
    {
        get
        {
            return InternalGrid[aColumn + 1, aRow + 1];
        }
        set
        {
            InternalGrid[aColumn + 1, aRow + 1] = value;
        }
    }

    public int ColumnCount { get; private set; } = 0;
    public int RowCount { get; private set; } = 0;

    public PaperGrid(List<string> aInput) : base()
    {
        ColumnCount = aInput[0].Length;
        RowCount = aInput.Count;
        InternalGrid = new char[ColumnCount + 2, RowCount + 2];

        for (var aRowIndex = 0; aRowIndex < InternalRowCount; aRowIndex++)
        {
            for (var aColumnIndex = 0; aColumnIndex < InternalColumnCount; aColumnIndex++)
            {
                InternalGrid[aColumnIndex, aRowIndex] = '.';
            }
        }

        for (var aRowIndex = 0; aRowIndex < aInput.Count; aRowIndex++)
        {
            var aLine = aInput[aRowIndex];
            for (var aColumnIndex = 0; aColumnIndex < aLine.Length; aColumnIndex++)
            {
                this[aColumnIndex, aRowIndex] = aLine[aColumnIndex];
            }
        }
    }

    private int GetNeighbourCountFor(int aInternalColumnIndex, int aInternalRowIndex)
    {
        int Result = 0;
        for (var aRowIndex = aInternalRowIndex - 1; aRowIndex <= aInternalRowIndex + 1; aRowIndex++)
        {
            for (var aColumnIndex = aInternalColumnIndex - 1; aColumnIndex <= aInternalColumnIndex + 1; aColumnIndex++)
            {
                if (InternalGrid[aColumnIndex, aRowIndex] != '.')
                    Result++;
            }
        }
        return Result - 1;
    }

    public int MarkAccessibles()
    {
        var Result = 0;
        for (var aRowIndex = 1; aRowIndex < InternalRowCount - 1; aRowIndex++)
        {
            for (var aColumnIndex = 1; aColumnIndex < InternalColumnCount - 1; aColumnIndex++)
            {
                if (InternalGrid[aColumnIndex, aRowIndex] == '.')
                    continue;

                if (InternalGrid[aColumnIndex, aRowIndex] == 'X')
                    InternalGrid[aColumnIndex, aRowIndex] = '@';

                var aNeighbourCount = GetNeighbourCountFor(aColumnIndex, aRowIndex);
                if(aNeighbourCount < 4)
                {
                    InternalGrid[aColumnIndex, aRowIndex] = 'X';
                    Result++;
                }
            }
        }
        return Result;
    }

}
