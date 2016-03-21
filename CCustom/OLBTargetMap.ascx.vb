
Partial Class CCustom_EDDMTargetMap
    Inherits CLibraryBase


    Protected ReadOnly Property USelectId As Integer
        Get
            Return 1
        End Get
    End Property


    Protected ReadOnly Property DistributionId As Integer
        Get
            Return Request.QueryString("d")

            ''Return appxCMS.Util.Querystring.GetInteger("distid")
            ''Dim iRet As Integer = 0
            ''If dRequest.ContainsKey("distid") Then
            ''    iRet = dRequest("distid")
            ''End If
            ''Return iRet

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

            '==============================================================================================================
            'NOTES: 
            ' I believe this property logic to be obsolete within this control. The exitURL prop is set 
            ' within the TargetDataMap2 page.
            ' 3/19/2015. DSF
            '==============================================================================================================
            'If String.IsNullOrEmpty(_ExitUrl) Then

            '    Dim sLoc As String = appxCMS.Util.Querystring.GetString("l")
            '    Dim imp As Integer = appxCMS.Util.Querystring.GetInteger("i")
            '    Dim campaign As String = appxCMS.Util.Querystring.GetString("c")
            '    Dim qty As Integer = appxCMS.Util.Querystring.GetInteger("q")
            '    Dim budget As Integer = appxCMS.Util.Querystring.GetInteger("b")
            '    Dim frequency As Integer = appxCMS.Util.Querystring.GetInteger("f")
            '    Dim startDate As String = appxCMS.Util.Querystring.GetString("s")
            '    Dim franchiseBrand As String = appxCMS.Util.Querystring.GetString("t")

            '    _ExitUrl = appxCMS.Util.urlHelp.AppRelativeToFullyQualified("~/resources/OLBUSelectEDDMExitHandler.ashx?referenceid={0}&uselectid=" & Me.USelectId & "&l=" & Server.UrlEncode(sLoc) & "&i=" & imp & "&c=" & Server.UrlEncode(campaign) & "&q=" & qty.ToString() & "&b=" & budget.ToString() & "&f=" & frequency.ToString() & "&s=" & Server.UrlEncode(startDate) & "&t=" & Server.UrlEncode(franchiseBrand))

            'End If
            Return _ExitUrl
        End Get
        Set(value As String)
            _ExitUrl = value
        End Set
    End Property

    Protected dRequest As Hashtable = appxCMS.PageBase.RebuildQuerystring(apphelp.GetRequestedURL(), False)

    Protected Overrides Sub BuildControl()

        ''If Not Request.IsAuthenticated Then
        ''    Dim sRedirect As String = Page.AppRelativeVirtualPath & "?distid=" & Me.DistributionId
        ''    Response.Redirect("~/account_signin.aspx?ReturnUrl=" & Server.UrlEncode(sRedirect))
        ''End If

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
            End If
        End If

        Dim oUSelect As Taradel.USelectMethod = Taradel.Helper.USelect.GetById(Me.USelectId)
        If oUSelect IsNot Nothing Then
            Dim sBaseUrl As String = oUSelect.ConfigurationUrl
            Dim sRedirect As String = Me.ExitUrl '"http://" & Request.Url.Host & "/resources/USelectEDDMExitHandler.ashx?referenceid={0}&uselectid=" & Me.USelectId
            Dim sMapUrl As String = sBaseUrl & "?saveredirect=" & Server.UrlEncode(sRedirect) & "&distid=" & Me.DistributionId & "&addr=" & Me.Address & "&zip=" & Me.ZipCode & "&refid=" & Server.UrlEncode(sRefId) & "&nc=" & Now.Ticks.ToString()
            'lMap.Text = "<div""><iframe id=""frmUSelect"" style=""width:100%;"" width=""100%"" height=""667"" scrolling=""no"" border=""0"" frameborder=""0"" src=""" & sMapUrl & """>Initializing...</iframe></div>"
            lMap.Text = "<div class=""embed-responsive embed-responsive-4by3""><iframe id=""frmUSelect"" scrolling=""no"" border=""0"" src=""" & sMapUrl & """>Initializing...</iframe></div>"


        End If
    End Sub

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        BuildControl()
    End Sub
End Class
