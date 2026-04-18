Partial Public Class Default3
    Inherits System.Web.UI.Page
    Dim Lo() As String
    Dim DB As New DataSet
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Lo = Me.Session("LogInfo")
        Dim Sql As String
        If IsNothing(Request("No")) Then
            If Session("No").ToString = "" Then
                Session("No") = GetlastSettl(Request("ClmNo")) + 1
            Else
                Session("No") = Session("No")
            End If
            'IIf(IsNothing(Session("No")), , )
        Else
            Session("No") = Request("No")

        End If
        If Not IsPostBack And IsNothing(Request("No")) Then

        Else
            Sql = "Select * FRom MainSattelement where MainSattelement.ClmNo='" & Request("ClmNo") & "' And MainSattelement.No=" & Session("No") & " And MainSattelement.TPID=" & Request("TPID") & ""
            Dim dbadabter As New SqlClient.SqlDataAdapter(Sql, Conn)
            dbadabter.Fill(DB)
            If DB.Tables(0).Rows.Count = 0 Then GoTo 10
            PayTo.Text = IIf(Not IsNothing(Request("No")) And PayTo.Text <> "", PayTo.Text, DB.Tables(0).Rows(0)("PayTo"))
            SettelementDesc.Text = IIf(Not IsNothing(Request("No")) And SettelementDesc.Text <> "", SettelementDesc.Text, DB.Tables(0).Rows(0)("SettelementDesc"))
        End If

10:
        If Request("TPID") <> "" Then
            TPID.Value = Request("TPID")

        Else

        End If

        If IsPostBack Then
            Session("TPID") = TPID.Value
        Else

            ' Session("No") = NoLbl.Text
        End If
        GvDetails.DataBind()
    End Sub
    Protected Sub btnMainData_Click(sender As Object, e As EventArgs)
        If IsNothing(Request("No")) Then
            ExecSql("INSERT INTO [MainSattelement]([ClmNo],[TPID],[No],[PayTo],[SettelementDesc],[UserName]) " _
                & "values('" _
                       & Request("ClmNo") & "'," _
                       & Request("TPID") & "," _
                       & Session("No") & ",'" _
                       & PayTo.Text & "','" _
                       & SettelementDesc.Text & "','" _
                       & Lo(0) & "')")
            'pageControl.TabPages.FindByName("TabMain").ClientVisible = False
        Else
            ExecSql("Update [MainSattelement] set PayTo='" & PayTo.Text & "',[SettelementDesc]='" & SettelementDesc.Text & "' where No=" & Request("No") & " And ClmNo='" & Request("ClmNo") & "'")
            'Session("No") = Request("No")
        End If

        TPidLbl.Text = TPID.Value

    End Sub

    Protected Sub Add_Click(sender As Object, e As EventArgs)
        ExecSql(" INSERT INTO [DetailSettelement] ([ClmNo] , [TPID] , [No] , [Value], [PaymentDesciption]  ) " _
                & "values('" & Request("ClmNo") & "'," & TPID.Value & "," & Session("No") & "," & Val(txtValue.Value) & ",'" & Paymentdescr.Text & "')")
        GvDetails.DataBind()

    End Sub
End Class

