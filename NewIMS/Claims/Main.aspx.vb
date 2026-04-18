Imports DevExpress.Web

Partial Class PolicyManagement_Main
    Inherits System.Web.UI.Page
    Dim Lo() As String
    Dim syst As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Lo = Me.Session("LogInfo")
        If IsNothing(Lo) Then
            Me.ClientScript.RegisterStartupScript(Me.GetType, "Msg", "alert('لم يتم تسجيل الدخول');", True)
            Response.Write("<script> window.open('../SystemManag/LogIn.aspx?ReturnUrl=" & Me.Page.AppRelativeVirtualPath & "','_self'); </script>")
        Else
            Call SetUserPermNAV(ASPxNavBar1, Lo, 2)
        End If
        If Not IsPostBack And Not IsNothing(Lo) Then
            Branch.Text = Lo(7)
            'SqlDataSource1.SelectParameters(0).DefaultValue = Branch.Text
            SqlDataSource1.SelectParameters(0).DefaultValue = Sys.Text
            ASPxDateEdit1.Date = Today.Date
            SqlDataSource1.DataBind()
            ASPxGridView1.GroupBy(ASPxGridView1.Columns("BranchName"))

        End If
    End Sub
    Protected Sub Search(ByVal source As Object, ByVal e As ListEditItemsRequestedByFilterConditionEventArgs)
        Dim comboBox As ASPxComboBox = CType(source, ASPxComboBox)
        SqlDataSource2.SelectCommand = "Select CustName,PolNo,BudyNo,TableNo" _
                        & " From (Select CustName,PolNo,BudyNo,TableNo,row_number()over(order by [CustName]) as [rn] From PolicyFile as st " _
                        & " Inner join CustomerFile on st.CustNo=CustomerFile.CustNo " _
                        & " Left outer join MotorFile  on st.OrderNo=MotorFile.OrderNo " _
                        & " where st.SubIns='" & Session("LocalSys") & "' and st.Branch='" & Branch.Text & "'" _
                        & " and (CustName Like @filter or PolNo like @filter or BudyNo like @filter or TableNo like @filter))as t" _
                        & " where t.[rn] between @startIndex and @endIndex"
        SqlDataSource2.SelectParameters.Clear()
        SqlDataSource2.SelectParameters.Add("filter", TypeCode.String, String.Format("%{0}%", e.Filter))
        SqlDataSource2.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString())
        SqlDataSource2.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString())
        comboBox.DataSource = SqlDataSource2
        comboBox.DataBind()
    End Sub
    Protected Sub ASPxComboBox_OnItemRequestedByValue_SQL(ByVal source As Object, ByVal e As ListEditItemRequestedByValueEventArgs)

    End Sub
    Protected Sub grid_HtmlRowPrepared(ByVal sender As Object, ByVal e As ASPxGridViewTableDataCellEventArgs)
        'If (e.DataColumn.FieldName = "net") Then
        'If e.GetValue("net") <> e.GetValue("value") Then
        'e.Cell.ForeColor = Drawing.Color.Red
        'Else
        'e.Cell.ForeColor = Drawing.Color.Green
        'End If
        'Dim ClaimRow As DevExpress.Web.Rendering.GridViewTableDataRow = e.Cell.Parent
        'End If
        If (e.DataColumn.FieldName = "net") And Not IsDBNull(e.GetValue("net")) Then
            If e.GetValue("Payed") = 0 Then
                e.Cell.BackColor = Drawing.Color.OrangeRed
            End If
        End If
        If (e.DataColumn.FieldName = "value") Then
            If e.GetValue("Outstanding") = 0 Then
                e.Cell.BackColor = Drawing.Color.OrangeRed
            End If
        End If
        If (e.DataColumn.FieldName = "PolPath") Then
            Dim CalimMenue As ASPxMenu = ASPxGridView1.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "CMenu")
            'If IsDBNull(e.GetValue("IssuDate")) Then
            'CalimMenue.Items(0).Items.Add("إصدار وثيقة")
            'CalimMenue.Items(0).Items.Add("الغاء الطلب")
            'CalimMenue.Items(0).Items.Add("تحرير الوثيقة")
            'CalimMenue.Items(0).Items.Add("أمر توريد")
            'Else
            'CalimMenue.Items(0).Items.Add("تحرير الوثيقة")
            'CalimMenue.Items(0).Items.Add("عمليات خاصة")
            CalimMenue.Items(0).Items.Add("توزيع الحادث")
            'End If
        End If
    End Sub
    Protected Sub RowPrepare(ByVal sender As Object, ByVal e As ASPxGridViewTableRowEventArgs)
        If e.RowType = GridViewRowType.Group Then
            e.Row.BackColor = Drawing.Color.LightSteelBlue
            e.Row.Font.Name = "Arial"
            e.Row.Font.Bold = True
        End If

    End Sub
    Protected Sub Initiate_Menu(ByVal sender As Object, ByVal e As EventArgs)
        Dim ClaimMenu As ASPxMenu = CType(sender, ASPxMenu)
        ClaimMenu.Items(0).Items.Clear()
    End Sub

    Protected Sub ASPxGridView1_AfterPerformCallback(ByVal sender As Object, ByVal e As ASPxGridViewAfterPerformCallbackEventArgs) Handles ASPxGridView1.AfterPerformCallback

        SqlDataSource1.SelectParameters(0).DefaultValue = Branch.Text
        If e.Args.Length = 1 Then
            If e.Args.Length > 0 Then
                Sys.Text = e.Args(0)
                SqlDataSource1.SelectParameters(0).DefaultValue = Sys.Text
                Session("LocalSys") = Sys.Text
            End If
        Else
            If Not IsNothing(Session("LocalSys")) Then
                Sys.Text = Session("LocalSys")
                SqlDataSource1.SelectParameters(0).DefaultValue = Sys.Text
            End If
        End If
        SqlDataSource1.DataBind()
        ASPxGridView1.DataBind()
        ASPxGridView1.ExpandAll()

    End Sub
End Class
