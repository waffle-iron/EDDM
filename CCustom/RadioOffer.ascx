<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RadioOffer.ascx.vb" Inherits="CCustom_RadioOffer" %>
<%@ Register TagPrefix="clib" TagName="SurveyForm" Src="~/CLibrary/SurveyForm.ascx" %>
<asp:LinkButton ID="lnkRadioOffer" runat="server" CausesValidation="false">
    <asp:Image ID="imgRadio" runat="server" /></asp:LinkButton>

<asp:Panel ID="pRadioOffer" runat="server" style="display:none;">
    <clib:SurveyForm ID="RadioSurvey" runat="server" InDialog="true" />
</asp:Panel>