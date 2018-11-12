
Public Class PersonContactsRepository
    Implements IPersonContactsRepository

    Public Function addPersonContact(personContact As Models.PersonContact) As Models.PersonContact Implements IPersonContactsRepository.addPersonContact
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonContact) = ctx.GetRepository(Of Models.PersonContact)()
            rep.Insert(personContact)
        End Using
        Return personContact
    End Function

    Public Function getPersonContact(personContactId As Integer, personId As Integer) As Models.PersonContact Implements IPersonContactsRepository.getPersonContact
        Dim personContact As Models.PersonContact

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonContact) = ctx.GetRepository(Of Models.PersonContact)()
            personContact = rep.GetById(Of Int32, Int32)(personContactId, personId)
        End Using
        Return personContact
    End Function

    Public Function getPersonContact(contactEmail As String, portalId As Integer) As Models.PersonContact Implements IPersonContactsRepository.getPersonContact
        Dim personContact As Models.PersonContact

        Using ctx As IDataContext = DataContext.Instance()
            personContact = ctx.ExecuteSingleOrDefault(Of Models.PersonContact)(CommandType.Text, "where contactemail1 = @0", contactEmail)
        End Using
        Return personContact
    End Function

    Public Function getPersonContacts(personId As Integer) As IEnumerable(Of Models.PersonContact) Implements IPersonContactsRepository.getPersonContacts
        Dim personContacts As IEnumerable(Of Models.PersonContact)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonContact) = ctx.GetRepository(Of Models.PersonContact)()
            personContacts = rep.Get(personId)
        End Using
        Return personContacts
    End Function

    Public Sub removePersonContact(personContactId As Integer, personId As Integer) Implements IPersonContactsRepository.removePersonContact
        Dim _item As Models.PersonContact = getPersonContact(personContactId, personId)
        If _item.Locked Then
            Throw New ArgumentException("Exception Occured")
        Else
            removePersonContact(_item)
        End If
    End Sub

    Public Sub removePersonContact(personContact As Models.PersonContact) Implements IPersonContactsRepository.removePersonContact
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonContact) = ctx.GetRepository(Of Models.PersonContact)()
            rep.Delete(personContact)
        End Using
    End Sub

    Public Sub removePersonContacts(personId As Integer) Implements IPersonContactsRepository.removePersonContacts
        Dim personContacts As IEnumerable(Of Models.PersonContact)
        Using context As IDataContext = DataContext.Instance()
            Dim rep = context.GetRepository(Of Models.PersonContact)()
            personContacts = rep.Find("Where PersonId = @0", personId)

            For Each bankRef In personContacts
                removePersonContact(bankRef)
            Next
        End Using
    End Sub

    Public Sub updatePersonContact(personContact As Models.PersonContact) Implements IPersonContactsRepository.updatePersonContact
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonContact) = ctx.GetRepository(Of Models.PersonContact)()
            rep.Update(personContact)
        End Using
    End Sub

End Class
