
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Web.Api
Imports RIW.Modules.WebAPI.Components.Models
Imports RIW.Modules.WebAPI.Components.Repositories
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Web.Hosting
Imports RIW.Modules.Common.Utilities
Imports DotNetNuke.Services.FileSystem
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Xml

Public Class EstimatesController
    'Inherits DnnApiControllerWithHub(Of EstimatesHub)
    Inherits DnnApiControllerWithHub
    'Inherits DnnApiController

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(EstimatesController))

    ''' <summary>
    ''' Adds a quick sales
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function AddCashier(dto As Cashier) As HttpResponseMessage
        Dim fileDir = New System.IO.DirectoryInfo(PortalSettings.HomeDirectoryMapPath & "davs\")
        Try
            Dim estimate As New Estimate
            Dim products As New List(Of EstimateItem)
            Dim estimateCtrl As New EstimatesRepository
            Dim cashier As New Cashier
            'Dim cashierCtrl As New CashiersRepository

            cashier.CreatedByUser = dto.CreatedByUser
            cashier.CreatedOnDate = dto.CreatedOnDate
            cashier.EstimateId = dto.EstimateId
            cashier.PortalId = dto.PortalId
            cashier.TotalBank = dto.TotalBank
            cashier.TotalCard = dto.TotalCard
            cashier.TotalDebit = dto.TotalDebit
            cashier.TotalCash = dto.TotalCash
            cashier.TotalCheck = dto.TotalCheck

            'cashierCtrl.AddCashier(cashier)

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, True)

            estimate.StatusId = 10
            estimate.SaleDate = Today()
            estimate.CashAmount = cashier.TotalCash
            estimate.ChequeAmount = cashier.TotalCheck
            estimate.CardAmount = cashier.TotalCard
            estimate.DebitAmount = cashier.TotalDebit
            estimate.CreditAmount = cashier.TotalBank
            estimate.ModifiedByUser = dto.CreatedByUser
            estimate.ModifiedOnDate = dto.CreatedOnDate

            estimateCtrl.UpdateEstimate(estimate)

            'Creates a new DAV
            products = CreateDav(fileDir, dto, estimate, estimateCtrl, products)

            Dim estimateHistory As New EstimateHistory

            estimateHistory.EstimateId = dto.estimateId
            estimateHistory.HistoryText = "Venda Concluida."
            estimateHistory.Locked = True
            estimateHistory.CreatedByUser = dto.CreatedByUser
            estimateHistory.CreatedOnDate = dto.CreatedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds a quick estimate item
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function AddEstimate(dto As Estimate) As HttpResponseMessage
        Try
            Dim estimateHistory As New EstimateHistory
            Dim estimate As New Estimate
            Dim estimateCtrl As New EstimatesRepository

            estimate.PortalId = dto.PortalId
            estimate.PersonId = dto.PersonId
            estimate.EstimateTitle = dto.EstimateTitle
            estimate.SalesRep = dto.SalesRep
            estimate.ViewPrice = dto.ViewPrice
            estimate.TotalAmount = dto.TotalAmount
            estimate.CreatedByUser = dto.CreatedByUser
            estimate.CreatedOnDate = dto.CreatedOnDate
            estimate.StatusId = 1

            estimateCtrl.AddEstimate(estimate)

            estimateHistory.EstimateId = estimate.EstimateId
            estimateHistory.HistoryText = "<p>Orçamento Inicializado</p>"
            estimateHistory.Locked = True
            estimateHistory.CreatedByUser = dto.CreatedByUser
            estimateHistory.CreatedOnDate = dto.CreatedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            Return Request.CreateResponse(HttpStatusCode.OK, estimate)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds a quick estimate item
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function AddEstimateItem(dto As EstimateItemsRequest) As HttpResponseMessage
        Try
            Dim estimateHistory As New EstimateHistory
            Dim estimate As New Estimate
            Dim estimateItem As New EstimateItem
            'Dim productsCtrl As New ProductsRepository
            Dim estimateCtrl As New EstimatesRepository
            Dim personCtrl As New PeopleRepository

            'Dim personCtrl As New PeopleRepository
            'dto.PersonId = personCtrl.GetPerson(dto.PersonId, dto.PortalId, dto.UserId).PersonId

            If dto.EstimateId > 0 Then
                estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, True)
            Else
                estimate.PortalId = dto.PortalId
                estimate.PersonId = If(dto.PersonId > 0, dto.PersonId, personCtrl.GetPerson(-1, dto.PortalId, dto.UserId).PersonId)
                estimate.EstimateTitle = dto.EstimateTitle
                estimate.Guid = dto.Guid
                estimate.SalesRep = dto.SalesRep
                estimate.ViewPrice = dto.ViewPrice
                estimate.Discount = 0
                estimate.NumDav = 0
                estimate.TotalAmount = dto.TotalAmount
                estimate.CreatedByUser = dto.CreatedByUser
                estimate.CreatedOnDate = dto.CreatedOnDate
                estimate.Comment = dto.Comment
                estimate.StatusId = dto.StatusId

                estimateCtrl.AddEstimate(estimate)

                estimateHistory.EstimateId = estimate.EstimateId
                estimateHistory.HistoryText = "<p>Orçamento Inicializado</p>"
                estimateHistory.Locked = True
                estimateHistory.CreatedByUser = dto.CreatedByUser
                estimateHistory.CreatedOnDate = dto.CreatedOnDate

                estimateCtrl.AddEstimateHistory(estimateHistory)
            End If

            Dim totalAmount = 0.00

            If dto.EstimateItems.Count > 0 Then

                For Each item In dto.EstimateItems

                    estimateItem.EstimateId = estimate.EstimateId
                    estimateItem.ProductId = item.ProductId
                    estimateItem.ProductQty = item.ProductQty
                    estimateItem.ProductEstimateOriginalPrice = item.ProductEstimateOriginalPrice
                    estimateItem.ProductEstimatePrice = item.ProductEstimatePrice
                    estimateItem.CreatedByUser = item.CreatedByUser
                    estimateItem.CreatedOnDate = item.CreatedOnDate
                    estimateCtrl.AddEstimateItem(estimateItem)

                    totalAmount = totalAmount + (item.ProductEstimateOriginalPrice * item.ProductQty)
                Next

            End If

            If estimate.EstimateId Then
                estimate.Comment = dto.Comment
                estimate.TotalAmount = totalAmount
                estimate.ModifiedByUser = dto.ModifiedByUser
                estimate.ModifiedOnDate = dto.ModifiedOnDate
                estimateCtrl.UpdateEstimate(estimate)

                estimate.EstimateItems = estimateCtrl.GetEstimateItems(dto.PortalId, estimate.EstimateId, "pt-BR")
            End If

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushEstimate()
                'Hub.Clients.AllExcept(dto.ConnId).pushEstimate()

                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .Estimate = estimate})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds a quick estimate item
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function AddEstimateItems(dto As EstimateItemsRequest) As HttpResponseMessage
        Try
            Dim estimateHistory As New EstimateHistory
            Dim estimate As New Estimate
            Dim estimateItem As New EstimateItem
            Dim estimateCtrl As New EstimatesRepository

            If dto.EstimateId > 0 Then
                estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, True)
            Else
                estimate.ViewPrice = dto.ViewPrice
                estimate.PersonId = dto.PersonId
                estimate.SalesRep = dto.SalesRep
                estimate.TotalAmount = dto.TotalAmount
                estimate.CreatedByUser = dto.CreatedByUser
                estimate.CreatedOnDate = dto.CreatedOnDate
                estimate.ModifiedByUser = dto.ModifiedByUser
                estimate.ModifiedOnDate = dto.ModifiedOnDate
                estimateCtrl.AddEstimate(estimate)
            End If

            Dim totalAmount = 0.00

            For Each item In dto.EstimateItems

                estimateItem.EstimateId = estimate.EstimateId
                estimateItem.ProductId = item.ProductId
                estimateItem.ProductQty = item.ProductQty
                estimateItem.ProductEstimateOriginalPrice = item.ProductEstimateOriginalPrice
                estimateItem.ProductEstimatePrice = item.ProductEstimatePrice
                estimateItem.CreatedByUser = item.CreatedByUser
                estimateItem.CreatedOnDate = item.CreatedOnDate
                estimateItem.ModifiedByUser = item.ModifiedByUser
                estimateItem.ModifiedOnDate = item.ModifiedOnDate
                estimateCtrl.AddEstimateItem(estimateItem)

                totalAmount = totalAmount + item.ProductEstimateOriginalPrice

                estimateHistory.EstimateId = item.EstimateId
                estimateHistory.HistoryText = String.Format("<p>Item ""{0}"" inserido (Qde: {1}).</p>", item.ProductName, item.ProductQty)
                estimateHistory.Locked = True
                estimateHistory.CreatedByUser = item.CreatedByUser
                estimateHistory.CreatedOnDate = item.CreatedOnDate

                estimateCtrl.AddEstimateHistory(estimateHistory)

            Next

            If estimate.EstimateId > 0 Then
                estimate.TotalAmount = totalAmount
                estimate.ModifiedByUser = dto.ModifiedByUser
                estimate.ModifiedOnDate = dto.ModifiedOnDate
                estimateCtrl.UpdateEstimate(estimate)
            End If

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushEstimate()
                'Hub.Clients.AllExcept(dto.ConnId).pushEstimate()

                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .Estimate = estimate})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds estimate history
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function AddHistory(dto As EstimateHistory) As HttpResponseMessage
        Try
            Dim estimateHistory As New EstimateHistory
            Dim estimateCtrl As New EstimatesRepository

            estimateHistory.EstimateId = dto.EstimateId
            estimateHistory.HistoryText = dto.HistoryText
            estimateHistory.Locked = dto.Locked
            estimateHistory.CreatedByUser = dto.CreatedByUser
            estimateHistory.CreatedOnDate = dto.CreatedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            Dim userInfo = UserController.GetUserById(PortalController.Instance.GetCurrentPortalSettings().PortalId, estimateHistory.CreatedByUser)
            estimateHistory.DisplayName = userInfo.DisplayName
            If userInfo.Profile.Photo IsNot Nothing Then
                If userInfo.Profile.Photo.Length > 2 Then
                    estimateHistory.Avatar = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(userInfo).FolderPath
                    estimateHistory.Avatar = estimateHistory.Avatar & DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(userInfo.Profile.Photo).FileName
                End If
            End If

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .EstimateHistory = estimateHistory})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates estimate pay form and condition
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function CancelEstimatePayCond(dto As Estimate) As HttpResponseMessage
        Try
            Dim estimateHistory As New EstimateHistory
            Dim estimate As New Estimate
            Dim estimateCtrl As New EstimatesRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, True)

            estimate.PayCondId = dto.PayCondId
            estimate.PayCondType = dto.PayCondType
            estimate.PayCondN = dto.PayCondN
            estimate.PayCondPerc = dto.PayCondPerc
            estimate.PayCondIn = dto.PayCondIn
            estimate.PayCondInst = dto.PayCondInst
            estimate.PayCondInterval = dto.PayCondInterval
            estimate.TotalPayments = dto.TotalPayments
            estimate.TotalPayCond = dto.TotalPayCond
            estimate.ModifiedByUser = dto.ModifiedByUser
            estimate.ModifiedOnDate = dto.ModifiedOnDate

            estimateCtrl.UpdateEstimate(estimate)

            estimateHistory.EstimateId = estimate.EstimateId
            estimateHistory.HistoryText = "<p>Condição de pagamento cancelada.</p>"
            estimateHistory.Locked = True
            estimateHistory.CreatedByUser = 0
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()

                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates estimate pay form and condition
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function CloseEstimate(dto As Estimate) As HttpResponseMessage
        Try
            Dim estimateHistory As New EstimateHistory
            Dim estimate As New Estimate
            Dim estimateCtrl As New EstimatesRepository
            Dim statusCtrl As New StatusesRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, True)

            estimate.StatusId = 5
            estimate.ModifiedByUser = dto.ModifiedByUser
            estimate.ModifiedOnDate = dto.ModifiedOnDate

            estimateCtrl.UpdateEstimate(estimate)

            estimateHistory.EstimateId = estimate.EstimateId
            estimateHistory.HistoryText = "<p>Status do orçamento atualizado como " & statusCtrl.GetStatus1(5, dto.PortalId).StatusTitle & ".</p>"
            estimateHistory.Locked = False
            estimateHistory.CreatedByUser = dto.ModifiedByUser
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            Dim product As New Product
            Dim productCtrl As New ProductsRepository

            For Each item In dto.Products

                product = productCtrl.GetProduct(item.ProductId, "pt-BR")

                Dim oldStock = product.QtyStockSet

                product.QtyStockSet = oldStock - item.QtyStockSet
                product.ModifiedByUser = item.ModifiedByUser
                product.ModifiedOnDate = item.ModifiedOnDate

                productCtrl.UpdateProduct(product)

            Next

            estimatesHub.Value.Clients.AllExcept(dto.ConnId).CloseEstimate(estimate.Guid)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Creates a new DAV
    ''' </summary>
    ''' <param name="fileDir"></param>
    ''' <param name="dto"></param>
    ''' <param name="estimate"></param>
    ''' <param name="estimateCtrl"></param>
    ''' <param name="products"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateDav(fileDir As DirectoryInfo, dto As Cashier, estimate As Estimate, estimateCtrl As EstimatesRepository, products As List(Of EstimateItem)) As List(Of EstimateItem)
        Dim path As String = fileDir.ToString
        'If fileDir.Exists = False Then
        '    'Add File folder
        '    DotNetNuke.Services.FileSystem.FolderManager.Instance().AddFolder(dto.PortalId, "davs/")
        'End If
        Common.Utilities.CreateDir(PortalController.Instance.GetCurrentPortalSettings(), "Davs")

        Dim filePath = String.Format("{0}DV{1}.TXT", path, (estimate.EstimateId.ToString().PadLeft(6, "0")))

        'If System.IO.File.Exists(filePath) Then
        '    System.IO.File.Delete(filePath)
        'End If

        'estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, False)

        Dim saveString As New StringBuilder
        saveString.Append("0") 'N Tipo
        saveString.Append("RIW.COM.BR".PadRight(15)) 'C Empresa
        saveString.Append("ARQUIVO DE IMPORTACAO DE VENDA AUXILIAR".PadRight(50)) 'C Nome do documento
        saveString.Append(Now.ToString("ddMMyyyy")) 'D Data de criação do arquivo
        saveString.Append(Now.ToString("hhmmss")) 'D Hora da criação do arquivo
        saveString.Append(String.Format("DV{0}.TXT", estimate.EstimateId.ToString().PadLeft(6, "0"))) 'C Nome do arquivo
        saveString.AppendLine()
        saveString.Append("1") 'D Tipo
        saveString.Append(estimate.SalesRep.ToString().PadLeft(8, "0")) 'C Código do vendedor
        saveString.Append(estimate.Discount.ToString("#.00").Replace(",", "").PadLeft(9, "0")) 'N Desconto geral
        saveString.Append("".PadLeft(9, "0")) 'N Acréscimo
        saveString.Append("".PadRight(50, "-")) 'N Observação
        saveString.Append(estimate.PersonId.ToString().PadLeft(8, "0")) 'C Código do cliente
        saveString.Append(estimate.PayCondId.ToString().PadLeft(3, "0")) 'N Código da condição de pagamento
        saveString.Append(estimate.EstimateId.ToString().PadLeft(8, "0")) 'N Número do DAV
        saveString.Append(Now.ToString("ddMMyyyy")) 'D Data de criação do arquivo
        saveString.Append("VENDA".PadRight(30)) 'N Título "VENDA" 
        saveString.Append("".PadRight(12)) 'N Número da transação do cartão de crédito
        saveString.AppendLine()

        products = estimateCtrl.GetEstimateItems(dto.PortalId, estimate.EstimateId, "pt-BR")

        Dim totalOfItems = 0.0
        Dim xChange = 0.0

        For Each item In products
            saveString.Append("2") 'N Tipo
            saveString.Append(item.ProductId.ToString().PadLeft(6, "0"))
            saveString.Append(item.ProductQty.ToString("#.000").Replace(",", "").PadLeft(9, "0"))
            saveString.Append(item.ProductEstimatePrice.ToString("#.000").Replace(",", "").PadLeft(9, "0"))
            Dim ref As String = item.Barcode.ToString()
            If Not ref <> "" Then
                If item.ProductRef <> "" Then
                    ref = item.ProductRef
                Else
                    ref = item.ProductId.ToString()
                End If
            End If
            saveString.Append(ref.PadRight(20))
            saveString.Append(item.ProductName.PadRight(42))
            saveString.AppendLine()
            totalOfItems = totalOfItems + item.ProductEstimatePrice
        Next

        If dto.TotalCash > totalOfItems Then
            xChange = dto.TotalCash - totalOfItems
        End If

        saveString.Append("3") 'N Tipo Formas de pagamento
        If dto.TotalCash > 0 Then
            saveString.Append("1".PadLeft(2, "0")) 'N Código da forma de pagamento
            'saveString.Append((dto.TotalBank + dto.TotalCard + dto.TotalCash + dto.TotalCheck).ToString("#.00").Replace(",", "").PadLeft(9, "0")) 'N valor do pagamento
            saveString.Append((dto.TotalCash - xChange).ToString("#.00").Replace(",", "").PadLeft(9, "0")) 'N valor do pagamento
            saveString.Append("0000") 'N Código da administradora do cartão
        ElseIf dto.TotalCard > 0 Then
            saveString.Append("2".PadLeft(2, "0")) 'N Código da forma de pagamento
            saveString.Append((dto.TotalCard).ToString("#.00").Replace(",", "").PadLeft(9, "0")) 'N valor do pagamento
            saveString.Append("0000") 'N Código da administradora do cartão
        ElseIf dto.TotalBank > 0 Then
            saveString.Append("3".PadLeft(2, "0")) 'N Código da forma de pagamento
            saveString.Append((dto.TotalBank).ToString("#.00").Replace(",", "").PadLeft(9, "0")) 'N valor do pagamento
            saveString.Append("0000") 'N Código da administradora do cartão
        Else
            saveString.Append("4".PadLeft(2, "0")) 'N Código da forma de pagamento
            saveString.Append((dto.TotalCheck).ToString("#.00").Replace(",", "").PadLeft(9, "0")) 'N valor do pagamento
            saveString.Append("0000") 'N Código da administradora do cartão
        End If

        'Dim info = New UTF8Encoding(True).GetBytes(saveString.ToString())

        If System.IO.File.Exists(filePath) Then
            System.IO.File.SetAttributes(filePath, FileAttributes.Normal)
            System.IO.File.Delete(filePath)
        End If

        'Using fs = File.Create(filePath)
        '    fs.Write(info, 0, info.Length)
        'End Using

        Using mySw = New StreamWriter(filePath, True, System.Text.Encoding.Default)
            mySw.AutoFlush = True
            mySw.Write(saveString)
        End Using
        Return products
    End Function

    ''' <summary>
    ''' Deletes an estimate
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function DeleteEstimate(dto As Estimate) As HttpResponseMessage
        Try
            Dim estimateHistory As New EstimateHistory
            Dim estimate As New Estimate
            Dim estimateCtrl As New EstimatesRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, True)

            estimate.IsDeleted = dto.IsDeleted
            estimate.ModifiedByUser = dto.ModifiedByUser
            estimate.ModifiedOnDate = dto.ModifiedOnDate

            estimateCtrl.DeleteEstimate(estimate)

            Dim historyText = ""
            If dto.IsDeleted Then
                historyText = "<p>Orçamento desativado.</p>"
            Else
                historyText = "<p>Orçamento ativado.</p>"
            End If

            estimateHistory.EstimateId = estimate.EstimateId
            estimateHistory.HistoryText = historyText
            estimateHistory.Locked = True
            estimateHistory.CreatedByUser = 0
            estimateHistory.CreatedOnDate = dto.CreatedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Downloads estimate txt
    ''' </summary>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function DownloadEstimateHtml(dto As EstimateTxt) As HttpResponseMessage
        Try
            Dim payCondCtrl As New PayConditionsRepository
            Dim personCtrl As New PeopleRepository
            Dim estimateCtrl As New EstimatesRepository
            Dim estimateInfo = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, True)
            Dim salesRepInfo = UserController.GetUserById(dto.PortalId, estimateInfo.SalesRep)

            Dim salesPhone = String.Empty
            salesPhone = salesRepInfo.Profile.Telephone

            Dim myPortalSettings = PortalController.Instance.GetPortalSettings(dto.PortalId)

            Dim settingCtrl As New SettingsRepository
            Dim settingsDictionay = settingCtrl.GetSettings(dto.PortalId)

            Dim body = "<!DOCTYPE html>"
            body += "<html xmlns='http://www.w3.org/1999/xhtml'>"
            body += "<head>"
            body += String.Format("<title>Orcamento n {0}</title>", dto.EstimateId.ToString())
            body += "<style>body { font-size: 12px; padding: 0x; margin: 0px; } .dashTd table td { padding: 1px; } button { display: block; margin: 5px; } @media print { button { display: none; } }</style>"
            body += "</head>"
            body += "<body style='font-size: 12px;'>"

            body += "<button onclick='window.print()'>Imprimir</button>"

            body += Now.ToString("dd/MM/yy H:mm")
            body += String.Format("<br />ORCAMENTO n {0}<br />", dto.EstimateId.ToString())

            Dim clientInfo = personCtrl.GetPerson(estimateInfo.PersonId, dto.PortalId, Null.NullInteger)
            body += String.Format("<br />{0}<br />", ReplaceAccentletters(clientInfo.DisplayName))

            If Not CInt(settingsDictionay("mainConsumerId")) = estimateInfo.PersonId Then
                Dim clientStr = If(clientInfo.Street <> "", String.Format("{0} {1}", clientInfo.Street, clientInfo.Unit), "")
                clientStr += CStr(If(clientInfo.District <> "", String.Format("<br />{0}", clientInfo.District), ""))
                clientStr += CStr(If(clientInfo.City <> "", String.Format("<br />{0} {1}", clientInfo.City, clientInfo.Region), ""))
                clientStr += CStr(If(clientInfo.PostalCode <> "", String.Format("<br />CEP: {0}", ZipMask(clientInfo.PostalCode)), ""))
                clientStr += CStr(If(clientInfo.Telephone <> "", String.Format("<br />Tel: {0}<br />", PhoneMask(clientInfo.Telephone)), ""))
                clientStr = ReplaceAccentletters(clientStr)

                body += clientStr
            End If

            body += "<br />&nbsp;Descricao<br />"
            body += "<table style='font-size: 12px;'>"
            body += "<tbody>"
            body += "<tr>"
            body += "<td style='width: 35px;'>Qde x</td>"
            body += "<td style='width: 60px;'>Valor U. =</td>"
            body += "<td>Total</td>"
            body += "</tr>"
            body += "<tr>"
            body += "<td colspan='3' class='dashTd'>------------------------------</td>"
            body += "</tr>"
            body += "</tbody>"
            body += "</table>"

            Dim estimateDetailInfo = estimateCtrl.GetEstimateItems(dto.PortalId, dto.EstimateId, "pt-BR")

            Dim itemRef = String.Empty
            Dim itemIndex = 0

            Dim totalItems As Single = 0.0
            Dim totalBeforeDiscount As Single = 0.0

            body += "<table style='font-size: 12px;'>"
            body += "<tbody>"

            For Each item In estimateDetailInfo

                itemIndex += 1

                Dim itemName = "0"
                If item.ProductName.Length > 28 Then
                    itemName = item.ProductName.ToLower().Remove(28)
                Else
                    itemName = item.ProductName.ToLower()
                End If

                Dim itemCode = ""
                If item.Barcode <> "" Then
                    itemCode = "CB: " & item.Barcode
                ElseIf item.ProductRef <> "" Then
                    itemCode = "REF: " & item.ProductRef.ToLower()
                End If

                totalBeforeDiscount = totalBeforeDiscount + (item.ProductEstimatePrice * item.ProductQty)

                Dim totalLiquid As Single = (item.ProductEstimatePrice - (item.ProductEstimatePrice / 100 * item.ProductDiscount)) * item.ProductQty

                body += "<tr>"
                body += String.Format("<td colspan='3'>{0}</td>", ReplaceAccentletters(itemName))
                body += "</tr>"
                body += "<tr>"
                body += String.Format("<td>{0} x</td>", item.ProductQty.ToString())
                body += String.Format("<td>{0} =</td>", FormatNumber(item.ProductEstimatePrice))
                body += String.Format("<td>{0}</td>", FormatNumber(totalLiquid))
                body += "</tr>"

                totalItems = totalItems + totalLiquid

            Next

            body += "</tbody>"
            body += "</table>"

            Dim discountValue = totalBeforeDiscount - totalItems
            Dim total As Single = totalBeforeDiscount ' - DiscountValue

            If estimateInfo.Discount > 0 Then
                discountValue = discountValue + (totalItems * estimateInfo.Discount / 100)
            End If

            total = total - discountValue

            If discountValue > 0.01 Then
                discountValue = String.Format("{0:C}", Math.Floor(discountValue * 100) / 100)
            End If

            body += String.Format("<br />SUBTOTAL: {0}{1}<br />TOTAL: {2}<br />", FormatNumber(totalBeforeDiscount),
                                  If(discountValue > 0, "<br />DESCONTO: " & FormatNumber(discountValue), ""),
                                  FormatNumber(total))

            If estimateInfo.PayCondType <> "" Then

                body += "<br />Condicao de Pagamento:"
                body += String.Format("<br />Forma: {0}", ReplaceAccentletters(estimateInfo.PayCondType))
                body += String.Format("<br />Entrada: {0}", estimateInfo.PayCondIn)
                body += String.Format("<br />Parcela: {0} x {1}", estimateInfo.PayCondN.ToString(), estimateInfo.PayCondInst)
                body += String.Format("<br />Total: {0}", estimateInfo.TotalPayments)

            End If

            body += String.Format("<br />{0}", ReplaceAccentletters(PortalSettings.PortalName))
            body += String.Format("<br />{0}", PortalSettings.PortalAlias.HTTPAlias)
            body += String.Format("<br />{0} {1}", ReplaceAccentletters(settingsDictionay("storeAddress")), ReplaceAccentletters(settingsDictionay("storeUnit")))
            body += String.Format("<br />{0}", ReplaceAccentletters(settingsDictionay("storeDistrict")))
            body += String.Format("<br />{0}", ReplaceAccentletters(settingsDictionay("storeCity")), ReplaceAccentletters(settingsDictionay("storeRegion")))
            body += String.Format("<br />CEP: {0}", ReplaceAccentletters(settingsDictionay("storePostalCode")))
            body += String.Format("<br />Fone: {0}", If(salesPhone IsNot Nothing, PhoneMask(salesPhone), PhoneMask(Left(settingsDictionay("storePhone1").Replace("voz=", ""), 10))))
            body += String.Format("<br />{0}", If(salesRepInfo.Email.Length > 0, salesRepInfo.Email, settingsDictionay("storeEmail")))

            If settingsDictionay("estimateTerm") <> "" Then
                body += String.Format("<br /><br />Observacoes Importantes:<br />{0}<br />{1}",
                                        ReplaceAccentletters(settingsDictionay("estimateTerm")), ReplaceAccentletters(estimateInfo.Comment))
            End If

            body += "</body>"
            body += "</html>"

            CreateDir(PortalController.Instance.GetCurrentPortalSettings(), "downloads")

            Dim destinationPath = FolderManager.Instance.GetFolder(dto.PortalId, "downloads")

            Dim filePath = String.Format("{0}orcamento_{1}.html", destinationPath.PhysicalPath, estimateInfo.EstimateId.ToString()) ', Replace(Now.ToString("dd-MM-yyyy HH"), ":", "-").Replace("/", "-").Replace(" ", "_"))

            If System.IO.File.Exists(filePath) Then
                System.IO.File.Delete(filePath)
            End If

            Using sw As New StreamWriter(filePath, True)
                sw.WriteLine(body)
                sw.Close()
            End Using

            'Return response
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Downloads estimate txt receipt
    ''' </summary>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function DownloadEstimateHtmlReceipt(dto As EstimateReceiptTxt) As HttpResponseMessage
        Try
            Dim payCondCtrl As New PayConditionsRepository
            Dim personCtrl As New PeopleRepository
            Dim estimateCtrl As New EstimatesRepository
            Dim estimateInfo = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, True)
            Dim salesRepInfo = UserController.GetUserById(dto.PortalId, estimateInfo.SalesRep)

            Dim salesPhone = String.Empty
            salesPhone = salesRepInfo.Profile.Telephone

            Dim myPortalSettings = PortalController.Instance.GetPortalSettings(dto.PortalId)

            Dim settingCtrl As New SettingsRepository
            Dim settingsDictionay = settingCtrl.GetSettings(dto.PortalId)

            Dim body = "<!DOCTYPE html>"
            body += "<html xmlns='http://www.w3.org/1999/xhtml'>"
            body += "<head>"
            body += String.Format("<title>Orcamento n {0}</title>", dto.EstimateId.ToString())
            body += "<style>body { font-size: 12px; padding: 0x; margin: 0px; } .dashTd table td { padding: 1px; } button { display: block; margin: 5px; } @media print { button { display: none; } }</style>"
            body += "</head>"
            body += "<body style='font-size: 12px;'>"

            body += "<button onclick='window.print()'>Imprimir</button>"

            body += Now.ToString("dd/MM/yy H:mm")
            body += String.Format("<br />ORCAMENTO n {0}<br />", dto.EstimateId.ToString())

            Dim clientInfo = personCtrl.GetPerson(estimateInfo.PersonId, dto.PortalId, Null.NullInteger)
            body += String.Format("<br />{0}<br />", ReplaceAccentletters(clientInfo.DisplayName))

            If Not CInt(settingsDictionay("mainConsumerId")) = estimateInfo.PersonId Then
                Dim clientStr = If(clientInfo.Street <> "", String.Format("{0} {1}", clientInfo.Street, clientInfo.Unit), "")
                clientStr += If(clientInfo.District <> "", String.Format("<br />{0}", clientInfo.District), "")
                clientStr += CStr(IIf(clientInfo.City <> "", String.Format("<br />{0} {1}", clientInfo.City, clientInfo.Region), ""))
                clientStr += CStr(IIf(clientInfo.PostalCode <> "", String.Format("<br />CEP: {0}", ZipMask(clientInfo.PostalCode)), ""))
                clientStr += CStr(If(clientInfo.Telephone <> "", String.Format("<br />Tel: {0}<br />", PhoneMask(clientInfo.Telephone)), ""))
                clientStr = ReplaceAccentletters(clientStr)

                body += clientStr
            End If

            body += "<br />&nbsp;Descricao<br />"
            body += "<table style='font-size: 12px;'>"
            body += "<tbody>"
            body += "<tr>"
            body += "<td style='width: 35px;'>Qde x</td>"
            body += "<td style='width: 60px;'>Valor U. =</td>"
            body += "<td>Total</td>"
            body += "</tr>"
            body += "<tr>"
            body += "<td colspan='3' class='dashTd'>------------------------------</td>"
            body += "</tr>"
            body += "</tbody>"
            body += "</table>"

            Dim estimateDetailInfo = estimateCtrl.GetEstimateItems(dto.PortalId, dto.EstimateId, "pt-BR")

            Dim itemRef = String.Empty
            Dim itemIndex = 0

            Dim totalItems As Single = 0.0
            Dim totalBeforeDiscount As Single = 0.0

            body += "<table style='font-size: 12px;'>"
            body += "<tbody>"

            For Each item In estimateDetailInfo

                itemIndex += 1

                Dim itemName = "0"
                If item.ProductName.Length > 35 Then
                    itemName = item.ProductName.ToLower().Remove(35)
                Else
                    itemName = item.ProductName.ToLower()
                End If

                Dim itemCode = ""
                If item.Barcode <> "" Then
                    itemCode = "CB: " & item.Barcode
                ElseIf item.ProductRef <> "" Then
                    itemCode = "REF: " & item.ProductRef.ToLower()
                End If

                totalBeforeDiscount = totalBeforeDiscount + (item.ProductEstimatePrice * item.ProductQty)

                Dim totalLiquid As Single = (item.ProductEstimatePrice - (item.ProductEstimatePrice / 100 * item.ProductDiscount)) * item.ProductQty

                body += "<tr>"
                body += String.Format("<td colspan='3'>{0}</td>", ReplaceAccentletters(itemName))
                body += "</tr>"
                body += "<tr>"
                body += String.Format("<td>{0} x</td>", item.ProductQty.ToString())
                body += String.Format("<td>{0} =</td>", FormatNumber(item.ProductEstimatePrice))
                body += String.Format("<td>{0}</td>", FormatNumber(totalLiquid))
                body += "</tr>"

                totalItems = totalItems + totalLiquid

            Next

            body += "</tbody>"
            body += "</table>"

            Dim discountValue = totalBeforeDiscount - totalItems
            Dim total As Single = totalBeforeDiscount ' - DiscountValue

            If estimateInfo.Discount > 0 Then
                discountValue = discountValue + (totalItems * estimateInfo.Discount / 100)
            End If

            total = total - discountValue

            If discountValue > 0.01 Then
                discountValue = String.Format("{0:C}", Math.Floor(discountValue * 100) / 100)
            End If

            Dim paidVal = CDec(estimateInfo.CreditAmount) + CDec(estimateInfo.ChequeAmount) + CDec(estimateInfo.CardAmount) + CDec(estimateInfo.CashAmount)

            body += String.Format("<br />SUBTOTAL: {0}{1}<br />TOTAL: {2}<br />", FormatNumber(totalBeforeDiscount),
                                  If(discountValue > 0, "<br />DESCONTO: " & FormatNumber(discountValue), ""),
                                  FormatNumber(total))

            body += "<br />Forma de Pagamento:<br />"

            body += If(estimateInfo.CreditAmount > 0, String.Format("Boleto: {0}<br />", FormatNumber(estimateInfo.CreditAmount)), "")
            body += If(estimateInfo.ChequeAmount > 0, String.Format("Cheque: {0}<br />", FormatNumber(estimateInfo.ChequeAmount)), "")
            body += If(estimateInfo.CardAmount > 0, String.Format("Cartao: {0}<br />", FormatNumber(estimateInfo.CardAmount)), "")
            body += If(estimateInfo.CashAmount > 0, String.Format("Dinheiro: {0}<br />", FormatNumber(estimateInfo.CashAmount)), "")

            Dim change = If(paidVal > total, (paidVal - total), 0)
            body += If(change > 0, String.Format("Troco: {0}<br />", FormatNumber(change)), "")

            If estimateInfo.PayCondType.Length > 1 Then

                body += "<br />Condicao de Pagamento:"
                body += String.Format("<br />Forma: {0}", ReplaceAccentletters(estimateInfo.PayCondType))
                body += String.Format("<br />Entrada: {0}", estimateInfo.PayCondIn)
                body += String.Format("<br />Parcela: {0} x {1}", estimateInfo.PayCondN.ToString(), estimateInfo.PayCondInst)
                body += String.Format("<br />Total: {0}", estimateInfo.TotalPayments)

            End If

            body += String.Format("<br />{0}", ReplaceAccentletters(PortalSettings.PortalName))
            body += String.Format("<br />{0}", PortalSettings.PortalAlias.HTTPAlias)
            body += String.Format("<br />{0} {1}", ReplaceAccentletters(settingsDictionay("storeAddress")), ReplaceAccentletters(settingsDictionay("storeUnit")))
            body += String.Format("<br />{0}", ReplaceAccentletters(settingsDictionay("storeDistrict")))
            body += String.Format("<br />{0}", ReplaceAccentletters(settingsDictionay("storeCity")), ReplaceAccentletters(settingsDictionay("storeRegion")))
            body += String.Format("<br />CEP: {0}", ReplaceAccentletters(settingsDictionay("storePostalCode")))
            body += String.Format("<br />Fone: {0}", If(salesPhone IsNot Nothing, PhoneMask(salesPhone), PhoneMask(Left(settingsDictionay("storePhone1").Replace("voz=", ""), 10))))
            body += String.Format("<br />{0}", If(salesRepInfo.Email.Length > 0, salesRepInfo.Email, settingsDictionay("storeEmail")))

            If Not Null.IsNull(estimateInfo.Comment) Then
                body += String.Format("<br />Observacoes Importantes:<br />{0}<br />{1}",
                                            ReplaceAccentletters(settingsDictionay("estimateTerm")), ReplaceAccentletters(estimateInfo.Comment))
            End If

            Dim destinationPath = FolderManager.Instance.GetFolder(dto.PortalId, "downloads")

            If destinationPath Is Nothing Then
                FolderManager.Instance.AddFolder(dto.PortalId, "downloads")
                FolderManager.Instance.Synchronize(dto.PortalId)
                destinationPath = FolderManager.Instance.GetFolder(dto.PortalId, "downloads")
            End If

            'Dim filePath = String.Format("{0}recibo_{1}_{2}.html", destinationPath.PhysicalPath, estimateInfo.EstimateId.ToString(), Replace(Now.ToString("dd-MM-yyyy HH"), ":", "-").Replace("/", "-").Replace(" ", "_"))
            Dim filePath = String.Format("{0}recibo_{1}.html", destinationPath.PhysicalPath, estimateInfo.EstimateId.ToString()) ', Replace(Now.ToString("dd-MM-yyyy HH"), ":", "-").Replace("/", "-").Replace(" ", "_"))

            If System.IO.File.Exists(filePath) Then
                System.IO.File.Delete(filePath)
            End If

            Using sw As New StreamWriter(filePath, True)
                sw.WriteLine(body)
                sw.Close()
            End Using

            'Return response
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Downloads estimate pdf
    ''' </summary>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function DownloadEstimatePdf(dto As EstimatePdf) As HttpResponseMessage
        Try
            Dim estimate As New Estimate
            Dim estimateItems As New List(Of EstimateItem)
            Dim estimateCtrl As New EstimatesRepository
            Dim payCondCtrl As New PayConditionsRepository
            Dim productCtrl As New ProductsRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, True)

            'Dim heading12 As Font = FontFactory.GetFont("ARIAL", 12)
            Dim heading10 As Font = FontFactory.GetFont("ARIAL", 10)
            Dim heading9 As Font = FontFactory.GetFont("VERDANA", 9)
            Dim heading8 As Font = FontFactory.GetFont("VERDANA", 8)
            Dim heading8b As Font = FontFactory.GetFont("VERDANA", 8, Font.BOLD)
            Dim heading7 As Font = FontFactory.GetFont("VERDANA", 7)

            'Dim str As New MemoryStream()

            'Dim pdfFile = HostingEnvironment.MapPath("\Portals\0\Downloads\" & String.Format("Orcamento_{0}_{1}.pdf", CStr(dto.EstimateId), Now().ToString().Replace(" ", "_").Replace("/", "-").Replace(":", "-")))
            Dim pdfFile = HostingEnvironment.MapPath("\Portals\0\Downloads\" & String.Format("Orcamento_{0}.pdf", CStr(dto.EstimateId))) ', Now().ToString().Replace(" ", "_").Replace("/", "-").Replace(":", "-")))

            If System.IO.File.Exists(pdfFile) Then
                System.IO.File.SetAttributes(pdfFile, FileAttributes.Normal)
                System.IO.File.Delete(pdfFile)
            End If

            Dim thePdf As New Document(PageSize.A4, 20, 20, 100, 15)

            Dim file As New FileStream(pdfFile, FileMode.OpenOrCreate)

            Dim writer As PdfWriter = PdfWriter.GetInstance(thePdf, file)
            'Dim writer As PdfWriter = PdfWriter.GetInstance(thePdf, str)

            Dim page As pdfPage = New pdfPage()
            writer.PageEvent = page

            thePdf.Open()

            Dim clientTable = New PdfPTable(2) With {.TotalWidth = 555.0F, .HorizontalAlignment = 0, .LockedWidth = True}
            Dim widthsHeaderTable As Single() = New Single() {1.0F, 1.0F}
            clientTable.SetWidths(widthsHeaderTable)

            Dim clientInfoHeader As New Paragraph("Informações do Cliente", heading10)

            Dim clientInfoLeft As New Paragraph(String.Format("Cliente: {0}", estimate.ClientDisplayName), heading8)
            clientInfoLeft.Add(Environment.NewLine())
            If estimate.ClientTelephone <> "" Then
                clientInfoLeft.Add(String.Format("Telefone: {0}{1}", PhoneMask(estimate.ClientTelephone), Environment.NewLine()))
            End If
            If estimate.ClientEmail <> "" Then
                clientInfoLeft.Add(String.Format("Email: {0}{1}", estimate.ClientEmail, Environment.NewLine()))
            End If
            If estimate.ClientEin <> "" Then
                clientInfoLeft.Add(String.Format("CNPJ: {0}{1}", estimate.ClientEin, Environment.NewLine()))
            End If
            If estimate.ClientSateTax <> "" Then
                clientInfoLeft.Add(String.Format("Inscrição Estadual: {0}{1}", estimate.ClientSateTax, Environment.NewLine()))
            End If
            If estimate.ClientCityTax <> "" Then
                clientInfoLeft.Add(String.Format("Inscrição Municipal: {0}{1}", estimate.ClientCityTax, Environment.NewLine()))
            End If

            Dim clientInfoRight As New Paragraph()
            If estimate.ClientAddress <> "" Then
                'clientInfoRight = New Paragraph(String.Format("Endereço: {0}{1}{2}{3}", estimate.ClientAddress, Space(1), estimate.ClientUnit, Environment.NewLine()), heading8)
                clientInfoRight = New Paragraph(String.Format("{0}{1}{2}{3}", estimate.ClientAddress, Space(1), estimate.ClientUnit, Environment.NewLine()), heading8)
            End If
            If estimate.ClientComplement <> "" Then
                'clientInfoRight.Add(String.Format("Complement: {0}{1}", estimate.ClientComplement, Environment.NewLine()))
                clientInfoRight.Add(String.Format("{0}{1}", estimate.ClientComplement, Environment.NewLine()))
            End If
            If estimate.ClientDistrict <> "" Then
                clientInfoRight.Add(String.Format("Bairro: {0}{1}", estimate.ClientDistrict, Environment.NewLine()))
            End If
            If estimate.ClientCity <> "" Then
                'clientInfoRight.Add(String.Format("Cidade: {0}{1}", estimate.ClientCity, Space(1)))
                clientInfoRight.Add(String.Format("{0}{1}, ", estimate.ClientCity, Space(1)))
            End If
            If estimate.ClientRegion <> "" Then
                'clientInfoRight.Add(String.Format("Estado: {0}{1}", estimate.ClientRegion, Space(1)))
                clientInfoRight.Add(String.Format("{0}{1} ", estimate.ClientRegion, Space(1)))
            End If
            If estimate.ClientPostalCode <> "" Then
                clientInfoRight.Add(String.Format("CEP: {0}", ZipMask(estimate.ClientPostalCode)))
            End If

            Dim clientCell1 = New PdfPCell(clientInfoHeader) With {.PaddingLeft = 7.0F, .PaddingTop = 5.0F, .PaddingBottom = 4.0F, .Colspan = 2, .BackgroundColor = New BaseColor(228, 228, 228)}
            Dim clientCell3 = New PdfPCell(clientInfoLeft) With {.Padding = 7.0F, .ExtraParagraphSpace = 2.0F}
            Dim clientCell4 = New PdfPCell(clientInfoRight) With {.Padding = 7.0F, .ExtraParagraphSpace = 2.0F}

            clientTable.AddCell(clientCell1)
            clientTable.AddCell(clientCell3)
            clientTable.AddCell(clientCell4)

            thePdf.Add(clientTable)

            Dim prodTable As New PdfPTable(dto.ColumnsCount) With {.SpacingBefore = 5.0F, .HorizontalAlignment = 0, .TotalWidth = clientTable.TotalWidth, .LockedWidth = True}
            Dim widthsProdTable As Single()
            If CSng(dto.ProductDiscountValue) > 0 Then
                widthsProdTable = New Single() {1.0F, 2.95F, 0.7F, 1.1F, 0.7F, 1.0F, 1.0F}
            Else
                widthsProdTable = New Single() {1.0F, 2.95F, 0.7F, 1.1F, 0.0F, 1.0F, 1.0F}
            End If
            prodTable.SetWidths(widthsProdTable)

            Dim prodCellHeader As PdfPCell = New PdfPCell(New Phrase("Itens do Orçamento", heading9)) With {.Colspan = dto.ColumnsCount, .PaddingLeft = 7.0F, .PaddingTop = 5.0F, .PaddingBottom = 4.0F, .BackgroundColor = New BaseColor(228, 228, 228)}
            prodTable.AddCell(prodCellHeader)

            For Each column In dto.Columns
                Dim prodCellHeaderColumns As PdfPCell = New PdfPCell(New Phrase(column, heading8)) With {.PaddingTop = 5.0F, .PaddingLeft = 5.0F}
                prodTable.AddCell(prodCellHeaderColumns)
            Next

            estimateItems = estimateCtrl.GetEstimateItems(dto.PortalId, dto.EstimateId, dto.Lang)

            For Each item In estimateItems

                Dim code = ""
                If item.Barcode <> "" Then
                    code = "CB: " & item.Barcode
                ElseIf item.ProductRef <> "" Then
                    code = "REF: " & item.ProductRef
                End If

                Dim prodCellProdRef As PdfPCell = New PdfPCell(New Phrase(code, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1}
                Dim prodCellProdName As PdfPCell = New PdfPCell(New Phrase(item.ProductName, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F}
                Dim prodCellQty As PdfPCell = New PdfPCell(New Phrase(item.ProductQty, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1}
                Dim prodCellEstProdOriginalPrice As PdfPCell = New PdfPCell(New Phrase(FormatCurrency(item.ProductEstimateOriginalPrice), heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1}
                Dim prodCellDiscount As PdfPCell = New PdfPCell(New Phrase(item.ProductDiscount, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1, .HorizontalAlignment = Element.ALIGN_RIGHT}
                Dim prodCellEstProdPrice As PdfPCell = New PdfPCell(New Phrase(FormatCurrency(item.ProductEstimatePrice), heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1, .HorizontalAlignment = Element.ALIGN_RIGHT}
                Dim prodCellExtendedAmount As PdfPCell = New PdfPCell(New Phrase(FormatCurrency(item.ExtendedAmount), heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1, .HorizontalAlignment = Element.ALIGN_RIGHT}

                If Not (item.ItemIndex Mod 2 = 1) Then
                    prodCellProdRef.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellProdName.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellQty.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellEstProdOriginalPrice.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellDiscount.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellEstProdPrice.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellExtendedAmount.BackgroundColor = New BaseColor(195, 195, 195)
                Else
                    prodCellProdRef.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellProdName.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellQty.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellEstProdOriginalPrice.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellDiscount.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellEstProdPrice.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellExtendedAmount.BackgroundColor = New BaseColor(228, 228, 228)
                End If

                prodTable.AddCell(prodCellProdRef)
                prodTable.AddCell(prodCellProdName)
                prodTable.AddCell(prodCellQty)
                prodTable.AddCell(prodCellEstProdOriginalPrice)
                prodTable.AddCell(prodCellDiscount)
                prodTable.AddCell(prodCellEstProdPrice)
                prodTable.AddCell(prodCellExtendedAmount)

                If dto.Expand Then
                    Dim expItem = productCtrl.GetProduct(item.ProductId, "pt-BR") 'Feature_Controller.Get_ProductDetail(CInt(item.GetDataKeyValue("ProdID")))
                    'For Each dRow In expItem
                    If expItem.Summary <> "" OrElse expItem.ProductImageId > 0 Then
                        '    Exit For
                        'End If
                        Dim prodCellProdIntro As PdfPCell = New PdfPCell(New Phrase(RemoveHtmlTags(HttpUtility.HtmlDecode(expItem.Summary)), heading7)) With {.PaddingTop = 10.0F, .PaddingLeft = 10.0F, .Border = 0}
                        Dim prodDetailTable As PdfPTable = New PdfPTable(2)
                        Dim widthsDetailTable As Single() = New Single() {0.5F, 3.0F}
                        prodDetailTable.SetWidths(widthsDetailTable)
                        Dim prodCellImage As PdfPCell = New PdfPCell() With {.PaddingTop = 10.0F, .PaddingLeft = 10.0F, .Border = 0}

                        If expItem.ProductImageId > 0 Then
                            Dim jpgProdImage As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(
                            New Uri(String.Format("http://{0}/databaseimages/{1}.{2}?maxwidth=130&maxheight=130{3}",
                                                  PortalSettings.PortalAlias.HTTPAlias, expItem.ProductImageId,
                                                  expItem.Extension, CStr(If(dto.Watermark <> "", "&watermark=outglow&text=" & dto.Watermark, "")))))
                            jpgProdImage.ScaleToFit(70.0F, 40.0F)
                            prodCellImage.AddElement(jpgProdImage)
                        End If

                        Dim prodCellProdDesc As PdfPCell = New PdfPCell(New Phrase(RemoveHtmlTags(HttpUtility.HtmlDecode(expItem.Description)), heading7)) With {.PaddingTop = 10.0F, .PaddingLeft = 10.0F, .Colspan = 2, .Border = 0}
                        prodDetailTable.AddCell(prodCellImage)
                        prodDetailTable.AddCell(prodCellProdIntro)
                        prodDetailTable.AddCell(prodCellProdDesc)
                        Dim prodCellDetail As PdfPCell = New PdfPCell(prodDetailTable) With {.Colspan = dto.Columns.Count}
                        prodTable.AddCell(prodCellDetail)
                        'Next
                    End If
                End If

            Next

            thePdf.Add(prodTable)

            Dim amountTable = New PdfPTable(2) With {.TotalWidth = prodTable.TotalWidth, .SpacingBefore = 5.0F, .LockedWidth = True}
            Dim widthsamountTable As Single() = New Single() {6.0F, 1.0F}
            amountTable.SetWidths(widthsamountTable)

            Dim amountCell = New PdfPCell() With {.Padding = 2.0F}

            'If Not UserInfo.IsInRole("Gerentes") And Not UserInfo.IsInRole("Vendedores") Then
            If dto.EstimateDiscountValue > 0 OrElse dto.ProductDiscountValue > 0 Then
                Dim originalAmountLabel As New Phrase("Valor Original: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(originalAmountLabel) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim originalAmount As New Phrase(String.Format("{0}", FormatCurrency(dto.ProductOriginalAmount)), heading7)
                amountCell = New PdfPCell(originalAmount) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If
            'Else

            If dto.ProductDiscountValue > 0 Then
                Dim productDiscountValueLabel As New Phrase("Desc. Produto: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(productDiscountValueLabel) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim productDiscountValue As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(dto.ProductDiscountValue), Environment.NewLine()), heading7)
                amountCell = New PdfPCell(productDiscountValue) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If

            If dto.EstimateDiscountValue > 0 Then
                Dim estimateDiscountValueLabel As New Phrase("Desc. Orçamento: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(estimateDiscountValueLabel) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim estimateDiscountValue As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(dto.EstimateDiscountValue), Environment.NewLine()), heading7)
                amountCell = New PdfPCell(estimateDiscountValue) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If

            'End If

            If dto.TotalDiscountPerc > 0 Then
                Dim totalDiscountTitleLabel As New Phrase("Desc. Total %: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(totalDiscountTitleLabel) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim totalDiscountTitle As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatPercent(dto.TotalDiscountPerc), Environment.NewLine()), heading7)
                amountCell = New PdfPCell(totalDiscountTitle) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If

            If dto.TotalDiscountValue > 0 Then
                Dim discountLabel As New Phrase("Desc. Total $: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(discountLabel) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim totalDiscountValue As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(dto.TotalDiscountValue), Environment.NewLine()), heading7)
                amountCell = New PdfPCell(totalDiscountValue) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If

            Dim paymentLabel As New Phrase("Valor Final: ", heading8b) With {.Leading = 20.0F}
            amountCell = New PdfPCell(paymentLabel) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            amountTable.AddCell(amountCell)

            Dim payment As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(dto.EstimateTotalAmount), Environment.NewLine()), heading7)
            amountCell = New PdfPCell(payment) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            amountTable.AddCell(amountCell)

            'If UserInfo.IsInRole("Gerentes") Then
            '    Dim MarkUpCurrency_Label As New Phrase("Markup $: ", heading8b) With {.Leading = 20.0F}
            '    amountCell = New PdfPCell(MarkUpCurrency_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            '    amountTable.AddCell(amountCell)

            '    Dim MarkUpCurrency As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(_MarkUpCurrency), Environment.NewLine()), heading7)
            '    amountCell = New PdfPCell(MarkUpCurrency) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            '    amountTable.AddCell(amountCell)

            '    Dim MarkupPerc_Label As New Phrase("Markup %: ", heading8b) With {.Leading = 20.0F}
            '    amountCell = New PdfPCell(MarkupPerc_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            '    amountTable.AddCell(amountCell)

            '    Dim MarkupPerc As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatPercent(_MarkupPerc), Environment.NewLine()), heading7)
            '    amountCell = New PdfPCell(MarkupPerc) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            '    amountTable.AddCell(amountCell)
            'End If

            thePdf.Add(amountTable)

            Dim payCondTitle As Paragraph = New Paragraph("Condições de Pagamento:", heading8b)
            thePdf.Add(payCondTitle)

            Dim payCond0 = payCondCtrl.GetPayConds(dto.PortalId, 0, CDec(dto.EstimateTotalAmount))
            If payCond0.Count > 0 Then
                For Each payCondType0 In payCond0
                    Dim payCondTitleType0 As New Paragraph(If(payCondType0.PayCondDisc > 0, String.Format("{0}{1}{0} Valor com desconto {2}",
                                                                                  Space(1),
                                                                                  payCondType0.PayCondTitle,
                                                                                  FormatCurrency(dto.EstimateTotalAmount - (dto.EstimateTotalAmount / 100 * payCondType0.PayCondDisc))), payCondType0.PayCondTitle) & " (mais condições de pagamento abaixo).", heading7)
                    thePdf.Add(payCondTitleType0)
                Next
            End If

            If estimate.PayCondType <> "" Then

                Dim payCondChosenTitle As Paragraph = New Paragraph("Condição de Pagamento Escolhida:", heading8b)
                thePdf.Add(payCondChosenTitle)
                Dim tablePayCondChosen = New PdfPTable(8) With {.TotalWidth = prodTable.TotalWidth, .SpacingBefore = 7.0F, .LockedWidth = True}
                Dim widthstablePayCondChosen As Single() = New Single() {1.2F, 1.4F, 1.2F, 1.4F, 1.4F, 1.0F, 1.0F, 1.2F}
                tablePayCondChosen.SetWidths(widthstablePayCondChosen)

                Dim payCondCellChosen As PdfPCell = New PdfPCell(New Phrase("Forma", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Número de parcelas", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Entrada", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Valor de Cada Parcela", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Valor do Parcelado", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Juros (a.m.)", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Interv. Dias", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Total", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(estimate.PayCondType, heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                If estimate.PayCondPerc > 0.0 Then
                    payCondCellChosen = New PdfPCell(New Phrase(String.Format("{0}x com juros", estimate.PayCondN), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                Else
                    payCondCellChosen = New PdfPCell(New Phrase(String.Format("{0}x sem juros", estimate.PayCondN), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                End If
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatCurrency(estimate.PayCondIn), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatCurrency(estimate.PayCondInst), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatCurrency(estimate.TotalPayments), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatPercent((estimate.PayCondPerc / 100)), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(CStr(If(estimate.PayCondInterval > 0, estimate.PayCondInterval, "Mensal")), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatCurrency(estimate.TotalPayCond), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                thePdf.Add(tablePayCondChosen)

            End If

            If dto.Conditions Then
                Dim objPayCond = payCondCtrl.GetPayConds(dto.PortalId, Null.NullInteger, CDec(dto.EstimateTotalAmount))

                If objPayCond.Count > 0 Then

                    Dim tablePayCond = New PdfPTable(8) With {.TotalWidth = prodTable.TotalWidth, .SpacingBefore = 7.0F, .LockedWidth = True}
                    Dim widthstablePayCond As Single() = New Single() {1.2F, 1.4F, 1.2F, 1.4F, 1.4F, 1.0F, 1.0F, 1.2F}
                    tablePayCond.SetWidths(widthstablePayCond)

                    Dim payCondCell As PdfPCell = New PdfPCell(New Phrase("Forma", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                    tablePayCond.AddCell(payCondCell)
                    payCondCell = New PdfPCell(New Phrase("Número de parcelas", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                    tablePayCond.AddCell(payCondCell)
                    payCondCell = New PdfPCell(New Phrase("Entrada", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                    tablePayCond.AddCell(payCondCell)
                    payCondCell = New PdfPCell(New Phrase("Valor de Cada Parcela", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                    tablePayCond.AddCell(payCondCell)
                    payCondCell = New PdfPCell(New Phrase("Valor do Parcelado", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                    tablePayCond.AddCell(payCondCell)
                    payCondCell = New PdfPCell(New Phrase("Juros (a.m.)", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                    tablePayCond.AddCell(payCondCell)
                    payCondCell = New PdfPCell(New Phrase("Interv. Dias", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                    tablePayCond.AddCell(payCondCell)
                    payCondCell = New PdfPCell(New Phrase("Total", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                    tablePayCond.AddCell(payCondCell)

                    For Each payCond In objPayCond

                        If payCond.PayCondType > 0 Then

                            Select Case payCond.PayCondType
                                Case 1
                                    payCondCell = New PdfPCell(New Phrase("Boleto", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                                Case 2
                                    payCondCell = New PdfPCell(New Phrase("Visa", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                                Case 3
                                    payCondCell = New PdfPCell(New Phrase("Master Card", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                                Case 4
                                    payCondCell = New PdfPCell(New Phrase("Amex", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                                Case 5
                                    payCondCell = New PdfPCell(New Phrase("Cheque Pré", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                                Case 6
                                    payCondCell = New PdfPCell(New Phrase("Cartão de Débito", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                                Case 7
                                    payCondCell = New PdfPCell(New Phrase("Dinners Clube", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            End Select
                            tablePayCond.AddCell(payCondCell)

                            Dim interestRate = 0.0
                            interestRate = payCond.PayCondPerc
                            Dim paymentQty = 0
                            paymentQty = payCond.PayCondN
                            Dim payIn = 0.0
                            payIn = payCond.PayCondIn

                            interestRate = interestRate / 100

                            Dim initialPay = (dto.EstimateTotalAmount / 100 * payIn)

                            Dim totalLabel = dto.EstimateTotalAmount - initialPay

                            Dim resultPayment As Double

                            If estimate.PayCondInterval > 0 Then

                                If interestRate > 0 Then

                                    Dim interestADay = interestRate / (DateTime.DaysInMonth(Year(DateTime.Now()), Month(DateTime.Now())))

                                    interestADay = interestADay * estimate.PayCondInterval

                                    If initialPay > 0 Then
                                        resultPayment = Pmt(interestADay, (paymentQty - 1), -totalLabel)
                                    Else
                                        resultPayment = Pmt(interestADay, 1, -totalLabel)
                                    End If

                                Else

                                    If initialPay > 0 Then
                                        resultPayment = Pmt(interestRate, (paymentQty - 1), -totalLabel)
                                    Else
                                        resultPayment = Pmt(interestRate, 1, -totalLabel)
                                    End If

                                End If

                            Else

                                If initialPay > 0 Then
                                    resultPayment = Pmt(interestRate, (paymentQty - 1), -totalLabel)
                                Else
                                    resultPayment = Pmt(interestRate, 1, -totalLabel)
                                End If

                            End If

                            If interestRate > 0.0 Then
                                payCondCell = New PdfPCell(New Phrase(String.Format("{0}x com juros", CStr(paymentQty)), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Else
                                payCondCell = New PdfPCell(New Phrase(String.Format("{0}x sem juros", CStr(paymentQty)), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            End If
                            tablePayCond.AddCell(payCondCell)

                            If initialPay > 0 Then
                                payCondCell = New PdfPCell(New Phrase(FormatCurrency(dto.EstimateTotalAmount / 100 * payIn), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Else
                                payCondCell = New PdfPCell(New Phrase(FormatCurrency(0), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            End If
                            tablePayCond.AddCell(payCondCell)

                            If initialPay > 0 Then
                                payCondCell = New PdfPCell(New Phrase(FormatCurrency(resultPayment), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Else
                                payCondCell = New PdfPCell(New Phrase(FormatCurrency(resultPayment / paymentQty), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            End If
                            tablePayCond.AddCell(payCondCell)

                            If initialPay > 0 Then
                                payCondCell = New PdfPCell(New Phrase(FormatCurrency((resultPayment * (paymentQty - 1))), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Else
                                payCondCell = New PdfPCell(New Phrase(FormatCurrency((resultPayment)), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            End If
                            tablePayCond.AddCell(payCondCell)

                            payCondCell = New PdfPCell(New Phrase(FormatPercent(interestRate), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            tablePayCond.AddCell(payCondCell)

                            payCondCell = New PdfPCell(New Phrase(If(payCond.PayCondInterval > 0, CStr(payCond.PayCondInterval), "Mensal"), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            tablePayCond.AddCell(payCondCell)

                            If initialPay > 0 Then
                                payCondCell = New PdfPCell(New Phrase(FormatCurrency((resultPayment * (paymentQty - 1)) + initialPay), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Else
                                payCondCell = New PdfPCell(New Phrase(FormatCurrency((resultPayment * 1) + initialPay), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            End If
                            tablePayCond.AddCell(payCondCell)

                        End If

                    Next

                    thePdf.Add(tablePayCond)

                End If
            End If

            Dim settingCtrl As New SettingsRepository
            Dim settingsDictionay = settingCtrl.GetSettings(dto.PortalId)

            Dim termsOE = ""
            'Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(dto.PortalId)
            If estimate.Inst <> "" Then
                termsOE = estimate.Inst.Replace("<br />", Environment.NewLine()).Replace("<p>", Environment.NewLine()).Replace("</p>", Environment.NewLine())
            Else
                termsOE = settingsDictionay("estimateTerm").Replace("<br />", Environment.NewLine()).Replace("<p>", Environment.NewLine()).Replace("</p>", Environment.NewLine())
            End If

            Dim obsTitle As New Paragraph("Observações Importantes:", heading9) With {.SpacingBefore = 10.0F}
            Dim obsText As New Paragraph(String.Format("{0}", RemoveHtmlTags(HttpUtility.HtmlDecode(termsOE))), heading7)
            Dim obsComment As New Paragraph(String.Format("{0}", RemoveHtmlTags(HttpUtility.HtmlDecode(estimate.Comment.Replace("<br />", Environment.NewLine).Replace("<p>", Environment.NewLine).Replace("</p>", Environment.NewLine)))), heading7) With {.SpacingBefore = 10.0F}
            thePdf.Add(obsTitle)
            thePdf.Add(obsText)
            thePdf.Add(obsComment)

            writer.CloseStream = False
            thePdf.Close()
            file.Close()
            'str.Position = 0

            'Dim _mediaType = MediaTypeHeaderValue.Parse("application/pdf")
            'Dim response As New HttpResponseMessage(HttpStatusCode.OK)
            'response.Content = New StreamContent(str)
            'response.Content.Headers.ContentType = _mediaType
            'response.Content.Headers.ContentDisposition = New ContentDispositionHeaderValue("fileName") With {.FileName = String.Format("Orcamento_{0}_{1}.pdf", CStr(dto.EstimateId), Replace(DateTime.Now.ToString(), ":", "-").Replace("/", "-").Replace(" ", ""))}

            Dim estimateHistory As New EstimateHistory

            estimateHistory.EstimateId = dto.EstimateId
            estimateHistory.HistoryText = "<p>Novo pdf foi gerado.</p>"
            estimateHistory.Locked = True
            estimateHistory.CreatedByUser = 0
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
            End If

            'Return response
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .FileName = System.IO.Path.GetFileName(pdfFile)})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Downloads estimate txt
    ''' </summary>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function DownloadEstimateTxt(dto As EstimateTxt) As HttpResponseMessage
        Try
            Dim payCondCtrl As New PayConditionsRepository
            Dim personCtrl As New PeopleRepository
            Dim estimateCtrl As New EstimatesRepository
            Dim estimateInfo = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, True)
            Dim salesRepInfo = UserController.GetUserById(dto.PortalId, estimateInfo.SalesRep)

            Dim salesPhone = String.Empty
            salesPhone = salesRepInfo.Profile.Telephone

            Dim myPortalSettings = PortalController.Instance.GetPortalSettings(dto.PortalId)

            Dim settingCtrl As New SettingsRepository
            Dim settingsDictionay = settingCtrl.GetSettings(dto.PortalId)

            Dim header = dto.TxtHeader.Replace("[BR]", Environment.NewLine)
            header = header.Replace("[DATA]", TitleCase(DateTime.Now.ToString("f", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"))))
            header = header.Replace("[EMPRESA]", ReplaceAccentletters(PortalSettings.PortalName))
            header = header.Replace("[SITE]", PortalSettings.PortalAlias.HTTPAlias)
            header = header.Replace("[RUA]", settingsDictionay("storeAddress"))
            header = header.Replace("[NUM]", settingsDictionay("storeUnit"))
            header = header.Replace("[BAIRRO]", settingsDictionay("storeDistrict"))
            header = header.Replace("[CIDADE]", settingsDictionay("storeCity"))
            header = header.Replace("[ESTADO]", settingsDictionay("storeRegion"))
            header = header.Replace("[CEP]", settingsDictionay("storePostalCode"))
            header = ReplaceAccentletters(header)

            ' HTML Structure
            Dim saveString = Null.NullString

            saveString = header

            Dim subHeader = dto.TxtSubHeader.Replace("[BR]", Environment.NewLine)
            subHeader = subHeader.Replace("[FONE]", If(salesPhone IsNot Nothing, PhoneMask(salesPhone), PhoneMask(Left(settingsDictionay("storePhone1").Replace("voz=", ""), 10))))
            subHeader = subHeader.Replace("[EMAIL]", If(salesRepInfo.Email.Length > 0, salesRepInfo.Email, settingsDictionay("storeEmail")))
            subHeader = subHeader.Replace("[ORC]", dto.EstimateId)
            subHeader = ReplaceAccentletters(subHeader)

            saveString += subHeader

            If Not CInt(settingsDictionay("mainConsumerId")) = dto.PersonId Then
                Dim clientInfo = personCtrl.GetPerson(dto.PersonId, dto.PortalId, Null.NullInteger)
                Dim clientStr = RemoveHtmlTags(ReplaceAccentletters(HttpUtility.HtmlDecode(dto.TxtClientInfo))).Replace("[BR]", Environment.NewLine)
                clientStr = clientStr.Replace("[NOME]", clientInfo.DisplayName)
                clientStr = clientStr.Replace("[RUA]", clientInfo.Street)
                clientStr = clientStr.Replace("[NUM]", clientInfo.Unit)
                clientStr = clientStr.Replace("[BAIRRO]", clientInfo.District)
                clientStr = clientStr.Replace("[CIDADE]", clientInfo.City)
                clientStr = clientStr.Replace("[ESTADO]", clientInfo.Region)
                clientStr = clientStr.Replace("[CEP]", ZipMask(clientInfo.PostalCode))
                clientStr = clientStr.Replace("[TELEFONE]", PhoneMask(clientInfo.Telephone))
                clientStr = ReplaceAccentletters(clientStr)

                saveString += clientStr
            End If

            saveString += dto.TxtColumnHeader.Replace("[BR]", Environment.NewLine)

            Dim estimateDetailInfo = estimateCtrl.GetEstimateItems(dto.PortalId, dto.EstimateId, "pt-BR")

            Dim itemRef = String.Empty
            Dim itemIndex = 0

            Dim totalItems As Single = 0.0
            Dim totalBeforeDiscount As Single = 0.0
            Dim builder As New StringBuilder()
            builder.Append(saveString)

            For Each item In estimateDetailInfo

                itemIndex += 1

                Dim itemName = "0"
                If item.ProductName.Length > 25 Then
                    itemName = item.ProductName.Remove(25)
                Else
                    itemName = item.ProductName
                End If

                Dim itemCode = ""
                If item.Barcode <> "" Then
                    itemCode = "CB: " & item.Barcode
                ElseIf item.ProductRef <> "" Then
                    itemCode = "REF: " & item.ProductRef
                End If

                totalBeforeDiscount = totalBeforeDiscount + (item.ProductEstimatePrice * item.ProductQty)

                Dim totalLiquid As Single = (item.ProductEstimatePrice - (item.ProductEstimatePrice / 100 * item.ProductDiscount)) * item.ProductQty

                Dim padItemName = Right(dto.TxtItemName.Replace("]", ""), 2).TrimStart("0"c)
                Dim padItemRef = Right(dto.TxtItemRef.Replace("]", ""), 2).TrimStart("0"c)
                Dim padPrice = Right(dto.TxtItemPrice.Replace("]", ""), 2).TrimStart("0"c)
                Dim padQty = Right(dto.TxtItemQty.Replace("]", ""), 2).TrimStart("0"c)
                Dim padUni = Right(dto.TxtItemUni.Replace("]", ""), 2).TrimStart("0"c)
                Dim padDisc = Right(dto.TxtItemDisc.Replace("]", ""), 2).TrimStart("0"c)

                Dim strItemName = dto.TxtItemName.Replace("[ITEMNAME:", RemoveHtmlTags(ReplaceAccentletters(HttpUtility.HtmlDecode(itemName))))
                Dim strItemRef = dto.TxtItemRef.Replace("[ITEMREF:", Right(itemCode, 13))
                Dim strItemPrice = dto.TxtItemPrice.Replace("[ITEMPRICE:", FormatCurrency(item.ProductEstimateOriginalPrice))
                Dim strItemQty = dto.TxtItemQty.Replace("[ITEMQTY:", item.ProductQty)
                Dim strItemUni = dto.TxtItemUni.Replace("[ITEMUNI:", Left(item.UnitTypeTitle, 3))
                Dim strItemDisc = dto.TxtItemDisc.Replace("[ITEMDISC:", FormatPercent((item.ProductDiscount) / 100))

                strItemName = Left(strItemName, itemName.Length)
                strItemRef = Left(strItemRef, Right(itemCode, dto.EstimateId).Length) & Environment.NewLine
                strItemPrice = Left(strItemPrice, FormatCurrency(item.ProductEstimateOriginalPrice).Length)
                strItemQty = Left(strItemQty, CStr(item.ProductQty).Length)
                strItemUni = Left(strItemUni, Left(item.UnitTypeTitle, 3).Length)
                strItemDisc = Left(strItemDisc, FormatPercent((item.ProductDiscount) / 100).Length)

                builder.Append(String.Concat(itemRef.PadRight(7),
                                            strItemName.PadRight(padItemName),
                                            strItemRef.PadRight(padItemRef),
                                            strItemPrice.PadRight(padPrice),
                                            strItemQty.PadRight(padQty),
                                            strItemUni.PadRight(padUni),
                                            strItemDisc.PadRight(padDisc),
                                            FormatCurrency(totalLiquid) & Environment.NewLine))

                totalItems = totalItems + totalLiquid

            Next
            saveString = builder.ToString()

            Dim lPadTotalDisc = Left(dto.TxtDiscount.Replace("[", ""), 2).TrimStart("0"c)
            Dim rPadTotalDisc = Right(dto.TxtDiscount.Replace("]", ""), 2).TrimStart("0"c)
            Dim strDiscount = ""
            Dim discountValue = totalBeforeDiscount - totalItems
            Dim total As Single = totalBeforeDiscount ' - DiscountValue

            If estimateInfo.Discount > 0 Then
                discountValue = discountValue + (totalItems * estimateInfo.Discount / 100)
                total = Math.Round(total - discountValue)
            End If

            If discountValue > 0.01 Then
                discountValue = String.Format("{0:C}", Math.Floor(discountValue * 100) / 100)
                strDiscount = String.Concat("DESCONTO:".PadRight(rPadTotalDisc), dto.TxtDiscount.Replace(String.Format("[{0}:DISCOUNT:", lPadTotalDisc), FormatCurrency(discountValue))).PadRight(rPadTotalDisc)
                strDiscount = strDiscount.Replace(rPadTotalDisc & "]", "")
            Else
                strDiscount = ""
            End If

            Dim rPadSubTotal = Right(dto.TxtSubTotal.Replace("]", ""), 2).TrimStart("0"c)
            Dim rPadTotal = Right(dto.TxtTotal.Replace("]", ""), 2).TrimStart("0"c)

            Dim lPadSubTotal = Left(dto.TxtSubTotal.Replace("[", ""), 2).TrimStart("0"c)
            Dim lPadTotal = Left(dto.TxtTotal.Replace("[", ""), 2).TrimStart("0"c)

            Dim strSubTotal = String.Concat("SUBTOTAL:".PadRight(rPadSubTotal), dto.TxtSubTotal.Replace(String.Format("[{0}:SUBTOTAL:", lPadSubTotal), FormatCurrency(totalBeforeDiscount))).PadRight(rPadSubTotal)
            Dim strTotal = String.Concat("TOTAL:".PadRight(rPadTotal), dto.TxtTotal.Replace(String.Format("[{0}:TOTAL:", lPadTotal), FormatCurrency(total))).PadRight(rPadTotal)

            strSubTotal = strSubTotal.Replace(rPadSubTotal & "]", "")
            strTotal = strTotal.Replace(rPadTotal & "]", "")

            saveString += String.Concat(Environment.NewLine, strSubTotal.PadLeft(lPadSubTotal), If(strDiscount.Length > 0, Environment.NewLine & strDiscount.PadLeft(CInt(lPadTotalDisc)), ""), Environment.NewLine, strTotal.PadLeft(lPadTotal))

            Dim objPayCond = payCondCtrl.GetPayConds(dto.PortalId, Null.NullInteger, total)

            If objPayCond.Count > 0 Then

                saveString += String.Concat(Environment.NewLine, Environment.NewLine, "Condicoes de Pagamento:", Environment.NewLine)

                saveString += String.Concat(Environment.NewLine, dto.TxtConditionColumnHeader.Replace("[BR]", Environment.NewLine), Environment.NewLine)

                For Each payCond In objPayCond

                    If payCond.PayCondType > 0 Then

                        Select Case payCond.PayCondType
                            Case 5
                                Dim padcheck = Right(dto.TxtCheck, 3).TrimStart("0"c).Replace("]", "")
                                saveString += String.Concat(Environment.NewLine, Left(dto.TxtCheck.Replace("[", ""), dto.TxtCheck.Length - 5).PadRight(padcheck))
                            Case 1
                                Dim padBankPay = Right(dto.TxtBankPay, 3).TrimStart("0"c).Replace("]", "")
                                saveString += String.Concat(Environment.NewLine, Left(dto.TxtBankPay.Replace("[", ""), dto.TxtBankPay.Length - 5).PadRight(padBankPay))
                            Case 2
                                Dim padVisa = Right(dto.TxtVisa, 3).TrimStart("0"c).Replace("]", "")
                                saveString += String.Concat(Environment.NewLine, Left(dto.TxtVisa.Replace("[", ""), dto.TxtVisa.Length - 5).PadRight(padVisa))
                            Case 3
                                Dim padMC = Right(dto.TxtMC, 3).TrimStart("0"c).Replace("]", "")
                                saveString += String.Concat(Environment.NewLine, Left(dto.TxtMC.Replace("[", ""), dto.TxtMC.Length - 5).PadRight(padMC))
                            Case 4
                                Dim padAmex = Right(dto.TxtAmex, 3).TrimStart("0"c).Replace("]", "")
                                saveString += String.Concat(Environment.NewLine, Left(dto.TxtAmex.Replace("[", ""), dto.TxtAmex.Length - 5).PadRight(padAmex))
                            Case 6
                                Dim padDinners = Right(dto.TxtDinners, 3).TrimStart("0"c).Replace("]", "")
                                saveString += String.Concat(Environment.NewLine, Left(dto.TxtDinners.Replace("[", ""), dto.TxtDinners.Length - 5).PadRight(padDinners))
                            Case 7
                                Dim padDinners = Right(dto.TxtDebit, 3).TrimStart("0"c).Replace("]", "")
                                saveString += String.Concat(Environment.NewLine, Left(dto.TxtDebit.Replace("[", ""), dto.TxtDebit.Length - 5).PadRight(padDinners))
                            Case Else
                        End Select

                        Dim interestRate = 0.0
                        interestRate = payCond.PayCondPerc
                        Dim paymentQty = 0
                        paymentQty = payCond.PayCondN
                        Dim payIn = 0.0
                        payIn = payCond.PayCondIn

                        interestRate = interestRate / 100

                        Dim totalLabel = total

                        Dim resultPayment As Double = Pmt(interestRate, paymentQty, -totalLabel, 0)

                        Dim padPayQty = Right(dto.TxtPayQty, 3).TrimStart("0"c).Replace("]", "")

                        If interestRate > 0 Then

                            Dim interestADay = interestRate / (DateTime.DaysInMonth(Year(DateTime.Now()), Month(DateTime.Now())))

                            interestADay = interestADay * payCond.PayCondInterval

                            If payIn > 0 Then
                                resultPayment = Pmt(interestADay, (paymentQty - 1), -totalLabel)
                                saveString += Left(dto.TxtPayQty.Replace("[", ""), "PAYQTY".Length).Replace("PAYQTY", paymentQty - 1).PadRight(padPayQty)
                            Else
                                resultPayment = Pmt(interestADay, paymentQty, -totalLabel)
                                saveString += Left(dto.TxtPayQty.Replace("[", ""), "PAYQTY".Length).Replace("PAYQTY", paymentQty).PadRight(padPayQty)
                            End If

                        Else

                            If payIn > 0 Then
                                resultPayment = Pmt(interestRate, (paymentQty - 1), -totalLabel)
                                saveString += Left(dto.TxtPayQty.Replace("[", ""), "PAYQTY".Length).Replace("PAYQTY", paymentQty - 1).PadRight(padPayQty)
                            Else
                                resultPayment = Pmt(interestRate, paymentQty, -totalLabel)
                                saveString += Left(dto.TxtPayQty.Replace("[", ""), "PAYQTY".Length).Replace("PAYQTY", paymentQty).PadRight(padPayQty)
                            End If

                        End If

                        Dim padResultPayment = Right(dto.TxtPayments, 3).TrimStart("0"c).Replace("]", "")
                        saveString += Left(dto.TxtPayments.Replace("[", ""), "PAYMENTS".Length).Replace("PAYMENTS", FormatCurrency(resultPayment)).PadRight(padResultPayment) & Environment.NewLine

                        Dim padInitialPay = Right(dto.TxtInitialPay, 3).TrimStart("0"c).Replace("]", "")
                        If payIn > 0 Then
                            saveString += Left(dto.TxtInitialPay.Replace("[", ""), "INITIALPAY".Length).Replace("INITIALPAY", FormatCurrency(total / 100 * payIn)).PadRight(padInitialPay)
                        Else
                            saveString += Left(dto.TxtInitialPay.Replace("[", ""), "INITIALPAY".Length).Replace("INITIALPAY", FormatCurrency(0)).PadRight(padInitialPay)
                        End If

                        Dim padInterest = Right(dto.TxtInterest, 3).TrimStart("0"c).Replace("]", "")
                        saveString += Left(dto.TxtInterest.Replace("[", ""), "INTEREST".Length).Replace("INTEREST", FormatPercent(interestRate)).PadRight(padInterest)

                        Dim padTotalPays = Right(dto.TxtTotalPays, 3).TrimStart("0"c).Replace("]", "")
                        saveString += Left(dto.TxtTotalPays.Replace("[", ""), "TOTALPAYS".Length).Replace("TOTALPAYS", FormatCurrency(resultPayment * paymentQty)).PadRight(padTotalPays)

                        saveString += If(payCond.PayCondInterval > 0, CStr(payCond.PayCondInterval), "Mensal")

                    End If

                Next

            End If

            ' ReplaceAccentletters(estimateInfo.Comment).Replace(". ", "").Replace("<br>", Environment.NewLine).Replace("<p>", Environment.NewLine).Replace("<br />", Environment.NewLine).Replace("</p>", Environment.NewLine).Replace("[BR]", Environment.NewLine))

            If settingsDictionay("estimateTerm") <> "" Then
                saveString += String.Format("{0}{0}{1}{0}{2}",
                                        Environment.NewLine,
                                        "Observacoes Importantes:",
                                        ReplaceAccentletters(settingsDictionay("estimateTerm")).Replace(". ", "").Replace("<br>", Environment.NewLine).Replace("<p>", Environment.NewLine).Replace("<br />", Environment.NewLine).Replace("</p>", Environment.NewLine).Replace("[BR]", Environment.NewLine))
            End If

            CreateDir(PortalController.Instance.GetCurrentPortalSettings(), "downloads")

            Dim destinationPath = FolderManager.Instance.GetFolder(dto.PortalId, "downloads")

            'Dim filePath = String.Format("{0}{1}", destinationPath.PhysicalPath, "orcamento.txt")
            Dim filePath = String.Format("{0}orcamento_{1}.txt", destinationPath.PhysicalPath, estimateInfo.EstimateId.ToString()) ', Replace(Now.ToString("dd-MM-yyyy HH"), ":", "-").Replace("/", "-").Replace(" ", "_"))

            If System.IO.File.Exists(filePath) Then
                System.IO.File.Delete(filePath)
            End If

            Using sw As New StreamWriter(filePath, True)
                sw.WriteLine(saveString)
                sw.Close()
            End Using

            'Dim response As New HttpResponseMessage()
            'response.Content.Headers.ContentType = New Headers.MediaTypeHeaderValue("text/plain")
            'response.Content.Headers.ContentDisposition = New Headers.ContentDispositionHeaderValue("attachment")
            'response.Content.Headers.ContentDisposition.FileName = String.Format("orcamento_{0}_{1}.txt", estimateInfo.EstimateId.ToString(), Replace(Now.ToString("dd-MM-yyyy HH"), ":", "-").Replace("/", "-").Replace(" ", "_"))

            'Return response
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Downloads estimate txt receipt
    ''' </summary>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function DownloadEstimateTxtReceipt(dto As EstimateReceiptTxt) As HttpResponseMessage
        Try
            'Dim payCondCtrl As New PayConditionsRepository
            Dim personCtrl As New PeopleRepository
            Dim estimateCtrl As New EstimatesRepository
            Dim estimateInfo = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, True)
            Dim salesRepInfo = UserController.GetUserById(dto.PortalId, estimateInfo.SalesRep)

            Dim salesPhone = String.Empty
            salesPhone = salesRepInfo.Profile.Telephone

            'Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(dto.PortalId)

            Dim settingCtrl As New SettingsRepository
            Dim settingsDictionay = settingCtrl.GetSettings(dto.PortalId)

            Dim header = dto.TxtHeader.Replace("[BR]", Environment.NewLine)
            header = header.Replace("[DATA]", TitleCase(DateTime.Now.ToString("f", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"))))
            header = header.Replace("[EMPRESA]", ReplaceAccentletters(PortalSettings.PortalName))
            header = header.Replace("[SITE]", PortalSettings.PortalAlias.HTTPAlias)
            header = header.Replace("[RUA]", settingsDictionay("storeAddress"))
            header = header.Replace("[NUM]", settingsDictionay("storeUnit"))
            header = header.Replace("[BAIRRO]", settingsDictionay("storeDistrict"))
            header = header.Replace("[CIDADE]", settingsDictionay("storeCity"))
            header = header.Replace("[ESTADO]", settingsDictionay("storeRegion"))
            header = header.Replace("[CEP]", settingsDictionay("storePostalCode"))
            header = ReplaceAccentletters(header)

            ' HTML Structure
            Dim saveString = Null.NullString

            saveString = header

            Dim subHeader = dto.TxtSubHeader.Replace("[BR]", Environment.NewLine)
            subHeader = subHeader.Replace("[FONE]", If(salesPhone.Length > 0, PhoneMask(salesPhone), PhoneMask(Left(settingsDictionay("storePhone1").Replace("voz=", ""), 10))))
            subHeader = subHeader.Replace("[EMAIL]", If((salesRepInfo.Email.Length > 0 OrElse salesRepInfo.Email.IndexOf("user") > 0), salesRepInfo.Email, settingsDictionay("storeEmail")))
            subHeader = subHeader.Replace("[ORC]", dto.EstimateId)
            subHeader = ReplaceAccentletters(subHeader)

            saveString += subHeader

            If Not CInt(settingsDictionay("mainConsumerId")) = dto.PersonId Then
                Dim clientInfo = personCtrl.GetPerson(dto.PersonId, dto.PortalId, Null.NullInteger)
                Dim clientStr = RemoveHtmlTags(ReplaceAccentletters(HttpUtility.HtmlDecode(dto.TxtClientInfo))).Replace("[BR]", Environment.NewLine)
                clientStr = clientStr.Replace("[NOME]", clientInfo.DisplayName)
                clientStr = clientStr.Replace("[RUA]", clientInfo.Street)
                clientStr = clientStr.Replace("[NUM]", clientInfo.Unit)
                clientStr = clientStr.Replace("[BAIRRO]", clientInfo.District)
                clientStr = clientStr.Replace("[CIDADE]", clientInfo.City)
                clientStr = clientStr.Replace("[ESTADO]", clientInfo.Region)
                clientStr = clientStr.Replace("[CEP]", ZipMask(clientInfo.PostalCode))
                clientStr = clientStr.Replace("[TELEFONE]", PhoneMask(clientInfo.Telephone))
                clientStr = ReplaceAccentletters(clientStr)

                saveString += clientStr
            End If

            saveString += dto.TxtColumnHeader.Replace("[BR]", Environment.NewLine)

            Dim estimateDetailInfo = estimateCtrl.GetEstimateItems(dto.PortalId, dto.EstimateId, "pt-BR")

            Dim itemRef = String.Empty
            Dim itemIndex = 0

            Dim totalItems As Single = 0.0
            Dim totalBeforeDiscount As Single = 0.0
            Dim builder As New StringBuilder()
            builder.Append(saveString)

            For Each item In estimateDetailInfo

                itemIndex += 1

                Dim itemName = "0"
                If item.ProductName.Length > 25 Then
                    itemName = item.ProductName.Remove(25)
                Else
                    itemName = item.ProductName
                End If
                'Dim itemCode = "0000000000000"
                'If item.Barcode.Length > 0 Then
                '    itemCode = item.Barcode
                'End If

                Dim itemCode = ""
                If item.Barcode <> "" Then
                    itemCode = "CB: " & item.Barcode
                ElseIf item.ProductRef <> "" Then
                    itemCode = "REF: " & item.ProductRef
                End If

                totalBeforeDiscount = totalBeforeDiscount + (item.ProductEstimatePrice * item.ProductQty)

                Dim totalLiquid As Single = (item.ProductEstimatePrice - (item.ProductEstimatePrice / 100 * item.ProductDiscount)) * item.ProductQty

                Dim padItemName = Right(dto.TxtItemName.Replace("]", ""), 2).TrimStart("0"c)
                Dim padItemRef = Right(dto.TxtItemRef.Replace("]", ""), 2).TrimStart("0"c)
                Dim padPrice = Right(dto.TxtItemPrice.Replace("]", ""), 2).TrimStart("0"c)
                Dim padQty = Right(dto.TxtItemQty.Replace("]", ""), 2).TrimStart("0"c)
                Dim padUni = Right(dto.TxtItemUni.Replace("]", ""), 2).TrimStart("0"c)
                Dim padDisc = Right(dto.TxtItemDisc.Replace("]", ""), 2).TrimStart("0"c)

                Dim strItemName = dto.TxtItemName.Replace("[ITEMNAME:", RemoveHtmlTags(ReplaceAccentletters(HttpUtility.HtmlDecode(itemName))))
                Dim strItemRef = dto.TxtItemRef.Replace("[ITEMREF:", Right(itemCode, 13))
                Dim strItemPrice = dto.TxtItemPrice.Replace("[ITEMPRICE:", FormatCurrency(item.ProductEstimateOriginalPrice))
                Dim strItemQty = dto.TxtItemQty.Replace("[ITEMQTY:", item.ProductQty)
                Dim strItemUni = dto.TxtItemUni.Replace("[ITEMUNI:", Left(item.UnitTypeTitle, 3))
                Dim strItemDisc = dto.TxtItemDisc.Replace("[ITEMDISC:", FormatPercent((item.ProductDiscount) / 100))

                strItemName = Left(strItemName, itemName.Length)
                strItemRef = Left(strItemRef, Right(itemCode, dto.EstimateId).Length) & Environment.NewLine
                strItemPrice = Left(strItemPrice, FormatCurrency(item.ProductEstimateOriginalPrice).Length)
                strItemQty = Left(strItemQty, CStr(item.ProductQty).Length)
                strItemUni = Left(strItemUni, Left(item.UnitTypeTitle, 3).Length)
                strItemDisc = Left(strItemDisc, FormatPercent((item.ProductDiscount) / 100).Length)

                builder.Append(String.Concat(itemRef.PadRight(7),
                                            strItemName.PadRight(padItemName),
                                            strItemRef.PadRight(padItemRef),
                                            strItemPrice.PadRight(padPrice),
                                            strItemQty.PadRight(padQty),
                                            strItemUni.PadRight(padUni),
                                            strItemDisc.PadRight(padDisc),
                                            FormatCurrency(totalLiquid) & Environment.NewLine))

                totalItems = totalItems + totalLiquid

            Next
            saveString = builder.ToString()

            Dim lPadTotalDisc = Left(dto.TxtDiscount.Replace("[", ""), 2).TrimStart("0"c)
            Dim rPadTotalDisc = Right(dto.TxtDiscount.Replace("]", ""), 2).TrimStart("0"c)
            Dim strDiscount = ""
            Dim discountValue = totalBeforeDiscount - totalItems
            Dim total As Single = totalBeforeDiscount ' - DiscountValue

            'Total = Math.Round(Total - (Total * estimate_Info.Discount / 100), 2)

            If estimateInfo.Discount > 0 Then
                discountValue = discountValue + (totalItems * estimateInfo.Discount / 100)
                total = Math.Round(total - discountValue)
            End If

            If discountValue > 0.01 Then
                discountValue = String.Format("{0:C}", Math.Floor(discountValue * 100) / 100)
                strDiscount = String.Concat("DESCONTO:".PadRight(rPadTotalDisc), dto.TxtDiscount.Replace(String.Format("[{0}:DISCOUNT:", lPadTotalDisc), FormatCurrency(discountValue))).PadRight(rPadTotalDisc)
                strDiscount = strDiscount.Replace(rPadTotalDisc & "]", "")
            Else
                strDiscount = ""
            End If

            Dim rPadSubTotal = Right(dto.TxtSubTotal.Replace("]", ""), 2).TrimStart("0"c)
            Dim rPadTotal = Right(dto.TxtTotal.Replace("]", ""), 2).TrimStart("0"c)

            Dim lPadSubTotal = Left(dto.TxtSubTotal.Replace("[", ""), 2).TrimStart("0"c)
            Dim lPadTotal = Left(dto.TxtTotal.Replace("[", ""), 2).TrimStart("0"c)

            Dim strSubTotal = String.Concat("SUBTOTAL:".PadRight(rPadSubTotal), dto.TxtSubTotal.Replace(String.Format("[{0}:SUBTOTAL:", lPadSubTotal), FormatCurrency(totalBeforeDiscount))).PadRight(rPadSubTotal)
            Dim strTotal = String.Concat("TOTAL:".PadRight(rPadTotal), dto.TxtTotal.Replace(String.Format("[{0}:TOTAL:", lPadTotal), FormatCurrency(total))).PadRight(rPadTotal)

            strSubTotal = strSubTotal.Replace(rPadSubTotal & "]", "")
            strTotal = strTotal.Replace(rPadTotal & "]", "")

            saveString += String.Concat(Environment.NewLine, strSubTotal.PadLeft(lPadSubTotal), If(strDiscount.Length > 0, Environment.NewLine & strDiscount.PadLeft(CInt(lPadTotalDisc)), ""), Environment.NewLine, strTotal.PadLeft(lPadTotal))

            Dim paidVal = CDec(dto.TxtBankIn.Replace(".", ",")) + CDec(dto.TxtCheckIn.Replace(".", ",")) + CDec(dto.TxtCardIn.Replace(".", ",")) + CDec(dto.TxtDebitIn.Replace(".", ",")) + CDec(dto.TxtCashIn.Replace(".", ","))

            'saveString += String.Concat(Environment.NewLine & Environment.NewLine & "PAGO:".PadRight(CInt(rPadTotal)), Localization.GetString("TotalPaid", LocalResourceFile).Replace(String.Format("[{0}:TOTALPAID:", lPadTotal), FormatCurrency(paidVal))).PadRight(CInt(rPadTotal)).Replace("]", "")

            saveString += String.Concat(Environment.NewLine, Environment.NewLine, "Forma de Pagamento:", Environment.NewLine)

            saveString += If(CInt(dto.TxtBankIn) > 0, "Boleto:".PadRight(10) & FormatCurrency(dto.TxtBankIn.Replace(".", ",")) & Environment.NewLine, "")
            saveString += If(CInt(dto.TxtCheckIn) > 0, "Cheque:".PadRight(10) & FormatCurrency(dto.TxtCheckIn.Replace(".", ",")) & Environment.NewLine, "")
            saveString += If(CInt(dto.TxtCardIn) > 0, "Cartao:".PadRight(10) & FormatCurrency(dto.TxtCardIn.Replace(".", ",")) & Environment.NewLine, "")
            saveString += If(CInt(dto.TxtDebitIn) > 0, "Debito:".PadRight(10) & FormatCurrency(dto.TxtDebitIn.Replace(".", ",")) & Environment.NewLine, "")
            saveString += If(CInt(dto.TxtCashIn) > 0, "Dinheiro:".PadRight(10) & FormatCurrency(dto.TxtCashIn.Replace(".", ",")) & Environment.NewLine, "")

            Dim change = If(paidVal > total, (paidVal - total), 0)
            saveString += "Troco:".PadRight(10) & FormatCurrency(change)
            'saveString += String.Concat(Environment.NewLine & "TROCO:".PadRight(CInt(rPadTotal)), Localization.GetString("Change", LocalResourceFile).Replace(String.Format("[{0}:CHANGE:", lPadTotal), IIf((paidVal - Total) > 0, FormatCurrency(paidVal - Total), FormatCurrency(0)))).PadRight(CInt(rPadTotal)).Replace("]", "")

            If estimateInfo.PayCondType.Length > 1 Then

                saveString += String.Format("{0}{0}{1}{0}", Environment.NewLine, "Condicao de Pagamento:")

                saveString += String.Concat(Environment.NewLine, dto.TxtConditionColumnHeader.Replace("[BR]", Environment.NewLine))

                Dim padcheck = Right(dto.TxtCheck, 3).TrimStart("0"c).Replace("]", "")
                saveString += String.Concat(Environment.NewLine, ReplaceAccentletters(estimateInfo.PayCondType).PadRight(padcheck))

                Dim padPayQty = Right(dto.TxtPayQty, 3).TrimStart("0"c).Replace("]", "")
                saveString += Left(dto.TxtPayQty.Replace("[", ""), "PAYQTY".Length).Replace("PAYQTY", estimateInfo.PayCondN).PadRight(padPayQty)

                Dim padResultPayment = Right(dto.TxtPayments, 3).TrimStart("0"c).Replace("]", "")
                saveString += Left(dto.TxtPayments.Replace("[", ""), "PAYMENTS".Length).Replace("PAYMENTS", FormatCurrency(estimateInfo.PayCondInst)).PadRight(padResultPayment) & Environment.NewLine

                Dim padInitialPay = Right(dto.TxtInitialPay, 3).TrimStart("0"c).Replace("]", "")
                saveString += Left(dto.TxtInitialPay.Replace("[", ""), "INITIALPAY".Length).Replace("INITIALPAY", FormatCurrency(estimateInfo.PayCondIn)).PadRight(padInitialPay)

                Dim padInterest = Right(dto.TxtInterest, 3).TrimStart("0"c).Replace("]", "")
                saveString += Left(dto.TxtInterest.Replace("[", ""), "INTEREST".Length).Replace("INTEREST", FormatPercent(estimateInfo.PayCondPerc / 100)).PadRight(padInterest)

                Dim padTotalPays = Right(dto.TxtTotalPays, 3).TrimStart("0"c).Replace("]", "")
                saveString += Left(dto.TxtTotalPays.Replace("[", ""), "TOTALPAYS".Length).Replace("TOTALPAYS", FormatCurrency(estimateInfo.TotalPayCond)).PadRight(padTotalPays)

                saveString += If(estimateInfo.PayCondInterval > 0, CStr(estimateInfo.PayCondInterval), If(estimateInfo.PayCondInterval < 0, "A Vista", "Mensal"))

            End If

            'saveString += String.Format("{0}{0}{1}{0}", Environment.NewLine, "Observacoes Importantes:")
            'Dim estimateComm = RemoveHtmlTags(ReplaceAccentletters(HttpUtility.HtmlDecode(estimate_Info.Inst))).Replace("[BR]", Environment.NewLine).Replace("<br />", Environment.NewLine)
            'Dim strTerms = RemoveHtmlTags(ReplaceAccentletters(HttpUtility.HtmlDecode(Portals.PortalController.GetPortalSetting("RIW_EstimateTerm", dto.PortalId, ""))))
            'saveString += CStr(IIf(estimateComm.Length > 0, String.Format("{0}{1}{0}", Environment.NewLine, estimateComm), "")) & String.Format("{0}{1}", Environment.NewLine, strTerms)

            If Not Null.IsNull(estimateInfo.Comment) Then
                saveString += String.Format("{0}{0}{1}{0}{0}{2}",
                                            Environment.NewLine,
                                            "Observacoes Importantes:",
                                            ReplaceAccentletters(estimateInfo.Comment).Replace("<p>", Environment.NewLine).Replace("<br />", Environment.NewLine).Replace("</p>", Environment.NewLine).Replace("[BR]", Environment.NewLine))
            End If

            'Dim file_txt = String.Format("Orcamento_{0}_{1}.txt", dto.EstimateId, Replace(DateTime.Now.ToString(), ":", "-").Replace("/", "-").Replace(" ", ""))

            Dim destinationPath = FolderManager.Instance.GetFolder(dto.PortalId, "downloads")

            If destinationPath Is Nothing Then
                FolderManager.Instance.AddFolder(dto.PortalId, "downloads")
                FolderManager.Instance.Synchronize(dto.PortalId)
                destinationPath = FolderManager.Instance.GetFolder(dto.PortalId, "downloads")
            End If

            Dim filePath = String.Format("{0}recibo_{1}.txt", destinationPath.PhysicalPath, estimateInfo.EstimateId.ToString()) ', Replace(Now.ToString("dd-MM-yyyy HH"), ":", "-").Replace("/", "-").Replace(" ", "_"))

            If System.IO.File.Exists(filePath) Then
                System.IO.File.Delete(filePath)
            End If

            Using sw As New StreamWriter(filePath, True)
                sw.WriteLine(saveString)
                sw.Close()
            End Using

            'Dim result = New HttpResponseMessage(HttpStatusCode.OK)
            'result.Content = New StringContent(saveString)
            'result.Content.Headers.ContentType = New MediaTypeHeaderValue("application/octet-stream")
            'result.Content.Headers.ContentDisposition = New ContentDispositionHeaderValue("attachment")
            'result.Content.Headers.ContentDisposition.FileName = file_txt

            'Return response
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets a list of estimates
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <param name="portalId"></param>
    ''' <param name="salesRep"></param>
    ''' <param name="statusId"></param>
    ''' <param name="sDate"></param>
    ''' <param name="eDate"></param>
    ''' <param name="filter"></param>
    ''' <param name="filterField"></param>
    ''' <param name="getAll">Get All</param>
    ''' <param name="orderDesc">Order Direction</param>
    ''' <param name="userId">User ID</param>
    ''' <param name="isDeleted"></param>
    ''' <param name="pageIndex"></param>
    ''' <param name="pageSize"></param>
    ''' <param name="orderBy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetDavs(Optional personId As Integer = -1, Optional pageIndex As Integer = 1, Optional portalId As Integer = 0,
                     Optional getAll As String = "False", Optional pageSize As Integer = 10, Optional orderBy As String = "",
                     Optional orderDesc As String = "") As HttpResponseMessage
        Try
            Dim estimatesCtrl As New EstimatesRepository

            Dim davs = estimatesCtrl.GetDavs(portalId, personId, getAll, pageIndex, pageSize, orderBy, orderDesc)

            Dim total = Nothing
            For Each item In davs
                total = item.TotalRows
                Exit For
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.data = davs, .total = total})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets an estimate by estimate id
    ''' </summary>
    ''' <param name="estimateId">Estimate ID</param>
    ''' <param name="portalId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetEstimate(estimateId As Integer, portalId As Integer, Optional userId As Integer = -1, Optional getAll As Boolean = False) As HttpResponseMessage
        Try
            Dim searchStr As String = HttpContext.Current.Request.Params("filter[filters][0][value]")

            Dim estimate As New Estimate
            Dim estimateCtrl As New EstimatesRepository

            If searchStr <> "" Then
                estimateId = searchStr
            End If
            estimate = estimateCtrl.GetEstimate(estimateId, portalId, getAll)

            If Not Null.IsNull(estimate) Then
                If userId > 0 Then
                    Notifications.RemoveEstimateEntryNotification(Constants.ContentTypeName.RIW_Estimate, Constants.NotificationTypeName.RIW_Estimate_Entry, estimateId, userId)
                    Notifications.RemoveEstimateEntryNotification(Constants.ContentTypeName.RIW_Estimate, Constants.NotificationTypeName.RIW_Estimate_Updated, estimateId, userId)
                End If
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .Estimate = estimate})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets an estimate by estimate id
    ''' </summary>
    ''' <param name="estimateId">Estimate ID</param>
    ''' <param name="portalId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetQuickEstimate(estimateId As Integer, portalId As Integer) As HttpResponseMessage
        Try

            Dim estimate As New Estimate
            Dim estimateCtrl As New EstimatesRepository

            estimate = estimateCtrl.GetEstimate(estimateId, portalId, False)

            estimate.EstimateItems = estimateCtrl.GetEstimateItems(portalId, estimateId, "pt-BR")

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .Estimate = estimate})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets list of estimate items by estimate id
    ''' </summary>
    ''' <param name="estimateId">Estimate ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetEstimateItems(portalId As Integer, estimateId As Integer, lang As String) As HttpResponseMessage
        Try
            Dim estimateItemsCtrl As New EstimatesRepository

            Dim estimateItems = estimateItemsCtrl.GetEstimateItems(portalId, estimateId, lang)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .Items = estimateItems})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets a list of estimates
    ''' </summary>
    ''' <param name="salesRep"></param>
    ''' <param name="personId">Person ID</param>
    ''' <param name="portalId"></param>
    ''' <param name="statusId"></param>
    ''' <param name="userId">User ID</param>
    ''' <param name="sDate"></param>
    ''' <param name="eDate"></param>
    ''' <param name="filter"></param>
    ''' <param name="filterField"></param>
    ''' <param name="getAll">Get All</param>
    ''' <param name="isDeleted"></param>
    ''' <param name="pageIndex"></param>
    ''' <param name="pageSize"></param>
    ''' <param name="orderBy"></param>
    ''' <param name="orderDesc">Order Direction</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetEstimates(salesRep As Integer,
                          Optional personId As Integer = -1,
                          Optional pageIndex As Integer = 1,
                          Optional portalId As Integer = 0,
                          Optional userId As Integer = -1,
                          Optional statusId As Integer = -1,
                          Optional filterDates As String = "ALL",
                          Optional sDate As String = Nothing,
                          Optional eDate As String = Nothing,
                          Optional filter As String = "",
                          Optional filterField As String = "",
                          Optional getAll As String = "",
                          Optional isDeleted As String = "",
                          Optional pageSize As Integer = 10,
                          Optional orderBy As String = "",
                          Optional orderDesc As String = "") As HttpResponseMessage
        Try
            Dim estimatesCtrl As New EstimatesRepository

            Dim searchStr As String = HttpContext.Current.Request.Params("filter[filters][0][value]")
            If searchStr IsNot Nothing Then
                filter = searchStr
            End If

            If sDate = Nothing Then
                sDate = Null.NullDate
            End If

            If eDate = Nothing Then
                eDate = Null.NullDate
            End If

            Dim estimates = estimatesCtrl.GetEstimates(portalId, personId, userId, salesRep, statusId, filterDates, sDate, eDate, filter.Replace("""", ""), filterField.Replace("""", ""), getAll, isDeleted, pageIndex, pageSize, orderBy, orderDesc)

            Dim total = Nothing
            For Each item In estimates
                total = item.TotalRows
                Exit For
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.data = estimates, .total = total})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets list estimate history
    ''' </summary>
    ''' <param name="estimateId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetHistories(estimateId As Integer) As HttpResponseMessage
        Try
            Dim estimateCtrl As New EstimatesRepository

            Dim histories As New List(Of EstimateHistory)

            Dim estimateHistories = estimateCtrl.GetEstimateHistories(estimateId)

            For Each history In estimateHistories
                Dim estimateHistoryComments = estimateCtrl.GetEstimateHistoryComments(history.EstimateHistoryId)
                history.HistoryComments = estimateHistoryComments
                histories.Add(history)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, histories)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets list estimate history
    ''' </summary>
    ''' <param name="estimateId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetMessages(estimateId As Integer) As HttpResponseMessage
        Try
            Dim estimateCtrl As New EstimatesRepository

            Dim messages As New List(Of EstimateMessage)

            Dim estimateMessages = estimateCtrl.GetEstimateMessages(estimateId)

            For Each message In estimateMessages
                Dim estimateMessageComments = estimateCtrl.GetEstimateMessageComments(message.EstimateMessageId)
                message.MessageComments = estimateMessageComments
                messages.Add(message)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, messages)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    <DnnAuthorize>
    <HttpGet>
    Function GetMoreMessageComments(messageId As Integer) As HttpResponseMessage
        Try
            Dim estimateCtrl As New EstimatesRepository

            Dim commentsData = estimateCtrl.GetEstimateMessageComments(messageId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.MessageComments = commentsData})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Saves person documents
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous>
    <HttpPost>
    Async Function PostDav() As Task(Of HttpResponseMessage)
        Try
            If Not Me.Request.Content.IsMimeMultipartContent("form-data") Then
                Throw New HttpResponseException(New HttpResponseMessage(HttpStatusCode.UnsupportedMediaType))
            End If

            Dim portalCtrl = New PortalController()
            Dim request As HttpRequestMessage = Me.Request

            Await request.Content.LoadIntoBufferAsync()
            Dim task = request.Content.ReadAsMultipartAsync()
            Dim result = Await task
            Dim contents = result.Contents
            Dim httpContent As HttpContent = contents.Last()
            Dim uploadedFileMediaType As String = httpContent.Headers.ContentType.MediaType

            Dim portalId = contents(0).ReadAsStringAsync().Result
            Dim userId = contents(2).ReadAsStringAsync().Result
            Dim theDate = contents(3).ReadAsStringAsync().Result

            Dim root = String.Format("{0}{1}", portalCtrl.GetPortal(portalId).HomeDirectoryMapPath, contents(1).ReadAsStringAsync().Result)

            Dim theFile As Stream = httpContent.ReadAsStreamAsync().Result

            Dim theGuid = Guid.NewGuid().ToString()

            'Dim theFileName = String.Format("{0}.{1}", httpContent.Headers.ContentDisposition.FileName, MediaTypeExtensionMap(uploadedFileMediaType))
            'Dim theFileName = If(Not String.IsNullOrWhiteSpace(httpContent.Headers.ContentDisposition.FileName), httpContent.Headers.ContentDisposition.FileName, theGuid).Replace("""", String.Empty)
            Dim theFilePath = Path.Combine(root, httpContent.Headers.ContentDisposition.FileName.Replace("""", String.Empty))

            If theFile.CanRead Then
                Using theFileStream As New FileStream(theFilePath, FileMode.Create)
                    theFile.CopyTo(theFileStream)
                End Using
            End If

            Dim xmlDoc As New XmlDocument()
            xmlDoc.Load(theFilePath)

            Dim no As XmlNode = xmlDoc.SelectSingleNode("CUPOM/ROWDATA/ROW")

            Dim estimate As New Estimate
            Dim estimateCtrl As New EstimatesRepository

            estimate = estimateCtrl.GetEstimate(no.Attributes("SEQUENCIA_DAV").Value(), portalId, True)
            estimate.NumDoc = no.Attributes("NUM_DOC").Value()
            estimate.SequenciaDav = no.Attributes("SEQUENCIA_DAV").Value()
            estimate.Coupon = no.Attributes("COO").Value().TrimStart("0")
            estimate.CouponAttached = no.Attributes("COO_VINCULADO").Value()
            estimate.Ccf = no.Attributes("CCF").Value().TrimStart("0")
            estimate.SaleDate = no.Attributes("DATA_MOVIMENTO").Value()
            estimate.Canceled = If(no.Attributes("CANCELADO").Value() = "N", False, True)
            estimate.PersonId = no.Attributes("COD_CLIENTE").Value()
            estimate.SalesRep = no.Attributes("COD_VENDEDOR").Value()
            estimate.Discount = no.Attributes("OUTROS_DESCONTOS").Value()
            estimate.Extras = no.Attributes("OUTROS_ACRESCIMOS").Value()
            estimate.CashAmount = no.Attributes("VL_DINHEIRO").Value()
            estimate.ChequeAmount = no.Attributes("VL_CHEQUE_VISTA").Value()
            estimate.ChequePreAmount = no.Attributes("VL_CHEQUE_PRE").Value()
            estimate.CardAmount = no.Attributes("VL_CARTAO").Value()
            estimate.CreditAmount = no.Attributes("VL_CREDIARIO").Value()
            estimate.CovenantAmount = no.Attributes("VL_CONVENIO").Value()
            estimate.TicketAmount = no.Attributes("VL_VALE").Value()
            estimate.PayCondId = no.Attributes("COD_COND_PAGTO").Value()
            estimate.ModifiedByUser = userId
            estimate.ModifiedOnDate = theDate
            estimateCtrl.UpdateEstimate(estimate)

            FolderManager.Instance().Synchronize(contents(0).ReadAsStringAsync().Result)

            Return request.CreateResponse(HttpStatusCode.Created, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message}))
        End Try
    End Function

    ''' <summary>
    ''' Saves person documents
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous>
    <HttpPost>
    Async Function PostDavItem() As Task(Of HttpResponseMessage)
        Try
            If Not Me.Request.Content.IsMimeMultipartContent("form-data") Then
                Throw New HttpResponseException(New HttpResponseMessage(HttpStatusCode.UnsupportedMediaType))
            End If

            Dim portalCtrl = New PortalController()
            Dim request As HttpRequestMessage = Me.Request

            Await request.Content.LoadIntoBufferAsync()
            Dim task = request.Content.ReadAsMultipartAsync()
            Dim result = Await task
            Dim contents = result.Contents
            Dim httpContent As HttpContent = contents.Last()
            Dim uploadedFileMediaType As String = httpContent.Headers.ContentType.MediaType

            Dim portalId = contents(0).ReadAsStringAsync().Result
            Dim userId = contents(2).ReadAsStringAsync().Result
            Dim theDate = contents(3).ReadAsStringAsync().Result

            Dim root = String.Format("{0}{1}", portalCtrl.GetPortal(portalId).HomeDirectoryMapPath, contents(1).ReadAsStringAsync().Result)

            Dim theFile As Stream = httpContent.ReadAsStreamAsync().Result

            Dim theGuid = Guid.NewGuid().ToString()

            'Dim theFileName = String.Format("{0}.{1}", httpContent.Headers.ContentDisposition.FileName, MediaTypeExtensionMap(uploadedFileMediaType))
            'Dim theFileName = If(Not String.IsNullOrWhiteSpace(httpContent.Headers.ContentDisposition.FileName), httpContent.Headers.ContentDisposition.FileName, theGuid).Replace("""", String.Empty)
            Dim theFilePath = Path.Combine(root, httpContent.Headers.ContentDisposition.FileName.Replace("""", String.Empty))

            If theFile.CanRead Then
                Using theFileStream As New FileStream(theFilePath, FileMode.Create)
                    theFile.CopyTo(theFileStream)
                End Using
            End If

            Dim xmlDoc As New XmlDocument()
            xmlDoc.Load(theFilePath)

            Dim no As XmlNode = xmlDoc.SelectSingleNode("CUPOM/ROWDATA/ROW")

            Dim estimate As New Estimate
            Dim estimateCtrl As New EstimatesRepository

            estimate = estimateCtrl.GetEstimate(no.Attributes("SEQUENCIA_DAV").Value(), portalId, True)
            estimate.NumDoc = no.Attributes("NUM_DOC").Value()
            estimate.SequenciaDav = no.Attributes("SEQUENCIA_DAV").Value()
            estimate.Coupon = no.Attributes("COO").Value().TrimStart("0")
            estimate.CouponAttached = no.Attributes("COO_VINCULADO").Value()
            estimate.Ccf = no.Attributes("CCF").Value().TrimStart("0")
            estimate.SaleDate = no.Attributes("DATA_MOVIMENTO").Value()
            estimate.Canceled = If(no.Attributes("CANCELADO").Value() = "N", False, True)
            estimate.PersonId = no.Attributes("COD_CLIENTE").Value()
            estimate.SalesRep = no.Attributes("COD_VENDEDOR").Value()
            estimate.Discount = no.Attributes("OUTROS_DESCONTOS").Value()
            estimate.Extras = no.Attributes("OUTROS_ACRESCIMOS").Value()
            estimate.CashAmount = no.Attributes("VL_DINHEIRO").Value()
            estimate.ChequeAmount = no.Attributes("VL_CHEQUE_VISTA").Value()
            estimate.ChequePreAmount = no.Attributes("VL_CHEQUE_PRE").Value()
            estimate.CardAmount = no.Attributes("VL_CARTAO").Value()
            estimate.CreditAmount = no.Attributes("VL_CREDIARIO").Value()
            estimate.CovenantAmount = no.Attributes("VL_CONVENIO").Value()
            estimate.TicketAmount = no.Attributes("VL_VALE").Value()
            estimate.PayCondId = no.Attributes("COD_COND_PAGTO").Value()
            estimate.ModifiedByUser = userId
            estimate.ModifiedOnDate = theDate
            estimateCtrl.UpdateEstimate(estimate)

            FolderManager.Instance().Synchronize(contents(0).ReadAsStringAsync().Result)

            Return request.CreateResponse(HttpStatusCode.Created, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message}))
        End Try
    End Function

    ''' <summary>
    ''' Deletes an estimate
    ''' </summary>
    ''' <param name="jsonData">Estimates Models</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpDelete>
    Function RemoveEstimate(estimates As List(Of Estimate)) As HttpResponseMessage
        Try
            Dim estimateCtrl As New EstimatesRepository

            For Each estimate In estimates

                estimateCtrl.RemoveEstimate(estimate.EstimateId, PortalController.Instance.GetCurrentPortalSettings().PortalId)

                Dim estimateItems = estimateCtrl.GetEstimateItems(PortalController.Instance.GetCurrentPortalSettings().PortalId, estimate.EstimateId, "pt-BR")

                For Each item In estimateItems
                    estimateCtrl.RemoveEstimateItem(item)
                Next

            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes an estimate item
    ''' </summary>
    ''' <param name="jsonData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function RemoveEstimateItems(jsonData As EstimateItemsRequest) As HttpResponseMessage
        Try
            Dim estimateHistory As New EstimateHistory
            Dim estimateRemovedItem As New EstimateItemRemoved
            Dim estimateCtrl As New EstimatesRepository

            For Each item In jsonData.EstimateItemsRemoved
                estimateRemovedItem.ProductId = item.ProductId
                estimateRemovedItem.ProductQty = item.ProductQty
                estimateRemovedItem.RemoveReasonId = item.RemoveReasonId
                estimateRemovedItem.EstimateId = item.EstimateId
                estimateRemovedItem.CreatedByUser = item.CreatedByUser
                estimateRemovedItem.CreatedOnDate = item.CreatedOnDate

                estimateCtrl.AddEstimateItemRemoved(estimateRemovedItem)

                estimateCtrl.RemoveEstimateItem(item.EstimateItemId, item.EstimateId)

                estimateHistory.EstimateId = item.EstimateId
                estimateHistory.HistoryText = String.Format("<p>Item ""{0}"" removido (Qde: {1}).</p>", item.ProductName, item.ProductQty)
                estimateHistory.Locked = True
                estimateHistory.CreatedByUser = 0
                estimateHistory.CreatedOnDate = item.CreatedOnDate

                estimateCtrl.AddEstimateHistory(estimateHistory)
            Next

            Dim estimate As New Estimate

            estimate = estimateCtrl.GetEstimate(jsonData.EstimateId, jsonData.PortalId, True)
            estimate.Discount = jsonData.Discount
            estimate.TotalAmount = jsonData.TotalAmount
            'estimate.PayCondType = ""
            'estimate.PayCondN = 0
            'estimate.PayCondPerc = 0
            'estimate.PayCondIn = 0
            'estimate.PayCondInst = 0
            'estimate.PayCondInterval = 0
            'estimate.TotalPayments = 0
            'estimate.TotalPayCond = 0
            estimate.Comment = jsonData.Comment
            estimate.ModifiedByUser = jsonData.ModifiedByUser
            estimate.ModifiedOnDate = jsonData.ModifiedOnDate

            estimateCtrl.UpdateEstimate(estimate)

            If jsonData.ConnId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(jsonData.ConnId).pushEstimate()
                'Hub.Clients.AllExcept(jsonData.ConnId).pushEstimate()

                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(jsonData.ConnId).pushHistory(estimateHistory)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes estimate message comment
    ''' </summary>
    ''' <param name="commentId"></param>
    ''' <param name="estimateHistoryId"></param>
    ''' <param name="connId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpDelete>
    Function RemoveHistoryComment(commentId As Integer, estimateHistoryId As Integer, connId As String) As HttpResponseMessage
        Try
            Dim estimateHistoryCommentCtrl As New EstimatesRepository

            estimateHistoryCommentCtrl.RemoveEstimateHistoryComment(commentId, estimateHistoryId)

            If connId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(connId).RemoveHistoryComment(commentId, estimateHistoryId)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes estimate message
    ''' </summary>
    ''' <param name="estimateMessageId"></param>
    ''' <param name="estimateId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpDelete>
    Function RemoveMessage(estimateMessageId As Integer, estimateId As Integer, connId As String) As HttpResponseMessage
        Try
            Dim estimateMessageCtrl As New EstimatesRepository

            estimateMessageCtrl.RemoveEstimateMessage(estimateMessageId, estimateId)

            If connId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(connId).RemoveMessage(estimateMessageId, estimateId)
                'Hub.Clients.AllExcept(MessageComment.connId).pushComment(estimateMessageComment, MessageComment.messageIndex)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes estimate message comment
    ''' </summary>
    ''' <param name="commentId"></param>
    ''' <param name="estimateMessageId"></param>
    ''' <param name="connId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpDelete>
    Function RemoveMessageComment(commentId As Integer, estimateMessageId As Integer, connId As String) As HttpResponseMessage
        Try
            Dim estimateMessageCommentCtrl As New EstimatesRepository

            estimateMessageCommentCtrl.RemoveEstimateMessageComment(commentId, estimateMessageId)

            If connId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(connId).RemoveMessageComment(commentId, estimateMessageId)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates estimate pay form and condition
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function RestoreEstimate(dto As Estimate) As HttpResponseMessage
        Try
            Dim estimateHistory As New EstimateHistory
            Dim estimate As New Estimate
            Dim estimateCtrl As New EstimatesRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, False)

            estimate.StatusId = 1
            estimate.ModifiedByUser = dto.ModifiedByUser
            estimate.ModifiedOnDate = dto.ModifiedOnDate

            estimateCtrl.UpdateEstimate(estimate)

            estimateHistory.EstimateId = estimate.EstimateId
            estimateHistory.HistoryText = "<p>Orçamento re-inicializado.</p>"
            estimateHistory.Locked = False
            estimateHistory.CreatedByUser = dto.ModifiedByUser
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            Dim product As New Product
            Dim productCtrl As New ProductsRepository

            Dim estimateItems = estimateCtrl.GetEstimateItems(dto.PortalId, dto.EstimateId, "pt-BR")

            For Each item In estimateItems

                product = productCtrl.GetProduct(item.ProductId, "pt-BR")

                Dim oldStock = product.QtyStockSet

                product.QtyStockSet = oldStock + item.ProductQty
                product.ModifiedByUser = dto.ModifiedByUser
                product.ModifiedOnDate = dto.ModifiedOnDate

                productCtrl.UpdateProduct(product)

            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .Estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, False)})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Returns product to stock
    ''' </summary>
    ''' <param name="products"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function ReturnProductStock(products As List(Of Product)) As HttpResponseMessage
        Try
            Dim product As New Product
            Dim productCtrl As New ProductsRepository

            For Each item In products
                product = productCtrl.GetProduct(item.ProductId, "pt-BR")

                Dim oldStock = product.QtyStockSet

                product.QtyStockSet = oldStock + item.QtyStockSet
                product.ModifiedByUser = item.ModifiedByUser
                product.ModifiedOnDate = item.ModifiedOnDate

                productCtrl.UpdateProduct(product)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Saves dav to SGI
    ''' </summary>
    ''' <param name="estimateId">Estimate ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function SaveDav(estimateId As Integer) As HttpResponseMessage
        Try
            Dim estimateCtrl As New EstimatesRepository
            Dim davsCtrl As New SofterRepository
            Dim dav As New Softer

            Dim estimate = estimateCtrl.GetEstimate(estimateId, 0, True)

            If estimate IsNot Nothing Then

                Dim existingDav = davsCtrl.GetSGIDav(estimate.NumDav)

                If existingDav IsNot Nothing Then
                    Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "info"})
                Else

                    dav.codicli = estimate.PersonId
                    dav.codiven = estimate.SalesRep
                    dav.total = estimate.TotalAmount
                    dav.datavenda = String.Format("{0:dd/MM/yyyy H:mm:ss}", estimate.SaleDate)
                    dav.status = Null.NullInteger
                    If estimate.PayCondId > 0 Then
                        dav.codpay = estimate.PayCondId
                    Else
                        dav.codpay = 1
                    End If
                    dav.valordin = estimate.CashAmount
                    dav.vrcartao = estimate.CardAmount
                    dav.vrcartao = IIf(estimate.DebitAmount > 0, estimate.DebitAmount, estimate.CardAmount)
                    dav.vrcred = estimate.TotalBank
                    dav.convenio = 0
                    dav.desconto = estimate.TotalAmount * estimate.Discount / 100
                    'dav.descperc = estimate.Discount
                    dav.observacao = estimate.Comment + estimate.Inst
                    dav.opercard = -1
                    dav.incres = 0
                    dav.datavenda = estimate.CreatedOnDate

                    Dim numDav = davsCtrl.SaveSGIDAV(dav)

                    estimate.NumDav = numDav
                    estimateCtrl.UpdateEstimate(estimate)

                    Dim davItems = estimateCtrl.GetEstimateItems(estimate.EstimateId)

                    For Each item In davItems

                        Dim davItem As New Softer

                        davItem.numdav = numDav
                        davItem.codproduto = item.ProductId
                        davItem.valorunitario = item.ProductEstimatePrice
                        davItem.quantidade = item.ProductQty
                        davItem.descontoperc = item.ProductDiscount

                        Dim numDavItem = davsCtrl.SaveSGIDAVItem(davItem)

                        item.NumDav = numDav
                        item.NumDavItem = numDavItem
                        estimateCtrl.UpdateEstimateItem(item)

                    Next

                    davsCtrl.UpdateSGIDAV(numDav)

                End If

            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Sends client notification about estimate being updated
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function SendAdminEstimateNotification(dto As EstimatePdf) As HttpResponseMessage
        Try
            Dim portalCtrl = New Portals.PortalController()
            Dim portalInfo = portalCtrl.GetPortal(dto.PortalId)
            Dim myPortalSettings = PortalController.Instance.GetPortalSettings(portalInfo.PortalID)
            'Dim portalEmail = Portals.PortalController.GetPortalSetting("RIW_StoreEmail", portalInfo.PortalID, portalInfo.Email)

            Dim settingCtrl As New SettingsRepository
            Dim settingsDictionay = settingCtrl.GetSettings(dto.PortalId)

            Dim recipientList As New List(Of Users.UserInfo)

            Dim personCtrl As New PeopleRepository
            Dim personInfo As New Users.UserInfo()

            Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = settingsDictionay("storeEmail"), .DisplayName = portalInfo.PortalName}

            Dim clientInfo = personCtrl.GetPerson(dto.PersonId, dto.PortalId, dto.UserId)

            If clientInfo.UserId > 0 Then
                personInfo = UserController.GetUserById(dto.PortalId, clientInfo.UserId)
            Else
                personInfo = New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = clientInfo.Email, .DisplayName = clientInfo.DisplayName}
            End If

            recipientList.Add(personInfo)

            Dim salesInfo = UserController.GetUserById(portalInfo.PortalID, dto.SalesPersonId)

            Dim mm As New Net.Mail.MailMessage() With {.Subject = dto.Subject, .Body = dto.MessageBody, .IsBodyHtml = True,
                                                       .From = New Net.Mail.MailAddress(storeUser.Email, salesInfo.DisplayName)}
            mm.ReplyToList.Add(New Net.Mail.MailAddress(salesInfo.Email, salesInfo.DisplayName))

            Dim email As New Thread(Sub() PostOffice.SendMail(mm, recipientList, settingsDictionay("smtpServer"), settingsDictionay("smtpPort"),
                                                              settingsDictionay("smtpConnection"), settingsDictionay("smtpLogin"),
                                                              settingsDictionay("smtpPassword"))) With {.IsBackground = True}
            email.Start()

            'Dim sentMails = distList.SendMail(mailMessage, recipientList, SettingsDictionay("smtpServer"), CInt(SettingsDictionay("smtpPort")), CBool(SettingsDictionay("smtpConnection")), SettingsDictionay("smtpLogin"), SettingsDictionay("smtpPassword"))

            Dim estimateHistory As New EstimateHistory
            Dim estimateCtrl As New EstimatesRepository

            estimateHistory.EstimateId = dto.EstimateId
            estimateHistory.HistoryText = "<p>Notificação enviada ao cliente via email.</p>"
            estimateHistory.Locked = True
            estimateHistory.CreatedByUser = 0
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            Notifications.SendNotification(Constants.ContentTypeName.RIW_Estimate,
                                           Constants.NotificationTypeName.RIW_Estimate_Entry,
                                           personInfo.UserID, salesInfo, "Gerentes", dto.PortalId, dto.EstimateId, String.Format("Novo Orçamento ({0})", dto.EstimateId),
                                           String.Format("Novo oçamento gerado por: <br /><br /><strong>Nome:</strong> {0}<br /><strong>Email:</strong> {1}<br /><br />Clique neste {2} para acessar o orçamento.",
                                                         personInfo.DisplayName, personInfo.Email, dto.EstimateLink.Replace("<p>", "").Replace("</p>", "")))

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"}) ', .SentEmail = sentMails})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Sends client notification about estimate being updated
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function SendClientEstimateNotification(dto As EstimatePdf) As HttpResponseMessage
        Try
            Dim portalCtrl = New Portals.PortalController()
            Dim portalInfo = portalCtrl.GetPortal(dto.PortalId)
            Dim myPortalSettings = PortalController.Instance.GetPortalSettings(portalInfo.PortalID)
            'Dim portalEmail = Portals.PortalController.GetPortalSetting("RIW_StoreEmail", portalInfo.PortalID, portalInfo.Email)

            Dim settingCtrl As New SettingsRepository
            Dim settingsDictionay = settingCtrl.GetSettings(dto.PortalId)

            Dim recipientList As New List(Of Users.UserInfo)

            Dim personCtrl As New PeopleRepository
            Dim personInfo As New Users.UserInfo()

            Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = settingsDictionay("storeEmail"), .DisplayName = portalInfo.PortalName}

            Dim clientInfo = personCtrl.GetPerson(dto.PersonId, dto.PortalId, dto.UserId)

            If clientInfo.UserId > 0 Then
                personInfo = UserController.GetUserById(dto.PortalId, clientInfo.UserId)
            Else
                personInfo = New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = clientInfo.Email, .DisplayName = clientInfo.DisplayName}
            End If

            recipientList.Add(personInfo)

            Dim salesInfo = UserController.GetUserById(portalInfo.PortalID, dto.SalesPersonId)

            Dim mm As New Net.Mail.MailMessage() With {.Subject = dto.Subject, .Body = dto.MessageBody, .IsBodyHtml = True,
                                                       .From = New Net.Mail.MailAddress(storeUser.Email, salesInfo.DisplayName)}
            mm.ReplyToList.Add(New Net.Mail.MailAddress(salesInfo.Email, salesInfo.DisplayName))

            Dim email As New Thread(Sub() PostOffice.SendMail(mm, recipientList, settingsDictionay("smtpServer"), settingsDictionay("smtpPort"),
                                                              settingsDictionay("smtpConnection"), settingsDictionay("smtpLogin"),
                                                              settingsDictionay("smtpPassword"))) With {.IsBackground = True}
            email.Start()

            'Dim sentMails = distList.SendMail(mailMessage, recipientList, SettingsDictionay("smtpServer"), CInt(SettingsDictionay("smtpPort")), CBool(SettingsDictionay("smtpConnection")), SettingsDictionay("smtpLogin"), SettingsDictionay("smtpPassword"))

            Dim estimateHistory As New EstimateHistory
            Dim estimateCtrl As New EstimatesRepository

            estimateHistory.EstimateId = dto.EstimateId
            estimateHistory.HistoryText = "<p>Notificação enviada ao cliente via email.</p>"
            estimateHistory.Locked = True
            estimateHistory.CreatedByUser = 0
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            Notifications.SendNotification(Constants.ContentTypeName.RIW_Estimate,
                                           Constants.NotificationTypeName.RIW_Estimate_Updated,
                                           salesInfo.UserID, personInfo, "", dto.PortalId, dto.EstimateId, String.Format("Seu Orçamento ({0})", dto.EstimateId),
                                           String.Format("Orçamento atualizado:<br /><br />Clique neste {0} para acessar o orçamento.",
                                                         dto.EstimateLink.Replace("<p>", "").Replace("</p>", "")))

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"}) ', .SentEmail = sentMails})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Generate Estimate PDF
    ''' </summary>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function SendEstimatePdf(dto As EstimatePdf) As HttpResponseMessage
        Try
            Dim portalCtrl = New Portals.PortalController()
            Dim estimate As New Estimate
            Dim estimateItems As New List(Of EstimateItem)
            Dim estimateCtrl As New EstimatesRepository
            Dim payCondCtrl As New PayConditionsRepository
            Dim productCtrl As New ProductsRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, True)

            'Using ms As MemoryStream = New MemoryStream()

            Dim str As New MemoryStream()

            'Dim heading12 As Font = FontFactory.GetFont("ARIAL", 12)
            Dim heading10 As Font = FontFactory.GetFont("ARIAL", 10)
            Dim heading9 As Font = FontFactory.GetFont("VERDANA", 9)
            Dim heading8 As Font = FontFactory.GetFont("VERDANA", 8)
            Dim heading8b As Font = FontFactory.GetFont("VERDANA", 8, Font.BOLD)
            Dim heading7 As Font = FontFactory.GetFont("VERDANA", 7)

            Dim myPortalSettings = PortalController.Instance.GetPortalSettings(dto.PortalId)

            'Dim pdfFile = HostingEnvironment.MapPath("\Portals\0\something.pdf")

            Dim thePdf As New Document(PageSize.A4, 20, 20, 100, 15)

            'Dim file As New System.IO.FileStream(pdfFile, System.IO.FileMode.OpenOrCreate)
            'Dim writer As PdfWriter = PdfWriter.GetInstance(thePdf, file)
            Dim writer As PdfWriter = PdfWriter.GetInstance(thePdf, str)

            Dim page As pdfPage = New pdfPage()
            writer.PageEvent = page

            thePdf.Open()

            Dim clientTable = New PdfPTable(2) With {.TotalWidth = 555.0F, .HorizontalAlignment = 0, .LockedWidth = True}
            Dim widthsHeaderTable As Single() = New Single() {1.0F, 1.0F}
            clientTable.SetWidths(widthsHeaderTable)

            Dim clientInfoHeader As New Paragraph("Informações do Cliente", heading10)

            Dim clientInfoLeft As New Paragraph(String.Format("Cliente: {0}", estimate.ClientDisplayName), heading8)
            clientInfoLeft.Add(Environment.NewLine())
            If estimate.ClientTelephone <> "" Then
                clientInfoLeft.Add(String.Format("Telefone: {0}{1}", PhoneMask(estimate.ClientTelephone), Environment.NewLine()))
            End If
            If estimate.ClientEmail <> "" Then
                clientInfoLeft.Add(String.Format("Email: {0}{1}", estimate.ClientEmail, Environment.NewLine()))
            End If
            If estimate.ClientEin <> "" Then
                clientInfoLeft.Add(String.Format("CNPJ: {0}{1}", estimate.ClientEin, Environment.NewLine()))
            End If
            If estimate.ClientSateTax <> "" Then
                clientInfoLeft.Add(String.Format("Inscrição Estadual: {0}{1}", estimate.ClientSateTax, Environment.NewLine()))
            End If
            If estimate.ClientCityTax <> "" Then
                clientInfoLeft.Add(String.Format("Inscrição Municipal: {0}{1}", estimate.ClientCityTax, Environment.NewLine()))
            End If

            Dim clientInfoRight As New Paragraph()
            If estimate.ClientAddress <> "" Then
                'clientInfoRight = New Paragraph(String.Format("Endereço: {0}{1}{2}{3}", estimate.ClientAddress, Space(1), estimate.ClientUnit, Environment.NewLine()), heading8)
                clientInfoRight = New Paragraph(String.Format("{0}{1}{2}{3}", estimate.ClientAddress, Space(1), estimate.ClientUnit, Environment.NewLine()), heading8)
            End If
            If estimate.ClientComplement <> "" Then
                'clientInfoRight.Add(String.Format("Complement: {0}{1}", estimate.ClientComplement, Environment.NewLine()))
                clientInfoRight.Add(String.Format("{0}{1}", estimate.ClientComplement, Environment.NewLine()))
            End If
            If estimate.ClientDistrict <> "" Then
                clientInfoRight.Add(String.Format("Bairro: {0}{1}", estimate.ClientDistrict, Environment.NewLine()))
            End If
            If estimate.ClientCity <> "" Then
                'clientInfoRight.Add(String.Format("Cidade: {0}{1}", estimate.ClientCity, Space(1)))
                clientInfoRight.Add(String.Format("{0}{1}, ", estimate.ClientCity, Space(1)))
            End If
            If estimate.ClientRegion <> "" Then
                'clientInfoRight.Add(String.Format("Estado: {0}{1}", estimate.ClientRegion, Space(1)))
                clientInfoRight.Add(String.Format("{0}{1} ", estimate.ClientRegion, Space(1)))
            End If
            If estimate.ClientPostalCode <> "" Then
                clientInfoRight.Add(String.Format("CEP: {0}", ZipMask(estimate.ClientPostalCode)))
            End If

            Dim clientCell1 = New PdfPCell(clientInfoHeader) With {.PaddingLeft = 7.0F, .PaddingTop = 5.0F, .PaddingBottom = 4.0F, .Colspan = 2, .BackgroundColor = New BaseColor(228, 228, 228)}
            Dim clientCell3 = New PdfPCell(clientInfoLeft) With {.Padding = 7.0F, .ExtraParagraphSpace = 2.0F}
            Dim clientCell4 = New PdfPCell(clientInfoRight) With {.Padding = 7.0F, .ExtraParagraphSpace = 2.0F}

            clientTable.AddCell(clientCell1)
            clientTable.AddCell(clientCell3)
            clientTable.AddCell(clientCell4)

            thePdf.Add(clientTable)

            Dim prodTable As New PdfPTable(dto.ColumnsCount) With {.SpacingBefore = 5.0F, .HorizontalAlignment = 0, .TotalWidth = clientTable.TotalWidth, .LockedWidth = True}
            Dim widthsProdTable As Single()
            If CSng(dto.ProductDiscountValue) > 0 Then
                widthsProdTable = New Single() {1.0F, 2.95F, 0.7F, 1.1F, 0.7F, 1.0F, 1.0F}
            Else
                widthsProdTable = New Single() {1.0F, 2.95F, 0.7F, 1.1F, 0.0F, 1.0F, 1.0F}
            End If
            prodTable.SetWidths(widthsProdTable)

            Dim prodCellHeader As PdfPCell = New PdfPCell(New Phrase("Itens do Orçamento", heading9)) With {.Colspan = dto.ColumnsCount, .PaddingLeft = 7.0F, .PaddingTop = 5.0F, .PaddingBottom = 4.0F, .BackgroundColor = New BaseColor(228, 228, 228)}
            prodTable.AddCell(prodCellHeader)

            For Each column In dto.Columns
                Dim prodCellHeaderColumns As PdfPCell = New PdfPCell(New Phrase(column, heading8)) With {.PaddingTop = 5.0F, .PaddingLeft = 5.0F}
                prodTable.AddCell(prodCellHeaderColumns)
            Next

            estimateItems = estimateCtrl.GetEstimateItems(dto.PortalId, dto.EstimateId, dto.Lang)

            For Each item In estimateItems

                Dim code = ""
                If item.Barcode <> "" Then
                    code = "CB: " & item.Barcode
                ElseIf item.ProductRef <> "" Then
                    code = "REF: " & item.ProductRef
                End If

                Dim prodCellProdRef As PdfPCell = New PdfPCell(New Phrase(code, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1}
                Dim prodCellProdName As PdfPCell = New PdfPCell(New Phrase(item.ProductName, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F}
                Dim prodCellQty As PdfPCell = New PdfPCell(New Phrase(item.ProductQty, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1}
                Dim prodCellEstProdOriginalPrice As PdfPCell = New PdfPCell(New Phrase(FormatCurrency(item.ProductEstimateOriginalPrice), heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1}
                Dim prodCellDiscount As PdfPCell = New PdfPCell(New Phrase(item.ProductDiscount, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1, .HorizontalAlignment = Element.ALIGN_RIGHT}
                Dim prodCellEstProdPrice As PdfPCell = New PdfPCell(New Phrase(FormatCurrency(item.ProductEstimatePrice), heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1, .HorizontalAlignment = Element.ALIGN_RIGHT}
                Dim prodCellExtendedAmount As PdfPCell = New PdfPCell(New Phrase(FormatCurrency(item.ExtendedAmount), heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1, .HorizontalAlignment = Element.ALIGN_RIGHT}

                If Not (item.ItemIndex Mod 2 = 1) Then
                    prodCellProdRef.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellProdName.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellQty.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellEstProdOriginalPrice.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellDiscount.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellEstProdPrice.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellExtendedAmount.BackgroundColor = New BaseColor(195, 195, 195)
                Else
                    prodCellProdRef.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellProdName.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellQty.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellEstProdOriginalPrice.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellDiscount.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellEstProdPrice.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellExtendedAmount.BackgroundColor = New BaseColor(228, 228, 228)
                End If

                prodTable.AddCell(prodCellProdRef)
                prodTable.AddCell(prodCellProdName)
                prodTable.AddCell(prodCellQty)
                prodTable.AddCell(prodCellEstProdOriginalPrice)
                prodTable.AddCell(prodCellDiscount)
                prodTable.AddCell(prodCellEstProdPrice)
                prodTable.AddCell(prodCellExtendedAmount)

                If dto.Expand Then
                    Dim expItem = productCtrl.GetProduct(item.ProductId, "pt-BR") 'Feature_Controller.Get_ProductDetail(CInt(item.GetDataKeyValue("ProdID")))
                    'For Each dRow In expItem
                    If Not expItem.Summary <> "" OrElse expItem.ProductImageId > 0 Then
                        '    Exit For
                        'End If
                        Dim prodCellProdIntro As PdfPCell = New PdfPCell(New Phrase(RemoveHtmlTags(HttpUtility.HtmlDecode(expItem.Summary)), heading7)) With {.PaddingTop = 10.0F, .PaddingLeft = 10.0F, .Border = 0}
                        Dim prodDetailTable As PdfPTable = New PdfPTable(2)
                        Dim widthsDetailTable As Single() = New Single() {1.0F, 3.0F}
                        prodDetailTable.SetWidths(widthsDetailTable)
                        Dim prodCellImage As PdfPCell = New PdfPCell() With {.PaddingTop = 10.0F, .PaddingLeft = 10.0F, .Border = 0}

                        If expItem.ProductImageId > 0 Then
                            Dim jpgProdImage As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(
                            New Uri(String.Format("http://{0}/databaseimages/{1}.{2}?maxwidth=130&maxheight=130{3}",
                                                  PortalSettings.PortalAlias.HTTPAlias, expItem.ProductImageId,
                                                  expItem.Extension, CStr(If(dto.Watermark <> "", "&watermark=outglow&text=" & dto.Watermark, "")))))
                            jpgProdImage.ScaleToFit(70.0F, 40.0F)
                            prodCellImage.AddElement(jpgProdImage)
                        End If

                        Dim prodCellProdDesc As PdfPCell = New PdfPCell(New Phrase(RemoveHtmlTags(HttpUtility.HtmlDecode(expItem.Description)), heading7)) With {.PaddingTop = 10.0F, .PaddingLeft = 10.0F, .Colspan = 2, .Border = 0}
                        prodDetailTable.AddCell(prodCellImage)
                        prodDetailTable.AddCell(prodCellProdIntro)
                        prodDetailTable.AddCell(prodCellProdDesc)
                        Dim prodCellDetail As PdfPCell = New PdfPCell(prodDetailTable) With {.Colspan = dto.Columns.Count}
                        prodTable.AddCell(prodCellDetail)
                        'Next
                    End If
                End If

            Next

            thePdf.Add(prodTable)

            Dim amountTable = New PdfPTable(2) With {.TotalWidth = prodTable.TotalWidth, .SpacingBefore = 5.0F, .LockedWidth = True}
            Dim widthsamountTable As Single() = New Single() {6.0F, 1.0F}
            amountTable.SetWidths(widthsamountTable)

            Dim amountCell = New PdfPCell() With {.Padding = 2.0F}

            'If Not UserInfo.IsInRole("Gerentes") And Not UserInfo.IsInRole("Vendedores") Then
            If dto.EstimateDiscountValue > 0 OrElse dto.ProductDiscountValue > 0 Then
                Dim originalAmountLabel As New Phrase("Valor Original: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(originalAmountLabel) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim originalAmount As New Phrase(String.Format("{0}", FormatCurrency(dto.ProductOriginalAmount)), heading7)
                amountCell = New PdfPCell(originalAmount) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If
            'Else

            If dto.ProductDiscountValue > 0 Then
                Dim productDiscountValueLabel As New Phrase("Desc. Produto: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(productDiscountValueLabel) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim productDiscountValue As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(dto.ProductDiscountValue), Environment.NewLine()), heading7)
                amountCell = New PdfPCell(productDiscountValue) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If

            If dto.EstimateDiscountValue > 0 Then
                Dim estimateDiscountValueLabel As New Phrase("Desc. Orçamento: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(estimateDiscountValueLabel) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim estimateDiscountValue As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(dto.EstimateDiscountValue), Environment.NewLine()), heading7)
                amountCell = New PdfPCell(estimateDiscountValue) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If

            'End If

            If dto.TotalDiscountPerc > 0 Then
                Dim totalDiscountTitleLabel As New Phrase("Desc. Total %: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(totalDiscountTitleLabel) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim totalDiscountTitle As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatPercent(dto.TotalDiscountPerc), Environment.NewLine()), heading7)
                amountCell = New PdfPCell(totalDiscountTitle) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If

            If dto.TotalDiscountValue > 0 Then
                Dim discountLabel As New Phrase("Desc. Total $: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(discountLabel) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim totalDiscountValue As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(dto.TotalDiscountValue), Environment.NewLine()), heading7)
                amountCell = New PdfPCell(totalDiscountValue) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If

            Dim paymentLabel As New Phrase("Valor Final: ", heading8b) With {.Leading = 20.0F}
            amountCell = New PdfPCell(paymentLabel) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            amountTable.AddCell(amountCell)

            Dim payment As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(dto.EstimateTotalAmount), Environment.NewLine()), heading7)
            amountCell = New PdfPCell(payment) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            amountTable.AddCell(amountCell)

            'If UserInfo.IsInRole("Gerentes") Then
            '    Dim MarkUpCurrency_Label As New Phrase("Markup $: ", heading8b) With {.Leading = 20.0F}
            '    amountCell = New PdfPCell(MarkUpCurrency_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            '    amountTable.AddCell(amountCell)

            '    Dim MarkUpCurrency As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(_MarkUpCurrency), Environment.NewLine()), heading7)
            '    amountCell = New PdfPCell(MarkUpCurrency) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            '    amountTable.AddCell(amountCell)

            '    Dim MarkupPerc_Label As New Phrase("Markup %: ", heading8b) With {.Leading = 20.0F}
            '    amountCell = New PdfPCell(MarkupPerc_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            '    amountTable.AddCell(amountCell)

            '    Dim MarkupPerc As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatPercent(_MarkupPerc), Environment.NewLine()), heading7)
            '    amountCell = New PdfPCell(MarkupPerc) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            '    amountTable.AddCell(amountCell)
            'End If

            thePdf.Add(amountTable)

            Dim payCond0 = payCondCtrl.GetPayConds(dto.PortalId, 0, CDec(dto.EstimateTotalAmount))
            If payCond0.Count > 0 Then
                For Each payCondType0 In payCond0
                    Dim payCondTitleType0 As New Paragraph(CStr(If(payCondType0.PayCondDisc > 0, String.Format("{0}{1}{0} Valor com desconto {2}", Space(1), payCondType0.PayCondTitle, FormatCurrency(dto.EstimateTotalAmount - (dto.EstimateTotalAmount / 100 * payCondType0.PayCondDisc))), payCondType0.PayCondTitle)), heading7)
                    thePdf.Add(payCondTitleType0)
                Next
            End If

            If estimate.PayCondType <> "" Then

                Dim payCondChosenTitle As Paragraph = New Paragraph("Condição de Pagamento Escolhida:", heading8b)
                thePdf.Add(payCondChosenTitle)
                Dim tablePayCondChosen = New PdfPTable(8) With {.TotalWidth = prodTable.TotalWidth, .SpacingBefore = 7.0F, .LockedWidth = True}
                Dim widthstablePayCondChosen As Single() = New Single() {1.2F, 1.4F, 1.2F, 1.4F, 1.4F, 1.0F, 1.0F, 1.2F}
                tablePayCondChosen.SetWidths(widthstablePayCondChosen)

                Dim payCondCellChosen As PdfPCell = New PdfPCell(New Phrase("Forma", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Número de parcelas", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Entrada", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Valor de Cada Parcela", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Valor do Parcelado", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Juros (a.m.)", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Interv. Dias", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Total", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(estimate.PayCondType, heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                If estimate.PayCondPerc > 0.0 Then
                    payCondCellChosen = New PdfPCell(New Phrase(String.Format("{0}x com juros", estimate.PayCondN), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                Else
                    payCondCellChosen = New PdfPCell(New Phrase(String.Format("{0}x sem juros", estimate.PayCondN), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                End If
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatCurrency(estimate.PayCondIn), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatCurrency(estimate.PayCondInst), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatCurrency(estimate.TotalPayments), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatPercent(estimate.PayCondPerc), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(CStr(If(estimate.PayCondInterval > 0, estimate.PayCondInterval, "Mensal")), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatCurrency(estimate.TotalPayCond), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                thePdf.Add(tablePayCondChosen)

            End If

            Dim objPayCond = payCondCtrl.GetPayConds(dto.PortalId, Null.NullInteger, CDec(dto.EstimateTotalAmount))

            If objPayCond.Count > 0 Then

                Dim payCondTitle As Paragraph = New Paragraph("Condições de Pagamento:", heading8b)
                thePdf.Add(payCondTitle)

                Dim tablePayCond = New PdfPTable(8) With {.TotalWidth = prodTable.TotalWidth, .SpacingBefore = 7.0F, .LockedWidth = True}
                Dim widthstablePayCond As Single() = New Single() {1.2F, 1.4F, 1.2F, 1.4F, 1.4F, 1.0F, 1.0F, 1.2F}
                tablePayCond.SetWidths(widthstablePayCond)

                Dim payCondCell As PdfPCell = New PdfPCell(New Phrase("Forma", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Número de parcelas", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Entrada", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Valor de Cada Parcela", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Valor do Parcelado", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Juros (a.m.)", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Interv. Dias", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Total", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)

                For Each payCond In objPayCond

                    If payCond.PayCondType > 0 Then

                        Select Case payCond.PayCondType
                            Case 1
                                payCondCell = New PdfPCell(New Phrase("Boleto", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Case 2
                                payCondCell = New PdfPCell(New Phrase("Visa", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Case 3
                                payCondCell = New PdfPCell(New Phrase("Master Card", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Case 4
                                payCondCell = New PdfPCell(New Phrase("Amex", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Case 5
                                payCondCell = New PdfPCell(New Phrase("Cheque Pré", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Case 6
                                payCondCell = New PdfPCell(New Phrase("Cartão de Débito", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Case 7
                                payCondCell = New PdfPCell(New Phrase("Dinners Clube", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        End Select
                        tablePayCond.AddCell(payCondCell)

                        Dim interestRate = 0.0
                        interestRate = payCond.PayCondPerc
                        Dim paymentQty = 0
                        paymentQty = payCond.PayCondN
                        Dim payIn = 0.0
                        payIn = payCond.PayCondIn

                        interestRate = interestRate / 100

                        Dim initialPay = (dto.EstimateTotalAmount / 100 * payIn)

                        Dim totalLabel = dto.EstimateTotalAmount - initialPay

                        Dim resultPayment As Double

                        If estimate.PayCondInterval > 0 Then

                            If interestRate > 0 Then

                                Dim interestADay = interestRate / (DateTime.DaysInMonth(Year(DateTime.Now()), Month(DateTime.Now())))

                                interestADay = interestADay * estimate.PayCondInterval

                                If initialPay > 0 Then
                                    resultPayment = Pmt(interestADay, (paymentQty - 1), -totalLabel)
                                Else
                                    resultPayment = Pmt(interestADay, 1, -totalLabel)
                                End If

                            Else

                                If initialPay > 0 Then
                                    resultPayment = Pmt(interestRate, (paymentQty - 1), -totalLabel)
                                Else
                                    resultPayment = Pmt(interestRate, 1, -totalLabel)
                                End If

                            End If

                        Else

                            If initialPay > 0 Then
                                resultPayment = Pmt(interestRate, (paymentQty - 1), -totalLabel)
                            Else
                                resultPayment = Pmt(interestRate, 1, -totalLabel)
                            End If

                        End If

                        If interestRate > 0.0 Then
                            payCondCell = New PdfPCell(New Phrase(String.Format("{0}x com juros", CStr(paymentQty)), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        Else
                            payCondCell = New PdfPCell(New Phrase(String.Format("{0}x sem juros", CStr(paymentQty)), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        End If
                        tablePayCond.AddCell(payCondCell)

                        If initialPay > 0 Then
                            payCondCell = New PdfPCell(New Phrase(FormatCurrency(dto.EstimateTotalAmount / 100 * payIn), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        Else
                            payCondCell = New PdfPCell(New Phrase(FormatCurrency(0), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        End If
                        tablePayCond.AddCell(payCondCell)

                        payCondCell = New PdfPCell(New Phrase(FormatCurrency(resultPayment), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        tablePayCond.AddCell(payCondCell)

                        If initialPay > 0 Then
                            payCondCell = New PdfPCell(New Phrase(FormatCurrency((resultPayment * (paymentQty - 1))), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        Else
                            payCondCell = New PdfPCell(New Phrase(FormatCurrency((resultPayment * paymentQty)), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        End If
                        tablePayCond.AddCell(payCondCell)

                        payCondCell = New PdfPCell(New Phrase(FormatPercent(interestRate), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        tablePayCond.AddCell(payCondCell)

                        payCondCell = New PdfPCell(New Phrase(If(payCond.PayCondInterval > 0, CStr(payCond.PayCondInterval), "Mensal"), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        tablePayCond.AddCell(payCondCell)

                        If initialPay > 0 Then
                            payCondCell = New PdfPCell(New Phrase(FormatCurrency((resultPayment * (paymentQty - 1)) + initialPay), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        Else
                            payCondCell = New PdfPCell(New Phrase(FormatCurrency((resultPayment * 1) + initialPay), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        End If
                        tablePayCond.AddCell(payCondCell)

                    End If

                Next

                thePdf.Add(tablePayCond)

            End If

            Dim settingCtrl As New SettingsRepository
            Dim settingsDictionay = settingCtrl.GetSettings(dto.PortalId)

            Dim termsOE = ""
            If estimate.Inst <> "" Then
                termsOE = estimate.Inst
            Else
                termsOE = settingsDictionay("estimateTerm")
            End If

            Dim obsTitle As New Paragraph("Observações Importantes:", heading9) With {.SpacingBefore = 10.0F}
            Dim obsText As New Paragraph(String.Format("{0}", RemoveHtmlTags(HttpUtility.HtmlDecode(termsOE))), heading7)
            Dim obsComment As New Paragraph(String.Format("{0}", RemoveHtmlTags(HttpUtility.HtmlDecode(estimate.Comment))), heading7) With {.SpacingBefore = 10.0F}
            thePdf.Add(obsTitle)
            thePdf.Add(obsText)
            thePdf.Add(obsComment)

            writer.CloseStream = False
            thePdf.Close()
            'file.Close()
            str.Position = 0

            Dim recipientList As New List(Of Users.UserInfo)
            Dim personCtrl As New PeopleRepository
            Dim personInfo As New Users.UserInfo()
            Dim ccUserInfo As New Users.UserInfo()

            Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = settingsDictionay("storeEmail"), .DisplayName = portalCtrl.GetPortal(dto.PortalId).PortalName}

            Dim clientInfo = personCtrl.GetPerson(dto.PersonId, dto.PortalId, Null.NullInteger)

            If clientInfo.UserId > 0 Then
                personInfo = UserController.GetUserById(dto.PortalId, clientInfo.UserId)
            Else
                personInfo = New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = clientInfo.Email, .DisplayName = clientInfo.DisplayName}
            End If

            recipientList.Add(personInfo)

            If dto.ToEmail.Length > 0 AndAlso Not clientInfo.Email = dto.ToEmail Then
                With ccUserInfo
                    .UserID = Null.NullInteger
                    .Email = dto.ToEmail
                    .DisplayName = clientInfo.DisplayName
                End With
                recipientList.Add(ccUserInfo)
            Else
                ccUserInfo = Nothing
            End If

            Dim salesInfo = UserController.GetUserById(dto.PortalId, dto.SalesPersonId)

            Dim mm As New Net.Mail.MailMessage() With {.Subject = dto.Subject, .Body = dto.MessageBody, .IsBodyHtml = True,
                                                       .From = New Net.Mail.MailAddress(storeUser.Email, salesInfo.DisplayName)}
            mm.ReplyToList.Add(New Net.Mail.MailAddress(salesInfo.Email, salesInfo.DisplayName))
            mm.Attachments.Add(New Net.Mail.Attachment(str, String.Format("Orcamento_{0}.pdf", CStr(dto.EstimateId)))) ', Replace(DateTime.Now.ToString(), ":", "-").Replace("/", "-"))))

            'Dim sentMails = distList.SendMail(mailMessage, recipientList, SettingsDictionay("smtpServer"), CInt(SettingsDictionay("smtpPort")), CBool(SettingsDictionay("smtpConnection")), SettingsDictionay("smtpLogin"), SettingsDictionay("smtpPassword"))

            Dim email As New Thread(Sub() PostOffice.SendMail(mm, recipientList, settingsDictionay("smtpServer"), settingsDictionay("smtpPort"),
                                                              settingsDictionay("smtpConnection"), settingsDictionay("smtpLogin"),
                                                              settingsDictionay("smtpPassword"))) With {.IsBackground = True}
            email.Start()

            'Notifications.SendStoreEmail(_salesInfo, personInfo, ccUserInfo, Nothing, dto.Subject, dto.MessageBody, str, "application/pdf", Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, dto.PortalId, True)
            'Notifications.SendStoreEmail(storeUser, _salesInfo, "", "", subject, msg.Replace("\n", "<br />"), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)
            'Notifications.SendStoreEmail(storeUser, Users.UserController.GetUserById(portalId, clientInfo.UserId), "", "", subject, msg.Replace("\n", "<br />"), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)

            'Notifications.EstimateNotification(Constants.ContentTypeName.RIStore_Estimate, Constants.NotificationEstimateTypeName.RIStore_Estimate_Updated, Null.NullInteger, _salesInfo, "Gerentes", portalId, eId, String.Format("Novo Orçamento Inserido (ID: {0})", CStr(eId)), msg)


            'ms.Position = 0

            'Dim response As HttpResponseMessage = New HttpResponseMessage(HttpStatusCode.OK)
            'response.Content = New StreamContent(ms)
            'response.Content.Headers.ContentDisposition = New ContentDispositionHeaderValue("attachment")
            'response.Content.Headers.ContentDisposition.FileName = String.Format("Orcamento_{0}_{1}.pdf", CStr(dto.EstimateId), Replace(DateTime.Now.ToString(), ":", "-").Replace("/", "-"))
            'response.Content.Headers.ContentType = New MediaTypeHeaderValue("application/pdf")
            'response.Content.Headers.ContentLength = ms.Length

            'Response.Expires = -1
            'Response.ContentType = "application/pdf"
            'Response.AppendHeader("content-disposition", "attachment; " & String.Format("filename=Orcamento_{0}_{1}.pdf", CStr(estimateId), Replace(DateTime.Now.ToString(), ":", "-").Replace("/", "-")))
            ''Response.AppendHeader("content-disposition", "inline; filename=" & Server.MapPath(filePath & ".pdf"))
            'Response.Buffer = True
            ''Response.WriteFile(Server.MapPath(filePath & ".pdf"))
            'Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length)
            'Response.Flush()
            'Response.End()
            'Response.Close()

            'Return response
            'End Using

            Dim estimateHistory As New EstimateHistory

            estimateHistory.EstimateId = dto.EstimateId
            estimateHistory.HistoryText = "<p>Orçamento enviado via email.</p>"
            estimateHistory.Locked = True
            estimateHistory.CreatedByUser = 0
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"}) ', .SentEmail = sentMails})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates a quick estimate item
    ''' </summary>
    ''' <param name="jsonData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function SyncEstimateItems(jsonData As EstimateItemsRequest) As HttpResponseMessage ' estimateItems As List(Of EstimateItem), estimateId As Integer) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository
            Dim estimateHistory As New EstimateHistory
            Dim estimateItem As New EstimateItem
            Dim estimateCtrl As New EstimatesRepository

            For Each item In jsonData.EstimateItems
                estimateItem = estimateCtrl.GetEstimateItem(item.EstimateItemId, item.EstimateId)

                Dim unitValue = productCtrl.GetProduct(estimateItem.ProductId, "pt-BR").UnitValue

                estimateItem.ProductQty = item.ProductQty
                estimateItem.ProductEstimateOriginalPrice = unitValue
                estimateItem.ProductEstimatePrice = unitValue
                'estimateItem.ProductDiscount = item.ProductDiscount
                estimateItem.ModifiedByUser = item.ModifiedByUser
                estimateItem.ModifiedOnDate = item.ModifiedOnDate

                estimateCtrl.UpdateEstimateItem(estimateItem)

                estimateHistory.EstimateId = item.EstimateId
                estimateHistory.HistoryText = String.Format("<p>Preço no item ""{0}"" sincronizado.</p>", item.ProductName)
                estimateHistory.Locked = True
                estimateHistory.CreatedByUser = 0
                estimateHistory.CreatedOnDate = item.ModifiedOnDate

                estimateCtrl.AddEstimateHistory(estimateHistory)
            Next

            Dim estimate As New Estimate

            estimate = estimateCtrl.GetEstimate(jsonData.EstimateId, jsonData.PortalId, True)
            estimate.Discount = jsonData.Discount
            estimate.TotalAmount = jsonData.TotalAmount
            'estimate.PayCondType = ""
            'estimate.PayCondN = 0
            'estimate.PayCondPerc = 0
            'estimate.PayCondIn = 0
            'estimate.PayCondInst = 0
            'estimate.PayCondInterval = 0
            'estimate.TotalPayments = 0
            'estimate.TotalPayCond = 0
            estimate.Comment = jsonData.Comment
            estimate.ModifiedByUser = jsonData.ModifiedByUser
            estimate.ModifiedOnDate = jsonData.ModifiedOnDate

            estimateCtrl.UpdateEstimate(estimate)

            '' SignalR
            estimatesHub.Value.Clients.AllExcept(jsonData.ConnId).pushEstimate()
            'Hub.Clients.AllExcept(jsonData.ConnId).pushEstimate()

            '' SignalR
            'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
            estimatesHub.Value.Clients.AllExcept(jsonData.ConnId).pushHistory(estimateHistory)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Syncs estimates from SGI
    ''' </summary>
    ''' <param name="sDate">Modified Date Start</param>
    ''' <param name="eDate">Modified Date End</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function SyncEstimates(sDate As String, eDate As String) As HttpResponseMessage
        Try
            Dim estimateCtrl As New EstimatesRepository
            Dim davsCtrl As New SofterRepository

            Dim davs = davsCtrl.GetSGIDAVs(sDate, eDate)
            Dim addedCount = 0
            Dim updatedCount = 0

            If davs.Count Then
                For Each dav In davs

                    Dim estimate As New Estimate

                    estimate.NumDav = dav.numdav
                    estimate.NumDoc = dav.numdoc
                    estimate.SequenciaDav = dav.sequenciadav
                    estimate.Coupon = dav.coo
                    estimate.CouponAttached = dav.coo_vinculado
                    estimate.Ccf = dav.ccf
                    estimate.SaleDate = dav.datavenda
                    estimate.Canceled = dav.cupomcancelado
                    estimate.PersonId = dav.codicli
                    estimate.SalesRep = dav.codiven
                    estimate.Discount = dav.outrosdescontos
                    estimate.Extras = dav.outrosacrescimos
                    estimate.CashAmount = dav.valordin
                    estimate.ChequeAmount = dav.vrchevis
                    estimate.ChequePreAmount = dav.vrchepre
                    estimate.CardAmount = dav.vrcartao
                    estimate.CreditAmount = dav.vrcred
                    estimate.CovenantAmount = dav.vrconv
                    estimate.TicketAmount = dav.vrvale
                    estimate.PayCondId = dav.condpag
                    estimate.StatusId = 10
                    estimate.TotalAmount = (dav.valordin + dav.vrchevis + dav.vrchepre + dav.vrcartao + dav.vrcred + dav.vrconv + dav.vrvale + dav.outrosacrescimos) - dav.outrosdescontos

                    Dim existingEstimate As New Estimate

                    existingEstimate = estimateCtrl.GetEstimateDav(dav.numdav, 0)

                    If existingEstimate IsNot Nothing Then

                        estimate.EstimateId = existingEstimate.EstimateId
                        estimate.EstimateTitle = existingEstimate.EstimateTitle
                        estimate.PayCondType = existingEstimate.PayCondType
                        estimate.PayCondN = existingEstimate.PayCondN
                        estimate.PayCondPerc = existingEstimate.PayCondPerc
                        estimate.PayCondIn = existingEstimate.PayCondIn
                        estimate.PayCondInst = existingEstimate.PayCondInst
                        estimate.PayCondInterval = existingEstimate.PayCondInterval
                        estimate.TotalPayments = existingEstimate.TotalPayments
                        estimate.TotalPayCond = existingEstimate.TotalPayCond
                        estimate.Comment = existingEstimate.Comment
                        estimate.ModifiedByUser = existingEstimate.ModifiedByUser
                        estimate.CreatedByUser = existingEstimate.CreatedByUser
                        estimate.CreatedOnDate = existingEstimate.CreatedOnDate

                        If Not dav.data_alteracao = Date.MinValue Then
                            estimate.ModifiedOnDate = dav.data_alteracao
                        Else
                            estimate.ModifiedOnDate = New Date(1900, 1, 1)
                        End If

                        estimateCtrl.UpdateEstimate(estimate)

                        Dim davItems = davsCtrl.GetSGIDAVItems(dav.numdav)

                        If davItems.Count > 0 Then

                            For Each item In davItems

                                Dim estimateItem As New EstimateItem

                                estimateItem.EstimateId = estimate.EstimateId
                                estimateItem.NumDav = dav.numdav
                                estimateItem.NumDavItem = item.codigo
                                estimateItem.ProductId = item.codproduto
                                estimateItem.ProductQty = item.quantidade
                                estimateItem.ProductEstimateOriginalPrice = item.valorunitario
                                estimateItem.ProductEstimatePrice = item.valorunitario
                                estimateItem.ProductDiscount = item.descontoperc
                                estimateItem.ModifiedByUser = item.codvendedor

                                estimateItem = estimateCtrl.GetEstimateItemDav(item.codigo)

                                If estimateItem IsNot Nothing Then
                                    If Not item.data_alteracao = Date.MinValue Then
                                        estimateItem.ModifiedOnDate = item.data_alteracao
                                    Else
                                        estimateItem.ModifiedOnDate = Today()
                                    End If

                                    estimateCtrl.UpdateEstimateItem(estimateItem)

                                    Dim estimateHistory As New EstimateHistory

                                    estimateHistory.EstimateId = estimate.EstimateId
                                    estimateHistory.HistoryText = String.Format("<p>Item ""{0}"" adicionado ao cupom (Qde: {1}).</p>", item.codproduto, item.quantidade)
                                    estimateHistory.Locked = True
                                    estimateHistory.CreatedByUser = item.codvendedor
                                    estimateHistory.CreatedOnDate = Today()

                                    estimateCtrl.AddEstimateHistory(estimateHistory)
                                End If

                            Next

                        End If

                        'Else

                        'estimate.NumDav = dav.numdav
                        'estimate.EstimateTitle = ""
                        'estimate.PayCondType = dav.nome
                        'estimate.PayCondN = dav.nro_parcelas
                        'estimate.PayCondPerc = dav.acrescimo
                        'estimate.PayCondIn = 0
                        'estimate.PayCondInst = 0
                        'estimate.PayCondInterval = 30
                        'estimate.TotalPayments = 0
                        'estimate.TotalPayCond = 0
                        'estimate.Comment = dav.observacao
                        'estimate.ModifiedByUser = dav.vendedor
                        'estimate.CreatedByUser = dav.vendedor
                        'estimate.CreatedOnDate = dav.datavenda

                        'If dav.data_alteracao = Date.MinValue Then
                        '    estimate.ModifiedOnDate = New Date(1900, 1, 1)
                        'Else
                        '    estimate.ModifiedOnDate = dav.data_alteracao
                        'End If

                        'estimateCtrl.AddEstimate(estimate)

                        'Dim davItems = davsCtrl.GetSGIDAVItems(dav.numdav)

                        'If davItems.Count > 0 Then

                        '    For Each item In davItems

                        '        estimateItem.EstimateId = estimate.EstimateId
                        '        estimateItem.NumDav = dav.numdav
                        '        estimateItem.NumDavItem = item.codigo
                        '        estimateItem.ProductId = item.codproduto
                        '        estimateItem.ProductQty = item.quantidade
                        '        estimateItem.ProductEstimateOriginalPrice = item.valorunitario
                        '        estimateItem.ProductEstimatePrice = item.valorunitario
                        '        estimateItem.ProductDiscount = item.descontoperc
                        '        estimateItem.ModifiedByUser = item.codvendedor
                        '        estimateItem.ModifiedOnDate = Today()
                        '        estimateItem.CreatedByUser = item.vendedor
                        '        estimateItem.CreatedOnDate = item.data_cadastro

                        '        estimateCtrl.AddEstimateItem(estimateItem)

                        '        estimateHistory.EstimateId = estimate.EstimateId
                        '        estimateHistory.HistoryText = String.Format("<p>Item ""{0}"" adicionado ao cupom (Qde: {1}).</p>", item.codproduto, item.quantidade)
                        '        estimateHistory.Locked = True
                        '        estimateHistory.CreatedByUser = item.codvendedor
                        '        estimateHistory.CreatedOnDate = Today()

                        '        estimateCtrl.AddEstimateHistory(estimateHistory)

                        '    Next

                        'End If

                    End If

                Next
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .Updated = updatedCount})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes product from stock
    ''' </summary>
    ''' <param name="products"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function TakeProductStock(products As List(Of Product)) As HttpResponseMessage
        Try
            Dim product As New Product
            Dim productCtrl As New ProductsRepository

            For Each item In products
                product = productCtrl.GetProduct(item.ProductId, "pt-BR")

                Dim oldStock = product.QtyStockSet

                product.QtyStockSet = oldStock - item.QtyStockSet
                product.ModifiedByUser = item.ModifiedByUser
                product.ModifiedOnDate = item.ModifiedOnDate

                productCtrl.UpdateProduct(product)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates estimate client id
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function UpdateEstimateClient(dto As Estimate) As HttpResponseMessage
        Try
            Dim estimate As New Estimate
            Dim estimateCtrl As New EstimatesRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, PortalController.Instance.GetCurrentPortalSettings().PortalId, True)

            estimate.PersonId = dto.PersonId
            estimate.ModifiedByUser = dto.ModifiedByUser
            estimate.ModifiedOnDate = dto.ModifiedOnDate

            estimateCtrl.UpdateEstimate(estimate)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates estimate pay form and condition
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function UpdateEstimateConfig(dto As Estimate) As HttpResponseMessage
        Try
            Dim estimateHistory As New EstimateHistory
            Dim estimate As New Estimate
            Dim estimateCtrl As New EstimatesRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, True)

            estimate.SalesRep = dto.SalesRep
            'estimate.Discount = dto.Discount
            estimate.StatusId = dto.StatusId
            estimate.ViewPrice = dto.ViewPrice
            estimate.Locked = dto.Locked
            estimate.ModifiedByUser = dto.ModifiedByUser
            estimate.ModifiedOnDate = dto.ModifiedOnDate

            estimateCtrl.UpdateEstimate(estimate)

            estimateHistory.EstimateId = estimate.EstimateId
            estimateHistory.HistoryText = "<p>Configuração do orçamento atualizada.</p>"
            estimateHistory.Locked = False
            estimateHistory.CreatedByUser = dto.ModifiedByUser
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates a quick estimate item
    ''' </summary>
    ''' <param name="jsonData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function UpdateEstimateItems(jsonData As EstimateItemsRequest) As HttpResponseMessage ' estimateItems As List(Of EstimateItem), estimateId As Integer) As HttpResponseMessage
        Try
            Dim estimate As New Estimate
            Dim estimateHistory As New EstimateHistory
            Dim estimateItem As New EstimateItem
            Dim estimateCtrl As New EstimatesRepository

            For Each item In jsonData.EstimateItems
                estimateItem = estimateCtrl.GetEstimateItem(item.EstimateItemId, item.EstimateId)

                estimateItem.ProductQty = item.ProductQty
                estimateItem.ProductEstimateOriginalPrice = item.ProductEstimateOriginalPrice
                estimateItem.ProductEstimatePrice = item.ProductEstimatePrice
                estimateItem.ProductDiscount = item.ProductDiscount
                estimateItem.ModifiedByUser = item.ModifiedByUser
                estimateItem.ModifiedOnDate = item.ModifiedOnDate

                estimateCtrl.UpdateEstimateItem(estimateItem)

                estimateHistory.EstimateId = item.EstimateId
                estimateHistory.HistoryText = String.Format("<p>Item ""{0}"" atualizado (Qde: {1}).</p>", item.ProductName, item.ProductQty)
                estimateHistory.Locked = True
                estimateHistory.CreatedByUser = 0
                estimateHistory.CreatedOnDate = item.ModifiedOnDate

                estimateCtrl.AddEstimateHistory(estimateHistory)
            Next

            estimate = estimateCtrl.GetEstimate(jsonData.EstimateId, jsonData.PortalId, True)
            estimate.EstimateTitle = jsonData.EstimateTitle
            estimate.Discount = jsonData.Discount
            estimate.TotalAmount = jsonData.TotalAmount
            estimate.PayCondType = ""
            estimate.PayCondN = 0
            estimate.PayCondPerc = 0
            estimate.PayCondIn = 0
            estimate.PayCondInst = 0
            estimate.PayCondInterval = 0
            estimate.TotalPayments = 0
            estimate.TotalPayCond = 0
            estimate.Comment = jsonData.Comment
            estimate.ModifiedByUser = jsonData.ModifiedByUser
            estimate.ModifiedOnDate = jsonData.ModifiedOnDate

            estimateCtrl.UpdateEstimate(estimate)

            If jsonData.ConnId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(jsonData.ConnId).pushEstimate()
                'Hub.Clients.AllExcept(jsonData.ConnId).pushEstimate()

                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(jsonData.ConnId).pushHistory(estimateHistory)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates estimate pay form and condition
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function UpdateEstimatePayCond(dto As Estimate) As HttpResponseMessage
        Try
            Dim estimateHistory As New EstimateHistory
            Dim estimate As New Estimate
            Dim estimateCtrl As New EstimatesRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, True)

            estimate.PayCondId = dto.PayCondId
            estimate.PayCondType = dto.PayCondType
            estimate.PayCondN = dto.PayCondN
            estimate.PayCondPerc = dto.PayCondPerc
            estimate.PayCondIn = dto.PayCondIn
            estimate.PayInDays = dto.PayInDays
            estimate.PayCondInst = dto.PayCondInst
            estimate.PayCondInterval = dto.PayCondInterval
            estimate.TotalPayments = dto.TotalPayments
            estimate.TotalPayCond = dto.TotalPayCond
            estimate.ModifiedByUser = dto.ModifiedByUser
            estimate.ModifiedOnDate = dto.ModifiedOnDate

            estimateCtrl.UpdateEstimate(estimate)

            estimateHistory.EstimateId = estimate.EstimateId
            estimateHistory.HistoryText = "<p>Condição de pagamento atuzaliada.</p>"
            estimateHistory.Locked = True
            estimateHistory.CreatedByUser = 0
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushPayCondition()
                'Hub.Clients.AllExcept(dto.ConnId).pushPayCondition()

                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ' ''' <summary>
    ' ''' Updates estimate total amount
    ' ''' </summary>
    ' ''' <param name="eId">Estimate ID</param>
    ' ''' <param name="amt">Amount</param>
    ' ''' <param name="uId">Modified By User User ID</param>
    ' ''' <param name="md">Modified Date</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    '<DnnAuthorize> _
    '<HttpPost> _
    'Function UpdateEstimateAmount(eId As Integer, amt As String, uId As Integer, md As Date) As HttpResponseMessage
    '    Try
    '        Dim culture = New CultureInfo("pt-BR")
    '        Dim numInfo = culture.NumberFormat

    '        Dim estimate As New Estimate
    '        Dim estimateCtrl As New EstimatesRepository

    '        estimate = estimateCtrl.GetEstimate(eId, PortalController.Instance.GetCurrentPortalSettings().PortalId, True)

    '        estimate.TotalAmount = Decimal.Parse(amt.Replace(".", ","), numInfo)
    '        'estimate.PayCondType = ""
    '        'estimate.PayCondN = 0
    '        'estimate.PayCondPerc = 0
    '        'estimate.PayCondIn = 0
    '        'estimate.PayCondInst = 0
    '        'estimate.PayCondInterval = 0
    '        'estimate.TotalPayments = 0
    '        'estimate.TotalPayCond = 0
    '        estimate.ModifiedByUser = uId
    '        estimate.ModifiedOnDate = md

    '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
    '    Catch ex As Exception
    '        'DnnLog.Error(ex)
    '        logger.[Error](ex)
    '        Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
    '    End Try
    'End Function

    ''' <summary>
    ''' Updates estimate pay form and condition
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function UpdateEstimateTerm(dto As Estimate) As HttpResponseMessage
        Try
            Dim estimateHistory As New EstimateHistory
            Dim estimate As New Estimate
            Dim estimateCtrl As New EstimatesRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, True)

            estimate.Inst = dto.Inst.Replace(vbCrLf, "<br/>")
            estimate.ModifiedByUser = dto.ModifiedByUser
            estimate.ModifiedOnDate = dto.ModifiedOnDate

            estimateCtrl.UpdateEstimate(estimate)

            estimateHistory.EstimateId = estimate.EstimateId
            estimateHistory.HistoryText = "<p>Termo do orçamento atualizado.</p>"
            estimateHistory.Locked = False
            estimateHistory.CreatedByUser = dto.ModifiedByUser
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            '' SignalR
            estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushNewTerm(dto.Inst)
            'Hub.Clients.AllExcept(dto.ConnId).pushPayCondition()

            '' SignalR
            'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
            estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates estimate message comment
    ''' </summary>
    ''' <param name="historyComment"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function UpdateHistoryComment(historyComment As HistoryComment) As HttpResponseMessage
        Try
            Dim estimateHistoryComment As New EstimateHistoryComment
            Dim estimateCtrl As New EstimatesRepository

            If historyComment.dto.CommentId > 0 Then
                estimateHistoryComment = estimateCtrl.GetEstimateHistoryComment(historyComment.dto.CommentId, historyComment.dto.EstimateHistoryId)
            End If

            estimateHistoryComment.EstimateHistoryId = historyComment.dto.EstimateHistoryId
            estimateHistoryComment.CommentText = historyComment.dto.CommentText
            estimateHistoryComment.CreatedByUser = historyComment.dto.CreatedByUser
            estimateHistoryComment.CreatedOnDate = historyComment.dto.CreatedOnDate

            Dim userInfo = UserController.GetUserById(PortalController.Instance.GetCurrentPortalSettings().PortalId, estimateHistoryComment.CreatedByUser)
            estimateHistoryComment.DisplayName = userInfo.DisplayName
            If userInfo.Profile.Photo IsNot Nothing Then
                If userInfo.Profile.Photo.Length > 2 Then
                    estimateHistoryComment.Avatar = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(userInfo).FolderPath
                    estimateHistoryComment.Avatar = estimateHistoryComment.Avatar & DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(userInfo.Profile.Photo).FileName
                End If
            End If

            If historyComment.dto.CommentId > 0 Then
                estimateCtrl.UpdateEstimateHistoryComment(estimateHistoryComment)
            Else
                estimateCtrl.AddEstimateHistoryComment(estimateHistoryComment)
            End If

            If historyComment.connId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(historyComment.connId).pushHistoryComment(estimateHistoryComment, historyComment.messageIndex)
                'Hub.Clients.AllExcept(MessageComment.connId).pushComment(estimateMessageComment, MessageComment.messageIndex)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .EstimateHistoryComment = estimateHistoryComment})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates estimate message
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function UpdateMessage(dto As EstimateMessage) As HttpResponseMessage
        Try
            Dim estimateMessage As New EstimateMessage
            Dim estimateCtrl As New EstimatesRepository

            If dto.EstimateMessageId > 0 Then
                estimateMessage = estimateCtrl.GetEstimateMessage(dto.EstimateMessageId, dto.EstimateId)
            End If

            estimateMessage.EstimateId = dto.EstimateId
            estimateMessage.Allowed = dto.Allowed
            estimateMessage.MessageText = dto.MessageText
            estimateMessage.CreatedByUser = dto.CreatedByUser
            estimateMessage.CreatedOnDate = dto.CreatedOnDate
            estimateMessage.ModifiedByUser = dto.CreatedByUser
            estimateMessage.ModifiedOnDate = dto.CreatedOnDate

            If dto.EstimateMessageId > 0 Then
                estimateCtrl.UpdateEstimateMessage(estimateMessage)
            Else
                estimateCtrl.AddEstimateMessage(estimateMessage)
            End If

            Dim userInfo = UserController.GetUserById(PortalController.Instance.GetCurrentPortalSettings().PortalId, estimateMessage.CreatedByUser)
            estimateMessage.DisplayName = userInfo.DisplayName
            If userInfo.Profile.Photo IsNot Nothing Then
                If userInfo.Profile.Photo.Length > 2 Then
                    estimateMessage.Avatar = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(userInfo).FolderPath
                    estimateMessage.Avatar = estimateMessage.Avatar & DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(userInfo.Profile.Photo).FileName
                End If
            End If

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushMessage(estimateMessage)
                'Hub.Clients.AllExcept(MessageComment.connId).pushComment(estimateMessageComment, MessageComment.messageIndex)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .EstimateMessage = estimateMessage})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates estimate message comment
    ''' </summary>
    ''' <param name="MessageComment"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function UpdateMessageComment(messageComment As MessageComment) As HttpResponseMessage
        Try
            Dim estimateMessageComment As New EstimateMessageComment
            Dim estimateCtrl As New EstimatesRepository

            If messageComment.dto.CommentId > 0 Then
                estimateMessageComment = estimateCtrl.GetEstimateMessageComment(messageComment.dto.CommentId, messageComment.dto.EstimateMessageId)
            End If

            estimateMessageComment.EstimateMessageId = messageComment.dto.EstimateMessageId
            estimateMessageComment.CommentText = messageComment.dto.CommentText
            estimateMessageComment.CreatedByUser = messageComment.dto.CreatedByUser
            estimateMessageComment.CreatedOnDate = messageComment.dto.CreatedOnDate

            Dim userInfo = UserController.GetUserById(PortalController.Instance.GetCurrentPortalSettings().PortalId, estimateMessageComment.CreatedByUser)
            estimateMessageComment.DisplayName = userInfo.DisplayName
            If userInfo.Profile.Photo IsNot Nothing Then
                If userInfo.Profile.Photo.Length > 2 Then
                    estimateMessageComment.Avatar = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(userInfo).FolderPath
                    estimateMessageComment.Avatar = estimateMessageComment.Avatar & DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(userInfo.Profile.Photo).FileName
                End If
            End If

            If messageComment.dto.CommentId > 0 Then
                estimateCtrl.UpdateEstimateMessageComment(estimateMessageComment)
            Else
                estimateCtrl.AddEstimateMessageComment(estimateMessageComment)
            End If

            If messageComment.connId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(messageComment.connId).pushMessageComment(estimateMessageComment, messageComment.messageIndex)
                'Hub.Clients.AllExcept(MessageComment.connId).pushComment(estimateMessageComment, MessageComment.messageIndex)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .EstimateMessageComment = estimateMessageComment})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates estimate status
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function UpdateStatus(dto As Estimate) As HttpResponseMessage
        Try
            Dim estimateCtrl As New EstimatesRepository

            Dim estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, True)

            estimate.StatusId = dto.StatusId
            estimate.ModifiedByUser = dto.ModifiedByUser
            estimate.ModifiedOnDate = dto.ModifiedOnDate

            estimateCtrl.UpdateEstimate(estimate)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ' ''' <summary>
    ' ''' Adds an estimate
    ' ''' </summary>
    ' ''' <param name="dto"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    '<DnnAuthorize> _
    '<HttpPost> _
    'Function addEstimate(dto As EstimateItemsRequest) As HttpResponseMessage
    '    Try
    '        Dim estimateHistory As New EstimateHistory
    '        Dim estimate As New Estimate
    '        'Dim estimateItems As New List(Of EstimateItem)
    '        Dim estimateCtrl As New EstimatesRepository

    '        Dim personCtrl As New PeopleRepository
    '        Dim personId = personCtrl.getPerson(Null.NullInteger, dto.PortalId, dto.UserId).PersonId

    '        estimate.StatusId = dto.StatusId
    '        estimate.PortalId = dto.PortalId
    '        estimate.PersonId = personId
    '        estimate.EstimateTitle = dto.EstimateTitle
    '        estimate.Guid = dto.Guid
    '        estimate.SalesRep = dto.SalesRep
    '        estimate.ViewPrice = dto.ViewPrice
    '        estimate.Discount = dto.Discount
    '        estimate.TotalAmount = dto.TotalAmount
    '        estimate.CreatedByUser = dto.CreatedByUser
    '        estimate.CreatedOnDate = dto.CreatedOnDate
    '        estimate.ModifiedByUser = dto.ModifiedByUser
    '        estimate.ModifiedOnDate = dto.ModifiedOnDate

    '        If Not estimate.StatusId > 0 Then
    '            Dim defaultStatus As New Status
    '            Dim defaultStatusCtrl As New StatusesRepository

    '            defaultStatus = defaultStatusCtrl.getStatus("Normal", dto.PortalId)

    '            If defaultStatus IsNot Nothing Then
    '                estimate.StatusId = defaultStatus.StatusId
    '            Else
    '                estimate.StatusId = defaultStatusCtrl.getStatuses(dto.PortalId, "False")(0).StatusId
    '            End If
    '        End If

    '        estimateCtrl.AddEstimate(estimate)

    '        estimateHistory.EstimateId = estimate.EstimateId
    '        estimateHistory.HistoryText = "<p>Orçamento Inicializado</p>"
    '        estimateHistory.Locked = True
    '        estimateHistory.CreatedByUser = dto.ModifiedByUser
    '        estimateHistory.CreatedOnDate = dto.ModifiedOnDate

    '        estimateCtrl.AddEstimateHistory(estimateHistory)

    '        If dto.ConnId IsNot Nothing Then
    '            '' SignalR
    '            estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushEstimate()
    '            'Hub.Clients.AllExcept(dto.ConnId).pushEstimate()

    '            '' SignalR
    '            'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
    '            estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
    '        End If

    '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .EstimateId = estimate.EstimateId})
    '    Catch ex As Exception
    '        'DnnLog.Error(ex)
    '        Logger.[Error](ex)
    '        Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
    '    End Try
    'End Function

    ''' <summary>
    ''' Updates estimate title
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function UpdateTitle(dto As EstimatePdf) As HttpResponseMessage
        Try
            Dim estimate As New Estimate
            Dim estimateCtrl As New EstimatesRepository
            Dim estimateHistory As New EstimateHistory

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId, True)
            estimate.EstimateTitle = dto.EstimateTitle
            estimate.ModifiedByUser = dto.ModifiedByUser
            estimate.ModifiedOnDate = dto.ModifiedOnDate
            estimateCtrl.UpdateEstimate(estimate)

            estimateHistory.EstimateId = dto.EstimateId
            estimateHistory.HistoryText = "<p>Título do orçamento atualizado.</p>"
            estimateHistory.Locked = True
            estimateHistory.CreatedByUser = 0
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushEstimateTitle(dto.EstimateTitle)
                'Hub.Clients.AllExcept(jsonData.ConnId).pushEstimate()

                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                'estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    Public Class EstimateItemsRequest
        Property Comment As String
        Property ConnId As String
        Property CreatedByUser As Integer
        Property CreatedOnDate As Nullable(Of DateTime)
        Property Discount As Decimal
        Property EstimateId As Integer
        Property EstimateItems As List(Of EstimateItem)
        Property EstimateItemsRemoved As List(Of EstimateItemRemoved)
        Property EstimateTitle As String
        Property Guid As String
        Property ModifiedByUser As Integer
        Property ModifiedOnDate As Nullable(Of DateTime)
        Property PersonId As Integer
        Property PortalId As Integer
        Property RemoveReasonId As String
        Property SalesRep As Integer
        Property StatusId As Integer
        Property TotalAmount As Decimal
        'Property ProductId As Integer
        'Property ProductName As String
        'Property ProductQty As Double
        'Property ProductEstimateOriginalPrice As Single
        'Property ProductEstimatePrice As Single

        Property UserId As Integer
        Property ViewPrice As Boolean

    End Class

    Public Class EstimatePdf
        Property Columns As List(Of String)
        Property ColumnsCount As Integer
        Property Conditions As Boolean
        Property ConnId As String
        Property EstimateDiscountValue As Single
        Property EstimateId As Integer
        Property EstimateLink As String
        Property EstimateTitle As String
        Property EstimateTotalAmount As Single
        Property Expand As Boolean
        Property Lang As String
        Property MessageBody As String
        Property ModifiedByUser As Integer
        Property ModifiedOnDate As DateTime
        Property PersonId As Integer
        Property PortalId As Integer
        Property ProductDiscountValue As Decimal
        Property ProductOriginalAmount As Single
        Property SalesPersonId As Integer
        Property Subject As String
        Property ToEmail As String
        Property TotalDiscountPerc As Decimal
        Property TotalDiscountValue As Single
        Property UserId As Integer
        Property Watermark As String
    End Class

    Public Class EstimateReceiptTxt
        Property TxtHeader As String
        Property TxtSubHeader As String
        Property TxtClientInfo As String
        Property TxtColumnHeader As String
        Property TxtItemName As String
        Property EstimateId As Integer
        Property PortalId As Integer
        Property PersonId As Integer
        Property TxtItemRef As String
        Property TxtItemPrice As String
        Property TxtItemUni As String
        Property TxtItemQty As String
        Property TxtItemDisc As String
        Property TxtDiscount As String
        Property TxtSubTotal As String
        Property TxtTotal As String
        Property TxtBankIn As String
        Property TxtCheckIn As String
        Property TxtCardIn As String
        Property TxtDebitIn As String
        Property TxtCashIn As String
        Property TxtCheck As String
        Property TxtConditionColumnHeader As String
        Property TxtPayQty As String
        Property TxtPayments As String
        Property TxtInitialPay As String
        Property TxtInterest As String
        Property TxtTotalPays As String
    End Class

    Public Class EstimatesRequest
        Public Property Estimates As List(Of Estimate)
    End Class

    Public Class EstimateTxt
        Property TxtHeader As String
        Property TxtSubHeader As String
        Property TxtClientInfo As String
        Property TxtColumnHeader As String
        Property TxtItemName As String
        Property EstimateId As Integer
        Property PortalId As Integer
        Property PersonId As Integer
        Property TxtItemRef As String
        Property TxtItemPrice As String
        Property TxtItemUni As String
        Property TxtItemQty As String
        Property TxtItemDisc As String
        Property TxtDiscount As String
        Property TxtSubTotal As String
        Property TxtTotal As String
        Property TxtConditionColumnHeader As String
        Property TxtCheck As String
        Property TxtBankPay As String
        Property TxtVisa As String
        Property TxtMC As String
        Property TxtAmex As String
        Property TxtDinners As String
        Property TxtDebit As String
        Property TxtPayQty As String
        Property TxtPayments As String
        Property TxtInitialPay As String
        Property TxtInterest As String
        Property TxtTotalPays As String
        Property printClient As Boolean
    End Class

    Public Class HistoryComment
        Public Property connId As String
        Public Property dto As EstimateHistoryComment
        Public Property messageIndex As Integer
    End Class

    '<DnnAuthorize> _
    '<HttpGet> _
    'Function getMoreMessageComments(messageId As Integer, comments As Integer) As HttpResponseMessage
    '    Try
    '        Dim estimateCtrl As New EstimatesRepository

    '        Dim counter = 0

    '        Do Until counter >= 300
    '            Threading.Thread.Sleep(4000)
    '            If getComments(messageId).Count > comments Then
    '                Exit Do
    '            Else
    '                counter = counter + 1
    '            End If
    '        Loop

    '        Dim commentsData = estimateCtrl.getEstimateMessageComments(messageId)
    '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.MessageComments = commentsData})
    '    Catch ex As Exception
    '        'DnnLog.Error(ex)
    '        Logger.[Error](ex)
    '        Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
    '    End Try
    'End Function

    'Function getComments(messageId As Integer) As IEnumerable(Of EstimateMessageComment)
    '    Dim estimateCtrl As New EstimatesRepository
    '    Return estimateCtrl.getEstimateMessageComments(messageId)
    'End Function

    Public Class MessageComment
        Public Property connId As String
        Public Property dto As EstimateMessageComment
        Public Property messageIndex As Integer
    End Class

End Class
