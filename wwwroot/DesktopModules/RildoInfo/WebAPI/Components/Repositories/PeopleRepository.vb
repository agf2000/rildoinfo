Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Repositories

    Public Class PeopleRepository
    Implements IPeopleRepository

        '#Region "Private Methods"

        '    Private Shared Function GetNull(field As Object) As Object
        '        Return Null.GetNull(field, DBNull.Value)
        '    End Function

        '#End Region

        #Region "Public Methods"

        Public Function GetPeople(portalId As Integer, searchField As String, salesRep As Integer, isDeleted As String, searchString As String,
                                  statusId As Integer, registrationType As String, startDate As DateTime, endDate As DateTime, filterDate As String,
                                  pageIndex As Integer, pageSize As Integer, orderBy As String, orderDesc As String) As IEnumerable(Of Person) Implements IPeopleRepository.GetPeople
            Return CBO.FillCollection(Of Person)(DataProvider.Instance().GetPeople(portalId, searchField, salesRep, isDeleted, searchString, statusId,
                                                                                   registrationType, startDate, endDate, filterDate, pageIndex, pageSize, orderBy, orderDesc))
            'Dim persons As IEnumerable(Of Models.Person)

            'Using ctx As IDataContext = DataContext.Instance()
            '    persons = ctx.ExecuteQuery(Of Models.Person)(CommandType.StoredProcedure, "RIW_People_GetList", portalId, salesRep, isDeleted, searchString, statusId, GetNull(startDate), GetNull(endDate), pageIndex, pageSize, orderBy).ToList()
            'End Using
            'Return persons
        End Function

        Public Function GetPerson(personId As Integer, portalId As Integer, userId As Integer) As Person Implements IPeopleRepository.GetPerson
            Return CBO.FillObject(Of Person)(DataProvider.Instance().GetPerson(personId, portalId, userId))
        End Function

        Public Function AddPerson(person As Person) As Person Implements IPeopleRepository.AddPerson
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Person) = ctx.GetRepository(Of Person)()
                rep.Insert(person)
            End Using
            Return person
        End Function

        Public Sub UpdatePerson(person As Person) Implements IPeopleRepository.UpdatePerson
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Person) = ctx.GetRepository(Of Person)()
                rep.Update(person)
            End Using
        End Sub

        Public Sub RemovePerson(person As Person) Implements IPeopleRepository.RemovePerson
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Person) = ctx.GetRepository(Of Person)()
                rep.Delete(person)
            End Using
        End Sub

        Public Sub RemovePerson(personId As Integer, portalId As Integer, cUserId As Integer) Implements IPeopleRepository.RemovePerson
            Dim item As Person = GetPerson(personId, portalId, Null.NullInteger)
            If item Is Nothing Then
                Throw New ArgumentException("Nada foi encontrado!")
            Else
                RemovePerson(item)

                If cUserId > 0 Then
                    Users.UserController.RemoveUser(Users.UserController.GetUserById(portalId, cUserId))
                End If
            End If
        End Sub

        Public Function GetPeopleCount(portalId As Integer, userId As Integer, types As String, Optional isDeleted As Boolean = False) As Integer Implements IPeopleRepository.GetPeopleCount
            Dim peopleCount As IEnumerable(Of PersonCount)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of PersonCount)()
                peopleCount = rep.Find("where portalid = @0 and ((not @1 > 0) or (salesrep = @1)) and registertypes in (@2) and isdeleted = @3",
                                       portalId, userId, types, isDeleted)
            End Using
            Return peopleCount.Count
        End Function

        Public Function GetUsers(portalId As Integer, roleName As String, isDeleted As String, searchStr As String, startDate As DateTime, endDate As DateTime, pageIndex As Integer, pageSize As Integer, sortCol As String) As IEnumerable(Of Person) Implements IPeopleRepository.GetUsers
            Return CBO.FillCollection(Of Person)(DataProvider.Instance().GetUsers(portalId, roleName, isDeleted, searchStr, startDate, endDate, pageIndex, pageSize, sortCol))
        End Function

        Public Function GetUsersCount(portalId As Integer) As Integer Implements IPeopleRepository.GetUsersCount
            Dim peopleCount As IEnumerable(Of PersonCount)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of PersonCount)()
                peopleCount = rep.Find("where isdeleted = 'False' and portalid = @0 and ((not @1 > 0) or (salesrep = @1))", portalId)
            End Using
            Return peopleCount.Count
        End Function

        #End Region

    End Class

End Namespace
