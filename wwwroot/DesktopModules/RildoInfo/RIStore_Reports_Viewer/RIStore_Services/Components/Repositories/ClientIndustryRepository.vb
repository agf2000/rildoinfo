
Namespace RI.Modules.RIStore_Services
    Public Class ClientIndustryRepository
        Implements IClientIndustriesRepository

        Public Function AddClientIndustry(t As Models.ClientIndustry) As Models.ClientIndustry Implements IClientIndustriesRepository.AddClientIndustry
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientIndustry) = ctx.GetRepository(Of Models.ClientIndustry)()
                rep.Insert(t)
            End Using
            Return t
        End Function

        Public Function GetClientIndustries(clientId As Integer) As IEnumerable(Of Models.Industry) Implements IClientIndustriesRepository.GetClientIndustries
            Dim t As IEnumerable(Of Models.Industry)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Industry) = ctx.GetRepository(Of Models.Industry)()
                t = rep.Get(clientId)
            End Using
            Return t
        End Function

        Public Sub RemoveClientIndustries(clientId As Integer) Implements IClientIndustriesRepository.RemoveClientIndustries
            Dim clientIndustries As IEnumerable(Of Models.ClientIndustry)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of Models.ClientIndustry)()
                clientIndustries = rep.Find("Where ClientId = @0", clientId)

                For Each industry In clientIndustries
                    RemoveClientIndustry(industry)
                Next
            End Using
        End Sub

        Public Sub RemoveClientIndustry(t As Models.ClientIndustry) Implements IClientIndustriesRepository.RemoveClientIndustry
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientIndustry) = ctx.GetRepository(Of Models.ClientIndustry)()
                rep.Delete(t)
            End Using
        End Sub

    End Class
End Namespace