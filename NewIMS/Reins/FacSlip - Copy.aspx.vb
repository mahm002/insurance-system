Imports System.Data.SqlClient
Imports System.Drawing
Imports DevExpress.Web

Public Class FacSlip
    Inherits Page

    Private ReadOnly Poldtl, Slpdt As New DataSet

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ReCom.DataBind()

        Dim STR, STR1
        If Request("War") = 1 Then
            STR = "FacWarRef"
            STR1 = "ElectiveWarRatio"
        Else
            STR = "FacPolRef"
            STR1 = "ElectiveRatio"
        End If
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim DistHistory As New DataSet

            Dim DistHadpt = New SqlDataAdapter("SELECT Np.PolNo," & STR1 & "," & STR & ",100-isnull(sum(AcceptedShare),0) As RemainFac ,isnull(sum(AcceptedShare),0) As Sold FROM FacClosingSlips " _
                            & " right JOIN NetPrm as np on FacClosingSlips.PolRef=np." & STR & " and FacClosingSlips.EndNo=np.EndNo and FacClosingSlips.LoadNo=np.LoadNo " _
                            & " where np.PolNo='" & Request("PolNo") & "' and np.EndNo=" & Request("EndNo") & " and np.LoadNo=" & Request("LoadNo") & "" _
                            & " group by " & STR & "," & STR1 & ",Np.PolNo " _
                            & " having (100-isnull(sum(AcceptedShare),0) <= 100) And " & STR1 & "<> 0 order by 100-isnull(sum(AcceptedShare),0) ", con)

            DistHadpt.Fill(DistHistory)

            If DistHistory.Tables(0).Rows(0)("Sold") <> 0 And DistHistory.Tables(0).Rows(0)("Sold") <= 100 Then
                CheckRemainlbl.Text = "النسبة المتبقية من الاختياري " + DistHistory.Tables(0).Rows(0)("RemainFac").ToString + " % "
                CheckRemainlbl.ForeColor = Color.DarkGreen
                AcceptedShare.ClientEnabled = IIf(Request("Operation") = "Edit" Or Request("SlipNo") = "0", True, False)
            Else
                CheckRemainlbl.Text = "النسبة المتبقية من الاختياري " + DistHistory.Tables(0).Rows(0)("RemainFac").ToString + " % "
                CheckRemainlbl.ForeColor = Color.Red
                AcceptedShare.ClientEnabled = IIf(Request("SlipNo") = "0", True, False)
            End If

            If Request("SlipNo") Is Nothing Then
                Exit Sub
            End If
            If Request("SlipNo") <> "0" Then
                Save.Text = IIf(Request("Operation") = "Edit", "UPDATE", "CANCEL")
                If Request("Operation") = "Edit" Then
                Else
                    Save.Theme = "Office365"
                    'Save.ForeColor = Color.Red
                End If
            End If

            If IsCallback Then
            Else
                If Request("SlipNo") = "0" Then
                    SlipDate.Value = Date.Today()
                    Call FillBaseData(Request("PolNo"), Request("EndNo"), Request("LoadNo"), Request("Ref"))
                Else
                    If IsCallback Then
                    Else
                        Call FillSavedDate(Request("SlipNo"))
                    End If
                End If
            End If
            con.Close()
        End Using
    End Sub

    Private Sub FillBaseData(Pol As String, Endn As Int16, Load As Int16, Sn As String)
        Dim STR
        If Request("War") = 1 Then
            STR = "FacWarRef"
        Else
            STR = "FacPolRef"
        End If
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim Poladpt = New SqlDataAdapter("SELECT policyfile.PolNo,policyfile.EndNo,policyfile.LoadNo,
rtrim(policyfile.PolNo)+'/'+rtrim(cast(policyfile.EndNo as nchar))+'/'+rtrim(cast(policyfile.LoadNo As nchar)) As PolKey, Rtrim(TpName) As Currency,PolicyFile.Currency As Cur,
policyfile.ExcRate,cast(issudate as date) as IssuDate,CustName,SubIns,Branch," & STR & ",netprm.DFm,netprm.DTo,
CASE  WHEN subins = 'MC' or subins='MA' or subins='MB' or subins='OC' THEN
 (select rtrim(TripFrom) +' إلى '+ rtrim(TripTo) from godfile where godfile.orderno=PolicyFile.OrderNo and godfile.SubIns=PolicyFile.SubIns)
      WHEN subins='CR' or subins='ER'  THEN
	  (select rtrim(cast(WorkStart as nvarchar(10)))+ ' إلى '+rtrim(cast(WorkEnd as nvarchar(10))) from ProjectFile where ProjectFile.orderno=PolicyFile.OrderNo and ProjectFile.SubIns=PolicyFile.SubIns)
      ELSE
	  rtrim(cast(cast(DFm as date) as nvarchar))+ ' إلى '+rtrim(cast(cast(DTo as date) as nvarchar))
END as Period,
dbo.BranchName(Branch) As BranchName, dbo.SysName(SubIns) As Sys,
isnull([dbo].[policyreport](SubIns),0) As Report,
netprm.TreatyCapacity,
round(sum(BaseSI),3) AS SumIns,
round(sum(BasePRM),3) AS NETPRM,
round(sum(netprm.net),3) AS Reinsnet,
round(sum(netprm.war),3) AS ReinsnetWar,

round(sum(TotreinsPRM),3) as TotreinsPRM,
round(sum(TotreinsWarPRM),3) as TotreinsWarPRM,

sum(TreatyDistPrm) as TreatyDistPrm,
Sum(TreatyDisSI) As TreatyDisSI,
 TreatyRatio,

round(Sum(TreatyDistWar),3) As TreatyDistWar,
round(Sum(TreatyDistWarSI),3) as TreatyDistWarSI,
TreatyWarRatio,

-- FACULTATIVE PREMIUM
round(Sum(ElectivePRM),3) As ElectivePRM,
round(sum(ElectiveSI),3) as ElectiveSI,
ElectiveRatio,

--FACULTATIVE WAR
round(Sum(ElectiveWarPRM),3) as ElectiveWarPRM,
round(sum(ElectiveWarSI),3) as ElectiveWarSI,
ElectiveWarRatio,

--SPECIAL RETENTION
Sum(SpecialRetPRM) as SpecialRetPRM,
SpecialRetRatio
from PolicyFile left join
NetPrm on netprm.PolNo=PolicyFile.PolNo and netprm.EndNo=PolicyFile.EndNo and netprm.LoadNo=PolicyFile.LoadNo
left join EXTRAINFO on PolicyFile.Currency=EXTRAINFO.TPNo and EXTRAINFO.TP='Cur'
LEFT OUTER JOIN
CustomerFile on PolicyFile.CustNo=CustomerFile.CustNo

where Currency=TPNo AND LEFT(OrderNo,2)<>'IW' AND IssuDate is not null
--and (netprm.PolNo=@PolNo) and (netprm.EndNo=@EndNo) and (netprm.LoadNo=@LoadNo)
and " & STR & "='" & Sn & "'
group by policyfile.PolNo,policyfile.EndNo,policyfile.LoadNo,OrderNo,TpName,PolicyFile.IssuDate,PolicyFile.NETPRM,
PolicyFile.TOTPRM,CustomerFile.CustName,PolicyFile.SubIns,netprm.TreatyCapacity,NetPrm.DFm,NetPrm.DTo,
" & STR & ",Currency,
PolicyFile.Branch,PolicyFile.EntryDate,PolicyFile.ExcRate,NetPrm.Net,NetPrm.War,
TreatyDistPrm,TreatyDisSI,TreatyRatio,BaseSI,BasePRM,TotreinsPRM,TotreinsWarPRM,TreatyDistWar,TreatyDistWarSI,TreatyWarRatio,
ElectivePRM,ElectiveSI,ElectiveRatio,ElectiveWarPRM,ElectiveWarSI,ElectiveWarRatio,SpecialRetPRM,SpecialRetRatio

HAVING round((sum(netprm.Elective)),3)<>0 or round((sum(netprm.Electivew)),3)<>0
ORDER BY EntryDate DESC, OrderNo DESC", con)
            Dim unused = Poladpt.Fill(Poldtl)

            PolNo.Text = Pol
            EndNo.Text = Endn
            LoadNo.Text = Load
            InsuredName.Text = Poldtl.Tables(0).Rows(0)("CustName")
            InsuranceType.Text = Poldtl.Tables(0).Rows(0)("Sys").ToString.Trim + IIf(Request("War") = 1, "/ War", "")
            Period.Value = Poldtl.Tables(0).Rows(0)("Period")
            Cur.Text = Poldtl.Tables(0).Rows(0)("Cur")
            ExcRate.Text = Poldtl.Tables(0).Rows(0)("ExcRate")
            SubIns.Text = Poldtl.Tables(0).Rows(0)("SubIns")
            PolRef.Text = Request("Ref")
            'CoverTo.Value = Poldtl.Tables(0).Rows(0)("DTo")
            OriginalSI.Text = IIf(Request("War") = 1, Poldtl.Tables(0).Rows(0)("ElectiveWarSI"), Poldtl.Tables(0).Rows(0)("ElectiveSI"))
            OriginalNet.Text = IIf(Request("War") = 1, Poldtl.Tables(0).Rows(0)("ElectiveWarPRM"), Poldtl.Tables(0).Rows(0)("ElectivePRM"))
            con.Close()
        End Using
    End Sub

    Private Sub FillSavedDate(Slip As String)
        If Request("Operation") = "Edit" Then
            SlipNo.ClientEnabled = False
        Else
            SlipNo.ClientEnabled = True
            ReCom.ClientEnabled = False
            CommisionRatio.ClientEnabled = False
            AcceptedShare.ClientEnabled = False
            CancelLbl.ClientVisible = True
            CancelLbl.Text = "في حالة الموافقة سيتم اصدار قسيمة إلغاء للقسيمة رقم " & Request("SlipNo")
            CancelLbl.ForeColor = Color.Red
        End If

        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim SlipData = New SqlDataAdapter("Select * FROM FacClosingSlips where SlipNo='" & Slip & "'", con)
            Dim unused = SlipData.Fill(Slpdt)

            SlipNo.Value = IIf(Request("Operation") = "Edit", Slpdt.Tables(0).Rows(0)("SlipNo"), "")

            PolNo.Value = Slpdt.Tables(0).Rows(0)("PolNo")
            EndNo.Value = Slpdt.Tables(0).Rows(0)("EndNo")
            LoadNo.Value = Slpdt.Tables(0).Rows(0)("LoadNo")
            Period.Value = Slpdt.Tables(0).Rows(0)("Period")
            SlipDate.Value = IIf(Request("Operation") = "Edit", Slpdt.Tables(0).Rows(0)("SlipDate"), Today.Date())
            InsuranceType.Text = Slpdt.Tables(0).Rows(0)("InsuranceType").ToString.Trim '+ IIf(Request("War") = 1, "/ War", "")
            ReCom.Value = Slpdt.Tables(0).Rows(0)("Recom")
            InsuredName.Text = Slpdt.Tables(0).Rows(0)("InsuredName")
            RefNo.Value = Slpdt.Tables(0).Rows(0)("RefNo")
            OriginalSI.Value = Slpdt.Tables(0).Rows(0)("OriginalSI")
            OriginalNet.Value = Slpdt.Tables(0).Rows(0)("OriginalNet")
            AcceptedShare.Value = Slpdt.Tables(0).Rows(0)("AcceptedShare") * IIf(Request("Operation") = "Edit", 1, -1)
            CommisionRatio.Value = Slpdt.Tables(0).Rows(0)("CommisionRatio")
            FacSI.Value = Slpdt.Tables(0).Rows(0)("FacSI") * IIf(Request("Operation") = "Edit", 1, -1)
            FacNet.Value = Slpdt.Tables(0).Rows(0)("FacNet") * IIf(Request("Operation") = "Edit", 1, -1)
            Commision.Value = Slpdt.Tables(0).Rows(0)("Commision") * IIf(Request("Operation") = "Edit", 1, -1)
            Balance.Value = Slpdt.Tables(0).Rows(0)("Balance") * IIf(Request("Operation") = "Edit", 1, -1)
            Cur.Text = Slpdt.Tables(0).Rows(0)("Currency")
            ExcRate.Text = Slpdt.Tables(0).Rows(0)("ExRate")
            SubIns.Text = Slpdt.Tables(0).Rows(0)("SubIns")
            PolRef.Text = Request("Ref")
            con.Close()
        End Using
        'OriginalSI.Text = IIf(Request("War") = 1, Slpdt.Tables(0).Rows(0)("ElectiveWarSI"), Slpdt.Tables(0).Rows(0)("ElectiveSI"))
        ' OriginalNet.Text = IIf(Request("War") = 1, Slpdt.Tables(0).Rows(0)("ElectiveWarPRM"), Slpdt.Tables(0).Rows(0)("ElectivePRM"))
    End Sub

    Protected Sub Scbp_Callback(source As Object, e As CallbackEventArgs)
        Select Case e.Parameter
            Case "ShareChange"
                'If AcceptedShare.Value <= 0 Then
                '    MsgBox("يجب أن اكون النسبة أكبر من 0")
                '    Exit Sub
                'Else
                'Dim script As String = "alert('يجب أن تكون النسبة أكبر من 0')"
                If AcceptedShare.Value <= 0 Then
                    CommLbl.ClientVisible = True
                    CommLbl.Text = "يجب أن تكون النسبة أكبر من 0"
                    CommLbl.ForeColor = Color.Red
                    'AcceptedShare.Focus()
                    'Exit Sub
                Else
                    CommLbl.ClientVisible = False
                    e.Result = Convert.ToDecimal(AcceptedShare.Value) * Convert.ToDecimal(OriginalSI.Value) / 100
                End If
                Dim STR, STR1
                If Request("War") = 1 Then
                    STR = "FacWarRef"
                    STR1 = "ElectiveWarRatio"
                Else
                    STR = "FacPolRef"
                    STR1 = "ElectiveRatio"
                End If
                Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    Else

                    End If
                    con.Open()
                    Dim DistHistory As New DataSet

                    Dim DistHadpt = New SqlDataAdapter("SELECT Np.PolNo," & STR1 & "," & STR & ",100-isnull(sum(AcceptedShare),0) As RemainFac ,isnull(sum(AcceptedShare),0) As Sold FROM FacClosingSlips " _
                            & " right JOIN NetPrm as np on FacClosingSlips.PolRef=np." & STR & " and FacClosingSlips.EndNo=np.EndNo and FacClosingSlips.LoadNo=np.LoadNo " _
                            & " where np.PolNo='" & Request("PolNo") & "' and np.EndNo=" & Request("EndNo") & " and np.LoadNo=" & Request("LoadNo") & "" _
                            & " group by " & STR & "," & STR1 & ",Np.PolNo " _
                            & " having (100-isnull(sum(AcceptedShare),0) <= 100) And " & STR1 & "<> 0 order by 100-isnull(sum(AcceptedShare),0) ", con)

                    DistHadpt.Fill(DistHistory)

                    If DistHistory.Tables(0).Rows.Count <> 0 Then
                        If DistHistory.Tables(0).Rows(0)("Sold") <> 0 And DistHistory.Tables(0).Rows(0)("Sold") < 100 Then
                            CheckRemainlbl.Text = "النسبة المتبقية من الاختياري " + DistHistory.Tables(0).Rows(0)("RemainFac").ToString + " % "
                            CheckRemainlbl.ForeColor = Color.DarkGreen
                        Else
                            CheckRemainlbl.Text = "النسبة المتبقية من الاختياري " + DistHistory.Tables(0).Rows(0)("RemainFac").ToString + " % "
                            CheckRemainlbl.ForeColor = Color.Red
                            AcceptedShare.ClientEnabled = IIf(Request("SlipNo") <> "0", True, False)

                        End If
                    End If
                    con.Close()
                End Using
            Case "SaveData"
                If ReCom.IsValid And SlipNo.IsValid And Period.IsValid And InsuredName.IsValid Then
                    If Request("SlipNo") = "0" Then
                        If AcceptedShare.Value <= 0 Then
                            CommLbl.ClientVisible = True
                            CommLbl.Text = "يجب أن تكون النسبة أكبر من 0"
                            CommLbl.ForeColor = Color.Red
                            'AcceptedShare.Focus()
                            'Exit Sub
                        Else
                            CommLbl.ClientVisible = False
                            'e.Result = Convert.ToDecimal(AcceptedShare.Value) * Convert.ToDecimal(OriginalSI.Value) / 100
                        End If
                        ExecConn("INSERT INTO FacClosingSlips (SlipNo, PolNo, EndNo, LoadNo, SlipDate, PolRef, SubIns, InsuranceType, InsuredName
                                 ,Period ,OriginalSI ,OriginalNet ,Currency ,ExRate, RefNo,Recom ,AcceptedShare,FacSI,FacNet
                                  ,CommisionRatio ,Commision ,Balance)
                                 VALUES
                    ('" & UCase(SlipNo.Value) & "'" _
                    & ",'" & PolNo.Value & "'" _
                    & "," & EndNo.Value & "" _
                    & "," & LoadNo.Value & "" _
                    & ",'" & Format(SlipDate.Value, "yyyy-MM-dd") & "'" _
                    & ",'" & Request("Ref") & "'" _
                    & ",'" & SubIns.Text & "'" _
                    & ",'" & InsuranceType.Value & "'" _
                    & ",'" & InsuredName.Value & "'" _
                    & ",'" & Period.Value & "'" _
                    & "," & OriginalSI.Value & "" _
                    & "," & OriginalNet.Value & "" _
                    & "," & Cur.Text & "" _
                    & "," & ExcRate.Text & "" _
                    & ",'" & RefNo.Value & "'" _
                    & "," & ReCom.Value & "" _
                    & "," & AcceptedShare.Value & "" _
                    & "," & FacSI.Value & "" _
                    & "," & FacNet.Value & "" _
                    & "," & CommisionRatio.Value & "" _
                    & "," & Commision.Value & "" _
                    & "," & Balance.Value & ")", Conn)

                        ASPxWebControl.RedirectOnCallback("../Reins/FacSlip.aspx?PolNo=" & PolNo.Value & "&EndNo=" & EndNo.Value & "&LoadNo=" & LoadNo.Value & "&Ref=" & Request("Ref") & "&War=" & Request("War") & "&SlipNo=" & SlipNo.Value & "&Operation=Edit")
                    Else
                        If Request("Operation") = "Edit" Then
                            'Dim script As String = "alert('يجب أن تكون النسبة أكبر من 0')"
                            If AcceptedShare.Value <= 0 Then
                                CommLbl.ClientVisible = True
                                CommLbl.Text = "يجب أن تكون النسبة أكبر من 0"
                                CommLbl.ForeColor = Color.Red
                                'AcceptedShare.Focus()
                                'Exit Sub
                            Else
                                CommLbl.ClientVisible = False
                                'e.Result = Convert.ToDecimal(AcceptedShare.Value) * Convert.ToDecimal(OriginalSI.Value) / 100
                            End If
                            ExecConn("UPDATE [dbo].[FacClosingSlips] " _
                            & " SET [SlipDate] ='" & Format(SlipDate.Value, "yyyy-MM-dd") & "'" _
                            & ", [PolRef] = '" & Request("Ref") & "'" _
                            & ", [SubIns] = '" & SubIns.Text & "'" _
                            & ", [InsuranceType] = '" & InsuranceType.Text & "'" _
                            & ", [InsuredName] = '" & InsuredName.Value & "'" _
                            & ", [Period] = '" & Period.Value & "'" _
                            & ", [OriginalSI] = " & OriginalSI.Value & "" _
                            & ", [OriginalNet] = " & OriginalNet.Value & "" _
                            & ", [Currency] = " & Cur.Text & "" _
                            & ", [ExRate] = " & ExcRate.Text & "" _
                            & ", [RefNo] = '" & RefNo.Value & "'" _
                            & ", [Recom] = " & ReCom.Value & "" _
                            & ", [AcceptedShare] = " & AcceptedShare.Value & "" _
                            & ", [CommisionRatio] = " & CommisionRatio.Value & "" _
                            & ", [FacSI] = " & FacSI.Value & "" _
                            & ", [FacNet] = " & FacNet.Value & "" _
                            & ", [Commision] = " & Commision.Value & "" _
                            & ", [Balance] = " & Balance.Value & "" _
                            & " WHERE SlipNo='" & Request("SlipNo") & "'", Conn)
                            ASPxWebControl.RedirectOnCallback("../Reins/FacSlip.aspx?PolNo=" & PolNo.Value & "&EndNo=" & EndNo.Value & "&LoadNo=" & LoadNo.Value & "&Ref=" & Request("Ref") & "&War=" & Request("War") & "&SlipNo=" & SlipNo.Value & "&Operation=Edit")
                        Else
                            ExecConn("INSERT INTO FacClosingSlips (SlipNo, CSlipNo , PolNo, EndNo, LoadNo, SlipDate, PolRef, SubIns, InsuranceType, InsuredName " _
                                & " ,Period ,OriginalSI ,OriginalNet ,Currency ,ExRate, RefNo,Recom ,AcceptedShare,FacSI,FacNet " _
                                 & " , CommisionRatio, Commision, Balance) VALUES " _
                    & " ('" & UCase(SlipNo.Value) & "'" _
                    & ",'" & Request("SlipNo") & "' " _
                    & ",'" & PolNo.Value & "'" _
                    & "," & EndNo.Value & "" _
                    & "," & LoadNo.Value & "" _
                    & ",'" & Format(SlipDate.Value, "yyyy-MM-dd") & "'" _
                    & ",'" & Request("Ref") & "'" _
                    & ",'" & SubIns.Text & "'" _
                    & ",'" & InsuranceType.Value & "'" _
                    & ",'" & InsuredName.Value & "'" _
                    & ",'" & Period.Value & "'" _
                    & "," & OriginalSI.Value & "" _
                    & "," & OriginalNet.Value & "" _
                    & "," & Cur.Text & "" _
                    & "," & ExcRate.Text & "" _
                    & ",'" & RefNo.Value & "'" _
                    & "," & ReCom.Value & "" _
                    & "," & AcceptedShare.Value & "" _
                    & "," & FacSI.Value & "" _
                    & "," & FacNet.Value & "" _
                    & "," & CommisionRatio.Value & "" _
                    & "," & Commision.Value & "" _
                    & "," & Balance.Value & ")", Conn)
                            Save.ClientEnabled = False
                            Save.ClientVisible = False
                            Save.Visible = False
                        End If

                    End If

                End If
            Case Else
        End Select

    End Sub

End Class