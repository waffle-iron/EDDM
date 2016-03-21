Imports System.IO
Imports System.Xml

Partial Class CLibrary_ProtectedDirectoryBrowser
    Inherits System.Web.UI.UserControl

#Region "Properties"
    Protected ReadOnly Property ProtectedBasePath() As String
        Get
            Return Server.MapPath("/app_data/ProtectedFiles")
        End Get
    End Property

    Private _ManagerRoles As String = "UserFunction.Admin"
    <appx.cms(appx.cmsAttribute.DataValueType.CMSUserFunctionList)> _
    Public Property ManagerRoles() As String
        Get
            Return _ManagerRoles
        End Get
        Set(ByVal value As String)
            _ManagerRoles = value
        End Set
    End Property

    Private _IsManager As Nullable(Of Boolean)
    Protected ReadOnly Property IsManager() As Boolean
        Get
            If Not _IsManager.HasValue Then
                Dim bManager As Boolean = False
                If Not String.IsNullOrEmpty(Me.ManagerRoles) Then
                    Dim aRoles() As String = Me.ManagerRoles.Split(aSplit)
                    For iRole As Integer = 0 To aRoles.Length - 1
                        Dim sRole As String = aRoles(iRole).Trim
                        If HttpContext.Current.User.IsInRole(sRole) Then
                            bManager = True
                            Exit For
                        End If
                    Next
                End If
                _IsManager = bManager
            End If
            Return _IsManager
        End Get
    End Property

    Private _DirectoryPath As String = ""
    <appx.cms(appx.cmsAttribute.DataValueType.CMSProtectedDirectory)> _
    Public Property DirectoryPath() As String
        Get
            Return _DirectoryPath
        End Get
        Set(ByVal value As String)
            _DirectoryPath = value
        End Set
    End Property

    Private aSplit() As Char = {",", ";"}
#End Region

#Region "Page Setup"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Sub BuildControl()
        If Me.IsManager Then
            phManage.Visible = True
        Else
            phManage.Visible = False
        End If

        If Not String.IsNullOrEmpty(Me.DirectoryPath) Then
            Dim sFullPath As String = Server.MapPath("/app_data/protectedfiles/" & Me.DirectoryPath)
            If Directory.Exists(sFullPath) Then
                lFirstPanel.Text = Me.DirectoryPath
                Dim oPDir As DirectoryInfo = New DirectoryInfo(sFullPath)
                Dim oSubs() As DirectoryInfo = oPDir.GetDirectories

                Dim oFiles() As FileInfo = oPDir.GetFiles

                Dim aFiles As New ArrayList
                aFiles.AddRange(oFiles)
                For iFile As Integer = aFiles.Count - 1 To 0 Step -1
                    Dim oFile As FileInfo = aFiles(iFile)

                    '-- Keep the cmsurl files included... we'll need them
                    '-- oFile.Extension.ToLower = ".cmsurl" Or 
                    If oFile.Extension.ToLower = ".cmsdesc" Or oFile.Extension.ToLower = ".cmsdir" Then
                        aFiles.RemoveAt(iFile)
                    End If
                Next

                '-- Sort files by name
                aFiles.Sort(New FileInfoComparer())

                lvDirectories.DataSource = oSubs
                lvDirectories.DataBind()
                lvFiles.DataSource = aFiles
                lvFiles.DataBind()

                If Me.IsManager Then
                    pDirManage.Visible = True
                End If
            Else
                lMsg.Text = pageBase.UpdateStatusMsg("The path cannot be found.", True)
                pDirInfo.Visible = False

                If Me.IsManager Then
                    pDirInfo.Visible = True
                    lMsg.Text = pageBase.UpdateStatusMsg("The specified path could not be found (" & Me.DirectoryPath & ").", True)
                    phCreateDir.Visible = True
                    phManage.Visible = False
                End If
            End If
        Else
            If Me.IsManager Then
                lMsg.Text = pageBase.UpdateStatusMsg("The directory name was not specified. This can be corrected by managing this page in the CMS.", True)
            Else
                lMsg.Text = pageBase.UpdateStatusMsg("The directory name was not specified.", True)
            End If
            pDirInfo.Visible = False
        End If

        jqueryHelper.Include(Page)
        jqueryHelper.IncludePlugin(Page, "JSON", "/scripts/jquery.json-2.2.min.js")
        Dim oJs As New StringBuilder
        oJs.AppendLine("jQuery(document).ready(function($) {")
        oJs.AppendLine("    $('#dirAccordion').accordion({autoHeight: false,collapsible: true});")
        If Me.IsManager Then
            oJs.AppendLine("    $('#CreateSubDirectory').dialog({")
            oJs.AppendLine("        autoOpen:false,width:600,height:300,modal:true,")
            oJs.AppendLine("        open:function() {$(this).parent().appendTo('form:first');}")
            oJs.AppendLine("    });")
            oJs.AppendLine("    $('a.openCD').click(function(e) {")
            oJs.AppendLine("        e.preventDefault();")
            oJs.AppendLine("        $('#CreateSubDirectory').dialog('open');")
            oJs.AppendLine("    });")
            oJs.AppendLine("    $('#DirectoryInfo').dialog({")
            oJs.AppendLine("        autoOpen:false,width:600,height:300,modal:true,")
            oJs.AppendLine("        open:function() {$(this).parent().appendTo('form:first');}")
            oJs.AppendLine("    });")
            oJs.AppendLine("    $('a.openDI').click(function(e) {")
            oJs.AppendLine("        e.preventDefault();")
            oJs.AppendLine("        $('#" & hfEditDirectoryPath.ClientID & "').val($(this).attr('file'));")
            oJs.AppendLine("        var oData = $.parseJSON($(this).attr('data'));")
            oJs.AppendLine("        $('#" & EditDirectoryTitle.ClientID & "').val(oData.Title);")
            oJs.AppendLine("        $('#" & EditDirectoryDescription.ClientID & "').val(oData.Description);")
            oJs.AppendLine("        $('#DirectoryInfo').dialog('open');")
            oJs.AppendLine("    });")
            oJs.AppendLine("    $('#UploadFile').dialog({")
            oJs.AppendLine("        autoOpen:false,width:600,height:350,modal:true,")
            oJs.AppendLine("        open:function() {$(this).parent().appendTo('form:first');}")
            oJs.AppendLine("    });")
            oJs.AppendLine("    $('a.openUF').click(function(e) {")
            oJs.AppendLine("        e.preventDefault();")
            oJs.AppendLine("        $('#" & hfNewFilePath.ClientID & "').val($(this).attr('file'));")
            oJs.AppendLine("        $('#UploadFile').dialog('open');")
            oJs.AppendLine("    });")
            oJs.AppendLine("    $('#CreateLink').dialog({")
            oJs.AppendLine("        autoOpen:false,width:600,height:350,modal:true,")
            oJs.AppendLine("        open:function() {$(this).parent().appendTo('form:first');}")
            oJs.AppendLine("    });")
            oJs.AppendLine("    $('a.openCL').click(function(e) {")
            oJs.AppendLine("        e.preventDefault();")
            oJs.AppendLine("        $('#" & hfCreateLinkPath.ClientID & "').val($(this).attr('file'));")
            oJs.AppendLine("        $('#CreateLink').dialog('open');")
            oJs.AppendLine("    });")
            oJs.AppendLine("    $('#DeleteDirectory').dialog({")
            oJs.AppendLine("        autoOpen:false,width:600,height:300,modal:true,")
            oJs.AppendLine("        open:function() {$(this).parent().appendTo('form:first');}")
            oJs.AppendLine("    });")
            oJs.AppendLine("    $('a.openDD').click(function(e) {")
            oJs.AppendLine("        e.preventDefault();")
            oJs.AppendLine("        $('#" & hfDeleteDirectoryPath.ClientID & "').val($(this).attr('file'));")
            oJs.AppendLine("        $('#DeleteDirectory').dialog('open');")
            oJs.AppendLine("    });")
            oJs.AppendLine("    $('#" & pEditFile.ClientID & "').dialog({")
            oJs.AppendLine("        autoOpen:false,width:600,height:300,modal:true,")
            oJs.AppendLine("        buttons:{")
            oJs.AppendLine("            'Save Changes':function() {")
            oJs.AppendLine("                var oData = " & appxCMS.Util.JavaScriptSerializer.Serialize(New appxProDir.FileMeta) & ";")
            oJs.AppendLine("                oData.Title = $('#" & EditFileTitle.ClientID & "').val();")
            oJs.AppendLine("                oData.Description = $('#" & EditFileDescription.ClientID & "').val();")
            oJs.AppendLine("                $.ajax({")
            oJs.AppendLine("                    type:'POST',")
            oJs.AppendLine("                    url:'/Resources/ProDir.ashx',")
            oJs.AppendLine("                    data:{maction:'edit',mtype:'file',mpath:$('#" & hfFilePath.ClientID & "').val(),mkey:'" & ConfigurationManager.AppSettings("ProDirEditKey") & "',mdata:$.toJSON(oData)},")
            oJs.AppendLine("                    success:function(msg) {")
            oJs.AppendLine("                        var aResult = msg.split('|');")
            oJs.AppendLine("                        if (aResult[0] == 'true') {")
            oJs.AppendLine("                            location.href = '" & pageBase.GetRequestedURL(Page) & "';")
            oJs.AppendLine("                        } else {")
            oJs.AppendLine("                            alert('There was an error deleting the file:\n\n' + aResult[1]);")
            oJs.AppendLine("                        }")
            oJs.AppendLine("                    },")
            oJs.AppendLine("                    error:function() {")
            oJs.AppendLine("                        alert('There was a communication error while deleting the file.');")
            oJs.AppendLine("                    }")
            oJs.AppendLine("                });")
            oJs.AppendLine("            },")
            oJs.AppendLine("            'Cancel':function() {")
            oJs.AppendLine("                ClearEditForm();")
            oJs.AppendLine("                $('#" & pEditFile.ClientID & "').dialog('close');")
            oJs.AppendLine("            }")
            oJs.AppendLine("        }")
            oJs.AppendLine("    });")
            oJs.AppendLine("    function ClearEditForm() {")
            oJs.AppendLine("        $('#" & hfFilePath.ClientID & "').val('');")
            oJs.AppendLine("        $('#" & EditFileTitle.ClientID & "').val('');")
            oJs.AppendLine("        $('#" & EditFileDescription.ClientID & "').val('');")
            oJs.AppendLine("    }")
            oJs.AppendLine("    $('a.editFI').click(function(e) {")
            oJs.AppendLine("        e.preventDefault();")
            oJs.AppendLine("        $('#" & hfFilePath.ClientID & "').val($(this).attr('file'));")
            oJs.AppendLine("        var oMeta = $.parseJSON($(this).attr('data'));")
            oJs.AppendLine("        $('#" & EditFileTitle.ClientID & "').val(oMeta.Title);")
            oJs.AppendLine("        $('#" & EditFileDescription.ClientID & "').val(oMeta.Description);")
            oJs.AppendLine("        $('#" & pEditFile.ClientID & "').dialog('open');")
            oJs.AppendLine("    });")
            oJs.AppendLine("    $('#" & pEditLink.ClientID & "').dialog({")
            oJs.AppendLine("        autoOpen:false,width:600,height:300,modal:true,")
            oJs.AppendLine("        open:function() {$(this).parent().appendTo('form:first');}")
            oJs.AppendLine("    });")
            oJs.AppendLine("    $('a.editLI').click(function(e) {")
            oJs.AppendLine("        e.preventDefault();")
            oJs.AppendLine("        $('#" & hfEditLinkPath.ClientID & "').val($(this).attr('file'));")
            oJs.AppendLine("        var oMeta = $.parseJSON($(this).attr('data'));")
            oJs.AppendLine("        $('#" & EditLinkURL.ClientID & "').val(oMeta.NavigateUrl);")
            oJs.AppendLine("        $('#" & EditLinkTitle.ClientID & "').val(oMeta.Title);")
            oJs.AppendLine("        $('#" & EditLinkDescription.ClientID & "').val(oMeta.Description);")
            oJs.AppendLine("        $('#" & pEditLink.ClientID & "').dialog('open');")
            oJs.AppendLine("    });")
            oJs.AppendLine("    $('a.deleteFI').click(function(e) {")
            oJs.AppendLine("        e.preventDefault();")
            oJs.AppendLine("        if (confirm('Are you sure you want to remove this file?')) {")
            oJs.AppendLine("            $.ajax({")
            oJs.AppendLine("                type:'POST',")
            oJs.AppendLine("                url:'/Resources/ProDir.ashx',")
            oJs.AppendLine("                data:{maction:'delete',mtype:$(this).attr('ptype'),mpath:$(this).attr('file'),mkey:'" & ConfigurationManager.AppSettings("ProDirEditKey") & "'},")
            oJs.AppendLine("                success:function(msg) {")
            oJs.AppendLine("                    var aResult = msg.split('|');")
            oJs.AppendLine("                    if (aResult[0] == 'true') {")
            oJs.AppendLine("                        location.href = '" & pageBase.GetRequestedURL(Page) & "';")
            oJs.AppendLine("                    } else {")
            oJs.AppendLine("                        alert('There was an error deleting the file:\n\n' + aResult[1]);")
            oJs.AppendLine("                    }")
            oJs.AppendLine("                },")
            oJs.AppendLine("                error:function() {")
            oJs.AppendLine("                    alert('There was a communication error while deleting the file.');")
            oJs.AppendLine("                }")
            oJs.AppendLine("            });")
            oJs.AppendLine("        }")
            oJs.AppendLine("    });")
            oJs.AppendLine("    $('a.deleteLI').click(function(e) {")
            oJs.AppendLine("        e.preventDefault();")
            oJs.AppendLine("        if (confirm('Are you sure you want to remove this link?')) {")
            oJs.AppendLine("            $.ajax({")
            oJs.AppendLine("                type:'POST',")
            oJs.AppendLine("                url:'/Resources/ProDir.ashx',")
            oJs.AppendLine("                data:{maction:'delete',mtype:$(this).attr('ptype'),mpath:$(this).attr('file'),mkey:'" & ConfigurationManager.AppSettings("ProDirEditKey") & "'},")
            oJs.AppendLine("                success:function(msg) {")
            oJs.AppendLine("                    var aResult = msg.split('|');")
            oJs.AppendLine("                    if (aResult[0] == 'true') {")
            oJs.AppendLine("                        location.href = '" & pageBase.GetRequestedURL(Page) & "';")
            oJs.AppendLine("                    } else {")
            oJs.AppendLine("                        alert('There was an error deleting the file:\n\n' + aResult[1]);")
            oJs.AppendLine("                    }")
            oJs.AppendLine("                },")
            oJs.AppendLine("                error:function() {")
            oJs.AppendLine("                    alert('There was a communication error while deleting the file.');")
            oJs.AppendLine("                }")
            oJs.AppendLine("            });")
            oJs.AppendLine("        }")
            oJs.AppendLine("    });")
            oJs.AppendLine("    $('a.openUrl').click(function(e) {")
            oJs.AppendLine("        e.preventDefault();")
            oJs.AppendLine("        location.href = $(this).attr('navurl');")
            oJs.AppendLine("    });")
            'oJs.AppendLine("    $('a.downloadFile').click(function(e) {")
            'oJs.AppendLine("        e.preventDefault();")
            'oJs.AppendLine("        var date = new Date(); var elID = 'pdfdl' + date.getTime();")
            'oJs.AppendLine("        $(document.body).append('<iframe id=""' + elID + '"" height=""0""></iframe>');")
            'oJs.AppendLine("        var iFrameX = jQuery('#' + elID);")
            'oJs.AppendLine("        iFrameX.attr('src', '/resources/ProDownload.ashx?path=' + $(this).attr('ddir') + '&file=' + $(this).attr('name'));")
            'oJs.AppendLine("    });")
        Else
            pEditFile.Visible = False
            pEditLink.Visible = False
        End If
        oJs.AppendLine("});")
        jqueryHelper.RegisterClientScript(Page, Me.ClientID & "Init", oJs.ToString)
    End Sub

    Protected Sub lvDirectories_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvDirectories.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim oDItem As ListViewDataItem = e.Item

            Dim oDInfo As DirectoryInfo = DirectCast(oDItem.DataItem, DirectoryInfo)

            Dim oDir As New appxProDir.DirMeta(Path.Combine(oDInfo.FullName, "this.cmsdir"))
            Dim sName As String = oDInfo.Name
            If oDir IsNot Nothing Then
                If Not String.IsNullOrEmpty(oDir.Title) Then
                    sName = oDir.Title
                End If
            End If
            Dim lName As Literal = DirectCast(e.Item.FindControl("lName"), Literal)
            lName.Text = sName

            If oDir IsNot Nothing Then
                If Not String.IsNullOrEmpty(oDir.Description) Then
                    Dim lDescription As Literal = DirectCast(e.Item.FindControl("lDescription"), Literal)
                    lDescription.Text = "<p>" & oDir.Description & "</p>"
                End If
            End If

            Dim lvFiles As ListView = DirectCast(e.Item.FindControl("lvFiles"), ListView)

            Dim aFiles As New ArrayList
            aFiles.AddRange(oDInfo.GetFiles)
            For iFile As Integer = aFiles.Count - 1 To 0 Step -1
                Dim oFile As FileInfo = aFiles(iFile)

                If oFile.Extension.ToLower = ".cmsdesc" Or oFile.Extension.ToLower = ".cmsdir" Then
                    aFiles.RemoveAt(iFile)
                End If
            Next

            lvFiles.DataSource = aFiles
            lvFiles.DataBind()

            If Me.IsManager Then
                Dim lnkOpenDirMeta As LinkButton = DirectCast(e.Item.FindControl("lnkOpenDirMeta"), LinkButton)
                lnkOpenDirMeta.Attributes.Add("file", oDInfo.FullName)
                lnkOpenDirMeta.Attributes.Add("data", appxCMS.Util.JavaScriptSerializer.Serialize(Of appxProDir.DirMeta)(oDir))

                Dim lnkDeleteDir As LinkButton = DirectCast(e.Item.FindControl("lnkDeleteDir"), LinkButton)
                lnkDeleteDir.Attributes.Add("file", oDInfo.FullName)

                Dim lnkOpenUploadFile As LinkButton = DirectCast(e.Item.FindControl("lnkOpenUploadFile"), LinkButton)
                lnkOpenUploadFile.Attributes.Add("file", oDInfo.FullName)

                Dim lnkOpenCreateLink As LinkButton = DirectCast(e.Item.FindControl("lnkOpenCreateLink"), LinkButton)
                lnkOpenCreateLink.Attributes.Add("file", oDInfo.FullName)
            End If
        End If
    End Sub

    Protected Sub FileList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs)
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim oDItem As ListViewDataItem = e.Item

            Dim oFInfo As FileInfo = DirectCast(oDItem.DataItem, FileInfo)

            Dim lnkDownloadFile As HyperLink = DirectCast(e.Item.FindControl("lnkDownloadFile"), HyperLink)
            Dim lFileType As Literal = DirectCast(e.Item.FindControl("lFileType"), Literal)
            Dim lFileSize As Literal = DirectCast(e.Item.FindControl("lFileSize"), Literal)
            Dim pDesc As Panel = DirectCast(e.Item.FindControl("pDesc"), Panel)
            Dim lDesc As Literal = DirectCast(e.Item.FindControl("lDesc"), Literal)

            If oFInfo.Extension = ".cmsurl" Then
                Dim oUrl As New appxProDir.LinkMeta(oFInfo.FullName)
                lnkDownloadFile.CssClass = "openUrl"
                lnkDownloadFile.NavigateUrl = oUrl.NavigateUrl
                lnkDownloadFile.Text = oUrl.Title
                lnkDownloadFile.Target = "_blank"
                'lnkDownloadFile.Attributes.Add("navurl", oUrl.NavigateUrl)
                'lnkDownloadFile.Text = oUrl.Title

                lFileType.Text = "Link"
                If Not String.IsNullOrEmpty(oUrl.Description) Then
                    pDesc.Visible = True
                    lDesc.Text = oUrl.Description
                Else
                    pDesc.Visible = False
                End If

                If Me.IsManager Then
                    Dim lnkEditMeta As LinkButton = DirectCast(e.Item.FindControl("lnkEditMeta"), LinkButton)
                    lnkEditMeta.CssClass = lnkEditMeta.CssClass.Replace("editFI", "editLI")
                    lnkEditMeta.Attributes.Add("data", appxCMS.Util.JavaScriptSerializer.Serialize(Of appxProDir.LinkMeta)(oUrl).Replace("'", "&apos;"))
                    lnkEditMeta.Attributes.Add("file", oFInfo.FullName)

                    Dim lnkDelete As LinkButton = DirectCast(e.Item.FindControl("lnkDelete"), LinkButton)
                    lnkDelete.CssClass = lnkDelete.CssClass.Replace("deleteFI", "deleteLI")
                    lnkDelete.Attributes.Add("file", oFInfo.FullName)
                    lnkDelete.Attributes.Add("ptype", "link")
                End If
            Else
                '-- Check for a meta file for this guy
                Dim sMetaFile As String = oFInfo.FullName & ".cmsdesc"
                Dim oFile As New appxProDir.FileMeta(oFInfo, sMetaFile)

                'lnkDownloadFile.CommandArgument = oFInfo.FullName
                'lnkDownloadFile.Attributes.Add("name", oFInfo.Name)
                Dim sFilePath As String = oFInfo.DirectoryName.ToLower
                Dim sPBase As String = Me.ProtectedBasePath.ToLower
                sFilePath = sFilePath.Replace(sPBase, "")
                'lnkDownloadFile.Attributes.Add("ddir", sFilePath)
                lnkDownloadFile.Text = oFile.Title
                'lnkDownloadFile.Target = "_blank"
                lnkDownloadFile.NavigateUrl = "/resources/ProDownload.ashx?path=" & Server.UrlEncode(sFilePath) & "&file=" & Server.UrlEncode(oFInfo.Name)

                Dim sType As String = ""
                Dim sExt As String = oFInfo.Extension.ToLower
                Select Case sExt
                    Case ".doc", ".docx"
                        sType = "MS Word"
                    Case ".xls", ".xlsx"
                        sType = "MS Excel"
                    Case ".ppt", "pptx"
                        sType = "MS Powerpoint"
                    Case ".jpg", ".png", ".gif"
                        sType = "Image"
                    Case ".pdf"
                        sType = "Adobe PDF"
                    Case Else
                        sType = sExt.Substring(1).ToUpper
                End Select
                lFileType.Text = sType

                lFileSize.Text = oFile.Size

                If String.IsNullOrEmpty(oFile.Description) Then
                    pDesc.Visible = False
                Else
                    lDesc.Text = oFile.Description
                End If

                If Me.IsManager Then
                    Dim lnkEditMeta As LinkButton = DirectCast(e.Item.FindControl("lnkEditMeta"), LinkButton)
                    lnkEditMeta.Attributes.Add("data", appxCMS.Util.JavaScriptSerializer.Serialize(Of appxProDir.FileMeta)(oFile).Replace("'", "&apos;"))
                    lnkEditMeta.Attributes.Add("file", oFInfo.FullName & ".cmsdesc")

                    Dim lnkDelete As LinkButton = DirectCast(e.Item.FindControl("lnkDelete"), LinkButton)
                    lnkDelete.Attributes.Add("file", oFInfo.FullName)
                    lnkDelete.Attributes.Add("ptype", "file")
                End If
            End If
        End If
    End Sub
#End Region

#Region "Methods"
    'Protected Sub DoDownload(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim oLnk As LinkButton = DirectCast(sender, LinkButton)

    '    Dim sFile As String = oLnk.CommandArgument

    '    If File.Exists(sFile) Then
    '        Dim sFileName As String = Path.GetFileName(sFile)
    '        Response.Clear()
    '        Response.AddHeader("Content-Disposition", "attachment; filename=" & sFileName)
    '        Response.ContentType = "application/octet-stream"
    '        Response.WriteFile(sFile)
    '        Response.End()
    '    End If
    'End Sub

    Protected Sub lnkProvision_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkProvision.Click
        Dim aProbs As New ArrayList

        Dim sProvPath As String = Path.Combine(Me.ProtectedBasePath, Me.DirectoryPath.Replace("/", "\"))
        '-- Recursively provision path
        Dim aPathSep() As Char = {"/", "\"}
        Dim aPaths() As String = Me.DirectoryPath.Split(aPathSep)
        For iPath As Integer = 0 To aPaths.Length - 1
            Dim sPathStr As String = ""
            Dim sJoin As String = ""
            Dim sCurPath As String = Me.ProtectedBasePath
            For i As Integer = 0 To iPath
                sCurPath = Path.Combine(sCurPath, aPaths(i))
            Next
            If Not Directory.Exists(sCurPath) Then
                'Response.Write("Need to provision " & sCurPath & "<br/>")
                Try
                    Directory.CreateDirectory(sCurPath)
                Catch ex As Exception
                    aProbs.Add("<div>There was an error creating the protected directory.</div>")
                End Try
            End If
        Next
        If aProbs.Count > 0 Then
            lMsg.Text = pageBase.UpdateStatusMsg(String.Join("", aProbs.ToArray(GetType(String))), True)
        Else
            Response.Redirect(pageBase.GetRequestedURL(Page))
        End If
    End Sub

    Protected Sub lnkCreateSubDirectory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCreateSubDirectory.Click
        If SubDirectoryName.Text.Contains("/") Or SubDirectoryName.Text.Contains("\") Then
            lMsg.Text = pageBase.UpdateStatusMsg("Disallowed character / or \ used in directory name.", True)
            Exit Sub
        End If

        Dim sPath As String = Path.Combine(Me.ProtectedBasePath, Me.DirectoryPath)
        Dim sProvPath As String = Path.Combine(sPath, SubDirectoryName.Text)
        Try
            Directory.CreateDirectory(sProvPath)

            Dim oDirInfo As New appxProDir.DirMeta()
            oDirInfo.Title = SubDirectoryTitle.Text
            oDirInfo.Description = SubDirectoryDescription.Text

            File.WriteAllText(Path.Combine(sProvPath, "this.cmsdir"), appxCMS.Util.JavaScriptSerializer.Serialize(oDirInfo))

            Response.Redirect(pageBase.GetRequestedURL(Page))
        Catch ex As Exception
            lMsg.Text = pageBase.UpdateStatusMsg("There was an error creating the directory: " & ex.Message, True)
        End Try
    End Sub
#End Region

#Region "Member Classes"

#End Region

    Protected Sub lnkSaveDirectoryInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSaveDirectoryInfo.Click
        Dim sDir As String = hfEditDirectoryPath.Value
        Dim sTitle As String = EditDirectoryTitle.Text
        Dim sDesc As String = EditDirectoryDescription.Text

        If Not String.IsNullOrEmpty(sDir) Then
            If Directory.Exists(sDir) Then
                Dim oDir As New appxProDir.DirMeta()
                oDir.Title = sTitle
                oDir.Description = sDesc

                Dim sData As String = appxCMS.Util.JavaScriptSerializer.Serialize(oDir)

                File.WriteAllText(Path.Combine(sDir, "this.cmsdir"), sData)

                Response.Redirect(pageBase.GetRequestedURL(Page))
            Else
                lMsg.Text = pageBase.UpdateStatusMsg("The directory cannot be found.", True)
            End If
        Else
            lMsg.Text = pageBase.UpdateStatusMsg("The directory was not specified.", True)
        End If
    End Sub

    Protected Sub lnkDirectoryDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDirectoryDelete.Click
        Dim sDir As String = hfDeleteDirectoryPath.Value
        Dim sConfirm As String = DeleteConfirm.Text

        If sConfirm = "DELETE" Then
            If Not String.IsNullOrEmpty(sDir) Then
                If Directory.Exists(sDir) Then
                    Try
                        Directory.Delete(sDir, True)
                        Response.Redirect(pageBase.GetRequestedURL(Page))
                    Catch ex As Exception
                        lMsg.Text = pageBase.UpdateStatusMsg("There was an error deleting the directory: " & ex.Message, True)
                    End Try
                Else
                    lMsg.Text = pageBase.UpdateStatusMsg("The directory cannot be found.", True)
                End If
            Else
                lMsg.Text = pageBase.UpdateStatusMsg("The directory was not specified.", True)
            End If
        Else
            lMsg.Text = pageBase.UpdateStatusMsg("The confirmation text was incorrect. Directory was NOT deleted.", True)
        End If
    End Sub

    Protected Sub lnkUploadFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkUploadFile.Click
        Dim sDir As String = hfNewFilePath.Value
        If String.IsNullOrEmpty(sDir) Then
            sDir = Path.Combine(Me.ProtectedBasePath, Me.DirectoryPath)
        End If

        If NewFile.HasFile Then
            Dim sFile As String = NewFile.FileName
            Dim sTitle As String = FileTitle.Text.Trim
            Dim sDesc As String = FileDescription.Text.Trim
            Dim oFile As New appxProDir.FileMeta()
            oFile.Title = sTitle
            oFile.Description = sDesc
            Dim bSaved As Boolean = False
            Dim sTarget As String = Path.Combine(sDir, sFile)
            Try
                If chkOverwrite.Checked Then
                    NewFile.MoveTo(sTarget, Brettle.Web.NeatUpload.MoveToOptions.Overwrite)
                Else
                    NewFile.MoveTo(sTarget, Brettle.Web.NeatUpload.MoveToOptions.None)
                End If
                bSaved = True
            Catch ex As Exception
                lMsg.Text = pageBase.UpdateStatusMsg("There was an error uploading the file: " & ex.Message, True)
            End Try

            If bSaved Then
                If File.Exists(sTarget & ".cmsdesc") Then
                    '-- Only overwrite if a title or description were provided
                    If Not String.IsNullOrEmpty(sTitle) Or Not String.IsNullOrEmpty(sDesc) Then
                        File.WriteAllText(sTarget & ".cmsdesc", appxCMS.Util.JavaScriptSerializer.Serialize(oFile))
                    End If
                Else
                    File.WriteAllText(sTarget & ".cmsdesc", appxCMS.Util.JavaScriptSerializer.Serialize(oFile))
                End If
                Response.Redirect(pageBase.GetRequestedURL(Page))
            End If
        Else
            lMsg.Text = pageBase.UpdateStatusMsg("You must browse and select a file to upload!", True)
        End If
    End Sub

    Protected Sub lnkSaveLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSaveLink.Click
        Dim sDir As String = hfCreateLinkPath.Value
        If String.IsNullOrEmpty(sDir) Then
            sDir = Path.Combine(Me.ProtectedBasePath, Me.DirectoryPath)
        End If

        Dim oUrl As New appxProDir.LinkMeta()
        oUrl.NavigateUrl = LinkURL.Text
        oUrl.Title = LinkTitle.Text
        oUrl.Description = LinkDescription.Text

        Dim sLinkName As String = System.Guid.NewGuid.ToString & ".cmsurl"
        File.WriteAllText(Path.Combine(sDir, sLinkName), appxCMS.Util.JavaScriptSerializer.Serialize(oUrl))
        Response.Redirect(pageBase.GetRequestedURL(Page))
    End Sub

    Protected Sub lnkUpdateLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkUpdateLink.Click
        Dim sFile As String = hfEditLinkPath.Value
        If String.IsNullOrEmpty(sFile) Then
            lMsg.Text = pageBase.UpdateStatusMsg("The link file was not specified.", True)
        Else
            Dim oLink As New appxProDir.LinkMeta
            oLink.NavigateUrl = EditLinkURL.Text
            oLink.Title = EditLinkTitle.Text
            oLink.Description = EditLinkDescription.Text

            File.WriteAllText(sFile, appxCMS.Util.JavaScriptSerializer.Serialize(oLink))
            Response.Redirect(pageBase.GetRequestedURL(Page))
        End If
    End Sub

    Protected Class FileInfoComparer
        Implements System.Collections.IComparer

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim oFIX As FileInfo = DirectCast(x, FileInfo)
            Dim oFIY As FileInfo = DirectCast(y, FileInfo)
            Return oFIX.Name.CompareTo(oFIY.Name)
        End Function
    End Class
End Class
