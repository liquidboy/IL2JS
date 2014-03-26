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

        Demo01Renderer _render;

        public void Loaded()
        {
            if (NumberOfScriptsToLoad <= 0) return;

            NumberOfScriptsToLoad--;
            if (NumberOfScriptsToLoad > 0) return;

            _render = new Demo01Renderer();

            try
            {
                //Browser.Window.Alert("loaded webGl app ... 1");

                var div = LibraryManager.WebGLCanvas.CreateRoot();
               // Browser.Window.Alert("loaded webGl app ... 2");
                var shader1 = LibraryManager.WebGLCanvas.Context.GetShader("2d-vertex-shader");
                //Browser.Window.Alert("loaded webGl app ... 3");
                var shader2 = LibraryManager.WebGLCanvas.Context.GetShader("black");
                //Browser.Window.Alert("loaded webGl app ... 4");

                var program = LibraryManager.WebGLCanvas.Context.CreateProgram();
                //Browser.Window.Alert("loaded webGl app ... 5");
                LibraryManager.WebGLCanvas.Context.AttachShader(program, shader1);
                //Browser.Window.Alert("loaded webGl app ... 6");
                LibraryManager.WebGLCanvas.Context.AttachShader(program, shader2);

                //Browser.Window.Alert("loaded webGl app ... 10");

                var linked = LibraryManager.WebGLCanvas.Context.GetLinkStatus(LibraryManager.WebGLCanvas.Context, program);
                if (!linked)
                {
                    Browser.Window.Alert("something went wrong in setting up webgl and linking");
                    var err = LibraryManager.WebGLCanvas.Context.GetProgramInfoLog(program);
                    Browser.Window.Alert("programme error : " + err);
             
                }
                else
                {
                    Browser.Window.Alert("linking was successful");
                }

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

    public class Demo01Renderer
    {
    }
}
