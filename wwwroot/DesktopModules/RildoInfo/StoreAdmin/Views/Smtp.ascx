<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Smtp.ascx.vb" Inherits="RIW.Modules.StoreAdmin.Views.Smtp" %>
<div id="smtp" class="container-fluid Normal">
    <div class="form-horizontal">
        <div class="control-group">
            <label class="control-label" for="servidorTextBox">
                <strong>Servidor:</strong>
                <i class="icon icon-info-sign" title="Servidor" data-content="O endereço do servidor de smtp."></i>
            </label>
            <div class="controls">
                <input id="servidorTextBox" type="text" class="enterastab input-xlarge" data-bind="value: server" required placeholder="Servidor" data-role="validate" title="Servidor..." data-content="Favor inserir um nome do servidor smtp" />
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="portTextBox">
                <strong>Porta:</strong>
                <i class="icon icon-info-sign" title="Porta" data-content="O número da porta que o servidor de smtp está configurado."></i>
            </label>
            <div class="controls">
                <input id="portTextBox" type="text" class="enterastab input-xlarge" data-bind="value: port" required placeholder="Número da Porta" data-role="validate" title="Porta..." data-content="Favor inserir o número da porta" />
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="loginTextBox">
                <strong>Login:</strong>
                <i class="icon icon-info-sign" title="Login" data-content="O login autorizado a usar o serviço de smtp."></i>
            </label>
            <div class="controls">
                <input id="loginTextBox" type="text" class="enterastab input-xlarge" data-bind="value: login" required placeholder="Login" data-role="validate" title="Login..." data-content="Favor inserir o login do smtp" />
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="passwordTextBox">
                <strong>Senha:</strong>
                <i class="icon icon-info-sign" title="Senha" data-content="A senha usada para autenticar o login."></i>
            </label>
            <div class="controls">
                <input id="passwordTextBox" type="text" class="enterastab input-xlarge" data-bind="value: password" required placeholder="Senha" data-role="validate" title="Senha..." data-content="Favor inserir a senha do smtp" />
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="connectionCheckBox">
                <strong>Conecção Segura:</strong>
                <i class="icon icon-info-sign" title="Conecção Segura" data-content="Sim ou não, o servidor de smtp exige uma coneção segura (ssl)."></i>
            </label>
            <div class="controls">
                <input id="connectionCheckBox" type="checkbox" class="normalCheckBox switch-small" data-on-label="SIM" data-off-label="NÃO" data-bind="attr: { 'checked': connection }" />
            </div>
        </div>
        <div class="form-actions">
			<button id="btnUpdateSmtp" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Atualizar</button>
        </div>
    </div>
</div>