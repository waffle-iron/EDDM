<%@ WebHandler Language="VB" Class="USelectExitHandler" %>

Imports System
Imports System.Web

Public Class USelectExitHandler : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/html"

        Dim sReferenceId As String = pageBase.QStringToVal("referenceid")
        Dim USelectId As Integer = pageBase.QStringToInt("uselectid")
        
        Dim oUSelect As Taradel.USelectMethod = Taradel.Helper.USelect.GetById(USelectId)
        If oUSelect IsNot Nothing Then
            Dim CustomerId As Integer = 0
            Dim sMsg As String = ""
            If context.Request.IsAuthenticated Then
                CustomerId = Taradel.Customers.GetCustomerId(context.User.Identity.Name, sMsg)
            Else
                CustomerId = Taradel.Customers.CreateAnonymous(sMsg)
            End If
	           
            Dim DistributionId As Integer = 0
            If CustomerId > 0 Then
                DistributionId = Taradel.CustomerDistributions.Save(CustomerId, sReferenceId, USelectId, sMsg)
            Else
                context.Response.Write("Customer information could not be located.")
            End If
            
            If DistributionId > 0 Then
                Dim oSb As New StringBuilder
                oSb.AppendLine("<html>")
                oSb.AppendLine("<head>")
                oSb.AppendLine("<base target=""_parent"" />")
                oSb.AppendLine("<script type=""text/javascript"">")
                oSb.AppendLine("function initUSelectSave() {")
                oSb.AppendLine("    window.top.location.href='" & linkHelp.SEOLink(oUSelect.Name, USelectId & "-" & DistributionId, linkHelp.LinkType.USelectNextSteps) & "';")
                oSb.AppendLine("}")
                oSb.AppendLine("</script>")
                oSb.AppendLine("</head>")
                oSb.AppendLine("<body onload=""initUSelectSave();"">")
                oSb.AppendLine("</body>")
                oSb.AppendLine("</html>")
           
                context.Response.Write(oSb.ToString)
            Else
                context.Response.Write("Distribution was not saved.")
            End If
        Else
            context.Response.Write("Unable to locate selected USelect product line.")
        End If
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class