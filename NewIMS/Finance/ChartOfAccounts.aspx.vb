Imports DevExpress.Utils
Imports DevExpress.Web
Imports DevExpress.Web.ASPxTreeList
Imports Microsoft.Reporting.WebForms

Public Class ChartOfAccounts
    Inherits Page
    Dim cnt As Int16

    Private ReadOnly Property IsBR As Boolean
        Get
            Dim cacheKey As String = "IsBR_" & Session.SessionID
            Dim cachedValue As Object = HttpContext.Current.Cache(cacheKey)
            If cachedValue IsNot Nothing Then Return cachedValue

            Dim result As Boolean = IsHeadQuarter(Session("Branch")) ' Your existing logic
            HttpContext.Current.Cache.Insert(cacheKey, result, Nothing, Date.Now.AddMinutes(30), Cache.NoSlidingExpiration)
            Return result
        End Get
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = TryCast(Session("UserInfo"), List(Of String))
        'If myList Is Nothing Then Exit Sub
        If myList Is Nothing Then

            FormsAuthentication.RedirectToLoginPage()
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 3)

        End If

        If IsPostBack And txtSearch.Text.Trim = "" Then

            SqlDataSource1.DataBind()
            'COA.CollapseAll()
        End If

        COA.SettingsEditing.AllowNodeDragDrop = False

    End Sub

    Private Sub COA_CommandColumnButtonInitialize(sender As Object, e As TreeListCommandColumnButtonEventArgs) Handles COA.CommandColumnButtonInitialize

        'If e.ButtonType <> TreeListCommandColumnButtonType.Delete Then
        '    Return;

        'If Convert.ToInt32(((DataRowView)ASPxTreeList1.FindNodeByKeyValue(e.NodeKey).DataItem) Then["EmployeeID"])%2=1
        '    e.Visible = DefaultBoolean.False

        'Dim node As TreeListNode = COA.FindNodeByKeyValue(e.NodeKey.ToString())
        Dim nodeKey As String = e.NodeKey.ToString()
        Dim node As TreeListNode = COA.FindNodeByKeyValue(nodeKey)
        'If node Is Nothing Then Return
        'Dim BTYP = e.ButtonType
        'Dim nK = node.Key
        'node.HasChildren Or
        'e.CustomButtonIndex 0 add
        'e.CustomButtonIndex 1 edit
        'e.CustomButtonIndex 2 delete
        'e.CustomButtonIndex 3 ledger

        If node.Level <= 3 Then
            If e.CustomButtonIndex = -1 Then
                e.Visible = DefaultBoolean.False
            Else
                Select Case e.CustomButtonIndex
                    Case 0
                        e.Visible = IIf(IsHeadQuarter(Session("Branch")), DefaultBoolean.True, DefaultBoolean.False)
                    Case 1
                        e.Visible = IIf(IsHeadQuarter(Session("Branch")), DefaultBoolean.True, DefaultBoolean.False)
                    Case 2
                        e.Visible = DefaultBoolean.False
                    Case 3
                        e.Visible = DefaultBoolean.False
                    Case Else
                        Exit Select
                End Select
            End If
        Else
            Select Case node.Level
                Case >= 4 And IsRootAccount(node.Key)
                    If Istranscationed(node.Key) Then
                        Select Case e.CustomButtonIndex
                            Case 0
                                e.Visible = DefaultBoolean.False
                            Case 1
                                e.Visible = DefaultBoolean.False
                            Case 2
                                e.Visible = DefaultBoolean.False
                            Case 3
                                e.Visible = DefaultBoolean.True
                            Case Else
                                Exit Select
                        End Select
                    Else
                        Select Case e.CustomButtonIndex
                            Case 0
                                e.Visible = IIf(IsHeadQuarter(Session("Branch")), DefaultBoolean.True, DefaultBoolean.False)
                            Case 1
                                e.Visible = IIf(IsHeadQuarter(Session("Branch")), DefaultBoolean.True, DefaultBoolean.False)
                            Case 2
                                e.Visible = IIf(node.HasChildren, DefaultBoolean.False, IIf(IsHeadQuarter(Session("Branch")), DefaultBoolean.True, DefaultBoolean.False))
                            Case 3
                                e.Visible = DefaultBoolean.False
                            Case Else
                                Exit Select
                        End Select
                    End If

                Case Else
                    Exit Select

            End Select

        End If

    End Sub

    Public Sub CBL_Callback(source As Object, e As CallbackEventArgs) Handles CBL.Callback
        Dim cmbsplited = e.Parameter.Split("|")
        Select Case cmbsplited(0)
            Case "Ledger"
                Dim Report = ReportsPath & "GeneralLedger"

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("DFrom", Format(Today.AddMonths(-1), "yyyy/MM/dd"), True),
                    New ReportParameter("DTo", Format(Today.Date, "yyyy/MM/dd").ToString, True),
                    New ReportParameter("AccFrom", cmbsplited(1).ToString, True),
                    New ReportParameter("AccTo", cmbsplited(1).ToString, True),
                    New ReportParameter("User", Session("User").ToString, False),
                    New ReportParameter("B1", Session("Branch").ToString, IIf(IsHeadQuarter(Session("Branch")), True, False))
                     }
                Session.Add("Parms", P)
                'CBL.JSProperties()
                CBL.JSProperties("cpMyAttribute") = "PrintLedger"
                CBL.JSProperties("cpResult") = " كشف حساب "
                CBL.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
            Case Else
                Exit Select
        End Select
    End Sub

    Private Sub COA_BeforeGetCallbackResult(sender As Object, e As EventArgs) Handles COA.BeforeGetCallbackResult
        If Not String.IsNullOrEmpty(COA.SearchPanelFilter) Then
            COA.ExpandAll()
        Else
            'COA.CollapseAll()
        End If
    End Sub

    Private Sub COA_NodeExpanded(sender As Object, e As TreeListNodeEventArgs) Handles COA.NodeExpanded
        If String.IsNullOrEmpty(COA.SearchPanelFilter) Then
            'COA.ExpandAll()
        Else
            'COA.CollapseAll()
        End If
    End Sub

    Private Sub COA_ToolbarItemClick(source As Object, e As TreeListToolbarItemClickEventArgs) Handles COA.ToolbarItemClick
        Dim Accs As String = String.Empty
        Select Case e.Item.Name
            Case "CustomLedger"
                'Dim unused As List(Of TreeListNode) = COA.GetSelectedNodes()
                'Dim listSelectedNodes As New List(Of TreeListNode)()
                'listSelectedNodes = COA.GetSelectedNodes()

                If COA.GetSelectedNodes().Count <> 0 Then
                    For Each node As TreeListNode In COA.GetSelectedNodes()
                        'node.GetValue("ReportGroupID")
                        If IsTransAccount(node.Key) Then
                            Accs += node.Key & ","
                        Else

                        End If
                    Next
                Else
                End If

                Dim Report = ReportsPath & "GeneralLedgerBulk"

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("DFrom", Format(Today.AddMonths(-1), "yyyy/MM/dd"), True),
                    New ReportParameter("DTo", Format(Today.Date, "yyyy/MM/dd").ToString, True),
                    New ReportParameter("Accs", Accs, True),
                    New ReportParameter("User", Session("User").ToString, False),
                    New ReportParameter("B1", Session("Branch").ToString, IIf(IsHeadQuarter(Session("Branch")), True, False))
                     }

                Session.Add("Parms", P)
                'CBL.JSProperties()
                COA.JSProperties("cpMyAttribute") = "PrintLedgerBulk"
                COA.JSProperties("cpResult") = "كشف حساب مخصص "
                COA.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
            Case Else
                Exit Select
        End Select

    End Sub

End Class