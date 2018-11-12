<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="StoreContacts.ascx.vb" Inherits="RIW.Modules.StoreAdmin.Views.StoreContacts" %>
<div id="storeContacts" class="container-fluid Normal">
    <div class="form-horizontal">
        <div class="span6">
            <div class="control-group">
                <label class="control-label" for="postalCodeTextBox"><strong>Código Postal:</strong></label>
                <div class="controls">
                    <input id="postalCodeTextBox" type="text" class="enterastab" data-bind="value: postalCode" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="streetTextBox"><strong>Endereço:</strong></label>
                <div class="controls" style="white-space: nowrap;">
                    <input id="streetTextBox" type="text" class="enterastab input-medium" data-bind="value: street" />
                    <input id="unitTextBox" type="text" class="enterastab input-mini" data-bind="value: unit" placeholder="Nº" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="complementTextBox"><strong>Complemento:</strong></label>
                <div class="controls">
                    <input id="complementTextBox" type="text" class="enterastab" data-bind="value: complement" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="districtTextBox"><strong>Bairro:</strong></label>
                <div class="controls">
                    <input id="districtTextBox" type="text" class="enterastab" data-bind="value: district" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="cityTextBox"><strong>Cidade:</strong></label>
                <div class="controls">
                    <input id="cityTextBox" type="text" class="enterastab" data-bind="value: city" />
                </div>
            </div>
        </div>
        <div class="span6">
            <div class="control-group">
                <label class="control-label" for="ddlCountries"><strong>País:</strong></label>
                <div class="controls">
                    <input id="ddlCountries" data-bind="kendoDropDownList: { data: countries, value: selectedCountry, dataTextField: 'CountryName', dataValueField: 'CountryCode' }" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="ddlRegions"><strong>Estado:</strong></label>
                <div class="controls">
                    <div data-bind="visible: hasRegions()">
                        <input id="ddlRegions" class="enterastab" data-bind="kendoDropDownList: { data: regions, value: selectedRegion, dataTextField: 'RegionName', dataValueField: 'RegionCode' }" />
                    </div>
                    <div data-bind="visible: !hasRegions()">
                        <input id="regionTextBox" type="text" data-bind="value: selectedRegion" />
                    </div>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="emailTextBox"><strong>Email:</strong></label>
                <div class="controls">
                    <input id="emailTextBox" class="enterastab" type="email" data-bind="value: email" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="replyEmailTextBox"><strong>Email de Resposta:</strong></label>
                <div class="controls">
                    <input id="replyEmailTextBox" class="enterastab" type="email" data-bind="value: replyEmail" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="txtPhone1"><strong>Telefone 1:</strong></label>
                <div class="controls">
                    <input id="txtPhone1" type="text" class="enterastab" data-bind="value: phone1" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="txtPhone2"><strong>Telefone 2:</strong></label>
                <div class="controls">
                    <input id="txtPhone2" type="text" class="enterastab" data-bind="value: phone2" />
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="form-actions">
			<button id="btnUpdateContacts" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Atualizar</button>
        </div>
    </div>
</div>
