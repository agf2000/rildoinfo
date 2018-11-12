Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IPersonDocsRepository

        ''' <summary>
        ''' Gets a list of person docs by person id
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPersonDocs(ByVal personId As Integer) As IEnumerable(Of PersonDoc)

        ''' <summary>
        ''' Gets a person doc by id
        ''' </summary>
        ''' <param name="personDocId">Person Doc ID</param>
        ''' <param name="personId">Personl ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPersonDoc(ByVal personDocId As Integer, ByVal personId As Integer) As PersonDoc

        ''' <summary>
        ''' Adds a person doc
        ''' </summary>
        ''' <param name="personDoc">Person Doc Model</param>
        ''' <remarks></remarks>
        Function AddPersonDoc(personDoc As PersonDoc) As PersonDoc

        ''' <summary>
        ''' Updates a person doc
        ''' </summary>
        ''' <param name="personDoc">Person Doc Model</param>
        ''' <remarks></remarks>
        Sub UpdatePersonDoc(personDoc As PersonDoc)

        ''' <summary>
        ''' Removes person doc by person id
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <param name="personDocId">Person Doc ID</param>
        ''' <remarks></remarks>
        Sub RemovePersonDoc(personDocId As Integer, personId As Integer)

        ''' <summary>
        ''' Remove all person doc
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <remarks></remarks>
        Sub RemovePersonDocs(personId As Integer)

        ''' <summary>
        ''' Removes a person doc
        ''' </summary>
        ''' <param name="personDoc">Person Doc Model</param>
        ''' <remarks></remarks>
        Sub RemovePersonDoc(ByVal personDoc As PersonDoc)

    End Interface

End Namespace
