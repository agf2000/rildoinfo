<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="LANGUAGE" Src="~/Admin/Skins/Language.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SEARCH" Src="~/Admin/Skins/Search.ascx" %>
<%@ Register TagPrefix="dnn" TagName="BREADCRUMB" Src="~/Admin/Skins/BreadCrumb.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="PRIVACY" Src="~/Admin/Skins/Privacy.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TERMS" Src="~/Admin/Skins/Terms.ascx" %>
<%@ Register TagPrefix="dnn" TagName="COPYRIGHT" Src="~/Admin/Skins/Copyright.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LINKTOMOBILE" Src="~/Admin/Skins/LinkToMobileSite.ascx" %>
<%@ Register TagPrefix="dnn" TagName="JQUERY" Src="~/Admin/Skins/jQuery.ascx" %>
<%@ Register TagPrefix="dnn" TagName="META" Src="~/Admin/Skins/Meta.ascx" %>
<%@ Register TagPrefix="dnn" TagName="NAV" Src="~/Admin/Skins/Nav.ascx" %>
<%@ Register TagPrefix="dnn" TagName="MENU" Src="~/DesktopModules/DDRMenu/Menu.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<dnn:META ID="META1" runat="server" Name="viewport" Content="width=device-width,initial-scale=1" />

<dnn:JQUERY ID="dnnjQuery" runat="server" jQueryHoverIntent="true" />

<dnn:DnnCssInclude runat="server" FilePath="/desktopmodules/rildoinfo/WebAPI/content/kendo/2015.3.930/styles/kendo.blueopal.min.css" />
<%--<dnn:DnnCssInclude runat="server" FilePath="bootstrap/css/bootstrap.min.css" PathNameAlias="SkinPath" />--%>
<%--<dnn:DnnCssInclude runat="server" FilePath="bootstrap/css/bootstrap-responsive.min.css" PathNameAlias="SkinPath" />--%>
<%--<dnn:DnnCssInclude runat="server" FilePath="css/animate.min.css" PathNameAlias="SkinPath" />--%>
<%--<dnn:DnnCssInclude runat="server" FilePath="css/animate.delay.css" PathNameAlias="SkinPath" />--%>
<%--<dnn:DnnCssInclude runat="server" FilePath="css/isotope.css" PathNameAlias="SkinPath" />--%>
<%--<dnn:DnnCssInclude runat="server" FilePath="css/uniform.tp.css" PathNameAlias="SkinPath" />--%>
<%--<dnn:DnnCssInclude runat="server" FilePath="css/jquery.jgrowl.css" PathNameAlias="SkinPath" />--%>
<%--<dnn:DnnCssInclude runat="server" FilePath="css/jquery.alerts.css" PathNameAlias="SkinPath" />--%>
<%--<dnn:DnnCssInclude runat="server" FilePath="css/roboto.css" PathNameAlias="SkinPath" />--%>
<%--<dnn:DnnCssInclude runat="server" FilePath="css/lato.css" PathNameAlias="SkinPath" />--%>
<dnn:DnnCssInclude runat="server" FilePath="css/style.default.css" PathNameAlias="SkinPath" />

<%--<dnn:DnnCssInclude runat="server" FilePath="css/responsive-tables.css" PathNameAlias="SkinPath" />--%>

<%--<dnn:DnnJsInclude runat="server" FilePath="js/jquery-migrate-1.2.1.min.js" PathNameAlias="SkinPath" />--%>
<dnn:DnnJsInclude runat="server" FilePath="js/bootstrap.min.js" PathNameAlias="SkinPath" />
<%--<dnn:DnnCssInclude ID="bootstrapCSS" runat="server" FilePath="bootstrap/css/bootstrap.min.css" PathNameAlias="SkinPath" Priority="15" />--%>
<%--<dnn:DnnJsInclude runat="server" FilePath="js/jquery.uniform.min.js" PathNameAlias="SkinPath" />--%>
<dnn:DnnJsInclude runat="server" FilePath="js/custom.js" PathNameAlias="SkinPath" />
<%--<dnn:DnnCssInclude runat="server" FilePath="bootstrap/css/font-awesome.min.css" PathNameAlias="SkinPath" />--%>
<%--<dnn:DnnJsInclude runat="server" FilePath="js/responsive-tables.js" PathNameAlias="SkinPath" />--%>
<!--[if lte IE 8]><dnn:DnnJsInclude runat="server" FilePath="js/excanvas.min.js" PathNameAlias="SkinPath" /><![endif]-->

<div class="mainwrapper">
    <div class="header">
        <div class="logo">
            <%--<span class="brand visible-desktop">
                <dnn:LOGO runat="server" id="dnnLOGO" />
            </span>--%><!--/Logo-->
            <%--<span class="brand hidden-desktop">
                <dnn:LOGO runat="server" id="dnnLOGOmobi" />
            </span>--%><!--/Logo-->
            <img src="/portals/0/admin_logo_rildo.png" />
        </div>
        <div class="headerinner">
            <ul class="headmenu">
                <li>
                    <a id="linkUserMsgs" href="/">
                        <span id="totalUnreadMessages" class="count">0</span>
                        <span class="head-icon head-message"></span>
                        <span class="headmenu-label">Mensagens</span>
                    </a>
                </li>
                <li>
                    <a id="linkUserNotis" href="/">
                        <span id="totalUnreadNotifications" class="count">0</span>
                        <span class="head-icon head-noti"></span>
                        <span class="headmenu-label">Notifica&#231;&#245;es</span>
                    </a>
                </li>
                <li>
                    <a id="linkEntities" href="/">
                        <span id="totalClients" class="count">0</span>
                        <span class="head-icon head-users"></span>
                        <span class="headmenu-label">Clientes</span>
                    </a>
                </li>
                <li>
                    <a id="linkEstimates" href="/">
                        <span id="totalEstimatesOpened" class="count">0</span>
                        <span class="head-icon head-bar"></span>
                        <span class="headmenu-label">Or&#231;amentos</span>
                    </a>
                </li>
                <li>
                    <a id="linkSales" href="/">
                        <span id="totalSales" class="count">0</span>
                        <span class="head-icon head-sales"></span>
                        <span class="headmenu-label">Vendas</span>
                    </a>
                </li>
                <li class="right">
                    <div class="userloggedinfo">
                        <img id="userPhotoSkinObject" alt="" src="/images/no_avatar.gif" />
                        <div class="userinfo">
                            <h5><span id="displayNameSkinObject"></span>&nbsp; - <small id="emailSkinObject"></small></h5>
                            <ul>
                                <li><a id="linkUserProfile" href="/">Editar Perfil</a></li>
                                <%--<li><a href="/gerenciar/profile?sel=2">Alterar Senha</a></li>--%>
                                <li><a href="/default.aspx?ctl=logoff">Sair</a></li>
                            </ul>
                        </div>
                    </div>
                </li>
            </ul>
            <!--headmenu-->
        </div>
    </div>
    
    <div class="clearfix"></div>

    <div class="leftpanel">

        <div class="leftmenu">

            <h5>NAVEGA&#199;&#195;O
            </h5>

            <%--<dnn:NAV runat="server" ID="dnnNAV" ProviderName="DNNMenuNavigationProvider" IndicateChildren="false" ControlOrientation="Vertical" CSSNodeRoot="admin_dnnmenu_rootitem" CSSNodeHoverRoot="admin_dnnmenu_rootitem_hover" CSSNodeSelectedRoot="admin_dnnmenu_rootitem_selected" CSSBreadCrumbRoot="admin_dnnmenu_rootitem_selected" CSSContainerSub="admin_dnnmenu_submenu" CSSNodeHoverSub="admin_dnnmenu_itemhover" CSSNodeSelectedSub="admin_dnnmenu_itemselected" CSSContainerRoot="admin_dnnmenu_container" CSSControl="admin_dnnmenu_bar" CSSNode="admin_dnnmenu_bar" CSSBreak="admin_dnnmenu_break" />--%>
            <dnn:MENU MenuStyle="Simple" runat="server"></dnn:MENU>

        </div>
        <!--leftmenu-->

    </div>
    <!-- leftpanel -->

    <div class="rightpanel">

        <div id="Breadcrumb" class="breadcrumbs">
            <dnn:BREADCRUMB ID="dnnBreadcrumb" runat="server" CssClass="breadcrumbLink" RootLevel="0" Separator="&lt;img src=&quot;images/breadcrumb.png&quot;&gt;" />
        </div>

        <div class="pageheader">
            <div class="pagetitle">
                <div class="pull-left">
                    <h1 id="moduleTitleSkinObject"></h1>
                </div>
                <div class="pull-right">
                    <div id="buttons"></div>
                </div>
            </div>
        </div>
        <!--pageheader-->

        <div class="maincontent">
            <div class="maincontentinner">
                <div class="row-fluid">

                    <div id="ContentPane" class="contentPane" runat="server"></div>

                </div>
                <!--row-fluid-->

                <div class="footer">
                    <div class="footer-left">
                        <span>
                            <dnn:COPYRIGHT ID="dnnCopyright" runat="server" />
                        </span>
                        <div>
                            Back-end: <span id="backEndVersion"></span>
                        </div>
                        <div>
                            Front-end: <span id="frontEndVersion"></span>
                        </div>
                        <div>
                            JS: <span id="jsVersion"></span>
                        </div>
                    </div>
                    <div class="footer-right">
                        <span>Designed by: <a href="http://web.rildoinformatica.net/" target="_blank">Web.RildoInform&#225;tica.Net</a> (31) 3037-0551</span>
                    </div>
                </div>
                <!--footer-->

            </div>
            <!--maincontentinner-->
        </div>
        <!--maincontent-->

    </div>
    <!--rightpanel-->

</div>

<script type="text/javascript">
    $(function () {
        //if (document.URL.toLowerCase().indexOf('gerenciar') !== -1) {
        $('#backEndVersion').text(asmVersion);
        $('#frontEndVersion').text(ascxVersion);
        $('#jsVersion').text(jsVersion);
        $('#userPhotoSkinObject').attr({ 'src': avatar.length > 0 ? '/portals/' + _portalID + '/' + avatar + '?maxwidthw=80' : '/images/no_avatar.gif' })
        $('#displayNameSkinObject').html(userInfoDisplayName.length > 30 ? my.shorten(userInfoDisplayName, 30) : userInfoDisplayName);
        $('#emailSkinObject').html(userInfoEmail.length > 30 ? my.shorten(userInfoEmail, 30) : userInfoEmail);
        //$('#moduleTitleSkinObject').html(_moduleTitle);
        $('#linkUserMsgs').css({ 'opacity': '1 !important' }).attr({ 'href': userMsgsURL });
        $('#linkUserNotis').attr({ 'href': userNotisURL });
        $('#linkEntities').attr({ 'href': entitiesManagerURL });
        $('#linkEstimates').attr({ 'href': estimatesManagerURL });
        $('#linkSales').attr({ 'href': salesManagerURL });
        $('#linkUserProfile').attr({ 'href': userProfileURL });
        
        switch (true) {
            case (document.URL.toLowerCase().indexOf('notifications') !== -1):
                $('ul.headmenu > li:nth-child(2)').addClass('odd');
                $('#moduleTitleSkinObject').html('Mensagens & Notifica&#231;&#245;es');
                break;
            case (document.URL.toLowerCase().indexOf('mensagens') !== -1):
                $('ul.headmenu > li:first-child').addClass('odd');
                $('#moduleTitleSkinObject').html('Mensagens & Notifica&#231;&#245;es');
                break;
            case (document.URL.toLowerCase().indexOf('entidades') !== -1):
                $('ul.headmenu > li:nth-child(3)').addClass('odd');
                break;
            case (document.URL.toLowerCase().indexOf('orcamentos') !== -1):
                $('ul.headmenu > li:nth-child(4)').addClass('odd');
                break;
            default:

        }

        my.shorten = function (text, maxLength) {
            var ret = text;
            if (ret.length > maxLength) {
                ret = ret.substr(0, maxLength - 3) + "...";
            }
            return ret;
        };
        //}
    });
</script>
