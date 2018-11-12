<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Contacts.ascx.vb" Inherits="Contacts" %>
<div class="container-fluid Normal">
    <div class="pull-left">
        <div class="form-horizontal">
            <div id="divMethod">
                <div class="control-group" data-bind="visible: _reqMethod">
                    <label class="control-label"><strong>Método Envio:</strong></label>
                    <div class="controls">
                        <label class="checkbox inline">
                            <input type="checkbox" value="1" name="method" data-bind="checked: methodType" />
                            <i class="fa fa-envelope"></i>
                            Email
                        </label>
                        <label class="checkbox inline">
                            <input type="checkbox" value="2" name="method" data-bind="checked: methodType" />
                            <i class="icon icon-correio"></i>
                            Correio
                        </label>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label required" for="companyTextBox"><strong>Empresa:</strong></label>
                    <div class="controls">
                        <input id="companyTextBox" type="text" class="enterastab" oninvalid="this.setCustomValidity('opção obrigatória')" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label required" for="displayNameTextBox"><strong>Seu Nome:</strong></label>
                    <div class="controls">
                        <input id="displayNameTextBox" type="text" class="enterastab" oninvalid="this.setCustomValidity('opção obrigatória')" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="phoneTextBox"><strong>Telefone:</strong></label>
                    <div class="controls">
                        <input id="phoneTextBox" type="text" class="enterastab" oninvalid="this.setCustomValidity('opção obrigatória')" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="btnCheckEmail"><strong>Email:</strong></label>
                    <div class="controls" style="white-space: nowrap;">
                        <input id="emailTextBox" class="enterastab" type="email" placeholder="ex. nome@yahoo.com" oninvalid="this.setCustomValidity('opção obrigatória')" data-email-msg=" " />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="websiteTextBox"><strong>Website:</strong></label>
                    <div class="controls">
                        <input id="websiteTextBox" type="text" class="enterastab" oninvalid="this.setCustomValidity('opção obrigatória')" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="commentsTextArea"><strong>Mensagem:</strong></label>
                    <div class="controls">
                        <textarea id="commentsTextArea"></textarea>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="pull-left">
        <div class="form-horizontal">
            <div class="control-group">
                <label class="control-label" for="postalCodeTextBox"><strong>Código Postal:</strong></label>
                <div class="controls">
                    <input id="postalCodeTextBox" type="text" class="enterastab" oninvalid="this.setCustomValidity('opção obrigatória')" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="streetTextBox"><strong>Endereço:</strong></label>
                <div class="controls" style="white-space: nowrap;">
                    <input id="streetTextBox" type="text" class="enterastab input-medium" oninvalid="this.setCustomValidity('opção obrigatória')" />
                    <input id="unitTextBox" type="text" class="enterastab input-mini" placeholder="Nº" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="complementTextBox"><strong>Complemento:</strong></label>
                <div class="controls">
                    <input id="complementTextBox" type="text" class="enterastab" oninvalid="this.setCustomValidity('opção obrigatória')" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="districtTextBox"><strong>Bairro:</strong></label>
                <div class="controls">
                    <input id="districtTextBox" type="text" class="enterastab" oninvalid="this.setCustomValidity('opção obrigatória')" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="cityTextBox"><strong>Cidade:</strong></label>
                <div class="controls">
                    <input id="cityTextBox" type="text" class="enterastab" oninvalid="this.setCustomValidity('opção obrigatória')" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="ddlRegions"><strong>Estado:</strong></label>
                <div class="controls">
                    <div data-bind="visible: hasRegions()">
                        <input id="ddlRegions" class="enterastab" data-bind="kendoDropDownList: { data: regions, value: selectedRegion, dataTextField: 'Text', dataValueField: 'Value', cascadeFrom: 'ddlCountries' }" />
                    </div>
                    <div data-bind="visible: !hasRegions()">
                        <input id="regionTextBox" type="text" data-bind="value: selectedRegion" oninvalid="this.setCustomValidity('opção obrigatória')" />
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
    <div id="divFiles" class="form-horizontal">
        <div class="control-group">
            <label class="control-label" for="postalCodeTextBox"><strong>Catálagos Disponíveis:</strong></label>
            <div class="controls">
                <div id="listView"></div>
            </div>
        </div>
    </div>
    <ul class="inline">
        <li>
            <button id="btnSubmit" class="btn btn-inverse">Enviar</button>
        </li>
        <li>
            <button id="btnCancel" class="btn">Cancelar</button>
        </li>
        <li>
            <button id="btnConfig" class="btn btn-info">Configurações</button>
        </li>
    </ul>
</div>
