
Namespace RI.Modules.RIStore_Services
    Public Interface IPayConditionsRepository

        ''' <summary>
        ''' Gets list of payment conditions by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="payCondType">Payment Condition Type</param>
        ''' <param name="payCondStart">Payment Condition Start Value</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPayConds(portalId As Integer, payCondType As String, payCondStart As Decimal) As IEnumerable(Of Models.PayCondition)

        ''' <summary>
        ''' Gets payment condition by id
        ''' </summary>
        ''' <param name="payConditionId">Pay Condition ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPayCond(payConditionId As Integer, portalId As Integer) As Models.PayCondition

        ''' <summary>
        ''' Adds payment condition
        ''' </summary>
        ''' <param name="payCondition">PayCondition Model</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddPayCond(payCondition As Models.PayCondition) As Models.PayCondition

        ''' <summary>
        ''' Updates payment condition
        ''' </summary>
        ''' <param name="payCondition">PayCondition Model</param>
        ''' <remarks></remarks>
        Sub UpdatePayCond(payCondition As Models.PayCondition)

        ''' <summary>
        ''' Sets payment condition as deleted
        ''' </summary>
        ''' <param name="payConditionId">Payment Condition ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <remarks></remarks>
        Sub RemovePayCond(payConditionId As Integer, portalId As Integer)

        ''' <summary>
        ''' Removes payment condition by id
        ''' </summary>
        ''' <param name="payCondition">PayCondition Model</param>
        ''' <remarks></remarks>
        Sub RemovePayCond(payCondition As Models.PayCondition)

    End Interface
End Namespace