
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation

Namespace RI.Modules.RIStore_Services
    Public Class PayFormsController
        Inherits DnnApiController

        Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(AccountsController))

        ''' <summary>
        ''' Gets list of payment forms by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetPayForms(portalId As Integer, isDeleted As String) As HttpResponseMessage
            Try
                Dim payForm As IEnumerable(Of Models.PayForm)
                Dim payFormCtrl As New PayFormsRepository

                payForm = payFormCtrl.GetPayForms(PortalController.GetCurrentPortalSettings().PortalId)

                Return Request.CreateResponse(HttpStatusCode.OK, payForm)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets payment form by id
        ''' </summary>
        ''' <param name="pfId">Pay Form ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetPayForm(pfId As Integer) As HttpResponseMessage
            Try
                Dim payForm As Models.PayForm
                Dim payFormCtrl As New PayFormsRepository

                payForm = payFormCtrl.GetPayForm(pfId, PortalController.GetCurrentPortalSettings().PortalId)

                Return Request.CreateResponse(HttpStatusCode.OK, payForm)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ' ''' <summary>
        ' ''' Gets payment form by id
        ' ''' </summary>
        ' ''' <param name="portalId">Portal ID</param>
        ' ''' <param name="pfTitle">Pay Form Title</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        '<DnnAuthorize> _
        '<HttpGet> _
        'Function GetPayFormByTitle(portalId As Integer, pfTitle As String) As HttpResponseMessage
        '    Try
        '        Dim payFormData = RIStore_Business_Controller.GetPayFormByTitle(portalId, pfTitle)
        '        Return Json(payFormData, JsonRequestBehavior.AllowGet)
        '        Return Request.CreateResponse(HttpStatusCode.OK, payCondsData)
        '    Catch ex As Exception
        '        'DnnLog.Error(ex)
        '        Logger.[Error](ex)
        '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        '    End Try
        'End Function

        ''' <summary>
        ''' Adds payment form
        ''' </summary>
        ''' <param name="pfId">Payment Form ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="pfTitle">Payment Form Title</param>
        ''' <param name="uId">Creator ID</param>
        ''' <param name="cd">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function UpdatePayForm(portalId As Integer, pfTitle As String, uId As Integer, cd As Date, Optional ByVal pfId As Integer = -1) As HttpResponseMessage
            Try
                Dim payForm As New Models.PayForm
                Dim payFormCtrl As New PayFormsRepository

                If pfId > 0 Then
                    payForm = payFormCtrl.GetPayForm(pfId, portalId)
                End If

                payForm.CreatedByUser = uId
                payForm.CreatedOnDate = cd
                payForm.ModifiedByUser = uId
                payForm.ModifiedOnDate = cd
                payForm.PayFormTitle = pfTitle
                payForm.PortalId = portalId

                If pfId > 0 Then
                    payFormCtrl.UpdatePayForm(payForm)

                    Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .PayFormId = payForm.PayFormId})
                Else
                    payFormCtrl.AddPayForm(payForm)

                    Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .PayFormId = payForm.PayFormId})
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Removes payment form by id
        ''' </summary>
        ''' <param name="pfId">Payment Form ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpDelete> _
        Function RemovePayForm(pfId As Integer) As HttpResponseMessage
            Try
                Dim payFormCtrl As New PayFormsRepository

                payFormCtrl.RemovePayForm(pfId, PortalController.GetCurrentPortalSettings().PortalId)
                
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

    End Class
End Namespace