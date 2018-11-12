Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models

Namespace Components.Repositories

    Public Class ClientPartnersRepository
    Implements IClientPartnersRepository

        Public Function AddClientPartner(clientPartner As ClientPartner) As ClientPartner Implements IClientPartnersRepository.AddClientPartner
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientPartner) = ctx.GetRepository(Of ClientPartner)()
                rep.Insert(clientPartner)
            End Using
            Return clientPartner
        End Function

        Public Function GetClientPartner(clientPartnerId As Integer, personId As Integer) As ClientPartner Implements IClientPartnersRepository.GetClientPartner
            Dim clientPartner As ClientPartner

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientPartner) = ctx.GetRepository(Of ClientPartner)()
                clientPartner = rep.GetById(Of Int32, Int32)(clientPartnerId, personId)
            End Using
            Return clientPartner
        End Function

        Public Function GetClientPartners(personId As Integer) As IEnumerable(Of ClientPartner) Implements IClientPartnersRepository.GetClientPartners
            Dim clientPartners As IEnumerable(Of ClientPartner)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientPartner) = ctx.GetRepository(Of ClientPartner)()
                clientPartners = rep.Get(personId)
            End Using
            Return clientPartners
        End Function

        Public Sub RemoveClientPartner(clientPartnerId As Integer, personId As Integer) Implements IClientPartnersRepository.RemoveClientPartner
            Dim item As ClientPartner = GetClientPartner(clientPartnerId, personId)
            If item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveClientPartner(item)
            End If
        End Sub

        Public Sub RemoveClientPartner(clientPartner As ClientPartner) Implements IClientPartnersRepository.RemoveClientPartner
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientPartner) = ctx.GetRepository(Of ClientPartner)()
                rep.Delete(clientPartner)
            End Using
        End Sub

        Public Sub RemoveClientPartners(personId As Integer) Implements IClientPartnersRepository.RemoveClientPartners
            Dim clientPartners As IEnumerable(Of ClientPartner)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of ClientPartner)()
                clientPartners = rep.Find("Where PersonId = @0", personId)

                For Each incomeSource In clientPartners
                    RemoveClientPartner(incomeSource)
                Next
            End Using
        End Sub

        Public Sub UpdateClientPartner(clientPartner As ClientPartner) Implements IClientPartnersRepository.UpdateClientPartner
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientPartner) = ctx.GetRepository(Of ClientPartner)()
                rep.Update(clientPartner)
            End Using
        End Sub

    End Class

End Namespace
