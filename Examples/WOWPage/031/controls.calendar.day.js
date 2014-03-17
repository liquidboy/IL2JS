function CalendarDay()
{

    var daysInMonth = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
    var monthNames = ['January', 'Febuary', 'March', 'April', 'May', 'June',
              'July', 'August', 'September', 'October', 'November', 'December'];


    function getDaysInMonth(month, year)
    {
        if ((month == 1) && (year % 4 == 0) && ((year % 100 != 0) || (year % 400 == 0)))
        {
            return 29;
        } else
        {
            return daysInMonth[month];
        }
    }

    this.CreateCalendarDayView = function _CreateCalendarDayView(y, m, d, appname)
    {

        var pagename = y + '-' + m + '-' + d;

        Experience.Instance.AllowVerticalNavigation = false;

        var controls = [];

        var displayNumber = 0;

        var selectedDate = new Date();
        selectedDate.setDate(d);
        selectedDate.setMonth(m-1);
        selectedDate.setYear(y);



        //controls.push(new TextAnimated(0, 35, 2, "FromBottom", y, "normal 60px Segoe UI", "white", 0, 0, -170));
        //controls.push(new TextAnimated(26, 35, 2, "FromBottom", monthNames[m - 1] + ' ' + d, "normal 90px Segoe UI", "white", 0, 0, -170));

        


        controls.push(new ElasticRectangleAnimated(0, "#9263A3", 1.0, 70, 1.5, "FromBottom", true, new Storyboard('quadratic', 'in', 1.2, 20, 'righttoleft', 0, 0.7)));
        controls.push(new TextAnimated(0, 70, 2, "FromBottom", "AM", "normal 40px Segoe UI", "white", 0, 10, -55));

        controls.push(new ElasticRectangleAnimated(1, "#9263A3", 1.0, 70, 1.5, "FromBottom", true, new Storyboard('quadratic', 'in', 1.2, 20, 'righttoleft', 0, 0.7)));
        controls.push(new TextAnimated(1, 70, 2, "FromBottom", "PM", "normal 40px Segoe UI", "white", 0, 10, -55));







        for (j = 0; j <= 23; j++)
        {

            if (j == 0) displayNumber = 12;
            else if (j == 12) displayNumber = 12;
            else if (j > 0 && j < 12) displayNumber = j;
            else if (j > 12 && j < 24) displayNumber = j - 12;

            controls.push(new ElasticButton(j + 2, "#BE00FF", "#410F53", new Storyboard('quadratic', 'in', 1.4, 20, 'righttoleft', 0, 1), undefined, null, displayNumber, 'normal 32px Segoe UI', 10, 0, "White"));

            displayNumber++;
        }


        Experience.Instance.Attach(new PageX(
            pagename
            , 24
            , 4
            , [
                0,
                12,
                [24, 72],
                [25, 73],
                [26, 74],
                [27, 75],
                [28, 76],
                [29, 77],
                [30, 78],
                [31, 79],
                [32, 80],
                [33, 81],
                [34, 82],
                [35, 83],
                [36, 84],
                [37, 85],
                [38, 86],
                [39, 87],
                [40, 88],
                [41, 89],
                [42, 90],
                [43, 91],
                [44, 92],
                [45, 93],
                [46, 94],
                [47, 95],
                [1,4]
              ]
            , controls
            , appname
        ));


    }

}