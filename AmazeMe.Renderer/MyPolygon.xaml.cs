using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using AmazeMe.Lib;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI;
using Windows.UI;

namespace AmazeMe.Renderer;

public sealed partial class MyPolygon : UserControl
{
    private int _sideLength = 50;
    private Color _color = Colors.White;

    public MyPolygon(Position position, int polygonSides)
    {
        this.InitializeComponent();
        Line[] myPolygon = polygonSides switch
        {
            4 => CreateMyRectangle(position),
            6 => CreateMyHexagon(position),
            _ => throw new NotImplementedException($"Drawing a polygon with {polygonSides} is currently not supported."),
        };
        
        for (int i = 0; i < myPolygon.Length; i++)
        {
            polygonCanvas.Children.Add(myPolygon[i]);
        }

        Sides = myPolygon;
    }

    public Line[] Sides
    {
        get;
        private set;
    }

    private Line[] CreateMyHexagon(Position startPosition)
    {
        double calculatedNumber = _sideLength * 1.866;
        Position tempPosition = new(calculatedNumber - _sideLength, 0);
        Position newPosition = new(tempPosition.X + calculatedNumber * startPosition.X, _sideLength * startPosition.Y);
        newPosition.Y += startPosition.X * _sideLength / 2;
        return MakeMyHexagon(newPosition);
    }

    private Line[] CreateMyRectangle(Position startPosition)
    {
        Position position = new(_sideLength * startPosition.X, _sideLength * startPosition.Y);
        return MakeMyRectangle(position);
    }

    private Line[] MakeMyRectangle(Position startPoint)
    {
        Line[] myPolygon = new Line[4];

        myPolygon[0] = new()
        {
            X1 = startPoint.X,
            Y1 = startPoint.Y,
            X2 = startPoint.X + _sideLength,
            Y2 = startPoint.Y
        };

        myPolygon[1] = new()
        {
            X2 = startPoint.X + _sideLength,
            Y2 = startPoint.Y + _sideLength
        };

        myPolygon[2] = new()
        {
            X2 = startPoint.X,
            Y2 = startPoint.Y + _sideLength
        };

        myPolygon[3] = new()
        {
            X2 = startPoint.X,
            Y2 = startPoint.Y
        };

        for (int i = 0; i < myPolygon.Length; i++)
        {
            if (i > 0)
            {
                myPolygon[i].X1 = myPolygon[i - 1].X2;
                myPolygon[i].Y1 = myPolygon[i - 1].Y2;
            }

            myPolygon[i].Stroke = new SolidColorBrush(_color);
        }

        return myPolygon;
    }

    private Line[] MakeMyHexagon(Position startPoint)
    {
        Line[] myPolygon = new Line[6];
        double calculatedNumber = _sideLength * 1.866;

        myPolygon[0] = new()
        {
            X1 = startPoint.X,
            Y1 = startPoint.Y,
            X2 = startPoint.X + _sideLength,
            Y2 = startPoint.Y
        };

        myPolygon[1] = new()
        {
            X2 = startPoint.X + calculatedNumber,
            Y2 = startPoint.Y + _sideLength / 2
        };

        myPolygon[2] = new()
        {
            X2 = startPoint.X + _sideLength,
            Y2 = startPoint.Y + _sideLength
        };

        myPolygon[3] = new()
        {
            X2 = startPoint.X,
            Y2 = startPoint.Y + _sideLength
        };

        myPolygon[4] = new()
        {
            X2 = startPoint.X + _sideLength - calculatedNumber,
            Y2 = startPoint.Y + _sideLength / 2
        };

        myPolygon[5] = new()
        {
            X2 = startPoint.X,
            Y2 = startPoint.Y
        };

        for (int i = 0; i < myPolygon.Length; i++)
        {
            if (i > 0)
            {
                myPolygon[i].X1 = myPolygon[i - 1].X2;
                myPolygon[i].Y1 = myPolygon[i - 1].Y2;
            }

            myPolygon[i].Stroke = new SolidColorBrush(_color);
        }

        return myPolygon;
    }
}
