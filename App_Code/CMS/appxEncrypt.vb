Imports System.Text

Namespace appxEncrypt

    Public Class XOrCrypto
        Public Property Key() As String
            Get
                Return _Key
            End Get
            Set(ByVal value As String)
                _Key = value
            End Set
        End Property
        Private _Key As String = "5"

        Public Function Decode(ByVal DataIn As String) As String
            Dim intXOrValue1 As Integer = 0
            Dim intXOrValue2 As Integer = 0
            Dim oSb As New StringBuilder

            For lonDataPtr As Long = 0 To (DataIn.Length / 2) - 1
                'The first value to be XOr-ed comes from the data to be encrypted
                intXOrValue1 = Val("&H" & DataIn.Substring((2 * lonDataPtr), 2))
                'The second value comes from the code key
                intXOrValue2 = Asc(Key.Substring(((lonDataPtr Mod Key.Length)), 1))
                oSb.Append(Chr(intXOrValue1 Xor intXOrValue2))
            Next lonDataPtr

            Return oSb.ToString
        End Function

        Public Function Encode(ByVal DataIn As String) As String
            Dim lonDataPtr As Long
            Dim strDataOut As String
            Dim temp As Integer
            Dim tempstring As String
            Dim intXOrValue1 As Integer
            Dim intXOrValue2 As Integer
            Dim oSb As New StringBuilder

            For lonDataPtr = 0 To DataIn.Length - 1
                'The first value to be XOr-ed comes from the data to be encrypted
                intXOrValue1 = Asc(DataIn.Substring(lonDataPtr, 1))
                'The second value comes from the code key
                intXOrValue2 = Asc(Key.Substring(((lonDataPtr Mod Key.Length)), 1))

                temp = (intXOrValue1 Xor intXOrValue2)
                tempstring = Hex(temp)
                If Len(tempstring) = 1 Then tempstring = "0" & tempstring
                oSb.Append(tempstring)
            Next lonDataPtr
            strDataOut = oSb.ToString
            Return strDataOut
        End Function

    End Class

End Namespace
