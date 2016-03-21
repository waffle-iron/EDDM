<%@ WebHandler Language="VB" Class="OLBUSelectEDDMExitHandler" %>

Imports System
Imports System.Web
Imports System.Collections.Generic
Imports System.Text

Public Class OLBUSelectEDDMExitHandler : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/html"

        Dim sReferenceId As String = pageBase.QStringToVal("referenceid")
        Dim uSelectId As Integer = pageBase.QStringToInt("uselectid")

        '-- These declarations should correspond to the values that were sent into the Map control in OLBTargetMap.ascx
        '10 params so far....
        'd = distID
        'l = location
        'i = impressions
        'c = campaign 
        'q = qty
        'b = budget
        'f = frequency (weeks)
        's = start date
        'p = productID
        'bp = baseProductID
        't = type, aka Franchise Type
        
        

        Dim sLoc As String = appxCMS.Util.Querystring.GetString("l")
        Dim imp As Integer = appxCMS.Util.Querystring.GetInteger("i")
        Dim campaign As String = appxCMS.Util.Querystring.GetString("c")
        Dim qty As Integer = appxCMS.Util.Querystring.GetInteger("q")
        Dim budget As Integer = appxCMS.Util.Querystring.GetInteger("b")
        Dim frequency As Integer = appxCMS.Util.Querystring.GetInteger("f")
        Dim startDate As String = appxCMS.Util.Querystring.GetString("s")
        Dim productID As Integer = appxCMS.Util.Querystring.GetInteger("p")
        Dim baseProductID As Integer = appxCMS.Util.Querystring.GetInteger("bp")
        Dim franchiseBrand As String = appxCMS.Util.Querystring.GetString("t")

        Dim oExitArgs As New SortedList(Of String, String)
        oExitArgs.Add("l", sLoc)
        oExitArgs.Add("i", imp.ToString())
        oExitArgs.Add("p", productID.ToString())
        oExitArgs.Add("bp", baseProductID.ToString())
        oExitArgs.Add("c", campaign.ToString())
        oExitArgs.Add("q", qty.ToString())
        oExitArgs.Add("b", budget.ToString())
        oExitArgs.Add("f", frequency.ToString())
        oExitArgs.Add("s", startDate)
        oExitArgs.Add("t", franchiseBrand)

        
        Dim oUSelect As Taradel.USelectMethod = Taradel.Helper.USelect.GetById(uSelectId)
        If oUSelect IsNot Nothing Then
            Dim customerId As Integer = 0
            Dim sMsg As String = ""
            If context.Request.IsAuthenticated Then
                customerId = Taradel.Customers.GetCustomerId(context.User.Identity.Name, sMsg)
            Else
                customerId = Taradel.Customers.CreateAnonymous(sMsg)
            End If
                       
	           
            Dim distributionId As Integer = 0
            If customerId > 0 Then
                Try
                    distributionId = Taradel.CustomerDistributions.Save(customerId, sReferenceId, uSelectId, sMsg)
                    context.Response.Write("<!-- New Dist ID " & distributionId & " -->")
                    context.Response.Write("<!-- Msg " & sMsg & " -->")
                Catch ex As Exception
                    context.Response.Write("<!-- " & ex.Message & " -->")
                End Try                
            Else
                context.Response.Write("Customer information could not be located.")
            End If
            
            'This is a user updating their map.
            If distributionId > 0 Then
                
                oExitArgs.Add("d", distributionId.ToString())
                
                'r = revised (even though a new DistID is created).
                oExitArgs.Add("r", "true")
                
                Dim sExitUrl As String = VirtualPathUtility.ToAbsolute("~/OLB/TargetDataMap2.aspx")
                Dim sQuery As String = ""
                Dim oExitArgKVP As New List(Of String)
                
                For Each sKey As String In oExitArgs.Keys
                    oExitArgKVP.Add(sKey & "=" & context.Server.UrlEncode(oExitArgs(sKey)))
                Next
                
                sQuery = String.Join("&", oExitArgKVP.ToArray())
                
                If Not String.IsNullOrEmpty(sQuery) Then
                    sExitUrl = sExitUrl & "?" & sQuery
                End If
                
                Dim oSb As New StringBuilder
                oSb.AppendLine("<html>")
                oSb.AppendLine("<head>")
                oSb.AppendLine("<base target=""_parent"" />")
                oSb.AppendLine("<script type=""text/javascript"">")
                oSb.AppendLine("function initUSelectSave() {")
                oSb.AppendLine("    window.top.location.href='" & sExitUrl & "';")
                oSb.AppendLine("}")
                oSb.AppendLine("</script>")
                oSb.AppendLine("</head>")
                oSb.AppendLine("<body onload=""initUSelectSave();"">")
                oSb.AppendLine("</body>")
                oSb.AppendLine("</html>")
           
                context.Response.Write(oSb.ToString)
            Else
                context.Response.Write("Distribution was not saved.")
                context.Response.Write("<!-- " & customerId & "-->")
                context.Response.Write("<!-- " & sReferenceId & "-->")
                context.Response.Write("<!-- " & uSelectId & "-->")
                context.Response.Write("<!-- " & sMsg & "-->")
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