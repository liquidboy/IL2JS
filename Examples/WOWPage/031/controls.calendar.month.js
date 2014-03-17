function Calendar()
{

    var daysInMonth = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
    var monthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
              'Jul', 'Aug', 'Sept', 'Oct', 'Nov', 'Dec'];


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

    this.CreateCalendarMonth = function _CreateCalendarMonth(y, m, appname) {

        var pagename = y + '-' + m;

        Experience.Instance.AllowVerticalNavigation = true;

        
        var month = m - 1;
        var year = y;

        var controls = [];

        var firstDayDate = new Date(year, month, 1);
        var firstDay = firstDayDate.getDay();
        var daysInMonth = getDaysInMonth(month, year);
        var displayNumber = 1;
        var itterateTo = daysInMonth + firstDay;

        var currentDay = new Date().getDate();
        var currentMonth = new Date().getMonth();



        months = 12;

        controls.push(new TextAnimated(0, 35, 2, "FromBottom", monthNames[month], "normal 90px Segoe UI", "white", 0, 0, -170));

        controls.push(new ElasticRectangleAnimated(0, "#9263A3", 1.0, 35, 1.5, "FromBottom", true, new Storyboard('quadratic', 'in', 1.2, 20, 'righttoleft', 0, 0.7)));
        controls.push(new TextAnimated(0, 35, 2, "FromBottom", "Sunday", "normal 16px Segoe UI", "white", 0, 10, -30));

        controls.push(new ElasticRectangleAnimated(1, "#9263A3", 1.0, 35, 1.5, "FromBottom", true, new Storyboard('quadratic', 'in', 1.2, 20, 'righttoleft', 0, 0.7)));
        controls.push(new TextAnimated(1, 35, 2, "FromBottom", "Monday", "normal 16px Segoe UI", "white", 0, 10, -30));

        controls.push(new ElasticRectangleAnimated(2, "#9263A3", 1.0, 35, 1.5, "FromBottom", true, new Storyboard('quadratic', 'in', 1.2, 20, 'righttoleft', 0, 0.7)));
        controls.push(new TextAnimated(2, 35, 2, "FromBottom", "Tuesday", "normal 16px Segoe UI", "white", 0, 10, -30));

        controls.push(new ElasticRectangleAnimated(3, "#9263A3", 1.0, 35, 1.5, "FromBottom", true, new Storyboard('quadratic', 'in', 1.2, 20, 'righttoleft', 0, 0.7)));
        controls.push(new TextAnimated(3, 35, 2, "FromBottom", "Wednesday", "normal 16px Segoe UI", "white", 0, 10, -30));

        controls.push(new ElasticRectangleAnimated(4, "#9263A3", 1.0, 35, 1.5, "FromBottom", true, new Storyboard('quadratic', 'in', 1.2, 20, 'righttoleft', 0, 0.7)));
        controls.push(new TextAnimated(4, 35, 2, "FromBottom", "Thursday", "normal 16px Segoe UI", "white", 0, 10, -30));

        controls.push(new ElasticRectangleAnimated(5, "#9263A3", 1.0, 35, 1.5, "FromBottom", true, new Storyboard('quadratic', 'in', 1.2, 20, 'righttoleft', 0, 0.7)));
        controls.push(new TextAnimated(5, 35, 2, "FromBottom", "Friday", "normal 16px Segoe UI", "white", 0, 10, -30));

        controls.push(new ElasticRectangleAnimated(6, "#9263A3", 1.0, 35, 1.5, "FromBottom", true, new Storyboard('quadratic', 'in', 1.2, 20, 'righttoleft', 0, 0.7)));
        controls.push(new TextAnimated(6, 35, 2, "FromBottom", "Saturday", "normal 16px Segoe UI", "white", 0, 10, -30));






        for (j = firstDay; j < itterateTo; j++) {
            controls.push(new ElasticButton(j + 7, "#BE00FF", "#410F53", new Storyboard('quadratic', 'in', 1.4, 20, 'righttoleft', 0, 1), 'requestToLoadApplication(\'showday\',' + y + ',' + m + ',' + displayNumber + ');', null, displayNumber, 'normal 32px Segoe UI', 10, 0, "White"));

            if (j == currentDay && month == currentMonth) {
                controls.push(new ElasticRectangleAnimated(j + 7, "#FFBA00", 1.0, 35, 1.5, "FromBottom", true, new Storyboard('quadratic', 'in', 1.2, 20, 'righttoleft', 0, 0.7)));
                controls.push(new TextAnimated(j + 7, 35, 2, "FromBottom", "today", "normal 16px Segoe UI", "white", 0, 10, -30));
            }

            displayNumber++;
        }


        Experience.Instance.Attach(new PageX(
            pagename
            , 7
            , 7
            , [
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
                10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
                20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
                30, 31, 32, 33, 34, 35, 36, 37, 38, 39,
                40, 41, 42, 43, 44, 45, 46, 47, 48
              ]
            , controls
            , appname
        ));


    }

}