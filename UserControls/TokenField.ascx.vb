Imports System.Linq
Imports Microsoft.ReportingServices.ReportProcessing.Persistence

Partial Class UserControls_TokenField
    Inherits System.Web.UI.UserControl

    Private _defaultValues As String = ""
    Public Property DefaultValues As String
        Get
            '-- Not implemented
        End Get
        Set(value As String)
            TokenField.Text = value
        End Set
    End Property

    Private _localLookupList As String = ""
    Public Property LocalLookupList As String
        Get
            Return _localLookupList
        End Get
        Set(value As String)
            _localLookupList = value
        End Set
    End Property

    Private _valueListSeparator As String = "|"
    Public Property ValueListSeparator As String
        Get
            Return _valueListSeparator
        End Get
        Set(value As String)
            _valueListSeparator = value
        End Set
    End Property

    Public ReadOnly Property ValueList As String
        Get
            Return hfTokenFieldValues.Value
        End Get
    End Property

    Private _allowDuplicates As Boolean = False
    Public Property AllowDuplicates As Boolean
        Get
            Return _allowDuplicates
        End Get
        Set(value As Boolean)
            _allowDuplicates = value
        End Set
    End Property

    Private _tokenLimit As Integer = 0
    Public Property TokenLimit As Integer
        Get
            Return _tokenLimit
        End Get
        Set(value As Integer)
            _tokenLimit = value
        End Set
    End Property

    Private _tokenMinLength As Integer = 0
    Public Property TokenMinLength As Integer
        Get
            Return _tokenMinLength
        End Get
        Set(value As Integer)
            _tokenMinLength = value
        End Set
    End Property

    Private _tokenMaxLength As Integer = 0
    Public Property TokenMaxLength As Integer
        Get
            Return _tokenMaxLength
        End Get
        Set(value As Integer)
            _tokenMaxLength = value
        End Set
    End Property

    Private _showAutoCompleteOnFocus As Boolean = False
    Public Property ShowAutoCompleteOnFocus As Boolean
        Get
            Return _showAutoCompleteOnFocus
        End Get
        Set(value As Boolean)
            _showAutoCompleteOnFocus = value
        End Set
    End Property

    Private _createTokensOnBlur As Boolean = False
    Public Property CreateTokensOnBlur As Boolean
        Get
            Return _createTokensOnBlur
        End Get
        Set(value As Boolean)
            _createTokensOnBlur = value
        End Set
    End Property

    Private _restrictInputToList As Boolean = False
    Public Property RestrictInputToList As Boolean
        Get
            Return _restrictInputToList
        End Get
        Set(value As Boolean)
            _restrictInputToList = value
        End Set
    End Property
    

    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        appxCMS.Util.jQuery.IncludePlugin(Page, "TokenFieldPlugin", "~/scripts/tokenfield/bootstrap-tokenfield.js")
        appxCMS.Util.jQuery.RegisterStylesheet(Page, "~/scripts/tokenfield/bootstrap-tokenfield.css")
        appxCMS.Util.jQuery.RegisterStylesheet(Page, "~/scripts/tokenfield/tokenfield-typeahead.css")
        Dim oJs As New StringBuilder
        TokenField.Attributes.Add("data-allowDuplicates", AllowDuplicates.ToString().ToLowerInvariant())
        TokenField.Attributes.Add("data-limit", TokenLimit.ToString())
        TokenField.Attributes.Add("data-minLength", TokenMinLength.ToString())
        TokenField.Attributes.Add("data-createTokensOnBlur", CreateTokensOnBlur.ToString().ToLowerInvariant())

        oJs.AppendLine("jQuery(document).ready(function($) {")
        oJs.AppendLine("    $('#" & TokenField.ClientID & "').tokenfield({")
        If Not String.IsNullOrEmpty(LocalLookupList) Then
            oJs.AppendLine("        autocomplete: {")
            oJs.AppendLine("            source: ['" & LocalLookupList.Replace("'", "''").Replace(",", "','") & "'],")
            oJs.AppendLine("            delay: 100")
            oJs.AppendLine("        },")
            oJs.AppendLine("        showAutocompleteOnFocus: " & ShowAutoCompleteOnFocus.ToString().ToLowerInvariant())
        End If
        oJs.AppendLine("    });")
        oJs.AppendLine("    $('#" & TokenField.ClientID & "').on('afterCreateToken', function(e) {")
        oJs.AppendLine("        var sVal = $('#" & TokenField.ClientID & "').tokenfield('getTokensList', '" & ValueListSeparator & "');")
        oJs.AppendLine("        $('#" & hfTokenFieldValues.ClientID & "').val(sVal);")
        oJs.AppendLine("    });")
        If RestrictInputToList Then
            oJs.AppendLine("    $('#" & TokenField.ClientID & "').on('beforeCreateToken', function(e) {")
            oJs.AppendLine("        console.log(e);")
            'oJs.AppendLine("            $('#" & hfTokenFieldValues.ClientID & "').val(('#" & TokenField.ClientID & "').tokenfield('getTokensList', '" & ValueListSeparator & "'));")
            oJs.AppendLine("    });")
        End If
        '-- Initialize our token selections
        oJs.AppendLine("    var sDefVal = $('#" & TokenField.ClientID & "').tokenfield('getTokensList', '" & ValueListSeparator & "');")
        oJs.AppendLine("    $('#" & hfTokenFieldValues.ClientID & "').val(sDefVal);")
        oJs.AppendLine("});")
        appxCMS.Util.jQuery.RegisterClientScript(Page, Me.ClientID & "TokenFieldInit", oJs.ToString())
    End Sub
End Class
