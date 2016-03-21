
Partial Class CLibrary_BlogTagCloud
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

    Public Property CssClass As String
        Get
            Return pTags.CssClass
        End Get
        Set(value As String)
            pTags.CssClass = value
        End Set
    End Property

    Dim dMinWt As Integer = Integer.MaxValue
    Dim dMaxWt As Integer = Integer.MinValue
    Dim FontScale As String() = {"h6", "h5", "h4", "h3", "h2", "h1"}
    Dim iCurTagGroupId As Integer = 0
    Dim dScaleUnitLen As Decimal = 0

    Protected Overrides Sub BuildControl()
        If String.IsNullOrEmpty(SectionHeader) Then
            phPanelHeader.Visible = False
        Else
            lHeaderText.Text = SectionHeader
        End If

        Dim oTagList = appxCMS.NewsBlogDataSource.GetTagCloudItems()
        If oTagList.Count > 0 Then
            For Each oTag In oTagList
                Dim iCount As Integer = oTag.Count
                If iCount > 0 Then
                    If iCount < dMinWt Then dMinWt = iCount
                    If iCount > dMaxWt Then dMaxWt = iCount
                End If
            Next
            dScaleUnitLen = (dMaxWt - dMinWt + 1) / Convert.ToDecimal(FontScale.Length)
        End If

        lvTagCloud.DataSource = oTagList
        lvTagCloud.DataBind()
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Sub lvTagCloud_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvTagCloud.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim oDItem As ListViewDataItem = e.Item

            Dim oTag As appxCMS.TagCloudEntry = DirectCast(oDItem.DataItem, appxCMS.TagCloudEntry)

            Dim dCount As Decimal = CType((oTag.Count * 1.0), Decimal)
            Dim iScale As Integer = Math.Truncate((dCount - dMinWt) / dScaleUnitLen)
            Dim lnkTag As HyperLink = DirectCast(e.Item.FindControl("hplTag"), HyperLink)
            lnkTag.CssClass = FontScale(iScale) & " text-nowrap"
            'lnkTag.Style.Add("font-size", FontScale(iScale))
        End If
    End Sub
End Class
