<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/OneColumn.master" AutoEventWireup="false" CodeFile="vpage_template.aspx.vb" Inherits="vpage_template" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="phHead" runat="Server">
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="phNav" runat="Server">
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="phBody" runat="Server">
    
    <div class="container">

        <div class="fullRibbon hidden-sm hidden-xs">
            <span class="arrowLeft"></span>
            Templates
            <span class="arrowRight"></span>
        </div>        

        <div class="fullRibbonSmall visible-sm visible-xs">
            Templates
        </div>        

        <div class="contentWrapper">
           
            <div class="row">

                <%--Front / Inside--%>
                <div class="col-sm-3">
                    <asp:HyperLink ID="hplFront" runat="server">
                        <asp:Image ID="imgFront" runat="server" CssClass="img-thumbnail img-responsive" />
                    </asp:HyperLink>

                    <asp:HyperLink ID="hplInside" runat="server">
                        <asp:Image ID="imgInside" runat="server" CssClass="img-thumbnail img-responsive" />
                    </asp:HyperLink>

                </div>

                <%--Back--%>
                <div class="col-sm-3">
                    <asp:HyperLink ID="hplBack" runat="server">
                        <asp:Image ID="imgBack" runat="server" CssClass="img-thumbnail img-responsive" />
                    </asp:HyperLink>
                </div>

                <%--Template Details--%>
                <div class="col-sm-6">
                    
                    <h5>Template #<asp:Literal ID="lTemplateIdTop" runat="server" /></h5>

                    <div class="row">
                        <div class="col-xs-5 text-right"><strong>Category:</strong></div>
                        <div class="col-xs-7"><asp:Literal ID="lIndustryName" runat="server" /></div>
                    </div>

                    <div class="row">
                        <div class="col-xs-5 text-right"><strong>Sub-Category:</strong></div>
                        <div class="col-xs-7"><asp:Literal ID="lBusinessLines" runat="server" /></div>
                    </div>

                    <div class="row">
                        <div class="col-xs-5 text-right"><strong>Template Size:</strong></div>
                        <div class="col-xs-7"><asp:Literal ID="lSize" runat="server" /></div>
                    </div>

                    <div class="row">
                        <div class="col-xs-5 text-right"><strong>Template Id Number:</strong></div>
                        <div class="col-xs-7"><asp:Literal ID="lTemplateId" runat="server" /></div>
                    </div>
                </div>

            </div>

            <div><span class="fa fa-search-plus"></span>&nbsp;Template Previews - Click Image(s) to Enlarge</div>

            <%--Template info...--%>
            <div class="row">
                <div class="col-sm-6 col-sm-offset-6">

                    <p class="lead">Select this template to continue with your order, or go back to view more template options.</p>

                    <asp:Panel ID="pnlTaradelContent" runat="server" Visible="False">
                        <p>This template qualifies for the Every Door Direct Mail&trade; program.</p>
                        <p>All template orders receive FREE text customization and logo placement by a Taradel graphic design professional - at no additional charge.</p>
                    </asp:Panel>

                    <asp:Panel ID="pnlGenericContent" runat="server" Visible="False">
                        <p>All template orders receive FREE text customization and logo placement by a graphic design professional - at no additional charge.</p>
                    </asp:Panel>

                    <p>&nbsp;</p>

                    <div class="row">
                        <div class="col-sm-6">
                            
                            <%--Commented out 11/2/15.  Possibly obsolete.--%>
                            <%--<asp:HyperLink ID="hplReturn" runat="server" Text="&laquo; Back to Templates" />--%>

                            <a href="#" class="btn btn-danger" onclick="history.go(-1);return false;">
                                <span class="fa fa-chevron-left"></span>&nbsp;Back To Templates
                            </a>

                        </div>
                        <div class="col-sm-6">
                            <asp:LinkButton ID="lnkSelect" runat="server" Tooltip="" CssClass="btn btn-danger">
                                Select and Continue&nbsp;<span class="fa fa-chevron-right"></span>
                            </asp:LinkButton>
                        </div>
                    </div>

                    <p>&nbsp;</p>


                </div>
            </div>

        </div>

    </div>

</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="phForm" runat="Server">
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="phFoot" runat="Server">
</asp:Content>
