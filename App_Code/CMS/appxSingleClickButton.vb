Namespace appx.Web
    <ToolboxData("<{0}:SingleClickButton runat=""server""></{0}:SingleClickButton>")> _
    Public Class SingleClickButton
        Inherits System.Web.UI.WebControls.Button

        Public Property ClientScriptKey() As String
            Get
                Return _ClientScriptKey
            End Get
            Set(ByVal value As String)
                _ClientScriptKey = value
            End Set
        End Property
        Private _ClientScriptKey As String = "appxSingleClickButton"

        Public Property ClickedText() As String
            Get
                Return _ClickedText
            End Get
            Set(ByVal value As String)
                _ClickedText = value
            End Set
        End Property
        Private _ClickedText As String = ""

        Private _ShowProcessingModal As Boolean = False
        Public Property ShowProcessingModal() As Boolean
            Get
                Return _ShowProcessingModal
            End Get
            Set(ByVal value As Boolean)
                _ShowProcessingModal = value
            End Set
        End Property

        Private _ProcessingModalTitle As String = ""
        Public Property ProcessingModalTitle() As String
            Get
                Return _ProcessingModalTitle
            End Get
            Set(ByVal value As String)
                _ProcessingModalTitle = value
            End Set
        End Property

        Private _ProcessingModalHtml As String = ""
        Public Property ProcessingModalHtml() As String
            Get
                Return _ProcessingModalHtml
            End Get
            Set(ByVal value As String)
                _ProcessingModalHtml = value
            End Set
        End Property

        Public Sub New()
            MyBase.UseSubmitBehavior = False
        End Sub

        Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
            If Page IsNot Nothing Then
                If Not Page.ClientScript.IsClientScriptBlockRegistered(Me.ClientScriptKey) Then
                    '-- Register the javascript handlers for this
                    Dim oJS As New StringBuilder
                    oJS.AppendLine("function InitSingleClickButton(sID, sAltText) {")

                    If Me.ShowProcessingModal Then
                        oJS.AppendLine("    jQuery('form:first').append('<div id=""SingleClickModal"" title=""" & Me.ProcessingModalTitle & """>" & Me.ProcessingModalHtml & "</div>');")
                        oJS.AppendLine("    jQuery('#SingleClickModal').dialog({autoOpen:false,modal:true,width:500});")
                    End If

                    oJS.AppendLine("    var oBtn = document.getElementById(sID);")
                    oJS.AppendLine("    if (oBtn) {")
                    oJS.AppendLine("        oBtn.aspnet_click = oBtn.onclick;")
                    oJS.AppendLine("        if (sAltText != null && sAltText != '') {")
                    oJS.AppendLine("            oBtn.singleclick_alttext = sAltText;")
                    oJS.AppendLine("        }")
                    oJS.AppendLine("        oBtn.onclick = SingleClickButtonHandler;")
                    oJS.AppendLine("    }")
                    oJS.AppendLine("}")
                    oJS.AppendLine("function SingleClickButtonHandler(e) {")
                    oJS.AppendLine("    var src = typeof(event) != 'undefined' ? event.srcElement : e.target;")
                    oJS.AppendLine("    src.disabled = true;")
                    oJS.AppendLine("    src.aspnet_click();")
                    oJS.AppendLine("    src.disabled = typeof(Page_IsValid) != 'undefined' ? Page_IsValid : true;")
                    oJS.AppendLine("    if (src.disabled && src.singleclick_alttext != null) {")
                    oJS.AppendLine("        src.value = src.singleclick_alttext;")
                    If Me.ShowProcessingModal Then
                        oJS.AppendLine("    jQuery('#SingleClickModal').dialog('open');")
                    End If
                    oJS.AppendLine("    }")
                    oJS.AppendLine("}")
                    Page.ClientScript.RegisterClientScriptBlock(GetType(String), Me.ClientScriptKey, oJS.ToString, True)
                End If

                '-- Get a unique key for registering the call to initialize this button
                Dim sButtonKey As String = Me.UniqueID
                If Not Page.ClientScript.IsClientScriptBlockRegistered(Me.ClientScriptKey & "_" & sButtonKey) Then
                    Page.ClientScript.RegisterStartupScript(GetType(String), sButtonKey, "InitSingleClickButton('" & Me.ClientID & "', '" & Me.ClickedText & "');", True)
                End If
            End If
            MyBase.OnPreRender(e)
        End Sub
    End Class
End Namespace
