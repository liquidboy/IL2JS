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
            canvas.Style.Width = Browser.Document.ParentWindow.InnerWidth + "px";
            canvas.Style.Height = Browser.Document.ParentWindow.InnerHeight + "px";
        }

        
        private void EaselJSLoaded()
        {

            canvas = new Canvas();
            canvas.Id = "canvasScene";
            canvas.Style.Width = Browser.Document.ParentWindow.InnerWidth + "px";
            canvas.Style.Height = Browser.Document.ParentWindow.InnerHeight + "px";

            Browser.Document.Body.Add(canvas);


            var stage = LibraryManager.CreateJS.Stage("canvasScene");
            var graphics = LibraryManager.CreateJS.Graphics();
            //System.Windows.Media.Colors.Red.
            graphics.setStrokeStyle(1);
            //var col = System.Windows.Media.Colors.Purple.ToString();
            //Browser.Window.Alert(col);
            //graphics.beginStroke(
            //    graphics.getRGB(
            //        (int)System.Windows.Media.Colors.Purple.R,
            //        (int)System.Windows.Media.Colors.Purple.G,
            //        (int)System.Windows.Media.Colors.Purple.B
            //    ));
            graphics.beginStroke("#800080");
            graphics.beginFill("#800080");
            graphics.drawCircle(20, 20, 10);

            var shape = LibraryManager.CreateJS.Shape(graphics);
            //var bitmap = LibraryManager.CreateJS.Bitmap("KeystoneLoginBackground01.png");
            //var rect = LibraryManager.CreateJS.Rectangle(10, 10, 100, 100);
            //stage.addChild(bitmap);
            //stage.addChild(rect);

            //shape.x = 50;
            //shape.y = 50;

            stage.addChild(shape);

            stage.update();

            //Browser.Window.Alert("finished createjs stuff");

            //var cjs = new createjs();
            //var stageffffff = cjs.stageDDDDD("canvasScene");
            //var image = createJs.Bitmap("KeystoneLoginBackground01.png");

            //stage.addChild(image);

        }

    }



    public static class LibraryManager
    {

        public static CreateJS CreateJS;


    }


    [Import]
    public class CreateJS
    {
        [Import(@"function(id){ return new this.createjs.Stage(id) ; }")]
        extern public Stage Stage(string canvasName);

        [Import(@"function(){ return new this.createjs.Graphics() ; }")]
        extern public Graphics Graphics();

        [Import(@"function(g){ return new this.createjs.Shape(g) ; }")]
        extern public Shape Shape(Graphics graphics);

        [Import(@"function(url){ return new this.createjs.Bitmap(url) ; }")]
        extern public Bitmap Bitmap(string url);

        [Import(@"function(x,y,w,h){ return new this.createjs.Rectangle(x,y,w,h) ; }")]
        extern public Rectangle Rectangle(int x, int y, int width, int height);
    }

    [Import]
    public class Stage
    {
        [Import(Creation = Creation.Object)]
        extern public Stage();

        extern public void addChild(Bitmap item);
        extern public void addChild(Rectangle item);
        extern public void addChild(Shape item);
        extern public void update();


    }

    [Import]
    public class Graphics
    {
        [Import(Creation = Creation.Object)]
        extern public Graphics();

        extern public void setStrokeStyle(int style);
        extern public void beginStroke(string color);
        extern public void beginFill(string color);
        extern public void drawCircle(int x, int y, int radius);
        extern public string getRGB(int r, int g, int b);

    }

    [Import]
    public class Shape
    {
        [Import(Creation = Creation.Object)]
        extern public Shape();
        extern public int x { get; set; }
        extern public int y { get; set; }
    }


    [Import]
    public class Bitmap
    {
        [Import(Creation = Creation.Object)]
        extern public Bitmap();
    }

    [Import]
    public class Rectangle
    {
        [Import(Creation = Creation.Object)]
        extern public Rectangle();
    }


    //[Export(PassInstanceAsArgument=true)]
    //public delegate void JQueryCallback(DomNode node);

    //[Export(PassInstanceAsArgument=true)]
    //public delegate bool JQueryForeach<T>(T node, int pos) where T : DomNode;

    //[Import]
    //public static class JQueryHelpers {

    //    extern public static JQuery JQuery(this string expr);
    //    extern public static JQuery JQuery(this DomNode node);
    //    extern public static JQuery JQuery(this DomNode[] nodes);
    //}

    //[Import]
    //public class JQuery {
    //    extern public JQuery Each<T>(JQueryForeach<T> f) where T : DomNode;
    //    extern public JQuery Find(string selector);
    //    extern public JQuery Hover(JQueryCallback onIn, JQueryCallback onOut);
    //    extern public int OuterWidth();
    //    extern public JQuery Animate(JQueryStyle style, AnimateOptions opts);
    //}

    //[Import]
    //public class AnimateOptions {
    //    extern public string Duration { get; set; }
    //    extern public string Easing { get; set; }
    //    extern public JQueryCallback Complete { get; set; }
    //    extern public JQueryCallback Step { get; set; }
    //    extern public bool Queue { get; set; }
    //}

    //[Import]
    //public class JQueryStyle
    //{
    //    [Import(Creation = Creation.Object)]
    //    extern public JQueryStyle();

    //    extern public string Left { get; set; }
    //    extern public double Opacity { get; set; }
    //}
}
