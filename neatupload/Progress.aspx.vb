Imports neatUpload_Progress

Partial Class neatUpload_Progress
    Inherits Brettle.Web.NeatUpload.ProgressPage

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    Dim oStyle As New HtmlGenericControl("link")
    '    oStyle.Attributes.Add("rel", "stylesheet")
    '    oStyle.Attributes.Add("type", "text/css")
    '    oStyle.Attributes.Add("href", "default.css")
    '    Page.Header.Controls.Add(oStyle)
    'End Sub

    'Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
    '    Page.MasterPageFile = "/app_masterpages/popup.master"
    '    Page.Theme = "taradelmain"

    '    Dim oStyle As New HtmlGenericControl("link")
    '    oStyle.Attributes.Add("rel", "stylesheet")
    '    oStyle.Attributes.Add("type", "text/css")
    '    oStyle.Attributes.Add("href", "/app_themes/taradelmain/taradelmain.css")
    '    Page.Header.Controls.Add(oStyle)
    'End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Page.Theme = ""
    End Sub
End Class
