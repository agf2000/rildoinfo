
Public Class ClientPartnersBankRefsRepository
    Implements IClientPartnerBankRefsRepository

    Public Function addClientPartnerBankRef(clientPartnerBankRef As Models.ClientPartnerBankRef) As Models.ClientPartnerBankRef Implements IClientPartnerBankRefsRepository.addClientPartnerBankRef
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientPartnerBankRef) = ctx.GetRepository(Of Models.ClientPartnerBankRef)()
            rep.Insert(clientPartnerBankRef)
        End Using
        Return clientPartnerBankRef
    End Function

    Public Function getClientPartnerBankRef(clientPartnerBankRefId As Integer, personId As Integer) As Models.ClientPartnerBankRef Implements IClientPartnerBankRefsRepository.getClientPartnerBankRef
        Dim clientPartnerBankRef As Models.ClientPartnerBankRef

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientPartnerBankRef) = ctx.GetRepository(Of Models.ClientPartnerBankRef)()
            clientPartnerBankRef = rep.GetById(Of Int32, Int32)(clientPartnerBankRefId, personId)
        End Using
        Return clientPartnerBankRef
    End Function

    Public Function getClientPartnerBankRefs(personId As Integer) As IEnumerable(Of Models.ClientPartnerBankRef) Implements IClientPartnerBankRefsRepository.getClientPartnerBankRefs
        Dim clientPartnerBankRefs As IEnumerable(Of Models.ClientPartnerBankRef)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientPartnerBankRef) = ctx.GetRepository(Of Models.ClientPartnerBankRef)()
            clientPartnerBankRefs = rep.Get(personId)
        End Using
        Return clientPartnerBankRefs
    End Function

    Public Sub removeClientPartnerBankRef(clientPartnerBankRefId As Integer, personId As Integer) Implements IClientPartnerBankRefsRepository.removeClientPartnerBankRef
        Dim _item As Models.ClientPartnerBankRef = getClientPartnerBankRef(clientPartnerBankRefId, personId)
        If _item.Locked Then
            Throw New ArgumentException("Exception Occured")
        Else
            removeClientPartnerBankRef(_item)
        End If
    End Sub

    Public Sub removeClientPartnerBankRef(clientPartnerBankRef As Models.ClientPartnerBankRef) Implements IClientPartnerBankRefsRepository.removeClientPartnerBankRef
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientPartnerBankRef) = ctx.GetRepository(Of Models.ClientPartnerBankRef)()
            rep.Delete(clientPartnerBankRef)
        End Using
    End Sub

    Public Sub removeClientPartnerBankRefs(personId As Integer) Implements IClientPartnerBankRefsRepository.removeClientPartnerBankRefs
        Dim clientPartnerBankRefs As IEnumerable(Of Models.ClientPartnerBankRef)
        Using context As IDataContext = DataContext.Instance()
            Dim rep = context.GetRepository(Of Models.ClientPartnerBankRef)()
            clientPartnerBankRefs = rep.Find("Where PersonId = @0", personId)

            For Each incomeSource In clientPartnerBankRefs
                removeClientPartnerBankRef(incomeSource)
            Next
        End Using
    End Sub

    Public Sub updateClientPartnerBankRef(clientPartnerBankRef As Models.ClientPartnerBankRef) Implements IClientPartnerBankRefsRepository.updateClientPartnerBankRef
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientPartnerBankRef) = ctx.GetRepository(Of Models.ClientPartnerBankRef)()
            rep.Update(clientPartnerBankRef)
        End Using
    End Sub

End Class
