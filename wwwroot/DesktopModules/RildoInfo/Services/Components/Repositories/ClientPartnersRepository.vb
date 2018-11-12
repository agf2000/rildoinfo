
Public Class ClientPartnersRepository
    Implements IClientPartnersRepository

    Public Function addClientPartner(clientPartner As Models.ClientPartner) As Models.ClientPartner Implements IClientPartnersRepository.addClientPartner
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientPartner) = ctx.GetRepository(Of Models.ClientPartner)()
            rep.Insert(clientPartner)
        End Using
        Return clientPartner
    End Function

    Public Function getClientPartner(clientPartnerId As Integer, personId As Integer) As Models.ClientPartner Implements IClientPartnersRepository.getClientPartner
        Dim clientPartner As Models.ClientPartner

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientPartner) = ctx.GetRepository(Of Models.ClientPartner)()
            clientPartner = rep.GetById(Of Int32, Int32)(clientPartnerId, personId)
        End Using
        Return clientPartner
    End Function

    Public Function getClientPartners(personId As Integer) As IEnumerable(Of Models.ClientPartner) Implements IClientPartnersRepository.getClientPartners
        Dim clientPartners As IEnumerable(Of Models.ClientPartner)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientPartner) = ctx.GetRepository(Of Models.ClientPartner)()
            clientPartners = rep.Get(personId)
        End Using
        Return clientPartners
    End Function

    Public Sub removeClientPartner(clientPartnerId As Integer, personId As Integer) Implements IClientPartnersRepository.removeClientPartner
        Dim _item As Models.ClientPartner = getClientPartner(clientPartnerId, personId)
        If _item.Locked Then
            Throw New ArgumentException("Exception Occured")
        Else
            removeClientPartner(_item)
        End If
    End Sub

    Public Sub removeClientPartner(clientPartner As Models.ClientPartner) Implements IClientPartnersRepository.removeClientPartner
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientPartner) = ctx.GetRepository(Of Models.ClientPartner)()
            rep.Delete(clientPartner)
        End Using
    End Sub

    Public Sub removeClientPartners(personId As Integer) Implements IClientPartnersRepository.removeClientPartners
        Dim clientPartners As IEnumerable(Of Models.ClientPartner)
        Using context As IDataContext = DataContext.Instance()
            Dim rep = context.GetRepository(Of Models.ClientPartner)()
            clientPartners = rep.Find("Where PersonId = @0", personId)

            For Each incomeSource In clientPartners
                removeClientPartner(incomeSource)
            Next
        End Using
    End Sub

    Public Sub updateClientPartner(clientPartner As Models.ClientPartner) Implements IClientPartnersRepository.updateClientPartner
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientPartner) = ctx.GetRepository(Of Models.ClientPartner)()
            rep.Update(clientPartner)
        End Using
    End Sub

End Class
