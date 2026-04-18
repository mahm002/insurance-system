Imports System.Data.SqlClient
Imports System.Web.UI
Imports DevExpress.Web

Partial Public Class settlement_Sadad
    Inherits Page

    Protected Sub Cbps_Callback(source As Object, e As CallbackEventArgs) Handles Cbps.Callback
        Select Case e.Parameter
            Case "Sadad"
                If DailyNo.IsValid Then
                    Dim dbadapter = New SqlDataAdapter("select distinct rtrim(MainClaimFile.ClmNo) As ClmNo, MainClaimFile.groupNo,MainClaimFile.endno,MainClaimFile.loadno, " _
                    & " RTRIM(MainClaimFile.ClmNo) + '&EndNo=' + LTRIM(MainClaimFile.EndNo) + '&LoadNo=' + LTRIM(MainClaimFile.LoadNo) + '&OrderNo=' + rtrim(PolicyFile.OrderNo) + '&Sys=' + PolicyFile.SubIns + '&Branch=' + PolicyFile.Branch + '&PolNo=' + PolicyFile.PolNo + '&GroupNo=' + LTRIM(mainclaimfile.GroupNo)  AS ClmPath,PolicyFile.SubIns As Sys " _
                    & " from MainClaimFile left outer join Estimation on MainClaimFile.ClmNo=Estimation.clmno and MainClaimFile.PolNo=Estimation.polno " _
                    & " left outer join NetPrm on NetPrm.PolNo = rtrim(MainClaimFile.PolNo) + '-' + rtrim(MainClaimFile.ClmNo) and round(Estimation.value,3)=round(netprm.Net,3) left outer join policyfile " _
                    & " on MainClaimFile.PolNo=PolicyFile.PolNo And MainClaimFile.EndNo=PolicyFile.EndNo And MainClaimFile.LoadNo=PolicyFile.LoadNo " _
                    & " Left outer join CustomerFile on CustomerFile.CustNo=policyfile.CustNo left outer join BranchInfo on mainclaimfile.Branch=BranchInfo.BranchNo " _
                    & " WHERE MainClaimFile.ClmNo='" & Request("ClmNo") & "'", Conn)

                    Dim ClmData As New DataSet
                    dbadapter.Fill(ClmData)

                    ExecConn("Update MainSattelement set DAILYNUM='" & DailyNo.Value & "', " _
                            & " DAILYDTE='" & DailyNo.SelectedItem.GetFieldValue("DAILYDTE") & "' " _
                            & " Where ClmNo='" & Request("ClmNo") & "' And No=" & Request("No") & " And DAILYDTE is NULL", Conn)

                    ASPxWebControl.RedirectOnCallback("~/Reins/DestributeClaim.aspx?ClmNo=" & ClmData.Tables(0).Rows(0)("ClmPath"))
                Else
                    Exit Sub
                End If
            Case Else
                Exit Select
        End Select
    End Sub

End Class