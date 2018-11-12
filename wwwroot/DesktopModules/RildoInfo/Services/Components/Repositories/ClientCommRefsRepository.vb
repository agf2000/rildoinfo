
Public Class ClientCommRefsRepository
    Implements IClientCommRefsRepository

    Public Function addClientCommRef(clientCommRef As Models.ClientCommRef) As Models.ClientCommRef Implements IClientCommRefsRepository.addClientCommRef
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientCommRef) = ctx.GetRepository(Of Models.ClientCommRef)()
            rep.Insert(clientCommRef)
        End Using
        Return clientCommRef
    End Function

    Public Function getClientCommRef(clientCommRefId As Integer, personId As Integer) As Models.ClientCommRef Implements IClientCommRefsRepository.getClientCommRef
        Dim clientCommRef As Models.ClientCommRef

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientCommRef) = ctx.GetRepository(Of Models.ClientCommRef)()
            clientCommRef = rep.GetById(Of Int32, Int32)(clientCommRefId, personId)
        End Using
        Return clientCommRef
    End Function

    Public Function getClientCommRefs(personId As Integer) As IEnumerable(Of Models.ClientCommRef) Implements IClientCommRefsRepository.getClientCommRefs
        Dim clientCommRefs As IEnumerable(Of Models.ClientCommRef)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientCommRef) = ctx.GetRepository(Of Models.ClientCommRef)()
            clientCommRefs = rep.Get(personId)
        End Using
        Return clientCommRefs
    End Function

    Public Sub removeClientCommRef(clientCommRefId As Integer, personId As Integer) Implements IClientCommRefsRepository.RemoveClientCommRef
        Dim _item As Models.ClientCommRef = getClientCommRef(clientCommRefId, personId)
        If _item.Locked Then
            Throw New ArgumentException("Exception Occured")
        Else
            removeClientCommRef(_item)
        End If
    End Sub

    Public Sub removeClientCommRef(clientCommRef As Models.ClientCommRef) Implements IClientCommRefsRepository.RemoveClientCommRef
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientCommRef) = ctx.GetRepository(Of Models.ClientCommRef)()
            rep.Delete(clientCommRef)
        End Using
    End Sub

    Public Sub removeClientCommRefs(personId As Integer) Implements IClientCommRefsRepository.removeClientCommRefs
        Dim clientCommRefs As IEnumerable(Of Models.ClientCommRef)
        Using context As IDataContext = DataContext.Instance()
            Dim rep = context.GetRepository(Of Models.ClientCommRef)()
            clientCommRefs = rep.Find("Where PersonId = @0", personId)

            For Each commRef In clientCommRefs
                removeClientCommRef(commRef)
            Next
        End Using
    End Sub

    Public Sub updateClientCommRef(clientCommRef As Models.ClientCommRef) Implements IClientCommRefsRepository.updateClientCommRef
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientCommRef) = ctx.GetRepository(Of Models.ClientCommRef)()
            rep.Update(clientCommRef)
        End Using
    End Sub

End Class
