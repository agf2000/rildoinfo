
Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
 
Namespace Components.Repositories

    Public Class PayConditionsRepository
    Implements IPayConditionsRepository

        Public Function AddPayCond(payCondition As Components.Models.PayCondition) As Components.Models.PayCondition Implements IPayConditionsRepository.AddPayCond
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.PayCondition) = ctx.GetRepository(Of Components.Models.PayCondition)()
                rep.Insert(payCondition)
            End Using
            Return payCondition
        End Function

        Public Function GetPayCond(payConditionId As Integer, portalId As Integer) As Components.Models.PayCondition Implements IPayConditionsRepository.GetPayCond
            Dim payCondition As Components.Models.PayCondition

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.PayCondition) = ctx.GetRepository(Of Components.Models.PayCondition)()
                payCondition = rep.GetById(Of Int32, Int32)(payConditionId, portalId)
            End Using
            Return payCondition
        End Function

        Public Function GetPayConds(portalId As Integer, payCondType As Integer, payCondStart As Single) As IEnumerable(Of Components.Models.PayCondition) Implements IPayConditionsRepository.GetPayConds
            Return CBO.FillCollection(Of Components.Models.PayCondition)(DataProvider.Instance().GetPayConds(portalId, payCondType, payCondStart))
            'Dim payConds As IEnumerable(Of Models.PayCondition)

            'Using ctx As IDataContext = DataContext.Instance()
            '    payConds = ctx.ExecuteQuery(Of Models.PayCondition)(CommandType.StoredProcedure, "RIW_PayConds_Get", portalId, payCondType, payCondStart).ToList()
            'End Using
            'Return payConds
        End Function

        Public Sub RemovePayCond(payConditionId As Integer, portalId As Integer) Implements IPayConditionsRepository.RemovePayCond
            Dim item As Components.Models.PayCondition = GetPayCond(payConditionId, portalId)
            If item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemovePayCond(item)
            End If
        End Sub

        Public Sub RemovePayCond(payCondition As Components.Models.PayCondition) Implements IPayConditionsRepository.RemovePayCond
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.PayCondition) = ctx.GetRepository(Of Components.Models.PayCondition)()
                rep.Delete(payCondition)
            End Using
        End Sub

        Public Sub UpdatePayCond(payCondition As Components.Models.PayCondition) Implements IPayConditionsRepository.UpdatePayCond
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.PayCondition) = ctx.GetRepository(Of Components.Models.PayCondition)()
                rep.Update(payCondition)
            End Using
        End Sub
    End Class

End Namespace
