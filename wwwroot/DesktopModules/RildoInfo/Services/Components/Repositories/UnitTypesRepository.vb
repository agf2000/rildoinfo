
Public Class UnitTypesRepository
    Implements IUnitTypesRepository

    Public Function addUnitType(unitType As Models.UnitType) As Models.UnitType Implements IUnitTypesRepository.addUnitType
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.UnitType) = ctx.GetRepository(Of Models.UnitType)()
            rep.Insert(unitType)
        End Using
        Return unitType
    End Function

    Public Function getUnitType(unitTypeId As Integer, portalId As Integer) As Models.UnitType Implements IUnitTypesRepository.getUnitType
        Dim unitType As Models.UnitType

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.UnitType) = ctx.GetRepository(Of Models.UnitType)()
            unitType = rep.GetById(Of Int32, Int32)(unitTypeId, portalId)
        End Using
        Return unitType
    End Function

    Public Function getUnitTypes(portalId As Integer, isDeleted As String) As IEnumerable(Of Models.UnitType) Implements IUnitTypesRepository.getUnitTypes
        Dim unitTypes As IEnumerable(Of Models.UnitType)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.UnitType) = ctx.GetRepository(Of Models.UnitType)()
            unitTypes = rep.Find("Where PortalId = @0 AND IsDeleted = @1", portalId, isDeleted)
        End Using
        Return unitTypes
    End Function

    Public Sub removeUnitType(unitTypeId As Integer, portalId As Integer) Implements IUnitTypesRepository.removeUnitType
        Dim unitType As Models.UnitType = getUnitType(unitTypeId, portalId)
        removeUnitType(unitType)
    End Sub

    Public Sub removeUnitType(unitType As Models.UnitType) Implements IUnitTypesRepository.removeUnitType
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.UnitType) = ctx.GetRepository(Of Models.UnitType)()
            rep.Delete(unitType)
        End Using
    End Sub

    Public Sub updateUnitType(unitType As Models.UnitType) Implements IUnitTypesRepository.updateUnitType
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.UnitType) = ctx.GetRepository(Of Models.UnitType)()
            rep.Update(unitType)
        End Using
    End Sub

End Class
