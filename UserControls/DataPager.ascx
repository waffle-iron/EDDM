<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DataPager.ascx.vb" Inherits="UserControls_DataPager" %>

<asp:Panel ID="pDataPager" runat="server">

    <div class="row">

        <div class="col-sm-7">
            <h5 class="text-center"><asp:Literal ID="lDescription" runat="server" Visible="false" /></h5>
        </div>

        <div class="col-sm-5">

            <ul class="pagination pagination-sm">
                <li runat="server" id="liFirst">
                    <asp:HyperLink ID="hplMoveFirst" runat="server">
                        <span class="glyphicon glyphicon-fast-backward"></span>
                    </asp:HyperLink>
                </li>
                <li runat="server" id="liPrev">
                    <asp:HyperLink ID="hplMovePrevious" runat="server">
                        <span class="glyphicon glyphicon-backward"></span>
                    </asp:HyperLink>
                </li>

                <asp:Repeater ID="rPages" runat="server">
                    <ItemTemplate>
                        <li>
                            <asp:HyperLink ID="hplTextNav" runat="server" NavigateUrl='<%#Eval("Value") %>' Text='<%#Eval("Key") %>' CssClass='<%#Iif(Eval("Key").ToString()=Me.CurPage.ToString, "active", "") %>' />
                        </li>
                    </ItemTemplate>
                </asp:Repeater>

                <li runat="server"  id="liNext">
                    <asp:HyperLink ID="hplMoveNext" runat="server" CssClass="makeMoveNextButtonIcon">
                        <span class="glyphicon glyphicon-forward"></span>
                    </asp:HyperLink>
                </li>
                <li runat="server" id="liLast">
                    <asp:HyperLink ID="hplMoveLast" runat="server" CssClass="makeMoveLastButtonIcon">
                        <span class="glyphicon glyphicon-fast-forward"></span>
                    </asp:HyperLink>
                </li>
            </ul>

        </div>

    </div>


</asp:Panel>

