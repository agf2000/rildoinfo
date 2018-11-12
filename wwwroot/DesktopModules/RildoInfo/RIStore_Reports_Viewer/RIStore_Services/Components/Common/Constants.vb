
Namespace RI.Modules.RIStore_Services

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
            RIStore_Estimate
        End Enum

        Friend Enum NotificationEstimateTypeName
            RIStore_Estimate_Entry
            RIStore_Estimate_Updated
            RIStore_Estimate_Notification
        End Enum

    End Class

End Namespace