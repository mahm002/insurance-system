Imports System
Imports System.IdentityModel.Tokens.Jwt
Imports System.Security.Claims
Imports Microsoft.IdentityModel.Tokens
Imports System.Text

Public Class JwtHelper
    Private Shared ReadOnly SecretKey As String = ConfigurationManager.AppSettings("JwtSecret") ' e.g., "your-very-long-secret-key-here"
    Private Shared ReadOnly Issuer As String = ConfigurationManager.AppSettings("JwtIssuer") ' e.g., "YourAppName"
    Private Shared ReadOnly Audience As String = ConfigurationManager.AppSettings("JwtAudience") ' e.g., "YourAppAudience"

    Public Shared Function GenerateToken(userData As String(), expiresInMinutes As Integer) As String
        ' Add all your LogInfo items as claims
        Dim claims As New List(Of Claim) From {
            New Claim("AccountLogIn", userData(0)),
            New Claim("AccountPermSys", userData(1)),
            New Claim("AccountPermClm", userData(2)),
            New Claim("AccountPermFin", userData(3)),
            New Claim("AccountPermRe", userData(4)),
            New Claim("AccountPermMan", userData(5)),
            New Claim("AccountSysManag", userData(6)),
            New Claim("Branch", userData(7)),
            New Claim("AccountName", userData(8)),
            New Claim("AccountNo", userData(9)),
            New Claim("AccLimit", userData(10))
        }
        ' Add more as needed

        Dim key As New SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey))
        Dim creds As New SigningCredentials(key, SecurityAlgorithms.HmacSha256)

        Dim token = New JwtSecurityToken(
            issuer:=Issuer,
            audience:=Audience,
            claims:=claims,
            expires:=DateTime.Now.AddMinutes(expiresInMinutes),
            signingCredentials:=creds
        )

        Return New JwtSecurityTokenHandler().WriteToken(token)
    End Function

    Public Shared Function ValidateToken(token As String) As ClaimsPrincipal
        Dim tokenHandler = New JwtSecurityTokenHandler()
        Dim validationParameters = New TokenValidationParameters() With {
            .ValidateIssuer = True,
            .ValidIssuer = Issuer,
            .ValidateAudience = True,
            .ValidAudience = Audience,
            .ValidateLifetime = True,
            .IssuerSigningKey = New SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
            .ClockSkew = TimeSpan.Zero
        }

        Try
            Dim principal = tokenHandler.ValidateToken(token, validationParameters, Nothing)
            Return principal
        Catch
            Return Nothing
        End Try
    End Function
End Class