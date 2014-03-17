function FloatingButtonImage(name, horizontalalignment, verticalalignment, paddingleft, paddingtop, paddingright, paddingbottom, url, click)
{
    var _slotwidth = 80;
    var _slotwidthgap = 10;
    var _slotheight = 80;
    var _colornormal = '#502357';
    var _colorover = '#8D3E99';

    var _pages = [];

    var stateOfLoading = 0; //0=nothing, 1=loading, 2=loaded
    var loadedImage = new Image();

    this.Initialize = function ()
    {
        Dbg.Print("PageNavigator Initialize ");

        this.Width = _slotwidth;
        this.Height = _slotheight;

        
    }



    this.Update = function (tick)
    {
    }

    this.Draw = function (surface)
    {

        if (stateOfLoading == 0 && (this.url != undefined || this.url != ""))
        {
            loadedImage.ib = this;
            loadedImage.src = this.ImageUrl;
            stateOfLoading = 1;
            loadedImage.onload = function ()
            {
                stateOfLoading = 2;
            };
        } else
        {
            if (stateOfLoading == 2)
            {

                surface.save();
                surface.globalAlpha = 1.0;

                if (this.VerticalAlignment == 'top')
                {
                    if (this.HorizontalAlignment == 'right')
                        surface.translate(this.X - metrics.width - this.PaddingRight, this.Y);
                    else if (this.HorizontalAlignment == 'left')
                    {
                        surface.beginPath();
                        surface.arc(this.X + (this.Width / 2) + this.PaddingLeft, this.Y + (this.Height / 2) + this.PaddingTop, this.Width / 2, 0, Math.PI * 2, false);
                        surface.lineWidth = '2.0';
                        surface.closePath();

                        surface.strokeStyle = "white";
                        surface.stroke();

                        surface.drawImage(loadedImage, this.X + this.PaddingLeft, this.Y + this.PaddingTop, _slotwidth, _slotheight);
                    }
                    else if (this.HorizontalAlignment == 'center')
                        surface.translate(this.PaddingLeft, this.Y);
                }


                //surface.fillRect(0, 0, _slotwidth, _slotheight);
                surface.restore();


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
        if(this.ClickCall!=undefined) eval(this.ClickCall);
        return true;
    }


    this.Unload = function ()
    {

    }

    FloatingControlBase.call(this); // inherit base

    this.HorizontalAlignment = horizontalalignment;
    this.VerticalAlignment = verticalalignment;
    this.Name = name;

    this.PaddingTop = paddingtop;
    this.PaddingBottom = paddingbottom;
    this.PaddingLeft = paddingleft;
    this.PaddingRight = paddingright;
    this.ImageUrl = url;
    this.ClickCall = click;
}
