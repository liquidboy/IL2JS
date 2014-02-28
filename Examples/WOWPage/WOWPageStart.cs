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
        Graphics graphics1 = null;
        Graphics graphics2 = null;
        private void DrawCanvas()
        {
            if (stage == null) stage = LibraryManager.CreateJS.Stage("canvasScene");
            if (graphics1 == null) graphics1 = LibraryManager.CreateJS.Graphics();
            if (graphics2 == null) graphics2 = LibraryManager.CreateJS.Graphics();



            //IMAGE
            var bitmap = LibraryManager.CreateJS.Bitmap("KeystoneLoginBackground01.png");
            stage.addChild(bitmap);

            //UPDATE
            stage.update();

            //CIRCLE 1
            graphics1.setStrokeStyle(1);
            graphics1.beginStroke("#800080");
            graphics1.beginFill("#800080");
            graphics1.drawCircle(100, 100, 10);
            var shape = LibraryManager.CreateJS.Shape(graphics1);

            //CIRCLE 1 CLICK
            shape.addEventListener("click", (e) =>
            {
                Browser.Window.Alert("clicked circle 1");
            });

            stage.addChild(shape);


            //UPDATE
            stage.update();
            stage.clear();
            //graphics1.clear();


            //CIRCLE 2
            graphics1.setStrokeStyle(1);
            graphics1.beginStroke("#800080");
            graphics1.beginFill("#800080");
            graphics1.drawCircle(500, 400, 40);
            var shape3 = LibraryManager.CreateJS.Shape(graphics1);
            var container1 = LibraryManager.CreateJS.Container();
            container1.addChild(shape3);

            //CIRCLE 2 CLICK
            shape3.addEventListener("click", (e) =>
            {
                Browser.Window.Alert("clicked circle 3");
            });


            stage.addChild(container1);




            //UPDATE
            stage.update();


            //RECTANGLE
            graphics2.setStrokeStyle(5);
            graphics2.beginStroke("#800080");
            graphics2.beginFill("#800080");
            graphics2.drawRoundRect(110, 110, 180, 90, 0);
            var shape2 = LibraryManager.CreateJS.Shape(graphics2);
            stage.addChild(shape2);

            //UPDATE
            stage.update();


            //TEXT
            var text = LibraryManager.CreateJS.Text("Hello World", "20px Arial", "#ff7700");
            text.x = 150;
            text.y = 155;
            text.textBaseline = "alphabetic";
            stage.addChild(text);

            //UPDATE
            stage.update();
            


            //TICKER
            LibraryManager.CreateJS.Ticker.addEventListener("tick", (evt)=>{
                text.x += 1;
                Browser.Window.Alert("got here");
                stage.update();
            });


            
            //Browser.Window.Alert("finished createjs stuff");


        }
    }



    public static class LibraryManager
    {

        public static CreateJS CreateJS;


    }


}
