Imports System.Xml

Partial Class usercontrols_navsectionContents
    Inherits System.Web.UI.UserControl

    Protected Sub tvSummary_TreeNodeDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs) Handles tvSummary.TreeNodeDataBound
        Dim sIcon As String = ""
        Dim oSMNode As SiteMapNode = e.Node.DataItem
        sIcon = oSMNode.Item("icon")
        If Not String.IsNullOrEmpty(sIcon) Then
            e.Node.ImageUrl = sIcon
        End If
        If Not oSMNode.IsAccessibleToUser(System.Web.HttpContext.Current) Then
            e.Node.SelectAction = TreeNodeSelectAction.None
            e.Node.NavigateUrl = ""
        End If
    End Sub
End Class
