Imports System.IO
Imports System.Xml

Partial Class CLibrary_NewsBlogArchiveList
    Inherits CLibraryBase

    Private _NumberOfStories As Integer = 5
    Public Property NumberOfStoriesToShow() As String
        Get
            Return _NumberOfStories.ToString
        End Get
        Set(ByVal value As String)
            Dim iVal As Integer = _NumberOfStories
            If Integer.TryParse(value, iVal) Then
                _NumberOfStories = iVal
            End If
        End Set
    End Property

    Private _NewsTypeId As Integer = 0
    <appx.cms(appx.cmsAttribute.DataValueType.NewsBlogCategory)> _
    Public Property NewsTypeId() As Integer
        Get
            Return _NewsTypeId
        End Get
        Set(value As Integer)
            _NewsTypeId = value
        End Set
    End Property

    Private _NoNewsMessage As String = "There are no news stories currently active."
    <appx.cms(appx.cmsAttribute.DataValueType.Free)> _
    Public Property NoNewsMessage() As String
        Get
            Return _NoNewsMessage
        End Get
        Set(ByVal value As String)
            _NoNewsMessage = value
        End Set
    End Property

    Private _ShowPostImagePreview As Boolean = False
    Public Property ShowPostImagePreview As Boolean
        Get
            Return _ShowPostImagePreview
        End Get
        Set(value As Boolean)
            _ShowPostImagePreview = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Private aExt() As String = {".jpg", ".png", ".gif"}
    Private oImagesDir As DirectoryInfo

    Protected Overrides Sub BuildControl()
        If Me.ShowPostImagePreview Then
            oImagesDir = New DirectoryInfo(Server.MapPath("~/cmsimages/PostImages"))
        End If

        Using oNewsA As New appxCMSNewsTableAdapters.NewsTableAdapter
            Dim oNewsT As appxCMSNews.NewsDataTable = Nothing
            If Me.NewsTypeId > 0 Then
                oNewsT = oNewsA.GetLatestByNewsType(Me._NumberOfStories, Me.NewsTypeId)
            Else
                oNewsT = oNewsA.GetLatest(Me._NumberOfStories)
            End If

            lvArchive.DataSource = oNewsT
            lvArchive.DataBind()
            oNewsT.Dispose()
        End Using

        Dim oJs As New StringBuilder
        oJs.AppendLine("jQuery(document).ready(function($) {")
        oJs.AppendLine("    $('img.postPreviewImage').each(function() {")
        oJs.AppendLine("        var oP = $(this).parent();")
        oJs.AppendLine("        console.log(oP.height());")
        oJs.AppendLine("        $(this).css('height', (oP.height()-2) + 'px');")
        oJs.AppendLine("    });")
        oJs.AppendLine("});")
        appxCMS.Util.jQuery.RegisterClientScript(Page, "NewsBlogArchiveListInit", oJs.ToString)
    End Sub

    Protected Sub lvNews_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvArchive.ItemCreated
        If e.Item.ItemType = ListViewItemType.EmptyItem Then
            Dim lEmptyMessage As Literal = DirectCast(e.Item.FindControl("lEmptyMessage"), Literal)
            lEmptyMessage.Text = Me.NoNewsMessage
        End If
    End Sub

    Dim sCurMonthYear As String = ""
    Protected Sub lvArchive_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvArchive.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim oDItem As ListViewDataItem = e.Item

            Dim dPubDate As DateTime = DataBinder.Eval(oDItem.DataItem, "PublishDate")
            Dim sPubDate As String = dPubDate.ToString("MMyyyy")
            If sPubDate <> sCurMonthYear Then
                Dim monthHeader As HtmlControl = DirectCast(e.Item.FindControl("monthHeader"), HtmlControl)
                If monthHeader IsNot Nothing Then
                    monthHeader.Visible = True
                    sCurMonthYear = sPubDate
                End If
            End If

            If ShowPostImagePreview Then
                Dim bHasImage As Boolean = False
                Dim imgPreview As Image = DirectCast(e.Item.FindControl("imgPreview"), Image)
                imgPreview.Visible = True
                imgPreview.Style.Add("width", "200px")
                imgPreview.Style.Add("height", "50px")

                Dim NewsId As Integer = DataBinder.Eval(oDItem.DataItem, "NewsId")
                Dim oPostImages() As FileInfo = oImagesDir.GetFiles(NewsId & ".*")
                If oPostImages.Length > 0 Then
                    For Each oPostImage As FileInfo In oPostImages
                        If Not oPostImage.Extension.Equals(".caption", System.StringComparison.OrdinalIgnoreCase) Then
                            imgPreview.Style.Add("background", "url(/cmsimages/PostImages/" & NewsId & oPostImage.Extension & ")")
                            imgPreview.Style.Add("background-position", "-10px -50px")
                            imgPreview.Style.Add("border", "1px solid #222")
                            bHasImage = True
                            Exit For
                        End If
                    Next
                End If

                If Not bHasImage Then
                    Dim sStory As String = DataBinder.Eval(oDItem.DataItem, "Story")
                    Dim sXHTML As String = ""
                    Using oTR As TextReader = New StringReader(sStory)
                        Dim oSGMLReader As New Sgml.SgmlReader
                        oSGMLReader.DocType = "HTML"
                        oSGMLReader.InputStream = oTR
                        sXHTML = oSGMLReader.ReadOuterXml
                    End Using

                    If Not String.IsNullOrEmpty(sXHTML) Then
                        Dim oXml As New XmlDocument
                        oXml.LoadXml(sXHTML)
                        Dim oImg As XmlNode = oXml.SelectSingleNode("//img")
                        If oImg IsNot Nothing Then
                            imgPreview.Style.Add("background", "url(" & xmlhelp.ReadAttribute(oImg, "src") & ")")
                            imgPreview.Style.Add("background-position", "-10px -50px")
                            imgPreview.Style.Add("border", "1px solid #222")
                        End If
                    End If
                End If
            End If
        End If
    End Sub
End Class
