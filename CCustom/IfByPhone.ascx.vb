
Partial Class CCustom_IfByPhone
    Inherits CLibraryBase

    Private _PhoneNumber As String = "" '3236
    Public Property PhoneNumber As String
        Get
            Return _PhoneNumber
        End Get
        Set(value As String)
            _PhoneNumber = value
        End Set
    End Property

    Private _ImageUrl As String = ""
    Public Property ImageUrl As String
        Get
            Return _ImageUrl
        End Get
        Set(value As String)
            _ImageUrl = value
        End Set
    End Property

    Private _stk As String = ""
    Public Property Stk As String
        Get
            Return _stk
        End Get
        Set(value As String)
            _stk = value
        End Set
    End Property



    Protected Overrides Sub BuildControl()

        If Not String.IsNullOrEmpty(Me.ImageUrl) Then
            pPhone.BackImageUrl = Me.ImageUrl
        End If

        lPhone.Text = "<span class=""fa fa-phone""></span>&nbsp;Call Us! " & PhoneNumber

        Dim oJs As New StringBuilder
        oJs.AppendLine("var _stk = '" & Me.Stk & "';")
        oJs.AppendLine("(function(){")
        oJs.AppendLine("	var a=document, b=a.createElement(""script""); b.type=""text/javascript"";")
        oJs.AppendLine("	b.async=!0; b.src= 'https:' == document.location.protocol ? 'https://' :")
        oJs.AppendLine("	'http://' + 'd31y97ze264gaa.cloudfront.net/assets/st/js/st.js';")
        oJs.AppendLine("	a=a.getElementsByTagName(""script"")[0]; a.parentNode.insertBefore(b,a);")
        oJs.AppendLine("})();")
        Page.ClientScript.RegisterStartupScript(GetType(String), "IfByPhoneNumberReplacement", oJs.ToString(), True)

        'Page.ClientScript.RegisterClientScriptInclude("IfByPhoneClickToReferral", "https://secure.ifbyphone.com/js/ibp_clickto_referral.js")
        'Dim oJs As New StringBuilder
        'oJs.AppendLine("    var _ibp_public_key = ""3c63dad39f58b07ea3105c0db6cc3b13f24ebee5"";")
        'oJs.AppendLine("    var _ibp_formatting = true;")
        'oJs.AppendLine("    var _ibp_keyword_set = 76274;")
        'Page.ClientScript.RegisterClientScriptBlock(GetType(String), "IfByPhoneInit", oJs.ToString, True)

        'lPhone.Text = "<script type=""text/JavaScript"" src=""https://secure.ifbyphone.com/js/keyword_replacement.js""></script>"
    End Sub
    

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

End Class
