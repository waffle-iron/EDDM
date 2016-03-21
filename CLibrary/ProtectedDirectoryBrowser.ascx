<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ProtectedDirectoryBrowser.ascx.vb"
    Inherits="CLibrary_ProtectedDirectoryBrowser" %>
<asp:Literal ID="lMsg" runat="server" />
<asp:Panel ID="pDirInfo" runat="server">
    <div id="dirAccordion">
        <h3>
            <asp:Literal ID="lFirstPanel" runat="server" /></h3>
        <div>
            <asp:PlaceHolder ID="phCreateDir" runat="server" Visible="false">
                <div id="CreateDir">
                    <p>
                        The directory path specified in this control has not been provisioned.</p>
                    <p>
                        <asp:LinkButton ID="lnkProvision" runat="server" Text="Provision Directory" CssClass="makeButton" /></p>
                </div>
            </asp:PlaceHolder>
            <asp:ListView ID="lvFiles" runat="server" ItemPlaceholderID="phItemTemplate" OnItemDataBound="FileList_ItemDataBound">
                <LayoutTemplate>
                    <ul class="download">
                        <asp:PlaceHolder ID="phItemTemplate" runat="server" />
                    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li>
                        <div>
                            <asp:HyperLink ID="lnkDownloadFile" NavigateUrl="javascript:void(0)" runat="server" CssClass="downloadFile" />
                            (<asp:Literal ID="lFileType" runat="server" />
                            <asp:Literal ID="lFileSize" runat="server" />)
                            <asp:PlaceHolder ID="phManageFile" runat="server" Visible='<%#Me.IsManager %>'>
                                <asp:LinkButton ID="lnkEditMeta" runat="server" CssClass="makeEditButtonIcon editFI"
                                    Text="Edit" />
                                <asp:LinkButton ID="lnkDelete" runat="server" CssClass="makeDeleteButtonIcon deleteFI"
                                    Text="Delete" />
                            </asp:PlaceHolder>
                        </div>
                        <asp:Panel ID="pDesc" runat="server" Style="font-weight: normal;">
                            <asp:Literal ID="lDesc" runat="server" /></asp:Panel>
                    </li>
                </ItemTemplate>
            </asp:ListView>
            <asp:Panel ID="pDirManage" runat="server" Visible="false" CssClass="makeButtonPane">
                <asp:LinkButton ID="lnkOpenCreateDirectory" runat="server" Text="Create Sub-Directory"
                    CssClass="openCD" />
                <asp:LinkButton ID="lnkOpenUploadFile" runat="server" Text="Upload File" CssClass="openUF" />
                <asp:LinkButton ID="lnkOpenCreateLink" runat="server" Text="Create Link" CssClass="openCL" />
            </asp:Panel>
        </div>
        <asp:ListView ID="lvDirectories" runat="server" ItemPlaceholderID="phItemTemplate">
            <LayoutTemplate>
                <asp:PlaceHolder ID="phItemTemplate" runat="server" />
            </LayoutTemplate>
            <ItemTemplate>
                <h3>
                    <asp:Literal ID="lName" runat="server" /></h3>
                <div>
                    <asp:Literal ID="lDescription" runat="server" />
                    <asp:ListView ID="lvFiles" runat="server" ItemPlaceholderID="phItemTemplate" OnItemDataBound="FileList_ItemDataBound">
                        <LayoutTemplate>
                            <ul class="download">
                                <asp:PlaceHolder ID="phItemTemplate" runat="server" />
                            </ul>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li>
                                <div>
                                    <asp:HyperLink ID="lnkDownloadFile" NavigateUrl="javascript:void(0)" runat="server" CssClass="downloadFile" />
                                    (<asp:Literal ID="lFileType" runat="server" />
                                    <asp:Literal ID="lFileSize" runat="server" />)
                                    <asp:PlaceHolder ID="phManageFile" runat="server" Visible='<%#Me.IsManager %>'>
                                        <asp:LinkButton ID="lnkEditMeta" runat="server" CssClass="makeEditButtonIcon editFI"
                                            Text="Edit" />
                                        <asp:LinkButton ID="lnkDelete" runat="server" CssClass="makeDeleteButtonIcon deleteFI"
                                            Text="Delete" />
                                    </asp:PlaceHolder>
                                </div>
                                <asp:Panel ID="pDesc" runat="server" Style="font-weight: normal;">
                                    <asp:Literal ID="lDesc" runat="server" /></asp:Panel>
                            </li>
                        </ItemTemplate>
                    </asp:ListView>
                    <asp:Panel ID="pDirManage" runat="server" Visible='<%#Me.IsManager %>' CssClass="makeButtonPane">
                        <asp:LinkButton ID="lnkOpenDirMeta" runat="server" Text="Directory Info" CssClass="openDI" />
                        <asp:LinkButton ID="lnkDeleteDir" runat="server" Text="Delete Directory" CssClass="openDD" />
                        <asp:LinkButton ID="lnkOpenUploadFile" runat="server" Text="Upload File" CssClass="openUF" />
                        <asp:LinkButton ID="lnkOpenCreateLink" runat="server" Text="Create Link" CssClass="openCL" />
                    </asp:Panel>
                </div>
            </ItemTemplate>
        </asp:ListView>
    </div>
</asp:Panel>
<asp:PlaceHolder ID="phManage" runat="server">
    <div id="CreateSubDirectory" class="ui-hide" title="Create Sub-Directory">
        <asp:ValidationSummary ID="vgCreateSubDir" runat="server" />
        <div>
            <div class="row">
                <div class="label">
                    Name:</div>
                <div class="aright">
                    <asp:TextBox ID="SubDirectoryName" runat="server" Width="80%" /><asp:RequiredFieldValidator
                        ID="rfvSubDirectoryName" runat="server" ControlToValidate="SubDirectoryName"
                        ErrorMessage="A directory name is required." Text="(*)" ValidationGroup="vgCreateSubDir" /></div>
            </div>
            <div class="row">
                <div class="label">
                    Title:</div>
                <div class="aright">
                    <asp:TextBox ID="SubDirectoryTitle" runat="server" Width="80%" />
                    <div class="explainer">
                        Overrides directory name for display, allows you to use special characters such
                        as / and \ in display name.</div>
                </div>
            </div>
            <div class="row">
                <div class="label">
                    Description:</div>
                <div class="aright">
                    <asp:TextBox ID="SubDirectoryDescription" runat="server" Width="80%" Rows="5" TextMode="MultiLine" /></div>
            </div>
            <div class="btnalign">
                <asp:LinkButton ID="lnkCreateSubDirectory" runat="server" Text="Create Directory"
                    ValidationGroup="vbCreateSubDir" CssClass="makeButton" />
            </div>
        </div>
    </div>
    <div id="DirectoryInfo" class="ui-hide" title="Edit Directory Information">
        <asp:HiddenField ID="hfEditDirectoryPath" runat="server" />
        <div class="row">
            <div class="label">
                Title:</div>
            <div class="aright">
                <asp:TextBox ID="EditDirectoryTitle" runat="server" Width="80%" />
                <div class="explainer">
                    Overrides directory name for display, allows you to use special characters such
                    as / and \ in display name.</div>
            </div>
        </div>
        <div class="row">
            <div class="label">
                Description:</div>
            <div class="aright">
                <asp:TextBox ID="EditDirectoryDescription" runat="server" Width="80%" Rows="5" TextMode="MultiLine" /></div>
        </div>
        <div class="btnalign">
            <asp:LinkButton ID="lnkSaveDirectoryInfo" runat="server" Text="Save Changes" CssClass="makeButton" />
        </div>
    </div>
    <div id="DeleteDirectory" class="ui-hide" title="Delete Directory">
        <asp:HiddenField ID="hfDeleteDirectoryPath" runat="server" />
        <p>
            To confirm your action, type the word DELETE (case-sensitive) into the box below
            and click the Delete Directory button.</p>
        <div class="row">
            <div class="label">
                &nbsp;</div>
            <div class="aright">
                <asp:TextBox ID="DeleteConfirm" runat="server" /></div>
        </div>
        <div class="btnalign">
            <asp:LinkButton ID="lnkDirectoryDelete" runat="server" Text="Delete Directory" CssClass="makeButton" />
        </div>
    </div>
    <div id="UploadFile" class="ui-hide" title="Upload File">
        <asp:HiddenField ID="hfNewFilePath" runat="server" />
        <asp:ValidationSummary ID="vgFileUpload" runat="server" />
        <div class="row">
            <div class="label">
                Choose File:</div>
            <div class="aright">
                <neatUpload:InputFile ID="NewFile" runat="server" />
                <div>
                    <asp:CheckBox ID="chkOverwrite" runat="server" Text="Overwrite existing file with same name" /></div>
            </div>
        </div>
        <div class="row">
            <div class="label">
                Title:</div>
            <div class="aright">
                <asp:TextBox ID="FileTitle" runat="server" MaxLength="100" Width="80%" />
                <div class="explainer">
                    The name that will be displayed for this file. If omitted, the name of the file
                    will be used.
                </div>
            </div>
        </div>
        <div class="row">
            <div class="label">
                Description:</div>
            <div class="aright">
                <asp:TextBox ID="FileDescription" runat="server" Rows="5" Width="80%" TextMode="MultiLine" /></div>
        </div>
        <div class="btnalign">
            <asp:LinkButton ID="lnkUploadFile" runat="server" Text="Upload File" CssClass="makeButton" />
        </div>
    </div>
    <div id="CreateLink" class="ui-hide" title="Create URL Link">
        <asp:HiddenField ID="hfCreateLinkPath" runat="server" />
        <asp:ValidationSummary ID="vSummCreateLink" ValidationGroup="vgCreateLink" />
        <div class="row">
            <div class="label">
                URL:</div>
            <div class="aright">
                <asp:TextBox ID="LinkURL" runat="server" MaxLength="100" Width="80%" /><asp:RequiredFieldValidator
                    ID="rfvLinkUrl" runat="server" ControlToValidate="LinkURL" ErrorMessage="You must provide the HTTP URL for this link."
                    Text="(*)" ValidationGroup="vgCreateLink" /></div>
        </div>
        <div class="row">
            <div class="label">
                Title:</div>
            <div class="aright">
                <asp:TextBox ID="LinkTitle" runat="server" MaxLength="100" Width="80%" /></div>
        </div>
        <div class="row">
            <div class="label">
                Description:</div>
            <div class="aright">
                <asp:TextBox ID="LinkDescription" runat="server" Rows="5" Width="80%" TextMode="MultiLine" /></div>
        </div>
        <div class="btnalign">
            <asp:LinkButton ID="lnkSaveLink" runat="server" Text="Save Link" ValidationGroup="vgCreateLink"
                CssClass="makeButton" />
        </div>
    </div>
    <asp:Panel ID="pEditFile" runat="server" ToolTip="Edit File Information">
        <asp:HiddenField ID="hfFilePath" runat="server" />
        <div class="row">
            <div class="label">
                Title:</div>
            <div class="aright">
                <asp:TextBox ID="EditFileTitle" runat="server" Width="80%" /></div>
        </div>
        <div class="row">
            <div class="label">
                Description:</div>
            <div class="aright">
                <asp:TextBox ID="EditFileDescription" runat="server" Width="80%" Rows="5" TextMode="MultiLine" /></div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pEditLink" runat="server" ToolTip="Edit URL Link Information">
        <asp:HiddenField ID="hfEditLinkPath" runat="server" />
        <asp:ValidationSummary ID="vSummEditLink" runat="server" ValidationGroup="vgEditLink" />
        <div class="row">
            <div class="label">
                URL:</div>
            <div class="aright">
                <asp:TextBox ID="EditLinkURL" runat="server" MaxLength="100" Width="80%" /><asp:RequiredFieldValidator
                    ID="rfvEditLinkURL" runat="server" ControlToValidate="EditLinkURL" ErrorMessage="You must provide the HTTP URL for this link."
                    Text="(*)" ValidationGroup="vgEditLink" /></div>
        </div>
        <div class="row">
            <div class="label">
                Title:</div>
            <div class="aright">
                <asp:TextBox ID="EditLinkTitle" runat="server" Width="80%" /></div>
        </div>
        <div class="row">
            <div class="label">
                Description:</div>
            <div class="aright">
                <asp:TextBox ID="EditLinkDescription" runat="server" Width="80%" Rows="5" TextMode="MultiLine" /></div>
        </div>
        <div class="btnalign">
            <asp:LinkButton ID="lnkUpdateLink" runat="server" Text="Save Changes" ValidationGroup="vgEditLink"
                CssClass="makeButton" />
        </div>
    </asp:Panel>
</asp:PlaceHolder>
