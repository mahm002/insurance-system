Imports System.Data.SqlClient
Imports System.Web.UI

Partial Public Class CloseClmFile
    Inherits Page

    Private Net As Double
    Private Perm() As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim ClmData As New DataSet
        Dim SettlData As New DataSet

        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select Dailynum,DAILYDTE from MainSattelement where ClmNo='{0}' AND DAILYDTE is null", Request("ClmNo")), con)
            dbadapter.Fill(SettlData)
            If SettlData.Tables(0).Rows.Count = 0 Then
                ASPxLabel1.ClientVisible = False
                ASPxButton6.Enabled = True
            Else
                ASPxLabel1.ClientVisible = True
                ASPxButton6.Enabled = False

            End If

            con.Close()
        End Using
        'adDatePicker1.SelectedDate = Format(Today.Date, "yyyy-MM-dd")
        If Not IsNothing(Request("ClmNo")) Then
            ClmNo.Text = Trim(Request("ClmNo"))
            'Dim ConnLocal As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection(ConfigurationManager.AppSettings("Conn"))
            Dim dbadapter = New SqlDataAdapter("SELECT (CASE WHEN Status=1 THEN 'OPENED' ELSE CASE WHEN Status=2 THEN 'CLOSED' ELSE 'REJECTED' END END) As Status  " _
                                                     & "From MainClaimFile Where  ClmNo='" & Trim(Request("ClmNo")) & "' And Branch='" & Session("Branch") & "'", Conn)
            dbadapter.Fill(ClmData)
            If Not IsPostBack Then
                'SetEbaValue(state, FindExtra(ClmData.Tables(0).Rows(0)("Status"), "FileState"))
                Label2.Value = ClmData.Tables(0).Rows(0)("Status").trim()
            Else
            End If
        End If
        'SetLists()
        'HyperLink2.NavigateUrl = "../ ClaimsManage /Default.aspx?sys=" & Request("Sys") & ""
    End Sub

    Protected Sub ASPxButton6_Click(sender As Object, e As EventArgs) Handles ASPxButton6.Click
        ExecConn("update MainClaimFile Set Status= 2,ClmCloseDate='" & Format(Today.Date, "yyyy/MM/dd") & "' where ClmNo='" & ClmNo.Text & "' ", Conn)
        ASPxButton6.ClientEnabled = False
        ASPxButton7.ClientEnabled = False
    End Sub

    Protected Sub ASPxButton7_Click(sender As Object, e As EventArgs) Handles ASPxButton7.Click
        ExecConn("update MainClaimFile Set Status= 3,ClmCloseDate='" & Format(Today.Date, "yyyy/MM/dd") & "' where ClmNo='" & ClmNo.Text & "'", Conn)
        ASPxButton6.ClientEnabled = False
        ASPxButton7.ClientEnabled = False
    End Sub

End Class