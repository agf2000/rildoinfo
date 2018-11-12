Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Repositories

    Public Class PersonContactsRepository
    Implements IPersonContactsRepository

        Public Function AddPersonContact(personContact As PersonContact) As PersonContact Implements IPersonContactsRepository.AddPersonContact
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonContact) = ctx.GetRepository(Of PersonContact)()
                rep.Insert(personContact)
            End Using
            Return personContact
        End Function

        Public Function GetPersonContact(personContactId As Integer, personId As Integer) As PersonContact Implements IPersonContactsRepository.GetPersonContact
            Dim personContact As PersonContact

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonContact) = ctx.GetRepository(Of PersonContact)()
                personContact = rep.GetById(Of Int32, Int32)(personContactId, personId)
            End Using
            Return personContact
        End Function

        Public Function GetPersonContact(contactEmail As String, personId As Integer) As PersonContact Implements IPersonContactsRepository.GetPersonContact
            Dim personContact As PersonContact

            Using ctx As IDataContext = DataContext.Instance()
                personContact = ctx.ExecuteSingleOrDefault(Of PersonContact)(CommandType.Text, "where contactemail1 = @0 and personid = @1", contactEmail, personId)
            End Using
            Return personContact
        End Function

        Public Function GetPersonContacts(personId As Integer) As IEnumerable(Of PersonContact) Implements IPersonContactsRepository.GetPersonContacts
            Dim personContacts As IEnumerable(Of PersonContact)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonContact) = ctx.GetRepository(Of PersonContact)()
                personContacts = rep.Get(personId)
            End Using
            Return personContacts
        End Function

        Public Sub RemovePersonContact(personContactId As Integer, personId As Integer) Implements IPersonContactsRepository.RemovePersonContact
            Dim item As PersonContact = GetPersonContact(personContactId, personId)
            If item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemovePersonContact(item)
            End If
        End Sub

        Public Sub RemovePersonContact(personContact As PersonContact) Implements IPersonContactsRepository.RemovePersonContact
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonContact) = ctx.GetRepository(Of PersonContact)()
                rep.Delete(personContact)
            End Using
        End Sub

        Public Sub RemovePersonContacts(personId As Integer) Implements IPersonContactsRepository.RemovePersonContacts
            Dim personContacts As IEnumerable(Of PersonContact)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of PersonContact)()
                personContacts = rep.Find("Where PersonId = @0", personId)

                For Each bankRef In personContacts
                    RemovePersonContact(bankRef)
                Next
            End Using
        End Sub

        Public Sub UpdatePersonContact(personContact As PersonContact) Implements IPersonContactsRepository.UpdatePersonContact
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonContact) = ctx.GetRepository(Of PersonContact)()
                rep.Update(personContact)
            End Using
        End Sub

    End Class

End Namespace
