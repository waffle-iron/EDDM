
Partial Class CCustom_RingRevenueButton
    Inherits CLibraryBase

    Private _RingRevenueId As String = "" '20661
    Public Property RingRevenueId As String
        Get
            Return _RingRevenueId
        End Get
        Set(value As String)
            _RingRevenueId = value
        End Set
    End Property

    Private _CampaignId As String = "" '19009
    Public Property CampaignId As String
        Get
            Return _CampaignId
        End Get
        Set(value As String)
            _CampaignId = value
        End Set
    End Property

    Private _RingPoolId As String = "" '3236
    Public Property RingPoolId As String
        Get
            Return _RingPoolId
        End Get
        Set(value As String)
            _RingPoolId = value
        End Set
    End Property

    Private _PhoneNumber As String = "" '3236
    Public Property PhoneNumber As String
        Get
            Return _PhoneNumber
        End Get
        Set(value As String)
            _PhoneNumber = value
        End Set
    End Property

    Private _ImageUrl As String = ""
    Public Property ImageUrl As String
        Get
            Return _ImageUrl
        End Get
        Set(value As String)
            _ImageUrl = value
        End Set
    End Property

    '    <!-- Begin RingRevenue Call Tracking Code -->
    '<!-- Omit http from path to ensure protocol is same as current request -->
    '<script src="//js5.ringrevenue.com/5/integration.js"></script>
    '<script type="text/javascript">
    '  RingRevenue.advertiser_integration = {
    '    id : '20661',
    '    numberSelector: '.promoNumber',
    '    numberToReplace: '804-364-8444',
    '    campaignId: '19009',
    '    ringPoolId: '3236'
    '  };
    '</script>
    '<!-- End RingRevenue Call Tracking Code -->

    Protected Overrides Sub BuildControl()
        If Not String.IsNullOrEmpty(Me.PhoneNumber) Then
            If Not String.IsNullOrEmpty(Me.ImageUrl) Then
                pPhone.BackImageUrl = Me.ImageUrl
            End If
            lPhone.Text = Me.PhoneNumber
            jqueryHelper.IncludePlugin(Page, "RingRevenue", "//js5.ringrevenue.com/5/integration.js")

            Dim oJs As New StringBuilder
            oJs.AppendLine("RingRevenue.advertiser_integration = {")
            oJs.AppendLine("    id : '" & Me.RingRevenueId & "',")
            oJs.AppendLine("    numberSelector: '.promoNumber',")
            oJs.AppendLine("    numberToReplace: '" & Me.PhoneNumber & "',")
            oJs.AppendLine("    campaignId: '" & Me.CampaignId & "',")
            oJs.AppendLine("    ringPoolId: '" & Me.RingPoolId & "'")
            oJs.AppendLine("};")
            jqueryHelper.RegisterStartupScript(Page, Me.ClientID & "Init", oJs.ToString)
        Else
            Me.Visible = False
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub
End Class
