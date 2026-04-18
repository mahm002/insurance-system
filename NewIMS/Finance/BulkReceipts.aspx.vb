Public Class BULK
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        GridData.DataBind()
    End Sub

End Class