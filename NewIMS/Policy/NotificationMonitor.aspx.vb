Imports System.IO

Public Class NotificationMonitor
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Check if user is admin
        'If Not User.IsInRole("Admin") Then
        '    Response.Redirect("~/AccessDenied.aspx")
        'End If
    End Sub

    Protected Sub BtnCheckNow_Click(sender As Object, e As EventArgs)
        Try
            ' Manually trigger notification check
            'NotificationService.CheckNow()
            lblStatus.Text = "Notification check triggered successfully."
        Catch ex As Exception
            lblStatus.Text = "Error: " & ex.Message
        End Try
    End Sub

    Protected Sub BtnViewLogs_Click(sender As Object, e As EventArgs)
        Dim logPath As String = Server.MapPath("~/Logs/")

        If Directory.Exists(logPath) Then
            Dim logFiles = Directory.GetFiles(logPath, "*.log")

            Dim logData As New DataTable()
            logData.Columns.Add("File Name")
            logData.Columns.Add("Last Modified")
            logData.Columns.Add("Size (KB)")

            For Each file In logFiles
                Dim fileInfo As New FileInfo(file)
                logData.Rows.Add(
                    fileInfo.Name,
                    fileInfo.LastWriteTime,
                    (fileInfo.Length / 1024).ToString("N2")
                )
            Next

            gvLogs.DataSource = logData
            gvLogs.DataBind()
            gvLogs.Visible = True
        Else
            lblStatus.Text = "Logs folder not found."
        End If
    End Sub

End Class