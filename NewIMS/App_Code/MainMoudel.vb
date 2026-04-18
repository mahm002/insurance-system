Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports DevExpress.Web
Imports Microsoft.Reporting.WebForms
Imports Newtonsoft.Json
Imports System.Linq
Imports System.Web.Caching
Imports System.Web.Hosting
Public Module MainMoudel

    Public Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString)
    Public Conn1 As New SqlConnection(ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString)
    Public AccConn As New SqlConnection(ConfigurationManager.ConnectionStrings("IMSAccConnectionString").ConnectionString)
    Public AttConn As New SqlConnection(ConfigurationManager.ConnectionStrings("AttachConString").ConnectionString)
    Public Parm As Array
    Public ReportsPath As String = ConfigurationManager.AppSettings("ReportsPath").ToString
    Public OrderNo As String

    'Public parameters As New List(Of ReportParameter)
    Public CurUser As String

    Public stm As Double
    Public LogRec As DataSet

    'Public NewCover As Date = Today.Date
    Public LogInfo(11) As String

    Public UserInfo As String

    Public Class Account
        Public Property AccountNo As String
        Public Property AccountName As String
        Public Property ParentAcc As String
        Public Property FullPath As String
        Public Property Level As Integer
        Public Property TranscationAcc As Boolean
    End Class

    Public Function HashPassword(password As String) As String
        Dim hashedPassword As String = BCrypt.Net.BCrypt.HashPassword(password)
        Return hashedPassword
    End Function

    Public Sub SendSMS(S As String, msgg As String)
        If S = "0" Or Left(S, 5) = "21895" Or Left(S, 5) = "21890" Or Left(S, 5) = "21899" Or Left(S, 5) = "21896" Or Left(S, 5) = "21897" Or Left(S, 5) = "21898" Or S = "218911111111" Then Exit Sub
        Dim SMSapiUrl As String = ConfigurationManager.AppSettings("SMSApiUrl").ToString
        Dim apikey As String = ConfigurationManager.AppSettings("SMSTokenKey").ToString
        Dim SenderName As String = ConfigurationManager.AppSettings("Sender_id").ToString

        Dim recipient = S.ToString.Trim

        Dim json As String = JsonConvert.SerializeObject(msgg)
        json = json.Replace("\r\n", "\n")

        Dim query As String = """api_token"": """ & apikey & """,""recipient"": """ & recipient & """, ""sender_id"": """ & SenderName & """, ""type"": ""plain"", ""message"": " & json & " "

        ServicePointManager.Expect100Continue = True
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim request As WebRequest = WebRequest.Create(SMSapiUrl)

        request.Method = "POST"
        'request.Headers.Add("Authorization", "Bearer " & apikey)
        request.ContentType = "application/json"
        'request.Headers("Accept") = "application/json"
        'request.Headers.Add("Accept", "application/json")

        Dim requestBody As String = "{" & query & "}"
        Dim byteArray As Byte() = Encoding.UTF8.GetBytes(requestBody)
        request.ContentLength = byteArray.Length

        Using dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
        End Using

        Try
            Dim response As WebResponse = request.GetResponse()
            Using dataStreamResponse As Stream = response.GetResponseStream()
                Using reader As New StreamReader(dataStreamResponse)
                    Dim responseFromServer As String = reader.ReadToEnd()
                    ''MsgBox("Job Response: " & responseFromServer)
                    recipient = S
                    ExecConn("Insert into DailySMS (TelNo) Values ('" & "00" & S.Trim & "')", Conn)
                End Using
            End Using
            response.Close()
        Catch ex As WebException
            ' MsgBox(ex.Message)
            ExecConn("Insert into DailySMS (TelNo, Error) Values ('" & "00" & S.Trim & "','" & ex.Message & "')", Conn)
        End Try
    End Sub

    Public Function Getaccname(Acc As String) As String
        Dim AccName As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("SELECT AccountName FROM Accounts where AccountNo='{0}'", Acc), con)
            Dim v = dbadapter.Fill(AccName)
            'GetName = RecSet("select BranchName from BranchInfo where BranchNo='" & SysNo & "'", Conn)
            Return If(AccName.Tables(0).Rows.Count <> 0, Trim(AccName.Tables(0).Rows(0)(0)), "الحساب")
            con.Close()
        End Using
    End Function

    Public Function GetCoverFrom(ByRef Order As String, ByRef EndNo As Integer, ByRef Sys As String) As Date
        Dim Days As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select Coverfrom From policyfile where OrderNo='{0}' and EndNo={1} and subins='{2}'", Order, EndNo, Sys), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(Days)
#Enable Warning IDE0058 ' Expression value is never used
            Return Days.Tables(0).Rows.Item(0).Item(0)
            con.Close()
        End Using
    End Function

    Public Function GetSysNet(Order As String, EndNo As Integer, LoadNo As Integer, Sys As String) As Double
        Dim netpremium As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()

            If Sys = "MC" Or Sys = "MB" Or Sys = "MA" Or Sys = "OC" Then
                If EndNo = 0 Then
                    Dim dbadapter = New SqlDataAdapter(String.Format("select round(isnull(sum(((([Marprm]+[ShipPrm])+[ExtraPrm])+[WarPrm])),0),3) As Prm From " & GetGroupFile(Sys) & " WHERE OrderNo='{0}' and EndNo={1} and LoadNo={2} and subins='{3}'", Order, EndNo, LoadNo, Sys), con)
                    dbadapter.Fill(netpremium)
                    Return netpremium.Tables(0).Rows.Item(0).Item(0)
                Else
                    Dim dbadapter = New SqlDataAdapter(String.Format("select NetPRM As Prm From PolicyFile WHERE OrderNo='{0}' and EndNo={1} and LoadNo={2} and subins='{3}'", Order, EndNo, LoadNo, Sys), con)
                    dbadapter.Fill(netpremium)
                    Return netpremium.Tables(0).Rows.Item(0).Item(0)
                End If
            Else

                Dim dbadapter = New SqlDataAdapter(String.Format("select round(isnull(sum(Premium),0),3) As Prm From " & GetGroupFile(Sys) & " WHERE OrderNo='{0}' and EndNo={1} and LoadNo={2} and subins='{3}'", Order, EndNo, LoadNo, Sys), con)
                dbadapter.Fill(netpremium)
                Return netpremium.Tables(0).Rows.Item(0).Item(0)
            End If

            con.Close()
        End Using
    End Function

    Public Function GetLastAccount(Acc As String) As Integer
        Dim MxAcc As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("SELECT top(1) max(cast(substring(AccountNo, (Len(ACCOUNTNO) - PATINDEX('%.%',REVERSE(ACCOUNTNO)))+2,LEN(ACCOUNTNO)) as bigint)) + 1 as MX " _
                    & " From Accounts " _
                    & " Where ParentAcc ='{0}' " _
                    & " Group by ACCOUNTNO " _
                    & " order by cast(substring(AccountNo,(LEN(ACCOUNTNO)-PATINDEX('%.%',REVERSE(ACCOUNTNO)))+2,LEN(ACCOUNTNO)) as bigint) desc", Acc), con)
            Dim unused = dbadapter.Fill(MxAcc)
            'GetName = RecSet("select BranchName from BranchInfo where BranchNo='" & SysNo & "'", Conn)

            Return If(MxAcc.Tables(0).Rows.Count = 0, 1, MxAcc.Tables(0).Rows(0)("MX"))
            con.Close()
        End Using
    End Function

    Public Function AccountLevel(Acc As String) As Short
        Dim Acclvl As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("SELECT level FROM Accounts where AccountNo='{0}'", Acc), con)
            Dim unused = dbadapter.Fill(Acclvl)
            'GetName = RecSet("select BranchName from BranchInfo where BranchNo='" & SysNo & "'", Conn)

            Return Trim(Acclvl.Tables(0).Rows(0)(0))

            con.Close()
        End Using
    End Function

    Public Function LastDayOfMonth(sourceDate As Date) As Date
        Dim lastDay As New Date(sourceDate.Year, sourceDate.Month, 1)
        Return lastDay.AddMonths(1).AddDays(-1)
    End Function

    Public Function FindControlRecursive(Container As Control, Name As String) As Control
        If Container.ID = Name Then
            Return Container
        End If
        For Each Ctrl As Control In Container.Controls
            Dim Foundctrl As Control = FindControlRecursive(Ctrl, Name)
            If Foundctrl IsNot Nothing Then
                Return Foundctrl
            End If
        Next
        Return Nothing
    End Function

    Public Function ExecuteSqlCommand(commandText As String, sqlParamters As SqlParameter(), sqlConnection As SqlConnection, isResultReturned As Boolean) As DataTable

        If sqlConnection.State = ConnectionState.Closed Then sqlConnection.Open()
        Dim sqlCommand As New SqlCommand(commandText, sqlConnection)

        For Each sqlParameter As SqlParameter In sqlParamters
            Dim unused = sqlCommand.Parameters.Add(sqlParameter)
        Next

        Dim unused1 = sqlCommand.ExecuteNonQuery()

        If isResultReturned Then
            Dim sqlDataAdapter As New SqlDataAdapter(sqlCommand)
            Dim dataTable As New DataTable()
            Dim unused2 = sqlDataAdapter.Fill(dataTable)
            Return dataTable
        Else
            Return Nothing
        End If
        ' If (sqlConnection.State = ConnectionState.Open) Then sqlConnection.Close()
    End Function

    Public Function RecSet(comand As String) As DataSet
        'On Error Resume Next
        'Try
        Dim Ds As DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()

            Ds = New DataSet
            Dim dbadapter As New SqlDataAdapter(comand, con)
            dbadapter.Fill(Ds)
            Return Ds
            Dim unused = Ds.Clone()
            'dbadapter.Dispose()
            con.Close()
        End Using
    End Function

    Public Function CallSP(S As String, Cn As SqlConnection, parm() As SqlParameter) As String
        Dim cmd As New SqlDataAdapter
        Dim rs As New DataSet
        Dim i As Integer
        If Cn.State = ConnectionState.Open Then
            Cn.Close()
        Else

        End If
        Cn.Open()
        cmd.SelectCommand = New SqlCommand(S, Cn) With {
            .CommandType = CommandType.StoredProcedure
        }
        If Not IsNothing(parm) Then
            For i = 0 To parm.Length - 1
                Dim unused1 = cmd.SelectCommand.Parameters.Add(parm(i))
            Next
        End If
        cmd.SelectCommand.CommandText = S
        Dim unused = cmd.Fill(rs)
        Return If(rs.Tables(0).Rows.Item(0).IsNull(0), "", rs.Tables(0).Rows.Item(0).Item(0).ToString)
        Cn.Close()
    End Function

    Public Function GetPolNo(Sys As String, OrderNo As String, EndNo As Integer, loadNo As Integer) As String
        Dim PolId As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select PolNo from PolicyFile where OrderNo='" & OrderNo & "' And SubIns='" & Sys & "' and EndNo=" & EndNo & " and LoadNo=" & loadNo & " ", con)
            Dim unused = dbadapter.Fill(PolId)
            Return PolId.Tables(0).Rows(0)(0)
            con.Close()
        End Using
    End Function

    Public Function GetMainCenter() As String
        Dim br As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("Select BranchNo From BranchInfo where BranchNo=dbo.MainCenter()", con)
            Dim unused = dbadapter.Fill(br)
            Return br.Tables(0).Rows(0)(0)
            con.Close()
        End Using
    End Function

    Public Function GetOfficeManager(Br As String) As Int32
        Dim mngr As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("Select ManagerId From BranchInfo where BranchNo='" & Br & "'", con)
            Dim unused = dbadapter.Fill(mngr)
            Return mngr.Tables(0).Rows(0)(0)
            con.Close()
        End Using
    End Function

    Public Function GetMainBranch(Brn As String) As String
        Return Left(Trim(Brn), 2) & "000"
    End Function

    Public Function GetMainSystem(Sys As String) As String
        Dim mSys As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select MAINSYS from SUBSYSTEMS where SUBSYSNO='{0}' and Branch=dbo.mainCenter()", Sys), con)
            dbadapter.Fill(mSys)
            'GetName = RecSet("select BranchName from BranchInfo where BranchNo='" & SysNo & "'", Conn)
            If mSys.Tables(0).Rows.Count <> 0 Then
                Return Trim(mSys.Tables(0).Rows(0)(0).ToString)
            Else
                Return "4"
            End If
            con.Close()
        End Using
    End Function

    Public Function GetBranchAcc(Br As String) As String
        Dim GetName As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select AccountingCode from BranchInfo where BranchNo='{0}'", Br), con)
            Dim unused = dbadapter.Fill(GetName)
            'GetName = RecSet("select BranchName from BranchInfo where BranchNo='" & SysNo & "'", Conn)
            Return If(GetName.Tables(0).Rows.Count <> 0, Trim(GetName.Tables(0).Rows(0)(0).ToString), "1")
            con.Close()
        End Using
    End Function

    Public Function GetNewBranchAcc() As Short
        Dim GetAcCode As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select isnull(Max(AccountingCode),0) from BranchInfo"), con)
            dbadapter.Fill(GetAcCode)

            Return CInt(GetAcCode.Tables(0).Rows(0)(0)) + 1
            con.Close()
        End Using
    End Function

    Public Function GetAccount(Ptp As Short) As String
        Dim GetAcc As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter As SqlDataAdapter
            Select Case Ptp
                Case 1
                    dbadapter = New SqlDataAdapter("select CashierAccount from BranchInfo where BranchNo=dbo.MainCenter()", con)
                Case 2, 3
                    dbadapter = New SqlDataAdapter("select ChequeAccount from BranchInfo where BranchNo=dbo.MainCenter()", con)
                Case Else
                    Exit Select
            End Select

#Disable Warning BC42104 ' Variable is used before it has been assigned a value
            Dim unused = dbadapter.Fill(GetAcc)
#Enable Warning BC42104 ' Variable is used before it has been assigned a value
            'GetName = RecSet("select BranchName from BranchInfo where BranchNo='" & SysNo & "'", Conn)
            Return Trim(GetAcc.Tables(0).Rows(0)(0).ToString)
            con.Close()
        End Using
    End Function

    Public Function GetEditForm(Sys As String) As String
        Dim editform As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("Select EditForm from SUBSYSTEMS where Subsysno='" & Sys & "' and Branch=dbo.MainCenter()", con)
            Dim unused = dbadapter.Fill(editform)
            Return If(editform.Tables(0).Rows.Count = 0, "", DirectCast(editform.Tables(0).Rows(0)(0), String))
            con.Close()
        End Using
    End Function

    Public Function CallSPDataSet(S As String, Cn As SqlConnection, parm() As SqlParameter) As DataSet
        Dim cmd As New SqlDataAdapter
        Dim rs As New DataSet
        Dim i As Integer

        cmd.SelectCommand = New SqlCommand(S, Cn) With {
            .CommandType = CommandType.StoredProcedure
        }
        If Not IsNothing(parm) Then
            For i = 0 To parm.Length - 1
                Dim unused = cmd.SelectCommand.Parameters.Add(parm(i))
            Next
        End If
        cmd.SelectCommand.CommandText = S
        Dim unused1 = cmd.Fill(rs)
        Return If(rs.Tables(0).Rows.Item(0).IsNull(0), Nothing, rs)
        'Cn.Close()
    End Function

    Public Sub SetPm(Name As String, DbTp As DbType, val As Object, ByRef Pm As Array, Pos As Integer)
        Dim Prm As New SqlParameter() With {.ParameterName = Name, .DbType = DbTp, .Value = val}
        Pm.SetValue(Prm, Pos)
    End Sub

    Public Function GetMainAccLevel(Accnt As String) As Integer
        Dim GetFromTo As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select ACCNTLVL From ACCNTTAB1 Where Accntnum='" & Accnt & "'", con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(GetFromTo)
#Enable Warning IDE0058 ' Expression value is never used
            'GetFromTo = RecSet("select CoverFrom,CoverTo From PolicyFile Where EndNo=0 and LoadNo=0 and PolNo='" & PolNo & "'", Conn)
            Return GetFromTo.Tables(0).Rows(0)(0) + 1
            con.Close()
        End Using
    End Function

    Public Sub SetRepPm(Name As String, DbTp As Boolean, val As Array, ByRef Pm As Array, Pos As Integer)
        If String.IsNullOrEmpty(Name) Then
            Throw New ArgumentException($"'{NameOf(Name)}' cannot be null or empty.", NameOf(Name))
        End If

        If val Is Nothing Then
            Throw New ArgumentNullException(NameOf(val))
        End If

        If Pm Is Nothing Then
            Throw New ArgumentNullException(NameOf(Pm))
        End If

        Dim Prm As New ReportParameter
        Dim i As Integer
        Prm.Name = Name
        Prm.Visible = DbTp

        For i = 0 To val.Length - 1
#Disable Warning IDE0058 ' Expression value is never used
            Prm.Values.Add(val(i))
#Enable Warning IDE0058 ' Expression value is never used
        Next
        Pm.SetValue(Prm, Pos)
    End Sub

    Public Sub ExecSqllog(S As String)
        Try
            Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                If con.State = ConnectionState.Open Then
                    con.Close()
                Else

                End If
                con.Open()

                Using Execute As New SqlDataSource() With {
                    .ConnectionString = con.ConnectionString,
                    .InsertCommandType = SqlDataSourceCommandType.Text,
                    .InsertCommand = S}
#Disable Warning IDE0058 ' Expression value is never used
                    Execute.Insert()

#Enable Warning IDE0058 ' Expression value is never used
                End Using
                con.Close()
            End Using
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Public Sub ExecSql(S As String)
        ExecSqllog("Insert Into LogData ([UserName] ,[Operation]) Values ('" & HttpContext.Current.Session("User") & "','" & S.Replace("'", "") & "')")
        Try
            Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                If con.State = ConnectionState.Open Then
                    con.Close()
                Else

                End If
                con.Open()

                Using Execute As New SqlDataSource() With {
                    .ConnectionString = con.ConnectionString,
                    .InsertCommandType = SqlDataSourceCommandType.Text,
                    .InsertCommand = S}
#Disable Warning IDE0058 ' Expression value is never used
                    Execute.Insert()

#Enable Warning IDE0058 ' Expression value is never used
                End Using
                con.Close()
            End Using
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    'Public Async Function ExecConn(S As String, SpcConnas As SqlConnection) As Task
    '    ' Log operation (synchronous as original)
    '    ExecSqllog("Insert Into LogData ([UserName],[Operation]) Values ('" & HttpContext.Current.Session("User") & "','" & S.Replace("'", "") & "')")

    '    Dim sqls = S.Split("; ")

    '    ' Ensure connection is closed before async open
    '    If SpcConnas.State = ConnectionState.Open Then
    '        SpcConnas.Close()
    '    End If

    '    Try
    '        Await SpcConnas.OpenAsync() ' Open asynchronously

    '        If sqls.Length < 2 Then
    '            ' Single command
    '            Using cmd As New SqlCommand(S, SpcConnas)
    '                Await cmd.ExecuteNonQueryAsync() ' Async execution
    '            End Using
    '        Else
    '            ' Batch commands: Execute sequentially
    '            For Each SS As String In sqls
    '                If String.IsNullOrWhiteSpace(SS) Then Continue For

    '                Try
    '                    Using cmd As New SqlCommand(SS, SpcConnas)
    '                        Await cmd.ExecuteNonQueryAsync() ' Async per command
    '                    End Using
    '                Catch ex As Exception
    '                    ' Exit immediately on first error (original behavior)
    '                    Exit For
    '                End Try
    '            Next
    '        End If
    '    Finally
    '        ' Ensure connection is always closed
    '        If SpcConnas.State = ConnectionState.Open Then
    '            SpcConnas.Close()
    '        End If
    '    End Try
    'End Function
    Public Sub ExecConn(S As String, SpcConnas As SqlConnection, Optional parameters As SqlParameter() = Nothing)
        Dim transaction As SqlTransaction = Nothing

        Try
            ' Log the operation
            LogOperation(S)

            ' Open connection
            If SpcConnas.State = ConnectionState.Closed Then
                SpcConnas.Open()
            End If

            ' Begin transaction
            transaction = SpcConnas.BeginTransaction(IsolationLevel.ReadCommitted)

            ' Check if it's a single parameterized statement
            If parameters IsNot Nothing AndAlso parameters.Length > 0 Then
                ' Execute single parameterized statement
                Using cmd As New SqlCommand(S, SpcConnas, transaction)
                    cmd.Parameters.AddRange(parameters)
                    cmd.ExecuteNonQuery()
                End Using
            Else
                ' Execute batch of statements
                Dim sqls = S.Split(New Char() {";"c}, StringSplitOptions.RemoveEmptyEntries)

                For Each sql As String In sqls
                    Dim trimmedSql = sql.Trim()
                    If Not String.IsNullOrEmpty(trimmedSql) Then
                        Using cmd As New SqlCommand(trimmedSql, SpcConnas, transaction)
                            cmd.ExecuteNonQuery()
                        End Using
                    End If
                Next
            End If

            ' Commit transaction
            transaction.Commit()
        Catch ex As Exception
            ' Rollback on error
            If transaction IsNot Nothing Then
                Try
                    transaction.Rollback()
                Catch rollbackEx As Exception
                    ' Log rollback error
                    ' Debug.WriteLine("Rollback failed: " & rollbackEx.Message)
                End Try
            End If

            ' Re-throw for caller to handle
            Throw
        Finally
            ' Close connection
            If SpcConnas.State = ConnectionState.Open Then
                SpcConnas.Close()
            End If
        End Try
    End Sub

    Private Sub LogOperation(sql As String)
        Try
            ' Use separate connection for logging to avoid transaction conflicts
            Using logConn As New SqlConnection(ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString)
                Using cmd As New SqlCommand(
                "INSERT INTO LogData ([UserName], [Operation], [IPAddress]) " &
                "VALUES (@UserName, @Operation, @IPAddress)", logConn)

                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 100).Value = If(HttpContext.Current.Session("User"), "Unknown")
                    cmd.Parameters.Add("@Operation", SqlDbType.NVarChar, 4000).Value = sql.Replace("'", "")
                    cmd.Parameters.Add("@IPAddress", SqlDbType.VarChar, 50).Value = HttpContext.Current.Request.UserHostAddress

                    If logConn.State = ConnectionState.Closed Then
                        logConn.Open()
                    End If
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch logEx As Exception
            ' Don't fail main operation if logging fails
            'System.Diagnostics.Debug.WriteLine("Logging failed: " & logEx.Message)
        End Try
    End Sub

    'Public Sub ExecConn(S As String, SpcConnas As SqlConnection)
    '    Dim transaction As SqlTransaction = Nothing
    '    Dim success As Boolean

    '    Try
    '        ' Log the operation (be careful not to log sensitive data)
    '        Dim logSql = "Insert Into LogData ([UserName], [Operation]) Values (@UserName, @Operation)"

    '        ' Execute log with parameters to avoid SQL injection in log too
    '        Using cmdLog As New SqlCommand(logSql, SpcConnas)
    '            cmdLog.Parameters.Add("@UserName", SqlDbType.VarChar, 100).Value = If(HttpContext.Current.Session("User"), String.Empty)
    '            cmdLog.Parameters.Add("@Operation", SqlDbType.NVarChar, 4000).Value = S.Replace("'", "''")

    '            If SpcConnas.State = ConnectionState.Closed Then
    '                SpcConnas.Open()
    '            End If
    '            cmdLog.ExecuteNonQuery()
    '        End Using

    '        ' Split SQL statements
    '        Dim sqls = S.Split(New Char() {";"c}, StringSplitOptions.RemoveEmptyEntries)

    '        If sqls.Length = 0 Then
    '            Return
    '        End If

    '        ' Begin transaction
    '        transaction = SpcConnas.BeginTransaction("BatchOperation")

    '        For Each SS As String In sqls
    '            Dim trimmedSql = SS.Trim()
    '            If Not String.IsNullOrEmpty(trimmedSql) Then
    '                Using cmd As New SqlCommand(trimmedSql, SpcConnas, transaction)
    '                    ' Execute command within transaction
    '                    cmd.ExecuteNonQuery()
    '                End Using
    '            End If
    '        Next

    '        ' Commit transaction if all succeeded
    '        transaction.Commit()
    '        success = True

    '    Catch ex As SqlException
    '        ' Rollback transaction on SQL error
    '        If transaction IsNot Nothing Then
    '            Try
    '                transaction.Rollback()
    '            Catch rollbackEx As Exception
    '                ' Log rollback error but don't mask original exception
    '                ' You could add additional logging here
    '            End Try
    '        End If

    '        ' Re-throw the exception to be handled by caller
    '        Throw New Exception("SQL execution failed: " & ex.Message, ex)

    '    Catch ex As Exception
    '        ' Rollback transaction on any other error
    '        If transaction IsNot Nothing Then
    '            Try
    '                transaction.Rollback()
    '            Catch rollbackEx As Exception
    '                ' Log rollback error
    '            End Try
    '        End If

    '        Throw New Exception("Execution failed: " & ex.Message, ex)

    '    Finally
    '        ' Clean up connection
    '        If SpcConnas.State = ConnectionState.Open Then
    '            SpcConnas.Close()
    '        End If
    '    End Try
    'End Sub
    '    Public Sub ExecConn(S As String, SpcConnas As SqlConnection)
    '        ExecSqllog("Insert Into LogData ([UserName] ,[Operation]) Values ('" & HttpContext.Current.Session("User") & "','" & S.Replace("'", "") & "')")
    '        Dim sqls = S.Split("; ")
    '        If SpcConnas.State = ConnectionState.Open Then
    '            SpcConnas.Close()
    '        Else

    '        End If
    '        SpcConnas.Open()
    '        If sqls.Length < 2 Then
    '            Using Execute As New SqlDataSource() With {.ConnectionString = SpcConnas.ConnectionString,
    '                                                        .InsertCommandType = SqlDataSourceCommandType.Text,
    '                                                        .InsertCommand = S}
    '#Disable Warning IDE0058 ' Expression value is never used
    '                Execute.Insert()
    '#Enable Warning IDE0058 ' Expression value is never used
    '            End Using
    '            'On Error exit
    '            SpcConnas.Close()
    '        Else
    '            For Each SS As String In sqls
    '                Try
    '                    If SS <> "" Then
    '                        Using Execute As New SqlDataSource() With {.ConnectionString = SpcConnas.ConnectionString,
    '                                                                    .InsertCommandType = SqlDataSourceCommandType.Text,
    '                                                                    .InsertCommand = SS}
    '#Disable Warning IDE0058 ' Expression value is never used
    '                            Execute.Insert()
    '#Enable Warning IDE0058 ' Expression value is never used
    '                        End Using
    '                    Else

    '                    End If

    '                    'On Error exit
    '                    SpcConnas.Close()
    '                Catch ex As Exception
    '                    SpcConnas.Close()
    '                    Exit Sub
    '                End Try
    '            Next
    '        End If
    '    End Sub
    'Public Sub ExecConn(S As String, SpcConnas As SqlConnection)
    '    Dim transaction As SqlTransaction = Nothing
    '    Dim success As Boolean

    '    Try
    '        ' Log the operation (be careful not to log sensitive data)
    '        Dim logSql = "Insert Into LogData ([UserName], [Operation]) Values (@UserName, @Operation)"

    '        ' Execute log with parameters to avoid SQL injection in log too
    '        Using cmdLog As New SqlCommand(logSql, SpcConnas)
    '            cmdLog.Parameters.Add("@UserName", SqlDbType.VarChar, 100).Value = If(HttpContext.Current.Session("User"), String.Empty)
    '            cmdLog.Parameters.Add("@Operation", SqlDbType.NVarChar, 4000).Value = S.Replace("'", "''")

    '            If SpcConnas.State = ConnectionState.Closed Then
    '                SpcConnas.Open()
    '            End If
    '            cmdLog.ExecuteNonQuery()
    '        End Using

    '        ' Split SQL statements
    '        Dim sqls = S.Split(New Char() {";"c}, StringSplitOptions.RemoveEmptyEntries)

    '        If sqls.Length = 0 Then
    '            Return
    '        End If

    '        ' Begin transaction
    '        transaction = SpcConnas.BeginTransaction("BatchOperation")

    '        For Each SS As String In sqls
    '            Dim trimmedSql = SS.Trim()
    '            If Not String.IsNullOrEmpty(trimmedSql) Then
    '                Using cmd As New SqlCommand(trimmedSql, SpcConnas, transaction)
    '                    ' Execute command within transaction
    '                    cmd.ExecuteNonQuery()
    '                End Using
    '            End If
    '        Next

    '        ' Commit transaction if all succeeded
    '        transaction.Commit()
    '        success = True

    '    Catch ex As SqlException
    '        ' Rollback transaction on SQL error
    '        If transaction IsNot Nothing Then
    '            Try
    '                transaction.Rollback()
    '            Catch rollbackEx As Exception
    '                ' Log rollback error but don't mask original exception
    '                ' You could add additional logging here
    '            End Try
    '        End If

    '        ' Re-throw the exception to be handled by caller
    '        Throw New Exception("SQL execution failed: " & ex.Message, ex)

    '    Catch ex As Exception
    '        ' Rollback transaction on any other error
    '        If transaction IsNot Nothing Then
    '            Try
    '                transaction.Rollback()
    '            Catch rollbackEx As Exception
    '                ' Log rollback error
    '            End Try
    '        End If

    '        Throw New Exception("Execution failed: " & ex.Message, ex)

    '    Finally
    '        ' Clean up connection
    '        If SpcConnas.State = ConnectionState.Open Then
    '            SpcConnas.Close()
    '        End If
    '    End Try
    'End Sub
    '    Public Sub ExecConn(S As String, SpcConnas As SqlConnection)
    '        ExecSqllog("Insert Into LogData ([UserName] ,[Operation]) Values ('" & HttpContext.Current.Session("User") & "','" & S.Replace("'", "") & "')")
    '        Dim sqls = S.Split("; ")
    '        If SpcConnas.State = ConnectionState.Open Then
    '            SpcConnas.Close()
    '        Else

    '        End If
    '        SpcConnas.Open()
    '        If sqls.Length < 2 Then
    '            Using Execute As New SqlDataSource() With {.ConnectionString = SpcConnas.ConnectionString,
    '                                                        .InsertCommandType = SqlDataSourceCommandType.Text,
    '                                                        .InsertCommand = S}
    '#Disable Warning IDE0058 ' Expression value is never used
    '                Execute.Insert()
    '#Enable Warning IDE0058 ' Expression value is never used
    '            End Using
    '            'On Error exit
    '            SpcConnas.Close()
    '        Else
    '            For Each SS As String In sqls
    '                Try
    '                    If SS <> "" Then
    '                        Using Execute As New SqlDataSource() With {.ConnectionString = SpcConnas.ConnectionString,
    '                                                                    .InsertCommandType = SqlDataSourceCommandType.Text,
    '                                                                    .InsertCommand = SS}
    '#Disable Warning IDE0058 ' Expression value is never used
    '                            Execute.Insert()
    '#Enable Warning IDE0058 ' Expression value is never used
    '                        End Using
    '                    Else

    '                    End If

    '                    'On Error exit
    '                    SpcConnas.Close()
    '                Catch ex As Exception
    '                    SpcConnas.Close()
    '                    Exit Sub
    '                End Try
    '            Next
    '        End If
    '    End Sub

    Public Function IsFind(Name As String, Optional NN As Byte = 1) As Boolean
        Dim Check As New DataSet
        Dim NameOrNo As String
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            If NN = 0 Then NameOrNo = "CustNo" Else NameOrNo = "CustName"
            Dim dbadapter = New SqlDataAdapter(String.Format("Select CustName, Specialcase From CustomerFile where {0}='{1}'", NameOrNo, Trim(Name)), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(Check)
#Enable Warning IDE0058 ' Expression value is never used
            'Check = RecSet("select CustName,specialcase From CustomerFile where " & NameOrNo & "='" & Trim(Name) & "'", Conn)
            Return Check.Tables(0).Rows.Count <> 0
            con.Close()
        End Using
    End Function

    Public Function GetSpCase(NN As Integer) As Integer
        Dim Check As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select CustName,specialcase From CustomerFile where CustNo={0}", NN), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(Check)
#Enable Warning IDE0058 ' Expression value is never used
            Return If(Check.Tables(0).Rows.Count = 0, 1, DirectCast(Check.Tables(0).Rows.Item(0).Item(1), Integer))
            con.Close()
        End Using
    End Function

    Public Function GetPhoneNo(NN As Integer) As String
        Dim ch2 As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select TelNo From CustomerFile where CustNo=" & NN & " And right(rtrim(TelNo),12) Not in (Select right(rtrim(TelNo),12) From DailySMS where SendDate='" & Format(Today.Date(), "yyyy-MM-dd") & "')", con)
            dbadapter.Fill(ch2)
            If ch2.Tables(0).Rows.Count = 0 Then Return 0

            If Len(ch2.Tables(0).Rows(0)(0).ToString.TrimEnd) = 1 Or ch2.Tables(0).Rows(0)(0).ToString.TrimEnd.Contains("0000000") Then
                Return 0
            Else
                Select Case Len(ch2.Tables(0).Rows(0)(0).ToString.TrimEnd)
                    Case 13, 14
                        Return Right(ch2.Tables(0).Rows(0)(0).ToString.TrimEnd, 12)
                    Case Else
                        Return 0
                End Select
            End If
            con.Close()
            If Len(ch2.Tables(0).Rows(0)(0).ToString.TrimEnd) = 1 Or ch2.Tables(0).Rows(0)(0).ToString.TrimEnd.Contains("00000000") Then
                Return 0
            Else
                Select Case Len(ch2.Tables(0).Rows(0)(0).ToString.TrimEnd)
                    Case 13, 14
                        Return Right(ch2.Tables(0).Rows(0)(0).ToString.TrimEnd, 12)
                    Case Else
                        Return 0
                End Select
            End If
            con.Close()
        End Using
    End Function

    Public Function GetCustByOrderNo(NN As String, endn As Integer, loadn As Integer) As Integer
        Dim ch1 As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select CustNo From PolicyFile where OrderNo='{0}' and EndNo={1} and LoadNo={2}", NN, endn, loadn), con)
            Dim unused = dbadapter.Fill(ch1)
            Return If(ch1.Tables(0).Rows.Count = 0, 0, DirectCast(ch1.Tables(0).Rows(0)(0), Integer))
            con.Close()
        End Using
    End Function

    Public Function GetUserNoByLogin(NN As String) As Integer
        Dim log As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select AccountNo From AccountFile where AccountLogIn='{0}'", NN), con)
            Dim unused = dbadapter.Fill(log)
            Return If(log.Tables(0).Rows.Count = 0, 0, DirectCast(log.Tables(0).Rows(0)(0), Integer))
            con.Close()
        End Using
    End Function

    Public Function FindCustomer(No As String) As String
        Dim Check As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select CustName,SpecialCase From CustomerFile where CustNo=" & Val(No), con)
            Dim unused = dbadapter.Fill(Check)
            'Check = RecSet("select CustName,SpecialCase From CustomerFile where CustNo=" & Val(No), Conn)
            Return If(Check.Tables(0).Rows.Count = 0, "", DirectCast(Check.Tables(0).Rows.Item(0).Item(0), String))
            con.Close()
        End Using
    End Function

    Public Function FindExtra(No As String, type As String) As String
        Dim Check As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            If IsNumeric(No) Then
                Dim dbadapter = New SqlDataAdapter(String.Format("select TPName From ExtraInfo where TPNO={0} and TP='{1}'", Val(No), type), con)
#Disable Warning IDE0058 ' Expression value is never used
                dbadapter.Fill(Check)
#Enable Warning IDE0058 ' Expression value is never used
                'Check = RecSet("select TPName From ExtraInfo where TPNO=" & Val(No) & " and TP='" & type & "'", Conn)
            Else
                Dim dbadapter = New SqlDataAdapter(String.Format("select TPName From ExtraInfo where TPName='{0}' and TP='{1}'", No, type), con)
#Disable Warning IDE0058 ' Expression value is never used
                dbadapter.Fill(Check)
#Enable Warning IDE0058 ' Expression value is never used
                'Check = RecSet("select TPName From ExtraInfo where TPName='" & No & "' and TP='" & type & "'", Conn)
            End If
            Return If(Check.Tables(0).Rows.Count = 0, "", Trim(Check.Tables(0).Rows.Item(0).Item(0)))
            con.Close()
        End Using

    End Function

    Public Function FindCustomerName(No As Integer) As String
        Dim Check As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select CustName,SpecialCase From CustomerFile where CustNo=" & No & "", con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(Check)
#Enable Warning IDE0058 ' Expression value is never used
            'Check = RecSet("Select CustName, SpecialCase From CustomerFile where CustNo=" & Val(No), Conn)
            Return If(Check.Tables(0).Rows.Count = 0, "", Trim(Check.Tables(0).Rows.Item(0).Item(0)))
            con.Close()
        End Using
    End Function

    Public Function FindCoverName(No As String, SubSystem As String) As String
        Dim Check As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select CoverName From Covers where CoverNo={0} and SubSystem='{1}'", Val(No), SubSystem), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(Check)
#Enable Warning IDE0058 ' Expression value is never used
            'Check = RecSet("select CoverName From Covers where CoverNo=" & Val(No) & " and SubSystem='" & SubSystem & "'", Conn)
            Return If(Check.Tables(0).Rows.Count = 0, "", DirectCast(Check.Tables(0).Rows.Item(0).Item(0), String))
            con.Close()
        End Using
    End Function

    Public Function GetCustomerName(CustNo As Integer) As String
        Dim CustomerName As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select CustName from CustomerFile where CustNo='{0}'", CustNo), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(CustomerName)
#Enable Warning IDE0058 ' Expression value is never used
            'LastEnd = RecSet("select CustNo from PolicyFile where PolNo='" & OrderNo & "' and endNo=" & Val(EndNo) - 1 & " and loadNo=" & LoadNo, Conn)
            Return If(CustomerName.Tables(0).Rows.Count <> 0, CustomerName.Tables(0).Rows(0)("CustName").ToString.TrimEnd, "")
            con.Close()
        End Using
    End Function

    Public Function GetCustomerDetails(CustNo As Integer) As List(Of Object)
        Dim customerDetails As New List(Of Object)()

        Using connection As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            Dim sql As String = "SELECT CustNo, CustName, CustNameE, TelNo, Address, Email, SpecialCase, AccNo FROM CustomerFile WHERE CustNo = @CustNo"

            Using command As New SqlCommand(sql, connection)
                command.Parameters.AddWithValue("@CustNo", CustNo)
                connection.Open()

                Using reader As SqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        customerDetails.Add(New With {
                        .CustNo = SafeGetInteger(reader, "CustNo"),
                        .CustName = SafeGetString(reader, "CustName"),
                        .CustNameE = SafeGetString(reader, "CustNameE"),
                        .TelNo = SafeGetString(reader, "TelNo"),
                        .Address = SafeGetString(reader, "Address"),
                        .Email = SafeGetString(reader, "Email"),
                        .SpecialCase = SafeGetString(reader, "SpecialCase"),
                        .AccNo = SafeGetString(reader, "AccNo")
                    })
                    End While
                End Using
            End Using
        End Using

        Return customerDetails
    End Function

    Private Function SafeGetInteger(reader As SqlDataReader, columnName As String) As Integer
        Try
            Dim ordinal As Integer = reader.GetOrdinal(columnName)
            Return If(reader.IsDBNull(ordinal), 0, reader.GetInt32(ordinal))
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Function SafeGetString(reader As SqlDataReader, columnName As String) As String
        Try
            Select Case columnName
                Case "TelNo"
                    If Left(reader(columnName), 5) = "00218" Or Left(reader(columnName), 4) = "+218" Then
                        Select Case Len(reader(columnName).ToString.TrimEnd)
                            Case 14, 13
                                Return reader(columnName).ToString().TrimEnd.Replace("00218", "+218")
                            Case 9
                                Return "+218" + reader(columnName).ToString().TrimEnd
                            Case 10
                                Return "+218" + Right(reader(columnName).ToString().TrimEnd, 9)
                            Case Else
                                Return reader(columnName).ToString().TrimEnd.Replace("00218", "+218").Replace("+218", "+218")
                        End Select
                    Else
                        Select Case Len(reader(columnName).ToString.TrimEnd)
                            Case 14, 13
                                Return reader(columnName).ToString().TrimEnd.Replace("00218", "+218")
                            Case 9
                                Return "+218" + reader(columnName).ToString().TrimEnd
                            Case 10
                                Return "+218" + Right(reader(columnName).ToString().TrimEnd, 9)
                            Case Else
                                Return "+218000000000"
                        End Select
                    End If
                Case Else
                    If reader(columnName) Is DBNull.Value Then
                        Return ""
                    Else
                        Return reader(columnName).ToString().TrimEnd
                    End If
            End Select
        Catch ex As Exception
            Return ""
        End Try

    End Function

    Public Sub SetDateValue(ByRef TextConrol As TextBox, Value As Date)
        TextConrol.Text = Format(Value, "yyyy/MM/dd")
    End Sub

    Public Sub SetDateEdit(ByRef dedit As ASPxDateEdit, Value As Date)
        dedit.Value = Value
    End Sub

    Public Sub SetComboIndex(ByRef Combo As ASPxComboBox, Value As Integer)
        Combo.SelectedIndex = Value
    End Sub

    Public Sub Setdxgridlook(ByRef dxlookup As ASPxGridLookup, vlu As Integer)
        'dxlookup.GridView.FocusedRowIndex = Value
        'SelectRowByKey(Value)
        dxlookup.GridView.FocusedRowIndex = vlu - 1
        'dxlookup.GridView.Selection.SelectRow(vlu)
        'dxlookup.GridView.Selection.SetSelection(Value, True)
    End Sub

    Public Function GetDropListValue(ByRef DropList As DropDownList) As Integer
        Try
            Return Trim(DropList.SelectedItem.Value)
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Function GetTextValue(ByRef TextConrol As TextBox) As String
        Try
            Return Trim(TextConrol.Text)
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function GetDxTextValue(ByRef TextConrol As ASPxTextBox) As String
        Try
            Return Trim(TextConrol.Text)
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function GetDateedit(ByRef dedit As ASPxDateEdit) As Date
        Try
            Return dedit.Value
        Catch ex As Exception
            Return ""
        End Try
    End Function

    'Public Sub SetTextValue(ByRef TextConrol As TextBox, ByVal Value As String)
    '    TextConrol.Text = Value
    'End Sub
    Public Sub SetDxtxtValue(ByRef TextConrol As ASPxTextBox, Value As String)

        TextConrol.Value = Value
    End Sub

    Public Sub Setspinedit(ByRef spinedit As ASPxSpinEdit, Value As Integer)
        spinedit.Value = Value
    End Sub

    Public Function Getspinedit(spinedit As ASPxSpinEdit) As Short
        Try
            Return spinedit.Value
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Function GetlookuptValue(ByRef lookupctrl As ASPxGridLookup) As Integer
        Try
            Return lookupctrl.Value
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Function GetComboValue(Combobox As ASPxComboBox) As Integer
        Try
            Return Combobox.Value
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Function GetComboText(ByRef Combobox As ASPxComboBox) As String
        Try
            Return Combobox.Text
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function GetIssDate(ByRef OrderN As String) As String
        Dim IssDate As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else
            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select IssuTime,SubIns,Branch From policyfile where OrderNo='{0}' ", OrderN), con)
            dbadapter.Fill(IssDate)
            Return Format(IssDate.Tables(0).Rows.Item(0).Item("IssuTime"), "HH:mm:ss yyyy/MM/dd dddd") + " / " + GetSysName(IssDate.Tables(0).Rows.Item(0).Item("SubIns")) + " / " + GetBranchName(IssDate.Tables(0).Rows.Item(0).Item("Branch"))
            con.Close()
        End Using
    End Function

    Public Function GetCoverDays(ByRef PolNo As String, ByRef EndNo As Integer, ByRef Sys As String) As Double
        Dim Days As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select coverfrom,coverto From policyfile where PolNo='{0}' and EndNo={1} and subins='{2}'", PolNo, EndNo, Sys), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(Days)
#Enable Warning IDE0058 ' Expression value is never used
            Return DateDiff(DateInterval.Day, Days.Tables(0).Rows.Item(0).Item(0), Days.Tables(0).Rows.Item(0).Item(1)) / 365 * 100
            con.Close()
        End Using
    End Function

    Public Function GetExtraFile(Sys As String) As String
        Dim IssueVal As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select ExtraInfo from SubSystems where SubSysNo='" & Sys & "' and branch=dbo.[MainCenter]()", con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(IssueVal)
#Enable Warning IDE0058 ' Expression value is never used
            Return IssueVal.Tables(0).Rows(0)(0)
            con.Close()
        End Using
    End Function

    Public Function GetRet(Sys As String) As String
        Dim IssueVal As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select Ret from SubSystems where SubSysNo='" & Sys & "' and branch=dbo.[MainCenter]()", con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(IssueVal)
#Enable Warning IDE0058 ' Expression value is never used
            Return IssueVal.Tables(0).Rows(0)(0)
            con.Close()
        End Using
    End Function

    'Public Function GetEbaValue(ByRef EbaConrol As eba.Web.Combo, Optional ByVal TP As Integer = 0) As String
    '    Try
    '        Dim Find As New DataSet
    '        Dim foundRows() As DataRow
    '        Find = EbaConrol.DataSource

    '        If IsNumeric(Trim(EbaConrol.TextBox.Value)) Then
    '            foundRows = Find.Tables(0).Select(String.Format("{0} ={1}", EbaConrol.DataValueField, Trim(EbaConrol.TextBox.Value)))
    '        Else
    '            If EbaConrol.CSSClassName <> "" And EbaConrol.DataTextField = "TPNAME" Then
    '                Dim FindLocal As New DataSet
    '                'Dim ConnLocal As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.AppSettings("Conn"))
    '                Dim dbadapter = New SqlDataAdapter(String.Format("select tpName,TpNo from Extrainfo where Tp='{0}' and TpName='{1}'", EbaConrol.CSSClassName, EbaConrol.TextBox.Value), Conn)
    '                dbadapter.Fill(FindLocal)
    '                'Find = RecSet("select tpName,TpNo from Extrainfo where Tp='" & EbaConrol.CSSClassName & "' and TpName='" & Trim(EbaConrol.TextBox.Value) & "'", Conn)
    '                foundRows = FindLocal.Tables(0).Select()
    '            Else
    '                'If EbaConrol.DataTextField = "ShipName" Then Find = RecSet("select * from ShipFile", Conn)
    '                foundRows = Find.Tables(0).Select(String.Format("{0}='{1}'", EbaConrol.DataTextField, Trim(EbaConrol.TextBox.Value)))
    '            End If
    '        End If

    '        If foundRows.Length <> 0 Then
    '            GetEbaValue = foundRows(0)(TP)
    '        Else
    '            GetEbaValue = 0
    '        End If
    '        'GetEbaValue = EbaConrol.SelectedRowValues(1)
    '    Catch ex As Exception
    '        GetEbaValue = ""
    '    End Try
    'End Function
    'Public Sub SetEbaValue(ByRef EbaConrol As eba.Web.Combo, ByVal value As String)
    '        Try
    '            EbaConrol.TextBox.Value = value
    '            If Not IsNumeric(value) Then EbaConrol.TextValue = Trim(value) Else EbaConrol.InitialSearch = ""
    '        Catch ex As Exception
    '            EbaConrol.TextBox.Value = ""
    '        End Try
    '    End Sub
    Public Sub SetComboValue(ByRef Combo As ASPxComboBox, Value As Integer)
        Combo.SelectedIndex = Value
    End Sub

    'Public Sub SetRadValue(ByRef RadConrol As RadDatePicker, value As DateTime)
    '    Try
    '        RadConrol.SelectedDate = value
    '        'If Not IsNumeric(value) Then RadConrol.InitialSearch = Trim(value) Else RadConrol.InitialSearch = ""
    '    Catch ex As Exception
    '        RadConrol.SelectedDate = ""
    '    End Try
    'End Sub

    Public Function CFormat(D As Date) As String
        Dim M, Y As String
        Dim X As String = Day(D) : If Len(X) = 1 Then X = "0" & X
        M = Month(D) : If Len(M) = 1 Then M = "0" & M
        Y = Year(D)
        Return String.Format("{0}-{1}-{2}", Y, M, X)
    End Function

    Public Function GetExrate(Currno As Integer, CurrName As String) As Double
        Dim lastrate As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select top(1) CURNCOST from CURRNECY where CURNCYNO=" & Currno & " and CURNCYNM='" & CurrName & "' order by RateDate desc", con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(lastrate)
#Enable Warning IDE0058 ' Expression value is never used

            Return If(lastrate.Tables(0).Rows.Count <> 0, CDbl(lastrate.Tables(0).Rows(0).Item("CURNCOST")), 1)
            con.Close()
        End Using
    End Function

    Public Sub IssueInWardPolicy(OrderNo As String, EndNo As Integer, LoadNo As Integer)
        ExecConn("Update PolicyFile set PolNo='" & OrderNo & "', IssuDate=" & "CONVERT(DATETIME,'" & Format(Date.Today.Date, "yyyy-MM-dd") & " 00:00:00',102)," _
            & " IssuTime=" & "CONVERT(DATETIME,'" & Format(Today.Date, "yyyy-MM-dd") & " 00:00:00',102)" _
            & " where OrderNo='" & OrderNo & "' And EndNo=" & EndNo & " And LoadNo=" & LoadNo & "", Conn)
    End Sub

    Public Sub IssuePolicy(OrderNo As String, PolNo As String, EndNo As Integer, LoadNo As Integer, Sys As String, Br As String, IssueUser As String, Optional SerialNo As Integer = 0)
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Parm = Array.CreateInstance(GetType(SqlParameter), 9)

            SetPm("@TP", DbType.String, Sys, Parm, 0)
            SetPm("@OrderNo", DbType.String, OrderNo, Parm, 1)
            SetPm("@PolNo", DbType.String, PolNo, Parm, 2)
            SetPm("@EndNo", DbType.Int16, EndNo, Parm, 3)
            SetPm("@LoadNo", DbType.Int16, LoadNo, Parm, 4)
            SetPm("@IssuDate", DbType.Date, Today.Date, Parm, 5)
            SetPm("@BranchNo", DbType.String, Br, Parm, 6)
            SetPm("@IssuUser", DbType.String, IssueUser, Parm, 7)
            SetPm("@Serial", DbType.Int32, SerialNo, Parm, 8)

            Dim Pol As String = CallSP("IssuPolicy", con, Parm)
            If EndNo <> 0 Then
                ExecConn("Update PolicyFile Set AccountNo=0,Paytype=1,Financed=0 where OrderNo='" & OrderNo & "'", con)
            End If
            'Conn.Close()
            con.Close()
        End Using
    End Sub

    Public Function RndTax(a As Double) As Double
        If Fix(a) = a Then RndTax = a : Exit Function
        Return If(Math.Abs((a - Fix(a)) * 1000) > 500, Fix(a) + (1 * a / Math.Abs(a)), Fix(a) + (0.5 * a / Math.Abs(a)))
    End Function

    Public Function RndTax25(a As Double) As Double
        If Fix(a) = a Then Return a : Exit Function
        Select Case Format(Math.Abs((a - Fix(a)) * 1000), "000")
            Case 0 To 250
                RndTax25 = Fix(a) + (0.25 * a / Math.Abs(a))
            Case 251 To 500
                RndTax25 = Fix(a) + (0.5 * a / Math.Abs(a))
            Case 501 To 750
                RndTax25 = Fix(a) + (0.75 * a / Math.Abs(a))
            Case >= 751
                RndTax25 = Fix(a) + (1 * a / Math.Abs(a))
            Case Else
                Exit Select
        End Select
        Return RndTax25
        'If Math.Abs((a - Fix(a)) * 1000) > 250 Then
        '    RndTax25 = CDbl(Fix(a) + 1 * a / Math.Abs(a))
        'Else
        '    RndTax25 = CDbl(Fix(a) + 0.25 * a / Math.Abs(a))
        'End If
    End Function

    Public Function RndOne(a As Double) As Double
        If Fix(a) = a Then Return a : Exit Function
        Select Case Format(Math.Abs((a - Fix(a)) * 1000), "000")
            Case 0 To 999
                Return Fix(a) + (1 * a / Math.Abs(a))
            Case Else
                Return 0
        End Select
        'Return RndOne()
        'If Math.Abs((a - Fix(a)) * 1000) > 250 Then
        '    RndTax25 = CDbl(Fix(a) + 1 * a / Math.Abs(a))
        'Else
        '    RndTax25 = CDbl(Fix(a) + 0.25 * a / Math.Abs(a))
        'End If
    End Function

    Public Function Rndqtr(a As Double) As Double
        If Fix(a) = a Then Return a : Exit Function
        'Select Case Math.Abs((a - Fix(a)) * 1000)
        '    Case 0 To 250
        '        rndqtr = CDbl(Fix(a) + 0.25 * a / Math.Abs(a))
        '    Case 250 To 500
        '        rndqtr = CDbl(Fix(a) + 0.5 * a / Math.Abs(a))
        '    Case 500 To 750
        '        rndqtr = CDbl(Fix(a) + 0.75 * a / Math.Abs(a))
        '    Case >= 750
        '        rndqtr = CDbl(Fix(a) + 1 * a / Math.Abs(a))
        'End Select
        'Return rndqtr()
        Rndqtr = If(Math.Abs((a - Fix(a)) * 1000) > 250, Fix(a) + (1 * a / Math.Abs(a)), Fix(a) + (0.25 * a / Math.Abs(a)))
        Return Rndqtr
    End Function

    Public Function Rndup(a As Double) As Double
        If Fix(a) = a Then Return a : Exit Function
        Return If(Math.Abs((a - Fix(a)) * 1000) > 1, Fix(a) + (1 * a / Math.Abs(a)), Fix(a) + (1 * a / Math.Abs(a)))
    End Function

    Public Function UpdateData(Frm As Object, sys As String, Ord As String, EndNo As Integer, LoadNo As Integer) As String
        Dim FieldSql As String = ""
        If Frm.HasControls() Then

            Dim cont As ControlCollection = Frm.Controls

            For Each mycontrol As Control In cont
                If TypeOf mycontrol Is ASPxPanel And mycontrol.Visible Then
                    Dim mytb As ASPxPanel = TryCast(mycontrol, ASPxPanel)
                    For i As Integer = 0 To mytb.Controls.Count - 1
                        If TypeOf mytb.Controls(i) Is ASPxTextBox Then
                            Select Case TryCast(mytb.Controls(i), ASPxTextBox).CssClass
                                Case 1
                                    FieldSql += mytb.Controls(i).ID & "='" & Trim(TryCast(mytb.Controls(i), ASPxTextBox).Value) & "', "
                                Case 2
                                    FieldSql += mytb.Controls(i).ID & "= PARSE('" & IIf(String.IsNullOrEmpty(TryCast(mytb.Controls(i), ASPxTextBox).Value), 0, TryCast(mytb.Controls(i), ASPxTextBox).Value) & "' AS decimal(22,3) USING 'en-us'), "
                                Case 3
                                    FieldSql += mytb.Controls(i).ID & "=" & Trim(TryCast(mytb.Controls(i), ASPxTextBox).Value) & ", "
                                Case 5
                                    FieldSql += mytb.Controls(i).ID & "= PARSE('" & TryCast(mytb.Controls(i), ASPxTextBox).Value & "' AS decimal(22,8) USING 'en-us'), "
                                Case Else
                                    Exit Select
                            End Select

                        End If
                    Next
                End If

                If TypeOf mycontrol Is ASPxRoundPanel And mycontrol.Visible Then
                    Dim mytb As ASPxRoundPanel = TryCast(mycontrol, ASPxRoundPanel)
                    For i As Integer = 0 To mytb.Controls.Count - 1

                        If TypeOf mytb.Controls(i) Is ASPxTextBox Then
                            Select Case TryCast(mytb.Controls(i), ASPxTextBox).CssClass
                                Case 1
                                    FieldSql += mytb.Controls(i).ID & "='" & Trim(TryCast(mytb.Controls(i), ASPxTextBox).Value) & "', "
                                Case 2
                                    FieldSql += mytb.Controls(i).ID & "= PARSE('" & TryCast(mytb.Controls(i), ASPxTextBox).Value & "' AS decimal(22,3) USING 'en-us'), "
                                Case 3
                                    FieldSql += mytb.Controls(i).ID & "=" & Trim(TryCast(mytb.Controls(i), ASPxTextBox).Value) & ", "
                                Case 5
                                    FieldSql += mytb.Controls(i).ID & "= PARSE('" & TryCast(mytb.Controls(i), ASPxTextBox).Value & "' AS decimal(22,8) USING 'en-us'), "
                                Case Else
                                    Exit Select
                            End Select
                        End If

                        If TypeOf mytb.Controls(i) Is ASPxDateEdit Then
                            Dim mytbd As ASPxDateEdit = TryCast(mytb.Controls(i), ASPxDateEdit)
                            FieldSql += mytbd.ID & "='" & Format(mytbd.Value, "yyyy/MM/dd") & "', "
                        End If
                    Next
                End If

                If TypeOf mycontrol Is ASPxFormLayout Then
                    Dim mytb As ASPxFormLayout = TryCast(mycontrol, ASPxFormLayout)
                    For Each LitemBase As LayoutItemBase In mytb.Items
                        For Each item In TryCast(LitemBase, LayoutGroupBase).Items
                            Dim NlayoutItem = TryCast(item, LayoutItem)
                            If NlayoutItem IsNot Nothing Then
                                'LEVEL OF CONTROLS
                                For Each Ctrl As Control In NlayoutItem.Controls
                                    If TypeOf Ctrl Is ASPxTextBox Then
                                        Select Case TryCast(Ctrl, ASPxTextBox).CssClass
                                            Case 1
                                                FieldSql += Ctrl.ID & "='" & Trim(TryCast(Ctrl, ASPxTextBox).Value) & "', "
                                            Case 2
                                                FieldSql += Ctrl.ID & "= PARSE('" & TryCast(Ctrl, ASPxTextBox).Value & "' AS decimal(22,3) USING 'en-us'), "
                                            Case 3
                                                FieldSql += Ctrl.ID & "=" & Trim(TryCast(Ctrl, ASPxTextBox).Value) & ", "
                                            Case 5
                                                FieldSql += Ctrl.ID & "= PARSE('" & TryCast(Ctrl, ASPxTextBox).Value & "' AS decimal(22,8) USING 'en-us'), "
                                            Case Else
                                        End Select
                                        'Dim TT As String = Ctrl.ID.ToString
                                    End If
                                    If TypeOf Ctrl Is ASPxSpinEdit Then
                                        Dim Tctrl As ASPxSpinEdit = TryCast(Ctrl, ASPxSpinEdit)
                                        FieldSql += Ctrl.ID & "=" & Tctrl.Value.ToString & ", "
                                    End If
                                    If TypeOf Ctrl Is ASPxComboBox Then
                                        Dim Tctrl As ASPxComboBox = TryCast(Ctrl, ASPxComboBox)
                                        FieldSql += Ctrl.ID & "=" & Tctrl.Value & ", "
                                    End If
                                    If TypeOf Ctrl Is ASPxTokenBox Then
                                        Dim Tctrl As ASPxTokenBox = TryCast(Ctrl, ASPxTokenBox)
                                        FieldSql += Ctrl.ID & "='" & Tctrl.Value & "', "
                                    End If
                                    If TypeOf Ctrl Is ASPxCheckBox Then
                                        If mycontrol.ID.ToString = "RateAll" Then
                                        Else
                                            Dim tctrl As ASPxCheckBox = TryCast(Ctrl, ASPxCheckBox)
                                            FieldSql += Ctrl.ID & "='" & tctrl.Checked.ToString() & "', "
                                        End If
                                    End If
                                    If TypeOf Ctrl Is ASPxDateEdit Then
                                        Dim Tctrl As ASPxDateEdit = TryCast(Ctrl, ASPxDateEdit)
                                        FieldSql += Ctrl.ID & "='" & Format(Tctrl.Value, "yyyy/MM/dd") & "', "
                                    End If
                                    If TypeOf Ctrl Is ASPxGridLookup Then
                                        Dim tctrl As ASPxGridLookup = TryCast(Ctrl, ASPxGridLookup)
                                        If IsNothing(tctrl.Value) Then
                                            FieldSql += Ctrl.ID & "=NULL, "
                                        Else
                                            FieldSql += Ctrl.ID & "=" & tctrl.Value.ToString & ", "
                                        End If
                                    End If
                                Next
                            End If
                        Next
                    Next
                End If

                If TypeOf mycontrol Is ASPxSpinEdit Then
                    Dim mytb As ASPxSpinEdit = TryCast(mycontrol, ASPxSpinEdit)
                    FieldSql += mytb.ID & "=" & mytb.Value.ToString & ", "
                End If

                If TypeOf mycontrol Is ASPxTextBox Then
                    Dim mytb As ASPxTextBox = TryCast(mycontrol, ASPxTextBox)
                    'css case 1 -> text //// case 2 -> Float //// case 3 ->integer or big integer
                    Select Case mytb.CssClass
                        Case 1
                            FieldSql += mytb.ID & "='" & Trim(mytb.Value) & "', "
                        Case 2
                            FieldSql += mytb.ID & "= PARSE('" & mytb.Value & "' AS decimal(22,3) USING 'en-us'), "
                             '1,234.00')
                        Case 3
                            If IsNothing(mytb.Value) Then
                            Else
                                FieldSql += mytb.ID & "=" & mytb.Value & ", "
                            End If
                        Case 5
                            FieldSql += mytb.ID & "= PARSE('" & mytb.Value & "' AS decimal(22,8) USING 'en-us'), "
                        Case Else

                    End Select
                End If

                If TypeOf mycontrol Is ASPxComboBox Then
                    Dim mytb As ASPxComboBox = TryCast(mycontrol, ASPxComboBox)
                    FieldSql += mytb.ID & "=" & mytb.Value & ", "
                End If

                If TypeOf mycontrol Is ASPxTokenBox Then
                    Dim mytb As ASPxTokenBox = TryCast(mycontrol, ASPxTokenBox)
                    FieldSql += mytb.ID & "='" & mytb.Value & "', "
                End If

                If TypeOf mycontrol Is ASPxDateEdit Then
                    Dim mytb As ASPxDateEdit = TryCast(mycontrol, ASPxDateEdit)
                    FieldSql += mytb.ID & "='" & Format(mytb.Value, "yyyy/MM/dd") & "', "
                End If

                If TypeOf mycontrol Is ASPxGridLookup Then
                    Dim mytb As ASPxGridLookup = TryCast(mycontrol, ASPxGridLookup)
                    If IsNothing(mytb.Value) Then
                        FieldSql += mytb.ID & "=NULL, "
                    Else
                        FieldSql += mytb.ID & "=" & mytb.Value.ToString & ", "
                    End If
                End If

                If TypeOf mycontrol Is ASPxCheckBox Then
                    If mycontrol.ID.ToString = "RateAll" Then
                    Else
                        Dim mytb As ASPxCheckBox = TryCast(mycontrol, ASPxCheckBox)
                        FieldSql += mytb.ID & "='" & mytb.Checked.ToString() & "', "
                    End If

                End If
            Next
        End If
        Return If(FieldSql <> "",
            "Update " & GetGroupFile(sys) & " Set " & Left(FieldSql, Len(FieldSql) - 2) & " Where SubIns='" & sys & "' AND OrderNo='" & Ord & "' and EndNo=" & EndNo & " and LoadNo=" & LoadNo, "")
    End Function

    Public Function InsertData(ByRef Frm As Object, sys As String, Ord As String, EndNo As Integer, LoadNo As Integer) As String
        If Trim(Ord).Length <= 1 Then
            Return ""
        End If
        Dim FieldSql As String = "(OrderNo,EndNo,LoadNo,SubIns,"
        Dim ValueSql As String = String.Format("('{0}',{1},{2},'{3}',", Ord, EndNo, LoadNo, sys)
        If Frm.HasControls() Then
            Dim cont As ControlCollection = Frm.Controls

            For Each mycontrol As Control In cont
                If TypeOf mycontrol Is ASPxPanel And mycontrol.Visible Then
                    Dim mytb As ASPxPanel = TryCast(mycontrol, ASPxPanel)
                    For i As Integer = 0 To mytb.Controls.Count - 1
                        If TypeOf mytb.Controls(i) Is ASPxTextBox Then
                            FieldSql += mytb.Controls(i).ID & ", "
                            Select Case TryCast(mytb.Controls(i), ASPxTextBox).CssClass
                                Case 1
                                    ValueSql += "'" & Trim(TryCast(mytb.Controls(i), ASPxTextBox).Value) & "', "
                                Case 2
                                    ValueSql += "PARSE('" & TryCast(mytb.Controls(i), ASPxTextBox).Value & "' AS decimal(22,3) USING 'en-us') , "
                                Case 3
                                    ValueSql += TryCast(mytb.Controls(i), ASPxTextBox).Value & ", "
                                Case 5
                                    ValueSql += "PARSE('" & TryCast(mytb.Controls(i), ASPxTextBox).Value & "' AS decimal(22,8) USING 'en-us') , "
                                Case Else
                                    Exit Select
                            End Select
                        End If
                    Next
                End If

                If TypeOf mycontrol Is ASPxRoundPanel And mycontrol.Visible Then
                    Dim mytb As ASPxRoundPanel = TryCast(mycontrol, ASPxRoundPanel)
                    For i As Integer = 0 To mytb.Controls.Count - 1
                        If TypeOf mytb.Controls(i) Is ASPxTextBox Then
                            FieldSql += mytb.Controls(i).ID & ", "
                            Select Case TryCast(mytb.Controls(i), ASPxTextBox).CssClass
                                Case 1
                                    ValueSql += "'" & Trim(TryCast(mytb.Controls(i), ASPxTextBox).Value) & "', "
                                Case 2
                                    ValueSql += "PARSE('" & TryCast(mytb.Controls(i), ASPxTextBox).Value & "' AS decimal(22,3) USING 'en-us') , "
                                Case 3
                                    ValueSql += TryCast(mytb.Controls(i), ASPxTextBox).Value & ", "
                                Case 5
                                    ValueSql += "PARSE('" & TryCast(mytb.Controls(i), ASPxTextBox).Value & "' AS decimal(22,8) USING 'en-us') , "
                                Case Else
                                    Exit Select
                            End Select
                        End If
                        If TypeOf mytb.Controls(i) Is ASPxDateEdit Then
                            'Dim mytbd As ASPxDateEdit = DirectCast(mytb.Controls(i), ASPxDateEdit)
                            FieldSql += mytb.Controls(i).ID & ","
                            ValueSql += "'" & Format(TryCast(mytb.Controls(i), ASPxDateEdit).Value, "yyyy/MM/dd") & "',"
                        End If
                    Next
                End If

                If TypeOf mycontrol Is ASPxFormLayout Then
                    Dim mytb As ASPxFormLayout = TryCast(mycontrol, ASPxFormLayout)
                    For Each LitemBase As LayoutItemBase In mytb.Items
                        For Each item In TryCast(LitemBase, LayoutGroupBase).Items
                            Dim NlayoutItem = TryCast(item, LayoutItem)
                            If NlayoutItem IsNot Nothing Then
                                'LEVEL OF CONTROLS
                                For Each Ctrl As Control In NlayoutItem.Controls
                                    If TypeOf Ctrl Is ASPxTextBox Then
                                        FieldSql += Ctrl.ID & ", "

                                        Select Case TryCast(Ctrl, ASPxTextBox).CssClass
                                            Case 1
                                                ValueSql += "'" & Trim(TryCast(Ctrl, ASPxTextBox).Value) & "', "
                                            Case 2
                                                ValueSql += "PARSE('" & TryCast(Ctrl, ASPxTextBox).Value & "' AS decimal(22,3) USING 'en-us') , "
                                            Case 3
                                                ValueSql += TryCast(Ctrl, ASPxTextBox).Value & ", "
                                            Case 5
                                                ValueSql += "PARSE('" & TryCast(Ctrl, ASPxTextBox).Value & "' AS decimal(22,8) USING 'en-us') , "
                                            Case Else
                                        End Select
                                        'Dim TT As String = Ctrl.ID.ToString
                                    End If

                                    If TypeOf Ctrl Is ASPxDateEdit Then
                                        FieldSql += Ctrl.ID & ","
                                        ValueSql += "'" & Format(TryCast(Ctrl, ASPxDateEdit).Value, "yyyy/MM/dd") & "',"
                                    End If

                                    If TypeOf Ctrl Is ASPxSpinEdit Then
                                        Dim TCtrl As ASPxSpinEdit = TryCast(Ctrl, ASPxSpinEdit)
                                        FieldSql += Ctrl.ID.ToString & ","
                                        ValueSql += TCtrl.Text & ","
                                    End If

                                    If TypeOf Ctrl Is ASPxComboBox Then
                                        Dim Tctrl As ASPxComboBox = TryCast(Ctrl, ASPxComboBox)
                                        FieldSql += Ctrl.ID.ToString & ","
                                        ValueSql += Tctrl.Value & ","
                                    End If

                                    If TypeOf Ctrl Is ASPxTokenBox Then
                                        Dim Tctrl As ASPxTokenBox = TryCast(Ctrl, ASPxTokenBox)
                                        FieldSql += Ctrl.ID.ToString & ","
                                        ValueSql += "'" & Tctrl.Value & "',"
                                    End If

                                    If TypeOf Ctrl Is ASPxCheckBox Then
                                        Dim Tctrl As ASPxCheckBox = TryCast(Ctrl, ASPxCheckBox)
                                        FieldSql += Ctrl.ID.ToString & ","
                                        ValueSql += "'" & Tctrl.Checked.ToString() & "', "
                                    End If

                                    If TypeOf Ctrl Is ASPxGridLookup Then
                                        Dim Tctrl As ASPxGridLookup = TryCast(Ctrl, ASPxGridLookup)
                                        If IsNothing(Tctrl.Value) Then
                                            FieldSql += ""
                                            ValueSql += ""
                                        Else
                                            FieldSql += Ctrl.ID.ToString & ","
                                            ValueSql += Tctrl.Value.ToString & ","
                                        End If
                                    End If
                                Next
                            End If
                        Next
                    Next
                End If

                If TypeOf mycontrol Is ASPxTextBox Then
                    'css case 1 -> text //// case 2 -> Float //// case 3 ->integer or big integer
                    Dim mytb As ASPxTextBox = TryCast(mycontrol, ASPxTextBox)
                    Select Case mytb.CssClass
                        Case 1
                            FieldSql += mytb.ID.ToString & ","
                            ValueSql += "'" & Trim(mytb.Text) & "',"
                        Case 2
                            FieldSql += mytb.ID.ToString & ","
                            ValueSql += "PARSE('" & mytb.Value & "' AS decimal(22,3) USING 'en-us'),"
                        Case 3
                            If IsNothing(mytb.Text) Then
                            Else
                                FieldSql += mytb.ID.ToString & ","
                                ValueSql += mytb.Text & ","
                            End If

                        Case 5
                            FieldSql += mytb.ID.ToString & ","
                            ValueSql += "PARSE('" & mytb.Value & "' AS decimal(22,8) USING 'en-us'),"
                        Case Else
                            Exit Select
                    End Select
                End If

                If TypeOf mycontrol Is ASPxSpinEdit Then

                    Dim mytb As ASPxSpinEdit = TryCast(mycontrol, ASPxSpinEdit)
                    FieldSql += mytb.ID.ToString & ","
                    ValueSql += mytb.Text & ","

                End If

                If TypeOf mycontrol Is ASPxComboBox Then

                    Dim mytb As ASPxComboBox = TryCast(mycontrol, ASPxComboBox)
                    FieldSql += mytb.ID.ToString & ","
                    ValueSql += mytb.Value & ","
                End If

                If TypeOf mycontrol Is ASPxTokenBox Then

                    Dim mytb As ASPxTokenBox = TryCast(mycontrol, ASPxTokenBox)
                    FieldSql += mytb.ID.ToString & ","
                    ValueSql += "'" & mytb.Value & "',"
                End If

                If TypeOf mycontrol Is ASPxDateEdit Then

                    Dim mytb As ASPxDateEdit = TryCast(mycontrol, ASPxDateEdit)
                    FieldSql += mytb.ID.ToString & ","
                    ValueSql += "'" & Format(mytb.Value, "yyyy/MM/dd") & "',"
                End If

                If TypeOf mycontrol Is ASPxGridLookup Then
                    Dim mytb As ASPxGridLookup = TryCast(mycontrol, ASPxGridLookup)
                    If IsNothing(mytb.Value) Then
                        FieldSql += ""
                        ValueSql += ""
                    Else
                        FieldSql += mytb.ID.ToString & ","
                        ValueSql += mytb.Value.ToString & ","
                    End If
                End If

                If TypeOf mycontrol Is ASPxCheckBox Then
                    Dim mytb As ASPxCheckBox = TryCast(mycontrol, ASPxCheckBox)
                    FieldSql += mytb.ID.ToString & ","
                    ValueSql += "'" & mytb.Checked.ToString() & "', "
                End If
            Next
        End If
        InsertData = String.Format("INSERT INTO {0} {1}) values{2})", GetGroupFile(sys), FieldSql, ValueSql)
        InsertData = InsertData.Replace(",)", ")")
        Return InsertData.Replace(", )", ")")
    End Function

    Public Function UpdatePolicyData(Frm As Object, sys As String, Br As String, Ord As String, EndNo As Integer, LoadNo As Integer) As String
        Dim FieldSqlP As String = "Update PolicyFile Set Branch='" & Br & "' ,EntryDate=CONVERT(DATETIME,getdate(),111) ,UserName='" & HttpContext.Current.Session("UserID") & "', "

        If Frm.HasControls() Then
            Dim cont As ControlCollection = Frm.Controls
            For Each mycontrol As Control In cont
                If TypeOf mycontrol Is ASPxRoundPanel And mycontrol.Visible Then

                    Dim mytb As ASPxRoundPanel = TryCast(mycontrol, ASPxRoundPanel)
                    For i As Integer = 0 To mytb.Controls.Count - 1
                        If TypeOf mytb.Controls(i) Is ASPxTextBox Then
                            Select Case TryCast(mytb.Controls(i), ASPxTextBox).CssClass
                                Case 1
                                    FieldSqlP += mytb.Controls(i).ID & "='" & Trim(TryCast(mytb.Controls(i), ASPxTextBox).Value) & "', "
                                Case 2
                                    FieldSqlP += mytb.Controls(i).ID & "= PARSE('" & TryCast(mytb.Controls(i), ASPxTextBox).Value & "' AS decimal(22,3) USING 'en-us'), "
                                Case 3
                                    FieldSqlP += mytb.Controls(i).ID & "=" & Trim(TryCast(mytb.Controls(i), ASPxTextBox).Value) & ", "
                                Case 5
                                    FieldSqlP += mytb.Controls(i).ID & "= PARSE('" & DirectCast(mytb.Controls(i), ASPxTextBox).Value & "' AS decimal(22,8) USING 'en-us'), "
                                Case 10
                                    Dim IsNewCust As New DataSet
                                    Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                                        If con.State = ConnectionState.Open Then
                                            con.Close()
                                        Else
                                        End If
                                        con.Open()
                                        Dim dbadapter = New SqlDataAdapter("select RecDate,UserName from CustomerFile WHERE CusTNo=" & GethiddenField(FindControlRecursive(Frm, "CustNo")) & " ", con)
                                        dbadapter.Fill(IsNewCust)
                                        'LastGroup = RecSet("select GroupNo From " & GetGroupFile(sys) & " where OrderNo='" & Trim(OrderNo) & "' and GroupNo=" & Group, Conn)
                                        If IsNewCust.Tables(0).Rows.Count = 0 Then
                                            Return False
                                        Else
                                            If DateDiff(DateInterval.Minute, IsNewCust.Tables(0).Rows(0)("RecDate"), Now()) > 30 Then
                                                ExecConn("Update CustomerFile Set " & mytb.Controls(i).ID & " = '" & TryCast(mytb.Controls(i), ASPxTextBox).Value.ToString.TrimEnd & "' WHERE CusTNo=" & GethiddenField(FindControlRecursive(Frm, "CustNo")) & "", Conn)
                                            Else
                                                If IsNewCust.Tables(0).Rows(0)("UserName") = HttpContext.Current.Session("User") Then
                                                    ExecConn("Update CustomerFile Set CustName = '" & GetDxTextValue(FindControlRecursive(Frm, "txtCustomerSearch")).ToString.TrimEnd & "' WHERE CusTNo=" & GethiddenField(FindControlRecursive(Frm, "CustNo")) & "", Conn)
                                                Else

                                                End If
                                                ExecConn("Update CustomerFile Set " & mytb.Controls(i).ID & " = '" & TryCast(mytb.Controls(i), ASPxTextBox).Value.ToString.TrimEnd & "' WHERE CusTNo=" & GethiddenField(FindControlRecursive(Frm, "CustNo")) & "", Conn)
                                            End If
                                        End If
                                        con.Close()
                                    End Using
                                Case Else
                                    Exit Select
                            End Select

                        End If
                        If TypeOf mytb.Controls(i) Is ASPxGridLookup Then
                            If IsNothing(TryCast(mytb.Controls(i), ASPxGridLookup).Value) Then
                                FieldSqlP += mytb.Controls(i).ID & "=0 , "
                            Else
                                FieldSqlP += mytb.Controls(i).ID & "=" & TryCast(mytb.Controls(i), ASPxGridLookup).Value.ToString & ", "
                            End If
                        End If
                        If TypeOf mytb.Controls(i) Is ASPxComboBox Then
                            FieldSqlP += mytb.Controls(i).ID & "=" & TryCast(mytb.Controls(i), ASPxComboBox).Value & ", "
                        End If
                        If TypeOf mytb.Controls(i) Is ASPxSpinEdit Then
                            FieldSqlP += mytb.Controls(i).ID & "=" & TryCast(mytb.Controls(i), ASPxSpinEdit).Value.ToString & ", "
                        End If
                        If TypeOf mytb.Controls(i) Is ASPxRadioButtonList Then
                            If TryCast(mytb.Controls(i), ASPxRadioButtonList).Value Is Nothing Then
                            Else
                                FieldSqlP += mytb.Controls(i).ID & "=" & TryCast(mytb.Controls(i), ASPxRadioButtonList).Value.ToString & ", "
                            End If
                        End If
                        If TypeOf mytb.Controls(i) Is ASPxDateEdit Then
                            FieldSqlP += mytb.Controls(i).ID & "='" & Format(CDate(TryCast(mytb.Controls(i), ASPxDateEdit).Value), "yyyy/MM/dd") & "', "
                        End If
                        If TypeOf mytb.Controls(i) Is HiddenField Then
                            FieldSqlP += mytb.Controls(i).ID & "=" & TryCast(mytb.Controls(i), HiddenField).Value & ", "
                        End If
                    Next
                End If
            Next
        End If
        Return " " & Left(FieldSqlP, Len(FieldSqlP) - 2) & " Where SubIns='" & sys & "' AND OrderNo='" & Ord & "' and EndNo=" & EndNo & " and LoadNo=" & LoadNo
    End Function

    Public Function InsertPolicyData(Frm As Object, sys As String, Br As String) As String

        Dim FieldSqlP As String = "(SubIns, Branch, EntryDate, UserName, "
        Dim ValueSqlP As String = String.Format("('{0}' ,'{1}' ,{2} ,'{3}' ,", sys, Br, " CONVERT(DATETIME,getdate(),111) ", HttpContext.Current.Session("UserID"))
        If Frm.HasControls() Then
            Dim cont As ControlCollection = Frm.Controls

            For Each mycontrol As Control In cont
                If TypeOf mycontrol Is ASPxRoundPanel And mycontrol.Visible Then
                    Dim mytb As ASPxRoundPanel = TryCast(mycontrol, ASPxRoundPanel)
                    For i As Integer = 0 To mytb.Controls.Count - 1
                        If TypeOf mytb.Controls(i) Is ASPxTextBox Then
                            If mytb.ID.ToString = "OrderNo" And Trim(TryCast(mytb.Controls(i), ASPxTextBox).Value) = "0" Then
                                Return ""
                            End If
                            'If mytb.Controls(i).ID = "CustNameE" Or mytb.Controls(i).ID = "TelNo" Or mytb.Controls(i).ID = "Address" Then
                            '    GoTo 10
                            'End If

                            Select Case TryCast(mytb.Controls(i), ASPxTextBox).CssClass
                                Case 1
                                    FieldSqlP += mytb.Controls(i).ID & ", "
                                    ValueSqlP += "'" & Trim(TryCast(mytb.Controls(i), ASPxTextBox).Value) & "', "
                                Case 2
                                    FieldSqlP += mytb.Controls(i).ID & ", "
                                    ValueSqlP += "PARSE('" & IIf(String.IsNullOrEmpty(TryCast(mytb.Controls(i), ASPxTextBox).Value), 0, TryCast(mytb.Controls(i), ASPxTextBox).Value) & "' AS decimal(22,3) USING 'en-us') , "
                                Case 3
                                    FieldSqlP += mytb.Controls(i).ID & ", "
                                    ValueSqlP += TryCast(mytb.Controls(i), ASPxTextBox).Value & ", "
                                Case 5
                                    FieldSqlP += mytb.Controls(i).ID & ", "
                                    ValueSqlP += "PARSE('" & TryCast(mytb.Controls(i), ASPxTextBox).Value & "' AS decimal(22,8) USING 'en-us') , "
                                Case 10
                                    Dim IsNewCust As New DataSet
                                    Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                                        If con.State = ConnectionState.Open Then
                                            con.Close()
                                        Else
                                        End If
                                        con.Open()
                                        Dim dbadapter = New SqlDataAdapter("select RecDate,UserName from CustomerFile WHERE CusTNo=" & GethiddenField(FindControlRecursive(Frm, "CustNo")) & " ", con)
                                        dbadapter.Fill(IsNewCust)
                                        'LastGroup = RecSet("select GroupNo From " & GetGroupFile(sys) & " where OrderNo='" & Trim(OrderNo) & "' and GroupNo=" & Group, Conn)
                                        If IsNewCust.Tables(0).Rows.Count = 0 Then
                                            Return False
                                        Else
                                            If DateDiff(DateInterval.Minute, IsNewCust.Tables(0).Rows(0)("RecDate"), Now()) > 30 Then
                                                ExecConn("Update CustomerFile Set " & mytb.Controls(i).ID & " = '" & TryCast(mytb.Controls(i), ASPxTextBox).Value.ToString.TrimEnd & "' WHERE CusTNo=" & GethiddenField(FindControlRecursive(Frm, "CustNo")) & "", Conn)
                                            Else
                                                If IsNewCust.Tables(0).Rows(0)("UserName") = HttpContext.Current.Session("User") Then
                                                    ExecConn("Update CustomerFile Set CustName = '" & GetDxTextValue(FindControlRecursive(Frm, "txtCustomerSearch")).ToString.TrimEnd & "' WHERE CusTNo=" & GethiddenField(FindControlRecursive(Frm, "CustNo")) & "", Conn)
                                                Else

                                                End If
                                                ExecConn("Update CustomerFile Set " & mytb.Controls(i).ID & " = '" & TryCast(mytb.Controls(i), ASPxTextBox).Value.ToString.TrimEnd & "' WHERE CusTNo=" & GethiddenField(FindControlRecursive(Frm, "CustNo")) & "", Conn)
                                            End If
                                        End If
                                        con.Close()
                                    End Using
                                Case Else
                                    Exit Select
                            End Select
                        End If
                        If TypeOf mytb.Controls(i) Is ASPxComboBox Then
                            FieldSqlP += mytb.Controls(i).ID & ", "
                            ValueSqlP += TryCast(mytb.Controls(i), ASPxComboBox).Value & ", "
                        End If
                        If TypeOf mytb.Controls(i) Is ASPxGridLookup Then
                            FieldSqlP += mytb.Controls(i).ID & ", "
                            If IsNothing(TryCast(mytb.Controls(i), ASPxGridLookup).Value) Then
                                ValueSqlP += "0, "
                            Else
                                ValueSqlP += TryCast(mytb.Controls(i), ASPxGridLookup).Value.ToString & ", "
                            End If
                        End If
                        If TypeOf mytb.Controls(i) Is ASPxDateEdit Then
                            FieldSqlP += mytb.Controls(i).ID & ", "
                            ValueSqlP += "'" & Format(CDate(TryCast(mytb.Controls(i), ASPxDateEdit).Value), "yyyy/MM/dd") & "', "
                        End If
                        If TypeOf mytb.Controls(i) Is ASPxSpinEdit Then
                            FieldSqlP += mytb.Controls(i).ID & ", "
                            ValueSqlP += TryCast(mytb.Controls(i), ASPxSpinEdit).Value.ToString & ", "
                        End If
                        If TypeOf mytb.Controls(i) Is ASPxRadioButtonList Then
                            If TryCast(mytb.Controls(i), ASPxRadioButtonList).Value Is Nothing Then
                            Else
                                FieldSqlP += mytb.Controls(i).ID & ", "
                                ValueSqlP += TryCast(mytb.Controls(i), ASPxRadioButtonList).Value.ToString & ", "
                            End If
                        End If
                        If TypeOf mytb.Controls(i) Is HiddenField Then
                            FieldSqlP += mytb.Controls(i).ID & ", "
                            ValueSqlP += TryCast(mytb.Controls(i), HiddenField).Value & ", "
                        End If
                    Next
                End If
            Next

        End If
        InsertPolicyData = String.Format("INSERT INTO PolicyFile {0}) values{1})", FieldSqlP, ValueSqlP)
        Return InsertPolicyData.Replace(", )", ")")
    End Function

    Public Function Getwakalavalue(Sys As String) As Double
        Dim WakalaValue As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("Select WakalaValue from systems where sysno=(Select DISTINCT MainSys from SubSystems where SubSysNo='" & Sys & "' and BRANCH=dbo.[Maincenter]())", con)
            Dim unused = dbadapter.Fill(WakalaValue)
            Return WakalaValue.Tables(0).Rows(0)(0)
            con.Close()
        End Using
    End Function

    Public Sub MainCalcDx(ByRef PageControl As Object, Net As Double, Sys As String, EndNo As Integer, SpCase As Integer, Optional CancelAll As Boolean = False)

        If Not CancelAll And Net > 0 Then
            If CDbl(TryCast(FindControlRecursive(PageControl, "Commision"), ASPxTextBox).Value) = 0 _
            And TryCast(FindControlRecursive(PageControl, "Broker"), ASPxGridLookup).Value = 0 Then
            Else
                If CDbl(TryCast(FindControlRecursive(PageControl, "Commision"), ASPxTextBox).Value) <> 0 And
                    TryCast(FindControlRecursive(PageControl, "Broker"), ASPxGridLookup).Value = 0 Then

                    SetDxtxtValue(FindControlRecursive(PageControl, "Commision"), 0)

                ElseIf TryCast(FindControlRecursive(PageControl, "Broker"), ASPxGridLookup).Value <> 0 And
                    CDbl(TryCast(FindControlRecursive(PageControl, "Commision"), ASPxTextBox).Value) = 0 Then

                    TryCast(FindControlRecursive(PageControl, "Broker"), ASPxGridLookup).Value = 0
                End If
            End If
        Else
            If CancelAll Then SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), CDbl(GetDxTextValue(FindControlRecursive(PageControl, "NETPRM"))) + CDbl(GetDxTextValue(FindControlRecursive(PageControl, "TAXPRM"))) + CDbl(GetDxTextValue(FindControlRecursive(PageControl, "CONPRM"))) + CDbl(GetDxTextValue(FindControlRecursive(PageControl, "STMPRM"))) + CDbl(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM")))) : Exit Sub
        End If

        If PageControl Is Nothing Then
            Throw New ArgumentNullException(NameOf(PageControl))
        End If

        If String.IsNullOrEmpty(Sys) Then
            Throw New ArgumentException($"'{NameOf(Sys)}' cannot be null or empty.", NameOf(Sys))
        End If

        Dim IssuPrm As Double

        Dim Curr = GetComboValue(FindControlRecursive(PageControl, "Currency"))
        Dim Formating As String = IIf(Curr = 1, "###,#0.000", "###,#0.00")

        SetDxtxtValue(FindControlRecursive(PageControl, "NETPRM"), Format(Net, Formating))
        Net = CDbl(Format(Net, Formating))

        SetDxtxtValue(FindControlRecursive(PageControl, "NETPRM"), Format(Net, Formating))

        Net = CDbl(Format(Net, "0.000"))

        SetDxtxtValue(FindControlRecursive(PageControl, "NETPRM"), Format(Net, Formating))

        'If Net < 0 Then
        '    SetDxtxtValue(FindControlRecursive(PageControl, "TAXPRM"), Format(RndTax(CDbl(Net) * 0.0), Formating))

        'Else
        SetDxtxtValue(FindControlRecursive(PageControl, "TAXPRM"), Format(RndTax(Net * 0.01), Formating))

        'End If
        If GetMainSystem(Sys) = 2 Then
            SetDxtxtValue(FindControlRecursive(PageControl, "CONPRM"), Format(Net * 0.01 / 4, Formating))
        Else
            SetDxtxtValue(FindControlRecursive(PageControl, "CONPRM"), Format(Net * 0.01 / 2, Formating))
        End If


        'STAMP
        If GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) = 0 Or GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) = GetStmVal(Sys, EndNo) Then
            SetDxtxtValue(FindControlRecursive(PageControl, "STMPRM"), Format(GetStmVal(Sys, EndNo), Formating))
        Else
            SetDxtxtValue(FindControlRecursive(PageControl, "STMPRM"), GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")))
        End If

        If Net = 12.8 And HttpContext.Current.Session("UserID") = 12108 Then
            SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(0.136, Formating))
            SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + RndTax(Net * 0.01) + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) + 0.136, Formating))
            Exit Sub
        End If

        If CancelAll And EndNo <> 0 Then
            IssuPrm = GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))
            SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM")))
        Else
            If Sys = "PH" Or Sys = "MN" Then
                IssuPrm = Format(GetIssuVal(Sys, EndNo), Formating)
                SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(GetIssuVal(Sys, EndNo), Formating))
            Else
                If Net < 0 And HttpContext.Current.Session("ClcIss") = 0 Then
                    IssuPrm = 0
                    SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(RndTax(Net * 0.0), Formating))
                    SetDxtxtValue(FindControlRecursive(PageControl, "STMPRM"), Format(RndTax(Net * 0.0), Formating))
                Else
                    If Fix(CDbl(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM")))) <> GetIssuVal(Sys, EndNo) Or CDbl(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))) = 0 Then
                        Select Case Sys
                            Case "MA", "MC", "MB", "PA", "FR", "FB", "ER", "CR", "CM", "EL", "CL", "FG", "PI", "HL"
                                IssuPrm = Format(CDbl(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))), Formating)
                                SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(CDbl(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))), Formating))
                            Case Else
                                IssuPrm = IIf(CDbl(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))) = 0, Format(GetIssuVal(Sys, EndNo), Formating), IIf(EndNo = 0, Fix(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))), Format(GetIssuVal(Sys, EndNo), Formating)))
                                SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), IIf(EndNo = 0, Fix(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))), Format(GetIssuVal(Sys, EndNo), Formating)))
                        End Select
                    Else
                        Select Case Sys
                            Case "MA", "MC", "MB", "PA", "FR", "FB", "ER", "CR", "CM", "EL", "CL", "FG", "PI", "HL"
                                IssuPrm = GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))
                                SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM")))
                            Case Else
                                IssuPrm = Format(GetIssuVal(Sys, EndNo), Formating)
                                SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(GetIssuVal(Sys, EndNo), Formating))
                        End Select
                    End If
                End If
            End If
        End If

        Select Case SpCase
            Case 1, 0
                'If Net < 0 Then
                '    SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + RndTax(0) + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) + IssuPrm, Formating))
                'Else
                SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + RndTax(Net * 0.01) + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) + IssuPrm, Formating))
                    'End If

            Case 2
                SetDxtxtValue(FindControlRecursive(PageControl, "TAXPRM"), Format(0, Formating))
                SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) + IssuPrm, "###,#0.000"))
            Case 3
                SetDxtxtValue(FindControlRecursive(PageControl, "TAXPRM"), Format(0, Formating))
                SetDxtxtValue(FindControlRecursive(PageControl, "STMPRM"), Format(0, Formating))
                SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + IssuPrm, Formating))

            Case 4
                SetDxtxtValue(FindControlRecursive(PageControl, "TAXPRM"), Format(0, Formating))

                SetDxtxtValue(FindControlRecursive(PageControl, "STMPRM"), Format(0, Formating))

                SetDxtxtValue(FindControlRecursive(PageControl, "CONPRM"), Format(0, Formating))

                SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + IssuPrm, Formating))
            Case Else
                Exit Select
        End Select
        Dim tempprm = CDbl(GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM")))

        Select Case Sys
            Case "01", "Or", "27", "08", "07"
                If EndNo = 0 Or GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM")) >= 0 Then
                    If GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM")) = 0 Or Fix(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))) = GetIssuVal(Sys, EndNo) Then
                        SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(RndTax(GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM"))), Formating))
                        SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(GetIssuVal(Sys, EndNo) + (GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM")) - tempprm), Formating))
                    Else
                        SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(RndTax(GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM"))), Formating))
                        SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(Fix(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))) + (GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM")) - tempprm), Formating))
                    End If
                Else

                End If

            Case "PH"
                SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(4.64, Formating))
                SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + RndTax(Net * 0.01) + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) + 4.64, Formating))
            Case "MA", "MC", "MB", "PA", "FR", "FB", "ER", "CR", "CM", "EL", "CL", "FG", "PI", "HL"
                'If Curr <> 1 Then

                'Else
                'GoTo clc
                'End If
            Case Else
clc:            If GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM")) = 0 Or Fix(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))) = GetIssuVal(Sys, EndNo) Then
                    SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(RndTax(GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM"))), Formating))
                    SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(GetIssuVal(Sys, EndNo) + (GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM")) - tempprm), Formating))
                Else
                    SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(RndTax(GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM"))), Formating))
                    SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(Fix(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))) + (GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM")) - tempprm), Formating))
                End If
        End Select
    End Sub

    '    Public Sub MainCalcDx(ByRef PageControl As Object, Net As Double, Sys As String, EndNo As Integer, SpCase As Integer, Optional CancelAll As Boolean = False)

    '        If Not CancelAll And Net > 0 Then
    '            If CDbl(TryCast(FindControlRecursive(PageControl, "Commision"), ASPxTextBox).Value) = 0 _
    '            And TryCast(FindControlRecursive(PageControl, "Broker"), ASPxGridLookup).Value = 0 Then
    '            Else
    '                If CDbl(TryCast(FindControlRecursive(PageControl, "Commision"), ASPxTextBox).Value) <> 0 And
    '                    TryCast(FindControlRecursive(PageControl, "Broker"), ASPxGridLookup).Value = 0 Then

    '                    SetDxtxtValue(FindControlRecursive(PageControl, "Commision"), 0)

    '                ElseIf TryCast(FindControlRecursive(PageControl, "Broker"), ASPxGridLookup).Value <> 0 And
    '                    CDbl(TryCast(FindControlRecursive(PageControl, "Commision"), ASPxTextBox).Value) = 0 Then

    '                    TryCast(FindControlRecursive(PageControl, "Broker"), ASPxGridLookup).Value = 0
    '                End If
    '            End If
    '        End If

    '        If PageControl Is Nothing Then
    '            Throw New ArgumentNullException(NameOf(PageControl))
    '        End If

    '        If String.IsNullOrEmpty(Sys) Then
    '            Throw New ArgumentException($"'{NameOf(Sys)}' cannot be null or empty.", NameOf(Sys))
    '        End If

    '        Dim IssuPrm As Double

    '        Dim Curr = GetComboValue(FindControlRecursive(PageControl, "Currency"))
    '        Dim Formating As String = IIf(Curr = 1, "###,#0.000", "###,#0.00")

    '        SetDxtxtValue(FindControlRecursive(PageControl, "NETPRM"), Format(Net, Formating))
    '        Net = CDbl(Format(Net, Formating))

    '        SetDxtxtValue(FindControlRecursive(PageControl, "NETPRM"), Format(Net, Formating))

    '        Net = CDbl(Format(Net, "0.000"))

    '        SetDxtxtValue(FindControlRecursive(PageControl, "NETPRM"), Format(Net, Formating))

    '        'If Net < 0 Then
    '        '    SetDxtxtValue(FindControlRecursive(PageControl, "TAXPRM"), Format(RndTax(CDbl(Net) * 0.0), Formating))

    '        'Else
    '        SetDxtxtValue(FindControlRecursive(PageControl, "TAXPRM"), Format(RndTax(Net * 0.01), Formating))

    '        'End If

    '        SetDxtxtValue(FindControlRecursive(PageControl, "CONPRM"), Format(Net * 0.01 / 2, Formating))

    '        'STAMP
    '        If GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) = 0 Or GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) = GetStmVal(Sys, EndNo) Then
    '            SetDxtxtValue(FindControlRecursive(PageControl, "STMPRM"), Format(GetStmVal(Sys, EndNo), Formating))
    '        Else
    '            SetDxtxtValue(FindControlRecursive(PageControl, "STMPRM"), If(Net < 0 And Sys = "OR" And Not CancelAll, GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) * -1, GetDxTextValue(FindControlRecursive(PageControl, "STMPRM"))))
    '        End If
    '        If CancelAll And EndNo <> 0 Then
    '            IssuPrm = GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))
    '            SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM")))
    '        Else
    '            If Net <= 0 And Sys = "OR" Then SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + GetDxTextValue(FindControlRecursive(PageControl, "TAXPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM")), Formating)) : Exit Sub
    '            If Sys = "PH" Or Sys = "MN" Then
    '                IssuPrm = Format(GetIssuVal(Sys, EndNo), Formating)
    '                SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(GetIssuVal(Sys, EndNo), Formating))
    '                GoTo finish
    '            Else
    '                If CancelAll Then GoTo Cont
    '                If Net <= 0 And Sys = "OR" Then SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + GetDxTextValue(FindControlRecursive(PageControl, "TAXPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM")), Formating)) : Exit Sub
    '                If Sys = "01" And Net < 0 And EndNo = 1 Then GoTo Cont
    '                If Sys = "01" And EndNo = 0 And DateDiff(DateInterval.Day, GetDateedit(FindControlRecursive(PageControl, "CoverFrom")), GetDateedit(FindControlRecursive(PageControl, "CoverTo"))) <= 90 Then
    '                    Select Case DateDiff(DateInterval.Day, GetDateedit(FindControlRecursive(PageControl, "CoverFrom")), GetDateedit(FindControlRecursive(PageControl, "CoverTo")))
    '                        Case 0 To 15
    '                            SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(0.568, Formating))
    '                        Case 16 To 30
    '                            SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(0.136, Formating))
    '                        Case 31 To 60
    '                            SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(0.704, Formating))
    '                        Case 61 To 90
    '                            SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(0.272, Formating))
    '                        Case Else
    '                    End Select

    '                    SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + RndTax(Net * 0.01) + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM")), Formating))
    '                    Exit Sub
    'Cont:           Else
    '                    If Fix(CDbl(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM")))) <> GetIssuVal(Sys, EndNo) Or CDbl(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))) = 0 Then
    '                        Select Case Sys
    '                            Case "MA", "09"
    '                                IssuPrm = Format(CDbl(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))), Formating)
    '                                SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(CDbl(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))), Formating))
    '                            Case Else
    '                                IssuPrm = IIf(CDbl(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))) = 0, Format(GetIssuVal(Sys, EndNo), Formating), IIf(EndNo = 0, Fix(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))), Format(Fix(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))), Formating)))
    '                                SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), IIf(EndNo = 0, Fix(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))), Format(Fix(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))), Formating)))
    '                        End Select
    '                    Else
    '                        Select Case Sys
    '                            Case "MA", "09"
    '                                IssuPrm = GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))
    '                                SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM")))
    '                            Case Else
    '                                IssuPrm = Format(GetIssuVal(Sys, EndNo), Formating)
    '                                SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(GetIssuVal(Sys, EndNo), Formating))
    '                        End Select
    '                    End If
    '                End If
    '            End If
    '        End If

    'finish: Select Case SpCase
    '            Case 1, 0
    '                'If Net < 0 Then
    '                '    SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + RndTax(0) + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) + IssuPrm, Formating))
    '                'Else
    '                SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + GetDxTextValue(FindControlRecursive(PageControl, "TAXPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) + IssuPrm, Formating))
    '                    'End If
    '            Case 2
    '                SetDxtxtValue(FindControlRecursive(PageControl, "TAXPRM"), Format(0, Formating))
    '                SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) + IssuPrm, "###,#0.000"))
    '            Case 3
    '                SetDxtxtValue(FindControlRecursive(PageControl, "TAXPRM"), Format(0, Formating))
    '                SetDxtxtValue(FindControlRecursive(PageControl, "STMPRM"), Format(0, Formating))
    '                SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + IssuPrm, Formating))
    '            Case 4
    '                SetDxtxtValue(FindControlRecursive(PageControl, "TAXPRM"), Format(0, Formating))

    '                SetDxtxtValue(FindControlRecursive(PageControl, "STMPRM"), Format(0, Formating))

    '                SetDxtxtValue(FindControlRecursive(PageControl, "CONPRM"), Format(0, Formating))

    '                SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + IssuPrm, Formating))
    '            Case Else
    '                Exit Select
    '        End Select
    '        Dim tempprm = CDbl(GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM")))
    '        Select Case Sys
    '            Case "01", "Or", "27", "07"
    '                If EndNo = 0 Or GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM")) >= 0 Then
    '                    If GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM")) = 0 Or Fix(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))) = GetIssuVal(Sys, EndNo) Then
    '                        SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(RndTax(GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM"))), Formating))
    '                        SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(GetIssuVal(Sys, EndNo) + (GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM")) - tempprm), Formating))
    '                    Else
    '                        SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(RndTax(GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM"))), Formating))
    '                        SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(Fix(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))) + (GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM")) - tempprm), Formating))
    '                    End If
    '                Else

    '                End If

    '            Case "PH"
    '                SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(4.64, Formating))
    '                SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + RndTax(Net * 0.01) + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) + 4.64, Formating))
    '                'Case "MA", "MC", "MB", "PA", "FR", "FB", "ER", "CR", "CM", "EL", "CL", "FG", "PI", "HL"
    '                '    'If Curr <> 1 Then

    '                '    'Else
    '                '    'GoTo clc
    '                '    'End If
    '            Case Else
    'clc:            If GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM")) = 0 Or Fix(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))) = GetIssuVal(Sys, EndNo) Then
    '                    SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(RndTax(GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM"))), Formating))
    '                    SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(GetIssuVal(Sys, EndNo) + (GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM")) - tempprm), Formating))
    '                Else
    '                    SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(RndOne(GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM"))), Formating))
    '                    SetDxtxtValue(FindControlRecursive(PageControl, "ISSPRM"), Format(Fix(GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM"))) + (GetDxTextValue(FindControlRecursive(PageControl, "TOTPRM")) - tempprm), Formating))
    '                End If
    '        End Select
    '        Select Case SpCase
    '            Case 1, 0
    '                'If Net < 0 Then
    '                '    SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + RndTax(0) + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) + IssuPrm, Formating))
    '                'Else
    '                SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + RndTax(Net * 0.01) + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM")), Formating))
    '                    'End If
    '            Case 2
    '                SetDxtxtValue(FindControlRecursive(PageControl, "TAXPRM"), Format(0, Formating))
    '                SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "STMPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM")), "###,#0.000"))
    '            Case 3
    '                SetDxtxtValue(FindControlRecursive(PageControl, "TAXPRM"), Format(0, Formating))
    '                SetDxtxtValue(FindControlRecursive(PageControl, "STMPRM"), Format(0, Formating))
    '                SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM")), Formating))
    '            Case 4
    '                SetDxtxtValue(FindControlRecursive(PageControl, "TAXPRM"), Format(0, Formating))

    '                SetDxtxtValue(FindControlRecursive(PageControl, "STMPRM"), Format(0, Formating))

    '                SetDxtxtValue(FindControlRecursive(PageControl, "CONPRM"), Format(0, Formating))

    '                SetDxtxtValue(FindControlRecursive(PageControl, "TOTPRM"), Format(Net + GetDxTextValue(FindControlRecursive(PageControl, "CONPRM")) + GetDxTextValue(FindControlRecursive(PageControl, "ISSPRM")), Formating))
    '            Case Else
    '                Exit Select
    '        End Select
    '    End Sub

    Public Function GetIssuVal(Sys As String, EndNo As Integer) As Double
        Dim IssueVal As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            If EndNo = 0 Then
                Dim dbadapter = New SqlDataAdapter(String.Format("Select PolIss from SubSystems where SubSysNo='{0}' and Branch=dbo.MainCenter() ", Sys), con)
#Disable Warning IDE0058 ' Expression value is never used
                dbadapter.Fill(IssueVal)
#Enable Warning IDE0058 ' Expression value is never used
                'IssueVal = RecSet("select PolIss from SubSystems where SubSysNo='" & Sys & "'", Conn)
            Else
                Dim dbadapter = New SqlDataAdapter(String.Format("select EndIss from SubSystems where SubSysNo='{0}' and Branch=dbo.MainCenter()", Sys), con)
#Disable Warning IDE0058 ' Expression value is never used
                dbadapter.Fill(IssueVal)
#Enable Warning IDE0058 ' Expression value is never used
                'IssueVal = RecSet("select EndIss from SubSystems where SubSysNo='" & Sys & "'", Conn)
            End If
            Return IssueVal.Tables(0).Rows(0)(0)
            con.Close()
        End Using
    End Function

    Public Function GetStmVal(Sys As String, EndNo As Integer) As Double
        Dim IssueVal As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            If EndNo = 0 Then
                Dim dbadapter = New SqlDataAdapter(String.Format("select PolStm from SubSystems where SubSysNo='{0}' and Branch=dbo.MainCenter()", Sys), con)
#Disable Warning IDE0058 ' Expression value is never used
                dbadapter.Fill(IssueVal)
#Enable Warning IDE0058 ' Expression value is never used
                'IssueVal = RecSet("select PolStm from SubSystems where SubSysNo='" & Sys & "'", Conn)
            Else
                Dim dbadapter = New SqlDataAdapter(String.Format("select EndStm from SubSystems where SubSysNo='{0}' and Branch=dbo.MainCenter()", Sys), con)
#Disable Warning IDE0058 ' Expression value is never used
                dbadapter.Fill(IssueVal)
#Enable Warning IDE0058 ' Expression value is never used
                'IssueVal = RecSet("select EndStm from SubSystems where SubSysNo='" & Sys & "'", Conn)
            End If
            Return IssueVal.Tables(0).Rows(0)(0)
            con.Close()
        End Using
    End Function

    Public Function GetGroupFile(Sys As String) As String
        Dim IssueVal As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select Groups from SubSystems where SubSysNo='{0}' and Branch=dbo.MainCenter()", Sys), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(IssueVal)
#Enable Warning IDE0058 ' Expression value is never used
            'IssueVal = RecSet("select Groups from SubSystems where SubSysNo='" & Sys & "'", Conn)
            Return IssueVal.Tables(0).Rows(0)(0)
            con.Close()
        End Using
    End Function

    Public Function GetOrderNo(Pol As String, Endn As Short, Loadn As Short) As String
        Dim Order As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select OrderNo FROM PolicyFile where PolNo='" & Pol & "' and EndNo=" & Endn & " And LoadNo=" & Loadn & "", con)
            dbadapter.Fill(Order)
            'Check = RecSet("select CustName,SpecialCase From CustomerFile where CustNo=" & Val(No), Conn)
            Return If(Order.Tables(0).Rows.Count = 0, "", Order.Tables(0).Rows.Item(0).Item(0).ToString)
            con.Close()
        End Using
    End Function

    Public Function GetSumIns(Sys As String, OrderNo As String, EndNo As Integer, loadNo As Integer, PolNo As String, GroupNo As Integer) As Double
        Dim TTTT As New DataSet
        GetSumIns = 0
        If GroupNo = 0 Then
            Dim dbadapter = New SqlDataAdapter("select " & GetEndFile(Sys) & " as SumIns from " & GetGroupFile(Sys) & " where (" & GetGroupFile(Sys) & ".OrderNo='" & OrderNo & "'" _
                     & " OR " & GetGroupFile(Sys) & ".OrderNo='" & PolNo & "' ) and " & GetGroupFile(Sys) & ".EndNo<=" & EndNo & " and " & GetGroupFile(Sys) & ".LoadNo=" & loadNo & " and " & GetGroupFile(Sys) & ".SubIns='" & Sys & "'", Conn)
            dbadapter.Fill(TTTT)
        Else
            Dim dbadapter = New SqlDataAdapter("select " & GetEndFile(Sys) & " as SumIns from " & GetGroupFile(Sys) & " where (" & GetGroupFile(Sys) & ".OrderNo='" & OrderNo & "'" _
                     & " OR " & GetGroupFile(Sys) & ".OrderNo='" & PolNo & "' ) and " & GetGroupFile(Sys) & ".EndNo<=" & EndNo & " and " & GetGroupFile(Sys) & ".LoadNo=" & loadNo & " and " & GetGroupFile(Sys) & ".GroupNo=" & GroupNo & " and " & GetGroupFile(Sys) & ".SubIns='" & Sys & "'", Conn)
            dbadapter.Fill(TTTT)
        End If

        If TTTT.Tables(0).Rows().Count > 1 Then
            For i As Integer = 1 To TTTT.Tables(0).Rows().Count
                GetSumIns += TTTT.Tables(0).Rows(i - 1)(0)
            Next
        Else
            Return TTTT.Tables(0).Rows(0)(0)
        End If
        Return GetSumIns
    End Function

    Public Function GetEndFile(Sys As String) As String
        Dim IssueVal As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select SumInsured from SubSystems where SubSysNo='{0}' and Branch=dbo.MainCenter()", Sys), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(IssueVal)
#Enable Warning IDE0058 ' Expression value is never used

            Return If(Not IssueVal.Tables(0).Rows.Item(0).IsNull(0), DirectCast(IssueVal.Tables(0).Rows(0)(0), String), "0 ")

            con.Close()
        End Using
    End Function

    Public Function GetLastEnd(PolNo As String, Optional LoadNo As Integer = 0) As Integer
        Dim LastEnd As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            ExecConn("Delete PolicyFile Where PolNo='" & PolNo & "' and EndNo<>0 and IssuDate Is Null", con)
            Dim dbadapter = New SqlDataAdapter(String.Format("select max(EndNo) from PolicyFile where PolNo='{0}' and LoadNo={1} and IssuDate is not null", PolNo, LoadNo), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(LastEnd)
#Enable Warning IDE0058 ' Expression value is never used
            'LastEnd = RecSet("select max(EndNo) from PolicyFile where PolNo='" & PolNo & "' and LoadNo=" & LoadNo & "", Conn)
            Return If(Not LastEnd.Tables(0).Rows.Item(0).IsNull(0), DirectCast(LastEnd.Tables(0).Rows.Item(0)(0), Integer), 0)
            con.Close()
        End Using
    End Function

    Public Function GetLastEndForAll(PolNo As String, Optional LoadNo As Integer = 0) As Integer
        Dim LastEnd As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select max(EndNo) from PolicyFile where PolNo='{0}' and LoadNo={1}", PolNo, LoadNo), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(LastEnd)
#Enable Warning IDE0058 ' Expression value is never used
            'LastEnd = RecSet("select max(EndNo) from PolicyFile where PolNo='" & PolNo & "' and LoadNo=" & LoadNo & "", Conn)
            Return If(Not LastEnd.Tables(0).Rows.Item(0).IsNull(0), DirectCast(LastEnd.Tables(0).Rows.Item(0)(0), Integer), 0)
            con.Close()
        End Using
    End Function

    Public Function GetLastSattleMent(ClmNo As String) As Integer
        Dim LastSattle As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select max(No) from ClmPhs where ClmNo='{0}'", ClmNo), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(LastSattle)
#Enable Warning IDE0058 ' Expression value is never used
            'LastEnd = RecSet("select max(EndNo) from PolicyFile where PolNo='" & PolNo & "' and LoadNo=" & LoadNo & "", Conn)
            Return If(Not LastSattle.Tables(0).Rows.Item(0).IsNull(0), DirectCast(LastSattle.Tables(0).Rows.Item(0)(0), Integer), 0)
            con.Close()
        End Using
    End Function

    Public Function GetLastSadad(ClmNo As String) As Integer
        Dim LastSattle As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select max(SadadNo) from ClmPhs where ClmNo='{0}'", ClmNo), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(LastSattle)
#Enable Warning IDE0058 ' Expression value is never used
            'LastEnd = RecSet("select max(EndNo) from PolicyFile where PolNo='" & PolNo & "' and LoadNo=" & LoadNo & "", Conn)
            Return If(Not LastSattle.Tables(0).Rows.Item(0).IsNull(0), DirectCast(LastSattle.Tables(0).Rows.Item(0)(0), Integer), 0)
            con.Close()
        End Using
    End Function

    Public Function GetLastOrEnd(PolNo As String, OrangeCard As String, Optional LoadNo As Integer = 0) As Integer
        Dim LastOrEnd As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            If Right(PolNo, 2) = "01" Then

                Dim dbadapter = New SqlDataAdapter(String.Format("select max(EndNo) from MotorFile where OrangeCard='{0}' and LoadNo={1}", PolNo, LoadNo), con)
#Disable Warning IDE0058 ' Expression value is never used
                dbadapter.Fill(LastOrEnd)
#Enable Warning IDE0058 ' Expression value is never used
                'LastOrEnd = RecSet("select max(EndNo) from MotorFile where OrangeCard='" & PolNo & "' and LoadNo=" & LoadNo, Conn)
            Else
                Dim dbadapter = New SqlDataAdapter(String.Format("select max(EndNo) from MotorFile where OrangeCard='{0}' and LoadNo={1}", OrangeCard, LoadNo), con)
#Disable Warning IDE0058 ' Expression value is never used
                dbadapter.Fill(LastOrEnd)
#Enable Warning IDE0058 ' Expression value is never used
                'LastOrEnd = RecSet("select max(EndNo) from MotorFile where OrangeCard='" & OrangeCard & "' and LoadNo=" & LoadNo, Conn)
            End If

            Return If(Not LastOrEnd.Tables(0).Rows.Item(0).IsNull(0), DirectCast(LastOrEnd.Tables(0).Rows.Item(0)(0), Integer), 0)
            con.Close()
        End Using
        'Conn.Close()
    End Function

    Public Function GetLastEndIssued(PolNo As String, Optional LoadNo As Integer = 0) As Integer
        Dim LastEnd As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select max(EndNo) from PolicyFile where PolNo='" & PolNo & "' and LoadNo=" & LoadNo & " and stop=0", con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(LastEnd)
#Enable Warning IDE0058 ' Expression value is never used
            'LastEnd = RecSet("select max(EndNo) from PolicyFile where PolNo='" & PolNo & "' and LoadNo=" & LoadNo & "", Conn)
            Return If(Not LastEnd.Tables(0).Rows.Item(0).IsNull(0), DirectCast(LastEnd.Tables(0).Rows.Item(0)(0), Integer), 0)
            con.Close()
        End Using
    End Function

    Public Function GetLastOrderEnd(OrderNo As String, sys As String) As Integer
        Dim LastEnd As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select max(EndNo) from PolicyFile where OrderNo='{0}' and SubIns='{1}'", Trim(OrderNo), sys), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(LastEnd)
#Enable Warning IDE0058 ' Expression value is never used
            'LastEnd = RecSet("select max(EndNo) from PolicyFile where OrderNo='" & Trim(OrderNo) & "' and SubIns='" & sys & "'", Conn)
            Return If(Not LastEnd.Tables(0).Rows.Item(0).IsNull(0), DirectCast(LastEnd.Tables(0).Rows.Item(0)(0), Integer), 0)
            con.Close()
        End Using
        'Conn.Close()
    End Function

    Public Function GethiddenField(ByRef HF As HiddenField) As Integer
        Try
            Return HF.Value
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Function GetLastLoad(PolNo As String) As Integer
        Dim LastEnd As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select max(LoadNo) from PolicyFile where PolNo='{0}'", PolNo), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(LastEnd)
#Enable Warning IDE0058 ' Expression value is never used
            'LastEnd = RecSet("select max(LoadNo) from PolicyFile where PolNo='" & PolNo & "'", Conn)
            Return If(Not LastEnd.Tables(0).Rows.Item(0).IsNull(0), DirectCast(LastEnd.Tables(0).Rows.Item(0)(0), Integer), 0)
            con.Close()
        End Using
        'Conn.Close()
    End Function

    '    Public Sub SetUserPermNAV(ByRef Tree As ASPxNavBar, CurUser() As String, T As Integer)
    '        'Dim Nd As New NavBarGroup
    '        'Dim Ndd As New NavBarGroup

    '        Dim UserRec As String = CurUser(T)
    '        Dim i As Integer = 1
    '        Dim j As Integer
    '        Dim k As Integer
    '        Dim PermChar As String

    '        'CurUser(7)
    '        Tree.Groups.Clear()
    '        If Tree.Groups.Count > 0 Then Exit Sub 'RecSet("select AccountPermSys From AccountFile Where AccountLogIn='" & CurUser & "'", Conn)
    '        If Len(Trim(UserRec)) = 0 Then
    '#Disable Warning IDE0058 ' Expression value is never used
    '            Tree.Groups.Add("NO SYSTEMS AVAILABLE")
    '#Enable Warning IDE0058 ' Expression value is never used
    '        End If
    '        'Using oCon As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
    '        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
    '            If con.State = ConnectionState.Open Then
    '                con.Close()
    '            Else

    '            End If
    '            con.Open()

    '            While i < Len(UserRec)
    '                j = InStr(i, CurUser(T), ";")
    '                PermChar = Mid(CurUser(T), i, j - i + 1)

    '                Dim Firstdash As Integer = PermChar.IndexOf("-"c)
    '                Dim Seconddash As Integer = PermChar.IndexOf("-"c, PermChar.IndexOf("-"c) + 1)

    '                Dim MainSystem As New DataSet
    '                Dim SubSystem As New DataSet

    '                Dim cx As String = String.Format("select ForUrl,SubSysName from SubSystems where SubsysNo='{0}' AND Branch='{1}'", Mid(PermChar, Firstdash + 2, Seconddash - Firstdash - 1), CurUser(7))

    '                Dim dbadapter = New SqlDataAdapter("select SysName from Systems where SysNo=" & CInt(Mid(PermChar, 1, Firstdash)), con)
    '#Disable Warning IDE0058 ' Expression value is never used
    '                dbadapter.Fill(MainSystem)
    '#Enable Warning IDE0058 ' Expression value is never used

    '                Dim dbadapter1 = New SqlDataAdapter(String.Format("select ForUrl,SubSysName,SubSysNo,Branch from SubSystems where SubsysNo='{0}' AND Branch='{1}'", Mid(PermChar, Firstdash + 2, Seconddash - Firstdash - 1), CurUser(7)), con)
    '#Disable Warning IDE0058 ' Expression value is never used
    '                dbadapter1.Fill(SubSystem)
    '#Enable Warning IDE0058 ' Expression value is never used

    '                Dim Nd = Nothing
    '                For k = 0 To Tree.Groups.Count - 1
    '                    Dim x As String = Tree.Groups(k).Text
    '                    If Trim(Tree.Groups(k).Text) = Trim(MainSystem.Tables(0).Rows(0)(0)) Then
    '                        Nd = Tree.Groups(k)
    '                        Exit For
    '                    End If
    '                Next

    '                Dim Ndd As New NavBarGroup
    '                If IsNothing(Nd) Then
    '                    Ndd.Text = Trim(MainSystem.Tables(0).Rows(0)(0))
    '                    If T = 1 Or T = 2 Then
    '#Disable Warning IDE0058 ' Expression value is never used
    '                        Ndd.Items.Add(Trim(SubSystem.Tables(0).Rows(0)(1)), Nothing, Nothing, "javascript:SetSystem('" & IIf(T = 1, RTrim(SubSystem.Tables(0).Rows(0)(2)), RTrim(SubSystem.Tables(0).Rows(0)(0))) & "','" & Trim(SubSystem.Tables(0).Rows(0)(3)) & "')")
    '#Enable Warning IDE0058 ' Expression value is never used
    '                    Else
    '#Disable Warning IDE0058 ' Expression value is never used
    '                        Ndd.Items.Add(Trim(SubSystem.Tables(0).Rows(0)(1)), Nothing, Nothing, RTrim(SubSystem.Tables(0).Rows(0)(0)))
    '#Enable Warning IDE0058 ' Expression value is never used
    '                    End If

    '                    'Ndd.NavigateUrl = RTrim(SubSystem.Tables(0).Rows(0)(0))
    '                    Tree.Groups.Add(Ndd)
    '                Else
    '                    If T = 1 Or T = 2 Then
    '#Disable Warning IDE0058 ' Expression value is never used
    '                        Nd.Items.Add(Trim(SubSystem.Tables(0).Rows(0)(1)), Nothing, Nothing, "javascript:SetSystem('" & IIf(T = 1, RTrim(SubSystem.Tables(0).Rows(0)(2)), RTrim(SubSystem.Tables(0).Rows(0)(0))) & "','" & Trim(SubSystem.Tables(0).Rows(0)(3)) & "')")
    '#Enable Warning IDE0058 ' Expression value is never used
    '                    Else
    '#Disable Warning IDE0058 ' Expression value is never used
    '                        Nd.Items.Add(Trim(SubSystem.Tables(0).Rows(0)(1)), Nothing, Nothing, RTrim(SubSystem.Tables(0).Rows(0)(0)))
    '#Enable Warning IDE0058 ' Expression value is never used
    '                    End If

    '                    'Nd.NavigateUrl = RTrim(SubSystem.Tables(0).Rows(0)(0))
    '                End If
    '                'Conn.Close()
    '                i = j + 1
    '                'oCon.Close()
    '            End While
    '            con.Close()
    '        End Using
    '        'oCon.Close()
    '        'End Using
    '        'Conn.Close()
    '        'Conn1.Close()
    '    End Sub
    Public Sub SetUserPermNAV(ByRef Tree As ASPxNavBar, CurUser() As String, T As Integer)
        Dim userPermString As String = CurUser(T)
        Dim branch As String = CurUser(7)

        If String.IsNullOrWhiteSpace(userPermString) Then
            Tree.Groups.Add("NO SYSTEMS AVAILABLE")
            Return
        End If

        Tree.Groups.Clear()

        ' Parse permission tokens
        Dim permissions = userPermString.TrimEnd(";"c).Split(";"c)

        ' Collect subsystem IDs
        Dim subSysIds = permissions.
                    Select(Function(p) p.Split("-"c)).
                    Where(Function(parts) parts.Length >= 2).
                    Select(Function(parts) parts(1)).
                    ToList()

        If subSysIds.Count = 0 Then
            Tree.Groups.Add("NO SYSTEMS AVAILABLE")
            Return
        End If

        ' Fetch subsystems data with a parameterized IN clause
        Dim subsystemsData As DataTable = Nothing
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString)
            con.Open()

            ' Build parameter placeholders (@p0, @p1, ...)
            Dim paramNames = subSysIds.Select(Function(s, idx) "@p" & idx).ToList()
            Dim cmdText = "SELECT s.SysName, sub.SubSysName, sub.ForUrl, sub.SubSysNo, sub.Branch " &
                      "FROM SubSystems sub INNER JOIN Systems s ON sub.MAINSYS = s.SysNo " &
                      "WHERE sub.SubSysNo IN (" & String.Join(",", paramNames) & ") AND sub.Branch = @Branch"

            Using cmd As New SqlCommand(cmdText, con)
                For i = 0 To subSysIds.Count - 1
                    cmd.Parameters.AddWithValue("@p" & i, subSysIds(i))
                Next
                cmd.Parameters.AddWithValue("@Branch", branch)

                Using da As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    subsystemsData = dt
                End Using
            End Using
        End Using

        If subsystemsData Is Nothing OrElse subsystemsData.Rows.Count = 0 Then
            Tree.Groups.Add("NO SYSTEMS AVAILABLE")
            Return
        End If

        ' Build navigation groups
        Dim groupDict As New Dictionary(Of String, NavBarGroup)(StringComparer.OrdinalIgnoreCase)

        For Each perm In permissions
            Dim parts = perm.Split("-"c)
            If parts.Length < 2 Then Continue For

            Dim subSysNo = parts(1)

            ' Find matching row using LINQ (safe from quote issues)
            Dim matchingRows = subsystemsData.AsEnumerable().
                            Where(Function(r) r.Field(Of String)("SubSysNo") = subSysNo AndAlso
                                           r.Field(Of String)("Branch") = branch).
                            ToArray()
            If matchingRows.Length = 0 Then Continue For
            Dim row = matchingRows(0)

            Dim sysName = row.Field(Of String)("SysName").Trim()
            Dim subSysName = row.Field(Of String)("SubSysName").Trim()
            Dim forUrl = row.Field(Of String)("ForUrl").Trim()
            Dim subSysBranch = row.Field(Of String)("Branch").Trim()

            ' Build navigation URL
            Dim navigateUrl As String
            If T = 1 Or T = 2 Then
                Dim param1 = If(T = 1, subSysNo, forUrl)
                ' Escape for JavaScript
                Dim safeParam1 = HttpUtility.JavaScriptStringEncode(param1, addDoubleQuotes:=False)
                Dim safeBranch = HttpUtility.JavaScriptStringEncode(subSysBranch, addDoubleQuotes:=False)
                navigateUrl = $"javascript:SetSystem('{safeParam1}', '{safeBranch}');"
            Else
                navigateUrl = forUrl
            End If

            ' Find or create parent group
            If Not groupDict.ContainsKey(sysName) Then
                Dim newGroup As New NavBarGroup(sysName)
                Tree.Groups.Add(newGroup)
                groupDict(sysName) = newGroup
            End If
            Dim parentGroup = groupDict(sysName)

            ' Add item
            parentGroup.Items.Add(subSysName, Nothing, Nothing, navigateUrl)
        Next
    End Sub
    Public Function IsPolicy(OrderNo As String, EndNo As String, LoadNo As Integer, sys As String) As Boolean
        Dim LastEnd As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select max(EndNo) from PolicyFile where OrderNo='{0}' " _
                                                & " And endNo={1} And loadNo={2} And SubIns='{3}'", OrderNo, EndNo, LoadNo, sys), con)

            dbadapter.Fill(LastEnd)

            Return Not LastEnd.Tables(0).Rows.Item(0).IsNull(0)
            con.Close()
        End Using
    End Function

    Public Function GetlastSettl(ClmNo As String) As Integer
        Dim LastSettl As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select max(No) from MainSattelement where ClmNo='" & ClmNo & "'", con)

            dbadapter.Fill(LastSettl)

            Return If(Not LastSettl.Tables(0).Rows.Item(0).IsNull(0), LastSettl.Tables(0).Rows.Item(0)(0), 0)
            con.Close()
        End Using
    End Function

    Public Function GetLastNet(Policy As String, EndNo As Integer, Optional LoadNo As Integer = 0) As Double
        Dim LastNet As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select Sum(NetPrm) from PolicyFile where PolNo='{0}' and loadNo={1} and EndNo <={2} and stop=0", Policy, LoadNo, EndNo), con)

            dbadapter.Fill(LastNet)

            'LastNet = RecSet("select LASTNET from PolicyFile where PolNo='" & Policy & "' and loadNo=" & LoadNo & " and EndNo=" & EndNo - 1 & " order by EndNo desc", Conn)
            Return If(LastNet.Tables(0).Rows(0).IsNull(0), 0, LastNet.Tables(0).Rows.Item(0)(0))
            con.Close()
        End Using
    End Function

    Public Function GetNumeric(value As String) As String
        Dim output As New StringBuilder
        For i = 0 To value.Length - 1
            If IsNumeric(value(i)) Or value(i) = "." Then
                output.Append(value(i))
            End If
        Next
        Return output.ToString()
    End Function

    Public Function GetLastSi(Policy As String, EndNo As Integer, Sys As String) As Double
        Dim LastSi As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("(select isnull(Sum(" & GetEndFile(Sys) & "),0) from PolicyFile inner join " & GetGroupFile(Sys) & " on PolicyFile.orderno=" & GetGroupFile(Sys) & ".orderno And PolicyFile.EndNo=" & GetGroupFile(Sys) & ".endno  where PolNo='" & Policy & "' and policyfile.EndNo <" & EndNo & ")", con)

            dbadapter.Fill(LastSi)
            'LastNet = RecSet("Select LASTNET from PolicyFile where PolNo='" & Policy & "' and loadNo=" & LoadNo & " and EndNo=" & EndNo - 1 & " order by EndNo desc", Conn)
            If LastSi.Tables(0).Rows(0).IsNull(0) Then
                Return 0
            Else
                Return LastSi.Tables(0).Rows.Item(0)(0)
            End If
            con.Close()
        End Using
    End Function

    Public Function GetLastDocValue(Policy As String, EndNo As Integer, Sys As String) As Double
        Dim LastDocVal As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select Sum(DocVal) from PolicyFile inner join " & GetGroupFile(Sys) & " on PolicyFile.orderno=" & GetGroupFile(Sys) & ".orderno And PolicyFile.EndNo=" & GetGroupFile(Sys) & ".endno  where PolNo='" & Policy & "' and policyfile.EndNo <" & EndNo & "", con)

            dbadapter.Fill(LastDocVal)
            'LastNet = RecSet("Select LASTNET from PolicyFile where PolNo='" & Policy & "' and loadNo=" & LoadNo & " and EndNo=" & EndNo - 1 & " order by EndNo desc", Conn)
            If LastDocVal.Tables(0).Rows(0).IsNull(0) Then
                Return 0
            Else
                Return LastDocVal.Tables(0).Rows.Item(0)(0)
            End If
            con.Close()
        End Using
    End Function

    Public Function IsExpired(OrderNo As String, EndNo As String) As Boolean
        Dim Expiration As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select * from PolicyFile where OrderNo='" & OrderNo & "' and endNo=" & EndNo & "", con)

#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(Expiration)
#Enable Warning IDE0058 ' Expression value is never used

            Return Expiration.Tables(0).Rows.Item(0)("CoverTo") > Today.Date()
            con.Close()
        End Using
    End Function

    Public Function IsFinanced(OrderNo As String, EndNo As String, LoadNo As Integer, sys As String) As Boolean
        Dim Financed As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select Financed from PolicyFile where " _
                           & "OrderNo='" & OrderNo & "' and endNo=" & EndNo & " and loadNo=" & LoadNo & " and SubIns='" & sys & "' " _
                           & "and issudate Is Not null", con)

#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(Financed)
#Enable Warning IDE0058 ' Expression value is never used
            'item 5 in data source is IssueDate
            'item 21 in data source is CoverStart
            'item 6 in data source is IssueTime HH:mm:ss
            Return Financed.Tables(0).Rows(0)(0)
            con.Close()
        End Using
    End Function

    Public Function IsIssued(OrderNo As String, EndNo As String, LoadNo As Integer, sys As String) As Boolean
        Dim Issued As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select * from PolicyFile where " _
                           & "OrderNo='" & OrderNo & "' and endNo=" & EndNo & " and loadNo=" & LoadNo & " and SubIns='" & sys & "' " _
                           & "and issudate Is Not null", con)

            dbadapter.Fill(Issued)
            'item 5 in data source is IssueDate
            'item 21 in data source is CoverStart
            'item 6 in data source is IssueTime HH:mm:ss
            If Issued.Tables(0).Rows.Count <> 0 Then
                Return True
            Else
                Return False
            End If
            con.Close()
        End Using
    End Function

    Public Function IsSameQuarter(OrderNo As String, EndNo As String, LoadNo As Integer, sys As String) As Boolean
        Dim SameQ As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select * from PolicyFile where " _
                           & "OrderNo='" & OrderNo & "' and endNo=" & EndNo & " and loadNo=" & LoadNo & " and SubIns='" & sys & "' and datepart(qq,issudate)=" & DatePart("q", Today.Date) & " and  year(issudate)=" & Year(Today.Date) & "", con)

#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(SameQ)
#Enable Warning IDE0058 ' Expression value is never used
            'item 5 in data source is IssueDate
            'item 21 in data source is CoverStart
            'item 6 in data source is IssueTime HH:mm:ss
            Return SameQ.Tables(0).Rows.Count <> 0
            con.Close()
        End Using
    End Function

    Public Function IsSameMonth(OrderNo As String, EndNo As String, LoadNo As Integer, sys As String) As Boolean
        Dim SameQ As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select * from PolicyFile where " _
                           & "OrderNo='" & OrderNo & "' and endNo=" & EndNo & " and loadNo=" & LoadNo & " and SubIns='" & sys & "' and datepart(mm,issudate)=" & DatePart("m", Today.Date) & " and  year(issudate)=" & Year(Today.Date) & "", con)

#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(SameQ)
#Enable Warning IDE0058 ' Expression value is never used
            'item 5 in data source is IssueDate
            'item 21 in data source is CoverStart
            'item 6 in data source is IssueTime HH:mm:ss
            If SameQ.Tables(0).Rows.Count <> 0 Then
                Return True
            Else
                Return False
            End If
            con.Close()
        End Using
    End Function

    Public Function GetTyp(S As String) As String
        Dim LastPoint As Integer
        Dim FirstPoint As Integer = InStr("'", S)
        LastPoint = InStr(FirstPoint + 1, "'", S)
        Return Mid(S, FirstPoint + 1, LastPoint - FirstPoint - 1)
    End Function

    Public Function GenArray(O As Object) As Array
        Dim Arr As Array = Array.CreateInstance(O.GetType, 1)
        Arr(0) = O
        Return Arr
    End Function

    Public Function PolRep(Sys As String) As String
        Dim Rep As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("SELECT PolReport FROM SUBSYSTEMS Where SubSysNo='{0}' And Branch=[dbo].[MainCenter]()", Sys), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(Rep)
#Enable Warning IDE0058 ' Expression value is never used
            'GetFromTo = RecSet("select CoverFrom,CoverTo From PolicyFile Where EndNo=0 and LoadNo=0 and PolNo='" & PolNo & "'", Conn)
            Return Rep.Tables(0).Rows(0)(0)
            con.Close()
        End Using
    End Function

    Public Function GetPolDays(PolNo As String) As Integer
        Dim GetFromTo As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select CoverFrom,CoverTo From PolicyFile Where EndNo=0 and LoadNo=0 and PolNo='{0}'", PolNo), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(GetFromTo)
#Enable Warning IDE0058 ' Expression value is never used
            'GetFromTo = RecSet("select CoverFrom,CoverTo From PolicyFile Where EndNo=0 and LoadNo=0 and PolNo='" & PolNo & "'", Conn)
            Return DateDiff(DateInterval.Day, GetFromTo.Tables(0).Rows(0)(0), GetFromTo.Tables(0).Rows(0)(1)) + 1
            con.Close()
        End Using
    End Function

    Public Function GetSysBranch(SysNo As String) As String
        Dim GetName As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select Branch from SubSystems where SubSysNo='{0}'", SysNo), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(GetName)
#Enable Warning IDE0058 ' Expression value is never used
            'GetName = RecSet("select Branch from SubSystems where SubSysNo='" & SysNo & "'", Conn)
            Return If(GetName.Tables(0).Rows.Count <> 0, Trim(GetName.Tables(0).Rows(0)(0)), GetMainCenter())
            con.Close()
        End Using
    End Function

    Public Function GetSysName(SysNo As String) As String
        Dim GetName As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select SUBSYSNAME from SubSystems where SubSysNo='{0}' and  Branch=[dbo].[MainCenter]() ", SysNo), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(GetName)
#Enable Warning IDE0058 ' Expression value is never used
            'GetName = RecSet("select Branch from SubSystems where SubSysNo='" & SysNo & "'", Conn)
            Return If(GetName.Tables(0).Rows.Count <> 0, Trim(GetName.Tables(0).Rows(0)(0)), " ")
            con.Close()
        End Using
    End Function

    Public Function GetBranchName(Br As String) As String
        Dim GetName As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select BranchName from BranchInfo where BranchNo='{0}'", Br), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(GetName)
#Enable Warning IDE0058 ' Expression value is never used
            'GetName = RecSet("select BranchName from BranchInfo where BranchNo='" & SysNo & "'", Conn)
            Return If(GetName.Tables(0).Rows.Count <> 0, Trim(GetName.Tables(0).Rows(0)(0)), "الفرع غير مضاف")
            con.Close()
        End Using
    End Function

    Public Function GetUnderWritinYear(PolN As String) As String
        Dim GetYear As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select UnderWY from NetPrm where PolNo='{0}'", PolN), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(GetYear)
#Enable Warning IDE0058 ' Expression value is never used
            'GetName = RecSet("select BranchName from BranchInfo where BranchNo='" & SysNo & "'", Conn)
            Return If(GetYear.Tables(0).Rows.Count <> 0, Trim(GetYear.Tables(0).Rows(0)(0)), "No UW Y registered")
            con.Close()
        End Using
    End Function

    Public Function GetGroupNo(ClmNo As String, BranchNo As String) As Int32
        Using oCon As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            Dim gn As New DataSet
            Dim dbadapter = New SqlDataAdapter("select GroupNo from MainClaimFile where ClmNo='" & ClmNo & "' and branch='" & BranchNo & "'", oCon)
            dbadapter.Fill(gn)
            If gn.Tables(0).Rows.Count <> 0 Then
                GetGroupNo = gn.Tables(0).Rows(0)(0)
            Else
                GetGroupNo = 0
            End If
            oCon.Close()
        End Using
    End Function

    Public Function GetPolBranch(PolNo As String, EndNo As Integer, loadNo As Integer) As String
        Using oCon As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            Dim gn As New DataSet
            Dim dbadapter = New SqlDataAdapter("select Branch from PolicyFile where PolNo='" & PolNo & "' and EndNo=" & EndNo & " and LoadNo=" & loadNo & " ", oCon)
            dbadapter.Fill(gn)
            If gn.Tables(0).Rows.Count <> 0 Then
                Return gn.Tables(0).Rows(0)(0).ToString.TrimEnd
            Else
                Return "0"
            End If
            oCon.Close()
        End Using
    End Function

    Public Function GetSumInsG(Sys As String, OrderNo As String, EndNo As Integer, loadNo As Integer, PolNo As String, GroupNo As Integer) As Double
        Dim TTTT As New DataSet
        GetSumInsG = 0
        If GroupNo = 0 Then
            Dim dbadapter = New SqlDataAdapter("select " & GetEndFile(Sys) & " as SumIns from " & GetGroupFile(Sys) & " where (" & GetGroupFile(Sys) & ".OrderNo='" & OrderNo & "'" _
                         & " OR " & GetGroupFile(Sys) & ".OrderNo='" & PolNo & "' ) and " & GetGroupFile(Sys) & ".EndNo<=" & EndNo & " and " & GetGroupFile(Sys) & ".LoadNo=" & loadNo & " and " & GetGroupFile(Sys) & ".SubIns='" & Sys & "'", Conn)
            dbadapter.Fill(TTTT)
        Else
            Dim dbadapter = New SqlDataAdapter("select " & GetEndFile(Sys) & " as SumIns from " & GetGroupFile(Sys) & " where (" & GetGroupFile(Sys) & ".OrderNo='" & OrderNo & "'" _
                         & " OR " & GetGroupFile(Sys) & ".OrderNo='" & PolNo & "' ) and " & GetGroupFile(Sys) & ".EndNo<=" & EndNo & " and " & GetGroupFile(Sys) & ".LoadNo=" & loadNo & " and " & GetGroupFile(Sys) & ".GroupNo=" & GroupNo & " and " & GetGroupFile(Sys) & ".SubIns='" & Sys & "'", Conn)
            dbadapter.Fill(TTTT)
        End If

        If TTTT.Tables(0).Rows().Count > 1 Then
            For i As Integer = 1 To TTTT.Tables(0).Rows().Count
                GetSumInsG += TTTT.Tables(0).Rows(i - 1)(0)
            Next
        Else
            GetSumInsG = TTTT.Tables(0).Rows(0)(0)
        End If

    End Function

    Public Function GetBranchbyOrderNo(Order As String) As String
        Dim GetBr As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select Branch from PolicyFile where OrderNo='{0}'", Order), con)
            dbadapter.Fill(GetBr)
            'GetName = RecSet("select BranchName from BranchInfo where BranchNo='" & SysNo & "'", Conn)
            If GetBr.Tables(0).Rows.Count <> 0 Then
                Return Trim(GetBr.Tables(0).Rows(0)(0))
            Else
                Return "الفرع غير مضاف"
            End If
            con.Close()
        End Using
    End Function

    Public Function GetUserByOrderNo(Order As String) As String
        Dim GetUser As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select IssueUser from PolicyFile where OrderNo='{0}'", Order), con)
            dbadapter.Fill(GetUser)
            'GetName = RecSet("select BranchName from BranchInfo where BranchNo='" & SysNo & "'", Conn)
            If GetUser.Tables(0).Rows.Count <> 0 Then
                Return Trim(GetUser.Tables(0).Rows(0)(0))
            Else
                Return "SysUser"
            End If
            con.Close()
        End Using
    End Function

    Public Function GetSubSysName(SysNo As String, Br As String) As String
        Dim GetName As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select SubSysName from SubSystems where SubSysNo='{0}' and Branch=[dbo].[MainCenter]() ", SysNo, Br), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(GetName)
#Enable Warning IDE0058 ' Expression value is never used
            'GetName = RecSet("select SubSysName from SubSystems where SubSysNo='" & SysNo & "' And Branch='" & Br & "'", Conn)
            Return If(GetName.Tables(0).Rows.Count <> 0, Trim(GetName.Tables(0).Rows(0)(0)), "")
            con.Close()
        End Using
    End Function

    Public Function IsStoped(PolNo As String, Endno As Integer, LoadNo As Integer) As Boolean
        If Trim(PolNo) = "" Then Return False : Exit Function
        Dim Policy As New DataSet
        'If Conn.State = ConnectionState.Open Then
        '    Conn.Close()
        'End If
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select * from PolicyFile where PolNo='{0}' and endNo={1} and loadno={2}", PolNo, Endno, LoadNo), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(Policy)
#Enable Warning IDE0058 ' Expression value is never used
            'Policy = RecSet("select * from PolicyFile where PolNo='" & PolNo & "' and endNo=" & Endno & " and loadno=" & LoadNo, Conn)
            Return Policy.Tables(0).Rows(0)("Stop")
            con.Close()
        End Using
    End Function

    Public Function IsClaimed(GroupN As Integer, PolNo As String, EndN As Integer) As Boolean
        If Trim(PolNo) = "" Then Return False : Exit Function
        Dim Claims As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select * from MainClaimFile where (PolNo='{0}' and GroupNo={1} and EndNo={2}) AND status<>4", PolNo, GroupN, EndN), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(Claims)
#Enable Warning IDE0058 ' Expression value is never used
            'Policy = RecSet("select * from PolicyFile where PolNo='" & PolNo & "' and endNo=" & Endno & " and loadno=" & LoadNo, Conn)
            Return Claims.Tables(0).Rows.Count <> 0
            con.Close()
        End Using
    End Function

    Public Function GetIssueDate(Order As String, EndNo As Integer, Sys As String) As Date
        Dim IssDate As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select IssuTime From policyfile where OrderNo='{0}' and EndNo={1} and subins='{2}'", Order, EndNo, Sys), con)
            dbadapter.Fill(IssDate)
            Return IssDate.Tables(0).Rows.Item(0).Item(0)
            con.Close()
        End Using
    End Function

    Public Function Getclosedate(ClmNo As String, Pol As String) As Date
        If Trim(ClmNo) = "" Then Return "" : Exit Function
        Dim Policy As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select * from MainClaimFile where ClmNo='{0}' AND PolNo='{1}'", ClmNo, Pol), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(Policy)
#Enable Warning IDE0058 ' Expression value is never used
            Return IIf(IsDate(Policy.Tables(0).Rows(0)("ClmCloseDate")), Policy.Tables(0).Rows(0)("ClmCloseDate"), "")
            con.Close()
        End Using
    End Function

    Public Function IsClosed(ClmNo As String, Pol As String) As Boolean
        If Trim(ClmNo) = "" Then Return False : Exit Function
        Dim ClmFile As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select * from MainClaimFile where ClmNo='{0}' AND PolNo='{1}' ", ClmNo, Pol), con)

            dbadapter.Fill(ClmFile)

            Return IIf(ClmFile.Tables(0).Rows(0)("Status") <> 1, True, False)
            con.Close()
        End Using
    End Function

    Public Sub InsetJournal(pol As String, endNo As Integer, loadNo As Integer, User As String, Order As String, RecNo As String, Br As String)
        Dim Commision As New DataSet
        Dim DBtTp As New DataSet
        Dim RecieptRecord As New DataSet
        'If Conn.State = ConnectionState.Open Then
        '    Conn.Close()
        'End If
        Using con1 As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con1.State = ConnectionState.Open Then
                con1.Close()
            Else

            End If
            con1.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select Commision,AccountNo,SubIns from PolicyFile where PolNo='{0}' and endNo={1} and loadno={2}", pol, endNo, loadNo), con1)
            dbadapter.Fill(Commision)
            'And Commision.Tables(0).Rows(0)("AccountNo").ToString.Trim <> "0"
            If Commision.Tables(0).Rows(0)("Commision") <> 0 Then
                Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    Else

                    End If
                    con.Open()
                    '.CommandText = "JournalEntrySinglePolicy",
                    Dim JournalCmd As New SqlCommand With {
                        .CommandText = "JournalEntrySingleCommision",
                        .CommandType = CommandType.StoredProcedure
                    }
                    JournalCmd.Parameters.AddWithValue("@Br", Odbc.OdbcType.NVarChar).Value = Trim(Br)
                    JournalCmd.Parameters.AddWithValue("@Sys", Odbc.OdbcType.NVarChar).Value = Commision.Tables(0).Rows(0)("SubIns").ToString.Trim
                    JournalCmd.Parameters.AddWithValue("@PolNo", Odbc.OdbcType.NVarChar).Value = Trim(pol)
                    JournalCmd.Parameters.AddWithValue("@EndNo", Odbc.OdbcType.Int).Value = endNo
                    JournalCmd.Parameters.AddWithValue("@loadNo", Odbc.OdbcType.Int).Value = loadNo
                    JournalCmd.Connection = con

                    Dim myReader As SqlDataReader = JournalCmd.ExecuteReader()
                    If myReader.HasRows Then
                        'Call New DataTable().Load(myReader)
                        Dim dt As New DataTable()
                        dt.Load(myReader)

                        'Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                        'SetPm("@TP", DbType.String, "1", Parm, 0)
                        'SetPm("@Year", DbType.String, Right(Year(dt.Rows(0).Item(8)).ToString, 2), Parm, 1)

                        Parm = Array.CreateInstance(GetType(SqlParameter), 3)
                        SetPm("@TP", DbType.String, "1", Parm, 0)
                        SetPm("@Year", DbType.String, Right(Year(dt.Rows(0).Item(8)).ToString, 2), Parm, 1)
                        SetPm("@Br", DbType.String, Br, Parm, 2)

                        Dim Dly As String = CallSP("LastDailyNo", con, Parm)

                        Dim Str = If(RecNo = "/", dt.Rows(0).Item(11).ToString, DirectCast(dt.Rows(0).Item(11) & " / RecNo.# " & RecNo, String))

                        ExecConn("INSERT INTO MainJournal ([DAILYNUM],[DAILYDTE] ,[DailyTyp] ,[ANALSNUM] ,[Comment] " _
                                    & ",[Currency] ,[Exchange] ,[CurUser] ,[MoveRef], [RecNo],[DailyChk],[Branch],[SubBranch])  " _
                                    & " VALUES ('" & Dly & "','" & Format(Today.Date, "yyyy/MM/dd") & "', " _
                                    & 1 & ", '" & "A" & "','" & Str & "', " & dt.Rows(0).Item(12) & "," & dt.Rows(0).Item(13) & "," _
                                    & "'" & User & "','" & Order & "','" & RecNo & "'," & 1 & ",'" & Br & "','" & Br & "')", con)
                        '& "'" & User & "','" & dt.Rows(0).Item(0).ToString + "/" + dt.Rows(0).Item(1).ToString + "/" + dt.Rows(0).Item(2).ToString & "'," & 1 & ")", con)

                        For Each row As DataRow In dt.Rows
                            ExecConn("INSERT INTO [dbo].[Journal]([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser],[Branch],[SubBranch]) " _
                                    & " VALUES ('" & Dly & "'," & 1 & ", '" & row.Item("AccountNumber") & "', " & CDbl(row.Item("DR")) & "," & CDbl(row.Item("CR")) & ",'" & User & "','" & Br & "','" & Br & "')", con)
                        Next row
                    Else
                        'MsgBox("No rows found.")
                    End If

                    con.Close()
                End Using
            Else
                Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    Else

                    End If
                    con.Open()

                    Dim JournalCmd As New SqlCommand With {
                        .CommandText = "JournalEntry",
                        .CommandType = CommandType.StoredProcedure
                    }

                    JournalCmd.Parameters.AddWithValue("@PolNo", Odbc.OdbcType.NVarChar).Value = Trim(pol)
                    JournalCmd.Parameters.AddWithValue("@EndNo", Odbc.OdbcType.Int).Value = endNo
                    JournalCmd.Parameters.AddWithValue("@loadNo", Odbc.OdbcType.Int).Value = loadNo
                    JournalCmd.Connection = con

                    Dim myReader As SqlDataReader = JournalCmd.ExecuteReader()
                    If myReader.HasRows Then
                        'Call New DataTable().Load(myReader)
                        Dim dt As New DataTable()
                        dt.Load(myReader)

                        'Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                        'SetPm("@TP", DbType.String, "1", Parm, 0)
                        'SetPm("@Year", DbType.String, Right(Year(dt.Rows(0).Item(8)).ToString, 2), Parm, 1)

                        Parm = Array.CreateInstance(GetType(SqlParameter), 3)
                        SetPm("@TP", DbType.String, "1", Parm, 0)
                        SetPm("@Year", DbType.String, Right(Year(dt.Rows(0).Item(8)).ToString, 2), Parm, 1)
                        SetPm("@Br", DbType.String, Br, Parm, 2)

                        Dim Dly As String = CallSP("LastDailyNo", con, Parm)

                        Dim Str = If(RecNo = "/", dt.Rows(0).Item(5).ToString, DirectCast(dt.Rows(0).Item(5) & " / RecNo.# " & RecNo, String))

                        ExecConn("INSERT INTO MainJournal ([DAILYNUM],[DAILYDTE] ,[DailyTyp] ,[ANALSNUM] ,[Comment] " _
                                    & ",[Currency] ,[Exchange] ,[CurUser] ,[MoveRef], [RecNo],[DailyChk],[Branch],[SubBranch])  " _
                                    & " VALUES ('" & Dly & "','" & Format(Today.Date, "yyyy/MM/dd") & "', " _
                                    & 1 & ", '" & "A" & "','" & Str & "', " & dt.Rows(0).Item(13) & "," & dt.Rows(0).Item(14) & "," _
                                    & "'" & User & "','" & Order & "','" & RecNo & "'," & 1 & ",'" & Br & "','" & Br & "')", con)
                        '& "'" & User & "','" & dt.Rows(0).Item(0).ToString + "/" + dt.Rows(0).Item(1).ToString + "/" + dt.Rows(0).Item(2).ToString & "'," & 1 & ")", con)

                        For Each row As DataRow In dt.Rows
                            If CDbl(row.Item("DR")) > 0 And row.Item("AccountNumber") = GetBoxAccount(Br) And RecNo <> "/" Then
                                Dim Recdptr = New SqlDataAdapter(String.Format("select Amount,Payment from AccMove where DocNo='{0}' ", RecNo), con)
                                Recdptr.Fill(RecieptRecord)
                                If RecieptRecord.Tables(0).Rows(0).Item("Amount") <> RecieptRecord.Tables(0).Rows(0).Item("Payment") Then
                                    ExecConn("INSERT INTO [dbo].[Journal]([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser],[Branch],[SubBranch]) " _
                                      & " VALUES ('" & Dly & "'," & 1 & ", '" & row.Item("AccountNumber") & "', " & RecieptRecord.Tables(0).Rows(0).Item("Payment") & "," & CDbl(row.Item("CR")) & ",'" & User & "','" & Br & "','" & Br & "')", con)
                                Else
                                    ExecConn("INSERT INTO [dbo].[Journal]([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser],[Branch],[SubBranch]) " _
                                     & " VALUES ('" & Dly & "'," & 1 & ", '" & row.Item("AccountNumber") & "', " & CDbl(row.Item("DR")) & "," & CDbl(row.Item("CR")) & ",'" & User & "','" & Br & "','" & Br & "')", con)
                                End If
                            Else
                                ExecConn("INSERT INTO [dbo].[Journal]([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser],[Branch],[SubBranch]) " _
                                     & " VALUES ('" & Dly & "'," & 1 & ", '" & row.Item("AccountNumber") & "', " & CDbl(row.Item("DR")) & "," & CDbl(row.Item("CR")) & ",'" & User & "','" & Br & "','" & Br & "')", con)
                            End If
                            'ExecConn("INSERT INTO [dbo].[Journal]([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser],[Branch],[SubBranch]) " _
                            '        & " VALUES ('" & Dly & "'," & 1 & ", '" & row.Item("AccountNumber") & "', " & CDbl(row.Item("DR")) & "," & CDbl(row.Item("CR")) & ",'" & User & "','" & Br & "','" & Br & "')", con)
                        Next row
                        'Dim dbadapter5 = New SqlDataAdapter(String.Format("select PayTp from AccMove where DocNo='{0}' ", RecNo), con1)
                        'dbadapter5.Fill(DBtTp)
                        'If DBtTp.Tables(0).Rows.Count = 0 Then
                        '    Exit Sub
                        'Else
                        '    Select Case DBtTp.Tables(0).Rows(0)("PayTp")
                        '        Case 1
                        '        Case 2
                        '            ExecConn("Update Journal set AccountNo ='" & GetcheqAccount(Br) & "' where DAILYNUM='" & Dly & "' and TP=1 and Dr>0 ", con1)
                        '        Case Else
                        '            Exit Select
                        '    End Select
                        'End If
                    Else
                        'MsgBox("No rows found.")
                    End If

                    con.Close()
                End Using
            End If
            con1.Close()
        End Using

        'Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
        '    If con.State = ConnectionState.Open Then
        '        con.Close()
        '    Else

        '    End If
        '    con.Open()

        '    Dim JournalCmd As New SqlCommand With {
        '        .CommandText = "JournalEntry",
        '        .CommandType = CommandType.StoredProcedure
        '    }

        '    JournalCmd.Parameters.AddWithValue("@PolNo", Odbc.OdbcType.NVarChar).Value = Trim(pol)
        '    JournalCmd.Parameters.AddWithValue("@EndNo", Odbc.OdbcType.Int).Value = endNo
        '    JournalCmd.Parameters.AddWithValue("@loadNo", Odbc.OdbcType.Int).Value = loadNo
        '    JournalCmd.Connection = con

        '    Dim myReader As SqlDataReader = JournalCmd.ExecuteReader()
        '    If myReader.HasRows Then
        '        'Call New DataTable().Load(myReader)
        '        Dim dt As New DataTable()
        '        dt.Load(myReader)

        '        'Parm = Array.CreateInstance(GetType(SqlParameter), 2)
        '        'SetPm("@TP", DbType.String, "1", Parm, 0)
        '        'SetPm("@Year", DbType.String, Right(Year(dt.Rows(0).Item(8)).ToString, 2), Parm, 1)

        '        Parm = Array.CreateInstance(GetType(SqlParameter), 3)
        '        SetPm("@TP", DbType.String, "1", Parm, 0)
        '        SetPm("@Year", DbType.String, Right(Year(dt.Rows(0).Item(8)).ToString, 2), Parm, 1)
        '        SetPm("@Br", DbType.String, Br, Parm, 2)

        '        Dim Dly As String = CallSP("LastDailyNo", con, Parm)

        '        Dim Str = If(RecNo = "/", dt.Rows(0).Item(5).ToString, DirectCast(dt.Rows(0).Item(5) & " / RecNo./ " & RecNo, String))

        '        ExecConn("INSERT INTO MainJournal ([DAILYNUM],[DAILYDTE] ,[DailyTyp] ,[ANALSNUM] ,[Comment] " _
        '                    & ",[Currency] ,[Exchange] ,[CurUser] ,[MoveRef], [RecNo],[DailyChk],[Branch],[SubBranch])  " _
        '                    & " VALUES ('" & Dly & "','" & Format(Today.Date, "yyyy/MM/dd") & "', " _
        '                    & 1 & ", '" & "A" & "','" & Str & "', " & dt.Rows(0).Item(13) & "," & dt.Rows(0).Item(14) & "," _
        '                    & "'" & User & "','" & Order & "','" & RecNo & "'," & 1 & ",'" & Br & "','" & Br & "')", con)
        '        '& "'" & User & "','" & dt.Rows(0).Item(0).ToString + "/" + dt.Rows(0).Item(1).ToString + "/" + dt.Rows(0).Item(2).ToString & "'," & 1 & ")", con)

        '        For Each row As DataRow In dt.Rows
        '            ExecConn("INSERT INTO [dbo].[Journal]([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser],[Branch],[SubBranch]) " _
        '                    & " VALUES ('" & Dly & "'," & 1 & ", '" & row.Item("AccountNumber") & "', " & CDbl(row.Item("DR")) & "," & CDbl(row.Item("CR")) & ",'" & User & "','" & Br & "','" & Br & "')", con)
        '        Next row
        '    Else
        '        'MsgBox("No rows found.")
        '    End If

        '    con.Close()
        'End Using
    End Sub

    Public Function IsExistsAcc(Acc As String) As Boolean
        Dim Check As New DataSet
        Dim dbadapter = New SqlDataAdapter("select AccountNo From Accounts where AccountNo='" & Trim(Acc).ToString.TrimEnd & "' ", AccConn)
        dbadapter.Fill(Check)
        IsExistsAcc = Check.Tables(0).Rows.Count <> 0
        Return IsExistsAcc
    End Function

    Public Function IsAccNo(Acc As String) As Boolean
        Dim Check As New DataSet
        Dim dbadapter = New SqlDataAdapter("select AccountNo From Accounts where AccountNo='" & Trim(Acc) & "' AND AccountNo NOT IN (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level>=4", AccConn)
        dbadapter.Fill(Check)
        IsAccNo = Check.Tables(0).Rows.Count <> 0
        Return IsAccNo
    End Function

    Public Function IsClosed(ClmNo As String) As Boolean
        If Trim(ClmNo) = "" Then Return False : Exit Function
        Dim Policy As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select * from MainClaimFile where ClmNo='{0}' ", ClmNo), con)
            dbadapter.Fill(Policy)
            If Policy.Tables(0).Rows(0)("Status") <> 1 Then
                Return True
            Else
                Return False
            End If
            con.Close()
        End Using
    End Function

    Public Function IsTransAccount(AccNo As String) As Boolean
        If Trim(AccNo) = "" Then Return False : Exit Function
        Dim Trans As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If

            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select iif(AccountNo in (Select AccountNo from Journal where AccountNo='{0}')  ,1,iif('{0}' in (Select ParentAcc from Accounts),0,TranscationAcc)) As TransAcc
                                    from Accounts
                                    where AccountNo='{0}'", AccNo), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(Trans)
#Enable Warning IDE0058 ' Expression value is never used
            'If Trans.Tables(0).Rows.Count > 0 Then
            Return Trans.Tables(0).Rows(0)("TransAcc")
            'Else
            'Return False
            'End If
            con.Close()
        End Using
    End Function

    Public Function IsRootAccount(AccNo As String) As Boolean
        If Trim(AccNo) = "" Then Return False : Exit Function
        Dim RootAcc, Root5 As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            ''" & CInt(IsTransAccount(AccNo)) & "
            'Dim dbadapter = New SqlDataAdapter(String.Format("select iif('{0}' in (Select ParentAcc from Accounts) ,1, 0) As IsRoot from Accounts where AccountNo='{0}'", AccNo), con)
            'dbadapter.Fill(RootAcc)

            Dim dbadapter5 = New SqlDataAdapter(String.Format("select TranscationAcc As IsRoot5 from Accounts where AccountNo='{0}'", AccNo), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter5.Fill(Root5)
#Enable Warning IDE0058 ' Expression value is never used
            'If Trans.Tables(0).Rows.Count > 0 Then
            Return Root5.Tables(0).Rows(0)("IsRoot5")
            'Else
            'Return False
            'End If
            con.Close()
        End Using
    End Function

    Public Function Istranscationed(AccNo As String) As Boolean
        If Trim(AccNo) = "" Then Return False : Exit Function
        Dim Trans As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            '" & CInt(IsTransAccount(AccNo)) & "
            Dim dbadapter = New SqlDataAdapter(String.Format("select * from Journal where AccountNo='{0}'", AccNo), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(Trans)
#Enable Warning IDE0058 ' Expression value is never used

            'If Trans.Tables(0).Rows.Count > 0 Then
            Return Trans.Tables(0).Rows.Count <> 0

            con.Close()
        End Using
    End Function

    Public Function IsOutDate(PolNo As String, Endno As Integer, LoadNo As Integer) As Boolean
        Dim Policy As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select * from PolicyFile where PolNo='{0}' and endNo={1} and loadno={2}", PolNo, Endno, LoadNo), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(Policy)
#Enable Warning IDE0058 ' Expression value is never used
            'Policy = RecSet("select * from PolicyFile where PolNo='" & PolNo & "' and endNo=" & Endno & " and loadno=" & LoadNo, Conn)
            Return Policy.Tables(0).Rows(0)("CoverTo") <= Today.Date And (Not Policy.Tables(0).Rows(0)("SubIns") = "MC" Or Not Policy.Tables(0).Rows(0)("SubIns") = "MA" Or Not Policy.Tables(0).Rows(0)("SubIns") = "MB")
            con.Close()
        End Using
    End Function

    Public Function GetPrevCustomer(OrderNo As String, EndNo As String, LoadNo As Integer) As Long
        Dim LastEnd As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select CustNo from PolicyFile where PolNo='{0}' and endNo={1} and loadNo={2}", OrderNo, Val(EndNo) - 1, LoadNo), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(LastEnd)
#Enable Warning IDE0058 ' Expression value is never used
            'LastEnd = RecSet("select CustNo from PolicyFile where PolNo='" & OrderNo & "' and endNo=" & Val(EndNo) - 1 & " and loadNo=" & LoadNo, Conn)
            Return If(LastEnd.Tables(0).Rows.Count <> 0, DirectCast(LastEnd.Tables(0).Rows(0)(0), Long), 0)
            con.Close()
        End Using
    End Function

    Public Sub MsgBob(ByRef Fr As Page, Msg As String)
        Fr.ClientScript.RegisterStartupScript(Fr.GetType(), "Message", String.Format("<script>alert('{0}');</script>", Msg))
    End Sub

    Public Function GetLastGroup(PolicyNo As String, sys As String) As Integer
        Dim LastGroup, LG As New DataSet
        'Dim T As String
        If PolicyNo = "" Then
            GetLastGroup = 1
        Else
            Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                If con.State = ConnectionState.Open Then
                    con.Close()
                Else

                End If
                con.Open()
                Dim dbadapter = New SqlDataAdapter(String.Format("SELECT MAX({0} .GroupNo)+1 AS gr, MAX(PolicyFile.OrderNo) AS grpol FROM PolicyFile INNER JOIN {1} ON {2}.OrderNo = PolicyFile.OrderNo where PolicyFile.PolNo='{3}' Or PolicyFile.OrderNo='{3}' And PolicyFile.SubIns={4}.SubIns And {5}.SubIns={6}", GetGroupFile(sys), GetGroupFile(sys), GetGroupFile(sys), PolicyNo, GetGroupFile(sys), GetGroupFile(sys), sys), con)
#Disable Warning IDE0058 ' Expression value is never used
                dbadapter.Fill(LastGroup)
#Enable Warning IDE0058 ' Expression value is never used
                'LastGroup = RecSet("SELECT MAX(" & GetGroupFile(sys) & " .GroupNo)+ 1 AS gr, MAX(PolicyFile.OrderNo) AS grpol" _
                '& " FROM PolicyFile INNER JOIN " & GetGroupFile(sys) & " ON " & GetGroupFile(sys) & ".OrderNo = PolicyFile.OrderNo " _
                '& "where PolicyFile.PolNo='" & PolicyNo & "' Or PolicyFile.OrderNo='" & PolicyNo & "' And PolicyFile.SubIns=" & GetGroupFile(sys) & ".SubIns And " & GetGroupFile(sys) & ".SubIns=" & sys & "", Conn)

                Return If(Not LastGroup.Tables(0).Rows.Item(0).IsNull(0), DirectCast(LastGroup.Tables(0).Rows(0)(0), Integer), 1)
                con.Close()
            End Using

        End If

    End Function

    Public Function IsGroup(OrderNo As String, sys As String, Group As Integer) As Boolean
        Dim LastGroup As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select GroupNo From {0} where OrderNo='{1}' and GroupNo={2}", GetGroupFile(sys), Trim(OrderNo), Group), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(LastGroup)
#Enable Warning IDE0058 ' Expression value is never used
            'LastGroup = RecSet("select GroupNo From " & GetGroupFile(sys) & " where OrderNo='" & Trim(OrderNo) & "' and GroupNo=" & Group, Conn)
            Return Not LastGroup.Tables(0).Rows.Count = 0
            con.Close()
        End Using
    End Function

    Public Function IsBranchOffice(Br As String) As Boolean
        Dim BranchOffice As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select iif(agent=1,0,1) from BranchInfo where BranchNo='" & Br & "'", con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(BranchOffice)
#Enable Warning IDE0058 ' Expression value is never used
            'LastGroup = RecSet("select GroupNo From " & GetGroupFile(sys) & " where OrderNo='" & Trim(OrderNo) & "' and GroupNo=" & Group, Conn)
            Return BranchOffice.Tables(0).Rows.Item(0).Item(0)
            con.Close()
        End Using
    End Function

    Public Function GetBoxAccount(Br As String) As String
        Dim BoxAccount As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select CashierAccount from BranchInfo where BranchNo='" & Br & "'", con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(BoxAccount)
#Enable Warning IDE0058 ' Expression value is never used
            'LastGroup = RecSet("select GroupNo From " & GetGroupFile(sys) & " where OrderNo='" & Trim(OrderNo) & "' and GroupNo=" & Group, Conn)
            Return BoxAccount.Tables(0).Rows.Item(0).Item(0)
            con.Close()
        End Using
    End Function

    Public Function GetcheqAccount(Br As String) As String
        Dim cheq As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select ChequeAccount from BranchInfo where BranchNo='" & Br & "'", con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(cheq)
#Enable Warning IDE0058 ' Expression value is never used
            'LastGroup = RecSet("select GroupNo From " & GetGroupFile(sys) & " where OrderNo='" & Trim(OrderNo) & "' and GroupNo=" & Group, Conn)
            Return cheq.Tables(0).Rows.Item(0).Item(0)
            con.Close()
        End Using
    End Function

    Public Function IsHeadQuarter(Br As String) As Boolean
        Dim Hq As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select BranchNo from BranchInfo where BranchNo=[dbo].[MainCenter]() ", con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(Hq)
#Enable Warning IDE0058 ' Expression value is never used
            'LastGroup = RecSet("select GroupNo From " & GetGroupFile(sys) & " where OrderNo='" & Trim(OrderNo) & "' and GroupNo=" & Group, Conn)
            Return Hq.Tables(0).Rows.Item(0).Item(0) = Br
            con.Close()
        End Using
    End Function

    Public Function IsBranch(Br As String) As Boolean
        Return IIf(Right(Trim(Br), 3) = "000", True, False)
    End Function

    Public Function IsAgentOrOffice(Br As String) As Boolean
        Return IIf(Right(Trim(Br), 3) <> "000", True, False)
    End Function

    Public Function IsOfficesManager(Uid As Int32) As Boolean
        Dim MultiManager As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select ManagerId From BranchInfo  where ManagerId={0}", Uid), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(MultiManager)
            Select Case MultiManager.Tables(0).Rows.Count
                Case 0
                    Return False
                Case 1
                    Return False
                Case > 1
                    Return True
                Case Else
                    Return False
            End Select

            con.Close()
        End Using
    End Function

    Public Function IsGroupedSys(sys As String) As Boolean
        Dim Grouped As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select Grouped From SubSystems  where SubSysNo='{0}' and Branch=[dbo].[MainCenter]()", sys), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(Grouped)
#Enable Warning IDE0058 ' Expression value is never used
            'Grouped = RecSet("select Grouped From SubSystems  where SubSysNo= '" & sys & "' and Branch='" & Br & "'", Conn)
            Return Grouped.Tables(0).Rows(0)(0)
            con.Close()
        End Using
    End Function

    Public Function IsReinsSys(sys As String) As Boolean
        Dim ReInsSys As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select SubSysNo From SubSystems where SysType=1 and SubSysNo='" & sys & "' and IssTyp is not null and Branch=[dbo].[MainCenter]()", sys), con)
#Disable Warning IDE0058 ' Expression value is never used
            dbadapter.Fill(ReInsSys)
#Enable Warning IDE0058 ' Expression value is never used
            'Grouped = RecSet("select Grouped From SubSystems  where SubSysNo= '" & sys & "' and Branch='" & Br & "'", Conn)
            If ReInsSys.Tables(0).Rows.Count = 0 Then
                Return False
            Else
                Return True
            End If
            con.Close()
        End Using
    End Function

    Public Function IsValidDate(d As String) As Boolean
        Dim DailyRec As New DataSet

        Dim dbadapter = New SqlDataAdapter("Select Month From Monthcheck where Month='" & Format(CDate(d), "yyyy/MM") & "' ", AccConn)
#Disable Warning IDE0058 ' Expression value is never used
        dbadapter.Fill(DailyRec)
#Enable Warning IDE0058 ' Expression value is never used
        'Response.Write(Format(CDate(d), "yyyy/MM"))

        Return DailyRec.Tables(0).Rows.Count <> 0

    End Function

    Public Class PaymentProcessor
        Public Shared Function ProcessMultiplePayments(
        payments As List(Of Dictionary(Of String, Object)),
        totalPaid As Decimal,
        totalDue As Decimal,
        receiptType As String,
        branchCode As String,
        currentUser As String,
        moveDate As Date,
        note As String,
        customerName As String,
        policyData As Dictionary(Of String, Object),
        connectionString As String
        ) As String

            ' ADD THIS: Check if already processing for this user/session
            Dim lockKey As String = $"MultiPayment_{branchCode}_{currentUser}_{Date.Now:HHmm}"
            If HttpContext.Current.Application(lockKey) IsNot Nothing Then
                Throw New Exception("جاري معالجة عملية سداد متعددة حالياً، يرجى الانتظار")
            End If

            Try
                ' Set lock
                HttpContext.Current.Application(lockKey) = True

                Using conn As New SqlConnection(connectionString)
                    conn.Open()
                    Using transaction As SqlTransaction = conn.BeginTransaction()
                        Try
                            ' 1. التحقق من التوازن
                            Dim tolerance As Decimal = 0.001D
                            If Math.Abs(totalPaid - totalDue) > tolerance Then
                                Throw New Exception($"إجمالي المدفوعات ({totalPaid:N3}) لا يساوي المبلغ المستحق ({totalDue:N3})")
                            End If

                            ' 2. الحصول على رقم الإيصال
                            Dim receiptNumber As String = GetReceiptNumber(conn, transaction, branchCode)

                            ' 3. حفظ تفاصيل الدفع
                            SavePaymentDetails(conn, transaction, receiptNumber, payments, receiptType, branchCode, currentUser, moveDate, note, customerName, policyData)

                            ' 4. تحديث حالة الوثائق (إذا كانت وثيقة تأمين)
                            ' CORRECTED: Check for Nothing first, then check if it's not "Other" receipt
                            If policyData("PolNo") <> "أخـرى" AndAlso policyData IsNot Nothing Then
                                UpdateDocumentStatus(conn, transaction, payments, policyData, totalPaid, branchCode)
                            End If

                            'If policyData IsNot Nothing AndAlso policyData.ContainsKey("PolNo") Then
                            '    Dim polNo As String = policyData("PolNo").ToString()
                            '    If polNo <> "أخـرى" Then
                            '        UpdateDocumentStatus(conn, transaction, payments, policyData, totalPaid, branchCode)
                            '    End If
                            'End If
                            ' 1. توليد رقم اليومية
                            Dim dailyNumber As String = GetDailyNumber(conn, transaction, branchCode)
                            ' 5. إنشاء إدخالات اليومية
                            CreateJournalEntries(dailyNumber, conn, transaction, receiptNumber, payments, totalPaid, receiptType, branchCode, currentUser, note, policyData)

                            ' 6. تسجيل النشاط
                            LogActivity(conn, transaction, receiptNumber, payments.Count, branchCode, currentUser)

                            ' 7. تأكيد
                            transaction.Commit()

                            Return dailyNumber 'receiptNumber
                        Catch ex As Exception
                            Try
                                transaction.Rollback()
                            Catch
                            End Try
                            Throw
                        End Try
                    End Using
                End Using
            Finally
                ' Release lock
                HttpContext.Current.Application.Remove(lockKey)
            End Try
        End Function

        Private Shared Function GetReceiptNumber(conn As SqlConnection, trans As SqlTransaction, branch As String) As String
            Using cmd As New SqlCommand("RecetNo", conn, trans)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@BR", branch)
                Dim result As Object = cmd.ExecuteScalar()
                Return If(result IsNot Nothing, result.ToString(), "")
            End Using
        End Function

        Private Shared Sub SavePaymentDetails(
        conn As SqlConnection,
        trans As SqlTransaction,
        receiptNumber As String,
        payments As List(Of Dictionary(Of String, Object)),
        receiptType As String,
        branchCode As String,
        currentUser As String,
        moveDate As DateTime,
        note As String,
        customerName As String,
        policyData As Dictionary(Of String, Object)
    )
            For i As Integer = 0 To payments.Count - 1
                Dim payment As Dictionary(Of String, Object) = payments(i)
                Dim subReceiptNo As String = $"{receiptNumber}-{i + 1:D2}"
                Dim accountInfo As String() = GetAccountInfoForPayment(payment, conn, trans, branchCode)
                If payment("Type") = 4 Or payment("Type") = 6 Then GoTo nxt
                Using cmd As New SqlCommand("INSERT INTO ACCMOVE(DocNo, SubDocNo, DocDat, CustName, PayMent, Amount, ForW, " _
                    & " EndNo, LoadNo, Tp, Branch, AccNo, Bank, Cur, Node, PayTp, UserName, Note, PaymentDetail, AccountUsed) " _
                    & " VALUES(@DocNo, @SubDocNo, @DocDat, @CustName, @PayMent, @Amount, @ForW," _
                    & " @EndNo, @LoadNo, @Tp, @Branch, @AccNo, @Bank, @Cur, @Node, @PayTp, @UserName, @Note, @PaymentDetail, @AccountUsed)", conn, trans)

                    cmd.Parameters.AddWithValue("@DocNo", receiptNumber)
                    cmd.Parameters.AddWithValue("@SubDocNo", subReceiptNo)
                    cmd.Parameters.AddWithValue("@DocDat", moveDate)
                    cmd.Parameters.AddWithValue("@CustName", customerName)
                    cmd.Parameters.AddWithValue("@PayMent", Convert.ToDecimal(payment("Amount")))
                    cmd.Parameters.AddWithValue("@Amount", Convert.ToDecimal(payment("Amount")))
                    cmd.Parameters.AddWithValue("@ForW", If(receiptType = "مقبوضات أخرى", "أخـرى", policyData("PolNo")))
                    cmd.Parameters.AddWithValue("@EndNo", If(receiptType = "مقبوضات أخرى", 0, If(policyData IsNot Nothing, CInt(policyData("EndNo")), 1)))
                    cmd.Parameters.AddWithValue("@LoadNo", If(receiptType = "مقبوضات أخرى", 0, If(policyData IsNot Nothing, CInt(policyData("LoadNo")), 1)))
                    cmd.Parameters.AddWithValue("@Tp", If(receiptType = "مقبوضات أخرى", "أخـرى", GetSysName(policyData("SubIns"))))
                    cmd.Parameters.AddWithValue("@Branch", branchCode)
                    cmd.Parameters.AddWithValue("@AccNo", accountInfo(0))
                    cmd.Parameters.AddWithValue("@Bank", accountInfo(1))
                    cmd.Parameters.AddWithValue("@Cur", policyData("tpName"))
                    cmd.Parameters.AddWithValue("@Node", "/")
                    cmd.Parameters.AddWithValue("@PayTp", payment("Type"))
                    cmd.Parameters.AddWithValue("@UserName", currentUser)
                    cmd.Parameters.AddWithValue("@Note", note.ToUpper.TrimEnd)
                    cmd.Parameters.AddWithValue("@PaymentDetail", $"{payment("TypeName")} - {payment("Details")}")
                    cmd.Parameters.AddWithValue("@AccountUsed", payment("Account").ToString())
                    cmd.ExecuteNonQuery()
                End Using
nxt:        Next
        End Sub

        Private Shared Function GetAccountInfoForPayment(
        payment As Dictionary(Of String, Object),
        conn As SqlConnection,
        trans As SqlTransaction,
        branchCode As String) As String()

            Dim accountNo As String
            Dim bankInfo As String

            Try
                Select Case payment("Type").ToString()
                    Case "1" ' نقداً
                        accountNo = GetCashAccount(conn, trans, branchCode)
                        bankInfo = "/"
                    Case "2" ' بصك
                        accountNo = GetCheckAccount(conn, trans, branchCode)
                        bankInfo = If(String.IsNullOrEmpty(payment("Details").ToString()), "/", payment("Details").ToString())
                    Case "3", "5" ' بإشعار, بطاقة مصرفية
                        If String.IsNullOrEmpty(payment("Account").ToString()) Then
                            accountNo = GetCashAccount(conn, trans, branchCode)
                        Else
                            accountNo = payment("Account").ToString()
                        End If
                        bankInfo = If(String.IsNullOrEmpty(payment("Details").ToString()), "/", payment("Details").ToString())
                    Case "4", "6" ' على الحساب, تحت التحصيل
                        If String.IsNullOrEmpty(payment("Account").ToString()) Then
                            accountNo = GetDefaultAccountForType(payment("Type").ToString(), conn, trans, branchCode)
                        Else
                            accountNo = payment("Account").ToString()
                        End If
                        bankInfo = "/"
                    Case Else
                        accountNo = GetCashAccount(conn, trans, branchCode)
                        bankInfo = "/"
                End Select
            Catch ex As Exception
                ' في حال الخطأ، استخدم الحساب النقدي كاحتياطي
                accountNo = GetCashAccount(conn, trans, branchCode)
                bankInfo = "/"
            End Try

            Return {accountNo, bankInfo}
        End Function

        Private Shared Function GetCashAccount(conn As SqlConnection, trans As SqlTransaction, branch As String) As String
            If conn Is Nothing Then
                Throw New ArgumentNullException(NameOf(conn))
            End If

            Return GetBoxAccount(branch)
            '    Using cmd As New SqlCommand("
            '        SELECT TOP 1 AccountNo
            '        FROM Accounts
            '        WHERE AccountName LIKE '%صندوق%'
            '        AND Level >= 4
            '        ORDER BY AccountNo
            '    ", conn, trans)
            '        cmd.Parameters.AddWithValue("@Branch", branch)
            '        Dim result As Object = cmd.ExecuteScalar()
            '        Return If(result IsNot Nothing, result.ToString(), "1.1.1.1.1")
            '    End Using
        End Function

        Private Shared Function GetCheckAccount(conn As SqlConnection, trans As SqlTransaction, branch As String) As String
            Return GetcheqAccount(branch)
            'Using cmd As New SqlCommand("
            '    SELECT TOP 1 AccountNo
            '    FROM Accounts
            '    WHERE AccountName LIKE '%صك%'
            '    AND Level >= 4
            '    ORDER BY AccountNo
            '", conn, trans)
            '    cmd.Parameters.AddWithValue("@Branch", branch)
            '    Dim result As Object = cmd.ExecuteScalar()
            '    Return If(result IsNot Nothing, result.ToString(), "1.1.1.2.1")
            'End Using
        End Function

        Private Shared Function GetDefaultAccountForType(paymentType As String, conn As SqlConnection, trans As SqlTransaction, branch As String) As String
            Select Case paymentType
                Case "4" ' على الحساب
                    Return "1.1.3.1.1"
                Case "6" ' تحت التحصيل
                    Return "1.1.10.1.1"
                Case Else
                    Return GetCashAccount(conn, trans, branch)
            End Select
        End Function

        Private Shared Sub UpdateDocumentStatus(
        conn As SqlConnection,
        trans As SqlTransaction,
        payments As List(Of Dictionary(Of String, Object)),
        policyData As Dictionary(Of String, Object),
        totalPaid As Decimal,
        branchCode As String)

            For i As Integer = 0 To payments.Count - 1
                Dim payment As Dictionary(Of String, Object) = payments(i)
                Dim accountInfo As String() = GetAccountInfoForPayment(payment, conn, trans, branchCode)
                If policyData("PolNo") = "أخـرى" Then GoTo nxt
                If payment("Type") = 4 Or payment("Type") = 6 Then
                    Using cmd As New SqlCommand(" UPDATE PolicyFile " _
                                    & "SET AccountNo = @Account, PayType = 2 " _
                                    & "WHERE PolNo = @PolNo AND EndNo = @EndNo AND LoadNo = @LoadNo AND Branch = @Branch ", conn, trans)

                        cmd.Parameters.AddWithValue("@Account", accountInfo(0))
                        cmd.Parameters.AddWithValue("@PolNo", policyData("PolNo"))
                        cmd.Parameters.AddWithValue("@EndNo", policyData("EndNo"))
                        cmd.Parameters.AddWithValue("@LoadNo", policyData("LoadNo"))
                        cmd.Parameters.AddWithValue("@Branch", branchCode)
                        cmd.ExecuteNonQuery()
                    End Using
                Else
                    Using cmd As New SqlCommand(" UPDATE PolicyFile " _
                                    & "SET Inbox = Inbox + @PaidAmount, Financed = 1 " _
                                    & "WHERE PolNo = @PolNo AND EndNo = @EndNo AND LoadNo = @LoadNo AND Branch = @Branch ", conn, trans)

                        cmd.Parameters.AddWithValue("@PaidAmount", Convert.ToDecimal(payment("Amount")))
                        cmd.Parameters.AddWithValue("@PolNo", policyData("PolNo"))
                        cmd.Parameters.AddWithValue("@EndNo", policyData("EndNo"))
                        cmd.Parameters.AddWithValue("@LoadNo", policyData("LoadNo"))
                        cmd.Parameters.AddWithValue("@Branch", branchCode)
                        cmd.ExecuteNonQuery()
                    End Using
                End If

nxt:        Next
        End Sub

        Private Shared Sub CreateJournalEntries(
        DailyNo As String,
        conn As SqlConnection,
        trans As SqlTransaction,
        receiptNumber As String,
        payments As List(Of Dictionary(Of String, Object)),
        totalAmount As Decimal,
        receiptType As String,
        branchCode As String,
        currentUser As String,
        note As String,
        policyData As Dictionary(Of String, Object))

            ' 2. إدخال رئيسي في اليومية
            Using cmd As New SqlCommand("INSERT INTO MainJournal ([DAILYNUM], [DAILYDTE], [DailyTyp], [ANALSNUM], [Comment],[MoveRef]," _
        & " [Currency], [Exchange], [CurUser], [RecNo], [DailyChk], [Branch], [SubBranch]) " _
        & " VALUES (@DAILYNUM, @DAILYDTE, @DailyTyp, @ANALSNUM, @Comment,@MoveRef, " _
        & " @Currency, @Exchange, @CurUser, @RecNo, @DailyChk, @Branch, @SubBranch) ", conn, trans)

                cmd.Parameters.AddWithValue("@DAILYNUM", DailyNo)
                cmd.Parameters.AddWithValue("@DAILYDTE", DateTime.Now)
                cmd.Parameters.AddWithValue("@DailyTyp", 1)
                cmd.Parameters.AddWithValue("@ANALSNUM", If(receiptType = "مقبوضات أخرى", "P", "A"))
                ' FIX: Build comment string first
                'Dim commentText As String = " سداد متعدد " & If(receiptType = "مقبوضات أخرى", "مقبوضات أخرى", " للوثيقة  " & policyData.Item("PolNo")) & " - " & note
                'Dim ttt As String = " سداد متعدد " & If(receiptType = "مقبوضات أخرى", "أخـرى", " للوثيقة  " & policyData("PolNo")) & " - " & note & "" 'If(receiptType = "مقبوضات أخرى", "أخـرى", policyData("PolNo"))
                '' FIX: Explicitly use NVarChar parameter type for Unicode support
                'Dim commentParam As SqlParameter = cmd.Parameters.Add("@Comment", SqlDbType.NVarChar, -1)
                'commentParam.Value = commentText
                cmd.Parameters.AddWithValue("@Comment", " سداد متعدد " & If(receiptType = "مقبوضات أخرى", "أخـرى", " للوثيقة  " & policyData("PolNo")) & " - " & note & "")
                cmd.Parameters.AddWithValue("@MoveRef", policyData.Item("OrderNo"))
                cmd.Parameters.AddWithValue("@Currency", 1)
                cmd.Parameters.AddWithValue("@Exchange", 1)
                cmd.Parameters.AddWithValue("@CurUser", currentUser)
                cmd.Parameters.AddWithValue("@RecNo", receiptNumber)
                cmd.Parameters.AddWithValue("@DailyChk", 0)
                cmd.Parameters.AddWithValue("@Branch", branchCode)
                cmd.Parameters.AddWithValue("@SubBranch", branchCode)
                cmd.ExecuteNonQuery()
            End Using

            ' 3. إدخالات المدين والدائن
            If receiptType = "مقبوضات أخرى" Then
                CreateOtherReceiptsJournalEntries(conn, trans, DailyNo, payments, branchCode, currentUser, receiptNumber, policyData)
            Else
                CreateInsuranceJournalEntries(conn, trans, DailyNo, payments, totalAmount, branchCode, currentUser, policyData, receiptNumber)
            End If
        End Sub
        Private Shared Sub CreateInsuranceJournalEntries(
        conn As SqlConnection,
        trans As SqlTransaction,
        dailyNumber As String,
        payments As List(Of Dictionary(Of String, Object)),
        totalAmount As Decimal,
        branchCode As String,
        currentUser As String,
        policyData As Dictionary(Of String, Object),
        receiptNumber As String)

            ' 1. Get the receivable account for this policy (used in several places)
            Dim receivableAccount As String = GetReceivableAccount(conn, trans, policyData, branchCode)

            ' 2. Query the PolicyFile for commission details (parameterized)
            Dim query As String = "SELECT Commision, AccountNo, SubIns FROM PolicyFile WHERE PolNo = @PolNo AND endNo = @EndNo AND loadno = @LoadNo"
            Dim commissionData As New DataSet

            Using cmd As New SqlCommand(query, conn, trans)
                ' Safely convert values; handle DBNull
                cmd.Parameters.AddWithValue("@PolNo", If(policyData("PolNo") Is DBNull.Value, Nothing, policyData("PolNo").ToString()))
                cmd.Parameters.AddWithValue("@EndNo", If(policyData("EndNo") Is DBNull.Value, 0, Convert.ToInt32(policyData("EndNo"))))
                cmd.Parameters.AddWithValue("@LoadNo", If(policyData("LoadNo") Is DBNull.Value, 0, Convert.ToInt32(policyData("LoadNo"))))

                Using adapter As New SqlDataAdapter(cmd)
                    adapter.Fill(commissionData)
                End Using
            End Using

            ' 3. Check if we found a policy record
            If commissionData.Tables.Count = 0 OrElse commissionData.Tables(0).Rows.Count = 0 Then
                Throw New Exception($"Policy not found: PolNo={policyData("PolNo")}, EndNo={policyData("EndNo")}, LoadNo={policyData("LoadNo")}")
            End If

            Dim policyRow As DataRow = commissionData.Tables(0).Rows(0)

            ' 4. Decide whether commission handling is needed
            If Convert.ToDecimal(policyRow("Commision")) <> 0 Then
                ' -----------------------------------------------------------------
                ' CASE 1: Commission exists → use stored procedure JournalEntrySingleCommision
                ' -----------------------------------------------------------------
                Using journalCmd As New SqlCommand("JournalEntrySingleCommision", conn, trans)
                    journalCmd.CommandType = CommandType.StoredProcedure

                    ' Use SqlDbType, not Odbc.OdbcType
                    journalCmd.Parameters.Add("@Br", SqlDbType.NVarChar).Value = branchCode
                    journalCmd.Parameters.Add("@Sys", SqlDbType.NVarChar).Value = policyRow("SubIns").ToString().Trim()
                    journalCmd.Parameters.Add("@PolNo", SqlDbType.NVarChar).Value = policyData("PolNo").ToString()
                    journalCmd.Parameters.Add("@EndNo", SqlDbType.Int).Value = Convert.ToInt32(policyData("EndNo"))
                    journalCmd.Parameters.Add("@loadNo", SqlDbType.Int).Value = Convert.ToInt32(policyData("LoadNo"))

                    Using reader As SqlDataReader = journalCmd.ExecuteReader()
                        If reader.HasRows Then
                            Dim dt As New DataTable()
                            dt.Load(reader)
                            ' Build the note string using receipt number (if any)
                            Dim noteBase As String = dt.Rows(0).Item(11).ToString()
                            Dim finalNote As String = If(receiptNumber = "/", noteBase, noteBase & IIf(payments(0)("Type").ToString().TrimEnd = "4" Or payments(0)("Type").ToString().TrimEnd = "6", "/", " / إيصال رقم# " & receiptNumber))

                            Dim noteStr As String = If(receiptNumber = "/", dt.Rows(0).Item(5).ToString(), dt.Rows(0).Item(5).ToString() & IIf(payments(0)("Type").ToString().TrimEnd = "4" Or payments(0)("Type").ToString().TrimEnd = "6", "/", " / إيصال رقم# " & receiptNumber))

                            ' For each row returned by the stored procedure, insert a journal entry
                            For Each row As DataRow In dt.Rows
                                Using insertCmd As New SqlCommand("INSERT INTO Journal([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser], [Branch], [SubBranch], [Note]) " &
                                     "VALUES (@DAILYNUM, @TP, @AccountNo, @Dr, @Cr, @CurUser, @Branch, @SubBranch, @Note)", conn, trans)

                                    insertCmd.Parameters.AddWithValue("@DAILYNUM", dailyNumber)
                                    insertCmd.Parameters.AddWithValue("@TP", 1)

                                    ' Determine the account number:
                                    '   - If AccTp is "TOT" or "COMout", use the first payment's account (you may need to adjust this logic)
                                    '   - Otherwise use the AccountNumber from the stored procedure
                                    Dim accountNo As String = ""
                                    Dim accTp As String = row("AccTp").ToString()
                                    If accTp = "TOT" OrElse accTp = "COMout" Then
                                        ' Use the account of the first payment (if any) – modify if you need a different payment
                                        If payments.Count > 0 AndAlso payments(0).ContainsKey("Account") Then
                                            For Each payment In payments
                                                Dim accountInfo As String() = GetAccountInfoForPayment(payment, conn, trans, branchCode)
                                                If payments(0)("Account").ToString().TrimEnd = "" Then
                                                    accountNo = accountInfo(0)
                                                Else
                                                    accountNo = payments(0)("Account").ToString().TrimEnd
                                                End If
                                            Next
                                        Else
                                            ' Fallback to the receivable account if no payment account is available
                                            accountNo = receivableAccount
                                        End If
                                    Else
                                        accountNo = If(IsDBNull(row("AccountNumber")), "", row("AccountNumber").ToString())
                                    End If

                                    insertCmd.Parameters.AddWithValue("@AccountNo", accountNo)
                                    insertCmd.Parameters.AddWithValue("@Dr", Convert.ToDecimal(row("DR")))
                                    insertCmd.Parameters.AddWithValue("@Cr", Convert.ToDecimal(row("CR")))
                                    insertCmd.Parameters.AddWithValue("@CurUser", currentUser)
                                    insertCmd.Parameters.AddWithValue("@Branch", branchCode)
                                    insertCmd.Parameters.AddWithValue("@SubBranch", branchCode)
                                    insertCmd.Parameters.AddWithValue("@Note", "/")   ' Use the note we built

                                    insertCmd.ExecuteNonQuery()
                                End Using
                            Next
                            Using UpdateCmd As New SqlCommand("UPDATE [dbo].[MainJournal] Set Comment=@Comment where DAILYNUM=@DAILYNUM And DailyTyp=@Tp", conn, trans)
                                UpdateCmd.Parameters.AddWithValue("@Comment", finalNote)
                                UpdateCmd.Parameters.AddWithValue("@DAILYNUM", dailyNumber)
                                UpdateCmd.Parameters.AddWithValue("@Tp", 1)
                                UpdateCmd.ExecuteNonQuery()
                            End Using
                        End If
                    End Using
                End Using
            Else
                ' -----------------------------------------------------------------
                ' CASE 2: No commission → insert debits per payment, then call JournalEntry
                ' -----------------------------------------------------------------
                ' إدخالات مدين حسب نوع الدفع
                For Each payment In payments
                    Dim accountInfo As String() = GetAccountInfoForPayment(payment, conn, trans, branchCode)
                    Dim accountNo As String = accountInfo(0)

                    Using cmd As New SqlCommand("INSERT INTO Journal([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser], [Branch], [SubBranch], [Note]) " _
                                                 & " VALUES (@DAILYNUM, @TP, @AccountNo, @Dr, @Cr, @CurUser, @Branch, @SubBranch, @Note) ", conn, trans)

                        cmd.Parameters.AddWithValue("@DAILYNUM", dailyNumber)
                        cmd.Parameters.AddWithValue("@TP", 1)
                        cmd.Parameters.AddWithValue("@AccountNo", accountNo)
                        cmd.Parameters.AddWithValue("@Dr", Convert.ToDecimal(payment("Amount")))
                        cmd.Parameters.AddWithValue("@Cr", 0)
                        cmd.Parameters.AddWithValue("@CurUser", currentUser)
                        cmd.Parameters.AddWithValue("@Branch", branchCode)
                        cmd.Parameters.AddWithValue("@SubBranch", branchCode)
                        cmd.Parameters.AddWithValue("@Note", payment("TypeName").ToString())
                        cmd.ExecuteNonQuery()
                    End Using
                Next

                ' FIXED: Properly call the stored procedure
                Using JournalCmd As New SqlCommand("JournalEntry", conn, trans)
                    JournalCmd.CommandType = CommandType.StoredProcedure

                    ' FIXED: Use SqlDbType instead of Odbc.OdbcType
                    JournalCmd.Parameters.Add("@PolNo", SqlDbType.NVarChar).Value = policyData.Item("PolNo")
                    JournalCmd.Parameters.Add("@EndNo", SqlDbType.Int).Value = policyData.Item("EndNo")
                    JournalCmd.Parameters.Add("@loadNo", SqlDbType.Int).Value = policyData.Item("LoadNo")

                    ' FIXED: Use ExecuteReader for stored procedures that return data
                    Using reader As SqlDataReader = JournalCmd.ExecuteReader()
                        If reader.HasRows Then
                            Dim dt As New DataTable()
                            dt.Load(reader)

                            Dim noteBase As String = dt.Rows(0).Item(5).ToString()
                            Dim finalNote As String = If(receiptNumber = "/", noteBase, noteBase & IIf(payments(0)("Type").ToString().TrimEnd = "4" Or payments(0)("Type").ToString().TrimEnd = "6", "/", " / إيصال رقم# " & receiptNumber))

                            Dim noteStr As String = If(receiptNumber = "/", dt.Rows(0).Item(5).ToString(), dt.Rows(0).Item(5).ToString() & IIf(payments(0)("Type").ToString().TrimEnd = "4" Or payments(0)("Type").ToString().TrimEnd = "6", "/", " / إيصال رقم# " & receiptNumber))

                            For Each row As DataRow In dt.Rows
                                If CDbl(row.Item("CR")) <> 0 Then
                                    Using cmd As New SqlCommand("INSERT INTO [dbo].[Journal]([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser],[Branch],[SubBranch]) " _
                                                     & " VALUES (@DAILYNUM,@TP, @AccountNo, @Dr,@Cr,@User,@Branch,@SubBranch)", conn, trans)

                                        cmd.Parameters.AddWithValue("@DAILYNUM", dailyNumber)
                                        cmd.Parameters.AddWithValue("@TP", 1)

                                        ' FIXED: Make sure column name is correct
                                        cmd.Parameters.AddWithValue("@AccountNo", If(IsDBNull(row.Item("AccountNumber")), row.Item(0).ToString(), row.Item("AccountNumber").ToString()))

                                        cmd.Parameters.AddWithValue("@Dr", 0)
                                        cmd.Parameters.AddWithValue("@Cr", Convert.ToDecimal(row.Item("CR")))

                                        ' FIXED: Parameter name was @@User, should be @User
                                        cmd.Parameters.AddWithValue("@User", currentUser)
                                        cmd.Parameters.AddWithValue("@Branch", branchCode)
                                        cmd.Parameters.AddWithValue("@SubBranch", branchCode)
                                        cmd.ExecuteNonQuery()
                                    End Using
                                    Using UpdateCmd As New SqlCommand("UPDATE [dbo].[MainJournal] Set Comment=@Comment where DAILYNUM=@DAILYNUM And DailyTyp=@Tp", conn, trans)
                                        UpdateCmd.Parameters.AddWithValue("@Comment", finalNote)
                                        UpdateCmd.Parameters.AddWithValue("@DAILYNUM", dailyNumber)
                                        UpdateCmd.Parameters.AddWithValue("@Tp", 1)
                                        UpdateCmd.ExecuteNonQuery()
                                    End Using
                                End If
                            Next
                        Else
                            ' No rows returned - handle appropriately
                            ' You might want to insert the receivable account entry here
                        End If
                    End Using

                End Using

            End If

        End Sub

        Private Shared Sub CreateOtherReceiptsJournalEntries(
        conn As SqlConnection,
        trans As SqlTransaction,
        dailyNumber As String,
        payments As List(Of Dictionary(Of String, Object)),
        branchCode As String,
        currentUser As String,
        receiptNumber As String,
        policyData As Dictionary(Of String, Object))

            Dim noteBase As String = " قيد استلام مبلغ من  / " & policyData("CustomerName").ToString.TrimEnd & " يمثل " & policyData("Note").ToString.TrimEnd

            For Each payment In payments
                Dim accountInfo As String() = GetAccountInfoForPayment(payment, conn, trans, branchCode)
                Dim accountNo As String = accountInfo(0)
                Using cmd As New SqlCommand(" INSERT INTO Journal([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser], [Branch], [Comment],[Note]) " _
                                         & "VALUES (@DAILYNUM, @TP, @AccountNo, @Dr, @Cr, @CurUser, @Branch, @Comment,@Note) ", conn, trans)
                    cmd.Parameters.AddWithValue("@DAILYNUM", dailyNumber)
                    cmd.Parameters.AddWithValue("@TP", 1)
                    cmd.Parameters.AddWithValue("@AccountNo", accountNo)
                    cmd.Parameters.AddWithValue("@Dr", Convert.ToDecimal(payment("Amount")))
                    cmd.Parameters.AddWithValue("@Cr", 0)
                    cmd.Parameters.AddWithValue("@CurUser", currentUser)
                    cmd.Parameters.AddWithValue("@Branch", branchCode)
                    cmd.Parameters.AddWithValue("@Comment", payment("TypeName").ToString())
                    cmd.Parameters.AddWithValue("@Note", payment("TypeName").ToString())

                    noteBase = noteBase & " ( " & Convert.ToDecimal(payment("Amount")).ToString & "/" & payment("TypeName").ToString() & " )/ "
                    cmd.ExecuteNonQuery()
                End Using
                noteBase = noteBase & " يايصال قبض رقم / " & receiptNumber

                Using UpdateCmd As New SqlCommand("UPDATE [dbo].[MainJournal] Set Comment=@Comment where DAILYNUM=@DAILYNUM And DailyTyp=@Tp", conn, trans)
                    UpdateCmd.Parameters.AddWithValue("@Comment", noteBase)
                    UpdateCmd.Parameters.AddWithValue("@DAILYNUM", dailyNumber)
                    UpdateCmd.Parameters.AddWithValue("@Tp", 1)
                    UpdateCmd.ExecuteNonQuery()
                End Using
            Next

        End Sub


        Private Shared Function GetDailyNumber(conn As SqlConnection, trans As SqlTransaction, branch As String) As String
            Using cmd As New SqlCommand("LastDailyNo", conn, trans)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@TP", "1")
                cmd.Parameters.AddWithValue("@Year", DateTime.Now.Year.ToString().Substring(2))
                cmd.Parameters.AddWithValue("@Br", branch)
                Dim result As Object = cmd.ExecuteScalar()
                Return If(result IsNot Nothing, result.ToString(), "")
            End Using
        End Function

        Private Shared Function GetReceivableAccount(
        conn As SqlConnection,
        trans As SqlTransaction,
        policyData As Dictionary(Of String, Object),
        branchCode As String
    ) As String
            ' محاولة جلب الحساب من ملف العميل
            Using cmd As New SqlCommand(" SELECT AccNo  " _
                & "FROM CustomerFile " _
                & "WHERE CustNo = @CustNo ", conn, trans)
                cmd.Parameters.AddWithValue("@CustNo", policyData("CustNo"))
                cmd.Parameters.AddWithValue("@Branch", branchCode)
                Dim result As Object = cmd.ExecuteScalar()
                If result IsNot Nothing Then Return result.ToString()
            End Using

            ' إذا فشل، استخدام الحساب الافتراضي
            Using cmd As New SqlCommand(" SELECT AccountNo  FROM Accounts " _
               & " WHERE AccountName LIKE '%مستحقات%'  " _
               & " ORDER BY AccountNo ", conn, trans)
                cmd.Parameters.AddWithValue("@Branch", branchCode)
                Dim result As Object = cmd.ExecuteScalar()
                Return If(result IsNot Nothing, result.ToString(), "1.1.3.1.1")
            End Using
        End Function

        Private Shared Sub LogActivity(
        conn As SqlConnection,
        trans As SqlTransaction,
        receiptNumber As String,
        paymentCount As Integer,
        branchCode As String,
        currentUser As String
    )
            Using cmd As New SqlCommand(" INSERT INTO SystemLog ([ActivityDate], [UserName], [ActivityType], " _
                 & "[Description], [BranchCode], [ReferenceNo]) " _
                 & "VALUES (@ActivityDate, @UserName, @ActivityType, " _
                 & "@Description, @BranchCode, @ReferenceNo) ", conn, trans)
                cmd.Parameters.AddWithValue("@ActivityDate", DateTime.Now)
                cmd.Parameters.AddWithValue("@UserName", currentUser)
                cmd.Parameters.AddWithValue("@ActivityType", "MULTI_PAYMENT")
                cmd.Parameters.AddWithValue("@Description", $"سداد متعدد - عدد الطرق: {paymentCount}")
                cmd.Parameters.AddWithValue("@BranchCode", branchCode)
                cmd.Parameters.AddWithValue("@ReferenceNo", receiptNumber)
                cmd.ExecuteNonQuery()
            End Using
        End Sub

    End Class


    Public Class BackgroundNotificationService
        Private Shared _lock As New Object()
        Private Shared _isRunning As Boolean = False

        Public Shared Sub Start()
            If _isRunning Then Return
            _isRunning = True
            ScheduleNextRun()
        End Sub

        Public Shared Sub [Stop]()
            _isRunning = False
            HttpRuntime.Cache.Remove("BackgroundNotificationService")
        End Sub

        Private Shared Sub ScheduleNextRun()
            If Not _isRunning Then Return

            Dim intervalMinutes As Integer = 6 * 60
            HttpRuntime.Cache.Insert(
            "BackgroundNotificationService",
            DateTime.Now,
            Nothing,
            Cache.NoAbsoluteExpiration,
            TimeSpan.FromMinutes(intervalMinutes),
            CacheItemPriority.NotRemovable,
            New CacheItemRemovedCallback(AddressOf OnCacheRemove)
        )
        End Sub

        Private Shared Sub OnCacheRemove(key As String, value As Object, reason As CacheItemRemovedReason)
            If reason = CacheItemRemovedReason.Expired Then
                Check72HourNotifications()
                ScheduleNextRun()
            End If
        End Sub

        Private Shared Sub Check72HourNotifications()
            SyncLock _lock
                Try
                    Dim connectionString As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString

                    Using con As New SqlConnection(connectionString)
                        con.Open()

                        Dim query As String = "
                    ;WITH FilteredPolicy AS (SELECT PolNo, IssuDate, IssueUser, SubIns, Branch, stop, PayType, AccountNo, inbox, TOTPRM
                        FROM PolicyFile
                        WHERE
                            right(Branch, 3) = '000'
                            AND stop = 0
                            AND PayType = 1
                            AND AccountNo = '0'
                            AND inbox <> TOTPRM
                            AND IssuDate IS NOT NULL
                    )
                    SELECT
                        Count(P.PolNo) as CNT,
                        P.SubIns,
                        dbo.SysName(P.SubIns) As Type,
                        Sum(P.Totprm) As TOT,
                        P.IssueUser As AccountNo,
                        A.AccountName,
                        Max(P.IssuDate) As dFrom,
                        Min(P.issudate) As DTO
                    FROM FilteredPolicy P
                    LEFT JOIN AccountFile A ON P.IssueUser = A.AccountNo
                    WHERE
                        DATEDIFF(MINUTE, P.IssuDate, GETDATE()) > 2880
                        AND NOT EXISTS (
                            SELECT 1 FROM Notifications n
                            WHERE n.UserId = P.IssueUser
                            AND n.Type = 3
                            AND n.Timestamp > DATEADD(HOUR, -72, GETDATE())
                            AND n.Message LIKE 'تذكير بتسوية%'
                        )
                    GROUP BY P.IssueUser, A.AccountName, P.SubIns
                    ORDER BY P.IssueUser"

                        Using cmd As New SqlCommand(query, con)
                            Using reader As SqlDataReader = cmd.ExecuteReader()
                                While reader.Read()
                                    Dim accountNo As String = reader("AccountNo").ToString()
                                    Dim cnt As Integer = reader("CNT")
                                    Dim typeName As String = reader("Type").ToString().TrimEnd()
                                    Dim tot As Decimal = reader("TOT")
                                    Dim dFrom As DateTime = reader("dFrom")
                                    Dim dTo As DateTime = reader("DTO")

                                    Dim message As String = $"تذكير بتسوية عدد {cnt} وثيقة {typeName} وبقيمة إجمالية {tot} صادرة في الفترة من {dFrom} إلى {dTo} يرجى مراجعة الإدارة المالية"

                                    ' Include Timestamp column
                                    Dim insertQuery As String = "
                                INSERT INTO Notifications
                                (Action, IsRead, Message, Type, UserId, GeneratedBy, GeneratedByID, Timestamp)
                                VALUES ('/', 0, @Message, 3, @UserId, 'SYSTEM', @UserId, GETDATE())"

                                    Using insertCmd As New SqlCommand(insertQuery, con)
                                        insertCmd.Parameters.AddWithValue("@Message", message)
                                        insertCmd.Parameters.AddWithValue("@UserId", accountNo)
                                        insertCmd.ExecuteNonQuery()
                                    End Using
                                End While
                            End Using
                        End Using

                        con.Close()
                    End Using
                Catch ex As Exception
                    WriteToLog("Error in Check72HourNotifications: " & ex.Message)
                End Try
            End SyncLock
        End Sub

        Private Shared Sub WriteToLog(message As String)
            Try
                Dim logPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Logs")
                If Not Directory.Exists(logPath) Then
                    Directory.CreateDirectory(logPath)
                End If
                Dim logFile As String = Path.Combine(logPath, $"Notifications_{DateTime.Today:yyyyMMdd}.log")
                File.AppendAllText(logFile, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}")
            Catch ex As Exception
                'Debug.WriteLine($"Log error: {ex.Message}")
            End Try
        End Sub
    End Class

    Public Class NotificationService
        Private Shared ReadOnly lockObject As New Object()
        Private Shared lastRunTime As DateTime = DateTime.MinValue

        Public Shared Sub Initialize()
            If HttpRuntime.Cache("NotificationServiceTimer") IsNot Nothing Then
                Return
            End If

            HttpRuntime.Cache.Add(
            "NotificationServiceTimer",
            DateTime.Now,
            Nothing,
            Cache.NoAbsoluteExpiration,
            TimeSpan.FromMinutes(30),
            CacheItemPriority.NotRemovable,
            New CacheItemRemovedCallback(AddressOf OnTimerExpired)
        )

            WriteToLog("Notification Service initialized.")
        End Sub

        Private Shared Sub OnTimerExpired(key As String, value As Object, reason As CacheItemRemovedReason)
            Try
                CheckAndSendNotifications()
                Initialize()
            Catch ex As Exception
                WriteToLog($"Error: {ex.Message}")
                Initialize()
            End Try
        End Sub

        Private Shared Sub CheckAndSendNotifications()
            SyncLock lockObject
                If (DateTime.Now - lastRunTime).TotalMinutes < 10 Then
                    Return
                End If

                Try
                    WriteToLog("Checking for overdue policies...")

                    Dim connectionString As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString

                    Using con As New SqlConnection(connectionString)
                        con.Open()

                        Dim query As String = "
                    SELECT DISTINCT p.IssueUser as AccountNo
                    FROM PolicyFile p
                    WHERE
                        RIGHT(p.Branch, 3) = '000'
                        AND p.stop = 0
                        AND p.PayType = 1
                        AND p.AccountNo = '0'
                        AND p.inbox <> p.TOTPRM
                        AND p.IssuDate IS NOT NULL
                        AND DATEDIFF(MINUTE, p.IssuDate, GETDATE()) > 2880
                        AND NOT EXISTS (SELECT 1 FROM Notifications n
                            WHERE n.UserId = p.IssueUser
                            AND n.Type = 3
                            AND n.Message LIKE N'%تذكير بتسوية%'
                            AND n.Timestamp > DATEADD(HOUR, -24, GETDATE())
                        )"

                        Using cmd As New SqlCommand(query, con)
                            Using reader As SqlDataReader = cmd.ExecuteReader()
                                While reader.Read()
                                    Dim accountNo As String = reader("AccountNo").ToString()
                                    ProcessUserNotifications(accountNo, con)
                                End While
                            End Using
                        End Using

                        lastRunTime = DateTime.Now
                        con.Close()
                    End Using

                    WriteToLog("Notification check completed.")
                Catch ex As Exception
                    'WriteToLog($"Error: {ex.Message}")
                End Try
            End SyncLock
        End Sub

        Private Shared Sub ProcessUserNotifications(accountNo As String, con As SqlConnection)
            Try
                Dim summaryQuery As String = " SELECT  COUNT(*) as PolicyCount,
                MAX(SubIns) as SubInsCode,
                SUM(TOTPRM) as TotalAmount,
                MIN(IssuDate) as EarliestDate,
                MAX(IssuDate) as LatestDate
            FROM PolicyFile
            WHERE
                RIGHT(Branch, 3) = '000'
                AND stop = 0
                AND PayType = 1
                AND AccountNo = '0'
                AND inbox <> TOTPRM
                AND IssuDate IS NOT NULL
                AND DATEDIFF(MINUTE, IssuDate, GETDATE()) > 2880
                AND IssueUser = @AccountNo"

                Using cmd As New SqlCommand(summaryQuery, con)
                    cmd.Parameters.Add("@AccountNo", SqlDbType.NVarChar, 50).Value = accountNo
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim policyCount As Integer = CInt(reader("PolicyCount"))
                            Dim subInsCode As String = reader("SubInsCode").ToString()
                            Dim totalAmount As Decimal = CDec(reader("TotalAmount"))
                            Dim earliestDate As DateTime = CDate(reader("EarliestDate"))
                            Dim latestDate As DateTime = CDate(reader("LatestDate"))

                            Dim policyType As String = GetArabicPolicyType(subInsCode, con)
                            SendNotification(accountNo, policyCount, policyType, totalAmount,
                                         earliestDate, latestDate, con)
                        End If
                    End Using
                End Using
            Catch ex As Exception
                WriteToLog($"Error processing user {accountNo}: {ex.Message}")
            End Try
        End Sub

        Private Shared Function GetArabicPolicyType(subInsCode As String, con As SqlConnection) As String
            Try
                Dim query As String = "SELECT SubSysName as SysName From SubSystems where SUBSYSNO=@SubIns and Branch=dbo.MainCenter() and SysType=1"
                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.Add("@SubIns", SqlDbType.NVarChar, 50).Value = subInsCode
                    Dim result As Object = cmd.ExecuteScalar()
                    Return If(result IsNot Nothing, result.ToString().Trim(), "غير محدد")
                End Using
            Catch ex As Exception
                Return "غير محدد"
            End Try
        End Function

        Private Shared Sub SendNotification(accountNo As String, policyCount As Integer,
                                        policyType As String, totalAmount As Decimal,
                                        earliestDate As DateTime, latestDate As DateTime,
                                        con As SqlConnection)
            Dim formattedAmount As String = totalAmount.ToString("N0", New Globalization.CultureInfo("en-US"))

            Dim messageBuilder As New StringBuilder()
            messageBuilder.Append("تذكير بتسوية عدد ")
            messageBuilder.Append(policyCount.ToString())
            messageBuilder.Append(" وثيقة ")
            messageBuilder.Append(policyType)
            messageBuilder.Append(" وبقيمة إجمالية ")
            messageBuilder.Append(formattedAmount)
            messageBuilder.Append(" صادرة في الفترة من ")
            messageBuilder.Append(earliestDate.ToString("yyyy/MM/dd", New Globalization.CultureInfo("en-US")))
            messageBuilder.Append(" إلى ")
            messageBuilder.Append(latestDate.ToString("yyyy/MM/dd", New Globalization.CultureInfo("en-US")))
            messageBuilder.Append(" يرجى مراجعة الإدارة المالية")

            Dim message As String = messageBuilder.ToString()

            Dim insertQuery As String = " INSERT INTO Notifications
        (Action, IsRead, Message, Type, UserId, GeneratedBy, GeneratedByID, Timestamp)
        VALUES (N'/', 0, @Message, 3, @UserId, N'SYSTEM', @UserId, GETDATE())"

            Using insertCmd As New SqlCommand(insertQuery, con)
                insertCmd.Parameters.Add("@Message", SqlDbType.NVarChar, -1).Value = message
                insertCmd.Parameters.Add("@UserId", SqlDbType.NVarChar, 50).Value = accountNo
                insertCmd.ExecuteNonQuery()
            End Using

            WriteToLog($"Notification sent to user {accountNo}")
        End Sub

        Private Shared Sub WriteToLog(message As String)
            Try
                ' Use AppDomain base path to avoid HttpContext
                Dim logPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Logs")
                If Not Directory.Exists(logPath) Then
                    Directory.CreateDirectory(logPath)
                End If
                Dim logFile As String = Path.Combine(logPath, $"Notifications_{DateTime.Today:yyyyMMdd}.log")
                File.AppendAllText(logFile, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}")
            Catch ex As Exception
                'Debug.WriteLine($"Log error: {ex.Message}")
            End Try
        End Sub

        Public Shared Sub CheckNow()
            CheckAndSendNotifications()
        End Sub
    End Class

End Module
'End Namespace