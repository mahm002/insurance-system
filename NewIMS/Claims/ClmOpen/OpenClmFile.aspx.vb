Imports System.Data.SqlClient
Imports System.Web.UI
Imports DevExpress.Web

Partial Public Class OpenClmFile
    Inherits Page

    'Dim Lo() As String
    Private Shared D1, D2 As DateTime

    'Protected Sub page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles form1.Load

    'End Sub
    'Private Sub CheckSelected(Idx As Integer)

    '    Dim Dset, Oset, OldClaim As New DataSet
    '    Dim TempOrderNo As String

    '    PolNo.Text = UCase(PolNo.Text.TrimEnd)
    '    'Dim focusedIndex As Integer 'on the first load it may be -1
    '    'If DetailsGrd.FocusedRowIndex < 0 Then
    '    '    'focusedIndex = 0
    '    'Else
    '    '    focusedIndex = DetailsGrd.FocusedRowIndex
    '    'End If
    '    Dim values As Object() = CType(DetailsGrd.GetRowValues(Idx.ToString(), "CustName", "EndNo", "GroupNo", "IssuDate"), Object())
    '    If values(0) Is Nothing OrElse values(1) Is Nothing Then
    '        Return
    '    End If

    '    CustName.Value = values(0)
    '    EndNo.Value = values(1)
    '    GroupNo.Value = values(2)
    '    IssDate.Value = values(3)
    '    If IsClaimed(Trim(GroupNo.Text), Trim(PolNo.Text), EndNo.Text) Then
    '        Dim dbadapter2 = New SqlDataAdapter("select ClmNo, PolNo, GroupNo, ClmDate from  MainClaimFile " _
    '                                                          & "where PolNo='" & Trim(PolNo.Text) & "' and EndNo=" & EndNo.Text & " And GroupNo=" & GroupNo.Text & " ", Conn)
    '        dbadapter2.Fill(OldClaim)
    '        GridView2.DataSource = OldClaim.Tables(0)
    '        GridView2.DataBind()

    '        ''updPerson.Update()
    '        ''mpePerson.Show()

    '    Else

    '    End If
    '    Dim dbadapter = New SqlDataAdapter("select * from  PolicyFile where PolNo='" & PolNo.Text & "' and EndNo=" & EndNo.Text & " ", Conn)
    '    dbadapter.Fill(Dset)

    '    TempOrderNo = Dset.Tables(0).Rows.Item(0).Item("OrderNo")

    '    If IsGroupedSys(Request("Sys"), GetMainCenter()) Then
    '        Dim dbadapter1 = New SqlDataAdapter("select sum(" & GetEndFile(Request("Sys")) & ") As SumIns from  " & GetGroupFile(Request("Sys")) & " " _
    '                                                              & "where OrderNo='" & TempOrderNo & "' and EndNo=" & EndNo.Text & " And GroupNo=" & GroupNo.Text & " and " & GetGroupFile(Request("Sys")) & ".SubIns='" & Request("Sys") & "'", Conn)
    '        dbadapter1.Fill(Oset)
    '    Else
    '        Dim dbadapter1 = New SqlDataAdapter("select sum(" & GetEndFile(Request("Sys")) & ") As SumIns from  " & GetGroupFile(Request("Sys")) & " " _
    '                  & "where OrderNo='" & TempOrderNo & "' and EndNo=" & EndNo.Text & " and " & GetGroupFile(Request("Sys")) & ".SubIns='" & Request("Sys") & "'", Conn)
    '        dbadapter1.Fill(Oset)
    '    End If

    '    SumIns.Text = Oset.Tables(0).Rows.Item(0).Item("SumIns")
    '    If Request("Sys") = "MC" Or Request("Sys") = "MB" Or Request("Sys") = "MA" Or Request("Sys") = "OC" Or Request("Sys") = "ER" Or Request("Sys") = "CR" Then
    '        ' If GridView1.SelectedRow.Cells(6).Text = GridView1.SelectedRow.Cells(7).Text Then
    '        ClmDate.Focus()
    '        ClmDate.MinDate = DetailsGrd.GetRowValues(Idx.ToString(), "IssuDate")
    '        'InfName.Focus()
    '    Else
    '        ClmDate.MinDate = DetailsGrd.GetRowValues(Idx.ToString(), "CoverFrom")
    '        'cover to
    '        ClmDate.MaxDate = DetailsGrd.GetRowValues(Idx.ToString(), "CoverTo")
    '        ClmDate.Value = DetailsGrd.GetRowValues(Idx.ToString(), "CoverTo")
    '        ClmDate.Focus()
    '    End If
    '    D1 = DetailsGrd.GetRowValues(Idx, "CoverFrom")
    '    D2 = DetailsGrd.GetRowValues(Idx, "CoverTo")
    '    Ret.Text = IIf(Len(DetailsGrd.GetRowValues(Idx.ToString(), "Ret")) = 1, 0, DetailsGrd.GetRowValues(Idx.ToString(), "Ret"))

    'End Sub
    Protected Sub FillClmData(ByVal ClmN As String, ByVal SysNo As String)
        If IsCallback Then Exit Sub
        'Lo = Session("LogInfo")
        SqlDataSource3.ConnectionString = AttConn.ConnectionString
        SqlDataSource3.SelectParameters("ClmNo").DefaultValue = ClmN
        SqlDataSource3.SelectCommandType = SqlDataSourceCommandType.Text
        SqlDataSource3.Select(DataSourceSelectArguments.Empty)
        Gallery.DataBind()

        SqlDataSource4.ConnectionString = Conn.ConnectionString
        'SqlDataSource4.SelectParameters("ClmNo").DefaultValue = ClmN
        SqlDataSource4.SelectCommandType = SqlDataSourceCommandType.Text
        SqlDataSource4.Select(DataSourceSelectArguments.Empty)
        'GridView5.DataBind()
        ASPxGridView1.DataBind()
        'If Request("Sys") = "02" Or Request("Sys") = "03" Then TABLE1.Visible = True

        Dim ClmData As New DataSet
        SqlDataSource1.ConnectionString = Conn.ConnectionString
        ' Dim ConnLocal As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection(ConfigurationManager.AppSettings("Conn"))
        Dim dbadapter = New SqlDataAdapter("SELECT MainClaimFile.*, CustomerFile.CustName " _
        & "FROM  MainClaimFile INNER JOIN PolicyFile ON MainClaimFile.EndNo = PolicyFile.EndNo AND MainClaimFile.PolNo = PolicyFile.PolNo AND " _
        & "MainClaimFile.LoadNo = PolicyFile.LoadNo INNER JOIN CustomerFile ON PolicyFile.CustNo = CustomerFile.CustNo Where MainClaimFile.ClmNo='" & ClmN & "' " _
        & "And MainClaimFile.SubIns='" & SysNo & "'", Conn)
        dbadapter.Fill(ClmData)
        If ClmData.Tables(0).Rows.Count = 0 Then
            Exit Sub
        Else
            SqlDataSource1.SelectParameters("Policy").DefaultValue = Trim(ClmData.Tables(0).Rows(0)("PolNo"))
            SqlDataSource1.SelectParameters("System").DefaultValue = Request("Sys")
            'SqlDataSource2.SelectParameters("Policy").DefaultValue = Trim(ClmData.Tables(0).Rows(0)("PolNo"))

        End If
        Dim Extra As String

        If IsGroupedSys(Request("Sys")) Then
            Extra = " AND " & GetGroupFile(Request("Sys")) & ".GroupNo=MainClaimFile.groupno"
        Else
            Select Case Request("Sys")
                Case "MC", "MB", "MA"
                    Extra = ""
                Case "OC"
                    Extra = " AND " & GetGroupFile(Request("Sys")) & ".loadno=MainClaimFile.loadno"
                Case Else
                    Extra = ""
            End Select

        End If
        SqlDataSource1.SelectCommand = "Select PolicyFile.PolNo, PolicyFile.EndNo, PolicyFile.OrderNo, CustomerFile.CustName, " _
                & " PolicyFile.IssuDate As IssuDate, PolicyFile.CoverFrom As CoverFrom, PolicyFile.CoverTo As CoverTo, " _
                & "" & IIf(IsGroupedSys(Request("Sys")), GetGroupFile(Request("Sys")) & ".GroupNo", "0") & " As GroupNo, " _
                & "" & IIf(GetRet(Request("Sys")) = "0", "0", GetGroupFile(Request("Sys")) & "." & GetRet(Request("Sys"))) & " As Ret," _
                & "" & GetExtraFile(Request("Sys")) & "" _
                & " From PolicyFile LEFT OUTER JOIN CustomerFile ON CustomerFile.CustNo = PolicyFile.CustNo " _
                & " LEFT OUTER JOIN " & GetGroupFile(Request("Sys")) & " ON " & GetGroupFile(Request("Sys")) & ".OrderNo = PolicyFile.OrderNo  " _
                & " left OUTER JOIN MainClaimFile ON " & GetGroupFile(Request("Sys")) & ".EndNo = MainClaimFile.EndNo AND " & GetGroupFile(Request("Sys")) & ".LoadNo = MainClaimFile.LoadNo " _
                & " And " & GetGroupFile(Request("Sys")) & ".SubIns = PolicyFile.SubIns And " & GetGroupFile(Request("Sys")) & ".EndNo = PolicyFile.EndNo " _
                & " WHERE (PolicyFile.PolNo = '" & Trim(ClmData.Tables(0).Rows(0)("PolNo")) & "') AND (PolicyFile.EndNo = " & ClmData.Tables(0).Rows(0)("EndNo") & ") AND (PolicyFile.SubIns='" & Request("Sys") & "' AND (MainClaimFile.ClmNo = '" & Request("ClmNo") & "' " & Extra & "))"

        PolNo.Text = ClmData.Tables(0).Rows(0)("PolNo")
        PolNo.ReadOnly = True
        EndNo.Text = ClmData.Tables(0).Rows(0)("EndNo")
        LoadNo.Text = ClmData.Tables(0).Rows(0)("LoadNo")
        GroupNo.Text = ClmData.Tables(0).Rows(0)("GroupNo")
        IssDate.Value = ClmData.Tables(0).Rows(0)("ClmSysDate")
        ClmDate.Value = ClmData.Tables(0).Rows(0)("ClmDate")
        InfDate.Value = ClmData.Tables(0).Rows(0)("ClmInfDate")
        CustName.Text = ClmData.Tables(0).Rows(0)("CustName")
        ClmNo.Text = ClmData.Tables(0).Rows(0)("ClmNo").ToString.Trim
        InfName.Text = IIf(IsDBNull(ClmData.Tables(0).Rows(0)("InfName")), "", ClmData.Tables(0).Rows(0)("InfName").ToString.Trim)
        PrevName.Text = IIf(IsDBNull(ClmData.Tables(0).Rows(0)("PrevName")), "", ClmData.Tables(0).Rows(0)("PrevName").ToString.Trim)
        ClmPlace.Text = IIf(IsDBNull(ClmData.Tables(0).Rows(0)("ClmPlace")), "", ClmData.Tables(0).Rows(0)("ClmPlace").ToString.Trim)
        ClmReason.Text = IIf(IsDBNull(ClmData.Tables(0).Rows(0)("ClmReason")), "", ClmData.Tables(0).Rows(0)("ClmReason").ToString.Trim)
        DmgDiscription.Text = IIf(IsDBNull(ClmData.Tables(0).Rows(0)("DmgDiscription")), "", Trim(ClmData.Tables(0).Rows(0)("DmgDiscription")).ToString.Trim)
        SumIns.Text = ClmData.Tables(0).Rows(0)("SumIns")
        Ret.Text = ClmData.Tables(0).Rows(0)("Ret")
        Button1.Visible = False
        Button1.Enabled = False
        'Label2.Visible = True
        'Label2.Text = "رقم التسلسلي"
        GroupNo.Visible = True
        DetailsGrd.Visible = True
        DetailsGrd.SettingsBehavior.AllowFocusedRow = False
        DetailsGrd.DataBind()
        If Request("Sys") = "MC" Or Request("Sys") = "MB" Or Request("Sys") = "MA" Or Request("Sys") = "OC" Or Request("Sys") = "ER" Or Request("Sys") = "CR" Then
            ' If GridView1.SelectedRow.Cells(6).Text = GridView1.SelectedRow.Cells(7).Text Then
            ClmDate.Focus()
            ClmDate.MinDate = DetailsGrd.GetRowValues(0, "IssuDate")
            'InfName.Focus()
        Else
            'ClmDate.MinDate = IIf(Request("ClmNo") <> "", ClmData.Tables(0).Rows(0)("ClmDate"), DetailsGrd.GetRowValues(0, "CoverFrom"))
            ClmDate.MinDate = IIf(Request("ClmNo") <> "", DetailsGrd.GetRowValues(0, "CoverFrom"), DetailsGrd.GetRowValues(0, "CoverFrom"))
            'cover to
            'ClmDate.MaxDate = IIf(Request("ClmNo") <> "", ClmData.Tables(0).Rows(0)("ClmDate"), DetailsGrd.GetRowValues(0, "CoverTo")) ' DetailsGrd.GetRowValues(0, "CoverTo")
            ClmDate.MaxDate = IIf(Request("ClmNo") <> "", DetailsGrd.GetRowValues(0, "CoverTo"), DetailsGrd.GetRowValues(0, "CoverTo")) ' DetailsGrd.GetRowValues(0, "CoverTo")
            ClmDate.Value = IIf(Request("ClmNo") <> "", ClmData.Tables(0).Rows(0)("ClmDate"), DetailsGrd.GetRowValues(0, "CoverTo"))
            ClmDate.Focus()
        End If
        ''DetailsGrd.sel
        'ClmDate.MinDate = ClmData.Tables(0).Rows(0)("ClmDate")
        'ClmDate.MaxDate = ClmData.Tables(0).Rows(0)("ClmDate")
    End Sub

    'Protected Sub DetailsGrd_FocusedRowChanged(sender As Object, e As EventArgs)

    '    'Response.Cookies(grid.ID)("FocudedIndex") = grid.FocusedRowIndex.ToString()
    '    'Response.Cookies(grid.ID).Expires = DateTime.Now.AddDays(1.0R)
    '    'If (grid.FocusedRowIndex <> -1) Then
    '    '    ' grid.VisibleRowCount
    '    '    CustName.Text = grid.GetRowValues(grid.FocusedRowIndex.ToString(), "CustName").ToString
    '    '    EndNo.Text = grid.GetRowValues(grid.FocusedRowIndex.ToString(), "EndNo").ToString
    '    '    GroupNo.Text = grid.GetRowValues(grid.FocusedRowIndex.ToString(), "GroupNo").ToString
    '    'End If
    'End Sub
    Private Sub DetailsGrd_DataBound(sender As Object, e As EventArgs) Handles DetailsGrd.DataBound
        Dim grid As ASPxGridView = TryCast(sender, ASPxGridView)
        If Not IsPostBack Then
            If Request.Cookies(grid.ID) IsNot Nothing Then
                ' grid.FocusedRowIndex = Convert.ToInt32(Request.Cookies(grid.ID)("FocudedIndex"))
            End If
        End If

    End Sub

    'Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridView1.SelectedIndexChanged
    '    Dim Dset, Oset, OldClaim As New DataSet
    '    Dim TempOrderNo As String

    '    CustName.Text = GridView1.SelectedRow.Cells(4).Text.Trim
    '    EndNo.Text = GridView1.SelectedRow.Cells(2).Text
    '    GroupNo.Text = GridView1.SelectedRow.Cells(8).Text
    '    'RadDatePicker1.SelectedDate = GridView1.SelectedRow.Cells(5).Text
    '    IssDate.Value = GridView1.SelectedRow.Cells(5).Text
    '    If IsClaimed(Trim(GroupNo.Text), Trim(PolNo.Text)) Then
    '        Dim dbadapter2 = New SqlDataAdapter("select ClmNo, PolNo, GroupNo, ClmDate from  MainClaimFile " _
    '                                                          & "where PolNo='" & PolNo.Text & "' and EndNo=" & EndNo.Text & " And GroupNo=" & GroupNo.Text & " ", Conn)
    '        dbadapter2.Fill(OldClaim)
    '        GridView2.DataSource = OldClaim.Tables(0)
    '        GridView2.DataBind()
    '        'updPerson.Update()
    '        'mpePerson.Show()
    '    Else

    '    End If
    '    Dim dbadapter = New SqlDataAdapter("select * from  PolicyFile where PolNo='" & PolNo.Text & "' and EndNo=" & EndNo.Text & " ", Conn)
    '    dbadapter.Fill(Dset)

    '    TempOrderNo = Dset.Tables(0).Rows.Item(0).Item("OrderNo")

    '    If IsGroupedSys(Request("Sys"), "TT00") Then
    '        Dim dbadapter1 = New SqlDataAdapter("select sum(" & GetEndFile(Request("Sys")) & ") As SumIns from  " & GetGroupFile(Request("Sys")) & " " _
    '                                                              & "where OrderNo='" & TempOrderNo & "' and EndNo=" & EndNo.Text & " And GroupNo=" & GroupNo.Text & " and " & GetGroupFile(Request("Sys")) & ".SubIns='" & Request("Sys") & "'", Conn)
    '        dbadapter1.Fill(Oset)
    '    Else
    '        Dim dbadapter1 = New SqlDataAdapter("select sum(" & GetEndFile(Request("Sys")) & ") As SumIns from  " & GetGroupFile(Request("Sys")) & " " _
    '                  & "where OrderNo='" & TempOrderNo & "' and EndNo=" & EndNo.Text & " and " & GetGroupFile(Request("Sys")) & ".SubIns='" & Request("Sys") & "'", Conn)
    '        dbadapter1.Fill(Oset)
    '    End If

    '    SumIns.Text = Oset.Tables(0).Rows.Item(0).Item("SumIns")
    '    If Request("Sys") = "MC" Or Request("Sys") = "MB" Or Request("Sys") = "MA" Or Request("Sys") = "OC" Or Request("Sys") = "ER" Or Request("Sys") = "CR" Then
    '        ' If GridView1.SelectedRow.Cells(6).Text = GridView1.SelectedRow.Cells(7).Text Then
    '        ClmDate.Focus()
    '        'InfName.Focus()
    '    Else
    '        ClmDate.MinDate = GridView1.SelectedRow.Cells(6).Text
    '        'cover to
    '        ClmDate.MaxDate = GridView1.SelectedRow.Cells(7).Text
    '        ClmDate.Value = GridView1.SelectedRow.Cells(7).Text
    '        ClmDate.Focus()
    '    End If
    '    D1 = GridView1.SelectedRow.Cells(6).Text
    '    D2 = GridView1.SelectedRow.Cells(7).Text
    '    Ret.Text = IIf(GridView1.SelectedRow.Cells(9).Text = "&nbsp;", 0, GridView1.SelectedRow.Cells(9).Text)
    'End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))

        'Dim SS As String
        'Dim Cars, St As String
        'Dim scriptManager_1 As ScriptManager =  ScriptManager.GetCurrent(Page)
        'scriptManager_1.RegisterPostBackControl(Button1)
        'scriptManager_1.RegisterPostBackControl(btnSubmit)
        'scriptManager_1.RegisterPostBackControl(Button12)
        'scriptManager_1.RegisterPostBackControl(ClmNo)
        ' scriptManager_1.RegisterPostBackControl(RadDatePicker1)
        'scriptManager_1.RegisterPostBackControl(RadDatePicker2)
        'Cars = ""
        'St = ""
        ' HyperLink1.NavigateUrl = "~/ClaimsManage/Default.aspx?sys=" & Request("sys")

        If Request("ClmNo") <> "" Then
            Call FillClmData(Request("ClmNo"), Request("Sys"))
            btnSubmit.Visible = True
            imgUpload.Visible = True
            Update.ClientVisible = True
        Else
            Update.ClientVisible = False
            If IsCallback Or IsPostBack Then
                If ClmNo.Text = "" Then
                    SqlDataSource1.SelectCommand = "SELECT PolicyFile.PolNo, PolicyFile.EndNo, PolicyFile.OrderNo, CustomerFile.CustName, " _
                & " PolicyFile.IssuDate As IssuDate, PolicyFile.CoverFrom As CoverFrom, PolicyFile.CoverTo As CoverTo, " _
                & "" & IIf(IsGroupedSys(Request("Sys")), GetGroupFile(Request("Sys")) & ".GroupNo", "0") & " As GroupNo, " _
                & "" & IIf(GetRet(Request("Sys")) = "0", "0", GetGroupFile(Request("Sys")) & "." & GetRet(Request("Sys"))) & " As Ret," _
                & "" & GetExtraFile(Request("Sys")) & "" _
                & " From PolicyFile LEFT OUTER JOIN CustomerFile ON CustomerFile.CustNo = PolicyFile.CustNo " _
                & " LEFT OUTER JOIN " & GetGroupFile(Request("Sys")) & " ON " & GetGroupFile(Request("Sys")) & ".OrderNo = PolicyFile.OrderNo  " _
                & " And " & GetGroupFile(Request("Sys")) & ".SubIns = PolicyFile.SubIns And " & GetGroupFile(Request("Sys")) & ".EndNo = PolicyFile.EndNo " _
                & " WHERE (PolicyFile.PolNo = '" & PolNo.Text.Trim & "')  AND IssuDate IS NOT NULL AND (PolicyFile.SubIns='" & Request("Sys") & "') " & IIf(IsGroupedSys(Request("Sys")), "Order by GroupNo", " ") & ""
                Else
                    Call FillClmData(ClmNo.Text, Request("Sys"))
                    btnSubmit.Visible = True
                    imgUpload.Visible = True
                End If
                Exit Sub
            End If
            IssDate.Value = Today.Date

        End If
        Dim TrNo As Integer
        Dim Tr As New DataSet
        Dim dbadapter = New SqlDataAdapter("Select Count(ClmNo) From ThirdParty where ClmNo='" & Trim(ClmNo.Text) & "' ", Conn)
        dbadapter.Fill(Tr)
        TrNo = Tr.Tables(0).Rows(0)(0)
        If ClmNo.Text = "" Then
            TABLE1.Visible = False
        Else
            TABLE1.Visible = True
            TP.SelectCommand = "SELECT distinct * From ExtraInfo where TP ='ThirdParty'"
        End If
        'If Session("Idx") IsNot Nothing Then
        '    CheckSelected(Session("Idx"))
        'End If
        If IsClosed(Request("ClmNo"), PolNo.Text.Trim) Then
            btnSubmit.Enabled = False
            imgUpload.Enabled = False
            Button12.ClientEnabled = False
            Update.Enabled = False
            'ASPxGridView1.SettingsCommandButton.EditButt = ""
        End If
    End Sub

    'Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
    '    Parm = Array.CreateInstance(GetType(ReportParameter), 1)
    '    SetRepPm("ClaimNo", True, GenArray(GetTextValue(FindControl("ClmNo"))), Parm, 0)
    '    Session.Add("Parms", Parm)
    '    Response.Write("<script> window.open('../Reporting/Previewer.aspx?Report=/IMSReports/PreViewReport','_new'); </script>")

    'End Sub

    Protected Sub BtnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        lblResult.Text = ""
        Dim connection As SqlConnection = Nothing
        Try
            Dim img As FileUpload = imgUpload
            Dim imgByte As Byte() = Nothing
            If img.HasFile AndAlso img.PostedFile IsNot Nothing Then
                'To create a PostedFile
                Dim File As HttpPostedFile = imgUpload.PostedFile
                'Create byte Array with file len
                imgByte = New Byte(File.ContentLength - 1) {}
                'force the control to load data in array
                If File.ContentLength <= 500000 Then
                    File.InputStream.Read(imgByte, 0, File.ContentLength)
                    connection = New SqlConnection(AttConn.ConnectionString)
                    connection.Open()
                Else
                    lblResult.Text = "الحجم يفوق المسموح به حاول ضغط الصورة"
                    connection = New SqlConnection(AttConn.ConnectionString)
                    connection.Open()
                    Exit Try
                End If
                'File.InputStream.Read(imgByte, 0, File.ContentLength)
            End If
            ' Insert the employee name and image into db
            'Dim conn1 As String = ConfigurationManager.ConnectionStrings("AttachDB").ConnectionString
            'connection = New SqlConnection(conn1)

            'connection.Open()
            Dim sql As String = "INSERT INTO ClaimsAttachments(ClmNo,Image,UserName) VALUES(@enm, @eimg,@UserN) SELECT @@IDENTITY"
            Dim cmd As SqlCommand = New SqlCommand(sql, connection)
            cmd.Parameters.AddWithValue("@enm", ClmNo.Text.Trim())
            cmd.Parameters.AddWithValue("@eimg", imgByte)
            cmd.Parameters.AddWithValue("@UserN", Session("User"))
            Dim id As Integer = Convert.ToInt32(cmd.ExecuteScalar())
            'lblResult.Text = String.Format("{0}", id)

            ' Display the image from the database
            'Image1.ImageUrl = "~/Images/ShowImage.ashx?id=" & id
        Catch
            lblResult.Text = "خطأ في التحميل"
        Finally
            connection.Close()
        End Try
        SqlDataSource3.ConnectionString = AttConn.ConnectionString
        SqlDataSource3.SelectParameters("ClmNo").DefaultValue = ClmNo.Text.Trim
        SqlDataSource3.SelectCommandType = SqlDataSourceCommandType.Text
        SqlDataSource3.Select(DataSourceSelectArguments.Empty)
        Gallery.DataBind()
    End Sub

    Protected Sub PolNo_TextChanged(sender As Object, e As EventArgs) Handles PolNo.TextChanged
        Dim St As String = ""
        PolNo.Text = UCase(PolNo.Text.TrimEnd).ToString.TrimEnd
        SqlDataSource1.ConnectionString = Conn.ConnectionString
        SqlDataSource1.SelectParameters("Policy").DefaultValue = UCase(PolNo.Text.TrimEnd).ToString.TrimEnd
        SqlDataSource1.SelectParameters("System").DefaultValue = Request("sys")
        SqlDataSource2.SelectParameters("Policy").DefaultValue = UCase(PolNo.Text.TrimEnd).ToString.TrimEnd
        SqlDataSource2.SelectParameters("System").DefaultValue = Request("sys")
        SqlDataSource3.SelectParameters("ClmNo").DefaultValue = ClmNo.Text.Trim

        SqlDataSource1.SelectCommand = "SELECT PolicyFile.PolNo, PolicyFile.EndNo, PolicyFile.OrderNo, CustomerFile.CustName, " _
                & " PolicyFile.IssuDate As IssuDate, PolicyFile.CoverFrom As CoverFrom,PolicyFile.CoverTo As CoverTo, " _
                & "" & IIf(IsGroupedSys(Request("Sys")), GetGroupFile(Request("Sys")) & ".GroupNo", "0") & " As GroupNo, " _
                & "" & IIf(GetRet(Request("Sys")) = "0", "0", GetGroupFile(Request("Sys")) & "." & GetRet(Request("Sys"))) & " As Ret," _
                & "" & GetExtraFile(Request("Sys")) & "" _
                & " From PolicyFile LEFT OUTER JOIN CustomerFile ON CustomerFile.CustNo = PolicyFile.CustNo " _
                & " LEFT OUTER JOIN " & GetGroupFile(Request("Sys")) & " ON " & GetGroupFile(Request("Sys")) & ".OrderNo = PolicyFile.OrderNo  " _
                & " And " & GetGroupFile(Request("Sys")) & ".SubIns = PolicyFile.SubIns And " & GetGroupFile(Request("Sys")) & ".EndNo = PolicyFile.EndNo " _
                & " WHERE (PolicyFile.PolNo = '" & UCase(PolNo.Text.TrimEnd).ToString.TrimEnd & "') AND PolicyFile.Stop=0 and PolicyFile.NetPRM>=0 AND IssuDate IS NOT NULL AND (PolicyFile.SubIns='" & Request("Sys") & "') " & IIf(IsGroupedSys(Request("Sys")), "Order by GroupNo", " ") & ""
        'DetailsGrd.Visible = True
        'GridView1.Columns("OrderNo").Visible = False
        'DetailsGrd.Columns("OrderNo").Visible = False
        DetailsGrd.DataBind()
        DetailsGrd.Visible = True
        DetailsGrd.DataBind()

        DetailsGrd.FocusedRowIndex = -1
    End Sub

    Private Sub ClmDate_DateChanged(sender As Object, e As EventArgs) Handles ClmDate.DateChanged
        If CDate(ClmDate.Value) < ClmDate.MinDate And CDate(ClmDate.Value) > ClmDate.MaxDate Then
            ClmDate.Value = ClmDate.MaxDate
            MsgBox("تاريخ الحادث ليس ضمن فترة التغطية ")
        Else
            If ClmNo.Text <> "" Then ExecConn("Update MainClaimFile Set ClmDate= CONVERT(DATETIME,'" & Format(CDate(ClmDate.Value), "yyyy/MM/dd") & " 00:00:00',102) WHERE ClmNo='" & Trim(ClmNo.Text) & "'", Conn)
        End If
    End Sub

    'Protected Sub ImagesDataSource_Inserting(sender As Object, e As SqlDataSourceCommandEventArgs)

    '    ConvertAndPopulateParameter(e.Command.Parameters(1), Logo.ContentBytes)

    'End Sub
    'Private Sub ConvertAndPopulateParameter(ByVal parameter As DbParameter, ByVal value() As Byte)
    '    Dim sqlVarBinaryParameter As SqlParameter = CType(parameter, SqlParameter)
    '    sqlVarBinaryParameter.SqlDbType = SqlDbType.VarBinary
    '    sqlVarBinaryParameter.Value = value
    'End Sub

    Protected Sub Cbp_Callback(source As Object, e As CallbackEventArgsBase) Handles Cbp.Callback
        Dim clmtpm As String = ""

        Select Case e.Parameter
            Case "Check"
                Dim Dset, Oset, OldClaim As New DataSet
                Dim TempOrderNo As String

                PolNo.Text = UCase(PolNo.Text.TrimEnd)
                'Dim focusedIndex As Integer 'on the first load it may be -1
                'If DetailsGrd.FocusedRowIndex < 0 Then
                '    'focusedIndex = 0
                'Else
                '    focusedIndex = DetailsGrd.FocusedRowIndex
                'End If
                Dim values As Object() = CType(DetailsGrd.GetRowValues(DetailsGrd.FocusedRowIndex, "CustName", "EndNo", "GroupNo", "IssuDate"), Object())
                If values(0) Is Nothing OrElse values(1) Is Nothing Then
                    Return
                End If

                CustName.Value = Trim(values(0))
                EndNo.Value = values(1)
                GroupNo.Value = values(2)
                IssDate.Value = values(3)

                Dim dbadapter = New SqlDataAdapter("select * from  PolicyFile where PolNo='" & PolNo.Text & "' and EndNo=" & EndNo.Text & " ", Conn)
                dbadapter.Fill(Dset)

                TempOrderNo = Dset.Tables(0).Rows.Item(0).Item("OrderNo")

                If IsGroupedSys(Request("Sys")) Then
                    Dim dbadapter1 = New SqlDataAdapter("select sum(" & GetEndFile(Request("Sys")) & ") As SumIns from  " & GetGroupFile(Request("Sys")) & " " _
                                                                      & "where OrderNo='" & TempOrderNo & "' and EndNo=" & EndNo.Text & " And GroupNo=" & GroupNo.Text & " and " & GetGroupFile(Request("Sys")) & ".SubIns='" & Request("Sys") & "'", Conn)
                    dbadapter1.Fill(Oset)
                Else
                    Dim dbadapter1 = New SqlDataAdapter("select sum(" & GetEndFile(Request("Sys")) & ") As SumIns from  " & GetGroupFile(Request("Sys")) & " " _
                          & "where OrderNo='" & TempOrderNo & "' and EndNo=" & EndNo.Text & " and " & GetGroupFile(Request("Sys")) & ".SubIns='" & Request("Sys") & "'", Conn)
                    dbadapter1.Fill(Oset)
                End If

                SumIns.Text = Oset.Tables(0).Rows.Item(0).Item("SumIns")
                If Request("Sys") = "MC" Or Request("Sys") = "MB" Or Request("Sys") = "MA" Or Request("Sys") = "OC" Or Request("Sys") = "ER" Or Request("Sys") = "CR" Then
                    ' If GridView1.SelectedRow.Cells(6).Text = GridView1.SelectedRow.Cells(7).Text Then
                    ClmDate.Focus()
                    ClmDate.MinDate = DetailsGrd.GetRowValues(DetailsGrd.FocusedRowIndex, "IssuDate")
                    'InfName.Focus()
                Else
                    ClmDate.MinDate = DetailsGrd.GetRowValues(DetailsGrd.FocusedRowIndex, "CoverFrom")
                    'cover to
                    ClmDate.MaxDate = DetailsGrd.GetRowValues(DetailsGrd.FocusedRowIndex, "CoverTo")
                    ClmDate.Value = DetailsGrd.GetRowValues(DetailsGrd.FocusedRowIndex, "CoverTo")
                    ClmDate.Focus()
                End If
                D1 = CDate(DetailsGrd.GetRowValues(DetailsGrd.FocusedRowIndex, "CoverFrom"))
                D2 = CDate(DetailsGrd.GetRowValues(DetailsGrd.FocusedRowIndex, "CoverTo"))
                Ret.Text = IIf(Len(DetailsGrd.GetRowValues(DetailsGrd.FocusedRowIndex, "Ret")) = 1, 0, DetailsGrd.GetRowValues(DetailsGrd.FocusedRowIndex, "Ret"))

                If IsClaimed(Trim(GroupNo.Text), Trim(PolNo.Text), EndNo.Text) Then
                    Dim dbadapter2 = New SqlDataAdapter("select ClmNo, PolNo, GroupNo, ClmDate from  MainClaimFile " _
                                                                  & "where PolNo='" & Trim(PolNo.Text) & "' and EndNo=" & EndNo.Text & " And GroupNo=" & GroupNo.Text & " ", Conn)
                    dbadapter2.Fill(OldClaim)
                    GridView2.DataSource = OldClaim.Tables(0)
                    GridView2.DataBind()

                    Cbp.JSProperties("cpMyAttribute") = "Claimed"
                    Cbp.JSProperties("cpShowClaimedPopup") = True

                    ''updPerson.Update()
                    ''mpePerson.Show()
                Else

                End If
            Case "OpenNewClaim"
                If DetailsGrd.FocusedRowIndex = -1 _
                   Or Not ClmDate.IsValid _
                   Or Not InfDate.IsValid _
                   Or Not InfName.IsValid _
                   Or Not PrevName.IsValid _
                   Or Not ClmPlace.IsValid _
                   Or Not ClmReason.IsValid _
                   Or Not DmgDiscription.IsValid Or Not IsDate(ClmDate.Value) Then

                    Exit Sub
                Else
                    D1 = CDate(DetailsGrd.GetRowValues(DetailsGrd.FocusedRowIndex, "CoverFrom"))
                    D2 = CDate(DetailsGrd.GetRowValues(DetailsGrd.FocusedRowIndex, "CoverTo"))
                    D1 = CDate(DetailsGrd.GetRowValues(DetailsGrd.FocusedRowIndex, "CoverFrom"))
                    D2 = CDate(DetailsGrd.GetRowValues(DetailsGrd.FocusedRowIndex, "CoverTo"))
                    If (ClmDate.Value >= D1 AndAlso ClmDate.Value.Date <= D2 AndAlso ClmDate.Value < Today.Date()) Then
                    Else
                        Exit Sub
                    End If
                    Cbp.JSProperties("cpMyAttribute") = "Issuance"
                    Cbp.JSProperties("cpShowIssueConfirmBox") = True
                End If
                'Cbp.JSProperties("cpMyAttribute") = "Issuance"
                'Cbp.JSProperties("cpShowIssueConfirmBox") = True
            Case "Issue"
                If CDate(ClmDate.Value) > Today.Date Or (CDate(ClmDate.Value) >= D1 And CDate(ClmDate.Value) <= D2) Then
                    If DetailsGrd.FocusedRowIndex = -1 _
        Or Not ClmDate.IsValid _
        Or Not InfDate.IsValid _
        Or Not InfName.IsValid _
        Or Not PrevName.IsValid _
        Or Not ClmPlace.IsValid _
        Or Not ClmReason.IsValid _
        Or Not DmgDiscription.IsValid Then
                        Exit Sub
                    End If
                    If (ClmDate.Value >= D1 AndAlso ClmDate.Value.Date <= D2 AndAlso ClmDate.Value < Today.Date()) Then
                    Else
                        Exit Sub
                    End If

                    ' =============== TRANSACTION START ===============
                    Dim transaction As SqlTransaction = Nothing
                    Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                        If con.State = ConnectionState.Open Then
                            con.Close()
                        Else

                        End If
                        con.Open()
                        Try

                            ' Start transaction
                            transaction = con.BeginTransaction()

                            ' Get next claim number within transaction
                            Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                            SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                            SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 1)
                            clmtpm = CallSPWithTransaction("LastClmNo", con, Parm, transaction).TrimEnd

                            Button1.Enabled = False
                            If SumIns.Value = 0 Then
                                If IsGroupedSys(Request("Sys")) Then
                                    SumIns.Value = GetSumInsG(Request("Sys"), DetailsGrd.GetRowValues(DetailsGrd.FocusedRowIndex, "OrderNo"), EndNo.Value, LoadNo.Value, UCase(PolNo.Value), GroupNo.Text)
                                Else
                                    SumIns.Value = GetSumInsG(Request("Sys"), DetailsGrd.GetRowValues(DetailsGrd.FocusedRowIndex, "OrderNo"), EndNo.Value, LoadNo.Value, UCase(PolNo.Value), 0)
                                End If
                            End If

                            ' Prepare date values
                            Dim claimDate As String
                            If IsDate(tmpdate.Value) Then
                                claimDate = Format(CDate(tmpdate.Value), "yyyy/MM/dd")
                            Else
                                claimDate = Format(ClmDate.Value, "yyyy/MM/dd")
                            End If

                            Dim infoDate As String = Format(CDate(InfDate.Value), "yyyy/MM/dd")

                            ' Insert record within transaction
                            Dim insertSql As String = "INSERT INTO MainClaimFile(ClmNo,PolNo,EndNo,LoadNo,GroupNo,ClmDate,ClmInfDate,InfName,PrevName,ClmPlace,ClmReason,SumIns,DmgDiscription,SubIns,Ret,Branch,UserName) " &
                                        "VALUES(@ClmNo, @PolNo, @EndNo, @LoadNo, @GroupNo, @ClmDate, @ClmInfDate, @InfName, @PrevName, @ClmPlace, @ClmReason, @SumIns, @DmgDiscription, @SubIns, @Ret, @Branch, @UserName)"

                            Using cmd As New SqlCommand(insertSql, con, transaction)
                                cmd.Parameters.AddWithValue("@ClmNo", clmtpm)
                                cmd.Parameters.AddWithValue("@PolNo", UCase(PolNo.Text))
                                cmd.Parameters.AddWithValue("@EndNo", EndNo.Text)
                                cmd.Parameters.AddWithValue("@LoadNo", Val(LoadNo.Text))
                                cmd.Parameters.AddWithValue("@GroupNo", Val(GroupNo.Text))
                                cmd.Parameters.AddWithValue("@ClmDate", claimDate)
                                cmd.Parameters.AddWithValue("@ClmInfDate", infoDate)
                                cmd.Parameters.AddWithValue("@InfName", InfName.Text)
                                cmd.Parameters.AddWithValue("@PrevName", PrevName.Text)
                                cmd.Parameters.AddWithValue("@ClmPlace", ClmPlace.Text)
                                cmd.Parameters.AddWithValue("@ClmReason", ClmReason.Text)
                                cmd.Parameters.AddWithValue("@SumIns", SumIns.Value)
                                cmd.Parameters.AddWithValue("@DmgDiscription", DmgDiscription.Text)
                                cmd.Parameters.AddWithValue("@SubIns", Request("SYS"))
                                cmd.Parameters.AddWithValue("@Ret", Ret.Text)
                                cmd.Parameters.AddWithValue("@Branch", MyBase.Session("Branch"))
                                cmd.Parameters.AddWithValue("@UserName", MyBase.Session("User"))

                                cmd.ExecuteNonQuery()
                            End Using

                            ' Commit transaction
                            transaction.Commit()
                            transaction = Nothing ' Set to null to prevent rollback in Finally block

                            ' =============== TRANSACTION SUCCESS ===============
                            SqlDataSource3.ConnectionString = AttConn.ConnectionString
                            SqlDataSource3.SelectParameters("ClmNo").DefaultValue = clmtpm
                            SqlDataSource3.SelectCommandType = SqlDataSourceCommandType.Text
                            SqlDataSource3.Select(DataSourceSelectArguments.Empty)

                            If Request("Sys") = "02" Or Request("Sys") = "03" Then TABLE1.Visible = True
                            Button1.Enabled = False
                            btnSubmit.Visible = True
                            imgUpload.Visible = True

                            ASPxWebControl.RedirectOnCallback("~/Claims/ClmOpen/OpenClmFile.aspx?Sys=" & Request("Sys") & "&ClmNo=" & clmtpm & "")
                        Catch ex As Exception
                            ' =============== TRANSACTION ERROR ===============
                            If transaction IsNot Nothing Then
                                transaction.Rollback()
                            End If

                            ' Log error (you can implement logging here)
                            ' LogError(ex)

                            ' Show error message to user
                            Response.Write("<script>alert('Error creating claim: " & ex.Message.Replace("'", "") & "');</script>")
                            Exit Sub
                        Finally
                            If transaction IsNot Nothing Then
                                transaction.Dispose()
                            End If
                        End Try
                        con.Close()
                    End Using
                    ' =============== TRANSACTION END ===============
                Else
                    If Request("Sys") = "MC" Or Request("sys") = "MB" Or Request("sys") = "MA" Or Request("sys") = "OC" Or Request("sys") = "ER" Or Request("sys") = "CR" Then
                        ' =============== TRANSACTION START (Second Insert Block) ===============
                        Dim transaction As SqlTransaction = Nothing
                        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                            If con.State = ConnectionState.Open Then
                                con.Close()
                            Else

                            End If
                            con.Open()
                            Try
                                ' Start transaction
                                transaction = con.BeginTransaction()

                                ' Get next claim number within transaction
                                Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                                SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                                SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 1)
                                clmtpm = CallSPWithTransaction("LastClmNo", con, Parm, transaction).TrimEnd

                                Button1.Enabled = False

                                ' Prepare date values for second block
                                Dim claimDate2 As String = Format(CDate(tmpdate.Value), "yyyy/MM/dd")
                                Dim infDate2 As String = Format(CDate(InfDate.Value), "yyyy/MM/dd")

                                ' Insert record within transaction
                                Dim insertSql As String = "INSERT INTO MainClaimFile(ClmNo,PolNo,EndNo,LoadNo,GroupNo,ClmDate,ClmInfDate,InfName,PrevName,ClmPlace,ClmReason,SumIns,DmgDiscription,SubIns,Ret,Branch,UserName) " &
                                        "VALUES(@ClmNo, @PolNo, @EndNo, @LoadNo, @GroupNo, @ClmDate, @ClmInfDate, @InfName, @PrevName, @ClmPlace, @ClmReason, @SumIns, @DmgDiscription, @SubIns, @Ret, @Branch, @UserName)"

                                Using cmd As New SqlCommand(insertSql, con, transaction)
                                    cmd.Parameters.AddWithValue("@ClmNo", clmtpm)
                                    cmd.Parameters.AddWithValue("@PolNo", UCase(PolNo.Text))
                                    cmd.Parameters.AddWithValue("@EndNo", EndNo.Text)
                                    cmd.Parameters.AddWithValue("@LoadNo", Val(LoadNo.Text))
                                    cmd.Parameters.AddWithValue("@GroupNo", Val(GroupNo.Text))
                                    cmd.Parameters.AddWithValue("@ClmDate", claimDate2)
                                    cmd.Parameters.AddWithValue("@ClmInfDate", infDate2)
                                    cmd.Parameters.AddWithValue("@InfName", InfName.Text)
                                    cmd.Parameters.AddWithValue("@PrevName", PrevName.Text)
                                    cmd.Parameters.AddWithValue("@ClmPlace", ClmPlace.Text)
                                    cmd.Parameters.AddWithValue("@ClmReason", ClmReason.Text)
                                    cmd.Parameters.AddWithValue("@SumIns", SumIns.Text)
                                    cmd.Parameters.AddWithValue("@DmgDiscription", DmgDiscription.Text)
                                    cmd.Parameters.AddWithValue("@SubIns", Request("SYS"))
                                    cmd.Parameters.AddWithValue("@Ret", Ret.Text)
                                    cmd.Parameters.AddWithValue("@Branch", Session("Branch"))
                                    cmd.Parameters.AddWithValue("@UserName", Session("User"))

                                    cmd.ExecuteNonQuery()
                                End Using

                                ' Commit transaction
                                transaction.Commit()
                                transaction = Nothing ' Set to null to prevent rollback in Finally block

                                ' =============== TRANSACTION SUCCESS ===============
                                SqlDataSource3.ConnectionString = AttConn.ConnectionString
                                SqlDataSource3.SelectParameters("ClmNo").DefaultValue = clmtpm
                                SqlDataSource3.SelectCommandType = SqlDataSourceCommandType.Text
                                SqlDataSource3.Select(DataSourceSelectArguments.Empty)

                                If Request("Sys") = "02" Or Request("Sys") = "03" Then TABLE1.Visible = True
                                Button1.Enabled = False
                                btnSubmit.Visible = True
                                imgUpload.Visible = True

                                ASPxWebControl.RedirectOnCallback("~/Claims/ClmOpen/OpenClmFile.aspx?Sys=" & Request("Sys") & "&ClmNo=" & clmtpm & "")
                            Catch ex As Exception
                                ' =============== TRANSACTION ERROR ===============
                                If transaction IsNot Nothing Then
                                    transaction.Rollback()
                                End If

                                ' Log error
                                ' LogError(ex)

                                ' Show error message to user
                                Response.Write("<script>alert('Error creating claim: " & ex.Message.Replace("'", "") & "');</script>")
                                Exit Sub
                            Finally
                                If transaction IsNot Nothing Then
                                    transaction.Dispose()
                                End If
                            End Try
                            con.Close()
                        End Using
                        ' =============== TRANSACTION END ===============
                    Else
                        Response.Write("<script>alert('تاريخ الحادث ليس ضمن فترة التغطية');</script>")
                        Exit Sub
                    End If
                End If
            Case "NewParty"
                Dim TrNo, Trno1 As Integer
                Dim Tr, Tr1 As New DataSet
                'Dim ConnLocal As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection(ConfigurationManager.AppSettings("Conn"))
                Dim dbadapter = New SqlDataAdapter("Select max(TPID) From ThirdParty where ClmNo='" & Trim(ClmNo.Text) & "' ", Conn)
                dbadapter.Fill(Tr)
                If IsDBNull(Tr.Tables(0).Rows(0)(0)) Then
                    TrNo = 1
                Else
                    TrNo = Tr.Tables(0).Rows(0)(0) + 1
                End If

                Dim dbadapter1 = New SqlDataAdapter("Select max(Sn) From Estimation where ClmNo='" & Trim(ClmNo.Text) & "' ", Conn)
                dbadapter1.Fill(Tr1)
                If IsDBNull(Tr1.Tables(0).Rows(0)(0)) Then
                    Trno1 = 1
                Else
                    Trno1 = Tr1.Tables(0).Rows(0)(0) + 1
                End If
                If Val(Value.Text) <= 0 Then
                    Response.Write("<script>alert('قيمة الاحتياطي مطلوبة');</script>")
                    Exit Sub
                Else
                    ExecConn("INSERT INTO ThirdParty(TPID,ClmNo,ThirdParty,Asset,Damage,Value) VALUES(" & IIf(TPId.Value = 0, 0, TrNo) & ",'" _
               & ClmNo.Text & "','" _
               & ThirdParty.Text & "','" _
               & Asset.Text & "','" _
               & Damage.Text & "'," _
               & Val(Value.Text) & " )", Conn)

                    ExecConn("INSERT INTO Estimation (Sn,TPID,ClmNo,PolNo,Value,Date) VALUES (" & Trno1 & "," & IIf(TPId.Value = 0, 0, TrNo) & ",'" & ClmNo.Text & "','" & PolNo.Text & "', " & Val(Value.Text) & ",CONVERT(DATETIME,'" & Format(Today.Date, "yyyy/MM/dd") & " 00:00:00',102))", Conn)
                    'ScriptManager.GetCurrent(Me.Page).RegisterPostBackControl(Button11)
                    SqlDataSource4.ConnectionString = Conn.ConnectionString
                    SqlDataSource4.SelectParameters("ClmNo").DefaultValue = ClmNo.Text
                    SqlDataSource4.SelectCommandType = SqlDataSourceCommandType.Text
                    SqlDataSource4.Select(DataSourceSelectArguments.Empty)
                    'GridView5.DataBind()
                    ASPxGridView1.DataBind()
                End If
                ' <> "01" And Request("Sys") <> "02" And Request("Sys") <> "03" And Request("Sys") <> "MD" And Request("Sys") <> "OR"
                If IsReinsSys(Request("Sys")) Then
                    ASPxWebControl.RedirectOnCallback("~/Reins/DestributeClaim.aspx?ClmNo=" & ClmNo.Value & "&EndNo=" & EndNo.Value & "&LoadNo=" & LoadNo.Value & "&OrderNo=" & GetOrderNo(PolNo.Value, EndNo.Value, LoadNo.Value) & "&Sys=" & Request("Sys") & "&Branch=" & Session("Branch") & "&PolNo=" & PolNo.Value & "&GroupNo=" & GroupNo.Value)
                End If

            Case Else
                Exit Select
        End Select
    End Sub

    Public Function CallSPWithTransaction(spName As String, connection As SqlConnection, parameters As SqlParameter(), transaction As SqlTransaction) As String
        Using cmd As New SqlCommand(spName, connection)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Transaction = transaction

            If parameters IsNot Nothing Then
                cmd.Parameters.AddRange(parameters)
            End If

            ' Your existing implementation to execute and return result
            ' For example:
            Dim result As Object = cmd.ExecuteScalar()
            Return If(result IsNot Nothing, result.ToString(), "")
        End Using
    End Function

    Public Sub ExecConnClm(sql As String, connection As SqlConnection, Optional transaction As SqlTransaction = Nothing)
        Using cmd As New SqlCommand(sql, connection)
            If transaction IsNot Nothing Then
                cmd.Transaction = transaction
            End If

            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Protected Sub DetailsGrd_FocusedRowChanged(sender As Object, e As EventArgs) Handles DetailsGrd.FocusedRowChanged

        Dim grid As ASPxGridView = TryCast(sender, ASPxGridView)
        If grid.FocusedRowIndex <> -1 Then
        Else

        End If

    End Sub

    Protected Sub DetailsGrd_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles DetailsGrd.CustomCallback

        'If IsCallback And e.Parameters <> ",,,," Then
        '    Dim Vals = e.Parameters.Split(",")
        '    DetailsGrd.JSProperties("cpCustName") = Trim(Vals(0))
        '    DetailsGrd.JSProperties("cpEndNo") = Vals(1)
        '    DetailsGrd.JSProperties("cpGroupNo") = Vals(2)
        '    DetailsGrd.JSProperties("cpIssuDate") = Vals(3)
        '    DetailsGrd.JSProperties("cpRet") = Vals(4)

        '    Session("Idx") = DetailsGrd.FocusedRowIndex
        '    'Idxs.Value = DetailsGrd.FocusedRowIndex

        '    'If Mid(Trim(Vals(2)), 1, 4) = "1120" And Request("Sys") = 3 Then
        '    '    'DocNum.Visible = True : CustName.Visible = True ': CustName.Text = PayedFor.Text
        '    '    DetailsGrd.JSProperties("cpshow") = "S"
        '    'Else
        '    '    DetailsGrd.JSProperties("cpshow") = "H"
        '    'End If
        '    'Session.Add("Mode", True)
        '    'AccNtnum.Value = Vals(2)
        '    ''AccntNam.Text = GridView1.Rows(GridView1.SelectedIndex).Cells(3).Text
        '    'Debtor.Value = Vals(3)
        '    'Creditor.Value = Vals(4)
        '    ''Label1.Text = GridView1.Rows(GridView1.SelectedIndex).Cells(1).Text
        '    'DocNum.Text = Vals(5)
        'Else
        '    'DetailsGrd.FocusedRowIndex = -1
        'End If
        'End Select
    End Sub
    Protected Sub Cbps_Callback(source As Object, e As CallbackEventArgs)
        Select Case e.Parameter
            Case "UpdateMainData"

                If Not ClmDate.IsValid _
                   Or Not InfDate.IsValid _
                   Or Not InfName.IsValid _
                   Or Not PrevName.IsValid _
                   Or Not ClmPlace.IsValid _
                   Or Not ClmReason.IsValid _
                   Or Not DmgDiscription.IsValid Or Not IsDate(ClmDate.Value) Then

                    Exit Sub
                Else
                    D1 = CDate(DetailsGrd.GetRowValues(0, "CoverFrom"))
                    D2 = CDate(DetailsGrd.GetRowValues(0, "CoverTo"))
                    If (ClmDate.Value >= D1 AndAlso ClmDate.Value.Date <= D2 AndAlso ClmDate.Value < Today.Date()) Then
                    Else
                        Exit Sub
                    End If
                    Dim claimDate2 As String = Format(CDate(ClmDate.Value), "yyyy/MM/dd")

                    ExecSql("Update MainClaimFile Set ClmDate='" & claimDate2 & "', DmgDiscription='" & DmgDiscription.Text.Trim & "', " _
                            & " ClmReason='" & ClmReason.Text.Trim & "', ClmPlace='" & ClmPlace.Text.Trim & "' , " _
                            & " InfName='" & InfName.Text.Trim & "', PrevName='" & PrevName.Text.Trim & "'   " _
                            & " WHERE ClmNo='" & Request("ClmNo") & "'")
                End If
            Case Else
                Exit Select
        End Select

    End Sub
    Protected Sub Cbp_Callback1(sender As Object, e As CallbackEventArgsBase)

    End Sub

    Private Sub DetailsGrd_AfterPerformCallback(sender As Object, e As ASPxGridViewAfterPerformCallbackEventArgs) Handles DetailsGrd.AfterPerformCallback
        ' CheckSelected(Session("Idx"))
    End Sub

End Class