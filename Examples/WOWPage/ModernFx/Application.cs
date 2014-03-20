using Microsoft.LiveLabs.Html;
using System;
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
    public class Application
    {
        public int NumberOfScriptsToLoad { get; set; }
        Stats stats;
        Tracing _tracing;
        InfiniteLayout startScreenLayout;

        public void Loaded()
        {
            if (NumberOfScriptsToLoad <= 0) return;

            NumberOfScriptsToLoad--;
            if (NumberOfScriptsToLoad > 0) return;

            try
            {

                var div = LibraryManager.DirectCanvas.CreateRoot();

                //statistics
                stats = LibraryManager.ThreeJS.Stats();
                stats.domElement.Style.Position = "absolute";
                stats.domElement.Style.Top = "0px";
                div.Add(stats.domElement);

                //tracing
                _tracing = new Tracing();

                //startScreen
                startScreenLayout = new InfiniteLayout(1000, 1000, -1000, -1000);
                startScreenLayout.AllowVerticalNavigation = true;

                //events
                var mouseDown = new HtmlObservable(h => LibraryManager.DirectCanvas.Canvas.MouseDown += h, h => LibraryManager.DirectCanvas.Canvas.MouseDown -= h);
                var mouseUp = new HtmlObservable(h => LibraryManager.DirectCanvas.Canvas.MouseUp += h, h => LibraryManager.DirectCanvas.Canvas.MouseUp -= h);
                var mouseMove = new HtmlObservable(h => LibraryManager.DirectCanvas.Canvas.MouseMove += h, h => LibraryManager.DirectCanvas.Canvas.MouseMove -= h);
                var mouseOut = new HtmlObservable(h => LibraryManager.DirectCanvas.Canvas.MouseOut += h, h => LibraryManager.DirectCanvas.Canvas.MouseOut -= h);
                var mouseOver = new HtmlObservable(h => LibraryManager.DirectCanvas.Canvas.MouseOver += h, h => LibraryManager.DirectCanvas.Canvas.MouseOver -= h);

                mouseDown.Subscribe(startScreenLayout.OnMouseDown);
                mouseUp.Subscribe(startScreenLayout.OnMouseUp);
                mouseMove.Subscribe(startScreenLayout.OnMouseMove);
                mouseOut.Subscribe(startScreenLayout.OnMouseOut);
                mouseOver.Subscribe(startScreenLayout.OnMouseOver);

                _loop();

            }
            catch (Exception ex)
            {
                Browser.Window.Alert("error .... " + ex.Message);
            }
        }

        public void Resize(HtmlEvent theEvent)
        {
            LibraryManager.DirectCanvas.Resize(Browser.Window.InnerWidth, Browser.Window.InnerHeight);
        }

        private void _loop()
        {
            Browser.Window.RequestAnimationFrame(_loop);

            _update();
            _draw();

        }

        private void _update()
        {
            if (stats != null) stats.update();
        }

        private void _draw()
        {
            startScreenLayout.Update();

            //LibraryManager.DirectCanvas.Context.LineWidth = 2;
            //LibraryManager.DirectCanvas.Context.StrokeStyle = "red";

            //background of canvas
            LibraryManager.DirectCanvas.Context.FillStyle = "#f3f3f3";
            LibraryManager.DirectCanvas.Context.FillRect(0, 0, Browser.Window.InnerWidth, Browser.Window.InnerHeight);

            //center rectangle for testing ...
            LibraryManager.DirectCanvas.Context.FillStyle = "#c6c6c6";
            LibraryManager.DirectCanvas.Context.FillRect(
                (int)startScreenLayout.X + ((Browser.Window.InnerWidth / 2) - 5),
                (int)startScreenLayout.Y + ((Browser.Window.InnerHeight / 2) - 5), 
                10, 
                10);
            

            //tracing output
            var data1 = string.Format("__Final X={0} Y={1}",
                startScreenLayout.TargetX.ToString(),
                startScreenLayout.TargetY.ToString());

            var data2 = string.Format("Current X={0} Y={1}",
                startScreenLayout.X.ToString(),
                startScreenLayout.Y.ToString());

            _tracing.DrawString(data1, 50, 100);
            _tracing.DrawString(data2, 50, 80);


        }


        
    }
}



//investigate using TH
//- FILES - http://hg.mozilla.org/labs/th/file/9f0aca725fc4/src/
//- http://hg.mozilla.org/labs/th/file/9f0aca725fc4/src/util.js
//- http://hg.mozilla.org/labs/th/file/9f0aca725fc4/src/th.js
//- http://hg.mozilla.org/labs/th/file/9f0aca725fc4/src/components.js