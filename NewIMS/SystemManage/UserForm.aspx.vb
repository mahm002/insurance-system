Imports System.Data.Common
Imports System.Data.SqlClient
Imports DevExpress.Web

Public Class UserForm
    Inherits Page

    Private Systems As String
    Private InsSys As String
    Private ClmSys As String
    Private ReSys As String
    Private userRec, LogName As New DataSet

    ' Dim Lo() As String
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Branches.SelectCommand = If(Request("Br") <> "", If(IsHeadQuarter(Request("Br")), "Select * From BranchInfo", IIf(IsBranch(Session("Branch")), "Select * From BranchInfo where left(BranchNo,2)='" & Left(Request("Br"), 2) & "'", "Select * From BranchInfo where BranchNo='" & Session("Branch") & "'")))
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))
        If Request("FromAgentsPage") = 1 And InStr(1, myList(8), "S1") = 0 Then
            If IsHeadQuarter(Request("Br")) And CType(Mid(myList(6), InStr(1, myList(6), Request("Lvl")) + 3, 1), Short) >= 2 Then
                Rule.ClientEnabled = True
            Else
                If IsBranch(Session("Branch")) And CType(Mid(myList(6), InStr(1, myList(6), Request("Lvl")) + 3, 1), Short) >= 2 Then
                    Rule.ClientEnabled = True
                Else
                    Rule.ClientEnabled = False
                End If
            End If
        Else
            If CType(Mid(myList(6), InStr(1, myList(6), "S1") + 3, 1), Short) < 5 Then
                Rule.ClientEnabled = False
            Else
                Rule.ClientEnabled = True
            End If
        End If


        Select Case Request("Br")
            Case ""
                If IsHeadQuarter(Request("Br")) Then
                    Branches.SelectCommand = "Select * From BranchInfo"
                Else
                    If IsBranch(Session("Branch")) Then
                        Branches.SelectCommand = "Select * From BranchInfo where left(BranchNo,2)='" & Left(Request("Br"), 2) & "'"
                    Else
                        Branches.SelectCommand = "Select * From BranchInfo where BranchNo='" & Session("Branch") & "'"
                        Rule.Visible = False
                    End If
                End If
            Case Else
                If IsHeadQuarter(Request("Br")) Then
                    Branches.SelectCommand = "Select * From BranchInfo"
                Else
                    If IsBranch(Session("Branch")) Then
                        Branches.SelectCommand = "Select * From BranchInfo where left(BranchNo,2)='" & Left(Request("Br"), 2) & "'"
                    Else
                        Branches.SelectCommand = "Select * From BranchInfo where BranchNo='" & Session("Branch") & "'"
                        Rule.Visible = False
                    End If
                End If
        End Select
        'If Request("log") <> "" Then
        '    btnShow.Text = " ⁄œÌ·"
        'Else
        '    btnShow.Text = "≈÷«›…"
        'End If
        IIf(IsHeadQuarter(Session("Branch")), BranchesD.ClientVisible = True, IIf(IsBranch(Session("Branch")), BranchesD.ClientVisible = True, BranchesD.ClientVisible = False))
        If Not IsPostBack Then
            If Request("log") <> "" Then
                Signature_Init(sender, e)
                Dim dbadapter = New SqlDataAdapter("select * from AccountFile where AccountNo=" & Request("log") & " And Branch='" & Request("Br") & "'", Conn)
                Dim unused1 = dbadapter.Fill(userRec)

                AccountName.Value = userRec.Tables(0).Rows(0)("AccountName").trim
                AccountLogIn.Value = userRec.Tables(0).Rows(0)("Accountlogin").trim
                PassWord.Value = userRec.Tables(0).Rows(0)("AccountPassWord").trim
                MaxIssu.Text = userRec.Tables(0).Rows(0)("AccLimit")
                'BranchesDD.ClearSelection()
                'BranchesDD.SelectedValue = Request("Br")
                'BranchesDD.Enabled = False
                BranchesD.SelectedIndex = -1
                BranchesD.Value = Request("Br")
                'BranchesD.Enabled = False
            End If
        End If
        If Not IsPostBack Then
            RootSystems.SelectedIndex = 0
            RootSystems_SelectedIndexChanged(Me, Nothing)
            BranchesD.Value = Session("Branch")
            SqlDataSource1.ConnectionString = Conn.ConnectionString
            SqlDataSource1.SelectCommand = "select *,str(Mainsys)+'-'+subsysno as subsysvalue from SUBSYSTEMS where " _
                & "SysType=" & RootSystems.Value & " AND Branch='" & Request("Br") & "' order By MainSys"
            Dim unused = SqlDataSource1.Select(DataSourceSelectArguments.Empty)

            SystemsList.Items.Clear()
            SystemsList.DataBind()

            If userRec.Tables.Count = 0 Then
                GoTo M
            Else
                Select Case RootSystems.SelectedIndex
                    Case 0 : Systems = userRec.Tables(0).Rows(0)("AccountPermSys")
                    Case 1 : Systems = userRec.Tables(0).Rows(0)("AccountPermClm")
                    Case 2 : Systems = userRec.Tables(0).Rows(0)("AccountPermFin")
                    Case 3 : Systems = userRec.Tables(0).Rows(0)("AccountPermRe")
                    Case 4 : Systems = userRec.Tables(0).Rows(0)("AccountPermMan")
                    Case 5 : Systems = userRec.Tables(0).Rows(0)("AccountSysManag")
                    Case Else
                        Exit Select
                End Select
            End If

M:          Dim i As Integer = 1

            While i < Len(Systems)
                Dim j As Integer = InStr(i, Systems, ";")
                Dim PermChar As String = Mid(Systems, i, j - 1)
                Rule.Value = CInt(Mid(Systems, InStr(1, Systems, ";") - 1, 1))
                For Ci As Integer = 0 To SystemsList.Items.Count - 1
                    If InStr(Systems, Trim(SystemsList.Items(Ci).Value)) <> 0 Then
                        SystemsList.Items(Ci).Selected = True
                    Else
                        SystemsList.Items(Ci).Selected = False
                    End If
                Next
                i = j + 1
            End While
        End If
        If Request("log") <> "" And Not IsPostBack Then
            Dim dbadapter1 = New SqlDataAdapter("select * from AccountFile where AccountNo=" & Request("log") & " And Branch='" & BranchesD.Value & "'", Conn)
            dbadapter1.Fill(userRec)
            'BranchesDD.ClearSelection()
            'BranchesDD.SelectedValue = Request("Br")
            BranchesD.SelectedIndex = -1
            BranchesD.Value = Request("Br")
            PassWord.ClientVisible = False
            ConfirmPass.ClientVisible = False
        Else
            'BranchesD.SelectedIndex = -1
            'BranchesD.Value = Trim(BranchesD.Value)
            'BranchesD.Enabled = False
        End If
        If Request("FromAgentsPage") = 1 Then
            BranchesD.SelectedIndex = -1
            BranchesD.Value = Request("Br")
            BranchesD.ReadOnly = True
            RootSystems.ReadOnly = True
            'DropDownList1.Enabled = False
        End If
    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback
        Dim callbackPanel As ASPxCallbackPanel = DirectCast(sender, ASPxCallbackPanel)
        Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)

        Select Case e.Parameter
            Case "Apply"
                syss.Text = ""
                If Request("log") = "" Then
                    If PassWord.Text.ToString.Trim <> "" Then
                        If PassWord.Text.ToString.Trim <> ConfirmPass.Text.ToString.Trim Then
                            lblMessage.Text = "THE PASSWORD AND THE CONFIRMATION PASSWORD NOT MATCHING."
                            Exit Sub
                        End If
                    Else
                        Exit Sub
                    End If
                End If
                For Ci As Integer = 0 To SystemsList.Items.Count - 1
                    Dim substr As String = SystemsList.Items(Ci).Value.ToString.Trim + "-" & Rule.Value.ToString.Trim + ";"
                    Dim test As Integer = InStr(syss.Text, substr)
                    If SystemsList.Items(Ci).Selected And InStr(syss.Text, SystemsList.Items(Ci).Value + "-" & Rule.Value.ToString.Trim + ";") = 0 Then
                        syss.Text = syss.Text + Trim(SystemsList.Items(Ci).Value.ToString.Trim) + "-" & Rule.Value.ToString.Trim + ";"
                    End If
                Next

                Dim SelSys As String = ""

                Select Case RootSystems.SelectedIndex
                    Case 0 : SelSys = "AccountPermSys" '«·ÊÀ«∆ﬁ
                    Case 1 : SelSys = "AccountPermClm" ' «· ⁄ÊÌ÷« 
                    Case 2 : SelSys = "AccountPermFin" ' «·Õ”«»« 
                    Case 3 : SelSys = "AccountPermRe" ' «·≈⁄«œ…
                    Case 4 : SelSys = "AccountPermMan" ' «·≈œ«—Ì…
                    Case 5 : SelSys = "AccountSysManag" ' ≈œ«—… «·‰Ÿ«„
                    Case Else
                        Exit Select
                End Select
                If Request("log") <> "" Then
                    'Dim hashedPassword As String = BCrypt.Net.BCrypt.HashPassword(PassWord.Value.ToString.Trim)
                    '& "AccountPassWord='" & hashedPassword & "'," _
                    ExecSql("update AccountFile set " _
                & "AccountName='" & AccountName.Value & "'," _
                & "AccountLogIn='" & AccountLogIn.Value & "'," _
                & "ModifiedBy='" & Session("UserID") & "'," _
                & "ModifyDate='" & Now.ToString("yyyy/MM/dd HH:mm:ss").ToString & "'," _
                & "Branch='" & BranchesD.Value & "'," _
                & "AccLimit='" & MaxIssu.Text & "'," _
                & SelSys & "='" & syss.Text & "' where AccountNo=" & Request("log") & " and Branch='" & Request("Br") & "'")
                    SignDs.Update()
                Else

                    Dim dbadapter2 = New SqlDataAdapter("select * from AccountFile where Accountlogin='" & RTrim(AccountLogIn.Text) & "'", Conn)
                    Dim unused = dbadapter2.Fill(LogName)

                    Dim Dept As New DataSet

                    Dim dbadapter1 = New SqlDataAdapter("select * from Departments where nParentIdn='" & Session("UserID") & "' ", Conn)
                    dbadapter1.Fill(Dept)
                    Dim Department As String
                    Dim department1 As Integer
                    If Request("dep") = "" Then
                        Department = "Dept"
                        department1 = 0
                    Else
                        Department = "Dept"
                        department1 = Val(Dept.Tables(0).Rows.Item(0).Item(0))
                    End If
                    If LogName.Tables(0).Rows.Count = 0 Then
                        Dim hashedPassword As String = BCrypt.Net.BCrypt.HashPassword(PassWord.Value.ToString.Trim)

                        ExecSql("insert into AccountFile(AccountName,AccountLogin,AccountPassWord,AccLimit,AddedBy," & Department & ", " & SelSys & ", Branch) values('" _
                    & AccountName.Text & "','" _
                    & AccountLogIn.Text & "','" _
                    & hashedPassword & "'," _
                    & MaxIssu.Text & ",'" _
                    & Session("UserID") & "'," _
                    & department1 & ",'" _
                    & syss.Text & "','" & BranchesD.Value & "')")
                        'SignDs.Update()
                        dbadapter2.Fill(LogName)

                        'Dim SS = "~/SystemManage/UserForm.aspx?log=" & GetUserNoByLogin(AccountLogIn.Value) & "&Br=" & BranchesD.Value.ToString & ""
                        'ClientScript.RegisterStartupScript(Me.GetType, "key", "window.close('_this');", True)
                        ASPxWebControl.RedirectOnCallback("~/SystemManage/UserForm.aspx?log=" & GetUserNoByLogin(AccountLogIn.Value) & "&Br=" & BranchesD.Value.ToString & "")
                    Else
                        'esponse.Write("<script>alert('«”„ «·œŒÊ· „ÕÃÊ“ „”»ﬁ« ');</script>")
                        AccountLogIn.BackColor = Drawing.Color.PaleVioletRed
                        AccountLogIn.Focus()
                    End If
                End If

            Case Else
                Exit Select
        End Select
    End Sub

    Private Sub ConvertAndPopulateParameter(parameter As DbParameter, value() As Byte)
        Dim sqlVarBinaryParameter As SqlParameter = CType(parameter, SqlParameter)
        sqlVarBinaryParameter.SqlDbType = SqlDbType.VarBinary
        sqlVarBinaryParameter.Value = value
    End Sub

    'Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    'End Sub

    Protected Sub RootSystems_SelectedIndexChanged(sender As Object, e As EventArgs)
        SqlDataSource1.ConnectionString = Conn.ConnectionString
        SqlDataSource1.SelectCommand = "select *,str(Mainsys)+'-'+subsysno as subsysvalue from SUBSYSTEMS where " _
        & "SysType=" & RootSystems.Value & " And Branch='" & IIf(BranchesD.Value Is Nothing, Session("Branch"), BranchesD.Value) & "' Order By MainSys"
        Dim unused = SqlDataSource1.Select(DataSourceSelectArguments.Empty)
        'CheckBoxList1.Items.Clear()
        'CheckBoxList1.DataBind()
        SystemsList.Items.Clear()
        SystemsList.DataBind()
        If Request("log") = "" Then
        Else
            Dim dbadapter = New SqlDataAdapter("select * from AccountFile where AccountNo=" & Request("log") & " And Branch='" & Request("Br") & "'", Conn)
            dbadapter.Fill(userRec)
        End If

        'On Error Resume Next
        If userRec.Tables.Count = 0 Then

            Dim SysS1 As String = RootSystems.SelectedIndex
            Select Case SysS1
                Case 0 : Systems = ""
                Case 1 : Systems = ""
                Case 2 : Systems = ""
                Case 3 : Systems = ""
                Case 4 : Systems = ""
                Case 5 : Systems = ""
                Case Else
                    Exit Select
            End Select
        Else
            Dim SysS1 As String = RootSystems.SelectedIndex
            Select Case SysS1
                Case 0 : Systems = userRec.Tables(0).Rows(0)("AccountPermSys")
                Case 1 : Systems = userRec.Tables(0).Rows(0)("AccountPermClm")
                Case 2 : Systems = userRec.Tables(0).Rows(0)("AccountPermFin")
                Case 3 : Systems = userRec.Tables(0).Rows(0)("AccountPermRe")
                Case 4 : Systems = userRec.Tables(0).Rows(0)("AccountPermMan")
                Case 5 : Systems = userRec.Tables(0).Rows(0)("AccountSysManag")
                Case Else
                    Exit Select
            End Select
        End If

        'Select Case RootSystems.SelectedIndex
        '    Case 0 : Systems = IIf(Request("log") = "", "", userRec.Tables(0).Rows(0)("AccountPermsys"))
        '    Case 1 : Systems = IIf(Request("log") = "", "", userRec.Tables(0).Rows(0)("AccountPermClm"))
        '    Case 2 : Systems = IIf(Request("log") = "", "", userRec.Tables(0).Rows(0)("AccountPermFin"))
        '    Case 3 : Systems = IIf(Request("log") = "", "", userRec.Tables(0).Rows(0)("AccountPermRe"))
        '    Case 4 : Systems = IIf(Request("log") = "", "", userRec.Tables(0).Rows(0)("AccountPermMan"))
        '    Case 5 : Systems = IIf(Request("log") = "", "", userRec.Tables(0).Rows(0)("AccountSysManag"))
        'End Select

        Dim i As Integer = 1
        Dim j As Integer = 1

        While i < Len(Systems)
            j = InStr(i, Systems, ";")
            Dim PermChar As String = Mid(Systems, i, j - 1)
            For Ci As Integer = 0 To SystemsList.Items.Count - 1
                If InStr(Systems, Trim(SystemsList.Items(Ci).Value.ToString.Trim)) <> 0 Then
                    SystemsList.Items(Ci).Selected = True
                Else
                    SystemsList.Items(Ci).Selected = False
                End If
            Next
            i = j + 1
        End While
        SystemsList.DataBind()
    End Sub

    Private Sub Signature_Init(sender As Object, e As EventArgs) Handles Signature.Init
        Dim dv As DataView = DirectCast(SignDs.Select(DataSourceSelectArguments.Empty), DataView)
        If dv.Table.Rows.Count > 0 AndAlso dv.Table.Rows(0)(0) IsNot DBNull.Value Then
            Signature.ContentBytes = CType(dv.Table.Rows(0)(0), Byte())
        End If
    End Sub

    Protected Sub SignDs_Updating(sender As Object, e As SqlDataSourceCommandEventArgs)
        ConvertAndPopulateParameter(e.Command.Parameters(1), Signature.ContentBytes)
    End Sub

End Class