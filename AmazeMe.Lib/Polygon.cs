namespace AmazeMe.Lib;

public abstract class Polygon
{
    private int _numberOfSides;

    /// <summary>
    /// Gets the open sides of the polygon.
    /// </summary>
    /// <value>The directions in which the polygon is open.</value>
    public List<Direction> OpenSides
    {
        get;
        private set;
    }

    public int NumberOfSides
    {
        get
        {
            return _numberOfSides;
        }

        private set
        {
            if (value <= 2)
            {
                throw new ArgumentOutOfRangeException(nameof(NumberOfSides), "The given value must not be smaller than 2.");
            }

            _numberOfSides = value;
        }
    }

    public Polygon(int numberOfPolygonSides)
    {
        OpenSides = new();
        NumberOfSides = numberOfPolygonSides;
    }
}
