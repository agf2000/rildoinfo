Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Repositories

    Public Class SettingsRepository
        Implements ISettingsRepository

        Public Function AddSetting(setting As Setting) As Setting Implements ISettingsRepository.AddSetting
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Setting) = ctx.GetRepository(Of Setting)()
                rep.Insert(setting)
            End Using
            Return setting
        End Function

        Public Function GetSetting(settingId As Integer) As Setting Implements ISettingsRepository.GetSetting
            Dim setting As Setting

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Setting) = ctx.GetRepository(Of Setting)()
                setting = rep.GetById(Of Int32)(settingId)
            End Using
            Return setting
        End Function

        Public Function GetSetting(settingName As String, portalId As Integer) As Setting Implements ISettingsRepository.GetSetting
            Dim setting As Setting

            Using ctx As IDataContext = DataContext.Instance()
                setting = ctx.ExecuteSingleOrDefault(Of Setting)(CommandType.Text, "where SettingName = @0 and PortalId = @1", settingName, portalId)
            End Using
            Return setting
        End Function

        Public Function GetSettings(portalId As Integer) As Dictionary(Of String, String) Implements ISettingsRepository.GetSettings
            Dim settingsDictionay As New Dictionary(Of String, String)
            Dim settings As IEnumerable(Of Setting)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Setting) = ctx.GetRepository(Of Setting)()
                settings = rep.Get(portalId)
            End Using
            For Each setting In settings
                settingsDictionay.Add(setting.SettingName, setting.SettingValue)
            Next
            Return settingsDictionay
        End Function

        Public Function GetAppSettings(portalId As Integer, cultureCode As String) As IEnumerable(Of Setting) Implements ISettingsRepository.GetAppSettings
            Dim settings As IEnumerable(Of Setting)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Setting) = ctx.GetRepository(Of Setting)()
                settings = rep.Get(portalId)
            End Using
            Return settings
        End Function

        Public Sub RemoveSetting(settingId As Integer) Implements ISettingsRepository.RemoveSetting
            Dim item As Setting = GetSetting(settingId)
            RemoveSetting(item)
        End Sub

        Public Sub RemoveSetting(setting As Setting) Implements ISettingsRepository.RemoveSetting
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Setting) = ctx.GetRepository(Of Setting)()
                rep.Delete(setting)
            End Using
        End Sub

        Public Sub UpdateSetting(setting As Setting) Implements ISettingsRepository.UpdateSetting
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Setting) = ctx.GetRepository(Of Setting)()
                rep.Update(setting)
            End Using
        End Sub

    End Class

End Namespace
