
Public Interface IUnitTypesRepository

    ''' <summary>
    ''' Adds a new unitType
    ''' </summary>
    ''' <param name="unitType">UnitType Model</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function addUnitType(unitType As Models.UnitType) As Models.UnitType

    ''' <summary>
    ''' Gets a list of unitTypes by portal id
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="isDeleted"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getUnitTypes(portalId As Integer, isDeleted As String) As IEnumerable(Of Models.UnitType)

    ''' <summary>
    ''' Gets unitType
    ''' </summary>
    ''' <param name="unitTypeId">UnitType ID</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getUnitType(unitTypeId As Integer, portalId As Integer) As Models.UnitType

    ''' <summary>
    ''' Updates unitType
    ''' </summary>
    ''' <param name="unitType">UnitType Model</param>
    ''' <remarks></remarks>
    Sub updateUnitType(unitType As Models.UnitType)

    ''' <summary>
    ''' Removes unitType
    ''' </summary>
    ''' <param name="unitTypeId">UnitType ID</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <remarks></remarks>
    Sub removeUnitType(unitTypeId As Integer, portalId As Integer)

    ''' <summary>
    ''' Removes unitType
    ''' </summary>
    ''' <param name="unitType">UnitType Model</param>
    ''' <remarks></remarks>
    Sub removeUnitType(unitType As Models.UnitType)

End Interface
