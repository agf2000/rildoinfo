Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models

Namespace Components.Repositories

    Public Class ClientPartnersBankRefsRepository
    Implements IClientPartnerBankRefsRepository

        Public Function AddClientPartnerBankRef(clientPartnerBankRef As ClientPartnerBankRef) As ClientPartnerBankRef Implements IClientPartnerBankRefsRepository.AddClientPartnerBankRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientPartnerBankRef) = ctx.GetRepository(Of ClientPartnerBankRef)()
                rep.Insert(clientPartnerBankRef)
            End Using
            Return clientPartnerBankRef
        End Function

        Public Function GetClientPartnerBankRef(clientPartnerBankRefId As Integer, personId As Integer) As ClientPartnerBankRef Implements IClientPartnerBankRefsRepository.GetClientPartnerBankRef
            Dim clientPartnerBankRef As ClientPartnerBankRef

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientPartnerBankRef) = ctx.GetRepository(Of ClientPartnerBankRef)()
                clientPartnerBankRef = rep.GetById(Of Int32, Int32)(clientPartnerBankRefId, personId)
            End Using
            Return clientPartnerBankRef
        End Function

        Public Function GetClientPartnerBankRefs(personId As Integer) As IEnumerable(Of ClientPartnerBankRef) Implements IClientPartnerBankRefsRepository.GetClientPartnerBankRefs
            Dim clientPartnerBankRefs As IEnumerable(Of ClientPartnerBankRef)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientPartnerBankRef) = ctx.GetRepository(Of ClientPartnerBankRef)()
                clientPartnerBankRefs = rep.Get(personId)
            End Using
            Return clientPartnerBankRefs
        End Function

        Public Sub RemoveClientPartnerBankRef(clientPartnerBankRefId As Integer, personId As Integer) Implements IClientPartnerBankRefsRepository.RemoveClientPartnerBankRef
            Dim item As ClientPartnerBankRef = GetClientPartnerBankRef(clientPartnerBankRefId, personId)
            If item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveClientPartnerBankRef(item)
            End If
        End Sub

        Public Sub RemoveClientPartnerBankRef(clientPartnerBankRef As ClientPartnerBankRef) Implements IClientPartnerBankRefsRepository.RemoveClientPartnerBankRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientPartnerBankRef) = ctx.GetRepository(Of ClientPartnerBankRef)()
                rep.Delete(clientPartnerBankRef)
            End Using
        End Sub

        Public Sub RemoveClientPartnerBankRefs(personId As Integer) Implements IClientPartnerBankRefsRepository.RemoveClientPartnerBankRefs
            Dim clientPartnerBankRefs As IEnumerable(Of ClientPartnerBankRef)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of ClientPartnerBankRef)()
                clientPartnerBankRefs = rep.Find("Where PersonId = @0", personId)

                For Each incomeSource In clientPartnerBankRefs
                    RemoveClientPartnerBankRef(incomeSource)
                Next
            End Using
        End Sub

        Public Sub UpdateClientPartnerBankRef(clientPartnerBankRef As ClientPartnerBankRef) Implements IClientPartnerBankRefsRepository.UpdateClientPartnerBankRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientPartnerBankRef) = ctx.GetRepository(Of ClientPartnerBankRef)()
                rep.Update(clientPartnerBankRef)
            End Using
        End Sub

    End Class

End Namespace
