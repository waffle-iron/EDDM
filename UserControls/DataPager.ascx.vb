Imports System.Collections.Generic
Imports System.Web.UI.HtmlControls



Partial Class UserControls_DataPager
    Inherits System.Web.UI.UserControl

    Public Property CssClass() As String
        Get
            Return pDataPager.CssClass
        End Get
        Set(ByVal value As String)
            pDataPager.CssClass = value
        End Set
    End Property

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

    Private _CurPage As Integer = 0
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

    Public Property ResultDescription As String
        Get
            Return lDescription.Text
        End Get
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                lDescription.Text = value
                lDescription.Visible = True
            Else
                lDescription.Text = value
                lDescription.Visible = False
            End If
        End Set
    End Property

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Me.CurPage = 0 Then
            Me.CurPage = 1
        End If
        Dim sNavJoin As String = "?"
        If Me.NavigatePage.Contains("?") Then
            sNavJoin = "&"
        End If
        Dim iFirstPage As Integer = 1
        Dim iLastPage As Integer = Math.Ceiling(Me.ResultCount / Me.PageSize)
        If iLastPage = 0 Then
            iLastPage = 1
        End If

        hplMoveFirst.NavigateUrl = Me.NavigatePage & sNavJoin & Me.QuerystringField & "=" & iFirstPage
        hplMoveLast.NavigateUrl = Me.NavigatePage & sNavJoin & Me.QuerystringField & "=" & iLastPage
        hplMovePrevious.NavigateUrl = Me.NavigatePage & sNavJoin & Me.QuerystringField & "=" & Me.CurPage - 1
        hplMoveNext.NavigateUrl = Me.NavigatePage & sNavJoin & Me.QuerystringField & "=" & Me.CurPage + 1

        If Me.CurPage <= iFirstPage Then
            hplMoveFirst.Enabled = False
            hplMoveFirst.CssClass = hplMoveFirst.CssClass & "disabled"
            hplMovePrevious.Enabled = False
            hplMovePrevious.CssClass = hplMovePrevious.CssClass & "disabled"
            liPrev.Attributes.Add("class", "disabled")
            liNext.Attributes.Add("class", "disabled")
            liFirst.Attributes.Add("class", "disabled")
            liLast.Attributes.Add("class", "disabled")
        End If

        If Me.CurPage = iLastPage Then
            hplMoveNext.Enabled = False
            hplMoveNext.CssClass = hplMoveNext.CssClass & "disabled"
            liNext.Attributes.Add("class", "disabled")
            hplMoveLast.Enabled = False
            hplMoveLast.CssClass = hplMoveLast.CssClass & "disabled"
            liLast.Attributes.Add("class", "disabled")
        End If

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



    End Sub
End Class
