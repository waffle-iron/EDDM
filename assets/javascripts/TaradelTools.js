// TARADEL TOOLS =============================================================================================
// SWITCH OUT THIS FILE IN Site.Master !!!!!!

// Version 1.1.0        2/18/2016   Added function to format and return Currency w/o cents - just whole dollars.
//                                  'CurrencyDollars'. - DSF
//
// Version 1.2.0        3/1/2016    Added RetrieveFromQueryString functions.
//============================================================================================================





function Formatter()
{}

    Formatter.Test = function (num)
    {
            num = num.toString().replace(/\$|\,/g, '');
            if (isNaN(num)) {
                num = "0";
            }

            sign = (num == (num = Math.abs(num)));
            num = Math.floor(num * 100 + 0.50000000001);
            cents = num % 100;
            num = Math.floor(num / 100).toString();

            //format as cents if < 1.00
            if (num == 0) {
                return cents + '&cent';
            }

            if (cents < 10) {
                cents = "0" + cents;
            }
            for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++) {
                num = num.substring(0, num.length - (4 * i + 3)) + ',' + num.substring(num.length - (4 * i + 3));
            }



            return (((sign) ? '' : '-') + '$' + num + '.' + cents);
    }



    //Converts to $ or &cent
    Formatter.Currency = function (num)
    {
        num = num.toString().replace(/\$|\,/g, '');
        if (isNaN(num)) {
            num = "0";
        }

        var sign = (num == (num = Math.abs(num)));
        num = Math.floor(num * 100 + 0.50000000001);
        var cents = num % 100;
        num = Math.floor(num / 100).toString();

        //format as cents if < 1.00
        if (num === 0) {
            return cents + '&cent';
        }

        if (cents < 10) {
            cents = "0" + cents;
        }
        for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++) {
            num = num.substring(0, num.length - (4 * i + 3)) + ',' + num.substring(num.length - (4 * i + 3));
        }

        return (((sign) ? '' : '-') + '$' + num + '.' + cents);
    }


    //Formats withOUT cents - whole dollar/int
    Formatter.CurrencyDollars = function (num)
    {
        var sign = (num == (num = Math.abs(num)));
        num = num.toString().replace(/\$|\,/g, '');

        if (isNaN(num))
        { num = "0";}

        for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++)
        {
            num = num.substring(0, num.length - (4 * i + 3)) + ',' + num.substring(num.length - (4 * i + 3));
        }

        return (((sign) ? '' : '-') + '$' + num);

    }



    ////should commas also do toFixed(2)
    //Shane says NOT to convert to int or anything 1/29/2016
    Formatter.Commas = function (val1) {
        return val1.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }


    Formatter.RemoveCommas = function (dirtyNum) {
        var number = Number(dirtyNum.toString().replace(/[^0-9\.]+/g, ""));
        var numberInt = parseInt(number)
        return numberInt;
    }









function RetrieveFromQueryString()
{ }



    //will return value of querystring parameter.
    RetrieveFromQueryString.GetValue = function (name)
        {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regexS = "[\\?&]" + name + "=([^&#]*)",
                regex = new RegExp(regexS),
                results = regex.exec(window.location.href);
            if (results == null) {
                return "";
            } else {
                return decodeURIComponent(results[1].replace(/\+/g, " "));
            }
        }








