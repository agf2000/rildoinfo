
Public Class Constants

    Public Const PRODUCTIMAGESFOLDER As String = "productimages"
    Public Const PRODUCTDOCSFOLDER As String = "productdocs"
    Public Const PRODUCTTHUMBSFOLDER As String = "productthumbs"
    Public Const ORDERUPLOADFOLDER As String = "orderuploads"
    Public Const TEMPLATEFOLDER As String = "Templates"

    Friend Enum Authorized
        CRUDAuthorized = 0
        AddAuthorization = 1
        EditAuthorization = 2
        DeleteAuthrization = 3
        NotAuthorized = 4
    End Enum

    Friend Enum ContentTypeName
        RIW
    End Enum

    Friend Enum NotificationEstimateTypeName
        RIW_Estimate_Entry
        RIW_Estimate_Updated
        RIW_Estimate_Notification
    End Enum

End Class
