Imports System.Data.SqlClient
Imports DevExpress.Web

Public Class InwardIssue
    Inherits Page

    Private Sub PolicyManagement_Inward_InwardIssue_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsCallback Then
        Else
            If Request("Mode") = "Edit" And Not IsPostBack() Then
                If Request("PolNo") <> "" Then Save.Visible = False
                FillFormData(Request("OrderNo"))
            Else

            End If
        End If

    End Sub

    Protected Sub Scbp_Callback(source As Object, e As CallbackEventArgs) Handles scbp.Callback
        Dim callback As ASPxCallback = TryCast(source, ASPxCallback)
        Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callback)
        'Dim tmp As String
        'If Not ASPxButton3.Visible Then
        'Else
        Select Case e.Parameter
            Case "ShareChange"
                'AcceptedSI.Value = AcceptedShare.Value / 100 * CDbl(MainSi.Value)
                'AcceptedNet.Value = AcceptedShare.Value / 100 * CDbl(MainNet.Value)
                'CommisionValue.Value = CommisionRatio.Value / 100 * AcceptedNet.Value
                'Balance.Value = AcceptedNet.Value - CommisionValue.Value
                e.Result = Convert.ToDecimal(AcceptedShare.Value) * Convert.ToDecimal(MainSi.Value) / 100
                    'GoTo 10
            Case "SaveData"
                'AcceptedSI.Value = AcceptedShare.Value / 100 * CDbl(MainSi.Value)
                'AcceptedNet.Value = AcceptedShare.Value / 100 * CDbl(MainNet.Value)
                If SlipN.Value = "0" Then
                    Parm = Array.CreateInstance(GetType(SqlParameter), 3)
                    SetPm("@Year", DbType.Date, Date.Today, Parm, 0)
                    SetPm("@Sys", DbType.String, Sys.Value, Parm, 1)
                    SetPm("@Type", DbType.String, "INW", Parm, 2)
                    e.Result = CallSP("InOutwardNo", Conn, Parm)
                Else

                    'SlipN.Value = tmp
                    'SetDxtxtValue(SlipN, CallSP("InOutwardNo", Conn, Parm))
                    'RefNo.Value = CallSP("InOutwardNo", Conn, Parm)

                    'ElseIf Right(Trim(SlipN.Value), 2) <> Sys.Value Then
                    '    'SetDxtxtValue(RefNo, RefNo.Value.Replace(Right(Trim(RefNo.Value), 2), Sys.Value))
                    '    'ExecConn(MainDataUpdate(SlipNo.Text) & "; " & SubfileUpdate(SlipNo.Value), Conn)
                    '    'RefNo.Value = RefNo.Value.Replace(Right(Trim(RefNo.Value), 2), Sys.Value)
                    'Else
                    ExecConn(MainDataUpdate(SlipN.Value) & "; " & SubfileUpdate(SlipN.Value), Conn)
                End If
            Case "InsertData"
                If SlipN.Value <> "0" Then
                    ExecConn(MainDataInsertText() & "; " & SubfileInsert(), Conn)
                End If

            Case Else
                Exit Select
                'ASPxButton3.Enabled = False
        End Select
        'End If
    End Sub

    'Protected Sub Scbp_Callback(source As Object, e As CallbackEventArgs)

    'End Sub

    Protected Function MainDataInsertText() As String
        Return "insert into policyFile(OrderNo,EndNo,LoadNo,CustNo,OwnNo,SubIns,Branch,EntryDate,CoverFrom,CoverTo,Currency,ExcRate,PayType,OldPolicy," _
                           & "NETPRM, TOTPRM, Commision,UserName) values('" _
                           & Trim(SlipN.Value) & "'," _
                           & EndNo.Value & "," _
                           & LoadNo.Value & "," _
                           & CustNo.Value & "," _
                           & OwnNo.Value & ",'" _
                           & Sys.Value & "','IW'," _
                           & "CONVERT(DATETIME,'" & Format(CDate(EntryDate.Value), "yyyy-MM-dd") & " 00:00:00',102)," _
                           & "CONVERT(DATETIME,'" & Format(CDate(CoverFrom.Value), "yyyy-MM-dd") & " 00:00:00',102)," _
                           & "CONVERT(DATETIME,'" & Format(CDate(CoverTo.Value), "yyyy-MM-dd") & " 00:00:00',102)," _
                           & 1 & "," _
                           & 1 & "," _
                           & PayType.Value & ",'" _
                           & PolNo.Value & "'," _
                           & CDbl(AcceptedShare.Value / 100 * CDbl(MainNet.Value)) & "," _
                           & CDbl(AcceptedShare.Value / 100 * CDbl(MainNet.Value)) & "," _
                           & CDbl(CommisionRatio.Value) & ",'" _
                           & MyBase.Session("UserID") & "')"
    End Function

    Protected Function SubfileInsert() As String
        Return " INSERT INTO LocalAcceptance (OrderNo,Date,IssueDate,CoverFrom,CoverTo,EndNo,LoadNo,SubIns,RiskDiscription,PolicyHolder,InsuranceCo,SumIns100,Premium100,AcceptedShare ,AcceptedSumIns, AcceptedPremium,CommisionRatio,Balance,CommisionValue) " _
                           & " VALUES ('" _
                           & Trim(SlipN.Value) & "'," _
                           & "CONVERT(DATETIME,'" & Format(Date.Today, "yyyy-MM-dd") & " 00:00:00',102)," _
                           & "CONVERT(DATETIME,'" & Format(CDate(EntryDate.Value), "yyyy-MM-dd") & " 00:00:00',102)," _
                           & "CONVERT(DATETIME,'" & Format(CDate(CoverFrom.Value), "yyyy-MM-dd") & " 00:00:00',102)," _
                           & "CONVERT(DATETIME,'" & Format(CDate(CoverTo.Value), "yyyy-MM-dd") & " 00:00:00',102)," _
                           & EndNo.Value & "," _
                           & LoadNo.Value & ",'" _
                           & Sys.Value & "','" _
                           & RiskDiscription.Value & "'," _
                           & CustNo.Value & "," _
                           & OwnNo.Value & "," _
                           & CDbl(MainSi.Value) & "," _
                           & CDbl(MainNet.Value) & "," _
                           & CDbl(AcceptedShare.Value) & "," _
                           & CDbl(AcceptedShare.Value / 100 * CDbl(MainSi.Value)) & "," _
                           & CDbl(AcceptedShare.Value / 100 * CDbl(MainNet.Value)) & "," _
                           & CDbl(CommisionRatio.Value) & "," _
                           & CDbl(Balance.Value) & "," _
                           & CDbl(CommisionValue.Value) & ")"
    End Function

    Protected Function MainDataUpdate(Ref As String) As String

        Return "Update policyFile set " _
                           & "OrderNo='" & Trim(Ref) & "', " _
                           & "EndNo=" & EndNo.Value & ", " _
                           & "LoadNo=" & LoadNo.Value & ", " _
                           & "CustNo=" & CustNo.Value & ", " _
                           & "OwnNo=" & OwnNo.Value & ", " _
                           & "SubIns='" & Sys.Value & "', " _
                           & "CoverFrom=" & "CONVERT(DATETIME,'" & Format(CDate(CoverFrom.Value), "yyyy-MM-dd") & " 00:00:00',102)," _
                           & "CoverTo=" & "CONVERT(DATETIME,'" & Format(CDate(CoverTo.Value), "yyyy-MM-dd") & " 00:00:00',102)," _
                           & "PayType=" & PayType.Value & "," _
                           & "OldPolicy='" & PolNo.Text & "', " _
                           & "NETPRM=" & (AcceptedShare.Value / 100 * CDbl(MainNet.Value)) & ", " _
                           & "TOTPRM=" & (AcceptedShare.Value / 100 * CDbl(MainNet.Value)) & ", " _
                           & "Commision=" & CDbl(CommisionRatio.Value) & " " _
                           & "Where OrderNo='" & Trim(Ref) & "'"
    End Function

    Protected Function SubfileUpdate(Ref As String) As String
        'Dim Newref As String = Ref.Replace(Right(Trim(Ref), 2), Trim(Sys.Value))
        Return "Update LocalAcceptance Set " _
                           & "OrderNo='" & Trim(Ref) & "', " _
                           & "EndNo=" & EndNo.Text & ", " _
                           & "LoadNo=" & LoadNo.Text & ", " _
                           & "PolicyHolder=" & CustNo.Value & ", " _
                           & "InsuranceCo=" & OwnNo.Value & ", " _
                           & "SubIns='" & Sys.Value & "', " _
                           & "RiskDiscription='" & RiskDiscription.Value & "', " _
                           & "CoverFrom=" & "CONVERT(DATETIME,'" & Format(CDate(CoverFrom.Value), "yyyy-MM-dd") & " 00:00:00',102)," _
                           & "CoverTo=" & "CONVERT(DATETIME,'" & Format(CDate(CoverTo.Value), "yyyy-MM-dd") & " 00:00:00',102)," _
                           & "SumIns100=" & CDbl(MainSi.Value) & ", " _
                           & "Premium100=" & CDbl(MainNet.Value) & ", " _
                           & "AcceptedSumIns=" & (AcceptedShare.Value / 100 * CDbl(MainSi.Value)) & ", " _
                           & "AcceptedPremium=" & (AcceptedShare.Value / 100 * CDbl(MainNet.Value)) & ", " _
                           & "CommisionRatio=" & CommisionRatio.Value & ", " _
                           & "CommisionValue=" & CommisionValue.Value & ", " _
                           & "Balance=" & Balance.Value & ", " _
                           & "AcceptedShare=" & AcceptedShare.Value & " " _
                           & "Where OrderNo='" & Trim(Ref) & "'"
    End Function

    Protected Sub FillFormData(Ref As String)
        Dim InWardSlip As New DataSet
        Dim IWadaptet = New SqlDataAdapter("SELECT PolicyFile.PolNo,LocalAcceptance.CoverFrom,LocalAcceptance.CoverTo, PolicyFile.OrderNo, PolicyFile.CustNo,PolicyFile.OwnNo," _
                                            & "PolicyFile.EndNo,PolicyFile.OldPolicy as originalPolicy, PolicyFile.LoadNo,PolicyFile.SubIns, " _
                                            & "LocalAcceptance.SumIns100, PolicyFile.Commision,PolicyFile.paytype," _
                                            & "LocalAcceptance.Premium100, LocalAcceptance.AcceptedSumIns, LocalAcceptance.AcceptedPremium,LocalAcceptance.CommisionRatio,LocalAcceptance.CommisionValue, " _
                                            & "LocalAcceptance.AcceptedShare,LocalAcceptance.Balance,LocalAcceptance.RiskDiscription,LocalAcceptance.IssueDate as OriginalIssDate, " _
                                            & "(SELECT CustName FROM CustomerFile " _
                                            & "WHERE  (PolicyFile.CustNo = CustNo)) As Holder, " _
                                            & "(SELECT CustName FROM  CustomerFile AS CustomerFile_1 " _
                                            & " WHERE  (PolicyFile.OwnNo = CustNo)) AS InsuranceCompany, " _
                                            & "(SELECT SUBSYSNAME FROM SUBSYSTEMS WHERE (PolicyFile.SubIns = SUBSYSNO) And (Branch =dbo.MainCenter())) AS Insurancetype " _
                                            & "FROM PolicyFile INNER JOIN LocalAcceptance ON PolicyFile.OrderNo = LocalAcceptance.OrderNo AND " _
                                            & "PolicyFile.EndNo = LocalAcceptance.EndNo And PolicyFile.LoadNo = LocalAcceptance.LoadNo And " _
                                            & "PolicyFile.SubIns = LocalAcceptance.SubIns " _
                                            & "WHERE (PolicyFile.OrderNo ='" & Trim(Ref) & "')", Conn)
        IWadaptet.Fill(InWardSlip)

        PolNo.Value = InWardSlip.Tables(0).Rows.Item(0)("originalPolicy")
        EndNo.Value = InWardSlip.Tables(0).Rows.Item(0)("EndNo")
        LoadNo.Value = InWardSlip.Tables(0).Rows.Item(0)("LoadNo")
        TryCast(CustNo, ASPxGridLookup).Value = CInt(InWardSlip.Tables(0).Rows.Item(0).Item("CustNo"))
        TryCast(OwnNo, ASPxGridLookup).Value = CInt(InWardSlip.Tables(0).Rows.Item(0).Item("OwnNo"))
        TryCast(Sys, ASPxGridLookup).Value = InWardSlip.Tables(0).Rows.Item(0)("SubIns")
        RiskDiscription.Value = InWardSlip.Tables(0).Rows.Item(0)("RiskDiscription")
        EntryDate.Date = InWardSlip.Tables(0).Rows.Item(0)("OriginalIssDate")
        CoverFrom.Date = InWardSlip.Tables(0).Rows.Item(0)("CoverFrom")
        CoverTo.Date = InWardSlip.Tables(0).Rows.Item(0)("CoverTo")
        MainSi.Value = InWardSlip.Tables(0).Rows.Item(0)("SumIns100")
        MainNet.Value = InWardSlip.Tables(0).Rows.Item(0)("Premium100")
        AcceptedShare.Value = InWardSlip.Tables(0).Rows.Item(0)("AcceptedShare")
        AcceptedSI.Text = InWardSlip.Tables(0).Rows.Item(0)("AcceptedSumIns")
        AcceptedNet.Text = InWardSlip.Tables(0).Rows.Item(0)("AcceptedPremium")
        CommisionRatio.Value = InWardSlip.Tables(0).Rows.Item(0)("CommisionRatio")
        CommisionValue.Value = InWardSlip.Tables(0).Rows.Item(0)("CommisionValue")
        Balance.Value = InWardSlip.Tables(0).Rows.Item(0)("Balance")
        PayType.SelectedIndex = InWardSlip.Tables(0).Rows.Item(0)("paytype")
        SlipN.Value = InWardSlip.Tables(0).Rows.Item(0)("OrderNo")
    End Sub

End Class