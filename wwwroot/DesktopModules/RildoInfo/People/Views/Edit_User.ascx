<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Edit_User.ascx.vb" Inherits="RIW.Modules.People.Views.EditUser" %>
<div id="editUser" class="row-fluid">
    <div class="span12">
        <div class="pull-left">
            <div id="personMenu">
                <ul>
                    <li id="1">Cadastro</li>
                    <li id="2">Login</li>
                    <li id="3">Avatar (foto)</li>
                    <li id="4">Grupos</li>
                </ul>
            </div>
        </div>
        <div class="pull-right">
            <div id="personForm">
                <h3 id="personTitle"></h3>
                <div class="form-horizontal">
                    <div class="pull-left">
                        <div class="control-group" id="liEmail">
                            <label class="control-label required" for="btnCheckEmail"><strong>Email:</strong></label>
                            <div class="controls" style="white-space: nowrap;">
                                <input id="emailTextBox" type="email" class="enterastab" data-bind="value: email" title="Email" data-content="Favor inserir o seu endereço de email ou um nome unique." data-role="validate" required />
                                <button id="btnCheckEmail" class="btn btn-small" title="Verificar" data-loading-text="<i class='fa fa-spinner fa-spin'></i>"><i class="fa fa-check"></i></button>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label required" for="firstNameTextBox"><strong>Nome:</strong></label>
                            <div class="controls">
                                <input id="firstNameTextBox" type="text" class="enterastab" data-bind="value: firstName" title="Nome" data-content="Favor inserir o nome." data-role="validate" required />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label required" for="lastNameTextBox"><strong>Sobrenome:</strong></label>
                            <div class="controls">
                                <input id="lastNameTextBox" type="text" class="enterastab" data-bind="value: lastName" title="Sobrenome" data-content="Favor inserir o sobrenome." data-role="validate" required />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="phoneTextBox"><strong>Telefone:</strong></label>
                            <div class="controls">
                                <input id="phoneTextBox" type="text" class="enterastab" data-bind="value: telephone" title="Telefone" data-content="Favor inserir o telefone para contato." />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="cellTextBox"><strong>Celular:</strong></label>
                            <div class="controls">
                                <input id="cellTextBox" type="text" class="enterastab" data-bind="value: cell" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="faxTextBox"><strong>Fax:</strong></label>
                            <div class="controls">
                                <input id="faxTextBox" type="text" class="enterastab" data-bind="value: fax" />
                            </div>
                        </div>
                        <div class="control-group" id="liComments">
                            <label class="control-label" for="commentsTextArea"><strong>Observações:</strong></label>
                            <div class="controls">
                                <textarea id="commentsTextArea" data-bind="value: comments"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="pull-right">
                        <div class="control-group" id="liGroups" data-bind="visible: my.userId === 0">
                            <label class="control-label required"><strong>Grupos:</strong></label>
                            <div class="controls" style="margin: 0; padding: 0;">
                                <input data-bind="kendoMultiSelect: { data: groups, dataTextField: 'RoleName', dataValueField: 'RoleId', value: selectedGroup }" />
                            </div>
                        </div>
                        <div data-bind="visible: askAddress() && personId() === 0">
                            <div class="control-group">
                                <label class="control-label" for="postalCodeTextBox"><strong>Código Postal:</strong></label>
                                <div class="controls">
                                    <input id="postalCodeTextBox" type="text" class="enterastab" data-bind="value: postalCode" title="CEP" data-content="Favor inserir o CEP referente ao endereço." />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label" for="streetTextBox"><strong>Endereço:</strong></label>
                                <div class="controls" style="white-space: nowrap;">
                                    <input id="streetTextBox" type="text" class="enterastab input-medium" data-bind="value: street" title="Endereço" data-content="Favor inserir o nome da rua, avenida..." />
                                    <input id="unitTextBox" type="text" class="enterastab input-mini" placeholder="Nº" data-bind="value: unit" />
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
                                    <input id="districtTextBox" type="text" class="enterastab" data-bind="value: district" title="Bairro" data-content="Favor inserir o nome do bairro ou distrito referente ao endereço." />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label" for="cityTextBox"><strong>Cidade:</strong></label>
                                <div class="controls">
                                    <input id="cityTextBox" type="text" class="enterastab" data-bind="value: city" title="Cidade" data-content="Favor inserir o nome da cidade referente ao endereço." />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label" for="ddlRegions"><strong>Estado:</strong></label>
                                <div class="controls">
                                    <div data-bind="visible: hasRegions()">
                                        <input id="ddlRegions" class="enterastab" data-bind="kendoDropDownList: { data: regions, value: selectedRegion, dataTextField: 'RegionName', dataValueField: 'RegionCode', cascadeFrom: 'ddlCountries' }" />
                                    </div>
                                    <div data-bind="visible: !hasRegions()">
                                        <input id="regionTextBox" type="text" data-bind="value: selectedRegion" title="Estado" data-content="Favor inserir o nome do estado ou região referente ao endereço." />
                                    </div>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label" for="ddlCountries"><strong>País:</strong></label>
                                <div class="controls">
                                    <input id="ddlCountries" data-bind="kendoDropDownList: { data: countries, value: selectedCountry, dataTextField: 'CountryName', dataValueField: 'CountryCode' }" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix">
                    </div>
                    <div class="form-actions">
                        <button class="btn btn-small btnReturn"><i class="fa fa-chevron-left"></i>&nbsp; Fechar</button>
                        <button id="btnUpdateUser" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                        <button id="btnDeleteUser" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon icon-ban-circle"></i>&nbsp; Desativar</button>
                        <button id="btnRemoveUser" class="btn btn-small btn-danger" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
                        <button id="btnRestoreUser" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon icon-refresh"></i>&nbsp; Restaurar</button>
                    </div>
                </div>
            </div>
            <div id="editLogin">
                <div class="accordion">
                    <div class="accordion-group">
                        <div class="accordion-heading">
                            <a class="accordion-toggle" data-toggle="collapse" href="#collapseLogin">
                                <h4>Login de Acesso</h4>
                            </a>
                        </div>
                        <div id="collapseLogin" class="accordion-body collapse in">
                            <div class="accordion-inner">
                                <div class="form-horizontal">
                                    <div class="control-group">
                                        <label class="control-label required" for="loginTextBox"><strong>Login:</strong></label>
                                        <div class="controls" style="white-space: nowrap;">
                                            <input id="loginTextBox" type="text" class="enterastab input-small" />
                                        </div>
                                    </div>
                                    <div class="form-actions">
                                        <button id="btnCheckLogin" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Verificar</button>
                                        <button id="btnUpdateLogin" class="btn btn-small" disabled="disabled" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Alterar Login</button>
                                        <span id="userNameCheck"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="accordion-group">
                        <div class="accordion-heading">
                            <a class="accordion-toggle" data-toggle="collapse" href="#collapsePassword">
                                <div class="pull-left">
                                    <h4>Alterar Senha</h4>
                                </div>
                                <div class="pull-right">
                                    <b>Senha Alterada em:</b> <span id="lastPasswordChangedDate"></span>
                                </div>
                                <div class="clearfix"></div>
                            </a>
                        </div>
                        <div id="collapsePassword" class="accordion-body collapse in">
                            <div class="accordion-inner">
                                <div class="form-horizontal">
                                    <div class="dnnFormItem Hidden">
                                        <label class="control-label required" for="currentPasswordTextBox">
                                            <i class="icon icon-exclamation-sign" title="Senha Atual" data-content="Senha deve ser de no mínimo 7 caracteres. Não deve conter espaços ou letras com acento."></i>
                                            <strong>Senha Atual:</strong>
                                        </label>
                                        <div class="controls">
                                            <input id="currentPasswordTextBox" type="password" class="enterastab input-medium" title="Senha Atual" data-content="É necessário inserir sua senha atual." />
                                            <span id="currentPass"></span>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label required" for="newPasswordTextBox">
                                            <i class="icon icon-exclamation-sign" title="Senha" data-content="Senha deve ser de no mínimo 7 caracteres. Não deve conter espaços ou letras com acento"></i>
                                            <strong>Senha:</strong>
                                        </label>
                                        <div class="controls" style="white-space: nowrap;">
                                            <input id="newPasswordTextBox" type="password" class="enterastab input-medium" required="required" data-role="validate" title="Senha" data-content="Senha deve ser de no mínimo 7 caracteres. Não deve conter espaços ou letras com acento." />
                                            <span id="passMetter"></span>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label required" for="confirmPasswordTextBox">
                                            <i class="icon icon-exclamation-sign" title="Confirmar Senha" data-content="Favor confirmar sua senha para evitar erro de digitação."></i>
                                            <strong>Repetir Senha:</strong>
                                        </label>
                                        <div class="controls" style="white-space: nowrap;">
                                            <input id="confirmPasswordTextBox" type="password" class="enterastab input-medium" required="required" data-role="validate" title="Confirmar Senha" data-content="Favor confirmar sua senha para evitar erro de digitação." />
                                            <span id="passConfirm"></span>
                                        </div>
                                    </div>
                                    <div class="form-actions">
                                        <button id="btnUpdatePassword" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Alterar Senha</button>
                                        <button id="btnRandomPassword" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Gerar Senha Aleatória</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="userPhoto">
                <div class="accordion">
                    <div class="accordion-group">
                        <div class="accordion-heading">
                            <a class="accordion-toggle" data-toggle="collapse" href="#collapseAvatar">
                                <h4>Foto de perfil</h4>
                            </a>
                        </div>
                        <div id="collapseAvatar" class="accordion-body collapse in">
                            <div class="accordion-inner">
                                <div class="form-horizontal">
                                    <div class="control-group">
                                        <label class="control-label" for="photos"><strong>Foto:</strong></label>
                                        <div class="controls">
                                            <input id="photos" name="photos" type="file" />
                                            <a id="aImg"></a>
                                            <div class="padded">
                                                <button id="btnRemoveAvatar" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Remover</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="userGroups">
                <ul class="inline">
                    <li>
                        <label><strong>Grupos Disponíveis:</strong></label>
                        <input id="kddlRoles" class="input-medium" />
                    </li>
                    <li>
                        <label><strong>Data Efetiva:</strong></label>
                        <input id="dpStartDate" placeholder="Opcional..." />
                    </li>
                    <li>
                        <label><strong>Data de Validade:</strong></label>
                        <input id="dpEndDate" placeholder="Opcional..." />
                    </li>
                    <li>
                        <button id="btnAddUserGroup" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus"></i>&nbsp; Adicionar</button>
                    </li>
                </ul>
                <div class="clearfix"></div>
                <div id="userGroupsGrid"></div>
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="status NormalRed"></div>
    </div>
</div>
