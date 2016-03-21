Imports System.Collections.Generic

Partial Class UserControls_BootstrapDataPager
    Inherits System.Web.UI.UserControl

    Public Property CssClass() As String
        Get
            Return ""
            'Return pDataPager.CssClass
        End Get
        Set(ByVal value As String)
            '-- Not implemented
            'pDataPager.CssClass = value
        End Set
    End Property

    'Public Property ShowPageSummary As Boolean
    '    Get
    '        Return phPageSummary.Visible
    '    End Get
    '    Set(value As Boolean)
    '        phPageSummary.Visible = value
    '    End Set
    'End Property

    Private _QuerystringField As String = "pg"
    Public Property QuerystringField() As String
        Get
            Return _QuerystringField
        End Get
        Set(ByVal value As String)
            _QuerystringField = value
        End Set
    End Property

    Private _NavigatePage As String = ""
    Public Property NavigatePage() As String
        Get
            Return _NavigatePage
        End Get
        Set(ByVal value As String)
            _NavigatePage = value
        End Set
    End Property

    Private _ResultCount As Integer = 0
    Public Property ResultCount() As Integer
        Get
            Return _ResultCount
        End Get
        Set(ByVal value As Integer)
            _ResultCount = value
        End Set
    End Property

    Private _CurPage As Integer = 1
    Public Property CurPage() As Integer
        Get
            Return _CurPage
        End Get
        Set(ByVal value As Integer)
            _CurPage = value
        End Set
    End Property

    Private _PageSize As Integer = 25
    Public Property PageSize() As Integer
        Get
            Return _PageSize
        End Get
        Set(ByVal value As Integer)
            _PageSize = value
        End Set
    End Property

    Private _ShowPageNumbers As Boolean = True
    Public Property ShowPageNumbers As Boolean
        Get
            Return _ShowPageNumbers
        End Get
        Set(ByVal value As Boolean)
            _ShowPageNumbers = value
        End Set
    End Property

    Private _ShowMoveMax As Boolean = True
    Public Property ShowMoveMax As Boolean
        Get
            Return _ShowMoveMax
        End Get
        Set(ByVal value As Boolean)
            _ShowMoveMax = value
        End Set
    End Property

    Private _HideUnusableItems As Boolean = False
    Public Property HideUnusableItems As Boolean
        Get
            Return _HideUnusableItems
        End Get
        Set(ByVal value As Boolean)
            _HideUnusableItems = value
        End Set
    End Property

    Private _ReversePageIndicators As Boolean = False
    Public Property ReversePageIndicators As Boolean
        Get
            Return _ReversePageIndicators
        End Get
        Set(ByVal value As Boolean)
            _ReversePageIndicators = value
        End Set
    End Property

    Private _MovePreviousClass As String = ""
    Public Property MovePreviousClass As String
        Get
            Return _MovePreviousClass
        End Get
        Set(ByVal value As String)
            _MovePreviousClass = value
        End Set
    End Property

    Private _MovePreviousText As String = "<i class=""fa fa-step-backward""></i>"
    Public Property MovePreviousText As String
        Get
            Return _MovePreviousText
        End Get
        Set(ByVal value As String)
            _MovePreviousText = value
        End Set
    End Property

    Private _MoveFirstClass As String = ""
    Public Property MoveFirstClass As String
        Get
            Return _MoveFirstClass
        End Get
        Set(ByVal value As String)
            _MoveFirstClass = value
        End Set
    End Property

    Private _MoveFirstText As String = "<i class=""fa fa-backward""></i>"
    Public Property MoveFirstText As String
        Get
            Return _MoveFirstText
        End Get
        Set(ByVal value As String)
            _MoveFirstText = value
        End Set
    End Property

    Private _MoveNextClass As String = ""
    Public Property MoveNextClass As String
        Get
            Return _MoveNextClass
        End Get
        Set(ByVal value As String)
            _MoveNextClass = value
        End Set
    End Property

    Private _MoveNextText As String = "<i class=""fa fa-step-forward""></i>"
    Public Property MoveNextText As String
        Get
            Return _MoveNextText
        End Get
        Set(ByVal value As String)
            _MoveNextText = value
        End Set
    End Property

    Private _MoveEndClass As String = ""
    Public Property MoveEndClass As String
        Get
            Return _MoveEndClass
        End Get
        Set(ByVal value As String)
            _MoveEndClass = value
        End Set
    End Property

    Private _MoveEndText As String = "<i class=""fa fa-forward""></i>"
    Public Property MoveEndText As String
        Get
            Return _MoveEndText
        End Get
        Set(ByVal value As String)
            _MoveEndText = value
        End Set
    End Property

    Public Property HideMaxMove As Boolean = False

    Private _summaryDesc As String = "result"
    Public Property SummaryDesc As String
        Get
            Return _summaryDesc
        End Get
        Set(value As String)
            _summaryDesc = value
        End Set
    End Property

    Private _pageSummary As String = ""
    Public Function GetPageSummary() As String
        Dim iFirst As Integer = ((Me.CurPage - 1) * Me.PageSize) + 1
        Dim iLast As Integer = (iFirst + Me.PageSize) - 1
        If iLast > Me.ResultCount Then
            iLast = Me.ResultCount
        End If
        Dim sDesc As String = SummaryDesc
        If Me.ResultCount <> 1 Then
            sDesc = sDesc & "s"
        End If
        If Me.ResultCount > 0 Then
            If Me.ResultCount <= Me.PageSize Then
                _pageSummary = "Showing all " & Me.ResultCount.ToString("N0") & " " & sDesc
            Else
                _pageSummary = "Showing " & iFirst.ToString("N0") & " - " & iLast.ToString("N0") & " of " & Me.ResultCount.ToString("N0") & " " & sDesc
            End If
        Else
            _pageSummary = "There are no " & sDesc & " to show."
        End If
        Return _pageSummary
    End Function

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init


    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim sNavJoin As String = "?"
        If Me.NavigatePage.Contains("?") Then
            sNavJoin = "&"
        End If
        Dim iFirstPage As Integer = 1
        Dim iLastPage As Integer = Math.Ceiling(Me.ResultCount / Me.PageSize)
        If iLastPage = 0 Then
            iLastPage = 1
        End If

        Dim iFirst As Integer = ((Me.CurPage - 1) * Me.PageSize) + 1
        'lFirstItemNumber.Text = iFirst
        Dim iLast As Integer = (iFirst + Me.PageSize) - 1
        If iLast > Me.ResultCount Then
            iLast = Me.ResultCount
        End If
        'lLastItemNumber.Text = iLast
        'lTotalItems.Text = Me.ResultCount.ToString("N0")

        If Not Me.ShowMoveMax Then
            hplMoveFirst.Visible = False
            hplMoveLast.Visible = False
        End If
        hplMoveFirst.NavigateUrl = Me.NavigatePage & sNavJoin & Me.QuerystringField & "=" & iFirstPage
        hplMoveLast.NavigateUrl = Me.NavigatePage & sNavJoin & Me.QuerystringField & "=" & iLastPage
        hplMovePrevious.NavigateUrl = Me.NavigatePage & sNavJoin & Me.QuerystringField & "=" & Me.CurPage - 1
        hplMoveNext.NavigateUrl = Me.NavigatePage & sNavJoin & Me.QuerystringField & "=" & Me.CurPage + 1

        hplMoveFirst.CssClass = Me.MoveFirstClass
        hplMoveFirst.Text = Me.MoveFirstText

        hplMoveLast.CssClass = Me.MoveEndClass
        hplMoveLast.Text = Me.MoveEndText

        hplMovePrevious.CssClass = Me.MovePreviousClass
        hplMovePrevious.Text = Me.MovePreviousText

        hplMoveNext.CssClass = Me.MoveNextClass
        hplMoveNext.Text = Me.MoveNextText

        If Me.CurPage <= iFirstPage Then
            hplMoveFirst.Enabled = False
            hplMoveFirst.CssClass = hplMoveFirst.CssClass & " disabled"
            hplMovePrevious.Enabled = False
            hplMovePrevious.CssClass = hplMovePrevious.CssClass & " disabled"

            If Me.HideUnusableItems Then
                hplMoveFirst.Visible = False
                hplMovePrevious.Visible = False
            End If
        End If

        If Me.CurPage >= iLastPage Then
            hplMoveNext.Enabled = False
            hplMoveFirst.CssClass = hplMoveFirst.CssClass & " disabled"
            hplMoveLast.Enabled = False
            hplMoveLast.CssClass = hplMoveLast.CssClass & " disabled"

            If Me.HideUnusableItems Then
                hplMoveNext.Visible = False
                hplMoveLast.Visible = False
            End If
        End If

        If Me.ShowPageNumbers Then
            Dim dNav As New Dictionary(Of String, String)
            Dim iStartText As Integer = Me.CurPage - 2
            If iStartText < 1 Then
                iStartText = 1
            End If
            Dim iEndText As Integer = iStartText + 4
            If iEndText > iLastPage Then
                iEndText = iLastPage
                iStartText = iLastPage - 4
            End If
            If iStartText < 1 Then
                iStartText = 1
            End If

            For i As Integer = iStartText To iEndText
                dNav.Add(i.ToString, Me.NavigatePage & sNavJoin & Me.QuerystringField & "=" & i)
            Next

            rPages.DataSource = dNav
            rPages.DataBind()
        End If

        If HideMaxMove Then
            hplMoveFirst.Visible = False
            hplMoveLast.Visible = False
        End If
    End Sub
End Class
