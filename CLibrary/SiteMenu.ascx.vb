Imports System
Imports System.Xml
Imports HtmlAgilityPack

Partial Class CLibrary_SiteMenu
    Inherits System.Web.UI.UserControl

    Private _menuId As Nullable(Of Integer)
    Public Property MenuId() As Nullable(Of Integer)
        Get
            Return _MenuId
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _MenuId = value
        End Set
    End Property

    Private _menuName As String = ""
    Public Property MenuName() As String
        Get
            Return _MenuName
        End Get
        Set(ByVal value As String)
            _MenuName = value
        End Set
    End Property

    Private _orientation As String = "Horiztonal"
    Public Property Orientation As String
        Get
            Return _Orientation
        End Get
        Set(ByVal value As String)
            _Orientation = value
        End Set
    End Property



    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub



    Protected Sub BuildControl()

        If Not Me.MenuId.HasValue Then
            If Not String.IsNullOrEmpty(Me.MenuName) Then
                Dim oMenu As appxCMS.Menu = appxCMS.MenuDataSource.GetMenu(Me.MenuName)
                If oMenu IsNot Nothing Then
                    Me.MenuId = oMenu.MenuId
                End If
            End If
        End If

        If Me.MenuId.HasValue AndAlso Me.MenuId.Value > 0 Then
            Dim iMenuId As Integer = Me.MenuId.Value
            Dim sMenuXml As String = appxCMS.MenuDataSource.BuildMenu(iMenuId, Me.Orientation)

            '-- Add attributes for current page
            If Not String.IsNullOrEmpty(sMenuXml) Then
                Dim oMenu As New HtmlDocument
                oMenu.OptionFixNestedTags = True
                oMenu.OptionAutoCloseOnEnd = True
                oMenu.OptionWriteEmptyNodes = True
                oMenu.LoadHtml(sMenuXml)

                Dim oUl As HtmlNode = oMenu.DocumentNode.SelectSingleNode("//ul")
                If oUl IsNot Nothing Then
                    oUl.Attributes.RemoveAll()
                    oUl.Attributes.Add("class", "nav navbar-nav")
                Else
                    phMenu.Controls.Add(New LiteralControl("No UL found"))
                End If

                '-- Clean-up top-level menu selection highlight (performed after retrieved from cache)
                Dim sPage As String = appxCMS.PageBase.GetRequestedURL(Page)
                If String.IsNullOrEmpty(sPage) Then
                    sPage = "/"
                End If


                Dim oA As HtmlNode = oMenu.DocumentNode.SelectSingleNode("//a[translate(@href, '" & appxCMS.Util.Xml.UCaseAlpha & "', '" & appxCMS.Util.Xml.LCaseAlpha & "')='" & sPage.ToLower & "']")
                If oA Is Nothing AndAlso sPage = "/" Then
                    '-- Check for /default, also
                    oA = oMenu.DocumentNode.SelectSingleNode("//a[translate(@href, '" & appxCMS.Util.Xml.UCaseAlpha & "', '" & appxCMS.Util.Xml.LCaseAlpha & "')='/default.aspx']")
                End If


                If oA Is Nothing Then
                    '-- Check for a higher level path
                    If sPage.IndexOf("/", 1, System.StringComparison.Ordinal) > 1 Then
                        sPage = sPage.Substring(0, sPage.IndexOf("/", 1, System.StringComparison.Ordinal))
                        oA = oMenu.DocumentNode.SelectSingleNode("//a[translate(@href, '" & appxCMS.Util.Xml.UCaseAlpha & "', '" & appxCMS.Util.Xml.LCaseAlpha & "')='" & sPage.ToLower & "']")
                    End If
                End If

                If oA IsNot Nothing Then
                    Dim oLi As HtmlNode = oA.ParentNode
                    If oLi IsNot Nothing Then
                        Dim sClass As String = ""
                        If oLi.Attributes("class") IsNot Nothing Then
                            sClass = oLi.Attributes("class").Value
                        End If
                        Dim sNewClass As String = (sClass & " " & "active").Trim
                        oLi.Attributes.Add("class", sNewClass)
                    End If
                End If
                
                'Dim oMenu As New XmlDocument
                'oMenu.LoadXml(sMenuXml)

                'Dim sPage As String = appxCMS.PageBase.GetRequestedURL(Page)
                'If String.IsNullOrEmpty(sPage) Then
                '    sPage = "/default.aspx"
                'End If
                'Dim oA As XmlNode = oMenu.SelectSingleNode("//a[translate(@href, '" & appxCMS.Util.Xml.UCaseAlpha & "', '" & appxCMS.Util.Xml.LCaseAlpha & "')='" & sPage.ToLower & "']")
                'If oA IsNot Nothing Then
                '    Dim oLi As XmlNode = oA.ParentNode
                '    If oLi IsNot Nothing Then
                '        Dim sClass As String = appxCMS.Util.Xml.ReadAttribute(oLi, "class")
                '        Dim sNewClass As String = (sClass & " " & "active").Trim
                '        appxCMS.Util.Xml.AddOrUpdateXMLAttribute(oLi, "class", sNewClass)

                '        sMenuXml = oMenu.OuterXml
                '    End If
                'End If

                sMenuXml = oUl.OuterHtml
            End If

            phMenu.Controls.Add(New LiteralControl(sMenuXml))
        Else
            Me.Visible = False
        End If
    End Sub


End Class
