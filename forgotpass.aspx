<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="false" CodeFile="forgotpass.aspx.vb" Inherits="forgotpass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="phForm" Runat="Server">

    <div class="container">

        <div class="partialRibbon hidden-sm hidden-xs">
            <span class="arrowLeft"></span>
            <span class="subRibbonPop">Reminder</span>
            <span class="subRibbon">Forgot your Password?</span>
        </div>

        <div class="partialRibbonSmall visible-sm visible-xs">
            <span class="subRibbonPopSmall">Reminder</span>
            <span class="subRibbonSmall">Forgot your Password?</span>
        </div>

        <div class="contentWrapper">
            
            <p>&nbsp;</p>

            <p>Please enter the email address you used when you created your account, and we will send you an email with further instructions.</p>
            
            <div class="row">

                <div class="col-md-8 col-md-offset-2">

                    <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="false">
                        <span class="glyphicon glyphicon-remove pull-left"></span>&nbsp;
                        <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                    </asp:Panel>

                    <br />

                    <asp:Panel ID="pnlSuccess" runat="server" CssClass="alert alert-success" Visible="false">
                        <span class="glyphicon glyphicon-ok pull-left"></span>&nbsp;
                        <asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>
                    </asp:Panel>

                </div>

                <div class="col-md-8 col-md-offset-2">

                    <div role="form">
                        <div class="form-group">
                            <label for="txtEmail" class="label formLabelRequired">Email address</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                        </div>

                        <%--<asp:Button ID="btnRecover" runat="server" Text="Recover Password" CssClass="btn btn-primary" />--%>
                        <asp:LinkButton ID="btnRecover" runat="server" CssClass="btn btn-danger btn-lg pull-right"><span class="fa fa-wrench"></span>&nbsp;Recover Password</asp:LinkButton>

                    </div>

                </div>

            </div>
            

            <p>&nbsp;</p>
            <p>&nbsp;</p>
            <p>&nbsp;</p>
            <p>&nbsp;</p>

        </div>
    </div>

</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cpScripts" Runat="Server">
</asp:Content>

