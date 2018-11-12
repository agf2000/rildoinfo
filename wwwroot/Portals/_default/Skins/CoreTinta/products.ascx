<%@ Control language="vb" CodeBehind="~/admin/Skins/skin.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SEARCH" Src="~/Admin/Skins/Search.ascx" %>
<%@ Register TagPrefix="dnn" TagName="CURRENTDATE" Src="~/Admin/Skins/CurrentDate.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="NAV" Src="~/Admin/Skins/Nav.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TERMS" Src="~/Admin/Skins/Terms.ascx" %>
<%@ Register TagPrefix="dnn" TagName="PRIVACY" Src="~/Admin/Skins/Privacy.ascx" %>
<%@ Register TagPrefix="dnn" TagName="COPYRIGHT" Src="~/Admin/Skins/Copyright.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<% if (DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo().IsInRole("Editores")) then %>
	<style type="text/css">
		#dnn_ControlPanel { display: none; }
		#Body { margin-top: 0px !important; }
    </style>
<% end if %>
<% if (DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo().IsInRole("Gerentes")) then %>
	<style type="text/css">
		#dnn_ControlPanel { display: none; }
		#Body { margin-top: 0px !important; }
    </style>
<% end if %>
<% if (DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo().IsSuperUser) then %>
	<style type="text/css">
		#dnn_ControlPanel { display: block; }
		#Body { margin-top: 14px !important; }
		body { background-position: -160px 33px; }
    </style>
<% end if %>
<div id="ControlPanel" runat="server"></div>
<div id="MainWrapper">
  <div id="TopArea">
    <div class="LogoArea">
      <dnn:LOGO runat="server" id="dnnLOGO" BorderWidth="0" />
    </div>
    <div class="LoginArea searchtext">Busca:
      <dnn:SEARCH runat="server" id="dnnSEARCH" Submit="<img alt=&quot;&quot; class=&quot;searchborder&quot; src=&quot;images/button-search.gif&quot;/>" ShowSite="False" ShowWeb="False" BorderWidth="0" hspace="-1" align="absmiddle" />
      <BR />
      <dnn:CURRENTDATE runat="server" id="dnnCURRENTDATE" CssClass="date" DateFormat="D" />
      <div class="dnnClear"></div>
      <div id="Login">
        <dnn:USER runat="server" id="dnnUser" LegacyMode="false" />
        <dnn:LOGIN runat="server" id="dnnLogin" LegacyMode="false" />
      </div>
    </div>
  </div>
  <div id="menuTop">
    <dnn:NAV runat="server" id="dnnNAV" ProviderName="DNNMenuNavigationProvider" IndicateChildren="false" ControlOrientation="Horizontal" CSSNodeRoot="main_dnnmenu_rootitem" CSSNodeHoverRoot="main_dnnmenu_rootitem_hover" CSSNodeSelectedRoot="main_dnnmenu_rootitem_selected" CSSBreadCrumbRoot="main_dnnmenu_rootitem_selected" CSSContainerSub="main_dnnmenu_submenu" CSSNodeHoverSub="main_dnnmenu_itemhover" CSSNodeSelectedSub="main_dnnmenu_itemselected" CSSContainerRoot="main_dnnmenu_container" CSSControl="main_dnnmenu_bar" CSSNode="main_dnnmenu_bar" CSSBreak="main_dnnmenu_break" />
  </div>
  <div id="ControlPanel" runat="server"></div>
  <div class="BannerArea" id="BannerPane" runat="server"></div>
  <div class="ContentWrapper">
    <table border="0" style="width: 100%;">
      <tr>
        <td class="LeftOutPane" id="LeftOutPane" runat="server"></td>
        <td><table border="0" style="width: 100%;">
            <tr>
              <td id="TopPane" class="TopPane" runat="server"></td>
            </tr>
            <tr>
              <td><table border="0" style="width: 100%;">
                  <tr>
                    <td id="TopLeftPane" class="TopLeftPane" runat="server"></td>
                    <td id="TopRightPane" class="TopRightPane" runat="server" visible="false"></td>
                  </tr>
                </table></td>
            </tr>
            <tr>
              <td><table border="0" style="width: 100%;">
                  <tr>
                    <td id="LeftPane" class="LeftPane" runat="server" visible="false"></td>
                    <td id="ContentPane" class="ContentPane" runat="server"></td>
                    <td id="RightPane" class="RightPane" runat="server" visible="false"></td>
                  </tr>
                </table></td>
            </tr>
            <tr>
              <td><table border="0" style="width: 100%;">
                  <tr>
                    <td id="BottomLeftPane" class="BottomLeftPane" runat="server"></td>
                    <td id="BottomRightPane" class="BottomRightPane" runat="server" visible="false"></td>
                  </tr>
                </table></td>
            </tr>
            <tr>
              <td id="BottomPane" class="BottomPane" runat="server"></td>
            </tr>
          </table></td>
        <td class="RightOutPane" id="RightOutPane" runat="server"></td>
      </tr>
    </table>
  </div>
  <div class="dnnClear"></div>
  <div id="footer">
    <div class="TermsArea terms-privacy">
      <dnn:TERMS runat="server" id="dnnTERMS" CssClass="terms-privacy" />
      &nbsp;&nbsp;|&nbsp;&nbsp;
      <dnn:PRIVACY runat="server" id="dnnPRIVACY" CssClass="terms-privacy" />
    </div>
    <div class="FooterPaneArea" id="FooterPane" runat="server"></div>
    <div class="CopyrightArea">
      <dnn:COPYRIGHT runat="server" id="dnnCOPYRIGHT" CssClass="copy-right" />
    </div>
  </div>
  <div id="Breadcrumb"> <a href="http://www.riw.com.br" onclick="window.open(this.href); return false;"><img alt="DesignByLogo" src="/Portals/0/design-by-logo.png" /></a> </div>
</div>
<dnn:DnnCssInclude ID="DnnCssInclude1" runat="server" FilePath="/desktopmodules/rildoinfo/ristoredataservices/Content/kendo/2013.2.716/kendo.default.min.css" Priority="82" />

<telerik:RadWindowManager ID="RadWindowManager1"   runat="server"
                            VisibleStatusbar="false"
                            Behaviors="Reload, Close, Maximize, Move" Modal="true"
                            Localization-Close="Fechar"
                            Localization-Maximize="Maximizar"
                            Localization-Restore="Restaurar"
                            Localization-Reload="Recarregar"
                            EnableShadow="true"
                            ShowContentDuringLoad="false"
                            Style="z-index: 7001"> </telerik:RadWindowManager>

<dnn:DnnJsInclude ID="DnnJsInclude1" runat="server" FilePath="jquery.cycle.min.js" PathNameAlias="SkinPath" />

<!-- Slider -->
<dnn:DnnJsInclude ID="DnnJsInclude2" runat="server" FilePath="/portals/_default/skins/coretinta/js/bjqs-1.3.min.js" />

<% if not (DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo().IsInRole("Administrators")) = True then %>
	<script type="application/javascript">

	    $(document).ready(function () {

	        // Set checkbox as checked 
	        $('input[value=rdDNNScheduled]').attr('checked', 'checked');
	        $('input[id=dnn_ctr416_AxonView_ctl02_chkEnabled]').attr('checked', true);

	        // Remove Messages and Notifications text
	        $('strong:contains("Mensagens")').html('');
	        $('strong:contains("Notificações")').html('');

	        // Terms and Pravacy module title
	        $('span:contains("Terms Of Use")').hide();
	        $('span:contains("Privacy Statement")').hide();

	        // Table spacing
	        $('.ModOnyakTechAxonC').find('td').css({ 'padding': '5px' });

	        // Axon
	        $('.rgGroupPanel').find("tr:contains('Drag a column header and drop it here to group by that column')").hide();

	        // Axon Menu
	        $('span:contains("Gráficos de Campanha")').hide();
	        $('span:contains("Cliques Rastreados")').hide();
	        $('span:contains("Processar Caixa de Entrada POP3")').hide();
	        $('span:contains("Registro de Envios Ativos")').hide();
	        $('span:contains("Leituras Rastreadas")').hide();
	        $('span:contains("Registro de Eventos")').hide();
	        $('span:contains("Gerador de Lista de Assinantes")').hide();

	        // Axon Campaign Details (New)
	        // $('.ModOnyakTechAxonC').find("tr:contains('Limite de Número de Envios:')").hide();
	        // $('.ModOnyakTechAxonC').find("tr:contains('Grupos Atribuídos:')").hide();
	        $('.ModOnyakTechAxonC').find("tr:contains('Opções de Envio:')").hide();
	        $('.ModOnyakTechAxonC').find("tr:contains('Tipo de Campanha:')").hide();
	        $('.ModOnyakTechAxonC').find("tr:contains('Endereço Web2Mail:')").hide();
	        $('.ModOnyakTechAxonC').find("tr:contains('Palavras Chave Auto- Resposta:')").hide();
	        $('.ModOnyakTechAxonC').find("tr:contains('Enviar uma cópia de cada email pra você')").hide();
	        $('.ModOnyakTechAxonC').find("tr:contains('Habilitar Personalizações?')").hide();
	        $('.ModOnyakTechAxonC').find("tr:contains('Solicite Confirmação de Leitura')").hide();
	        $('.ModOnyakTechAxonC').find("tr:contains('Definir a Precedência para Envio em Massa')").hide();
	        $('.ModOnyakTechAxonC').find("tr:contains('Opções de Execução Automatizadas')").hide();
	        $('.ModOnyakTechAxonC').find("tr:contains('Condição para Executar Campanha')").hide();
	        $('.ModOnyakTechAxonC').find("tr:contains('Comando SQL')").hide();
	        $('.ModOnyakTechAxonC').find("tr:contains('Email Para RSS')").hide();
	        $('.ModOnyakTechAxonC').find("tr:contains('Link para Inscrição')").hide();

	        // Axon Campaign Details
	        $('.ModOnyakTechAxonC').find("a:contains('Add new record')").html('Adicionar Novo Assinante');
	        $('.ModOnyakTechAxonC').find("a:contains('First Name')").html('Nome');
	        $('.ModOnyakTechAxonC').find("a:contains('Last Name')").html('Sobrenome');
	        $('.ModOnyakTechAxonC').find("a:contains('Enabled')").html('Ativado');
	        $('.ModOnyakTechAxonC').find("a:contains('Created Date')").html('Criado Em');
	        $('.ModOnyakTechAxonC').find("a:contains('Disabled Date')").html('Desativado Em');
	        $('.ModOnyakTechAxonC').find("a:contains('Refresh')").html('Atualizar Lista');

	        // Axon Grids
	        $('.ModOnyakTechAxonC').find("a:contains('Disponibilizar Campanha à Lista de Assintantes')").hide();
	        $('.ModOnyakTechAxonC').find("tr:contains('Tokens de Conteúdo')").hide();

	        // Axon Inserts Fields Sizes
	        $('#tblNewEditTagInfo').find("input:text").width('470');
	        $('#tblNewEditTagInfo').find("textarea").width('470');
	        $('#tblNewEditTagInfo').find(".dnnTooltip").find("div").width('120');
	        $('#tblNewEditTagInfo').find(".dnnTooltip").find("a").width('120');

	        // Axon Optin Manager (Campaign Manager)
	        //        $('.ModOnyakTechAxonC').find("tr:contains('Faça upload do arquivo CSV:')").hide();


	    });

    </script>
<% end if %>
