//VERSION 1.0.1


//NOTES:
//  1.0.1           1/11/2016           Allowed control and script to retrieve and use "MinPrintQty" amounts per product/USelect type.
//                                      Also, script is now fetching PostageRate from hidden field on Page.
//  1.0.2           1/25/2016           Removed dependency from obsolete library 'td_tools'.



$(function ()
{
    //Page Load
    //update initial Overview labels
    //lblExtraCopies is updated by the Step2-ProductOptions.js file.
    $('#lblProductName').html($('#hidProductName').val());
    $('#lblPiecesMailed').html($('#lblTotalDeliveries').html());


    //Could be a postback from Template Browser so look in txtExtraCopies for real value
    var extraCopies = $('#txtExtraCopies').val();
    if (extraCopies > 0)
    { $('#lblExtraCopies').html(extraCopies); }

    else
    { $('#lblExtraCopies').html('0'); }

    UpdatePriceQuote();

});



function NumberWithoutCommas(dirtyNum)
{

    var number = Number(dirtyNum.toString().replace(/[^0-9\.]+/g, ""));
    var numberInt = parseInt(number)

    //For debugging
    //console.log("numberInt (NumberWithoutCommas): " + numberInt);
    //End

    return numberInt;

}



function NumberWithCommas(rawNum)
{ return rawNum.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","); }



function UpdatePriceQuote()
{


    $('#campaignOverview').block
    ({
        message: '<div><br/><br/><br/><br/><img src="/assets/images/indicator.gif" width="16" height="16"/><br/><br/><br/><br/><p class="lead">Updating Estimate...</p><br/><br/><br/><br/></div>',
        css: { width: '100%', height: '100%', border: '1px solid #dddddd' }

    });


    //mode
    var debug = "false";


    //Delay for USelectID 7
    var delay = 2000;


    //Distribution type
    var UselectID = $('#hidUSelectID').val();


    //Distribution ID
    var distributionID = $('#hidDistributionID').val();


    //Product ID
    var baseProductID = $('#hidBaseProductID').val();


    //Product ID
    var productID = $('#hidProductID').val();


    //Mark up
    var markUp = $('#hidMarkUp').val()


    //Mark up Type
    var markUpType = $('#hidMarkUpType').val()



    //Total Deliveries.  # of selections per drop/impression. Get from Step2-ProductOptions.aspx!
    var totalDeliveries = $('#lblTotalDeliveries').html();
    totalDeliveries = Number(totalDeliveries.toString().replace(",", ""));
    totalDeliveries = parseInt(totalDeliveries);


    //# of Drops
    var numOfDrops = $('#ddlNumOfDrops').val();


    //Send all at once? ddlDrops = Y/N question.
    var sendAllAtOnce = $('#ddlDrops').val();


    //# of Impressions
    var numImpressions = parseInt($('#ddlImpressions').val());


    //Hold Qty.  Currently not in use.
    var holdQTY = $('#hidHoldQuantity').val();


    //Extra copies.
    var extraCopies = parseInt($('#txtExtraCopies').val());


    //Total Mailed (drops x total deliveries)
    var totalMailed = 0;
    if (numImpressions > 0)
    { totalMailed = (totalDeliveries * numImpressions); }





    //Calculate # of Drops and Impressions.  Handler cannot distinquish the difference between the two.
    //SINGLE IMPRSSSION
    if (numImpressions == 1) {

        //Overriding # of Drops if user decides to send all at once.
        //By design, the ddlNumOfDrops control does not offer a "1" selection.
        if (sendAllAtOnce == "Yes")
        { numOfDrops = 1 }

    }

        //MULTIPLE IMPRESSIONS            
    else {
        //This should be a string of true/false
        var noMultipleImpressionsFee = $('#hidNoMultipleFee').val().toLowerCase();


        //We will not charge for Multiple Imp
        if (noMultipleImpressionsFee == "true") {
            //console.log("numImpressions was overridden.  Are not charging for multiple drops/impressions.");
            numImpressions = 1;
            numOfDrops = 1;
        }

        else {
            //set the # of drops to the same # as numImpressions since Handler will calculate based on numOfDrops
            numOfDrops = numImpressions;
        }

    }




    //Zip Code.  Should ALWAYS be populated.
    var zipCode = $('#ZipCode').val();


    //Design Option
    var designOption = $('#ddlDesignOption').val();


    //Pro Design ?
    var proDesign = false;
    if (designOption == "Pro")
    { proDesign = true; }


    //Free Template?
    var freeTemplate = false;
    if (designOption == "Template")
    { freeTemplate = true; }





    if ((UselectID == 1) || (UselectID == 5) || (UselectID == 6))
    {


        //***** JSon Obj *******
        var oPostData = {};
        oPostData.pid = baseProductID;
        oPostData.qty = totalMailed;

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
        oPostData.m = markUp;
        oPostData.mt = markUpType;


        //get the product attributes from the controls.
        $('.prodopt').each(function ()
        { oPostData['opt' + $(this).attr('optcatid')] = $(this).val(); });




        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: '/resources/PrintRateQuote.ashx',
            data: oPostData,

            success: function (msg) {

                if (msg != null && msg != '') {

                    //Possibly obsolete
                    $('#designOptionsBlock').find('select.prodopt').each(function () {

                        var oDD = $(this);
                        var OptCatId = oDD.attr('optcatid');
                        var optVal = oDD.val();

                        if (OptCatId > 0) {

                            $('div[data-optcatid=' + OptCatId + ']').hide();

                            oDD.find('option').remove();
                            var iOpts = 0;

                            for (var i = 0; i < msg.OptionCategories.length; i++) {
                                var oCat = msg.OptionCategories[i];

                                if (oCat.OptCatId == OptCatId) {
                                    var sSel = (optVal == oCat.OptionId) ? ' selected="selected"' : '';
                                    oDD.append('<option value="' + oCat.OptionId + '"' + sSel + '>' + oCat.OptName + '</option>');
                                    iOpts++;
                                }

                            }

                            if (iOpts > 1)
                            { $('div[data-optcatid=' + OptCatId + ']').show(); }
                        }

                    });


                    if (msg.GapMessage != '') {

                        var findme = "not offered at this time. We recommend";
                        var uSelectID = parseInt($("#hidUSelectID").val());
                        var minQtyString = "";
                        var minQty = 0;

                        if (uSelectID == 1)
                        { minQty = parseInt($("#hidMinEDDMPricingQty").val()); }

                        else
                        { minQty = parseInt($("#hidMinAddressedPricingQty").val()); }



                        //Intercept the message write our own
                        if (msg.GapMessage.indexOf(findme) > -1) {
                            //console.log("FOUND IT");
                            $('#warningBlock').removeAttr("class");
                            $('#warningBlock').attr("class", "alert alert-warning");
                            $('#warningBlock').show();
                            $('#lblWarningMessage').html("This price is based on our lowest required print minimum of " + NumberWithCommas(minQty) + ".");

                        }

                        else {
                            //not sure what would be here.  Why would 'gap message' contain something other than the warning?
                            //may need to revisit.
                        }

                    }

                    else {
                        $('#warningBlock').removeAttr("class");
                        $('#warningBlock').attr("class", "alert alert-warning hidden");
                        $('#warningBlock').hide();
                    }




                    //EXPERIMENTAL 1/11/2016
                    var minRequiredQty = 0;                                                         //Required Min Qty to base pricing from.  User can order under this qty but pricing will be based on this Min Qty value. 
                    var minEDDMPricingQty = parseInt($("#hidMinEDDMPricingQty").val());             //EDDM Min Required Qty for pricing
                    var minAddressedPricingQty = parseInt($("#hidMinAddressedPricingQty").val());   //AddressedList Min Required Qty for pricing
                    var postageRate = parseFloat($("#hidPostageRate").val());
                    var minRequiredOrderTotal = 0;                                                  //This is calculated Required Min Order Total.
                    var extraPcsPricePerPiece = 0;                                                  //Price Per Piece for Extra Copies.  Does not include postage.
                    var extraPcsCost = 0;                                                           //Total cost of Extra Copies (extra copies qty * extra copies price per piece).
                    var estOrderTotal = 0;                                                          //Estimated Order Total. Used to check against. Does NOT include design fee.
                    var adjustedEstTotal = 0;                                                       //Cost of (total mailed x ppp) + extraPcsCost.  NOT charging postage on extra pcs.
                    var designFee = Number(msg.DesignPrice);                                        //Not a part of qualifying calculations.  Added only TO estimated or calculated costs.



                    //1) Determine if this is an EDDM or AddressedList quote and set minRequiredQty as needed
                    if (parseInt(UselectID) == 1)
                    { minRequiredQty = parseInt(minEDDMPricingQty); }
                    else
                    { minRequiredQty = parseInt(minAddressedPricingQty); }




                    //2) Set the Min Required Order Total
                    minRequiredOrderTotal = parseFloat(minRequiredQty * parseFloat(msg.PricePerPiece));




                    //3) Make calculations for extra copies. 
                    extraPcsPricePerPiece = parseFloat(msg.PricePerPiece - postageRate);
                    extraPcsCost = parseFloat((extraCopies * extraPcsPricePerPiece));
                    estOrderTotal = parseFloat(((totalMailed) * parseFloat(msg.PricePerPiece) + extraPcsCost));



                    //4) Check to see if the calculated order amount is < the minRequiredOrderTotal
                    if (estOrderTotal < minRequiredOrderTotal) {

                        //Min Order Amt was NOT met. Use minRequiredOrderTotal as Quoted Price.
                        //console.log("--->min order was NOT met.");

                        $('#lblEstTotal').html(Formatter.Currency(minRequiredOrderTotal + designFee));
                        $('#lblPricePerPiece').html(Formatter.Currency((msg.PricePerPiece)) + ' Each');

                    }

                    else {
                        //Min Order Amt WAS met.  Now, let's just charge for the print cost of the extra pcs - no postage on these pcs.
                        //console.log("--->min order was WAS met.");

                        adjustedEstTotal = parseFloat(((totalMailed) * parseFloat(msg.PricePerPiece)) + extraPcsCost + designFee);

                        $('#lblEstTotal').html(Formatter.Currency(adjustedEstTotal));
                        $('#lblPricePerPiece').html(Formatter.Currency(msg.PricePerPiece) + ' Each');


                    }
                    //END EXPERIMENTAL


                }


                if (debug == "true") {


                    //Debugging
                    console.log("************UpdatePriceQuote****************");
                    console.log('UpdatePriceQuote called');
                    console.log("UselectID: " + UselectID);
                    console.log("numImpressions: " + numImpressions);
                    console.log("BaseProductID: " + baseProductID);
                    console.log("totalDeliveries (num selected): " + totalDeliveries);
                    console.log("holdQTY: " + holdQTY);
                    console.log("extraCopies: " + extraCopies);
                    console.log("totalMailed (totalDeliveries x numImpressions): " + totalMailed);
                    console.log("distributionID: " + distributionID);
                    console.log("numOfDrops: " + numOfDrops);
                    console.log("zipCode: " + zipCode);
                    console.log("proDesign: " + proDesign);
                    console.log("freeTemplate: " + freeTemplate);
                    console.log("markUp: " + markUp);
                    console.log("markUpType: " + markUpType);
                    console.log("*minEDDMPricingQty: " + minEDDMPricingQty);
                    console.log("*minAddressedPricingQty: " + minAddressedPricingQty);
                    console.log("*minRequiredQty: " + minRequiredQty);
                    console.log("*minRequiredOrderTotal: " + minRequiredOrderTotal);
                    console.log("*estOrderTotal: " + estOrderTotal);
                    console.log("*postageRate: " + postageRate);
                    console.log("*extraPcsPricePerPiece: " + extraPcsPricePerPiece);
                    console.log("*extraPcsCost: " + extraPcsCost);
                    console.log("*adjustedEstTotal: " + adjustedEstTotal);
                    console.log("*designFee: " + designFee);
                    console.log("msg.Price: " + msg.Price);
                    console.log("msg.FormattedTotalPrice: " + msg.FormattedTotalPrice);
                    console.log("msg.FormattedPricePerPiece: " + msg.FormattedPricePerPiece);
                    console.log("msg.PricePerPiece: " + msg.PricePerPiece);
                    console.log("**********UpdatePriceQuote END****************");
                    //end



                }


                $('#campaignOverview').unblock();

            },

            error: function () { $('#campaignOverview').unblock(); }

        });


    }




}



function CalculateWeightedPrice(totalDeliveries, eddmEstPrice, addressedEstPrice) {

    var results = 0;

    results = ((addressedEstPrice + eddmEstPrice) / totalDeliveries)
    results = results.toFixed(2)

    //console.log('addressedEstPrice: ' + addressedEstPrice);
    //console.log('eddmEstPrice: ' + eddmEstPrice);
    //console.log('totalDeliveries: ' + totalDeliveries);

    return results

}



function CalculateCombinedPrice(eddmEstPrice, addressedEstPrice) {

    var results = 0;

    results = (addressedEstPrice + eddmEstPrice)
    results = FormatAsMoney(results);

    return results

}



function FormatAsMoney(rawNumber) {

    var newNumber = rawNumber.toFixed(2).split(".");

    return "$" + newNumber[0].split("").reverse().reduce(function (acc, rawNumber, i, orig)
    { return rawNumber + (i && !(i % 3) ? "," : "") + acc; }, "") + "." + newNumber[1];

}

