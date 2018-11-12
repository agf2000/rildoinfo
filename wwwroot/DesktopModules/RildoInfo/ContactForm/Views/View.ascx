<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="View.ascx.vb" Inherits="RIW.Modules.ContactForm.View" %>
<div id="contactForm" class="container-fluid Normal">
    <div class="pull-left">
        <div class="form-horizontal">
            <div id="divMethod" data-bind="visible: reqSend">
                <div class="control-group">
                    <label class="control-label">
                        <strong>Método Envio:</strong>
                        <i class="fa fa-exclamation-circle" title="Método de envio" data-content="Escolha o método de envio caso deseja receber nossos catálagos"></i>
                    </label>
                    <div class="controls">
                        <label class="checkbox inline">
                            <input type="checkbox" value="1" data-bind="checked: emailMethodType" />
                            <i class="fa fa-envelope"></i>
                            Email
                        </label>
                        <label class="checkbox inline">
                            <input type="checkbox" value="2" data-bind="checked: pOMethodType" />
                            <i class="icon icon-correio"></i>
                            Correio
                        </label>
                    </div>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="companyTextBox"><strong>Empresa:</strong></label>
                <div class="controls">
                    <input id="companyTextBox" type="text" class="enterastab" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label required" for="displayNameTextBox"><strong>Seu Nome:</strong></label>
                <div class="controls">
                    <input id="displayNameTextBox" type="text" class="enterastab" required placeholder="Nome Completo" data-role="validate" title="Seu Nome" data-content="Favor inserir seu nome completo" />
                </div>
            </div>
            <div class="control-group">
                <label for="phoneTextBox" data-bind="attr: { 'class': (reqTelephone() ? 'control-label required' : 'control-label') }"><strong>Telefone:</strong></label>
                <div class="controls">
                    <input id="phoneTextBox" type="text" class="enterastab" placeholder="(55) 5555-5555" title="Seu Telefone" data-content="Favor inserir seu telefone de contato" data-bind="attr: { 'required': reqTelephone(), 'data-role': (reqTelephone() ? 'validate' : '') }" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="emailTextBox"><strong>Email:</strong></label>
                <div class="controls">
                    <input id="emailTextBox" class="enterastab" type="email" placeholder="ex. nome@yahoo.com" title="Email" data-content="Favor inserir um endereço de email válido" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="websiteTextBox"><strong>Website:</strong></label>
                <div class="controls">
                    <input id="websiteTextBox" type="text" class="enterastab" placeholder="http://" />
                </div>
            </div>
        </div>
    </div>
    <div class="pull-left">
        <div class="form-horizontal" data-bind="visible: reqAddress">
            <div class="control-group">
                <label class="control-label" for="postalCodeTextBox"><strong>Código Postal:</strong></label>
                <div class="controls">
                    <input id="postalCodeTextBox" type="text" class="enterastab" title="Código Postal" data-content="Favor inserir o código postal do endereço." />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="streetTextBox"><strong>Endereço:</strong></label>
                <div class="controls" style="white-space: nowrap;">
                    <input id="streetTextBox" type="text" class="enterastab input-medium" title="Endereço" data-content="Favor inserir seu endereço" />
                    <input id="unitTextBox" type="text" class="enterastab input-mini" placeholder="Nº" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="complementTextBox"><strong>Complemento:</strong></label>
                <div class="controls">
                    <input id="complementTextBox" type="text" class="enterastab" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="districtTextBox"><strong>Bairro:</strong></label>
                <div class="controls">
                    <input id="districtTextBox" type="text" class="enterastab" title="Bairro" data-content="Favor inserir o bairro ou distrito" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="cityTextBox"><strong>Cidade:</strong></label>
                <div class="controls">
                    <input id="cityTextBox" type="text" class="enterastab" title="Cidade" data-content="Favor inserir o nome da cidade referente ao endereço" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="ddlRegions"><strong>Estado:</strong></label>
                <div class="controls">
                    <div data-bind="visible: hasRegions()">
                        <input id="ddlRegions" class="enterastab" data-bind="kendoDropDownList: { data: regions, value: selectedRegion, dataTextField: 'RegionName', dataValueField: 'RegionCode', cascadeFrom: 'ddlCountries' }" />
                    </div>
                    <div data-bind="visible: !hasRegions()">
                        <input id="regionTextBox" type="text" data-bind="value: selectedRegion" title="Estado" data-content="Precisamos de saber o estado" />
                    </div>
                </div>
            </div>
            <div class="control-group hidden">
                <label class="control-label" for="ddlCountries"><strong>País:</strong></label>
                <div class="controls">
                    <input id="ddlCountries" data-bind="kendoDropDownList: { data: countries, value: selectedCountry, dataTextField: 'CountryName', dataValueField: 'CountryCode' }" />
                </div>
            </div>
        </div>
    </div>
    <div class="clearfix">
    </div>
    <div class="form-horizontal">
        <div class="control-group">
            <label class="control-label required" for="commentsTextArea"><strong>Mensagem:</strong></label>
            <div class="controls">
                <textarea id="commentsTextArea" class="markdown-editor" required data-role="validate" placeholder="Sua mensagem..." title="Mensagem" data-content="Favor nos dizer a razão de seu contato."></textarea>
            </div>
        </div>
    </div>
    <div id="divFiles" class="form-horizontal" data-bind="visible: _userID > 0">
        <div class="control-group">
            <label class="control-label" for="listView"><strong>Catálagos Disponíveis:</strong></label>
            <div class="controls">
                <div id="listView"></div>
                <script type="text/x-kendo-template" id="tmplCatalogs">
                    <div class="catalogs">
                        <div class="well">
                            <label id="label_${ DocId }" class="checkbox">
                                <input id="${ DocId }" type="checkbox" />
                                <span class="title">${ DocName }</span>
                                <div class="tooltipcontent hidden">
                                    ${ DocDesc }
                                </div>
                            </label>
                        </div>
                    </div>
                </script>
            </div>
        </div>
    </div>
    <ul class="inline buttons">
        <li>
            <button id="btnSubmit" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." data-role="trigger-validation">Enviar</button>
        </li>
        <li>
            <button id="btnCancel" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Cancelar</button>
        </li>
        <li>
            <button id="btnConfig" class="btn btn-small btn-info" data-bind="visible: _authorized > 2" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Configurações</button>
        </li>
    </ul>
</div>