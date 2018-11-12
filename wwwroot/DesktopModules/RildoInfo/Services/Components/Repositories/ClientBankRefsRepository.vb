
Public Class ClientBankRefsRepository
    Implements IClientBankRefsRepository

    Public Function addClientBankRef(clientBankRef As Models.ClientBankRef) As Models.ClientBankRef Implements IClientBankRefsRepository.addClientBankRef
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientBankRef) = ctx.GetRepository(Of Models.ClientBankRef)()
            rep.Insert(clientBankRef)
        End Using
        Return clientBankRef
    End Function

    Public Function getClientBankRef(clientBankRefId As Integer, personId As Integer) As Models.ClientBankRef Implements IClientBankRefsRepository.getClientBankRef
        Dim clientBankRef As Models.ClientBankRef

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientBankRef) = ctx.GetRepository(Of Models.ClientBankRef)()
            clientBankRef = rep.GetById(Of Int32, Int32)(clientBankRefId, personId)
        End Using
        Return clientBankRef
    End Function

    Public Function getClientBankRefs(personId As Integer) As IEnumerable(Of Models.ClientBankRef) Implements IClientBankRefsRepository.getClientBankRefs
        Dim clientBankRefs As IEnumerable(Of Models.ClientBankRef)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientBankRef) = ctx.GetRepository(Of Models.ClientBankRef)()
            clientBankRefs = rep.Get(personId)
        End Using
        Return clientBankRefs
    End Function

    Public Sub removeClientBankRef(clientBankRefId As Integer, personId As Integer) Implements IClientBankRefsRepository.removeClientBankRef
        Dim _item As Models.ClientBankRef = getClientBankRef(clientBankRefId, personId)
        If _item.Locked Then
            Throw New ArgumentException("Exception Occured")
        Else
            removeClientBankRef(_item)
        End If
    End Sub

    Public Sub removeClientBankRef(clientBankRef As Models.ClientBankRef) Implements IClientBankRefsRepository.removeClientBankRef
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientBankRef) = ctx.GetRepository(Of Models.ClientBankRef)()
            rep.Delete(clientBankRef)
        End Using
    End Sub

    Public Sub removeClientBankRefs(personId As Integer) Implements IClientBankRefsRepository.removeClientBankRefs
        Dim clientBankRefs As IEnumerable(Of Models.ClientBankRef)
        Using context As IDataContext = DataContext.Instance()
            Dim rep = context.GetRepository(Of Models.ClientBankRef)()
            clientBankRefs = rep.Find("Where PersonId = @0", personId)

            For Each bankRef In clientBankRefs
                removeClientBankRef(bankRef)
            Next
        End Using
    End Sub

    Public Sub updateClientBankRef(clientBankRef As Models.ClientBankRef) Implements IClientBankRefsRepository.updateClientBankRef
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientBankRef) = ctx.GetRepository(Of Models.ClientBankRef)()
            rep.Update(clientBankRef)
        End Using
    End Sub

End Class
