Imports System
Imports System.Xml

Partial Class UserControls_SiteMenu
    Inherits System.Web.UI.UserControl

    Public Property TopLevelEqualWidth() As Boolean
        Get
            Return _TopLevelEqualWidth
        End Get
        Set(ByVal value As Boolean)
            _TopLevelEqualWidth = value
        End Set
    End Property
    Private _TopLevelEqualWidth As Boolean = False

    Public Property HeadRefID() As String
        Get
            Return _HeadRefID
        End Get
        Set(ByVal value As String)
            _HeadRefID = value
        End Set
    End Property
    Private _HeadRefID As String = ""

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim sMenuXML As String = ""

        Using oMenuA As New appxCMSDataTableAdapters.EasyMenuTableAdapter
            Dim iTopLevel As Integer = oMenuA.ParentCount
            If iTopLevel = 0 Then
                Me.Visible = False
                Exit Sub
            End If
            Dim oTopMenuWidth As Integer = 100 / iTopLevel

            If Me.TopLevelEqualWidth Then
                '-- Add an adjustment to the stylesheet to make the menu widths equal
            End If

            Using oMenuT As appxCMSData.EasyMenuDataTable = oMenuA.GetData()

                Dim oMenuXml As New XmlDocument
                oMenuXml.LoadXml("<ul />")
                Dim oMenuRoot As XmlNode = oMenuXml.SelectSingleNode("//ul")
                xmlhelp.AddOrUpdateXMLAttribute(oMenuRoot, "class", "sf-menu")
                Dim oMenuList As New System.Collections.Generic.Dictionary(Of String, XmlNode)

                For Each dr As appxCMSData.EasyMenuRow In oMenuT.Rows
                    '-- Populate top level Menu Items

                    Dim pid As Integer = dr.Id
                    Dim iChildren As Integer = dr.ChildCount

                    Dim sIcon As String = ""
                    If Not dr.IsIconNull Then
                        sIcon = dr.Icon
                    End If

                    Dim sHtml As String = ""
                    If Not dr.IsInnerHtmlNull Then
                        sHtml = dr.InnerHtml
                    End If

                    Dim url As String = ""
                    If Not dr.IsUrlNull Then
                        url = dr.Url
                    End If

                    Dim menuID As String = "item_" & pid

                    '-- Find If Item has Child Items
                    If dr.ParentId = 0 Then
                        Dim oItem As XmlNode = xmlhelp.AddOrUpdateXMLNode(oMenuRoot, "li", "")
                        Dim oItemLnk As XmlNode = xmlhelp.AddOrUpdateXMLNode(oItem, "a", "")
                        xmlhelp.AddOrUpdateXMLAttribute(oItemLnk, "href", url)
                        If Not String.IsNullOrEmpty(sIcon) Then
                            Dim oItemIcon As XmlNode = xmlhelp.AddOrUpdateXMLNode(oItemLnk, "img", "")
                            xmlhelp.AddOrUpdateXMLAttribute(oItemIcon, "src", sIcon)
                        End If
                        If sHtml.Contains("<") Then
                            Dim oItemText As XmlNode = xmlhelp.AddOrUpdateXMLNode(oItemLnk, "span", "")

                            Dim oImpDoc As New XmlDocument
                            oImpDoc.LoadXml("<imp>" & sHtml & "</imp>")
                            Dim oImpLoader As XmlNode = oImpDoc.SelectSingleNode("//imp")
                            Dim oImpLoadNode As XmlNode = oImpLoader.FirstChild
                            Dim oImpNode As XmlNode = oMenuXml.ImportNode(oImpLoadNode, True)
                            oItemText.AppendChild(oImpNode)
                            'Dim oContentData As XmlNode = oMenuXml.CreateCDataSection(sHtml)
                            'oItemText.AppendChild(oContentData)
                        Else
                            Dim oItemText As XmlNode = xmlhelp.AddOrUpdateXMLNode(oItemLnk, "span", sHtml)
                        End If

                        If iChildren > 0 Then
                            Dim oItemChild As XmlNode = xmlhelp.AddOrUpdateXMLNode(oItem, "ul", "")
                            oMenuList.Add(pid.ToString, oItemChild)
                        End If
                    Else
                        If oMenuList.ContainsKey(dr.ParentId.ToString) Then
                            Dim oParent As XmlNode = oMenuList(dr.ParentId.ToString)
                            If oParent IsNot Nothing Then
                                Dim oItem As XmlNode = xmlhelp.AddOrUpdateXMLNode(oParent, "li", "")
                                Dim oItemLnk As XmlNode = xmlhelp.AddOrUpdateXMLNode(oItem, "a", "")
                                xmlhelp.AddOrUpdateXMLAttribute(oItemLnk, "href", url)
                                If Not String.IsNullOrEmpty(sIcon) Then
                                    Dim oItemIcon As XmlNode = xmlhelp.AddOrUpdateXMLNode(oItemLnk, "img", "")
                                    xmlhelp.AddOrUpdateXMLAttribute(oItemIcon, "src", sIcon)
                                End If
                                If sHtml.Contains("<") Then
                                    Dim oItemText As XmlNode = xmlhelp.AddOrUpdateXMLNode(oItemLnk, "span", "")

                                    Dim oImpDoc As New XmlDocument
                                    oImpDoc.LoadXml("<imp>" & sHtml & "</imp>")
                                    Dim oImpLoader As XmlNode = oImpDoc.SelectSingleNode("//imp")
                                    Dim oImpLoadNode As XmlNode = oImpLoader.FirstChild
                                    Dim oImpNode As XmlNode = oMenuXml.ImportNode(oImpLoadNode, True)
                                    oItemText.AppendChild(oImpNode)
                                    'Dim oContentData As XmlNode = oMenuXml.CreateCDataSection(sHtml)
                                    'oItemText.AppendChild(oContentData)
                                Else
                                    Dim oItemText As XmlNode = xmlhelp.AddOrUpdateXMLNode(oItemLnk, "span", sHtml)
                                End If

                                If iChildren > 0 Then
                                    Dim oItemChild As XmlNode = xmlhelp.AddOrUpdateXMLNode(oItem, "ul", "")
                                    If Not oMenuList.ContainsKey(pid.ToString) Then
                                        oMenuList.Add(pid.ToString, oItemChild)
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
                sMenuXML = oMenuRoot.OuterXml
            End Using
        End Using

        phMenu.Controls.Add(New LiteralControl(sMenuXML))
    End Sub
End Class
