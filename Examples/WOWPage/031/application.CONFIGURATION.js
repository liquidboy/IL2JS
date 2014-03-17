function ApplicationConfiguration()
{
    this.LoadConfiguration = function (appnumber, param1, param2, param3) 
    {
        Experience.Instance.Reset();
        switch (appnumber)
        {
            case 1: ShowTooltip(); break;
        }
        Experience.Instance.Start();
    }

    function ShowTooltip()
    {
        Experience.Instance.AllowVerticalNavigation = false;

        //PAGES
        Experience.Instance.Attach(new PageX('sampletooltip', 7, 3
            , [1, 6, 16, 19]
            , [
                new ElasticButton(0, "#593D62", '#42244D', new Storyboard('quadratic', 'in', 1.5, 20, 'righttoleft', 0.5, 1),
                undefined,
                undefined, ':)', "normal 45px Segoe UI", 80, 60, 'white', true),


                new ElasticButton(1, "#593D62", '#42244D', new Storyboard('quadratic', 'in', 1.5, 20, 'righttoleft', 0.5, 1),
                undefined,
                undefined, ':o', "normal 45px Segoe UI", 80, 60, 'white', true),


                new ElasticButton(2, "#593D62", '#42244D', new Storyboard('quadratic', 'in', 1.5, 20, 'righttoleft', 0.5, 1),
                undefined,
                undefined, ':p', "normal 45px Segoe UI", 80, 60, 'white', true),


                new ElasticButton(3, "#593D62", '#42244D', new Storyboard('quadratic', 'in', 1.5, 20, 'righttoleft', 0.5, 1),
                undefined,
                undefined, ':#', "normal 45px Segoe UI", 80, 60, 'white', true),
            ]
            , 'sampletooltip')
        );

        //FLOATING CONTROLS
        if (!Experience.Instance.FloatingControlExists('tt')) Experience.Instance.AttachFloatingControl(
            new FloatingToolTip('tt', 250, 150, '#EBE458'));


        //SHOW APPLICATION
        Experience.Instance.ShowApplication('sampletooltip', 'tt');

    }
    




}

