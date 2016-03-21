Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Linq

Public Class TemplateUtility
    Public Property RequireAuth As Boolean '-- Does template access require authentication
    Public Property IndustryId As Integer '-- Current industry, or Site industry for restricted templates
    Public Property BusinessLineId As Integer '-- Restricted Business Line used for sub-brand templates
    Public Property TemplateAccess As String '-- Configured template access level
    Public Property RestrictToTemplates As Boolean = False

    Public Sub New()
        Dim siteId As Integer = appxCMS.Util.CMSSettings.GetSiteId
        Using oTemplates As New TemplateCode.TemplateAPIClient
            Dim oSiteReq As New TemplateCode.GetMyIndustryRequest(siteId)
            Dim oSiteResponse As TemplateCode.GetMyIndustryResponse = oTemplates.GetMyIndustry(oSiteReq)
            If oSiteResponse.GetMyIndustryResult.Count > 0 Then
                Me.IndustryId = oSiteResponse.GetMyIndustryResult.FirstOrDefault.IndustryId
            End If
        End Using

        ApplySettings()
    End Sub

    Public Sub New(industryId As Integer)
        Me.IndustryId = industryId

        ApplySettings()
    End Sub

    Private Sub ApplySettings()
        '-- Set whether they can ONLY use templates
        Dim bDisableTemplates As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "DisableTemplates")
        Dim bDisableProDesign As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "DisableProDesign")
        Dim bDisableUpload As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "DisableUploadArtwork")
        RestrictToTemplates = (bDisableProDesign AndAlso bDisableUpload AndAlso Not bDisableTemplates)

        Dim sTemplateAccess As String = appxCMS.Util.CMSSettings.GetSetting("TemplateAccess", "RestrictMode")
        Dim bRequireAuth As Boolean = False

        Dim iBl As Integer = 0
        Select Case sTemplateAccess.ToLower()
            Case "public", "" '-- set all undefined to public
                '-- don't change anything, we're good this way
            Case "auth" '-- require authorization to view
                bRequireAuth = True
                '-- just make sure they are authenticated before they can see anything
            Case "property" '-- require matching property (defined by TemplateAccess.RestrictProperty
                Dim iInd As Integer = IndustryId
                bRequireAuth = True
                '-- set a custom industry based on their matches from the root industry folder

                '-- Get the current users access property value
                Dim sBrandId As String = Taradel.CustomerDataSource.GetCustomerProperty(HttpContext.Current.User.Identity.Name, "UPROP-BRANDID")
                Dim sBrandList As String = Taradel.CustomerDataSource.GetCustomerProperty(HttpContext.Current.User.Identity.Name, "UPROP-BRANDIDLIST")
                Dim sBrandNames As String = Taradel.CustomerDataSource.GetCustomerProperty(HttpContext.Current.User.Identity.Name, "UPROP-BRANDNAMELIST")

                Dim oBrandIds As List(Of String) = sBrandList.Split(New Char() {","}).ToList()
                Dim oBrands As List(Of String) = sBrandNames.Split(New Char() {","}).ToList()

                Dim sPropVal As String = ""
                Dim idx As Integer = oBrandIds.IndexOf(sBrandId)
                If idx >= 0 Then
                    '-- This will get their current brand id
                    Try
                        sPropVal = oBrands(idx)
                    Catch ex As Exception

                    End Try
                End If

                If Not String.IsNullOrEmpty(sPropVal) Then
                    Using oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("TemplateServerConn").ConnectionString)
                        oConn.Open()

                        Dim sSql As String = ""
                        ''-- what kind of a match are we doing here?
                        'If sAccessMatchMode.ToLower = "contains" Then
                        '    '-- We need to look in a list of possible values

                        '    Dim aPropVals = sPropVal.Split(New Char() {","})
                        '    Dim oBlList As New List(Of String)
                        '    For Each sProp In aPropVals
                        '        oBlList.Add("'" & sProp.Replace("'", "''") & "'")
                        '    Next
                        '    Dim sBlList As String = String.Join(",", oBlList.ToArray())

                        '    sSql = "SELECT TOP 1 BusinessLineId FROM BusinessLine WHERE IndustryId=" & iInd & " AND Name IN (" & sBlList & ")"
                        'Else
                        '    '-- We need to get an exact match
                        '    sSql = "SELECT TOP 1 BusinessLineId FROM BusinessLine WHERE IndustryId=" & iInd & " AND Name='" & sPropVal.Replace("'", "''") & "'"
                        'End If

                        sSql = "SELECT TOP 1 BusinessLineId FROM BusinessLine WHERE IndustryId=" & iInd & " AND Name='" & sPropVal.Replace("'", "''") & "'"

                        Using oCmd As New SqlCommand(sSql, oConn)
                            Dim oBl = oCmd.ExecuteScalar()
                            If oBl IsNot Nothing Then
                                Dim sBl As String = oBl.ToString()
                                Integer.TryParse(sBl, iBl)
                            End If
                        End Using
                    End Using
                Else
                    '-- We can't get this value, so set industry to 0
                    Me.IndustryId = 0
                End If
        End Select

        BusinessLineId = iBl
        RequireAuth = bRequireAuth
    End Sub

    Public Function AvailableSizes() As List(Of Integer)
        Dim oRet As New List(Of Integer)

        Dim sSql As String = ""
        If BusinessLineId > 0 Then
            sSql = "SELECT DISTINCT PageSize from TemplateBusinessLine tbl LEFT JOIN Template t ON t.TemplateId=tbl.TemplateId WHERE BusinessLineId=" & BusinessLineId
        ElseIf IndustryId > 0 Then
            sSql = "SELECT DISTINCT PageSize from TemplateBusinessLine tbl LEFT JOIN BusinessLine bl ON bl.BusinessLineId=tbl.BusinessLineId LEFT JOIN Template t ON t.TemplateId=tbl.TemplateId WHERE bl.IndustryId=" & IndustryId
        Else
            '-- Return all possible sizes
            sSql = "SELECT PageSizeId FROM PageSize"
        End If

        Using oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("TemplateServerConn").ConnectionString)
            oConn.Open()

            Using oCmd As New SqlCommand(sSql, oConn)
                Using oRdr As SqlDataReader = oCmd.ExecuteReader()
                    While oRdr.Read()
                        Dim iSize As Integer = 0
                        Integer.TryParse(oRdr(0).ToString(), iSize)
                        If iSize > 0 Then
                            oRet.Add(iSize)
                        End If
                    End While
                End Using
            End Using
        End Using

        Return oRet
    End Function
End Class
