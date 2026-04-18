Imports System.Data.SqlClient

Partial Public Class Reinsurance_DestributeClaim
    Inherits Page
    Dim Treaty As New DataSet
    Dim DB, PolDistRatios As New DataSet
    Dim mainexcrate As Double
    Dim Gno As Integer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Request("ClmNo") <> "" Then
            Gno = GetGroupNo(Request("ClmNo"), GetPolBranch(Request("PolNo"), Request("EndNo"), Request("loadNo")))
            If Not IsPostBack Then
                'Dim sssql As String = "SELECT MainClaimFile.ClmNo, MainClaimFile.PolNo, PolicyFile.OrderNo, PolicyFile.Currency, PolicyFile.ExcRate, PolicyFile.CustNo, MainClaimFile.EndNo, MainClaimFile.LoadNo, " _
                '         & "MainClaimFile.SubIns, MainClaimFile.Branch, MainClaimFile.ClmDate, MainClaimFile.ClmSysDate, CustomerFile.CustName," & GetSumIns(Request("Sys"), Request("OrderNo"), Request("EndNo"), Request("loadNo"), Request("PolNo"), Request("GroupNo")) & " AS SumIns " _
                '         & " FROM MainClaimFile INNER JOIN PolicyFile ON PolicyFile.EndNo = MainClaimFile.EndNo AND MainClaimFile.PolNo = PolicyFile.PolNo AND MainClaimFile.LoadNo = PolicyFile.LoadNo AND MainClaimFile.SubIns = PolicyFile.SubIns " _
                '         & " INNER JOIN CustomerFile ON PolicyFile.CustNo = CustomerFile.CustNo" _
                '         & " WHERE (MainClaimFile.ClmNo = '" & Request("ClmNo") & "') AND (MainClaimFile.PolNo = '" & Request("polNo") & "') "

                Dim sssql As String = "Select MainClaimFile.ClmNo, MainClaimFile.PolNo, PolicyFile.OrderNo, PolicyFile.Currency,PolicyFile.CoverType , PolicyFile.ExcRate, PolicyFile.CustNo, MainClaimFile.EndNo, MainClaimFile.LoadNo, " _
                    & "MainClaimFile.SubIns, MainClaimFile.Branch, MainClaimFile.ClmDate, MainClaimFile.ClmSysDate, CustomerFile.CustName " _
                    & ", " & GetSumInsG(Request("Sys"), Request("OrderNo"), Request("EndNo"), Request("loadNo"), Request("PolNo"), Request("GroupNo")) & "  As SumIns, " _
                    & "isnull(sum(DetailSettelement.Value),0) As Net, " _
                    & " MainSattelement.DAILYDTE As Date, " _
                    & "sum(DetailSettelement.Value) As Flag, " _
                    & " 'سداد' as Type " _
                    & " From MainClaimFile LEFT OUTER Join " _
                    & " MainSattelement On MainClaimFile.ClmNo = MainSattelement.ClmNo LEFT OUTER Join " _
                    & " DetailSettelement On MainSattelement.ClmNo=DetailSettelement.ClmNo And MainSattelement.TPID=DetailSettelement.TPID And MainSattelement.No=DetailSettelement.no LEFT OUTER Join " _
                    & "PolicyFile On MainClaimFile.PolNo = PolicyFile.PolNo And MainClaimFile.EndNo = PolicyFile.EndNo And MainClaimFile.LoadNo = PolicyFile.LoadNo LEFT OUTER Join " _
                    & "CustomerFile On PolicyFile.CustNo = CustomerFile.CustNo LEFT OUTER Join " _
                    & " ThirdParty On MainSattelement.ClmNo = ThirdParty.ClmNo And MainSattelement.TPID = ThirdParty.TPID " _
                    & " Where (MainClaimFile.ClmNo = '" & Request("ClmNo") & "') AND (MainClaimFile.PolNo = '" & Request("polNo") & "') and MainSattelement.DAILYNUM<>0" _
                    & " Group by DetailSettelement.ClmNo,DetailSettelement.No, DetailSettelement.TPID,MainClaimFile.ClmNo,MainClaimFile.PolNo,PolicyFile.OrderNo, PolicyFile.Currency, " _
                    & "PolicyFile.ExcRate, PolicyFile.CustNo, MainClaimFile.EndNo, MainClaimFile.LoadNo, " _
                    & "MainClaimFile.SubIns, MainClaimFile.Branch, MainClaimFile.ClmDate, MainClaimFile.ClmSysDate, CustomerFile.CustName, MainSattelement.DAILYDTE, PolicyFile.CoverType " _
                & " UNION all " _
                    & " Select MainClaimFile.ClmNo, MainClaimFile.PolNo, PolicyFile.OrderNo, PolicyFile.Currency, PolicyFile.CoverType, PolicyFile.ExcRate, PolicyFile.CustNo, MainClaimFile.EndNo, MainClaimFile.LoadNo, " _
                    & "MainClaimFile.SubIns, MainClaimFile.Branch, MainClaimFile.ClmDate, MainClaimFile.ClmSysDate, CustomerFile.CustName " _
                    & ", " & GetSumInsG(Request("Sys"), Request("OrderNo"), Request("EndNo"), Request("loadNo"), Request("PolNo"), Request("GroupNo")) & " As SumIns, " _
                    & " sum(estimation.value) As Net, " _
                    & " Estimation.Date, " _
                    & " -1 As Flag, " _
                    & "'احتياط' as Type " _
                    & " From MainClaimFile LEFT OUTER Join " _
                    & " Estimation On  MainClaimFile.ClmNo=Estimation.ClmNo left  OUTER Join " _
                    & " MainSattelement On MainClaimFile.ClmNo = MainSattelement.ClmNo LEFT OUTER Join " _
                    & " PolicyFile On MainClaimFile.PolNo = PolicyFile.PolNo And MainClaimFile.EndNo = PolicyFile.EndNo And MainClaimFile.LoadNo = PolicyFile.LoadNo LEFT OUTER Join " _
                    & " CustomerFile On PolicyFile.CustNo = CustomerFile.CustNo LEFT OUTER Join " _
                    & " ThirdParty On MainSattelement.ClmNo = ThirdParty.ClmNo And MainSattelement.TPID = ThirdParty.TPID " _
                    & " Where (MainClaimFile.ClmNo = '" & Request("ClmNo") & "') AND (MainClaimFile.PolNo = '" & Request("polNo") & "')" _
                    & " Group by MainClaimFile.ClmNo, MainClaimFile.PolNo, PolicyFile.OrderNo, PolicyFile.Currency, " _
                    & " PolicyFile.ExcRate, PolicyFile.CustNo, MainClaimFile.EndNo, MainClaimFile.LoadNo, " _
                    & " MainClaimFile.SubIns, MainClaimFile.Branch, MainClaimFile.ClmDate, MainClaimFile.ClmSysDate, CustomerFile.CustName, Estimation.TPID, Estimation.Sn, Estimation.Date, PolicyFile.CoverType"
                'Response.Write(ssSQL)
                Dim dbadapter = New SqlDataAdapter(sssql, Conn)
                dbadapter.Fill(DB)
                PolNo.Text = DB.Tables(0).Rows(0)("PolNo")
                ClmNo.Text = DB.Tables(0).Rows(0)("ClmNo")
                EndNo.Text = DB.Tables(0).Rows(0)("EndNo")
                LoadNo.Text = DB.Tables(0).Rows(0)("LoadNo")
                CustName.Text = DB.Tables(0).Rows(0)("CustName")
                IssuDate.Text = Format(DB.Tables(0).Rows(0)("ClmSysDate"), "yyyy/MM/dd")
                SumInsured.Text = Format(DB.Tables(0).Rows(0)("SumIns"), "###,#0.000")
                NETPAID.Text = Format(DB.Tables(0).Rows(0)("Net"), "###,#0.000")
                'EndCnt.Text = DB.Tables(0).Rows(0)("Cnt")
                Exc.Text = DB.Tables(0).Rows(0)("ExcRate")
                CustNo.Text = DB.Tables(0).Rows(0)("CustNo")
                TOTPAID.Text = DB.Tables(0).Rows(0)("Net")
                Cur.Text = DB.Tables(0).Rows(0)("Currency")
                State.Text = DB.Tables(0).Rows(0)("Type")
                mainexcrate = DB.Tables(0).Rows(0)("ExcRate")
                Dim TyDigit As Integer
                Dim Ty As String
                'Cfrom.Text = Format(DB.Tables(0).Rows(0)("CoverFrom"), "yyyy/MM/dd")
                'Cto.Text = Format(DB.Tables(0).Rows(0)("CoverTo"), "yyyy/MM/dd")
                'Dim TotPRM As Double
                If Request("PolNo") = Request("OrderNo") Then
                    Ty = Mid(PolNo.Text, 1, 4)
                    TyDigit = Val(Mid(PolNo.Text, 1, 4))
                Else

                    Ty = GetUnderWritinYear(PolNo.Text)
                    TyDigit = CInt(GetUnderWritinYear(PolNo.Text))

                    'Ty = Mid(PolNo.Text, 7, 4)
                    'TyDigit = Val(Mid(PolNo.Text, 7, 4))
                End If

                Select Case Request("Sys")

                    Case "MC", "MB", "MA", "OC"
                        TreatyNo.Text = Request("Sys") + IIf(Len(CStr(DB.Tables(0).Rows(0)("CoverType"))) = 1, "0" + CStr(DB.Tables(0).Rows(0)("CoverType")), CStr(DB.Tables(0).Rows(0)("CoverType"))) + Ty

                    Case Else
                        TreatyNo.Text = Request("Sys") + IIf(Len(CStr(DB.Tables(0).Rows(0)("CoverType"))) = 1, "0" + CStr(DB.Tables(0).Rows(0)("CoverType")), CStr(DB.Tables(0).Rows(0)("CoverType"))) + Ty
                End Select
                DistNet.Text = Format(CDbl(NETPAID.Text), "###,#0.000")

                Dim TreatyData = New SqlDataAdapter("Select * from TRREGFLE where TreatyNo='" & TreatyNo.Text & "'", Conn)
                TreatyData.Fill(Treaty)

                Capacity.Text = Format(Treaty.Tables(0).Rows(0)("TRCAPCTY"), "###,#0.000")
                Ret.Text = Format(Treaty.Tables(0).Rows(0)("TRRETAMT"), "###,#0.000")
                QS.Text = Format(Treaty.Tables(0).Rows(0)("TRQSRAMT"), "###,#0.000")
                FirstSup.Text = Format(Treaty.Tables(0).Rows(0)("TR1STAMT"), "###,#0.000")
                SecondSup.Text = Format(Treaty.Tables(0).Rows(0)("TR2STAMT"), "###,#0.000")

                If CDbl(Capacity.Value) >= CDbl(DB.Tables(0).Rows(0)("SumIns")) Then
                    DistNet.Text = Format(CDbl(NETPAID.Text), "###,#0.000")
                Else
                    DistNet.Text = Format(((CDbl(Capacity.Value) / DB.Tables(0).Rows(0)("SumIns")) * 100) * (DB.Tables(0).Rows(0)("Net")) / 100, "###,#0.000")
                End If
                DistRe()
            End If

        End If

    End Sub

    Private Sub DistRe()
        ExecConn("delete from NETPRM where PolNo='" & Trim(PolNo.Text) & "-" & Trim(ClmNo.Text) & "'" _
                 & " and EndNo=" & Request("EndNo") & " and " _
                 & " LoadNo=" & Request("LoadNo") & " and TP='" & Request("Sys") & "'", Conn)
        'If State.Text = "سداد" The n DistResp(0)
        Dim EndFile As New DataSet
        Dim EndData = New SqlDataAdapter("Select MainClaimFile.ClmNo, MainClaimFile.PolNo, PolicyFile.OrderNo, PolicyFile.Currency, PolicyFile.CoverType, PolicyFile.ExcRate, PolicyFile.CustNo, MainClaimFile.EndNo, MainClaimFile.LoadNo, " _
                    & "MainClaimFile.SubIns, MainClaimFile.Branch, MainClaimFile.ClmDate, MainClaimFile.ClmSysDate, CustomerFile.CustName " _
                    & ", " & GetSumInsG(Request("Sys"), Request("OrderNo"), Request("EndNo"), Request("loadNo"), Request("PolNo"), Request("GroupNo")) & "  As SumIns, " _
                    & "isnull(sum(DetailSettelement.Value),0) As Net, " _
                    & " MainSattelement.DAILYDTE As Date, " _
                    & "sum(DetailSettelement.Value) As Flag, " _
                    & " 'سداد' as Type " _
                    & " From MainClaimFile LEFT OUTER Join " _
                    & " MainSattelement On MainClaimFile.ClmNo = MainSattelement.ClmNo LEFT OUTER Join " _
                    & " DetailSettelement On MainSattelement.ClmNo=DetailSettelement.ClmNo And MainSattelement.TPID=DetailSettelement.TPID And MainSattelement.No=DetailSettelement.no LEFT OUTER Join " _
                    & "PolicyFile On MainClaimFile.PolNo = PolicyFile.PolNo And MainClaimFile.EndNo = PolicyFile.EndNo And MainClaimFile.LoadNo = PolicyFile.LoadNo LEFT OUTER Join " _
                    & "CustomerFile On PolicyFile.CustNo = CustomerFile.CustNo LEFT OUTER Join " _
                    & " ThirdParty On MainSattelement.ClmNo = ThirdParty.ClmNo And MainSattelement.TPID = ThirdParty.TPID " _
                    & " Where (MainClaimFile.ClmNo = '" & Request("ClmNo") & "') AND (MainClaimFile.PolNo = '" & Request("polNo") & "') and MainSattelement.DAILYNUM<>0" _
                    & " Group by DetailSettelement.ClmNo,DetailSettelement.No, DetailSettelement.TPID,MainClaimFile.ClmNo,MainClaimFile.PolNo,PolicyFile.OrderNo, PolicyFile.Currency, " _
                    & "PolicyFile.ExcRate, PolicyFile.CustNo, MainClaimFile.EndNo, MainClaimFile.LoadNo, " _
                    & "MainClaimFile.SubIns, MainClaimFile.Branch, MainClaimFile.ClmDate, MainClaimFile.ClmSysDate, CustomerFile.CustName, MainSattelement.DAILYDTE,PolicyFile.CoverType " _
                & " UNION all " _
                    & " Select MainClaimFile.ClmNo, MainClaimFile.PolNo, PolicyFile.OrderNo, PolicyFile.Currency, PolicyFile.CoverType, PolicyFile.ExcRate, PolicyFile.CustNo, MainClaimFile.EndNo, MainClaimFile.LoadNo, " _
                    & "MainClaimFile.SubIns, MainClaimFile.Branch, MainClaimFile.ClmDate, MainClaimFile.ClmSysDate, CustomerFile.CustName " _
                    & ", " & GetSumInsG(Request("Sys"), Request("OrderNo"), Request("EndNo"), Request("loadNo"), Request("PolNo"), Request("GroupNo")) & " As SumIns, " _
                    & " estimation.value As Net, " _
                    & " Estimation.Date, " _
                    & " -1 As Flag, " _
                    & "'احتياط' as Type " _
                    & " From MainClaimFile LEFT OUTER Join " _
                    & " Estimation On  MainClaimFile.ClmNo=Estimation.ClmNo left  OUTER Join " _
                    & " MainSattelement On MainClaimFile.ClmNo = MainSattelement.ClmNo LEFT OUTER Join " _
                    & " PolicyFile On MainClaimFile.PolNo = PolicyFile.PolNo And MainClaimFile.EndNo = PolicyFile.EndNo And MainClaimFile.LoadNo = PolicyFile.LoadNo LEFT OUTER Join " _
                    & " CustomerFile On PolicyFile.CustNo = CustomerFile.CustNo LEFT OUTER Join " _
                    & " ThirdParty On MainSattelement.ClmNo = ThirdParty.ClmNo And MainSattelement.TPID = ThirdParty.TPID " _
                    & "Where (MainClaimFile.ClmNo = '" & Request("ClmNo") & "') AND (MainClaimFile.PolNo = '" & Request("polNo") & "')" _
                    & "Group by MainClaimFile.ClmNo, MainClaimFile.PolNo, PolicyFile.OrderNo, PolicyFile.Currency, " _
                    & "PolicyFile.ExcRate, PolicyFile.CustNo, MainClaimFile.EndNo, MainClaimFile.LoadNo, " _
                    & "MainClaimFile.SubIns, MainClaimFile.Branch, MainClaimFile.ClmDate, MainClaimFile.ClmSysDate, CustomerFile.CustName, Estimation.TPID, Estimation.Sn,PolicyFile.CoverType, Estimation.Date,Estimation.Value", Conn)
        EndData.Fill(EndFile)
        Dim Dist As New DataSet
        EndCnt.Text = EndFile.Tables(0).Rows.Count
        'ExecSql("delete from NETPRM where PolNo='" & Trim(PolNo.Text) + "-" + Trim(ClmNo.Text) & "'" _
        '& " and EndNo=" & Request("EndNo") & " and " _
        '& "LoadNo=" & Request("LoadNo") & " and TP='" & Request("Sys") & "'")
        Dim DistData = New SqlDataAdapter("select * from NetPRm where PolNo='" & Trim(PolNo.Text) + "-" + Trim(ClmNo.Text) & "'" _
                        & " and EndNo=" & Request("EndNo") & " and " _
                        & "LoadNo=" & Request("LoadNo") & " and TP='" & Request("Sys") & "'", Conn)
        DistData.Fill(Dist)
        If Val(EndCnt.Text) = 0 Then
            'DistResp(0)
        End If
        'If State.Text <> "تحت التسوية" Then
        For i As Integer = 1 To EndFile.Tables(0).Rows.Count
            If EndFile.Tables(0).Rows(i - 1)("Net") = 0 And IsDBNull(EndFile.Tables(0).Rows(i - 1)("Date")) Then

                GoTo 10
            End If

            Exc.Text = EndFile.Tables(0).Rows(i - 1)("ExcRate")
            If EndFile.Tables(0).Rows.Count <> Val(EndCnt.Text) Then
                'If Val(War.Text) <> 0 And i = 1 Then War.Text = Format(Val(War.Text) / Val(EndCnt.Text), "0.000")
                IssuDate.Text = EndFile.Tables(0).Rows(i - 1)("Date")
                DistPolicy(EndFile.Tables(0).Rows(i - 1)("Net") * EndFile.Tables(0).Rows(i - 1)("SumIns") / CDbl(SumInsured.Text), EndFile.Tables(0).Rows(i - 1)("SumIns") / IIf(mainexcrate = Exc.Value, 1, EndFile.Tables(0).Rows(i - 1)("ExcRate")))
            Else
                IssuDate.Text = EndFile.Tables(0).Rows(i - 1)("Date")
                DistPolicy(EndFile.Tables(0).Rows(i - 1)("Net") * EndFile.Tables(0).Rows(i - 1)("SumIns") / CDbl(SumInsured.Text), EndFile.Tables(0).Rows(i - 1)("SumIns") / IIf(mainexcrate = Exc.Value, 1, EndFile.Tables(0).Rows(i - 1)("ExcRate")))
            End If
            ExecConn("INSERT INTO [NetPrm]([PolNo],[LoadNo],[EndNo],[CustNo],[InsAmt],[Net],[Total],[PolD],[Cur],[Exc]" _
                        & ",[Tp],[Br],[Treaty],[Amount],[Qs],[FirsSup],[SecondSup],[Elective],[SpecialRet],[LineSlip],[Qsw],[FirsSupw],[Electivew],[Type]) values('" _
                        & Trim(PolNo.Text).Trim + "-" + Trim(ClmNo.Text).Trim & "'," _
                        & LoadNo.Text & "," _
                        & EndNo.Text & "," _
                        & CustNo.Text & "," _
                        & CDbl(TSumIns.Text) & "," _
                        & CDbl(TNetPrm.Text) & "," _
                        & IIf(EndFile.Tables(0).Rows(i - 1)("Type") = "سداد", EndFile.Tables(0).Rows(i - 1)("Net"), -1) & "," _
                        & "CONVERT(DATETIME,'" & Format(CDate(IssuDate.Text), "yyyy-MM-dd") & " 00:00:00',102)," _
                        & EndFile.Tables(0).Rows(i - 1)("Currency") & "," _
                        & EndFile.Tables(0).Rows(i - 1)("ExcRate") & ",'" _
                        & Request("Sys") & "','" _
                        & Request("Branch") & "','" _
                        & TreatyNo.Text & "'," _
                        & CDbl(TRet.Text) & "," _
                        & CDbl(TQS.Text) & "," _
                        & CDbl(TFirstSup.Text) & "," _
                        & CDbl(TSecondSup.Text) & "," _
                        & CDbl(TFac.Text) & "," & CDbl(TSpRet.Text) & "," _
                        & CDbl(TLineSlip.Text) & "," _
                        & CDbl(wQS.Text) & "," _
                        & CDbl(wFirstSup.Text) & "," _
                        & CDbl(wFac.Text) & ",'Clm')", Conn)
10:     Next
        'End If
        SqlDataSource1.SelectParameters(0).DefaultValue = Trim(PolNo.Text).Trim + "-" + Trim(ClmNo.Text).Trim
        SqlDataSource1.SelectParameters(1).DefaultValue = EndNo.Text
        SqlDataSource1.SelectParameters(2).DefaultValue = LoadNo.Text
        ASPxGridView1.DataBind()

    End Sub

    'Private Sub DistResp(ByVal Count As Integer)

    '    Dim EndFile As New DataSet

    '    Dim EndData = New System.Data.SqlClient.SqlDataAdapter("SELECT " & GetSumIns(Request("Sys"), Request("OrderNo"), Request("EndNo"), Request("loadNo"), Request("PolNo"), Request("GroupNo")) & " AS SumIns, " _
    '                            & "Estimation.value AS NetPrm, Estimation.Date AS Date " _
    '                            & "FROM Estimation FULL OUTER JOIN MainClaimFile ON Estimation.clmno = MainClaimFile.ClmNo AND Estimation.polno = MainClaimFile.PolNo " _
    '                            & "WHERE MainClaimFile.ClmNo = '" & Request("ClmNo") & "' AND Estimation.value is not NULL AND Estimation.polno='" & Request("PolNo") & "'", Conn)
    '    EndData.Fill(EndFile)
    '    If EndFile.Tables(0).Rows.Count > 0 Then Count = Count + EndFile.Tables(0).Rows.Count
    '    Dim Dist As New DataSet

    '    ExecSql("delete from NETPRM where PolNo='" & Trim(PolNo.Text) + "-" + Trim(ClmNo.Text) & "'" _
    '                           & " and EndNo=" & Request("EndNo") & " and " _
    '                           & "LoadNo=" & Request("LoadNo") & " and TP='" & Request("Sys") & "'")
    '    Dim DistData = New System.Data.SqlClient.SqlDataAdapter("select * from NetPRm where PolNo='" & Trim(PolNo.Text) + "-" + Trim(ClmNo.Text) & "'" _
    '                    & " and EndNo=" & Request("EndNo") & " and " _
    '                    & "LoadNo=" & Request("LoadNo") & " and TP='" & Request("Sys") & "'", Conn)
    '    DistData.Fill(Dist)

    '    For i As Integer = 1 To Count
    '        If EndFile.Tables(0).Rows.Count <> Count Then
    '            'If Val(War.Text) <> 0 And i = 1 Then War.Text = Format(Val(War.Text) / Val(EndCnt.Text), "0.000")
    '            IssuDate.Text = EndFile.Tables(0).Rows(i - 1)("Date")
    '            DistPolicy(CDbl(NETPAID.Text) / Count, CDbl(SumInsured.Text) / Count)
    '        Else
    '            IssuDate.Text = EndFile.Tables(0).Rows(i - 1)("Date")
    '            DistPolicy(EndFile.Tables(0).Rows(i - 1)("NetPrm") * EndFile.Tables(0).Rows(i - 1)("SumIns") / CDbl(SumInsured.Text), EndFile.Tables(0).Rows(i - 1)("SumIns"))
    '        End If
    '        ExecSql("INSERT INTO [NetPrm]([PolNo],[LoadNo],[EndNo],[CustNo],[InsAmt],[Net],[Total],[PolD],[Cur],[Exc]" _
    '                & ",[Tp],[Br],[Treaty],[Amount],[Qs],[FirsSup],[SecondSup],[Elective],[LineSlip],[Qsw],[FirsSupw],[Electivew]) values('" _
    '                & Trim(PolNo.Text) + "-" + Trim(ClmNo.Text) & "'," _
    '                & LoadNo.Text & "," _
    '                & EndNo.Text & "," _
    '                & CustNo.Text & "," _
    '                & CDbl(TSumIns.Text) & "," _
    '                & CDbl(TNetPrm.Text) & "," _
    '                & -1 & "," _
    '                & "CONVERT(DATETIME,'" & Format(CDate(IssuDate.Text), "yyyy-MM-dd") & " 00:00:00',102)," _
    '                & Cur.Text & "," _
    '                & Val(Exc.Text) & ",'" _
    '                & Request("Sys") & "','" _
    '                & Request("Branch") & "','" _
    '                & TreatyNo.Text & "'," _
    '                & CDbl(TRet.Text) & "," _
    '                & CDbl(TQS.Text) & "," _
    '                & CDbl(TFirstSup.Text) & "," _
    '                & CDbl(TSecondSup.Text) & "," _
    '                & CDbl(TFac.Text) & "," _
    '                & CDbl(TLineSlip.Text) & "," _
    '                & CDbl(wQS.Text) & "," _
    '                & CDbl(wFirstSup.Text) & "," _
    '                & CDbl(wFac.Text) & ")"
    '        )
    '    Next

    '    SqlDataSource1.SelectParameters(0).DefaultValue = Trim(PolNo.Text) + "-" + Trim(ClmNo.Text)
    '    SqlDataSource1.SelectParameters(1).DefaultValue = EndNo.Text
    '    SqlDataSource1.SelectParameters(2).DefaultValue = LoadNo.Text
    '    ASPxGridView1.DataBind()

    'End Sub
    Protected Function GetFirstOrder() As String
        Dim IssueVal As New DataSet

        Dim dbadapter = New System.Data.SqlClient.SqlDataAdapter("select Max(PolicyFile.orderNo) from PolicyFile Inner Join " & GetGroupFile(Request("Sys")) & " On  " _
                               & "PolicyFile.OrderNo=" & GetGroupFile(Request("Sys")) & ".OrderNo and " _
                               & "PolicyFile.EndNo=" & GetGroupFile(Request("Sys")) & ".EndNo and " _
                               & "PolicyFile.LoadNo=" & GetGroupFile(Request("Sys")) & ".LoadNo and " _
                               & "PolicyFile.SubIns=" & GetGroupFile(Request("Sys")) & ".SubIns  " _
                               & " where PolNo='" & Trim(Request("PolNo")) & "' and  PolicyFile.SubIns='" & Request("Sys") & "'" _
                            , Conn)
        dbadapter.Fill(IssueVal)
        GetFirstOrder = IssueVal.Tables(0).Rows(0)(0)
    End Function

    Protected Function GetEndData() As Boolean
        Dim IssueVal As New DataSet

        Dim dbadapter = New SqlDataAdapter("select * from " & GetGroupFile(Request("Sys")) & "  where " _
                               & " OrderNo='" & Trim(Request("OrderNo")) & "' and EndNo=" & Request("EndNo") _
                               & " and LoadNo=" & Request("LoadNo") & " and SubIns='" & Request("Sys") & "'" _
                            , Conn)
        dbadapter.Fill(IssueVal)
        If IssueVal.Tables(0).Rows.Count = 0 Then
            GetEndData = False
        Else
            GetEndData = True
        End If
    End Function

    Protected Sub DistPolicy(ByVal NET As Double, ByVal SumINS As Double)
        Dim dbadapter1 = New SqlDataAdapter("Select * from netprm where PolNo='" & Request("polNo") & "' AND EndNo=" & Request("EndNo") & "", Conn)
        dbadapter1.Fill(PolDistRatios)

        Dim TreatyData = New SqlDataAdapter("select * from TRREGFLE where TreatyNo='" & TreatyNo.Text & "'", Conn)
        TreatyData.Fill(Treaty)

        TSumIns.Text = Format(SumINS, "###,#0.000")
        TNetPrm.Text = Format(NET, "###,#0.000")
        TRet.Text = "0"
        TQS.Text = "0"
        TFirstSup.Text = "0"
        TSecondSup.Text = "0"
        TFac.Value = "0"
        TLineSlip.Text = "0"
        wQS.Text = "0"
        wFirstSup.Text = "0"
        wFac.Text = "0"
        If Val(State.Text) <> 0 Then
            If SumINS <= (2000000 / Val(Exc.Text)) Then
                wQS.Text = State.Text / Val(EndCnt.Text)
            Else
                If SumINS <= (6000000 / Val(Exc.Text)) Then
                    wQS.Text = (2000000 / Val(Exc.Text)) / SumINS * State.Text / Val(EndCnt.Text)
                    wFirstSup.Text = (SumINS - (2000000 / Val(Exc.Text))) / SumINS * State.Text / Val(EndCnt.Text)
                Else
                    wQS.Text = (2000000 / Val(Exc.Text)) / SumINS * State.Text / Val(EndCnt.Text)
                    wFirstSup.Text = (4000000 / Val(Exc.Text)) / SumINS * State.Text / Val(EndCnt.Text)
                    wFac.Text = (SumINS - (6000000 / Val(Exc.Text))) / SumINS * State.Text / Val(EndCnt.Text)
                End If
            End If
        End If
        If Val(DistNet.Text) = NET Then
            SumINS = CDbl(DistNet.Text) / NET * SumINS
            TFac.Text = NET - CDbl(DistNet.Text)
            NET = CDbl(DistNet.Text)
        End If
        With Treaty.Tables(0).Rows(0)
            If SumINS <= ((Ret.Value) / Val(Exc.Text)) + ((QS.Value) / Val(Exc.Text)) Then
                On Error Resume Next
                TRet.Text = Format(((Ret.Value) / Val(Exc.Text)) / (((Ret.Value) / Val(Exc.Text)) + ((QS.Value) / Val(Exc.Text))) * NET, "0.00\0")
                TQS.Text = Format(((QS.Value) / Val(Exc.Text)) / (((Ret.Value) / Val(Exc.Text)) + ((QS.Value) / Val(Exc.Text))) * NET, "0.00\0")
                'If Treaty!TRQSRAMT = 0 Then Dist!FirsSup = Format((Dist!InsAmt - (Treaty!TRRETAMT / Dist!Exc) - (Treaty!TRQSRAMT / Dist!Exc)) / Dist!InsAmt * cdbl(Dist!net), "0.00\0")
            End If
            If (SumINS > ((Ret.Value) / Val(Exc.Text)) + ((QS.Value) / Val(Exc.Text))) And (SumINS <= ((Capacity.Value) / Val(Exc.Text))) Then
                If SumINS <= ((Ret.Value) / Val(Exc.Text)) + ((QS.Value) / Val(Exc.Text)) + ((FirstSup.Value) / Val(Exc.Text)) Then
                    TRet.Text = Format(((Ret.Value) / Val(Exc.Text)) / SumINS * NET, "0.00\0")
                    TQS.Text = Format(((QS.Value) / Val(Exc.Text)) / SumINS * NET, "0.00\0")
                    'If TP = 1 Then
                    TFirstSup.Text = Format((SumINS - ((Ret.Value) / Val(Exc.Text)) - ((QS.Value) / Val(Exc.Text))) / SumINS * NET, "0.00\0")
                    'Else
                    ' TFac.Text = Format((SumINS - (.Item("TRRETAMT") / Val(Exc.Text)) - (.Item("TRQSRAMT") / Val(Exc.Text))) / SumINS * NET, "0.00\0")
                    'End If
                Else
                    TRet.Text = Format(((Ret.Value) / Val(Exc.Text)) / SumINS * NET, "0.00\0")
                    TQS.Text = Format(((QS.Value) / Val(Exc.Text)) / SumINS * NET, "0.00\0")
                    'If TP = 1 Then
                    TFirstSup.Text = Format((((FirstSup.Value) / Val(Exc.Text))) / SumINS * NET, "0.00\0")
                    TSecondSup.Text = Format((SumINS - (((Capacity.Value) - (SecondSup.Value)) / Val(Exc.Text))) / SumINS * NET, "0.00\0")
                    'Else
                    'Dist!Elective = Format((SumINS - ((.Item("TRCAPCTY") - .Item("TR2STAMT")) / Val(Exc.Text))) / SumINS * NET, "0.00\0")
                    'End If
                End If
            End If
            Dim m As Double
            m = ((Capacity.Value) / Val(Exc.Text)) + ((SecondSup.Value) / Val(Exc.Text))
            If SumINS > ((Capacity.Value) / Val(Exc.Text)) Then
                TRet.Text = Format(((Ret.Value) / Val(Exc.Text)) / SumINS * NET, "0.00\0")
                TQS.Text = Format(((QS.Value) / Val(Exc.Text)) / SumINS * NET, "0.00\0")
                'If TP = 1 Then
                TFirstSup.Text = Format((((FirstSup.Value) / Val(Exc.Text))) / SumINS * NET, "0.00\0")
                TSecondSup.Text = Format((((SecondSup.Value) / Val(Exc.Text))) / SumINS * NET, "0.00\0")
                If Request("Sys") = "MC" Or Request("Sys") = "MB" Or Request("Sys") = "MA" Or Request("Sys") = "OC" Then
                    If SumINS - ((Capacity.Value) / Val(Exc.Text)) <= 4000000 And (Request("Sys") = "MC" Or Request("Sys") = "MB" Or Request("Sys") = "MA" Or Request("Sys") = "OC") And ((Capacity.Value) / Val(Exc.Text)) <> 0 Then
                        TLineSlip.Text = Format(((SumINS - ((Capacity.Value) / Val(Exc.Text)))) / SumINS * NET, "0.00\0")
                    Else
                        If ((Capacity.Value) / Val(Exc.Text)) <> 0 Then
                            TLineSlip.Text = Format((4000000) / SumINS * NET, "0.00\0")
                            TFac.Text = Format((SumINS - ((Capacity.Value) / Val(Exc.Text)) - 4000000) / SumINS * NET, "0.00\0")
                        Else
                            TFac.Text = Format((SumINS - ((Capacity.Value) / Val(Exc.Text))) / SumINS * NET, "0.00\0")
                        End If
                    End If
                Else
                    If SumINS >= m Then
                        TFac.Text = Format((SumINS - ((Capacity.Value) / Val(Exc.Text))) / SumINS * NET, "0.00\0")
                    Else
                        TFac.Text = Format((SumINS - ((Capacity.Value) / Val(Exc.Text))) / SumINS * NET, "0.00\0")
                    End If
                End If
            Else
            End If
        End With

        If (PolDistRatios.Tables(0).Rows(0)("Amount") / PolDistRatios.Tables(0).Rows(0)("NET")) * 100 = (Val(TRet.Value) / NET) * 100 Then

        Else
            TRet.Text = (PolDistRatios.Tables(0).Rows(0)("Amount") / PolDistRatios.Tables(0).Rows(0)("NET")) * NET
        End If
        If (PolDistRatios.Tables(0).Rows(0)("Qs") / PolDistRatios.Tables(0).Rows(0)("NET")) * 100 = (Val(TQS.Value) / NET) * 100 Then

        Else
            TQS.Text = (PolDistRatios.Tables(0).Rows(0)("Qs") / PolDistRatios.Tables(0).Rows(0)("NET")) * NET
        End If

        If (PolDistRatios.Tables(0).Rows(0)("FirsSup") / PolDistRatios.Tables(0).Rows(0)("NET")) * 100 = (Val(TFirstSup.Value) / NET) * 100 Then

        Else
            TFirstSup.Text = (PolDistRatios.Tables(0).Rows(0)("FirsSup") / PolDistRatios.Tables(0).Rows(0)("NET")) * NET
        End If
        If (PolDistRatios.Tables(0).Rows(0)("Elective") / PolDistRatios.Tables(0).Rows(0)("NET")) * 100 = (Val(TFac.Value) / NET) * 100 Then


        Else
            TFac.Text = (PolDistRatios.Tables(0).Rows(0)("Elective") / PolDistRatios.Tables(0).Rows(0)("NET")) * NET
        End If

        If (PolDistRatios.Tables(0).Rows(0)("SpecialRet") / PolDistRatios.Tables(0).Rows(0)("NET")) * 100 <> 0 Then

            TSpRet.Text = (PolDistRatios.Tables(0).Rows(0)("SpecialRetRatio") / 100) * NET
        Else


        End If
    End Sub
    Protected Sub ASPxButton5_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ASPxButton5.Click
        Dim TFacValue As Double = 0
        If CDbl(NETPAID.Text) <> CDbl(DistNet.Text) Then
            TFacValue = CDbl(NETPAID.Text) - CDbl(DistNet.Text)
        End If

        ExecSql("delete from NETPRM where PolNo='" & Trim(PolNo.Text) + "-" + Trim(ClmNo.Text) & "'" _
                               & " and EndNo=" & Request("EndNo") & " and " _
                               & "LoadNo=" & Request("LoadNo") & " and TP='" & Request("Sys") & "'")

        'ExecSql(SQL)
        DistRe()
        ASPxGridView1.DataBind()
    End Sub
    Public Function valu(ByVal Vr As Object) As Double
        If IsNothing(Vr) Then
            valu = 0
        Else
            If Val(Vr) <> 0 Then
                valu = Val(Vr)
            Else
                If Vr = True Then
                    valu = 1
                Else
                    valu = 0
                End If
            End If
        End If
    End Function

    Protected Sub ASPxButton3_Click(sender As Object, e As EventArgs) Handles ASPxButton3.Click

    End Sub

End Class