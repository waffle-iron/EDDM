Imports System
Imports System.Web.Caching
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Xml
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.Design
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts

Namespace appx.CustomWebParts
    ' <summary>
    ' Catalog for reading WebParts from an Xml Document
    ' </summary>
    Public Class XmlCatalogPart
        Inherits CatalogPart

        Private document As XmlDocument

        ' <summary>
        ' Creates a new instance of the class
        ' </summary>
        'Public Sub New()
        '    MyBase.New()
        'End Sub

        ' <summary>
        ' Overrides the Title to display Xml Catalog Part by default
        ' </summary>
        Public Overrides Property Title() As String
            Get
                Dim baseTitle As String = MyBase.Title
                Return IIf(String.IsNullOrEmpty(baseTitle), "Xml Catalog Part", baseTitle)
            End Get
            Set(ByVal value As String)
                MyBase.Title = value
            End Set
        End Property

        Public Property WPManagerID() As String
            Get
                Return _WPManagerID
            End Get
            Set(ByVal value As String)
                _WPManagerID = value
            End Set
        End Property
        Private _WPManagerID As String = ""

        ' <summary>
        ' Specifies the Path for the Xml File that contains the declaration of the WebParts, 
        '     more specifically the WebPartDescriptions
        ' </summary>
        <UrlProperty(), _
         DefaultValue(""), _
         Editor(GetType(System.Web.UI.Design.XmlUrlEditor), GetType(System.Drawing.Design.UITypeEditor))> _
        Public Property DataFile() As String
            Get
                Dim o As Object = ViewState("wpDataFile")
                Return IIf(o Is Nothing, "", CStr(o))
            End Get
            Set(ByVal value As String)
                ViewState("wpDataFile") = value
            End Set
        End Property

        ' <summary>
        ' Returns the WebPartDescriptions
        ' </summary>
        Public Overrides Function GetAvailableWebPartDescriptions() As WebPartDescriptionCollection
            If Me.DesignMode Then
                Return New WebPartDescriptionCollection(New Object() { _
                        New WebPartDescription("1", "Xml WebPart 1", "Sample Webpart 1", Nothing), _
                        New WebPartDescription("2", "Xml WebPart 2", "Sample Webpart 2", Nothing), _
                        New WebPartDescription("3", "Xml WebPart 3", "Sample Webpart 3", Nothing)})
            End If
            Dim document As XmlDocument = Me.GetDocument()
            Dim list As New List(Of WebPartDescription)

            Dim aSplit() As Char = {",", ";", "|"}
            For Each oElem As XmlNode In document.SelectNodes("/parts/part")
                Dim sRoles As String = xmlhelp.ReadAttribute(oElem, "roles")
                Dim bAuth As Boolean = False
                If Not String.IsNullOrEmpty(sRoles) Then
                    Dim aRoles() As String = sRoles.Split(aSplit)
                    For iRole As Integer = 0 To aRoles.Length - 1
                        Dim sRole As String = aRoles(iRole)
                        If Not String.IsNullOrEmpty(sRole) Then
                            If HttpContext.Current.User.IsInRole(sRole) Then
                                bAuth = True
                                Exit For
                            End If
                        End If
                    Next
                Else
                    bAuth = True
                End If
                If bAuth Then
                    Dim oWP As WebPartDescription = New WebPartDescription(oElem.Attributes.GetNamedItem("id").Value, _
                                    oElem.Attributes.GetNamedItem("title").Value, _
                                    oElem.Attributes.GetNamedItem("description").Value, _
                                    oElem.Attributes.GetNamedItem("imageUrl").Value)
                    list.Add(oWP)
                End If
            Next
            Return New WebPartDescriptionCollection(list)
        End Function

        ' <summary>
        ' Returns a new instance of the WebPart specified by the description
        ' </summary>
        Public Overrides Function GetWebPart(ByVal description As WebPartDescription) As WebPart
            Dim oPartInfo As Dictionary(Of String, String) = Me.GetPartInfo(description.ID)

            Dim typeName As String = oPartInfo("type")
            If typeName.ToLower.EndsWith(".ascx") Then
                '-- Loading a User Control
                Dim oWPMgr As WebPartManager = WebPartManager.GetCurrentWebPartManager(Me.Page)
                'Dim oWPMgr As WebPartManager = DirectCast(appHelp.DeepFindControl(Page, WPManagerID), WebPartManager)
                If oWPMgr IsNot Nothing Then
                    Dim oC As Control = Page.LoadControl(typeName)
                    oC.ID = "wpuc" & description.ID
                    Dim oWP As WebPart = oWPMgr.CreateWebPart(oC)
                    oWP.Title = description.Title
                    oWP.Description = description.Description
                    If oPartInfo.ContainsKey("allowClose") Then
                        oWP.AllowClose = Boolean.Parse(oPartInfo("allowClose"))
                    End If
                    Return oWP
                Else
                    'HttpContext.Current.Response.Write(WPManagerID)
                    'HttpContext.Current.Response.End()
                    Return Nothing
                End If
            Else
                '-- Loading a type name
                Dim webPartType As Type = System.Type.GetType(typeName)
                Return CType(Activator.CreateInstance(webPartType, Reflection.BindingFlags.Default), WebPart)
            End If
        End Function

        ' <summary>
        ' private function to load the document and cache it
        ' </summary>
        Private Function GetDocument() As XmlDocument
            Dim file As String = Context.Server.MapPath(Me.DataFile)
            Dim key As String = ("__WebPartCatalog:" & file.ToLower)
            Dim document As XmlDocument = CType(Context.Cache(key), XmlDocument)
            If (document Is Nothing) Then
                document = New XmlDocument
                document.Load(file)

                Using dependency As CacheDependency = New CacheDependency(file)
                    Context.Cache.Add(key, document, dependency, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, Nothing)
                End Using
            End If
            Return document
        End Function
        ' <summary>
        ' Returns the type
        ' </summary>
        Private Function GetPartInfo(ByVal webPartID As String) As Dictionary(Of String, String)
            Dim oDic As New Dictionary(Of String, String)
            Dim document As XmlDocument = GetDocument()
            Dim element As XmlElement = CType(document.SelectSingleNode("/parts/part[@id='" & webPartID & "']"), XmlElement)
            For Each oA As XmlAttribute In element.Attributes
                oDic.Add(oA.Name, oA.Value)
            Next
            Return oDic
        End Function
    End Class
End Namespace
