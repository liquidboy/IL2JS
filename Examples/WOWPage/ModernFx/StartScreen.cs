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


        double mLastMouseX;
        double mLastMouseY;
        double mCurrentInertiaX;
        double mCurrentInertiaY;
        public double mCurrentVelocityX;
        public double mCurrentVelocityY;
        double mLastMotionUpdateX;
        double mLastMotionUpdateY;

        double mMousePointerX;
        double mMousePointerY;
        double mMousePointerDownX;
        double mMousePointerDownY;
        public double mMousePointerRealX;
        public double mMousePointerRealY;

        bool mouseDownHandled;
        bool mPanningActive;
        bool mbProcessInertiaX;
        bool mbProcessInertiaY;

        double mMouseDragOpacityTarget;

        double mLastX;
        double mLastY;
        double mViewportTargetX;
        double mViewportTargetY;

        public double mInertiaMaxTimeX;
        public double mInertiaMaxTimeY;

        System.Windows.Threading.DispatcherTimer dt = new System.Windows.Threading.DispatcherTimer();

        public StartScreen()
        {
            _tracing = new Tracing();

            dt.Interval = TimeSpan.FromMilliseconds(50);
            dt.Tick += (e,t) => {
                mMousePointerDownX = 0;
                mMousePointerDownY = 0;
                dt.Stop();
            };

        }

        public void OnMouseDown(HtmlEvent mouseEvent)
        {
            //_tracing.DrawString("MOUSE DOWN x: " + mouseEvent.ClientX + " y: " + mouseEvent.ClientY, 20, 100);

            int offX = 0, offY = 0;
            offX = mouseEvent.OffsetX;
            offY = mouseEvent.OffsetY;

            mLastMouseX = mouseEvent.PageX;
            mLastMouseY = mouseEvent.PageY;
            mCurrentInertiaX = 0;
            mCurrentVelocityX = 0;
            mLastMotionUpdateX = 0; 
            mLastMotionUpdateY = 0;


            mouseEvent.PreventDefault();


            mMousePointerX = offX;  
            mMousePointerY = offY; 

            mMousePointerDownX = offX;  
            mMousePointerDownY = offY;  

            mLastX = mViewportTargetX;
            mLastY = mViewportTargetY;

            //positioned here so that controls can intercept message and cancel the bubble to the code
            //that follows
            //notifyControlsOfMouseDownEvents(offX, offY);

            // if mouse wasnt handled by any of the pages, use it for scrolling
            //if (!mouseDownHandled)
            //{
                mPanningActive = true;
                mMouseDragOpacityTarget = 1;
            //}



        }
        public void OnMouseUp(HtmlEvent mouseEvent)
        {
            //_tracing.DrawString("MOUSE UP x: " + mouseEvent.ClientX + " y: " + mouseEvent.ClientY, 20, 120);

             //firefox issue with offset !!! grrrr
            int offX = 0, offY = 0;
            offX = mouseEvent.OffsetX;
            offY = mouseEvent.OffsetY;

            if (mPanningActive)
            {
                mPanningActive = false;
                mCurrentInertiaX = Math.Abs(mCurrentVelocityX);
                mCurrentInertiaY = Math.Abs(mCurrentVelocityY);
                mInertiaMaxTimeX = 300 + mCurrentInertiaX * 500;
                mInertiaMaxTimeY = 300 + mCurrentInertiaY * 500;
                mbProcessInertiaX = true;
                mbProcessInertiaY = true;

                // decrease velocity; duration of last mousemove to this mouseup event
                // indicates a hold gesture
                var timeNow = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds ; //new Date().getTime();

                var deltaTimeX = timeNow - mLastMotionUpdateX;
                deltaTimeX = Math.Max(10, deltaTimeX); // low-timer granularity compensation
                mLastMotionUpdateX = 0;

                //Dbg.Print("deltaTime : " + deltaTime);

                var deltaTimeY = timeNow - mLastMotionUpdateY;
                deltaTimeY = Math.Max(10, deltaTimeY); // low-timer granularity compensation
                mLastMotionUpdateY = 0;

                // 100msec is a full hold gesture that complete zeroes out the velocity to be used as inertia
                mCurrentVelocityX *= 1 - Math.Min(1, Math.Max(0, deltaTimeX / 100));
                mCurrentVelocityY *= 1 - Math.Min(1, Math.Max(0, deltaTimeY / 100));
            }

            mLastX = 0;
            mLastY = 0;
            mMouseDragOpacityTarget = 0;



            dt.Start();

            //notifyControlsOfMouseUpEvents(offX, offY);

        }
        public void OnMouseMove(HtmlEvent mouseEvent)
        {
            //_tracing.DrawString("MOUSE MOVE x: " + mouseEvent.PageX + " y: " + mouseEvent.PageY, 20, 140);

            //firefox issue with offset !!! grrrr
            int offX = 0, offY = 0;
            offX = mouseEvent.OffsetX;
            offY = mouseEvent.OffsetY;

            if (mPanningActive)
            {

                // the granularity of JS timers in IE9 is around 16msec
                // which means if we are getting 60+ mouse-move updates per second
                // once in a while we will get a near-zero timespan
                // which throws any velocity/inertia computation out the window
                var timeNow = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
                //var deltaTime = timeNow - mLastMotionUpdate;

                mMouseDragOpacityTarget = 1;

                //Dbg.Print('pageX : ' + mouseEvent.pageX + ' offsetX : ' + offX); //mouseEvent.offsetX);
                //Dbg.Print('pageY : ' + mouseEvent.pageY + ' offsetY : ' + offY);  //mouseEvent.offsetY);

                // apply camera panning
                var newX = mouseEvent.PageX;
                var newY = mouseEvent.PageY;
                var deltaX = newX - mLastMouseX;
                var deltaY = newY - mLastMouseY;

                //Dbg.Print("deltaX : " + deltaX);

                mLastMouseX = newX;
                mLastMouseY = newY;
                mLastMotionUpdateX = timeNow;
                mLastMotionUpdateY = timeNow;

                mMousePointerX = offX;  //mouseEvent.offsetX;
                mMousePointerY = offY; //mouseEvent.offsetY;

                mViewportTargetX -= deltaX;
                mViewportTargetY -= deltaY;


            }

            mMousePointerRealX = offX; // mouseEvent.offsetX;
            mMousePointerRealY = offY; // mouseEvent.offsetY;


            //notifyControlsOfMouseMoveEvents(offX, offY); //mouseEvent.offsetX, mouseEvent.offsetY);


        }
        public void OnMouseOut(HtmlEvent mouseEvent)
        {
            //_tracing.DrawString("MOUSE OUT x: " + mouseEvent.ClientX + " y: " + mouseEvent.ClientY, 20, 160);
        }
        public void OnMouseOver(HtmlEvent mouseEvent)
        {
            //_tracing.DrawString("MOUSE OVER x: " + mouseEvent.ClientX + " y: " + mouseEvent.ClientY, 20, 180);
        }

    }
}
