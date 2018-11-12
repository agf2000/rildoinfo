Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IHtmlContentsRepository

        Function GetHtmlContentList(portalId As Integer) As IEnumerable(Of HtmlContent)

        Function GetHtmlContent(portalId As Integer, contentId As Integer) As HtmlContent

        Function AddHtmlContent(content As HtmlContent)

        Sub UpdateHtmlContent(content As HtmlContent)

        Sub RemoveHtmlContent(contentId As Integer, portalId As Integer)

        Sub RemoveHtmlContent(content As HtmlContent)

    End Interface

End Namespace
