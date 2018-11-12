
Imports System.Collections.Generic
Imports DotNetNuke.Data

Namespace RI.Modules.RIStore_Services
    Public Class ClientPartnersBankRefsRepository
        Implements IClientPartnerBankRefsRepository

        Public Function AddClientPartnerBankRef(clientPartnerBankRef As Models.ClientPartnerBankRef) As Models.ClientPartnerBankRef Implements IClientPartnerBankRefsRepository.AddClientPartnerBankRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientPartnerBankRef) = ctx.GetRepository(Of Models.ClientPartnerBankRef)()
                rep.Insert(clientPartnerBankRef)
            End Using
            Return clientPartnerBankRef
        End Function

        Public Function GetClientPartnerBankRef(clientPartnerBankRefId As Integer, clientId As Integer) As Models.ClientPartnerBankRef Implements IClientPartnerBankRefsRepository.GetClientPartnerBankRef
            Dim clientPartnerBankRef As Models.ClientPartnerBankRef

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientPartnerBankRef) = ctx.GetRepository(Of Models.ClientPartnerBankRef)()
                clientPartnerBankRef = rep.GetById(Of Int32, Int32)(clientPartnerBankRefId, clientId)
            End Using
            Return clientPartnerBankRef
        End Function

        Public Function GetClientPartnerBankRefs(clientId As Integer) As IEnumerable(Of Models.ClientPartnerBankRef) Implements IClientPartnerBankRefsRepository.GetClientPartnerBankRefs
            Dim clientPartnerBankRefs As IEnumerable(Of Models.ClientPartnerBankRef)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientPartnerBankRef) = ctx.GetRepository(Of Models.ClientPartnerBankRef)()
                clientPartnerBankRefs = rep.Get(clientId)
            End Using
            Return clientPartnerBankRefs
        End Function

        Public Sub RemoveClientPartnerBankRef(clientPartnerBankRefId As Integer, clientId As Integer) Implements IClientPartnerBankRefsRepository.RemoveClientPartnerBankRef
            Dim _item As Models.ClientPartnerBankRef = GetClientPartnerBankRef(clientPartnerBankRefId, clientId)
            If _item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveClientPartnerBankRef(_item)
            End If
        End Sub

        Public Sub RemoveClientPartnerBankRef(clientPartnerBankRef As Models.ClientPartnerBankRef) Implements IClientPartnerBankRefsRepository.RemoveClientPartnerBankRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientPartnerBankRef) = ctx.GetRepository(Of Models.ClientPartnerBankRef)()
                rep.Delete(clientPartnerBankRef)
            End Using
        End Sub

        Public Sub RemoveClientPartnerBankRefs(clientId As Integer) Implements IClientPartnerBankRefsRepository.RemoveClientPartnerBankRefs
            Dim clientPartnerBankRefs As IEnumerable(Of Models.ClientPartnerBankRef)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of Models.ClientPartnerBankRef)()
                clientPartnerBankRefs = rep.Find("Where ClientId = @0", clientId)

                For Each incomeSource In clientPartnerBankRefs
                    RemoveClientPartnerBankRef(incomeSource)
                Next
            End Using
        End Sub

        Public Sub UpdateClientPartnerBankRef(clientPartnerBankRef As Models.ClientPartnerBankRef) Implements IClientPartnerBankRefsRepository.UpdateClientPartnerBankRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientPartnerBankRef) = ctx.GetRepository(Of Models.ClientPartnerBankRef)()
                rep.Update(clientPartnerBankRef)
            End Using
        End Sub

    End Class
End Namespace