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
        WOWPage.ModernFx.AngularApplication angularApplication = new ModernFx.AngularApplication();
        WOWPage.ModernFx.WebGLApplication webglApp = new ModernFx.WebGLApplication();
        

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
            application.NumberOfScriptsToLoad = 2;
            Browser.Document.IncludeScripts(application.Loaded, "DirectCanvas.js");
            Browser.Document.IncludeScripts(application.Loaded, "stats.min.js");

            ////setup/configure angular application
            //angularApplication.NumberOfScriptsToLoad = 4;
            //Browser.Document.IncludeScripts(angularApplication.Loaded, "Scripts/angular.js");
            //Browser.Document.IncludeScripts(angularApplication.Loaded, "Scripts/angular-route.js");
            //Browser.Document.IncludeScripts(angularApplication.Loaded, "Scripts/angular-touch.js");
            //Browser.Document.IncludeScripts(angularApplication.Loaded, "Scripts/angular-sanitize.js");

            //setup/configure angular application
            webglApp.NumberOfScriptsToLoad = 1;
            Browser.Document.IncludeScripts(webglApp.Loaded, "Scripts/WebGLCanvas.js");
            


            Browser.Window.Resize += application.Resize; 
        }



    }



   

    
}
