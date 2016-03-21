//========================================================================================================================================
//VERSION 1.0.0
//
//
//========================================================================================================================================


//global variable
var debug = "false";



// PAGE LOAD
$(function ()
{ });




$('a[data-action=selectTemplate]').click(function (e)
{
    //No postback
    e.preventDefault();

    //grab the TemplateID, file name, and template name
    var templateID = $(this).attr('data-templateID');
    var fileName = $(this).attr('data-thumbnail');
    var templateName = $(this).attr('data-templateName');

    //hidden field
    $('#hidSelectedTemplate').val(templateID);

    //change the thumbnail
    $('#imgSelectedTemplate').attr('src', fileName);

    //change the description
    $('#litTemplateMsg').html('Good choice!  You have selected <strong>' + templateName + ' (#' + templateID + ') </strong>. Now you are ready for the last step.');

    //hide the template modal
    $('#modalTemplates').modal('hide');

    //remove all the css classes (mainly disabled) from the button and readd add the correct ones.
    $('#btnCheckOut').removeAttr('class');
    $('#btnCheckOut').attr('class', 'btn btn-danger pull-right');

    //turn the Show Templates from Red to Blue
    $('#btnShowTemplates').removeAttr('class');
    $('#btnShowTemplates').attr('class', 'btn btn-primary btn-lg');


    //$('#progressBar').removeAttr('style');
    $('#progressBar').attr('aria-valuenow', '100');
    $('#progressBar').attr('style', 'width: 100%;');
    $('#progressVal').html('100%');

});

