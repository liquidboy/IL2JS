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
    public class InfiniteLayout
    {
        Tracing _tracing;


        double mLastMouseX;
        double mLastMouseY;

        double mCurrentVelocityX;
        double mCurrentVelocityY;
        double mLastMotionUpdateX;
        double mLastMotionUpdateY;

        double mMousePointerDownX;
        double mMousePointerDownY;

        bool mouseDownHandled;
        bool mPanningActive;
        bool mbProcessInertiaX;
        bool mbProcessInertiaY;
        public bool AllowVerticalNavigation;

        double mLastX;
        double mLastY;
        public double ViewportTargetX;
        public double ViewportTargetY;

        double mCurrentDirectionX;
        double mCurrentDirectionY;
        double mViewportX;
        double mViewportY;

        public double X { get { return mViewportX * -1; } }
        public double Y { get { return mViewportY * -1; } }

        int mViewportMaxX ;
        int mViewportMaxY ;
        int mViewportMinX ;
        int mViewportMinY ;


        System.Windows.Threading.DispatcherTimer dt = new System.Windows.Threading.DispatcherTimer();

        public InfiniteLayout(int maxX, int maxY, int minX, int minY)
        {
            _tracing = new Tracing();

            dt.Interval = TimeSpan.FromMilliseconds(50);
            dt.Tick += (e,t) => {
                dt.Stop();
                mMousePointerDownX = 0;
                mMousePointerDownY = 0;
                
            };


            mViewportMaxX = maxX;
            mViewportMaxY = maxY;
            mViewportMinX = minX;
            mViewportMinY = minY;

        }

        public void OnMouseDown(HtmlEvent mouseEvent)
        {
            //_tracing.DrawString("MOUSE DOWN x: " + mouseEvent.ClientX + " y: " + mouseEvent.ClientY, 20, 100);

            int offX = 0, offY = 0;
            offX = mouseEvent.OffsetX;
            offY = mouseEvent.OffsetY;

            mLastMouseX = mouseEvent.PageX;
            mLastMouseY = mouseEvent.PageY;
            
            mCurrentVelocityX = 0;
            mLastMotionUpdateX = 0; 
            mLastMotionUpdateY = 0;


            mouseEvent.PreventDefault();

            mMousePointerDownX = offX;  
            mMousePointerDownY = offY;  

            mLastX = ViewportTargetX;
            mLastY = ViewportTargetY;


            mPanningActive = true;

        }
        public void OnMouseMove(HtmlEvent mouseEvent)
        {
            //_tracing.DrawString("MOUSE MOVE x: " + mouseEvent.PageX + " y: " + mouseEvent.PageY, 20, 140);

            if (mPanningActive)
            {

                // the granularity of JS timers in IE9 is around 16msec
                // which means if we are getting 60+ mouse-move updates per second
                // once in a while we will get a near-zero timespan
                // which throws any velocity/inertia computation out the window
                var timeNow = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
                
                // apply camera panning
                var newX = mouseEvent.PageX;
                var newY = mouseEvent.PageY;
                var deltaX = newX - mLastMouseX;
                var deltaY = newY - mLastMouseY;

                //_tracing.DrawString("deltaX : " + deltaX.ToString(), 20, 140);
                //_tracing.DrawString("deltaY : " + deltaY.ToString(), 20, 160);

                mLastMouseX = newX;
                mLastMouseY = newY;
                mLastMotionUpdateX = timeNow;
                mLastMotionUpdateY = timeNow;


                ViewportTargetX -= deltaX;
                ViewportTargetY -= deltaY;


            }



        }
        public void OnMouseUp(HtmlEvent mouseEvent)
        {
            //_tracing.DrawString("MOUSE UP x: " + mouseEvent.ClientX + " y: " + mouseEvent.ClientY, 20, 120);

            if (mPanningActive)
            {
                mPanningActive = false;
                mbProcessInertiaX = true;
                mbProcessInertiaY = true;

                // decrease velocity; duration of last mousemove to this mouseup event
                // indicates a hold gesture
                var timeNow = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds ; //new Date().getTime();

                var deltaTimeX = timeNow - mLastMotionUpdateX;
                deltaTimeX = Math.Max(10, deltaTimeX); // low-timer granularity compensation
                mLastMotionUpdateX = 0;
                
                var deltaTimeY = timeNow - mLastMotionUpdateY;
                deltaTimeY = Math.Max(10, deltaTimeY); // low-timer granularity compensation
                mLastMotionUpdateY = 0;

                // 100msec is a full hold gesture that complete zeroes out the velocity to be used as inertia
                mCurrentVelocityX *= 1d - Math.Min(1, Math.Max(0, deltaTimeX / 100d));
                mCurrentVelocityY *= 1d - Math.Min(1, Math.Max(0, deltaTimeY / 100d));

                //_tracing.DrawString("mCurrentVelocityX : " + mCurrentVelocityX.ToString(), 20, 120);
                //_tracing.DrawString("mCurrentVelocityY : " + mCurrentVelocityY.ToString(), 20, 140);
            }

            mLastX = 0;
            mLastY = 0;
            
            dt.Start();

        }
       
        public void OnMouseOut(HtmlEvent mouseEvent)
        {
            //_tracing.DrawString("MOUSE OUT x: " + mouseEvent.ClientX + " y: " + mouseEvent.ClientY, 20, 160);
        }
        public void OnMouseOver(HtmlEvent mouseEvent)
        {
            //_tracing.DrawString("MOUSE OVER x: " + mouseEvent.ClientX + " y: " + mouseEvent.ClientY, 20, 180);
        }



        public void Update()
        {

            // pre-cap target viewport
            ViewportTargetX = Math.Min(mViewportMaxX, Math.Max(mViewportMinX, ViewportTargetX));
            ViewportTargetY = Math.Min(mViewportMaxY, Math.Max(mViewportMinY, ViewportTargetY));

            if (mPanningActive) // builds current velocity
            {
                //X ======
                var dX = ViewportTargetX - mLastX;
                mLastX = ViewportTargetX;

                // we track this for inertia's sake
                var velocity = Math.Abs(dX);
                mCurrentVelocityX += (velocity - mCurrentVelocityX) * .3;
                mCurrentDirectionX = dX < 0 ? -1 : 1;

                //Y ======
                var dY = ViewportTargetY - mLastY;
                mLastY = ViewportTargetY;

                // we track this for inertia's sake
                var velocityY = Math.Abs(dY);
                mCurrentVelocityY += (velocityY - mCurrentVelocityY) * .3;
                mCurrentDirectionY = dY < 0 ? -1 : 1;

            }
            else
            {

                if (mbProcessInertiaX) // decreases current velocity
                {	// apply simple inertia

                    ViewportTargetX += mCurrentVelocityX * mCurrentDirectionX;
                    mCurrentVelocityX *= .9;

                    //Dbg.Print("mViewportTargetX: " + mViewportTargetX);
                    //Dbg.Print("mCurrentVelocityX: " + mCurrentVelocityX);

                    if (ViewportTargetX < mViewportMinX || ViewportTargetX > mViewportMaxX)
                    {	// precap and cut inertia short because we hit a wall
                        mCurrentVelocityX = 0;
                        mbProcessInertiaX = false;
                    }

                    if (mCurrentVelocityX < 0.01)
                    {	// end inertia
                        mbProcessInertiaX = false;
                        mCurrentVelocityX = 0;
                    }

                }

                if (mbProcessInertiaY) // decreases current velocity
                {	// apply simple inertia

                    ViewportTargetY += mCurrentVelocityY * mCurrentDirectionY;
                    mCurrentVelocityY *= .9;

                    if (ViewportTargetY < mViewportMinY || ViewportTargetY > mViewportMaxY)
                    {	// precap and cut inertia short because we hit a wall
                        mCurrentVelocityY = 0;
                        mbProcessInertiaY = false;
                    }

                    if (mCurrentVelocityY < 0.01)
                    {	// end inertia
                        mbProcessInertiaY = false;
                        mCurrentVelocityY = 0;
                    }

                }
            }
            // at this point the current velocity has been computed and is ready for use
            this.mCurrentVelocityX = mCurrentVelocityX / 15d; //15ms = framelength
            this.mCurrentVelocityY = mCurrentVelocityY / 15d;

            // catch up the viewport to the virtual viewport
            // this allows us to add smoothing really simply and consistently
            var smoothingFactor = 0.12; // [0.05,1] is a sensible range
            var speed = (ViewportTargetX - mViewportX) * smoothingFactor;
            mViewportX += speed;

            if (this.AllowVerticalNavigation)
            {
                var smoothingFactorY = 0.12; // [0.05,1] is a sensible range
                var speedY = (ViewportTargetY - mViewportY) * smoothingFactorY;
                mViewportY += speedY;
            }
            else mViewportY = 0;


        }

    }
}
