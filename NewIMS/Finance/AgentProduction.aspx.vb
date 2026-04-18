Imports System.Data.SqlClient
Imports System.Drawing
Imports DevExpress.Web
Imports Microsoft.Reporting.WebForms

Public Class AgentProduction
    Inherits Page

    Private Err As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = TryCast(Session("UserInfo"), List(Of String))
        If myList Is Nothing Then
            FormsAuthentication.RedirectToLoginPage()
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 3)
        End If
        ProdDate.MaxDate = DateAdd(DateInterval.Month, -1, Today.Date())

        If IsCallback And Agent.Value IsNot Nothing And ProdDate.Value IsNot Nothing Then
            Dim Transferred As New DataSet
            Using con1 As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                If con1.State = ConnectionState.Open Then
                    con1.Close()
                Else

                End If
                con1.Open()
                Dim dbadapter = New SqlDataAdapter("Select top(1) financed From PolicyFile Where Branch='" & Agent.Value & "' " _
                                  & " AND Month(IssuDate)=" & Month(ProdDate.Value) & " AND Year(IssuDate)=" & Year(ProdDate.Value) & "", con1)

                dbadapter.Fill(Transferred)

                If Transferred.Tables(0).Rows.Count = 0 Then
                    ProductionGrid.SettingsText.EmptyDataRow = "لا يوجد انتاج لهذا الشهر"
                Else
                    If Transferred.Tables(0).Rows(0)("Financed") Then
                        ProductionGrid.SettingsText.EmptyDataRow = " تم ترحيل الانتاج " & Agent.Text & "  لشهر " & Format(ProdDate.Value, "yyyy/MM")
                    End If
                End If
                con1.Close()
            End Using
            Session("Mnth") = Month(ProdDate.Value)
            Session("Yer") = Year(ProdDate.Value)

            ProductionGrid.DataBind()

            ProductionGrid.GroupBy(ProductionGrid.Columns("Comment"))

            ProductionGrid.ExpandAll()
        Else
            Session("Mnth") = ""
            Session("Yer") = ""
        End If

    End Sub

    Protected Sub ProductionGrid_ToolbarItemClick(source As Object, e As Data.ASPxGridViewToolbarItemClickEventArgs)

    End Sub

    Protected Sub ProductionGrid_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs)
        Select Case e.Parameters
            Case "IssueJournal"
                'ProductionGrid.Toolbars.FindByName("IssueAgentJournal").Enabled = False

                Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    Else

                    End If
                    con.Open()
                    Dim Comnt = ""
                    Dim JournalCmd As New SqlCommand With {
                        .CommandText = "JournalEntryMonthlyAgentBulk",
                        .CommandType = CommandType.StoredProcedure
                    }
                    If Agent.Value Is Nothing Or ProdDate.Value Is Nothing Then
                        Err = "يرجى اختيار الوكيل أو المكتبوالشهر ليتم ترحسل الانتاج"
                        GoTo Errr
                    End If

                    JournalCmd.Parameters.AddWithValue("@Br", Odbc.OdbcType.NVarChar).Value = Agent.Value.ToString
                    JournalCmd.Parameters.AddWithValue("@Month", Odbc.OdbcType.Int).Value = Month(ProdDate.Value)
                    JournalCmd.Parameters.AddWithValue("@Year", Odbc.OdbcType.Int).Value = Year(ProdDate.Value)
                    JournalCmd.Connection = con

                    Dim myReader As SqlDataReader = JournalCmd.ExecuteReader()
                    If myReader.HasRows Then

                        Dim dt As New DataTable()
                        dt.Load(myReader)
                        Dim JournalList = dt.AsEnumerable.GroupBy(Function(r) Tuple.Create(r("SubIns").ToString, r("Comment").ToString)).Select(Function(g) g.CopyToDataTable).ToList

                        For k As Integer = 0 To JournalList.Count - 1
                            Comnt += JournalList(k).Rows(0).ItemArray(11) & vbCrLf
                        Next k

                        Parm = Array.CreateInstance(GetType(SqlParameter), 3)
                        SetPm("@TP", DbType.String, "1", Parm, 0)
                        SetPm("@Year", DbType.String, Right(Year(ProdDate.Value).ToString, 2), Parm, 1)
                        SetPm("@Br", DbType.String, Session("Branch"), Parm, 2)

                        Dim Dn = CallSP("LastDailyNo", con, Parm)

                        For i As Integer = 0 To JournalList.Count - 1
                            If i = 0 Then
                                ExecConn("INSERT INTO MainJournal([DAILYNUM],[DAILYDTE],[DailyTyp],[ANALSNUM],[DailyChk] ,[DailyPrv] ,[Comment],[CurUser],[Branch],[SubBranch]) " _
                                 & " VALUES ('" & Dn & "','" & Format(JournalList(i).Rows(0).ItemArray(4), "yyyy-MM-dd") & "', 1,'AG', 1, 0,'" & Comnt & "'," _
                                 & "'" & Session("User") & "','" & JournalList(i).Rows(0).ItemArray(3) & "','" & JournalList(i).Rows(0).ItemArray(10) & "')", con)
                            Else

                            End If

                            For j As Integer = 0 To JournalList(i).Rows.Count - 1
                                If JournalList(i).Rows(j).IsNull("AccountNumber") Or JournalList(i).Rows(j).ItemArray(2) Is Nothing Or JournalList(i).Rows(j).ItemArray(2).ToString.Trim = "/" Then
                                    ExecConn("Delete MainJournal where DAILYNUM='" & Dn & "' and DailyTyp=1", con)
                                    ExecConn("Delete Journal where DAILYNUM='" & Dn & "' and TP=1", con)
                                    ExecConn("UPDATE PolicyFile set financed=0 where " _
                                    & " Branch='" & JournalList(i).Rows(i).ItemArray(10) & "' AND MONTH(ISSUDATE)=" & Month(ProdDate.Value) & " AND YEAR(ISSUDATE)=" & Year(ProdDate.Value) & "", con)
                                    ProductionGrid.SettingsText.Title = "خطأ في حسابات الوكيل/المكتب يرجي التأكد من إدخال حسابات الوكيل/المكتب" & Agent.Text.ToString.Trim
                                    Err = ProductionGrid.SettingsText.Title
                                    GoTo Errr
                                Else
                                    ExecConn("INSERT INTO Journal ([DAILYNUM], [TP], [AccountNo], [Dr], [Cr],[CurUser] ,[Branch] ,[SubBranch]) " _
                                   & " VALUES ('" & Dn & "', 1, '" & JournalList(i).Rows(j).ItemArray(2) & "', " & JournalList(i).Rows(j).ItemArray(6) & ", " _
                                   & " " & JournalList(i).Rows(j).ItemArray(7) & ",'" & Session("User") & "','" & JournalList(i).Rows(j).ItemArray(3) & "','" & JournalList(i).Rows(j).ItemArray(10) & "')", con)
                                End If
                                ExecConn("UPDATE PolicyFile set financed=1 where SubIns='" & JournalList(i).Rows(j).ItemArray(1) & "' And  Branch='" & Agent.Value.ToString & "' AND MONTH(ISSUDATE)=" & Month(ProdDate.Value) & " AND YEAR(ISSUDATE)=" & Year(ProdDate.Value) & "", con)
                            Next j
                            'ExecConn("UPDATE PolicyFile set financed=1 where SubIns='" & JournalList(i).Rows(i).ItemArray(1) & "' AND " _
                            '         & "Branch='" & JournalList(i).Rows(i).ItemArray(10) & "' AND MONTH(ISSUDATE)=" & Month(ProdDate.Value) & " AND YEAR(ISSUDATE)=" & Year(ProdDate.Value) & "", con)
                        Next i
                    Else

                    End If

                    con.Close()
                End Using

                ProductionGrid.SettingsText.Title = "تم ترحيل الانتاج بنجاح الوكيل/المكتب " & Agent.Value.ToString & " لشهر " & Month(ProdDate.Value).ToString & " / " & Year(ProdDate.Value).ToString
                Err = ""
            Case "IssuesAgent"
                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("Month", Month(Today.Date), True),
                    New ReportParameter("Year", Year(Today.Date), True)
                }
                Session.Add("Parms", P)

                Dim Report = ReportsPath & "AgentsProduction"
                ProductionGrid.JSProperties("cpResult") = "تقرير الوكلاء/المكاتب الغير مرحلة"
                ProductionGrid.JSProperties("cpMyAttribute") = "PRINT"
                ProductionGrid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
                Exit Sub
            Case Else
                Exit Select
        End Select
Errr:
        If e.Parameters = "" Then
            ProductionGrid.DataBind()
            ProductionGrid.GroupBy(ProductionGrid.Columns("Comment"))
            ProductionGrid.ExpandAll()
        Else
            If Err = "" Then
                ProductionGrid.SettingsText.Title = " تم ترحيل الانتاج بنجاح الوكيل/المكتب " & Agent.Value.ToString & " لشهر " & Month(ProdDate.Value).ToString & " / " & Year(ProdDate.Value).ToString
                Err = ProductionGrid.SettingsText.Title
                ProductionGrid.DataBind()
                ProductionGrid.GroupBy(ProductionGrid.Columns("Comment"))
                ProductionGrid.ExpandAll()
                ProductionGrid.Styles.TitlePanel.ForeColor = Color.Green
                ProductionGrid.SettingsText.Title = " تم ترحيل الانتاج بنجاح الوكيل/المكتب " & Agent.Value.ToString & " لشهر " & Month(ProdDate.Value).ToString & " / " & Year(ProdDate.Value).ToString
            Else
                ProductionGrid.SettingsText.Title = " خطأ في حسابات الوكيل/المكتب يرجي التأكد من إدخال حسابات الوكيل/المكتب " & Agent.Text.ToString.Trim
                Err = ProductionGrid.SettingsText.Title
                ProductionGrid.DataBind()
                ProductionGrid.GroupBy(ProductionGrid.Columns("Comment"))
                ProductionGrid.ExpandAll()
                ProductionGrid.SettingsText.Title = " خطأ في حسابات الوكيل/المكتب يرجي التأكد من إدخال حسابات الوكيل/المكتب " & Agent.Text.ToString.Trim
                ProductionGrid.Styles.TitlePanel.ForeColor = Color.Red
                Err = ProductionGrid.SettingsText.Title
            End If

        End If

    End Sub

    Protected Sub Accnam_DataBound(sender As Object, e As EventArgs)
        Dim Acclabel As ASPxLabel = CType(sender, ASPxLabel)
        Acclabel.Text = If(IsDBNull(DirectCast(Acclabel.DataItemContainer, GridViewDataItemTemplateContainer).[Text]),
            "خطأ في رقم الحساب",
            Getaccname(DirectCast(Acclabel.DataItemContainer, GridViewDataItemTemplateContainer).[Text]))

    End Sub

    Protected Sub ProductionGrid_HtmlRowPrepared(sender As Object, e As ASPxGridViewTableRowEventArgs)
        If e.RowType <> GridViewRowType.Data Then
            Return
        End If
        ' Get the value of a specific column for the current row
        Dim categoryValue As Boolean = IIf(String.IsNullOrEmpty(e.GetValue("AccountNumber").ToString), False, IsAccNo(e.GetValue("AccountNumber").ToString)) ' Replace "Category" with your actual column name

        ' Apply conditional formatting based on the column value
        If categoryValue Then
            'e.Row.BackColor = Color.LightGreen
            'ElseIf categoryValue = 2 Then
            '    e.Row.BackColor = Color.LightBlue
        Else
            e.Row.BackColor = Color.Red
            e.Row.ForeColor = Color.White
        End If
    End Sub

End Class