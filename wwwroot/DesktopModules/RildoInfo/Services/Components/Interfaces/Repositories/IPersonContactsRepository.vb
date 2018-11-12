
Public Interface IPersonContactsRepository

    ''' <summary>
    ''' Gets a list of person contacts by person id
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getPersonContacts(ByVal personId As Integer) As IEnumerable(Of Models.PersonContact)

    ''' <summary>
    ''' Gets a person contact by id
    ''' </summary>
    ''' <param name="personContactId">PersonContact ID</param>
    ''' <param name="personId">Personl ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getPersonContact(ByVal personContactId As Integer, ByVal personId As Integer) As Models.PersonContact

    ''' <summary>
    ''' Gets a person contact by id
    ''' </summary>
    ''' <param name="contactEmail">PersonContact Email</param>
    ''' <param name="portalId">Personl ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getPersonContact(ByVal contactEmail As String, ByVal portalId As Integer) As Models.PersonContact

    ''' <summary>
    ''' Adds a person contact
    ''' </summary>
    ''' <param name="personContact">Person Contact Model</param>
    ''' <remarks></remarks>
    Function addPersonContact(personContact As Models.PersonContact) As Models.PersonContact

    ''' <summary>
    ''' Updates a person contact
    ''' </summary>
    ''' <param name="personContact">Person Contact Model</param>
    ''' <remarks></remarks>
    Sub updatePersonContact(personContact As Models.PersonContact)

    ''' <summary>
    ''' Removes person contact by person id
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <param name="personContactId">Person Contact ID</param>
    ''' <remarks></remarks>
    Sub removePersonContact(personContactId As Integer, personId As Integer)

    ''' <summary>
    ''' Remove all person contact
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <remarks></remarks>
    Sub removePersonContacts(personId As Integer)

    ''' <summary>
    ''' Removes a person contact
    ''' </summary>
    ''' <param name="personContact">Person Contact Model</param>
    ''' <remarks></remarks>
    Sub removePersonContact(ByVal personContact As Models.PersonContact)

End Interface
