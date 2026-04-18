Imports System.Data.SqlClient
Imports DevExpress.Web

Partial Public Class Reinsurance_Quarters
    Inherits Page

    Private ReadOnly Treaty As New DataSet
    Private ReadOnly TT1 As New DataSet
    Private ReadOnly TT2 As New DataSet
    Private ReadOnly TT3 As New DataSet
    Private ReadOnly TT4 As New DataSet
    Private ReadOnly TT5 As New DataSet
    Private ReadOnly TT6 As New DataSet
    Private ReadOnly TT7 As New DataSet
    Private ReadOnly TT8 As New DataSet
    Private ReadOnly TT9 As New DataSet
    Private ReadOnly TT10 As New DataSet
    Private ReadOnly TT11 As New DataSet
    Private ReadOnly TT12 As New DataSet
    Private ReadOnly TT13 As New DataSet
    Private ReadOnly TT14 As New DataSet
    Private ReadOnly TT15 As New DataSet

    Private ReadOnly DB As New DataSet
    Private QSs, Fsurps As Double
    Private splt = ""
    Private Group, SubGroupQs, SubGroupFs As String

    Protected Sub ASPxiButton6_Click(sender As Object, e As EventArgs) Handles ASPxButton6.Click
        'ExecSql("delete netprm where pold= CONVERT(DATETIME, '1900-01-01 00:00:00', 102) and len(polno)=16")
        Dim sssql As String
        splt = ""
        Dim strarr() As String
        strarr = InsType.Value.Split(","c)
        'Dim splt = ""
        For Each s As String In strarr
            splt = splt + "'" + s + "'," 'MessageBox.Show(s)
        Next
        splt = Mid(splt, 1, Len(splt) - 1)

        For Each item As ListEditItem In Reinsurer.Items
            Dim value As Object = item.Value
            Dim name As Object = item.Text

            If InsType.SelectedItem.Text = "Engineering" Or InsType.SelectedItem.Text = "HULL" Or InsType.SelectedItem.Text = "Marine" Or InsType.SelectedItem.Text = "2NDSurplus" Or InsType.SelectedItem.Text = "LineSlip" Then
                Group = "UW"
            Else
                Group = "CC"
                'Reinsurer.ValueField = "Idx"
            End If
            Select Case InsType.SelectedItem.Text
                Case "Engineering"
                    SubGroupQs = "GetClaimsQsEng"
                    SubGroupFs = "GetClaimsFsEng"
                Case "Marine"
                    SubGroupQs = "GetClaimsQsMarine"
                    SubGroupFs = "GetClaimsFsMarine"
                Case "HULL"
                    SubGroupQs = "GetClaimsQsHull"
                    SubGroupFs = "GetClaimsFsHull"
                Case "Fire"
                    SubGroupQs = "GetClaimsQsFire"
                    SubGroupFs = "GetClaimsFsFire"
                Case "General Accident"
                    SubGroupQs = "GetClaimsQsGA"
                    SubGroupFs = "GetClaimsFsGA"
                Case Else
                    Exit Select
            End Select
            Select Case InsType.SelectedItem.Text
                Case "War"
                    sssql = "(SELECT UWYear AS UWYear, " _
                    & "'WQ/S' as Type, " _
    & "sum([Premium]) as [Premium]," _
    & "sum([Premium Reverse Released]) as [Premium Reverse Released]," _
    & "sum([Loss Portfolio Entry]) as [Loss Portfolio Entry]," _
    & "Sum([Premium Portfoilio Entry]) as [Premium Portfoilio Entry]," _
    & "sum([Interest Cn Reserves]) as [Interest Cn Reserves]," _
    & "sum([Loss Reverse Released]) as [Loss Reverse Released]," _
    & "Sum([Commission]) as [Commission]," _
    & "Sum([Losses Paid]) as [Losses Paid]," _
    & "Sum([Premium Reserve Retained]) as [Premium Reserve Retained]," _
    & "Sum([Loss Reverse Retained]) As [Loss Reverse Retained]," _
    & "Round((sum([Premium]) + sum([Premium Reverse Released]) + sum([Loss Portfolio Entry]) + Sum([Premium Portfoilio Entry]) + sum([Loss Reverse Released]) + sum([Interest Cn Reserves]))-(Sum([Commission]) + Sum([Losses Paid])+ Sum([Premium Reserve Retained]) + Sum([Loss Reverse Retained])),3) As [BALANCE]," _
    & "sum([Share]) As [Share]," _
    & "Round(((sum([Premium]) + sum([Premium Reverse Released]) + sum([Loss Portfolio Entry]) + Sum([Premium Portfoilio Entry]) + sum([Loss Reverse Released]) + sum([Interest Cn Reserves]))-(Sum([Commission]) + Sum([Losses Paid])+ Sum([Premium Reserve Retained]) + Sum([Loss Reverse Retained])))*(Sum([Share])/100),3) As [AMOUNT]" _
    & "FROM" _
    & "	(Select UnderWY as UWYear," _
    & "'WQ/S' as Type," _
    & "round(sum(Qsw * Exc), 3) As [Premium]," _
    & "0 as [Premium Reverse Released]," _
    & "0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	0 as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	round(sum(Qsw*Exc)*([dbo].[GetWQsComm](BB,UnderWY,'MC'))/100,3) as [Commission]," _
    & "	0 as [Losses Paid]," _
    & "	iif(sum(QSw*Exc)>0,round(sum(QSw*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "',UnderWY),3),0) as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	0 As [Share]" _
    & "	From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Pol' group by UnderWY" _
    & " UNION all" _
    & "	Select UnderWY as UWYear," _
    & " 'WQ/S' as Type," _
    & "	0 as [Premium]," _
    & "	0 as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	0 as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	0 as [Commission]," _
    & "	round(sum(Qsw*Exc),3) as [Losses Paid]," _
    & "	0 as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	0 As [Share]" _
    & "	From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Clm' and total>0 group by UnderWY" _
    & " UNION all" _
    & "	Select UnderWY as UWYear," _
    & " 'WQ/S' as Type," _
    & "	0 as [Premium]," _
    & "	0 as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	0 as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	0 as [Commission]," _
    & "	0 as [Losses Paid]," _
    & "	0 as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	0 As [Share]" _
    & "	From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Clm' and total=-1 group by UnderWY" _
    & " UNION all" _
    & "	Select UnderWY as UWYear," _
    & " 'WQ/S' as Type," _
    & "	0 as [Premium]," _
    & "	0 as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	0 as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	0 as [Commission]," _
    & "	0 as [Losses Paid]," _
    & "	0 as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	[dbo].[GetShare] (BB,UnderWY)  As [Share]" _
    & "	From NetPrm where tp in (" & splt & ") group by UnderWY" _
    & " UNION all" _
    & "	Select UnderWY as UWYear," _
    & " 'WQ/S' as Type," _
    & "	0 as [Premium]," _
    & "	iif(sum(QSw*Exc)>0,round(sum(QSw*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "',UnderWY),3),0) as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	iif(sum(QSw*Exc)>0,round(sum(QSw*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "',UnderWY) *0.01,3),0) as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	0 as [Commission]," _
    & "	0 as [Losses Paid]," _
    & "	0 as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	0 As [Share]" _
    & "	From NetPrm where  tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) - 1 & " and Type='Pol' group by UnderWY)  as X group by UWYear)" _
    & " Union ALL" _
    & "(SELECT UWYear AS UWYear," _
    & " 'W1STSurplus' as Type," _
    & "sum([Premium]) as [Premium]," _
    & "sum([Premium Reverse Released]) as [Premium Reverse Released]," _
    & "sum([Loss Portfolio Entry]) as [Loss Portfolio Entry]," _
    & "Sum([Premium Portfoilio Entry]) as [Premium Portfoilio Entry]," _
    & "sum([Interest Cn Reserves]) as [Interest Cn Reserves]," _
    & "sum([Loss Reverse Released]) as [Loss Reverse Released]," _
    & "Sum([Commission]) as [Commission]," _
    & "Sum([Losses Paid]) as [Losses Paid]," _
    & "Sum([Premium Reserve Retained]) as [Premium Reserve Retained]," _
    & "Sum([Loss Reverse Retained]) As [Loss Reverse Retained] ," _
    & "Round((sum([Premium]) + sum([Premium Reverse Released]) + sum([Loss Portfolio Entry]) + Sum([Premium Portfoilio Entry]) + sum([Loss Reverse Released]) + sum([Interest Cn Reserves]))-(Sum([Commission]) + Sum([Losses Paid])+ Sum([Premium Reserve Retained]) + Sum([Loss Reverse Retained])),3) As [BALANCE]," _
    & "sum([Share]) As [Share]," _
    & "Round(((sum([Premium]) + sum([Premium Reverse Released]) + sum([Loss Portfolio Entry]) + Sum([Premium Portfoilio Entry]) + sum([Loss Reverse Released]) + sum([Interest Cn Reserves]))-(Sum([Commission]) + Sum([Losses Paid])+ Sum([Premium Reserve Retained]) + Sum([Loss Reverse Retained])))*(Sum([Share])/100),3) As [AMOUNT]" _
    & "FROM" _
    & "	(Select UnderWY as UWYear," _
    & "  'W1STSurplus' as Type," _
    & "	round(sum(FirsSupw*Exc),3) as [Premium]," _
    & "	0 as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	0 as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	round(sum(FirsSupw*Exc)*([dbo].[GetW1SComm](12,UnderWY," & splt & "))/100,3) as [Commission]," _
    & "	0 as [Losses Paid], " _
    & " iif(sum(FirsSupw*Exc)>0,round(sum(FirsSupw*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "',UnderWY),3),0) as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	0 As [Share]" _
    & "	From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Pol' group by UnderWY" _
    & " UNION all" _
    & "	Select UnderWY as UWYear," _
    & " 'W1STSurplus' as Type," _
    & "	0 as [Premium]," _
    & "	0 as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	0 as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	0 as [Commission]," _
    & "	round(sum(FirsSupw*Exc),3) as [Losses Paid]," _
    & "	0 as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	0 As [Share]" _
    & "	From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Clm' and total>0 group by UnderWY" _
    & " UNION all" _
    & "	Select UnderWY as UWYear," _
    & " 'W1STSurplus' as Type," _
    & "	0 as [Premium]," _
    & "	0 as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	0 as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	0 as [Commission]," _
    & "	0 as [Losses Paid]," _
    & "	0 as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	0 As [Share]" _
    & "	From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Clm' and total=-1 group by UnderWY" _
    & " UNION all" _
    & "	Select UnderWY as UWYear," _
    & " 'W1STSurplus' as Type," _
    & "	0 as [Premium]," _
    & "	0 as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	0 as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	0 as [Commission]," _
    & "	0 as [Losses Paid]," _
    & "	0 as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & " [dbo].[GetShare](12, Substring(PolNo, 3, 4)) As [Share]" _
    & "	From NetPrm where tp In (" & splt & ") group by UnderWY" _
    & " UNION all" _
    & "	Select UnderWY As UWYear," _
    & " 'W1STSurplus' as Type," _
    & "	0 as [Premium]," _
    & "	iif(sum(FirsSupw*Exc)>0,round(sum(FirsSupw*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "',UnderWY),3),0) as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	iif(sum(FirsSupw*Exc)>0,round(sum(FirsSupw*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "',UnderWY)*0.01,3),0) as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	0 as [Commission]," _
    & "	0 as [Losses Paid]," _
    & "	0 as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	0 As [Share]" _
    & "	From NetPrm where  tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) - 1 & " and Type='Pol' group by UnderWY) as Y  group by UWYear)" _
    & "order by UWYear,Type desc "
                Case "2NDSurplus"
                    sssql = "(SELECT UWYear AS UWYear," _
                    & "'2NDSurplus' as Type," _
    & "sum([Premium]) as [Premium]," _
    & "sum([Premium Reverse Released]) as [Premium Reverse Released]," _
    & "sum([Loss Portfolio Entry]) as [Loss Portfolio Entry]," _
    & "Sum([Premium Portfoilio Entry]) as [Premium Portfoilio Entry]," _
    & "sum([Interest Cn Reserves]) as [Interest Cn Reserves]," _
    & "sum([Loss Reverse Released]) as [Loss Reverse Released]," _
    & "Sum([Commission]) as [Commission]," _
    & "Sum([Losses Paid]) as [Losses Paid]," _
    & "Sum([Premium Reserve Retained]) as [Premium Reserve Retained]," _
    & "Sum([Loss Reverse Retained]) As [Loss Reverse Retained]," _
    & "Round((sum([Premium]) + sum([Premium Reverse Released]) + sum([Loss Portfolio Entry]) + Sum([Premium Portfoilio Entry]) + sum([Loss Reverse Released]) + sum([Interest Cn Reserves]))-(Sum([Commission]) + Sum([Losses Paid])+ Sum([Premium Reserve Retained]) + Sum([Loss Reverse Retained])),3) As [BALANCE]," _
    & "sum([Share]) As [Share]," _
    & "Round(((sum([Premium]) + sum([Premium Reverse Released]) + sum([Loss Portfolio Entry]) + Sum([Premium Portfoilio Entry]) + sum([Loss Reverse Released]) + sum([Interest Cn Reserves]))-(Sum([Commission]) + Sum([Losses Paid])+ Sum([Premium Reserve Retained]) + Sum([Loss Reverse Retained])))*(Sum([Share])/100),3) As [AMOUNT]" _
    & "FROM" _
    & "	(Select UnderWY as UWYear," _
    & " '2NDSurplus' as Type," _
    & "	round(sum(SecondSup * Exc), 3) As [Premium]," _
    & "	0 as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	0 as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	round(sum(SecondSup*Exc)*([dbo].[Get2ndsComm](7,UnderWY,'ER'))/100,3) as [Commission]," _
    & "	0 as [Losses Paid]," _
    & "	0 as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	0 As [Share]" _
    & "	From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Pol' group by UnderWY" _
    & " UNION all" _
    & "	Select UnderWY as UWYear," _
    & " '2NDSurplus' as Type," _
    & "	0 as [Premium]," _
    & "	0 as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	0 as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	0 as [Commission]," _
    & "	round(sum(SecondSup*Exc),3) as [Losses Paid]," _
    & "	0 as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	0 As [Share]" _
    & "	From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Clm' and total>0 group by UnderWY" _
    & "UNION all" _
    & "	Select UnderWY as UWYear," _
    & " '2NDSurplus' as Type," _
    & "	0 as [Premium]," _
    & "	0 as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	0 as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	0 as [Commission]," _
    & "	0 as [Losses Paid]," _
    & "	0 as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	0 As [Share]" _
    & "	From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Clm' and total=-1 group by UnderWY" _
    & "UNION all" _
    & "	Select UnderWY as UWYear," _
    & " '2NDSurplus' as Type," _
    & "	0 as [Premium]," _
    & "	0 as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	0 as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	0 as [Commission]," _
    & "	0 as [Losses Paid]," _
    & "	0 as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	100  As [Share]" _
    & "	From NetPrm where tp in (" & splt & ") and year(pold)=" & Val(Year.Text) & " group by UnderWY" _
    & "UNION all" _
    & "	Select UnderWY as UWYear," _
    & " '2NDSurplus' as Type," _
    & "	0 as [Premium]," _
    & "	0 as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	0 as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	0 as [Commission]," _
    & "	0 as [Losses Paid]," _
    & "	0 as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	0 As [Share]" _
    & "	From NetPrm where  tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) - 1 & " and Type='Pol' group by UnderWY)  as X group by UWYear) "
                Case "LineSlip"
                    sssql = "(SELECT UWYear AS UWYear," _
                    & "'LineSlip' as Type," _
    & "sum([Premium]) as [Premium]," _
    & "sum([Premium Reverse Released]) as [Premium Reverse Released]," _
    & "sum([Loss Portfolio Entry]) as [Loss Portfolio Entry]," _
    & "Sum([Premium Portfoilio Entry]) as [Premium Portfoilio Entry]," _
    & "sum([Interest Cn Reserves]) as [Interest Cn Reserves]," _
    & "sum([Loss Reverse Released]) as [Loss Reverse Released]," _
    & "Sum([Commission]) as [Commission]," _
    & "Sum([Losses Paid]) as [Losses Paid]," _
    & "Sum([Premium Reserve Retained]) as [Premium Reserve Retained]," _
    & "Sum([Loss Reverse Retained]) As [Loss Reverse Retained]," _
    & "Round((sum([Premium]) + sum([Premium Reverse Released]) + sum([Loss Portfolio Entry]) + Sum([Premium Portfoilio Entry]) + sum([Loss Reverse Released]) + sum([Interest Cn Reserves]))-(Sum([Commission]) + Sum([Losses Paid])+ Sum([Premium Reserve Retained]) + Sum([Loss Reverse Retained])),3) As [BALANCE]," _
    & "sum([Share]) As [Share]," _
    & "Round(((sum([Premium]) + sum([Premium Reverse Released]) + sum([Loss Portfolio Entry]) + Sum([Premium Portfoilio Entry]) + sum([Loss Reverse Released]) + sum([Interest Cn Reserves]))-(Sum([Commission]) + Sum([Losses Paid])+ Sum([Premium Reserve Retained]) + Sum([Loss Reverse Retained])))*(Sum([Share])/100),3) As [AMOUNT]" _
    & "                FROM " _
    & "	(Select UnderWY as UWYear," _
    & "                'LineSlip' as Type," _
    & "	round(sum(LineSlip * Exc), 3) As [Premium]," _
    & "	0 as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	0 as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	round(sum(LineSlip*Exc)*([dbo].[GetLsComm](7,UnderWY,'MC'))/100,3) as [Commission]," _
    & "	0 as [Losses Paid]," _
    & "	0 as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	0 As [Share]" _
    & "	From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Pol' group by UnderWY" _
    & "                UNION all" _
    & "	Select UnderWY as UWYear," _
    & " 'LineSlip' as Type," _
    & "	0 as [Premium]," _
    & "	0 as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	0 as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	0 as [Commission]," _
    & "	round(sum(LineSlip*Exc),3) as [Losses Paid]," _
    & "	0 as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	0 As [Share]" _
    & "	From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Clm' and total>0 group by UnderWY" _
    & "UNION all" _
    & "	Select UnderWY as UWYear," _
    & "'LineSlip' as Type," _
    & "	0 as [Premium]," _
    & "	0 as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	0 as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	0 as [Commission]," _
    & "	0 as [Losses Paid]," _
    & "	0 as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	0 As [Share]" _
    & "	From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Clm' and total=-1 group by UnderWY" _
    & "UNION all" _
    & "	Select UnderWY as UWYear," _
    & " 'LineSlip' as Type," _
    & "	0 as [Premium]," _
    & "	0 as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	0 as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	0 as [Commission]," _
    & "	0 as [Losses Paid]," _
    & "	0 as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	100  As [Share]" _
    & "	From NetPrm where tp in (" & splt & ") and year(pold)=" & Val(Year.Text) & " group by UnderWY" _
    & "UNION all" _
    & "	Select UnderWY as UWYear," _
    & " 'LineSlip' as Type," _
    & "	0 as [Premium]," _
    & "	0 as [Premium Reverse Released]," _
    & "	0 as [Loss Portfolio Entry]," _
    & "	0 as [Premium Portfoilio Entry]," _
    & "	0 as [Interest Cn Reserves]," _
    & "	0 as [Loss Reverse Released]," _
    & "	0 as [Commission]," _
    & "	0 as [Losses Paid]," _
    & "	0 as [Premium Reserve Retained]," _
    & "	0 as [Loss Reverse Retained]," _
    & "	0 As [Share]" _
    & "	From NetPrm where  tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) - 1 & " and Type='Pol'   group by UnderWY)  as X group by UWYear)"

                Case "General Accident"
                    sssql = "(SELECT " & IIf(Group = "UW", "UWYear AS UWYear,", "") & " " _
                                           & "'Q/S' as Type, " _
                                           & "sum([Premium]) as [Premium], " _
                                           & "sum([PremiumBBB]) as [PremiumBBB], " _
                                           & "sum([Premium Reverse Released]) as [Premium Reverse Released], " _
                                           & "sum([Loss Portfolio Entry]) as [Loss Portfolio Entry]," _
                                           & "Sum([Premium Portfoilio Entry]) as [Premium Portfoilio Entry], " _
                                           & "sum([Interest Cn Reserves]) as [Interest Cn Reserves], " _
                                           & "sum([Loss Reverse Released]) as [Loss Reverse Released], " _
                                           & "Sum([Commission]) as [Commission], " _
                                           & "Sum([CommissionBBB]) as [CommissionBBB], " _
                                           & "Sum([Losses Paid]) as [Losses Paid], " _
                                           & "Sum([Premium Reserve Retained]) as [Premium Reserve Retained], " _
                                           & "Sum([Loss Reverse Retained]) As [Loss Reverse Retained], " _
                                           & "Round((sum([Premium]) + sum([PremiumBBB]) + sum([Premium Reverse Released]) + sum([Loss Portfolio Entry]) + Sum([Premium Portfoilio Entry]) + sum([Loss Reverse Released]) + sum([Interest Cn Reserves]))-(Sum([Commission]) + Sum([CommissionBBB]) + Sum([Losses Paid])+ Sum([Premium Reserve Retained]) + " & IIf(Group = "UW", "Sum([Loss Reverse Retained])", "0") & "),3) As [BALANCE], " _
                                           & "sum([Share]) As [Share], " _
                                           & "Round(((sum([Premium]) + sum([PremiumBBB]) + sum([Premium Reverse Released]) + sum([Loss Portfolio Entry]) + Sum([Premium Portfoilio Entry]) + sum([Loss Reverse Released]) + sum([Interest Cn Reserves]))-(Sum([Commission]) + Sum([CommissionBBB]) + Sum([Losses Paid])+ Sum([Premium Reserve Retained]) + " & IIf(Group = "UW", "Sum([Loss Reverse Retained])", "0") & "))*(Sum([Share])/100),3) As [AMOUNT] " _
                                           & "FROM " _
                                               & "(Select " & IIf(Group = "UW", "UnderWY as UWYear, ", "") & "" _
                                               & "'Q/S' as Type, " _
                                               & "round(sum(QS * Exc), 3) As [Premium], " _
                                               & "0 As [PremiumBBB], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "0 as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "round(sum(QS*Exc)*([dbo].[GetQsComm](" & item.Value & "," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & ",'" & Left(splt, 4).Substring(1, 2) & "'))/100,3) as [Commission], " _
                                               & "0 as [CommissionBBB], " _
                                               & "0 as [Losses Paid], " _
                                               & "" & IIf(Group = "UW", "round(sum(QS*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "'," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "),3)", IIf(Quarters.SelectedItem.Value <> 4, "round(sum(QS*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "'," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "),3)", "0")) & " as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained], " _
                                               & "0 As [Share] " _
                                               & " From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Pol' " & IIf(Group = "UW", "group by UnderWY", "") & "" _
                                           & " " & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                           & "Select " & IIf(Group = "UW", "UnderWY As UWYear, ", "") & "" _
                                               & "'Q/S' as Type, " _
                                               & "0 As [Premium], " _
                                               & "round(sum(QS * Exc), 3) As [PremiumBBB], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "0 as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "round(sum(QS*Exc)*([dbo].[GetQsComm](" & item.Value & "," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & ",'BB'))/100,3) as [Commission], " _
                                               & "0 as [Losses Paid], " _
                                               & "" & IIf(Group = "UW", "round(sum(QS*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "'," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "),3)", IIf(Quarters.SelectedItem.Value <> 4, "round(sum(QS*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "'," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "),3)", "0")) & " as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained], " _
                                               & "0 As [Share] " _
                                               & " From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Pol' " & IIf(Group = "UW", "group by UnderWY", "") & "" _
                                               & " " & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & " Select " & IIf(Group = "UW", "UnderWY as UWYear,", "") & "" _
                                               & "'Q/S' as Type,0 as [Premium], " _
                                               & "0 As [PremiumBBB], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "0 as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "0 as [CommissionBBB], " _
                                               & "round(sum(QS*Exc),3) as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained], " _
                                               & "0 As [Share] " _
                                               & "From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Clm' and total>0 " & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                           & " " & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & " Select " & IIf(Group = "UW", "UnderWY as UWYear,", "") & "" _
                                               & "'Q/S' as Type,0 as [Premium], " _
                                               & "0 As [PremiumBBB], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "" & IIf(Quarters.Value = 4 And Group = "UW", "round([dbo].[" & SubGroupQs & "] (UnderWY," & Val(Year.Text) - 1 & ")*[dbo].[GetIntRes] (UnderWY),3)", "0") & " as [Interest Cn Reserves], " _
                                               & "" & IIf(Quarters.Value = 4 And Group = "UW", "round([dbo].[" & SubGroupQs & "] (UnderWY," & Val(Year.Text) - 1 & "),3)", "0") & " as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "0 as [CommissionBBB], " _
                                               & "0 as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained], " _
                                               & "0 As [Share] " _
                                               & "From NetPrm where Type='Clm' and total=-1 " & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                           & " " & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & " Select " & IIf(Group = "UW", "UnderWY as UWYear,", "top(1)") & "" _
                                               & "'Q/S' as Type,0 as [Premium], " _
                                               & "0 As [PremiumBBB], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "0 as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "0 as [CommissionBBB], " _
                                               & "0 as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "" & IIf(Quarters.Value = 4, "round([dbo].[" & SubGroupQs & "] (" & IIf(Group = "UW", "UnderWY,", "") & "" & Val(Year.Text) & "),3)", "0") & " as [Loss Reverse Retained], " _
                                               & "0 As [Share] " _
                                               & "From NetPrm where Type='Clm' and total=-1 " & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                           & " " & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & " Select " & IIf(Group = "UW", "UnderWY as UWYear,", "") & "" _
                                               & "'Q/S' as Type,0 as [Premium], " _
                                               & "0 As [PremiumBBB], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "0 as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "0 as [CommissionBBB], " _
                                               & "0 as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained], " _
                                               & "[dbo].[GetShare] (" & item.Value & "," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & ")  As [Share] " _
                                               & "From NetPrm where tp in (" & splt & ") and (year(pold)=" & Val(Year.Text) & " OR year(pold)=" & Val(Year.Text) - 1 & " OR year(pold)=UnderWY) " & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                           & " " & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & " Select " & IIf(Group = "UW", "UnderWY as UWYear,", "") & " " _
                                               & "'Q/S' as Type,0 as [Premium], " _
                                               & "0 As [PremiumBBB], " _
                                               & "" & IIf(Group = "UW", "round(sum(QS*Exc)*[dbo].[GetReserveRatio](" & splt.replace("'", "") & "," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "),3)", IIf(Quarters.SelectedItem.Value <> 4, "0", GetCleanCutReleaseQS(Val(Year.Text), splt, "Qs"))) & " as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "" & IIf(Group = "UW", "round(sum(QS*Exc)*[dbo].[GetReserveRatio](" & splt.replace("'", "") & ",UnderWY) *[dbo].[GetIntRes] (UnderWY),3)", IIf(Quarters.SelectedItem.Value <> 4, "0", GetCleanCutReleaseQSRatio(Val(Year.Text), splt, "Qs"))) & " as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "0 as [CommissionBBB], " _
                                               & "0 as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained], " _
                                               & "0 As [Share] " _
                                               & "From NetPrm where " & IIf(Group = "UW", " tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) - 1 & " and Type='Pol' ", "tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Pol' ") & " " _
                                           & " " & IIf(Group = "UW", "group by UnderWY", "") & ") " _
                                           & " as X " & IIf(Group = "UW", "group by UWYear", "") & ")" _
                                       & " Union ALL " _
                                           & "(SELECT " & IIf(Group = "UW", "UWYear AS UWYear,", "") & "" _
                                           & "'1STSurplus' as Type," _
                                           & "sum([Premium]) as [Premium]," _
                                           & "sum([PremiumBBB]) as [PremiumBBB]," _
                                           & "sum([Premium Reverse Released]) as [Premium Reverse Released]," _
                                           & "sum([Loss Portfolio Entry]) as [Loss Portfolio Entry], " _
                                           & "Sum([Premium Portfoilio Entry]) as [Premium Portfoilio Entry]," _
                                           & "sum([Interest Cn Reserves]) as [Interest Cn Reserves], " _
                                           & "sum([Loss Reverse Released]) as [Loss Reverse Released], " _
                                           & "Sum([Commission]) as [Commission], " _
                                           & "Sum([CommissionBBB]) as [CommissionBBB], " _
                                           & "Sum([Losses Paid]) as [Losses Paid], " _
                                           & "Sum([Premium Reserve Retained]) as [Premium Reserve Retained], " _
                                           & "Sum([Loss Reverse Retained]) As [Loss Reverse Retained] , " _
                                           & "Round((sum([Premium]) + sum([PremiumBBB]) + sum([Premium Reverse Released]) + sum([Loss Portfolio Entry]) + Sum([Premium Portfoilio Entry]) + sum([Loss Reverse Released]) + sum([Interest Cn Reserves]))-(Sum([Commission]) + Sum([CommissionBBB]) + Sum([Losses Paid])+ Sum([Premium Reserve Retained]) + " & IIf(Group = "UW", "Sum([Loss Reverse Retained])", "0") & "),3) As [BALANCE], " _
                                           & "sum([Share]) As [Share], " _
                                           & "Round(((sum([Premium]) + sum([PremiumBBB]) + sum([Premium Reverse Released]) + sum([Loss Portfolio Entry]) + Sum([Premium Portfoilio Entry]) + sum([Loss Reverse Released]) + sum([Interest Cn Reserves]))-(Sum([Commission]) + Sum([CommissionBBB]) + Sum([Losses Paid])+ Sum([Premium Reserve Retained]) + " & IIf(Group = "UW", "Sum([Loss Reverse Retained])", "0") & "))*(Sum([Share])/100),3) As [AMOUNT] " _
                                           & "FROM " _
                                               & "(Select " & IIf(Group = "UW", "UnderWY as UWYear,", "") & " " _
                                               & "'1STSurplus' as Type," _
                                               & "round(sum(FirsSup*Exc),3) as [Premium], " _
                                               & "0 as [PremiumBBB], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "0 as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "round(sum(FirsSup*Exc)*([dbo].[Get1SComm](" & item.Value & "," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "," & Left(splt, 4) & " ))/100,3) as [Commission], " _
                                               & "0 as [CommissionBBB], " _
                                               & "0 as [Losses Paid], " _
                                               & "" & IIf(Group = "UW", "round(sum(FirsSup*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "'," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "),3)", IIf(Quarters.SelectedItem.Value <> 4, "round(sum(FirsSup*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "'," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "),3)", "0")) & " as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained], " _
                                               & "0 As [Share] " _
                                               & "From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Pol' " & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                         & "" & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                         & "Select " & IIf(Group = "UW", "UnderWY As UWYear,", "") & " " _
                                               & "'1STSurplus' as Type," _
                                               & "0 as [Premium], " _
                                               & "round(sum(FirsSup*Exc),3) as [PremiumBBB], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "0 as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "round(sum(FirsSup*Exc)*([dbo].[Get1SComm](" & item.Value & "," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & ",'BB'))/100,3) as [CommissionBBB], " _
                                               & "0 as [Losses Paid], " _
                                               & "" & IIf(Group = "UW", "round(sum(FirsSup*Exc)*[dbo].[GetReserveRatio](" & splt.replace("'", "") & "," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "),3)", IIf(Quarters.SelectedItem.Value <> 4, "round(sum(FirsSup*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "'," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "),3)", "0")) & " as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained], " _
                                               & "0 As [Share] " _
                                               & "From NetPrm where tp in ('BB') and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Pol' " & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                               & "" & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & "Select " & IIf(Group = "UW", "UnderWY as UWYear,", "") & " " _
                                               & "'1STSurplus' as Type, " _
                                               & "0 as [Premium], " _
                                               & "0 as [PremiumBBB], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "0 as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "0 as [CommissionBBB], " _
                                               & "round(sum(FirsSup*Exc),3) as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained]," _
                                               & "0 As [Share] " _
                                               & "From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & "and year(pold)=" & Val(Year.Text) & " and Type='Clm' and total>0 " & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                           & " " & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & " Select " & IIf(Group = "UW", "UnderWY as UWYear,", "") & "" _
                                               & "'1STSurplus' as Type,0 as [Premium], " _
                                               & "0 as [PremiumBBB], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "" & IIf(Quarters.Value = 4 And Group = "UW", "round([dbo].[" & SubGroupFs & "] (UnderWY," & Val(Year.Text) - 1 & ")*[dbo].[GetIntRes] (UnderWY),3)", "0") & " as [Interest Cn Reserves], " _
                                               & "" & IIf(Quarters.Value = 4 And Group = "UW", "round([dbo].[" & SubGroupFs & "] (UnderWY," & Val(Year.Text) - 1 & "),3)", "0") & " as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "0 as [CommissionBBB], " _
                                               & "0 as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained], " _
                                               & "0 As [Share] " _
                                               & "From NetPrm where Type='Clm' and total=-1 " & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                           & " " & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & " Select " & IIf(Group = "UW", "UnderWY as UWYear,", "top(1)") & "" _
                                               & "'1STSurplus' as Type,0 as [Premium], " _
                                               & "0 as [PremiumBBB], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "0 as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "0 as [CommissionBBB], " _
                                               & "0 as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "" & IIf(Quarters.Value = 4, "round([dbo].[" & SubGroupFs & "] (" & IIf(Group = "UW", "UnderWY,", "") & "" & Val(Year.Text) & "),3)", "0") & " as [Loss Reverse Retained], " _
                                               & "0 As [Share] " _
                                               & "From NetPrm where Type='Clm' and total=-1 " & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                         & "" & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & "Select " & IIf(Group = "UW", "UnderWY as UWYear,", "") & " " _
                                               & "'1STSurplus' as Type, " _
                                               & "0 as [Premium], " _
                                               & "0 as [PremiumBBB], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "0 as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "0 as [CommissionBBB], " _
                                               & "0 as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained]," _
                                               & "[dbo].[GetShare](" & item.Value & "," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & ") As [Share] " _
                                               & "From NetPrm where tp in (" & splt & ") and (year(pold)=" & Val(Year.Text) & " OR year(pold)=" & Val(Year.Text) - 1 & " OR year(pold)=UnderWY)" & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                         & " " & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & " Select " & IIf(Group = "UW", "UnderWY as UWYear,", "") & " " _
                                               & "'1STSurplus' as Type, " _
                                               & "0 as [Premium], " _
                                               & "0 as [PremiumBBB], " _
                                               & "" & IIf(Group = "UW", "round(sum(FirsSup*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "'," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "),3)", IIf(Quarters.SelectedItem.Value <> 4, "0", GetCleanCutReleaseFS(Val(Year.Text), InsType.Value, "FirsSup"))) & " as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "" & IIf(Group = "UW", "round(sum(FirsSup*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "'," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & ")*[dbo].[GetIntRes] (UnderWY),3)", IIf(Quarters.SelectedItem.Value <> 4, "0", GetCleanCutReleaseFSRatio(Val(Year.Text), InsType.Value, "FirsSup"))) & " as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "0 as [CommissionBBB], " _
                                               & "0 as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained]," _
                                               & "0 As [Share] " _
                                               & " From NetPrm where " & IIf(Group = "UW", " tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) - 1 & " and Type='Pol' ", "tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Pol' ") & "  " & IIf(Group = "UW", " group by UnderWY", "") & ") as Y " _
                                           & " " & IIf(Group = "UW", "group by UWYear) order by UWYear,Type desc", ")") & " "
                Case Else
                    sssql = "(SELECT " & IIf(Group = "UW", "UWYear AS UWYear,", "") & " " _
                                           & "'Q/S' as Type, " _
                                           & "sum([Premium]) as [Premium], " _
                                           & "sum([Premium Reverse Released]) as [Premium Reverse Released], " _
                                           & "sum([Loss Portfolio Entry]) as [Loss Portfolio Entry]," _
                                           & "Sum([Premium Portfoilio Entry]) as [Premium Portfoilio Entry], " _
                                           & "sum([Interest Cn Reserves]) as [Interest Cn Reserves], " _
                                           & "sum([Loss Reverse Released]) as [Loss Reverse Released], " _
                                           & "Sum([Commission]) as [Commission], " _
                                           & "Sum([Losses Paid]) as [Losses Paid], " _
                                           & "Sum([Premium Reserve Retained]) as [Premium Reserve Retained], " _
                                           & "Sum([Loss Reverse Retained]) As [Loss Reverse Retained], " _
                                           & "Round((sum([Premium]) + sum([Premium Reverse Released]) + sum([Loss Portfolio Entry]) + Sum([Premium Portfoilio Entry]) + sum([Loss Reverse Released]) + sum([Interest Cn Reserves]))-(Sum([Commission]) + Sum([Losses Paid])+ Sum([Premium Reserve Retained]) + " & IIf(Group = "UW", "Sum([Loss Reverse Retained])", "0") & "),3) As [BALANCE], " _
                                           & "sum([Share]) As [Share], " _
                                           & "Round(((sum([Premium]) + sum([Premium Reverse Released]) + sum([Loss Portfolio Entry]) + Sum([Premium Portfoilio Entry]) + sum([Loss Reverse Released]) + sum([Interest Cn Reserves]))-(Sum([Commission]) + Sum([Losses Paid])+ Sum([Premium Reserve Retained]) + " & IIf(Group = "UW", "Sum([Loss Reverse Retained])", "0") & "))*(Sum([Share])/100),3) As [AMOUNT] " _
                                           & "FROM " _
                                               & "(Select " & IIf(Group = "UW", "UnderWY as UWYear, ", "") & "" _
                                               & "'Q/S' as Type, round(sum(QS * Exc), 3) As [Premium], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "0 as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "round(sum(QS*Exc)*([dbo].[GetQsComm](" & item.Value & "," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "," & Left(splt, 4) & "))/100,3) as [Commission], " _
                                               & "0 as [Losses Paid], " _
                                               & "iif(sum(QS*Exc)>0 or sum(QS*Exc) is null," & IIf(Group = "UW", "round(sum(QS*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "'," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "),3)", IIf(Quarters.SelectedItem.Value <> 4, "round(sum(QS*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "'," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "),3)", "0")) & ",0) [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained], " _
                                               & "0 As [Share] " _
                                               & " From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Pol' " & IIf(Group = "UW", "group by UnderWY", "") & "" _
                                           & " " & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & " Select " & IIf(Group = "UW", "UnderWY as UWYear,", "") & "" _
                                               & "'Q/S' as Type,0 as [Premium], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "0 as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "round(sum(QS*Exc),3) as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained], " _
                                               & "0 As [Share] " _
                                               & "From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Clm' and total>0 " & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                           & " " & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & " Select " & IIf(Group = "UW", "UnderWY as UWYear,", "") & "" _
                                               & "'Q/S' as Type,0 as [Premium], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "" & IIf(Quarters.Value = 4 And Group = "UW", "round([dbo].[" & SubGroupQs & "] (UnderWY," & Val(Year.Text) - 1 & ")*[dbo].[GetIntRes] (UnderWY),3)", "0") & " as [Interest Cn Reserves], " _
                                               & "" & IIf(Quarters.Value = 4 And Group = "UW", "round([dbo].[" & SubGroupQs & "] (UnderWY," & Val(Year.Text) - 1 & "),3)", "0") & " as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "0 as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained], " _
                                               & "0 As [Share] " _
                                               & "From NetPrm where Type='Clm' and total=-1 " & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                           & " " & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & " Select " & IIf(Group = "UW", "UnderWY as UWYear,", "top(1)") & "" _
                                               & "'Q/S' as Type,0 as [Premium], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "0 as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "0 as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "" & IIf(Quarters.Value = 4, "round([dbo].[" & SubGroupQs & "] (" & IIf(Group = "UW", "UnderWY,", "") & "" & Val(Year.Text) & "),3)", "0") & " as [Loss Reverse Retained], " _
                                               & "0 As [Share] " _
                                               & "From NetPrm where Type='Clm' and total=-1 " & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                           & " " & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & " Select " & IIf(Group = "UW", "UnderWY as UWYear,", "") & "" _
                                               & "'Q/S' as Type,0 as [Premium], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "0 as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "0 as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained], " _
                                               & "[dbo].[GetShare] (" & item.Value & "," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & ")  As [Share] " _
                                               & "From NetPrm where tp in (" & splt & ") and (year(pold)=" & Val(Year.Text) & " OR year(pold)=" & Val(Year.Text) - 1 & " OR year(pold)=UnderWY) " & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                           & " " & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & " Select " & IIf(Group = "UW", "UnderWY as UWYear,", "") & " " _
                                               & "'Q/S' as Type,0 as [Premium], " _
                                               & "iif(sum(QS*Exc)>0 or sum(QS*Exc) is null," & IIf(Group = "UW", "round(sum(QS*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "'," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "),3)", IIf(Quarters.SelectedItem.Value <> 4, "0", GetCleanCutReleaseQS(Val(Year.Text), splt, "Qs"))) & ",0) as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "iif(sum(QS*Exc)>0 or sum(QS*Exc) is null," & IIf(Group = "UW", "round(sum(QS*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "',UnderWY) *[dbo].[GetIntRes] (UnderWY),3)", IIf(Quarters.SelectedItem.Value <> 4, "0", GetCleanCutReleaseQSRatio(Val(Year.Text), splt, "Qs"))) & ",0) as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "0 as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained], " _
                                               & "0 As [Share] " _
                                               & "From NetPrm where " & IIf(Group = "UW", " tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) - 1 & " and Type='Pol' ", "tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Pol' ") & " " _
                                           & " " & IIf(Group = "UW", "group by UnderWY", "") & ") " _
                                           & " as X " & IIf(Group = "UW", "group by UWYear", "") & ")" _
                                       & " Union ALL " _
                                           & "(SELECT " & IIf(Group = "UW", "UWYear AS UWYear,", "") & "" _
                                           & "'1STSurplus' as Type," _
                                           & "sum([Premium]) as [Premium]," _
                                           & "sum([Premium Reverse Released]) as [Premium Reverse Released]," _
                                           & "sum([Loss Portfolio Entry]) as [Loss Portfolio Entry], " _
                                           & "Sum([Premium Portfoilio Entry]) as [Premium Portfoilio Entry]," _
                                           & "sum([Interest Cn Reserves]) as [Interest Cn Reserves], " _
                                           & "sum([Loss Reverse Released]) as [Loss Reverse Released], " _
                                           & "Sum([Commission]) as [Commission], " _
                                           & "Sum([Losses Paid]) as [Losses Paid], " _
                                           & "Sum([Premium Reserve Retained]) as [Premium Reserve Retained], " _
                                           & "Sum([Loss Reverse Retained]) As [Loss Reverse Retained] , " _
                                           & "Round((sum([Premium]) + sum([Premium Reverse Released]) + sum([Loss Portfolio Entry]) + Sum([Premium Portfoilio Entry]) + sum([Loss Reverse Released]) + sum([Interest Cn Reserves]))-(Sum([Commission]) + Sum([Losses Paid])+ Sum([Premium Reserve Retained]) + " & IIf(Group = "UW", "Sum([Loss Reverse Retained])", "0") & "),3) As [BALANCE], " _
                                           & "sum([Share]) As [Share], " _
                                           & "Round(((sum([Premium]) + sum([Premium Reverse Released]) + sum([Loss Portfolio Entry]) + Sum([Premium Portfoilio Entry]) + sum([Loss Reverse Released]) + sum([Interest Cn Reserves]))-(Sum([Commission]) + Sum([Losses Paid])+ Sum([Premium Reserve Retained]) + " & IIf(Group = "UW", "Sum([Loss Reverse Retained])", "0") & "))*(Sum([Share])/100),3) As [AMOUNT] " _
                                           & "FROM " _
                                               & "(Select " & IIf(Group = "UW", "UnderWY as UWYear,", "") & " " _
                                               & "'1STSurplus' as Type," _
                                               & "round(sum(FirsSup*Exc),3) as [Premium], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "0 as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "round(sum(FirsSup*Exc)*([dbo].[Get1SComm](" & item.Value & "," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "," & Left(splt, 4) & "))/100,3) as [Commission], " _
                                               & "0 as [Losses Paid], " _
                                               & "iif(sum(FirsSup*Exc)>0 or sum(FirsSup*Exc) is null," & IIf(Group = "UW", "round(sum(FirsSup*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "'," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "),3)", IIf(Quarters.SelectedItem.Value <> 4, "round(sum(FirsSup*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "'," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "),3)", "0")) & ",0) as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained], " _
                                               & "0 As [Share] " _
                                               & "From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Pol' " & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                         & "" & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & "Select " & IIf(Group = "UW", "UnderWY as UWYear,", "") & " " _
                                               & "'1STSurplus' as Type, " _
                                               & "0 as [Premium], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "0 as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "round(sum(FirsSup*Exc),3) as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained]," _
                                               & "0 As [Share] " _
                                               & "From NetPrm where tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & "and year(pold)=" & Val(Year.Text) & " and Type='Clm' and total>0 " & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                           & " " & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & " Select " & IIf(Group = "UW", "UnderWY as UWYear,", "") & "" _
                                               & "'1STSurplus' as Type,0 as [Premium], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "" & IIf(Quarters.Value = 4 And Group = "UW", "round([dbo].[" & SubGroupFs & "] (UnderWY," & Val(Year.Text) - 1 & ")*[dbo].[GetIntRes] (UnderWY),3)", "0") & " as [Interest Cn Reserves], " _
                                               & "" & IIf(Quarters.Value = 4 And Group = "UW", "round([dbo].[" & SubGroupFs & "] (UnderWY," & Val(Year.Text) - 1 & "),3)", "0") & " as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "0 as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained], " _
                                               & "0 As [Share] " _
                                               & "From NetPrm where Type='Clm' and total=-1 " & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                           & " " & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & " Select " & IIf(Group = "UW", "UnderWY as UWYear,", "top(1)") & "" _
                                               & "'1STSurplus' as Type,0 as [Premium], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "0 as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "0 as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "" & IIf(Quarters.Value = 4, "round([dbo].[" & SubGroupFs & "] (" & IIf(Group = "UW", "UnderWY,", "") & "" & Val(Year.Text) & "),3)", "0") & " as [Loss Reverse Retained], " _
                                               & "0 As [Share] " _
                                               & "From NetPrm where Type='Clm' and total=-1 " & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                         & "" & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & "Select " & IIf(Group = "UW", "UnderWY as UWYear,", "") & " " _
                                               & "'1STSurplus' as Type, " _
                                               & "0 as [Premium], " _
                                               & "0 as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "0 as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "0 as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained]," _
                                               & "[dbo].[GetShare](" & item.Value & "," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & ") As [Share] " _
                                               & "From NetPrm where tp in (" & splt & ") and (year(pold)=" & Val(Year.Text) & " OR year(pold)=" & Val(Year.Text) - 1 & " OR year(pold)=UnderWY)" & IIf(Group = "UW", "group by UnderWY", "") & " " _
                                         & " " & IIf(Group = "UW", "UNION all", "UNION ") & " " _
                                               & " Select " & IIf(Group = "UW", "UnderWY as UWYear,", "") & " " _
                                               & "'1STSurplus' as Type, " _
                                               & "0 as [Premium], " _
                                               & " iif(sum(FirsSup*Exc)>0 or sum(FirsSup*Exc) is null," & IIf(Group = "UW", "round(sum(FirsSup*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "'," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & "),3)", IIf(Quarters.SelectedItem.Value <> 4, "0", GetCleanCutReleaseFS(Val(Year.Text), splt, "FirsSup"))) & ",0) as [Premium Reverse Released], " _
                                               & "0 as [Loss Portfolio Entry], " _
                                               & "0 as [Premium Portfoilio Entry], " _
                                               & "iif(sum(FirsSup*Exc)>0 or sum(FirsSup*Exc) is null," & IIf(Group = "UW", "round(sum(FirsSup*Exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "'," & IIf(Group = "UW", "UnderWY", "" & Year.Text & "") & ")*[dbo].[GetIntRes] (UnderWY),3)", IIf(Quarters.SelectedItem.Value <> 4, "0", GetCleanCutReleaseFSRatio(Val(Year.Text), splt, "FirsSup"))) & ",0) as [Interest Cn Reserves], " _
                                               & "0 as [Loss Reverse Released], " _
                                               & "0 as [Commission], " _
                                               & "0 as [Losses Paid], " _
                                               & "0 as [Premium Reserve Retained], " _
                                               & "0 as [Loss Reverse Retained]," _
                                               & "0 As [Share] " _
                                               & " From NetPrm where " & IIf(Group = "UW", " tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) - 1 & " and Type='Pol' ", "tp in (" & splt & ") and datepart(qq,pold)=" & Quarters.Value & " and year(pold)=" & Val(Year.Text) & " and Type='Pol' ") & "  " & IIf(Group = "UW", " group by UnderWY", "") & ") as Y " _
                                           & " " & IIf(Group = "UW", "group by UWYear) order by UWYear,Type desc", ")") & " "

            End Select
            ASPxGridView1.Columns.Clear()
            DB.Clear()

            reinscom.Text = "To/ " & name
            Using Ocon As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                If Ocon.State = ConnectionState.Open Then
                    Ocon.Close()
                Else

                End If
                Dim dbadapter = New SqlDataAdapter(sssql, Ocon)
                dbadapter.SelectCommand.CommandTimeout = 200
                Dim unused = dbadapter.Fill(DB)
                ASPxGridView1.DataSource = DB
                ASPxGridView1.AutoGenerateColumns = True
                ASPxGridView1.DataBind()

                Dim TreatyNo As String
                'If Val(Year.Text) <= "2014" Then
                '    Select Case Mid(InsType.Value, 1, 2)
                '        Case "04", "05" : TreatyNo = "0401" + Year.Text
                '        Case "06" : TreatyNo = "0601" + Year.Text
                '        Case "22", "23" : TreatyNo = "2201" + Year.Text
                '        Case "21" : TreatyNo = "2101" + Year.Text
                '        Case "14", "15", "16" : TreatyNo = "1401" + Year.Text
                '        Case "08", "09", "10", "11", "12", "13", "18" : TreatyNo = "1408" + Year.Text

                '    End Select
                'Else

                'Select Case Mid(splt, 2, 2)
                '    Case "04", "05" : TreatyNo = "0401" + Year.Text
                '    Case "06" : TreatyNo = "0601" + Year.Text
                '    Case "22", "23", "21" : TreatyNo = "2201" + Year.Text
                '    Case "14", "15", "16" : TreatyNo = "1401" + Year.Text
                '    Case "12" : TreatyNo = "1201" + Year.Text
                '    Case "10" : TreatyNo = "1001" + Year.Text
                '    Case "13" : TreatyNo = "1301" + Year.Text
                '    Case "09", "18" : TreatyNo = "1801" + Year.Text
                '    Case "11" : TreatyNo = "1101" + Year.Text
                '    Case "08" : TreatyNo = "0801" + Year.Text
                '    Case Else : TreatyNo = Mid(splt, 2, 2) + "01" + Year.Text
                'End Select
                TreatyNo = Mid(splt, 2, 2) + "01" + Year.Text
                'End If
                TreatyN.Text = TreatyNo
                If DB.Tables(0).Rows.Count = 2 Then
                    CCTable.Visible = True
                    UWTable.Visible = False
                    Share.Text = DB.Tables(0).Rows(0)("Share")
                    ShareQs.Text = DB.Tables(0).Rows(0)("AMOUNT")
                    SharefSurp.Text = DB.Tables(0).Rows(1)("AMOUNT")
                    ShareTAmount.Text = Format(Val(DB.Tables(0).Rows(0)("AMOUNT")) + Val(DB.Tables(0).Rows(1)("AMOUNT")), "###,#0.000")
                Else
                    CCTable.Visible = False
                    UWTable.Visible = True
                    QSs = 0
                    Fsurps = 0
                    If InsType.SelectedItem.Text <> "2NDSurplus" And InsType.SelectedItem.Text <> "LineSlip" Then
                        For i As Integer = 0 To DB.Tables(0).Rows.Count - 1 Step 2
                            QSs += Val(DB.Tables(0).Rows(i)("AMOUNT"))

                        Next
                        For i As Integer = 1 To DB.Tables(0).Rows.Count Step 2
                            Fsurps += Val(DB.Tables(0).Rows(i)("AMOUNT"))
                        Next
                    Else
                        For i As Integer = 0 To DB.Tables(0).Rows.Count - 1 Step 1
                            QSs += Val(DB.Tables(0).Rows(i)("AMOUNT"))
                        Next
                    End If

                    ShareTAmount0.Text = Format(Fsurps + QSs, "###,#0.000")
                    If (Val(ShareTAmount0.Text) > 0) Then
                        Label1.Text = "Due To You"
                    Else
                        Label1.Text = "Due To Us"
                    End If
                End If

                ExecConn("Delete Soa where Q=" & Quarters.Value & " And Year=" & Val(Year.Text) & " and InsType='" & InsType.Text & "' AND Recom=" & item.Value & " ", Conn)
                For i As Integer = 0 To DB.Tables(0).Rows.Count - 1 Step 1
                    Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                        If con.State = ConnectionState.Open Then
                            con.Close()
                        Else

                        End If
                        con.Open()
                        If Group = "UW" Or InsType.SelectedItem.Text = "War" Then
                            ExecConn("INSERT INTO [dbo].[Soa] ([Q],[Recom],[Year],[InsType],[UWYear],[Type],[Premium],[PRRel] ,[LPE],[PPE],[ICR],[LRRel],[Commission],[LP],[PRRet],[LRRet],[Balance],[Share],[Amount])  VALUES(" _
                                        & Quarters.Value & ", " _
                                        & item.Value & "," _
                                        & Val(Year.Text) & ",'" _
                                        & InsType.Text & "'," _
                                        & DB.Tables(0).Rows(i)("UWYear") & ",'" _
                                        & Trim(DB.Tables(0).Rows(i)("Type")) & "'," _
                                        & Val(DB.Tables(0).Rows(i)("Premium")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Premium Reverse Released")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Loss Portfolio Entry")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Premium Portfoilio Entry")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Interest Cn Reserves")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Loss Reverse Released")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Commission")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Losses Paid")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Premium Reserve Retained")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Loss Reverse Retained")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("BALANCE")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Share")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("AMOUNT")) & ")", con)
                        Else
                            Select Case InsType.SelectedItem.Text
                                Case "General Accident"
                                    ExecConn("INSERT INTO [dbo].[Soa] ([Q],[Recom],[Year],[InsType],[UWYear],[Type],[Premium],[PremiumBBB],[PRRel] ,[LPE],[PPE],[ICR],[LRRel],[Commission],[CommissionBBB],[LP],[PRRet],[LRRet],[Balance],[Share],[Amount])  VALUES(" _
                                        & Quarters.Value & ", " _
                                        & item.Value & "," _
                                        & Val(Year.Text) & ",'" _
                                        & InsType.Text & "'," _
                                        & Val(Year.Text) & ",'" _
                                        & Trim(DB.Tables(0).Rows(i)("Type")) & "'," _
                                        & Val(DB.Tables(0).Rows(i)("Premium")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("PremiumBBB")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Premium Reverse Released")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Loss Portfolio Entry")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Premium Portfoilio Entry")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Interest Cn Reserves")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Loss Reverse Released")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Commission")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("CommissionBBB")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Losses Paid")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Premium Reserve Retained")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Loss Reverse Retained")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("BALANCE")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("Share")) & "," _
                                        & Val(DB.Tables(0).Rows(i)("AMOUNT")) & ")", con)
                                Case Else
                                    ExecConn("INSERT INTO [dbo].[Soa] ([Q],[Recom],[Year],[InsType],[UWYear],[Type],[Premium],[PRRel] ,[LPE],[PPE],[ICR],[LRRel],[Commission],[LP],[PRRet],[LRRet],[Balance],[Share],[Amount])  VALUES(" _
                                       & Quarters.Value & ", " _
                                       & item.Value & "," _
                                       & Val(Year.Text) & ",'" _
                                       & InsType.Text & "'," _
                                       & Val(Year.Text) & ",'" _
                                       & Trim(DB.Tables(0).Rows(i)("Type")) & "'," _
                                       & Val(DB.Tables(0).Rows(i)("Premium")) & "," _
                                       & Val(DB.Tables(0).Rows(i)("Premium Reverse Released")) & "," _
                                       & Val(DB.Tables(0).Rows(i)("Loss Portfolio Entry")) & "," _
                                       & Val(DB.Tables(0).Rows(i)("Premium Portfoilio Entry")) & "," _
                                       & Val(DB.Tables(0).Rows(i)("Interest Cn Reserves")) & "," _
                                       & Val(DB.Tables(0).Rows(i)("Loss Reverse Released")) & "," _
                                       & Val(DB.Tables(0).Rows(i)("Commission")) & "," _
                                       & Val(DB.Tables(0).Rows(i)("Losses Paid")) & "," _
                                       & Val(DB.Tables(0).Rows(i)("Premium Reserve Retained")) & "," _
                                       & Val(DB.Tables(0).Rows(i)("Loss Reverse Retained")) & "," _
                                       & Val(DB.Tables(0).Rows(i)("BALANCE")) & "," _
                                       & Val(DB.Tables(0).Rows(i)("Share")) & "," _
                                       & Val(DB.Tables(0).Rows(i)("AMOUNT")) & ")", con)
                            End Select

                        End If
                        con.Close()
                    End Using
                Next
            End Using
        Next item
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        ASPxGridView1.Visible = IsPostBack
        If (Not IsPostBack) AndAlso (Not IsCallback) Then
        End If
        If (Not IsPostBack) And (Year.Text <> "" Or Quarters.Value <> "" Or InsType.Value <> "") Then
            ASPxGridView1.DataBind()
        Else

        End If

        Dim strarr() As String
        strarr = InsType.Value.Split(","c)
        'Dim splt = ""
        For Each s As String In strarr
            splt = splt + "'" + s + "'," 'MessageBox.Show(s)
        Next
        splt = Mid(splt, 1, Len(splt) - 1)

        If Year.Text <> "" And Quarters.Value <> "" And InsType.Value <> "" Then

            Select Case InsType.SelectedItem.Text
                Case "Engineering", "Marine", "HULL"
                    SqlDataSource2.SelectCommand = "SELECT DISTINCT EXTRAINFO.TpNo, EXTRAINFO.TPName FROM TRSECFLE, EXTRAINFO WHERE TRSECFLE.War<>1 AND (RIGHT(RTRIM(TRSECFLE.TREATYNO), 4) in (select UnderWY from netprm where year(pold)<=" & Year.Text & " and DATEPART(qq,PolD)=" & Quarters.Value & " and tp in (" & splt & "))) and TRSECFLE.TRREINSCO = EXTRAINFO.TPNo AND EXTRAINFO.TP = 'ReCom'"
                    Reinsurer.DataSourceID = "SqlDataSource2"
                    SqlDataSource2.DataBind()
                    Reinsurer.DataBind()
                Case "War"
                    SqlDataSource2.SelectCommand = "SELECT DISTINCT EXTRAINFO.TpNo, EXTRAINFO.TPName FROM TRSECFLE, EXTRAINFO WHERE TRSECFLE.War<>1 AND (RIGHT(RTRIM(TRSECFLE.TREATYNO), 4) in (select UnderWY from netprm where year(pold)<=" & Year.Text & " and DATEPART(qq,PolD)=" & Quarters.Value & " and tp in (" & splt & "))) and TRSECFLE.TRREINSCO = EXTRAINFO.TPNo AND EXTRAINFO.TP = 'ReCom'"
                    Reinsurer.DataSourceID = "SqlDataSource2"
                    SqlDataSource2.DataBind()
                    Reinsurer.DataBind()
                Case "2NDSurplus"
                    SqlDataSource2.SelectCommand = "SELECT DISTINCT EXTRAINFO.TpNo, EXTRAINFO.TPName FROM TRSECFLE, EXTRAINFO WHERE TPNo= 7 AND (RIGHT(RTRIM(TRSECFLE.TREATYNO), 4) in (select UnderWY from netprm where year(pold)='" & Year.Text & "' and DATEPART(qq,PolD)=" & Quarters.Value & " and tp in (" & splt & "))) and TRSECFLE.TRREINSCO = EXTRAINFO.TPNo AND EXTRAINFO.TP = 'ReCom'"
                    Reinsurer.DataSourceID = "SqlDataSource2"
                    SqlDataSource2.DataBind()
                    Reinsurer.DataBind()
                Case "LineSlip"
                    SqlDataSource2.SelectCommand = "SELECT DISTINCT EXTRAINFO.TpNo, EXTRAINFO.TPName FROM TRSECFLE, EXTRAINFO WHERE TPNo= 7 AND (RIGHT(RTRIM(TRSECFLE.TREATYNO), 4) in (select UnderWY from netprm where year(pold)='" & Year.Text & "' and DATEPART(qq,PolD)=" & Quarters.Value & " and tp in (04))) and TRSECFLE.TRREINSCO = EXTRAINFO.TPNo AND EXTRAINFO.TP = 'ReCom'"
                    Reinsurer.DataSourceID = "SqlDataSource2"
                    SqlDataSource2.DataBind()
                    Reinsurer.DataBind()
                Case Else
                    SqlDataSource1.SelectCommand = "SELECT DISTINCT EXTRAINFO.TpNo, EXTRAINFO.TPName FROM TRSECFLE , EXTRAINFO WHERE TRSECFLE.War<>1 AND (RIGHT(RTRIM(TRSECFLE.TREATYNO), 4) = '" & Year.Text & "' ) and TRSECFLE.TRREINSCO = EXTRAINFO.TPNo AND EXTRAINFO.TP = 'ReCom'"
                    '"SELECT DISTINCT EXTRAINFO.TpNo, EXTRAINFO.TPName FROM TRSECFLE, EXTRAINFO WHERE TRSECFLE.War<>1 AND (RIGHT(RTRIM(TRSECFLE.TREATYNO), 4) in (select substring(polno,3,4) from netprm where substring(PolNo,3,4)='" & Year.Text & "' and DATEPART(qq,PolD)=" & Quarters.Value & " and tp in (" & splt & "))) and TRSECFLE.TRREINSCO = EXTRAINFO.TPNo AND EXTRAINFO.TP = 'ReCom'"
                    Reinsurer.DataSourceID = "SqlDataSource1"
                    SqlDataSource1.DataBind()
                    Reinsurer.DataBind()
            End Select

        End If
    End Sub

    Protected Sub ASPxGridView1_DataBinding(sender As Object, e As EventArgs) Handles ASPxGridView1.DataBinding
        'ASPxGridView1.DataSource = DB
    End Sub

    Protected Function GetCleanCutReleaseQS(Y As Integer, Sys As String, Category As String) As Double
        For i = 1 To 3
            Select Case i
                Case 1
                    Dim TQ1 = New SqlDataAdapter("select Sum(Qs*exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "','" & Y & "') AS T from NetPrm where datepart(qq,pold) <>4 AND datepart(qq,pold) =" & i & " AND year(Pold)=" & Y & " AND TP in (" & splt & ") AND Type='Pol'", Conn)
                    Dim unused2 = TQ1.Fill(TT1)
                Case 2
                    Dim TQ2 = New SqlDataAdapter("select Sum(Qs*exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "','" & Y & "') AS T from NetPrm where datepart(qq,pold) <>4 AND datepart(qq,pold) =" & i & " AND year(Pold)=" & Y & " AND TP in (" & splt & ") AND Type='Pol'", Conn)
                    Dim unused1 = TQ2.Fill(TT2)
                Case 3
                    Dim TQ3 = New SqlDataAdapter("select Sum(Qs*exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "','" & Y & "') AS T from NetPrm where datepart(qq,pold) <>4 AND datepart(qq,pold) =" & i & " AND year(Pold)=" & Y & " AND TP in (" & splt & ") AND Type='Pol'", Conn)
                    Dim unused = TQ3.Fill(TT3)
                Case Else
                    Exit Select
            End Select
        Next
        Dim Released As Double = Format(IIf(IsDBNull(TT1.Tables(0).Rows(0)(0)), 0, TT1.Tables(0).Rows(0)(0)) + IIf(IsDBNull(TT2.Tables(0).Rows(0)(0)), 0, TT2.Tables(0).Rows(0)(0)) + IIf(IsDBNull(TT3.Tables(0).Rows(0)(0)), 0, TT3.Tables(0).Rows(0)(0)), "###,#0.000")
        Return Released
    End Function

    Protected Function GetCleanCutReleaseFS(Y As Integer, Sys As String, Category As String) As Double
        For i = 1 To 3
            Select Case i
                Case 1
                    Dim TQ4 = New SqlDataAdapter("select Sum(FirsSup*exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "','" & Y & "') AS T from NetPrm where datepart(qq,pold) <>4 AND datepart(qq,pold) =" & i & " AND year(Pold)=" & Y & " AND TP in (" & splt & ") AND Type='Pol'", Conn)
                    Dim unused2 = TQ4.Fill(TT4)
                Case 2
                    Dim TQ5 = New SqlDataAdapter("select Sum(FirsSup*exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "','" & Y & "') AS T from NetPrm where datepart(qq,pold) <>4 AND datepart(qq,pold) =" & i & " AND year(Pold)=" & Y & " AND TP in (" & splt & ") AND Type='Pol'", Conn)
                    Dim unused1 = TQ5.Fill(TT5)
                Case 3
                    Dim TQ6 = New SqlDataAdapter("select Sum(FirsSup*exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "','" & Y & "') AS T from NetPrm where datepart(qq,pold) <>4 AND datepart(qq,pold) =" & i & " AND year(Pold)=" & Y & " AND TP in (" & splt & ") AND Type='Pol'", Conn)
                    Dim unused = TQ6.Fill(TT6)
                Case Else
                    Exit Select
            End Select
        Next
        Dim Releasedfs As Double = Format(IIf(IsDBNull(TT4.Tables(0).Rows(0)(0)), 0, TT4.Tables(0).Rows(0)(0)) + IIf(IsDBNull(TT5.Tables(0).Rows(0)(0)), 0, TT5.Tables(0).Rows(0)(0)) + IIf(IsDBNull(TT6.Tables(0).Rows(0)(0)), 0, TT6.Tables(0).Rows(0)(0)), "###,#0.000")
        Return Releasedfs
    End Function

    Protected Function GetCleanCutReleaseQSRatio(Y As Integer, Sys As String, Category As String) As Double
        For i = 1 To 3
            Select Case i
                Case 1
                    Dim TQ7 = New SqlDataAdapter("select Sum(Qs*exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "','" & Y & "') AS T from NetPrm where datepart(qq,pold) <>4 AND datepart(qq,pold) =" & i & " AND year(Pold)=" & Y & " AND TP in (" & splt & ") AND Type='Pol'", Conn)
                    Dim unused3 = TQ7.Fill(TT7)
                Case 2
                    Dim TQ8 = New SqlDataAdapter("select Sum(Qs*exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "','" & Y & "') AS T from NetPrm where datepart(qq,pold) <>4 AND datepart(qq,pold) =" & i & " AND year(Pold)=" & Y & " AND TP in (" & splt & ") AND Type='Pol'", Conn)
                    Dim unused2 = TQ8.Fill(TT8)
                Case 3
                    Dim TQ9 = New SqlDataAdapter("select Sum(Qs*exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "','" & Y & "') AS T from NetPrm where datepart(qq,pold) <>4 AND datepart(qq,pold) =" & i & " AND year(Pold)=" & Y & " AND TP in (" & splt & ") AND Type='Pol'", Conn)
                    Dim unused1 = TQ9.Fill(TT9)
                Case Else
                    Exit Select
            End Select
        Next
        Dim EntRQs = New SqlDataAdapter("select [dbo].[GetIntRes]('" & Year.Text & "') AS T ", Conn)
        Dim unused = EntRQs.Fill(TT15)

        Dim Releasedqsr As Double = Format(((IIf(IsDBNull(TT7.Tables(0).Rows(0)(0)), 0, TT7.Tables(0).Rows(0)(0)) * 0.75) + (IIf(IsDBNull(TT8.Tables(0).Rows(0)(0)), 0, TT8.Tables(0).Rows(0)(0)) * 0.5) + (IIf(IsDBNull(TT9.Tables(0).Rows(0)(0)), 0, TT9.Tables(0).Rows(0)(0)) * 0.25)) * TT15.Tables(0).Rows(0)(0), "###,#0.000")
        Return Releasedqsr
    End Function

    Protected Function GetCleanCutReleaseFSRatio(Y As Integer, Sys As String, Category As String) As Double
        For i = 1 To 3
            Select Case i
                Case 1
                    Dim TQ10 = New SqlDataAdapter("select Sum(FirsSup*exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "','" & Y & "') AS T from NetPrm where datepart(qq,pold) <>4 AND datepart(qq,pold) =" & i & " AND year(Pold)=" & Y & " AND TP in (" & splt & ") AND Type='Pol'", Conn)
                    Dim unused3 = TQ10.Fill(TT10)
                Case 2
                    Dim TQ11 = New SqlDataAdapter("select Sum(FirsSup*exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "','" & Y & "') AS T from NetPrm where datepart(qq,pold) <>4 AND datepart(qq,pold) =" & i & " AND year(Pold)=" & Y & " AND TP in (" & splt & ") AND Type='Pol'", Conn)
                    Dim unused2 = TQ11.Fill(TT11)
                Case 3
                    Dim TQ12 = New SqlDataAdapter("select Sum(FirsSup*exc)*[dbo].[GetReserveRatio]('" & splt.replace("'", "") & "','" & Y & "') AS T from NetPrm where datepart(qq,pold) <>4 AND datepart(qq,pold) =" & i & " AND year(Pold)=" & Y & " AND TP in (" & splt & ") AND Type='Pol'", Conn)
                    Dim unused1 = TQ12.Fill(TT12)
                Case Else
                    Exit Select
            End Select
        Next
        Dim EntRFs = New SqlDataAdapter("select [dbo].[GetIntRes]('" & Year.Text & "') AS T ", Conn)
        Dim unused = EntRFs.Fill(TT14)
        Dim Releasedfsr As Double = Format(((IIf(IsDBNull(TT10.Tables(0).Rows(0)(0)), 0, TT10.Tables(0).Rows(0)(0)) * 0.75) + (IIf(IsDBNull(TT11.Tables(0).Rows(0)(0)), 0, TT11.Tables(0).Rows(0)(0)) * 0.5) + (IIf(IsDBNull(TT12.Tables(0).Rows(0)(0)), 0, TT12.Tables(0).Rows(0)(0)) * 0.25)) * TT14.Tables(0).Rows(0)(0), "###,#0.000")
        Return Releasedfsr
    End Function

    Protected Function IsLeader(Comp As Integer) As Boolean
        Dim TQ13 = New SqlDataAdapter("select Top(1) IsLeader from TRSECFLE where TRREINSCO=" & Comp & " And right(TREATYNO,4) ='" & Year.Text & "'", Conn)
        Dim unused = TQ13.Fill(TT13)

        Dim Leader As Boolean = TT13.Tables(0).Rows(0)(0)
        Return Leader
    End Function

    Private Sub ASPxGridView1_HtmlRowCreated(sender As Object, e As ASPxGridViewTableRowEventArgs) Handles ASPxGridView1.HtmlRowCreated
        If e.RowType = GridViewRowType.Data Then
            If (e.GetValue("BALANCE") * e.GetValue("Share")) = 0 Then
                e.Row.Visible = False
            End If
        End If
    End Sub

    Protected Sub SaveCC(sender As Object, e As EventArgs)

        For i As Integer = 0 To DB.Tables(0).Rows.Count - 1 Step 1
            ExecConn("INSERT INTO [dbo].[Soa] ([Q],[Recom],[Year] ,[Type],[Premium] ,[PRRel] ,[LPE],[PPE],[ICR],[LRRel],[Commission],[LP],[PRRet],[LRRet],[Balance],[Share],[Amount])  VALUES(" _
                    & Quarters.Value & ", " & Val(ReInsuranceCo.Text) & "," & Val(Year.Text) & ",'" & Trim(DB.Tables(0).Rows(1)("Premium")) & "'," & Val(DB.Tables(0).Rows(1)("Premium Reverse Released")) & "," & Val(DB.Tables(0).Rows(i)("Loss Portfolio Entry")) & "," & Val(DB.Tables(0).Rows(i)("Premium Portfoilio Entry")) & "," & Val(DB.Tables(0).Rows(i)("Interest Cn Reserves")) & "," & Val(DB.Tables(0).Rows(i)("Loss Reverse Released")) & "," & Val(DB.Tables(0).Rows(i)("Commission")) & "," & Val(DB.Tables(0).Rows(i)("Losses Paid")) & "," & Val(DB.Tables(0).Rows(i)("Premium Reserve Retained")) & "," & Val(DB.Tables(0).Rows(i)("Loss Reverse Retained")) & "," & Val(DB.Tables(0).Rows(i)("BALANCE")) & "," & Val(DB.Tables(0).Rows(i)("Share")) & "," & Val(DB.Tables(0).Rows(i)("AMOUNT")) & "", Conn)
        Next
    End Sub

    Protected Sub SaveUW(sender As Object, e As EventArgs)

    End Sub

End Class