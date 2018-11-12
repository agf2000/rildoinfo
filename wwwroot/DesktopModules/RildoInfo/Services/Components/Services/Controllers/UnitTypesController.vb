
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Security.Membership
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Web.Api
Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Net.Http

Public Class UnitTypesController
    Inherits DnnApiController

    Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(AccountsController))

    ''' <summary>
    ''' Gets a list of unit types by portal id
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="isDeleted">Optional IsDeleted</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function getUnitTypes(Optional portalId As Integer = 0, Optional isDeleted As String = "") As HttpResponseMessage
        Try
            Dim unitTypesData As IEnumerable(Of Models.UnitType)
            Dim unitTypesDataCtrl As New UnitTypesRepository

            unitTypesData = unitTypesDataCtrl.getUnitTypes(portalId, isDeleted)

            Return Request.CreateResponse(HttpStatusCode.OK, unitTypesData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets unitType
    ''' </summary>
    ''' <param name="unitTypeId">UnitType ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function getUnitType(unitTypeId As Integer) As HttpResponseMessage
        Try
            Dim unitTypeData As New Models.UnitType
            Dim unitTypeDataCtrl As New UnitTypesRepository

            unitTypeData = unitTypeDataCtrl.getUnitType(unitTypeId, PortalController.GetCurrentPortalSettings().PortalId)

            Return Request.CreateResponse(HttpStatusCode.OK, unitTypeData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds a new unitType
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function updateUnitType(dto As Models.UnitType) As HttpResponseMessage
        Try
            Dim unitType As New Models.UnitType
            Dim unitTypeCtrl As New UnitTypesRepository

            If dto.UnitTypeId > 0 Then
                unitType = unitTypeCtrl.getUnitType(dto.UnitTypeId, dto.PortalId)
            End If

            unitType.CreatedByUser = dto.CreatedByUser
            unitType.CreatedOnDate = dto.CreatedOnDate
            unitType.IsDeleted = dto.IsDeleted
            unitType.PortalId = dto.PortalId
            unitType.UnitTypeTitle = dto.UnitTypeTitle

            If dto.UnitTypeId > 0 Then
                unitTypeCtrl.updateUnitType(unitType)
            Else
                unitTypeCtrl.addUnitType(unitType)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .UnitTypeId = unitType.UnitTypeId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes unitType
    ''' </summary>
    ''' <param name="unitTypeId">UnitType ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpDelete> _
    Function removeUnitType(unitTypeId As Integer) As HttpResponseMessage
        Try
            Dim unitTypeCtrl As New UnitTypesRepository

            unitTypeCtrl.removeUnitType(unitTypeId, PortalController.GetCurrentPortalSettings().PortalId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

End Class
