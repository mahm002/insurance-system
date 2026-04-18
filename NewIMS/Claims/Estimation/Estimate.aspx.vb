Imports System.Web.UI
Partial Public Class Estimate
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim IssuData As DataSet
        Dim MaxDate As DataSet
        Dim Perm() As String

        Perm = Session("LogInfo")
        ' Lo = Me.Session("LogInfo")
        date1.Value = Format(Today.Date, "yyyy-MM-dd")

        If Not IsNothing(Request("ClmNo")) Then
            clmno.Text = Request("ClmNo")
            polno.Text = Request("PolNo")
            loadno.Text = Request("loadno")
        End If

        IssuData = RecSet("select PolNo from MainClaimFile where ClmNo='" & clmno.Text & "' AND Branch='" & Session("Branch") & "' ")
        polno.Text = IssuData.Tables(0).Rows.Item(0).Item("PolNo")
        MaxDate = RecSet("select MAX(DATE) as D from Estimation where ClmNo='" & clmno.Text & "' AND PolNo= '" & polno.Text & "' ")
        SqlDataSource1.ConnectionString = Conn.ConnectionString
        SqlDataSource1.SelectCommand = "SELECT value,date,Sn FROM Estimation WHERE clmno='" & clmno.Text & "' and PolNo= '" & polno.Text & "' Order By Sn"
        SqlDataSource1.SelectCommandType = SqlDataSourceCommandType.Text
        SqlDataSource1.Select(DataSourceSelectArguments.Empty)
        GridView1.DataBind()
        GridView1.DataBind()
        value.Focus()
        date1.MinDate = MaxDate.Tables(0).Rows.Item(0).Item("D")
    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button3.Click

        Dim IssuData As DataSet

        'If Val(Mid(Perm(2), InStr(Perm(2), "01") + 3, 1)) < 5 Then
        'MsgBob(Me, "  áíÓ áÏÓß ÇáÕáÇÍíÉ  ")
        'End If
        Try
            If Not IsNumeric(value.Text) Then
                value.Text = ""
                value.Focus()
                'Exit Sub
            End If
            IssuData = RecSet("select * from Estimation where clmno='" & clmno.Text & "' AND PolNo='" & polno.Text & "'")
            If IssuData.Tables(0).Rows.Count = 0 Then
                count.Text = "1"
            Else
                count.Text = IssuData.Tables(0).Rows.Count + 1
            End If
            ExecConn("insert into Estimation(polno,clmno,date,value,Sn) values('" & polno.Text & "','" & clmno.Text & "'," & "CONVERT(DATETIME,'" & Format(CDate(date1.Value), "yyyy-MM-dd") & " 00:00:00',102)," & value.Text & "," & count.Text & ")", Conn)
            'MsgBob(Me, "  áíÓ áÏÓß ÇáÕáÇÍíÉ  ")
            'ExecSql("insert into UnderWriting(polno,clmno) values('" & polno.Text & "','" & clmno.Text & "')")
            SqlDataSource1.ConnectionString = Conn.ConnectionString
            SqlDataSource1.SelectCommand = "SELECT value,date,Sn FROM Estimation WHERE clmno='" & clmno.Text & "' AND PolNo='" & polno.Text & "' Order By Count"
            SqlDataSource1.SelectCommandType = SqlDataSourceCommandType.Text
            SqlDataSource1.Select(DataSourceSelectArguments.Empty)
            GridView1.DataBind()
            'ExecSql("insert into collec(polno,clmno,date,value,count,type) values('" & GetTextValue(pageFooter.FindControl("polNo")) & "','" & GetTextValue(pageFooter.FindControl("clmnum")) & "'," & "CONVERT(DATETIME,'" & Format(CDate(maincdat.Text), "yyyy-MM-dd") & " 00:00:00',102)," & mainfdat.Text & "," & docval.Text & "," & fl & ")")
            'Button3.Enabled = False
            Clearscr()
        Catch
            MsgBob(Me.Page, Err.Description)
        End Try

    End Sub

    Protected Sub Mainfdat_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles value.TextChanged
        Dim IssuData As DataSet
        IssuData = RecSet("select * from Estimation where clmno='" & clmno.Text & "' AND PolNo='" & polno.Text & "'")
        If IssuData.Tables(0).Rows.Count = 0 Then
            count.Text = "1"
        Else
            count.Text = IssuData.Tables(0).Rows.Count + 1
        End If
        Button3.Enabled = True

    End Sub

    Public Sub Clearscr()
        value.Text = ""
        count.Text = ""
    End Sub

    Protected Sub Button4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button4.Click
        SqlDataSource1.ConnectionString = Conn.ConnectionString
        SqlDataSource1.SelectCommand = "SELECT clmno,value,date FROM Estimation WHERE clmno='" & clmno.Text & "' AND PolNo='" & polno.Text & "'"
        SqlDataSource1.SelectCommandType = SqlDataSourceCommandType.Text
        SqlDataSource1.Select(DataSourceSelectArguments.Empty)
        GridView1.DataBind()
        GridView1.DataBind()
    End Sub

    Protected Sub ImageButton1_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
        'sending report parameters to /TakafulyReports/RESPALL
        Select Case e.CommandName
            Case "LvlPrn"
                GridView1.SelectedIndex = Val(e.CommandArgument.ToString)
                'Parm = Array.CreateInstance(GetType(ReportParameter), IIf(Request("sys") = "05", 4, 3))
                Parm = Array.CreateInstance(GetType(Microsoft.Reporting.WebForms.ReportParameter), 1)
                SetRepPm("clmno", False, GenArray(clmno.Text), Parm, 0)
                'SetRepPm("PolicyNo", False, GenArray(polno.Text), Parm, 1)
                'SetRepPm("EndNo", False, GenArray(EndNo.Text), Parm, 1)
                'SetRepPm("Sys", False, GenArray(Request("sys")), Parm, 1)
                'If Request("sys") = "05" Then SetRepPm("LoadNo", False, GenArray(loadno.Text), Parm, 1)

                'Parm = Array.CreateInstance(GetType(Microsoft.Reporting.WebForms.ReportParameter), 1)
                'SetRepPm("clmno", False, GenArray(clmno.Text), Parm, 0)
                'SetRepPm("clmno", False, GenArray(loadno.Text), Parm, 0)
                Me.Session.Add("Parms", Parm)
                'Response.Write("<script> window.open('http://" & System.Server.MachineName & "/unitedweb/OutPutManagement/Preview.aspx?Report=" & "/TakafulyReports/RESPALL" & "','_new'); </script>")
                Response.Write("<script> window.open('http://" & Server.MachineName & "/Takafuly/OutPutManagement/Preview.aspx?Report=" & "/TakafulyReports/RESP" & "','_new'); </script>")

        End Select
        'Parm = Array.CreateInstance(GetType(Microsoft.Reporting.WebForms.ReportParameter), 1)
        'SetRepPm("esalno", False, GenArray(GridView2.SelectedRow.Cells(2).Text.ToString), Parm, 0)
        'Me.Session.Add("Parms", Parm)
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click

    End Sub

    Protected Sub Button5_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button5.Click
        ExecConn("insert into UnderWriting(polno,clmno) values('" & polno.Text & "','" & clmno.Text & "')", Conn)
        'Response.Write("<script> window.open('http://" & Server.MachineName & "/Takafuly/OutPutManagement/Preview.aspx?Report=" & "/TakafulyReports/RESPALL" & "','_new'); </script>")
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Select Case e.CommandName
            Case "LvlPrn"
                GridView1.SelectedIndex = Val(e.CommandArgument.ToString)
                'Parm = Array.CreateInstance(GetType(ReportParameter), IIf(Request("sys") = "05", 4, 3))
                Parm = Array.CreateInstance(GetType(Microsoft.Reporting.WebForms.ReportParameter), 1)
                SetRepPm("ClmNO", False, GenArray(clmno.Text), Parm, 0)
                Me.Session.Add("Parms", Parm)
                'Response.Write("<script> window.open('http://" & System.Server.MachineName & "/unitedweb/OutPutManagement/Preview.aspx?Report=" & "/TakafulyReports/RESPALL" & "','_new'); </script>")
                Response.Write("<script> window.open('http://" & Server.MachineName & "/Takafuly/OutPutManagement/Preview.aspx?Report=" & "/TakafulyReports/RESP" & "','_new'); </script>")

        End Select
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click

    End Sub

End Class