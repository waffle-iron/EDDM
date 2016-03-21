<%@ Control Language="VB" AutoEventWireup="false" CodeFile="USelectProjectPicker.ascx.vb" Inherits="CLibrary_USelectProjectPicker" %>
<asp:PlaceHolder ID="phUSelect" runat="server">
<asp:LinkButton ID="lnkShowSelections" runat="server" CssClass="makeButton" Text="Saved U-Select Projects" />
<div id="uselectPicker" style="display: none;" title="Use a saved U-Select Project">
    <asp:ListView ID="lvSelects" runat="server" ItemPlaceholderID="phItemTemplate">
        <LayoutTemplate>
            <table class="cart" border="0" cellpadding="5" cellspacing="0" width="100%">
                <thead>
                    <tr>
                        <th>
                            Name
                        </th>
                        <th>
                            Created Date
                        </th>
                        <th>
                            Total Deliveries
                        </th>
                        <th>
                            Delivery Method
                        </th>
                        <th>
                            &nbsp;
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder ID="phItemTemplate" runat="server" />
                </tbody>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%#Eval("Name")%>
                </td>
                <td class="center">
                    <%#DateTime.Parse(Eval("CreatedDate")).ToString("MM/dd/yyyy")%>
                </td>
                <td class="center">
                    <%#Integer.Parse(Eval("TotalDeliveries")).ToString("N0")%>
                </td>
                <td>
                    <%#Eval("USelectMethod.Name")%>
                </td>
                <td>
                    <asp:HyperLink ID="hplUse" runat="server" Text="Choose Project" CssClass="makeButton" />
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <p>
                You do not have any saved USelect projects.</p>
        </EmptyDataTemplate>
    </asp:ListView>
</div>
</asp:PlaceHolder>