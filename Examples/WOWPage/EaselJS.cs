using System;
using System.Collections.Generic;
using Microsoft.LiveLabs.Html;
using Microsoft.LiveLabs.JavaScript.IL2JS;
using Microsoft.LiveLabs.JavaScript.Interop;
using System.Diagnostics;
using WOWPage.OpenJS;

namespace WOWPage
{



    [Import]
    public class EaselJS
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

        [Import(@"function(text, font, color){ return new this.createjs.Text(text, font, color) ; }")]
        extern public Text Text(string text, string font, string color);

        [Import(@"function(){ return new this.createjs.Container() ; }")]
        extern public Container Container();

        [Import("createjs.Ticker")]
        extern public Ticker Ticker{get;}
    }

    [Import]
    public class Stage
    {
        [Import(Creation = Creation.Object)]
        extern public Stage();

        extern public void addChild(Bitmap item);
        extern public void addChild(Rectangle item);
        extern public void addChild(Shape item);
        extern public void addChild(Text item);
        extern public void addChild(Container item);
        extern public void update();
        extern public void clear();



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
        extern public void drawRoundRect(int x, int y, int width, int height, int radius);
        extern public string getRGB(int r, int g, int b);
        extern public void clear();
    }

    [Import]
    public class Text
    {
        [Import(Creation = Creation.Object)]
        extern public Text();
        extern public int x { get; set; }
        extern public int y { get; set; }
        extern public string textBaseline { get; set; }
    }

    [Import]
    public class Shape
    {
        [Import(Creation = Creation.Object)]
        extern public Shape();
        extern public int x { get; set; }
        extern public int y { get; set; }

        extern public void addEventListener(string eventName, EventListenerCallback function);


    }

    [Import]
    public class Container
    {
        [Import(Creation = Creation.Object)]
        extern public Container();

        extern public void addChild(Bitmap item);
        extern public void addChild(Rectangle item);
        extern public void addChild(Shape item);
        extern public void addChild(Text item);
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

    [Import]
    public class Ticker
    {
        [Import(Creation = Creation.Object)]
        extern public Ticker();

        extern public void addEventListener(string eventName, EventListenerCallback function);

    }


    [Import]
    public class Event
    {
        [Import(Creation = Creation.Object)]
        extern public Event();

        extern public Microsoft.LiveLabs.JavaScript.JSObject target { get; set; }
    }

    [Export(PassInstanceAsArgument = true)]
    public delegate void EventListenerCallback(Event node);
}
