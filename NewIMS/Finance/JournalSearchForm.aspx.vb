Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.Web
Imports Microsoft.Reporting.WebForms

Public Class JournalSearchForm

    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            SearchGrid.DataBind()
            '  SearchClm.DataBind()
        End If
    End Sub

    Protected Sub ASPxButton1_Click(sender As Object, e As EventArgs) Handles ASPxButton1.Click
        ViewState("needBind") = True
        SearchGrid.DataBind()
        ' SearchClm.DataBind()
    End Sub

    Protected Sub SearchGrid_DataBinding(sender As Object, e As EventArgs) Handles SearchGrid.DataBinding
        If ViewState("needBind") IsNot Nothing AndAlso CBool(ViewState("needBind")) Then
            SearchGrid.DataSource = GetData()
        End If

    End Sub

    Private Function GetData() As DataTable
        Try
            Dim ConnectionPath As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ToString()
            Dim da As New SqlDataAdapter()
            Dim dt As New DataTable()
            Parm = Array.CreateInstance(GetType(SqlParameter), 1)
            SetPm("@searchText", DbType.String, searchtxt.Text.TrimEnd, Parm, 0)

            Dim Sql As String = CallSP("JournalBulkSearch", Conn, Parm)
            SqlDataSource1.SelectCommand = Sql

            Using sqlCon = New SqlConnection(ConnectionPath)
                Using cmd As New SqlCommand(Sql, sqlCon)
                    sqlCon.Open()
                    da.SelectCommand = cmd
                    da.Fill(dt)
                    sqlCon.Close()
                End Using
                Return dt
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub SearchGrid_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs) Handles SearchGrid.CustomButtonCallback
        Select Case e.ButtonID
            Case "Print"
                Dim Report = ReportsPath & "GeneralDaily"
                Dim DailyNo = SearchGrid.GetRowValues(e.VisibleIndex, "DAILYNUM").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim Sys = SearchGrid.GetRowValues(e.VisibleIndex, "DailyTyp").ToString().Trim

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("Daily", DailyNo, False),
                    New ReportParameter("Typ", Sys, False)
                }

                Session.Add("Parms", P)

                ASPxWebControl.RedirectOnCallback("~/Reporting/Previewer.aspx?Report=" & Report & "")

            Case "Edit"
                Dim DailyNo = SearchGrid.GetRowValues(e.VisibleIndex, "DAILYNUM").ToString().Trim
                Dim Sys = SearchGrid.GetRowValues(e.VisibleIndex, "DailyTyp").ToString().Trim

                If Page.IsCallback Then
                    SearchGrid.JSProperties("cpMyAttribute") = "Edit"
                    SearchGrid.JSProperties("cpResult") = GetSysName(Sys)
                    Select Case Request("Sys")
                        Case 4
                            ASPxWebControl.RedirectOnCallback("../Finance/DailySarfSD.aspx?daily=" + DailyNo + "&Sys=" + Sys)
                            ' SearchGrid.JSProperties("cpNewWindowUrl") = "../Finance/DailySarfSD.aspx?daily=" + DailyNo + "&Sys=" + Sys
                        Case Else
                            ASPxWebControl.RedirectOnCallback("../Finance/DailySarf.aspx?daily=" + DailyNo + "&Sys=" + Sys)
                    End Select
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If

            Case Else
                Exit Select
        End Select
    End Sub

    Public Function IsValidDate(d As String) As Boolean
        Dim DailyRec As New DataSet

        Dim dbadapter = New SqlDataAdapter("Select Month From Monthcheck where Month='" & Format(CDate(d), "yyyy/MM") & "' ", AccConn)
        dbadapter.Fill(DailyRec)
        'Response.Write(Format(CDate(d), "yyyy/MM"))

        If DailyRec.Tables(0).Rows.Count = 0 Then
            Return False
        Else
            Return True
        End If

    End Function

    Protected Sub SearchGrid_CustomButtonInitialize(sender As Object, e As ASPxGridViewCustomButtonEventArgs) Handles SearchGrid.CustomButtonInitialize
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))
        Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        ' Dim Field = IIf( , grid.GetRowValues(e.VisibleIndex, "IssuDate").ToString, "")
        Dim ValidDte = IsValidDate(grid.GetRowValues(e.VisibleIndex, "DAILYDTE").ToString().Trim)

        Select Case e.ButtonID
            Case "Edit"
                If ValidDte Then
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                Else
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
        End Select
    End Sub

End Class