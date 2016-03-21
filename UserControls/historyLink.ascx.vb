
Partial Class usercontrols_historyLink
    Inherits System.Web.UI.UserControl

    Public Property AuditTable() As String
        Get
            Return _HistoryTable
        End Get
        Set(ByVal value As String)
            _HistoryTable = value
        End Set
    End Property
    Private _HistoryTable As String = ""

    Public Property AuditID() As String
        Get
            Return _HistoryID
        End Get
        Set(ByVal value As String)
            _HistoryID = value
        End Set
    End Property
    Private _HistoryID As String = ""

    Public Property LinkText() As String
        Get
            Return _LinkText
        End Get
        Set(ByVal value As String)
            _LinkText = value
        End Set
    End Property
    Private _LinkText As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HttpContext.Current.User.IsInRole("AuditHistory.View") Then
            lnkHistory.Visible = False
        End If
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        lnkHistory.OnClientClick = "window.open('" & VirtualPathUtility.ToAbsolute("~/admin/audithistory.aspx") & "?tbl=" & Server.UrlEncode(AuditTable) & "&id=" & AuditID & "', 'auditHistory', 'height=600,width=500,resizable=yes,scrollbars=yes,toolbars=no,statusbar=no');return false;"
        If Not String.IsNullOrEmpty(LinkText) Then
            lLinkText.Text = " " & LinkText
        End If
    End Sub
End Class
