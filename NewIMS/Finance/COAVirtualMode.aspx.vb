Imports System.Data.SqlClient
Imports System.Threading.Tasks
Imports DevExpress.Web.ASPxTreeList

Public Class COAVirtualMode
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadRootNodesAsync()
        End If
    End Sub

    Private Async Function LoadRootNodesAsync() As Task
        Dim rootNodes = Await Task.Run(Function() GetChildrenFromDatabase(Nothing))
        COA.DataSource = rootNodes
        COA.DataBind()
    End Function
    Private Sub BindTreeList()
        Dim accounts As List(Of Account) = GetAllAccounts()
        COA.DataSource = accounts
        COA.DataBind()
    End Sub
    Public Function GetAllAccounts() As List(Of Account)
        Dim accounts As New List(Of Account)()
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString

        Using connection As New SqlConnection(connectionString)
            Dim query As String = "SELECT AccountNo, AccountName, ParentAcc, FullPath, Level, TranscationAcc FROM Accounts"
            Dim command As New SqlCommand(query, connection)

            connection.Open()
            Dim reader As SqlDataReader = command.ExecuteReader()

            While reader.Read()
                Dim account As New Account()
                account.AccountNo = reader("AccountNo").ToString()
                account.AccountName = reader("AccountName").ToString()
                account.ParentAcc = If(IsDBNull(reader("ParentAcc")), Nothing, reader("ParentAcc").ToString())
                account.FullPath = reader("FullPath").ToString()
                account.Level = Convert.ToInt32(reader("Level"))
                account.TranscationAcc = Convert.ToBoolean(reader("TranscationAcc"))
                accounts.Add(account)
            End While
        End Using

        Return accounts
    End Function

    Protected Sub COA_VirtualModeCreateChildren(sender As Object, e As TreeListVirtualModeCreateChildrenEventArgs)
        Dim parentAcc As String = If(String.IsNullOrEmpty(e.NodeObject), Nothing, e.NodeObject.ToString())
        Dim children As List(Of Account) = GetChildrenFromDatabase(parentAcc)
        e.Children = children
    End Sub

    Private Function GetChildrenFromDatabase(parentAcc As String) As List(Of Account)
        Dim children As New List(Of Account)()
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString

        Using connection As New SqlConnection(connectionString)

            Dim query As String = "SELECT AccountNo, AccountName, ParentAcc, FullPath, Level, TranscationAcc FROM Accounts WHERE ParentAcc = @ParentAcc"
            Dim command As New SqlCommand(query, connection)
            command.Parameters.AddWithValue("@ParentAcc", If(String.IsNullOrEmpty(parentAcc), CObj(DBNull.Value), parentAcc))
            'command.Parameters.AddWithValue("@ParentAcc", If(String.IsNullOrEmpty(parentAcc), "NULL", parentAcc))
            connection.Open()
            Dim reader As SqlDataReader = command.ExecuteReader()

            While reader.Read()
                Dim account As New Account()
                account.AccountNo = reader("AccountNo").ToString()
                account.AccountName = reader("AccountName").ToString()
                account.ParentAcc = If(IsDBNull(reader("ParentAcc")), Nothing, reader("ParentAcc").ToString())
                account.FullPath = reader("FullPath").ToString()
                account.Level = Convert.ToInt32(reader("Level"))
                account.TranscationAcc = Convert.ToBoolean(reader("TranscationAcc"))
                children.Add(account)
            End While
        End Using

        Return children
    End Function

    Protected Sub COA_VirtualModeNodeCreating(sender As Object, e As TreeListVirtualModeNodeCreatingEventArgs)
        Dim accountNo As String = e.NodeKeyValue.ToString()
        Dim account As Account = GetNodeFromDatabase(accountNo)

        e.NodeKeyValue = account.AccountNo
        e.SetNodeValue("AccountNo", account.AccountNo)
        e.SetNodeValue("AccountName", account.AccountName)
        e.SetNodeValue("FullPath", account.FullPath)
        e.SetNodeValue("Level", account.Level)
        e.SetNodeValue("TranscationAcc", account.TranscationAcc)
    End Sub


    Private Function GetNodeFromDatabase(accountNo As String) As Account
        Dim account As New Account()
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString

        Using connection As New SqlConnection(connectionString)
            Dim query As String = "SELECT AccountNo, AccountName, ParentAcc, FullPath, Level, TranscationAcc FROM Accounts WHERE AccountNo = @AccountNo"
            Dim command As New SqlCommand(query, connection)
            command.Parameters.AddWithValue("@AccountNo", accountNo)

            connection.Open()
            Dim reader As SqlDataReader = command.ExecuteReader()

            If reader.Read() Then
                account.AccountNo = reader("AccountNo").ToString()
                account.AccountName = reader("AccountName").ToString()
                account.ParentAcc = If(IsDBNull(reader("ParentAcc")), Nothing, reader("ParentAcc").ToString())
                account.FullPath = reader("FullPath").ToString()
                account.Level = Convert.ToInt32(reader("Level"))
                account.TranscationAcc = Convert.ToBoolean(reader("TranscationAcc"))
            End If
        End Using

        Return account
    End Function
End Class