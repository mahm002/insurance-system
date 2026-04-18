Imports System.Data.SqlClient
Imports System.Drawing
Imports DevExpress.Web

Public Class UpdateAccount
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
        ASPxLabel2.Text = " رقم الحساب : " + Request("AccountNo")

        Dim dbadapter As New SqlDataAdapter("select ParentAcc, AccountNo from Accounts where AccountNo='" & Request("AccountNo") & "'", Conn)
        dbadapter.Fill(AcRec)
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
            ASPxLabel2.Text = Getaccname(Request("AccountNo"))

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
        Dim AcRec As New DataSet
        Select Case e.Parameter
            Case "Save"
                If isValid Then
                    Dim dbadapter = New SqlDataAdapter("select * from Accounts where AccountNo='" & ASPxTextBox2.Text & "'", Conn)
                    dbadapter.Fill(AcRec)
                    'If Right(ASPxTextBox4.Text, 1) = "-" Then
                    '    ASPxLabel1.Visible = True
                    '    ASPxLabel1.Text = "خطأ في رقم الحساب"
                    '    ASPxLabel1.ForeColor = Color.Red
                    '    Exit Sub
                    'End If

                    'If AcRec.Tables(0).Rows.Count = 0 And ASPxTextBox2.Text.IndexOf(" ") < 0 And ASPxTextBox2.Text.Trim <> Request("AccountNo") And ASPxTextBox4.Text.IndexOf("_") < 0 Then
                    ExecConn("Update Accounts set AccountName='" & AccountName.Text.Trim & "' where AccountNo='" & Request("AccountNo") & "'", Conn)
                    btnShow.Visible = False
                    ASPxButton2.Visible = True
                    ASPxLabel1.Visible = True
                    ASPxLabel1.Text = "تم تعديل الحساب"
                    ASPxLabel1.ForeColor = Color.Green
                    'Else
                    '    ASPxLabel1.Visible = True
                    '    ASPxLabel1.Text = "خطأ في رقم الحساب"
                    '    ASPxLabel1.ForeColor = Color.Red
                    'End If
                Else
                End If

            Case Else
                Exit Select
        End Select

    End Sub

End Class