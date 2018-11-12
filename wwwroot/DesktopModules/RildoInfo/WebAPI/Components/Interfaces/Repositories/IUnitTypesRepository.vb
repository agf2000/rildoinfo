Namespace Components.Interfaces.Repositories

    Public Interface IUnitTypesRepository

        ''' <summary>
        ''' Adds a new unitType
        ''' </summary>
        ''' <param name="unitType">UnitType Model</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddUnitType(unitType As Components.Models.UnitType) As Components.Models.UnitType

        ''' <summary>
        ''' Gets a list of unitTypes by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="isDeleted"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetUnitTypes(portalId As Integer, isDeleted As String) As IEnumerable(Of Components.Models.UnitType)

        ''' <summary>
        ''' Gets unitType
        ''' </summary>
        ''' <param name="unitTypeId">UnitType ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetUnitType(unitTypeId As Integer, portalId As Integer) As Components.Models.UnitType

        ''' <summary>
        ''' Gets a unit by abreviation
        ''' </summary>
        ''' <param name="unitTypeAbbv">Unit Type Abreviation</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetUnitType(unitTypeAbbv As String, portalId As Integer) As Components.Models.UnitType

        ''' <summary>
        ''' Updates unitType
        ''' </summary>
        ''' <param name="unitType">UnitType Model</param>
        ''' <remarks></remarks>
        Sub UpdateUnitType(unitType As Components.Models.UnitType)

        ''' <summary>
        ''' Removes unitType
        ''' </summary>
        ''' <param name="unitTypeId">UnitType ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <remarks></remarks>
        Sub RemoveUnitType(unitTypeId As Integer, portalId As Integer)

        ''' <summary>
        ''' Removes unitType
        ''' </summary>
        ''' <param name="unitType">UnitType Model</param>
        ''' <remarks></remarks>
        Sub RemoveUnitType(unitType As Components.Models.UnitType)

    End Interface

End Namespace
