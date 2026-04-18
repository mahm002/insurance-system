


Imports DevExpress.Web


Partial Class ClaimsManage_settlement_Default2
    Inherits System.Web.UI.Page
    Protected Sub detailGrid_BeforePerformDataSelect(ByVal sender As Object, ByVal e As EventArgs)
        Dim masterKey As Object = (TryCast(sender, ASPxGridView)).GetMasterRowKeyValue()
        Session("key") = masterKey
        Session("No") = masterGrid.GetRowValuesByKeyValue(masterKey, "No")
        Session("ClmNo") = masterGrid.GetRowValuesByKeyValue(masterKey, "ClmNo")
    End Sub

    Protected Sub Grid_RowDeleting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataDeletingEventArgs)
        ' Database editing is not allowed in online examples
        e.Cancel = True
    End Sub

    Protected Sub Grid_RowInserting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInsertingEventArgs)
        ' Database editing is not allowed in online examples
        e.Cancel = True
    End Sub

    Protected Sub Grid_RowUpdating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs)
        ' Database editing is not allowed in online examples
        e.Cancel = True
    End Sub
End Class
