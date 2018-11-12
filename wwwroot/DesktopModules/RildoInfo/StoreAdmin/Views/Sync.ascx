<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Sync.ascx.vb" Inherits="RIW.Modules.StoreAdmin.Views.Sync" %>
<div id="sync" class="container-fluid Normal">
    <div class="form-horizontal">
        <div class="control-group">
            <label class="control-label" for="enableSync">
                <strong>Ativar Sincronizacao:</strong>
                <i class="icon icon-info-sign" title="Sincronizar com SGI" data-content="Ativar ou não, a sincronização com o banco de dados do SGI."></i>
            </label>
            <div class="controls">
                <input id="enableSync" type="checkbox" class="normalCheckBox switch-small" data-on-label="SIM" data-off-label="NÃO" />
            </div>
        </div>
    <div class="form-actions">
		<button id="btnUpdateSync" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Atualizar</button>
    </div>
</div>

