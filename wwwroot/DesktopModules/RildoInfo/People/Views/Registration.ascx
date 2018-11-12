<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Registration.ascx.vb" Inherits="RIW.Modules.People.Views.Registration" %>
<div id="peopleRegistration" class="row-fluid">
    <div class="span12">
        <div id="personForm">
            <h3 id="personTitle"></h3>
            <div class="pull-left">
                <div class="form-horizontal">
                    <div class="control-group" id="liPersonType" data-bind="visible: askCompany()">
                        <label class="control-label" for="radioPerson"><strong>Pessoa:</strong></label>
                        <div class="controls">
                            <label class="radio inline">
                                <input type="radio" value="1" id="radioPerson" name="Person" data-bind="checked: personType" />
                                Física
                            </label>
                            <label class="radio inline">
                                <input type="radio" value="0" id="radioBusiness" name="Person" data-bind="checked: personType" />
                                Jurídica
                            </label>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label required" for="emailTextBox">
                            <strong>Email:</strong>
                            <i class="icon icon-exclamation-sign" title="Email" data-content="Email é de preenchimento obrigatório. Use seu email para logar em nosso website."></i>
                        </label>
                        <div class="controls" style="white-space: nowrap;">
                            <input id="emailTextBox" class="enterastab" type="email" data-bind="value: email" data-role="validate" title="Email" data-content="Email é de preenchimento obrigatório. Email é usado para se autenticar no website." required />
                            <i id="emailResult"></i>
                        </div>
                    </div>
                    <div class="control-group" id="liCompanyName">
                        <label class="control-label" for="companyTextBox"><strong>Razão Social:</strong></label>
                        <div class="controls">
                            <input id="companyTextBox" type="text" class="enterastab" data-bind="value: companyName" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label required" for="firstNameTextBox"><strong>Nome:</strong></label>
                        <div class="controls">
                            <input id="firstNameTextBox" type="text" class="enterastab" data-bind="value: firstName" data-role="validate" title="Nome" data-content="Favor inserir seu nome." required />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label" for="lastNameTextBox"><strong>Sobrenome:</strong></label>
                        <div class="controls">
                            <input id="lastNameTextBox" type="text" class="enterastab" data-bind="value: lastName" title="Sobrenome" data-content="Favor inserir seu sobrenome." />
                        </div>
                    </div>
                    <div class="control-group" id="liPhone">
                        <label class="control-label" for="phoneTextBox"><strong>Telefone:</strong></label>
                        <div class="controls">
                            <input id="phoneTextBox" type="text" class="enterastab" data-bind="value: telephone" title="Telefone" data-content="Favor inserir o número de telefone de contato." />
                        </div>
                    </div>
                    <div class="control-group" id="liCell">
                        <label class="control-label" for="cellTextBox"><strong>Celular:</strong></label>
                        <div class="controls">
                            <input id="cellTextBox" type="text" class="enterastab" data-bind="value: cell" />
                        </div>
                    </div>
                    <div class="control-group" id="liFax">
                        <label class="control-label" for="faxTextBox"><strong>Fax:</strong></label>
                        <div class="controls">
                            <input id="faxTextBox" type="text" class="enterastab" data-bind="value: fax" />
                        </div>
                    </div>
                    <div class="control-group" id="liZero0800">
                        <label class="control-label" for="0800TextBox"><strong>Tel. 0800:</strong></label>
                        <div class="controls">
                            <input id="0800TextBox" type="text" class="enterastab" data-bind="value: zero800" />
                        </div>
                    </div>
                    <div class="control-group" id="liCPF">
                        <label class="control-label" for="cpfTextBox"><strong>CPF:</strong></label>
                        <div class="controls">
                            <input id="cpfTextBox" type="text" class="enterastab" data-bind="value: cpf" title="CPF" data-content="Favor inserir o seu CPF" />
                        </div>
                    </div>
                    <div class="control-group" id="liIdent">
                        <label class="control-label" for="identTextBox"><strong>RG:</strong></label>
                        <div class="controls">
                            <input id="identTextBox" type="text" class="enterastab" data-bind="value: ident" title="Identidade" data-content="Favor inserir o seu RG" />
                        </div>
                    </div>
                    <div class="control-group" id="liWebsite">
                        <label class="control-label" for="websiteTextBox"><strong>Website:</strong></label>
                        <div class="controls">
                            <input id="websiteTextBox" type="text" class="enterastab" data-bind="value: website" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label required" for="passwordTextBox">
                            <strong>Senha:</strong>
                            <i class="icon icon-exclamation-sign" title="Senha" data-content="É necessário inserir uma senha para acesso futuro. A Senha deve ser de no mínimo 7 caracteres. Não deve conter espaços ou letras com acento."></i>
                        </label>
                        <div class="controls" style="white-space: nowrap;">
                            <input id="passwordTextBox" type="password" class="enterastab" required="required" data-role="validate" title="Senha" data-content="É necessário inserir uma senha para acesso futuro. A Senha deve ser de no mínimo 7 caracteres. Não deve conter espaços ou letras com acento." />
                            <span id="passMetter"></span>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label required" for="confirmPasswordTextBox">
                            <strong>Repetir Senha:</strong>
                            <i class="icon icon-exclamation-sign" title="Confirmar Senha" data-content="Favor confirmar sua senha para evitar erro de digitação."></i>
                        </label>
                        <div class="controls" style="white-space: nowrap;">
                            <input id="confirmPasswordTextBox" type="password" class="enterastab" required="required" data-role="validate" title="Confirmar Senha" data-content="Favor confirmar sua senha para evitar erro de digitação." />
                            <span id="passConfirm"></span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="pull-right">
                <div class="form-horizontal">
                    <div id="registerTypes" class="control-group hidden">
                        <label class="control-label" for="selectRegisterTypes"><strong>Registro:</strong></label>
                        <div class="controls" style="margin: 0; padding: 0;">
                            <ul class="inline" style="height: 0;">
                                <li>
                                    <label class="checkbox">
                                        <input id="rTypeClient" name="rTypes" type="checkbox" value="1" data-bind="checked: registerTypes" />
                                        Cliente
                                    </label>
                                </li>
                                <li>
                                    <label class="checkbox">
                                        <input id="rTypeProvider" name="rTypes" type="checkbox" value="2" data-bind="checked: registerTypes" />
                                        Fornecedor
                                    </label>
                                </li>
                                <li>
                                    <label class="checkbox">
                                        <input id="rTypeTransport" name="rTypes" type="checkbox" value="3" data-bind="checked: registerTypes" />
                                        Transportadora
                                    </label>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="control-group" id="liEIN">
                        <label class="control-label" for="einTextBox"><strong>CNPJ:</strong></label>
                        <div class="controls">
                            <input id="einTextBox" type="text" class="enterastab" data-bind="value: ein" />
                        </div>
                    </div>
                    <div class="control-group" id="liST">
                        <label class="control-label" for="stateTaxTextBox"><strong>Insc. Est.:</strong></label>
                        <div class="controls">
                            <input id="stateTaxTextBox" type="text" class="enterastab" data-bind="value: stateTax" />
                        </div>
                    </div>
                    <div class="control-group" id="liCT">
                        <label class="control-label" for="cityTaxTextBox"><strong>Insc. Mun.:</strong></label>
                        <div class="controls">
                            <input id="cityTaxTextBox" type="text" class="enterastab" data-bind="value: cityTax" />
                        </div>
                    </div>
                    <div id="liAddress" data-bind="visible: askAddress() && false">
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
                                <input id="districtTextBox" type="text" class="enterastab" data-bind="value: district" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="cityTextBox"><strong>Cidade:</strong></label>
                            <div class="controls">
                                <input id="cityTextBox" type="text" class="enterastab" data-bind="value: city" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="ddlRegions"><strong>Estado:</strong></label>
                            <div class="controls">
                                <div data-bind="visible: hasRegions()">
                                    <input id="ddlRegions" class="enterastab" data-bind="kendoDropDownList: { data: regions, value: selectedRegion, dataTextField: 'Text', dataValueField: 'Value', cascadeFrom: 'ddlCountries' }" />
                                </div>
                                <div data-bind="visible: !hasRegions()">
                                    <input id="regionTextBox" type="text" data-bind="value: selectedRegion" />
                                </div>
                            </div>
                        </div>
                        <div class="control-group Hidden">
                            <label class="control-label" for="ddlCountries"><strong>País:</strong></label>
                            <div class="controls">
                                <input id="ddlCountries" data-bind="kendoDropDownList: { data: countries, value: selectedCountry, dataTextField: 'CountryName', dataValueField: 'CountryCode' }" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="clearfix">
            </div>
            <ul class="inline buttons">
                <li>
                    <button id="btnUpdatePerson" class="btn btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Cadastrar</button>
                </li>
            </ul>
        </div>
        <div class="status NormalRed"></div>
    </div>
</div>
