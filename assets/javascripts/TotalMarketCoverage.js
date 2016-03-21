
$("#btnGoToDemographics").click(function ()
{
    $("#tradeAreaBlock").hide(1000);
    $('#demographicsBlock').show(1000);
    $("#optionsBlock").hide();
});



$("#btnBackToTradeArea").click(function ()
{
    $("#tradeAreaBlock").show(1000);
    $('#demographicsBlock').hide(1000);
    $("#optionsBlock").hide();
});




//Show optiosn based on Market Area
//Drive time vs Radius
$("input[id^=radiusDriveTime]:radio").change(function ()
{
    var selected = this.value;

    if (selected == "driveTime")
    {
        $('#drivetimeBlock').show(1000);
        $("#radiusBlock").hide(1000);
    }

    else
    {
        $('#drivetimeBlock').hide(1000);
        $("#radiusBlock").show(1000);
    }

});



//Let the define number of peope / budget
$("input[id^=BudgetQuestion]:radio").change(function ()
{
    var selected = this.value;

    if (selected == "Y")
    {
        $('#peopleToTargetBlock').hide(1000);
        $("#peopleToTargetBlock").show(1000);
    }

    else
    {
        $('#peopleToTargetBlock').show(1000);
        $("#peopleToTargetBlock").hide(1000);
    }


});




function DemographicsChanged()
{

    $("#demographicCheckboxes input:checked").each(function ()
    {
        
        var option = ($(this).attr("value"));

        if (option == "Home")
        {
            $('#homeOwnershipBlock').show(1000);
        }


        if (option == "Kids")
        {
            $('#kidsBlock').show(1000);
        }


        if (option == "Incm")
        {
            $('#incomeBlock').show(1000);
        }


        if (option == "Age")
        {
            $('#ageBlock').show(1000);
        }


        if (option == "Gdr")
        {
            $('#genderBlock').show(1000);
        }



    });


    $("#demographicCheckboxes").each(function ()
    {

        //Total hack

        if (!$("#chkDemographics_0").is(":checked"))
        {
            $('#homeOwnershipBlock').hide(1000);
        }

        if (!$("#chkDemographics_1").is(":checked"))
        {
            $('#kidsBlock').hide(1000);
        }

        if (!$("#chkDemographics_2").is(":checked"))
        {
            $('#incomeBlock').hide(1000);
        }

        if (!$("#chkDemographics_3").is(":checked"))
        {
            $('#ageBlock').hide(1000);
        }

        if (!$("#chkDemographics_4").is(":checked"))
        {
            $('#genderBlock').hide(1000);
        }


    });


}




//To resuse
//function CheckDemographicsChecked()
//{
//    var chkBoxList = document.getElementById('#chkDemographics');
//    var chkBoxCount = chkBoxList.getElementsByTagName("input");
//    var btn = document.getElementById('#lnk_Submit');
//    var i = 0;
//    var tot = 0;
//    for (i = 0; i < chkBoxCount.length; i++)
//    {
//        if (chkBoxCount[i].checked) {
//            tot = tot + 1;
//            if (tot > 3) {
//                alert('Cannot check more than 3 check boxes');
//                chkBoxCount[i].checked = false;
//            }
//        }
//    }


//}
