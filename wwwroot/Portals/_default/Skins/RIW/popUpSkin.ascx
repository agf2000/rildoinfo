<%@ Control Language="C#" CodeBehind="~/DesktopModules/Skins/skin.cs" AutoEventWireup="false" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="dnn" TagName="JQUERY" Src="~/Admin/Skins/jQuery.ascx" %>
<dnn:JQUERY ID="dnnjQuery" runat="server" />

<%--<dnn:DnnJsInclude runat="server" FilePath="js/bootstrap.min.js" PathNameAlias="SkinPath" />--%>
<dnn:DnnJsInclude runat="server" FilePath="/desktopmodules/rildoinfo/WebAPI/Scripts/Bootstrap/bootstrap.min.js" />
<dnn:DnnJsInclude runat="server" FilePath="~/Resources/Shared/Scripts/slides.min.jquery.js" />
<dnn:DnnCssInclude runat="server" FilePath="/desktopmodules/rildoinfo/WebAPI/content/kendo/2015.3.930/styles/kendo.blueopal.min.css" />
<%--<dnn:DnnCssInclude ID="bootstrapCSS" runat="server" FilePath="css/bootstrap.min.css" PathNameAlias="SkinPath" Priority="15" />--%>
<dnn:DnnCssInclude runat="server" FilePath="css/style.default.css" PathNameAlias="SkinPath" />
<%--<dnn:DnnCssInclude runat="server" FilePath="/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap.min.css" Priority="15" />--%>

<script>
    $(function () {
        $('#slides').slides({
            preload: true,
            preloadImage: '<%= Page.ResolveClientUrl("~/images/loading.gif") %>',
            play: 0,
            next: 'next',
            prev: 'prev',
            pagination: true,
            generatePagination: false,
            hoverPause: true
        });
    });
</script>

<div id="ContentPane" runat="server" />

<div id="sysVersion">
    <div class="font-size-small">
        Atualiza&#231;&#227;o do Sistema:
    </div>
    <div class="font-size-small">
        Frontend: <span id="frontEndVersion"></span>
    </div>
    <div class="font-size-small">
        JS: <span id="jsVersion"></span>
    </div>
</div>
<script>
    $(function () {
        if (document.URL.toLowerCase().indexOf('login') === -1) {
            if ($('#dnn_dnnUser_enhancedRegisterLink').text().indexOf('Superuser') !== -1) {
                $('#sysVersion').show();
            }
        }

        $('#frontEndVersion').text(ascxVersion);
        $('#jsVersion').text(jsVersion);
    });
</script>