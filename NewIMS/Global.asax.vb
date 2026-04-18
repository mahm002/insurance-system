Public Class Global_asax
    Inherits HttpApplication

    Public Sub Application_Start(sender As Object, e As EventArgs)
        Try
            BackgroundNotificationService.Start()
            NotificationService.Initialize()
            Application("NotificationServiceRunning") = True
        Catch ex As Exception
            ' Log to a file (use a simple method, or Windows Event Log)
            IO.File.AppendAllText(
                IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "StartupErrors.log"),
                $"{DateTime.Now} - {ex}{Environment.NewLine}"
            )
        End Try

        ' Register jQuery for unobtrusive validation
        Dim jQueryScript As New ScriptResourceDefinition
        jQueryScript.Path = "~/Scripts/jquery-3.7.1.min.js"
        jQueryScript.DebugPath = "~/Scripts/jquery-3.7.1.js"
        jQueryScript.CdnPath = "https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"
        jQueryScript.CdnDebugPath = "https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.js"
        jQueryScript.CdnSupportsSecureConnection = True

        ScriptManager.ScriptResourceMapping.AddDefinition("jquery", jQueryScript)
    End Sub

    Public Sub Session_Start(sender As Object, e As EventArgs)
        ' Your session logic – but be careful with redirects
        'If Session("LogInfo") Is Nothing Then
        '    FormsAuthentication.RedirectToLoginPage()
        'End If
    End Sub

    Public Sub Application_End(sender As Object, e As EventArgs)
        Application("NotificationServiceRunning") = False
        BackgroundNotificationService.Stop()
    End Sub

End Class