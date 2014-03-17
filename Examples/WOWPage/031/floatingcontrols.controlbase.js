
function FloatingControlBase()
{

    //All these values are set by the page that implements this base
    this.Width = 0;
    this.Height = 0;
    this.X = 0; //relative to entire dashboard width
    this.Y = 0; //relative to entire dashboard height
    this.AbsoluteX = 0; //relative to visible width (no viewportx)
    this.AbsoluteY = 0; //relative to visible height (no viewporty)
    this.PaddingTop = 0;
    this.PaddingBottom = 0;
    this.PaddingLeft = 0;
    this.PaddingRight = 0;
    this.HorizontalAlignment = '';
    this.VerticalAlignment = '';
    this.OverlapX = 0;

    this.IsActive = false;
    this.Name = '';

    this.MouseX = 0;
    this.MouseY = 0;

    this.AlwaysActive = false;

    this.DisplayDebug = false;


    this.Now = function ()
    {
        if (Date.now) return Date.now();
        else return (new Date().getTime());
    }


    this.Derived_Initialize = this.Initialize;
    this.Initialize = function ()
    {

        Dbg.Print("FloatingControl.ControlBase::Initialize(); label = " + this.Label);

        _updateLocation(this);

        this.Derived_Initialize();

        
    }


    function _updateLocation(fc)
    {

        switch (fc.HorizontalAlignment)
        {
            case 'left': fc.X = 0 + Experience.Instance.GetViewportX(); fc.AbsoluteX = 0 + fc.PaddingLeft; break;
            case 'center': fc.X = ((Experience.Instance.Width - fc.Width) / 2) + Experience.Instance.GetViewportX(); fc.AbsoluteX = ((Experience.Instance.Width - fc.Width) / 2); break;
            case 'right': fc.X = (Experience.Instance.Width - fc.Width) + Experience.Instance.GetViewportX(); fc.AbsoluteX = (Experience.Instance.Width - fc.Width) + fc.PaddingRight; break;
        }

        switch (fc.VerticalAlignment)
        {
            case 'top': fc.Y = 0 + Experience.Instance.GetViewportY(); fc.AbsoluteY = 0 + fc.PaddingTop; break;
            case 'center': fc.Y = ((Experience.Instance.Height - fc.Height) / 2) + Experience.Instance.GetViewportY(); fc.AbsoluteY = ((Experience.Instance.Height - fc.Height) / 2); break;
            case 'bottom': fc.Y = (Experience.Instance.Height - fc.Height) + Experience.Instance.GetViewportY(); fc.AbsoluteY = (Experience.Instance.Height - fc.Height) + fc.PaddingBottom; break;
        }

    }


    this.Derived_Update = this.Update;
    this.Update = function (frameLengthMsec)
    {
        
        this.Derived_Update(frameLengthMsec);
    }

    this.Derived_Draw = this.Draw;
    this.Draw = function (surface)
    {

        surface.save();

        _updateLocation(this);

        try
        {	// we can restore the drawing context here
            // so if the derived class messes up, this shouldn't destroy the entire experience
            this.DrawImpl(surface);
        }
        catch (err)
        {
            Dbg.Print("FloatingControl.ControlBase " + this.Label + " unhandled inner-draw error: " + err);
            this.Broken = true;
        }
        surface.restore();
    }

    this.DrawImpl = function (surface) {

        // xform into local space
        // so the page renderer doesn't need to worry about the timeline
        //surface.translate(this.X, this.Y);

        this.Derived_Draw(surface);
        
    }



    this.IsVisible = function ()
    {
//        // if within bounds or close-enough to be within bounds
//        if ((this.X + this.Width) < x || this.X > (x + w))
//            return false;

        return true;
    }



    this.HitTest = function (x, y)
    {
        //Dbg.Print(this.AbsoluteX + ' | ' + this.AbsoluteY);

        if (this.HorizontalAlignment == 'left')
        {
            if (x >= this.AbsoluteX
                    && x <= (this.AbsoluteX + this.Width)
                    && y >= this.AbsoluteY
                    && y <= (this.AbsoluteY + this.Height)
                   )
            {
                return true;
            }
        } else
        {
            if (x >= this.AbsoluteX
                    && x <= (this.AbsoluteX + this.Width)
                    && y >= this.AbsoluteY
                    && y <= (this.AbsoluteY + this.Height)
                   )
            {
                return true;
            }
        }

        return false;
    }

    this.Derived_RootCanvasMouseMove = this.RootCanvasMouseMove;
    this.RootCanvasMouseMove = function (x, y)
    {
        this.MouseX = x;
        this.MouseY = y;
        if (this.Derived_RootCanvasMouseMove != undefined) return this.Derived_RootCanvasMouseMove();
    }

    this.HasMouseOver = false;
    this.Derived_MouseOver = this.MouseOver;
    this.MouseOver = function (x, y)
    {
        this.HasMouseOver = true;
        this.MouseX = x;
        this.MouseY = y;
        if (this.Derived_MouseOver != undefined) return this.Derived_MouseOver();
    }


    this.Derived_MouseOut = this.MouseOut;
    this.MouseOut = function ()
    {
        this.HasMouseOver = false;
        if (this.Derived_MouseOut != undefined) return this.Derived_MouseOut();
    }


    this.HasMouseDown = false;
    this.Derived_MouseDown = this.MouseDown;
    this.MouseDown = function (x, y)
    {
        this.HasMouseDown = true;
        this.MouseX = x;
        this.MouseY = y;
        if (this.Derived_MouseDown != undefined) return this.Derived_MouseDown();
    }


    this.Derived_MouseUp = this.MouseUp;
    this.MouseUp = function (x, y)
    {
        this.HasMouseDown = false;
        this.MouseX = x;
        this.MouseY = y;
        if (this.Derived_MouseUp != undefined) return this.Derived_MouseUp();
    }
}
