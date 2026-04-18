Imports System.Data.SqlClient
Imports DevExpress.Web

Public Class MedicalRespSpecial
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Specialize.Value = 44 Then
            Premium.ClientEnabled = True
            CustomSP.ClientEnabled = True
        Else
            Premium.ClientEnabled = False
            CustomSP.ClientEnabled = False

        End If
    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback

        If IsIssued(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")), Request("Sys")) Then
            Exit Sub
        Else
            Dim callbackPanel As ASPxCallbackPanel = CType(sender, ASPxCallbackPanel)
            Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)
            Dim T As Boolean
            Dim diff As Double = DateDiff(DateInterval.Year, GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")), GetDateedit(FindControlRecursive(PolicyControl, "CoverTo"))) ' / DateDiff(DateInterval.Day, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))), DateAdd(DateInterval.Year, 1, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))))), "0.000"))
            Select Case e.Parameter

                Case "Calc"

                    If isValid Then

                        Premium.Text = Format(GetNet(Specialize.Value) * diff, "0.000")
                        If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = 0 Then
                            T = True
                            Premium.Text = Format(GetNet(Specialize.Value) * diff, "0.000")
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(GetNet(Specialize.Value) * diff, "0.000"))
                            'SetTextValue(pageFooter.FindControl("NETPRM"), Format(Net - GetLastNet(GetTextValue(pageFooter.FindControl("PolNo")), GetTextValue(pageFooter.FindControl("EndNo"))), "###,###,##.000"))
                        Else
                            T = False
                            Premium.Text = Format(GetNet(Specialize.Value) * diff, "0.000") 'GetNet(GetEbaValue(PermType, 1), Val(Power.Text), Val(PassNo.Text), Val(Carry.Text))
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(Val(Premium.Text) - GetLastNet(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) - 1, GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), "###,##.000"))
                            'SetTextValue(pageFooter.FindControl("NETPRM"), Format(0, "###,###,##.000"))
                        End If

                        MainCalcDx(PolicyControl, GetDxTextValue(FindControlRecursive(PolicyControl, "NETPRM")) _
                             , Request("sys"), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) _
                             , Val(GetSpCase(GethiddenField(FindControlRecursive(PolicyControl, "CustNo")))) _
                             , False)
                        If GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) = "0" And Request("Order") = "" Then
                            Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                            SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                            SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 1)
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "OrderNo"), CallSP("LastOrderNo", Conn, Parm))

                            'insert new data requested
                            ExecConn(InsertData(Callback, Request("Sys"),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) _
                         + "; " + InsertPolicyData(PolicyControl, Request("Sys"), Session("Branch")),
                         Conn)
                        Else
                            'Update data requested
                            ExecConn(UpdateData(Callback,
                                    Request("Sys"),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) + "; " +
                           UpdatePolicyData(PolicyControl, Request("Sys"), Session("Branch"),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), Conn)
                        End If
                    Else
                    End If
                Case "Endorsment"
                    SetDxtxtValue(FindControlRecursive(PolicyControl, "EndNo"), GetLastEnd(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), 0) + 1)
                    FindControlRecursive(PolicyControl, "Endorsment").Visible = False
                    'Notes.Text = ""
                Case "ExRate"

                    SetDxtxtValue(FindControlRecursive(PolicyControl, "ExcRate"), GetExrate(GetComboValue(FindControlRecursive(PolicyControl, "Currency")), GetComboText(FindControlRecursive(PolicyControl, "Currency"))))

                Case "PayType"

                    If GetComboValue(FindControlRecursive(PolicyControl, "PayType")) = 2 And GethiddenField(FindControlRecursive(PolicyControl, "CustNo")) <> 0 Then
                        Dim AccountNo As New DataSet

                        Dim dbadapter = New SqlDataAdapter("select AccNo from Customerfile where CustNo=" & GethiddenField(FindControlRecursive(PolicyControl, "CustNo")), Conn)
                        dbadapter.Fill(AccountNo)
                        'AccountNo = RecSet("select AccNo from Customerfile where CustNo=" & CustNo.Text, Conn)
                        If Not AccountNo.Tables(0).Rows.Item(0).IsNull(0) Then
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "AccountNo"), AccountNo.Tables(0).Rows(0)(0))
                            'AccNo.Text = AccountNo.Tables(0).Rows(0)(0)
                        Else
                            SetComboIndex(FindControlRecursive(PolicyControl, "PayType"), 0)
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "AccountNo"), 0)
                            'MsgBox.confirm("! لايوجد رقم حساب لهذا الزبون :" & OrderNo.Text & " هل تريد تسجيل رقم حساب له ?", "AccNo_request")
                        End If
                    Else
                        SetComboIndex(FindControlRecursive(PolicyControl, "PayType"), 0)
                        SetDxtxtValue(FindControlRecursive(PolicyControl, "AccountNo"), 0)
                    End If

                Case Else
                    Exit Select
            End Select
            If e.Parameter = "Calc" Then
                'LoadUserControl()
                'MsgBox("hhh")

                'MsgBox(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value).ToString)
            Else
                'UnloadUserControl()
                Session.Remove("Loaded")
            End If
        End If
    End Sub

    Protected Function GetNet(Sp As Integer) As Double
        Dim Net As New DataSet
        If Sp <> 44 Then
            Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                If con.State = ConnectionState.Open Then
                    con.Close()
                Else

                End If
                con.Open()
                Dim dbadapter = New SqlDataAdapter("Select Rate " _
                   & " From MedicalRespRate " _
                   & "Where (Special = @Sp) ", con)
                dbadapter.SelectCommand.Parameters.AddWithValue("@Sp", Sp)
                dbadapter.Fill(Net)

                If Net.Tables(0).Rows.Count <> 0 Then

                    GetNet = Net.Tables(0).Rows.Item(0).Item(0)
                Else
                    GetNet = "لا توجد تسعيرة حسب المدخلات"
                End If

                con.Close()
            End Using
        Else
            GetNet = Premium.Value
        End If
        'Conn.Close()
    End Function

    Protected Sub Specialize_SelectedIndexChanged(sender As Object, e As EventArgs)
        If Specialize.Value = 44 Then
            Premium.ClientEnabled = True
        Else
            Premium.ClientEnabled = False
        End If
    End Sub

End Class