Imports System.Collections
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.XSL
Imports System.Data
Imports System.IO

Public Class xmlhelp
    Public Shared ReadOnly Property UCaseAlpha() As String
        Get
            Return LCaseAlpha.ToUpper
        End Get
    End Property

    Public Shared ReadOnly Property LCaseAlpha() As String
        Get
            Return "abcdefghijklmnopqrstuvwxyz"
        End Get
    End Property

    Public Shared Function ReadAttributeValue(ByVal oNode As XmlNode, ByVal sAttr As String) As String
        Dim sVal As String = ""
        If oNode IsNot Nothing Then
            Dim oAttr As XmlNode = oNode.Attributes.GetNamedItem(sAttr)
            If oAttr IsNot Nothing Then
                sVal = oAttr.Value
            End If
        End If
        Return sVal
    End Function

    Public Shared Function SortNodeList(ByVal nodeList As XmlNodeList, ByVal aKeyAttribute As ArrayList) As XmlNodeList

        Dim list As New SortedList
        For Each v As XmlElement In nodeList
            Dim sKey As String
            Dim oKeySB As New StringBuilder
            Try
                For iKey As Integer = 0 To aKeyAttribute.Count - 1
                    Dim sTmp As String = xmlhelp.ReadAttribute(v, aKeyAttribute(iKey))
                    oKeySB.Append(sTmp)
                Next
                sKey = oKeySB.ToString

                If list.IndexOfKey(sKey) >= 0 Then
                    '-- It already exists
                    Dim sDupeTmp As String = list.Item(sKey)
                    list.Remove(sKey)
                    list.Add(sKey, sDupeTmp & v.OuterXml.ToString)
                Else
                    ' Key is type String (default behavior)
                    list.Add(sKey, v.OuterXml.ToString)
                End If
            Catch ex As Exception
                HttpContext.Current.Response.Write(ex.Message & "<br/>" & ex.StackTrace & "<br/>")
            End Try
        Next

        Dim oSb As New StringBuilder

        Dim oLEnum As IDictionaryEnumerator = list.GetEnumerator
        While oLEnum.MoveNext
            oSb.Append(oLEnum.Value)
        End While

        Dim sXML As String = "<xsorteddata>" & oSb.ToString & "</xsorteddata>"
        Dim oXML As New XmlDocument
        oXML.LoadXml(sXML)
        Dim oList As XmlNodeList = oXML.SelectNodes("/xsorteddata/*")

        Return oList
    End Function

    Public Shared Function SortNodeList(ByVal nodeList As XmlNodeList, ByVal sKeyAttribute As String, _
         Optional ByVal ty As TypeCode = TypeCode.String) As XmlNodeList

        Dim list As New SortedList
        For Each v As XmlElement In nodeList
            ' Select the user-specified attribute as the key:
            Dim sKey As String

            Try
                sKey = xmlhelp.ReadAttribute(v, sKeyAttribute)
                'sKey = CType(v, XmlElement).Attributes(sKeyAttribute).Value

                Dim oKey As Object = Nothing
                Try
                    oKey = Convert.ChangeType(sKey, ty)
                Catch ex As Exception
                    oKey = sKey
                End Try

                If list.IndexOfKey(oKey) >= 0 Then
                    '-- It already exists
                    Dim sDupeTmp As String = list.Item(oKey)
                    list.Remove(oKey)
                    list.Add(oKey, sDupeTmp & v.OuterXml.ToString)
                    'list.Item(Convert.ChangeType(sKey, ty)) = sDupeTmp & v.OuterXml.ToString
                Else
                    If ty = VariantType.String Then
                        ' Key is type String (default behavior)
                        list.Add(oKey, v.OuterXml.ToString)
                    Else
                        ' Convert value to type specified by caller:
                        list.Add(oKey, v.OuterXml.ToString)
                    End If
                End If
            Catch ex As Exception
                HttpContext.Current.Response.Write(ex.Message & "<br/>" & ex.StackTrace & "<br/>")
            End Try
        Next

        Dim oSb As New StringBuilder

        Dim oLEnum As IDictionaryEnumerator = list.GetEnumerator
        While oLEnum.MoveNext
            oSb.Append(oLEnum.Value)
        End While

        Dim sXML As String = "<xsorteddata>" & oSb.ToString & "</xsorteddata>"
        Dim oXML As New XmlDocument
        oXML.LoadXml(sXML)
        Dim oList As XmlNodeList = oXML.SelectNodes("/xsorteddata/*")

        Return oList
    End Function

    Public Shared Sub AddOrUpdateXMLAttribute(ByVal oXMLNode As XmlNode, ByVal sAttrName As String, ByVal sAttrValue As String)
        If oXMLNode IsNot Nothing Then
            AddOrUpdateXMLAttribute(oXMLNode.OwnerDocument, oXMLNode, sAttrName, sAttrValue)
        End If
    End Sub

    Public Shared Sub AddOrUpdateXMLAttribute(ByRef oXMLDoc As XmlDocument, ByRef oXMLNode As XmlNode, ByVal sAttrName As String, ByVal sAttrValue As String)
        Dim oTmpAttr As XmlAttribute = oXMLDoc.CreateAttribute(sAttrName)
        oTmpAttr.Value = sAttrValue
        oXMLNode.Attributes.SetNamedItem(oTmpAttr)
    End Sub

    Public Shared Function AddOrUpdateXMLNode(ByRef oXMLNode As XmlNode, ByVal sNodeName As String, ByVal sNodeValue As String) As XmlNode
        Dim oNode As XmlNode = Nothing
        If oXMLNode IsNot Nothing Then
            Dim oXMLDoc As XmlDocument = oXMLNode.OwnerDocument
            If oXMLDoc IsNot Nothing Then
                oNode = oXMLDoc.CreateElement(sNodeName)
            End If
            If oNode IsNot Nothing Then
                If Not String.IsNullOrEmpty(sNodeValue) Then
                    oNode.InnerText = sNodeValue
                End If
                oXMLNode.AppendChild(oNode)
            End If
        End If
        Return oNode
    End Function

    Public Shared Function AddOrUpdateCDataNode(ByRef oXmlNode As XmlNode, ByVal sNodeName As String, ByVal sNodeValue As String) As XmlNode
        Dim oNode As XmlNode = Nothing
        If oXmlNode IsNot Nothing Then
            Dim oXMLDoc As XmlDocument = oXmlNode.OwnerDocument
            If oXMLDoc IsNot Nothing Then
                oNode = oXMLDoc.CreateElement(sNodeName)
            End If
            If oNode IsNot Nothing Then
                If Not String.IsNullOrEmpty(sNodeValue) Then
                    Dim oCdata As XmlNode = oXmlNode.OwnerDocument.CreateCDataSection(sNodeValue)
                    oNode.AppendChild(oCdata)
                End If
                oXmlNode.AppendChild(oNode)
            End If
        End If
        Return oNode
    End Function


    Public Shared Function ReadAttribute(ByVal oNode As XmlNode, ByVal sAttrib As String) As String
        Dim sVal As String = ""
        If oNode IsNot Nothing Then
            Dim oAttrib As XmlNode = oNode.Attributes.GetNamedItem(sAttrib)
            If oAttrib IsNot Nothing Then
                sVal = oAttrib.Value
            End If
        End If
        Return sVal
    End Function

    Public Shared Function ReadAttribute(ByVal oNode As XmlNode, ByVal sAttrib As String, ByVal sNS As String) As String
        Dim sVal As String = ""
        If oNode IsNot Nothing Then
            Dim oAttrib As XmlNode = oNode.Attributes.GetNamedItem(sAttrib, sNS)
            If oAttrib IsNot Nothing Then
            sVal = oAttrib.Value
        End If
        End If
        Return sVal
    End Function

    Public Shared Function ReadNode(ByVal oNode As XmlNode) As String
        Dim sValue As String = ""
        If oNode IsNot Nothing Then
            sValue = oNode.InnerText
        End If
        Return sValue
    End Function

    Public Shared Function InMemoryTransform(ByVal xml As String, ByVal xslPath As String) As String
        Return InMemoryTransform(xml, Nothing, xslPath)
    End Function

    Public Shared Function InMemoryTransform(ByVal xml As String, ByVal aArgs As Hashtable, ByVal xslPath As String) As String
        Dim sResult As String = ""
        If xslPath.StartsWith("/") Then
            xslPath = HttpContext.Current.Server.MapPath(xslPath)
        End If
        Using oMs As New MemoryStream
            Dim sXMLBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(xml)

            oMs.Write(sXMLBytes, 0, sXMLBytes.Length)
            oMs.Position = 0

            Dim oXMLDoc As XPathDocument = New XPathDocument(oMs)
            Dim oXSLDoc As XPathDocument = New XPathDocument(xslPath)

            Dim oXSLProc As XslCompiledTransform = New XslCompiledTransform

            Dim oSB As New StringBuilder

            Dim oXSLTArgs As XsltArgumentList = Nothing
            If aArgs IsNot Nothing Then
                oXSLTArgs = New XsltArgumentList
                Dim oEnum As IDictionaryEnumerator = aArgs.GetEnumerator
                While oEnum.MoveNext
                    oXSLTArgs.AddParam(oEnum.Key, "", oEnum.Value)
                End While
            End If

            Dim oXSLOpts As New System.Xml.Xsl.XsltSettings(True, False)

            oXSLProc.Load(oXSLDoc, oXSLOpts, Nothing)

            Using oSW As New StringWriter(oSB)
                oXSLProc.Transform(oXMLDoc, oXSLTArgs, oSW)
            End Using

            sResult = oSB.ToString
        End Using

        Return sResult

    End Function 'InMemoryTransform

    Public Shared Function DataToXMLString(ByVal oDS As DataSet) As String
        Dim sXML As String = ""
        Using oSW As New StringWriter
            oDS.WriteXml(oSW, XmlWriteMode.IgnoreSchema)
            sXML = oSW.ToString
        End Using
        Return sXML
    End Function

    Public Shared Function DataToXMLString(ByVal oDT As DataTable) As String
        Dim sXML As String = ""
        Using oSW As New StringWriter
            oDT.WriteXml(oSW, XmlWriteMode.IgnoreSchema)
            sXML = oSW.ToString
        End Using
        Return sXML
    End Function

End Class
