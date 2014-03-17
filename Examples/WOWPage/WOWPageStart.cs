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

        WOWPage.ModernFx.Application application = new ModernFx.Application();

        // Entry point for JavaScript target (boilerplate)
        [EntryPoint]
        public static void Run()
        {
            new WOWPageStart();
        }

        public WOWPageStart()
        {
            //set page default style
            Browser.Document.Body.Style.Margin = "0px";
            Browser.Document.Body.Style.OverflowX = "hidden";
            Browser.Document.Body.Style.OverflowY = "hidden";
            Browser.Document.Body.Style.BackgroundColor = "#fff";

            //setup/configure modern application
            application.NoScriptsToLoad = 2;
            Browser.Document.IncludeScripts(application.Loaded, "DirectCanvas.js");
            Browser.Document.IncludeScripts(application.Loaded, "stats.min.js");

            Browser.Window.Resize += application.Resize; 
        }

    }



   

    
}
