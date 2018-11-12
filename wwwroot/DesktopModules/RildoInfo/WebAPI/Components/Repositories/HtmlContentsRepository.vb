Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Repositories

    Public Class HtmlContentsRepository
    Implements IHtmlContentsRepository

        Public Function AddHtmlContent(content As HtmlContent) As Object Implements IHtmlContentsRepository.AddHtmlContent
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of HtmlContent) = ctx.GetRepository(Of HtmlContent)()
                rep.Insert(content)
            End Using
            Return content
        End Function

        Public Function GetHtmlContent(portalId As Integer, contentId As Integer) As HtmlContent Implements IHtmlContentsRepository.GetHtmlContent
            Dim content As HtmlContent

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of HtmlContent) = ctx.GetRepository(Of HtmlContent)()
                content = rep.GetById(Of Int32, Int32)(contentId, portalId)
            End Using
            Return content
        End Function

        Public Function GetHtmlContentList(portalId As Integer) As IEnumerable(Of HtmlContent) Implements IHtmlContentsRepository.GetHtmlContentList
            Dim contentList As IEnumerable(Of HtmlContent)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of HtmlContent) = ctx.GetRepository(Of HtmlContent)()
                contentList = rep.Get(portalId)
            End Using
            Return contentList
        End Function

        Public Sub RemoveHtmlContent(contentId As Integer, portalId As Integer) Implements IHtmlContentsRepository.RemoveHtmlContent
            Dim content As HtmlContent = GetHtmlContent(contentId, portalId)
            RemoveHtmlContent(content)
        End Sub

        Public Sub RemoveHtmlContent(content As HtmlContent) Implements IHtmlContentsRepository.RemoveHtmlContent
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of HtmlContent) = ctx.GetRepository(Of HtmlContent)()
                rep.Delete(content)
            End Using
        End Sub

        Public Sub UpdateHtmlContent(content As HtmlContent) Implements IHtmlContentsRepository.UpdateHtmlContent
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of HtmlContent) = ctx.GetRepository(Of HtmlContent)()
                rep.Update(content)
            End Using
        End Sub

    End Class

End Namespace
