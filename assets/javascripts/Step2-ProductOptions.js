//VERSION 1.0.4


//NOTES:
//  1.0.1           1/5/2016        Updated HideSplitDrops logic which only applies to Ramp Express.
//  1.0.2           1/6/2016        Fixed Proceed To Checkout Hide/Show in CalculateAndValidateQuantity function.
//  1.0.3           1/13/2016       Commented out all console.log lines for production.
//  1.0.4           1/28/2016       Enhanced address validation for extra copies in ExtraCopiesChanged.



$(function ()
{
    
    //Page Load

    //See if a cookie was set on a previous visit/order OR if user selected a template from the Design Page.
    //hidSelectedTemplateID is set in code-behind if cookie is detected.

    var prevSelectedTemplateID = ($('#hidPrevSelectedTemplateID').val());

    if ((prevSelectedTemplateID != null) && (prevSelectedTemplateID != "") && (prevSelectedTemplateID != 0))
    {
        ShowPreviousUsedTemplate(prevSelectedTemplateID);
        //console.log('prevSelectedTemplateID: ' + prevSelectedTemplateID + ' (pageload function)');
    }

    // Call to init the best available design option
    DesignChanged();



});



function ValidateLaunchWeek() 
{

    var control1 = $('#phForm_ddlMyDesignLaunchWeek_ddlDropDate').val();
    var control2 = $('#phForm_ddlTemplateDesignLaunchWeek_ddlDropDate').val();
    var control3 = $('#phForm_ddlProDesignLaunchWeek_ddlDropDate').val();
    var notSelected = "--Please Select--"

    //no week selected for any variation
    if ((control1 == notSelected) && (control2 == notSelected) && (control3 == notSelected))
    {
        $('#btnCheckout').removeAttr('class');
        $('#btnCheckout').attr('class', 'btn btn-default pull-right disabled');
        //EnableCheckoutButton(false);

        $('#launchWeekBlock').removeAttr('class');
        $('#launchWeekBlock').attr('class', 'form-group has-error');
    }

    else
    {
        //EnableCheckoutButton(true);
        $('#btnCheckout').removeAttr('class');
        $('#btnCheckout').attr('class', 'btn btn-danger pull-right');

        $('#launchWeekBlock').removeAttr('class');
        $('#launchWeekBlock').attr('class', 'form-group');
    }

    CalculateAndValidateQuantity();
}



function ValidateAddress()
{

    var shipToAddress = $('#ddlDeliveryAddressId').val();
    var address1 = $('#DeliveryAddress').val();
    var city = $('#DeliveryCity').val();


    if (shipToAddress == "(New Address)")
    {
        if ((address1 == '') && (city == '')) 
        {

            $('#address1Block').removeAttr('class');
            $('#address1Block').attr('class', 'form-group has-error');

            $('#cityBlock').removeAttr('class');
            $('#cityBlock').attr('class', 'form-group has-error');
        }

        else
        {
            $('#address1Block').removeAttr('class');
            $('#address1Block').attr('class', 'form-group');

            $('#cityBlock').removeAttr('class');
            $('#cityBlock').attr('class', 'form-group');
        }

    }


}



function ValidateEntireAddress()
{

    var shipToAddress = $('#ddlDeliveryAddressId').val();
    var address1 = $('#DeliveryAddress').val();
    var city = $('#DeliveryCity').val();
    var zip = $('#DeliveryCity').val();


    if (parseInt(shipToAddress) == 0)
    {

        if ((address1 != '') && (city != '') && (zip != ''))
        {
            $('#btnContinue').removeAttr('class');
            $('#btnContinue').attr('class', 'btn btn-danger btn-lg pull-right');
        }

        else
        {
            $('#btnContinue').removeAttr('class');
            $('#btnContinue').attr('class', 'btn btn-default btn-lg pull-right disabled');
        }

    }


}



function StreetChanged()
{

    var address1 = $('#DeliveryAddress').val();


    if (address1 == "")
    {
        $('#address1Block').removeAttr('class');
        $('#address1Block').attr('class', 'form-group has-error');
    }


    else 
    {
        $('#address1Block').removeAttr('class');
        $('#address1Block').attr('class', 'form-group');
    }


}



function CityChanged()
{

    var city = $('#DeliveryCity').val();


    if (city == "")
    {
        $('#cityBlock').removeAttr('class');
        $('#cityBlock').attr('class', 'form-group has-error');
    }


    else {
        $('#cityBlock').removeAttr('class');
        $('#cityBlock').attr('class', 'form-group');
    }


}



function ZipChanged()
{

    var zip = $('#ZipCode').val();


    if (zip == "")
    {
        $('#zipBlock').removeAttr('class');
        $('#zipBlock').attr('class', 'form-group has-error');
    }


    else {
        $('#zipBlock').removeAttr('class');
        $('#zipBlock').attr('class', 'form-group');
    }


}



function LaunchWeekChanged(controlName)
{

    if (controlName == "phForm_ddlMyDesignLaunchWeek_ddlDropDate")
    {
        newWeek = ($('#phForm_ddlMyDesignLaunchWeek_ddlDropDate').val());
        $('#hidLaunchWeek').val(newWeek);
        $('#txtLaunchWeek').val(newWeek);
    }


    if (controlName == "phForm_ddlTemplateDesignLaunchWeek_ddlDropDate")
    {
        newWeek = ($('#phForm_ddlTemplateDesignLaunchWeek_ddlDropDate').val());
        $('#hidLaunchWeek').val(newWeek);
        $('#txtLaunchWeek').val(newWeek);
    }


    if (controlName == "phForm_ddlProDesignLaunchWeek_ddlDropDate")
    {
        newWeek = ($('#phForm_ddlProDesignLaunchWeek_ddlDropDate').val());
        $('#hidLaunchWeek').val(newWeek);
        $('#txtLaunchWeek').val(newWeek);
    }


}



function ProductChanged()
{ $('#warningModal').modal('show'); }



function ExtraCopiesChanged()
{
    
    var extraCopies = ($('#txtExtraCopies').val());
    var deliverAddressID = ($("#ddlDeliveryAddressId").val());


    if (extraCopies > 0)
    {

        $('#shipToAddressBlock').show(1000);

        if (deliverAddressID == "--Please Select--")
        {

            //strip and disable button.
            $('#btnContinue').removeAttr('class');
            $('#btnContinue').attr('class', 'btn btn-default pull-right disabled');

            $('#ShippingAddressInnerBlock').removeAttr('class');
            $('#ShippingAddressInnerBlock').attr('class', 'form-group has-error');     //<-- padded too much?

        }

        else
        {
            //good to go. Enable button.
            $('#btnContinue').removeAttr('class');
            $('#btnContinue').attr('class', 'btn btn-danger btn-lg pull-right');

            $('#ShippingAddressInnerBlock').removeAttr('class');
            $('#ShippingAddressInnerBlock').attr('class', 'form-group');

        }

    }

    else
    { $('#shipToAddressBlock').hide(1000); }


    //Set the Overiview Label.  NumberWithCommas is in CampaignOverview.js
    $('#lblExtraCopies').html(NumberWithCommas(extraCopies));

    UpdatePriceQuote();
    CalculateAndValidateQuantity();

}



function CalculateAndValidateQuantity()
{

    var isExclusive = ($('#hidExclusiveSite').val()); 


    //If this is a site which offers route exclusivity
    if (isExclusive) 
    {

        var extraCopies = parseInt(($('#txtExtraCopies').val()));
        var impressionsChoice = parseInt(($('#ddlImpressions').val()));
        var dropsChoice = ($('#ddlDrops').val());
        var totalDeliveries = $('#lblTotalDeliveries').html();
        totalDeliveries = Number(totalDeliveries.toString().replace(",", ""));
        totalDeliveries = parseInt(totalDeliveries);

        var totalOrderQuantity = parseInt(totalDeliveries * impressionsChoice) + parseInt(extraCopies);
        var minQuantity = parseInt($('#hdnMinimumToOrder').val());
        var minImpressions = parseInt($('#hidMinimumImpressionExclusive').val());
        var minExclusiveQuantity = parseInt($('#hidMinimumQtyExclusive').val());

        //qualifies for exclusive
        if ((totalOrderQuantity >= minExclusiveQuantity) && (impressionsChoice >= minImpressions))  
        {
            $('#btnContinue').removeAttr('class');
            $('#btnContinue').attr('class', 'btn btn-danger btn-lg pull-right');

            var message = $('#hidExclusiveQualify').val();
            $('#lExclusive').text(message);
            $('#btnCheckout').removeAttr('class');
            $('#btnCheckout').attr('class', 'btn btn-danger pull-right');
        }

        //doesn't qualify for exclusive
        else
        {

            if (totalOrderQuantity < minQuantity)
            {
                //quantity too small to checkout
                var message = $('#hidExclusiveNeedsMore').val();
                $('#lExclusive').text(message);
                $('#btnCheckout').removeAttr('class');
                $('#btnCheckout').attr('class', 'btn btn-default pull-right disabled');
            }

            else
            {


                //quantity can check out 
                var message = $('#hidExclusiveDoesNotQualify').val();
                var launchWeek = $('#hidLaunchWeek').val();

                //but not enough for exclusive
                $('#lExclusive').text(message);

                //A valid lauch week must be selected
                if (launchWeek != "--Please Select--")
                {
                    $('#btnCheckout').removeAttr('class');
                    $('#btnCheckout').attr('class', 'btn btn-danger pull-right');
                }

            }

        }

    }


    //Finally, double check the ShipToAddressID and Extra Copies. Let's make sure they picked an actual address before continuing.
    var extraCopies = ($('#txtExtraCopies').val());
    var deliverAddressID = ($("#ddlDeliveryAddressId").val());

    if ((parseInt(extraCopies) > 0) && (deliverAddressID == "--Please Select--"))
    {
        //disable the button until they pick an address
        $('#btnContinue').removeAttr('class');
        $('#btnContinue').attr('class', 'btn btn-default btn-lg pull-right disabled');

    }



}



function DeliveryAddressChanged()
{

    var addressChoice = ($('#ddlDeliveryAddressId').val());

    //User selected to enter a new address
    if (addressChoice == "0")
    { $('#newAddressBlock').show(1000); }

    //user has selected either a pre-existing address or Please Select is still selected.
    else 
    {

        $('#newAddressBlock').hide(1000);

        //Not always present.  Only avail on some sites.
        if (addressChoice == "--Please Select--")
        {
            //strip and disable button.
            $('#btnContinue').removeAttr('class');
            $('#btnContinue').attr('class', 'btn btn-default btn-lg pull-right disabled');

            $('#ShippingAddressInnerBlock').removeAttr('class');
            $('#ShippingAddressInnerBlock').attr('class', 'form-group has-error');
        }

        //Good to go!
        else
        {
            //good to go. Enable button.
            $('#btnContinue').removeAttr('class');
            $('#btnContinue').attr('class', 'btn btn-danger btn-lg pull-right');

            $('#ShippingAddressInnerBlock').removeAttr('class');
            $('#ShippingAddressInnerBlock').attr('class', 'form-group');
        }
        
    }

}



function OptionsUpdated()
{
    //place holder
}



function DesignChanged()
{


    var designChoice = ($('#ddlDesignOption').val());
    var selectedTemplate = ($('#hidSelectedTemplateID').val());
    //console.log("DesignChanged called - " + designChoice);

    if (designChoice == "My") 
    {
        $('#myDesignBlock').show(1000);
        $('#freeTemplateBlock').hide(1000);
        $('#professionalDesignBlock').hide(1000);

        //Reset button styling
        $('#btnContinue').removeAttr('class');
        $('#btnContinue').attr('class', 'btn btn-danger btn-lg pull-right');
        //console.log(" DesignChanged -- btnContinue should now be enabled");

        //Show correct Launch Week
        $('#myDesignLaunchWeekBlock').show(1000);
        $('#templateDesignLaunchWeekBlock').hide(1000);
        $('#proDesignLaunchWeekBlock').hide(1000);

        //RESET the default Launch Week Date
        var defaultLaunchWeek = ($('#phForm_ddlMyDesignLaunchWeek_ddlDropDate').val());
        $('#hidLaunchWeek').val(defaultLaunchWeek);
        $('#txtLaunchWeek').val(defaultLaunchWeek);

        //Reset the other two drop down values
        $("#phForm_ddlTemplateDesignLaunchWeek_ddlDropDate").prop('selectedIndex', 0);
        $("#phForm_ddlProDesignLaunchWeek_ddlDropDate").prop('selectedIndex', 0);


    }

    else if (designChoice == "Template")
    {
        var prevSelectedTemplateID = ($('#hidPrevSelectedTemplateID').val());

        if ((prevSelectedTemplateID != null) && (prevSelectedTemplateID != "") && (prevSelectedTemplateID != 0)) {
            //ShowPreviousUsedTemplate(prevSelectedTemplateID);
            //console.log('prevSelectedTemplateID: ' + prevSelectedTemplateID + ' (pageload function)');
            selectedTemplate = prevSelectedTemplateID;
        }

        $('#myDesignBlock').hide(1000);
        $('#freeTemplateBlock').show(1000);
        $('#professionalDesignBlock').hide(1000);

        //Show correct Launch Week
        $('#myDesignLaunchWeekBlock').hide(1000);
        $('#templateDesignLaunchWeekBlock').show(1000);
        $('#proDesignLaunchWeekBlock').hide(1000);

        //RESET the default Launch Week Date
        var defaultLaunchWeek = ($('#phForm_ddlTemplateDesignLaunchWeek_ddlDropDate').val());
        $('#hidLaunchWeek').val(defaultLaunchWeek);
        $('#txtLaunchWeek').val(defaultLaunchWeek);

        //Reset the other two drop down values
        $("#phForm_ddlMyDesignLaunchWeek_ddlDropDate").prop('selectedIndex', 0);
        $("#phForm_ddlProDesignLaunchWeek_ddlDropDate").prop('selectedIndex', 0);
        //console.log("TEMPLATE DETECTED. RESET other two drop down weeks. (DesignChanged)");
        //console.log("selectedTemplate:" + selectedTemplate);
        //console.log("Manually put number in template");
        //selectedTemplate = 1;

        //No template has been selected so disable Continue button
        if (selectedTemplate == "0")
        {
            $('#btnContinue').removeAttr('class');
            $('#btnContinue').attr('class', 'btn btn-default btn-lg pull-right disabled');
            //console.log("DesignChanged btnContinue should be disabled");

        }

        else
        {
            //Reset button styling
            $('#btnContinue').removeAttr('class');
            $('#btnContinue').attr('class', 'btn btn-danger btn-lg pull-right');
            //console.log("DesignChanged btnContinue should be enabled");

        }

    }

    else if (designChoice == "Pro")
    {

        $('#myDesignBlock').hide(1000);
        $('#freeTemplateBlock').hide(1000);
        $('#professionalDesignBlock').show(1000);

        //Show correct Launch Week
        $('#myDesignLaunchWeekBlock').hide(1000);
        $('#templateDesignLaunchWeekBlock').hide(1000);
        $('#proDesignLaunchWeekBlock').show(1000);

        //Reset button styling
        $('#btnContinue').removeAttr('class');
        $('#btnContinue').attr('class', 'btn btn-danger btn-lg pull-right');

        //RESET the default Launch Week Date
        var defaultLaunchWeek = ($('#phForm_ddlProDesignLaunchWeek_ddlDropDate').val());
        $('#hidLaunchWeek').val(defaultLaunchWeek);
        $('#txtLaunchWeek').val(defaultLaunchWeek);

        //Reset the other two drop down values
        $("#phForm_ddlMyDesignLaunchWeek_ddlDropDate").prop('selectedIndex', 0);
        $("#phForm_ddlTemplateDesignLaunchWeek_ddlDropDate").prop('selectedIndex', 0);
        //console.log("PRO DESIGN DETECTED. RESET other two drop down weeks. (DesignChanged)");

    }

    //console.log("UpdatePriceQuote called. (DesignChanged)");
    UpdatePriceQuote();

    //console.log("ValidateLaunchWeek called. (DesignChanged)");
    ValidateLaunchWeek();

    CalculateAndValidateQuantity();
}



function ImpressionsChanged()
{

    var impressionsChoice = ($('#ddlImpressions').val());
    var dropsChoice = ($('#ddlDrops').val());
    var hidHideSplitDrops = ($('#hidHideSplitDrops').val().toLowerCase());

    var totalDeliveries = $('#lblTotalDeliveries').html();
    totalDeliveries = Number(totalDeliveries.toString().replace(",", ""));
    totalDeliveries = parseInt(totalDeliveries);
    var totalMailedCalc = (totalDeliveries * impressionsChoice)

    if (parseInt(impressionsChoice) == 1)
    {
        //Don't show any split drop questions
        if (hidHideSplitDrops == "true")
        {
            $('#pnlDropsBlock').hide(1000);         //<-- Q: Do you want to mail all of your pieces at once?
            $('#pnlNumOfDropsBlock').hide(1000);
            $('#pnlFrequencyBlock').hide(1000);     //<-- Every X amount of weeks
        }
        else
        {
            $('#pnlDropsBlock').show(1000);         //<-- Q: Do you want to mail all of your pieces at once?
            $('#pnlNumOfDropsBlock').show(1000);
            $('#pnlFrequencyBlock').show(1000);     //<-- Every X amount of weeks
        }
    }

    //Split drops not offered with Multiple Impressions
    else
    {

        $('#pnlDropsBlock').hide(1000);             //<-- Q: Do you want to mail all of your pieces at once?
        $('#pnlNumOfDropsBlock').hide(1000);
        $('#pnlFrequencyBlock').show(1000);         //<-- Every X amount of weeks

        //Reset # of drops
        $("#ddlNumOfDrops").prop('selectedIndex', 0);

        //Reset 'Mail All At Once' question
        $("#ddlDrops").prop('selectedIndex', 0);

    }


    //Set the Overview Label
    $('#lblPiecesMailed').html(NumberWithCommas(totalMailedCalc));
    CalculateAndValidateQuantity();

}



function DropsChanged()
{
    var dropsChoice = ($('#ddlDrops').val());

    if (dropsChoice == "Yes")
    {
        $('#pnlNumOfDropsBlock').hide(1000);
        $('#pnlFrequencyBlock').hide(1000);
    }

    else
    {
        $('#pnlNumOfDropsBlock').show(1000);
        $('#pnlFrequencyBlock').show(1000);
    }

    CalculateAndValidateQuantity();

}



function EnableCheckoutButton(enable)
{

    //console.log("EnableCheckoutButton:" + enable)

    //If user arrives (or comes back) to this page, do not let them checkout until they pick a Launch Week.
    if (enable)
    {
        //unlock button
        $('#btnCheckOut').removeAttr('class');
        $('#btnCheckOut').attr('class', 'btn btn-danger btn-lg pull-right');
        //console.log("EnableCheckoutButton: btnCheckOut should now be enabled");
    }
    else
    {
        $('#btnCheckOut').removeAttr('class');
        $('#btnCheckOut').attr('class', 'btn btn-default pull-right disabled');
        //console.log("EnableCheckoutButton: btnCheckOut should now be disabled");

    }

   
}



function ContinueToDeliveryOptions()
{

    //Hide Design Options, Show Delivery Options
    $('#deliveryOptionsBlock').show(1000);
    $('#designOptionsBlock').hide(1000);

    var defaultMyDesignLaunchWeek = ($('#phForm_ddlMyDesignLaunchWeek_ddlDropDate').val());
    var defaultTemplateDesignLaunchWeek = ($('#phForm_ddlTemplateDesignLaunchWeek_ddlDropDate').val());
    var defaultProDesignLaunchWeek = ($('#phForm_ddlProDesignLaunchWeek_ddlDropDate').val());
    var designChoice = ($('#ddlDesignOption').val());

    //Ensure they cannot click Checkout & Quote buttons if no Launch Week has been selected.
    if ((designChoice == "My") && (defaultMyDesignLaunchWeek == "--Please Select--"))
    {
        //If user arrives (or comes back) to this page, do not let them checkout until they pick a Launch Week.
        $('#btnCheckout').removeAttr('class');
        $('#btnCheckout').attr('class', 'btn btn-default pull-right disabled');
        //EnableCheckoutButton(false);
    }

    if ((designChoice == "Template") && (defaultTemplateDesignLaunchWeek == "--Please Select--"))
    {
        //If user arrives (or comes back) to this page, do not let them checkout until they pick a Launch Week.
        $('#btnCheckout').removeAttr('class');
        $('#btnCheckout').attr('class', 'btn btn-default pull-right disabled');
        //EnableCheckoutButton(false);

    }

    if ((designChoice == "Pro") && (defaultProDesignLaunchWeek == "--Please Select--"))
    {
        //If user arrives (or comes back) to this page, do not let them checkout until they pick a Launch Week.
        $('#btnCheckout').removeAttr('class');
        $('#btnCheckout').attr('class', 'btn btn-default pull-right disabled');
        //EnableCheckoutButton(false);
    }


}



function BackToDesignOptions()
{
    $('#deliveryOptionsBlock').hide(1000);
    $('#designOptionsBlock').show(1000);
}



function ShowPreviousUsedTemplate()
{

    $('#myDesignBlock').hide(1000);
    $('#freeTemplateBlock').show(1000);
    $('#professionalDesignBlock').hide(1000);

    //Set the selected item
    $("#ddlDesignOption").val("Template");

    //hide message    
    $('#notSelectedTemplateBlock').hide();

    //Show.
    //Image and lblYouHaveSelected are set in code-behind
    $('#goodChoiceFreeDesignBlock').removeAttr('class');
    $('#goodChoiceFreeDesignBlock').attr('class', 'alert alert-success');
    $('#goodChoiceFreeDesignBlock').show();

    //unlock button
    $('#btnContinue').removeAttr('class');
    $('#btnContinue').attr('class', 'btn btn-danger btn-lg pull-right');

    CalculateAndValidateQuantity();

}



function LoadNewProduct()
{

    var selectedProductID = ($('#ddlProduct').val());
    var distributionID = ($('#hidDistributionID').val());
    var selectedBaseProductID = $('#ddlProduct').find(":selected").attr('baseprodid');

    window.location = "/Step2-ProductOptions.aspx?productid=" + selectedProductID + "&distid=" + distributionID + "&baseid=" + selectedBaseProductID;

}



function ShowDeliveryOptions()
{
    $('#pnlCampaignOverview').show(1000);
}



//More Information Windows
$('a[data-action=infowindow]').click(function (e)
{
    e.preventDefault();

    var productID = $('#hidProductID').val();
    var baseProductID = $('#hidBaseProductID').val();
    var pageref = $(this).attr('data-helpfile');
    var title = $(this).attr('data-title');


    //Product options only
    var optcatid = $(this).attr('data-optcatid');

    $('#infoTitle').html(title);
    $('#infoContent').load('/Resources/ProductConfigHelp.ashx?pid=' + productID + '&bpid=' + baseProductID + '&catid=' + optcatid + '&pageref=' + pageref);

    //Debugging
    //console.log("productID: " + productID);
    //console.log("baseProductID: " + baseProductID);
    //console.log("optcatid: " + optcatid);
    //console.log("pageref: " + pageref);

});



$('#txtExtraCopies').bind('keypress', function (evt)
{
    //Do not allow Enter key to trigger this button
    if (evt.keyCode == 13)
    { evt.preventDefault(); }
});


