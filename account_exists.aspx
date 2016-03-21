<%@ Page Language="VB" MasterPageFile="~/app_masterpages/Site.master" AutoEventWireup="false" CodeFile="account_exists.aspx.vb" Inherits="account_exists" Title="Untitled Page" %>

<asp:Content ID="Content2" ContentPlaceHolderID="phBody" runat="Server">

    <div class="container">

        <div class="partialRibbon hidden-sm hidden-xs">
            <span class="arrowLeft"></span>
            <span class="subRibbonPop">Registration</span>
            <span class="subRibbon">Account Already Exists</span>
        </div>

        <div class="partialRibbonSmall visible-sm visible-xs">
            <span class="subRibbonPopSmall">Registration</span>
            <span class="subRibbonSmall">Account Already Exists</span>
        </div>

        <div class="contentWrapper">
            
            <p><asp:Literal ID="lMsg" runat="server" />&nbsp;</p>

            <p>The account you have tried to create already exists.</p>
            
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
                            <label for="EmailAddress" class="label formLabelRequired">Email address</label>
                            <asp:TextBox ID="EmailAddress" runat="server" CssClass="form-control" />
                        </div>

                        <asp:LinkButton ID="btnRecover" runat="server" CssClass="btn btn-danger btn-lg pull-right">
                            <span class="fa fa-wrench"></span>&nbsp;Recover Password
                        </asp:LinkButton>

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
