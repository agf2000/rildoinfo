Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IPersonContactsRepository

        ''' <summary>
        ''' Gets a list of person contacts by person id
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPersonContacts(ByVal personId As Integer) As IEnumerable(Of PersonContact)

        ''' <summary>
        ''' Gets a person contact by id
        ''' </summary>
        ''' <param name="personContactId">PersonContact ID</param>
        ''' <param name="personId">Personl ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPersonContact(ByVal personContactId As Integer, ByVal personId As Integer) As PersonContact

        ''' <summary>
        ''' Gets a person contact by id
        ''' </summary>
        ''' <param name="contactEmail">PersonContact Email</param>
        ''' <param name="personId">Person ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPersonContact(ByVal contactEmail As String, ByVal personId As Integer) As PersonContact

        ''' <summary>
        ''' Adds a person contact
        ''' </summary>
        ''' <param name="personContact">Person Contact Model</param>
        ''' <remarks></remarks>
        Function AddPersonContact(personContact As PersonContact) As PersonContact

        ''' <summary>
        ''' Updates a person contact
        ''' </summary>
        ''' <param name="personContact">Person Contact Model</param>
        ''' <remarks></remarks>
        Sub UpdatePersonContact(personContact As PersonContact)

        ''' <summary>
        ''' Removes person contact by person id
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <param name="personContactId">Person Contact ID</param>
        ''' <remarks></remarks>
        Sub RemovePersonContact(personContactId As Integer, personId As Integer)

        ''' <summary>
        ''' Remove all person contact
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <remarks></remarks>
        Sub RemovePersonContacts(personId As Integer)

        ''' <summary>
        ''' Removes a person contact
        ''' </summary>
        ''' <param name="personContact">Person Contact Model</param>
        ''' <remarks></remarks>
        Sub RemovePersonContact(ByVal personContact As PersonContact)

    End Interface

End Namespace
