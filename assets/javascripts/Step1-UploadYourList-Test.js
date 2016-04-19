//==============================================================================================================
//VERSION 1.0.0
//
//==============================================================================================================




//PAGE LOAD
$(function ()
{


    //Hide initial elements
    $("#identifyStep2").hide();
    $("#continueStep3").hide();
    $("#uploadResponse").hide();
    $("#warningMessageAlert").hide();
    $("#dialog").hide();


    //set the height of the results panels
    $("#original-addresses-body").css("height","200px");
    $("#invalid-addresses-body").css("height", "200px");
    $("#valid-addresses-body").css("height", "200px");



    var projectId = getParameterByName('p');
    var columnCount = 0;
    var sExt = '';
    var sRet = '';
    var debug = "false";

    //DROP ZONE
    Dropzone.autoDiscover = false;

    $('#mydropzone').dropzone(
    {
        url: 'MyListUpload.ashx?p=' + escape(projectId), addRemoveLinks : false, 
    });

    var oDropZone = Dropzone.forElement('#mydropzone');

    oDropZone.on('success', function (file, response)
    {

        //console.log('success so far....');

        if (response.Success === true)
        {
            oDropZone.removeFile(file);
            doIdentify(response.FileName);
        }
    });



    // Field map check list. Does not need to contain entries with spaces, they will be compared with spaces removed.
    var oMapFields = [];

    oMapFields.push({ value: '', text: 'Unmapped' });
    oMapFields.push({ value: 'FirstName', text: 'First Name', check: ['firstname', 'fname'] });
    oMapFields.push({ value: 'LastName', text: 'Last Name', check: ['lastname', 'lname'] });
    oMapFields.push({ value: 'FullName', text: 'Full Name', check: ['fullname', 'name'] });
    oMapFields.push({ value: 'Company', text: 'Company Name', check: ['company', 'companyname', 'organization'] });
    oMapFields.push({ value: 'Address1', text: 'Address 1', check: ['address1', 'addressline1', 'deliveryline1']});
    oMapFields.push({ value: 'Address2', text: 'Address 2', check: ['address2', 'addressline2', 'deliveryline2'] });
    oMapFields.push({ value: 'City', text: 'City', check: ['city'] });
    oMapFields.push({ value: 'State', text: 'State', check: ['state', 'province'] });
    oMapFields.push({ value: 'ZipCode', text: 'Zip Code', check: ['zip', 'zipcode', 'postalcode'] });


    var oMapReqOpts = [];

    // Some name variant + address + zip code
    oMapReqOpts.push(['FirstName', 'LastName', 'Address1', 'ZipCode'].sort());
    oMapReqOpts.push(['FullName', 'Address1', 'ZipCode'].sort());
    oMapReqOpts.push(['Company', 'Address1', 'ZipCode'].sort());

    // Some name variant + address + city + state
    oMapReqOpts.push(['FirstName', 'LastName', 'Address1', 'City', 'State'].sort());
    oMapReqOpts.push(['FullName', 'Address1', 'City', 'State'].sort());
    oMapReqOpts.push(['Company', 'Address1', 'City', 'State'].sort());



    function getMapOptions()

    {
        
        for (var i = 0; i < oMapFields.length; i++)
        {
            sRet += '<option value="' + oMapFields[i].value + '">' + oMapFields[i].text + '</option>';
            //console.log(oMapFields[i].value);
        }

        return sRet;
    }



    function doIdentify(sFile)
    {

        sExt = sFile.split('.').pop();

        var sMapOptions = getMapOptions();

        $.ajax(
        {
            url:'MyListIdentify.ashx',
            data: {p:projectId, ext:sExt},
            success: function (data)
            {

                //console.log("data.ColumnCount: " + data.ColumnCount);

                if (data.ColumnCount > 0)
                {
                    columnCount = data.ColumnCount;

                    $('#myIdentify table > tbody').empty().append(data.PreviewRows);

                    $('#myIdentify table > thead > tr').empty();
                    for (var i = 0; i < data.ColumnCount; i++)
                    {
                        $('#myIdentify table > thead > tr').append('<th><select class="form-control input-sm" id="col' + i + '">' + sMapOptions + '</select></th>');
                    }

                    $('#warningMessageAlert').hide();
                    $('#uploadStep1').hide(1000);
                    $('#identifyStep2').show(1000);


                }

                else
                {
                    $('#identifyStep2').hide();
                    $('#warningMessageAlert').show(1000);
                    $('#warningMessage').html('The file you uploaded appears to be missing the required number of columns.');

                }

            },

            error: function ()
            {
                $('#warningMessageAlert').show(1000);
                $('#warningMessage').html('Sorry but there was an error uploading and reading your data.<br /><br />' + ex.statusText);
            }

        });

    }



    $('#chkFirstContainsFieldNames').change(function ()
    {
        if ($(this).is(':checked'))
        {

            //console.log('checkbox checked...');

            var oCurMaps = [];

            //hide row 1
            $("#row1").hide();

            for (var iMap = 0; iMap < oMapFields.length; iMap++)
            {

                //console.log('iMap: ' + iMap);

                var sCurVal = oMapFields[iMap].value;
                if (sCurVal !== '')
                {
                    if ($('#myIdentify select[value=' + sCurVal + ']').length > 0) oCurMaps.push(sCurVal);
                }

            }

            var iCols = $('#myIdentify table > thead > tr > th').length;

            for (var iCol = 0; iCol < iCols; iCol++)
            {

                var oSel = $('#myIdentify table > thead > tr > th:eq(' + iCol + ') select');
                var sName = $('#myIdentify table > tbody > tr:first > td:eq(' + iCol + ')').text();
                var cName = sName.toLowerCase().replace(/[^A-Z0-9az]/ig, '');

                // check our list of possible matches to try to map
                for (var iCur = 0; iCur < oMapFields.length; iCur++)
                {
                    var sCurMapVal = oMapFields[iCur].value;

                    if (sCurMapVal !== '')
                    {

                        var oCheck = oMapFields[iCur].check;

                        if (oCurMaps.indexOf(sCurMapVal) < 0)
                        {
                            // check the list of "check" values
                            if (oCheck.indexOf(cName) >= 0)
                            {
                                oSel.val(sCurMapVal);
                                oCurMaps.push(sCurMapVal);
                                break;
                            }

                        }

                    }

                }

            }

        }


        else
        {

            $("#row1").show();


            for (var iCol = 0; iCol < iCols; iCol++)
            {

                // check our list of possible matches to try to map
                for (var iCur = 0; iCur < oMapFields.length; iCur++)
                {
                    $("#col" + iCur).prop('selectedIndex', 0);
                }

            }

        }


    });



    function getFieldMap()
    {
        var oRet = [];
        var iCols = $('#myIdentify table > thead > tr > th').length;

        for (var iCol = 0; iCol < iCols; iCol++)
        {
            var oSel = $('#myIdentify table > thead > tr > th:eq(' + iCol + ') select');
            oRet.push({col:iCol, colname:'', fld:oSel.val()});
        }

        return oRet;

    }


    $('#btnBackToFileUpload').click(function (e)
    {
        e.preventDefault();

        $("#uploadStep1").show(1000);
        $("#identifyStep2").hide(1000);
        $("#continueStep3").hide(1000);
        $("#warningMessageAlert").hide(1000);

    });


    $('#btnProcessMyList').click(function (e)
    {

        e.preventDefault();

        // Need to run a sanity check on what we have in here right now (Make sure at least one of the req scenarios is met)
        var oCurMaps = [];

        $('#myIdentify select').each(function ()
        {
            if ($(this).val() !== '')
            {
                oCurMaps.push($(this).val());
            }

        });

        var bValid = false;

        for (var i = 0; i < oMapReqOpts.length; i++)
        {
            // Check the map options
            var bCheckValid = true;
            var oReq = oMapReqOpts[i];

            for (var iReq = 0; iReq < oReq.length; iReq++)
            {

                if (oCurMaps.indexOf(oReq[iReq]) < 0)
                {
                    bCheckValid = false;
                    break;
                }

            }

            if (bCheckValid)
            {
                bValid = true;
                break;
            }

        }

        if (!bValid)
        {
            // Show a message that they need to map a minimum amount of data
            $('#warningMessageAlert').show();
            $('#warningMessage').html("Please provide mapping for a minimum amount of data: The recipients name in the form of First Name and Last Name, Full Name or Company Name, the street address and either the City and State or the Zip Code.");

            return;
        }
        

        $('#warningMessageAlert').hide();
        $('#identifyStep2').hide(1000);
        $('#continueStep3').show(1000);

        //console.log("getFieldMap:" + JSON.stringify(getFieldMap()));


        //Rename this handler ???  Why version #2?
        $.ajax({
            url: '/Addressed/MyListCertify2.ashx',
            //url: '/Addressed/MyListCertify.ashx',
            type: 'POST',
            data: { p: projectId, ext: sExt, hr:$('#chkFirstContainsFieldNames').is(':checked'), fldMap: JSON.stringify(getFieldMap()) },
            success: function (data)
            {

                if (data)
                {

                    console.log(data.OriginalCount);
                    console.log(data.InvalidCount);
                    console.log(data.ValidCount);


                    $('#oAddressCount').html(Formatter.Commas(data.OriginalCount));
                    $('#oInvalidCount').html(Formatter.Commas(data.InvalidCount));
                    $('#oValidCount').html(Formatter.Commas(data.ValidCount));
                    $('#hfListCount').val(data.ValidCount);

                    $('#processingFile').hide(1000);
                    $('#uploadResponse').show(1000);
                    $('#warningMessageAlert').hide(1000);

                    //Disable button if there are zero errors.
                    if (data.InvalidCount <= 0)
                    {
                        $("#btnDownloadErrors").removeClass();
                        $("#btnDownloadErrors").addClass("btn btn-block btn-default btn-sm disabled");
                    }

                }

                else
                {
                    $('#warningMessageAlert').show(1000);
                    $('#warningMessage').html('Sorry but there was a problem certifying your list (data is empty).');
                }
            },

            error: function ()
            {
                // there was a problem

                $('#warningMessageAlert').show(1000);
                $('#warningMessage').html('Sorry but there was a problem certifying your list (error reading data).');

            }

        });

    });



    if (debug.toLowerCase() == "true")
    {
        $("#identifyStep2").show();
        $("#continueStep3").show();
        $("#uploadResponse").show();
        $("#processingFile").show();
        $("#warningMessageAlert").show();

        $("#warningMessageAlert").show();
        $('#warningMessage').html('<span class="fa fa-warning"></span>&nbsp;Warning message here.......');

        $("#pnlDebug").removeAttr("class");
    }

});




//functions
function getParameterByName(name)
{
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}




//Events
$("#show-errors").click(function ()
{

    event.preventDefault();


    //Request url
    var projectId = getParameterByName('p');
    var endPoint = "/Resources/ShowInvalidUploadedAddresses.ashx?p=" + projectId;


    $('#invalid-addresses-data').block
    ({
        message: '<div><br/><h5><span class="fa fa-2x fa-cog fa-spin"></span>&nbsp;Getting your addresses.....</h5><br/><br/><br/></div>',
        css: { width: '100%', height: '100%', border: '1px solid #dddddd' }
    });


    $.ajax(
        {
            url: endPoint,
            type: 'POST',

            success: function (response)
            {

                if (response != null && response != '')
                {

                    $("#invalid-addresses-data").html(response);

                }

                $('#invalid-addresses-data').unblock();

            },

            error: function (ex)
            {
                console.log("Show Sample Error: " + ex.statusText);
                $("#invalid-addresses-data").html('<div class="alert alert-danger"><h3 class="text-danger"><span class="fa fa-exclamation-circle fa-2x"></span>&nbsp;Uh oh, something went wrong.</h3><p>We\'re sorry but something went wrong when retrieving the list. Please <a href="/Support">contact us</a> to request this data.</p><p>&nbsp;</p><p>Error Code: ' + ex.statusText + '</p></div>');
                $('#invalid-addresses-data').unblock();

            }

        });

});




// attach the .equals method to Array's prototype to call it on any array
Array.prototype.equals = function (array) {
    // if the other array is a falsy value, return
    if (!array)
        return false;

    // compare lengths - can save a lot of time 
    if (this.length != array.length)
        return false;

    for (var i = 0, l = this.length; i < l; i++) {
        // Check if we have nested arrays
        if (this[i] instanceof Array && array[i] instanceof Array) {
            // recurse into the nested arrays
            if (!this[i].equals(array[i]))
                return false;
        }
        else if (this[i] != array[i]) {
            // Warning - two different object instances will never be equal: {x:20} != {x:20}
            return false;
        }
    }
    return true;
}