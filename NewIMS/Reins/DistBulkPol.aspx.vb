Imports System.Data.SqlClient
Imports DevExpress.Web
Imports Microsoft.Reporting.WebForms

Partial Public Class DistBulkPol
    Inherits Page

    Private ReadOnly DB As New DataSet
    Private ReadOnly DB1 As New DataSet
    Private Lo() As String
    Private ExtraValue As Double = 0
    Private ReadOnly WExtraValue As Double = 0
    Private ReadOnly FExtraValue As Double = 0
    Private ReadOnly WFExtraValue As Double = 0
    Private ReadOnly stat As Boolean

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim Treaty, PreTreaty As New DataSet
        Lo = Session("LogInfo")
        Dim ssSQL, Ty As String
        Dim TyDigit As Integer

        'If PolNo.text = OrderNo.text Then
        '    ASPxButton4.Visible = False
        '    ASPxButton5.Text = "احتساب القسط الإضافي"
        'Else
        '    ASPxButton4.Visible = True
        '    AlertL0.Visible = False
        '    ExtPrm.Visible = False
        'End If
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter1 = New SqlDataAdapter("SELECT  p.OrderNo, p.LoadNo, p.EndNo,  p.IssuDate,p.SubIns AS Sys,
    p.PolNo,  p.Branch, p.NetPrm  FROM PolicyFile p  WHERE EXISTS (   SELECT 1 FROM NetPrm n  WHERE n.PolNo = p.PolNo
      AND n.EndNo = p.EndNo AND n.LoadNo = p.LoadNo GROUP BY n.PolNo, n.EndNo, n.LoadNo HAVING round(SUM(n.net),0) <> round(p.NETPRM,0) )
AND p.SubIns NOT IN ('01','02','03','OR','27','08','PH','MN','MK','07','09')  AND p.IssuDate IS NOT NULL ORDER BY p.IssuDate DESC;
", con)
            dbadapter1.Fill(DB1)
            For i As Integer = 0 To DB1.Tables(0).Rows.Count - 1 Step 1
                If DB1.Tables(0).Rows(i)("Sys") = "BB" Then
                    ssSQL = "select rtrim(PolNo) As PolNo,CustName,Commision,CoverFrom,CoverTo,Stop,TOTPRM,PolicyFile.CustNo,IssuDate,PolicyFile.ExcRate,PolicyFile.Currency,PolicyFile.EndNo,PolicyFile.LoadNo,NetPRM,TOTPRM,CoverFrom,CoverTo,CoverType," _
                               & "(select " & IIf(DB1.Tables(0).Rows(i)("Sys") = "MC" Or DB1.Tables(0).Rows(i)("Sys") = "OC", "Loads", "count(OrderNo)") _
                               & " from " & GetGroupFile(DB1.Tables(0).Rows(i)("Sys")) & " as EndFile where EndFile.orderno=" & IIf(GetEndData(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), DB1.Tables(0).Rows(i)("EndNo"), DB1.Tables(0).Rows(i)("LoadNo")), "PolicyFile.OrderNo", "'" & GetFirstOrder(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("PolNo")) & "'") & " and " _
                               & IIf(GetEndData(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), DB1.Tables(0).Rows(i)("EndNo"), DB1.Tables(0).Rows(i)("LoadNo")), " EndFile.EndNo=" & IIf(GetEndData(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), DB1.Tables(0).Rows(i)("EndNo"), DB1.Tables(0).Rows(i)("LoadNo")), "PolicyFile.EndNo", "0") & " and EndFile.LoadNo=PolicyFile.LoadNo and ", "") & " EndFile.SubIns=PolicyFile.SubIns) as EndCnt," _
                               & IIf(DB1.Tables(0).Rows(i)("Sys") = "MC" Or DB1.Tables(0).Rows(i)("Sys") = "05", " WarPrm,", "") & " (select " & GetEndFile(DB1.Tables(0).Rows(i)("Sys")) & " from " & GetGroupFile(DB1.Tables(0).Rows(i)("Sys")) & " as EndFile_1 where EndFile_1.orderno=" _
                               & IIf(GetEndData(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), DB1.Tables(0).Rows(i)("EndNo"), DB1.Tables(0).Rows(i)("LoadNo")), "PolicyFile.OrderNo", "'" & GetFirstOrder(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("PolNo")) & "'") & " and " _
                               & IIf(GetEndData(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), DB1.Tables(0).Rows(i)("EndNo"), DB1.Tables(0).Rows(i)("LoadNo")), " EndFile_1.EndNo=" & IIf(GetEndData(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), DB1.Tables(0).Rows(i)("EndNo"), DB1.Tables(0).Rows(i)("LoadNo")), "PolicyFile.EndNo", "0") & " and EndFile_1.LoadNo=PolicyFile.LoadNo and", "") & " EndFile_1.SubIns=PolicyFile.SubIns) as SumIns from PolicyFile Inner Join CustomerFile" _
                               & " on PolicyFile.CustNo=CustomerFile.CustNo" _
                               & " inner Join " & GetGroupFile(DB1.Tables(0).Rows(i)("Sys")) & " on " & GetGroupFile(DB1.Tables(0).Rows(i)("Sys")) & ".OrderNo=" & IIf(GetEndData(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), DB1.Tables(0).Rows(i)("EndNo"), DB1.Tables(0).Rows(i)("LoadNo")), "PolicyFile.OrderNo", "'" & GetFirstOrder(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("PolNo")) & "'") _
                               & " and " & IIf(GetEndData(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), DB1.Tables(0).Rows(i)("EndNo"), DB1.Tables(0).Rows(i)("LoadNo")), GetGroupFile(DB1.Tables(0).Rows(i)("Sys")) & ".EndNo=" & IIf(GetEndData(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), DB1.Tables(0).Rows(i)("EndNo"), DB1.Tables(0).Rows(i)("LoadNo")), "PolicyFile.EndNo", "0") & " and " _
                               & GetGroupFile(DB1.Tables(0).Rows(i)("Sys")) & ".LoadNo=PolicyFile.LoadNo and ", "") & GetGroupFile(DB1.Tables(0).Rows(i)("Sys")) & ".SubIns=PolicyFile.SubIns" _
                               & " where PolicyFile.OrderNo='" & Trim(DB1.Tables(0).Rows(i)("OrderNo")) & "' and PolicyFile.EndNo=" & DB1.Tables(0).Rows(i)("EndNo") _
                               & " and PolicyFile.LoadNo=" & DB1.Tables(0).Rows(i)("LoadNo") & " and PolicyFile.SubIns='" & DB1.Tables(0).Rows(i)("Sys") & "'" _
                               & " and PolicyFile.Branch='" & DB1.Tables(0).Rows(i)("Branch") & "'"
                Else
                    ssSQL = "select rtrim(PolNo) As PolNo,CustName,Commision,CoverFrom,CoverTo,Stop,TOTPRM,PolicyFile.CustNo,IssuDate,PolicyFile.ExcRate,PolicyFile.Currency,PolicyFile.EndNo,PolicyFile.LoadNo,NetPRM,TOTPRM,CoverFrom,CoverTo,CoverType," _
                               & "(select " & IIf(DB1.Tables(0).Rows(i)("Sys") = "MC" Or DB1.Tables(0).Rows(i)("Sys") = "MB" Or DB1.Tables(0).Rows(i)("Sys") = "MA" Or DB1.Tables(0).Rows(i)("Sys") = "OC", "Loads", "count(OrderNo)") _
                               & " from " & GetGroupFile(DB1.Tables(0).Rows(i)("Sys")) & " as EndFile where EndFile.orderno=" & IIf(GetEndData(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), DB1.Tables(0).Rows(i)("EndNo"), DB1.Tables(0).Rows(i)("LoadNo")), "PolicyFile.OrderNo", "'" & GetFirstOrder(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("PolNo")) & "'") & " and " _
                               & IIf(GetEndData(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), DB1.Tables(0).Rows(i)("EndNo"), DB1.Tables(0).Rows(i)("LoadNo")), " EndFile.EndNo=" & IIf(GetEndData(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), DB1.Tables(0).Rows(i)("EndNo"), DB1.Tables(0).Rows(i)("LoadNo")), "PolicyFile.EndNo", "0") & " and EndFile.LoadNo=PolicyFile.LoadNo and ", "") & " EndFile.SubIns=PolicyFile.SubIns) as EndCnt," _
                               & IIf(DB1.Tables(0).Rows(i)("Sys") = "MC" Or DB1.Tables(0).Rows(i)("Sys") = "MB" Or DB1.Tables(0).Rows(i)("Sys") = "MA" Or DB1.Tables(0).Rows(i)("Sys") = "OC", " WarPrm,", "") & " (select sum(" & GetEndFile(DB1.Tables(0).Rows(i)("Sys")) & ") from " & GetGroupFile(DB1.Tables(0).Rows(i)("Sys")) & " as EndFile_1 where EndFile_1.orderno=" _
                               & IIf(GetEndData(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), DB1.Tables(0).Rows(i)("EndNo"), DB1.Tables(0).Rows(i)("LoadNo")), "PolicyFile.OrderNo", "'" & GetFirstOrder(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("PolNo")) & "'") & " and " _
                               & IIf(GetEndData(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), DB1.Tables(0).Rows(i)("EndNo"), DB1.Tables(0).Rows(i)("LoadNo")), " EndFile_1.EndNo=" & IIf(GetEndData(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), DB1.Tables(0).Rows(i)("EndNo"), DB1.Tables(0).Rows(i)("LoadNo")), "PolicyFile.EndNo", "0") & " and EndFile_1.LoadNo=PolicyFile.LoadNo and", "") & " EndFile_1.SubIns=PolicyFile.SubIns) as SumIns from PolicyFile Inner Join CustomerFile" _
                               & " on PolicyFile.CustNo=CustomerFile.CustNo" _
                               & " inner Join " & GetGroupFile(DB1.Tables(0).Rows(i)("Sys")) & " on " & GetGroupFile(DB1.Tables(0).Rows(i)("Sys")) & ".OrderNo=" & IIf(GetEndData(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), DB1.Tables(0).Rows(i)("EndNo"), DB1.Tables(0).Rows(i)("LoadNo")), "PolicyFile.OrderNo", "'" & GetFirstOrder(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("PolNo")) & "'") _
                               & " and " & IIf(GetEndData(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), DB1.Tables(0).Rows(i)("EndNo"), DB1.Tables(0).Rows(i)("LoadNo")), GetGroupFile(DB1.Tables(0).Rows(i)("Sys")) & ".EndNo=" & IIf(GetEndData(DB1.Tables(0).Rows(i)("Sys"), DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), DB1.Tables(0).Rows(i)("EndNo"), DB1.Tables(0).Rows(i)("LoadNo")), "PolicyFile.EndNo", "0") & " and " _
                               & GetGroupFile(DB1.Tables(0).Rows(i)("Sys")) & ".LoadNo=PolicyFile.LoadNo and ", "") & GetGroupFile(DB1.Tables(0).Rows(i)("Sys")) & ".SubIns=PolicyFile.SubIns" _
                               & " where PolicyFile.OrderNo='" & Trim(DB1.Tables(0).Rows(i)("OrderNo")) & "' and PolicyFile.EndNo=" & DB1.Tables(0).Rows(i)("EndNo") _
                               & " and PolicyFile.LoadNo=" & DB1.Tables(0).Rows(i)("LoadNo") & " and PolicyFile.SubIns='" & DB1.Tables(0).Rows(i)("Sys") & "'" _
                               & " and PolicyFile.Branch='" & DB1.Tables(0).Rows(i)("Branch") & "'"
                End If
                'Response.Write(ssSQL)
                Dim dbadapter = New SqlDataAdapter(ssSQL, con)
                DB.Clear()
                Treaty.Clear()

                dbadapter.Fill(DB)
                Exc.Text = DB.Tables(0).Rows(0)("ExcRate")
                PolNo.Text = If(DB1.Tables(0).Rows(i)("PolNo") = DB1.Tables(0).Rows(i)("OrderNo"), DB1.Tables(0).Rows(i)("PolNo"), TryCast(DB.Tables(0).Rows(0)("PolNo"), String))
                PolNo.Text = PolNo.Text.Replace(vbTab, "")
                EndNo.Text = DB1.Tables(0).Rows(i)("EndNo")
                LoadNo.Text = DB1.Tables(0).Rows(i)("LoadNo")
                CustName.Text = DB.Tables(0).Rows(0)("CustName")
                Sys.Text = DB1.Tables(0).Rows(i)("Sys")
                OrderNo.Text = DB1.Tables(0).Rows(i)("OrderNo")
                Branch.Text = DB1.Tables(0).Rows(i)("Branch")
                'DB.Tables(0).Rows(0)("IssuDate").IsNull(0)
                IssuDate.Text = If(DB1.Tables(0).Rows(i)("PolNo") = DB1.Tables(0).Rows(i)("OrderNo"), "1900/01/01", Format(DB.Tables(0).Rows(0)("IssuDate"), "yyyy/MM/dd"))
                SumInsured.Text = Format(DB.Tables(0).Rows(0)("SumIns"), "###,#0.000")
                NetPRM.Text = Format(DB.Tables(0).Rows(0)("NetPRM"), "###,#0.000")
                HNetPRM.Text = Format(DB.Tables(0).Rows(0)("NetPRM"), "###,#0.000")
                Comission.Text = Format(DB.Tables(0).Rows(0)("NetPRM") * IIf(DB.Tables(0).Rows(0).IsNull("Commision"), 0, DB.Tables(0).Rows(0)("Commision")) / 100, "0.000")
                EndCnt.Text = IIf(DB.Tables(0).Rows(0)("EndCnt") = 0 Or Sys.Text = "PA", 1, DB.Tables(0).Rows(0)("EndCnt"))
                CustNo.Text = DB.Tables(0).Rows(0)("CustNo")
                TOTPRM.Text = DB.Tables(0).Rows(0)("TOTPRM")
                Cur.Text = DB.Tables(0).Rows(0)("Currency")
                Cfrom.Text = Format(DB.Tables(0).Rows(0)("CoverFrom"), "yyyy/MM/dd")
                Cto.Text = Format(DB.Tables(0).Rows(0)("CoverTo"), "yyyy/MM/dd")
                'If PolNo.text = OrderNo.text Then
                '    Ty = Mid(PolNo.Text, 1, 4)
                '    TyDigit = Val(Mid(PolNo.Text, 1, 4))
                'Else

                If Len(RTrim(PolNo.Text)) > 14 Then
                    Ty = Mid(PolNo.Text, 5, 4)
                    TyDigit = Val(Mid(PolNo.Text, 5, 4))
                Else
                    Ty = Mid(PolNo.Text, 4, 4)
                    TyDigit = Val(Mid(PolNo.Text, 4, 4))
                End If
                'End If
                'Dim TotPRM As Double
                Select Case DB1.Tables(0).Rows(i)("Sys")

                    Case "MC", "MB", "MA", "OC"
                        TreatyNo.Text = DB1.Tables(0).Rows(i)("Sys") + IIf(Len(CStr(DB.Tables(0).Rows(0)("CoverType"))) = 1, "0" + CStr(DB.Tables(0).Rows(0)("CoverType")), CStr(DB.Tables(0).Rows(0)("CoverType"))) + Ty
                        If EndNo.Text = 0 Then
                            War.Text = DB.Tables(0).Rows(0)("WarPrm")
                            HWar.Text = DB.Tables(0).Rows(0)("WarPrm")
                        Else
                            War.Text = WarPrmCalc()
                            HWar.Text = WarPrmCalc()
                        End If
                    Case Else
                        TreatyNo.Text = DB1.Tables(0).Rows(i)("Sys") + IIf(Len(CStr(DB.Tables(0).Rows(0)("CoverType"))) = 1, "0" + CStr(DB.Tables(0).Rows(0)("CoverType")), CStr(DB.Tables(0).Rows(0)("CoverType"))) + Ty
                End Select

                If Val(War.Text) <> 0 Then
                    'NetPRM.Text = CDbl(NetPRM.Text) - Val(War.Text)
                Else
                    War.Text = "0"
                    HWar.Text = "0"
                End If
                DistNet.Text = Format(NetPRM.Text - Val(Comission.Text) - Val(War.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                HDistNet.Text = Format(NetPRM.Text - Val(Comission.Text) - Val(War.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))

RefreshData:    Dim TreatyData = New SqlDataAdapter("select * from TRREGFLE where TreatyNo='" & TreatyNo.Text & "'", con)
                TreatyData.Fill(Treaty)
                If Treaty.Tables(0).Rows.Count = 0 Then
                    Dim PreTreatyData = New SqlDataAdapter("select * from TRREGFLE where TreatyNo='" & Left(TreatyNo.Text, 4) & CInt(Right(TreatyNo.Text, 4)) - 1 & "'", con)
                    PreTreatyData.Fill(PreTreaty)
                    If PreTreaty.Tables(0).Rows.Count = 0 Then

                        MsgBob(Me, "لم يتم إدخال حدود الاتفاقية لهذا النوع، يرجى الاتصال بإدارة إعادة التأمين ")
                        ExecConn("INSERT INTO [TRREGFLE]([TreatyNo],[Descrip],[Acctype],[Portfolio],[ReserveR],[TRSYSDTE],[TRINSDTE],[TREXPDTE],[TRCAPCTY],[TRRETAMT],[TRQSRAMT]
,[TRQSRCOM],[TR1STAMT],[TR1STCOM],[TR2STAMT],[TR2STCOM],[TRLQSRCOM],[TRL1STCOM],[TRL2STCOM],[TRWQSRCOM],[TRW1STCOM],[InterestRRes],[TRLSAMT],[TRLSCOMM])
 VALUES('" & Trim(TreatyNo.Text) & "','" & "No Treaty entry" & Request("Sys") & "/" & "20" & Ty & "'," & 2 & "," & 1 & "," _
     & 0 & ",'" & Right(TreatyNo.Text, 4) & "/01" & "/01" & "','" & Right(TreatyNo.Text, 4) & "/01" & "/01" & "','" & Right(TreatyNo.Text, 4) & "/12" & "/31" & "'," _
     & 0 & "," & 0 & "," & 0 & "," & 0 & "," & 0 & "," & 0 & "," & 0 & "," & 0 & "," & 0 & "," & 0 & "," _
     & 0 & "," & 0 & "," & 0 & "," & 0 & "," & 0 & "," & 0 & ")", con)
                        GoTo RefreshData

                    Else
                        MsgBob(Me, "لم يتم إدخال حدود الاتفاقية لهذا النوع، يرجى الاتصال بإدارة إعادة التأمين ")
                        ExecConn("	  INSERT INTO TRREGFLE
 ([TRSYSDTE], [Descrip], [AccType], [Portfolio], [ReserveR], [TREATYNO], [TRINSDTE], [TREXPDTE], [TRPNOCLN], [TRPORTYP], [TRCAPCTY], [TRLOSADV] 
 ,[TRCSHLIS],[TRACCONT],[TRPROCOM],[TRMAGEXP],[TRBASISF],[TRPRDRET],[TRPRDREL],[TRPRDINT],[TRLRDRET],[TRLRDREL],[TRLRDINT],[TRPFWPRM]
, [TRPFWLOS], [TRPFEPRM], [TRPFELOS], [TRRETAMT], [TRQSRAMT], [TRQSRCOM], [TRLQSRCOM], [TRWQSRCOM], [TR1STAMT], [TR1STCOM], [TRL1STCOM],
[TRW1STCOM]
 ,[TR2STAMT],[TR2STCOM],[TRLSAMT],[TRL2STCOM],[TR3STAMT],[TR3STCOM],[TR4STAMT],[TR4STCOM],[TRLSAMUNT],[TRLSCOMM],[TRSRAMNT],[TRSRCOMM]
,[TRFACAMT],[TRFACCOM],[InterestRRes],[NET])

 Select dateadd(YEAR,1,[TRSYSDTE]),Descrip + ' Renewed Treaty Entry from Prev Year Treaty ' + '" & CInt(Right(TreatyNo.Text, 4)) - 1 & "' ,AccType, Portfolio,ReserveR,
'" & TreatyNo.Text & "' , dateadd(YEAR,1,TRINSDTE), dateadd(YEAR,1,TREXPDTE),[TRPNOCLN], 
[TRPORTYP], [TRCAPCTY], [TRLOSADV] 
,[TRCSHLIS],[TRACCONT],[TRPROCOM],[TRMAGEXP],[TRBASISF],[TRPRDRET],[TRPRDREL],[TRPRDINT],[TRLRDRET],[TRLRDREL],[TRLRDINT],[TRPFWPRM]
, [TRPFWLOS], [TRPFEPRM], [TRPFELOS], [TRRETAMT], [TRQSRAMT], [TRQSRCOM], [TRLQSRCOM], [TRWQSRCOM], [TR1STAMT], [TR1STCOM], [TRL1STCOM], [TRW1STCOM]
,[TR2STAMT],[TR2STCOM],[TRLSAMT],[TRL2STCOM],[TR3STAMT],[TR3STCOM],[TR4STAMT],[TR4STCOM],[TRLSAMUNT],[TRLSCOMM],[TRSRAMNT],[TRSRCOMM]
,[TRFACAMT],[TRFACCOM],[InterestRRes],[NET] 
from TRREGFLE where right(treatyno,4)=" & CInt(Right(TreatyNo.Text, 4)) - 1 & " AND left(treatyno,4) ='" & Left(TreatyNo.Text, 4) & "'", con)
                        GoTo RefreshData

                    End If

                End If

                Capacity.Text = Format(Treaty.Tables(0).Rows(0)("TRCAPCTY"), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
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
                If CDbl(Treaty.Tables(0).Rows(0)("TRCAPCTY")) + CDbl(Treaty.Tables(0).Rows(0)("TRLSAMT")) >= (Math.Abs(CDbl(DB.Tables(0).Rows(0)("SumIns")) * Val(Exc.Text)) / Val(EndCnt.Text)) Then
                    DistNet.Text = Format(NetPRM.Text - Comission.Text - War.Text, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    HDistNet.Text = Format(NetPRM.Text - Comission.Text - War.Text, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    F.Text = 0
                    HF.Text = 0
                    HFM.Text = 0
                Else
                    DistNet.Text = Format(Val((Treaty.Tables(0).Rows(0).Item("TRCAPCTY") + Treaty.Tables(0).Rows(0).Item("TRLSAMT")) / Val(Exc.Text)) / Math.Abs(DB.Tables(0).Rows(0)("SumIns")) * 100 * (DB.Tables(0).Rows(0)("NetPRM") - CDbl(Comission.Text)) / 100, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    HDistNet.Text = Format(Val((Treaty.Tables(0).Rows(0).Item("TRCAPCTY") + Treaty.Tables(0).Rows(0).Item("TRLSAMT")) / Val(Exc.Text)) / Math.Abs(DB.Tables(0).Rows(0)("SumIns")) * 100 * (DB.Tables(0).Rows(0)("NetPRM") - CDbl(Comission.Text)) / 100, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    F.Text = Format(Math.Round(NetPRM.Text - DistNet.Text, 3), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    HF.Text = Math.Round(NetPRM.Text - DistNet.Text, 3) ', IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    HFM.Text = Math.Round(NetPRM.Text - DistNet.Text, 3) ', IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    FacRatio.PositionStart = 100 - (Val((Treaty.Tables(0).Rows(0).Item("TRCAPCTY") + Treaty.Tables(0).Rows(0).Item("TRLSAMT")) / Val(Exc.Text)) / Math.Abs(DB.Tables(0).Rows(0)("SumIns")) * 100)
                    FacRatio.MinValue = FacRatio.PositionStart
                End If
                DistRe()
                DistNet.Text = 0
                HDistNet.Text = 0
                F.Text = 0
                HF.Text = 0
                HFM.Text = 0
                FacRatio.PositionStart = 0
                FacRatio.MinValue = 0
            Next
            'AlertL.Visible = Capacity.Text + CDbl(LineSlip.Text) < SumInsured.Text * Val(Exc.Text)
            con.Close()
        End Using
    End Sub

    'Protected Sub ASPxButton5_Click(sender As Object, e As EventArgs) Handles ASPxButton5.Click
    '    ExecSql("delete from NETPRM where PolNo='" & PolNo.Text & "'" _
    '                           & " and EndNo=" & EndNo.Text & " and " _
    '                           & "LoadNo=" & Load.Text & " and TP='" & Sys.Text & "'")
    '    'ExecSql(SQL)
    '    DistRe()
    '    ASPxGridView1.DataBind()
    'End Sub

    Private Sub DistRe()

        Dim EndFile, OldD As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim EndData = New SqlDataAdapter("select " & IIf(IsGroupedSys(Sys.Text), "Premium,", "") & "" & GetEndFile(Sys.Text) & " as SumIns from " & GetGroupFile(Sys.Text) & " where " & GetGroupFile(Sys.Text) & ".OrderNo='" & OrderNo.Text & "'" _
                       & " and " & GetGroupFile(Sys.Text) & ".EndNo=" & EndNo.Text & " and " _
                       & GetGroupFile(Sys.Text) & ".LoadNo=" & LoadNo.Text & " and " & GetGroupFile(Sys.Text) & ".SubIns='" & Sys.Text & "'", con)
            Dim unused = EndData.Fill(EndFile)
            Dim Dist As New DataSet
            'ExecSql("delete from NETPRM where PolNo='" & PolNo.Text & "'" _
            '              & " and EndNo=" & Request("EndNo") & " and " _
            '              & "LoadNo=" & Request("LoadNo") & " and TP='" & Sys.text & "'")
            Dim DistData = New SqlDataAdapter("select * from NetPRm where PolNo='" & PolNo.Text & "'" _
                        & " and EndNo=" & EndNo.Text & " and " _
                        & "LoadNo=" & LoadNo.Text & " and TP='" & Sys.Text & "'", con)

            'If CDbl(NetPRM.Text) <> CDbl(DistNet.Text) + Val(Comission.Text) + Val(War.Text) Then
            'TFacValue = (CDbl(NetPRM.Text) - CDbl(War.Text)) - CDbl(DistNet.Text)
            'End If
            DistData.Fill(Dist)
            ExtraValue = 0

            If Dist.Tables(0).Rows.Count = 0 Then
                Dim Oldadpt = New SqlDataAdapter("select * from NetPrm where PolNo='" & PolNo.Text & "' and EndNo=" & EndNo.Text & " and LoadNo=" & LoadNo.Text & "", con)

                Oldadpt.Fill(OldD)
                If OldD.Tables(0).Rows.Count <> 0 Then
                    ExecConn("Delete NetPrm where PolNo='" & PolNo.Text & "' and EndNo=" & EndNo.Text & " and LoadNo=" & LoadNo.Text & "", con)
                Else

                End If

                For i As Integer = 1 To Val(EndCnt.Text)
                    If EndFile.Tables(0).Rows.Count <> Val(EndCnt.Text) Then
                        If Val(War.Text) <> 0 And i = 1 Then War.Text = War.Text
                        'If Sys.text = "04" Or Sys.text = "05" Then
                        'DistPolicy(CDbl(NetPRM.Text) / Val(EndCnt.Text), CDbl(SumInsured.Text) / Val(EndCnt.Text))
                        'Else
                        DistPolicy(Format(NetPRM.Text / Val(EndCnt.Text) - (War.Text / Val(EndCnt.Text)), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) - Val(Comission.Text) / Val(EndCnt.Text), Format(CDbl(SumInsured.Text) / Val(EndCnt.Text), IIf(Exc.Text = 1, "###,#0.0", "###,#0.0")))
                        'End If
                    Else
                        'DistPolicy((CDbl(DistNet.Text) - Val(Comission.Text) - Val(War.Text) / Val(EndCnt.Text)) * EndFile.Tables(0).Rows(i - 1)("SumIns") / CDbl(SumInsured.Text), EndFile.Tables(0).Rows(i - 1)("SumIns"))
                        If IsGroupedSys(Sys.Text) Then
                            DistPolicy(EndFile.Tables(0).Rows(i - 1)("Premium"), EndFile.Tables(0).Rows(i - 1)("SumIns"))
                        Else
                            DistPolicy((NetPRM.Text - Val(Comission.Text) - (Val(War.Text) / Val(EndCnt.Text))) * EndFile.Tables(0).Rows(i - 1)("SumIns") / CDbl(SumInsured.Text), EndFile.Tables(0).Rows(i - 1)("SumIns"))
                        End If

                    End If

                    ExecSql("INSERT INTO [NetPrm]([PolNo],[LoadNo],[EndNo],[CustNo],[InsAmt],[Net],[War],[Total],[PolD],[Cur],[Exc],[DFm],[DTo]" _
                        & ",[Tp],[Br],[Treaty],[Amount],[Qs],[FirsSup],[SecondSup],[Elective],[LineSlip],[Qsw],[FirsSupw],[Electivew],[Comission],[UserName],[SpecialRet],[Type]) values('" _
                        & Trim(PolNo.Text) & "'," _
                        & LoadNo.Text & "," _
                        & EndNo.Text & "," _
                        & CustNo.Text & "," _
                        & IIf(Sys.Text <> "BB", CDbl(TSumIns.Text), IIf(CDbl(NetPRM.Text) >= 0, 1, -1) * CDbl(TSumIns.Text)) & "," _
                        & CDbl(TNetPrm.Text) & "," _
                        & CDbl(Format(Val(War.Text) / Val(EndCnt.Text), "0.000")) & "," _
                        & CDbl(TOTPRM.Text) & "," _
                        & "CONVERT(DATETIME,'" & Format(CDate(IssuDate.Text), "yyyy-MM-dd") & " 00:00:00',102)," _
                        & Cur.Text & "," _
                        & Val(Exc.Text) & "," _
                        & "CONVERT(DATETIME,'" & Format(CDate(Cfrom.Text), "yyyy-MM-dd") & " 00:00:00',102)," _
                        & "CONVERT(DATETIME,'" & Format(CDate(Cto.Text), "yyyy-MM-dd") & " 00:00:00',102),'" _
                        & Sys.Text & "','" _
                        & Branch.Text & "','" _
                        & TreatyNo.Text & "'," _
                        & CDbl(TRet.Text) & "," _
                        & CDbl(TQS.Text) & "," _
                        & CDbl(TFirstSup.Text) & "," _
                        & CDbl(TSecondSup.Text) & "," _
                        & IIf(Sys.Text <> "BB", CDbl(TFac.Value), IIf(NetPRM.Text >= 0, 1, -1) * CDbl(TFac.Value)) & "," _
                        & CDbl(TLineSlip.Text) & "," _
                        & CDbl(wQS.Text) & "," _
                        & CDbl(wFirstSup.Text) & "," _
                        & CDbl(wFac.Text) & "," _
                        & (Comission.Text / Val(EndCnt.Text)) & ",'" _
                        & "Bulk Dist" & "'," _
                        & CDbl(TSpRet.Value) & ",'Pol')")
                    'IIf(Sys.text <> "04", CDbl(SpRet.Text), CDbl(SpRet.Text) / Val(EndCnt.Text))
                    ' & IIf(SpRetRatio.Value <> 0 And Sys.text <> "04", CDbl(HNetPRM.Text), IIf(Sys.text <> "04", CDbl(TNetPrm.Text), (CDbl(HNetPRM.Text) - CDbl(War.Text) - CDbl(Comission.Text)) / Val(EndCnt.Text))) & "," _
                Next
            End If
            SqlDataSource1.SelectParameters(0).DefaultValue = PolNo.Text
            SqlDataSource1.SelectParameters(1).DefaultValue = EndNo.Text
            SqlDataSource1.SelectParameters(2).DefaultValue = LoadNo.Text
            ASPxGridView1.DataBind()
            If Request("Report") = 1 Then
                Dim SelPolicyRep = "/IMSValidate/" & PolRep(Sys.Text)  'SelectReport(Mid(PolNo.text, 12, 2), Sys.text)
                Parm = Array.CreateInstance(GetType(ReportParameter), IIf(Sys.Text = "OC", 4, 3))
                SetRepPm("PolicyNo", False, GenArray(OrderNo.Text), Parm, 0)
                SetRepPm("EndNo", False, GenArray(Request("EndNo")), Parm, 1)
                SetRepPm("Sys", False, GenArray(Sys.Text), Parm, 2)

                If Sys.Text = "OC" Then SetRepPm("LoadNo", False, GenArray(LoadNo.Text), Parm, 3)
                'If Sys.text <> "06" Then Me.Session.Add("Parms", Parm)
                Session.Add("Parms", Parm)
                'ClientScript.RegisterStartupScript(GetType(), "OpenWindow", "<script> window.open('../OutPutManagement/Preview.aspx?Report=" & SelPolicyRep & "','_self'); </script>")
                ASPxWebControl.RedirectOnCallback("~/Reporting/PreviewPDF.aspx?Report=" & SelPolicyRep & "")
            End If
            con.Close()
        End Using
        'Dim temp As Double = CDbl(TSumIns.Text) / CDbl(Capacity.Text) * (CDbl(HNetPRM.Text) - CDbl(War.Text) - CDbl(Comission.Text))
        'ExtraValue = Format(ExtraValue + (temp / Val(EndCnt.Text) - (temp / Val(EndCnt.Text) * (CDbl(TRQSRCOM.Text) / 100))) * 1.83, "###,#0.000")
        'ExtPrm.Text = ExtraValue
    End Sub

    Public Function GetEndFile(Sys As String) As String
        Dim IssueVal As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select SumInsured from SubSystems where SubSysNo='" & Sys & "'
                                                AND Branch=dbo.MainCenter()", con)
            dbadapter.Fill(IssueVal)
            Select Case Sys
                Case "MC", "MB", "MA"
                    GetEndFile = If(Request("EndNo") = 0,
                        IssueVal.Tables(0).Rows(0)(0).ToString,
                        DirectCast(IIf(stat, IssueVal.Tables(0).Rows(0)(0).ToString, "New" + IssueVal.Tables(0).Rows(0)(0)), String))
                Case Else
                    GetEndFile = IssueVal.Tables(0).Rows(0)(0).ToString
            End Select
            con.Close()
        End Using
    End Function

    Protected Function GetFirstOrder(Sys As String, Pol As String) As String
        Dim IssueVal As New DataSet
        Dim Temp = "PolicyFile.PolNo"
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select Max(PolicyFile.orderNo) from PolicyFile " _
                               & " Inner Join " & GetGroupFile(Sys) & " On " _
                               & "PolicyFile.OrderNo=" & GetGroupFile(Sys) & ".OrderNo And " _
                               & "PolicyFile.EndNo=" & GetGroupFile(Sys) & ".EndNo And " _
                               & "PolicyFile.LoadNo=" & GetGroupFile(Sys) & ".LoadNo And " _
                               & "PolicyFile.SubIns=" & GetGroupFile(Sys) & ".SubIns  " _
                               & " where " & Temp & "='" & Trim(Pol) & "'" _
                               & " And PolicyFile.SubIns='" & Sys & "'" _
                            , con)

            dbadapter.Fill(IssueVal)
            Return IssueVal.Tables(0).Rows(0)(0)
            'On Error Resume Next
            con.Close()
        End Using
    End Function

    Protected Function WarPrmCalc() As Double
        Dim WarprmCalc1 As New DataSet
        Dim Temp As String
        Dim Tempend As Integer
        Temp = If(OrderNo.Text = PolNo.Text, "policyFile.OrderNo", "PolicyFile.PolNo")
        Tempend = If(Val(EndNo.Text) > 0 And PolNo.Text = OrderNo.Text, Val(EndNo.Text), Val(EndNo.Text) - 1)
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("Select Sum(GodFile.WarPrm) As WarPrm FROM GodFile INNER Join PolicyFile On" _
                  & " GodFile.OrderNo=PolicyFile.OrderNo And GodFile.SubIns=PolicyFile.SubIns WHERE " & Temp & "='" & RTrim(PolNo.Text) & "' And GodFile.EndNo= " & Tempend & "", con)

            'Dim dbadapter = New Data.SqlClient.SqlDataAdapter("(SELECT Sum(WarPrm) As WarPrm FROM godfile  inner join policyfile on godfile.orderno =" _
            '                     & "(Select min(PolF.orderno) from PolicyFile AS PolF where godfile.endno<" & Val(Request("EndNo")) & " AND PolF.PolNo='" & RTrim(PolNo.Text) & "' ) AND godfile.SubIns = '" & Sys.text & "' ) ", ConnLocal)
            dbadapter.Fill(WarprmCalc1)

            If IsDBNull(WarprmCalc1.Tables(0).Rows(0)("WarPrm")) Or WarprmCalc1.Tables(0).Rows(0)("WarPrm") < 0 Then
                WarPrmCalc = 0
            Else
                WarPrmCalc = If(NetPRM.Text < 0,
                    DirectCast(IIf(WarprmCalc1.Tables(0).Rows(0)("WarPrm") = DB.Tables(0).Rows(0)("WarPrm"), -1 * DB.Tables(0).Rows(0)("WarPrm"), DB.Tables(0).Rows(0)("WarPrm") - WarprmCalc1.Tables(0).Rows(0)("WarPrm")), Double),
                    DirectCast(Math.Abs(DB.Tables(0).Rows(0)("WarPrm") - CDbl(WarprmCalc1.Tables(0).Rows(0)("WarPrm"))), Double))
            End If
            con.Close()
        End Using
    End Function

    Protected Function GetEndData(Sys As String, Order As String, Pol As String, Endn As Integer, load As Integer) As Boolean
        Dim IssueVal As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("select * from " & GetGroupFile(Sys) & "  where " _
                               & " OrderNo='" & Trim(Order) & "' and EndNo=" & Endn & "" _
                               & " and LoadNo=" & load & " and SubIns='" & Sys & "'" _
                            , con)
            dbadapter.Fill(IssueVal)
            If IssueVal.Tables(0).Rows.Count = 0 Then
                Return False
            Else
                Return True
            End If
            con.Close()
        End Using
    End Function

    Protected Sub DistPolicy(NET As Double, SumINS As Double)

        Dim cnt As Integer
        Dim DistTreaty As New DataSet

        'If PolNo.Text = "TT002024FB/00004" Then
        '    PolNo.Text = "TT002024FB/00004"
        'Else

        'End If

        If IsGroupedSys(Sys.Text) Or Sys.Text = "MC" Or Sys.Text = "MB" Or Sys.Text = "MA" Or Sys.Text = "OC" Then
            cnt = 1
        Else
            cnt = Val(EndCnt.Text)
        End If

        Dim TreatyData = New SqlDataAdapter("select * from TRREGFLE where TreatyNo='" & TreatyNo.Text & "'", Conn)
        TreatyData.Fill(DistTreaty)
        Dim WsI As Double = SumINS

        If CDbl(DistTreaty.Tables(0).Rows(0)("TRCAPCTY")) + CDbl(DistTreaty.Tables(0).Rows(0)("TRLSAMT")) >= (Math.Abs(SumINS) * Val(Exc.Text)) / Val(EndCnt.Text) Then
        Else
            FacRatio.PositionStart = 100 - (Val((DistTreaty.Tables(0).Rows(0).Item("TRCAPCTY") + DistTreaty.Tables(0).Rows(0).Item("TRLSAMT")) / Val(Exc.Text)) / Math.Abs(SumINS) * 100)
            FacRatio.MinValue = FacRatio.PositionStart
        End If

        TSumIns.Text = Format(SumINS, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
        TNetPrm.Text = Format(NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))

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
        'TFac.Value = Format(FacRatio.Value * NET / 100, IIf(Exc.Value = 1, "###,#0.000", "###,#0.00")) ' - CDbl(DistNet.Text)
        'TSpRet.Value = Format(SpRetRatio.Value * TFac.Value / 100, IIf(Exc.Value = 1, "###,#0.000", "###,#0.00"))
        'TFac.Value = Math.Abs(CDbl(TSpRet.Value) - CDbl(TFac.Value))
        'NET = Format(NET - (NET * FacRatio.Value / 100), IIf(Exc.Value = 1, "###,#0.000", "###,#0.00"))
        'SumINS = Format(SumINS - (SumINS * FacRatio.Value / 100), IIf(Exc.Value = 1, "###,#0.000", "###,#0.00"))
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
                wFac.Text = Format(War.Text / Val(EndCnt.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
            Else
                wFac.Text = Format(DistWar.Text / Val(EndCnt.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
            End If
            wQS.Text = Format(DistWar.Text / cnt, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
        End If

        With DistTreaty.Tables(0).Rows(0)

            '------- 1

            If Math.Abs(SumINS) <= (.Item("TRRETAMT") / Val(Exc.Text)) + (.Item("TRQSRAMT") / Val(Exc.Text)) Then
                On Error Resume Next
                TRet.Text = Format(.Item("TRRETAMT") / Val(Exc.Text) / ((.Item("TRRETAMT") / Val(Exc.Text)) + (.Item("TRQSRAMT") / Val(Exc.Text))) * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))

                If TRet.Text = "NaN" Or TRet.Text = "ليس رقمًا" Then
                    TRet.Text = 0
                Else

                End If

                TQS.Text = Format(.Item("TRQSRAMT") / Val(Exc.Text) / ((.Item("TRRETAMT") / Val(Exc.Text)) + (.Item("TRQSRAMT") / Val(Exc.Text))) * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))

                If TQS.Text = "NaN" Or TQS.Text = "ليس رقمًا" Then
                    TQS.Text = 0
                Else

                End If
            End If
            If (Math.Abs(SumINS) > (.Item("TRRETAMT") / Val(Exc.Text)) + (.Item("TRQSRAMT") / Val(Exc.Text))) And (Math.Abs(SumINS) <= (.Item("TRCAPCTY") / Val(Exc.Text))) Then
                If Math.Abs(SumINS) <= (.Item("TRRETAMT") / Val(Exc.Text)) + (.Item("TRQSRAMT") / Val(Exc.Text)) + (.Item("TR1STAMT") / Val(Exc.Text)) Then
                    TRet.Text = Format(.Item("TRRETAMT") / Val(Exc.Text) / Math.Abs(SumINS) * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    TQS.Text = Format(.Item("TRQSRAMT") / Val(Exc.Text) / Math.Abs(SumINS) * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))

                    TFirstSup.Text = Format((Math.Abs(SumINS) - (.Item("TRRETAMT") / Val(Exc.Text)) - (.Item("TRQSRAMT") / Val(Exc.Text))) / Math.Abs(SumINS) * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                Else
                    TRet.Text = Format(.Item("TRRETAMT") / Val(Exc.Text) / Math.Abs(SumINS) * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    TQS.Text = Format(.Item("TRQSRAMT") / Val(Exc.Text) / Math.Abs(SumINS) * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    'If TP = 1 Then
                    TFirstSup.Text = Format(.Item("TR1STAMT") / Val(Exc.Text) / Math.Abs(SumINS) * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    TSecondSup.Text = Format((Math.Abs(SumINS) - ((.Item("TRCAPCTY") - .Item("TR2STAMT")) / Val(Exc.Text))) / Math.Abs(SumINS) * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))

                End If
            End If
            '-------- 2
            Dim m As Double
            m = ((.Item("TRCAPCTY") / Val(Exc.Text)) + (.Item("TR2STAMT") / Val(Exc.Text))) / cnt
            If Math.Abs(SumINS) > (.Item("TRCAPCTY") / Val(Exc.Text)) Then
                TRet.Text = Format(.Item("TRRETAMT") / Val(Exc.Text) / Math.Abs(SumINS) * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) / cnt 'Retention
                TQS.Text = Format(.Item("TRQSRAMT") / Val(Exc.Text) / Math.Abs(SumINS) * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) / cnt 'Quatashare

                TFirstSup.Text = Format(.Item("TR1STAMT") / Val(Exc.Text) / Math.Abs(SumINS) * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) / cnt '1st surplus
                TSecondSup.Text = Format(.Item("TR2STAMT") / Val(Exc.Text) / Math.Abs(SumINS) * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) / cnt '2nd surplus
                If Sys.Text = "MC" Or Sys.Text = "MB" Or Sys.Text = "MA" Or Sys.Text = "OC" Then
                    If SumINS - (.Item("TRCAPCTY") / Val(Exc.Text)) <= (.Item("TRLSAMT") / Val(Exc.Text)) And (Sys.Text = "MC" Or Sys.Text = "MB" Or Sys.Text = "MA" Or Sys.Text = "OC") And (.Item("TRCAPCTY") / cnt) <> 0 Then
                        TLineSlip.Text = Format((SumINS - (.Item("TRCAPCTY") / Val(Exc.Text))) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) / cnt 'lineslip
                    Else
                        If (.Item("TRCAPCTY") / Val(Exc.Text)) <> 0 Then
                            TLineSlip.Text = Format(.Item("TRLSAMT") / Val(Exc.Text) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                            'TFac.Text = Format((SumINS - (.Item("TRCAPCTY") / Val(Exc.Text)) - (4000000 / Val(Exc.Text))) / SumINS * NET, "0.000\0") / Val(EndCnt.Text)
                            'TFac.Text = Format(NET - (CDbl(TRet.Value) + CDbl(TQS.Text) + CDbl(TSecondSup.Text) + CDbl(TFirstSup.Text) + CDbl(TLineSlip.Text)), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                        Else
                            'TFac.Text = Format((SumINS - (.Item("TRCAPCTY") / Val(Exc.Text))) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) / cnt
                        End If
                    End If
                    TFac.Value = NET - (CDbl(TRet.Value) + CDbl(TQS.Value) + CDbl(TSecondSup.Value) + CDbl(TFirstSup.Text) + CDbl(TLineSlip.Text))
                Else
                    If Math.Abs(SumINS) >= m And FacRatio.Value = 0 Then
                        If SpRetRatio.Value = 0 And HF.Value = 0 Then
                            TFac.Value = Format(NET - (CDbl(TRet.Value) + CDbl(TQS.Value) + CDbl(TSecondSup.Value) + CDbl(TFirstSup.Text) + CDbl(TLineSlip.Text)), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) - SpRet.Value 'Format((SumINS - (.Item("TRCAPCTY") / Val(Exc.Text))) / SumINS * NET, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) '/ Val(EndCnt.Text)
                        Else
                            'TFac.Text = Format(NET - (CDbl(TRet.Value) + CDbl(TQS.Text) + CDbl(TSecondSup.Text) + CDbl(TFirstSup.Text) + CDbl(TLineSlip.Text)), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00")) - SpRet.Value
                        End If
                    Else
                        TFac.Value = NET - (CDbl(TRet.Value) + CDbl(TQS.Value) + CDbl(TSecondSup.Value) + CDbl(TFirstSup.Text) + CDbl(TLineSlip.Text))
                    End If
                End If
            Else
            End If

            If Val(War.Text) <> 0 Then
                If Math.Abs(WsI) <= (.Item("TRRETAMT") + .Item("TRQSRAMT")) / Val(Exc.Text) Then
                    wQS.Text = Format((War.Text / Val(EndCnt.Text)) - wFac.Text, IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                Else
                    If Math.Abs(WsI) <= (.Item("TRCAPCTY") / Val(Exc.Text)) Then
                        wQS.Text = Format((.Item("TRRETAMT") + .Item("TRQSRAMT")) / Val(Exc.Text) / WsI * (War.Text - wFac.Text) / Val(EndCnt.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                        wFirstSup.Text = Format((WsI - ((.Item("TRRETAMT") + .Item("TRQSRAMT")) / Val(Exc.Text))) / WsI * (War.Text - wFac.Text) / Val(EndCnt.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    Else
                        wQS.Text = Format((.Item("TRRETAMT") + .Item("TRQSRAMT")) / Val(Exc.Text) / WsI * War.Text / Val(EndCnt.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                        wFirstSup.Text = Format(.Item("TR1STAMT") / Val(Exc.Text) / WsI * War.Text / Val(EndCnt.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                        wFac.Text = Format((WsI - (.Item("TRCAPCTY") / Val(Exc.Text))) / WsI * War.Text / Val(EndCnt.Text), IIf(Exc.Text = 1, "###,#0.000", "###,#0.00"))
                    End If
                End If
            End If
            'TNetPrm.Text = NET
            ExtPrm.Text = Format(ExtraValue, "###,#0.000")
            If WarFacRatio.Value <> 0 Then

            End If
        End With
        'If TFac.Value <> 0 Then
        '    TFac.Value = 0
        'Else
        '    TFac.Value = 0
        'End If
    End Sub

    Public Function Valu(Vr As Object) As Double
        If IsNothing(Vr) Then
            Valu = 0
        Else
            Valu = If(Val(Vr) <> 0, Val(Vr), If(Vr = True, 1, 0))
        End If
    End Function

    Protected Sub ASPxButton3_Click(sender As Object, e As EventArgs) Handles ASPxButton3.Click
        If PolNo.Text = OrderNo.Text Then
            ExecSql("update PolicyFile Set EXTPRM=" & CDbl(ExtPrm.Text) & ",ExtraRate=163 where " _
                               & "  PolicyFile.OrderNo='" & Trim(OrderNo.Text) & "' and PolicyFile.EndNo=" & Request("EndNo") _
                               & " and PolicyFile.LoadNo=" & Request("LoadNo") & " and PolicyFile.SubIns='" & Sys.Text & "'" _
                               & " and PolicyFile.Branch='" & Branch.Text & "'")
            ExecSql("delete netprm where " _
                              & "  PolNo='" & Trim(OrderNo.Text) & "' and EndNo=" & Request("EndNo") _
                              & " and LoadNo=" & Request("LoadNo") & " and TP='" & Sys.Text & "'" _
                              & " and Br='" & Branch.Text & "'")
        Else
            ExecSql("update PolicyFile Set Financed=1 where " _
                                & "  PolicyFile.OrderNo='" & Trim(OrderNo.Text) & "' and PolicyFile.EndNo=" & Request("EndNo") _
                                & " and PolicyFile.LoadNo=" & Request("LoadNo") & " and PolicyFile.SubIns='" & Sys.Text & "'" _
                                & " and PolicyFile.Branch='" & Branch.Text & "'")
        End If
    End Sub

End Class