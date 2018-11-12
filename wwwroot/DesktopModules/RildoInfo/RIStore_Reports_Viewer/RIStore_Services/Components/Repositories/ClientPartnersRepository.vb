
Imports System.Collections.Generic
Imports DotNetNuke.Data

Namespace RI.Modules.RIStore_Services
    Public Class ClientPartnersRepository
        Implements IClientPartnersRepository

        Public Function AddClientPartner(clientPartner As Models.ClientPartner) As Models.ClientPartner Implements IClientPartnersRepository.AddClientPartner
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientPartner) = ctx.GetRepository(Of Models.ClientPartner)()
                rep.Insert(clientPartner)
            End Using
            Return clientPartner
        End Function

        Public Function GetClientPartner(clientPartnerId As Integer, clientId As Integer) As Models.ClientPartner Implements IClientPartnersRepository.GetClientPartner
            Dim clientPartner As Models.ClientPartner

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientPartner) = ctx.GetRepository(Of Models.ClientPartner)()
                clientPartner = rep.GetById(Of Int32, Int32)(clientPartnerId, clientId)
            End Using
            Return clientPartner
        End Function

        Public Function GetClientPartners(clientId As Integer) As IEnumerable(Of Models.ClientPartner) Implements IClientPartnersRepository.GetClientPartners
            Dim clientPartners As IEnumerable(Of Models.ClientPartner)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientPartner) = ctx.GetRepository(Of Models.ClientPartner)()
                clientPartners = rep.Get(clientId)
            End Using
            Return clientPartners
        End Function

        Public Sub RemoveClientPartner(clientPartnerId As Integer, clientId As Integer) Implements IClientPartnersRepository.RemoveClientPartner
            Dim _item As Models.ClientPartner = GetClientPartner(clientPartnerId, clientId)
            If _item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveClientPartner(_item)
            End If
        End Sub

        Public Sub RemoveClientPartner(clientPartner As Models.ClientPartner) Implements IClientPartnersRepository.RemoveClientPartner
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientPartner) = ctx.GetRepository(Of Models.ClientPartner)()
                rep.Delete(clientPartner)
            End Using
        End Sub

        Public Sub RemoveClientPartners(clientId As Integer) Implements IClientPartnersRepository.RemoveClientPartners
            Dim clientPartners As IEnumerable(Of Models.ClientPartner)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of Models.ClientPartner)()
                clientPartners = rep.Find("Where ClientId = @0", clientId)

                For Each incomeSource In clientPartners
                    RemoveClientPartner(incomeSource)
                Next
            End Using
        End Sub

        Public Sub UpdateClientPartner(clientPartner As Models.ClientPartner) Implements IClientPartnersRepository.UpdateClientPartner
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientPartner) = ctx.GetRepository(Of Models.ClientPartner)()
                rep.Update(clientPartner)
            End Using
        End Sub

    End Class
End Namespace