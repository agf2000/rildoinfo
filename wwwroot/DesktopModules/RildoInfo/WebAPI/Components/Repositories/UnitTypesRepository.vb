Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
 
Namespace Components.Repositories

    Public Class UnitTypesRepository
    Implements IUnitTypesRepository

        Public Function AddUnitType(unitType As Components.Models.UnitType) As Components.Models.UnitType Implements IUnitTypesRepository.AddUnitType
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.UnitType) = ctx.GetRepository(Of Components.Models.UnitType)()
                rep.Insert(unitType)
            End Using
            Return unitType
        End Function

        Public Function GetUnitType(unitTypeId As Integer, portalId As Integer) As Components.Models.UnitType Implements IUnitTypesRepository.GetUnitType
            Dim unitType As Components.Models.UnitType

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.UnitType) = ctx.GetRepository(Of Components.Models.UnitType)()
                unitType = rep.GetById(Of Int32, Int32)(unitTypeId, portalId)
            End Using
            Return unitType
        End Function

        Public Function GetUnitType(unitTypeAbbv As String, portalId As Integer) As Components.Models.UnitType Implements IUnitTypesRepository.GetUnitType
            Dim unitType As Components.Models.UnitType

            Using ctx As IDataContext = DataContext.Instance()
                unitType = ctx.ExecuteSingleOrDefault(Of Components.Models.UnitType)(CommandType.Text, "where UnitTypeAbbv = @0 and PortalId = @1", unitTypeAbbv, portalId)
            End Using
            Return unitType
        End Function

        Public Function GetUnitTypes(portalId As Integer, isDeleted As String) As IEnumerable(Of Components.Models.UnitType) Implements IUnitTypesRepository.GetUnitTypes
            Dim unitTypes As IEnumerable(Of Components.Models.UnitType)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.UnitType) = ctx.GetRepository(Of Components.Models.UnitType)()
                unitTypes = rep.Find("Where PortalId = @0 AND IsDeleted = @1", portalId, isDeleted)
            End Using
            Return unitTypes
        End Function

        Public Sub RemoveUnitType(unitTypeId As Integer, portalId As Integer) Implements IUnitTypesRepository.RemoveUnitType
            Dim unitType As Components.Models.UnitType = GetUnitType(unitTypeId, portalId)
            RemoveUnitType(unitType)
        End Sub

        Public Sub RemoveUnitType(unitType As Components.Models.UnitType) Implements IUnitTypesRepository.RemoveUnitType
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.UnitType) = ctx.GetRepository(Of Components.Models.UnitType)()
                rep.Delete(unitType)
            End Using
        End Sub

        Public Sub UpdateUnitType(unitType As Components.Models.UnitType) Implements IUnitTypesRepository.UpdateUnitType
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.UnitType) = ctx.GetRepository(Of Components.Models.UnitType)()
                rep.Update(unitType)
            End Using
        End Sub

        'Public Function GetUnitTypeByName(unitName As String, portalId As Integer) As Models.UnitType Implements IUnitTypesRepository.getUnitTypeByName
        '    Dim unit As Models.UnitType

        '    Using ctx As IDataContext = DataContext.Instance()
        '        unit = ctx.ExecuteSingleOrDefault(Of Models.UnitType)(CommandType.Text, "select * from riw_unittypes where unittypeabbv = @0 AND portalid = @1", unitName, portalId)
        '    End Using
        '    Return unit
        'End Function
    End Class

End Namespace
