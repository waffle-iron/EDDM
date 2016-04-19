<%@ Page Title="Select a pre-made template" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="false" CodeFile="CustomTemplates.aspx.vb" Inherits="CustomTemplates" %>
<%@ Register TagPrefix="template" TagName="Search" Src="~/CCustom/TemplateSearch.ascx" %>
<%@ Register TagPrefix="template" TagName="PagedList" Src="~/CCustom/TemplatePagedList.ascx" %>
<%@ Register Src="~/CCustom/PageHeader.ascx" TagPrefix="template" TagName="PageHeader" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="phHead" runat="Server">
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="phNav" runat="Server">
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="phBody" runat="Server">
</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="phForm" runat="Server">

    <div class="container">

        <template:PageHeader runat="server" id="PageHeader" />


        <div class="contentWrapper">
            <asp:PlaceHolder runat="server" ID="phAuth" Visible="False">
                <div class="center">
                    <p>Sign in to your account to view templates</p>
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder runat="server" ID="phTemplates" Visible="False">
                <h2 class="text-center">All the templates shown below are easily customized just for you!</h2>

                <div>&nbsp;</div>

                <div class="row">
                    <div class="col-sm-3">
                        <div>&nbsp;</div>
                        <div>&nbsp;</div>
                        <template:Search ID="Search" runat="server" SearchUrl="~/CustomTemplateSearch.aspx" />
                        <div>&nbsp;</div>
                        <div>&nbsp;</div>
                        <asp:Literal ID="litCustomMessage" runat="server" />
                    </div>
                    
                    <div class="col-sm-9">

                        <template:PagedList ID="PagedList" runat="server" ShowName="True" />

                    </div>

                </div>
            </asp:PlaceHolder>
        </div>

    </div>

</asp:Content>


<asp:Content ID="Content6" ContentPlaceHolderID="phFoot" runat="Server">
</asp:Content>



<asp:Content ID="Content7" ContentPlaceHolderID="cpScripts" runat="Server">

    <script type="text/javascript">
        $('li.active').removeAttr('class'); 
        $("li:contains('Design')").addClass("active")
    </script>

</asp:Content>
