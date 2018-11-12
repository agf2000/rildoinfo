Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Repositories

    Public Class PersonAddressesRepository
    Implements IPersonAddressesRepository

        Public Function AddPersonAddress(personAddress As PersonAddress) As PersonAddress Implements IPersonAddressesRepository.AddPersonAddress
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonAddress) = ctx.GetRepository(Of PersonAddress)()
                rep.Insert(personAddress)
            End Using
            Return personAddress
        End Function

        Public Function GetPersonAddress(personAddressId As Integer, personId As Integer) As PersonAddress Implements IPersonAddressesRepository.GetPersonAddress
            Dim personAddress As PersonAddress

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonAddress) = ctx.GetRepository(Of PersonAddress)()
                personAddress = rep.GetById(Of Int32, Int32)(personAddressId, personId)
            End Using
            Return personAddress
        End Function

        Public Function GetPersonAddresses(personId As Integer) As IEnumerable(Of PersonAddress) Implements IPersonAddressesRepository.GetPersonAddresses
            Dim personAddresses As IEnumerable(Of PersonAddress)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonAddress) = ctx.GetRepository(Of PersonAddress)()
                personAddresses = rep.Get(personId)
            End Using
            Return personAddresses
        End Function

        Public Function GetPersonMainAddress(personId As Integer) As PersonAddress Implements IPersonAddressesRepository.GetPersonMainAddress
            Dim address As PersonAddress

            Using ctx As IDataContext = DataContext.Instance()
                address = ctx.ExecuteSingleOrDefault(Of PersonAddress)(CommandType.Text, "where PersonId = @0 and ViewOrder = 1", personId)
            End Using
            Return address
        End Function

        Public Sub RemovePersonAddress(personAddressId As Integer, personId As Integer) Implements IPersonAddressesRepository.RemovePersonAddress
            Dim item As PersonAddress = GetPersonAddress(personAddressId, personId)
            If item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemovePersonAddress(item)
            End If
        End Sub

        Public Sub RemovePersonAddress(personAddress As PersonAddress) Implements IPersonAddressesRepository.RemovePersonAddress
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonAddress) = ctx.GetRepository(Of PersonAddress)()
                rep.Delete(personAddress)
            End Using
        End Sub

        Public Sub UpdatePersonAddress(personAddress As PersonAddress) Implements IPersonAddressesRepository.UpdatePersonAddress
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonAddress) = ctx.GetRepository(Of PersonAddress)()
                rep.Update(personAddress)
            End Using
        End Sub

        Public Sub RemovePersonAddresses(personId As Integer) Implements IPersonAddressesRepository.RemovePersonAddresses
            Dim personAddresses As IEnumerable(Of PersonAddress)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of PersonAddress)()
                personAddresses = rep.Find("Where PersonId = @0", personId)

                For Each address In personAddresses
                    RemovePersonAddress(address)
                Next
            End Using
        End Sub
    End Class

End Namespace
