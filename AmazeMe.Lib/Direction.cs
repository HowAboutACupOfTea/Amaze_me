namespace AmazeMe.Lib;

public record Direction
{
    public static readonly Direction NoDirection = new(0, 0);
    public static readonly Direction North = new(0, -1);
    public static readonly Direction East = new(1, 0);
    public static readonly Direction South = new(0, 1);
    public static readonly Direction West = new(-1, 0);
    private int _x;
    private int _y;

    public int X
    {
        get
        {
            return _x;
        }

        set
        {
            if (value < -1 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "The given value must be between -1 and 1 (both included).");
            }

            _x = value;
        }
    }

    public int Y
    {
        get
        {
            return _y;
        }

        set
        {
            if (value < -1 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "The given value must be between -1 and 1 (both included).");
            }

            _y = value;
        }
    }

    public Direction(int x, int y)
    {
        X = x;
        Y = y;
    }
}
