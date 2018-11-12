
Imports RIW.Modules.Quick_Estimate.Components

Namespace Views

    Partial Class ViewQuickEstimate
    Inherits RiwQuickEstimateModuleBase

        Private Const RiwModulePathScript As String = "mod_ModulePathScript"

        #Region "Subs and Functions"

        'Public Sub PrintEstimateWindow()
        '    Try
        '        Dim payCondCtrl As New PayConditionsRepository
        '        Dim personCtrl As New PeopleRepository
        '        Dim estimateCtrl As New EstimatesRepository
        '        Dim estimate_Info = estimateCtrl.GetEstimate(CInt(hfEId.Value), PortalId)
        '        Dim SalesRepInfo = Users.UserController.GetUserById(PortalId, estimate_Info.SalesRep)

        '        Dim SalesPhone = String.Empty
        '        SalesPhone = SalesRepInfo.Profile.Telephone

        '        Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(PortalId)

        '        Dim header = Localization.GetString("Header", LocalResourceFile).Replace("[BR]", Environment.NewLine)
        '        header = header.Replace("[DATA]", Utilities.TitleCase(DateTime.Now.ToString("f", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"))))
        '        header = header.Replace("[EMPRESA]", Utilities.ReplaceAccentletters(PortalSettings.PortalName))
        '        header = header.Replace("[SITE]", PortalSettings.PortalAlias.HTTPAlias)
        '        header = header.Replace("[RUA]", CStr(SettingsDictionay("StoreAddress")))
        '        header = header.Replace("[NUM]", CStr(SettingsDictionay("StoreUnit")))
        '        header = header.Replace("[BAIRRO]", CStr(SettingsDictionay("StoreDistrict")))
        '        header = header.Replace("[CIDADE]", CStr(SettingsDictionay("StoreCity")))
        '        header = header.Replace("[ESTADO]", CStr(SettingsDictionay("StoreRegion")))

        '        ' HTML Structure
        '        Dim saveString = Null.NullString

        '        saveString = header

        '        Dim subHeader = Localization.GetString("SubHeader", LocalResourceFile).Replace("[BR]", Environment.NewLine)
        '        subHeader = subHeader.Replace("[FONE]", Utilities.PhoneMask(IIf(SalesPhone.Length > 0, SalesPhone, Left(SettingsDictionay("StorePhones").Replace("voz=", ""), 10))))
        '        subHeader = subHeader.Replace("[EMAIL]", CStr(IIf(SalesRepInfo.Email.Length > 0, SalesRepInfo.Email, SettingsDictionay("StoreEmail"))))
        '        subHeader = subHeader.Replace("[ORC]", hfEId.Value)

        '        saveString += subHeader

        '        Dim clientInfo = personCtrl.getPerson(CInt(hfCId.Value), PortalId, Null.NullInteger)
        '        Dim clientStr = String.Format(Localization.GetString("ClientInfo", LocalResourceFile).Replace("[BR]", Environment.NewLine), "")
        '        clientStr = clientStr.Replace("[NOME]", clientInfo.DisplayName)
        '        clientStr = clientStr.Replace("[RUA]", clientInfo.Street)
        '        clientStr = clientStr.Replace("[NUM]", clientInfo.Unit)
        '        clientStr = clientStr.Replace("[BAIRRO]", clientInfo.District)
        '        clientStr = clientStr.Replace("[CIDADE]", clientInfo.City)
        '        clientStr = clientStr.Replace("[ESTADO]", clientInfo.Region)
        '        clientStr = clientStr.Replace("[CEP]", clientInfo.PostalCode)
        '        clientStr = clientStr.Replace("[TELEFONE]", clientInfo.Telephone)

        '        saveString += clientStr

        '        'saveString += String.Format("{1}{1}{0}{1}{2}{1}Telefone: {3}{1}Email: {4}{1}{1}ORCAMENTO n {5}", ReplaceAccentletters(PortalSettings.PortalName), Environment.NewLine, header, SalesPhone, SalesRepInfo.Email, Label_EstimateNumber.Text)

        '        'saveString += String.Format(Localization.GetString("ColumnHeader", LocalResourceFile), Space(3), Space(4), Space(6), Space(9))

        '        saveString += Localization.GetString("ColumnHeader", LocalResourceFile).Replace("[BR]", Environment.NewLine)

        '        'saveString += String.Format("{4}{4}ITEM{1}DESCRICAO{4}CODIGO{3}VL.UN{1}QTD{0}UN{0}DESC{0}TOTAL{4}---------------------------------------------------{4}", Space(3), Space(4), Space(6), Space(9), Environment.NewLine)

        '        Dim estimateDetailInfo = estimateCtrl.GetEstimateItems(PortalId, CInt(hfEId.Value), "pt-BR")

        '        Dim itemRef = String.Empty
        '        Dim itemIndex = 0

        '        Dim totalItems As Single = 0.0
        '        Dim totalBeforeDiscount As Single = 0.0

        '        For Each item In estimateDetailInfo

        '            itemIndex += 1

        '            Dim itemName = "0"
        '            If item.ProductName.Length > 25 Then
        '                itemName = item.ProductName.Remove(25)
        '            Else
        '                itemName = item.ProductName
        '            End If
        '            Dim itemCode = "0000000000000"
        '            If item.Barcode.Length > 0 Then
        '                itemCode = item.Barcode
        '            ElseIf item.ProductRef.Length > 0 Then
        '                itemCode = item.ProductRef
        '            End If

        '            totalBeforeDiscount = totalBeforeDiscount + (item.ProductEstimateOriginalPrice * CDbl(item.ProductQty))

        '            Dim totalLiquid As Single = item.ProductEstimatePrice * CDbl(item.ProductQty)

        '            Dim padItemName = Right(Localization.GetString("ItemName", LocalResourceFile).Replace("]", ""), 2).TrimStart("0"c)
        '            Dim padItemRef = Right(Localization.GetString("ItemRef", LocalResourceFile).Replace("]", ""), 2).TrimStart("0"c)
        '            Dim padPrice = Right(Localization.GetString("ItemPrice", LocalResourceFile).Replace("]", ""), 2).TrimStart("0"c)
        '            Dim padQty = Right(Localization.GetString("ItemQty", LocalResourceFile).Replace("]", ""), 2).TrimStart("0"c)
        '            Dim padUni = Right(Localization.GetString("ItemUni", LocalResourceFile).Replace("]", ""), 2).TrimStart("0"c)
        '            Dim padDisc = Right(Localization.GetString("ItemDisc", LocalResourceFile).Replace("]", ""), 2).TrimStart("0"c)

        '            Dim strItemName = Localization.GetString("ItemName", LocalResourceFile).Replace("[ITEMNAME:", itemName)
        '            Dim strItemRef = Localization.GetString("ItemRef", LocalResourceFile).Replace("[ITEMREF:", Right(itemCode, 13))
        '            Dim strItemPrice = Localization.GetString("ItemPrice", LocalResourceFile).Replace("[ITEMPRICE:", FormatCurrency(item.ProductEstimateOriginalPrice))
        '            Dim strItemQty = Localization.GetString("ItemQty", LocalResourceFile).Replace("[ITEMQTY:", CStr(item.ProductQty))
        '            Dim strItemUni = Localization.GetString("ItemUni", LocalResourceFile).Replace("[ITEMUNI:", Left(item.UnitTypeTitle, 3))
        '            Dim strItemDisc = Localization.GetString("ItemDisc", LocalResourceFile).Replace("[ITEMDISC:", FormatPercent((item.ProductDiscount) / 100))

        '            strItemName = Left(strItemName, itemName.Length)
        '            strItemRef = Left(strItemRef, Right(itemCode, hfEId.Value).Length) & Environment.NewLine
        '            strItemPrice = Left(strItemPrice, FormatCurrency(item.ProductEstimateOriginalPrice).Length)
        '            strItemQty = Left(strItemQty, CStr(item.ProductQty).Length)
        '            strItemUni = Left(strItemUni, Left(item.UnitTypeTitle, 3).Length)
        '            strItemDisc = Left(strItemDisc, FormatPercent((item.ProductDiscount) / 100).Length)

        '            saveString += String.Concat(itemRef.PadRight(7),
        '                              strItemName.PadRight(CInt(padItemName)),
        '                              strItemRef.PadRight(CInt(padItemRef)),
        '                              strItemPrice.PadRight(CInt(padPrice)),
        '                              strItemQty.PadRight(CInt(padQty)),
        '                              strItemUni.PadRight(CInt(padUni)),
        '                              strItemDisc.PadRight(CInt(padDisc)),
        '                              FormatCurrency(totalLiquid) & Environment.NewLine)

        '            totalItems = totalItems + totalLiquid

        '        Next

        '        Dim lPadTotalDisc = Left(Localization.GetString("Discount", LocalResourceFile).Replace("[", ""), 2).TrimStart("0"c)
        '        Dim rPadTotalDisc = Right(Localization.GetString("Discount", LocalResourceFile).Replace("]", ""), 2).TrimStart("0"c)
        '        Dim strDiscount = ""
        '        Dim DiscountValue = totalBeforeDiscount - totalItems
        '        Dim Total As Single = totalBeforeDiscount - DiscountValue

        '        'Total = Math.Round(Total - (Total * estimate_Info.Discount / 100), 2)

        '        If estimate_Info.Discount > 0 Then
        '            DiscountValue = DiscountValue + (totalItems * estimate_Info.Discount / 100)
        '            Total = Math.Round(Total - DiscountValue)
        '        End If

        '        If DiscountValue > 0.01 Then
        '            DiscountValue = String.Format("{0:C}", Math.Floor(DiscountValue * 100) / 100)
        '            strDiscount = String.Concat("DESCONTO:".PadRight(CInt(rPadTotalDisc)), Localization.GetString("Discount", LocalResourceFile).Replace(String.Format("[{0}:DISCOUNT:", lPadTotalDisc), FormatCurrency(DiscountValue))).PadRight(CInt(rPadTotalDisc))
        '            strDiscount = strDiscount.Replace(rPadTotalDisc & "]", "")
        '        Else
        '            strDiscount = ""
        '        End If

        '        Dim rPadSubTotal = Right(Localization.GetString("SubTotal", LocalResourceFile).Replace("]", ""), 2).TrimStart("0"c)
        '        Dim rPadTotal = Right(Localization.GetString("Total", LocalResourceFile).Replace("]", ""), 2).TrimStart("0"c)

        '        Dim lPadSubTotal = Left(Localization.GetString("SubTotal", LocalResourceFile).Replace("[", ""), 2).TrimStart("0"c)
        '        Dim lPadTotal = Left(Localization.GetString("Total", LocalResourceFile).Replace("[", ""), 2).TrimStart("0"c)

        '        Dim strSubTotal = String.Concat("SUBTOTAL:".PadRight(CInt(rPadSubTotal)), Localization.GetString("SubTotal", LocalResourceFile).Replace(String.Format("[{0}:SUBTOTAL:", lPadSubTotal), FormatCurrency(totalBeforeDiscount))).PadRight(CInt(rPadSubTotal))
        '        Dim strTotal = String.Concat("TOTAL:".PadRight(CInt(rPadTotal)), Localization.GetString("Total", LocalResourceFile).Replace(String.Format("[{0}:TOTAL:", lPadTotal), FormatCurrency(Total))).PadRight(CInt(rPadTotal))

        '        strSubTotal = strSubTotal.Replace(rPadSubTotal & "]", "")
        '        strTotal = strTotal.Replace(rPadTotal & "]", "")

        '        saveString += String.Concat(Environment.NewLine, strSubTotal.PadLeft(CInt(lPadSubTotal)), CStr(IIf(strDiscount.Length > 0, Environment.NewLine & strDiscount.PadLeft(CInt(lPadTotalDisc)), "")), Environment.NewLine, strTotal.PadLeft(CInt(lPadTotal)))

        '        Dim objPayCond = payCondCtrl.getPayConds(PortalId, Null.NullInteger, CSng(Total))

        '        If objPayCond.Count > 0 Then

        '            saveString += String.Concat(Environment.NewLine, Environment.NewLine, "Condicoes de Pagamento:", Environment.NewLine)

        '            saveString += String.Concat(Environment.NewLine, Localization.GetString("ConditionColumnHeader", LocalResourceFile).Replace("[BR]", Environment.NewLine), Environment.NewLine)

        '            For Each payCond In objPayCond

        '                If payCond.PayCondType > 0 Then

        '                    Select Case payCond.PayCondType
        '                        Case 5
        '                            Dim padcheck = Right(Localization.GetString("Check", LocalResourceFile), 3).TrimStart("0"c).Replace("]", "")
        '                            saveString += String.Concat(Environment.NewLine, Left(Localization.GetString("Check", LocalResourceFile).Replace("[", ""), Localization.GetString("Check", LocalResourceFile).Length - 5).PadRight(CInt(padcheck)))
        '                        Case 1
        '                            Dim padBankPay = Right(Localization.GetString("BankPay", LocalResourceFile), 3).TrimStart("0"c).Replace("]", "")
        '                            saveString += String.Concat(Environment.NewLine, Left(Localization.GetString("BankPay", LocalResourceFile).Replace("[", ""), Localization.GetString("BankPay", LocalResourceFile).Length - 5).PadRight(CInt(padBankPay)))
        '                        Case 2
        '                            Dim padVisa = Right(Localization.GetString("Visa", LocalResourceFile), 3).TrimStart("0"c).Replace("]", "")
        '                            saveString += String.Concat(Environment.NewLine, Left(Localization.GetString("Visa", LocalResourceFile).Replace("[", ""), Localization.GetString("Visa", LocalResourceFile).Length - 5).PadRight(CInt(padVisa)))
        '                        Case 3
        '                            Dim padMC = Right(Localization.GetString("MC", LocalResourceFile), 3).TrimStart("0"c).Replace("]", "")
        '                            saveString += String.Concat(Environment.NewLine, Left(Localization.GetString("MC", LocalResourceFile).Replace("[", ""), Localization.GetString("MC", LocalResourceFile).Length - 5).PadRight(CInt(padMC)))
        '                        Case 4
        '                            Dim padAmex = Right(Localization.GetString("Amex", LocalResourceFile), 3).TrimStart("0"c).Replace("]", "")
        '                            saveString += String.Concat(Environment.NewLine, Left(Localization.GetString("Amex", LocalResourceFile).Replace("[", ""), Localization.GetString("Amex", LocalResourceFile).Length - 5).PadRight(CInt(padAmex)))
        '                        Case 6
        '                            Dim padDinners = Right(Localization.GetString("Dinners", LocalResourceFile), 3).TrimStart("0"c).Replace("]", "")
        '                            saveString += String.Concat(Environment.NewLine, Left(Localization.GetString("Dinners", LocalResourceFile).Replace("[", ""), Localization.GetString("Dinners", LocalResourceFile).Length - 5).PadRight(CInt(padDinners)))
        '                        Case 7
        '                            Dim padDinners = Right(Localization.GetString("Debit", LocalResourceFile), 3).TrimStart("0"c).Replace("]", "")
        '                            saveString += String.Concat(Environment.NewLine, Left(Localization.GetString("Debit", LocalResourceFile).Replace("[", ""), Localization.GetString("Debit", LocalResourceFile).Length - 5).PadRight(CInt(padDinners)))
        '                        Case Else
        '                    End Select

        '                    Dim interestRate = 0.0
        '                    interestRate = payCond.PayCondPerc
        '                    Dim paymentQty = 0
        '                    paymentQty = payCond.PayCondN
        '                    Dim payIn = 0.0
        '                    payIn = payCond.PayCondIn

        '                    interestRate = interestRate / 100

        '                    Dim TotalLabel = Total

        '                    Dim resultPayment As Double = Pmt(interestRate, paymentQty, -TotalLabel, 0)

        '                    Dim padPayQty = Right(Localization.GetString("PayQty", LocalResourceFile), 3).TrimStart("0"c).Replace("]", "")

        '                    If interestRate > 0 Then

        '                        Dim interestADay = interestRate / (DateTime.DaysInMonth(Year(DateTime.Now()), Month(DateTime.Now())))

        '                        interestADay = interestADay * payCond.PayCondInterval

        '                        If payIn > 0 Then
        '                            resultPayment = Pmt(interestADay, (paymentQty - 1), -TotalLabel)
        '                            saveString += Left(Localization.GetString("PayQty", LocalResourceFile).Replace("[", ""), "PAYQTY".Length).Replace("PAYQTY", CStr(paymentQty - 1)).PadRight(CInt(padPayQty))
        '                        Else
        '                            resultPayment = Pmt(interestADay, paymentQty, -TotalLabel)
        '                            saveString += Left(Localization.GetString("PayQty", LocalResourceFile).Replace("[", ""), "PAYQTY".Length).Replace("PAYQTY", CStr(paymentQty)).PadRight(CInt(padPayQty))
        '                        End If

        '                    Else

        '                        If payIn > 0 Then
        '                            resultPayment = Pmt(interestRate, (paymentQty - 1), -TotalLabel)
        '                            saveString += Left(Localization.GetString("PayQty", LocalResourceFile).Replace("[", ""), "PAYQTY".Length).Replace("PAYQTY", CStr(paymentQty - 1)).PadRight(CInt(padPayQty))
        '                        Else
        '                            resultPayment = Pmt(interestRate, paymentQty, -TotalLabel)
        '                            saveString += Left(Localization.GetString("PayQty", LocalResourceFile).Replace("[", ""), "PAYQTY".Length).Replace("PAYQTY", CStr(paymentQty)).PadRight(CInt(padPayQty))
        '                        End If

        '                    End If

        '                    Dim padResultPayment = Right(Localization.GetString("Payments", LocalResourceFile), 3).TrimStart("0"c).Replace("]", "")
        '                    saveString += Left(Localization.GetString("Payments", LocalResourceFile).Replace("[", ""), "PAYMENTS".Length).Replace("PAYMENTS", FormatCurrency(resultPayment)).PadRight(CInt(padResultPayment)) & Environment.NewLine

        '                    Dim padInitialPay = Right(Localization.GetString("InitialPay", LocalResourceFile), 3).TrimStart("0"c).Replace("]", "")
        '                    If payIn > 0 Then
        '                        saveString += Left(Localization.GetString("InitialPay", LocalResourceFile).Replace("[", ""), "INITIALPAY".Length).Replace("INITIALPAY", FormatCurrency(Total / 100 * payIn)).PadRight(CInt(padInitialPay))
        '                    Else
        '                        saveString += Left(Localization.GetString("InitialPay", LocalResourceFile).Replace("[", ""), "INITIALPAY".Length).Replace("INITIALPAY", FormatCurrency(0)).PadRight(CInt(padInitialPay))
        '                    End If

        '                    Dim padInterest = Right(Localization.GetString("Interest", LocalResourceFile), 3).TrimStart("0"c).Replace("]", "")
        '                    saveString += Left(Localization.GetString("Interest", LocalResourceFile).Replace("[", ""), "INTEREST".Length).Replace("INTEREST", FormatPercent(interestRate)).PadRight(CInt(padInterest))

        '                    Dim padTotalPays = Right(Localization.GetString("TotalPays", LocalResourceFile), 3).TrimStart("0"c).Replace("]", "")
        '                    saveString += Left(Localization.GetString("TotalPays", LocalResourceFile).Replace("[", ""), "TOTALPAYS".Length).Replace("TOTALPAYS", FormatCurrency(resultPayment * paymentQty)).PadRight(CInt(padTotalPays))

        '                    saveString += CStr(IIf(payCond.PayCondInterval > 0, CStr(payCond.PayCondInterval), "Mensal"))

        '                End If

        '            Next

        '        End If

        '        saveString += String.Format("{0}{0}{1}{0}", Environment.NewLine, "Observacoes Importantes:")
        '        Dim estimateComm = estimate_Info.Inst
        '        Dim strTerms = Utilities.RemoveHtmlTags(Utilities.ReplaceAccentletters(HttpUtility.HtmlDecode(Portals.PortalController.GetPortalSetting("RIW_EstimateTerm", PortalId, ""))))
        '        saveString += CStr(IIf(estimateComm.Length > 0, String.Format("{0}{1}{0}", Environment.NewLine, estimateComm), "")) & String.Format("{0}{1}", Environment.NewLine, strTerms)

        '        Dim file_txt = String.Format("filename=Orcamento_{0}_{1}.txt", hfEId.Value, Replace(DateTime.Now.ToString(), ":", "-").Replace("/", "-"))

        '        Response.Clear()
        '        Response.Expires = -1
        '        Response.ContentType = "plain/text"
        '        Response.AppendHeader("content-disposition", "attachment; " & file_txt)
        '        Response.AppendHeader("Content-Length", saveString.Length.ToString())
        '        Response.Write(saveString)
        '        Response.End()

        '    Catch exc As Exception    'Module failed to load
        '        ProcessModuleLoadException(Me, exc)
        '    End Try
        'End Sub

        'Public Sub PrintEstimateReceipt()
        '    Try
        '        Dim payCondCtrl As New PayConditionsRepository
        '        Dim personCtrl As New PeopleRepository
        '        Dim estimateCtrl As New EstimatesRepository
        '        Dim estimate_Info = estimateCtrl.GetEstimate(CInt(hfEId.Value), PortalId)
        '        Dim SalesRepInfo = Users.UserController.GetUserById(PortalId, estimate_Info.SalesRep)

        '        Dim SalesPhone = String.Empty
        '        SalesPhone = SalesRepInfo.Profile.Telephone

        '        Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(PortalId)

        '        Dim header = Localization.GetString("Header", LocalResourceFile).Replace("[BR]", Environment.NewLine)
        '        header = header.Replace("[DATA]", Utilities.TitleCase(DateTime.Now.ToString("f", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"))))
        '        header = header.Replace("[EMPRESA]", Utilities.ReplaceAccentletters(PortalSettings.PortalName))
        '        header = header.Replace("[SITE]", PortalSettings.PortalAlias.HTTPAlias)
        '        header = header.Replace("[RUA]", CStr(SettingsDictionay("StoreAddress")))
        '        header = header.Replace("[NUM]", CStr(SettingsDictionay("StoreUnit")))
        '        header = header.Replace("[BAIRRO]", CStr(SettingsDictionay("StoreDistrict")))
        '        header = header.Replace("[CIDADE]", CStr(SettingsDictionay("StoreCity")))
        '        header = header.Replace("[ESTADO]", CStr(SettingsDictionay("StoreRegion")))

        '        ' HTML Structure
        '        Dim saveString = Null.NullString

        '        saveString = header

        '        Dim subHeader = Localization.GetString("SubHeader", LocalResourceFile).Replace("[BR]", Environment.NewLine)
        '        subHeader = subHeader.Replace("[FONE]", Utilities.PhoneMask(IIf(SalesPhone.Length > 0, SalesPhone, Left(SettingsDictionay("StorePhones").Replace("voz=", ""), 10))))
        '        subHeader = subHeader.Replace("[EMAIL]", CStr(IIf(SalesRepInfo.Email.Length > 0, SalesRepInfo.Email, SettingsDictionay("StoreEmail"))))
        '        subHeader = subHeader.Replace("[ORC]", hfEId.Value)

        '        saveString += subHeader

        '        Dim clientInfo = personCtrl.getPerson(CInt(hfCId.Value), PortalId, Null.NullInteger)
        '        Dim clientStr = String.Format(Localization.GetString("ClientInfo", LocalResourceFile).Replace("[BR]", Environment.NewLine), "")
        '        clientStr = clientStr.Replace("[NOME]", clientInfo.DisplayName)
        '        clientStr = clientStr.Replace("[RUA]", clientInfo.Street)
        '        clientStr = clientStr.Replace("[NUM]", clientInfo.Unit)
        '        clientStr = clientStr.Replace("[BAIRRO]", clientInfo.District)
        '        clientStr = clientStr.Replace("[CIDADE]", clientInfo.City)
        '        clientStr = clientStr.Replace("[ESTADO]", clientInfo.Region)
        '        clientStr = clientStr.Replace("[CEP]", clientInfo.PostalCode)
        '        clientStr = clientStr.Replace("[TELEFONE]", clientInfo.Telephone)

        '        saveString += clientStr

        '        saveString += Localization.GetString("ColumnHeader", LocalResourceFile).Replace("[BR]", Environment.NewLine)

        '        Dim estimateDetailInfo = estimateCtrl.GetEstimateItems(PortalId, CInt(hfEId.Value), "pt-BR")

        '        Dim itemRef = String.Empty
        '        Dim itemIndex = 0

        '        Dim totalItems As Single = 0.0
        '        Dim totalBeforeDiscount As Single = 0.0

        '        For Each item In estimateDetailInfo

        '            itemIndex += 1

        '            Dim itemName = "0"
        '            If item.ProductName.Length > 25 Then
        '                itemName = item.ProductName.Remove(25)
        '            Else
        '                itemName = item.ProductName
        '            End If
        '            Dim itemCode = "0000000000000"
        '            If item.Barcode.Length > 0 Then
        '                itemCode = item.Barcode
        '            End If

        '            totalBeforeDiscount = totalBeforeDiscount + (item.ProductEstimateOriginalPrice * CDbl(item.ProductQty))

        '            Dim totalLiquid As Single = item.ProductEstimatePrice * CDbl(item.ProductQty)

        '            Dim padItemName = Right(Localization.GetString("ItemName", LocalResourceFile).Replace("]", ""), 2).TrimStart("0"c)
        '            Dim padItemRef = Right(Localization.GetString("ItemRef", LocalResourceFile).Replace("]", ""), 2).TrimStart("0"c)
        '            Dim padPrice = Right(Localization.GetString("ItemPrice", LocalResourceFile).Replace("]", ""), 2).TrimStart("0"c)
        '            Dim padQty = Right(Localization.GetString("ItemQty", LocalResourceFile).Replace("]", ""), 2).TrimStart("0"c)
        '            Dim padUni = Right(Localization.GetString("ItemUni", LocalResourceFile).Replace("]", ""), 2).TrimStart("0"c)
        '            Dim padDisc = Right(Localization.GetString("ItemDisc", LocalResourceFile).Replace("]", ""), 2).TrimStart("0"c)

        '            Dim strItemName = Localization.GetString("ItemName", LocalResourceFile).Replace("[ITEMNAME:", itemName)
        '            Dim strItemRef = Localization.GetString("ItemRef", LocalResourceFile).Replace("[ITEMREF:", Right(itemCode, 13))
        '            Dim strItemPrice = Localization.GetString("ItemPrice", LocalResourceFile).Replace("[ITEMPRICE:", FormatCurrency(item.ProductEstimateOriginalPrice))
        '            Dim strItemQty = Localization.GetString("ItemQty", LocalResourceFile).Replace("[ITEMQTY:", CStr(item.ProductQty))
        '            Dim strItemUni = Localization.GetString("ItemUni", LocalResourceFile).Replace("[ITEMUNI:", Left(item.UnitTypeTitle, 3))
        '            Dim strItemDisc = Localization.GetString("ItemDisc", LocalResourceFile).Replace("[ITEMDISC:", FormatPercent((item.ProductDiscount) / 100))

        '            strItemName = Left(strItemName, itemName.Length)
        '            strItemRef = Left(strItemRef, Right(itemCode, hfEId.Value).Length) & Environment.NewLine
        '            strItemPrice = Left(strItemPrice, FormatCurrency(item.ProductEstimateOriginalPrice).Length)
        '            strItemQty = Left(strItemQty, CStr(item.ProductQty).Length)
        '            strItemUni = Left(strItemUni, Left(item.UnitTypeTitle, 3).Length)
        '            strItemDisc = Left(strItemDisc, FormatPercent((item.ProductDiscount) / 100).Length)

        '            saveString += String.Concat(itemRef.PadRight(7),
        '                              strItemName.PadRight(CInt(padItemName)),
        '                              strItemRef.PadRight(CInt(padItemRef)),
        '                              strItemPrice.PadRight(CInt(padPrice)),
        '                              strItemQty.PadRight(CInt(padQty)),
        '                              strItemUni.PadRight(CInt(padUni)),
        '                              strItemDisc.PadRight(CInt(padDisc)),
        '                              FormatCurrency(totalLiquid) & Environment.NewLine)

        '            totalItems = totalItems + totalLiquid

        '        Next

        '        Dim lPadTotalDisc = Left(Localization.GetString("Discount", LocalResourceFile).Replace("[", ""), 2).TrimStart("0"c)
        '        Dim rPadTotalDisc = Right(Localization.GetString("Discount", LocalResourceFile).Replace("]", ""), 2).TrimStart("0"c)
        '        Dim strDiscount = ""
        '        Dim DiscountValue = totalBeforeDiscount - totalItems
        '        Dim Total As Single = totalBeforeDiscount - DiscountValue

        '        'Total = Math.Round(Total - (Total * estimate_Info.Discount / 100), 2)

        '        If estimate_Info.Discount > 0 Then
        '            DiscountValue = DiscountValue + (totalItems * estimate_Info.Discount / 100)
        '            Total = Math.Round(Total - DiscountValue)
        '        End If

        '        If DiscountValue > 0.01 Then
        '            DiscountValue = String.Format("{0:C}", Math.Floor(DiscountValue * 100) / 100)
        '            strDiscount = String.Concat("DESCONTO:".PadRight(CInt(rPadTotalDisc)), Localization.GetString("Discount", LocalResourceFile).Replace(String.Format("[{0}:DISCOUNT:", lPadTotalDisc), FormatCurrency(DiscountValue))).PadRight(CInt(rPadTotalDisc))
        '            strDiscount = strDiscount.Replace(rPadTotalDisc & "]", "")
        '        Else
        '            strDiscount = ""
        '        End If

        '        Dim rPadSubTotal = Right(Localization.GetString("SubTotal", LocalResourceFile).Replace("]", ""), 2).TrimStart("0"c)
        '        Dim rPadTotal = Right(Localization.GetString("Total", LocalResourceFile).Replace("]", ""), 2).TrimStart("0"c)

        '        Dim lPadSubTotal = Left(Localization.GetString("SubTotal", LocalResourceFile).Replace("[", ""), 2).TrimStart("0"c)
        '        Dim lPadTotal = Left(Localization.GetString("Total", LocalResourceFile).Replace("[", ""), 2).TrimStart("0"c)

        '        Dim strSubTotal = String.Concat("SUBTOTAL:".PadRight(CInt(rPadSubTotal)), Localization.GetString("SubTotal", LocalResourceFile).Replace(String.Format("[{0}:SUBTOTAL:", lPadSubTotal), FormatCurrency(totalBeforeDiscount))).PadRight(CInt(rPadSubTotal))
        '        Dim strTotal = String.Concat("TOTAL:".PadRight(CInt(rPadTotal)), Localization.GetString("Total", LocalResourceFile).Replace(String.Format("[{0}:TOTAL:", lPadTotal), FormatCurrency(Total))).PadRight(CInt(rPadTotal))

        '        strSubTotal = strSubTotal.Replace(rPadSubTotal & "]", "")
        '        strTotal = strTotal.Replace(rPadTotal & "]", "")

        '        saveString += String.Concat(Environment.NewLine, strSubTotal.PadLeft(CInt(lPadSubTotal)), CStr(IIf(strDiscount.Length > 0, Environment.NewLine & strDiscount.PadLeft(CInt(lPadTotalDisc)), "")), Environment.NewLine, strTotal.PadLeft(CInt(lPadTotal)))

        '        Dim paidVal = CDec(hfBankIn.Value.Replace(".", ",")) + CDec(hfCheckIn.Value.Replace(".", ",")) + CDec(hfCardIn.Value.Replace(".", ",")) + CDec(hfCashIn.Value.Replace(".", ","))

        '        'saveString += String.Concat(Environment.NewLine & Environment.NewLine & "PAGO:".PadRight(CInt(rPadTotal)), Localization.GetString("TotalPaid", LocalResourceFile).Replace(String.Format("[{0}:TOTALPAID:", lPadTotal), FormatCurrency(paidVal))).PadRight(CInt(rPadTotal)).Replace("]", "")

        '        saveString += String.Concat(Environment.NewLine, Environment.NewLine, "Forma de Pagamento:", Environment.NewLine)

        '        saveString += CStr(IIf(CInt(hfBankIn.Value) > 0, "Boleto:".PadRight(10) & FormatCurrency(hfBankIn.Value.Replace(".", ",")) & Environment.NewLine, ""))
        '        saveString += CStr(IIf(CInt(hfCheckIn.Value) > 0, "Cheque:".PadRight(10) & FormatCurrency(hfCheckIn.Value.Replace(".", ",")) & Environment.NewLine, ""))
        '        saveString += CStr(IIf(CInt(hfCardIn.Value) > 0, "Cartão:".PadRight(10) & FormatCurrency(hfCardIn.Value.Replace(".", ",")) & Environment.NewLine, ""))
        '        saveString += CStr(IIf(CInt(hfCashIn.Value) > 0, "Dinheiro:".PadRight(10) & FormatCurrency(hfCashIn.Value.Replace(".", ",")) & Environment.NewLine, ""))

        '        Dim change = CSng(IIf(paidVal > Total, (paidVal - Total), 0))
        '        saveString += CStr("Troco:".PadRight(10) & FormatCurrency(change))
        '        'saveString += String.Concat(Environment.NewLine & "TROCO:".PadRight(CInt(rPadTotal)), Localization.GetString("Change", LocalResourceFile).Replace(String.Format("[{0}:CHANGE:", lPadTotal), IIf((paidVal - Total) > 0, FormatCurrency(paidVal - Total), FormatCurrency(0)))).PadRight(CInt(rPadTotal)).Replace("]", "")

        '        If estimate_Info.PayCondType.Length > 1 Then

        '            saveString += String.Concat(Environment.NewLine, "Condicao de Pagamento:", Environment.NewLine)

        '            saveString += String.Concat(Environment.NewLine, Localization.GetString("ConditionColumnHeader", LocalResourceFile).Replace("[BR]", Environment.NewLine))

        '            Dim padcheck = Right(Localization.GetString("Check", LocalResourceFile), 3).TrimStart("0"c).Replace("]", "")
        '            saveString += String.Concat(Environment.NewLine, estimate_Info.PayCondType.PadRight(CInt(padcheck)))

        '            Dim padPayQty = Right(Localization.GetString("PayQty", LocalResourceFile), 3).TrimStart("0"c).Replace("]", "")
        '            saveString += Left(Localization.GetString("PayQty", LocalResourceFile).Replace("[", ""), "PAYQTY".Length).Replace("PAYQTY", estimate_Info.PayCondN).PadRight(CInt(padPayQty))

        '            Dim padResultPayment = Right(Localization.GetString("Payments", LocalResourceFile), 3).TrimStart("0"c).Replace("]", "")
        '            saveString += Left(Localization.GetString("Payments", LocalResourceFile).Replace("[", ""), "PAYMENTS".Length).Replace("PAYMENTS", FormatCurrency(estimate_Info.PayCondInst)).PadRight(CInt(padResultPayment)) & Environment.NewLine

        '            Dim padInitialPay = Right(Localization.GetString("InitialPay", LocalResourceFile), 3).TrimStart("0"c).Replace("]", "")
        '            saveString += Left(Localization.GetString("InitialPay", LocalResourceFile).Replace("[", ""), "INITIALPAY".Length).Replace("INITIALPAY", FormatCurrency(estimate_Info.PayCondIn)).PadRight(CInt(padInitialPay))

        '            Dim padInterest = Right(Localization.GetString("Interest", LocalResourceFile), 3).TrimStart("0"c).Replace("]", "")
        '            saveString += Left(Localization.GetString("Interest", LocalResourceFile).Replace("[", ""), "INTEREST".Length).Replace("INTEREST", FormatPercent(estimate_Info.PayCondPerc / 100)).PadRight(CInt(padInterest))

        '            Dim padTotalPays = Right(Localization.GetString("TotalPays", LocalResourceFile), 3).TrimStart("0"c).Replace("]", "")
        '            saveString += Left(Localization.GetString("TotalPays", LocalResourceFile).Replace("[", ""), "TOTALPAYS".Length).Replace("TOTALPAYS", FormatCurrency(estimate_Info.TotalPayCond)).PadRight(CInt(padTotalPays))

        '            saveString += CStr(IIf(estimate_Info.PayCondInterval > 0, CStr(estimate_Info.PayCondInterval), "Mensal"))

        '        End If

        '        saveString += String.Format("{0}{0}{1}{0}", Environment.NewLine, "Observacoes Importantes:")
        '        Dim estimateComm = estimate_Info.Inst
        '        Dim strTerms = Utilities.RemoveHtmlTags(Utilities.ReplaceAccentletters(HttpUtility.HtmlDecode(Portals.PortalController.GetPortalSetting("RIW_EstimateTerm", PortalId, ""))))
        '        saveString += CStr(IIf(estimateComm.Length > 0, String.Format("{0}{1}{0}", Environment.NewLine, estimateComm), "")) & String.Format("{0}{1}", Environment.NewLine, strTerms)

        '        Dim file_txt = String.Format("filename=Orcamento_{0}_{1}.txt", hfEId.Value, Replace(DateTime.Now.ToString(), ":", "-").Replace("/", "-"))

        '        Response.Clear()
        '        Response.Expires = -1
        '        Response.ContentType = "plain/text"
        '        Response.AppendHeader("content-disposition", "attachment; " & file_txt)
        '        Response.AppendHeader("Content-Length", saveString.Length.ToString())
        '        Response.Write(saveString)
        '        Response.End()

        '    Catch exc As Exception    'Module failed to load
        '        ProcessModuleLoadException(Me, exc)
        '    End Try
        'End Sub

        #End Region

        #Region "Events"

        Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            ClientResourceManagement.ClientResourceManager.RegisterScript(Parent.Page, String.Format("DesktopModules/RildoInfo/{0}/viewmodels/viewQuick_Estimate.js", Me.ModuleConfiguration.DesktopModule.FolderName), 99)
        End Sub

        Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
            'If Not HasModuleAccess(SecurityAccessLevel.Edit, "EXCLUDEPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "EDITPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "INSERTPERMISSION", ModuleConfiguration) Then
            '    btnPrintCompact.Disabled = True
            '    btnPrintCompact.Attributes.Add("title", "Desativado")
            '    btnPrintCompact.Attributes.Add("class", "k-state-disabled")
            'End If
            End If
            RegisterModuleInfo()
        End Sub

        Public Sub RegisterModuleInfo()

            'Dim modCtrl As New Entities.Modules.ModuleController()

            Dim scriptblock = New StringBuilder()
            scriptblock.Append("<script>")
            scriptblock.Append(String.Format("var ascxVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(String.Format("{0}DesktopModules\RildoInfo\{1}\views\view_quick_estimate.ascx", Request.PhysicalApplicationPath, Me.ModuleConfiguration.DesktopModule.FolderName)).ToString("g")))
            scriptblock.Append(String.Format("var jsVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(String.Format("{0}DesktopModules\RildoInfo\{1}\viewmodels\viewQuick_Estimate.js", Request.PhysicalApplicationPath, Me.ModuleConfiguration.DesktopModule.FolderName)).ToString("g")))
            scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(RiwModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), RiwModulePathScript, scriptblock.ToString())
            End If
        End Sub

        'Protected Sub printReceipt_Click(sender As Object, e As EventArgs) Handles btnPrintCompact.ServerClick
        '    PrintEstimateWindow()
        'End Sub

        'Private Sub btnPrint_ServerClick(sender As Object, e As EventArgs) Handles btnPrint.ServerClick
        '    PrintEstimateReceipt()
        'End Sub

        #End Region

    End Class

End Namespace
