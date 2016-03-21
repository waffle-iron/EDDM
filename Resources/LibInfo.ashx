<%@ WebHandler Language="VB" Class="LibInfo" %>

Imports System
Imports System.Web
Imports System.Reflection
Imports System.Web.UI
Imports System.Web.UI.Page
Imports System.IO

Public Class LibInfo : Implements IHttpHandler
    
    Private ProtectedDirectoryBase As String = ""
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim sControl As String = ""
        If context.Request.QueryString("cc") IsNot Nothing Then
            sControl = context.Request.QueryString("cc")
        End If
        If Not sControl.StartsWith("/") Then
            sControl = "/" & sControl
        End If
                
        Dim oSB As New StringBuilder()
        If File.Exists(context.Server.MapPath(sControl)) Then
            Dim oType As Type
            Dim oPg As New Page
            Dim oControl As Control = oPg.LoadControl(sControl)
            oType = oControl.GetType
            Dim aAllProps As PropertyInfo() = oType.GetProperties()
            Dim alProps As New ArrayList

            Dim sType As String = oType.Name.ToLower
            If sType.EndsWith("_ascx") Then
                sType = sType.Replace("_ascx", "")
            End If

            For iAll As Integer = 0 To aAllProps.Length - 1
                Dim oProp As PropertyInfo = aAllProps(iAll)
                Dim oMethodInfo As MethodInfo = oProp.GetSetMethod(False)
                If oMethodInfo IsNot Nothing Then
                    Dim sProp As String = oProp.DeclaringType.ToString.ToLower
                    If sProp.Contains("_") Then
                        sProp = sProp.Substring(sProp.IndexOf("_"))
                    End If
                    If sType.EndsWith(sProp) Then
                        alProps.Add(oProp)
                    End If
                End If
                
                'If oProp.DeclaringType.ToString.ToLower = oType.Name.ToLower.Replace("_ascx", "") Then
                '    alProps.Add(oProp)
                'End If
            Next
            
            oSB.AppendLine("<script type=""text/javascript"" src=""" & VirtualPathUtility.ToAbsolute(ConfigurationManager.AppSettings("jQuery-Min")) & """></script>")
            oSB.AppendLine("<table border=""0"" cellspacing=""0"" cellpadding=""5"" style=""margin: 0px; padding: 0px; width: 100%;"">")
            Dim aProps() As PropertyInfo = alProps.ToArray(GetType(PropertyInfo))
            For iProp As Integer = 0 To aProps.Length - 1
                Dim oProp As PropertyInfo = aProps(iProp)
                Dim sPropName As String = oProp.Name
                Dim oCMSINfo As New appx.cmsAttribute
                
                Dim oAttr() As Object = oProp.GetCustomAttributes(True)
                For Each o As Object In oAttr
                    If TypeOf o Is appx.cmsAttribute Then
                        oCMSINfo = DirectCast(o, appx.cmsAttribute)
                        Exit For
                    End If
                Next

                oSB.AppendLine("<tr><td style=""width:1%;text-align:right;font-weight:bold;white-space:nowrap;"" valign=""top"">" & apphelp.CamelCaseToTitle(sPropName) & ":</td>")
                oSB.AppendLine("<td style=""width:100%;"" valign=""top"">")
                
                Select Case oCMSINfo.MyValueType
                    Case appx.cmsAttribute.DataValueType.CMSImage
                        '-- Single CMS Image
                        ' Temporary, until image selector is completed
                        oSB.AppendLine("<input type=""text"" class=""prop"" id=""" & sPropName & """ />")
                        
                    Case appx.cmsAttribute.DataValueType.CMSImageList
                        '-- Multiple CMS Images
                        oSB.AppendLine("<textarea class=""prop"" id=""" & sPropName & """ rows=""3"" style=""width:80%;""></textarea>")
                        
                    Case appx.cmsAttribute.DataValueType.CMSDirectory
                        '-- Single CMS Directory
                        oSB.AppendLine("<input type=""text"" class=""prop"" id=""" & sPropName & """ rel=""CMSDirectory"" />")
                        'oSB.AppendLine("<div title=""Browse to Directory"" id=""browseButton_" & sPropName & """ style=""font-size:7pt;padding-top:4px;width:48px;height:15px;width:express('50px')height:express('21px');text-align:center;font-weight:bold;"" class=""button"">Browse</div>")                        
                        
                    Case appx.cmsAttribute.DataValueType.CSVideo
                        '-- Single Community Server Video
                        oSB.AppendLine("<input type=""hidden"" class=""prop"" id=""" & sPropName & """ rel=""CSVideo"" />")
                        oSB.AppendLine("<img id=""previmg_" & sPropName & """ style=""display:none;"" /> <div title=""Browse Community Server Videos"" id=""browsButton_" & sPropName & """ style=""font-size:7pt;padding-top:4px;width:48px;height:15px;width:expression('50px');height:expression('21px');background-image: url('../../editor/ed_styles/ed_ab_button.gif');text-align:center;font-weight:bold;"" class=""button"">Browse</div>")
                        
                    Case appx.cmsAttribute.DataValueType.CSVideoList
                        '-- Multiple Community Server Videos
                        
                    Case appx.cmsAttribute.DataValueType.CMSSurvey
                        '-- Single Survey 
                        oSB.AppendLine("<select class=""prop"" id=""" & sPropName & """>")
                        oSB.AppendLine("<option value="""">Select One</option>")
                        Using oSurveyA As New appxSurveyTableAdapters.SurveyHeaderTableAdapter
                            Using oSurveyT As appxSurvey.SurveyHeaderDataTable = oSurveyA.GetData()
                                For Each oSurvey As appxSurvey.SurveyHeaderRow In oSurveyT.Rows
                                    oSB.AppendLine("<option value=""" & oSurvey.SurveyID & """>" & oSurvey.SurveyName & "</option>")
                                Next
                            End Using
                        End Using
                        oSB.AppendLine("</select>")
                        
                    Case appx.cmsAttribute.DataValueType.CMSProtectedDirectory
                        ProtectedDirectoryBase = context.Server.MapPath("/app_data/ProtectedFiles")
                        '-- Build list of available protected directories
                        oSB.AppendLine("<input type=""text"" class=""prop"" id=""" & sPropName & """ style=""width:80%;"" />")
                        oSB.AppendLine("<div>Or select an existing directory:<br/><select class=""prop"" style=""width:80%;"" id=""" & sPropName & "Select"" onchange=""document.getElementById('" & sPropName & "').value=this.value;"">")
                        oSB.AppendLine("<option value="""">Choose Path</option>")
                        Dim oPDir As New DirectoryInfo(ProtectedDirectoryBase)
                        '-- Get subs
                        PathToList(oPDir, oSB)
                        oSB.AppendLine("</select></div>")
                        
                    Case appx.cmsAttribute.DataValueType.CMSUserFunctionList
                        '-- Build a checkbox list of options
                        oSB.AppendLine("<input type=""text"" class=""prop userfunctionlist"" id=""" & sPropName & """ style=""width:80%;"" />")
                        Dim oRoles As System.Collections.Generic.List(Of appxCMS.Role) = appxCMS.RoleDataSource.GetUserFunctions
                        oSB.AppendLine("<div>")
                        For Each oRole As appxCMS.Role In oRoles
                            oSB.AppendLine("<span style=""white-space:nowrap;""><input type=""checkbox"" class=""userfunctionchk"" value=""" & oRole.RoleCat & "." & oRole.RoleName & """ id=""" & sPropName & "_" & oRole.RoleID & """ onclick=""var sVal=this.value;var oTarget=document.getElementById('" & sPropName & "'); var sCurStr=oTarget.value;"" /><label for=""" & sPropName & "_" & oRole.RoleID & """>" & oRole.RoleName & "</label></span>")
                        Next
                        oSB.AppendLine("</div>")
                        
                    Case appx.cmsAttribute.DataValueType.Free
                        If oProp.PropertyType.IsEnum Then
                            ' build a drop-down list of the values 
                            oSB.AppendLine("<select class=""prop"" id=""" & sPropName & """>")
                            Dim aVals() As String = [Enum].GetNames(oProp.PropertyType)
                            For iVal As Integer = 0 To aVals.Length - 1
                                oSB.AppendLine("<option value=""" & aVals(iVal) & """>" & aVals(iVal) & "</option>")
                            Next
                            oSB.AppendLine("</select>")
                        Else
                            Dim oPropType As System.Type = oProp.PropertyType
                            If oPropType Is GetType(System.Boolean) Then
                                oSB.AppendLine("<select class=""prop"" id=""" & sPropName & """>")
                                oSB.AppendLine("<option value=""False"">No</option>")
                                oSB.AppendLine("<option value=""True"">Yes</option>")
                                oSB.AppendLine("</select>")
                            Else
                                ' a text input should do well here 
                                oSB.AppendLine("<input type=""text"" class=""prop"" id=""" & sPropName & """ />")
                            End If
                        End If
                        
                    Case appx.cmsAttribute.DataValueType.URLList
                        oSB.AppendLine("<textarea class=""prop"" id=""" & sPropName & """ rows=""3"" style=""width:90%;""></textarea>")
                    
                    Case Else
                        Dim oPropType As System.Type = oProp.PropertyType
                        If oPropType Is GetType(System.Boolean) Then
                            oSB.AppendLine("<select class=""prop"" id=""" & sPropName & """>")
                            oSB.AppendLine("<option value=""False"">No</option>")
                            oSB.AppendLine("<option value=""True"">Yes</option>")
                            oSB.AppendLine("</select>")
                        Else
                            ' a text input should do well here 
                            oSB.AppendLine("<input type=""text"" class=""prop"" id=""" & sPropName & """ />")
                        End If
                        
                End Select
               
                oSB.AppendLine("</td></tr>")
            Next
            oSB.AppendLine("</table>")
        Else
            oSB.AppendLine("<p>Control not found: " & context.Server.MapPath(sControl) & "</p>")
        End If
        
        oSB.AppendLine("<script type=""text/javascript"">")
        oSB.AppendLine("jQuery(document).ready(function($) {")
        oSB.AppendLine("    $('input.userfunctionchk').click(function() {")
        oSB.AppendLine("        var sSelect = '';")
        oSB.AppendLine("        var oParent = $(this).closest('td');")
        oSB.AppendLine("        var sJoin = '';")
        oSB.AppendLine("        $('input.userfunctionchk:checked', oParent).each(function() {")
        oSB.AppendLine("            sSelect += sJoin + $(this).val();")
        oSB.AppendLine("            sJoin = ',';")
        oSB.AppendLine("        });")
        oSB.AppendLine("        $('input.userfunctionlist', oParent).val(sSelect);")
        oSB.AppendLine("    });")
        oSB.AppendLine("});")
        oSB.AppendLine("</script>")
        context.Response.ContentType = "text/html"
        context.Response.Write(oSB.ToString)
    End Sub
    
    Private Sub PathToList(ByVal oDir As DirectoryInfo, ByVal oSb As StringBuilder)
        Dim oSubs() As DirectoryInfo = oDir.GetDirectories
        For Each oSub As DirectoryInfo In oSubs
            Dim sDirName As String = oSub.FullName.Replace(ProtectedDirectoryBase, "").Replace("\", "/")
            If sDirName.StartsWith("/") Then
                sDirName = sDirName.Substring(1)
            End If
            oSb.AppendLine("<option value=""" & sDirName & """>" & sDirName & "</option>")
            PathToList(oSub, oSb)
        Next
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class