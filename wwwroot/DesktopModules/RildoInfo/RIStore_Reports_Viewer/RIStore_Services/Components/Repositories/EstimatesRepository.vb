
Imports System.Collections.Generic
Imports DotNetNuke.Data

Namespace RI.Modules.RIStore_Services
    Public Class EstimatesRepository
        Implements IEstimatesRepository

        Public Function AddEstimate(estimate As Models.Estimate) As Models.Estimate Implements IEstimatesRepository.AddEstimate
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Estimate) = ctx.GetRepository(Of Models.Estimate)()
                rep.Insert(estimate)
            End Using
            Return estimate
        End Function

        Public Function AddEstimateItem(estimateItem As Models.EstimateItem) As Models.EstimateItem Implements IEstimatesRepository.AddEstimateItem
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateItem) = ctx.GetRepository(Of Models.EstimateItem)()
                rep.Insert(estimateItem)
            End Using
            Return estimateItem
        End Function

        Public Function GetEstimate(estimateId As Integer, portalId As Integer) As Models.Estimate Implements IEstimatesRepository.GetEstimate
            Dim estimate As Models.Estimate

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Estimate) = ctx.GetRepository(Of Models.Estimate)()
                estimate = rep.GetById(Of Int32, Int32)(estimateId, portalId)
            End Using
            Return estimate
        End Function

        Public Function GetEstimates(ByVal portalId As String, ByVal clientId As String, ByVal salesRep As String, ByVal SearchTerm As String, ByVal pageNumber As Integer, ByVal pageSize As Integer, ByVal orderBy As String) As IEnumerable(Of Models.Estimate) Implements IEstimatesRepository.GetEstimates
            Return CBO.FillCollection(Of Models.Estimate)(DataProvider.Instance().GetEstimates(portalId, clientId, salesRep, SearchTerm, pageNumber, pageSize, orderBy))
        End Function

        Public Function GetEstimateItems(ByVal eId As Integer) As IEnumerable(Of Models.EstimateItem) Implements IEstimatesRepository.GetEstimateItems
            Return CBO.FillCollection(Of Models.EstimateItem)(DataProvider.Instance().GetEstimateItems(eId))
        End Function

        Public Sub RemoveEstimate(estimateId As Integer, portalId As Integer) Implements IEstimatesRepository.RemoveEstimate
            Dim _item As Models.Estimate = GetEstimate(estimateId, portalId)
            RemoveEstimate(_item)
        End Sub

        Public Sub RemoveEstimate(estimate As Models.Estimate) Implements IEstimatesRepository.RemoveEstimate
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Estimate) = ctx.GetRepository(Of Models.Estimate)()
                rep.Delete(estimate)
            End Using
        End Sub

        Public Sub RemoveEstimates(clientId As Integer, portalId As Integer) Implements IEstimatesRepository.RemoveEstimates
            Dim estimates As IEnumerable(Of Models.Estimate)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of Models.Estimate)()
                estimates = rep.Find("Where ClientId = @0 And PortalId = @1 ", clientId, portalId)

                For Each estimate In estimates
                    RemoveEstimate(estimate)
                Next
            End Using
        End Sub

        Public Sub UpdateEstimate(estimate As Models.Estimate) Implements IEstimatesRepository.UpdateEstimate
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Estimate) = ctx.GetRepository(Of Models.Estimate)()
                rep.Update(estimate)
            End Using
        End Sub

        Public Function GetEstimateItem(estimateItemId As Integer, estimateId As Integer) As Models.EstimateItem Implements IEstimatesRepository.GetEstimateItem
            Dim estimateItem As Models.EstimateItem

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateItem) = ctx.GetRepository(Of Models.EstimateItem)()
                estimateItem = rep.GetById(Of Int32, Int32)(estimateItemId, estimateId)
            End Using
            Return estimateItem
        End Function

        Public Sub RemoveEstimateItem(estimateItemId As Integer, estimateId As Integer) Implements IEstimatesRepository.RemoveEstimateItem
            Dim _item As Models.EstimateItem = GetEstimateItem(estimateItemId, estimateId)
            RemoveEstimateItem(_item)
        End Sub

        Public Sub RemoveEstimateItem(estimateItem As Models.EstimateItem) Implements IEstimatesRepository.RemoveEstimateItem
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateItem) = ctx.GetRepository(Of Models.EstimateItem)()
                rep.Delete(estimateItem)
            End Using
        End Sub

        Public Function AddEstimateItemRemoved(estimateItem As Models.EstimateRemovedItem) As Models.EstimateRemovedItem Implements IEstimatesRepository.AddEstimateItemRemoved
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateRemovedItem) = ctx.GetRepository(Of Models.EstimateRemovedItem)()
                rep.Insert(estimateItem)
            End Using
            Return estimateItem
        End Function

        Public Function AddEstimateHistory(estimateHistory As Models.EstimateHistory) As Models.EstimateHistory Implements IEstimatesRepository.AddEstimateHistory
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateHistory) = ctx.GetRepository(Of Models.EstimateHistory)()
                rep.Insert(estimateHistory)
            End Using
            Return estimateHistory
        End Function

    End Class
End Namespace