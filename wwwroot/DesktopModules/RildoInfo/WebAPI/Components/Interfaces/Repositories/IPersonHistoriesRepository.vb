Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IPersonHistoriesRepository

        ''' <summary>
        ''' Gets a list of person Histories by person id
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPersonHistories(ByVal personId As Integer) As IEnumerable(Of PersonHistory)

        ''' <summary>
        ''' Gets a person doc by id
        ''' </summary>
        ''' <param name="personHistoryId">PersonHistory ID</param>
        ''' <param name="personId">Personl ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPersonHistory(ByVal personHistoryId As Integer, ByVal personId As Integer) As PersonHistory

        ''' <summary>
        ''' Adds a person doc
        ''' </summary>
        ''' <param name="personHistory">Person History Model</param>
        ''' <remarks></remarks>
        Function AddPersonHistory(personHistory As PersonHistory) As PersonHistory

        ''' <summary>
        ''' Updates a person doc
        ''' </summary>
        ''' <param name="personHistory">Person History Model</param>
        ''' <remarks></remarks>
        Sub UpdatePersonHistory(personHistory As PersonHistory)

        ''' <summary>
        ''' Removes person doc by person id
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <param name="personHistoryId">Person History ID</param>
        ''' <remarks></remarks>
        Sub RemovePersonHistory(personHistoryId As Integer, personId As Integer)

        ''' <summary>
        ''' Remove all person doc
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <remarks></remarks>
        Sub RemovePersonHistories(personId As Integer)

        ''' <summary>
        ''' Removes a person doc
        ''' </summary>
        ''' <param name="personHistory">Person History Model</param>
        ''' <remarks></remarks>
        Sub RemovePersonHistory(ByVal personHistory As PersonHistory)

        ''' <summary>
        ''' Gets estimate history comments
        ''' </summary>
        ''' <param name="personHistoryId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPersonHistoryComments(personHistoryId As Integer) As IEnumerable(Of PersonHistoryComment)

        ''' <summary>
        ''' Adds an estimate history comment
        ''' </summary>
        ''' <param name="personHistoryComment"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddPersonHistoryComment(personHistoryComment As PersonHistoryComment) As PersonHistoryComment

        ''' <summary>
        ''' Gets an estimate message comment
        ''' </summary>
        ''' <param name="commentId"></param>
        ''' <param name="personHistoryId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPersonHistoryComment(commentId As Integer, personHistoryId As Integer) As PersonHistoryComment

        ''' <summary>
        ''' Updates an estimate history comment
        ''' </summary>
        ''' <param name="personHistoryComment"></param>
        ''' <remarks></remarks>
        Sub UpdatePersonHistoryComment(personHistoryComment As PersonHistoryComment)

        ''' <summary>
        ''' Removes an estimate history comment
        ''' </summary>
        ''' <param name="commentId"></param>
        ''' <param name="personHistoryId"></param>
        ''' <remarks></remarks>
        Sub RemovePersonHistoryComment(commentId As Integer, personHistoryId As Integer)

        ''' <summary>
        ''' Removes a person history comment
        ''' </summary>
        ''' <param name="personHistoryComment"></param>
        ''' <remarks></remarks>
        Sub RemovePersonHistoryComment(personHistoryComment As PersonHistoryComment)

    End Interface

End Namespace
