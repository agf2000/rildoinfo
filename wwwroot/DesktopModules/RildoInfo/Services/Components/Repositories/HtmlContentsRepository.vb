Public Class HtmlContentsRepository
    Implements IHtmlContentsRepository

    Public Function addHtmlContent(content As Models.HtmlContent) As Object Implements IHtmlContentsRepository.addHtmlContent
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.HtmlContent) = ctx.GetRepository(Of Models.HtmlContent)()
            rep.Insert(content)
        End Using
        Return content
    End Function

    Public Function getHtmlContent(portalId As Integer, contentId As Integer) As Models.HtmlContent Implements IHtmlContentsRepository.getHtmlContent
        Dim content As Models.HtmlContent

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.HtmlContent) = ctx.GetRepository(Of Models.HtmlContent)()
            content = rep.GetById(Of Int32, Int32)(contentId, portalId)
        End Using
        Return content
    End Function

    Public Function getHtmlContentList(portalId As Integer) As IEnumerable(Of Models.HtmlContent) Implements IHtmlContentsRepository.getHtmlContentList
        Dim contentList As IEnumerable(Of Models.HtmlContent)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.HtmlContent) = ctx.GetRepository(Of Models.HtmlContent)()
            contentList = rep.Get(portalId)
        End Using
        Return contentList
    End Function

    Public Sub removeHtmlContent(contentId As Integer, portalId As Integer) Implements IHtmlContentsRepository.removeHtmlContent
        Dim content As Models.HtmlContent = getHtmlContent(contentId, portalId)
        removeHtmlContent(content)
    End Sub

    Public Sub removeHtmlContent(content As Models.HtmlContent) Implements IHtmlContentsRepository.removeHtmlContent
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.HtmlContent) = ctx.GetRepository(Of Models.HtmlContent)()
            rep.Delete(content)
        End Using
    End Sub

    Public Sub updateHtmlContent(content As Models.HtmlContent) Implements IHtmlContentsRepository.updateHtmlContent
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.HtmlContent) = ctx.GetRepository(Of Models.HtmlContent)()
            rep.Update(content)
        End Using
    End Sub

End Class
