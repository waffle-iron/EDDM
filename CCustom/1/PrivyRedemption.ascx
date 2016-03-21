<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PrivyRedemption.ascx.cs" Inherits="CCustom_PrivyRedemption" %>


<!-- BEGIN PRIVY REDEMPTION CODE -->
<script>
(function(win, doc, scr, url, obj, tag, el) {
win[obj] = win[obj] || function() {(win[obj].q = win[obj].q || []).push(arguments)};
tag = doc.createElement(scr), el = doc.getElementsByTagName(scr)[0];
tag.async = 1;tag.src = document.location.protocol+url;el.parentNode.insertBefore(tag, el)
})(window, document, 'script', '//widget.privy.com/assets/online_redemption.js', '$PrivyORT');
$PrivyORT('init', 'privy.com');
$PrivyORT('redeemOnlineOffer', {'offer_id':10335});
</script>
<!-- END PRIVY REDEMPTION CODE -->
