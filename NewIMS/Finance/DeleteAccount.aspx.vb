Imports System.Data.SqlClient
Imports System.Drawing
Imports DevExpress.Web

Public Class DeleteAccount
    Inherits Page

    Private splt As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim AcRec As New DataSet
        splt = ""
        Dim strarr() As Char = Request("AccountNo").ToCharArray
        'strarr = .ToCharArray
        'Dim splt = ""

        'For Each s As String In strarr
        '    splt = splt + "\" + s + "" 'MessageBox.Show(s)
        'Next
        'ASPxLabel2.Text = Request("Parent") + "." + AccountNo.Text.Trim
        'ASPxLabel2.Text = " رقم الحساب : " + Request("AccountNo")

        Dim dbadapter As SqlDataAdapter = New SqlDataAdapter("select ParentAcc, AccountNo from Accounts where AccountNo='" & Request("AccountNo") & "'", Conn)
        dbadapter.Fill(AcRec)

        If AcRec.Tables(0).Rows.Count = 0 Then
            Exit Sub
        End If

        If AcRec.Tables(0).Rows.Item(0).IsNull(0) Then
            ASPxTextBox1.Text = "حساب رئيسي" 'AcRec.Tables(0).Rows(0)("ParentAcc") + " / " + Getaccname(CType(AcRec.Tables(0).Rows(0)("ParentAcc"), String))
        Else
            ASPxTextBox1.Text = AcRec.Tables(0).Rows(0)("ParentAcc") + " / " + Getaccname(CType(AcRec.Tables(0).Rows(0)("ParentAcc"), String))
        End If

        If IsPostBack Then
            'Dim MainAcc As Boolean = False
            ''IIf(Len(Request("AccountNo")) < 8, ASPxCheckBox.Visible = False, ASPxCheckBox.Visible = True)
            'If Len(Request("AccountNo")) < 6 Then
            '    ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">a"
            '    ASPxTextBox2.MaskSettings.PromptChar = "_"
            'Else
            '    Select Case MainAcc
            '        Case True
            '            If Len(Request("AccountNo")) = 6 Then
            '                ASPxTextBox2.Caption = "حساب رئيسي رقم"
            '                ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">-a"
            '                ASPxTextBox2.MaskSettings.PromptChar = "_"
            '            Else
            '                ASPxTextBox2.Caption = "حساب حركة رقم"
            '                ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">-aaaaa"
            '                ASPxTextBox2.MaskSettings.PromptChar = "_"
            '            End If
            '        Case Else
            '            If Len(Request("AccountNo")) = 6 Then
            '                ASPxTextBox2.Caption = "حساب حركة رقم"
            '                ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">-aaaaa"
            '                ASPxTextBox2.MaskSettings.PromptChar = "_"
            '            Else
            '                ASPxTextBox2.Caption = "حساب حركة رقم"
            '                ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">-aaaaa"
            '                ASPxTextBox2.MaskSettings.PromptChar = "_"
            '            End If

            '    End Select

            'End If
        Else
            ASPxLabel2.Text = Getaccname(Request("AccountNo")) & Request("AccountNo")

            'IIf(Len(Request("AccountNo")) < 8, ASPxCheckBox.Visible = False, ASPxCheckBox.Visible = True)
            'If Len(Request("AccountNo")) < 6 Then
            '    ''ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">a"
            '    ''ASPxTextBox2.MaskSettings.PromptChar = "_"
            '    ASPxTextBox2.Text = Request("AccountNo")
            'Else
            '    Select Case Len(Request("AccountNo"))
            '        Case 6
            '            'ASPxTextBox2.Caption = "حساب حركة رقم"
            '            'ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">-aaaaa"
            '            'ASPxTextBox2.MaskSettings.PromptChar = Request("AccountNo")
            '            ASPxTextBox2.Text = Request("AccountNo")
            '        Case > 6
            '            'ASPxTextBox2.Caption = "حساب حركة رقم"
            '            'ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">aaaa"
            '            'ASPxTextBox2.MaskSettings.PromptChar = "_"
            '            ASPxTextBox2.Text = Request("AccountNo")
            '    End Select
            '    'If Len(Request("Parent")) = 8 Then
            '    '    ASPxTextBox2.Caption = "حساب حركة رقم"
            '    '    ASPxTextBox2.MaskSettings.Mask = Request("Parent") + ">aaaa"
            '    '    ASPxTextBox2.MaskSettings.PromptChar = "*"
            '    'Else
            '    '    ASPxTextBox2.Caption = "حساب حركة رقم"
            '    '    ASPxTextBox2.MaskSettings.Mask = Request("Parent") + ">-aaaaa"
            '    '    ASPxTextBox2.MaskSettings.PromptChar = "*"
            '    'End If
            'End If
        End If
        'ASPxTextBox2.MaskSettings.ErrorText = "تأكد من إدخال رقم الحساب كاملاً"
        'ASPxTextBox2.ValidationSettings.RegularExpression.ValidationExpression = "^[A-Z0-9 \\-]+"

    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase)
        Dim callbackPanel As ASPxCallbackPanel = CType(sender, ASPxCallbackPanel)
        Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)

        Select Case e.Parameter
            Case "Delete"
                If isValid Then
                    Try
                        ' Parameterized query
                        Dim sql As String = "DELETE Accounts WHERE AccountNo = @AccountNo AND AccountNo NOT IN (SELECT AccountNo FROM AgentsCommisions) AND AccountNo NOT IN (SELECT AccountNo FROM Journal) AND TranscationAcc=0"

                        ' Create command with parameters
                        Using cmd As New SqlCommand(sql, Conn)
                            ' Use correct parameter type based on your database column type
                            cmd.Parameters.Add("@AccountNo", SqlDbType.VarChar, 50).Value = Request("AccountNo")

                            ' Open connection if not already open
                            If Conn.State <> ConnectionState.Open Then
                                Conn.Open()
                            End If

                            ' Execute the command
                            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                            ' Check if any rows were deleted
                            If rowsAffected > 0 Then
                                btnShow.Visible = False
                                ASPxButton2.Visible = True
                                ASPxLabel1.Visible = True
                                ASPxLabel1.Text = "تم شطب الحساب"
                                ASPxLabel1.ForeColor = Color.Red
                            Else
                                ' No rows deleted - account might be referenced in AgentsCommissions
                                ASPxLabel1.Visible = True
                                ASPxLabel1.Text = "لا يمكن حذف الحساب لأنه مرتبط بعمولات وكلاء"
                                ASPxLabel1.ForeColor = Color.Orange
                            End If
                        End Using

                    Catch sqlEx As SqlException
                        ' Handle SQL Server specific exceptions
                        ASPxLabel1.Visible = True
                        ASPxLabel1.Text = "خطأ في قاعدة البيانات: " & sqlEx.Message
                        ASPxLabel1.ForeColor = Color.Red

                        ' Log the exception (optional)
                        ' LogError(sqlEx)

                    Catch ex As Exception
                        ' Handle general exceptions
                        ASPxLabel1.Visible = True
                        ASPxLabel1.Text = "حدث خطأ: " & ex.Message
                        ASPxLabel1.ForeColor = Color.Red

                        ' Log the exception (optional)
                        ' LogError(ex)
                    Finally
                        ' Ensure connection is closed
                        If Conn.State = ConnectionState.Open Then
                            Conn.Close()
                        End If
                    End Try
                End If
        End Select
    End Sub

End Class