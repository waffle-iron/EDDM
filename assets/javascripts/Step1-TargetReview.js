//=====================================================================================================================
//VERSION 1.1.0
//
//  1.1.0   2/23/2016   Added the data-action listener to show "Help Modals". DSF.
//=====================================================================================================================





//Page Load
$(function ()
{


    if ($('#radNew').is(':checked'))
    {
        $('#pConfirmAcctPass').show(1000);
        $('#pEletterSignup').hide(1000);
        $('#pnlNewCustomer').show(1000);
    }


    $(window).load(function ()
    {
        var phones = [{ "mask": "(###) ###-####" }, { "mask": "(###) ###-##############" }];
        $('#txtPhoneNumber').inputmask({
            mask: phones,
            greedy: false,
            definitions: { '#': { validator: "[0-9]", cardinality: 1 } }
        });
    });



    //Hide button until FindLatLonData is finished.
    $('#targetReviewBlock').block
    ({
        message: '<h5><br /><br /><br /><br /><span class="fa fa-2x fa-cog fa-spin"></span>&nbsp;Building Map Preview.....</h5>',
        css: { width: '100%', height: '100%', border: '1px solid #dddddd' }
    });

    //disable after 4 seconds
    setTimeout(function ()
    { $('#targetReviewBlock').unblock(); }, 4000);




});



function ValidateForm()
{
    //$('.AccountStatus').
    var newOrExisting = '';
    var errorMsg = '';

    //always check email
    var email = ($("input:text[id*='EmailAddress']").val());
    if (email == '') {
        errorMsg += ('\r\n**Email Address is required');
    }

    //always check password
    var pword = ($("input:password[id*='AccountPass']").val());
    if (pword == '') {
        errorMsg += ('\r\n**Password is required');
    }

    if ($("input:radio[id*='radNew']:checked").val() != null) {
        newOrExisting = 'New';
    }
    else {
        newOrExisting = 'Existing';
    }


    if (newOrExisting == 'New')
    {

        var firstName = ($("input:text[id*='txtFirstName']").val())
        var lastName = ($("input:text[id*='txtLastName']").val());
        var userEmail = ($("input:text[id*='EmailAddress']").val());
        var companyName = ($("input:text[id*='txtCompanyName']").val());
        var phoneNumber = ($("input:text[id*='txtPhoneNumber']").val());
        var business = ($("#ddlBusinessType").val());
        var industry = ($("#ddlIndustry").val());



        if (firstName == '')
        { errorMsg += ('\r\n**First name is required'); }


        if (lastName == '')
        { errorMsg += ('\r\n**Last name is required'); }


        if (lastName == firstName)
        { errorMsg += ('\r\n**Last Name and First Name cannot be the same'); }


        if (!IsEmail(userEmail))
        { errorMsg += ('\r\n**A valid email address is required.'); }


        if (companyName == '')
        { errorMsg += ('\r\n**Company name is required'); }


        if (phoneNumber.length < 14)
        { errorMsg += ('\r\n**Please enter a valid 10-digit phone number including the area code.');}


        if (business == '')
        { errorMsg += ('\r\n**Business Type is required'); }


        if (industry == '')
        { errorMsg += ('\r\n**Industry is required'); }


    }

    else //must be existing
    {
        //no logic yet that is JUST for existing
    }

    if (errorMsg != '')
    {
        //errorMsg += ('\r\nform not valid!');
        alert(errorMsg);
        return false;
    }

    return true;

} 



function IsEmail(email)
{
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}





//Events
$('#radNew').click(function ()
{
    $('#newCustomerBlock').show(1000);
    $('#newPasswordBlock').show(1000);
    $('#newletterBlock').show(1000);
    $('#industryBlock').show(1000);
});



$('#radExisting').click(function ()
{
    $('#newCustomerBlock').hide(1000);
    $('#newPasswordBlock').hide(1000);
    $('#newletterBlock').hide(1000);
    $('#industryBlock').hide(1000);
});


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

});
