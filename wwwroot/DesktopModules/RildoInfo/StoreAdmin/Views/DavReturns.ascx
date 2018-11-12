<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DavReturns.ascx.vb" Inherits="RIW.Modules.StoreAdmin.Views.DavReturns" %>
<div id="davsAdmin" class="row-fluid">
    <div id="davsGrid"></div>
    <script id="tmplToolbar" type="text/x-kendo-template">
        <ul id="ulToolbar" class="inline">
            <li>
                <button id="btnSync" class="btn btn-small btn-inverse"><i class="icon-refresh icon-white"></i> Sinconizar</button>
            </li>
            <li>
                <span id="SyncMsg"></span>
            </li>
        </ul>
    </script>
    <div class="form-horizontal hidden">
        <div class="control-group">
            <label class="control-label required" style="padding-top: 15px;" for="files"><strong>Arquivo:</strong></label>
            <div class="controls">
                <input name="files" id="files" type="file" />
            </div>
        </div>
    </div>
</div>
</div>
