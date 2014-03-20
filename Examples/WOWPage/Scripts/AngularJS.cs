using Microsoft.LiveLabs.JavaScript.Interop;
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

namespace WOWPage.Scripts
{
    [Import]
    public class AngularJS
    {

        [Import(@"function(appName, services){ return angular.module(appn, services); }")]
        extern public object RegisterModule(string appName, string[] services);
    }
}
