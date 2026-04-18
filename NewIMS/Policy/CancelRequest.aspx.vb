Imports System.Data.SqlClient

Public Class CancelRequest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub ASPxButton1_Click(sender As Object, e As EventArgs)
        If ReasonOfCncl.IsValid Then
            Dim Sysadmins, RequestsHist As New DataSet
            Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                If con.State = ConnectionState.Open Then
                    con.Close()
                Else
                End If
                con.Open()
                If IsBranchOffice(Request("Br")) Or IsBranch(Request("Br")) Then
                    Dim dbadapter = New SqlDataAdapter("select AccountNo,AccountName,AccountLogIn from AccountFile " _
                                                     & " where (AccountPermSys Like '%' + '-'+rtrim('" & Request("Sys") & "')+'-4;' + '%' or AccountPermSys like '%' + '-'+rtrim('" & Request("Sys") & "')+'-5;' + '%' ) " _
                                                     & " And (AccountFile.Branch = '" & RTrim(Request("Br")) & "' OR AccountFile.Branch = [dbo].[MainBranchCode]('" & RTrim(Request("Br")) & "') OR AccountFile.Branch =dbo.MainCenter() OR AccountFile.AccountNo in (Select ManagerId From BranchInfo  where BranchNo='" & RTrim(Request("Br")) & "') and AccountFile.Stop=0)", con)
                    dbadapter.Fill(Sysadmins)
                Else
                    Dim dbadapter = New SqlDataAdapter("select AccountNo,AccountName,AccountLogIn from AccountFile " _
                                                        & " where (AccountPermSys Like '%' + '-'+rtrim('" & Request("Sys") & "')+'-4;' + '%' OR AccountPermSys like '%' + '-'+rtrim('" & Request("Sys") & "')+'-5;' + '%' ) " _
                                                        & " And (AccountFile.Branch = [dbo].[MainBranchCode]('" & RTrim(Request("Br")) & "')) and AccountFile.Stop=0", con)
                    dbadapter.Fill(Sysadmins)
                End If

                Dim dbadapter1 = New SqlDataAdapter("select * From Notifications Where Action='../SystemManage/SpecialProcedures.aspx?OrderNo=" & Request("OrderNo") & "&PolNo=" & Request("PolNo") & "&EndNo=" & Request("EndNo") & "&LoadNo=" & Request("LoadNo") & "&sys=" & Request("Sys") & "'", con)
                dbadapter1.Fill(RequestsHist)
                If RequestsHist.Tables(0).Rows.Count <> 0 Then
                    Exit Sub
                End If
                If Sysadmins.Tables(0).Rows.Count <> 0 Then
                    For i As Integer = 1 To Sysadmins.Tables(0).Rows.Count
                        ExecConn("Insert Into Notifications (Action,IsRead,Message,Type,UserId,GeneratedBy,GeneratedByID) values('../SystemManage/SpecialProcedures.aspx?OrderNo=" & Request("OrderNo") & "&PolNo=" & Request("PolNo") & "&EndNo=" & Request("EndNo") & "&LoadNo=" & Request("LoadNo") & "&sys=" & Request("Sys") & "'," _
                                    & 0 & ",'" _
                                    & "طلب إلغاء الوثيقة رقم " & Trim(Request("PolNo")) & " / " & ReasonOfCncl.Value.ToString.TrimEnd & "',1,'" & Sysadmins.Tables(0).Rows(i - 1)("AccountNo") & "','" & Session("User") & "'," & Session("UserID") & ")", Conn)
                    Next
                End If

                con.Close()
            End Using

        End If
    End Sub

End Class