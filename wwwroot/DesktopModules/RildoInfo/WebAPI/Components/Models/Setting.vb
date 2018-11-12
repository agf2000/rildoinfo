
Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_Settings")> _
    <PrimaryKey("AppSettingId", AutoIncrement:=True)> _
    <Cacheable("Settings", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class Setting
        Implements ISetting

        Public Property AppSettingId As Integer Implements ISetting.AppSettingId

        Public Property CreatedByUser As Integer Implements ISetting.CreatedByUser

        Public Property CreatedOnDate As Date Implements ISetting.CreatedOnDate

        Public Property CultureCode As String Implements ISetting.CultureCode

        Public Property ModifiedByUser As Integer Implements ISetting.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements ISetting.ModifiedOnDate

        Public Property PortalId As Integer Implements ISetting.PortalId

        Public Property SettingName As String Implements ISetting.SettingName

        Public Property SettingValue As String Implements ISetting.SettingValue
    End Class
End Namespace
