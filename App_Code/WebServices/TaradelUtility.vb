Imports System.ServiceModel
Imports System.ServiceModel.Activation

<ServiceContract(Namespace:="http://www.taradel.com")> _
<SilverLightFaultBehavior()> _
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)> _
Public Class TaradelUtility

    <OperationContract()> _
    Public Function AssociationCodeIsValid(ByVal sCode As String) As Integer
        Dim iRet As Integer = 0
        If Not String.IsNullOrEmpty(sCode) Then
            Dim bValid As Boolean = Taradel.AssociationDataSource.AssociationCodeIsValid(sCode)
            If bValid Then
                iRet = 1
            Else
                iRet = -1
            End If
        End If

        Return iRet
    End Function

    <OperationContract()> _
    Public Function GetUserName() As String
        Dim sUser As String = ""
        If HttpContext.Current.Request.IsAuthenticated Then
            sUser = HttpContext.Current.User.Identity.Name
        End If
        Return sUser
    End Function

    <OperationContract()> _
    Public Function IsAuthenticated() As Boolean
        Dim bRet As Boolean = False
        If HttpContext.Current.Request.IsAuthenticated Then
            bRet = True
        End If
        Return bRet
    End Function
End Class
