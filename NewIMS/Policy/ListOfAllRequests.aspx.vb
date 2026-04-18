Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.Web

Public Class ListOfAllRequests
    Inherits Page
    Private ReadOnly Property ConnectionString() As String
        Get
            Return ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString
        End Get
    End Property
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Using connection As New SqlConnection(ConnectionString)
            If connection.State = ConnectionState.Open Then
                connection.Close()
            Else

            End If
            connection.Open()
            ExecConn("UPDATE Notifications Set IsTreated=1, TreatingDate='" & Now.ToString("yyyy/MM/dd HH:mm:ss").ToString & "', TreatedBy='" & Session("User") & "' WHERE Timestamp < DATEADD(day, -30, GETDATE()) and IsTreated=0", connection)
        End Using
    End Sub

    Protected Sub CancelRequestsGrid_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs)
        Select Case e.ButtonID
            Case "Cancel"
                Dim Action = CancelRequestsGrid.GetRowValues(e.VisibleIndex, "Action").ToString()
                Dim OrderNo = CancelRequestsGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim Pol = CancelRequestsGrid.GetRowValues(e.VisibleIndex, "PolNo").ToString()
                Dim EndNo = CancelRequestsGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                Dim LoadNo = CancelRequestsGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                Dim Sys = CancelRequestsGrid.GetRowValues(e.VisibleIndex, "Sys").ToString()
                Dim Br = GetBranchbyOrderNo(CancelRequestsGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString())
                Dim ReqIdentifier = CancelRequestsGrid.GetRowValues(e.VisibleIndex, "Message").ToString()
                'Dim Br = Right(MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString(), 4)
                Action = Action + "&Request=" + ReqIdentifier
                ASPxWebControl.RedirectOnCallback(Action)
            Case Else
                Exit Select
        End Select
    End Sub
    Private Sub CancelRequestsGrid_CustomButtonInitialize(sender As Object, e As ASPxGridViewCustomButtonEventArgs) Handles CancelRequestsGrid.CustomButtonInitialize
        Dim Treated = CancelRequestsGrid.GetRowValues(e.VisibleIndex, "IsTreated")
        Dim isRejected = CancelRequestsGrid.GetRowValues(e.VisibleIndex, "IsTreated")
        Select Case e.ButtonID
            Case "Cancel"
                If Treated Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
            Case Else
                Exit Select
        End Select

    End Sub

    Protected Sub IssuDate_DataBound(sender As Object, e As EventArgs)
        Dim DateLbl As ASPxLabel = CType(sender, ASPxLabel)
        If IsDBNull(DirectCast(DateLbl.DataItemContainer, GridViewDataItemTemplateContainer).[Text]) Then
            DateLbl.Text = "لم تصدر"
        Else
            DateLbl.Text = GetIssDate(DirectCast(DateLbl.DataItemContainer, GridViewDataItemTemplateContainer).[Text])
        End If
    End Sub
End Class