<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="false"
    CodeFile="vpage_product.aspx.vb" Inherits="vpage_product" %>

<%@ Register TagPrefix="taradel" TagName="BannerFlag" Src="~/CCustom/BannerFlag.ascx" %>
<%@ Register TagPrefix="taradel" TagName="SamplePricingTable" Src="~/CCustom/SamplePricingTable.ascx" %>
<%@ Register TagPrefix="taradel" TagName="GetMapping" Src="~/CCustom/GetStartedMappingWithAddress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phHead" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phNav" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phBody" runat="Server">
    <asp:Panel ID="pNoProd" runat="server">
        <h1>
            We're Sorry</h1>
        <p>
            It doesn't look like that product request is valid.</p>
        <p>
            Here are some other products we offer:</p>
        <ul>
            <asp:Repeater ID="rProds" runat="server">
                <ItemTemplate>
                    <li>
                        <asp:HyperLink ID="hplProduct" runat="server" Text='<%#Eval("WLProduct.Name") %>'
                            NavigateUrl='<%#appxCMS.SEO.Rewrite.BuildLink(Eval("WLProduct.Name"), Eval("WLProduct.ProductId"), "p") %>' /></li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </asp:Panel>
    <asp:Panel ID="pProduct" runat="server" CssClass="ui-helper-clearfix" Visible="false">
        <div class="seventy fleft">
            <div class="contentBlock prodView">
                <div class="ui-helper-clearfix">
                    <div style="width: 294px; float: left;">
                        <h1>
                            <asp:Literal ID="lProductName" runat="server" /></h1>
                        <asp:Panel ID="pImage" runat="server" CssClass="center">
                            <asp:Image ID="imgProduct" runat="server" /></asp:Panel>
                        <div class="ui-helper-clearfix description">
                            <asp:Literal ID="lProductDescription" runat="server" />
                        </div>
                    </div>
                    <div style="float: left; width: 294px;">
                        <div class="padleft" style="margin-top: 2.5em;">
                            <h2>
                                Why Order EDDM&trade; Online?</h2>
                            <ul>
                                <li class="first"><span style="font-size: 12pt">No Permit Required</span></li>
                                <li><span style="font-size: 12pt">No Paperwork Required</span></li>
                                <li><span style="font-size: 12pt">FREE Design Templates</span></li>
                                <li><span style="font-size: 12pt">FREE "Point &amp; Click" Mapping Tool</span></li>
                                <li><span style="font-size: 12pt">Easy, Affordable One Stop Shopping</span></li>
                                <li><span style="font-size: 12pt">No Trips to the U.S. Post Office&reg;</span></li>
                                <li><span style="font-size: 12pt">USPS&reg; Postage INCLUDED<br>
                                </span></li>
                                <li class="last"><span style="font-size: 12pt">Guaranteed Accurate Delivery, On Schedule<br>
                                </span></li>
                            </ul>
                            <div>
                                <b>Sample Pricing:</b></div>
                            <taradel:SamplePricingTable ID="SamplePricing" runat="server" />
                        </div>
                    </div>
                </div>
                <div>
                    <div style="font-size: 20pt; font-weight: bold;">
                        Order Every Door Direct Mail&trade; Online, For Less.</div>
                    <taradel:GetMapping ID="NewMap" runat="server" />
                </div>
            </div>
        </div>
        <div class="thirty fleft">
            <div class="padleft">
                <div class="contentBlock">
                    <div class="contentHeader">
                        Ordering Options
                    </div>
                    <ul class="arrowList">
                        <li class="first"><a id="ctl00_phForm_hplNewUSelect" href="Step1-Target.aspx">Start
                            a new U-Select Project</a></li>
                        <li><a id="ctl00_phForm_hplExistingProject" href="myaccount/account_selects.aspx">Open
                            an existing U-Select Project</a></li>
                        <li class="last"><a id="ctl00_phForm_hplTemplates" href="Templates">Explore our Template
                            Library</a></li>
                    </ul>
                    <p class="center padtop">
                        <img src="/cmsimages/1/satisfaction-seal.png" />
                    </p>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="phForm" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="phFoot" runat="Server">
</asp:Content>
