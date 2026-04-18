Imports System
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Data
Imports System.Data.SqlClient

Public Class AccountHandler : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        Dim serializer As New JavaScriptSerializer()

        Try
            Dim jsonString As String = New System.IO.StreamReader(context.Request.InputStream).ReadToEnd()
            If String.IsNullOrEmpty(jsonString) Then
                Throw New Exception("No data received")
            End If

            Dim requestData As Dictionary(Of String, Object) = serializer.Deserialize(Of Dictionary(Of String, Object))(jsonString)

            Dim action As String = requestData("action").ToString()

            Select Case action
                Case "getAccounts"
                    GetAccounts(context, requestData, serializer)
                Case Else
                    Throw New Exception("Unknown action: " & action)
            End Select

        Catch ex As Exception
            context.Response.Write(serializer.Serialize(New With {
                .success = False,
                .message = "Error: " & ex.Message
            }))
        End Try
    End Sub

    Private Sub GetAccounts(ByVal context As HttpContext,
                           ByVal requestData As Dictionary(Of String, Object),
                           ByVal serializer As JavaScriptSerializer)
        Dim dataSource As String = requestData("dataSource").ToString()
        Dim branch As String = If(requestData.ContainsKey("branch"), requestData("branch").ToString(), "")

        Dim connStr As String = System.Configuration.ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString

        Dim query As String = ""

        Select Case dataSource
            Case "BankAccounts"
                query = "select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level>=4 and (left(AccountNo,7)='1.1.1.2')"
            Case "Accounts"
                query = "select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level>=4 and (left(AccountNo,5)='1.1.3')"
            Case "AccountsNotPayed"
                query = "select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level>=5 and (left(ltrim(AccountNo),8)='1.1.10.1')"
            Case Else
                Throw New Exception("Invalid data source: " & dataSource)
        End Select

        Using conn As New SqlConnection(connStr)
            conn.Open()

            Using cmd As New SqlCommand(query, conn)
                Dim accounts As New List(Of Object)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        accounts.Add(New With {
                            .Value = If(reader("AccountNo") IsNot DBNull.Value, reader("AccountNo").ToString(), ""),
                            .Text = If(reader("AccountName") IsNot DBNull.Value, reader("AccountName").ToString(), "")
                        })
                    End While
                End Using

                context.Response.Write(serializer.Serialize(New With {
                    .success = True,
                    .accounts = accounts
                }))
            End Using
        End Using
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class