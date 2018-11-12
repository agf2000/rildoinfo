
Public Interface IHtmlContentsRepository

    Function getHtmlContentList(portalId As Integer) As IEnumerable(Of Models.HtmlContent)

    Function getHtmlContent(portalId As Integer, contentId As Integer) As Models.HtmlContent

    Function addHtmlContent(content As Models.HtmlContent)

    Sub updateHtmlContent(content As Models.HtmlContent)

    Sub removeHtmlContent(contentId As Integer, portalId As Integer)

    Sub removeHtmlContent(content As Models.HtmlContent)

End Interface
