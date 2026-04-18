Imports System.Data.SqlClient
Imports DevExpress.Web

Partial Public Class CustomersForm
    Inherits Page

    Private CustRec As New DataSet

    'Dim Lo() As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Lo = Me.Session("LogInfo")
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))

        If Request("Cust") <> "" And Not IsPostBack Then
            'Button1.Text = "Õ›Ÿ"

            Dim dbadapter = New SqlDataAdapter("select * from Customerfile where CustNo=" & Request("Cust"), Conn)
            dbadapter.Fill(CustRec)
            CustNo.Text = Request("Cust")
            CustNo.ReadOnly = True
            'userRec = RecSet("select * from BranchInfo where BranchNo='" & Request("Br") & "'", Conn)
            CustName.Text = Trim(CustRec.Tables(0).Rows(0)("CustName"))
            If CType(Mid(myList(6), InStr(1, myList(6), "S5") + 3, 1), Int16) >= 5 Then
                CustName.Enabled = True
            Else
                CustName.Enabled = False
            End If
            CustNameE.Text = Trim(CustRec.Tables(0).Rows(0)("CustNameE"))
            IDNo.Text = Trim(CustRec.Tables(0).Rows(0)("IDNo"))
            DrCardNo.Text = Trim(CustRec.Tables(0).Rows(0)("DrCardNo"))
            TelNo.Text = Trim(CustRec.Tables(0).Rows(0)("TelNo"))
            FaxNo.Text = Trim(CustRec.Tables(0).Rows(0)("FaxNo"))
            Address.Text = Trim(CustRec.Tables(0).Rows(0)("Address"))
            eMail.Text = Trim(CustRec.Tables(0).Rows(0)("Email"))
            AccNo.Value = CustRec.Tables(0).Rows(0)("AccNo")
            SpCase.SelectedIndex = CustRec.Tables(0).Rows(0)("SpecialCase") - 1
        Else

        End If
        If Not Page.IsPostBack Then CustName.Focus()
        'Dim DB, DB1 As New DataSet

        'Dim dbadapter = New SqlDataAdapter("Select TPName, TPNo from EXTRAINFO where TP='special'", Conn)
        'dbadapter.Fill(DB)
        ''Dim LogInConn1 As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection(ConfigurationManager.AppSettings("AccConn"))

        ''dbadapter = New SqlDataAdapter(String.Format("select ACCNTNAM,ACCNTNUM from ACCNTTAB1 where len(ACCNTNUM)=9 and left(ACCNTNUM,4)='1421' and substring(ACCNTNUM,5,1)='{0}'", Val(Lo(7))), AccConn)
        'dbadapter.Fill(DB1)
        'SpCase.DataSource = DB 'RecSet("select TPName,TPNo from EXTRAINFO where TP='special'", Conn)
        'AccName.DataSource = DB1 'RecSet("select ACCNTNAM,ACCNTNUM from ACCNTTAB1 where len(ACCNTNUM)=9 and left(ACCNTNUM,4)='1421'", AccConn)

        If Not IsNothing(Request("Cust")) Then
            CustNo.Text = Request("Cust")
            '' CustNo_TextChanged(Me, Nothing)
        End If

    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback
        Dim callbackPanel As ASPxCallbackPanel = DirectCast(sender, ASPxCallbackPanel)
        Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)

        Select Case e.Parameter
            Case "Apply"
                If isValid Then
                    If AccNo.SelectedIndex = -1 Then
                        ExecConn("update CustomerFile set " _
                                 & "CustName='" & Trim(CustName.Text) & "'," _
                                 & "CustNameE='" & Trim(CustNameE.Text) & "'," _
                                 & "IDNo='" & Trim(IDNo.Text) & "'," _
                                 & "DrCardNo='" & Trim(DrCardNo.Text) & "'," _
                                 & "TelNo='" & Trim(TelNo.Text) & "'," _
                                 & "FaxNo='" & Trim(FaxNo.Text) & "'," _
                                 & "Address='" & Trim(Address.Text) & "'," _
                                 & "Email='" & Trim(eMail.Text) & "'," _
                                 & "AccNo=" & "NULL" & "," _
                                 & "SpecialCase=" & SpCase.Value & " Where CustNo=" & CustNo.Text & "", Conn)
                    Else
                        ExecConn("update CustomerFile set " _
                            & "CustName='" & Trim(CustName.Text) & "'," _
                            & "CustNameE='" & Trim(CustNameE.Text) & "'," _
                            & "IDNo='" & Trim(IDNo.Text) & "'," _
                            & "DrCardNo='" & Trim(DrCardNo.Text) & "'," _
                            & "TelNo='" & Trim(TelNo.Text) & "'," _
                            & "FaxNo='" & Trim(FaxNo.Text) & "'," _
                            & "Address='" & Trim(Address.Text) & "'," _
                            & "Email='" & Trim(eMail.Text) & "'," _
                            & "AccNo='" & AccNo.Value & "'," _
                            & "SpecialCase=" & SpCase.Value & " Where CustNo=" & CustNo.Text & "", Conn)
                    End If
                    ASPxLabel1.Text = " „  ⁄œ»· »Ì«‰«  «·“»Ê‰"
                    ASPxLabel1.Visible = True
                    btnShow.Enabled = False
                Else

                End If

            Case Else
                Exit Select
        End Select
    End Sub

    'Protected Sub Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Submit.Click

    '    If FindCustomer(CustNo.Text) <> "" Then
    '        ExecSql("update CustomerFile set " _
    '                & "CustName='" & Trim(CustName.Text) & "'," _
    '                & "CustNameE='" & Trim(CustNameE.Text) & "'," _
    '                & "IDNo='" & Trim(IDNo.Text) & "'," _
    '                & "DrCardNo='" & Trim(DrCardNo.Text) & "'," _
    '                & "TelNo='" & Trim(TelNo.Text) & "'," _
    '                & "FaxNo='" & Trim(FaxNo.Text) & "'," _
    '                & "Address='" & Trim(Address.Text) & "'," _
    '                & "Email='" & Trim(Email.Text) & "'," _
    '                & "AccNo='" & AccName.Value & "'," _
    '                & "SpecialCase=" & SpCase.Value _
    '                & " where CustNo=" & CustNo.Text)
    '    Else
    '        If Not IsFind(CustName.Text, 1) And CustName.Text <> "" Then
    '            Parm = Array.CreateInstance(GetType(SqlParameter), 0)
    '            CustNo.Text = CallSP("lastKey", Conn, Parm)
    '            ExecSql("insert into CustomerFile(CustNo,CustName,CustNameE,IDNo,DrCardNo,RecDate,TelNo,FaxNo,Address,Email,AccNo,SpecialCase) values(" _
    '            & Val(CustNo.Text) & ",'" _
    '            & CustName.Text & "','" _
    '            & CustNameE.Text & "','" _
    '            & IDNo.Text & "','" _
    '            & DrCardNo.Text & "'," _
    '            & "CONVERT(DATETIME,'" & Format(Today.Date, "yyyy-MM-dd") & " 00:00:00',102),'" _
    '            & TelNo.Text & "','" _
    '            & FaxNo.Text & "','" _
    '            & Address.Text & "','" _
    '            & Email.Text & "','" _
    '            & AccName.Value & "'," _
    '            & SpCase.Value _
    '            & ")")
    '        End If
    '    End If

    'End Sub

    'Protected Sub CustNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustNo.TextChanged
    '    If IsNumeric(CustNo.Text) Then
    '        Dim CustRec As New DataSet '= RecSet("select * from Customerfile where CustNo=" & CustNo.Text, Conn)
    '        'Dim LogInConn As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection(ConfigurationManager.AppSettings("Conn"))
    '        Dim dbadapter = New SqlDataAdapter("select * from Customerfile where CustNo=" & CustNo.Text, Conn)
    '        dbadapter.Fill(CustRec)
    '        On Error Resume Next
    '        If CustRec.Tables(0).Rows.Count <> 0 Then
    '            CustName.Text = CustRec.Tables(0).Rows(0)("CustName")
    '            ''If Left(Lo(0), 5) <> "admin" Then CustName.ReadOnly = True : CustNameE.ReadOnly = True : CustName.Enabled = False : CustNameE.Enabled = False
    '            CustNameE.Text = CustRec.Tables(0).Rows(0)("CustNameE")
    '            IDNo.Text = CustRec.Tables(0).Rows(0)("IDNo")
    '            DrCardNo.Text = CustRec.Tables(0).Rows(0)("DrCardNo")
    '            TelNo.Text = CustRec.Tables(0).Rows(0)("TelNo")
    '            FaxNo.Text = CustRec.Tables(0).Rows(0)("FaxNo")
    '            Address.Text = CustRec.Tables(0).Rows(0)("Address")
    '            Email.Text = CustRec.Tables(0).Rows(0)("Email")
    '            'AccNo.Text = CustRec.Tables(0).Rows(0)("AccNo")
    '            spCase.Value.Value = FindExtra(CustRec.Tables(0).Rows(0)("SpecialCase"), "special")
    '        End If
    '    Else
    '        Response.Write("<script>alert('ÌÃ» «œŒ«· —ﬁ„ Ê·Ì” Õ—Ê› ')</script>")
    '    End If
    'End Sub

    'Protected Sub AccName_Select(ByVal sender As Object, ByVal e As System.EventArgs) Handles AccName.Select
    '    Dim AccTest As New DataSet
    '    Dim dbadapter = New Data.SqlClient.SqlDataAdapter(String.Format("select * from CustomerFile where AccNo='{0}'", AccName.SelectedRowValues(1)), Conn)
    '    dbadapter.Fill(AccTest)
    '    If AccTest.Tables(0).Rows.Count > 0 Then
    '        Response.Write("<script>alert('—ﬁ„ «·Õ”«» „ÕÃÊ“ ·“»Ê‰ ¬Œ—');</script>")
    '    End If
    'End Sub

    'Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
    '    If Not Request("Parent") Is Nothing Then
    '        Response.Redirect(String.Format("../{0}?sys={1}", Mid(Request("Parent"), 2, Len(Request("Parent")) - 1), Me.Request("sys")))
    '    Else
    '        Response.Redirect(String.Format("../Default.aspx"))

    '    End If
    'End Sub
End Class