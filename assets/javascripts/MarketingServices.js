//VERSION 1.0.0

//  File was created and included on MarketServices.aspx to include Addressed AddOn functionality.



//Page Load
$(function ()
{

    var addressedAddOnProspects = parseInt($('#hidAddOnAddressedProspects').val());
    var addressedAddOnMinQty = 1000;                                                            //Min required amt to offer
    var debug = "false";
    var suggestedAddressedAddOnStartQty = parseInt($('#hidSuggestedAddressedAddOnStartQty').val());



    //set initial values, classes
    $("#minProspects").html(Formatter.Commas(addressedAddOnMinQty));
    $("#maxProspects").html(Formatter.Commas(addressedAddOnProspects));
    $("#lblSendToAddOns").html(Formatter.Commas(suggestedAddressedAddOnStartQty));
    $("#lblSendNumTimes").html("3 times");                                                      //default to 3 times.
    $("#hidNumTimesSelected").val('3')                                                          //default to 3 times.
    $("#txtNumTimesSelected").val('3')                                                          //default to 3 times.
    $('#addressedMailPriceError').hide();


    



    //Set initial cost for AddOnAddressedProspects
    UpdateAddressedAddOnCost();


    $("#sliderControl").slider(
        {
            range: "max",
            min: 1000,
            step: 1, 
            max: addressedAddOnProspects,
            value: GetStartPosition(addressedAddOnProspects, addressedAddOnMinQty),             //calculate the default position of the slider
            stop: function (event, ui)
            {
                UpdateAddressedAddOnCost();
            },

            slide: function (event, ui)
            {
                //console.log("slide");
                $("#lblSendToAddOns").html(Formatter.Commas(ui.value));  //<-- look here
                $("#txtSelectedAddOnAddressedList").val(ui.value);
                $("#hidSelectedAddOnAddressedList").val(ui.value);

            },



            // **** I didn't seem to need this.....DSF. 2/15/2016
            //////change: function (event, ui)
            //////{
            //////    //console.log("change");
            //////    $("#lblSendToAddOns").html(Formatter.Commas(ui.value));
            //////    $("#txtSelectedAddOnAddressedList").val(ui.value);
            //////    $("#hidSelectedAddOnAddressedList").val(ui.value);

            //////},
            //////create: function (event, ui)
            //////{
            //////    //console.log("create");
            //////    setTimeout(SetStartPosition(addOnAddressedProspects, addressedAddOnMinQty), 1000);
            //////},
            // **** END
            

        });




    if(debug == "true")
    {

        console.log("addressedAddOnProspects: " + addressedAddOnProspects);
        console.log("addressedAddOnMinQty: " + addressedAddOnMinQty);
        console.log("GetStartPosition: " + GetStartPosition(addressedAddOnProspects, addressedAddOnMinQty));
        console.log("suggestedAddressedAddOnStartQty: " + suggestedAddressedAddOnStartQty);
        
    }


});


function GetStartPosition(addressedAddOnProspects, addressedAddOnMinQty)
{
    //console.log('test');
    //$('#sliderControl').slide(value, "1000"); //using this we can set the slider value
    //no work --> $("#slider").slider('value', 1000);
    //var ui = $("#slider"); //.slider('value', '1000');


    //Function should set the start position of the slider.
    //Slider position should be the 'middle' value between 1000 (min acceptable value) and the max value (addOnAddressedProspects).

    var startPosition = parseInt((addressedAddOnProspects - addressedAddOnMinQty) / 2) + addressedAddOnMinQty;
    
    //NEW override functionality for cart - if there is a value in the cart - use that value as the starting point 3/2/2016
    if ($("#hidOverrideAddOnAddressedTimes").length > 0)
    {
        var addressedAddOnsNumTimes = parseInt($("#hidOverrideAddOnAddressedTimes").val()); //should be at least 0 if it exists
        
        if (addressedAddOnsNumTimes > 0) {
            if ($("#hidOverrideAddOnAddressedProspects").length > 0) {
                var numberOfAddressedAddOnsInCart = parseInt($("#hidOverrideAddOnAddressedProspects").val());
                var startPositionOverride = numberOfAddressedAddOnsInCart / addressedAddOnsNumTimes;
                console.log("numberOfAddressedAddOnsInCart " + numberOfAddressedAddOnsInCart);
                console.log("addressedAddOnsNumTimes " + addressedAddOnsNumTimes);
                console.log("Override startPostion to " + startPositionOverride);
                startPosition = startPositionOverride;
                $("#lblSendToAddOns").html(Formatter.Commas(startPosition));
                //now the button
                if (addressedAddOnsNumTimes == 1) {
                    NumTimesAddressedAddOnSelected('btn1Time');
                }

                if (addressedAddOnsNumTimes == 2) {
                    NumTimesAddressedAddOnSelected('btn2Times');
                }

                if (addressedAddOnsNumTimes == 3) {
                    NumTimesAddressedAddOnSelected('btn3Times');
                }
            }
        }
    }



    return startPosition;

    //$("#lblSendToAddOns").html(Formatter.Commas(startPosition));
    //$("#txtSelectedAddOnAddressedList").val(startPosition);
    //$("#hidSelectedAddOnAddressedList").val(startPosition);
    ////NumTimesSelected('btn3Times');
    //UpdateAddressedCost();

}
 




//Functions
function NumTimesAddressedAddOnSelected(btnName)
{

    //One Time IS selected, remove styling from others
    if (btnName == 'btn1Time')
    {

        //style it
        $('#btn1Time').removeClass();
        $('#btn1Time').addClass('btn btn-sm btn-block selected btn-shadow');

        //strip off styling of Two Times, Three Times
        $('#btn2Times').removeClass();
        $('#btn2Times').addClass('btn btn-sm btn-block');
        $('#btn3Times').removeClass();
        $('#btn3Times').addClass('btn btn-sm btn-block');

        //Set hidden field
        $("#hidNumTimesSelected").val('1')
        $("#txtNumTimesSelected").val('1')

        $("#lblSendNumTimes").html("1 time");

    }


    //Two Times IS selected, remove styling from others
    if (btnName == 'btn2Times')
    {

        //style it
        $('#btn2Times').removeClass();
        $('#btn2Times').addClass('btn btn-sm btn-block selected btn-shadow');

        //strip off styling of Two Times, Three Times
        $('#btn1Time').removeClass();
        $('#btn1Time').addClass('btn btn-sm btn-block');
        $('#btn3Times').removeClass();
        $('#btn3Times').addClass('btn btn-sm btn-block');

        //Set hidden field
        $("#hidNumTimesSelected").val('2')
        $("#txtNumTimesSelected").val('2')

        $("#lblSendNumTimes").html("2 times");

    }

    //Three Times IS selected, remove styling from others
    if (btnName == 'btn3Times')
    {

        //style it
        $('#btn3Times').removeClass();
        $('#btn3Times').addClass('btn btn-sm btn-block selected btn-shadow');

        //strip off styling of Two Times, Three Times
        $('#btn1Time').removeClass();
        $('#btn1Time').addClass('btn btn-sm btn-block');
        $('#btn2Times').removeClass();
        $('#btn2Times').addClass('btn btn-sm btn-block');

        //Set hidden field
        $("#hidNumTimesSelected").val('3')
        $("#txtNumTimesSelected").val('3')

        $("#lblSendNumTimes").html("3 times");

    }

    UpdateAddressedAddOnCost();

}



function UpdateAddressedAddOnCost()
{

    //Function will calculate cost of AddressedAddOns.  Will block off <div>addressedMailPriceBlock</div> while calculations are made.
    //Will also block off <div>addressedPricePerPieceBlock</div>

    $('#addressedMailPriceBlock').block
    ({
        message: '<h5><br /><br /><span class="fa fa-cog fa-spin"></span>&nbsp;Updating....</h5>',
        css: { width: '100%', height: '100%', border: '1px solid #dddddd' }
    });

    $('#addressedPricePerPieceBlock').block
    ({
        message: '<span class="fa fa-cog fa-spin"></span>&nbsp;Updating....',
        css: { width: '100%', height: '100%', border: '1px solid #dddddd' }
    });


    //mode
    var debug = "false";

    //Distribution type
    var UselectID = 6;  //<-- system created 'With List' product.


    //Distribution ID
    //We do not want to send the handler a distribution ID b/c it will automatically get the count/total of the distribution/map
    //as use that value as the qty.  Booooo....
    var distributionID = 0;


    //Product ID
    var baseProductID = $('#hidAddressedAddOnBaseProductID').val();


    //Product ID
    //var productID = $('#hidProductID').val();


    //Mark up
    var addressedMailMarkUp = $('#hidAddressedMailMarkUp').val()


    //Mark up Type
    var addressedMailMarkUpType = $('#hidAddressedMailMarkUpType').val()


    //Total Selected to Add On.....
    var totalSelected = parseInt($('#hidSelectedAddOnAddressedList').val());


    //How many times did user want to send?
    var numTimesSelected = parseInt($('#hidNumTimesSelected').val());


    //Total to send out ...for quoted PPP
    var totalToSend = parseInt(totalSelected * numTimesSelected);


    //# of Drops
    var numOfDrops = 1;



    //Hold Qty.  Currently not in use.
    var holdQTY = 0;


    //Extra copies.
    var extraCopies = 0;



    //Zip Code.  Should ALWAYS be populated.
    var zipCode = $('#hidZipCode').val();


    //Design Option - we are hard coding this value so we do not charge the customer 2x for a Design Fee in the event they chose Pro Design on the 
    //EDDM product.  We already collected that fund so no need to get a quote back with a second design fee added to it.
    var designOption = "Template";


    //Pro Design ?
    var proDesign = false;
    if (designOption == "Professional Design")
    { proDesign = true; }


    //Free Template?
    var freeTemplate = false;
    if (designOption == "Template")
    { freeTemplate = true; }




    //***** JSon Obj *******
    var oPostData = {};

    oPostData.pid = baseProductID;

    oPostData.qty = totalToSend;

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
    oPostData.wd = proDesign;
    oPostData.wt = freeTemplate;

    //Mark up
    oPostData.m = addressedMailMarkUp;
    oPostData.mt = addressedMailMarkUpType;


       $.ajax({
            type: 'POST',
            dataType: 'json',
            url: '/resources/PrintRateQuote.ashx',
            data: oPostData,

            success: function (msg)
            {

                //Will need PricePerPiece
                if (msg != null && msg != '')
                {

                    //Product Options logic stripped out.  May need to revisit.  See CampaignOverview.js
                    //Gap Message logic stripped out.  May need to revisit.  See CampaignOverview.js
                    //Minimum Pricing Quote Amt logic stripped out.  May need to revisit.  See CampaignOverview.js


                    //Incase the price returned was zero or less....
                    if (parseFloat(msg.PricePerPiece) <= 0)
                    {
                        $('#addressedMailPriceBlock').hide();
                        $('#addressedMailPriceError').show();

                        //the css class is initially set to hidden so it doesn't show briefly on page_load.  Now, let's apply the correct styling.
                        $('#addressedMailPriceError').removeClass();
                        $('#addressedMailPriceError').addClass('alert alert-danger');

                    }

                    else
                    {
                        $('#txtAddressedAddOnPricePerPiece').val(msg.PricePerPiece);
                        $('#hidAddressedAddOnPricePerPiece').val(msg.PricePerPiece);

                        var selectedAddOnAddressedProspects = parseInt($("#hidSelectedAddOnAddressedList").val());
                        var numTimesSelected = parseInt($("#hidNumTimesSelected").val())

                        $("#lblAddressedAddOnPrice").html(Formatter.Currency((selectedAddOnAddressedProspects * parseFloat(msg.PricePerPiece)) * numTimesSelected));
                        $('#addressedPricePerPiece').html(msg.FormattedPricePerPiece);

                        setTimeout(function ()
                        { $('#addressedMailPriceBlock').unblock(); }, 1000);

                        setTimeout(function ()
                        { $('#addressedPricePerPieceBlock').unblock(); }, 1000);


                    }



                }


                if (debug == "true")
                {


                    //Debugging
                    console.log("*******UpdatePriceQuote for Addressed Mail AddOns***********");
                    console.log("UselectID: " + UselectID);
                    console.log("BaseProductID: " + baseProductID);
                    console.log("totalSelected: " + totalSelected);
                    console.log("numTimesSelected: " + numTimesSelected);
                    console.log("totalToSend: " + totalToSend);
                    console.log("holdQTY: " + holdQTY);
                    console.log("extraCopies: " + extraCopies);
                    console.log("distributionID: " + distributionID);
                    console.log("numOfDrops: " + numOfDrops);
                    console.log("zipCode: " + zipCode);
                    console.log("proDesign: " + proDesign);
                    console.log("freeTemplate: " + freeTemplate);
                    console.log("addressedMailMarkUp: " + addressedMailMarkUp);
                    console.log("addressedMailMarkUpType: " + addressedMailMarkUpType);
                    console.log("msg.Price: " + msg.Price);
                    console.log("msg.FormattedTotalPrice: " + msg.FormattedTotalPrice);
                    console.log("msg.FormattedPricePerPiece: " + msg.FormattedPricePerPiece);
                    console.log("msg.PricePerPiece: " + msg.PricePerPiece);
                    console.log("*******END UpdatePriceQuote for Addressed Mail AddOns*******");
                    //end

                }

            },

            error: function ()
            {


                setTimeout(function ()
                {
                    $('#addressedMailPriceBlock').unblock();
                    $('#addressedMailPriceBlock').hide();
                    $('#addressedMailPriceError').show();
                    
                }, 1000);

                //show error block

                console.log('**Ruh Roh Scooby - Error in GetAddressedMailPricePerPiece.');
            }


    });


}









//Events
//More Information Windows
$('a[data-action=infowindow]').click(function (e)
{
    e.preventDefault();

    var productID = $('#hidProductID').val();
    var baseProductID = $('#hidBaseProductID').val();
    var pageref = $(this).attr('data-helpfile');
    var title = $(this).attr('data-title');


    //Product options only
    var optcatid = 0;

    //All that really seems to matter here is that the pageref is passed. ProductID and BaseProductID don't seem to matter here.
    $('#infoTitle').html(title);
    $('#infoContent').load('/Resources/ProductConfigHelp.ashx?pid=' + productID + '&bpid=' + baseProductID + '&catid=' + optcatid + '&pageref=' + pageref);


});



