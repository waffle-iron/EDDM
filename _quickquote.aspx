<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="false" CodeFile="_quickquote.aspx.vb" Inherits="_quickquote" %>

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

        <div class="roundedWrapper">
            
            <asp:Literal ID="lEstimate" runat="server" />

            <div class="row">
                <div class="col-sm-4">
                    <h5 class="text-right">Product</h5>
                </div>
                <div class="col-sm-4">
                    <asp:RadioButtonList ID="radProduct" runat="server" RepeatColumns="1" RepeatDirection="Vertical" CssClass="paddedRadioButtonList" />
                </div>
                <div class="col-sm-4">
                    <asp:LinkButton ID="lnkGetEstimate" runat="server" cssclass="btn btn-danger">
                        Get Estimate
                    </asp:LinkButton>
                </div>
            </div>

        </div> 

    </div>


</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="phFoot" runat="Server">
</asp:Content>
