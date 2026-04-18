Partial Public Class SystemManag_ErrPage
    Inherits Page

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles form1.Load
        'Dim objErr As Exception = Server.GetLastError().GetBaseException()
        'Dim err As String = "Error Caught in Application_Error event\n" + _
        '        "Error in: " + Request.Url.ToString() + _
        '        "\nError Message:" + objErr.Message.ToString() + _
        '        "\nStack Trace:" + objErr.StackTrace.ToString()
        'EventLog.WriteEntry("Sample_WebApp", err, EventLogEntryType.Error)
        'Server.ClearError()
        Label1.Text = Err.Description
    End Sub

End Class