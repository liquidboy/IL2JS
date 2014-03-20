using Microsoft.LiveLabs.Html;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WOWPage.ModernFx
{
    public class StartDashboard : InfiniteLayout
    {
        List<Tile> _tiles = new List<Tile>();

        public StartDashboard(int maxX, int maxY, int minX, int minY)
            : base(maxX, maxY, minX, minY)
        {

            for (var row = 0; row < 5; row++)
            {
                for (var col = 0; col < 100; col++)
                {
                    _tiles.Add(new Tile() { X = 122 * col, Y = 122 * row, Width = 120, Height = 120 });
                }
            }

           
        }


        public void Update()
        {
            base.Update();
        }


        public void Draw()
        {

            foreach(var tile in _tiles){

                LibraryManager.DirectCanvas.Context.FillStyle = "#c6c6c6";
                LibraryManager.DirectCanvas.Context.StrokeStyle = "#c6c6c6";
                LibraryManager.DirectCanvas.Context.LineWidth = 1;
                LibraryManager.DirectCanvas.Context.FillRect((int)this.X + tile.X + 50, (int)this.Y + tile.Y + 80, tile.Width, tile.Height);
            }


        }

    }


    public class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
