using Microsoft.LiveLabs.JavaScript.Interop;

namespace Microsoft.LiveLabs.Html
{
    [Import]
    public class Canvas : HtmlElement
    {
        public Canvas(JSContext ctxt) : base(ctxt) { }

        [Import(@"function() { return document.createElement(""Canvas""); }")]
        extern public Canvas();

    }
}