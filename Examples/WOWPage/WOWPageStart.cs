using System;
using System.Collections.Generic;
using Microsoft.LiveLabs.Html;
using Microsoft.LiveLabs.JavaScript.IL2JS;
using Microsoft.LiveLabs.JavaScript.Interop;
using System.Diagnostics;
using WOWPage.OpenJS;

namespace WOWPage
{
    public class WOWPageStart : Page
    {
        // Entry point for JavaScript target (boilerplate)
        [EntryPoint]
        public static void Run()
        {
            new WOWPageStart();
        }

        public WOWPageStart()
        {

            BrowserUtility.InitBrowserUtility();

            var button = new Button { Text = "DocumentMode?" };
            button.Click += e =>
            {
                var div = new Div { Text =  Browser.Document.DocumentMode.ToString() };
                Browser.Document.Body.Add(div);
            };
            Browser.Document.Body.Add(button);

            button = new Button { Text = "UserAgent?" };
            button.Click += e =>
            {
                var div = new Div { Text = Browser.Window.Navigator.UserAgent.ToLower()  };
                Browser.Document.Body.Add(div);
            };
            Browser.Document.Body.Add(button);
        }

    }

    // The members of this class may be invoked from JavaScript
    [Export]
    [Interop(DefaultKey = "id")]
    public class Person
    {
        public string Name { get; set; }
        public City City { get; set; }

        [NotExported]
        public Person() { }

        public void Render(HtmlElement parent)
        {
            var s = new Span { Text = Name, Style = { Margin = "10pt" } };
            parent.AppendChild(s);
            var d = new Div();
            City.Render(d);
            parent.AppendChild(d);
        }
    }

    // The extern member of this class are implemented by JavaScript
    [Import]
    public class City
    {
        extern public string Name { get; set; }
        extern public int Population { get; set; }

        public void Render(HtmlElement parent)
        {
            var s1 = new Span { Text = Name, Style = { Margin = "10pt" } };
            parent.AppendChild(s1);
            var s2 = new Span { Text = Population.ToString(), Style = { Margin = "10pt" } };
            parent.AppendChild(s2);
        }
    }

    [Export(PassInstanceAsArgument=true)]
    public delegate void JQueryCallback(DomNode node);

    [Export(PassInstanceAsArgument=true)]
    public delegate bool JQueryForeach<T>(T node, int pos) where T : DomNode;

    [Import]
    public static class JQueryHelpers {

        extern public static JQuery JQuery(this string expr);
        extern public static JQuery JQuery(this DomNode node);
        extern public static JQuery JQuery(this DomNode[] nodes);
    }

    [Import]
    public class JQuery {
        extern public JQuery Each<T>(JQueryForeach<T> f) where T : DomNode;
        extern public JQuery Find(string selector);
        extern public JQuery Hover(JQueryCallback onIn, JQueryCallback onOut);
        extern public int OuterWidth();
        extern public JQuery Animate(JQueryStyle style, AnimateOptions opts);
    }

    [Import]
    public class AnimateOptions {
        extern public string Duration { get; set; }
        extern public string Easing { get; set; }
        extern public JQueryCallback Complete { get; set; }
        extern public JQueryCallback Step { get; set; }
        extern public bool Queue { get; set; }
    }

    [Import]
    public class JQueryStyle
    {
        [Import(Creation = Creation.Object)]
        extern public JQueryStyle();

        extern public string Left { get; set; }
        extern public double Opacity { get; set; }
    }
}
