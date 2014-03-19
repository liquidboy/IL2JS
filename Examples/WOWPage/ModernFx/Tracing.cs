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

            //LibraryManager.DirectCanvas.Context.Restore();
            

            LibraryManager.DirectCanvas.Context.FontFamily = "10pt DebugFont";
            LibraryManager.DirectCanvas.Context.TextBaseline = "top";
            LibraryManager.DirectCanvas.Context.TextAlign = "left";

            LibraryManager.DirectCanvas.Context.FillStyle = "#f00";
            LibraryManager.DirectCanvas.Context.FillText(data, x, y);

            //LibraryManager.DirectCanvas.Context.Save();
            
        }
    }
}
