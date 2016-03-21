<%@ Page Language="VB" MasterPageFile="~/app_masterpages/Member.master" AutoEventWireup="false"
    CodeFile="account_quote.aspx.vb" Inherits="account_quote" Title="My Custom Quote Details"
    EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="phBody" runat="Server">
    <h1>My Custom Quote Details</h1>
    <asp:ObjectDataSource ID="oQuoteItems" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="GetCustomerQuoteWithAddress" TypeName="pndQuoteTableAdapters.QuoteItemTableAdapter">
        <SelectParameters>
            <asp:QueryStringParameter Name="QuoteID" QueryStringField="ID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="oQuoteItemAttributes" runat="server" TypeName="pndQuoteTableAdapters.QuoteItemAttributeTableAdapter"
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetQuoteAttributes">
        <SelectParameters>
            <asp:Parameter Name="QuoteItemID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Panel ID="pError" runat="server" Visible="false">
        <p>
            We're sorry. The requested quote is not available.</p>
    </asp:Panel>
    <asp:Panel ID="pQuote" runat="server">
        <table class="quote" width="100%" runat="server">
            <tr>
                <td>
                    <div class="row">
                        <div class="label">
                            Name:</div>
                        <div class="aright">
                            <asp:Literal ID="Name" runat="server" />
                        </div>
                        <div class="row" style="padding-top: 5px;">
                            <div class="label">
                                Quote Date:</div>
                            <div class="aright">
                                <asp:Literal ID="lQuoteDate" runat="server" /></div>
                        </div>
                        <div class="row" style="padding-top: 5px;">
                            <div class="label">
                                Quote Expires:</div>
                            <div class="aright">
                                <asp:Literal ID="QuoteExpiration" runat="server" />
                            </div>
                        </div>
                        <div class="clear" style="padding-top: 5px;">
                        </div>
                        
                        <asp:ValidationSummary ID="vSummQuote" runat="server" ValidationGroup="AddToCart" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pProductConfig" runat="server">
                        <asp:HiddenField ID="ShippingWeight" runat="server" Value="130" />
                        <asp:HiddenField ID="DesignFee" runat="server" />
                        <asp:HiddenField ID="NoDesignFee" runat="server" Value="0" />
                        <input type="hidden" id="hdnqty" runat="server" />
                        <input type="hidden" id="hdnprice" runat="server" />
                        <input type="hidden" id="hdnink" runat="server" />
                        <input type="hidden" id="hdnfold" runat="server" />
                        <input type="hidden" id="hdnimage" runat="server" />
                    </asp:Panel>
                    <div style="float: left; width: 100%; padding-top: 10px; padding-bottom: 10px;">
                        <asp:ListView ID="lsvQuoteItemEmail" runat="server" DataKeyNames="QuoteItemId" DataSourceID="oQuoteItems"
                            OnItemDataBound="lvQuoteItems_ItemDataBound" ItemPlaceholderID="phItemTemplate">
                            <LayoutTemplate>
                                <table class="cart" border="0" cellpadding="5" cellspacing="0" width="100%">
                                    <tr>
                                        <th width="50%">
                                            Name
                                        </th>
                                        <th width="25%">
                                            Quantity
                                        </th>
                                        <th width="25%">
                                            Printing Price
                                        </th>
                                    </tr>
                                    <asp:PlaceHolder ID="phItemTemplate" runat="server" />
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="background-color: #FFF; border: 1px solid #32AAC3">
                                        <asp:HiddenField ID="hfQuoteItemID" runat="server" Value='<%#Eval("QuoteItemID") %>' />
                                        <asp:Literal ID="lName" runat="server" Text='<%#Eval("Name") %>' />
                                    </td>
                                    <td style="background-color: #FFF; border: 1px solid #32AAC3">
                                        <asp:Literal ID="lQty" runat="server" Text='<%#Integer.Parse(Eval("Quantity")).ToString("N0") %>' />
                                    </td>
                                    <td style="background-color: #FFF; border: 1px solid #32AAC3">
                                        <asp:Label ID="lblPrintingPrice" Visible="false" runat="server" Text='<%# Eval("PrintingPrice") %>'></asp:Label>
                                        <asp:Literal ID="lPrintingPrice" runat="server" Text='<%#Decimal.Parse(Eval("PrintingPrice")).ToString("C") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: #FFF; border-left: 1px dotted #32AAC3;">
                                        &nbsp;
                                    </td>
                                    <td colspan="2" style="background-color: #FFF; border-right: 1px dotted #32AAC3;">
                                        <asp:ListView ID="lvQuoteItemsAttr" runat="server" DataKeyNames="QuoteAttrID" ItemPlaceholderID="phItemTemplate">
                                            <LayoutTemplate>
                                                <table border="0" cellpadding="5" cellspacing="0" width="98%" style="background-color: #ffcccc;">
                                                    <tr>
                                                        <th>
                                                            Attribute
                                                        </th>
                                                    </tr>
                                                    <asp:PlaceHolder ID="phItemTemplate" runat="server" />
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:Literal ID="lAttrName" runat="server" Text='<%#Eval("Name") %>' />
                                                        :
                                                        <asp:Literal ID="lrAttrValue" runat="server" Text='<%#Eval("ItemValue") %>' />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:Literal ID="lAttrName" runat="server" Text='<%#Eval("Name") %>' />
                                                        :
                                                        <asp:Literal ID="lrAttrValue" runat="server" Text='<%#Eval("ItemValue") %>' />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        Ship To:
                                        <%#Eval("ShipToAddress") %>
                                    </td>
                                    <td style="background-color: #FFF; border: 1px solid #32AAC3">
                                        <asp:Label ID="lblShippingPrice" Visible="false" runat="server" Text='<%# Eval("ShippingPrice") %>'></asp:Label>
                                        <asp:Literal ID="lShippingPrice" runat="server" Text='<%#Decimal.Parse(Eval("ShippingPrice")).ToString("C") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: #FFF; border-left: 1px dotted #32AAC3;">
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        Total:
                                    </td>
                                    <td style="background-color: #FFF; border: 1px solid #32AAC3">
                                        <%#(Decimal.Parse(Eval("ShippingPrice")) + Decimal.Parse(Eval("PrintingPrice"))).ToString("C")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: #FFF; border-left: 1px dotted #32AAC3;">
                                        &nbsp;
                                    </td>
                                    <td colspan="2" style="border-right: 1px dotted #32AAC3;">
                                        <div id="artworkMore">
                                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <p align="center">
                                                            Click the &quot;Browse&quot; button and select the file(s) you wish to send.</p>
                                                        <div class="row">
                                                            <div class="label">
                                                                File 1:
                                                            </div>
                                                            <div class="aright">
                                                                <neatUpload:InputFile ID="FrontDesignFile" runat="server" />
                                                            </div>
                                                        </div>
                                                        <asp:Panel ID="pFile2" runat="server" CssClass="row">
                                                            <div class="label">
                                                                File 2:</div>
                                                            <div class="aright">
                                                                <neatUpload:InputFile ID="BackDesignFile" runat="server" />
                                                            </div>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: #FFF; border-left: 1px dotted #32AAC3; border-bottom: 1px dotted #32AAC3">
                                        &nbsp;
                                    </td>
                                    <td colspan="2" style="border-right: 1px dotted #32AAC3; border-bottom: 1px dotted #32AAC3;">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr id="rowValidate" runat="server" style="display: none;">
                                                <td style="color: Red; border: solid 1px red;">
                                                    <table cellspacing="0" border="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <span style="position: relative; top: 3px;">&#160;<b>Please review and correct the following:</b></span>
                                                                <ul>
                                                                    <li>Please provide a unique job name that you can use to refer to this quote item.</li></ul>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 5px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <b>Please enter a Job Name below.</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="JobName" runat="server" Columns="15" MaxLength="100" Width="80%" />
                                                    <asp:RequiredFieldValidator ID="rfvJobName" runat="server" ControlToValidate="JobName"
                                                        ErrorMessage="Please provide a unique job name that you can use to refer to this print order."
                                                        Text=" *" ValidationGroup="AddToCart" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <b>Comments</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="JobComments" runat="server" Columns="15" Rows="5" Width="80%" TextMode="MultiLine" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 10px;">
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                    <div style="float: left; width: 100%; padding-top: 10px; padding-bottom: 10px;">
                        <div class="btnalign">
                            <asp:ImageButton ID="btnAddToCart" runat="server" ImageUrl="~/resources/add_to_cart.gif"
                                AlternateText="Add To Cart" Width="168" Height="27" OnClientClick="ShowFileProgressWindow('AddToCart');" /></div>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <upload:Progress ID="ProgressWindow1" runat="server" />
</asp:Content>
