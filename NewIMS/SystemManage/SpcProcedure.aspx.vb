Imports System.Data.SqlClient
Imports System.Web.UI
Imports DevExpress.Web

Partial Class SystemManage_SpcProcedure
    Inherits Page
    'Dim Lo() As String
    'Dim myList = CType(Session("UserInfo"), List(Of String))
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        Dim myList = DirectCast(Session("UserInfo"), List(Of String))
        Dim TT As Integer = Mid(myList(1), InStr(1, myList(1), Request("Sys")) + 3, 1)
        If Not IsPostBack And Not Callback.IsCallback Then
            Dim PolicyRec As New DataSet
            If Request("EndNo") = 0 Then
                Select Case Request("sys")
                    Case "MC", "MB", "MA", "ER", "CR" : Dim dbadapter = New SqlDataAdapter("Select PolNo,sum(NETPRM),sum(TAXPRM),sum(CONPRM),sum(STMPRM),sum(ISSPRM),sum(EXTPRM),sum(TOTPRM),sum(lastnet) from PolicyFile Group By PolNo Having PolNo='" & Request("PolNo") & "'", Conn)
                        dbadapter.Fill(PolicyRec)
                        'PolicyRec = RecSet("Select PolNo,sum(NETPRM),sum(TAXPRM),sum(CONPRM),sum(STMPRM),sum(ISSPRM),sum(TOTPRM),sum(lastnet) from PolicyFile Group By PolNo Having PolNo='" & Request("PolNo") & "'", Conn)
                    Case "OC" : Dim dbadapter1 = New SqlDataAdapter("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' and loadNo=" & Request("LoadNo"), Conn)
                        dbadapter1.Fill(PolicyRec)
                        'PolicyRec = RecSet("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' and loadNo=" & Request("LoadNo"), Conn)

                    Case Else : Dim dbadapter2 = New SqlDataAdapter("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "'", Conn)
                        dbadapter2.Fill(PolicyRec)
                        'PolicyRec = RecSet("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "'", Conn)
                End Select
            Else
                Dim dbadapter3 = New SqlDataAdapter("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' AND EndNo=" & Request("EndNo") & "", Conn)
                dbadapter3.Fill(PolicyRec)
                'PolicyRec = RecSet("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' AND EndNo=" & Request("EndNo") & "", Conn)
            End If
            PolNo.Text = PolicyRec.Tables(0).Rows(0)("PolNo")
            NETPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(1), "###0.000")
            TAXPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(2), "###0.000")
            CONPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(3), "###0.000")
            STMPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(4), "###0.000")
            ISSPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(5), "###0.000")
            TOTPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(7), "###0.000")
            lastNet.Text = Format(PolicyRec.Tables(0).Rows(0)(8), "###0.000")
            Dim PolicyRec1 As New DataSet
            Dim dbadapter4 = New SqlDataAdapter("Select * from PolicyFile,CustomerFile where PolicyFile.CustNo=CustomerFile.CustNo and PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") _
                  & " and LoadNo=" & Request("LoadNo"), Conn)
            dbadapter4.Fill(PolicyRec1)
            'PolicyRec = RecSet("Select * from PolicyFile,CustomerFile where PolicyFile.CustNo=CustomerFile.CustNo and PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") _
            '& " and LoadNo=" & Request("LoadNo"), Conn)
            EndNo.Text = PolicyRec1.Tables(0).Rows(0)("EndNo")
            LoadNo.Text = PolicyRec1.Tables(0).Rows(0)("LoadNo")
            CoverTo.Text = Format(PolicyRec1.Tables(0).Rows(0)("CoverTo"), "yyyy/MM/dd")
            CoverFrom.Text = Format(PolicyRec1.Tables(0).Rows(0)("CoverFrom"), "yyyy/MM/dd")
            CustName.Text = PolicyRec1.Tables(0).Rows(0)("custname")
            CustNo.Text = PolicyRec1.Tables(0).Rows(0)("CustNo")
            PayType.Text = PolicyRec1.Tables(0).Rows(0)("PayType")
            Currency.SelectedIndex = PolicyRec1.Tables(0).Rows(0)("Currency") - 1
            Exchange.Text = PolicyRec1.Tables(0).Rows(0)("ExcRate")
            IssuDate.Text = Format(PolicyRec1.Tables(0).Rows(0)("IssuDate"), "yyyy/MM/dd")
            SerialNo.Text = IIf(PolicyRec1.Tables(0).Rows(0)("SerialNo") Is Nothing, 0, PolicyRec1.Tables(0).Rows(0)("SerialNo"))
            Br.Text = PolicyRec1.Tables(0).Rows(0)("Branch")
            'Or (Request("Sys") = "01" And IsSameMonth(Request("OrderNo"), Request("EndNo"),Request("LoadNo"),Request("Sys")))
            If (Request("Sys") = "OR" And DateDiff(DateInterval.Minute, PolicyRec1.Tables(0).Rows(0)("IssuTime"), Now()) > 60) Then
                'Response.Write("<script>alert(' «·ÊÀÌﬁ… «·„ÿ·Ê» «· ⁄œÌ· ⁄·ÌÂ«  Ã«Ê“  30 œﬁÌﬁ… „‰ ⁄·Ï ≈’œ«—Â« ·« Ì„ﬂ‰ ««·«” —Ã«⁄ √Ê «·«·€«¡ ');</script>")

                If CType(Mid(myList(1), InStr(1, myList(1), Request("Sys")) + 3, 1), Short) >= 5 Then
                    GoTo 10
                Else
                    MsgBob(Me, " 60 MIN LIMIT EXEEDED ")
                    Cancel.Visible = False
                    Refund.Visible = False
                    'Exit Sub
                End If

            Else
10:             If (Month(PolicyRec1.Tables(0).Rows(0)("IssuDate")) = Month(Today.Date) And Year(PolicyRec1.Tables(0).Rows(0)("IssuDate")) = Year(Today.Date)) Or IsHeadQuarter(Session("Branch")) Then

                    If IsHeadQuarter(Session("Branch")) Or Session("Branch") = Trim(Br.Text) Then

                    Else
                        If GetMainBranch(Trim(Br.Text)) <> Session("Branch") Then
                            Cancel.Enabled = False
                            Refund.Visible = False
                            Exit Sub
                        Else

                        End If
                    End If
                Else
                    If IsHeadQuarter(Session("Branch")) Then

                    Else
                        Cancel.Enabled = False
                        Refund.Visible = False
                        Exit Sub
                    End If
                End If
                '10:             If Session("Branch") <> Trim(Br.Text) And Not IsHeadQuarter(Session("Branch")) Then
                '                    If Month(PolicyRec1.Tables(0).Rows(0)("IssuDate")) = Month(Today.Date) And Year(PolicyRec1.Tables(0).Rows(0)("IssuDate")) = Year(Today.Date) Or Not IsHeadQuarter(Session("Branch")) Then
                '                        Cancel.Enabled = False
                '                        Refund.Visible = False
                '                        Exit Sub
                '                    Else
                '                        Response.Write("<script>alert(' «·ÊÀÌﬁ… «·„ÿ·Ê» «· ⁄œÌ· ⁄·ÌÂ« ·„  ’œ— »‰›” «·‘Â— ·« Ì„ﬂ‰ ≈Ã—«¡ ≈·€«¡ 100% Ì—ÃÏ ≈÷«›… „·Õﬁ ›Ì Â–Â «·Õ«·… ');</script>")
                '                        Cancel.Enabled = False
                '                    End If
                '                    'Val(Mid(myList(1), InStr(myList(1), Request("Sys")) + 3, 1)) < 3 And
                '                    'Month(PolicyRec1.Tables(0).Rows(0)("IssuDate")) <> Month(Today.Date) And Year(PolicyRec1.Tables(0).Rows(0)("IssuDate")) <> Year(Today.Date) Or 
                '                    'IsHeadQuarter(Session("Branch"))
                '                    Response.Write("<script>alert(' «·ÊÀÌﬁ… «·„ÿ·Ê» «· ⁄œÌ· ⁄·ÌÂ« ·„  ’œ— »‰›” «·‘Â— ·« Ì„ﬂ‰ ≈Ã—«¡ ≈·€«¡ 100% Ì—ÃÏ ≈÷«›… „·Õﬁ ›Ì Â–Â «·Õ«·… ');</script>")
                '                    Cancel.Enabled = False
                '                    Refund.Visible = False
                '                    Exit Sub
                '                End If
            End If
        End If
    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback

        Dim PolData As New DataSet
        Dim adptr = New SqlDataAdapter("Select * from PolicyFile,CustomerFile where PolicyFile.CustNo=CustomerFile.CustNo and PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") _
                  & " and LoadNo=" & Request("LoadNo"), Conn)
        adptr.Fill(PolData)

        Dim splited = e.Parameter.Split("|")

        Select Case splited(0)

            Case "Cancel100"

                NETPRM.Text = Format(CDbl(PolData.Tables(0).Rows(0)("NETPRM")) * -1, "###,0.000")
                NETPRM.ClientEnabled = False
                TAXPRM.Text = Format(CDbl(PolData.Tables(0).Rows(0)("TAXPRM")) * -1, "###,0.000")
                TAXPRM.ClientEnabled = False
                CONPRM.Text = Format(CDbl(PolData.Tables(0).Rows(0)("CONPRM")) * -1, "###,0.000")
                CONPRM.ClientEnabled = False
                STMPRM.Text = Format(CDbl(PolData.Tables(0).Rows(0)("STMPRM")) * -1, "###,0.000")
                STMPRM.ClientEnabled = False
                ISSPRM.Text = Format(CDbl(PolData.Tables(0).Rows(0)("ISSPRM")) * -1, "###,0.000")
                ISSPRM.ClientEnabled = False
                TOTPRM.Text = Format(CDbl(PolData.Tables(0).Rows(0)("TOTPRM")) * -1, "###,0.000")
                TOTPRM.ClientEnabled = False
                OK.Enabled = True
                OK.Focus()
                Cancel.Enabled = False

            Case "ConfirmCancel"
                Dim myList = DirectCast(Session("UserInfo"), List(Of String))
                Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                SetPm("@BranchNo", DbType.String, IIf(Left(PolNo.Text, 2) = Left(myList(7), 2) And Right(PolNo.Text, 2) = Right(myList(7), 2), myList(7), Left(PolNo.Text, 4)), Parm, 1)
                'SetPm("@BranchNo", DbType.String, Br.Text, Parm, 1)
                OrderNo.Text = CallSP("LastOrderNo", Conn, Parm)

                EndNo.Text = GetLastEnd(PolNo.Text) + 1

                Select Case Request("Sys")

                    Case "01", "02", "03", "OR", "MC", "MB", "MA", "OC", "ER", "CR", "07", "08", "27", "MM", "MS", "PH"
                        ExecSql("insert into policyFile(PolNo,OldPolicy,OrderNo,EndNo,LoadNo,issuDate,issuTime,EntryDate,CustNo,AgentNo,OwnNo,SubIns,CoverType,Measure,Interval,CoverFrom,CoverTo,Currency,PayType,ExcRate,Stop,LASTNET,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,Branch,UserName,SerialNo) values('" _
           & PolNo.Text & "','" _
           & OldPolicy.Text & "','" _
           & OrderNo.Text & "'," _
           & EndNo.Text & "," _
           & LoadNo.Text & "," _
           & "CONVERT(DATETIME,'" & Format(Today.Date, "yyyy-MM-dd") & " 00:00:00',102)," _
           & "convert(varchar, '" & Format(Now(), "yyyy-MM-dd HH:mm:ss") & "', 120)," _
           & "CONVERT(DATETIME,'" & Format(Today.Date, "yyyy-MM-dd") & " 00:00:00',102)," _
           & CustNo.Text & "," _
           & 0 & "," _
           & 0 & ",'" _
           & Request("sys") & "'," _
           & 1 & "," _
           & 5 & "," _
           & 0 & "," _
           & "CONVERT(DATETIME,'" & Format(RefundFrom.Value, "yyyy-MM-dd") & " 00:00:00',102)," _
           & "CONVERT(DATETIME,'" & Format(CDate(CoverTo.Text), "yyyy-MM-dd") & " 00:00:00',102)," _
           & Currency.Value & "," _
           & Val(PayType.Text) & "," _
           & Exchange.Text & "," _
           & 1 & "," _
           & CDbl(lastNet.Text) & "," _
           & CDbl(NETPRM.Value) & "," _
           & CDbl(TAXPRM.Value) & "," _
           & CDbl(CONPRM.Value) & "," _
           & CDbl(STMPRM.Value) & "," _
           & CDbl(ISSPRM.Value) & "," _
           & CDbl(TOTPRM.Value) & ",'" & Br.Text.Trim & "','" & myList(8) & "','" & SerialNo.Text & "'" _
           & ")")

                        ExecSql("update policyFile set Stop=1 where PolNo='" & PolNo.Text & "' and endNo=" & Request("EndNo") & " and LoadNo=" & Request("LoadNo"))
                        Cancel.Enabled = False
                        Parm = Array.CreateInstance(GetType(SqlParameter), 6)
                        SetPm("@table", DbType.String, GetGroupFile(Request("Sys")), Parm, 0)
                        SetPm("@OldOrder", DbType.String, Request("OrderNo"), Parm, 1)
                        SetPm("@NewOrder", DbType.String, OrderNo.Text, Parm, 2)
                        SetPm("@OldEnd", DbType.String, Request("EndNo"), Parm, 3)
                        SetPm("@NewEnd", DbType.String, EndNo.Text, Parm, 4)
                        SetPm("@Sys", DbType.String, Request("sys"), Parm, 5)
                        ExecSql(CallSP("BuildInsertReissue", Conn, Parm))
                    Case Else
                        ExecSql("insert into policyFile(PolNo,OldPolicy,OrderNo,EndNo,LoadNo,EntryDate,CustNo,AgentNo,OwnNo,SubIns,CoverType,Measure,Interval,CoverFrom,CoverTo,Currency,PayType,ExcRate,Stop,LASTNET,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,Branch,UserName,SerialNo) values('" _
           & PolNo.Text & "','" _
           & OldPolicy.Text & "','" _
           & OrderNo.Text & "'," _
           & EndNo.Text & "," _
           & LoadNo.Text & "," _
           & "CONVERT(DATETIME,'" & Format(Today.Date, "yyyy-MM-dd") & " 00:00:00',102)," _
           & CustNo.Text & "," _
           & 0 & "," _
           & 0 & ",'" _
           & Request("sys") & "'," _
           & 1 & "," _
           & 5 & "," _
           & 0 & "," _
           & "CONVERT(DATETIME,'" & Format(CDate(CoverFrom.Text), "yyyy-MM-dd") & " 00:00:00',102)," _
           & "CONVERT(DATETIME,'" & Format(CDate(CoverTo.Text), "yyyy-MM-dd") & " 00:00:00',102)," _
           & Currency.Value & "," _
           & Val(PayType.Text) & "," _
           & Exchange.Text & "," _
           & 1 & "," _
           & CDbl(lastNet.Text) & "," _
           & CDbl(NETPRM.Value) & "," _
           & CDbl(TAXPRM.Value) & "," _
           & CDbl(CONPRM.Value) & "," _
           & CDbl(STMPRM.Value) & "," _
           & CDbl(ISSPRM.Value) & "," _
           & CDbl(TOTPRM.Value) & ",'" & Br.Text.Trim & "','" & myList(8) & "','" & SerialNo.Text & "'" _
           & ")")

                        ExecSql("update policyFile set Stop=1 where PolNo='" & PolNo.Text & "' and endNo=" & Request("EndNo") & " and LoadNo=" & Request("LoadNo"))
                        Cancel.Enabled = False
                        Parm = Array.CreateInstance(GetType(SqlParameter), 6)
                        SetPm("@table", DbType.String, GetGroupFile(Request("Sys")), Parm, 0)
                        SetPm("@OldOrder", DbType.String, Request("OrderNo"), Parm, 1)
                        SetPm("@NewOrder", DbType.String, OrderNo.Text, Parm, 2)
                        SetPm("@OldEnd", DbType.String, Request("EndNo"), Parm, 3)
                        SetPm("@NewEnd", DbType.String, EndNo.Text, Parm, 4)
                        SetPm("@Sys", DbType.String, Request("sys"), Parm, 5)
                        ExecSql(CallSP("BuildInsertReissue", Conn, Parm))

                End Select
                ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('../Policy/Default.aspx?sys=" & Request("sys") & "','_self'); </script>")
                If Request("Sys") = "01" Or Request("Sys") = "02" Or Request("Sys") = "03" Or Request("Sys") = "27" Or Request("Sys") = "OR" Then

                Else
                    ASPxWebControl.RedirectOnCallback("../Reins/DistPolicy.aspx?OrderNo=" + OrderNo.Text.Trim + "&EndNo=" + EndNo.Text + "&LoadNo=" + LoadNo.Text + "&Branch=" + Left(Trim(PolNo.Text), 4) + "&Sys=" + Request("sys") + "&PolNo=" + PolNo.Text + "")
                End If
            Case "Refund" '„·Õﬁ „— œ
                EndNo.Text = Val(GetLastEnd(PolNo.Text, LoadNo.Text)) + 1
                Table1.Visible = True
                'Calculate.Focus()
                RefundFrom.MinDate = CDate(CoverFrom.Text)
                RefundFrom.MaxDate = CDate(CoverTo.Text)
                'RefundFrom.Value = Today().Date

            Case "CalcRefund"
                Table1.Visible = True
                'tmpdate.Text = Format(splited(1), "yyyy/MM/dd")
                RefundFrom.MinDate = CDate(CoverFrom.Text)
                RefundFrom.MaxDate = CDate(CoverTo.Text)

                Dim PolicyRec As New DataSet
                If Request("EndNo") = 0 Then
                    Select Case Request("sys")
                        Case "MC", "MB", "MA" : Dim dbadapter = New SqlDataAdapter("Select PolNo,sum(NETPRM),sum(TAXPRM),sum(CONPRM),sum(STMPRM),sum(ISSPRM),sum(EXTPRM),sum(TOTPRM),sum(lastnet) from PolicyFile Group By PolNo Having PolNo='" & Request("PolNo") & "'", Conn)
                            dbadapter.Fill(PolicyRec)
                        'PolicyRec = RecSet("Select PolNo,sum(NETPRM),sum(TAXPRM),sum(CONPRM),sum(STMPRM),sum(ISSPRM),sum(TOTPRM),sum(lastnet) from PolicyFile Group By PolNo Having PolNo='" & Request("PolNo") & "'", Conn)
                        Case "OC" : Dim dbadapter1 = New SqlDataAdapter("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' and loadNo=" & Request("LoadNo"), Conn)
                            dbadapter1.Fill(PolicyRec)
                            'PolicyRec = RecSet("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' and loadNo=" & Request("LoadNo"), Conn)

                        Case Else : Dim dbadapter2 = New SqlDataAdapter("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "'", Conn)
                            dbadapter2.Fill(PolicyRec)
                            'PolicyRec = RecSet("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "'", Conn)
                    End Select
                Else
                    Dim dbadapter3 = New SqlDataAdapter("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' AND EndNo=" & Request("EndNo") & "", Conn)
                    dbadapter3.Fill(PolicyRec)
                    'PolicyRec = RecSet("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' AND EndNo=" & Request("EndNo") & "", Conn)
                End If
                PolNo.Text = PolicyRec.Tables(0).Rows(0)("PolNo")
                NETPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(1), "###0.000")
                TAXPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(2), "###0.000")
                CONPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(3), "###0.000")
                STMPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(4), "###0.000")
                ISSPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(5), "###0.000")
                TOTPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(7), "###0.000")
                lastNet.Text = Format(PolicyRec.Tables(0).Rows(0)(8), "###0.000")
                Dim PolicyRec1 As New DataSet
                Dim dbadapter4 = New SqlDataAdapter("Select * from PolicyFile,CustomerFile where PolicyFile.CustNo=CustomerFile.CustNo and PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") _
                  & " and LoadNo=" & Request("LoadNo"), Conn)
                dbadapter4.Fill(PolicyRec1)
                'PolicyRec = RecSet("Select * from PolicyFile,CustomerFile where PolicyFile.CustNo=CustomerFile.CustNo and PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") _
                '& " and LoadNo=" & Request("LoadNo"), Conn)
                'EndNo.Text = PolicyRec1.Tables(0).Rows(0)("EndNo")
                'LoadNo.Text = PolicyRec1.Tables(0).Rows(0)("LoadNo")
                CoverTo.Text = Format(PolicyRec1.Tables(0).Rows(0)("CoverTo"), "yyyy/MM/dd")
                CoverFrom.Text = Format(PolicyRec1.Tables(0).Rows(0)("CoverFrom"), "yyyy/MM/dd")
                CustName.Text = PolicyRec1.Tables(0).Rows(0)("custname")
                CustNo.Text = PolicyRec1.Tables(0).Rows(0)("CustNo")
                PayType.Text = PolicyRec1.Tables(0).Rows(0)("PayType")
                Currency.SelectedIndex = PolicyRec1.Tables(0).Rows(0)("Currency") - 1
                Exchange.Text = PolicyRec1.Tables(0).Rows(0)("ExcRate")
                IssuDate.Text = Format(PolicyRec1.Tables(0).Rows(0)("IssuDate"), "yyyy/MM/dd")
                SerialNo.Text = IIf(PolicyRec1.Tables(0).Rows(0)("SerialNo") Is Nothing, 0, PolicyRec1.Tables(0).Rows(0)("SerialNo"))
                Dim DayDiff = DateDiff(DateInterval.Day, CDate(CoverTo.Text), CDate(splited(1))) / DateDiff(DateInterval.Day, CDate(CoverFrom.Text), CDate(CoverTo.Text)) / 12
                If Request("sys") = "MC" Or Request("sys") = "MB" Or Request("sys") = "MA" Or Request("sys") = "OC" Or Request("sys") = "ER" Or Request("sys") = "CR" Or Today.Date < CDate(CoverFrom.Text) Then
                    NETPRM.Text = Format(NETPRM.Text * -1, "###,0.00\0")
                    'TAXPRM.Text = 0
                    TAXPRM.Text = Format(CDbl(TAXPRM.Text) * -1, "###,0.00\0")
                    CONPRM.Text = Format(CONPRM.Text * -1, "###,0.00\0")
                    If Val(STMPRM.Text) <> 0 Then STMPRM.Text = Format(GetStmVal(Request("sys"), EndNo.Text), "###,0.000")
                    'STMPRM.Text = 0
                    ISSPRM.Text = Format(GetIssuVal(Request("sys"), EndNo.Text), "###,0.000")
                    TOTPRM.Text = Format(NETPRM.Text + CDbl(TAXPRM.Text) + CDbl(CONPRM.Text) + CDbl(STMPRM.Text) + CDbl(ISSPRM.Text), "###,0.00\0")
                Else
                    NETPRM.Text = Format(IIf(CDbl(NETPRM.Text) = 0, CDbl(lastNet.Text), CDbl(NETPRM.Text)) * DateDiff(DateInterval.Day, CDate(CoverTo.Text), CDate(splited(1))) / DateDiff(DateInterval.Day, CDate(CoverFrom.Text), CDate(CoverTo.Text)), "###,0.00\0")
                    MainCalcDx(Me, NETPRM.Text, Request("sys"), EndNo.Text, GetSpCase(Val(CustNo.Text)))
                End If
                'Table1.Visible = True
                'me.Page.Server.
                OKRefund.Enabled = True

            Case "ConfirmRefund"

                Table1.Visible = True
                RefundFrom.MinDate = CDate(CoverFrom.Text)
                RefundFrom.MaxDate = CDate(CoverTo.Text)
                Dim PolicyRec As New DataSet
                If Request("EndNo") = 0 Then
                    Select Case Request("sys")
                        Case "MC", "MB", "MA" : Dim dbadapter = New SqlDataAdapter("Select PolNo,sum(NETPRM),sum(TAXPRM),sum(CONPRM),sum(STMPRM),sum(ISSPRM),sum(EXTPRM),sum(TOTPRM),sum(lastnet) from PolicyFile Group By PolNo Having PolNo='" & Request("PolNo") & "'", Conn)
                            dbadapter.Fill(PolicyRec)
                        'PolicyRec = RecSet("Select PolNo,sum(NETPRM),sum(TAXPRM),sum(CONPRM),sum(STMPRM),sum(ISSPRM),sum(TOTPRM),sum(lastnet) from PolicyFile Group By PolNo Having PolNo='" & Request("PolNo") & "'", Conn)
                        Case "OC" : Dim dbadapter1 = New SqlDataAdapter("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' and loadNo=" & Request("LoadNo"), Conn)
                            dbadapter1.Fill(PolicyRec)
                            'PolicyRec = RecSet("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' and loadNo=" & Request("LoadNo"), Conn)

                        Case Else : Dim dbadapter2 = New SqlDataAdapter("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "'", Conn)
                            dbadapter2.Fill(PolicyRec)
                            'PolicyRec = RecSet("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "'", Conn)
                    End Select
                Else
                    Dim dbadapter3 = New SqlDataAdapter("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' AND EndNo=" & Request("EndNo") & "", Conn)
                    dbadapter3.Fill(PolicyRec)
                    'PolicyRec = RecSet("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' AND EndNo=" & Request("EndNo") & "", Conn)
                End If
                PolNo.Text = PolicyRec.Tables(0).Rows(0)("PolNo")
                NETPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(1), "###0.000")
                TAXPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(2), "###0.000")
                CONPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(3), "###0.000")
                STMPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(4), "###0.000")
                ISSPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(5), "###0.000")
                TOTPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(7), "###0.000")
                lastNet.Text = Format(PolicyRec.Tables(0).Rows(0)(8), "###0.000")
                Dim PolicyRec1 As New DataSet
                Dim dbadapter4 = New SqlDataAdapter("Select * from PolicyFile,CustomerFile where PolicyFile.CustNo=CustomerFile.CustNo and PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") _
                  & " and LoadNo=" & Request("LoadNo"), Conn)
                dbadapter4.Fill(PolicyRec1)
                'PolicyRec = RecSet("Select * from PolicyFile,CustomerFile where PolicyFile.CustNo=CustomerFile.CustNo and PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") _
                '& " and LoadNo=" & Request("LoadNo"), Conn)
                'EndNo.Text = PolicyRec1.Tables(0).Rows(0)("EndNo")
                'LoadNo.Text = PolicyRec1.Tables(0).Rows(0)("LoadNo")
                CoverTo.Text = Format(PolicyRec1.Tables(0).Rows(0)("CoverTo"), "yyyy/MM/dd")
                CoverFrom.Text = Format(PolicyRec1.Tables(0).Rows(0)("CoverFrom"), "yyyy/MM/dd")
                CustName.Text = PolicyRec1.Tables(0).Rows(0)("custname")
                CustNo.Text = PolicyRec1.Tables(0).Rows(0)("CustNo")
                PayType.Text = PolicyRec1.Tables(0).Rows(0)("PayType")
                Currency.SelectedIndex = PolicyRec1.Tables(0).Rows(0)("Currency") - 1
                Exchange.Text = PolicyRec1.Tables(0).Rows(0)("ExcRate")
                IssuDate.Text = Format(PolicyRec1.Tables(0).Rows(0)("IssuDate"), "yyyy/MM/dd")
                SerialNo.Text = IIf(PolicyRec1.Tables(0).Rows(0)("SerialNo") Is Nothing, 0, PolicyRec1.Tables(0).Rows(0)("SerialNo"))
                'Dim DayDiff = DateDiff(DateInterval.Day, CDate(CoverTo.Text), CDate(splited(1))) / DateDiff(DateInterval.Day, CDate(CoverFrom.Text), CDate(CoverTo.Text)) / 12
                Dim DayDiff = DateDiff(DateInterval.Day, CDate(CoverTo.Text), CDate(tmpdate.Text)) / DateDiff(DateInterval.Day, CDate(CoverFrom.Text), CDate(CoverTo.Text)) / 12
                If Request("sys") = "MC" Or Request("sys") = "MB" Or Request("sys") = "MA" Or Request("sys") = "OC" Or Request("sys") = "ER" Or Request("sys") = "CR" Or Today.Date < CDate(CoverFrom.Text) Then
                    NETPRM.Text = Format(NETPRM.Text * -1, "###,0.000")
                    'TAXPRM.Text = 0
                    'TAXPRM.Text = Format(CDbl(TAXPRM.Text) * -1, "###,0.00\0")
                    'CONPRM.Text = Format(CONPRM.Text * -1, "###,0.000")
                    'If Val(STMPRM.Text) <> 0 Then STMPRM.Text = Format(GetStmVal(Request("sys"), EndNo.Text), "###,0.000")
                    ''STMPRM.Text = 0
                    'ISSPRM.Text = Format(GetIssuVal(Request("sys"), EndNo.Text), "###,0.000")
                    'TOTPRM.Text = Format(NETPRM.Text + CDbl(TAXPRM.Text) + CDbl(CONPRM.Text) + CDbl(STMPRM.Text) + CDbl(ISSPRM.Text), "###,0.00\0")
                    MainCalcDx(Me, NETPRM.Text, Request("sys"), EndNo.Text, GetSpCase(Val(CustNo.Text)))
                Else
                    NETPRM.Text = Format(IIf(CDbl(NETPRM.Text) = 0, CDbl(lastNet.Text), CDbl(NETPRM.Text)) * DateDiff(DateInterval.Day, CDate(CoverTo.Text), CDate(tmpdate.Text)) / DateDiff(DateInterval.Day, CDate(CoverFrom.Text), CDate(CoverTo.Text)), "###,0.00\0")
                    MainCalcDx(Me, NETPRM.Text, Request("sys"), EndNo.Text, GetSpCase(Val(CustNo.Text)))
                End If
                'Table1.Visible = True
                'me.Page.Server.
                OKRefund.Enabled = True
                Dim myList = DirectCast(Session("UserInfo"), List(Of String))
                Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                SetPm("@BranchNo", DbType.String, IIf(Left(PolNo.Text, 2) = Left(myList(7), 2) And Right(PolNo.Text, 2) = Right(myList(7), 2), myList(7), Left(PolNo.Text, 4)), Parm, 1)
                OrderNo.Text = CallSP("LastOrderNo", Conn, Parm)

                'EndNo.Text = GetLastEnd(PolNo.Text) + 1

                Select Case Request("Sys")

                    Case "01", "02", "03", "OR", "MC", "OC", "CR", "ER", "07", "08", "27", "MM", "MS"
                        ExecSql("insert into policyFile(PolNo,OldPolicy,OrderNo,EndNo,LoadNo,issuDate,issuTime,EntryDate,CustNo,AgentNo,OwnNo,SubIns,CoverType,Measure,Interval,CoverFrom,CoverTo,Currency,PayType,ExcRate,Stop,LASTNET,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,Branch,UserName,SerialNo) values('" _
           & PolNo.Text & "','" _
           & OldPolicy.Text & "','" _
           & OrderNo.Text & "'," _
           & EndNo.Text & "," _
           & LoadNo.Text & "," _
           & "CONVERT(DATETIME,'" & Format(Today.Date, "yyyy-MM-dd") & " 00:00:00',102)," _
           & "convert(varchar, '" & Format(Now(), "yyyy-MM-dd HH:mm:ss") & "', 120)," _
           & "CONVERT(DATETIME,'" & Format(Today.Date, "yyyy-MM-dd") & " 00:00:00',102)," _
           & CustNo.Text & "," _
           & 0 & "," _
           & 0 & ",'" _
           & Request("sys") & "'," _
           & 1 & "," _
           & 5 & "," _
           & 0 & "," _
           & "CONVERT(DATETIME,'" & Format(CDate(tmpdate.Text), "yyyy-MM-dd") & " 00:00:00',102)," _
           & "CONVERT(DATETIME,'" & Format(CDate(CoverTo.Text), "yyyy-MM-dd") & " 00:00:00',102)," _
           & Currency.Value & "," _
           & Val(PayType.Text) & "," _
           & Exchange.Text & "," _
           & 1 & "," _
           & CDbl(lastNet.Text) & "," _
           & CDbl(NETPRM.Value) & "," _
           & CDbl(TAXPRM.Value) & "," _
           & CDbl(CONPRM.Value) & "," _
           & CDbl(STMPRM.Value) & "," _
           & CDbl(ISSPRM.Value) & "," _
           & CDbl(TOTPRM.Value) & ",'" & Br.Text.Trim & "','" & myList(8) & "','" & SerialNo.Text & "'" _
           & ")")

                        ExecSql("update policyFile set Stop=1 where PolNo='" & PolNo.Text & "' and endNo=" & Request("EndNo") & " and LoadNo=" & Request("LoadNo"))
                        Cancel.Enabled = False
                        Parm = Array.CreateInstance(GetType(SqlParameter), 6)
                        SetPm("@table", DbType.String, GetGroupFile(Request("Sys")), Parm, 0)
                        SetPm("@OldOrder", DbType.String, Request("OrderNo"), Parm, 1)
                        SetPm("@NewOrder", DbType.String, OrderNo.Text, Parm, 2)
                        SetPm("@OldEnd", DbType.String, Request("EndNo"), Parm, 3)
                        SetPm("@NewEnd", DbType.String, EndNo.Text, Parm, 4)
                        SetPm("@Sys", DbType.String, Request("sys"), Parm, 5)
                        ExecSql(CallSP("BuildInsertReissue", Conn, Parm))
                    Case Else
                        ExecSql("insert into policyFile(PolNo,OldPolicy,OrderNo,EndNo,LoadNo,EntryDate,CustNo,AgentNo,OwnNo,SubIns,CoverType,Measure,Interval,CoverFrom,CoverTo,Currency,PayType,ExcRate,Stop,LASTNET,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,Branch,UserName,SerialNo) values('" _
           & PolNo.Text & "','" _
           & OldPolicy.Text & "','" _
           & OrderNo.Text & "'," _
           & EndNo.Text & "," _
           & LoadNo.Text & "," _
           & "CONVERT(DATETIME,'" & Format(Today.Date, "yyyy-MM-dd") & " 00:00:00',102)," _
           & CustNo.Text & "," _
           & 0 & "," _
           & 0 & ",'" _
           & Request("sys") & "'," _
           & 1 & "," _
           & 5 & "," _
           & 0 & "," _
           & "CONVERT(DATETIME,'" & Format(CDate(tmpdate.Text), "yyyy-MM-dd") & " 00:00:00',102)," _
           & "CONVERT(DATETIME,'" & Format(CDate(CoverTo.Text), "yyyy-MM-dd") & " 00:00:00',102)," _
           & Currency.Value & "," _
           & Val(PayType.Text) & "," _
           & Exchange.Text & "," _
           & 1 & "," _
           & CDbl(lastNet.Text) & "," _
           & CDbl(NETPRM.Value) & "," _
           & CDbl(TAXPRM.Value) & "," _
           & CDbl(CONPRM.Value) & "," _
           & CDbl(STMPRM.Value) & "," _
           & CDbl(ISSPRM.Value) & "," _
           & CDbl(TOTPRM.Value) & ",'" & Br.Text.Trim & "','" & myList(8) & "','" & SerialNo.Text & "'" _
           & ")")

                        ExecSql("update policyFile set Stop=1 where PolNo='" & PolNo.Text & "' and endNo=" & Request("EndNo") & " and LoadNo=" & Request("LoadNo"))
                        Cancel.Enabled = False
                        Parm = Array.CreateInstance(GetType(SqlParameter), 6)
                        SetPm("@table", DbType.String, GetGroupFile(Request("Sys")), Parm, 0)
                        SetPm("@OldOrder", DbType.String, Request("OrderNo"), Parm, 1)
                        SetPm("@NewOrder", DbType.String, OrderNo.Text, Parm, 2)
                        SetPm("@OldEnd", DbType.String, Request("EndNo"), Parm, 3)
                        SetPm("@NewEnd", DbType.String, EndNo.Text, Parm, 4)
                        SetPm("@Sys", DbType.String, Request("sys"), Parm, 5)
                        ExecSql(CallSP("BuildInsertReissue", Conn, Parm))

                End Select
                ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('../Policy/Default.aspx?sys=" & Request("sys") & "','_self'); </script>")
                If Request("Sys") = "01" Or Request("Sys") = "02" Or Request("Sys") = "03" Or Request("Sys") = "27" Or Request("Sys") = "OR" Or Request("Sys") = "08" Then

                Else
                    ASPxWebControl.RedirectOnCallback("../Reins/DistPolicy.aspx?OrderNo=" + OrderNo.Text.Trim + "&EndNo=" + EndNo.Text + "&LoadNo=" + LoadNo.Text + "&Branch=" + Left(Trim(PolNo.Text), 4) + "&Sys=" + Request("sys") + "&PolNo=" + PolNo.Text + "")
                End If
        End Select
    End Sub





End Class
