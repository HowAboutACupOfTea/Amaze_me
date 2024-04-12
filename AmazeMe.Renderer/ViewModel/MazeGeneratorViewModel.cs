using AmazeMe.Lib;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using static Microsoft.UI.Xaml.Visibility;

namespace AmazeMe.Renderer.ViewModel;

public class MazeGeneratorViewModel
{
    public MazeGeneratorViewModel()
    {
        MazeGenerator mazeGenerator = new();
        mazeGenerator.Settings.SaveSetting(Environment.GetCommandLineArgs());
        Settings settings = mazeGenerator.Settings;
        Width = settings.Width;
        Height = settings.Height;
        Maze = mazeGenerator.Generate(settings.Width, settings.Height, settings.PolygonSides);
        MyPolygons = new();

        foreach (var polygon in Maze)
        {
            MyPolygon temp = new(polygon.Key, settings.PolygonSides);
            Action<MyPolygon, List<Direction>> action = settings.PolygonSides switch
            {
                4 => MakeOpenRectangleSidesInvisible,
                6 => MakeOpenHexagonSidesInvisible,
                _ => throw new NotImplementedException($"Drawing a polygon with {settings.PolygonSides} is currently not supported."),
            };

            action(temp, polygon.Value.OpenSides);
            MyPolygons.Add(temp);
        }
    }

    public int Height
    {
        get;
        private set;
    }

    public int Width
    {
        get;
        private set;
    }

    public SortedList<Position, Lib.Polygon> Maze
    {
        get;
        private set;
    }

    public List<MyPolygon> MyPolygons
    {
        get;
        private set;
    }

    private void MakeOpenHexagonSidesInvisible(MyPolygon myPolygon, List<Direction> openSides)
    {
        foreach (var openSide in openSides)
        {
            if (openSide == Direction.NoDirection)
                continue;

            Line chosenSide = null;

            if (openSide.X == 0)
            {
                if (openSide.Y < 0)
                    chosenSide = myPolygon.Sides[0];
                else
                    chosenSide = myPolygon.Sides[3];
            }

            if (openSide.X > 0)
            {
                if (openSide.Y < 0)
                    chosenSide = myPolygon.Sides[1];
                else
                    chosenSide = myPolygon.Sides[2];
            }

            if (openSide.X < 0)
            {
                if (openSide.Y > 0)
                    chosenSide = myPolygon.Sides[4];
                else
                    chosenSide = myPolygon.Sides[5];
            }

            if (chosenSide != null)
                chosenSide.Visibility = Collapsed;
        }
    }

    private void MakeOpenRectangleSidesInvisible(MyPolygon myPolygon, List<Direction> openSides)
    {
        foreach (var openSide in openSides)
        {
            if (openSide == Direction.NoDirection)
                continue;

            Line chosenSide = null;

            if (openSide == Direction.North)
            {
                chosenSide = myPolygon.Sides[0];
            }
            else if (openSide == Direction.East)
            {
                chosenSide = myPolygon.Sides[1];
            }
            else if (openSide == Direction.South)
            {
                chosenSide = myPolygon.Sides[2];
            }
            else if (openSide == Direction.West)
            {
                chosenSide = myPolygon.Sides[3];
            }

            if (chosenSide != null)
                chosenSide.Visibility = Collapsed;
        }
    }
}
