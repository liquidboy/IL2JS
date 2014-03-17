function FloatingLabel(name, horizontalalignment, verticalalignment, title, titlefont, titlefillstyle, paddingleft, paddingtop, paddingright, paddingbottom)
{

    this.Initialize = function ()
    {
        Dbg.Print("PageNavigator Initialize ");

    }




    this.Update = function (tick)
    {
    }

    this.Draw = function (surface)
    {
        surface.save();
        surface.font = this.TitleFont;
        surface.globalAlpha = 1.0;
        surface.fillStyle = this.TitleFillStyle;

        var metrics = surface.measureText(this.Title);

        if (this.VerticalAlignment == 'top')
        {
            if (this.HorizontalAlignment == 'right')
                surface.translate(this.X - metrics.width - this.PaddingRight, this.Y);
            else if (this.HorizontalAlignment == 'left')
                surface.translate(this.PaddingLeft, this.Y);
            else if (this.HorizontalAlignment == 'center')
                surface.translate(this.PaddingLeft, this.Y);
        }

        surface.fillText(this.Title, 0, 0, 300);
        surface.restore();
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
    this.Name = name;

    this.Title = title;
    this.TitleFont = titlefont;
    this.TitleFillStyle = titlefillstyle;
    this.PaddingTop = paddingtop;
    this.PaddingBottom = paddingbottom;
    this.PaddingLeft = paddingleft;
    this.PaddingRight = paddingright;
    
}
