<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/admin/SiteManager.master" AutoEventWireup="false" CodeFile="ProductConfiguration.aspx.vb" Inherits="admin_Site_Site0_ProductConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <asp:Literal ID="lMsg" runat="server" />
    <div class="ui-helper-clearfix">
        
        <table width="99%" border="1" align="center" cellspacing="10">

            <tr valign="top">
                <td colspan="2"><p><strong>CHECKOUT OPTIONS</strong></p></td>
                <td colspan="2"><p><strong>DROP OPTIONS</strong></p></td>
            </tr>

            <tr valign="top">
                
                <td width="45%">                    
                    Enable EDDM Order Up-sizing with extra Hold & Deliver copies:
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkEnableUpsizing"/>
                </td>

                <td width="45%">                    
                     Monday Drop Dates
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkMondayDropDates"/>
                </td>

            </tr>

            <tr valign="top" style="background-color: #dddddd;">
                
                <td width="45%">                    
                    Enable Marketing Services Up-Sell:
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkMarketingUpsell"/>
                </td>

                <td width="45%">                    
                     Tuesday Drop Dates
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkTuesdayDropDates"/>
                </td>

            </tr>

            <tr valign="top">
                
                <td width="45%">                    
                    Enable New Mover:
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkNewMover"/>
                </td>

                <td width="45%">                    
                     Wednesday Drop Dates
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkWednesdayDropDates"/>
                </td>

            </tr>

            <tr valign="top" style="background-color: #dddddd;">
                
                <td width="45%">                    
                    Enable Email:
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkEmail"/>
                </td>

                <td width="45%">                    
                     Thursday Drop Dates
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkThursdayDropDates"/>
                </td>

            </tr>



            <tr valign="top">

                <td width="45%">                    
                    Enable AddressedList AddOns:
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkAddressedListAddOns"/>
                </td>

                <td width="45%">                    
                     Friday Drop Dates
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkFridayDropDates"/>
                </td>

            </tr>





            <tr valign="top" style="background-color: #dddddd;">
                
                <td width="45%">                    
                    Enable Multiple Impressions:
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkMultipleImpressions"/>
                </td>

                <td width="45%">                    
                     &nbsp;
                </td>
                
                <td width="5%">
                    &nbsp;
                </td>

            </tr>

            <tr valign="top">
                
                <td width="45%">                    
                    Do Not Charge For Multiple Impressions:
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkMultipleImpressionsNoFee"/>
                </td>

                <td width="45%">                    
                     &nbsp;
                </td>
                
                <td width="5%">
                    &nbsp;
                </td>

            </tr>

            <tr valign="top" style="background-color: #dddddd;">
                
                <td width="45%">                    
                    Use New Receipt Process:
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkNewReceiptProcess"/>
                </td>

                <td width="45%">                    
                     &nbsp;
                </td>
                
                <td width="5%">
                    &nbsp;
                </td>

            </tr>

            <tr valign="top">
                
                <td width="45%">                    
                    Disable State Sales Tax:
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkDisableSalesTax"/>
                </td>

                <td width="45%">                    
                     &nbsp;
                </td>
                
                <td width="5%">
                    &nbsp;
                </td>

            </tr>

            <tr valign="top" style="background-color: #dddddd;">
                
                <td width="45%">                    
                    Disable Free Templates:
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkDisableTemplates"/>
                </td>

                <td width="45%">                    
                     &nbsp;
                </td>
                
                <td width="5%">
                    &nbsp;
                </td>

            </tr>

            <tr valign="top">
                
                <td width="45%">                    
                    Disable Professional Design:
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkDisableProDesign"/>
                </td>

                <td width="45%">                    
                     &nbsp;
                </td>
                
                <td width="5%">
                    &nbsp;
                </td>

            </tr>

            <tr valign="top" style="background-color: #dddddd;">
                
                <td width="45%">                    
                    Disable Upload Artwork:
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkDisableArtwork"/>
                </td>

                <td width="45%">                    
                     &nbsp;
                </td>
                
                <td width="5%">
                    &nbsp;
                </td>

            </tr>


        </table>

        <br /><br />

        <table width="99%" border="1" align="center" cellspacing="10">

            <tr valign="top">
                <td colspan="2"><p><strong>ROUTE OPTIONS</strong></p></td>
                <td colspan="2"><p><strong>&nbsp;</strong></p></td>
            </tr>

            <tr valign="top">
                
                <td width="45%">                    
                    Offers Exclusive Routes:
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkExclusiveRoutes"/>
                </td>

                <td width="45%">                    
                    &nbsp;
                </td>
                
                <td width="5%">
                    &nbsp;
                </td>

            </tr>

            <tr valign="top" style="background-color: #dddddd;">
                
                <td width="45%">                    
                    Uses Exclusive Territories:
                </td>
                
                <td width="5%">
                    <asp:CheckBox runat="server" ID="chkExclusiveTerritories"/>
                </td>

                <td width="45%">                    
                     &nbsp;
                </td>
                
                <td width="5%">
                    &nbsp;
                </td>

            </tr>

        </table>


    </div>

    <div class="makeButtonPane">
        <asp:LinkButton ID="lnkSave" runat="server" Text="Save Product Configuration" />
    </div>

</asp:Content>

