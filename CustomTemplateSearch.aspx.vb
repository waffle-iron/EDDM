
Partial Class CustomTemplateSearch
    Inherits appxCMS.PageBase

    Protected ReadOnly Property IndustryId As Integer
        Get
            Return appxCMS.Util.Querystring.GetInteger("i")
        End Get
    End Property

    Protected ReadOnly Property SearchTerm As String
        Get
            Return appxCMS.Util.Querystring.GetString("q")
        End Get
    End Property

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Search.IndustryId = Me.IndustryId
        Search.DoRender()

        PagedList.IndustryId = Me.IndustryId
        PagedList.Keywords = SearchTerm
        PagedList.PageCount = 21
        PagedList.DoRender()
    End Sub
End Class
