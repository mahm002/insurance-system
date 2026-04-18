Imports DevExpress.Web
Imports Microsoft.Reporting.WebForms

Public Class PrintCheques
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub ASPxGridView1_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs) Handles ASPxGridView1.CustomButtonCallback
        Select Case e.ButtonID
            Case "Print"
                Dim Report = ReportsPath & "Cheque"
                Dim DailyNo = ASPxGridView1.GetRowValues(e.VisibleIndex, "DAILYNUM").ToString().Trim
                Dim AccNo = ASPxGridView1.GetRowValues(e.VisibleIndex, "AccountNo").ToString().Trim
                Dim Docn = ASPxGridView1.GetRowValues(e.VisibleIndex, "DocNum").ToString().Trim
                Dim Sys = ASPxGridView1.GetRowValues(e.VisibleIndex, "DailyTyp").ToString().Trim

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("DNo", AccNo, False),
                    New ReportParameter("DocNo", Docn, False),
                    New ReportParameter("DocNoTo", Docn, False),
                    New ReportParameter("DailyNo", DailyNo, False),
                    New ReportParameter("Typ", Sys, False)
                }

                Session.Add("Parms", P)

                If Page.IsCallback Then
                    ASPxGridView1.JSProperties("cpMyAttribute") = "PRINT"
                    ASPxGridView1.JSProperties("cpResult") = "طباعة الصك رقم " & Docn

                    ASPxGridView1.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
                Else

                End If

        End Select
    End Sub

End Class