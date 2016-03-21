
//Change 'environment' variable to match the environment. Values should be either "dev" or "prod".

//TO DO: Assign the drop down lists and controls specific css classes instead of targeting them by ID.  This way, we do not need to 
//code for site or environment specific control IDs. Then, simply find the control by it's specific custom class.


var environment = "dev";
var haveListControlID = "";
var productControlID = "";


if (environment.toLowerCase() == "dev")
{
    haveListControlID = "phBody_ctl01_fld1500";
    productControlID = "phBody_ctl01_fld1492";
}

else
{
    haveListControlID = "phBody_ctl01_fld980";
    productControlID = "phBody_ctl01_fld981";
}



//Wire the control
$('#' + haveListControlID).attr('onchange', 'ListChoiceChanged();');            

//hide products
$('#listProducts').hide();



function ListChoiceChanged()
{


    var haveOwnList = $('#' + haveListControlID).val();


    //User Generated List Products
    var createdListProd1 = 'Direct Mail 4x6 Postcard with Build Your List';
    var createdListProd2 = 'Direct Mail 5.5x8.5 Postcard with Build Your List';
    var createdListProd3 = 'Direct Mail 6x11 Postcard with Build Your List';

    //Uploaded List Products
    var uploadedListProd1 = 'Direct Mail 4x6 Postcard with Upload Your List';
    var uploadedListProd2 = 'Direct Mail 5.5x8.5 Postcard with Upload Your List';
    var uploadedListProd3 = 'Direct Mail 6x11 Postcard with Upload Your List';



    if (haveOwnList == "Yes")
    {

        $('#listProducts').show(1000);


        //Remove User Generated Lists
        $("select#" + productControlID + " option[value='" + createdListProd1 + "']").remove();
        $("select#" + productControlID + " option[value='" + createdListProd2 + "']").remove();
        $("select#" + productControlID + " option[value='" + createdListProd3 + "']").remove();


        //Add back Uploaded List Products if needed
        if ($("#" + productControlID + " option").length < 3)
        {
            $("#" + productControlID).append($("<option></option>").attr("value", uploadedListProd1).text(uploadedListProd1));
            $("#" + productControlID).append($("<option></option>").attr("value", uploadedListProd2).text(uploadedListProd2));
            $("#" + productControlID).append($("<option></option>").attr("value", uploadedListProd3).text(uploadedListProd3));
        }

    }


    else if (haveOwnList == "No")
    {
        $('#listProducts').show(1000);

        //Remove Uploaded List Products
        $("select#" + productControlID + " option[value='" + uploadedListProd1 + "']").remove();
        $("select#" + productControlID + " option[value='" + uploadedListProd2 + "']").remove();
        $("select#" + productControlID + " option[value='" + uploadedListProd3 + "']").remove();


        //Add back User Generated List Products if needed
        if ($("#" + productControlID + " option").length < 3)
        {
            $("#" + productControlID).append($("<option></option>").attr("value", createdListProd1).text(createdListProd1));
            $("#" + productControlID).append($("<option></option>").attr("value", createdListProd2).text(createdListProd2));
            $("#" + productControlID).append($("<option></option>").attr("value", createdListProd3).text(createdListProd3));
        }

    }

    else
    {
        $('#listProducts').hide();
    }



}


