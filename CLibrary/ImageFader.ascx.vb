Imports System.IO

Partial Class CLibrary_ImageFader
    Inherits System.Web.UI.UserControl

    Public Property CssClass() As String
        Get
            Return pContainer.CssClass
        End Get
        Set(ByVal value As String)
            pContainer.CssClass = value
        End Set
    End Property

    Private _ImageDirectory As String = ""
    <appx.cms(appx.cmsAttribute.DataValueType.Free)> _
    Public Property cmsimagesImageDirectoryName() As String
        Get
            Return _ImageDirectory
        End Get
        Set(ByVal value As String)
            _ImageDirectory = Server.UrlDecode(value)
        End Set
    End Property

    Private _FadeTimeoutInSeconds As Integer = 4000
    <appx.cms(appx.cmsAttribute.DataValueType.Free)> _
    Public Property FadeTimeoutInSeconds() As Integer
        Get
            Return _FadeTimeoutInSeconds
        End Get
        Set(ByVal value As Integer)
            _FadeTimeoutInSeconds = value
        End Set
    End Property

    Private _FadeImageHeightInPixels As String = "auto"
    <appx.cms(appx.cmsAttribute.DataValueType.Free)> _
    Public Property FadeImageHeightInPixels() As String
        Get
            Return _FadeImageHeightInPixels
        End Get
        Set(ByVal value As String)
            _FadeImageHeightInPixels = value
        End Set
    End Property

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        BuildControl()
    End Sub

    Protected Sub BuildControl()
        Dim aExt As New ArrayList
        aExt.Add(".jpg")
        aExt.Add(".gif")
        aExt.Add(".png")

        Dim sDirName As String = Me.cmsimagesImageDirectoryName.ToLower
        If Not sDirName.StartsWith("/") Then
            sDirName = "/" & sDirName
        End If
        If Not sDirName.StartsWith("/cmsimages") Then
            sDirName = "/cmsimages" & sDirName
        End If
        Dim sDirPath As String = Server.MapPath(sDirName)
        If Not sDirName.EndsWith("/") Then
            sDirName = sDirName & "/"
        End If
        Dim sCacheKey As String = "ImageFader:" & sDirName.Replace("/", ":").Trim
        Dim sNoCache As Integer = pageBase.QStringToInt("nocache")
        If sNoCache = 1 Then
            Cache.Remove(sCacheKey)
        End If

        Dim aFiles As ArrayList
        Dim iFiles As Integer = 0
        If Cache(sCacheKey) Is Nothing Then
            If Directory.Exists(sDirPath) Then
                aFiles = New ArrayList
                Dim oDir As New DirectoryInfo(sDirPath)
                Dim oFiles() As FileInfo = oDir.GetFiles()
                For Each oFile As FileInfo In oFiles
                    Dim sFile As String = oFile.Name
                    Dim sExt As String = oFile.Extension.ToLower
                    If aExt.Contains(sExt) Then
                        aFiles.Add(sDirName & sFile)
                    End If
                Next

                If aFiles.Count > 0 Then
                    iFiles = aFiles.Count
                    Using oDepends As New CacheDependency(sDirPath)
                        Cache.Insert(sCacheKey, aFiles, oDepends)
                    End Using
                End If
            End If
        End If

        aFiles = DirectCast(Cache(sCacheKey), ArrayList)
        If aFiles.Count > 1 Then
            lvImageRotate.DataSource = aFiles
            lvImageRotate.DataBind()

            jqueryHelper.RegisterStylesheet(Page, "~/scripts/jquery.imagefader.css")

            jqueryHelper.Include(Page)
            jqueryHelper.IncludePlugin(Page, "jQuery-ImageFader", "~/scripts/jquery.innerfade.js")

            Dim oJs As New StringBuilder
            oJs.AppendLine("jQuery(document).ready(function() {")
            oJs.AppendLine("    jQuery('ul.imgRotate').innerfade({")
            oJs.AppendLine("        speed:'slow',")
            oJs.AppendLine("        timeout:" & Me.FadeTimeoutInSeconds * 1000 & ",")
            oJs.AppendLine("        type:'sequence',")
            oJs.AppendLine("        containerheight:'" & Me.FadeImageHeightInPixels.ToLower & IIf(Me.FadeImageHeightInPixels.ToLower = "auto", "", "px") & "'")
            oJs.AppendLine("    });")
            oJs.AppendLine("});")
            Page.ClientScript.RegisterClientScriptBlock(GetType(String), "appxImageFaderInit", oJs.ToString, True)
        Else
            If aFiles.Count = 1 Then
                imgSingle.ImageUrl = aFiles(0)
                pImgSingle.Visible = True
            End If
            pContainer.Visible = False
        End If
    End Sub
End Class
