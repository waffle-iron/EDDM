Imports Microsoft.VisualBasic

Public Class linkHelp
    Public Enum LinkType
        PrintCategory = 1
        PrintProduct = 2
        Target = 3
        Design = 4
        DesignGallery = 5
        ResourceCenter = 6
        ResourceGallery = 7
        Template = 8
        Survey = 9
        SurveyResponseConfirmation = 10
        DesignDIY = 11
        DesignDIYTool = 12
        DesignDIYNextSteps = 13
        DesignDIYByProduct = 14
        USelectTool = 15
        USelectNextSteps = 16
    End Enum

    Public Shared Function SEOLink(ByVal lnkTitle As String, ByVal lnkID As String, ByVal lnkType As LinkType) As String
        Dim sLnkIndicator As String = ""
        Select Case lnkType
            Case LinkType.PrintCategory
                sLnkIndicator = "printcat"
            Case LinkType.PrintProduct
                sLnkIndicator = "printprod"
            Case LinkType.Target
                sLnkIndicator = "deliver"
            Case LinkType.Design
                sLnkIndicator = "design"
            Case LinkType.DesignGallery
                sLnkIndicator = "gallery"
            Case LinkType.ResourceCenter
                sLnkIndicator = "resource"
            Case LinkType.ResourceGallery
                sLnkIndicator = "resourcegallery"
            Case LinkType.Template
                sLnkIndicator = "template"
            Case LinkType.Survey
                sLnkIndicator = "survey"
            Case LinkType.SurveyResponseConfirmation
                sLnkIndicator = "surveyreceived"
            Case LinkType.DesignDIY
                sLnkIndicator = "designdiy"
            Case LinkType.DesignDIYTool
                sLnkIndicator = "designdiytool"
            Case LinkType.DesignDIYNextSteps
                sLnkIndicator = "designdiynext"
            Case LinkType.DesignDIYByProduct
                sLnkIndicator = "designdiyproductline"
            Case LinkType.USelectTool
                sLnkIndicator = "uselecttool"
            Case LinkType.USelectNextSteps
                sLnkIndicator = "uselectreview"
        End Select

        '-- Replace special characters in link title with URL/SEO friendly terms
        lnkTitle = lnkTitle.Replace(" ", "-")

        lnkTitle = lnkTitle.Replace("(", "")
        lnkTitle = lnkTitle.Replace(")", "")
        lnkTitle = lnkTitle.Replace("w/", "with")
        lnkTitle = lnkTitle.Replace("W/", "with")
        lnkTitle = lnkTitle.Replace("/", "-and-")
        lnkTitle = lnkTitle.Replace("-x-", "-by-")
        lnkTitle = lnkTitle.Replace("'", "-foot")
        lnkTitle = lnkTitle.Replace(ControlChars.Quote, "-inch")
        lnkTitle = lnkTitle.Replace("#", "-pound-paper")

        Return String.Format("/{0}-{1}{2}", lnkTitle, sLnkIndicator, lnkID)
    End Function

    Public Shared Function ResolveSEOLink(ByVal lnkID As String, ByVal lnkType As LinkType) As String
        Dim sLnkTitle As String = ""

        Select Case lnkType
            Case LinkType.PrintCategory
                Dim oCat As Taradel.Category = Taradel.CategoryDataSource.GetCategoryById(lnkID)
                If oCat IsNot Nothing Then
                    sLnkTitle = oCat.Name
                End If
            Case LinkType.PrintProduct
                Dim oProd As Taradel.Product = Taradel.ProductDataSource.GetProduct(lnkID)
                If oProd IsNot Nothing Then
                    sLnkTitle = oProd.Name
                End If
                If String.IsNullOrEmpty(sLnkTitle) Then
                    Return "~/Print.aspx"
                Else
                    '-- Need to get the first category this product appears in
                    'Using oProdCatA As New taradelCatalogTableAdapters.CategoryProductTableAdapter
                    '    Dim sCatID As String = oProdCatA.GetFirstCategory(lnkID).ToString
                    '    If Not String.IsNullOrEmpty(sCatID) Then
                    '        lnkID = lnkID & "-" & sCatID
                    '    End If
                    'End Using
                End If
            Case LinkType.ResourceGallery
                '-- Try to resolve the previous gallery to a new resource center vertical by name
                'Using oResourceA As New taradelResourceCenterTableAdapters.VerticalTableAdapter
                '    Dim oResourceT As taradelResourceCenter.VerticalDataTable = oResourceA.ResolveDesignCatToVertical(lnkID)
                '    If oResourceT.Rows.Count > 0 Then
                '        Dim oResource As taradelResourceCenter.VerticalRow = oResourceT.Rows(0)
                '        lnkID = oResource.VerticalID
                '        sLnkTitle = oResource.Name
                '    Else
                '        lnkID = 0
                '    End If
                '    oResourceT.Dispose()
                'End Using
                If lnkID = 0 Then
                    Return "/Industry-Resources.aspx"
                End If
            Case LinkType.DesignDIY
                'Using oVerticalA As New taradelDesignDIYTableAdapters.PageTableAdapter
                '    sLnkTitle = oVerticalA.GetPageTitle(lnkID)
                'End Using
            Case LinkType.DesignDIYTool
                'Using oTemplateA As New taradelDesignDIYTableAdapters.TemplateTableAdapter
                '    sLnkTitle = oTemplateA.GetPathTitle(lnkID)
                'End Using
            Case LinkType.DesignDIYNextSteps
                'Using oTemplateA As New taradelDesignDIYTableAdapters.TemplateTableAdapter
                '    sLnkTitle = oTemplateA.GetPathTitle(lnkID)
                'End Using
            Case LinkType.DesignDIYByProduct
                'Using oProductA As New taradelCatalogTableAdapters.ProductTableAdapter
                '    sLnkTitle = oProductA.GetName(lnkID)
                'End Using
            Case LinkType.USelectTool
                sLnkTitle = "Every Door Direct Mail"
        End Select

        Return SEOLink(sLnkTitle, lnkID, lnkType)
    End Function

    Public Shared Function DePluralizeText(ByVal sTxt As String) As String
        Dim aTxt() As String = Array.CreateInstance(GetType(String), 1)
        If sTxt.Contains("/") Then
            aTxt = sTxt.Split("/")
        Else
            aTxt(0) = sTxt
        End If
        For i As Integer = 0 To aTxt.Length - 1
            Dim sTmp As String = aTxt(i).Trim

            If sTmp.ToLower.EndsWith("s") Then
                sTmp = sTmp.Substring(0, sTmp.Length - 1)
            End If
            aTxt(i) = sTmp
        Next
        Return String.Join("/", aTxt)
    End Function

    Public Shared Function PluralizeText(ByVal sTxt As String) As String
        If Not sTxt.ToLower.EndsWith("s") Then
            sTxt = sTxt & "s"
        End If
        Return sTxt
    End Function

    Public Shared Sub Redirect301(ByVal sURL As String, ByVal endResponse As Boolean)
        If sURL.StartsWith("~") Then
            Dim sQuery As String = ""
            If sURL.Contains("?") Then
                sQuery = sURL.Substring(sURL.IndexOf("?") + 1)
                sURL = sURL.Substring(0, sURL.IndexOf("?"))
            End If
            sURL = VirtualPathUtility.ToAbsolute(sURL)
            If Not String.IsNullOrEmpty(sQuery) Then
                sURL = sURL & "?" & sQuery
            End If
        End If
        Dim oContext As HttpContext = System.Web.HttpContext.Current
        If oContext IsNot Nothing Then
            oContext.Response.StatusCode = 301
            oContext.Response.Status = "301 Moved Permanently"
            oContext.Response.RedirectLocation = sURL
            If endResponse Then
                oContext.Response.End()
            End If
        End If
    End Sub
End Class
