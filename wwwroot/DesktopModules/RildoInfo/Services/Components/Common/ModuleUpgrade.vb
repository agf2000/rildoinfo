
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Security.Membership

Public Class ModuleUpgrade
    Implements IUpgradeable

    Private ReadOnly ps As New Portals.PortalSettings(0)

    Private userPath As String = String.Empty

    Public Function UpgradeModule(Version As String) As String Implements IUpgradeable.UpgradeModule

        If String.Compare(Version, "00.00.01") = 0 Then

            ' Statuses
            AddStatuses()

            ' Industries
            AddIndustries()

            '' Unit Types
            'AddUnitTypes()

            ' Add Brazillian States
            AddBRStates()

            ' Add first Category
            'AddCategory()

            ' Add Product Templates
            'AddProductsTemplate()

            ' Add images for products
            'AddProductImages()

            ' Add template txt files
            'AddTemplates()

            ' Add New Roles
            CreateRoles()

            'Add New Accounts
            CreateAccounts()

            'Add Notification
            Notifications.AddNotificationTypes()

            ' Portal Settings
            AddInitialPortalData()

        End If

        Return ""

    End Function

    'Private Sub AddTemplates()

    '    ''Check if this is a User Folder
    '    'Dim tEmailDir = New IO.DirectoryInfo(objPortalInfo.HomeDirectoryMapPath & "Templates/Email")
    '    'If tEmailDir.Exists = False Then
    '    '    'Add User folder
    '    '    DotNetNuke.Services.FileSystem.FolderManager.Instance().AddFolder(0, "Templates/Email")

    '    '    DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(ps.PortalId)

    '    '    Dim tEmailFolder = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetFolder(ps.PortalId, "Templates/Email")
    '    '    Dim tEmailFiles = IO.Directory.GetFiles(DotNetNuke.Common.Globals.ApplicationMapPath() & "\DesktopModules\RIStore\Templates\Email")
    '    '    For Each _fileEmail In tEmailFiles
    '    '        IO.File.Copy(_fileEmail, String.Format("{0}{1}.txt", tEmailFolder.PhysicalPath, _fileEmail.Replace(DotNetNuke.Common.Globals.ApplicationMapPath() & "\DesktopModules\RIStore\Templates\Email\", "")))
    '    '    Next
    '    'End If

    '    ''Check if this is a User Folder
    '    'Dim tProductsDir = New IO.DirectoryInfo(objPortalInfo.HomeDirectoryMapPath & "Templates/Products")
    '    'If tProductsDir.Exists = False Then
    '    '    'Add User folder
    '    '    DotNetNuke.Services.FileSystem.FolderManager.Instance().AddFolder(ps.PortalId, "Templates/Products")

    '    '    DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(ps.PortalId)

    '    '    Dim tProductsFolder = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetFolder(ps.PortalId, "Templates/Products")
    '    '    Dim tProductsFiles = IO.Directory.GetFiles(DotNetNuke.Common.Globals.ApplicationMapPath() & "\DesktopModules\RIStore\Templates\Products")
    '    '    For Each _fileProduct In tProductsFiles
    '    '        IO.File.Copy(_fileProduct, String.Format("{0}{1}.txt", tProductsFolder.PhysicalPath, _fileProduct.Replace(DotNetNuke.Common.Globals.ApplicationMapPath() & "\DesktopModules\RIStore\Templates\Products\", "")))
    '    '    Next
    '    'End If

    '    Using objRIStoreModuleBase = New RIStoreModuleBase()
    '        objRIStoreModuleBase.CreateFile(TemplateController.GetTemplate(String.Format("{0}\Templates\Products\Products.Random.pt-BR.txt", ps.HomeDirectoryMapPath)))
    '        objRIStoreModuleBase.CreateFile(TemplateController.GetTemplate(String.Format("{0}\Templates\Products\Products.Random.txt", ps.HomeDirectoryMapPath)))
    '    End Using

    'End Sub

    'Private Sub AddProductImages()
    '    Dim productsCtrl As New ProductsRepository
    '    Dim productImages = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetFiles( 'productsCtrl.getProducts(ps.PortalId, Null.NullInteger, "pt-BR", "", False, False, "", "", "", 1, 10, "", False, False, "", "", False)

    '    For Each row In dsProducts
    '        If IO.File.Exists(String.Format("{0}Images/Products/{1}.jpg", ps.HomeDirectoryMapPath, CStr(row.ProductId))) Then
    '            Using originalImageStream = New IO.FileStream(String.Format("{0}Images/Products/{1}.jpg", ps.HomeDirectoryMapPath, CStr(row.ProductId)), IO.FileMode.Open)
    '                Dim imageToBeSaved As System.Drawing.Image = System.Drawing.Image.FromStream(originalImageStream)

    '                Dim objProdImage As New Models.Product With {.PortalId = ps.PortalId, .ProductId = row.ProductId, .CreatedByUser = 2, .CreatedOnDate = DateTime.Now}

    '                objProdImage.ProdImageBinary = RIStoreModuleBase.ConvertImageToByteArray(imageToBeSaved, System.Drawing.Imaging.ImageFormat.Jpeg)
    '                objProdImage.ContentLength = CInt(originalImageStream.Length)
    '                objProdImage.FileName = String.Format("{0}.jpg", CStr(row.ProdID))
    '                objProdImage.Extension = "jpg"

    '                Feature_Controller.Add_ProductImage(objProdImage)

    '            End Using
    '        End If
    '    Next

    'End Sub

    'Private Sub AddCategory()

    '    Dim catInfo As New Models.Category
    '    With catInfo
    '        .PortalID = ps.PortalId
    '        .ModuleId = RIStoreModuleBase.GetModInfo("Categories Manager", ps.PortalId).ModuleID
    '        .CatCode = "1"
    '        .CatOrder = 1
    '        .ParentId = 0
    '        .Name = "Geral"
    '        .ImageBinary = Nothing
    '        .Description = String.Empty
    '        .CreatedByUser = 2
    '        .CreatedDate = DateTime.Now()
    '    End With
    '    Feature_Controller.Add_Category(catInfo)

    'End Sub

    Private Sub UpdateCountryNames()

        Dim myListCtrl As New DotNetNuke.Common.Lists.ListController()
        Dim countries = myListCtrl.GetListEntryInfoItems("Country")

        Dim pt_BR_Countries As New NameValueCollection
        Dim countryName As String
        Dim countryValues() As String

        pt_BR_Countries.Add("Afeganistão", "AF")
        pt_BR_Countries.Add("África do Sul", "ZA")
        pt_BR_Countries.Add("Albânia", "AL")
        pt_BR_Countries.Add("Alemanha", "DE")
        pt_BR_Countries.Add("Andorra", "AD")
        pt_BR_Countries.Add("Angola", "AO")
        pt_BR_Countries.Add("Anguilla", "AI")
        pt_BR_Countries.Add("Antártica", "AQ")
        pt_BR_Countries.Add("Antígua e Barbuda", "AG")
        pt_BR_Countries.Add("Antilhas Holandesas", "AN")
        pt_BR_Countries.Add("Arábia Saudita", "SA")
        pt_BR_Countries.Add("Argélia", "DZ")
        pt_BR_Countries.Add("Argentina", "AR")
        pt_BR_Countries.Add("Arménia", "AM")
        pt_BR_Countries.Add("Aruba", "AW")
        pt_BR_Countries.Add("Austrália", "AU")
        pt_BR_Countries.Add("Áustria", "AT")
        pt_BR_Countries.Add("Azerbaijão", "AZ")
        pt_BR_Countries.Add("Bahamas", "BS")
        pt_BR_Countries.Add("Bahrein", "BH")
        pt_BR_Countries.Add("Bangladesh", "BD")
        pt_BR_Countries.Add("Barbados", "BB")
        pt_BR_Countries.Add("Bélgica", "BE")
        pt_BR_Countries.Add("Belize", "BZ")
        pt_BR_Countries.Add("Benin", "BJ")
        pt_BR_Countries.Add("Bermudas", "BM")
        pt_BR_Countries.Add("Bielorrússia", "BY")
        pt_BR_Countries.Add("Bolívia", "BO")
        pt_BR_Countries.Add("Bósnia e Herzegovina", "BA")
        pt_BR_Countries.Add("Botswana", "BW")
        pt_BR_Countries.Add("Brasil", "BR")
        pt_BR_Countries.Add("Brunei Darussalam", "BN")
        pt_BR_Countries.Add("Bulgária", "BG")
        pt_BR_Countries.Add("Burkina Faso", "BF")
        pt_BR_Countries.Add("Burundi", "BI")
        pt_BR_Countries.Add("Butão", "BT")
        pt_BR_Countries.Add("Cabo Verde", "CV")
        pt_BR_Countries.Add("Camarões", "CM")
        pt_BR_Countries.Add("Camboja", "KH")
        pt_BR_Countries.Add("Canadá", "CA")
        pt_BR_Countries.Add("Cazaquistão", "KZ")
        pt_BR_Countries.Add("Chade", "TD")
        pt_BR_Countries.Add("Chile", "CL")
        pt_BR_Countries.Add("China", "CN")
        pt_BR_Countries.Add("Chipre", "CY")
        pt_BR_Countries.Add("Cocos", "CC")
        pt_BR_Countries.Add("Colômbia", "CO")
        pt_BR_Countries.Add("Comores", "KM")
        pt_BR_Countries.Add("Congo", "CG")
        pt_BR_Countries.Add("Coréia do Norte", "KP")
        pt_BR_Countries.Add("Coréia do Sul", "KR")
        pt_BR_Countries.Add("Costa do Marfim", "CI")
        pt_BR_Countries.Add("Costa Rica", "CR")
        pt_BR_Countries.Add("Croácia", "HR")
        pt_BR_Countries.Add("Cuba", "CU")
        pt_BR_Countries.Add("Dinamarca", "DK")
        pt_BR_Countries.Add("Djibouti", "DJ")
        pt_BR_Countries.Add("Dominica", "DM")
        pt_BR_Countries.Add("Egito", "EG")
        pt_BR_Countries.Add("El Salvador", "SV")
        pt_BR_Countries.Add("Emirados Árabes Unidos", "AE")
        pt_BR_Countries.Add("Equador", "EC")
        pt_BR_Countries.Add("Eritreia", "ER")
        pt_BR_Countries.Add("Eslováquia", "SK")
        pt_BR_Countries.Add("Eslovénia", "SI")
        pt_BR_Countries.Add("Espanha", "ES")
        pt_BR_Countries.Add("Estados Unidos", "US")
        pt_BR_Countries.Add("Estónia", "EE")
        pt_BR_Countries.Add("Etiópia", "ET")
        pt_BR_Countries.Add("EUA Ilhas Menores Distantes", "UM")
        pt_BR_Countries.Add("Federa&ccedil;ão Russa", "RU")
        pt_BR_Countries.Add("Fiji", "FJ")
        pt_BR_Countries.Add("Filipinas", "PH")
        pt_BR_Countries.Add("Finlândia", "FI")
        pt_BR_Countries.Add("Fran&ccedil;a", "FR")
        pt_BR_Countries.Add("Gabão", "GA")
        pt_BR_Countries.Add("Gâmbia", "GM")
        pt_BR_Countries.Add("Gana", "GH")
        pt_BR_Countries.Add("Geórgia", "GE")
        pt_BR_Countries.Add("Gibraltar", "GI")
        pt_BR_Countries.Add("Grécia", "GR")
        pt_BR_Countries.Add("Grenada", "GD")
        pt_BR_Countries.Add("Gronelândia", "GL")
        pt_BR_Countries.Add("Guadalupe", "GP")
        pt_BR_Countries.Add("Guam", "GU")
        pt_BR_Countries.Add("Guatemala", "GT")
        pt_BR_Countries.Add("Guiana", "GY")
        pt_BR_Countries.Add("Guiana Francesa", "GF")
        pt_BR_Countries.Add("Guiné Equatorial", "GQ")
        pt_BR_Countries.Add("Guinea", "GN")
        pt_BR_Countries.Add("Guiné-Bissau", "GW")
        pt_BR_Countries.Add("Haiti", "HT")
        pt_BR_Countries.Add("Holanda", "NL")
        pt_BR_Countries.Add("Honduras", "HN")
        pt_BR_Countries.Add("Hong Kong", "HK")
        pt_BR_Countries.Add("Hungria", "HU")
        pt_BR_Countries.Add("Iémen", "YE")
        pt_BR_Countries.Add("Ilha Bouvet", "BV")
        pt_BR_Countries.Add("Ilha de Norfolk", "NF")
        pt_BR_Countries.Add("Ilhas Caymans", "KY")
        pt_BR_Countries.Add("Ilhas Christmas", "CX")
        pt_BR_Countries.Add("Ilhas Cook", "CK")
        pt_BR_Countries.Add("Ilhas Feroé", "FO")
        pt_BR_Countries.Add("Ilhas Heard e McDonald", "HM")
        pt_BR_Countries.Add("Ilhas Malvinas", "FK")
        pt_BR_Countries.Add("Ilhas Marianas do Norte", "MP")
        pt_BR_Countries.Add("Ilhas Marshall", "MH")
        pt_BR_Countries.Add("Ilhas Pitcairn", "PN")
        pt_BR_Countries.Add("Ilhas Salomão", "SB")
        pt_BR_Countries.Add("Ilhas Svalbard e Jan Mayen", "SJ")
        pt_BR_Countries.Add("Ilhas Virgens Britânicas", "VG")
        pt_BR_Countries.Add("Ilhas Virgens dos EUA", "VI")
        pt_BR_Countries.Add("Ilhas Wallis e Futuna", "WF")
        pt_BR_Countries.Add("Índia", "IN")
        pt_BR_Countries.Add("Indonésia", "ID")
        pt_BR_Countries.Add("Irão", "IR")
        pt_BR_Countries.Add("Iraque", "IQ")
        pt_BR_Countries.Add("Irlanda", "IE")
        pt_BR_Countries.Add("Islândia", "IS")
        pt_BR_Countries.Add("Israel", "IL")
        pt_BR_Countries.Add("Itália", "IT")
        pt_BR_Countries.Add("Iugoslávia", "YU")
        pt_BR_Countries.Add("Jamaica", "JM")
        pt_BR_Countries.Add("Japão", "JP")
        pt_BR_Countries.Add("Jordânia", "JO")
        pt_BR_Countries.Add("Kiribati", "KI")
        pt_BR_Countries.Add("Kuwait", "KW")
        pt_BR_Countries.Add("Laos", "LA")
        pt_BR_Countries.Add("Lesoto", "LS")
        pt_BR_Countries.Add("Letónia", "LV")
        pt_BR_Countries.Add("Líbano", "LB")
        pt_BR_Countries.Add("Libéria", "LR")
        pt_BR_Countries.Add("Líbia", "LY")
        pt_BR_Countries.Add("Liechtenstein", "LI")
        pt_BR_Countries.Add("Lituânia", "LT")
        pt_BR_Countries.Add("Luxemburgo", "LU")
        pt_BR_Countries.Add("Macau", "MO")
        pt_BR_Countries.Add("Macedónia", "MK")
        pt_BR_Countries.Add("Madagascar", "MG")
        pt_BR_Countries.Add("Malásia", "MY")
        pt_BR_Countries.Add("Malawi", "MW")
        pt_BR_Countries.Add("Maldivas", "MV")
        pt_BR_Countries.Add("Mali", "ML")
        pt_BR_Countries.Add("Malta", "MT")
        pt_BR_Countries.Add("Marrocos", "MA")
        pt_BR_Countries.Add("Martinica", "MQ")
        pt_BR_Countries.Add("Maurícias", "MU")
        pt_BR_Countries.Add("Mauritania", "MR")
        pt_BR_Countries.Add("Mayotte", "YT")
        pt_BR_Countries.Add("México", "MX")
        pt_BR_Countries.Add("Mianmar", "MM")
        pt_BR_Countries.Add("Micronésia", "FM")
        pt_BR_Countries.Add("Mo&ccedil;ambique", "MZ")
        pt_BR_Countries.Add("Moldávia", "MD")
        pt_BR_Countries.Add("Mónaco", "MC")
        pt_BR_Countries.Add("Mongólia", "MN")
        pt_BR_Countries.Add("Montserrat", "MS")
        pt_BR_Countries.Add("Namíbia", "NA")
        pt_BR_Countries.Add("Nauru", "NR")
        pt_BR_Countries.Add("Nepal", "NP")
        pt_BR_Countries.Add("Nicarágua", "NI")
        pt_BR_Countries.Add("Níger", "NE")
        pt_BR_Countries.Add("Nigéria", "NG")
        pt_BR_Countries.Add("Niue", "NU")
        pt_BR_Countries.Add("Noruega", "NO")
        pt_BR_Countries.Add("Nova Caledônia", "NC")
        pt_BR_Countries.Add("Nova Zelândia", "NZ")
        pt_BR_Countries.Add("Omã", "OM")
        pt_BR_Countries.Add("Palau", "PW")
        pt_BR_Countries.Add("Panamá", "PA")
        pt_BR_Countries.Add("Papua-Nova Guiné", "PG")
        pt_BR_Countries.Add("Paquistão", "PK")
        pt_BR_Countries.Add("Paraguai", "PY")
        pt_BR_Countries.Add("Peru", "PE")
        pt_BR_Countries.Add("Polinésia Francesa", "PF")
        pt_BR_Countries.Add("Polónia", "PL")
        pt_BR_Countries.Add("Porto Rico", "PR")
        pt_BR_Countries.Add("Portugal", "PT")
        pt_BR_Countries.Add("Qatar", "QA")
        pt_BR_Countries.Add("Quénia", "KE")
        pt_BR_Countries.Add("Quirguistão", "KG")
        pt_BR_Countries.Add("Reino Unido", "GB")
        pt_BR_Countries.Add("República Centro-Africana", "CF")
        pt_BR_Countries.Add("República Checa", "CZ")
        pt_BR_Countries.Add("República Dominicana", "DO")
        pt_BR_Countries.Add("Reunião", "RE")
        pt_BR_Countries.Add("Roménia", "RO")
        pt_BR_Countries.Add("Ruanda", "RW")
        pt_BR_Countries.Add("S. Georgia and S. Sandwich Islands", "GS")
        pt_BR_Countries.Add("Sahara Ocidental", "EH")
        pt_BR_Countries.Add("Saint-Pierre and Miquelon", "PM")
        pt_BR_Countries.Add("Samoa", "WS")
        pt_BR_Countries.Add("Samoa Americana", "AS")
        pt_BR_Countries.Add("San Marino", "SM")
        pt_BR_Countries.Add("Santa Helena", "SH")
        pt_BR_Countries.Add("Santa Lúcia", "LC")
        pt_BR_Countries.Add("São Cristóvão e Nevis", "KN")
        pt_BR_Countries.Add("São Tomé e Príncipe", "ST")
        pt_BR_Countries.Add("São Vicente e Granadinas", "VC")
        pt_BR_Countries.Add("Senegal", "SN")
        pt_BR_Countries.Add("Serra Leoa", "SL")
        pt_BR_Countries.Add("Seychelles", "SC")
        pt_BR_Countries.Add("Singapura", "SG")
        pt_BR_Countries.Add("Síria", "SY")
        pt_BR_Countries.Add("Somália", "SO")
        pt_BR_Countries.Add("Sri Lanka", "LK")
        pt_BR_Countries.Add("Suazilândia", "SZ")
        pt_BR_Countries.Add("Sudão", "SD")
        pt_BR_Countries.Add("Suécia", "SE")
        pt_BR_Countries.Add("Suí&ccedil;a", "CH")
        pt_BR_Countries.Add("Suriname", "SR")
        pt_BR_Countries.Add("Tailândia", "TH")
        pt_BR_Countries.Add("Taiwan", "TW")
        pt_BR_Countries.Add("Tajiquistão", "TJ")
        pt_BR_Countries.Add("Tanzânia", "TZ")
        pt_BR_Countries.Add("Território Britânico do Oceano Índico", "IO")
        pt_BR_Countries.Add("Territórios Franceses do Sul", "TF")
        pt_BR_Countries.Add("Timor Leste", "TP")
        pt_BR_Countries.Add("Togo", "TG")
        pt_BR_Countries.Add("Tokelau", "TK")
        pt_BR_Countries.Add("Tonga", "TO")
        pt_BR_Countries.Add("Trinidad e Tobago", "TT")
        pt_BR_Countries.Add("Tunísia", "TN")
        pt_BR_Countries.Add("Turks and Caicos Islands", "TC")
        pt_BR_Countries.Add("Turquemenistão", "TM")
        pt_BR_Countries.Add("Turquía", "TR")
        pt_BR_Countries.Add("Tuvalu", "TV")
        pt_BR_Countries.Add("Ucrânia", "UA")
        pt_BR_Countries.Add("Uganda", "UG")
        pt_BR_Countries.Add("União Soviética", "SU")
        pt_BR_Countries.Add("Uruguai", "UY")
        pt_BR_Countries.Add("Uzbequistão", "UZ")
        pt_BR_Countries.Add("Vanuatu", "VU")
        pt_BR_Countries.Add("Venezuela", "VE")
        pt_BR_Countries.Add("Vietnã", "VN")
        pt_BR_Countries.Add("Zaire", "ZR")
        pt_BR_Countries.Add("Zâmbia", "ZM")
        pt_BR_Countries.Add("Zimbabué", "ZW")

        For Each countryName In pt_BR_Countries
            countryValues = pt_BR_Countries.GetValues(countryName)
            For Each value As String In countryValues
                For Each country As DotNetNuke.Common.Lists.ListEntryInfo In countries
                    If country.Value = value Then
                        Dim newCountry As New DotNetNuke.Common.Lists.ListEntryInfo()
                        With newCountry
                            .DefinitionID = Null.NullInteger
                            .PortalID = country.PortalID
                            .ListName = country.ListName
                            .Value = value
                            .Text = countryName
                            .ParentKey = country.ParentKey
                            .EntryID = country.EntryID
                        End With
                        myListCtrl.UpdateListEntry(newCountry)
                    End If
                Next
            Next
        Next

    End Sub

    Private Sub AddBRStates()

        Dim myListCtrl As New DotNetNuke.Common.Lists.ListController()
        Dim regions = myListCtrl.GetListInfoCollection("Region", "Country.BR")
        If Not regions.Count > 0 Then

            Dim statesBR As New NameValueCollection
            Dim stateName As String
            Dim stateValues() As String
            statesBR.Add("Acre", "AC")
            statesBR.Add("Alagoas", "AL")
            statesBR.Add("Amapá", "AP")
            statesBR.Add("Amazonas", "AM")
            statesBR.Add("Bahia", "BA")
            statesBR.Add("Ceará", "CE")
            statesBR.Add("Distrito Federal", "DF")
            statesBR.Add("Espírito Santo", "ES")
            statesBR.Add("Goiás", "GO")
            statesBR.Add("Maranhão", "MA")
            statesBR.Add("Mato Grosso", "MT")
            statesBR.Add("Mato Grosso do Sul", "MS")
            statesBR.Add("Minas Gerais", "MG")
            statesBR.Add("Pará", "PA")
            statesBR.Add("Paraíba", "PB")
            statesBR.Add("Paraná", "PR")
            statesBR.Add("Pernambuco", "PE")
            statesBR.Add("Piauí", "PI")
            statesBR.Add("Rio de Janeiro", "RJ")
            statesBR.Add("Rio Grande do Norte", "RN")
            statesBR.Add("Rio Grande do Sul", "RS")
            statesBR.Add("Rondônia", "RO")
            statesBR.Add("Roraima", "RR")
            statesBR.Add("Santa Catarina", "SC")
            statesBR.Add("São Paulo", "SP")
            statesBR.Add("Sergipe", "SE")
            statesBR.Add("Tocantins", "TO")

            Dim parentEntry As DotNetNuke.Common.Lists.ListEntryInfo = myListCtrl.GetListEntryInfo(29)
            For Each stateName In statesBR
                stateValues = statesBR.GetValues(stateName)
                For Each value As String In stateValues
                    Dim myListInfo As New DotNetNuke.Common.Lists.ListEntryInfo()
                    With myListInfo
                        .DefinitionID = Null.NullInteger
                        .PortalID = Null.NullInteger
                        .ListName = "Region"
                        .Value = value
                        .Text = stateName
                        .ParentKey = parentEntry.Key
                        .ParentID = 29
                        .Level = parentEntry.Level + 1
                    End With
                    myListCtrl.AddListEntry(myListInfo)
                Next
            Next

            ' Translate Countries to Portuguese
            UpdateCountryNames()
        End If

    End Sub

    Private Sub AddInitialPortalData()

        Dim newSettings As New NameValueCollection
        Dim SettingName As String
        Dim SettingValues() As String
        newSettings.Add("RIW_Address_Required", "False")
        newSettings.Add("RIW_Address_Show", "True")
        newSettings.Add("RIW_AgendaMessage", "<p>Caro(a) [CLIENTE], saudações.<br /><br />Confirmo através deste e-mail agenda marcada para: [DATA]</p>[SOBRE]")
        newSettings.Add("RIW_AllowPurchase", "False")
        newSettings.Add("RIW_BW_Logo", "")
        newSettings.Add("RIW_CCBlockList", "")
        newSettings.Add("RIW_CCTestMode", "")
        newSettings.Add("RIW_CityTax_Required", "False")
        newSettings.Add("RIW_CityTax_Show", "False")
        newSettings.Add("RIW_CNPJ_Required", "False")
        newSettings.Add("RIW_CNPJ_Show", "False")
        newSettings.Add("RIW_CompanyName_Required", "False")
        newSettings.Add("RIW_CompanyName_Show", "True")
        newSettings.Add("RIW_CPF_Required", "False")
        newSettings.Add("RIW_CPF_Show", "False")
        'newSettings.Add("RIW_Custom_Settings", "")
        newSettings.Add("RIW_DisplayName_Required", "False")
        newSettings.Add("RIW_DisplayName_Show", "False")
        newSettings.Add("RIW_EstimateEmailBody", "<p>Caro(a) [CLIENTE].<br /><br />Acesse seu Orçamento com ([ID]) usando o link apresentado neste email.<br /><br />Clique <a href='[LINK]'>aqui</a> para abrir o orçamento.</p>")
        newSettings.Add("RIW_EstimateEmailSubject", "Seu Orçamento com ([ID]) no website [WEBSITE]")
        newSettings.Add("RIW_EstimateMaxDiscount", "10")
        newSettings.Add("RIW_EstimateMaxDuration", "3")
        newSettings.Add("RIW_EstimateTerm", "<p>Validade deste oçamento é de 3 dias. <br> Todas as imagens são meramente ilustrativas. <br> Valores podem ser alterados sem prévio aviso. <br> Prazo de entrega(s) do(s) produtos e execução de serviços a combinar.</p>")
        newSettings.Add("RIW_Ident_Required", "False")
        newSettings.Add("RIW_Ident_Show", "False")
        newSettings.Add("RIW_Industry_Required", "False")
        newSettings.Add("RIW_Industry_Show", "False")
        newSettings.Add("RIW_InventoryCheck", "False")
        newSettings.Add("RIW_LastName_Required", "False")
        newSettings.Add("RIW_LastName_Show", "True")
        newSettings.Add("RIW_PageSize", "5")
        newSettings.Add("RIW_ProductTemplate", "")
        newSettings.Add("RIW_ProductWatermark", "")
        newSettings.Add("RIW_SalesPerson", Users.UserController.GetUserByName("vendedor").UserID)
        newSettings.Add("RIW_SaveCCData", "False")
        newSettings.Add("RIW_ShowEstimatePrice", "True")
        newSettings.Add("RIW_ShowNoStockProduct", "False")
        newSettings.Add("RIW_ShowProductPrice", "True")
        newSettings.Add("RIW_SMTPConnection", "True")
        newSettings.Add("RIW_SMTPLogin", "contato@riw.com.br")
        newSettings.Add("RIW_SMTPPassword", "Senha123.")
        newSettings.Add("RIW_SMTPPort", "587")
        newSettings.Add("RIW_SMTPServer", "smtp.gmail.com")
        newSettings.Add("RIW_StateTax_Required", "False")
        newSettings.Add("RIW_StateTax_Show", "False")
        newSettings.Add("RIW_StoreAddress", "Rua Barão de Aiuruoca")
        newSettings.Add("RIW_StoreCity", "Belo Horizonte")
        newSettings.Add("RIW_StoreComplement", "Loja A")
        newSettings.Add("RIW_StoreCountry", "Brasil")
        newSettings.Add("RIW_StoreDistrict", "João Pinheiro")
        newSettings.Add("RIW_StoreEmail", "contato@riw.com.br")
        newSettings.Add("RIW_StorePhones", "voz=3136538199")
        newSettings.Add("RIW_StorePostalCode", "3053009-1")
        newSettings.Add("RIW_StoreRegion", "Minas Gerais")
        newSettings.Add("RIW_StoreRepplyEmail", "")
        newSettings.Add("RIW_StoreUnit", "298")
        newSettings.Add("RIW_Telephone_Required", "False")
        newSettings.Add("RIW_Telephone_Show", "True")
        newSettings.Add("RIW_VisitFee", "")
        newSettings.Add("RIW_Website_Required", "False")
        newSettings.Add("RIW_Website_Show", "False")

        For Each SettingName In newSettings
            SettingValues = newSettings.GetValues(SettingName)
            For Each value As String In SettingValues
                'Feature_Controller.Add_PortalSetting(PortalId, SettingName, value)
                Portals.PortalController.UpdatePortalSetting(ps.PortalId, SettingName, value)
            Next
        Next

        'Dim portalCtrl = New Portals.PortalController()
        'Dim objPortalInfo = portalCtrl.GetPortal(ps.PortalId)

        'Dim _tabsCtrl = New Tabs.TabController()
        'objPortalInfo.RegisterTabId = _tabsCtrl.GetTabByName("Cadastro", ps.PortalId).TabID
        'objPortalInfo.UserTabId = _tabsCtrl.GetTabByName("Perfil", ps.PortalId).TabID
        'portalCtrl.UpdatePortalInfo(objPortalInfo)

    End Sub

    Private Sub AddIndustries()

        Dim Industries As New List(Of String)()
        Industries.Add("Agencia Publicidade")
        Industries.Add("Atacadista")
        Industries.Add("Casa de Ração")
        Industries.Add("Celulares")
        Industries.Add("Clube de Laser")
        Industries.Add("Construtora")
        Industries.Add("Distribuidora")
        Industries.Add("Escola")
        Industries.Add("Farmácia")
        Industries.Add("Indústria")
        Industries.Add("Informática")
        Industries.Add("Livraria")
        Industries.Add("Logística")
        Industries.Add("Loja de Equipamentos")
        Industries.Add("Loja Variedades")
        Industries.Add("Materiais Religiosos")
        Industries.Add("Materias de Construção")
        Industries.Add("Mineradora")
        Industries.Add("Móveis")
        Industries.Add("Outros")
        Industries.Add("Padaria")
        Industries.Add("Papelaria")
        Industries.Add("Prestador de serviço")
        Industries.Add("Publicidade")
        Industries.Add("Revenda")
        Industries.Add("Siderúrgica")
        Industries.Add("Som e Acessorios")
        Industries.Add("Supermercado")
        Industries.Add("Transportes")
        Industries.Add("Transportes Coletivos")
        Industries.Add("Varejista")

        For Each IndustryTitle As String In Industries
            Dim IndustryCtrl As New IndustriesRepository
            Dim IndustriesInfo As New Models.Industry
            With IndustriesInfo
                .PortalId = ps.PortalId
                .IndustryTitle = IndustryTitle
                .CreatedByUser = 2
                .CreatedOnDate = DateTime.Now()
                .ModifiedByUser = 2
                .ModifiedOnDate = DateTime.Now()
            End With
            IndustryCtrl.AddIndustry(IndustriesInfo)
        Next

    End Sub

    Private Sub AddStatuses()

        Dim newStatuses As New NameValueCollection
        Dim StatusTitle As String
        Dim StatusColors() As String
        newStatuses.Add("Normal", "#FFFFFF")
        newStatuses.Add("Em Progresso", "#92D050")
        newStatuses.Add("Aguardando", "#FF6347")
        newStatuses.Add("Inativo", "#FFDEAD")
        newStatuses.Add("Fechado", "#FF6347")
        newStatuses.Add("Pendente", "#AFEEEE")
        newStatuses.Add("Agendado", "#FFFF00")
        newStatuses.Add("Prioridade", "#FFC000")
        newStatuses.Add("Cancelado", "#FFEFD5")
        newStatuses.Add("Venda", "#00F7FF")

        For Each StatusTitle In newStatuses.Keys
            StatusColors = newStatuses.GetValues(StatusTitle)
            For Each value As String In StatusColors
                Dim StatusesCtrl As New StatusesRepository
                Dim newStatus As New Models.Status
                With newStatus
                    .CreatedByUser = 2
                    .CreatedOnDate = DateTime.Now()
                    .IsDeleted = False
                    .IsReadOnly = True
                    .ModifiedByUser = 2
                    .ModifiedOnDate = DateTime.Now()
                    .PortalId = ps.PortalId
                    .StatusColor = value
                    .StatusTitle = StatusTitle
                End With
                StatusesCtrl.addStatus(newStatus)
            Next
        Next

        DotNetNuke.Data.DataProvider.Instance().ExecuteSQL("UPDATE dbo.[RIS_Statuses] SET IsReadOnly = 'True' WHERE StatusTitle = 'Normal' GO")

    End Sub

    'Private Sub AddUnitTypes()

    '    Dim newUnitTypes As New List(Of String)()
    '    newUnitTypes.Add("UN")
    '    newUnitTypes.Add("M")
    '    newUnitTypes.Add("M²")
    '    newUnitTypes.Add("M³")
    '    newUnitTypes.Add("CM")
    '    newUnitTypes.Add("CM³")
    '    newUnitTypes.Add("MM")
    '    newUnitTypes.Add("PÉ")
    '    newUnitTypes.Add("POL")
    '    newUnitTypes.Add("JD")
    '    newUnitTypes.Add("Kg")
    '    newUnitTypes.Add("G")
    '    newUnitTypes.Add("LB")
    '    newUnitTypes.Add("L")
    '    newUnitTypes.Add("GL")
    '    newUnitTypes.Add("1/4")
    '    newUnitTypes.Add("L")

    '    For Each UnitTypeTitle As String In newUnitTypes
    '        Dim unitTypeInfo As New UnitType_Info()
    '        With unitTypeInfo
    '            .PortalID = ps.PortalId
    '            .UnitTypeTitle = UnitTypeTitle
    '            .CreatedByUser = 2
    '            .CreatedDate = DateTime.Now()
    '        End With
    '        Feature_Controller.Add_UnitType(unitTypeInfo)
    '    Next

    'End Sub

    Private Sub CreateRoles()

        'set roles here
        Dim objRoleCtrl As New RoleController

        Dim objRoleEntitiesGroupInfo = RoleController.GetRoleGroupByName(ps.PortalId, "Entidades")
        Dim intRoleEntitiesGroupID = Null.NullInteger
        If Null.IsNull(objRoleEntitiesGroupInfo) Then
            Dim objNewRoleGroupInfo As New RoleGroupInfo() With {.PortalID = ps.PortalId, .RoleGroupName = "Entidades", .Description = "Grupo de Entidades, como clientes, revendedores, transportadoras, fornecedores, etc."}
            intRoleEntitiesGroupID = RoleController.AddRoleGroup(objNewRoleGroupInfo)
        Else
            intRoleEntitiesGroupID = objRoleEntitiesGroupInfo.RoleGroupID
        End If

        Dim objClientsRoleInfo = objRoleCtrl.GetRoleByName(ps.PortalId, "Clientes")
        If Null.IsNull(objClientsRoleInfo) Then
            Dim objNewRole As New RoleInfo() With {.PortalID = ps.PortalId, .RoleGroupID = intRoleEntitiesGroupID, .RoleName = "Clientes", .Description = "Grupo de Clientes.", .IsPublic = False, .Status = RoleStatus.Approved}
            objRoleCtrl.AddRole(objNewRole)
        End If

        Dim objDealersRoleInfo = objRoleCtrl.GetRoleByName(ps.PortalId, "Revendedores")
        If Null.IsNull(objDealersRoleInfo) Then
            Dim objNewRole As New RoleInfo() With {.PortalID = ps.PortalId, .RoleGroupID = intRoleEntitiesGroupID, .RoleName = "Revendedores", .Description = "Grupo de Revendedores.", .IsPublic = False, .Status = RoleStatus.Approved}
            objRoleCtrl.AddRole(objNewRole)
        End If

        Dim objCarriersRoleInfo = objRoleCtrl.GetRoleByName(ps.PortalId, "Transportadoras")
        If Null.IsNull(objCarriersRoleInfo) Then
            Dim objNewRole As New RoleInfo() With {.PortalID = ps.PortalId, .RoleGroupID = intRoleEntitiesGroupID, .RoleName = "Transportadoras", .Description = "Grupo de Transportadoras.", .IsPublic = False, .Status = RoleStatus.Approved}
            objRoleCtrl.AddRole(objNewRole)
        End If

        Dim objVendorsRoleInfo = objRoleCtrl.GetRoleByName(ps.PortalId, "Fornecedores")
        If Null.IsNull(objVendorsRoleInfo) Then
            Dim objNewRole As New RoleInfo() With {.PortalID = ps.PortalId, .RoleGroupID = intRoleEntitiesGroupID, .RoleName = "Fornecedores", .Description = "Grupo de Fornecedores.", .IsPublic = False, .Status = RoleStatus.Approved}
            objRoleCtrl.AddRole(objNewRole)
        End If

        Dim objRoleDepartmentsGroupInfo = RoleController.GetRoleGroupByName(ps.PortalId, "Departamentos")
        Dim intRoleGroupID = Null.NullInteger
        If Null.IsNull(objRoleDepartmentsGroupInfo) Then
            Dim objNewRoleGroupInfo As New RoleGroupInfo() With {.PortalID = ps.PortalId, .RoleGroupName = "Departamentos", .Description = "Grupo de Departamentos."}
            intRoleGroupID = RoleController.AddRoleGroup(objNewRoleGroupInfo)
        Else
            intRoleGroupID = objRoleDepartmentsGroupInfo.RoleGroupID
        End If

        Dim objEditoresRoleInfo = objRoleCtrl.GetRoleByName(ps.PortalId, "Editores")
        If Null.IsNull(objEditoresRoleInfo) Then
            Dim objNewRole As New RoleInfo() With {.PortalID = ps.PortalId, .RoleGroupID = intRoleGroupID, .RoleName = "Editores", .Description = "Grupo de Editores.", .IsPublic = False, .Status = RoleStatus.Approved}
            objRoleCtrl.AddRole(objNewRole)
        End If

        Dim objManagersRoleInfo = objRoleCtrl.GetRoleByName(ps.PortalId, "Gerentes")
        If Null.IsNull(objManagersRoleInfo) Then
            Dim objNewRole As New RoleInfo() With {.PortalID = ps.PortalId, .RoleGroupID = intRoleGroupID, .RoleName = "Gerentes", .Description = "Grupo de Gerentes.", .IsPublic = False, .Status = RoleStatus.Approved}
            objRoleCtrl.AddRole(objNewRole)
        End If

        Dim objSalesRoleInfo = objRoleCtrl.GetRoleByName(ps.PortalId, "Vendedores")
        If Null.IsNull(objSalesRoleInfo) Then
            Dim objNewRole As New RoleInfo() With {.PortalID = ps.PortalId, .RoleGroupID = intRoleGroupID, .RoleName = "Vendedores", .Description = "Grupo de Vendedores.", .IsPublic = False, .Status = RoleStatus.Approved}
            objRoleCtrl.AddRole(objNewRole)
        End If

        Dim objCashiersRoleInfo = objRoleCtrl.GetRoleByName(ps.PortalId, "Caixas")
        If Null.IsNull(objCashiersRoleInfo) Then
            Dim objNewRole As New RoleInfo() With {.PortalID = ps.PortalId, .RoleGroupID = intRoleGroupID, .RoleName = "Caixas", .Description = "Grupo de Caixas.", .IsPublic = False, .Status = RoleStatus.Approved}
            objRoleCtrl.AddRole(objNewRole)
        End If

    End Sub

    Private Sub CreateAccounts()

        Dim _portalCtrl = New Portals.PortalController()
        Dim objPortalInfo = _portalCtrl.GetPortal(ps.PortalId)

        Dim objRoleCtrl As New RoleController
        Dim objRegisteredRoleInfo = objRoleCtrl.GetRoleByName(ps.PortalId, objPortalInfo.RegisteredRoleName)

        ' Remove subscribers role
        Dim objRoleSubscribersInfo = RoleController.GetRoleGroupByName(ps.PortalId, "Subscribers")
        If Not Null.IsNull(objRoleSubscribersInfo) Then
            objRoleCtrl.DeleteRole(objRoleCtrl.GetRoleByName(ps.PortalId, "Subscribers").RoleID, ps.PortalId)
        End If

        '' Remove Admin if exists and set hostmaster as portal administrator
        'Dim adminUser = Users.UserController.GetUserByName(ps.PortalId, "admin")
        'If adminUser IsNot Nothing Then
        '    Users.UserController.RemoveUser(adminUser)
        'End If
        'objPortalInfo.AdministratorId = 1
        '_portalCtrl.UpdatePortalInfo(objPortalInfo)
        'objRoleCtrl.AddUserRole(ps.PortalId, 1, objPortalInfo.AdministratorRoleId, Null.NullDate)

        ' check manager username doesnt exist and add manager
        Dim objManagerUserInfo = Users.UserController.GetUserByName(ps.PortalId, "gerente")
        Dim objNewManagerUserInfo As New Users.UserInfo()
        If objManagerUserInfo Is Nothing Then
            objNewManagerUserInfo.PortalID = ps.PortalId
            objNewManagerUserInfo.Username = "gerente"
            objNewManagerUserInfo.FirstName = "Gerente"
            objNewManagerUserInfo.LastName = "Padrão"
            objNewManagerUserInfo.DisplayName = "Gerente Padrão"
            objNewManagerUserInfo.Email = "comercial@riw.com.br"
            objNewManagerUserInfo.Membership.Approved = True
            objNewManagerUserInfo.AffiliateID = Null.NullInteger
            objNewManagerUserInfo.Membership.Password = "gerente"
            objNewManagerUserInfo.LastIPAddress = Authentication.AuthenticationLoginBase.GetIPAddress()

            Dim objUserCreateStatus = Users.UserController.CreateUser(objNewManagerUserInfo)

            If objUserCreateStatus = UserCreateStatus.Success Then

                objNewManagerUserInfo.Profile.SetProfileProperty("Website", "www.rildoinformatica.net")
                objNewManagerUserInfo.Profile.SetProfileProperty("Cell", "3187777420")
                objNewManagerUserInfo.Profile.SetProfileProperty("Telephone", "3136538199")
                objNewManagerUserInfo.Profile.SetProfileProperty("Country", "Brasil")
                objNewManagerUserInfo.Profile.SetProfileProperty("Photo", "0")
                objNewManagerUserInfo.Profile.SetProfileProperty("PreferredTimeZone", "E. South America Standard Time")
                objNewManagerUserInfo.Profile.SetProfileProperty("PreferredLocale", "pt-BR")

                userPath = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(objNewManagerUserInfo).FolderPath 'PathUtils.Instance().GetUserFolderPath(objNewManagerUserInfo))).Replace("//", "/")

                'Check if this is a User Folder
                Dim UserDir = New System.IO.DirectoryInfo(objPortalInfo.HomeDirectoryMapPath & userPath.Replace("/", "\"))
                'Dim Path As String = UserDir.ToString
                If UserDir.Exists = False Then
                    'Add User folder
                    DotNetNuke.Services.FileSystem.FolderManager.Instance().AddFolder(ps.PortalId, userPath)
                End If

                If Not IO.File.Exists(String.Format("{0}{1}{2}.jpg", objPortalInfo.HomeDirectoryMapPath, userPath, CStr(objNewManagerUserInfo.UserID))) Then
                    If IO.File.Exists(String.Format("{0}{1}.jpg", objPortalInfo.HomeDirectoryMapPath, CStr(objNewManagerUserInfo.UserID))) Then
                        IO.File.Copy(String.Format("{0}{1}.jpg", objPortalInfo.HomeDirectoryMapPath, CStr(objNewManagerUserInfo.UserID)), String.Format("{0}{1}{2}.jpg", objPortalInfo.HomeDirectoryMapPath, userPath, CStr(objNewManagerUserInfo.UserID)))

                        DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(ps.PortalId, userPath)

                        Dim fileManager = DotNetNuke.Services.FileSystem.FileManager.Instance()
                        Dim folderManager = DotNetNuke.Services.FileSystem.FolderManager.Instance()
                        Dim folder = folderManager.GetFolder(ps.PortalId, userPath)

                        Dim objFileInfo = fileManager.GetFile(folder, CStr(objNewManagerUserInfo.UserID) & ".jpg")

                        objNewManagerUserInfo.Profile.SetProfileProperty("Photo", CStr(objFileInfo.FileId))

                    End If

                Else

                    'DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(ps.PortalId)

                    Dim fileManager = DotNetNuke.Services.FileSystem.FileManager.Instance()
                    Dim folderManager = DotNetNuke.Services.FileSystem.FolderManager.Instance()
                    Dim folder = folderManager.GetFolder(ps.PortalId, userPath)

                    Dim objFileInfo = fileManager.GetFile(folder, CStr(objNewManagerUserInfo.UserID) & ".jpg")

                    objNewManagerUserInfo.Profile.SetProfileProperty("Photo", CStr(objFileInfo.FileId))

                End If

                Entities.Profile.ProfileController.UpdateUserProfile(objNewManagerUserInfo)

                Dim objManagerRoleInfo = objRoleCtrl.GetRoleByName(ps.PortalId, "Gerentes")
                objRoleCtrl.AddUserRole(ps.PortalId, objNewManagerUserInfo.UserID, objManagerRoleInfo.RoleID, Null.NullDate)
                objRoleCtrl.AddUserRole(ps.PortalId, objNewManagerUserInfo.UserID, objRegisteredRoleInfo.RoleID, Null.NullDate)

                ' Remove user from subscribers role
                objRoleCtrl.UpdateUserRole(0, objNewManagerUserInfo.UserID, objRegisteredRoleInfo.RoleID, True)
            End If
        End If

        ' check sales person username doesnt exist and add sales person
        Dim objSalesUserInfo = Users.UserController.GetUserByName(ps.PortalId, "vendedor")
        Dim objNewSalesUserInfo As New Users.UserInfo()
        If objSalesUserInfo Is Nothing Then
            objNewSalesUserInfo.PortalID = ps.PortalId
            objNewSalesUserInfo.Username = "vendedor"
            objNewSalesUserInfo.FirstName = "Vendedor"
            objNewSalesUserInfo.LastName = "Padrão"
            objNewSalesUserInfo.DisplayName = "Vendedor Padrão"
            objNewSalesUserInfo.Email = "vendas@riw.com.br"
            objNewSalesUserInfo.Membership.Approved = True
            objNewSalesUserInfo.AffiliateID = Null.NullInteger
            objNewSalesUserInfo.Membership.Password = "vendedor"
            objNewSalesUserInfo.LastIPAddress = Authentication.AuthenticationLoginBase.GetIPAddress()

            Dim objUserCreateStatus = Users.UserController.CreateUser(objNewSalesUserInfo)

            If objUserCreateStatus = UserCreateStatus.Success Then

                objNewSalesUserInfo.Profile.SetProfileProperty("Website", "www.rildoinformatica.net")
                objNewSalesUserInfo.Profile.SetProfileProperty("Cell", "3187777420")
                objNewSalesUserInfo.Profile.SetProfileProperty("Telephone", "3136538199")
                objNewSalesUserInfo.Profile.SetProfileProperty("Country", "Brasil")
                objNewSalesUserInfo.Profile.SetProfileProperty("Photo", "0")
                objNewSalesUserInfo.Profile.SetProfileProperty("PreferredTimeZone", "E. South America Standard Time")
                objNewSalesUserInfo.Profile.SetProfileProperty("PreferredLocale", "pt-BR")

                'userPath = PathUtils.Instance().GetUserFolderPath(objNewSalesUserInfo).Replace("//", "/")
                userPath = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(objNewSalesUserInfo).FolderPath

                'Check if this is a User Folder
                Dim UserDir = New System.IO.DirectoryInfo(String.Concat(objPortalInfo.HomeDirectoryMapPath, userPath.Replace("/", "\")))
                Dim Path As String = UserDir.ToString
                If UserDir.Exists = False Then
                    'Add User folder
                    DotNetNuke.Services.FileSystem.FolderManager.Instance().AddFolder(ps.PortalId, userPath)
                End If

                If Not IO.File.Exists(String.Format("{0}{1}{2}.jpg", objPortalInfo.HomeDirectoryMapPath, userPath, CStr(objNewSalesUserInfo.UserID))) Then
                    If IO.File.Exists(String.Format("{0}{1}.jpg", objPortalInfo.HomeDirectoryMapPath, CStr(objNewSalesUserInfo.UserID))) Then
                        IO.File.Copy(String.Format("{0}{1}.jpg", objPortalInfo.HomeDirectoryMapPath, CStr(objNewSalesUserInfo.UserID)), String.Format("{0}{1}{2}.jpg", objPortalInfo.HomeDirectoryMapPath, userPath, CStr(objNewSalesUserInfo.UserID)))

                        DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(ps.PortalId, userPath)

                        Dim fileManager = DotNetNuke.Services.FileSystem.FileManager.Instance()
                        Dim folderManager = DotNetNuke.Services.FileSystem.FolderManager.Instance()
                        Dim folder = folderManager.GetFolder(ps.PortalId, userPath)

                        Dim objFileInfo = fileManager.GetFile(folder, CStr(objNewSalesUserInfo.UserID) & ".jpg")

                        objNewSalesUserInfo.Profile.SetProfileProperty("Photo", CStr(objFileInfo.FileId))

                    End If

                Else

                    'DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(ps.PortalId)

                    Dim fileManager = DotNetNuke.Services.FileSystem.FileManager.Instance()
                    Dim folderManager = DotNetNuke.Services.FileSystem.FolderManager.Instance()
                    Dim folder = folderManager.GetFolder(ps.PortalId, userPath)

                    Dim objFileInfo = fileManager.GetFile(folder, CStr(objNewSalesUserInfo.UserID) & ".jpg")

                    objNewSalesUserInfo.Profile.SetProfileProperty("Photo", CStr(objFileInfo.FileId))

                End If

                Entities.Profile.ProfileController.UpdateUserProfile(objNewSalesUserInfo)

                Dim objSalesRoleInfo = objRoleCtrl.GetRoleByName(ps.PortalId, "Vendedores")
                objRoleCtrl.AddUserRole(ps.PortalId, objNewSalesUserInfo.UserID, objSalesRoleInfo.RoleID, Null.NullDate)
                objRoleCtrl.AddUserRole(ps.PortalId, objNewSalesUserInfo.UserID, objRegisteredRoleInfo.RoleID, Null.NullDate)

                ' Remove user from subscribers role
                objRoleCtrl.UpdateUserRole(0, objNewSalesUserInfo.UserID, objRegisteredRoleInfo.RoleID, True)
            End If
        End If

        ' check register person username doesnt exist and add register person
        Dim objCaxiersUserInfo = Users.UserController.GetUserByName(ps.PortalId, "caixa")
        Dim objNewCaxiersUserInfo As New Users.UserInfo()
        If objCaxiersUserInfo Is Nothing Then
            objNewCaxiersUserInfo.PortalID = ps.PortalId
            objNewCaxiersUserInfo.Username = "caixa"
            objNewCaxiersUserInfo.FirstName = "Caixa"
            objNewCaxiersUserInfo.LastName = "Padrão"
            objNewCaxiersUserInfo.DisplayName = "Caixa Padrão"
            objNewCaxiersUserInfo.Email = "caixa@riw.com.br"
            objNewCaxiersUserInfo.Membership.Approved = True
            objNewCaxiersUserInfo.AffiliateID = Null.NullInteger
            objNewCaxiersUserInfo.Membership.Password = "registradora"
            objNewCaxiersUserInfo.LastIPAddress = Authentication.AuthenticationLoginBase.GetIPAddress()

            Dim objUserCreateStatus = Users.UserController.CreateUser(objNewCaxiersUserInfo)

            If objUserCreateStatus = UserCreateStatus.Success Then

                objNewCaxiersUserInfo.Profile.SetProfileProperty("Website", "www.rildoinformatica.net")
                objNewCaxiersUserInfo.Profile.SetProfileProperty("Cell", "3187777420")
                objNewCaxiersUserInfo.Profile.SetProfileProperty("Telephone", "3136538199")
                objNewCaxiersUserInfo.Profile.SetProfileProperty("Country", "Brasil")
                objNewCaxiersUserInfo.Profile.SetProfileProperty("Photo", "0")
                objNewCaxiersUserInfo.Profile.SetProfileProperty("PreferredTimeZone", "E. South America Standard Time")
                objNewCaxiersUserInfo.Profile.SetProfileProperty("PreferredLocale", "pt-BR")

                'userPath = PathUtils.Instance().GetUserFolderPath(objCaxiersUserInfo).Replace("//", "/")
                userPath = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(objNewCaxiersUserInfo).FolderPath

                'Check if this is a User Folder
                Dim UserDir = New System.IO.DirectoryInfo(String.Concat(objPortalInfo.HomeDirectoryMapPath, userPath.Replace("/", "\")))
                Dim Path As String = UserDir.ToString
                If UserDir.Exists = False Then
                    'Add User folder
                    DotNetNuke.Services.FileSystem.FolderManager.Instance().AddFolder(ps.PortalId, userPath)
                End If

                If Not IO.File.Exists(String.Format("{0}{1}{2}.jpg", objPortalInfo.HomeDirectoryMapPath, userPath, CStr(objNewCaxiersUserInfo.UserID))) Then
                    If IO.File.Exists(String.Format("{0}{1}.jpg", objPortalInfo.HomeDirectoryMapPath, CStr(objNewCaxiersUserInfo.UserID))) Then
                        IO.File.Copy(String.Format("{0}{1}.jpg", objPortalInfo.HomeDirectoryMapPath, CStr(objNewCaxiersUserInfo.UserID)), String.Format("{0}{1}{2}.jpg", objPortalInfo.HomeDirectoryMapPath, userPath, CStr(objNewCaxiersUserInfo.UserID)))

                        DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(ps.PortalId, userPath)

                        Dim fileManager = DotNetNuke.Services.FileSystem.FileManager.Instance()
                        Dim folderManager = DotNetNuke.Services.FileSystem.FolderManager.Instance()
                        Dim folder = folderManager.GetFolder(ps.PortalId, userPath)

                        Dim objFileInfo = fileManager.GetFile(folder, CStr(objNewCaxiersUserInfo.UserID) & ".jpg")

                        objNewCaxiersUserInfo.Profile.SetProfileProperty("Photo", CStr(objFileInfo.FileId))

                    End If

                Else

                    'DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(ps.PortalId)

                    Dim fileManager = DotNetNuke.Services.FileSystem.FileManager.Instance()
                    Dim folderManager = DotNetNuke.Services.FileSystem.FolderManager.Instance()
                    Dim folder = folderManager.GetFolder(ps.PortalId, userPath)

                    Dim objFileInfo = fileManager.GetFile(folder, CStr(objNewCaxiersUserInfo.UserID) & ".jpg")

                    objNewCaxiersUserInfo.Profile.SetProfileProperty("Photo", CStr(objFileInfo.FileId))

                End If

                Entities.Profile.ProfileController.UpdateUserProfile(objNewCaxiersUserInfo)

                Dim objCashiersRoleInfo = objRoleCtrl.GetRoleByName(ps.PortalId, "Caixas")
                objRoleCtrl.AddUserRole(ps.PortalId, objNewCaxiersUserInfo.UserID, objCashiersRoleInfo.RoleID, Null.NullDate)
                objRoleCtrl.AddUserRole(ps.PortalId, objNewCaxiersUserInfo.UserID, objRegisteredRoleInfo.RoleID, Null.NullDate)

                ' Remove user from subscribers role
                objRoleCtrl.UpdateUserRole(0, objNewCaxiersUserInfo.UserID, objRegisteredRoleInfo.RoleID, True)
            End If
        End If

        ' check client username doesnt exist and add consumer
        Dim objClientUserInfo = Users.UserController.GetUserByName(ps.PortalId, "consumidor")
        Dim objNewClientUserInfo As New Users.UserInfo()
        If objClientUserInfo Is Nothing Then
            objNewClientUserInfo.PortalID = ps.PortalId
            objNewClientUserInfo.Username = "consumidor"
            objNewClientUserInfo.FirstName = "Consumidor"
            objNewClientUserInfo.LastName = "Padrão"
            objNewClientUserInfo.DisplayName = "Consumidor Padrão"
            objNewClientUserInfo.Email = "sac@riw.com.br"
            objNewClientUserInfo.Membership.Approved = True
            objNewClientUserInfo.AffiliateID = Null.NullInteger
            objNewClientUserInfo.Membership.Password = "consumidor"
            objNewClientUserInfo.LastIPAddress = Authentication.AuthenticationLoginBase.GetIPAddress()

            Dim objUserCreateStatus = Users.UserController.CreateUser(objNewClientUserInfo)

            If objUserCreateStatus = UserCreateStatus.Success Then

                objNewClientUserInfo.Profile.SetProfileProperty("Website", "www.rildoinformatica.net")
                objNewClientUserInfo.Profile.SetProfileProperty("Cell", "3187777420")
                objNewClientUserInfo.Profile.SetProfileProperty("Telephone", "3136538199")
                objNewClientUserInfo.Profile.SetProfileProperty("Country", "Brasil")
                objNewClientUserInfo.Profile.SetProfileProperty("Photo", "0")
                objNewClientUserInfo.Profile.SetProfileProperty("PreferredTimeZone", "E. South America Standard Time")
                objNewClientUserInfo.Profile.SetProfileProperty("PreferredLocale", "pt-BR")

                'userPath = PathUtils.Instance().GetUserFolderPath(objNewClientUserInfo).Replace("//", "/")
                userPath = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(objNewClientUserInfo).FolderPath

                'Check if this is a User Folder
                Dim UserDir = New System.IO.DirectoryInfo(objPortalInfo.HomeDirectoryMapPath & userPath.Replace("/", "\"))
                Dim Path As String = UserDir.ToString
                If UserDir.Exists = False Then
                    'Add User folder
                    DotNetNuke.Services.FileSystem.FolderManager.Instance().AddFolder(ps.PortalId, userPath)
                End If

                If Not IO.File.Exists(String.Format("{0}{1}{2}.jpg", objPortalInfo.HomeDirectoryMapPath, userPath, CStr(objNewClientUserInfo.UserID))) Then
                    If IO.File.Exists(String.Format("{0}{1}.jpg", objPortalInfo.HomeDirectoryMapPath, CStr(objNewClientUserInfo.UserID))) Then
                        IO.File.Copy(String.Format("{0}{1}.jpg", objPortalInfo.HomeDirectoryMapPath, CStr(objNewClientUserInfo.UserID)), String.Format("{0}{1}{2}.jpg", objPortalInfo.HomeDirectoryMapPath, userPath, CStr(objNewClientUserInfo.UserID)))

                        DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(ps.PortalId, userPath)

                        Dim fileManager = DotNetNuke.Services.FileSystem.FileManager.Instance()
                        Dim folderManager = DotNetNuke.Services.FileSystem.FolderManager.Instance()
                        Dim folder = folderManager.GetFolder(ps.PortalId, userPath)

                        Dim objFileInfo = fileManager.GetFile(folder, CStr(objNewClientUserInfo.UserID) & ".jpg")

                        objNewClientUserInfo.Profile.SetProfileProperty("Photo", CStr(objFileInfo.FileId))

                    End If

                Else

                    'DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(ps.PortalId)

                    Dim fileManager = DotNetNuke.Services.FileSystem.FileManager.Instance()
                    Dim folderManager = DotNetNuke.Services.FileSystem.FolderManager.Instance()
                    Dim folder = folderManager.GetFolder(ps.PortalId, userPath)

                    Dim objFileInfo = fileManager.GetFile(folder, CStr(objNewClientUserInfo.UserID) & ".jpg")

                    objNewClientUserInfo.Profile.SetProfileProperty("Photo", CStr(objFileInfo.FileId))

                End If

                Entities.Profile.ProfileController.UpdateUserProfile(objNewClientUserInfo)

                Dim objClientsRoleInfo = objRoleCtrl.GetRoleByName(ps.PortalId, "Clientes")
                objRoleCtrl.AddUserRole(ps.PortalId, objNewClientUserInfo.UserID, objClientsRoleInfo.RoleID, Null.NullDate)
                objRoleCtrl.AddUserRole(ps.PortalId, objNewClientUserInfo.UserID, objRegisteredRoleInfo.RoleID, Null.NullDate)

                ' Remove user from subscribers role
                objRoleCtrl.UpdateUserRole(0, objNewClientUserInfo.UserID, objRegisteredRoleInfo.RoleID, True)

                Dim person As New Models.Person
                Dim personHistory As New Models.PersonHistory
                Dim personHistoryCtrl As New PersonHistoriesRepository
                Dim address As New Models.PersonAddress
                Dim personCtrl As New PeopleRepository
                Dim addressCtrl As New PersonAddressesRepository

                person.DateFounded = New Date(1900, 1, 1)
                person.DateRegistered = New Date(1900, 1, 1)
                person.PortalId = ps.PortalId
                person.PersonType = True
                person.SalesRep = Users.UserController.GetUserByName("vendedor").UserID
                person.CreatedByUser = 2
                person.CreatedOnDate = DateTime.Now()
                person.ModifiedByUser = 2
                person.ModifiedOnDate = DateTime.Now()
                person.FirstName = "Consumidor"
                person.DisplayName = "Consumidor Padrão"
                person.LastName = "Padrão"
                person.Telephone = "3138895879"
                person.Cell = "3187777420"
                person.Email = "consumidor@riw.com.br"
                person.RegisterTypes = "1"

                Dim defaultStatus As New Models.Status
                Dim defaultStatusCtrl As New StatusesRepository

                defaultStatus = defaultStatusCtrl.getStatus("Normal", ps.PortalId)

                If defaultStatus IsNot Nothing Then
                    person.StatusId = defaultStatus.StatusId
                Else
                    person.StatusId = defaultStatusCtrl.getStatuses(ps.PortalId, "False")(0).StatusId
                End If

                personCtrl.addPerson(person)

                personHistory.PersonId = person.PersonId
                personHistory.HistoryText = "<p>Conta gerada.</p>"
                personHistory.CreatedByUser = -1
                personHistory.CreatedOnDate = DateTime.Now()

                personHistoryCtrl.addPersonHistory(personHistory)

                address.PersonId = person.PersonId
                address.Street = "Rua Barão de Aiuruoca"
                address.Unit = "298"
                address.District = "João Pinheiro"
                address.City = "Belo Horizonte"
                address.Region = "MG"
                address.PostalCode = "30530090"
                address.Telephone = "3138895879"
                address.Country = "BR"
                address.ModifiedByUser = person.PersonId
                address.ModifiedOnDate = DateTime.Now()
                address.AddressName = "Principal"
                address.ViewOrder = 1
                address.CreatedByUser = person.PersonId
                address.CreatedOnDate = DateTime.Now()

                If address.Street <> "" Then
                    addressCtrl.addPersonAddress(address)
                End If

                person.UserId = objNewClientUserInfo.UserID
                personCtrl.updatePerson(person)

            End If

        End If

        ' check manager username doesnt exist and add manager
        Dim objEditorUserInfo = Users.UserController.GetUserByName(ps.PortalId, "editor")
        Dim objNewEditorUserInfo As New Users.UserInfo()
        If objEditorUserInfo Is Nothing Then
            objNewEditorUserInfo.PortalID = ps.PortalId
            objNewEditorUserInfo.Username = "editor"
            objNewEditorUserInfo.FirstName = "Editor"
            objNewEditorUserInfo.LastName = "Chefe"
            objNewEditorUserInfo.DisplayName = "Editor Chefe"
            objNewEditorUserInfo.Email = "suporte@riw.com.br"
            objNewEditorUserInfo.Membership.Approved = True
            objNewEditorUserInfo.AffiliateID = Null.NullInteger
            objNewEditorUserInfo.Membership.Password = "editorchefe"
            objNewEditorUserInfo.LastIPAddress = Authentication.AuthenticationLoginBase.GetIPAddress()

            Dim objUserCreateStatus = Users.UserController.CreateUser(objNewEditorUserInfo)

            If objUserCreateStatus = UserCreateStatus.Success Then

                objNewEditorUserInfo.Profile.SetProfileProperty("Website", "www.rildoinformatica.net")
                objNewEditorUserInfo.Profile.SetProfileProperty("Cell", "3187777420")
                objNewEditorUserInfo.Profile.SetProfileProperty("Telephone", "3136538199")
                objNewEditorUserInfo.Profile.SetProfileProperty("Country", "Brasil")
                objNewEditorUserInfo.Profile.SetProfileProperty("Photo", "0")
                objNewEditorUserInfo.Profile.SetProfileProperty("PreferredTimeZone", "E. South America Standard Time")
                objNewEditorUserInfo.Profile.SetProfileProperty("PreferredLocale", "pt-BR")

                'userPath = (String.Format("{0}", PathUtils.Instance().GetUserFolderPath(objNewEditorUserInfo))).Replace("//", "/")
                userPath = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(objNewEditorUserInfo).FolderPath

                'Check if this is a User Folder
                Dim UserDir = New System.IO.DirectoryInfo(objPortalInfo.HomeDirectoryMapPath & userPath.Replace("/", "\"))
                Dim Path As String = UserDir.ToString
                If UserDir.Exists = False Then
                    'Add User folder
                    DotNetNuke.Services.FileSystem.FolderManager.Instance().AddFolder(ps.PortalId, userPath)
                End If

                If Not IO.File.Exists(String.Format("{0}{1}{2}.jpg", objPortalInfo.HomeDirectoryMapPath, userPath, CStr(objNewEditorUserInfo.UserID))) Then
                    If IO.File.Exists(String.Format("{0}{1}.jpg", objPortalInfo.HomeDirectoryMapPath, CStr(objNewEditorUserInfo.UserID))) Then
                        IO.File.Copy(String.Format("{0}{1}.jpg", objPortalInfo.HomeDirectoryMapPath, CStr(objNewEditorUserInfo.UserID)), String.Format("{0}{1}{2}.jpg", objPortalInfo.HomeDirectoryMapPath, userPath, CStr(objNewEditorUserInfo.UserID)))

                        DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(ps.PortalId, userPath)

                        Dim fileManager = DotNetNuke.Services.FileSystem.FileManager.Instance()
                        Dim folderManager = DotNetNuke.Services.FileSystem.FolderManager.Instance()
                        Dim folder = folderManager.GetFolder(ps.PortalId, userPath)

                        Dim objFileInfo = fileManager.GetFile(folder, CStr(objNewEditorUserInfo.UserID) & ".jpg")

                        objNewEditorUserInfo.Profile.SetProfileProperty("Photo", CStr(objFileInfo.FileId))

                    End If

                Else

                    'DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(ps.PortalId)

                    Dim fileManager = DotNetNuke.Services.FileSystem.FileManager.Instance()
                    Dim folderManager = DotNetNuke.Services.FileSystem.FolderManager.Instance()
                    Dim folder = folderManager.GetFolder(ps.PortalId, userPath)

                    Dim objFileInfo = fileManager.GetFile(folder, CStr(objNewEditorUserInfo.UserID) & ".jpg")

                    objNewEditorUserInfo.Profile.SetProfileProperty("Photo", CStr(objFileInfo.FileId))

                End If

                Entities.Profile.ProfileController.UpdateUserProfile(objNewEditorUserInfo)

                Dim objEditorsRoleInfo = objRoleCtrl.GetRoleByName(ps.PortalId, "Editores")
                objRoleCtrl.AddUserRole(ps.PortalId, objNewEditorUserInfo.UserID, objEditorsRoleInfo.RoleID, Null.NullDate)
                objRoleCtrl.AddUserRole(ps.PortalId, objNewEditorUserInfo.UserID, objRegisteredRoleInfo.RoleID, Null.NullDate)

                ' Remove user from subscribers role
                objRoleCtrl.UpdateUserRole(0, objNewEditorUserInfo.UserID, objRegisteredRoleInfo.RoleID, True)

            End If
        End If

        ' Log host in
        'Users.UserController.UserLogin(ps.PortalId, Users.UserController.GetUserById(ps.PortalId, 1), objPortalInfo.PortalName, Authentication.AuthenticationLoginBase.GetIPAddress(), True)

    End Sub

End Class
