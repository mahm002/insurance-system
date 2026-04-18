' Helpers\ApiHelper.vb
Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports System.Configuration
Imports System.Security.Authentication
Imports System.Diagnostics
' PolicyPrintHelper.vb (in App_Code folder)
Imports System.Web

Public Class ApiHelper

    Private Shared ReadOnly _cookieContainer As New CookieContainer()
    Private ReadOnly _httpClient As HttpClient

    Public Sub New()
        Dim handler As New HttpClientHandler() With {
            .UseCookies = True,
            .CookieContainer = _cookieContainer,
            .SslProtocols = SslProtocols.Tls12
        }

        _httpClient = New HttpClient(handler)
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "InsuranceWebApp/1.0")
        _httpClient.Timeout = TimeSpan.FromSeconds(30)
    End Sub


    Public Class PolicyPrintHelper
        ' Method to print any policy from anywhere in the application
        Public Shared Sub PrintPolicy(policyNumber As String)
            If HttpContext.Current IsNot Nothing Then
                HttpContext.Current.Response.Redirect($"~/PrintPolicy.aspx?policy={HttpUtility.UrlEncode(policyNumber)}")
            End If
        End Sub

        ' Method to get PDF URL for embedding
        Public Shared Function GetPolicyPdfUrl(policyNumber As String) As String
            Return $"~/GetPolicyPdf.ashx?card={HttpUtility.UrlEncode(policyNumber)}"
        End Function

        ' Method to validate policy number format
        Public Shared Function IsValidPolicyNumber(policyNumber As String) As Boolean
            Return Not String.IsNullOrWhiteSpace(policyNumber) AndAlso policyNumber.Length >= 5
        End Function
    End Class
    Private Shared Function GetBaseUrl() As String
        Dim url As String = ConfigurationManager.AppSettings("ApiBaseUrl")
        If String.IsNullOrWhiteSpace(url) Then
            Throw New InvalidOperationException("Missing 'ApiBaseUrl' in Web.config")
        End If
        If Not url.EndsWith("/") Then url &= "/"
        Return url
    End Function

    Private Shared Function BuildUrl(endpoint As String) As String
        Dim baseUri As String = GetBaseUrl()
        If endpoint.StartsWith("/") Then endpoint = endpoint.Substring(1)
        Return baseUri & endpoint
    End Function

    Private Async Function GetLookupListAsync(endpoint As String) As Task(Of List(Of LookupItem))
        Try
            ' Use MultipartFormDataContent (as in Postman)
            Dim content As New MultipartFormDataContent()

            ' Add user_name field
            Dim userNameContent As New StringContent(ConfigurationManager.AppSettings("ApiUsername"))
            userNameContent.Headers.Add("Content-Disposition", "form-data; name=""user_name""")
            content.Add(userNameContent)

            ' Add pass_word field
            Dim passwordContent As New StringContent(ConfigurationManager.AppSettings("ApiPassword"))
            passwordContent.Headers.Add("Content-Disposition", "form-data; name=""pass_word""")
            content.Add(passwordContent)

            Dim url As String = BuildUrl(endpoint)

            ' ✅ LOG THE EXACT URL BEING CALLED
            Debug.WriteLine($"Calling URL: {url}")
            Debug.WriteLine($"Endpoint requested: {endpoint}")

            Dim response As HttpResponseMessage = Await _httpClient.PostAsync(url, content)

            ' ✅ LOG THE RESPONSE STATUS AND CONTENT TYPE
            Debug.WriteLine($"Response Status: {response.StatusCode}")
            Debug.WriteLine($"Content Type: {response.Content.Headers.ContentType}")

            If Not response.IsSuccessStatusCode Then
                Dim errorText As String = Await response.Content.ReadAsStringAsync()
                Debug.WriteLine($"Error Response: {errorText}")
                Throw New ApplicationException($"API Error ({response.StatusCode}) on {endpoint}: {errorText}")
            End If

            Dim json As String = Await response.Content.ReadAsStringAsync()
            Debug.WriteLine($"Response JSON length: {json.Length}")

            ' Deserialize to wrapper, then extract the data array
            Dim wrapper = JsonConvert.DeserializeObject(Of ApiResponse(Of List(Of LookupItem)))(json)

            If Not wrapper.status Then
                Throw New ApplicationException($"API returned status false: {wrapper.message}")
            End If

            Return wrapper.data
        Catch ex As TaskCanceledException
            Throw New ApplicationException($"Request timed out for {endpoint}. Check network or server.")
        Catch ex As Exception
            Debug.WriteLine($"Exception in GetLookupListAsync: {ex.Message}")
            Throw
        End Try
    End Function

    ' Add this new method to ApiHelper.vb
    Public Async Function GetClausesAsync() As Task(Of List(Of InsuranceClause))
        Try
            Dim content As New MultipartFormDataContent()

            Dim userNameContent As New StringContent(ConfigurationManager.AppSettings("ApiUsername"))
            userNameContent.Headers.Add("Content-Disposition", "form-data; name=""user_name""")
            content.Add(userNameContent)

            Dim passwordContent As New StringContent(ConfigurationManager.AppSettings("ApiPassword"))
            passwordContent.Headers.Add("Content-Disposition", "form-data; name=""pass_word""")
            content.Add(passwordContent)

            Dim url As String = BuildUrl("api/insuranceclause/all")
            Debug.WriteLine($"Calling: {url}")

            Dim response As HttpResponseMessage = Await _httpClient.PostAsync(url, content)

            If Not response.IsSuccessStatusCode Then
                Dim errorText As String = Await response.Content.ReadAsStringAsync()
                Throw New ApplicationException($"Clauses API Error ({response.StatusCode}): {errorText}")
            End If

            Dim json As String = Await response.Content.ReadAsStringAsync()
            Dim wrapper = JsonConvert.DeserializeObject(Of ApiResponse(Of List(Of InsuranceClause)))(json)

            If Not wrapper.status Then
                Throw New ApplicationException($"Clauses API returned status false: {wrapper.message}")
            End If

            Return wrapper.data
        Catch ex As Exception
            Throw
        End Try
    End Function

    ' ───────────────────────────────────────────────
    ' PUBLIC METHODS FOR DROPDOWN DATA
    ' ───────────────────────────────────────────────
    ' If it's /api/cars (not /api/cars/all)
    Public Async Function GetCarTypesAsync() As Task(Of List(Of LookupItem))
        Return Await GetLookupListAsync("api/cars/all")  ' ✅ Changed from "api/cars/all"
    End Function

    Public Async Function GetVehicleNationalitiesAsync() As Task(Of List(Of LookupItem))
        Return Await GetLookupListAsync("api/vehiclenationality/all")  ' ✅ Changed
    End Function

    Public Async Function GetInsuranceCountriesAsync() As Task(Of List(Of LookupItem))
        Return Await GetLookupListAsync("api/countries/all")  ' ✅ Changed
    End Function

    'Public Async Function GetClausesAsync() As Task(Of List(Of LookupItem))
    '    Return Await GetLookupListAsync("api/insuranceclause/all")  ' ✅ Changed
    'End Function

    ' ───────────────────────────────────────────────
    ' POLICY CREATION - FIXED TO USE FORMDATA
    ' ───────────────────────────────────────────────

    ''' <summary>
    ''' Creates insurance policy. This endpoint expects FormData (multipart/form-data) with credentials.
    ''' </summary>
    Public Async Function CreateInsurancePolicyAsync(policyData As Object) As Task(Of ApiResponse)
        Try
            ' --- CRITICAL FIX: Use MultipartFormDataContent instead of JSON ---
            Dim content As New MultipartFormDataContent()

            ' Add credentials first
            Dim userNameContent As New StringContent(ConfigurationManager.AppSettings("ApiUsername"))
            userNameContent.Headers.Add("Content-Disposition", "form-data; name=""user_name""")
            content.Add(userNameContent)

            Dim passwordContent As New StringContent(ConfigurationManager.AppSettings("ApiPassword"))
            passwordContent.Headers.Add("Content-Disposition", "form-data; name=""pass_word""")
            content.Add(passwordContent)

            ' Add policy data fields one by one
            Dim insuranceNameContent As New StringContent(CType(policyData.insurance_name, String))
            insuranceNameContent.Headers.Add("Content-Disposition", "form-data; name=""insurance_name""")
            content.Add(insuranceNameContent)

            Dim insuranceLocationContent As New StringContent(CType(policyData.insurance_location, String))
            insuranceLocationContent.Headers.Add("Content-Disposition", "form-data; name=""insurance_location""")
            content.Add(insuranceLocationContent)

            Dim insurancePhoneContent As New StringContent(CType(policyData.insurance_phone, String))
            insurancePhoneContent.Headers.Add("Content-Disposition", "form-data; name=""insurance_phone""")
            content.Add(insurancePhoneContent)

            Dim chassisNumberContent As New StringContent(CType(policyData.chassis_number, String))
            chassisNumberContent.Headers.Add("Content-Disposition", "form-data; name=""chassis_number""")
            content.Add(chassisNumberContent)

            Dim motorNumberContent As New StringContent(CType(policyData.motor_number, String))
            motorNumberContent.Headers.Add("Content-Disposition", "form-data; name=""motor_number""")
            content.Add(motorNumberContent)

            Dim plateNumberContent As New StringContent(CType(policyData.plate_number, String))
            plateNumberContent.Headers.Add("Content-Disposition", "form-data; name=""plate_number""")
            content.Add(plateNumberContent)

            Dim carMadeDateContent As New StringContent(CType(policyData.car_made_date, String))
            carMadeDateContent.Headers.Add("Content-Disposition", "form-data; name=""car_made_date""")
            content.Add(carMadeDateContent)

            Dim carsIdContent As New StringContent(policyData.cars_id.ToString())
            carsIdContent.Headers.Add("Content-Disposition", "form-data; name=""cars_id""")
            content.Add(carsIdContent)

            Dim vehicleNatsIdContent As New StringContent(policyData.vehicle_nationalities_id.ToString())
            vehicleNatsIdContent.Headers.Add("Content-Disposition", "form-data; name=""vehicle_nationalities_id""")
            content.Add(vehicleNatsIdContent)

            Dim insuranceFromContent As New StringContent(CType(policyData.insurance_day_from, String))
            insuranceFromContent.Headers.Add("Content-Disposition", "form-data; name=""insurance_day_from""")
            content.Add(insuranceFromContent)

            Dim insuranceDaysContent As New StringContent(policyData.insurance_days_number.ToString())
            insuranceDaysContent.Headers.Add("Content-Disposition", "form-data; name=""insurance_days_number""")
            content.Add(insuranceDaysContent)

            Dim insuranceClausesIdContent As New StringContent(policyData.insurance_clauses_id.ToString())
            insuranceClausesIdContent.Headers.Add("Content-Disposition", "form-data; name=""insurance_clauses_id""")
            content.Add(insuranceClausesIdContent)

            Dim insuranceCountryNumContent As New StringContent(policyData.insurance_country_number.ToString())
            insuranceCountryNumContent.Headers.Add("Content-Disposition", "form-data; name=""insurance_country_number""")
            content.Add(insuranceCountryNumContent)

            Dim countriesIdContent As New StringContent(policyData.countries_id.ToString())
            countriesIdContent.Headers.Add("Content-Disposition", "form-data; name=""countries_id""")
            content.Add(countriesIdContent)

            Dim url As String = BuildUrl("api/insurance/create")

            ' ✅ LOG THE REQUEST DETAILS
            Debug.WriteLine($"API Call: POST {url}")
            Debug.WriteLine($"Request Content-Type: multipart/form-data")

            Dim response As HttpResponseMessage = Await _httpClient.PostAsync(url, content)

            ' ✅ LOG THE RESPONSE DETAILS
            Debug.WriteLine($"Response Status: {response.StatusCode}")
            Debug.WriteLine($"Response Headers: {String.Join(", ", response.Headers.Select(Function(h) $"{h.Key}={String.Join(";", h.Value)}"))}")

            If Not response.IsSuccessStatusCode Then
                Dim errorText As String = Await response.Content.ReadAsStringAsync()
                'Debug.WriteLine($"Error Response Body: {errorText}")
                Throw New HttpRequestException($"API Error ({response.StatusCode}): {errorText}")
            End If

            Dim responseJson As String = Await response.Content.ReadAsStringAsync()
            'Debug.WriteLine($"Success Response Body: {responseJson}")

            ' Deserialize the complete response
            Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse)(responseJson)

            ' Check if the operation was successful according to API
            If apiResponse.code <> 1 OrElse Not apiResponse.status Then
                Throw New ApplicationException($"API Operation Failed: {apiResponse.message}")
            End If

            Return apiResponse

        Catch ex As TaskCanceledException
            Throw New TimeoutException("Request timed out. Check network or server.", ex)
        Catch ex As HttpRequestException
            Throw
        Catch ex As Exception
            Debug.WriteLine($"General Exception in CreateInsurancePolicyAsync: {ex.Message}")
            Throw
        End Try
    End Function

End Class
' Add response wrapper classes to match your API
Public Class ApiResponse
    Public Property code As Integer
    Public Property status As Boolean
    Public Property policyNumber As String
    Public Property message As String
    Public Property data As PolicyData
End Class

Public Class PolicyData
    Public Property id As Integer
    Public Property issuing_date As String
    Public Property insurance_name As String
    Public Property insurance_location As String
    Public Property insurance_phone As String
    Public Property motor_number As String
    Public Property plate_number As String
    Public Property chassis_number As String
    Public Property car_made_date As String
    Public Property cars_id As Integer
    Public Property vehicle_nationalities_id As Integer
    Public Property insurance_day_from As String
    Public Property insurance_days_number As Integer
    Public Property nsurance_day_to As String
    Public Property insurance_country_number As Integer
    Public Property insurance_installment_daily As String
    Public Property insurance_installment As String
    Public Property insurance_supervision As String
    Public Property insurance_tax As String
    Public Property insurance_version As String
    Public Property insurance_stamp As String
    Public Property insurance_total As String
    Public Property insurance_clauses_id As String
    Public Property countries_id As Integer
    Public Property companies_id As Integer
    Public Property company_users_id As Integer
    Public Property cards As Object
    Public Property vehicle_nationalities As VehicleNationality
    Public Property companies As Company
    Public Property company_users As CompanyUser
    Public Property cars As Car
    Public Property countries As Country
End Class

Public Class VehicleNationality
    Public Property id As Integer
    Public Property name As String
    Public Property symbol As String
    Public Property active As Integer
    Public Property created_at As String
End Class

Public Class Company
    Public Property id As Integer
    Public Property name As String
    Public Property phonenumber As String
    Public Property code As String
    Public Property fullname_manger As String
    Public Property phonenumber_manger As String
    Public Property address As String
    Public Property email As String
    Public Property website As String
    Public Property logo As String
    Public Property cities_id As Object
    Public Property regions_id As Object
    Public Property created_at As String
    Public Property active As Integer
End Class

Public Class CompanyUser
    Public Property id As Integer
    Public Property fullname As String
    Public Property username As String
    Public Property email As String
    Public Property active As Integer
    Public Property created_at As String
    Public Property companies_id As Integer
    Public Property user_type_id As Integer
End Class

Public Class Car
    Public Property id As Integer
    Public Property name As String
    Public Property symbol As String
    Public Property active As Integer
    Public Property created_at As String
End Class

Public Class Country
    Public Property id As Integer
    Public Property name As String
    Public Property symbol As String
    Public Property active As Integer
    Public Property created_at As String
End Class
