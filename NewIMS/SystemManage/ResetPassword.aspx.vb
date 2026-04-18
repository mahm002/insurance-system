Imports System.Data.SqlClient
Imports DevExpress.Web

Public Class ResetPassword
    Inherits System.Web.UI.Page
    Private userRec, LogName As New DataSet

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request("log") <> "" Then
                Dim dbadapter = New SqlDataAdapter("select * from AccountFile where AccountNo=" & Request("log") & " And Branch='" & Request("Br") & "'", Conn)
                Dim unused1 = dbadapter.Fill(userRec)
                AccountName.Value = userRec.Tables(0).Rows(0)("AccountName").trim
                AccountName.ClientEnabled = False
                'PassWord.Value = userRec.Tables(0).Rows(0)("AccountPassWord").trim
                'MaxIssu.Text = userRec.Tables(0).Rows(0)("AccLimit")
                'BranchesDD.ClearSelection()
                'BranchesDD.SelectedValue = Request("Br")
                'BranchesDD.Enabled = False
                'BranchesD.SelectedIndex = -1
                'BranchesD.Value = Request("Br")
                'BranchesD.Enabled = False
            End If
        End If
    End Sub

    Protected Sub Callback_Callback1(sender As Object, e As CallbackEventArgsBase)
        Dim callbackPanel As ASPxCallbackPanel = DirectCast(sender, ASPxCallbackPanel)
        Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)
        Select Case e.Parameter
            Case "Apply"
                If PassWord.Text.ToString.Trim <> "" Then
                    If PassWord.Text.ToString.Trim <> ConfirmPass.Text.ToString.Trim Then
                        lblMessage.Text = "THE PASSWORD AND THE CONFIRMATION PASSWORD NOT MATCHING."
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If

                If Request("log") <> "" Then
                    Dim hashedPassword As String = BCrypt.Net.BCrypt.HashPassword(PassWord.Value.ToString.Trim)
                    '& "AccountPassWord='" & hashedPassword & "'," _
                    ExecSql("update AccountFile set AccountPassWord='" & hashedPassword & "',ModifiedBy='" & Session("User") & "',ModifyDate='" & Now.ToString("yyyy/MM/dd HH:mm:ss").ToString & "' where AccountNo=" & Request("log") & " and Branch='" & Request("Br") & "'")
                    lblMessage.Text = "Reset Password Succeed"
                Else

                End If

            Case Else
                Exit Select
        End Select
    End Sub

End Class