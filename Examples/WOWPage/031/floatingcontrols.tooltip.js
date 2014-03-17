function FloatingToolTip(name, tootip_width, tooltip_height, tooltip_color)
{

    var _mx = 0;
    var _my = 0;
    var _mwidth = tootip_width;
    var _mheight = tooltip_height;
    var _cursorpadding = 30;

    
    var _scrollEaserInProgress = false;
    var _scrollEaser = new Easing.Easer({ type: 'linear', side: 'in' });
    var _scrollEaserStart = null;
    var _scrollEaserEnd = null;
    var _scrollEaserDelta = 0;


    this.Initialize = function ()
    {
        Dbg.Print("PageNavigator Initialize ");

    }

    this.Update = function (tick)
    {
    }

    this.StartShow = function ()
    {
        _scrollEaserStart = this.Now();
        _scrollEaserEnd = _scrollEaserStart + 0.5 * 1000;  //500 millseconds
        _scrollEaserInProgress = true;
    }

    this.StopShow = function ()
    {
        _scrollEaserInProgress = false;
        _scrollEaserDelta = 0;
    }

    this.Draw = function (surface)
    {



        if (Experience.Instance.ShowToolTip)
        {

            if (_scrollEaserDelta == 0)
            {
                this.StartShow();
            }

            var e = _scrollEaser,
                    now = this.Now() - _scrollEaserStart,
                    end = _scrollEaserEnd - _scrollEaserStart;

            _scrollEaserDelta = e.ease(now, 0.05, 1, end);



            var newx1 = 0, newy1 = 0;
            var newx2 = 0, newy2 = 0;
            var newx3 = 0, newy3 = 0;
            var newx4 = 0, newy4 = 0;


            surface.save();


            surface.globalAlpha = _scrollEaserDelta;


            //RECTANGLE
            surface.fillStyle = this.BackgroundColor;

            if (Experience.Instance.GetMousePositionReal().x > (Experience.Instance.Width / 2))
            {
                if (Experience.Instance.GetMousePositionReal().y > (Experience.Instance.Height / 2))
                {
                    //bottom right
                    newx1 = this.X + _mx + Experience.Instance.GetViewportX() - _mwidth - _cursorpadding;
                    newy1 = this.Y + _my + Experience.Instance.GetViewportY() - _mheight - _cursorpadding;



                    //LINE
                    newx2 = _mx - 7 + Experience.Instance.GetViewportX();
                    newy2 = _my - 7;
                    newx3 = _mx - _cursorpadding - 3 + Experience.Instance.GetViewportX();
                    newy3 = _my - _cursorpadding - 3;




                } else
                {
                    //top right
                    newx1 = this.X + _mx + Experience.Instance.GetViewportX() - _mwidth - _cursorpadding;
                    newy1 = this.Y + _my + Experience.Instance.GetViewportY() + _cursorpadding;



                    //LINE
                    newx2 = _mx - 7 + Experience.Instance.GetViewportX();
                    newy2 = _my + 7;
                    newx3 = _mx - _cursorpadding - 3 + Experience.Instance.GetViewportX();
                    newy3 = _my + _cursorpadding + 3;


                }


            }
            else
            {
                if (Experience.Instance.GetMousePositionReal().y > (Experience.Instance.Height / 2))
                {
                    //bottom left
                    newx1 = this.X + _mx + Experience.Instance.GetViewportX() + _cursorpadding;
                    newy1 = this.Y + _my + Experience.Instance.GetViewportY() - _mheight - _cursorpadding;



                    //LINE
                    newx2 = _mx + 7 + Experience.Instance.GetViewportX();
                    newy2 = _my - 7;
                    newx3 = _mx + _cursorpadding + 3 + Experience.Instance.GetViewportX();
                    newy3 = _my - _cursorpadding - 3

                } else
                {
                    //top left
                    newx1 = this.X + _mx + Experience.Instance.GetViewportX() + _cursorpadding;
                    newy1 = this.Y + _my + Experience.Instance.GetViewportY() + _cursorpadding;


                    //LINE
                    newx2 = _mx + 7 + Experience.Instance.GetViewportX();
                    newy2 = _my + 7;
                    newx3 = _mx + _cursorpadding + 3 + Experience.Instance.GetViewportX();
                    newy3 = _my + _cursorpadding + 3

                }


            }

            //DOT
            newx4 = _mx + Experience.Instance.GetViewportX();
            newy4 = _my;



            //RECTANGLE
            surface.fillRect(newx1, newy1, _mwidth, _mheight);

            //LINE
            surface.beginPath();
            surface.lineWidth = 5;
            surface.moveTo(newx2, newy2);
            surface.lineTo(newx3, newy3);
            surface.strokeStyle = this.BackgroundColor;
            surface.stroke();

            //DOT
            surface.beginPath();
            surface.arc(newx4, newy4, 10, 0, 2 * Math.PI, false);
            surface.lineWidth = 5;
            surface.strokeStyle = this.BackgroundColor;
            surface.stroke();



            surface.restore();



        } else
        {
            this.StopShow();
        }
    }

    this.MouseOver = function ()
    {
        return false;
    }

    this.MouseOut = function ()
    {
        return false;
    }

    this.MouseDown = function ()
    {
        
        return false;
    }

    this.MouseUp = function ()
    {
        return false;
    }

    this.RootCanvasMouseMove = function ()
    {
        _mx = this.MouseX;
        _my = this.MouseY;




        return false;
    }
    

//    this.HitTest = function ()
//    {
//        return false;
//    }

    this.Unload = function ()
    {

    }

    FloatingControlBase.call(this); // inherit base

    this.Name = name;
    this.AlwaysActive = true;
    this.BackgroundColor = tooltip_color;
    
}
