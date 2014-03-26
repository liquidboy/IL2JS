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

        [Import("function(inst, aa, pr) { return inst.getContext('experimental-webgl', {antialias:aa, preserveDrawingBuffer: pr}); }", PassInstanceAsArgument = true)]
        extern public ContextWebGL ContextWebGl(bool antialias, bool preserveDrawingBuffer);
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


    [Import]
    public class ContextWebGL
    {
        [Import("viewportWidth")]
        extern public int ViewportWidth { get; set; }

        [Import("viewportHeight")]
        extern public int ViewportHeight { get; set; }


        [Import("createProgram")]
        extern public WebGLProgram CreateProgram();

        [Import(@"function(inst, id) { 

            var shaderScript = document.getElementById(id);
            if (!shaderScript) {
                return null;
            }

            var str = """";
            var k = shaderScript.firstChild;
            while (k) {
                if (k.nodeType == 3)
                    str += k.textContent;
                k = k.nextSibling;
            }

            var shader;
            if (shaderScript.type == ""x-shader/x-fragment"") {
                shader = inst.createShader(inst.FRAGMENT_SHADER);
            } else if (shaderScript.type == ""x-shader/x-vertex"") {
                shader = inst.createShader(inst.VERTEX_SHADER);
            } else {
                return null;
            }

            inst.shaderSource(shader, str);
            inst.compileShader(shader);

            if (!inst.getShaderParameter(shader, inst.COMPILE_STATUS)) {
                //alert(inst.getShaderInfoLog(shader));
                return null;
            }

            return shader;
        }", PassInstanceAsArgument = true)]
        extern public Shader GetShader(string id);


        [Import(@"function(inst, program, shader) { inst.attachShader(program, shader); }", PassInstanceAsArgument = true)]
        extern public void AttachShader(WebGLProgram program, Shader shader);

        [Import(@"function(inst, program){ inst.linkProgram(program); }", PassInstanceAsArgument = true)]
        extern public void LinkProgram(WebGLProgram program);

        [Import(@"function(inst, program){ return inst.getProgramParameter(program, inst.LINK_STATUS); }", PassInstanceAsArgument = true)]
        extern public bool GetLinkStatus(WebGLProgram program);

        [Import(@"function(inst, program){ return inst.getProgramInfoLog(program); }", PassInstanceAsArgument = true)]
        extern public string GetProgramInfoLog(WebGLProgram program);
    }

    [Import]
    public class Shader : HtmlElement
    {
        public Shader(JSContext ctxt) : base(ctxt) { }
    }

    [Import]
    public class WebGLProgram : HtmlElement
    {
        public WebGLProgram(JSContext ctxt) : base(ctxt) { }


    }
}