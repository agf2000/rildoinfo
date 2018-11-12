
Public Class PersonAddressesRepository
    Implements IPersonAddressesRepository

    Public Function addPersonAddress(personAddress As Models.PersonAddress) As Models.PersonAddress Implements IPersonAddressesRepository.addPersonAddress
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonAddress) = ctx.GetRepository(Of Models.PersonAddress)()
            rep.Insert(personAddress)
        End Using
        Return personAddress
    End Function

    Public Function getPersonAddress(personAddressId As Integer, personId As Integer) As Models.PersonAddress Implements IPersonAddressesRepository.getPersonAddress
        Dim personAddress As Models.PersonAddress

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonAddress) = ctx.GetRepository(Of Models.PersonAddress)()
            personAddress = rep.GetById(Of Int32, Int32)(personAddressId, personId)
        End Using
        Return personAddress
    End Function

    Public Function getPersonAddresses(personId As Integer) As IEnumerable(Of Models.PersonAddress) Implements IPersonAddressesRepository.getPersonAddresses
        Dim personAddresses As IEnumerable(Of Models.PersonAddress)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonAddress) = ctx.GetRepository(Of Models.PersonAddress)()
            personAddresses = rep.Get(personId)
        End Using
        Return personAddresses
    End Function

    Public Sub removePersonAddress(personAddressId As Integer, personId As Integer) Implements IPersonAddressesRepository.removePersonAddress
        Dim _item As Models.PersonAddress = getPersonAddress(personAddressId, personId)
        If _item.Locked Then
            Throw New ArgumentException("Exception Occured")
        Else
            removePersonAddress(_item)
        End If
    End Sub

    Public Sub removePersonAddress(personAddress As Models.PersonAddress) Implements IPersonAddressesRepository.removePersonAddress
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonAddress) = ctx.GetRepository(Of Models.PersonAddress)()
            rep.Delete(personAddress)
        End Using
    End Sub

    Public Sub updatePersonAddress(personAddress As Models.PersonAddress) Implements IPersonAddressesRepository.updatePersonAddress
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonAddress) = ctx.GetRepository(Of Models.PersonAddress)()
            rep.Update(personAddress)
        End Using
    End Sub

    Public Sub removePersonAddresses(personId As Integer) Implements IPersonAddressesRepository.removePersonAddresses
        Dim personAddresses As IEnumerable(Of Models.PersonAddress)
        Using context As IDataContext = DataContext.Instance()
            Dim rep = context.GetRepository(Of Models.PersonAddress)()
            personAddresses = rep.Find("Where PersonId = @0", personId)

            For Each address In personAddresses
                removePersonAddress(address)
            Next
        End Using
    End Sub

End Class
