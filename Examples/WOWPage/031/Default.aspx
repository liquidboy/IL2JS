<%@ Page Language="C#" AutoEventWireup="true" Inherits="webWinDesignerDeveloper._031.Default" %>
<%@ Register src="../AllPagesNavigation.ascx" tagname="AllPagesNavigation" tagprefix="uc1" %>
<%@ Register src="../Analytics.ascx" tagname="Analytics" tagprefix="uc2" %>
<%@ Register src="../BackHeader.ascx" tagname="BackHeader" tagprefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Win8 Dashboard - Step 28 - Calendar Control (part 5) - Calendar Chooser using 'SingleSwipeList' controls</title>
    <meta http-equiv="X-UA-Compatible" content="IE=9" />
    <meta name="application-name" content="Windows 8 Dashboard" />
    <meta name="msapplication-tooltip" content="Building the Win8 Dashboard using HTML5 Canvas" />
    <meta name="msapplication-navbutton-color" content="#00CCFF" />
    

    <script src="utils.frameratecounter.js" type="text/javascript"></script>
    <script src="utils.interpolation.js" type="text/javascript"></script>
    <script src="utils.textdraw.js" type="text/javascript"></script>
    <script src="utils.image.js" type="text/javascript"></script>
    <script src="views.pagebase.js" type="text/javascript"></script>
    <script src="views.pagex.js" type="text/javascript"></script>
    <script src="controls.controlbase.js" type="text/javascript"></script>
    <script src="controls.rectangle.js" type="text/javascript"></script>
    <script src="controls.toggleonoff.js" type="text/javascript"></script>
    <script src="controls.imagebasic.js" type="text/javascript"></script>
    <script src="controls.rectangleanimated.js" type="text/javascript"></script>
    <script src="controls.rectangleanimatedwithtext.js" type="text/javascript"></script>
    <script src="controls.imageanimated.js" type="text/javascript"></script>
    <script src="controls.textanimated.js" type="text/javascript"></script>
    <script src="controls.silverlighthost.js" type="text/javascript"></script>
    <script src="controls.canvashost.js" type="text/javascript"></script>
    <script src="controls.framehost.js" type="text/javascript"></script>
    <script src="controls.videobasic.js" type="text/javascript"></script>
    <script src="controls.videocontrolsbasic.js" type="text/javascript"></script>
    <script src="controls.elasticbutton.js" type="text/javascript"></script>
    <script src="controls.singleswipelist.js" type="text/javascript"></script>
    <script src="controls.elasticrectangleanimated.js" type="text/javascript"></script>
    <script src="controls.calendar.month.js" type="text/javascript"></script>
    <script src="controls.calendar.day.js" type="text/javascript"></script>
    <script src="controls.behaviors.clickanimation.js" type="text/javascript"></script>
    
    <script src="floatingcontrols.controlbase.js" type="text/javascript"></script>
    <script src="floatingcontrols.pagenavigator.js" type="text/javascript"></script>
    <script src="floatingcontrols.label.js" type="text/javascript"></script>
    <script src="floatingcontrols.buttonimage.js" type="text/javascript"></script>
    <script src="floatingcontrols.tooltip.js" type="text/javascript"></script>
    
    <script src="animation.easing.js" type="text/javascript"></script>
    <script src="animation.storyboard.js" type="text/javascript"></script>
    <script src="animation.animationengine.js" type="text/javascript"></script>
    
    <script src="threed.library.jsgl.js" type="text/javascript"></script>
    <script src="threed.planeprojection.js" type="text/javascript"></script>
    
    <script src="experience.preloader.js" type="text/javascript"></script>
    <script src="experience.dbg.js" type="text/javascript"></script>
    <script src="experience.js" type="text/javascript"></script>
    
    <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.ba-dotimeout.min.js" type="text/javascript"></script>
    <script src="/Scripts/modernizr.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Scripts/Silverlight.js"></script>
    <script type="text/javascript" src="assets/LiquidParticles.js"></script>
    
    <script src="application.configuration.js" type="text/javascript"></script>
    <script src="application.appcontainer.js" type="text/javascript"></script>
    <script src="application.bootstrap.js" type="text/javascript"></script>


    <style type="text/css">
        .AudioOn{display:none;}
        #browserMessage
        {
            display:none;
        }
        
        #container
        {
            background-color:#260930;
            width:100%;
            height:610px;
            margin-top:30px;
        }
        
        canvas
        {
            width:100%;
            height:610px;
            display:block;
            background-color:transparent;
        }
        
        
        
        .desc
        {
            padding-top:5px;
            padding-bottom:5px;
            font-family:Segoe UI Verdana Times New Roman;
            font-size:18px;
        }
        .desc a
        {
            font-family:Segoe UI Verdana Times New Roman;
            font-size:16px;
            text-decoration: none;
            background-color:Yellow;
        }
        .desc a:hover
        {
            text-decoration: underline;
        }
        
        
        
        /* no-faceface fallback */
        .no-fontface h1 {
	        font-family: Arial, Helvetica, sans-serif;
	        font-weight: normal;
	        font-size: 16px;
	        letter-spacing: 0;
	        text-transform: none;
        }


        .canvashost
        {
            background-color:#64FF00;
            width:100%;
            height:100%;
        }
        .framehost
        {
            background-color:#FF8B00;
            width:100%;
            height:100%;
            frameborder:0;
            border:0;
        }
        .videohost
        {
            background-color:#FF8B00;
            width:100%;
            height:100%;
            border:0;
        }
    </style>
    <uc2:Analytics ID="ctrlAnalytics" runat="server" />

    <script language="javascript">
        function requestToLoadApplication(x,a,b,c)
        {
            AppContainer.Instance.LoadApplication(x,a,b,c);
            if(x==1) $("#divApplicationNavigation").css('display', 'block');
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <uc3:BackHeader ID="BackHeader1" runat="server" />
    <div>
        <div id="container">

            <div id="canvasAlignmentHelper">


                <div id="outroContainerBG" class="outroContainer">
                </div>

                <div style="width:100%;" >
                    <canvas id="Canvas"></canvas>
                </div>

                <div style="float:right;margin-left:10px;margin-top:10px;"><div class="stp">28</div></div>

                <div style="width:100%;margin-top:10px; " class="desc">
                    Not much needs explaing with this new FloatingControl 'ToolTip'. As the name suggests if 
                    a control has a tooltip defined it will show this ToolTip control and render the tooltip
                    content within it.
                </div>

                <div style="width:100%;margin-top:10px; " class="desc">
                   Here is the basic architecture of a tooltip, from the floating control to 
                   how it is globally hidden/shown via the experience variable.  [<a href="/031/assets/ref001.jpg" target="_blank">reference image</a>]
                </div>

                <div style="width:100%;margin-top:10px; " class="desc">
                   In the Floating.ToolTip.js control itself there is logic to determine in which quadrant to
                   display the tooltip rectangle. This logic tries to render the tooltip as close to the center
                   of the screen as possible. 
                </div>

                <div style="width:100%;margin-top:10px; " class="desc">
                   Now that we have a tooltip we can wire this up to the other controls within the calendar. 
                   The calendar control has a very real need to show more information than what can be layed
                   out in the UI, hence the need for tooltips :)
                </div>


                <div style="float:left;width:100%;">
                    <uc1:AllPagesNavigation ID="AllPagesNavigation1" runat="server" />
                </div>

            </div>
        </div>

        <audio id="track1" src="audio/Tron Soundtrack 1a.mp3" loop></audio>
        <button id="audioToggle" class="AudioOn"></button>

        <div id="browserMessage">
            This site is best experienced with Internet Explorer 9 for Windows Vista and Windows 7.<br/><br/><a href="http://windows.microsoft.com/ie9" style="color:#85cbd8;" >Learn more about Internet Explorer 9</a>
        </div>

    </div>
    </form>
</body>
</html>
