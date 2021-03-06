﻿Namespace Components.Interfaces.Models
    Interface IClientPartner

        Property ClientPartnerId As Integer

        Property PersonId As Integer

        Property PartnerName As String

        Property PartnerCPF As String

        Property PartnerIdentity As String

        Property PartnerPhone As String

        Property PartnerCell As String

        Property PartnerEmail As String

        Property PartnerQuota As Double

        Property PartnerPostalCode As String

        Property PartnerAddress As String

        Property PartnerAddressUnit As String

        Property PartnerComplement As String

        Property PartnerDistrict As String

        Property PartnerRegion As String

        Property PartnerCity As String

        Property PartnerCountry As String

        Property Locked As Boolean
    End Interface
End Namespace