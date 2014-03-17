function Experience()
{
    this.pages = [];
    this.floatingcontrols = [];

    this.DrawCallCount = 0; // live counter
    this.AnimationEngine = new AnimationEngine();    
    this.Width = 0;
    this.Height = 0;

    var mPanningActive = false;
    var mDrawDebugInfo = true;
    var mAutopilot = false;
    var mAutopilotY = false;

    var mLastMouseX = 0;
    var mViewportX = 0;
    var mLastMouseY = 0;
    var mViewportY = 0;
    var ViewportMax = 0;  //5500; // stop on the right edge
    var ViewportMaxY = 0;
    var ViewportMin = 0;
    var ViewportMinY = 0;



    var mSnapPosition = 300; // unused
    var mSnapEnabled = false;

    var mKeyboardVelocityX = 0;
    var mCurrentVelocityX = 0;
    var mCurrentDirectionX = 0;
    var mCurrentInertiaX = 0;
    var mKeyboardVelocityY = 0;
    var mCurrentVelocityY = 0;
    var mCurrentDirectionY = 0;
    var mCurrentInertiaY = 0;
    var mInertiaMaxTime = 1200; // msec
    var mInertiaMaxTimeY = 1200; // msec
    //var mInertiaTime = 0;
    var mbProcessInertia = false;
    var mbProcessInertiaY = false;

    var mViewportTargetX = 0;
    var mViewportTargetY = 0;
    var mLastMotionUpdate = 0;
    var mLastMotionUpdateY = 0;

    this.Timeline = 0;
    this.TimelineY = 0;
    this.CurrentVelocity = 0;
    this.CurrentVelocityY = 0;
    this.MouseDownCancelBubble = false;

    this.ShowToolTip  = false; //used as a global variable throught the app to show/hide tooltip

    var mMouseDragIndicator = Preloader.Open("/slices/mouse-drag.png");

    var mMousePointer = { "x": 0, "y": 0 };
    var mMousePointerReal = { "x": 0, "y": 0 };
    var mMousePointerDown = { "x": 0, "y": 0 };
    var mMouseDragOpacity = 0;
    var mMouseDragOpacityTarget = 0;

    //used in notify mouse up function to tell controls where mouse has moved (up, down, left, right etc)
    var mMouseDownXY = { "x": 0, "y": 0 };

    var mLastX = 0;
    var mLastY = 0;
    var mLastTime = 0;
    var mFrameLength = 0;

    // used to remove tiny movement and to align pixels to avoid sub-pixel rendering gaps
    var mRoundedViewportX = 0;
    var mRoundedViewportY = 0;


    this.AllowVerticalNavigation = false;


    this.PageCount = function ()
    {
        return Experience.Instance.pages.length;
    }

    var mActivePageIndexes = '';
    this.ActivePageCount = function ()
    {
        var counter = 0;
        mActivePageIndexs = "";
        for (var i = 0; i < Experience.Instance.pages.length; i++)
        {
            if (Experience.Instance.pages[i].IsActive) { counter++; mActivePageIndexes += i + ','; }
        }
        return counter;
    }

    this.ActivePageIndexes = function ()
    {
        return mActivePageIndexes;
    }

    this.PageExists = function (name) {
        for (var i = 0; i < Experience.Instance.pages.length; i++) {
            if (Experience.Instance.pages[i].Label == name) return true;
        }
        return false;
    }

    this.FloatingControlExists = function (name) {
        for (var i = 0; i < Experience.Instance.floatingcontrols.length; i++) {
            if (Experience.Instance.floatingcontrols[i].Name == name) return true;
        }
        return false;
    }

    

    this.GetActivePagesIndexs = function ()
    {
        var ret = [];
        for (var i = 0; i < Experience.Instance.pages.length; i++)
        {
            if (Experience.Instance.pages[i].IsActive) ret.push(i) ;
        }
        return ret.reverse();
    }

    this.GetPreviousActivePage = function (currentactiveindex)
    {
        for (var i = currentactiveindex-1; i >= 0; i--)
        {
            if (Experience.Instance.pages[i].IsActive) return Experience.Instance.pages[i];
        }
        return undefined;
    }

    this.Attach = function (page)
    {
        if (Experience.Instance.PageExists(page.Label)) return;

        Experience.Instance.pages.push(page);

        GrowViewportMaxX(page.Width);
        ViewportMaxY = page.Height > ViewportMaxY ? page.Height : ViewportMaxY;

        return page;
    }

    function GrowViewportMaxX(w)
    {
        ViewportMax += w - 190;  
    }

    this.AttachFloatingControl = function (floatingcontrol) {
        if (Experience.Instance.FloatingControlExists(floatingcontrol.Name)) return;


        Experience.Instance.floatingcontrols.push(floatingcontrol);

        return floatingcontrol;
    }

    this.ReplaceFloatingControl = function (fc)
    {
        for (var i = 0; i < Experience.Instance.floatingcontrols.length; i++)
        {
            if (Experience.Instance.floatingcontrols[i].Name == fc.Name)
            {
                Experience.Instance.floatingcontrols[i] = fc;
                return true;
            }
        }
        this.AttachFloatingControl(fc);

        return true;
    }

    this.Reset = function () {
        //nolonger clear, we just inactivate
        for (var i = 0; i < Experience.Instance.pages.length; i++) {
            Experience.Instance.pages[i].IsActive = false;
        }
        for (var i = 0; i < Experience.Instance.floatingcontrols.length; i++) {
            Experience.Instance.floatingcontrols[i].IsActive = false;
        }


//        Experience.Instance.pages = [];
//        Experience.Instance.floatingcontrols = [];
//        ViewportMax = 0;
//        ViewportMaxY = 0;
    }

    this.ShowApplication = function (appname, floatingcontrols)
    {

        ViewportMax = 0;
        for (var i = 0; i < Experience.Instance.pages.length; i++)
        {
            if (Experience.Instance.pages[i].AppName == appname)
            {
                GrowViewportMaxX(Experience.Instance.pages[i].Width);
                Experience.Instance.pages[i].IsActive = true;
            }
        }

        var items = floatingcontrols.split('|');
        for (var i = 0; i < items.length; i++)
        {
            for (var j = 0; j < Experience.Instance.floatingcontrols.length; j++)
            {
                if (Experience.Instance.floatingcontrols[j].Name == items[i])
                {
                    Experience.Instance.floatingcontrols[j].IsActive = true;
                    continue;
                }
            }
        }
    }

    this.ShowApplicationPages = function (appname, floatingcontrols, pages)
    {

        var pgs = pages.split(',');

        ViewportMax = 0;
        for (var i = 0; i < Experience.Instance.pages.length; i++)
        {
            if (Experience.Instance.pages[i].AppName == appname)
            {
                for (var j = 0; j < pgs.length; j++)
                {
                    if (Experience.Instance.pages[i].Label == pgs[j])
                    {
                        GrowViewportMaxX(Experience.Instance.pages[i].Width);
                        Experience.Instance.pages[i].IsActive = true;
                    }
                }
            }
        }

        var items = floatingcontrols.split(',');
        for (var i = 0; i < items.length; i++)
        {
            for (var j = 0; j < Experience.Instance.floatingcontrols.length; j++)
            {
                if (Experience.Instance.floatingcontrols[j].Name == items[i])
                {
                    Experience.Instance.floatingcontrols[j].IsActive = true;
                    continue;
                }
            }
        }
    }


    this.Init = function ()
    {
        Dbg.Print("Initializing Preloader..");

        //Preloader.OnReady = function () { Experience.Instance.Start(); };

        //Preloader.Init();

        this.AnimationEngine.Run();
    }

    this.Start = function ()
    {
        _UnInitCoreEvents();

        Dbg.Print("Loading Experience..");

        // lay everything out one after another

        for (var i = 0; i < Experience.Instance.pages.length; i++)
        {
            if (Experience.Instance.pages[i].IsActive)
            {
                var pa = this.GetPreviousActivePage(i);
                if (pa != undefined) Experience.Instance.pages[i].X = pa.X + pa.Width + 5 - Experience.Instance.pages[i].OverlapX;
                else Experience.Instance.pages[i].X = 0;
                //Experience.Instance.pages[i].X = Experience.Instance.pages[i - 1].X + Experience.Instance.pages[i - 1].Width + 5 - Experience.Instance.pages[i].OverlapX;
                Experience.Instance.pages[i].zIndex = i;
            }
        }

        // initialize all the pages
        for (var i = 0; i < Experience.Instance.pages.length; i++)
        {
            if (Experience.Instance.pages[i].IsActive)
                Experience.Instance.pages[i].Initialize();
        }

        // initialize all the floatingcontrols
        for (var i = 0; i < Experience.Instance.floatingcontrols.length; i++)
        {
            if (Experience.Instance.floatingcontrols[i].IsActive)
                Experience.Instance.floatingcontrols[i].Initialize();
        }


        // resort draw order based on optionally specified zIndex in Initialize() above
        // sometimes we want a page to draw on top of a page to its right
        Experience.Instance.pages.sort(function (a, b) { return a.zIndex - b.zIndex; });

        _InitCoreEvents();

        //ViewportMin = 1920 - Experience.Instance.Width;
        ViewportMin = 0;
        ViewportMinY = 0;
        mViewportTargetX = ViewportMin;
        mViewportTargetY = ViewportMinY;

        Dbg.Print("Viewport Min/Max (x): " + ViewportMin + "/" + ViewportMax);
        Dbg.Print("Viewport Min/Max (y): " + ViewportMinY + "/" + ViewportMaxY);

        Experience.Instance.StartX = mViewportTargetX;
        Experience.Instance.StartY = mViewportTargetY;

        $(document).focus();

    }

    // we want to us the mouse cursor for dragging, selection can screw that up
    this.PreventSelection = function (mouseEvent)
    {
        mouseEvent.preventDefault();
    }

    this.OnKeyDown = function (keyEvent)
    {
        var KeySensitivity = 1;

        if (keyEvent.keyCode == 38)
        {	// up key

            //mKeyboardVelocityX += KeySensitivity;
            mKeyboardVelocityY += KeySensitivity;

            keyEvent.preventDefault();
        }

        if (keyEvent.keyCode == 40)
        {	// up key

            //mKeyboardVelocityX += KeySensitivity;
            mKeyboardVelocityY -= KeySensitivity;

            keyEvent.preventDefault();
        }


        if (keyEvent.keyCode == 39)
        {	// right key

            mKeyboardVelocityX += KeySensitivity;
            //mKeyboardVelocityY += KeySensitivity;

            keyEvent.preventDefault();
        }

        if (keyEvent.keyCode == 37)
        {	// left key

            mKeyboardVelocityX -= KeySensitivity;
            //mKeyboardVelocityY -= KeySensitivity;

            keyEvent.preventDefault();
        }

        if (keyEvent.keyCode == 32)
        {	// space-bar auto-move toggle
            mAutopilot = !mAutopilot;
            mAutopilotY = !mAutopilotY;
            Dbg.Print("Toggled Tron Autopilot!");
        }

        if (keyEvent.keyCode == 68)
        {	// toggle drawing debug-info
            mDrawDebugInfo = !mDrawDebugInfo;
        }
    }

    this.SetKeyboardVelocityX = function (v)
    {
        mKeyboardVelocityX += v;
    }

    this.SetKeyboardVelocityY = function (v)
    {
        mKeyboardVelocityY += v;
    }

    this.GetMousePosition = function ()
    {
        return mMousePointer;
    }

    this.GetMousePositionReal = function ()
    {
        return mMousePointerReal;
    }

    this.GetMousePositionDown = function ()
    {
        return mMousePointerDown;
    }

    this.GetViewportX = function ()
    {
        return mViewportX;
    }

    this.GetViewportY = function ()
    {
        return mViewportY;
    }

    this.GetPanningActive = function ()
    {
        return mPanningActive;
    }

    this.OnMouseDown = function (mouseEvent)
    {
        //firefox issue with offset !!! grrrr
        var offX = 0, offY = 0;
        if (!mouseEvent.offsetX) offX = mouseEvent.layerX - $(mouseEvent.target).position().left;
        else offX = mouseEvent.offsetX;
        if (!mouseEvent.offsetY) offY = mouseEvent.layerY - $(mouseEvent.target).position().top;
        else offY = mouseEvent.offsetY;


        mLastMouseX = mouseEvent.pageX;
        mLastMouseY = mouseEvent.pageY;
        mCurrentInertiaX = 0;
        mCurrentVelocityX = 0;
        mLastMotionUpdate = 0; //new Date().getTime();
        mLastMotionUpdateY = 0; //new Date().getTime();

        mKeyboardVelocityX = 0; // mouse down resets any keyboard motion
        mKeyboardVelocityY = 0;

        mouseEvent.preventDefault();

        mMousePointer.x = offX;  //mouseEvent.offsetX;
        mMousePointer.y = offY; // mouseEvent.offsetY;

        mMousePointerDown.x = offX;  //mouseEvent.offsetX;
        mMousePointerDown.y = offY;  //mouseEvent.offsetY;

        mLastX = mViewportTargetX;
        mLastY = mViewportTargetY;

        var mouseDownHandled = false;

        //positioned here so that controls can intercept message and cancel the bubble to the code
        //that follows
        notifyControlsOfMouseDownEvents(offX, offY);

        // if mouse wasnt handled by any of the pages, use it for scrolling
        if (!mouseDownHandled)
        //if (!this.MouseDownCancelBubble)
        {
            mPanningActive = true;
            mMouseDragOpacityTarget = 1;
        }
    }

    this.OnMouseMove = function (mouseEvent)
    {
        //firefox issue with offset !!! grrrr
        var offX = 0, offY = 0;
        if (!mouseEvent.offsetX) offX = mouseEvent.layerX - $(mouseEvent.target).position().left;
        else offX = mouseEvent.offsetX;
        if (!mouseEvent.offsetY) offY = mouseEvent.layerY - $(mouseEvent.target).position().top;
        else offY = mouseEvent.offsetY;

        if (mPanningActive)
        {

            // the granularity of JS timers in IE9 is around 16msec
            // which means if we are getting 60+ mouse-move updates per second
            // once in a while we will get a near-zero timespan
            // which throws any velocity/inertia computation out the window
            var timeNow = new Date().getTime();
            //var deltaTime = timeNow - mLastMotionUpdate;

            mMouseDragOpacityTarget = 1;

            //Dbg.Print('pageX : ' + mouseEvent.pageX + ' offsetX : ' + offX); //mouseEvent.offsetX);
            //Dbg.Print('pageY : ' + mouseEvent.pageY + ' offsetY : ' + offY);  //mouseEvent.offsetY);

            // apply camera panning
            var newX = mouseEvent.pageX;
            var newY = mouseEvent.pageY;
            var deltaX = newX - mLastMouseX;
            var deltaY = newY - mLastMouseY;

            //Dbg.Print("deltaX : " + deltaX);

            mLastMouseX = newX;
            mLastMouseY = newY;
            mLastMotionUpdate = timeNow;
            mLastMotionUpdateY = timeNow;

            mMousePointer.x = offX;  //mouseEvent.offsetX;
            mMousePointer.y = offY; //mouseEvent.offsetY;

            mViewportTargetX -= deltaX;
            mViewportTargetY -= deltaY;


        }

        mMousePointerReal.x = offX; // mouseEvent.offsetX;
        mMousePointerReal.y = offY; // mouseEvent.offsetY;


        notifyControlsOfMouseMoveEvents(offX, offY); //mouseEvent.offsetX, mouseEvent.offsetY);
    }

    this.OnMouseLost = function (mouseEvent)
    {
        this.OnMouseUp(mouseEvent);
    }

    this.OnMouseEnter = function (mouseEvent)
    {
    }

    this.OnMouseUp = function (mouseEvent)
    {
        //firefox issue with offset !!! grrrr
        var offX = 0, offY = 0;
        if (!mouseEvent.offsetX) offX = mouseEvent.layerX - $(mouseEvent.target).position().left;
        else offX = mouseEvent.offsetX;
        if (!mouseEvent.offsetY) offY = mouseEvent.layerY - $(mouseEvent.target).position().top;
        else offY = mouseEvent.offsetY;

        if (mPanningActive)
        {
            mPanningActive = false;
            mCurrentInertiaX = Math.abs(mCurrentVelocityX);
            mCurrentInertiaY = Math.abs(mCurrentVelocityY);
            mInertiaMaxTime = 300 + mCurrentInertiaX * 500;
            mInertiaMaxTimeY = 300 + mCurrentInertiaY * 500;
            mbProcessInertia = true;
            mbProcessInertiaY = true;

            // decrease velocity; duration of last mousemove to this mouseup event
            // indicates a hold gesture
            var timeNow = new Date().getTime();

            var deltaTime = timeNow - mLastMotionUpdate;
            deltaTime = Math.max(10, deltaTime); // low-timer granularity compensation
            mLastMotionUpdate = 0;

            Dbg.Print("deltaTime : " + deltaTime);

            var deltaTimeY = timeNow - mLastMotionUpdateY;
            deltaTimeY = Math.max(10, deltaTimeY); // low-timer granularity compensation
            mLastMotionUpdateY = 0;

            // 100msec is a full hold gesture that complete zeroes out the velocity to be used as inertia
            mCurrentVelocityX *= 1 - Math.min(1, Math.max(0, deltaTime / 100));
            mCurrentVelocityY *= 1 - Math.min(1, Math.max(0, deltaTimeY / 100));
        }

        mLastX = 0;
        mLastY = 0;
        mMouseDragOpacityTarget = 0;

        $.doTimeout("clearmousedown", 50, function ()
        {
            mMousePointerDown.x = 0;
            mMousePointerDown.y = 0;
        });

        notifyControlsOfMouseUpEvents(offX, offY);
    }

    this.OnTouchDown = function (touchEvent) 
    {
        mLastMouseX = touchEvent.targetTouches[0].clientX;
        mCurrentInertiaX = 0;
        mCurrentVelocityX = 0;
        mLastMotionUpdate = 0; //new Date().getTime();

        mKeyboardVelocityX = 0; // touch down resets any keyboard motion

        mMousePointer.x = touchEvent.targetTouches[0].pageX;
        mMousePointer.y = touchEvent.targetTouches[0].pageY;

        mMousePointerDown.x = touchEvent.targetTouches[0].pageX;
        mMousePointerDown.y = touchEvent.targetTouches[0].pageY;

        mLastX = mViewportTargetX;

        var mouseDownHandled = false;

//        if (Experience.Instance.RinzlerCalloutActive || Experience.Instance.SamCalloutActive) {
//            mouseDownHandled = true;
//        }

        touchEvent.preventDefault();

        if(!mouseDownHandled){
        //if(!this.MouseDownCancelBubble){
            mPanningActive = true;
            mMouseDragOpacityTarget = 1;
        }
    }

    this.OnTouchMove = function (touchEvent) {
        if (mPanningActive) {
            // the granularity of JS timers in IE9 is around 16msec
            // which means if we are getting 60+ mouse-move updates per second
            // once in a while we will get a near-zero timespan
            // which throws any velocity/inertia computation out the window
            var timeNow = new Date().getTime();
            //var deltaTime = timeNow - mLastMotionUpdate;

            mMouseDragOpacityTarget = 1;

            // apply camera panning
            var newX = touchEvent.targetTouches[0].clientX;
            var deltaX = newX - mLastMouseX;

            mLastMouseX = newX;
            mLastMotionUpdate = timeNow;

            mMousePointer.x = touchEvent.targetTouches[0].pageX;
            mMousePointer.y = touchEvent.targetTouches[0].pageY;

            mViewportTargetX -= deltaX;
        }

        mMousePointerReal.x = touchEvent.targetTouches[0].pageX;
        mMousePointerReal.y = touchEvent.targetTouches[0].pageY;

        notifyControlsOfEvents(mMousePointerReal.x, mMousePointerReal.y);
    }

    this.OnTouchEnd = function (touchEvent) {

        if (mPanningActive) {
            mPanningActive = false;
            mCurrentInertiaX = Math.abs(mCurrentVelocityX);
            mInertiaMaxTime = 300 + mCurrentInertiaX * 500;
            mbProcessInertia = true;

            // decrease velocity; duration of last mousemove to this mouseup event
            // indicates a hold gesture
            var timeNow = new Date().getTime();
            var deltaTime = timeNow - mLastMotionUpdate;
            deltaTime = Math.max(10, deltaTime); // low-timer granularity compensation
            mLastMotionUpdate = 0;

            // 100msec is a full hold gesture that complete zeroes out the velocity to be used as inertia
            mCurrentVelocityX *= 1 - Math.min(1, Math.max(0, deltaTime / 100));
        }

        mLastX = 0;
        mMouseDragOpacityTarget = 0;

        $.doTimeout("clearmousedown", 50, function () {
            mMousePointerDown.x = 0;
            mMousePointerDown.y = 0;
        });

    }


    this.Update = function (frameLength)
    {
        //Dbg.Print('frameLength : ' + frameLength);
        mFrameLength = frameLength;
        var frameTick = frameLength / 60;

        if (mKeyboardVelocityX != 0)
        {
            mViewportX += mKeyboardVelocityX * frameLength;
            mViewportX = Math.min(ViewportMax, mViewportX);

            mViewportTargetX = mViewportX;
            mKeyboardVelocityX *= .94;
            if (Math.abs(mKeyboardVelocityX) < 0.001)
            {
                mKeyboardVelocityX = 0;
            }
        }

        if (mKeyboardVelocityY != 0)
        {
            mViewportY += mKeyboardVelocityY * frameLength;
            mViewportY = Math.min(ViewportMaxY, mViewportY);

            mViewportTargetY = mViewportY;
            mKeyboardVelocityY *= .94;
            if (Math.abs(mKeyboardVelocityY) < 0.001)
            {
                mKeyboardVelocityY = 0;
            }
        }


        if (mAutopilot && !mPanningActive && !mbProcessInertia)
        {
            mViewportTargetX += 2 * frameTick;
        }

        if (mAutopilotY && !mPanningActive && !mbProcessInertiaY)
        {
            mViewportTargetY += 2 * frameTick;
        }

        // pre-cap target viewport
        mViewportTargetX = Math.min(ViewportMax, Math.max(ViewportMin, mViewportTargetX));
        mViewportTargetY = Math.min(ViewportMaxY, Math.max(ViewportMinY, mViewportTargetY));

        if (mPanningActive) // builds current velocity
        {	// measure total user-input delta
            // we do this here because the MouseMove() can get called many times more often than Update()
            // particularly on systems with oversampled mice (eg: gaming rigs)

            var dX = mViewportTargetX - mLastX;
            mLastX = mViewportTargetX;

            // we track this for inertia's sake
            var velocity = Math.abs(dX);
            mCurrentVelocityX += (velocity - mCurrentVelocityX) * .3;
            mCurrentDirectionX = dX < 0 ? -1 : 1;


            var dY = mViewportTargetY - mLastY;
            mLastY = mViewportTargetY;

            // we track this for inertia's sake
            var velocityY = Math.abs(dY);
            mCurrentVelocityY += (velocityY - mCurrentVelocityY) * .3;
            mCurrentDirectionY = dY < 0 ? -1 : 1;

        }
        else
        {
            if (mbProcessInertia) // decreases current velocity
            {	// apply simple inertia

                mViewportTargetX += mCurrentVelocityX * mCurrentDirectionX;
                mCurrentVelocityX *= .9;

                Dbg.Print("mViewportTargetX: " + mViewportTargetX);
                Dbg.Print("mCurrentVelocityX: " + mCurrentVelocityX);

                if (mViewportTargetX < ViewportMin || mViewportTargetX > ViewportMax)
                {	// precap and cut inertia short because we hit a wall
                    mCurrentVelocityX = 0;
                    mbProcessInertia = 0;
                }

                if (mCurrentVelocityX < 0.01)
                {	// end inertia
                    mbProcessInertia = false;
                    mCurrentVelocityX = 0;
                }

            }

            if (mbProcessInertiaY) // decreases current velocity
            {	// apply simple inertia

                mViewportTargetY += mCurrentVelocityY * mCurrentDirectionY;
                mCurrentVelocityY *= .9;

                if (mViewportTargetY < ViewportMinY || mViewportTargetY > ViewportMaxY)
                {	// precap and cut inertia short because we hit a wall
                    mCurrentVelocityY = 0;
                    mbProcessInertiaY = 0;
                }

                if (mCurrentVelocityY < 0.01)
                {	// end inertia
                    mbProcessInertiaY = false;
                    mCurrentVelocityY = 0;
                }

            }
        }
        // at this point the current velocity has been computed and is ready for use
        this.CurrentVelocity = mCurrentVelocityX / frameLength;
        this.CurrentVelocityY = mCurrentVelocityY / frameLength;


        // catch up the viewport to the virtual viewport
        // this allows us to add smoothing really simply and consistently
        var smoothingFactor = 0.12; // [0.05,1] is a sensible range
        var speed = (mViewportTargetX - mViewportX) * smoothingFactor;
        mViewportX += speed;

        if (this.AllowVerticalNavigation)
        {
            var smoothingFactorY = 0.12; // [0.05,1] is a sensible range
            var speedY = (mViewportTargetY - mViewportY) * smoothingFactorY;
            mViewportY += speedY;
        } else mViewportY = 0;



        // this is the only viewport variable used by the draw system, and we round it for better rendering
        //mRoundedViewportX = Math.round(-mViewportX);
        mRoundedViewportX = -mViewportX;

        mRoundedViewportY = -mViewportY;


        // export the viewport translation and use it as a master timeline for all animations
        // we can offset it in any way necessary here
        Experience.Instance.Timeline = mViewportX;
        Experience.Instance.TimelineY = mViewportY;

        // update all of the visible pages
        for (var i = 0; i < Experience.Instance.pages.length; i++)
        {
            // degrade in case of unhandled errors by dropping broken pages
            if (Experience.Instance.pages[i].Broken) continue;
            if (!Experience.Instance.pages[i].IsActive) continue;

            try
            {
                // update page with latest timeline info
                Experience.Instance.pages[i].Update(frameLength);
            }
            catch (err)
            {	// if a page commits an error, remove it from circulation
                Experience.Instance.pages[i].Broken = true;
                Dbg.Print("Page " + Experience.Instance.pages[i].Label + " unhandled error in Update(): " + err);
            }
        }

        // the drag-indicator helper
        mMouseDragOpacity += (mMouseDragOpacityTarget - mMouseDragOpacity) * .1;


        // mousedown helper
        if (this.CurrentVelocity != 0)
        {
            mMousePointerDown.x = 0;
            mMousePointerDown.y = 0;
        }
    }

    this.ForceX = function (value)
    {
        mViewportTargetX = value;
    }

    this.ForceY = function (value)
    {
        mViewportTargetY = value;
    }


    //used to bubble events to page/control on events that matter
    //this is a very intense notifier, thousands of messages can potentially be bubbled around
    function notifyControlsOfMouseMoveEvents(x, y)
    {
        var cancelmouseoverbubble = false;
        var cancelmouseoutbubble = false;

        Experience.Instance.ShowToolTip = false;

        for (var i = 0; i < Experience.Instance.floatingcontrols.length; i++)
        {
            var fc = Experience.Instance.floatingcontrols[i];
            if (fc.IsActive && fc.IsVisible())
            {
                if (fc.AlwaysActive)
                {
                    cancelmouseoverbubble = fc.RootCanvasMouseMove(x, y);
                }
                else
                {
                    if (fc.HitTest(x, y))
                    {
                        if (!cancelmouseoverbubble) cancelmouseoverbubble = fc.MouseOver(x, y);
                        else fc.MouseOut(x, y);
                    } else
                    {
                        if (fc.HasMouseOver && !cancelmouseoutbubble) cancelmouseoutbubble = fc.MouseOut();
                    }

                }
            }
        }

        for (var i = 0; i < Experience.Instance.pages.length; i++) {
            var p = Experience.Instance.pages[i];
            if (p.IsActive && p.IsVisible()) {
                for (var j = 0; j < p.Controls.length; j++) {
                    var c = p.Controls[j];
                    if (c.IsVisible()) {
                        if (c.HitTest(x, y)) {
                            if (!cancelmouseoverbubble) cancelmouseoverbubble = c.MouseOver();
                            else c.MouseOut();
                        }
                        else {
                            if (c.HasMouseOver && !cancelmouseoutbubble) cancelmouseoutbubble = c.MouseOut();
                        }
                    }
                }
            }
        }

    }
    var mMouseDownHitControls = [];
    function notifyControlsOfMouseDownEvents(x, y)
    {
        var cancelmousedownbubble = false;
        mMouseDownXY.x = x;
        mMouseDownXY.y = y;

        for (var i = 0; i < Experience.Instance.floatingcontrols.length; i++)
        {
            var fc = Experience.Instance.floatingcontrols[i];
            if (fc.IsActive && fc.IsVisible())
            {
                if (fc.AlwaysActive)
                {
                    cancelmousedownbubble = fc.MouseDown(x, y);
                }
                else
                {
                    if (fc.HitTest(x, y))
                    {
                        if (!cancelmousedownbubble) cancelmousedownbubble = fc.MouseDown(x, y);
                        else fc.MouseUp(x, y);
                    }
                }
            }
        }


        for (var i = 0; i < Experience.Instance.pages.length; i++)
        {
            var p = Experience.Instance.pages[i];
            if (p.IsActive && p.IsVisible())
            {
                for (var j = 0; j < p.Controls.length; j++)
                {
                    var c = p.Controls[j];
                    if (c.IsVisible())
                    {
                        if (c.HitTest(x, y))
                        {
                            if (!cancelmousedownbubble) c.MouseDown();
                            else c.MouseUp(x, y);

                            mMouseDownHitControls.push(c);
                        }
                    }
                }
            }
        }
    }

    function notifyControlsOfMouseUpEvents(x, y)
    {
        var tempDeltaX = x - mMouseDownXY.x;
        var tempDeltaY = y - mMouseDownXY.y;

        var cancelmouseupbubble = false;

        for (var i = 0; i < Experience.Instance.floatingcontrols.length; i++)
        {
            var fc = Experience.Instance.floatingcontrols[i];
            if (fc.IsActive && fc.IsVisible())
            {
                if (fc.AlwaysActive)
                {
                    cancelmouseupbubble = fc.MouseUp(x, y);
                } 
                else
                {
                    if (fc.HitTest(x, y))
                    {
                        cancelmouseupbubble = fc.MouseUp(x, y);
                    }
                }
            }
        }


        for (var i = 0; i < Experience.Instance.pages.length; i++)
        {
            var p = Experience.Instance.pages[i];
            if (p.IsActive && p.IsVisible())
            {
                for (var j = 0; j < p.Controls.length; j++)
                {
                    var c = p.Controls[j];
                    if (c.HasMouseDown) //IsVisible())
                    {
                        if (c.HitTest(x, y))
                        {
                            cancelmouseupbubble = c.MouseUp(x, y);
                        } else
                        {
                            c.HasMouseDown = false; //reset that control because the mousedown has been lost
                        }

                        var cp = mMouseDownHitControls.pop();
                        while (cp != undefined)
                        {
                            if (Math.abs(tempDeltaY) > 10)
                            {
                                if (tempDeltaY < 0) cp.GestureUp();
                                else cp.GestureDown();
                            }
                            cp = mMouseDownHitControls.pop();
                        }
                        
                    }


                }
            }
        }
    }
    
    this.DrawPanelDebug = function (surface, panel)
    {
        surface.beginPath();
        surface.moveTo(0, 0);
        surface.lineTo(0, panel.Height - 1);
        surface.moveTo(0, panel.Height - 1);
        surface.lineTo(panel.Width - 1, panel.Height - 1);
        surface.moveTo(panel.Width - 1, panel.Height - 1);
        surface.lineTo(panel.Width - 1, 0);
        surface.moveTo(panel.Width - 1, 0);
        surface.lineTo(0, 0);
        surface.strokeStyle = "rgba(200,200,200,.5)";
        surface.stroke();
    }

    this.Draw = function (surface, surfaceWidth, surfaceHeight)
    {

        this.DrawCallCount = 0;

        // clear surface & prepare for rendering of all visible pages
        surface.clearRect(0, 0, surfaceWidth, surfaceHeight);
        surface.save();


        surface.translate(mRoundedViewportX, mRoundedViewportY); //20);



        for (var i = 0; i < Experience.Instance.pages.length; i++)
        {
            var panel = Experience.Instance.pages[i];

            // degrade in case of unhandled errors
            if (panel.Broken) continue;
            if (!panel.IsActive) continue;

            try
            {
                if (panel.IsVisible(mViewportX, mViewportY, this.Width, this.Height))
                {
                    panel.Draw(surface);
                }
            }
            catch (err)
            {	// this shouldn't ever happen as the base Draw() method wrapper will catch
                // all draw errors
                Experience.Instance.pages[i].Broken = true;
                Dbg.Print("Page " + Experience.Instance.pages[i].Label + " unhandled error in Draw(): " + err);
            }
        }

        for (var i = 0; i < Experience.Instance.floatingcontrols.length; i++)
        {
            var fltctl = Experience.Instance.floatingcontrols[i];

            if (!fltctl.IsActive) continue;
            
            try
            {
                if (fltctl.IsVisible(mViewportX, mViewportY, this.Width, this.Height))
                {
                    fltctl.Draw(surface);
                }
            }
            catch (err)
            {	// this shouldn't ever happen as the base Draw() method wrapper will catch
                // all draw errors
                Experience.Instance.pages[i].Broken = true;
                Dbg.Print("Floating Control " + fltctl.Label + " unhandled error in Draw(): " + err);
            }
        }
        



        TextDraw.RetainedDraw();
        surface.restore();

        surface.globalAlpha = 1;

        if (mDrawDebugInfo)
        {	// draw some debug data

            var roundedFPS = Math.round(mFpsCounter.GetFPS() * 100) / 100;

            var x = 30;
            var y = 30;
            var lineHeight = 18;

            Dbg.DrawString("Viewport (x): " + mViewportX.toFixed(2), x, y); y += lineHeight;
            Dbg.DrawString("Viewport (y): " + mViewportY.toFixed(2), x, y); y += lineHeight;
            Dbg.DrawString("Draw Calls: " + Experience.Instance.DrawCallCount, x, y); y += lineHeight;
            Dbg.DrawString("Draw Calls (/s): " + Math.round(Experience.Instance.DrawCallCount * roundedFPS) + "/sec", x, y); y += lineHeight;
            Dbg.DrawString("FPS: " + roundedFPS, x, y); y += lineHeight;
            Dbg.DrawString("Current Velocity (x): " + Experience.Instance.CurrentVelocity, x, y); y += lineHeight;
            Dbg.DrawString("Current Velocity (y): " + Experience.Instance.CurrentVelocityY, x, y); y += lineHeight;
            Dbg.DrawString("Timeline (x): " + (Experience.Instance.Timeline + Experience.Instance.Width).toFixed(2), x, y); y += lineHeight;
            Dbg.DrawString("Timeline (y): " + (Experience.Instance.TimelineY + Experience.Instance.Height).toFixed(2), x, y); y += lineHeight;
            Dbg.DrawString("MousePosition: " + Experience.Instance.GetMousePosition().x + " , " + Experience.Instance.GetMousePosition().y, x, y); y += lineHeight;
            Dbg.DrawString("MousePositionReal: " + Experience.Instance.GetMousePositionReal().x + " , " + Experience.Instance.GetMousePositionReal().y, x, y); y += lineHeight;
            Dbg.DrawString("MousePositionDown: " + Experience.Instance.GetMousePositionDown().x + " , " + Experience.Instance.GetMousePositionDown().y, x, y); y += lineHeight;
            Dbg.DrawString("Height & Width: " + this.Height + " | " + this.Width, x, y); y += lineHeight;

            //            var frameTick = mFrameLength / 60;
            //            Dbg.DrawString("FrameTick: " + frameTick, x, y); y += lineHeight;


            Dbg.Surface.font = "15pt DebugFont";
            Dbg.Surface.textBaseline = "top";
            Dbg.Surface.textAlign = "left";

            var str = "BUILD 2011.08.14.19.20.00";
            Dbg.DrawString(str, x, this.Height - lineHeight - 10);
        }

        surface.globalAlpha = mMouseDragOpacity;
        if (mMouseDragIndicator.ReadyForRendering)
            surface.drawImage(mMouseDragIndicator, mMousePointer.x - 37, mMousePointer.y + 35, mMouseDragIndicator.width, mMouseDragIndicator.height);
        surface.globalAlpha = 1;
    }

    function _InitCoreEvents()
    {
        $("Canvas").bind('mousedown', Experience.Instance.OnMouseDown);
        $("Canvas").bind('mouseup', Experience.Instance.OnMouseUp);
        $("Canvas").bind('mousemove', Experience.Instance.OnMouseMove);
        $("Canvas").bind('mouseout', Experience.Instance.OnMouseUp);
        $("Canvas").bind('mouseover', Experience.Instance.OnMouseEnter);
        $(document).bind('keydown', Experience.Instance.OnKeyDown);
        $(document).bind('mousedown', Experience.Instance.PreventSelection);


        $("Canvas").bind('touchstart', Experience.Instance.OnTouchDown);
        $("Canvas").bind('touchmove', Experience.Instance.OnTouchMove);
        $("Canvas").bind('touchend', Experience.Instance.OnTouchEnd);
        $("Canvas").bind('touchleave', Experience.Instance.OnTouchEnd);

    }

    function _UnInitCoreEvents()
    {
        $("Canvas").unbind('mousedown');
        $("Canvas").unbind('mouseup');
        $("Canvas").unbind('mousemove');
        $("Canvas").unbind('mouseout');
        $("Canvas").unbind('mouseover');
        $(document).unbind('keydown');
        $(document).unbind('mousedown');

        $("Canvas").unbind('touchstart');
        $("Canvas").unbind('touchmove');
        $("Canvas").unbind('touchend');
        $("Canvas").unbind('touchleave');
    }




}

Experience.Instance = new Experience();


