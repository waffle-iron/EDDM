<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DevWarningBanner.ascx.cs" Inherits="DevWarningBanner" %>

<asp:Panel ID="pnlDevBanner" runat="server" Visible="False">

    <div id="devBannerWrapper">
        <div id="devBannerText">DEVELOPMENT SITE</div>
    </div>

    <script type="text/javascript">
        $('#devBannerWrapper').fadeOut(1000);
        //$('#devBannerWrapper').slideUp(3000);
    </script>

</asp:Panel>