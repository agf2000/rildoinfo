Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Web.Api
Imports System.Net
Imports System.Net.Http
Imports RIW.Modules.WebAPI.Components.Repositories

Public Class UnitTypesController
    Inherits DnnApiController

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(UnitTypesController))

    ''' <summary>
    ''' Gets a list of unit types by portal id
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="isDeleted">Optional IsDeleted</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetUnitTypes(Optional portalId As Integer = 0, Optional isDeleted As String = "") As HttpResponseMessage
        Try
            Dim unitTypesDataCtrl As New UnitTypesRepository

            Dim unitTypesData = unitTypesDataCtrl.GetUnitTypes(portalId, isDeleted)

            Return Request.CreateResponse(HttpStatusCode.OK, unitTypesData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
            Dim unitTypeDataCtrl As New UnitTypesRepository

            Dim unitTypeData = unitTypeDataCtrl.GetUnitType(unitTypeId, PortalController.Instance.GetCurrentPortalSettings().PortalId)

            Return Request.CreateResponse(HttpStatusCode.OK, unitTypeData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets unitType
    ''' </summary>
    ''' <param name="unitTypeTitle">Unit Type Title</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function getUnitType(unitTypeAbbv As String) As HttpResponseMessage
        Try
            Dim unitTypeDataCtrl As New UnitTypesRepository

            Dim unitTypeData = unitTypeDataCtrl.GetUnitType(unitTypeAbbv, PortalController.Instance.GetCurrentPortalSettings().PortalId)

            Return Request.CreateResponse(HttpStatusCode.OK, unitTypeData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    Function updateUnitType(dto As Components.Models.UnitType) As HttpResponseMessage
        Try
            Dim unitType As New Components.Models.UnitType
            Dim unitTypeCtrl As New UnitTypesRepository

            If dto.UnitTypeId > 0 Then
                unitType = unitTypeCtrl.GetUnitType(dto.UnitTypeId, dto.PortalId)
            End If

            unitType.CreatedByUser = dto.CreatedByUser
            unitType.CreatedOnDate = dto.CreatedOnDate
            unitType.ModifiedByUser = dto.ModifiedByUser
            unitType.ModifiedOnDate = dto.ModifiedOnDate
            unitType.IsDeleted = dto.IsDeleted
            unitType.PortalId = dto.PortalId
            unitType.UnitTypeAbbv = dto.UnitTypeAbbv
            unitType.UnitTypeTitle = dto.UnitTypeTitle

            If dto.UnitTypeId > 0 Then
                unitTypeCtrl.UpdateUnitType(unitType)
            Else
                unitTypeCtrl.AddUnitType(unitType)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .UnitTypeId = unitType.UnitTypeId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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

            unitTypeCtrl.RemoveUnitType(unitTypeId, PortalController.Instance.GetCurrentPortalSettings().PortalId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

End Class
