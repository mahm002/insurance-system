Imports System.Data.SqlClient

Partial Public Class Reins_DestributePolicy
    Inherits Page

    Private Treaty As New DataSet
    Private DB As New DataSet
    Private Lo() As String
    Private ExtraValue As Double = 0
    Private WExtraValue As Double = 0
    Private FExtraValue As Double = 0
    Private WFExtraValue As Double = 0

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Lo = Me.Session("LogInfo")
        Dim ssSQL, Ty As String
        Dim TyDigit As Int32
        If Request("PolNo") = Request("OrderNo") Then
            ASPxButton4.Visible = False
            ASPxButton5.Text = "احتساب القسط الإضافي"
        Else
            ASPxButton4.Visible = True
            'AlertL0.Visible = False
            'ExtPrm.Visible = False
        End If

        If Request("OrderNo") <> "" Then
            If Not IsPostBack Then
                If Request("Sys") = "12" Then
                    ssSQL = "select PolNo,CustName,Commision,CoverFrom,CoverTo,TOTPRM,PolicyFile.CustNo,IssuDate,PolicyFile.ExcRate,PolicyFile.Currency,PolicyFile.EndNo,PolicyFile.LoadNo,NetPRM,TOTPRM,CoverFrom,CoverTo,CoverType," _
                                   & "(select " & IIf(Request("Sys") = "04" Or Request("Sys") = "05", "Loads", "count(OrderNo)") _
                                   & " from " & GetGroupFile(Request("Sys")) & " as EndFile where EndFile.orderno=" & IIf(GetEndData(), "PolicyFile.OrderNo", "'" & GetFirstOrder() & "'") & " and " _
                                   & IIf(GetEndData(), " EndFile.EndNo=" & IIf(GetEndData(), "PolicyFile.EndNo", "0") & " and EndFile.LoadNo=PolicyFile.LoadNo and ", "") & " EndFile.SubIns=PolicyFile.SubIns) as EndCnt," _
                                   & IIf(Request("Sys") = "04" Or Request("Sys") = "05", " WarPrm,", "") & " (select " & GetSumInsFields(Request("Sys")) & " from " & GetGroupFile(Request("Sys")) & " as EndFile_1 where EndFile_1.orderno=" _
                                   & IIf(GetEndData(), "PolicyFile.OrderNo", "'" & GetFirstOrder() & "'") & " and " _
                                   & IIf(GetEndData(), " EndFile_1.EndNo=" & IIf(GetEndData(), "PolicyFile.EndNo", "0") & " and EndFile_1.LoadNo=PolicyFile.LoadNo and", "") & " EndFile_1.SubIns=PolicyFile.SubIns) as SumIns from PolicyFile Inner Join CustomerFile" _
                                   & " on PolicyFile.CustNo=CustomerFile.CustNo" _
                                   & " inner Join " & GetGroupFile(Request("Sys")) & " on " & GetGroupFile(Request("Sys")) & ".OrderNo=" & IIf(GetEndData(), "PolicyFile.OrderNo", "'" & GetFirstOrder() & "'") _
                                   & " and " & IIf(GetEndData(), GetGroupFile(Request("Sys")) & ".EndNo=" & IIf(GetEndData(), "PolicyFile.EndNo", "0") & " and " _
                                   & GetGroupFile(Request("Sys")) & ".LoadNo=PolicyFile.LoadNo and ", "") & GetGroupFile(Request("Sys")) & ".SubIns=PolicyFile.SubIns" _
                                   & " where PolicyFile.OrderNo='" & Trim(Request("OrderNo")) & "' and PolicyFile.EndNo=" & Request("EndNo") _
                                   & " and PolicyFile.LoadNo=" & Request("LoadNo") & " and PolicyFile.SubIns='" & Request("Sys") & "'" _
                                   & " and PolicyFile.Branch='" & Request("Branch") & "'"
                Else
                    ssSQL = "select PolNo,CustName,Commision,CoverFrom,CoverTo,TOTPRM,PolicyFile.CustNo,IssuDate,PolicyFile.ExcRate,PolicyFile.Currency,PolicyFile.EndNo,PolicyFile.LoadNo,NetPRM,TOTPRM,CoverFrom,CoverTo,CoverType," _
                                   & "(select " & IIf(Request("Sys") = "04" Or Request("Sys") = "05", "Loads", "count(OrderNo)") _
                                   & " from " & GetGroupFile(Request("Sys")) & " as EndFile where EndFile.orderno=" & IIf(GetEndData(), "PolicyFile.OrderNo", "'" & GetFirstOrder() & "'") & " and " _
                                   & IIf(GetEndData(), " EndFile.EndNo=" & IIf(GetEndData(), "PolicyFile.EndNo", "0") & " and EndFile.LoadNo=PolicyFile.LoadNo and ", "") & " EndFile.SubIns=PolicyFile.SubIns) as EndCnt," _
                                   & IIf(Request("Sys") = "04" Or Request("Sys") = "05", " WarPrm,", "") & " (select sum(" & GetSumInsFields(Request("Sys")) & ") from " & GetGroupFile(Request("Sys")) & " as EndFile_1 where EndFile_1.orderno=" _
                                   & IIf(GetEndData(), "PolicyFile.OrderNo", "'" & GetFirstOrder() & "'") & " and " _
                                   & IIf(GetEndData(), " EndFile_1.EndNo=" & IIf(GetEndData(), "PolicyFile.EndNo", "0") & " and EndFile_1.LoadNo=PolicyFile.LoadNo and", "") & " EndFile_1.SubIns=PolicyFile.SubIns) as SumIns from PolicyFile Inner Join CustomerFile" _
                                   & " on PolicyFile.CustNo=CustomerFile.CustNo" _
                                   & " inner Join " & GetGroupFile(Request("Sys")) & " on " & GetGroupFile(Request("Sys")) & ".OrderNo=" & IIf(GetEndData(), "PolicyFile.OrderNo", "'" & GetFirstOrder() & "'") _
                                   & " and " & IIf(GetEndData(), GetGroupFile(Request("Sys")) & ".EndNo=" & IIf(GetEndData(), "PolicyFile.EndNo", "0") & " and " _
                                   & GetGroupFile(Request("Sys")) & ".LoadNo=PolicyFile.LoadNo and ", "") & GetGroupFile(Request("Sys")) & ".SubIns=PolicyFile.SubIns" _
                                   & " where PolicyFile.OrderNo='" & Trim(Request("OrderNo")) & "' and PolicyFile.EndNo=" & Request("EndNo") _
                                   & " and PolicyFile.LoadNo=" & Request("LoadNo") & " and PolicyFile.SubIns='" & Request("Sys") & "'" _
                                   & " and PolicyFile.Branch='" & Request("Branch") & "'"
                End If
                'Response.Write(ssSQL)
                Dim dbadapter = New SqlDataAdapter(ssSQL, Conn)

                dbadapter.Fill(DB)
                Exc.Text = DB.Tables(0).Rows(0)("ExcRate")
                If Request("PolNo") = Request("OrderNo") Then
                    PolNo.Text = Request("PolNo")
                Else
                    PolNo.Text = DB.Tables(0).Rows(0)("PolNo")
                End If
                EndNo.Text = DB.Tables(0).Rows(0)("EndNo")
                LoadNo.Text = DB.Tables(0).Rows(0)("LoadNo")
                CustName.Text = DB.Tables(0).Rows(0)("CustName")
                'DB.Tables(0).Rows(0)("IssuDate").IsNull(0)
                If Request("PolNo") = Request("OrderNo") Then
                    IssuDate.Text = "1900/01/01"
                Else
                    IssuDate.Text = Format(DB.Tables(0).Rows(0)("IssuDate"), "yyyy/MM/dd")
                End If
                SumInsured.Text = Format(DB.Tables(0).Rows(0)("SumIns"), "###,#0.000")
                NetPRM.Text = Format(DB.Tables(0).Rows(0)("NetPRM"), "###,#0.000")
                HNetPRM.Text = Format(DB.Tables(0).Rows(0)("NetPRM"), "###,#0.000")
                'Comission.Text = 0 'Format(DB.Tables(0).Rows(0)("NetPRM") * IIf(DB.Tables(0).Rows(0).IsNull("Commision"), 0, DB.Tables(0).Rows(0)("Commision")) / 100, "0.000")
                EndCnt.Text = DB.Tables(0).Rows(0)("EndCnt")
                CustNo.Text = DB.Tables(0).Rows(0)("CustNo")
                TOTPRM.Text = DB.Tables(0).Rows(0)("TOTPRM")
                Cur.Text = DB.Tables(0).Rows(0)("Currency")
                Cfrom.Text = Format(DB.Tables(0).Rows(0)("CoverFrom"), "yyyy/MM/dd")
                Cto.Text = Format(DB.Tables(0).Rows(0)("CoverTo"), "yyyy/MM/dd")
                If Request("PolNo") = Request("OrderNo") Then
                    Ty = Mid(PolNo.Text, 1, 4)
                    TyDigit = Val(Mid(PolNo.Text, 1, 4))
                Else
                    Ty = GetUnderWritinYear(PolNo.Text)
                    TyDigit = CInt(GetUnderWritinYear(PolNo.Text))
                End If

                Select Case Request("Sys")

                    Case "04", "05"
                        TreatyNo.Text = Request("Sys") + IIf(Len(CStr(DB.Tables(0).Rows(0)("CoverType"))) = 1, "0" + CStr(DB.Tables(0).Rows(0)("CoverType")), CStr(DB.Tables(0).Rows(0)("CoverType"))) + Ty
                        If EndNo.Text = 0 Then
                            War.Text = DB.Tables(0).Rows(0)("WarPrm")
                            HWar.Text = DB.Tables(0).Rows(0)("WarPrm")
                        Else
                            War.Text = WarPrmCalc(DB.Tables(0).Rows(0)("WarPrm"))
                            HWar.Text = WarPrmCalc(DB.Tables(0).Rows(0)("WarPrm"))
                        End If

                    Case Else
                        TreatyNo.Text = Request("Sys") + IIf(Len(CStr(DB.Tables(0).Rows(0)("CoverType"))) = 1, "0" + CStr(DB.Tables(0).Rows(0)("CoverType")), CStr(DB.Tables(0).Rows(0)("CoverType"))) + Ty
                End Select
                'End If

                If Val(War.Text) <> 0 Then
                    'NetPRM.Text = CDbl(NetPRM.Text) - Val(War.Text)
                Else
                    War.Text = "0"
                    HWar.Text = "0"
                End If
                DistNet.Text = Format(CDbl(NetPRM.Text) - ((CDbl(SpecialRetentionAmount.Value) * CDbl(NetPRM.Text)) / 100) - Val(War.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                HDistNet.Text = Format(CDbl(NetPRM.Text) - ((CDbl(SpecialRetentionAmount.Value) * CDbl(NetPRM.Text)) / 100) - Val(War.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                Dim TreatyData = New SqlDataAdapter("select * from TRREGFLE where TreatyNo='" & TreatyNo.Text & "'", Conn)
                TreatyData.Fill(Treaty)
                If Treaty.Tables(0).Rows.Count = 0 Then
                    MsgBob(Me, "لم يتم إدخال حدود الاتفاقية لهذا النوع، يرجى الاتصال بإدارة إعادة التأمين ")

                    Exit Sub
                Else

                End If
                Capacity.Text = Format(Treaty.Tables(0).Rows(0)("TRCAPCTY"), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                TreatyDescrip.Text = Treaty.Tables(0).Rows(0)("Descrip")
                Ret.Text = Format(Treaty.Tables(0).Rows(0)("TRRETAMT"), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                QS.Text = Format(Treaty.Tables(0).Rows(0)("TRQSRAMT"), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                FirstSup.Text = Format(Treaty.Tables(0).Rows(0)("TR1STAMT"), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                SecondSup.Text = Format(Treaty.Tables(0).Rows(0)("TR2STAMT"), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                LineSlip.Text = Format(Treaty.Tables(0).Rows(0)("TRLSAMT"), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                TRQSRCOM.Text = Format(Treaty.Tables(0).Rows(0)("TRQSRCOM"), IIf(Exc.Text = 1, "###,#0.00", "###,#0.00"))
                TRWQSRCOM.Text = Format(Treaty.Tables(0).Rows(0)("TRWQSRCOM"), IIf(Exc.Text = 1, "###,#0.00", "###,#0.00"))
                TR1STCOM.Text = Format(Treaty.Tables(0).Rows(0)("TR1STCOM"), IIf(Exc.Text = 1, "###,#0.00", "###,#0.00"))
                TRW1STCOM.Text = Format(Treaty.Tables(0).Rows(0)("TRW1STCOM"), IIf(Exc.Text = 1, "###,#0.00", "###,#0.00"))
                TR2STCOM.Text = Format(Treaty.Tables(0).Rows(0)("TR2STCOM"), IIf(Exc.Text = 1, "###,#0.00", "###,#0.00"))
                'TRLSCOMM.Text = Format(Treaty.Tables(0).Rows(0)("TRLSCOMM"), IIf(Exc.Text = 1, "###,#0.00", "###,#0.00"))
                'IIf(IsDBNull(Treaty.Tables(0).Rows(0)("TRLSCOMM")), 0, Treaty.Tables(0).Rows(0)("TRLSCOMM"))
                TRLSCOMM.Text = IIf(IsDBNull(Treaty.Tables(0).Rows(0)("TRLSCOMM")), 0.00, Format(Treaty.Tables(0).Rows(0)("TRLSCOMM"), "###,#0.00"))
                FacComm.Text = Format(10, IIf(Exc.Text = 1, "###,#0.00", "###,#0.00"))
                If CDbl(Treaty.Tables(0).Rows(0)("TRCAPCTY")) + CDbl(Treaty.Tables(0).Rows(0)("TRLSAMT")) >= (CDbl(DB.Tables(0).Rows(0)("SumIns") * Val(Exc.Text)) / Val(EndCnt.Text)) Then
                    DistNet.Text = Format(CDbl(NetPRM.Text) - ((CDbl(SpecialRetentionAmount.Value) * CDbl(NetPRM.Text)) / 100) - CDbl(War.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    HDistNet.Text = Format(CDbl(NetPRM.Text) - ((CDbl(SpecialRetentionAmount.Value) * CDbl(NetPRM.Text)) / 100) - CDbl(War.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    SpecialRetention.Text = Format(CDbl(NetPRM.Text) - (CDbl(DistNet.Text)), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    F.Text = 0
                    HF.Text = 0
                    HFM.Text = 0
                Else
                    DistNet.Text = Format(((Val((Treaty.Tables(0).Rows(0).Item("TRCAPCTY") + Treaty.Tables(0).Rows(0).Item("TRLSAMT")) / Val(Exc.Text)) / DB.Tables(0).Rows(0)("SumIns")) * 100) * (DB.Tables(0).Rows(0)("NetPRM") - ((CDbl(SpecialRetentionAmount.Value) * CDbl(NetPRM.Text)) / 100)) / 100, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    HDistNet.Text = Format(((Val((Treaty.Tables(0).Rows(0).Item("TRCAPCTY") + Treaty.Tables(0).Rows(0).Item("TRLSAMT")) / Val(Exc.Text)) / DB.Tables(0).Rows(0)("SumIns")) * 100) * (DB.Tables(0).Rows(0)("NetPRM") - ((CDbl(SpecialRetentionAmount.Value) * CDbl(NetPRM.Text)) / 100)) / 100, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    SpecialRetention.Text = Format(((Val((Treaty.Tables(0).Rows(0).Item("TRCAPCTY") + Treaty.Tables(0).Rows(0).Item("TRLSAMT")) / Val(Exc.Text)) / DB.Tables(0).Rows(0)("SumIns")) * 100) * (DB.Tables(0).Rows(0)("NetPRM") - ((CDbl(DistNet.Text)))) / 100, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    F.Text = Format(Math.Round(CDbl(NetPRM.Text) - CDbl(DistNet.Text), 3), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    HF.Text = Math.Round(CDbl(NetPRM.Text) - CDbl(DistNet.Text), 3) ', IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    HFM.Text = Math.Round(CDbl(NetPRM.Text) - CDbl(DistNet.Text), 3) ', IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    FacRatio.PositionStart = 100 - ((Val((Treaty.Tables(0).Rows(0).Item("TRCAPCTY") + Treaty.Tables(0).Rows(0).Item("TRLSAMT")) / Val(Exc.Text)) / DB.Tables(0).Rows(0)("SumIns")) * 100)
                    FacRatio.MinValue = FacRatio.PositionStart
                End If
                DistRe()
            End If

        End If
        If CDbl(Capacity.Text) + CDbl(LineSlip.Text) < CDbl(SumInsured.Text) * Val(Exc.Text) Then
            AlertL.Visible = True
        Else
            AlertL.Visible = False
        End If

    End Sub

    Protected Sub ASPxButton5_Click(sender As Object, e As EventArgs) Handles ASPxButton5.Click
        ExecConn("delete from NETPRM where PolNo='" & PolNo.Text & "'" _
                               & " and EndNo=" & Request("EndNo") & " and " _
                               & "LoadNo=" & Request("LoadNo") & " and TP='" & Request("Sys") & "'", Conn)
        'ExecSql(SQL)
        DistRe()
        ASPxGridView1.DataBind()
    End Sub

    Private Sub DistRe()
        Dim TFacValue As Double = 0

        Dim EndFile As New DataSet

        Dim EndData = New SqlDataAdapter("select " & IIf(Request("Sys") = "06" Or Request("Sys") = "22" Or Request("Sys") = "16" Or Request("Sys") = "10" Or Request("Sys") = "08" Or Request("Sys") = "21", "Premium,", "") & "" & GetSumInsFields(Request("Sys")) & " as SumIns from " & GetGroupFile(Request("Sys")) & " where " & GetGroupFile(Request("Sys")) & ".OrderNo='" & Request("OrderNo") & "'" _
                       & " and " & GetGroupFile(Request("Sys")) & ".EndNo=" & Request("EndNo") & " and " _
                       & GetGroupFile(Request("Sys")) & ".LoadNo=" & Request("LoadNo") & " and " & GetGroupFile(Request("Sys")) & ".SubIns='" & Request("Sys") & "'", Conn)
        EndData.Fill(EndFile)
        Dim Dist As New DataSet
        'ExecSql("delete from NETPRM where PolNo='" & PolNo.Text & "'" _
        '              & " and EndNo=" & Request("EndNo") & " and " _
        '              & "LoadNo=" & Request("LoadNo") & " and TP='" & Request("Sys") & "'")
        Dim DistData = New SqlDataAdapter("select * from NetPRm where PolNo='" & PolNo.Text & "'" _
                        & " and EndNo=" & Request("EndNo") & " and " _
                        & "LoadNo=" & Request("LoadNo") & " and TP='" & Request("Sys") & "'", Conn)

        'If CDbl(NetPRM.Text) <> CDbl(DistNet.Text) + Val(Comission.Text) + Val(War.Text) Then
        'TFacValue = (CDbl(NetPRM.Text) - CDbl(War.Text)) - CDbl(DistNet.Text)
        'End If
        DistData.Fill(Dist)
        ExtraValue = 0
        If Dist.Tables(0).Rows.Count = 0 Then
            For i As Integer = 1 To Val(EndCnt.Text)
                If EndFile.Tables(0).Rows.Count <> Val(EndCnt.Text) Then
                    If Val(War.Text) <> 0 And i = 1 Then War.Text = War.Text
                    'If Request("Sys") = "04" Or Request("Sys") = "05" Then
                    'DistPolicy(CDbl(NetPRM.Text) / Val(EndCnt.Text), CDbl(SumInsured.Text) / Val(EndCnt.Text))
                    'Else
                    DistPolicy(Format(CDbl(NetPRM.Text) / Val(EndCnt.Text) - (CDbl(War.Text) / Val(EndCnt.Text)), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) - ((CDbl(SpecialRetentionAmount.Value) * CDbl(NetPRM.Text)) / 100) / Val(EndCnt.Text), Format(CDbl(SumInsured.Text) / Val(EndCnt.Text), IIf(Exc.Text = 1, "###,#0.0", "###,#0.0")))
                    'End If
                Else
                    'DistPolicy((CDbl(DistNet.Text) - Val(Comission.Text) - Val(War.Text) / Val(EndCnt.Text)) * EndFile.Tables(0).Rows(i - 1)("SumIns") / CDbl(SumInsured.Text), EndFile.Tables(0).Rows(i - 1)("SumIns"))
                    If Request("Sys") = "06" Or Request("Sys") = "22" Then
                        DistPolicy(EndFile.Tables(0).Rows(i - 1)("Premium"), EndFile.Tables(0).Rows(i - 1)("SumIns"))
                    Else
                        DistPolicy((CDbl(NetPRM.Text) - ((CDbl(SpecialRetentionAmount.Value) * CDbl(NetPRM.Text)) / 100) - Val(War.Text) / Val(EndCnt.Text)) * EndFile.Tables(0).Rows(i - 1)("SumIns") / CDbl(SumInsured.Text), EndFile.Tables(0).Rows(i - 1)("SumIns"))
                    End If

                End If
                Dim Etra As Double = 1.63
                Dim AccTP As Int16 = IIf(CDbl(NetPRM.Text) >= 0, 1, -1)
                Dim Temp As Double = CDbl(TNetPrm.Text) - (CDbl(TFac.Text) + CDbl(SpRet.Text) + CDbl(TRet.Text))
                Dim Wtemp As Double = (CDbl(War.Text) - CDbl(wFac.Text)) / Val(EndCnt.Text)
                Dim Ttemp As Double = CDbl(TFac.Text)
                Dim WTtemp As Double = CDbl(wFac.Text)
                ExtraValue += Format((Temp - (Temp * IIf(CDbl(TRQSRCOM.Text) = 0, CDbl(TR1STCOM.Text), CDbl(TRQSRCOM.Text)) / 100)) * Etra, "###,#0.000")
                WExtraValue += Format((Wtemp - (Wtemp * IIf(CDbl(TRWQSRCOM.Text) = 0, CDbl(TRW1STCOM.Text), CDbl(TRWQSRCOM.Text)) / 100)) * Etra, "###,#0.000")
                FExtraValue = Format(FExtraValue + (Ttemp - (CDbl(Ttemp) * (CDbl(FacComm.Text) / 100))) * Etra)
                WFExtraValue = Format(WFExtraValue + (WTtemp - (CDbl(WTtemp) * 0.1)) * Etra)
                'ExtPrm.Text = ExtraValue + FExtraValue + WExtraValue + WFExtraValue
                ExecConn("INSERT INTO [NetPrm]([PolNo],[LoadNo],[EndNo],[CustNo],[InsAmt],[Net],[War],[Total],[PolD],[Cur],[Exc],[DFm],[DTo]" _
                        & ",[Tp],[Br],[Treaty],[Amount],[Qs],[FirsSup],[SecondSup],[Elective],[LineSlip],[Qsw],[FirsSupw],[Electivew],[Comission],[UserName],[SpecialRet]) values('" _
                        & PolNo.Text & "'," _
                        & LoadNo.Text & "," _
                        & EndNo.Text & "," _
                        & CustNo.Text & "," _
                        & CDbl(TSumIns.Text) * AccTP & "," _
                        & CDbl(TNetPrm.Text) & "," _
                        & CDbl(Format(Val(War.Text) / Val(EndCnt.Text), "0.000")) & "," _
                        & CDbl(TOTPRM.Text) & "," _
                        & "CONVERT(DATETIME,'" & Format(CDate(IssuDate.Text), "yyyy-MM-dd") & " 00:00:00',102)," _
                        & Cur.Text & "," _
                        & Val(Exc.Text) & "," _
                        & "CONVERT(DATETIME,'" & Format(CDate(Cfrom.Text), "yyyy-MM-dd") & " 00:00:00',102)," _
                        & "CONVERT(DATETIME,'" & Format(CDate(Cto.Text), "yyyy-MM-dd") & " 00:00:00',102),'" _
                        & Request("Sys") & "','" _
                        & Request("Branch") & "','" _
                        & TreatyNo.Text & "'," _
                        & CDbl(TRet.Text) & "," _
                        & CDbl(TQS.Text) & "," _
                        & CDbl(TFirstSup.Text) & "," _
                        & CDbl(TSecondSup.Text) & "," _
                        & CDbl(TFac.Text) & "," _
                        & CDbl(TLineSlip.Text) & "," _
                        & CDbl(wQS.Text) & "," _
                        & CDbl(wFirstSup.Text) & "," _
                        & CDbl(wFac.Text) & "," _
                        & 0 & ",'" _
                        & Lo(0) & "'," _
                        & CDbl(TSpRet.Text) + CDbl(SpecialRetention.Text) & ")", Conn)
                'CDbl(Comission.Text) / Val(EndCnt.Text)
                'IIf(Request("Sys") <> "04", CDbl(SpRet.Text), CDbl(SpRet.Text) / Val(EndCnt.Text))
                ' & IIf(SpRetRatio.Value <> 0 And Request("Sys") <> "04", CDbl(HNetPRM.Text), IIf(Request("Sys") <> "04", CDbl(TNetPrm.Text), (CDbl(HNetPRM.Text) - CDbl(War.Text) - CDbl(Comission.Text)) / Val(EndCnt.Text))) & "," _
            Next
        End If
        SqlDataSource1.SelectParameters(0).DefaultValue = PolNo.Text
        SqlDataSource1.SelectParameters(1).DefaultValue = EndNo.Text
        SqlDataSource1.SelectParameters(2).DefaultValue = LoadNo.Text
        ASPxGridView1.DataBind()
        'Dim temp As Double = CDbl(TSumIns.Text) / CDbl(Capacity.Text) * (CDbl(HNetPRM.Text) - CDbl(War.Text) - CDbl(Comission.Text))
        'ExtraValue = Format(ExtraValue + (temp / Val(EndCnt.Text) - (temp / Val(EndCnt.Text) * (CDbl(TRQSRCOM.Text) / 100))) * 1.83, "###,#0.000")
        'ExtPrm.Text = ExtraValue
    End Sub

    Public Shared Function GetSumInsFields(ByVal Sys As String) As String
        Dim IssueVal As New DataSet
        Dim dbadapter = New SqlDataAdapter("select SumInsured from SubSystems where SubSysNo='" & Sys & "' and SumInsured is not null", Conn)
        dbadapter.Fill(IssueVal)
        GetSumInsFields = IssueVal.Tables(0).Rows(0)(0)
    End Function

    Protected Function GetFirstOrder() As String
        Dim IssueVal As New DataSet
        Dim Temp As String
        If Request("PolNo") = Request("OrderNo") Then
            Temp = "policyFile.OrderNo"
        Else
            Temp = "PolicyFile.PolNo"
        End If
        Dim dbadapter = New SqlDataAdapter("select Max(PolicyFile.orderNo) from PolicyFile Inner Join " & GetGroupFile(Request("Sys")) & " On  " _
                               & "PolicyFile.OrderNo=" & GetGroupFile(Request("Sys")) & ".OrderNo and " _
                               & "PolicyFile.EndNo=" & GetGroupFile(Request("Sys")) & ".EndNo and " _
                               & "PolicyFile.LoadNo=" & GetGroupFile(Request("Sys")) & ".LoadNo and " _
                               & "PolicyFile.SubIns=" & GetGroupFile(Request("Sys")) & ".SubIns  " _
                               & " where " & Temp & "='" & Trim(Request("PolNo")) & "' and  PolicyFile.SubIns='" & Request("Sys") & "'" _
                            , Conn)
        dbadapter.Fill(IssueVal)
        GetFirstOrder = IssueVal.Tables(0).Rows(0)(0)
    End Function

    Protected Function WarPrmCalc(ByVal War As Double) As Double
        Dim WarprmCalc1 As New DataSet
        Dim Temp As String
        Dim Tempend As Integer
        If Request("PolNo") = Request("OrderNo") Then
            Temp = "policyFile.OrderNo"
        Else
            Temp = "PolicyFile.PolNo"
        End If
        If Val(EndNo.Text) > 0 And Request("PolNo") = Request("OrderNo") Then
            Tempend = Val(EndNo.Text)
        Else
            Tempend = Val(EndNo.Text) - 1
        End If

        Dim dbadapter = New SqlDataAdapter("Select Sum(GodFile.WarPrm) As WarPrm FROM GodFile INNER Join PolicyFile On" _
                  & " GodFile.OrderNo=PolicyFile.OrderNo And GodFile.SubIns=PolicyFile.SubIns WHERE " & Temp & "='" & RTrim(PolNo.Text) & "' And GodFile.EndNo= " & Tempend & "", Conn)

        'Dim dbadapter = New Data.SqlClient.SqlDataAdapter("(SELECT Sum(WarPrm) As WarPrm FROM godfile  inner join policyfile on godfile.orderno =" _
        '                     & "(Select min(PolF.orderno) from PolicyFile AS PolF where godfile.endno<" & Val(Request("EndNo")) & " AND PolF.PolNo='" & RTrim(PolNo.Text) & "' ) AND godfile.SubIns = '" & Request("Sys") & "' ) ", ConnLocal)
        dbadapter.Fill(WarprmCalc1)

        If IsDBNull(WarprmCalc1.Tables(0).Rows(0)("WarPrm")) Or (WarprmCalc1.Tables(0).Rows(0)("WarPrm")) < 0 Then
            WarPrmCalc = 0
        Else
            If CDbl(NetPRM.Text) < 0 Then
                WarPrmCalc = IIf(WarprmCalc1.Tables(0).Rows(0)("WarPrm") = DB.Tables(0).Rows(0)("WarPrm"), -1 * DB.Tables(0).Rows(0)("WarPrm"), DB.Tables(0).Rows(0)("WarPrm") - WarprmCalc1.Tables(0).Rows(0)("WarPrm"))
            Else
                WarPrmCalc = Math.Abs(DB.Tables(0).Rows(0)("WarPrm") - CDbl(WarprmCalc1.Tables(0).Rows(0)("WarPrm")))
            End If
        End If
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
        Dim TreatyData = New SqlDataAdapter("select * from TRREGFLE where TreatyNo='" & TreatyNo.Text & "'", Conn)
        TreatyData.Fill(Treaty)

        TSumIns.Text = Format(SumINS, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
        TNetPrm.Text = Format(NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))

        TRet.Text = "0"
        TQS.Text = "0"
        TFirstSup.Text = "0"
        TSecondSup.Text = "0"
        TFac.Text = "0"
        TLineSlip.Text = "0"
        wQS.Text = "0"
        wFirstSup.Text = "0"
        wFac.Text = "0"
        TSpRet.Text = "0"
        Dim cnt As Integer
        If IsGroupedSys(Request("Sys")) Or (Request("Sys") = "MC" Or Request("Sys") = "MB" Or Request("Sys") = "MA") Then
            cnt = 1
        Else : cnt = Val(EndCnt.Text)
        End If

        'And SpRetRatio.Value = 0
        'If FacRatio.Value <> FacRatio.MinValue Then
        TRet.Text = "0"
        TQS.Text = "0"
        TFirstSup.Text = "0"
        TSecondSup.Text = "0"
        TFac.Text = "0"
        TLineSlip.Text = "0"
        'wQS.Text = "0"
        ' wFirstSup.Text = "0"
        'wFac.Text = "0"
        'SumINS = CDbl(DistNet.Text) / Val(TNetPrm.Text) * SumINS
        'TFac.Text = Format((NET - SpRet.Value), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) ' - CDbl(DistNet.Text)
        TFac.Text = Format(FacRatio.Value * NET / 100, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) ' - CDbl(DistNet.Text)
        TSpRet.Text = Format(SpRetRatio.Value * TFac.Text / 100, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
        TFac.Text = Math.Abs(TSpRet.Text - TFac.Text)
        NET = Format(CDbl(NET) - (CDbl(NET) * FacRatio.Value / 100), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
        SumINS = Format(CDbl(SumINS) - (CDbl(SumINS) * FacRatio.Value / 100), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
        'Else
        '    TRet.Text = "0"
        '    TQS.Text = "0"
        '    TFirstSup.Text = "0"
        '    TSecondSup.Text = "0"
        '    TFac.Text = "0"
        '    TLineSlip.Text = "0"
        '    'wQS.Text = "0"
        '    ' wFirstSup.Text = "0"
        '    'wFac.Text = "0"
        '    'SumINS = CDbl(DistNet.Text) / Val(TNetPrm.Text) * SumINS
        '    TFac.Text = Format(FacRatio.Value * NET / 100, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
        '    NET = Format(CDbl(NET) - (CDbl(NET) * FacRatio.Value / 100), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
        '    SumINS = Format(CDbl(SumINS) - (CDbl(SumINS) * FacRatio.Value / 100), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
        'End If

        If WarFacRatio.Value <> 0 Then
            wQS.Text = "0"
            wFirstSup.Text = "0"
            wFac.Text = "0"
            If CDbl(War.Text) = CDbl(DistWar.Text) Then
                wFac.Text = Format((CDbl(War.Text) / Val(EndCnt.Text)), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
            Else
                wFac.Text = Format((CDbl(DistWar.Text) / Val(EndCnt.Text)), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
            End If
            wQS.Text = Format(DistWar.Text / cnt, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
        End If

        With Treaty.Tables(0).Rows(0)
            If Val(War.Text) <> 0 Then
                If SumINS <= (2000000 / Val(Exc.Text)) Then
                    wQS.Text = Format((War.Text / Val(EndCnt.Text)) - wFac.Text, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                Else
                    If SumINS <= (.Item("TRCAPCTY") / Val(Exc.Text)) Then
                        wQS.Text = Format((2000000 / Val(Exc.Text)) / SumINS * (War.Text - wFac.Text) / Val(EndCnt.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                        wFirstSup.Text = Format((SumINS - (2000000 / Val(Exc.Text))) / SumINS * (War.Text - wFac.Text) / Val(EndCnt.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    Else
                        wQS.Text = Format((2000000 / Val(Exc.Text)) / SumINS * War.Text / Val(EndCnt.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                        wFirstSup.Text = Format((4000000 / Val(Exc.Text)) / SumINS * War.Text / Val(EndCnt.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                        wFac.Text = Format((SumINS - (6000000 / Val(Exc.Text))) / SumINS * War.Text / Val(EndCnt.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    End If
                End If
            End If
            '------- 1

            If SumINS <= (.Item("TRRETAMT") / Val(Exc.Text)) + (.Item("TRQSRAMT") / Val(Exc.Text)) Then
                On Error Resume Next
                TRet.Text = Format((.Item("TRRETAMT") / Val(Exc.Text)) / ((.Item("TRRETAMT") / Val(Exc.Text)) + (.Item("TRQSRAMT") / Val(Exc.Text))) * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                TQS.Text = Format((.Item("TRQSRAMT") / Val(Exc.Text)) / ((.Item("TRRETAMT") / Val(Exc.Text)) + (.Item("TRQSRAMT") / Val(Exc.Text))) * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))

            End If
            If (SumINS > (.Item("TRRETAMT") / Val(Exc.Text)) + (.Item("TRQSRAMT") / Val(Exc.Text))) And (SumINS <= (.Item("TRCAPCTY") / Val(Exc.Text))) Then
                If SumINS <= (.Item("TRRETAMT") / Val(Exc.Text)) + (.Item("TRQSRAMT") / Val(Exc.Text)) + (.Item("TR1STAMT") / Val(Exc.Text)) Then
                    TRet.Text = Format((.Item("TRRETAMT") / Val(Exc.Text)) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    TQS.Text = Format((.Item("TRQSRAMT") / Val(Exc.Text)) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))

                    TFirstSup.Text = Format((SumINS - (.Item("TRRETAMT") / Val(Exc.Text)) - (.Item("TRQSRAMT") / Val(Exc.Text))) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                Else
                    TRet.Text = Format((.Item("TRRETAMT") / Val(Exc.Text)) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    TQS.Text = Format((.Item("TRQSRAMT") / Val(Exc.Text)) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    'If TP = 1 Then
                    TFirstSup.Text = Format(((.Item("TR1STAMT") / Val(Exc.Text))) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    TSecondSup.Text = Format((SumINS - ((.Item("TRCAPCTY") - .Item("TR2STAMT")) / Val(Exc.Text))) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))

                End If
            End If
            '-------- 2
            Dim m As Double
            m = ((.Item("TRCAPCTY") / Val(Exc.Text)) + (.Item("TR2STAMT") / Val(Exc.Text))) / cnt
            If SumINS > (.Item("TRCAPCTY") / Val(Exc.Text)) Then
                TRet.Text = Format((.Item("TRRETAMT") / Val(Exc.Text)) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) / cnt 'Retention
                TQS.Text = Format((.Item("TRQSRAMT") / Val(Exc.Text)) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) / cnt 'Quatashare

                TFirstSup.Text = Format(((.Item("TR1STAMT") / Val(Exc.Text))) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) / cnt '1st surplus
                TSecondSup.Text = Format(((.Item("TR2STAMT") / Val(Exc.Text))) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) / cnt '2nd surplus
                If Request("Sys") = "04" Or Request("Sys") = "05" Then
                    If SumINS - (.Item("TRCAPCTY") / Val(Exc.Text)) <= (.Item("TRLSAMT") / Val(Exc.Text)) And (Request("Sys") = "04" Or Request("Sys") = "05") And (.Item("TRCAPCTY") / cnt) <> 0 Then
                        TLineSlip.Text = Format(((SumINS - (.Item("TRCAPCTY") / Val(Exc.Text)))) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) / cnt 'lineslip
                    Else
                        If (.Item("TRCAPCTY") / Val(Exc.Text)) <> 0 Then
                            TLineSlip.Text = Format((.Item("TRLSAMT") / Val(Exc.Text)) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                            'TFac.Text = Format((SumINS - (.Item("TRCAPCTY") / Val(Exc.Text)) - (4000000 / Val(Exc.Text))) / SumINS * NET, "0.000\0") / Val(EndCnt.Text)
                            'TFac.Text = Format(NET - (CDbl(TRet.Value) + CDbl(TQS.Text) + CDbl(TSecondSup.Text) + CDbl(TFirstSup.Text) + CDbl(TLineSlip.Text)), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                        Else
                            'TFac.Text = Format((SumINS - (.Item("TRCAPCTY") / Val(Exc.Text))) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) / cnt
                        End If
                    End If
                Else
                    If SumINS >= m And FacRatio.Value = 0 Then
                        If SpRetRatio.Value = 0 And HF.Value = 0 Then
                            TFac.Text = Format(NET - (CDbl(TRet.Value) + CDbl(TQS.Text) + CDbl(TSecondSup.Text) + CDbl(TFirstSup.Text) + CDbl(TLineSlip.Text)), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) - SpRet.Value 'Format((SumINS - (.Item("TRCAPCTY") / Val(Exc.Text))) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) '/ Val(EndCnt.Text)
                        Else
                            'TFac.Text = Format(NET - (CDbl(TRet.Value) + CDbl(TQS.Text) + CDbl(TSecondSup.Text) + CDbl(TFirstSup.Text) + CDbl(TLineSlip.Text)), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) - SpRet.Value
                        End If
                    Else
                        'TFac.Text = F.Text
                    End If
                End If
            Else
            End If
            'TNetPrm.Text = NET
            'ExtPrm.Text = Format(ExtraValue, "###,#0.000")
            If WarFacRatio.Value <> 0 Then

            End If
        End With
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
        If Request("PolNo") = Request("OrderNo") Then
            'ExecSql("update PolicyFile Set EXTPRM=" & CDbl(ExtPrm.Text) & ",ExtraRate=163 where " _
            '                   & "  PolicyFile.OrderNo='" & Trim(Request("OrderNo")) & "' and PolicyFile.EndNo=" & Request("EndNo") _
            '                   & " and PolicyFile.LoadNo=" & Request("LoadNo") & " and PolicyFile.SubIns='" & Request("Sys") & "'" _
            '                   & " and PolicyFile.Branch='" & Request("Branch") & "'")
            ExecConn("delete netprm where " _
                              & "  PolNo='" & Trim(Request("OrderNo")) & "' and EndNo=" & Request("EndNo") _
                              & " and LoadNo=" & Request("LoadNo") & " and TP='" & Request("Sys") & "'" _
                              & " and Br='" & Request("Branch") & "'", Conn)
        Else
            ExecConn("update PolicyFile Set Financed=1 where " _
                                & "  PolicyFile.OrderNo='" & Trim(Request("OrderNo")) & "' and PolicyFile.EndNo=" & Request("EndNo") _
                                & " and PolicyFile.LoadNo=" & Request("LoadNo") & " and PolicyFile.SubIns='" & Request("Sys") & "'" _
                                & " and PolicyFile.Branch='" & Request("Branch") & "'", Conn)
        End If
    End Sub

End Class