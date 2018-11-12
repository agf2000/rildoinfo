
Public Class PeopleRepository
    Implements IPeopleRepository

#Region "Private Methods"

    Private Shared Function GetNull(field As Object) As Object
        Return Null.GetNull(field, DBNull.Value)
    End Function

#End Region

#Region "Public Methods"

    Public Function getPeople(portalId As Integer, salesRep As Integer, isDeleted As String, searchString As String, statusId As Integer, registrionType As String, startDate As DateTime, endDate As DateTime, pageIndex As Integer, pageSize As Integer, orderBy As String) As IEnumerable(Of Models.Person) Implements IPeopleRepository.getPeople
        Return CBO.FillCollection(Of Models.Person)(DataProvider.Instance().GetPeople(portalId, salesRep, isDeleted, searchString, statusId, registrionType, startDate, endDate, pageIndex, pageSize, orderBy))
        'Dim persons As IEnumerable(Of Models.Person)

        'Using ctx As IDataContext = DataContext.Instance()
        '    persons = ctx.ExecuteQuery(Of Models.Person)(CommandType.StoredProcedure, "RIW_People_GetList", portalId, salesRep, isDeleted, searchString, statusId, GetNull(startDate), GetNull(endDate), pageIndex, pageSize, orderBy).ToList()
        'End Using
        'Return persons
    End Function

    Public Function getPerson(personId As Integer, portalId As Integer, userId As Integer) As Models.Person Implements IPeopleRepository.getPerson
        Return CBO.FillObject(Of Models.Person)(DataProvider.Instance().GetPerson(personId, portalId, userId))
        'Dim person As Models.Person

        'Using ctx As IDataContext = DataContext.Instance()
        '    person = ctx.ExecuteSingleOrDefault(Of Models.Person)(CommandType.StoredProcedure, "RIW_Person_Get", personId, portalId)
        'End Using
        'Return person
    End Function

    Public Function addPerson(person As Models.Person) As Models.Person Implements IPeopleRepository.addPerson
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Person) = ctx.GetRepository(Of Models.Person)()
            rep.Insert(person)
        End Using
        Return person
    End Function

    Public Sub updatePerson(person As Models.Person) Implements IPeopleRepository.updatePerson
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Person) = ctx.GetRepository(Of Models.Person)()
            rep.Update(person)
        End Using
    End Sub

    Public Sub removePerson(person As Models.Person) Implements IPeopleRepository.removePerson
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Person) = ctx.GetRepository(Of Models.Person)()
            rep.Delete(person)
        End Using
    End Sub

    Public Sub removePerson(personId As Integer, portalId As Integer, cUserId As Integer) Implements IPeopleRepository.removePerson
        Dim _item As Models.Person = getPerson(personId, portalId, Null.NullInteger)
        If _item Is Nothing Then
            Throw New ArgumentException("Nada foi encontrado!")
        Else
            removePerson(_item)

            If cUserId > 0 Then
                Users.UserController.RemoveUser(Users.UserController.GetUserById(portalId, cUserId))
            End If
        End If
    End Sub

#End Region

End Class
