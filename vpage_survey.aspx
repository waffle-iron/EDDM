<%@ Page Title="" Language="VB" MasterPageFile="~/app_masterpages/OneColumn.master" AutoEventWireup="false" CodeFile="vpage_survey.aspx.vb" Inherits="vpage_survey" %>

<asp:Content ID="Content2" ContentPlaceHolderID="phHead" runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="phNav" runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="phBody" runat="Server">
    
    <div class="container extraTopPadding">
        <div class="contentWrapper">

            <asp:PlaceHolder ID="phPreText" runat="server" />
    
            <asp:ValidationSummary ID="vSumm" runat="server" ValidationGroup="validSurvey" CssClass="alert alert-danger" />
    
            <asp:PlaceHolder ID="phSurvey" runat="server" />
            
            <asp:PlaceHolder ID="phPostText" runat="server" />

            <asp:Panel class="makeButtonPane" runat="server" ID="pSubmit">
                <div class="pull-right">
                    <appx:SingleClickButton ID="btnSave" runat="server" CssClass="btn btn-danger btn-lg" Text="Submit" ValidationGroup="validSurvey"
                        OnClick="btnSave_Click" ClickedText="Processing..." ShowProcessingModal="True" 
                        ProcessingModalTitle="Saving Your Response" 
                        ProcessingModalHtml='<div class="processingModal"><div class="sectionHeader">Processing</div><br /><br /><br /><br /><h4>Processing your quote....</h4><br /><br /><br /><br /><p><img src="/cmsimages/loadingbar.gif" height="22" width="126"/></p></div>' />
                </div>
            </asp:Panel>

            <asp:Literal ID="lLoadMsg" runat="server" />

            <div>&nbsp;</div>
            <div>&nbsp;</div>
            <div>&nbsp;</div>

        </div>
    </div>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="phFoot" runat="Server">
</asp:Content>
