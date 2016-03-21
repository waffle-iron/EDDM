Imports System.IO

Partial Class CLibrary_BlogPage
    Inherits CLibraryBase
    
    Protected Overrides Sub BuildControl()
        Dim siteId As Integer = appxCMS.Util.CMSSettings.GetSiteId

        Dim sPageFile As String = Server.MapPath("~/cmsimages/_bloglayout/" & siteId & "/BlogPageLayout.htm")

        Dim sCK As String = "SiteId" & siteId & "BlogPageLayout"
        Dim sPageContent As String = appxCMS.Util.Cache.GetString(sCK)
        If String.IsNullOrEmpty(sPageContent) AndAlso File.Exists(sPageFile) Then
            sPageContent = File.ReadAllText(sPageFile)
            appxCMS.Util.Cache.Add(sCK, sPageContent, sPageFile)
        End If
        If Not String.IsNullOrEmpty(sPageContent) Then
            phBlogPage.Controls.Add(DirectCast(Page, appxCMS.PageBase).FormatContent(sPageContent))
        End If

        'Obsolete 6/26/2015
        'appxCMS.Util.jQuery.RegisterStylesheet(Page, "~/cmsimages/_bloglayout/" & siteId & "/blog.css")

    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub
End Class
