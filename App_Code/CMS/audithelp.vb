Public Class audithelp
    Public Shared Function GetAuditRow(ByVal sTable As String, ByVal sKeyField As String, ByVal sKey As Integer) As String
        Dim oSB As New StringBuilder
        Using oA As New appxAuditTableAdapters.GetXMLDataTableAdapter
            Using oT As appxAudit.GetXMLDataDataTable = oA.GetData(sTable, sKeyField, sKey)
                For Each oRow As appxAudit.GetXMLDataRow In oT.Rows
                    oSB.Append(oRow.Item(0))
                Next
            End Using
        End Using
        Return oSB.ToString
    End Function

    Public Shared Sub AuditChange(ByVal sTable As String, ByVal sKeyField As String, ByVal sKey As Integer, ByVal sAction As String, ByVal sUser As String, ByVal sUserID As Integer)
        Dim sAuditData As String = audithelp.GetAuditRow(sTable, sKeyField, sKey)
        Using oA As New appxAuditTableAdapters.ChangeTableAdapter
            oA.GetData(sTable, sKeyField, sKey, sAction, sUser, sUserID, sAuditData)
        End Using
    End Sub
End Class
