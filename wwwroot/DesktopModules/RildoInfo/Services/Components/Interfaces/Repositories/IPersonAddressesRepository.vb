
Public Interface IPersonAddressesRepository

    ''' <summary>
    ''' Gets a list of person addresses
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getPersonAddresses(ByVal personId As Integer) As IEnumerable(Of Models.PersonAddress)

    ''' <summary>
    ''' Gets a person address by id
    ''' </summary>
    ''' <param name="PersonAddressId">PersonAddress ID</param>
    ''' <param name="personId">Personl ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getPersonAddress(ByVal personAddressId As Integer, ByVal personId As Integer) As Models.PersonAddress

    ''' <summary>
    ''' Adds a person address
    ''' </summary>
    ''' <param name="personAddress">Person Address Model</param>
    ''' <remarks></remarks>
    Function addPersonAddress(personAddress As Models.PersonAddress) As Models.PersonAddress

    ''' <summary>
    ''' Updates a person address
    ''' </summary>
    ''' <param name="personAddress">Person Address Model</param>
    ''' <remarks></remarks>
    Sub updatePersonAddress(personAddress As Models.PersonAddress)

    ''' <summary>
    ''' Removes person address by person id
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <param name="personAddressId">Person Address ID</param>
    ''' <remarks></remarks>
    Sub removePersonAddress(personAddressId As Integer, personId As Integer)

    ''' <summary>
    ''' Remove all person addresses
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <remarks></remarks>
    Sub removePersonAddresses(personId As Integer)

    ''' <summary>
    ''' Removes a person address
    ''' </summary>
    ''' <param name="personAddress">Person Address Model</param>
    ''' <remarks></remarks>
    Sub removePersonAddress(ByVal personAddress As Models.PersonAddress)

End Interface
