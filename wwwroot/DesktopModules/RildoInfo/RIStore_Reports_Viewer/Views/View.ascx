<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="View.ascx.vb" Inherits="View" %>
<div id="reportsViewer">
    <div class="divReports">
        <strong>Relatórios Disponíveis: </strong>
        &nbsp;
        <input id="kddlReports" />
    </div>
    <fieldset>
        <legend>Filtros</legend>
        <ul>
            <li>
                <label>Clientes: </label>
                <input id="clientSearchBox" />
            </li>
            <li>
                <input id="kddlSalesPerson" /><br />
                <input id="kddlStatuses" />
            </li>
            <li>
                <label>Data Inicial: </label>
                <input id="kdpStartDate" /><br />
                <label>Dat Final: </label>
                <input id="kdpEndDate" />
            </li>
            <li>
                <button class="k-button">Aplicar Filtro</button>
            </li>
        </ul>
        <div class="dnnClear"></div>
    </fieldset>
    <div id="reportGrid"></div>
</div>
