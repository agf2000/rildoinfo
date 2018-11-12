
Imports System.Collections.Generic
Imports DotNetNuke.Data

Namespace RI.Modules.RIStore_Services
    Public Class ClientContactsRepository
        Implements IClientContactsRepository

        Public Function AddClientContact(clientContact As Models.ClientContact) As Models.ClientContact Implements IClientContactsRepository.AddClientContact
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientContact) = ctx.GetRepository(Of Models.ClientContact)()
                rep.Insert(clientContact)
            End Using
            Return clientContact
        End Function

        Public Function GetClientContact(clientContactId As Integer, clientId As Integer) As Models.ClientContact Implements IClientContactsRepository.GetClientContact
            Dim clientContact As Models.ClientContact

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientContact) = ctx.GetRepository(Of Models.ClientContact)()
                clientContact = rep.GetById(Of Int32, Int32)(clientContactId, clientId)
            End Using
            Return clientContact
        End Function

        Public Function GetClientContacts(clientId As Integer) As IEnumerable(Of Models.ClientContact) Implements IClientContactsRepository.GetClientContacts
            Dim clientContacts As IEnumerable(Of Models.ClientContact)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientContact) = ctx.GetRepository(Of Models.ClientContact)()
                clientContacts = rep.Get(clientId)
            End Using
            Return clientContacts
        End Function

        Public Sub RemoveClientContact(clientContactId As Integer, clientId As Integer) Implements IClientContactsRepository.RemoveClientContact
            Dim _item As Models.ClientContact = GetClientContact(clientContactId, clientId)
            If _item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveClientContact(_item)
            End If
        End Sub

        Public Sub RemoveClientContact(clientContact As Models.ClientContact) Implements IClientContactsRepository.RemoveClientContact
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientContact) = ctx.GetRepository(Of Models.ClientContact)()
                rep.Delete(clientContact)
            End Using
        End Sub

        Public Sub RemoveClientContacts(clientId As Integer) Implements IClientContactsRepository.RemoveClientContacts
            Dim clientContacts As IEnumerable(Of Models.ClientContact)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of Models.ClientContact)()
                clientContacts = rep.Find("Where ClientId = @0", clientId)

                For Each bankRef In clientContacts
                    RemoveClientContact(bankRef)
                Next
            End Using
        End Sub

        Public Sub UpdateClientContact(clientContact As Models.ClientContact) Implements IClientContactsRepository.UpdateClientContact
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientContact) = ctx.GetRepository(Of Models.ClientContact)()
                rep.Update(clientContact)
            End Using
        End Sub

    End Class
End Namespace