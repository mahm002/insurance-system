Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Web.SessionState
Imports System.Configuration

Public Class NotificationsHandler
    Implements IHttpHandler, IRequiresSessionState

    Private ReadOnly connectionString As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString

    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        Try
            ' 1. Authentication
            Dim userId = context.Session("UserId")?.ToString()
            If String.IsNullOrEmpty(userId) Then
                RespondError(context, 401, "Unauthorized")
                Return
            End If

            ' 2. Route request
            Dim method = context.Request.HttpMethod.ToUpperInvariant()
            Dim id = context.Request.QueryString("id")

            Select Case method
                Case "GET"
                    HandleGet(context, userId)
                Case "POST"
                    If Not String.IsNullOrEmpty(id) Then
                        HandlePost(context, userId, id)
                    Else
                        RespondError(context, 400, "Missing notification ID")
                    End If
                Case Else
                    RespondError(context, 405, "Method not allowed")
            End Select

        Catch ex As Exception
            ' Log the exception (write to Windows Event Log or a text file)
            LogError(ex)
            RespondError(context, 500, "Internal server error")
        End Try
    End Sub

    Private Sub HandleGet(context As HttpContext, userId As String)
        Dim sql = "SELECT Id, [Message], GeneratedBy, [Timestamp], IsRead, [Type] 
                   FROM Notifications 
                   WHERE UserId = @UserId and IsTreated = 0
                   ORDER BY [Timestamp] DESC"

        Dim notifications As New List(Of NotificationItem)

        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@UserId", userId)
                conn.Open()
                Using reader = cmd.ExecuteReader()
                    While reader.Read()
                        notifications.Add(New NotificationItem With {
                            .Id = reader("Id").ToString(),
                            .Message = reader("Message").ToString(),
                            .GeneratedBy = reader("GeneratedBy").ToString(),
                            .Timestamp = Convert.ToDateTime(reader("Timestamp")),
                            .IsRead = Convert.ToBoolean(reader("IsRead")),
                            .Type = reader("Type").ToString()
                        })
                    End While
                End Using
            End Using
        End Using

        ' Return JSON
        context.Response.ContentType = "application/json"
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Dim serializer As New JavaScriptSerializer()
        context.Response.Write(serializer.Serialize(notifications))
    End Sub

    Private Sub HandlePost(context As HttpContext, userId As String, id As String)
        Dim sql = "UPDATE Notifications SET IsRead = 1 WHERE Id = @Id AND UserId = @UserId"
        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Id", id)
                cmd.Parameters.AddWithValue("@UserId", userId)
                conn.Open()
                Dim rowsAffected = cmd.ExecuteNonQuery()
                If rowsAffected = 0 Then
                    RespondError(context, 404, "Notification not found")
                    Return
                End If
            End Using
        End Using

        context.Response.StatusCode = 200
        context.Response.Write("OK")
    End Sub

    Private Sub RespondError(context As HttpContext, statusCode As Integer, message As String)
        context.Response.StatusCode = statusCode
        context.Response.ContentType = "application/json"
        Dim serializer As New JavaScriptSerializer()
        context.Response.Write(serializer.Serialize(New With {.error = message}))
    End Sub

    Private Sub LogError(ex As Exception)
        ' Write to Windows Event Log or a log file
        Try
            EventLog.WriteEntry("NotificationsHandler", ex.ToString(), EventLogEntryType.Error)
        Catch
            ' If event log fails, write to a file (optional)
            'System.IO.File.AppendAllText(context.Server.MapPath("~/App_Data/ErrorLog.txt"), ex.ToString() & vbCrLf)
        End Try
    End Sub

    Public ReadOnly Property IsReusable As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False ' Keep false to avoid unexpected state issues
        End Get
    End Property
End Class

Public Class NotificationItem
    Public Property Id As String
    Public Property Message As String
    Public Property GeneratedBy As String
    Public Property Timestamp As DateTime
    Public Property IsRead As Boolean
    Public Property Type As String
End Class