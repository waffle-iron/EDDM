//========================================================================================================================================
//VERSION 1.0.0
//
//  Contains shared javascript functions used on TargetDataMap1 and TargetDataMap2.
//========================================================================================================================================


function LoadPricePerPieceGrid(baseProductID)
{
    var priceAt1000 = 0.59;
    var priceAt2500 = 0.45;
    var priceAt5000 = 0.36;
    var priceAt10000 = 0.32;
    var priceAt25000 = 0.31;
    var priceAt50000 = 0.30;

    $('#lblPriceAt1000').html(priceAt1000.toFixed(2));
    $('#lblPriceAt2500').html(priceAt2500.toFixed(2));
    $('#lblPriceAt5000').html(priceAt5000.toFixed(2));
    $('#lblPriceAt10000').html(priceAt10000.toFixed(2));
    $('#lblPriceAt25000').html(priceAt25000.toFixed(2));
    $('#lblPriceAt50000').html(priceAt50000.toFixed(2));

    if (baseProductID != 0)
    {
        GetPriceQuote(1000, baseProductID);
        GetPriceQuote(2500, baseProductID);
        GetPriceQuote(5000, baseProductID);
        GetPriceQuote(10000, baseProductID);
        GetPriceQuote(25000, baseProductID);
        GetPriceQuote(50000, baseProductID);
    }



}



function GetPriceQuote(quantity, productID)
{

    var debug = "false";

    var oPostData = {};
    oPostData.pid = productID;
    oPostData.qty = quantity;


    var impressions = ($('#ddlImpressions').val());

    //if drop down was not present (like on TargetDataMap2), then look in the querystring.
    if ($('#ddlImpressions').length)
    { impressions = ($('#ddlImpressions').val()); }
    else
    { impressions = parseInt(RetrieveFromQueryString.GetValue('i')) || 0; }

    oPostData.hqty = 0;
    oPostData.sqty = 0;
    oPostData.distid = 0;
    oPostData.drops = impressions;
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
            success: function (msg) {
                if (msg != null && msg != '') {


                    if (oPostData.qty >= 1000 && oPostData.qty < 2500)
                    { $('#lblPriceAt1000').html(msg.PricePerPiece.toFixed(2)); }

                    if (oPostData.qty >= 2500 && oPostData.qty < 5000)
                    { $('#lblPriceAt2500').html(msg.PricePerPiece.toFixed(2)); }

                    if (oPostData.qty >= 5000 && oPostData.qty < 10000)
                    { $('#lblPriceAt5000').html(msg.PricePerPiece.toFixed(2)); }

                    if (oPostData.qty >= 10000 && oPostData.qty < 25000)
                    { $('#lblPriceAt10000').html(msg.PricePerPiece.toFixed(2)); }

                    if (oPostData.qty >= 25000 && oPostData.qty < 50000)
                    { $('#lblPriceAt25000').html(msg.PricePerPiece.toFixed(2)); }

                    if (oPostData.qty >= 50000)
                    { $('#lblPriceAt50000').html(msg.PricePerPiece.toFixed(2)); }


                    if (debug == "true") {
                        console.log("--------");
                        console.log("msg.PricePerPiece: " + msg.PricePerPiece);
                        console.log("Quantity: " + oPostData.qty);
                        console.log("productID: " + productID);
                        console.log("oPostData.drops: " + oPostData.drops);
                        console.log("-------");
                    }

                }

            },

            error: function (msg) {
                DebugHelper("Error: ", msg.error);
            }

        });
}



function Calculate()
{

    if (debug == "true")
    { console.log("Calculate called"); }


    //Look for a 'saved map' indicator

    var campaignChoice = ($('#ddlTargetCampaign').val());
    var campaignChoiceUrl = RetrieveFromQueryString.GetValue("c");
    var isSavedMap = "false";

    if (campaignChoice == "savedmap")
    { isSavedMap = "true"; }

    if (campaignChoiceUrl == "savedmap")
    { isSavedMap = "true"; }


    if (debug == "true")
    { console.log("[SavedMapCalculate] isSavedMap: " + isSavedMap); }


    //Previous map - Calculateslightly different.
    if ((isSavedMap == "true")) {
        SavedMapCalculate();

        //break out
        return;
    }





    //Used to Initially calculate the default, recommended routes. Fires on PageLoad.
    // *reset of checkboxes, labels.
    // *reads rows for checked items.
    // *counts and totals.
    // *checks boxes as needed.
    // *sets label totals in Campaign Overview.

    ClearAllTheChecks();
    $('#lblSelected').text(0);
    $('#lblAmount').text(0);
    $('#lblTotalMailed').text(0);
    $('#lblPricePerPiece').text(0);

    //temp//
    $('#hidSelected').val(0);
    $('#hidPctMatchAvg').val(0);
    $('#hidAmount').val(0);
    $('#hidTotalMailed').val(0);
    $('#hidPricePerPiece').val(0);
    //temp//

    //first one should always be checked
    var okToCheck = true;

    $('#tblRoutes tr').each(

        function (index, row) {
            var allCells = $(row).find('td');
            var testThis = $(row).find('input[type="checkbox"]');

            okToCheck = LimitReached();

            if (RetrieveFromQueryString.GetValue("r") == "true")
            { okToCheck = true; }


            if (okToCheck) {
                testThis.prop('checked', true);
                UpdateRouteTotals();
            }

            else { return false; }

        }
    );



    //debug
    if (debug == "true") {
        //console.log("[Calculate] distID: " + distID);
        //console.log("[Calculate] campaignChoice: " + campaignChoice);
    }

}



function SavedMapCalculate()
{

    if (debug == "true")
    { console.log("SavedMapCalculate called."); }


    try
    {

        //We need to determine which page we are on.  TargetDataMap1 or TargetDataMap2/3
        var currentPage = "";
        var textOfMapSelected = "";
        var regExp = /\(([^)]+)\)/;
        var impressions = 0;




        //Must be on TargetDataMap2/3. Get location from querystring and fake it...
        if (parseInt(window.location.href.indexOf("TargetDataMap1")) == -1)
        {
            textOfMapSelected = RetrieveFromQueryString.GetValue("l") + " (" + $("#lblSelected").html() + ")";
            impressions = RetrieveFromQueryString.GetValue("i");
        }

            //must be on TargetDataMap1. Get location from control.
        else {
            textOfMapSelected = ($('#ddlSavedMaps option:selected').text());
            impressions = ($('#ddlImpressions').val());
        }




        if (debug == "true")
        {

            if (parseInt(window.location.href.indexOf("TargetDataMap1")) == -1)
            { console.log("We are on TargetDataMap2 or 3"); }

            else
            { console.log("We are on TargetDataMap1"); }


            console.log("textOfMapSelected: " + textOfMapSelected);
            console.log("impressions:" + impressions);

        }



        //calculations
        var matches = regExp.exec(textOfMapSelected);
        var count = matches[1];
        var totQty = (count * impressions);



        if (debug == "true")
        {
            console.log("matches: " + matches);
            console.log("count:" + count);
            console.log("totQty: " + totQty);
        }



        //If the qty is below 1000 then we need to block it. 
        if (totQty < 1000)
        {
            $('#underMinimumBlock').show();
            $('#campaignOverview').hide();
        }

        //Reset Labels
        $('#lblFranchise').text($('#ddlOLBBrands').val());
        $('#lblLocation').text($('#ddlOLBTerritories').val());
        $('#lblSelected').text(Formatter.Commas(count));
        $('#lblImpressions').text($('#ddlImpressions').val());
        $('#lblTotalMailed').text(Formatter.Commas(totQty));
        $('#hidTotalMailed').val(Formatter.Commas(totQty));

        UpdatePriceLabel(totQty);

        //hide this - not relevant to Saved Maps
        $("#phForm_rowAvgMatch").hide();

    }

    catch (e)
    {
        console.log(e.message);
    }


}



function SavedMapCalculateDelay()
{
    setTimeout(SavedMapCalculate, 2000);
}



function ClearAllTheChecks()
{


    $('#tblRoutes tr').each(function (index, row)
    {
        var allCells = $(row).find('td');
        var testThis = $(row).find('input[type="checkbox"]');
        testThis.prop('checked', false);
    });
}



function LimitReached()
{

    var campaignChoice = "";
    var maxResidents = 0;
    var maxBudget = 0;
    var totalSelected = parseFloat(Formatter.RemoveCommas($('#lblSelected').text()));
    var amountSpent = parseFloat(Formatter.RemoveCommas($('#lblAmount').text()));


    //If on Step1, values will come from DropDownLists. If on Step2, values will come from querystring.
    if ($('#ddlTargetCampaign').length)
    { campaignChoice = ($('#ddlTargetCampaign').val()); }
    else
    { campaignChoice = RetrieveFromQueryString.GetValue('c'); }

    if (campaignChoice == "OLB")
    { maxResidents = 10000; }

    else if (campaignChoice == "numpieces") {
        //val comes from step1
        if ($('#ddlNumPieces').length)
        { maxResidents = parseFloat($('#ddlNumPieces').val()); }

            //val comes from step2
        else
        { maxResidents = RetrieveFromQueryString.GetValue('q'); }
    }

    //DebugHelper("max residents: ", maxResidents);

    if ($('#ddlBudget').length)
    { maxBudget = ($('#ddlBudget').val()); }
    else
    { maxBudget = Formatter.RemoveCommas(RetrieveFromQueryString.GetValue('b')); }

    //DebugHelper("max budget: ", maxBudget);
    //console.log("totalSelected:" + totalSelected + " amountSpent:" + amountSpent + " campaignChoice:" + campaignChoice + " maxBudget:" + maxBudget + " maxResidents:" + maxResidents);
    //alert("Total Selected: " + totalSelected);
    //alert("Total Selected: " + totalSelected);

    if (campaignChoice == "numpieces" || campaignChoice == "OLB") {
        if (totalSelected > maxResidents) {
            //console.log("          resident limit hit");
            return false;
        }
    }



    if (campaignChoice == "budget") {
        var i = 1;

        if (amountSpent > maxBudget) {
            //console.log("          budget limit hit");
            return false;
        }
    }

    return true;
}



function UpdateRouteTotals()
{

    //verify that we are not in a saved maps situation
    var campaignTarget = $('#ddlTargetCampaign').val();
    if (campaignTarget == "savedmap") {
        SavedMapCalculate();
        return;
    }




    if (debug == "true") {
        console.log("UpdateRouteTotals called");
    }








    //Recalculates totals based on selected checkboxes in Repeater control.
    var i = 0;
    var runningMatch = 0;
    var numChecks = 0;
    var impressions = 0;

    if ($('#ddlImpressions').length)
    { impressions = ($('#ddlImpressions').val()); }
    else
    { impressions = parseInt(RetrieveFromQueryString.GetValue('i')) || 0; }


    $('#tblRoutes tbody tr').each(function (index, row) {
        //Num of Residents in row
        var lblRes = $(this).find('#lblResTotal');
        var holdThis = lblRes.html();
        var numRes = parseInt(holdThis) || 0;

        //Perc match in row
        var lblPctMatch = $(this).find('#lblPctMatch');
        var pct = lblPctMatch.html();
        var pct2 = parseFloat(pct) || 0;

        //get checked value from row.
        var allCells = $(row).find('td');
        var testThis = $(row).find('input[type="checkbox"]');

        if (testThis.is(':checked')) {
            i = parseInt(i) + parseInt(numRes);
            runningMatch = runningMatch + pct2;
            numChecks = numChecks + 1;
        }


    });

    $('#lblSelected').text(Formatter.Commas(i));
    $('#hidSelected').val(Formatter.Commas(i));

    $('#lblTotalMailed').text(Formatter.Commas(i * impressions));
    $('#hidTotalMailed').val(Formatter.Commas(i * impressions));

    runningMatch = runningMatch / numChecks;

    if (campaignTarget != 'savedmap') {
        $('#lblPctMatchAvg').text(runningMatch.toFixed(2) + '%');
        $('#hidPctMatchAvg').val(runningMatch.toFixed(2) + '%');
    }

    //console.log("UpdateRouteTotals:" + (i * impressions));
    UpdatePriceLabel(i * impressions);

    return;
}



function UpdatePriceLabel(qty2)
{

    //This is called multiple times.  Once for every checkbox that is checked.
    //Called from UpdateRouteTotals.
    //console.log("UpdatePriceLabel qty2:" + qty2);
    var qty = parseInt(qty2);
    var pricePerPiece = 0;

    if (qty >= 1000 && qty < 2500)
    { pricePerPiece = $('#lblPriceAt1000').html(); }

    if (qty >= 2500 && qty < 5000)
    { pricePerPiece = $('#lblPriceAt2500').html(); }

    if (qty >= 5000 && qty < 10000)
    { pricePerPiece = $('#lblPriceAt5000').html(); }

    if (qty >= 10000 && qty < 25000)
    { pricePerPiece = $('#lblPriceAt10000').html(); }

    if (qty >= 25000 && qty < 50000)
    { pricePerPiece = $('#lblPriceAt25000').html(); }

    if (qty >= 50000)
    { pricePerPiece = $('#lblPriceAt50000').html(); }

    //console.log("UpdatePriceLabel():  pricePerPiece:" + pricePerPiece);
    var cost = pricePerPiece * qty;
    var pricePerPiece1 = pricePerPiece * 1;
    //console.log("UpdatePriceLabel():  Estimated Cost:" + cost);
    //console.log("UpdatePriceLabel:" + qty);



    $('#lblPricePerPiece').html(pricePerPiece1.toFixed(2));
    $('#hidPricePerPiece').val(pricePerPiece1.toFixed(2));

    $('#lblAmount').text(Formatter.Commas(cost.toFixed(2)));
    $('#hidAmount').val(Formatter.Commas(cost.toFixed(2)));

    //DebugHelper("Est Cost: ", NumberWithCommas(cost.toFixed(2)));

}
