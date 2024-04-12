namespace AmazeMe.Lib;

public class Settings
{
    public int Width
    {
        get;
        private set;
    }

    public int Height
    {
        get;
        private set;
    }

    public int PolygonSides
    {
        get;
        private set;
    }

    public void SaveSetting(string[] args)
    {
        if (args.Length != 4)
        {
            throw new ArgumentOutOfRangeException(nameof(args));
        }

        bool isWidthANumber = int.TryParse(args[1], out int width);
        bool isHeightANumber = int.TryParse(args[2], out int height);
        bool isPolygonSidesANumber = int.TryParse(args[3], out int polygonSides);

        if (isWidthANumber && width > 2)
        {
            Width = width;
        }
        else
        {
            throw new ArgumentException(nameof(width));
        }

        if (isHeightANumber && height > 2)
        {
            Height = height;
        }
        else
        {
            throw new ArgumentException(nameof(height));
        }

        if (isPolygonSidesANumber)
        {
            PolygonSides = polygonSides;
        }
        else
        {
            throw new ArgumentException(nameof(polygonSides));
        }
    }
}
