Imports DevExpress.Web
Imports Microsoft.Reporting.WebForms

Public Class RiskProFile
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))

        If myList Is Nothing Then
            'ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('SystemManage/LogIn.aspx','_self'); </script>")
            FormsAuthentication.RedirectToLoginPage()
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 4)

            TryCast(FindControlRecursive(Form, "RightPane"), ASPxPanel).FixedPosition = PanelFixedPosition.WindowLeft

            TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar).RightToLeft = DevExpress.Utils.DefaultBoolean.False

        End If
    End Sub


    Protected Sub ASPxCallback1_Callback(source As Object, e As CallbackEventArgs) Handles ASPxCallback1.Callback
        Select Case e.Parameter
            Case "CollectData"
                Dim splt = ""
                Parm = Array.CreateInstance(GetType(SqlClient.SqlParameter), 1)

                ExecConn("TRUNCATE TABLE RiskFile", Conn)

                Dim Systems = "("

                Dim strarr() As String

                strarr = SubSys.Value.Split(","c)
                'Dim splt = ""
                For Each s As String In strarr
                    splt = splt + "'" + s + "',"
                Next
                splt = Mid(splt, 1, Len(splt) - 1)

                Dim unused As String = Mid(Systems, 1, Len(Systems) - 1) + ")"
                Dim Systemtitle As String = Trim(Sys.SelectedItem.Text) + "(" + SubSys.Text + ")"

                For i As Long = SumInsF.Text To SumInsT.Text Step SumInsBy.Text
                    'On Error Resume Next
                    'ExecSql("DELETE NetPrm WHERE PolD=CONVERT(DATETIME, '1900-01-01 00:00:00', 102) and Type='Pol'")
                    'If i = 35000000 Then
                    '    i = i
                    'End If
                    ExecConn("INSERT INTO [RiskFile]" _
               & " ([From],[To],[SumIns],[Primum],[RiskNo],[PrimumP],[Retention],[Qs],[FirsSup],[SecondSup],[Elective],[LineSlip],[SpRet],[Tp],[UY],[STp])" _
               & " select " & i & "," & (i + CLng(SumInsBy.Value) - 1) & ", isnull(sum(InsAMT*Exc),0),isnull(sum(Net*Exc),0),count(Net) As Count, 0,isnull(sum(Amount*Exc),0)," _
               & " isnull(Sum(QS*Exc),0),isnull(Sum(FirsSup*Exc),0),isnull(sum(SecondSup*Exc),0),isnull(Sum(Elective*Exc),0),isnull(Sum(LineSlip*Exc),0)," _
               & " isnull(Sum(SpecialRet*Exc),0)," & Left(splt, 4) & ", right(Treaty,4), TP From NetPrm " _
               & " WHERE Type='Pol' And Tp in (" & splt & ")" _
               & " AND (PolD BETWEEN '" & Format(DFrom.Value, "yyyy/MM/dd") & "' AND '" & Format(DTo.Value, "yyyy/MM/dd") & "') " _
               & " AND (abs(InsAmt)*Exc BETWEEN " & i & " AND " & CDbl(i + CLng(SumInsBy.Text) - 1) & ") Group By right(Treaty,4), TP", Conn)
                Next
                Dim Report = ReportsPath & "RiskProfile"

                Dim P As New List(Of ReportParameter) From {
            New ReportParameter("SiFrom", CDbl(SumInsF.Value), False),
            New ReportParameter("SiTo", CDbl(SumInsT.Value), False),
            New ReportParameter("InsType", GenArray(Systemtitle), False),
            New ReportParameter("DateFrom", CDate(DFrom.Value), False),
            New ReportParameter("DateTo", CDate(DTo.Value), False)
        }
                Session.Add("Parms", P)

                ASPxCallback1.JSProperties("cpMyAttribute") = "PRINT"
                ASPxCallback1.JSProperties("cpResult") = "RiskProfile"

                ASPxCallback1.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
                ' Response.Write("<script> window.open('../Reporting/Previewer.aspx?Report=" & ReportsPath & "RiskProFile','_new'); </script>")

            Case Else
                Exit Select
        End Select
    End Sub



    'Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    '    Dim splt = ""
    '    Parm = Array.CreateInstance(GetType(SqlClient.SqlParameter), 1)

    '    ExecSql("TRUNCATE TABLE RiskFile")

    '    Dim Systems = "("

    '    Dim strarr() As String

    '    strarr = SubSys.Value.Split(","c)
    '    'Dim splt = ""
    '    For Each s As String In strarr
    '        splt = splt + "'" + s + "',"
    '    Next
    '    splt = Mid(splt, 1, Len(splt) - 1)

    '    Dim unused As String = Mid(Systems, 1, Len(Systems) - 1) + ")"
    '    Dim Systemtitle As String = Trim(Sys.SelectedItem.Text) + "(" + SubSys.Text + ")"

    '    For i As Integer = SumInsF.Value To SumInsT.Value Step CDbl(SumInsBy.Value)
    '        'On Error Resume Next
    '        ExecSql("DELETE NetPrm WHERE PolD=CONVERT(DATETIME, '1900-01-01 00:00:00', 102) and Type='Pol'")

    '        ExecSql("INSERT INTO [RiskFile]" _
    '           & " ([From],[To],[SumIns],[Primum],[RiskNo],[PrimumP],[Amount],[Qs],[FirsSup],[SecondSup],[Elective],[LineSlip],[SpRet],[Tp],[UY],[STp])" _
    '           & " select " & i & "," & i & SumInsBy.Value - 1 & ",isnull(sum(InsAMT*Exc),0),isnull(sum(Net*Exc),0),count(Net) As Count,0,isnull(sum(Amount*Exc),0)," _
    '           & " isnull(Sum(QS*Exc),0),isnull(Sum(FirsSup*Exc),0),isnull(sum(SecondSup*Exc),0),isnull(Sum(Elective*Exc),0),isnull(Sum(LineSlip*Exc),0)," _
    '           & " isnull(Sum(SpecialRet*Exc),0)," & Left(splt, 4) & ",right(Treaty,4),TP From NetPrm " _
    '           & " where Type='Pol' And Tp in (" & splt & ")" _
    '           & " And PolD>='" & Format(DFrom.Value, "yyyy/MM/dd") & "' " _
    '           & " and PolD<='" & Format(DTo.Value, "yyyy/MM/dd") & "' " _
    '           & " and abs(InsAmt)*Exc>=" & i & " and abs(InsAmt)*Exc<=" & i & SumInsBy.Value & " Group By right(Treaty,4), TP")
    '    Next

    '    Dim P As New List(Of ReportParameter) From {
    '        New ReportParameter("SiFrom", CDbl(SumInsF.Value), False),
    '        New ReportParameter("SiTo", CDbl(SumInsT.Value), False),
    '        New ReportParameter("InsType", GenArray(Systemtitle), False),
    '        New ReportParameter("DateFrom", GenArray(DFrom.Value), False),
    '        New ReportParameter("DateTo", GenArray(DTo.Value), False)
    '    }

    '    Session.Add("Parms", P)

    '    Response.Write("<script> window.open('../Reporting/Previewer.aspx?Report=" & ReportsPath & "RiskProFile','_new'); </script>")
    'End Sub

End Class