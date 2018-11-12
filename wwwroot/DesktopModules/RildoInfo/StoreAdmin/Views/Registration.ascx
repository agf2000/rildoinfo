<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Registration.ascx.vb" Inherits="RIW.Modules.StoreAdmin.Views.Registration" %>
<div id="registration" class="container-fluid Normal">
    <div id="checkboxes" class="span6">
        <div class="form-horizontal">
            <div class="control-group">
                <label class="control-label">
                    <strong>Empresa:</strong>
                </label>
                <div class="controls">
                    <label class="checkbox inline">
                        <input id="companyShow" type="checkbox" />
                        Requisitar
                    </label>
                    <label class="checkbox inline">
                        <input id="companyReq" type="checkbox" />
                        Obrigatório
                    </label>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label">
                    <strong>Sobrenome:</strong>
                </label>
                <div class="controls">
                    <label class="checkbox inline">
                        <input id="lastNameShow" type="checkbox" />
                        Requisitar
                    </label>
                    <label class="checkbox inline">
                        <input id="lastNameReq" type="checkbox" />
                        Obrigatório
                    </label>
                </div>
            </div>
            <div class="control-group hidden">
                <label class="control-label">
                    <strong>Ramo de Atividade:</strong>
                </label>
                <div class="controls">
                    <label class="checkbox inline">
                        <input id="industryShow" type="checkbox" />
                        Requisitar
                    </label>
                    <label class="checkbox inline">
                        <input id="industryReq" type="checkbox" />
                        Obrigatório
                    </label>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label">
                    <strong>Telefone:</strong>
                </label>
                <div class="controls">
                    <label class="checkbox inline">
                        <input id="phoneShow" type="checkbox" />
                        Requisitar
                    </label>
                    <label class="checkbox inline">
                        <input id="phoneReq" type="checkbox" />
                        Obrigatório
                    </label>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label">
                    <strong>CNPJ:</strong>
                </label>
                <div class="controls">
                    <label class="checkbox inline">
                        <input id="einShow" type="checkbox" />
                        Requisitar
                    </label>
                    <label class="checkbox inline">
                        <input id="einReq" type="checkbox" />
                        Obrigatório
                    </label>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label">
                    <strong>Insc. Estadual:</strong>
                </label>
                <div class="controls">
                    <label class="checkbox inline">
                        <input id="stateTaxShow" type="checkbox" />
                        Requisitar
                    </label>
                    <label class="checkbox inline">
                        <input id="stateTaxReq" type="checkbox" />
                        Obrigatório
                    </label>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label">
                    <strong>Insc. Minicipal:</strong>
                </label>
                <div class="controls">
                    <label class="checkbox inline">
                        <input id="cityTaxShow" type="checkbox" />
                        Requisitar
                    </label>
                    <label class="checkbox inline">
                        <input id="cityTaxReq" type="checkbox" />
                        Obrigatório
                    </label>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label">
                    <strong>CPF:</strong>
                </label>
                <div class="controls">
                    <label class="checkbox inline">
                        <input id="ssnShow" type="checkbox" />
                        Requisitar
                    </label>
                    <label class="checkbox inline">
                        <input id="ssnReq" type="checkbox" />
                        Obrigatório
                    </label>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label">
                    <strong>Identidade:</strong>
                </label>
                <div class="controls">
                    <label class="checkbox inline">
                        <input id="identShow" type="checkbox" />
                        Requisitar
                    </label>
                    <label class="checkbox inline">
                        <input id="identReq" type="checkbox" />
                        Obrigatório
                    </label>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label">
                    <strong>Website:</strong>
                </label>
                <div class="controls">
                    <label class="checkbox inline">
                        <input id="websiteShow" type="checkbox" />
                        Requisitar
                    </label>
                    <label class="checkbox inline">
                        <input id="websiteReq" type="checkbox" />
                        Obrigatório
                    </label>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label">
                    <strong>Endereço:</strong>
                </label>
                <div class="controls">
                    <label class="checkbox inline">
                        <input id="addressShow" type="checkbox" />
                        Requisitar
                    </label>
                    <label class="checkbox inline">
                        <input id="addressReq" type="checkbox" />
                        Obrigatório
                    </label>
                </div>
            </div>
        </div>
    </div>
    <div class="span6">
        <div id="groupsGrid"></div>
        <script id="tmplGroupsToolbar" type="text/x-kendo-template">
            <ul>
                <li>
                    <button id="btnAddNewService" class="btn btn-small btn-inverse k-grid-add"><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                </li>
            </ul>
        </script>
        <div id="industriesGrid" class="hidden"></div>
        <script id="tmplToolbar" type="text/x-kendo-template">
            <ul id="ulToolbar">
                <li>
                    <button onclick="my.addNewIndustry(); return false;" class="btn btn-small btn-inverse"><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                </li>
            </ul>
        </script>
    </div>
    <div class="clearfix"></div>
    <div id="newIndustry">
        <div class="form-horizontal">
            <div class="control-group">
                <label class="control-label" for="industryTitleTextBox">
                    <strong>Título:</strong>
                </label>
                <div class="controls">
                    <input id="industryTitleTextBox" type="text" class="input-medium" />
                </div>
            </div>
            <div class="form-actions">
                <button id="btnAddNewIndustry" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Adicionar</button>
                <button id="btnReturn" class="btn btn-small"><i class="fa fa-chevron-left"></i>&nbsp; Retornar</button>
            </div>
        </div>
    </div>
</div>
