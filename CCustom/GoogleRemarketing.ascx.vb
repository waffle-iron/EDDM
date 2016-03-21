
Imports appxCMS.Util

Partial Class CCustom_GoogleRemarketing
    Inherits CLibraryBase

    '======================================================================================================================
    'To use on EDDM Site (1) ONLY.
    '======================================================================================================================


    Private _googleConversionId As String = ""
    Public Property GoogleConversionId As String
        Get
            Return _googleConversionId
        End Get
        Set(value As String)
            _googleConversionId = value
        End Set
    End Property


    Protected Overrides Sub BuildControl()
        Dim sCurPage As String = Page.AppRelativeVirtualPath.ToLower()

        Dim bValid As Boolean = ValidDisplayPage(sCurPage)

        '-- Need to only show this control on public pages- nothing related to account, checkout, receipt, etc
        If bValid AndAlso Not String.IsNullOrEmpty(GoogleConversionId) Then

            Dim oJs As New StringBuilder
            oJs.AppendLine("<!-- Google Remarketing Injection -->")
            oJs.AppendLine("<script type=""text/javascript"">")
            oJs.AppendLine("/* <![CDATA[ */")
            oJs.AppendLine("var google_conversion_id = " & GoogleConversionId & ";")
            oJs.AppendLine("var google_custom_params = window.google_tag_params;")
            oJs.AppendLine("var google_remarketing_only = true;")
            oJs.AppendLine("/* ]]> */")
            oJs.AppendLine("</script>")
            oJs.AppendLine("<script type=""text/javascript"" src=""//www.googleadservices.com/pagead/conversion.js"">")
            oJs.AppendLine("</script>")
            oJs.AppendLine("<noscript>")
            oJs.AppendLine("<div style=""display:inline;"">")
            oJs.AppendLine("<img height=""1"" width=""1"" style=""border-style:none;"" alt="""" src=""//googleads.g.doubleclick.net/pagead/viewthroughconversion/" & GoogleConversionId & "/?value=0&amp;guid=ON&amp;script=0""/>")
            oJs.AppendLine("</div>")
            oJs.AppendLine("</noscript>")

            Page.Form.Controls.Add(New LiteralControl(oJs.ToString()))

        End If
    End Sub


    Private Function ValidDisplayPage(sPage As String) As Boolean
        Dim bRet As Boolean = True

        If sPage.StartsWith("~/myaccount/") Then
            Return False
        End If

        If sPage.StartsWith("~/account_") Then
            Return False
        End If

        If sPage.StartsWith("~/documentgen") Then
            Return False
        End If

        If sPage.Contains("checkout") Or sPage.Contains("receipt") Or sPage.Contains("profile") Then
            Return False
        End If

        Return bRet
    End Function


    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

        Dim currentMode As String = appxCMS.Util.AppSettings.GetString("Environment").ToLower()

        If (currentMode <> "dev") Then
            BuildControl()
        End If

    End Sub

End Class
