<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Banner-BrowseTemplates.ascx.cs" Inherits="CCustom_Banner_BrowseTemplates" %>

<div class="bannerAdWrapper hidden-xs">
    <div class="row">

        <div class="col-sm-2 text-center">
            <span class="fa fa-folder-open fa-4x fontAwesomeDrawing"></span>
        </div>

        <div class="col-sm-6">
            <div class="bannerAdHeader text-center">Browse <strong>Free</strong> Templates</div>
        </div>

        <div class="col-sm-4">
            <p class="text-center">
                <asp:HyperLink ID="hypViewTemplates" runat="server" CssClass="btn btn-lg btn-cta btn-shadow" />
            </p>
        </div>

    </div>
</div>
