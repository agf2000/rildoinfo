<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Payments_Manager.ascx.vb" Inherits="RIW.Modules.Store_Manager.Views.PaymentsManager" %>
<div id="divPayments" class="row-fluid">
    <div class="span12">
        <div class="accordion">
            <div class="accordion-group">
                <div class="accordion-heading">
                    <a class="accordion-toggle" data-toggle="collapse" href="#collapseFilter">
                        <h4>Filtro</h4>
                    </a>
                </div>
                <div id="collapseFilter" class="accordion-body collapse">
                    <div class="accordion-inner">
                        <ul id="divPaymentsButtons" class="inline">
                            <li class="dropdowns">
                                <strong>Contas:</strong>
                                <i class="icon icon-exclamation-sign" title="Filtrar por Contas" data-content="Escolha a conta desejada e clique em [Aplicar Filtro]."></i>
                                <input id="ddlAccounts" />
                                <div class="padded"></div>
                                <strong>Categoria:</strong>
                                <i class="icon icon-exclamation-sign" title="Créditos e Débitos" data-content="Vêr crédito e débito ou apenas um ou outro. Escolha e clique em [Aplicar Filtro]."></i>
                                <select id="ddlCategory" class="input-small">
                                    <option value="">Vêr Créditos e Débitos</option>
                                    <option value="1">Vêr Somente Crédito</option>
                                    <option value="2">Vêr Somente Débitos</option>
                                </select>
                                <div class="padded"></div>
                                <strong>Incluir Quitadas?</strong>
                                <i class="icon icon-exclamation-sign" title="Pagamentos Quitados" data-content="Marque sim ou não se deseja inluir pagamentos já pagos na lista abaixo e clique em [Aplicar Filtro]."></i>
                                <input id="includePaid" type="checkbox" checked class="normalCheckBox switch-small" data-on-label="Sim" data-off-label="Não" />
                            </li>
                            <li>
                                <strong>Filtrar por Datas:</strong>
                                <i class="icon icon-exclamation-sign" title="Filtrar por Datas" data-content="Escolha entre data de inserção, alteração ou vencimento. É necessário inserir a data incial e a data final. Clique em [Aplicar Filtro] patra ativar o filtro."></i>
                                <select id="ddlDates" class="input-small">
                                    <option value="DueDate">Vencimento Em</option>
                                    <option value="ModifiedOnDate">Alterado Em</option>
                                    <option value="CreatedOnDate">Inserido Em</option>
                                </select>
                                <div class="padded"></div>
                                <strong>Data Inicial:</strong>
                                <i class="icon icon-exclamation-sign" title="Filtrar Inicial" data-content="Esolha entre data de inserção, alteração ou vencimento, insira a data inicial e clique em [Aplicar Filtro]."></i>
                                <input id="kdpStartDate" placeholder="Data Inicial" title="Data da emissão" />
                                <div class="padded"></div>
                                <strong>Data Final:</strong>
                                <i class="icon icon-exclamation-sign" title="Filtrar Final" data-content="Esolha entre data de inserção, alteração ou vencimento, insira a data final e clique em [Aplicar Filtro]."></i>
                                <input id="kdpEndDate" placeholder="Data Final" title="Data da emissão" />
                            </li>
                            <li>
                                <strong>Fornecedores:</strong>
                                <i class="icon icon-exclamation-sign" title="Filtre por Fornecedores" data-content="Digite 5 carecteres ou mais para iniciar sua busca. As opções são Nome próprio, Empresa, Telefone, Email, CPF ou CNPJ. Encontre o fornecedor e clique em [Aplicar Filtro]."></i>
                                <input id="vendorSearchBox" class="input-large" />
                                <div class="padded"></div>
                                <strong>Clientes:</strong>
                                <i class="icon icon-exclamation-sign" title="Filtre por Clientes" data-content="Digite 5 carecteres ou mais para iniciar sua busca. As opções são Nome próprio, Empresa, Telefone, Email, CPF ou CNPJ. Encontre o cliente e clique em [Aplicar Filtro]."></i>
                                <input id="clientSearchBox" class="input-large" />
                                <div class="padded"></div>
                                <div class="form-inline text-right">
                                    <i class="icon icon-exclamation-sign" title="Palavra Chave" data-content="Faça sua busca por palavras contidas no campo de descrição. Digite a palavra e clique em [Aplicar Filtro]."></i>
                                    <input id="txtBoxSearch" autocomplete="off" type="text" class="input-small" placeholder="Palavra Chave" />
                                    <button id="btnSearch" class="btn btn-small" title="Clique aqui para fazer sua busca"><span class="icon icon-search"></span>&nbsp; Aplicar Filtro</button>
                                    <button id="btnRemoveFilter" class="btn btn-small" title="Clique aqui para remover filtros"><span class="k-icon k-i-cancel"></span></button>
                                </div>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                </div>
            </div>
        </div>
        <div id="balances1" style="float: right; margin-top: 10px;">
            <ul class="inline">
                <li>
                    De &nbsp; <b><span id="spanStartDate"></span></b> &nbsp; à &nbsp; <b><span id="spanEndDate"></span></b>
                </li>
                <li>
                    <label id="columnCredit"></label>
                </li>
                <li>
                    <label id="columnDebit"></label>
                </li>
                <li>
                    <label id="columnBalance"></label>
                </li>
            </ul>
        </div>
        <div class="clearfix"></div>
        <div id="paymentsGrid"></div>
        <div id="paymentWindow"></div>
        <script id="tmplToolbar" type="text/x-kendo-template">
            <ul id="ulPaymentsToolbar" class="inline">
                <li>
                    <button id="btnAddNewPayment" class="btn btn-small btn-inverse"><span class="icon-plus icon-white"></span>&nbsp; Adicionar</button>
                </li>
                <li>
                    <button id="btnEditSelected" class="btn btn-small k-state-disabled" disabled="disabled" onclick="my.editPayment(); return false;"><span class="icon icon-edit"></span> Editar</button>
                </li>
                <li>
                    <button id="btnCalendar" class="btn btn-small" title="Clique aqui para fazer sua busca"><span class="icon icon-calendar"></span> Calendário</button>
                </li>
                <li>
                    <button id="btnDeleteSelected" class="btn btn-small" onclick="my.deletePayment(); return false;"><span class="icon icon-remove-circle"></span> Cancelar Selecionado(s)</button>
                </li>
                <li>
                    <button id="btnRestoreSelected" class="btn btn-small" onclick="my.restorePayment(); return false;"><span class="icon icon-refresh"></span> Restaurar</button>
                </li>
                <li>                    
                    <div id="myLegend">
                        <ul class="inline">
                            <li><span style="background-color: green;"></span>Quitado</li>
                            <li><span style="background-color: red;"></span>Atrazado</li>
                        </ul>
                    </div>
                </li>
            </ul>
        </script>
        <div id="balances2" class="right padded">
            <ul class="inline">
                <li>
                    <b>Balanço Futuro:</b>
                </li>
                <li>
                    <label id="balanceDebitLabel"></label>
                </li>
                <li>
                    <label id="balanceDifferenceLabel"></label>
                </li>
                <li>
                    <label id="balanceCreditLabel"></label>
                </li>
            </ul>
        </div>
    </div>
</div>
