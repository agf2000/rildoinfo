Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Repositories

    Public Class PersonIndustryRepository
    Implements IPersonIndustriesRepository

        Public Function AddPersonIndustry(personIndustry As PersonIndustry) As PersonIndustry Implements IPersonIndustriesRepository.AddPersonIndustry
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonIndustry) = ctx.GetRepository(Of PersonIndustry)()
                rep.Insert(personIndustry)
            End Using
            Return personIndustry
        End Function

        Public Function GetPersonIndustries(personId As Integer) As IEnumerable(Of Industry) Implements IPersonIndustriesRepository.GetPersonIndustries
            Dim personIndustry As IEnumerable(Of Industry)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Industry) = ctx.GetRepository(Of Industry)()
                personIndustry = rep.Get(personId)
            End Using
            Return personIndustry
        End Function

        Public Sub RemovePersonIndustries(personId As Integer) Implements IPersonIndustriesRepository.RemovePersonIndustries
            Dim personIndustries As IEnumerable(Of PersonIndustry)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of PersonIndustry)()
                personIndustries = rep.Find("Where PersonId = @0", personId)

                For Each industry In personIndustries
                    RemovePersonIndustry(industry)
                Next
            End Using
        End Sub

        Public Sub RemovePersonIndustry(personIndustry As PersonIndustry) Implements IPersonIndustriesRepository.RemovePersonIndustry
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonIndustry) = ctx.GetRepository(Of PersonIndustry)()
                rep.Delete(personIndustry)
            End Using
        End Sub

    End Class

End Namespace
