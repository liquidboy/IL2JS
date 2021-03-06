using System;
using System.Collections.Generic;
using Microsoft.LiveLabs.Html;
using Microsoft.LiveLabs.JavaScript.IL2JS;
using Microsoft.LiveLabs.JavaScript.Interop;
using System.Diagnostics;

namespace HelloBrowser
{
    public class HelloBrowserPage : Page
    {
        // Entry point for JavaScript target (boilerplate)
        [EntryPoint]
        public static void Run()
        {
            new HelloBrowserPage();
        }

        public HelloBrowserPage()
        {
            // Conventional DOM interaction
            var button = new Button { Text = "Hello?" };
            button.Click += e =>
            {
                var div = new Div { Text = "Hello Browser!"};
                Browser.Document.Body.Add(div);              
            };
            Browser.Document.Body.Add(button);
            
            // Load external JavaScript, invoke continuation when available
            Browser.Document.IncludeScripts(ExampleLoaded, "example.js");

            Browser.Document.IncludeScripts(JQueryLoaded, "jquery-1.4.2.js");
        }

        private static void ExampleLoaded()
        {
            // A mini database
            var dict = new Dictionary<string, int>() {
                { "Seattle", 602000 },
                { "Portland", 582130 },
                { "Vancouver", 578041 }
            };
            Func<string, int> lookup = name => dict[name];

            var button = new Button { Text = "Info" };
            button.Click += e =>
                     {
                         // Display some facts about Mark
                         var seattle = Find("WA", lookup);
                         var person = new Person { Name = "Mark", City = seattle };
                         Show(person);
                     };
            Browser.Document.Body.Add(button);
        }

        [Import(MemberNameCasing = Casing.Exact)]
        extern private static City Find(string name, Func<string, int> lookup);

        [Import]
        extern private static void Show(Person person);

        public void JQueryLoaded()
        {
            var placeholder = new Div();
            placeholder.Style.Position = "absolute";
            placeholder.Style.Left = "0px";
            placeholder.Style.Top = "200px";
            Document.Body.AppendChild(placeholder);

            var status = new Div
                             {
                                 Id = "status",
                                 Style =
                                     {
                                         Position = "absolute",
                                         Width = "60px",
                                         Height = "10px",
                                         Left = "320px",
                                         Top = "0px"
                                     }
                             };

            var outer = new Div
                            {
                                Id = "outer",
                                Style =
                                    {
                                        Position = "absolute",
                                        Width = "300px",
                                        Height = "60px",
                                        Left = "0px",
                                        Top = "0px",
                                        Overflow = "hidden",
                                        BorderWidth = "2px",
                                        BorderColor = "black",
                                        BorderStyle = "solid",
                                        Margin = "1px"
                                    }
                            };

            var img = new Image { Src = "mslogo-1.jpg", Style = { Width = "300px", Height = "60px" } };

            var inner = new Image
                            {
                                Id = "inner",
                                Src = "ms_net_rgb_web.jpg",
                                Style =
                                    {
                                        Position = "absolute",
                                        Width = "300px",
                                        Height = "60px",
                                        Left = "0px",
                                        Top = "0px"
                                    }
                            };

            placeholder.AppendChild(status);
            outer.AppendChild(img);
            outer.AppendChild(inner);
            placeholder.AppendChild(outer);

            "#outer".JQuery().HoverA
                (node =>
                     {
                         "#status".JQuery().Each<Div>
                             ((node2, _) =>
                                  {
                                      node2.Text = "enter";
                                      return true;
                                  });
                         var width = node.JQuery().OuterWidth();
                         "#inner".JQuery().Animate
                             (new JQueryStyle { Left = width + "px", Opacity = 0.0 },
                              new AnimateOptions { Queue = false, Duration = "300" });
                     },
                 node =>
                 {
                     "#status".JQuery().Each<Div>
                         ((node2, _) =>
                              {
                                  node2.Text = "leave";
                                  return true;
                              });
                     "#inner".JQuery().Animate
                         (new JQueryStyle { Left = "0px", Opacity = 1.0 },
                          new AnimateOptions { Queue = false, Duration = "300" });
                 });
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
        extern public JQuery HoverA(JQueryCallback onIn, JQueryCallback onOut);
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
