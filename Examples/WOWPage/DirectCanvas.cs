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
    public class DirectCanvas
    {
        Div _divRoot;
        Canvas _canvasRoot;

        public Context2D Context { get { return _canvasRoot.Context2D(); } }


        public Div CreateRoot()
        {
            _divRoot = new Div();


            _canvasRoot = new Canvas();
            _canvasRoot.Id = "canvasScene";


            Resize(Browser.Window.InnerWidth, Browser.Window.InnerHeight);

            _divRoot.Add(_canvasRoot);

            Browser.Document.Body.Add(_divRoot);

            return _divRoot;

        }



        public void Resize(int width, int height)
        {

            _divRoot.SetAttribute("Width", width + "px");
            _divRoot.SetAttribute("Height", height + "px");

            _canvasRoot.SetAttribute("Width", width + "px");
            _canvasRoot.SetAttribute("Height", height + "px");


        }
    }
}
