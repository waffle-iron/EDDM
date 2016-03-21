Imports System.Collections.Generic
Imports System.Linq

Partial Class CLibrary_BlogCategoryList
    Inherits CLibraryBase

    Private _sectionHeader As String = ""
    Public Property SectionHeader As String
        Get
            Return _sectionHeader
        End Get
        Set(value As String)
            _sectionHeader = value
        End Set
    End Property

    Protected Overrides Sub BuildControl()
        If String.IsNullOrEmpty(SectionHeader) Then
            phPanelHeader.Visible = False
        Else
            lHeaderText.Text = SectionHeader
        End If

        Dim siteId As Integer = appxCMS.Util.CMSSettings.GetSiteId
        Dim oBlogCats As List(Of appxCMS.NewsBlogCategory) = Nothing
        Using oDb As New appxCMS.appxCMSEntities
            oBlogCats = (From c In oDb.NewsBlogCategories _
                    Where c.SiteId = siteId _
                    And c.ParentId = 0 _
                    And c.Posts.Count > 0 _
                    Order By c.Name Ascending).ToList
        End Using

        lvCategoryList.DataSource = oBlogCats 'appxCMS.NewsBlogCategoryDataSource.GetList()
        lvCategoryList.DataBind()
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub
End Class
