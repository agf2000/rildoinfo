
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_ClientIncomeSources")> _
    <PrimaryKey("ClientISId", AutoIncrement:=True)> _
    <Cacheable("ClientIncomeSources", CacheItemPriority.Default, 20)> _
    <Scope("ClientId")>
    Public Class ClientIncomeSource
        Implements IClientIncomeSource

#Region " Private variables "

        Public Property ClientId As Integer Implements IClientIncomeSource.ClientId

        Public Property ClientISId As Integer Implements IClientIncomeSource.ClientISId

        Public Property ISAddress As String Implements IClientIncomeSource.ISAddress

        Public Property ISAddressUnit As String Implements IClientIncomeSource.ISAddressUnit

        Public Property ISCity As String Implements IClientIncomeSource.ISCity

        Public Property ISComplement As String Implements IClientIncomeSource.ISComplement

        Public Property ISCT As String Implements IClientIncomeSource.ISCT

        Public Property ISDistrict As String Implements IClientIncomeSource.ISDistrict

        Public Property ISEIN As String Implements IClientIncomeSource.ISEIN

        Public Property ISFax As String Implements IClientIncomeSource.ISFax

        Public Property ISIncome As Single Implements IClientIncomeSource.ISIncome

        Public Property ISName As String Implements IClientIncomeSource.ISName

        Public Property ISPhone As String Implements IClientIncomeSource.ISPhone

        Public Property ISPostalCode As String Implements IClientIncomeSource.ISPostalCode

        Public Property ISProof As Boolean Implements IClientIncomeSource.ISProof

        Public Property ISRegion As String Implements IClientIncomeSource.ISRegion

        Public Property ISST As String Implements IClientIncomeSource.ISST

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IClientIncomeSource.Locked

#End Region

    End Class
End Namespace