<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BingConversionCode.ascx.vb" Inherits="CCustom_BingConversionCode" %>



<asp:Panel ID="pnlBingCode" runat="server" Visible="false">

    <!-- Bing Conversion Code -->
    <script type="text/javascript">
        if (!window.mstag) mstag = { loadTag: function () { }, time: (new Date()).getTime() };
    </script>

    <script id="mstag_tops" type="text/javascript" src="//flex.msn.com/mstag/site/70284dd6-17b4-4493-b514-94072ce299d0/mstag.js"></script>

    <script type="text/javascript">
        mstag.loadTag("analytics", { dedup: "1", domainId: "289704", type: "1", actionid: "153847" })
    </script>

    <noscript>
        <iframe src="//flex.msn.com/mstag/tag/70284dd6-17b4-4493-b514-94072ce299d0/analytics.html?dedup=1&domainId=289704&type=1&actionid=153847" frameborder="0" scrolling="no" width="1" height="1" style="visibility: hidden; display: none"></iframe>
    </noscript>

</asp:Panel>
