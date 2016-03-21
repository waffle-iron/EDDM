<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SurveyForm.ascx.vb" Inherits="CLibrary_SurveyForm" %>

<asp:Panel ID="pSurvey" runat="server" DefaultButton="btnSave">

    <asp:PlaceHolder ID="phPreText" runat="server" />

    <asp:ValidationSummary ID="vSumm" runat="server" ValidationGroup="validSurvey" CssClass="alert alert-danger" HeaderText="Please check for the following errors:" />
    
    <asp:PlaceHolder ID="phSurvey" runat="server" />
    
    <asp:PlaceHolder ID="phPostText" runat="server" />
    
    <asp:Literal ID="lLoadMsg" runat="server" />
    
    <asp:Panel ID="pCaptcha" runat="server" Visible="false">
        <div style="float: right;">
            <asp:TextBox ID="reCaptchaValidatorShim" runat="server" Style="display: none;" />
            <asp:RequiredFieldValidator ID="reCaptchaValidator" runat="server" ControlToValidate="reCaptchaValidatorShim" ErrorMessage="You must complete the CAPTCHA to submit this form." Text="(*)" ValidationGroup="validSurvey" EnableClientScript="false" />
        </div>
        <reCaptcha:RecaptchaControl ID="reCaptcha" runat="server" />
    </asp:Panel>

    <asp:Panel ID="pSubmit" runat="server" CssClass="makeButtonPane">

        <asp:LinkButton ID="btnSave" runat="server" Text="Submit" ValidationGroup="validSurvey" OnClick="btnSave_Click" CssClass="btn btn-danger">
            
        </asp:LinkButton>
        
        <appx:SingleClickButton ID="btnSingleClickSave" runat="server" Visible="False" 
            CssClass="btn btn-danger btn-lg" Text="Submit" 
            ValidationGroup="validSurvey" 
            OnClick="btnSave_Click" 
            ClickedText="Processing..." 
            ShowProcessingModal="True" 
            ProcessingModalTitle="Saving Your Response"
            Framework="bootstrap" 
            ProcessingModalHtml='<div class="processingModal"><div class="sectionHeader">Processing</div><br /><br /><br /><br /><h4>Processing Your Custom Quote....</h4><br /><br /><br /><br /><p><img src="/assets/images/loadingbar.gif" height="22" width="126" title="Processing..." alt="Processing..." /></p></div>' 
            />

    </asp:Panel>

</asp:Panel>
