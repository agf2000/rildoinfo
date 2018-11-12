<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Store.ascx.vb" Inherits="RIW.Modules.StoreAdmin.Views.Store" %>
<div id="storeAdmin" class="row-fluid">
    <div class="span12">
        <ul class="shortcuts">
            <li class="address">
                <asp:hyperlink ID="linkAddress" runat="server">
                    <span class="shortcuts-icon fa fa-book"></span>
                    <span class="shortcuts-label">Endereço</span>
                </asp:hyperlink>
            </li>
            <li class="registration">
                <asp:hyperlink ID="linkRegistration" runat="server">
                    <span class="shortcuts-icon fa fa-edit"></span>
                    <span class="shortcuts-label">Cadastro</span>
                </asp:hyperlink>
            </li>
            <li class="payConds">
                <asp:hyperlink ID="linkPayConds" runat="server">
                    <span class="shortcuts-icon fa fa-credit-card"></span>
                    <span class="shortcuts-label">Condições</span>
                </asp:hyperlink>
            </li>
            <li class="estimate">
                <asp:hyperlink ID="linkEstimate" runat="server">
                    <span class="shortcuts-icon fa fa-briefcase"></span>
                    <span class="shortcuts-label">Orçamento</span>
                </asp:hyperlink>
            </li>
            <li class="smtp">
                <asp:hyperlink ID="linkSMTP" runat="server">
                    <span class="shortcuts-icon fa fa-envelope-o"></span>
                    <span class="shortcuts-label">SMTP</span>
                </asp:hyperlink>
            </li>
            <li class="statuses">
                <asp:hyperlink ID="linkStatuses" runat="server">
                    <span class="shortcuts-icon fa fa-check-circle"></span>
                    <span class="shortcuts-label">Status</span>
                </asp:hyperlink>
            </li>
            <li class="website">
                <asp:hyperlink ID="linkWebsite" runat="server">
                    <span class="shortcuts-icon fa fa-bookmark"></span>
                    <span class="shortcuts-label">Website</span>
                </asp:hyperlink>
            </li>
            <li class="templates">
                <asp:hyperlink ID="linkTemplates" runat="server">
                    <span class="shortcuts-icon fa fa-puzzle-piece"></span>
                    <span class="shortcuts-label">Templates</span>
                </asp:hyperlink>
            </li>
        </ul>
    </div>
</div>
