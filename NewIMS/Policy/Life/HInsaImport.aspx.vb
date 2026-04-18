Imports DevExpress.Spreadsheet
Imports DevExpress.Spreadsheet.Export

Public Class HInsaImport
    Inherits Page

    Private Property FilePath() As String
        Get
            Return If(Session("FilePath") Is Nothing, String.Empty, Session("FilePath").ToString())
        End Get
        Set(ByVal value As String)
            Session("FilePath") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs)
        If Not IsPostBack Then
            FilePath = String.Empty
        End If
    End Sub

    Protected Sub FileUploadComplete(ByVal sender As Object, ByVal e As DevExpress.Web.FileUploadCompleteEventArgs)
        FilePath = Page.MapPath("~/App_Data/") & e.UploadedFile.FileName
        e.UploadedFile.SaveAs(FilePath)
    End Sub

    Private Function GetTableFromExcel() As DataTable
        Dim book As New Workbook()
        AddHandler book.InvalidFormatException, AddressOf book_InvalidFormatException
        book.LoadDocument(FilePath)
        Dim sheet As Worksheet = book.Worksheets.ActiveWorksheet
        Dim range As CellRange = sheet.GetUsedRange()
        Dim table As DataTable = sheet.CreateDataTable(range, False)
        Dim exporter As DataTableExporter = sheet.CreateDataTableExporter(range, table, False)
        AddHandler exporter.CellValueConversionError, AddressOf exporter_CellValueConversionError
        exporter.Export()
        Return table
    End Function

    Private Sub exporter_CellValueConversionError(ByVal sender As Object, ByVal e As CellValueConversionErrorEventArgs)
        e.Action = DataTableExporterAction.Continue
        e.DataTableValue = Nothing
    End Sub

    Private Sub book_InvalidFormatException(ByVal sender As Object, ByVal e As SpreadsheetInvalidFormatExceptionEventArgs)

    End Sub

    Protected Sub Grid_Init(ByVal sender As Object, ByVal e As EventArgs)
        If Not String.IsNullOrEmpty(FilePath) Then
            Grid.DataSource = GetTableFromExcel()

            Grid.DataBind()

            'SaveData()

        End If
    End Sub

    Private Sub SaveData()

    End Sub

    Protected Sub ASPxButton5_Click(sender As Object, e As EventArgs) Handles ASPxButton5.Click
        Dim i As Integer = 1 'Rows
        Dim j As Integer = 1 'Columns
        Dim GRP As Integer = 1 '
        NewMethod(i, GRP)
    End Sub

    Private Sub NewMethod(ByRef i As Integer, ByRef GRP As Integer)
        'If Grid.VisibleColumns.Count <> 9 Then
        '    MsgBox("ERROR IN DATA")
        'Else

        While Grid.VisibleRowCount > i
            'While Grid.VisibleColumns.Count > j
            ExecConn("insert into HealthInsurance(SubIns,OrderNo,EndNo,LoadNo,Name,Dob,EmpId,CardNo,Relation,SumIns,Rate,Premium) Values('" _
                              & Request("Sys") & "','" _
                              & Request("OrderNo") & "'," _
                              & Request("EndNo") & "," _
                              & Request("LoadNo") & ",'" _
                              & CStr(Grid.GetDataRow(i)(2).ToString.Trim) & "','" _
                              & Format(CDate(Grid.GetDataRow(i)(3).ToString.Trim), "yyyy/MM/dd") & "','" _
                              & CStr(Grid.GetDataRow(i)(0).ToString.Trim) & "','" _
                              & CStr(Grid.GetDataRow(i)(1).ToString.Trim) & "','" _
                              & CStr(Grid.GetDataRow(i)(4).ToString.Trim) & "'," & CDbl(SumIns.Value) & "," & CDbl(Rate.Value) & "," & CDbl(Rate.Value) & ")", Conn)
            On Error Resume Next
            GRP += 1
            i += 1
        End While
        ASPxButton5.Enabled = False
        'End If
    End Sub

End Class