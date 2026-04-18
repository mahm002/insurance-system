Imports System.Data.SqlClient
Imports DevExpress.Web

Public Class FinanceBulkReceipt
    Inherits Page
    Private ReadOnly Policy As New DataSet
    Private RNo As String = "/"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Lo = Session("LogInfo")
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))
        'GridData.Columns.Clear()
        MoveDate.Value = IIf(IsDate(MoveDate.Value), MoveDate.Value, Today.Date)
        MoveDateTo.Value = IIf(IsDate(MoveDateTo.Value), MoveDateTo.Value, Today.Date)
        GridData.DataSourceID = "MoveDS"
        GridData.DataBind()

    End Sub

    Protected Sub PayTyp_ValueChanged(sender As Object, e As EventArgs)
        GridData.DataBind()
    End Sub

    Protected Sub ASPxButton2_Click(sender As Object, e As EventArgs) Handles ASPxButton2.Click
        Response.Write("<script type='text/javascript'> " + "window.close(); " + "</script>")
    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback
        Dim callbackPanel As ASPxCallbackPanel = TryCast(sender, ASPxCallbackPanel)
        Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)
        If Month(MoveDate.Value) = Month(MoveDateTo.Value) And Year(MoveDate.Value) = Year(MoveDateTo.Value) Then
        Else
            MsgBob(Me, " يجب أن يكون نطاق الفترة في نفس الشهر")
            Exit Sub
        End If

        Select Case e.Parameter
            Case "PaymentChanged"
                Select Case PayTyp.Value
                    Case 1
                        AccNo.ClientVisible = False
                        Bank.ClientVisible = False
                        AccName.ClientVisible = False
                    Case 2
                        AccNo.ClientVisible = True
                        Bank.ClientVisible = True
                        AccName.ClientVisible = False
                    Case 3
                        AccNo.ClientVisible = True
                        Bank.ClientVisible = True
                        AccName.ClientVisible = True

                        AccName.DataSourceID = "BankAccounts"
                        AccName.Caption = "الحساب المصرفي رقم"
                    Case 4
                        AccNo.ClientVisible = False
                        Bank.ClientVisible = False
                        AccName.ClientVisible = True

                        AccName.DataSourceID = "Accounts"
                        AccName.Caption = "حساب المدينون رقم"
                            'Accounts.DataBind()
                    Case 5
                        AccNo.ClientVisible = False
                        Bank.ClientVisible = False
                        AccName.ClientVisible = True

                        AccName.DataSourceID = "BankAccounts"
                        AccName.Caption = "الحساب المصرفي رقم"
                        'BankAccounts.DataBind()
                    Case 6
                        AccNo.ClientVisible = False
                        Bank.ClientVisible = False
                        AccName.ClientVisible = True

                        AccName.DataSourceID = "AccountsNotPayed"
                        AccName.Caption = "حساب تحت التحصيل رقم"
                    Case Else
                        Exit Select
                End Select
                AccName.DataBind()
                Dim temppay As String = ""

                Select Case PayTyp.Value
                    Case 1, 2, 3, 5
                        temppay = "وتحصيل"
                    Case 4
                        temppay = " ومديونية "
                    Case 6
                        temppay = " (تحت التحصيل) "
                End Select
                Dim temp1 As String = ""
                Dim temp2 As New List(Of String)
                Dim temp3 As String = ""
                Dim tempTot As Double = 0

                Dim selectItems As List(Of Object) = GridData.GetSelectedFieldValues("PolNo", "EndNo", "LoadNo", "TOTPRM", "AccountName")
                If selectItems.Count = 0 Then
                    Payed.Value = 0
                    Note.Value = "/"
                    sdad.Enabled = False
                Else
                    sdad.Enabled = True
                    For Each selectedItem As Object In selectItems
                        temp1 += "'" & Right(selectedItem(0).ToString.TrimEnd, 5) & "',"
                        temp2.Add(selectedItem(0).ToString.TrimEnd)
                        If temp3.Contains(selectedItem(4).ToString.TrimEnd) Then
                        Else
                            temp3 += "" & selectedItem(4).ToString.TrimEnd & "/"
                        End If
                        tempTot += selectedItem(3)
                        'Table.Rows.Remove(Table.Rows.Find(selectItemId))
                    Next

                    Dim unused As String = Left(temp1, Len(temp1) - 1)
                    Payed.Value = tempTot

                    Note.Value = $" إثبات إصدار {temppay} عدد {selectItems.Count} وثيقة {Sys.Text} / " + IIf(MoveDate.Value = MoveDateTo.Value, " صادرة بتاريخ  " + Format(MoveDate.Value, "yyyy/MM/dd").ToString, "صادرة في الفترة من " + Format(MoveDate.Value, "yyyy/MM/dd").ToString + " إلى " + Format(MoveDateTo.Value, "yyyy/MM/dd").ToString) + " / " + PayTyp.Text + " / " + temp3 + " / " + GetBranchName(Session("Branch")).ToString
                End If

            Case "SelectionChanged"
                Dim temppay As String = ""

                Select Case PayTyp.Value
                    Case 1, 2, 3, 5
                        temppay = "وتحصيل"
                    Case 4
                        temppay = " ومديونية "
                    Case 6
                        temppay = " (تحت التحصيل) "
                End Select
                Dim temp1 As String = ""
                Dim temp2 As New List(Of String)
                Dim temp3 As String = ""
                Dim tempTot As Double = 0

                Dim selectItems As List(Of Object) = GridData.GetSelectedFieldValues("PolNo", "EndNo", "LoadNo", "TOTPRM", "AccountName")

                If selectItems.Count = 0 Then
                    Payed.Value = 0
                    Note.Value = "/"
                    sdad.Enabled = False
                Else
                    sdad.Enabled = True
                    For Each selectedItem As Object In selectItems
                        temp1 += "'" & Right(selectedItem(0).ToString.TrimEnd, 5) & "',"
                        temp2.Add(selectedItem(0).ToString.TrimEnd)
                        If temp3.Contains(selectedItem(4).ToString.TrimEnd) Then
                        Else
                            temp3 += "" & selectedItem(4).ToString.TrimEnd & "/"
                        End If
                        tempTot += selectedItem(3)
                        'Table.Rows.Remove(Table.Rows.Find(selectItemId))
                    Next
                    temp1 = Left(temp1, Len(temp1) - 1)
                    Payed.Value = tempTot

                    Note.Value = $" إثبات إصدار {temppay} عدد {selectItems.Count} وثيقة {Sys.Text} / " + IIf(MoveDate.Value = MoveDateTo.Value, " صادرة بتاريخ  " + Format(MoveDate.Value, "yyyy/MM/dd").ToString, "صادرة في الفترة من " + Format(MoveDate.Value, "yyyy/MM/dd").ToString + " إلى " + Format(MoveDateTo.Value, "yyyy/MM/dd").ToString) + " / " + PayTyp.Text + " / " + temp3 + " / " + GetBranchName(Session("Branch")).ToString
                End If
                'For Each item As ListEditItem In SelList.Items
                '    Note.Value += "-" & item.Value
                'Next
            Case "Sadad"
                Dim temppay As String

                Select Case PayTyp.Value
                    Case 1, 2, 3, 5
                        temppay = "وتحصيل"
                    Case 4
                        temppay = " ومديونية "
                    Case 6
                        temppay = " (تحت التحصيل) "
                    Case Else
                        Exit Select
                End Select
                Dim temp1 As String = ""
                Dim temp2 As New List(Of String)
                Dim temp3 As String = ""
                Dim tempTot As Double = 0
                Dim TempOrders As String

                Dim selectItems As List(Of Object) = GridData.GetSelectedFieldValues("PolNo", "EndNo", "LoadNo", "TOTPRM", "OrderNo", "AccountName")

                If selectItems.Count = 0 Then
                    Payed.Value = 0
                    Note.Value = "/"
                    sdad.Enabled = False
                    Exit Select
                Else

                    sdad.Enabled = True
                    For Each selectedItem As Object In selectItems
                        temp1 += "'" & Right(selectedItem(0).ToString.TrimEnd, 5) & "'"
                        temp2.Add(selectedItem(4).ToString.TrimEnd)
                        If temp3.Contains(selectedItem(5).ToString.TrimEnd) Then
                        Else
                            temp3 += "" & selectedItem(5).ToString.TrimEnd & "/"
                        End If
                        tempTot += selectedItem(3)
#Disable Warning BC42104 ' Variable is used before it has been assigned a value
                        TempOrders += "'" & selectedItem(4).ToString.TrimEnd & "',"
#Enable Warning BC42104 ' Variable is used before it has been assigned a value
                        'Table.Rows.Remove(Table.Rows.Find(selectItemId))
                    Next
                    temp1 = Left(temp1, Len(temp1) - 1)
                    temp1 = temp1.Replace("'", "/")
                    temp1 = temp1.Replace("//", "/")
                    TempOrders = Left(TempOrders, Len(TempOrders) - 1)
                    TempOrders = TempOrders.Replace("'", "")
                    Payed.Value = tempTot

#Disable Warning BC42104 ' Variable is used before it has been assigned a value
                    Note.Value = $" إثبات إصدار {temppay} عدد {selectItems.Count.ToString} وثيقة {temp1}{Sys.Text} / " + IIf(MoveDate.Value = MoveDateTo.Value, " صادرة بتاريخ  " + Format(MoveDate.Value, "yyyy/MM/dd").ToString, "صادرة في الفترة من " + Format(MoveDate.Value, "yyyy/MM/dd").ToString + " إلى " + Format(MoveDateTo.Value, "yyyy/MM/dd").ToString) + " / " + PayTyp.Text + " / " + temp3 + " / " + GetBranchName(Session("Branch")).ToString
#Enable Warning BC42104 ' Variable is used before it has been assigned a value
                End If
                Dim Accnt As String
                Select Case PayTyp.Value
                    Case 1, 2, 3
                        Accnt = GetAccount(PayTyp.Value)
                    Case Else
                        Accnt = AccName.Value
                End Select

                Select Case PayTyp.Value
                    Case 1, 2, 3, 5
                        Parm = Array.CreateInstance(GetType(SqlParameter), 1)
                        SetPm("@BR", DbType.String, Session("Branch"), Parm, 0)
                        RNo = CallSP("RecetNo", Conn, Parm)

                        ExecConn("Insert into ACCMOVE(DocNo,DocDat,CustNAme,PayMent,Amount,ForW,EndNo,LoadNo,Tp,Branch,AccNo,Bank,Cur,Node,PayTp,UserName,Note) Values('" _
                                & RNo & "'," _
                                & "CONVERT(DATETIME, '" & Format(IIf(MoveDate.Value = MoveDateTo.Value, MoveDate.Value, MoveDateTo.Value), "yyyy-MM-dd") & " 00:00:00', 102),'" _
                                & Trim(Customer.Text) & "'," _
                                & CDbl(Payed.Text) & "," _
                                & CDbl(Payed.Text) & ",'" _
                                & Note.Value & "'," _
                                & 0 & "," _
                                & 0 & ",'" _
                                & Sys.Text & "','" _
                                & Session("Branch") & "','" _
                                & AccNo.Text & "','" _
                                & Bank.Text & "','" _
                                & "دينار ليبي" & "','" _
                                & "" & "'," _
                                & PayTyp.Value & ",'" _
                                & Session("User") & "','" _
                                & temp1 & "')", Conn)

                        ExecConn("Update PolicyFile Set Inbox=TOTPRM,Financed=1 where OrderNo IN (select * From string_split('" & TempOrders & "',',')) ", Conn)
                    Case 4
                        RNo = "/"
                        ExecConn("Update PolicyFile Set AccountNo='" & AccName.Value & "',PayType=2, Financed = 1 where OrderNo IN (select * From string_split('" & TempOrders & "',',')) ", Conn)
                    Case 6
                        RNo = "/"
                        ExecConn("Update PolicyFile Set AccountNo='" & AccName.Value & "',PayType=2, Financed = 1 where OrderNo IN (select * From string_split('" & TempOrders & "',',')) ", Conn)
                    Case Else

                End Select

                Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    Else

                    End If
                    con.Open()

                    Dim JournalCmd As New SqlCommand With {
                        .CommandText = "JournalEntryBulkMove",
                        .CommandType = CommandType.StoredProcedure
                    }

                    JournalCmd.Parameters.AddWithValue("@Sys", Odbc.OdbcType.NVarChar).Value = Sys.Value
                    JournalCmd.Parameters.AddWithValue("@Br", Odbc.OdbcType.Int).Value = Session("Branch")
                    JournalCmd.Parameters.AddWithValue("@IssueDate", Odbc.OdbcType.Int).Value = Format(MoveDate.Value, "yyyy/MM/dd").ToString
                    JournalCmd.Parameters.AddWithValue("@IssueDateTo", Odbc.OdbcType.Int).Value = Format(MoveDateTo.Value, "yyyy/MM/dd").ToString
                    JournalCmd.Parameters.AddWithValue("@Orders", Odbc.OdbcType.NVarChar).Value = TempOrders
                    JournalCmd.Parameters.AddWithValue("@AccountN", Odbc.OdbcType.NVarChar).Value = Accnt
                    JournalCmd.Connection = con

                    Dim myReader As SqlDataReader = JournalCmd.ExecuteReader()
                    If myReader.HasRows Then
                        'Call New DataTable().Load(myReader)
                        Dim dt As New DataTable()
                        dt.Load(myReader)

                        'Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                        'SetPm("@TP", DbType.String, "1", Parm, 0)
                        'SetPm("@Year", DbType.String, Right(Year(dt.Rows(0).Item(8)).ToString, 2), Parm, 1)

                        Parm = Array.CreateInstance(GetType(SqlParameter), 3)
                        SetPm("@TP", DbType.String, "1", Parm, 0)
                        SetPm("@Year", DbType.String, Right(Year(dt.Rows(0).Item(8)).ToString, 2), Parm, 1)
                        SetPm("@Br", DbType.String, Session("Branch"), Parm, 2)

                        Dim Dly As String = CallSP("LastDailyNo", con, Parm)

                        ExecConn("INSERT INTO MainJournal ([DAILYNUM],[DAILYDTE] ,[DailyTyp] ,[ANALSNUM] ,[Comment] " _
                                    & ",[Currency] ,[Exchange] ,[CurUser] ,[MoveRef], [RecNo],[DailyChk],[Branch],[SubBranch])  " _
                                    & " VALUES ('" & Dly & "','" & Format(IIf(MoveDate.Value = MoveDateTo.Value, MoveDate.Value, MoveDateTo.Value), "yyyy/MM/dd") & "', " _
                                    & 1 & ", 'A','" & Note.Value & "', " & dt.Rows(0).Item(12) & "," & dt.Rows(0).Item(13) & ",'" _
                                    & Session("User") & "','" & TempOrders & "','" & RNo & "'," & 1 & ",'" & Session("Branch") & "','" & Session("Branch") & "')", con)
                        '& "'" & User & "','" & dt.Rows(0).Item(0).ToString + "/" + dt.Rows(0).Item(1).ToString + "/" + dt.Rows(0).Item(2).ToString & "'," & 1 & ")", con)

                        For Each row As DataRow In dt.Rows
                            ExecConn("INSERT INTO [dbo].[Journal]([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser],[Branch],[SubBranch]) " _
                                    & " VALUES ('" & Dly & "'," & 1 & ", '" & row.Item("AccountNumber") & "', " & CDbl(row.Item("DR")) & "," & CDbl(row.Item("CR")) & ",'" & Session("User") & "','" & Session("Branch") & "','" & Session("Branch") & "')", con)
                        Next row
                        Dim DBtTp As New DataSet
                        Dim dlyno, dlynoCr As New DataSet
                        If PayTyp.Value = 4 Or PayTyp.Value = 6 Then
                            Dim dladabtercr = New SqlDataAdapter("select DAILYNUM,DailyTyp from MainJournal where MoveRef='" & TempOrders & "'", Conn)
                            dladabtercr.Fill(dlynoCr)
                            ASPxWebControl.RedirectOnCallback("~/Finance/DailySarf.aspx?daily=" + Dly + "&Sys=1")
                            Exit Sub
                        End If
                        Dim dladabter = New SqlDataAdapter("select DAILYNUM,DailyTyp from MainJournal where MoveRef='" & TempOrders & "' and RecNo='" & RNo & "'", Conn)
                        dladabter.Fill(dlyno)
                        Using con1 As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                            Dim dbadapter5 = New SqlDataAdapter(String.Format("select PayTp from AccMove where DocNo='{0}' ", RNo), con1)
                            dbadapter5.Fill(DBtTp)
                            If DBtTp.Tables(0).Rows.Count = 0 Then
                                Exit Sub
                            Else
                                Select Case DBtTp.Tables(0).Rows(0)("PayTp")
                                    Case 1
                                        ExecConn("Update MainJournal set Comment=Comment + '(" & PayTyp.Text.Trim & ")' where DAILYNUM='" & Dly & "' and DailyTyp=1 and ANALSNUM='A'", con1)
                                        ExecConn("Update Journal set AccountNo ='" & GetBoxAccount(Session("Branch")) & "' where DAILYNUM='" & Dly & "' and TP=1 and Dr>0 and Left(AccountNo,2)<>'5.'", con1)
                                    Case 2
                                        ExecConn("Update MainJournal set Comment=Comment + '(" & PayTyp.Text.Trim & ")' where DAILYNUM='" & Dly & "' and DailyTyp=1 and ANALSNUM='A' ", con1)
                                        ExecConn("Update Journal set AccountNo ='" & GetcheqAccount(Session("Branch")) & "'  where DAILYNUM='" & Dly & "' and TP=1 and Dr>0 and Left(AccountNo,2)<>'5.'", con1)
                                    Case 3
                                        ExecConn("Update MainJournal set Comment=Comment + '(" & PayTyp.Text.Trim & ")' where DAILYNUM='" & Dly & "' and DailyTyp=1 and ANALSNUM='A' ", con1)
                                        ExecConn("Update Journal set AccountNo ='" & AccName.Value & "' where DAILYNUM='" & Dly & "' and TP=1 and Dr>0 and Left(AccountNo,2)<>'5.'", con1)
                                    Case 5
                                        ExecConn("Update MainJournal set Comment=Comment + '(" & PayTyp.Text.Trim & ")' where DAILYNUM='" & Dly & "' and DailyTyp=1 and ANALSNUM='A'", con1)
                                        ExecConn("Update Journal set AccountNo ='" & AccName.Value & "' where DAILYNUM='" & Dly & "' and TP=1 and Dr>0 and Left(AccountNo,2)<>'5.'", con1)
                                    Case Else
                                        Exit Select
                                End Select
                            End If
                            ASPxWebControl.RedirectOnCallback("~/Finance/DailySarf.aspx?daily=" + Dly + "&Sys=1")
                        End Using
                        'Dim DBtTp As New DataSet
                        'Dim dbadapter5 = New SqlDataAdapter(String.Format("select PayTp from AccMove where DocNo='{0}' ", RNo), con)
                        'dbadapter5.Fill(DBtTp)
                        'If DBtTp.Tables(0).Rows.Count = 0 Then
                        '    ASPxWebControl.RedirectOnCallback("~/Finance/DailySarf.aspx?daily=" & Dly & "&Sys=1")
                        'Else
                        '    Select Case DBtTp.Tables(0).Rows(0)("PayTp")
                        '        Case 1
                        '            ExecConn("Update Journal set AccountNo ='" & GetBoxAccount(Session("Branch")) & "' where DAILYNUM='" & Dly & "' and TP=1 and Dr>0 ", con)
                        '        Case 2
                        '            ExecConn("Update Journal set AccountNo ='" & GetcheqAccount(Session("Branch")) & "' where DAILYNUM='" & Dly & "' and TP=1 and Dr>0 ", con)
                        '        Case 3, 5
                        '            ExecConn("Update Journal set AccountNo ='" & AccName.Value & "' where DAILYNUM='" & Dly & "' and TP=1 and Dr>0 ", con)
                        '        Case Else
                        '            Exit Select
                        '    End Select
                        'End If
                        'ASPxWebControl.RedirectOnCallback("~/Finance/DailySarf.aspx?daily=" & Dly & "&Sys=1")
                    Else
                        'MsgBox("No rows found.")
                    End If

                    con.Close()

                End Using
            Case Else
                Exit Select
        End Select

        'End If
ss:
    End Sub

    Private Sub MoveDate_ValueChanged(sender As Object, e As EventArgs) Handles MoveDate.ValueChanged
        GridData.DataBind()
    End Sub

End Class