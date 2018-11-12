﻿
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    '<PrimaryKey("KitItemId", AutoIncrement:=True)> _
    <TableName("RIS_EstimateRemovedItems")> _
    <Cacheable("EstimateRemovedItems", CacheItemPriority.Default, 20)> _
    <Scope("EstimateId")>
    Public Class EstimateRemovedItem
        Implements IEstimateItem

        <IgnoreColumn> _
        Public Property ClientId As Integer Implements IEstimate.ClientId

        <IgnoreColumn> _
        Public Property Comment As String Implements IEstimate.Comment

        <IgnoreColumn> _
        Public Property CommentText As String Implements IEstimate.CommentText

        Public Property CreatedByUser As Integer Implements IEstimate.CreatedByUser

        Public Property CreatedOnDate As Date Implements IEstimate.CreatedOnDate

        <IgnoreColumn> _
        Public Property CurrentTimestamp As Boolean Implements IEstimate.CurrentTimestamp

        <IgnoreColumn> _
        Public Property Description As String Implements IEstimate.Description

        Public Property Discount As Double Implements IEstimate.Discount

        <IgnoreColumn> _
        Public Property DisplayName As String Implements IEstimate.DisplayName

        <IgnoreColumn> _
        Public Property DisplayTitle As String Implements IEstimate.DisplayTitle

        <IgnoreColumn> _
        Public Property EstDiscount As Decimal Implements IEstimate.EstDiscount

        Public Property EstimateItemId As Integer Implements IEstimate.EstimateItemId

        <IgnoreColumn> _
        Public Property EstimateId As Integer Implements IEstimate.EstimateId

        <IgnoreColumn> _
        Public Property EstimateTitle As String Implements IEstimate.EstimateTitle

        <IgnoreColumn> _
        Public Property EstProdOriginalPrice As String Implements IEstimate.EstProdOriginalPrice

        <IgnoreColumn> _
        Public Property EstProdPrice As String Implements IEstimate.EstProdPrice

        <IgnoreColumn> _
        Public Property ExtendedAmount As Single Implements IEstimate.ExtendedAmount

        <IgnoreColumn> _
        Public Property Finan_Cost As String Implements IEstimate.Finan_Cost

        <IgnoreColumn> _
        Public Property Finan_Rep As String Implements IEstimate.Finan_Rep

        <IgnoreColumn> _
        Public Property Finan_SalesPerson As String Implements IEstimate.Finan_SalesPerson

        <IgnoreColumn> _
        Public Property Finan_Tech As String Implements IEstimate.Finan_Tech

        <IgnoreColumn> _
        Public Property Finan_Telemarketing As String Implements IEstimate.Finan_Telemarketing

        <IgnoreColumn> _
        Public Property Guid As String Implements IEstimate.Guid

        <IgnoreColumn> _
        Public Property HistoryText As String Implements IEstimate.HistoryText

        <IgnoreColumn> _
        Public Property Inst As String Implements IEstimate.Inst

        <IgnoreColumn> _
        Public Property IsDeleted As Boolean Implements IEstimate.IsDeleted

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IEstimate.Locked

        <IgnoreColumn> _
        Public Property ModifiedByUser As Integer Implements IEstimate.ModifiedByUser

        <IgnoreColumn> _
        Public Property ModifiedOnDate As Date Implements IEstimate.ModifiedOnDate

        <IgnoreColumn> _
        Public Property MsgText As String Implements IEstimate.MsgText

        <IgnoreColumn> _
        Public Property PayCondDisc As Double Implements IEstimate.PayCondDisc

        <IgnoreColumn> _
        Public Property PayCondIn As Single Implements IEstimate.PayCondIn

        <IgnoreColumn> _
        Public Property PayCondInst As Single Implements IEstimate.PayCondInst

        <IgnoreColumn> _
        Public Property PayCondInterval As Integer Implements IEstimate.PayCondInterval

        <IgnoreColumn> _
        Public Property PayCondN As Integer Implements IEstimate.PayCondN

        <IgnoreColumn> _
        Public Property PayCondPerc As Double Implements IEstimate.PayCondPerc

        <IgnoreColumn> _
        Public Property PayCondType As String Implements IEstimate.PayCondType

        <IgnoreColumn> _
        Public Property PayForm As Integer Implements IEstimate.PayForm

        <IgnoreColumn> _
        Public Property PayOption As Integer Implements IEstimate.PayOption

        <IgnoreColumn> _
        Public Property PortalId As Integer Implements IEstimate.PortalId

        <IgnoreColumn> _
        Public Property POSels As String Implements IEstimate.POSels

        <IgnoreColumn> _
        Public Property POSelsText As String Implements IEstimate.POSelsText

        <IgnoreColumn> _
        Public Property Price As String Implements IEstimate.Price

        <IgnoreColumn> _
        Public Property ProdBarCode As String Implements IEstimate.ProdBarCode

        <IgnoreColumn> _
        Public Property ProdDesc As String Implements IEstimate.ProdDesc

        <IgnoreColumn> _
        Public Property ProdId As Integer Implements IEstimate.ProdId

        <IgnoreColumn> _
        Public Property ProdImageBinary As Byte() Implements IEstimate.ProdImageBinary

        <IgnoreColumn> _
        Public Property ProdImagesId As Integer Implements IEstimate.ProdImagesId

        <IgnoreColumn> _
        Public Property ProdImageUrl As String Implements IEstimate.ProdImageUrl

        <IgnoreColumn> _
        Public Property ProdIntro As String Implements IEstimate.ProdIntro

        <IgnoreColumn> _
        Public Property ProdName As String Implements IEstimate.ProdName

        <IgnoreColumn> _
        Public Property ProdRef As String Implements IEstimate.ProdRef

        <IgnoreColumn> _
        Public Property ProdUnit As Integer Implements IEstimate.ProdUnit

        Public Property Qty As String Implements IEstimate.Qty

        Public Property RemoveReasonId As String Implements IEstimate.RemoveReasonId

        <IgnoreColumn> _
        Public Property SaleEndDate As Date Implements IEstimate.SaleEndDate

        <IgnoreColumn> _
        Public Property SalesRep As Integer Implements IEstimate.SalesRep

        <IgnoreColumn> _
        Public Property SalesRepName As String Implements IEstimate.SalesRepName

        <IgnoreColumn> _
        Public Property SaleStartDate As Date Implements IEstimate.SaleStartDate

        <IgnoreColumn> _
        Public Property ShowPrice As Boolean Implements IEstimate.ShowPrice

        <IgnoreColumn> _
        Public Property Specs As String Implements IEstimate.Specs

        <IgnoreColumn> _
        Public Property StatusColor As String Implements IEstimate.StatusColor

        <IgnoreColumn> _
        Public Property StatusId As Integer Implements IEstimate.StatusId

        <IgnoreColumn> _
        Public Property StatusTitle As String Implements IEstimate.StatusTitle

        <IgnoreColumn> _
        Public Property Stock As Integer Implements IEstimate.Stock

        <IgnoreColumn> _
        Public Property Total As Single Implements IEstimate.Total

        <IgnoreColumn> _
        Public Property TotalAmount As Single Implements IEstimate.TotalAmount

        <IgnoreColumn> _
        Public Property TotalPayCond As Single Implements IEstimate.TotalPayCond

        <IgnoreColumn> _
        Public Property TotalPayments As Single Implements IEstimate.TotalPayments

        <IgnoreColumn> _
        Public Property TotalRows As Integer Implements IEstimate.TotalRows

        <IgnoreColumn> _
        Public Property UnitAbv As String Implements IEstimate.UnitAbv

        <IgnoreColumn> _
        Public Property UnitTypeId As Integer Implements IEstimate.UnitTypeId

        <IgnoreColumn> _
        Public Property UnitTypeTitle As String Implements IEstimate.UnitTypeTitle

        <IgnoreColumn> _
        Public Property UnitValue As Single Implements IEstimate.UnitValue

        <IgnoreColumn> _
        Public Property ViewPrice As Boolean Implements IEstimate.ViewPrice

    End Class
End Namespace