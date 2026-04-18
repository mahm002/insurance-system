
Imports Microsoft.Reporting.WebForms
Imports System.Data
Partial Class Sadad
    Inherits System.Web.UI.Page
    Dim Perm() As String
    Dim Lo() As String
    Dim Net As Double
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'RadDatePicker1.SelectedDate = Date.Today
            'RadDatePicker2.SelectedDate = Date.Today
            Perm = Me.Session("LogInfo")
            If Not IsNothing(Request("ClmNo")) Then
                TextBox1.Text = Request("ClmNo")
                TextBox4.Text = Request("PolNo")
            End If
            Lo = Me.Session("LogInfo")
            SqlDataSource1.ConnectionString = Conn.ConnectionString
            SqlDataSource1.SelectCommand = "SELECT tasno,net,total" & " FROM esal1 WHERE clmno='" & TextBox1.Text & "' and branch = '" & Lo(7) & "'"
            SqlDataSource1.SelectCommandType = SqlDataSourceCommandType.Text
            SqlDataSource1.Select(System.Web.UI.DataSourceSelectArguments.Empty)
            GridView1.DataBind()
            GridView1.DataBind()
        Catch
            MsgBob(Me.Page, Err.Description)
        End Try
        Call SetLists()
        Table4.Visible = False
    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try
            'esno.Text = CallSP("lastinvno", Conn, Nothing)

            If Not IsNumeric(cash.Text) Then
                cash.Text = ""
                cash.Focus()
                Exit Sub
            End If
            If Not IsNumeric(disno.Text) Then
                disno.Text = ""
                disno.Focus()
                Exit Sub
            End If
            If Not IsNumeric(esno.Text) Then
                esno.Text = ""
                esno.Focus()
                Exit Sub
            End If
            If Not IsNumeric(disno.Text) Then
                disno.Text = ""
                disno.Focus()
                Exit Sub
            End If

            ExecSql("insert into esal(clmno,disd,esd,acname,cash,total,net,tasno,eno,disno,esno,Currency,ExcRate,branch,UserName) values('" & TextBox1.Text & "'," & "CONVERT(DATETIME,'" & Format(CDate(RadDatePicker2.SelectedDate), "yyyy-MM-dd") & " 00:00:00',102)," & "CONVERT(DATETIME,'" & Format(CDate(RadDatePicker1.SelectedDate), "yyyy-MM-dd") & " 00:00:00',102),'" & acname.Text & "'," & cash.Text & "," & TextBox2.Text & "," & TextBox3.Text & "," & tasno.Text & "," & eno.Text & "," & disno.Text & "," & esno.Text & "," & GetEbaValue(Currency, 1) & "," & Exc.Text & " ,'" & Lo(7) & "','" & Lo(0) & "')")
            ' ExecSql("insert into collec(polno,clmno,date,value,count,type,tasno) values('" & GetTextValue(pageFooter.FindControl("polNo")) & "','" & GetTextValue(pageFooter.FindControl("clmnum")) & "'," & "CONVERT(DATETIME,'" & Format(CDate(esd.Text), "yyyy-MM-dd") & " 00:00:00',102)," & cash.Text & "," & eno.Text & "," & fl & "," & tasno.Text & ")")
            'Parm = Array.CreateInstance(GetType(ReportParameter), 1)
            'SetRepPm("inv", False, GenArray(esno.Text), Parm, 0)
            'Me.Session.Add("Parms", Parm)
            'Response.Write("<script> window.open('http://" & System.Net.Dns.GetHostAddresses(Server.MachineName)(0).ToString & "/unitedweb/OutPutManagement/Preview.aspx?Report=" & "/TakafulyReports/esal" & "','_new'); </script>")
            acname.Enabled = True
            clearscr()
            'Button3.Enabled = False
        Catch
            MsgBob(Me.Page, Err.Description)
        End Try
    End Sub
    Protected Sub tasno_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tasno.TextChanged

    End Sub

    Public Sub clearscr()
        eno.Text = ""
        tasno.Text = ""
        acname.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        cash.Text = ""
        disno.Text = ""
        esno.Text = ""
        Exc.Text = ""
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand

    End Sub


    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Dim issudata As DataSet, n As Integer
        Try
            acname.Enabled = True
            acname.Text = ""
            tasno.Text = GridView1.SelectedRow.Cells(1).Text
            TextBox2.Text = GridView1.SelectedRow.Cells(3).Text
            TextBox3.Text = GridView1.SelectedRow.Cells(2).Text
            SqlDataSource2.ConnectionString = Conn.ConnectionString
            SqlDataSource2.SelectCommand = "SELECT tasno,net,total,esno" & " FROM esal WHERE clmno='" & TextBox1.Text & "' and tasno=" & GridView1.SelectedRow.Cells(1).Text & " and branch = '" & Lo(7) & "'"
            SqlDataSource2.SelectCommandType = SqlDataSourceCommandType.Text
            SqlDataSource2.Select(System.Web.UI.DataSourceSelectArguments.Empty)
            GridView2.DataBind()
            GridView2.DataBind()

            issudata = RecSet("select * from esal where clmno='" & TextBox1.Text & "' and tasno=" & tasno.Text & "and branch = '" & Lo(7) & "'", Conn)
            If issudata.Tables(0).Rows.Count = 0 Then
                eno.Text = "1"
            Else
                n = issudata.Tables(0).Rows.Count()
                eno.Text = Val(issudata.Tables(0).Rows.Item(n - 1).Item("eno")) + 1
                acname.Text = issudata.Tables(0).Rows.Item(0).Item("acname")
                acname.Enabled = False
                Button3.Enabled = True

            End If
            issudata = RecSet("select * from esal ", Conn)
            n = issudata.Tables(0).Rows.Count
            esno.Text = n + 1
            esno.Enabled = False

        Catch
            MsgBob(Me.Page, Err.Description)
        End Try

    End Sub

    Protected Sub acname_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles acname.TextChanged
        cash.Focus()
        Button3.Enabled = True
    End Sub

    Protected Sub cash_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cash.TextChanged
        esno.Focus()
    End Sub

    Protected Sub esno_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles esno.TextChanged
    End Sub

    Protected Sub disno_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles disno.TextChanged
    End Sub


    Protected Sub eno_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles eno.TextChanged

    End Sub


    Protected Sub SqlDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SqlDataSource1.Selecting

    End Sub


    Protected Sub GridView2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView2.RowCommand
        'sending report parameters to /TakafulyReports/esal
        Select Case e.CommandName
            Case "LvlPrn"
                GridView2.SelectedIndex = Val(e.CommandArgument.ToString)
                Parm = Array.CreateInstance(GetType(Microsoft.Reporting.WebForms.ReportParameter), 2)
                SetRepPm("clmno", False, GenArray(TextBox1.Text), Parm, 0)
                SetRepPm("tasno", False, GenArray(GridView2.SelectedRow.Cells(1).Text.ToString()), Parm, 1)
                Me.Session.Add("Parms", Parm)
                Response.Write("<script> window.open('http://" & Server.MachineName & "/Takafuly/OutPutManagement/Preview.aspx?Report=" & "/TakafulyReports/esal" & "','_new'); </script>")
        End Select
    End Sub

    Protected Sub GridView2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView2.SelectedIndexChanged
        Dim issudata As DataSet, n As Integer
        Table4.Visible = True
        SqlDataSource3.ConnectionString = Conn.ConnectionString
        SqlDataSource3.SelectCommand = "SELECT givno,val,res,gidate,giwho" & " FROM MaClmGiv WHERE ClmNo='" & TextBox1.Text & "' and Branch = '" & Lo(7) & "'"
        SqlDataSource3.SelectCommandType = SqlDataSourceCommandType.Text
        SqlDataSource3.Select(System.Web.UI.DataSourceSelectArguments.Empty)
        GridView3.DataBind()
        GridView3.DataBind()

        issudata = RecSet("select * from MaClmGiv where ClmNo='" & TextBox1.Text & "' and Branch = '" & Lo(7) & "'", Conn)
        If issudata.Tables(0).Rows.Count = 0 Then
            TextBox7.Text = "1"
        Else
            n = issudata.Tables(0).Rows.Count()
            TextBox7.Text = Val(issudata.Tables(0).Rows.Item(n - 1).Item("givno")) + 1

        End If
    End Sub

    Protected Sub TextBox2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub

    Protected Sub TextBox1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub
    'Protected Sub Currency_GetPage(ByVal sender As Object, ByVal e As eba.Web.ComboGetPageEventArgs) Handles Currency.GetPage
    '    Dim DB As New DataSet
    '    Dim Connlocal As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection(ConfigurationManager.AppSettings("Conn"))
    '    Dim dbadapter = New Data.SqlClient.SqlDataAdapter("SELECT TOP " & e.PageSize & " TpName,TpNo FROM ExtraInfo WHERE tp='" & sender.CssClassName & "' and TpName > '" & e.LastString & "' AND TpName LIKE '" & e.SearchSubstring & "%' ORDER BY TpName", Connlocal)
    '    dbadapter.Fill(DB)
    '    e.NextPage = DB
    '    e.NextPage = RecSet("SELECT TOP " & e.PageSize & " TpName,TpNo FROM ExtraInfo WHERE tp='" & sender.CssClassName & "' and TpName > '" & e.LastString & "' AND TpName LIKE '" & e.SearchSubstring & "%' ORDER BY TpName", Conn)
    'End Sub


    Private Sub SetLists()
        'Bank.DataSource = RecSet("select TPName,TPNo from EXTRAINFO where TP='Banks'", Conn)
        Currency.DataSource = RecSet("select TPName,TPNo from EXTRAINFO where TP='Cur'", Conn)

    End Sub

    Protected Sub TextBox3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged

    End Sub

    Protected Sub Button4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button4.Click
        ExecSql("insert into MaClmGiv(ClmNo,gidate,giwho,givno,val,res,Branch) values('" & TextBox1.Text & "'," & "CONVERT(DATETIME,'" & Format(CDate(RadDatePicker3.SelectedDate), "yyyy-MM-dd") & " 00:00:00',102),'" & TextBox6.Text & "'," & TextBox7.Text & "," & TextBox5.Text & ",'" & TextBox8.Text & "','" & Lo(7) & "')")
        GridView3.DataBind()
        GridView3.DataBind()

    End Sub

    Protected Sub GridView3_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView3.RowCommand
        Select Case e.CommandName
            Case "LvlPrn2"
                GridView3.SelectedIndex = Val(e.CommandArgument.ToString)
                Parm = Array.CreateInstance(GetType(ReportParameter), 4)
                SetRepPm("datef", True, GenArray(Format(RadDatePicker1.SelectedDate, "yyyy/MM/dd")), Parm, 0)
                SetRepPm("datet", True, GenArray(Format(RadDatePicker1.SelectedDate, "yyyy/MM/dd")), Parm, 1)
                SetRepPm("SubSystem", False, GenArray(Request("sys")), Parm, 2)
                SetRepPm("branch", False, GenArray(Lo(7)), Parm, 3)
                'Select Case Request("sys")
                '   Case "01"
                'Dim SubSys() As String = {"01"}
                'SetRepPm("SubSystem", IIf(Lo(0) = "administrator", True, False), SubSys, Parm, 2)
                '   Case Else
                'SetRepPm("SubSystem", IIf(Lo(0) = "administrator", True, False), GenArray(Page.Request.QueryString("sys")), Parm, 2)
                'End Select
                Me.Session.Add("Parms", Parm)
                Response.Write("<script> window.open('http://" & Server.MachineName & "/Takafuly/OutPutManagement/Preview.aspx?Report=/TakafulyReports/ClaimsRecovered','_new'); </script>")

        End Select
    End Sub

    Protected Sub GridView3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView3.SelectedIndexChanged

    End Sub

    Protected Sub Form1_Load(sender As Object, e As System.EventArgs) Handles Form1.Load

    End Sub
End Class
