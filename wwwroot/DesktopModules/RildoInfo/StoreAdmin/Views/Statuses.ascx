<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Statuses.ascx.vb" Inherits="RIW.Modules.StoreAdmin.Views.Statuses" %>
<div id="statuses" class="container-fluid Normal">
    <div id="statusesGrid">
    </div>
    <script id="tmplToolbar" type="text/x-kendo-template">
        <ul id="ulToolbar">
            <li>
                <button onclick="my.addNewStatus(); return false;" class="btn btn-small btn-inverse"><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
            </li>
        </ul>
    </script>
    <div id="newStatus">
        <div class="form-horizontal">
            <div class="control-group">
                <label class="control-label" for="statusTitleTextBox">
                    <strong>Título:</strong>
                </label>
                <div class="controls">
                    <input id="statusTitleTextBox" type="text" class="input-medium" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="statusColorPallete">
                    <strong>Cor:</strong>
                </label>
                <div class="controls">
                    <input id="statusColorPallete" class="input-small" value="#FFFFFF" />
                </div>
            </div>
            <div class="form-actions">
			    <button id="btnAddNewStatus" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Adicionar</button>
                <button id="btnReturn" class="btn btn-small"><i class="fa fa-chevron-left"></i>&nbsp; Retornar</button>
            </div>
        </div>
    </div>
</div>
