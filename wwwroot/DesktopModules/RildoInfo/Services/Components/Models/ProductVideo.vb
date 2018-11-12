
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_ProductVideo")> _
    <PrimaryKey("VideoId", AutoIncrement:=True)> _
    <Cacheable("ProductVideos", CacheItemPriority.Default, 20)> _
    <Scope("ProductId")>
    Public Class ProductVideo
        Implements IProductVideo

        Public Property Alt As String Implements IProductVideo.Alt

        Public Property AutoStart As Boolean Implements IProductVideo.AutoStart

        Public Property height As Integer Implements IProductVideo.height

        Public Property MediaAlignment As Integer Implements IProductVideo.MediaAlignment

        Public Property MediaDesc As String Implements IProductVideo.MediaDesc

        Public Property MediaLoop As Boolean Implements IProductVideo.MediaLoop

        Public Property MediaType As Integer Implements IProductVideo.MediaType

        Public Property ModuleId As Integer Implements IProductVideo.ModuleId

        Public Property NavigateUrl As String Implements IProductVideo.NavigateUrl

        Public Property NewWindow As Boolean Implements IProductVideo.NewWindow

        Public Property ProductId As Integer Implements IProductVideo.ProductId

        Public Property Src As String Implements IProductVideo.Src

        Public Property TrackClicks As Boolean Implements IProductVideo.TrackClicks

        Public Property VideoId As Integer Implements IProductVideo.VideoId

        Public Property width As Integer Implements IProductVideo.width
    End Class
End Namespace