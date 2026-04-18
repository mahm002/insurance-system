Imports System.Web.Script.Services
Imports System.Web.Services
Imports DevExpress.Web

Partial Class Policy_Default_Modern
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))
        If IsNothing(Session("UserInfo")) Then
            FormsAuthentication.SignOut()
            ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            Exit Sub

            'End If
            'If myList Is Nothing Then
            '    FormsAuthentication.SignOut()
            '    'FormsAuthentication.RedirectToLoginPage()
            '    'ASPxWebControl.RedirectOnCallback(TARGET_URL)
            '    ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 1)
            'Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMS-DBConnectionString").ConnectionString)
            '    If con.State = ConnectionState.Open Then
            '        con.Close()
            '    Else

            '    End If
            '    con.Open()

            '    SqlDataSource.ConnectionString = con.ConnectionString

            '    con.Close()
            'End Using
        End If
        If Not IsPostBack Then
            ' Initialize hidden fields
            hdnUserID.Value = Session("UserID")
        End If
    End Sub

    ' ============================================
    ' 1. SIMPLE TEST METHOD - Start with this
    ' ============================================
    <WebMethod(EnableSession:=True)>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function TestConnection() As Object
        Try
            Return New With {
                .success = True,
                .message = "✅ الاتصال ناجح! الخادم يعمل بشكل صحيح.",
                .serverTime = Date.Now.ToString("yyyy-MM-dd HH:mm:ss")
            }
        Catch ex As Exception
            Return New With {
                .success = False,
                .message = "❌ خطأ: " & ex.Message
            }
        End Try
    End Function

    ' ============================================
    ' 2. SIMPLE DATA METHOD - Basic data retrieval
    ' ============================================
    <WebMethod(EnableSession:=True)>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function GetSimpleData(
        system As String,
        branch As String,
        searchText As String,
        pageNumber As Integer,
        pageSize As Integer) As Object

        Try
            ' Check session
            If HttpContext.Current.Session("UserID") Is Nothing Then
                Return New With {
                    .success = False,
                    .message = "انتهت جلسة العمل. الرجاء إعادة تسجيل الدخول."
                }
            End If

            ' Validate inputs
            If String.IsNullOrEmpty(system) Then system = "01"
            If String.IsNullOrEmpty(branch) Then branch = "01000"
            If pageNumber < 1 Then pageNumber = 1
            If pageSize < 1 Then pageSize = 10

            ' Calculate skip count
            Dim skipCount As Integer = (pageNumber - 1) * pageSize

            ' Create test data
            Dim testData As New List(Of Object)()
            Dim totalRecords As Integer = 50 ' Test total

            For i As Integer = 1 To pageSize
                Dim recordId As Integer = skipCount + i

                If recordId <= totalRecords Then
                    testData.Add(New With {
                        .OrderNo = "ORD" & recordId.ToString("0000"),
                        .CustName = "عميل تجريبي " & recordId,
                        .PolNo = "POL" & recordId.ToString("0000"),
                        .EndNo = "0",
                        .LoadNo = "0",
                        .IssuDate = DateTime.Now.AddDays(-recordId).ToString("yyyy-MM-dd"),
                        .EntryDate = DateTime.Now.AddDays(-recordId - 1).ToString("yyyy-MM-dd"),
                        .NETPRM = CDec(1000 * recordId),
                        .TOTPRM = CDec(1200 * recordId),
                        .TpName = "دينار",
                        .Payment = If(recordId Mod 2 = 0, "Paid", "Pending"),
                        .SubIns = system,
                        .Br = branch
                    })
                End If
            Next

            ' Apply search filter if provided
            Dim filteredData As List(Of Object) = testData
            If Not String.IsNullOrEmpty(searchText) Then
                filteredData = testData.Where(Function(item)
                                                  Return item.CustName.Contains(searchText) OrElse
                                                         item.PolNo.Contains(searchText) OrElse
                                                         item.OrderNo.Contains(searchText)
                                              End Function).ToList()
            End If

            Return New With {
                .success = True,
                .data = filteredData,
                .totalRecords = totalRecords,
                .message = "تم تحميل البيانات بنجاح",
                .searchText = searchText,
                .pageNumber = pageNumber,
                .pageSize = pageSize
            }
        Catch ex As Exception
            Return New With {
                .success = False,
                .message = "خطأ في تحميل البيانات: " & ex.Message,
                .stackTrace = ex.StackTrace
            }
        End Try
    End Function

    ' ============================================
    ' 3. HELPER FUNCTIONS - Add these gradually
    ' ============================================

    ' Safe string conversion
    Private Shared Function SafeString(value As Object) As String
        If value Is Nothing OrElse IsDBNull(value) Then
            Return String.Empty
        End If
        Return value.ToString()
    End Function

    ' Safe date conversion
    Private Shared Function SafeDate(value As Object) As String
        If value Is Nothing OrElse IsDBNull(value) Then
            Return String.Empty
        End If
        Try
            Return Convert.ToDateTime(value).ToString("yyyy-MM-dd")
        Catch
            Return String.Empty
        End Try
    End Function

    ' Safe decimal conversion
    Private Shared Function SafeDecimal(value As Object) As Decimal
        If value Is Nothing OrElse IsDBNull(value) Then
            Return 0
        End If
        Try
            Return Convert.ToDecimal(value)
        Catch
            Return 0
        End Try
    End Function

End Class