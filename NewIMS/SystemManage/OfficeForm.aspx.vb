Imports System.Drawing
Imports DevExpress.Utils
Imports DevExpress.Web

Partial Public Class OfficeForm
    Inherits Page

    Private Systems As String
    Private InsSys As String
    Private ClmSys As String
    Private ReSys As String
    Private userRec, BrRec As New DataSet
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Request("Agent") <> "" And Not IsPostBack Then
            'Button1.Text = "حفظ"
            Session("AgentNo") = Request("Agent")
            Dim dbadapter = New SqlClient.SqlDataAdapter("select * from BranchInfo where BranchNo='" & Request("Agent") & "'", Conn)
            dbadapter.Fill(userRec)
            'userRec = RecSet("select * from BranchInfo where BranchNo='" & Request("Br") & "'", Conn)
            BranchName.Text = userRec.Tables(0).Rows(0)("BranchName")
            BranchNo.Text = Right(userRec.Tables(0).Rows(0)("BranchNo"), 3)
            BranchNo.ReadOnly = True
            Address.Text = IIf(String.IsNullOrEmpty(userRec.Tables(0).Rows(0)("Address")), "/", userRec.Tables(0).Rows(0)("Address"))
            Telephone.Text = IIf(String.IsNullOrEmpty(userRec.Tables(0).Rows(0)("Telephone")), "0", userRec.Tables(0).Rows(0)("Telephone"))
            FaxNo.Text = IIf(String.IsNullOrEmpty(userRec.Tables(0).Rows(0)("FaxNo")), "0", userRec.Tables(0).Rows(0)("FaxNo"))
            eMail.Text = IIf(String.IsNullOrEmpty(userRec.Tables(0).Rows(0)("eMail")), "/", userRec.Tables(0).Rows(0)("eMail"))
            BranchNo.Enabled = False
            OfficeManager.Value = IIf(CInt(userRec.Tables(0).Rows(0)("ManagerId")) = 0, 0, CInt(userRec.Tables(0).Rows(0)("ManagerId")))
            AgentOrOffice.Value = userRec.Tables(0).Rows(0)("Agent")
            AgentOrOffice.ReadOnly = True
            'MaxIssu.Text = userRec.Tables(0).Rows(0)("AccLimit")
            BranchesDD.ClearSelection()
            BranchesDD.SelectedValue = Left(Request("Agent"), 2) + "000" 'Request("Br")
        Else
            AgentOrOffice.ReadOnly = True
        End If
        If Request("Update") = "1" Then
            Dim unused = Request("Update").Replace("1", "0")
            ExecConn("update BranchInfo set " _
            & "BranchName='" & BranchName.Text & "'," _
            & "Address='" & Address.Text & "'," _
            & "telephone='" & Telephone.Text & "'," _
            & "FaxNo='" & FaxNo.Text & "'," _
            & "ManagerId=" & IIf(IsNothing(OfficeManager.Value) Or Not IsNumeric(OfficeManager.Value), 0, OfficeManager.Value) & "," _
            & "Agent='" & AgentOrOffice.SelectedItem.Value & "'," _
            & "CompanyOffice='" & IIf(AgentOrOffice.SelectedItem.Value, 0, 1) & "'," _
            & "eMail='" & eMail.Text & "' where BranchNo='" & Request("Agent") & "' ", Conn)
            'Response.Write("<script>alert('هذا الفرع موجود مسبقاً');</script>")
            BranchNo.BackColor = Color.PaleVioletRed
            BranchNo.Focus()
        End If
        If Not IsPostBack Then

        End If
        'If IsHeadQuarter(BranchNo.Value) Then
        '    If Session("User") = "branchAdmin" Then
        '        SqlDataSource2.SelectCommand = "select *,dbo.[BranchName](Branch) As BrName from AccountFile where left(Branch,2)='" & Left(BranchNo.Value, 2) & "' "
        '        SqlDataSource2.Select(DataSourceSelectArguments.Empty)
        '        SqlDataSource1.SelectCommand = "SELECT DISTINCT BranchNo, RTRIM(BranchName) As BranchName FROM BranchInfo where right(BranchNo,3)='000'"
        '        SqlDataSource1.Select(DataSourceSelectArguments.Empty)
        '    Else
        '        SqlDataSource2.SelectCommand = "select *,dbo.[BranchName](Branch) As BrName from AccountFile where left(Branch,2)='" & Left(BranchNo.Value, 2) & "' and AccountName<>'branchAdmin'"
        '        SqlDataSource2.Select(DataSourceSelectArguments.Empty)
        '        SqlDataSource1.SelectCommand = "SELECT DISTINCT BranchNo, RTRIM(BranchName) As BranchName FROM BranchInfo where right(BranchNo,3)='000'"
        '        SqlDataSource1.Select(DataSourceSelectArguments.Empty)
        '    End If
        'Else
        'If IsBranch(Request("Agent")) Then
        '    If Session("User") = "branchAdmin" Then
        '        SqlDataSource2.SelectCommand = "select *,dbo.[BranchName](Branch) As BrName from AccountFile where Branch='" & Request("Agent") & "' and AccountName<>'branchAdmin'"
        '        SqlDataSource2.Select(DataSourceSelectArguments.Empty)
        '        SqlDataSource1.SelectCommand = "SELECT DISTINCT BranchNo, RTRIM(BranchName) As BranchName FROM BranchInfo where right(BranchNo,3)='000' AND LEFT(BranchNo,2)='" & Left(Session("Branch"), 2) & "'"
        '        SqlDataSource1.Select(DataSourceSelectArguments.Empty)
        '    Else
        '        SqlDataSource2.SelectCommand = "select *,dbo.[BranchName](Branch) As BrName from AccountFile where left(Branch,2)='" & Left(BranchNo.Value, 2) & "' and AccountName<>'branchAdmin'"
        '        SqlDataSource2.Select(DataSourceSelectArguments.Empty)
        '        SqlDataSource1.SelectCommand = "SELECT DISTINCT BranchNo, RTRIM(BranchName) As BranchName FROM BranchInfo where right(BranchNo,3)='000' AND LEFT(BranchNo,2)='" & Left(Session("Branch"), 2) & "' and AccountName<>'branchAdmin'"
        '        SqlDataSource1.Select(DataSourceSelectArguments.Empty)
        '    End If
        'Else
        SqlDataSource2.SelectCommand = "select *,dbo.[BranchName](Branch) As BrName from AccountFile where Branch='" & Request("Agent") & "'  and AccountName<>'branchAdmin'"
        SqlDataSource2.Select(DataSourceSelectArguments.Empty)
        SqlDataSource1.SelectCommand = "SELECT DISTINCT BranchNo, RTRIM(BranchName) As BranchName FROM BranchInfo where BranchNo='" & Request("Agent") & "'"
        SqlDataSource1.Select(DataSourceSelectArguments.Empty)
        'End If
        'End If
        AgentCommGrid.DataBind()
        AgentUsers.DataBind()
    End Sub

    Protected Sub AgentUsers_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs)
        Select Case e.Parameters
            Case "Activate"
                Dim Order = AgentUsers.GetRowValues(AgentUsers.FocusedRowIndex, "AccountNo").ToString()
                ExecConn("Update AccountFile Set Stop=0 where AccountNo=" & Order & "", Conn)
            Case Else
                AgentUsers.DataBind()
                'cmbReports.DataBind()
        End Select
        AgentUsers.DataBind()
    End Sub

    Protected Sub AgentUsers_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs)

    End Sub

    Protected Sub AgentUsers_RowDeleting(sender As Object, e As Data.ASPxDataDeletingEventArgs)
        Dim SelectedIndx = AgentUsers.FindVisibleIndexByKeyValue(e.Keys("AccountNo"))

        Dim LogNo = AgentUsers.GetRowValues(SelectedIndx, "AccountNo")

        ExecConn("Update AccountFile Set Stop=1 where AccountNo=" & LogNo & "", Conn)
        AgentUsers.DataBind()
    End Sub

    Protected Sub AgentUsers_ToolbarItemClick(source As Object, e As Data.ASPxGridViewToolbarItemClickEventArgs)
        Select Case e.Item.Name
            Case "NewUser"
                Dim Lvl = "S3"
                AgentUsers.JSProperties("cpMyAttribute") = "New"
                AgentUsers.JSProperties("cpResult") = "إضافة"
                AgentUsers.JSProperties("cpSize") = 1100
                AgentUsers.JSProperties("cpNewWindowUrl") = "../SystemManage/UserForm.aspx?Br=" & Request("Agent") & "&FromAgentsPage=1&Lvl=" + Lvl + ""
            Case Else
                Exit Select
        End Select
    End Sub

    'Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
    '    Dim Conds As String = ""
    '    syss.Text = ""

    '    Dim dbadapter1 = New SqlClient.SqlDataAdapter("select * from BranchInfo where " _
    '                & "BranchNo='" & Left(Session("Branch"), 2) + Trim(BranchNo.Text) & "' and agent=1", Conn)
    '    dbadapter1.Fill(BrRec)

    '    If BrRec.Tables(0).Rows.Count = 0 And BranchNo.Text.Trim.IndexOf(" ") < 0 Then
    '        If Request("Agent") <> "" Then
    '            ExecSql("update BranchInfo set " _
    '             & "BranchName='" & BranchName.Text & "'," _
    '             & "Address='" & Address.Text & "'," _
    '             & "telephone='" & Telephone.Text & "'," _
    '             & "FaxNo='" & FaxNo.Text & "'," _
    '             & "eMail='" & eMail.Text & "' where  left(BranchNo,2)= Left('" & Session("Branch") & "', 2) and right(BranchNo,2)='" & Request("Agent") & "'and agent=1")
    '        Else
    '            If Len(BranchNo.Text) = 2 And BranchNo.Text.Trim.IndexOf(" ") < 0 Then
    '                'If Not CheckBox.Checked Then
    '                ExecSql("insert into BranchInfo (BranchName,BranchNo,Address,telephone,FaxNo,eMail,Agent) values('" _
    '                 & BranchName.Text & "','" _
    '                 & Left(Session("Branch"), 2) + Trim(BranchNo.Text) & "','" _
    '                 & Address.Text & "','" _
    '                 & Telephone.Text & "','" _
    '                 & FaxNo.Text & "','" & eMail.Text & "'," & 1 & ")")
    '                ExecSql("INSERT INTO SUBSYSTEMS " _
    '                & "(SUBSYSNO, Branch, MAINSYS, SUBSYSNAME, SUBSYSNAMEL, LastPolNo, LastOrderNo, LastClaimNo, PolIss, EndIss, PolSTM, EndSTM, Groups, PolReport, SumInsured, ForURL, EditForm, SysType, Grouped)" _
    '                & "SELECT SUBSYSNO, '" & Left(Session("Branch"), 2) + Trim(BranchNo.Text) & "', MAINSYS, SUBSYSNAME, SUBSYSNAMEL, 0, 0, 0, PolIss, EndIss, PolSTM, EndSTM, Groups, PolReport, SumInsured, ForURL, EditForm, SysType, Grouped " _
    '                & "FROM SUBSYSTEMS where Branch=[dbo].[MainCenter]() and subsysno in ('01','OR','27','S1','S4','07','PH')")

    '                ExecSql("INSERT INTO AccountFile " _
    '                & "(AccountLogin, Branch, AccountName, AccountPassWord, " _
    '                & "AccountSysManag,AccLimit)" _
    '                & "SELECT '" & Left(Session("Branch"), 2) + Trim(BranchNo.Text) & "', '" & Left(Session("Branch"), 2) + Trim(BranchNo.Text) & "','branchAdmin','" & Left(Session("Branch"), 2) + Trim(BranchNo.Text) & "', " _
    '                & "'" & "96-S1-5;" & "',AccLimit  FROM AccountFile where AccountLogin='administrator'")
    '                'MsgBox("")
    '                'Me.ClientScript.RegisterStartupScript(Me.GetType, "Msg", "alert('نفسه كلمة المرور'" & BranchNo.Text & "' للتحكم في الأنظمة وصلاحيات المستخدمين يمكنك الدخول بالمستخدم');", True)
    '                Response.Write("<script>alert('للتحكم في الأنظمة وصلاحيات المستخدمين استعمل المستخدم " + Left(Session("Branch"), 2) + Trim(BranchNo.Text) + " وتكون كلمة المرور نفس الاسم""');</script>")
    '                'Response.Write("<script>alert('السيارة المطلوبة لديها وثيقة سارية حتى  " + Format(ChckCar.Tables(0).Rows.Item(0).Item("CoverTo"), "yyyy/MM/d") + " برقم " + ChckCar.Tables(0).Rows.Item(0).Item("PolNo") + "');</script>")
    '            Else
    '                Response.Write("<script>alert('رمز الوكيل يجب أن يكون خانتين');</script>")
    '                BranchNo.BackColor = System.Drawing.Color.PaleVioletRed
    '                BranchNo.Focus()
    '            End If
    '        End If
    '    Else
    '        MsgBox1.confirm("! هذا الوكيل موجود مسبقا، :" & Left(Session("Branch"), 2) + Trim(BranchNo.Text) & "  هل تريد التعديل في بياناته ?", "Update")

    '    End If

    '    'Response.Write("<script>alert('نفسه كلمة المرور'" & BranchNo.Text & "' للتحكم في الأنظمة وصلاحيات المستخدمين يمكنك الدخول بالمستخدم');</script>")
    '    Me.ClientScript.RegisterStartupScript(Me.GetType, "key", "window.close('_this');", True)
    'End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback
        Dim callbackPanel As ASPxCallbackPanel = DirectCast(sender, ASPxCallbackPanel)
        Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)
        Select Case e.Parameter
            Case "Apply"
                syss.Text = ""

                Dim dbadapter1 = New SqlClient.SqlDataAdapter("select * from BranchInfo where " _
                            & "BranchNo='" & Left(Request("Branch"), 2) & Trim(BranchNo.Text) & "' ", Conn)
                dbadapter1.Fill(BrRec)

                If BrRec.Tables(0).Rows.Count = 0 And BranchNo.Text.Trim.IndexOf(" ") < 0 Then
                    If Request("Agent") <> "" Then
                        ExecSql("update BranchInfo set " _
                         & "BranchName='" & BranchName.Text & "'," _
                         & "Address='" & Address.Text & "'," _
                         & "telephone='" & Telephone.Text & "'," _
                         & "FaxNo='" & FaxNo.Text & "'," _
                         & "ManagerId=" & IIf(IsNothing(OfficeManager.Value) Or Not IsNumeric(OfficeManager.Value), 0, OfficeManager.Value) & "," _
                         & "Agent='" & AgentOrOffice.SelectedItem.Value & "'," _
                         & "CompanyOffice='" & IIf(AgentOrOffice.SelectedItem.Value, 0, 1) & "'," _
                         & "eMail='" & eMail.Text & "' where BranchNo='" & Request("Agent") & "'")
                    Else
                        If Len(BranchNo.Text) = 3 And BranchNo.Text.Trim.IndexOf(" ") < 0 Then
                            'If Not CheckBox.Checked Then
                            ExecSql("insert into BranchInfo (BranchName,BranchNo,Address,telephone,FaxNo,eMail,Agent,CompanyOffice,ManagerId) values('" _
                             & BranchName.Text & "','" _
                             & Left(Request("Branch"), 2) & Trim(BranchNo.Text) & "','" _
                             & Address.Text & "','" _
                             & Telephone.Text & "','" _
                             & FaxNo.Text & "','" & eMail.Text & "','" & AgentOrOffice.SelectedItem.Value & "','" & IIf(AgentOrOffice.SelectedItem.Value, 0, 1) & "'," & IIf(IsNothing(OfficeManager.Value) Or Not IsNumeric(OfficeManager.Value), 0, OfficeManager.Value) & ")")

                            Session("AgentNo") = Left(Request("Branch"), 2) + Trim(BranchNo.Text)
                            ExecSql("INSERT INTO SUBSYSTEMS " _
                            & "(SUBSYSNO, SUBSYSCODE, Branch, MAINSYS, SUBSYSNAME, SUBSYSNAMEL, LastPolNo, LastOrderNo, LastClaimNo, PolIss, EndIss, PolSTM, EndSTM, Groups, PolReport, SumInsured, ForURL, EditForm, SysType, Grouped)" _
                            & "SELECT SUBSYSNO, SUBSYSCODE, '" & Left(Request("Branch"), 2) & Trim(BranchNo.Value) & "', MAINSYS, SUBSYSNAME, SUBSYSNAMEL, 0, 0, 0, PolIss, EndIss, PolSTM, EndSTM, Groups, PolReport, SumInsured, ForURL, EditForm, SysType, Grouped " _
                            & "FROM SUBSYSTEMS where Branch=[dbo].[MainCenter]() and subsysno in ('01','OR','27','08','04','S1')")

                            ExecSql("INSERT INTO AccountFile " _
                            & "(AccountLogin, Branch, AccountName, AccountPassWord, " _
                            & "AccountSysManag,AccLimit,Stop)" _
                            & "SELECT '" & Left(Request("Branch"), 2) & Trim(BranchNo.Value) & "', '" & Left(Request("Branch"), 2) & Trim(BranchNo.Value) & "','branchAdmin','" & BCrypt.Net.BCrypt.HashPassword(Left(Session("Branch"), 2) & Trim(BranchNo.Value)) & "', " _
                            & "'" & "96-S1-5;" & "',AccLimit,1  FROM AccountFile where AccountLogin='administrator'")

                            Dim ParentAcc As String
                            If IsHeadQuarter(Request("Branch")) Then
                                ParentAcc = "1.1.3.3"
                            Else
                                If IsExistsAcc("1.1.3.3.0") Then

                                Else
                                    ExecConn("insert into Accounts (AccountNo, ParentAcc, AccountName) " _
                                             & " Values('1.1.3.3.0','1.1.3.3','" _
                                             & "جاري وكلاء ومكاتب الفروع" & "')", Conn)
                                End If
                                ParentAcc = "1.1.3.3" & ".0." & GetBranchAcc(Request("Branch"))
                            End If
                            If IsExistsAcc(ParentAcc) Then

                            Else
                                ExecConn("insert into Accounts (AccountNo, ParentAcc, AccountName) values('" _
                                          & ParentAcc & "','1.1.3.3.0','" _
                                          & "جاري وكلاء ومكاتب /" + GetBranchName(Request("Branch")) & "')", Conn)
                            End If

                            'ParentAcc = ParentAcc.Replace(".", "")
                            Dim lastAcc = GetLastAccount(ParentAcc).ToString
                            Dim NewMainAgentAcc = ParentAcc + "." + lastAcc
                            ExecConn("insert into Accounts (AccountNo, ParentAcc, AccountName) values('" _
                                         & NewMainAgentAcc & "','" _
                                         & ParentAcc & "','" _
                                         & BranchName.Value.ToString & "')", Conn)

                            For Agaccs As Integer = 1 To 5
                                Dim Acctp As String = ""
                                Dim TPCode As String = ""
                                Select Case Agaccs
                                    Case 1
                                        Acctp = " إجباري "
                                        TPCode = "01"
                                    Case 2
                                        Acctp = " بطاقة موحدة "
                                        TPCode = "OR"
                                    Case 3
                                        Acctp = " مساقرين "
                                        TPCode = "27"
                                    Case 4
                                        Acctp = " تكميلي / طرف ثالث "
                                        TPCode = "04"
                                    Case 5
                                        Acctp = " مسئولية طبية "
                                        TPCode = "08"
                                    Case Else
                                        Exit Select
                                End Select
                                ExecConn("insert into Accounts (AccountNo, ParentAcc, AccountName) values('" _
                                    & NewMainAgentAcc & "." & Agaccs.ToString & "','" _
                                    & NewMainAgentAcc & "','" _
                                    & BranchName.Value.ToString & "/" & Acctp.ToString & "')", Conn)

                                ExecConn("INSERT INTO AgentsCommisions (AgentNo,SubIns,Comm,AccountNo) VALUES ('" _
                                     & Left(Request("Branch"), 2) & Trim(BranchNo.Text) & "', '" _
                                     & TPCode & "', " _
                                     & 0 & ",'" _
                                     & NewMainAgentAcc & "." & Agaccs.ToString & "')", Conn)
                            Next
                            'MsgBox("")
                            'Me.ClientScript.RegisterStartupScript(Me.GetType, "Msg", "alert('نفسه كلمة المرور'" & BranchNo.Text & "' للتحكم في الأنظمة وصلاحيات المستخدمين يمكنك الدخول بالمستخدم');", True)
                            'Response.Write("<script>alert('للتحكم في الأنظمة وصلاحيات المستخدمين استعمل المستخدم " + Left(Session("Branch"), 2) + Trim(BranchNo.Text) + " وتكون كلمة المرور نفس الاسم""');</script>")
                            'ASPxLabel1.Text = "للتحكم في الأنظمة وصلاحيات المستخدمين استعمل المستخدم " & Left(Session("Branch"), 2) & Trim(BranchNo.Value) & " وتكون كلمة المرور نفس الاسم"
                            ASPxLabel1.Visible = True
                            btnShow.Enabled = False
                            ASPxWebControl.RedirectOnCallback("~/SystemManage/OfficeForm.aspx?Agent=" & Left(Session("Branch"), 2) & Trim(BranchNo.Value) & "")
                            'Response.Write("<script>alert('السيارة المطلوبة لديها وثيقة سارية حتى  " + Format(ChckCar.Tables(0).Rows.Item(0).Item("CoverTo"), "yyyy/MM/d") + " برقم " + ChckCar.Tables(0).Rows.Item(0).Item("PolNo") + "');</script>")
                        Else
                            Response.Write("<script>alert('رمز الوكيل يجب أن يكون  3 خانات');</script>")
                            BranchNo.BackColor = Color.PaleVioletRed
                            BranchNo.Focus()
                        End If
                    End If
                Else
                    'MsgBox1.confirm("! هذا الوكيل موجود مسبقا، :" & Left(Session("Branch"), 2) + Trim(BranchNo.Text) & "  هل تريد التعديل في بياناته ?", "Update")
                    If Request("Agent") <> "" Then
                        ExecConn("update BranchInfo set " _
                       & "BranchName='" & BranchName.Value & "'," _
                       & "Address='" & Address.Value & "'," _
                       & "telephone='" & Telephone.Value & "'," _
                       & "FaxNo='" & FaxNo.Value & "'," _
                       & "ManagerId=" & IIf(IsNothing(OfficeManager.Value) Or Not IsNumeric(OfficeManager.Value), 0, OfficeManager.Value) & "," _
                       & "Agent='" & AgentOrOffice.SelectedItem.Value & "'," _
                       & "eMail='" & eMail.Text & "' where BranchNo='" & Left(Request("Branch"), 2) & Trim(BranchNo.Value) & "'", Conn)
                        ASPxLabel1.Text = "تم تعدبل بيانات " & IIf(AgentOrOffice.SelectedItem.Value, "الوكيل", "المكتب")
                        ASPxLabel1.Visible = True
                        btnShow.Enabled = False
                    Else
                        ASPxLabel1.Visible = True
                        BranchNo.BackColor = Color.PaleVioletRed
                        BranchNo.Focus()
                    End If
                    If Len(BranchNo.Text) = 3 And BranchNo.Text.Trim.IndexOf(" ") < 0 Then
                    Else

                    End If
                End If

                'Response.Write("<script>alert('نفسه كلمة المرور'" & BranchNo.Text & "' للتحكم في الأنظمة وصلاحيات المستخدمين يمكنك الدخول بالمستخدم');</script>")
                ClientScript.RegisterStartupScript(Me.GetType, "key", "window.close('_this');", True)
            Case Else
                Exit Select
        End Select
    End Sub

    Private Sub AgentUsers_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs) Handles AgentUsers.CustomButtonCallback
        Select Case e.ButtonID
            Case "Edit"
                Dim Log = AgentUsers.GetRowValues(e.VisibleIndex, "AccountNo").ToString()
                Dim logname = AgentUsers.GetRowValues(e.VisibleIndex, "AccountName").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim Br = AgentUsers.GetRowValues(e.VisibleIndex, "Branch").ToString()

                If Page.IsCallback Then
                    AgentUsers.JSProperties("cpMyAttribute") = "Edit"
                    AgentUsers.JSProperties("cpResult") = logname
                    AgentUsers.JSProperties("cpSize") = 1100
                    Dim Lvl = "S3"
                    AgentUsers.JSProperties("cpNewWindowUrl") = "../SystemManage/UserForm.aspx?log=" + Log + "&Br=" + Br + "&FromAgentsPage=1&Lvl=" + Lvl + ""
                    'Response.Redirect("../SystemManage/UserForm.aspx? " + "?Log=" + Log + "&Br=" + Br & "")
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If
            Case "Activate"
                Dim Log = AgentUsers.GetRowValues(e.VisibleIndex, "AccountNo").ToString()
                Dim logname = AgentUsers.GetRowValues(e.VisibleIndex, "AccountName").ToString().Trim
                AgentUsers.JSProperties("cpRowIndex") = e.VisibleIndex
                AgentUsers.JSProperties("cpMyAttribute") = "Activation"
                AgentUsers.JSProperties("cpCust") = logname
                AgentUsers.JSProperties("cpShowIssueConfirmBox") = True
                'End If

            Case "Delete"
                Dim Log = AgentUsers.GetRowValues(e.VisibleIndex, "AccountNo").ToString()
                Dim logname = AgentUsers.GetRowValues(e.VisibleIndex, "AccountName").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)

                AgentUsers.JSProperties("cpRowIndex") = e.VisibleIndex
                AgentUsers.JSProperties("cpMyAttribute") = "Delete"
                AgentUsers.JSProperties("cpCust") = logname
                AgentUsers.JSProperties("cpShowDeleteConfirmBox") = True
            Case "ResetPass"
                Dim Log = AgentUsers.GetRowValues(e.VisibleIndex, "AccountNo").ToString()
                Dim logname = AgentUsers.GetRowValues(e.VisibleIndex, "AccountName").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim Br = AgentUsers.GetRowValues(e.VisibleIndex, "Branch").ToString()
                If Page.IsCallback Then
                    AgentUsers.JSProperties("cpMyAttribute") = "ResetPass"
                    AgentUsers.JSProperties("cpResult") = logname

                    AgentUsers.JSProperties("cpSize") = 550

                    AgentUsers.JSProperties("cpNewWindowUrl") = "../SystemManage/ResetPassword.aspx?log=" + Log + "&Br=" + Br
                    'Response.Redirect("../SystemManage/UserForm.aspx? " + "?Log=" + Log + "&Br=" + Br & "")
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If

            Case Else
                Exit Select
        End Select
    End Sub

    Private Sub AgentUsers_CustomButtonInitialize(sender As Object, e As ASPxGridViewCustomButtonEventArgs) Handles AgentUsers.CustomButtonInitialize
        Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))
        Dim Stopped As Boolean = grid.GetRowValues(e.VisibleIndex, "Stop") 'IsStoped(grid.GetRowValues(e.VisibleIndex, "PolNo").ToString().Trim, grid.GetRowValues(e.VisibleIndex, "EndNo").ToString(), grid.GetRowValues(e.VisibleIndex, "LoadNo").ToString())
        Dim TT As Integer = CType(Mid(myList(6), InStr(1, myList(6), "S3") + 3, 1), Short)
        Select Case e.ButtonID
            Case "Activate"
                If Stopped Then
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                Else
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
            Case "Delete"
                If Stopped Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                Else
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                End If

            Case "Edit"
                If Stopped Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                Else
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                End If
            Case "ResetPass"
                If Stopped Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                    Exit Sub
                Else
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                    Exit Sub
                End If

                If TT < 5 Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                Else
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                End If

            Case Else
                Exit Select
        End Select
    End Sub

    Protected Sub AgentCommGrid_RowDeleted(sender As Object, e As Data.ASPxDataDeletedEventArgs)

        Dim loging = "تم حذف الحساب رقم " & e.Values().Values(1) & " وبعمولة " & e.Values().Values(4) & " من الوكيل رقم " & e.Values().Values(3) & ""

        ExecSqllog("Insert Into LogData ([UserName] ,[Operation]) Values ('" & HttpContext.Current.Session("UserID") & "','" & loging.Replace("'", "") & "')")
        'e.values()
    End Sub

    Protected Sub AgentCommGrid_RowDeleting(sender As Object, e As Data.ASPxDataDeletingEventArgs)

    End Sub

    Private Sub AgentUsers_HtmlRowPrepared(sender As Object, e As ASPxGridViewTableRowEventArgs) Handles AgentUsers.HtmlRowPrepared
        If AgentUsers.GetRowValues(e.VisibleIndex, "Stop") Then
            e.Row.BackColor = Color.DarkGray
            e.Row.ForeColor = Color.White
        Else

        End If
    End Sub

End Class