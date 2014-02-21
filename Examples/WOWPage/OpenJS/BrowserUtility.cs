using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.LiveLabs.Html;
using Microsoft.LiveLabs.JavaScript;
using Microsoft.LiveLabs.JavaScript.Interop;

namespace WOWPage.OpenJS
{
    internal static class BrowserUtility
    {
        public static void InitBrowserUtility()
        {
            FireFox = false;
            InternetExplorer = false;
            InternetExplorer7 = false;
            InternetExplorer8Standard = false;

            string agt = Browser.Window.Navigator.UserAgent.ToLower();
            InternetExplorer = (agt.IndexOf("msie") != -1);
            if (InternetExplorer)
            {
                string stIEVer = agt.Substring(agt.IndexOf("msie ") + 5);
                var j = stIEVer.IndexOf(';');
                if (j > 0)
                    stIEVer = stIEVer.Substring(0, j);
                var v = default(double);
                if (double.TryParse(stIEVer, out v))
                    InternetExplorer7 = v == 7.0;

                // In IE8, the version number, more specifically the number follows msie in userAgent string is not necessary 8,
                // Thus we should really look at documentMode to see how the page is rendered. The documentMode can be 8,7,5
                InternetExplorer8Standard = Browser.Document.DocumentMode == 8;
            }

            bool nav = ((agt.IndexOf("mozilla") != -1) && ((agt.IndexOf("spoofer") == -1) && (agt.IndexOf("compatible") == -1)));
            FireFox = nav && (agt.IndexOf("firefox") != -1);
        }

        public static bool FireFox { get; private set; }
        public static bool InternetExplorer { get; private set; }
        public static bool InternetExplorer7 { get; private set; }
        public static bool InternetExplorer8Standard { get; private set; }
    }
}
