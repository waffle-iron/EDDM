Imports System.IO
Imports System.Collections

<Serializable()> _
Public Class CMSPage
    Private _PageRef As String = ""
    Public Property PageRef() As String
        Get
            Return _PageRef
        End Get
        Set(ByVal value As String)
            _PageRef = value
        End Set
    End Property

    Private _PageTemplate As String = ""
    Public Property PageTemplate() As String
        Get
            Dim sTemplate As String = _PageTemplate
            sTemplate = sTemplate.Replace("/vpage_", "").Replace(".aspx", "")
            Return sTemplate
        End Get
        Set(ByVal value As String)
            _PageTemplate = value
        End Set
    End Property

    Private _IsVirtual As Boolean = False
    Public Property IsVirtual() As Boolean
        Get
            Return _IsVirtual
        End Get
        Set(ByVal value As Boolean)
            _IsVirtual = value
        End Set
    End Property

    Private _MetaDefined As Boolean = False
    Public Property MetaDefined() As Boolean
        Get
            Return _MetaDefined
        End Get
        Set(ByVal value As Boolean)
            _MetaDefined = value
        End Set
    End Property

    Private _IsExpiring As Boolean = False
    Public Property IsExpiring() As Boolean
        Get
            Return _IsExpiring
        End Get
        Set(ByVal value As Boolean)
            _IsExpiring = value
        End Set
    End Property

    Private _ExpirationDate As DateTime
    Public Property ExpirationDate() As DateTime
        Get
            Return _ExpirationDate
        End Get
        Set(ByVal value As DateTime)
            _ExpirationDate = value
        End Set
    End Property

    Private _ExpirationAction As String
    Public Property ExpirationAction() As String
        Get
            Return _ExpirationAction
        End Get
        Set(ByVal value As String)
            _ExpirationAction = value
        End Set
    End Property
End Class

<Serializable()> _
Public Class CMSTemplates
    Inherits CollectionBase

    Public Function Add(ByVal oTemplate As CMSTemplate) As Integer
        Return List.Add(oTemplate)
    End Function

    Public Sub Remove(ByVal oTempate As CMSTemplate)
        List.Remove(oTempate)
    End Sub
End Class

<Serializable()> _
Public Class CMSTemplate
    Private _TemplateFile As String = ""
    Public Property TemplateFile() As String
        Get
            Return _TemplateFile
        End Get
        Set(ByVal value As String)
            _TemplateFile = value
        End Set
    End Property

    Private _TemplateName As String = ""
    Public Property TemplateName() As String
        Get
            If String.IsNullOrEmpty(_TemplateName) Then
                Dim sTmp As String = TemplateFile.Replace("/vpage_", "").Replace(".aspx", "").Trim
                _TemplateName = apphelp.CamelCaseToTitle(sTmp)
            End If
            Return _TemplateName
        End Get
        Set(ByVal value As String)

        End Set
    End Property
End Class

<Serializable()> _
Public Class CMSDAL
    Public Function GetPageTemplates() As System.Collections.Generic.List(Of CMSTemplate)
        Dim aPageTemplates As New System.Collections.Generic.List(Of CMSTemplate)

        Try
            Dim sWebRoot As String = HttpContext.Current.Server.MapPath("/")

            Dim sPath As String = sWebRoot

            Dim oFolder As DirectoryInfo = New DirectoryInfo(sPath)
            Dim aFiles() As FileInfo = oFolder.GetFiles("vpage_*.aspx")

            For i As Integer = 0 To aFiles.Length - 1
                Dim sPage As String = aFiles(i).FullName.Replace(sPath, "").Replace("\", "/")
                If Not sPage.StartsWith("/") Then sPage = "/" & sPage
                Dim oTemplate As New CMSTemplate
                oTemplate.TemplateFile = sPage
                aPageTemplates.Add(oTemplate)
                'aPageTemplates.Add(sPage)
            Next
        Catch ex As Exception
            HttpContext.Current.Response.Write("<p>cmsDAL.GetPageTemplates: " & ex.Message & "<br/>" & ex.StackTrace & "</p>")
        End Try

        Return aPageTemplates
    End Function

    Public Function GetPhysicalPages() As ArrayList
        Dim aPageRefs As New ArrayList

        Try
            Dim sWebRoot As String = HttpContext.Current.Server.MapPath("/")

            Dim sPath As String = sWebRoot

            Dim oFolder As DirectoryInfo = New DirectoryInfo(sPath)
            Dim aFiles() As FileInfo = oFolder.GetFiles("*.aspx")

            For i As Integer = 0 To aFiles.Length - 1
                Dim sPage As String = aFiles(i).FullName.Replace(sPath, "").Replace("\", "/")
                If Not sPage.StartsWith("vpage_") And Not sPage.StartsWith("404") Then
                    If Not sPage.StartsWith("/") Then sPage = "/" & sPage
                    aPageRefs.Add(sPage)
                End If
            Next
        Catch ex As Exception
            HttpContext.Current.Response.Write("<p>cmsDAL.GetPhysicalPages: " & ex.Message & "</p>")
        End Try

        Return aPageRefs
    End Function

    Public Function GetUnDefinedPages() As ArrayList
        Dim aPageRefs As New ArrayList
        Try
            Dim aPageList As New ArrayList

            Dim sWebRoot As String = HttpContext.Current.Server.MapPath("/")

            Dim sPath As String = sWebRoot

            Dim oFolder As DirectoryInfo = New DirectoryInfo(sPath)
            Dim aFiles() As FileInfo = oFolder.GetFiles("*.aspx")

            For i As Integer = 0 To aFiles.Length - 1
                Dim sPage As String = aFiles(i).FullName.Replace(sPath, "").Replace("\", "/")
                If Not sPage.StartsWith("vpage_") And Not sPage.StartsWith("404") Then
                    If Not sPage.StartsWith("/") Then sPage = "/" & sPage
                    If aPageList.IndexOf(sPage.ToLower) < 0 Then
                        aPageRefs.Add(sPage)
                    End If
                End If
            Next

            Dim oVPageA As New appxCMSDataTableAdapters.VirtualTableAdapter
            Dim oVPageT As appxCMSData.VirtualDataTable = oVPageA.GetData
            For Each oVPage As appxCMSData.VirtualRow In oVPageT.Rows
                If aPageList.IndexOf(oVPage.PageRef.ToLower) < 0 Then
                    aPageRefs.Add(oVPage.PageRef)
                End If
            Next
            oVPageT.Dispose()
            oVPageA.Dispose()

            aPageRefs.Sort()
        Catch ex As Exception
            HttpContext.Current.Response.Write("<p>cmsDAL.GetUnDefinedPages: " & ex.Message & "</p>")
        End Try

        Return aPageRefs
    End Function

    Public Function GetPages() As System.Collections.Generic.List(Of CMSPage) 'CollectionX(Of CMSPage)
        Dim aPageRefs As New System.Collections.Generic.List(Of CMSPage) 'CollectionX(Of CMSPage)

        Try
            Dim sWebRoot As String = HttpContext.Current.Server.MapPath("/")

            Dim sPath As String = sWebRoot

            Dim oFolder As DirectoryInfo = New DirectoryInfo(sPath)
            Dim aFiles() As FileInfo = oFolder.GetFiles("*.aspx")

            Using oMetaA As New appxCMSDataTableAdapters.ContentPageTableAdapter
                For i As Integer = 0 To aFiles.Length - 1
                    Dim sPage As String = aFiles(i).FullName.Replace(sPath, "").Replace("\", "/")
                    If Not sPage.StartsWith("vpage_") And Not sPage.StartsWith("404") Then
                        If Not sPage.StartsWith("/") Then sPage = "/" & sPage
                        Dim oCMSPage As New CMSPage
                        oCMSPage.PageRef = sPage
                        oCMSPage.IsVirtual = False
                        Dim iMeta As Integer = oMetaA.MetaExists(sPage)
                        If iMeta > 0 Then
                            oCMSPage.MetaDefined = True
                        End If
                        aPageRefs.Add(oCMSPage)
                    End If
                Next

                '-- Get List of Survey Pages
                Using oSurveyA As New appxSurveyTableAdapters.SurveyHeaderTableAdapter
                    Using oSurveyT As appxSurvey.SurveyHeaderDataTable = oSurveyA.GetData()
                        For Each oSurvey As appxSurvey.SurveyHeaderRow In oSurveyT.Rows
                            Dim oCMSPage As New CMSPage
                            oCMSPage.PageRef = linkHelp.SEOLink(oSurvey.SurveyName, oSurvey.SurveyID, linkHelp.LinkType.Survey)
                            oCMSPage.IsVirtual = True
                            oCMSPage.MetaDefined = False
                            aPageRefs.Add(oCMSPage)
                        Next
                    End Using
                End Using

                Dim oVPageA As New appxCMSDataTableAdapters.VirtualTableAdapter
                Dim oVPageT As appxCMSData.VirtualDataTable = oVPageA.GetData
                For Each oVPage As appxCMSData.VirtualRow In oVPageT.Rows
                    Dim oCMSPage As New CMSPage
                    oCMSPage.PageRef = oVPage.PageRef
                    oCMSPage.PageTemplate = oVPage.PageTemplate
                    oCMSPage.IsVirtual = True
                    Dim iMeta As Integer = oMetaA.MetaExists(oVPage.PageRef)
                    If iMeta > 0 Then
                        oCMSPage.MetaDefined = True
                    End If
                    aPageRefs.Add(oCMSPage)
                    'aPageRefs.Add(oVPage.PageRef)
                Next
                oVPageT.Dispose()
                oVPageA.Dispose()
            End Using

            'aPageRefs.Sort()
            'aPageRefs.Sort("PageRef", SortOrder.ASC)
        Catch ex As Exception
            HttpContext.Current.Response.Write("<p>cmsDAL.GetPages: " & ex.Message & "</p>")
        End Try
        aPageRefs.Sort(AddressOf SortByName)
        'Function(p1 As CMSPage, p2 As CMSPage) Return p1.PageRef.CompareTo(p2.PageRef)
        Return aPageRefs
    End Function

    Public Function FindPages(ByVal sPageName As String, ByVal sTemplate As String) As System.Collections.Generic.List(Of CMSPage)
        Dim aPageRefs As New System.Collections.Generic.List(Of CMSPage)

        Try
            If sPageName Is Nothing Then
                sPageName = ""
            Else
                sPageName = sPageName.ToLower.Trim
            End If
            If sTemplate Is Nothing Then
                sTemplate = ""
            Else
                sTemplate = sTemplate.Trim.ToLower
            End If

            Dim aProcPaths As New ArrayList
            aProcPaths.Add(HttpContext.Current.Server.MapPath("/"))
            aProcPaths.Add(HttpContext.Current.Server.MapPath("/Members"))

            Dim sBasePath As String = HttpContext.Current.Server.MapPath("/")

            Using oMetaA As New appxCMSDataTableAdapters.ContentPageTableAdapter
                For iPath As Integer = 0 To aProcPaths.Count - 1
                    'Dim sWebRoot As String = HttpContext.Current.Server.MapPath("/")
                    'Dim sPath As String = sWebRoot
                    Dim spath As String = aProcPaths(iPath)
                    If Directory.Exists(spath) Then
                        Dim oFolder As DirectoryInfo = New DirectoryInfo(spath)
                        Dim aFiles() As FileInfo = oFolder.GetFiles("*.aspx")

                        For i As Integer = 0 To aFiles.Length - 1
                            Dim sPage As String = aFiles(i).FullName.Replace(sBasePath, "").Replace("\", "/")
                            If Not sPage.StartsWith("/vpage_") And Not sPage.StartsWith("/vtemplate_") Then
                                If sPage.ToLower.Contains(sPageName.ToLower) Then
                                    If Not sPage.StartsWith("/") Then sPage = "/" & sPage
                                    Dim oCMSPage As New CMSPage
                                    oCMSPage.PageRef = sPage
                                    oCMSPage.IsVirtual = False
                                    Dim iMeta As Integer = oMetaA.MetaExists(sPage)
                                    If iMeta > 0 Then
                                        oCMSPage.MetaDefined = True
                                    End If
                                    aPageRefs.Add(oCMSPage)
                                End If
                            End If
                        Next
                    End If
                Next

                Dim oVPageA As New appxCMSDataTableAdapters.VirtualTableAdapter
                Dim oVPageT As appxCMSData.VirtualDataTable = oVPageA.GetData
                For Each oVPage As appxCMSData.VirtualRow In oVPageT.Rows
                    Dim sPage As String = oVPage.PageRef
                    Dim sPgTemplate As String = oVPage.PageTemplate

                    If sPage.ToLower.Contains(sPageName.ToLower) Or (sPgTemplate.ToLower.Contains(sTemplate.ToLower) And Not String.IsNullOrEmpty(sTemplate)) Then
                        Dim oCMSPage As New CMSPage
                        oCMSPage.PageRef = oVPage.PageRef
                        oCMSPage.PageTemplate = oVPage.PageTemplate
                        oCMSPage.IsVirtual = True
                        Dim iMeta As Integer = oMetaA.MetaExists(oVPage.PageRef)
                        If iMeta > 0 Then
                            oCMSPage.MetaDefined = True
                        End If
                        If Not oVPage.IsExpirationDateNull Then
                            oCMSPage.IsExpiring = True
                            oCMSPage.ExpirationDate = oVPage.ExpirationDate
                            If Not oVPage.IsExpirationActionNull Then
                                oCMSPage.ExpirationAction = oVPage.ExpirationAction
                            End If
                        End If
                        aPageRefs.Add(oCMSPage)
                    End If
                Next
                oVPageT.Dispose()
                oVPageA.Dispose()
            End Using


            aPageRefs.Sort(AddressOf SortByName)
        Catch ex As Exception
            HttpContext.Current.Response.Write("<p>cmsDAL.FindPages: " & ex.Message & "<br/>" & ex.StackTrace & "</p>")
        End Try

        Return aPageRefs
    End Function

    Public Sub BuildPageLinkList(ByVal oDD As DropDownList, ByVal dActions As Hashtable)
        Dim sWebRoot As String = HttpContext.Current.Server.MapPath("/")
        Dim sPath As String = sWebRoot

        Dim oLList As New SortedList
        Dim oA As New appxCMSDataTableAdapters.getPageLinksTableAdapter
        Dim oT As appxCMSData.getPageLinksDataTable = oA.GetData
        For Each oRow As appxCMSData.getPageLinksRow In oT.Rows
            If Not oLList.Contains(oRow.pageRef) Then
                oLList.Add(oRow.PageRef.Trim, oRow.PageRef)
            End If
        Next
        oA.Dispose()
        oT.Dispose()

        Dim oFolder As DirectoryInfo = New DirectoryInfo(sPath)
        Dim aFiles() As FileInfo = oFolder.GetFiles("*.aspx")

        For i As Integer = 0 To aFiles.Length - 1
            Dim sPage As String = aFiles(i).FullName.Replace(sPath, "").Replace("\", "/")
            If Not sPage.StartsWith("/vpage_") And Not sPage.StartsWith("/_allpages.") And Not sPage.StartsWith("/404.") And Not sPage.StartsWith("/500.") Then
                If Not sPage.StartsWith("/") Then sPage = "/" & sPage
                If Not oLList.Contains(sPage) Then
                    oLList.Add(sPage, sPage)
                End If
            End If
        Next

        '-- Get List of Survey Pages
        Using oSurveyA As New appxSurveyTableAdapters.SurveyHeaderTableAdapter
            Using oSurveyT As appxSurvey.SurveyHeaderDataTable = oSurveyA.GetData()
                For Each oSurvey As appxSurvey.SurveyHeaderRow In oSurveyT.Rows
                    Dim sPage As String = linkHelp.SEOLink(oSurvey.SurveyName, oSurvey.SurveyID, linkHelp.LinkType.Survey)
                    oLList.Add(sPage, sPage & " (" & oSurvey.SurveyName & ")")
                Next
            End Using
        End Using

        '-- List of "Blogs"
        Using oNewsTypesA As New appxCMSNewsTableAdapters.BlogListTableAdapter
            Using oNewsTypesT As appxCMSNews.BlogListDataTable = oNewsTypesA.GetData
                For Each oNewsType As appxCMSNews.BlogListRow In oNewsTypesT.Rows
                    Dim sPage As String = appxCMS.SEO.Rewrite.GetLink(oNewsType.NewsType, "0-" & oNewsType.NewsTypeId.ToString, appxCMS.SEO.Rewrite.LinkType.NewsBlog)
                    oLList.Add(sPage, sPage & " (" & oNewsType.NewsType & " Blog)")
                Next
            End Using
        End Using

        Dim oListEnum As IDictionaryEnumerator = oLList.GetEnumerator
        While oListEnum.MoveNext
            Dim oItem As ListItem = Nothing
            Dim sPageName As String = oListEnum.Value
            If sPageName.StartsWith("/") Then sPageName = sPageName.Substring(1)
            oItem = New ListItem(sPageName, oListEnum.Key)
            oDD.Items.Add(oItem)
        End While

        If dActions IsNot Nothing Then
            Dim oEnum As IDictionaryEnumerator = dActions.GetEnumerator
            While oEnum.MoveNext
                oDD.Attributes.Add(oEnum.Key, oEnum.Value)
            End While
        End If
    End Sub

    Private Function SortByName(ByVal p1 As CMSPage, ByVal p2 As CMSPage) As Integer
        Return p1.PageRef.CompareTo(p2.PageRef)
    End Function
End Class
