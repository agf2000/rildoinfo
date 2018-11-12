
Namespace Components.Interfaces.Repositories

    Public Interface ISettingsRepository

        ''' <summary>
        ''' Gets a list of settings by portal id and culture code
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="cultureCode">Culture Code</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetAppSettings(portalId As Integer, cultureCode As String) As IEnumerable(Of Components.Models.Setting)

        ''' <summary>
        ''' Gets a list of settings by portal id and culture code
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetSettings(portalId As Integer) As Dictionary(Of String, String)

        ''' <summary>
        ''' Gets a setting by id
        ''' </summary>
        ''' <param name="settingId">Setting ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetSetting(settingId As Integer) As Components.Models.Setting

        ''' <summary>
        ''' Gets a setting by name
        ''' </summary>
        ''' <param name="settingName">Setting Name</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetSetting(settingName As String, portalId As Integer) As Components.Models.Setting

        ''' <summary>
        ''' Updates a setting
        ''' </summary>
        ''' <param name="setting">Setting Model</param>
        ''' <remarks></remarks>
        Sub UpdateSetting(setting As Components.Models.Setting)

        ''' <summary>
        ''' Adds a new setting
        ''' </summary>
        ''' <param name="setting">Setting Model</param>
        ''' <remarks></remarks>
        Function AddSetting(setting As Components.Models.Setting) As Components.Models.Setting

        ''' <summary>
        ''' Removes a setting by id
        ''' </summary>
        ''' <param name="settingId">Setting ID</param>
        ''' <remarks></remarks>
        Sub RemoveSetting(settingId As Integer)

        ''' <summary>
        ''' Removes a setting
        ''' </summary>
        ''' <param name="setting">Setting Model</param>
        ''' <remarks></remarks>
        Sub RemoveSetting(setting As Components.Models.Setting)

    End Interface

End Namespace
