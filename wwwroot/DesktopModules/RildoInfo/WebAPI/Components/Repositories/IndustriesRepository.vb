Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Repositories

    Public Class IndustriesRepository
    Implements IIndustriesRepository

        Public Function AddIndustry(industry As Industry) As Industry Implements IIndustriesRepository.AddIndustry
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Industry) = ctx.GetRepository(Of Industry)()
                rep.Insert(industry)
            End Using
            Return industry
        End Function

        Public Function GetIndustries(portalId As Integer, isDeleted As String) As IEnumerable(Of Industry) Implements IIndustriesRepository.GetIndustries
            Dim industries As IEnumerable(Of Industry)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Industry) = ctx.GetRepository(Of Industry)()
                industries = rep.Find("Where ((PortalId = @0) AND ((@1 = '') OR (IsDeleted = @1)))", portalId, isDeleted)
            End Using
            Return industries
        End Function

        Public Function GetIndustry(accountId As Integer, portalId As Integer) As Industry Implements IIndustriesRepository.GetIndustry
            Dim industry As Industry

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Industry) = ctx.GetRepository(Of Industry)()
                industry = rep.GetById(Of Int32, Int32)(accountId, portalId)
            End Using
            Return industry
        End Function

        Public Sub RemoveIndustry(industryId As Integer, portalId As Integer) Implements IIndustriesRepository.RemoveIndustry
            Dim item As Industry = GetIndustry(industryId, portalId)
            RemoveIndustry(item)
        End Sub

        Public Sub RemoveIndustry(industry As Industry) Implements IIndustriesRepository.RemoveIndustry
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Industry) = ctx.GetRepository(Of Industry)()
                rep.Delete(industry)
            End Using
        End Sub

        Public Sub UpdateIndustry(industry As Industry) Implements IIndustriesRepository.UpdateIndustry
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Industry) = ctx.GetRepository(Of Industry)()
                rep.Update(industry)
            End Using
        End Sub

    End Class

End Namespace
