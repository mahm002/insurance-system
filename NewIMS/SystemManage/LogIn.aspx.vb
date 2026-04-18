Imports System.Data.SqlClient
Imports DevExpress.Web

Public Class LogIn
    Inherits Page
    Private Logintems As New List(Of String)

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Request("CMD") = "logoff" Then
            FormsAuthentication.SignOut()
            FormsAuthentication.RedirectToLoginPage()
            Response.Redirect("~/Default.aspx")
            Logintems.Clear()
            Session.RemoveAll()
            Session.Timeout() = 0
        End If
        'If HttpContext.Current.User.Identity.IsAuthenticated AndAlso HttpContext.Current.User IsNot Nothing Then
        '    Dim myList = CType(Session("UserInfo"), List(Of String))
        '    If myList IsNot Nothing Then
        '        'UpdateUserMenuItemsVisible()
        '        'UpdateUserInfo()
        '    Else
        '        ' Session expired - sign out and redirect
        '        FormsAuthentication.SignOut()
        '        RedirectToLoginPage() ' Use centralized redirect helper
        '    End If
        'Else
        '    RedirectToLoginPage() ' User not authenticated
        'End If
        'Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
        '    conn.Open()

        '    ' قراءة كلمات المرور الحالية
        '    Using cmd As New SqlCommand("SELECT AccountNo, AccountPassWordOld FROM AccountFile", conn)
        '        Using reader As SqlDataReader = cmd.ExecuteReader()
        '            While reader.Read()
        '                Dim accountNo As Integer = reader("AccountNo")
        '                Dim password As String = reader("AccountPassWordOld").ToString().Trim()

        '                ' تشفير كلمة المرور

        '                Dim hashedPassword As String = BCrypt.Net.BCrypt.HashPassword(password)

        '                ' تحديث السجل بالهاش الجديد
        '                Using updateCmd As New SqlCommand("UPDATE AccountFile SET AccountPassWord=@Hash WHERE AccountNo=@Id", conn)
        '                    updateCmd.Parameters.AddWithValue("@Hash", hashedPassword)
        '                    updateCmd.Parameters.AddWithValue("@Id", accountNo)
        '                    updateCmd.ExecuteNonQuery()
        '                End Using
        '            End While
        '        End Using
        '    End Using
        'End Using
    End Sub

    Private Sub RedirectToLoginPage()
        If Page.IsCallback Then
            ' Handle callback redirect (DevExpress method)
            Dim loginUrl As String = FormsAuthentication.LoginUrl
            loginUrl = VirtualPathUtility.ToAbsolute(loginUrl)
            ASPxWebControl.RedirectOnCallback(loginUrl)
        Else
            ' Standard redirect
            FormsAuthentication.RedirectToLoginPage()
        End If
    End Sub

    Protected Sub Loginbtn_Click(sender As Object, e As EventArgs) Handles Loginbtn.Click
        'Dim cszList = New List(Of String)()
        Session.Clear()
        Dim LData As New DataSet()
        'load the List one time to be used thru out the intire application
        Dim ConnString = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString
        Using oCon As New SqlConnection(ConnString)
            If oCon.State = ConnectionState.Open Then
                oCon.Close()
            Else

            End If
            oCon.Open()
            Using cmd As New SqlCommand With {
                       .CommandText = "LogIn",
                       .CommandType = CommandType.StoredProcedure
                   }
                cmd.Parameters.AddWithValue("@User", usr.Value.ToString.Trim) 'make sure you assign a value To UserName
                'Dim unused1 = cmd.Parameters.AddWithValue("@Pass", pwd.Value.ToString) 'make sure you assign a value To PassWord
                cmd.Connection = oCon

                Using Adpt As New SqlDataAdapter(cmd)
                    Adpt.Fill(LData)
                End Using
                'If we get a record back from the above stored procedure call, that in itself means the information the user provided from
                'the UI is in the database. On the other hand, if we do not get a record back from the stored procedure call, we should
                'simply advise the user that the information they provided does not exist in the database, and to double check their spelling.
                If LData.Tables.Count = 0 OrElse (LData.Tables.Count > 0 AndAlso LData.Tables(0).Rows.Count = 0) Then
                    'FormsAuthentication.SetAuthCookie(usr.Value.ToString, True)
                    ClientScript.RegisterStartupScript(Me.GetType, "Msg", "alert('لم يتم تسجيل الدخول تأكد من اسم المستخدم وكلمة المرور');", True)
                    oCon.Close()
                    'FormsAuthentication.RedirectFromLoginPage(usr.Value.ToString.Trim, False)
                Else
                    Dim storedHash As String = LData.Tables(0).Rows(0)("AccountPassWord").ToString().Trim
                    Dim inputPassword As String = pwd.Value.ToString.Trim

                    ' التحقق من كلمة المرور باستخدام BCrypt
                    If BCrypt.Net.BCrypt.Verify(inputPassword, storedHash) Then
                        FormsAuthentication.SetAuthCookie(LData.Tables(0).Rows.Item(0).Item("AccountPermSys").ToString, False)

                        'ClientScript.RegisterStartupScript(Me.GetType, "Msg", "alert('مرحباً بك " & LData.Tables(0).Rows.Item(0).Item("AccountLogIn") & "');", True)

                        'FormsAuthentication.SetAuthCookie(Request("usr"), False)
                        Logintems.Clear()

                        Dim LogInfo(11) As String
                        CurUser = Trim(Request("usr"))
                        LogInfo(0) = LData.Tables(0).Rows.Item(0)("AccountLogIn")
                        Logintems.Add(LData.Tables(0).Rows.Item(0)("AccountLogIn"))

                        'نظام الوثائق 1
                        LogInfo(1) = IIf(LData.Tables(0).Rows(0).IsNull("AccountPermSys"), "", LData.Tables(0).Rows(0)("AccountPermSys"))
                        Logintems.Add(IIf(LData.Tables(0).Rows(0).IsNull("AccountPermSys"), "", LData.Tables(0).Rows(0)("AccountPermSys")))
                        'نظام التعويضات والمطالبات 2
                        LogInfo(2) = IIf(LData.Tables(0).Rows(0).IsNull("AccountPermClm"), "", LData.Tables(0).Rows(0)("AccountPermClm"))
                        Logintems.Add(IIf(LData.Tables(0).Rows(0).IsNull("AccountPermClm"), "", LData.Tables(0).Rows(0)("AccountPermClm")))
                        '3 الأقساط والحسابات
                        LogInfo(3) = IIf(LData.Tables(0).Rows(0).IsNull("AccountPermFin"), "", LData.Tables(0).Rows(0)("AccountPermFin"))
                        Logintems.Add(IIf(LData.Tables(0).Rows(0).IsNull("AccountPermFin"), "", LData.Tables(0).Rows(0)("AccountPermFin")))
                        '4 نظام الإعادة
                        LogInfo(4) = IIf(LData.Tables(0).Rows(0).IsNull("AccountPermRe"), "", LData.Tables(0).Rows(0)("AccountPermRe"))
                        Logintems.Add(IIf(LData.Tables(0).Rows(0).IsNull("AccountPermRe"), "", LData.Tables(0).Rows(0)("AccountPermRe")))
                        'الأنظمة الإدارية 5
                        LogInfo(5) = IIf(LData.Tables(0).Rows(0).IsNull("AccountPermMan"), "", LData.Tables(0).Rows(0)("AccountPermMan"))
                        Logintems.Add(IIf(LData.Tables(0).Rows(0).IsNull("AccountPermMan"), "", LData.Tables(0).Rows(0)("AccountPermMan")))
                        'إدارة النظام 6
                        LogInfo(6) = IIf(LData.Tables(0).Rows(0).IsNull("AccountSysManag"), "", LData.Tables(0).Rows(0)("AccountSysManag"))
                        Logintems.Add(IIf(LData.Tables(0).Rows(0).IsNull("AccountSysManag"), "", LData.Tables(0).Rows(0)("AccountSysManag")))
                        'الفرع
                        LogInfo(7) = IIf(LData.Tables(0).Rows(0).IsNull("Branch"), "", LData.Tables(0).Rows(0)("Branch"))
                        Logintems.Add(IIf(LData.Tables(0).Rows(0).IsNull("Branch"), "", LData.Tables(0).Rows(0)("Branch")))
                        'اسم الحساب
                        LogInfo(8) = IIf(LData.Tables(0).Rows(0).IsNull("AccountName"), "", LData.Tables(0).Rows(0)("AccountName"))
                        Logintems.Add(IIf(LData.Tables(0).Rows(0).IsNull("AccountName"), "", LData.Tables(0).Rows(0)("AccountName")))
                        'ref No.
                        LogInfo(9) = LData.Tables(0).Rows(0)("AccountNo")
                        Logintems.Add(LData.Tables(0).Rows(0)("AccountNo"))
                        'Account Limit.
                        LogInfo(10) = LData.Tables(0).Rows(0)("AccLimit")
                        Logintems.Add(LData.Tables(0).Rows(0)("AccLimit"))

                        LogInfo(11) = LData.Tables(0).Rows(0)("AccountLogIn")
                        Logintems.Add(LData.Tables(0).Rows(0)("AccountLogIn"))

                        Session.Add("LogInfo", LogInfo)
                        Session.Add("UserInfo", Logintems)
                        Session.Add("Branch", Logintems.Item(7))
                        Session.Add("User", Logintems.Item(8))
                        Session.Add("UserId", Logintems.Item(9))
                        Session.Add("UserLogin", Logintems.Item(11))
                        Session.Add("BranchAcc", GetBranchAcc(Logintems.Item(7)))

                        Session.Timeout() = 60

                        oCon.Close()

                        If Not String.IsNullOrEmpty(Request.QueryString("ReturnUrl")) Then
                            Response.Redirect("~/Default.aspx") ' Fallback to default page
                            'Dim physicalPath = Server.MapPath(Request.QueryString("ReturnUrl"))

                            'If IO.File.Exists(physicalPath) Then
                            '    Response.Redirect(Request.QueryString("ReturnUrl"))
                            'Else
                            '    Response.Redirect("~/Default.aspx") ' Fallback to default page
                            'End If
                            'Response.Redirect(Request.QueryString("ReturnUrl"))
                        Else
                            Response.Redirect("~/Default.aspx")
                            'FormsAuthentication.RedirectFromLoginPage(Login1.UserName, Login1.RememberMeSet)
                        End If
                    Else
                        ClientScript.RegisterStartupScript([GetType], "Msg", "alert('لم يتم تسجيل الدخول تأكد من اسم المستخدم وكلمة المرور');", True)
                    End If
                    'ClientScript.RegisterStartupScript(Me.GetType, "Msg", "alert(' المستخدم عير موحود أو تم إبقافه ');", True)
                End If
            End Using
        End Using

        'Return cszList
        'Using Ocon As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)

        '    Ocon.Open()
        '    Dim cmd As New SqlCommand With {
        '        .CommandText = "LogInT",
        '        .CommandType = CommandType.StoredProcedure
        '    }

        '    'cmd.Parameters.Add("@User", SqlDbType.Char, 17, "AccountLogIn")
        '    'cmd.Parameters.Add("@Pass", SqlDbType.Char, 14, "AccountPassWord")

        '    cmd.Parameters.AddWithValue("@User", SqlDbType.NVarChar).Value = usr.ToString.Trim
        '    cmd.Parameters.AddWithValue("@Pass", SqlDbType.NVarChar).Value = pwd.ToString.Trim
        '    cmd.Connection = Ocon

        '    Dim reader As SqlDataReader
        '    Dim dataReader As IDataReader
        '    Dim dataRecords As IDataRecord()
        '    'Dim myReader As SqlDataReader = cmd.ExecuteReader()

        '    Try

        '        Ocon.Open()
        '        reader = cmd.ExecuteDataTable()

        '    Finally

        '        Ocon.Dispose()

        '    'If myReader.HasRows Then

        '    'End If

        '    'If LReader.HasRows Then
        '    '    Dim logtbl As New DataTable

        '    '    logtbl.Load(LReader)

        '    '    For Each row As DataRow In logtbl.Rows
        '    '        'Sms = "السادة /" & row.Item("CustName").trim & "" & vbCrLf & "تبلغكم شركة تيبستي للتأمين عن قرب انتهاء تغطية وثيقتكم وذلك بتاريخ " & Format(row.Item("CoverTo"), "yyyy/MM/dd").ToString & " " & vbCrLf & "يمكنكم زيارة أقرب فرع أو وكيل لتجديد التغطية" & vbCrLf & "معاً لمستقبل آمن"

        '    '        'Call SendWelcomeSMS(row.Item("TelNo"), Sms)
        '    '    Next row
        '    'Else

        '    'End If
        '    'Dim da As New SqlDataAdapter(cmd)

        '    'Dim ds As New DataSet("AccountFile")

        '    'If oCon.State = ConnectionState.Closed Then
        '    '    oCon.Open()
        '    'End If

        '    ''da.Fill(ds)
        '    'If Dtbl.Tables(0).Rows.Count <> 1 Then
        '    '    FormsAuthentication.SetAuthCookie(Request("usr"), True)

        '    '    ClientScript.RegisterStartupScript(Me.GetType, "Msg", "alert('لم يتم تسجيل الدخول تأكد من اسم المستخدم وكلمة المرور');", True)
        '    '    oCon.Close()
        '    'Else

        '    '    '    FormsAuthentication.SetAuthCookie(Request("usr"), True)
        '    '    '    Logintems.Clear()

        '    '    '    Dim LogInfo(10) As String
        '    '    '    CurUser = Trim(Request("usr"))
        '    '    '    LogInfo(0) = ds.Tables(0).Rows.Item(0)("AccountLogIn")
        '    '    '    Logintems.Add(ds.Tables(0).Rows.Item(0)("AccountLogIn"))

        '    '    '    'نظام الوثائق 1
        '    '    '    LogInfo(1) = IIf(ds.Tables(0).Rows(0).IsNull("AccountPermSys"), "", ds.Tables(0).Rows(0)("AccountPermSys"))
        '    '    '    Logintems.Add(IIf(ds.Tables(0).Rows(0).IsNull("AccountPermSys"), "", ds.Tables(0).Rows(0)("AccountPermSys")))
        '    '    '    'نظام التعويضات والمطالبات 2
        '    '    '    LogInfo(2) = IIf(ds.Tables(0).Rows(0).IsNull("AccountPermClm"), "", ds.Tables(0).Rows(0)("AccountPermClm"))
        '    '    '    Logintems.Add(IIf(ds.Tables(0).Rows(0).IsNull("AccountPermClm"), "", ds.Tables(0).Rows(0)("AccountPermClm")))
        '    '    '    '3 الأقساط والحسابات
        '    '    '    LogInfo(3) = IIf(ds.Tables(0).Rows(0).IsNull("AccountPermFin"), "", ds.Tables(0).Rows(0)("AccountPermFin"))
        '    '    '    Logintems.Add(IIf(ds.Tables(0).Rows(0).IsNull("AccountPermFin"), "", ds.Tables(0).Rows(0)("AccountPermFin")))
        '    '    '    '4 نظام الإعادة
        '    '    '    LogInfo(4) = IIf(ds.Tables(0).Rows(0).IsNull("AccountPermRe"), "", ds.Tables(0).Rows(0)("AccountPermRe"))
        '    '    '    Logintems.Add(IIf(ds.Tables(0).Rows(0).IsNull("AccountPermRe"), "", ds.Tables(0).Rows(0)("AccountPermRe")))
        '    '    '    'الأنظمة الإدارية 5
        '    '    '    LogInfo(5) = IIf(ds.Tables(0).Rows(0).IsNull("AccountPermMan"), "", ds.Tables(0).Rows(0)("AccountPermMan"))
        '    '    '    Logintems.Add(IIf(ds.Tables(0).Rows(0).IsNull("AccountPermMan"), "", ds.Tables(0).Rows(0)("AccountPermMan")))
        '    '    '    'إدارة النظام 6
        '    '    '    LogInfo(6) = IIf(ds.Tables(0).Rows(0).IsNull("AccountSysManag"), "", ds.Tables(0).Rows(0)("AccountSysManag"))
        '    '    '    Logintems.Add(IIf(ds.Tables(0).Rows(0).IsNull("AccountSysManag"), "", ds.Tables(0).Rows(0)("AccountSysManag")))
        '    '    '    'الفرع
        '    '    '    LogInfo(7) = IIf(ds.Tables(0).Rows(0).IsNull("Branch"), "", ds.Tables(0).Rows(0)("Branch"))
        '    '    '    Logintems.Add(IIf(ds.Tables(0).Rows(0).IsNull("Branch"), "", ds.Tables(0).Rows(0)("Branch")))
        '    '    '    'اسم الحساب
        '    '    '    LogInfo(8) = IIf(ds.Tables(0).Rows(0).IsNull("AccountName"), "", ds.Tables(0).Rows(0)("AccountName"))
        '    '    '    Logintems.Add(IIf(ds.Tables(0).Rows(0).IsNull("AccountName"), "", ds.Tables(0).Rows(0)("AccountName")))
        '    '    '    'ref No.
        '    '    '    LogInfo(9) = ds.Tables(0).Rows(0)("AccountNo")
        '    '    '    Logintems.Add(ds.Tables(0).Rows(0)("AccountNo"))
        '    '    '    'Account Limit.
        '    '    '    LogInfo(10) = ds.Tables(0).Rows(0)("AccLimit")
        '    '    '    Logintems.Add(ds.Tables(0).Rows(0)("AccLimit"))

        '    '    '    Session.Add("LogInfo", LogInfo)
        '    '    '    Session.Add("UserInfo", Logintems)
        '    '    '    Session.Add("Branch", Logintems.Item(7))
        '    '    '    Session.Add("User", Logintems.Item(8))
        '    '    '    Session.Add("UserID", Logintems.Item(9))
        '    '    '    Session.Add("BranchAcc", GetBranchAcc(Logintems.Item(7)))

        '    '    '    'Dim ReturnUrl As String = Convert.ToString(Request.QueryString("ReturnUrl"))

        '    '    '    If Not String.IsNullOrEmpty(Request.QueryString("ReturnUrl")) Then
        '    '    '        FormsAuthentication.SetAuthCookie(Request("usr"), False)

        '    '    '        Response.Redirect(Request.QueryString("ReturnUrl"))
        '    '    '    Else
        '    '    '        FormsAuthentication.RedirectFromLoginPage(Request("usr"), False)
        '    '    '    End If
        '    '    '    If (oCon.State = ConnectionState.Open) Then oCon.Close()

        '    '    'End If

        '    '    If oCon.State = ConnectionState.Open Then oCon.Close()

        'End Using

    End Sub

End Class