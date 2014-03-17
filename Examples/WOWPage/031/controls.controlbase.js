
function ControlBase()
{

    this.Slot = null;
    this.SlotCell = null;
    this.ParentPageX = 0;
    this.ParentPageY = 0;
    this.ParentPage = null;
    this.StoryboardOnLoad = null;

    this.MouseX = 0;
    this.MouseY = 0;
    this.MouseDeltaX = 0;
    this.MouseDeltaY = 0;

    var isInitialized = false;
    this.Derived_Initialize = this.Initialize;
    this.Initialize = function ()
    {
        if (!isInitialized)
        {
            Dbg.Print("ControlBase::Initialize();");

            if (this.StoryboardOnLoad != null 
                || this.StoryboardOnLoad != undefined)
                this.StoryboardOnLoad.Init(this);

            this.Derived_Initialize();
            isInitialized = true;
        }
    }

    this.Derived_Update = this.Update;
    this.Update = function (frameLengthMsec)
    {
        this.Derived_Update(frameLengthMsec);
    }

    this.Derived_Unload = this.Unload;
    this.Unload = function ()
    {
        Dbg.Print("ControlBase::Unload();");

        if (_visibilityChanged)
        {
            if (this.StoryboardOnLoad != undefined)
            {
                if (_isVisible)
                {
                    this.StoryboardOnLoad.Reset();
                    this.StoryboardOnLoad.IsPaused = false;
                }
                else
                {
                    this.StoryboardOnLoad.IsPaused = true;
                    this.StoryboardOnLoad.Reset();
                }
            }
            _visibilityChanged = false;
        }


        this.Derived_Unload();

    }

    this.Derived_Draw = this.Draw;
    this.Draw = function (surface) {
        
        surface.save();
        try {	// we can restore the drawing context here
            // so if the derived class messes up, this shouldn't destroy the entire experience
            this.DrawImpl(surface);
        }
        catch (err) {
            Dbg.Print("Control " + this.Label + " unhandled inner-draw error: " + err);
            
            this.Broken = true;
        }
        surface.restore();

    }
    this.DrawImpl = function (surface)
    {
        this.Derived_Draw(surface);
    }

    var _lastIsVisible = false;
    var _visibilityChanged = false;
    var _isVisible = false;
    this.IsVisible = function (x, y, w, h)
    {

        // if within bounds or close-enough to be within bounds
        //Dbg.Print(this.ParentPageX + " | " +  (parseFloat(this.SlotCell.vpx1) + parseFloat(this.SlotCell.width)) + " | " + x + " | " + this.SlotCell.vpx1 + " | " + (x + w));
        //Dbg.Print(parseFloat(this.SlotCell.vpx1) + " | " + (x + w) + " | " + this.ParentPageX);
        if (
            (parseFloat(this.SlotCell.vpx1) + parseFloat(this.SlotCell.width)) < 0
            || (parseFloat(this.SlotCell.vpx1) > Experience.Instance.Width)
        )
        {
            _isVisible = false;
        }
        else
        {
            _isVisible = true;
        }


        //Dbg.Print("false");
        //this.Derived_Unload();
        if (!_isVisible)
        {
            this.Derived_Unload();
            lastVisibility = false;
            this.HasMouseOver = false;
        }

        if (_lastIsVisible != _isVisible)
        {
            _visibilityChanged = true;
            this.Unload();
        }

        _lastIsVisible = _isVisible;

        return _isVisible;

    }
    
    this.HitTest = function (x, y) {
        if (x >= ((this.X() + this.ParentPageX) - Experience.Instance.GetViewportX())
            && x <= ((this.X() + this.ParentPageX) - Experience.Instance.GetViewportX() + this.Width())
            && y >= ((this.Y() + this.ParentPageY) - Experience.Instance.GetViewportY())
            && y <= ((this.Y() + this.ParentPageY) - Experience.Instance.GetViewportY() + this.Height())) {
            return true;
        }

        return false;
    }

    this.HasMouseOver = false;
    this.Derived_MouseOver = this.MouseOver;
    this.MouseOver = function () {
        this.HasMouseOver = true;
        if (this.Derived_MouseOver != undefined) this.Derived_MouseOver();
    }

    this.HasMouseDown = false;
    this.Derived_MouseDown = this.MouseDown;
    this.MouseDown = function ()
    {
        this.HasMouseDown = true;
        Dbg.Print('ControlBase - MouseDown');
        if (this.Derived_MouseDown != undefined) this.Derived_MouseDown();
    }

    this.Derived_MouseUp = this.MouseUp;
    this.MouseUp = function (x, y)
    {
        this.HasMouseDown = false;
        Dbg.Print('ControlBase - MouseUp');
        this.MouseX = x;
        this.MouseY = y;
        if (this.Derived_MouseUp != undefined) return this.Derived_MouseUp();
    }

    this.Derived_MouseOut = this.MouseOut;
    this.MouseOut = function () {
        this.HasMouseOver = false;
        if (this.Derived_MouseOut != undefined) this.Derived_MouseOut();
    }

    this.Derived_GestureUp = this.GestureUp;
    this.GestureUp = function ()
    {
        Dbg.Print('ControlBase - GestureUp');
        if (this.Derived_GestureUp != undefined) this.Derived_GestureUp();
    }

    this.Derived_GestureDown = this.GestureDown;
    this.GestureDown = function ()
    {
        Dbg.Print('ControlBase - GestureDown');
        if (this.Derived_GestureDown != undefined) this.Derived_GestureDown();
    }

    //it will be up to a implementing control wether to update the slotcell click count or not
    this.ClickCountIncrement = function ()
    {
        //CLICK LOGIC
        if (this.SlotCell.clickedprocessing == 0)
        {
            Dbg.Print('ControlBase - ClickCountIncrement');
            this.SlotCell.clickedprocessing = 1;

            $.doTimeout(this.SlotCell.id, 100, function (state)
            {
                if (Experience.Instance.GetPanningActive() == false)
                {
                    state.clicked = state.clicked == 1 ? 0 : 1;
                }
                state.clickedprocessing = 0;
            }, this.SlotCell);
        }
    }
    this.ClickedOn = function () { return this.SlotCell.clicked == 0 ? false : true; }

    this.Now = function () {
        if (Date.now) return Date.now();
        else return (new Date().getTime());
    }


    this.X = function ()
    {
        var x = this.SlotCell.vpx1o - this.ParentPageX;
        if (this.StoryboardOnLoad != null)
        {
            if (this.StoryboardOnLoad.AnimDirection == 'lefttoright')
                x += this.StoryboardOnLoad.X - this.StoryboardOnLoad.AnimAreaX;
            else if (this.StoryboardOnLoad.AnimDirection == 'righttoleft')
                x += this.StoryboardOnLoad.X;
            else x += this.StoryboardOnLoad.X;
        }
        
        return x;
    }
    this.Y = function ()
    {
        var y = this.SlotCell.vpy1o - this.ParentPageY;
        if (this.StoryboardOnLoad != null)
        {
            if (this.StoryboardOnLoad.AnimDirection == 'bottomtotop')
                y += this.StoryboardOnLoad.Y;
            else if (this.StoryboardOnLoad.AnimDirection == 'toptobottom')
                y += this.StoryboardOnLoad.Y - this.StoryboardOnLoad.AnimAreaX;
            else y += this.StoryboardOnLoad.Y;
        }

        return y;
    }
    this.Width = function () { return this.SlotCell.width; }
    this.Height = function () { return this.SlotCell.height; }
    this.Opacity = function () { return this.StoryboardOnLoad.Opacity; }


}