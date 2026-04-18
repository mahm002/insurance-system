Imports System.Data.SqlClient
Imports DevExpress.Web
Imports Microsoft.Reporting.WebForms

Public Class SettlmentsPay
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = CType(Session("UserInfo"), List(Of String))
        If myList Is Nothing Then
            'ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('SystemManage/LogIn.aspx','_self'); </script>")
            FormsAuthentication.RedirectToLoginPage()
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 3)
        End If
        ASPxGridView1.GroupBy(ASPxGridView1.Columns("SysName"))
        ASPxGridView1.ExpandAll()
    End Sub

    Private Sub ASPxGridView1_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs) Handles ASPxGridView1.CustomButtonCallback
        Select Case e.ButtonID
            Case "ApprovePay"
                Dim TNo = ASPxGridView1.GetRowValues(e.VisibleIndex, "No").ToString()
                Dim ClmNo = ASPxGridView1.GetRowValues(e.VisibleIndex, "ClmNo").ToString().Trim
                Dim Sys = ASPxGridView1.GetRowValues(e.VisibleIndex, "SysName").ToString()

                ASPxGridView1.JSProperties("cpRowIndex") = e.VisibleIndex
                ASPxGridView1.JSProperties("cpMyAttribute") = "Issuance"
                ASPxGridView1.JSProperties("cpCust") = "تأكيد إصدار إذن صرف للتسوية رقم " & TNo & " لحادث " & Sys & " رقم " & ClmNo & " "
                ASPxGridView1.JSProperties("cpShowIssueConfirmBox") = True

            Case "Print"
                Dim Report = ReportsPath & "Settelement"

                Dim TNo = ASPxGridView1.GetRowValues(e.VisibleIndex, "No").ToString()
                Dim ClmNo = ASPxGridView1.GetRowValues(e.VisibleIndex, "ClmNo").ToString().Trim
                Dim Sys = ASPxGridView1.GetRowValues(e.VisibleIndex, "SysName").ToString()
                Dim Tpid = ASPxGridView1.GetRowValues(e.VisibleIndex, "TPID").ToString()

                'Dim p As ReportParameter() = New ReportParameter(3) {}

                'p.SetValue(New ReportParameter("ClmNo", ClmNo, False), 0)
                'p.SetValue(New ReportParameter("No", TNo, False), 1)
                'p.SetValue(New ReportParameter("TPID", Tpid, False), 2)
                'p.SetValue(New ReportParameter("Sys", Sys, False), 3)

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("ClmNo", ClmNo, False),
                    New ReportParameter("No", TNo, False),
                    New ReportParameter("TPID", Tpid, False),
                    New ReportParameter("Sys", Sys, False)
                }
                Session.Add("Parms", P)

                ASPxGridView1.JSProperties("cpMyAttribute") = "PRINT"
                ASPxGridView1.JSProperties("cpResult") = ClmNo & " - طباعة مذكرة تسوية للحادث - "
                ASPxGridView1.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
            Case Else
                Exit Select
        End Select
    End Sub

    Private Sub ASPxGridView1_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles ASPxGridView1.CustomCallback
        Select Case e.Parameters
            Case "Issue"
                Dim TNo = ASPxGridView1.GetRowValues(ASPxGridView1.FocusedRowIndex, "No").ToString()
                Dim ClmNo = ASPxGridView1.GetRowValues(ASPxGridView1.FocusedRowIndex, "ClmNo").ToString().Trim
                Dim Sys = ASPxGridView1.GetRowValues(ASPxGridView1.FocusedRowIndex, "SysName").ToString()
                Dim Net = CDbl(ASPxGridView1.GetRowValues(ASPxGridView1.FocusedRowIndex, "Net"))
                Dim PayTo = ASPxGridView1.GetRowValues(ASPxGridView1.FocusedRowIndex, "PayTo").ToString()
                Dim UserName = ASPxGridView1.GetRowValues(ASPxGridView1.FocusedRowIndex, "UserName").ToString()
                Dim Acc = ASPxGridView1.GetRowValues(ASPxGridView1.FocusedRowIndex, "AccountNo").ToString()
                Dim Tpid = ASPxGridView1.GetRowValues(ASPxGridView1.FocusedRowIndex, "TPID").ToString()
                Dim SubIns = ASPxGridView1.GetRowValues(ASPxGridView1.FocusedRowIndex, "SubIns").ToString()
                'Parm = Array.CreateInstance(GetType(SqlParameter), 2)

                'SetPm("@TP", DbType.String, "3", Parm, 0)
                'SetPm("@Year", DbType.String, Right(Year(Today.Date).ToString, 2), Parm, 1)

                Parm = Array.CreateInstance(GetType(SqlParameter), 3)
                SetPm("@TP", DbType.String, "3", Parm, 0)
                SetPm("@Year", DbType.String, Right(Year(Today.Date).ToString, 2), Parm, 1)
                SetPm("@Br", DbType.String, Session("Branch"), Parm, 2)

                Dim Dly As String = CallSP("LastDailyNo", Conn, Parm)

                'Dim Str As String = ASPxGridView1.GetRowValues(ASPxGridView1.FocusedRowIndex, "SysName").ToString()
                Dim Str As String = " قيمة سداد مذكرة تسوية رقم " & TNo & " لحادث رقم " & ClmNo & " حسب المرفقات طيه / " & Sys & ""

                ExecConn("INSERT INTO MainJournal ([DAILYNUM],[DAILYDTE] ,[DailyTyp] ,[ANALSNUM] ,[Comment] " _
                                            & ",[Currency] ,[Exchange] ,[CurUser] ,[MoveRef],[DailyChk],[PayedValue],[PayedFor],[Branch],[SubBranch])" _
                                            & " VALUES ('" & Dly & "','" & Format(Today.Date, "yyyy/MM/dd") & "', " _
                                            & 3 & ", 'AC','" & Str & "', " & 1 & "," & 1 & "," _
                                            & "'" & UserName.Trim & "','" & (ClmNo + "/" + TNo) & "'," & 0 & "," & Net & ",'" & PayTo.Trim & "','" & MyBase.Session("Branch") & "','" & MyBase.Session("Branch") & "')", Conn)

                ExecConn("INSERT INTO [dbo].[Journal]([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser],[Branch],[SubBranch]) VALUES ('" & Dly.ToString & "',3, '" & Acc & "'," & CDbl(Net) & ",0,'" & MyBase.Session("User") & "','" & MyBase.Session("Branch") & "','" & MyBase.Session("Branch") & "')", Conn)

                ExecConn("Update MainSattelement Set DAILYNUM='" & Dly & "', DAILYDTE='" & Format(Today.Date, "yyyy/MM/dd") & "' WHERE ClmNo='" & ClmNo & "' and No=" & TNo & " And TPID=" & Tpid & "", Conn)

                Dim TrNo, Trno1 As Integer
                Dim Tr, Tr1 As New DataSet

                Dim dbadapter = New SqlDataAdapter("Select max(TPID) From ThirdParty where ClmNo='" & ClmNo & "' ", Conn)
                dbadapter.Fill(Tr)
                If IsDBNull(Tr.Tables(0).Rows(0)(0)) Then
                    TrNo = 1
                Else
                    TrNo = Tr.Tables(0).Rows(0)(0) + 1
                End If

                Dim dbadapter1 = New SqlDataAdapter("Select max(Sn),PolNo From Estimation where ClmNo='" & ClmNo & "' Group By PolNo", Conn)
                dbadapter1.Fill(Tr1)
                If IsDBNull(Tr1.Tables(0).Rows(0)(0)) Then
                    Trno1 = 1
                Else
                    Trno1 = Tr1.Tables(0).Rows(0)(0) + 1
                End If

                ExecConn("INSERT INTO Estimation (Sn,TPID,ClmNo,PolNo,Value,Date) VALUES (" & Trno1 & "," & IIf(Tpid = 0, 0, TrNo) & ",'" & ClmNo & "','" & Tr1.Tables(0).Rows.Item(0).Item("PolNo") & "', " & 0 & ",CONVERT(DATETIME,'" & Format(Today.Date, "yyyy/MM/dd") & " 00:00:00',102))", Conn)
                Dim dbadapter11 = New SqlDataAdapter("select distinct rtrim(MainClaimFile.ClmNo) As ClmNo, MainClaimFile.groupNo,MainClaimFile.endno,MainClaimFile.loadno, " _
                    & " RTRIM(MainClaimFile.ClmNo) + '&EndNo=' + LTRIM(MainClaimFile.EndNo) + '&LoadNo=' + LTRIM(MainClaimFile.LoadNo) + '&OrderNo=' + rtrim(PolicyFile.OrderNo) + '&Sys=' + PolicyFile.SubIns + '&Branch=' + PolicyFile.Branch + '&PolNo=' + PolicyFile.PolNo + '&GroupNo=' + LTRIM(mainclaimfile.GroupNo)  AS ClmPath,PolicyFile.SubIns As Sys " _
                    & " from MainClaimFile left outer join Estimation on MainClaimFile.ClmNo=Estimation.clmno and MainClaimFile.PolNo=Estimation.polno " _
                    & " left outer join NetPrm on NetPrm.PolNo = rtrim(MainClaimFile.PolNo) + '-' + rtrim(MainClaimFile.ClmNo) and round(Estimation.value,3)=round(netprm.Net,3) left outer join policyfile " _
                    & " on MainClaimFile.PolNo=PolicyFile.PolNo And MainClaimFile.EndNo=PolicyFile.EndNo And MainClaimFile.LoadNo=PolicyFile.LoadNo " _
                    & " Left outer join CustomerFile on CustomerFile.CustNo=policyfile.CustNo left outer join BranchInfo on mainclaimfile.Branch=BranchInfo.BranchNo " _
                    & " WHERE MainClaimFile.ClmNo='" & ClmNo & "'", Conn)

                Dim ClmData As New DataSet
                dbadapter11.Fill(ClmData)

                If SubIns = "01" Or SubIns = "OR" Or SubIns = "27" Or SubIns = "02" Or SubIns = "03" Or SubIns = "08" Or SubIns = "07" Or SubIns = "PH" Or SubIns = "04" Then
                Else
                    ASPxWebControl.RedirectOnCallback("~/Reins/DestributeClaim.aspx?ClmNo=" & ClmData.Tables(0).Rows(0)("ClmPath"))
                End If
                If Page.IsCallback Then
                    ASPxGridView1.JSProperties("cpMyAttribute") = "Edit"
                    ASPxGridView1.JSProperties("cpResult") = GetSysName(3)
                    ASPxGridView1.JSProperties("cpNewWindowUrl") = "../Finance/DailySarf.aspx?daily=" & Dly & "&Sys=3"
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If
            Case Else
                ASPxGridView1.DataBind()
                'cmbReports.DataBind()

        End Select
    End Sub

End Class