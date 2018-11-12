
Public Class ClientPersonalRefsRepository
    Implements IClientPersonalRefsRepository

    Public Function addClientPersonalRef(clientPersonalRef As Models.ClientPersonalRef) As Models.ClientPersonalRef Implements IClientPersonalRefsRepository.addClientPersonalRef
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientPersonalRef) = ctx.GetRepository(Of Models.ClientPersonalRef)()
            rep.Insert(clientPersonalRef)
        End Using
        Return clientPersonalRef
    End Function

    Public Function getClientPersonalRef(ClientPersonalRefId As Integer, personId As Integer) As Models.ClientPersonalRef Implements IClientPersonalRefsRepository.getClientPersonalRef
        Dim clientPersonalRef As Models.ClientPersonalRef

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientPersonalRef) = ctx.GetRepository(Of Models.ClientPersonalRef)()
            clientPersonalRef = rep.GetById(Of Int32, Int32)(ClientPersonalRefId, personId)
        End Using
        Return clientPersonalRef
    End Function

    Public Function getClientPersonalRefs(personId As Integer) As IEnumerable(Of Models.ClientPersonalRef) Implements IClientPersonalRefsRepository.getClientPersonalRefs
        Dim clientPersonalRefs As IEnumerable(Of Models.ClientPersonalRef)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientPersonalRef) = ctx.GetRepository(Of Models.ClientPersonalRef)()
            clientPersonalRefs = rep.Get(personId)
        End Using
        Return clientPersonalRefs
    End Function

    Public Sub removeClientPersonalRef(ClientPersonalRefId As Integer, personId As Integer) Implements IClientPersonalRefsRepository.removeClientPersonalRef
        Dim _item As Models.ClientPersonalRef = getClientPersonalRef(ClientPersonalRefId, personId)
        If _item.Locked Then
            Throw New ArgumentException("Exception Occured")
        Else
            removeClientPersonalRef(_item)
        End If
    End Sub

    Public Sub removeClientPersonalRef(clientPersonalRef As Models.ClientPersonalRef) Implements IClientPersonalRefsRepository.removeClientPersonalRef
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientPersonalRef) = ctx.GetRepository(Of Models.ClientPersonalRef)()
            rep.Delete(clientPersonalRef)
        End Using
    End Sub

    Public Sub removeClientPersonalRefs(personId As Integer) Implements IClientPersonalRefsRepository.removeClientPersonalRefs
        Dim clientPersonalRefs As IEnumerable(Of Models.ClientPersonalRef)
        Using context As IDataContext = DataContext.Instance()
            Dim rep = context.GetRepository(Of Models.ClientPersonalRef)()
            clientPersonalRefs = rep.Find("Where PersonId = @0", personId)

            For Each personalRef In clientPersonalRefs
                removeClientPersonalRef(personalRef)
            Next
        End Using
    End Sub

    Public Sub updateClientPersonalRef(clientPersonalRef As Models.ClientPersonalRef) Implements IClientPersonalRefsRepository.updateClientPersonalRef
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientPersonalRef) = ctx.GetRepository(Of Models.ClientPersonalRef)()
            rep.Update(clientPersonalRef)
        End Using
    End Sub

End Class
