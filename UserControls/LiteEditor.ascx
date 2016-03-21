<%@ Control Language="VB" AutoEventWireup="false" CodeFile="LiteEditor.ascx.vb" Inherits="usercontrols_LiteEditor" %>
<obouted:EditorPopupHolder runat="server" ID="popupHolder" DefaultAddPolicy="Demand"
    OnClientPopupStateChanged="popupStateChanged" />
<asp:PlaceHolder ID="phDynControls" runat="server" />
<asp:UpdatePanel id="pEd" runat="server">
    <ContentTemplate>
        <obouted:Editor ID="oEd" runat="server" Height="400" PopupHolderID="popupHolder">
            <TopToolbar ID="Top" Appearance="Lite" runat="server">
                <AddButtons>
                    <obouted:InsertLink />
                    <obouted:InsertImage />
                </AddButtons>
            </TopToolbar>
        </obouted:Editor>    
    </ContentTemplate>
</asp:UpdatePanel>
