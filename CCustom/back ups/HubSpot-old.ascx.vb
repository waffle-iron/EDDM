
Partial Class CCustom_HubSpot
    Inherits CLibraryBase

    Private _PortalId As String = ""
    Public Property PortalId As String
        Get
            Return _PortalId
        End Get
        Set(value As String)
            _PortalId = value
        End Set
    End Property

    Private _PPA As String = ""
    Public Property PPA As String
        Get
            Return _PPA
        End Get
        Set(value As String)
            _PPA = value
        End Set
    End Property



    Protected Overrides Sub BuildControl()

        Dim oJs As New StringBuilder

        oJs.AppendLine("<!-- HUBSPOT injection -->")
        oJs.AppendLine("(function(d,s,i,r) {")
        oJs.AppendLine("    if (d.getElementById(i)){return;}")
        oJs.AppendLine("    var n=d.createElement(s),e=d.getElementsByTagName(s)[0];")
        oJs.AppendLine("    n.id=i;n.src='//js.hubspot.com/analytics/'+(Math.ceil(new Date()/r)*r)+'/" & Me.PortalId & ".js';")
        oJs.AppendLine("    e.parentNode.insertBefore(n, e);")
        oJs.AppendLine("})(document,""script"",""hs-analytics"",300000);")

        oJs.AppendLine("(function(d,s,i,r) {")
        oJs.AppendLine("    if (d.getElementById(i)){return;}")
        oJs.AppendLine("    var n=d.createElement(s),e=d.getElementsByTagName(s)[0];")
        oJs.AppendLine("    n.id=i;n.src='//fast.wistia.com/static/integrations-hubspot-v1.js';")
        oJs.AppendLine("    e.parentNode.insertBefore(n, e);")
        oJs.AppendLine("})(document,""script"",""wistia-analytics"",300000);")

        'oJs.AppendLine("var hs_portalid=" & Me.PortalId & ";")
        'oJs.AppendLine("var hs_salog_version = '2.00';")
        'oJs.AppendLine("var hs_ppa = '" & PPA & "';")
        'oJs.AppendLine("document.write(unescape('%3Cscript src=""' + document.location.protocol + '//' + hs_ppa + '/salog.js.aspx"" type=""text/javascript""%3E%3C/script%3E'));")

        'oJs.AppendLine("(function() {")
        'oJs.AppendLine("var p = document.createElement('script');")
        'oJs.AppendLine("p.type = 'text/javascript'; p.async = true;")
        'oJs.AppendLine("p.src = '//d2f7h8c8hc0u7y.cloudfront.net/performable/pax/3dxjbQ.js';")
        'oJs.AppendLine("var s = document.getElementsByTagName('script')[0];")
        'oJs.AppendLine("s.parentNode.insertBefore(p, s);")
        'oJs.AppendLine("})();")

        jqueryHelper.RegisterStartupScript(Page, "HubspotTracking", oJs.ToString)

    End Sub


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Dim currentMode As String = appxCMS.Util.AppSettings.GetString("Environment").ToLower()

        If (currentMode <> "dev") Then
            BuildControl()
        End If

    End Sub


End Class
