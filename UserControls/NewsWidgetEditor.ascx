<%@ Control Language="VB" AutoEventWireup="false" CodeFile="NewsWidgetEditor.ascx.vb" Inherits="usercontrols_NewsWidgetEditor" %>
<obouted:EditorPopupHolder runat="server" ID="popupHolder" DefaultAddPolicy="Demand"
    OnClientPopupStateChanged="popupStateChanged" />
<asp:PlaceHolder ID="phDynControls" runat="server" />
<asp:UpdatePanel ID="pEd" runat="server">
    <ContentTemplate>
        <obouted:Editor ID="oEd" runat="server" Height="400" PopupHolderID="popupHolder">
            <TopToolbar ID="Top" Appearance="Custom" runat="server">
                <AddButtons>
                    <obouted:Bold />
                    <obouted:Italic />
                    <obouted:Underline />
                    <obouted:StrikeThrough />
                    <obouted:SubScript />
                    <obouted:SuperScript />
                    <obouted:HorizontalSeparator />
                    <obouted:ForeColorGroup ID="ForeColorGroup1" runat="server" />
                    <obouted:BackColorGroup />
                    <obouted:HorizontalSeparator />
                    <obouted:RemoveStyles />
                    <obouted:HorizontalSeparator />
                    <obouted:Header />
                    <%--New Row--%>
                    <obouted:VerticalSeparator />
                    <obouted:Undo />
                    <obouted:Redo />
                    <obouted:HorizontalSeparator />
                    <obouted:Cut />
                    <obouted:Copy />
                    <obouted:Paste />
                    <obouted:PasteText />
                    <obouted:PasteWord />
                    <obouted:HorizontalSeparator />
                    <obouted:Paragraph />
                    <obouted:OrderedList />
                    <obouted:BulletedList />
                    <obouted:InsertTable />
                    <obouted:InsertDiv />
                    <obouted:HorizontalSeparator />
                    <obouted:InsertSpecialCharacter />
                    <obouted:InsertLink />
                    <obouted:InsertImage />
                    <obouted:InsertFlash />
                    <obouted:InsertMedia />
                    <%--<obouted:HorizontalSeparator />
                    <obouted:SpellCheck />--%>
                </AddButtons>
            </TopToolbar>
        </obouted:Editor>
    </ContentTemplate>
</asp:UpdatePanel>
