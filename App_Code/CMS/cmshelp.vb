Imports Microsoft.VisualBasic

Public Class cmshelp
    Public Shared Sub SetupContentPage(ByVal PageRef As String, ByVal oPage As Page)
        Dim oPgA As New appxCMSDataTableAdapters.ContentPageTableAdapter
        Dim oPgT As appxCMSData.ContentPageDataTable = oPgA.GetByPageRef(PageRef)
        If oPgT.Rows.Count > 0 Then
            Dim oPg As appxCMSData.ContentPageRow = oPgT.Rows(0)

            oPage.Title = oPg.PageTitle

            Dim oMeta As New StringBuilder
            If Not oPg.IsMeta_AbstractNull Then
                oMeta.AppendLine("<meta name=""abstract"" content=""" & oPg.Meta_Abstract & """/>")
            End If
            If Not oPg.IsMeta_AuthorNull Then
                oMeta.AppendLine("<meta name=""author"" content=""" & oPg.Meta_Author & """/>")
            End If
            If Not oPg.IsMeta_CopyrightNull Then
                oMeta.AppendLine("<meta name=""copyright"" content=""" & oPg.Meta_Copyright & """/>")
            End If
            If Not oPg.IsMeta_DescriptionNull Then
                oMeta.AppendLine("<meta name=""description"" content=""" & oPg.Meta_Description & """/>")
            End If
            If Not oPg.IsMeta_KeywordNull Then
                oMeta.AppendLine("<meta name=""keywords"" content=""" & oPg.Meta_Keyword & """/>")
            End If
            oPage.Header.Controls.Add(New LiteralControl(oMeta.ToString))
        Else
            Dim sPgTitle As String = oPage.Title
            Dim sTitle As String = ConfigurationManager.AppSettings("SiteTitle").ToString
            If String.IsNullOrEmpty(sPgTitle) Or sPgTitle.ToLowerInvariant = "untitled page" Or sPgTitle = sTitle Then
                '-- Have not been able to get title info from meta or elsewhere, try to build it from the menu system
                Using oMenuA As New appxCMSDataTableAdapters.EasyMenuTableAdapter
                    sTitle = oMenuA.GetMenuTitle(PageRef)
                End Using
                If String.IsNullOrEmpty(sTitle) Then
                    If ConfigurationManager.AppSettings("SiteTitle") IsNot Nothing Then
                        sTitle = ConfigurationManager.AppSettings("SiteTitle").ToString
                    End If
                End If
                oPage.Title = sTitle
            End If
        End If
    End Sub

    Public Shared Function RecursiveControlSearch(ByVal oMC As Control, ByVal sID As String) As Control
        Dim oC As Control = oMC.FindControl(sID)
        If oC IsNot Nothing Then
            Return RecursiveControlSearch(oC, sID)
        Else
            Return oMC
        End If
    End Function

    Public Shared Function DeepFindContentPlaceHolder(ByVal oBase As MasterPage, ByVal sTargetID As String) As Control
        Dim oMC As Control = Nothing
        If oBase IsNot Nothing Then
            For Each oC As Control In oBase.Controls
                oMC = apphelp.DeepFindControl(oC, sTargetID)
                If oMC IsNot Nothing Then
                    Exit For
                End If
            Next
        End If
        Return oMC
    End Function

    Public Shared Function GetConfigValue(ByVal ConfigCat As String, ByVal ConfigName As String) As String
        Dim sRet As String = ""
        Dim sCacheKey As String = "Config:" & ConfigCat & ":" & ConfigName
        If HttpContext.Current.Cache(sCacheKey) IsNot Nothing Then
            sRet = HttpContext.Current.Cache(sCacheKey)
        Else
            Using oConfigA As New appxSetupTableAdapters.ConfigTableAdapter
                sRet = oConfigA.GetConfigItemValue(ConfigCat, ConfigName).ToString
            End Using
            HttpContext.Current.Cache.Insert(sCacheKey, sRet)
        End If
        Return sRet
    End Function

    Public Shared Function SaveConfigValue(ByVal ConfigCat As String, ByVal ConfigName As String, ByVal ConfigValue As String) As Boolean
        Dim id As Integer = 0
        Using oConfigA As New appxSetupTableAdapters.ConfigTableAdapter
            id = oConfigA.UpdateConfigItem(ConfigValue, ConfigCat, ConfigName)
        End Using
        '-- Clear the cached value
        Dim sCacheKey As String = "Config:" & ConfigCat & ":" & ConfigName
        If HttpContext.Current.Cache(sCacheKey) IsNot Nothing Then
            HttpContext.Current.Cache.Remove(sCacheKey)
        End If
        Return True
    End Function

    Public Shared Function ShowSignUp() As Boolean
        Dim bShowSignup As Boolean = False
        Dim sShowSignup As String = GetConfigValue("Membership", "ShowSignUpLink")
        If sShowSignup.ToLower = "true" Or sShowSignup = "1" Then
            bShowSignup = True
        End If
        Return bShowSignup
    End Function

    Public Shared Function CacheEnabled() As Boolean
        Dim bEnableCache As Boolean = False
        Dim sEnableCache As String = GetConfigValue("CMSConfig", "EnableCache")
        If sEnableCache.ToLower = "true" Or sEnableCache = "1" Then
            bEnableCache = True
        End If
        Return bEnableCache
    End Function

    Public Shared Function DebugEnabled() As Boolean
        Dim bEnableDebug As Boolean = False
        Dim sEnableDebug As String = GetConfigValue("CMSConfig", "EnableDebug")
        If sEnableDebug.ToLower = "true" Or sEnableDebug = "1" Then
            bEnableDebug = True
        End If
        Return bEnableDebug
    End Function
End Class
