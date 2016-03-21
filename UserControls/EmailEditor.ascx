<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EmailEditor.ascx.vb"
    Inherits="usercontrols_EmailEditor" %>
<asp:PlaceHolder ID="phDynControls" runat="server" />
<obouted:EditorPopupHolder runat="server" ID="popupHolder" DefaultAddPolicy="Demand"
    OnClientPopupStateChanged="popupStateChanged" />
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
                    <obouted:ForeColorClear />
                    <obouted:BackColorGroup />
                    <obouted:BackColorClear />
                    <obouted:HorizontalSeparator />
                    <obouted:RemoveStyles />
                    <obouted:HorizontalSeparator />
                    <obouted:FontName />
                    <obouted:HorizontalSeparator />
                    <obouted:FontSize />
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
                    <obouted:ImportDocument />
                    <obouted:HorizontalSeparator />
                    <obouted:Paragraph />
                    <obouted:JustifyLeft />
                    <obouted:JustifyCenter />
                    <obouted:JustifyRight />
                    <obouted:JustifyFull />
                    <obouted:RemoveAlignment />
                    <obouted:HorizontalSeparator />
                    <obouted:OrderedList />
                    <obouted:BulletedList />
                    <obouted:HorizontalSeparator />
                    <obouted:IncreaseIndent />
                    <obouted:DecreaseIndent />
                    <%--New Row--%>
                    <obouted:VerticalSeparator />
                    <obouted:InsertLink />
                    <obouted:InsertTable />
                    <obouted:InsertDiv />
                    <obouted:InsertHR />
                    <obouted:InsertSpecialCharacter />
                    <obouted:InsertImage />
                    <obouted:InsertFlash />
                    <obouted:InsertMedia />
                    <obouted:HorizontalSeparator />
                    <obouted:ToLowerCase />
                    <obouted:ToUpperCase />
                    <obouted:HorizontalSeparator />
                    <obouted:GetFormat />
                    <obouted:ApplyFormat />
                    <obouted:HorizontalSeparator />
                    <obouted:InsertAnchor />
                    <obouted:AnchorsToggle />
                    <obouted:HorizontalSeparator />
                    <obouted:SelectAll />
                    <obouted:SelectNone />
                    <obouted:HorizontalSeparator />
                    <obouted:Print />
                    <obouted:HorizontalSeparator />
                    <obouted:SpellCheck />
                    <obouted:HorizontalSeparator />
                    <obouted:ContextMenuButton />
                </AddButtons>
            </TopToolbar>
        </obouted:Editor>
    </ContentTemplate>
</asp:UpdatePanel>
