
Public Interface IPersonDocsRepository

    ''' <summary>
    ''' Gets a list of person docs by person id
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getPersonDocs(ByVal personId As Integer) As IEnumerable(Of Models.PersonDoc)

    ''' <summary>
    ''' Gets a person doc by id
    ''' </summary>
    ''' <param name="personDocId">Person Doc ID</param>
    ''' <param name="personId">Personl ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getPersonDoc(ByVal personDocId As Integer, ByVal personId As Integer) As Models.PersonDoc

    ''' <summary>
    ''' Adds a person doc
    ''' </summary>
    ''' <param name="personDoc">Person Doc Model</param>
    ''' <remarks></remarks>
    Function addPersonDoc(personDoc As Models.PersonDoc) As Models.PersonDoc

    ''' <summary>
    ''' Updates a person doc
    ''' </summary>
    ''' <param name="personDoc">Person Doc Model</param>
    ''' <remarks></remarks>
    Sub updatePersonDoc(personDoc As Models.PersonDoc)

    ''' <summary>
    ''' Removes person doc by person id
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <param name="personDocId">Person Doc ID</param>
    ''' <remarks></remarks>
    Sub removePersonDoc(personDocId As Integer, personId As Integer)

    ''' <summary>
    ''' Remove all person doc
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <remarks></remarks>
    Sub removePersonDocs(personId As Integer)

    ''' <summary>
    ''' Removes a person doc
    ''' </summary>
    ''' <param name="personDoc">Person Doc Model</param>
    ''' <remarks></remarks>
    Sub removePersonDoc(ByVal personDoc As Models.PersonDoc)

End Interface
