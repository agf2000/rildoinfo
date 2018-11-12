
Imports System.Collections.Generic
Imports DotNetNuke.Data

Namespace RI.Modules.RIStore_Services
    Public Class PayConditionsRepository
        Implements IPayConditionsRepository

        Public Function AddPayCond(payCondition As Models.PayCondition) As Models.PayCondition Implements IPayConditionsRepository.AddPayCond
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.PayCondition) = ctx.GetRepository(Of Models.PayCondition)()
                rep.Insert(payCondition)
            End Using
            Return payCondition
        End Function

        Public Function GetPayCond(payConditionId As Integer, portalId As Integer) As Models.PayCondition Implements IPayConditionsRepository.GetPayCond
            Dim payCondition As Models.PayCondition

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.PayCondition) = ctx.GetRepository(Of Models.PayCondition)()
                payCondition = rep.GetById(Of Int32, Int32)(payConditionId, portalId)
            End Using
            Return payCondition
        End Function

        Public Function GetPayConds(portalId As Integer, payCondType As String, payCondStart As Decimal) As IEnumerable(Of Models.PayCondition) Implements IPayConditionsRepository.GetPayConds
            Return CBO.FillCollection(Of Models.PayCondition)(DataProvider.Instance().GetPayConds(portalId, payCondType, payCondStart))
        End Function

        Public Sub RemovePayCond(payConditionId As Integer, portalId As Integer) Implements IPayConditionsRepository.RemovePayCond
            Dim _item As Models.PayCondition = GetPayCond(payConditionId, portalId)
            If _item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemovePayCond(_item)
            End If
        End Sub

        Public Sub RemovePayCond(payCondition As Models.PayCondition) Implements IPayConditionsRepository.RemovePayCond
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.PayCondition) = ctx.GetRepository(Of Models.PayCondition)()
                rep.Delete(payCondition)
            End Using
        End Sub

        Public Sub UpdatePayCond(payCondition As Models.PayCondition) Implements IPayConditionsRepository.UpdatePayCond
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.PayCondition) = ctx.GetRepository(Of Models.PayCondition)()
                rep.Update(payCondition)
            End Using
        End Sub
    End Class
End Namespace