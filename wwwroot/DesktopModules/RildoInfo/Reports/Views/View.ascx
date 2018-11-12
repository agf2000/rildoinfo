<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="View.ascx.vb" Inherits="RIW.Modules.Reports.Views.View" %>
<div id="reportsViewer" class="reportsViewer">
    <ul id="kmReports">
        <li>Entidades
            <ul>
                <li id="menu_2">Lista de Clientes
                </li>
                <li id="menu_6">Lista de Fornecedores
                </li>
            </ul>
        </li>
        <li>Produtos
            <ul>
                <li id="menu_4">Lista de Produtos
                </li>
                <%--<li id="menu_11">Produtos ativos mais Tributações
                </li>--%>
                <li id="menu_7">Produtos > Fornecedores
                </li>
                <li id="menu_8">Fornecedores > Produtos
                </li>
            </ul>
        </li>
        <li>Orçamentos
            <ul>
                <li id="menu_1">Orçamentos > Produtos
                </li>
                <li id="menu_3">Produtos > Orçamentos
                </li>
            </ul>
        </li>
        <li>Financeiro
            <ul>
                <li id="menu_5">Movimentações
                </li>
                <li id="menu_9">Vendas
                </li>
                <li id="menu_10">Fluxo
                </li>
            </ul>
        </li>
    </ul>
    <table id="tblReport">
        <tr>
            <td><h4><strong>Relatório:</strong></h4></td>
            <td><h4 id="reportName" style="font-weight: bold;"></h4></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <h5 id="reportDescription"></h5>
            </td>
        </tr>
    </table>
    <div class="accordion">
        <div class="accordion-group">
            <div class="accordion-heading">
                <a class="accordion-toggle" data-toggle="collapse" href="#collapseFilter">
                    <h4>Filtro</h4>
                </a>
            </div>
            <div id="collapseFilter" class="accordion-body collapse in">
                <div class="accordion-inner">
                    <ul>
                        <li>
                            <label>Procurar Clientes:</label>
                            <select id="kddlClientFields" data-bind="kendoDropDownList: {}" class="input-medium">
                                <option value="PERSONID">Código</option>
                                <option value="FIRSTNAME" selected>Nome</option>
                                <option value="LASTNAME">Sobrenome</option>
                                <option value="COMPANYNAME">Empresa</option>
                                <option value="EMAIL">Email</option>
                                <option value="TELEPHONE">Telefone</option>
                                <option value="CPF">CPF</option>
                                <option value="IDENT">Identidade</option>
                                <option value="EIN">CNPJ</option>
                            </select>
                        </li>
                        <li>
                            <label>Clientes: </label>
                            <input id="clientSearchBox" class="input-xxlarge" />
                        </li>
                        <li>
                            <label>Status:</label>
                            <select id="kmsStatuses" style="width: 220px;"></select>
                        </li>
                    </ul>
                    <div class="clearfix"></div>
                    <ul>
                        <li>
                            <label>Procurar Fornecedores:</label>
                            <select id="kddlProviderFields" data-bind="kendoDropDownList: {}" class="input-medium">
                                <option value="PERSONID">Código</option>
                                <option value="DISPLAYNAME" selected>Nome</option>
                                <option value="COMPANYNAME">Empresa</option>
                                <option value="EMAIL">Email</option>
                                <option value="TELEPHONE">Telefone</option>
                                <option value="CPF">CPF</option>
                                <option value="IDENT">Identidade</option>
                                <option value="EIN">CNPJ</option>
                            </select>
                        </li>
                        <li>
                            <label>Fornecedores: </label>
                            <input id="providerSearchBox" class="input-xxlarge" />
                        </li>
                        <li>
                            <label>Crédito somente: </label>
                            <input type="checkbox" id="chkBoxCredit" /> <strong>Marcar p/ Sim</strong>
                        </li>
                    </ul>
                    <div class="clearfix"></div>
                    <ul>
                        <li>
                            <label>Procurar Produtos:</label>
                            <select id="kddlProductFields" data-bind="kendoDropDownList: {}" class="input-medium">
                                <option value="PRODUCTID">Código</option>
                                <option value="PRODUCTNAME" selected>Nome</option>
                                <option value="PRODUCTREF">Referência</option>
                                <option value="BARCODE">Cod. Barras</option>
                            </select>
                        </li>
                        <li>
                            <label>Produtos: </label>
                            <input id="productSearchBox" class="input-xxlarge" />
                        </li>
                        <li>
                            <label>Vendedores:</label>
                            <input id="kddlSalesPerson" />
                        </li>
                    </ul>
                    <div class="dnnClear"></div>
                    <ul>
                        <li>
                            <label>Filtar por Datas:</label>
                            <select id="kddlDates" data-bind="kendoDropDownList: {}" class="input-medium">
                                <option value="CREATEDONDATE" selected>Inserido Em</option>
                                <option value="MODIFIEDONDATE">Alterado Em</option>
                                <option value="TRANSDATE">Movimentado Em</option>
                                <option value="DUEDATE">Vencendo Em</option>
                            </select>
                        </li>
                        <li>
                            <label>Data Inicial: </label>
                            <input id="kdpStartDate" />
                        </li>
                        <li>
                            <label>Data Final: </label>
                            <input id="kdpEndDate" />
                        </li>
                        <li>
                            <label>Quitados somente: </label>
                            <input type="checkbox" id="chkBoxDone" /> <strong>Marcar p/ Sim</strong>
                        </li>
                        <li>
                            <button id="btnFilter" class="k-primary" style="margin-top: 25px;">Aplicar Filtro</button>
                        </li>
                    </ul>
                    <div class="dnnClear"></div>
                </div>
            </div>
        </div>
    </div>
    <div id="ulToolbar">
        <ul class="inline">
            <li>
                <button id="btnPrint" class="k-button" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento enquanto a página de impressão está sendo gerada..."><i class="fa fa-print"></i>&nbsp; Imprimir</button>
            </li>
            <li>
                <button id="btnExcel" class="k-button" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento enquanto o PDF está sendo gerada..."><i class="fa fa-file-excel-o"></i>&nbsp; Exportar Excel</button>
            </li>
        </ul>
    </div>

    <div id="reportGrid"></div>
    <script type="text/x-kendo-template" id="tmplDetail">
        <div class="details"></div>
    </script>
</div>
