<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Banner-3rdPartyTemplates.ascx.cs" Inherits="ThirdPartyTemplates" %>

<div class="bannerAdWrapper hidden-xs">
    <div class="row">

        <div class="col-sm-2">
            <span class="fa fa-paint-brush fa-4x fontAwesomeDrawing"></span>
        </div>

        <div class="col-sm-6">
            <div class="bannerAdHeader text-center">Access <asp:Literal ID="litBrand" runat="server" /> Design Services</div>
        </div>

        <div class="col-sm-4">
            <p class="text-center">
                <asp:HyperLink ID="hypViewTemplates" runat="server" CssClass="btn btn-lg btn-cta btn-shadow" Target="_blank" />
            </p>
        </div>

    </div>
</div>
