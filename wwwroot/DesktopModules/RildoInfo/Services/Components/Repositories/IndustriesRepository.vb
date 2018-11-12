
Public Class IndustriesRepository
    Implements IIndustriesRepository

    Public Function AddIndustry(industry As Models.Industry) As Models.Industry Implements IIndustriesRepository.AddIndustry
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Industry) = ctx.GetRepository(Of Models.Industry)()
            rep.Insert(industry)
        End Using
        Return industry
    End Function

    Public Function GetIndustries(portalId As Integer) As IEnumerable(Of Models.Industry) Implements IIndustriesRepository.GetIndustries
        Dim t As IEnumerable(Of Models.Industry)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Industry) = ctx.GetRepository(Of Models.Industry)()
            t = rep.Get(portalId)
        End Using
        Return t
    End Function

    Public Function GetIndustry(accountId As Integer, portalId As Integer) As Models.Industry Implements IIndustriesRepository.GetIndustry
        Dim industry As Models.Industry

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Industry) = ctx.GetRepository(Of Models.Industry)()
            industry = rep.GetById(Of Int32, Int32)(accountId, portalId)
        End Using
        Return industry
    End Function

    Public Sub RemoveIndustry(industryId As Integer, portalId As Integer) Implements IIndustriesRepository.RemoveIndustry
        Dim _item As Models.Industry = GetIndustry(industryId, portalId)
        RemoveIndustry(_item)
    End Sub

    Public Sub RemoveIndustry(industry As Models.Industry) Implements IIndustriesRepository.RemoveIndustry
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Industry) = ctx.GetRepository(Of Models.Industry)()
            rep.Delete(industry)
        End Using
    End Sub

    Public Sub UpdateIndustry(industry As Models.Industry) Implements IIndustriesRepository.UpdateIndustry
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Industry) = ctx.GetRepository(Of Models.Industry)()
            rep.Update(industry)
        End Using
    End Sub

End Class
