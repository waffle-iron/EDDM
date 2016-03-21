Imports System.Collections.Generic
Imports System.Linq
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports log4net


Partial Class MyAccount_account_selects
    Inherits MyAccountBase

    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected SelectedTab As Integer = 0


    Protected ReadOnly Property PageNumber As Integer
        Get
            Dim iPg As Integer = QStringToInt("pg")
            If iPg = 0 Then
                iPg = 1
            End If
            Return iPg
        End Get
    End Property

    Protected ReadOnly Property SiteID As Integer

        Get
            Dim _siteID As Integer = appxCMS.Util.CMSSettings.GetSiteId

            If _siteID = 0 Then
                _siteID = 1
            End If

            Return _siteID

        End Get

    End Property


    Protected PageSize As Integer = 20
    Protected TotalRecords As Integer = 0

    Protected iStartImportRow As Integer = -1
    Protected iCUZipCol As Integer = -1
    Protected iCUCRCol As Integer = -1
    Protected iCUSFDUCol As Integer = -1
    Protected iCUMFDUCol As Integer = -1
    Protected iCUResCol As Integer = -1
    Protected iCUBizCol As Integer = -1
    Protected iCUBoxCol As Integer = -1

    Protected sCUZipCol As String = ""
    Protected sCUCRCol As String = ""
    Protected sCUSFDUCol As String = ""
    Protected sCUMFDUCol As String = ""
    Protected sCUResCol As String = ""
    Protected sCUBizCol As String = ""
    Protected sCUBoxCol As String = ""

    Protected sMinCol As String = ""
    Protected sMaxCol As String = ""

    Protected iMinCol As Integer = 999999
    Protected iMaxCol As Integer = 0

    Protected CustomerId As Integer = GetCustomerId




    Protected Sub Page_Init1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim siteID As Integer = appxCMS.Util.CMSSettings.GetSiteId()
        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(siteID)

        BindList()

        'Set Page Header control
        If Not (SiteDetails.UseRibbonBanners) Then
            PageHeader.headerType = "simple"
            PageHeader.simpleHeader = "Saved Lists and Maps"
        Else
            PageHeader.headerType = "full"
            PageHeader.fullHeader = "Saved U-Select Maps"
        End If

        'oSelects.SelectParameters("CustomerId").DefaultValue = Me.CustomerId

        Dim bCustomUSelect As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Postal", "EnableCustomUSelect")
        If bCustomUSelect Then

            phCustomUploadTab.Visible = True
            phCustomUpload.Visible = True

            iStartImportRow = appxCMS.Util.CMSSettings.GetInteger("Postal", "CustomUSelectImportRow")
            sCUZipCol = appxCMS.Util.CMSSettings.GetSetting("Postal", "CustomUSelectZipCol")
            sCUCRCol = appxCMS.Util.CMSSettings.GetSetting("Postal", "CustomUSelectCRCol")
            sCUSFDUCol = appxCMS.Util.CMSSettings.GetSetting("Postal", "CustomUSelectSFDUCol")
            sCUMFDUCol = appxCMS.Util.CMSSettings.GetSetting("Postal", "CustomUSelectMFDUCol")
            sCUResCol = appxCMS.Util.CMSSettings.GetSetting("Postal", "CustomUSelectResCol")
            sCUBizCol = appxCMS.Util.CMSSettings.GetSetting("Postal", "CustomUSelectBizCol")
            sCUBoxCol = appxCMS.Util.CMSSettings.GetSetting("Postal", "CustomUSelectPOBoxCol")

            AdjustExcelColNameToIndex(sCUZipCol, iCUZipCol)
            AdjustExcelColNameToIndex(sCUCRCol, iCUCRCol)
            AdjustExcelColNameToIndex(sCUSFDUCol, iCUSFDUCol)
            AdjustExcelColNameToIndex(sCUMFDUCol, iCUMFDUCol)
            AdjustExcelColNameToIndex(sCUResCol, iCUResCol)
            AdjustExcelColNameToIndex(sCUBizCol, iCUBizCol)
            AdjustExcelColNameToIndex(sCUBoxCol, iCUBoxCol)

            Dim oExplanation As New StringBuilder

            oExplanation.Append("<ol>")
            oExplanation.Append("<li>Data in rows before <mark>Row " & iStartImportRow & "</mark> in the spreadsheet will NOT be imported.</li>")
            oExplanation.Append("<li>The zip code must be in column <mark>" & sCUZipCol & "</mark>.</li>")
            oExplanation.Append("<li>The carrier route must be in column <mark>" & sCUCRCol & "</mark>.</li>")
            oExplanation.Append("<li>Rows without data in the zip code (" & sCUZipCol & ") and carrier route (" & sCUCRCol & ") columns will be ignored.</li>")

            If Not String.IsNullOrEmpty(sCUSFDUCol) And Not String.IsNullOrEmpty(sCUMFDUCol) Then
                oExplanation.Append("<li>The single family delivery count (SFDU) must be in column <mark>" & sCUSFDUCol & "</mark>.</li>")
                oExplanation.Append("<li>The multi-family delivery count (MFDU) must be in column <mark>" & sCUMFDUCol & "</mark>.</li>")
            End If

            If Not String.IsNullOrEmpty(sCUResCol) Then
                oExplanation.Append("<li>The total residential deliveries must be in column <mark>" & sCUResCol & "</mark>.</li>")
            End If

            If Not String.IsNullOrEmpty(sCUBizCol) Then
                oExplanation.Append("<li>The business delivery total must be in column <mark>" & sCUBizCol & "</mark>.</li>")
            End If

            If Not String.IsNullOrEmpty(sCUBoxCol) Then
                oExplanation.Append("<li>The PO box delivery total must be in column <mark>" & sCUBoxCol & "</mark>.</li>")
            End If

            oExplanation.Append("<li>Data import will run until 10 rows are encountered without a zip code and a carrier route.</li>")
            oExplanation.Append("</ol>")

            lCustomExplanation.Text = oExplanation.ToString

            Dim oSample As New StringBuilder
            'oSample.Append("<table border=""1"" cellpadding=""2"" cellspacing=""0"">")
            'oSample.Append("<tr>")
            'oSample.Append("<th style=""background-color:#CCC;color:#000;"">&nbsp;</th>")
            oSample.Append("<table class=""table table-condensed table-bordered table-striped"">")
            oSample.Append("<tr>")
            oSample.Append("<th>&nbsp;</th>")

            For iCol As Integer = 0 To iMaxCol
                oSample.Append("<th style=""background-color:#dddddd;"">" & Chr(iCol + 65) & "</th>")
            Next

            oSample.Append("<th>Description</th>")
            oSample.Append("</tr>")

            For iRow As Integer = 0 To iStartImportRow + 5
                Dim oRowMsg As New List(Of String)
                Dim sCellStyle As String = ""

                If iRow + 1 < iStartImportRow Then
                    sCellStyle = "background-color:#dddddd;color:#999999;"
                    oRowMsg.Add("Not Imported")
                ElseIf iRow + 1 = iStartImportRow Then
                    oRowMsg.Add("Start Import At Row " & iStartImportRow)
                End If

                oSample.Append("<tr>")
                oSample.Append("<th style=""background-color:#dddddd;"">" & iRow + 1 & "</th>")
                For i As Integer = 0 To iMaxCol
                    Dim sCellVal As String = "&nbsp;"
                    Dim sCurCol As String = Chr(i + 65)

                    If iRow + 1 >= iStartImportRow Then
                        If sCurCol = sCUZipCol Then
                            sCellVal = "Zip"
                        ElseIf sCurCol = sCUCRCol Then
                            sCellVal = "CR"
                        ElseIf sCurCol = sCUSFDUCol Then
                            sCellVal = "SFDU"
                        ElseIf sCurCol = sCUMFDUCol Then
                            sCellVal = "MFDU"
                        ElseIf sCurCol = sCUResCol Then
                            sCellVal = "Res"
                        ElseIf sCurCol = sCUBizCol Then
                            sCellVal = "Biz"
                        ElseIf sCurCol = sCUBoxCol Then
                            sCellVal = "PO Box"
                        End If
                    End If

                    oSample.Append("<td" & IIf(Not String.IsNullOrEmpty(sCellStyle), " style=""" & sCellStyle & """", "") & ">" & sCellVal & "</td>")
                Next
                '-- Description text out here
                oSample.Append("<td" & IIf(Not String.IsNullOrEmpty(sCellStyle), " style=""" & sCellStyle & """", "") & ">" & String.Join("; ", oRowMsg.ToArray) & "</td>")
                oSample.Append("</tr>")
            Next

            oSample.Append("</table>")

            lCustomSample.Text = oSample.ToString

        End If
    End Sub


    Protected Sub BindList()
        Dim oSavedList As List(Of Taradel.CustomerDistribution) = Taradel.CustomerDistributions.GetList(Me.CustomerId, False)
        TotalRecords = oSavedList.Count

        Dim iCurPg As Integer = Me.PageNumber - 1
        If iCurPg <= 0 Then iCurPg = 0
        Dim oSaved As List(Of Taradel.CustomerDistribution) = oSavedList.Skip(iCurPg * Me.PageSize).Take(Me.PageSize).ToList()

        lvSelects.DataSource = oSaved
        lvSelects.DataBind()

    End Sub


    Protected Sub AdjustExcelColNameToIndex(ByRef sColName As String, ByRef iColIndex As Integer)
        If Not String.IsNullOrEmpty(sColName.Trim) Then
            Dim iRes As Integer
            If Not Integer.TryParse(sColName.Trim, iRes) Then
                Dim iAlpha As Integer = Asc(sColName.ToUpper.Trim)
                iRes = iAlpha - 65
                iColIndex = iRes
            Else
                iColIndex = iRes
            End If

            sColName = Chr(iColIndex + 65)

            If iColIndex < iMinCol Then
                iMinCol = iColIndex
                sMinCol = sColName
            ElseIf iColIndex > iMaxCol Then
                iMaxCol = iColIndex
                sMaxCol = sColName
            End If
        End If
    End Sub


    Protected Sub lvSelects_DataBound(sender As Object, e As System.EventArgs) Handles lvSelects.DataBound
        Dim pgTop As UserControls_DataPager = DirectCast(lvSelects.FindControl("pgTop"), UserControls_DataPager)
        If pgTop IsNot Nothing Then
            If TotalRecords < PageSize Then
                pgTop.Visible = False
            Else
                pgTop.CurPage = Me.PageNumber
                pgTop.PageSize = Me.PageSize
                pgTop.ResultCount = TotalRecords
                pgTop.NavigatePage = Page.AppRelativeVirtualPath
                pgTop.QuerystringField = "pg"
            End If
        End If

        Dim pgBottom As UserControls_DataPager = DirectCast(lvSelects.FindControl("pgBottom"), UserControls_DataPager)
        If pgBottom IsNot Nothing Then
            If TotalRecords < PageSize Then
                pgBottom.Visible = False
            Else
                pgBottom.CurPage = Me.PageNumber
                pgBottom.PageSize = Me.PageSize
                pgBottom.ResultCount = TotalRecords
                pgBottom.NavigatePage = Page.AppRelativeVirtualPath
                pgBottom.QuerystringField = "pg"
            End If
        End If
    End Sub


    Protected Sub lvSelects_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvSelects.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim oDItem As ListViewDataItem = e.Item

            Dim oDesc As New List(Of String)
            oDesc.Add("Residential")

            Dim oSelect As Taradel.CustomerDistribution = DirectCast(oDItem.DataItem, Taradel.CustomerDistribution)
            Dim oSummary As Taradel.MapServer.UserData.SelectionSummary = Taradel.CustomerDistributions.GetSelectionSummary(oSelect.ReferenceId)

            If oSummary IsNot Nothing Then
                Dim bBiz As Boolean = oSummary.UseBusiness
                Dim bBox As Boolean = oSummary.UsePOBox
                If bBiz Then oDesc.Add("Business")
                If bBox Then oDesc.Add("PO Boxes")
                Dim iTotal As Integer = oSelect.TotalDeliveries

                If Not bBiz Or Not bBox Then
                    iTotal = 0
                    Dim oSelectAreas As List(Of Taradel.MapServer.UserData.AreaSelection) = Taradel.CustomerDistributions.GetSelections(oSelect.ReferenceId)
                    For Each oSelectArea As Taradel.MapServer.UserData.AreaSelection In oSelectAreas
                        iTotal = iTotal + oSelectArea.Residential
                        If bBiz Then iTotal = iTotal + oSelectArea.Business
                        If bBox Then iTotal = iTotal + oSelectArea.POBoxes
                    Next
                End If

                Dim lTotalDeliveries As Literal = DirectCast(e.Item.FindControl("lTotalDeliveries"), Literal)
                Dim lTotalDeliveriesXS As Literal = DirectCast(e.Item.FindControl("lTotalDeliveriesXS"), Literal)

                Dim sDesc As String = String.Join(", ", oDesc.ToArray())

                'lTotalDeliveries.Text = iTotal.ToString("N0") & "<div><sub>" & sDesc & "</sub></div>"
                lTotalDeliveries.Text = iTotal.ToString("N0") & "<br />" & sDesc
                lTotalDeliveriesXS.Text = iTotal.ToString("N0") & " " & sDesc

            End If

            Dim iSelectId As Integer = oSelect.USelectMethod.USelectId

            If iSelectId = 1 Then

                Dim hplOpenSelect As HyperLink = DirectCast(e.Item.FindControl("hplOpenSelect"), HyperLink)
                Dim hypOpenSelectXS As HyperLink = DirectCast(e.Item.FindControl("hypOpenSelectXS"), HyperLink)

                'OLB.
                If SiteID = 11 Then
                    hplOpenSelect.NavigateUrl = "~/olb/TargetDataMap1.aspx?distid=" & oSelect.DistributionId
                    hypOpenSelectXS.NavigateUrl = "~/olb/TargetDataMap1.aspx?distid=" & oSelect.DistributionId

                'Everyone else
                Else
                    hplOpenSelect.NavigateUrl = "~/Step1-TargetReview.aspx?distid=" & oSelect.DistributionId
                    hypOpenSelectXS.NavigateUrl = "~/Step1-TargetReview.aspx?distid=" & oSelect.DistributionId
                End If

            Else
                e.Item.Visible = False
            End If

        End If
    End Sub


    Protected Sub OpenMapForEdit(sender As Object, e As System.EventArgs)

        Dim oLnk As LinkButton = DirectCast(sender, LinkButton)

        If oLnk IsNot Nothing Then
            Dim sCmd As String = oLnk.CommandArgument
            Dim aCmd As String() = sCmd.Split("|")

            Dim sDistId As String = aCmd(0)
            Dim ReferenceId As String = aCmd(1)
            Dim DistributionId As Integer = 0
            Integer.TryParse(sDistId, DistributionId)

            Dim oSummary As Taradel.MapServer.UserData.SelectionSummary = Taradel.CustomerDistributions.GetSelectionSummary(ReferenceId)

            If oSummary IsNot Nothing Then
                Dim sAddr As String = oSummary.StartAddress.ToString
                Dim sZip As String = oSummary.StartZipCode.ToString

                If String.IsNullOrEmpty(sAddr) Or String.IsNullOrEmpty(sZip) Then
                    ''-- We can't open this guy
                    'lMsg.Text = UpdateStatusMsg("Cannot open the selected project for editing, as it is missing a starting address.", True)
                    '-- Send them forward without the address, let the map handle it
                    Response.Redirect("~/Step1-Target.aspx?distid=" & DistributionId)
                Else
                    Response.Redirect("~/Step1-Target.aspx?distid=" & DistributionId & "&addr=" & Server.UrlEncode(sAddr) & "&zip=" & Server.UrlEncode(sZip))
                End If

            Else

                'lMsg.Text = UpdateStatusMsg("Cannot locate the project summary detail need to edit the project.", True)
                ShowError("Cannot locate the project summary detail need to edit the project.", "myprojects")
            End If
        End If
    End Sub


    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        '    Dim oJs As New StringBuilder
        '    oJs.AppendLine("jQuery(document).ready(function($) {")
        '    oJs.AppendLine("    $('#selectTabs').tabs({selected:" & Me.SelectedTab & "});")
        '    oJs.AppendLine("    $('#combinedSettings').dialog({")
        '    oJs.AppendLine("        autoOpen:false,")
        '    oJs.AppendLine("        modal:true,")
        '    oJs.AppendLine("        width:600,height:400,")
        '    oJs.AppendLine("        open:function() { jQuery(this).parent().appendTo('form:first'); }")
        '    oJs.AppendLine("    });")
        '    oJs.AppendLine("    $('#" & lnkCombine.ClientID & "').click(function(e) {")
        '    oJs.AppendLine("        e.preventDefault();")
        '    oJs.AppendLine("        $('#combinedSettings').dialog('open');")
        '    oJs.AppendLine("    });")
        '    'oJs.AppendLine("    $('#customAccordion').accordion({")
        '    'oJs.AppendLine("        active:2, heightStyle:'content'")
        '    'oJs.AppendLine("    });")
        '    oJs.AppendLine("});")
        '    jqueryHelper.RegisterClientScript(Page, "AccountSelectsInit", oJs.ToString)
    End Sub


    Protected Sub lnkGetStarted_Click(sender As Object, e As System.EventArgs) Handles lnkGetStarted.Click
        '    Me.SelectedTab = 1
        Page.Validate(lnkGetStarted.ValidationGroup)
        If Page.IsValid Then
            Response.Redirect("~/Step1-Target.aspx?addr=" & Server.UrlEncode(StreetAddress.Text) & "&zip=" & Server.UrlEncode(ZipCode.Text))
        End If
    End Sub


    Protected Sub lnkCreateCombined_Click(sender As Object, e As System.EventArgs) Handles lnkCreateCombined.Click

        Dim sSelectName As String = CombinedName.Text
        Dim bBiz As Boolean = chkCombinedBusiness.Checked
        Dim bPO As Boolean = chkCombinedPOBoxes.Checked

        Dim oDistIds As New List(Of Integer)

        For Each oLVI As ListViewItem In lvSelects.Items
            If oLVI.ItemType = ListViewItemType.DataItem Then
                Dim chkCombine As CheckBox = DirectCast(oLVI.FindControl("chkCombine"), CheckBox)
                If chkCombine IsNot Nothing Then
                    If chkCombine.Checked Then
                        Dim hfDistributionId As HiddenField = DirectCast(oLVI.FindControl("hfDistributionId"), HiddenField)
                        If hfDistributionId IsNot Nothing Then
                            Dim DistId As Integer
                            If Integer.TryParse(hfDistributionId.Value, DistId) Then
                                oDistIds.Add(DistId)
                            End If
                        End If
                    End If
                End If
            End If
        Next

        If oDistIds.Count > 1 Then
            Dim oNewAreas As New List(Of Taradel.MapServer.UserData.AreaSelection)
            For Each oDistId As Integer In oDistIds
                Dim DistId As Integer = oDistId

                Dim oDistAreas As List(Of Taradel.MapServer.UserData.AreaSelection) = Taradel.CustomerDistributions.GetSelections(oDistId)
                For Each oDistArea In oDistAreas
                    Dim sAreaName As String = oDistArea.Name
                    Dim oFound As Taradel.MapServer.UserData.AreaSelection = oNewAreas.FirstOrDefault(Function(a) a.Name.Equals(sAreaName, System.StringComparison.OrdinalIgnoreCase))
                    If oFound Is Nothing Then
                        oNewAreas.Add(oDistArea)
                    End If
                Next
            Next

            Dim sSaveMsg As String = ""
            Dim oMSClient As New Taradel.MapServer.UserData.UserDataClient
            Dim oSaveReq As New Taradel.MapServer.UserData.SaveSelectionsRequest(sSelectName, oNewAreas, "", "", bBiz, bPO, sSaveMsg)
            Dim oSaveResp As Taradel.MapServer.UserData.SaveSelectionsResponse = oMSClient.SaveSelections(oSaveReq)
            Dim sReferenceId As String = oSaveResp.SaveSelectionsResult

            If String.IsNullOrEmpty(sReferenceId) Then

                '-- Check for an error in the msg
                If Not String.IsNullOrEmpty(oSaveResp.sMsg) Then
                    'lMsg.Text = UpdateStatusMsg("There was a problem creating the combined project: " & oSaveResp.sMsg, True)
                    ShowError("There was a problem creating the combined project: " & oSaveResp.sMsg & ".", "myprojects")
                Else
                    'lMsg.Text = UpdateStatusMsg("There was a problem creating the combined project.", True)
                    ShowError("There was a problem creating the combined project.", "myprojects")
                End If
            Else
                '-- Save this to their account
                Dim NewDistId As Integer = Taradel.CustomerDistributions.Save(Me.GetCustomerId, sReferenceId, 1, sSaveMsg)
                If NewDistId = 0 Then
                    'lMsg.Text = UpdateStatusMsg("There was a problem linking the combined project to your account.", True)
                    ShowError("There was a problem linking the combined project to your account.", "myprojects")
                Else
                    'lMsg.Text = UpdateStatusMsg("Your combined project '" & sSelectName & "' has been saved!")
                    ShowSuccess("Your combined project '" & sSelectName & "' has been saved.", "myprojects")
                    BindList()
                End If
            End If

        Else

            If oDistIds.Count = 1 Then
                'lMsg.Text = UpdateStatusMsg("You must choose more than one project to create a combined project.", True)
                ShowError("You must choose more than one project to create a combined project.", "myprojects")
            Else
                'lMsg.Text = UpdateStatusMsg("You must choose at least two projects to create a combined project.", True)
                ShowError("You must choose at least two projects to create a combined project.", "myprojects")
            End If

        End If

    End Sub


    Protected Sub lnkUpload_Click(sender As Object, e As System.EventArgs) Handles lnkUpload.Click

        'Me.SelectedTab = 3
        If CustomFile.HasFile Then
            Dim sFile As String = CustomFile.FileName
            Dim sExt As String = Path.GetExtension(sFile).ToLowerInvariant()

            If sExt = ".xls" Or sExt = ".xlsx" Then
                Dim sDistRefId As String = System.Guid.NewGuid.ToString
                Dim sFileName As String = Path.Combine(Server.MapPath("~/app_data/tmp"), sDistRefId & sExt)
                CustomFile.MoveTo(sFileName, Brettle.Web.NeatUpload.MoveToOptions.None)

                If File.Exists(sFileName) Then
                    Log.Info("Importing custom file: " & sFileName)
                    Dim oExcel As New appxCMS.Reporting.ExcelDataHelper()

                    oExcel.ExcelFilePath = sFileName
                    oExcel.HeaderRow = False
                    oExcel.LimitScanRows = True

                    Dim oSelects As New List(Of Taradel.MapServer.UserData.AreaSelection)

                    Dim iBizTotal As Integer = 0
                    Dim iResTotal As Integer = 0
                    Dim iBoxTotal As Integer = 0

                    Using oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("MapServerConn").ConnectionString)
                        oConn.Open()

                        Dim oSheets As List(Of String) = oExcel.WorkSheetNames()
                        Response.Write("Custom file has " & oSheets.Count & " Worksheets<br/>")
                        Log.Info("Custom file has " & oSheets.Count & " Worksheets")
                        For iSheet As Integer = 0 To oSheets.Count - 1
                            Dim sWorkSheet As String = oSheets(iSheet)

                            Using oDt As DataTable = oExcel.ExcelToDataTableCustomRange("SELECT * FROM [" & sWorkSheet & sMinCol & iStartImportRow & ":" & sMaxCol & "]")
                                Dim iEmpty As Integer = 0
                                Response.Write("Import file has " & oDt.Rows.Count & " rows.<br/>")
                                Log.Info("Import file has " & oDt.Rows.Count & " rows.")
                                For iRow As Integer = 0 To oDt.Rows.Count - 1
                                    '-- Stop processing after 10 not qualifying rows are encountered
                                    If iEmpty > 10 Then
                                        Exit For
                                    End If
                                    Dim oRow As DataRow = oDt.Rows(iRow)

                                    If Not oRow(iCUZipCol) Is DBNull.Value And Not oRow(iCUCRCol) Is DBNull.Value Then
                                        Dim sZip As String = oRow(iCUZipCol).ToString.Trim
                                        If sZip.Length = 4 Then
                                            sZip = "0" & sZip
                                        End If
										If sZip.Length = 3 Then
											sZip = "00" & sZip
										End If
                                        Dim sCR As String = oRow(iCUCRCol).ToString.Trim

                                        Log.Info("Importing " & sZip & ", " & sCR)

                                        If Not String.IsNullOrEmpty(sZip) And Not String.IsNullOrEmpty(sCR) Then
                                            iEmpty = 0

                                            Dim iRes As Integer = 0
                                            Dim iBiz As Integer = 0
                                            Dim iBox As Integer = 0

                                            If iCUSFDUCol >= 0 And iCUMFDUCol >= 0 Then
                                                Dim iSFDU As Integer = 0
                                                Dim iMFDU As Integer = 0

                                                If Not oRow(iCUSFDUCol) Is DBNull.Value Then
                                                    Integer.TryParse(oRow(iCUSFDUCol), iSFDU)
                                                End If

                                                If Not oRow(iCUMFDUCol) Is DBNull.Value Then
                                                    Integer.TryParse(oRow(iCUMFDUCol), iMFDU)
                                                End If

                                                iRes = iSFDU + iMFDU

                                            ElseIf iCUResCol >= 0 Then
                                                If Not oRow(iCUResCol) Is DBNull.Value Then
                                                    Integer.TryParse(oRow(iCUResCol), iRes)
                                                End If
                                            End If

                                            If iCUBizCol < oDt.Columns.Count Then
                                                If iCUBizCol >= 0 Then
                                                    If Not oRow(iCUBizCol) Is DBNull.Value Then
                                                        Integer.TryParse(oRow(iCUBizCol), iBiz)
                                                    End If
                                                End If
                                            End If

                                            If iCUBoxCol < oDt.Columns.Count Then
                                                If iCUBoxCol >= 0 Then
                                                    If Not oRow(iCUBoxCol) Is DBNull.Value Then
                                                        Integer.TryParse(oRow(iCUBoxCol), iBox)
                                                    End If
                                                End If
                                            End If

                                            Dim oArea As New Taradel.MapServer.UserData.AreaSelection
                                            oArea.Business = iBiz
                                            oArea.Residential = iRes
                                            oArea.POBoxes = iBox
                                            oArea.Total = iBiz + iRes + iBox
                                            oArea.Name = sZip & sCR

                                            Try
                                                Using oCmd As New SqlCommand("select top 1 City + ', ' + State As Name from CarrierRouteDistribution where GeocodeRef='" & oArea.Name & "'", oConn)
                                                    Dim sName As String = oCmd.ExecuteScalar.ToString()
                                                    oArea.FriendlyName = sName
                                                End Using
                                            Catch ex As Exception
                                                oArea.FriendlyName = oArea.Name
                                            End Try

                                            oSelects.Add(oArea)

                                            iResTotal = iResTotal + iRes
                                            iBizTotal = iBizTotal + iBiz
                                            iBoxTotal = iBoxTotal + iBox
                                        Else
                                            iEmpty = iEmpty + 1
                                        End If
                                    Else
                                        iEmpty = iEmpty + 1
                                    End If
                                Next
                            End Using
                        Next

                        Dim sSelects As String = Taradel.JavascriptSerializer.Serialize(Of List(Of Taradel.MapServer.UserData.AreaSelection))(oSelects)

                        Using oCmd As New SqlCommand("INSERT INTO SavedSelection (ReferenceId, CreatedDate, CreatedIPAddress, Name, StartAddress, StartZipCode, UseBusiness, UsePOBoxes, Selection) VALUES (@DistRefId, GetDate(), @IPAddress, @DistName, '', '', @UseBusiness, @UsePOBox, @Dist)", oConn)
                            oCmd.Parameters.AddWithValue("@DistRefId", sDistRefId)
                            oCmd.Parameters.AddWithValue("@IPAddress", Request.UserHostAddress)
                            oCmd.Parameters.AddWithValue("@DistName", CustomName.Text)
                            oCmd.Parameters.AddWithValue("@UseBusiness", IIf(iBizTotal > 0, 1, 0))
                            oCmd.Parameters.AddWithValue("@UsePOBox", IIf(iBoxTotal > 0, 1, 0))
                            oCmd.Parameters.AddWithValue("@Dist", sSelects)

                            oCmd.ExecuteScalar()
                        End Using
                        oConn.Close()
                    End Using

                    Using oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString)
                        oConn.Open()

                        Using oCmd As New SqlCommand("INSERT INTO pnd_CustomerDistribution (CustomerId, Name, CreatedDate, ReferenceId, TotalDeliveries, USelectId) values (@CustomerId, @DistName, GetDate(), @DistRefId, @TotalDeliveries, @USelectId)", oConn)
                            oCmd.Parameters.AddWithValue("@CustomerId", Me.CustomerId)
                            oCmd.Parameters.AddWithValue("@DistName", CustomName.Text)
                            oCmd.Parameters.AddWithValue("@DistRefId", sDistRefId)
                            oCmd.Parameters.AddWithValue("@TotalDeliveries", iResTotal + iBizTotal + iBoxTotal)
                            oCmd.Parameters.AddWithValue("@USelectId", 1)

                            oCmd.ExecuteScalar()
                        End Using

                        oConn.Close()
                    End Using

                    '-- Delete their uploaded file
                    Try
                        File.Delete(sFileName)
                    Catch ex As Exception
                        Log.Error(ex.Message, ex)
                        Log.Info("Error deleting custom import file: " & sFileName)
                    End Try

                    Response.Redirect(Page.AppRelativeVirtualPath)

                Else
                    ShowError("There was a problem saving your file. Please try again.", "customfiles")
                    'lCustomMsg.Text = UpdateStatusMsg("There was a problem saving your file. Please try again.", True)

                End If

            Else
                'lCustomMsg.Text = UpdateStatusMsg("You must upload a Microsoft Excel file to continue.", True)
                ShowError("You must upload a Microsoft Excel file to continue.", "customfiles")
            End If

        Else

            'lCustomMsg.Text = UpdateStatusMsg("You must choose a file to continue.", True)
            ShowError("You must choose a file to continue.", "customfiles")

        End If

    End Sub


    Protected Sub lnkProcessPasteData_Click(sender As Object, e As System.EventArgs) Handles lnkProcessPasteData.Click

        'Me.SelectedTab = 2

        Page.Validate("vgPasteData")
        If Not Page.IsValid Then
            Exit Sub
        End If

        Dim sPasteData As String = PasteData.Text.Trim()
        If Not String.IsNullOrEmpty(sPasteData) Then
            Dim sDistRefId As String = System.Guid.NewGuid.ToString

            Dim aPasteData As String() = sPasteData.Split(New Char() {",", ";", ControlChars.NewLine, ControlChars.CrLf, ControlChars.Cr, ControlChars.Lf})
            If aPasteData.Length = 0 Then
                'lPasteMsg.Text = UpdateStatusMsg("No lines could be parsed from the provided data.", True)
                ShowError("No lines could be parsed from the provided data.", "pastezips")
                Exit Sub
            End If

            '...new 4/29/2015 - scrub any repeats from the string - should do same for file upload
            Dim aPasteData2 As New List(Of String)
            For i As Integer = 0 To aPasteData.Length - 1
                Dim sZip As String = aPasteData(i).Trim()
                If (aPasteData2.Contains(sZip)) Then
                Else
                    aPasteData2.Add(sZip)
                End If
            Next
            aPasteData = aPasteData2.ToArray()
            '...end new 4/29/2015

            Dim oSelects As New List(Of Taradel.MapServer.UserData.AreaSelection)

            Dim iBizTotal As Integer = 0
            Dim iResTotal As Integer = 0
            Dim iBoxTotal As Integer = 0

            Using oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("MapServerConn").ConnectionString)
                oConn.Open()
                For i As Integer = 0 To aPasteData.Length - 1
                    Dim sZip As String = aPasteData(i).Trim()

                    If Not String.IsNullOrEmpty(sZip) Then

                        '-- Load all of the carrier routes in this zip code
                        Try
                            '-- Account for zip code or zip + carrier route name
                            Dim sSql As String = "select GeocodeRef, City + ', ' + State As Name, ResidentialTotal, BusinessActive, BoxCount from CarrierRouteDistribution WHERE ZipCode='" & sZip & "'"
                            If sZip.Length > 5 Then
                                sZip = sZip.Replace(" ", "")
                                sSql = "select GeocodeRef, City + ', ' + State As Name, ResidentialTotal, BusinessActive, BoxCount from CarrierRouteDistribution WHERE GeocodeRef='" & sZip & "'"
                            End If

                            Using oCmd As New SqlCommand(sSql, oConn)
                                Dim oRdr As SqlDataReader = oCmd.ExecuteReader()
                                If oRdr.HasRows Then

                                    While oRdr.Read()
                                        Dim bAdded As Boolean = False
                                        Dim sGeocodeRef As String = oRdr.GetString(0)
                                        Dim sCr As String = sGeocodeRef.Substring(5)

                                        Dim iBiz As Integer = 0
                                        If chkPasteBusiness.Checked Then iBiz = oRdr.GetInt32(3)
                                        Dim iRes As Integer = oRdr.GetInt32(2)
                                        Dim iBox As Integer = 0
                                        If chkPastePOBoxes.Checked Then iBox = oRdr.GetInt32(4)

                                        Dim oArea As Taradel.MapServer.UserData.AreaSelection = Nothing
                                        '-- Collapse "B" routes into a single PBOX
                                        If sCr.ToUpper().StartsWith("B") Then
                                            If iBox > 0 Then
                                                Dim sAreaZip As String = sGeocodeRef.Substring(0, 5)
                                                sGeocodeRef = sAreaZip & "PBOX"
                                                oArea = oSelects.FirstOrDefault(Function(a) a.Name.Equals(sGeocodeRef, StringComparison.OrdinalIgnoreCase))
                                                If oArea Is Nothing Then
                                                    oArea = New Taradel.MapServer.UserData.AreaSelection
                                                    oArea.Name = sGeocodeRef
                                                    oArea.FriendlyName = oRdr.GetString(1)
                                                    bAdded = True
                                                End If
                                                oArea.Business = 0
                                                oArea.Residential = 0
                                                oArea.POBoxes = oArea.POBoxes + iBox
                                                oArea.Total = oArea.POBoxes
                                                If bAdded Then
                                                    oSelects.Add(oArea)
                                                End If
                                                iBoxTotal = iBoxTotal + iBox
                                            End If                                            
                                        Else
                                            If iRes > 0 Or iBiz > 0 Then
                                                oArea = New Taradel.MapServer.UserData.AreaSelection
                                                oArea.Name = sGeocodeRef
                                                oArea.FriendlyName = oRdr.GetString(1)
                                                oArea.Business = iBiz
                                                oArea.Residential = iRes
                                                oArea.POBoxes = 0
                                                oArea.Total = iRes + iBiz
                                                oSelects.Add(oArea)
                                                iResTotal = iResTotal + iRes
                                                iBizTotal = iBizTotal + iBiz
                                            End If                                            
                                        End If
                                    End While
                                End If
                                oRdr.Close()
                            End Using
                        Catch ex As Exception
                            Log.Error(ex.Message, ex)
                            ShowError("An unexpected error occurred: " & ex.Message & ".", "pastezips")
                        End Try
                    End If
                Next
            End Using

            Dim sSelects As String = Taradel.JavascriptSerializer.Serialize(Of List(Of Taradel.MapServer.UserData.AreaSelection))(oSelects)

            Using oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("MapServerConn").ConnectionString)
                oConn.Open()

                Using oCmd As New SqlCommand("INSERT INTO SavedSelection (ReferenceId, CreatedDate, CreatedIPAddress, Name, StartAddress, StartZipCode, UseBusiness, UsePOBoxes, Selection) VALUES (@DistRefId, GetDate(), @IPAddress, @DistName, '', '', @UseBusiness, @UsePOBox, @Dist)", oConn)
                    oCmd.Parameters.AddWithValue("@DistRefId", sDistRefId)
                    oCmd.Parameters.AddWithValue("@IPAddress", Request.UserHostAddress)
                    oCmd.Parameters.AddWithValue("@DistName", PasteDataName.Text)
                    oCmd.Parameters.AddWithValue("@UseBusiness", IIf(chkPasteBusiness.Checked, 1, 0))
                    oCmd.Parameters.AddWithValue("@UsePOBox", IIf(chkPastePOBoxes.Checked, 1, 0))
                    oCmd.Parameters.AddWithValue("@Dist", sSelects)

                    oCmd.ExecuteScalar()
                End Using
                oConn.Close()
            End Using

            Using oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString)
                oConn.Open()

                Using oCmd As New SqlCommand("INSERT INTO pnd_CustomerDistribution (CustomerId, Name, CreatedDate, ReferenceId, TotalDeliveries, USelectId) values (@CustomerId, @DistName, GetDate(), @DistRefId, @TotalDeliveries, @USelectId)", oConn)
                    oCmd.Parameters.AddWithValue("@CustomerId", Me.CustomerId)
                    oCmd.Parameters.AddWithValue("@DistName", PasteDataName.Text)
                    oCmd.Parameters.AddWithValue("@DistRefId", sDistRefId)
                    oCmd.Parameters.AddWithValue("@TotalDeliveries", iResTotal + iBizTotal + iBoxTotal)
                    oCmd.Parameters.AddWithValue("@USelectId", 1)

                    oCmd.ExecuteScalar()
                End Using

                oConn.Close()
            End Using

            Response.Redirect(Page.AppRelativeVirtualPath)
        Else
            'lPasteMsg.Text = UpdateStatusMsg("You must enter one or more zip code or zip code and carrier route combinations into the box.", True)
            ShowError("You must enter one or more zip code or zip code and carrier route combinations into the box.", "pastezips")
        End If
    End Sub


    Protected Sub DeleteUSelect(sender As Object, e As System.EventArgs)
        Dim oLnk As LinkButton = DirectCast(sender, LinkButton)

        Dim sDistId As String = oLnk.CommandArgument
        Dim iDistId As Integer = 0
        If Integer.TryParse(sDistId, iDistId) Then
            Dim sMsg As String = ""
            Dim bDeleted As Boolean = Taradel.CustomerDistributions.Delete(iDistId, sMsg)

            If bDeleted Then
                BindList()
            Else
                'lMsg.Text = UpdateStatusMsg("There was a problem removing your distribution.", True)
                ShowError("There was a problem removing your distribution.", "myprojects")
            End If
        End If
    End Sub


    Protected Sub ShowError(errorMsg As String, tabName As String)

        Select Case tabName
            Case "myprojects"
                pnlProjSuccess.Visible = False
                pnlProjError.Visible = True
                lblProjError.Text = errorMsg
            Case "pastezips"
                pnlZipSuccess.Visible = False
                pnlZipError.Visible = True
                lblZipError.Text = errorMsg
            Case "customfiles"
                pnlUploadSuccess.Visible = False
                pnlUploadError.Visible = True
                lblUploadError.Text = errorMsg
        End Select


    End Sub


    Protected Sub ShowSuccess(successMsg As String, tabName As String)

        Select Case tabName
            Case "myprojects"
                pnlProjError.Visible = False
                pnlProjSuccess.Visible = True
                lblProjSuccess.Text = successMsg
            Case "pastezips"
                pnlZipError.Visible = False
                pnlZipSuccess.Visible = True
                lblZipSuccess.Text = successMsg
            Case "customfiles"
                pnlUploadError.Visible = False
                pnlUploadSuccess.Visible = True
                lblZipSuccess.Text = successMsg
        End Select

    End Sub


End Class
