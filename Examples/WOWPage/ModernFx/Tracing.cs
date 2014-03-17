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

namespace WOWPage.ModernFx
{
    public class Tracing
    {
        public void DrawString(string data, int x, int y)
        {
            //Dbg.Surface.font = "10pt DebugFont";
            LibraryManager.DirectCanvas.Context.FontFamily = "10pt DebugFont";
            //Dbg.Surface.textBaseline = "top";
            LibraryManager.DirectCanvas.Context.TextBaseline = "top";
            //Dbg.Surface.textAlign = "left";
            LibraryManager.DirectCanvas.Context.TextAlign = "left";

            ////Dbg.Surface.fillStyle = "#000";
            ////Dbg.Surface.fillText(str, x + 1, y + 1);

            ////Dbg.Surface.fillStyle = "#fff";
            //Dbg.Surface.fillStyle = "#f00";
            LibraryManager.DirectCanvas.Context.FillStyle = "#f00";
            //Dbg.Surface.fillText(str, x, y);
            LibraryManager.DirectCanvas.Context.FillText(data, x, y);
        }
    }
}
