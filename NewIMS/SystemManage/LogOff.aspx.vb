Imports System.Data.SqlClient
Imports System.Security
Public Class LogOff
    Inherits System.Web.UI.Page
    Private Logintems As New List(Of String)
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        FormsAuthentication.SignOut()
        'FormsAuthentication.RedirectToLoginPage()
        'Response.Redirect("~/Default.aspx")
        'Logintems.Clear()

    End Sub

    Private Sub Loginbtn_Click(sender As Object, e As EventArgs) Handles Loginbtn.Click
        Dim cmd As New SqlCommand()
        Using oCon As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            cmd.CommandText = "LogIn"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = oCon
            cmd.Parameters.Add(New SqlParameter("@User", SqlDbType.Char, 17, "AccountLogIn"))
            cmd.Parameters.Add(New SqlParameter("@Pass", SqlDbType.Char, 14, "AccountPassWord"))
            'cmd.Parameters.AddWithValue("@Sys", Odbc.OdbcType.NVarChar).Value
            Dim da As New SqlDataAdapter(cmd)

            Dim ds As New DataSet("AccountFile")

            da.SelectCommand.Parameters(0).Value = Request("usr")
            da.SelectCommand.Parameters(1).Value = Request("pwd")

            If oCon.State = ConnectionState.Closed Then
                oCon.Open()
            End If
            da.Fill(ds)

            If ds.Tables(0).Rows.Count <> 1 Then
                FormsAuthentication.SetAuthCookie(Request("usr"), True)
                ClientScript.RegisterStartupScript(Me.GetType, "Msg", "alert('لم يتم تسجيل الدخول تأكد من اسم المستخدم وكلمة المرور');", True)
                oCon.Close()
            Else
                FormsAuthentication.SetAuthCookie(Request("usr"), False)
                Logintems.Clear()

                Dim LogInfo(10) As String
                CurUser = Trim(Request("usr"))
                LogInfo(0) = ds.Tables(0).Rows.Item(0)("AccountLogIn")
                Logintems.Add(ds.Tables(0).Rows.Item(0)("AccountLogIn"))

                'نظام الوثائق 1
                LogInfo(1) = IIf(ds.Tables(0).Rows(0).IsNull("AccountPermSys"), "", ds.Tables(0).Rows(0)("AccountPermSys"))
                Logintems.Add(IIf(ds.Tables(0).Rows(0).IsNull("AccountPermSys"), "", ds.Tables(0).Rows(0)("AccountPermSys")))
                'نظام التعويضات والمطالبات 2
                LogInfo(2) = IIf(ds.Tables(0).Rows(0).IsNull("AccountPermClm"), "", ds.Tables(0).Rows(0)("AccountPermClm"))
                Logintems.Add(IIf(ds.Tables(0).Rows(0).IsNull("AccountPermClm"), "", ds.Tables(0).Rows(0)("AccountPermClm")))
                '3 الأقساط والحسابات 
                LogInfo(3) = IIf(ds.Tables(0).Rows(0).IsNull("AccountPermFin"), "", ds.Tables(0).Rows(0)("AccountPermFin"))
                Logintems.Add(IIf(ds.Tables(0).Rows(0).IsNull("AccountPermFin"), "", ds.Tables(0).Rows(0)("AccountPermFin")))
                '4 نظام الإعادة
                LogInfo(4) = IIf(ds.Tables(0).Rows(0).IsNull("AccountPermRe"), "", ds.Tables(0).Rows(0)("AccountPermRe"))
                Logintems.Add(IIf(ds.Tables(0).Rows(0).IsNull("AccountPermRe"), "", ds.Tables(0).Rows(0)("AccountPermRe")))
                'الأنظمة الإدارية 5 
                LogInfo(5) = IIf(ds.Tables(0).Rows(0).IsNull("AccountPermMan"), "", ds.Tables(0).Rows(0)("AccountPermMan"))
                Logintems.Add(IIf(ds.Tables(0).Rows(0).IsNull("AccountPermMan"), "", ds.Tables(0).Rows(0)("AccountPermMan")))
                'إدارة النظام 6 
                LogInfo(6) = IIf(ds.Tables(0).Rows(0).IsNull("AccountSysManag"), "", ds.Tables(0).Rows(0)("AccountSysManag"))
                Logintems.Add(IIf(ds.Tables(0).Rows(0).IsNull("AccountSysManag"), "", ds.Tables(0).Rows(0)("AccountSysManag")))
                'الفرع
                LogInfo(7) = IIf(ds.Tables(0).Rows(0).IsNull("Branch"), "", ds.Tables(0).Rows(0)("Branch"))
                Logintems.Add(IIf(ds.Tables(0).Rows(0).IsNull("Branch"), "", ds.Tables(0).Rows(0)("Branch")))
                'اسم الحساب
                LogInfo(8) = IIf(ds.Tables(0).Rows(0).IsNull("AccountName"), "", ds.Tables(0).Rows(0)("AccountName"))
                Logintems.Add(IIf(ds.Tables(0).Rows(0).IsNull("AccountName"), "", ds.Tables(0).Rows(0)("AccountName")))
                'ref No.
                LogInfo(9) = ds.Tables(0).Rows(0)("AccountNo")
                Logintems.Add(ds.Tables(0).Rows(0)("AccountNo"))
                'Account Limit.
                LogInfo(10) = ds.Tables(0).Rows(0)("AccLimit")
                Logintems.Add(ds.Tables(0).Rows(0)("AccLimit"))

                Session.Add("LogInfo", LogInfo)
                Session.Add("UserInfo", Logintems)
                Session.Add("Branch", Logintems.Item(7))
                Session.Add("User", Logintems.Item(8))
                Session.Add("UserID", Logintems.Item(9))
                Session.Add("BranchAcc", GetBranchAcc(Logintems.Item(7)))
                Session.Add("FianceStr", Logintems.Item(3))

                Dim ReturnUrl As String = Convert.ToString(Request.QueryString("ReturnUrl"))

                If (ReturnUrl IsNot Nothing) Then
                    Response.Redirect(ReturnUrl)
                Else
                    'Dim stringArray As String() = New String() {LogInfo(1), LogInfo(2), LogInfo(3), LogInfo(4), LogInfo(5), LogInfo(6)}
                    'Dim longest As String = stringArray.OrderByDescending(Function(x) x.Length).FirstOrDefault()
                    'If longest = LogInfo(1) Then Response.Redirect("~/Policy/PolMain.aspx")
                    'If longest = LogInfo(2) Then Response.Redirect("#")
                    'If longest = LogInfo(3) Then Response.Redirect("~/Finance/Coa.aspx")
                    'If longest = LogInfo(4) Then Response.Redirect("#")
                    'If longest = LogInfo(5) Then Response.Redirect("#")
                    'If longest = LogInfo(6) Then Response.Redirect("~/SystemManage/Default.aspx")
                    'Dim Target As String '= IIf(IsNothing(Request("Parent")), "/Default.aspx", Mid(Request("Parent"), 2, Len(Request("Parent")) - 1))
                End If
                'Dim Target As String '= IIf(IsNothing(Request("Parent")), "/Default.aspx", Mid(Request("Parent"), 2, Len(Request("Parent")) - 1))
                If IsNothing(Request("ReturnUrl")) Then
                    'Target = "/Default.aspx"
                Else
                    'Target = Mid(Request("ReturnUrl"), 2, Len(Request("ReturnUrl")) - 1)
                End If
                If (oCon.State = ConnectionState.Open) Then oCon.Close()

            End If


            If oCon.State = ConnectionState.Open Then oCon.Close()

        End Using
    End Sub
End Class