Imports System.Collections.Generic
Imports System.Linq

Partial Class CLibrary_NewsBlogPostsByMonth
    Inherits CLibraryBase
    
    Private _sectionHeader As String = "Archive"
    Public Property SectionHeader As String
        Get
            Return _sectionHeader
        End Get
        Set(value As String)
            _sectionHeader = value
        End Set
    End Property

    Protected oSummary As List(Of ArchiveSummary) = Nothing
    Protected Overrides Sub BuildControl()
        lHeaderText.Text = "<b>" & SectionHeader & "</b>"


        Dim siteId As Integer = appxCMS.Util.CMSSettings.GetSiteId

        Using oDb As New appxCMS.appxCMSEntities
            oSummary = (From p In oDb.NewsBlogPosts _
                        Where p.Published = True _
                        And p.PublishDate <= DateTime.Now _
                        And p.SiteId = siteId _
                        Group By pMonth = p.PublishDate.Month, _
                        pYear = p.PublishDate.Year _
                        Into g = Group _
                        Order By pYear Descending, pMonth Descending _
                        Select New ArchiveSummary With { _
                            .Month = pMonth, _
                            .Year = pYear, _
                            .Count = g.Count() _
                            }).ToList()
        End Using

        Dim oYears As List(Of Integer) = oSummary.OrderByDescending(Function(s) s.Year).Select(Function(s) s.Year).Distinct().ToList()
        If oYears.Count > 1 Then
            lvYears.DataSource = oYears
            lvYears.DataBind()
        Else
            lvPosts.DataSource = oSummary
            lvPosts.DataBind()
        End If
    End Sub

    Protected Sub lvYears_OnItemDataBound(ByVal sender As Object, ByVal e As ListViewItemEventArgs) Handles lvYears.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim oDItem As ListViewDataItem = e.Item

            Dim year As Integer = CType(oDItem.DataItem, Integer)

            Dim pYear As Panel = DirectCast(e.Item.FindControl("pYear"), Panel)
            Dim hplYear As HyperLink = DirectCast(e.Item.FindControl("hplYear"), HyperLink)
            hplYear.Attributes.Add("href", "#" & pYear.ClientID)

            Dim oMonths As List(Of ArchiveSummary) = oSummary.Where(Function(s) s.Year = year).ToList()
            Dim lvPosts As ListView = DirectCast(e.Item.FindControl("lvPosts"), ListView)
            lvPosts.DataSource = oMonths
            lvPosts.DataBind()
        End If
    End Sub


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Class ArchiveSummary
        Private _month As Integer
        Public Property Month As Integer
            Get
                Return _month
            End Get
            Set(value As Integer)
                _month = value
            End Set
        End Property

        Private _year As Integer
        Public Property Year As Integer
            Get
                Return _year
            End Get
            Set(value As Integer)
                _year = value
            End Set
        End Property

        Public ReadOnly Property Name As String
            Get
                Dim dRet As New DateTime(Year, Month, 1)
                Return dRet.ToString("MMMM yyyy")
            End Get
        End Property

        Private _count As Integer
        Public Property Count As Integer
            Get
                Return _count
            End Get
            Set(value As Integer)
                _count = value
            End Set
        End Property

        Public Sub New()

        End Sub
    End Class

End Class
