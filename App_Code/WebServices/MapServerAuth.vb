Imports System.ServiceModel
Imports System.ServiceModel.Activation

<ServiceContract(Namespace:="")> _
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)> _
Public Class MapServerAuth

    <OperationContract()> _
    Public Function IsAuthenticated() As Boolean
        Dim bRet As Boolean = False
        If HttpContext.Current.Request.IsAuthenticated Then
            bRet = True
        End If
        Return bRet
    End Function
End Class
