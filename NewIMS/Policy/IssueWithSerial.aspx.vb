Imports System.Data.SqlClient
Imports DevExpress.Web
Imports Microsoft.Reporting.WebForms

Public Class IssueWithSerial
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        AvailableSerials.DataBind()
        If AvailableSerials.Items.Count = 0 Then
            AlertSerial.Text = "لا توجد نماذج متوفرة حالياً/ يرجى الاتصال بمدير النظام"
            AlertSerial.Visible = True
            Issue.Enabled = False
        Else
            Dim No As Integer = AvailableSerials.Items.Count
            AlertSerial.ForeColor = Drawing.Color.Red
            AlertSerial.Text = "  العدد المتبقي من النماذج  " & No
            AlertSerial.Visible = True
        End If

    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback
        Select Case e.Parameter
            Case "Issue"
                Dim SrlN As New DataSet
                Issue.Enabled = False
                Dim Report = ReportsPath & PolRep(Request("Sys")) 'IIf((Trim(Request("Br")) = "TR00") And (Request("Sys") = "01" Or Request("Sys") = "OR"), PolRep(Request("Sys")) + "M", PolRep(Request("Sys")))
                Using oCon As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    Else

                    End If
                    oCon.Open()
                    Dim SrlAdptr = New SqlDataAdapter(";with cte as (select SerialF ,SerialT from SUBSYSTEMS where Branch='" & Request("Br") & "' And SUBSYSNO='" & Request("Sys") & "' union all select SerialF+1 ,SerialT from cte where SerialF<SerialT) select TOP(1) SerialF from cte where SerialF not in (Select SerialNo From PolicyFile where Branch='" & Request("Br") & "' AND SubIns='" & Request("Sys") & "') order by SerialF option (maxrecursion 0)", oCon)
                    Dim unused = SrlAdptr.Fill(SrlN)
                    oCon.Close()
                End Using
                If SrlN.Tables(0).Rows.Count = 0 Then Exit Sub

                Dim Order = Request("OrderNo")
                Dim EndNo = Request("EndNo")
                Dim LoadNo = Request("LoadNo")
                Dim Sys = Request("Sys")
                Dim Br = Request("Br")
                Dim Serial = IIf(AvailableSerials.Value = CInt(SrlN.Tables(0).Rows.Item(0).Item("SerialF")), CInt(SrlN.Tables(0).Rows.Item(0).Item("SerialF")), AvailableSerials.Value)
                If Serial Is Nothing Then
                    Exit Sub
                Else
                    IssuePolicy(Order, "", EndNo, LoadNo, Sys, Br, Session("UserID"), Serial)
                End If

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("PolicyNo", Order, False),
                    New ReportParameter("EndNo", EndNo, False),
                    New ReportParameter("Sys", Sys, False)
                }

                Session.Add("Parms", P)

                ASPxWebControl.RedirectOnCallback("~/Reporting/PreviewPDF.aspx?Report=" & Report & "")

                Dim Message As String = "شركة مجموعة الخليج للتأمين" & vbCrLf & "ترحب بكم وتشكركم على ثقتكم"

                Call SendSMS(GetPhoneNo(GetCustByOrderNo(Order, EndNo, LoadNo)), Message)

                Dim ExpiredNote As SqlCommand
                Dim Dtbl As New DataTable

                Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    Else

                    End If
                    con.Open()

                    ExpiredNote = New SqlCommand With {
                        .CommandText = "ExpiredPols",
                        .CommandType = CommandType.StoredProcedure
                    }

                    'ExpiredNote.Parameters.AddWithValue("@Sys", Odbc.OdbcType.NVarChar).Value = Request("Sys")
                    ExpiredNote.Connection = con

                    Dim Sms As String = ""

                    Dim myReader As SqlDataReader = ExpiredNote.ExecuteReader()

                    If myReader.HasRows Then
                        Dtbl.Load(myReader)

                        For Each row As DataRow In Dtbl.Rows
                            Sms = "السادة /" & row.Item("CustName").trim & "" & vbCrLf & "تبلغكم شركة مجموعة الخليج للتأمين عن قرب انتهاء تغطية وثيقتكم وذلك بتاريخ " & Format(row.Item("CoverTo"), "yyyy/MM/dd").ToString & " " & vbCrLf & "يمكنكم زيارة أقرب فرع أو وكيل لتجديد التغطية" & vbCrLf & "معاً لمستقبل آمن"

                            'Call SendWelcomeSMS(row.Item("TelNo"), Sms)
                        Next row
                    Else

                    End If

                    con.Close()
                End Using

            Case Else
                Exit Select
        End Select
    End Sub

    Private Shared Function NewMethod() As SqlCommand

        'Call SendWelcomeSMS(GetPhoneNo(GetCustByOrderNo(Order, EndNo, LoadNo)), Message)
        Dim ExpiredNote As New SqlCommand
        Return ExpiredNote
    End Function

End Class