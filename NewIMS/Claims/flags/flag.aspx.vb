Partial Class CFlag
    Inherits System.Web.UI.Page
    Dim Net As Double
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetLists()

    End Sub

    Private Sub SetLists()
        Dim fl As String
        fl = " "
        MarBy.DataSource = RecSet("select type from EXTRAINFO where type<>'" & fl & "'", Conn)

    End Sub
    Private Sub PremCalc()
    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim IssuData As DataSet, st As String, n As Integer
        IssuData = RecSet("select * from EXTRAINFO where type='" & MarBy.TextValue & "'", Conn)

        st = IssuData.Tables(0).Rows.Item(0).Item("TP")
        IssuData = RecSet("select * from EXTRAINFO where tp='" & st & "'", Conn)
        n = IssuData.Tables(0).Rows.Count
        n = n + 1
        IssuData = RecSet("select * from EXTRAINFO where tp='" & st & "' and tpname='" & Margin.Text & "'", Conn)
        If IssuData.Tables(0).Rows.Count = 0 Then
            ExecSql("insert into EXTRAINFO(TP,TPNo,TPName) values('" & st & "'," & n & ",'" & Margin.Text & "')")
            SqlDataSource1.ConnectionString = Conn.ConnectionString
            SqlDataSource1.SelectCommand = "SELECT tpno,tpname" & " FROM EXTRAINFO WHERE tp='" & st & "' "
            SqlDataSource1.SelectCommandType = SqlDataSourceCommandType.Text
            SqlDataSource1.Select(System.Web.UI.DataSourceSelectArguments.Empty)
            GridView1.DataBind()
            GridView1.DataBind()

            Button3.Enabled = False
            Button2.Enabled = False
            clearscr()
        Else
            Margin.Text = ""
            Margin.Focus()
        End If
    End Sub


    Public Sub clearscr()
        MarBy.TextValue = ""
        Margin.Text = ""
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Button3.Enabled = False
        Button2.Enabled = False
        clearscr()
    End Sub

    Protected Sub Margin_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Margin.TextChanged
        Dim IssuData As DataSet, st As String
        IssuData = RecSet("select * from EXTRAINFO where type='" & MarBy.TextValue & "'", Conn)
        st = IssuData.Tables(0).Rows.Item(0).Item("TP")

        SqlDataSource1.ConnectionString = Conn.ConnectionString
        SqlDataSource1.SelectCommand = "SELECT tpno,tpname" & " FROM EXTRAINFO WHERE tp='" & st & "' "
        SqlDataSource1.SelectCommandType = SqlDataSourceCommandType.Text
        SqlDataSource1.Select(System.Web.UI.DataSourceSelectArguments.Empty)
        GridView1.DataBind()
        GridView1.DataBind()

        Button3.Enabled = True
        Button2.Enabled = True

    End Sub



    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub
End Class
