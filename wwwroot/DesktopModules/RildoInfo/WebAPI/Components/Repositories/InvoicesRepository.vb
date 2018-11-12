Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Repositories

    Public Class InvoicesRepository
    Implements IInvoicesRepository

        '#Region "Private Methods"

        'Private Shared Function GetNull(field As Object) As Object
        '    Return Null.GetNull(field, DBNull.Value)
        'End Function

        '#End Region

        Public Function AddInvoice(invoice As Invoice) As Invoice Implements IInvoicesRepository.AddInvoice
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Invoice) = ctx.GetRepository(Of Invoice)()
                rep.Insert(invoice)
            End Using
            Return invoice
        End Function

        Public Function GetInvoices(portalId As Integer, providerId As Integer, clientId As Integer, productId As Integer, sDate As DateTime, eDate As DateTime, pageNumber As Integer, pageSize As Integer, orderBy As String) As IEnumerable(Of Invoice) Implements IInvoicesRepository.GetInvoices
            Return CBO.FillCollection(Of Invoice)(DataProvider.Instance().GetInvoices(portalId, providerId, clientId, productId, sDate, eDate, pageNumber, pageSize, orderBy))
        End Function

        Public Function GetInvoice(invoiceId As Integer) As Invoice Implements IInvoicesRepository.GetInvoice
            Return CType(CBO.FillObject(DataProvider.Instance().GetInvoice(invoiceId), GetType(Invoice)), Invoice)
        End Function

        Public Sub UpdateInvoice(invoice As Invoice) Implements IInvoicesRepository.UpdateInvoice
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Invoice) = ctx.GetRepository(Of Invoice)()
                rep.Update(invoice)
            End Using
        End Sub

        Public Sub RemoveInvoice(invoiceId As Integer) Implements IInvoicesRepository.RemoveInvoice
            Dim item As Invoice = GetInvoice(invoiceId)
            If item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveInvoice(item)
            End If
        End Sub

        Public Sub RemoveInvoice(invoice As Invoice) Implements IInvoicesRepository.RemoveInvoice
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Invoice) = ctx.GetRepository(Of Invoice)()
                rep.Delete(invoice)
            End Using
        End Sub

        Public Function GetInvoiceItem(invoiceItemId As Integer, invoiceId As Integer) As InvoiceItem Implements IInvoicesRepository.GetInvoiceItem
            Dim invoiceItem As InvoiceItem

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of InvoiceItem) = ctx.GetRepository(Of InvoiceItem)()
                invoiceItem = rep.GetById(Of Int32, Int32)(invoiceItemId, invoiceId)
            End Using
            Return invoiceItem
        End Function

        Public Function GetInvoiceItems(invoiceId As Integer) As IEnumerable(Of InvoiceItem) Implements IInvoicesRepository.GetInvoiceItems
            Return CBO.FillCollection(Of InvoiceItem)(DataProvider.Instance().GetInvoiceItems(invoiceId))
        End Function

        Public Sub RemoveInvoiceItem(invoiceItem As InvoiceItem) Implements IInvoicesRepository.RemoveInvoiceItem
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of InvoiceItem) = ctx.GetRepository(Of InvoiceItem)()
                rep.Delete(invoiceItem)
            End Using
        End Sub

        Public Sub RemoveInvoiceItem1(invoiceItemId As Integer, invoiceId As Integer) Implements IInvoicesRepository.RemoveInvoiceItem1
            Dim item As InvoiceItem = GetInvoiceItem(invoiceItemId, invoiceId)
            RemoveInvoiceItem(item)
        End Sub

        Public Sub RemoveInvoiceItems(invoiceId As Integer) Implements IInvoicesRepository.RemoveInvoiceItems
            Dim invoiceItems As IEnumerable(Of InvoiceItem) = GetInvoiceItems(invoiceId)
            If invoiceItems.Count > 0 Then
                For Each invoiceItem In invoiceItems
                    RemoveInvoiceItem(invoiceItem)
                Next
            End If
        End Sub

        Public Sub UpdateInvoiceItem(invoiceItem As InvoiceItem) Implements IInvoicesRepository.UpdateInvoiceItem
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of InvoiceItem) = ctx.GetRepository(Of InvoiceItem)()
                rep.Update(invoiceItem)
            End Using
        End Sub

        Public Function AddInvoiceItem(invoiceItem As InvoiceItem) As InvoiceItem Implements IInvoicesRepository.AddInvoiceItem
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of InvoiceItem) = ctx.GetRepository(Of InvoiceItem)()
                rep.Insert(invoiceItem)
            End Using
            Return invoiceItem
        End Function

    End Class

End Namespace
