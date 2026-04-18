Imports System.Data.SqlClient
Imports System.Drawing
Imports DevExpress.Web

Public Class NewAccount
    Inherits Page

    Private splt As String = ""
    Private AcRec As New DataSet

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If AccountLevel(Request("Parent")) + 1 <= 3 Then
            'TransOrParent.Enabled = False
            'TransOrParent.Value = 0
        Else
            If AccountLevel(Request("Parent")) + 1 >= 4 Then
                'TransOrParent.Enabled = False
                'TransOrParent.Value = 0
            Else
                'TransOrParent.Enabled = False
            End If
        End If

        'splt = ""
        'Dim strarr() As Char = Request("Parent").ToCharArray
        ''strarr = .ToCharArray
        ''Dim splt = ""
        'For Each s As String In strarr
        '    splt = splt + "\" + s + "" 'MessageBox.Show(s)
        'Next

        ParentAcc.Text = Request("Parent") + " / " + Getaccname(Request("Parent"))
        ParentAcc.Text = ParentAcc.Text.Replace(".", "")

        AccountNo.Text = GetLastAccount(Request("Parent")).ToString
        AccountNo.Enabled = False
        ASPxLabel2.Text = Request("Parent") + "." + AccountNo.Text.Trim
        ASPxLabel2.Text = " رقم الحساب الجديد : " + ASPxLabel2.Text.Replace(".", "")

        'If IsPostBack Then
        '    Dim MainAcc As Boolean = IsTransAcc.Checked
        '    'IIf(Len(Request("Parent")) <= 7, IsParentAcc.Visible = False, IsParentAcc.Visible = True)
        '    If Len(Request("Parent")) < 4 Then
        '        ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">a"
        '        ASPxTextBox2.MaskSettings.PromptChar = "_"
        '    Else
        '        Select Case MainAcc
        '            Case True
        '                Select Case Len(Request("Parent"))
        '                    Case 4
        '                        ASPxTextBox2.Text = Request("AccountNo")
        '                        ASPxTextBox2.Caption = "حساب حركة رقم"
        '                        ASPxTextBox3.Caption = "اسم حساب الحركة"
        '                        ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">a"
        '                        ASPxTextBox2.MaskSettings.PromptChar = "_"
        '                    Case 5
        '                        ASPxTextBox2.Text = Request("AccountNo")
        '                        ASPxTextBox2.Caption = "حساب حركة رقم"
        '                        ASPxTextBox3.Caption = "اسم حساب الحركة"
        '                        ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">a"
        '                        ASPxTextBox2.MaskSettings.PromptChar = "_"
        '                    Case 6
        '                        ASPxTextBox2.Text = Request("AccountNo")
        '                        ASPxTextBox2.Caption = "حساب حركة رقم"
        '                        ASPxTextBox3.Caption = "اسم حساب الحركة"
        '                        ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">a"
        '                        ASPxTextBox2.MaskSettings.PromptChar = "_"
        '                    Case 8
        '                        ASPxTextBox2.Caption = "حساب رئيسي رقم"
        '                        ASPxTextBox3.Caption = "اسم الحساب الرئيسي"
        '                        ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">a"
        '                        ASPxTextBox2.MaskSettings.PromptChar = "_"
        '                End Select
        '                'If Len(Request("Parent")) <= 6 Then
        '                '    ASPxTextBox2.Caption = "حساب رئيسي رقم"
        '                '    ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">-a"
        '                '    ASPxTextBox2.MaskSettings.PromptChar = "_"
        '                'Else
        '                '    ASPxTextBox2.Caption = "حساب حركة رقم"
        '                '    ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">aaaa"
        '                '    ASPxTextBox2.MaskSettings.PromptChar = "_"
        '                'End If
        '            Case Else
        '                Select Case Len(Request("Parent"))
        '                    Case 4
        '                        ASPxTextBox2.Text = Request("AccountNo")
        '                        ASPxTextBox2.Caption = "حساب حركة رقم"
        '                        ASPxTextBox3.Caption = "اسم حساب الحركة"
        '                        ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">aaaaaaa"
        '                        ASPxTextBox2.MaskSettings.PromptChar = "_"
        '                    Case 5
        '                        ASPxTextBox2.Text = Request("AccountNo")
        '                        ASPxTextBox2.Caption = "حساب حركة رقم"
        '                        ASPxTextBox3.Caption = "اسم حساب الحركة"
        '                        ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">aaaaaa"
        '                        ASPxTextBox2.MaskSettings.PromptChar = "_"
        '                    Case 6
        '                        ASPxTextBox2.Text = Request("AccountNo")
        '                        ASPxTextBox2.Caption = "حساب حركة رقم"
        '                        ASPxTextBox3.Caption = "اسم حساب الحركة"
        '                        ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">aaaaa"
        '                        ASPxTextBox2.MaskSettings.PromptChar = "_"
        '                    Case 8
        '                        ASPxTextBox2.Caption = "حساب حركة رقم"
        '                        ASPxTextBox3.Caption = "اسم حساب الحركة"
        '                        ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">aaaa"
        '                        ASPxTextBox2.MaskSettings.PromptChar = "_"
        '                End Select

        '        End Select

        '    End If
        'Else
        '    'IIf(Len(Request("Parent")) <= 7, IsParentAcc.Visible = False, IsParentAcc.Visible = True)
        '    If Len(Request("Parent")) < 4 Then
        '        ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">a"
        '        ASPxTextBox2.MaskSettings.PromptChar = "_"
        '    Else
        '        Select Case Len(Request("Parent"))
        '            Case 4
        '                ASPxTextBox2.Text = Request("AccountNo")
        '                ASPxTextBox2.Caption = "حساب حركة رقم"
        '                ASPxTextBox3.Caption = "اسم حساب الحركة"
        '                ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">aaaaaaa"
        '                ASPxTextBox2.MaskSettings.PromptChar = "_"
        '            Case 5
        '                ASPxTextBox2.Text = Request("AccountNo")
        '                ASPxTextBox2.Caption = "حساب حركة رقم"
        '                ASPxTextBox3.Caption = "اسم حساب الحركة"
        '                ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">aaaaaa"
        '                ASPxTextBox2.MaskSettings.PromptChar = "_"
        '            Case 6
        '                ASPxTextBox2.Text = Request("AccountNo")
        '                ASPxTextBox2.Caption = "حساب حركة رقم"
        '                ASPxTextBox3.Caption = "اسم حساب الحركة"
        '                ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">aaaaa"
        '                ASPxTextBox2.MaskSettings.PromptChar = "_"
        '            Case 8
        '                ASPxTextBox2.Text = Request("AccountNo")
        '                ASPxTextBox2.Caption = "حساب حركة رقم"
        '                ASPxTextBox3.Caption = "اسم حساب الحركة"
        '                ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">aaaa"
        '                ASPxTextBox2.MaskSettings.PromptChar = "_"
        '            Case 9
        '                ASPxTextBox2.Text = Request("AccountNo")
        '                ASPxTextBox2.Caption = "حساب حركة رقم"
        '                ASPxTextBox3.Caption = "اسم حساب الحركة"
        '                ASPxTextBox2.MaskSettings.Mask = "<" + splt + ">" + ">aaa"
        '                ASPxTextBox2.MaskSettings.PromptChar = "_"
        '        End Select

        '    End If
        'End If
        'AccountNo.MaskSettings.ErrorText = "تأكد من إدخال رقم الحساب كاملاً"
        'AccountNo.ValidationSettings.RegularExpression.ValidationExpression = "^[A-Z0-9 \\-]+"

    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase)
        Dim callbackPanel As ASPxCallbackPanel = CType(sender, ASPxCallbackPanel)
        Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)

        Select Case e.Parameter
            Case "Save"
                If isValid Then
                    Dim dbadapter = New SqlDataAdapter("select * from Accounts where AccountNo='" & (Request("Parent") + "." + AccountNo.Text.Trim) & "'", Conn)
                    dbadapter.Fill(AcRec)
                    'If Right(ASPxTextBox4.Text, 1) = "-" Then
                    '    ASPxLabel1.Visible = True
                    '    ASPxLabel1.Text = "خطأ في رقم الحساب"
                    '    ASPxLabel1.ForeColor = Color.Red
                    '    Exit Sub
                    'End If
                    'AccountNo.Text.Trim <> Request("Parent") And
                    If AcRec.Tables(0).Rows.Count = 0 And AccountNo.Text.IndexOf(" ") < 0 And ASPxTextBox4.Text.IndexOf("_") < 0 Then
                        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                            If con.State = ConnectionState.Open Then
                                con.Close()
                            Else

                            End If
                            con.Open()
                            ExecConn("insert into Accounts (AccountNo, ParentAcc, AccountName) values('" _
                                         & Request("Parent") + "." + AccountNo.Text.Trim & "','" _
                                         & Request("Parent") & "','" _
                                         & AccountName.Text.Trim & "')", con)
                            btnShow.Visible = False
                            ASPxButton2.Visible = True
                            ASPxLabel1.Visible = True
                            ASPxLabel1.Text = "تم إضافة الحساب"
                            ASPxLabel1.ForeColor = Color.Green
                            con.Close()
                        End Using
                    Else
                        ASPxLabel1.Visible = True
                        ASPxLabel1.Text = "خطأ في رقم الحساب"
                        ASPxLabel1.ForeColor = Color.Red
                    End If
                Else
                End If

            Case Else
                Exit Select
        End Select

    End Sub

End Class