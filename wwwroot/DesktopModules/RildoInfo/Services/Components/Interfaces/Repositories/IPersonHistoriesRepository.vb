
Public Interface IPersonHistoriesRepository

    ''' <summary>
    ''' Gets a list of person Histories by person id
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getPersonHistories(ByVal personId As Integer) As IEnumerable(Of Models.PersonHistory)

    ''' <summary>
    ''' Gets a person doc by id
    ''' </summary>
    ''' <param name="personHistoryId">PersonHistory ID</param>
    ''' <param name="personId">Personl ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getPersonHistory(ByVal personHistoryId As Integer, ByVal personId As Integer) As Models.PersonHistory

    ''' <summary>
    ''' Adds a person doc
    ''' </summary>
    ''' <param name="personHistory">Person History Model</param>
    ''' <remarks></remarks>
    Function addPersonHistory(personHistory As Models.PersonHistory) As Models.PersonHistory

    ''' <summary>
    ''' Updates a person doc
    ''' </summary>
    ''' <param name="personHistory">Person History Model</param>
    ''' <remarks></remarks>
    Sub updatePersonHistory(personHistory As Models.PersonHistory)

    ''' <summary>
    ''' Removes person doc by person id
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <param name="personHistoryId">Person History ID</param>
    ''' <remarks></remarks>
    Sub removePersonHistory(personHistoryId As Integer, personId As Integer)

    ''' <summary>
    ''' Remove all person doc
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <remarks></remarks>
    Sub removePersonHistories(personId As Integer)

    ''' <summary>
    ''' Removes a person doc
    ''' </summary>
    ''' <param name="personHistory">Person History Model</param>
    ''' <remarks></remarks>
    Sub removePersonHistory(ByVal personHistory As Models.PersonHistory)

    ''' <summary>
    ''' Gets estimate history comments
    ''' </summary>
    ''' <param name="personHistoryId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getPersonHistoryComments(personHistoryId As Integer) As IEnumerable(Of Models.PersonHistoryComment)

    ''' <summary>
    ''' Adds an estimate history comment
    ''' </summary>
    ''' <param name="personHistoryComment"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function addPersonHistoryComment(personHistoryComment As Models.PersonHistoryComment) As Models.PersonHistoryComment

    ''' <summary>
    ''' Gets an estimate message comment
    ''' </summary>
    ''' <param name="commentId"></param>
    ''' <param name="personHistoryId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getPersonHistoryComment(commentId As Integer, personHistoryId As Integer) As Models.PersonHistoryComment

    ''' <summary>
    ''' Updates an estimate history comment
    ''' </summary>
    ''' <param name="personHistoryComment"></param>
    ''' <remarks></remarks>
    Sub updatePersonHistoryComment(personHistoryComment As Models.PersonHistoryComment)

    ''' <summary>
    ''' Removes an estimate history comment
    ''' </summary>
    ''' <param name="commentId"></param>
    ''' <param name="personHistoryId"></param>
    ''' <remarks></remarks>
    Sub removePersonHistoryComment(commentId As Integer, personHistoryId As Integer)

    ''' <summary>
    ''' Removes a person history comment
    ''' </summary>
    ''' <param name="personHistoryComment"></param>
    ''' <remarks></remarks>
    Sub removePersonHistoryComment(personHistoryComment As Models.PersonHistoryComment)

End Interface
