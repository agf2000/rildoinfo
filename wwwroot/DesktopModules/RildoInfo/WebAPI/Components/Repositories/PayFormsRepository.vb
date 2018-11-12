
Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
 
Namespace Components.Repositories

    Public Class PayFormsRepository
    Implements IPayFormsRepository

        Public Function AddPayForm(payForm As Components.Models.PayForm) As Components.Models.PayForm Implements IPayFormsRepository.AddPayForm
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.PayForm) = ctx.GetRepository(Of Components.Models.PayForm)()
                rep.Insert(payForm)
            End Using
            Return payForm
        End Function

        Public Sub RemovePayForm(payFormId As Integer, portalId As Integer) Implements IPayFormsRepository.RemovePayForm
            Dim item As Components.Models.PayForm = GetPayForm(payFormId, portalId)
            RemovePayForm(item)
        End Sub

        Public Sub RemovePayForm(payForm As Components.Models.PayForm) Implements IPayFormsRepository.RemovePayForm
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.PayForm) = ctx.GetRepository(Of Components.Models.PayForm)()
                rep.Delete(payForm)
            End Using
        End Sub

        Public Function GetPayForms(portalId As Integer, isDeleted As String) As IEnumerable(Of Components.Models.PayForm) Implements IPayFormsRepository.GetPayForms
            Return CBO.FillCollection(Of Components.Models.PayForm)(DataProvider.Instance().GetPayForms(portalId, isDeleted))
            'Dim payForms As IEnumerable(Of Models.PayForm)

            'Using ctx As IDataContext = DataContext.Instance()
            '    Dim rep As IRepository(Of Models.PayForm) = ctx.GetRepository(Of Models.PayForm)()
            '    payForms = rep.Get(portalId)
            'End Using
            'Return payForms
        End Function

        Public Function GetPayForm(payFormId As Integer, portalId As Integer) As Components.Models.PayForm Implements IPayFormsRepository.GetPayForm
            Dim payForm As Components.Models.PayForm

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.PayForm) = ctx.GetRepository(Of Components.Models.PayForm)()
                payForm = rep.GetById(Of Int32, Int32)(payFormId, portalId)
            End Using
            Return payForm
        End Function

        Public Sub UpdatePayForm(payForm As Components.Models.PayForm) Implements IPayFormsRepository.UpdatePayForm
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.PayForm) = ctx.GetRepository(Of Components.Models.PayForm)()
                rep.Update(payForm)
            End Using
        End Sub

    End Class

End Namespace
