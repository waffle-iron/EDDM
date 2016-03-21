<%@ WebHandler Language="VB" Class="USelectEDDMExitHandler" %>

Imports System
Imports System.Web
Imports System.Data.SqlClient
Imports System.Data

Public Class USelectEDDMExitHandler : Implements IHttpHandler
    
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
                If USelectId = 1 Then
                    DistributionId = Taradel.CustomerDistributions.Save(CustomerId, sReferenceId, USelectId, sMsg)
                    
                Else
                    Using oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString)
                        oConn.Open()
                    
                        Using oCmd As New SqlCommand("usp_InsertCustomerDistribution", oConn)
                            oCmd.Parameters.AddWithValue("@CustomerId", CustomerId)
                            oCmd.Parameters.AddWithValue("@DistName", pageBase.QStringToVal("savedName"))
                            oCmd.Parameters.AddWithValue("@DistRefId", sReferenceId)
                            oCmd.Parameters.AddWithValue("@TotalDeliveries", pageBase.QStringToInt("savedCount"))
                            oCmd.Parameters.AddWithValue("@USelectId", USelectId)
                            oCmd.Parameters.Add("@OutputID", SqlDbType.Int).Direction = ParameterDirection.Output
                            oCmd.CommandType = CommandType.StoredProcedure
                            oCmd.ExecuteScalar()
                            DistributionId = Convert.ToInt32(oCmd.Parameters("@OutputID").Value)                        
                        End Using
                    End Using
                End If
                
                
                '
                
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
                oSb.AppendLine("    window.top.location.href='" & VirtualPathUtility.ToAbsolute("~/Step1-TargetReview.aspx") & "?distid=" & DistributionId & "';")
                oSb.AppendLine("}")
                oSb.AppendLine("</script>")
                oSb.AppendLine("</head>")
                oSb.AppendLine("<body onload=""initUSelectSave();"">")
                oSb.AppendLine("</body>")
                oSb.AppendLine("</html>")
           
                context.Response.Write(oSb.ToString)
            Else
                context.Response.Write("Distribution was not saved.")
                context.Response.Write("<!-- " & CustomerId & "-->")
                context.Response.Write("<!-- " & sReferenceId & "-->")
                context.Response.Write("<!-- " & USelectId & "-->")
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