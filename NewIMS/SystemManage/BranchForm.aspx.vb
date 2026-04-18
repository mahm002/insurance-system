Imports System.Data.Common
Imports System.Data.SqlClient
Imports DevExpress.Web

Public Class BranchForm
    Inherits Page
    Private Systems As String
    Private InsSys As String
    Private ClmSys As String
    Private ReSys As String
    Private userRec, BrRec As New DataSet

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Request("Br") <> "" And Not IsPostBack Then
            'btnShow.Text = "حفظ"
            LogoDs_Init(sender, e)
            Dim dbadapter = New SqlDataAdapter("select * from BranchInfo where BranchNo='" & Request("Br") & "'", Conn)
            dbadapter.Fill(userRec)
            'userRec = RecSet("select * from BranchInfo where BranchNo='" & Request("Br") & "'", Conn)
            BranchName.Text = userRec.Tables(0).Rows(0)("BranchName")
            BranchNo.Text = userRec.Tables(0).Rows(0)("BranchNo")
            BranchNo.ReadOnly = True
            Address.Text = IIf(IsDBNull(userRec.Tables(0).Rows(0)("Address")), "", userRec.Tables(0).Rows(0)("Address"))
            Telephone.Text = IIf(IsDBNull(userRec.Tables(0).Rows(0)("Telephone")), "", userRec.Tables(0).Rows(0)("Telephone"))
            FaxNo.Text = IIf(IsDBNull(userRec.Tables(0).Rows(0)("FaxNo")), "", userRec.Tables(0).Rows(0)("FaxNo"))
            eMail.Text = IIf(IsDBNull(userRec.Tables(0).Rows(0)("eMail")), "", userRec.Tables(0).Rows(0)("eMail"))
            'Logo.ContentBytes = IIf(IsDBNull(userRec.Tables(0).Rows(0)("Logo")), "", CType(userRec.Tables(0).Rows(0)("Logo"), Byte))
            'MaxIssu.Text = userRec.Tables(0).Rows(0)("AccLimit")
            BranchNo.Enabled = False
            BranchesDD.ClearSelection()
            BranchesDD.SelectedValue = Request("Br")
        End If
        If Not IsPostBack Then

        End If
    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback
        Dim callbackPanel As ASPxCallbackPanel = DirectCast(sender, ASPxCallbackPanel)
        Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)

        Select Case e.Parameter
            Case "Apply"

                If isValid Then
                    syss.Text = ""

                    If Request("Br") Is Nothing Then
                        Dim dbadapter = New SqlDataAdapter("select * from BranchInfo where BranchNo='" & BranchNo.Text & "' and agent=0", Conn)
                        dbadapter.Fill(BrRec)

                        If BrRec.Tables(0).Rows.Count = 0 And Len(BranchNo.Text.TrimStart) = 5 And BranchNo.Text.Trim.IndexOf(" ") < 0 Then

                            'If Len(BranchNo.Text) = 4 Then
                            'If Not CheckBox.Checked Then

                            ExecSql("insert into BranchInfo (BranchName,BranchNo,Address,telephone,FaxNo,eMail,agent,AccountingCode) values('" _
                                     & BranchName.Value & "','" _
                                     & BranchNo.Value & "','" _
                                     & Address.Value & "','" _
                                     & Telephone.Value & "','" _
                                     & FaxNo.Value & "','" & eMail.Value & "'," & 0 & "," & GetNewBranchAcc() & ")")

                            ExecSql("INSERT INTO SUBSYSTEMS " _
                                & "(SUBSYSNO, SUBSYSCODE, Branch, MAINSYS, SUBSYSNAME, SUBSYSNAMEL, LastPolNo, LastOrderNo, LastClaimNo, PolIss, EndIss, PolSTM, EndSTM, Groups, PolReport, SumInsured, ForURL, EditForm, SysType, Grouped)" _
                                & "SELECT SUBSYSNO, SUBSYSCODE, '" & BranchNo.Value & "', MAINSYS, SUBSYSNAME, SUBSYSNAMEL, 0, 0, 0, PolIss, EndIss, PolSTM, EndSTM, Groups, PolReport, SumInsured, ForURL, EditForm, SysType, Grouped " _
                                & "FROM SUBSYSTEMS where Branch=[dbo].[MainCenter]() And MainSys<>97 And SUBSYSNO<>'S2' And SUBSYSNO<>'S5' And SUBSYSNO<>'F8'")

                            ExecSql("INSERT INTO AccountFile " _
                                & "(AccountLogin, Branch, AccountName, AccountPassWord, AccountPermSys, AccountPermClm, AccountPermFin, AccountPermMan, AccountSysManag,AccLimit)" _
                                & "SELECT '" & Trim(BranchNo.Value) & "', '" & Trim(BranchNo.Value) & "','branchAdmin','" & BCrypt.Net.BCrypt.HashPassword(Trim(BranchNo.Value)) & "', AccountPermSys, AccountPermClm, AccountPermFin, " _
                                & "AccountPermMan,'" & "96-S1-5;96-S3-5;" & "', AccLimit FROM AccountFile where AccountLogin='administrator'")
                            'MsgBox("")
                            'Me.ClientScript.RegisterStartupScript(Me.GetType, "Msg", "alert('نفسه كلمة المرور'" & BranchNo.Text & "' للتحكم في الأنظمة وصلاحيات المستخدمين يمكنك الدخول بالمستخدم');", True)
                            'Response.Write("<script>alert('للتحكم في الأنظمة وصلاحيات المستخدمين استعمل المستخدم " + BranchNo.Text + " وتكون كلمة المرور نفس الاسم""');</script>")
                            'MsgBox1.alert("للتحكم في الأنظمة وصلاحيات المستخدمين استعمل المستخدم" + BranchNo.Text + " وتكون كلمة المرور نفس الاسم")
                            'ClientScript.RegisterStartupScript([GetType], "key", "window.close('_this');", True)
                            ASPxWebControl.RedirectOnCallback("~/SystemManage/BranchForm.aspx?Br=" + Trim(BranchNo.Value).ToString.Trim + "")
                        Else
                            'ExecSql("update BranchInfo set " _
                            '    & "BranchName='" & BranchName.Text & "'," _
                            '    & "Address='" & Address.Text & "'," _
                            '    & "telephone='" & Telephone.Text & "'," _
                            '    & "FaxNo='" & FaxNo.Text & "'," _
                            '    & "eMail='" & eMail.Text & "', " _
                            '    & "Agent=0 where BranchNo='" & Request("Br") & "'")
                            'LogoDs.Update()
                            'Response.Write("<script>alert('يرجى التأكد من رمز الفرع');</script>")
                            'MsgBox1.alert("يرجى التأكد من رمز الفرع")
                            BranchNo.BackColor = Drawing.Color.PaleVioletRed
                            BranchNo.Focus()
                            Exit Sub
                        End If
                    Else
                        If Len(BranchNo.Text.TrimStart) = 5 And BranchNo.Text.Trim.IndexOf(" ") < 0 Then
                            ExecConn("update BranchInfo set " _
                             & "BranchName='" & BranchName.Value & "'," _
                             & "Address='" & Address.Value & "'," _
                             & "telephone='" & Telephone.Value & "'," _
                             & "FaxNo='" & FaxNo.Value & "'," _
                             & "eMail='" & eMail.Value & "' where BranchNo='" & Request("Br") & "'", Conn)
                            LogoDs.Update()
                            ClientScript.RegisterStartupScript([GetType], "key", "window.close('_this');", True)
                        Else
                            'Response.Write("<script>alert('يرجى التأكد من رمز الفرع');</script>")
                            'MsgBox1.alert("يرجى التأكد من رمز الفرع")
                            BranchNo.BackColor = Drawing.Color.PaleVioletRed
                            BranchNo.Focus()
                            Exit Sub
                        End If
                    End If
                Else

                End If

            Case Else
                Exit Select
        End Select
    End Sub

    Private Sub LogoDs_Init(sender As Object, e As EventArgs) Handles LogoDs.Init
        Dim dv As DataView = DirectCast(LogoDs.Select(DataSourceSelectArguments.Empty), DataView)
        If dv.Table.Rows.Count > 0 AndAlso dv.Table.Rows(0)(0) IsNot DBNull.Value Then
            Logo.ContentBytes = CType(dv.Table.Rows(0)(0), Byte())
        End If
    End Sub

    Protected Sub LogoDs_Updating(sender As Object, e As SqlDataSourceCommandEventArgs)
        ConvertAndPopulateParameter(e.Command.Parameters(1), Logo.ContentBytes)
    End Sub

    Protected Sub LogoDs_Inserting(sender As Object, e As SqlDataSourceCommandEventArgs)

    End Sub

    Private Sub ConvertAndPopulateParameter(parameter As DbParameter, value() As Byte)
        Dim sqlVarBinaryParameter As SqlParameter = CType(parameter, SqlParameter)
        sqlVarBinaryParameter.SqlDbType = SqlDbType.VarBinary
        sqlVarBinaryParameter.Value = value
    End Sub

End Class