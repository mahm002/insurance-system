Partial Class ClaimsManage_MoClaims_MoFollowUp
    Inherits System.Web.UI.Page
    Protected WithEvents Garage1 As eba.Web.Combo
    Dim DataRet As New DataView
    Private Const GarageCol As Integer = 1    ' Database column containing contact name 
    Private Const spareCol As Integer = 2   ' Database column containing contact email
    Private Const repCol As Integer = 4    ' Database column containing contact company name
    Private Const totCol As Integer = 5
    Private Const TpNo As Integer = 6

    'Protected Sub Check(ByVal sender As Object, ByVal e As System.EventArgs) Handles check1.SelectedIndexChanged
    'Dim i As Integer
    'mess.Text = "<p>Selected Item(s):</p>"
    'For i = 0 To check1.Items.Count - 1
    'If check1.Items(i).Selected Then
    'mess.Text += check1.Items(i).Text + "<br />"
    'End If
    'Next

    'End Sub
    Private Sub InitializeComponent()

    End Sub
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

    Private Sub BindGrid()
        ' Bind the grid to the datasource
        Me.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim CarInf As DataSet
        Dim Pol, Gr As String

        Call FillCombos(Me.Page.Form, Request("sys"))
        InitializeComponent()
        'Call FillCombos(GridView1.SelectedRow.Controls("garage1"))
        SqlDataSource1.SelectParameters("ClmNo").DefaultValue = Request("ClmNo")
        SqlDataSource2.SelectParameters("ClmNo").DefaultValue = Request("ClmNo")
        'GridView1.DataSource = SqlDataSource1
        'GridView1.DataBind()

        Try
            Pol = pageFooter.FindControl("PolNo").ToString()
            Gr = Val(pageFooter.FindControl("GroupNo").ToString())

            CarInf = RecSet("SELECT PolicyFile.PolNo, PolicyFile.EndNo, MOMOTORFILE.BudyNo, MOMOTORFILE.TableNo, MOMOTORFILE.EnginNo, MOMOTORFILE.GroupNo , PolicyFile.IssuDate, PolicyFile.CoverFrom, PolicyFile.CoverTo, MOMOTORFILE.State, CustomerFile.CustName, EXTRAINFO.TPName FROM EXTRAINFO INNER JOIN MOMOTORFILE ON EXTRAINFO.TPNo = MOMOTORFILE.CarType AND EXTRAINFO.TP = 'cartp' LEFT OUTER JOIN CustomerFile LEFT OUTER JOIN PolicyFile ON CustomerFile.CustNo = PolicyFile.CustNo ON MOMOTORFILE.OrderNo = PolicyFile.OrderNo AND MOMOTORFILE.EndNo = PolicyFile.EndNo where PolNo ='" & Pol & "' AND groupno= " & Val(Gr.ToString()) & "", Conn)
        Catch
        End Try
        If (Not Page.IsPostBack) Then

            BindGrid()
        End If

        ' if datagrid is in edit mode, bind events to comboboxes
        If (GridView1.EditItemIndex <> -1) Then
            BindCombo(GridView1.EditItemIndex)
        End If


        'g.DataSource = RecSet(" select top 10 TpName,TpNo from extrainfo where Tp='Garages'", Conn)
        'garages.DataSource = RecSet(" select Tpname from extrainfo where Tp='Garages'", Conn)
        'Dim carr As DataSet
        ' pageFooter.FindControl("groupno").EnableViewState = True
        'SetTextValue(pageFooter.FindControl("Label1"), "حوادث تكميلي")

    End Sub


    Private Sub BindCombo(ByVal curRow As Integer)
        ' This function is called on each postback caused by the select or getpage events. 
        ' It is necessary to rebind the combobox on each postback.
        Dim Garage As eba.Web.Combo
        ' Locate the row being edited and use FindControl function to retrieve combobox
        Garage = DirectCast(GridView1.Items(curRow).FindControl("Garage"), eba.Web.Combo)


        If (Not (Garage Is Nothing)) Then
            ' Bind combobox to appropriate datasource
            Garage.DataSource = GetComboDs(Garage.List.PageSize, "a", 0, "")
            ' Bind comboxes events to appropriate event handlers
            Dim GetPageHandler As eba.Web.ComboGetPageEventHandler
            GetPageHandler = AddressOf Me.Garage_GetPage
            AddHandler Garage.GetPage, GetPageHandler

            Dim SelectHandler As System.EventHandler
            SelectHandler = AddressOf Me.Garage_Select
            AddHandler Garage.Select, SelectHandler

        End If

    End Sub


    Private Function GetComboDs(ByVal PageSize As Integer, ByVal SubString As String, ByVal CurrentRecord As Integer, ByVal LastString As String) As DataSet

        Dim dbAdapter As Data.SqlClient.SqlDataAdapter
        Dim ds As DataSet
        Dim serverPath As String = Server.MapPath("")

        ' Create a DB connection to the MDB.
        'dbConn = New OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; data source=" & serverPath & dbPath)

        ' Create a dataset and add the table to it.
        ds = New DataSet
        'ds.Tables.Add("Extrainfo")
        ' Fill the dataset with data.  Match the query to the substring and only fetch the page
        ' starting at CurrentRecord.
        dbAdapter = New Data.SqlClient.SqlDataAdapter("SELECT TOP 10 TPName,TpNo FROM extrainfo WHERE TP='Garages' AND TpName > '" & LastString & "' AND TpName LIKE '" & SubString & "%' ORDER BY TPName", Conn)
        dbAdapter.Fill(ds)
        Return ds

    End Function

    Public Sub Garage_Select(ByVal sender As Object, ByVal e As EventArgs)

        ' Called when a name is selected from cmbName.
        Dim txtspare, txtrep, txttotal, txttpno As System.Web.UI.WebControls.TextBox
        Dim Garage As eba.Web.Combo = Nothing
        ' Retrieving the combobox from sender
        Garage = DirectCast(sender, eba.Web.Combo)
        ' Using the sender object to retrieve its parent row.
        Dim dgi As DataGridItem = DirectCast(Garage.Parent.Parent, DataGridItem)
        ' Using FindControl to retrieve references to both textboxes
        txtspare = DirectCast(dgi.FindControl("txtspare"), System.Web.UI.WebControls.TextBox)
        txtrep = DirectCast(dgi.FindControl("txtrep"), System.Web.UI.WebControls.TextBox)
        txttotal = DirectCast(dgi.FindControl("txttotal"), System.Web.UI.WebControls.TextBox)
        txttpno = DirectCast(dgi.FindControl("tpno"), System.Web.UI.WebControls.TextBox)
        If (Garage.SelectedItem Is Nothing) Then
            ' Retrieve associated email and company name
            txtspare.Text = Garage.SelectedRowValues(spareCol)
            txtrep.Text = Garage.SelectedRowValues(repCol)
            txttotal.Text = Garage.SelectedRowValues(totCol)
            txttpno.Text = Garage.SelectedRowValues(TpNo)

        End If
    End Sub
    Public Sub GridView1_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GridView1.EditCommand
        ' Set the EditItemIndex property to the index of the item clicked 
        ' in the DataGrid control to enable editing for that item.
        TPN.Text = Val(DirectCast(GridView1.Items(e.Item.ItemIndex).Cells(3).Controls(0), DataBoundLiteralControl).Text)
        Name.Text = DirectCast(GridView1.Items(e.Item.ItemIndex).Cells(2).Controls(0), DataBoundLiteralControl).Text
        Dim currText As String = DirectCast(GridView1.Items(e.Item.ItemIndex).Cells(2).Controls(0), DataBoundLiteralControl).Text
        GridView1.EditItemIndex = e.Item.ItemIndex
        BindGrid()
        ' Set the combobox's text field to selected contact name
        Dim Garage As eba.Web.Combo = DirectCast(GridView1.Items(e.Item.ItemIndex).FindControl("Garage"), eba.Web.Combo)
        Garage.TextValue = currText.Trim

    End Sub

    Private Sub GridView1_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GridView1.UpdateCommand

        ' Update info and return grid to non-edit mode. Here, the label control at the bottom of the page
        ' displays the updated info. In a functioning datagrid, the mdb file would be updated through SQL.
        Dim txtspare, txtrep, txttotal As System.Web.UI.WebControls.TextBox
        Dim Garage As eba.Web.Combo

        ' Use the event argument and FindControl function to retrieve references to cmbName, and textboxes
        Garage = DirectCast(e.Item.Cells(GarageCol).FindControl("Garage"), eba.Web.Combo)
        txtspare = DirectCast(e.Item.Cells(spareCol).FindControl("txtspare"), System.Web.UI.WebControls.TextBox)
        txtrep = DirectCast(e.Item.Cells(repCol).FindControl("txtrep"), System.Web.UI.WebControls.TextBox)
        txttotal = DirectCast(e.Item.Cells(totCol).FindControl("txttotal"), System.Web.UI.WebControls.TextBox)


        If Garage.TextValue.Trim <> Name.Text.Trim Then
            ExecSql("update MoClaimFile set GarageRef=" & Garage.SelectedItem.Value & ",Total=" & CDbl(txtrep.Text) + CDbl(txtspare.Text) & ",EstimatedSpare=" & CDbl(txtspare.Text) & ",EstimatedRep = " & CDbl(txtrep.Text) & " WHERE ClmNo='" & Request("ClmNo") & "' AND GarageRef=" & Val(TPN.Text) & " AND MoClaimFile.ThirdParty=1")
        Else
            ExecSql("update MoClaimFile set GarageRef=" & TPN.Text & ",Total=" & CDbl(txtrep.Text) + CDbl(txtspare.Text) & ",EstimatedSpare=" & CDbl(txtspare.Text) & ",EstimatedRep = " & CDbl(txtrep.Text) & " WHERE ClmNo='" & Request("ClmNo") & "' AND GarageRef=" & Val(TPN.Text) & " AND MoClaimFile.ThirdParty=1")
        End If


        'lblUpdate.Text = "Row: " & e.Item.ItemIndex & " has been updated with values of: Name=" & Garage.TextBox.Value & " " & Garage.SelectedItem.Value & ", Email= " & txtspare.Text & ", Company= " & txtrep.Text
        lblUpdate.Visible = True
        GridView1.EditItemIndex = -1
        BindGrid()
    End Sub

    Private Sub GridView1_CancelCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GridView1.CancelCommand
        ' On a cancel request, return datagrid to non-edit mode.
        GridView1.EditItemIndex = -1
        BindGrid()
    End Sub

    Protected Function GetInitialDataSource(ByVal pageSize As Integer) As DataSet
        ' Called from within the aspx page using databind syntax
        GetInitialDataSource = GetComboDs(pageSize, "a", 0, "")

    End Function

    Protected Sub Garage_GetPage(ByVal sender As Object, ByVal e As eba.Web.ComboGetPageEventArgs)
        e.NextPage = GetComboDs(e.PageSize, e.SearchSubstring, e.StartingRecordIndex, e.LastString)
    End Sub

    Protected Sub GarageRef_GetPage(ByVal sender As Object, ByVal e As eba.Web.ComboGetPageEventArgs) Handles GarageRef.GetPage
        e.NextPage = RecSet("SELECT TOP " & e.PageSize & " TPName,TPNo FROM EXTRAINFO where TP='Garages' and  TPName > '" & e.LastString & "' AND TPName LIKE '" & e.SearchSubstring & "%' ORDER BY TPName", Conn)
    End Sub
    Protected Sub ClmType_GetPage(ByVal sender As Object, ByVal e As eba.Web.ComboGetPageEventArgs) Handles ClmType.GetPage
        e.NextPage = RecSet("SELECT TOP " & e.PageSize & " TPName,TPNo FROM EXTRAINFO where TP='MoClmType' and  TPName > '" & e.LastString & "' AND TPName LIKE '" & e.SearchSubstring & "%' ORDER BY TPName", Conn)
    End Sub


End Class
