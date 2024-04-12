namespace AmazeMe.Lib;

public struct Position : IComparable<Position>
{
    public double X
    {
        get;
        set;
    }

    public double Y
    {
        get;
        set;
    }
    
    public Position(double x, double y)
    {
        X = x;
        Y = y;
    }

    public int CompareTo(Position otherPosition)
    {
        if (this.X == otherPosition.X && this.Y == otherPosition.Y)
        {
            return 0;
        }

        if (Y < otherPosition.Y)
        {
            return -1;
        }

        if (Y > otherPosition.Y)
        {
            return 1;
        }

        if (X < otherPosition.X)
        {
            return -1;
        }

        if (X > otherPosition.X)
        {
            return 1;
        }

        throw new Exception("How did this happen?");
    }
}