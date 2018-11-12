
Public Class PersonIndustryRepository
    Implements IPersonIndustriesRepository

    Public Function addPersonIndustry(personIndustry As Models.PersonIndustry) As Models.PersonIndustry Implements IPersonIndustriesRepository.addPersonIndustry
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonIndustry) = ctx.GetRepository(Of Models.PersonIndustry)()
            rep.Insert(personIndustry)
        End Using
        Return personIndustry
    End Function

    Public Function getPersonIndustries(personId As Integer) As IEnumerable(Of Models.Industry) Implements IPersonIndustriesRepository.getPersonIndustries
        Dim personIndustry As IEnumerable(Of Models.Industry)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Industry) = ctx.GetRepository(Of Models.Industry)()
            personIndustry = rep.Get(personId)
        End Using
        Return personIndustry
    End Function

    Public Sub removePersonIndustries(personId As Integer) Implements IPersonIndustriesRepository.removePersonIndustries
        Dim personIndustries As IEnumerable(Of Models.PersonIndustry)
        Using context As IDataContext = DataContext.Instance()
            Dim rep = context.GetRepository(Of Models.PersonIndustry)()
            personIndustries = rep.Find("Where PersonId = @0", personId)

            For Each industry In personIndustries
                RemovePersonIndustry(industry)
            Next
        End Using
    End Sub

    Public Sub RemovePersonIndustry(personIndustry As Models.PersonIndustry) Implements IPersonIndustriesRepository.removePersonIndustry
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonIndustry) = ctx.GetRepository(Of Models.PersonIndustry)()
            rep.Delete(personIndustry)
        End Using
    End Sub

End Class
