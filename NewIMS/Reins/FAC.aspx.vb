Imports DevExpress.Web
Imports Microsoft.Reporting.WebForms
Imports System.Drawing

Public Class FAC
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))

        'Call SetUserPermNAV(FindControlRecursive(Form, "SideBar"), myList.ToArray, 4)
        If myList Is Nothing Then
            'ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('SystemManage/LogIn.aspx','_self'); </script>")
            FormsAuthentication.RedirectToLoginPage()
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 4)

            TryCast(FindControlRecursive(Form, "RightPane"), ASPxPanel).FixedPosition = PanelFixedPosition.WindowLeft

            TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar).RightToLeft = DevExpress.Utils.DefaultBoolean.False

        End If
        'cmbReports.Dat

        Grid.DataBind()


    End Sub
    Protected Sub TraetyGrid_BeforePerformDataSelect(sender As Object, e As EventArgs)
        Dim tt = CType(sender, ASPxGridView).GetMasterRowKeyValue().ToString.Split("|")
        MyBase.Session("PolNo") = tt(0)
        MyBase.Session("EndNo") = CInt(tt(1))
        MyBase.Session("LoadNo") = CInt(tt(2))
        MyBase.Session("FacPolRef") = tt(3)
        'CType(sender, ASPxGridView).DataBind()
    End Sub
    Protected Sub Pb_DataBound(sender As Object, e As EventArgs)

        Dim progressBar As ASPxProgressBar = CType(sender, ASPxProgressBar)

        If progressBar.Position > 100 Then
            progressBar.Position = 100
        End If

        If progressBar.Position < 40 Then
            progressBar.IndicatorStyle.BackColor = Color.LightGreen
        ElseIf progressBar.Position >= 40 AndAlso progressBar.Position < 75 Then
            progressBar.IndicatorStyle.BackColor = Color.Yellow
        ElseIf progressBar.Position >= 75 Then
            progressBar.IndicatorStyle.BackColor = Color.Red
        End If
    End Sub
    Protected Sub APb_DataBound(sender As Object, e As EventArgs)

        Dim progressBar As ASPxProgressBar = CType(sender, ASPxProgressBar)

        If progressBar.Position > 100 Then
            progressBar.Position = 100
        End If
        If progressBar.Position < 40 Then
            progressBar.IndicatorStyle.BackColor = Color.Red
            progressBar.ForeColor = Color.Red
        ElseIf progressBar.Position >= 40 AndAlso progressBar.Position < 75 Then
            progressBar.IndicatorStyle.BackColor = Color.Yellow
        ElseIf progressBar.Position >= 75 Then
            progressBar.IndicatorStyle.BackColor = Color.LightGreen
        End If
    End Sub

    Protected Sub FG_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs)
        Dim cmbsplited = e.Parameters.Split(",")

        Dim FGrid As ASPxGridView = TryCast(sender, ASPxGridView)
        Select Case cmbsplited(0)
            Case "إضافة قسيمة اختياري"
                FGrid.JSProperties("cpMyAttribute") = "NewSlip"
                FGrid.JSProperties("cpResult") = cmbsplited(1) & "/" & cmbsplited(2) & "/" & cmbsplited(3)
                FGrid.JSProperties("cpNewWindowUrl") = "../Reins/FacSlip.aspx?PolNo=" & cmbsplited(1) & "&EndNo=" & cmbsplited(2) & "&LoadNo=" & cmbsplited(3) & "&Ref=" & cmbsplited(4) & "&War=0&SlipNo=0"
            Case Else
                Exit Select
        End Select
    End Sub

    Protected Sub FacGrid_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs)
        If e.DataColumn.FieldName = " " Then
            Dim FacMenue As ASPxMenu = TryCast(sender, ASPxGridView).FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "FacMenu")
            Dim unused = FacMenue.Items(0).Items.Add("إضافة قسيمة اختياري")
        End If

    End Sub

    Protected Sub SlipGrid_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs)

        Dim SlipKey = sender.GetRowValues(e.VisibleIndex, "SlipNo").ToString()
        Dim PolN = sender.GetRowValues(e.VisibleIndex, "PolNo").ToString()
        Dim Endn = sender.GetRowValues(e.VisibleIndex, "EndNo").ToString()
        Dim LoadN = sender.GetRowValues(e.VisibleIndex, "LoadNo").ToString()

        Select Case e.ButtonID
            Case "Edit"
                sender.JSProperties("cpMyAttribute") = "Edit"
                sender.JSProperties("cpResult") = SlipKey + " UPDATE "
                sender.JSProperties("cpNewWindowUrl") = "../Reins/FacSlip.aspx?PolNo=" & PolN & "&EndNo=" & Endn & "&LoadNo=" & LoadN & "&Ref=" & Session("FacPolRef") & "&War=0&SlipNo=" & SlipKey & "&Operation=Edit"

            Case "Cancel"
                sender.JSProperties("cpMyAttribute") = "Cancel"
                sender.JSProperties("cpResult") = SlipKey + "Cancel"
                sender.JSProperties("cpNewWindowUrl") = "../Reins/FacSlip.aspx?PolNo=" & PolN & "&EndNo=" & Endn & "&LoadNo=" & LoadN & "&Ref=" & Session("FacPolRef") & "&War=0&SlipNo=" & SlipKey & "&Operation=Cancel"

            Case "Print"
                Dim Report = "/IMSReports/FacSlip"
                'Dim p As ReportParameter() = New ReportParameter(0) {}
                'p.SetValue(New ReportParameter("SlipNo", SlipKey, False), 0)

                Dim P As New List(Of ReportParameter) From {
                        New ReportParameter("SlipNo", SlipKey, False)
                        }
                Session.Add("Parms", p)

                sender.JSProperties("cpMyAttribute") = "PRINT"
                sender.JSProperties("cpResult") = SlipKey & " / PRINT / "
                sender.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""

            Case Else

                Exit Select
        End Select

    End Sub
    Protected Sub SlipGrid_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs)
        Dim SlpGrid As ASPxGridView = TryCast(sender, ASPxGridView)

        Dim Cmds = e.Parameters.Split(",")
        Select Case Cmds(0)
            Case "Delete"
                ExecConn("Delete FacClosingSlips where SlipNo='" & Cmds(1).Trim & "' ", Conn)
            Case Else
                Exit Select
        End Select

        'SlpGrid.DataBind()

        'Dim PolN = Grid.GetRowValues(e.VisibleIndex, "PolNo").ToString()
        'Grid.ExpandRow(0)
    End Sub
End Class