Imports System.Data.SqlClient
Imports DevExpress.Web
Imports Microsoft.Reporting.WebForms

Partial Public Class DistPolicy
    Inherits Page

    Private Treaty As New DataSet
    Private DB, DB1 As New DataSet
    Private Lo() As String
    Private ExtraValue As Double = 0
    Private WExtraValue As Double = 0
    Private FExtraValue As Double = 0
    Private WFExtraValue As Double = 0
    Private stat As Boolean

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Lo = Session("LogInfo")
        Dim SsSQL, Ty As String
        Dim TyDigit As Integer
        If Not IsPostBack And Request("PolNo") = Request("OrderNo") Then
            'RTNPG
            GoBack.ClientVisible = True
            GoBack.PostBackUrl = Session("RTNPG")
        Else

        End If

        ExecConn("delete netprm where PolD='1900/01/01'", Conn)

        If Request("PolNo") = Request("OrderNo") And Request("Branch") <> "IW" Then
            ASPxButton4.Visible = False
            'ASPxButton5.Text = "احتساب القسط الإضافي"
        Else
            ASPxButton4.Visible = True
            AlertL0.Visible = False
            ExtPrm.Visible = False
        End If
        Dim DistHistory As New DataSet

        Dim DistHadpt = New SqlDataAdapter("SELECT Np.PolNo,ElectiveRatio,FacPolRef,100-isnull(sum(AcceptedShare),0) As RemainFac ,isnull(sum(AcceptedShare),0) As Sold " _
                    & "FROM FacClosingSlips right JOIN NetPrm as np on FacClosingSlips.PolRef=np.FacPolRef And FacClosingSlips.EndNo=np.EndNo And FacClosingSlips.LoadNo=np.LoadNo " _
                    & " where np.PolNo='" & Request("PolNo") & "' and np.EndNo=" & Request("EndNo") & " and np.LoadNo=" & Request("LoadNo") & "" _
                    & " group by FacPolRef,ElectiveRatio,Np.PolNo " _
                    & " having (100-isnull(sum(AcceptedShare),0) <= 100) And ElectiveRatio<> 0 order by 100-isnull(sum(AcceptedShare),0) ", Conn)

        DistHadpt.Fill(DistHistory)

        If DistHistory.Tables(0).Rows.Count <> 0 Then
            If DistHistory.Tables(0).Rows(0)("Sold") <> 0 Then
                FacRatio.Position = DistHistory.Tables(0).Rows(0)("ElectiveRatio")
                FacRatio.ClientEnabled = False
                SpRetRatio.ClientEnabled = False
                ASPxButton5.ClientEnabled = False
            Else

            End If
        End If
        If Request("PolNo") = Request("OrderNo") Then
            stat = False
        Else
            Dim dbadapter1 = New SqlDataAdapter("Select Stop from policyFile where PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") & " and LoadNo=" & Request("LoadNo") & "", Conn)
            Dim unused = dbadapter1.Fill(DB1)
            stat = DB1.Tables(0).Rows(0)("Stop")
        End If

        If Request("OrderNo") <> "" Then
            If Not IsPostBack Then
                If Request("Branch") <> "IW" Then
                    SsSQL = If(Request("Sys") = "BB",
                        DirectCast("select PolNo,CustName,Commision,CoverFrom,CoverTo,Stop,TOTPRM,PolicyFile.CustNo,IssuDate,PolicyFile.ExcRate,PolicyFile.Currency,PolicyFile.EndNo,PolicyFile.LoadNo,NetPRM,TOTPRM,CoverFrom,CoverTo,CoverType," _
                                       & "(select " & IIf(Request("Sys") = "MC" Or Request("Sys") = "OC", "Loads", "count(OrderNo)") _
                                       & " from " & GetGroupFile(Request("Sys")) & " as EndFile where EndFile.orderno=" & IIf(GetEndData(), "PolicyFile.OrderNo", "'" & GetFirstOrder() & "'") & " and " _
                                       & IIf(GetEndData(), " EndFile.EndNo=" & IIf(GetEndData(), "PolicyFile.EndNo", "0") & " and EndFile.LoadNo=PolicyFile.LoadNo and ", "") & " EndFile.SubIns=PolicyFile.SubIns) as EndCnt," _
                                       & IIf(Request("Sys") = "MC" Or Request("Sys") = "05", " WarPrm,", "") & " (select " & GetEndFile(Request("Sys")) & " from " & GetGroupFile(Request("Sys")) & " as EndFile_1 where EndFile_1.orderno=" _
                                       & IIf(GetEndData(), "PolicyFile.OrderNo", "'" & GetFirstOrder() & "'") & " and " _
                                       & IIf(GetEndData(), " EndFile_1.EndNo=" & IIf(GetEndData(), "PolicyFile.EndNo", "0") & " and EndFile_1.LoadNo=PolicyFile.LoadNo and", "") & " EndFile_1.SubIns=PolicyFile.SubIns) as SumIns from PolicyFile Inner Join CustomerFile" _
                                       & " on PolicyFile.CustNo=CustomerFile.CustNo" _
                                       & " inner Join " & GetGroupFile(Request("Sys")) & " on " & GetGroupFile(Request("Sys")) & ".OrderNo=" & IIf(GetEndData(), "PolicyFile.OrderNo", "'" & GetFirstOrder() & "'") _
                                       & " and " & IIf(GetEndData(), GetGroupFile(Request("Sys")) & ".EndNo=" & IIf(GetEndData(), "PolicyFile.EndNo", "0") & " and " _
                                       & GetGroupFile(Request("Sys")) & ".LoadNo=PolicyFile.LoadNo and ", "") & GetGroupFile(Request("Sys")) & ".SubIns=PolicyFile.SubIns" _
                                       & " where PolicyFile.OrderNo='" & Trim(Request("OrderNo")) & "' and PolicyFile.EndNo=" & Request("EndNo") _
                                       & " and PolicyFile.LoadNo=" & Request("LoadNo") & " and PolicyFile.SubIns='" & Request("Sys") & "'" _
                                       & " and PolicyFile.Branch='" & Request("Branch") & "'", String),
                        DirectCast("select PolNo,CustName,Commision," & IIf(Request("Sys") = "ER" Or Request("Sys") = "CR", "ProjectFile.WorkStart As CoverFrom,ProjectFile.WorkEnd As CoverTo,", "CoverFrom ,CoverTo ,") & " Stop,TOTPRM,PolicyFile.CustNo,IssuDate,PolicyFile.ExcRate,PolicyFile.Currency,PolicyFile.EndNo,PolicyFile.LoadNo,NetPRM,TOTPRM,CoverFrom,CoverTo,CoverType," _
                                       & "(select " & IIf(Request("Sys") = "MC" Or Request("Sys") = "MB" Or Request("Sys") = "MA" Or Request("Sys") = "OC", "Loads", "count(OrderNo)") _
                                       & " from " & GetGroupFile(Request("Sys")) & " as EndFile where EndFile.orderno=" & IIf(GetEndData(), "PolicyFile.OrderNo", "'" & GetFirstOrder() & "'") & " and " _
                                       & IIf(GetEndData(), " EndFile.EndNo=" & IIf(GetEndData(), "PolicyFile.EndNo", "0") & " and EndFile.LoadNo=PolicyFile.LoadNo and ", "") & " EndFile.SubIns=PolicyFile.SubIns) as EndCnt," _
                                       & IIf(Request("Sys") = "MC" Or Request("Sys") = "MB" Or Request("Sys") = "MA" Or Request("Sys") = "OC", " WarPrm,", "") & " (select sum(" & GetEndFile(Request("Sys")) & ") from " & GetGroupFile(Request("Sys")) & " as EndFile_1 where EndFile_1.orderno=" _
                                       & IIf(GetEndData(), "PolicyFile.OrderNo", "'" & GetFirstOrder() & "'") & " and " _
                                       & IIf(GetEndData(), " EndFile_1.EndNo=" & IIf(GetEndData(), "PolicyFile.EndNo", "0") & " and EndFile_1.LoadNo=PolicyFile.LoadNo and", "") & " EndFile_1.SubIns=PolicyFile.SubIns) as SumIns from PolicyFile Inner Join CustomerFile" _
                                       & " on PolicyFile.CustNo=CustomerFile.CustNo" _
                                       & " inner Join " & GetGroupFile(Request("Sys")) & " on " & GetGroupFile(Request("Sys")) & ".OrderNo=" & IIf(GetEndData(), "PolicyFile.OrderNo", "'" & GetFirstOrder() & "'") _
                                       & " and " & IIf(GetEndData(), GetGroupFile(Request("Sys")) & ".EndNo=" & IIf(GetEndData(), "PolicyFile.EndNo", "0") & " and " _
                                       & GetGroupFile(Request("Sys")) & ".LoadNo=PolicyFile.LoadNo and ", "") & GetGroupFile(Request("Sys")) & ".SubIns=PolicyFile.SubIns" _
                                       & " where PolicyFile.OrderNo='" & Trim(Request("OrderNo")) & "' and PolicyFile.EndNo=" & Request("EndNo") _
                                       & " and PolicyFile.LoadNo=" & Request("LoadNo") & " and PolicyFile.SubIns='" & Request("Sys") & "'" _
                                       & " and PolicyFile.Branch='" & Request("Branch") & "'", String))
                Else
                    SsSQL = "select PolNo,CustName,policyfile.Commision,policyfile.CoverFrom,'01' As CoverType,policyfile.CoverTo,TOTPRM,PolicyFile.CustNo,IssuDate,PolicyFile.ExcRate,PolicyFile.Currency,PolicyFile.EndNo,PolicyFile.LoadNo,NetPRM,TOTPRM, " _
                            & "(select count(OrderNo) from LocalAcceptance as EndFile where EndFile.orderno=PolicyFile.OrderNo and EndFile.EndNo=PolicyFile.EndNo and EndFile.LoadNo=PolicyFile.LoadNo and  EndFile.SubIns=PolicyFile.SubIns) as EndCnt, " _
                            & "(select AcceptedSumIns from LocalAcceptance as EndFile_1 where EndFile_1.orderno=PolicyFile.OrderNo and  EndFile_1.EndNo=PolicyFile.EndNo and EndFile_1.LoadNo=PolicyFile.LoadNo and EndFile_1.SubIns=PolicyFile.SubIns) as SumIns " _
                            & "from PolicyFile " _
                            & "Inner Join CustomerFile on PolicyFile.CustNo=CustomerFile.CustNo " _
                            & "inner Join LocalAcceptance on LocalAcceptance.OrderNo=PolicyFile.OrderNo and LocalAcceptance.EndNo=PolicyFile.EndNo and LocalAcceptance.LoadNo=PolicyFile.LoadNo and LocalAcceptance.SubIns=PolicyFile.SubIns " _
                            & "where PolicyFile.OrderNo='" & Trim(Request("OrderNo")) & "' and PolicyFile.EndNo=" & Request("EndNo") & " and PolicyFile.LoadNo=" & Request("LoadNo") & " and PolicyFile.SubIns=LocalAcceptance.SubIns"
                End If
                Dim dbadapter = New SqlDataAdapter(SsSQL, Conn)

                Dim unused1 = dbadapter.Fill(DB)
                Exc.Text = DB.Tables(0).Rows(0)("ExcRate")
                PolNo.Text = If(Request("PolNo") = Request("OrderNo"), Request("PolNo"), DirectCast(DB.Tables(0).Rows(0)("PolNo"), String))
                EndNo.Text = DB.Tables(0).Rows(0)("EndNo")
                LoadNo.Text = DB.Tables(0).Rows(0)("LoadNo")
                CustName.Text = DB.Tables(0).Rows(0)("CustName")
                CoverFrom.Value = Format(DB.Tables(0).Rows(0)("CoverFrom"), "yyyy-MM-dd")
                CoverTo.Value = Format(DB.Tables(0).Rows(0)("CoverTo"), "yyyy-MM-dd")
                'DB.Tables(0).Rows(0)("IssuDate").IsNull(0)
                IssuDate.Text = If(Request("PolNo") = Request("OrderNo") And Request("Branch") <> "IW", "1900-01-01", Format(DB.Tables(0).Rows(0)("IssuDate"), "yyyy-MM-dd"))
                SumInsured.Text = Format(DB.Tables(0).Rows(0)("SumIns"), "###,#0.000")
                NetPRM.Text = Format(DB.Tables(0).Rows(0)("NetPRM"), "###,#0.000")
                HNetPRM.Text = Format(DB.Tables(0).Rows(0)("NetPRM"), "###,#0.000")
                Comission.Text = 0 ' Format(DB.Tables(0).Rows(0)("NetPRM") * IIf(DB.Tables(0).Rows(0).IsNull("Commision"), 0, DB.Tables(0).Rows(0)("Commision")) / 100, "0.000")
                EndCnt.Text = IIf(DB.Tables(0).Rows(0)("EndCnt") = 0, 1, DB.Tables(0).Rows(0)("EndCnt"))
                CustNo.Text = DB.Tables(0).Rows(0)("CustNo")
                TOTPRM.Text = DB.Tables(0).Rows(0)("TOTPRM")
                Cur.Text = DB.Tables(0).Rows(0)("Currency")
                'Cfrom.Text = Format(DB.Tables(0).Rows(0)("CoverFrom"), "yyyy/MM/dd")
                'Cto.Text = Format(DB.Tables(0).Rows(0)("CoverTo"), "yyyy/MM/dd")
                If Request("PolNo") = Request("OrderNo") And Request("Branch") <> "IW" Then
                    Ty = Mid(PolNo.Text, 1, 4)
                    TyDigit = Val(Mid(PolNo.Text, 1, 4))
                Else
                    Ty = IIf(Request("Branch") = "IW", Mid(PolNo.Text, 6, 4), Mid(PolNo.Text, 5, 4))
                    TyDigit = IIf(Request("Branch") = "IW", Val(Mid(PolNo.Text, 6, 4)), Val(Mid(PolNo.Text, 5, 4)))
                End If
                'Dim TotPRM As Double
                Select Case Request("Sys")

                    Case "MC", "MB", "MA", "OC"
                        If Request("Branch") <> "IW" Then
                            TreatyNo.Text = Request("Sys") + IIf(Len(CStr(DB.Tables(0).Rows(0)("CoverType"))) = 1, "0" + CStr(DB.Tables(0).Rows(0)("CoverType")), CStr(DB.Tables(0).Rows(0)("CoverType"))) + Ty
                            If EndNo.Text = 0 Then
                                War.Text = DB.Tables(0).Rows(0)("WarPrm")
                                HWar.Text = DB.Tables(0).Rows(0)("WarPrm")
                            Else
                                War.Text = WarPrmCalc(DB.Tables(0).Rows(0)("WarPrm"))
                                HWar.Text = WarPrmCalc(DB.Tables(0).Rows(0)("WarPrm"))
                            End If
                        Else
                            War.Value = 0
                            HWar.Value = 0
                        End If
                    Case Else
                        TreatyNo.Text = Request("Sys") + IIf(Len(CStr(DB.Tables(0).Rows(0)("CoverType"))) = 1, "0" + CStr(DB.Tables(0).Rows(0)("CoverType")), CStr(DB.Tables(0).Rows(0)("CoverType"))) + Ty
                End Select
                TreatyNo.Text = Request("Sys") + IIf(Len(CStr(DB.Tables(0).Rows(0)("CoverType"))) = 1, "0" + CStr(DB.Tables(0).Rows(0)("CoverType")), CStr(DB.Tables(0).Rows(0)("CoverType"))) + Ty
                If Val(War.Text) <> 0 Then
                    'NetPRM.Text = CDbl(NetPRM.Text) - Val(War.Text)
                Else
                    War.Text = "0"
                    HWar.Text = "0"
                End If
                DistNet.Text = Format(NetPRM.Text - Val(Comission.Text) - Val(War.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                HDistNet.Text = Format(NetPRM.Text - Val(Comission.Text) - Val(War.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))

RefreshData:    Dim TreatyData = New SqlDataAdapter("select * from TRREGFLE where TreatyNo='" & TreatyNo.Text & "'", Conn)
                Dim unused2 = TreatyData.Fill(Treaty)
                If Treaty.Tables(0).Rows.Count = 0 Then
                    MsgBob(Me, "لم يتم إدخال حدود الاتفاقية لهذا النوع، يرجى الاتصال بإدارة إعادة التأمين ")

                    ExecConn("INSERT INTO [TRREGFLE]([TreatyNo],[Descrip],[Acctype],[Portfolio],[ReserveR],[TRSYSDTE],[TRINSDTE],[TREXPDTE],[TRCAPCTY],[TRRETAMT],[TRQSRAMT]
,[TRQSRCOM],[TR1STAMT],[TR1STCOM],[TR2STAMT],[TR2STCOM],[TRLQSRCOM],[TRL1STCOM],[TRL2STCOM],[TRWQSRCOM],[TRW1STCOM],[InterestRRes],[TRLSAMT],[TRLSCOMM])
 VALUES('" & Trim(TreatyNo.Text) & "','" & "NO Treaty Entry " & Request("Sys") & "/" & Ty & "'," & 2 & "," & 1 & "," _
     & 0 & ",'" & Right(TreatyNo.Text, 4) & "/01" & "/01" & "','" & Right(TreatyNo.Text, 4) & "/01" & "/01" & "','" & Right(TreatyNo.Text, 4) & "/12" & "/31" & "'," _
     & 0 & "," & 0 & "," & 0 & "," & 0 & "," & 0 & "," & 0 & "," & 0 & "," & 0 & "," & 0 & "," & 0 & "," _
     & 0 & "," & 0 & "," & 0 & "," & 0 & "," & 0 & "," & 0 & ")", Conn)
                    GoTo RefreshData
                Else

                End If
                Dim IslocalAcc As Double
                If Request("Branch") <> "IW" Then
                    IslocalAcc = 1
                Else
                    IslocalAcc = Treaty.Tables(0).Rows(0)("LAR") / 100
                End If
                Capacity.Value = Format(Treaty.Tables(0).Rows(0)("TRCAPCTY") * IslocalAcc, IIf(Exc.Text = 1, "###,#0.000", "###,#0.000"))
                Ret.Value = Format(Treaty.Tables(0).Rows(0)("TRRETAMT") * IslocalAcc, IIf(Exc.Text = 1, "###,#0.000", "###,#0.000"))
                QS.Value = Format(Treaty.Tables(0).Rows(0)("TRQSRAMT") * IslocalAcc, IIf(Exc.Text = 1, "###,#0.000", "###,#0.000"))
                FirstSup.Value = Format(Treaty.Tables(0).Rows(0)("TR1STAMT") * IslocalAcc, IIf(Exc.Text = 1, "###,#0.000", "###,#0.000"))
                SecondSup.Value = Format(Treaty.Tables(0).Rows(0)("TR2STAMT") * IslocalAcc, IIf(Exc.Text = 1, "###,#0.000", "###,#0.000"))
                LineSlip.Value = Format(Treaty.Tables(0).Rows(0)("TRLSAMT") * IslocalAcc, IIf(Exc.Text = 1, "###,#0.000", "###,#0.000"))
                TRQSRCOM.Value = Format(Treaty.Tables(0).Rows(0)("TRQSRCOM") * IslocalAcc, IIf(Exc.Text = 1, "###,#0.00", "###,#0.000"))
                TRWQSRCOM.Value = Format(Treaty.Tables(0).Rows(0)("TRWQSRCOM") * IslocalAcc, IIf(Exc.Text = 1, "###,#0.00", "###,#0.000"))
                TR1STCOM.Value = Format(Treaty.Tables(0).Rows(0)("TR1STCOM") * IslocalAcc, IIf(Exc.Text = 1, "###,#0.00", "###,#0.000"))
                TRW1STCOM.Value = Format(Treaty.Tables(0).Rows(0)("TRW1STCOM") * IslocalAcc, IIf(Exc.Text = 1, "###,#0.00", "###,#0.000"))
                TR2STCOM.Value = Format(Treaty.Tables(0).Rows(0)("TR2STCOM") * IslocalAcc, IIf(Exc.Text = 1, "###,#0.00", "###,#0.000"))
                'TRLSCOMM.Text = Format(Treaty.Tables(0).Rows(0)("TRLSCOMM"), IIf(Exc.Text = 1, "###,#0.00", "###,#0.00"))
                'IIf(IsDBNull(Treaty.Tables(0).Rows(0)("TRLSCOMM")), 0, Treaty.Tables(0).Rows(0)("TRLSCOMM"))
                TRLSCOMM.Value = IIf(IsDBNull(Treaty.Tables(0).Rows(0)("TRLSCOMM")) * IslocalAcc, 0.00, Treaty.Tables(0).Rows(0)("TRLSCOMM"))
                FacComm.Value = Format(10, IIf(Exc.Text = 1, "###,#0.000", "###,#0.000"))
                If (CDbl(Treaty.Tables(0).Rows(0)("TRCAPCTY")) * IslocalAcc) + (CDbl(Treaty.Tables(0).Rows(0)("TRLSAMT")) * IslocalAcc) >= (Math.Abs(CDbl(DB.Tables(0).Rows(0)("SumIns")) * Val(Exc.Text)) / Val(EndCnt.Text)) Then
                    DistNet.Value = Format(NetPRM.Value - Comission.Value - War.Value, "###,#0.00")
                    HDistNet.Value = Format(NetPRM.Value - Comission.Value - War.Value, "###,#0.00")
                    F.Value = 0
                    HF.Value = 0
                    HFM.Value = 0
                Else
                    DistNet.Value = Format(Val((Treaty.Tables(0).Rows(0).Item("TRCAPCTY") * IslocalAcc) + (Treaty.Tables(0).Rows(0).Item("TRLSAMT") * IslocalAcc / Val(Exc.Text))) / Math.Abs(DB.Tables(0).Rows(0)("SumIns")) * 100 * (DB.Tables(0).Rows(0)("NetPRM") - CDbl(Comission.Value)) / 100, "###,#0.00")
                    HDistNet.Value = Format(Val((Treaty.Tables(0).Rows(0).Item("TRCAPCTY") * IslocalAcc) + (Treaty.Tables(0).Rows(0).Item("TRLSAMT") * IslocalAcc / Val(Exc.Text))) / Math.Abs(DB.Tables(0).Rows(0)("SumIns")) * 100 * (DB.Tables(0).Rows(0)("NetPRM") - CDbl(Comission.Value)) / 100, "###,#0.00")
                    F.Value = NetPRM.Value - DistNet.Value
                    HF.Value = NetPRM.Value - DistNet.Value ', IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    HFM.Value = NetPRM.Value - DistNet.Value ', IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    FacRatio.PositionStart = 100 - (((Treaty.Tables(0).Rows(0).Item("TRCAPCTY") * IslocalAcc) + (Treaty.Tables(0).Rows(0).Item("TRLSAMT") * IslocalAcc)) / Exc.Value / Math.Abs(DB.Tables(0).Rows(0)("SumIns")) * 100)
                    FacRatio.MinValue = FacRatio.PositionStart
                End If
                DistRe()
            End If

        End If
        AlertL.Visible = Capacity.Value + CDbl(LineSlip.Value) < SumInsured.Value * Val(Exc.Value)

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
        Dim EndFile As New DataSet
        If Request("Branch") = "IW" Then
            Dim EndData = New SqlDataAdapter("select AcceptedPremium As Premium,AcceptedSumIns as SumIns from LocalAcceptance where LocalAcceptance.OrderNo='" & Request("OrderNo") & "'" _
                                   & " and LocalAcceptance.EndNo=" & Request("EndNo") & " and " _
                                   & " LocalAcceptance.LoadNo=" & Request("LoadNo") & " and LocalAcceptance.SubIns='" & Request("Sys") & "'", Conn)
            EndData.Fill(EndFile)
        Else
            Dim EndData = New SqlDataAdapter("select " & IIf(IsGroupedSys(Request("Sys")), "Premium,", "") & "" & GetEndFile(Request("Sys")) & " as SumIns from " & GetGroupFile(Request("Sys")) & " where " & GetGroupFile(Request("Sys")) & ".OrderNo='" & Request("OrderNo") & "'" _
                                   & " and " & GetGroupFile(Request("Sys")) & ".EndNo=" & Request("EndNo") & " and " _
                                   & GetGroupFile(Request("Sys")) & ".LoadNo=" & Request("LoadNo") & " and " & GetGroupFile(Request("Sys")) & ".SubIns='" & Request("Sys") & "'", Conn)
            EndData.Fill(EndFile)
        End If

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
        Dim unused1 = DistData.Fill(Dist)
        ExtraValue = 0
        If Dist.Tables(0).Rows.Count = 0 Then
            For i As Integer = 1 To Val(EndCnt.Text)
                If EndFile.Tables(0).Rows.Count <> Val(EndCnt.Text) Then
                    If Val(War.Text) <> 0 And i = 1 Then War.Text = War.Text
                    'If Request("Sys") = "04" Or Request("Sys") = "05" Then
                    'DistPolicy(CDbl(NetPRM.Text) / Val(EndCnt.Text), CDbl(SumInsured.Text) / Val(EndCnt.Text))
                    'Else
                    DistPolicy((NetPRM.Text / Val(EndCnt.Text)) - (War.Text / Val(EndCnt.Text)) - (Val(Comission.Text) / Val(EndCnt.Text)), SumInsured.Text / Val(EndCnt.Text))
                    'End If
                Else
                    'DistPolicy((CDbl(DistNet.Text) - Val(Comission.Text) - Val(War.Text) / Val(EndCnt.Text)) * EndFile.Tables(0).Rows(i - 1)("SumIns") / CDbl(SumInsured.Text), EndFile.Tables(0).Rows(i - 1)("SumIns"))
                    If Request("Sys") = "HL" Or Request("Sys") = "FR" Then
                        DistPolicy(Format(EndFile.Tables(0).Rows(i - 1)("Premium"), "###,#0.000"), Format(EndFile.Tables(0).Rows(i - 1)("SumIns"), "###,#0.000"))
                    Else
                        DistPolicy(Format((NetPRM.Text - Val(Comission.Text) - (Val(War.Text) / Val(EndCnt.Text))) * EndFile.Tables(0).Rows(i - 1)("SumIns") / CDbl(SumInsured.Text), "###,#0.000"), Format(EndFile.Tables(0).Rows(i - 1)("SumIns"), "###,#0.000"))
                    End If

                End If
                'Dim Etra As Double = 1.63
                'Dim AccTP As Int16 = IIf(stat, -1, 1) ' IIf(CDbl(NetPRM.Text) >= 0, 1, -1)
                Dim Temp As Double = TNetPrm.Text - (CDbl(TFac.Value) + SpRet.Text + TRet.Text)
                Dim Wtemp As Double = (War.Text - wFac.Text) / Val(EndCnt.Text)
                Dim Ttemp As Double = TFac.Value
                Dim WTtemp As Double = wFac.Text
                ExtraValue += Format(Temp - (Temp * IIf(CDbl(TRQSRCOM.Text) = 0, CDbl(TR1STCOM.Text), CDbl(TRQSRCOM.Text)) / 100), "###,#0.000")
                WExtraValue += Format(Wtemp - (Wtemp * IIf(CDbl(TRWQSRCOM.Text) = 0, CDbl(TRW1STCOM.Text), CDbl(TRWQSRCOM.Text)) / 100), "###,#0.000")
                FExtraValue = Format(FExtraValue + (Ttemp - (Ttemp * (FacComm.Text / 100))))
                WFExtraValue = Format(WFExtraValue + (WTtemp - (WTtemp * 0.1)))
                ExtPrm.Text = ExtraValue + FExtraValue + WExtraValue + WFExtraValue

                'If (TRet.Value + TQS.Value + TFirstSup.Value + TFac.Value + TSpRet.Value) <> TNetPrm.Value Then
                '    TRet.Value += TNetPrm.Value - (TRet.Value + TQS.Value + TFirstSup.Value) + TFac.Value + TSpRet.Value
                'End If

                ExecConn("INSERT INTO [NetPrm]([PolNo],[LoadNo],[EndNo],[CustNo],[InsAmt],[Net],[War],[Total],[PolD],[Cur],[Exc],[DFm],[DTo]" _
                        & ",[Tp],[Br],[Treaty],[Amount],[Qs],[FirsSup],[SecondSup],[Elective],[LineSlip],[Qsw],[FirsSupw],[Electivew],[Comission],[UserName],[SpecialRet],[Type]) values('" _
                        & PolNo.Text & "'," _
                        & LoadNo.Text & "," _
                        & EndNo.Text & "," _
                        & CustNo.Text & "," _
                        & IIf(Request("Sys") <> "BB", TSumIns.Value, IIf(NetPRM.Value >= 0, 1, -1) * TSumIns.Value) & "," _
                        & TNetPrm.Value & "," _
                        & Val(War.Value) / Val(EndCnt.Value) & "," _
                        & TOTPRM.Value & "," _
                        & "CONVERT(DATETIME,'" & Format(CDate(IssuDate.Text), "yyyy-MM-dd") & " 00:00:00',102)," _
                        & Cur.Text & "," _
                        & Val(Exc.Text) & ",'" _
                        & "" & IIf(Request("Sys") = "MC" Or Request("Sys") = "MB" Or Request("Sys") = "MA", IssuDate.Value, CoverFrom.Value) & "','" _
                        & "" & CoverTo.Value & "','" _
                        & Request("Sys") & "','" _
                        & Request("Branch") & "','" _
                        & TreatyNo.Text & "'," _
                        & CDbl(TRet.Value) & "," _
                        & CDbl(TQS.Value) & "," _
                        & CDbl(TFirstSup.Value) & "," _
                        & CDbl(TSecondSup.Value) & "," _
                        & IIf(NetPRM.Value >= 0 And TFac.Value >= 0, 1 * TFac.Value, IIf(NetPRM.Value >= 0, 1, -1) * TFac.Value) & "," _
                        & CDbl(TLineSlip.Value) & "," _
                        & CDbl(wQS.Value) & "," _
                        & CDbl(wFirstSup.Value) & "," _
                        & CDbl(wFac.Value) & "," _
                        & (Comission.Value / Val(EndCnt.Value)) & ",'" _
                        & Lo(0) & "'," _
                        & CDbl(TSpRet.Value) & ",'Pol')", Conn)
                'IIf(Request("Sys") <> "04", CDbl(SpRet.Text), CDbl(SpRet.Text) / Val(EndCnt.Text))
                ' & IIf(SpRetRatio.Value <> 0 And Request("Sys") <> "04", CDbl(HNetPRM.Text), IIf(Request("Sys") <> "04", CDbl(TNetPrm.Text), (CDbl(HNetPRM.Text) - CDbl(War.Text) - CDbl(Comission.Text)) / Val(EndCnt.Text))) & "," _
            Next
        End If
        SqlDataSource1.SelectParameters(0).DefaultValue = PolNo.Text
        SqlDataSource1.SelectParameters(1).DefaultValue = EndNo.Text
        SqlDataSource1.SelectParameters(2).DefaultValue = LoadNo.Text
        ASPxGridView1.DataBind()

        If Request("Report") = 1 Then
            Dim SelPolicyRep = "/IMSValidate/" & PolRep(Request("sys"))  'SelectReport(Mid(Request("PolNo"), 12, 2), Request("sys"))
            Parm = Array.CreateInstance(GetType(ReportParameter), IIf(Request("sys") = "OC", 4, 3))
            SetRepPm("PolicyNo", False, GenArray(Request("OrderNo")), Parm, 0)
            SetRepPm("EndNo", False, GenArray(Request("EndNo")), Parm, 1)
            SetRepPm("Sys", False, GenArray(Request("sys")), Parm, 2)

            If Request("sys") = "OC" Then SetRepPm("LoadNo", False, GenArray(LoadNo.Text), Parm, 3)
            'If Request("sys") <> "06" Then Me.Session.Add("Parms", Parm)
            Session.Add("Parms", Parm)
            'ClientScript.RegisterStartupScript(GetType(), "OpenWindow", "<script> window.open('../OutPutManagement/Preview.aspx?Report=" & SelPolicyRep & "','_self'); </script>")
            ASPxWebControl.RedirectOnCallback("~/Reporting/PreviewPDF.aspx?Report=" & SelPolicyRep & "")
        End If

        'Dim temp As Double = CDbl(TSumIns.Text) / CDbl(Capacity.Text) * (CDbl(HNetPRM.Text) - CDbl(War.Text) - CDbl(Comission.Text))
        'ExtraValue = Format(ExtraValue + (temp / Val(EndCnt.Text) - (temp / Val(EndCnt.Text) * (CDbl(TRQSRCOM.Text) / 100))) * 1.83, "###,#0.000")
        'ExtPrm.Text = ExtraValue
    End Sub

    Protected Sub DistPolicy(NET As Double, SumINS As Double)

        Dim TreatyData = New SqlDataAdapter("select * from TRREGFLE where TreatyNo='" & TreatyNo.Value.ToString.Trim & "'", Conn)

        Dim unused = TreatyData.Fill(Treaty)

        Dim IslocalAcc As Double
        If Request("Branch") <> "IW" Then
            IslocalAcc = 1
        Else
            IslocalAcc = Treaty.Tables(0).Rows(0)("LAR") / 100
        End If

        Dim WsI As Double = SumINS

        TSumIns.Value = SumINS
        TNetPrm.Value = NET

        TRet.Text = "0"
        TQS.Text = "0"
        TFirstSup.Text = "0"
        TSecondSup.Text = "0"
        TFac.Value = 0
        TLineSlip.Text = "0"
        wQS.Text = "0"
        wFirstSup.Text = "0"
        wFac.Text = "0"
        TSpRet.Value = 0
        Dim cnt = If(IsGroupedSys(Request("Sys")) Or Request("Sys") = "MC" Or Request("Sys") = "MB" Or Request("Sys") = "MA" Or Request("Sys") = "OC",
            1,
            EndCnt.Value)
        Dim Fratio As Double

        If (Treaty.Tables(0).Rows(0)("TRCAPCTY") * IslocalAcc) + Treaty.Tables(0).Rows(0)("TRLSAMT") * IslocalAcc >= Math.Abs(SumINS) * Exc.Value / cnt Then
        Else
            Fratio = Format(100 - ((Treaty.Tables(0).Rows(0).Item("TRCAPCTY") * IslocalAcc + Treaty.Tables(0).Rows(0).Item("TRLSAMT") * IslocalAcc) / Exc.Value / Math.Abs(SumINS) * 100), "###,#0.00")

        End If

        If Fratio = Format(FacRatio.MinValue, "###,#0.00") Then
        Else
            If (Treaty.Tables(0).Rows(0)("TRCAPCTY") * IslocalAcc) + Treaty.Tables(0).Rows(0)("TRLSAMT") * IslocalAcc >= Math.Abs(SumINS) * Val(Exc.Text) / cnt Then
                'DistNet.Value = NetPRM.Value - Comission.Value - War.Value
                'HDistNet.Value = NetPRM.Value - Comission.Value - War.Value
                DistNet.Value = Format(NetPRM.Value - Comission.Value - War.Value, "###,#0.00")
                HDistNet.Value = Format(NetPRM.Value - Comission.Value - War.Value, "###,#0.00")
                F.Value = 0
                HF.Value = 0
                HFM.Value = 0
                FacRatio.PositionStart = 0
                FacRatio.MinValue = FacRatio.PositionStart
            Else
                'DistNet.Value = Val((Treaty.Tables(0).Rows(0).Item("TRCAPCTY") * IslocalAcc + Treaty.Tables(0).Rows(0).Item("TRLSAMT") * IslocalAcc) / Val(Exc.Text)) / Math.Abs(SumINS) * 100 * (NET - Comission.Value) / 100
                'HDistNet.Value = Val((Treaty.Tables(0).Rows(0).Item("TRCAPCTY") * IslocalAcc + Treaty.Tables(0).Rows(0).Item("TRLSAMT") * IslocalAcc) / Val(Exc.Text)) / Math.Abs(SumINS) * 100 * (NET - Comission.Value) / 100
                DistNet.Value = Format(Val((Treaty.Tables(0).Rows(0).Item("TRCAPCTY") * IslocalAcc) + (Treaty.Tables(0).Rows(0).Item("TRLSAMT") * IslocalAcc / Val(Exc.Text))) / Math.Abs(SumINS) * 100 * (NET - CDbl(Comission.Value)) / 100, "###,#0.00")
                HDistNet.Value = Format(Val((Treaty.Tables(0).Rows(0).Item("TRCAPCTY") * IslocalAcc) + (Treaty.Tables(0).Rows(0).Item("TRLSAMT") * IslocalAcc / Val(Exc.Text))) / Math.Abs(SumINS) * 100 * (NET - CDbl(Comission.Value)) / 100, "###,#0.00")
                F.Value = NetPRM.Value - DistNet.Value
                HF.Value = NetPRM.Value - DistNet.Value ', IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                HFM.Value = NetPRM.Value - DistNet.Value ', IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                FacRatio.PositionStart = 100 - (((Treaty.Tables(0).Rows(0).Item("TRCAPCTY") * IslocalAcc) + Treaty.Tables(0).Rows(0).Item("TRLSAMT") * IslocalAcc) / Exc.Value / Math.Abs(SumINS) * 100)
                FacRatio.MinValue = FacRatio.PositionStart
            End If
        End If
        'And SpRetRatio.Value = 0
        'If FacRatio.Value <> FacRatio.MinValue Then
        TRet.Text = "0"
        TQS.Text = "0"
        TFirstSup.Text = "0"
        TSecondSup.Text = "0"
        TFac.Value = 0
        TLineSlip.Text = "0"
        'wQS.Text = "0"
        ' wFirstSup.Text = "0"
        'wFac.Text = "0"
        'SumINS = CDbl(DistNet.Text) / Val(TNetPrm.Text) * SumINS
        'TFac.Text = Format((NET - SpRet.Value), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) ' - CDbl(DistNet.Text)
        TFac.Value = FacRatio.Value * NET / 100 ' - CDbl(DistNet.Text)
        TSpRet.Value = SpRetRatio.Value * TFac.Value / 100
        TFac.Value = Math.Abs(TSpRet.Value - TFac.Value)
        NET -= Math.Round(NET * FacRatio.Value / 100, 3)
        SumINS -= SumINS * FacRatio.Value / 100
        SumINS = Format(SumINS, "###,#0.00")
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
                wFac.Text = War.Text / Val(EndCnt.Text)
            Else
                wFac.Text = DistWar.Text / Val(EndCnt.Text)
            End If
            wQS.Text = DistWar.Text / cnt
        End If

        With Treaty.Tables(0).Rows(0)

            '------- 1
            If Math.Abs(SumINS) <= (.Item("TRRETAMT") * IslocalAcc / Val(Exc.Value)) + (.Item("TRQSRAMT") * IslocalAcc / Val(Exc.Value)) Then
                'On Error Resume Next
                TRet.Value = .Item("TRRETAMT") * IslocalAcc / Exc.Value / ((.Item("TRRETAMT") * IslocalAcc / Exc.Value) + (.Item("TRQSRAMT") * IslocalAcc / Exc.Value)) * NET

                If TRet.Value.ToString = "NaN" Or TRet.Value.ToString = "ليس رقمًا" Then
                    TRet.Value = 0
                Else

                End If

                TQS.Value = .Item("TRQSRAMT") * IslocalAcc / Val(Exc.Value) / ((.Item("TRRETAMT") * IslocalAcc / Val(Exc.Value)) + (.Item("TRQSRAMT") * IslocalAcc / Val(Exc.Value))) * NET

                If TQS.Value.ToString = "NaN" Or TQS.Value.ToString = "ليس رقمًا" Then
                    TQS.Value = 0
                Else

                End If
            End If

            If Math.Abs(SumINS) > (.Item("TRRETAMT") * IslocalAcc / Val(Exc.Value)) + (.Item("TRQSRAMT") * IslocalAcc / Val(Exc.Value)) And (Math.Abs(SumINS) <= (.Item("TRCAPCTY") * IslocalAcc / Val(Exc.Value))) Then
                If Math.Abs(SumINS) <= (.Item("TRRETAMT") * IslocalAcc / Val(Exc.Value)) + (.Item("TRQSRAMT") * IslocalAcc / Val(Exc.Value)) + (.Item("TR1STAMT") * IslocalAcc / Val(Exc.Value)) Then
                    TRet.Value = .Item("TRRETAMT") * IslocalAcc / Exc.Value / Math.Abs(SumINS) * NET
                    TQS.Value = .Item("TRQSRAMT") * IslocalAcc / Exc.Value / Math.Abs(SumINS) * NET

                    If TRet.Value.ToString = "NaN" Or TRet.Value.ToString = "ليس رقمًا" Then
                        TRet.Value = 0
                    Else

                    End If

                    If TQS.Value.ToString = "NaN" Or TQS.Value.ToString = "ليس رقمًا" Then
                        TQS.Value = 0
                    Else

                    End If
                    TFirstSup.Value = (Math.Abs(SumINS) - (.Item("TRRETAMT") * IslocalAcc / Exc.Value) - (.Item("TRQSRAMT") * IslocalAcc / Exc.Value)) / Math.Abs(SumINS) * NET
                Else
                    TRet.Value = .Item("TRRETAMT") * IslocalAcc / Val(Exc.Value) / Math.Abs(SumINS) * NET
                    TQS.Value = .Item("TRQSRAMT") * IslocalAcc / Val(Exc.Value) / Math.Abs(SumINS) * NET
                    If TRet.Value.ToString = "NaN" Or TRet.Value.ToString = "ليس رقمًا" Then
                        'If Not IsNumeric(TRet.Value) Then
                        TRet.Value = 0
                    Else

                    End If

                    'If TQS.Value.ToString = "NaN" Or TQS.Value.ToString = "ليس رقمًا" Then
                    'If Not IsNumeric(TQS.Value) Then
                    If TQS.Value.ToString = "NaN" Or TQS.Value.ToString = "ليس رقمًا" Then
                        TQS.Value = 0
                    Else

                    End If
                    TFirstSup.Value = .Item("TR1STAMT") * IslocalAcc / Val(Exc.Value) / Math.Abs(SumINS) * NET
                    TSecondSup.Value = (Math.Abs(SumINS) - ((.Item("TRCAPCTY") * IslocalAcc - .Item("TR2STAMT") * IslocalAcc) / Exc.Value)) / Math.Abs(SumINS) * NET

                End If
            End If
            '-------- 2
            Dim m As Double
            m = ((.Item("TRCAPCTY") * IslocalAcc / Val(Exc.Value)) + (.Item("TR2STAMT") * IslocalAcc / Val(Exc.Value))) / cnt
            If Math.Abs(SumINS) > (.Item("TRCAPCTY") * IslocalAcc / Val(Exc.Value)) Then
                TRet.Value = .Item("TRRETAMT") * IslocalAcc / Math.Abs(SumINS) / Exc.Value * NET / cnt 'Retention
                TQS.Value = .Item("TRQSRAMT") * IslocalAcc / Math.Abs(SumINS) / Exc.Value * NET / cnt 'Quatashare

                If TRet.Value.ToString = "NaN" Or TRet.Value.ToString = "ليس رقمًا" Then
                    TRet.Value = 0
                Else

                End If
                If TQS.Value.ToString = "NaN" Or TQS.Value.ToString = "ليس رقمًا" Then
                    TQS.Value = 0
                Else

                End If
                TFirstSup.Value = .Item("TR1STAMT") * IslocalAcc / Val(Exc.Value) / Math.Abs(SumINS) * NET / cnt '1st surplus
                TSecondSup.Value = .Item("TR2STAMT") * IslocalAcc / Val(Exc.Value) / Math.Abs(SumINS) * NET / cnt '2nd surplus
                If Request("Sys") = "MC" Or Request("Sys") = "MB" Or Request("Sys") = "MA" Or Request("Sys") = "OC" Then
                    If Math.Abs(SumINS) - (.Item("TRCAPCTY") * IslocalAcc / Val(Exc.Value)) <= (.Item("TRLSAMT") * IslocalAcc / Val(Exc.Value)) And (Request("Sys") = "MC" Or Request("Sys") = "MB" Or Request("Sys") = "MA" Or Request("Sys") = "OC") And (.Item("TRCAPCTY") / cnt) <> 0 Then
                        TLineSlip.Value = (SumINS - (.Item("TRCAPCTY") * IslocalAcc / Val(Exc.Value))) / SumINS * NET / cnt 'lineslip
                    Else
                        If (.Item("TRCAPCTY") * IslocalAcc / Val(Exc.Value)) <> 0 Then
                            TLineSlip.Value = .Item("TRLSAMT") * IslocalAcc / Val(Exc.Value) / SumINS * NET
                            'TFac.Text = Format((SumINS - (.Item("TRCAPCTY") / Val(Exc.Text)) - (4000000 / Val(Exc.Text))) / SumINS * NET, "0.000\0") / Val(EndCnt.Text)
                            'TFac.Text = Format(NET - (CDbl(TRet.Value) + CDbl(TQS.Text) + CDbl(TSecondSup.Text) + CDbl(TFirstSup.Text) + CDbl(TLineSlip.Text)), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                        Else
                            'TFac.Text = Format((SumINS - (.Item("TRCAPCTY") / Val(Exc.Text))) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) / cnt
                        End If
                    End If
                Else
                    If Math.Abs(SumINS) >= m And FacRatio.Value = 0 Then
                        If SpRetRatio.Value = 0 And HF.Value = 0 Then
                            TFac.Value = NET - (TRet.Value + TQS.Value + TSecondSup.Value + TFirstSup.Value + TLineSlip.Value) - SpRet.Value 'Format((SumINS - (.Item("TRCAPCTY") / Val(Exc.Text))) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) '/ Val(EndCnt.Text)
                        Else
                            'TFac.Text = Format(NET - (CDbl(TRet.Value) + CDbl(TQS.Text) + CDbl(TSecondSup.Text) + CDbl(TFirstSup.Text) + CDbl(TLineSlip.Text)), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) - SpRet.Value
                        End If
                    Else
                        'TFac.Text = F.Text
                    End If
                End If
            Else
                If Math.Abs(SumINS) <= (.Item("TRRETAMT") * IslocalAcc / Val(Exc.Value)) + (.Item("TRQSRAMT") * IslocalAcc / Val(Exc.Value)) Then
                    'On Error Resume Next
                    TRet.Value = .Item("TRRETAMT") * IslocalAcc / Val(Exc.Value) / ((.Item("TRRETAMT") * IslocalAcc / Val(Exc.Value)) + (.Item("TRQSRAMT") * IslocalAcc / Val(Exc.Value))) * NET

                    If TRet.Value.ToString = "NaN" Or TRet.Value.ToString = "ليس رقمًا" Then
                        TRet.Value = 0
                    Else

                    End If
                    If TQS.Value.ToString = "NaN" Or TQS.Value.ToString = "ليس رقمًا" Then
                        TQS.Value = 0
                    Else

                    End If

                    'If Not IsNumeric(TRet.Value) Then
                    '    TRet.Value = 0
                    'Else

                    'End If

                    'If TQS.Value.ToString = "NaN" Or TQS.Value.ToString = "ليس رقمًا" Then
                    If Not IsNumeric(TQS.Value) Then
                        TQS.Value = 0
                    Else

                    End If
                End If
            End If

            If Val(War.Text) <> 0 Then
                If Math.Abs(WsI) <= (.Item("TRRETAMT") * IslocalAcc + .Item("TRQSRAMT")) * IslocalAcc / Val(Exc.Value) Then
                    wQS.Text = Format((War.Value / Val(EndCnt.Value)) - wFac.Value, IIf(Exc.Value = 1, "###,#0.000", "###,#0.00"))
                Else
                    If Math.Abs(WsI) <= (.Item("TRCAPCTY") * IslocalAcc / Val(Exc.Value)) Then
                        wQS.Value = Format((.Item("TRRETAMT") * IslocalAcc + .Item("TRQSRAMT") * IslocalAcc) / Val(Exc.Value) / WsI * (War.Value - wFac.Value) / Val(EndCnt.Value), IIf(Exc.Value = 1, "###,#0.000", "###,#0.00"))
                        wFirstSup.Value = Format((WsI - ((.Item("TRRETAMT") * IslocalAcc + .Item("TRQSRAMT") * IslocalAcc) / Val(Exc.Value))) / WsI * (War.Value - wFac.Value) / Val(EndCnt.Value), IIf(Exc.Value = 1, "###,#0.000", "###,#0.00"))
                    Else
                        wQS.Value = Format((.Item("TRRETAMT") * IslocalAcc + .Item("TRQSRAMT") * IslocalAcc) / Val(Exc.Value) / WsI * War.Value / Val(EndCnt.Value), IIf(Exc.Value = 1, "###,#0.000", "###,#0.00"))
                        wFirstSup.Value = Format(.Item("TR1STAMT") * IslocalAcc / Val(Exc.Value) / WsI * War.Value / Val(EndCnt.Value), IIf(Exc.Value = 1, "###,#0.000", "###,#0.00"))
                        wFac.Value = Format((WsI - (.Item("TRCAPCTY") * IslocalAcc / Val(Exc.Value))) / WsI * War.Value / Val(EndCnt.Value), IIf(Exc.Value = 1, "###,#0.000", "###,#0.00"))
                    End If
                End If
            End If
            'TNetPrm.Text = NET
            ExtPrm.Text = ExtraValue
            If WarFacRatio.Value <> 0 Then

            End If

            'TRet.Value = Math.Round(TRet.Value, 3)
            'TQS.Value = Math.Round(TQS.Value, 3)
            'TFirstSup.Value = Math.Round(TFirstSup.Value, 3)
            'TFac.Value = Math.Round(TFac.Value, 3)
            'TSpRet.Value = Math.Round(TSpRet.Value, 3)
            'If TRet.Value + TQS.Value + TFirstSup.Value + TFac.Value + TSpRet.Value <> NET Then
            '    TRet.Value += NET - (TRet.Value + TQS.Value + TFirstSup.Value + TFac.Value + TSpRet.Value)
            'End If
        End With
    End Sub

    Public Function GetEndFile(Sys As String) As String
        Dim IssueVal As New DataSet
        'Dim result As String
        Dim dbadapter = New SqlDataAdapter("select SumInsured from SubSystems where SubSysNo='" & Sys & "' AND Branch=dbo.MainCenter()", Conn)
        Dim unused = dbadapter.Fill(IssueVal)
        Select Case Sys
            Case "MC", "MB", "MA"
                GetEndFile = If(Request("EndNo") = 0,
                    IssueVal.Tables(0).Rows(0)(0).ToString,
                    DirectCast(IIf(stat, IssueVal.Tables(0).Rows(0)(0).ToString, "New" + IssueVal.Tables(0).Rows(0)(0)), String))
            Case Else
                GetEndFile = IssueVal.Tables(0).Rows(0)(0).ToString
        End Select
    End Function

    Protected Function GetFirstOrder() As String
        Dim IssueVal As New DataSet
        Dim Temp = If(Request("PolNo") = Request("OrderNo"), "policyFile.OrderNo", "PolicyFile.PolNo")
        Dim dbadapter = New SqlDataAdapter("select Max(PolicyFile.orderNo) from PolicyFile Inner Join " & GetGroupFile(Request("Sys")) & " On  " _
                               & "PolicyFile.OrderNo=" & GetGroupFile(Request("Sys")) & ".OrderNo and " _
                               & "PolicyFile.EndNo=" & GetGroupFile(Request("Sys")) & ".EndNo and " _
                               & "PolicyFile.LoadNo=" & GetGroupFile(Request("Sys")) & ".LoadNo and " _
                               & "PolicyFile.SubIns=" & GetGroupFile(Request("Sys")) & ".SubIns  " _
                               & " where " & Temp & "='" & Trim(Request("PolNo")) & "' and  PolicyFile.SubIns='" & Request("Sys") & "'" _
                            , Conn)
        Dim unused = dbadapter.Fill(IssueVal)
        GetFirstOrder = IssueVal.Tables(0).Rows(0)(0)
    End Function

#Disable Warning IDE0060 ' Remove unused parameter

    Protected Function WarPrmCalc(War As Double) As Double
#Enable Warning IDE0060 ' Remove unused parameter
        Dim WarprmCalc1 As New DataSet
        Dim Temp As String
        Dim Tempend As Integer
        Temp = If(Request("PolNo") = Request("OrderNo"), "policyFile.OrderNo", "PolicyFile.PolNo")
        Tempend = If(Val(EndNo.Text) > 0 And Request("PolNo") = Request("OrderNo"), Val(EndNo.Text), Val(EndNo.Text) - 1)

        Dim dbadapter = New SqlDataAdapter("Select Sum(GodFile.WarPrm) As WarPrm FROM GodFile INNER Join PolicyFile On" _
                  & " GodFile.OrderNo=PolicyFile.OrderNo And GodFile.SubIns=PolicyFile.SubIns WHERE " & Temp & "='" & RTrim(PolNo.Text) & "' And GodFile.EndNo= " & Tempend & "", Conn)

        'Dim dbadapter = New Data.SqlClient.SqlDataAdapter("(SELECT Sum(WarPrm) As WarPrm FROM godfile  inner join policyfile on godfile.orderno =" _
        '                     & "(Select min(PolF.orderno) from PolicyFile AS PolF where godfile.endno<" & Val(Request("EndNo")) & " AND PolF.PolNo='" & RTrim(PolNo.Text) & "' ) AND godfile.SubIns = '" & Request("Sys") & "' ) ", ConnLocal)
        Dim unused = dbadapter.Fill(WarprmCalc1)

        If IsDBNull(WarprmCalc1.Tables(0).Rows(0)("WarPrm")) Or WarprmCalc1.Tables(0).Rows(0)("WarPrm") < 0 Then
            WarPrmCalc = 0
        Else
            WarPrmCalc = If(NetPRM.Text < 0,
                DirectCast(IIf(WarprmCalc1.Tables(0).Rows(0)("WarPrm") = DB.Tables(0).Rows(0)("WarPrm"), -1 * DB.Tables(0).Rows(0)("WarPrm"), DB.Tables(0).Rows(0)("WarPrm") - WarprmCalc1.Tables(0).Rows(0)("WarPrm")), Double),
                DirectCast(Math.Abs(DB.Tables(0).Rows(0)("WarPrm") - CDbl(WarprmCalc1.Tables(0).Rows(0)("WarPrm"))), Double))
        End If
    End Function

    Protected Function GetEndData() As Boolean
        Dim IssueVal As New DataSet
        Dim dbadapter = New SqlDataAdapter("select * from " & GetGroupFile(Request("Sys")) & "  where " _
                               & " OrderNo='" & Trim(Request("OrderNo")) & "' and EndNo=" & Request("EndNo") _
                               & " and LoadNo=" & Request("LoadNo") & " and SubIns='" & Request("Sys") & "'" _
                            , Conn)
        Dim unused = dbadapter.Fill(IssueVal)
        If IssueVal.Tables(0).Rows.Count = 0 Then
            GetEndData = False
        Else
            GetEndData = True
        End If
    End Function

    Public Function Valu(Vr As Object) As Double
        If IsNothing(Vr) Then
            Valu = 0
        Else
            Valu = If(Val(Vr) <> 0, Val(Vr), If(Vr = True, 1, 0))
        End If
    End Function

    Protected Sub ASPxButton3_Click(sender As Object, e As EventArgs) Handles ASPxButton3.Click
        If Request("PolNo") = Request("OrderNo") Then
            ExecConn("update PolicyFile Set EXTPRM=" & CDbl(ExtPrm.Text) & ",ExtraRate=163 where " _
                               & "  PolicyFile.OrderNo='" & Trim(Request("OrderNo")) & "' and PolicyFile.EndNo=" & Request("EndNo") _
                               & " and PolicyFile.LoadNo=" & Request("LoadNo") & " and PolicyFile.SubIns='" & Request("Sys") & "'" _
                               & " and PolicyFile.Branch='" & Request("Branch") & "'", Conn)
            ExecConn("delete netprm where " _
                              & " PolNo='" & Trim(Request("OrderNo")) & "' and EndNo=" & Request("EndNo") _
                              & " and LoadNo=" & Request("LoadNo") & " and TP='" & Request("Sys") & "'" _
                              & " and Br='" & Request("Branch") & "'", Conn)
        Else
            ExecConn("update PolicyFile Set Reinsured=1 where " _
                                & " PolicyFile.OrderNo='" & Trim(Request("OrderNo")) & "' and PolicyFile.EndNo=" & Request("EndNo") _
                                & " and PolicyFile.LoadNo=" & Request("LoadNo") & " and PolicyFile.SubIns='" & Request("Sys") & "'" _
                                & " and PolicyFile.Branch='" & Request("Branch") & "'", Conn)
        End If
    End Sub

End Class