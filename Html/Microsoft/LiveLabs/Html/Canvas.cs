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
        [Import("beginPath")]
        extern public void BeginPath();

        

        [Import("fillRect")]
        extern public string FillRect(int x, int y, int w, int h);

        [Import("fillStyle")]
        extern public string FillStyle { get; set; }


        [Import("fillText")]
        extern public void FillText(string str, int x, int y);

        [Import("font")]
        extern public string FontFamily { get; set; }


        [Import("lineTo")]
        extern public void LineTo(int x, int y);

        [Import("lineWidth")]
        extern public int LineWidth { get; set; }


        [Import("moveTo")]
        extern public void MoveTo(int x, int y);


        [Import("restore")]
        extern public void Restore();

        [Import("save")]
        extern public void Save();

        [Import("strokeStyle")]
        extern public string StrokeStyle { get; set; }

        [Import("stroke")]
        extern public void Stroke();

        [Import("textBaseline")]
        extern public string TextBaseline{ get; set; }

        [Import("textAlign")]
        extern public string TextAlign{ get; set; }




    }
}