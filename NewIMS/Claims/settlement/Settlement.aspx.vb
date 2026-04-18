Imports DevExpress.Web.Data
Imports DevExpress.Web
Imports DevExpress.XtraPrinting

Partial Class ClaimsManage_Sattlments_Settlement
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        'GridViewFeaturesHelper.SetupGlobalGridViewBehavior(grid)
        If Request("TPID") = "" Then

        End If
        If (Not IsPostBack) Then
            grid.StartEdit(2)
        End If
        grid.SettingsEditing.Mode = GridViewEditingMode.PopupEditForm
    End Sub

    Protected Sub grid_RowUpdating(ByVal sender As Object, ByVal e As ASPxDataUpdatingEventArgs)
        e.NewValues("PayTo") = GetMemoText()
        e.NewValues("DAILYNUM") = GetDailyText()
        e.NewValues("DAILYNUM") = GetDailyNo()
    End Sub
    Protected Sub grid_RowInserting(ByVal sender As Object, ByVal e As ASPxDataInsertingEventArgs)
        e.NewValues("PayTo") = GetMemoText()
        e.NewValues("DAILYNUM") = GetDailyText()
        e.NewValues("DAILYNUM") = GetDailyNo()
    End Sub
    Protected Function GetMemoText() As String
        Dim pageControl As ASPxPageControl = TryCast(grid.FindEditFormTemplateControl("pageControl"), ASPxPageControl)
        Dim memo As ASPxMemo = TryCast(pageControl.FindControl("notesEditor"), ASPxMemo)
        Return memo.Text
    End Function
    Protected Function GetDailyText() As String
        Dim pageControl As ASPxPageControl = TryCast(grid.FindEditFormTemplateControl("pageControl"), ASPxPageControl)
        Dim DAILYNUM As ASPxTextBox = TryCast(pageControl.FindControl("DailyNo"), ASPxTextBox)
        Return DAILYNUM.Text
    End Function
    Protected Function GetDailyNo() As String
        Dim pageControl As ASPxPageControl = TryCast(grid.FindEditFormTemplateControl("pageControl"), ASPxPageControl)
        Dim DAILYNUM As ASPxComboBox = TryCast(pageControl.FindControl("DailyNo"), ASPxComboBox)
        Return DAILYNUM.Text
    End Function
    Protected Sub detailGrid_DataSelect(ByVal sender As Object, ByVal e As EventArgs)
        Session("ClmNo") = (TryCast(sender, ASPxGridView)).GetMasterRowKeyValue()
        Session("TPID") = (TryCast(sender, ASPxGridView)).GetMasterRowKeyValue()
    End Sub
    Protected Sub detailGrid_CustomUnboundColumnData(ByVal sender As Object, ByVal e As ASPxGridViewColumnDataEventArgs)
        If e.Column.FieldName = "Total" Then
            Dim price As Decimal = CDec(e.GetListSourceFieldValue("UnitPrice"))
            Dim quantity As Integer = Convert.ToInt32(e.GetListSourceFieldValue("Quantity"))
            e.Value = price * quantity
        End If
    End Sub
End Class
