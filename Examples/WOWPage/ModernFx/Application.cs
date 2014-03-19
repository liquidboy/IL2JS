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
        StartScreen startScreen;

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
                startScreen = new StartScreen();

                //events
                var mouseDown = new HtmlObservable(h => LibraryManager.DirectCanvas.Canvas.MouseDown += h, h => LibraryManager.DirectCanvas.Canvas.MouseDown -= h);
                var mouseUp = new HtmlObservable(h => LibraryManager.DirectCanvas.Canvas.MouseUp += h, h => LibraryManager.DirectCanvas.Canvas.MouseUp -= h);
                var mouseMove = new HtmlObservable(h => LibraryManager.DirectCanvas.Canvas.MouseMove += h, h => LibraryManager.DirectCanvas.Canvas.MouseMove -= h);
                var mouseOut = new HtmlObservable(h => LibraryManager.DirectCanvas.Canvas.MouseOut += h, h => LibraryManager.DirectCanvas.Canvas.MouseOut -= h);
                var mouseOver = new HtmlObservable(h => LibraryManager.DirectCanvas.Canvas.MouseOver += h, h => LibraryManager.DirectCanvas.Canvas.MouseOver -= h);

                mouseDown.Subscribe(startScreen.OnMouseDown);
                mouseUp.Subscribe(startScreen.OnMouseUp);
                mouseMove.Subscribe(startScreen.OnMouseMove);
                mouseOut.Subscribe(startScreen.OnMouseOut);
                mouseOver.Subscribe(startScreen.OnMouseOver);

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

            if (stats != null) stats.update();

            _update();
            _draw();

        }

        private void _update()
        {

        }

        private void _draw()
        {
            LibraryManager.DirectCanvas.Context.LineWidth = 2;
            LibraryManager.DirectCanvas.Context.StrokeStyle = "red";

            LibraryManager.DirectCanvas.Context.FillStyle = "#9ea7b8";
            LibraryManager.DirectCanvas.Context.FillRect(0, 0, Browser.Window.InnerWidth, Browser.Window.InnerHeight);


            _tracing.DrawString(startScreen.mCurrentVelocityX.ToString(), 100, 100);

        }


        
    }
}



//investigate using TH
//- FILES - http://hg.mozilla.org/labs/th/file/9f0aca725fc4/src/
//- http://hg.mozilla.org/labs/th/file/9f0aca725fc4/src/util.js
//- http://hg.mozilla.org/labs/th/file/9f0aca725fc4/src/th.js
//- http://hg.mozilla.org/labs/th/file/9f0aca725fc4/src/components.js