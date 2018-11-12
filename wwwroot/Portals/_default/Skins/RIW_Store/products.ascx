<%@ Control language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="LANGUAGE" Src="~/Admin/Skins/Language.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="BREADCRUMB" Src="~/Admin/Skins/BreadCrumb.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="PRIVACY" Src="~/Admin/Skins/Privacy.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TERMS" Src="~/Admin/Skins/Terms.ascx" %>
<%@ Register TagPrefix="dnn" TagName="COPYRIGHT" Src="~/Admin/Skins/Copyright.ascx" %>
<%@ Register TagPrefix="dnn" TagName="MENU" src="~/DesktopModules/DDRMenu/Menu.ascx" %>
<%@ Register TagPrefix="dnn" TagName="JQUERY" Src="~/Admin/Skins/jQuery.ascx" %>
<%@ Register TagPrefix="dnn" TagName="META" Src="~/Admin/Skins/Meta.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnJsInclude ID="bootstrapInclude" runat="server" FilePath="bootstrap/js/bootstrap.min.js" PathNameAlias="SkinPath" />
<dnn:DnnCssInclude runat="server" FilePath="/desktopmodules/rildoinfo/WebAPI/content/kendo/2015.3.930/styles/kendo.blueopal.min.css" />

<dnn:META runat="server" Name="viewport" Content="width=device-width,initial-scale=1" />

<div id="pageWrapper">
    <div id="userControls" class="container">
        <div class="row-fluid">
            <div class="span2 language pull-left">
                <dnn:LANGUAGE runat="server" showMenu="False" showLinks="True" />
            </div>
            <div id="login" class="span5 pull-right">
                <dnn:LOGIN ID="dnnLogin" CssClass="LoginLink" runat="server" LegacyMode="false" />
                <dnn:USER ID="dnnUser" runat="server" LegacyMode="false" /> 
            </div><!--/login-->
        </div>
    </div><!--/userControls-->
    <div id="siteHeadouter">
        <div id="siteHeadinner" class="container">
        	<div class="navbar">
            	<div class="navbar-inner">
                    <span class="brand visible-desktop">
                        <dnn:LOGO runat="server" id="dnnLOGO" />
                    </span><!--/Logo-->
                    <span class="brand hidden-desktop">
                        <dnn:LOGO runat="server" id="dnnLOGOmobi" />
                    </span><!--/Logo-->
                    <a class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse">Menu</a>
                    <div class="nav-collapse collapse pull-right">
                        <dnn:MENU MenuStyle="Simple" runat="server"></dnn:MENU>
                    </div><!-- END nav-collapse -->
                </div><!-- END navbar-inner -->
			</div><!-- END navbar -->
        </div><!--/siteHeadinner-->
    </div><!--/siteHeadouter-->
    <div id="contentWrapper" class="container">
        <div class="row-fluid">
    	    <div id="Breadcrumb" class="span12">
                <dnn:BREADCRUMB ID="dnnBreadcrumb" runat="server" CssClass="breadcrumbLink" RootLevel="0" Separator="&lt;img src=&quot;img/breadcrumb.png&quot;&gt;" />
    	    </div>
        </div>
        <div class="row-fluid">
		    <div id="contentPane" class="contentPane" runat="server"></div>
            <div id="leftPane" class="leftPane span3" runat="server"></div>
            <div id="rightPane" class="rightPane span9" runat="server"></div>
        </div>
        <div id="mobileContent" class="row visible-phone">
            <div id="mobileContentPane" class="span12" runat="server" />
        </div><!-- /mobileContent -->
        <div id="footer">
            <div class="row-fluid">
                <hr class="span12"/>
            </div>
            <div id="copyright" class="row-fluid">
				<div class="pull-right">
					<dnn:TERMS ID="dnnTerms" runat="server" /> |
					<dnn:PRIVACY ID="dnnPrivacy" runat="server" />
				</div>
				<dnn:COPYRIGHT ID="dnnCopyright" runat="server" CssClass="pull-left" />
            </div><!--/copyright-->
        </div><!--/footer-->
    </div>
</div><!-- /pageWrap -->