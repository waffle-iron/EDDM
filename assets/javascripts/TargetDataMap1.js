//========================================================================================================================================
//VERSION 1.0.0
//
//
//========================================================================================================================================


//global variable
var debug = "false";



// PAGE LOAD
$(function ()
{

    //Hide these initially
    $('#step1Section').hide();
    $('#campaignOverview').hide();
    $('#campaignProgress').hide();
    $('#underMinimumBlock').hide();
    $("#part2Block").hide();


    //execute these
    SetBaseProductID();
    CheckForSavedMap();
    ShowHideOptions();


    //see what do to based on selected Territory (aka Locations)
    var territory = $('#ddlOLBTerritories :selected').val();

    if (territory != "n/a")
    {
        setTimeout(function ()
        {
            //Gives pricing grid time to finish loading (x seconds).
            $("#step1LoadingSection").hide();
            $('#step1Section').fadeIn(500);
            $("#step1Section").show();
            $('#campaignOverview').fadeIn(500);
            $("#campaignOverview").show();
            $('#campaignProgress').fadeIn(500);
            $("#campaignProgress").show();
            $("#part2Block").show();

            Calculate();
        },
        4000);
    }

    else
    {
        $("#step1LoadingSection").hide();
        $('#step1Section').fadeIn(500);
        $("#step1Section").show();
        $('#campaignProgress').fadeIn(500);
        $("#campaignProgress").show();
        $("#part2Block").hide();

    }



    //look for selected Saved Map.
    var savedMap = $('#ddlSavedMaps :selected').val();

    if (savedMap != 0)
    {
        $('#AverageMatchRow').hide(750);
    }

    $("input[type=checkbox]").change(function ()
    { UpdateRouteTotals(); });




});





function CampaignChanged()
{

    //Used only in Step1.
    var campaignChoice = ($('#ddlTargetCampaign').val());


    //Hide num of pieces
    if (campaignChoice == "budget")
    {
        $('#numPiecesGroup').hide(750);
        $('#budgetGroup').show(750);
        $('#savedMapsGroup').hide(750);
    }

        //Hide budget
    else if (campaignChoice == "numpieces")
    {
        $('#numPiecesGroup').show(750);
        $('#budgetGroup').hide(750);
        $('#savedMapsGroup').hide(750);
    }

        //hide budget and num of pieces
    else if (campaignChoice == "OLB")
    {
        $('#numPiecesGroup').hide(750);
        $('#budgetGroup').hide(750);
        $('#savedMapsGroup').hide(750);
    }

    else if (campaignChoice == "savedmap") {
        $('#numPiecesGroup').hide(750);
        $('#budgetGroup').hide(750);
        $('#savedMapsGroup').show(750);
    }

}



function ImpressionsChanged()
{


    var numOfImpressions = ($('#ddlImpressions').val());


    //Hide frequency
    if (numOfImpressions == "1")
    {
        $('#frequencyGroup').hide(750);
        $('#frequencyRow').hide(750);

        //remove stripping and then reapply classes w/o striping
        setTimeout(function ()
        {
            $('#overviewTable').removeAttr('class');
            $('#overviewTable').attr('class', 'table table-bordered table-hover table-condensed detailedData');


            //If the user has selected a Saved Map (which can have a low count)
            if ($('#ddlTargetCampaign').val() == "savedmap")
            {
                $('#AverageMatchRow').hide(750);
                var textOfMapSelected = ($('#ddlSavedMaps option:selected').text());
                var regExp = /\(([^)]+)\)/;
                var matches = regExp.exec(textOfMapSelected);
                var totalSelected = matches[1];
                var totQty = (totalSelected * numOfImpressions);


                //...and if the qty is below 1000 then we need to block it. 
                if (totQty < 1000)
                {
                    $('#underMinimumBlock').show();
                    $('#campaignOverview').hide();
                }

                else
                {
                    $('#underMinimumBlock').hide();
                    $('#campaignOverview').show();
                }


            }




        },750);
    }

    //Show if choice is 2 or 3
    else 
    {
        $('#frequencyGroup').show(750);
        $('#frequencyRow').show(750);

        $('#underMinimumBlock').hide();
        $('#campaignOverview').show();

        //remove stripping and then reapply
        setTimeout(function ()
        {
            $('#overviewTable').removeAttr('class');
            $('#overviewTable').attr('class', 'table table-striped table-bordered table-hover table-condensed detailedData');
        }, 750);
    }


    $('#lblImpressions').html(numOfImpressions);

}



function TemplateChanged()
{

    //console.log("TemplateChanged");

    var baseProductID = $("#ddlOLBTemplates").find('option:selected').attr("baseprodid");
    //console.log("baseProductID:" + baseProductID);

    if (typeof baseProductID == 'undefined')
    {
        //console.log("baseProductID is *undefined*");

        var ProductID = $("#ddlOLBTemplates").find('option:selected').val();
        //console.log("ProductID: " + ProductID);

        if (ProductID == 66)
        { baseProductID = 225; }

        if (ProductID == 734)
        { baseProductID = 229; }

        if(ProductID == 953)
        { baseProductID = 248; }

        if(ProductID == 65)
        {baseProductID = 216;}
    }
    

    var templateDesc = "undefined";

    if (baseProductID == "229")
    { templateDesc = "4.25&quot; x 11&quot; EDDM™ Postcard"; }

    if (baseProductID == "225")
    { templateDesc = "6.25&quot; x 9&quot; EDDM™ Postcard"; }

    if (baseProductID == "216")
    { templateDesc = "8.5&quot; x 11&quot; EDDM™ Postcard"; }

    if (baseProductID == "248")
    { templateDesc = "11&quot; x 15&quot; EDDM™ Flyer"; }


    $('#lblTemplate').html(templateDesc);
    if (templateDesc == "undefined")
    { LoadPricePerPieceGrid("0"); }

    LoadPricePerPieceGrid(baseProductID);

    //the labels are updated in GetPriceQuote, called by LoadPricePerPiece();
    //so Calculate should work - 
    setTimeout(fooCalculate, 1000);

    
}



function fooCalculate()
{
    Calculate();
}



function FrequencyChanged()
{
    //alert("freq changed");

    var selectedFrequnecy = ($('#ddlFrequency').val());

    if (selectedFrequnecy == "1")
    { selectedFrequnecy = "Every Week"; }

    else
    { selectedFrequnecy = "Every " + selectedFrequnecy + " Weeks";}

    $('#lblFrequency').html(selectedFrequnecy);

}



function LaunchWeekChanged()
{
    //alert("launch changed");

    var selectedWeek = ($('#ddlLaunchWeek').val());
    $('#lblLaunchWeek').html("Week of " + selectedWeek);

}



function PPPTester()
{


    var oPostData = {};
    oPostData.pid = 216;
    oPostData.qty = 31083;


    oPostData.hqty = 0;
    oPostData.sqty = 0;
    oPostData.distid = 0;
    oPostData.drops = 3;
    oPostData.zip = "28352";
    oPostData.wd = false;
    oPostData.wt = true;
    oPostData.m = 0;
    oPostData.mt = 'Percent';


    $.ajax(
        {
            type: 'POST',
            dataType: 'json',
            url: '/Resources/PrintRateQuote.ashx',
            data: oPostData,
            success: function (msg)
            {
                if (msg != null && msg != '')
                {

                    //console.log("********");
                    //console.log("ppp: " + msg.PricePerPiece);
                    //console.log("********");

                }

            },

            error: function (msg) {
                DebugHelper("Error: ", msg.error);
            }

        });
}





    //Debug
    function DebugHelper(debugMsg, debugObj)
    {
        $('#pnlDebug').addClass('visible').removeClass('hidden');
        $('#debugMsg').text(debugMsg + debugObj);
    }



    function CheckForSavedMap()
    {

        //Called on Page Load.  Will run one of two calculation functions.

        var campaignTarget = ($('#ddlTargetCampaign').val());
        var baseProductID = ($('#txtBaseProductID').text());

        if (debug == "true")
        {

            if (campaignTarget != "savedmap")
            {
                console.log("[CheckForSavedMap] New Map");
            }

            else
            {
                console.log("[CheckForSavedMap] Saved Map");
            }



        }



        if (campaignTarget != "savedmap")
        {
            LoadPricePerPieceGrid(baseProductID);
            Calculate();
        }

        else
        {
            LoadPricePerPieceGrid(baseProductID);
            SavedMapCalculate();
        }

    }



    function SetBaseProductID()
    {

        var baseProductID = $("#ddlOLBTemplates").find('option:selected').attr("baseprodid");
        $('#txtBaseProductID').text(baseProductID);
        $('#hidBaseProductID').val(baseProductID);

    }



    function ShowHideOptions()
    {

        //If the user has come back to this page from Step2, hide and show the relevant drop downs.
        var campaignChoice = ($('#ddlTargetCampaign').val());

        if (campaignChoice == "budget")
        {
            $('#numPiecesGroup').hide(500);
            $('#budgetGroup').show(500);
            $('#savedMapsGroup').hide(500);
        }

            //Hide budget
        else if (campaignChoice == "numpieces")
        {
            $('#numPiecesGroup').show(500);
            $('#budgetGroup').hide(500);
            $('#savedMapsGroup').hide(500);
        }

            //hide budget and num of pieces
        else if (campaignChoice == "OLB") {
            $('#numPiecesGroup').hide(500);
            $('#budgetGroup').hide(500);
            $('#savedMapsGroup').hide(500);
        }

            //hide budget and num of pieces
        else if (campaignChoice == "savedmap")
        {
            $('#numPiecesGroup').hide(500);
            $('#budgetGroup').hide(500);
            $('#savedMapsGroup').show(500);
        }

    }







