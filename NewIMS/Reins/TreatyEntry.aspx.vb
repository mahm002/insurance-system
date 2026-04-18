Imports System.Data.SqlClient
Imports DevExpress.Web

Public Class TreatyEntry
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Participants.DataBind()
        Reinsurers.DataBind()
    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback
        'Dim myList = CType(Session("UserInfo"), List(Of String))
        Select Case e.Parameter
            Case "GetData"
                Dim Treaty As New DataSet
                Dim dbadapter = New SqlDataAdapter("Select * From TRREGFLE where TreatyNo='" & TreatyNo.Value & "'", Conn)
                dbadapter.Fill(Treaty)
                If Treaty.Tables(0).Rows.Count <> 0 Then
                    Dim day1 As DateTime = New DateTime(Request("Year"), 1, 1)
                    Dim day365 As DateTime = New DateTime(Request("Year"), 12, 31)

                    'TreatyDate.va = ""
                    TRINSDTE.Value = day1
                    TREXPDTE.Value = day365
                    TRSYSDTE.Value = day1
                    'TreatyDate.Text = Format(Treaty.Tables(0).Rows(0)("TRSYSDTE"), "yyyy/MM/dd")
                    Descrip.Value = IIf(IsDBNull(Treaty.Tables(0).Rows(0)("Descrip")), "/", Treaty.Tables(0).Rows(0)("Descrip"))
                    AccType.SelectedIndex = IIf(IsDBNull(Treaty.Tables(0).Rows(0)("AccType")), 0, Treaty.Tables(0).Rows(0)("Acctype") - 1)
                    Portfolio.SelectedIndex = IIf(IsDBNull(Treaty.Tables(0).Rows(0)("PortFolio")), 0, Treaty.Tables(0).Rows(0)("PortFolio") - 1)
                    'TReatyFrom.Text = Format(Treaty.Tables(0).Rows(0)("TRINSDTE"), "yyyy/MM/dd")
                    'TreatyTo.Text = Format(Treaty.Tables(0).Rows(0)("TREXPDTE"), "yyyy/MM/dd")
                    TRCAPCTY.Value = Treaty.Tables(0).Rows(0)("TRCAPCTY")
                    Ret.Value = Treaty.Tables(0).Rows(0)("TRRETAMT")
                    QS.Value = Treaty.Tables(0).Rows(0)("TRQSRAMT")
                    QSCom.Value = Treaty.Tables(0).Rows(0)("TRQSRCOM").ToString + " %"
                    FSup.Value = Treaty.Tables(0).Rows(0)("TR1STAMT")
                    FSupCom.Value = Treaty.Tables(0).Rows(0)("TR1STCOM").ToString + " %"
                    SSup.Value = Treaty.Tables(0).Rows(0)("TR2STAMT")
                    SSupCom.Value = Treaty.Tables(0).Rows(0)("TR2STCOM").ToString + " %"
                    LQSCom.Value = Treaty.Tables(0).Rows(0)("TRLQSRCOM").ToString + " %"
                    LFSupCom.Value = Treaty.Tables(0).Rows(0)("TRL1STCOM").ToString + " %"
                    LSSupCom.Value = Treaty.Tables(0).Rows(0)("TRL2STCOM").ToString + " %"
                    WQSCom.Value = Treaty.Tables(0).Rows(0)("TRWQSRCOM").ToString + " %"
                    WFSupCom.Value = Treaty.Tables(0).Rows(0)("TRW1STCOM").ToString + " %"
                    ReserveR.Value = Treaty.Tables(0).Rows(0)("ReserveR").ToString + " %"
                    LSCom.Value = IIf(IsDBNull(Treaty.Tables(0).Rows(0)("TRLSCOMM")), "0 %", Treaty.Tables(0).Rows(0)("TRLSCOMM").ToString + " %")
                    LS.Value = Treaty.Tables(0).Rows(0)("TRLSAMT")
                    InterestRRes.Value = Treaty.Tables(0).Rows(0)("InterestRRes").ToString + " %"
                    LAR.Value = Treaty.Tables(0).Rows(0)("LAR").ToString + " %"
                Else
                    Dim day1 As DateTime = New DateTime(Request("Year"), 1, 1)
                    Dim day365 As DateTime = New DateTime(Request("Year"), 12, 31)

                    'TreatyDate.va = ""
                    TRINSDTE.Value = day1
                    TREXPDTE.Value = day365
                    TRSYSDTE.Value = day1
                    Descrip.Value = "/"
                    AccType.SelectedIndex = -1
                    Portfolio.SelectedIndex = -1
                    TRCAPCTY.Value = 0
                    Ret.Value = 0
                    QS.Value = 0
                    QSCom.Value = 0
                    FSup.Value = 0
                    FSupCom.Value = 0
                    SSup.Value = 0
                    SSupCom.Value = 0
                    LQSCom.Value = 0
                    LFSupCom.Value = 0
                    LSSupCom.Value = 0
                    WQSCom.Value = 0
                    WFSupCom.Value = 0
                    InterestRRes.Value = 0
                    LSCom.Value = 0
                    LS.Value = 0
                    ReserveR.Value = 0
                    LAR.Value = 0
                End If
            Case "SaveData"
                If CDbl(TRCAPCTY.Value) = CDbl(Ret.Value) + CDbl(QS.Value) + CDbl(FSup.Value) Then
                    ExecConn("DELETE FROM TRREGFLE  WHERE TreatyNo='" & TreatyNo.Text & "'; " _
                    & "INSERT INTO [TRREGFLE]([TreatyNo],[Descrip],[Acctype],[Portfolio],[ReserveR],[TRSYSDTE],[TRINSDTE],[TREXPDTE],[TRCAPCTY],[TRRETAMT],[TRQSRAMT]" _
                    & ",[TRQSRCOM],[TR1STAMT],[TR1STCOM],[TR2STAMT],[TR2STCOM],[TRLQSRCOM],[TRL1STCOM],[TRL2STCOM],[TRWQSRCOM],[TRW1STCOM],[InterestRRes],[TRLSAMT],[TRLSCOMM],[LAR])" _
                    & " VALUES('" _
                    & Trim(TreatyNo.Value) & "','" _
                    & Descrip.Value & "'," _
                    & AccType.Value & "," _
                    & Portfolio.Value & "," _
                    & Val(ReserveR.Value.Replace("%", "")) & "," _
                    & "CONVERT(DATETIME,'" & Format(CDate(IIf(IsDate(TRSYSDTE.Value), TRSYSDTE.Value, Today.Date)), "yyyy-MM-dd") & " 00:00:00',102)" & "," _
                    & "CONVERT(DATETIME,'" & Format(CDate(TRINSDTE.Value), "yyyy-MM-dd") & " " & Format(Now, "hh:mm") & "',102)," _
                    & "CONVERT(DATETIME,'" & Format(CDate(TRINSDTE.Value), "yyyy-MM-dd") & " " & Format(Now, "hh:mm") & "',102)," _
                    & Val(TRCAPCTY.Value) & "," _
                    & Val(Ret.Value) & "," _
                    & Val(QS.Value) & "," _
                    & Val(QSCom.Value.Replace("%", "")) & "," _
                    & Val(FSup.Value) & "," _
                    & Val(FSupCom.Value.Replace("%", "")) & "," _
                    & Val(SSup.Value) & "," _
                    & Val(SSupCom.Value.Replace("%", "")) & "," & Val(LQSCom.Value.Replace("%", "")) & "," & Val(LFSupCom.Value.Replace("%", "")) & "," _
                    & Val(LSSupCom.Value.Replace("%", "")) & "," & Val(WQSCom.Value.Replace("%", "")) & "," & Val(WFSupCom.Value.Replace("%", "")) & "," & Val(InterestRRes.Value.Replace("%", "")) & "," & Val(LS.Value) & "," & Val(LSCom.Value.Replace("%", "")) & "," & Val(LAR.Value.Replace("%", "")) & ")", Conn)

                    Dim day1 As New DateTime(Request("Year"), 1, 1)
                    Dim day365 As New DateTime(Request("Year"), 12, 31)
                    Covers.SelectedIndex = -1
                    TreatyNo.Value = ""
                    Descrip.Value = "/"
                    AccType.SelectedIndex = -1
                    Portfolio.SelectedIndex = -1
                    TRCAPCTY.Value = 0
                    Ret.Value = 0
                    QS.Value = 0
                    QSCom.Value = 0
                    FSup.Value = 0
                    FSupCom.Value = 0
                    SSup.Value = 0
                    SSupCom.Value = 0
                    LQSCom.Value = 0
                    LFSupCom.Value = 0
                    LSSupCom.Value = 0
                    WQSCom.Value = 0
                    WFSupCom.Value = 0
                    InterestRRes.Value = 0
                    LSCom.Value = 0
                    LS.Value = 0
                    ReserveR.Value = 0
                    LAR.Value = 0
                Else
                    TRCAPCTY.Focus()
                End If

            Case Else
                Exit Select
        End Select
    End Sub

End Class