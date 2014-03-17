function PageNavigator(name, horizontalalignment, verticalalignment)
{
    var _slotwidth = 30;
    var _slotwidthgap = 10;
    var _slotheight = 20;
    var _colornormal = '#502357';
    var _colorover = '#8D3E99';

    var _pages = [];



    this.Initialize = function ()
    {
        Dbg.Print("PageNavigator Initialize ");

        this.Width = (Experience.Instance.PageCount() * _slotwidth) + ((Experience.Instance.PageCount() - 1) * _slotwidthgap);
        this.Height = _slotheight;

        buildSlots(this);
    }

    function buildSlots(ctl)
    {
        _pages = [];
        var c = Experience.Instance.ActivePageCount();
        for (var i = 0; i < c; i++)
        {
            //var pg = Experience.Instance.pages[i];

            //absolute positioning (used for hit testing)
            var xa1 = ctl.AbsoluteX + (i * _slotwidth) + (i * _slotwidthgap);
            var xa2 = xa1 + _slotwidth;

            //relative to dashboard positioning (used to place on canvas)
            var xb1 = ctl.X + (i * _slotwidth) + (i * _slotwidthgap);
            var xb2 = xb1 + _slotwidth;

            _pages.push({
                x1: xb1,
                y1: ctl.Y + ctl.PaddingTop,
                x2: xb1 + _slotwidth,
                y2: ctl.Y + ctl.PaddingTop + _slotheight,

                xa1: xa1,
                xa2: xa2
            });

            //Dbg.Print("this.Opacity() : " + this.Opacity());
        }
    }

    function updateSlots(ctl)
    {
        
        var c = _pages.length;
        for (var i = 0; i < c; i++)
        {
            var pg = _pages[i];

            //absolute positioning (used for hit testing)
            var xa1 = ctl.AbsoluteX + (i * _slotwidth) + (i * _slotwidthgap);
            var xa2 = xa1 + _slotwidth;

            //relative to dashboard positioning (used to place on canvas)
            var xb1 = ctl.X + (i * _slotwidth) + (i * _slotwidthgap);
            var xb2 = xb1 + _slotwidth;


            pg.x1 = xb1;
            pg.y1 = ctl.Y + ctl.PaddingTop;
            pg.x2 = xb1 + _slotwidth;
            pg.y2 = ctl.Y + ctl.PaddingTop + _slotheight;

            pg.xa1 = xa1;
            pg.xa2 = xa2;

        }
    }



    this.Update = function (tick)
    {
    }

    this.Draw = function (surface)
    {
        if (this.IsVisible())
        {
            updateSlots(this);

            var c = _pages.length;
            for (var i = 0; i < c; i++)
            {

                surface.globalAlpha = 0.8;

                if (this.HasMouseOver)
                {
                    if (this.MouseX > _pages[i].xa1 && this.MouseX < _pages[i].xa2)
                    {
                        surface.fillStyle = _colorover;
                    } else
                    {
                        surface.fillStyle = _colornormal;
                    }
                } else
                {
                    surface.fillStyle = _colornormal;
                }

                surface.fillRect(_pages[i].x1, _pages[i].y1, _slotwidth, _slotheight);

                //Dbg.Print("this.Opacity() : " + this.Opacity());
            }
        }
    }

    this.MouseOver = function ()
    {
        return true;
    }

    this.MouseOut = function ()
    {
        return true;
    }

    this.MouseDown = function ()
    {
        return true;
    }

    this.MouseUp = function ()
    {
        for (var i = 0; i < _pages.length; i++)
        {
            var p = _pages[i];

            var roundedFPS = Math.round(mFpsCounter.GetFPS() * 100) / 100;
            
            Dbg.Print('roundedFPS : ' + roundedFPS);
            
            if (roundedFPS == Infinity || roundedFPS == 0) roundedFPS = 67;
            if (roundedFPS > 70) roundedFPS = 67;

            if (this.MouseX > p.xa1 && this.MouseX < p.xa2)
            {
                var parts = Experience.Instance.ActivePageIndexes().split(',');
                var pg = Experience.Instance.pages[parts[i]];

                //current X position
                var currX = Experience.Instance.GetViewportX();
                var delta = 0;
                if (pg.X < currX)
                {
                    //move left
                    delta = currX - pg.X;
                    delta = delta / roundedFPS;
                    delta = delta / 4.6;
                    delta = -1 * delta;

                } else if (pg.X > currX)
                {
                    //move right
                    delta = pg.X - currX;
                    delta = delta / roundedFPS;
                    delta = delta / 4.6;
                }

                if (delta != 0)
                {
                    Experience.Instance.SetKeyboardVelocityX(delta);
                    Experience.Instance.SetKeyboardVelocityY(-2);
                }

                continue;
            }
        }


        return true;
    }


//    this.HitTest = function ()
//    {
//        return false;
//    }

    this.Unload = function ()
    {

    }

    FloatingControlBase.call(this); // inherit base

    this.HorizontalAlignment = horizontalalignment;
    this.VerticalAlignment = verticalalignment;
    this.PaddingTop = 10;
    this.Name = name;
}
