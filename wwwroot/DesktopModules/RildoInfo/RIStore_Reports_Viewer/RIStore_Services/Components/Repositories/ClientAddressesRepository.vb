
Imports System.Collections.Generic
Imports DotNetNuke.Data

Namespace RI.Modules.RIStore_Services
    Public Class ClientAddressesRepository
        Implements IClientAddressesRepository

        Public Function AddClientAddress(clientAddress As Models.ClientAddress) As Models.ClientAddress Implements IClientAddressesRepository.AddClientAddress
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientAddress) = ctx.GetRepository(Of Models.ClientAddress)()
                rep.Insert(clientAddress)
            End Using
            Return clientAddress
        End Function

        Public Function GetClientAddress(clientAddressId As Integer, clientId As Integer) As Models.ClientAddress Implements IClientAddressesRepository.GetClientAddress
            Dim clientAddress As Models.ClientAddress

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientAddress) = ctx.GetRepository(Of Models.ClientAddress)()
                clientAddress = rep.GetById(Of Int32, Int32)(clientAddressId, clientId)
            End Using
            Return clientAddress
        End Function

        Public Function GetClientAddresses(clientId As Integer) As IEnumerable(Of Models.ClientAddress) Implements IClientAddressesRepository.GetClientAddresses
            Dim clientAddress As IEnumerable(Of Models.ClientAddress)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientAddress) = ctx.GetRepository(Of Models.ClientAddress)()
                clientAddress = rep.Get(clientId)
            End Using
            Return clientAddress
        End Function

        Public Sub RemoveClientAddress(clientAddressId As Integer, clientId As Integer) Implements IClientAddressesRepository.RemoveClientAddress
            Dim _item As Models.ClientAddress = GetClientAddress(clientAddressId, clientId)
            If _item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveClientAddress(_item)
            End If
        End Sub

        Public Sub RemoveClientAddress(clientAddress As Models.ClientAddress) Implements IClientAddressesRepository.RemoveClientAddress
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientAddress) = ctx.GetRepository(Of Models.ClientAddress)()
                rep.Delete(clientAddress)
            End Using
        End Sub

        Public Sub UpdateClientAddress(clientAddress As Models.ClientAddress) Implements IClientAddressesRepository.UpdateClientAddress
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientAddress) = ctx.GetRepository(Of Models.ClientAddress)()
                rep.Update(clientAddress)
            End Using
        End Sub

        Public Sub RemoveClientAddresses(clientId As Integer) Implements IClientAddressesRepository.RemoveClientAddresses
            Dim clientAddresses As IEnumerable(Of Models.ClientAddress)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of Models.ClientAddress)()
                clientAddresses = rep.Find("Where ClientId = @0", clientId)

                For Each address In clientAddresses
                    RemoveClientAddress(address)
                Next
            End Using
        End Sub

    End Class
End Namespace