using AmazeMe.Renderer.ViewModel;
using Microsoft.UI.Xaml;
using System;

namespace AmazeMe.Renderer;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        MazeGeneratorViewModel vm = new();

        foreach (var item in vm.MyPolygons)
        {
            myCanvas.Children.Add(item);
        }

        myCanvas.Height = vm.Height * 150;
        myCanvas.Width = vm.Width * 150;
    }
}
