' ModernForm.aspx.vb
Partial Class TravelForm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Set default values
            txtCoverageTo.Text = "2026-02-08"
            txtCoverageDays.Text = "10"
            txtCoverageFrom.Text = DateTime.Now.ToString("yyyy-MM-dd")
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Page.IsValid Then
            Try
                ' Save data to database
                SaveCustomerData()

                ' Show success message
                lblMessage.Text = "تم حفظ بيانات الزبون بنجاح!"
                divMessage.Visible = True

                ' You could redirect or clear form here
                ' ClearForm()

            Catch ex As Exception
                lblMessage.Text = "حدث خطأ أثناء الحفظ: " & ex.Message
                'divMessage.CssClass = "alert alert-danger alert-dismissible fade show"
                divMessage.Visible = True
            End Try
        End If
    End Sub

    Protected Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearForm()
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        If txtSearch.Text.Length >= 3 Then
            ' Search for customer in database
            SearchCustomer(txtSearch.Text)
        Else
            lblMessage.Text = "الرجاء إدخال 3 أحرف على الأقل للبحث"
            'divMessage.CssClass = "alert alert-warning alert-dismissible fade show"
            divMessage.Visible = True
        End If
    End Sub

    Protected Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        ' Print functionality
        ClientScript.RegisterStartupScript(Me.GetType(), "Print", "window.print();", True)
    End Sub

    Private Sub SaveCustomerData()
        ' Implement your database saving logic here
        ' Example using parameters to prevent SQL injection

        ' Dim connectionString As String = ConfigurationManager.ConnectionStrings("YourConnectionString").ConnectionString
        ' Using conn As New SqlConnection(connectionString)
        '     Using cmd As New SqlCommand("INSERT INTO Customers (...) VALUES (...)", conn)
        '         cmd.Parameters.AddWithValue("@CertificateNo", txtCertificateNo.Text)
        '         ' ... add other parameters
        '         conn.Open()
        '         cmd.ExecuteNonQuery()
        '     End Using
        ' End Using
    End Sub

    Private Sub SearchCustomer(searchTerm As String)
        ' Implement your search logic here
        ' This is a placeholder - implement actual database search

        ' Example: Populate fields with found customer data
        txtCustomerName.Text = "اسم الزبون الموجد"
        txtEnglishName.Text = "Customer Name Found"
        txtPhone.Text = "912345678"
        ' ... etc
    End Sub

    Private Sub ClearForm()
        ' Clear all form fields
        txtCertificateNo.Text = String.Empty
        txtFileNo.Text = String.Empty
        txtSearch.Text = String.Empty
        txtCustomerName.Text = String.Empty
        txtEnglishName.Text = String.Empty
        txtPhone.Text = String.Empty
        txtBirthDate.Text = String.Empty
        ddlGender.SelectedIndex = 0
        txtNationality.Text = String.Empty
        txtAddress.Text = String.Empty
        ddlCoverageArea.SelectedIndex = 0
        txtCoverageFrom.Text = DateTime.Now.ToString("yyyy-MM-dd")
        txtCoverageTo.Text = "2026-02-08"
        txtCoverageDays.Text = "10"
        txtSection.Text = String.Empty
        txtIssueValid.Text = String.Empty

        divMessage.Visible = False
    End Sub

    ' Validation methods
    Protected Sub ValidateCertificateNo(sender As Object, e As ServerValidateEventArgs)
        e.IsValid = Not String.IsNullOrEmpty(txtCertificateNo.Text)
    End Sub

    Protected Sub ValidateCustomerName(sender As Object, e As ServerValidateEventArgs)
        e.IsValid = Not String.IsNullOrEmpty(txtCustomerName.Text)
    End Sub
End Class