<%@ control language="vb" autoeventwireup="false" codebehind="DavReturns.ascx.vb" inherits="RIW.Modules.Store_Manager.Views.DavReturns" %>
<div id="davsAdmin" class="row-fluid">
    <div class="span12">
        <div>
            <div id="davsGrid"></div>
        </div>
        <div class="form-horizontal">
            <div class="control-group">
                <label class="control-label required" style="padding-top: 15px;" for="files"><strong>Arquivo:</strong></label>
                <div class="controls">
                    <input name="files" id="files" type="file" />
                </div>
            </div>
        </div>
    </div>
</div>
