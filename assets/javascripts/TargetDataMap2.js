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
    $('#step2Section').hide();
    $('#campaignOverview').hide();
    $('#customMap').hide();
    $('#campaignProgress').hide();


    //baseProductID is needed to recalculate PPP.
    var baseProductID = RetrieveFromQueryString.GetValue('bp');
    LoadPricePerPieceGrid(baseProductID);


    setTimeout(function ()
    {
        $("#step2LoadingSection").hide();
        $('#step2Section').fadeIn(500);
        $("#step2Section").show();
        $('#campaignOverview').fadeIn(500);
        $('#campaignOverview').show();
        $('#customMap').fadeIn(500);
        $('#customMap').show();
        $('#campaignProgress').fadeIn(1000);
        $('#campaignProgress').show();

        Calculate();
    },
    4000);



    $("input[type=checkbox]").change(function ()
    { UpdateRouteTotals(); });

    $('#btnShowMeMap').on('click', function (e)
    {
        var src = $(this).attr('data-src');

        $("#modalMap iframe").attr(
        {
            'src': src,
        });
    });



});
