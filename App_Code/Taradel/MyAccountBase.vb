Imports Microsoft.VisualBasic

Public Class MyAccountBase
    Inherits appxCMS.PageBase

    Public ReadOnly Property GetCustomerId As Integer
        Get
            Dim CustomerId As Integer = 0
            Dim sUser As String = HttpContext.Current.User.Identity.Name

            Dim sKey As String = "appxAuth:" & sUser & ":Id"
            If appxCMS.Util.Cache.Exists(sKey) Then
                CustomerId = appxCMS.Util.Cache.GetInteger(sKey)
            Else
                Dim oUser As Taradel.Customer = Taradel.CustomerDataSource.GetCustomer(sUser)
                If oUser IsNot Nothing Then
                    CustomerId = oUser.CustomerID
                    '-- Cache the value
                    appxCMS.Util.Cache.Add(sKey, CustomerId)
                End If
            End If
            Return CustomerId
        End Get
    End Property
End Class
