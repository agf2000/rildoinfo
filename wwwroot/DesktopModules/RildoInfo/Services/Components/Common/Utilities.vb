
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities
Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Web
Imports RIW.Modules.Services
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Services.Localization

Public Class Utilities

#Region "Utilities"

    Public Shared Function GetValidLocales() As LocaleCollection
        'TODO: Change this to DNN5 portal localization when we goto DNN5 only
        Dim supportedLanguages As LocaleCollection = DotNetNuke.Services.Localization.Localization.GetEnabledLocales()
        If supportedLanguages.Count = 0 Then
            ' the getenabledlocales doesn;t work correct in DNN5, so use this as a fallback
            supportedLanguages = DotNetNuke.Services.Localization.Localization.GetSupportedLocales()
        End If

        Return supportedLanguages
    End Function

    Public Shared Sub CreateDir(ByVal PortalSettings As DotNetNuke.Entities.Portals.PortalSettings, ByVal FolderName As String)
        Dim folderInfo As DotNetNuke.Services.FileSystem.FolderInfo
        Dim blnCreated As Boolean = False

        'try normal test (doesn;t work on medium trust, but avoids waiting for GetFolder.)
        Try
            blnCreated = System.IO.Directory.Exists(PortalSettings.HomeDirectoryMapPath & FolderName)
        Catch ex As Exception
            blnCreated = False
        End Try

        If Not blnCreated Then
            FolderManager.Instance().Synchronize(PortalSettings.PortalId, PortalSettings.HomeDirectory, True, True)
            folderInfo = CType(FolderManager.Instance().GetFolder(PortalSettings.PortalId, FolderName), DotNetNuke.Services.FileSystem.FolderInfo)
            If folderInfo Is Nothing And FolderName <> "" Then
                'add folder and permissions
                Try
                    FolderManager.Instance().AddFolder(PortalSettings.PortalId, FolderName)
                Catch ex As Exception
                End Try
                folderInfo = CType(FolderManager.Instance().GetFolder(PortalSettings.PortalId, FolderName), DotNetNuke.Services.FileSystem.FolderInfo)
                If Not folderInfo Is Nothing Then
                    Dim folderid As Integer = folderInfo.FolderID
                    Dim objPermissionController As New DotNetNuke.Security.Permissions.PermissionController
                    Dim arr As ArrayList = objPermissionController.GetPermissionByCodeAndKey("SYSTEM_FOLDER", "")
                    For Each objpermission As DotNetNuke.Security.Permissions.PermissionInfo In arr
                        If objpermission.PermissionKey = "WRITE" Then
                            ' add READ permissions to the All Users Role
                            FolderManager.Instance().SetFolderPermission(folderInfo, objpermission.PermissionID, Integer.Parse(glbRoleAllUsers))
                        End If
                    Next
                End If
            End If
        End If
    End Sub

    Public Shared Function ToNullableDateTime([date] As String) As System.Nullable(Of DateTime)
        Dim _dateTime As DateTime
        Return If((DateTime.TryParse([date], _dateTime)), CType(_dateTime, System.Nullable(Of DateTime)), Nothing)
    End Function


    Public Shared Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        Dim Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function

    Public Shared Function GetIPAddress() As String
        Dim _IPAddress As String
        If HttpContext.Current.Request.UserHostAddress IsNot Nothing Then
            _IPAddress = HttpContext.Current.Request.UserHostAddress
        Else
            _IPAddress = Null.NullString
        End If
        Return _IPAddress
    End Function

    Public Shared Function deleteFile(filename As String) As String
        Dim ret As String = ""

        If IO.File.Exists(filename) = True Then
            Dim timedOut As Boolean = False
            Dim deleted As Boolean = False
            Dim starttime As DateTime = Now
            Do Until deleted OrElse timedOut
                Try
                    IO.File.Delete(filename)
                    deleted = True
                Catch ex As Exception
                    deleted = False
                    timedOut = DateAdd(DateInterval.Second, 2, starttime) < Now
                    If timedOut Then
                        ret = "Could not delete destination File. Error: " + ex.Message
                        Exit Do
                    End If
                End Try
            Loop
        End If

        Return ret
    End Function

    Public Shared Function ResizeImage(ByVal image As Image, ByVal size As Size, Optional ByVal preserveAspectRatio As Boolean = True) As Image
        Dim newWidth As Integer
        Dim newHeight As Integer
        If preserveAspectRatio Then
            Dim originalWidth As Integer = image.Width
            Dim originalHeight As Integer = image.Height
            Dim percentWidth As Single = CSng(size.Width) / CSng(originalWidth)
            Dim percentHeight As Single = CSng(size.Height) / CSng(originalHeight)
            Dim percent As Single = If(percentHeight < percentWidth,
        percentHeight, percentWidth)
            newWidth = CInt(originalWidth * percent)
            newHeight = CInt(originalHeight * percent)
        Else
            newWidth = size.Width
            newHeight = size.Height
        End If
        Dim newImage As Image = New Bitmap(newWidth, newHeight)
        Using graphicsHandle As Graphics = Graphics.FromImage(newImage)
            graphicsHandle.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight)
        End Using
        Return newImage
    End Function

    Public Shared Function GetModInfo(modefName As String, PortalId As Integer) As Entities.Modules.ModuleInfo
        Dim modCtrl As New Entities.Modules.ModuleController
        Dim modInfo = modCtrl.GetModuleByDefinition(PortalId, modefName)
        Return modInfo
    End Function

    Public Shared Function GetPageID(modDef As String, PortalId As Integer) As DotNetNuke.Entities.Tabs.TabInfo
        Dim page As New DotNetNuke.Entities.Tabs.TabInfo
        Dim _module = GetModInfo(modDef, PortalId) '"RIStore Estimate"
        Dim pages = Tabs.TabController.GetPortalTabs(PortalId, Null.NullInteger, False, True, False, False)
        For Each ti As Entities.Tabs.TabInfo In pages
            ' For each page, find all the modules on the page
            Dim modCtrl = New Entities.Modules.ModuleController()
            Dim modules As Dictionary(Of Integer, Entities.Modules.ModuleInfo) = modCtrl.GetTabModules(ti.TabID)
            For Each mi As Entities.Modules.ModuleInfo In modules.Values
                ' If the module on the page is the module we
                ' are looking for, return it.
                If mi.DesktopModuleID = _module.DesktopModuleID Then
                    page = ti
                End If
            Next
        Next
        Return page
    End Function

    'Public Shared Function ValidatePerson(vTerm As String) As Integer
    '    Dim clients As IEnumerable(Of Models.Person)
    '    Using context As IDataContext = DataContext.Instance()
    '        Dim rep = context.GetRepository(Of Models.Person)()
    '        clients = rep.Find("Where Email = @0", vTerm)
    '        'If Not clients.Count > 0 Then
    '        '    clients = repository.Find("Where Telephone = @0", vTerm)
    '        '    If Not clients.Count > 0 Then
    '        '        clients = repository.Find("Where Cell = @0", vTerm)
    '        '    End If
    '        'End If
    '    End Using
    '    Return clients.Count
    'End Function

    'Public Shared Function ValidateNewUser(userName As String) As Integer
    '    Return Users.UserController.GetUsersByEmail(Null.NullInteger, userName.ToLower(), Null.NullInteger(), Null.NullInteger(), Null.NullInteger()).Count
    'End Function

    'Public Shared Function ValidateUser(userName As String) As Integer
    '    Return Users.UserController.GetUsersByUserName(Null.NullInteger, userName.ToLower(), Null.NullInteger(), Null.NullInteger(), Null.NullInteger()).Count
    'End Function

    Public Shared Function GeneratePassword(ByVal customerIDLength As Integer) As String
        ' add any more characters that you wish! 
        Const strChars As String = "1234567890"

        Dim r As New Random()
        Dim strNewCustomerID As String = ""
        For i As Integer = 0 To customerIDLength - 1

            Dim intRandom As Integer = r.[Next](0, strChars.Length)

            strNewCustomerID += strChars(intRandom)
        Next

        Return strNewCustomerID
    End Function

    Public Shared Function PhoneMask(phoneString As String) As String

        If phoneString IsNot Nothing Then
            If phoneString.Length > 0 Then
                Dim phoneSet1 = String.Empty
                Dim phoneSet2 = String.Empty
                Dim phoneSet3 = String.Empty

                If phoneString.StartsWith("08") Or phoneString.StartsWith("03") Then

                    phoneSet1 = Left(phoneString, 4)
                    phoneSet2 = Mid(phoneString, 5, 3)
                    phoneSet3 = Right(phoneString, 4)

                    PhoneMask = String.Format("({0}) {1}-{2}", phoneSet1, phoneSet2, phoneSet3)

                Else

                    phoneSet1 = Left(phoneString, 2)
                    phoneSet2 = Mid(phoneString, 3, 4)
                    phoneSet3 = Right(phoneString, 4)

                    phoneString = String.Format("({0}) {1}-{2}", phoneSet1, phoneSet2, phoneSet3)
                End If
            End If
        End If

        Return phoneString

    End Function

    Public Shared Function ZipMask(zipString As String) As String

        If zipString IsNot Nothing Then
            If zipString.Length > 0 Then
                Dim zipSet1 As String = Left(zipString, 2)
                Dim zipSet2 As String = Mid(zipString, 3, 3)
                Dim zipSet3 As String = Right(zipString, 3)

                zipString = String.Format("{0}.{1}-{2}", zipSet1, zipSet2, zipSet3)
            End If
        End If
        Return zipString

    End Function

    Public Shared Function TitleCase(ByVal value As String) As String
        Dim ret = String.Empty
        If value IsNot Nothing Then
            ret = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(value)
        End If
        Return ret
    End Function

    Public Shared Function UpperCase(ByVal value As String) As String
        Dim ret = String.Empty
        If value IsNot Nothing Then
            ret = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToUpper(value)
        End If
        Return ret
    End Function

    Public Shared Function LowerCase(ByVal value As String) As String
        Dim ret = String.Empty
        If value IsNot Nothing Then
            ret = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToLower(value)
        End If
        Return ret
    End Function

    Public Shared Function RemoveHtmlTags(text_containg_html As String) As String

        ' regular expressions, first remove tags, then remove any excess white space

        Return Regex.Replace(Regex.Replace(text_containg_html, "<(.|\n)*?>", [String].Empty), "\s+", " ")

    End Function

    Public Shared Function EncodeAccentletters(text_containing_letters As String) As String

        Dim txt = text_containing_letters
        txt = txt.Replace("À", "&Agrave;")
        txt = txt.Replace("Á", "&Aacute;")
        txt = txt.Replace("Â", "&Acirc;")
        txt = txt.Replace("Ã", "&Atilde;")
        txt = txt.Replace("Ç", "&Ccedil;")
        txt = txt.Replace("É", "&Eacute;")
        txt = txt.Replace("Ê", "&Ecirc;")
        txt = txt.Replace("Í", "&Iacute;")
        txt = txt.Replace("Ó", "&Oacute;")
        txt = txt.Replace("Ô", "&Ocirc;")
        txt = txt.Replace("Õ", "&Otilde;")
        txt = txt.Replace("Ü", "&Uuml;")
        txt = txt.Replace("Ú", "&Uacute;")
        txt = txt.Replace("à", "&agrave;")
        txt = txt.Replace("á", "&aacute;")
        txt = txt.Replace("ã", "&atilde;")
        txt = txt.Replace("â", "&acirc;")
        txt = txt.Replace("ç", "&ccedil;")
        txt = txt.Replace("é", "&eacute;")
        txt = txt.Replace("ê", "&ecirc;")
        txt = txt.Replace("í", "&iacute;")
        txt = txt.Replace("ó", "&oacute;")
        txt = txt.Replace("ô", "&ocirc;")
        txt = txt.Replace("õ", "&otilde;")
        txt = txt.Replace("ú", "&uacute;")
        txt = txt.Replace("º", "&ordm;")
        txt = txt.Replace("ª", "&ordf;")
        txt = txt.Replace("«", "&laquo;")
        txt = txt.Replace("»", "&raquo;")
        txt = txt.Replace("<", "&lsaquo;")
        txt = txt.Replace(">", "&rsaquo;")
        Return txt

    End Function

    Public Shared Function DecodeAccentletters(text_containing_letters As String) As String

        If text_containing_letters <> "" Then
            Dim txt = text_containing_letters
            txt = txt.Replace("&Agrave;", "À")
            txt = txt.Replace("&Aacute;", "Á")
            txt = txt.Replace("&Acirc;", "Â")
            txt = txt.Replace("&Atilde;", "Ã")
            txt = txt.Replace("&Ccedil;", "Ç")
            txt = txt.Replace("&Eacute;", "É")
            txt = txt.Replace("&Ecirc;", "Ê")
            txt = txt.Replace("&Iacute;", "Í")
            txt = txt.Replace("&Oacute;", "Ó")
            txt = txt.Replace("&Ocirc;", "Ô")
            txt = txt.Replace("&Otilde;", "Õ")
            txt = txt.Replace("&Uuml;", "Ü")
            txt = txt.Replace("&Uacute;", "Ú")
            txt = txt.Replace("&agrave;", "à")
            txt = txt.Replace("&aacute;", "á")
            txt = txt.Replace("&atilde;", "ã")
            txt = txt.Replace("&acirc;", "â")
            txt = txt.Replace("&ccedil;", "ç")
            txt = txt.Replace("&eacute;", "é")
            txt = txt.Replace("&ecirc;", "ê")
            txt = txt.Replace("&iacute;", "í")
            txt = txt.Replace("&oacute;", "ó")
            txt = txt.Replace("&ocirc;", "ô")
            txt = txt.Replace("&otilde;", "õ")
            txt = txt.Replace("&uacute;", "ú")
            txt = txt.Replace("&uuml;", "ü")
            txt = txt.Replace("&ordm;", "º")
            txt = txt.Replace("&ordf;", "ª")
            txt = txt.Replace("&laquo;", "«")
            txt = txt.Replace("&raquo;", "»")
            txt = txt.Replace("&lsaquo;", "<")
            txt = txt.Replace("&rsaquo;", ">")
            Return txt
        End If

        Return ""

    End Function

    Public Shared Function ReplaceAccentletters(text_containing_letters As String) As String

        Dim txt = text_containing_letters
        txt = txt.Replace("À", "A")
        txt = txt.Replace("Á", "A")
        txt = txt.Replace("Â", "A")
        txt = txt.Replace("Ã", "A")
        txt = txt.Replace("Ç", "C")
        txt = txt.Replace("É", "E")
        txt = txt.Replace("Ê", "E")
        txt = txt.Replace("Í", "I")
        txt = txt.Replace("Ó", "O")
        txt = txt.Replace("Ô", "O")
        txt = txt.Replace("Õ", "O")
        txt = txt.Replace("Ù", "U")
        txt = txt.Replace("Ü", "U")
        txt = txt.Replace("à", "a")
        txt = txt.Replace("á", "a")
        txt = txt.Replace("ã", "a")
        txt = txt.Replace("â", "a")
        txt = txt.Replace("ã", "a")
        txt = txt.Replace("ç", "c")
        txt = txt.Replace("é", "e")
        txt = txt.Replace("ê", "e")
        txt = txt.Replace("í", "i")
        txt = txt.Replace("ó", "o")
        txt = txt.Replace("ô", "o")
        txt = txt.Replace("õ", "o")
        txt = txt.Replace("ú", "u")
        txt = txt.Replace("ü", "u")
        Return txt

    End Function

    Public Shared Function GetPortalSettings(portalId As Integer) As DotNetNuke.Entities.Portals.PortalSettings
        'new settings object
        Dim ps As New PortalSettings()
        'controller instances
        Dim pc As New PortalController()
        Dim tc As New TabController()
        Dim pac As New PortalAliasController()

        'get the first portal alias found to be used as the current portal alias
        'Dim portalAlias As PortalAliasInfo = Nothing
        'Dim aliases As PortalAliasCollection = pac.GetPortalAliasByPortalID(portalId)
        'Dim aliasKey As String = ""
        'If aliases IsNot Nothing AndAlso aliases.Count > 0 Then
        '    'get the first portal alias in the list and use that
        '    For Each key As String In aliases.Keys
        '        aliasKey = key
        '        portalAlias = aliases(key)
        '        Exit For
        '    Next
        'End If
        'get the portal and copy across the settings
        Dim portal As PortalInfo = pc.GetPortal(portalId)
        If portal IsNot Nothing Then
            'ps.PortalAlias = portalAlias
            ps.PortalId = portal.PortalID
            ps.PortalName = portal.PortalName
            ps.LogoFile = portal.LogoFile
            ps.FooterText = portal.FooterText
            ps.ExpiryDate = portal.ExpiryDate
            ps.UserRegistration = portal.UserRegistration
            ps.BannerAdvertising = portal.BannerAdvertising
            ps.Currency = portal.Currency
            ps.AdministratorId = portal.AdministratorId
            ps.Email = portal.Email
            ps.HostFee = portal.HostFee
            ps.HostSpace = portal.HostSpace
            ps.PageQuota = portal.PageQuota
            ps.UserQuota = portal.UserQuota
            ps.AdministratorRoleId = portal.AdministratorRoleId
            ps.AdministratorRoleName = portal.AdministratorRoleName
            ps.RegisteredRoleId = portal.RegisteredRoleId
            ps.RegisteredRoleName = portal.RegisteredRoleName
            ps.Description = portal.Description
            ps.KeyWords = portal.KeyWords
            ps.BackgroundFile = portal.BackgroundFile
            ps.GUID = portal.GUID
            ps.SiteLogHistory = portal.SiteLogHistory
            ps.AdminTabId = portal.AdminTabId
            ps.SuperTabId = portal.SuperTabId
            ps.SplashTabId = portal.SplashTabId
            ps.HomeTabId = portal.HomeTabId
            ps.LoginTabId = portal.LoginTabId
            ps.UserTabId = portal.UserTabId
            ps.DefaultLanguage = portal.DefaultLanguage
            'ps.TimeZoneOffset = portal.TimeZoneOffset
            ps.HomeDirectory = portal.HomeDirectory
            'ps.Version = portal.Version
            'ps.AdminSkin = SkinController.GetSkin(SkinInfo.RootSkin, portalId, SkinType.Admin)
            'If ps.AdminSkin Is Nothing Then
            '    ps.AdminSkin = SkinController.GetSkin(SkinInfo.RootSkin, DotNetNuke.Common.Utilities.Null.NullInteger, SkinType.Admin)
            'End If
            'ps.PortalSkin = SkinController.GetSkin(SkinInfo.RootSkin, portalId, SkinType.Portal)
            'If ps.PortalSkin Is Nothing Then
            '    ps.PortalSkin = SkinController.GetSkin(SkinInfo.RootSkin, DotNetNuke.Common.Utilities.Null.NullInteger, SkinType.Portal)
            'End If
            'ps.AdminContainer = SkinController.GetSkin(SkinInfo.RootContainer, portalId, SkinType.Admin)
            'If ps.AdminContainer Is Nothing Then
            '    ps.AdminContainer = SkinController.GetSkin(SkinInfo.RootContainer, DotNetNuke.Common.Utilities.Null.NullInteger, SkinType.Admin)
            'End If
            'ps.PortalContainer = SkinController.GetSkin(SkinInfo.RootContainer, portalId, SkinType.Portal)
            'If ps.PortalContainer Is Nothing Then
            '    ps.PortalContainer = SkinController.GetSkin(SkinInfo.RootContainer, DotNetNuke.Common.Utilities.Null.NullInteger, SkinType.Portal)
            'End If
            ps.Pages = portal.Pages
            ps.Users = portal.Users
            ' set custom properties
            If DotNetNuke.Common.Utilities.Null.IsNull(ps.HostSpace) Then
                ps.HostSpace = 0
            End If
            If DotNetNuke.Common.Utilities.Null.IsNull(ps.DefaultLanguage) Then
                ps.DefaultLanguage = DotNetNuke.Services.Localization.Localization.SystemLocale
            End If
            'If DotNetNuke.Common.Utilities.Null.IsNull(ps.TimeZoneOffset) Then
            '    ps.TimeZoneOffset = DotNetNuke.Services.Localization.Localization.SystemTimeZoneOffset
            'End If
            ps.HomeDirectory = String.Format("{0}/{1}/", DotNetNuke.Common.Globals.ApplicationPath, portal.HomeDirectory)
            ' get application version
            'Dim arrVersion As String() = DotNetNuke.Common.Assembly.glbAppVersion.Split("."c)
            'Dim intMajor As Integer = 0
            'Dim intMinor As Integer = 0
            'Dim intBuild As Integer = 0
            'Int32.TryParse(arrVersion(0), intMajor)
            'Int32.TryParse(arrVersion(1), intMinor)
            'Int32.TryParse(arrVersion(2), intBuild)
            'ps.Version = intMajor.ToString() & "." & intMinor.ToString() & "." & intBuild.ToString()
        End If
        'Add each portal Tab to DekstopTabs
        'Dim portalTab As TabInfo = Nothing
        'ps.DesktopTabs = New ArrayList()
        'Dim first As Boolean = True
        'For Each tabPair As KeyValuePair(Of Integer, Tabs.TabInfo) In tc.GetTabsByPortal(ps.PortalId)
        '    ' clone the tab object ( to avoid creating an object reference to the data cache )
        '    portalTab = tabPair.Value.Clone()
        '    ' set custom properties
        '    If portalTab.TabOrder = 0 Then
        '        portalTab.TabOrder = 999
        '    End If
        '    If DotNetNuke.Common.Utilities.Null.IsNull(portalTab.StartDate) Then
        '        portalTab.StartDate = System.DateTime.MinValue
        '    End If
        '    If DotNetNuke.Common.Utilities.Null.IsNull(portalTab.EndDate) Then
        '        portalTab.EndDate = System.DateTime.MaxValue
        '    End If
        '    ps.DesktopTabs.Add(portalTab)

        '    'assign the first 'normal' tab as the active tab - could be the home tab, if it 
        '    'still exists, or it will be after the admin tab(s)
        '    If first AndAlso (portalTab.TabID = portal.HomeTabId OrElse portalTab.TabID > portal.AdminTabId) Then
        '        ps.ActiveTab = portalTab
        '        first = False
        '    End If
        'Next
        'last gasp chance in case active tab was not set
        'If ps.ActiveTab Is Nothing Then
        '    ps.ActiveTab = portalTab
        'End If
        'Add each host Tab to DesktopTabs
        'Dim hostTab As Tabs.TabInfo = Nothing
        'For Each tabPair As KeyValuePair(Of Integer, Tabs.TabInfo) In tc.GetTabsByPortal(DotNetNuke.Common.Utilities.Null.NullInteger)
        '    ' clone the tab object ( to avoid creating an object reference to the data cache )
        '    hostTab = tabPair.Value.Clone()
        '    hostTab.PortalID = ps.PortalId
        '    hostTab.StartDate = System.DateTime.MinValue
        '    hostTab.EndDate = System.DateTime.MaxValue
        '    ps.DesktopTabs.Add(hostTab)
        'Next

        'now add the portal settings to the httpContext
        'If System.Web.HttpContext.Current Is Nothing Then
        '    'if there is no HttpContext, then mock one up by creating a fake WorkerRequest
        '    Dim appVirtualDir As String = DotNetNuke.Common.Globals.ApplicationPath
        '    Dim appPhysicalDir As String = AppDomain.CurrentDomain.BaseDirectory
        '    Dim page As String = ps.PortalAlias.HTTPAlias
        '    Dim query As String = String.Empty
        '    Dim output As System.IO.TextWriter = Nothing
        '    'create a dummy simple worker request
        '    Dim workerRequest As New System.Web.Hosting.SimpleWorkerRequest(page, query, output)
        '    System.Web.HttpContext.Current = New System.Web.HttpContext(workerRequest)
        'End If
        'stash the portalSettings in the Context Items, where the rest of the DNN Code expects it to be
        'System.Web.HttpContext.Current.Items.Add("PortalSettings", ps)
        Return ps
    End Function

#End Region

End Class
