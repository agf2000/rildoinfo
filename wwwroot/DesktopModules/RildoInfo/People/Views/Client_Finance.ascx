<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Client_Finance.ascx.vb" Inherits="RIW.Modules.People.Views.ClientFinance" %>
<div id="clientFinances" class="row-fluid">
    <div class="span12">
        <div id="actionsMenu">
            <ul>
                <li id="12">
                    <i class="fa fa-chevron-left"></i>&nbsp; Retornar
                </li>
                <li id="7">
                    <i class="icon icon-edit"></i>&nbsp;Dados Básicos
                </li>
                <li id="8">
                    <i class="fa fa-phone-square"></i>&nbsp; Atender
                </li>
                <li id="9">
                    <i class="fa fa-usd"></i>&nbsp; Dados Financeiros
                </li>
                <li id="11">
                    <i class="fa fa-envelope"></i>&nbsp; Comunicar
                </li>
                <li id="10">
                    <i class="icon-tasks"></i>&nbsp;Mais Ações
                </li>
            </ul>
        </div>
        <div class="clearfix">
            &nbsp;
        </div>
        <div class="span6">
            <div class="form-horizontal">
                <div class="control-group">
                    <label class="control-label"><strong>Renda / Faturamento:</strong></label>
                    <div class="controls">
                        <input data-bind="kendoNumericTextBox: { value: income, format: 'c' }" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label"><strong>Crédito:</strong></label>
                    <div class="controls">
                        <input data-bind="kendoNumericTextBox: { value: credit, format: 'c' }" />
                    </div>
                </div>
                <button id="btnUpdateClientFinan" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Atualizar</button>
            </div>
        </div>
        <div class="pull-left">
            <div data-bind="text: finanAddress()"></div>        
        </div>
        <div class="clearfix"></div>
        <div id="clientFinanceTabs">
            <ul>
                <li id="liIncomes">Fonte de Renda</li>
                <li id="liPersonalRefs">Ref. Pessoal</li>
                <li id="liBankRefs" class="k-state-active">Ref. Bancária</li>
                <li id="liCommRefs">Ref. Comercial</li>
                <li id="liPartners">Dados dos Sócios</li>
                <li id="liPartnerBanks">Ref. Bancária dos Sócios</li>
            </ul>
            <div>
                <div id="editIncomes">
                    <div id="lvIncomeSources"></div>
                    <script type="text/x-kendo-tmpl" id="tmplClientIncomeSources"> 
                        <div class="padded">
                            <div class="well">
                                <div class="span6">
                                    <address>
                                        <strong>${ ISName }</strong>
                                        <a href="/" onclick="my.editClientIncomeSource(${ ClientIncomeSourceId }); return false;" title="Editar Renda">
                                            <i class="icon-edit"></i> Editar
                                        </a><br />
                                        # if (ISEIN) { #
                                            <strong>CNPJ: </strong>${ ISEIN }<br />
                                        # } #
                                        # if (ISST) { #
                                            <strong>Insc. Est.: </strong>${ ISST }<br />
                                        # } #
                                        # if (ISCT) { #
                                            <strong>Insc. Mun.: </strong>${ ISCT }<br />
                                        # } #
                                        <strong>Renda Mensal: </strong>#= kendo.toString(ISIncome, 'c') #<br />
                                        # if (ISProof) { #
                                            <strong>Renda pode ser comprovada? </strong>Sim<br />
                                        # } else { #
                                            <strong>Renda pode ser comprovada? </strong>Não
                                        # } #
                                    </address>
                                </div>
                                <div class="span6">
                                    <address>
                                        # if (ISAddress) { #
                                            <strong>Endereço: </strong>${ ISAddress} Nº: ${ ISAddressUnit }<br />
                                        # } #
                                        # if (ISComplement) { #
                                            <strong>Complemento: </strong>${ ISComplement }<br />
                                        # } #
                                        # if (ISDistrict) { #
                                            <strong>Bairro: </strong>${ ISDistrict }<br />
                                        # } #
                                        # if (ISCity) { #
                                            <strong>Cidade: </strong>${ ISCity }
                                        # } #
                                        # if (ISPostalCode) { #
                                            CEP: ${ my.formatPostalcode(ISPostalCode) }<br />
                                        # } #
                                        # if (ISRegion) { #
                                            <strong>Estado: </strong>${ ISRegion }<br />
                                        # } #
                                        # if (ISPhone) { #
                                            <strong>Telefone: </strong>${ ISPhone }<br />
                                        # } #
                                        # if (ISFax) { #
                                            <strong>Fax: </strong>${ ISFax }<br />
                                        # } #
                                    </address>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                        </div>
                    </script>
                    <div class="clearfix"></div>
                    <div id="collapseIncomeSource" class="accordion">
                        <div class="accordion-group">
                            <div class="accordion-heading">
                                <h4 class="panel-title">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#collapseIncomeSource" href="#collapseIncomeSourceForm">Adicionar / Editar Fonte de Renda
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseIncomeSourceForm" class="panel-collapse collapse">
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="span6">
                                            <div class="control-group">
                                                <label class="control-label required" for="iSNameTextBox"><strong>Nome:</strong></label>
                                                <div class="controls">
                                                    <input id="iSNameTextBox" type="text" class="enterastab" placeholder="Nome da fonte de renda" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="iSPhoneTextBox"><strong>Telefone:</strong></label>
                                                <div class="controls">
                                                    <input id="iSPhoneTextBox" type="text" class="enterastab" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="iSEINTextBox"><strong>CNPJ:</strong></label>
                                                <div class="controls">
                                                    <input id="iSEINTextBox" type="text" class="enterastab" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="iSNameTextBox"><strong>Insc. Est.:</strong></label>
                                                <div class="controls">
                                                    <input id="iSInsEstTextBox" type="text" class="enterastab" placeholder="Inscrição Estadual" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="iSInsMunTextBox"><strong>Insc. Mun.:</strong></label>
                                                <div class="controls">
                                                    <input id="iSInsMunTextBox" type="text" class="enterastab" placeholder="Inscrição Municipal" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label required" for="iSInsMunTextBox"><strong>Renda:</strong></label>
                                                <div class="controls">
                                                    <input id="iSIncomeNumericTextBox" class="enterastab" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="iSProofCheckbox"><strong>Renda Comprov.:</strong></label>
                                                <div class="controls ">
                                                    <label class="checkbox">
                                                        <input id="iSProofCheckbox" type="checkbox" /><span id="iSProof">Marque se puder ser comprovado.</span>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group Hidden">
                                                <label class="control-label" for="iSFaxTextBox"><strong>Fax:</strong></label>
                                                <div class="controls">
                                                    <input id="iSFaxTextBox" type="text" class="enterastab" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="iSPostalCodeTextBox"><strong>CEP:</strong></label>
                                                <div class="controls">
                                                    <input id="iSPostalCodeTextBox" type="text" class="enterastab" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="iSStreetTextBox"><strong>Endereço:</strong></label>
                                                <div class="controls">
                                                    <input id="iSStreetTextBox" type="text" class="input-medium enterastab" />
                                                    <input id="iSUnitTextBox" type="text" class="input-mini enterastab" placeholder="Nº" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="iSComplementTextBox"><strong>Complemento:</strong></label>
                                                <div class="controls" style="white-space: nowrap;">
                                                    <input id="iSComplementTextBox" type="text" class="enterastab" placeholder="Casa, Loja, etc" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="iSDistrictTextBox"><strong>Bairro:</strong></label>
                                                <div class="controls">
                                                    <input id="iSDistrictTextBox" type="text" class="enterastab" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="iSCityTextBox"><strong>Cidade:</strong></label>
                                                <div class="controls">
                                                    <input id="iSCityTextBox" type="text" class="enterastab" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="ddlISRegions"><strong>Estado:</strong></label>
                                                <div class="controls">
                                                    <div data-bind="visible: hasRegions()">
                                                        <input id="ddlISRegions" data-bind="kendoDropDownList: { data: regions, value: selectedRegion, dataTextField: 'RegionName', dataValueField: 'RegionCode', cascadeFrom: 'ddlCountries' }" />
                                                    </div>
                                                    <div data-bind="visible: !hasRegions()">
                                                        <input id="iSRegionTextBox" type="text" class="enterastab" data-bind="value: selectedRegion" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group Hidden">
                                                <label class="control-label" for="ddlISCountries"><strong>País:</strong></label>
                                                <div class="controls">
                                                    <input id="ddlISCountries" class="enterastab" data-bind="kendoDropDownList: { data: countries, value: selectedCountry, dataTextField: 'CountryName', dataValueField: 'CountryCode' }" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                        <div class="form-actions">
                                            <button id="btnUpdateIncomeSource" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar Renda</button>
                                            <button id="btnCancelIncomeSource" class="btn btn-small"><i class="fa fa-level-up"></i>&nbsp; Cancelar</button>
                                            <button id="btnRemoveIncomeSource" class="btn btn-small btn-danger" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
                                            <button id="btnCopyIncomeSource" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-copy"></i>&nbsp; Copiar</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="accordion-group">
                            <div class="accordion-heading">
                                <div class="pull-left">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#collapseIncomeSource" href="#collapseIncomeSourceHistory">
                                        <h4>Histórico (<span data-bind="text: filteredHistories().length"></span>)</h4>
                                    </a>
                                </div>
                                <div class="pull-right">
                                    <br />
                                    <strong>Filtrar: </strong>
                                    <input type="text" data-bind="value: filterHistoryTerm, valueUpdate: 'afterkeydown'" placeholder="Filtrar histórico..." />
                                    &nbsp;
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div id="collapseIncomeSourceHistory" class="accordion-body collapse">
                                <div class="accordion-inner">
                                    <div class="form-horizontal">
                                        <div class="control-group">
                                            <label class="control-label">
                                                <strong>Adicionar ao Histórico:</strong><br />
                                                <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                                                <button class="btn btn-mini togglePreview" value="preview" data-toggle="tooltip" data-provider="historyTextArea2" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                                                    <i class="fa fa-eye"></i>&nbsp; Visualizar
                                                </button>
                                            </label>
                                            <div class="controls">
                                                <textarea id="historyTextArea2" class="markdown-editor"></textarea><br />
                                                <button class="btn btn-small btn-inverse btnAddHistory" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                                            </div>
                                        </div>
                                    </div>
                                    <ul class="msgHolder" data-bind="foreach: filteredHistories">
                                        <li class="postHolder">
                                            <img data-bind="attr: { src: historyByAvatar }" />
                                            <p>
                                                <a data-bind="text: historyByName"></a>: <span class="timeago" data-bind="    html: createdOnDate"></span><span data-bind="    html: historyText"></span>
                                            </p>
                                            <div class="postFooter">
                                                <a class="linkComment" href="#" data-bind="click: toggleComment">Comentários (<span data-bind="    text: historyComments().length"></span>)</a>
                                                <div class="commentSection">
                                                    <ul data-bind="foreach: historyComments">
                                                        <li class="commentHolder">
                                                            <img data-bind="attr: { src: commentedByAvatar }"><a data-bind="    text: commentedByName"></a>: <span class="timeago" data-bind="    html: createdOnDate"></span><span data-bind="    html: commentText"></span>
                                                        </li>
                                                    </ul>
                                                    <div class="publishComment">
                                                        <input class="commentTextArea" data-bind="value: newCommentHistory, jqAutoresize: {}" placeholder="digite seu comentário..." />
                                                        <input type="button" value="Comentar" class="btnComment btn btn-mini btn-primary" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." data-bind="click: addHistoryComment.bind($data, $index())" />
                                                    </div>
                                                </div>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <div id="editPersonalRefs">
                    <div id="lvPersonalRefs"></div>
                    <script type="text/x-kendo-tmpl" id="tmplClientPersonalRefs">
                        <div class="pull-left padded">
                            <div class="well">
                                <address>
                                    <strong>${ PRName }</strong>
                                    <a href="/" onclick="my.editClientPersonalRef(${ ClientPersonalRefId }); return false;" title="Editar Referência">
                                        <i class="icon-edit"></i> Editar</a><br />
                                    # if (PRPhone.length > 0) { #
                                        <strong>Telefone: </strong>${ my.formatPhone(PRPhone) }
                                    # } #<br />
                                    # if (PREmail.length > 0) { #
                                        <strong>Email: </strong>${ PREmail }
                                    # } #
                                </address>
                            </div>
                        </div>
                    </script>
                    <div class="clearfix"></div>
                    <div id="collapsePersonalRef" class="accordion">
                        <div class="accordion-group">
                            <div class="accordion-heading">
                                <h4 class="panel-title">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#collapsePersonalRef" href="#collapsePersonalRefForm">Adicionar / Editar Referência Pessoal
                                    </a>
                                </h4>
                            </div>
                            <div id="collapsePersonalRefForm" class="panel-collapse collapse">
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="control-group">
                                            <label class="control-label required" for="personalRefNameTextBox required"><strong>Nome:</strong></label>
                                            <div class="controls">
                                                <input id="personalRefNameTextBox" type="text" class="enterastab" placeholder="Nome completo" />
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label required" for="personalRefPhoneTextBox"><strong>Telefone:</strong></label>
                                            <div class="controls">
                                                <input id="personalRefPhoneTextBox" type="text" class="enterastab" />
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label" for="personalRefEmailTextBox"><strong>Email:</strong></label>
                                            <div class="controls">
                                                <input id="personalRefEmailTextBox" type="email" class="enterastab" />
                                            </div>
                                        </div>
                                        <div class="form-actions">
                                            <button id="btnUpdatePersonalRef" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar Referência</button>
                                            <button id="btnCancelPersonalRef" class="btn btn-small"><i class="fa fa-level-up"></i>&nbsp; Cancelar</button>
                                            <button id="btnRemovePersonalRef" class="btn btn-small btn-danger" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="accordion-group">
                            <div class="accordion-heading">
                                <div class="pull-left">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#collapsePersonalRef" href="#collapsePersonalRefHistory">
                                        <h4>Histórico (<span data-bind="text: filteredHistories().length"></span>)</h4>
                                    </a>
                                </div>
                                <div class="pull-right">
                                    <br />
                                    <strong>Filtrar: </strong>
                                    <input type="text" data-bind="value: filterHistoryTerm, valueUpdate: 'afterkeydown'" placeholder="Filtrar histórico..." />
                                    &nbsp;
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div id="collapsePersonalRefHistory" class="accordion-body collapse">
                                <div class="accordion-inner">
                                    <div class="form-horizontal">
                                        <div class="control-group">
                                            <label class="control-label">
                                                <strong>Adicionar ao Histórico:</strong><br />
                                                <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                                                <button class="btn btn-mini togglePreview" value="preview" data-toggle="tooltip" data-provider="historyTextArea3" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                                                    <i class="fa fa-eye"></i>&nbsp; Visualizar
                                                </button>
                                            </label>
                                            <div class="controls">
                                                <textarea id="historyTextArea3" class="markdown-editor"></textarea><br />
                                                <button class="btn btn-small btn-inverse btnAddHistory" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                                            </div>
                                        </div>
                                    </div>
                                    <ul class="msgHolder" data-bind="foreach: filteredHistories">
                                        <li class="postHolder">
                                            <img data-bind="attr: { src: historyByAvatar }" />
                                            <p>
                                                <a data-bind="text: historyByName"></a>: <span class="timeago" data-bind="    html: createdOnDate"></span><span data-bind="    html: historyText"></span>
                                            </p>
                                            <div class="postFooter">
                                                <a class="linkComment" href="#" data-bind="click: toggleComment">Comentários (<span data-bind="    text: historyComments().length"></span>)</a>
                                                <div class="commentSection">
                                                    <ul data-bind="foreach: historyComments">
                                                        <li class="commentHolder">
                                                            <img data-bind="attr: { src: commentedByAvatar }"><a data-bind="    text: commentedByName"></a>: <span class="timeago" data-bind="    html: createdOnDate"></span><span data-bind="    html: commentText"></span>
                                                        </li>
                                                    </ul>
                                                    <div class="publishComment">
                                                        <input class="commentTextArea" data-bind="value: newCommentHistory, jqAutoresize: {}" placeholder="digite seu comentário..." />
                                                        <input type="button" value="Comentar" class="btnComment btn btn-mini btn-primary" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." data-bind="click: addHistoryComment.bind($data, $index())" />
                                                    </div>
                                                </div>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <div id="editBankRefs">
                    <div id="lvBankRefs"></div>
                    <script type="text/x-kendo-tmpl" id="tmplClientBankRefs">
                        <div class="pull-left padded">
                            <div class="well">
                                <address>
                                    <strong>${ BankRef }</strong>
                                    <a href="/" onclick="my.editClientBankRef(${ ClientBankRefId }); return false;" title="Editar Referência">
                                        <i class="icon-edit"></i> Editar</a><br />
                                    # if (BankRefAgency) { #
                                        <strong>Agência: </strong>${ BankRefAgency }<br />
                                    # } #
                                    # if(BankRefAccount) { #
                                        <strong> Conta: </strong>${ BankRefAccount }<br />
                                    # } #
                                    # if (BankRefClientSince) { #
                                        <strong>Cliente desde: </strong> ${ kendo.toString(kendo.parseDate(BankRefClientSince), 'd') }<br />
                                    # } #
                                    # if (BankRefContact) { #
                                        <strong>Contato: </strong>${ BankRefContact }
                                    # } #
                                    # if(BankRefPhone) { #
                                        - ${ my.formatPhone(BankRefPhone) }<br />
                                    # } #
                                    # if (BankRefAccountType) { #
                                        <strong>Tipo de conta: </strong> ${ BankRefAccountType }<br />
                                    # } #
                                    # if (BankRefCredit > 0) { #
                                        <strong>Crédito: </strong>${ kendo.toString(BankRefCredit, 'c') }
                                    # } #
                                </address>
                            </div>
                        </div>
                    </script>
                    <div class="clearfix"></div>
                    <div id="collapseBankRef" class="accordion">
                        <div class="accordion-group">
                            <div class="accordion-heading">
                                <h4 class="panel-title">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#collapseBankRef" href="#collapseBankRefForm">Adicionar / Editar Referência Bancária
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseBankRefForm" class="panel-collapse collapse">
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="span6">
                                            <div class="control-group">
                                                <label class="control-label required" for="bankRefTextBox"><strong>Banco:</strong></label>
                                                <div class="controls">
                                                    <input id="bankRefTextBox" type="text" class="enterastab" placeholder="Nome do banco" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label required" for="bankRefAgencyTextBox"><strong>Agência:</strong></label>
                                                <div class="controls">
                                                    <input id="bankRefAgencyTextBox" type="text" class="enterastab" placeholder="Número da agência" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label required" for="bankRefAccountTextBox"><strong>Conta:</strong></label>
                                                <div class="controls">
                                                    <input id="bankRefAccountTextBox" type="text" class="enterastab" placeholder="Número da conta" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label required" for="bankRefClientSinceTextBox"><strong>Cliente desde:</strong></label>
                                                <div class="controls">
                                                    <input id="bankRefClientSinceTextBox" class="enterastab clientContact Combo" placeholder="ex: 05/12/2000" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <label class="control-label" for="bankRefContactNameTextBox"><strong>Contato:</strong></label>
                                                <div class="controls">
                                                    <input id="bankRefContactNameTextBox" type="text" class="enterastab" placeholder="Nome do Contato" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="bankRefContactPhoneTextBox"><strong>Telefone:</strong></label>
                                                <div class="controls">
                                                    <input id="bankRefContactPhoneTextBox" type="text" class="enterastab" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label required" for="bankRefAccountTypeTextBox"><strong>Tipo de Conta:</strong></label>
                                                <div class="controls">
                                                    <input id="bankRefAccountTypeTextBox" type="text" class="enterastab" placeholder="Conta Corrente, Poupança, etc" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="bankRefCreditLimitTextBox"><strong>Limite de Crédito:</strong></label>
                                                <div class="controls">
                                                    <input id="bankRefCreditLimitTextBox" class="enterastab" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                        <div class="form-actions">
                                            <button id="btnUpdateBankRef" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar Referência</button>
                                            <button id="btnCancelBankRef" class="btn btn-small"><i class="fa fa-level-up"></i>&nbsp; Cancelar</button>
                                            <button id="btnRemoveBankRef" class="btn btn-small btn-danger" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="accordion-group">
                            <div class="accordion-heading">
                                <div class="pull-left">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#collapseBankRef" href="#collapseBankRefHistory">
                                        <h4>Histórico (<span data-bind="text: filteredHistories().length"></span>)</h4>
                                    </a>
                                </div>
                                <div class="pull-right">
                                    <br />
                                    <strong>Filtrar: </strong>
                                    <input type="text" data-bind="value: filterHistoryTerm, valueUpdate: 'afterkeydown'" placeholder="Filtrar histórico..." />
                                    &nbsp;
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div id="collapseBankRefHistory" class="accordion-body collapse">
                                <div class="accordion-inner">
                                    <div class="form-horizontal">
                                        <div class="control-group">
                                            <label class="control-label">
                                                <strong>Adicionar ao Histórico:</strong><br />
                                                <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                                                <button class="btn btn-mini togglePreview" value="preview" data-toggle="tooltip" data-provider="historyTextArea4" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                                                    <i class="fa fa-eye"></i>&nbsp; Visualizar
                                                </button>
                                            </label>
                                            <div class="controls">
                                                <textarea id="historyTextArea4" class="markdown-editor"></textarea><br />
                                                <button class="btn btn-small btn-inverse btnAddHistory" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                                            </div>
                                        </div>
                                    </div>
                                    <ul class="msgHolder" data-bind="foreach: filteredHistories">
                                        <li class="postHolder">
                                            <img data-bind="attr: { src: historyByAvatar }" />
                                            <p>
                                                <a data-bind="text: historyByName"></a>: <span class="timeago" data-bind="    html: createdOnDate"></span><span data-bind="    html: historyText"></span>
                                            </p>
                                            <div class="postFooter">
                                                <a class="linkComment" href="#" data-bind="click: toggleComment">Comentários (<span data-bind="    text: historyComments().length"></span>)</a>
                                                <div class="commentSection">
                                                    <ul data-bind="foreach: historyComments">
                                                        <li class="commentHolder">
                                                            <img data-bind="attr: { src: commentedByAvatar }"><a data-bind="    text: commentedByName"></a>: <span class="timeago" data-bind="    html: createdOnDate"></span><span data-bind="    html: commentText"></span>
                                                        </li>
                                                    </ul>
                                                    <div class="publishComment">
                                                        <input class="commentTextArea" data-bind="value: newCommentHistory, jqAutoresize: {}" placeholder="digite seu comentário..." />
                                                        <input type="button" value="Comentar" class="btnComment btn btn-mini btn-primary" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." data-bind="click: addHistoryComment.bind($data, $index())" />
                                                    </div>
                                                </div>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <div id="editCommRefs">
                    <div id="lvCommRefs"></div>
                    <script type="text/x-kendo-tmpl" id="tmplClientCommRefs">
                        <div class="pull-left padded">
                            <div class="well">
                                <address>
                                    <strong>${ CommRefBusiness }</strong>
                                    <a href="/" onclick="my.editClientCommRef(${ ClientCommRefId }); return false;" title="Editar Referência">
                                        <i class="icon-edit"></i> Editar</a><br />
                                    # if (CommRefContact) { #
                                        <strong>Contato: </strong>${ CommRefContact }<br />
                                    # } #
                                    # if (CommRefPhone) { #
                                        <strong>Telefone:</strong> ${ my.formatPhone(CommRefPhone) }<br />
                                    # } #
                                    # if (CommRefLastActivity) { #
                                        <strong>Última atividade: </strong> ${ kendo.toString(kendo.parseDate(CommRefLastActivity), 'd') }<br />
                                    # } #
                                    # if (CommRefCredit > 0) { #
                                        <strong>Crédito: </strong>${ kendo.toString(CommRefCredit, 'c') }<br />
                                    # } #
                                </address>
                            </div>
                        </div>
                    </script>
                    <div class="clearfix"></div>
                    <div id="collapseCommRef" class="accordion">
                        <div class="accordion-group">
                            <div class="accordion-heading">
                                <h4 class="panel-title">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#collapseCommRef" href="#collapseCommRefForm">Adicionar / Editar Referência Comercial
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseCommRefForm" class="panel-collapse collapse">
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="span6">
                                            <div class="control-group">
                                                <label class="control-label required" for="commRefNameTextBox"><strong>Razão Social:</strong></label>
                                                <div class="controls">
                                                    <input id="commRefNameTextBox" type="text" class="enterastab" placeholder="Nome da instituição comercial" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="commRefContactTextBox"><strong>Contato:</strong></label>
                                                <div class="controls">
                                                    <input id="commRefContactTextBox" type="text" class="enterastab" placeholder="Nome do contato" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label required" for="commRefPhoneTextBox"><strong>Telefone:</strong></label>
                                                <div class="controls">
                                                    <input id="commRefPhoneTextBox" type="text" class="enterastab" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <label class="control-label" for="commRefLastActivityTextBox"><strong>Última Compra:</strong></label>
                                                <div class="controls">
                                                    <input id="commRefLastActivityTextBox" class="enterastab Combo" placeholder="Data última compra" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="commRefCreditTextBox"><strong>Crédito:</strong></label>
                                                <div class="controls">
                                                    <input id="commRefCreditTextBox" class="enterastab" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                        <div class="form-actions">
                                            <button id="btnUpdateCommRef" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar Referência</button>
                                            <button id="btnCancelCommRef" class="btn btn-small"><i class="fa fa-level-up"></i>&nbsp; Cancelar</button>
                                            <button id="btnRemoveCommRef" class="btn btn-small btn-danger" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="accordion-group">
                            <div class="accordion-heading">
                                <div class="pull-left">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#collapseCommRef" href="#collapseCommRefHistory">
                                        <h4>Histórico (<span data-bind="text: filteredHistories().length"></span>)</h4>
                                    </a>
                                </div>
                                <div class="pull-right">
                                    <br />
                                    <strong>Filtrar: </strong>
                                    <input type="text" data-bind="value: filterHistoryTerm, valueUpdate: 'afterkeydown'" placeholder="Filtrar histórico..." />
                                    &nbsp;
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div id="collapseCommRefHistory" class="accordion-body collapse">
                                <div class="accordion-inner">
                                    <div class="form-horizontal">
                                        <div class="control-group">
                                            <label class="control-label">
                                                <strong>Adicionar ao Histórico:</strong><br />
                                                <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                                                <button class="btn btn-mini togglePreview" value="preview" data-toggle="tooltip" data-provider="historyTextArea5" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                                                    <i class="fa fa-eye"></i>&nbsp; Visualizar
                                                </button>
                                            </label>
                                            <div class="controls">
                                                <textarea id="historyTextArea5" class="markdown-editor"></textarea><br />
                                                <button class="btn btn-small btn-inverse btnAddHistory" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                                            </div>
                                        </div>
                                    </div>
                                    <ul class="msgHolder" data-bind="foreach: filteredHistories">
                                        <li class="postHolder">
                                            <img data-bind="attr: { src: historyByAvatar }" />
                                            <p>
                                                <a data-bind="text: historyByName"></a>: <span class="timeago" data-bind="    html: createdOnDate"></span><span data-bind="    html: historyText"></span>
                                            </p>
                                            <div class="postFooter">
                                                <a class="linkComment" href="#" data-bind="click: toggleComment">Comentários (<span data-bind="    text: historyComments().length"></span>)</a>
                                                <div class="commentSection">
                                                    <ul data-bind="foreach: historyComments">
                                                        <li class="commentHolder">
                                                            <img data-bind="attr: { src: commentedByAvatar }"><a data-bind="    text: commentedByName"></a>: <span class="timeago" data-bind="    html: createdOnDate"></span><span data-bind="    html: commentText"></span>
                                                        </li>
                                                    </ul>
                                                    <div class="publishComment">
                                                        <input class="commentTextArea" data-bind="value: newCommentHistory, jqAutoresize: {}" placeholder="digite seu comentário..." />
                                                        <input type="button" value="Comentar" class="btnComment btn btn-mini btn-primary" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." data-bind="click: addHistoryComment.bind($data, $index())" />
                                                    </div>
                                                </div>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <div id="editPartners">
                    <div id="lvClientPartners"></div>
                    <script type="text/x-kendo-tmpl" id="tmplClientPartners">
                        <div class="pull-left padded">
                            <div class="well">
                                <div class="pull-left">
                                    <address>
                                        <strong>${ PartnerName }</strong>
                                        <a href="/" onclick="my.editClientPartner(${ ClientPartnerId }); return false;" title="Editar Referência">
                                            <i class="icon-edit"></i> Editar</a><br />
                                        # if (PartnerCPF) { #
                                            <strong>CPF: </strong>${ PartnerCPF }<br />
                                        # } #
                                        # if (PartnerIdentity) { #
                                            <strong>RG: </strong>${ PartnerIdentity }<br />
                                        # } #
                                        # if (PartnerPhone) { #
                                            <strong>Telefone: </strong>${ my.formatPhone(PartnerPhone) }<br />
                                        # } #
                                        # if (PartnerCell) { #
                                            <strong>Celular: </strong>${ my.formatPhone(PartnerCell) }<br />
                                        # } #
                                        # if (PartnerEmail) { #
                                            <strong>Email: </strong>${ PartnerEmail }<br />
                                        # } #
                                        # if (PartnerQuota > 0) { #
                                            <strong>Participação: </strong>${ PartnerQuota } %<br />
                                        # } #
                                    </address>
                                </div>
                                <div class="pull-left">
                                    <address>
                                        # if (PartnerAddress) { #
                                            <strong>Endereço: </strong>${ PartnerAddress } <strong> Nº: </strong>${ PartnerAddressUnit }<br />
                                        # } #
                                        # if (PartnerComplement) { #
                                            ${ PartnerComplement }<br />
                                        # } #
                                        # if (PartnerDistrict) { #
                                            <strong>Bairro: </strong>${ PartnerDistrict }<br />
                                        # } #
                                        # if (PartnerCity) { #
                                            <strong>Cidade: </strong>${ PartnerCity }
                                        # } #
                                        # if (PartnerPostalCode) { #
                                            &nbsp; <strong>CEP: </strong>${ PartnerPostalCode }<br />
                                        # } #
                                        # if (PartnerRegion) { #
                                            <strong>Estado: </strong>${ PartnerRegion }
                                        # } #
                                    </address>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                        </div>
                    </script>
                    <div class="clearfix"></div>
                    <div id="collapsePartner" class="accordion">
                        <div class="accordion-group">
                            <div class="accordion-heading">
                                <h4 class="panel-title">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#collapsePartner" href="#collapsePartnerForm">Adicionar / Editar Dados dos Sócios
                                    </a>
                                </h4>
                            </div>
                            <div id="collapsePartnerForm" class="panel-collapse collapse">
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="span6">
                                            <div class="control-group">
                                                <label class="control-label required" for="partnerNameTextBox"><strong>Nome:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerNameTextBox" type="text" class="enterastab" placeholder="Nome completo do sócio" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="partnerCPFTextBox"><strong>CPF:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerCPFTextBox" type="text" class="enterastab" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="partnerIdentTextBox"><strong>RG:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerIdentTextBox" type="text" class="enterastab" placeholder="Número da Identidade" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label required" for="partnerPhoneTextBox"><strong>Telefone:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerPhoneTextBox" type="text" class="enterastab" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="partnerCellTextBox"><strong>Celular:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerCellTextBox" type="text" class="enterastab" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="partnerEmailTextBox"><strong>Email:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerEmailTextBox" type="email" class="enterastab" placeholder="Endereço de email do sócio" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="partnerQuotaTextBox"><strong>Participação:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerQuotaTextBox" name="partnerQuotaTextBox" class="enterastab" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <label class="control-label" for="partnerPostalCodeTextBox"><strong>CEP:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerPostalCodeTextBox" type="text" class="enterastab" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="partnerStreetTextBox"><strong>Endereço:</strong></label>
                                                <div class="controls" style="white-space: nowrap;">
                                                    <input id="partnerStreetTextBox" type="text" class="input-medium enterastab" />
                                                    <input id="partnerUnitTextBox" type="text" class="input-mini enterastab" placeholder="Nº" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="partnerComplementTextBox"><strong>Complemento:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerComplementTextBox" type="text" class="enterastab" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="partnerDistrictTextBox"><strong>Bairro:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerDistrictTextBox" type="text" class="enterastab" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="partnerCityTextBox"><strong>Cidade:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerCityTextBox" type="text" class="enterastab" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="ddlPartnerRegions"><strong>Estado:</strong></label>
                                                <div class="controls">
                                                    <div data-bind="visible: hasRegions()">
                                                        <input id="ddlPartnerRegions" data-bind="kendoDropDownList: { data: regions, value: selectedRegion, dataTextField: 'RegionName', dataValueField: 'RegionCode', cascadeFrom: 'ddlCountries' }" />
                                                    </div>
                                                    <div data-bind="visible: !hasRegions()">
                                                        <input id="partnerRegionTextBox" type="text" class="enterastab" data-bind="value: selectedRegion" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group Hidden">
                                                <label class="control-label" for="ddlPartnerCountries"><strong>País:</strong></label>
                                                <div class="controls">
                                                    <input id="ddlPartnerCountries" class="enterastab" data-bind="kendoDropDownList: { data: countries, value: selectedCountry, dataTextField: 'CountryName', dataValueField: 'CountryCode' }" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                        <div class="form-actions">
                                            <button id="btnUpdatePartner" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar Sócio</button>
                                            <button id="btnCancelPartner" class="btn btn-small"><i class="fa fa-level-up"></i>&nbsp; Cancelar</button>
                                            <button id="btnRemovePartner" class="btn btn-small btn-danger" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
                                            <button id="btnCopyPartner" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-copy"></i>&nbsp; Copiar</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="accordion-group">
                            <div class="accordion-heading">
                                <div class="pull-left">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#collapsePartner" href="#collapsePartnerHistory">
                                        <h4>Histórico (<span data-bind="text: filteredHistories().length"></span>)</h4>
                                    </a>
                                </div>
                                <div class="pull-right">
                                    <br />
                                    <strong>Filtrar: </strong>
                                    <input type="text" data-bind="value: filterHistoryTerm, valueUpdate: 'afterkeydown'" placeholder="Filtrar histórico..." />
                                    &nbsp;
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div id="collapsePartnerHistory" class="accordion-body collapse">
                                <div class="accordion-inner">
                                    <div class="form-horizontal">
                                        <div class="control-group">
                                            <label class="control-label">
                                                <strong>Adicionar ao Histórico:</strong><br />
                                                <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                                                <button class="btn btn-mini togglePreview" value="preview" data-toggle="tooltip" data-provider="historyTextArea6" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                                                    <i class="fa fa-eye"></i>&nbsp; Visualizar
                                                </button>
                                            </label>
                                            <div class="controls">
                                                <textarea id="historyTextArea6" class="markdown-editor"></textarea><br />
                                                <button class="btn btn-small btn-inverse btnAddHistory" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                                            </div>
                                        </div>
                                    </div>
                                    <ul class="msgHolder" data-bind="foreach: filteredHistories">
                                        <li class="postHolder">
                                            <img data-bind="attr: { src: historyByAvatar }" />
                                            <p>
                                                <a data-bind="text: historyByName"></a>: <span class="timeago" data-bind="    html: createdOnDate"></span><span data-bind="    html: historyText"></span>
                                            </p>
                                            <div class="postFooter">
                                                <a class="linkComment" href="#" data-bind="click: toggleComment">Comentários (<span data-bind="    text: historyComments().length"></span>)</a>
                                                <div class="commentSection">
                                                    <ul data-bind="foreach: historyComments">
                                                        <li class="commentHolder">
                                                            <img data-bind="attr: { src: commentedByAvatar }"><a data-bind="    text: commentedByName"></a>: <span class="timeago" data-bind="    html: createdOnDate"></span><span data-bind="    html: commentText"></span>
                                                        </li>
                                                    </ul>
                                                    <div class="publishComment">
                                                        <input class="commentTextArea" data-bind="value: newCommentHistory, jqAutoresize: {}" placeholder="digite seu comentário..." />
                                                        <input type="button" value="Comentar" class="btnComment btn btn-mini btn-primary" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." data-bind="click: addHistoryComment.bind($data, $index())" />
                                                    </div>
                                                </div>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <div id="editPartnerBankRefs">
                    <div id="lvPartnerBankRefs"></div>
                    <script type="text/x-kendo-tmpl" id="tmplClientPartnerBankRefs">
                        <div class="pull-left padded">
                            <div class="well">
                                <address>
                                    <strong>${ PartnerName }</strong>
                                    <a href="/" onclick="my.editClientPartnerBankRef(${ ClientPartnerBankRefId }); return false;" title="Editar Referência">
                                        <i class="icon-edit"></i> Editar</a><br />
                                    # if (BankRef.length > 0) { #
                                        <strong>Banco: </strong>${ BankRef }<br />
                                    # } #
                                    # if (BankRefAgency) { #
                                        <strong>Agência: </strong>${ BankRefAgency }<br />
                                    # } #
                                    # if(BankRefAccount) { #
                                        <strong>Conta: </strong>${ BankRefAccount }<br />
                                    # } #
                                    # if (BankRefClientSince) { #
                                        <strong>Cliente desde: </strong> ${ kendo.toString(kendo.parseDate(BankRefClientSince), 'd') }<br />
                                    # } #
                                    # if (BankRefContact) { #
                                        <strong>Contato: </strong>${ BankRefContact }
                                    # } #
                                    # if(BankRefPhone) { #
                                        <strong>Telefone: </strong>${ BankRefPhone }<br />
                                    # } #
                                    # if (BankRefAccountType) { #
                                        <strong>Tipo de conta: </strong> ${ BankRefAccountType }<br />
                                    # } #
                                    # if (BankRefCredit > 0) { #
                                        <strong>Crédito: </strong>${ kendo.toString(BankRefCredit, 'c') }
                                    # } #
                                </address>
                            </div>
                        </div>
                    </script>
                    <div class="clearfix"></div>
                    <div id="collapseBankPartnerRef" class="accordion">
                        <div class="accordion-group">
                            <div class="accordion-heading">
                                <h4 class="panel-title">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#collapseBankPartnerRef" href="#collapseBankPartnerRefForm">Adicionar / Editar Referência Bancária dos Sócios
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseBankPartnerRefForm" class="panel-collapse collapse">
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="span6">
                                            <div class="control-group">
                                                <label class="control-label required" for="partnerBankRefPartnerFullNameTextBox"><strong>Nome:</strong></label>
                                                <div class="controls">
                                                    <input id="ddlClientPartners" class="enterastab" placeholder=" Selecionar Sócio " style="width: 220px;" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label required" for="partnerBankRefTextBox"><strong>Banco:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerBankRefTextBox" type="text" class="enterastab" placeholder="Nome do banco" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label required" for="partnerBankRefAgencyTextBox"><strong>Agência:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerBankRefAgencyTextBox" type="text" class="enterastab" placeholder="Número da agência" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label required" for="partnerBankRefAccountTextBox"><strong>Conta:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerBankRefAccountTextBox" type="text" class="enterastab" placeholder="Número da conta" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="partnerBankRefCreditLimitTextBox"><strong>Crédito:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerBankRefCreditLimitTextBox" class="enterastab" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <label class="control-label" for="partnerBankRefContactNameTextBox"><strong>Contato:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerBankRefContactNameTextBox" type="text" class="enterastab" placeholder="Nome do contato" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="partnerBankRefContactPhoneTextBox"><strong>Telefone:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerBankRefContactPhoneTextBox" type="text" class="enterastab" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label required" for="partnerBankRefAccountTypeTextBox"><strong>Tipo de Conta:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerBankRefAccountTypeTextBox" type="text" class="enterastab" placeholder="Conta Corrente, Poupança, etc" />
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label required" for="partnerBankRefClientSinceTextBox"><strong>Cliente desde:</strong></label>
                                                <div class="controls">
                                                    <input id="partnerBankRefClientSinceTextBox" class="enterastab Combo" placeholder="ex: 05/12/2000" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                        <div class="form-actions">
                                            <button id="btnUpdatePartnerBankRef" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar Referência</button>
                                            <button id="btnCancelPartnerBankRef" class="btn btn-small"><i class="fa fa-level-up"></i>&nbsp; Cancelar</button>
                                            <button id="btnRemovePartnerBankRef" class="btn btn-small btn-danger" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="accordion-group">
                            <div class="accordion-heading">
                                <div class="pull-left">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#collapseBankPartnerRef" href="#collapsePartnerRefHistory">
                                        <h4>Histórico (<span data-bind="text: filteredHistories().length"></span>)</h4>
                                    </a>
                                </div>
                                <div class="pull-right">
                                    <br />
                                    <strong>Filtrar: </strong>
                                    <input type="text" data-bind="value: filterHistoryTerm, valueUpdate: 'afterkeydown'" placeholder="Filtrar histórico..." />
                                    &nbsp;
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div id="collapsePartnerRefHistory" class="accordion-body collapse">
                                <div class="accordion-inner">
                                    <div class="form-horizontal">
                                        <div class="control-group">
                                            <label class="control-label">
                                                <strong>Adicionar ao Histórico:</strong><br />
                                                <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                                                <button class="btn btn-mini togglePreview" value="preview" data-toggle="tooltip" data-provider="historyTextArea7" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                                                    <i class="fa fa-eye"></i>&nbsp; Visualizar
                                                </button>
                                            </label>
                                            <div class="controls">
                                                <textarea id="historyTextArea7" class="markdown-editor"></textarea><br />
                                                <button class="btn btn-small btn-inverse btnAddHistory" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                                            </div>
                                        </div>
                                    </div>
                                    <ul class="msgHolder" data-bind="foreach: filteredHistories">
                                        <li class="postHolder">
                                            <img data-bind="attr: { src: historyByAvatar }" />
                                            <p>
                                                <a data-bind="text: historyByName"></a>: <span class="timeago" data-bind="    html: createdOnDate"></span><span data-bind="    html: historyText"></span>
                                            </p>
                                            <div class="postFooter">
                                                <a class="linkComment" href="#" data-bind="click: toggleComment">Comentários (<span data-bind="    text: historyComments().length"></span>)</a>
                                                <div class="commentSection">
                                                    <ul data-bind="foreach: historyComments">
                                                        <li class="commentHolder">
                                                            <img data-bind="attr: { src: commentedByAvatar }"><a data-bind="    text: commentedByName"></a>: <span class="timeago" data-bind="    html: createdOnDate"></span><span data-bind="    html: commentText"></span>
                                                        </li>
                                                    </ul>
                                                    <div class="publishComment">
                                                        <input class="commentTextArea" data-bind="value: newCommentHistory, jqAutoresize: {}" placeholder="digite seu comentário..." />
                                                        <input type="button" value="Comentar" class="btnComment btn btn-mini btn-primary" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." data-bind="click: addHistoryComment.bind($data, $index())" />
                                                    </div>
                                                </div>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="status NormalRed"></div>
    <div id="HTML5Audio">
        <input id="audiofile" type="text" value="" style="display: none;" />
    </div>
    <audio id="myaudio">
        <script>
            function LegacyPlaySound(soundobj) {
                var thissound = document.getElementById(soundobj);
                thissound.Play();
            }
        </script>
        <span id="OldSound"></span>
        <input type="button" value="Play Sound" onclick="LegacyPlaySound('LegacySound')">
    </audio>
    <div class="hidden">
        <div id="editBasicFinance">
            <div class="form-horizontal">
                <div class="span7">
                    <div id="divClientName" class="control-group">
                        <label class="control-label"><strong>Nome completo:</strong></label>
                        <div class="controls">
                            <label class="inline">
                                <span data-bind="text: displayName()"></span>
                            </label>
                        </div>
                    </div>
                    <div id="divBusinessName" class="control-group">
                        <label class="control-label"><strong>Razão Social:</strong></label>
                        <div class="controls">
                            <label class="inline">
                                <span id="clientCompanyName" data-bind="text: companyName()"></span>
                            </label>
                        </div>
                    </div>
                    <div id="divDisplayName" class="control-group">
                        <label class="control-label"><strong>Display Name:</strong></label>
                        <div class="controls">
                            <label class="inline">
                                <span id="clientDisplayName" data-bind="text: displayName()"></span>
                            </label>
                        </div>
                    </div>
                    <div id="divEIN2" class="control-group">
                        <label class="control-label"><strong>CNPJ:</strong></label>
                        <div class="controls">
                            <label class="inline">
                                <span id="clientEIN" data-bind="text: ein()"></span>
                            </label>
                        </div>
                    </div>
                    <div id="divST2" class="control-group">
                        <label class="control-label"><strong>Insc. Est.:</strong></label>
                        <div class="controls">
                            <label class="inline">
                                <span id="clientInsEst" data-bind="text: stateTax()"></span>
                            </label>
                        </div>
                    </div>
                    <div id="divIM2" class="control-group">
                        <label class="control-label"><strong>Insc. Mun.:</strong></label>
                        <div class="controls">
                            <label class="inline">
                                <span class="clientInsMun" data-bind="text: cityTax()"></span>
                            </label>
                        </div>
                    </div>
                    <div id="divCPF2" class="control-group">
                        <label class="control-label"><strong>CPF:</strong></label>
                        <div class="controls">
                            <label class="inline">
                                <span id="clientCPF" data-bind="text: cpf()"></span>
                            </label>
                        </div>
                    </div>
                    <div id="divIdent2" class="control-group">
                        <label class="control-label"><strong>RG:</strong></label>
                        <div class="controls">
                            <label class="inline">
                                <span id="clientIdent" data-bind="text: ident()"></span>
                            </label>
                        </div>
                    </div>
                </div>
                <div class="pull-left">
                    <br />
                    <div class="form-horizontal">
                        <div class="control-group">
                            <label class="control-label"><strong>Endereço:</strong></label>
                            <div class="controls">
                                <input id="ddlFinanAddress" class="enterastab" data-bind="kendoDropDownList: { data: addresses, value: selectedFinanAddress, dataTextField: 'AddressName', dataValueField: 'PersonAddressId', optionLabel: 'Selecionar' }" />
                                <div data-bind="text: finanAddress()"></div>
                            </div>
                        </div>
                    </div>
                    <div id="addressLiteral"></div>
                </div>
                <div class="clearfix">
                </div>
                <div class="form-actions">
                </div>
            </div>
            <div class="accordion">
                <div class="accordion-group">
                    <div class="accordion-heading">
                        <div class="pull-left">
                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseHistory">
                                <h4>Histórico (<span data-bind="text: filteredHistories().length"></span>)</h4>
                            </a>
                        </div>
                        <div class="pull-right">
                            <br />
                            <strong>Filtrar: </strong>
                            <input type="text" data-bind="value: filterHistoryTerm, valueUpdate: 'afterkeydown'" placeholder="Filtrar histórico..." />
                            &nbsp;
                        </div>
                        <div class="clearfix"></div>
                    </div>
                    <div id="collapseHistory" class="accordion-body collapse">
                        <div class="accordion-inner">
                            <div class="form-horizontal">
                                <div class="control-group">
                                    <label class="control-label">
                                        <strong>Adicionar ao Histórico:</strong><br />
                                        <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                                        <button class="btn btn-mini togglePreview" value="preview" data-toggle="tooltip" data-provider="historyTextArea1" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                                            <i class="fa fa-eye"></i>&nbsp; Visualizar
                                        </button>
                                    </label>
                                    <div class="controls">
                                        <textarea id="historyTextArea1" class="markdown-editor"></textarea><br />
                                        <button class="btn btn-small btn-inverse btnAddHistory" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                                    </div>
                                </div>
                            </div>
                            <ul class="msgHolder" data-bind="foreach: filteredHistories">
                                <li class="postHolder">
                                    <img data-bind="attr: { src: historyByAvatar }" />
                                    <p>
                                        <a data-bind="text: historyByName"></a>: <span class="timeago" data-bind="    html: createdOnDate"></span><span data-bind="    html: historyText"></span>
                                    </p>
                                    <div class="postFooter">
                                        <a class="linkComment" href="#" data-bind="click: toggleComment">Comentários (<span data-bind="    text: historyComments().length"></span>)</a>
                                        <div class="commentSection">
                                            <ul data-bind="foreach: historyComments">
                                                <li class="commentHolder">
                                                    <img data-bind="attr: { src: commentedByAvatar }"><a data-bind="    text: commentedByName"></a>: <span class="timeago" data-bind="    html: createdOnDate"></span><span data-bind="    html: commentText"></span>
                                                </li>
                                            </ul>
                                            <div class="publishComment">
                                                <input class="commentTextArea" data-bind="value: newCommentHistory, jqAutoresize: {}" placeholder="digite seu comentário..." />
                                                <input type="button" value="Comentar" class="btnComment btn btn-mini btn-primary" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." data-bind="click: addHistoryComment.bind($data, $index())" />
                                            </div>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
