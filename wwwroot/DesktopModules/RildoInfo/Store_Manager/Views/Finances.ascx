<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Finances.ascx.vb" Inherits="RIW.Modules.Store_Manager.Views.Finances" %>
<div id="storeManager" class="row-fluid">
    <div class="span12">
        <ul class="shortcuts">
            <li>
                <asp:HyperLink ID="linkInvoices" runat="server">
                    <span class="shortcuts-icon fa fa-briefcase"></span>
                    <span class="shortcuts-label">Lançamentos</span>
                </asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="linkFluxo" runat="server">
                    <span class="shortcuts-icon fa fa-book"></span>
                    <span class="shortcuts-label">Fluxo</span>
                </asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="linkCashiers" runat="server">
                    <span class="shortcuts-icon iconsi-archive"></span>
                    <span class="shortcuts-label">Caixa</span>
                </asp:HyperLink>
            </li>
        </ul>
    </div>
</div>
