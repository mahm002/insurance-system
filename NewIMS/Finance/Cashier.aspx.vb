Imports DevExpress.Web
Imports Microsoft.Reporting.WebForms

Public Class Cashier
    Inherits Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session.Remove("CurrentPolNo")
        Session.Remove("CurrentEndNo")
        Session.Remove("CurrentLoadNo")
        Session.Remove("PolicyData")
        ViewState.Clear()

        Dim myList = CType(Session("UserInfo"), List(Of String))
        If myList Is Nothing Then
            'ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('SystemManage/LogIn.aspx','_self'); </script>")
            FormsAuthentication.RedirectToLoginPage()
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 3)
        End If
        'If Not IsPostBack Then
        detailDataSource.SelectCommand = "SELECT PolicyFile.PolNo, PolicyFile.TOTPRM, PolicyFile.TOTPRM - ISNULL(PolicyFile.Inbox, 0) As Remain, ISNULL(PolicyFile.Inbox, 0) AS InBox, " _
            & "EXTRAINFO.TPName, CustomerFile.CustName, PolicyFile.IssuDate, PolicyFile.NETPRM, PolicyFile.Branch, " _
            & "ISNULL(PolicyFile.Stop, 0) AS Stop, " _
            & "CASE WHEN SubSysName IS NULL THEN BranchName +'/'+ 'أخرى' ELSE BranchName +'/'+ SubSysName END AS BR, " _
            & "CASE WHEN PolicyFile.PayType=1 THEN '0' ELSE PolicyFile.AccountNo END AS AccountN, " _
            & "CASE WHEN Commision<>0 and Broker<>0 then CAST(Commision AS NVARCHAR(100)) + iif(CommisionType=1,' '+EXTRAINFO.TPName,' %') +'-' + BrokersInfo.TPName else '0' END As Commissioned, " _
            & "PolicyFile.EndNo, PolicyFile.LoadNo, AccountFile.AccountName, " _
            & "RTRIM(PolicyFile.PolNo) + '&EndNo=' + RTRIM(PolicyFile.EndNo) + '&LoadNo=' + RTRIM(PolicyFile.LoadNo) + '&Sys=' + RTRIM(PolicyFile.SubIns) + '&AccNo=' + CASE WHEN PolicyFile.PayType=1 THEN '0' ELSE PolicyFile.AccountNo END  AS Pol" _
            & " FROM PolicyFile LEFT OUTER JOIN CustomerFile ON PolicyFile.CustNo = CustomerFile.CustNo " _
            & "LEFT OUTER JOIN BranchInfo ON PolicyFile.Branch = BranchInfo.BranchNo " _
            & "LEFT OUTER JOIN SUBSYSTEMS ON PolicyFile.SubIns = SUBSYSTEMS.SUBSYSNO AND SUBSYSTEMS.Branch = PolicyFile.Branch " _
            & "INNER JOIN EXTRAINFO ON EXTRAINFO.TP = 'Cur' AND EXTRAINFO.TPNo = PolicyFile.Currency LEFT OUTER JOIN " _
            & "EXTRAINFO As BrokersInfo on BrokersInfo.TP='Broker' and PolicyFile.Broker=BrokersInfo.TPNo " _
            & " LEFT OUTER JOIN AccountFile ON PolicyFile.IssueUser=AccountFile.AccountNo " _
            & "WHERE Inbox<>TOTPRM AND month(IssuDate)=month(@IssuDate) AND year(IssuDate)=year(@IssuDate) and PolicyFile.Branch=@Br AND PayType=@Ptyp and PolicyFile.Stop=0"
        detailDataSource.SelectParameters(0).DefaultValue = Today.Date
        detailDataSource.SelectParameters(1).DefaultValue = Session("Branch")
        detailDataSource.SelectParameters(2).DefaultValue = PayType.Value

        CurrentPays.SelectParameters(0).DefaultValue = Today.Date

        AccMoveGrid.DataBind()
        detailGrid.DataBind()
        detailGrid.GroupBy(detailGrid.Columns("BR"))
        AccMoveGrid.GroupBy(AccMoveGrid.Columns("BR"))

        detailGrid.ExpandAll()
        AccMoveGrid.ExpandAll()
        ASPxTextBox1.Text = GetBranchName(Session("Branch"))

        'End If
    End Sub

    Protected Sub CasherDay(source As Object, e As EventArgs)
        detailDataSource.SelectParameters(0).DefaultValue = dateEdit.Date
        detailDataSource.SelectParameters(1).DefaultValue = Session("Branch")
        CurrentPays.SelectParameters(0).DefaultValue = dateEdit.Date
        'CurrentPays.SelectParameters(1).DefaultValue = Session("Branch")
        detailGrid.DataBind()
        AccMoveGrid.DataBind()
        detailGrid.GroupBy(detailGrid.Columns("BR"))
        AccMoveGrid.GroupBy(AccMoveGrid.Columns("BR"))
        'detailGrid.set
        detailGrid.ExpandAll()
        AccMoveGrid.ExpandAll()
    End Sub

    Protected Sub Grid_HtmlRowPrepared(sender As Object, e As ASPxGridViewTableRowEventArgs)
        'If e.GetValue("TOTPRM") = 0 And e.RowType <> GridViewRowType.Group Then e.Row.BackColor = Drawing.Color.Blue
        'If e.GetValue("TOTPRM") <= 0 Or e.GetValue("Stop") Then e.Row.Enabled = False
    End Sub

    Protected Sub DateEdit_DateChanged(sender As Object, e As EventArgs) Handles dateEdit.DateChanged

    End Sub

    Protected Sub Filter(sender As Object, e As ASPxGridViewHeaderFilterEventArgs)
        If e.Column.Name = "PolNo" Then
            detailDataSource.SelectCommand = "select PolNo,TOTPRM-InBox as TOTPRM,PolicyFile.TOTPRM - ISNULL(PolicyFile.Inbox, 0) As Remain,InBox,TpName,CustName,IssuDate,NETPRM,SubIns,Stop,BranchName+SubSysName as BR,EndNo,LoadNo,rtrim(PolNo)+'&EndNo='+rtrim(EndNo)+'&LoadNo='+rtrim(LoadNo) as Pol" _
                     & ",rtrim(PolNo)+';'+rtrim(EndNo)+':'+rtrim(LoadNo) as Poll" _
                     & " from PolicyFile inner join Customerfile on PolicyFile.CustNo=CustomerFile.CustNo " _
                     & " inner join BranchInfo on Substring(polno,1,4)=BranchNo " _
                     & " inner join SubSystems on SubIns=SubSysNo and SubSystems.Branch=Branch " _
                     & " inner join ExtraInfo on TP='Cur' and TpNo=Currency " _
                     & " where PolNo='" & e.Values("PolNo").Value & "'"

            detailGrid.DataBind()
            detailGrid.GroupBy(detailGrid.Columns("BR"))
            ASPxTextBox1.Text = GetBranchName(Session("Branch"))

            'detailGrid.ExpandAll()

        End If
    End Sub

    Protected Sub DetailGrid_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs)
        If e.DataColumn.FieldName = "TOTPRM" Then
            If e.GetValue("TOTPRM") <> 0 Then
                e.Cell.ForeColor = Drawing.Color.Red
            Else
                e.Cell.ForeColor = Drawing.Color.Green
            End If

        End If
        If (e.GetValue("InBox") <> 0) Or e.GetValue("PolNo") = "أخـرى" Then
            If e.DataColumn.FieldName = "Pol" Then e.Cell.Enabled = False
            If e.DataColumn.FieldName = "Poll" Then e.Cell.Enabled = True
        Else
            If e.DataColumn.FieldName = "Poll" Then e.Cell.Enabled = False
        End If
        If e.GetValue("TOTPRM") <> e.GetValue("InBox") Then e.Cell.Enabled = True
    End Sub

    Private Sub AccMoveGrid_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs) Handles AccMoveGrid.CustomButtonCallback
        Select Case e.ButtonID
            Case "Print"
                Dim Report = ReportsPath & "Reciept"
                Dim ReciptNo = AccMoveGrid.GetRowValues(e.VisibleIndex, "Poll").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("ReciptNo", ReciptNo, False)
                }
                Session.Add("Parms", P)

                AccMoveGrid.JSProperties("cpMyAttribute") = "PRINT"
                AccMoveGrid.JSProperties("cpResult") = " - إيصال قبض - "
                AccMoveGrid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
            Case Else
                Exit Select
        End Select
    End Sub

    Protected Sub Search_TextChanged(sender As Object, e As EventArgs) Handles Search.TextChanged
        If Search.Text <> "" Then
            detailDataSource.SelectCommand = "select PolNo,TOTPRM-InBox as TOTPRM,PolicyFile.TOTPRM - ISNULL(PolicyFile.Inbox, 0) As Remain,InBox,TPName,CustName,IssuDate,NETPRM,SubIns,Stop,BranchName+SubSysName as BR,EndNo,LoadNo,rtrim(PolNo)+'&EndNo='+rtrim(EndNo)+'&LoadNo='+rtrim(LoadNo) as Pol" _
                 & ",rtrim(PolNo)+';'+rtrim(EndNo)+':'+rtrim(LoadNo) as Poll" _
                 & " from PolicyFile inner join Customerfile on PolicyFile.CustNo=CustomerFile.CustNo " _
                 & " inner join BranchInfo on PolicyFile.Branch=BranchInfo.BranchNo " _
                 & " inner join SubSystems on SubIns=SubSysNo and SubSystems.Branch=policyfile.Branch " _
                 & " inner join ExtraInfo on TP='Cur' and TpNo=Currency " _
                 & " where PolNo='" & Trim(Search.Text) & "'"
        Else
            detailDataSource.SelectCommand = "select PolNo,TOTPRM-InBox as TOTPRM,PolicyFile.TOTPRM - ISNULL(PolicyFile.Inbox, 0) As Remain,InBox,TPName,CustName,IssuDate,NETPRM,SubIns,Stop,BranchName+SubSysName as BR,EndNo,LoadNo,rtrim(PolNo)+'&EndNo='+rtrim(EndNo)+'&LoadNo='+rtrim(LoadNo) as Pol" _
            & ",rtrim(PolNo)+';'+rtrim(EndNo)+':'+rtrim(LoadNo) as Poll" _
            & " from PolicyFile inner join Customerfile on PolicyFile.CustNo=CustomerFile.CustNo " _
            & " inner join BranchInfo on PolicyFile.Branch=BranchInfo.BranchNo " _
            & " inner join SubSystems on SubIns=SubSysNo and SubSystems.Branch=policyfile.Branch " _
            & " inner join ExtraInfo on TP='Cur' and TpNo=Currency " _
            & " where month(IssuDate)=month(@IssuDate) and year(IssuDate)=year(@IssuDate) and PolicyFile.Branch=@Br and PolicyFile.Stop=0"
            detailDataSource.SelectParameters(0).DefaultValue = Today.Date
            detailDataSource.SelectParameters(1).DefaultValue = Session("Branch") 'IIf(Request("BR") = "01", "المركز الرئيسي", IIf(Request("BR") = "02", "فرع بنغازي", IIf(Request("BR") = "03", "فرع مصراتة", "فرع الزاوية")))
        End If
        'detailDataSource.SelectParameters(0).DefaultValue = Today.Date
        'detailDataSource.SelectParameters(1).DefaultValue = Request("BR") 'IIf(Request("BR") = "01", "المركز الرئيسي", IIf(Request("BR") = "02", "فرع بنغازي", IIf(Request("BR") = "03", "فرع مصراتة", "فرع الزاوية")))
        detailGrid.DataBind()
        detailGrid.GroupBy(detailGrid.Columns("BR"))
        ASPxTextBox1.Text = GetBranchName(Session("Branch"))
        'detailGrid.set
        detailGrid.ExpandAll()

    End Sub

    Protected Sub ASPxCallback1_Callback(source As Object, e As CallbackEventArgs) Handles ASPxCallback1.Callback
        Select Case e.Parameter
            Case "PrintCashierD"
                Dim Report = ReportsPath & "CasherList"
                'Dim ReciptNo = AccMoveGrid.GetRowValues(e.VisibleIndex, "Poll").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim D = IIf(IsDate(dateEdit.Value), dateEdit.Value, Today.Date)

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("CasherD", D.ToString, True),
                    New ReportParameter("CasherDT", D.ToString, True),
                    New ReportParameter("BR", Session("Branch").ToString, IsHeadQuarter(Session("Branch").ToString))
                }

                Session.Add("Parms", P)
                ASPxCallback1.JSProperties("cpMyAttribute") = "PRINTD"
                ASPxCallback1.JSProperties("cpResult") = " - كشف حركةالخزينة - "
                ASPxCallback1.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
            Case "ShowChasherInOrOut"
                Dim Report = ReportsPath & "CasherListInOut"
                'Dim ReciptNo = AccMoveGrid.GetRowValues(e.VisibleIndex, "Poll").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim D = IIf(IsDate(dateEdit.Value), dateEdit.Value, Today.Date)

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("CasherD", D.ToString, True),
                    New ReportParameter("CasherDT", D.ToString, True),
                    New ReportParameter("BR", Session("Branch").ToString, IsHeadQuarter(Session("Branch").ToString))
                }

                Session.Add("Parms", P)
                ASPxCallback1.JSProperties("cpMyAttribute") = "PRINTD"
                ASPxCallback1.JSProperties("cpResult") = " - كشف حركة الصادر والوارد - "
                ASPxCallback1.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
            Case "PrintUnPaidD"
                Dim Report = ReportsPath & "UnPaid"
                'Dim ReciptNo = AccMoveGrid.GetRowValues(e.VisibleIndex, "Poll").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim D = IIf(IsDate(dateEdit.Value), dateEdit.Value, Today.Date)

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("CasherDF", D.ToString, True),
                    New ReportParameter("CasherDT", D.ToString, True),
                    New ReportParameter("BR", Session("Branch").ToString, IsHeadQuarter(Session("Branch").ToString))
                }

                Session.Add("Parms", P)

                ASPxCallback1.JSProperties("cpMyAttribute") = "PRINTU"
                ASPxCallback1.JSProperties("cpResult") = " - كشف أقساط تحت التحصيل - "
                ASPxCallback1.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
            Case Else
                Exit Select
        End Select
    End Sub

End Class