
Imports System.Collections.Generic
Imports DotNetNuke.Data

Namespace RI.Modules.RIStore_Services
    Public Class ClientBankRefsRepository
        Implements IClientBankRefsRepository

        Public Function AddClientBankRef(clientBankRef As Models.ClientBankRef) As Models.ClientBankRef Implements IClientBankRefsRepository.AddClientBankRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientBankRef) = ctx.GetRepository(Of Models.ClientBankRef)()
                rep.Insert(clientBankRef)
            End Using
            Return clientBankRef
        End Function

        Public Function GetClientBankRef(clientBankRefId As Integer, clientId As Integer) As Models.ClientBankRef Implements IClientBankRefsRepository.GetClientBankRef
            Dim clientBankRef As Models.ClientBankRef

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientBankRef) = ctx.GetRepository(Of Models.ClientBankRef)()
                clientBankRef = rep.GetById(Of Int32, Int32)(clientBankRefId, clientId)
            End Using
            Return clientBankRef
        End Function

        Public Function GetClientBankRefs(clientId As Integer) As IEnumerable(Of Models.ClientBankRef) Implements IClientBankRefsRepository.GetClientBankRefs
            Dim clientBankRefs As IEnumerable(Of Models.ClientBankRef)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientBankRef) = ctx.GetRepository(Of Models.ClientBankRef)()
                clientBankRefs = rep.Get(clientId)
            End Using
            Return clientBankRefs
        End Function

        Public Sub RemoveClientBankRef(clientBankRefId As Integer, clientId As Integer) Implements IClientBankRefsRepository.RemoveClientBankRef
            Dim _item As Models.ClientBankRef = GetClientBankRef(clientBankRefId, clientId)
            If _item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveClientBankRef(_item)
            End If
        End Sub

        Public Sub RemoveClientBankRef(clientBankRef As Models.ClientBankRef) Implements IClientBankRefsRepository.RemoveClientBankRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientBankRef) = ctx.GetRepository(Of Models.ClientBankRef)()
                rep.Delete(clientBankRef)
            End Using
        End Sub

        Public Sub RemoveClientBankRefs(clientId As Integer) Implements IClientBankRefsRepository.RemoveClientBankRefs
            Dim clientBankRefs As IEnumerable(Of Models.ClientBankRef)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of Models.ClientBankRef)()
                clientBankRefs = rep.Find("Where ClientId = @0", clientId)

                For Each bankRef In clientBankRefs
                    RemoveClientBankRef(bankRef)
                Next
            End Using
        End Sub

        Public Sub UpdateClientBankRef(clientBankRef As Models.ClientBankRef) Implements IClientBankRefsRepository.UpdateClientBankRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientBankRef) = ctx.GetRepository(Of Models.ClientBankRef)()
                rep.Update(clientBankRef)
            End Using
        End Sub

    End Class
End Namespace