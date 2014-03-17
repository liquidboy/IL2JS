using Microsoft.LiveLabs.Html;
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
    public class StartScreen
    {
        Tracing _tracing;

        public StartScreen()
        {
            _tracing = new Tracing();
        }

        public void OnMouseDown(HtmlEvent mouseEvent)
        {
            _tracing.DrawString("MOUSE DOWN x: " + mouseEvent.ClientX + " y: " + mouseEvent.ClientY, 20, 100);
        }
        public void OnMouseUp(HtmlEvent mouseEvent)
        {
            _tracing.DrawString("MOUSE UP x: " + mouseEvent.ClientX + " y: " + mouseEvent.ClientY, 20, 120);
        }
        public void OnMouseMove(HtmlEvent mouseEvent)
        {
            _tracing.DrawString("MOUSE MOVE x: " + mouseEvent.ClientX + " y: " + mouseEvent.ClientY, 20, 140);
        }
        public void OnMouseOut(HtmlEvent mouseEvent)
        {
            _tracing.DrawString("MOUSE OUT x: " + mouseEvent.ClientX + " y: " + mouseEvent.ClientY, 20, 160);
        }
        public void OnMouseOver(HtmlEvent mouseEvent)
        {
            _tracing.DrawString("MOUSE OVER x: " + mouseEvent.ClientX + " y: " + mouseEvent.ClientY, 20, 180);
        }

    }
}
