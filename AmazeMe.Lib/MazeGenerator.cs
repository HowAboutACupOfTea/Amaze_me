namespace AmazeMe.Lib;

public class MazeGenerator
{
    private readonly Random _random = new();

    public Settings Settings
    {
        get;
        private set;
    }

    public MazeGenerator()
    {
        Settings = new Settings();
    }

    public SortedList<Position, Polygon> Generate(int width, int height, int polygonSides)
    {
        if (width <= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(width), $"The given value must be bigger than one.");
        }

        if (height <= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(height), $"The given value must be bigger than one.");
        }

        return polygonSides switch
        {
            4 => GenerateWithRectangles(width, height),
            6 => GenerateWithHexagons(width, height),
            _ => throw new NotImplementedException($"Drawing a polygon with {polygonSides} is currently not supported."),
        };
    }

    private SortedList<Position, Polygon> GenerateWithHexagons(int width, int height)
    {
        if (width <= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(width), $"The given value must be bigger than one.");
        }

        if (height <= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(height), $"The given value must be bigger than one.");
        }
        
        List<Direction> possibleDirections = new()
        {
            new Direction(0, -1),
            new Direction(1, -1),
            new Direction(1, 0),
            new Direction(0, 1),
            new Direction(-1, 1),
            new Direction(-1, 0)
        };

        var polygons = GenerateHexagons(width, height);
        return ConvertToMaze(polygons, possibleDirections);
    }

    private SortedList<Position, Polygon> GenerateWithRectangles(int width, int height)
    {
        if (width <= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(width), $"The given value must be bigger than one.");
        }

        if (height <= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(height), $"The given value must be bigger than one.");
        }

        List<Direction> possibleDirections = new()
        {
            Direction.North,
            Direction.East,
            Direction.South,
            Direction.West
        };

        var polygons = GenerateRectangles(width, height);
        return ConvertToMaze(polygons, possibleDirections);
    }

    private SortedList<Position, Polygon> ConvertToMaze(SortedList<Position, Polygon> polygons, List<Direction> possibleDirections)
    {
        SortedList<Position, Polygon> maze = new();
        KeyValuePair<Position, Polygon> tempPolygon = SelectRandomPair(polygons, maze);
        maze.Add(tempPolygon.Key, tempPolygon.Value);

        while (maze.Count < polygons.Count)
        {
            Dictionary<Position, Direction> currentWalk = DoRandomWalk(polygons, maze, possibleDirections);
            AddWalkToMaze(currentWalk, polygons, maze);
        }
        
        return maze;
    }

    private static void AddWalkToMaze(Dictionary<Position, Direction> walk, SortedList<Position, Polygon> polygons, SortedList<Position, Polygon> maze)
    {
        KeyValuePair<Position, Direction> pair = walk.First();

        while (pair.Key.CompareTo(walk.Last().Key) != 0)
        {
            if (!maze.ContainsKey(pair.Key))
            {
                maze.Add(pair.Key, polygons[pair.Key]);
            }

            maze[pair.Key].OpenSides.Add(pair.Value);
            Position nextPosition = GetNextPosition(pair.Key, pair.Value);
            Direction lastDirection = pair.Value;
            pair = new(nextPosition, walk.GetValueOrDefault(nextPosition) ?? throw new NullReferenceException(nameof(pair)));

            Direction openSide = new(lastDirection.X * (-1), lastDirection.Y * (-1));

            if (!maze.ContainsKey(pair.Key))
            {
                maze.Add(pair.Key, polygons[pair.Key]);
            }

            maze[pair.Key].OpenSides.Add(openSide);
        }
    }

    private Dictionary<Position, Direction> DoRandomWalk(SortedList<Position, Polygon> polygons, SortedList<Position, Polygon> maze, List<Direction> possibleDirections)
    {
        Dictionary<Position, Direction> currentWalk = new();
        Position currentPosition = SelectRandomElement(polygons.Keys, maze.Keys);
        Position nextPosition;

        do
        {
            Direction chosenDirection = SelectRandomElement(possibleDirections);
            nextPosition = GetNextPosition(currentPosition, chosenDirection);

            if (!polygons.ContainsKey(nextPosition))
            {
                continue;
            }

            if (currentWalk.ContainsKey(currentPosition))
            {
                currentWalk[currentPosition] = chosenDirection;
            }
            else
            {
                currentWalk.Add(currentPosition, chosenDirection);
            }

            currentPosition = nextPosition;
        } while (!maze.ContainsKey(nextPosition));

        currentWalk.Add(nextPosition, Direction.NoDirection);

        return currentWalk;
    }

    private static Position GetNextPosition(Position oldPosition, Direction direction)
    {
        return new Position(oldPosition.X + direction.X, oldPosition.Y + direction.Y);
    }

    private T SelectRandomElement<T>(IList<T> elements)
    {
        if (elements.Count < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(elements), $"The given value must contain at least one element.");
        }

        int randomIndex = _random.Next(0, elements.Count);
        return elements[randomIndex];
    }

    private T SelectRandomElement<T>(IList<T> elements, IList<T> excludedElements)
    {
        if (elements.Count < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(elements), $"The given list must contain at least one element.");
        }

        while (true)
        {
            int randomIndex = _random.Next(0, elements.Count);

            if (!excludedElements.Contains(elements[randomIndex]))
            {
                return elements[randomIndex];
            }
        }
    }

    private KeyValuePair<TKey, TValue> SelectRandomPair<TKey, TValue>(SortedList<TKey, TValue> elements, SortedList<TKey, TValue> excludedElements) where TKey : notnull
    {
        if (elements.Count < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(elements), $"The given value must contain at least one element.");
        }
        
        while (true)
        {
            int randomIndex = _random.Next(0, elements.Count);
            KeyValuePair<TKey, TValue> chosenPolygon = elements.ElementAt(randomIndex);

            if (!excludedElements.ContainsKey(chosenPolygon.Key))
            {
                return chosenPolygon;
            }
        }
    }

    private static SortedList<Position, Polygon> GenerateHexagons(int width, int height)
    {
        SortedList<Position, Polygon> polygons = new();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                polygons.Add(new Position(i, j), new Hexagon());
            }
        }

        return polygons;
    }

    private static SortedList<Position, Polygon> GenerateRectangles(int width, int height)
    {
        SortedList<Position, Polygon> polygons = new();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                polygons.Add(new Position(i, j), new Rectangle());
            }
        }

        return polygons;
    }
}
