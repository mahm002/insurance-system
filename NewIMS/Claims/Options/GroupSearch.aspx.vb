Imports System.Data.SqlClient
Imports System.Web.UI

Partial Public Class _PolicyManagement_MOMOTOR_GroupSearch
    Inherits Page

    Private cmd As New SqlCommand
    Public dr As SqlDataReader

    Protected Sub BudyNo_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles BudyNo.TextChanged
        Dim dr As SqlDataReader
        If Not String.IsNullOrEmpty(BudyNo.Text) Then
            'Conn.Open()
            If Check.Checked = True Then
                Dim cmd As New SqlCommand("SELECT case when MOMOTORFILE.SubIns='02' then 'خصوصي' else 'تجاري' end as Type ,MainClaimFile.ClmNo, PolicyFile.PolNo, MOMOTORFILE.OrderNo, MOMOTORFILE.GroupNo, MOMOTORFILE.EndNo, MOMOTORFILE.Zone, MOMOTORFILE.tableNo " _
                     & "FROM MOMOTORFILE INNER JOIN PolicyFile ON MOMOTORFILE.OrderNo = PolicyFile.OrderNo AND MOMOTORFILE.EndNo = PolicyFile.EndNo AND MOMOTORFILE.LoadNo = PolicyFile.LoadNo " _
                     & "AND MOMOTORFILE.SubIns = PolicyFile.SubIns INNER JOIN MainClaimFile ON PolicyFile.PolNo = MainClaimFile.PolNo AND PolicyFile.EndNo = MainClaimFile.EndNo AND " _
                     & "PolicyFile.LoadNo = MainClaimFile.LoadNo And MOMOTORFILE.GroupNo = MainClaimFile.GroupNo And MOMOTORFILE.SubIns = MainClaimFile.SubIns And PolicyFile.SubIns = MainClaimFile.SubIns " _
                     & "WHERE (MOMOTORFILE.BudyNo = @Name) AND (MOMOTORFILE.Zone = 1) ORDER BY MOMOTORFILE.OrderNo DESC", Conn)
                cmd.Parameters.AddWithValue("@Name", BudyNo.Text)
                'cmd.Connection = Conn
                dr = cmd.ExecuteReader()
            Else
                Dim cmd As New SqlCommand("SELECT case when MOMOTORFILE.SubIns='02' then 'خصوصي' else 'تجاري' end as Type ,CustomerFile.CustName, PolicyFile.PolNo, MOMOTORFILE.EndNo, MOMOTORFILE.GroupNo, PolicyFile.CoverFrom, PolicyFile.CoverTo, MOMOTORFILE.OrderNo, MOMOTORFILE.Zone, " _
                   & "MOMOTORFILE.TableNo FROM MOMOTORFILE INNER JOIN PolicyFile ON MOMOTORFILE.OrderNo = PolicyFile.OrderNo AND MOMOTORFILE.EndNo = PolicyFile.EndNo AND " _
                   & "MOMOTORFILE.SubIns = PolicyFile.SubIns AND MOMOTORFILE.LoadNo = PolicyFile.LoadNo INNER JOIN CustomerFile ON PolicyFile.CustNo = CustomerFile.CustNo" _
                   & " WHERE BudyNo=@Name And Zone=1 ", Conn)
                'cmd.Connection = Conn
                cmd.Parameters.AddWithValue("@Name", BudyNo.Text)
                dr = cmd.ExecuteReader()
            End If

            'order by PolicyFile.OrderNo desc
            'Dim dr As SqlDataReader = cmd.ExecuteReader()
            If dr.HasRows Then
                GridView1.DataSource = dr
                GridView1.DataBind()
                checkusername.Visible = True
                imgstatus.ImageUrl = "~/Images/Icon_Available.gif"
                lblStatus.Text = "السيارة موجودة"
                Threading.Thread.Sleep(2000)
                dr.Close()
                'Conn.Close()
            Else
                GridView1.DataSource = dr
                GridView1.DataBind()
                checkusername.Visible = True
                imgstatus.ImageUrl = "~/Images/NotAvailable.jpg"
                lblStatus.Text = "السيارة غير موجودة"
                Threading.Thread.Sleep(2000)
                dr.Close()
                'Conn.Close()
            End If
        Else
            checkusername.Visible = False
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Conn.Open()
        HyperLink1.NavigateUrl = "../ClaimsManage/Default.aspx?sys=" & Request("sys")
    End Sub

    Protected Sub TableNo_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TableNo.TextChanged
        Dim dr1 As SqlDataReader
        If Not String.IsNullOrEmpty(TableNo.Text) Then
            'Conn.Open()
            If Check.Checked = True Then
                Dim cmd As New SqlCommand("SELECT case when MOMOTORFILE.SubIns='02' then 'خصوصي' else 'تجاري' end as Type ,MainClaimFile.ClmNo, PolicyFile.PolNo, MOMOTORFILE.OrderNo, MOMOTORFILE.GroupNo, MOMOTORFILE.EndNo, MOMOTORFILE.Zone, MOMOTORFILE.BudyNo " _
                       & "FROM MOMOTORFILE INNER JOIN PolicyFile ON MOMOTORFILE.OrderNo = PolicyFile.OrderNo AND MOMOTORFILE.EndNo = PolicyFile.EndNo AND MOMOTORFILE.LoadNo = PolicyFile.LoadNo " _
                       & "AND MOMOTORFILE.SubIns = PolicyFile.SubIns INNER JOIN MainClaimFile ON PolicyFile.PolNo = MainClaimFile.PolNo AND PolicyFile.EndNo = MainClaimFile.EndNo AND " _
                       & "PolicyFile.LoadNo = MainClaimFile.LoadNo And MOMOTORFILE.GroupNo = MainClaimFile.GroupNo And MOMOTORFILE.SubIns = MainClaimFile.SubIns And PolicyFile.SubIns = MainClaimFile.SubIns " _
                       & "WHERE (MOMOTORFILE.TableNo = @Name) AND (MOMOTORFILE.Zone = 1) ORDER BY MOMOTORFILE.OrderNo DESC", Conn)
                'And MOMOTORFILE.SubIns = MainClaimFile.SubIns And PolicyFile.SubIns = MainClaimFile.SubIns
                cmd.Parameters.AddWithValue("@Name", TableNo.Text)
                dr1 = cmd.ExecuteReader()
            Else
                Dim cmd As New SqlCommand("SELECT case when MOMOTORFILE.SubIns='02' then 'خصوصي' else 'تجاري' end as Type ,CustomerFile.CustName, PolicyFile.PolNo, MOMOTORFILE.EndNo, MOMOTORFILE.GroupNo, PolicyFile.CoverFrom, PolicyFile.CoverTo, MOMOTORFILE.OrderNo, MOMOTORFILE.Zone," _
            & "MOMOTORFILE.BudyNo FROM MOMOTORFILE INNER JOIN PolicyFile ON MOMOTORFILE.OrderNo = PolicyFile.OrderNo AND MOMOTORFILE.EndNo = PolicyFile.EndNo AND " _
            & "MOMOTORFILE.SubIns = PolicyFile.SubIns AND MOMOTORFILE.LoadNo = PolicyFile.LoadNo INNER JOIN CustomerFile ON PolicyFile.CustNo = CustomerFile.CustNo" _
            & " WHERE TableNo=@Name And Zone=1", Conn)
                cmd.Parameters.AddWithValue("@Name", TableNo.Text)
                dr1 = cmd.ExecuteReader()
            End If

            ' order by PolicyFile.OrderNo desc

            If dr1.HasRows Then
                GridView1.DataSource = dr1
                GridView1.DataBind()
                checkusername.Visible = True
                imgstatus.ImageUrl = "~/Images/Icon_Available.gif"
                lblStatus.Text = "السيارة موجودة"
                Threading.Thread.Sleep(2000)
                dr1.Close()
                'Conn.Close()
            Else
                GridView1.DataSource = dr1
                GridView1.DataBind()
                checkusername.Visible = True
                imgstatus.ImageUrl = "~/Images/NotAvailable.jpg"
                lblStatus.Text = "السيارة غير موجودة"
                Threading.Thread.Sleep(2000)
                dr1.Close()
                'Conn.Close()
            End If
        Else
            checkusername.Visible = False
        End If
    End Sub

    Protected Sub EngineNo_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles EngineNo.TextChanged
        If Not String.IsNullOrEmpty(EngineNo.Text) Then
            'Conn.Open()
            Dim cmd As New SqlCommand("SELECT  case when MOMOTORFILE.SubIns='02' then 'خصوصي' else 'تجاري' end as Type ,CustomerFile.CustName, PolicyFile.PolNo, MOMOTORFILE.EndNo, MOMOTORFILE.GroupNo, PolicyFile.CoverFrom, PolicyFile.CoverTo, MOMOTORFILE.OrderNo, MOMOTORFILE.Zone, " _
            & "MOMOTORFILE.TableNo FROM MOMOTORFILE INNER JOIN PolicyFile ON MOMOTORFILE.OrderNo = PolicyFile.OrderNo AND MOMOTORFILE.EndNo = PolicyFile.EndNo AND " _
            & "MOMOTORFILE.SubIns = PolicyFile.SubIns AND MOMOTORFILE.LoadNo = PolicyFile.LoadNo INNER JOIN CustomerFile ON PolicyFile.CustNo = CustomerFile.CustNo" _
            & " WHERE EngineNo=@Name And Zone=1", Conn)
            ' order by PolicyFile.OrderNo desc
            cmd.Parameters.AddWithValue("@Name", EngineNo.Text)
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            If dr.HasRows Then
                GridView1.DataSource = dr
                GridView1.DataBind()
                checkusername.Visible = True
                imgstatus.ImageUrl = "~/Images/Icon_Available.gif"
                lblStatus.Text = "السيارة موجودة"
                Threading.Thread.Sleep(2000)
                dr.Close()
                'Conn.Close()
            Else
                GridView1.DataSource = dr
                GridView1.DataBind()
                checkusername.Visible = True
                imgstatus.ImageUrl = "~/Images/NotAvailable.jpg"
                lblStatus.Text = "السيارة غير موجودة"
                Threading.Thread.Sleep(2000)
                dr.Close()
                'Conn.Close()
            End If
        Else
            checkusername.Visible = False
        End If
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Unload
        Conn.Close()
    End Sub

    Protected Sub Check_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Check.CheckedChanged
        BudyNo.Text = ""
        TableNo.Text = ""
        EngineNo.Text = ""
    End Sub

End Class