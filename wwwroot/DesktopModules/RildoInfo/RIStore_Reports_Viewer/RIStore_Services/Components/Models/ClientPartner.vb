
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_ClientPartners")> _
    <PrimaryKey("ClientPartnerId", AutoIncrement:=True)> _
    <Cacheable("ClientPartners", CacheItemPriority.Default, 20)> _
    <Scope("ClientId")>
    Public Class ClientPartner
        Implements IClientPartner

#Region " Private variables "

        Public Property ClientId As Integer Implements IClientPartner.ClientId

        Public Property ClientPartnerId As Integer Implements IClientPartner.ClientPartnerId

        Public Property PartnerAddress As String Implements IClientPartner.PartnerAddress

        Public Property PartnerAddressUnit As String Implements IClientPartner.PartnerAddressUnit

        Public Property PartnerCell As String Implements IClientPartner.PartnerCell

        Public Property PartnerCity As String Implements IClientPartner.PartnerCity

        Public Property PartnerComplement As String Implements IClientPartner.PartnerComplement

        Public Property PartnerCPF As String Implements IClientPartner.PartnerCPF

        Public Property PartnerDistrict As String Implements IClientPartner.PartnerDistrict

        Public Property PartnerEmail As String Implements IClientPartner.PartnerEmail

        Public Property PartnerIdentity As String Implements IClientPartner.PartnerIdentity

        Public Property PartnerName As String Implements IClientPartner.PartnerName

        Public Property PartnerPhone As String Implements IClientPartner.PartnerPhone

        Public Property PartnerPostalCode As String Implements IClientPartner.PartnerPostalCode

        Public Property PartnerQuota As Double Implements IClientPartner.PartnerQuota

        Public Property PartnerRegion As String Implements IClientPartner.PartnerRegion

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IClientPartner.Locked

#End Region

    End Class
End Namespace
