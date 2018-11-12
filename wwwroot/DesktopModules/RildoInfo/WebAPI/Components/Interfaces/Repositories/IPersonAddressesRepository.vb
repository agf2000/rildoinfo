Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IPersonAddressesRepository

        ''' <summary>
        ''' Gets a list of person addresses
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPersonAddresses(personId As Integer) As IEnumerable(Of PersonAddress)

        ''' <summary>
        ''' Gets a person address by id
        ''' </summary>
        ''' <param name="PersonAddressId">PersonAddress ID</param>
        ''' <param name="personId">Personl ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPersonAddress(personAddressId As Integer, personId As Integer) As PersonAddress

        ''' <summary>
        ''' Gets a person address by id
        ''' </summary>
        ''' <param name="personId">Personl ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPersonMainAddress(personId As Integer) As PersonAddress

        ''' <summary>
        ''' Adds a person address
        ''' </summary>
        ''' <param name="personAddress">Person Address Model</param>
        ''' <remarks></remarks>
        Function AddPersonAddress(personAddress As PersonAddress) As PersonAddress

        ''' <summary>
        ''' Updates a person address
        ''' </summary>
        ''' <param name="personAddress">Person Address Model</param>
        ''' <remarks></remarks>
        Sub UpdatePersonAddress(personAddress As PersonAddress)

        ''' <summary>
        ''' Removes person address by person id
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <param name="personAddressId">Person Address ID</param>
        ''' <remarks></remarks>
        Sub RemovePersonAddress(personAddressId As Integer, personId As Integer)

        ''' <summary>
        ''' Remove all person addresses
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <remarks></remarks>
        Sub RemovePersonAddresses(personId As Integer)

        ''' <summary>
        ''' Removes a person address
        ''' </summary>
        ''' <param name="personAddress">Person Address Model</param>
        ''' <remarks></remarks>
        Sub RemovePersonAddress(personAddress As PersonAddress)

    End Interface

End Namespace
