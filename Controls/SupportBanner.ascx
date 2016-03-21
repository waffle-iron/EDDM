<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SupportBanner.ascx.cs" Inherits="CLibrary_SupportBanner" %>
<%@ Register Src="~/CCustom/BoldChatTextLink.ascx" TagPrefix="appx" TagName="BoldChatTextLink" %>


<section id="SupportBanner">

    <div id="supportBannerBlock" class="supportBanner hidden-xs">
        <div class="container">

            <div class="row">
                
                <asp:Panel ID="pnlWithChat" runat="server" Visible="false">

                    <div class="col-sm-3">
                        <p class="text-center">ORDER<br />24/7</p>
                    </div>

                    <div class="col-sm-3">
                        <p class="text-center">Call Us Toll Free<br /><span class="fa fa-phone"></span>&nbsp;<asp:Literal ID="litPhone" runat="server" /></p>
                    </div>

                    <div class="col-sm-3">
                        <p class="text-center">Live Chat<br />
                        <appx:BoldChatTextLink runat="server" id="BoldChatText" /></p>
                    </div>

                    <div class="col-sm-3">
                        <p class="text-center"><asp:Literal ID="litSupport" runat="server" /><br />
                        <span class="fa fa-question-circle"></span>&nbsp;<asp:HyperLink ID="hypSupport" runat="server" ToolTip="Get Help!" /></p>
                    </div>

                </asp:Panel>

                <asp:Panel ID="pnlWithoutChat" runat="server" Visible="false">

                    <div class="col-sm-4">
                        <p class="text-center">ORDER<br />24/7</p>
                    </div>

                    <div class="col-sm-4">
                        <p class="text-center">Call Us Toll Free<br /><span class="fa fa-phone"></span>&nbsp;<asp:Literal ID="litPhone2" runat="server" /></p>
                    </div>

                    <div class="col-sm-4">
                        <p class="text-center">FAQ<br />
                        <span class="fa fa-question-circle"></span>&nbsp;<a href="/Support" title="Get Help!">Get Answers Here</a></p>
                    </div>

                </asp:Panel>
         
                <asp:Panel ID="pnlWithoutChatFaq" runat="server" Visible="false">

                    <div class="col-sm-6">
                        <p class="text-center">ORDER<br />24/7</p>
                    </div>

                    <div class="col-sm-6">
                        <p class="text-center">Call Us Toll Free<br /><span class="fa fa-phone"></span>&nbsp;<asp:Literal ID="litPhone3" runat="server" /></p>
                    </div>


                </asp:Panel>

            </div>

        </div>
    </div>

</section>
