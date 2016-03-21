
Imports Taradel

Partial Class CCustom_EDDMTargetMap
    Inherits CLibraryBase

    Protected Property USelectId As Integer
    'Get
    '    '-- Cannot be hard-coded, has to be based on dist id
    '    'Return 1 '-- Hardcode to EDDM Carrier Route selection map
    'End Get
    'End Property

    Protected ReadOnly Property DistributionId As Integer
        Get
            Return appxCMS.Util.Querystring.GetInteger("distid")
        End Get
    End Property

    Protected ReadOnly Property Address As String
        Get
            Return appxCMS.Util.Querystring.GetString("addr")
        End Get
    End Property

    Protected ReadOnly Property ZipCode As String
        Get
            Return appxCMS.Util.Querystring.GetString("zip")
        End Get
    End Property

    Private _ExitUrl As String = ""
    Public Property ExitUrl As String
        Get
            If String.IsNullOrEmpty(_ExitUrl) Then
                _ExitUrl = appxCMS.Util.urlHelp.AppRelativeToFullyQualified("~/resources/USelectEDDMExitHandler.ashx?referenceid={0}&uselectid=" & Me.USelectId)
            End If
            Return _ExitUrl
        End Get
        Set(value As String)
            _ExitUrl = value
        End Set
    End Property

    Protected dRequest As Hashtable = appxCMS.PageBase.RebuildQuerystring(apphelp.GetRequestedURL(), False)

    Protected Overrides Sub BuildControl()

        'If Not Request.IsAuthenticated Then
        '    Dim sRedirect As String = Page.AppRelativeVirtualPath & "?distid=" & Me.DistributionId
        '    Response.Redirect("~/account_signin.aspx?ReturnUrl=" & Server.UrlEncode(sRedirect))
        'End If

        If Request.IsSecureConnection Then
            '-- Convert our current page request to Non-SSL
            Dim oUri As New UriBuilder(Request.Url)
            oUri.Scheme = "http"
            oUri.Port = -1
            Response.Redirect(oUri.ToString)
        End If

        Dim sRefId As String = ""
        If Me.DistributionId > 0 Then
            '-- Get the reference id for this dist
            Dim oDist As Taradel.CustomerDistribution = Taradel.CustomerDistributions.GetDistribution(Me.DistributionId)
            If oDist IsNot Nothing Then
                sRefId = oDist.ReferenceId
                Me.USelectId = oDist.USelectMethodReference.ForeignKey()
            End If
        Else
            Me.USelectId = 1
        End If

        Dim oUSelect As Taradel.USelectMethod = Taradel.Helper.USelect.GetById(Me.USelectId)

        If oUSelect IsNot Nothing Then

            Dim siteId As Integer = appxCMS.Util.CMSSettings.GetSiteId()

            Dim bEnableLock As Boolean = False

            Dim bLockExclusive As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Route", "OffersExclusiveRoutes", siteId)
            Dim bLockTerritory As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Route", "UseExclusiveTerritories", siteId)

            If bLockExclusive OrElse bLockTerritory Then
                bEnableLock = True
            End If

            Dim sBaseUrl As String = oUSelect.ConfigurationUrl

            If USelectId = 1 AndAlso bEnableLock Then
                If sBaseUrl.EndsWith("/") Then
                    sBaseUrl = sBaseUrl.Substring(0, sBaseUrl.Length - 1)
                End If
                sBaseUrl = sBaseUrl & "Lock/"
            End If

            Dim sRedirect As String = Me.ExitUrl '"http://" & Request.Url.Host & "/resources/USelectEDDMExitHandler.ashx?referenceid={0}&uselectid=" & Me.USelectId
            Dim sMapUrl As String = sBaseUrl & "?saveredirect=" & Server.UrlEncode(sRedirect) & "&distid=" & Me.DistributionId & "&addr=" & Me.Address & "&zip=" & Me.ZipCode & "&refid=" & Server.UrlEncode(sRefId) & "&nc=" & Now.Ticks.ToString()

            'lMap.Text = "<div""><iframe id=""frmUSelect"" style=""width:100%;"" width=""100%"" height=""600"" scrolling=""no"" border=""0"" frameborder=""0"" src=""" & sMapUrl & """>Initializing...</iframe></div>"

            Dim iframeOutput As New StringBuilder
            iframeOutput.Append("<iframe id=""frmUSelect"" style=""width:100%;"" ")
            iframeOutput.Append("height=""600"" scrolling=""no"" ")
            iframeOutput.Append("border=""0"" frameborder=""0"" ")
            iframeOutput.Append("src=""" & sMapUrl & """>Initializing..................</iframe>")

            lMap.Text = iframeOutput.ToString()


        End If

    End Sub

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        BuildControl()
    End Sub

End Class
