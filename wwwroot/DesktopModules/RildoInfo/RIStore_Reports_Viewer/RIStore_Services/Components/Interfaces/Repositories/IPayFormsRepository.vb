
Namespace RI.Modules.RIStore_Services
    Public Interface IPayFormsRepository

        ''' <summary>
        ''' Add pay form
        ''' </summary>
        ''' <param name="payForm"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddPayForm(ByVal payForm As Models.PayForm) As Models.PayForm

        ''' <summary>
        ''' Removes pay form by id
        ''' </summary>
        ''' <param name="payFormId"></param>
        ''' <param name="portalId"></param>
        ''' <remarks></remarks>
        Sub RemovePayForm(ByVal payFormId As Integer, ByVal portalId As Integer)

        ''' <summary>
        ''' Removes pay form
        ''' </summary>
        ''' <param name="payForm"></param>
        ''' <remarks></remarks>
        Sub RemovePayForm(ByVal payForm As Models.PayForm)

        ''' <summary>
        ''' Gets list of pay forms
        ''' </summary>
        ''' <param name="portalId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPayForms(ByVal portalId As Integer) As IEnumerable(Of Models.PayForm)

        ''' <summary>
        ''' Gets pay form
        ''' </summary>
        ''' <param name="payFormId"></param>
        ''' <param name="portalId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPayForm(ByVal payFormId As Integer, ByVal portalId As Integer) As Models.PayForm

        ''' <summary>
        ''' Updates pay form
        ''' </summary>
        ''' <param name="payForm"></param>
        ''' <remarks></remarks>
        Sub UpdatePayForm(ByVal payForm As Models.PayForm)

    End Interface
End Namespace