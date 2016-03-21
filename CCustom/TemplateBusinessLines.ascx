<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TemplateBusinessLines.ascx.vb" Inherits="CCustom_TemplateBusinessLines" %>

<div class="panel panel-primary">

    <div class="panel-heading">
        <div class="panel-title">
            <strong><asp:Literal ID="lIntroText" runat="server" Text="Business" /></strong>&nbsp;
            <asp:Literal ID="lFullText" runat="server" Text="Type" />
        </div>
    </div>

    <div class="panel-body">

        <div class="form-group">

            <asp:ListView ID="lvBusinessLines" runat="server" ItemPlaceholderID="phItemTemplate">

                <LayoutTemplate>
                    <div class="leftNavPrimary">
                        <ul>
                            <asp:PlaceHolder ID="phItemTemplate" runat="server" />
                        </ul>
                    </div>
                </LayoutTemplate>

                <ItemTemplate>
                    <li><asp:HyperLink ID="hplBusinessLine" runat="server" Text='<%#Eval("Name") %>' /></li>
                </ItemTemplate>

            </asp:ListView>

        </div>

    </div>

</div>  

