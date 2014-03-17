function ElasticButton(slot, hovercolor, bkgcolor, sbonload, onclick, imgurl, title, titlefont, titlex, titley, titlecolor)
{
    var laststate = -99;

    var bhButClick = new BehaviorClickAnimation(this);
    var stateOfLoading = 0; //0=nothing, 1=loading, 2=loaded
    var loadedImage = new Image();
    var _clickedState = false;

    this.Initialize = function ()
    {
        this.zIndex += 3;
        Dbg.Print("ElasticButton Initialize " + slot);

        //if (this.Title == '[slot]') this.Title = this.SlotCell.id;
        if (this.Title == '[slot]') this.Title = '';
    }

    this.MouseDown = function ()
    {
        Experience.Instance.ShowToolTip = false;
        Dbg.Print("ElasticButton - MouseDown");
    }

    this.MouseUp = function ()
    {
        Dbg.Print("ElasticButton - MouseUp");
        Experience.Instance.ShowToolTip = true;
        if (Experience.Instance.CurrentVelocity == 0) this.ClickCountIncrement();
    }

    this.MouseOver = function ()
    {
        //Dbg.Print("ElasticButton - MouseOver");
        Experience.Instance.ShowToolTip = true;
        this.CurrentColor = this.HoverColor;
    }

    this.MouseOut = function ()
    {
        //Dbg.Print("ElasticButton - MouseOut");
        Experience.Instance.ShowToolTip = false;
        this.CurrentColor = this.BkgColor;
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


            //shadow
            //            surface.shadowOffsetX = 5;
            //            surface.shadowOffsetY = 5;
            //            surface.shadowBlur = 4;
            //            surface.shadowColor = 'rgba(0, 0, 0, .5)';

            surface.fillStyle = this.CurrentColor;
            surface.globalAlpha = this.Opacity();
            surface.fillRect(this.X() + (bhButClick.Delta / 2), this.Y() + (bhButClick.Delta / 2), this.Width() - bhButClick.Delta, this.Height() - bhButClick.Delta);



            //            if (stateOfLoading == 0 && (this.url != undefined || this.url != ""))
            //            {
            //                loadedImage.ib = this;
            //                loadedImage.src = this.ImageUrl;
            //                stateOfLoading = 1;
            //                loadedImage.onload = function ()
            //                {
            //                    stateOfLoading = 2;
            //                };
            //            } else
            //            {
            //                if (stateOfLoading == 2)
            //                    surface.drawImage(
            //                        loadedImage,
            //                        this.X() + (bhButClick.Delta / 2),
            //                        this.Y() + (bhButClick.Delta / 2),
            //                        this.Width() - bhButClick.Delta - 2,
            //                        this.Height() - bhButClick.Delta - 2
            //                    );
            //            }


            if (this.Title != undefined && this.Opacity() > 0.50)
            {
                surface.font = this.TitleFont;
                if (this.TitleColor != undefined) surface.fillStyle = this.TitleColor;
                surface.globalAlpha = this.Opacity();
                surface.fillText(this.Title, this.X() + this.TitleX, this.Y() + this.TitleY, this.Width());
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