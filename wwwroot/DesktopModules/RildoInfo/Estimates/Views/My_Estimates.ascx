<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="My_Estimates.ascx.vb" Inherits="RIW.Modules.Estimates.Views.MyEstimates" %>
<div id="divManageEstimates" class="row-fluid">
    <div class="span12">
        <input id="ask_permission" class="btn" type="button" value="Permitir Notificações" title="Permissões" data-content="Clique nesta opção para permitir o recebimento de notificações em tempo real enquanto estiver neste navegador.">
        <div id="estimatesGrid"></div>
        <script id="tmplToolbar" type="text/x-kendo-template">
            <ul id="ulToolbar">
                <li>
                    <h4>Orçamentos</h4>
                </li>
            </ul>
        </script>
    </div>
    <div id="estimateWindow"></div>
</div>
<div id="HTML5Audio">
    <input id="audiofile" type="text" value="" style="display: none;"/></div>
<audio id="myaudio">
    <script>
        function LegacyPlaySound(soundobj) {
            var thissound = document.getElementById(soundobj);
            thissound.Play();
        }
    </script>
    <span id="OldSound"></span>        
    <input type="button" value="Play Sound" onClick="LegacyPlaySound('LegacySound')">
</audio>