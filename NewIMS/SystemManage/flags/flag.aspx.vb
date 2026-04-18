Imports System.Data.SqlClient

Partial Public Class Flag
    Inherits Page

    Private Sel As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsNothing(Me.Request("extra")) Then
            Sel = " and (" + Request("extra").Replace(";", " tp= ") + ")"
            Sel = Sel.Replace("or)", ")")
        End If
        SetLists()
        If Not IsNothing(Extras.SelectedItem) Then
            SqlDataSource1.ConnectionString = Conn.ConnectionString
            SqlDataSource1.SelectCommand = "SELECT tpno,tpname FROM EXTRAINFO WHERE tp='" & Extras.Value & "' order by TpNo desc"
            SqlDataSource1.SelectCommandType = SqlDataSourceCommandType.Text
            SqlDataSource1.Select(DataSourceSelectArguments.Empty)
            GridView1.DataBind()
        Else

        End If

    End Sub

    Private Sub SetLists()
        'MarBy.DataSource = RecSet("select descRip,tp from EXTRAcat where len(descrip)>0 " & Sel, Conn)
        'MarBy.DataSource = RecSet("select descRip,tp from EXTRACAT where len(descrip)>0 " & Sel & " ORDER BY Descrip", Conn)
        Extras.DataSource = RecSet("select descRip,tp from EXTRACAT where len(descrip)>0 " & Sel & " ORDER BY Descrip")
        Extras.DataBind()
    End Sub

    Protected Sub Extras_SelectedIndexChanged(sender As Object, e As EventArgs)
        Label2.Visible = False
        Label3.Visible = False
        SqlDataSource1.ConnectionString = Conn.ConnectionString
        SqlDataSource1.SelectCommand = "SELECT tpno,tpname" & " FROM EXTRAINFO WHERE tp='" & Extras.Value & "' order by TpNo desc"
        SqlDataSource1.SelectCommandType = SqlDataSourceCommandType.Text
        SqlDataSource1.Select(DataSourceSelectArguments.Empty)
        GridView1.DataBind()
        If GridView1.Rows.Count = 0 Then
            TextBox1.Visible = True
            Button2.Visible = True
            TextBox1.Focus()
        Else
            TextBox1.Visible = False
            Button2.Visible = False
        End If
        GridView1.DataBind()
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Select Case e.CommandName
            Case "addnew"
                TextBox1.Visible = True
                'If MarBy.SelectedRowValues(1) = "Ships" Then
                TextBox2.Visible = True
                TextBox3.Visible = True
                'End If
                Button2.Visible = True
                TextBox1.Focus()
            Case Else
                Exit Select
        End Select
        If Extras.TextField = "”›‰" Then
            TextBox4.Visible = True
            TextBox5.Visible = True
            Label2.Visible = True
            Label3.Visible = True
            Label4.Visible = True
            Label5.Visible = True
        End If
        GridView1.DataBind()
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim extra, ShipCat, extra1 As New DataSet

        Dim dbadapter = New SqlDataAdapter("select Tpname from Extrainfo where TpName='" & TextBox1.Text & "' and TP='" & Extras.Value & "'", Conn)

        dbadapter.Fill(extra)
        If extra.Tables(0).Rows.Count = 0 Then
            Dim dbadapter1 = New SqlDataAdapter("select max(TpNo) from Extrainfo where TP='" & Extras.Value & "'", Conn)

            dbadapter1.Fill(extra1)
            Dim LastN As Long
            If extra1.Tables(0).Rows.Item(0).IsNull(0) Then
                LastN = 1
            Else
                LastN = extra1.Tables(0).Rows(0)(0) + 1
            End If
            If Extras.TextField = "”›‰" Then
                ExecConn("insert into ExtraInfo(tp,TpNo,TpName,value,Accessor,Type,Part) values('" _
          & Extras.Value & "'," _
          & LastN & ",'" _
          & TextBox1.Text & "'," _
          & TextBox2.Text & ",'" _
          & TextBox3.Text & "','" & TextBox4.Text & "','" & TextBox5.Text & "')", Conn)
                Dim dbadapter2 = New SqlDataAdapter("select max(ShipNo) from ShipFile", Conn)
                dbadapter2.Fill(ShipCat)
                Dim lastNoShip As Long
                If ShipCat.Tables(0).Rows.Item(0).IsNull(0) Then
                    lastNoShip = 1
                Else
                    lastNoShip = ShipCat.Tables(0).Rows(0)(0) + 1
                End If
                ExecConn("insert into ShipFile(ShipNo,ShipName,MadeYear,Nation,Material,Type) values(" _
         & lastNoShip & ",'" _
         & TextBox1.Text & "'," _
         & TextBox2.Text & ",'" _
         & TextBox3.Text & "','" & TextBox4.Text & "','" & TextBox5.Text & "')", Conn)

                SqlDataSource1.Select(DataSourceSelectArguments.Empty)
                GridView1.DataBind()
                TextBox1.Visible = False
                TextBox2.Visible = False
                TextBox3.Visible = False
                TextBox4.Visible = False
                TextBox5.Visible = False
                Button2.Visible = False
                TextBox1.Focus()
            Else
                ExecConn("insert into ExtraInfo(tp,TpNo,TpName,value,Accessor,Type,Part) values('" _
          & Extras.Value & "'," _
          & LastN & ",'" _
          & TextBox1.Text & "'," _
          & TextBox2.Text & ",'" _
          & TextBox3.Text & "','" & TextBox4.Text & "','" & TextBox5.Text & "')", Conn)
                SqlDataSource1.Select(DataSourceSelectArguments.Empty)
                GridView1.DataBind()
                TextBox1.Visible = False
                TextBox2.Visible = False
                TextBox3.Visible = False
                TextBox4.Visible = False
                TextBox5.Visible = False
                Button2.Visible = False
                TextBox1.Focus()
            End If
        Else
            'MsgBox1.alert(" ·«Ì„ﬂ‰  ﬂ—«— ‰›” «·»Ì«‰ ›Ì ‰›” «· ’‰Ì›")
        End If
        Label2.Visible = False
        Label3.Visible = False
        Label4.Visible = False
        Label5.Visible = False
        GridView1.DataBind()
    End Sub

End Class