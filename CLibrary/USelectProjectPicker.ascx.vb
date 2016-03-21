Imports System.Collections.Generic
Imports System.Linq

Partial Class CLibrary_USelectProjectPicker
    Inherits CLibraryBase

    Protected Overrides Sub BuildControl()
        Dim oDist As List(Of Taradel.CustomerDistribution) = Taradel.CustomerDistributions.GetList(GetCustomerId)
        If oDist.Count > 0 Then
            lvSelects.DataSource = oDist
            lvSelects.DataBind()

            Dim oJs As New StringBuilder
            oJs.AppendLine("jQuery(document).ready(function($) {")
            oJs.AppendLine("    $('#uselectPicker').dialog({")
            oJs.AppendLine("        width:600,")
            oJs.AppendLine("        height:600,")
            oJs.AppendLine("        modal:true,")
            oJs.AppendLine("        autoOpen:false")
            oJs.AppendLine("    });")
            oJs.AppendLine("    $('#" & lnkShowSelections.ClientID & "').click(function(e) {")
            oJs.AppendLine("        e.preventDefault();")
            oJs.AppendLine("        $('#uselectPicker').dialog('open');")
            oJs.AppendLine("    });")
            oJs.AppendLine("});")
            appxCMS.Util.jQuery.RegisterClientScript(Page, Me.ClientID & "Init", oJs.ToString)
        Else
            phUSelect.Visible = False
        End If
    End Sub

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        BuildControl()
    End Sub

    Protected Function GetCustomerId() As Integer
        Dim CustomerId As Integer = 0
        Dim sUser As String = HttpContext.Current.User.Identity.Name

        Dim sKey As String = "appxAuth:" & sUser & ":Id"
        If appxCMS.Util.Cache.Exists(sKey) Then
            CustomerId = appxCMS.Util.Cache.GetInteger(sKey)
        Else
            Dim oUser As Taradel.Customer = Taradel.CustomerDataSource.GetCustomer(sUser)
            If oUser IsNot Nothing Then
                CustomerId = oUser.CustomerID
                '-- Cache the value
                appxCMS.Util.Cache.Add(sKey, CustomerId)
            End If
        End If
        Return CustomerId
    End Function

    Protected Sub lvSelects_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvSelects.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim oDItem As ListViewDataItem = e.Item

            Dim oSelect As Taradel.CustomerDistribution = DirectCast(oDItem.DataItem, Taradel.CustomerDistribution)
            Dim iSelectId As Integer = oSelect.USelectMethod.USelectId
            If iSelectId = 1 Then
                Dim hplOpenSelect As HyperLink = DirectCast(e.Item.FindControl("hplOpenSelect"), HyperLink)

                hplOpenSelect.NavigateUrl = "~/Step1-TargetReview.aspx?distid=" & oSelect.DistributionId
            Else
                e.Item.Visible = False
            End If
        End If
    End Sub
End Class
