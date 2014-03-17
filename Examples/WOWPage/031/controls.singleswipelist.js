function SingleSwipeList(slot, hovercolor, bkgcolor, sbonload, onclick, imgurl, title, titlefont, titlex, titley, titlecolor, listitems, selectedindex)
{
    var laststate = -99;

    var bhButClick = new BehaviorClickAnimation(this);
    var stateOfLoading = 0; //0=nothing, 1=loading, 2=loaded
    var loadedImage = new Image();
    var _clickedState = false;
    var _items = listitems;
    var _selectedIndex = selectedindex;

    var _scrollEaserInProgress = false;
    var _scrollEaser = new Easing.Easer({ type: 'quadratic', side: 'both' });
    var _scrollEaserStart = null;
    var _scrollEaserEnd = null;
    var _scrollEaserDelta = 0;

    var _scrollDirection = '';
    
    var _hiddenTitle = '';
    var _hiddenTitleIndex = 0;
    var _hiddenTitleOpacity = 0;

    this.Initialize = function ()
    {
        this.zIndex += 3;
        Dbg.Print("ElasticButton Initialize " + slot);

        //if (this.Title == '[slot]') this.Title = this.SlotCell.id;
        if (this.Title == '[slot]') this.Title = '';
        this.Title = _items[_selectedIndex];


        _hiddenTitleIndex = _selectedIndex + 1;
        if (_hiddenTitleIndex >= _items.length) _hiddenTitleIndex = 0;
        if (_hiddenTitleIndex == -1) _hiddenTitleIndex = _items.length - 1;


        _hiddenTitle = _items[_hiddenTitleIndex];
    }

    this.MouseDown = function ()
    {
        Dbg.Print("ElasticButton - MouseDown");
    }

    this.MouseUp = function ()
    {
        Dbg.Print("ElasticButton - MouseUp : " + this.MouseDeltaX + ':' + this.MouseDeltaY);
        if (Experience.Instance.CurrentVelocity == 0) this.ClickCountIncrement();
        return true;
    }

    this.MouseOver = function ()
    {
        //Dbg.Print("ElasticButton - MouseOver");
        this.CurrentColor = this.HoverColor;
    }

    this.MouseOut = function ()
    {
        //Dbg.Print("ElasticButton - MouseOut");
        this.CurrentColor = this.BkgColor;
    }

    this.GestureUp = function ()
    {
        Dbg.Print("ElasticButton - GestureUp");

        _selectedIndex++;
        if (_selectedIndex >= _items.length) _selectedIndex = 0;

        _hiddenTitleIndex = _selectedIndex ;
        if (_hiddenTitleIndex >= _items.length) _hiddenTitleIndex = 0;
        _hiddenTitle = _items[_hiddenTitleIndex];

        _scrollEaserStart = this.Now();
        _scrollEaserEnd = _scrollEaserStart + 0.2 * 1000;
        _scrollEaserInProgress = true;
        _scrollDirection = 'up';

    }


    this.GestureDown = function ()
    {
        Dbg.Print("ElasticButton - GestureDown");

        _selectedIndex--;
        if (_selectedIndex == -1) _selectedIndex = _items.length - 1;

        _hiddenTitleIndex = _selectedIndex ;
        if (_hiddenTitleIndex == -1) _hiddenTitleIndex = _items.length - 1;
        _hiddenTitle = _items[_hiddenTitleIndex];

        _scrollEaserStart = this.Now();
        _scrollEaserEnd = _scrollEaserStart + 0.2 * 1000;
        _scrollEaserInProgress = true;
        _scrollDirection = 'down';
    }

    this.Update = function (tick)
    {

    }

    this.Clicked = function (state)
    {

        if (state == 1) //clicked
        {
            Dbg.Print("ElasticButton [slot " + slot + "] Clicked");
            if (onclick != undefined)
            {
                setTimeout(function () { eval(onclick); }, 500); //giving click animation some time to showoff
            }
        }
        else //reset
        {
            Dbg.Print("ElasticButton [slot " + slot + "] Reset to zero");
        }

    }

    this.Draw = function (surface)
    {

        if (this.IsVisible())
        {

            var co = this.ClickedOn();

            //Dbg.Print("ElasticButton [slot " + slot + "] " + co);

            bhButClick.CalculateDelta(co);

            if (co != _clickedState)
            {
                _clickedState = co;
                this.Clicked(co);
            }


            surface.fillStyle = this.CurrentColor;
            surface.globalAlpha = this.Opacity();
            surface.fillRect(this.X() + (bhButClick.Delta / 2), this.Y() + (bhButClick.Delta / 2), this.Width() - bhButClick.Delta, this.Height() - bhButClick.Delta);




            if (_scrollEaserInProgress)
            {
                var e = _scrollEaser,
                now = this.Now() - _scrollEaserStart,
                end = _scrollEaserEnd - _scrollEaserStart;

                if (now > end)
                {
                    _scrollEaserInProgress = false;
                    _scrollEaserDelta = 0;
                    this.Title = _items[_selectedIndex];
                    //_hiddenTitle = _items[_hiddenTitleIndex];
                    _hiddenTitleOpacity = 0;
                } else
                {
                    _scrollEaserDelta = e.ease(now, 0, this.Height(), end);
                    _hiddenTitleOpacity = _scrollEaserDelta / this.Height();
                    Dbg.Print('_scrollEaserDelta : ' + _scrollEaserDelta);
                }
            }



            if (this.Title != undefined && this.Opacity() > 0.50)
            {
                surface.font = this.TitleFont;
                if (this.TitleColor != undefined) surface.fillStyle = this.TitleColor;


                surface.globalAlpha = 1 - _hiddenTitleOpacity;  //this.Opacity();
                //surface.globalAlpha = this.Opacity();
                if (_scrollDirection == 'up') _scrollEaserDelta = _scrollEaserDelta * -1;
                surface.fillText(this.Title, this.X() + this.TitleX, this.Y() + this.TitleY + _scrollEaserDelta, this.Width());

                surface.globalAlpha = _hiddenTitleOpacity;
                if (_scrollDirection == 'up')
                    surface.fillText(_hiddenTitle, this.X() + this.TitleX, this.Y() + this.TitleY + _scrollEaserDelta + this.Height(), this.Width());
                else
                    surface.fillText(_hiddenTitle, this.X() + this.TitleX, this.Y() + this.TitleY + _scrollEaserDelta - this.Height(), this.Width());

                //surface.globalAlpha = 1;
            }




        }

    }

    this.Unload = function ()
    {

    }


    ControlBase.call(this); // inherit base

    this.ImageUrl = imgurl;
    this.Slot = slot;
    this.CurrentColor = bkgcolor;
    this.BkgColor = bkgcolor;
    this.HoverColor = hovercolor;
    this.StoryboardOnLoad = sbonload;

    this.Title = title;
    this.TitleColor = titlecolor;
    this.TitleFont = titlefont;
    this.TitleX = titlex;
    this.TitleY = titley;

    this.CropX = 0; // cropx;
    this.CropY = 0; // cropy;
    this.CropW = 600; //  cropw;
    this.CropH = 500; // croph;
}