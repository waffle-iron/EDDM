<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GetStartedWithAddress.ascx.cs" Inherits="CCustom_GetStartedWithAddress" %>


<h2 class="headerUnderlined">Try the EDDM Targeting Tool now</h2>

<p>Enter an address and ZIP Code you want to mail around.</p>

<asp:ValidationSummary ID="vsGetStarted" runat="server" ValidationGroup="vgGetStarted" CssClass="alert alert-danger" />
                      
<div role="form">
    <div class="form-group">
        <label for="StreetAddress" class="label formLabel formLabelRequired">Street Address</label>
        <div class="row">
            <div class="col-sm-11">
                <asp:TextBox ID="StreetAddress" runat="server" CssClass="form-control" MaxLength="100" />
            </div>
            <div class="col-sm-1">
                    <asp:RequiredFieldValidator ID="rfvStreetAddress" runat="server" ControlToValidate="StreetAddress" ErrorMessage="Please enter the street address." ValidationGroup="vgGetStarted" CssClass="text-danger" Display="Dynamic">
                    <span class="fa fa-2x fa-exclamation-circle"></span>
                </asp:RequiredFieldValidator>
            </div>
        </div>
    </div>

    <div class="form-group">
        <label for="ZipCode" class="label formLabel formLabelRequired">Zip Code</label>
        <div class="row">
            <div class="col-sm-11">
                <asp:TextBox ID="ZipCode" runat="server" CssClass="form-control" MaxLength="5" />
            </div>
            <div class="col-sm-1">
                <asp:RequiredFieldValidator ID="rfvZipCode" runat="server" ControlToValidate="ZipCode" ErrorMessage="Please enter the zip code." ValidationGroup="vgGetStarted" CssClass="text-danger" Display="Dynamic">
                    <span class="fa fa-2x fa-exclamation-circle"></span>
                </asp:RequiredFieldValidator>
            </div>
        </div>
    </div>

    <p class="text-right"><asp:LinkButton ID="lnkGetStarted" runat="server" ValidationGroup="vgGetStarted" CssClass="btn btn-cta btn-shadow btn-lg lrgActionButton" OnClick="lnkGetStarted_Click">
        <span class="fa fa-check"></span>&nbsp;Get Started
    </asp:LinkButton></p>

</div>
    
<div>&nbsp;</div>

