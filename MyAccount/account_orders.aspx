<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="false" CodeFile="account_orders.aspx.vb" Inherits="MyAccount_account_orders" %>
<%@ Register Src="~/CLibrary/YourAccountMenu.ascx" TagPrefix="appx" TagName="MemberSideBarRD" %>
<%@ Register Src="~/CCustom/PageHeader.ascx" TagPrefix="appx" TagName="PageHeader" %>



<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="phForm" Runat="Server">

<section id="PageContent">
    <div class="container">

        <appx:PageHeader runat="server" id="PageHeader" />

        <div class="contentWrapper">

            <div class="row">

                <%--Side bar--%>
                <div class="col-sm-3 hidden-xs">
                    <appx:membersidebarrd runat="server" id="MemberSideBarRD" />
                </div>

                <%--Main form--%>
                <div class="col-sm-9 col-xs-12">

                    <%--Error--%>
                    <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger">
                        <p>&nbsp;</p>
                        <span class="fa fa-2x fa-exclamation-circle text-danger"></span>&nbsp;
                        <asp:Literal ID="litErrorMessage" runat="server" />
                        <p>&nbsp;</p>
                        <p>&nbsp;</p>
                    </asp:Panel>

                    <%--No Orders--%>
                    <asp:Panel ID="pnlNoOrders" runat="server" Visible="false">
                        <p class="lead">There are no orders for your account.</p>
                    </asp:Panel>


                    <%--For all accounts except Staples --%>
                    <asp:ListView ID="lvOrders" runat="server" ItemPlaceholderID="phItemTemplate" Visible="False">

                        <LayoutTemplate>
                            <table class="table table-condensed table-striped table-bordered table-hover">
                                <thead>

                                <tr class="tableHeaderRow">
                                    <th class="col-sm-3 text-center">Order #<br />Click for Receipt</th>
                                    <th class="col-sm-3 text-center">Order Date</th>
                                    <th class="col-sm-3 text-center">Click To Reorder</th>
                                    <th class="col-sm-3 text-center">Print Qty</th>
                                </tr>

                                </thead>
                        
                                <tbody>
                                    <asp:PlaceHolder runat="server" ID="phItemTemplate" />
                                </tbody>

                            </table>
                        </LayoutTemplate>

                        <ItemTemplate>
                            <tr>
                            <td class="col-sm-3 text-center"><asp:HyperLink ID="hypReceipt" runat="server" Text='<%#Eval("OrderID") %>' Target="_blank" /></td>
                            <td class="col-sm-3 text-center"><asp:Literal ID="OrderDate" runat="server" Text='<%#Eval("Created", "{0:MM/dd/yyyy}") %>' /></td>
                            <td class="col-sm-3 text-center"><asp:HyperLink ID="hypMapName" runat="server"/></td>
                            <td class="col-sm-3 text-center"><asp:Literal ID="PrintQty" runat="server" /></td>
                            </tr>
                        </ItemTemplate>

                    </asp:ListView>


                    <%--For Staples b/c the use PO and 'Rewards Number'--%>
                    <asp:ListView ID="lvOrders2" runat="server" ItemPlaceholderID="phItemTemplate" Visible="False">

                        <LayoutTemplate>
                            <table class="table table-condensed table-striped table-bordered table-hover">
                                <thead>

                                <tr class="tableHeaderRow">
                                    <th class="col-sm-2 text-center">Order #<br />Click for Receipt</th>
                                    <th class="col-sm-2 text-center">Order Date</th>
                                    <th class="col-sm-3 text-center">Rewards Number</th>
                                    <th class="col-sm-3 text-center">Map Name<br />Click To Reorder</th>
                                    <th class="col-sm-2 text-center">EDDM<br />Print Qty</th>
                                </tr>

                                </thead>
                        
                                <tbody>
                                <asp:PlaceHolder runat="server" ID="phItemTemplate" />
                                </tbody>

                            </table>
                        </LayoutTemplate>

                        <ItemTemplate>
                            <tr>
                            <td><asp:HyperLink ID="hypReceipt" runat="server" Text='<%#Eval("OrderID") %>' Target="_blank" /></td>
                            <td><asp:Literal ID="OrderDate" runat="server" Text='<%#Eval("Created", "{0:MM/dd/yyyy}") %>' /></td>
                            <td><asp:Literal ID="RewardsNumber" runat="server"/></td>
                            <td><asp:HyperLink ID="hypMapName" runat="server"/></td>
                            <td><asp:Literal ID="PrintQty" runat="server" /></td>
                            </tr>
                        </ItemTemplate>

                    </asp:ListView>


                     <%--For Locked Routes--%>
                    <asp:ListView ID="lvOrdersLocked" runat="server" ItemPlaceholderID="phItemTemplate" Visible="False">

                        <LayoutTemplate>
                            <table class="table table-condensed table-striped table-bordered table-hover">
                                <thead>

                                <tr class="tableHeaderRow">
                                    <th class="col-sm-2 text-center">Order #<br />Click for Receipt</th>
                                    <th class="col-sm-2 text-center">Order Date</th>
                                    <th class="col-sm-3 text-center">Lock Expires</th>
                                    <th class="col-sm-3 text-center">Map Name<br />Click To Reorder</th>
                                    <th class="col-sm-2 text-center">EDDM<br />Print Qty</th>
                                </tr>

                                </thead>
                        
                                <tbody>
                                <asp:PlaceHolder runat="server" ID="phItemTemplate" />
                                </tbody>

                            </table>
                        </LayoutTemplate>

                        <ItemTemplate>
                            <tr>
                            <td class="col-sm-2 text-center"><asp:HyperLink ID="hypReceipt" runat="server" Text='<%#Eval("OrderID") %>' Target="_blank" /></td>
                            <td class="col-sm-2 text-center"><asp:Literal ID="OrderDate" runat="server" Text='<%#Eval("Created", "{0:MM/dd/yyyy}") %>' /></td>
                            <td class="col-sm-2 text-center"><asp:Literal ID="litLockExpires" runat="server"/></td>
                            <td class="col-sm-2 text-center"><asp:HyperLink ID="hypMapName" runat="server"/></td>
                            <td class="col-sm-2 text-center"><asp:Literal ID="PrintQty" runat="server" /></td>
                            </tr>
                        </ItemTemplate>

                    </asp:ListView>


                </div>
            </div>

        </div>

    </div>
</section>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cpScripts" Runat="Server">
</asp:Content>

