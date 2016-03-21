jQuery(window).load(function ()
{

    if (jQuery('.row.equal .panel-body').length > 1)
    {

        var aRow = new Array();

        jQuery('.row.equal .panel-body').each(function ()
        {
            var elOffset = jQuery(this).offset();
            if (typeof (aRow[elOffset.top]) != 'undefined')
            {
                if (jQuery(this).height() > aRow[elOffset.top]) aRow[elOffset.top] = jQuery(this).height();
            }
            else
            {
                aRow[elOffset.top] = jQuery(this).height();
            }
        });

        jQuery('.row.equal .panel-body').each(function ()
        {
            var elOffSet = jQuery(this).offset();
            if (typeof (aRow[elOffSet.top]) != 'undefined')
            {
                jQuery(this).css('height', (aRow[elOffSet.top]+50) + 'px').addClass('processed').closest('.row.equal').addClass('processed');
            }
        });
    }



    if (jQuery('.row.equal .stretchRow').length > 1)
    {

        var aRow = new Array();

        jQuery('.row.equal .stretchRow').each(function () {
            var elOffset = jQuery(this).offset();
            if (typeof (aRow[elOffset.top]) != 'undefined') {
                if (jQuery(this).height() > aRow[elOffset.top]) aRow[elOffset.top] = jQuery(this).height();
            }
            else {
                aRow[elOffset.top] = jQuery(this).height();
            }
        });

        jQuery('.row.equal .stretchRow').each(function () {
            var elOffSet = jQuery(this).offset();
            if (typeof (aRow[elOffSet.top]) != 'undefined') {
                jQuery(this).css('height', (aRow[elOffSet.top] + 50) + 'px').addClass('processed').closest('.row.equal').addClass('processed');
            }
        });
    }



});