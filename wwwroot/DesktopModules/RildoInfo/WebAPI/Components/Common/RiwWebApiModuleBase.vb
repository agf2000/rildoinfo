'Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Web.Client.ClientResourceManagement
'Imports DotNetNuke.Security.Permissions.ModulePermissionController
Imports RIW.Modules.Common.Utilities

Namespace Components.Common

    Public Class RiwWebApiModuleBase
    Inherits PortalModuleBase

        Private Const RiwModulePathScript As String = "riw_Web_API_ModulePathScript"

        Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            jQuery.RequestDnnPluginsRegistration()
            RegisterModulePath()
        End Sub

        Public Sub RegisterModulePath()

            Dim intAuthorized As Integer = -1

            If UserInfo.IsInRole("Gerentes") Then
                intAuthorized = 3
            ElseIf UserInfo.IsInRole("Vendedores") Then
                intAuthorized = 1
            ElseIf UserInfo.IsInRole("Editores") Then
                intAuthorized = 0
            ElseIf UserInfo.IsInRole("Caixas") Then
                intAuthorized = 2
            End If

            If Not UserId = -1 Then
                If Not UserInfo.IsSuperUser Then
                    If TabController.CurrentPage.FullUrl.ToLower().IndexOf("perfil") > -1 Then
                        If intAuthorized >= 0 Then
                            Response.Redirect(NavigateURL(GetPageID("RIW My Profile", PortalId).TabID, "", "?sel=1"))
                        End If
                    End If
                End If
            End If

            Dim mCtrl As New DotNetNuke.Entities.Modules.ModuleController()
            Dim msgModuleInfo = mCtrl.GetModuleByDefinition(PortalId, "Message Center")

            Dim photoPath = ""
            If UserInfo.Profile.Photo IsNot Nothing Then
                If UserInfo.Profile.Photo.Length > 2 Then
                    photoPath = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(UserInfo).FolderPath
                    photoPath = photoPath & DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(CInt(UserInfo.Profile.Photo)).FileName
                End If
            End If

            Dim asm = System.Reflection.Assembly.Load("RIW.Modules.WebAPI")

            Dim settingCtrl As New WebAPI.Components.Repositories.SettingsRepository
            Dim settingsDictionay = settingCtrl.GetSettings(PortalId)

            Dim customerLogo = ""
            If settingsDictionay("bW_Logo") <> "" AndAlso Not settingsDictionay("bW_Logo") = "-1" Then
                If CStr(settingsDictionay("bW_Logo")).IndexOf("Portal") > 0 Then
                    customerLogo = settingsDictionay("bW_Logo")
                Else
                    customerLogo = PortalSettings.HomeDirectory & CStr(settingsDictionay("bW_Logo"))
                End If
            Else
                If PortalSettings.LogoFile <> "" Then
                    If Not PortalSettings.LogoFile.IndexOf("Portal") = -1 Then
                        customerLogo = PortalSettings.LogoFile
                    Else
                        customerLogo = PortalSettings.HomeDirectory & PortalSettings.LogoFile
                    End If
                End If
            End If

            Dim bwLogoUrl = ""
            Dim bwLogoFile = ""
            If settingsDictionay("bW_Logo") <> "" AndAlso Not settingsDictionay("bW_Logo") = "-1" Then
                bwLogoUrl = settingsDictionay("bW_Logo")
                bwLogoFile = settingsDictionay("bW_Logo")
            Else
                bwLogoUrl = PortalSettings.LogoFile
                bwLogoFile = PortalSettings.LogoFile
            End If

            Dim strFooter = New StringBuilder()
            strFooter.Append(String.Format("{0} nº: {1} - {2}", settingsDictionay("storeAddress"), settingsDictionay("storeUnit"), settingsDictionay("storeComplement")))
            strFooter.Append("[NEWLINE]")
            strFooter.Append(String.Format(" Bairro: {0}", settingsDictionay("storeDistrict")))
            strFooter.Append("[NEWLINE]")
            strFooter.Append(String.Format("{0} - {1} - {2} ", settingsDictionay("storeCity"), settingsDictionay("storeRegion"), settingsDictionay("storeCountry")))
            If settingsDictionay("storePostalCode") <> "" AndAlso Not settingsDictionay("storePostalCode") = "-1" Then
                strFooter.Append(String.Format("CEP: {0}", ZipMask(settingsDictionay("storePostalCode"))))
            End If

            strFooter.Append("[NEWLINE]")

            If settingsDictionay("storePhone1") <> "" AndAlso Not settingsDictionay("storePhone1") = "-1" Then
                Dim strOptions = settingsDictionay("storePhone1").Split(CChar(vbCrLf))
                If strOptions.Length > 0 Then
                    For i = 0 To strOptions.Length - 1
                        strOptions(i) = strOptions(i).Trim()
                        If strOptions(i).StartsWith("vo") Then
                            strFooter.Append(String.Format("Fone: {0} ", PhoneMask(strOptions(i).Remove(0, 4))))
                        End If
                        If strOptions(i).StartsWith("fa") Then
                            strFooter.Append(String.Format("Fax: {0} ", PhoneMask(strOptions(i).Remove(0, 4))))
                        End If
                        If strOptions(i).StartsWith("ce") Then
                            strFooter.Append(String.Format("Cel: {0} ", PhoneMask(strOptions(i).Remove(0, 4))))
                        End If
                    Next
                End If
            End If

            If settingsDictionay("storePhone2") <> "" AndAlso Not settingsDictionay("storePhone2") = "-1" Then
                Dim strOptions = settingsDictionay("storePhone2").Split(CChar(vbCrLf))
                If strOptions.Length > 0 Then
                    For i = 0 To strOptions.Length - 1
                        strOptions(i) = strOptions(i).Trim()
                        If strOptions(i).StartsWith("vo") Then
                            strFooter.Append(String.Format("Fone: {0} ", PhoneMask(strOptions(i).Remove(0, 4))))
                        End If
                        If strOptions(i).StartsWith("fa") Then
                            strFooter.Append(String.Format("Fax: {0} ", PhoneMask(strOptions(i).Remove(0, 4))))
                        End If
                        If strOptions(i).StartsWith("ce") Then
                            strFooter.Append(String.Format("Cel: {0} ", PhoneMask(strOptions(i).Remove(0, 4))))
                        End If
                    Next
                End If
            End If

            'Dim theUsers As New Users.UserController

            Dim scriptblock = New StringBuilder()
            scriptblock.Append("<script type='text/javascript' id='riw_web_api'>")
            scriptblock.Append(String.Format("var portalID = ""{0}""; ", PortalId))
            scriptblock.Append(String.Format("var authorized = {0}; ", intAuthorized))
            scriptblock.Append(String.Format("var msgModuleTitle = ""{0}""; ", msgModuleInfo.ModuleTitle))
            scriptblock.Append(String.Format("var userID = {0}; ", UserInfo.UserID))
            scriptblock.Append(String.Format("var userName = ""{0}""; ", UserInfo.Username))
            scriptblock.Append(String.Format("var userInfoDisplayName = ""{0}""; ", UserInfo.DisplayName))
            'scriptblock.Append(String.Format("var email = ""{0}""; ", UserInfo.Email))
            scriptblock.Append(String.Format("var userInfoEmail = ""{0}""; ", UserInfo.Email))
            scriptblock.Append(String.Format("var avatar = ""{0}""; ", photoPath))

            Dim roleCtrl As New DotNetNuke.Security.Roles.RoleController
            scriptblock.Append(String.Format("var clientsRoleId = ""{0}""; ", roleCtrl.GetRoleByName(PortalId, "Clientes").RoleID.ToString()))

            scriptblock.Append(String.Format("var managerRole = ""{0}""; ", UserInfo.IsInRole("Gerentes")))
            scriptblock.Append(String.Format("var userMsgsURL = ""{0}""; ", "/mensagens"))
            scriptblock.Append(String.Format("var userNotisURL = ""{0}""; ", "/mensagens?view=notifications"))
            scriptblock.Append(String.Format("var bwLogoURL = ""{0}""; ", bwLogoUrl))
            scriptblock.Append(String.Format("var bwLogoFile = ""{0}""; ", bwLogoFile))
            scriptblock.Append(String.Format("var customerLogo = ""{0}""; ", customerLogo))
            scriptblock.Append(String.Format("var pdfFooter = ""{0}""; ", strFooter.ToString()))
            scriptblock.Append(String.Format("var footerText = '{0}'; ", PortalSettings.FooterText.Replace("[year]", Now.Year.ToString())))
            scriptblock.Append(String.Format("var userProfileURL = ""{0}""; ", NavigateURL(GetPageID("RIW My Profile", PortalId).TabID, "", "?sel=1")))
            scriptblock.Append(String.Format("var siteName = ""{0}""; ", PortalSettings.PortalName))
            scriptblock.Append(String.Format("var description = ""{0}""; ", PortalSettings.Description))
            scriptblock.Append(String.Format("var keywords = ""{0}""; ", PortalSettings.KeyWords))
            scriptblock.Append(String.Format("var siteURL = ""{0}""; ", PortalSettings.PortalAlias.HTTPAlias))
            scriptblock.Append(String.Format("var logoURL = ""{0}""; ", PortalSettings.HomeDirectory & PortalSettings.LogoFile))
            scriptblock.Append(String.Format("var logoFile = ""{0}""; ", PortalSettings.LogoFile))
            scriptblock.Append(String.Format("var returnURL = ""{0}""; ", NavigateURL()))
            '_scriptblock.Append(String.Format("var _detailURL = ""{0}""; ", EditUrl()))
            'scriptblock.Append(String.Format("var watermark = ""{0}""; ", settingsDictionay("ProductWatermark")))
            scriptblock.Append(String.Format("var orURL = ""{0}""; ", NavigateURL(GetPageID("RIW Quick Estimate", PortalId).TabID)))
            scriptblock.Append(String.Format("var entitiesManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW People Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var estimatesManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Estimates Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var salesManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Estimates Manager", PortalId).TabID, "", "vendas=1")))

            For Each setting In settingsDictionay
                scriptblock.Append(String.Format("var {0} = ""{1}""; ", setting.Key, setting.Value))
            Next

            'scriptblock.Append(String.Format("var salesPerson = ""{0}""; ", settingsDictionay("SalesPerson")))
            'scriptblock.Append(String.Format("var viewPrice = ""{0}""; ", settingsDictionay("ShowEstimatePrice").ToLower()))
            'scriptblock.Append(String.Format("var pageSize = ""{0}""; ", settingsDictionay("PageSize")))
            'scriptblock.Append(String.Format("var watermark = ""{0}""; ", settingsDictionay("ProductWatermark")))
            'scriptblock.Append(String.Format("var storeStreet = ""{0}""; ", settingsDictionay("StoreAddress")))
            'scriptblock.Append(String.Format("var storeUnit = ""{0}""; ", settingsDictionay("StoreUnit")))
            'scriptblock.Append(String.Format("var storeComplement = ""{0}""; ", settingsDictionay("StoreComplement")))
            'scriptblock.Append(String.Format("var storeDistrict = ""{0}""; ", settingsDictionay("StoreDistrict")))
            'scriptblock.Append(String.Format("var storeCity = ""{0}""; ", settingsDictionay("StoreCity")))
            'scriptblock.Append(String.Format("var storePostalCode = ""{0}""; ", settingsDictionay("StorePostalCode")))
            'scriptblock.Append(String.Format("var storeRegion = ""{0}""; ", settingsDictionay("StoreRegion")))
            'scriptblock.Append(String.Format("var storeCountry = ""{0}""; ", settingsDictionay("StoreCountry")))
            'scriptblock.Append(String.Format("var storeEmail = ""{0}""; ", settingsDictionay("StoreEmail")))
            'scriptblock.Append(String.Format("var storeReplyEmail = ""{0}""; ", settingsDictionay("StoreReplyEmail")))
            'scriptblock.Append(String.Format("var maxDiscount = {0}; ", settingsDictionay("EstimateMaxDiscount")))
            'scriptblock.Append(String.Format("var storePhone = ""{0}""; ", settingsDictionay("StorePhones").ToLower()))
            'scriptblock.Append(String.Format("var mainConsumerId = ""{0}""; ", settingsDictionay("MainConsumer")))
            'scriptblock.Append(String.Format("var mainConsumer = ""{0}""; ", theUsers.GetUser(PortalId, settingsDictionay("MainConsumer")).DisplayName))
            'scriptblock.Append(String.Format("var noStockAllowed = ""{0}""; ", settingsDictionay("NoStockAllowed").ToLower()))
            'scriptblock.Append(String.Format("var maxDuration = ""{0}""; ", settingsDictionay("EstimateMaxDuration")))
            'scriptblock.Append(String.Format("var estimateTerm = ""{0}""; ", settingsDictionay("EstimateTerm")))
            'scriptblock.Append(String.Format("var allowPurchase = ""{0}""; ", settingsDictionay("AllowPurchase").ToLower()))
            'scriptblock.Append(String.Format("var askLastName = ""{0}""; ", settingsDictionay("askLastName").ToLower()))
            'scriptblock.Append(String.Format("var askIndustry = ""{0}""; ", settingsDictionay("askIndustry").ToLower()))
            'scriptblock.Append(String.Format("var askTelephone = ""{0}""; ", settingsDictionay("askTelephone").ToLower()))
            'scriptblock.Append(String.Format("var askEIN = ""{0}""; ", settingsDictionay("askEIN").ToLower()))
            'scriptblock.Append(String.Format("var askCompany = ""{0}""; ", settingsDictionay("askCompany").ToLower()))
            'scriptblock.Append(String.Format("var askST = ""{0}""; ", settingsDictionay("askST").ToLower()))
            'scriptblock.Append(String.Format("var askCT = ""{0}""; ", settingsDictionay("askCT").ToLower()))
            'scriptblock.Append(String.Format("var askWebsite = ""{0}""; ", settingsDictionay("askWebsite").ToLower()))
            'scriptblock.Append(String.Format("var askSSN = ""{0}""; ", settingsDictionay("askSSN").ToLower()))
            'scriptblock.Append(String.Format("var askIdent = ""{0}""; ", settingsDictionay("askIdent").ToLower()))
            'scriptblock.Append(String.Format("var askAddress = ""{0}""; ", settingsDictionay("askAddress").ToLower()))
            'scriptblock.Append(String.Format("var reqAddress = ""{0}""; ", settingsDictionay("reqAddress").ToLower()))
            'scriptblock.Append(String.Format("var reqLastName = ""{0}""; ", settingsDictionay("reqLastName").ToLower()))
            'scriptblock.Append(String.Format("var reqSSN = ""{0}""; ", settingsDictionay("reqSSN").ToLower()))
            'scriptblock.Append(String.Format("var reqIdent = ""{0}""; ", settingsDictionay("reqIdent").ToLower()))
            'scriptblock.Append(String.Format("var reqWebsite = ""{0}""; ", settingsDictionay("reqWebsite").ToLower()))
            'scriptblock.Append(String.Format("var reqTelephone = ""{0}""; ", settingsDictionay("reqTelephone").ToLower()))
            'scriptblock.Append(String.Format("var reqEIN = ""{0}""; ", settingsDictionay("reqEIN").ToLower()))
            'scriptblock.Append(String.Format("var reqCompany = ""{0}""; ", settingsDictionay("reqCompany").ToLower()))
            'scriptblock.Append(String.Format("var reqST = ""{0}""; ", settingsDictionay("reqST").ToLower()))
            'scriptblock.Append(String.Format("var reqCT = ""{0}""; ", settingsDictionay("reqCT").ToLower()))
            'scriptblock.Append(String.Format("var reqIndustry = ""{0}""; ", settingsDictionay("reqIndustry").ToLower()))
            'scriptblock.Append(String.Format("var reqCPF = ""{0}""; ", settingsDictionay("reqSSN").ToLower()))
            'scriptblock.Append(String.Format("var smtpServer = ""{0}""; ", settingsDictionay("smtpServer")))
            'scriptblock.Append(String.Format("var smtpPort = ""{0}""; ", settingsDictionay("smtpPort")))
            'scriptblock.Append(String.Format("var smtpLogin = ""{0}""; ", settingsDictionay("smtpLogin")))
            'scriptblock.Append(String.Format("var smtpPassword = ""{0}""; ", settingsDictionay("smtpPassword")))
            'scriptblock.Append(String.Format("var smtpConnection = ""{0}""; ", settingsDictionay("smtpConnection").ToLower()))
            'scriptblock.Append(String.Format("var agendaInfo = ""{0}""; ", settingsDictionay("AgendaMessage"))) ' Localization.GetString("AgendaInfo", String.Format("{0}/App_LocalResources/{1}", TemplateSourceDirectory, Localization.LocalSharedResourceFile))))
            'scriptblock.Append(String.Format("var msgContent = ""{0}""; ", settingsDictionay("PasswordMessage"))) ' Localization.GetString("PasswordBody", String.Format("{0}/App_LocalResources/{1}", TemplateSourceDirectory, Localization.LocalSharedResourceFile))))
            'scriptblock.Append(String.Format("var passwordSubject = ""{0}""; ", settingsDictionay("PasswordSubject"))) ' Localization.GetString("PasswordSubject", String.Format("{0}/App_LocalResources/{1}", TemplateSourceDirectory, Localization.LocalSharedResourceFile))))
            'scriptblock.Append(String.Format("var passwordBody = ""{0}""; ", settingsDictionay("PasswordMessage"))) ' Localization.GetString("PasswordBody", String.Format("{0}/App_LocalResources/{1}", TemplateSourceDirectory, Localization.LocalSharedResourceFile))))
            'scriptblock.Append(String.Format("var defaultClient = ""{0}""; ", settingsDictionay("MainConsumer")))
            'scriptblock.Append(String.Format("var discountMax = ""{0}""; ", settingsDictionay("EstimateMaxDiscount")))
            scriptblock.Append(String.Format("var userName = ""{0}""; ", UserInfo.Username))
            scriptblock.Append(String.Format("var displayName = ""{0}""; ", UserInfo.DisplayName))
            scriptblock.Append(String.Format("var firstName = ""{0}""; ", UserInfo.FirstName))
            scriptblock.Append(String.Format("var lastName = ""{0}""; ", UserInfo.LastName))
            scriptblock.Append(String.Format("var telephone = ""{0}""; ", UserInfo.Profile.Telephone))
            scriptblock.Append(String.Format("var cell = ""{0}""; ", UserInfo.Profile.Cell))
            scriptblock.Append(String.Format("var fax = ""{0}""; ", UserInfo.Profile.Fax))
            scriptblock.Append(String.Format("var biography = ""{0}""; ", UserInfo.Profile.Biography))
            scriptblock.Append(String.Format("var postalCode = ""{0}""; ", UserInfo.Profile.PostalCode))
            scriptblock.Append(String.Format("var street = ""{0}""; ", UserInfo.Profile.Street))
            scriptblock.Append(String.Format("var unit = ""{0}""; ", UserInfo.Profile.Unit))
            scriptblock.Append(String.Format("var complement = ""{0}""; ", UserInfo.Profile.GetPropertyValue("IM")))
            scriptblock.Append(String.Format("var district = ""{0}""; ", UserInfo.Profile.GetPropertyValue("LinkedIn")))
            scriptblock.Append(String.Format("var city = ""{0}""; ", UserInfo.Profile.City))
            scriptblock.Append(String.Format("var region = ""{0}""; ", UserInfo.Profile.Region))
            scriptblock.Append(String.Format("var country = ""{0}""; ", UserInfo.Profile.Country))
            scriptblock.Append(String.Format("var email = ""{0}""; ", UserInfo.Email))
            'scriptblock.Append(String.Format("var avatar = ""{0}""; ", avatarPath))
            scriptblock.Append(String.Format("var userFolder = ""{0}""; ", DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(UserInfo).FolderPath))
            scriptblock.Append(String.Format("var lastPasswordChangeDate = ""{0}""; ", UserInfo.Membership.LastPasswordChangeDate.ToString()))
            scriptblock.Append(String.Format("var asmVersion = ""{0}""; ", asm.GetName().Version))
            scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(RiwModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), RiwModulePathScript, scriptblock.ToString())
            End If

            ' register javascript libraries
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.signalR-1.1.4.min.js", 19)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/kendo.web.min.js", 22)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/cultures/kendo.culture.pt-BR.min.js", 23)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/messages/kendo.messages.pt-BR.min.js", 24)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/modernizr.min.js", 25)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/pnotify/jquery.pnotify.min.js", 26)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.scrollTo.min.js", 27)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/moment.min.js", 28)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/moment-with-langs.min.js", 29)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/showdown.js", 30)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/app/utilities.js", 31)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/amplify.min.js", 32)
            ClientResourceManager.RegisterScript(Parent.Page, "/signalr/hubs", 60)

            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.css", 75)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.icons.css", 76)

        End Sub

    End Class

End Namespace
