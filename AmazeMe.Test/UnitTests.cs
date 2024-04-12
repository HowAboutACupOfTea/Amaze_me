using AmazeMe.Lib;

namespace AmazeMe.Test;

public class Tests
{
    [TestCase(0, 8, 6)]
    [TestCase(-1, 8, 6)]
    [TestCase(8, 0, 6)]
    [TestCase(8, -1, 6)]
    public void Too_Small_Mazes_Can_Not_Be_Generated(int width, int height, int polygonSides)
    {
        //Arrange
        MazeGenerator mazeGenerator = new();

        //Act
        void testDelegate() { mazeGenerator.Generate(width, height, polygonSides); }

        //Assert
        Assert.Throws<ArgumentOutOfRangeException>(testDelegate, "The given value must be bigger than one.");
    }

    /// <summary>
    /// Currently supported (number of) polygons(sides) : Rectangle(4), Hexagon(6)
    /// </summary>
    [TestCase(8, 8, -1)]
    [TestCase(8, 8, 1)]
    [TestCase(8, 8, 2)]
    [TestCase(8, 8, 5)]
    [TestCase(8, 8, 16)]
    public void Mazes_With_Unsupported_Polygons_Can_Not_Be_Generated(int width, int height, int countPolygonSides)
    {
        //Arrange
        MazeGenerator mazeGenerator = new();

        //Act
        void testDelegate() { mazeGenerator.Generate(width, height, countPolygonSides); }

        //Assert
        Assert.Throws<NotImplementedException>(testDelegate, $"Drawing a polygon with {countPolygonSides} is currently not supported.");
    }

    [Test]
    public void A_Maze_With_Closed_Off_Polygons_Is_Invalid([Random(2, 30, 10)] int width, [Random(2, 30, 10)] int height,[Values (4, 6)] int polygonSides)
    {
        //Arrange
        MazeGenerator mazeGenerator = new();
        var maze = mazeGenerator.Generate(width, height, polygonSides);
        bool hasNoOpenSides = false;

        //Act
        foreach (var item in maze)
        {
            if(item.Value.OpenSides.Count <= 0)
            {
                hasNoOpenSides = true;
            }
        }

        //Assert
        Assert.That(hasNoOpenSides, Is.False);
    }

    [Test]
    public void A_Maze_With_Isolated_Polygons_Is_Invalid([Random(2, 30, 10)] int width, [Random(2, 30, 10)] int height, [Values(4, 6)] int polygonSides)
    {
        //Arrange
        MazeGenerator mazeGenerator = new();
        var maze = mazeGenerator.Generate(width, height, polygonSides);
        HashSet<Position> neighboringPositions = new();

        foreach (KeyValuePair<Position, Polygon> polygon in maze)
        {
            foreach (Direction openSide in polygon.Value.OpenSides)
            {
                Position neighbor = new(polygon.Key.X + openSide.X, polygon.Key.Y + openSide.Y);
                neighboringPositions.Add(neighbor);
            }
        }

        //Assert
        Assert.That(maze, Has.Count.EqualTo(neighboringPositions.Count));
    }

    /// <summary>
    /// This method uses a backtracking algorithm to test the maze.
    /// 
    /// (The backtracking algorithm should probably be written in the <see cref="AmazeMe.Lib"/> library.
    ///   After testing the algorithm, it could be used to test the maze.)
    /// </summary>
    //[Test]
    public void A_Maze_With_Unvisitable_Positions_Is_Invalid([Random(2, 10, 10)] int width, [Random(2, 10, 10)] int height, [Values(4, 6)] int polygonSides)
    {
        //Arrange
        MazeGenerator mazeGenerator = new();
        var maze = mazeGenerator.Generate(width, height, polygonSides);
        HashSet<Position> visitedPositions = new();
        Position startPosition = maze.First().Key;

        //Act
        VisitUnvisitedNeighbor(maze, startPosition, visitedPositions);

        //Assert
        Assert.That(maze, Has.Count.EqualTo(visitedPositions.Count));
    }

    private void VisitUnvisitedNeighbor(SortedList<Position, Polygon> maze, Position currentPosition, HashSet<Position> visitedPositions)
    {
        IList<Position> positions = maze.Keys;
        List<Direction> possibleDirections = maze[currentPosition].OpenSides;
        Position? neighbor;

        do
        {
            visitedPositions.Add(currentPosition);
            neighbor = ChooseRandomUnvisitedNeighbor(positions, possibleDirections, currentPosition, visitedPositions);

            if (neighbor.HasValue)
            {
                currentPosition = neighbor.Value;
                VisitUnvisitedNeighbor(maze, currentPosition, visitedPositions);
            }
        } while (neighbor.HasValue);
    }

    private Position? ChooseRandomUnvisitedNeighbor(IList<Position> neighbors, List<Direction> possibleDirections, Position currentPosition, HashSet<Position> visitedNeighbors)
    {
        HashSet<Direction> triedDirections = new();

        do
        {
            Direction chosenDirection = possibleDirections[Random.Shared.Next(0, possibleDirections.Count)];
            Position possibleNeighbor = new(currentPosition.X + chosenDirection.X, currentPosition.Y + chosenDirection.Y);

            if (neighbors.Contains(possibleNeighbor) && !visitedNeighbors.Contains(possibleNeighbor))
            {
                return possibleNeighbor;
            }

            triedDirections.Add(chosenDirection);
        } while (triedDirections.Count < possibleDirections.Count);

        return null;
    }
}