
Public Class ApiResponse(Of T)
    Public Property code As Integer
    Public Property status As Boolean
    Public Property message As String
    Public Property [data] As T
End Class