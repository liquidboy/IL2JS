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
    public class WebGLApplication
    {
        public int NumberOfScriptsToLoad { get; set; }

        public void Loaded()
        {
            if (NumberOfScriptsToLoad <= 0) return;

            NumberOfScriptsToLoad--;
            if (NumberOfScriptsToLoad > 0) return;

            try
            {
                //Browser.Window.Alert("loaded webGl app ... 1");

                var div = LibraryManager.WebGLCanvas.CreateRoot();

                Browser.Window.Alert("loaded webGl app ... 2");

            }
            catch (Exception ex)
            {
                Browser.Window.Alert("error .... " + ex.Message);
            }
        }

        public void Resize(HtmlEvent theEvent)
        {

        }


      


        
    }
}
