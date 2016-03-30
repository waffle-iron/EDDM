//==============================================================================================================
//VERSION 1.0.1
//
//  1.0.1   2/2/2016    Removed 'btn-default' from jquery applied classes to these buttons to utilize the new ui-controls class.
//
//  1.1.0   2/19/2016   Added a lot of functionality and functions to allow the filters to be preset upon used using the
//                      back button. CheckForRevisit(), HideFirstTimeBlocks(), PreselectFiltersAndLabels().
//
//  1.2.0   3/24/2016   Added show-sample click function. 
//==============================================================================================================





// API Parameter Values:
// Radius: int
// Lat: decimal
// Lon: decimal
// RadiusType: String. Minutes or Miles
// CountOnly: String. True / False
// Gender: F, M, or U (unknown). 
// HomeOwnership: O or R
// Income:  A-J, comma delimted
// Ethnicity: char values, comma delimited.
// KidsPresent: Y,N
// MartialStatus: A, B, M, S.  Currently ONLY using M or S
// NetWorth: char values, comma delimited.
// Property Value (HomeMktVal): char values, comma delimited.
// OutputSample=True/False" Will return sample set of X or full complete set.


// INCOME Ranges:
// 0 - 14999     A
// 15000-24999   B
// 25000-34999   C
// 35000-49999   D
// 50000-74999   E
// 75000-99999   F
// 100000-124999 G
// 125000-149999 H
// 150000-174999 I
// 175000-199999 J
// 200000-249999 K
// 250000+       L



// AGE RANGES:
// 18-24
// 25-29
// 30-34
// 35-39
// 40-44
// 45-49
// 50-54
// 55-59
// 60-64
// 65-69
// 70-74
// 75+



// NET WORTH VALUE RANGES:
// 1 = Less than or equal to $0
// 2 = $1 - $4,999
// 3 = $5,000 - $9,999
// 4 = $10,000 - $24,999
// 5 = $25,000 - $49,999
// 6 = $50,000 - $99,999
// 7 = $100,000 - $249,999
// 8 = $250,000 - $499,999
// 9 = $500,000 - $999,999
// A = $1,000,000 - $1,999,999
// B = $2,000,000 +



// PROPERTY VALUE "HomeMktVal""
// A = $1,000 - $24,999
// B = $25,000 - $49,999
// C = $50,000 - $74,999
// D = $75,000 - $99,999
// E = $100,000 - $124,999
// F = $125,000 - $149,999
// G = $150,000 - $174,999
// H = $175,000 - $199,999
// I = $200,000 - $224,999
// J = $225,000 - $249,999
// K = $250,000 - $274,999
// L = $275,000 - $299,999
// M = $300,000 - $349,999
// N = $350,000 - $399,999
// O = $400,000 - $449,999
// P = $450,000 - $499,999
// Q = $500,000 - $749,999
// R = $750,000 - $999,999
// S = $1,000,000 Plus



//ETHNICITY
//africanAmerican = 'Z';
//arab = 'R';
//asian = 'O';
//asianNonOriental = 'B';
//french = 'F';
//german = 'G';
//hispanic = 'H';
//italian = 'I';
//jewish = 'J';
//miscellaneous = 'M';
//northernEurpoean = 'N';
//polynesian = 'P';
//scottishIrish = 'S';
//southernEuropean = 'D';
//unclassified = 'X';






// PAGE LOAD
$(function () {

    //income controls
    $("#incomeSlider").slider(
    {

        range: true,
        min: 0,
        max: 250000,
        step: 5000,
        values: [0, 250000],     //<-- default settings
        stop: function (event, ui) {
            GetIncomeRange();
            BuildPostUrl();
            CalculateAddressedCount();
            //console.log("STOP");
        },

        slide: function (event, ui) {

            $("#minIncome").html(Formatter.CurrencyDollars(ui.values[0]));

            //add '+' if maxed out
            if (ui.values[1] >= 250000)
            { $("#maxIncome").html(Formatter.CurrencyDollars(ui.values[1]) + '+'); }
            else
            { $("#maxIncome").html(Formatter.CurrencyDollars(ui.values[1])); }


            //update filter category label
            if ((ui.values[0] == 0) && (ui.values[1] >= 250000))
            { $("#incomeLabel").html('No Filter'); }
            else
            { $("#incomeLabel").html(BuildIncomeLabel()); }


            $("#hidRawMinIncome").val(ui.values[0]);
            $("#hidRawMaxIncome").val(ui.values[1]);
            $("#txtMinIncome").val(ui.values[0]);
            $("#txtMaxIncome").val(ui.values[1]);

        }

    });



    $("#hidRawMinIncome").val($("#incomeSlider").slider("values", 0));
    $("#hidRawMaxIncome").val($("#incomeSlider").slider("values", 1));
    $("#minIncome").html(Formatter.CurrencyDollars($("#incomeSlider").slider("values", 0)));


    //add '+' if maxed out
    if ($("#incomeSlider").slider("values", 1) >= 250000) {
        $("#maxIncome").html(Formatter.CurrencyDollars(parseInt($("#incomeSlider").slider("values", 1))) + '+');
    }

    else {
        $("#maxIncome").html(Formatter.CurrencyDollars(parseInt($("#incomeSlider").slider("values", 1))));
    }

    //debug
    //console.log("selectedMin in Document Ready " + Formatter.CurrencyDollars($("#incomeSlider").slider("values", 0)));
    //console.log("selectedMax in Document Ready " + Formatter.CurrencyDollars(parseInt($("#incomeSlider").slider("values", 1))));
    //console.log("slider value 0: " + $("#incomeSlider").slider("values", 0));
    //console.log("slider value 1: " + $("#incomeSlider").slider("values", 1));
    //console.log("Formatted 0: " + Formatter.CurrencyDollars($("#incomeSlider").slider("values", 0)));
    //console.log("Formatted 1: " + Formatter.CurrencyDollars($("#incomeSlider").slider("values", 1)));
    //console.log("Old Method: " + '$' + NumberWithCommas($("#incomeSlider").slider("values", 1) + '+'));
    //console.log("New Method: " + Formatter.CurrencyDollars($("#incomeSlider").slider("values", 1)) + '+');
    //end of income controls





    //age controls
    $("#ageSlider").slider(
        {
            range: true,
            min: 18,
            max: 75,
            step: 1,
            values: [18, 75],     //<-- default settings
            stop: function (event, ui) {
                GetAgeRange();
                BuildPostUrl();
                CalculateAddressedCount();
            },

            slide: function (event, ui) {

                $("#minAge").html(ui.values[0]);

                //add '+' if maxed out
                if (ui.values[1] >= 75)
                { $("#maxAge").html((ui.values[1]) + '+'); }
                else
                { $("#maxAge").html((ui.values[1])); }

                //update filter age label
                if ((ui.values[0] == 18) && (ui.values[1] == 75))
                { $("#ageLabel").html('No Filter'); }
                else
                { $("#ageLabel").html(BuildAgeLabel()); }


                $("#hidRawMinAge").val(ui.values[0]);
                $("#hidRawMaxAge").val(ui.values[1]);
                $("#txtMinAge").val(ui.values[0]);
                $("#txtMaxAge").val(ui.values[1]);


            }

        });




    $("#hidRawMinAge").val($("#ageSlider").slider("values", 0));
    $("#hidRawMaxAge").val($("#ageSlider").slider("values", 1));

    $("#minAge").html(($("#ageSlider").slider("values", 0)));

    //add '+' if maxed out
    if ($("#ageSlider").slider("values", 1) >= 75)
    { $("#maxAge").html(($("#ageSlider").slider("values", 1) + '+')); }
    else
    { $("#maxAge").html(($("#ageSlider").slider("values", 1))); }
    //end of age controls





    //net worth value controls
    $("#netWorthSlider").slider(
    {
        range: true,
        min: 0,
        max: 1000000,
        step: 10000,
        values: [0, 1000000],     //<-- default settings

        slide: function (event, ui) {

            $("#minNetWorth").html('$' + NumberWithCommas(ui.values[0]));

            //add ' and above' if maxed out
            if (ui.values[1] >= 1000000)
            { $("#maxNetWorth").html('$' + NumberWithCommas(ui.values[1]) + ' and above'); }
            else
            { $("#maxNetWorth").html('$' + NumberWithCommas(ui.values[1])); }


            //update filter category label
            if ((ui.values[0] == 0) && (ui.values[1] >= 1000000))
            { $("#netWorthLabel").html('No Filter'); }
            else
            { $("#netWorthLabel").html(BuildNetWorthLabel()); }


            $("#hidRawMinNetWorth").val(ui.values[0]);
            $("#hidRawMaxNetWorth").val(ui.values[1]);

            GetNetWorthRange();
            BuildPostUrl();
            CalculateAddressedCount();

        }

    });


    $("#hidRawMinNetWorth").val($("#netWorthSlider").slider("values", 0));
    $("#hidRawMaxNetWorth").val($("#netWorthSlider").slider("values", 1));
    $("#minNetWorth").html('$' + Formatter.Commas($("#netWorthSlider").slider("values", 0)));

    //add '+' if maxed out
    if ($("#netWorthSlider").slider("values", 1) >= 1000000)
    { $("#maxNetWorth").html('$' + Formatter.Commas($("#netWorthSlider").slider("values", 1) + '+')); }
    else
    { $("#maxNetWorth").html('$' + Formatter.Commas($("#netWorthSlider").slider("values", 1))); }
    //end of property value controls


    //Check for user coming 'back' by using the back button. Will reset filters and controls as needed.
    if (CheckForRevisit() == true) {
        PreselectFiltersAndLabels();
        HideFirstTimeBlocks();
    }

    else {
        $("#defineYourAreaToBegin").show(2000);
        $("#defineYourAreaToBegin").slideDown(2000);
    }

});





//** Target Area **
function RadiusValueChanged() {

    FindLatLonData();
    BuildPostUrl();

}



function RadiusTypeChanged() {

    FindLatLonData();
    BuildPostUrl();

}



function TargetAreaTypeChanged() {

    //Zip Codes is selected.  Check string.
    if ($('#radZipCodes').is(':checked')) {
        $("#txtZipCodes").val($("#txtZipCodesList").val());
        BuildPostUrl();
    }

    else {
        $("#txtZipCodes").val("(not defined)");
        BuildPostUrl();
    }

    $("#defineYourAreaToBegin").hide(1000);

}



function AddressChanged() {


    if (parseInt($('#txtAddress').val().length) > 0) {

        //hide error message, skin button, skin control wrapper
        $('#validationSummary').hide();
        $('#hypNext').removeAttr("class");
        $('#hypNext').attr("class", "btn btn-cta");
        $('#addressControlBlock').removeAttr("class");
        $('#addressControlBlock').attr("class", "form-group");
        $("#addressLabel").removeAttr("class");
        $("#addressLabel").attr("class", "label formLabel formLabelRequired");


        ValidateAddressAndZip();

    }

    else {

        //show error message, skin button, skin control wrapper
        $('#validationSummary').show();
        $('#validationSummary').removeAttr("class");
        $('#validationSummary').attr("class", "alert alert-danger");
        $("#validationMessage").html("Address is required.");
        $('#hypNext').removeAttr("class");
        $('#hypNext').attr("class", "btn btn-default disabled");
        $('#addressControlBlock').removeAttr("class");
        $('#addressControlBlock').attr("class", "form-group has-error");
        $("#addressLabel").removeAttr("class");
        $("#addressLabel").attr("class", "label label-danger");

    }

}



function ZipCodeChanged() {

    if (parseInt($('#txtZipCode').val().length) >= 5) {

        //hide error message, skin button, skin control
        $('#validationSummary').hide();
        $('#hypNext').removeAttr("class");
        $('#hypNext').attr("class", "btn btn-cta");
        $('#txtZipCode').removeAttr("class");
        $('#txtZipCode').attr("class", "form-control");
        $('#zipCodeControlBlock').removeAttr("class");
        $('#zipCodeControlBlock').attr("class", "form-group");
        $("#zipCodeLabel").removeAttr("class");
        $("#zipCodeLabel").attr("class", "label formLabel formLabelRequired");

        ValidateAddressAndZip();

    }


    else {
        $('#validationSummary').show();
        $('#validationSummary').removeAttr("class");
        $('#validationSummary').attr("class", "alert alert-danger");
        $("#validationMessage").html("A 5 digiti Zip Code is required.");
        $('#hypNext').removeAttr("class");
        $('#hypNext').attr("class", "btn btn-default disabled");
        $('#zipCodeControlBlock').removeAttr("class");
        $('#zipCodeControlBlock').attr("class", "form-group has-error");
        $("#zipCodeLabel").removeAttr("class");
        $("#zipCodeLabel").attr("class", "label label-danger");

    }



}



function ZipCodesListChanged() {

    var providedZipList = $("#txtZipCodesList").val();


    //Zip Code string is too short
    if (providedZipList.length < 5) {

        $('#validationSummary').show();
        $('#validationSummary').removeAttr("class");
        $('#validationSummary').attr("class", "alert alert-danger");
        $("#validationMessage").html("A valid 5 digit Zip Code is required.");
        $('#hypNextZipList').removeAttr("class");
        $('#hypNextZipList').attr("class", "btn btn-default disabled");
        $('#zipCodesListControlBlock').removeAttr("class");
        $('#zipCodesListControlBlock').attr("class", "form-group has-error");
        $("#zipCodesListLabel").removeAttr("class");
        $("#zipCodesListLabel").attr("class", "label label-danger");
        $("#zipCodeInstructions").show();
    }

        //Zip Codes string passes length test
    else {


        //hide error message, skin button, skin control
        $('#validationSummary').hide();
        $('#hypNextZipList').removeAttr("class");
        $('#hypNextZipList').attr("class", "btn btn-cta");
        $('#txtZipCodesList').removeAttr("class");
        $('#txtZipCodesList').attr("class", "form-control");
        $('#zipCodesListControlBlock').removeAttr("class");
        $('#zipCodesListControlBlock').attr("class", "form-group");
        $("#zipCodesListLabel").removeAttr("class");
        $("#zipCodesListLabel").attr("class", "label formLabel");
        $("#txtZipCodes").val(providedZipList.replace(" ", ""));
        $("#zipCodeInstructions").hide();

        BuildPostUrl();

    }



}








//** Home Ownership **
function HomeOwnershipSelected(buttonName) {


    //Homeowner IS selected, remove styling from renter
    if (buttonName == 'btnHomeowner') {

        //strip off styling of Renter
        $('#btnRenter').removeClass();
        $('#btnRenter').addClass('btn btn-sm btn-block');
        $('#txtHomeOwnership').val('O');


        //see if homeowner was already selected
        var currentClass = $('#btnHomeowner').attr('class');

        //Homeowner was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {
            $('#txtHomeOwnership').val('(not defined)');
            $('#btnHomeowner').removeClass();
            $('#btnHomeowner').addClass('btn btn-sm btn-block');

            //fill the label
            $('#homeOwnershipLabel').html('No Filter');


        }

            //Homeowner was NOT selected so let's select it
        else {
            $('#txtHomeOwnership').val('O');
            $('#btnHomeowner').removeClass();
            $('#btnHomeowner').addClass('btn btn-sm btn-block selected');

            //fill the label
            $('#homeOwnershipLabel').html('Homeowner');

        }

    }

        //Renter is selected
    else {

        //strip off styling of Homeowner
        $('#btnHomeowner').removeClass();
        $('#btnHomeowner').addClass('btn btn-sm btn-block');
        $('#txtHomeOwnership').val('R');


        //see if renter was already selected
        var currentClass = $('#btnRenter').attr('class');

        //Renter was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {
            $('#txtHomeOwnership').val('(not defined)');
            $('#btnRenter').removeClass();
            $('#btnRenter').addClass('btn btn-sm btn-block');

            //fill the label
            $('#homeOwnershipLabel').html('No Filter');

        }

            //Renter was NOT selected so let's select it
        else {
            $('#txtHomeOwnership').val('R');
            $('#btnRenter').removeClass();
            $('#btnRenter').addClass('btn btn-sm btn-block selected');

            //fill the label
            $('#homeOwnershipLabel').html('Renter');

        }

    }

    BuildPostUrl();
    CalculateAddressedCount();

}





//** Marital Status **
function MaritalSelected(buttonName) {


    //Married IS selected, remove styling from Not Married
    if (buttonName == 'btnMarried') {

        //strip off styling of Not Married
        $('#btnSingle').removeClass();
        $('#btnSingle').addClass('btn btn-sm btn-block');
        $('#txtMartialStatus').val('M');


        //see if married was already selected
        var currentClass = $('#btnMarried').attr('class');

        //Married was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {
            $('#txtMartialStatus').val('(not defined)');
            $('#btnMarried').removeClass();
            $('#btnMarried').addClass('btn btn-sm btn-block');

            //fill the label
            $('#maritalStatusLabel').html('No Filter');

        }

            //Married was NOT selected so let's select it
        else {
            $('#txtMartialStatus').val('M');
            $('#btnMarried').removeClass();
            $('#btnMarried').addClass('btn btn-sm btn-block selected');

            //fill the label
            $('#maritalStatusLabel').html('Married');

        }

    }

        //Single is selected
    else {

        //strip off styling of Children
        $('#btnMarried').removeClass();
        $('#btnMarried').addClass('btn btn-sm btn-block');
        $('#txtMartialStatus').val('S');


        //see if NOT Married was already selected
        var currentClass = $('#btnSingle').attr('class');

        //Not Married was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {
            $('#txtMartialStatus').val('(not defined)');
            $('#btnSingle').removeClass();
            $('#btnSingle').addClass('btn btn-sm btn-block');

            //fill the label
            $('#maritalStatusLabel').html('No Filter');

        }

            //Not Married was NOT selected so let's select it
        else {
            $('#txtMartialStatus').val('S');
            $('#btnSingle').removeClass();
            $('#btnSingle').addClass('btn btn-sm btn-block selected');

            //fill the label
            $('#maritalStatusLabel').html('Single');

        }

    }

    BuildPostUrl();
    CalculateAddressedCount();

}





//** Children **
function ChildrenSelected(buttonName) {


    //Homeowner IS selected, remove styling from renter
    if (buttonName == 'btnChildren') {

        //strip off styling of No Children
        $('#btnNoChildren').removeClass();
        $('#btnNoChildren').addClass('btn btn-sm btn-block');
        $('#txtChildren').val('Y');


        //see if children was already selected
        var currentClass = $('#btnChildren').attr('class');

        //Children was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {
            $('#txtChildren').val('(not defined)');
            $('#btnChildren').removeClass();
            $('#btnChildren').addClass('btn btn-sm btn-block');

            //fill the label
            $('#childrenLabel').html('No Filter');

        }

            //Children was NOT selected so let's select it
        else {
            $('#txtChildren').val('Y');
            $('#btnChildren').removeClass();
            $('#btnChildren').addClass('btn btn-sm btn-block selected');

            //fill the label
            $('#childrenLabel').html('Children');

        }

    }

        //NO Children is selected
    else {

        //strip off styling of Children
        $('#btnChildren').removeClass();
        $('#btnChildren').addClass('btn btn-sm btn-block');
        $('#txtChildren').val('N');


        //see if NO Children was already selected
        var currentClass = $('#btnNoChildren').attr('class');

        //No Children was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {
            $('#txtChildren').val('(not defined)');
            $('#btnNoChildren').removeClass();
            $('#btnNoChildren').addClass('btn btn-sm btn-block');

            //fill the label
            $('#childrenLabel').html('No Filter');

        }

            //No Children was NOT selected so let's select it
        else {
            $('#txtChildren').val('N');
            $('#btnNoChildren').removeClass();
            $('#btnNoChildren').addClass('btn btn-sm btn-block selected');

            //fill the label
            $('#childrenLabel').html('No Children');

        }
    }

    BuildPostUrl();
    CalculateAddressedCount();

}





//** Gender **
function GenderSelected(buttonName) {


    //Male IS selected, remove styling from renter
    if (buttonName == 'btnMale') {

        //strip off styling of No Children
        $('#btnFemale').removeClass();
        $('#btnFemale').addClass('btn btn-sm btn-block');
        $('#txtGender').val('M');


        //see if male was already selected
        var currentClass = $('#btnMale').attr('class');

        //Male was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {
            $('#txtGender').val('(not defined)');
            $('#btnMale').removeClass();
            $('#btnMale').addClass('btn btn-sm btn-block');

            //fill the label
            $('#genderLabel').html('No Filter');

        }

            //Male was NOT selected so let's select it
        else {
            $('#txtGender').val('M');
            $('#btnMale').removeClass();
            $('#btnMale').addClass('btn btn-sm btn-block selected');

            //fill the label
            $('#genderLabel').html('Male');

        }

    }

        //Female is selected
    else {

        //strip off styling of male
        $('#btnMale').removeClass();
        $('#btnMale').addClass('btn btn-sm btn-block');
        $('#txtGender').val('F');


        //see if female was already selected
        var currentClass = $('#btnFemale').attr('class');

        //female was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {
            $('#txtGender').val('(not defined)');
            $('#btnFemale').removeClass();
            $('#btnFemale').addClass('btn btn-sm btn-block');

            //fill the label
            $('#genderLabel').html('No Filter');

        }

            //Female was NOT selected so let's select it
        else {
            $('#txtGender').val('F');
            $('#btnFemale').removeClass();
            $('#btnFemale').addClass('btn btn-sm btn-block selected');

            //fill the label
            $('#genderLabel').html('Female');

        }
    }

    BuildPostUrl();
    CalculateAddressedCount();

}





//** Income **
function GetIncomeRange() {


    var Range = function (letter, minVal, maxVal) {
        this.letter = letter;
        this.minVal = minVal;
        this.maxVal = maxVal;
    }

    var ranges = [

        new Range("A", 0, 14999),
        new Range("B", 15000, 24999),
        new Range("C", 25000, 34999),
        new Range("D", 35000, 49999),
        new Range("E", 50000, 74999),
        new Range("F", 75000, 99999),
        new Range("G", 100000, 124999),
        new Range("H", 125000, 149999),
        new Range("I", 150000, 174999),
        new Range("J", 175000, 199999),
        new Range("K", 200000, 249999),
        new Range("L", 250000, 999999)

    ];



    ranges.inArray2 = function (lowSelection, highSelection) {

        var retVal = "";
        var self = this;

        for (var index = 0; index < self.length; index++) {

            var item = self[index];

            if ((lowSelection <= item.maxVal) && (highSelection >= item.minVal)) {
                retVal = retVal + item.letter + ",";
                $('#txtCombinedIncome').val(retVal);
            }


        }



        if ((parseInt(lowSelection) == 0) && (parseInt(highSelection) >= 250000))
        { $("#txtCombinedIncome").val('(not defined)'); }

        else
        {
            //strip off last char
            var lengthOfString = $('#txtCombinedIncome').val().length;
            var strippedString = ($('#txtCombinedIncome').val()).substring(0, lengthOfString - 1);
            $('#txtCombinedIncome').val(strippedString);
        }




    };




    //Call it
    var lowSelection = parseInt($("#hidRawMinIncome").val());
    var highSelection = parseInt($("#hidRawMaxIncome").val());

    ranges.inArray2(lowSelection, highSelection);


}



function BuildIncomeLabel() {

    //This is the label in the filter header. Ex: '$25k - $225k'.
    var results = "";
    var selectedMin = $("#minIncome").html();
    var selectedMax = $("#maxIncome").html();
    var minLength = selectedMin.length;
    var maxLength = selectedMax.length;


    //min string
    if (selectedMin == "$0") {
        selectedMin = "$0,000";
        minLength = selectedMin.length;
    }

    var trimmedMin = selectedMin.substring(0, minLength - 4) + 'k';


    //max string
    var trimmedMax = "";
    if (selectedMax == "$250,000+")
    { trimmedMax = "$250k+" }

    else
    { trimmedMax = selectedMax.substring(0, maxLength - 4) + 'k'; }

    results = trimmedMin + ' - ' + trimmedMax;

    //debug
    //console.log('Building Income label....');
    //console.log("selectedMin in BuildIncomeLabel" + selectedMin);
    //console.log("selectedMax in BuildIncomeLabel " + selectedMax);


    return results;

}





//** Age **
function GetAgeRange() {


    var Range = function (ageRange, minAge, maxAge) {
        this.ageRange = ageRange;
        this.minAge = minAge;
        this.maxAge = maxAge;
    }

    var ranges =
    [
        new Range("18-24", 18, 24),
        new Range("25-29", 25, 29),
        new Range("30-34", 30, 34),
        new Range("35-39", 35, 39),
        new Range("40-44", 40, 44),
        new Range("45-49", 45, 49),
        new Range("50-54", 50, 54),
        new Range("55-59", 55, 59),
        new Range("60-64", 60, 64),
        new Range("65-69", 65, 69),
        new Range("70-74", 70, 74),
        new Range("75+", 75, 99),

    ];



    ranges.inArray2 = function (lowSelection, highSelection) {

        var retVal = "";
        var self = this;


        for (var index = 0; index < self.length; index++) {

            var item = self[index];

            if ((lowSelection <= item.maxAge) && (highSelection >= item.minAge)) {
                retVal = retVal + item.ageRange + ",";
                $('#txtAgeRanges').val(retVal);

            }


        }



        if ((parseInt(lowSelection) == 18) && (parseInt(highSelection) >= 75))
        { $('#txtAgeRanges').val('(not defined)'); }
        else
        {
            //strip off last char
            var lengthOfString = $('#txtAgeRanges').val().length;
            var strippedString = ($('#txtAgeRanges').val()).substring(0, lengthOfString - 1);
            $('#txtAgeRanges').val(strippedString);
        }


    };



    //Call it
    var lowSelection = parseInt($("#hidRawMinAge").val());
    var highSelection = parseInt($("#hidRawMaxAge").val());

    ranges.inArray2(lowSelection, highSelection);

}



function BuildAgeLabel() {

    var selectedMin = $("#minAge").html();
    var selectedMax = $("#maxAge").html();
    var results = "";

    if ((selectedMin == "18") && (selectedMax >= "75+"))
    { results = "No Filter"; }
    else
    { results = selectedMin + '-' + selectedMax; }

    return results;

}





//** Property Value **
function PropValueSelected() {

    //reset Select All if needed
    var totSelected = parseInt(CountSelectedCheckboxes("PropertyValue"));
    var numOfPropValuesCheckboxes = 19;


    if (totSelected < numOfPropValuesCheckboxes) {
        $("#selectAllPropValueLabel").html("Select All");
        $("#chkSelectAllPropValue").prop("checked", false);
    }

    BuildPropValueString();
    BuildPropValueLabel();
    BuildPostUrl();
    CalculateAddressedCount();

    //Hide checkboxes for 2.5 seconds - give time to API call to complete
    $('#propertyControlBlock').block
    ({
        message: '<p class="text-danger text-center"><br /><br /><span class="fa fa-2x fa-cog fa-spin"></span></p>',
        css: { width: '100%', height: '100%', border: '1px solid #dddddd' }
    });

    setTimeout(function ()
    { $('#propertyControlBlock').unblock(); }, 2500);


}



function BuildPropValueLabel() {

    var totSelected = parseInt($("#hidPropValueTotalSelected").val());


    if (totSelected == 0)
    { results = "No Filter" }

    else
    {
        if (totSelected == 1)
        { results = totSelected + " range selected"; }

        else
        { results = totSelected + " ranges selected"; }

    }

    $("#propertyLabel").html(results);

}



function BuildPropValueString() {

    //Updates text box for BuildPostURL to read from
    var numOfPropValuesCheckboxes = 19;
    var results = "";
    var totSelected = parseInt(CountSelectedCheckboxes("PropertyValue"));


    if ((totSelected == 0) | (totSelected == numOfPropValuesCheckboxes)) {
        results = "(not defined)";
    }

    else {

        for (i = 1; i <= numOfPropValuesCheckboxes; i++) {
            if ($("#chkPropValue" + i).is(':checked')) {
                results = results + $("#chkPropValue" + i).val() + ",";
            }
        }


        //scrub it
        var lenOfString = results.length;
        results = results.substring(0, lenOfString - 1);

    }



    //Done
    $("#txtPropertyValue").val(results)


}






//** Net Worth **
function NetWorthSelected() {

    //reset Select All if needed
    var totSelected = parseInt(CountSelectedCheckboxes("NetWorth"));
    var numOfNetWorthCheckboxes = 11;


    if (totSelected < numOfNetWorthCheckboxes) {
        $("#selectAllNetWorthLabel").html("Select All");
        $("#chkSelectAllNetWorth").prop("checked", false);
    }


    BuildNetWorthString();
    BuildNetWorthLabel();
    BuildPostUrl();
    CalculateAddressedCount();


    //Hide checkboxes for 2.5 seconds - give time to API call to complete
    $('#netWorthControlBlock').block
    ({
        message: '<p class="text-danger text-center"><br /><br /><span class="fa fa-2x fa-cog fa-spin"></span></p>',
        css: { width: '100%', height: '100%', border: '1px solid #dddddd' }
    });

    setTimeout(function ()
    { $('#netWorthControlBlock').unblock(); }, 2500);


}



function BuildNetWorthLabel() {

    var totSelected = parseInt($("#hidNetWorthTotalSelected").val());


    if (totSelected == 0)
    { results = "No Filter" }

    else
    {
        if (totSelected == 1)
        { results = totSelected + " range selected"; }

        else
        { results = totSelected + " ranges selected"; }

    }

    $("#netWorthLabel").html(results);

}



function BuildNetWorthString() {

    //Updates text box for BuildPostURL to read from
    var numOfNetWorthCheckboxes = 11;
    var results = "";
    var totSelected = parseInt(CountSelectedCheckboxes("NetWorth"));


    if ((totSelected == 0) | (totSelected == numOfNetWorthCheckboxes)) {
        results = "(not defined)";
    }

    else {

        for (i = 1; i <= numOfNetWorthCheckboxes; i++) {
            if ($("#chkNetWorth" + i).is(':checked')) {
                results = results + $("#chkNetWorth" + i).val() + ",";
            }
        }


        //scrub it
        var lenOfString = results.length;
        results = results.substring(0, lenOfString - 1);

    }



    //Done
    $("#txtNetWorth").val(results)


}




//** Ethnicity **
function EthnicitySelected(buttonName) {


    var prevSelections = $('#txtEthnicity').val();
    var africanAmerican = 'Z';
    var arab = 'R';
    var asian = 'O';
    var asianNonOriental = 'B';
    var french = 'F';
    var german = 'G';
    var hispanic = 'H';
    var italian = 'I';
    var jewish = 'J';
    var miscellaneous = 'M';
    var northernEurpoean = 'N';
    var polynesian = 'P';
    var scottishIrish = 'S';
    var southernEuropean = 'D';
    var unclassified = 'X';



    //African American was selected
    if (buttonName == 'btnAfricanAmerican') {

        //see if African American was already selected
        var currentClass = $('#btnAfricanAmerican').attr('class');

        //btnAfricanAmerican was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {

            $('#btnAfricanAmerican').removeClass();
            $('#btnAfricanAmerican').addClass('btn btn-sm btn-block');

            //Evaluate whether to remove from string or set to 'undefined'.
            //string already existed. remove current selecttion
            if (prevSelections.length > 1) {
                prevSelections = prevSelections.replace(',' + africanAmerican, '');
                prevSelections = prevSelections.replace(africanAmerican + ',', '');
                $('#txtEthnicity').val(prevSelections);
            }

            else { $('#txtEthnicity').val('(not defined)'); }

            UpdateEthnicityTotal(-1);

        }

            //btnAfricanAmerican was NOT selected so let's select it
        else {
            $('#txtEthnicity').val(BuildEthnicityString(africanAmerican));
            $('#btnAfricanAmerican').removeClass();
            $('#btnAfricanAmerican').addClass('btn btn-sm btn-block selected');

            UpdateEthnicityTotal(1);

        }

    }



    //Arab was selected
    if (buttonName == 'btnArab') {


        //see if Arab was already selected
        var currentClass = $('#btnArab').attr('class');

        //btnArab was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {

            $('#btnArab').removeClass();
            $('#btnArab').addClass('btn btn-sm btn-block');

            //Evaluate whether to remove from string or set to 'undefined'.
            //string already existed. remove current selecttion
            if (prevSelections.length > 1) {
                prevSelections = prevSelections.replace(',' + arab, '');
                prevSelections = prevSelections.replace(arab + ',', '');
                $('#txtEthnicity').val(prevSelections);
            }

            else { $('#txtEthnicity').val('(not defined)'); }

            UpdateEthnicityTotal(-1);

        }

            //btnArab was NOT selected so let's select it
        else {
            $('#txtEthnicity').val(BuildEthnicityString(arab));
            $('#btnArab').removeClass();
            $('#btnArab').addClass('btn btn-sm btn-block selected');

            UpdateEthnicityTotal(1);

        }

    }



    //Asian was selected
    if (buttonName == 'btnAsian') {

        //see if Asian was already selected
        var currentClass = $('#btnAsian').attr('class');

        //btnAsian was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {

            $('#btnAsian').removeClass();
            $('#btnAsian').addClass('btn btn-sm btn-block');

            //Evaluate whether to remove from string or set to 'undefined'.
            //string already existed. remove current selecttion
            if (prevSelections.length > 1) {
                prevSelections = prevSelections.replace(',' + asian, '');
                prevSelections = prevSelections.replace(asian + ',', '');
                $('#txtEthnicity').val(prevSelections);
            }

            else { $('#txtEthnicity').val('(not defined)'); }

            UpdateEthnicityTotal(-1);


        }

            //btnAsian was NOT selected so let's select it
        else {
            $('#txtEthnicity').val(BuildEthnicityString(asian));
            $('#btnAsian').removeClass();
            $('#btnAsian').addClass('btn btn-sm btn-block selected');

            UpdateEthnicityTotal(1);

        }

    }



    //AsianNonOriental was selected
    if (buttonName == 'btnAsianNonOriental') {

        //see if Asian was already selected
        var currentClass = $('#btnAsianNonOriental').attr('class');

        //btnAsianNonOriental was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {
            $('#btnAsianNonOriental').removeClass();
            $('#btnAsianNonOriental').addClass('btn btn-sm btn-block');

            //Evaluate whether to remove from string or set to 'undefined'.
            //string already existed. remove current selecttion
            if (prevSelections.length > 1) {
                prevSelections = prevSelections.replace(',' + asianNonOriental, '');
                prevSelections = prevSelections.replace(asianNonOriental + ',', '');
                $('#txtEthnicity').val(prevSelections);
            }

            else { $('#txtEthnicity').val('(not defined)'); }

            UpdateEthnicityTotal(-1);

        }

            //btnAsianNonOriental was NOT selected so let's select it
        else {
            $('#txtEthnicity').val(BuildEthnicityString(asianNonOriental));
            $('#btnAsianNonOriental').removeClass();
            $('#btnAsianNonOriental').addClass('btn btn-sm btn-block selected');

            UpdateEthnicityTotal(1);

        }

    }



    //btnFrench was selected
    if (buttonName == 'btnFrench') {

        //see if btnFrench was already selected
        var currentClass = $('#btnFrench').attr('class');

        //btnFrench was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {

            $('#btnFrench').removeClass();
            $('#btnFrench').addClass('btn btn-sm btn-block');

            //Evaluate whether to remove from string or set to 'undefined'.
            //string already existed. remove current selecttion
            if (prevSelections.length > 1) {
                prevSelections = prevSelections.replace(',' + french, '');
                prevSelections = prevSelections.replace(french + ',', '');
                $('#txtEthnicity').val(prevSelections);
            }

            else { $('#txtEthnicity').val('(not defined)'); }

            UpdateEthnicityTotal(-1);


        }

            //btnFrench was NOT selected so let's select it
        else {
            $('#txtEthnicity').val(BuildEthnicityString(french));
            $('#btnFrench').removeClass();
            $('#btnFrench').addClass('btn btn-sm btn-block selected');

            UpdateEthnicityTotal(1);

        }

    }


    //btnGerman was selected
    if (buttonName == 'btnGerman') {

        //see if btnGerman was already selected
        var currentClass = $('#btnGerman').attr('class');

        //btnGerman was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {

            $('#btnGerman').removeClass();
            $('#btnGerman').addClass('btn btn-sm btn-block');

            //Evaluate whether to remove from string or set to 'undefined'.
            //string already existed. remove current selecttion
            if (prevSelections.length > 1) {
                prevSelections = prevSelections.replace(',' + german, '');
                prevSelections = prevSelections.replace(german + ',', '');
                $('#txtEthnicity').val(prevSelections);
            }

            else { $('#txtEthnicity').val('(not defined)'); }

            UpdateEthnicityTotal(-1);


        }

            //btnGerman was NOT selected so let's select it
        else {
            $('#txtEthnicity').val(BuildEthnicityString(german));
            $('#btnGerman').removeClass();
            $('#btnGerman').addClass('btn btn-sm btn-block selected');

            UpdateEthnicityTotal(1);

        }

    }



    //btnHispanic was selected
    if (buttonName == 'btnHispanic') {

        //see if btnHispanic was already selected
        var currentClass = $('#btnHispanic').attr('class');

        //btnHispanic was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {

            $('#btnHispanic').removeClass();
            $('#btnHispanic').addClass('btn btn-sm btn-block');

            //Evaluate whether to remove from string or set to 'undefined'.
            //string already existed. remove current selecttion
            if (prevSelections.length > 1) {
                prevSelections = prevSelections.replace(',' + hispanic, '');
                prevSelections = prevSelections.replace(hispanic + ',', '');
                $('#txtEthnicity').val(prevSelections);
            }

            else { $('#txtEthnicity').val('(not defined)'); }

            UpdateEthnicityTotal(-1);


        }

            //btnHispanic was NOT selected so let's select it
        else {
            $('#txtEthnicity').val(BuildEthnicityString(hispanic));
            $('#btnHispanic').removeClass();
            $('#btnHispanic').addClass('btn btn-sm btn-block selected');

            UpdateEthnicityTotal(1);

        }

    }


    //btnItalian was selected
    if (buttonName == 'btnItalian') {

        //see if btnItalian was already selected
        var currentClass = $('#btnItalian').attr('class');

        //btnItalian was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {

            $('#btnItalian').removeClass();
            $('#btnItalian').addClass('btn btn-sm btn-block');

            //Evaluate whether to remove from string or set to 'undefined'.
            //string already existed. remove current selecttion
            if (prevSelections.length > 1) {
                prevSelections = prevSelections.replace(',' + italian, '');
                prevSelections = prevSelections.replace(italian + ',', '');
                $('#txtEthnicity').val(prevSelections);
            }

            else { $('#txtEthnicity').val('(not defined)'); }

            UpdateEthnicityTotal(-1);

        }

            //btnItalian was NOT selected so let's select it
        else {
            $('#txtEthnicity').val(BuildEthnicityString(italian));
            $('#btnItalian').removeClass();
            $('#btnItalian').addClass('btn btn-sm btn-block selected');

            UpdateEthnicityTotal(1);

        }

    }


    //btnJewish was selected
    if (buttonName == 'btnJewish') {

        //see if btnItalian was already selected
        var currentClass = $('#btnJewish').attr('class');

        //btnJewish was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {

            $('#btnJewish').removeClass();
            $('#btnJewish').addClass('btn btn-sm btn-block');

            //Evaluate whether to remove from string or set to 'undefined'.
            //string already existed. remove current selecttion
            if (prevSelections.length > 1) {
                prevSelections = prevSelections.replace(',' + jewish, '');
                prevSelections = prevSelections.replace(jewish + ',', '');
                $('#txtEthnicity').val(prevSelections);
            }

            else { $('#txtEthnicity').val('(not defined)'); }

            UpdateEthnicityTotal(-1);


        }

            //btnJewish was NOT selected so let's select it
        else {
            $('#txtEthnicity').val(BuildEthnicityString(jewish));
            $('#btnJewish').removeClass();
            $('#btnJewish').addClass('btn btn-sm btn-block selected');

            UpdateEthnicityTotal(1);

        }

    }



    //btnMiscellaneous was selected
    if (buttonName == 'btnMiscellaneous') {

        //see if btnMiscellaneous was already selected
        var currentClass = $('#btnMiscellaneous').attr('class');

        //btnMiscellaneous was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {

            $('#btnMiscellaneous').removeClass();
            $('#btnMiscellaneous').addClass('btn btn-sm btn-block');

            //Evaluate whether to remove from string or set to 'undefined'.
            //string already existed. remove current selecttion
            if (prevSelections.length > 1) {
                prevSelections = prevSelections.replace(',' + miscellaneous, '');
                prevSelections = prevSelections.replace(miscellaneous + ',', '');
                $('#txtEthnicity').val(prevSelections);
            }

            else { $('#txtEthnicity').val('(not defined)'); }

            UpdateEthnicityTotal(-1);


        }

            //btnMiscellaneous was NOT selected so let's select it
        else {
            $('#txtEthnicity').val(BuildEthnicityString(miscellaneous));
            $('#btnMiscellaneous').removeClass();
            $('#btnMiscellaneous').addClass('btn btn-sm btn-block selected');

            UpdateEthnicityTotal(1);

        }

    }


    //btnNorthernEuropean was selected
    if (buttonName == 'btnNorthernEuropean') {

        //see if btnNorthernEuropean was already selected
        var currentClass = $('#btnNorthernEuropean').attr('class');

        //btnNorthernEuropean was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {

            $('#btnNorthernEuropean').removeClass();
            $('#btnNorthernEuropean').addClass('btn btn-sm btn-block');

            //Evaluate whether to remove from string or set to 'undefined'.
            //string already existed. remove current selecttion
            if (prevSelections.length > 1) {
                prevSelections = prevSelections.replace(',' + northernEurpoean, '');
                prevSelections = prevSelections.replace(northernEurpoean + ',', '');
                $('#txtEthnicity').val(prevSelections);
            }

            else { $('#txtEthnicity').val('(not defined)'); }

            UpdateEthnicityTotal(-1);


        }

            //btnEuropean was NOT selected so let's select it
        else {
            $('#txtEthnicity').val(BuildEthnicityString(northernEurpoean));
            $('#btnNorthernEuropean').removeClass();
            $('#btnNorthernEuropean').addClass('btn btn-sm btn-block selected');

            UpdateEthnicityTotal(1);

        }

    }


    //btnPolynesian was selected
    if (buttonName == 'btnPolynesian') {

        //see if btnPolynesian was already selected
        var currentClass = $('#btnPolynesian').attr('class');

        //btnPolynesian was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {

            $('#btnPolynesian').removeClass();
            $('#btnPolynesian').addClass('btn btn-sm btn-block');

            //Evaluate whether to remove from string or set to 'undefined'.
            //string already existed. remove current selecttion
            if (prevSelections.length > 1) {
                prevSelections = prevSelections.replace(',' + polynesian, '');
                prevSelections = prevSelections.replace(polynesian + ',', '');
                $('#txtEthnicity').val(prevSelections);
            }

            else { $('#txtEthnicity').val('(not defined)'); }

            UpdateEthnicityTotal(-1);


        }

            //btnPolynesian was NOT selected so let's select it
        else {
            $('#txtEthnicity').val(BuildEthnicityString(polynesian));
            $('#btnPolynesian').removeClass();
            $('#btnPolynesian').addClass('btn btn-sm btn-block selected');

            UpdateEthnicityTotal(1);

        }

    }


    //btnScottishIrish was selected
    if (buttonName == 'btnScottishIrish') {

        //see if btnScottishIrish was already selected
        var currentClass = $('#btnScottishIrish').attr('class');

        //btnScottishIrish was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {

            $('#btnScottishIrish').removeClass();
            $('#btnScottishIrish').addClass('btn btn-sm btn-block');

            //Evaluate whether to remove from string or set to 'undefined'.
            //string already existed. remove current selecttion
            if (prevSelections.length > 1) {
                prevSelections = prevSelections.replace(',' + scottishIrish, '');
                prevSelections = prevSelections.replace(scottishIrish + ',', '');
                $('#txtEthnicity').val(prevSelections);
            }

            else { $('#txtEthnicity').val('(not defined)'); }

            UpdateEthnicityTotal(-1);


        }

            //btnScottishIrish was NOT selected so let's select it
        else {
            $('#txtEthnicity').val(BuildEthnicityString(scottishIrish));
            $('#btnScottishIrish').removeClass();
            $('#btnScottishIrish').addClass('btn btn-sm btn-block selected');

            UpdateEthnicityTotal(1);

        }

    }


    //btnSouthernEuropean was selected
    if (buttonName == 'btnSouthernEuropean') {

        //see if btnSouthernEuropean was already selected
        var currentClass = $('#btnSouthernEuropean').attr('class');

        //btnSouthernEuropean was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {

            $('#btnSouthernEuropean').removeClass();
            $('#btnSouthernEuropean').addClass('btn btn-sm btn-block');

            //Evaluate whether to remove from string or set to 'undefined'.
            //string already existed. remove current selecttion
            if (prevSelections.length > 1) {
                prevSelections = prevSelections.replace(',' + southernEuropean, '');
                prevSelections = prevSelections.replace(southernEuropean + ',', '');
                $('#txtEthnicity').val(prevSelections);
            }

            else { $('#txtEthnicity').val('(not defined)'); }

            UpdateEthnicityTotal(-1);


        }

            //btnSouthernEuropean was NOT selected so let's select it
        else {
            $('#txtEthnicity').val(BuildEthnicityString(southernEuropean));
            $('#btnSouthernEuropean').removeClass();
            $('#btnSouthernEuropean').addClass('btn btn-sm btn-block selected');

            UpdateEthnicityTotal(1);

        }

    }


    //btnUnclassified was selected
    if (buttonName == 'btnUnclassified') {

        //see if btnUnclassified was already selected
        var currentClass = $('#btnUnclassified').attr('class');

        //btnUnclassified was already selected but now is to be turned off
        if (currentClass == 'btn btn-sm btn-block selected') {

            $('#btnUnclassified').removeClass();
            $('#btnUnclassified').addClass('btn btn-sm btn-block');

            //Evaluate whether to remove from string or set to 'undefined'.
            //string already existed. remove current selecttion
            if (prevSelections.length > 1) {
                prevSelections = prevSelections.replace(',' + unclassified, '');
                prevSelections = prevSelections.replace(unclassified + ',', '');
                $('#txtEthnicity').val(prevSelections);
            }

            else { $('#txtEthnicity').val('(not defined)'); }

            UpdateEthnicityTotal(-1);


        }

            //btnUnclassified was NOT selected so let's select it
        else {
            $('#txtEthnicity').val(BuildEthnicityString(unclassified));
            $('#btnUnclassified').removeClass();
            $('#btnUnclassified').addClass('btn btn-sm btn-block selected');

            UpdateEthnicityTotal(1);

        }

    }


    //Hide checkboxes for 2.5 seconds - give time to API call to complete
    $('#ethnicityControlBlock').block
    ({
        message: '<h5 class="text-center"><br /><br /><span class="fa fa-2x fa-cog fa-spin"></span></h5>',
        css: { width: '100%', height: '100%', border: '1px solid #dddddd' }
    });

    setTimeout(function ()
    { $('#ethnicityControlBlock').unblock(); }, 2500);

    BuildPostUrl();
    CalculateAddressedCount();

}



function UpdateEthnicityTotal(increment) {

    var currentTotal = parseInt($("#hidTotalEthnicitySelected").val());
    var newTotal = currentTotal + parseInt(increment);

    if (newTotal < 1)
    { $("#ethnicityLabel").html('No Filter'); }

    else
    { $("#ethnicityLabel").html(newTotal + ' selected'); }


    $("#txtTotalEthnicitySelected").val(newTotal);
    $("#hidTotalEthnicitySelected").val(newTotal);


}



function BuildEthnicityString(selectedItem) {

    //look to see what is already selected.
    var prevSelections = $('#txtEthnicity').val();
    var results = "";

    //something else was already selected
    if (prevSelections != '(not defined)')
    { results = prevSelections + "," + selectedItem; }

        //nothing yet selected
    else
    { results = selectedItem; }


    return results;

}






//API  
function CalculateAddressedCount() {

    $('#resultsBlock').block
    ({
        message: '<div><br/><br/><br/><br/><h5><span class="fa fa-2x fa-cog fa-spin"></span>&nbsp;Targeting <u>your</u> customers.....</h5><br/><br/><br/><br/><br/><br/><br/><br/></div>',
        css: { width: '100%', height: '100%', border: '1px solid #dddddd' }
    });



    var count = 0;
    var postURL = $("#txtUrlText").val();

    $.ajax({
        url: postURL,
        type: "GET",
        dataType: "json",
        success: function (msg) {
            count = parseInt(msg);

            if (count <= 0) {
                $("#returnedResults").hide();
                $("#noResults").show();
            }

            else {
                $("#returnedResults").show();
                $("#noResults").hide();
                $("#addressedMailCount").html(Formatter.Commas(count));
                $("#txtCount").val(count);

            }


            $('#resultsBlock').unblock();

        },

        error: function (ex) {
            console.log('postURL: ' + postURL);
            console.log('error calling api...');
            $("#addressedMailCount").html("Error" + ex.statusText);
            $('#resultsBlock').unblock();
        }

    });



}



function BuildPostUrl() {

    var radius = $("#ddlRadiusValue").val();
    var radiusType = $("#ddlRadiusType").val();

    var lat = $("#txtLatitude").val();
    var lon = $("#txtLongitude").val();
    var gender = $("#txtGender").val();
    var homeOwnership = $("#txtHomeOwnership").val();
    var income = $("#txtCombinedIncome").val();
    var ethnicity = $("#txtEthnicity").val();
    var children = $("#txtChildren").val();
    var married = $("#txtMartialStatus").val();
    var ages = $("#txtAgeRanges").val();
    var propertyValue = $("#txtPropertyValue").val();
    var netWorth = $("#txtNetWorth").val();
    var zipCodes = $("#txtZipCodes").val();

    var postUrl = "//ktools.eddmsite.com/AddressedMailCounts.ashx";

    postUrl = postUrl + "?Radius=" + radius;
    postUrl = postUrl + "&Lat=" + lat;
    postUrl = postUrl + "&Lon=" + lon;
    postUrl = postUrl + "&RadiusType=" + radiusType;
    postUrl = postUrl + "&CountOnly=True";
    postUrl = postUrl + "&Gender=" + gender;
    postUrl = postUrl + "&HomeOwnership=" + homeOwnership;
    postUrl = postUrl + "&Income=" + income;
    postUrl = postUrl + "&Ethnicity=" + ethnicity;
    postUrl = postUrl + "&KidsPresent=" + children;
    postUrl = postUrl + "&MaritalStatus=" + married;
    postUrl = postUrl + "&AgeRange=" + ages;
    postUrl = postUrl + "&ZipCode=" + zipCodes;
    postUrl = postUrl + "&HomeMktVal=" + propertyValue;
    postUrl = postUrl + "&NetWorth=" + netWorth;

    $("#txtUrlText").val(ReplaceAll(postUrl, '(not defined)', ''));
    $("#txtAPIUrl").val(ReplaceAll(postUrl, '(not defined)', ''));

}





//CONTROLS / BUTTONS
function GoToStep2() {



    //Zip Codes is selected.  Check string.
    if ($('#radZipCodes').is(':checked')) {

        var zipCodeLength = $("#txtZipCodes").val();

        //Zip Code string is too short
        if (zipCodeLength.length < 5) {

            $('#validationSummary').show();
            $('#validationSummary').removeAttr("class");
            $('#validationSummary').attr("class", "alert alert-danger");
            $("#validationMessage").html("A valid 5 digit Zip Code is required.");
            $('#hypNextZipList').removeAttr("class");
            $('#hypNextZipList').attr("class", "btn btn-default disabled");
            $('#zipCodesListControlBlock').removeAttr("class");
            $('#zipCodesListControlBlock').attr("class", "form-group has-error");
            $("#zipCodesListLabel").removeAttr("class");
            $("#zipCodesListLabel").attr("class", "label label-danger");


        }

            //Zip Codes string passes length test
        else {

            //hide error message, skin button, skin control
            $('#validationSummary').hide();
            $('#hypNextZipList').removeAttr("class");
            $('#hypNextZipList').attr("class", "btn btn-cta");
            $('#txtZipCodesList').removeAttr("class");
            $('#txtZipCodesList').attr("class", "form-control");
            $('#zipCodesListControlBlock').removeAttr("class");
            $('#zipCodesListControlBlock').attr("class", "form-group");
            $("#zipCodesListLabel").removeAttr("class");
            $("#zipCodesListLabel").attr("class", "label formLabel formLabelRequired");

            BuildPostUrl();
            UpdateOrderSteps(2);

            $("#introBlock").hide(1000);
            $("#targetAreaBlock").hide(1000);

            $("#resultsBlock").show(1000);
            $("#filtersBlock").show(1000);

            CalculateAddressedCount();

        }



    }

        //Radius / Address was selected
    else {

        UpdateOrderSteps(2);

        $("#introBlock").hide(1000);
        $("#targetAreaBlock").hide(1000);

        $("#resultsBlock").show(1000);
        $("#filtersBlock").show(1000);

        CalculateAddressedCount();

    }



}



$('a[data-action=rotateIcon]').click(function (e) {

    //No postback
    e.preventDefault();

    var filterTriggered = $(this).attr('id');
    var currentClass = "";




    if (filterTriggered == 'homeOwnershipSelect') {

        currentClass = $('#homeOwnershipIcon').attr('class');

        //if NOT expanded then show minus
        if (currentClass == 'fa fa-plus fa-border') {
            $('#homeOwnershipIcon').removeAttr('class');
            $('#homeOwnershipIcon').attr('class', 'fa fa-minus fa-border');
        }

        else {
            $('#homeOwnershipIcon').removeAttr('class');
            $('#homeOwnershipIcon').attr('class', 'fa fa-plus fa-border');
        }

        //set other icons to plus
        $('#maritalStatusIcon').removeAttr('class');
        $('#maritalStatusIcon').attr('class', 'fa fa-plus fa-border');

        $('#childrenIcon').removeAttr('class');
        $('#childrenIcon').attr('class', 'fa fa-plus fa-border');

        $('#genderIcon').removeAttr('class');
        $('#genderIcon').attr('class', 'fa fa-plus fa-border');

        $('#incomeIcon').removeAttr('class');
        $('#incomeIcon').attr('class', 'fa fa-plus fa-border');

        $('#ageIcon').removeAttr('class');
        $('#ageIcon').attr('class', 'fa fa-plus fa-border');

        $('#propertyIcon').removeAttr('class');
        $('#propertyIcon').attr('class', 'fa fa-plus fa-border');

        $('#ethnicityIcon').removeAttr('class');
        $('#ethnicityIcon').attr('class', 'fa fa-plus fa-border');

        $('#netWorthIcon').removeAttr('class');
        $('#netWorthIcon').attr('class', 'fa fa-plus fa-border');

    }


    if (filterTriggered == 'maritalStatusSelect') {

        currentClass = $('#maritalStatusIcon').attr('class');

        //if NOT expanded then show minus
        if (currentClass == 'fa fa-plus fa-border') {
            $('#maritalStatusIcon').removeAttr('class');
            $('#maritalStatusIcon').attr('class', 'fa fa-minus fa-border');
        }

        else {
            $('#maritalStatusIcon').removeAttr('class');
            $('#maritalStatusIcon').attr('class', 'fa fa-plus fa-border');
        }

        //set other icons to plus
        $('#homeOwnershipIcon').removeAttr('class');
        $('#homeOwnershipIcon').attr('class', 'fa fa-plus fa-border');

        $('#childrenIcon').removeAttr('class');
        $('#childrenIcon').attr('class', 'fa fa-plus fa-border');

        $('#genderIcon').removeAttr('class');
        $('#genderIcon').attr('class', 'fa fa-plus fa-border');

        $('#incomeIcon').removeAttr('class');
        $('#incomeIcon').attr('class', 'fa fa-plus fa-border');

        $('#ageIcon').removeAttr('class');
        $('#ageIcon').attr('class', 'fa fa-plus fa-border');

        $('#propertyIcon').removeAttr('class');
        $('#propertyIcon').attr('class', 'fa fa-plus fa-border');

        $('#ethnicityIcon').removeAttr('class');
        $('#ethnicityIcon').attr('class', 'fa fa-plus fa-border');

        $('#netWorthIcon').removeAttr('class');
        $('#netWorthIcon').attr('class', 'fa fa-plus fa-border');

    }



    if (filterTriggered == 'childrenSelect') {

        currentClass = $('#childrenIcon').attr('class');

        //if NOT expanded then show minus
        if (currentClass == 'fa fa-plus fa-border') {
            $('#childrenIcon').removeAttr('class');
            $('#childrenIcon').attr('class', 'fa fa-minus fa-border');
        }

        else {
            $('#childrenIcon').removeAttr('class');
            $('#childrenIcon').attr('class', 'fa fa-plus fa-border');
        }

        //set other icons to plus
        $('#homeOwnershipIcon').removeAttr('class');
        $('#homeOwnershipIcon').attr('class', 'fa fa-plus fa-border');

        $('#maritalStatusIcon').removeAttr('class');
        $('#maritalStatusIcon').attr('class', 'fa fa-plus fa-border');

        $('#genderIcon').removeAttr('class');
        $('#genderIcon').attr('class', 'fa fa-plus fa-border');

        $('#incomeIcon').removeAttr('class');
        $('#incomeIcon').attr('class', 'fa fa-plus fa-border');

        $('#ageIcon').removeAttr('class');
        $('#ageIcon').attr('class', 'fa fa-plus fa-border');

        $('#propertyIcon').removeAttr('class');
        $('#propertyIcon').attr('class', 'fa fa-plus fa-border');

        $('#ethnicityIcon').removeAttr('class');
        $('#ethnicityIcon').attr('class', 'fa fa-plus fa-border');

        $('#netWorthIcon').removeAttr('class');
        $('#netWorthIcon').attr('class', 'fa fa-plus fa-border');

    }



    if (filterTriggered == 'genderSelect') {

        currentClass = $('#genderIcon').attr('class');

        //if NOT expanded then show minus
        if (currentClass == 'fa fa-plus fa-border') {
            $('#genderIcon').removeAttr('class');
            $('#genderIcon').attr('class', 'fa fa-minus fa-border');
        }

        else {
            $('#genderIcon').removeAttr('class');
            $('#genderIcon').attr('class', 'fa fa-plus fa-border');
        }

        //set other icons to plus
        $('#homeOwnershipIcon').removeAttr('class');
        $('#homeOwnershipIcon').attr('class', 'fa fa-plus fa-border');

        $('#maritalStatusIcon').removeAttr('class');
        $('#maritalStatusIcon').attr('class', 'fa fa-plus fa-border');

        $('#childrenIcon').removeAttr('class');
        $('#childrenIcon').attr('class', 'fa fa-plus fa-border');

        $('#incomeIcon').removeAttr('class');
        $('#incomeIcon').attr('class', 'fa fa-plus fa-border');

        $('#ageIcon').removeAttr('class');
        $('#ageIcon').attr('class', 'fa fa-plus fa-border');

        $('#propertyIcon').removeAttr('class');
        $('#propertyIcon').attr('class', 'fa fa-plus fa-border');

        $('#ethnicityIcon').removeAttr('class');
        $('#ethnicityIcon').attr('class', 'fa fa-plus fa-border');

        $('#netWorthIcon').removeAttr('class');
        $('#netWorthIcon').attr('class', 'fa fa-plus fa-border');

    }



    if (filterTriggered == 'incomeSelect') {

        currentClass = $('#incomeIcon').attr('class');

        //if NOT expanded then show minus
        if (currentClass == 'fa fa-plus fa-border') {
            $('#incomeIcon').removeAttr('class');
            $('#incomeIcon').attr('class', 'fa fa-minus fa-border');
        }

        else {
            $('#incomeIcon').removeAttr('class');
            $('#incomeIcon').attr('class', 'fa fa-plus fa-border');
        }

        //set other icons to plus
        $('#homeOwnershipIcon').removeAttr('class');
        $('#homeOwnershipIcon').attr('class', 'fa fa-plus fa-border');

        $('#maritalStatusIcon').removeAttr('class');
        $('#maritalStatusIcon').attr('class', 'fa fa-plus fa-border');

        $('#childrenIcon').removeAttr('class');
        $('#childrenIcon').attr('class', 'fa fa-plus fa-border');

        $('#genderIcon').removeAttr('class');
        $('#genderIcon').attr('class', 'fa fa-plus fa-border');

        $('#ageIcon').removeAttr('class');
        $('#ageIcon').attr('class', 'fa fa-plus fa-border');

        $('#propertyIcon').removeAttr('class');
        $('#propertyIcon').attr('class', 'fa fa-plus fa-border');

        $('#ethnicityIcon').removeAttr('class');
        $('#ethnicityIcon').attr('class', 'fa fa-plus fa-border');

        $('#netWorthIcon').removeAttr('class');
        $('#netWorthIcon').attr('class', 'fa fa-plus fa-border');

    }


    if (filterTriggered == 'ageSelect') {

        currentClass = $('#ageIcon').attr('class');

        //if NOT expanded then show minus
        if (currentClass == 'fa fa-plus fa-border') {
            $('#ageIcon').removeAttr('class');
            $('#ageIcon').attr('class', 'fa fa-minus fa-border');
        }

        else {
            $('#ageIcon').removeAttr('class');
            $('#ageIcon').attr('class', 'fa fa-plus fa-border');
        }

        //set other icons to plus
        $('#homeOwnershipIcon').removeAttr('class');
        $('#homeOwnershipIcon').attr('class', 'fa fa-plus fa-border');

        $('#maritalStatusIcon').removeAttr('class');
        $('#maritalStatusIcon').attr('class', 'fa fa-plus fa-border');

        $('#childrenIcon').removeAttr('class');
        $('#childrenIcon').attr('class', 'fa fa-plus fa-border');

        $('#genderIcon').removeAttr('class');
        $('#genderIcon').attr('class', 'fa fa-plus fa-border');

        $('#incomeIcon').removeAttr('class');
        $('#incomeIcon').attr('class', 'fa fa-plus fa-border');

        $('#propertyIcon').removeAttr('class');
        $('#propertyIcon').attr('class', 'fa fa-plus fa-border');

        $('#ethnicityIcon').removeAttr('class');
        $('#ethnicityIcon').attr('class', 'fa fa-plus fa-border');

        $('#netWorthIcon').removeAttr('class');
        $('#netWorthIcon').attr('class', 'fa fa-plus fa-border');

    }



    if (filterTriggered == 'propertySelect') {

        currentClass = $('#propertyIcon').attr('class');

        //if NOT expanded then show minus
        if (currentClass == 'fa fa-plus fa-border') {
            $('#propertyIcon').removeAttr('class');
            $('#propertyIcon').attr('class', 'fa fa-minus fa-border');
        }

        else {
            $('#propertyIcon').removeAttr('class');
            $('#propertyIcon').attr('class', 'fa fa-plus fa-border');
        }

        //set other icons to plus
        $('#homeOwnershipIcon').removeAttr('class');
        $('#homeOwnershipIcon').attr('class', 'fa fa-plus fa-border');

        $('#maritalStatusIcon').removeAttr('class');
        $('#maritalStatusIcon').attr('class', 'fa fa-plus fa-border');

        $('#childrenIcon').removeAttr('class');
        $('#childrenIcon').attr('class', 'fa fa-plus fa-border');

        $('#genderIcon').removeAttr('class');
        $('#genderIcon').attr('class', 'fa fa-plus fa-border');

        $('#incomeIcon').removeAttr('class');
        $('#incomeIcon').attr('class', 'fa fa-plus fa-border');

        $('#ethnicityIcon').removeAttr('class');
        $('#ethnicityIcon').attr('class', 'fa fa-plus fa-border');

        $('#ageIcon').removeAttr('class');
        $('#ageIcon').attr('class', 'fa fa-plus fa-border');

        $('#netWorthIcon').removeAttr('class');
        $('#netWorthIcon').attr('class', 'fa fa-plus fa-border');

    }



    if (filterTriggered == 'netWorthSelect') {

        currentClass = $('#netWorthIcon').attr('class');

        //if NOT expanded then show minus
        if (currentClass == 'fa fa-plus fa-border') {
            $('#netWorthIcon').removeAttr('class');
            $('#netWorthIcon').attr('class', 'fa fa-minus fa-border');
        }

        else {
            $('#netWorthIcon').removeAttr('class');
            $('#netWorthIcon').attr('class', 'fa fa-plus fa-border');
        }

        //set other icons to plus
        $('#homeOwnershipIcon').removeAttr('class');
        $('#homeOwnershipIcon').attr('class', 'fa fa-plus fa-border');

        $('#maritalStatusIcon').removeAttr('class');
        $('#maritalStatusIcon').attr('class', 'fa fa-plus fa-border');

        $('#childrenIcon').removeAttr('class');
        $('#childrenIcon').attr('class', 'fa fa-plus fa-border');

        $('#genderIcon').removeAttr('class');
        $('#genderIcon').attr('class', 'fa fa-plus fa-border');

        $('#incomeIcon').removeAttr('class');
        $('#incomeIcon').attr('class', 'fa fa-plus fa-border');

        $('#ethnicityIcon').removeAttr('class');
        $('#ethnicityIcon').attr('class', 'fa fa-plus fa-border');

        $('#ageIcon').removeAttr('class');
        $('#ageIcon').attr('class', 'fa fa-plus fa-border');

        $('#propertyIcon').removeAttr('class');
        $('#propertyIcon').attr('class', 'fa fa-plus fa-border');

    }



    if (filterTriggered == 'ethnicitySelect') {

        currentClass = $('#ethnicityIcon').attr('class');

        //if NOT expanded then show minus
        if (currentClass == 'fa fa-plus fa-border') {
            $('#ethnicityIcon').removeAttr('class');
            $('#ethnicityIcon').attr('class', 'fa fa-minus fa-border');
        }

        else {
            $('#ethnicityIcon').removeAttr('class');
            $('#ethnicityIcon').attr('class', 'fa fa-plus fa-border');
        }

        //set other icons to plus
        $('#homeOwnershipIcon').removeAttr('class');
        $('#homeOwnershipIcon').attr('class', 'fa fa-plus fa-border');

        $('#maritalStatusIcon').removeAttr('class');
        $('#maritalStatusIcon').attr('class', 'fa fa-plus fa-border');

        $('#childrenIcon').removeAttr('class');
        $('#childrenIcon').attr('class', 'fa fa-plus fa-border');

        $('#genderIcon').removeAttr('class');
        $('#genderIcon').attr('class', 'fa fa-plus fa-border');

        $('#incomeIcon').removeAttr('class');
        $('#incomeIcon').attr('class', 'fa fa-plus fa-border');

        $('#ageIcon').removeAttr('class');
        $('#ageIcon').attr('class', 'fa fa-plus fa-border');

        $('#propertyIcon').removeAttr('class');
        $('#propertyIcon').attr('class', 'fa fa-plus fa-border');

        $('#netWorthIcon').removeAttr('class');
        $('#netWorthIcon').attr('class', 'fa fa-plus fa-border');

    }

});



$("#resetFilters").click(function (event) {
    event.preventDefault();

    var numOfNetWorthCheckboxes = 11;
    var numOfPropValuesCheckboxes = 19;


    $('#btnHomeowner').removeClass();
    $('#btnHomeowner').addClass('btn btn-sm btn-block');
    $('#btnRenter').removeClass();
    $('#btnRenter').addClass('btn btn-sm btn-block');
    $('#homeOwnershipLabel').html('No Filter');
    $('#txtHomeOwnership').val('(not defined)');

    $('#btnMarried').removeClass();
    $('#btnMarried').addClass('btn btn-sm btn-block');
    $('#btnSingle').removeClass();
    $('#btnSingle').addClass('btn btn-sm btn-block');
    $('#txtMartialStatus').val('(not defined)');
    $('#maritalStatusLabel').html('No Filter');

    $('#btnChildren').removeClass();
    $('#btnChildren').addClass('btn btn-sm btn-block');
    $('#btnNoChildren').removeClass();
    $('#btnNoChildren').addClass('btn btn-sm btn-block');
    $('#txtChildren').val('(not defined)');
    $('#childrenLabel').html('No Filter');

    $("#minIncome").html('$0');
    $("#maxIncome").html('$250,000+');
    $('#txtCombinedIncome').val('(not defined)');
    $("#hidRawMinIncome").val(0);
    $("#hidRawMaxIncome").val(250000);
    $("#incomeLabel").html('No Filter');

    $("#minAge").html('18');
    $("#maxAge").html('75');
    $("#ageLabel").html('No Filter');
    $("#hidRawMinAge").val(18);
    $("#hidRawMaxAge").val(75);
    $('#txtAgeRanges').val('(not defined)');

    $("#minProperty").html('$0');
    $("#maxProperty").html('$500k+');
    $("#propertyLabel").html('No Filter');
    $("#hidRawMinProperty").val(0);
    $("#hidRawMaxProperty").val(500000);
    $('#txtPropertyValue').val('(not defined)');


    //NET WORTH
    for (i = 1; i <= numOfNetWorthCheckboxes; i++) {
        if ($("#chkNetWorth" + i).is(':checked'))
        { $("#chkNetWorth" + i).prop("checked", false); }
    }
    $("#chkSelectAllNetWorth").prop("checked", false);
    $("#selectAllNetWorthLabel").html("Select All");
    $("#netWorthLabel").html("No Filter");
    $('#txtNetWorth').val('(not defined)');
    $("#hidNetWorthTotalSelected").val("0");
    $("#txtNetWorthTotalSelected").val("0");



    //PROPERTY VALUE
    for (i = 1; i <= numOfPropValuesCheckboxes; i++) {
        if ($("#chkPropValue" + i).is(':checked'))
        { $("#chkPropValue" + i).prop("checked", false); }
    }
    $("#chkSelectAllPropValue").prop("checked", false);
    $("#selectAllPropValueLabel").html("Select All");
    $("#propertyLabel").html("No Filter");
    $('#txtPropertyValue').val('(not defined)');
    $("#hidPropValueTotalSelected").val("0");
    $("#txtPropValueTotalSelected").val("0");




    $('#btnMale').removeClass();
    $('#btnMale').addClass('btn btn-sm btn-block');
    $('#btnFemale').removeClass();
    $('#btnFemale').addClass('btn btn-sm btn-block');
    $('#txtGender').val('(not defined)');
    $('#genderLabel').html('No Filter');

    $('#txtEthnicity').val('(not defined)');
    $("#ethnicityLabel").html('No Filter');
    $('#btnAfricanAmerican').removeClass();
    $('#btnAfricanAmerican').addClass('btn btn-sm btn-block');
    $('#btnArab').removeClass();
    $('#btnArab').addClass('btn btn-sm btn-block');
    $('#btnAsian').removeClass();
    $('#btnAsian').addClass('btn btn-sm btn-block');
    $('#btnAsianNonOriental').removeClass();
    $('#btnAsianNonOriental').addClass('btn btn-sm btn-block');
    $('#btnFrench').removeClass();
    $('#btnFrench').addClass('btn btn-sm btn-block');
    $('#btnGerman').removeClass();
    $('#btnGerman').addClass('btn btn-sm btn-block');
    $('#btnHispanic').removeClass();
    $('#btnHispanic').addClass('btn btn-sm btn-block');
    $('#btnItalian').removeClass();
    $('#btnItalian').addClass('btn btn-sm btn-block');
    $('#btnJewish').removeClass();
    $('#btnJewish').addClass('btn btn-sm btn-block');
    $('#btnMiscellaneous').removeClass();
    $('#btnMiscellaneous').addClass('btn btn-sm btn-block');
    $('#btnNorthernEuropean').removeClass();
    $('#btnNorthernEuropean').addClass('btn btn-sm btn-block');
    $('#btnPolynesian').removeClass();
    $('#btnPolynesian').addClass('btn btn-sm btn-block');
    $('#btnScottishIrish').removeClass();
    $('#btnScottishIrish').addClass('btn btn-sm btn-block');
    $('#btnSouthernEuropean').removeClass();
    $('#btnSouthernEuropean').addClass('btn btn-sm btn-block');
    $('#btnUnclassified').removeClass();
    $('#btnUnclassified').addClass('btn btn-sm btn-block');
    $("#txtTotalEthnicitySelected").val('0');
    $("#hidTotalEthnicitySelected").val('0');

    BuildPostUrl();
    CalculateAddressedCount();
    UpdateOrderSteps(1);

});



$("#changeTarget").click(function (event) {

    event.preventDefault();

    $("#introBlock").show(1000);
    $("#targetAreaBlock").show(1000);

    $("#resultsBlock").hide(1000);
    $("#filtersBlock").hide(1000);

    UpdateOrderSteps(1);

});



$('#radAddress').click(function () {
    $('#addressBlock').show(1000);
    $('#zipCodesBlock').hide(1000);
});



$('#radZipCodes').click(function () {
    $('#addressBlock').hide(1000);
    $('#zipCodesBlock').show(1000);
});



$("#chkSelectAllNetWorth").click(function () {

    if ($(this).prop("checked")) {
        $(".networth").prop("checked", true);
        $("#selectAllNetWorthLabel").html("Deselect All");
        $("#txtNetWorth").val("(not defined)");
    }

    else {
        $(".networth").prop("checked", false);
        $("#selectAllNetWorthLabel").html("Select All");
    }


    $('.networth').click(function () {
        if ($(".networth").length == $(".networth:checked").length) {
            $("#chkSelectAllNetWorth").prop("checked", true);
        }
        else {
            $("#chkSelectAllNetWorth").prop("checked", false);
        }
    });

    BuildNetWorthString();
    BuildPostUrl();
    BuildNetWorthLabel();

});



$("#chkSelectAllPropValue").click(function () {

    if ($(this).prop("checked")) {
        $(".propval").prop("checked", true);
        $("#selectAllPropValueLabel").html("Deselect All");
        $("#txtPropertyValue").val("(not defined)");
    }

    else {
        $(".propval").prop("checked", false);
        $("#selectAllPropValueLabel").html("Select All");
    }


    $('.propval').click(function () {
        if ($(".propval").length == $(".propval:checked").length) {
            $("#chkSelectAllPropValue").prop("checked", true);
        }

        else {
            $("#chkSelectAllPropValue").prop("checked", false);
        }

    });

    BuildPropValueString();
    BuildPostUrl();
    BuildPropValueLabel();

});



$("#show-sample").click(function () {

    event.preventDefault();


    //Build the request url
    var hostName = window.location.hostname;
    var baseEndPoint = "http://" + hostName  + "/Resources/GetAddressedSampleSet.ashx?OutputSample=True&CountOnly=False";
    var radius = "&Radius=" + $("#ddlRadiusValue").val();
    var lat = "&Lat=" + $("#txtLatitude").val();
    var lon = "&Lon=" + $("#txtLongitude").val();
    var radiusType = "&RadiusType=" + $("#ddlRadiusType").val();

    var gender = "";
    if ($("#txtGender").val() != "(not defined)")
    { gender = "&Gender=" + $("#txtGender").val(); }

    var homeOwnership = "";
    if ($("#txtHomeOwnership").val() != "(not defined)")
    { homeOwnership = "&HomeOwnership=" + $("#txtHomeOwnership").val(); }

    var income = "";
    if ($("#txtCombinedIncome").val() != "(not defined)")
    { income = "&Income=" + $("#txtCombinedIncome").val(); }

    var children = "";
    if ($("#txtChildren").val() != "(not defined)")
    { children = "&KidsPresent=" + $("#txtChildren").val(); }

    var maritalStatus = "";
    if ($("#txtMartialStatus").val() != "(not defined)")
    { maritalStatus = "&MaritalStatus=" + $("#txtMartialStatus").val(); }

    var ageRange = "";
    if ($("#txtAgeRanges").val() != "(not defined)")
    { ageRange = "&AgeRange=" + $("#txtAgeRanges").val(); }

    var ethnicity = "";
    if ($("#txtEthnicity").val() != "(not defined)")
    { ethnicity = "&Ethnicity=" + $("#txtEthnicity").val(); }

    var zipcode = "";
    if ($("#txtZipCodes").val() != "(not defined)")
    { zipcode = "&ZipCode=" + $("#txtZipCodes").val(); }

    var propertyValue = "";
    if ($("#txtPropertyValue").val() != "(not defined)")
    { propertyValue = "&HomeMktVal=" + $("#txtPropertyValue").val(); }

    var netWorth = "";
    if ($("#txtNetWorth").val() != "(not defined)")
    { propertyValue = "&NetWorth=" + $("#txtNetWorth").val(); }


    var fullEndPoint = baseEndPoint + radius + lat + lon + radiusType + gender + homeOwnership + income + children + maritalStatus + ageRange + ethnicity + zipcode + propertyValue + netWorth;


    console.log(fullEndPoint);



    $('#sample-data').block
    ({
        message: '<div><br/><h5><span class="fa fa-2x fa-cog fa-spin"></span>&nbsp;Getting your sample.....</h5><br/><br/><br/></div>',
        css: { width: '100%', height: '100%', border: '1px solid #dddddd' }
    });


    $.ajax(
        {
            url: fullEndPoint,
            type: 'POST',

            success: function (response) {

                if (response != null && response != '') {

                    //Response is too short.  Something must be wrong.
                    if (response.length < 400)
                    { $("#sample-data").html('<div class="alert alert-danger"><h3 class="text-danger"><span class="fa fa-exclamation-circle fa-2x"></span>&nbsp;Uh oh, something went wrong.</h3><p>We\'re sorry but something went wrong when retrieving the sample. Please <a href="/Support">contact us</a> to request sample data.</p><p>&nbsp;</p></div>'); }
                    else
                    { $("#sample-data").html(response); }


                }

                $('#sample-data').unblock();

            },

            error: function (ex) {
                console.log("Show Sample Error: " + ex.statusText);
                $("#sample-data").html('<div class="alert alert-danger"><h3 class="text-danger"><span class="fa fa-exclamation-circle fa-2x"></span>&nbsp;Uh oh, something went wrong.</h3><p>We\'re sorry but something went wrong when retrieving the sample. Please <a href="/Support">contact us</a> to request sample data.</p><p>&nbsp;</p><p>Error Code: ' + ex.statusText + '</p></div>');
                $('#sample-data').unblock();

            }

        });

});








// HELPER / CALCULATE / CALL FUNCTIONS
function FindLatLonData() {


    //***** JSon Obj *******
    var EDDMTotal = 0;

    var address = $('#txtAddress').val();
    var zip = $('#txtZipCode').val();

    $.ajax(
        {
            url: '//demographics2.eddmsite.com/Resources/Geocode.ashx',
            type: 'POST',
            data:
                {
                    'A': address,
                    'Z': zip
                },

            success: function (data) {
                //strip out bad chars
                var msg2 = ReplaceAll(data, '(', '');
                var msg3 = ReplaceAll(msg2, ')', '');
                var msg4 = ReplaceAll(msg3, ';', '');
                var testObj = JSON.parse(msg4);

                $('#txtLatitude').val(testObj.Latitude);
                $('#txtLongitude').val(testObj.Longitude);

            },

            error: function (ex)
            { console.log("FindLatLon Error " + ex.statusText); }

        });
}



function EscapeRegExp(string) {
    return string.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
}



function ReplaceAll(string, find, replace) {
    return string.replace(new RegExp(EscapeRegExp(find), 'g'), replace);  //g flag replaces all
}



function GetPriceQuote(baseProductID, totalCount, pppLabel, totalCostLabel) {
    //CURRENTLY NOT USE. 10/29/2015.

    //mode
    var debug = "false";


    //Distribution ID
    var distributionID = 0;

    //Mark up
    var markUp = 0;


    //Mark up Type
    var markUpType = "Percent";


    //# of Drops
    var numOfDrops = 1;


    //# of Impressions
    var numImpressions = 1;


    //Hold Qty.  Currently not in use.
    var holdQTY = 0;


    //Extra copies.
    var extraCopies = 0;



    //Zip Code.  Should ALWAYS be populated.
    var zipCode = $('#txtZipCode').val();




    //***** JSon Obj *******
    var oPostData = {};
    oPostData.pid = baseProductID;
    oPostData.qty = totalCount;

    //Hold Qty.  Currently not in use.
    oPostData.hqty = holdQTY;

    //Extra copies. aka 'Ship QTY'
    oPostData.sqty = extraCopies;

    //DistID
    oPostData.distid = distributionID;

    //Drops
    oPostData.drops = numOfDrops;

    //Zip
    oPostData.zip = zipCode;

    //Design options
    oPostData.wd = "false";
    oPostData.wt = "false";

    //Mark up
    oPostData.m = markUp;
    oPostData.mt = markUpType;


    $.ajax(
        {
            type: 'POST',
            dataType: 'json',
            url: '/resources/PrintRateQuote.ashx',
            data: oPostData,

            success: function (msg) {

                if (msg != null && msg != '') {


                    var minPrice = msg.PricePerPiece * 1000;

                    //Override to min price if needed.
                    if (Number(msg.Price) < Number(minPrice)) {


                        var differenceInPrice = Number(minPrice) - Number(msg.Price);
                        var minPrice2 = differenceInPrice + Number(msg.Price);
                        var designFee = Number(msg.DesignPrice);

                        if (designFee > 0)
                        { minPrice2 = minPrice2 + designFee; }


                        msg.FormattedTotalPrice = '$' + minPrice2.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');

                        //find the place in the page to update
                        //$('#lblEstTotal').html(msg.FormattedTotalPrice);

                    }


                    else {
                        //console.log('minimum WAS met');
                    }

                    //find the place in the page to update...

                    $(totalCostLabel).html(msg.FormattedTotalPrice);
                    $(pppLabel).html(msg.FormattedPricePerPiece);

                }


                if (debug == "true") {

                    //Debugging
                    //console.log("************UpdatePriceQuote****************");
                    //end

                }



            },

            error: function () {
                //nothing yet
            }

        });


}



function CountSelectedCheckboxes(checkboxGroup) {

    var results = 0;
    var totNumNetWorthCheckboxes = 11;
    var totNumPropValueCheckboxes = 19;

    if (checkboxGroup == "NetWorth") {

        for (i = 1; i <= totNumNetWorthCheckboxes; i++) {
            if ($("#chkNetWorth" + i).is(':checked'))
            { results++; }
        }

        $("#txtNetWorthTotalSelected").val(results);
        $("#hidNetWorthTotalSelected").val(results);

    }


    if (checkboxGroup == "PropertyValue") {

        for (i = 1; i <= totNumPropValueCheckboxes; i++) {
            if ($("#chkPropValue" + i).is(':checked'))
            { results++; }
        }

        $("#txtPropValueTotalSelected").val(results);
        $("#hidPropValueTotalSelected").val(results);

    }


    return results;

}



function UpdateOrderSteps(stepNumber) {

    if (stepNumber == 1) {
        //pass the number for the CURRENT Step to be shown as current

        $("#liStep1").removeClass();
        $("#liStep2").removeClass();
        $("#liStep3").removeClass();
        $("#liStep4").removeClass();
        $("#liStep5").removeClass();

        $("#liStep1").addClass("current");
    }


    if (stepNumber == 2) {
        $("#liStep1").removeClass();
        $("#liStep2").removeClass();
        $("#liStep3").removeClass();
        $("#liStep4").removeClass();
        $("#liStep5").removeClass();

        $("#liStep1").addClass("visited");
        $("#liStep2").addClass("current");
    }

    if (stepNumber == 3) {
        $("#liStep1").removeClass();
        $("#liStep2").removeClass();
        $("#liStep3").removeClass();
        $("#liStep4").removeClass();
        $("#liStep5").removeClass();

        $("#liStep1").addClass("visited");
        $("#liStep2").addClass("visited");
        $("#liStep3").addClass("current");
    }

    if (stepNumber == 4) {
        $("#liStep1").removeClass();
        $("#liStep2").removeClass();
        $("#liStep3").removeClass();
        $("#liStep4").removeClass();
        $("#liStep5").removeClass();

        $("#liStep1").addClass("visited");
        $("#liStep2").addClass("visited");
        $("#liStep3").addClass("visited");
        $("#liStep4").addClass("current");
    }

    if (stepNumber == 5) {
        $("#liStep1").removeClass();
        $("#liStep2").removeClass();
        $("#liStep3").removeClass();
        $("#liStep4").removeClass();
        $("#liStep5").removeClass();

        $("#liStep1").addClass("visited");
        $("#liStep2").addClass("visited");
        $("#liStep3").addClass("visited");
        $("#liStep4").addClass("visited");
        $("#liStep5").addClass("current");
    }

}




function ValidateAddressAndZip() {

    var addressLength = $('#txtAddress').val().length;
    var zipcodeLength = $('#txtZipCode').val().length;

    //address and valid(ish) zip code exists
    if ((addressLength > 1) && (zipcodeLength >= 5)) {
        $('#addressInstructions').hide();
        $('#hypNext').removeAttr("class");
        $("#hypNext").attr("class", "btn btn-cta");

        //Hide buttons temporarily
        $('#targetAreaBlock').block
        ({
            message: '<h5><br /><br /><br /><br /><span class="fa fa-cog fa-spin"></span>&nbsp;Targeting with this address.....</h5>',
            css: { width: '100%', height: '100%', border: '1px solid #dddddd' }
        });


        //Get LatLong values right away
        FindLatLonData();

        //Wait for FindLatLonData to finish
        setTimeout(function () {
            BuildPostUrl();

            //unblock button
            $('#targetAreaBlock').unblock();

        }, 2500);

    }

    else {
        $('#addressInstructions').show();
        $('#hypNext').removeAttr("class");
        $("#hypNext").attr("class", "btn btn-default disabled");
    }

    //console.log("addressLength: " + addressLength);
    //console.log("zipcodeLength: " + zipcodeLength);

}




function CheckForRevisit() {

    //This function will check the controls to see if they contain content.  This likely happens if the user hits the 
    //'back' button from Step1-TargetReview.aspx. If so, it will re-able the button and hide warning.

    var address = $('#txtAddress').val();
    var zipCode = $('#txtZipCode').val();
    var zipCodeList = $('#txtZipCodesList').val();
    var results = false;



    //address and zip code was filled out
    if (address.length >= 5 && zipCode.length >= 5)
    { results = true; }

        //zip code(s) was filled out
    else if (zipCodeList.length >= 5)
    { results = true; }

        //must be first visit
    else
    { results = false; }

    return results;

}




function HideFirstTimeBlocks() {

    //This function hide the page elements which are visible to the user the FIRST time they use this page.
    //Since they have come 'back' to this page via the Back button, these elements should be disabled.

    //re-enable 'Go To Step2 buttons (both of them)'
    $('#hypNext').removeAttr("class");
    $("#hypNext").attr("class", "btn btn-cta");
    $("#addressInstructions").hide();

    $('#hypNextZipList').removeAttr("class");
    $("#hypNextZipList").attr("class", "btn btn-cta");
    $("#zipCodeInstructions").hide();

    //hide defineYourAreaToBegin
    $("#defineYourAreaToBegin").hide();


    //See which radio button is checked and show the appropriate block.
    if ($('#radZipCodes').is(':checked')) {
        $("#addressBlock").hide();
        $("#zipCodesBlock").show();
    }

    if ($('#radAddress').is(':checked')) {
        $("#addressBlock").show();
        $("#zipCodesBlock").hide();
    }

}




function PreselectFiltersAndLabels() {


    //Homeownership
    var homeownership = $("#txtHomeOwnership").val();

    if (homeownership == "O") {
        $('#btnHomeowner').removeClass();
        $('#btnHomeowner').addClass('btn btn-sm btn-block selected');

        //fill the label
        $('#homeOwnershipLabel').html('Homeowner');
    }

    if (homeownership == "R") {
        $('#btnRenter').removeClass();
        $('#btnRenter').addClass('btn btn-sm btn-block selected');

        //fill the label
        $('#homeOwnershipLabel').html('Renter');
    }





    //Marital Status
    var maritalStatus = $("#txtMartialStatus").val();

    if (maritalStatus == "M") {
        $('#btnMarried').removeClass();
        $('#btnMarried').addClass('btn btn-sm btn-block selected');

        //fill the label
        $('#maritalStatusLabel').html('Married');
    }

    if (maritalStatus == "S") {
        $('#btnSingle').removeClass();
        $('#btnSingle').addClass('btn btn-sm btn-block selected');

        //fill the label
        $('#maritalStatusLabel').html('Single');
    }





    //Kids Present
    var kidsPresent = $("#txtChildren").val();

    if (kidsPresent == "Y") {
        $('#btnChildren').removeClass();
        $('#btnChildren').addClass('btn btn-sm btn-block selected');

        //fill the label
        $('#childrenLabel').html('Children');
    }

    if (kidsPresent == "N") {
        $('#btnNoChildren').removeClass();
        $('#btnNoChildren').addClass('btn btn-sm btn-block selected');

        //fill the label
        $('#childrenLabel').html('No Children');
    }





    //Gender
    var gender = $("#txtGender").val();

    if (gender == "M") {
        $('#btnMale').removeClass();
        $('#btnMale').addClass('btn btn-sm btn-block selected');

        //fill the label
        $('#genderLabel').html('Male');
    }

    if (gender == "F") {
        $('#btnFemale').removeClass();
        $('#btnFemale').addClass('btn btn-sm btn-block selected');

        //fill the label
        $('#genderLabel').html('Female');
    }





    //Income
    var income = $("#txtCombinedIncome").val();
    var minIncome = $("#txtMinIncome").val();
    var maxIncome = $("#txtMaxIncome").val();

    if (income != "(not defined)") {

        //min label
        $("#minIncome").html(Formatter.CurrencyDollars(minIncome));


        //add '+' if maxed out
        if (maxIncome >= 250000)
        { $("#maxIncome").html(Formatter.CurrencyDollars(maxIncome) + '+'); }
        else
        { $("#maxIncome").html(Formatter.CurrencyDollars(maxIncome)); }


        //incomeLabel
        $("#incomeLabel").html(BuildIncomeLabel());

        //Reset the Income slider.
        var preselectedMinIncome = parseInt($("#txtMinIncome").val());
        var preselectedMaxIncome = parseInt($("#txtMaxIncome").val());
        $("#incomeSlider").slider({ values: [preselectedMinIncome, preselectedMaxIncome] });


    }





    //Age
    var minAge = $("#txtMinAge").val();
    var maxAge = $("#txtMaxAge").val();
    var ageRanges = $("#txtAgeRanges").val();

    if (ageRanges != "(not defined)") {

        //set the min and max labesl above the slider.
        //min label
        $("#minAge").html(minAge);

        //max age
        if (maxAge >= 75)
        { $("#maxAge").html(maxAge + '+'); }
        else
        { $("#maxAge").html(maxAge); }


        //reset the age range label
        $("#ageLabel").html(BuildAgeLabel());


        //Reset the Age slider.
        var preselectedMinAge = parseInt($("#txtMinAge").val());
        var preselectedMaxAge = parseInt($("#txtMaxAge").val());
        $("#ageSlider").slider({ values: [preselectedMinAge, preselectedMaxAge] });


    }





    //Property Value
    var selectedPropertyCount = $("#txtPropValueTotalSelected").val();
    var selectedPropertyValues = $("#txtPropertyValue").val();

    if (selectedPropertyValues != "(not defined)") {
        //checkboxes maintain their state when going 'back' so only the total needs to be recalcualted.
        $("#propertyLabel").html(selectedPropertyCount + " ranges selected");

    }




    //Net Worth
    var selectedNetWorthCount = $("#txtNetWorthTotalSelected").val();
    var selectedNetWorthValues = $("#txtNetWorth").val();

    if (selectedNetWorthValues != "(not defined)") {
        //checkboxes maintain their state when going 'back' so only the total needs to be recalcualted.
        $("#netWorthLabel").html(selectedNetWorthCount + " ranges selected");

    }





    //Ethnicity
    var selectedEthnicityCount = $("#txtTotalEthnicitySelected").val();
    var selectedEthnicytValues = $("#txtEthnicity").val();

    if (selectedEthnicytValues != "(not defined)") {
        //checkboxes maintain their state when going 'back' so only the total needs to be recalcualted.
        $("#ethnicityLabel").html(selectedEthnicityCount + " selected");

        //go and reset buttons
        var africanAmerican = 'Z';
        var arab = 'R';
        var asian = 'O';
        var asianNonOriental = 'B';
        var french = 'F';
        var german = 'G';
        var hispanic = 'H';
        var italian = 'I';
        var jewish = 'J';
        var miscellaneous = 'M';
        var northernEurpoean = 'N';
        var polynesian = 'P';
        var scottishIrish = 'S';
        var southernEuropean = 'D';
        var unclassified = 'X';

        //var string = "foo",
        //substring = "oo";
        //console.log(string.indexOf(substring) > -1);

        //if not found, will return a -1.
        if (selectedEthnicytValues.indexOf(africanAmerican) > -1) {
            $('#btnAfricanAmerican').removeClass();
            $('#btnAfricanAmerican').addClass('btn btn-sm btn-block selected');
        }

        if (selectedEthnicytValues.indexOf(arab) > -1) {
            $('#btnArab').removeClass();
            $('#btnArab').addClass('btn btn-sm btn-block selected');
        }

        if (selectedEthnicytValues.indexOf(asian) > -1) {
            $('#btnAsian').removeClass();
            $('#btnAsian').addClass('btn btn-sm btn-block selected');
        }

        if (selectedEthnicytValues.indexOf(asianNonOriental) > -1) {
            $('#btnAsianNonOriental').removeClass();
            $('#btnAsianNonOriental').addClass('btn btn-sm btn-block selected');
        }

        if (selectedEthnicytValues.indexOf(french) > -1) {
            $('#btnFrench').removeClass();
            $('#btnFrench').addClass('btn btn-sm btn-block selected');
        }

        if (selectedEthnicytValues.indexOf(german) > -1) {
            $('#btnGerman').removeClass();
            $('#btnGerman').addClass('btn btn-sm btn-block selected');
        }

        if (selectedEthnicytValues.indexOf(hispanic) > -1) {
            $('#btnHispanic').removeClass();
            $('#btnHispanic').addClass('btn btn-sm btn-block selected');
        }

        if (selectedEthnicytValues.indexOf(italian) > -1) {
            $('#btnItalian').removeClass();
            $('#btnItalian').addClass('btn btn-sm btn-block selected');
        }

        if (selectedEthnicytValues.indexOf(jewish) > -1) {
            $('#btnJewish').removeClass();
            $('#btnJewish').addClass('btn btn-sm btn-block selected');
        }

        if (selectedEthnicytValues.indexOf(miscellaneous) > -1) {
            $('#btnMiscellaneous').removeClass();
            $('#btnMiscellaneous').addClass('btn btn-sm btn-block selected');
        }

        if (selectedEthnicytValues.indexOf(northernEurpoean) > -1) {
            $('#btnNorthernEuropean').removeClass();
            $('#btnNorthernEuropean').addClass('btn btn-sm btn-block selected');
        }

        if (selectedEthnicytValues.indexOf(polynesian) > -1) {
            $('#btnPolynesian').removeClass();
            $('#btnPolynesian').addClass('btn btn-sm btn-block selected');
        }

        if (selectedEthnicytValues.indexOf(scottishIrish) > -1) {
            $('#btnScottishIrish').removeClass();
            $('#btnScottishIrish').addClass('btn btn-sm btn-block selected');
        }

        if (selectedEthnicytValues.indexOf(southernEuropean) > -1) {
            $('#btnSouthernEuropean').removeClass();
            $('#btnSouthernEuropean').addClass('btn btn-sm btn-block selected');
        }

        if (selectedEthnicytValues.indexOf(unclassified) > -1) {
            $('#btnUnclassified').removeClass();
            $('#btnUnclassified').addClass('btn btn-sm btn-block selected');
        }


    }


}

