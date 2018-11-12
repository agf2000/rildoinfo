
Imports System.Collections.Generic
Imports DotNetNuke.Data

Namespace RI.Modules.RIStore_Services
    Public Class InvoicesRepository
        Implements IInvoicesRepository

#Region "Private Methods"

        Private Shared Function GetNull(field As Object) As Object
            Return Null.GetNull(field, DBNull.Value)
        End Function

#End Region

        Public Function AddInvoice(invoice As Models.Invoice) As Models.Invoice Implements IInvoicesRepository.AddInvoice
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Invoice) = ctx.GetRepository(Of Models.Invoice)()
                rep.Insert(invoice)
            End Using
            Return invoice
        End Function

        Public Function GetInvoices(portalId As String, providerId As String, clientId As String, prodId As String, sDate As String, eDate As String, pageNumber As Integer, pageSize As Integer, orderBy As String) As IEnumerable(Of Models.Invoice) Implements IInvoicesRepository.GetInvoices
            Dim invoices As IEnumerable(Of Models.Invoice)

            Using ctx As IDataContext = DataContext.Instance()
                invoices = ctx.ExecuteQuery(Of Models.Invoice)(CommandType.StoredProcedure, "RIS_Invoices_Get", portalId, providerId, clientId, prodId, GetNull(CDate(sDate)), GetNull(CDate(eDate)), pageNumber, pageSize, orderBy)
            End Using
            Return invoices
        End Function

        Public Function GetInvoice(invoiceId As Integer) As Models.Invoice Implements IInvoicesRepository.GetInvoice
            Dim invoice As Models.Invoice

            Using ctx As IDataContext = DataContext.Instance()
                invoice = ctx.ExecuteQuery(Of Models.Invoice)(CommandType.StoredProcedure, "RIS_Invoice_Get", invoiceId)
            End Using
            Return invoice
        End Function

        Public Sub UpdateInvoice(invoice As Models.Invoice) Implements IInvoicesRepository.UpdateInvoice
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Invoice) = ctx.GetRepository(Of Models.Invoice)()
                rep.Update(invoice)
            End Using
        End Sub

        Public Sub RemoveInvoice(invoiceId As Integer) Implements IInvoicesRepository.RemoveInvoice
            Dim _item As Models.Invoice = GetInvoice(invoiceId)
            If _item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveInvoice(_item)
            End If
        End Sub

        Public Sub RemoveInvoice(invoice As Models.Invoice) Implements IInvoicesRepository.RemoveInvoice
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Invoice) = ctx.GetRepository(Of Models.Invoice)()
                rep.Delete(invoice)
            End Using
        End Sub

        Public Function GetInvoiceItem(invoiceItemId As Integer, invoiceId As Integer) As Models.InvoiceItem Implements IInvoicesRepository.GetInvoiceItem
            Dim invoiceItem As Models.InvoiceItem

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.InvoiceItem) = ctx.GetRepository(Of Models.InvoiceItem)()
                invoiceItem = rep.GetById(Of Int32, Int32)(invoiceItemId, invoiceId)
            End Using
            Return invoiceItem
        End Function

        Public Function GetInvoiceItems(invoiceId As Integer) As IEnumerable(Of Models.InvoiceItem) Implements IInvoicesRepository.GetInvoiceItems
            Dim invoiceItems As IEnumerable(Of Models.InvoiceItem)

            Using ctx As IDataContext = DataContext.Instance()
                invoiceItems = ctx.ExecuteQuery(Of Models.InvoiceItem)(CommandType.StoredProcedure, "RIS_InvoiceItems_Get", invoiceId)
            End Using
            Return invoiceItems
        End Function

        Public Sub RemoveInvoiceItem(invoiceItem As Models.InvoiceItem) Implements IInvoicesRepository.RemoveInvoiceItem
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.InvoiceItem) = ctx.GetRepository(Of Models.InvoiceItem)()
                rep.Delete(invoiceItem)
            End Using
        End Sub

        Public Sub RemoveInvoiceItem(invoiceItemId As Integer, invoiceId As Integer) Implements IInvoicesRepository.RemoveInvoiceItem
            Dim _item As Models.InvoiceItem = GetInvoiceItem(invoiceItemId, invoiceId)
            RemoveInvoiceItem(_item)
        End Sub

        Public Sub RemoveInvoiceItems(invoiceId As Integer) Implements IInvoicesRepository.RemoveInvoiceItems
            Dim invoiceItems As List(Of Models.InvoiceItem) = GetInvoiceItems(invoiceId)
            If invoiceItems.Count > 0 Then
                For Each invoiceItem In invoiceItems
                    RemoveInvoiceItem(invoiceItem)
                Next
            End If
        End Sub

        Public Sub UpdateInvoiceItem(invoiceItem As Models.InvoiceItem) Implements IInvoicesRepository.UpdateInvoiceItem
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.InvoiceItem) = ctx.GetRepository(Of Models.InvoiceItem)()
                rep.Update(invoiceItem)
            End Using
        End Sub

        Public Function AddInvoiceItem(invoiceItem As Models.InvoiceItem) As Models.InvoiceItem Implements IInvoicesRepository.AddInvoiceItem
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.InvoiceItem) = ctx.GetRepository(Of Models.InvoiceItem)()
                rep.Insert(invoiceItem)
            End Using
            Return invoiceItem
        End Function

    End Class
End Namespace