using Microsoft.LiveLabs.JavaScript.Interop;

namespace Microsoft.LiveLabs.Html
{
    [Import]
    public class Canvas : HtmlElement
    {
        public Canvas(JSContext ctxt) : base(ctxt) { }

        [Import(@"function() { return document.createElement(""Canvas""); }")]
        extern public Canvas();

        [Import("function(inst) { return inst.getContext('2d'); }", PassInstanceAsArgument = true)]
        extern public Context2D Context2D();

    }


    [Import]
    public class Context2D
    {
        [Import("fillStyle")]
        extern public string FillStyle { get; set; }

        [Import("lineWidth")]
        extern public int LineWidth { get; set; }

        [Import("strokeStyle")]
        extern public string StrokeStyle { get; set; }

        [Import("fillRect")]
        extern public string FillRect(int x, int y, int w, int h);
    }
}