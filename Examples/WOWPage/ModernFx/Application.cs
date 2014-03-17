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
        public int NoScriptsToLoad { get; set; }
        Stats stats;

        public void Loaded()
        {
            if (NoScriptsToLoad <= 0) return;

            NoScriptsToLoad--;
            if (NoScriptsToLoad > 0) return;

            try
            {

                var div = LibraryManager.DirectCanvas.CreateRoot();

                stats = LibraryManager.ThreeJS.Stats();
                stats.domElement.Style.Position = "absolute";
                stats.domElement.Style.Top = "0px";
                div.Add(stats.domElement);


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

            _render();

        }

        private void _render()
        {
            LibraryManager.DirectCanvas.Context.LineWidth = 2;
            LibraryManager.DirectCanvas.Context.StrokeStyle = "red";

            LibraryManager.DirectCanvas.Context.FillStyle = "#9ea7b8";
            LibraryManager.DirectCanvas.Context.FillRect(0, 0, Browser.Window.InnerWidth, Browser.Window.InnerHeight);
        }

    }
}
