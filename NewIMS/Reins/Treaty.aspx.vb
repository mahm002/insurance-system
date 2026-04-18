Partial Class Reinsurance_Treaty
    Inherits System.Web.UI.Page
    Dim Extras As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        For i As Integer = 0 To Page.Form.Controls.Count - 1
            If TypeOf (Page.Form.Controls(i)) Is eba.Web.Combo Then
                Dim xCombo As eba.Web.Combo = Page.Form.FindControl(Page.Form.Controls(i).ID)
                Extras = Extras + ";'" + xCombo.CSSClassName + "' or "
                If xCombo.DisabledWarningMessages <> "" Then
                    ExecSql("insert into extracat(Tp,Descrip) select '" & xCombo.CSSClassName & "','" & xCombo.DisabledWarningMessages & "'" _
                    & " where '" & xCombo.CSSClassName & "' not in (select Tp From ExtraCat)")
                End If
            End If
        Next
        TreatyType.Focus()
        If Not IsPostBack Then
            TreatyType.SelectedIndex = -1
            TreatyDate.Text = Format(Now, "yyyy/MM/dd")
        End If
        Call FillCombos(Me.Page.Form, Request("sys"))
        Extras = ";'Recom' or"
        HyperLink2.NavigateUrl = "../SystemManag/flags/flag.aspx" & "?extra=" & Extras
        HyperLink1.NavigateUrl = "../Reinsurance/Default.aspx?Sys=R1"
        'TreatyNo.Text = TreatyType.SelectedValue + "01" + Year(Now)
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Val(Capacity.Text) = Val(Ret.Text) + Val(QS.Text) + Val(FSup.Text) Then
            ExecSql("delete from TRREGFLE  where TreatyNo='" & TreatyNo.Text & "'; INSERT INTO [MainDB].[dbo].[TRREGFLE]([TreatyNo],[Descrip],[Acctype],[Portfolio],[ReserveR],[TRSYSDTE],[TRINSDTE],[TREXPDTE],[TRCAPCTY],[TRRETAMT],[TRQSRAMT]" _
        & ",[TRQSRCOM],[TR1STAMT],[TR1STCOM],[TR2STAMT],[TR2STCOM],[TRLQSRCOM],[TRL1STCOM],[TRL2STCOM],[TRWQSRCOM],[TRW1STCOM],[InterestRRes],[TRLSAMT],[TRLSCOMM])" _
        & " VALUES('" _
        & TreatyNo.Text & "','" _
        & Descrip.Text & "'," _
        & AccType.SelectedValue & "," _
        & PortFolio.SelectedValue & "," _
        & Val(ReserveR.Text.Replace("%", "")) & "," _
        & "CONVERT(DATETIME,'" & Format(CDate(IIf(IsDate(TreatyDate.Text), TreatyDate.Text, Today.Date)), "yyyy-MM-dd") & " 00:00:00',102)" & "," _
        & "CONVERT(DATETIME,'" & Format(CDate(TReatyFrom.Text), "yyyy-MM-dd") & " " & Format(Now, "hh:mm") & "',102)," _
        & "CONVERT(DATETIME,'" & Format(CDate(TreatyTo.Text), "yyyy-MM-dd") & " " & Format(Now, "hh:mm") & "',102)," _
        & Val(Capacity.Text) & "," _
        & Val(Ret.Text) & "," _
        & Val(QS.Text) & "," _
        & Val(QSCom.Text.Replace("%", "")) & "," _
        & Val(FSup.Text) & "," _
        & Val(FSupCom.Text.Replace("%", "")) & "," _
        & Val(SSup.Text) & "," _
        & Val(SSupCom.Text.Replace("%", "")) & "," & Val(LQSCom.Text.Replace("%", "")) & "," & Val(LFSupCom.Text.Replace("%", "")) & "," _
        & Val(LSSupCom.Text.Replace("%", "")) & "," & Val(WQSCom.Text.Replace("%", "")) & "," & Val(WFSupCom.Text.Replace("%", "")) & "," & Val(InterestRRes.Text.Replace("%", "")) & "," & Val(LS.Text) & "," & Val(LsCom.Text.Replace("%", "")) & ")")
        Else
            MsgBob(Me, "    Œÿ√ ›Ì ÕœÊœ «·« ›«ﬁÌ…    ")
            Capacity.Focus()
        End If
    End Sub

    Protected Sub TreatyType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TreatyType.SelectedIndexChanged
        Select Case TreatyType.SelectedValue
            Case "GA"
                TreatyNo.Text = "0801" & Request("Year")
            Case "Bu"
                TreatyNo.Text = "2101" & Request("Year")
            Case "0402"
                TreatyNo.Text = "0402" & Request("Year")
            Case Else
                TreatyNo.Text = TreatyType.SelectedValue & "01" & Request("Year")
        End Select

        Dim Treaty As New Data.DataSet

        Dim dbadapter = New Data.SqlClient.SqlDataAdapter("Select * from TRREGFLE where TreatyNo='" & TreatyNo.Text & "'", Conn)
        dbadapter.Fill(Treaty)
        'Treaty = RecSet("select * from TRREGFLE where TreatyNo='" & TreatyNo.Text & "'", Conn)
        If Treaty.Tables(0).Rows.Count <> 0 Then
            SqlDataSource1.SelectParameters("Treaty").DefaultValue = TreatyNo.Text
            GridView1.DataBind()
            TreatyDate.Text = Format(Treaty.Tables(0).Rows(0)("TRSYSDTE"), "yyyy/MM/dd")
            Descrip.Text = IIf(IsDBNull(Treaty.Tables(0).Rows(0)("Descrip")), "", Treaty.Tables(0).Rows(0)("Descrip"))
            AccType.SelectedValue = IIf(IsDBNull(Treaty.Tables(0).Rows(0)("AccType")), 1, Treaty.Tables(0).Rows(0)("Acctype"))
            PortFolio.SelectedValue = IIf(IsDBNull(Treaty.Tables(0).Rows(0)("PortFolio")), 1, Treaty.Tables(0).Rows(0)("PortFolio"))
            TReatyFrom.Text = Format(Treaty.Tables(0).Rows(0)("TRINSDTE"), "yyyy/MM/dd")
            TreatyTo.Text = Format(Treaty.Tables(0).Rows(0)("TREXPDTE"), "yyyy/MM/dd")
            Capacity.Text = Treaty.Tables(0).Rows(0)("TRCAPCTY")
            Ret.Text = Treaty.Tables(0).Rows(0)("TRRETAMT")
            QS.Text = Treaty.Tables(0).Rows(0)("TRQSRAMT")
            QSCom.Text = Treaty.Tables(0).Rows(0)("TRQSRCOM").ToString + " %"
            FSup.Text = Treaty.Tables(0).Rows(0)("TR1STAMT")
            FSupCom.Text = Treaty.Tables(0).Rows(0)("TR1STCOM").ToString + " %"
            SSup.Text = Treaty.Tables(0).Rows(0)("TR2STAMT")
            SSupCom.Text = Treaty.Tables(0).Rows(0)("TR2STCOM").ToString + " %"
            LQSCom.Text = Treaty.Tables(0).Rows(0)("TRLQSRCOM").ToString + " %"
            LFSupCom.Text = Treaty.Tables(0).Rows(0)("TRL1STCOM").ToString + " %"
            LSSupCom.Text = Treaty.Tables(0).Rows(0)("TRL2STCOM").ToString + " %"
            WQSCom.Text = Treaty.Tables(0).Rows(0)("TRWQSRCOM").ToString + " %"
            WFSupCom.Text = Treaty.Tables(0).Rows(0)("TRW1STCOM").ToString + " %"
            ReserveR.Text = Treaty.Tables(0).Rows(0)("ReserveR").ToString + " %"
            LsCom.Text = IIf(IsDBNull(Treaty.Tables(0).Rows(0)("TRLSCOMM")), "0 %", Treaty.Tables(0).Rows(0)("TRLSCOMM").ToString + " %")
            LS.Text = Treaty.Tables(0).Rows(0)("TRLSAMT")
            InterestRRes.Text = Treaty.Tables(0).Rows(0)("InterestRRes").ToString + " %"
        Else
            TreatyDate.Text = ""
            TReatyFrom.Text = ""
            TreatyTo.Text = ""
            Descrip.Text = ""
            Capacity.Text = ""
            Ret.Text = ""
            QS.Text = ""
            QSCom.Text = ""
            FSup.Text = ""
            FSupCom.Text = ""
            SSup.Text = ""
            SSupCom.Text = ""
            LQSCom.Text = ""
            LFSupCom.Text = ""
            LSSupCom.Text = ""
            WQSCom.Text = ""
            WFSupCom.Text = ""
            InterestRRes.Text = ""
            LsCom.Text = ""
            LS.Text = ""
            ReserveR.Text = ""
        End If
        SqlDataSource1.SelectParameters("Treaty").DefaultValue = TreatyNo.Text
        GridView1.DataBind()
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Select Case e.CommandName
            Case "DelLine"
                GridView1.SelectedIndex = Val(e.CommandArgument.ToString)
                ExecSql("delete from TRSECFLE where right(rtrim(TreatyNo),4)='" & Right(TreatyNo.Text, 4) & "' and TRREINSCO=" & GridView1.Rows(e.CommandArgument.ToString).Cells(1).Text)
                'ExecSql("update dailytab2 set GroupNo=GroupNo-1 where DailyNum='" & DailyNum.Text & "' and GroupNo>" & GridView1.Rows(e.Command Argument.ToString).Cells(1).Text)
                SqlDataSource1.SelectParameters("Treaty").DefaultValue = TreatyNo.Text
                GridView1.DataBind()
        End Select
    End Sub

    Protected Sub ReCom_GetPage(ByVal sender As Object, ByVal e As eba.Web.ComboGetPageEventArgs) Handles ReCom.GetPage
        Dim DB As New Data.DataSet

        Dim dbadapter = New Data.SqlClient.SqlDataAdapter("SELECT TOP " & e.PageSize & " TpName,TpNo FROM ExtraInfo WHERE tp='" & sender.CssClassName & "' and TpName > '" & e.LastString & "' AND TpName LIKE '" & e.SearchSubstring & "%' ORDER BY TpName", Conn)
        dbadapter.Fill(DB)
        e.NextPage = DB
        'e.NextPage = RecSet("SELECT TOP " & e.PageSize & " TpName,TpNo FROM ExtraInfo WHERE tp='" & sender.CssClassName & "' and TpName > '" & e.LastString & "' AND TpName LIKE '" & e.SearchSubstring & "%' ORDER BY TpName", Conn)
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        If TreatyNo.Text <> "" Then
            If ReCom.TextValue <> "" Then
                ExecSql("insert into TRSECFLE(TreatyNo,TRREINSCO,TRSHACOM,Isleader) values('" _
                        & Right(Trim(TreatyNo.Text), 4) & "'," _
                        & ReCom.SelectedRowValues(1) & "," _
                        & Val(Share.Text) & ",'" & Leader.Checked & "')")
                SqlDataSource1.SelectParameters("Treaty").DefaultValue = TreatyNo.Text
                GridView1.DataBind()
                Dim AllShare As New Data.DataSet

                Dim dbadapter = New Data.SqlClient.SqlDataAdapter("select sum(TRSHACOM) from TRSECFLE where right(rtrim(TreatyNo),4)='" & Request("Year") & "'", Conn)
                dbadapter.Fill(AllShare)
                AllShare = RecSet("select sum(TRSHACOM) from TRSECFLE where right(rtrim(TreatyNo),4)='" & Request("Year") & "'", Conn)
                If Not AllShare.Tables(0).Rows(0).IsNull(0) Then
                    If AllShare.Tables(0).Rows(0)(0) = 100 Then
                        'Button1.Visible = True
                    End If
                End If
            Else
                MsgBob(Me, "    ÌÃ»  ÕœÌœ «·„⁄Ìœ Ê‰”»… „‘«—ﬂ Â    ")
            End If
        Else
            MsgBob(Me, "    ÌÃ»  ÕœÌœ —ﬁ„ «·≈ ›«ﬁÌ…    ")
        End If
    End Sub

    Protected Sub SSupCom_TextChanged(sender As Object, e As EventArgs) Handles SSupCom.TextChanged
        Dim AllShare As New Data.DataSet
        Dim dbadapter = New Data.SqlClient.SqlDataAdapter("select sum(TRSHACOM) from TRSECFLE where right(rtrim(TreatyNo),4)='" & Request("Year") & "'", Conn)
        dbadapter.Fill(AllShare)
        AllShare = RecSet("select sum(TRSHACOM) from TRSECFLE where right(rtrim(TreatyNo),4)='" & Request("Year") & "'", Conn)
        If Not AllShare.Tables(0).Rows(0).IsNull(0) Then
            If AllShare.Tables(0).Rows(0)(0) = 100 Then
                Button1.Visible = True
            End If
        End If
    End Sub
End Class
