
Imports System.Collections.Generic
Imports DotNetNuke.Data

Namespace RI.Modules.RIStore_Services
    Public Class PayFormsRepository
        Implements IPayFormsRepository

        Public Function AddPayForm(payForm As Models.PayForm) As Models.PayForm Implements IPayFormsRepository.AddPayForm
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.PayForm) = ctx.GetRepository(Of Models.PayForm)()
                rep.Insert(payForm)
            End Using
            Return payForm
        End Function

        Public Sub RemovePayForm(payFormId As Integer, portalId As Integer) Implements IPayFormsRepository.RemovePayForm
            Dim _item As Models.PayForm = GetPayForm(payFormId, portalId)
            RemovePayForm(_item)
        End Sub

        Public Sub RemovePayForm(payForm As Models.PayForm) Implements IPayFormsRepository.RemovePayForm
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.PayForm) = ctx.GetRepository(Of Models.PayForm)()
                rep.Delete(payForm)
            End Using
        End Sub

        Public Function GetPayForms(portalId As Integer) As IEnumerable(Of Models.PayForm) Implements IPayFormsRepository.GetPayForms
            Dim payForms As IEnumerable(Of Models.PayForm)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.PayForm) = ctx.GetRepository(Of Models.PayForm)()
                payForms = rep.Get(portalId)
            End Using
            Return payForms
        End Function

        Public Function GetPayForm(payFormId As Integer, portalId As Integer) As Models.PayForm Implements IPayFormsRepository.GetPayForm
            Dim payForm As Models.PayForm

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.PayForm) = ctx.GetRepository(Of Models.PayForm)()
                payForm = rep.GetById(Of Int32, Int32)(payFormId, portalId)
            End Using
            Return payForm
        End Function

        Public Sub UpdatePayForm(payForm As Models.PayForm) Implements IPayFormsRepository.UpdatePayForm
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.PayForm) = ctx.GetRepository(Of Models.PayForm)()
                rep.Update(payForm)
            End Using
        End Sub

    End Class
End Namespace