<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Painel.ascx.vb" Inherits="RIW.Modules.StoreAdmin.Views.Painel" %>
<div id="storeAdmin" class="row-fluid">
    <div class="span12">
        <div class="span7">
            <ul class="shortcuts">
                <li>
                    <asp:HyperLink ID="linkCategories" runat="server">
                        <span class="shortcuts-icon fa fa-sitemap"></span>
                        <span class="shortcuts-label">Categorias</span>
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:hyperlink ID="linkProducts" runat="server">
                        <span class="shortcuts-icon fa fa-barcode"></span>
                        <span class="shortcuts-label">Produtos</span>
                    </asp:hyperlink>
                </li>
                <li>
                    <asp:hyperlink ID="linkEntities" runat="server">
                        <span class="shortcuts-icon fa fa-group"></span>
                        <span class="shortcuts-label">Entidades</span>
                    </asp:hyperlink>
                </li>
                <li>
                    <asp:hyperlink ID="linkUsers" runat="server">
                        <span class="shortcuts-icon fa fa-suitcase"></span>
                        <span class="shortcuts-label">Colaboradores</span>
                    </asp:hyperlink>
                </li>
                <li>
                    <asp:HyperLink ID="linkDav" runat="server">
                        <span class="shortcuts-icon fa fa-edit"></span>
                        <span class="shortcuts-label">DAVs</span>
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:hyperlink ID="linkFinances" runat="server">
                        <span class="shortcuts-icon fa fa-archive"></span>
                        <span class="shortcuts-label">Financeiro</span>
                    </asp:hyperlink>
                </li>
                <li>
                    <asp:hyperlink ID="linkAgenda" runat="server">
                        <span class="shortcuts-icon fa fa-calendar"></span>
                        <span class="shortcuts-label">Agenda</span>
                    </asp:hyperlink>
                </li>
                <li>
                    <asp:hyperlink ID="linkOR" runat="server">
                        <span class="shortcuts-icon fa fa-shopping-cart"></span>
                        <span class="shortcuts-label">OR</span>
                    </asp:hyperlink>
                </li>
                <li>
                    <asp:hyperlink ID="linkReports" runat="server">
                        <span class="shortcuts-icon fa fa-bar-chart-o"></span>
                        <span class="shortcuts-label">Relatórios</span>
                    </asp:hyperlink>
                </li>
                <li class="last">
                    <asp:hyperlink ID="linkStore" runat="server">
                        <span class="shortcuts-icon fa fa-cogs"></span>
                        <span class="shortcuts-label">Loja</span>
                    </asp:hyperlink>
                </li>
            </ul>
        </div>
        <div class="span5">
            <div id="divIndicators">
                <fieldset>
                    <legend>Orçamentos</legend>
                    <div class="span6">
                        <label class="font-size-large"><strong>Produtos:</strong></label>
                        <label class="indicators" data-bind="text: kendo.toString(totalProductsEstimates(), 'c')"></label>
                        <div class="padded"></div>
                        <label class="font-size-large"><strong>Serviços:</strong></label>
                        <label class="indicators" data-bind="text: kendo.toString(totalServicesEstimates(), 'c')"></label>
                        <div class="padded"></div>
                        <label class="font-size-large"><strong>Total:</strong></label>
                        <label class="indicators" data-bind="text: kendo.toString(totalEstimatesBalance(), 'c')"></label>
                        <div class="padded"></div>
                    </div>
                    <div class="span3">
                        <div class="padded">
                            <strong>Data Inicial:</strong><input id="kdpStartingEstimate" />
                        </div>
                        <div class="padded">
                            <strong>Data Final:</strong><input id="kdpEndingEstimate" />
                        </div>
                        <div class="padded"></div>
                        <div>
                            <button id="btnTotalEstimates" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>"><i class="icon icon-refresh"></i>&nbsp; Atualizar</button>
                        </div>
                    </div>
                </fieldset>
                <fieldset>
                    <legend>Vendas</legend>
                    <div class="span6">
                        <label class="font-size-large"><strong>Produtos:</strong></label>
                        <label class="indicators" data-bind="text: kendo.toString(totalProductSales(), 'c')"></label>
                        <div class="padded"></div>
                        <label class="font-size-large"><strong>Serviços:</strong></label>
                        <label class="indicators" data-bind="text: kendo.toString(totalServiceSales(), 'c')"></label>
                        <div class="padded"></div>
                        <label class="font-size-large"><strong>Total:</strong></label>
                        <label class="indicators" data-bind="text: kendo.toString(totalSalesBalance(), 'c')"></label>
                        <div class="padded"></div>
                    </div>
                    <div class="span3">
                        <div class="padded">
                            <strong>Data Inicial:</strong><input id="kdpStartingSale" />
                        </div>
                        <div class="padded">
                            <strong>Data Final:</strong><input id="kdpEndingSale" />
                        </div>
                        <div class="padded"></div>
                        <div>
                            <button id="btnTotalSales" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>"><i class="icon icon-refresh"></i>&nbsp; Atualizar</button>
                        </div>
                    </div>
                </fieldset>
                <fieldset>
                    <legend>Crédito</legend>
                    <div class="span4">
                        <div>
                            <label class="font-size-large"><strong>Previsto:</strong></label>
                            <label class="indicators" data-bind="text: kendo.toString(credit4Seen(), 'c')"></label>
                        </div>
                        <div class="padded"></div>
                        <div>
                            <strong>Data Inicial:</strong><br /><input id="kdpStartingIncome" />
                        </div>
                        <div class="padded"></div>
                    </div>
                    <div class="span4">
                        <div>
                            <label class="font-size-large"><strong>Realizado:</strong></label>
                            <label class="indicators" data-bind="text: kendo.toString(creditActual(), 'c')"></label>
                        </div>
                        <div class="padded"></div>
                        <div>
                            <strong>Data Final:</strong><br /><input id="kdpEndingIncome" />
                        </div>
                        <div class="padded"></div>
                    </div>
                    <div>
                        <div>
                            <label class="font-size-large"><strong>Em Aberto:</strong></label>
                            <label class="indicators" data-bind="text: kendo.toString(creditBalance(), 'c'), attr: { class: creditBalance() >= 0 ? 'indicators negative' : 'indicators positive' }""></label>
                        </div>
                        <div class="padded"></div>
                        <div>
                            <br />
                            <button id="btnCredit" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>"><i class="icon icon-refresh"></i>&nbsp; Atualizar</button>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                </fieldset>
                <fieldset>
                    <legend>Débito</legend>
                    <div class="span4">
                        <div>
                            <label class="font-size-large"><strong>Previsto:</strong></label>
                            <label class="indicators" data-bind="text: kendo.toString(debit4Seen(), 'c')"></label>
                        </div>
                        <div class="padded"></div>
                        <div>
                            <strong>Data Inicial:</strong><br /><input id="kdpStartingDebit" />
                        </div>
                        <div class="padded"></div>
                    </div>
                    <div class="span4">
                        <div>
                            <label class="font-size-large"><strong>Realizado:</strong></label>
                            <label class="indicators" data-bind="text: kendo.toString(debitActual(), 'c')"></label>
                        </div>
                        <div class="padded"></div>
                        <div>
                            <strong>Data Final:</strong><br /><input id="kdpEndingDebit" />
                        </div>
                        <div class="padded"></div>
                    </div>
                    <div>
                        <div>
                            <label class="font-size-large"><strong>Em Aberto:</strong></label>
                            <label data-bind="text: kendo.toString(debitBalance(), 'c'), attr: { class: debitBalance() > 0 ? 'indicators negative' : 'indicators' }"></label>
                        </div>
                        <div class="padded"></div>
                        <div>
                            <br />
                            <button id="btnDebit" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>"><i class="icon icon-refresh"></i>&nbsp; Atualizar</button>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                </fieldset>
            </div>
        </div>
        <div class="clearfix">
        </div>
    </div>
</div>
