Imports Microsoft.VisualBasic
Imports System
Imports System.Web
Imports System.Reflection
Imports System.Web.UI
Imports System.Web.UI.Page
Imports System.IO

Namespace appxCMSCore
    Public Class LibraryInfo
        Implements IHttpHandler

        Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
            Get
                Return True
            End Get
        End Property

        Public Sub ProcessRequest(ByVal context As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest
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

                oSB.AppendLine("<table border=""0"" cellspacing=""0"" cellpadding=""5"" style=""margin: 0px; padding: 0px; width: 100%;"">")
                Dim aProps() As PropertyInfo = alProps.ToArray(GetType(PropertyInfo))
                For iProp As Integer = 0 To aProps.Length - 1
                    Dim oProp As PropertyInfo = aProps(iProp)
                    Dim sPropName As String = oProp.Name
                    Dim oCMSINfo As New appX.cmsAttribute

                    Dim oAttr() As Object = oProp.GetCustomAttributes(True)
                    For Each o As Object In oAttr
                        If TypeOf o Is appX.cmsAttribute Then
                            oCMSINfo = DirectCast(o, appX.cmsAttribute)
                            Exit For
                        End If
                    Next

                    oSB.AppendLine("<tr><td style=""width:1%;text-align:right;font-weight:bold;white-space:nowrap;"">" & apphelp.CamelCaseToTitle(sPropName) & ":</td>")
                    oSB.AppendLine("<td style=""width:100%;"">")

                    Select Case oCMSINfo.MyValueType
                        Case appX.cmsAttribute.DataValueType.CMSImage
                            '-- Single CMS Image

                        Case appX.cmsAttribute.DataValueType.CMSImageList
                            '-- Multiple CMS Images


                        Case appX.cmsAttribute.DataValueType.CSVideo
                            '-- Single Community Server Video
                            oSB.AppendLine("<input type=""hidden"" class=""prop"" id=""" & sPropName & """ rel=""CSVideo"" />")
                            oSB.AppendLine("<img id=""previmg_" & sPropName & """ style=""display:none;"" /> <div title=""Browse Community Server Videos"" id=""browsButton_" & sPropName & """ style=""font-size:7pt;padding-top:4px;width:48px;height:15px;width:expression('50px');height:expression('21px');background-image: url('../../editor/ed_styles/ed_ab_button.gif');text-align:center;font-weight:bold;"" class=""button"">Browse</div>")

                        Case appX.cmsAttribute.DataValueType.CSVideoList
                            '-- Multiple Community Server Videos


                        Case appX.cmsAttribute.DataValueType.Free
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
                            oSB.AppendLine("<textarea class=""prop"" id=""" & sPropName & """ rows=""3"" style=""width:90%;"" />")

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

            context.Response.ContentType = "text/html"
            context.Response.Write(oSB.ToString)
        End Sub
    End Class
End Namespace
