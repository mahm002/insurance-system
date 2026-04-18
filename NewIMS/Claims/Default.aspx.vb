Imports System.Web.UI
Imports DevExpress.Web
Imports Microsoft.Reporting.WebForms

Partial Class ClaimsManage_Default
    Inherits Page

    ReadOnly DataRet As New DataView
    ReadOnly Lo() As String
    Private Menuitems As String
    Private ClmState As Boolean

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))
        If myList Is Nothing Then
            ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('SystemManage/LogIn.aspx','_self'); </script>")
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 2)
        End If
        If IsCallback Then
            'Session("No") = ""
            'Session("Sys") = Quarters.Value()
        Else
            ' Session("Year") = Years.Text
            'Session("Q") = Quarters.Value()
        End If
        MainGrid.DataBind()

    End Sub

    Protected Sub ClaimStatus_Init(sender As Object, e As EventArgs)
        Dim image = TryCast(sender, ASPxImage)
        Dim container = TryCast(image.NamingContainer, GridViewDataItemTemplateContainer)
        Dim index = container.VisibleIndex
        Dim value = CInt(Math.Truncate(container.Grid.GetRowValues(index, "Status")))
        Dim url = "../Content/Images/"
        Dim State = ""
        Select Case value
            Case 1
                url &= "unlocked.png"
                State = "الملف مفتوح"
            Case 2
                url &= "locked.png"
                State = "الملف مقفل"
            Case 3
                url &= "DeleteRed.png"
                State = "الملف مقفل بدون تسوية"

            Case Else

        End Select
        image.ImageUrl = url
        image.ToolTip = State

    End Sub

    Protected Sub MainGrid_HtmlRowPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs)
        MainGrid.GetRowValues(4, "Status")
        'If (e.DataColumn.FieldName = "Status") Then 'And Not IsDBNull(e.GetValue("net")
        '    If e.GetValue("Status") = "الملف مفتوح" Then
        '        e.Cell.BackColor = Drawing.Color.Green
        '    Else
        '        e.Cell.BackColor = Drawing.Color.Red
        '    End If
        'End If
        'If (e.DataColumn.FieldName = "value") Then
        '    If e.GetValue("Outstanding") = 0 Then
        '        e.Cell.BackColor = Drawing.Color.OrangeRed
        '    End If
        'End If
        'If e.GetValue("ClmNo").ToString.Trim = "TIP00230002502" Then
        ClmState = IsClosed(e.GetValue("ClmNo").ToString.Trim)
        'End If

        If (e.DataColumn.FieldName = "") Then
            Dim MClaimsMenu As ASPxMenu = MainGrid.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "MClmMenu")
            'If IsDBNull(e.GetValue("IssuDate")) Then
            'If IsDBNull(e.GetValue("IssuDate")) Then
            If ClmState Then
                'If MainGrid.GetRowValues(2, "ClmNo") = "TIP00230002502" Then

                MClaimsMenu.Items(0).Items.Add("معاينة")
                MClaimsMenu.Items(0).Items.Add("تقرير بيانات الحادث")
            Else
                MClaimsMenu.Items(0).Items.Add("إقفال")
                MClaimsMenu.Items(0).Items.Add("معاينة")
                MClaimsMenu.Items(0).Items.Add("تقرير بيانات الحادث")
            End If
            'MClaimsMenu.Items(0).Items.Add("إقفال")
            'MClaimsMenu.Items(0).Items.Add("معاينة")
            If Sys.Text <> "01" And Sys.Text <> "02" And Sys.Text <> "03" And Sys.Text <> "MD" And Sys.Text <> "OR" Then
                MClaimsMenu.Items(0).Items.Add("توزيع")
            End If
            'MClaimsMenu.Items(0).Items.FindByText("إقفال").NavigateUrl = "~/ClaimsManage/close/CloseClmFile.aspx?ClmNo=" + MainGrid.GetRowValues(1, "ClmNo") + "&Sys=" + Sys.Text + ""
            'Else
            'ClaimsMenu.Items(0).Items.Add("تحرير الوثيقة")
            '        ClaimsMenu.Items(0).Items.Add("عمليات خاصة")
            '        ClaimsMenu.Items(0).Items.Add("توزيع الوثيقة")
            'End If
            'End If
        End If
    End Sub

    Protected Sub GV1_HtmlRowPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs)
        Dim grd As ASPxGridView = TryCast(sender, ASPxGridView)

        If (e.DataColumn.FieldName = "") Then

            ClmState = IsClosed(Session("ClmNo"))

            Dim ClaimsMenu As ASPxMenu = (grd).FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "ClmMenu")
            'If IsDBNull(e.GetValue("IssuDate")) Then
            'If IsDBNull(e.GetValue("IssuDate")) Then
            If ClmState Then
            Else
                ClaimsMenu.Items(0).Items.Add("تقدير احتياطي")
                ClaimsMenu.Items(0).Items.Add("تسوية جديدة")
            End If

            'ClaimsMenu.Items(0).Items.Add("سداد")
            'ClaimsMenu.Items(0).Items.Add("معاينة")
            'Else
            'ClaimsMenu.Items(0).Items.Add("تحرير الوثيقة")
            '        ClaimsMenu.Items(0).Items.Add("عمليات خاصة")
            '        ClaimsMenu.Items(0).Items.Add("توزيع الوثيقة")
            'End If
            'End If
        End If
        'If (e.DataColumn.FieldName = "TOTPRM") Then
        '    If e.GetValue("TOTPRM") <> e.GetValue("InBox") Then
        '        e.Cell.ForeColor = Drawing.Color.Red
        '    Else
        '        e.Cell.ForeColor = Drawing.Color.Green
        '    End If
        '    Dim PolicyRow As DevExpress.Web.ASPxGridView.Rendering.GridViewTableDataRow = e.Cell.Parent
        'End If
        'If (e.DataColumn.FieldName = "NETPRM") Then
        '    If e.GetValue("Financed") = 0 Then
        '        e.Cell.BackColor = Drawing.Color.Orange
        '    End If
        'End If
    End Sub

    Protected Sub GV2_HtmlRowPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs)
        Dim grd1 As ASPxGridView = TryCast(sender, ASPxGridView)
        If (e.DataColumn.FieldName = "DAILYNUM") Then
            Menuitems = e.CellValue.ToString
            ClmState = IsClosed(Session("ClmNo"))
            '    If e.GetValue("TOTPRM") <> e.GetValue("InBox") Then
            '        e.Cell.ForeColor = Drawing.Color.Red
            '    Else
            '        e.Cell.ForeColor = Drawing.Color.Green
            '    End If
            '    Dim PolicyRow As DevExpress.Web.ASPxGridView.Rendering.GridViewTableDataRow = e.Cell.Parent
        End If

        'If (e.DataColumn.FieldName = "NETPRM") Then
        '    If e.GetValue("Financed") = 0 Then
        '        e.Cell.BackColor = Drawing.Color.Orange
        '    End If
        'End If
        If (e.DataColumn.FieldName = "") Then
            Dim SetlmMenu As ASPxMenu = (grd1).FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "SetlmMenu")
            If e.GetValue("DAILYNUM").ToString.Trim = "0" Then
                SetlmMenu.Items(0).Items.Add("طباعة")
                If Sys.Text = "02" Or Sys.Text = "03" Then
                    SetlmMenu.Items(0).Items.Add("إقرار سداد ومخالصة")
                Else

                End If
                SetlmMenu.Items(0).Items.Add("تحرير")
                SetlmMenu.Items(0).Items.Add("سداد")
                SetlmMenu.Items(0).Items.Add("إلغاء التسوية")
            Else
                SetlmMenu.Items(0).Items.Add("طباعة")
                If Sys.Text = "02" Or Sys.Text = "03" Then
                    SetlmMenu.Items(0).Items.Add("إقرار سداد ومخالصة")
                Else

                End If
            End If

            'If Menuitems = "0" Then
            '        SetlmMenu.Items(0).Items.Add("تحرير")
            '        SetlmMenu.Items(0).Items.Add("سداد")
            '        SetlmMenu.Items(0).Items.Add("إلغاء التسوية")
            '    Else

            '    End If
            '
            'Else
            'ClaimsMenu.Items(0).Items.Add("تحرير الوثيقة")
            '        ClaimsMenu.Items(0).Items.Add("عمليات خاصة")
            '        ClaimsMenu.Items(0).Items.Add("توزيع الوثيقة")
            'End If
            'End If
        End If
    End Sub

    Protected Sub GV1_BeforePerformDataSelect(sender As Object, e As EventArgs)
        Session("ClmNo") = (CType(sender, ASPxGridView)).GetMasterRowKeyValue()

        Dim grd As ASPxGridView = CType(sender, ASPxGridView)
        grd.DetailRows.ExpandAllRows()
        grd.SettingsDetail.ShowDetailButtons = True
        'Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        'grid.ExpandAll()
    End Sub

    Protected Sub GV1_DataBound(sender As Object, e As EventArgs)
        'If Convert.ToBoolean(Session("expandAll")) Then
        '    CType(sender, ASPxGridView).DetailRows.ExpandAllRows()
        'End If
        Dim grd As ASPxGridView = CType(sender, ASPxGridView)
        grd.DetailRows.ExpandAllRows()
        grd.SettingsDetail.ShowDetailButtons = True
    End Sub

    Protected Sub GV2_BeforePerformDataSelect(sender As Object, e As EventArgs)
        Session("TPID") = (CType(sender, ASPxGridView)).GetMasterRowKeyValue()

        Dim grd As ASPxGridView = CType(sender, ASPxGridView)
        grd.DetailRows.ExpandAllRows()
        grd.SettingsDetail.ShowDetailButtons = True
        'Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        'grid.ExpandAll()
        'Session("ClmNo") = (CType(sender, ASPxGridView)).GetRowValues(1)
    End Sub

    Protected Sub GV2_ContextMenuInitialize(sender As Object, e As ASPxGridViewContextMenuInitializeEventArgs)
        Dim grd As ASPxGridView = CType(sender, ASPxGridView)
        grd.DetailRows.ExpandAllRows()
        grd.SettingsDetail.ShowDetailButtons = True
    End Sub

    Protected Sub GV2_ContextMenuItemClick(sender As Object, e As ASPxGridViewContextMenuItemClickEventArgs)
        If e.Item.Name = "طباعة" Then
            'GV2_ContextMenuItemClick.ExportXlsToResponse(New XlsExportOptions())
        End If
    End Sub

    Protected Sub GV2_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs)

        Dim cmbsplited = e.Parameters.Split(",")

        Dim grid As ASPxGridView = CType(sender, ASPxGridView)

        'Dim Report = grid.GetRowValues(e.selectedindex, "OrderNo").ToString().Trim
        Select Case cmbsplited(0)
            Case "طباعة"
                Dim Report = ReportsPath & "Settelement"

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("ClmNo", cmbsplited(1).ToString, False),
                    New ReportParameter("No", cmbsplited(2).ToString, False),
                    New ReportParameter("TPID", Session("TPID").ToString, False),
                    New ReportParameter("Sys", Sys.Value.ToString, False)
                     }

                Session.Add("Parms", P)

                grid.JSProperties("cpMyAttribute") = "SettlePrint"
                grid.JSProperties("cpResult") = cmbsplited(1) & " - طباعة مذكرة تسوية للحادث - "
                grid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""

            Case "إقرار سداد ومخالصة"

                Dim Report = ReportsPath & "Eqrar"

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("ClmNo", cmbsplited(1).ToString, False),
                    New ReportParameter("No", cmbsplited(2).ToString, False)
                     }
                Session.Add("Parms", P)

                grid.JSProperties("cpMyAttribute") = "Eqrar"
                grid.JSProperties("cpResult") = cmbsplited(1) & " - طباعة إقرار سداد ومخالصة - "
                grid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""

            Case "تحرير"
                grid.JSProperties("cpMyAttribute") = "EditSatl"
                grid.JSProperties("cpResult") = cmbsplited(1) & "/" & cmbsplited(2) & " - تحرير مذكرة تسوية - "
                grid.JSProperties("cpNewWindowUrl") = "../Claims/settlement/Settle.aspx?ClmNo=" & cmbsplited(1) & "&TPID=" & cmbsplited(3) & "&No=" & cmbsplited(2) & "&Mode=Up&Sys=" & Sys.Text.Trim
            Case "إلغاء التسوية"
                If cmbsplited(4) <> "0" Then
                    grid.JSProperties("cpMyAttribute") = "DelSatl"
                    grid.JSProperties("cpResult") = cmbsplited(1) & "/" & cmbsplited(2) & " - لا يمكن إلغاء هذه التسوية لوجود إذن صرف  - "
                    grid.JSProperties("cpNewWindowUrl") = "" '"../Claims/settlement/Settle.aspx?ClmNo=" & cmbsplited(1) & "&TPID=" & Session("TPID").ToString & "&No=" & cmbsplited(2) & "&Mode=Up&Sys=" & Sys.Text.Trim
                Else
                    ExecConn("Delete MainSattelement where ClmNo='" & cmbsplited(1) & "' AND TPID=" & cmbsplited(3) & " AND NO=" & cmbsplited(2) & "", Conn)
                    ExecConn("Delete DetailSettelement where ClmNo='" & cmbsplited(1) & "' AND TPID=" & cmbsplited(3) & " AND NO=" & cmbsplited(2) & "", Conn)
                    If Sys.Text = "02" Or Sys.Text = "03" Then
                        ExecConn("Delete MoClaimFile where ClmNo='" & cmbsplited(1) & "' AND TPID=" & cmbsplited(3) & " AND NO=" & cmbsplited(2) & "", Conn)
                    Else
                    End If
                End If

        End Select

        Dim grd As ASPxGridView = CType(sender, ASPxGridView)
        grd.DetailRows.ExpandAllRows()
        grd.SettingsDetail.ShowDetailButtons = True
        grd.DataBind()
    End Sub

    Protected Sub ASPxCallback1_Callback(source As Object, e As CallbackEventArgs)
        Select Case e.Parameter
            Case "OutStnding"
                Dim Report = ReportsPath & "Outstanding"
                'Dim ReciptNo = AccMoveGrid.GetRowValues(e.VisibleIndex, "Poll").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim D = Format(Today.Date, "yyyy/MM/dd")

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("D1", D.ToString, True)
                     }
                'p.SetValue(New ReportParameter("BR", Session("Branch").ToString, False), 1)

                Session.Add("Parms", P)

                ASPxCallback1.JSProperties("cpMyAttribute") = "PRINTD"
                ASPxCallback1.JSProperties("cpResult") = " - كشف بالتعويضات تحت التسوية "
                ASPxCallback1.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
            Case "Payed"
                Dim Report = ReportsPath & "PaidClaims"
                'Dim ReciptNo = AccMoveGrid.GetRowValues(e.VisibleIndex, "Poll").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim D = Format(Today.Date, "yyyy/MM/dd")

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("D1", D.ToString, True),
                    New ReportParameter("D2", D.ToString, True)
                     }
                'p.SetValue(New ReportParameter("BR", Session("Branch").ToString, True), 2)

                Session.Add("Parms", P)

                ASPxCallback1.JSProperties("cpMyAttribute") = "PRINTDU"
                ASPxCallback1.JSProperties("cpResult") = " - كشف بالتعويضات المسددة "
                ASPxCallback1.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
        End Select
    End Sub

    Private Sub MainGrid_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles MainGrid.CustomCallback
        Dim cmbsplited = e.Parameters.Split(",")

        Dim grid As ASPxGridView = CType(sender, ASPxGridView)

        'Dim Report = grid.GetRowValues(e.selectedindex, "OrderNo").ToString().Trim
        Select Case cmbsplited(0)
            Case "تقرير بيانات الحادث"
                Dim Report = ReportsPath & "ClmDetail"

                'Dim p As ReportParameter() = New ReportParameter(1) {}

                'p.SetValue(New ReportParameter("ClmNo", cmbsplited(1).ToString, False), 0)
                'p.SetValue(New ReportParameter("Br", Branch.Text.ToString, False), 1)
                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("ClmNo", cmbsplited(1).ToString, False),
                    New ReportParameter("Br", Branch.Text.ToString, False)
                }

                Session.Add("Parms", P)

                grid.JSProperties("cpMyAttribute") = "ClmReport"
                grid.JSProperties("cpResult") = cmbsplited(1) & " - طباعة تقرير تفاصيل الحادث - "
                grid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
            Case Else
                Exit Select
        End Select
    End Sub

End Class