using System;
using System.Collections.Generic;
using Microsoft.LiveLabs.Html;
using Microsoft.LiveLabs.JavaScript.IL2JS;
using Microsoft.LiveLabs.JavaScript.Interop;
using System.Diagnostics;
using WOWPage.OpenJS;
using Microsoft.LiveLabs.JavaScript;

namespace WOWPage
{
    public class WOWPageStart : Page
    {
        
        Canvas canvas = new Canvas();
        

        // Entry point for JavaScript target (boilerplate)
        [EntryPoint]
        public static void Run()
        {
            new WOWPageStart();
        }

        public WOWPageStart()
        {
            Browser.Document.Body.Style.Margin = "0px";
            Browser.Document.Body.Style.OverflowX = "hidden";
            Browser.Document.Body.Style.OverflowY = "hidden";

            Browser.Document.IncludeScripts(EaselJSLoaded, "easeljs-0.7.1.min.js");

            Browser.Window.Resize += Window_Resize;
        }

        void Window_Resize(HtmlEvent theEvent)
        {
            //setting "style" attributes for width/height skews the canvas... 
            //canvas.Style.Width = Browser.Document.ParentWindow.InnerWidth + "px";
            //canvas.Style.Height = Browser.Document.ParentWindow.InnerHeight + "px";

            canvas.SetAttribute("Width", Browser.Document.ParentWindow.InnerWidth + "px");
            canvas.SetAttribute("Height", Browser.Document.ParentWindow.InnerHeight + "px");

            DrawCanvas();
        }

        
        private void EaselJSLoaded()
        {

            canvas = new Canvas();

            canvas.Id = "canvasScene";
            canvas.SetAttribute("Width", Browser.Document.ParentWindow.InnerWidth + "px");
            canvas.SetAttribute("Height", Browser.Document.ParentWindow.InnerHeight + "px");

            //setting "style" attributes for width/height skews the canvas... 
            //canvas.Style.Width = Browser.Document.ParentWindow.InnerWidth + "px";
            //canvas.Style.Height = Browser.Document.ParentWindow.InnerHeight + "px";

            Browser.Document.Body.Add(canvas);

            DrawCanvas();


        }

        Stage stage = null;
        Graphics graphics = null;

        private void DrawCanvas()
        {




            if (stage == null) stage = LibraryManager.CreateJS.Stage("canvasScene");
            if (graphics == null) graphics = LibraryManager.CreateJS.Graphics();




            //IMAGE
            var bitmap = LibraryManager.CreateJS.Bitmap("KeystoneLoginBackground01.png");
            stage.addChild(bitmap);

            //CIRCLE
            graphics.setStrokeStyle(1);
            graphics.beginStroke("#800080");
            graphics.beginFill("#800080");
            graphics.drawCircle(100, 100, 10);
            var shape = LibraryManager.CreateJS.Shape(graphics);
            stage.addChild(shape);



            //RECTANGLE
            //var rect = LibraryManager.CreateJS.Rectangle(10, 10, 100, 100);
            //stage.addChild(rect);
            //shape.x = 50;
            //shape.y = 50;

           


            //TEXT
            var text = LibraryManager.CreateJS.Text("Hello World", "20px Arial", "#ff7700");
            text.x = 120;
            text.y = 105;
            text.textBaseline = "alphabetic";
            stage.addChild(text);


            //UPDATE
            stage.update();



            //Browser.Window.Alert("finished createjs stuff");


        }
    }



    public static class LibraryManager
    {

        public static CreateJS CreateJS;


    }


}
