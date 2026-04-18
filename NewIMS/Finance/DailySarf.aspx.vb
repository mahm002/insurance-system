Imports System.Array
Imports System.Data.SqlClient
Imports System.Drawing
Imports DevExpress.Web
Imports Microsoft.Reporting.WebForms

Public Class DailySarf
    Inherits Page
    Dim br As String = ""
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request("Daily") = "" Then
                If Request("Sys") <> 3 Then
                    AnalsNum.Focus()
                    Payment.Visible = False
                    DocNum.Visible = False
                    CustName.Visible = False
                    PayedFor.Visible = False
                    PayedValue.Visible = False
                Else
                    AnalsNum.Text = "$"
                    AnalsNum.Enabled = False
                    DailyDte.Focus()
                    Payment.Visible = True
                    DocNum.Visible = True
                    CustName.Visible = True
                    PayedFor.Visible = True
                    PayedValue.Visible = True
                End If
                DailyDte.Value = Today.Date
            Else
                If Request("Sys") <> 3 Then
                    AnalsNum.Focus()
                    Payment.Visible = False
                    DocNum.Visible = False
                    CustName.Visible = False
                    PayedFor.Visible = False
                    PayedValue.Visible = False
                Else
                    AnalsNum.Text = "$"
                    AnalsNum.Enabled = False
                    DailyDte.Focus()
                    Payment.Visible = True
                    DocNum.Visible = True
                    CustName.Visible = True
                    PayedFor.Visible = True
                    PayedValue.Visible = True
                End If

                DailyNum.Value = Request("Daily")
                FillFormData(Request("Daily"))
            End If
        End If
        If IsCallback Then
            'GridView1.DataBind()
        Else
            GridView1.FocusedRowIndex = -1
        End If
        If Currency.Value = 1 Then
            Exch.Visible = False
        Else
            Exch.Visible = True
        End If
        If Request("Sys") = 3 Then
            GridView1.Columns(8).Visible = True
        Else
            GridView1.Columns(8).Visible = False
        End If
        Session("Flag") = Exch.Value
        AccNtnum.DataSourceID = "Accds"
        GridView1.DataBind()
        If DailyNum.Text <> "" Then
            Dim DlyBr As New DataSet
            Dim dbadapter = New SqlDataAdapter("Select Branch From MainJournal Where DAILYNUM='" & DailyNum.Text & "' And DailyTyp=" & Request("Sys") & "", AccConn)
            dbadapter.Fill(DlyBr)
            br = DlyBr.Tables(0).Rows(0)("Branch").ToString.Trim
        Else
            br = ""
        End If
        If AnalsNum.Text = "AG" Then
            DailyDte.ClientEnabled = False
            AnalsNum.ClientEnabled = False
        End If
        'If IsCallback Then
        Call CheckBlnce()
        'End If

    End Sub

    Protected Sub FillFormData(Ref As String)
        If Ref <> "" And Not Page.IsPostBack Then
            SqlDataSource1.SelectParameters("Daily").DefaultValue = DailyNum.Text

            SqlDataSource1.SelectParameters("Typ").DefaultValue = Request("sys")
            Dim DailyRec As New DataSet
            Dim dbadapter = New SqlDataAdapter("select * from MainJournal where dailyNum='" & Ref & "' and DailyTyp=" & Request("sys"), AccConn)
            dbadapter.Fill(DailyRec)

            'DailyRec = RecSet("select * from Dailytab1 where dailyNum='" & DailyNum.Text & "' and branch=1 and DailyTyp=" & IIf(Request("sys") = "F1", 1, 3), AccConn)
            DailyDte.Value = DailyRec.Tables(0).Rows(0)("DailyDte")
            DailySRL.Text = DailyRec.Tables(0).Rows(0)("DailySrl")
            If Not DailyRec.Tables(0).Rows(0).IsNull("AnalsNum") Then AnalsNum.Text = DailyRec.Tables(0).Rows(0)("AnalsNum")
            If Not DailyRec.Tables(0).Rows(0).IsNull("Comment") Then Comment.Text = DailyRec.Tables(0).Rows(0)("Comment")
            If Not DailyRec.Tables(0).Rows(0).IsNull("PayedFor") Then PayedFor.Text = DailyRec.Tables(0).Rows(0)("PayedFor")
            If Not DailyRec.Tables(0).Rows(0).IsNull("PayedValue") Then PayedValue.Text = DailyRec.Tables(0).Rows(0)("PayedValue")
            If Not DailyRec.Tables(0).Rows(0).IsNull("PayedValue") Then PayedValue.Text = DailyRec.Tables(0).Rows(0)("PayedValue")
            If Not DailyRec.Tables(0).Rows(0).IsNull("Currency") Then Currency.SelectedIndex = DailyRec.Tables(0).Rows.Item(0).Item("Currency") - 1 'Tables(0).Rows(0)("Currency") + 1
            If Not DailyRec.Tables(0).Rows(0).IsNull("Exchange") Then Exchange.Value = DailyRec.Tables(0).Rows.Item(0).Item("Exchange")
            If Not DailyRec.Tables(0).Rows(0).IsNull("RecNo") Then RecNo.Value = DailyRec.Tables(0).Rows.Item(0).Item("RecNo")
            If AnalsNum.Value = "A" Or AnalsNum.Value = "P" Then AnalsNum.Enabled = False
            'Call CheckBlnce()
        End If
    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback

        Dim callbackPanel As ASPxCallbackPanel = DirectCast(sender, ASPxCallbackPanel)
        Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)

        If isValid Then
            Select Case e.Parameter
                Case "AccCheck"
                    If AccNtnum.SelectedIndex <> -1 Then
                        'AccntNam.Text = AccNtnum.TextField
                        If (Mid(Trim(AccNtnum.Value), 1, 7) = "1.1.1.2") And Request("Sys") = 3 Then
                            DocNum.ClientVisible = True : CustName.ClientVisible = True : CustName.Text = PayedFor.Text
                        Else
                            DocNum.ClientVisible = False : CustName.ClientVisible = False
                        End If
                        Debtor.Focus()
                    Else
                        Errlbl.Text = " خطأ في رقم الحساب "
                        Errlbl.ForeColor = Color.Red
                    End If
                Case "ExRate"
                    SetDxtxtValue(FindControlRecursive(form1, "Exchange"), GetExrate(GetComboValue(FindControlRecursive(form1, "Currency")), GetComboText(FindControlRecursive(form1, "Currency"))))
                Case "Save"
                    If AccNtnum.SelectedIndex <> -1 Then

                        Dim JorCount As New DataSet
                        Dim dbadapter = New SqlDataAdapter("select * from Journal where DAILYNUM='" & DailyNum.Text & "' and TP=" & Request("Sys") & "", AccConn)
                        dbadapter.Fill(JorCount)

                        If Not IsDate(DailyDte.Value) Or Not IsValidDate(DailyDte.Value) Then Errlbl.Text = "  التاريخ غير صحيح " : Errlbl.ForeColor = Color.Red : Exit Sub
                        'If Len(Trim(AccNtnum.Value)) < 12 Then Errlbl.Text = " خطأ في رقم الحساب " : Errlbl.ForeColor = Color.Red : Exit Sub
                        If Not IsAccNo(Trim(AccNtnum.Value)) Then Errlbl.Text = " خطأ في رقم الحساب " : Errlbl.ForeColor = Color.Red : Exit Sub
                        If Trim(Comment.Text) = "" Then Errlbl.Text = "  يجب إدخال الشرح   " : Errlbl.ForeColor = Color.Red : Exit Sub
                        If AnalsNum.Text = "" Then Errlbl.Text = "   يجب تعريف تحليل القيد    " : Errlbl.ForeColor = Color.Red : Exit Sub
                        If Val(Debtor.Text) + Val(Creditor.Text) = 0 Then Errlbl.Text = " يجب ادخال الدائن أو المدين من القيد  " : Exit Sub
                        If Debtor.Value <> "0" And Creditor.Value <> "0" Then Errlbl.Text = "  لايمكن إدخال مدين ودائن لنفس الحركة   " : Errlbl.ForeColor = Color.Red : Exit Sub
                        If Debtor.Value = "0" And JorCount.Tables(0).Rows.Count = 0 Then Errlbl.Text = "    يجب ادخال الجانب المدين من القيد أولاً   " : Errlbl.ForeColor = Color.Red : Exit Sub

                        Select Case JorCount.Tables(0).Rows.Count
                            Case 0
                            Case Else
                                ' تعليق العملية التي لا تقبل الا نوع واحد من الحسابات P OR S
                                'If AccSorP.Tables(0).Rows(0)(0).ToString <> Left(Trim(AccNtnum.Value), 1) Then Errlbl.Text = "يجب أن تكون الحسابات من نفس النوع  " : Errlbl.ForeColor = Color.Red : Exit Sub
                        End Select

                        Dim m As String = Exchange.Value
                        'InStr(1, , "/") + 1

                        If Not IsDaily() Then
                            If DailyNum.Text = "" Then
                                Parm = CreateInstance(GetType(SqlParameter), 3)
                                SetPm("@TP", DbType.String, Request("Sys"), Parm, 0)
                                SetPm("@Year", DbType.String, Right(Year(DailyDte.Value).ToString, 2), Parm, 1)
                                SetPm("@Br", DbType.String, Session("Branch"), Parm, 2)

                                SetDxtxtValue(DailyNum, CallSP("LastDailyNo", AccConn, Parm))
                            End If
                            Insertdailydata()
                            If DailyNum.Text <> "" Then
                                Dim DlyBrs As New DataSet
                                Dim dbadapters = New SqlDataAdapter("Select Branch From MainJournal Where DAILYNUM='" & DailyNum.Text & "' And DailyTyp=" & Request("Sys") & "", AccConn)
                                dbadapters.Fill(DlyBrs)
                                br = DlyBrs.Tables(0).Rows(0)("Branch").ToString.Trim
                            Else
                                br = ""
                            End If

                            Insertdailyline(Session("Flag"))
                            Errlbl.Text = "   تمت الاضافة بنجاح    " : Errlbl.ForeColor = Color.Green
                        Else
                            'UpdateDailyData()
                            'Label1.Text = ""
                            If Not Session("Mode") Then
                                'Label1.Text = GetLastGroup(DailyNum.Text) + 1
                                UpdateDailyData()
                                Insertdailyline(Session("Flag"))
                                Errlbl.Text = "   تمت الاضافة بنجاح    " : Errlbl.ForeColor = Color.Green
                            Else
                                UpDateDailyLine(Session("Flag"))
                                Errlbl.Text = "   تم التعديل بنجاح    " : Errlbl.ForeColor = Color.Green
                                'UpdateDailyData()
                            End If
                        End If
                        AccNtnum.SelectedIndex = -1

                        SqlDataSource1.SelectParameters("Daily").DefaultValue = DailyNum.Text
                        SqlDataSource1.SelectParameters("Typ").DefaultValue = Request("sys")
                        SqlDataSource1.DataBind()
                    End If
                Case "Issue"
                    If Not IsDate(DailyDte.Value) Or Not IsValidDate(DailyDte.Value) Then Errlbl.Text = "  التاريخ غير صحيح " : Errlbl.ForeColor = Color.Red : Exit Sub

                    Parm = CreateInstance(GetType(SqlParameter), 2)

                    SetPm("@anals", DbType.String, AnalsNum.Text, Parm, 0)
                    SetPm("@year", DbType.String, DailyDte.Text, Parm, 1)

                    'SetPm("@Anals", DbType.String, GenArray(AnalsNum.Value.ToString), Parm, 0)
                    'SetPm("@Dt", DbType.String, GenArray(DailyDte.Text), Parm, 1)

                    '
                    If DailySRL.Value = "0" Or DailySRL.Value = "" Then
                        SetDxtxtValue(DailySRL, CallSP("DailySRL", AccConn, Parm))
                    Else
                        'Dim Firstslash As String = DailySRL.Text.Split("/")
                        Dim Vals = DailySRL.Text.Split("/")
                        'Dim Secondslash As Integer = DailySRL.Text.IndexOf("/"c, DailySRL.Text.IndexOf("/"c) + 1)
                        If CInt(Vals(0)) = CInt(Month(DailyDte.Value)) Then
                            'If Month(DailyDte.Value) <> CInt(Left(DailySRL.Value, InStr(1, DailySRL.Value, "/") - 1)) Then
                            '    SetDxtxtValue(DailySRL, CallSP("DailySRL", AccConn, Parm))
                        Else
                            SetDxtxtValue(DailySRL, CallSP("DailySRL", AccConn, Parm))
                        End If
                    End If
                    UpdateDailyData()
                    'ExecSql("update dailytab1 set Dailyprv=1 where dailynum='" & DailyNum.Text & "' and Dailytyp=" & IIf(Request("sys") = "F5", 3, 1))
                    'MsgBob(Me, " تم حفظ القيد ")

                    'Errlbl.Text = " تم حفظ القيد " : Errlbl.ForeColor = Color.Green

                    Dim Report = ReportsPath & "GeneralDaily"

                    Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("Daily", DailyNum.Value.ToString, False),
                    New ReportParameter("Typ", Request("Sys").ToString, False)
                     }

                    Session.Add("Parms", P)

                    ASPxWebControl.RedirectOnCallback("~/Reporting/Previewer.aspx?Report=" & Report & "")

                    Session("Flag") = Exch.Value
                Case Else
                    Exit Select

                    'Response.Write("<script> window.open('../Account/ReportPreview.aspx?Report=/FinanceReport/GeneralDaily','_new'); </script>")
                    ' Response.Write("<script> window.open('../Account/Default.aspx?sys=" & Request("sys") & "&tp=" & IIf(Request("sys") = "F5", 3, 1) & "','_self'); </script>")
            End Select
        Else
            Select Case e.Parameter
                Case "ExRate"
                    SetDxtxtValue(FindControlRecursive(form1, "Exchange"), GetExrate(GetComboValue(FindControlRecursive(form1, "Currency")), GetComboText(FindControlRecursive(form1, "Currency"))))
                    If Currency.Value = 1 Then
                        Exch.Visible = False
                    Else
                        Exch.Visible = True
                    End If

                    Session("Flag") = Exch.Value
                Case "Ex"
                    If Currency.Value = 1 Then
                        Exch.Visible = False
                    Else
                        Exch.Visible = True
                    End If

                Case Else
                    Exit Select
            End Select

        End If
        If Session("Mode") Then
            btnShow.Text = "تعديل"
        Else
            btnShow.Text = "إضافة"
            Session.Add("Mode", False)
        End If

        GridView1.DataBind()
        Call GetLastGroup(DailyNum.Text)

    End Sub

    Protected Sub Insertdailydata()

        Try
            ExecConn("INSERT INTO MainJournal(dailyNum, DailyDte, DailySRL, AnalsNum, DailyTyp, PayedFor, PayedValue, Currency, Exchange, CurUser, Comment,Branch) values('" _
 & DailyNum.Text & "','" _
 & "" & Format(DailyDte.Value, "yyyy/MM/dd") & "','" _
 & 0 & "','" _
 & AnalsNum.Text & "'," _
 & Request("sys") & ",'" _
 & IIf(Request("sys") = 3, PayedFor.Text, "") & "'," _
 & IIf(Request("sys") = 3, Val(PayedValue.Text), 0) & "," _
 & Currency.Value & "," _
 & Val(Exchange.Text) & ",'" _
 & Session("User") & "','" _
 & Comment.Text & "','" & Session("Branch") & "')", AccConn)
            ExecConn("Update Journal Set AccountNo=rtrim(ltrim(AccountNo))", AccConn)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub UpdateDailyData()
        'If IsSameUser(Session("User")) Then Errlbl.Text = "  تحتاج إلى المراجعة من مستخدم آخر " : Errlbl.ForeColor = Color.Red
        If Not IsDate(DailyDte.Value) Or Not IsValidDate(DailyDte.Value) Then Errlbl.Text = "  التاريخ غير صحيح " : Errlbl.ForeColor = Color.Red : Exit Sub
        Dim summaryDBT As ASPxSummaryItem = GridView1.TotalSummary.First(Function(i) i.Tag = "DBT")
        Dim summaryCRD As ASPxSummaryItem = GridView1.TotalSummary.First(Function(i) i.Tag = "CRD")
        Dim Chck As Boolean = CDbl(GridView1.GetTotalSummaryValue(summaryDBT)) = CDbl(GridView1.GetTotalSummaryValue(summaryCRD))
        If AnalsNum.Value = "A" Or AnalsNum.Value = "P" Or AnalsNum.Value = "AG" Then
            ExecConn("UPDATE MainJournal Set " _
      & "DailyDte='" & Format(CDate(DailyDte.Value), "yyyy-MM-dd") & "'," _
      & "DAILYSRL='" & DailySRL.Value & "'," _
      & "ANALSNUM='" & AnalsNum.Value & "'," _
      & "Comment='" & Comment.Text & "'," _
      & "DailyPrv=" & 1 & ", " _
      & "DailyChk='" & Chck & "'," _
      & "PayedFor='" & IIf(Request("sys") = 3, PayedFor.Text, "") & "'," _
      & "PayedValue=" & IIf(Request("sys") = 3, CDbl(PayedValue.Value), 0) & "," _
      & "Currency=" & Currency.Value & "," _
      & "Exchange=" & Val(Exchange.Value) & "," _
      & "UpUser='" & Session("User") & "'" _
      & "WHERE DailyNum='" & DailyNum.Value & "' and Branch='" & Session("Branch") & "' and DailyTyp=" & Request("sys") & "", AccConn)
        Else
            ExecConn("UPDATE MainJournal Set " _
       & "DailyDte='" & Format(CDate(DailyDte.Value), "yyyy-MM-dd") & "'," _
       & "DAILYSRL='" & DailySRL.Value & "'," _
       & "ANALSNUM='" & AnalsNum.Value & "'," _
       & "Comment='" & Comment.Text & "'," _
       & "DailyPrv=" & 1 & ", " _
       & "DailyChk='" & Chck & "'," _
       & "PayedFor='" & IIf(Request("sys") = 3, PayedFor.Text, "") & "'," _
       & "PayedValue=" & IIf(Request("sys") = 3, CDbl(PayedValue.Value), 0) & "," _
       & "Currency=" & Currency.Value & "," _
       & "Exchange=" & Val(Exchange.Value) & "," _
       & "UpUser='" & Session("User") & "'" _
       & "WHERE DailyNum='" & DailyNum.Value & "' and Branch='" & Session("Branch") & "' and DailyTyp=" & Request("sys") & "", AccConn)

        End If
        'ExecConn("UPDATE MainJournal Set " _
        '& "DailyDte='" & Format(CDate(DailyDte.Value), "yyyy-MM-dd") & "'," _
        '& "DAILYSRL='" & DailySRL.Value & "'," _
        '& "ANALSNUM='" & AnalsNum.Value & "'," _
        '& "Comment='" & Comment.Text & "'," _
        '& "DailyPrv=" & 1 & ", " _
        '& "DailyChk='" & Chck & "'," _
        '& "PayedFor='" & IIf(Request("sys") = 3, PayedFor.Text, "") & "'," _
        '& "PayedValue=" & IIf(Request("sys") = 3, CDbl(PayedValue.Value), 0) & "," _
        '& "Currency=" & Currency.Value & "," _
        '& "Exchange=" & Val(Exchange.Value) & "," _
        '& "UpUser='" & Session("User") & "'" _
        '& "WHERE DailyNum='" & DailyNum.Value & "' and Branch='" & Session("Branch") & "' and DailyTyp=" & Request("sys") & "", AccConn)
        ExecConn("Update Journal Set AccountNo=rtrim(ltrim(AccountNo))", AccConn)

    End Sub

    Protected Sub Insertdailyline(Exc As String)
        If Len(Trim(Debtor.Text)) = 0 Then Debtor.Text = 0
        If Len(Trim(Creditor.Text)) = 0 Then Creditor.Text = 0
        'If Len(Trim(AccNtnum.Value)) < 12 Then MsgBox(Me, "خطأ في رقم الحساب") : Exit Sub

        Try
            ExecConn("INSERT INTO Journal(dailyNum,TP,AccountNo,Dr,Cr,DocNum,CustName,GroupNo,CurUser,Note, Branch) values('" _
        & DailyNum.Text & "'," _
        & Request("sys") & ",'" _
        & Trim(AccNtnum.Value) & "'," _
        & Format(CDbl(Debtor.Value) * IIf(Exc, Val(Exchange.Value), 1), IIf(IIf(Exc, Val(Exchange.Value), 1) <> 1, "0.000", "0.000")) & "," _
        & Format(Math.Abs(CDbl(Creditor.Value)) * -1 * IIf(Exc, Val(Exchange.Value), 1), IIf(IIf(Exc, Val(Exchange.Value), 1) <> 1, "0.000", "0.000")) & ",'" _
        & DocNum.Text & "','" _
        & IIf(Request("sys") = 3, CustName.Text, "") & "'," _
        & 0 & ", '" _
        & Session("User") & "','" & Note.Text & "','" & br & "')", AccConn)

            Dim cashier1 As New DataSet
            Dim dbadapter11 = New SqlDataAdapter("SELECT CashierAccount from BranchInfo where BranchNo='" & Session("Branch") & "'", AccConn)
            dbadapter11.Fill(cashier1)

            If Trim(AccNtnum.Value) = cashier1.Tables(0).Rows(0)(0).ToString And Len(RTrim(RecNo.Value)) <> 1 And (AnalsNum.Value = "A" Or AnalsNum.Value = "P") Then
                Dim Inbox As New DataSet
                Dim Inboxdptr = New SqlDataAdapter("SELECT isnull(Sum(Dr),0) As Cash from Journal Where DailyNum='" & DailyNum.Text & "' AND Branch='" & br & "'and AccountNo='" & cashier1.Tables(0).Rows.Item(0).Item("CashierAccount") & "' and TP=" & Request("sys") & "", AccConn)
                Inboxdptr.Fill(Inbox)

                'ExecConn("Update Accmove set Payment=" & Inbox.Tables(0).Rows.Item(0).Item("Cash") + CDbl(Debtor.Value) & ",Amount=" & Inbox.Tables(0).Rows.Item(0).Item("Cash") + CDbl(Debtor.Value) & ",PayTp=1, DocDat='" & Format(CDate(DailyDte.Value), "yyyy-MM-dd") & "', Note='" & Comment.Value & "' " _
                '         & ",UserName= '/ تم التعديل بواسطة ' + '" & Session("User") & "' where DocNo='" & RecNo.Value & "'", AccConn)
            End If
            If RecNo.Value <> "/" And CDbl(Debtor.Value) <> 0 Then
                If Trim(AccNtnum.Value) = cashier1.Tables(0).Rows(0)(0).ToString Then
                    Dim Inbox2 As New DataSet
                    Dim Inboxdptr2 = New SqlDataAdapter("SELECT isnull(Sum(Dr),0) As Cash from Journal Where DailyNum='" & DailyNum.Text & "' AND Branch='" & br & "'and AccountNo='" & cashier1.Tables(0).Rows.Item(0).Item("CashierAccount") & "' and TP=" & Request("sys") & "", AccConn)
                    Inboxdptr2.Fill(Inbox2)
                    'ExecConn("Update Accmove set Payment=" & Inbox2.Tables(0).Rows.Item(0).Item("Cash") & ",Amount=" & Inbox2.Tables(0).Rows.Item(0).Item("Cash") & ",PayTp=1, DocDat='" & Format(CDate(DailyDte.Value), "yyyy-MM-dd") & "', Note='" & Comment.Value & "' " _
                    '     & ",UserName= '/ تم التعديل بواسطة ' + '" & Session("User") & "' where DocNo='" & RecNo.Value & "'", AccConn)
                Else
                    Dim Inbox2 As New DataSet
                    Dim Inboxdptr2 = New SqlDataAdapter("SELECT isnull(Sum(Dr),0) As Cash from Journal Where DailyNum='" & DailyNum.Text & "' AND Branch='" & br & "'and left(AccountNo,5)='1.1.1' and TP=" & Request("sys") & "", AccConn)
                    Inboxdptr2.Fill(Inbox2)
                    'ExecConn("Update Accmove set Payment=" & Inbox2.Tables(0).Rows.Item(0).Item("Cash") & ",Amount=" & Inbox2.Tables(0).Rows.Item(0).Item("Cash") & ",PayTp=5, DocDat='" & Format(CDate(DailyDte.Value), "yyyy-MM-dd") & "', Note='" & Comment.Value & "' " _
                    '     & ",UserName= '/ تم التعديل بواسطة ' + '" & Session("User") & "' where DocNo='" & RecNo.Value & "'", AccConn)
                End If

            End If

                AccNtnum.SelectedIndex = -1
            Debtor.Text = 0
            Creditor.Text = 0
            GroupNo.Text = ""
            CustName.Text = ""
            DocNum.Text = ""
            idx.Text = ""
            Note.Text = "/"
            If Request("sys") = 3 Then DocNum.Text = ""
            AccNtnum.Focus()
        Catch ex As Exception

        End Try
        'GridView1.DataBind()
        ExecConn("Update Journal Set AccountNo=rtrim(ltrim(AccountNo))", AccConn)
    End Sub

    Protected Sub UpDateDailyLine(Exc As String)
        Dim DailyRec1 As New DataSet
        Dim dbadapter1 = New SqlDataAdapter("SELECT CashierAccount from BranchInfo  where BranchNo='" & Session("Branch") & "'", AccConn)
        dbadapter1.Fill(DailyRec1)

        If Len(Trim(Debtor.Text)) = 0 Then Debtor.Text = 0
        If Len(Trim(Creditor.Text)) = 0 Then Creditor.Text = 0
        Try
            ExecConn("UPDATE Journal SET " _
        & "AccountNo='" & Trim(AccNtnum.Value) & "'," _
        & "Dr=" & Format(CDbl(Debtor.Value) * IIf(Exc, Exchange.Value, 1), IIf(IIf(Exc, Exchange.Value, 1) <> 1, "0.000", "0.000")) & "," _
        & "Cr=" & Format(Math.Abs(CDbl(Creditor.Value)) * -1 * IIf(Exc, Exchange.Value, 1), IIf(IIf(Exc, Exchange.Value, 1) <> 1, "0.000", "0.000")) & "," _
        & "DocNum='" & DocNum.Text & "'," _
        & "CustName='" & CustName.Text & "'," _
        & "CurUser='" & Session("User") & "'," _
        & "Note='" & Note.Text.Trim & "'" _
        & "where DailyNum='" & DailyNum.Text & "' AND Branch='" & br & "' and idx=" & idx.Value & " and TP=" & Request("sys") _
        , AccConn)

            If RecNo.Value <> "/" And (AnalsNum.Value = "A" Or AnalsNum.Value = "P") Then
                If Trim(AccNtnum.Value) = DailyRec1.Tables(0).Rows.Item(0).Item("CashierAccount") Then
                    Dim Inbox As New DataSet
                    Dim Inboxdptr = New SqlDataAdapter("SELECT isnull(Sum(Dr),0) As Cash from Journal Where DailyNum='" & DailyNum.Text & "' AND Branch='" & br & "'and AccountNo='" & DailyRec1.Tables(0).Rows.Item(0).Item("CashierAccount") & "' and TP=" & Request("sys") & "", AccConn)
                    Inboxdptr.Fill(Inbox)

                    'ExecConn("Update Accmove set Payment=" & Inbox.Tables(0).Rows.Item(0).Item("Cash") & ",Amount=" & Inbox.Tables(0).Rows.Item(0).Item("Cash") & ",PayTP=1, DocDat='" & Format(CDate(DailyDte.Value), "yyyy-MM-dd") & "', Note='" & Comment.Value & "' " _
                    ' & ",UserName= '/ تم التعديل بواسطة ' + '" & Session("User") & "' where DocNo='" & RecNo.Value & "' ", AccConn)
                Else
                    Dim Inbox As New DataSet
                    Dim Inboxdptr = New SqlDataAdapter("SELECT isnull(Sum(Dr),0) As Cash from Journal Where DailyNum='" & DailyNum.Text & "' AND Branch='" & br & "'and left(AccountNo,5)='1.1.1' and TP=" & Request("sys") & "", AccConn)
                    Inboxdptr.Fill(Inbox)

                    'ExecConn("Update Accmove set Payment=" & Inbox.Tables(0).Rows.Item(0).Item("Cash") & ",Amount=" & Inbox.Tables(0).Rows.Item(0).Item("Cash") & ",PayTP=5, DocDat='" & Format(CDate(DailyDte.Value), "yyyy-MM-dd") & "', Note='" & Comment.Value & "' " _
                    '     & ",UserName= '/ تم التعديل بواسطة ' + '" & Session("User") & "' where DocNo='" & RecNo.Value & "'", AccConn)
                End If
            Else

            End If

            AccNtnum.SelectedIndex = -1
            'AccntNam.Text = ""
            Debtor.Text = 0
            Creditor.Text = 0
            GroupNo.Text = ""
            CustName.Text = ""
            DocNum.Text = ""
            idx.Text = ""
            Note.Text = "/"
            If Request("sys") = 3 Then DocNum.Text = ""
            AccNtnum.Focus()
        Catch ex As Exception

        End Try
        If Session("Mode") Then
            Session("Mode") = False
        Else

        End If

        ExecConn("Update Journal Set AccountNo=rtrim(ltrim(AccountNo))", AccConn)
        'GridView1.DataBind()

        'ExecConn("update dailytab1 set Dailyprv=0,Trans=0 where DailyNum='" & DailyNum.Text & "' and branch=" & Session("BranchAcc") & " and  DailyTyp=" & IIf(Request("sys") = "f5", 3, 1), AccConn)
    End Sub

    Protected Function IsSameUser(CurrentUser As String) As Boolean
        Dim Check1 As New DataSet

        Dim dbadapter = New SqlDataAdapter("select CurUser,UpUser,DailyPrv From MainJournal where DAILYNUM='" & DailyNum.Value & "' and DailyTyp=" & Request("Sys") & "", AccConn)
        Dim unused = dbadapter.Fill(Check1)
        Select Case Check1.Tables(0).Rows(0)("DailyPrv")
            Case True
                Return If(Check1.Tables(0).Rows(0)("UpUser") <> CurrentUser, True, DirectCast(Check1.Tables(0).Rows(0)("CurUser") = CurrentUser, Boolean))
            Case False
                Return Check1.Tables(0).Rows(0)("CurUser") = CurrentUser

            Case Else
                Exit Select
        End Select
        'Return IsSameUser
#Disable Warning BC42353 ' Function doesn't return a value on all code paths
    End Function

#Enable Warning BC42353 ' Function doesn't return a value on all code paths

    Protected Sub GetLastGroup(Daily As String)
        ExecConn("Update Journal " _
          & "Set Journal.GroupNo=D.GroupNoT " _
          & " From(select idx,ROW_NUMBER() over (partition by S.DailyNum,S.TP order by S.Dr DESC, S.Cr ASC) " _
          & " as GroupNoT " _
          & " From Journal S) D  " _
          & " Where Journal.idx=D.idx and Journal.TP=" & Request("sys") & "  " _
          & " And Journal.DailyNum='" & Daily & "'", AccConn)
        ExecConn("Update Journal Set AccountNo=rtrim(ltrim(AccountNo))", AccConn)
    End Sub

    Public Function IsDaily() As Boolean
        Dim LastEnd As New DataSet
        Dim dbadapter = New SqlDataAdapter("select DailyNum from MainJournal where dailynum='" & DailyNum.Text & "' and DailyTyp=" & Request("Sys") & "", AccConn)
        dbadapter.Fill(LastEnd)
        'LastEnd = RecSet("select DailyNum from dailytab1 where dailynum='" & DailyNum.Text & "'", AccConn)
        If LastEnd.Tables(0).Rows.Count <> 0 Then
            IsDaily = True
        Else
            IsDaily = False
        End If
    End Function

    Protected Sub CheckBlnce()

        Dim DailyRec1 As New DataSet
        Dim dbadapter1 = New SqlDataAdapter("SELECT RecNo from MainJournal where Branch='" & Session("Branch") & "' AND DAILYNUM='" & DailyNum.Value & "' and MainJournal.dailytyp=" & Request("sys") & "", AccConn)
        dbadapter1.Fill(DailyRec1)

        Dim DailyRec As New DataSet
        Dim dbadapter = New SqlDataAdapter("select Sum(Dr),Sum(Cr) from MainJournal,Journal where MainJournal.DailyNum=Journal.DailyNum " _
          & " and MainJournal.dailytyp=Journal.TP and MainJournal.dailytyp=" & Request("sys") & " and MainJournal.DailyNum='" & DailyNum.Value & "'", AccConn)
        dbadapter.Fill(DailyRec)

        If DailyRec.Tables(0).Rows(0)(0).ToString = "" And DailyRec.Tables(0).Rows(0)(1).ToString = "" And DailyRec1.Tables(0).Rows.Count = 1 Then
            'ExecConn("Update Accmove set Payment=0, DocDat='" & Format(CDate(DailyDte.Value), "yyyy-MM-dd") & "', Note='CANCELLED', CustName='CANCELLED'" _
            '             & ",UserName= '// تم الإلغاء بواسطة ' + '" & Session("User") & "' where DocNo='" & RecNo.Value & "'", AccConn)
        End If

        'DailyRec = RecSet("select Sum(Debtor),Sum(Creditor) from Dailytab1,DailyTab2 where Dailytab1.DailyNum=Dailytab2.DailyNum and Dailytab1.branch=Dailytab2.Branch" _
        '& " and dailytab1.dailytyp=dailytab2.dailytyp and dailytab1.dailytyp=" & IIf(Request("sys") = "F5", 3, 1) & " and DailyTab1.DailyNum='" & SqlDataSource1.SelectParameters("Daily").DefaultValue & "' and Dailytab2.Branch=" & SqlDataSource1.SelectParameters("Branch").DefaultValue, AccConn)
        If Not DailyRec.Tables(0).Rows(0).IsNull(0) Then
            Dsum.Text = Format(DailyRec.Tables(0).Rows(0)(0), "###,0.000")
            Csum.Text = Format(Math.Abs(CDbl(DailyRec.Tables(0).Rows(0)(1))), "###,0.000")
            'Dsum.Visible = True
            'Csum.Visible = True
        Else
            'Dsum.Visible = False
            'Csum.Visible = False
        End If
        'If Trim(Csum.Text) = Trim(Dsum.Text) And Trim(Dsum.Text) <> "" Then
        '    Button3.Visible = True
        'Else
        '    Button3.Visible = False
        'End If
        Dim summaryDBT As ASPxSummaryItem = GridView1.TotalSummary.First(Function(i) i.Tag = "DBT")
        Dim summaryCRD As ASPxSummaryItem = GridView1.TotalSummary.First(Function(i) i.Tag = "CRD")
        Dsum.Text = GridView1.GetTotalSummaryValue(summaryDBT)
        Csum.Text = GridView1.GetTotalSummaryValue(summaryCRD)

        If GridView1.GetTotalSummaryValue(summaryDBT) <> GridView1.GetTotalSummaryValue(summaryCRD) Or GridView1.VisibleRowCount = 0 Then
            DiffernceLbl.Visible = True
            DiffernceLbl.Font.Bold = True
            If GridView1.VisibleRowCount = 0 Then
                DiffernceLbl.Text = "لا يوجد مدخلات"
            Else
                DiffernceLbl.Text = Format(CDbl(DailyRec.Tables(0).Rows(0)(1)) + CDbl(DailyRec.Tables(0).Rows(0)(0)), "###,0.000") + IIf(CDbl(DailyRec.Tables(0).Rows(0)(1)) + CDbl(DailyRec.Tables(0).Rows(0)(0)) > 0, " + مدين  ", "دائن  ")
            End If

            DiffernceLbl.ForeColor = Color.Red

            ExecConn("Update MainJournal Set DailyChk='False',DailyPrv='False' WHERE DAILYNUM='" & DailyNum.Value & "' And DailyTyp=" & Request("Sys") & " ", AccConn)
            btnIssue.ClientVisible = False
        Else
            DiffernceLbl.Visible = True
            DiffernceLbl.Font.Bold = True
            DiffernceLbl.Text = "BALANCED"
            DiffernceLbl.ForeColor = Color.Green

            ExecConn("Update MainJournal Set DailyChk='True' WHERE DAILYNUM='" & DailyNum.Value & "' And DailyTyp=" & Request("Sys") & "", AccConn)
            btnIssue.ClientVisible = True
        End If

    End Sub

    Private Sub GridView1_DataBound(sender As Object, e As EventArgs) Handles GridView1.DataBound
        If IsCallback Then
            Call CheckBlnce()
        End If

        ' GetTotalSummaryValue()
    End Sub

    Protected Sub GridView1_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles GridView1.CustomCallback

        If IsCallback And e.Parameters <> ",,,,," And btnShow.ClientVisible Then
            Dim Vals = e.Parameters.Split(",")
            GridView1.JSProperties("cpGroupNo") = Vals(0)
            GridView1.JSProperties("cpAccnum") = Vals(2)
            GridView1.JSProperties("cpDebtor") = Vals(3) / Exchange.Value
            GridView1.JSProperties("cpCreditor") = Vals(4) / Exchange.Value
            GridView1.JSProperties("cpDocNum") = Vals(5)
            GridView1.JSProperties("cpCustName") = Vals(6)
            GridView1.JSProperties("cpidx") = Vals(7)
            GridView1.JSProperties("cpNote") = Vals(8)
            GridView1.JSProperties("cpTP") = Request("Sys")
            If (Mid(Trim(Vals(2)), 1, 7) = "1.1.1.2") And Request("Sys") = 3 Then
                'DocNum.Visible = True : CustName.Visible = True ': CustName.Text = PayedFor.Text
                DocNum.ClientVisible = True : CustName.ClientVisible = True : CustName.Text = PayedFor.Text
                GridView1.JSProperties("cpshow") = "1"
            Else
                GridView1.JSProperties("cpshow") = "0"
            End If

            Session.Add("Mode", True)
            'AccNtnum.Value = Vals(2)
            ''AccntNam.Text = GridView1.Rows(GridView1.SelectedIndex).Cells(3).Text
            'Debtor.Value = Vals(3)
            'Creditor.Value = Vals(4)
            ''Label1.Text = GridView1.Rows(GridView1.SelectedIndex).Cells(1).Text
            'DocNum.Text = Vals(5)
        Else
            GridView1.FocusedRowIndex = -1
        End If
    End Sub

    Protected Sub GridView1_DataBinding(sender As Object, e As EventArgs)
        Dim grid As ASPxGridView = TryCast(sender, ASPxGridView)

        If Not IsValidDate(DailyDte.Value) Then
            TryCast(grid.Columns("Commands"), GridViewColumn).Visible = False
            btnShow.ClientVisible = False
            btnShow.ClientEnabled = False
        Else
            TryCast(grid.Columns("Commands"), GridViewColumn).Visible = True
            btnShow.ClientVisible = True
            btnShow.ClientEnabled = True
        End If

    End Sub

    Protected Sub DailyDte_ValueChanged(sender As Object, e As EventArgs)
        If Request("Daily") = "" And IsValidDate(DailyDte.Value) Then
            btnShow.ClientVisible = True
            btnShow.ClientEnabled = True
        End If
    End Sub

    Private Sub GridView1_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs) Handles GridView1.CustomButtonCallback
        'Dim grid As ASPxGridView = TryCast(sender, ASPxGridView)
        Select Case e.ButtonID
            Case "Edit"
                GridView1.JSProperties("cpGroupNo") = GridView1.GetRowValues(e.VisibleIndex, "GroupNo").ToString().Trim
                GridView1.JSProperties("cpAccnum") = GridView1.GetRowValues(e.VisibleIndex, "RealAccountNo").ToString().Trim
                GridView1.JSProperties("cpDebtor") = CDbl(GridView1.GetRowValues(e.VisibleIndex, "Dr").ToString().Trim) / Exchange.Value
                GridView1.JSProperties("cpCreditor") = CDbl(GridView1.GetRowValues(e.VisibleIndex, "Cr").ToString().Trim) / Exchange.Value
                GridView1.JSProperties("cpDocNum") = GridView1.GetRowValues(e.VisibleIndex, "DocNum").ToString().Trim
                GridView1.JSProperties("cpCustName") = GridView1.GetRowValues(e.VisibleIndex, "CustName").ToString().Trim
                GridView1.JSProperties("cpNote") = GridView1.GetRowValues(e.VisibleIndex, "Note").ToString().Trim
                GridView1.JSProperties("cpidx") = GridView1.GetRowValues(e.VisibleIndex, "idx").ToString().Trim
                GridView1.JSProperties("cpTP") = Request("Sys")
                If (Mid(Trim(GridView1.JSProperties("cpAccnum")), 1, 7) = "1.1.1.2") And Request("Sys") = 3 Then
                    'DocNum.Visible = True : CustName.Visible = True ': CustName.Text = PayedFor.Text
                    DocNum.ClientVisible = True : CustName.ClientVisible = True : CustName.Text = PayedFor.Text
                    GridView1.JSProperties("cpshow") = "1"
                Else
                    GridView1.JSProperties("cpshow") = "0"
                End If
                Session.Add("Mode", True)
            Case Else
                Exit Select
        End Select
    End Sub

    Protected Sub SqlDataSource1_Deleted(sender As Object, e As SqlDataSourceStatusEventArgs)

    End Sub

    Protected Sub GridView1_RowUpdated(sender As Object, e As Data.ASPxDataUpdatedEventArgs)

    End Sub

    Protected Sub GridView1_RowInserted(sender As Object, e As Data.ASPxDataInsertedEventArgs)

    End Sub

    Protected Sub GridView1_RowDeleted1(sender As Object, e As Data.ASPxDataDeletedEventArgs)
        Dim loging = "تم حذف الحساب " & e.Values().Values(10) & " من اليومية رقم " & e.Values().Values(2) & " بقيمة " & e.Values().Values(5) & "///" & e.Values().Values(6) & ""

        ExecSqllog("Insert Into LogData ([UserName] ,[Operation]) Values ('" & HttpContext.Current.Session("User") & "','" & loging.Replace("'", "") & "')")

        Dim tt = e.Values().Values(10)
        Dim DailyRec1 As New DataSet
        Dim dbadapter1 = New SqlDataAdapter("SELECT CashierAccount from BranchInfo where BranchNo='" & Session("Branch") & "'", AccConn)
        dbadapter1.Fill(DailyRec1)

        If DailyRec1.Tables(0).Rows(0)(0).ToString = e.Values().Values(10) And Len(RTrim(RecNo.Value)) <> 1 And DailyRec1.Tables(0).Rows.Count = 1 And (AnalsNum.Value = "A" Or AnalsNum.Value = "P") Then
            Dim Inbox As New DataSet
            Dim Inboxdptr = New SqlDataAdapter("SELECT isnull(Sum(Dr),0) As Cash from Journal Where DailyNum='" & DailyNum.Text & "' AND Branch='" & br & "'and (AccountNo='" & DailyRec1.Tables(0).Rows.Item(0).Item("CashierAccount") & "' or left(AccountNo,5)='1.1.1')  and TP=" & Request("sys") & "", AccConn)
            Inboxdptr.Fill(Inbox)
            If Inbox.Tables(0).Rows.Item(0).Item("Cash") <> 0 Then
                'ExecConn("Update Accmove set Payment=" & Inbox.Tables(0).Rows.Item(0).Item("Cash") & ", Amount=" & Inbox.Tables(0).Rows.Item(0).Item("Cash") & " ,PayTP=1, DocDat='" & Format(CDate(DailyDte.Value), "yyyy-MM-dd") & "', Note=Note + 'CANCELLED'" _
                '         & ",UserName= '/ تم الإلغاء بواسطة ' + '" & Session("User") & "' where DocNo='" & RecNo.Value & "'", AccConn)
            Else
                'ExecConn("Update Accmove set Payment=0, Amount=0 , DocDat='" & Format(CDate(DailyDte.Value), "yyyy-MM-dd") & "', Note=Note + 'CANCELLED'" _
                '         & ",UserName= '/ تم الإلغاء بواسطة ' + '" & Session("User") & "' where DocNo='" & RecNo.Value & "'", AccConn)
            End If
            'Inbox.Tables(0).Rows.Item(0).Item("Cash")

        End If
    End Sub

End Class