<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="false" CodeFile="Step1-Target.aspx.vb" Inherits="Step1_Target" %>
<%@ Register Src="~/CCustom/EDDMTargetMap.ascx" TagPrefix="appx" TagName="EDDMTargetMap" %>

<asp:Content ID="Content2" ContentPlaceHolderID="phBody" Runat="Server">

    <div class="container-fluid">
        <appx:EDDMTargetMap runat="server" id="EDDMTargetMap" />
    </div>

</asp:Content>



<asp:Content ID="Content3" ContentPlaceHolderID="cpScripts" runat="Server">

    <script type="text/javascript">
        $('li.active').removeAttr('class'); 
        $("li:contains('Target')").addClass("active")
    </script>

</asp:Content>
