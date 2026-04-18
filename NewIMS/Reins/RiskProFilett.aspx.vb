Imports Microsoft.Reporting.WebForms

Partial Class Reinsurance_RiskProFilett
    Inherits Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Parm = Array.CreateInstance(GetType(SqlClient.SqlParameter), 1)
        'SetPm("@table", DbType.String, "RiskFile", Parm, 0)
        'ExecSql(CallSP("deletetable", Conn, Parm))
        ExecSql("TRUNCATE TABLE RiskFile")
        Dim message As String = ""
        Dim Systems = "("
        Dim Systemtitle = "("
        For Each item As ListItem In TreatyType.Items

            If item.Selected Then
                message += item.Text & " " + item.Value & "\n"
                Systems += "'" + item.Value + "'" + ","
                Systemtitle += Trim(item.Text) + "- "
            End If
        Next
        Systems = Mid(Systems, 1, Len(Systems) - 1) + ")"
        Systemtitle = Trim(Sys.SelectedItem.Text) + " " + Systemtitle + ")"
        'Select Case TreatyType.SelectedValue
        '    Case "04" : Systems = "('04','05')"
        '    Case "06" : Systems = "('06','26')"
        '    Case "08" : Systems = "('08','10','11','12','13','18')"
        '    Case "14" : Systems = "('14','15','16')"
        '    Case "22" : Systems = "('22','23')"
        '    Case "21" : Systems = "('21')"
        'End Select
        For i As Integer = Val(SumInsF.Text) To Val(SumInsT.Text) Step Val(SumInsBy.Text)
            'On Error Resume Next
            ExecSql("delete netprm where pold= CONVERT(DATETIME, '1900-01-01 00:00:00', 102) and len(polno)=16")
            ExecSql("INSERT INTO [RiskFile]" _
               & " ([From],[To],[SumIns],[Primum],[RiskNo],[PrimumP],[Amount],[Qs],[FirsSup],[SecondSup],[Elective],[LineSlip],[SpRet],[Tp])" _
               & " select " & i & "," & i + Val(SumInsBy.Text) - 1 & ",isnull(sum(InsAMT*Exc),0),isnull(sum(Net*Exc),0),count(Net) As Count,0,isnull(sum(Amount*Exc),0)," _
               & " isnull(Sum(QS*Exc),0),isnull(Sum(FirsSup*Exc),0),isnull(sum(SecondSup*Exc),0),isnull(Sum(Elective*Exc),0),isnull(Sum(LineSlip*Exc),0)," _
               & " isnull(Sum(SpecialRet*Exc),0),'" & TreatyType.SelectedValue & "' From NetPrm " _
               & " where len(rtrim(polno))=13 And Tp in " + Systems + "" _
               & " And PolD>=CONVERT(DATETIME,'" & Format(CDate(DFrom.Text), "yyyy-MM-dd") & " 00:00:00',102)" _
               & " and PolD<=CONVERT(DATETIME,'" & Format(CDate(DTo.Text), "yyyy-MM-dd") & " 00:00:00',102)" _
               & " and InsAmt*Exc>=" & i & " and InsAmt*Exc<=" & i + Val(SumInsBy.Text) - 1 & " ")
            '& " and Cur=" & TreatyType0.SelectedValue)
            'ExecSql("insert into RiskFile([from],[to]) values(" & i & "," & i + Val(SumInsBy.Text) - 1 & ")")
            ' & " From RiskFile where " & i & " Not in (select [From] from RiskFile )")
        Next
        Parm = Array.CreateInstance(GetType(ReportParameter), 5)
        SetRepPm("Dfrom", False, GenArray(SumInsF.Text), Parm, 0)
        SetRepPm("Dto", False, GenArray(SumInsT.Text), Parm, 1)
        SetRepPm("InsType", False, GenArray(Systemtitle), Parm, 2)
        SetRepPm("DateFrom", False, GenArray(DFrom.Text), Parm, 3)
        SetRepPm("DateTo", False, GenArray(DTo.Text), Parm, 4)
        Me.Session.Add("Parms", Parm)
        Response.Write("<script> window.open('../OutPutManagement/Preview.aspx?Report=/UnitedReports/RiskProFile','_new'); </script>")
    End Sub

    Protected Sub Submit(ByVal sender As Object, ByVal e As EventArgs)
        Dim message As String = ""
        Dim systems = "("
        For Each item As ListItem In TreatyType.Items

            If item.Selected Then
                message += item.Text & " " + item.Value & "\n"
                systems += "'" + item.Value + "'" + ","
            End If
        Next
        systems = Mid(systems, 1, Len(systems) - 1) + ")"
        ClientScript.RegisterClientScriptBlock(Me.[GetType](), "alert", "alert('" & message & "');", True)
    End Sub

End Class