Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports Microsoft.Reporting.WebForms

Public Class Dist
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))

        If myList Is Nothing Then
            ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('SystemManage/LogIn.aspx','_self'); </script>")
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 4)

            TryCast(FindControlRecursive(Form, "RightPane"), ASPxPanel).FixedPosition = PanelFixedPosition.WindowLeft

            TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar).RightToLeft = DevExpress.Utils.DefaultBoolean.False

        End If
        Session("Order") = ""
        MainGrid.DataBind()
    End Sub

    Private Sub MainGrid_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs) Handles MainGrid.CustomButtonCallback

        Select Case e.ButtonID
            Case "Print"
                Dim Report = ReportsPath & MainGrid.GetRowValues(e.VisibleIndex, "Report").ToString()
                Dim PolicyNo = MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = MainGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                Dim LoadNo = MainGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                Dim Sys = MainGrid.GetRowValues(e.VisibleIndex, "SubIns").ToString()
                Dim Br = MainGrid.GetRowValues(e.VisibleIndex, "Br").ToString()

                Dim Pol = MainGrid.GetRowValues(e.VisibleIndex, "PolNo").ToString()
                If IsStoped(Pol, EndNo, LoadNo) Then
                    Report = ReportsPath & "ReIssu"
                Else
                    'SelPolicyRep = SelectReport(Mid(PolNo.Text, 12, 2), Request("sys"))
                End If

                If Sys <> "OC" Then
                    'Dim p As ReportParameter() = New ReportParameter(2) {}
                    'p.SetValue(New ReportParameter("PolicyNo", PolicyNo, False), 0)
                    'p.SetValue(New ReportParameter("EndNo", EndNo, False), 1)
                    'p.SetValue(New ReportParameter("Sys", Sys, False), 2)
                    Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("PolicyNo", PolicyNo, False),
                    New ReportParameter("EndNo", EndNo, False),
                    New ReportParameter("Sys", Sys, False)
                }
                    Session.Add("Parms", P)
                Else
                    Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("PolicyNo", PolicyNo, False),
                    New ReportParameter("EndNo", EndNo, False),
                    New ReportParameter("LoadNo", LoadNo, False),
                    New ReportParameter("Sys", Sys, False)
                }
                    Session.Add("Parms", P)
                End If

                If Page.IsCallback Then
                    MainGrid.JSProperties("cpMyAttribute") = "PRINT"
                    MainGrid.JSProperties("cpResult") = GetSysName(Sys)
                    Select Case Sys
                        Case "02", "03"
                            MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
                        Case "OR"
                            If IsIssued(PolicyNo, EndNo, LoadNo, Sys) Then
                                MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/PreviewPDF.aspx?Report=" & Report & ""
                            Else

                            End If
                        Case Else
                            MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/PreviewPDF.aspx?Report=" & Report & ""
                    End Select

                    'If Sys = "02" Or Sys = "03" Then
                    '    MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
                    'Else

                    'End If
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If
            Case "Distribute"
                'Dim Report = "/IMSReports/" & MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "Report").ToString()
                Dim Order = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "EndNo").ToString()
                Dim LoadNo = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "LoadNo").ToString()
                Dim Sys = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "SubIns").ToString()
                Dim Br = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "Br").ToString()
                'IssuePolicy(Order, "", EndNo, LoadNo, Sys, Br, Session("User"))
                'Dim p As ReportParameter() = New ReportParameter(2) {}

                'p.SetValue(New ReportParameter("PolicyNo", Order, False), 0)
                'p.SetValue(New ReportParameter("EndNo", EndNo, False), 1)
                'p.SetValue(New ReportParameter("Sys", Sys, False), 2)

                'Session.Add("Parms", p)

                If Page.IsCallback Then

                    Dim PolicyNo = GetPolNo(Sys, Order, EndNo, LoadNo)
                    MainGrid.JSProperties("cpMyAttribute") = "Distribute"
                    MainGrid.JSProperties("cpResult") = GetSysName(Sys)
                    MainGrid.JSProperties("cpNewWindowUrl") = "../Reins/DistPolicy.aspx?OrderNo=" + Order + "&EndNo=" + EndNo + "&LoadNo=" + LoadNo + "&Branch=" + Br + "&Sys=" + Sys + "&PolNo=" + PolicyNo
                End If

            Case Else
                Exit Select

        End Select

    End Sub

    Protected Sub MainGrid_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs)
        Select Case e.Parameters
            Case "Issue"
            Case Else
                Exit Select

        End Select

    End Sub

    Protected Sub MainGrid_ToolbarItemClick(source As Object, e As ASPxGridViewToolbarItemClickEventArgs)
        Select Case e.Item.Name
            Case "NewPolicy"
                MainGrid.JSProperties("cpMyAttribute") = "New"
                MainGrid.JSProperties("cpResult") = GetSysName(Sys.Value)
                Select Case Sys.Value
                    Case "01", "27", "OR"
                        MainGrid.JSProperties("cpSize") = 900
                    Case Else
                        MainGrid.JSProperties("cpSize") = 1100
                End Select

                MainGrid.JSProperties("cpNewWindowUrl") = GetEditForm(Sys.Value) + "?Sys=" + Sys.Value
            Case "ExtraSearch"
                MainGrid.JSProperties("cpMyAttribute") = "Search"
                MainGrid.JSProperties("cpResult") = GetSysName(Sys.Value)
                MainGrid.JSProperties("cpNewWindowUrl") = "../Policy/SearchForm.aspx?SystemTable=" + GetGroupFile(Sys.Value) + "&Sys=" + Sys.Value
            Case Else
                Exit Select
        End Select

    End Sub

    Protected Sub MainGrid_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs)
        If e.DataColumn.FieldName = "PolNo" Then
            If e.GetValue("PolNo") <> "" Then
                If IsStoped(e.GetValue("PolNo"), e.GetValue("EndNo"), e.GetValue("LoadNo")) Then
                    e.Cell.ForeColor = Drawing.Color.Red
                    e.Cell.Text = e.GetValue("PolNo") + " / Cancelled"
                    e.Cell.HorizontalAlign = HorizontalAlign.Center
                Else
                    If IsIssued(e.GetValue("OrderNo"), e.GetValue("EndNo"), e.GetValue("LoadNo"), e.GetValue("SubIns")) Then
                        e.Cell.ForeColor = Drawing.Color.DarkGreen
                        e.Cell.HorizontalAlign = HorizontalAlign.Center
                    Else
                        e.Cell.BackColor = Drawing.Color.LightGray
                        e.Cell.ForeColor = Drawing.Color.DarkGreen
                        e.Cell.HorizontalAlign = HorizontalAlign.Center
                        e.Cell.Text = e.GetValue("PolNo") + " / عرض / ملحق / إشعار"
                    End If
                End If
            Else
                e.Cell.BackColor = Drawing.Color.Red
                e.Cell.ForeColor = Drawing.Color.White
                e.Cell.Text = "فاتورة مبدئية"
                e.Cell.HorizontalAlign = HorizontalAlign.Center
            End If
        End If
    End Sub

End Class