
Imports System.Data.SqlClient
    Imports System.Web.Script.Serialization
    Imports DevExpress.Web

Public Class PolicyUC

    Inherits UserControl

    Private ReadOnly Property ConnectionString() As String
        Get
            Return ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString
        End Get
    End Property

    Private PolCoverFrom, PolCoverTo As Date

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request("OrderNo") <> "" Then
                RetrieveData()
            Else

            End If
            'If Request("EndNo") = EndNo.Text Then
            SpecifyRestrictions(IIf(Request("OrderNo") Is Nothing, "", Request("OrderNo")), EndNo.Text)

        Else

        End If

        If PolNo.Value = "" Then
        Else
            Dim DT As New DataSet
            Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                If con.State = ConnectionState.Open Then
                    con.Close()
                Else

                End If
                con.Open()
                Dim dbadapter = New SqlDataAdapter(String.Format("SELECT CoverFrom, CoverTo From PolicyFile where PolNo='{0}' and EndNo=0", PolNo.Value), con)
                dbadapter.Fill(DT)
                PolCoverFrom = CDate(DT.Tables(0).Rows.Item(0).Item(0))
                PolCoverTo = CDate(DT.Tables(0).Rows.Item(0).Item(1))
                con.Close()
            End Using
        End If
        ' IsBranchOffice(Session("Branch")) Or
        If IsBranch(Session("Branch")) Then
            STMPRM.ClientEnabled = True
            ISSPRM.ClientEnabled = True
        Else
            STMPRM.ClientEnabled = False
            ISSPRM.ClientEnabled = False
        End If
        SpecifyRestrictions(IIf(Request("OrderNo") Is Nothing, "", Request("OrderNo")), EndNo.Text)
        ExcelImp.ClientVisible = IsGroupedSys(Request("Sys"))
        DistPolicy.ClientVisible = IsReinsSys(Request("Sys")) And Not IsIssued(OrderNo.Value, EndNo.Value, LoadNo.Value, Request("Sys"))

        If IsBranch(Session("Branch")) Then
            Commissions.Visible = True
            OwnNo.Visible = True
        Else
            If IsBranchOffice(Session("Branch")) Then
                Commissions.Visible = True
            Else
                Commissions.Visible = False
            End If
            OwnNo.Visible = False
        End If
        If IsBranch(Session("Branch")) Then
            Currency.ClientEnabled = True
            ExcRate.ClientEnabled = True
        Else
            Currency.ClientEnabled = False
            ExcRate.ClientEnabled = False
        End If
        If Broker.Value = 0 Then
            Commision.Value = 0
            CommisionType.Value = 0
        Else
            If Commision.Value = 0 Then
                Broker.Value = 0
                CommisionType.Value = 0
            End If
            If CommisionType.Value = 0 Then
                Broker.Value = 0
                Commision.Value = 0
            End If
        End If
    End Sub

    Private Sub SpecifyRestrictions(Order, endn)
        If Order <> "" Then
            If IsIssued(OrderNo.Value.ToString, EndNo.Text, LoadNo.Text, Request("Sys")) Then
                'in case the offer is registered and issued
                If endn = 0 Then
                    Select Case Request("Sys")
                        Case "01", "PH", "08", "MN", "04"
                            CoverFrom.ReadOnly = True
                            CoverTo.ReadOnly = True
                            Measure.ReadOnly = False

                            If Format(GetIssueDate(OrderNo.Value.ToString, EndNo.Text, Request("Sys")), "yyyy/MM/dd") = Format(Today.Date, "yyyy/MM/dd") Then
                                Endorsment.ClientVisible = False
                            Else
                                Endorsment.ClientVisible = True
                            End If

                            'End If
                        Case "27"
                            CoverFrom.ReadOnly = True
                            CoverTo.ReadOnly = True
                            Measure.SelectedIndex = 0
                            Measure.ReadOnly = True
                            Endorsment.ClientVisible = True
                        Case "OR"
                            Measure.SelectedIndex = 0
                            Measure.ReadOnly = True
                            Interval.MinValue = 7
                            Interval.MaxValue = 90
                        Case "MC", "MB", "MA"
                            CoverDate.Visible = False
                            Endorsment.ClientVisible = True
                        Case "OC"
                            CoverDate.Visible = False
                            Shipment.ClientVisible = True
                        Case "02", "03", "FG", "CM", "PA", "EL", "CT", "CS", "HL", "ER", "CR", "PI", "CL", "FR", "FB", "09", "07", "MP"
                            If Request("EndType") = "Expand" Then
                                Measure.SelectedIndex = 0
                                Measure.ReadOnly = True
                                Interval.MinValue = 10
                                Interval.MaxValue = 90
                                Interval.ReadOnly = False
                            Else
                                'Measure.SelectedIndex = 2
                                CoverTo.ReadOnly = True
                                'Measure.ReadOnly = True
                                Endorsment.ClientVisible = True
                            End If
                        Case "09"
                            CoverTo.ReadOnly = True
                            'Measure.ReadOnly = True
                            Endorsment.ClientVisible = True
                        Case Else
                            Exit Select
                    End Select
                Else
                    Select Case Request("Sys")
                        Case "01", "04"
                            CoverFrom.ReadOnly = True
                            CoverTo.ReadOnly = True
                            Measure.ReadOnly = False
                            Interval.ReadOnly = True
                            Select Case Measure.SelectedIndex
                                Case 2
                                    Interval.MaxValue = 3
                                Case 1
                                    Interval.MaxValue = 2
                                Case 0
                                    Interval.MaxValue = IIf(IsBranch(Session("Branch")), 1095, 90)
                                Case Else
                                    Exit Select
                            End Select
                            Endorsment.ClientVisible = True
                        Case "PH"
                            CoverFrom.ReadOnly = True
                            CoverTo.ReadOnly = True
                            Measure.ReadOnly = False
                            Interval.ReadOnly = True
                            Select Case Measure.SelectedIndex
                                Case 2
                                    Interval.MaxValue = 1
                                Case 1
                                    Interval.MaxValue = 12
                                Case 0
                                    Interval.MaxValue = 365
                                Case Else
                                    Exit Select
                            End Select
                        Case "08", "MN"
                            CoverFrom.ReadOnly = True
                            CoverTo.ReadOnly = True
                            Measure.ReadOnly = False

                        Case "27"
                            Measure.SelectedIndex = 0
                            Measure.ReadOnly = True
                        Case "MC", "OC", "MB", "MA"
                            CoverDate.Visible = False
                        Case "02", "03", "FG", "CM", "PA", "EL", "CS", "CT", "HL", "PI", "CL", "FR", "FB", "09", "07", "MP"
                            If Request("EndType") = "Expand" Then
                                Measure.SelectedIndex = 0
                                Measure.ReadOnly = True
                                Interval.MinValue = 10
                                Interval.MaxValue = 90
                                Interval.ReadOnly = False
                            Else
                                'Measure.SelectedIndex = 2
                                CoverTo.ReadOnly = True
                                'Measure.ReadOnly = True
                                Interval.ReadOnly = True
                            End If

                        Case Else
                            Exit Select
                    End Select
                End If
            Else
                'in case the offer is registered and not issued
                If endn = 0 Then
                    Select Case Request("Sys")
                        Case "01", "MN", "04"
                            CoverFrom.Value = IIf(CoverFrom.Value <= Today.Date, DateAdd(DateInterval.Day, 1, Today.Date), CoverFrom.Value)
                            CoverFrom.MinDate = DateAdd(DateInterval.Day, 1, Today.Date)
                            Measure.ReadOnly = False
                            Select Case Measure.SelectedIndex
                                Case 2
                                    Interval.MaxValue = 3
                                    CoverTo.Value = IIf(CoverFrom.Value <= Today.Date, DateAdd(DateInterval.Year, Interval.Value, CoverFrom.Value), CoverTo.Value)
                                Case 1
                                    Interval.MaxValue = 2
                                    CoverTo.Value = IIf(CoverFrom.Value <= Today.Date, DateAdd(DateInterval.Month, Interval.Value, CoverFrom.Value), CoverTo.Value)
                                Case 0
                                    Interval.MaxValue = IIf(IsBranch(Session("Branch")), 1095, 90)
                                    CoverTo.Value = IIf(CoverFrom.Value <= Today.Date, DateAdd(DateInterval.Day, Interval.Value, CoverFrom.Value), CoverTo.Value)
                                Case Else
                                    Exit Select
                            End Select
                        Case "PH"
                            CoverFrom.Value = IIf(CoverFrom.Value <= Today.Date, DateAdd(DateInterval.Day, 1, Today.Date), CoverFrom.Value)
                            CoverFrom.MinDate = DateAdd(DateInterval.Day, 1, Today.Date)
                            Measure.ReadOnly = False
                            Select Case Measure.SelectedIndex
                                Case 2
                                    Interval.MaxValue = 1
                                    CoverTo.Value = IIf(CoverFrom.Value <= Today.Date, DateAdd(DateInterval.Year, Interval.Value, CoverFrom.Value), CoverTo.Value)
                                Case 1
                                    Interval.MaxValue = 12
                                    CoverTo.Value = IIf(CoverFrom.Value <= Today.Date, DateAdd(DateInterval.Month, Interval.Value, CoverFrom.Value), CoverTo.Value)
                                Case 0
                                    Interval.MaxValue = 365
                                    CoverTo.Value = IIf(CoverFrom.Value <= Today.Date, DateAdd(DateInterval.Day, Interval.Value, CoverFrom.Value), CoverTo.Value)
                                Case Else
                                    Exit Select
                            End Select
                        Case "08"
                            CoverFrom.Value = IIf(CoverFrom.Value <= Today.Date, DateAdd(DateInterval.Day, 1, Today.Date), CoverFrom.Value)
                            CoverFrom.MinDate = DateAdd(DateInterval.Day, 1, Today.Date)
                            Measure.ReadOnly = False
                            Select Case Measure.SelectedIndex
                                Case 2
                                    Interval.MaxValue = 1
                                    CoverTo.Value = IIf(CoverFrom.Value <= Today.Date, DateAdd(DateInterval.Year, Interval.Value, CoverFrom.Value), CoverTo.Value)
                                Case 1
                                    Interval.MaxValue = 12
                                    CoverTo.Value = IIf(CoverFrom.Value <= Today.Date, DateAdd(DateInterval.Month, Interval.Value, CoverFrom.Value), CoverTo.Value)
                                Case Else
                                    Exit Select
                                    'Case 0
                                    '    Interval.MaxValue = IIf(IsBranch(Session("Branch")), 1095, 90)
                                    '    CoverTo.Value = IIf(CoverFrom.Value <= Today.Date, DateAdd(DateInterval.Day, Interval.Value, CoverFrom.Value), CoverTo.Value)
                            End Select

                        Case "27"
                            CoverFrom.Value = IIf(CoverFrom.Value <= Today.Date, DateAdd(DateInterval.Day, 0, Today.Date), CoverFrom.Value)
                            CoverFrom.MinDate = DateAdd(DateInterval.Day, 0, Today.Date)
                            CoverTo.Value = IIf(CoverFrom.Value <= Today.Date, DateAdd(DateInterval.Year, Interval.Value, CoverFrom.Value), CoverTo.Value)
                            CoverTo.Value = DateAdd(DateInterval.Day, Interval.Value - 0, CoverFrom.Value)
                            Measure.SelectedIndex = 0
                            Measure.ReadOnly = True
                        Case "OR"
                            CoverFrom.Value = IIf(CoverFrom.Value <= Today.Date, DateAdd(DateInterval.Day, 0, Today.Date), CoverFrom.Value)
                            CoverFrom.MinDate = DateAdd(DateInterval.Day, 0, Today.Date)
                            CoverTo.Value = IIf(CoverFrom.Value <= Today.Date, DateAdd(DateInterval.Year, Interval.Value, CoverFrom.Value), CoverTo.Value)
                            CoverTo.Value = DateAdd(DateInterval.Day, IIf(CoverFrom.Value = Today.Date, Interval.Value, Interval.Value - 1), CoverFrom.Value)
                            Interval.MinValue = 7
                            Interval.MaxValue = 90
                            Measure.SelectedIndex = 0
                            Measure.ReadOnly = True
                        Case "MC", "MB", "MA", "OC", "ER", "CR"
                            CoverDate.Visible = False
                        Case "02", "03", "FG", "CM", "PA", "EL", "CS", "CT", "HL", "FR", "FB", "09", "07", "MP"
                            If Request("EndType") = "Expand" Then
                                Measure.SelectedIndex = 0
                                Measure.ReadOnly = True
                                Interval.MinValue = 10
                                Interval.MaxValue = 90
                                Interval.ReadOnly = False
                            Else
                                'Measure.SelectedIndex = 2
                                'Measure.ReadOnly = True
                            End If
                        Case Else
                            'Interval.MinValue = 1
                    End Select
                Else
                    Measure.ReadOnly = True
                    Interval.ReadOnly = True
                    Select Case Request("Sys")
                        Case "01", "PH", "08", "MN", "04"
                            If CoverFrom.Value <= Today.Date Then
                                CoverFrom.ReadOnly = True
                            Else
                                'CoverFrom.Value = IIf(CoverFrom.Value <= Today.Date, DateAdd(DateInterval.Day, 1, Today.Date), CoverFrom.Value)
                                CoverFrom.MinDate = DateAdd(DateInterval.Day, 1, Today.Date)
                                'CoverTo.Value = IIf(CoverFrom.Value <= Today.Date, DateAdd(DateInterval.Year, Interval.Value, CoverFrom.Value), CoverTo.Value)
                                Measure.ReadOnly = False
                            End If

                        Case "27"
                            If CoverFrom.Value <= Today.Date Then
                                CoverFrom.ReadOnly = True
                            Else
                                CoverFrom.MinDate = DateAdd(DateInterval.Day, 0, Today.Date)
                                Measure.ReadOnly = True
                            End If
                        Case "MC", "MB", "MA", "OC", "ER", "CR"
                            CoverDate.Visible = False
                        Case "02", "03", "FG", "PA", "CM", "EL", "CS", "CT", "HL", "PI", "CL", "FR", "FB", "09", "07", "MP"
                            If Request("EndType") = "Expand" Then
                                Measure.SelectedIndex = 0
                                Measure.ReadOnly = True
                                Interval.MinValue = 10
                                Interval.MaxValue = 90
                                Interval.ReadOnly = False
                            Else
                                'Measure.SelectedIndex = 2
                                CoverFrom.MinDate = PolCoverFrom
                                CoverFrom.MaxDate = PolCoverTo
                                CoverTo.ReadOnly = True
                                'Measure.ReadOnly = True
                                Interval.ReadOnly = True
                            End If

                        Case Else
                            Exit Select
                    End Select
                End If
            End If
            'in case the offer is registered and not issued
            Select Case Request("Sys")
                Case "01"
                    Measure.ReadOnly = False
                    Select Case Measure.SelectedIndex
                        Case 2
                            Interval.MaxValue = 3
                        Case 1
                            Interval.MaxValue = 2
                        Case 0
                            'Interval.MinValue = 15
                            Interval.ReadOnly = False
                            Interval.MaxValue = IIf(IsBranch(Session("Branch")), 1095, 90)
                        Case Else
                            Exit Select
                    End Select
                    'If IsIssued(Order, endn, 0, Request("Sys")) Then
                    '    Endorsment.ClientVisible = Format(GetIssueDate(OrderNo.Value.ToString, EndNo.Text, Request("Sys")), "yyyy/MM/dd") <> Format(Today.Date, "yyyy/MM/dd")
                    'Else
                    Endorsment.ClientVisible = True
                    'End If
                Case "MN", "04"
                    Measure.ReadOnly = False
                    Select Case Measure.SelectedIndex
                        Case 2
                            Interval.MaxValue = 3
                        Case 1
                            Interval.MaxValue = 2
                        Case 0
                            'Interval.MinValue = 15
                            Interval.ReadOnly = False
                            Interval.MaxValue = IIf(IsBranch(Session("Branch")), 1095, 90)
                        Case Else
                            Exit Select
                    End Select

                Case "PH"
                    Measure.ReadOnly = False
                    Select Case Measure.SelectedIndex
                        Case 2
                            Interval.MaxValue = 1
                        Case 1
                            Interval.MaxValue = 12
                        Case 0
                            'Interval.MinValue = 15
                            Interval.ReadOnly = False
                            Interval.MaxValue = 365
                        Case Else
                            Exit Select
                    End Select
                Case "08"
                    Measure.ReadOnly = False
                    Select Case Measure.SelectedIndex
                        Case 2
                            Interval.MaxValue = 3
                        Case 1
                            Interval.MaxValue = 12
                        Case Else
                            Exit Select
                    End Select
                Case "27"
                    Measure.SelectedIndex = 0
                    Measure.ReadOnly = True
                Case "OR"
                    Measure.SelectedIndex = 0
                    Measure.ReadOnly = True
                    'Interval.MinValue = 7
                    'Interval.MaxValue = 90
                Case "MC", "MB", "MA", "OC", "ER", "CR"
                    CoverDate.Visible = False
                Case Else
                    'Interval.MinValue = 1
            End Select
        Else
            'in case the New Offer
            Select Case Request("Sys")
                Case "01"
                    If OrderNo.Text = "0" Then
                        CoverFrom.MinDate = DateAdd(DateInterval.Day, 1, Today.Date)
                        CoverFrom.Value = IIf(Not Page.IsCallback, DateAdd(DateInterval.Day, 1, Today.Date), CoverFrom.Value)
                        'CoverFrom.Value = IIf(Not Page.IsCallback, Today.Date, CoverFrom.Value)

                        Measure.ReadOnly = False
                        Select Case Measure.Value
                            Case 3
                                Interval.MaxValue = 3
                                Interval.Value = IIf(Interval.Value = 0, 1, Interval.Value)
                                CoverTo.Value = DateAdd(DateInterval.Year, Interval.Value, CoverFrom.Value)
                                CoverTo.Value = DateAdd(DateInterval.Day, -1, CoverTo.Value)
                                'CoverTo.Value = DateAdd(DateInterval.Day, 365 * (Interval.Value), CoverFrom.Value)
                            Case 2
                                Interval.MaxValue = 2
                                Interval.Value = IIf(Interval.Value = 0, 1, Interval.Value)
                                CoverTo.Value = DateAdd(DateInterval.Month, Interval.Value, CoverFrom.Value)
                                CoverTo.Value = DateAdd(DateInterval.Day, -1, CoverTo.Value)
                                'CoverTo.Value = DateAdd(DateInterval.Month, Interval.Value, CoverFrom.Value)
                            Case 1
                                Interval.MinValue = 15
                                Interval.MaxValue = IIf(IsBranch(Session("Branch")), 1095, 90)
                                'Interval.Value = Interval.Value
                                Interval.Value = IIf(Page.IsCallback, 15, Interval.Value)
                                'Interval.Value = 15
                                'CoverTo.Value = DateAdd(DateInterval.Day, Interval.Value - 1, CoverFrom.Value)
                                CoverTo.Value = DateAdd(DateInterval.Day, Interval.Value, CoverFrom.Value)
                                CoverTo.Value = DateAdd(DateInterval.Day, -1, CoverTo.Value)
                                'CoverTo.Value = DateAdd(DateInterval.Day, Interval.Value, CoverFrom.Value)
                        End Select
                    Else
                        CoverFrom.MinDate = DateAdd(DateInterval.Day, 1, Today.Date)
                        Select Case Measure.Value
                            Case 3
                                Interval.MaxValue = 3
                            Case 2
                                Interval.MaxValue = 2
                            Case 1
                                Interval.Value = IIf(Interval.Value < 15, 15, Interval.Value)
                                Interval.MinValue = 15
                                Interval.MaxValue = IIf(IsBranch(Session("Branch")), 1095, 90)
                                CoverTo.Value = DateAdd(DateInterval.Day, Interval.Value, CoverFrom.Value)
                                CoverTo.Value = DateAdd(DateInterval.Day, -1, CoverTo.Value)
                        End Select
                    End If
                Case "MN", "04"
                    If OrderNo.Text = "0" Then
                        CoverFrom.MinDate = DateAdd(DateInterval.Day, 1, Today.Date)
                        'CoverFrom.Value = Format(DateAdd(DateInterval.Day, 1, Today.Date), "yyyy/MM/dd")
                        Measure.ReadOnly = True
                        Measure.SelectedIndex = 2
                        Interval.MinValue = 1
                        Interval.MaxValue = 1
                        Interval.Value = 1
                    Else
                        CoverFrom.MinDate = DateAdd(DateInterval.Day, 1, Today.Date)
                        Measure.ReadOnly = True
                        Measure.SelectedIndex = 2
                        Measure.ReadOnly = True
                        Interval.MinValue = 1
                        Interval.MaxValue = 1

                    End If
                Case "PH"
                    If OrderNo.Text = "0" Then
                        Measure.ReadOnly = False
                        CoverFrom.MinDate = DateAdd(DateInterval.Day, 1, Today.Date)
                        CoverFrom.Value = IIf(Not Page.IsCallback, DateAdd(DateInterval.Day, 1, Today.Date), CoverFrom.Value)
                        Select Case Measure.SelectedIndex
                            Case 2
                                Interval.MaxValue = 1
                                Interval.Value = 1
                                CoverTo.Value = DateAdd(DateInterval.Year, Interval.Value, CoverFrom.Value)
                                CoverTo.Value = DateAdd(DateInterval.Day, -1, CoverTo.Value)
                                'CoverTo.Value = DateAdd(DateInterval.Day, 365 * (Interval.Value), CoverFrom.Value)
                            Case 1
                                Interval.MaxValue = 12
                                Interval.Value = 1
                                CoverTo.Value = DateAdd(DateInterval.Month, Interval.Value, CoverFrom.Value)
                                CoverTo.Value = DateAdd(DateInterval.Day, -1, CoverTo.Value)
                                'CoverTo.Value = DateAdd(DateInterval.Month, Interval.Value, CoverFrom.Value)
                            Case 0
                                Interval.MinValue = 1
                                Interval.MaxValue = 365
                                'Interval.Value = Interval.Value
                                Interval.Value = IIf(Page.IsCallback, 365, Interval.Value)
                                'Interval.Value = 15
                                'CoverTo.Value = DateAdd(DateInterval.Day, Interval.Value - 1, CoverFrom.Value)
                                CoverTo.Value = DateAdd(DateInterval.Day, Interval.Value, CoverFrom.Value)
                                CoverTo.Value = DateAdd(DateInterval.Day, -1, CoverTo.Value)
                            Case Else
                                Exit Select
                                'CoverTo.Value = DateAdd(DateInterval.Day, Interval.Value, CoverFrom.Value)
                        End Select
                    Else
                        CoverFrom.MinDate = DateAdd(DateInterval.Day, 1, Today.Date)
                        Select Case Measure.SelectedIndex
                            Case 2
                                Interval.MaxValue = 1
                            Case 1
                                Interval.MaxValue = 12
                            Case 0
                                Interval.MinValue = 1
                                Interval.MaxValue = 365
                            Case Else
                                Exit Select
                        End Select

                    End If
                Case "08"
                    If OrderNo.Text = "0" Then
                        CoverFrom.MinDate = DateAdd(DateInterval.Day, 1, Today.Date)
                        'CoverFrom.Value = Format(DateAdd(DateInterval.Day, 1, Today.Date), "yyyy/MM/dd")
                        Measure.ReadOnly = False
                        'Measure.SelectedIndex = 2
                        Interval.MinValue = 1
                        Interval.MaxValue = 12
                        'Interval.Value = 1
                    Else
                        CoverFrom.MinDate = DateAdd(DateInterval.Day, 1, Today.Date)
                        Measure.ReadOnly = False
                        'Measure.SelectedIndex = 2
                        Measure.ReadOnly = False
                        Interval.MinValue = 1
                        Interval.MaxValue = 12

                    End If
                Case "27"
                    If OrderNo.Text = "0" Then
                        CoverFrom.MinDate = Today.Date
                        CoverFrom.Value = IIf(Not Page.IsCallback, Today.Date, CoverFrom.Value)
                        Measure.SelectedIndex = 0
                        Measure.ReadOnly = True
                        Interval.MinValue = 14
                        Interval.MaxValue = 730
                        Interval.Value = IIf(Not Page.IsCallback, 14, Interval.Value)
                        CoverTo.Value = DateAdd(DateInterval.Day, Interval.Value - 0, CoverFrom.Value)
                    Else
                        CoverFrom.MinDate = Today.Date
                        'CoverFrom.Value = Today.Date
                        Measure.SelectedIndex = 0
                        Measure.ReadOnly = True
                        Interval.MinValue = 14
                        Interval.MaxValue = 730
                        'Interval.Value = 7
                        'CoverTo.Value = DateAdd(DateInterval.Day, Interval.Value - 1, CoverFrom.Value)
                    End If
                Case "OR"
                    If OrderNo.Text = "0" Then
                        CoverFrom.MinDate = Today.Date
                        CoverFrom.Value = IIf(Not Page.IsCallback, Today.Date, CoverFrom.Value) 'Today.Date
                        'IIf(Page.IsCallback, Today.Date, CoverFrom.Value)
                        Measure.SelectedIndex = 0
                        Measure.ReadOnly = True
                        Interval.MinValue = 7
                        Interval.MaxValue = 90
                        Interval.Value = IIf(Not Page.IsCallback, 7, Interval.Value)
                        CoverTo.Value = DateAdd(DateInterval.Day, IIf(CoverFrom.Value = Today.Date, Interval.Value, Interval.Value - 1), CoverFrom.Value)
                    Else
                        CoverFrom.MinDate = Today.Date
                        'CoverFrom.Value = Today.Date
                        Measure.SelectedIndex = 0
                        Measure.ReadOnly = True
                        Interval.MinValue = 7
                        Interval.MaxValue = 90
                        'Interval.Value = 7
                        'CoverTo.Value = DateAdd(DateInterval.Day, Interval.Value - 1, CoverFrom.Value)
                    End If

                Case "MC", "MB", "MA", "OC", "ER", "CR"
                    CoverDate.Visible = False
                Case Else
                    'Interval.MinValue = 1
            End Select
        End If

    End Sub

    Private Sub RetrieveData()
        Dim MainData As New DataSet
        Dim PRM As Double
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()

            Dim dbadapter = New SqlDataAdapter("select * from PolicyFile where OrderNo='" & Request("OrderNo") & "' AND EndNo=" & Request("EndNo") & " " _
            & "AND LoadNo=" & Request("LoadNo") & " AND SubIns='" & Request("Sys") & "'", con)
            dbadapter.Fill(MainData)
            If HasControls() Then
                Dim cont As ControlCollection = Controls
                For Each mycontrol As Control In cont
                    If TypeOf mycontrol Is ASPxRoundPanel Then
                        Dim mytb As ASPxRoundPanel = TryCast(mycontrol, ASPxRoundPanel)
                        For i As Integer = 0 To mytb.Controls.Count - 1
                            If TypeOf mytb.Controls(i) Is ASPxTextBox Then
                                Select Case TryCast(mytb.Controls(i), ASPxTextBox).CssClass
                                    Case 1
                                        TryCast(mytb.Controls(i), ASPxTextBox).Value = IIf(Convert.IsDBNull(MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID)), "", MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID))
                                    Case 2
                                        If mytb.Controls(i).ID = "NETPRM" Then
                                            TryCast(mytb.Controls(i), ASPxTextBox).Value = Format(MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID), "###,#0.000")
                                            PRM = Format(MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID), "###,#0.000")
                                        Else
                                            TryCast(mytb.Controls(i), ASPxTextBox).Value = Format(MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID), "###,#0.000")
                                        End If
                                    Case 3
                                        TryCast(mytb.Controls(i), ASPxTextBox).Value = IIf(IsDBNull(MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID)), "/", MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID))
                                    Case 5
                                        TryCast(mytb.Controls(i), ASPxTextBox).Value = Format(MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID), "###,#0.0000000")
                                    Case Else
                                        Exit Select
                                End Select
                            End If
                            If TypeOf mytb.Controls(i) Is ASPxGridLookup Then
                                TryCast(mytb.Controls(i), ASPxGridLookup).Value = MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID)
                            End If
                            If TypeOf mytb.Controls(i) Is HiddenField Then
                                TryCast(mytb.Controls(i), HiddenField).Value = MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID)
                                'txtCustomerSearch.Value = GetCustomerName(MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID))

                                Dim customerinfo = GetCustomerDetails(MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID))
                                txtCustomerSearch.Value = Trim(customerinfo(0).CustName)
                                CustNameE.Value = Trim(customerinfo(0).CustNameE)
                                Address.Value = Trim(customerinfo(0).Address)
                                TelNo.Value = IIf(Len(Trim(customerinfo(0).TelNo).TrimEnd) < 13, "+21800", Trim(customerinfo(0).TelNo))
                            End If
                            If TypeOf mytb.Controls(i) Is ASPxDateEdit Then
                                TryCast(mytb.Controls(i), ASPxDateEdit).Value = MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID)
                            End If
                            If TypeOf mytb.Controls(i) Is ASPxSpinEdit Then
                                TryCast(mytb.Controls(i), ASPxSpinEdit).Value = MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID).ToString
                            End If
                            If TypeOf mytb.Controls(i) Is ASPxComboBox Then

                                If mytb.Controls(i).ID = "Measure" And (Request("Sys") = "MC" Or Request("Sys") = "MB" Or Request("Sys") = "MA" Or Request("Sys") = "OC" Or Request("Sys") = "CR" Or Request("Sys") = "ER") Then
                                Else
                                    TryCast(mytb.Controls(i), ASPxComboBox).DataBind()
                                    TryCast(mytb.Controls(i), ASPxComboBox).Value = MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID)
                                    'TryCast(mytb.Controls(i), ASPxComboBox).SelectedIndex = MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID) - 1
                                End If

                            End If
                            If TypeOf mytb.Controls(i) Is ASPxRadioButtonList Then
                                TryCast(mytb.Controls(i), ASPxRadioButtonList).Value = MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID)
                            End If
                        Next
                    End If
                    If TypeOf mycontrol Is ASPxFormLayout Then
                        Dim mytb As ASPxFormLayout = DirectCast(mycontrol, ASPxFormLayout)
                        For i As Integer = 0 To mytb.Controls.Count - 1
                            If TypeOf mytb.Controls(i) Is ASPxTextBox Then
                                Select Case DirectCast(mytb.Controls(i), ASPxTextBox).CssClass
                                    Case 1
                                        DirectCast(mytb.Controls(i), ASPxTextBox).Value = Trim(MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID))
                                    Case 2
                                        If mytb.Controls(i).ID = "NETPRM" Then
                                            DirectCast(mytb.Controls(i), ASPxTextBox).Value = Format(MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID), "###,#0.000")
                                            PRM = Format(MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID), "###,#0.000")
                                        Else
                                            DirectCast(mytb.Controls(i), ASPxTextBox).Value = Format(MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID), "###,#0.000")
                                        End If
                                    Case 3
                                        DirectCast(mytb.Controls(i), ASPxTextBox).Value = IIf(IsDBNull(MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID)), "/", MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID))
                                    Case 5
                                        DirectCast(mytb.Controls(i), ASPxTextBox).Value = Format(MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID), "###,#0.0000000")
                                    Case Else
                                        Exit Select
                                End Select
                            End If
                            If TypeOf mytb.Controls(i) Is ASPxGridLookup Then
                                DirectCast(mytb.Controls(i), ASPxGridLookup).Value = MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID)
                            End If
                            If TypeOf mytb.Controls(i) Is ASPxDateEdit Then
                                DirectCast(mytb.Controls(i), ASPxDateEdit).Value = MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID)
                            End If
                            If TypeOf mytb.Controls(i) Is ASPxSpinEdit Then
                                DirectCast(mytb.Controls(i), ASPxSpinEdit).Value = MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID).ToString
                            End If
                            If TypeOf mytb.Controls(i) Is ASPxComboBox Then

                                If mytb.Controls(i).ID = "Measure" And (Request("Sys") = "MC" Or Request("Sys") = "MB" Or Request("Sys") = "MA" Or Request("Sys") = "OC" Or Request("Sys") = "CR" Or Request("Sys") = "ER") Then
                                Else
                                    DirectCast(mytb.Controls(i), ASPxComboBox).DataBind()
                                    DirectCast(mytb.Controls(i), ASPxComboBox).Value = MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID)
                                    'DirectCast(mytb.Controls(i), ASPxComboBox).SelectedIndex = MainData.Tables(0).Rows.Item(0).Item(mytb.Controls(i).ID) - 1
                                End If

                            End If
                        Next
                    End If
                Next
            End If

            If Parent.HasControls() Then
                Dim SytempolicyData As New DataSet

                Dim dbadapter1 = New SqlDataAdapter("select * from " & GetGroupFile(Request("Sys")) & " where OrderNo='" & Request("OrderNo") & "' And " _
                                                          & "EndNo=" & Request("EndNo") & " And LoadNo=" & Request("LoadNo"), Conn)
                dbadapter1.Fill(SytempolicyData)
                If SytempolicyData.Tables(0).Rows.Count = 0 Then
                Else
                    'css case 1 -> text //// case 2 -> Float //// case 3 ->integer or big integer

                    For i As Integer = 0 To Parent.Controls.Count - 1

                        If TypeOf Parent.Controls(i) Is ASPxPanel Then
                            Dim mytbD As ASPxPanel = TryCast(Parent.Controls(i), ASPxPanel)
                            For j As Integer = 0 To mytbD.Controls.Count - 1
                                If TypeOf mytbD.Controls(j) Is ASPxTextBox Then
                                    Select Case TryCast(mytbD.Controls(j), ASPxTextBox).CssClass
                                        Case 1
                                            TryCast(mytbD.Controls(j), ASPxTextBox).Value = Trim(IIf(IsDBNull(SytempolicyData.Tables(0).Rows.Item(0).Item(mytbD.Controls(j).ID)), "/", SytempolicyData.Tables(0).Rows.Item(0).Item(mytbD.Controls(j).ID)))
                                        Case 2
                                            If mytbD.Controls(j).ID = "Premium" Then
                                                TryCast(mytbD.Controls(j), ASPxTextBox).Value = Format(PRM, "###,#0.000")
                                            Else
                                                TryCast(mytbD.Controls(j), ASPxTextBox).Value = Format(SytempolicyData.Tables(0).Rows.Item(0).Item(mytbD.Controls(j).ID), "###,#0.000")
                                            End If

                                        Case 3
                                            TryCast(mytbD.Controls(j), ASPxTextBox).Value = IIf(IsDBNull(SytempolicyData.Tables(0).Rows.Item(0).Item(mytbD.Controls(j).ID)), 0, SytempolicyData.Tables(0).Rows.Item(0).Item(mytbD.Controls(j).ID))
                                        Case 5
                                            TryCast(mytbD.Controls(j), ASPxTextBox).Value = Format(SytempolicyData.Tables(0).Rows.Item(0).Item(mytbD.Controls(j).ID), "###,#0.00000000")
                                        Case Else

                                    End Select
                                End If
                            Next
                        End If

                        If TypeOf Parent.Controls(i) Is ASPxRoundPanel Then
                            Dim mytbD As ASPxRoundPanel = TryCast(Parent.Controls(i), ASPxRoundPanel)
                            For k As Integer = 0 To mytbD.Controls.Count - 1
                                If TypeOf mytbD.Controls(k) Is ASPxTextBox Then
                                    Select Case TryCast(mytbD.Controls(k), ASPxTextBox).CssClass
                                        Case 1
                                            TryCast(mytbD.Controls(k), ASPxTextBox).Value = Trim(IIf(IsDBNull(SytempolicyData.Tables(0).Rows.Item(0).Item(mytbD.Controls(k).ID)), "/", SytempolicyData.Tables(0).Rows.Item(0).Item(mytbD.Controls(k).ID)))
                                        Case 2
                                            If mytbD.Controls(k).ID = "Premium" Then
                                                TryCast(mytbD.Controls(k), ASPxTextBox).Value = Format(PRM, "###,#0.000")
                                            Else
                                                TryCast(mytbD.Controls(k), ASPxTextBox).Value = Format(SytempolicyData.Tables(0).Rows.Item(0).Item(mytbD.Controls(k).ID), "###,#0.000")
                                            End If
                            'DirectCast((Parent.Controls(i)), ASPxTextBox).Value = Format(SytempolicyData.Tables(0).Rows.Item(0).Item(Parent.Controls(i).ID), "###,#0.000")
                                        Case 3
                                            TryCast(mytbD.Controls(k), ASPxTextBox).Value = IIf(IsDBNull(SytempolicyData.Tables(0).Rows.Item(0).Item(mytbD.Controls(k).ID)), 0, SytempolicyData.Tables(0).Rows.Item(0).Item(mytbD.Controls(k).ID))
                                        Case 5
                                            TryCast(mytbD.Controls(k), ASPxTextBox).Value = Format(SytempolicyData.Tables(0).Rows.Item(0).Item(mytbD.Controls(k).ID), "###,#0.00000000")
                                        Case Else

                                    End Select
                                End If
                                If TypeOf mytbD.Controls(k) Is ASPxDateEdit Then
                                    TryCast(mytbD.Controls(k), ASPxDateEdit).Value = SytempolicyData.Tables(0).Rows.Item(0).Item(mytbD.Controls(k).ID)
                                End If
                            Next
                        End If

                        If TypeOf Parent.Controls(i) Is ASPxFormLayout Then
                            Dim mytbD As ASPxFormLayout = TryCast(Parent.Controls(i), ASPxFormLayout)
                            For Each LitemBase As LayoutItemBase In mytbD.Items
                                For Each item In TryCast(LitemBase, LayoutGroupBase).Items
                                    Dim NlayoutItem = TryCast(item, LayoutItem)
                                    If NlayoutItem IsNot Nothing Then
                                        'LEVEL OF CONTROLS
                                        For Each Ctrl As Control In NlayoutItem.Controls

                                            If TypeOf Ctrl Is ASPxTextBox Then
                                                Select Case TryCast(Ctrl, ASPxTextBox).CssClass
                                                    Case 1
                                                        TryCast(Ctrl, ASPxTextBox).Value = Trim(IIf(IsDBNull(SytempolicyData.Tables(0).Rows.Item(0).Item(Ctrl.ID)), "/", SytempolicyData.Tables(0).Rows.Item(0).Item(Ctrl.ID)))
                                                    Case 2
                                                        If Ctrl.ID = "Premium" Then
                                                            TryCast(Ctrl, ASPxTextBox).Value = Format(PRM, "###,#0.000")
                                                        Else
                                                            TryCast(Ctrl, ASPxTextBox).Value = Format(SytempolicyData.Tables(0).Rows.Item(0).Item(Ctrl.ID), "###,#0.000")
                                                        End If
                                                    Case 3
                                                        TryCast(Ctrl, ASPxTextBox).Value = IIf(IsDBNull(SytempolicyData.Tables(0).Rows.Item(0).Item(Ctrl.ID)), 0, SytempolicyData.Tables(0).Rows.Item(0).Item(Ctrl.ID))
                                                    Case 5
                                                        TryCast(Ctrl, ASPxTextBox).Value = Format(SytempolicyData.Tables(0).Rows.Item(0).Item(Ctrl.ID), "###,#0.00000000")
                                                    Case Else
                                                End Select
                                                'Dim TT As String = Ctrl.ID.ToString
                                            End If
                                            If TypeOf Ctrl Is ASPxSpinEdit Then
                                                TryCast(Ctrl, ASPxSpinEdit).Value = SytempolicyData.Tables(0).Rows.Item(0).Item(Ctrl.ID)
                                            End If
                                            If TypeOf Ctrl Is ASPxComboBox Then
                                                TryCast(Ctrl, ASPxComboBox).DataBind()
                                                TryCast(Ctrl, ASPxComboBox).Value = SytempolicyData.Tables(0).Rows.Item(0).Item(Ctrl.ID)
                                                'TryCast(Ctrl, ASPxComboBox).SelectedIndex = SytempolicyData.Tables(0).Rows.Item(0).Item(Ctrl.ID) - 1
                                                'TryCast(Ctrl, ASPxComboBox).SelectedIndex = TryCast(Ctrl, ASPxComboBox).Items.IndexOfValue(SytempolicyData.Tables(0).Rows.Item(0).Item(Ctrl.ID))
                                            End If
                                            If TypeOf Ctrl Is ASPxTokenBox Then
                                                TryCast(Ctrl, ASPxTokenBox).Value = Trim(IIf(IsDBNull(SytempolicyData.Tables(0).Rows.Item(0).Item(Ctrl.ID)), "", SytempolicyData.Tables(0).Rows.Item(0).Item(Ctrl.ID)))
                                            End If
                                            If TypeOf Ctrl Is ASPxCheckBox Then
                                                If Ctrl.ID = "RateAll" Then
                                                Else
                                                    TryCast(Ctrl, ASPxCheckBox).Value = SytempolicyData.Tables(0).Rows.Item(0).Item(Ctrl.ID)
                                                End If

                                            End If
                                        Next
                                    End If
                                Next
                            Next
                        End If

                        If TypeOf Parent.Controls(i) Is ASPxTextBox Then
                            Select Case TryCast(Parent.Controls(i), ASPxTextBox).CssClass
                                Case 1
                                    TryCast(Parent.Controls(i), ASPxTextBox).Value = Trim(IIf(IsDBNull(SytempolicyData.Tables(0).Rows.Item(0).Item(Parent.Controls(i).ID)), "/", SytempolicyData.Tables(0).Rows.Item(0).Item(Parent.Controls(i).ID)))
                                Case 2
                                    If Parent.Controls(i).ID = "Premium" Then
                                        TryCast(Parent.Controls(i), ASPxTextBox).Value = Format(PRM, "###,#0.000")
                                    Else
                                        TryCast(Parent.Controls(i), ASPxTextBox).Value = Format(SytempolicyData.Tables(0).Rows.Item(0).Item(Parent.Controls(i).ID), "###,#0.000")
                                    End If
                            'DirectCast((Parent.Controls(i)), ASPxTextBox).Value = Format(SytempolicyData.Tables(0).Rows.Item(0).Item(Parent.Controls(i).ID), "###,#0.000")
                                Case 3
                                    TryCast(Parent.Controls(i), ASPxTextBox).Value = IIf(IsDBNull(SytempolicyData.Tables(0).Rows.Item(0).Item(Parent.Controls(i).ID)), 0, SytempolicyData.Tables(0).Rows.Item(0).Item(Parent.Controls(i).ID))
                                Case 5
                                    TryCast(Parent.Controls(i), ASPxTextBox).Value = Format(SytempolicyData.Tables(0).Rows.Item(0).Item(Parent.Controls(i).ID), "###,#0.00000000")
                                Case Else

                            End Select
                        End If

                        If TypeOf Parent.Controls(i) Is ASPxGridLookup Then
                            TryCast(Parent.Controls(i), ASPxGridLookup).Value = SytempolicyData.Tables(0).Rows.Item(0).Item(Parent.Controls(i).ID)
                        End If

                        If TypeOf Parent.Controls(i) Is ASPxDateEdit Then
                            TryCast(Parent.Controls(i), ASPxDateEdit).Value = SytempolicyData.Tables(0).Rows.Item(0).Item(Parent.Controls(i).ID)
                        End If

                        If TypeOf Parent.Controls(i) Is ASPxSpinEdit Then
                            TryCast(Parent.Controls(i), ASPxSpinEdit).Value = SytempolicyData.Tables(0).Rows.Item(0).Item(Parent.Controls(i).ID)
                        End If

                        If TypeOf Parent.Controls(i) Is ASPxComboBox Then
                            TryCast(Parent.Controls(i), ASPxComboBox).DataBind()
                            TryCast(Parent.Controls(i), ASPxComboBox).Value = SytempolicyData.Tables(0).Rows.Item(0).Item(Parent.Controls(i).ID)
                            'TryCast(Parent.Controls(i), ASPxComboBox).SelectedIndex = SytempolicyData.Tables(0).Rows.Item(0).Item(Parent.Controls(i).ID) - 1
                        End If

                        If TypeOf Parent.Controls(i) Is ASPxTokenBox Then
                            TryCast(Parent.Controls(i), ASPxTokenBox).Value = Trim(IIf(IsDBNull(SytempolicyData.Tables(0).Rows.Item(0).Item(Parent.Controls(i).ID)), "", SytempolicyData.Tables(0).Rows.Item(0).Item(Parent.Controls(i).ID)))
                        End If

                        If TypeOf Parent.Controls(i) Is ASPxCheckBox Then
                            If Parent.Controls(i).ID = "RateAll" Then
                            Else
                                TryCast(Parent.Controls(i), ASPxCheckBox).Value = SytempolicyData.Tables(0).Rows.Item(0).Item(Parent.Controls(i).ID)
                            End If

                        End If
                    Next
                End If
            End If
            con.Close()
        End Using
    End Sub

    Protected Sub Currency_SelectedIndexChanged(sender As Object, e As EventArgs)
        ExcRate.Text = GetExrate(Currency.Value, Currency.SelectedItem.Text)
        ExcRate.Enabled = False
    End Sub

    Protected Sub Currency_TextChanged(sender As Object, e As EventArgs)
        'ExcRate.Text = GetExrate(Currency.Value, Currency.SelectedItem.Text)
        SetDxtxtValue(ExcRate, GetExrate(Currency.Value, Currency.SelectedItem.Text))
        ExcRate.Enabled = False
    End Sub

    Protected Sub Paytype_SelectedIndexChanged(sender As Object, e As EventArgs)
        'Paytype.TextBox.Value = Paytype.SelectedItem.Text
        'Paytype.InitialSearch = Paytype.SelectedItem.Text
        If PayType.Value = 2 Then
            Dim AccountNo As New DataSet

            Dim dbadapter = New SqlDataAdapter("select AccNo from Customerfile where CustNo=" & CustNo.Value, Conn)
            dbadapter.Fill(AccountNo)
            'AccountNo = RecSet("select AccNo from Customerfile where CustNo=" & CustNo.Text, Conn)
            If Not AccountNo.Tables(0).Rows.Item(0).IsNull(0) Then
                SetDxtxtValue(Me.AccountNo, AccountNo.Tables(0).Rows(0)(0))
                'AccNo.Text = AccountNo.Tables(0).Rows(0)(0)
            Else
                'MsgBox.confirm("! لايوجد رقم حساب لهذا الزبون :" & CustNo.Text & " هل تريد تسجيل رقم حساب له ?", "AccNo_request")
            End If
        End If
        Conn.Close()
    End Sub

    Protected Sub CallbackCustomer_Callback(source As Object, e As CallbackEventArgs)
        Try
            Dim callbackData As String = e.Parameter
            'System.Diagnostics.Debug.WriteLine("Callback received: " & callbackData)

            If callbackData.StartsWith("SEARCH:") Then
                Dim searchTerm As String = callbackData.Substring("SEARCH:".Length)
                Dim customers As New List(Of Object)()

                Using connection As New SqlConnection(ConnectionString)
                    ' استعلام محسن للحصول على أفضل النتائج
                    Dim sql As String = "SELECT TOP(6) CustNo, CustName, TelNo, Address, CustNameE FROM CustomerFile " _
                          & "WHERE CustName LIKE '%' + @searchTerm + '%' OR CustNameE LIKE '%' + @searchTerm + '%' OR TelNo LIKE '%' + @searchTerm + '%' " _
                          & "ORDER BY " _
                          & " CASE WHEN CustName = @searchTerm THEN 0 " _
                          & "     WHEN CustName LIKE @searchTerm + '%' THEN 1 " _
                          & "       ELSE 2 END, " _
                          & "  CustNo DESC"

                    Using command As New SqlCommand(sql, connection)
                        command.Parameters.AddWithValue("@searchTerm", searchTerm)
                        connection.Open()

                        Using reader As SqlDataReader = command.ExecuteReader()
                            While reader.Read()
                                customers.Add(New With {
                        .CustNo = SafeGetString(reader, "CustNo"),
                        .CustName = SafeGetString(reader, "CustName"),
                        .TelNo = SafeGetString(reader, "TelNo"),
                        .Address = SafeGetString(reader, "Address"),
                        .CustNameE = SafeGetString(reader, "CustNameE")
                    })
                            End While
                        End Using
                    End Using
                End Using

                Dim serializer As New JavaScriptSerializer()
                e.Result = serializer.Serialize(customers)

            ElseIf callbackData.StartsWith("INSERT:") Then
                Dim newCustomerName As String = callbackData.Substring("INSERT:".Length)
                Dim newId As Integer = 0

                Using connection As New SqlConnection(ConnectionString)
                    ' Get next customer number
                    Dim getMaxSql As String = "SELECT ISNULL(MAX(CAST(CustNo AS INT)), 0) + 1 FROM CustomerFile"
                    Using getMaxCmd As New SqlCommand(getMaxSql, connection)
                        connection.Open()
                        newId = Convert.ToInt32(getMaxCmd.ExecuteScalar())
                    End Using

                    ' Insert new customer using your stored procedure
                    Dim insertSql As String = "NewCustomer"
                    Using command As New SqlCommand(insertSql, connection)
                        command.CommandType = CommandType.StoredProcedure
                        command.Parameters.AddWithValue("@CustName", newCustomerName)
                        command.Parameters.AddWithValue("@CustNameE", newCustomerName)
                        command.Parameters.AddWithValue("@TelNo", "+218000000009")
                        command.Parameters.AddWithValue("@Address", "ليبيا")
                        command.Parameters.AddWithValue("@User", If(Session("User"), "System"))

                        command.ExecuteNonQuery()
                    End Using
                End Using

                e.Result = newId.ToString()
            Else
                e.Result = ""
            End If
        Catch ex As Exception
            'Debug.WriteLine("Callback error: " & ex.Message)
            'Debug.WriteLine("Stack trace: " & ex.StackTrace)
            e.Result = "ERROR: " & ex.Message
        End Try
    End Sub

    'Protected Sub Broker_ValueChanged(sender As Object, e As EventArgs)
    '    'If Broker.Value = 0 Then
    '    '    Commision.ClientEnabled = False
    '    '    CommisionType.ClientEnabled = False
    '    'Else
    '    '    Commision.ClientEnabled = True
    '    '    CommisionType.ClientEnabled = True
    '    'End If
    'End Sub

    'Protected Sub Endorsment_Click(sender As Object, e As EventArgs) Handles Endorsment.Click
    '    'EndNo.Text = Val(EndNo.Text) + 1
    '    'OrderNo.Text = 0
    '    'Endorsment.Visible = False
    '    SpecifyRestrictionsEnd(EndNo.Text)
    'End Sub
    Private Sub SpecifyRestrictionsEnd(endn)
        If endn <> 0 Then
            If IsIssued(OrderNo.Value.ToString, endn, LoadNo.Text, Request("Sys")) Then
                'in case the offer is registered and issued
                Select Case Request("Sys")
                    Case "01", "04"
                        CoverFrom.ReadOnly = True
                        CoverTo.ReadOnly = True
                        Measure.ReadOnly = True
                        Interval.ReadOnly = True
                    Case "27"
                        Measure.SelectedIndex = 0
                        Measure.ReadOnly = True
                    Case "MC", "MB", "MA", "OC", "ER", "CR"
                        CoverDate.Visible = False
                    Case Else
                        Exit Select
                End Select
            Else
                'in case the offer is registered and not issued

            End If
            'in case the offer is registered and not issued
            Select Case Request("Sys")
                Case "01", "04"
                    Measure.ReadOnly = True
                Case "OR", "27"
                    Measure.SelectedIndex = 0
                    Measure.ReadOnly = True
                Case "MC", "MB", "MA", "OC", "ER", "CR"
                    CoverDate.Visible = False
                Case Else
                    Exit Select
            End Select
        Else
            'in case the New Offer
            ''Select Case Request("Sys")
            ''    Case "01"
            ''        CoverFrom.MinDate = DateAdd(DateInterval.Day, 1, Today.Date)
            ''        CoverFrom.Value = Format(DateAdd(DateInterval.Day, 1, Today.Date), "yyyy/MM/dd")
            ''        Measure.ReadOnly = True
            ''    Case "OR", "27"
            ''        CoverFrom.MinDate = Today.Date
            ''        CoverFrom.Value = Format(Today.Date, "yyyy/MM/dd")
            ''        Measure.SelectedIndex = 0
            ''        Measure.ReadOnly = True
            ''    Case "04"
            ''        CoverDate.Visible = False
            ''    Case Else

            ''End Select

        End If
        'If Right(Session("Branch"), 2) = "00" Then
        '    Commissions.Visible = True
        'Else
        '    Commissions.Visible = False
        'End If
    End Sub

    Private Sub OrderNo_ValueChanged(sender As Object, e As EventArgs) Handles OrderNo.ValueChanged
        'Dim chcks As New DataSet
        'Using oCon As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
        '    If oCon.State = ConnectionState.Open Then
        '        oCon.Close()
        '    Else

        '    End If
        '    oCon.Open()
        '    Dim SrlAdptr6 = New SqlDataAdapter("Select PolicyFile.OrderNo From PolicyFile Left Join " & GetGroupFile(Request("Sys")).TrimEnd & " On PolicyFile.OrderNo=" & GetGroupFile(Request("Sys")).TrimEnd & ".OrderNo  Where PolicyFile.OrderNo='" & OrderNo.Value.ToString.TrimEnd & "'", oCon)
        '    SrlAdptr6.Fill(chcks)
        '    oCon.Close()
        'End Using
        'If chcks.Tables(0).Rows.Count = 0 Then
        '    'ERROR TOASTR
        'Else
        '    If CDbl(NETPRM.Value) + CDbl(TAXPRM.Value) + CDbl(CONPRM.Value) + CDbl(STMPRM.Value) + CDbl(ISSPRM.Value) = CDbl(TOTPRM.Value) And CDbl(TOTPRM.Value) <> 0 Then
        '        'SUCESS SAVED TOAST
        '    Else
        '        'ERROR IN NET PREMIUM
        '    End If
        'End If
    End Sub

    'Protected Sub CustNo_ItemsRequestedByFilterCondition(source As Object, e As ListEditItemsRequestedByFilterConditionEventArgs) Handles CustNo.ItemsRequestedByFilterCondition
    '    Dim comboBox As ASPxComboBox = DirectCast(source, ASPxComboBox)
    '    comboBox.ValueType = TypeOf (UInteger)
    '    SqlDataSource2.SelectCommand = "SELECT rtrim(CustName) As CustName, rtrim(CustNameE) As CustNameE, TelNo, CustNo FROM (select CustName,  CustNameE, TelNo, CustNo, row_number() over (order by t.CustNo desc) as [rn] from CustomerFile as t where (CustName LIKE @filter OR  CustNameE LIKE @filter  OR TelNo LIKE @filter )) as st where st.[rn] between @startIndex and @endIndex"
    '    SqlDataSource2.SelectParameters.Clear()
    '    SqlDataSource2.SelectParameters.Add("filter", TypeCode.String, String.Format("%{0}%", e.Filter))
    '    SqlDataSource2.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString())
    '    SqlDataSource2.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString())

    '    comboBox.DataSource = SqlDataSource2
    '    comboBox.DataBind()
    'End Sub

    'Protected Sub CustNo_ItemRequestedByValue(source As Object, e As ListEditItemRequestedByValueEventArgs) Handles CustNo.ItemRequestedByValue
    '    Dim value As Long = 0
    '    If e.Value Is Nothing OrElse Not Int64.TryParse(e.Value.ToString(), value) Then
    '        Return
    '    End If
    '    Dim comboBox As ASPxComboBox = DirectCast(source, ASPxComboBox)
    '    SqlDataSource2.SelectCommand = "SELECT rtrim(CustName) As CustName, rtrim(CustNameE) As CustNameE, TelNo, CustNo FROM CustomerFile WHERE (CustNo = @CustNo) ORDER BY CustNo desc"

    '    SqlDataSource2.SelectParameters.Clear()
    '    SqlDataSource2.SelectParameters.Add("CustNo", TypeCode.Int64, e.Value.ToString())
    '    comboBox.DataSource = SqlDataSource2
    '    comboBox.DataBind()
    'End Sub

    'Protected Sub CustNo_Callback(sender As Object, e As CallbackEventArgsBase)

    'End Sub
End Class