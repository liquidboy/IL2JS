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

        [Import("bezierCurveTo")]
        extern public void BezierCurveTo(double cp1x, double cp1y, double cp2x, double cp2y, double x, double y);

        [Import("closePath")]
        extern public void ClosePath();

        [Import("fillRect")]
        extern public string FillRect(int x, int y, int w, int h);

        [Import("fillStyle")]
        extern public string FillStyle { get; set; }


        [Import("fillText")]
        extern public void FillText(string str, int x, int y);


        [Import("globalAlpha")]
        extern public bool GlobalAlpha { get; set; }


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

        [Import("translate")]
        extern public void Translate(int x, int y);



        public void DrawEllipse(int x,int y,double w,double h) {
          double kappa = .5522848,
              ox = (w / 2) * kappa, // control point offset horizontal
              oy = (h / 2) * kappa, // control point offset vertical
              xe = x + w,           // x-end
              ye = y + h,           // y-end
              xm = x + w / 2,       // x-middle
              ym = y + h / 2;       // y-middle

          this.BeginPath();
          this.MoveTo(x, y);
          this.BezierCurveTo(x, ym - oy, xm - ox, y, xm, y);
          this.BezierCurveTo(xm + ox, y, xe, ym - oy, xe, ym);
          this.BezierCurveTo(xe, ym + oy, xm + ox, ye, xm, ye);
          this.BezierCurveTo(xm - ox, ye, x, ym + oy, x, ym);
          this.ClosePath();
          this.Stroke();
        }



    }
}