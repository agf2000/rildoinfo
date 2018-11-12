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
<style>
    /*** LOGIN PAGE ***/

    .loginpanel {
        position: absolute;
        top: 50%;
        left: 50%;
        height: 300px;
    }

    .loginpanelinner {
        position: relative;
        top: -300px;
        left: -50%;
    }

        .loginpanelinner .logo {
            text-align: center;
            padding: 20px 40px 20px 0;
            .inputwrapper input;

    {
        border: 0;
        padding: 10px;
        background: #fff;
        width: 250px;
    }

    .inputwrapper input:active, .inputwrapper input:focus {
        background: #fff;
        border: 0;
    }

    .inputwrapper button {
        display: block;
        border: 1px solid #0c57a3;
        padding: 10px;
        background: #0972dd;
        width: 100%;
        color: #fff;
        text-transform: uppercase;
    }

        .inputwrapper button:focus, .inputwrapper button:active, .inputwrapper button:hover {
            background: #1e82e8;
        }

    .inputwrapper label {
        display: inline-block;
        margin-top: 10px;
        color: rgba(255,255,255,0.8);
        font-size: 11px;
        vertical-align: middle;
    }

        .inputwrapper label input {
            width: auto;
            margin: -3px 5px 0 0;
            vertical-align: middle;
        }

    .inputwrapper .remember {
        padding: 0;
        background: none;
    }

    .login-alert {
        display: none;
    }

        .login-alert .alert {
            font-size: 11px;
            text-align: center;
            padding: 5px 0;
            border: 0;
        }

    .loginfooter {
        font-size: 11px;
        color: rgba(255,255,255,0.5);
        position: absolute;
        position: fixed;
        bottom: 0;
        left: 0;
        width: 100%;
        text-align: center;
        font-family: arial, sans-serif !important;
        padding: 5px 0;
    }
</style>
<div class="loginpanel">
    <div class="loginpanelinner">
        <div class="logo animate0 bounceIn">
            <img src="/portals/0/logo.png" alt="" /></div>
        <div id="ContentPane" class="contentPane" runat="server"></div>
    </div>
</div>
