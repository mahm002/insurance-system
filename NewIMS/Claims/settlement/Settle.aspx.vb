Imports System.Web.UI

Partial Public Class Settle
    Inherits Page

    Private Lo() As String
    Private DB As New DataSet

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Lo = Session("LogInfo")

        Dim Sql As String

        MoDiv.Visible = Request("Sys") = "02" Or Request("Sys") = "03"

        If Request("Mode") = "New" And Not IsPostBack Then
            NoLbl.Text = GetlastSettl(Request("ClmNo")) + 1
            Session("No") = NoLbl.Text
            TPidLbl.Text = Request("TPID")
        Else
            NoLbl.Text = IIf(Request("Mode") = "Up", Request("No"), Session("No"))
            TPidLbl.Text = Request("TPID")
        End If

        Sql = "Select * From MainSattelement where MainSattelement.ClmNo='" & Request("ClmNo") & "' And MainSattelement.No=" & NoLbl.Text & " And MainSattelement.TPID=" & Request("TPID") & ""
        Dim dbadabter As New SqlClient.SqlDataAdapter(Sql, Conn)
        dbadabter.Fill(DB)
        If DB.Tables(0).Rows.Count = 0 Then GoTo 10
        PayTo.Text = IIf(Not IsNothing(Request("No")) And PayTo.Text <> "", PayTo.Text, DB.Tables(0).Rows(0)("PayTo"))
        SettelementDesc.Text = IIf(Not IsNothing(Request("No")) And SettelementDesc.Text <> "", SettelementDesc.Text, DB.Tables(0).Rows(0)("SettelementDesc"))
10:
        TPID.Value = Request("TPID")
        If IsPostBack Then
            Session("TPID") = TPID.Value
        Else

            ' Session("No") = NoLbl.Text
        End If
    End Sub

    Protected Sub btnMainData_Click(sender As Object, e As EventArgs)
        If Request("Mode") = "New" Then
            ExecConn("INSERT INTO [MainSattelement]([ClmNo],[TPID],[No],[PayTo],[SettelementDesc],[UserName]) " _
                & "values('" _
                       & Request("ClmNo") & "'," _
                       & TPidLbl.Text & "," _
                       & NoLbl.Text & ",'" _
                       & PayTo.Text & "','" _
                       & SettelementDesc.Text & "','" _
                       & Lo(8) & "')", Conn)
        Else
            ExecConn("Update [MainSattelement] set PayTo='" & PayTo.Text & "',[SettelementDesc]='" & SettelementDesc.Text & "' where No=" & NoLbl.Text & " And ClmNo='" & Request("ClmNo") & "'", Conn)
            'Session("No") = Request("No")
        End If

        TPidLbl.Text = TPID.Value

    End Sub

    Protected Sub Add_Click(sender As Object, e As EventArgs)
        'ExecSql(" INSERT INTO [DetailSettelement] ([ClmNo] , [TPID] , [No] , [Value], [PaymentDesciption]  ) " _
        '        & "values('" & Request("ClmNo") & "'," & TPID.Value & "," & Session("No") & "," & Val(txtValue.Value) & ",'" & Paymentdescr.Text & "')")
        'GvDetails.DataBind()

    End Sub

End Class